using System;
using System.ComponentModel;
using System.Diagnostics;
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
        private const int BORDER_WIDTH = 2;
        // 边框热区附加宽度
        private const int BORDER_REGION_ADDTIONAL_WIDTH = 2;
        // 标题栏图标大小
        private const int ICON_SIZE = 16;
        // 标题栏按钮大小
        private const int BUTTON_WIDTH = 30;
        private const int BUTTON_HEIGHT = 30;

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

        /*
         * 标识窗口已从最大化或最小化恢复成原始尺寸，
         * 于WM_SYSCOMMAND中WParam为SC_RESTORE时，由XForm内部Post到消息队列末尾，
         * WndProc处理此消息时将_isRestoring置为false
         */
        private const int WM_RESTORED = WM_USER + 0xFF;
        // 标识窗口正在从最大化或最小化恢复成原始尺寸
        private bool _isRestoring;
        // 记录窗口最大化或最小化前的原始窗口矩形
        private RECT _rectWndBeforeRestored;

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
                        return new Rectangle(BORDER_WIDTH, BORDER_WIDTH, Width - BORDER_WIDTH * 2, TitleBarHeight);
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
                            TitleBarRectangle.Right - BUTTON_WIDTH,
                            TitleBarRectangle.Top + (TitleBarRectangle.Height - BUTTON_HEIGHT) / 2,
                            BUTTON_WIDTH,
                            BUTTON_HEIGHT);
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
                            CloseButtonRectangle.X - BUTTON_WIDTH,
                            CloseButtonRectangle.Y,
                            BUTTON_WIDTH,
                            BUTTON_HEIGHT);
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
                            MaximizeButtonRectangle.X - BUTTON_WIDTH,
                            CloseButtonRectangle.Y,
                            BUTTON_WIDTH,
                            BUTTON_HEIGHT);
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
                    new Point(_imgBtnMinimize.Width + _imgBtnMaximize.Width + _imgBtnClose.Width + BORDER_WIDTH, MinimizeButtonRectangle.Y),
                    _titleBarStartColor, _titleBarEndColor);

                g.FillRectangle(brs, new Rectangle(0, 0, _imgBtnMinimize.Width, _imgBtnMinimize.Height));
                g.DrawImage(Properties.Resources.btnMinimize, new Rectangle(0, 0, _imgBtnMinimize.Width, _imgBtnMinimize.Height));
            }
            // 最小化禁用
            using (Graphics g = Graphics.FromImage(_imgBtnMinimizeDisabled))
            {
                LinearGradientBrush brs = new LinearGradientBrush(
                    new Point(-MinimizeButtonRectangle.X, MinimizeButtonRectangle.Y),
                    new Point(_imgBtnMinimize.Width + _imgBtnMaximize.Width + _imgBtnClose.Width + BORDER_WIDTH, MinimizeButtonRectangle.Y),
                    _titleBarStartColor, _titleBarEndColor);

                g.FillRectangle(brs, new Rectangle(0, 0, _imgBtnMinimizeDisabled.Width, _imgBtnMinimizeDisabled.Height));
                g.DrawImage(Properties.Resources.btnMinimizeDisabled, new Rectangle(0, 0, _imgBtnMinimizeDisabled.Width, _imgBtnMinimizeDisabled.Height));
            }
            // 最大化
            using (Graphics g = Graphics.FromImage(_imgBtnMaximize))
            {
                LinearGradientBrush brs = new LinearGradientBrush(
                    new Point(-MaximizeButtonRectangle.X, MinimizeButtonRectangle.Y),
                    new Point(_imgBtnMaximize.Width + _imgBtnClose.Width + BORDER_WIDTH, MinimizeButtonRectangle.Y),
                    _titleBarStartColor, _titleBarEndColor);

                g.FillRectangle(brs, new Rectangle(0, 0, _imgBtnMaximize.Width, _imgBtnMaximize.Height));
                g.DrawImage(Properties.Resources.btnMaximize, new Rectangle(0, 0, _imgBtnMaximize.Width, _imgBtnMaximize.Height));
            }
            // 最大化禁用
            using (Graphics g = Graphics.FromImage(_imgBtnMaximizeDisabled))
            {
                LinearGradientBrush brs = new LinearGradientBrush(
                    new Point(-MaximizeButtonRectangle.X, MinimizeButtonRectangle.Y),
                    new Point(_imgBtnMaximizeDisabled.Width + _imgBtnClose.Width + BORDER_WIDTH, MinimizeButtonRectangle.Y),
                    _titleBarStartColor, _titleBarEndColor);

                g.FillRectangle(brs, new Rectangle(0, 0, _imgBtnMaximizeDisabled.Width, _imgBtnMaximizeDisabled.Height));
                g.DrawImage(Properties.Resources.btnMaximizeDisabled, new Rectangle(0, 0, _imgBtnMaximizeDisabled.Width, _imgBtnMaximizeDisabled.Height));
            }
            // 最大化恢复
            using (Graphics g = Graphics.FromImage(_imgBtnNormal))
            {
                LinearGradientBrush brs = new LinearGradientBrush(
                    new Point(-MaximizeButtonRectangle.X, MinimizeButtonRectangle.Y),
                    new Point(_imgBtnNormal.Width + _imgBtnClose.Width + BORDER_WIDTH, MinimizeButtonRectangle.Y),
                    _titleBarStartColor, _titleBarEndColor);

                g.FillRectangle(brs, new Rectangle(0, 0, _imgBtnNormal.Width, _imgBtnNormal.Height));
                g.DrawImage(Properties.Resources.btnNormal, new Rectangle(0, 0, _imgBtnNormal.Width, _imgBtnNormal.Height));
            }
            // 最大化恢复禁用
            using (Graphics g = Graphics.FromImage(_imgBtnNormalDisabled))
            {
                LinearGradientBrush brs = new LinearGradientBrush(
                    new Point(-MaximizeButtonRectangle.X, MinimizeButtonRectangle.Y),
                    new Point(_imgBtnNormalDisabled.Width + _imgBtnClose.Width + BORDER_WIDTH, MinimizeButtonRectangle.Y),
                    _titleBarStartColor, _titleBarEndColor);

                g.FillRectangle(brs, new Rectangle(0, 0, _imgBtnNormalDisabled.Width, _imgBtnNormalDisabled.Height));
                g.DrawImage(Properties.Resources.btnNormalDisabled, new Rectangle(0, 0, _imgBtnNormalDisabled.Width, _imgBtnNormalDisabled.Height));
            }
            // 关闭
            using (Graphics g = Graphics.FromImage(_imgBtnClose))
            {
                LinearGradientBrush brs = new LinearGradientBrush(
                    new Point(-CloseButtonRectangle.X, MinimizeButtonRectangle.Y),
                    new Point(_imgBtnClose.Width + BORDER_WIDTH, MinimizeButtonRectangle.Y),
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
                    new Point(_imgBtnMinimizeHovering.Width + _imgBtnMaximizeHovering.Width + _imgBtnCloseHovering.Width + BORDER_WIDTH, MinimizeButtonRectangle.Y),
                    cStart, cEnd);

                g.FillRectangle(brs, new Rectangle(0, 0, _imgBtnMinimizeHovering.Width, _imgBtnMinimizeHovering.Height));
                g.DrawImage(Properties.Resources.btnMinimize, new Rectangle(0, 0, _imgBtnMinimizeHovering.Width, _imgBtnMinimizeHovering.Height));
            }
            // 最大化
            using (Graphics g = Graphics.FromImage(_imgBtnMaximizeHovering))
            {
                LinearGradientBrush brs = new LinearGradientBrush(
                    new Point(-MaximizeButtonRectangle.X, MinimizeButtonRectangle.Y),
                    new Point(_imgBtnMaximizeHovering.Width + _imgBtnCloseHovering.Width + BORDER_WIDTH, MinimizeButtonRectangle.Y),
                    cStart, cEnd);

                g.FillRectangle(brs, new Rectangle(0, 0, _imgBtnMaximizeHovering.Width, _imgBtnMaximizeHovering.Height));
                g.DrawImage(Properties.Resources.btnMaximize, new Rectangle(0, 0, _imgBtnMaximizeHovering.Width, _imgBtnMaximizeHovering.Height));
            }
            // 最大化恢复
            using (Graphics g = Graphics.FromImage(_imgBtnNormalHovering))
            {
                LinearGradientBrush brs = new LinearGradientBrush(
                    new Point(-MaximizeButtonRectangle.X, MinimizeButtonRectangle.Y),
                    new Point(_imgBtnNormalHovering.Width + _imgBtnCloseHovering.Width + BORDER_WIDTH, MinimizeButtonRectangle.Y),
                    cStart, cEnd);

                g.FillRectangle(brs, new Rectangle(0, 0, _imgBtnNormalHovering.Width, _imgBtnNormalHovering.Height));
                g.DrawImage(Properties.Resources.btnNormal, new Rectangle(0, 0, _imgBtnNormalHovering.Width, _imgBtnNormalHovering.Height));
            }
            // 关闭
            using (Graphics g = Graphics.FromImage(_imgBtnCloseHovering))
            {
                LinearGradientBrush brs = new LinearGradientBrush(
                    new Point(-CloseButtonRectangle.X, MinimizeButtonRectangle.Y),
                    new Point(_imgBtnCloseHovering.Width + BORDER_WIDTH, MinimizeButtonRectangle.Y),
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
                    new Point(_imgBtnMinimizeHolding.Width + _imgBtnMaximizeHolding.Width + _imgBtnCloseHolding.Width + BORDER_WIDTH, MinimizeButtonRectangle.Y),
                    cStart, cEnd);

                g.FillRectangle(brs, new Rectangle(0, 0, _imgBtnMinimizeHolding.Width, _imgBtnMinimizeHolding.Height));
                g.DrawImage(Properties.Resources.btnMinimize, new Rectangle(0, 0, _imgBtnMinimizeHolding.Width, _imgBtnMinimizeHolding.Height));
            }
            // 最大化
            using (Graphics g = Graphics.FromImage(_imgBtnMaximizeHolding))
            {
                LinearGradientBrush brs = new LinearGradientBrush(
                    new Point(-MaximizeButtonRectangle.X, MinimizeButtonRectangle.Y),
                    new Point(_imgBtnMaximizeHolding.Width + _imgBtnCloseHolding.Width + BORDER_WIDTH, MinimizeButtonRectangle.Y),
                    cStart, cEnd);

                g.FillRectangle(brs, new Rectangle(0, 0, _imgBtnMaximizeHolding.Width, _imgBtnMaximizeHolding.Height));
                g.DrawImage(Properties.Resources.btnMaximize, new Rectangle(0, 0, _imgBtnMaximizeHolding.Width, _imgBtnMaximizeHolding.Height));
            }
            // 最大化恢复
            using (Graphics g = Graphics.FromImage(_imgBtnNormalHolding))
            {
                LinearGradientBrush brs = new LinearGradientBrush(
                    new Point(-MaximizeButtonRectangle.X, MinimizeButtonRectangle.Y),
                    new Point(_imgBtnNormalHolding.Width + _imgBtnCloseHolding.Width + BORDER_WIDTH, MinimizeButtonRectangle.Y),
                    cStart, cEnd);

                g.FillRectangle(brs, new Rectangle(0, 0, _imgBtnNormalHolding.Width, _imgBtnNormalHolding.Height));
                g.DrawImage(Properties.Resources.btnNormal, new Rectangle(0, 0, _imgBtnNormalHolding.Width, _imgBtnNormalHolding.Height));
            }
            // 关闭
            using (Graphics g = Graphics.FromImage(_imgBtnCloseHolding))
            {
                LinearGradientBrush brs = new LinearGradientBrush(
                    new Point(-CloseButtonRectangle.X, MinimizeButtonRectangle.Y),
                    new Point(_imgBtnCloseHolding.Width + BORDER_WIDTH, MinimizeButtonRectangle.Y),
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
                    new Rectangle(0, 0, Width, BORDER_WIDTH),// Top
                    new Rectangle(0, Height - BORDER_WIDTH, Width, BORDER_WIDTH),// Bottom
                    new Rectangle(0, 0, BORDER_WIDTH, Height),// Left
                    new Rectangle(Width - BORDER_WIDTH, 0, BORDER_WIDTH, Height),// Right
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
                    TitleBarRectangle.Left + (TitleBarRectangle.Height - ICON_SIZE) / 2,
                    TitleBarRectangle.Top + (TitleBarRectangle.Height - ICON_SIZE) / 2,
                    ICON_SIZE, ICON_SIZE));

            // 标题文本
            int txtX = TitleBarRectangle.Left + (TitleBarRectangle.Height - ICON_SIZE) / 2 + ICON_SIZE;
            SizeF szText = g.MeasureString(Text, SystemFonts.CaptionFont, Width, StringFormat.GenericDefault);
            using Brush brsText = new SolidBrush(_titleBarForeColor);
            g.DrawString(Text,
                SystemFonts.CaptionFont,
                brsText,
                new RectangleF(txtX,
                    TitleBarRectangle.Top + (TitleBarRectangle.Bottom - szText.Height) / 2,
                    Width - BORDER_WIDTH * 2,
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
            Bitmap bmpBackground = new Bitmap(Width + BORDER_WIDTH * 4, Height + BORDER_WIDTH * 4);

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
                brs.FocusScales = new PointF(1 - BORDER_WIDTH * 4F / Width, 1 - BORDER_WIDTH * 4F / Height);
                // 边框环绕颜色
                brs.SurroundColors = new[] { Color.FromArgb(0, 0, 0, 0) };
                // 掏空窗口实际区域
                gp.AddRectangle(new Rectangle(BORDER_WIDTH * 2, BORDER_WIDTH * 2, Width, Height));
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

                AlignShadowPos(true);

                if (Visible)
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
        /// <param name="disconnectedFromForeground">指定已解除与前台窗口的父子关系</param>
        private void AlignShadowPos(bool disconnectedFromForeground = false)
        {
            if (DesignMode) return;

            lock (this)
            {
                if (_shadow == null || _shadow.IsDisposed || _shadow.Disposing || _buildingShadow) return;

                GetWindowRect(Handle, out RECT rect);

                // 解除父子窗口关系
                if (!disconnectedFromForeground)
                    SetWindowLong(Handle, GWL_HWNDPARENT, 0);

                SetWindowPos(_shadow.Handle,
                    IntPtr.Zero,
                    rect.Left - BORDER_WIDTH * 2,
                    rect.Top - BORDER_WIDTH * 2,
                    rect.Width + BORDER_WIDTH * 4,
                    rect.Height + BORDER_WIDTH * 4,
                    SWP_NOZORDER | SWP_NOACTIVATE);

                // 设置父子窗口关系
                if (!disconnectedFromForeground)
                {
                    SetWindowLong(Handle, GWL_HWNDPARENT, _shadow.Handle.ToInt32());
                    Activate();
                }
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

                AlignShadowPos(true);

                if (Visible)
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
            return new Rectangle(rectOrigin.X - BORDER_WIDTH,
                rectOrigin.Y - TitleBarHeight + BORDER_WIDTH,
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

                cp.Style |= CS_DROPSHADOW;

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
                        m.Result = IntPtr.Zero;
                        base.WndProc(ref m);
                    }
                    break;
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
                                const int borderRegionWidth = BORDER_WIDTH + BORDER_REGION_ADDTIONAL_WIDTH;
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
                        if (_formBorderStyle == FormBorderStyle.None)
                        {
                            base.WndProc(ref m);
                            break;
                        }

                        // 自定义客户区
                        if (m.WParam != IntPtr.Zero)
                        {
                            NCCALCSIZE_PARAMS paramsIn = (NCCALCSIZE_PARAMS)
                                Marshal.PtrToStructure(m.LParam, typeof(NCCALCSIZE_PARAMS));

                            // 窗口移动调整后
                            RECT rectWndAfterMovedOrResized = paramsIn.rgrc[0];
                            // 窗口移动调整前
                            RECT rectWndBeforeMovedOrResized = paramsIn.rgrc[1];
                            // 客户区移动调整前
                            RECT rectClientBeforeMovedOrResized = paramsIn.rgrc[2];

                            NCCALCSIZE_PARAMS paramsOut = new NCCALCSIZE_PARAMS
                            {
                                rgrc = new RECT[3],
                                lppos = paramsIn.lppos
                            };

                            // 客户区调整后
                            RECT rectClientAfterMovedOrResized = new RECT(
                                rectWndAfterMovedOrResized.Left + BORDER_WIDTH,
                                rectWndAfterMovedOrResized.Top + BORDER_WIDTH + TitleBarHeight,
                                rectWndAfterMovedOrResized.Right - BORDER_WIDTH,
                                rectWndAfterMovedOrResized.Bottom + TitleBarHeight - BORDER_WIDTH);

                            paramsOut.rgrc[0] = rectClientAfterMovedOrResized;
                            paramsOut.rgrc[1] = rectWndBeforeMovedOrResized;
                            paramsOut.rgrc[2] = rectClientBeforeMovedOrResized;

                            Marshal.StructureToPtr(paramsOut, m.LParam, false);

                            m.Result = (IntPtr)WVR_VALIDRECTS;
                        }
                        else
                        {
                            RECT @params = (RECT)
                                Marshal.PtrToStructure(m.LParam, typeof(RECT));

                            @params.Left += BORDER_WIDTH;
                            @params.Top += BORDER_WIDTH + TitleBarHeight;
                            @params.Right -= BORDER_WIDTH;
                            @params.Bottom -= TitleBarHeight + BORDER_WIDTH;

                            Marshal.StructureToPtr(@params, m.LParam, false);

                            m.Result = (IntPtr)1;
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
                        base.WndProc(ref m);

                        switch (m.WParam.ToInt32())
                        {
                            case HTCLOSE:
                                SendMessage(Handle, WM_SYSCOMMAND, SC_CLOSE, 0);
                                break;
                            case HTMAXBUTTON:
                                if (MaximizeBox)
                                {
                                    int wParam = WindowState == FormWindowState.Maximized
                                        ? SC_RESTORE
                                        : SC_MAXIMIZE;
                                    SendMessage(Handle, WM_SYSCOMMAND, wParam, 0);
                                }
                                break;
                            case HTMINBUTTON:
                                if (MinimizeBox)
                                    SendMessage(Handle, WM_SYSCOMMAND, SC_MINIMIZE, 0);
                                break;
                        }
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
                case WM_WINDOWPOSCHANGING:
                    {
                        if (_isRestoring)
                        {
                            WINDOWPOS @params = (WINDOWPOS)
                                Marshal.PtrToStructure(m.LParam, typeof(WINDOWPOS));

                            @params.cx = _rectWndBeforeRestored.Width;
                            @params.cy = _rectWndBeforeRestored.Height;

                            Marshal.StructureToPtr(@params, m.LParam, false);

                            PostMessage(Handle, WM_NCPAINT, 0, 0);
                        }

                        base.WndProc(ref m);
                    }
                    break;
                case WM_WINDOWPOSCHANGED:
                    {
                        base.WndProc(ref m);
                        AlignShadowPos(true);
                    }
                    break;
                case WM_ENTERSIZEMOVE:
                    {
                        base.WndProc(ref m);

                        if (_userSizedOrMoved && _showShadow)
                            _shadow.Hide();
                    }
                    break;
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
                case WM_SYSCOMMAND:
                    {
                        switch ((int)m.WParam)
                        {
                            case SC_MINIMIZE:
                            case SC_MAXIMIZE:
                                GetWindowRect(Handle, out _rectWndBeforeRestored);
                                break;
                            case SC_RESTORE:
                                PostMessage(Handle, WM_RESTORED, 0, 0);
                                _isRestoring = true;
                                break;
                        }
                        base.WndProc(ref m);
                    }
                    break;
                case WM_RESTORED:
                    _isRestoring = false;
                    base.WndProc(ref m);
                    break;
                case WM_PAINT:
                    base.WndProc(ref m);
                    SendMessage(Handle, WM_NCPAINT, 0, 0);
                    break;
                default:
                    base.WndProc(ref m);
                    break;
            }
        }

        /// <inheritdoc />
        protected override void OnLoad(EventArgs e)
        {
            base.OnLoad(e);

            /*
             * 因调用SetWindowLong强制设置父子窗口关系，
             * 导致Application.Run创建模态消息循环的时候不会被视为模态窗口，
             * 故base.OnLoad中不会调用CenterToScreen，在此补全
             */
            if (StartPosition == FormStartPosition.CenterScreen)
                CenterToScreen();
        }

        /// <inheritdoc />
        protected override void OnShown(EventArgs e)
        {
            base.OnShown(e);

            if (_showShadow && _shadow == null)
                BuildShadow();
        }

        /// <inheritdoc />
        protected override void OnVisibleChanged(EventArgs e)
        {
            base.OnVisibleChanged(e);

            if (DesignMode)
                return;

            if (!_showShadow)
                return;

            bool visible = Visible;

            lock (this)
            {
                if (_shadow != null && _shadow?.Visible != visible)
                    _shadow.Visible = visible;
            }
        }

        /// <inheritdoc />
        protected override void OnSizeChanged(EventArgs e)
        {
            base.OnSizeChanged(e);

            if (WindowState == FormWindowState.Minimized)
                return;

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

        #endregion 重写
    }
}
