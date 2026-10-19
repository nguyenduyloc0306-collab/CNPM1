using System;
using System.Data;
using System.Windows.Forms;

namespace CNPM1
{
	public partial class frmQuanLyKho : Form
	{
		// Biến lưu thông tin nhân viên kho đang đăng nhập
		private int maNhanVien => User.MaTK;
		private string tenNhanVien => User.HoTen;

		public frmQuanLyKho()
		{
			InitializeComponent();
		}

		// Hàm khởi tạo nhận tham số từ màn hình chính sau khi đăng nhập thành công
		public frmQuanLyKho(int maTK, string hoTen, string tenVaiTro)
		{
			InitializeComponent();
			User.MaTK = maTK;
			User.HoTen = hoTen;
			User.TenVaiTro = tenVaiTro;
		}

		private void frmQuanLyKho_Load(object sender, EventArgs e)
		{
			// === ĐĂNG KÝ CÁC SỰ KIỆN GIAO DIỆN ===
			tabControl.SelectedIndexChanged += tabControl_SelectedIndexChanged;
			button1.Click += button1_Click; // Nút tìm kiếm
			button3.Click += button3_Click; // Nút lưu
			dataGridView1.CellClick += dataGridView1_CellClick;
			dataGridView2.CellClick += dataGridView2_CellClick;

			// 1. Cấu hình ban đầu cho panel thông tin sản phẩm (panel1):
			textBox4.ReadOnly = true; // Mã sản phẩm
			textBox5.ReadOnly = true; // Tên sản phẩm
			textBox6.ReadOnly = true; // Mô tả
			textBox2.ReadOnly = true; // Số lượng tồn kho
			textBox3.ReadOnly = true; // Mức tối thiểu

			textBox8.ReadOnly = false; // Số lượng thực tế
			textBox9.ReadOnly = false; // Lý do điều chỉnh

			// Khi mới mở form, xóa trắng các ô nhập liệu bên panel1
			XoaTrangPanelSanPham();

			// 2. Cấu hình cho panel lịch sử điều chỉnh (panel2): khóa tất cả các ô chỉ để xem
			SetPanel2ReadOnly(true);

			// Đảm bảo ban đầu tabControl hiển thị tab Sản phẩm và panel1 hiện, panel2 ẩn
			tabControl.SelectedTab = tabSP;
			panel1.Visible = true;
			panel2.Visible = false;

			// Load dữ liệu lên 2 bảng
			LoadDanhSachSanPham("");
			LoadDanhSachLichSu();
		}

		// ================= TẢI DỮ LIỆU LÊN GRIDVIEW =================

		private void LoadDanhSachSanPham(string tuKhoa)
		{
			DataTable dt = QuanLyKhoDAL.LayDanhSachSanPham(tuKhoa);

			dataGridView1.AutoGenerateColumns = false;
			dataGridView1.DataSource = dt;

			dataGridView1.Columns["colMaSP1"].DataPropertyName = "MaSP";
			dataGridView1.Columns["colTenSP"].DataPropertyName = "TenSP";
			dataGridView1.Columns["colSoLuongTonKho"].DataPropertyName = "SoLuongTon";
			dataGridView1.Columns["colMucToiThieu"].DataPropertyName = "MucTonToiThieu";
			dataGridView1.Columns["colMoTa"].DataPropertyName = "MoTa";
		}

		private void LoadDanhSachLichSu()
		{
			DataTable dt = QuanLyKhoDAL.LayDanhSachLichSu();

			dataGridView2.AutoGenerateColumns = false;
			dataGridView2.DataSource = dt;

			dataGridView2.Columns["colMaDieuChinh"].DataPropertyName = "MaDieuChinh";
			dataGridView2.Columns["colMaSP2"].DataPropertyName = "MaSP";
			dataGridView2.Columns["colMaNVK"].DataPropertyName = "TenNhanVienKho";
			dataGridView2.Columns["colSoLuongTon2"].DataPropertyName = "SoLuongHeThong";
			dataGridView2.Columns["colSoLuongThucTe"].DataPropertyName = "SoLuongThucTe";
			dataGridView2.Columns["colNgayDC"].DataPropertyName = "NgayDieuChinh";
			dataGridView2.Columns["colChenhLech"].DataPropertyName = "ChenhLech";
			dataGridView2.Columns["colLydo"].DataPropertyName = "LyDo";
		}

		// ================= XỬ LÝ TAB VÀ CHUYỂN ĐỔI PANEL =================

		private void tabControl_SelectedIndexChanged(object sender, EventArgs e)
		{
			if (tabControl.SelectedTab == tabSP)
			{
				panel1.Visible = true;
				panel1.BringToFront();
				panel2.Visible = false;
			}
			else if (tabControl.SelectedTab == tabLichSu)
			{
				panel1.Visible = false;
				panel2.Visible = true;
				panel2.BringToFront();
				
				// Nạp dữ liệu cho bảng lịch sử
				LoadDanhSachLichSu();
			}
		}

		// ================= XỬ LÝ KHI CHỌN DÒNG TRÊN TAB SẢN PHẨM =================

		private void dataGridView1_CellClick(object sender, DataGridViewCellEventArgs e)
		{
			if (e.RowIndex < 0) return;

			DataGridViewRow row = dataGridView1.Rows[e.RowIndex];

			// Đổ dữ liệu từ dòng được chọn lên panel1 (Thông tin sản phẩm)
			textBox4.Text = row.Cells["colMaSP1"].Value?.ToString() ?? "";
			textBox5.Text = row.Cells["colTenSP"].Value?.ToString() ?? ""; // Đã sửa từ colTenSP1 thành colTenSP
			textBox2.Text = row.Cells["colSoLuongTonKho"].Value?.ToString() ?? "0";
			textBox3.Text = row.Cells["colMucToiThieu"].Value?.ToString() ?? "0";
			textBox6.Text = row.Cells["colMoTa"].Value?.ToString() ?? "";

			// Xóa sạch ô nhập số lượng thực tế và lý do để người dùng nhập mới
			textBox8.Clear();
			textBox9.Clear();
		}

		// ================= XỬ LÝ NÚT LƯU ĐIỀU CHỈNH KHO (BUTTON3) =================

		private void button3_Click(object sender, EventArgs e)
		{
			if (string.IsNullOrEmpty(textBox4.Text.Trim()))
			{
				MessageBox.Show("Vui lòng chọn sản phẩm cần điều chỉnh từ danh sách!", "Thông báo", MessageBoxButtons.OK, MessageBoxIcon.Warning);
				return;
			}

			if (string.IsNullOrEmpty(textBox8.Text.Trim()))
			{
				MessageBox.Show("Vui lòng nhập số lượng thực tế!", "Thông báo", MessageBoxButtons.OK, MessageBoxIcon.Warning);
				textBox8.Focus();
				return;
			}

			if (!int.TryParse(textBox8.Text.Trim(), out int soLuongThucTe) || soLuongThucTe < 0)
			{
				MessageBox.Show("Số lượng thực tế phải là một số nguyên không âm!", "Lỗi định dạng", MessageBoxButtons.OK, MessageBoxIcon.Error);
				textBox8.Focus();
				return;
			}

			int maSP = Convert.ToInt32(textBox4.Text.Trim());
			int soLuongHeThong = Convert.ToInt32(textBox2.Text.Trim());
			string lyDo = textBox9.Text.Trim();

			bool thanhCong = QuanLyKhoDAL.ThucHienDieuChinh(maSP, maNhanVien, soLuongHeThong, soLuongThucTe, lyDo);

			if (thanhCong)
			{
				MessageBox.Show("Lưu điều chỉnh kho thành công!", "Thông báo", MessageBoxButtons.OK, MessageBoxIcon.Information);

				XoaTrangPanelSanPham();
				LoadDanhSachSanPham("");
				LoadDanhSachLichSu();
			}
			else
			{
				MessageBox.Show("Có lỗi xảy ra khi lưu dữ liệu vào cơ sở dữ liệu!", "Lỗi", MessageBoxButtons.OK, MessageBoxIcon.Error);
			}
		}

		// ================= XỬ LÝ KHI CHỌN DÒNG TRÊN TAB LỊCH SỬ =================

		private void dataGridView2_CellClick(object sender, DataGridViewCellEventArgs e)
		{
			if (e.RowIndex < 0) return;

			DataGridViewRow row = dataGridView2.Rows[e.RowIndex];

			// Các thông tin lấy trực tiếp từ các cột có sẵn trên giao diện dataGridView2
			textBox17.Text = row.Cells["colMaDieuChinh"].Value?.ToString() ?? "";
			textBox15.Text = row.Cells["colMaNVK"].Value?.ToString() ?? tenNhanVien;
			textBox7.Text = row.Cells["colNgayDC"].Value?.ToString() ?? "";
			textBox13.Text = row.Cells["colSoLuongTon2"].Value?.ToString() ?? "";
			textBox11.Text = row.Cells["colSoLuongThucTe"].Value?.ToString() ?? "";
			textBox10.Text = row.Cells["colLydo"].Value?.ToString() ?? "";

			// Riêng Tên sản phẩm thì lấy ngầm từ nguồn dữ liệu (DataTable) để đổi từ Mã sang Tên
			DataRowView drv = row.DataBoundItem as DataRowView;
			if (drv != null)
			{
				textBox16.Text = drv["TenSP"]?.ToString() ?? "";
			}
		}

		// ================= TÌM KIẾM SẢN PHẨM =================

		private void button1_Click(object sender, EventArgs e)
		{
			string tuKhoa = textBox1.Text.Trim(); // Ô nhập từ khóa chung

			if (tabControl.SelectedTab == tabSP)
			{
				// Tab Sản phẩm: Tìm kiếm theo Mã SP hoặc Tên SP
				dataGridView1.DataSource = QuanLyKhoDAL.LayDanhSachSanPham(tuKhoa);
			}
			else if (tabControl.SelectedTab == tabLichSu)
			{
				// Tab Lịch sử: Chỉ lọc theo Mã SP
				dataGridView2.DataSource = QuanLyKhoDAL.LayDanhSachLichSu(tuKhoa);
			}
		}

		// ================= CÁC HÀM HỖ TRỢ PHỤ =================

		private void XoaTrangPanelSanPham()
		{
			textBox4.Clear();
			textBox5.Clear();
			textBox6.Clear();
			textBox2.Clear();
			textBox3.Clear();
			textBox8.Clear();
			textBox9.Clear();
		}

		private void SetPanel2ReadOnly(bool isReadOnly)
		{
			textBox17.ReadOnly = isReadOnly;
			textBox16.ReadOnly = isReadOnly;
			textBox15.ReadOnly = isReadOnly;
			textBox7.ReadOnly = isReadOnly;
			textBox13.ReadOnly = isReadOnly;
			textBox11.ReadOnly = isReadOnly;
			textBox10.ReadOnly = isReadOnly;
		}

		// Các sự kiện phụ sinh ra từ giao diện
		private void panel1_Paint(object sender, PaintEventArgs e) { }
		private void panel2_Paint(object sender, PaintEventArgs e) { }
		private void textBox9_TextChanged(object sender, EventArgs e) { }
		private void label10_Click(object sender, EventArgs e) { }
		private void label11_Click(object sender, EventArgs e) { }
		private void textBox10_TextChanged(object sender, EventArgs e) { }
		private void textBox11_TextChanged(object sender, EventArgs e) { }
		private void label12_Click(object sender, EventArgs e) { }
		private void textBox13_TextChanged(object sender, EventArgs e) { }
		private void label14_Click(object sender, EventArgs e) { }

		private void label1_Click(object sender, EventArgs e)
		{

		}

		private void label13_Click(object sender, EventArgs e)
		{

		}

		private void label3_Click(object sender, EventArgs e)
		{

		}

		private void textBox2_TextChanged(object sender, EventArgs e)
		{

		}
	}
}