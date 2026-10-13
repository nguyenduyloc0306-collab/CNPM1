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
    public partial class frmNhapHang : Form
    {
        public frmNhapHang()
        {
            InitializeComponent();
            Load += frmNhapHang_Load;
            dataGridView1.SelectionChanged += dataGridView1_SelectionChanged;
        }

        private void frmNhapHang_Load(object sender, EventArgs e)
        {
            try
            {
                dataGridView1.AutoGenerateColumns = false;

                // 5 cột thiết kế sẵn: gán theo tên cột SQL (bỏ qua NhanVienKho)
                dataGridView1.Columns[0].DataPropertyName = "MaPhieuNhap";
                dataGridView1.Columns[1].DataPropertyName = "TenNCC";
                dataGridView1.Columns[2].DataPropertyName = "NgayNhap";
                dataGridView1.Columns[3].DataPropertyName = "TongTien";
                dataGridView1.Columns[4].DataPropertyName = "TrangThai";
                dataGridView1.Columns[2].DefaultCellStyle.Format = "dd/MM/yyyy HH:mm";
                dataGridView1.Columns[3].DefaultCellStyle.Format = "N0";

                dataGridView1.ReadOnly = true;
                dataGridView1.AllowUserToAddRows = false;
                dataGridView1.SelectionMode = DataGridViewSelectionMode.FullRowSelect;
                dataGridView1.AutoSizeColumnsMode = DataGridViewAutoSizeColumnsMode.Fill;

                // Lấy dữ liệu từ SQL và hiển thị
                dataGridView1.DataSource = PhieuNhapDAL.LayTatCa();
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

            textBox2.Text = r["MaPhieuNhap"].ToString();
            textBox3.Text = r["TenNCC"].ToString();
            textBox4.Text = Convert.ToDateTime(r["NgayNhap"]).ToString("dd/MM/yyyy HH:mm");
            textBox5.Text = Convert.ToDecimal(r["TongTien"]).ToString("N0");
            textBox6.Text = r["TrangThai"].ToString();
        }
    }
}
