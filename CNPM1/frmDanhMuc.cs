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
    public partial class frmDanhMuc : Form
    {
        private DataTable _dt;              // dữ liệu lấy từ SQL
        private bool _dangThem = false;     // true khi đang nhập danh mục mới
        private bool _sanSang = false;      // true khi đã dựng xong giao diện
        private int _maDM = -1;             // mã danh mục của dòng đang chọn

        // Các control tìm được trên form
        private TextBox txtTim;
        private Button btnTim, btnLuu, btnThem, btnKhoa;

        // Nút do code tự tạo
        private Button btnXem;

        public frmDanhMuc()
        {
            InitializeComponent();
            Load += frmDanhMuc_Load;
            dataGridView1.SelectionChanged += dataGridView1_SelectionChanged;
            dataGridView1.CellClick += dataGridView1_CellClick;
            dataGridView1.CellDoubleClick += dataGridView1_CellDoubleClick;
        }

        // ================= KHỞI TẠO =================
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

                // Tìm nút theo chữ trên nút (không phụ thuộc tên control)
                btnTim = TimNut(this, "Tìm kiếm");
                btnLuu = TimNut(this, "Lưu");
                btnThem = TimNut(this, "Thêm");
                btnKhoa = TimNut(this, "Xóa");
                if (btnTim == null || btnLuu == null || btnThem == null || btnKhoa == null)
                {
                    MessageBox.Show("Không tìm thấy các nút Tìm kiếm / Lưu / Thêm / Xóa trên form.");
                    return;
                }

                // Ô tìm kiếm: ô nhập còn lại ngoài 3 ô của khung bên phải
                List<TextBox> tatCa = new List<TextBox>();
                ThuThapTextBox(this, tatCa);
                txtTim = tatCa
                    .Where(t => t != textBox2 && t != textBox4 && t != textBox5)
                    .OrderBy(t => t.Parent.PointToScreen(t.Location).Y)
                    .ThenBy(t => t.Parent.PointToScreen(t.Location).X)
                    .FirstOrDefault();
                if (txtTim == null)
                {
                    MessageBox.Show("Không tìm thấy ô tìm kiếm trên form.");
                    return;
                }

                // Khung bên phải
                textBox4.ReadOnly = true;       // mã danh mục do SQL tự tăng
                textBox5.MaxLength = 100;       // tên danh mục
                textBox2.MaxLength = 255;       // mô tả
                btnKhoa.Text = "Khóa";

                // Nút "Xem sản phẩm" đặt dưới ô Mô tả
                btnXem = new Button();
                btnXem.Text = "Xem sản phẩm";
                btnXem.Font = btnLuu.Font;
                btnXem.UseVisualStyleBackColor = true;
                btnXem.Size = new Size(textBox2.Width, btnLuu.Height);
                btnXem.Location = new Point(textBox2.Left, textBox2.Bottom + 25);
                textBox2.Parent.Controls.Add(btnXem);

                // Gắn sự kiện
                btnTim.Click += btnTim_Click;
                btnThem.Click += btnThem_Click;
                btnLuu.Click += btnLuu_Click;
                btnKhoa.Click += btnKhoa_Click;
                btnXem.Click += btnXem_Click;
                txtTim.KeyDown += TimKiem_KeyDown;

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

        private void DatCheDo(bool them)
        {
            _dangThem = them;
            btnThem.Text = them ? "Xác nhận thêm" : "Thêm";
        }

        // ================= TÌM KIẾM =================

        // Lấy dữ liệu từ SQL theo từ khóa (mã hoặc tên danh mục)
        private void LoadData()
        {
            _dt = DanhMucDAL.TimKiem(txtTim.Text.Trim());
            dataGridView1.DataSource = _dt;
            if (!_dangThem) CapNhatKhungPhai();
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

        // ================= CHỌN DÒNG =================

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

        // Nhấp đúp vào dòng = xem sản phẩm của danh mục đó
        private void dataGridView1_CellDoubleClick(object sender, DataGridViewCellEventArgs e)
        {
            if (_sanSang && e.RowIndex >= 0) XemSanPham();
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

            _maDM = Convert.ToInt32(r["MaDanhMuc"]);
            textBox4.Text = r["MaDanhMuc"].ToString();
            textBox5.Text = r["TenDanhMuc"].ToString();
            textBox2.Text = r["MoTa"].ToString();
        }

        private void LamTrongO()
        {
            _maDM = -1;
            textBox4.Clear();
            textBox5.Clear();
            textBox2.Clear();
        }

        // ================= THÊM DANH MỤC =================
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

            string ten, moTa;
            if (!DocForm(-1, out ten, out moTa)) return;

            try
            {
                int maMoi = DanhMucDAL.Them(ten, moTa);

                DatCheDo(false);
                txtTim.Clear();
                LoadData();
                ChonDong(maMoi);
                MessageBox.Show("Đã thêm danh mục.");
            }
            catch (Exception ex)
            {
                MessageBox.Show("Lỗi: " + ex.Message);
            }
        }

        // ================= CHỈNH SỬA DANH MỤC =================

        private void btnLuu_Click(object sender, EventArgs e)
        {
            if (_dangThem)
            {
                MessageBox.Show("Bạn đang thêm danh mục mới. Bấm \"Xác nhận thêm\" để lưu, "
                              + "hoặc bấm vào một dòng trong danh sách để hủy.");
                return;
            }
            if (_maDM < 0)
            {
                MessageBox.Show("Hãy chọn một danh mục trong danh sách.");
                return;
            }

            string ten, moTa;
            if (!DocForm(_maDM, out ten, out moTa)) return;

            if (MessageBox.Show("Lưu thay đổi cho danh mục này?", "Xác nhận",
                MessageBoxButtons.YesNo, MessageBoxIcon.Question) != DialogResult.Yes) return;

            try
            {
                int giu = _maDM;
                DanhMucDAL.Sua(giu, ten, moTa);
                LoadData();
                ChonDong(giu);
                MessageBox.Show("Đã lưu thay đổi.");
            }
            catch (Exception ex)
            {
                MessageBox.Show("Lỗi: " + ex.Message);
            }
        }

        // Đọc và kiểm tra tên, mô tả; loaiTruMa = mã đang sửa (-1 khi thêm mới)
        private bool DocForm(int loaiTruMa, out string ten, out string moTa)
        {
            ten = textBox5.Text.Trim();
            string mt = textBox2.Text.Trim();
            moTa = (mt == "") ? null : mt;

            if (ten == "")
            {
                MessageBox.Show("Nhập tên danh mục.");
                textBox5.Focus();
                return false;
            }
            if (ten.Length > 100)
            {
                MessageBox.Show("Tên danh mục tối đa 100 ký tự.");
                textBox5.Focus();
                return false;
            }
            if (mt.Length > 255)
            {
                MessageBox.Show("Mô tả tối đa 255 ký tự.");
                textBox2.Focus();
                return false;
            }

            try
            {
                if (DanhMucDAL.TrungTen(ten, loaiTruMa))
                {
                    MessageBox.Show("Đã có danh mục tên \"" + ten + "\". Hãy chọn tên khác.");
                    textBox5.Focus();
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

        // ================= KHÓA DANH MỤC (TrangThai = 0) =================

        private void btnKhoa_Click(object sender, EventArgs e)
        {
            if (_dangThem || _maDM < 0)
            {
                MessageBox.Show("Hãy chọn một danh mục trong danh sách để khóa.");
                return;
            }

            try
            {
                int soSP = DanhMucDAL.DemSanPham(_maDM);
                if (soSP > 0)
                {
                    MessageBox.Show("Không thể khóa danh mục \"" + textBox5.Text + "\" vì còn "
                        + soSP + " sản phẩm đang kinh doanh.\n"
                        + "Hãy ngừng kinh doanh hoặc chuyển các sản phẩm sang danh mục khác trước.",
                        "Không thể khóa", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                    return;
                }

                if (MessageBox.Show("Khóa danh mục \"" + textBox5.Text + "\"?\n"
                    + "Danh mục bị khóa sẽ không còn hiện trong danh sách.",
                    "Xác nhận khóa", MessageBoxButtons.YesNo, MessageBoxIcon.Warning) != DialogResult.Yes) return;

                DanhMucDAL.Khoa(_maDM);
                LoadData();
                MessageBox.Show("Đã khóa danh mục.");
            }
            catch (Exception ex)
            {
                MessageBox.Show("Lỗi: " + ex.Message);
            }
        }

        // ================= XEM SẢN PHẨM THEO DANH MỤC =================

        private void btnXem_Click(object sender, EventArgs e)
        {
            XemSanPham();
        }

        private void XemSanPham()
        {
            if (_dangThem || _maDM < 0)
            {
                MessageBox.Show("Hãy chọn một danh mục trong danh sách.");
                return;
            }

            try
            {
                DataTable sp = DanhMucDAL.SanPhamTheoDanhMuc(_maDM);
                if (sp.Rows.Count == 0)
                {
                    MessageBox.Show("Danh mục \"" + textBox5.Text + "\" chưa có sản phẩm đang kinh doanh.");
                    return;
                }

                Form f = new Form();
                f.Text = "Sản phẩm thuộc danh mục: " + textBox5.Text + " (" + sp.Rows.Count + ")";
                f.StartPosition = FormStartPosition.CenterParent;
                f.Size = new Size(760, 420);

                DataGridView g = new DataGridView();
                g.Dock = DockStyle.Fill;
                g.ReadOnly = true;
                g.AllowUserToAddRows = false;
                g.SelectionMode = DataGridViewSelectionMode.FullRowSelect;
                g.AutoSizeColumnsMode = DataGridViewAutoSizeColumnsMode.Fill;

                // Tạo cột bằng tay để không phụ thuộc việc tự sinh cột
                g.AutoGenerateColumns = false;
                g.Columns.Add(new DataGridViewTextBoxColumn { DataPropertyName = "MaSP", HeaderText = "Mã sản phẩm" });
                g.Columns.Add(new DataGridViewTextBoxColumn { DataPropertyName = "TenSP", HeaderText = "Tên sản phẩm" });
                g.Columns.Add(new DataGridViewTextBoxColumn
                {
                    DataPropertyName = "GiaBan",
                    HeaderText = "Đơn giá",
                    DefaultCellStyle = new DataGridViewCellStyle { Format = "N0" }
                });
                g.Columns.Add(new DataGridViewTextBoxColumn { DataPropertyName = "SoLuongTon", HeaderText = "Tồn kho" });

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

        // Chọn lại dòng có mã danh mục cho trước sau khi tải lại danh sách
        private void ChonDong(int maDM)
        {
            foreach (DataGridViewRow row in dataGridView1.Rows)
            {
                DataRowView r = row.DataBoundItem as DataRowView;
                if (r != null && Convert.ToInt32(r["MaDanhMuc"]) == maDM)
                {
                    row.Selected = true;
                    dataGridView1.CurrentCell = row.Cells[0];
                    return;
                }
            }
        }

        private void panel3_Paint(object sender, PaintEventArgs e)
        {
        }
    }
}