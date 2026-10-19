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
    public partial class frmManHinhChinhBanHang : Form
    {
        public int MaTK { get; private set; }
        public string HoTen { get; private set; }
        public string TenVaiTro { get; private set; }

        public frmManHinhChinhBanHang() { InitializeComponent(); }
        public frmManHinhChinhBanHang(int maTK, string hoTen, string tenVaiTro)    
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

        // Quản lý sản phẩm
        private void button3_Click(object sender, EventArgs e)
        {
            frmQuanLySanPham f = new frmQuanLySanPham();
            f.ShowDialog();
        }

        // Quản lý bán hàng
        private void button6_Click(object sender, EventArgs e)
        {
            frmBanHang f = new frmBanHang(MaTK);
            f.ShowDialog();
        }
    }
}
