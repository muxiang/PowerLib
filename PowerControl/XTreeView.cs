using System;
using System.ComponentModel;
using System.Drawing;
using System.Drawing.Drawing2D;
using System.Linq;
using System.Windows.Forms;
using PowerControl.Properties;

namespace PowerControl
{
    /// <summary>
    /// 表示一个树形菜单
    /// </summary>
    public sealed partial class XTreeView : TreeView
    {
        // 选择项背景色
        private Color _selectedBackColorFocused = Color.FromArgb(0, 122, 204);
        private Color _selectedBackColorUnFocused = Color.FromArgb(204, 206, 219);

        // 选择项前景色
        private Color _selectedForeColorFocused = Color.FromArgb(255, 255, 255);
        private Color _selectedForeColorUnFocused = Color.FromArgb(0, 0, 0);

        /// <summary>
        /// 初始化<see cref="XTreeView"/>的实例
        /// </summary>
        public XTreeView()
        {
            InitializeComponent();

            DrawMode = TreeViewDrawMode.OwnerDrawAll;
        }

        /// <summary>
        /// 选择项焦点背景色
        /// </summary>
        [Browsable(true)]
        [Category("Appearance")]
        public Color SelectedBackColorFocused
        {
            get => _selectedBackColorFocused;
            set
            {
                _selectedBackColorFocused = value;
                Invalidate();
            }
        }

        /// <summary>
        /// 选择项非焦点背景色
        /// </summary>
        [Browsable(true)]
        [Category("Appearance")]
        public Color SelectedBackColorUnFocused
        {
            get => _selectedBackColorUnFocused;
            set
            {
                _selectedBackColorUnFocused = value;
                Invalidate();
            }
        }

        /// <summary>
        /// 选择项焦点前景色
        /// </summary>
        [Browsable(true)]
        [Category("Appearance")]
        public Color SelectedForeColorFocused
        {
            get => _selectedForeColorFocused;
            set
            {
                _selectedForeColorFocused = value;
                Invalidate();
            }
        }

        /// <summary>
        /// 选择项非焦点前景色
        /// </summary>
        [Browsable(true)]
        [Category("Appearance")]
        public Color SelectedForeColorUnFocused
        {
            get => _selectedForeColorUnFocused;
            set
            {
                _selectedForeColorUnFocused = value;
                Invalidate();
            }
        }

        /// <summary>
        /// 判定坐标是否在节点中，返回节点或其子节点
        /// </summary>
        /// <param name="pt">坐标</param>
        /// <param name="node">节点</param>
        /// <returns></returns>
        private static TreeNode InNode(Point pt, TreeNode node)
        {
            if (pt.Y > node.Bounds.Top && pt.Y < node.Bounds.Bottom && pt.X > node.Bounds.X)
                return node;

            foreach (TreeNode child in node.Nodes)
            {
                TreeNode result = InNode(pt, child);
                if (result != null)
                    return result;
            }

            return null;
        }

        protected override void OnDrawNode(DrawTreeNodeEventArgs e)
        {
            base.OnDrawNode(e);

            if (e.Bounds == default) return;

            e.Graphics.CompositingQuality = CompositingQuality.HighQuality;
            e.Graphics.SmoothingMode = SmoothingMode.AntiAlias;
            e.Graphics.PixelOffsetMode = PixelOffsetMode.HighQuality;

            using Brush brsBack = new SolidBrush(BackColor);
            using Brush brsFore = new SolidBrush(ForeColor);
            using Pen penFore = new Pen(ForeColor);
            using Brush brsSelectedBack = new SolidBrush(Focused ? _selectedBackColorFocused : _selectedBackColorUnFocused);
            using Brush brsSelectedFore = new SolidBrush(Focused ? _selectedForeColorFocused : _selectedForeColorUnFocused);
            using Pen penSelectedFore = new Pen(Focused ? _selectedForeColorFocused : _selectedForeColorUnFocused);

            e.Graphics.FillRectangle(brsBack, e.Bounds);

            // 绘制节点背景色
            if (e.Node.IsSelected)
                e.Graphics.FillRectangle(brsSelectedBack, e.Bounds);

            // 绘制节点文本
            e.Graphics.DrawString(e.Node.Text, Font, e.Node.IsSelected ? brsSelectedFore : brsFore, e.Node.Bounds,
                StringFormat.GenericTypographic);

            float fontHeight = Font.GetHeight(e.Graphics);

            // 绘制节点折叠图标
            int x = (int)(e.Node.Bounds.X - fontHeight), y = e.Node.Bounds.Y;

            if (e.Node.Nodes.Count <= 0)
                return;

            e.Graphics.DrawIconUnstretched(e.Node.IsExpanded ? Resources.tree_collapse : Resources.tree_expand, new Rectangle(x, y, Resources.tree_expand.Width, Resources.tree_expand.Height));

            //PointF[] trianglePts = new PointF[3];
            //if (e.Node.IsExpanded)
            //{
            //    // 左下
            //    trianglePts[0] = new PointF(x + fontHeight / 5, y + fontHeight / 5 * 3);
            //    // 右下
            //    trianglePts[1] = new PointF(x + fontHeight / 5 * 3, y + fontHeight / 5 * 3);
            //    // 右上
            //    trianglePts[2] = new PointF(x + fontHeight / 5 * 3, y + fontHeight / 5);

            //    e.Graphics.FillPolygon(e.Node.IsSelected ? brsSelectedFore : brsFore, trianglePts);
            //}
            //else
            //{
            //    // 左上
            //    trianglePts[0] = new PointF(x + fontHeight / 4, y + fontHeight / 5);
            //    // 左下
            //    trianglePts[1] = new PointF(x + fontHeight / 4, y + fontHeight / 5 * 4);
            //    // 右
            //    trianglePts[2] = new PointF(x + fontHeight * .5f, y + fontHeight / 2);

            //    e.Graphics.DrawPolygon(e.Node.IsSelected ? penSelectedFore : penFore, trianglePts);
            //}
        }

        protected override void OnAfterSelect(TreeViewEventArgs e)
        {
            base.OnAfterSelect(e);
            Invalidate();
        }

        protected override void OnPaint(PaintEventArgs pe)
        {
            using (Brush brs = new SolidBrush(BackColor))
                pe.Graphics.FillRectangle(brs, ClientRectangle);

            base.OnPaint(pe);
        }

        protected override void OnMouseDown(MouseEventArgs e)
        {
            base.OnMouseDown(e);

            foreach (TreeNode node in Nodes)
            {
                TreeNode clicked = InNode(e.Location, node);
                if (clicked == null) continue;
                SelectedNode = clicked;
                return;
            }
        }

        protected override void OnMouseDoubleClick(MouseEventArgs e)
        {
            base.OnMouseDoubleClick(e);

            foreach (TreeNode node in Nodes)
            {
                TreeNode clicked = InNode(e.Location, node);
                if (clicked == null) continue;

                // 1px偏差
                Rectangle rectRealBounds = new Rectangle(clicked.Bounds.X, clicked.Bounds.Y, clicked.Bounds.Width + 1, clicked.Bounds.Height);
                if (rectRealBounds.Contains(e.Location))
                    return;

                if (clicked.IsExpanded)
                    clicked.Collapse();
                else
                    clicked.Expand();

                return;
            }
        }
    }
}
