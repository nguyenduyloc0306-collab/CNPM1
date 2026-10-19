using System;
using System.Data;

namespace CNPM1
{
    /// <summary>Đăng ký tài khoản khách hàng (kèm đầy đủ thông tin cá nhân).</summary>
    public static class DangKyDAL
    {
        public const int MaVaiTroKhachHang = 5; // bảng VaiTro: 5 = Khách hàng

        /// <summary>
        /// Thêm tài khoản khách hàng mới và trả về MaTK. Mật khẩu được băm (SHA-256) trước khi lưu.
        /// Tên đăng nhập trùng sẽ ném SqlException (Number 2627 hoặc 2601) do ràng buộc UNIQUE.
        /// </summary>
        public static int DangKyKhachHang(string tenDangNhap, string matKhau, string hoTen,
                                          DateTime ngaySinh, string soDienThoai, string diaChi, string email)
        {
            object id = DbHelper.ExecuteScalar(
                @"INSERT INTO TaiKhoan (TenDangNhap, MatKhau, HoTen, NgaySinh, SoDienThoai, DiaChi, Email, MaVaiTro)
                  VALUES (@u, @p, @ten, @ns, @sdt, @dc, @em, @vt);
                  SELECT CAST(SCOPE_IDENTITY() AS INT);",
                DbHelper.Param("@u", tenDangNhap),
                DbHelper.Param("@p", DbHelper.HashPassword(matKhau)),
                DbHelper.Param("@ten", hoTen),
                DbHelper.Param("@ns", ngaySinh.Date),
                DbHelper.Param("@sdt", soDienThoai),
                DbHelper.Param("@dc", diaChi),
                DbHelper.Param("@em", email),
                DbHelper.Param("@vt", MaVaiTroKhachHang));
            return Convert.ToInt32(id);
        }
    }
}
