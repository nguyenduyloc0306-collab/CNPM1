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
    public partial class frmGioHang : Form
    {
        private readonly int maKhachHang; // mã tài khoản (MaTK) của khách đang đăng nhập

        public frmGioHang(int maKH)
        {
            InitializeComponent();
            maKhachHang = maKH;
        }

        private void frmGioHang_Load(object sender, EventArgs e)
        {
            dgvGioHang.AutoGenerateColumns = false;
            dgvGioHang.Columns["colGiaBan"].DefaultCellStyle.Format = "N0";
            dgvGioHang.Columns["colThanhTien"].DefaultCellStyle.Format = "N0";
            cboHinhThuc.SelectedIndex = 0; // mặc định "Tiền mặt"
            LoadGioHang();
        }

        // Nạp lại giỏ hàng của khách này từ CSDL; hiển thị tổng tiền hàng, giảm giá và số tiền phải trả
        private void LoadGioHang()
        {
            try
            {
                DataTable dt = GioHangDAL.Lay(maKhachHang);
                dgvGioHang.DataSource = dt;

                decimal tong = 0;
                foreach (DataRow r in dt.Rows)
                {
                    tong += Convert.ToDecimal(r["ThanhTien"]);
                }
                decimal giam = GioHangDAL.TinhGiamGia(maKhachHang);

                lblTongTien.Text = "Tổng tiền hàng: " + tong.ToString("N0") + " đ";
                lblGiamGia.Text = "Giảm giá: " + giam.ToString("N0") + " đ";
                lblThanhToan.Text = "Phải trả: " + (tong - giam).ToString("N0") + " đ";
                btnThanhToan.Enabled = dt.Rows.Count > 0;
            }
            catch (Exception ex)
            {
                MessageBox.Show("Lỗi tải giỏ hàng: " + ex.Message, "Lỗi",
                                MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        // Kiểm tra số lượng khách vừa nhập (chỉ khi ô đang ở chế độ sửa)
        private void dgvGioHang_CellValidating(object sender, DataGridViewCellValidatingEventArgs e)
        {
            if (dgvGioHang.Columns[e.ColumnIndex].Name != "colSoLuong") return;
            if (!dgvGioHang.IsCurrentCellInEditMode) return;

            int sl;
            if (!int.TryParse(Convert.ToString(e.FormattedValue), out sl) || sl < 1)
            {
                MessageBox.Show("Số lượng phải là số nguyên từ 1 trở lên.\n(Nhấn Esc để hủy chỉnh sửa.)");
                e.Cancel = true;
                return;
            }

            DataRowView row = dgvGioHang.Rows[e.RowIndex].DataBoundItem as DataRowView;
            if (row != null && sl > Convert.ToInt32(row["SoLuongTon"]))
            {
                MessageBox.Show("Trong kho chỉ còn " + row["SoLuongTon"] + " sản phẩm này.\n(Nhấn Esc để hủy chỉnh sửa.)");
                e.Cancel = true;
            }
        }

        // Lưu số lượng mới vào CSDL rồi nạp lại để cập nhật Thành tiền, Giảm giá và Phải trả
        private void dgvGioHang_CellEndEdit(object sender, DataGridViewCellEventArgs e)
        {
            if (dgvGioHang.Columns[e.ColumnIndex].Name != "colSoLuong") return;

            DataRowView row = dgvGioHang.Rows[e.RowIndex].DataBoundItem as DataRowView;
            if (row == null) return;

            try
            {
                GioHangDAL.CapNhatSoLuong(maKhachHang,
                    Convert.ToInt32(row["MaSP"]), Convert.ToInt32(row["SoLuong"]));
            }
            catch (Exception ex)
            {
                MessageBox.Show("Lỗi cập nhật: " + ex.Message, "Lỗi",
                                MessageBoxButtons.OK, MessageBoxIcon.Error);
            }

            BeginInvoke(new Action(LoadGioHang)); // nạp lại sau khi ô thoát chế độ sửa
        }

        private void btnXoa_Click(object sender, EventArgs e)
        {
            DataRowView row = dgvGioHang.CurrentRow == null
                ? null : dgvGioHang.CurrentRow.DataBoundItem as DataRowView;
            if (row == null)
            {
                MessageBox.Show("Vui lòng chọn sản phẩm cần xóa khỏi giỏ.");
                return;
            }

            if (MessageBox.Show("Xóa \"" + row["TenSP"] + "\" khỏi giỏ hàng?", "Xác nhận",
                MessageBoxButtons.YesNo, MessageBoxIcon.Question) != DialogResult.Yes) return;

            try
            {
                GioHangDAL.Xoa(maKhachHang, Convert.ToInt32(row["MaSP"]));
                LoadGioHang();
            }
            catch (Exception ex)
            {
                MessageBox.Show("Lỗi: " + ex.Message, "Lỗi",
                                MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void btnThanhToan_Click(object sender, EventArgs e)
        {
            if (dgvGioHang.Rows.Count == 0)
            {
                MessageBox.Show("Giỏ hàng đang trống.");
                return;
            }

            // Combo hiển thị tiếng Việt; CSDL chỉ chấp nhận "TienMat" hoặc "ChuyenKhoan"
            string hinhThuc = cboHinhThuc.Text == "Chuyển khoản" ? "ChuyenKhoan" : "TienMat";

            if (MessageBox.Show(lblThanhToan.Text + "\nHình thức: " + cboHinhThuc.Text + "\n\nXác nhận thanh toán?",
                "Thanh toán", MessageBoxButtons.YesNo, MessageBoxIcon.Question) != DialogResult.Yes) return;

            try
            {
                int maHD = GioHangDAL.ThanhToan(maKhachHang, hinhThuc);
                MessageBox.Show("Thanh toán thành công!\nMã hóa đơn: " + maHD, "Thanh toán",
                                MessageBoxButtons.OK, MessageBoxIcon.Information);
                LoadGioHang();
            }
            catch (InvalidOperationException ex) // giỏ trống / không đủ tồn kho: lỗi nghiệp vụ, đã rollback
            {
                MessageBox.Show(ex.Message, "Không thể thanh toán",
                                MessageBoxButtons.OK, MessageBoxIcon.Warning);
                LoadGioHang();
            }
            catch (Exception ex)
            {
                MessageBox.Show("Lỗi thanh toán: " + ex.Message, "Lỗi",
                                MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void btnDong_Click(object sender, EventArgs e)
        {
            dgvGioHang.CancelEdit(); // bỏ ô đang sửa dở (nếu có) để không chặn việc đóng form
            Close();
        }
    }
}