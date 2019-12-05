using System;
using System.ComponentModel;
using System.Drawing;
using System.Drawing.Drawing2D;
using System.Runtime.InteropServices;
using System.Windows.Forms;

using static PowerControl.NativeMethods;
using static PowerControl.NativeConstants;
using static PowerControl.NativeStructures;

namespace PowerControl
{
    /// <summary>
    /// 表示用户界面窗口
    /// </summary>
    public class XForm : Form
    {
        private enum TitleBarButtonState
        {
            //TODO:
            Normal,
            Holding,
            Hovering,
            Disabled
        }

        #region 常量

        // 边框宽度
        private const int BorderWidth = 4;
        // 标题栏高度
        private const int TitleBarHeight = 30;
        // 标题栏图标大小
        private const int IconSize = 16;
        // 标题栏按钮大小
        private const int ButtonWidth = 30;
        private const int ButtonHeight = 30;

        #endregion 常量

        #region 字段

        // 阴影背景
        private XFormBackground _backWindow;

        private FormBorderStyle _formBorderStyle = FormBorderStyle.Sizable;
        private Color _titleBarStartColor = Color.FromArgb(89, 98, 255);
        private Color _titleBarEndColor = Color.FromArgb(130, 101, 255);
        private Color _titleForeColor = Color.White;

        // 标题栏按钮图标
        private Image _imgBtnClose;
        private Image _imgBtnCloseHovering;
        private Image _imgBtnCloseHolding;
        private Image _imgBtnMinimize;
        private Image _imgBtnMinimizeDisabled;
        private Image _imgBtnMinimizeHovering;
        private Image _imgBtnMinimizeHolding;
        private Image _imgBtnMaximize;
        private Image _imgBtnMaximizeDisabled;
        private Image _imgBtnMaximizeHovering;
        private Image _imgBtnMaximizeHolding;
        private Image _imgBtnNormal;
        private Image _imgBtnNormalDisabled;
        private Image _imgBtnNormalHovering;
        private Image _imgBtnNormalHolding;
        
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
                //| ControlStyles.ResizeRedraw
                , true);

            InitializeComponent();
            CreateButtonImages();
        }

        private void InitializeComponent()
        {
            Name = "XForm";
            BackColor = Color.White;
            Padding = new Padding(0);
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
        /// 获取或设置窗体的内边距
        /// </summary>
        [Browsable(true)]
        [Category("Appearance")]
        [Description("获取或设置窗体的内边距")]
        public new Padding Padding
        {
            get => new Padding(base.Padding.Left, base.Padding.Top, base.Padding.Right, base.Padding.Bottom - TitleBarHeight);
            set => base.Padding = new Padding(value.Left, value.Top, value.Right, value.Bottom + TitleBarHeight);
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
        /// 获取或设置窗体标题的前景色
        /// </summary>
        [Browsable(true)]
        [Category("Appearance")]
        [Description("获取或设置窗体标题的前景色")]
        [DefaultValue(typeof(Color), "White")]
        public Color TitleForeColor
        {
            get => _titleForeColor;
            set
            {
                _titleForeColor = value;
                Invalidate();
            }
        }

        #endregion 设计器

        #region 常规

        /// <summary>
        /// 获取表示标题栏的矩形(相对于包含客户区与非客户区的整个窗口)
        /// </summary>
        [Browsable(false)]
        private Rectangle TitleBarRectangle
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

        /// <summary>
        /// 获取表示标题栏关闭按钮的矩形
        /// </summary>
        private Rectangle CloseButtonRectangle
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
                        return new Rectangle(Width - BorderWidth - ButtonWidth,
                            (TitleBarHeight - ButtonHeight) / 2,
                            ButtonWidth,
                            ButtonHeight);
                    default:
                        throw new ArgumentOutOfRangeException();
                }
            }
        }

        /// <summary>
        /// 获取表示标题栏最大化按钮的矩形
        /// </summary>
        private Rectangle MaximizeButtonRectangle
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
                        return new Rectangle(CloseButtonRectangle.X - ButtonWidth,
                            (TitleBarHeight - ButtonHeight) / 2,
                            ButtonWidth,
                            ButtonHeight);
                    default:
                        throw new ArgumentOutOfRangeException();
                }
            }
        }

        /// <summary>
        /// 获取表示标题栏最小化按钮的矩形
        /// </summary>
        private Rectangle MinimizeButtonRectangle
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
                        return new Rectangle(MaximizeButtonRectangle.X - ButtonWidth,
                            (TitleBarHeight - ButtonHeight) / 2,
                            ButtonWidth,
                            ButtonHeight);
                    default:
                        throw new ArgumentOutOfRangeException();
                }
            }
        }

        #endregion 常规

        #endregion 属性

        #region 方法

        /// <summary>
        /// 创建按钮图标
        /// </summary>
        private void CreateButtonImages()
        {
            CreateNormalDisabledImages();
            CreateHoveringImages();
            CreateHoldingImages();
        }

        /// <summary>
        /// 创建常规与禁用状态按钮图标
        /// </summary>
        /// <returns></returns>
        private void CreateNormalDisabledImages()
        {
            _imgBtnMinimize = new Bitmap(Properties.Resources.btnMinimize.Width, Properties.Resources.btnMinimize.Height);
            _imgBtnMinimizeDisabled = new Bitmap(Properties.Resources.btnMinimizeDisabled.Width, Properties.Resources.btnMinimizeDisabled.Height);
            _imgBtnMaximize = new Bitmap(Properties.Resources.btnMaximize.Width, Properties.Resources.btnMaximize.Height);
            _imgBtnMaximizeDisabled = new Bitmap(Properties.Resources.btnMaximizeDisabled.Width, Properties.Resources.btnMaximizeDisabled.Height);
            _imgBtnNormal = new Bitmap(Properties.Resources.btnNormal.Width, Properties.Resources.btnNormal.Height);
            _imgBtnNormalDisabled = new Bitmap(Properties.Resources.btnNormalDisabled.Width, Properties.Resources.btnNormalDisabled.Height);
            _imgBtnClose = new Bitmap(Properties.Resources.btnClose.Width, Properties.Resources.btnClose.Height);

            // 最小化
            using (Graphics g = Graphics.FromImage(_imgBtnMinimize))
            {
                LinearGradientBrush brs = new LinearGradientBrush(
                    new Point(-MinimizeButtonRectangle.X, MinimizeButtonRectangle.Y),
                    new Point(_imgBtnMinimize.Width + _imgBtnMaximize.Width + _imgBtnClose.Width + BorderWidth, MinimizeButtonRectangle.Y),
                    _titleBarStartColor, _titleBarEndColor);

                g.FillRectangle(brs, new Rectangle(0, 0, _imgBtnMinimize.Width, _imgBtnMinimize.Height));
                g.DrawImage(Properties.Resources.btnMinimize, new Rectangle(0, 0,
                    Properties.Resources.btnMinimize.Width,
                    Properties.Resources.btnMinimize.Height));
            }
            // 最小化禁用
            using (Graphics g = Graphics.FromImage(_imgBtnMinimizeDisabled))
            {
                LinearGradientBrush brs = new LinearGradientBrush(
                    new Point(-MinimizeButtonRectangle.X, MinimizeButtonRectangle.Y),
                    new Point(_imgBtnMinimize.Width + _imgBtnMaximize.Width + _imgBtnClose.Width + BorderWidth, MinimizeButtonRectangle.Y),
                    _titleBarStartColor, _titleBarEndColor);

                g.FillRectangle(brs, new Rectangle(0, 0, _imgBtnMinimizeDisabled.Width, _imgBtnMinimizeDisabled.Height));
                g.DrawImage(Properties.Resources.btnMinimizeDisabled, new Rectangle(0, 0,
                    Properties.Resources.btnMinimizeDisabled.Width,
                    Properties.Resources.btnMinimizeDisabled.Height));
            }
            // 最大化
            using (Graphics g = Graphics.FromImage(_imgBtnMaximize))
            {
                LinearGradientBrush brs = new LinearGradientBrush(
                    new Point(-MaximizeButtonRectangle.X, MinimizeButtonRectangle.Y),
                    new Point(_imgBtnMaximize.Width + _imgBtnClose.Width + BorderWidth, MinimizeButtonRectangle.Y),
                    _titleBarStartColor, _titleBarEndColor);

                g.FillRectangle(brs, new Rectangle(0, 0, _imgBtnMaximize.Width, _imgBtnMaximize.Height));
                g.DrawImage(Properties.Resources.btnMaximize, new Rectangle(0, 0,
                    Properties.Resources.btnMaximize.Width,
                    Properties.Resources.btnMaximize.Height));
            }
            // 最大化禁用
            using (Graphics g = Graphics.FromImage(_imgBtnMaximizeDisabled))
            {
                LinearGradientBrush brs = new LinearGradientBrush(
                    new Point(-MaximizeButtonRectangle.X, MinimizeButtonRectangle.Y),
                    new Point(_imgBtnMaximizeDisabled.Width + _imgBtnClose.Width + BorderWidth, MinimizeButtonRectangle.Y),
                    _titleBarStartColor, _titleBarEndColor);

                g.FillRectangle(brs, new Rectangle(0, 0, _imgBtnMaximizeDisabled.Width, _imgBtnMaximizeDisabled.Height));
                g.DrawImage(Properties.Resources.btnMaximizeDisabled, new Rectangle(0, 0,
                    Properties.Resources.btnMaximizeDisabled.Width,
                    Properties.Resources.btnMaximizeDisabled.Height));
            }
            // 最大化恢复
            using (Graphics g = Graphics.FromImage(_imgBtnNormal))
            {
                LinearGradientBrush brs = new LinearGradientBrush(
                    new Point(-MaximizeButtonRectangle.X, MinimizeButtonRectangle.Y),
                    new Point(_imgBtnNormal.Width + _imgBtnClose.Width + BorderWidth, MinimizeButtonRectangle.Y),
                    _titleBarStartColor, _titleBarEndColor);

                g.FillRectangle(brs, new Rectangle(0, 0, _imgBtnNormal.Width, _imgBtnNormal.Height));
                g.DrawImage(Properties.Resources.btnNormal, new Rectangle(0, 0,
                    Properties.Resources.btnNormal.Width,
                    Properties.Resources.btnNormal.Height));
            }
            // 最大化恢复禁用
            using (Graphics g = Graphics.FromImage(_imgBtnNormalDisabled))
            {
                LinearGradientBrush brs = new LinearGradientBrush(
                    new Point(-MaximizeButtonRectangle.X, MinimizeButtonRectangle.Y),
                    new Point(_imgBtnNormalDisabled.Width + _imgBtnClose.Width + BorderWidth, MinimizeButtonRectangle.Y),
                    _titleBarStartColor, _titleBarEndColor);

                g.FillRectangle(brs, new Rectangle(0, 0, _imgBtnNormalDisabled.Width, _imgBtnNormalDisabled.Height));
                g.DrawImage(Properties.Resources.btnNormalDisabled, new Rectangle(0, 0,
                    Properties.Resources.btnNormalDisabled.Width,
                    Properties.Resources.btnNormalDisabled.Height));
            }
            // 关闭
            using (Graphics g = Graphics.FromImage(_imgBtnClose))
            {
                LinearGradientBrush brs = new LinearGradientBrush(
                    new Point(-CloseButtonRectangle.X, MinimizeButtonRectangle.Y),
                    new Point(_imgBtnClose.Width + BorderWidth, MinimizeButtonRectangle.Y),
                    _titleBarStartColor, _titleBarEndColor);

                g.FillRectangle(brs, new Rectangle(0, 0, _imgBtnClose.Width, _imgBtnClose.Height));
                g.DrawImage(Properties.Resources.btnClose, new Rectangle(0, 0,
                    Properties.Resources.btnClose.Width,
                    Properties.Resources.btnClose.Height));
            }
        }

        /// <summary>
        /// 创建鼠标悬浮状态按钮图标
        /// </summary>
        /// <returns></returns>
        private void CreateHoveringImages()
        {
            _imgBtnMinimizeHovering = new Bitmap(Properties.Resources.btnMinimize.Width, Properties.Resources.btnMinimize.Height);
            _imgBtnMaximizeHovering = new Bitmap(Properties.Resources.btnMaximize.Width, Properties.Resources.btnMaximize.Height);
            _imgBtnNormalHovering = new Bitmap(Properties.Resources.btnNormal.Width, Properties.Resources.btnNormal.Height);
            _imgBtnCloseHovering = new Bitmap(Properties.Resources.btnClose.Width, Properties.Resources.btnClose.Height);

            // 最小化
            using (Graphics g = Graphics.FromImage(_imgBtnMinimizeHovering))
            {
                LinearGradientBrush brs = new LinearGradientBrush(
                    new Point(-MinimizeButtonRectangle.X, MinimizeButtonRectangle.Y),
                    new Point(_imgBtnMinimizeHovering.Width + _imgBtnMaximizeHovering.Width + _imgBtnCloseHovering.Width + BorderWidth, MinimizeButtonRectangle.Y),
                    _titleBarStartColor, _titleBarEndColor);

                g.FillRectangle(brs, new Rectangle(0, 0, _imgBtnMinimizeHovering.Width, _imgBtnMinimizeHovering.Height));
                g.DrawImage(Properties.Resources.btnMinimize, new Rectangle(0, 0,
                    Properties.Resources.btnMinimize.Width,
                    Properties.Resources.btnMinimize.Height));
            }
            // 最大化
            using (Graphics g = Graphics.FromImage(_imgBtnMaximizeHovering))
            {
                LinearGradientBrush brs = new LinearGradientBrush(
                    new Point(-MaximizeButtonRectangle.X, MinimizeButtonRectangle.Y),
                    new Point(_imgBtnMaximizeHovering.Width + _imgBtnCloseHovering.Width + BorderWidth, MinimizeButtonRectangle.Y),
                    _titleBarStartColor, _titleBarEndColor);

                g.FillRectangle(brs, new Rectangle(0, 0, _imgBtnMaximizeHovering.Width, _imgBtnMaximizeHovering.Height));
                g.DrawImage(Properties.Resources.btnMaximize, new Rectangle(0, 0,
                    Properties.Resources.btnMaximize.Width,
                    Properties.Resources.btnMaximize.Height));
            }
            // 最大化恢复
            using (Graphics g = Graphics.FromImage(_imgBtnNormalHovering))
            {
                LinearGradientBrush brs = new LinearGradientBrush(
                    new Point(-MaximizeButtonRectangle.X, MinimizeButtonRectangle.Y),
                    new Point(_imgBtnNormalHovering.Width + _imgBtnCloseHovering.Width + BorderWidth, MinimizeButtonRectangle.Y),
                    _titleBarStartColor, _titleBarEndColor);

                g.FillRectangle(brs, new Rectangle(0, 0, _imgBtnNormalHovering.Width, _imgBtnNormalHovering.Height));
                g.DrawImage(Properties.Resources.btnNormal, new Rectangle(0, 0,
                    Properties.Resources.btnNormal.Width,
                    Properties.Resources.btnNormal.Height));
            }
            // 关闭
            using (Graphics g = Graphics.FromImage(_imgBtnCloseHovering))
            {
                LinearGradientBrush brs = new LinearGradientBrush(
                    new Point(-CloseButtonRectangle.X, MinimizeButtonRectangle.Y),
                    new Point(_imgBtnCloseHovering.Width + BorderWidth, MinimizeButtonRectangle.Y),
                    _titleBarStartColor, _titleBarEndColor);

                g.FillRectangle(brs, new Rectangle(0, 0, _imgBtnCloseHovering.Width, _imgBtnCloseHovering.Height));
                g.DrawImage(Properties.Resources.btnClose, new Rectangle(0, 0,
                    Properties.Resources.btnClose.Width,
                    Properties.Resources.btnClose.Height));
            }
        }

        /// <summary>
        /// 创建鼠标按下状态按钮图标
        /// </summary>
        /// <returns></returns>
        private void CreateHoldingImages()
        {
            _imgBtnMinimizeHolding = new Bitmap(Properties.Resources.btnMinimize.Width, Properties.Resources.btnMinimize.Height);
            _imgBtnMaximizeHolding = new Bitmap(Properties.Resources.btnMaximize.Width, Properties.Resources.btnMaximize.Height);
            _imgBtnNormalHolding = new Bitmap(Properties.Resources.btnNormal.Width, Properties.Resources.btnNormal.Height);
            _imgBtnCloseHolding = new Bitmap(Properties.Resources.btnClose.Width, Properties.Resources.btnClose.Height);

            // 最小化
            using (Graphics g = Graphics.FromImage(_imgBtnMinimizeHolding))
            {
                LinearGradientBrush brs = new LinearGradientBrush(
                    new Point(-MinimizeButtonRectangle.X, MinimizeButtonRectangle.Y),
                    new Point(_imgBtnMinimizeHolding.Width + _imgBtnMaximizeHolding.Width + _imgBtnCloseHolding.Width + BorderWidth, MinimizeButtonRectangle.Y),
                    _titleBarStartColor, _titleBarEndColor);

                g.FillRectangle(brs, new Rectangle(0, 0, _imgBtnMinimizeHolding.Width, _imgBtnMinimizeHolding.Height));
                g.DrawImage(Properties.Resources.btnMinimize, new Rectangle(0, 0,
                    Properties.Resources.btnMinimize.Width,
                    Properties.Resources.btnMinimize.Height));
            }
            // 最大化
            using (Graphics g = Graphics.FromImage(_imgBtnMaximizeHolding))
            {
                LinearGradientBrush brs = new LinearGradientBrush(
                    new Point(-MaximizeButtonRectangle.X, MinimizeButtonRectangle.Y),
                    new Point(_imgBtnMaximizeHolding.Width + _imgBtnCloseHolding.Width + BorderWidth, MinimizeButtonRectangle.Y),
                    _titleBarStartColor, _titleBarEndColor);

                g.FillRectangle(brs, new Rectangle(0, 0, _imgBtnMaximizeHolding.Width, _imgBtnMaximizeHolding.Height));
                g.DrawImage(Properties.Resources.btnMaximize, new Rectangle(0, 0,
                    Properties.Resources.btnMaximize.Width,
                    Properties.Resources.btnMaximize.Height));
            }
            // 最大化恢复
            using (Graphics g = Graphics.FromImage(_imgBtnNormalHolding))
            {
                LinearGradientBrush brs = new LinearGradientBrush(
                    new Point(-MaximizeButtonRectangle.X, MinimizeButtonRectangle.Y),
                    new Point(_imgBtnNormalHolding.Width + _imgBtnCloseHolding.Width + BorderWidth, MinimizeButtonRectangle.Y),
                    _titleBarStartColor, _titleBarEndColor);

                g.FillRectangle(brs, new Rectangle(0, 0, _imgBtnNormalHolding.Width, _imgBtnNormalHolding.Height));
                g.DrawImage(Properties.Resources.btnNormal, new Rectangle(0, 0,
                    Properties.Resources.btnNormal.Width,
                    Properties.Resources.btnNormal.Height));
            }
            // 关闭
            using (Graphics g = Graphics.FromImage(_imgBtnCloseHolding))
            {
                LinearGradientBrush brs = new LinearGradientBrush(
                    new Point(-CloseButtonRectangle.X, MinimizeButtonRectangle.Y),
                    new Point(_imgBtnCloseHolding.Width + BorderWidth, MinimizeButtonRectangle.Y),
                    _titleBarStartColor, _titleBarEndColor);

                g.FillRectangle(brs, new Rectangle(0, 0, _imgBtnCloseHolding.Width, _imgBtnCloseHolding.Height));
                g.DrawImage(Properties.Resources.btnClose, new Rectangle(0, 0,
                    Properties.Resources.btnClose.Width,
                    Properties.Resources.btnClose.Height));
            }
        }

        /// <summary>
        /// 绘制标题栏
        /// </summary>
        private void DrawTitleBar()
        {
            IntPtr hdc = GetWindowDC(Handle);
            Graphics g = Graphics.FromHdc(hdc);

            // 标题栏背景
            using (Brush brsTitleBar = new LinearGradientBrush(TitleBarRectangle,
                _titleBarStartColor, _titleBarEndColor, LinearGradientMode.Horizontal))
                g.FillRectangle(brsTitleBar, TitleBarRectangle);

            int txtX = BorderWidth;
            // 标题栏图标
            if (ShowIcon)
            {
                txtX += IconSize;

                g.DrawIcon(Icon, new Rectangle(
                    BorderWidth, TitleBarRectangle.Top + (TitleBarRectangle.Height - IconSize) / 2,
                    IconSize, IconSize));
            }

            // 标题文本
            SizeF szText = g.MeasureString(Text, SystemFonts.CaptionFont, Width, StringFormat.GenericDefault);
            using Brush brsText = new SolidBrush(_titleForeColor);
            g.DrawString(Text,
                SystemFonts.CaptionFont,
                brsText,
                new RectangleF(txtX,
                    TitleBarRectangle.Top + (TitleBarRectangle.Bottom - szText.Height) / 2,
                    Width - BorderWidth * 2 - SystemInformation.MinimumWindowSize.Width,
                    TitleBarHeight),
                StringFormat.GenericDefault);

            ReleaseDC(Handle, hdc);
        }

        /// <summary>
        /// 绘制标题栏按钮
        /// </summary>
        private void DrawTitleButtons()
        {
            IntPtr hdc = GetWindowDC(Handle);
            Graphics g = Graphics.FromHdc(hdc);

            g.DrawImage(_imgBtnClose, CloseButtonRectangle);

            if (MaximizeBox || MinimizeBox)
            {
                if (WindowState == FormWindowState.Maximized)
                    g.DrawImage(MaximizeBox ? _imgBtnNormal : _imgBtnNormalDisabled, MaximizeButtonRectangle);
                else
                    g.DrawImage(MaximizeBox ? _imgBtnMaximize : _imgBtnMaximizeDisabled, MaximizeButtonRectangle);

                g.DrawImage(MinimizeBox ? _imgBtnMinimize : _imgBtnMinimizeDisabled, MinimizeButtonRectangle);
            }

            ReleaseDC(Handle, hdc);
        }

        /// <summary>
        /// 将绘图矩形校正为用于处理鼠标消息的逻辑矩形(因自定义非客户区)
        /// </summary>
        /// <param name="rectOrigin">绘图矩形</param>
        /// <returns></returns>
        private Rectangle CorrectToLogical(Rectangle rectOrigin)
        {
            return new Rectangle(rectOrigin.X, rectOrigin.Y - TitleBarHeight, rectOrigin.Width, rectOrigin.Height);
        }

        #endregion 方法

        #region 重写

        /// <inheritdoc />
        protected override CreateParams CreateParams
        {
            get
            {
                CreateParams cp = base.CreateParams;
                if (MinimizeBox)
                    cp.Style |= (int)WS_MINIMIZEBOX;
                if (MaximizeBox)
                    cp.Style |= (int)WS_MAXIMIZEBOX;
                return cp;
            }
        }

        /// <inheritdoc />
        protected override void WndProc(ref Message m)
        {
            switch (m.Msg)
            {
                case WM_NCPAINT:
                    {
                        DrawTitleBar();
                        CreateButtonImages();
                        DrawTitleButtons();
                        m.Result = (IntPtr)1;
                        break;
                    }
                case WM_NCHITTEST:
                    {
                        base.WndProc(ref m);

                        Point pt = PointToClient(new Point((int)m.LParam & 0xFFFF, (int)m.LParam >> 16 & 0xFFFF));

                        switch (_formBorderStyle)
                        {
                            case FormBorderStyle.None:
                                break;
                            case FormBorderStyle.FixedSingle:
                            case FormBorderStyle.Fixed3D:
                            case FormBorderStyle.FixedDialog:
                            case FormBorderStyle.FixedToolWindow:
                                if (pt.Y < 0)
                                    m.Result = (IntPtr)HTCAPTION;
                                break;
                            case FormBorderStyle.Sizable:
                            case FormBorderStyle.SizableToolWindow:
                                if (pt.Y < 0)
                                    m.Result = (IntPtr)HTCAPTION;

                                if (CorrectToLogical(CloseButtonRectangle).Contains(pt))
                                    m.Result = (IntPtr)HTCLOSE;
                                if (CorrectToLogical(MaximizeButtonRectangle).Contains(pt))
                                    m.Result = (IntPtr)HTMAXBUTTON;
                                if (CorrectToLogical(MinimizeButtonRectangle).Contains(pt))
                                    m.Result = (IntPtr)HTMINBUTTON;

                                if (WindowState == FormWindowState.Maximized)
                                    break;

                                bool bTop = pt.Y <= -TitleBarHeight + BorderWidth;
                                bool bBottom = pt.Y >= Height - TitleBarHeight - BorderWidth;
                                bool bLeft = pt.X <= BorderWidth;
                                bool bRight = pt.X >= Width - BorderWidth;

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
                case WM_NCCALCSIZE:
                    {
                        if (m.WParam != IntPtr.Zero)
                        {
                            NCCALCSIZE_PARAMS @params = (NCCALCSIZE_PARAMS)
                                Marshal.PtrToStructure(m.LParam, typeof(NCCALCSIZE_PARAMS));
                            @params.rgrc[0].Top += TitleBarHeight;
                            @params.rgrc[0].Bottom += TitleBarHeight;
                            Marshal.StructureToPtr(@params, m.LParam, false);
                            m.Result = (IntPtr)(WVR_ALIGNTOP | WVR_ALIGNBOTTOM | WVR_REDRAW);
                        }

                        base.WndProc(ref m);
                        break;
                    }
                case WM_SIZE:
                    {
                        DrawTitleBar();
                        CreateButtonImages();
                        DrawTitleButtons();
                        base.WndProc(ref m);
                        break;
                    }
                case WM_EXITSIZEMOVE:
                    {
                        DrawTitleBar();
                        CreateButtonImages();
                        DrawTitleButtons();
                        base.WndProc(ref m);
                        break;
                    }
                //case WM_SYSCOMMAND:
                //    {
                //        switch ((int)m.WParam)
                //        {
                //            case SC_MAXIMIZE:
                //            case SC_RESTORE:
                //                if (!MaximizeBox)
                //                    return;
                //                break;
                //        }
                //        base.WndProc(ref m);
                //        break;
                //    }
                case WM_NCLBUTTONDBLCLK:
                    {
                        if (!MaximizeBox)
                            return;
                        base.WndProc(ref m);
                        break;
                    }
                //case WM_NCLBUTTONDOWN:
                //    {
                //        //base.WndProc(ref m);
                //        break;
                //    }
                case WM_NCLBUTTONUP:
                    {
                        switch (m.WParam.ToInt32())
                        {
                            case HTCLOSE:
                                m.Msg = WM_SYSCOMMAND;
                                m.WParam = (IntPtr)SC_CLOSE;
                                break;
                            case HTMAXBUTTON:
                                if (MaximizeBox)
                                {
                                    m.Msg = WM_SYSCOMMAND;
                                    m.WParam = WindowState == FormWindowState.Maximized
                                        ? (IntPtr)SC_RESTORE
                                        : (IntPtr)SC_MAXIMIZE;
                                }
                                break;
                            case HTMINBUTTON:
                                if (MinimizeBox)
                                {
                                    m.Msg = WM_SYSCOMMAND;
                                    m.WParam = (IntPtr)SC_MINIMIZE;
                                }
                                break;
                        }
                        base.WndProc(ref m);
                        break;
                    }
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
