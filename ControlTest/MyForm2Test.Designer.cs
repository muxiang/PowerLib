namespace ControlTest
{
    partial class MyForm2Test
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
            this.xPanel1 = new PowerControl.XShadowPanel();
            this.button1 = new System.Windows.Forms.Button();
            this.xPanel1.SuspendLayout();
            this.SuspendLayout();
            // 
            // xPanel1
            // 
            this.xPanel1.AutoScroll = true;
            this.xPanel1.Controls.Add(this.button1);
            this.xPanel1.Location = new System.Drawing.Point(164, 129);
            this.xPanel1.Name = "xPanel1";
            this.xPanel1.ShadowColor = System.Drawing.Color.FromArgb(((int)(((byte)(245)))), ((int)(((byte)(245)))), ((int)(((byte)(245)))));
            this.xPanel1.Size = new System.Drawing.Size(615, 307);
            this.xPanel1.TabIndex = 0;
            this.xPanel1.TopBorderColor = System.Drawing.Color.FromArgb(((int)(((byte)(224)))), ((int)(((byte)(224)))), ((int)(((byte)(224)))));
            // 
            // button1
            // 
            this.button1.Location = new System.Drawing.Point(186, 127);
            this.button1.Name = "button1";
            this.button1.Size = new System.Drawing.Size(75, 254);
            this.button1.TabIndex = 0;
            this.button1.Text = "button1";
            this.button1.UseVisualStyleBackColor = true;
            // 
            // MyForm2Test
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 12F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(1135, 730);
            this.Controls.Add(this.xPanel1);
            this.Name = "MyForm2Test";
            this.Text = "MyForm2Test";
            this.xPanel1.ResumeLayout(false);
            this.ResumeLayout(false);

        }

        #endregion

        private PowerControl.XShadowPanel xPanel1;
        private System.Windows.Forms.Button button1;
    }
}