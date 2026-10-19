namespace CNPM1
{
    partial class frmGioHang
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
			this.lblTieuDe = new System.Windows.Forms.Label();
			this.dgvGioHang = new System.Windows.Forms.DataGridView();
			this.colMaSP = new System.Windows.Forms.DataGridViewTextBoxColumn();
			this.colTenSP = new System.Windows.Forms.DataGridViewTextBoxColumn();
			this.colGiaBan = new System.Windows.Forms.DataGridViewTextBoxColumn();
			this.colSoLuong = new System.Windows.Forms.DataGridViewTextBoxColumn();
			this.colThanhTien = new System.Windows.Forms.DataGridViewTextBoxColumn();
			this.panelDuoi = new System.Windows.Forms.Panel();
			this.lblTongTien = new System.Windows.Forms.Label();
			this.lblGiamGia = new System.Windows.Forms.Label();
			this.lblThanhToan = new System.Windows.Forms.Label();
			this.lblHinhThuc = new System.Windows.Forms.Label();
			this.cboHinhThuc = new System.Windows.Forms.ComboBox();
			this.btnXoa = new System.Windows.Forms.Button();
			this.btnThanhToan = new System.Windows.Forms.Button();
			this.btnDong = new System.Windows.Forms.Button();
			((System.ComponentModel.ISupportInitialize)(this.dgvGioHang)).BeginInit();
			this.panelDuoi.SuspendLayout();
			this.SuspendLayout();
			// 
			// lblTieuDe
			// 
			this.lblTieuDe.AutoSize = true;
			this.lblTieuDe.Font = new System.Drawing.Font("Segoe UI", 20F, System.Drawing.FontStyle.Bold);
			this.lblTieuDe.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(15)))), ((int)(((byte)(23)))), ((int)(((byte)(41)))));
			this.lblTieuDe.Location = new System.Drawing.Point(20, 15);
			this.lblTieuDe.Name = "lblTieuDe";
			this.lblTieuDe.Size = new System.Drawing.Size(238, 37);
			this.lblTieuDe.TabIndex = 0;
			this.lblTieuDe.Text = "Giỏ hàng của bạn";
			// 
			// dgvGioHang
			// 
			this.dgvGioHang.AllowUserToAddRows = false;
			this.dgvGioHang.AllowUserToDeleteRows = false;
			this.dgvGioHang.Anchor = System.Windows.Forms.AnchorStyles.None;
			this.dgvGioHang.AutoSizeColumnsMode = System.Windows.Forms.DataGridViewAutoSizeColumnsMode.Fill;
			this.dgvGioHang.BackgroundColor = System.Drawing.Color.White;
			this.dgvGioHang.CellBorderStyle = System.Windows.Forms.DataGridViewCellBorderStyle.SingleHorizontal;
			this.dgvGioHang.ColumnHeadersBorderStyle = System.Windows.Forms.DataGridViewHeaderBorderStyle.None;
			dataGridViewCellStyle1.Alignment = System.Windows.Forms.DataGridViewContentAlignment.MiddleLeft;
			dataGridViewCellStyle1.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(241)))), ((int)(((byte)(245)))), ((int)(((byte)(249)))));
			dataGridViewCellStyle1.Font = new System.Drawing.Font("Segoe UI", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(163)));
			dataGridViewCellStyle1.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(30)))), ((int)(((byte)(41)))), ((int)(((byte)(59)))));
			dataGridViewCellStyle1.SelectionBackColor = System.Drawing.Color.FromArgb(((int)(((byte)(241)))), ((int)(((byte)(245)))), ((int)(((byte)(249)))));
			dataGridViewCellStyle1.SelectionForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(30)))), ((int)(((byte)(41)))), ((int)(((byte)(59)))));
			dataGridViewCellStyle1.WrapMode = System.Windows.Forms.DataGridViewTriState.True;
			this.dgvGioHang.ColumnHeadersDefaultCellStyle = dataGridViewCellStyle1;
			this.dgvGioHang.ColumnHeadersHeight = 38;
			this.dgvGioHang.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.DisableResizing;
			this.dgvGioHang.Columns.AddRange(new System.Windows.Forms.DataGridViewColumn[] {
            this.colMaSP,
            this.colTenSP,
            this.colGiaBan,
            this.colSoLuong,
            this.colThanhTien});
			dataGridViewCellStyle2.Alignment = System.Windows.Forms.DataGridViewContentAlignment.MiddleLeft;
			dataGridViewCellStyle2.BackColor = System.Drawing.SystemColors.Window;
			dataGridViewCellStyle2.Font = new System.Drawing.Font("Segoe UI", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(163)));
			dataGridViewCellStyle2.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(30)))), ((int)(((byte)(41)))), ((int)(((byte)(59)))));
			dataGridViewCellStyle2.SelectionBackColor = System.Drawing.Color.FromArgb(((int)(((byte)(224)))), ((int)(((byte)(231)))), ((int)(((byte)(255)))));
			dataGridViewCellStyle2.SelectionForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(30)))), ((int)(((byte)(41)))), ((int)(((byte)(59)))));
			dataGridViewCellStyle2.WrapMode = System.Windows.Forms.DataGridViewTriState.False;
			this.dgvGioHang.DefaultCellStyle = dataGridViewCellStyle2;
			this.dgvGioHang.EnableHeadersVisualStyles = false;
			this.dgvGioHang.GridColor = System.Drawing.Color.FromArgb(((int)(((byte)(241)))), ((int)(((byte)(245)))), ((int)(((byte)(249)))));
			this.dgvGioHang.Location = new System.Drawing.Point(20, 65);
			this.dgvGioHang.Name = "dgvGioHang";
			dataGridViewCellStyle3.Alignment = System.Windows.Forms.DataGridViewContentAlignment.MiddleLeft;
			dataGridViewCellStyle3.BackColor = System.Drawing.SystemColors.Control;
			dataGridViewCellStyle3.Font = new System.Drawing.Font("Segoe UI", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(163)));
			dataGridViewCellStyle3.ForeColor = System.Drawing.SystemColors.WindowText;
			dataGridViewCellStyle3.SelectionBackColor = System.Drawing.SystemColors.Highlight;
			dataGridViewCellStyle3.SelectionForeColor = System.Drawing.SystemColors.HighlightText;
			dataGridViewCellStyle3.WrapMode = System.Windows.Forms.DataGridViewTriState.True;
			this.dgvGioHang.RowHeadersDefaultCellStyle = dataGridViewCellStyle3;
			this.dgvGioHang.RowHeadersVisible = false;
			this.dgvGioHang.SelectionMode = System.Windows.Forms.DataGridViewSelectionMode.FullRowSelect;
			this.dgvGioHang.Size = new System.Drawing.Size(860, 395);
			this.dgvGioHang.TabIndex = 1;
			this.dgvGioHang.CellEndEdit += new System.Windows.Forms.DataGridViewCellEventHandler(this.dgvGioHang_CellEndEdit);
			this.dgvGioHang.CellValidating += new System.Windows.Forms.DataGridViewCellValidatingEventHandler(this.dgvGioHang_CellValidating);
			// 
			// colMaSP
			// 
			this.colMaSP.DataPropertyName = "MaSP";
			this.colMaSP.HeaderText = "Mã SP";
			this.colMaSP.Name = "colMaSP";
			this.colMaSP.Visible = false;
			// 
			// colTenSP
			// 
			this.colTenSP.DataPropertyName = "TenSP";
			this.colTenSP.HeaderText = "Tên sản phẩm";
			this.colTenSP.Name = "colTenSP";
			// 
			// colGiaBan
			// 
			this.colGiaBan.DataPropertyName = "GiaBan";
			this.colGiaBan.HeaderText = "Đơn giá";
			this.colGiaBan.Name = "colGiaBan";
			// 
			// colSoLuong
			// 
			this.colSoLuong.DataPropertyName = "SoLuong";
			this.colSoLuong.HeaderText = "Số lượng";
			this.colSoLuong.Name = "colSoLuong";
			// 
			// colThanhTien
			// 
			this.colThanhTien.DataPropertyName = "ThanhTien";
			this.colThanhTien.HeaderText = "Thành tiền";
			this.colThanhTien.Name = "colThanhTien";
			// 
			// panelDuoi
			// 
			this.panelDuoi.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
			this.panelDuoi.Controls.Add(this.lblTongTien);
			this.panelDuoi.Controls.Add(this.lblGiamGia);
			this.panelDuoi.Controls.Add(this.lblThanhToan);
			this.panelDuoi.Controls.Add(this.lblHinhThuc);
			this.panelDuoi.Controls.Add(this.cboHinhThuc);
			this.panelDuoi.Controls.Add(this.btnXoa);
			this.panelDuoi.Controls.Add(this.btnThanhToan);
			this.panelDuoi.Controls.Add(this.btnDong);
			this.panelDuoi.Location = new System.Drawing.Point(20, 475);
			this.panelDuoi.Name = "panelDuoi";
			this.panelDuoi.Size = new System.Drawing.Size(860, 130);
			this.panelDuoi.TabIndex = 2;
			// 
			// lblTongTien
			// 
			this.lblTongTien.AutoSize = true;
			this.lblTongTien.Font = new System.Drawing.Font("Segoe UI", 14.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
			this.lblTongTien.ForeColor = System.Drawing.Color.Black;
			this.lblTongTien.Location = new System.Drawing.Point(3, 13);
			this.lblTongTien.Name = "lblTongTien";
			this.lblTongTien.Size = new System.Drawing.Size(175, 25);
			this.lblTongTien.TabIndex = 0;
			this.lblTongTien.Text = "Tổng tiền hàng: 0 đ";
			// 
			// lblGiamGia
			// 
			this.lblGiamGia.AutoSize = true;
			this.lblGiamGia.Font = new System.Drawing.Font("Segoe UI", 14.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
			this.lblGiamGia.ForeColor = System.Drawing.Color.Black;
			this.lblGiamGia.Location = new System.Drawing.Point(3, 46);
			this.lblGiamGia.Name = "lblGiamGia";
			this.lblGiamGia.Size = new System.Drawing.Size(122, 25);
			this.lblGiamGia.TabIndex = 1;
			this.lblGiamGia.Text = "Giảm giá: 0 đ";
			// 
			// lblThanhToan
			// 
			this.lblThanhToan.AutoSize = true;
			this.lblThanhToan.Font = new System.Drawing.Font("Segoe UI", 18F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(163)));
			this.lblThanhToan.ForeColor = System.Drawing.Color.Black;
			this.lblThanhToan.Location = new System.Drawing.Point(5, 78);
			this.lblThanhToan.Name = "lblThanhToan";
			this.lblThanhToan.Size = new System.Drawing.Size(152, 32);
			this.lblThanhToan.TabIndex = 2;
			this.lblThanhToan.Text = "Phải trả: 0 đ";
			// 
			// lblHinhThuc
			// 
			this.lblHinhThuc.AutoSize = true;
			this.lblHinhThuc.Font = new System.Drawing.Font("Segoe UI", 14.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
			this.lblHinhThuc.ForeColor = System.Drawing.Color.Black;
			this.lblHinhThuc.Location = new System.Drawing.Point(321, 20);
			this.lblHinhThuc.Name = "lblHinhThuc";
			this.lblHinhThuc.Size = new System.Drawing.Size(195, 25);
			this.lblHinhThuc.TabIndex = 3;
			this.lblHinhThuc.Text = "Hình thức thanh toán:";
			// 
			// cboHinhThuc
			// 
			this.cboHinhThuc.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
			this.cboHinhThuc.Font = new System.Drawing.Font("Segoe UI", 14.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(163)));
			this.cboHinhThuc.FormattingEnabled = true;
			this.cboHinhThuc.Items.AddRange(new object[] {
            "Tiền mặt",
            "Chuyển khoản"});
			this.cboHinhThuc.Location = new System.Drawing.Point(522, 18);
			this.cboHinhThuc.Name = "cboHinhThuc";
			this.cboHinhThuc.Size = new System.Drawing.Size(320, 33);
			this.cboHinhThuc.TabIndex = 4;
			// 
			// btnXoa
			// 
			this.btnXoa.Font = new System.Drawing.Font("Segoe UI", 14.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(163)));
			this.btnXoa.ForeColor = System.Drawing.Color.Black;
			this.btnXoa.Location = new System.Drawing.Point(326, 60);
			this.btnXoa.Name = "btnXoa";
			this.btnXoa.Size = new System.Drawing.Size(155, 50);
			this.btnXoa.TabIndex = 5;
			this.btnXoa.Text = "Xóa khỏi giỏ";
			this.btnXoa.UseVisualStyleBackColor = true;
			this.btnXoa.Click += new System.EventHandler(this.btnXoa_Click);
			// 
			// btnThanhToan
			// 
			this.btnThanhToan.Font = new System.Drawing.Font("Segoe UI", 14.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(163)));
			this.btnThanhToan.ForeColor = System.Drawing.Color.Black;
			this.btnThanhToan.Location = new System.Drawing.Point(491, 60);
			this.btnThanhToan.Name = "btnThanhToan";
			this.btnThanhToan.Size = new System.Drawing.Size(180, 50);
			this.btnThanhToan.TabIndex = 6;
			this.btnThanhToan.Text = "Thanh toán";
			this.btnThanhToan.UseVisualStyleBackColor = true;
			this.btnThanhToan.Click += new System.EventHandler(this.btnThanhToan_Click);
			// 
			// btnDong
			// 
			this.btnDong.Font = new System.Drawing.Font("Segoe UI", 14.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(163)));
			this.btnDong.ForeColor = System.Drawing.Color.Black;
			this.btnDong.Location = new System.Drawing.Point(681, 60);
			this.btnDong.Name = "btnDong";
			this.btnDong.Size = new System.Drawing.Size(160, 50);
			this.btnDong.TabIndex = 7;
			this.btnDong.Text = "Đóng";
			this.btnDong.UseVisualStyleBackColor = true;
			this.btnDong.Click += new System.EventHandler(this.btnDong_Click);
			// 
			// frmGioHang
			// 
			this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
			this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
			this.BackColor = System.Drawing.Color.LightSteelBlue;
			this.ClientSize = new System.Drawing.Size(900, 620);
			this.Controls.Add(this.panelDuoi);
			this.Controls.Add(this.dgvGioHang);
			this.Controls.Add(this.lblTieuDe);
			this.ForeColor = System.Drawing.Color.White;
			this.Name = "frmGioHang";
			this.StartPosition = System.Windows.Forms.FormStartPosition.CenterParent;
			this.Text = "Giỏ hàng";
			this.Load += new System.EventHandler(this.frmGioHang_Load);
			((System.ComponentModel.ISupportInitialize)(this.dgvGioHang)).EndInit();
			this.panelDuoi.ResumeLayout(false);
			this.panelDuoi.PerformLayout();
			this.ResumeLayout(false);
			this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.Label lblTieuDe;
        private System.Windows.Forms.DataGridView dgvGioHang;
        private System.Windows.Forms.DataGridViewTextBoxColumn colMaSP;
        private System.Windows.Forms.DataGridViewTextBoxColumn colTenSP;
        private System.Windows.Forms.DataGridViewTextBoxColumn colGiaBan;
        private System.Windows.Forms.DataGridViewTextBoxColumn colSoLuong;
        private System.Windows.Forms.DataGridViewTextBoxColumn colThanhTien;
        private System.Windows.Forms.Panel panelDuoi;
        private System.Windows.Forms.Label lblTongTien;
        private System.Windows.Forms.Label lblGiamGia;
        private System.Windows.Forms.Label lblThanhToan;
        private System.Windows.Forms.Label lblHinhThuc;
        private System.Windows.Forms.ComboBox cboHinhThuc;
        private System.Windows.Forms.Button btnXoa;
        private System.Windows.Forms.Button btnThanhToan;
        private System.Windows.Forms.Button btnDong;
    }
}