using System;
using System.Data;

namespace CNPM1
{
    /// <summary>
    /// Dữ liệu sản phẩm cho màn hình bán hàng tại quầy (frmBanHang / frmChonSanPham).
    /// Quy tắc khuyến mãi giống KhuyenMaiDAL.SanPhamKhuyenMai, GioHangDAL và MuaHangDAL:
    /// mỗi sản phẩm lấy khuyến mãi đang hiệu lực giảm nhiều nhất.
    /// </summary>
    public static class BanHangMoiDAL
    {
        // Cột: MaSP, TenSP, TenDanhMuc, GiaBan (decimal), SoLuongTon (int),
        //      MaKhuyenMai (int, có thể NULL), GiamMoiSP (decimal: số tiền giảm cho 1 sản phẩm)
        private const string SqlSanPham =
            @"SELECT s.MaSP, s.TenSP, d.TenDanhMuc, s.GiaBan, s.SoLuongTon, k.MaKhuyenMai,
                     CAST(CASE WHEN k.MaKhuyenMai IS NULL THEN 0
                               WHEN k.LoaiGiam = N'PhanTram' THEN s.GiaBan * k.GiaTriGiam / 100
                               WHEN k.GiaTriGiam > s.GiaBan THEN s.GiaBan
                               ELSE k.GiaTriGiam END AS DECIMAL(18,2)) AS GiamMoiSP
              FROM SanPham s
              JOIN DanhMuc d ON d.MaDanhMuc = s.MaDanhMuc
              OUTER APPLY (
                  SELECT TOP 1 km.MaKhuyenMai, km.LoaiGiam, km.GiaTriGiam
                  FROM KhuyenMai km
                  WHERE km.TrangThai = 1
                    AND CAST(GETDATE() AS DATE) BETWEEN CAST(km.NgayBatDau AS DATE)
                                                    AND CAST(km.NgayKetThuc AS DATE)
                  ORDER BY CASE km.LoaiGiam WHEN N'PhanTram' THEN s.GiaBan * km.GiaTriGiam / 100
                                            ELSE km.GiaTriGiam END DESC
              ) k
              WHERE s.TrangThai = 1";

        private static string ThoatLike(string s)
        {
            return (s ?? "").Replace("[", "[[]").Replace("%", "[%]").Replace("_", "[_]");
        }

        /// <summary>Sản phẩm đang bán và còn hàng, tìm theo mã hoặc tên (khớp một phần; rỗng = tất cả).</summary>
        public static DataTable DanhSachSanPham(string tuKhoa)
        {
            return DbHelper.ExecuteQuery(
                SqlSanPham + @"
                AND s.SoLuongTon > 0
                AND (CAST(s.MaSP AS VARCHAR(10)) LIKE @k OR s.TenSP LIKE @k)
              ORDER BY s.MaSP",
                DbHelper.Param("@k", "%" + ThoatLike(tuKhoa) + "%"));
        }

        /// <summary>Thông tin hiện tại của một sản phẩm đang bán; null nếu không tồn tại hoặc đã ngừng bán.</summary>
        public static DataRow ThongTin(int maSP)
        {
            DataTable dt = DbHelper.ExecuteQuery(
                SqlSanPham + " AND s.MaSP = @sp",
                DbHelper.Param("@sp", maSP));
            return dt.Rows.Count == 1 ? dt.Rows[0] : null;
        }
    }
}
