using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Diagnostics;
using System.Drawing;
using System.Drawing.Drawing2D;
using System.Drawing.Printing;
using System.Linq;
using System.Text;
using System.Windows.Forms;

using static PowerControl.NativeMethods;
using static PowerControl.NativeConstants;

namespace PowerControl
{
    /// <summary>
    /// 表示用户界面窗口
    /// </summary>
    public class XForm : Form
    {
        #region 常量

        // 边框宽度
        private const int BorderWidth = 4;
        // 标题栏高度
        private const int TitleBarHeight = 30;

        #endregion 常量

        #region 字段

        // 阴影背景
        private XFormBackground _backWindow;

        private FormBorderStyle _formBorderStyle = FormBorderStyle.Sizable;
        private Color _titleBarStartColor = Color.FromArgb(89, 98, 255);
        private Color _titleBarEndColor = Color.FromArgb(130, 101, 255);

        #endregion 字段

        /// <summary>
        /// 初始化用户界面窗口
        /// </summary>
        public XForm()
        {
            base.FormBorderStyle = FormBorderStyle.None;
            SetStyle(ControlStyles.DoubleBuffer
                     | ControlStyles.AllPaintingInWmPaint
                     | ControlStyles.UserPaint
                     | ControlStyles.ResizeRedraw, true);

            InitializeComponent();
        }

        private void InitializeComponent()
        {
            Name = "XForm";
            ClientSize = new Size(300, 300);
            BackColor = Color.White;
        }

        #region 属性

        #region 设计器

        /// <summary>
        /// 获取或设置窗体的边框样式
        /// </summary>
        [Browsable(true)]
        [Category("Appearance")]
        [Description("获取或设置窗体的边框样式")]
        [DefaultValue(FormBorderStyle.Sizable)]
        public new FormBorderStyle FormBorderStyle
        {
            get => _formBorderStyle;
            set
            {
                _formBorderStyle = value;
                Invalidate();
            }
        }

        /// <summary>
        /// 获取或设置窗体标题栏的渐变起始颜色
        /// </summary>
        [Browsable(true)]
        [Category("Appearance")]
        [Description("获取或设置窗体标题栏的渐变起始颜色")]
        [DefaultValue(typeof(Color), "89, 98, 255")]
        public Color TitleBarStartColor
        {
            get => _titleBarStartColor;
            set
            {
                _titleBarStartColor = value;
                Invalidate();
            }
        }

        /// <summary>
        /// 获取或设置窗体标题栏的渐变结束颜色
        /// </summary>
        [Browsable(true)]
        [Category("Appearance")]
        [Description("获取或设置窗体标题栏的渐变结束颜色")]
        [DefaultValue(typeof(Color), "130, 101, 255")]
        public Color TitleBarEndColor
        {
            get => _titleBarEndColor;
            set
            {
                _titleBarEndColor = value;
                Invalidate();
            }
        }

        /// <summary>
        /// 获取或设置控件的内边距
        /// </summary>
        [Browsable(true)]
        [Category("Appearance")]
        [Description("获取或设置窗体的边框样式")]
        public new Padding Padding
        {
            get =>
                new Padding(
                    base.Padding.Left,
                    base.Padding.Top - TitleBarHeight,
                    base.Padding.Right,
                    base.Padding.Bottom);
            set =>
                base.Padding = new Padding(
                    value.Left,
                    value.Top + TitleBarHeight,
                    value.Right,
                    value.Bottom);
        }

        #endregion 设计器

        #region 常规

        /// <summary>
        /// 获取表示工作区的矩形
        /// </summary>
        [Browsable(false)]
        public new Rectangle ClientRectangle
        {
            get
            {
                switch (_formBorderStyle)
                {
                    case FormBorderStyle.None:
                        return new Rectangle(Point.Empty, Size);
                    case FormBorderStyle.FixedSingle:
                    case FormBorderStyle.Fixed3D:
                    case FormBorderStyle.FixedDialog:
                    case FormBorderStyle.Sizable:
                    case FormBorderStyle.FixedToolWindow:
                    case FormBorderStyle.SizableToolWindow:
                        return new Rectangle(BorderWidth,
                            BorderWidth + TitleBarHeight,
                            Width - BorderWidth * 2,
                            Height - TitleBarHeight - BorderWidth * 2);
                    default:
                        throw new ArgumentOutOfRangeException();
                }
            }
        }

        /// <summary>
        /// 获取表示顶边框的矩形
        /// </summary>
        [Browsable(false)]
        public Rectangle TopBorderRectangle
        {
            get
            {
                switch (_formBorderStyle)
                {
                    case FormBorderStyle.None:
                        return Rectangle.Empty;
                    case FormBorderStyle.FixedSingle:
                    case FormBorderStyle.Fixed3D:
                    case FormBorderStyle.FixedDialog:
                    case FormBorderStyle.Sizable:
                    case FormBorderStyle.FixedToolWindow:
                    case FormBorderStyle.SizableToolWindow:
                        return new Rectangle(0, 0, Width, BorderWidth);
                    default:
                        throw new ArgumentOutOfRangeException();
                }
            }
        }

        /// <summary>
        /// 获取表示底边框的矩形
        /// </summary>
        [Browsable(false)]
        public Rectangle BottomBorderRectangle
        {
            get
            {
                switch (_formBorderStyle)
                {
                    case FormBorderStyle.None:
                        return Rectangle.Empty;
                    case FormBorderStyle.FixedSingle:
                    case FormBorderStyle.Fixed3D:
                    case FormBorderStyle.FixedDialog:
                    case FormBorderStyle.Sizable:
                    case FormBorderStyle.FixedToolWindow:
                    case FormBorderStyle.SizableToolWindow:
                        return new Rectangle(0, Height - BorderWidth, Width, BorderWidth);
                    default:
                        throw new ArgumentOutOfRangeException();
                }
            }
        }

        /// <summary>
        /// 获取表示左边框的矩形
        /// </summary>
        [Browsable(false)]
        public Rectangle LeftBorderRectangle
        {
            get
            {
                switch (_formBorderStyle)
                {
                    case FormBorderStyle.None:
                        return Rectangle.Empty;
                    case FormBorderStyle.FixedSingle:
                    case FormBorderStyle.Fixed3D:
                    case FormBorderStyle.FixedDialog:
                    case FormBorderStyle.Sizable:
                    case FormBorderStyle.FixedToolWindow:
                    case FormBorderStyle.SizableToolWindow:
                        return new Rectangle(0, 0, BorderWidth, Height);
                    default:
                        throw new ArgumentOutOfRangeException();
                }
            }
        }

        /// <summary>
        /// 获取表示右边框的矩形
        /// </summary>
        [Browsable(false)]
        public Rectangle RightBorderRectangle
        {
            get
            {
                switch (_formBorderStyle)
                {
                    case FormBorderStyle.None:
                        return Rectangle.Empty;
                    case FormBorderStyle.FixedSingle:
                    case FormBorderStyle.Fixed3D:
                    case FormBorderStyle.FixedDialog:
                    case FormBorderStyle.Sizable:
                    case FormBorderStyle.FixedToolWindow:
                    case FormBorderStyle.SizableToolWindow:
                        return new Rectangle(Width - BorderWidth, 0, BorderWidth, Height);
                    default:
                        throw new ArgumentOutOfRangeException();
                }
            }
        }

        /// <summary>
        /// 获取表示标题栏的矩形
        /// </summary>
        [Browsable(false)]
        public Rectangle TitleBarRectangle
        {
            get
            {
                switch (_formBorderStyle)
                {
                    case FormBorderStyle.None:
                        return Rectangle.Empty;
                    case FormBorderStyle.FixedSingle:
                    case FormBorderStyle.Fixed3D:
                    case FormBorderStyle.FixedDialog:
                    case FormBorderStyle.Sizable:
                    case FormBorderStyle.FixedToolWindow:
                    case FormBorderStyle.SizableToolWindow:
                        return new Rectangle(0, 0, Width, TitleBarHeight);
                    default:
                        throw new ArgumentOutOfRangeException();
                }
            }
        }

        #endregion 常规

        #endregion 属性

        #region 重写

        /// <inheritdoc />
        protected override Padding DefaultPadding =>
            new Padding(
                base.DefaultPadding.Left,
                base.DefaultPadding.Top + TitleBarHeight,
                base.DefaultPadding.Right,
                base.DefaultPadding.Bottom);
        protected override CreateParams CreateParams
        {
            get
            {
                CreateParams cp = base.CreateParams;
                cp.Style |= (int)(WS_SYSMENU | WS_MINIMIZEBOX);
                return cp;
            }
        }

        /// <inheritdoc />
        protected override void OnPaint(PaintEventArgs e)
        {
            base.OnPaint(e);

            switch (FormBorderStyle)
            {
                case FormBorderStyle.None:
                    break;
                case FormBorderStyle.FixedSingle:
                case FormBorderStyle.FixedToolWindow:
                case FormBorderStyle.Fixed3D:
                case FormBorderStyle.FixedDialog:
                case FormBorderStyle.Sizable:
                case FormBorderStyle.SizableToolWindow:
                    //标题栏背景
                    using (Brush brsTitleBar = new LinearGradientBrush(TitleBarRectangle,
                        _titleBarStartColor, _titleBarEndColor, LinearGradientMode.Horizontal))
                    {
                        e.Graphics.FillRectangle(brsTitleBar, TitleBarRectangle);
                    }
                    break;
                default:
                    throw new ArgumentOutOfRangeException();
            }
        }

        /// <inheritdoc />
        protected override void WndProc(ref Message m)
        {
            switch (m.Msg)
            {
                case WM_NCHITTEST:
                    {
                        base.WndProc(ref m);

                        Point pt = PointToClient(new Point((int)m.LParam & 0xFFFF, (int)m.LParam >> 16 & 0xFFFF));

                        switch (_formBorderStyle)
                        {
                            case FormBorderStyle.None:
                            case FormBorderStyle.FixedSingle:
                            case FormBorderStyle.Fixed3D:
                            case FormBorderStyle.FixedDialog:
                            case FormBorderStyle.FixedToolWindow:
                                break;
                            case FormBorderStyle.Sizable:
                            case FormBorderStyle.SizableToolWindow:
                                bool bTop = pt.Y <= BorderWidth;
                                bool bBottom = pt.Y >= ClientSize.Height - BorderWidth;
                                bool bLeft = pt.X <= BorderWidth;
                                bool bRight = pt.X >= ClientSize.Width - BorderWidth;

                                if (bLeft)
                                    if (bTop)
                                        m.Result = (IntPtr)HTTOPLEFT;
                                    else if (bBottom)
                                        m.Result = (IntPtr)HTBOTTOMLEFT;
                                    else m.Result = (IntPtr)HTLEFT;
                                else if (bRight)
                                    if (bTop)
                                        m.Result = (IntPtr)HTTOPRIGHT;
                                    else if (bBottom)
                                        m.Result = (IntPtr)HTBOTTOMRIGHT;
                                    else m.Result = (IntPtr)HTRIGHT;
                                else if (bTop)
                                    m.Result = (IntPtr)HTTOP;
                                else if (bBottom)
                                    m.Result = (IntPtr)HTBOTTOM;
                                break;
                            default:
                                throw new ArgumentOutOfRangeException();
                        }
                        break;
                    }
                case WM_LBUTTONDOWN:
                    {
                        Point pt = new Point((int)m.LParam & 0xFFFF, (int)m.LParam >> 16 & 0xFFFF);

                        if (pt.Y < TitleBarHeight)
                        {
                            m.Msg = WM_NCLBUTTONDOWN;



                            m.WParam = (IntPtr)HTCAPTION;
                        }

                        base.WndProc(ref m);
                        break;
                    }
                case WM_SYSCOMMAND:
                    {
                        switch ((int)m.WParam)
                        {
                            case SC_MINIMIZE:

                                break;
                        }
                        base.WndProc(ref m);
                        break;
                    }
                //case WM_LBUTTONUP:
                //    {
                //        Point pt = PointToClient(new Point((int)m.LParam & 0xFFFF, (int)m.LParam >> 16 & 0xFFFF));

                //        if (pt.Y < TitleHeight)
                //        {
                //            m.Msg = WM_NCLBUTTONUP;



                //            m.WParam = (IntPtr)HTCAPTION;
                //        }
                //        base.WndProc(ref m);

                //        break;
                //    }
                //case WM_MOUSEMOVE:
                //    {

                //        Point pt = PointToClient(new Point((int)m.LParam & 0xFFFF, (int)m.LParam >> 16 & 0xFFFF));

                //        if (pt.Y < TitleHeight)
                //        {
                //            m.Msg = WM_NCMOUSEMOVE;


                //            m.WParam = (IntPtr)HTCAPTION;
                //        }
                //        base.WndProc(ref m);

                //        break;
                //    }
                //case WM_NCLBUTTONDOWN:
                //    {
                //        base.WndProc(ref m);

                //        Point pt = PointToClient(new Point((int)m.LParam & 0xFFFF, (int)m.LParam >> 16 & 0xFFFF));

                //        switch ((int)m.WParam)
                //        {
                //            case HTCAPTION:
                //                _xPadding = pt.X;
                //                _yPadding = pt.Y;
                //                _dragging = true;
                //                break;
                //        }

                //        break;
                //    }
                //case WM_NCLBUTTONUP:
                //    {
                //        base.WndProc(ref m);

                //        Point pt = PointToClient(new Point((int)m.LParam & 0xFFFF, (int)m.LParam >> 16 & 0xFFFF));

                //        switch ((int)m.WParam)
                //        {
                //            case HTCAPTION:
                //                _dragging = false;
                //                break;
                //        }

                //        break;
                //    }
                //case WM_NCMOUSEMOVE:
                //    {
                //        base.WndProc(ref m);

                //        Point pt = PointToClient(new Point((int)m.LParam & 0xFFFF, (int)m.LParam >> 16 & 0xFFFF));

                //        switch ((int)m.WParam)
                //        {
                //            case HTCAPTION:

                //                if (!_dragging) return;
                //                Location = new Point(pt.X - _xPadding, pt.Y - _yPadding);
                //                break;
                //        }

                //        break;
                //    }
                default:
                    base.WndProc(ref m);
                    break;
            }
        }

        #endregion 重写
    }
}
