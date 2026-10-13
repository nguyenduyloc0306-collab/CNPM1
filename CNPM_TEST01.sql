/* =====================================================================
   CSDL: CNPM
   Đề tài: QUẢN LÝ CỬA HÀNG GIA DỤNG - Nhóm 12
   Xây dựng dựa trên Chương 2 (Danh sách yêu cầu chức năng nghiệp vụ)
   của tài liệu Xác định yêu cầu:
     - QLTK: Quản lý tài khoản
     - QLDM: Quản lý danh mục
     - QLSP: Quản lý sản phẩm
     - QLNCC: Quản lý nhà cung cấp
     - QLNH: Quản lý nhập hàng
     - QLKH: Quản lý kho hàng
     - QLBH: Quản lý bán hàng
     - QLKM: Quản lý khuyến mãi
     - BCTK: Báo cáo - Thống kê
   DBMS: Microsoft SQL Server
   ===================================================================== */

IF DB_ID('CNPM') IS NOT NULL
BEGIN
    ALTER DATABASE CNPM SET SINGLE_USER WITH ROLLBACK IMMEDIATE;
    DROP DATABASE CNPM;
END
GO

CREATE DATABASE CNPM;
GO

USE CNPM;
GO

/* =====================================================================
   1. QLTK - QUẢN LÝ TÀI KHOẢN
   Vai trò: Quản trị viên, Quản lý cửa hàng, Nhân viên bán hàng,
            Nhân viên kho, Khách hàng (mục 3.1 - Nhận diện tác nhân)
   ===================================================================== */
CREATE TABLE VaiTro (
    MaVaiTro    INT IDENTITY(1,1) PRIMARY KEY,
    TenVaiTro   NVARCHAR(50) NOT NULL UNIQUE
);
GO

SELECT * FROM VaiTro

/*
1: Quan tri vien
2: Quan ly cua hang
3: Nhan vien ban hang
4: Nhan vien kho
*/

CREATE TABLE TaiKhoan (
    MaTK            INT IDENTITY(1,1) PRIMARY KEY,
    TenDangNhap     VARCHAR(50)   NOT NULL UNIQUE,
    MatKhau         VARCHAR(255)  NOT NULL,      -- lưu dạng băm (Bcrypt/SHA-256), theo YCPCN mục Bảo mật
    HoTen           NVARCHAR(100) NOT NULL,
    NgaySinh        DATE          NULL,
    SoDienThoai     VARCHAR(15)   NULL,
    DiaChi          NVARCHAR(255) NULL,
    Email           VARCHAR(100)  NULL,
    MaVaiTro        INT           NOT NULL,
    TrangThai       BIT           NOT NULL DEFAULT 1,   -- 1: hoạt động, 0: bị khóa (QLTK - Khóa tài khoản)
    NgayTao         DATETIME      NOT NULL DEFAULT GETDATE(),
    CONSTRAINT FK_TaiKhoan_VaiTro FOREIGN KEY (MaVaiTro) REFERENCES VaiTro(MaVaiTro)
);
GO
SELECT *
FROM TaiKhoan
/* =====================================================================
   2. QLDM - QUẢN LÝ DANH MỤC SẢN PHẨM
   ===================================================================== */
CREATE TABLE DanhMuc (
    MaDanhMuc   INT IDENTITY(1,1) PRIMARY KEY,
    TenDanhMuc  NVARCHAR(100) NOT NULL,
    MoTa        NVARCHAR(255) NULL,
    TrangThai   BIT NOT NULL DEFAULT 1     -- 1: đang dùng, 0: đã khóa (QLDM - Khóa danh mục)
);
GO

/* =====================================================================
   3. QLNCC - QUẢN LÝ NHÀ CUNG CẤP  (theo biểu mẫu QLNCC_BM1)
   ===================================================================== */
CREATE TABLE NhaCungCap (
    MaNCC           INT IDENTITY(1,1) PRIMARY KEY,
    TenNCC          NVARCHAR(150) NOT NULL,
    DiaChi          NVARCHAR(255) NULL,
    Email           VARCHAR(100)  NULL,
    ThongTinLienHe  NVARCHAR(255) NULL,
    MoTa            NVARCHAR(500) NULL,
    TrangThai       BIT NOT NULL DEFAULT 1  -- 1: đang hợp tác, 0: ngừng hợp tác
);
GO

/* =====================================================================
   4. QLSP - QUẢN LÝ SẢN PHẨM  (theo biểu mẫu QLSP_BM1)
   ===================================================================== */
CREATE TABLE SanPham (
    MaSP            INT IDENTITY(1,1) PRIMARY KEY,
    TenSP           NVARCHAR(150) NOT NULL,
    MaDanhMuc       INT NOT NULL,
    GiaNhap         DECIMAL(18,2) NOT NULL CHECK (GiaNhap >= 0),   -- giá nhập tham khảo/gần nhất
    GiaBan          DECIMAL(18,2) NOT NULL CHECK (GiaBan >= 0),
    SoLuongTon      INT NOT NULL DEFAULT 0 CHECK (SoLuongTon >= 0),
    MucTonToiThieu  INT NOT NULL DEFAULT 5,     -- dùng cho QLKH - Cảnh báo sắp hết hàng
    DonViTinh       NVARCHAR(20)  NULL,
    MoTa            NVARCHAR(1000) NULL,
    HinhAnh         VARCHAR(255)  NULL,
    TrangThai       BIT NOT NULL DEFAULT 1,     -- 1: đang bán, 0: ngừng bán
    NgayTao         DATETIME NOT NULL DEFAULT GETDATE(),
    CONSTRAINT FK_SanPham_DanhMuc FOREIGN KEY (MaDanhMuc) REFERENCES DanhMuc(MaDanhMuc)
);
GO

/* ---------------------------------------------------------------------
   Bảng trung gian (n-n): một sản phẩm có thể nhập từ nhiều nhà cung
   cấp, mỗi NCC báo một giá nhập riêng cho sản phẩm đó.
   --------------------------------------------------------------------- */
CREATE TABLE NhaCungCap_SanPham (
    MaNCC       INT NOT NULL,
    MaSP        INT NOT NULL,
    GiaNhap     DECIMAL(18,2) NOT NULL CHECK (GiaNhap >= 0),  -- giá nhập của riêng NCC này
    TrangThai   BIT NOT NULL DEFAULT 1,   -- 1: còn cung cấp, 0: NCC này ngừng cung cấp SP này
    PRIMARY KEY (MaNCC, MaSP),
    CONSTRAINT FK_NCCSP_NCC FOREIGN KEY (MaNCC) REFERENCES NhaCungCap(MaNCC),
    CONSTRAINT FK_NCCSP_SP  FOREIGN KEY (MaSP)  REFERENCES SanPham(MaSP)
);
GO

INSERT INTO NhaCungCap_SanPham (MaNCC, MaSP, GiaNhap) VALUES
 (1, 1, 450000), (1, 2, 120000), (2, 3, 25000);

SELECT * FROM NhaCungCap; SELECT * FROM NhaCungCap_SanPham;

/* =====================================================================
   5. QLNH - QUẢN LÝ NHẬP HÀNG  (theo biểu mẫu QLNH_BM1: PHIẾU NHẬP HÀNG)
   ===================================================================== */
CREATE TABLE PhieuNhapHang (
    MaPhieuNhap     INT IDENTITY(1,1) PRIMARY KEY,
    MaNhanVienKho   INT NOT NULL,               -- FK -> TaiKhoan (vai trò Nhân viên kho)
    MaNCC           INT NOT NULL,
    NgayNhap        DATETIME NOT NULL DEFAULT GETDATE(),
    TongTien        DECIMAL(18,2) NOT NULL DEFAULT 0,
    TrangThai       NVARCHAR(20) NOT NULL DEFAULT N'ChoXacNhan'
                     CHECK (TrangThai IN (N'ChoXacNhan', N'DaXacNhan')),
    CONSTRAINT FK_PhieuNhap_NhanVien FOREIGN KEY (MaNhanVienKho) REFERENCES TaiKhoan(MaTK),
    CONSTRAINT FK_PhieuNhap_NCC      FOREIGN KEY (MaNCC)         REFERENCES NhaCungCap(MaNCC)
);
GO

CREATE TABLE ChiTietPhieuNhap (
    MaPhieuNhap INT NOT NULL,
    MaSP        INT NOT NULL,
    SoLuong     INT NOT NULL CHECK (SoLuong > 0),
    GiaNhap     DECIMAL(18,2) NOT NULL CHECK (GiaNhap >= 0),
    ThanhTien   AS (SoLuong * GiaNhap) PERSISTED,
    PRIMARY KEY (MaPhieuNhap, MaSP),
    CONSTRAINT FK_CTPN_PhieuNhap FOREIGN KEY (MaPhieuNhap) REFERENCES PhieuNhapHang(MaPhieuNhap),
    CONSTRAINT FK_CTPN_SanPham   FOREIGN KEY (MaSP)        REFERENCES SanPham(MaSP)
);
GO

/* =====================================================================
   6. QLKH - QUẢN LÝ KHO HÀNG (Kiểm kê & Điều chỉnh tồn kho)
   ===================================================================== */
CREATE TABLE LichSuDieuChinhTonKho (
    MaDieuChinh     INT IDENTITY(1,1) PRIMARY KEY,
    MaSP            INT NOT NULL,
    MaNhanVienKho   INT NOT NULL,
    NgayDieuChinh   DATETIME NOT NULL DEFAULT GETDATE(),
    SoLuongHeThong  INT NOT NULL,
    SoLuongThucTe   INT NOT NULL,
    ChenhLech       AS (SoLuongThucTe - SoLuongHeThong) PERSISTED,
    LyDo            NVARCHAR(500) NULL,
    CONSTRAINT FK_LSDC_SanPham  FOREIGN KEY (MaSP)          REFERENCES SanPham(MaSP),
    CONSTRAINT FK_LSDC_NhanVien FOREIGN KEY (MaNhanVienKho) REFERENCES TaiKhoan(MaTK)
);
GO

/* =====================================================================
   7. QLKM - QUẢN LÝ KHUYẾN MÃI
   ===================================================================== */
CREATE TABLE KhuyenMai (
    MaKhuyenMai     INT IDENTITY(1,1) PRIMARY KEY,
    TenKhuyenMai    NVARCHAR(150) NOT NULL,
    MoTa            NVARCHAR(500) NULL,
    LoaiGiam        NVARCHAR(20) NOT NULL CHECK (LoaiGiam IN (N'PhanTram', N'SoTien')),
    GiaTriGiam      DECIMAL(18,2) NOT NULL CHECK (GiaTriGiam >= 0),
    NgayBatDau      DATETIME NOT NULL,
    NgayKetThuc     DATETIME NOT NULL,
    DieuKienApDung  NVARCHAR(255) NULL,     -- vd: đơn hàng tối thiểu 500.000đ
    TrangThai       BIT NOT NULL DEFAULT 1,
    CHECK (NgayKetThuc >= NgayBatDau)
);
GO

/* =====================================================================
   8. QLBH - QUẢN LÝ BÁN HÀNG  (theo biểu mẫu QLBH_BM1: HÓA ĐƠN)
   Giá tiền = Tổng tiền hóa đơn - Giảm giá (mục 2.1 - Bộ phận QLBH)
   ===================================================================== */
CREATE TABLE HoaDon (
    MaHoaDon            INT IDENTITY(1,1) PRIMARY KEY,
    MaNhanVien          INT NULL,       -- nhân viên bán hàng lập hóa đơn
    MaKhachHang         INT NULL,           -- FK -> TaiKhoan (vai trò Khách hàng); NULL nếu khách vãng lai tại quầy
    MaKhuyenMai         INT NULL,
    NgayTao             DATETIME NOT NULL DEFAULT GETDATE(),
    HinhThucThanhToan   NVARCHAR(20) NOT NULL CHECK (HinhThucThanhToan IN (N'TienMat', N'ChuyenKhoan')),
    TongTienHang        DECIMAL(18,2) NOT NULL DEFAULT 0,
    TienGiamGia         DECIMAL(18,2) NOT NULL DEFAULT 0,
    TongThanhToan       AS (TongTienHang - TienGiamGia) PERSISTED,
    TrangThai           NVARCHAR(20) NOT NULL DEFAULT N'DaThanhToan',
    CONSTRAINT FK_HoaDon_NhanVien   FOREIGN KEY (MaNhanVien)   REFERENCES TaiKhoan(MaTK),
    CONSTRAINT FK_HoaDon_KhachHang  FOREIGN KEY (MaKhachHang)  REFERENCES TaiKhoan(MaTK),
    CONSTRAINT FK_HoaDon_KhuyenMai  FOREIGN KEY (MaKhuyenMai)  REFERENCES KhuyenMai(MaKhuyenMai)
);
GO

CREATE TABLE ChiTietHoaDon (
    MaHoaDon    INT NOT NULL,
    MaSP        INT NOT NULL,
    SoLuong     INT NOT NULL CHECK (SoLuong > 0),
    DonGia      DECIMAL(18,2) NOT NULL CHECK (DonGia >= 0),
    ThanhTien   AS (SoLuong * DonGia) PERSISTED,
    PRIMARY KEY (MaHoaDon, MaSP),
    CONSTRAINT FK_CTHD_HoaDon FOREIGN KEY (MaHoaDon) REFERENCES HoaDon(MaHoaDon),
    CONSTRAINT FK_CTHD_SanPham FOREIGN KEY (MaSP)    REFERENCES SanPham(MaSP)
);
GO

/* =====================================================================
   CHỈ MỤC HỖ TRỢ TÌM KIẾM / TRA CỨU
   (QLTK-4, QLSP-4, QLNCC-4: tìm theo mã, tên, danh mục, NCC...)
   ===================================================================== */
CREATE INDEX IX_TaiKhoan_HoTen      ON TaiKhoan(HoTen);
CREATE INDEX IX_SanPham_TenSP       ON SanPham(TenSP);
CREATE INDEX IX_SanPham_DanhMuc     ON SanPham(MaDanhMuc);
CREATE INDEX IX_NhaCungCap_TenNCC   ON NhaCungCap(TenNCC);
CREATE INDEX IX_NCCSP_SP            ON NhaCungCap_SanPham(MaSP);
CREATE INDEX IX_HoaDon_NgayTao      ON HoaDon(NgayTao);
CREATE INDEX IX_PhieuNhap_NgayNhap  ON PhieuNhapHang(NgayNhap);
GO

/* =====================================================================
   DỮ LIỆU MẶC ĐỊNH: VAI TRÒ (mục 3.1 - Nhận diện tác nhân)
   ===================================================================== */
INSERT INTO VaiTro (TenVaiTro) VALUES
    (N'Quản trị viên'),
    (N'Quản lý cửa hàng'),
    (N'Nhân viên bán hàng'),
    (N'Nhân viên kho'),
    (N'Khách hàng');
GO

PRINT N'Đã tạo xong CSDL CNPM cho hệ thống Quản lý cửa hàng gia dụng.';
GO


-- Giỏ hàng: mỗi khách hàng một giỏ riêng; mỗi sản phẩm một dòng trong giỏ của khách đó.
-- Chạy file này MỘT LẦN trên CSDL CNPM đã có (không xóa dữ liệu hiện tại).
IF OBJECT_ID(N'dbo.GioHang', N'U') IS NULL
BEGIN
    CREATE TABLE GioHang (
        MaKhachHang INT      NOT NULL,   -- FK -> TaiKhoan (vai trò Khách hàng)
        MaSP        INT      NOT NULL,
        SoLuong     INT      NOT NULL CHECK (SoLuong > 0),
        NgayThem    DATETIME NOT NULL DEFAULT GETDATE(),
        PRIMARY KEY (MaKhachHang, MaSP),
        CONSTRAINT FK_GioHang_KhachHang FOREIGN KEY (MaKhachHang) REFERENCES TaiKhoan(MaTK),
        CONSTRAINT FK_GioHang_SanPham   FOREIGN KEY (MaSP)        REFERENCES SanPham(MaSP)
    );
END
GO
 
-- Kiểm tra sau khi khách thanh toán (MaTK = 4):
--   SELECT h.MaHoaDon, h.MaNhanVien, h.MaKhachHang, h.MaKhuyenMai, h.HinhThucThanhToan,
--          h.TongTienHang, h.TienGiamGia, h.TongThanhToan, h.TrangThai FROM HoaDon h WHERE h.MaKhachHang = 4;
--   SELECT * FROM ChiTietHoaDon WHERE MaHoaDon = (SELECT MAX(MaHoaDon) FROM HoaDon);
--   SELECT MaSP, SoLuongTon FROM SanPham;
--   SELECT * FROM GioHang WHERE MaKhachHang = 4;   -- phải trống