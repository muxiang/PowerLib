namespace ControlTest
{
    partial class FrmAbout
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(FrmAbout));
            this.ripplePictureBox1 = new PowerControl.RipplePictureBox();
            ((System.ComponentModel.ISupportInitialize)(this.ripplePictureBox1)).BeginInit();
            this.SuspendLayout();
            // 
            // ripplePictureBox1
            // 
            this.ripplePictureBox1.DragSplashRadius = 10;
            this.ripplePictureBox1.HoverSplash = true;
            this.ripplePictureBox1.Image = ((System.Drawing.Image)(resources.GetObject("ripplePictureBox1.Image")));
            this.ripplePictureBox1.Location = new System.Drawing.Point(24, 12);
            this.ripplePictureBox1.MinimumSize = new System.Drawing.Size(256, 256);
            this.ripplePictureBox1.Name = "ripplePictureBox1";
            this.ripplePictureBox1.Size = new System.Drawing.Size(630, 256);
            this.ripplePictureBox1.TabIndex = 0;
            this.ripplePictureBox1.TabStop = false;
            // 
            // FrmAbout
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(7F, 16F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(708, 360);
            this.Controls.Add(this.ripplePictureBox1);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedSingle;
            this.MaximizeBox = false;
            this.MinimizeBox = false;
            this.Name = "FrmAbout";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
            this.Text = "关于";
            ((System.ComponentModel.ISupportInitialize)(this.ripplePictureBox1)).EndInit();
            this.ResumeLayout(false);

        }

        #endregion

        private PowerControl.RipplePictureBox ripplePictureBox1;
    }
}