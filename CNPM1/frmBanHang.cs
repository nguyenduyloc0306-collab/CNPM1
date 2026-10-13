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
    public partial class frmBanHang : Form
    {
        public frmBanHang()
        {
            InitializeComponent();
            Load += frmBanHang_Load;
        }

        private void frmBanHang_Load(object sender, EventArgs e)
        {
            try
            {
                // Lưới giỏ hàng: 5 cột thiết kế sẵn, gán theo thứ tự
                dataGridView1.AutoGenerateColumns = false;
                dataGridView1.Columns[0].DataPropertyName = "MaSP";
                dataGridView1.Columns[1].DataPropertyName = "TenSP";
                dataGridView1.Columns[2].DataPropertyName = "DonGia";
                dataGridView1.Columns[3].DataPropertyName = "SoLuong";
                dataGridView1.Columns[4].DataPropertyName = "ThanhTien";
                dataGridView1.Columns[2].DefaultCellStyle.Format = "N0";
                dataGridView1.Columns[4].DefaultCellStyle.Format = "N0";

                dataGridView1.ReadOnly = true;
                dataGridView1.AllowUserToAddRows = false;
                dataGridView1.SelectionMode = DataGridViewSelectionMode.FullRowSelect;
                dataGridView1.AutoSizeColumnsMode = DataGridViewAutoSizeColumnsMode.Fill;

                // Các ô chỉ để xem
                textBox1.ReadOnly = true;
                textBox2.ReadOnly = true;
                textBox3.ReadOnly = true;
                textBox4.ReadOnly = true;


                // Lấy hóa đơn mới nhất từ SQL
                DataTable hd = BanHangDAL.HoaDonMoiNhat();
                if (hd.Rows.Count == 0) return;   // chưa có hóa đơn nào

                DataRow r = hd.Rows[0];
                int maHD = Convert.ToInt32(r["MaHoaDon"]);

                textBox4.Text = maHD.ToString();
                textBox6.Text = Convert.ToDecimal(r["TongTienHang"]).ToString("N0");
                textBox1.Text = Convert.ToDecimal(r["TienGiamGia"]).ToString("N0");
                textBox2.Text = Convert.ToDecimal(r["TongThanhToan"]).ToString("N0");

                string ht = r["HinhThucThanhToan"].ToString() == "ChuyenKhoan"
                            ? "Chuyển khoản" : "Tiền mặt";
                int idx = comboBox2.FindStringExact(ht);
                if (idx >= 0) comboBox2.SelectedIndex = idx;

                dataGridView1.DataSource = BanHangDAL.ChiTiet(maHD);
            }
            catch (Exception ex)
            {
                MessageBox.Show("Lỗi tải dữ liệu: " + ex.Message, "Lỗi",
                                MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }
    }
}
