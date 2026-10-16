using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace CNPM1
{
    public partial class frmDangNhap : Form
    {
        public frmDangNhap()
        {
            InitializeComponent();
        }


        private void button3_Click(object sender, EventArgs e)
        {
            DataRow user = TaiKhoanDAL.DangNhap(textBox1.Text.Trim(), textBox2.Text);
            if (user == null)
            {
                MessageBox.Show("Sai tài khoản/mật khẩu hoặc tài khoản bị khóa.");
                return;
            }

			User.MaTK = (int)user["MaTK"];
			User.HoTen = (string)user["HoTen"];
			User.TenVaiTro = (string)user["TenVaiTro"];

			Form frm;
            switch ((int)user["MaVaiTro"])
            {
                case 2: // Quản lý cửa hàng
                    frm = new frmManHinhChinhQuanLy((int)user["MaTK"], (string)user["HoTen"], (string)user["TenVaiTro"]);
                    break;
                case 3: // Nhân viên bán hàng
                    frm = new frmManHinhChinhBanHang((int)user["MaTK"], (string)user["HoTen"], (string)user["TenVaiTro"]);
                    break;
                case 4: // Nhân viên kho
                    frm = new frmManHinhChinhKho((int)user["MaTK"], (string)user["HoTen"], (string)user["TenVaiTro"]);
                    break;
                case 5: // Khách hàng
                    frm = new frmManHinhChinhKhachHang((int)user["MaTK"], (string)user["HoTen"], (string)user["TenVaiTro"], (DateTime)user["NgaySinh"], (string)user["SoDienThoai"], (string)user["DiaChi"], (string)user["Email"]);
                    break;
                default: // 1 - Quản trị viên
                    frm = new frmManHinhChinh((int)user["MaTK"], (string)user["HoTen"], (string)user["TenVaiTro"]);
                    break;
            }

            frm.FormClosed += (s, args) => this.Close(); // đóng form chính thì thoát chương trình
            frm.Show();
            this.Hide();
        }

        private void comboBox1_SelectedIndexChanged(object sender, EventArgs e)
        {
                
        }

        private void label4_Click(object sender, EventArgs e)
        {

        }

        private void button1_Click(object sender, EventArgs e)
        {
            frmDangKyKhachHang frm2 = new frmDangKyKhachHang();
            frm2.Show();
        }
    }
}
