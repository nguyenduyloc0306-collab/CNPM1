using System;
using System.Data;
using System.Data.SqlClient;

namespace CNPM1
{
	public static class NhapHangDAL
	{
		public static DataTable LayDanhSachSanPham()
		{
			return DbHelper.ExecuteQuery("SELECT MaSP, TenSP FROM SanPham");
		}

		// Lấy danh sách tất cả phiếu nhập hàng (Khớp với các cột trên dataGridViewPhieuNhap)
		public static DataTable LayDanhSachPhieuNhap()
{
    // Bỏ JOIN đi để test xem dữ liệu có lên không
    return DbHelper.ExecuteQuery(
        @"SELECT MaPhieuNhap AS colMaPhieu, 
                 MaNCC AS colNCC, 
                 MaNhanVienKho AS colNVKho, 
                 NgayNhap AS colNgayNhap, 
                 TongTien AS colTongtien, 
                 TrangThai AS colTrangThai
          FROM PhieuNhapHang");
}

		// Lấy danh sách nhà cung cấp cho ComboBox
		public static DataTable LayDanhSachNCC()
		{
			return DbHelper.ExecuteQuery(@"SELECT MaNCC, TenNCC FROM NhaCungCap");
		}

		// Lấy danh sách chi tiết của một phiếu nhập cụ thể
		public static DataTable LayChiTietPhieuNhap(int maPhieuNhap)
		{
			return DbHelper.ExecuteQuery(
				@"SELECT ctpn.MaPhieuNhap AS colMaPhieu2, ctpn.MaSP AS colMaSP, 
                         sp.TenSP, ctpn.SoLuong AS colSoLuong, ctpn.GiaNhap AS colDonGia, ctpn.ThanhTien AS colThanhTien
                  FROM ChiTietPhieuNhap ctpn
                  JOIN SanPham sp ON ctpn.MaSP = sp.MaSP
                  WHERE ctpn.MaPhieuNhap = @maPhieu",
				DbHelper.Param("@maPhieu", maPhieuNhap));
		}

		// Tạo mới phiếu nhập hàng kèm theo danh sách chi tiết sản phẩm (Dùng Transaction)
		public static bool TaoPhieuNhap(int maNhanVienKho, int maNCC, DataTable dtChiTiet, out int maPhieuNhapMoi)
		{
			int generatedId = -1;
			try
			{
				DbHelper.ExecuteTransaction((conn, tran) =>
				{
					// Bước 1: Thêm vào bảng PhieuNhapHang và lấy lại MaPhieuNhap vừa sinh ra
					string sqlPhieuNhap = @"INSERT INTO PhieuNhapHang (MaNhanVienKho, MaNCC, NgayNhap, TongTien, TrangThai)
                                            OUTPUT INSERTED.MaPhieuNhap
                                            VALUES (@maNV, @maNCC, GETDATE(), 0, N'ChoXacNhan')";

					object result = DbHelper.ExecuteScalar(conn, tran, sqlPhieuNhap,
						DbHelper.Param("@maNV", maNhanVienKho),
						DbHelper.Param("@maNCC", maNCC));

					generatedId = Convert.ToInt32(result);

					decimal tongTienPhieu = 0;

					// Bước 2: Thêm từng sản phẩm trong danh sách chi tiết vào bảng ChiTietPhieuNhap
					foreach (DataRow row in dtChiTiet.Rows)
					{
						int maSP = Convert.ToInt32(row["MaSP"]);
						int soLuong = Convert.ToInt32(row["SoLuong"]);
						decimal giaNhap = Convert.ToDecimal(row["GiaNhap"]);
						decimal thanhTien = soLuong * giaNhap;

						tongTienPhieu += thanhTien;

						string sqlChiTiet = @"INSERT INTO ChiTietPhieuNhap (MaPhieuNhap, MaSP, SoLuong, GiaNhap)
                                              VALUES (@maPhieu, @maSP, @soLuong, @giaNhap)";
						DbHelper.ExecuteNonQuery(conn, tran, sqlChiTiet,
							DbHelper.Param("@maPhieu", generatedId),
							DbHelper.Param("@maSP", maSP),
							DbHelper.Param("@soLuong", soLuong),
							DbHelper.Param("@giaNhap", giaNhap));
					}

					// Bước 3: Cập nhật lại tổng tiền cho phiếu nhập vừa tạo
					string sqlUpdateTongTien = @"UPDATE PhieuNhapHang SET TongTien = @tongTien WHERE MaPhieuNhap = @maPhieu";
					DbHelper.ExecuteNonQuery(conn, tran, sqlUpdateTongTien,
						DbHelper.Param("@tongTien", tongTienPhieu),
						DbHelper.Param("@maPhieu", generatedId));
				});

				maPhieuNhapMoi = generatedId;
				return true;
			}
			catch (Exception ex)
			{
				System.Windows.Forms.MessageBox.Show("Lỗi thật sự là: " + ex.Message, "Chi tiết lỗi DAL", System.Windows.Forms.MessageBoxButtons.OK, System.Windows.Forms.MessageBoxIcon.Error);

				maPhieuNhapMoi = -1;
				return false;
			}
		}

		// Xác nhận phiếu nhập (Chuyển trạng thái từ ChoXacNhan sang DaXacNhan)
		public static bool XacNhanPhieuNhap(int maPhieuNhap)
		{
			try
			{
				DbHelper.ExecuteTransaction((conn, tran) =>
				{
					// Cập nhật trạng thái phiếu nhập thành 'DaXacNhan'
					string sqlUpdate = @"UPDATE PhieuNhapHang SET TrangThai = N'DaXacNhan' WHERE MaPhieuNhap = @maPhieu";
					DbHelper.ExecuteNonQuery(conn, tran, sqlUpdate, DbHelper.Param("@maPhieu", maPhieuNhap));

					// Cộng dồn số lượng tồn kho cho sản phẩm
					string sqlLayChiTiet = @"SELECT MaSP, SoLuong FROM ChiTietPhieuNhap WHERE MaPhieuNhap = @maPhieu";
					DataTable dtCT = DbHelper.ExecuteQuery(sqlLayChiTiet, DbHelper.Param("@maPhieu", maPhieuNhap));

					foreach (DataRow row in dtCT.Rows)
					{
						int maSP = Convert.ToInt32(row["MaSP"]);
						int soLuongNhap = Convert.ToInt32(row["SoLuong"]);

						string sqlUpdateTonKho = @"UPDATE SanPham SET SoLuongTon = SoLuongTon + @sl WHERE MaSP = @maSP";
						DbHelper.ExecuteNonQuery(conn, tran, sqlUpdateTonKho,
							DbHelper.Param("@sl", soLuongNhap),
							DbHelper.Param("@maSP", maSP));
					}
				});

				return true;
			}
			catch (Exception)
			{
				return false;
			}
		}
	}
}