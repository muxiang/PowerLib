
namespace PowerLib.Winform.Sample
{
    partial class FrmXFileProgressBarTest
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
            this.xFileProgressBar1 = new PowerLib.Winform.Controls.XFileProgressBar();
            this.numRandomLow = new System.Windows.Forms.NumericUpDown();
            this.numRandomHigh = new System.Windows.Forms.NumericUpDown();
            this.label1 = new System.Windows.Forms.Label();
            this.label2 = new System.Windows.Forms.Label();
            this.label3 = new System.Windows.Forms.Label();
            this.btnClear = new PowerLib.Winform.Controls.XButton();
            this.btnAuto = new PowerLib.Winform.Controls.XButton();
            ((System.ComponentModel.ISupportInitialize)(this.numRandomLow)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.numRandomHigh)).BeginInit();
            this.SuspendLayout();
            // 
            // xFileProgressBar1
            // 
            this.xFileProgressBar1.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(255)))), ((int)(((byte)(255)))), ((int)(((byte)(255)))));
            this.xFileProgressBar1.Font = new System.Drawing.Font("微软雅黑", 8F);
            this.xFileProgressBar1.Location = new System.Drawing.Point(60, 47);
            this.xFileProgressBar1.Name = "xFileProgressBar1";
            this.xFileProgressBar1.Size = new System.Drawing.Size(400, 80);
            this.xFileProgressBar1.TabIndex = 0;
            this.xFileProgressBar1.Text = "xFileProgressBar1";
            this.xFileProgressBar1.TotalSizeInBytes = ((long)(0));
            // 
            // numRandomLow
            // 
            this.numRandomLow.Location = new System.Drawing.Point(125, 165);
            this.numRandomLow.Maximum = new decimal(new int[] {
            -1,
            -1,
            -1,
            0});
            this.numRandomLow.Name = "numRandomLow";
            this.numRandomLow.Size = new System.Drawing.Size(75, 21);
            this.numRandomLow.TabIndex = 2;
            this.numRandomLow.Value = new decimal(new int[] {
            204800,
            0,
            0,
            0});
            // 
            // numRandomHigh
            // 
            this.numRandomHigh.Location = new System.Drawing.Point(273, 165);
            this.numRandomHigh.Maximum = new decimal(new int[] {
            -1,
            -1,
            -1,
            0});
            this.numRandomHigh.Name = "numRandomHigh";
            this.numRandomHigh.Size = new System.Drawing.Size(77, 21);
            this.numRandomHigh.TabIndex = 3;
            this.numRandomHigh.Value = new decimal(new int[] {
            819200,
            0,
            0,
            0});
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(247, 169);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(11, 12);
            this.label1.TabIndex = 4;
            this.label1.Text = "-";
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Location = new System.Drawing.Point(202, 169);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(35, 12);
            this.label2.TabIndex = 4;
            this.label2.Text = "bytes";
            // 
            // label3
            // 
            this.label3.AutoSize = true;
            this.label3.Location = new System.Drawing.Point(352, 169);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(35, 12);
            this.label3.TabIndex = 4;
            this.label3.Text = "bytes";
            // 
            // btnClear
            // 
            this.btnClear.CheckedEndColor = System.Drawing.Color.FromArgb(((int)(((byte)(83)))), ((int)(((byte)(128)))), ((int)(((byte)(252)))));
            this.btnClear.CheckedForeColor = System.Drawing.Color.White;
            this.btnClear.CheckedStartColor = System.Drawing.Color.FromArgb(((int)(((byte)(109)))), ((int)(((byte)(169)))), ((int)(((byte)(255)))));
            this.btnClear.DefaultButtonBorderWidth = 2;
            this.btnClear.EndColor = System.Drawing.Color.FromArgb(((int)(((byte)(232)))), ((int)(((byte)(232)))), ((int)(((byte)(232)))));
            this.btnClear.Font = new System.Drawing.Font("微软雅黑", 8F);
            this.btnClear.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(80)))), ((int)(((byte)(80)))), ((int)(((byte)(80)))));
            this.btnClear.HoldingEndColor = System.Drawing.Color.FromArgb(((int)(((byte)(83)))), ((int)(((byte)(128)))), ((int)(((byte)(252)))));
            this.btnClear.HoldingForeColor = System.Drawing.Color.White;
            this.btnClear.HoldingImage = null;
            this.btnClear.HoldingStartColor = System.Drawing.Color.FromArgb(((int)(((byte)(109)))), ((int)(((byte)(169)))), ((int)(((byte)(255)))));
            this.btnClear.Image = null;
            this.btnClear.Location = new System.Drawing.Point(287, 238);
            this.btnClear.Name = "btnClear";
            this.btnClear.Size = new System.Drawing.Size(110, 46);
            this.btnClear.StartColor = System.Drawing.Color.White;
            this.btnClear.TabIndex = 1;
            this.btnClear.Text = "Clear";
            this.btnClear.Click += new System.EventHandler(this.btnClear_Click);
            // 
            // btnAuto
            // 
            this.btnAuto.CheckedEndColor = System.Drawing.Color.FromArgb(((int)(((byte)(83)))), ((int)(((byte)(128)))), ((int)(((byte)(252)))));
            this.btnAuto.CheckedForeColor = System.Drawing.Color.White;
            this.btnAuto.CheckedStartColor = System.Drawing.Color.FromArgb(((int)(((byte)(109)))), ((int)(((byte)(169)))), ((int)(((byte)(255)))));
            this.btnAuto.DefaultButtonBorderWidth = 2;
            this.btnAuto.EndColor = System.Drawing.Color.FromArgb(((int)(((byte)(232)))), ((int)(((byte)(232)))), ((int)(((byte)(232)))));
            this.btnAuto.Font = new System.Drawing.Font("微软雅黑", 8F);
            this.btnAuto.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(80)))), ((int)(((byte)(80)))), ((int)(((byte)(80)))));
            this.btnAuto.HoldingEndColor = System.Drawing.Color.FromArgb(((int)(((byte)(83)))), ((int)(((byte)(128)))), ((int)(((byte)(252)))));
            this.btnAuto.HoldingForeColor = System.Drawing.Color.White;
            this.btnAuto.HoldingImage = null;
            this.btnAuto.HoldingStartColor = System.Drawing.Color.FromArgb(((int)(((byte)(109)))), ((int)(((byte)(169)))), ((int)(((byte)(255)))));
            this.btnAuto.Image = null;
            this.btnAuto.Location = new System.Drawing.Point(128, 238);
            this.btnAuto.Name = "btnAuto";
            this.btnAuto.Size = new System.Drawing.Size(110, 46);
            this.btnAuto.StartColor = System.Drawing.Color.White;
            this.btnAuto.TabIndex = 1;
            this.btnAuto.Text = "Auto";
            this.btnAuto.Click += new System.EventHandler(this.btnAuto_Click);
            // 
            // FrmXFileProgressBarTest
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 12F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(531, 317);
            this.Controls.Add(this.label3);
            this.Controls.Add(this.label2);
            this.Controls.Add(this.label1);
            this.Controls.Add(this.numRandomHigh);
            this.Controls.Add(this.numRandomLow);
            this.Controls.Add(this.btnAuto);
            this.Controls.Add(this.btnClear);
            this.Controls.Add(this.xFileProgressBar1);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedSingle;
            this.MaximizeBox = false;
            this.MinimizeBox = false;
            this.Name = "FrmXFileProgressBarTest";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterParent;
            this.Text = "文件进度条示例";
            ((System.ComponentModel.ISupportInitialize)(this.numRandomLow)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.numRandomHigh)).EndInit();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private Controls.XFileProgressBar xFileProgressBar1;
        private System.Windows.Forms.NumericUpDown numRandomLow;
        private System.Windows.Forms.NumericUpDown numRandomHigh;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.Label label3;
        private Controls.XButton btnClear;
        private Controls.XButton btnAuto;
    }
}