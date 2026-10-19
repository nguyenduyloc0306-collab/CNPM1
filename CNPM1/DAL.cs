using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;

namespace CNPM1
{
    /* =====================================================================
       Lớp truy cập dữ liệu, bám sát schema CSDL CNPM (SQL Server).
       Quy ước: nút "Xóa" trên form = KHÓA bản ghi (TrangThai = 0), vì các bảng
       có khóa ngoại tới hóa đơn / phiếu nhập nên không xóa cứng được.
       ===================================================================== */

    // ------------------------------------------------------------------
    // QLTK - Tài khoản
    // ------------------------------------------------------------------
    public static class TaiKhoanDAL
    {
        /// <summary>Trả về 1 dòng (MaTK, HoTen, MaVaiTro, TenVaiTro) nếu đăng nhập đúng; null nếu sai hoặc bị khóa.</summary>
        public static DataRow DangNhap(string tenDangNhap, string matKhau)
        {
            DataTable dt = DbHelper.ExecuteQuery(
                @"SELECT t.MaTK, t.HoTen, t.MaVaiTro, v.TenVaiTro, t.NgaySinh, t.SoDienThoai, t.DiaChi, t.Email
                  FROM TaiKhoan t JOIN VaiTro v ON v.MaVaiTro = t.MaVaiTro
                  WHERE t.TenDangNhap = @u AND t.MatKhau = @p AND t.TrangThai = 1",
                DbHelper.Param("@u", tenDangNhap),
                DbHelper.Param("@p", DbHelper.HashPassword(matKhau)));
            return dt.Rows.Count == 1 ? dt.Rows[0] : null;
        }

        // Cột trả về: MaTK, TenDangNhap, HoTen, LoaiNguoiDung, TenVaiTro
        public static DataTable TimKiem(string tuKhoa)
        {
            return DbHelper.ExecuteQuery(
                @"SELECT t.MaTK, t.TenDangNhap, t.HoTen,
                         CASE WHEN t.MaVaiTro = 5 THEN N'Khách hàng' ELSE N'Nhân viên' END AS LoaiNguoiDung,
                         v.TenVaiTro
                  FROM TaiKhoan t JOIN VaiTro v ON v.MaVaiTro = t.MaVaiTro
                  WHERE t.TrangThai = 1
                    AND (CAST(t.MaTK AS VARCHAR(10)) LIKE @k OR t.HoTen LIKE @k OR t.TenDangNhap LIKE @k)
                  ORDER BY t.MaTK",
                DbHelper.Param("@k", "%" + (tuKhoa ?? "") + "%"));
        }

        public static DataTable LayVaiTro()
        {
            return DbHelper.ExecuteQuery("SELECT MaVaiTro, TenVaiTro FROM VaiTro ORDER BY MaVaiTro");
        }

        /// <summary>Thêm tài khoản, trả về MaTK mới. Mật khẩu được băm trước khi lưu.</summary>
        public static int Them(string tenDangNhap, string matKhau, string hoTen, int maVaiTro)
        {
            object id = DbHelper.ExecuteScalar(
                @"INSERT INTO TaiKhoan (TenDangNhap, MatKhau, HoTen, MaVaiTro)
                  VALUES (@u, @p, @ten, @vt);
                  SELECT CAST(SCOPE_IDENTITY() AS INT);",
                DbHelper.Param("@u", tenDangNhap),
                DbHelper.Param("@p", DbHelper.HashPassword(matKhau)),
                DbHelper.Param("@ten", hoTen),
                DbHelper.Param("@vt", maVaiTro));
            return Convert.ToInt32(id);
        }

        /// <summary>Cập nhật thông tin; chỉ đổi mật khẩu khi matKhauMoi không rỗng.</summary>
        public static int Sua(int maTK, string tenDangNhap, string hoTen, int maVaiTro, string matKhauMoi)
        {
            bool doiMK = !string.IsNullOrEmpty(matKhauMoi);
            string sql = "UPDATE TaiKhoan SET TenDangNhap=@u, HoTen=@ten, MaVaiTro=@vt"
                       + (doiMK ? ", MatKhau=@p" : "") + " WHERE MaTK=@id";
            List<SqlParameter> ps = new List<SqlParameter>
            {
                DbHelper.Param("@u", tenDangNhap), DbHelper.Param("@ten", hoTen),
                DbHelper.Param("@vt", maVaiTro),   DbHelper.Param("@id", maTK)
            };
            if (doiMK) ps.Add(DbHelper.Param("@p", DbHelper.HashPassword(matKhauMoi)));
            return DbHelper.ExecuteNonQuery(sql, ps.ToArray());
        }

        public static int Khoa(int maTK)
        {
            return DbHelper.ExecuteNonQuery("UPDATE TaiKhoan SET TrangThai = 0 WHERE MaTK = @id",
                DbHelper.Param("@id", maTK));
        }
    }

    // ------------------------------------------------------------------
    // QLDM - Danh mục
    // ------------------------------------------------------------------
    public static class DanhMucDAL
    {
        // Cột: MaDanhMuc, TenDanhMuc, MoTa
        public static DataTable TimKiem(string tuKhoa)
        {
            return DbHelper.ExecuteQuery(
                @"SELECT MaDanhMuc, TenDanhMuc, MoTa FROM DanhMuc
                  WHERE TrangThai = 1
                    AND (CAST(MaDanhMuc AS VARCHAR(10)) LIKE @k OR TenDanhMuc LIKE @k)
                  ORDER BY MaDanhMuc",
                DbHelper.Param("@k", "%" + (tuKhoa ?? "") + "%"));
        }

        public static int Them(string ten, string moTa)
        {
            object id = DbHelper.ExecuteScalar(
                @"INSERT INTO DanhMuc (TenDanhMuc, MoTa) VALUES (@t, @m);
                  SELECT CAST(SCOPE_IDENTITY() AS INT);",
                DbHelper.Param("@t", ten), DbHelper.Param("@m", moTa));
            return Convert.ToInt32(id);
        }

        public static int Sua(int ma, string ten, string moTa)
        {
            return DbHelper.ExecuteNonQuery(
                "UPDATE DanhMuc SET TenDanhMuc=@t, MoTa=@m WHERE MaDanhMuc=@id",
                DbHelper.Param("@t", ten), DbHelper.Param("@m", moTa), DbHelper.Param("@id", ma));
        }

        public static int Khoa(int ma)
        {
            return DbHelper.ExecuteNonQuery("UPDATE DanhMuc SET TrangThai = 0 WHERE MaDanhMuc = @id",
                DbHelper.Param("@id", ma));
        }
    }

    // ------------------------------------------------------------------
    // QLSP - Sản phẩm
    // ------------------------------------------------------------------
    public static class SanPhamDAL
    {
        // Cột: MaSP, TenSP, GiaBan, TenDanhMuc, DanhSachNCC (vd "1, 3")
        public static DataTable TimKiem(string tuKhoa)
        {
            return DbHelper.ExecuteQuery(
                @"SELECT s.MaSP, s.TenSP, s.GiaBan, d.TenDanhMuc,
                         ISNULL(STUFF((SELECT ', ' + CAST(n.MaNCC AS VARCHAR(10))
                                       FROM NhaCungCap_SanPham n
                                       WHERE n.MaSP = s.MaSP AND n.TrangThai = 1
                                       FOR XML PATH('')), 1, 2, ''), '') AS DanhSachNCC
                  FROM SanPham s JOIN DanhMuc d ON d.MaDanhMuc = s.MaDanhMuc
                  WHERE s.TrangThai = 1
                    AND (CAST(s.MaSP AS VARCHAR(10)) LIKE @k OR s.TenSP LIKE @k)
                  ORDER BY s.MaSP",
                DbHelper.Param("@k", "%" + (tuKhoa ?? "") + "%"));
        }

        /// <summary>Thêm sản phẩm (và liên kết NCC nếu có) trong một transaction. Trả về MaSP mới.</summary>
        public static int Them(string ten, int maDanhMuc, decimal giaNhap, decimal giaBan, int? maNCC)
        {
            int maSP = 0;
            DbHelper.ExecuteTransaction((conn, tran) =>
            {
                maSP = Convert.ToInt32(DbHelper.ExecuteScalar(conn, tran,
                    @"INSERT INTO SanPham (TenSP, MaDanhMuc, GiaNhap, GiaBan) VALUES (@t, @dm, @gn, @gb);
                      SELECT CAST(SCOPE_IDENTITY() AS INT);",
                    DbHelper.Param("@t", ten), DbHelper.Param("@dm", maDanhMuc),
                    DbHelper.Param("@gn", giaNhap), DbHelper.Param("@gb", giaBan)));

                if (maNCC.HasValue)
                {
                    DbHelper.ExecuteNonQuery(conn, tran,
                        "INSERT INTO NhaCungCap_SanPham (MaNCC, MaSP, GiaNhap) VALUES (@ncc, @sp, @gn)",
                        DbHelper.Param("@ncc", maNCC.Value), DbHelper.Param("@sp", maSP),
                        DbHelper.Param("@gn", giaNhap));
                }
            });
            return maSP;
        }

        public static int Sua(int maSP, string ten, int maDanhMuc, decimal giaBan)
        {
            return DbHelper.ExecuteNonQuery(
                "UPDATE SanPham SET TenSP=@t, MaDanhMuc=@dm, GiaBan=@gb WHERE MaSP=@id",
                DbHelper.Param("@t", ten), DbHelper.Param("@dm", maDanhMuc),
                DbHelper.Param("@gb", giaBan), DbHelper.Param("@id", maSP));
        }

        public static int NgungBan(int maSP)
        {
            return DbHelper.ExecuteNonQuery("UPDATE SanPham SET TrangThai = 0 WHERE MaSP = @id",
                DbHelper.Param("@id", maSP));
        }
    }

    // ------------------------------------------------------------------
    // QLKH - Kho hàng (frmQuanLyKho)
    // ------------------------------------------------------------------
    public static class KhoDAL
    {
        // Cột: MaSP, TenSP, TenDanhMuc, SoLuongTon, MucTonToiThieu
        public static DataTable TimKiem(string tuKhoa)
        {
            return DbHelper.ExecuteQuery(
                @"SELECT s.MaSP, s.TenSP, d.TenDanhMuc, s.SoLuongTon, s.MucTonToiThieu
                  FROM SanPham s JOIN DanhMuc d ON d.MaDanhMuc = s.MaDanhMuc
                  WHERE s.TrangThai = 1
                    AND (CAST(s.MaSP AS VARCHAR(10)) LIKE @k OR s.TenSP LIKE @k)
                  ORDER BY s.MaSP",
                DbHelper.Param("@k", "%" + (tuKhoa ?? "") + "%"));
        }

        /// <summary>Kiểm kê: đặt tồn kho theo số thực tế và ghi lịch sử điều chỉnh (cùng một transaction).</summary>
        public static void DieuChinhTon(int maSP, int maNhanVienKho, int soLuongThucTe, string lyDo)
        {
            DbHelper.ExecuteTransaction((conn, tran) =>
            {
                // UPDLOCK để không bị thay đổi giữa lúc đọc và ghi
                object cu = DbHelper.ExecuteScalar(conn, tran,
                    "SELECT SoLuongTon FROM SanPham WITH (UPDLOCK) WHERE MaSP = @sp",
                    DbHelper.Param("@sp", maSP));
                if (cu == null) throw new InvalidOperationException("Không tìm thấy sản phẩm.");

                DbHelper.ExecuteNonQuery(conn, tran,
                    @"INSERT INTO LichSuDieuChinhTonKho (MaSP, MaNhanVienKho, SoLuongHeThong, SoLuongThucTe, LyDo)
                      VALUES (@sp, @nv, @ht, @tt, @ld)",
                    DbHelper.Param("@sp", maSP), DbHelper.Param("@nv", maNhanVienKho),
                    DbHelper.Param("@ht", Convert.ToInt32(cu)), DbHelper.Param("@tt", soLuongThucTe),
                    DbHelper.Param("@ld", lyDo));

                DbHelper.ExecuteNonQuery(conn, tran,
                    "UPDATE SanPham SET SoLuongTon = @tt WHERE MaSP = @sp",
                    DbHelper.Param("@tt", soLuongThucTe), DbHelper.Param("@sp", maSP));
            });
        }
    }

    // ------------------------------------------------------------------
    // QLBH - Bán hàng
    // ------------------------------------------------------------------
    public class DongHoaDon
    {
        public int MaSP { get; set; }
        public int SoLuong { get; set; }
    }

    public static class HoaDonDAL
    {
        /// <summary>
        /// Lập hóa đơn trong MỘT transaction: tạo HoaDon, thêm ChiTietHoaDon (đơn giá lấy từ SanPham.GiaBan),
        /// trừ tồn kho (thất bại nếu không đủ hàng), cập nhật TongTienHang. Trả về MaHoaDon.
        /// hinhThuc chỉ nhận "TienMat" hoặc "ChuyenKhoan" (theo CHECK của bảng).
        /// </summary>
        public static int TaoHoaDon(int maNhanVien, int? maKhachHang, int? maKhuyenMai,
                                    string hinhThuc, decimal tienGiamGia, List<DongHoaDon> dong)
        {
            if (dong == null || dong.Count == 0)
                throw new ArgumentException("Hóa đơn chưa có sản phẩm.");

            int maHD = 0;
            DbHelper.ExecuteTransaction((conn, tran) =>
            {
                maHD = Convert.ToInt32(DbHelper.ExecuteScalar(conn, tran,
                    @"INSERT INTO HoaDon (MaNhanVien, MaKhachHang, MaKhuyenMai, HinhThucThanhToan, TienGiamGia)
                      VALUES (@nv, @kh, @km, @ht, @gg);
                      SELECT CAST(SCOPE_IDENTITY() AS INT);",
                    DbHelper.Param("@nv", maNhanVien), DbHelper.Param("@kh", maKhachHang),
                    DbHelper.Param("@km", maKhuyenMai), DbHelper.Param("@ht", hinhThuc),
                    DbHelper.Param("@gg", tienGiamGia)));

                foreach (DongHoaDon d in dong)
                {
                    // Trừ kho có điều kiện: 0 dòng bị ảnh hưởng nghĩa là không đủ hàng
                    int ok = DbHelper.ExecuteNonQuery(conn, tran,
                        "UPDATE SanPham SET SoLuongTon = SoLuongTon - @sl WHERE MaSP = @sp AND SoLuongTon >= @sl",
                        DbHelper.Param("@sl", d.SoLuong), DbHelper.Param("@sp", d.MaSP));
                    if (ok == 0)
                        throw new InvalidOperationException("Sản phẩm mã " + d.MaSP + " không đủ tồn kho.");

                    DbHelper.ExecuteNonQuery(conn, tran,
                        @"INSERT INTO ChiTietHoaDon (MaHoaDon, MaSP, SoLuong, DonGia)
                          SELECT @hd, MaSP, @sl, GiaBan FROM SanPham WHERE MaSP = @sp",
                        DbHelper.Param("@hd", maHD), DbHelper.Param("@sl", d.SoLuong),
                        DbHelper.Param("@sp", d.MaSP));
                }

                DbHelper.ExecuteNonQuery(conn, tran,
                    @"UPDATE HoaDon
                      SET TongTienHang = (SELECT SUM(ThanhTien) FROM ChiTietHoaDon WHERE MaHoaDon = @hd)
                      WHERE MaHoaDon = @hd",
                    DbHelper.Param("@hd", maHD));
            });
            return maHD;
        }
    }
    public static class BanHangDAL
    {
        // Hóa đơn mới nhất (0 hoặc 1 dòng)
        public static DataTable HoaDonMoiNhat()
        {
            return DbHelper.ExecuteQuery(
                @"SELECT TOP 1 MaHoaDon, NgayTao, HinhThucThanhToan,
                     TongTienHang, TienGiamGia, TongThanhToan
              FROM HoaDon ORDER BY MaHoaDon DESC");
        }

        // Các dòng sản phẩm của một hóa đơn
        public static DataTable ChiTiet(int maHoaDon)
        {
            return DbHelper.ExecuteQuery(
                @"SELECT c.MaSP, s.TenSP, c.DonGia, c.SoLuong, c.ThanhTien
              FROM ChiTietHoaDon c JOIN SanPham s ON s.MaSP = c.MaSP
              WHERE c.MaHoaDon = @id ORDER BY c.MaSP",
                DbHelper.Param("@id", maHoaDon));
        }
    }
    public static class PhieuNhapDAL
    {
        public static DataTable LayTatCa()
        {
            return DbHelper.ExecuteQuery(
                @"SELECT p.MaPhieuNhap, n.TenNCC, t.HoTen AS NhanVienKho,
                     p.NgayNhap, p.TongTien,
                     CASE p.TrangThai WHEN N'DaXacNhan' THEN N'Đã xác nhận'
                                      ELSE N'Chờ xác nhận' END AS TrangThai
              FROM PhieuNhapHang p
              JOIN NhaCungCap n ON n.MaNCC = p.MaNCC
              JOIN TaiKhoan t   ON t.MaTK  = p.MaNhanVienKho
              ORDER BY p.MaPhieuNhap DESC");
        }
    }
    public static class KhuyenMaiDAL
    {
        public static DataTable SanPhamKhuyenMai()
        {
            return DbHelper.ExecuteQuery(
                @"SELECT s.MaSP, s.TenSP, s.GiaBan AS GiaGoc,
                 CASE WHEN k.MaKhuyenMai IS NULL THEN N'Không có'
                      WHEN k.LoaiGiam = N'PhanTram' THEN FORMAT(k.GiaTriGiam, '0.##') + N'%'
                      ELSE FORMAT(k.GiaTriGiam, 'N0') END AS MucGiam,
                 CASE WHEN k.MaKhuyenMai IS NULL THEN s.GiaBan
                      WHEN k.LoaiGiam = N'PhanTram' THEN s.GiaBan * (100 - k.GiaTriGiam) / 100
                      WHEN s.GiaBan - k.GiaTriGiam < 0 THEN 0
                      ELSE s.GiaBan - k.GiaTriGiam END AS GiaDaGiam
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
          WHERE s.TrangThai = 1
          ORDER BY s.MaSP");
        }
    }

    // ------------------------------------------------------------------
    // Giỏ hàng của khách hàng (bảng GioHang: mỗi cặp MaKhachHang + MaSP một dòng)
    // Giỏ hàng chỉ lưu sản phẩm khách đã thêm; KHÔNG trừ tồn kho (chỉ trừ khi lập hóa đơn).
    // ------------------------------------------------------------------
    public static class GioHangDAL
    {
        /// <summary>Thêm sản phẩm vào giỏ; nếu đã có thì cộng dồn số lượng.</summary>
        public static void Them(int maKhachHang, int maSP, int soLuong)
        {
            DbHelper.ExecuteNonQuery(
                @"UPDATE GioHang SET SoLuong = SoLuong + @sl
                  WHERE MaKhachHang = @kh AND MaSP = @sp;
                  IF @@ROWCOUNT = 0
                      INSERT INTO GioHang (MaKhachHang, MaSP, SoLuong) VALUES (@kh, @sp, @sl);",
                DbHelper.Param("@kh", maKhachHang),
                DbHelper.Param("@sp", maSP),
                DbHelper.Param("@sl", soLuong));
        }

        // Cột: MaSP (int), TenSP, GiaBan (decimal), SoLuong (int),
        //      ThanhTien (decimal = SoLuong * GiaBan), SoLuongTon (int)
        // Chỉ lấy sản phẩm còn bán (TrangThai = 1).
        public static DataTable Lay(int maKhachHang)
        {
            return DbHelper.ExecuteQuery(
                @"SELECT g.MaSP, s.TenSP, s.GiaBan, g.SoLuong,
                         g.SoLuong * s.GiaBan AS ThanhTien, s.SoLuongTon
                  FROM GioHang g JOIN SanPham s ON s.MaSP = g.MaSP
                  WHERE g.MaKhachHang = @kh AND s.TrangThai = 1
                  ORDER BY g.NgayThem",
                DbHelper.Param("@kh", maKhachHang));
        }

        public static int CapNhatSoLuong(int maKhachHang, int maSP, int soLuong)
        {
            return DbHelper.ExecuteNonQuery(
                "UPDATE GioHang SET SoLuong = @sl WHERE MaKhachHang = @kh AND MaSP = @sp",
                DbHelper.Param("@sl", soLuong),
                DbHelper.Param("@kh", maKhachHang),
                DbHelper.Param("@sp", maSP));
        }

        public static int Xoa(int maKhachHang, int maSP)
        {
            return DbHelper.ExecuteNonQuery(
                "DELETE FROM GioHang WHERE MaKhachHang = @kh AND MaSP = @sp",
                DbHelper.Param("@kh", maKhachHang),
                DbHelper.Param("@sp", maSP));
        }

        // Câu SELECT dùng chung cho phần xem trước và phần thanh toán: mỗi dòng trong giỏ (sản phẩm còn bán)
        // kèm khuyến mãi tốt nhất, theo CÙNG quy tắc với KhuyenMaiDAL.SanPhamKhuyenMai.
        // Cột: MaSP, SoLuong, GiaBan, MaKhuyenMai (có thể NULL), GiamMoiSP (số tiền giảm cho 1 sản phẩm)
        private const string SqlDongGioHang =
            @"SELECT g.MaSP, g.SoLuong, s.GiaBan, k.MaKhuyenMai,
                     CAST(CASE WHEN k.MaKhuyenMai IS NULL THEN 0
                               WHEN k.LoaiGiam = N'PhanTram' THEN s.GiaBan * k.GiaTriGiam / 100
                               WHEN k.GiaTriGiam > s.GiaBan THEN s.GiaBan
                               ELSE k.GiaTriGiam END AS DECIMAL(18,2)) AS GiamMoiSP
              FROM GioHang g
              JOIN SanPham s ON s.MaSP = g.MaSP
              OUTER APPLY (
                  SELECT TOP 1 km.MaKhuyenMai, km.LoaiGiam, km.GiaTriGiam
                  FROM KhuyenMai km
                  WHERE km.TrangThai = 1
                    AND CAST(GETDATE() AS DATE) BETWEEN CAST(km.NgayBatDau AS DATE)
                                                    AND CAST(km.NgayKetThuc AS DATE)
                  ORDER BY CASE km.LoaiGiam WHEN N'PhanTram' THEN s.GiaBan * km.GiaTriGiam / 100
                                            ELSE km.GiaTriGiam END DESC
              ) k
              WHERE g.MaKhachHang = @kh AND s.TrangThai = 1";

        /// <summary>Tổng tiền giảm giá của cả giỏ (decimal) nếu thanh toán ngay bây giờ.</summary>
        public static decimal TinhGiamGia(int maKhachHang)
        {
            object r = DbHelper.ExecuteScalar(
                "SELECT ISNULL(SUM(d.GiamMoiSP * d.SoLuong), 0) FROM (" + SqlDongGioHang + ") d",
                DbHelper.Param("@kh", maKhachHang));
            return Convert.ToDecimal(r);
        }

        /// <summary>
        /// Thanh toán toàn bộ giỏ hàng của khách trong MỘT transaction:
        /// kiểm tra tồn kho, trừ kho, tạo HoaDon (MaNhanVien = NULL vì khách tự mua), thêm ChiTietHoaDon
        /// (đơn giá = GiaBan hiện tại), áp khuyến mãi, rồi xóa các dòng đã mua khỏi giỏ.
        /// Có lỗi ở bước nào thì rollback toàn bộ (kho và giỏ giữ nguyên). Trả về MaHoaDon.
        /// hinhThuc chỉ nhận "TienMat" hoặc "ChuyenKhoan".
        /// </summary>
        public static int ThanhToan(int maKhachHang, string hinhThuc)
        {
            if (hinhThuc != "TienMat" && hinhThuc != "ChuyenKhoan")
                throw new ArgumentException("Hình thức thanh toán không hợp lệ.");

            // Toàn bộ thanh toán là MỘT khối T-SQL: câu lệnh có tham số chạy qua sp_executesql, và bảng tạm
            // #Dong chỉ tồn tại trong phạm vi một lần gọi, nên không thể tách thành nhiều lệnh C# riêng.
            // Khối trả về một chuỗi: "OK|<MaHoaDon>" nếu thành công, ngược lại là thông báo lỗi nghiệp vụ.
            string sql =
                @"SET NOCOUNT ON;
                  SELECT * INTO #Dong FROM (" + SqlDongGioHang + @") x;

                  DECLARE @soDong INT, @thieu NVARCHAR(150), @daTru INT, @hd INT;

                  SELECT @soDong = COUNT(*) FROM #Dong;
                  IF @soDong = 0
                  BEGIN
                      SELECT N'Giỏ hàng trống.';
                      RETURN;
                  END

                  -- Báo rõ sản phẩm nào không đủ hàng
                  SELECT TOP 1 @thieu = s.TenSP
                  FROM #Dong d JOIN SanPham s ON s.MaSP = d.MaSP
                  WHERE d.SoLuong > s.SoLuongTon;
                  IF @thieu IS NOT NULL
                  BEGIN
                      SELECT N'Sản phẩm ' + @thieu + N' không đủ tồn kho. Vui lòng giảm số lượng trong giỏ.';
                      RETURN;
                  END

                  -- Trừ kho có điều kiện; số dòng bị ảnh hưởng phải bằng số dòng trong giỏ
                  UPDATE s SET s.SoLuongTon = s.SoLuongTon - d.SoLuong
                  FROM SanPham s JOIN #Dong d ON d.MaSP = s.MaSP
                  WHERE s.SoLuongTon >= d.SoLuong;
                  SET @daTru = @@ROWCOUNT;
                  IF @daTru <> @soDong
                  BEGIN
                      SELECT N'Tồn kho vừa thay đổi, một số sản phẩm không còn đủ hàng. Vui lòng thử lại.';
                      RETURN;   -- phía C# sẽ rollback phần kho đã trừ
                  END

                  -- Hóa đơn: TienGiamGia = tổng giảm; MaKhuyenMai = khuyến mãi giảm nhiều nhất trong giỏ
                  INSERT INTO HoaDon (MaNhanVien, MaKhachHang, MaKhuyenMai, HinhThucThanhToan, TienGiamGia)
                  VALUES (NULL, @kh,
                          (SELECT TOP 1 MaKhuyenMai FROM #Dong WHERE MaKhuyenMai IS NOT NULL
                           GROUP BY MaKhuyenMai ORDER BY SUM(GiamMoiSP * SoLuong) DESC),
                          @ht,
                          (SELECT ISNULL(SUM(GiamMoiSP * SoLuong), 0) FROM #Dong));
                  SET @hd = CAST(SCOPE_IDENTITY() AS INT);

                  -- Chi tiết hóa đơn (đơn giá = giá bán gốc, giống HoaDonDAL.TaoHoaDon)
                  INSERT INTO ChiTietHoaDon (MaHoaDon, MaSP, SoLuong, DonGia)
                  SELECT @hd, MaSP, SoLuong, GiaBan FROM #Dong;

                  UPDATE HoaDon
                  SET TongTienHang = (SELECT SUM(ThanhTien) FROM ChiTietHoaDon WHERE MaHoaDon = @hd)
                  WHERE MaHoaDon = @hd;

                  -- Xóa các dòng đã mua khỏi giỏ (sản phẩm ngừng bán vẫn nằm lại trong giỏ)
                  DELETE g FROM GioHang g JOIN #Dong d ON d.MaSP = g.MaSP WHERE g.MaKhachHang = @kh;

                  SELECT N'OK|' + CAST(@hd AS NVARCHAR(20));";

            int maHD = 0;
            DbHelper.ExecuteTransaction((conn, tran) =>
            {
                string kq = Convert.ToString(DbHelper.ExecuteScalar(conn, tran, sql,
                    DbHelper.Param("@kh", maKhachHang),
                    DbHelper.Param("@ht", hinhThuc)));

                if (kq != null && kq.StartsWith("OK|"))
                    maHD = int.Parse(kq.Substring(3));
                else
                    throw new InvalidOperationException(kq); // lỗi nghiệp vụ: ExecuteTransaction sẽ rollback
            });
            return maHD;
        }
    }
}