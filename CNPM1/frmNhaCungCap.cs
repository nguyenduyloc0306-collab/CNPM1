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
    public partial class frmNhaCungCap : Form
    {
        public frmNhaCungCap()
        {
            InitializeComponent();
            Load += frmNhaCungCap_Load;
            dataGridView1.SelectionChanged += dataGridView1_SelectionChanged;
        }

        private void frmNhaCungCap_Load(object sender, EventArgs e)
        {
            try
            {
                dataGridView1.AutoGenerateColumns = false;

                // 4 cột thiết kế sẵn, gán theo thứ tự
                dataGridView1.Columns[0].DataPropertyName = "MaNCC";
                dataGridView1.Columns[1].DataPropertyName = "TenNCC";
                dataGridView1.Columns[2].DataPropertyName = "DiaChi";
                dataGridView1.Columns[3].DataPropertyName = "ThongTinLienHe";

                dataGridView1.ReadOnly = true;
                dataGridView1.AllowUserToAddRows = false;
                dataGridView1.SelectionMode = DataGridViewSelectionMode.FullRowSelect;
                dataGridView1.AutoSizeColumnsMode = DataGridViewAutoSizeColumnsMode.Fill;

                comboBox1.DropDownStyle = ComboBoxStyle.DropDownList;

                // Lấy dữ liệu từ SQL và hiển thị
                dataGridView1.DataSource = NhaCungCapDAL.TimKiem("");
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

            textBox4.Text = r["MaNCC"].ToString();
            textBox5.Text = r["TenNCC"].ToString();
            textBox6.Text = r["DiaChi"].ToString();
            textBox7.Text = r["ThongTinLienHe"].ToString();

            // Danh sách mã sản phẩm mà nhà cung cấp này đang cung cấp
            try
            {
                DataTable sp = NhaCungCapDAL.LaySanPhamCuaNCC(Convert.ToInt32(r["MaNCC"]));
                comboBox1.DataSource = sp;
                comboBox1.DisplayMember = "MaSP";
                comboBox1.ValueMember = "MaSP";
            }
            catch (Exception ex)
            {
                MessageBox.Show("Lỗi tải sản phẩm của nhà cung cấp: " + ex.Message);
            }
        }
    }
}