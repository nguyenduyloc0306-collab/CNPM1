using System;
using System.Data;
using System.Data.SqlClient;

namespace CNPM1
{
	public class NhapHangDAL
	{
		// Chuỗi kết nối đến cơ sở dữ liệu (Bạn thay đổi DataSource/Initial Catalog cho phù hợp với máy của bạn)
		private string connectionString = "Data Source=.\\SQLEXPRESS;Initial Catalog=CNPM;Integrated Security=True";

		// 1. Lấy danh sách tất cả phiếu nhập hiển thị lên dataGridViewPhieuNhap
		public DataTable LayDanhSachPhieuNhap()
		{
			DataTable dt = new DataTable();
			string query = @"SELECT p.MaPhieuNhap, n.TenNCC, t.HoTen AS NVKho, 
                            p.NgayNhap, p.TongTien, 
                            CASE 
                                WHEN p.TrangThai = N'ChoXacNhan' THEN N'Chờ xác nhận'
                                WHEN p.TrangThai = N'DaXacNhan' THEN N'Đã xác nhận'
                                ELSE p.TrangThai 
                            END AS TrangThai 
                     FROM PhieuNhapHang p
                     JOIN NhaCungCap n ON p.MaNCC = n.MaNCC
                     JOIN TaiKhoan t ON p.MaNhanVienKho = t.MaTK
                     ORDER BY p.NgayNhap DESC";

			using (SqlConnection conn = new SqlConnection(connectionString))
			{
				using (SqlCommand cmd = new SqlCommand(query, conn))
				{
					using (SqlDataAdapter da = new SqlDataAdapter(cmd))
					{
						da.Fill(dt);
					}
				}
			}
			return dt;
		}

		// 2. Lấy danh sách Nhà Cung Cấp cho ComboBox
		public DataTable LayDanhSachNCC()
		{
			DataTable dt = new DataTable();
			string query = "SELECT MaNCC, TenNCC FROM NhaCungCap WHERE TrangThai = 1";

			using (SqlConnection conn = new SqlConnection(connectionString))
			{
				using (SqlCommand cmd = new SqlCommand(query, conn))
				{
					using (SqlDataAdapter da = new SqlDataAdapter(cmd))
					{
						da.Fill(dt);
					}
				}
			}
			return dt;
		}

		// 3. Lấy danh sách Sản Phẩm theo Mã Nhà Cung Cấp (từ bảng NhaCungCap_SanPham)
		public DataTable LaySanPhamTheoNCC(int maNCC)
		{
			DataTable dt = new DataTable();
			string query = @"SELECT s.MaSP, s.TenSP, ncp.GiaNhap 
                             FROM NhaCungCap_SanPham ncp
                             JOIN SanPham s ON ncp.MaSP = s.MaSP
                             WHERE ncp.MaNCC = @MaNCC AND ncp.TrangThai = 1 AND s.TrangThai = 1";

			using (SqlConnection conn = new SqlConnection(connectionString))
			{
				using (SqlCommand cmd = new SqlCommand(query, conn))
				{
					cmd.Parameters.AddWithValue("@MaNCC", maNCC);
					using (SqlDataAdapter da = new SqlDataAdapter(cmd))
					{
						da.Fill(dt);
					}
				}
			}
			return dt;
		}

		// 4. Tạo mới phiếu nhập (âm thầm) khi bấm nút Tạo Mới, trả về MaPhieuNhap vừa tạo
		public int TaoPhieuNhapMoi(int maNhanVienKho, int maNCC)
		{
			int maPhieuNhapMoi = -1;
			string query = @"INSERT INTO PhieuNhapHang (MaNhanVienKho, MaNCC, NgayNhap, TongTien, TrangThai) 
                             OUTPUT INSERTED.MaPhieuNhap
                             VALUES (@MaNV, @MaNCC, GETDATE(), 0, N'ChoXacNhan')";

			using (SqlConnection conn = new SqlConnection(connectionString))
			{
				using (SqlCommand cmd = new SqlCommand(query, conn))
				{
					cmd.Parameters.AddWithValue("@MaNV", maNhanVienKho);
					cmd.Parameters.AddWithValue("@MaNCC", maNCC);

					conn.Open();
					object result = cmd.ExecuteScalar();
					if (result != null && int.TryParse(result.ToString(), out int id))
					{
						maPhieuNhapMoi = id;
					}
				}
			}
			return maPhieuNhapMoi;
		}

		// 5. Thêm sản phẩm vào ChiTietPhieuNhap và cập nhật lại Tổng Tiền của PhieuNhapHang
		public bool ThemChiTietPhieuNhap(int maPhieuNhap, int maSP, int soLuong, decimal giaNhap)
		{
			using (SqlConnection conn = new SqlConnection(connectionString))
			{
				conn.Open();
				SqlTransaction transaction = conn.BeginTransaction();

				try
				{
					// Bước 1: Thêm vào bảng ChiTietPhieuNhap (Thành tiền tự tính trong CSDL nhờ cột PERSISTED)
					string queryInsertCT = @"INSERT INTO ChiTietPhieuNhap (MaPhieuNhap, MaSP, SoLuong, GiaNhap) 
                                             VALUES (@MaPhieu, @MaSP, @SoLuong, @GiaNhap)";
					using (SqlCommand cmd1 = new SqlCommand(queryInsertCT, conn, transaction))
					{
						cmd1.Parameters.AddWithValue("@MaPhieu", maPhieuNhap);
						cmd1.Parameters.AddWithValue("@MaSP", maSP);
						cmd1.Parameters.AddWithValue("@SoLuong", soLuong);
						cmd1.Parameters.AddWithValue("@GiaNhap", giaNhap);
						cmd1.ExecuteNonQuery();
					}

					// Bước 2: Cập nhật lại tổng tiền cho PhieuNhapHang
					string queryUpdateTotal = @"UPDATE PhieuNhapHang 
                                                SET TongTien = (SELECT ISNULL(SUM(ThanhTien), 0) FROM ChiTietPhieuNhap WHERE MaPhieuNhap = @MaPhieu) 
                                                WHERE MaPhieuNhap = @MaPhieu";
					using (SqlCommand cmd2 = new SqlCommand(queryUpdateTotal, conn, transaction))
					{
						cmd2.Parameters.AddWithValue("@MaPhieu", maPhieuNhap);
						cmd2.ExecuteNonQuery();
					}

					transaction.Commit();
					return true;
				}
				catch (Exception)
				{
					transaction.Rollback();
					return false;
				}
			}
		}

		// 6. Lấy danh sách chi tiết phiếu nhập theo MaPhieuNhap (dùng cho xem chi tiết hoặc khi đang tạo)
		public DataTable LayChiTietPhieuNhap(int maPhieuNhap)
		{
			DataTable dt = new DataTable();
			string query = @"SELECT c.MaPhieuNhap, c.MaSP, s.TenSP, c.SoLuong, c.GiaNhap, c.ThanhTien 
                             FROM ChiTietPhieuNhap c
                             JOIN SanPham s ON c.MaSP = s.MaSP
                             WHERE c.MaPhieuNhap = @MaPhieu";

			using (SqlConnection conn = new SqlConnection(connectionString))
			{
				using (SqlCommand cmd = new SqlCommand(query, conn))
				{
					cmd.Parameters.AddWithValue("@MaPhieu", maPhieuNhap);
					using (SqlDataAdapter da = new SqlDataAdapter(cmd))
					{
						da.Fill(dt);
					}
				}
			}
			return dt;
		}

		// 7. Xóa phiếu nhập và chi tiết khi người dùng bấm Hủy lúc đang tạo mới
		public bool XoaPhieuNhapTam(int maPhieuNhap)
		{
			using (SqlConnection conn = new SqlConnection(connectionString))
			{
				conn.Open();
				SqlTransaction transaction = conn.BeginTransaction();

				try
				{
					// Xóa chi tiết trước vì vướng khóa ngoại
					string queryDelCT = "DELETE FROM ChiTietPhieuNhap WHERE MaPhieuNhap = @MaPhieu";
					using (SqlCommand cmd1 = new SqlCommand(queryDelCT, conn, transaction))
					{
						cmd1.Parameters.AddWithValue("@MaPhieu", maPhieuNhap);
						cmd1.ExecuteNonQuery();
					}

					// Xóa phiếu nhập chính
					string queryDelPhieu = "DELETE FROM PhieuNhapHang WHERE MaPhieuNhap = @MaPhieu";
					using (SqlCommand cmd2 = new SqlCommand(queryDelPhieu, conn, transaction))
					{
						cmd2.Parameters.AddWithValue("@MaPhieu", maPhieuNhap);
						cmd2.ExecuteNonQuery();
					}

					transaction.Commit();
					return true;
				}
				catch (Exception)
				{
					transaction.Rollback();
					return false;
				}
			}
		}

		// 8. Xác nhận phiếu nhập (Đổi trạng thái từ 'ChoXacNhan' sang 'DaXacNhan')
		public bool XacNhanPhieuNhap(int maPhieuNhap)
		{
			string query = "UPDATE PhieuNhapHang SET TrangThai = N'DaXacNhan' WHERE MaPhieuNhap = @MaPhieu";

			using (SqlConnection conn = new SqlConnection(connectionString))
			{
				using (SqlCommand cmd = new SqlCommand(query, conn))
				{
					cmd.Parameters.AddWithValue("@MaPhieu", maPhieuNhap);
					conn.Open();
					return cmd.ExecuteNonQuery() > 0;
				}
			}
		}

		// 9. Cập nhật số lượng sản phẩm khi phiếu nhập trong trạng thái chờ
		public bool SuaChiTietPhieuNhap(int maPhieuNhap, int maSP, int soLuongMoi, decimal giaNhapMoi)
		{
			using (SqlConnection conn = new SqlConnection(connectionString))
			{
				conn.Open();
				SqlTransaction transaction = conn.BeginTransaction();

				try
				{
					// Bước 1: Update bảng ChiTietPhieuNhap
					string queryUpdateCT = @"UPDATE ChiTietPhieuNhap 
                                     SET SoLuong = @SoLuong, GiaNhap = @GiaNhap 
                                     WHERE MaPhieuNhap = @MaPhieu AND MaSP = @MaSP";
					using (SqlCommand cmd1 = new SqlCommand(queryUpdateCT, conn, transaction))
					{
						cmd1.Parameters.AddWithValue("@SoLuong", soLuongMoi);
						cmd1.Parameters.AddWithValue("@GiaNhap", giaNhapMoi);
						cmd1.Parameters.AddWithValue("@MaPhieu", maPhieuNhap);
						cmd1.Parameters.AddWithValue("@MaSP", maSP);
						cmd1.ExecuteNonQuery();
					}

					// Bước 2: Cập nhật lại tổng tiền cho PhieuNhapHang
					string queryUpdateTotal = @"UPDATE PhieuNhapHang 
                                        SET TongTien = (SELECT ISNULL(SUM(ThanhTien), 0) FROM ChiTietPhieuNhap WHERE MaPhieuNhap = @MaPhieu) 
                                        WHERE MaPhieuNhap = @MaPhieu";
					using (SqlCommand cmd2 = new SqlCommand(queryUpdateTotal, conn, transaction))
					{
						cmd2.Parameters.AddWithValue("@MaPhieu", maPhieuNhap);
						cmd2.ExecuteNonQuery();
					}

					transaction.Commit();
					return true;
				}
				catch (Exception)
				{
					transaction.Rollback();
					return false;
				}
			}
		}
	}
}