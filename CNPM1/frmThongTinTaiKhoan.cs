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
    public partial class frmThongTinTaiKhoan : Form
    {
        public frmThongTinTaiKhoan()
        {
            InitializeComponent();
        }
        public frmThongTinTaiKhoan(int maTK, string hoTen, string tenVaiTro, DateTime ngaySinh, string soDienThoai, string diaChi, string email)
        {
            InitializeComponent();
            textBox2.Text = hoTen;
            textBox3.Text = ngaySinh.ToString();
            textBox4.Text = soDienThoai;
            textBox5.Text = diaChi;
            textBox6.Text = email;

        }
    }
}
