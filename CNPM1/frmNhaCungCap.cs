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
	public partial class frmNhaCungCap : Form
	{
		private bool isThemMoi = false;

		public frmNhaCungCap()
		{
			InitializeComponent();
			dataGridView1.CellFormatting += dataGridView1_CellFormatting;
			Load += frmNhaCungCap_Load;
			dataGridView1.SelectionChanged += dataGridView1_SelectionChanged;

			button1.Click += btnTimKiem_Click; // Nút Tìm kiếm
			button2.Click += btnThem_Click;    // Nút Thêm
			button3.Click += btnLuu_Click;     // Nút Lưu
			textBox1.KeyDown += textBox1_KeyDown;
		}

		private void frmNhaCungCap_Load(object sender, EventArgs e)
		{
			try
			{
				dataGridView1.AutoGenerateColumns = false;

				// Gắn dữ liệu cho các cột trên GridView theo đúng thứ tự thiết kế
				dataGridView1.Columns[0].DataPropertyName = "MaNCC";
				dataGridView1.Columns[1].DataPropertyName = "TenNCC";
				dataGridView1.Columns[2].DataPropertyName = "DiaChi";
				dataGridView1.Columns[3].DataPropertyName = "Email";
				dataGridView1.Columns[4].DataPropertyName = "ThongTinLienHe"; // Số điện thoại
				dataGridView1.Columns[5].DataPropertyName = "TrangThai";

				dataGridView1.ReadOnly = true;
				dataGridView1.AllowUserToAddRows = false;
				dataGridView1.SelectionMode = DataGridViewSelectionMode.FullRowSelect;
				dataGridView1.AutoSizeColumnsMode = DataGridViewAutoSizeColumnsMode.Fill;

				textBox4.ReadOnly = true; // Mã NCC tự sinh
				comboBox1.DropDownStyle = ComboBoxStyle.DropDownList;

				LoadDanhSach("");
			}
			catch (Exception ex)
			{
				MessageBox.Show("Lỗi tải dữ liệu: " + ex.Message, "Lỗi",
								MessageBoxButtons.OK, MessageBoxIcon.Error);
			}
		}

		private void LoadDanhSach(string tuKhoa)
		{
			try
			{
				DataTable dt = NhaCungCapDAL.LayDanhSach(tuKhoa);
				dataGridView1.DataSource = dt;

				if (dt.Rows.Count > 0)
				{
					isThemMoi = false;
					dataGridView1.Rows[0].Selected = true;
					HienThiChiTiet(dt.Rows[0]);
				}
				else
				{
					LamMoi();
				}
			}
			catch (Exception ex)
			{
				MessageBox.Show("Lỗi tải danh sách nhà cung cấp: " + ex.Message, "Lỗi",
								MessageBoxButtons.OK, MessageBoxIcon.Error);
			}
		}

		private void HienThiChiTiet(DataRow r)
		{
			if (r == null) return;

			textBox4.Text = r["MaNCC"].ToString();
			textBox5.Text = r["TenNCC"].ToString();
			textBox6.Text = r["DiaChi"] != DBNull.Value ? r["DiaChi"].ToString() : "";
			textBox2.Text = r["Email"] != DBNull.Value ? r["Email"].ToString() : "";         // textBox2 là Email
			textBox7.Text = r["ThongTinLienHe"] != DBNull.Value ? r["ThongTinLienHe"].ToString() : ""; // textBox7 là SĐT
			textBox3.Text = r["MoTa"] != DBNull.Value ? r["MoTa"].ToString() : "";

			// Xử lý Trạng thái cho ComboBox1 (1: Đang hợp tác -> chọn index 0, 0: Ngừng hợp tác -> chọn index 1)
			if (r["TrangThai"] != DBNull.Value)
			{
				int trangThai = Convert.ToInt32(r["TrangThai"]);
				comboBox1.SelectedIndex = (trangThai == 1) ? 0 : 1;
			}
		}

		private void dataGridView1_CellFormatting(object sender, DataGridViewCellFormattingEventArgs e)
		{
			// Kiểm tra đúng cột Trạng thái (dựa vào tên cột 'colTrangThai' từ file Designer của bạn)
			if (dataGridView1.Columns[e.ColumnIndex].Name == "colTrangThai")
			{
				if (e.Value != null && e.Value != DBNull.Value)
				{
					bool trangThai = Convert.ToBoolean(e.Value);
					e.Value = trangThai ? "Đang hợp tác" : "Ngừng hợp tác";
					e.FormattingApplied = true;
				}
			}
		}
		private void dataGridView1_SelectionChanged(object sender, EventArgs e)
		{
			if (isThemMoi)
			{
				isThemMoi = false;
			}

			if (dataGridView1.CurrentRow == null) return;
			DataRowView r = dataGridView1.CurrentRow.DataBoundItem as DataRowView;
			if (r == null) return;

			HienThiChiTiet(r.Row);
		}

		private void btnTimKiem_Click(object sender, EventArgs e)
		{
			string tuKhoa = textBox1.Text.Trim();
			isThemMoi = false;
			LoadDanhSach(tuKhoa);
		}

		private void textBox1_KeyDown(object sender, KeyEventArgs e)
		{
			if (e.KeyCode == Keys.Enter)
			{
				e.SuppressKeyPress = true;
				btnTimKiem_Click(sender, e);
			}
		}

		private void LamMoi()
		{
			isThemMoi = true;
			textBox4.Text = "(Tự sinh)";
			textBox5.Clear();
			textBox6.Clear();
			textBox2.Clear(); // Email
			textBox7.Clear(); // SĐT
			textBox3.Clear(); // Mô tả
			comboBox1.SelectedIndex = 0; // Mặc định "Đang hợp tác"
		}

		private void btnThem_Click(object sender, EventArgs e)
		{
			isThemMoi = true;
			LamMoi();
			dataGridView1.ClearSelection();
			textBox5.Focus();
		}

		private void btnLuu_Click(object sender, EventArgs e)
		{
			string tenNCC = textBox5.Text.Trim();
			if (string.IsNullOrWhiteSpace(tenNCC))
			{
				MessageBox.Show("Vui lòng nhập tên nhà cung cấp.", "Thông báo",
								MessageBoxButtons.OK, MessageBoxIcon.Warning);
				textBox5.Focus();
				return;
			}

			string diaChi = textBox6.Text.Trim();
			string email = textBox2.Text.Trim();
			string sdt = textBox7.Text.Trim();
			string moTa = textBox3.Text.Trim();

			// ComboBox1: index 0 là Đang hợp tác (1), index 1 là Ngừng hợp tác (0)
			int trangThai = (comboBox1.SelectedIndex == 1) ? 0 : 1;

			try
			{
				if (isThemMoi || !int.TryParse(textBox4.Text, out int maNCC))
				{
					// Thêm mới
					bool kq = NhaCungCapDAL.Them(tenNCC, diaChi, email, sdt, moTa, trangThai);
					if (kq)
					{
						MessageBox.Show("Thêm nhà cung cấp mới thành công!", "Thông báo",
										MessageBoxButtons.OK, MessageBoxIcon.Information);
						isThemMoi = false;
						LoadDanhSach("");
					}
				}
				else
				{
					// Cập nhật
					bool kq = NhaCungCapDAL.Sua(maNCC, tenNCC, diaChi, email, sdt, moTa, trangThai);
					if (kq)
					{
						MessageBox.Show("Cập nhật thông tin nhà cung cấp thành công!", "Thông báo",
										MessageBoxButtons.OK, MessageBoxIcon.Information);
						isThemMoi = false;
						LoadDanhSach("");
						ChonDongTheoMa(maNCC);
					}
				}
			}
			catch (Exception ex)
			{
				MessageBox.Show("Lỗi khi lưu dữ liệu: " + ex.Message, "Lỗi",
								MessageBoxButtons.OK, MessageBoxIcon.Error);
			}
		}

		private void btnXoa_Click(object sender, EventArgs e)
		{
			if (!int.TryParse(textBox4.Text, out int maNCC) || maNCC <= 0)
			{
				MessageBox.Show("Vui lòng chọn một nhà cung cấp từ danh sách.", "Thông báo",
								MessageBoxButtons.OK, MessageBoxIcon.Warning);
				return;
			}

			DialogResult result = MessageBox.Show(
				"Bạn có chắc chắn muốn chuyển trạng thái ngừng hợp tác với nhà cung cấp: \"" + textBox5.Text + "\"?",
				"Xác nhận",
				MessageBoxButtons.YesNo,
				MessageBoxIcon.Question);

			if (result != DialogResult.Yes) return;

			try
			{
				string tenNCC = textBox5.Text.Trim();
				string diaChi = textBox6.Text.Trim();
				string email = textBox2.Text.Trim();
				string sdt = textBox7.Text.Trim();
				string moTa = "";

				// Cập nhật trạng thái thành 0 (ngừng hợp tác)
				NhaCungCapDAL.Sua(maNCC, tenNCC, diaChi, email, sdt, moTa, 0);

				MessageBox.Show("Đã cập nhật trạng thái ngừng hợp tác thành công!", "Thông báo",
								MessageBoxButtons.OK, MessageBoxIcon.Information);
				isThemMoi = false;
				LoadDanhSach("");
				ChonDongTheoMa(maNCC);
			}
			catch (Exception ex)
			{
				MessageBox.Show("Lỗi khi cập nhật trạng thái: " + ex.Message, "Lỗi",
								MessageBoxButtons.OK, MessageBoxIcon.Error);
			}
		}

		private void ChonDongTheoMa(int maNCC)
		{
			foreach (DataGridViewRow row in dataGridView1.Rows)
			{
				DataRowView drv = row.DataBoundItem as DataRowView;
				if (drv != null && Convert.ToInt32(drv["MaNCC"]) == maNCC)
				{
					row.Selected = true;
					dataGridView1.CurrentCell = row.Cells[0];
					HienThiChiTiet(drv.Row);
					break;
				}
			}
		}

		// Các sự kiện phụ sinh ra từ giao diện thiết kế để trống tránh lỗi biên dịch
		private void label8_Click(object sender, EventArgs e) { }
		private void textBox7_TextChanged(object sender, EventArgs e) { }
		private void label9_Click(object sender, EventArgs e) { }
		private void comboBox1_SelectedIndexChanged(object sender, EventArgs e) { }
		private void textBox2_TextChanged(object sender, EventArgs e) { }
		private void frmNhaCungCap_Load_1(object sender, EventArgs e) { }

		private void dataGridView1_CellContentClick(object sender, DataGridViewCellEventArgs e) { }
	}
}