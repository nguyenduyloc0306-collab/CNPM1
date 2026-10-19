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
    /// <summary>
    /// Hộp thoại chọn sản phẩm cho hóa đơn tại quầy (thay cho việc quét mã vạch).
    /// Hộp thoại giữ nguyên khi thêm để nhân viên chọn liên tiếp nhiều sản phẩm; bấm "Xong" để đóng.
    /// Việc thêm dòng vào hóa đơn do form gọi thực hiện qua themDong(sản phẩm, số lượng):
    /// trả về null nếu thành công, hoặc thông báo lỗi (ví dụ vượt tồn kho).
    /// </summary>
    public partial class frmChonSanPham : Form
    {
        private readonly Func<DataRowView, int, string> themDong;

        public frmChonSanPham(Func<DataRowView, int, string> themDong)
        {
            InitializeComponent();
            this.themDong = themDong;
        }

        private void frmChonSanPham_Load(object sender, EventArgs e)
        {
            dgvSanPham.AutoGenerateColumns = false;
            dgvSanPham.Columns["colGia"].DefaultCellStyle.Format = "N0";
            dgvSanPham.Columns["colGiaKM"].DefaultCellStyle.Format = "N0";
            LoadSanPham();
            txtTim.Focus();
        }

        private void LoadSanPham()
        {
            try
            {
                DataTable dt = BanHangMoiDAL.DanhSachSanPham(txtTim.Text.Trim());
                dt.Columns.Add("GiaSauKM", typeof(decimal), "GiaBan - GiamMoiSP"); // giá sau khuyến mãi
                dgvSanPham.DataSource = dt;
                btnThem.Enabled = dt.Rows.Count > 0;
                CapNhatSoLuongToiDa();
            }
            catch (Exception ex)
            {
                MessageBox.Show("Lỗi tải sản phẩm: " + ex.Message, "Lỗi",
                                MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private DataRowView SanPhamDangChon()
        {
            return dgvSanPham.CurrentRow == null
                ? null : dgvSanPham.CurrentRow.DataBoundItem as DataRowView;
        }

        // Số lượng tối đa theo tồn kho của sản phẩm đang chọn
        private void CapNhatSoLuongToiDa()
        {
            DataRowView r = SanPhamDangChon();
            int ton = r == null ? 1 : Convert.ToInt32(r["SoLuongTon"]);
            nudSoLuong.Maximum = Math.Max(1, ton);
            nudSoLuong.Value = 1;
        }

        private void dgvSanPham_SelectionChanged(object sender, EventArgs e)
        {
            CapNhatSoLuongToiDa();
        }

        private void dgvSanPham_CellDoubleClick(object sender, DataGridViewCellEventArgs e)
        {
            if (e.RowIndex >= 0) btnThem_Click(sender, EventArgs.Empty);
        }

        private void btnTim_Click(object sender, EventArgs e)
        {
            LoadSanPham();
        }

        private void txtTim_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.KeyCode == Keys.Enter)
            {
                e.SuppressKeyPress = true;
                LoadSanPham();
            }
        }

        private void btnThem_Click(object sender, EventArgs e)
        {
            DataRowView r = SanPhamDangChon();
            if (r == null)
            {
                MessageBox.Show("Vui lòng chọn một sản phẩm.");
                return;
            }

            int sl = (int)nudSoLuong.Value;
            string loi = themDong(r, sl);
            if (loi != null)
            {
                MessageBox.Show(loi, "Không thể thêm", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            lblThongBao.Text = "Đã thêm: " + sl + " x " + r["TenSP"];
            nudSoLuong.Value = 1;
        }
    }
}
