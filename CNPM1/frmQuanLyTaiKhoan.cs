using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Data.SqlClient;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace CNPM1
{
    public partial class frmQuanLyTaiKhoan : Form
    {
        private DataTable _dt;            // dữ liệu lấy từ SQL
        private bool _dangThem = false;   // true khi đang nhập tài khoản mới

        public frmQuanLyTaiKhoan()
        {
            InitializeComponent();
            Load += frmQuanLyTaiKhoan_Load;

            // Tìm kiếm
            button1.Click += button1_Click;
            textBox1.KeyDown += TimKiem_KeyDown;
            comboBox2.SelectedIndexChanged += LocLai;
            textBox3.TextChanged += LocLai;

            // Thêm / Lưu / Khóa
            button2.Click += button2_Click;
            button3.Click += button3_Click;
            button4.Click += button4_Click;

            // Chọn dòng
            dataGridView1.SelectionChanged += dataGridView1_SelectionChanged;
            dataGridView1.CellClick += dataGridView1_CellClick;
        }

        private void frmQuanLyTaiKhoan_Load(object sender, EventArgs e)
        {
            // Cấu hình lưới: gán cột theo tên cột SQL trả về
            dataGridView1.AutoGenerateColumns = false;
            colID.DataPropertyName = "MaTK";
            colTenDN.DataPropertyName = "TenDangNhap";
            colTenND.DataPropertyName = "HoTen";
            colRole.DataPropertyName = "LoaiNguoiDung";
            colVaiTro.DataPropertyName = "TenVaiTro";
            dataGridView1.ReadOnly = true;
            dataGridView1.AllowUserToAddRows = false;
            dataGridView1.SelectionMode = DataGridViewSelectionMode.FullRowSelect;

            // Ô lọc phía trên
            comboBox2.DropDownStyle = ComboBoxStyle.DropDownList;
            comboBox2.Items.Clear();
            comboBox2.Items.AddRange(new object[] { "Tất cả", "Nhân viên", "Khách hàng" });
            comboBox2.SelectedIndex = 0;

            // Panel bên phải
            label2.Text = "Vai trò (phân quyền)";
            label8.Text = "Mật khẩu (trống = không đổi)";
            textBox7.UseSystemPasswordChar = true;
            textBox4.ReadOnly = true;            // mã tài khoản do SQL tự tăng
            button2.Text = "Thêm";
            button3.Text = "Lưu";
            button4.Text = "Khóa";

            try
            {
                comboBox1.DropDownStyle = ComboBoxStyle.DropDownList;
                comboBox1.DisplayMember = "TenVaiTro";
                comboBox1.ValueMember = "MaVaiTro";
                comboBox1.DataSource = TaiKhoanDAL.LayVaiTro();
                LoadData();
            }
            catch (Exception ex)
            {
                MessageBox.Show("Lỗi tải dữ liệu: " + ex.Message, "Lỗi",
                                MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        // ================= TÌM KIẾM =================

        // Lấy dữ liệu từ SQL rồi hiển thị
        private void LoadData()
        {
            _dt = TaiKhoanDAL.TimKiem(textBox1.Text.Trim());
            dataGridView1.DataSource = _dt.DefaultView;
            LocLai(null, null);
        }

        // Lọc phía client theo comboBox2 (loại) và textBox3 (vai trò)
        private void LocLai(object sender, EventArgs e)
        {
            if (_dt == null) return;

            string filter = "";
            if (comboBox2.SelectedIndex > 0)
                filter = "LoaiNguoiDung = '" + comboBox2.Text + "'";

            string vt = textBox3.Text.Trim();
            if (vt != "")
                filter += (filter == "" ? "" : " AND ")
                        + "TenVaiTro LIKE '%" + EscapeLike(vt) + "%'";

            _dt.DefaultView.RowFilter = filter;
            if (!_dangThem) CapNhatKhungPhai();
        }

        private static string EscapeLike(string s)
        {
            s = s.Replace("[", "[[]").Replace("%", "[%]").Replace("*", "[*]");
            return s.Replace("'", "''");
        }

        private void button1_Click(object sender, EventArgs e)
        {
            try { LoadData(); }
            catch (Exception ex) { MessageBox.Show("Lỗi: " + ex.Message); }
        }

        // Nhấn Enter trong ô tìm kiếm = bấm Tìm kiếm
        private void TimKiem_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.KeyCode == Keys.Enter)
            {
                e.SuppressKeyPress = true;
                button1_Click(sender, EventArgs.Empty);
            }
        }

        // ================= CHỌN DÒNG =================

        // Bấm vào dòng nào thì hiện thông tin dòng đó sang panel bên phải
        private void dataGridView1_SelectionChanged(object sender, EventArgs e)
        {
            if (!_dangThem) CapNhatKhungPhai();
        }

        // Bấm vào một dòng khi đang thêm mới thì thoát chế độ thêm
        private void dataGridView1_CellClick(object sender, DataGridViewCellEventArgs e)
        {
            if (_dangThem && e.RowIndex >= 0)
            {
                ThoatCheDoThem();
                CapNhatKhungPhai();
            }
        }

        private void CapNhatKhungPhai()
        {
            DataRowView r = (dataGridView1.CurrentRow == null) ? null
                            : dataGridView1.CurrentRow.DataBoundItem as DataRowView;
            if (r == null)
            {
                LamTrong();
                return;
            }

            textBox4.Text = r["MaTK"].ToString();
            textBox5.Text = r["TenDangNhap"].ToString();
            textBox6.Text = r["HoTen"].ToString();
            textBox7.Clear();
            int idx = comboBox1.FindStringExact(r["TenVaiTro"].ToString());
            if (idx >= 0) comboBox1.SelectedIndex = idx;
        }

        private void LamTrong()
        {
            textBox4.Clear();
            textBox5.Clear();
            textBox6.Clear();
            textBox7.Clear();
            if (comboBox1.Items.Count > 0) comboBox1.SelectedIndex = 0;
        }

        // ================= THÊM TÀI KHOẢN =================
        // Lần bấm 1: xóa trống các ô để nhập. Lần bấm 2 ("Xác nhận thêm"): lưu vào CSDL.

        private void button2_Click(object sender, EventArgs e)
        {
            if (!_dangThem)
            {
                _dangThem = true;
                button2.Text = "Xác nhận thêm";
                LamTrong();
                textBox4.Text = "(tự động)";
                textBox5.Focus();
                return;
            }

            // Đang ở chế độ thêm: kiểm tra rồi lưu
            string tenDN = textBox5.Text.Trim();
            string hoTen = textBox6.Text.Trim();

            if (tenDN.Length < 3 || tenDN.Contains(" "))
            {
                MessageBox.Show("Tên đăng nhập phải từ 3 ký tự và không có khoảng trắng.");
                textBox5.Focus();
                return;
            }
            if (hoTen == "")
            {
                MessageBox.Show("Nhập tên người dùng.");
                textBox6.Focus();
                return;
            }
            if (textBox7.Text.Length < 6)
            {
                MessageBox.Show("Mật khẩu phải từ 6 ký tự.");
                textBox7.Focus();
                return;
            }
            if (comboBox1.SelectedValue == null)
            {
                MessageBox.Show("Chọn vai trò.");
                return;
            }

            try
            {
                int maMoi = TaiKhoanDAL.Them(tenDN, textBox7.Text, hoTen,
                                             Convert.ToInt32(comboBox1.SelectedValue));
                ThoatCheDoThem();
                textBox1.Clear(); comboBox2.SelectedIndex = 0; textBox3.Clear();
                LoadData();
                ChonDong(maMoi);
                MessageBox.Show("Đã thêm tài khoản.");
            }
            catch (SqlException ex)
            {
                MessageBox.Show(ex.Number == 2627 || ex.Number == 2601
                    ? "Tên đăng nhập đã tồn tại." : "Lỗi: " + ex.Message);
            }
            catch (Exception ex)
            {
                MessageBox.Show("Lỗi: " + ex.Message);
            }
        }

        private void ThoatCheDoThem()
        {
            _dangThem = false;
            button2.Text = "Thêm";
        }

        // ================= LƯU: chỉnh sửa / phân quyền =================

        private void button3_Click(object sender, EventArgs e)
        {
            if (_dangThem)
            {
                MessageBox.Show("Bạn đang thêm tài khoản mới. Bấm \"Xác nhận thêm\" để lưu, "
                              + "hoặc bấm vào một dòng trong danh sách để hủy.");
                return;
            }

            int maTK;
            if (!int.TryParse(textBox4.Text, out maTK))
            {
                MessageBox.Show("Hãy chọn một tài khoản trong danh sách.");
                return;
            }

            string tenDN = textBox5.Text.Trim();
            string hoTen = textBox6.Text.Trim();
            string matKhau = textBox7.Text;

            if (tenDN.Length < 3 || tenDN.Contains(" "))
            {
                MessageBox.Show("Tên đăng nhập phải từ 3 ký tự và không có khoảng trắng.");
                textBox5.Focus();
                return;
            }
            if (hoTen == "")
            {
                MessageBox.Show("Nhập tên người dùng.");
                textBox6.Focus();
                return;
            }
            if (matKhau != "" && matKhau.Length < 6)
            {
                MessageBox.Show("Mật khẩu mới phải từ 6 ký tự (hoặc để trống nếu không đổi).");
                textBox7.Focus();
                return;
            }
            if (comboBox1.SelectedValue == null)
            {
                MessageBox.Show("Chọn vai trò.");
                return;
            }

            if (MessageBox.Show("Lưu thay đổi cho tài khoản này?", "Xác nhận",
                MessageBoxButtons.YesNo, MessageBoxIcon.Question) != DialogResult.Yes) return;

            try
            {
                TaiKhoanDAL.Sua(maTK, tenDN, hoTen,
                                Convert.ToInt32(comboBox1.SelectedValue), matKhau);
                LoadData();
                ChonDong(maTK);
                MessageBox.Show("Đã lưu thay đổi.");
            }
            catch (SqlException ex)
            {
                MessageBox.Show(ex.Number == 2627 || ex.Number == 2601
                    ? "Tên đăng nhập đã tồn tại." : "Lỗi: " + ex.Message);
            }
            catch (Exception ex)
            {
                MessageBox.Show("Lỗi: " + ex.Message);
            }
        }

        // ================= KHÓA TÀI KHOẢN (TrangThai = 0) =================

        private void button4_Click(object sender, EventArgs e)
        {
            int maTK;
            if (_dangThem || !int.TryParse(textBox4.Text, out maTK))
            {
                MessageBox.Show("Hãy chọn một tài khoản trong danh sách để khóa.");
                return;
            }

            if (MessageBox.Show("Khóa tài khoản \"" + textBox5.Text + "\"?\nTài khoản bị khóa sẽ không đăng nhập được.",
                "Xác nhận khóa", MessageBoxButtons.YesNo, MessageBoxIcon.Warning) != DialogResult.Yes) return;

            try
            {
                TaiKhoanDAL.Khoa(maTK);
                LoadData();
                MessageBox.Show("Đã khóa tài khoản.");
            }
            catch (Exception ex)
            {
                MessageBox.Show("Lỗi: " + ex.Message);
            }
        }

        // Chọn lại dòng có mã tài khoản cho trước sau khi tải lại danh sách
        private void ChonDong(int maTK)
        {
            foreach (DataGridViewRow row in dataGridView1.Rows)
            {
                DataRowView r = row.DataBoundItem as DataRowView;
                if (r != null && Convert.ToInt32(r["MaTK"]) == maTK)
                {
                    row.Selected = true;
                    dataGridView1.CurrentCell = row.Cells[0];
                    return;
                }
            }
        }

        private void panel1_Paint(object sender, PaintEventArgs e) { }
    }
}