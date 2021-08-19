namespace PowerLib.Winform.Controls
{
    partial class XDataGridView
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(XDataGridView));
            this.pnlOuter = new System.Windows.Forms.Panel();
            this.dataGridView1 = new System.Windows.Forms.DataGridView();
            this.xScrollBar1 = new PowerLib.Winform.Controls.XScrollBar();
            this.pnlOuter.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.dataGridView1)).BeginInit();
            this.SuspendLayout();
            //  
            //  pnlOuter
            //  
            this.pnlOuter.Controls.Add(this.dataGridView1);
            this.pnlOuter.Dock = System.Windows.Forms.DockStyle.Fill;
            this.pnlOuter.Location = new System.Drawing.Point(0, 0);
            this.pnlOuter.Margin = new System.Windows.Forms.Padding(0);
            this.pnlOuter.Name = "pnlOuter";
            this.pnlOuter.Size = new System.Drawing.Size(370, 164);
            this.pnlOuter.TabIndex = 1;
            //  
            //  dataGridView1
            //  
            this.dataGridView1.AllowUserToAddRows = false;
            this.dataGridView1.AllowUserToDeleteRows = false;
            this.dataGridView1.AllowUserToResizeColumns = false;
            this.dataGridView1.AllowUserToResizeRows = false;
            this.dataGridView1.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            this.dataGridView1.Dock = System.Windows.Forms.DockStyle.Left;
            this.dataGridView1.Location = new System.Drawing.Point(0, 0);
            this.dataGridView1.Margin = new System.Windows.Forms.Padding(0);
            this.dataGridView1.Name = "dataGridView1";
            this.dataGridView1.RowTemplate.Height = 23;
            this.dataGridView1.Size = new System.Drawing.Size(265, 164);
            this.dataGridView1.TabIndex = 0;
            //  
            //  xScrollBar1
            //  
            this.xScrollBar1.ChannelColor = System.Drawing.Color.FromArgb(((int)(((byte)(212)))), ((int)(((byte)(212)))), ((int)(((byte)(212)))));
            this.xScrollBar1.Dock = System.Windows.Forms.DockStyle.Right;
            this.xScrollBar1.DownArrowImage = ((System.Drawing.Image)(resources.GetObject("xScrollBar1.DownArrowImage")));
            this.xScrollBar1.LargeChange = 10;
            this.xScrollBar1.Location = new System.Drawing.Point(370, 0);
            this.xScrollBar1.Maximum = 100;
            this.xScrollBar1.Minimum = 0;
            this.xScrollBar1.MinimumSize = new System.Drawing.Size(15, 92);
            this.xScrollBar1.Name = "xScrollBar1";
            this.xScrollBar1.Size = new System.Drawing.Size(15, 164);
            this.xScrollBar1.SmallChange = 1;
            this.xScrollBar1.TabIndex = 2;
            this.xScrollBar1.Text = "xScrollBar1";
            this.xScrollBar1.ThumbBottomImage = ((System.Drawing.Image)(resources.GetObject("xScrollBar1.ThumbBottomImage")));
            this.xScrollBar1.ThumbBottomSpanImage = ((System.Drawing.Image)(resources.GetObject("xScrollBar1.ThumbBottomSpanImage")));
            this.xScrollBar1.ThumbMiddleImage = ((System.Drawing.Image)(resources.GetObject("xScrollBar1.ThumbMiddleImage")));
            this.xScrollBar1.ThumbTopImage = ((System.Drawing.Image)(resources.GetObject("xScrollBar1.ThumbTopImage")));
            this.xScrollBar1.ThumbTopSpanImage = ((System.Drawing.Image)(resources.GetObject("xScrollBar1.ThumbTopSpanImage")));
            this.xScrollBar1.UpArrowImage = ((System.Drawing.Image)(resources.GetObject("xScrollBar1.UpArrowImage")));
            this.xScrollBar1.Value = 0;
            //  
            //  XDataGridView
            //  
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 12F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.Controls.Add(this.pnlOuter);
            this.Controls.Add(this.xScrollBar1);
            this.Name = "XDataGridView";
            this.Size = new System.Drawing.Size(385, 164);
            this.pnlOuter.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.dataGridView1)).EndInit();
            this.ResumeLayout(false);

        }

        #endregion
        private System.Windows.Forms.Panel pnlOuter;
        private System.Windows.Forms.DataGridView dataGridView1;
        private XScrollBar xScrollBar1;
    }
}
