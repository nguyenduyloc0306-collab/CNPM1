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
    public partial class frmQuanLyTaiKhoan : Form
    {
        public frmQuanLyTaiKhoan()
        {
            InitializeComponent();
            Load += frmQuanLyTaiKhoan_Load;
            dataGridView1.SelectionChanged += dataGridView1_SelectionChanged;
        }

        private void frmQuanLyTaiKhoan_Load(object sender, EventArgs e)
        {
            try
            {
                // Lưới đã có sẵn 5 cột: gán mỗi cột với tên cột SQL trả về
                dataGridView1.AutoGenerateColumns = false;
                colID.DataPropertyName = "MaTK";
                colTenDN.DataPropertyName = "TenDangNhap";
                colTenND.DataPropertyName = "HoTen";
                colRole.DataPropertyName = "LoaiNguoiDung";
                colVaiTro.DataPropertyName = "TenVaiTro";

                dataGridView1.ReadOnly = true;
                dataGridView1.AllowUserToAddRows = false;
                dataGridView1.SelectionMode = DataGridViewSelectionMode.FullRowSelect;

                // Khung bên phải chỉ để xem
                textBox4.ReadOnly = true;
                textBox5.ReadOnly = true;
                textBox6.ReadOnly = true;
                textBox7.ReadOnly = true;
                comboBox1.DropDownStyle = ComboBoxStyle.DropDownList;
                comboBox1.Items.Clear();
                comboBox1.Items.AddRange(new object[] { "Nhân viên", "Khách hàng" });

                // Lấy dữ liệu từ SQL và hiển thị
                dataGridView1.DataSource = TaiKhoanDAL.TimKiem("");
            }
            catch (Exception ex)
            {
                MessageBox.Show("Lỗi tải dữ liệu: " + ex.Message, "Lỗi",
                                MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        // Bấm vào dòng nào thì hiện thông tin dòng đó ở khung bên phải
        private void dataGridView1_SelectionChanged(object sender, EventArgs e)
        {
            if (dataGridView1.CurrentRow == null) return;
            DataRowView r = dataGridView1.CurrentRow.DataBoundItem as DataRowView;
            if (r == null) return;

            textBox4.Text = r["MaTK"].ToString();
            textBox5.Text = r["TenDangNhap"].ToString();
            textBox6.Text = r["HoTen"].ToString();
            comboBox1.SelectedIndex = comboBox1.FindStringExact(r["LoaiNguoiDung"].ToString());
            textBox7.Text = r["TenVaiTro"].ToString();
        }

        private void panel1_Paint(object sender, PaintEventArgs e)
        {
        }
    }
}
