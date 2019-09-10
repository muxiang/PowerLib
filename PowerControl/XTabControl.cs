using System;
using System.ComponentModel;
using System.Drawing;
using System.Drawing.Drawing2D;
using System.Runtime.InteropServices;
using System.Windows.Forms;

namespace PowerControl
{
    public partial class XTabControl : TabControl
    {
        private Color _backColor = Color.FromArgb(231, 231, 231);
        private Color _borderColor = Color.FromArgb(215, 215, 215);

        private Color _headerBackColorStart = Color.FromArgb(255, 255, 255);
        private Color _headerBackColorEnd = Color.FromArgb(227, 227, 227);
        private Color _headerForeColor = Color.FromArgb(80, 80, 80);
        private Color _headerSelectedBackColorStart = Color.FromArgb(109, 169, 255);
        private Color _headerSelectedBackColorEnd = Color.FromArgb(83, 128, 252);
        private Color _headerSelectedForeColor = Color.FromArgb(255, 255, 255);

        public XTabControl()
        {
            SetStyle(ControlStyles.UserPaint
                     | ControlStyles.DoubleBuffer
                     | ControlStyles.OptimizedDoubleBuffer
                     | ControlStyles.AllPaintingInWmPaint
                     | ControlStyles.ResizeRedraw
                     | ControlStyles.SupportsTransparentBackColor, true);
        }

        [Browsable(true)]
        [EditorBrowsable(EditorBrowsableState.Always)]
        public override Color BackColor
        {
            get => _backColor;
            set
            {
                _backColor = value;
                Invalidate(true);
            }
        }

        public Color BorderColor
        {
            get => _borderColor;
            set
            {
                _borderColor = value;
                Invalidate(true);
            }
        }

        public Color HeaderSelectedBackColorStart
        {
            get => _headerSelectedBackColorStart;
            set
            {
                _headerSelectedBackColorStart = value;
                Invalidate();
            }
        }

        public Color HeaderSelectedBackColorEnd
        {
            get => _headerSelectedBackColorEnd;
            set
            {
                _headerSelectedBackColorEnd = value;
                Invalidate();
            }
        }

        public Color HeaderSelectedForeColor
        {
            get => _headerSelectedForeColor;
            set
            {
                _headerSelectedForeColor = value;
                Invalidate();
            }
        }

        public Color HeaderBackColorStart
        {
            get => _headerBackColorStart;
            set
            {
                _headerBackColorStart = value;
                Invalidate();
            }
        }

        public Color HeaderBackColorEnd
        {
            get => _headerBackColorEnd;
            set
            {
                _headerBackColorEnd = value;
                Invalidate();
            }
        }

        public Color HeaderForeColor
        {
            get => _headerForeColor;
            set
            {
                _headerForeColor = value;
                Invalidate();
            }
        }

        protected override void OnPaintBackground(PaintEventArgs pe)
        {
            if (DesignMode)
            {
                LinearGradientBrush backBrush = new LinearGradientBrush(
                    Bounds,
                    SystemColors.ControlLightLight,
                    SystemColors.ControlLight,
                    LinearGradientMode.Vertical);
                pe.Graphics.FillRectangle(backBrush, Bounds);
                backBrush.Dispose();
            }
            else
                PaintTransparentBackground(pe.Graphics, ClientRectangle);
        }

        /// <summary>
        ///  TabContorl 背景色设置
        /// </summary>
        /// <param name="g"></param>
        /// <param name="clipRect"></param>
        protected void PaintTransparentBackground(Graphics g, Rectangle clipRect)
        {
            if ((Parent != null))
            {
                clipRect.Offset(Location);
                PaintEventArgs e = new PaintEventArgs(g, clipRect);
                GraphicsState state = g.Save();
                g.SmoothingMode = SmoothingMode.AntiAlias;
                try
                {
                    g.TranslateTransform(-Location.X, -Location.Y);
                    InvokePaintBackground(Parent, e);
                    InvokePaint(Parent, e);
                }
                finally
                {
                    g.Restore(state);
                    clipRect.Offset(-Location.X, -Location.Y);
                    //新加片段,待测试
                    using (SolidBrush brush = new SolidBrush(_backColor))
                    {
                        clipRect.Inflate(1, 1);
                        g.FillRectangle(brush, clipRect);
                    }
                }
            }
            else
            {
                LinearGradientBrush backBrush = new LinearGradientBrush(Bounds, SystemColors.ControlLightLight, SystemColors.ControlLight, LinearGradientMode.Vertical);
                g.FillRectangle(backBrush, Bounds);
                backBrush.Dispose();
            }
        }

        protected override void OnPaint(PaintEventArgs e)
        {
            base.OnPaint(e);
            PaintTransparentBackground(e.Graphics, ClientRectangle);
            PaintAllTheTabs(e);
            PaintTheTabPageBorder(e);
            PaintTheSelectedTab(e);
        }

        private void PaintAllTheTabs(PaintEventArgs e)
        {
            if (TabCount > 0)
                for (int index = 0; index < TabCount; index++)
                    PaintTab(e, index);
        }

        private void PaintTab(PaintEventArgs e, int index)
        {
            GraphicsPath path = GetPath(index);
            PaintTabBackground(e.Graphics, index, path);
            PaintTabBorder(e.Graphics, index, path);
            PaintTabText(e.Graphics, index);
            PaintTabImage(e.Graphics, index);
        }

        /// <summary>
        /// 设置选项卡头部颜色
        /// </summary>
        /// <param name="graph"></param>
        /// <param name="index"></param>
        /// <param name="path"></param>
        private void PaintTabBackground(Graphics graph, int index, GraphicsPath path)
        {
            Rectangle rect = GetTabRect(index);
            Brush buttonBrush = new LinearGradientBrush(rect, _headerBackColorStart, _headerBackColorEnd, LinearGradientMode.Vertical);

            if (index == SelectedIndex)
                buttonBrush = new LinearGradientBrush(rect, _headerSelectedBackColorStart, _headerSelectedBackColorEnd, LinearGradientMode.Vertical);
            graph.FillPath(buttonBrush, path);
            buttonBrush.Dispose();
        }

        /// <summary>
        /// 设置选项卡头部边框色
        /// </summary>
        /// <param name="graph"></param>
        /// <param name="index"></param>
        /// <param name="path"></param>
        private void PaintTabBorder(Graphics g, int index, GraphicsPath path)
        {
            Pen borderPen = new Pen(_borderColor);// TabPage 非选中时候的 TabPage 头部边框色
            /*if (index == SelectedIndex)
                borderPen = new Pen(_borderColor); // TabPage 选中后的 TabPage 头部边框色*/
            g.DrawPath(borderPen, path);
            borderPen.Dispose();
        }

        private void PaintTabImage(Graphics g, int index)
        {
            Image tabImage = null;
            if (TabPages[index].ImageIndex > -1 && ImageList != null)
                tabImage = ImageList.Images[TabPages[index].ImageIndex];
            else if (TabPages[index].ImageKey.Trim().Length > 0 && ImageList != null)
                tabImage = ImageList.Images[TabPages[index].ImageKey];
            if (tabImage != null)
            {
                Rectangle rect = GetTabRect(index);
                g.DrawImage(tabImage, rect.Right - rect.Height - 4, 4, rect.Height - 2, rect.Height - 2);
            }
        }

        private void PaintTabText(Graphics graph, int index)
        {
            Rectangle rect = GetTabRect(index);

            Rectangle rect2 = new Rectangle(rect.Left, rect.Top, rect.Width, rect.Height);

            string tabtext = TabPages[index].Text;

            StringFormat format = new StringFormat
            {
                Alignment = StringAlignment.Near,
                LineAlignment = StringAlignment.Center,
                Trimming = StringTrimming.EllipsisCharacter
            };

            Brush forebrush;

            forebrush = TabPages[index].Enabled
                ? (index == SelectedIndex ? new SolidBrush(_headerSelectedForeColor) : new SolidBrush(_headerForeColor))
                : SystemBrushes.ControlDark;

            Font tabFont = Font;
            if (index == SelectedIndex)
                tabFont = new Font(Font, FontStyle.Bold);
            graph.DrawString(tabtext, tabFont, forebrush, rect2, format);
        }

        /// <summary>
        /// 设置 TabPage 内容页边框色
        /// </summary>
        /// <param name="e"></param>
        private void PaintTheTabPageBorder(PaintEventArgs e)
        {
            if (TabCount > 0)
            {
                Rectangle borderRect = TabPages[0].Bounds;
                borderRect.Inflate(1, 1);
                //ControlPaint.DrawBorder(e.Graphics, borderRect, ThemedColors.ToolBorder, ButtonBorderStyle.Solid);
                ControlPaint.DrawBorder(e.Graphics, borderRect, BorderColor, ButtonBorderStyle.Solid);
            }
        }

        /// <summary>
        /// // TabPage 页头部间隔色
        /// </summary>
        /// <param name="e"></param>
        private void PaintTheSelectedTab(PaintEventArgs e)
        {
            Rectangle selrect;
            int selrectRight = 0;

            switch (SelectedIndex)
            {
                case -1:
                    break;
                case 0:
                    selrect = GetTabRect(SelectedIndex);
                    selrectRight = selrect.Right;
                    e.Graphics.DrawLine(SystemPens.ControlLightLight, selrect.Left + 2, selrect.Bottom + 1, selrectRight - 2, selrect.Bottom + 1);
                    break;
                default:
                    selrect = GetTabRect(SelectedIndex);
                    selrectRight = selrect.Right;
                    e.Graphics.DrawLine(SystemPens.ControlLightLight, selrect.Left + 6 - selrect.Height, selrect.Bottom + 2, selrectRight - 2, selrect.Bottom + 2);
                    break;
            }
        }

        private GraphicsPath GetPath(int index)
        {
            GraphicsPath path = new GraphicsPath();
            path.Reset();

            Rectangle rect = GetTabRect(index);

            switch (Alignment)
            {
                case TabAlignment.Top:

                    break;
                case TabAlignment.Bottom:

                    break;
                case TabAlignment.Left:

                    break;
                case TabAlignment.Right:

                    break;
            }

            if (index == 0)
            {
                path.AddLine(rect.Left - 1, rect.Top + 1, rect.Left - 1, rect.Top + 1);
                path.AddLine(rect.Left - 1, rect.Top + 1, rect.Right, rect.Top + 1);
                path.AddLine(rect.Right, rect.Top + 1, rect.Right, rect.Bottom);
                path.AddLine(rect.Right, rect.Bottom, rect.Left - 1, rect.Bottom);
            }
            else
            {
                if (index == SelectedIndex)
                {
                    path.AddLine(rect.Left - 1, rect.Top, rect.Left - 1, rect.Top);
                    path.AddLine(rect.Left - 1, rect.Top, rect.Right, rect.Top);
                    path.AddLine(rect.Right, rect.Top, rect.Right, rect.Bottom);
                    path.AddLine(rect.Right, rect.Bottom + 1, rect.Left - 1, rect.Bottom + 1);
                }
                else
                {
                    path.AddLine(rect.Left - 1, rect.Top, rect.Left - 1, rect.Top);
                    path.AddLine(rect.Left - 1, rect.Top, rect.Right, rect.Top);
                    path.AddLine(rect.Right, rect.Top, rect.Right, rect.Bottom);
                    path.AddLine(rect.Right, rect.Bottom + 1, rect.Left - 1, rect.Bottom + 1);
                }
            }
            return path;
        }

        [DllImport("user32.dll")]
        private static extern IntPtr SendMessage(IntPtr hWnd, int Msg, IntPtr wParam, IntPtr lParam);

        private const int WM_SETFONT = 0x30;
        private const int WM_FONTCHANGE = 0x1d;

        protected override void OnCreateControl()
        {
            base.OnCreateControl();
            OnFontChanged(EventArgs.Empty);
        }

        protected override void OnFontChanged(EventArgs e)
        {
            base.OnFontChanged(e);
            IntPtr hFont = Font.ToHfont();
            SendMessage(Handle, WM_SETFONT, hFont, (IntPtr)(-1));
            SendMessage(Handle, WM_FONTCHANGE, IntPtr.Zero, IntPtr.Zero);
            UpdateStyles();
            //this.ItemSize = new Size(0, this.Font.Height + 2);
        }
    }
}
