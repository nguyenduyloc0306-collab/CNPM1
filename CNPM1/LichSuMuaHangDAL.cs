using System;
using System.Data;

namespace CNPM1
{
    /// <summary>
    /// Lịch sử mua hàng của khách: chỉ lấy hóa đơn có MaKhachHang trùng với khách đang đăng nhập.
    /// (Hóa đơn bán tại quầy cho khách vãng lai có MaKhachHang = NULL nên không xuất hiện ở đây.)
    /// </summary>
    public static class LichSuMuaHangDAL
    {
        // Cột: MaHoaDon (int), NgayTao (DateTime), HinhThuc, TongTienHang (decimal),
        //      TienGiamGia (decimal), TongThanhToan (decimal), TenKhuyenMai, TrangThai
        public static DataTable DanhSach(int maKhachHang)
        {
            return DbHelper.ExecuteQuery(
                @"SELECT h.MaHoaDon, h.NgayTao,
                         CASE h.HinhThucThanhToan WHEN N'ChuyenKhoan' THEN N'Chuyển khoản'
                                                  ELSE N'Tiền mặt' END AS HinhThuc,
                         h.TongTienHang, h.TienGiamGia, h.TongThanhToan,
                         ISNULL(k.TenKhuyenMai, N'') AS TenKhuyenMai,
                         CASE h.TrangThai WHEN N'DaThanhToan' THEN N'Đã thanh toán'
                                          ELSE h.TrangThai END AS TrangThai
                  FROM HoaDon h
                  LEFT JOIN KhuyenMai k ON k.MaKhuyenMai = h.MaKhuyenMai
                  WHERE h.MaKhachHang = @kh
                  ORDER BY h.MaHoaDon DESC",
                DbHelper.Param("@kh", maKhachHang));
        }

        // Cột: MaSP, TenSP, DonGia (decimal), SoLuong (int), ThanhTien (decimal)
        // Có kèm điều kiện MaKhachHang để khách không xem được hóa đơn của người khác.
        public static DataTable ChiTiet(int maKhachHang, int maHoaDon)
        {
            return DbHelper.ExecuteQuery(
                @"SELECT c.MaSP, s.TenSP, c.DonGia, c.SoLuong, c.ThanhTien
                  FROM ChiTietHoaDon c
                  JOIN HoaDon h  ON h.MaHoaDon = c.MaHoaDon
                  JOIN SanPham s ON s.MaSP = c.MaSP
                  WHERE c.MaHoaDon = @hd AND h.MaKhachHang = @kh
                  ORDER BY c.MaSP",
                DbHelper.Param("@hd", maHoaDon),
                DbHelper.Param("@kh", maKhachHang));
        }
    }
}
