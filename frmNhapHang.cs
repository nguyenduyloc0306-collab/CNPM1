using System;
using System.Data;
using System.Windows.Forms;

namespace CNPM1
{
	public partial class frmNhapHang : Form
	{
		private NhapHangDAL nhapHangDAL;
		private int maPhieuDangTaoOrXem = -1; // Lưu mã phiếu đang thao tác (tạo mới hoặc xem chi tiết)
		private bool dangTaoMoiHoacSua = false; // Phân biệt trạng thái đang chỉnh sửa hay chỉ xem

		public frmNhapHang()
		{
			InitializeComponent();
			nhapHangDAL = new NhapHangDAL();

			// Đăng ký các sự kiện
			this.Load += FrmNhapHang_Load;
			this.btnTaoMoi.Click += BtnTaoMoi_Click;
			this.btnChiTiet.Click += BtnChiTiet_Click;
			this.btnXacNhan.Click += BtnXacNhan_Click;
			this.btnThem.Click += BtnThem_Click;
			this.btnLuu.Click += BtnLuu_Click;
			this.btnHuy.Click += BtnHuy_Click;
			this.btnDong.Click += BtnDong_Click;

			this.comboBoxNCC.SelectedIndexChanged += ComboBoxNCC_SelectedIndexChanged;
			this.comboMaSP.SelectedIndexChanged += ComboMaSP_SelectedIndexChanged;
			this.textBoxSoLuong.TextChanged += TinhThanhTien;
			this.textBoxDonGia.TextChanged += TinhThanhTien;
			this.dataGridViewPhieuNhap.CellClick += DataGridViewPhieuNhap_CellClick;
			this.dataGridViewChiTiet.CellClick += DataGridViewChiTiet_CellClick;
		}

		private void FrmNhapHang_Load(object sender, EventArgs e)
		{
			textBoxMaPhieu.ReadOnly = true;
			textBoxNCC.ReadOnly = true;
			textBoxNVKho.ReadOnly = true;
			textBoxNgayNhap.ReadOnly = true;
			textBoxTongTien.ReadOnly = true;
			textBoxTrangThai.ReadOnly = true;

			HienThiGiaoDienDanhSach();
			LoadDanhSachPhieuNhap();
			LoadComboboxNCC();
		}

		// ================= PHẦN 1: HIỂN THỊ GIAO DIỆN =================

		private void HienThiGiaoDienDanhSach()
		{
			dataGridViewPhieuNhap.Visible = true;
			panelPhieuNhap.Visible = true;
			dataGridViewChiTiet.Visible = false;
			panelChiTiet.Visible = false;

			maPhieuDangTaoOrXem = -1;
			dangTaoMoiHoacSua = false;
		}

		private void HienThiGiaoDienChiTiet()
		{
			dataGridViewPhieuNhap.Visible = false;
			panelPhieuNhap.Visible = false;
			dataGridViewChiTiet.Visible = true;
			panelChiTiet.Visible = true;
		}

		private void LoadDanhSachPhieuNhap()
		{
			DataTable dt = nhapHangDAL.LayDanhSachPhieuNhap();
			dataGridViewPhieuNhap.Rows.Clear();
			foreach (DataRow row in dt.Rows)
			{
				dataGridViewPhieuNhap.Rows.Add(
					row["MaPhieuNhap"],
					row["TenNCC"],
					row["NVKho"],
					Convert.ToDateTime(row["NgayNhap"]).ToString("dd/MM/yyyy HH:mm"),
					row["TongTien"],
					row["TrangThai"]
				);
			}
		}

		private void LoadComboboxNCC()
		{
			DataTable dt = nhapHangDAL.LayDanhSachNCC();
			comboBoxNCC.DataSource = dt;
			comboBoxNCC.DisplayMember = "TenNCC";
			comboBoxNCC.ValueMember = "MaNCC";
			comboBoxNCC.SelectedIndex = -1;
		}

		// Khi click chọn 1 dòng ở danh sách phiếu nhập -> Đưa dữ liệu lên panelThongTinPhieu
		private void DataGridViewPhieuNhap_CellClick(object sender, DataGridViewCellEventArgs e)
		{
			if (e.RowIndex >= 0)
			{
				DataGridViewRow row = dataGridViewPhieuNhap.Rows[e.RowIndex];
				textBoxMaPhieu.Text = row.Cells["colMaPhieu"].Value?.ToString();
				textBoxNCC.Text = row.Cells["colNCC"].Value?.ToString();
				textBoxNVKho.Text = row.Cells["colNVKho"].Value?.ToString();
				textBoxNgayNhap.Text = row.Cells["colNgayNhap"].Value?.ToString();
				textBoxTongTien.Text = row.Cells["colTongTien"].Value?.ToString();

				string trangThai = row.Cells["colTrangThai"].Value?.ToString();
				textBoxTrangThai.Text = trangThai;

				if (int.TryParse(textBoxMaPhieu.Text, out int maPhieu))
				{
					maPhieuDangTaoOrXem = maPhieu;
				}

				// Xử lý ẩn/hiện nút Xác nhận dựa trên trạng thái
				if (trangThai == "Chờ xác nhận")
				{
					btnXacNhan.Enabled = true;
					btnXacNhan.Visible = true; // Hoặc bạn có thể dùng btnXacNhan.Enabled = true; cho mờ đi
				}
				else
				{
					btnXacNhan.Enabled = false; // Hoặc btnXacNhan.Visible = false; nếu muốn ẩn hẳn
				}
			}
		}

		// Biến toàn cục trong form để lưu Mã SP đang được chọn sửa trên lưới chi tiết
		private int maSPDangChon = -1;

		private void DataGridViewChiTiet_CellClick(object sender, DataGridViewCellEventArgs e)
		{
			if (e.RowIndex >= 0)
			{
				DataGridViewRow row = dataGridViewChiTiet.Rows[e.RowIndex];

				string maSP = row.Cells["colMaSP"].Value?.ToString();
				string soLuong = row.Cells["colSoLuong"].Value?.ToString();
				string donGia = row.Cells["colDonGia"].Value?.ToString();
				string thanhTien = row.Cells["colThanhTien"].Value?.ToString();

				if (!string.IsNullOrEmpty(maSP))
				{
					maSPDangChon = Convert.ToInt32(maSP);
					comboMaSP.SelectedValue = maSP;
				}

				textBoxSoLuong.Text = soLuong;
				textBoxDonGia.Text = donGia;
				textBoxThanhTien.Text = thanhTien;
			}
		}

		// ================= PHẦN 2: NGHIỆP VỤ TẠO MỚI =================

		private void BtnTaoMoi_Click(object sender, EventArgs e)
		{
			// Reset trạng thái tạo mới
			maPhieuDangTaoOrXem = -1; // Chưa tạo phiếu trong CSDL cho đến khi thêm sản phẩm đầu tiên
			dangTaoMoiHoacSua = true;

			// Reset giao diện panelChiTiet
			textBoxMaPhieu2.Text = "(Tự động)";
			textBoxMaPhieu2.ReadOnly = true;

			// MỞ KHÓA TẤT CẢ CHO PHÉP NHẬP LIỆU
			comboBoxNCC.Enabled = true;       // Cho phép chọn Nhà cung cấp
			comboBoxNCC.SelectedIndex = -1;   // Reset về chưa chọn

			comboMaSP.Enabled = true;         // Cho phép chọn sản phẩm
			comboMaSP.DataSource = null;

			textBoxSoLuong.ReadOnly = false;  // Cho phép nhập số lượng
			textBoxSoLuong.Clear();

			textBoxDonGia.ReadOnly = false;   // Cho phép nhập/sửa đơn giá (hoặc tự lấy theo sản phẩm)
			textBoxDonGia.Clear();

			textBoxThanhTien.ReadOnly = true;  // Thành tiền luôn tự tính nên giữ khóa
			textBoxThanhTien.Text = "0";

			dataGridViewChiTiet.Rows.Clear();

			// Chuyển sang giao diện chi tiết
			HienThiGiaoDienChiTiet();

			// Hiển thị và bật sáng các nút chức năng
			btnThem.Visible = true;
			btnThem.Enabled = true;

			btnLuu.Visible = true;
			btnLuu.Enabled = true;

			btnHuy.Visible = true;
			btnHuy.Enabled = true;

			btnDong.Visible = false; // Đang tạo mới thì ẩn nút Đóng đi
		}

		// Khi chọn NCC ở panelChiTiet -> Load danh sách sản phẩm tương ứng của NCC đó
		private void ComboBoxNCC_SelectedIndexChanged(object sender, EventArgs e)
		{
			if (comboBoxNCC.SelectedValue != null && int.TryParse(comboBoxNCC.SelectedValue.ToString(), out int maNCC))
			{
				DataTable dt = nhapHangDAL.LaySanPhamTheoNCC(maNCC);
				comboMaSP.DataSource = dt;
				comboMaSP.DisplayMember = "TenSP";
				comboMaSP.ValueMember = "MaSP";
				comboMaSP.SelectedIndex = -1;
			}
		}

		// Khi chọn sản phẩm -> Tự động điền đơn giá nhập của sản phẩm đó
		private void ComboMaSP_SelectedIndexChanged(object sender, EventArgs e)
		{
			if (comboMaSP.SelectedItem is DataRowView row)
			{
				textBoxDonGia.Text = row["GiaNhap"].ToString();
			}
		}

		// Tự động tính thành tiền = số lượng * đơn giá
		private void TinhThanhTien(object sender, EventArgs e)
		{
			if (decimal.TryParse(textBoxSoLuong.Text, out decimal sl) && decimal.TryParse(textBoxDonGia.Text, out decimal dg))
			{
				textBoxThanhTien.Text = (sl * dg).ToString();
			}
			else
			{
				textBoxThanhTien.Text = "0";
			}
		}

		// Nút Thêm sản phẩm vào chi tiết
		private void BtnThem_Click(object sender, EventArgs e)
		{
			// 1. Kiểm tra Nhà cung cấp
			if (comboBoxNCC.SelectedValue == null)
			{
				MessageBox.Show("Vui lòng chọn Nhà cung cấp trước khi thêm sản phẩm!", "Thông báo", MessageBoxButtons.OK, MessageBoxIcon.Warning);
				return;
			}

			// 2. Kiểm tra Sản phẩm
			if (comboMaSP.SelectedValue == null)
			{
				MessageBox.Show("Vui lòng chọn sản phẩm!", "Thông báo", MessageBoxButtons.OK, MessageBoxIcon.Warning);
				return;
			}

			// 3. Kiểm tra Số lượng hợp lệ
			if (!int.TryParse(textBoxSoLuong.Text, out int soLuong) || soLuong <= 0)
			{
				MessageBox.Show("Số lượng phải là số nguyên lớn hơn 0!", "Lỗi", MessageBoxButtons.OK, MessageBoxIcon.Error);
				textBoxSoLuong.Focus();
				return;
			}

			// 4. Kiểm tra Đơn giá hợp lệ
			if (!decimal.TryParse(textBoxDonGia.Text, out decimal donGia) || donGia < 0)
			{
				MessageBox.Show("Đơn giá không hợp lệ!", "Lỗi", MessageBoxButtons.OK, MessageBoxIcon.Error);
				return;
			}

			int maNCC = Convert.ToInt32(comboBoxNCC.SelectedValue);
			int maSP = Convert.ToInt32(comboMaSP.SelectedValue);

			// 5. Kiểm tra xem sản phẩm này ĐÃ CÓ trong danh sách chi tiết của phiếu này chưa
			bool daCo = false;
			foreach (DataGridViewRow r in dataGridViewChiTiet.Rows)
			{
				if (r.Cells["colMaSP"].Value != null && Convert.ToInt32(r.Cells["colMaSP"].Value) == maSP)
				{
					daCo = true;
					break;
				}
			}

			bool ketQua = false;
			if (daCo)
			{
				// Nếu sản phẩm đã có trong phiếu -> Gọi hàm sửa số lượng / đơn giá
				ketQua = nhapHangDAL.SuaChiTietPhieuNhap(maPhieuDangTaoOrXem, maSP, soLuong, donGia);
			}
			else
			{
				// Nếu chưa có trong phiếu:
				// Nếu là phiếu mới tạo (chưa có trong CSDL) thì khởi tạo phiếu nhập trước
				if (maPhieuDangTaoOrXem == -1)
				{
					maPhieuDangTaoOrXem = nhapHangDAL.TaoPhieuNhapMoi(User.MaTK, maNCC);
					if (maPhieuDangTaoOrXem > 0)
					{
						textBoxMaPhieu2.Text = maPhieuDangTaoOrXem.ToString();
					}
				}

				// Sau đó thêm chi tiết sản phẩm vào
				ketQua = nhapHangDAL.ThemChiTietPhieuNhap(maPhieuDangTaoOrXem, maSP, soLuong, donGia);
			}

			// 6. Xử lý thông báo kết quả
			if (ketQua)
			{
				// Sau khi thêm/sửa thành công sản phẩm đầu tiên, khóa luôn ô chọn Nhà Cung Cấp lại
				comboBoxNCC.Enabled = false;

				LoadChiTietLenGrid(maPhieuDangTaoOrXem);

				// Reset lại ô nhập số lượng và sản phẩm để chuẩn bị nhập tiếp
				textBoxSoLuong.Clear();
				comboMaSP.SelectedIndex = -1;

				MessageBox.Show("Cập nhật thông tin chi tiết thành công!", "Thông báo", MessageBoxButtons.OK, MessageBoxIcon.Information);
			}
			else
			{
				MessageBox.Show("Thực hiện thao tác thất bại!", "Lỗi", MessageBoxButtons.OK, MessageBoxIcon.Error);
			}
		}

		private void LoadChiTietLenGrid(int maPhieu)
		{
			DataTable dt = nhapHangDAL.LayChiTietPhieuNhap(maPhieu);
			dataGridViewChiTiet.Rows.Clear();
			foreach (DataRow row in dt.Rows)
			{
				dataGridViewChiTiet.Rows.Add(
					row["MaPhieuNhap"],
					row["MaSP"],
					row["SoLuong"],
					row["GiaNhap"],
					row["ThanhTien"]
				);
			}
		}

		// Nút Lưu phiếu nhập (Hoàn tất tạo mới)
		private void BtnLuu_Click(object sender, EventArgs e)
		{
			if (dataGridViewChiTiet.Rows.Count == 0)
			{
				MessageBox.Show("Phiếu nhập phải có ít nhất một sản phẩm trước khi lưu!", "Thông báo", MessageBoxButtons.OK, MessageBoxIcon.Warning);
				return;
			}

			MessageBox.Show("Lưu phiếu nhập thành công với trạng thái Chờ xác nhận!", "Thông báo", MessageBoxButtons.OK, MessageBoxIcon.Information);
			HienThiGiaoDienDanhSach();
			LoadDanhSachPhieuNhap();
		}

		// Nút Hủy (Xóa phiếu tạm đang tạo)
		private void BtnHuy_Click(object sender, EventArgs e)
		{
			DialogResult dr = MessageBox.Show("Bạn có chắc chắn muốn hủy quá trình tạo phiếu nhập này không?", "Xác nhận", MessageBoxButtons.YesNo, MessageBoxIcon.Question);
			if (dr == DialogResult.Yes)
			{
				// Chỉ xóa trong CSDL nếu đã thực sự sinh ra phiếu (tức là đã bấm Thêm ít nhất 1 sản phẩm)
				if (dangTaoMoiHoacSua && maPhieuDangTaoOrXem > 0)
				{
					nhapHangDAL.XoaPhieuNhapTam(maPhieuDangTaoOrXem);
				}
				HienThiGiaoDienDanhSach();
				LoadDanhSachPhieuNhap();
			}
		}

		// ================= PHẦN 3: NGHIỆP VỤ XEM CHI TIẾT & XÁC NHẬN =================

		private void BtnChiTiet_Click(object sender, EventArgs e)
		{
			if (maPhieuDangTaoOrXem == -1)
			{
				MessageBox.Show("Vui lòng chọn một phiếu nhập từ danh sách để xem chi tiết!", "Thông báo", MessageBoxButtons.OK, MessageBoxIcon.Warning);
				return;
			}

			string trangThai = textBoxTrangThai.Text.Trim();
			textBoxMaPhieu2.Text = maPhieuDangTaoOrXem.ToString();
			textBoxMaPhieu2.ReadOnly = true;
			textBoxThanhTien.ReadOnly = true;
			LoadChiTietLenGrid(maPhieuDangTaoOrXem);

			HienThiGiaoDienChiTiet();

			// Lấy MaNCC của phiếu này gán ngược lại cho comboBoxNCC ở panel chi tiết
			if (dataGridViewPhieuNhap.CurrentRow != null)
			{
				string tenNCC = dataGridViewPhieuNhap.CurrentRow.Cells["colNCC"].Value?.ToString();
				// Tìm và chọn lại NCC trên comboBoxNCC dựa vào TenNCC
				foreach (DataRowView item in comboBoxNCC.Items)
				{
					if (item["TenNCC"].ToString() == tenNCC)
					{
						comboBoxNCC.SelectedValue = item["MaNCC"];
						break;
					}
				}
			}

			if (trangThai == "Chờ xác nhận")
			{
				dangTaoMoiHoacSua = true;
				comboBoxNCC.Enabled = false; // Đã chốt NCC từ lúc tạo nên khóa lại

				// MỞ KHÓA CHO PHÉP SỬA THÔNG TIN SẢN PHẨM
				comboMaSP.Enabled = true;          // Hoặc có thể khóa mã SP nếu chỉ muốn sửa số lượng của dòng đó
				textBoxSoLuong.ReadOnly = false;   // Cho phép sửa số lượng
				textBoxDonGia.ReadOnly = false;    // Cho phép sửa đơn giá (nếu cần)

				btnThem.Visible = true;
				btnThem.Enabled = true;

				btnLuu.Visible = true;
				btnLuu.Enabled = true;

				btnHuy.Visible = true;
				btnHuy.Enabled = true;

				btnDong.Visible = true;
				btnDong.Enabled = true;
			}
			else
			{
				// TRƯỜNG HỢP 2: Đã xác nhận -> KHÓA HẾT, chỉ cho xem
				dangTaoMoiHoacSua = false;

				// Khóa toàn bộ các ComboBox và TextBox nhập liệu ở panelChiTiet
				comboBoxNCC.Enabled = false;
				comboMaSP.Enabled = false;
				textBoxSoLuong.ReadOnly = true;
				textBoxDonGia.ReadOnly = true;
				textBoxThanhTien.ReadOnly = true;
				textBoxMaPhieu2.ReadOnly = true;

				// Ẩn các nút thao tác thêm/sửa/xóa
				btnThem.Visible = false;
				btnLuu.Visible = false;
				btnHuy.Visible = false;

				// Chỉ hiện nút Đóng để thoát ra
				btnDong.Visible = true;
			}
		}

		// Nút Xác nhận phiếu nhập (từ panelThongTinPhieu)
		private void BtnXacNhan_Click(object sender, EventArgs e)
		{
			if (maPhieuDangTaoOrXem == -1)
			{
				MessageBox.Show("Vui lòng chọn phiếu cần xác nhận!", "Thông báo", MessageBoxButtons.OK, MessageBoxIcon.Warning);
				return;
			}

			string trangThai = textBoxTrangThai.Text.Trim();
			if (trangThai == "DaXacNhan")
			{
				MessageBox.Show("Phiếu này đã được xác nhận trước đó rồi!", "Thông báo", MessageBoxButtons.OK, MessageBoxIcon.Information);
				return;
			}

			bool kq = nhapHangDAL.XacNhanPhieuNhap(maPhieuDangTaoOrXem);
			if (kq)
			{
				MessageBox.Show("Xác nhận phiếu nhập thành công!", "Thông báo", MessageBoxButtons.OK, MessageBoxIcon.Information);
				LoadDanhSachPhieuNhap();
			}
			else
			{
				MessageBox.Show("Xác nhận thất bại!", "Lỗi", MessageBoxButtons.OK, MessageBoxIcon.Error);
			}
		}

		// Nút Đóng (khi xem phiếu đã xác nhận)
		private void BtnDong_Click(object sender, EventArgs e)
		{
			HienThiGiaoDienDanhSach();
		}

		private void comboBox1_SelectedIndexChanged(object sender, EventArgs e) { }
		private void textBox5_TextChanged(object sender, EventArgs e) { }
		private void textBox3_TextChanged(object sender, EventArgs e) { }
		private void labelThanhTien_Click(object sender, EventArgs e) { }
		private void labelSoLuong_Click(object sender, EventArgs e) { }
		private void labelMaSP_Click(object sender, EventArgs e) { }
	}
}