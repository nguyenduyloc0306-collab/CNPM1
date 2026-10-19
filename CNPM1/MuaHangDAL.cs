using System;
using System.Data;

namespace CNPM1
{
    /// <summary>
    /// Mua ngay một sản phẩm (nút "Mua" trong frmMuaHang), không đi qua giỏ hàng.
    /// Quy tắc khuyến mãi giống KhuyenMaiDAL.SanPhamKhuyenMai và GioHangDAL.ThanhToan.
    /// </summary>
    public static class MuaHangDAL
    {
        // Một dòng duy nhất cho sản phẩm @sp với số lượng @sl, kèm khuyến mãi tốt nhất đang hiệu lực.
        // Cột: MaSP, SoLuong, GiaBan, MaKhuyenMai (có thể NULL), GiamMoiSP (số tiền giảm cho 1 sản phẩm)
        private const string SqlDongSanPham =
            @"SELECT s.MaSP, @sl AS SoLuong, s.GiaBan, k.MaKhuyenMai,
                     CAST(CASE WHEN k.MaKhuyenMai IS NULL THEN 0
                               WHEN k.LoaiGiam = N'PhanTram' THEN s.GiaBan * k.GiaTriGiam / 100
                               WHEN k.GiaTriGiam > s.GiaBan THEN s.GiaBan
                               ELSE k.GiaTriGiam END AS DECIMAL(18,2)) AS GiamMoiSP
              FROM SanPham s
              OUTER APPLY (
                  SELECT TOP 1 km.MaKhuyenMai, km.LoaiGiam, km.GiaTriGiam
                  FROM KhuyenMai km
                  WHERE km.TrangThai = 1
                    AND CAST(GETDATE() AS DATE) BETWEEN CAST(km.NgayBatDau AS DATE)
                                                    AND CAST(km.NgayKetThuc AS DATE)
                  ORDER BY CASE km.LoaiGiam WHEN N'PhanTram' THEN s.GiaBan * km.GiaTriGiam / 100
                                            ELSE km.GiaTriGiam END DESC
              ) k
              WHERE s.MaSP = @sp AND s.TrangThai = 1";

        /// <summary>
        /// Thông tin hiện tại của sản phẩm còn bán: MaSP, TenSP, GiaBan (decimal), SoLuongTon (int).
        /// Trả về null nếu sản phẩm không tồn tại hoặc đã ngừng bán.
        /// </summary>
        public static DataRow ThongTin(int maSP)
        {
            DataTable dt = DbHelper.ExecuteQuery(
                "SELECT MaSP, TenSP, GiaBan, SoLuongTon FROM SanPham WHERE MaSP = @sp AND TrangThai = 1",
                DbHelper.Param("@sp", maSP));
            return dt.Rows.Count == 1 ? dt.Rows[0] : null;
        }

        /// <summary>Tiền giảm giá nếu mua soLuong sản phẩm này ngay bây giờ.</summary>
        public static decimal TinhGiamGia(int maSP, int soLuong)
        {
            object r = DbHelper.ExecuteScalar(
                "SELECT ISNULL(SUM(d.GiamMoiSP * d.SoLuong), 0) FROM (" + SqlDongSanPham + ") d",
                DbHelper.Param("@sp", maSP),
                DbHelper.Param("@sl", soLuong));
            return Convert.ToDecimal(r);
        }

        /// <summary>
        /// Mua ngay trong MỘT transaction: kiểm tra tồn kho, trừ kho, tạo HoaDon (MaNhanVien = NULL vì khách tự mua)
        /// và ChiTietHoaDon (đơn giá = GiaBan hiện tại), áp khuyến mãi. Trả về MaHoaDon.
        /// Lỗi nghiệp vụ (hết hàng, ngừng bán...) ném InvalidOperationException và rollback toàn bộ.
        /// hinhThuc chỉ nhận "TienMat" hoặc "ChuyenKhoan".
        /// </summary>
        public static int MuaNgay(int maKhachHang, int maSP, int soLuong, string hinhThuc)
        {
            if (soLuong < 1)
                throw new ArgumentException("Số lượng phải từ 1 trở lên.");
            if (hinhThuc != "TienMat" && hinhThuc != "ChuyenKhoan")
                throw new ArgumentException("Hình thức thanh toán không hợp lệ.");

            // Một khối T-SQL duy nhất (bảng tạm #Dong chỉ sống trong phạm vi một lần gọi có tham số).
            // Trả về chuỗi: "OK|<MaHoaDon>" nếu thành công, ngược lại là thông báo lỗi nghiệp vụ.
            string sql =
                @"SET NOCOUNT ON;
                  SELECT * INTO #Dong FROM (" + SqlDongSanPham + @") x;

                  DECLARE @soDong INT, @ton INT, @daTru INT, @hd INT;

                  SELECT @soDong = COUNT(*) FROM #Dong;
                  IF @soDong = 0
                  BEGIN
                      SELECT N'Sản phẩm không tồn tại hoặc đã ngừng bán.';
                      RETURN;
                  END

                  SELECT @ton = s.SoLuongTon
                  FROM #Dong d JOIN SanPham s ON s.MaSP = d.MaSP
                  WHERE d.SoLuong > s.SoLuongTon;
                  IF @ton IS NOT NULL
                  BEGIN
                      SELECT N'Không đủ tồn kho. Hiện chỉ còn ' + CAST(@ton AS NVARCHAR(10)) + N' sản phẩm.';
                      RETURN;
                  END

                  UPDATE s SET s.SoLuongTon = s.SoLuongTon - d.SoLuong
                  FROM SanPham s JOIN #Dong d ON d.MaSP = s.MaSP
                  WHERE s.SoLuongTon >= d.SoLuong;
                  SET @daTru = @@ROWCOUNT;
                  IF @daTru <> @soDong
                  BEGIN
                      SELECT N'Tồn kho vừa thay đổi, sản phẩm không còn đủ hàng. Vui lòng thử lại.';
                      RETURN;   -- phía C# sẽ rollback
                  END

                  INSERT INTO HoaDon (MaNhanVien, MaKhachHang, MaKhuyenMai, HinhThucThanhToan, TienGiamGia)
                  VALUES (NULL, @kh,
                          (SELECT TOP 1 MaKhuyenMai FROM #Dong WHERE MaKhuyenMai IS NOT NULL),
                          @ht,
                          (SELECT ISNULL(SUM(GiamMoiSP * SoLuong), 0) FROM #Dong));
                  SET @hd = CAST(SCOPE_IDENTITY() AS INT);

                  INSERT INTO ChiTietHoaDon (MaHoaDon, MaSP, SoLuong, DonGia)
                  SELECT @hd, MaSP, SoLuong, GiaBan FROM #Dong;

                  UPDATE HoaDon
                  SET TongTienHang = (SELECT SUM(ThanhTien) FROM ChiTietHoaDon WHERE MaHoaDon = @hd)
                  WHERE MaHoaDon = @hd;

                  SELECT N'OK|' + CAST(@hd AS NVARCHAR(20));";

            int maHD = 0;
            DbHelper.ExecuteTransaction((conn, tran) =>
            {
                string kq = Convert.ToString(DbHelper.ExecuteScalar(conn, tran, sql,
                    DbHelper.Param("@kh", maKhachHang),
                    DbHelper.Param("@sp", maSP),
                    DbHelper.Param("@sl", soLuong),
                    DbHelper.Param("@ht", hinhThuc)));

                if (kq != null && kq.StartsWith("OK|"))
                    maHD = int.Parse(kq.Substring(3));
                else
                    throw new InvalidOperationException(kq); // lỗi nghiệp vụ: ExecuteTransaction sẽ rollback
            });
            return maHD;
        }

        // Thoát các ký tự đặc biệt của LIKE (% _ [) để khách gõ gì cũng được hiểu là chữ thường
        private static string ThoatLike(string s)
        {
            return (s ?? "").Replace("[", "[[]").Replace("%", "[%]").Replace("_", "[_]");
        }

        /// <summary>
        /// Tìm sản phẩm đang bán theo tên, danh mục và khoảng giá bán. Bỏ trống một điều kiện nghĩa là
        /// không lọc theo điều kiện đó (giaTu / giaDen là null = không giới hạn). Tên và danh mục khớp một phần.
        /// Cột (giống lưới frmMuaHang): MaSP, TenSP, GiaBan (decimal), TenDanhMuc.
        /// </summary>
        public static DataTable TimKiemSanPham(string tenSP, string danhMuc, decimal? giaTu, decimal? giaDen)
        {
            return DbHelper.ExecuteQuery(
                @"SELECT s.MaSP, s.TenSP, s.GiaBan, d.TenDanhMuc
                  FROM SanPham s JOIN DanhMuc d ON d.MaDanhMuc = s.MaDanhMuc
                  WHERE s.TrangThai = 1
                    AND s.TenSP LIKE @ten
                    AND d.TenDanhMuc LIKE @dm
                    AND (@tu  IS NULL OR s.GiaBan >= @tu)
                    AND (@den IS NULL OR s.GiaBan <= @den)
                  ORDER BY s.MaSP",
                DbHelper.Param("@ten", "%" + ThoatLike(tenSP) + "%"),
                DbHelper.Param("@dm", "%" + ThoatLike(danhMuc) + "%"),
                DbHelper.Param("@tu", giaTu),
                DbHelper.Param("@den", giaDen));
        }
    }
}