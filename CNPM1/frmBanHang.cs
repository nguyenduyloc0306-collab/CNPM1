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
    /// Bán hàng tại quầy: nhân viên bấm "Chọn sản phẩm" (thay cho việc quét mã vạch) để thêm các món khách mua
    /// vào hóa đơn, xem tổng tiền, giảm giá, số tiền cần thanh toán, tiền khách đưa / thối lại, rồi hoàn tất.
    /// </summary>
    public partial class frmBanHang : Form
    {
        private readonly int maNhanVien; // MaTK của nhân viên đang lập hóa đơn (HoaDon.MaNhanVien)
        private DataTable dtHoaDon;      // các dòng của hóa đơn đang lập
        private decimal tongTien;        // tổng tiền hàng (trước giảm giá)
        private decimal giamGia;         // tổng giảm giá
        private decimal canThanhToan;    // tongTien - giamGia
        private bool dangCapNhat;        // chặn TextChanged lặp khi chương trình tự gán ô "Khách đưa"

        public frmBanHang(int maNV)
        {
            InitializeComponent();
            maNhanVien = maNV;
            Load += frmBanHang_Load;
        }

        private void frmBanHang_Load(object sender, EventArgs e)
        {
            // Lưới hóa đơn: 5 cột thiết kế sẵn, gán nguồn dữ liệu theo tên cột của bảng hóa đơn
            dataGridView1.AutoGenerateColumns = false;
            colID.DataPropertyName = "MaSP";
            colTenSP.DataPropertyName = "TenSP";
            colDonGia.DataPropertyName = "DonGia";
            colSoLuong.DataPropertyName = "SoLuong";
            colThanhTien.DataPropertyName = "ThanhTien";
            colDonGia.DefaultCellStyle.Format = "N0";
            colThanhTien.DefaultCellStyle.Format = "N0";

            dataGridView1.ReadOnly = true;
            dataGridView1.AllowUserToAddRows = false;
            dataGridView1.MultiSelect = false;
            dataGridView1.SelectionMode = DataGridViewSelectionMode.FullRowSelect;
            dataGridView1.AutoSizeColumnsMode = DataGridViewAutoSizeColumnsMode.Fill;

            // Các ô chỉ để xem (do chương trình tính)
            textBox1.ReadOnly = true; // Giảm giá
            textBox2.ReadOnly = true; // Cần thanh toán
            textBox4.ReadOnly = true; // Mã hóa đơn
            textBox5.ReadOnly = true; // Thối lại
            textBox6.ReadOnly = true; // Tổng tiền hàng

            HoaDonMoi();
            comboBox2.SelectedIndex = 0; // mặc định "Tiền mặt"
        }

        // ------------------------------------------------------------------ hóa đơn đang lập

        // Bắt đầu một hóa đơn trống
        private void HoaDonMoi()
        {
            dtHoaDon = new DataTable();
            dtHoaDon.Columns.Add("MaSP", typeof(int));
            dtHoaDon.Columns.Add("TenSP", typeof(string));
            dtHoaDon.Columns.Add("DonGia", typeof(decimal));
            dtHoaDon.Columns.Add("SoLuong", typeof(int));
            dtHoaDon.Columns.Add("ThanhTien", typeof(decimal));
            dtHoaDon.Columns.Add("GiamMoiSP", typeof(decimal));   // tiền giảm cho 1 sản phẩm (khuyến mãi)
            dtHoaDon.Columns.Add("MaKhuyenMai", typeof(int));     // NULL nếu không có khuyến mãi
            dtHoaDon.Columns.Add("SoLuongTon", typeof(int));      // tồn kho lúc chọn
            dataGridView1.DataSource = dtHoaDon;

            textBox4.Text = "(tự động)";
            label6.Text = "Thời gian: " + DateTime.Now.ToString("dd/MM/yyyy HH:mm");

            dangCapNhat = true;
            textBox3.Clear();
            dangCapNhat = false;

            CapNhatTong();
        }

        // Tính lại tổng tiền hàng, giảm giá, cần thanh toán từ các dòng hóa đơn
        private void CapNhatTong()
        {
            tongTien = 0;
            giamGia = 0;
            foreach (DataRow r in dtHoaDon.Rows)
            {
                int sl = (int)r["SoLuong"];
                tongTien += (decimal)r["DonGia"] * sl;
                giamGia += (decimal)r["GiamMoiSP"] * sl;
            }
            canThanhToan = tongTien - giamGia;

            textBox6.Text = tongTien.ToString("N0");
            textBox1.Text = giamGia.ToString("N0");
            textBox2.Text = canThanhToan.ToString("N0");

            CapNhatThoiLai();
        }

        // Tiền mặt: thối lại = khách đưa - cần thanh toán. Chuyển khoản: khách đưa đúng số tiền, thối lại 0.
        private void CapNhatThoiLai()
        {
            if (dangCapNhat || dtHoaDon == null) return;
            dangCapNhat = true;
            try
            {
                if (comboBox2.Text == "Chuyển khoản")
                {
                    textBox3.ReadOnly = true;
                    textBox3.Text = canThanhToan.ToString("N0");
                    textBox5.Text = "0";
                }
                else
                {
                    textBox3.ReadOnly = false;
                    decimal khachDua;
                    if (dtHoaDon.Rows.Count > 0 && DocTien(textBox3.Text, out khachDua) && khachDua >= canThanhToan)
                        textBox5.Text = (khachDua - canThanhToan).ToString("N0");
                    else
                        textBox5.Text = "";
                }
            }
            finally
            {
                dangCapNhat = false;
            }
        }

        // Đọc số tiền: chấp nhận 100000, 100,000, 100.000, 100.000đ. Ô trống hoặc sai định dạng trả về false.
        private static bool DocTien(string text, out decimal tien)
        {
            tien = 0;
            string s = (text ?? "").Trim().ToLowerInvariant();
            if (s.EndsWith("vnd")) s = s.Substring(0, s.Length - 3);
            else if (s.EndsWith("đ") || s.EndsWith("d")) s = s.Substring(0, s.Length - 1);
            s = s.Replace(" ", "").Replace(".", "").Replace(",", "");
            if (s.Length == 0 || !s.All(char.IsDigit)) return false;
            return decimal.TryParse(s, System.Globalization.NumberStyles.None,
                                    System.Globalization.CultureInfo.InvariantCulture, out tien);
        }

        private static int? LayKM(object o)
        {
            return o is DBNull ? (int?)null : Convert.ToInt32(o);
        }

        // Khuyến mãi ghi vào hóa đơn: khuyến mãi đóng góp nhiều nhất vào tổng giảm giá (HoaDon chỉ lưu một mã)
        private int? ChonKhuyenMai()
        {
            Dictionary<int, decimal> tong = new Dictionary<int, decimal>();
            foreach (DataRow r in dtHoaDon.Rows)
            {
                int? km = LayKM(r["MaKhuyenMai"]);
                if (!km.HasValue) continue;
                decimal g = (decimal)r["GiamMoiSP"] * (int)r["SoLuong"];
                tong[km.Value] = (tong.ContainsKey(km.Value) ? tong[km.Value] : 0) + g;
            }

            int? tot = null;
            decimal max = -1;
            foreach (KeyValuePair<int, decimal> kv in tong)
            {
                if (kv.Value > max) { tot = kv.Key; max = kv.Value; }
            }
            return tot;
        }

        // ------------------------------------------------------------------ chọn sản phẩm

        // Nút "Chọn sản phẩm" (thay cho quét mã): mở hộp thoại chọn sản phẩm + số lượng
        private void btnChonSanPham_Click(object sender, EventArgs e)
        {
            using (frmChonSanPham dlg = new frmChonSanPham(ThemDong))
            {
                dlg.ShowDialog(this);
            }
        }

        // Thêm sản phẩm vào hóa đơn; trả về null nếu thành công, hoặc thông báo lỗi.
        // Số lượng tính cộng dồn với số đã có trong hóa đơn để không vượt tồn kho.
        private string ThemDong(DataRowView sp, int soLuong)
        {
            int maSP = Convert.ToInt32(sp["MaSP"]);
            int ton = Convert.ToInt32(sp["SoLuongTon"]);

            DataRow dong = null;
            foreach (DataRow r in dtHoaDon.Rows)
            {
                if ((int)r["MaSP"] == maSP) { dong = r; break; }
            }

            int daCo = dong == null ? 0 : (int)dong["SoLuong"];
            if (daCo + soLuong > ton)
                return "Không đủ tồn kho: còn " + ton + " sản phẩm, hóa đơn đã có " + daCo + ".";

            decimal gia = Convert.ToDecimal(sp["GiaBan"]);
            if (dong == null)
            {
                dong = dtHoaDon.NewRow();
                dong["MaSP"] = maSP;
                dong["TenSP"] = sp["TenSP"].ToString();
                dtHoaDon.Rows.Add(dong);
            }
            dong["DonGia"] = gia;
            dong["SoLuong"] = daCo + soLuong;
            dong["ThanhTien"] = gia * (daCo + soLuong);
            dong["GiamMoiSP"] = Convert.ToDecimal(sp["GiamMoiSP"]);
            dong["MaKhuyenMai"] = sp["MaKhuyenMai"]; // có thể DBNull
            dong["SoLuongTon"] = ton;

            CapNhatTong();
            return null;
        }

        // Nút "Xóa dòng": bỏ sản phẩm đang chọn khỏi hóa đơn
        private void btnXoaDong_Click(object sender, EventArgs e)
        {
            DataRowView row = dataGridView1.CurrentRow == null
                ? null : dataGridView1.CurrentRow.DataBoundItem as DataRowView;
            if (row == null)
            {
                MessageBox.Show("Vui lòng chọn dòng cần xóa.");
                return;
            }

            dtHoaDon.Rows.Remove(row.Row);
            CapNhatTong();
        }

        // Nút "Hóa đơn mới": hủy hóa đơn đang lập
        private void btnHoaDonMoi_Click(object sender, EventArgs e)
        {
            if (dtHoaDon.Rows.Count > 0 &&
                MessageBox.Show("Hủy hóa đơn đang lập và bắt đầu hóa đơn mới?", "Hóa đơn mới",
                    MessageBoxButtons.YesNo, MessageBoxIcon.Question) != DialogResult.Yes) return;

            HoaDonMoi();
        }

        private void textBox3_TextChanged(object sender, EventArgs e)
        {
            CapNhatThoiLai();
        }

        private void comboBox2_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (dtHoaDon == null) return;
            if (comboBox2.Text != "Chuyển khoản")
            {
                dangCapNhat = true;
                textBox3.Clear(); // bỏ số tiền tự điền của chuyển khoản
                dangCapNhat = false;
            }
            CapNhatThoiLai();
        }

        // ------------------------------------------------------------------ hoàn tất thanh toán

        private void button3_Click(object sender, EventArgs e)
        {
            if (dtHoaDon.Rows.Count == 0)
            {
                MessageBox.Show("Hóa đơn chưa có sản phẩm. Bấm \"Chọn sản phẩm\" để thêm.");
                return;
            }

            try
            {
                // 1. Đối chiếu lại giá, khuyến mãi và tồn kho với CSDL ngay trước khi thanh toán
                bool thayDoi = false;
                foreach (DataRow r in dtHoaDon.Rows)
                {
                    DataRow sp = BanHangMoiDAL.ThongTin((int)r["MaSP"]);
                    if (sp == null)
                    {
                        MessageBox.Show("Sản phẩm \"" + r["TenSP"] + "\" không còn được bán. Vui lòng xóa dòng này.");
                        return;
                    }

                    int ton = Convert.ToInt32(sp["SoLuongTon"]);
                    if ((int)r["SoLuong"] > ton)
                    {
                        MessageBox.Show("Sản phẩm \"" + r["TenSP"] + "\" chỉ còn " + ton +
                                        " trong kho. Vui lòng giảm số lượng.");
                        return;
                    }

                    decimal gia = Convert.ToDecimal(sp["GiaBan"]);
                    decimal giam = Convert.ToDecimal(sp["GiamMoiSP"]);
                    int? km = LayKM(sp["MaKhuyenMai"]);
                    if (gia != (decimal)r["DonGia"] || giam != (decimal)r["GiamMoiSP"] || km != LayKM(r["MaKhuyenMai"]))
                    {
                        r["DonGia"] = gia;
                        r["ThanhTien"] = gia * (int)r["SoLuong"];
                        r["GiamMoiSP"] = giam;
                        r["MaKhuyenMai"] = sp["MaKhuyenMai"];
                        thayDoi = true;
                    }
                    r["SoLuongTon"] = ton;
                }
                if (thayDoi)
                {
                    CapNhatTong();
                    MessageBox.Show("Giá hoặc khuyến mãi vừa thay đổi. Hóa đơn đã được cập nhật, " +
                                    "vui lòng kiểm tra lại rồi thanh toán.", "Cập nhật hóa đơn",
                                    MessageBoxButtons.OK, MessageBoxIcon.Information);
                    return;
                }

                // 2. Kiểm tra tiền khách đưa (chỉ với tiền mặt)
                bool chuyenKhoan = comboBox2.Text == "Chuyển khoản";
                decimal khachDua = canThanhToan;
                if (!chuyenKhoan)
                {
                    if (!DocTien(textBox3.Text, out khachDua) || khachDua < canThanhToan)
                    {
                        MessageBox.Show("Số tiền khách đưa chưa đủ (cần ít nhất " + canThanhToan.ToString("N0") + " đ).");
                        textBox3.Focus();
                        return;
                    }
                }
                decimal thoiLai = khachDua - canThanhToan;

                string xacNhan = "Cần thanh toán: " + canThanhToan.ToString("N0") + " đ\n" +
                                 "Hình thức: " + comboBox2.Text +
                                 (chuyenKhoan ? "" : "\nKhách đưa: " + khachDua.ToString("N0") + " đ" +
                                                     "\nThối lại: " + thoiLai.ToString("N0") + " đ") +
                                 "\n\nXác nhận hoàn tất thanh toán?";
                if (MessageBox.Show(xacNhan, "Thanh toán", MessageBoxButtons.YesNo,
                                    MessageBoxIcon.Question) != DialogResult.Yes) return;

                // 3. Lập hóa đơn (một transaction: tạo hóa đơn, chi tiết, trừ kho)
                List<DongHoaDon> dong = new List<DongHoaDon>();
                foreach (DataRow r in dtHoaDon.Rows)
                {
                    dong.Add(new DongHoaDon { MaSP = (int)r["MaSP"], SoLuong = (int)r["SoLuong"] });
                }

                int maHD = HoaDonDAL.TaoHoaDon(maNhanVien, null, ChonKhuyenMai(),
                                               chuyenKhoan ? "ChuyenKhoan" : "TienMat", giamGia, dong);

                MessageBox.Show("Thanh toán thành công!\nMã hóa đơn: " + maHD +
                                (chuyenKhoan ? "" : "\nThối lại: " + thoiLai.ToString("N0") + " đ"),
                                "Thanh toán", MessageBoxButtons.OK, MessageBoxIcon.Information);
                HoaDonMoi();
            }
            catch (InvalidOperationException ex) // không đủ tồn kho: lỗi nghiệp vụ, đã rollback
            {
                MessageBox.Show(ex.Message, "Không thể thanh toán", MessageBoxButtons.OK, MessageBoxIcon.Warning);
            }
            catch (Exception ex)
            {
                MessageBox.Show("Lỗi thanh toán: " + ex.Message, "Lỗi", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }
    }
}