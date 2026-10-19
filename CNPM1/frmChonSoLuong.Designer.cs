namespace CNPM1
{
    partial class frmChonSoLuong
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
            this.lblTenSP = new System.Windows.Forms.Label();
            this.lblDonGia = new System.Windows.Forms.Label();
            this.lblTon = new System.Windows.Forms.Label();
            this.lblSoLuong = new System.Windows.Forms.Label();
            this.nudSoLuong = new System.Windows.Forms.NumericUpDown();
            this.lblTong = new System.Windows.Forms.Label();
            this.lblGiam = new System.Windows.Forms.Label();
            this.lblPhaiTra = new System.Windows.Forms.Label();
            this.lblHinhThuc = new System.Windows.Forms.Label();
            this.cboHinhThuc = new System.Windows.Forms.ComboBox();
            this.btnDongY = new System.Windows.Forms.Button();
            this.btnHuy = new System.Windows.Forms.Button();
            ((System.ComponentModel.ISupportInitialize)(this.nudSoLuong)).BeginInit();
            this.SuspendLayout();
            // 
            // lblTenSP
            // 
            this.lblTenSP.Font = new System.Drawing.Font("Microsoft Sans Serif", 14F, System.Drawing.FontStyle.Bold);
            this.lblTenSP.Location = new System.Drawing.Point(20, 15);
            this.lblTenSP.Name = "lblTenSP";
            this.lblTenSP.Size = new System.Drawing.Size(360, 50);
            this.lblTenSP.TabIndex = 6;
            this.lblTenSP.Text = "Tên sản phẩm";
            // 
            // lblDonGia
            // 
            this.lblDonGia.AutoSize = true;
            this.lblDonGia.Font = new System.Drawing.Font("Microsoft Sans Serif", 12F);
            this.lblDonGia.Location = new System.Drawing.Point(20, 70);
            this.lblDonGia.Name = "lblDonGia";
            this.lblDonGia.Size = new System.Drawing.Size(100, 20);
            this.lblDonGia.TabIndex = 7;
            this.lblDonGia.Text = "Đơn giá: 0 đ";
            // 
            // lblTon
            // 
            this.lblTon.AutoSize = true;
            this.lblTon.Font = new System.Drawing.Font("Microsoft Sans Serif", 12F);
            this.lblTon.Location = new System.Drawing.Point(20, 95);
            this.lblTon.Name = "lblTon";
            this.lblTon.Size = new System.Drawing.Size(130, 20);
            this.lblTon.TabIndex = 8;
            this.lblTon.Text = "Còn trong kho: 0";
            // 
            // lblSoLuong
            // 
            this.lblSoLuong.AutoSize = true;
            this.lblSoLuong.Font = new System.Drawing.Font("Microsoft Sans Serif", 12F);
            this.lblSoLuong.Location = new System.Drawing.Point(20, 130);
            this.lblSoLuong.Name = "lblSoLuong";
            this.lblSoLuong.Size = new System.Drawing.Size(80, 20);
            this.lblSoLuong.TabIndex = 9;
            this.lblSoLuong.Text = "Số lượng:";
            // 
            // nudSoLuong
            // 
            this.nudSoLuong.Font = new System.Drawing.Font("Microsoft Sans Serif", 12F);
            this.nudSoLuong.Location = new System.Drawing.Point(140, 126);
            this.nudSoLuong.Name = "nudSoLuong";
            this.nudSoLuong.Size = new System.Drawing.Size(100, 26);
            this.nudSoLuong.TabIndex = 0;
            this.nudSoLuong.ValueChanged += new System.EventHandler(this.nudSoLuong_ValueChanged);
            // 
            // lblTong
            // 
            this.lblTong.AutoSize = true;
            this.lblTong.Font = new System.Drawing.Font("Microsoft Sans Serif", 12F);
            this.lblTong.Location = new System.Drawing.Point(20, 170);
            this.lblTong.Name = "lblTong";
            this.lblTong.Size = new System.Drawing.Size(150, 20);
            this.lblTong.TabIndex = 10;
            this.lblTong.Text = "Tổng tiền hàng: 0 đ";
            // 
            // lblGiam
            // 
            this.lblGiam.AutoSize = true;
            this.lblGiam.Font = new System.Drawing.Font("Microsoft Sans Serif", 12F);
            this.lblGiam.Location = new System.Drawing.Point(20, 198);
            this.lblGiam.Name = "lblGiam";
            this.lblGiam.Size = new System.Drawing.Size(110, 20);
            this.lblGiam.TabIndex = 11;
            this.lblGiam.Text = "Giảm giá: 0 đ";
            // 
            // lblPhaiTra
            // 
            this.lblPhaiTra.AutoSize = true;
            this.lblPhaiTra.Font = new System.Drawing.Font("Microsoft Sans Serif", 14F, System.Drawing.FontStyle.Bold);
            this.lblPhaiTra.Location = new System.Drawing.Point(20, 228);
            this.lblPhaiTra.Name = "lblPhaiTra";
            this.lblPhaiTra.Size = new System.Drawing.Size(120, 24);
            this.lblPhaiTra.TabIndex = 12;
            this.lblPhaiTra.Text = "Phải trả: 0 đ";
            // 
            // lblHinhThuc
            // 
            this.lblHinhThuc.AutoSize = true;
            this.lblHinhThuc.Font = new System.Drawing.Font("Microsoft Sans Serif", 12F);
            this.lblHinhThuc.Location = new System.Drawing.Point(20, 283);
            this.lblHinhThuc.Name = "lblHinhThuc";
            this.lblHinhThuc.Size = new System.Drawing.Size(90, 20);
            this.lblHinhThuc.TabIndex = 13;
            this.lblHinhThuc.Text = "Hình thức:";
            // 
            // cboHinhThuc
            // 
            this.cboHinhThuc.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.cboHinhThuc.Font = new System.Drawing.Font("Microsoft Sans Serif", 12F);
            this.cboHinhThuc.FormattingEnabled = true;
            this.cboHinhThuc.Items.AddRange(new object[] {
            "Tiền mặt",
            "Chuyển khoản"});
            this.cboHinhThuc.Location = new System.Drawing.Point(140, 279);
            this.cboHinhThuc.Name = "cboHinhThuc";
            this.cboHinhThuc.Size = new System.Drawing.Size(200, 28);
            this.cboHinhThuc.TabIndex = 1;
            // 
            // btnDongY
            // 
            this.btnDongY.DialogResult = System.Windows.Forms.DialogResult.OK;
            this.btnDongY.Font = new System.Drawing.Font("Microsoft Sans Serif", 14F);
            this.btnDongY.Location = new System.Drawing.Point(40, 330);
            this.btnDongY.Name = "btnDongY";
            this.btnDongY.Size = new System.Drawing.Size(170, 45);
            this.btnDongY.TabIndex = 2;
            this.btnDongY.Text = "Mua";
            this.btnDongY.UseVisualStyleBackColor = true;
            // 
            // btnHuy
            // 
            this.btnHuy.DialogResult = System.Windows.Forms.DialogResult.Cancel;
            this.btnHuy.Font = new System.Drawing.Font("Microsoft Sans Serif", 14F);
            this.btnHuy.Location = new System.Drawing.Point(220, 330);
            this.btnHuy.Name = "btnHuy";
            this.btnHuy.Size = new System.Drawing.Size(140, 45);
            this.btnHuy.TabIndex = 3;
            this.btnHuy.Text = "Hủy";
            this.btnHuy.UseVisualStyleBackColor = true;
            // 
            // frmChonSoLuong
            // 
            this.AcceptButton = this.btnDongY;
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.CancelButton = this.btnHuy;
            this.ClientSize = new System.Drawing.Size(400, 395);
            this.Controls.Add(this.btnHuy);
            this.Controls.Add(this.btnDongY);
            this.Controls.Add(this.cboHinhThuc);
            this.Controls.Add(this.lblHinhThuc);
            this.Controls.Add(this.lblPhaiTra);
            this.Controls.Add(this.lblGiam);
            this.Controls.Add(this.lblTong);
            this.Controls.Add(this.nudSoLuong);
            this.Controls.Add(this.lblSoLuong);
            this.Controls.Add(this.lblTon);
            this.Controls.Add(this.lblDonGia);
            this.Controls.Add(this.lblTenSP);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedDialog;
            this.MaximizeBox = false;
            this.MinimizeBox = false;
            this.Name = "frmChonSoLuong";
            this.ShowInTaskbar = false;
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterParent;
            this.Text = "Chọn số lượng";
            ((System.ComponentModel.ISupportInitialize)(this.nudSoLuong)).EndInit();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.Label lblTenSP;
        private System.Windows.Forms.Label lblDonGia;
        private System.Windows.Forms.Label lblTon;
        private System.Windows.Forms.Label lblSoLuong;
        private System.Windows.Forms.NumericUpDown nudSoLuong;
        private System.Windows.Forms.Label lblTong;
        private System.Windows.Forms.Label lblGiam;
        private System.Windows.Forms.Label lblPhaiTra;
        private System.Windows.Forms.Label lblHinhThuc;
        private System.Windows.Forms.ComboBox cboHinhThuc;
        private System.Windows.Forms.Button btnDongY;
        private System.Windows.Forms.Button btnHuy;
    }
}
