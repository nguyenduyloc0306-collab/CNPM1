using System;
using System.Data;
using System.Data.SqlClient;

namespace CNPM1
{
	public static class QuanLyKhoDAL
	{
		public static DataTable LayDanhSachSanPham(string tuKhoa)
		{
			return DbHelper.ExecuteQuery(
				@"SELECT MaSP, TenSP, SoLuongTon, MucTonToiThieu, MoTa 
                  FROM SanPham 
                  WHERE TrangThai = 1 
                    AND (CAST(MaSP AS VARCHAR(10)) LIKE @k OR TenSP LIKE @k)
                  ORDER BY MaSP",
				DbHelper.Param("@k", "%" + (tuKhoa ?? "") + "%"));
		}

		public static DataTable LayDanhSachLichSu(string tuKhoa = "")
		{
			if (string.IsNullOrEmpty(tuKhoa))
			{
				return DbHelper.ExecuteQuery(
					@"SELECT ls.MaDieuChinh, ls.MaSP, sp.TenSP, t.HoTen AS TenNhanVienKho, 
                     ls.SoLuongHeThong, ls.SoLuongThucTe, ls.ChenhLech, ls.NgayDieuChinh, ls.LyDo
					FROM LichSuDieuChinhTonKho ls
					JOIN TaiKhoan t ON ls.MaNhanVienKho = t.MaTK
					JOIN SanPham sp ON ls.MaSP = sp.MaSP
					ORDER BY ls.NgayDieuChinh DESC");
			}

			return DbHelper.ExecuteQuery(
				@"SELECT ls.MaDieuChinh, ls.MaSP, sp.TenSP, t.HoTen AS TenNhanVienKho, 
                 ls.SoLuongHeThong, ls.SoLuongThucTe, ls.ChenhLech, ls.NgayDieuChinh, ls.LyDo
				FROM LichSuDieuChinhTonKho ls
				JOIN TaiKhoan t ON ls.MaNhanVienKho = t.MaTK
				JOIN SanPham sp ON ls.MaSP = sp.MaSP
				WHERE CAST(ls.MaSP AS VARCHAR(10)) LIKE @k
				ORDER BY ls.NgayDieuChinh DESC",
				DbHelper.Param("@k", "%" + tuKhoa.Trim() + "%"));
		}

		public static bool ThucHienDieuChinh(int maSP, int maNhanVien, int soLuongHeThong, int soLuongThucTe, string lyDo)
		{
			try
			{
				// Sử dụng cơ chế Transaction của DbHelper để bảo vệ dữ liệu
				DbHelper.ExecuteTransaction((conn, tran) =>
				{
					// 1. Thêm bản ghi vào bảng lịch sử
					string sqlLichSu = @"INSERT INTO LichSuDieuChinhTonKho (MaSP, MaNhanVienKho, SoLuongHeThong, SoLuongThucTe, LyDo)
                                         VALUES (@maSP, @maNV, @slht, @sltt, @lyDo)";
					DbHelper.ExecuteNonQuery(conn, tran, sqlLichSu,
						DbHelper.Param("@maSP", maSP),
						DbHelper.Param("@maNV", maNhanVien),
						DbHelper.Param("@slht", soLuongHeThong),
						DbHelper.Param("@sltt", soLuongThucTe),
						DbHelper.Param("@lyDo", string.IsNullOrEmpty(lyDo) ? (object)DBNull.Value : lyDo));

					// 2. Cập nhật số lượng tồn mới cho sản phẩm
					string sqlSanPham = @"UPDATE SanPham SET SoLuongTon = @sltt WHERE MaSP = @maSP";
					DbHelper.ExecuteNonQuery(conn, tran, sqlSanPham,
						DbHelper.Param("@sltt", soLuongThucTe),
						DbHelper.Param("@maSP", maSP));
				});

				return true; // Thành công hoàn toàn
			}
			catch (Exception ex)
			{
				// Nếu có lỗi, transaction tự động rollback, trả về false
				return false;
			}
		}
	}
}