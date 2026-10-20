using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Data.SqlClient;
using System.Drawing;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace CNPM1
{
    public partial class frmKhuyenMai : Form
    {
        private const string TT_DANG = "Đang áp dụng";
        private const string TT_SAP = "Sắp diễn ra";

        private DataTable _dt;                 // dữ liệu lấy từ SQL
        private bool _dangThem = false;        // true khi đang tạo khuyến mãi mới
        private int _maKM = -1;                // mã khuyến mãi của dòng đang chọn
        private string _tenKM = "";

        // Các ô nhập (dùng lại ô sẵn có trong Designer, thêm vài ô mới)
        private TextBox txtMa, txtTen, txtMuc, txtDieuKien, txtMoTa;
        private DateTimePicker dtpBatDau, dtpKetThuc;

        public frmKhuyenMai()
        {
            InitializeComponent();
            Load += frmKhuyenMai_Load;
        }

        // ================= KHỞI TẠO =================
        private void frmKhuyenMai_Load(object sender, EventArgs e)
        {
            try
            {
                DungGiaoDien();
                CauHinhLuoi();

                button1.Click += button1_Click;           // Tìm kiếm
                textBox1.KeyDown += TimKiem_KeyDown;
                button2.Click += btnThem_Click;           // Thêm / Xác nhận thêm
                button3.Click += btnLuu_Click;            // Lưu
                button4.Click += btnXoa_Click;            // Xóa / Hủy
                dataGridView1.SelectionChanged += dataGridView1_SelectionChanged;
                dataGridView1.CellDoubleClick += dataGridView1_CellDoubleClick;
                dataGridView1.CellFormatting += dataGridView1_CellFormatting;

                DatCheDo(false);
                LoadData();
            }
            catch (Exception ex)
            {
                MessageBox.Show("Lỗi tải dữ liệu: " + ex.Message, "Lỗi",
                                MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        // Các hàm sự kiện rỗng mà Designer đã gắn sẵn (phải giữ để build được)
        private void textBox6_TextChanged(object sender, EventArgs e) { }
        private void textBox4_TextChanged(object sender, EventArgs e) { }
        private void label7_Click(object sender, EventArgs e) { }
        private void label5_Click(object sender, EventArgs e) { }

        private Label TaoNhan(string chu, int x, int y)
        {
            Label l = new Label();
            l.AutoSize = true;
            l.Font = label8.Font;
            l.ForeColor = label8.ForeColor;
            l.Text = chu;
            l.Location = new Point(x, y);
            panel1.Controls.Add(l);
            return l;
        }

        private DateTimePicker TaoNgay(int x, int y)
        {
            DateTimePicker d = new DateTimePicker();
            d.Font = textBox6.Font;
            d.Format = DateTimePickerFormat.Custom;
            d.CustomFormat = "dd/MM/yyyy";
            d.Location = new Point(x, y);
            d.Width = 142;
            panel1.Controls.Add(d);
            return d;
        }

        // Sắp xếp lại khung bên phải cho khuyến mãi (mọi ô nằm phía trên nút Lưu, y < 450)
        private void DungGiaoDien()
        {
            label1.Text = "Nhập tên hoặc mã khuyến mãi";

            // Dùng lại các nhãn và ô sẵn có
            label5.Text = "Mã khuyến mãi";
            label5.Location = new Point(17, 60);
            txtMa = textBox4;
            txtMa.Location = new Point(21, 84);
            txtMa.ReadOnly = true;                       // mã do SQL tự tăng

            label6.Text = "Tên khuyến mãi";
            label6.Location = new Point(17, 116);
            txtTen = textBox5;
            txtTen.Location = new Point(21, 140);
            txtTen.MaxLength = 150;

            label7.Text = "Mức giảm (10% hoặc 50000đ)";
            label7.Location = new Point(17, 172);
            txtMuc = textBox6;
            txtMuc.Location = new Point(21, 196);

            // Hàng ngày: Từ ngày | Đến ngày
            TaoNhan("Từ ngày", 17, 228);
            TaoNhan("Đến ngày", 173, 228);
            dtpBatDau = TaoNgay(21, 252);
            dtpKetThuc = TaoNgay(173, 252);

            label8.Text = "Điều kiện áp dụng (ghi chú)";
            label8.Location = new Point(17, 284);
            txtDieuKien = textBox7;
            txtDieuKien.Location = new Point(21, 308);
            txtDieuKien.MaxLength = 255;

            TaoNhan("Mô tả", 17, 340);
            txtMoTa = new TextBox();
            txtMoTa.Font = textBox7.Font;
            txtMoTa.BorderStyle = textBox7.BorderStyle;
            txtMoTa.Multiline = true;
            txtMoTa.ScrollBars = ScrollBars.Vertical;
            txtMoTa.MaxLength = 500;
            txtMoTa.Location = new Point(21, 364);
            txtMoTa.Size = new Size(292, 44);
            panel1.Controls.Add(txtMoTa);

            Label ghiChu = new Label();
            ghiChu.AutoSize = false;
            ghiChu.Font = new Font("Segoe UI", 8.25F);
            ghiChu.ForeColor = Color.DimGray;
            ghiChu.Location = new Point(17, 412);
            ghiChu.Size = new Size(305, 34);
            ghiChu.Text = "Áp dụng cho mọi sản phẩm; nhiều khuyến mãi cùng lúc lấy mức cao nhất. "
                        + "Nhấp đúp vào dòng để xem giá sau giảm.";
            panel1.Controls.Add(ghiChu);
        }

        private static DataGridViewTextBoxColumn Cot(string thuocTinh, string tieuDe, float doRong, string dinhDang)
        {
            DataGridViewTextBoxColumn c = new DataGridViewTextBoxColumn();
            c.Name = "col" + thuocTinh;
            c.DataPropertyName = thuocTinh;
            c.HeaderText = tieuDe;
            c.FillWeight = doRong;
            if (dinhDang != null) c.DefaultCellStyle.Format = dinhDang;
            return c;
        }

        private void CauHinhLuoi()
        {
            dataGridView1.AutoGenerateColumns = false;
            dataGridView1.Columns.Clear();
            dataGridView1.Columns.Add(Cot("MaKhuyenMai", "Mã KM", 45, null));
            dataGridView1.Columns.Add(Cot("TenKhuyenMai", "Tên khuyến mãi", 190, null));
            dataGridView1.Columns.Add(Cot("MucGiam", "Mức giảm", 75, null));
            dataGridView1.Columns.Add(Cot("ThoiGian", "Thời gian áp dụng", 150, null));
            dataGridView1.Columns.Add(Cot("TinhTrang", "Tình trạng", 90, null));

            dataGridView1.ReadOnly = true;
            dataGridView1.AllowUserToAddRows = false;
            dataGridView1.MultiSelect = false;
            dataGridView1.SelectionMode = DataGridViewSelectionMode.FullRowSelect;
            dataGridView1.AutoSizeColumnsMode = DataGridViewAutoSizeColumnsMode.Fill;
        }

        // Tô màu cột Tình trạng
        private void dataGridView1_CellFormatting(object sender, DataGridViewCellFormattingEventArgs e)
        {
            if (e.RowIndex < 0 || e.ColumnIndex < 0) return;
            if (dataGridView1.Columns[e.ColumnIndex].DataPropertyName != "TinhTrang") return;

            string t = Convert.ToString(e.Value);
            if (t == TT_DANG) e.CellStyle.ForeColor = Color.Green;
            else if (t == TT_SAP) e.CellStyle.ForeColor = Color.DarkOrange;
            else e.CellStyle.ForeColor = Color.Gray;
        }

        // Chế độ thêm: nút Thêm thành "Xác nhận thêm", nút Xóa thành "Hủy"
        private void DatCheDo(bool them)
        {
            _dangThem = them;
            button2.Text = them ? "Xác nhận thêm" : "+ Thêm";
            button4.Text = them ? "Hủy" : "Xóa";
        }

        // ================= TÌM KIẾM =================

        // Lấy dữ liệu từ SQL theo từ khóa (mã hoặc tên khuyến mãi)
        private void LoadData()
        {
            _dt = KhuyenMaiDAL.TimKiem(textBox1.Text.Trim());
            dataGridView1.DataSource = _dt;
            if (!_dangThem) CapNhatKhungPhai();
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

        // ================= XEM THÔNG TIN =================

        private DataRowView LayDong()
        {
            return dataGridView1.CurrentRow == null ? null
                   : dataGridView1.CurrentRow.DataBoundItem as DataRowView;
        }

        // Bấm vào dòng nào thì hiện thông tin sang khung bên phải
        private void dataGridView1_SelectionChanged(object sender, EventArgs e)
        {
            if (!_dangThem) CapNhatKhungPhai();
        }

        private void CapNhatKhungPhai()
        {
            DataRowView r = LayDong();
            if (r == null)
            {
                LamTrongO();
                return;
            }

            _maKM = Convert.ToInt32(r["MaKhuyenMai"]);
            _tenKM = r["TenKhuyenMai"].ToString();

            txtMa.Text = _maKM.ToString();
            txtTen.Text = _tenKM;

            bool phanTram = r["LoaiGiam"].ToString() == "PhanTram";
            decimal gt = Convert.ToDecimal(r["GiaTriGiam"]);
            txtMuc.Text = phanTram
                ? gt.ToString("0.##", CultureInfo.InvariantCulture) + "%"
                : gt.ToString("N0");

            dtpBatDau.Value = Convert.ToDateTime(r["NgayBatDau"]).Date;
            dtpKetThuc.Value = Convert.ToDateTime(r["NgayKetThuc"]).Date;
            txtDieuKien.Text = r["DieuKienApDung"].ToString();
            txtMoTa.Text = r["MoTa"].ToString();
        }

        private void LamTrongO()
        {
            _maKM = -1;
            _tenKM = "";
            txtMa.Clear();
            txtTen.Clear();
            txtMuc.Clear();
            dtpBatDau.Value = DateTime.Today;
            dtpKetThuc.Value = DateTime.Today.AddDays(30);
            txtDieuKien.Clear();
            txtMoTa.Clear();
        }

        // ================= TẠO KHUYẾN MÃI =================
        // Lần bấm 1: xóa trống để nhập. Lần bấm 2 ("Xác nhận thêm"): lưu vào CSDL.

        private void btnThem_Click(object sender, EventArgs e)
        {
            if (!_dangThem)
            {
                DatCheDo(true);
                LamTrongO();
                txtMa.Text = "(tự động)";
                txtTen.Focus();
                return;
            }

            string ten, moTa, loai, dieuKien;
            decimal giaTri;
            DateTime bd, kt;
            if (!DocForm(-1, out ten, out moTa, out loai, out giaTri, out bd, out kt, out dieuKien)) return;

            try
            {
                int maMoi = KhuyenMaiDAL.Them(ten, moTa, loai, giaTri, bd, kt, dieuKien);

                DatCheDo(false);
                textBox1.Clear();
                LoadData();
                ChonDong(maMoi);
                MessageBox.Show("Đã tạo khuyến mãi.");
            }
            catch (Exception ex)
            {
                MessageBox.Show("Lỗi: " + ex.Message);
            }
        }

        // ================= CHỈNH SỬA KHUYẾN MÃI =================

        private void btnLuu_Click(object sender, EventArgs e)
        {
            if (_dangThem)
            {
                MessageBox.Show("Bạn đang tạo khuyến mãi mới. Bấm \"Xác nhận thêm\" để lưu, hoặc \"Hủy\".");
                return;
            }
            if (_maKM < 0)
            {
                MessageBox.Show("Hãy chọn một khuyến mãi trong danh sách.");
                return;
            }

            string ten, moTa, loai, dieuKien;
            decimal giaTri;
            DateTime bd, kt;
            if (!DocForm(_maKM, out ten, out moTa, out loai, out giaTri, out bd, out kt, out dieuKien)) return;

            try
            {
                string hoi = "Lưu thay đổi cho khuyến mãi này?";
                int soHD = KhuyenMaiDAL.DemHoaDon(_maKM);
                if (soHD > 0)
                    hoi = "Khuyến mãi này đã được dùng trong " + soHD + " hóa đơn.\n"
                        + "Hóa đơn cũ giữ nguyên số tiền giảm đã tính; thay đổi chỉ áp dụng cho đơn mới.\n\n"
                        + "Lưu thay đổi?";

                if (MessageBox.Show(hoi, "Xác nhận",
                    MessageBoxButtons.YesNo, MessageBoxIcon.Question) != DialogResult.Yes) return;

                int giu = _maKM;
                KhuyenMaiDAL.Sua(giu, ten, moTa, loai, giaTri, bd, kt, dieuKien);
                LoadData();
                ChonDong(giu);
                MessageBox.Show("Đã lưu thay đổi.");
            }
            catch (Exception ex)
            {
                MessageBox.Show("Lỗi: " + ex.Message);
            }
        }

        // Đọc số: phần trăm cho phép số lẻ (10 hoặc 7.5); số tiền là số nguyên (bỏ dấu ngăn cách)
        private static bool DocSo(string s, bool laPhanTram, out decimal v)
        {
            v = 0;
            s = new string((s ?? "").Where(c => char.IsDigit(c) || c == '.' || c == ',').ToArray());
            if (s == "") return false;

            if (laPhanTram)
            {
                s = s.Replace(',', '.');
                return decimal.TryParse(s, NumberStyles.AllowDecimalPoint, CultureInfo.InvariantCulture, out v);
            }
            s = s.Replace(".", "").Replace(",", "");
            return decimal.TryParse(s, NumberStyles.None, CultureInfo.InvariantCulture, out v);
        }

        // Đọc và kiểm tra dữ liệu nhập; loaiTruMa = mã đang sửa (-1 khi thêm mới)
        private bool DocForm(int loaiTruMa, out string ten, out string moTa, out string loai,
                             out decimal giaTri, out DateTime bd, out DateTime kt, out string dieuKien)
        {
            ten = txtTen.Text.Trim();
            string mt = txtMoTa.Text.Trim();
            moTa = (mt == "") ? null : mt;
            string dk = txtDieuKien.Text.Trim();
            dieuKien = (dk == "") ? null : dk;
            loai = "PhanTram";
            giaTri = 0;
            bd = dtpBatDau.Value.Date;
            kt = dtpKetThuc.Value.Date;

            if (ten == "")
            {
                MessageBox.Show("Nhập tên khuyến mãi.");
                txtTen.Focus();
                return false;
            }

            string s = txtMuc.Text.Trim();
            if (s == "")
            {
                MessageBox.Show("Nhập mức giảm (ví dụ 10% hoặc 50000).");
                txtMuc.Focus();
                return false;
            }

            bool laPhanTram = s.Contains("%");
            loai = laPhanTram ? "PhanTram" : "SoTien";

            decimal v;
            if (!DocSo(s, laPhanTram, out v))
            {
                MessageBox.Show(laPhanTram
                    ? "Mức giảm phần trăm không hợp lệ (ví dụ 10% hoặc 7.5%)."
                    : "Mức giảm tiền không hợp lệ (ví dụ 50000).");
                txtMuc.Focus();
                return false;
            }
            if (laPhanTram) v = Math.Round(v, 2);
            if (v <= 0)
            {
                MessageBox.Show("Mức giảm phải lớn hơn 0.");
                txtMuc.Focus();
                return false;
            }
            if (laPhanTram && v > 100)
            {
                MessageBox.Show("Phần trăm giảm không được vượt quá 100%.");
                txtMuc.Focus();
                return false;
            }
            if (!laPhanTram && v < 1000 &&
                MessageBox.Show("Bạn nhập giảm " + v.ToString("N0")
                    + " đồng. Nếu muốn giảm theo phần trăm hãy gõ kèm dấu %.\nVẫn tiếp tục?",
                    "Xác nhận", MessageBoxButtons.YesNo, MessageBoxIcon.Question) != DialogResult.Yes)
                return false;
            giaTri = v;

            if (kt < bd)
            {
                MessageBox.Show("Ngày kết thúc phải từ ngày bắt đầu trở đi.");
                dtpKetThuc.Focus();
                return false;
            }
            if (kt < DateTime.Today &&
                MessageBox.Show("Ngày kết thúc đã qua nên khuyến mãi sẽ ở trạng thái \"Đã hết hạn\". Vẫn lưu?",
                    "Cảnh báo", MessageBoxButtons.YesNo, MessageBoxIcon.Warning) != DialogResult.Yes)
                return false;

            try
            {
                if (KhuyenMaiDAL.TrungTen(ten, loaiTruMa))
                {
                    MessageBox.Show("Đã có khuyến mãi tên \"" + ten + "\". Hãy chọn tên khác.");
                    txtTen.Focus();
                    return false;
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show("Lỗi: " + ex.Message);
                return false;
            }
            return true;
        }

        // ================= XÓA KHUYẾN MÃI / HỦY THÊM =================

        private void btnXoa_Click(object sender, EventArgs e)
        {
            // Đang ở chế độ thêm: nút này là "Hủy"
            if (_dangThem)
            {
                DatCheDo(false);
                CapNhatKhungPhai();
                return;
            }

            if (_maKM < 0)
            {
                MessageBox.Show("Hãy chọn một khuyến mãi trong danh sách để xóa.");
                return;
            }

            int ma = _maKM;
            string ten = _tenKM;

            if (MessageBox.Show("Xóa khuyến mãi \"" + ten + "\"?\nCác đơn mới sẽ không còn được giảm theo khuyến mãi này.",
                "Xác nhận xóa", MessageBoxButtons.YesNo, MessageBoxIcon.Warning) != DialogResult.Yes) return;

            try
            {
                KhuyenMaiDAL.Xoa(ma);
                LoadData();
                MessageBox.Show("Đã xóa khuyến mãi.");
            }
            catch (SqlException ex) when (ex.Number == 547)
            {
                // Đã dùng trong hóa đơn: không xóa hẳn được, đề nghị ngừng áp dụng
                if (MessageBox.Show("Khuyến mãi \"" + ten + "\" đã được dùng trong hóa đơn nên không thể xóa hẳn.\n\n"
                    + "Ngừng áp dụng khuyến mãi này thay thế? (Hóa đơn cũ giữ nguyên.)",
                    "Không thể xóa", MessageBoxButtons.YesNo, MessageBoxIcon.Question) != DialogResult.Yes) return;

                try
                {
                    KhuyenMaiDAL.NgungApDung(ma);
                    LoadData();
                    MessageBox.Show("Đã ngừng áp dụng khuyến mãi.");
                }
                catch (Exception ex2)
                {
                    MessageBox.Show("Lỗi: " + ex2.Message);
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show("Lỗi: " + ex.Message);
            }
        }

        // ================= XEM GIÁ SẢN PHẨM SAU KHI GIẢM =================

        // Nhấp đúp vào dòng: liệt kê sản phẩm với giá sau giảm (tính riêng khuyến mãi này)
        private void dataGridView1_CellDoubleClick(object sender, DataGridViewCellEventArgs e)
        {
            if (e.RowIndex < 0 || _dangThem || _maKM < 0) return;

            try
            {
                DataTable sp = KhuyenMaiDAL.SanPhamApDung(_maKM);

                Form f = new Form();
                f.Text = "Khuyến mãi \"" + _tenKM + "\": giá sau giảm (tính riêng khuyến mãi này) - "
                       + sp.Rows.Count + " sản phẩm";
                f.StartPosition = FormStartPosition.CenterParent;
                f.Size = new Size(780, 420);

                DataGridView g = new DataGridView();
                g.Dock = DockStyle.Fill;
                g.ReadOnly = true;
                g.AllowUserToAddRows = false;
                g.SelectionMode = DataGridViewSelectionMode.FullRowSelect;
                g.AutoSizeColumnsMode = DataGridViewAutoSizeColumnsMode.Fill;
                g.AutoGenerateColumns = false;
                g.Columns.Add(Cot("MaSP", "Mã sản phẩm", 60, null));
                g.Columns.Add(Cot("TenSP", "Tên sản phẩm", 200, null));
                g.Columns.Add(Cot("GiaGoc", "Giá gốc", 80, "N0"));
                g.Columns.Add(Cot("TienGiam", "Giảm", 70, "N0"));
                g.Columns.Add(Cot("GiaSauGiam", "Giá sau giảm", 80, "N0"));

                f.Controls.Add(g);
                g.DataSource = sp;

                f.ShowDialog(this);
                f.Dispose();
            }
            catch (Exception ex)
            {
                MessageBox.Show("Lỗi: " + ex.Message);
            }
        }

        // Chọn lại dòng có mã khuyến mãi cho trước sau khi tải lại danh sách
        private void ChonDong(int maKM)
        {
            foreach (DataGridViewRow row in dataGridView1.Rows)
            {
                DataRowView r = row.DataBoundItem as DataRowView;
                if (r != null && Convert.ToInt32(r["MaKhuyenMai"]) == maKM)
                {
                    row.Selected = true;
                    dataGridView1.CurrentCell = row.Cells[0];
                    return;
                }
            }
        }
    }
}