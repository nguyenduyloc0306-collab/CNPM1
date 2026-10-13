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
    public partial class frmMuaHang : Form
    {
        public int MaTK { get; private set; }
        public frmMuaHang(int maTK)
        {
            InitializeComponent();
            MaTK = maTK;
        }

        private void dataGridView1_CellContentClick(object sender, DataGridViewCellEventArgs e)
        {

        }

        private void frmMuaHang_Load(object sender, EventArgs e)
        {
            try
            {
                dataGridView1.AutoGenerateColumns = false;

                // 5 cột thiết kế sẵn, gán theo thứ tự
                dataGridView1.Columns[0].DataPropertyName = "TenSP";
                dataGridView1.Columns[1].DataPropertyName = "GiaBan";
                dataGridView1.Columns[2].DataPropertyName = "TenDanhMuc";
                dataGridView1.Columns[1].DefaultCellStyle.Format = "N0";

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

        private int maSPDangChon = 0; // dùng cho nút Mua / Thêm vào giỏ

        private void dataGridView1_SelectionChanged(object sender, EventArgs e)
        {
            if (dataGridView1.CurrentRow == null) return;

            DataRowView row = dataGridView1.CurrentRow.DataBoundItem as DataRowView;
            if (row == null) return;

            maSPDangChon = Convert.ToInt32(row["MaSP"]);
            textBox5.Text = row["TenSP"].ToString();
            textBox6.Text = Convert.ToDecimal(row["GiaBan"]).ToString("N0");
            textBox8.Text = row["TenDanhMuc"].ToString();
        }

        private void button4_Click(object sender, EventArgs e)
        {
            frmGioHang frm = new frmGioHang(MaTK);
            frm.Show();

        }

        private void button2_Click(object sender, EventArgs e) // Thêm vào giỏ
        {
            if (maSPDangChon == 0)
            {
                MessageBox.Show("Vui lòng chọn một sản phẩm trong danh sách.");
                return;
            }

            try
            {
                DataRow sp = MuaHangDAL.ThongTin(maSPDangChon);
                if (sp == null) { MessageBox.Show("Sản phẩm không còn được bán."); return; }

                int ton = Convert.ToInt32(sp["SoLuongTon"]);
                if (ton < 1) { MessageBox.Show("Sản phẩm này đã hết hàng."); return; }

                using (frmChonSoLuong dlg = new frmChonSoLuong(maSPDangChon, sp["TenSP"].ToString(),
                           Convert.ToDecimal(sp["GiaBan"]), ton, false))
                {
                    if (dlg.ShowDialog(this) != DialogResult.OK) return;

                    GioHangDAL.Them(MaTK, maSPDangChon, dlg.SoLuong);
                    MessageBox.Show("Đã thêm " + dlg.SoLuong + " \"" + sp["TenSP"] + "\" vào giỏ hàng.",
                                    "Giỏ hàng", MessageBoxButtons.OK, MessageBoxIcon.Information);
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show("Lỗi: " + ex.Message, "Lỗi", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void button3_Click(object sender, EventArgs e) // Mua
        {
            if (maSPDangChon == 0)
            {
                MessageBox.Show("Vui lòng chọn một sản phẩm trong danh sách.");
                return;
            }

            try
            {
                DataRow sp = MuaHangDAL.ThongTin(maSPDangChon);
                if (sp == null) { MessageBox.Show("Sản phẩm không còn được bán."); return; }

                int ton = Convert.ToInt32(sp["SoLuongTon"]);
                if (ton < 1) { MessageBox.Show("Sản phẩm này đã hết hàng."); return; }

                using (frmChonSoLuong dlg = new frmChonSoLuong(maSPDangChon, sp["TenSP"].ToString(),
                           Convert.ToDecimal(sp["GiaBan"]), ton, true))
                {
                    if (dlg.ShowDialog(this) != DialogResult.OK) return;

                    int maHD = MuaHangDAL.MuaNgay(MaTK, maSPDangChon, dlg.SoLuong, dlg.HinhThuc);
                    MessageBox.Show("Mua hàng thành công!\nMã hóa đơn: " + maHD +
                                    "\nBạn có thể xem lại trong \"Lịch sử mua hàng\".",
                                    "Mua hàng", MessageBoxButtons.OK, MessageBoxIcon.Information);
                }
            }
            catch (InvalidOperationException ex) // hết hàng / ngừng bán: lỗi nghiệp vụ, đã rollback
            {
                MessageBox.Show(ex.Message, "Không thể mua hàng", MessageBoxButtons.OK, MessageBoxIcon.Warning);
            }
            catch (Exception ex)
            {
                MessageBox.Show("Lỗi mua hàng: " + ex.Message, "Lỗi", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }
        private void button1_Click(object sender, EventArgs e) // Tìm kiếm
        {
            decimal? giaTu, giaDen;
            if (!TryDocGia(textBox3.Text, out giaTu))
            {
                MessageBox.Show("Giá bắt đầu không hợp lệ. Chỉ nhập số, ví dụ 100000.");
                textBox3.Focus();
                return;
            }
            if (!TryDocGia(textBox9.Text, out giaDen))
            {
                MessageBox.Show("Giá kết thúc không hợp lệ. Chỉ nhập số, ví dụ 500000.");
                textBox9.Focus();
                return;
            }
            if (giaTu.HasValue && giaDen.HasValue && giaTu.Value > giaDen.Value)
            {
                MessageBox.Show("Khoảng giá không hợp lệ: giá bắt đầu lớn hơn giá kết thúc.");
                return;
            }

            try
            {
                DataTable dt = MuaHangDAL.TimKiemSanPham(
                    textBox1.Text.Trim(), textBox2.Text.Trim(), giaTu, giaDen);
                dataGridView1.DataSource = dt;

                if (dt.Rows.Count == 0)
                {
                    // Không còn dòng nào được chọn: xóa sản phẩm đang chọn để nút Mua / Thêm vào giỏ không dùng nhầm
                    maSPDangChon = 0;
                    textBox5.Clear();
                    textBox6.Clear();
                    textBox8.Clear();
                    MessageBox.Show("Không tìm thấy sản phẩm phù hợp.", "Tìm kiếm",
                                    MessageBoxButtons.OK, MessageBoxIcon.Information);
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show("Lỗi tìm kiếm: " + ex.Message, "Lỗi",
                                MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        // Đọc ô giá: bỏ trống = không giới hạn. Chấp nhận 100000, 100,000, 100.000, 100.000đ, 100000 vnd.
        private static bool TryDocGia(string text, out decimal? gia)
        {
            gia = null;
            string s = text.Trim().ToLowerInvariant();
            if (s.Length == 0) return true;

            if (s.EndsWith("vnd")) s = s.Substring(0, s.Length - 3);
            else if (s.EndsWith("đ") || s.EndsWith("d")) s = s.Substring(0, s.Length - 1);
            s = s.Replace(" ", "").Replace(".", "").Replace(",", "");

            decimal v;
            if (s.Length == 0 || !s.All(char.IsDigit) ||
                !decimal.TryParse(s, System.Globalization.NumberStyles.None,
                                  System.Globalization.CultureInfo.InvariantCulture, out v))
                return false;

            gia = v;
            return true;
        }
    }
}
