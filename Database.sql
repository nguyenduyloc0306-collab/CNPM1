
-- =========================================================
-- DỮ LIỆU MẪU - CSDL CNPM
-- =========================================================
USE CNPM;
GO

-- =========================================================
-- 1. TÀI KHOẢN
-- Lưu ý: VaiTro phải được thêm dữ liệu trước khi chạy phần này.
-- Mật khẩu mẫu: 123456
-- =========================================================
DECLARE @mk VARCHAR(255) =
    CONVERT(VARCHAR(64), HASHBYTES('SHA2_256', '123456'), 2);

IF NOT EXISTS (SELECT 1 FROM TaiKhoan WHERE TenDangNhap = 'admin')
    INSERT INTO TaiKhoan (TenDangNhap, MatKhau, HoTen, MaVaiTro)
    VALUES ('admin', @mk, N'Nguyễn Duy Lộc', 1);

IF NOT EXISTS (SELECT 1 FROM TaiKhoan WHERE TenDangNhap = 'banhang')
    INSERT INTO TaiKhoan (TenDangNhap, MatKhau, HoTen, MaVaiTro)
    VALUES ('banhang', @mk, N'Nguyễn Thị Hoa', 3);

IF NOT EXISTS (SELECT 1 FROM TaiKhoan WHERE TenDangNhap = 'kho')
    INSERT INTO TaiKhoan (TenDangNhap, MatKhau, HoTen, MaVaiTro)
    VALUES ('kho', @mk, N'Đoàn Minh Trọng', 4);

IF NOT EXISTS (SELECT 1 FROM TaiKhoan WHERE TenDangNhap = 'khach')
INSERT INTO TaiKhoan (TenDangNhap, MatKhau, HoTen, MaVaiTro)
    VALUES ('khach', @mk, N'Lê Tín', 5);
GO

-- =========================================================
-- 2. DANH MỤC
-- =========================================================
IF NOT EXISTS (SELECT 1 FROM DanhMuc WHERE TenDanhMuc = N'Đồ bếp')
    INSERT INTO DanhMuc (TenDanhMuc, MoTa)
    VALUES (N'Đồ bếp', N'Dụng cụ nhà bếp');

IF NOT EXISTS (SELECT 1 FROM DanhMuc WHERE TenDanhMuc = N'Điện gia dụng')
    INSERT INTO DanhMuc (TenDanhMuc, MoTa)
    VALUES (N'Điện gia dụng', N'Nồi cơm, quạt, bàn ủi');

IF NOT EXISTS (SELECT 1 FROM DanhMuc WHERE TenDanhMuc = N'Đồ nhựa')
    INSERT INTO DanhMuc (TenDanhMuc, MoTa)
    VALUES (N'Đồ nhựa', N'Hộp, thau, rổ');

SELECT * FROM DanhMuc;
GO

-- =========================================================
-- 3. NHÀ CUNG CẤP
-- =========================================================
IF NOT EXISTS (SELECT 1 FROM NhaCungCap WHERE TenNCC = N'Công ty Hòa Phát')
    INSERT INTO NhaCungCap (TenNCC, DiaChi, ThongTinLienHe)
    VALUES (N'Công ty Hòa Phát', N'TP. Hồ Chí Minh', N'0901234567');

IF NOT EXISTS (SELECT 1 FROM NhaCungCap WHERE TenNCC = N'Công ty Sunhouse')
    INSERT INTO NhaCungCap (TenNCC, DiaChi, ThongTinLienHe)
    VALUES (N'Công ty Sunhouse', N'Hà Nội', N'0912345678');
GO

-- =========================================================
-- 4. SẢN PHẨM
-- Lấy MaDanhMuc theo tên, không giả định mã ID cố định.
-- =========================================================
IF NOT EXISTS (SELECT 1 FROM SanPham WHERE TenSP = N'Nồi cơm điện 1.8L')
    INSERT INTO SanPham
        (TenSP, MaDanhMuc, GiaNhap, GiaBan, SoLuongTon, MucTonToiThieu)
    VALUES
        (N'Nồi cơm điện 1.8L',
         (SELECT MaDanhMuc FROM DanhMuc WHERE TenDanhMuc = N'Điện gia dụng'),
         450000, 590000, 20, 5);

IF NOT EXISTS (SELECT 1 FROM SanPham WHERE TenSP = N'Chảo chống dính 26cm')
    INSERT INTO SanPham
        (TenSP, MaDanhMuc, GiaNhap, GiaBan, SoLuongTon, MucTonToiThieu)
    VALUES
        (N'Chảo chống dính 26cm',
         (SELECT MaDanhMuc FROM DanhMuc WHERE TenDanhMuc = N'Đồ bếp'),
         120000, 169000, 35, 10);

IF NOT EXISTS (SELECT 1 FROM SanPham WHERE TenSP = N'Thau nhựa lớn')
    INSERT INTO SanPham
        (TenSP, MaDanhMuc, GiaNhap, GiaBan, SoLuongTon, MucTonToiThieu)
    VALUES
        (N'Thau nhựa lớn',
         (SELECT MaDanhMuc FROM DanhMuc WHERE TenDanhMuc = N'Đồ nhựa'),
         25000, 39000, 50, 10);

SELECT * FROM SanPham;
GO

-- =========================================================
-- 5. NHÀ CUNG CẤP - SẢN PHẨM
-- =========================================================
IF NOT EXISTS (
    SELECT 1
    FROM NhaCungCap_SanPham nsp
    JOIN NhaCungCap ncc ON ncc.MaNCC = nsp.MaNCC
    JOIN SanPham sp ON sp.MaSP = nsp.MaSP
    WHERE ncc.TenNCC = N'Công ty Hòa Phát'
      AND sp.TenSP = N'Nồi cơm điện 1.8L'
)
    INSERT INTO NhaCungCap_SanPham (MaNCC, MaSP, GiaNhap)
    SELECT ncc.MaNCC, sp.MaSP, 450000
    FROM NhaCungCap ncc
    CROSS JOIN SanPham sp
    WHERE ncc.TenNCC = N'Công ty Hòa Phát'
      AND sp.TenSP = N'Nồi cơm điện 1.8L';

IF NOT EXISTS (
    SELECT 1
    FROM NhaCungCap_SanPham nsp
    JOIN NhaCungCap ncc ON ncc.MaNCC = nsp.MaNCC
    JOIN SanPham sp ON sp.MaSP = nsp.MaSP
    WHERE ncc.TenNCC = N'Công ty Hòa Phát'
      AND sp.TenSP = N'Chảo chống dính 26cm'
)
    INSERT INTO NhaCungCap_SanPham (MaNCC, MaSP, GiaNhap)
    SELECT ncc.MaNCC, sp.MaSP, 120000
    FROM NhaCungCap ncc
    CROSS JOIN SanPham sp
    WHERE ncc.TenNCC = N'Công ty Hòa Phát'
      AND sp.TenSP = N'Chảo chống dính 26cm';

IF NOT EXISTS (
    SELECT 1
    FROM NhaCungCap_SanPham nsp
    JOIN NhaCungCap ncc ON ncc.MaNCC = nsp.MaNCC
    JOIN SanPham sp ON sp.MaSP = nsp.MaSP
    WHERE ncc.TenNCC = N'Công ty Sunhouse'
      AND sp.TenSP = N'Thau nhựa lớn'
)
    INSERT INTO NhaCungCap_SanPham (MaNCC, MaSP, GiaNhap)
    SELECT ncc.MaNCC, sp.MaSP, 25000
    FROM NhaCungCap ncc
    CROSS JOIN SanPham sp
    WHERE ncc.TenNCC = N'Công ty Sunhouse'
      AND sp.TenSP = N'Thau nhựa lớn';

SELECT * FROM NhaCungCap;
SELECT * FROM NhaCungCap_SanPham;
GO
-- =========================================================
-- 6. NHẬP HÀNG
-- Chỉ tạo phiếu mẫu nếu chưa có phiếu nhập tương ứng.
-- =========================================================
DECLARE @nvKho INT =
    (SELECT MaTK FROM TaiKhoan WHERE TenDangNhap = 'kho');

DECLARE @ncc INT =
    (SELECT TOP 1 MaNCC FROM NhaCungCap
     WHERE TenNCC = N'Công ty Hòa Phát');

IF @nvKho IS NULL OR @ncc IS NULL
BEGIN
    RAISERROR(N'Không tìm thấy tài khoản kho hoặc nhà cung cấp.', 16, 1);
END
ELSE
BEGIN
    DECLARE @pn INT;

    INSERT INTO PhieuNhapHang (MaNhanVienKho, MaNCC, TrangThai)
    VALUES (@nvKho, @ncc, N'DaXacNhan');

    SET @pn = CONVERT(INT, SCOPE_IDENTITY());

    INSERT INTO ChiTietPhieuNhap (MaPhieuNhap, MaSP, SoLuong, GiaNhap)
    SELECT TOP 2 @pn, sp.MaSP, 10, sp.GiaNhap
    FROM SanPham sp
    ORDER BY sp.MaSP;

    UPDATE PhieuNhapHang
    SET TongTien = (
        SELECT SUM(ThanhTien)
        FROM ChiTietPhieuNhap
        WHERE MaPhieuNhap = @pn
    )
    WHERE MaPhieuNhap = @pn;
END;

SELECT * FROM PhieuNhapHang;
GO

-- =========================================================
-- 7. KHUYẾN MÃI
-- =========================================================
IF NOT EXISTS (
    SELECT 1 FROM KhuyenMai
    WHERE TenKhuyenMai = N'Giảm 10% tháng 10'
)
    INSERT INTO KhuyenMai
        (TenKhuyenMai, LoaiGiam, GiaTriGiam,
         NgayBatDau, NgayKetThuc, DieuKienApDung)
    VALUES
        (N'Giảm 10% tháng 10', N'PhanTram', 10,
         '20261001', '20261031', N'Mọi đơn hàng');

SELECT * FROM KhuyenMai;
GO

-- =========================================================
-- 8. HÓA ĐƠN
-- =========================================================
DECLARE @nvBanHang INT =
    (SELECT MaTK FROM TaiKhoan WHERE TenDangNhap = 'banhang');

IF @nvBanHang IS NULL
BEGIN
    RAISERROR(N'Không tìm thấy tài khoản bán hàng.', 16, 1);
END
ELSE
BEGIN
    DECLARE @hd INT;

    INSERT INTO HoaDon
        (MaNhanVien, MaKhachHang, MaKhuyenMai,
         HinhThucThanhToan, TienGiamGia)
    VALUES
        (@nvBanHang, NULL, NULL, N'TienMat', 10000);

    SET @hd = CONVERT(INT, SCOPE_IDENTITY());

    INSERT INTO ChiTietHoaDon (MaHoaDon, MaSP, SoLuong, DonGia)
    SELECT TOP 2 @hd, MaSP, 2, GiaBan
    FROM SanPham
    ORDER BY MaSP;

    UPDATE HoaDon
    SET TongTienHang = (
        SELECT SUM(ThanhTien)
        FROM ChiTietHoaDon
        WHERE MaHoaDon = @hd
    )
    WHERE MaHoaDon = @hd;
END;

SELECT * FROM HoaDon;
SELECT * FROM ChiTietHoaDon;
GO

INSERT INTO GioHang (MaKhachHang, MaSP, SoLuong)
SELECT 4, v.MaSP, v.SoLuong
FROM (VALUES
        (1, 1),   -- Nồi cơm điện 1.8L        x1 (tồn 20)
        (2, 2),   -- Chảo chống dính 26cm     x2 (tồn 35)
        (3, 3)    -- Thau nhựa lớn            x3 (tồn 50)
     ) AS v (MaSP, SoLuong)
WHERE EXISTS (SELECT 1 FROM SanPham s WHERE s.MaSP = v.MaSP AND s.TrangThai = 1)
  AND NOT EXISTS (SELECT 1 FROM GioHang g WHERE g.MaKhachHang = 4 AND g.MaSP = v.MaSP);
GO