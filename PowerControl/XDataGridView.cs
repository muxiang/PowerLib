using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace PowerControl
{
    public partial class XDataGridView : DataGridView
    {
        /// <summary>
        /// 缺省表格列头样式
        /// </summary>
        private static readonly DataGridViewCellStyle DefaultTableHeaderStyle = new DataGridViewCellStyle
        {
            Alignment = DataGridViewContentAlignment.MiddleLeft,
            Font = new Font("微软雅黑", 8F, FontStyle.Bold, GraphicsUnit.Point, 134),
            BackColor = Color.FromArgb(249, 249, 251),
            ForeColor = Color.Black,
            SelectionBackColor = Color.FromArgb(249, 249, 251),
            SelectionForeColor = Color.Black,
            WrapMode = DataGridViewTriState.True,
        };

        /// <summary>
        /// 缺省表格单元格样式
        /// </summary>
        private static readonly DataGridViewCellStyle DefaultTableCellStyle = new DataGridViewCellStyle
        {
            Alignment = DataGridViewContentAlignment.MiddleLeft,
            Font = new Font("微软雅黑", 8F, FontStyle.Bold, GraphicsUnit.Point, 134),
            BackColor = Color.FromArgb(255, 255, 255),
            ForeColor = Color.Black,
            SelectionBackColor = Color.FromArgb(115, 180, 247),
            SelectionForeColor = Color.White,
            WrapMode = DataGridViewTriState.False,
        };

        public XDataGridView()
        {
            InitializeComponent();

            ColumnHeadersDefaultCellStyle = DefaultTableHeaderStyle;
            DefaultCellStyle = DefaultTableCellStyle;
            BackgroundColor = Color.White;
            RowHeadersVisible = false;
            GridColor = Color.FromArgb(233, 233, 235);
            SelectionMode = DataGridViewSelectionMode.FullRowSelect;
            AllowUserToResizeRows = false;
            EnableHeadersVisualStyles = false;
        }

        protected override void OnPaint(PaintEventArgs pe)
        {
            base.OnPaint(pe);
        }
    }
}
