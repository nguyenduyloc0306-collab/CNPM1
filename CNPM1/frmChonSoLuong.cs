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
    /// Hộp thoại chọn số lượng cho một sản phẩm. Dùng cho cả "Thêm vào giỏ" (laMuaNgay = false)
    /// và "Mua" (laMuaNgay = true: có thêm hình thức thanh toán và hiển thị giảm giá).
    /// </summary>
    public partial class frmChonSoLuong : Form
    {
        private readonly int maSP;
        private readonly decimal donGia;
        private readonly bool laMuaNgay;

        public int SoLuong
        {
            get { return (int)nudSoLuong.Value; }
        }

        // Giá trị CSDL chấp nhận: "TienMat" hoặc "ChuyenKhoan"
        public string HinhThuc
        {
            get { return cboHinhThuc.Text == "Chuyển khoản" ? "ChuyenKhoan" : "TienMat"; }
        }

        // tonKho phải >= 1 (nơi gọi đã kiểm tra)
        public frmChonSoLuong(int maSP, string tenSP, decimal donGia, int tonKho, bool laMuaNgay)
        {
            InitializeComponent();
            this.maSP = maSP;
            this.donGia = donGia;
            this.laMuaNgay = laMuaNgay;

            Text = laMuaNgay ? "Mua hàng" : "Thêm vào giỏ hàng";
            btnDongY.Text = laMuaNgay ? "Mua" : "Thêm vào giỏ";
            lblTenSP.Text = tenSP;
            lblDonGia.Text = "Đơn giá: " + donGia.ToString("N0") + " đ";
            lblTon.Text = "Còn trong kho: " + tonKho;
            cboHinhThuc.SelectedIndex = 0; // mặc định "Tiền mặt"

            nudSoLuong.Minimum = 1;
            nudSoLuong.Maximum = tonKho;
            nudSoLuong.Value = 1;

            if (!laMuaNgay)
            {
                // Thêm vào giỏ: không cần hình thức thanh toán và giảm giá (tính khi thanh toán giỏ hàng)
                lblGiam.Visible = false;
                lblPhaiTra.Visible = false;
                lblHinhThuc.Visible = false;
                cboHinhThuc.Visible = false;
                btnDongY.Top = 215;
                btnHuy.Top = 215;
                ClientSize = new Size(ClientSize.Width, 280);
            }

            CapNhatTien();
        }

        private void CapNhatTien()
        {
            int sl = (int)nudSoLuong.Value;
            decimal tong = donGia * sl;
            lblTong.Text = (laMuaNgay ? "Tổng tiền hàng: " : "Thành tiền: ") + tong.ToString("N0") + " đ";

            if (!laMuaNgay) return;
            try
            {
                decimal giam = MuaHangDAL.TinhGiamGia(maSP, sl);
                lblGiam.Text = "Giảm giá: " + giam.ToString("N0") + " đ";
                lblPhaiTra.Text = "Phải trả: " + (tong - giam).ToString("N0") + " đ";
            }
            catch (Exception ex)
            {
                MessageBox.Show("Lỗi tính giảm giá: " + ex.Message, "Lỗi",
                                MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void nudSoLuong_ValueChanged(object sender, EventArgs e)
        {
            CapNhatTien();
        }
    }
}
