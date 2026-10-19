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
    public partial class frmQuanLySanPham : Form
    {
        public frmQuanLySanPham()
        {
            InitializeComponent();
            Load += frmQuanLySanPham_Load;
            dataGridView1.SelectionChanged += dataGridView1_SelectionChanged;
        }

        private void frmQuanLySanPham_Load(object sender, EventArgs e)
        {
            try
            {
                dataGridView1.AutoGenerateColumns = false;

                // 5 cột thiết kế sẵn, gán theo thứ tự
                dataGridView1.Columns[0].DataPropertyName = "MaSP";
                dataGridView1.Columns[1].DataPropertyName = "TenSP";
                dataGridView1.Columns[2].DataPropertyName = "GiaBan";
                dataGridView1.Columns[3].DataPropertyName = "TenDanhMuc";
                dataGridView1.Columns[4].DataPropertyName = "DanhSachNCC";
                dataGridView1.Columns[2].DefaultCellStyle.Format = "N0";

                dataGridView1.ReadOnly = true;
                dataGridView1.AllowUserToAddRows = false;
                dataGridView1.SelectionMode = DataGridViewSelectionMode.FullRowSelect;
                dataGridView1.AutoSizeColumnsMode = DataGridViewAutoSizeColumnsMode.Fill;

                // Lấy dữ liệu từ SQL và hiển thị
                dataGridView1.DataSource = SanPhamDAL.TimKiem("");
            }
            catch (Exception ex)
            {
                MessageBox.Show("Lỗi tải dữ liệu: " + ex.Message, "Lỗi",
                                MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        // Bấm vào dòng nào thì hiện thông tin sang khung bên phải
        private void dataGridView1_SelectionChanged(object sender, EventArgs e)
        {
            if (dataGridView1.CurrentRow == null) return;
            DataRowView r = dataGridView1.CurrentRow.DataBoundItem as DataRowView;
            if (r == null) return;

            textBox4.Text = r["MaSP"].ToString();
            textBox5.Text = r["TenSP"].ToString();
            textBox6.Text = Convert.ToDecimal(r["GiaBan"]).ToString("N0");
            textBox8.Text = r["TenDanhMuc"].ToString();
            textBox7.Text = r["DanhSachNCC"].ToString();
        }
    }
}