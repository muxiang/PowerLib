namespace ControlTest
{
    partial class FrmShadowPanelTest
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
            System.Windows.Forms.DataGridViewCellStyle dataGridViewCellStyle13 = new System.Windows.Forms.DataGridViewCellStyle();
            System.Windows.Forms.DataGridViewCellStyle dataGridViewCellStyle14 = new System.Windows.Forms.DataGridViewCellStyle();
            System.Windows.Forms.DataGridViewCellStyle dataGridViewCellStyle15 = new System.Windows.Forms.DataGridViewCellStyle();
            System.Windows.Forms.DataGridViewCellStyle dataGridViewCellStyle16 = new System.Windows.Forms.DataGridViewCellStyle();
            this.xPanel1 = new PowerControl.XShadowPanel();
            this.xGroupBox1 = new PowerControl.XGroupBox();
            this.xDataGridView1 = new PowerControl.XDataGridView();
            this.Column_Id = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.Column_Name = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.Column_Details = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.Column_DateTime = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.checkBox1 = new System.Windows.Forms.CheckBox();
            this.xButton1 = new PowerControl.XButton();
            this.xFlowLayoutPanel1 = new PowerControl.XFlowLayoutPanel();
            this.xButton2 = new PowerControl.XButton();
            this.xButton3 = new PowerControl.XButton();
            this.xPanel1.SuspendLayout();
            this.xGroupBox1.SuspendLayout();
            this.SuspendLayout();
            // 
            // xPanel1
            // 
            this.xPanel1.Controls.Add(this.xGroupBox1);
            this.xPanel1.Controls.Add(this.xButton3);
            this.xPanel1.Controls.Add(this.xButton2);
            this.xPanel1.Location = new System.Drawing.Point(391, 12);
            this.xPanel1.Name = "xPanel1";
            this.xPanel1.ShadowColor = System.Drawing.Color.FromArgb(((int)(((byte)(245)))), ((int)(((byte)(245)))), ((int)(((byte)(245)))));
            this.xPanel1.Size = new System.Drawing.Size(787, 345);
            this.xPanel1.TabIndex = 0;
            this.xPanel1.TopBorderColor = System.Drawing.Color.FromArgb(((int)(((byte)(224)))), ((int)(((byte)(224)))), ((int)(((byte)(224)))));
            // 
            // xGroupBox1
            // 
            this.xGroupBox1.Controls.Add(this.xDataGridView1);
            this.xGroupBox1.Location = new System.Drawing.Point(107, 56);
            this.xGroupBox1.Name = "xGroupBox1";
            this.xGroupBox1.Size = new System.Drawing.Size(531, 229);
            this.xGroupBox1.TabIndex = 2;
            this.xGroupBox1.TabStop = false;
            this.xGroupBox1.Text = "中文测试";
            this.xGroupBox1.TitleFont = new System.Drawing.Font("微软雅黑", 15F, System.Drawing.FontStyle.Bold);
            this.xGroupBox1.TitleMarkColor = System.Drawing.Color.FromArgb(((int)(((byte)(66)))), ((int)(((byte)(215)))), ((int)(((byte)(250)))));
            // 
            // xDataGridView1
            // 
            this.xDataGridView1.AlternatingRowsDefaultCellStyle = dataGridViewCellStyle13;
            this.xDataGridView1.BackgroundColor = System.Drawing.SystemColors.AppWorkspace;
            dataGridViewCellStyle14.Alignment = System.Windows.Forms.DataGridViewContentAlignment.MiddleLeft;
            dataGridViewCellStyle14.BackColor = System.Drawing.SystemColors.Control;
            dataGridViewCellStyle14.Font = new System.Drawing.Font("微软雅黑", 8F, System.Drawing.FontStyle.Bold);
            dataGridViewCellStyle14.ForeColor = System.Drawing.SystemColors.WindowText;
            dataGridViewCellStyle14.SelectionBackColor = System.Drawing.SystemColors.Highlight;
            dataGridViewCellStyle14.SelectionForeColor = System.Drawing.SystemColors.HighlightText;
            dataGridViewCellStyle14.WrapMode = System.Windows.Forms.DataGridViewTriState.True;
            this.xDataGridView1.ColumnHeadersDefaultCellStyle = dataGridViewCellStyle14;
            this.xDataGridView1.ColumnHeadersHeight = 24;
            this.xDataGridView1.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            this.xDataGridView1.ColumnHeadersVisible = true;
            this.xDataGridView1.Columns.AddRange(new System.Windows.Forms.DataGridViewColumn[] {
            this.Column_Id,
            this.Column_Name,
            this.Column_Details,
            this.Column_DateTime});
            dataGridViewCellStyle15.Alignment = System.Windows.Forms.DataGridViewContentAlignment.MiddleLeft;
            dataGridViewCellStyle15.BackColor = System.Drawing.SystemColors.Window;
            dataGridViewCellStyle15.Font = new System.Drawing.Font("微软雅黑", 8F, System.Drawing.FontStyle.Bold);
            dataGridViewCellStyle15.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(80)))), ((int)(((byte)(80)))), ((int)(((byte)(80)))));
            dataGridViewCellStyle15.SelectionBackColor = System.Drawing.SystemColors.Highlight;
            dataGridViewCellStyle15.SelectionForeColor = System.Drawing.SystemColors.HighlightText;
            dataGridViewCellStyle15.WrapMode = System.Windows.Forms.DataGridViewTriState.False;
            this.xDataGridView1.DefaultCellStyle = dataGridViewCellStyle15;
            this.xDataGridView1.GridColor = System.Drawing.SystemColors.ControlDark;
            this.xDataGridView1.Location = new System.Drawing.Point(6, 35);
            this.xDataGridView1.Margin = new System.Windows.Forms.Padding(3, 4, 3, 4);
            this.xDataGridView1.Name = "xDataGridView1";
            dataGridViewCellStyle16.Alignment = System.Windows.Forms.DataGridViewContentAlignment.MiddleLeft;
            dataGridViewCellStyle16.BackColor = System.Drawing.SystemColors.Control;
            dataGridViewCellStyle16.Font = new System.Drawing.Font("微软雅黑", 8F, System.Drawing.FontStyle.Bold);
            dataGridViewCellStyle16.ForeColor = System.Drawing.SystemColors.WindowText;
            dataGridViewCellStyle16.SelectionBackColor = System.Drawing.SystemColors.Highlight;
            dataGridViewCellStyle16.SelectionForeColor = System.Drawing.SystemColors.HighlightText;
            dataGridViewCellStyle16.WrapMode = System.Windows.Forms.DataGridViewTriState.True;
            this.xDataGridView1.RowHeadersDefaultCellStyle = dataGridViewCellStyle16;
            this.xDataGridView1.RowHeadersWidth = 41;
            this.xDataGridView1.RowTemplate.Height = 23;
            this.xDataGridView1.Size = new System.Drawing.Size(498, 136);
            this.xDataGridView1.TabIndex = 0;
            // 
            // Column_Id
            // 
            this.Column_Id.HeaderText = "编号";
            this.Column_Id.Name = "Column_Id";
            // 
            // Column_Name
            // 
            this.Column_Name.HeaderText = "姓名";
            this.Column_Name.Name = "Column_Name";
            // 
            // Column_Details
            // 
            this.Column_Details.HeaderText = "详细";
            this.Column_Details.Name = "Column_Details";
            // 
            // Column_DateTime
            // 
            this.Column_DateTime.HeaderText = "日期";
            this.Column_DateTime.Name = "Column_DateTime";
            // 
            // checkBox1
            // 
            this.checkBox1.AutoSize = true;
            this.checkBox1.Location = new System.Drawing.Point(227, 238);
            this.checkBox1.Name = "checkBox1";
            this.checkBox1.Size = new System.Drawing.Size(87, 20);
            this.checkBox1.TabIndex = 3;
            this.checkBox1.Text = "checkBox1";
            this.checkBox1.UseVisualStyleBackColor = true;
            // 
            // xButton1
            // 
            this.xButton1.BorderColor = System.Drawing.Color.FromArgb(((int)(((byte)(212)))), ((int)(((byte)(212)))), ((int)(((byte)(212)))));
            this.xButton1.CheckedEndColor = System.Drawing.Color.FromArgb(((int)(((byte)(83)))), ((int)(((byte)(128)))), ((int)(((byte)(252)))));
            this.xButton1.CheckedForeColor = System.Drawing.Color.Empty;
            this.xButton1.CheckedStartColor = System.Drawing.Color.FromArgb(((int)(((byte)(109)))), ((int)(((byte)(169)))), ((int)(((byte)(255)))));
            this.xButton1.DisplayStyle = PowerControl.XButtonDisplayStyle.ImageAndText;
            this.xButton1.EndColor = System.Drawing.Color.FromArgb(((int)(((byte)(232)))), ((int)(((byte)(232)))), ((int)(((byte)(232)))));
            this.xButton1.Font = new System.Drawing.Font("微软雅黑", 18F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Pixel);
            this.xButton1.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(134)))), ((int)(((byte)(134)))), ((int)(((byte)(134)))));
            this.xButton1.HoldingEndColor = System.Drawing.Color.FromArgb(((int)(((byte)(83)))), ((int)(((byte)(128)))), ((int)(((byte)(252)))));
            this.xButton1.HoldingForeColor = System.Drawing.Color.White;
            this.xButton1.HoldingImage = global::ControlTest.Properties.Resources.核素管理2;
            this.xButton1.HoldingStartColor = System.Drawing.Color.FromArgb(((int)(((byte)(109)))), ((int)(((byte)(169)))), ((int)(((byte)(255)))));
            this.xButton1.Image = global::ControlTest.Properties.Resources.核素管理1;
            this.xButton1.Location = new System.Drawing.Point(139, 68);
            this.xButton1.Name = "xButton1";
            this.xButton1.Size = new System.Drawing.Size(150, 35);
            this.xButton1.StartColor = System.Drawing.Color.White;
            this.xButton1.TabIndex = 1;
            this.xButton1.Text = "format C:";
            this.xButton1.Click += new System.EventHandler(this.xButton1_Click);
            // 
            // xFlowLayoutPanel1
            // 
            this.xFlowLayoutPanel1.Location = new System.Drawing.Point(9, 360);
            this.xFlowLayoutPanel1.Margin = new System.Windows.Forms.Padding(0);
            this.xFlowLayoutPanel1.Name = "xFlowLayoutPanel1";
            this.xFlowLayoutPanel1.Size = new System.Drawing.Size(1256, 434);
            this.xFlowLayoutPanel1.TabIndex = 4;
            // 
            // xButton2
            // 
            this.xButton2.BorderColor = System.Drawing.Color.FromArgb(((int)(((byte)(212)))), ((int)(((byte)(212)))), ((int)(((byte)(212)))));
            this.xButton2.CheckedEndColor = System.Drawing.Color.FromArgb(((int)(((byte)(83)))), ((int)(((byte)(128)))), ((int)(((byte)(252)))));
            this.xButton2.CheckedForeColor = System.Drawing.Color.Empty;
            this.xButton2.CheckedStartColor = System.Drawing.Color.FromArgb(((int)(((byte)(109)))), ((int)(((byte)(169)))), ((int)(((byte)(255)))));
            this.xButton2.EndColor = System.Drawing.Color.FromArgb(((int)(((byte)(232)))), ((int)(((byte)(232)))), ((int)(((byte)(232)))));
            this.xButton2.Font = new System.Drawing.Font("微软雅黑", 18F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Pixel);
            this.xButton2.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(134)))), ((int)(((byte)(134)))), ((int)(((byte)(134)))));
            this.xButton2.HoldingEndColor = System.Drawing.Color.FromArgb(((int)(((byte)(83)))), ((int)(((byte)(128)))), ((int)(((byte)(252)))));
            this.xButton2.HoldingForeColor = System.Drawing.Color.White;
            this.xButton2.HoldingImage = global::ControlTest.Properties.Resources.核素管理2;
            this.xButton2.HoldingStartColor = System.Drawing.Color.FromArgb(((int)(((byte)(109)))), ((int)(((byte)(169)))), ((int)(((byte)(255)))));
            this.xButton2.Image = global::ControlTest.Properties.Resources.核素管理1;
            this.xButton2.Location = new System.Drawing.Point(96, 280);
            this.xButton2.Name = "xButton2";
            this.xButton2.Size = new System.Drawing.Size(150, 35);
            this.xButton2.StartColor = System.Drawing.Color.White;
            this.xButton2.TabIndex = 1;
            this.xButton2.Text = "+";
            this.xButton2.Click += new System.EventHandler(this.xButton2_Click);
            // 
            // xButton3
            // 
            this.xButton3.BorderColor = System.Drawing.Color.FromArgb(((int)(((byte)(212)))), ((int)(((byte)(212)))), ((int)(((byte)(212)))));
            this.xButton3.CheckedEndColor = System.Drawing.Color.FromArgb(((int)(((byte)(83)))), ((int)(((byte)(128)))), ((int)(((byte)(252)))));
            this.xButton3.CheckedForeColor = System.Drawing.Color.Empty;
            this.xButton3.CheckedStartColor = System.Drawing.Color.FromArgb(((int)(((byte)(109)))), ((int)(((byte)(169)))), ((int)(((byte)(255)))));
            this.xButton3.EndColor = System.Drawing.Color.FromArgb(((int)(((byte)(232)))), ((int)(((byte)(232)))), ((int)(((byte)(232)))));
            this.xButton3.Font = new System.Drawing.Font("微软雅黑", 18F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Pixel);
            this.xButton3.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(134)))), ((int)(((byte)(134)))), ((int)(((byte)(134)))));
            this.xButton3.HoldingEndColor = System.Drawing.Color.FromArgb(((int)(((byte)(83)))), ((int)(((byte)(128)))), ((int)(((byte)(252)))));
            this.xButton3.HoldingForeColor = System.Drawing.Color.White;
            this.xButton3.HoldingImage = global::ControlTest.Properties.Resources.核素管理2;
            this.xButton3.HoldingStartColor = System.Drawing.Color.FromArgb(((int)(((byte)(109)))), ((int)(((byte)(169)))), ((int)(((byte)(255)))));
            this.xButton3.Image = global::ControlTest.Properties.Resources.核素管理1;
            this.xButton3.Location = new System.Drawing.Point(312, 280);
            this.xButton3.Name = "xButton3";
            this.xButton3.Size = new System.Drawing.Size(150, 35);
            this.xButton3.StartColor = System.Drawing.Color.White;
            this.xButton3.TabIndex = 1;
            this.xButton3.Text = "-";
            this.xButton3.Click += new System.EventHandler(this.xButton3_Click);
            // 
            // FrmShadowPanelTest
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(7F, 16F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(1351, 803);
            this.Controls.Add(this.xFlowLayoutPanel1);
            this.Controls.Add(this.checkBox1);
            this.Controls.Add(this.xButton1);
            this.Controls.Add(this.xPanel1);
            this.Location = new System.Drawing.Point(0, 0);
            this.Name = "FrmShadowPanelTest";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
            this.Text = "FrmShadowPanelTest";
            this.Load += new System.EventHandler(this.FrmShadowPanelTest_Load);
            this.xPanel1.ResumeLayout(false);
            this.xGroupBox1.ResumeLayout(false);
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private PowerControl.XShadowPanel xPanel1;
        private PowerControl.XGroupBox xGroupBox1;
        private PowerControl.XButton xButton1;
        private System.Windows.Forms.CheckBox checkBox1;
        private PowerControl.XFlowLayoutPanel xFlowLayoutPanel1;
        private PowerControl.XDataGridView xDataGridView1;
        private System.Windows.Forms.DataGridViewTextBoxColumn Column_Id;
        private System.Windows.Forms.DataGridViewTextBoxColumn Column_Name;
        private System.Windows.Forms.DataGridViewTextBoxColumn Column_Details;
        private System.Windows.Forms.DataGridViewTextBoxColumn Column_DateTime;
        private PowerControl.XButton xButton3;
        private PowerControl.XButton xButton2;
    }
}