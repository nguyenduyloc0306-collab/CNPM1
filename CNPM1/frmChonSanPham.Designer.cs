namespace CNPM1
{
	partial class frmChonSanPham
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
			this.lblTim = new System.Windows.Forms.Label();
			this.txtTim = new System.Windows.Forms.TextBox();
			this.btnTim = new System.Windows.Forms.Button();
			this.dgvSanPham = new System.Windows.Forms.DataGridView();
			this.colMaSP = new System.Windows.Forms.DataGridViewTextBoxColumn();
			this.colTenSP = new System.Windows.Forms.DataGridViewTextBoxColumn();
			this.colDanhMuc = new System.Windows.Forms.DataGridViewTextBoxColumn();
			this.colGia = new System.Windows.Forms.DataGridViewTextBoxColumn();
			this.colGiaKM = new System.Windows.Forms.DataGridViewTextBoxColumn();
			this.colTon = new System.Windows.Forms.DataGridViewTextBoxColumn();
			this.lblSoLuong = new System.Windows.Forms.Label();
			this.nudSoLuong = new System.Windows.Forms.NumericUpDown();
			this.btnThem = new System.Windows.Forms.Button();
			this.lblThongBao = new System.Windows.Forms.Label();
			this.btnXong = new System.Windows.Forms.Button();
			((System.ComponentModel.ISupportInitialize)(this.dgvSanPham)).BeginInit();
			((System.ComponentModel.ISupportInitialize)(this.nudSoLuong)).BeginInit();
			this.SuspendLayout();
			// 
			// lblTim
			// 
			this.lblTim.AutoSize = true;
			this.lblTim.Font = new System.Drawing.Font("Segoe UI", 12F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(163)));
			this.lblTim.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(71)))), ((int)(((byte)(85)))), ((int)(((byte)(105)))));
			this.lblTim.Location = new System.Drawing.Point(15, 18);
			this.lblTim.Name = "lblTim";
			this.lblTim.Size = new System.Drawing.Size(131, 21);
			this.lblTim.TabIndex = 5;
			this.lblTim.Text = "Mã hoặc tên SP:";
			// 
			// txtTim
			// 
			this.txtTim.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
			this.txtTim.Font = new System.Drawing.Font("Segoe UI", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(163)));
			this.txtTim.Location = new System.Drawing.Point(150, 14);
			this.txtTim.Name = "txtTim";
			this.txtTim.Size = new System.Drawing.Size(300, 29);
			this.txtTim.TabIndex = 0;
			this.txtTim.KeyDown += new System.Windows.Forms.KeyEventHandler(this.txtTim_KeyDown);
			// 
			// btnTim
			// 
			this.btnTim.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(37)))), ((int)(((byte)(99)))), ((int)(((byte)(235)))));
			this.btnTim.FlatAppearance.BorderSize = 0;
			this.btnTim.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
			this.btnTim.Font = new System.Drawing.Font("Segoe UI", 12F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(163)));
			this.btnTim.ForeColor = System.Drawing.Color.White;
			this.btnTim.Location = new System.Drawing.Point(460, 11);
			this.btnTim.Name = "btnTim";
			this.btnTim.Size = new System.Drawing.Size(100, 32);
			this.btnTim.TabIndex = 1;
			this.btnTim.Text = "Tìm";
			this.btnTim.UseVisualStyleBackColor = false;
			this.btnTim.Click += new System.EventHandler(this.btnTim_Click);
			// 
			// dgvSanPham
			// 
			this.dgvSanPham.AllowUserToAddRows = false;
			this.dgvSanPham.AllowUserToDeleteRows = false;
			this.dgvSanPham.Anchor = System.Windows.Forms.AnchorStyles.None;
			this.dgvSanPham.AutoSizeColumnsMode = System.Windows.Forms.DataGridViewAutoSizeColumnsMode.Fill;
			this.dgvSanPham.BackgroundColor = System.Drawing.Color.White;
			this.dgvSanPham.CellBorderStyle = System.Windows.Forms.DataGridViewCellBorderStyle.SingleHorizontal;
			dataGridViewCellStyle1.Alignment = System.Windows.Forms.DataGridViewContentAlignment.MiddleLeft;
			dataGridViewCellStyle1.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(241)))), ((int)(((byte)(245)))), ((int)(((byte)(249)))));
			dataGridViewCellStyle1.Font = new System.Drawing.Font("Segoe UI", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(163)));
			dataGridViewCellStyle1.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(30)))), ((int)(((byte)(41)))), ((int)(((byte)(59)))));
			dataGridViewCellStyle1.SelectionBackColor = System.Drawing.Color.FromArgb(((int)(((byte)(241)))), ((int)(((byte)(245)))), ((int)(((byte)(249)))));
			dataGridViewCellStyle1.SelectionForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(30)))), ((int)(((byte)(41)))), ((int)(((byte)(59)))));
			dataGridViewCellStyle1.WrapMode = System.Windows.Forms.DataGridViewTriState.True;
			this.dgvSanPham.ColumnHeadersDefaultCellStyle = dataGridViewCellStyle1;
			this.dgvSanPham.ColumnHeadersHeight = 38;
			this.dgvSanPham.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.DisableResizing;
			this.dgvSanPham.Columns.AddRange(new System.Windows.Forms.DataGridViewColumn[] {
            this.colMaSP,
            this.colTenSP,
            this.colDanhMuc,
            this.colGia,
            this.colGiaKM,
            this.colTon});
			dataGridViewCellStyle2.Alignment = System.Windows.Forms.DataGridViewContentAlignment.MiddleLeft;
			dataGridViewCellStyle2.BackColor = System.Drawing.Color.White;
			dataGridViewCellStyle2.Font = new System.Drawing.Font("Segoe UI", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(163)));
			dataGridViewCellStyle2.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(30)))), ((int)(((byte)(41)))), ((int)(((byte)(59)))));
			dataGridViewCellStyle2.SelectionBackColor = System.Drawing.Color.FromArgb(((int)(((byte)(224)))), ((int)(((byte)(231)))), ((int)(((byte)(255)))));
			dataGridViewCellStyle2.SelectionForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(30)))), ((int)(((byte)(41)))), ((int)(((byte)(59)))));
			dataGridViewCellStyle2.WrapMode = System.Windows.Forms.DataGridViewTriState.False;
			this.dgvSanPham.DefaultCellStyle = dataGridViewCellStyle2;
			this.dgvSanPham.EnableHeadersVisualStyles = false;
			this.dgvSanPham.GridColor = System.Drawing.Color.FromArgb(((int)(((byte)(241)))), ((int)(((byte)(245)))), ((int)(((byte)(249)))));
			this.dgvSanPham.Location = new System.Drawing.Point(15, 55);
			this.dgvSanPham.MultiSelect = false;
			this.dgvSanPham.Name = "dgvSanPham";
			this.dgvSanPham.ReadOnly = true;
			dataGridViewCellStyle3.Alignment = System.Windows.Forms.DataGridViewContentAlignment.MiddleLeft;
			dataGridViewCellStyle3.BackColor = System.Drawing.SystemColors.Control;
			dataGridViewCellStyle3.Font = new System.Drawing.Font("Segoe UI", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
			dataGridViewCellStyle3.ForeColor = System.Drawing.SystemColors.WindowText;
			dataGridViewCellStyle3.SelectionBackColor = System.Drawing.SystemColors.Highlight;
			dataGridViewCellStyle3.SelectionForeColor = System.Drawing.SystemColors.HighlightText;
			dataGridViewCellStyle3.WrapMode = System.Windows.Forms.DataGridViewTriState.True;
			this.dgvSanPham.RowHeadersDefaultCellStyle = dataGridViewCellStyle3;
			this.dgvSanPham.SelectionMode = System.Windows.Forms.DataGridViewSelectionMode.FullRowSelect;
			this.dgvSanPham.Size = new System.Drawing.Size(790, 330);
			this.dgvSanPham.TabIndex = 2;
			this.dgvSanPham.CellDoubleClick += new System.Windows.Forms.DataGridViewCellEventHandler(this.dgvSanPham_CellDoubleClick);
			this.dgvSanPham.SelectionChanged += new System.EventHandler(this.dgvSanPham_SelectionChanged);
			// 
			// colMaSP
			// 
			this.colMaSP.DataPropertyName = "MaSP";
			this.colMaSP.HeaderText = "Mã SP";
			this.colMaSP.Name = "colMaSP";
			this.colMaSP.ReadOnly = true;
			// 
			// colTenSP
			// 
			this.colTenSP.DataPropertyName = "TenSP";
			this.colTenSP.HeaderText = "Tên sản phẩm";
			this.colTenSP.Name = "colTenSP";
			this.colTenSP.ReadOnly = true;
			// 
			// colDanhMuc
			// 
			this.colDanhMuc.DataPropertyName = "TenDanhMuc";
			this.colDanhMuc.HeaderText = "Danh mục";
			this.colDanhMuc.Name = "colDanhMuc";
			this.colDanhMuc.ReadOnly = true;
			// 
			// colGia
			// 
			this.colGia.DataPropertyName = "GiaBan";
			this.colGia.HeaderText = "Đơn giá";
			this.colGia.Name = "colGia";
			this.colGia.ReadOnly = true;
			// 
			// colGiaKM
			// 
			this.colGiaKM.DataPropertyName = "GiaSauKM";
			this.colGiaKM.HeaderText = "Giá sau khuyến mãi";
			this.colGiaKM.Name = "colGiaKM";
			this.colGiaKM.ReadOnly = true;
			// 
			// colTon
			// 
			this.colTon.DataPropertyName = "SoLuongTon";
			this.colTon.HeaderText = "Tồn kho";
			this.colTon.Name = "colTon";
			this.colTon.ReadOnly = true;
			// 
			// lblSoLuong
			// 
			this.lblSoLuong.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)));
			this.lblSoLuong.AutoSize = true;
			this.lblSoLuong.Font = new System.Drawing.Font("Segoe UI", 12F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(163)));
			this.lblSoLuong.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(71)))), ((int)(((byte)(85)))), ((int)(((byte)(105)))));
			this.lblSoLuong.Location = new System.Drawing.Point(15, 410);
			this.lblSoLuong.Name = "lblSoLuong";
			this.lblSoLuong.Size = new System.Drawing.Size(83, 21);
			this.lblSoLuong.TabIndex = 6;
			this.lblSoLuong.Text = "Số lượng:";
			// 
			// nudSoLuong
			// 
			this.nudSoLuong.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)));
			this.nudSoLuong.Font = new System.Drawing.Font("Segoe UI", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(163)));
			this.nudSoLuong.Location = new System.Drawing.Point(100, 406);
			this.nudSoLuong.Minimum = new decimal(new int[] {
            1,
            0,
            0,
            0});
			this.nudSoLuong.Name = "nudSoLuong";
			this.nudSoLuong.Size = new System.Drawing.Size(90, 29);
			this.nudSoLuong.TabIndex = 3;
			this.nudSoLuong.Value = new decimal(new int[] {
            1,
            0,
            0,
            0});
			// 
			// btnThem
			// 
			this.btnThem.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)));
			this.btnThem.BackColor = System.Drawing.Color.DodgerBlue;
			this.btnThem.FlatAppearance.BorderSize = 0;
			this.btnThem.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
			this.btnThem.Font = new System.Drawing.Font("Segoe UI", 12F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(163)));
			this.btnThem.ForeColor = System.Drawing.Color.White;
			this.btnThem.Location = new System.Drawing.Point(209, 399);
			this.btnThem.Name = "btnThem";
			this.btnThem.Size = new System.Drawing.Size(216, 42);
			this.btnThem.TabIndex = 4;
			this.btnThem.Text = "Thêm vào hóa đơn";
			this.btnThem.UseVisualStyleBackColor = false;
			this.btnThem.Click += new System.EventHandler(this.btnThem_Click);
			// 
			// lblThongBao
			// 
			this.lblThongBao.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)));
			this.lblThongBao.AutoSize = true;
			this.lblThongBao.Font = new System.Drawing.Font("Microsoft Sans Serif", 12F);
			this.lblThongBao.ForeColor = System.Drawing.Color.ForestGreen;
			this.lblThongBao.Location = new System.Drawing.Point(15, 455);
			this.lblThongBao.Name = "lblThongBao";
			this.lblThongBao.Size = new System.Drawing.Size(0, 20);
			this.lblThongBao.TabIndex = 7;
			// 
			// btnXong
			// 
			this.btnXong.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
			this.btnXong.BackColor = System.Drawing.Color.WhiteSmoke;
			this.btnXong.DialogResult = System.Windows.Forms.DialogResult.OK;
			this.btnXong.FlatAppearance.BorderSize = 0;
			this.btnXong.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
			this.btnXong.Font = new System.Drawing.Font("Segoe UI", 12F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(163)));
			this.btnXong.Location = new System.Drawing.Point(665, 398);
			this.btnXong.Name = "btnXong";
			this.btnXong.Size = new System.Drawing.Size(140, 42);
			this.btnXong.TabIndex = 8;
			this.btnXong.Text = "Xong";
			this.btnXong.UseVisualStyleBackColor = false;
			// 
			// frmChonSanPham
			// 
			this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
			this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
			this.BackColor = System.Drawing.Color.LightSteelBlue;
			this.CancelButton = this.btnXong;
			this.ClientSize = new System.Drawing.Size(820, 495);
			this.Controls.Add(this.btnXong);
			this.Controls.Add(this.lblThongBao);
			this.Controls.Add(this.btnThem);
			this.Controls.Add(this.nudSoLuong);
			this.Controls.Add(this.lblSoLuong);
			this.Controls.Add(this.dgvSanPham);
			this.Controls.Add(this.btnTim);
			this.Controls.Add(this.txtTim);
			this.Controls.Add(this.lblTim);
			this.MinimizeBox = false;
			this.Name = "frmChonSanPham";
			this.ShowInTaskbar = false;
			this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
			this.Text = "Chọn sản phẩm";
			this.Load += new System.EventHandler(this.frmChonSanPham_Load);
			((System.ComponentModel.ISupportInitialize)(this.dgvSanPham)).EndInit();
			((System.ComponentModel.ISupportInitialize)(this.nudSoLuong)).EndInit();
			this.ResumeLayout(false);
			this.PerformLayout();

		}

		#endregion

		private System.Windows.Forms.Label lblTim;
		private System.Windows.Forms.TextBox txtTim;
		private System.Windows.Forms.Button btnTim;
		private System.Windows.Forms.DataGridView dgvSanPham;
		private System.Windows.Forms.DataGridViewTextBoxColumn colMaSP;
		private System.Windows.Forms.DataGridViewTextBoxColumn colTenSP;
		private System.Windows.Forms.DataGridViewTextBoxColumn colDanhMuc;
		private System.Windows.Forms.DataGridViewTextBoxColumn colGia;
		private System.Windows.Forms.DataGridViewTextBoxColumn colGiaKM;
		private System.Windows.Forms.DataGridViewTextBoxColumn colTon;
		private System.Windows.Forms.Label lblSoLuong;
		private System.Windows.Forms.NumericUpDown nudSoLuong;
		private System.Windows.Forms.Button btnThem;
		private System.Windows.Forms.Label lblThongBao;
		private System.Windows.Forms.Button btnXong;
	}
}
