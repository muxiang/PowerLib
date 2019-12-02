namespace SignalerTool.Forms
{
    partial class FrmDetectorsSelect
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
            this.btnOK = new PowerControl.XButton();
            this.btnCancel = new PowerControl.XButton();
            this.chklstDetectors = new System.Windows.Forms.CheckedListBox();
            this.SuspendLayout();
            //  
            //  btnOK
            //  
            this.btnOK.BorderColor = System.Drawing.Color.White;
            this.btnOK.CheckedEndColor = System.Drawing.Color.FromArgb(((int)(((byte)(157)))), ((int)(((byte)(153)))), ((int)(((byte)(155)))));
            this.btnOK.CheckedStartColor = System.Drawing.Color.FromArgb(((int)(((byte)(186)))), ((int)(((byte)(185)))), ((int)(((byte)(181)))));
            this.btnOK.EndColor = System.Drawing.Color.FromArgb(((int)(((byte)(90)))), ((int)(((byte)(107)))), ((int)(((byte)(125)))));
            this.btnOK.Font = new System.Drawing.Font("微软雅黑", 8F, System.Drawing.FontStyle.Bold);
            this.btnOK.ForeColor = System.Drawing.Color.White;
            this.btnOK.Image = null;
            this.btnOK.Location = new System.Drawing.Point(69, 537);
            this.btnOK.Margin = new System.Windows.Forms.Padding(3, 4, 3, 4);
            this.btnOK.Name = "btnOK";
            this.btnOK.Size = new System.Drawing.Size(87, 36);
            this.btnOK.StartColor = System.Drawing.Color.FromArgb(((int)(((byte)(126)))), ((int)(((byte)(135)))), ((int)(((byte)(142)))));
            this.btnOK.TabIndex = 0;
            this.btnOK.Text = "确定";
            this.btnOK.Click += new System.EventHandler(this.btnOK_Click);
            //  
            //  btnCancel
            //  
            this.btnCancel.BorderColor = System.Drawing.Color.White;
            this.btnCancel.CheckedEndColor = System.Drawing.Color.FromArgb(((int)(((byte)(157)))), ((int)(((byte)(153)))), ((int)(((byte)(155)))));
            this.btnCancel.CheckedStartColor = System.Drawing.Color.FromArgb(((int)(((byte)(186)))), ((int)(((byte)(185)))), ((int)(((byte)(181)))));
            this.btnCancel.DialogResult = System.Windows.Forms.DialogResult.Cancel;
            this.btnCancel.EndColor = System.Drawing.Color.FromArgb(((int)(((byte)(90)))), ((int)(((byte)(107)))), ((int)(((byte)(125)))));
            this.btnCancel.Font = new System.Drawing.Font("微软雅黑", 8F, System.Drawing.FontStyle.Bold);
            this.btnCancel.ForeColor = System.Drawing.Color.White;
            this.btnCancel.Image = null;
            this.btnCancel.Location = new System.Drawing.Point(237, 537);
            this.btnCancel.Margin = new System.Windows.Forms.Padding(3, 4, 3, 4);
            this.btnCancel.Name = "btnCancel";
            this.btnCancel.Size = new System.Drawing.Size(87, 36);
            this.btnCancel.StartColor = System.Drawing.Color.FromArgb(((int)(((byte)(126)))), ((int)(((byte)(135)))), ((int)(((byte)(142)))));
            this.btnCancel.TabIndex = 0;
            this.btnCancel.Text = "取消";
            this.btnCancel.Click += new System.EventHandler(this.btnCancel_Click);
            //  
            //  chklstDetectors
            //  
            this.chklstDetectors.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(45)))), ((int)(((byte)(52)))), ((int)(((byte)(60)))));
            this.chklstDetectors.BorderStyle = System.Windows.Forms.BorderStyle.None;
            this.chklstDetectors.CheckOnClick = true;
            this.chklstDetectors.ForeColor = System.Drawing.Color.White;
            this.chklstDetectors.FormattingEnabled = true;
            this.chklstDetectors.Location = new System.Drawing.Point(15, 17);
            this.chklstDetectors.Margin = new System.Windows.Forms.Padding(3, 4, 3, 4);
            this.chklstDetectors.Name = "chklstDetectors";
            this.chklstDetectors.Size = new System.Drawing.Size(359, 476);
            this.chklstDetectors.TabIndex = 1;
            //  
            //  FrmDetectorsSelect
            //  
            this.AcceptButton = this.btnOK;
            this.AutoScaleDimensions = new System.Drawing.SizeF(7F, 16F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(45)))), ((int)(((byte)(52)))), ((int)(((byte)(60)))));
            this.CancelButton = this.btnCancel;
            this.ClientSize = new System.Drawing.Size(388, 592);
            this.Controls.Add(this.chklstDetectors);
            this.Controls.Add(this.btnCancel);
            this.Controls.Add(this.btnOK);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedSingle;
            this.Margin = new System.Windows.Forms.Padding(3, 4, 3, 4);
            this.MaximizeBox = false;
            this.MinimizeBox = false;
            this.Name = "FrmDetectorsSelect";
            this.ShowIcon = false;
            this.ShowInTaskbar = false;
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterParent;
            this.Text = "检测器选择";
            this.Load += new System.EventHandler(this.FrmDetectorsSelect_Load);
            this.ResumeLayout(false);

        }

        #endregion

        private PowerControl.XButton btnOK;
        private PowerControl.XButton btnCancel;
        private System.Windows.Forms.CheckedListBox chklstDetectors;
    }
}