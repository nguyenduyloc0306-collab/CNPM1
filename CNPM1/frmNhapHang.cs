using System;
using System.Data;
using System.Windows.Forms;

namespace CNPM1
{
	public partial class frmNhapHang : Form
	{
		// Bien trang thai chuan khong dau
		private bool isDangTaoMoi = false;

		// DataTable luu danh sach chi tiet tam thoi
		private DataTable dtChiTietTam = new DataTable();

		public frmNhapHang()
		{
			InitializeComponent();
			DangKySuKien();
			KhoiTaoCotChiTietTam();
		}

		private void DangKySuKien()
		{
			this.Load += FrmNhapHang_Load;
			btnTaoMoi.Click += btnTaoMoi_Click;
			btnChiTiet.Click += btnChiTiet_Click;
			btnXacNhan.Click += btnXacNhan_Click;
			btnThem.Click += btnThem_Click;
			btnLuu.Click += btnLuu_Click;
			btnHuy.Click += btnHuy_Click;

			dataGridViewPhieuNhap.CellClick += dataGridViewPhieuNhap_CellClick;
			comboBoxNCC.SelectedIndexChanged += comboBoxNCC_SelectedIndexChanged;

			// Tự động tính thành tiền tạm khi nhập số lượng hoặc đơn giá
			textBox3.TextChanged += TinhThanhTienTam;
			textBox4.TextChanged += TinhThanhTienTam;
		}

		private void KhoiTaoCotChiTietTam()
		{
			dtChiTietTam.Columns.Add("MaSP", typeof(int));
			dtChiTietTam.Columns.Add("SoLuong", typeof(int));
			dtChiTietTam.Columns.Add("GiaNhap", typeof(decimal));
		}

        private void FrmNhapHang_Load(object sender, EventArgs e)
        {
            try
            {
                dataGridViewPhieuNhap.AllowUserToAddRows = false; // không hiện dòng trống ở cuối
                dataGridViewPhieuNhap.ReadOnly = true;
                dataGridViewPhieuNhap.SelectionMode = DataGridViewSelectionMode.FullRowSelect;

                LoadComboBoxNCC();
                LoadComboBoxSanPham();
                TrangThaiBanDau(); // bên trong đã gọi LoadDanhSachPhieuNhap()
            }
            catch (Exception ex)
            {
                MessageBox.Show("Loi FrmNhapHang_Load: " + ex.Message, "Thong bao", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void TrangThaiBanDau()
		{
			isDangTaoMoi = false;

			// Hien thi phan danh sach, an phan chi tiet
			dataGridViewPhieuNhap.Visible = true;
			dataGridViewPhieuNhap.BringToFront(); // Đảm bảo bảng nổi lên trên cùng

			panelThongTinPhieu.Visible = true;
			dataGridViewChiTiet.Visible = false;
			panelChiTiet.Visible = false;

			// Xoa trang du lieu
			textBoxMaPhieu.Clear();
			textBoxNVKho.Clear();
			textBoxNgayNhap.Clear();
			textBoxTongTien.Clear();
			textBoxTrangThai.Clear();

			// Khoa toan bo cac o nhap lieu thong tin phieu
			textBoxMaPhieu.ReadOnly = true;
			textBoxMaPhieu.Enabled = false;
			textBoxNVKho.ReadOnly = true;
			textBoxNgayNhap.ReadOnly = true;
			textBoxTongTien.ReadOnly = true;
			textBoxTrangThai.ReadOnly = true;

			// Chi cho phep chon Nha Cung Cap khi vua mo form
			comboBoxNCC.Enabled = true;
			comboBoxNCC.SelectedIndex = -1;

			// Trang thai cac nut
			btnTaoMoi.Enabled = false;
			btnChiTiet.Enabled = false;
			btnXacNhan.Enabled = false;

			// ==> BỔ SUNG GỌI HÀM NÀY ĐỂ NẠP DỮ LIỆU LÊN BẢNG NGAY LẬP TỨC:
			LoadDanhSachPhieuNhap();
		}

        private void LoadDanhSachPhieuNhap()
        {
            DataTable dt = NhapHangDAL.LayDanhSachPhieuNhap();

            dataGridViewPhieuNhap.Rows.Clear(); // hàm này được gọi nhiều lần, không xóa sẽ nhân đôi dòng

            foreach (DataRow row in dt.Rows)
            {
                int i = dataGridViewPhieuNhap.Rows.Add();
                DataGridViewRow r = dataGridViewPhieuNhap.Rows[i];
                r.Cells["colMaPhieu"].Value = row[0]?.ToString(); // Mã phiếu
                r.Cells["colNVKho"].Value = row[1]?.ToString(); // Mã NV kho
                r.Cells["colNCC"].Value = row[2]?.ToString(); // Mã NCC
                r.Cells["colNgayNhap"].Value = row[3]?.ToString(); // Ngày nhập
                r.Cells["colTongtien"].Value = row[4]?.ToString(); // Tổng tiền
                r.Cells["colTrangThai"].Value = row[5]?.ToString(); // Trạng thái
            }
        }

        private void LoadComboBoxNCC()
		{
			comboBoxNCC.DataSource = NhapHangDAL.LayDanhSachNCC();
			comboBoxNCC.DisplayMember = "TenNCC";
			comboBoxNCC.ValueMember = "MaNCC";
			comboBoxNCC.SelectedIndex = -1;
		}

		private void LoadComboBoxSanPham()
		{
			comboBox1.DataSource = NhapHangDAL.LayDanhSachSanPham();
			comboBox1.DisplayMember = "TenSP";
			comboBox1.ValueMember = "MaSP";
			comboBox1.SelectedIndex = -1;
		}

		private void comboBoxNCC_SelectedIndexChanged(object sender, EventArgs e)
		{
			// Neu chua tao moi va nguoi dung da chon 1 NCC hop le thi mo khoa nut Tao moi
			if (!isDangTaoMoi && comboBoxNCC.SelectedIndex != -1)
			{
				btnTaoMoi.Enabled = true;
			}
			else if (!isDangTaoMoi && comboBoxNCC.SelectedIndex == -1)
			{
				btnTaoMoi.Enabled = false;
			}
		}

		private void dataGridViewPhieuNhap_CellClick(object sender, DataGridViewCellEventArgs e)
		{
			if (e.RowIndex < 0) return;

			DataGridView dgv = sender as DataGridView;
			if (dgv == null) return;

			DataGridViewRow row = dataGridViewPhieuNhap.Rows[e.RowIndex];

			textBoxMaPhieu.Text = row.Cells["colMaPhieu"].Value?.ToString() ?? "";
			comboBoxNCC.Text = row.Cells["colNCC"].Value?.ToString() ?? "";
			textBoxNVKho.Text = row.Cells["colNVKho"].Value?.ToString() ?? "";
			textBoxNgayNhap.Text = row.Cells["colNgayNhap"].Value?.ToString() ?? "";
			textBoxTongTien.Text = row.Cells["colTongtien"].Value?.ToString() ?? "";
			string trangThai = row.Cells["colTrangThai"].Value?.ToString() ?? "";
			textBoxTrangThai.Text = trangThai;

			btnChiTiet.Enabled = true;

			if (trangThai == "ChoXacNhan")
			{
				btnXacNhan.Enabled = true;
			}
			else
			{
				btnXacNhan.Enabled = false;
			}
		}

		private void btnTaoMoi_Click(object sender, EventArgs e)
		{
			isDangTaoMoi = true;

			comboBoxNCC.Enabled = true;

			textBoxMaPhieu.Text = "(Tu dong sinh)";
			textBoxNVKho.Text = User.HoTen;
			textBoxNgayNhap.Text = DateTime.Now.ToString("dd/MM/yyyy HH:mm:ss");
			textBoxTongTien.Text = "0";
			textBoxTrangThai.Text = "ChoXacNhan";

			dtChiTietTam.Clear();

			dataGridViewPhieuNhap.Visible = false;
			panelThongTinPhieu.Visible = false;
			dataGridViewChiTiet.Visible = true;
			dataGridViewChiTiet.DataSource = dtChiTietTam;
			panelChiTiet.Visible = true;

			textBox1.Text = "(Tu dong)";
			textBox1.ReadOnly = true;
			textBox5.ReadOnly = true;
			btnHuy.Enabled = true;
		}

		private void TinhThanhTienTam(object sender, EventArgs e)
		{
			if (int.TryParse(textBox3.Text, out int sl) && decimal.TryParse(textBox4.Text, out decimal dg))
			{
				textBox5.Text = (sl * dg).ToString();
			}
			else
			{
				textBox5.Text = "0";
			}
		}

		private void btnThem_Click(object sender, EventArgs e)
		{
			if (comboBox1.SelectedValue == null)
			{
				MessageBox.Show("Vui long chon san pham!", "Thong bao", MessageBoxButtons.OK, MessageBoxIcon.Warning);
				return;
			}

			if (!int.TryParse(textBox3.Text, out int soLuong) || !decimal.TryParse(textBox4.Text, out decimal giaNhap))
			{
				MessageBox.Show("Vui long nhap dung dinh dang so cho So luong va Don gia!", "Thong bao", MessageBoxButtons.OK, MessageBoxIcon.Warning);
				return;
			}

			if (soLuong <= 0 || giaNhap < 0)
			{
				MessageBox.Show("So luong phai lon hon 0 va don gia khong duoc am!", "Thong bao", MessageBoxButtons.OK, MessageBoxIcon.Warning);
				return;
			}

			int maSP = Convert.ToInt32(comboBox1.SelectedValue);

			DataRow row = dtChiTietTam.NewRow();
			row["MaSP"] = maSP;
			row["SoLuong"] = soLuong;
			row["GiaNhap"] = giaNhap;
			dtChiTietTam.Rows.Add(row);

			dataGridViewChiTiet.DataSource = dtChiTietTam;

			comboBox1.SelectedIndex = -1;
			textBox3.Clear();
			textBox4.Clear();
			textBox5.Clear();
		}

		private void btnLuu_Click(object sender, EventArgs e)
		{
			if (comboBoxNCC.SelectedValue == null)
			{
				MessageBox.Show("Vui long chon Nha cung cap!", "Thong bao", MessageBoxButtons.OK, MessageBoxIcon.Warning);
				return;
			}

			if (dtChiTietTam.Rows.Count == 0)
			{
				MessageBox.Show("Phieu nhap phai co it nhat mot san pham!", "Thong bao", MessageBoxButtons.OK, MessageBoxIcon.Warning);
				return;
			}

			int maNCC = Convert.ToInt32(comboBoxNCC.SelectedValue);
			int maNV = User.MaTK;

			bool ketQua = NhapHangDAL.TaoPhieuNhap(maNV, maNCC, dtChiTietTam, out int maPhieuMoi);
			if (ketQua)
			{
				MessageBox.Show($"Tao phieu nhap thanh cong! Ma phieu: {maPhieuMoi}", "Thong bao", MessageBoxButtons.OK, MessageBoxIcon.Information);
				LoadDanhSachPhieuNhap();
				TrangThaiBanDau();
			}
			else
			{
				MessageBox.Show("Loi khi luu phieu nhap vao co so du lieu!", "Loi", MessageBoxButtons.OK, MessageBoxIcon.Error);
			}
		}

		private void btnHuy_Click(object sender, EventArgs e)
		{
			TrangThaiBanDau();
		}

		private void btnChiTiet_Click(object sender, EventArgs e)
		{
			if (string.IsNullOrEmpty(textBoxMaPhieu.Text)) return;

			int maPhieu = Convert.ToInt32(textBoxMaPhieu.Text);
			string trangThai = textBoxTrangThai.Text;

			dataGridViewPhieuNhap.Visible = false;
			panelThongTinPhieu.Visible = false;
			dataGridViewChiTiet.Visible = true;
			panelChiTiet.Visible = true;

			textBox1.Text = maPhieu.ToString();
			textBox1.ReadOnly = true;

			dataGridViewChiTiet.DataSource = NhapHangDAL.LayChiTietPhieuNhap(maPhieu);

			if (trangThai == "ChoXacNhan")
			{
				btnThem.Enabled = true;
				btnLuu.Enabled = true;
				btnHuy.Enabled = false;
			}
			else
			{
				btnThem.Enabled = false;
				btnLuu.Enabled = false;
				btnHuy.Enabled = true;
			}
		}

		private void btnXacNhan_Click(object sender, EventArgs e)
		{
			if (string.IsNullOrEmpty(textBoxMaPhieu.Text)) return;

			int maPhieu = Convert.ToInt32(textBoxMaPhieu.Text);

			if (MessageBox.Show($"Ban co chac chan muon xac nhan phieu nhap so {maPhieu} khong?", "Xac nhan", MessageBoxButtons.YesNo, MessageBoxIcon.Question) == DialogResult.Yes)
			{
				if (NhapHangDAL.XacNhanPhieuNhap(maPhieu))
				{
					MessageBox.Show("Xac nhan phieu nhap thanh cong!", "Thong bao", MessageBoxButtons.OK, MessageBoxIcon.Information);
					LoadDanhSachPhieuNhap();
					TrangThaiBanDau();
				}
				else
				{
					MessageBox.Show("Xac nhan that bai!", "Loi", MessageBoxButtons.OK, MessageBoxIcon.Error);
				}
			}
		}
	}
}