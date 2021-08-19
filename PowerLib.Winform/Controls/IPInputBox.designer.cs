namespace PowerLib.Winform.Controls
{
    partial class IPInputBox
    {
        /// <summary> 
        /// 必需的设计器变量。
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary> 
        /// 清理所有正在使用的资源。
        /// </summary>
        /// <param name="disposing">如果应释放托管资源，为 true；否则为 false。</param>
        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        #region 组件设计器生成的代码

        /// <summary> 
        /// 设计器支持所需的方法 - 不要修改
        /// 使用代码编辑器修改此方法的内容。
        /// </summary>
        private void InitializeComponent()
        {
            this.txt1 = new System.Windows.Forms.TextBox();
            this.txt2 = new System.Windows.Forms.TextBox();
            this.txt3 = new System.Windows.Forms.TextBox();
            this.txt4 = new System.Windows.Forms.TextBox();
            this.SuspendLayout();
            // 
            // txt1
            // 
            this.txt1.BackColor = System.Drawing.Color.White;
            this.txt1.BorderStyle = System.Windows.Forms.BorderStyle.None;
            this.txt1.Location = new System.Drawing.Point(3, 3);
            this.txt1.Margin = new System.Windows.Forms.Padding(3, 3, 10, 3);
            this.txt1.MaxLength = 3;
            this.txt1.Name = "txt1";
            this.txt1.Size = new System.Drawing.Size(28, 14);
            this.txt1.TabIndex = 0;
            this.txt1.TextAlign = System.Windows.Forms.HorizontalAlignment.Center;
            // 
            // txt2
            // 
            this.txt2.BackColor = System.Drawing.Color.White;
            this.txt2.BorderStyle = System.Windows.Forms.BorderStyle.None;
            this.txt2.Location = new System.Drawing.Point(36, 3);
            this.txt2.Margin = new System.Windows.Forms.Padding(3, 3, 10, 3);
            this.txt2.MaxLength = 3;
            this.txt2.Name = "txt2";
            this.txt2.Size = new System.Drawing.Size(28, 14);
            this.txt2.TabIndex = 0;
            this.txt2.TabStop = false;
            this.txt2.TextAlign = System.Windows.Forms.HorizontalAlignment.Center;
            // 
            // txt3
            // 
            this.txt3.BackColor = System.Drawing.Color.White;
            this.txt3.BorderStyle = System.Windows.Forms.BorderStyle.None;
            this.txt3.Location = new System.Drawing.Point(69, 3);
            this.txt3.Margin = new System.Windows.Forms.Padding(3, 3, 10, 3);
            this.txt3.MaxLength = 3;
            this.txt3.Name = "txt3";
            this.txt3.Size = new System.Drawing.Size(28, 14);
            this.txt3.TabIndex = 0;
            this.txt3.TabStop = false;
            this.txt3.TextAlign = System.Windows.Forms.HorizontalAlignment.Center;
            // 
            // txt4
            // 
            this.txt4.BackColor = System.Drawing.Color.White;
            this.txt4.BorderStyle = System.Windows.Forms.BorderStyle.None;
            this.txt4.Location = new System.Drawing.Point(102, 3);
            this.txt4.Margin = new System.Windows.Forms.Padding(3, 3, 10, 3);
            this.txt4.MaxLength = 3;
            this.txt4.Name = "txt4";
            this.txt4.Size = new System.Drawing.Size(28, 14);
            this.txt4.TabIndex = 0;
            this.txt4.TabStop = false;
            this.txt4.TextAlign = System.Windows.Forms.HorizontalAlignment.Center;
            // 
            // IPTextBox
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 12F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.BackColor = System.Drawing.Color.White;
            this.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.Controls.Add(this.txt4);
            this.Controls.Add(this.txt3);
            this.Controls.Add(this.txt2);
            this.Controls.Add(this.txt1);
            this.Name = "IPTextBox";
            this.Padding = new System.Windows.Forms.Padding(1);
            this.Size = new System.Drawing.Size(133, 21);
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.TextBox txt1;
        private System.Windows.Forms.TextBox txt2;
        private System.Windows.Forms.TextBox txt3;
        private System.Windows.Forms.TextBox txt4;
    }
}
