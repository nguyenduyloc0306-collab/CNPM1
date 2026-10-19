using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Data.SqlClient;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace CNPM1
{
    /// <summary>
    /// Đăng ký tài khoản khách hàng. Thu thập đủ họ tên, tài khoản, ngày sinh, số điện thoại, email, địa chỉ,
    /// mật khẩu để khi đăng nhập, frmDangNhap có thể truyền các thông tin này (không NULL) sang
    /// frmManHinhChinhKhachHang(MaTK, HoTen, TenVaiTro, NgaySinh, SoDienThoai, DiaChi, Email).
    /// </summary>
    public partial class frmDangKyKhachHang : Form
    {
        public frmDangKyKhachHang()
        {
            InitializeComponent();
            dtpNgaySinh.MaxDate = DateTime.Today;          // không cho chọn ngày trong tương lai
            dtpNgaySinh.Value = new DateTime(2000, 1, 1);  // giá trị gợi ý ban đầu
        }

        private void btnDangKy_Click(object sender, EventArgs e)
        {
            string hoTen = txtHoTen.Text.Trim();
            string tenDangNhap = txtTaiKhoan.Text.Trim();
            DateTime ngaySinh = dtpNgaySinh.Value.Date;
            string soDienThoai = Regex.Replace(txtSoDienThoai.Text, @"[\s.\-]", ""); // bỏ khoảng trắng, dấu chấm, gạch ngang
            string email = txtEmail.Text.Trim();
            string diaChi = txtDiaChi.Text.Trim();
            string matKhau = txtMatKhau.Text;   // không Trim: khoảng trắng có thể là một phần của mật khẩu
            string nhapLai = txtNhapLai.Text;

            // ---- Họ và tên (NVARCHAR(100))
            if (hoTen.Length == 0)
            {
                Loi("Vui lòng nhập họ và tên.", txtHoTen);
                return;
            }
            if (hoTen.Length > 100)
            {
                Loi("Họ và tên tối đa 100 ký tự.", txtHoTen);
                return;
            }

            // ---- Tài khoản: cột TenDangNhap là VARCHAR(50), không Unicode nên chỉ cho chữ không dấu, số, '.', '_'
            if (tenDangNhap.Length < 4 || tenDangNhap.Length > 50)
            {
                Loi("Tên tài khoản phải từ 4 đến 50 ký tự.", txtTaiKhoan);
                return;
            }
            if (!Regex.IsMatch(tenDangNhap, @"^[A-Za-z0-9._]+$"))
            {
                Loi("Tên tài khoản chỉ gồm chữ không dấu, số, dấu chấm (.) và gạch dưới (_).", txtTaiKhoan);
                return;
            }

            // ---- Ngày sinh (DATE)
            if (ngaySinh > DateTime.Today)
            {
                Loi("Ngày sinh không được ở tương lai.", dtpNgaySinh);
                return;
            }

            // ---- Số điện thoại (VARCHAR(15)): 10 số bắt đầu bằng 0, hoặc +84 và 9 số
            if (!Regex.IsMatch(soDienThoai, @"^(0|\+84)\d{9}$"))
            {
                Loi("Số điện thoại không hợp lệ. Ví dụ: 0912345678 hoặc +84912345678.", txtSoDienThoai);
                return;
            }

            // ---- Email (VARCHAR(100))
            if (email.Length > 100 || !Regex.IsMatch(email, @"^[A-Za-z0-9._%+\-]+@[A-Za-z0-9.\-]+\.[A-Za-z]{2,}$"))
            {
                Loi("Email không hợp lệ (tối đa 100 ký tự, ví dụ: ten@gmail.com).", txtEmail);
                return;
            }

            // ---- Địa chỉ (NVARCHAR(255))
            if (diaChi.Length == 0)
            {
                Loi("Vui lòng nhập địa chỉ.", txtDiaChi);
                return;
            }
            if (diaChi.Length > 255)
            {
                Loi("Địa chỉ tối đa 255 ký tự.", txtDiaChi);
                return;
            }

            // ---- Mật khẩu
            if (matKhau.Length < 6)
            {
                Loi("Mật khẩu phải có ít nhất 6 ký tự.", txtMatKhau);
                return;
            }
            if (matKhau != nhapLai)
            {
                txtNhapLai.Clear();
                Loi("Mật khẩu nhập lại không khớp.", txtNhapLai);
                return;
            }

            try
            {
                // Vai trò luôn là Khách hàng, do chương trình quyết định chứ không do người dùng chọn
                DangKyDAL.DangKyKhachHang(tenDangNhap, matKhau, hoTen, ngaySinh, soDienThoai, diaChi, email);

                MessageBox.Show("Đăng ký thành công! Bạn có thể đăng nhập bằng tài khoản \"" + tenDangNhap + "\".",
                                "Đăng ký", MessageBoxButtons.OK, MessageBoxIcon.Information);
                Close();
            }
            catch (SqlException ex) when (ex.Number == 2627 || ex.Number == 2601) // vi phạm UNIQUE của TenDangNhap
            {
                Loi("Tên tài khoản \"" + tenDangNhap + "\" đã được sử dụng. Vui lòng chọn tên khác.", txtTaiKhoan);
                txtTaiKhoan.SelectAll();
            }
            catch (Exception ex)
            {
                MessageBox.Show("Lỗi đăng ký: " + ex.Message, "Lỗi", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        // Hiện thông báo lỗi và đưa con trỏ về ô cần sửa
        private void Loi(string thongBao, Control oCanSua)
        {
            MessageBox.Show(thongBao, "Đăng ký", MessageBoxButtons.OK, MessageBoxIcon.Warning);
            oCanSua.Focus();
        }
    }
}