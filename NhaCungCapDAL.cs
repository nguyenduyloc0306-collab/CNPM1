using System;
using System.Data;
using System.Data.SqlClient;

public class NhaCungCapDAL
{
	// Đổi chuỗi kết nối này theo tên Server CSDL của bạn
	private static string connectionString = @"Data Source=.\SQLEXPRESS;Initial Catalog=CNPM;Integrated Security=True";

	// 1. Lấy danh sách nhà cung cấp
	public static DataTable LayDanhSach(string tuKhoa = "")
	{
		DataTable dt = new DataTable();
		using (SqlConnection conn = new SqlConnection(connectionString))
		{
			string query = @"SELECT MaNCC, TenNCC, DiaChi, Email, ThongTinLienHe, MoTa, TrangThai 
                             FROM NhaCungCap";

			if (!string.IsNullOrWhiteSpace(tuKhoa))
			{
				query += " WHERE TenNCC LIKE @tuKhoa OR CAST(MaNCC AS VARCHAR) LIKE @tuKhoa OR Email LIKE @tuKhoa";
			}

			using (SqlCommand cmd = new SqlCommand(query, conn))
			{
				if (!string.IsNullOrWhiteSpace(tuKhoa))
				{
					cmd.Parameters.AddWithValue("@tuKhoa", "%" + tuKhoa.Trim() + "%");
				}

				using (SqlDataAdapter da = new SqlDataAdapter(cmd))
				{
					da.Fill(dt);
				}
			}
		}
		return dt;
	}

	// 2. Thêm nhà cung cấp mới
	public static bool Them(string tenNCC, string diaChi, string email, string sdt, string moTa, int trangThai)
	{
		using (SqlConnection conn = new SqlConnection(connectionString))
		{
			string query = @"INSERT INTO NhaCungCap (TenNCC, DiaChi, Email, ThongTinLienHe, MoTa, TrangThai) 
                             VALUES (@tenNCC, @diaChi, @email, @sdt, @moTa, @trangThai)";

			using (SqlCommand cmd = new SqlCommand(query, conn))
			{
				cmd.Parameters.AddWithValue("@tenNCC", tenNCC);
				cmd.Parameters.AddWithValue("@diaChi", string.IsNullOrEmpty(diaChi) ? (object)DBNull.Value : diaChi);
				cmd.Parameters.AddWithValue("@email", string.IsNullOrEmpty(email) ? (object)DBNull.Value : email);
				cmd.Parameters.AddWithValue("@sdt", string.IsNullOrEmpty(sdt) ? (object)DBNull.Value : sdt);
				cmd.Parameters.AddWithValue("@moTa", string.IsNullOrEmpty(moTa) ? (object)DBNull.Value : moTa);
				cmd.Parameters.AddWithValue("@trangThai", trangThai);

				conn.Open();
				return cmd.ExecuteNonQuery() > 0;
			}
		}
	}

	// 3. Cập nhật thông tin nhà cung cấp
	public static bool Sua(int maNCC, string tenNCC, string diaChi, string email, string sdt, string moTa, int trangThai)
	{
		using (SqlConnection conn = new SqlConnection(connectionString))
		{
			string query = @"UPDATE NhaCungCap 
                             SET TenNCC = @tenNCC, 
                                 DiaChi = @diaChi, 
                                 Email = @email, 
                                 ThongTinLienHe = @sdt, 
                                 MoTa = @moTa, 
                                 TrangThai = @trangThai 
                             WHERE MaNCC = @maNCC";

			using (SqlCommand cmd = new SqlCommand(query, conn))
			{
				cmd.Parameters.AddWithValue("@maNCC", maNCC);
				cmd.Parameters.AddWithValue("@tenNCC", tenNCC);
				cmd.Parameters.AddWithValue("@diaChi", string.IsNullOrEmpty(diaChi) ? (object)DBNull.Value : diaChi);
				cmd.Parameters.AddWithValue("@email", string.IsNullOrEmpty(email) ? (object)DBNull.Value : email);
				cmd.Parameters.AddWithValue("@sdt", string.IsNullOrEmpty(sdt) ? (object)DBNull.Value : sdt);
				cmd.Parameters.AddWithValue("@moTa", string.IsNullOrEmpty(moTa) ? (object)DBNull.Value : moTa);
				cmd.Parameters.AddWithValue("@trangThai", trangThai);

				conn.Open();
				return cmd.ExecuteNonQuery() > 0;
			}
		}
	}

	//Phần này file khác gọi, không phải NhaCungCap
	public static DataTable LayTatCa()
	{
		return LayDanhSach("");
	}

	// Bổ sung hàm LaySanPhamCuaNCC (tránh lỗi ở form Nhập Hàng)
	public static DataTable LaySanPhamCuaNCC(int maNCC)
	{
		DataTable dt = new DataTable();
		using (SqlConnection conn = new SqlConnection(connectionString))
		{
			// Lấy danh sách sản phẩm thuộc nhà cung cấp này (tùy thuộc cấu trúc bảng SanPham của bạn có cột MaNCC hay không)
			string query = "SELECT * FROM SanPham WHERE MaNCC = @MaNCC";
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
}