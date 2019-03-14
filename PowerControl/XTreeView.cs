using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Diagnostics;
using System.Drawing;
using System.Drawing.Drawing2D;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace PowerControl
{
    /// <summary>
    /// 表示一个树形菜单
    /// </summary>
    public sealed partial class XTreeView : TreeView
    {
        //前景画刷
        private Brush _brsFore;
        //背景画刷
        private Brush _brsBack;

        //选择项背景色
        private Color _selectedBackColor = Color.FromArgb(0, 150, 170);

        /// <summary>
        /// 初始化<see cref="XTreeView"/>的实例
        /// </summary>
        public XTreeView()
        {
            InitializeComponent();

            DrawMode = TreeViewDrawMode.OwnerDrawAll;
            _brsFore = new SolidBrush(ForeColor);
            _brsBack = new SolidBrush(BackColor);
        }

        /// <summary>
        /// 选择项背景色
        /// </summary>
        [Browsable(true)]
        [Category("Appearance")]
        public Color SelectedBackColor
        {
            get => _selectedBackColor;
            set
            {
                _selectedBackColor = value;
                Invalidate();
            }
        }

        private static bool IsChildSelected(TreeNode tn)
        {
            return tn.Nodes.Count != 0 && tn.Nodes.OfType<TreeNode>().Any(child => child.IsSelected || IsChildSelected(child));
        }

        protected override void OnForeColorChanged(EventArgs e)
        {
            base.OnForeColorChanged(e);

            _brsFore = new SolidBrush(ForeColor);
        }

        protected override void OnBackColorChanged(EventArgs e)
        {
            base.OnBackColorChanged(e);

            _brsBack = new SolidBrush(BackColor);
        }

        protected override void OnDrawNode(DrawTreeNodeEventArgs e)
        {
            base.OnDrawNode(e);

            if (e.Bounds == default) return;

            e.Graphics.CompositingQuality = CompositingQuality.HighQuality;
            e.Graphics.SmoothingMode = SmoothingMode.HighQuality;
            e.Graphics.PixelOffsetMode = PixelOffsetMode.HighQuality;

            e.Graphics.FillRectangle(_brsBack, e.Bounds);

            //绘制节点背景色
            if (e.Node.IsSelected || IsChildSelected(e.Node))
            {
                LinearGradientBrush brs = new LinearGradientBrush(
                    new PointF(e.Bounds.X, e.Bounds.Y + e.Bounds.Height / 2),
                    new PointF(e.Bounds.X + e.Bounds.Width, e.Bounds.Y + e.Bounds.Height / 2),
                    Color.Transparent, _selectedBackColor
                )
                {
                    Blend = new Blend
                    {
                        Positions = new[] { 0, .5f, 1 },
                        Factors = new[] { 0, 1f, 0 }
                    }
                };

                e.Graphics.FillRectangle(brs, e.Bounds);
            }

            float fontHeight = Font.GetHeight(e.Graphics);

            //绘制节点折叠图标
            int x = (int)(e.Node.Bounds.X - fontHeight), y = e.Node.Bounds.Y;

            if (e.Node.Nodes.Count > 0)
            {
                PointF[] trianglePts = new PointF[3];
                if (e.Node.IsExpanded)
                {
                    trianglePts[0] = new PointF(x + fontHeight / 2 / 2, y + fontHeight / 2 / 2);
                    trianglePts[1] = new PointF(x + fontHeight * .75f, y + fontHeight / 2 / 2);
                    trianglePts[2] = new PointF(x + fontHeight / 2, y + fontHeight * .75f);
                }
                else
                {
                    trianglePts[0] = new PointF(x + fontHeight / 2 / 2, y + fontHeight / 2 / 2);
                    trianglePts[1] = new PointF(x + fontHeight / 2 / 2, y + fontHeight * .75f);
                    trianglePts[2] = new PointF(x + fontHeight * .75f, y + fontHeight / 2);
                }

                e.Graphics.FillPolygon(_brsFore, trianglePts);
            }

            e.Graphics.DrawString(e.Node.Text, Font, _brsFore, e.Node.Bounds, StringFormat.GenericTypographic);
        }

        protected override void OnAfterSelect(TreeViewEventArgs e)
        {
            base.OnAfterSelect(e);
            Invalidate();
        }

        protected override void OnPaint(PaintEventArgs pe)
        {
            pe.Graphics.FillRectangle(_brsBack, ClientRectangle);
            base.OnPaint(pe);
        }
    }
}
