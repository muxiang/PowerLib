using System;
using System.ComponentModel;
using System.Drawing;
using System.Drawing.Drawing2D;
using System.Runtime.InteropServices;
using System.Windows.Forms;

using PowerLib.Utilities;

using static PowerLib.NativeCodes.NativeMethods;
using static PowerLib.NativeCodes.NativeConstants;
using static PowerLib.NativeCodes.NativeStructures;

namespace PowerLib.Winform
{
    /// <summary>
    /// 表示组成应用程序的用户界面的窗口或对话框。
    /// </summary>
    [ToolboxItem(false)]
    public class XForm : Form
    {
        #region 常量

        /// <summary>
        /// 标题栏高度
        /// </summary>
        public const int TitleBarHeight = 30;

        // 边框宽度
        private const int BorderWidth = 2;
        // 边框热区附加宽度
        private const int BorderRegionAddtionalWidth = 2;
        // 标题栏图标大小
        private const int IconSize = 16;
        // 标题栏按钮大小
        private const int ButtonWidth = 30;
        private const int ButtonHeight = 30;

        #endregion 常量

        #region 字段

        // 是否渲染阴影
        private bool _showShadow = true;
        // 阴影
        private XFormShadow _shadow;
        // 正在构建阴影
        private bool _buildingShadow;
        // 边框样式
        private FormBorderStyle _formBorderStyle = FormBorderStyle.Sizable;

        // 标题栏背景渐变起始颜色
        private Color _titleBarStartColor;
        // 标题栏背景渐变结束颜色
        private Color _titleBarEndColor;
        // 标题栏前景色
        private Color _titleBarForeColor;
        // 边框颜色
        private Color _borderColor;

        /* 标题栏按钮图标 */
        // 关闭(常规、鼠标移上、鼠标按下)
        private Image _imgBtnClose;
        private Image _imgBtnCloseHovering;
        private Image _imgBtnCloseHolding;
        // 最小化(常规、禁用、鼠标移上、鼠标按下)
        private Image _imgBtnMinimize;
        private Image _imgBtnMinimizeDisabled;
        private Image _imgBtnMinimizeHovering;
        private Image _imgBtnMinimizeHolding;
        // 最大化(常规、禁用、鼠标移上、鼠标按下)
        private Image _imgBtnMaximize;
        private Image _imgBtnMaximizeDisabled;
        private Image _imgBtnMaximizeHovering;
        private Image _imgBtnMaximizeHolding;
        // 最大化还原(常规、禁用、鼠标移上、鼠标按下)
        private Image _imgBtnNormal;
        private Image _imgBtnNormalDisabled;
        private Image _imgBtnNormalHovering;
        private Image _imgBtnNormalHolding;

        // 用于标记标题栏按钮需要重绘
        private bool _redrawTitleBarButtonsRequired;

        // 标识用户通过鼠标操作调整尺寸或移动窗口
        private bool _userSizedOrMoved;

        #endregion 字段

        #region 构造

        /// <summary>
        /// 初始化<see cref="XForm"/>的实例
        /// </summary>
        protected XForm()
        {
            base.FormBorderStyle = FormBorderStyle.None;
            SetStyle(ControlStyles.DoubleBuffer
                     | ControlStyles.AllPaintingInWmPaint
                     | ControlStyles.UserPaint
                , true);

            Name = "XForm";
            BackColor = Color.White;
            Padding = new Padding(0);

            _titleBarStartColor = DefaultTitleBarStartColor == Color.Transparent
                ? Color.PaleVioletRed
                : DefaultTitleBarStartColor;

            _titleBarEndColor = DefaultTitleBarEndColor == Color.Transparent
                ? Color.Pink
                : DefaultTitleBarEndColor;

            _titleBarForeColor = DefaultTitleBarForeColor == Color.Transparent
                ? Color.White
                : DefaultTitleBarForeColor;

            _borderColor = DefaultBorderColor == Color.Transparent
                ? Color.Gray
                : DefaultBorderColor;
        }

        #endregion 构造

        #region 属性

        #region 设计器

        /// <summary>
        /// 获取或设置窗体的边框样式。
        /// </summary>
        [Browsable(true)]
        [Category("Appearance")]
        [Description("获取或设置窗体的边框样式。")]
        [DefaultValue(FormBorderStyle.Sizable)]
        public new FormBorderStyle FormBorderStyle
        {
            get => _formBorderStyle;
            set
            {
                _formBorderStyle = value;
                UpdateStyles();
                DrawTitleBar();
                DrawBorder();
            }
        }

        /// <summary>
        /// 获取或设置窗体的内边距。
        /// </summary>
        [Browsable(true)]
        [Category("Appearance")]
        [Description("获取或设置窗体的内边距。")]
        public new Padding Padding
        {
            get => new Padding(base.Padding.Left, base.Padding.Top, base.Padding.Right, base.Padding.Bottom - TitleBarHeight);
            set => base.Padding = new Padding(value.Left, value.Top, value.Right, value.Bottom + TitleBarHeight);
        }

        /// <summary>
        /// 获取或设置窗体标题栏的渐变起始颜色。
        /// </summary>
        [Browsable(true)]
        [Category("Appearance")]
        [Description("获取或设置窗体标题栏的渐变起始颜色。")]
        [DefaultValue(typeof(Color), "89, 98, 255")]
        public Color TitleBarStartColor
        {
            get => _titleBarStartColor;
            set
            {
                _titleBarStartColor = value;
                DrawTitleBar();
            }
        }

        /// <summary>
        /// 获取或设置窗体标题栏的渐变结束颜色。
        /// </summary>
        [Browsable(true)]
        [Category("Appearance")]
        [Description("获取或设置窗体标题栏的渐变结束颜色。")]
        [DefaultValue(typeof(Color), "130, 101, 255")]
        public Color TitleBarEndColor
        {
            get => _titleBarEndColor;
            set
            {
                _titleBarEndColor = value;
                DrawTitleBar();
            }
        }

        /// <summary>
        /// 获取或设置窗体标题的前景色。
        /// </summary>
        [Browsable(true)]
        [Category("Appearance")]
        [Description("获取或设置窗体标题的前景色。")]
        [DefaultValue(typeof(Color), "White")]
        public Color TitleBarForeColor
        {
            get => _titleBarForeColor;
            set
            {
                _titleBarForeColor = value;
                DrawTitleBar();
            }
        }

        /// <summary>
        /// 获取或设置窗体边框的颜色。
        /// </summary>
        [Browsable(true)]
        [Category("Appearance")]
        [Description("获取或设置窗体边框的颜色。")]
        [DefaultValue(typeof(Color), "Gray")]
        public Color BorderColor
        {
            get => _borderColor;
            set
            {
                _borderColor = value;
                DrawBorder();
            }
        }

        /// <summary>
        /// 获取或设置一个值，该值指示是否渲染窗体的阴影。
        /// </summary>
        [Browsable(true)]
        [Category("Appearance")]
        [Description("获取或设置一个值，该值指示是否渲染窗体的阴影。")]
        [DefaultValue(true)]
        public bool Shadow
        {
            get => _showShadow;
            set
            {
                _showShadow = value;
                if (DesignMode)
                    return;

                if (_showShadow && Visible)
                    BuildShadow();
                else
                    _shadow?.Hide();
            }
        }

        #endregion 设计器

        #region 静态

        /// <summary>
        /// 指定一个值，将覆盖当前AppDomain下所有XForm的图标
        /// </summary>
        public static Icon OverrideIcon { get; set; }

        /// <summary>
        /// 指定一个值，作为当前AppDomain下所有<see cref="XForm"/>的<see cref="TitleBarStartColor"/>，包括<see cref="XMessageBox"/>
        /// </summary>
        public static Color DefaultTitleBarStartColor { get; set; } = Color.Transparent;

        /// <summary>
        /// 指定一个值，作为当前AppDomain下所有<see cref="XForm"/>的<see cref="TitleBarEndColor"/>，包括<see cref="XMessageBox"/>
        /// </summary>
        public static Color DefaultTitleBarEndColor { get; set; } = Color.Transparent;

        /// <summary>
        /// 指定一个值，作为当前AppDomain下所有<see cref="XForm"/>的<see cref="TitleBarForeColor"/>，包括<see cref="XMessageBox"/>
        /// </summary>
        public static Color DefaultTitleBarForeColor { get; set; } = Color.Transparent;

        /// <summary>
        /// 指定一个值，作为当前AppDomain下所有<see cref="XForm"/>的<see cref="BorderColor"/>，包括<see cref="XMessageBox"/>
        /// </summary>
        public static Color DefaultBorderColor { get; set; } = Color.Transparent;

        #endregion 静态

        #region 实例

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
                        return new Rectangle(BorderWidth, BorderWidth, Width - BorderWidth * 2, TitleBarHeight);
                    default:
                        throw new ArgumentOutOfRangeException();
                }
            }
        }

        /// <summary>
        /// 获取表示标题栏关闭按钮的矩形
        /// </summary>
        [Browsable(false)]
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

                        return new Rectangle(
                            TitleBarRectangle.Right - ButtonWidth,
                            TitleBarRectangle.Top + (TitleBarRectangle.Height - ButtonHeight) / 2,
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
        [Browsable(false)]
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
                        return new Rectangle(
                            CloseButtonRectangle.X - ButtonWidth,
                            CloseButtonRectangle.Y,
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
        [Browsable(false)]
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
                        return new Rectangle(
                            MaximizeButtonRectangle.X - ButtonWidth,
                            CloseButtonRectangle.Y,
                            ButtonWidth,
                            ButtonHeight);
                    default:
                        throw new ArgumentOutOfRangeException();
                }
            }
        }

        #endregion 实例

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
            _imgBtnMinimize = new Bitmap(TitleBarHeight, TitleBarHeight);
            _imgBtnMinimizeDisabled = new Bitmap(TitleBarHeight, TitleBarHeight);
            _imgBtnMaximize = new Bitmap(TitleBarHeight, TitleBarHeight);
            _imgBtnMaximizeDisabled = new Bitmap(TitleBarHeight, TitleBarHeight);
            _imgBtnNormal = new Bitmap(TitleBarHeight, TitleBarHeight);
            _imgBtnNormalDisabled = new Bitmap(TitleBarHeight, TitleBarHeight);
            _imgBtnClose = new Bitmap(TitleBarHeight, TitleBarHeight);

            // 最小化
            using (Graphics g = Graphics.FromImage(_imgBtnMinimize))
            {
                LinearGradientBrush brs = new LinearGradientBrush(
                    new Point(-MinimizeButtonRectangle.X, MinimizeButtonRectangle.Y),
                    new Point(_imgBtnMinimize.Width + _imgBtnMaximize.Width + _imgBtnClose.Width + BorderWidth, MinimizeButtonRectangle.Y),
                    _titleBarStartColor, _titleBarEndColor);

                g.FillRectangle(brs, new Rectangle(0, 0, _imgBtnMinimize.Width, _imgBtnMinimize.Height));
                g.DrawImage(Properties.Resources.btnMinimize, new Rectangle(0, 0, _imgBtnMinimize.Width, _imgBtnMinimize.Height));
            }
            // 最小化禁用
            using (Graphics g = Graphics.FromImage(_imgBtnMinimizeDisabled))
            {
                LinearGradientBrush brs = new LinearGradientBrush(
                    new Point(-MinimizeButtonRectangle.X, MinimizeButtonRectangle.Y),
                    new Point(_imgBtnMinimize.Width + _imgBtnMaximize.Width + _imgBtnClose.Width + BorderWidth, MinimizeButtonRectangle.Y),
                    _titleBarStartColor, _titleBarEndColor);

                g.FillRectangle(brs, new Rectangle(0, 0, _imgBtnMinimizeDisabled.Width, _imgBtnMinimizeDisabled.Height));
                g.DrawImage(Properties.Resources.btnMinimizeDisabled, new Rectangle(0, 0, _imgBtnMinimizeDisabled.Width, _imgBtnMinimizeDisabled.Height));
            }
            // 最大化
            using (Graphics g = Graphics.FromImage(_imgBtnMaximize))
            {
                LinearGradientBrush brs = new LinearGradientBrush(
                    new Point(-MaximizeButtonRectangle.X, MinimizeButtonRectangle.Y),
                    new Point(_imgBtnMaximize.Width + _imgBtnClose.Width + BorderWidth, MinimizeButtonRectangle.Y),
                    _titleBarStartColor, _titleBarEndColor);

                g.FillRectangle(brs, new Rectangle(0, 0, _imgBtnMaximize.Width, _imgBtnMaximize.Height));
                g.DrawImage(Properties.Resources.btnMaximize, new Rectangle(0, 0, _imgBtnMaximize.Width, _imgBtnMaximize.Height));
            }
            // 最大化禁用
            using (Graphics g = Graphics.FromImage(_imgBtnMaximizeDisabled))
            {
                LinearGradientBrush brs = new LinearGradientBrush(
                    new Point(-MaximizeButtonRectangle.X, MinimizeButtonRectangle.Y),
                    new Point(_imgBtnMaximizeDisabled.Width + _imgBtnClose.Width + BorderWidth, MinimizeButtonRectangle.Y),
                    _titleBarStartColor, _titleBarEndColor);

                g.FillRectangle(brs, new Rectangle(0, 0, _imgBtnMaximizeDisabled.Width, _imgBtnMaximizeDisabled.Height));
                g.DrawImage(Properties.Resources.btnMaximizeDisabled, new Rectangle(0, 0, _imgBtnMaximizeDisabled.Width, _imgBtnMaximizeDisabled.Height));
            }
            // 最大化恢复
            using (Graphics g = Graphics.FromImage(_imgBtnNormal))
            {
                LinearGradientBrush brs = new LinearGradientBrush(
                    new Point(-MaximizeButtonRectangle.X, MinimizeButtonRectangle.Y),
                    new Point(_imgBtnNormal.Width + _imgBtnClose.Width + BorderWidth, MinimizeButtonRectangle.Y),
                    _titleBarStartColor, _titleBarEndColor);

                g.FillRectangle(brs, new Rectangle(0, 0, _imgBtnNormal.Width, _imgBtnNormal.Height));
                g.DrawImage(Properties.Resources.btnNormal, new Rectangle(0, 0, _imgBtnNormal.Width, _imgBtnNormal.Height));
            }
            // 最大化恢复禁用
            using (Graphics g = Graphics.FromImage(_imgBtnNormalDisabled))
            {
                LinearGradientBrush brs = new LinearGradientBrush(
                    new Point(-MaximizeButtonRectangle.X, MinimizeButtonRectangle.Y),
                    new Point(_imgBtnNormalDisabled.Width + _imgBtnClose.Width + BorderWidth, MinimizeButtonRectangle.Y),
                    _titleBarStartColor, _titleBarEndColor);

                g.FillRectangle(brs, new Rectangle(0, 0, _imgBtnNormalDisabled.Width, _imgBtnNormalDisabled.Height));
                g.DrawImage(Properties.Resources.btnNormalDisabled, new Rectangle(0, 0, _imgBtnNormalDisabled.Width, _imgBtnNormalDisabled.Height));
            }
            // 关闭
            using (Graphics g = Graphics.FromImage(_imgBtnClose))
            {
                LinearGradientBrush brs = new LinearGradientBrush(
                    new Point(-CloseButtonRectangle.X, MinimizeButtonRectangle.Y),
                    new Point(_imgBtnClose.Width + BorderWidth, MinimizeButtonRectangle.Y),
                    _titleBarStartColor, _titleBarEndColor);

                g.FillRectangle(brs, new Rectangle(0, 0, _imgBtnClose.Width, _imgBtnClose.Height));
                g.DrawImage(Properties.Resources.btnClose, new Rectangle(0, 0, _imgBtnClose.Width, _imgBtnClose.Height));
            }
        }

        /// <summary>
        /// 创建鼠标悬浮状态按钮图标
        /// </summary>
        /// <returns></returns>
        private void CreateHoveringImages()
        {
            _imgBtnMinimizeHovering = new Bitmap(TitleBarHeight, TitleBarHeight);
            _imgBtnMaximizeHovering = new Bitmap(TitleBarHeight, TitleBarHeight);
            _imgBtnNormalHovering = new Bitmap(TitleBarHeight, TitleBarHeight);
            _imgBtnCloseHovering = new Bitmap(TitleBarHeight, TitleBarHeight);

            Color cStart = CommonUtility.GetLighterColor(_titleBarStartColor);
            Color cEnd = CommonUtility.GetLighterColor(_titleBarEndColor);

            // 最小化
            using (Graphics g = Graphics.FromImage(_imgBtnMinimizeHovering))
            {
                LinearGradientBrush brs = new LinearGradientBrush(
                    new Point(-MinimizeButtonRectangle.X, MinimizeButtonRectangle.Y),
                    new Point(_imgBtnMinimizeHovering.Width + _imgBtnMaximizeHovering.Width + _imgBtnCloseHovering.Width + BorderWidth, MinimizeButtonRectangle.Y),
                    cStart, cEnd);

                g.FillRectangle(brs, new Rectangle(0, 0, _imgBtnMinimizeHovering.Width, _imgBtnMinimizeHovering.Height));
                g.DrawImage(Properties.Resources.btnMinimize, new Rectangle(0, 0, _imgBtnMinimizeHovering.Width, _imgBtnMinimizeHovering.Height));
            }
            // 最大化
            using (Graphics g = Graphics.FromImage(_imgBtnMaximizeHovering))
            {
                LinearGradientBrush brs = new LinearGradientBrush(
                    new Point(-MaximizeButtonRectangle.X, MinimizeButtonRectangle.Y),
                    new Point(_imgBtnMaximizeHovering.Width + _imgBtnCloseHovering.Width + BorderWidth, MinimizeButtonRectangle.Y),
                    cStart, cEnd);

                g.FillRectangle(brs, new Rectangle(0, 0, _imgBtnMaximizeHovering.Width, _imgBtnMaximizeHovering.Height));
                g.DrawImage(Properties.Resources.btnMaximize, new Rectangle(0, 0, _imgBtnMaximizeHovering.Width, _imgBtnMaximizeHovering.Height));
            }
            // 最大化恢复
            using (Graphics g = Graphics.FromImage(_imgBtnNormalHovering))
            {
                LinearGradientBrush brs = new LinearGradientBrush(
                    new Point(-MaximizeButtonRectangle.X, MinimizeButtonRectangle.Y),
                    new Point(_imgBtnNormalHovering.Width + _imgBtnCloseHovering.Width + BorderWidth, MinimizeButtonRectangle.Y),
                    cStart, cEnd);

                g.FillRectangle(brs, new Rectangle(0, 0, _imgBtnNormalHovering.Width, _imgBtnNormalHovering.Height));
                g.DrawImage(Properties.Resources.btnNormal, new Rectangle(0, 0, _imgBtnNormalHovering.Width, _imgBtnNormalHovering.Height));
            }
            // 关闭
            using (Graphics g = Graphics.FromImage(_imgBtnCloseHovering))
            {
                LinearGradientBrush brs = new LinearGradientBrush(
                    new Point(-CloseButtonRectangle.X, MinimizeButtonRectangle.Y),
                    new Point(_imgBtnCloseHovering.Width + BorderWidth, MinimizeButtonRectangle.Y),
                    cStart, cEnd);

                g.FillRectangle(brs, new Rectangle(0, 0, _imgBtnCloseHovering.Width, _imgBtnCloseHovering.Height));
                g.DrawImage(Properties.Resources.btnClose, new Rectangle(0, 0, _imgBtnCloseHovering.Width, _imgBtnCloseHovering.Height));
            }
        }

        /// <summary>
        /// 创建鼠标按下状态按钮图标
        /// </summary>
        /// <returns></returns>
        private void CreateHoldingImages()
        {
            _imgBtnMinimizeHolding = new Bitmap(TitleBarHeight, TitleBarHeight);
            _imgBtnMaximizeHolding = new Bitmap(TitleBarHeight, TitleBarHeight);
            _imgBtnNormalHolding = new Bitmap(TitleBarHeight, TitleBarHeight);
            _imgBtnCloseHolding = new Bitmap(TitleBarHeight, TitleBarHeight);

            Color cStart = CommonUtility.GetDeeperColor(_titleBarStartColor);
            Color cEnd = CommonUtility.GetDeeperColor(_titleBarEndColor);

            // 最小化
            using (Graphics g = Graphics.FromImage(_imgBtnMinimizeHolding))
            {
                LinearGradientBrush brs = new LinearGradientBrush(
                    new Point(-MinimizeButtonRectangle.X, MinimizeButtonRectangle.Y),
                    new Point(_imgBtnMinimizeHolding.Width + _imgBtnMaximizeHolding.Width + _imgBtnCloseHolding.Width + BorderWidth, MinimizeButtonRectangle.Y),
                    cStart, cEnd);

                g.FillRectangle(brs, new Rectangle(0, 0, _imgBtnMinimizeHolding.Width, _imgBtnMinimizeHolding.Height));
                g.DrawImage(Properties.Resources.btnMinimize, new Rectangle(0, 0, _imgBtnMinimizeHolding.Width, _imgBtnMinimizeHolding.Height));
            }
            // 最大化
            using (Graphics g = Graphics.FromImage(_imgBtnMaximizeHolding))
            {
                LinearGradientBrush brs = new LinearGradientBrush(
                    new Point(-MaximizeButtonRectangle.X, MinimizeButtonRectangle.Y),
                    new Point(_imgBtnMaximizeHolding.Width + _imgBtnCloseHolding.Width + BorderWidth, MinimizeButtonRectangle.Y),
                    cStart, cEnd);

                g.FillRectangle(brs, new Rectangle(0, 0, _imgBtnMaximizeHolding.Width, _imgBtnMaximizeHolding.Height));
                g.DrawImage(Properties.Resources.btnMaximize, new Rectangle(0, 0, _imgBtnMaximizeHolding.Width, _imgBtnMaximizeHolding.Height));
            }
            // 最大化恢复
            using (Graphics g = Graphics.FromImage(_imgBtnNormalHolding))
            {
                LinearGradientBrush brs = new LinearGradientBrush(
                    new Point(-MaximizeButtonRectangle.X, MinimizeButtonRectangle.Y),
                    new Point(_imgBtnNormalHolding.Width + _imgBtnCloseHolding.Width + BorderWidth, MinimizeButtonRectangle.Y),
                    cStart, cEnd);

                g.FillRectangle(brs, new Rectangle(0, 0, _imgBtnNormalHolding.Width, _imgBtnNormalHolding.Height));
                g.DrawImage(Properties.Resources.btnNormal, new Rectangle(0, 0, _imgBtnNormalHolding.Width, _imgBtnNormalHolding.Height));
            }
            // 关闭
            using (Graphics g = Graphics.FromImage(_imgBtnCloseHolding))
            {
                LinearGradientBrush brs = new LinearGradientBrush(
                    new Point(-CloseButtonRectangle.X, MinimizeButtonRectangle.Y),
                    new Point(_imgBtnCloseHolding.Width + BorderWidth, MinimizeButtonRectangle.Y),
                    cStart, cEnd);

                g.FillRectangle(brs, new Rectangle(0, 0, _imgBtnCloseHolding.Width, _imgBtnCloseHolding.Height));
                g.DrawImage(Properties.Resources.btnClose, new Rectangle(0, 0, _imgBtnCloseHolding.Width, _imgBtnCloseHolding.Height));
            }
        }

        /// <summary>
        /// 绘制标题栏
        /// </summary>
        private void DrawTitleBar()
        {
            if (_formBorderStyle == FormBorderStyle.None)
                return;

            DrawTitleBackgroundTextIcon();
            CreateButtonImages();
            DrawTitleButtons();
        }

        /// <summary>
        /// 绘制边框
        /// </summary>
        private void DrawBorder()
        {
            if (_formBorderStyle == FormBorderStyle.None)
                return;

            IntPtr hdc = GetWindowDC(Handle);
            Graphics g = Graphics.FromHdc(hdc);
            using (Brush brsBorder = new SolidBrush(_borderColor))
                g.FillRectangles(brsBorder, new[]
                {
                    new Rectangle(0, 0, Width, BorderWidth),// Top
                    new Rectangle(0, Height - BorderWidth, Width, BorderWidth),// Bottom
                    new Rectangle(0, 0, BorderWidth, Height),// Left
                    new Rectangle(Width - BorderWidth, 0, BorderWidth, Height),// Right
                });

            g.Dispose();
            ReleaseDC(Handle, hdc);
        }

        /// <summary>
        /// 绘制标题栏背景、文字、图标
        /// </summary>
        private void DrawTitleBackgroundTextIcon()
        {
            IntPtr hdc = GetWindowDC(Handle);
            Graphics g = Graphics.FromHdc(hdc);

            // 标题栏背景
            using (Brush brsTitleBar = new LinearGradientBrush(TitleBarRectangle,
                _titleBarStartColor, _titleBarEndColor, LinearGradientMode.Horizontal))
                g.FillRectangle(brsTitleBar, TitleBarRectangle);

            // 标题栏图标
            if (ShowIcon)
                g.DrawIcon(Icon, new Rectangle(
                    TitleBarRectangle.Left + (TitleBarRectangle.Height - IconSize) / 2,
                    TitleBarRectangle.Top + (TitleBarRectangle.Height - IconSize) / 2,
                    IconSize, IconSize));

            // 标题文本
            int txtX = TitleBarRectangle.Left + (TitleBarRectangle.Height - IconSize) / 2 + IconSize;
            SizeF szText = g.MeasureString(Text, SystemFonts.CaptionFont, Width, StringFormat.GenericDefault);
            using Brush brsText = new SolidBrush(_titleBarForeColor);
            g.DrawString(Text,
                SystemFonts.CaptionFont,
                brsText,
                new RectangleF(txtX,
                    TitleBarRectangle.Top + (TitleBarRectangle.Bottom - szText.Height) / 2,
                    Width - BorderWidth * 2,
                    TitleBarHeight),
                StringFormat.GenericDefault);

            g.Dispose();
            ReleaseDC(Handle, hdc);
        }

        /// <summary>
        /// 绘制标题栏按钮
        /// </summary>
        private void DrawTitleButtons()
        {
            IntPtr hdc = GetWindowDC(Handle);
            Graphics g = Graphics.FromHdc(hdc);

            DrawCloseButton(g, XFormTitleBarButtonState.Normal);

            if (MaximizeBox || MinimizeBox)
            {
                DrawMaximizeButton(g, MaximizeBox ? XFormTitleBarButtonState.Normal : XFormTitleBarButtonState.Disabled);
                DrawMinimizeButton(g, MinimizeBox ? XFormTitleBarButtonState.Normal : XFormTitleBarButtonState.Disabled);
            }

            g.Dispose();
            ReleaseDC(Handle, hdc);
        }

        /// <summary>
        /// 绘制关闭按钮
        /// </summary>
        /// <param name="g"></param>
        /// <param name="state"></param>
        private void DrawCloseButton(Graphics g, XFormTitleBarButtonState state)
        {
            Image img = state switch
            {
                XFormTitleBarButtonState.Normal => _imgBtnClose,
                XFormTitleBarButtonState.Holding => _imgBtnCloseHolding,
                XFormTitleBarButtonState.Hovering => _imgBtnCloseHovering,
                _ => throw new ArgumentOutOfRangeException(nameof(state), state, null)
            };

            g.DrawImage(img, CloseButtonRectangle);
        }

        /// <summary>
        /// 绘制最大化按钮
        /// </summary>
        /// <param name="g"></param>
        /// <param name="state"></param>
        private void DrawMaximizeButton(Graphics g, XFormTitleBarButtonState state)
        {
            Image img = state switch
            {
                XFormTitleBarButtonState.Normal => (WindowState == FormWindowState.Maximized
                    ? _imgBtnNormal
                    : _imgBtnMaximize),
                XFormTitleBarButtonState.Holding => (WindowState == FormWindowState.Maximized
                    ? _imgBtnNormalHolding
                    : _imgBtnMaximizeHolding),
                XFormTitleBarButtonState.Hovering => (WindowState == FormWindowState.Maximized
                    ? _imgBtnNormalHovering
                    : _imgBtnMaximizeHovering),
                XFormTitleBarButtonState.Disabled => (WindowState == FormWindowState.Maximized
                    ? _imgBtnNormalDisabled
                    : _imgBtnMaximizeDisabled),
                _ => throw new ArgumentOutOfRangeException(nameof(state), state, null)
            };

            g.DrawImage(img, MaximizeButtonRectangle);
        }

        /// <summary>
        /// 绘制最小化按钮
        /// </summary>
        /// <param name="g"></param>
        /// <param name="state"></param>
        private void DrawMinimizeButton(Graphics g, XFormTitleBarButtonState state)
        {
            Image img = state switch
            {
                XFormTitleBarButtonState.Normal => _imgBtnMinimize,
                XFormTitleBarButtonState.Holding => _imgBtnMinimizeHolding,
                XFormTitleBarButtonState.Hovering => _imgBtnMinimizeHovering,
                XFormTitleBarButtonState.Disabled => _imgBtnMinimizeDisabled,
                _ => throw new ArgumentOutOfRangeException(nameof(state), state, null)
            };

            g.DrawImage(img, MinimizeButtonRectangle);
        }

        /// <summary>
        /// 重绘标题栏按钮
        /// </summary>
        private void RedrawTitleBarButtons()
        {
            if (!_redrawTitleBarButtonsRequired)
                return;

            DrawTitleButtons();
            _redrawTitleBarButtonsRequired = false;
        }

        /// <summary>
        /// 创建阴影位图
        /// </summary>
        /// <returns></returns>
        private Bitmap CreateShadowBitmap()
        {
            Bitmap bmpBackground = new Bitmap(Width + BorderWidth * 4, Height + BorderWidth * 4);

            GraphicsPath gp = new GraphicsPath();
            gp.AddRectangle(new Rectangle(0, 0, bmpBackground.Width, bmpBackground.Height));

            using (Graphics g = Graphics.FromImage(bmpBackground))
            using (PathGradientBrush brs = new PathGradientBrush(gp))
            {
                g.CompositingMode = CompositingMode.SourceCopy;
                g.InterpolationMode = InterpolationMode.HighQualityBicubic;
                g.PixelOffsetMode = PixelOffsetMode.HighQuality;
                g.SmoothingMode = SmoothingMode.AntiAlias;

                // 中心颜色
                brs.CenterColor = Color.FromArgb(100, Color.Black);
                // 指定从实际阴影边界到窗口边框边界的渐变
                brs.FocusScales = new PointF(1 - BorderWidth * 4F / Width, 1 - BorderWidth * 4F / Height);
                // 边框环绕颜色
                brs.SurroundColors = new[] { Color.FromArgb(0, 0, 0, 0) };
                // 掏空窗口实际区域
                gp.AddRectangle(new Rectangle(BorderWidth * 2, BorderWidth * 2, Width, Height));
                g.FillPath(brs, gp);
            }

            gp.Dispose();
            return bmpBackground;
        }

        /// <summary>
        /// 创建阴影窗口
        /// </summary>
        private void BuildShadow()
        {
            if (DesignMode) return;

            lock (this)
            {
                _buildingShadow = true;

                if (_shadow != null && !_shadow.IsDisposed && !_shadow.Disposing)
                {
                    // 解除父子窗口关系
                    SetWindowLong(
                        Handle,
                        GWL_HWNDPARENT,
                        0);

                    _shadow.Dispose();
                }

                _shadow = new XFormShadow(CreateShadowBitmap());

                _buildingShadow = false;

                AlignShadowPos();
                _shadow.Show();

                // 设置父子窗口关系
                SetWindowLong(
                    Handle,
                    GWL_HWNDPARENT,
                    _shadow.Handle.ToInt32());

                Activate();
            }
        }

        /// <summary>
        /// 对齐阴影窗口位置
        /// </summary>
        private void AlignShadowPos()
        {
            if (DesignMode) return;

            lock (this)
            {
                if (_shadow == null || _shadow.IsDisposed || _shadow.Disposing || _buildingShadow) return;

                GetWindowRect(Handle, out RECT rect);

                SetWindowPos(_shadow.Handle,
                    IntPtr.Zero,
                    rect.Left - BorderWidth * 2,
                    rect.Top - BorderWidth * 2,
                    rect.Width + BorderWidth * 4,
                    rect.Height + BorderWidth * 4,
                    SWP_NOACTIVATE);
            }
        }

        /// <summary>
        /// 对齐阴影窗口位置和尺寸
        /// </summary>
        private void AlignShadowPosSize()
        {
            if (DesignMode) return;

            lock (this)
            {
                _buildingShadow = true;

                if (_shadow == null || _shadow.IsDisposed || _shadow.Disposing)
                    return;

                _shadow.Hide();

                _shadow.UpdateBmp(CreateShadowBitmap());

                _buildingShadow = false;

                AlignShadowPos();
                _shadow.Show();

                Activate();
            }//end of lock(this)
        }

        /// <summary>
        /// 将绘图矩形校正为用于处理鼠标消息的逻辑矩形(因自定义非客户区)
        /// </summary>
        /// <param name="rectOrigin">绘图矩形</param>
        /// <returns></returns>
        private static Rectangle CorrectToLogical(Rectangle rectOrigin)
        {
            return new Rectangle(rectOrigin.X - BorderWidth,
                rectOrigin.Y - TitleBarHeight + BorderWidth,
                rectOrigin.Width,
                rectOrigin.Height);
        }

        #endregion 方法

        #region 重写

        /// <inheritdoc />
        public sealed override Color BackColor
        {
            get => base.BackColor;
            set => base.BackColor = value;
        }

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
                        DrawBorder();
                        m.Result = (IntPtr)1;
                        break;
                    }
                case WM_NCHITTEST:
                    {
                        base.WndProc(ref m);

                        Point pt = PointToClient(new Point((int)m.LParam & 0xFFFF, (int)m.LParam >> 16 & 0xFFFF));

                        _userSizedOrMoved = true;

                        switch (_formBorderStyle)
                        {
                            case FormBorderStyle.None:
                                break;
                            case FormBorderStyle.FixedSingle:
                            case FormBorderStyle.Fixed3D:
                            case FormBorderStyle.FixedDialog:
                            case FormBorderStyle.FixedToolWindow:
                                if (pt.Y < 0)
                                {
                                    _userSizedOrMoved = false;
                                    m.Result = (IntPtr)HTCAPTION;
                                }

                                if (CorrectToLogical(CloseButtonRectangle).Contains(pt))
                                    m.Result = (IntPtr)HTCLOSE;
                                if (MaximizeBox && CorrectToLogical(MaximizeButtonRectangle).Contains(pt))
                                    m.Result = (IntPtr)HTMAXBUTTON;
                                if (MinimizeBox && CorrectToLogical(MinimizeButtonRectangle).Contains(pt))
                                    m.Result = (IntPtr)HTMINBUTTON;

                                break;
                            case FormBorderStyle.Sizable:
                            case FormBorderStyle.SizableToolWindow:
                                if (pt.Y < 0)
                                {
                                    _userSizedOrMoved = false;
                                    m.Result = (IntPtr)HTCAPTION;
                                }

                                if (CorrectToLogical(CloseButtonRectangle).Contains(pt))
                                    m.Result = (IntPtr)HTCLOSE;
                                if (MaximizeBox && CorrectToLogical(MaximizeButtonRectangle).Contains(pt))
                                    m.Result = (IntPtr)HTMAXBUTTON;
                                if (MinimizeBox && CorrectToLogical(MinimizeButtonRectangle).Contains(pt))
                                    m.Result = (IntPtr)HTMINBUTTON;

                                if (WindowState == FormWindowState.Maximized)
                                    break;

                                Point ptScreen = PointToScreen(pt);
                                const int borderRegionWidth = BorderWidth + BorderRegionAddtionalWidth;
                                bool bTop = ptScreen.Y >= Top && ptScreen.Y <= Top + borderRegionWidth;
                                bool bBottom = ptScreen.Y <= Bottom && ptScreen.Y >= Bottom - borderRegionWidth;
                                bool bLeft = ptScreen.X >= Left && ptScreen.X <= Left + borderRegionWidth;
                                bool bRight = ptScreen.X <= Right && ptScreen.X >= Right - borderRegionWidth;

                                if (bLeft)
                                {
                                    _userSizedOrMoved = true;
                                    if (bTop)
                                        m.Result = (IntPtr)HTTOPLEFT;
                                    else if (bBottom)
                                        m.Result = (IntPtr)HTBOTTOMLEFT;
                                    else
                                        m.Result = (IntPtr)HTLEFT;
                                }
                                else if (bRight)
                                {
                                    _userSizedOrMoved = true;
                                    if (bTop)
                                        m.Result = (IntPtr)HTTOPRIGHT;
                                    else if (bBottom)
                                        m.Result = (IntPtr)HTBOTTOMRIGHT;
                                    else
                                        m.Result = (IntPtr)HTRIGHT;
                                }
                                else if (bTop)
                                {
                                    _userSizedOrMoved = true;
                                    m.Result = (IntPtr)HTTOP;
                                }
                                else if (bBottom)
                                {
                                    _userSizedOrMoved = true;
                                    m.Result = (IntPtr)HTBOTTOM;
                                }
                                break;
                            default:
                                throw new ArgumentOutOfRangeException();
                        }
                        break;
                    }
                case WM_NCCALCSIZE:
                    {
                        // 自定义客户区
                        if (m.WParam != IntPtr.Zero && _formBorderStyle != FormBorderStyle.None)
                        {
                            NCCALCSIZE_PARAMS @params = (NCCALCSIZE_PARAMS)
                                Marshal.PtrToStructure(m.LParam, typeof(NCCALCSIZE_PARAMS));

                            @params.rgrc[0].Top += BorderWidth + TitleBarHeight;
                            @params.rgrc[0].Bottom -= BorderWidth;
                            @params.rgrc[0].Left += BorderWidth;
                            @params.rgrc[0].Right -= BorderWidth;

                            @params.rgrc[1] = @params.rgrc[0];

                            Marshal.StructureToPtr(@params, m.LParam, false);

                            m.Result = (IntPtr)WVR_VALIDRECTS;
                        }

                        base.WndProc(ref m);
                        break;
                    }
                case WM_NCLBUTTONDBLCLK:
                    {
                        if (!MaximizeBox)
                            return;

                        base.WndProc(ref m);
                        break;
                    }
                case WM_NCLBUTTONDOWN:
                    {
                        switch ((int)m.WParam)
                        {
                            case HTCLOSE:
                                {
                                    IntPtr hdc = GetWindowDC(Handle);
                                    Graphics g = Graphics.FromHdc(hdc);
                                    DrawCloseButton(g, XFormTitleBarButtonState.Holding);
                                    ReleaseDC(Handle, hdc);
                                    _redrawTitleBarButtonsRequired = true;
                                    break;
                                }
                            case HTMAXBUTTON:
                                {
                                    if (!MaximizeBox)
                                        return;
                                    IntPtr hdc = GetWindowDC(Handle);
                                    Graphics g = Graphics.FromHdc(hdc);
                                    DrawMaximizeButton(g, XFormTitleBarButtonState.Holding);
                                    ReleaseDC(Handle, hdc);
                                    _redrawTitleBarButtonsRequired = true;
                                    break;
                                }
                            case HTMINBUTTON:
                                {
                                    if (!MinimizeBox)
                                        return;
                                    IntPtr hdc = GetWindowDC(Handle);
                                    Graphics g = Graphics.FromHdc(hdc);
                                    DrawMinimizeButton(g, XFormTitleBarButtonState.Holding);
                                    ReleaseDC(Handle, hdc);
                                    _redrawTitleBarButtonsRequired = true;
                                    break;
                                }
                            default:
                                base.WndProc(ref m);
                                return;
                        }
                        break;
                    }
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
                case WM_NCMOUSEMOVE:
                    {
                        // 通过追踪鼠标事件，进一步筛选出鼠标离开消息
                        TRACKMOUSEEVENT tme = new TRACKMOUSEEVENT();
                        tme.cbSize = (uint)Marshal.SizeOf(tme);
                        // 鼠标指针从非客户区离开
                        tme.dwFlags = TME_LEAVE | TME_NONCLIENT;
                        tme.hwndTrack = m.HWnd;
                        // 则引发WM_NCMOUSELEAVE
                        TrackMouseEvent(tme);

                        switch ((int)m.WParam)
                        {
                            case HTCLOSE:
                                {
                                    IntPtr hdc = GetWindowDC(Handle);
                                    Graphics g = Graphics.FromHdc(hdc);

                                    if (MaximizeBox) DrawMaximizeButton(g, XFormTitleBarButtonState.Normal);
                                    if (MinimizeBox) DrawMinimizeButton(g, XFormTitleBarButtonState.Normal);

                                    DrawCloseButton(g, XFormTitleBarButtonState.Hovering);

                                    ReleaseDC(Handle, hdc);
                                    _redrawTitleBarButtonsRequired = true;
                                    return;
                                }
                            case HTMAXBUTTON:
                                {
                                    IntPtr hdc = GetWindowDC(Handle);
                                    Graphics g = Graphics.FromHdc(hdc);

                                    DrawCloseButton(g, XFormTitleBarButtonState.Normal);

                                    if (MinimizeBox)
                                        DrawMinimizeButton(g, XFormTitleBarButtonState.Normal);

                                    if (!MaximizeBox)
                                    {
                                        ReleaseDC(Handle, hdc);
                                        break;
                                    }

                                    DrawMaximizeButton(g, XFormTitleBarButtonState.Hovering);
                                    ReleaseDC(Handle, hdc);
                                    _redrawTitleBarButtonsRequired = true;
                                    return;
                                }
                            case HTMINBUTTON:
                                {
                                    IntPtr hdc = GetWindowDC(Handle);
                                    Graphics g = Graphics.FromHdc(hdc);

                                    DrawCloseButton(g, XFormTitleBarButtonState.Normal);

                                    if (MaximizeBox)
                                        DrawMaximizeButton(g, XFormTitleBarButtonState.Normal);

                                    if (!MinimizeBox)
                                    {
                                        ReleaseDC(Handle, hdc);
                                        break;
                                    }

                                    DrawMinimizeButton(g, XFormTitleBarButtonState.Hovering);
                                    ReleaseDC(Handle, hdc);
                                    _redrawTitleBarButtonsRequired = true;
                                    return;
                                }
                            default:
                                RedrawTitleBarButtons();
                                break;
                        }
                        break;
                    }
                case WM_WINDOWPOSCHANGED:
                    {
                        base.WndProc(ref m);

                        if (_userSizedOrMoved)
                        {
                            DrawTitleBar();
                            DrawBorder();
                        }

                        AlignShadowPos();
                        break;
                    }
                case WM_ENTERSIZEMOVE:
                    {
                        base.WndProc(ref m);

                        if (_userSizedOrMoved && _showShadow)
                            _shadow.Hide();

                        break;
                    }
                case WM_EXITSIZEMOVE:
                    {
                        base.WndProc(ref m);

                        if (_userSizedOrMoved && _showShadow)
                            AlignShadowPosSize();

                        break;
                    }
                case WM_MOUSEMOVE:
                case WM_NCMOUSELEAVE:
                    base.WndProc(ref m);
                    RedrawTitleBarButtons();
                    break;
                default:
                    base.WndProc(ref m);
                    break;
            }
        }

        /// <inheritdoc />
        protected override void OnShown(EventArgs e)
        {
            base.OnShown(e);
            AlignShadowPos();
        }

        /// <inheritdoc />
        protected override void OnVisibleChanged(EventArgs e)
        {
            base.OnVisibleChanged(e);

            if (DesignMode)
                return;

            if (!_showShadow)
                return;

            if (_shadow == null)
                BuildShadow();

            lock (this)
            {
                if (_shadow.Visible != Visible)
                    _shadow.Visible = Visible;
            }
        }

        /// <inheritdoc />
        protected override void OnSizeChanged(EventArgs e)
        {
            base.OnSizeChanged(e);

            DrawTitleBar();
            DrawBorder();

            if (_shadow != null && _shadow.Visible)
                AlignShadowPosSize();
        }

        /// <inheritdoc />
        protected override void OnHandleCreated(EventArgs e)
        {
            base.OnHandleCreated(e);

            if (OverrideIcon != null)
                Icon = OverrideIcon;
        }

        /// <inheritdoc />
        protected override void OnTextChanged(EventArgs e)
        {
            base.OnTextChanged(e);
            DrawTitleBar();
        }

        protected override void SetBoundsCore(int x, int y, int width, int height, BoundsSpecified specified)
        {
            int newWidth = width, newHeight = height;
            if (width != Width) newWidth = width + BorderWidth * 2;
            if (height != Height) newHeight = height + TitleBarHeight + BorderWidth * 2;

            base.SetBoundsCore(x, y, newWidth, newHeight, specified);
        }

        #endregion 重写
    }
}
