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
    public partial class frmDanhMuc : Form
    {
        public frmDanhMuc()
        {
            InitializeComponent();
            Load += frmDanhMuc_Load;
            dataGridView1.SelectionChanged += dataGridView1_SelectionChanged;
        }

        private void frmDanhMuc_Load(object sender, EventArgs e)
        {
            try
            {
                dataGridView1.AutoGenerateColumns = false;
                dataGridView1.Columns[0].DataPropertyName = "MaDanhMuc";
                dataGridView1.Columns[1].DataPropertyName = "TenDanhMuc";
                dataGridView1.Columns[2].DataPropertyName = "MoTa";

                dataGridView1.ReadOnly = true;
                dataGridView1.AllowUserToAddRows = false;
                dataGridView1.SelectionMode = DataGridViewSelectionMode.FullRowSelect;
                dataGridView1.AutoSizeColumnsMode = DataGridViewAutoSizeColumnsMode.Fill;

                dataGridView1.DataSource = DanhMucDAL.TimKiem("");
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

            textBox4.Text = r["MaDanhMuc"].ToString();
            textBox5.Text = r["TenDanhMuc"].ToString();
            textBox2.Text = r["MoTa"].ToString();
        }

        private void panel3_Paint(object sender, PaintEventArgs e)
        {
        }
    }
}
