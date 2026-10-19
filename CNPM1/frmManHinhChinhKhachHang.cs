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
    public partial class frmManHinhChinhKhachHang : Form
    {
        public int MaTK { get; private set; }
        public string HoTen { get; private set; }
        public string TenVaiTro { get; private set; }
        public DateTime NgaySinh { get; private set; }
        public string SoDienThoai { get; private set; }
        public string DiaChi{ get; private set; }
        public string Email { get; private set; }
        public frmManHinhChinhKhachHang()
        {
            InitializeComponent();
        }
        public frmManHinhChinhKhachHang(int maTK, string hoTen, string tenVaiTro, DateTime ngaySinh, string soDienThoai, string diaChi, string email)
        {
            InitializeComponent();
            MaTK = maTK;
            HoTen = hoTen;
            TenVaiTro = tenVaiTro;
            NgaySinh = ngaySinh;
            SoDienThoai = soDienThoai;
            DiaChi = diaChi;
            Email = email;
            label3.Text = "Chào mừng! " + tenVaiTro + " " + hoTen;
        }


        private void button1_Click(object sender, EventArgs e)
        {
            frmMuaHang frm = new frmMuaHang(MaTK);
            frm.Show();
        }

        private void button3_Click(object sender, EventArgs e)
        {
            frmGioHang frm = new frmGioHang(MaTK);
            frm.Show();
        }

        private void button4_Click(object sender, EventArgs e)
        {
            frmThongTinTaiKhoan frm = new frmThongTinTaiKhoan(MaTK, HoTen, TenVaiTro, NgaySinh, SoDienThoai, DiaChi, Email);
            frm.Show();
        }

        private void button2_Click(object sender, EventArgs e)
        {
            frmLichSuMuaHang frm = new frmLichSuMuaHang(MaTK);
            frm.Show();
        }
    }
}
