namespace PowerLib.Winform.Samples
{
    partial class FrmRipplePictureBoxTest
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(FrmRipplePictureBoxTest));
            this.ripplePictureBox1 = new PowerLib.Winform.Controls.RipplePictureBox();
            this.SuspendLayout();
            // 
            // ripplePictureBox1
            // 
            this.ripplePictureBox1.AnimationEnabled = false;
            this.ripplePictureBox1.Dock = System.Windows.Forms.DockStyle.Fill;
            this.ripplePictureBox1.DragSplashRadius = 10;
            this.ripplePictureBox1.Image = ((System.Drawing.Image)(resources.GetObject("ripplePictureBox1.Image")));
            this.ripplePictureBox1.Location = new System.Drawing.Point(0, 0);
            this.ripplePictureBox1.MinimumSize = new System.Drawing.Size(256, 256);
            this.ripplePictureBox1.Name = "ripplePictureBox1";
            this.ripplePictureBox1.Size = new System.Drawing.Size(800, 450);
            this.ripplePictureBox1.TabIndex = 0;
            this.ripplePictureBox1.Text = "ripplePictureBox1";
            // 
            // FrmRipplePictureBoxTest
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 12F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(800, 450);
            this.Controls.Add(this.ripplePictureBox1);
            this.Name = "FrmRipplePictureBoxTest";
            this.Text = "FrmRipplePictureBoxTest";
            this.ResumeLayout(false);

        }

        #endregion

        private Controls.RipplePictureBox ripplePictureBox1;
    }
}