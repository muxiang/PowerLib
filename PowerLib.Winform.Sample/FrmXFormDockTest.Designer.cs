
namespace PowerLib.Winform.Sample
{
    partial class FrmXFormDockTest
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
            this.panel1 = new System.Windows.Forms.Panel();
            this.xButton1 = new PowerLib.Winform.Controls.XButton();
            this.panel1.SuspendLayout();
            this.SuspendLayout();
            // 
            // panel1
            // 
            this.panel1.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.panel1.Controls.Add(this.xButton1);
            this.panel1.Dock = System.Windows.Forms.DockStyle.Fill;
            this.panel1.Location = new System.Drawing.Point(0, 0);
            this.panel1.Name = "panel1";
            this.panel1.Size = new System.Drawing.Size(664, 634);
            this.panel1.TabIndex = 0;
            // 
            // xButton1
            // 
            this.xButton1.CheckedEndColor = System.Drawing.Color.FromArgb(((int)(((byte)(83)))), ((int)(((byte)(128)))), ((int)(((byte)(252)))));
            this.xButton1.CheckedForeColor = System.Drawing.Color.White;
            this.xButton1.CheckedStartColor = System.Drawing.Color.FromArgb(((int)(((byte)(109)))), ((int)(((byte)(169)))), ((int)(((byte)(255)))));
            this.xButton1.DefaultButtonBorderWidth = 2;
            this.xButton1.EndColor = System.Drawing.Color.FromArgb(((int)(((byte)(232)))), ((int)(((byte)(232)))), ((int)(((byte)(232)))));
            this.xButton1.Font = new System.Drawing.Font("微软雅黑", 8F);
            this.xButton1.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(80)))), ((int)(((byte)(80)))), ((int)(((byte)(80)))));
            this.xButton1.HoldingEndColor = System.Drawing.Color.FromArgb(((int)(((byte)(83)))), ((int)(((byte)(128)))), ((int)(((byte)(252)))));
            this.xButton1.HoldingForeColor = System.Drawing.Color.White;
            this.xButton1.HoldingImage = null;
            this.xButton1.HoldingStartColor = System.Drawing.Color.FromArgb(((int)(((byte)(109)))), ((int)(((byte)(169)))), ((int)(((byte)(255)))));
            this.xButton1.Image = null;
            this.xButton1.Location = new System.Drawing.Point(413, 136);
            this.xButton1.Name = "xButton1";
            this.xButton1.Size = new System.Drawing.Size(75, 23);
            this.xButton1.StartColor = System.Drawing.Color.White;
            this.xButton1.TabIndex = 0;
            this.xButton1.Text = "xButton1";
            this.xButton1.Click += new System.EventHandler(this.xButton1_Click);
            // 
            // FrmXFormDockTest
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 12F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(664, 664);
            this.Controls.Add(this.panel1);
            this.Location = new System.Drawing.Point(100, 100);
            this.Name = "FrmXFormDockTest";
            this.StartPosition = System.Windows.Forms.FormStartPosition.Manual;
            this.Text = "FrmXFormDockTest";
            this.panel1.ResumeLayout(false);
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.Panel panel1;
        private Controls.XButton xButton1;
    }
}