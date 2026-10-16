using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace CNPM1
{
    public partial class frmQuanLySanPham : Form
    {
        private DataTable _dt;               // dữ liệu lấy từ SQL
        private bool _dangThem = false;      // true khi đang nhập sản phẩm mới
        private bool _sanSang = false;       // true khi đã dựng xong giao diện
        private int _maSP = -1;              // mã sản phẩm của dòng đang chọn

        // Các control tìm được trên form
        private TextBox txtTimKiem, txtLocGia, txtLocNCC;
        private Button btnTim, btnLuu, btnThem, btnXoa;

        // Các control do code tự tạo
        private ComboBox cboDanhMuc, cboNCC;
        private TextBox txtGiaNhap;
        private Label lblGiaNhap;

        public frmQuanLySanPham()
        {
            InitializeComponent();
            Load += frmQuanLySanPham_Load;
            dataGridView1.SelectionChanged += dataGridView1_SelectionChanged;
            dataGridView1.CellClick += dataGridView1_CellClick;
        }

        // ================= KHỞI TẠO =================
        private void frmQuanLySanPham_Load(object sender, EventArgs e)
        {
            try
            {
                // Lưới: 5 cột thiết kế sẵn, gán theo thứ tự
                dataGridView1.AutoGenerateColumns = false;
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

                // Tìm nút theo chữ trên nút (không phụ thuộc tên control)
                btnTim = TimNut("Tìm kiếm");
                btnLuu = TimNut("Lưu");
                btnThem = TimNut("Thêm");
                btnXoa = TimNut("Xóa");
                if (btnTim == null || btnLuu == null || btnThem == null || btnXoa == null)
                {
                    MessageBox.Show("Không tìm thấy các nút Tìm kiếm / Lưu / Thêm / Xóa trên form.");
                    return;
                }

                // Tìm 3 ô nhập phía trên lưới theo vị trí: tìm kiếm, đơn giá, mã nhà cung cấp
                List<TextBox> tren = TextBoxPhiaTren();
                if (tren.Count < 3)
                {
                    MessageBox.Show("Không tìm thấy đủ 3 ô nhập phía trên lưới.");
                    return;
                }
                txtTimKiem = tren[0];
                txtLocGia = tren[1];
                txtLocNCC = tren[2];

                // Khung bên phải: mã, tên, đơn giá giữ ô chữ; danh mục, NCC thành ô chọn
                textBox4.ReadOnly = true;              // mã sản phẩm do SQL tự tăng
                cboDanhMuc = TaoComboThayThe(textBox8);
                cboNCC = TaoComboThayThe(textBox7);
                TaoOGiaNhap();

                // Nạp danh sách cho 2 ô chọn
                NapDanhMuc();
                NapNhaCungCap();

                // Đổi chữ nút xóa thành ngừng kinh doanh
                btnXoa.Text = "Ngừng KD";

                // Gắn sự kiện
                btnTim.Click += btnTim_Click;
                btnThem.Click += btnThem_Click;
                btnLuu.Click += btnLuu_Click;
                btnXoa.Click += btnXoa_Click;
                txtTimKiem.KeyDown += TimKiem_KeyDown;
                txtLocGia.TextChanged += LocLai;
                txtLocNCC.TextChanged += LocLai;

                DatCheDo(false);
                _sanSang = true;

                LoadData();
            }
            catch (Exception ex)
            {
                MessageBox.Show("Lỗi tải dữ liệu: " + ex.Message, "Lỗi",
                                MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        // ---------- tìm control ----------
        private Button TimNut(string chu)
        {
            return TimNut(this, chu);
        }

        private Button TimNut(Control cha, string chu)
        {
            foreach (Control c in cha.Controls)
            {
                Button b = c as Button;
                if (b != null && b.Text.Trim() == chu) return b;
                Button con = TimNut(c, chu);
                if (con != null) return con;
            }
            return null;
        }

        private void ThuThapTextBox(Control cha, List<TextBox> ds)
        {
            foreach (Control c in cha.Controls)
            {
                TextBox t = c as TextBox;
                if (t != null) ds.Add(t);
                ThuThapTextBox(c, ds);
            }
        }

        // Các ô nhập nằm phía trên lưới, xếp từ trái sang phải
        private List<TextBox> TextBoxPhiaTren()
        {
            List<TextBox> tat = new List<TextBox>();
            ThuThapTextBox(this, tat);
            int yLuoi = dataGridView1.PointToScreen(Point.Empty).Y;
            return tat
                .Where(t => t.Parent.PointToScreen(new Point(t.Left, t.Bottom)).Y <= yLuoi)
                .OrderBy(t => t.Parent.PointToScreen(t.Location).X)
                .ToList();
        }

        // Thay một ô chữ bằng ô chọn cùng vị trí, cùng kích thước
        private ComboBox TaoComboThayThe(TextBox tb)
        {
            ComboBox cb = new ComboBox();
            cb.Font = tb.Font;
            cb.DropDownStyle = ComboBoxStyle.DropDownList;
            cb.Location = tb.Location;
            cb.Width = tb.Width;
            cb.Anchor = tb.Anchor;
            tb.Parent.Controls.Add(cb);
            cb.BringToFront();
            tb.Visible = false;
            return cb;
        }

        // Thêm nhãn + ô "Giá nhập" ngay dưới ô nhà cung cấp
        private void TaoOGiaNhap()
        {
            Control cha = textBox6.Parent;

            Label nhanMau = null;
            foreach (Control c in cha.Controls)
            {
                Label l = c as Label;
                if (l != null && l.Text.Trim() == "Đơn giá") { nhanMau = l; break; }
            }

            lblGiaNhap = new Label();
            lblGiaNhap.AutoSize = true;
            lblGiaNhap.Text = "Giá nhập";
            if (nhanMau != null)
            {
                lblGiaNhap.Font = nhanMau.Font;
                lblGiaNhap.ForeColor = nhanMau.ForeColor;
            }
            lblGiaNhap.Location = new Point(textBox6.Left, textBox7.Bottom + 6);

            txtGiaNhap = new TextBox();
            txtGiaNhap.Font = textBox6.Font;
            txtGiaNhap.Width = textBox6.Width;
            txtGiaNhap.Location = new Point(textBox6.Left, lblGiaNhap.Top + lblGiaNhap.PreferredHeight + 2);

            cha.Controls.Add(lblGiaNhap);
            cha.Controls.Add(txtGiaNhap);
        }

        private void NapDanhMuc()
        {
            cboDanhMuc.DataSource = DanhMucDAL.TimKiem("");
            cboDanhMuc.DisplayMember = "TenDanhMuc";
            cboDanhMuc.ValueMember = "MaDanhMuc";
        }

        private void NapNhaCungCap()
        {
            DataTable nguon = NhaCungCapDAL.TimKiem("");
            DataTable ncc = new DataTable();
            ncc.Columns.Add("Ma", typeof(int));
            ncc.Columns.Add("Hien", typeof(string));
            ncc.Rows.Add(DBNull.Value, "(không chọn)");
            foreach (DataRow r in nguon.Rows)
                ncc.Rows.Add(Convert.ToInt32(r["MaNCC"]), r["MaNCC"] + " - " + r["TenNCC"]);

            cboNCC.DataSource = ncc;
            cboNCC.DisplayMember = "Hien";
            cboNCC.ValueMember = "Ma";
        }

        // Chế độ thêm: cho nhập nhà cung cấp và giá nhập; chế độ xem/sửa: chỉ hiển thị
        private void DatCheDo(bool them)
        {
            _dangThem = them;
            btnThem.Text = them ? "Xác nhận thêm" : "Thêm";
            cboNCC.Enabled = them;
            txtGiaNhap.ReadOnly = !them;
        }

        // ================= TÌM KIẾM / LỌC =================

        // Lấy dữ liệu từ SQL theo từ khóa (tên hoặc mã), rồi áp bộ lọc còn lại
        private void LoadData()
        {
            _dt = SanPhamDAL.TimKiem(txtTimKiem.Text.Trim());
            dataGridView1.DataSource = _dt.DefaultView;
            LocLai(null, null);
        }

        // Lọc tại chỗ: đơn giá tối đa (ô giữa) và mã nhà cung cấp (ô phải)
        private void LocLai(object sender, EventArgs e)
        {
            if (_dt == null) return;

            List<string> dk = new List<string>();

            string giaStr = txtLocGia.Text.Trim();
            if (giaStr != "")
            {
                decimal gia;
                if (TryParseTien(giaStr, out gia))
                    dk.Add("GiaBan <= " + gia.ToString(CultureInfo.InvariantCulture));
                else
                    dk.Add("1 = 0");
            }

            string ncc = txtLocNCC.Text.Trim();
            if (ncc != "")
            {
                List<string> ids = new List<string>();
                foreach (DataRow r in _dt.Rows)
                {
                    foreach (string p in r["DanhSachNCC"].ToString().Split(','))
                    {
                        if (p.Trim() == ncc) { ids.Add(r["MaSP"].ToString()); break; }
                    }
                }
                dk.Add(ids.Count == 0 ? "1 = 0" : "MaSP IN (" + string.Join(",", ids) + ")");
            }

            _dt.DefaultView.RowFilter = string.Join(" AND ", dk);
            if (!_dangThem) CapNhatKhungPhai();
        }

        private static bool TryParseTien(string s, out decimal v)
        {
            s = s.Trim();
            return decimal.TryParse(s, NumberStyles.Number, CultureInfo.CurrentCulture, out v)
                || decimal.TryParse(s, NumberStyles.Number, CultureInfo.InvariantCulture, out v);
        }

        private void btnTim_Click(object sender, EventArgs e)
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
                btnTim_Click(sender, EventArgs.Empty);
            }
        }

        // ================= XEM THÔNG TIN =================

        // Bấm vào dòng nào thì hiện thông tin sang khung bên phải
        private void dataGridView1_SelectionChanged(object sender, EventArgs e)
        {
            if (_sanSang && !_dangThem) CapNhatKhungPhai();
        }

        // Bấm vào một dòng khi đang thêm mới thì hủy chế độ thêm
        private void dataGridView1_CellClick(object sender, DataGridViewCellEventArgs e)
        {
            if (_sanSang && _dangThem && e.RowIndex >= 0)
            {
                DatCheDo(false);
                CapNhatKhungPhai();
            }
        }

        private void CapNhatKhungPhai()
        {
            DataRowView r = (dataGridView1.CurrentRow == null) ? null
                            : dataGridView1.CurrentRow.DataBoundItem as DataRowView;
            if (r == null)
            {
                LamTrongO();
                return;
            }

            _maSP = Convert.ToInt32(r["MaSP"]);
            textBox4.Text = r["MaSP"].ToString();
            textBox5.Text = r["TenSP"].ToString();
            textBox6.Text = Convert.ToDecimal(r["GiaBan"]).ToString("N0");

            int idxDM = cboDanhMuc.FindStringExact(r["TenDanhMuc"].ToString());
            cboDanhMuc.SelectedIndex = idxDM >= 0 ? idxDM : (cboDanhMuc.Items.Count > 0 ? 0 : -1);

            // Nhà cung cấp: hiển thị nhà cung cấp đầu tiên của sản phẩm (nếu có)
            string dsNCC = r["DanhSachNCC"].ToString();
            string dau = dsNCC.Split(',')[0].Trim();
            int maNCC;
            if (dau != "" && int.TryParse(dau, out maNCC))
            {
                cboNCC.SelectedValue = maNCC;
                if (cboNCC.SelectedIndex < 0) cboNCC.SelectedIndex = 0;
            }
            else if (cboNCC.Items.Count > 0)
                cboNCC.SelectedIndex = 0;

            try { txtGiaNhap.Text = SanPhamDAL.LayGiaNhap(_maSP).ToString("N0"); }
            catch { txtGiaNhap.Clear(); }
        }

        private void LamTrongO()
        {
            _maSP = -1;
            textBox4.Clear();
            textBox5.Clear();
            textBox6.Clear();
            txtGiaNhap.Clear();
            if (cboDanhMuc.Items.Count > 0) cboDanhMuc.SelectedIndex = 0;
            if (cboNCC.Items.Count > 0) cboNCC.SelectedIndex = 0;
        }

        // ================= THÊM SẢN PHẨM =================
        // Lần bấm 1: xóa trống các ô để nhập. Lần bấm 2 ("Xác nhận thêm"): lưu vào CSDL.

        private void btnThem_Click(object sender, EventArgs e)
        {
            if (!_dangThem)
            {
                DatCheDo(true);
                LamTrongO();
                textBox4.Text = "(tự động)";
                textBox5.Focus();
                return;
            }

            string ten; int maDM; decimal giaBan;
            if (!DocForm(out ten, out maDM, out giaBan)) return;

            decimal giaNhap;
            if (!TryParseTien(txtGiaNhap.Text, out giaNhap) || giaNhap < 0)
            {
                MessageBox.Show("Giá nhập phải là số không âm.");
                txtGiaNhap.Focus();
                return;
            }

            if (giaBan < giaNhap &&
                MessageBox.Show("Giá bán đang thấp hơn giá nhập. Vẫn thêm sản phẩm?", "Cảnh báo",
                    MessageBoxButtons.YesNo, MessageBoxIcon.Warning) != DialogResult.Yes) return;

            int? maNCC = (cboNCC.SelectedValue == null || cboNCC.SelectedValue is DBNull)
                         ? (int?)null : Convert.ToInt32(cboNCC.SelectedValue);

            try
            {
                int maMoi = SanPhamDAL.Them(ten, maDM, giaNhap, giaBan, maNCC);

                DatCheDo(false);
                txtTimKiem.Clear(); txtLocGia.Clear(); txtLocNCC.Clear();
                LoadData();
                ChonDong(maMoi);
                MessageBox.Show("Đã thêm sản phẩm.");
            }
            catch (Exception ex)
            {
                MessageBox.Show("Lỗi: " + ex.Message);
            }
        }

        // ================= CHỈNH SỬA THÔNG TIN =================
        // Sửa tên, danh mục, giá bán của sản phẩm đang chọn.

        private void btnLuu_Click(object sender, EventArgs e)
        {
            if (_dangThem)
            {
                MessageBox.Show("Bạn đang thêm sản phẩm mới. Bấm \"Xác nhận thêm\" để lưu, "
                              + "hoặc bấm vào một dòng trong danh sách để hủy.");
                return;
            }
            if (_maSP < 0)
            {
                MessageBox.Show("Hãy chọn một sản phẩm trong danh sách.");
                return;
            }

            string ten; int maDM; decimal giaBan;
            if (!DocForm(out ten, out maDM, out giaBan)) return;

            decimal giaNhap = 0;
            TryParseTien(txtGiaNhap.Text, out giaNhap);
            string hoi = "Lưu thay đổi cho sản phẩm này?";
            MessageBoxIcon bieuTuong = MessageBoxIcon.Question;
            if (giaBan < giaNhap)
            {
                hoi = "Giá bán đang thấp hơn giá nhập. Vẫn lưu thay đổi?";
                bieuTuong = MessageBoxIcon.Warning;
            }
            if (MessageBox.Show(hoi, "Xác nhận", MessageBoxButtons.YesNo, bieuTuong) != DialogResult.Yes) return;

            try
            {
                int giu = _maSP;
                SanPhamDAL.Sua(giu, ten, maDM, giaBan);
                LoadData();
                ChonDong(giu);
                MessageBox.Show("Đã lưu thay đổi.");
            }
            catch (Exception ex)
            {
                MessageBox.Show("Lỗi: " + ex.Message);
            }
        }

        // Đọc và kiểm tra tên, danh mục, giá bán từ khung bên phải
        private bool DocForm(out string ten, out int maDM, out decimal giaBan)
        {
            ten = textBox5.Text.Trim();
            maDM = 0;
            giaBan = 0;

            if (ten == "")
            {
                MessageBox.Show("Nhập tên sản phẩm.");
                textBox5.Focus();
                return false;
            }
            if (!TryParseTien(textBox6.Text, out giaBan) || giaBan < 0)
            {
                MessageBox.Show("Đơn giá phải là số không âm.");
                textBox6.Focus();
                return false;
            }
            if (cboDanhMuc.SelectedValue == null)
            {
                MessageBox.Show("Chọn danh mục.");
                return false;
            }
            maDM = Convert.ToInt32(cboDanhMuc.SelectedValue);
            return true;
        }

        // ================= NGỪNG KINH DOANH (TrangThai = 0) =================

        private void btnXoa_Click(object sender, EventArgs e)
        {
            if (_dangThem || _maSP < 0)
            {
                MessageBox.Show("Hãy chọn một sản phẩm trong danh sách để ngừng kinh doanh.");
                return;
            }

            if (MessageBox.Show("Ngừng kinh doanh sản phẩm \"" + textBox5.Text + "\"?\n"
                + "Sản phẩm sẽ không còn hiện trong danh sách (hóa đơn cũ vẫn giữ nguyên).",
                "Xác nhận", MessageBoxButtons.YesNo, MessageBoxIcon.Warning) != DialogResult.Yes) return;

            try
            {
                SanPhamDAL.NgungBan(_maSP);
                LoadData();
                MessageBox.Show("Đã ngừng kinh doanh sản phẩm.");
            }
            catch (Exception ex)
            {
                MessageBox.Show("Lỗi: " + ex.Message);
            }
        }

        // Chọn lại dòng có mã sản phẩm cho trước sau khi tải lại danh sách
        private void ChonDong(int maSP)
        {
            foreach (DataGridViewRow row in dataGridView1.Rows)
            {
                DataRowView r = row.DataBoundItem as DataRowView;
                if (r != null && Convert.ToInt32(r["MaSP"]) == maSP)
                {
                    row.Selected = true;
                    dataGridView1.CurrentCell = row.Cells[0];
                    return;
                }
            }
        }
    }
}