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
    public partial class frmManHinhChinhQuanLy : Form
    {
        public int MaTK { get; private set; }
        public string HoTen { get; private set; }
        public string TenVaiTro { get; private set; }

        public frmManHinhChinhQuanLy() { InitializeComponent(); }
        public frmManHinhChinhQuanLy(int maTK, string hoTen, string tenVaiTro)    
        {
            InitializeComponent();
            MaTK = maTK;
            HoTen = hoTen;
            TenVaiTro = tenVaiTro;
            label3.Text = "Chào mừng! " + tenVaiTro + " " + hoTen;
        }

        private void label3_Click(object sender, EventArgs e)
        {

        }
        // Quản lý tài khoản
        private void button1_Click(object sender, EventArgs e)
        {
            frmQuanLyTaiKhoan f = new frmQuanLyTaiKhoan();
            f.ShowDialog();
        }
        // Quản lý danh mục
        private void button2_Click(object sender, EventArgs e)
        {
            frmDanhMuc f = new frmDanhMuc();
            f.ShowDialog();
        }

        // Quản lý sản phẩm
        private void button3_Click(object sender, EventArgs e)
        {
            frmQuanLySanPham f = new frmQuanLySanPham();
            f.ShowDialog();
        }

        // Quản lý nhà cung cấp
        private void button4_Click(object sender, EventArgs e)
        {
            frmNhaCungCap f = new frmNhaCungCap();
            f.ShowDialog();
        }

        // Quản lý kho
        private void button5_Click(object sender, EventArgs e)
        {
            frmQuanLyKho f = new frmQuanLyKho();
            f.ShowDialog();
        }

        // Quản lý bán hàng
        private void button6_Click(object sender, EventArgs e)
        {
            frmBanHang f = new frmBanHang(MaTK);
            f.ShowDialog();
        }

        // Quản lý khuyến mãi
        private void button7_Click(object sender, EventArgs e)
        {
            frmKhuyenMai f = new frmKhuyenMai();
            f.ShowDialog();
        }

        // Quản lý nhập hàng
        private void button8_Click(object sender, EventArgs e)
        {
            frmNhapHang f = new frmNhapHang();
            f.ShowDialog();
        }
    }
}
