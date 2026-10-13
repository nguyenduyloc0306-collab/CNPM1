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
    public partial class frmLichSuMuaHang : Form
    {
        private readonly int maKhachHang; // mã tài khoản (MaTK) của khách đang đăng nhập

        public frmLichSuMuaHang(int maKH)
        {
            InitializeComponent();
            maKhachHang = maKH;
        }

        private void frmLichSuMuaHang_Load(object sender, EventArgs e)
        {
            dgvHoaDon.AutoGenerateColumns = false;
            dgvChiTiet.AutoGenerateColumns = false;

            dgvHoaDon.Columns["colNgay"].DefaultCellStyle.Format = "dd/MM/yyyy HH:mm";
            dgvHoaDon.Columns["colTongTien"].DefaultCellStyle.Format = "N0";
            dgvHoaDon.Columns["colGiam"].DefaultCellStyle.Format = "N0";
            dgvHoaDon.Columns["colThanhToan"].DefaultCellStyle.Format = "N0";
            dgvChiTiet.Columns["colCTDonGia"].DefaultCellStyle.Format = "N0";
            dgvChiTiet.Columns["colCTThanhTien"].DefaultCellStyle.Format = "N0";

            LoadLichSu();
        }

        // Nạp danh sách hóa đơn của khách, rồi hiện chi tiết hóa đơn đang chọn
        private void LoadLichSu()
        {
            try
            {
                DataTable dt = LichSuMuaHangDAL.DanhSach(maKhachHang);
                dgvHoaDon.DataSource = dt;

                if (dt.Rows.Count == 0)
                {
                    lblTongKet.Text = "Bạn chưa có hóa đơn nào.";
                }
                else
                {
                    decimal tongChi = 0;
                    foreach (DataRow r in dt.Rows)
                    {
                        tongChi += Convert.ToDecimal(r["TongThanhToan"]);
                    }
                    lblTongKet.Text = "Số hóa đơn: " + dt.Rows.Count +
                                      "   |   Tổng đã thanh toán: " + tongChi.ToString("N0") + " đ";
                }

                LoadChiTiet();
            }
            catch (Exception ex)
            {
                MessageBox.Show("Lỗi tải lịch sử mua hàng: " + ex.Message, "Lỗi",
                                MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        // Hiện các sản phẩm của hóa đơn đang chọn ở lưới phía dưới
        private void LoadChiTiet()
        {
            DataRowView row = dgvHoaDon.CurrentRow == null
                ? null : dgvHoaDon.CurrentRow.DataBoundItem as DataRowView;
            if (row == null)
            {
                dgvChiTiet.DataSource = null;
                return;
            }

            try
            {
                dgvChiTiet.DataSource = LichSuMuaHangDAL.ChiTiet(maKhachHang, Convert.ToInt32(row["MaHoaDon"]));
            }
            catch (Exception ex)
            {
                MessageBox.Show("Lỗi tải chi tiết hóa đơn: " + ex.Message, "Lỗi",
                                MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        // Đổi dòng hóa đơn thì đổi chi tiết theo
        private void dgvHoaDon_SelectionChanged(object sender, EventArgs e)
        {
            LoadChiTiet();
        }

        private void btnDong_Click(object sender, EventArgs e)
        {
            Close();
        }
    }
}