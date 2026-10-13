using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using static System.Windows.Forms.VisualStyles.VisualStyleElement;

namespace CNPM1
{
    public partial class frmQuanLyKho : Form
    {
        public frmQuanLyKho()
        {
            InitializeComponent();
            dataGridView1.SelectionChanged += dataGridView1_SelectionChanged;
        }

        private void frmQuanLyKho_Load(object sender, EventArgs e)
        {
            try
            {
                dataGridView1.AutoGenerateColumns = false;

                // 4 cột thiết kế sẵn, gán theo thứ tự
                dataGridView1.Columns[0].DataPropertyName = "MaSP";
                dataGridView1.Columns[1].DataPropertyName = "TenSP";
                dataGridView1.Columns[2].DataPropertyName = "TenDanhMuc";
                dataGridView1.Columns[3].DataPropertyName = "SoLuongTon";

                dataGridView1.ReadOnly = true;
                dataGridView1.AllowUserToAddRows = false;
                dataGridView1.SelectionMode = DataGridViewSelectionMode.FullRowSelect;
                dataGridView1.AutoSizeColumnsMode = DataGridViewAutoSizeColumnsMode.Fill;

                // Lấy dữ liệu từ SQL và hiển thị
                dataGridView1.DataSource = KhoDAL.TimKiem("");
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
            textBox7.Text = r["TenDanhMuc"].ToString();
            textBox2.Text = r["SoLuongTon"].ToString();
        }

        private void label3_Click(object sender, EventArgs e)
        {
        }

        private void panel1_Paint(object sender, PaintEventArgs e)
        {
        }
    }
}
