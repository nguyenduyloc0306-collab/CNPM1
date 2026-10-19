namespace CNPM1
{
    partial class frmLichSuMuaHang
    {
        /// <summary>
        /// Required designer variable.
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary>
        /// Clean up any resources being used.
        /// </summary>
        /// <param name="disposing">true if managed resources should be disposed; otherwise, false.</param>
        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        #region Windows Form Designer generated code

        /// <summary>
        /// Required method for Designer support - do not modify
        /// the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
			System.Windows.Forms.DataGridViewCellStyle dataGridViewCellStyle1 = new System.Windows.Forms.DataGridViewCellStyle();
			System.Windows.Forms.DataGridViewCellStyle dataGridViewCellStyle2 = new System.Windows.Forms.DataGridViewCellStyle();
			System.Windows.Forms.DataGridViewCellStyle dataGridViewCellStyle3 = new System.Windows.Forms.DataGridViewCellStyle();
			System.Windows.Forms.DataGridViewCellStyle dataGridViewCellStyle4 = new System.Windows.Forms.DataGridViewCellStyle();
			this.lblTieuDe = new System.Windows.Forms.Label();
			this.lblDanhSach = new System.Windows.Forms.Label();
			this.dgvHoaDon = new System.Windows.Forms.DataGridView();
			this.colMaHD = new System.Windows.Forms.DataGridViewTextBoxColumn();
			this.colNgay = new System.Windows.Forms.DataGridViewTextBoxColumn();
			this.colHinhThuc = new System.Windows.Forms.DataGridViewTextBoxColumn();
			this.colTongTien = new System.Windows.Forms.DataGridViewTextBoxColumn();
			this.colGiam = new System.Windows.Forms.DataGridViewTextBoxColumn();
			this.colThanhToan = new System.Windows.Forms.DataGridViewTextBoxColumn();
			this.colKhuyenMai = new System.Windows.Forms.DataGridViewTextBoxColumn();
			this.colTrangThai = new System.Windows.Forms.DataGridViewTextBoxColumn();
			this.lblChiTiet = new System.Windows.Forms.Label();
			this.dgvChiTiet = new System.Windows.Forms.DataGridView();
			this.colCTTenSP = new System.Windows.Forms.DataGridViewTextBoxColumn();
			this.colCTDonGia = new System.Windows.Forms.DataGridViewTextBoxColumn();
			this.colCTSoLuong = new System.Windows.Forms.DataGridViewTextBoxColumn();
			this.colCTThanhTien = new System.Windows.Forms.DataGridViewTextBoxColumn();
			this.lblTongKet = new System.Windows.Forms.Label();
			this.btnDong = new System.Windows.Forms.Button();
			((System.ComponentModel.ISupportInitialize)(this.dgvHoaDon)).BeginInit();
			((System.ComponentModel.ISupportInitialize)(this.dgvChiTiet)).BeginInit();
			this.SuspendLayout();
			// 
			// lblTieuDe
			// 
			this.lblTieuDe.AutoSize = true;
			this.lblTieuDe.Font = new System.Drawing.Font("Segoe UI", 20.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(163)));
			this.lblTieuDe.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(15)))), ((int)(((byte)(23)))), ((int)(((byte)(42)))));
			this.lblTieuDe.Location = new System.Drawing.Point(17, 9);
			this.lblTieuDe.Name = "lblTieuDe";
			this.lblTieuDe.Size = new System.Drawing.Size(239, 37);
			this.lblTieuDe.TabIndex = 0;
			this.lblTieuDe.Text = "Lịch sử mua hàng";
			// 
			// lblDanhSach
			// 
			this.lblDanhSach.AutoSize = true;
			this.lblDanhSach.Font = new System.Drawing.Font("Segoe UI", 12F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(163)));
			this.lblDanhSach.Location = new System.Drawing.Point(20, 62);
			this.lblDanhSach.Name = "lblDanhSach";
			this.lblDanhSach.Size = new System.Drawing.Size(156, 21);
			this.lblDanhSach.TabIndex = 1;
			this.lblDanhSach.Text = "Danh sách hóa đơn";
			// 
			// dgvHoaDon
			// 
			this.dgvHoaDon.AllowUserToAddRows = false;
			this.dgvHoaDon.AllowUserToDeleteRows = false;
			this.dgvHoaDon.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
			this.dgvHoaDon.AutoSizeColumnsMode = System.Windows.Forms.DataGridViewAutoSizeColumnsMode.Fill;
			this.dgvHoaDon.BackgroundColor = System.Drawing.Color.White;
			dataGridViewCellStyle1.Alignment = System.Windows.Forms.DataGridViewContentAlignment.MiddleLeft;
			dataGridViewCellStyle1.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(241)))), ((int)(((byte)(245)))), ((int)(((byte)(249)))));
			dataGridViewCellStyle1.Font = new System.Drawing.Font("Segoe UI", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(163)));
			dataGridViewCellStyle1.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(30)))), ((int)(((byte)(41)))), ((int)(((byte)(59)))));
			dataGridViewCellStyle1.SelectionBackColor = System.Drawing.Color.FromArgb(((int)(((byte)(241)))), ((int)(((byte)(245)))), ((int)(((byte)(249)))));
			dataGridViewCellStyle1.SelectionForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(30)))), ((int)(((byte)(41)))), ((int)(((byte)(59)))));
			dataGridViewCellStyle1.WrapMode = System.Windows.Forms.DataGridViewTriState.True;
			this.dgvHoaDon.ColumnHeadersDefaultCellStyle = dataGridViewCellStyle1;
			this.dgvHoaDon.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
			this.dgvHoaDon.Columns.AddRange(new System.Windows.Forms.DataGridViewColumn[] {
            this.colMaHD,
            this.colNgay,
            this.colHinhThuc,
            this.colTongTien,
            this.colGiam,
            this.colThanhToan,
            this.colKhuyenMai,
            this.colTrangThai});
			dataGridViewCellStyle2.Alignment = System.Windows.Forms.DataGridViewContentAlignment.MiddleLeft;
			dataGridViewCellStyle2.BackColor = System.Drawing.SystemColors.Window;
			dataGridViewCellStyle2.Font = new System.Drawing.Font("Segoe UI", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(163)));
			dataGridViewCellStyle2.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(30)))), ((int)(((byte)(41)))), ((int)(((byte)(59)))));
			dataGridViewCellStyle2.SelectionBackColor = System.Drawing.Color.FromArgb(((int)(((byte)(224)))), ((int)(((byte)(231)))), ((int)(((byte)(255)))));
			dataGridViewCellStyle2.SelectionForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(30)))), ((int)(((byte)(41)))), ((int)(((byte)(59)))));
			dataGridViewCellStyle2.WrapMode = System.Windows.Forms.DataGridViewTriState.False;
			this.dgvHoaDon.DefaultCellStyle = dataGridViewCellStyle2;
			this.dgvHoaDon.GridColor = System.Drawing.Color.White;
			this.dgvHoaDon.Location = new System.Drawing.Point(20, 88);
			this.dgvHoaDon.MultiSelect = false;
			this.dgvHoaDon.Name = "dgvHoaDon";
			this.dgvHoaDon.ReadOnly = true;
			this.dgvHoaDon.SelectionMode = System.Windows.Forms.DataGridViewSelectionMode.FullRowSelect;
			this.dgvHoaDon.Size = new System.Drawing.Size(960, 210);
			this.dgvHoaDon.TabIndex = 2;
			this.dgvHoaDon.SelectionChanged += new System.EventHandler(this.dgvHoaDon_SelectionChanged);
			// 
			// colMaHD
			// 
			this.colMaHD.DataPropertyName = "MaHoaDon";
			this.colMaHD.HeaderText = "Mã hóa đơn";
			this.colMaHD.Name = "colMaHD";
			this.colMaHD.ReadOnly = true;
			// 
			// colNgay
			// 
			this.colNgay.DataPropertyName = "NgayTao";
			this.colNgay.HeaderText = "Ngày mua";
			this.colNgay.Name = "colNgay";
			this.colNgay.ReadOnly = true;
			// 
			// colHinhThuc
			// 
			this.colHinhThuc.DataPropertyName = "HinhThuc";
			this.colHinhThuc.HeaderText = "Hình thức";
			this.colHinhThuc.Name = "colHinhThuc";
			this.colHinhThuc.ReadOnly = true;
			// 
			// colTongTien
			// 
			this.colTongTien.DataPropertyName = "TongTienHang";
			this.colTongTien.HeaderText = "Tổng tiền hàng";
			this.colTongTien.Name = "colTongTien";
			this.colTongTien.ReadOnly = true;
			// 
			// colGiam
			// 
			this.colGiam.DataPropertyName = "TienGiamGia";
			this.colGiam.HeaderText = "Giảm giá";
			this.colGiam.Name = "colGiam";
			this.colGiam.ReadOnly = true;
			// 
			// colThanhToan
			// 
			this.colThanhToan.DataPropertyName = "TongThanhToan";
			this.colThanhToan.HeaderText = "Đã thanh toán";
			this.colThanhToan.Name = "colThanhToan";
			this.colThanhToan.ReadOnly = true;
			// 
			// colKhuyenMai
			// 
			this.colKhuyenMai.DataPropertyName = "TenKhuyenMai";
			this.colKhuyenMai.HeaderText = "Khuyến mãi";
			this.colKhuyenMai.Name = "colKhuyenMai";
			this.colKhuyenMai.ReadOnly = true;
			// 
			// colTrangThai
			// 
			this.colTrangThai.DataPropertyName = "TrangThai";
			this.colTrangThai.HeaderText = "Trạng thái";
			this.colTrangThai.Name = "colTrangThai";
			this.colTrangThai.ReadOnly = true;
			// 
			// lblChiTiet
			// 
			this.lblChiTiet.AutoSize = true;
			this.lblChiTiet.Font = new System.Drawing.Font("Segoe UI", 12F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(163)));
			this.lblChiTiet.Location = new System.Drawing.Point(20, 312);
			this.lblChiTiet.Name = "lblChiTiet";
			this.lblChiTiet.Size = new System.Drawing.Size(217, 21);
			this.lblChiTiet.TabIndex = 3;
			this.lblChiTiet.Text = "Chi tiết hóa đơn đang chọn";
			// 
			// dgvChiTiet
			// 
			this.dgvChiTiet.AllowUserToAddRows = false;
			this.dgvChiTiet.AllowUserToDeleteRows = false;
			this.dgvChiTiet.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
			this.dgvChiTiet.AutoSizeColumnsMode = System.Windows.Forms.DataGridViewAutoSizeColumnsMode.Fill;
			this.dgvChiTiet.BackgroundColor = System.Drawing.Color.White;
			dataGridViewCellStyle3.Alignment = System.Windows.Forms.DataGridViewContentAlignment.MiddleLeft;
			dataGridViewCellStyle3.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(241)))), ((int)(((byte)(245)))), ((int)(((byte)(249)))));
			dataGridViewCellStyle3.Font = new System.Drawing.Font("Segoe UI", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(163)));
			dataGridViewCellStyle3.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(30)))), ((int)(((byte)(41)))), ((int)(((byte)(59)))));
			dataGridViewCellStyle3.SelectionBackColor = System.Drawing.Color.FromArgb(((int)(((byte)(241)))), ((int)(((byte)(245)))), ((int)(((byte)(249)))));
			dataGridViewCellStyle3.SelectionForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(30)))), ((int)(((byte)(41)))), ((int)(((byte)(59)))));
			dataGridViewCellStyle3.WrapMode = System.Windows.Forms.DataGridViewTriState.True;
			this.dgvChiTiet.ColumnHeadersDefaultCellStyle = dataGridViewCellStyle3;
			this.dgvChiTiet.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
			this.dgvChiTiet.Columns.AddRange(new System.Windows.Forms.DataGridViewColumn[] {
            this.colCTTenSP,
            this.colCTDonGia,
            this.colCTSoLuong,
            this.colCTThanhTien});
			dataGridViewCellStyle4.Alignment = System.Windows.Forms.DataGridViewContentAlignment.MiddleLeft;
			dataGridViewCellStyle4.BackColor = System.Drawing.SystemColors.Window;
			dataGridViewCellStyle4.Font = new System.Drawing.Font("Segoe UI", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(163)));
			dataGridViewCellStyle4.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(30)))), ((int)(((byte)(41)))), ((int)(((byte)(59)))));
			dataGridViewCellStyle4.SelectionBackColor = System.Drawing.Color.FromArgb(((int)(((byte)(224)))), ((int)(((byte)(231)))), ((int)(((byte)(255)))));
			dataGridViewCellStyle4.SelectionForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(30)))), ((int)(((byte)(41)))), ((int)(((byte)(59)))));
			dataGridViewCellStyle4.WrapMode = System.Windows.Forms.DataGridViewTriState.False;
			this.dgvChiTiet.DefaultCellStyle = dataGridViewCellStyle4;
			this.dgvChiTiet.GridColor = System.Drawing.Color.White;
			this.dgvChiTiet.Location = new System.Drawing.Point(20, 338);
			this.dgvChiTiet.MultiSelect = false;
			this.dgvChiTiet.Name = "dgvChiTiet";
			this.dgvChiTiet.ReadOnly = true;
			this.dgvChiTiet.SelectionMode = System.Windows.Forms.DataGridViewSelectionMode.FullRowSelect;
			this.dgvChiTiet.Size = new System.Drawing.Size(960, 190);
			this.dgvChiTiet.TabIndex = 4;
			// 
			// colCTTenSP
			// 
			this.colCTTenSP.DataPropertyName = "TenSP";
			this.colCTTenSP.HeaderText = "Tên sản phẩm";
			this.colCTTenSP.Name = "colCTTenSP";
			this.colCTTenSP.ReadOnly = true;
			// 
			// colCTDonGia
			// 
			this.colCTDonGia.DataPropertyName = "DonGia";
			this.colCTDonGia.HeaderText = "Đơn giá";
			this.colCTDonGia.Name = "colCTDonGia";
			this.colCTDonGia.ReadOnly = true;
			// 
			// colCTSoLuong
			// 
			this.colCTSoLuong.DataPropertyName = "SoLuong";
			this.colCTSoLuong.HeaderText = "Số lượng";
			this.colCTSoLuong.Name = "colCTSoLuong";
			this.colCTSoLuong.ReadOnly = true;
			// 
			// colCTThanhTien
			// 
			this.colCTThanhTien.DataPropertyName = "ThanhTien";
			this.colCTThanhTien.HeaderText = "Thành tiền";
			this.colCTThanhTien.Name = "colCTThanhTien";
			this.colCTThanhTien.ReadOnly = true;
			// 
			// lblTongKet
			// 
			this.lblTongKet.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)));
			this.lblTongKet.AutoSize = true;
			this.lblTongKet.Font = new System.Drawing.Font("Segoe UI", 12F);
			this.lblTongKet.Location = new System.Drawing.Point(20, 556);
			this.lblTongKet.Name = "lblTongKet";
			this.lblTongKet.Size = new System.Drawing.Size(106, 21);
			this.lblTongKet.TabIndex = 5;
			this.lblTongKet.Text = "Số hóa đơn: 0";
			// 
			// btnDong
			// 
			this.btnDong.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
			this.btnDong.BackColor = System.Drawing.Color.WhiteSmoke;
			this.btnDong.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
			this.btnDong.Font = new System.Drawing.Font("Segoe UI", 12.75F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(163)));
			this.btnDong.Location = new System.Drawing.Point(838, 549);
			this.btnDong.Name = "btnDong";
			this.btnDong.Size = new System.Drawing.Size(142, 34);
			this.btnDong.TabIndex = 6;
			this.btnDong.Text = "Đóng";
			this.btnDong.UseVisualStyleBackColor = false;
			this.btnDong.Click += new System.EventHandler(this.btnDong_Click);
			// 
			// frmLichSuMuaHang
			// 
			this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
			this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
			this.BackColor = System.Drawing.Color.LightSteelBlue;
			this.ClientSize = new System.Drawing.Size(1000, 610);
			this.Controls.Add(this.btnDong);
			this.Controls.Add(this.lblTongKet);
			this.Controls.Add(this.dgvChiTiet);
			this.Controls.Add(this.lblChiTiet);
			this.Controls.Add(this.dgvHoaDon);
			this.Controls.Add(this.lblDanhSach);
			this.Controls.Add(this.lblTieuDe);
			this.Name = "frmLichSuMuaHang";
			this.StartPosition = System.Windows.Forms.FormStartPosition.CenterParent;
			this.Text = "Lịch sử mua hàng";
			this.Load += new System.EventHandler(this.frmLichSuMuaHang_Load);
			((System.ComponentModel.ISupportInitialize)(this.dgvHoaDon)).EndInit();
			((System.ComponentModel.ISupportInitialize)(this.dgvChiTiet)).EndInit();
			this.ResumeLayout(false);
			this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.Label lblTieuDe;
        private System.Windows.Forms.Label lblDanhSach;
        private System.Windows.Forms.DataGridView dgvHoaDon;
        private System.Windows.Forms.Label lblChiTiet;
        private System.Windows.Forms.DataGridView dgvChiTiet;
        private System.Windows.Forms.DataGridViewTextBoxColumn colMaHD;
        private System.Windows.Forms.DataGridViewTextBoxColumn colNgay;
        private System.Windows.Forms.DataGridViewTextBoxColumn colHinhThuc;
        private System.Windows.Forms.DataGridViewTextBoxColumn colTongTien;
        private System.Windows.Forms.DataGridViewTextBoxColumn colGiam;
        private System.Windows.Forms.DataGridViewTextBoxColumn colThanhToan;
        private System.Windows.Forms.DataGridViewTextBoxColumn colKhuyenMai;
        private System.Windows.Forms.DataGridViewTextBoxColumn colTrangThai;
        private System.Windows.Forms.DataGridViewTextBoxColumn colCTTenSP;
        private System.Windows.Forms.DataGridViewTextBoxColumn colCTDonGia;
        private System.Windows.Forms.DataGridViewTextBoxColumn colCTSoLuong;
        private System.Windows.Forms.DataGridViewTextBoxColumn colCTThanhTien;
        private System.Windows.Forms.Label lblTongKet;
        private System.Windows.Forms.Button btnDong;
    }
}