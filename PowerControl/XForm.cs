using System;
using System.ComponentModel;
using System.Drawing;
using System.Drawing.Drawing2D;
using System.Windows.Forms;

using static PowerControl.NativeMethods;
using static PowerControl.NativeStructures;
using static PowerControl.NativeConstants;

namespace PowerControl
{
    /// <summary>
    /// 表示一个包含渐变色标题栏的窗口
    /// </summary>
    [ToolboxItem(false)]
    public partial class XForm : Form
    {
        /// <summary>
        /// 指定一个值，将覆盖当前AppDomain下所有XForm的图标
        /// </summary>
        public static Icon OverrideIcon { get; set; }

        #region 构造器

        /// <summary>
        /// 构建XForm的实例
        /// </summary>
        public XForm()
        {
            InitializeComponent();

            SetStyle(ControlStyles.ResizeRedraw | ControlStyles.OptimizedDoubleBuffer | ControlStyles.AllPaintingInWmPaint | ControlStyles.UserPaint, true);

            Font = new Font("微软雅黑", 8, FontStyle.Bold, GraphicsUnit.Point);

            CreateButtonImages();
        }

        #endregion 构造器

        #region 私有方法

        /// <summary>
        /// 创建按钮图标
        /// </summary>
        private void CreateButtonImages()
        {
            _NonClientSizeInfo ncInfo = GetNonClientInfo(Handle);
            CreateNormalImages(ncInfo);
            CreateHoveringImages(ncInfo);
            CreateHoldingImages(ncInfo);
        }

        /// <summary>
        /// 创建常规状态按钮图标
        /// </summary>
        /// <returns></returns>
        private void CreateNormalImages(_NonClientSizeInfo ncInfo)
        {
            int closeBtnPosX = ncInfo.CaptionRect.Right - ncInfo.BorderSize.Width - ncInfo.CaptionButtonSize.Width;
            int maxBtnPosX = closeBtnPosX - ncInfo.CaptionButtonSize.Width;
            int minBtnPosX = maxBtnPosX - ncInfo.CaptionButtonSize.Width;
            int btnPosY = ncInfo.BorderSize.Height + (ncInfo.CaptionHeight - ncInfo.CaptionButtonSize.Height) / 2 - 0;

            Rectangle closeRect = new Rectangle(new Point(closeBtnPosX, btnPosY), ncInfo.CaptionButtonSize);
            Rectangle maxRect = new Rectangle(new Point(maxBtnPosX, btnPosY), ncInfo.CaptionButtonSize);
            Rectangle minRect = new Rectangle(new Point(minBtnPosX, btnPosY), ncInfo.CaptionButtonSize);

            Color cLeft = Color.FromArgb(89, 98, 255);
            Color cRight = Color.FromArgb(130, 101, 255);

            _imgBtnMinimize = new Bitmap(Properties.Resources.btnMinimize.Width, Properties.Resources.btnMinimize.Height);
            _imgBtnMaximize = new Bitmap(Properties.Resources.btnMaximize.Width, Properties.Resources.btnMaximize.Height);
            _imgBtnMaximizeDisabled = new Bitmap(Properties.Resources.btnMaximizeDisabled.Width, Properties.Resources.btnMaximizeDisabled.Height);
            _imgBtnNormal = new Bitmap(Properties.Resources.btnNormal.Width, Properties.Resources.btnNormal.Height);
            _imgBtnNormalDisabled = new Bitmap(Properties.Resources.btnNormalDisabled.Width, Properties.Resources.btnNormalDisabled.Height);
            _imgBtnClose = new Bitmap(Properties.Resources.btnClose.Width, Properties.Resources.btnClose.Height);

            //最小化
            using (Graphics g = Graphics.FromImage(_imgBtnMinimize))
            {
                LinearGradientBrush brs = new LinearGradientBrush(
                    new Point(-minRect.X, btnPosY),
                    new Point(_imgBtnMinimize.Width + _imgBtnMaximize.Width + _imgBtnClose.Width + ncInfo.BorderSize.Width, btnPosY),
                    cLeft, cRight);

                g.FillRectangle(brs, new Rectangle(0, 0, _imgBtnMinimize.Width, _imgBtnMinimize.Height));
                g.DrawImage(Properties.Resources.btnMinimize, new Rectangle(0, 0,
                    Properties.Resources.btnMinimize.Width,
                    Properties.Resources.btnMinimize.Height));
            }
            //最大化
            using (Graphics g = Graphics.FromImage(_imgBtnMaximize))
            {
                LinearGradientBrush brs = new LinearGradientBrush(
                    new Point(-maxRect.X, btnPosY),
                    new Point(_imgBtnMaximize.Width + _imgBtnClose.Width + ncInfo.BorderSize.Width, btnPosY),
                    cLeft, cRight);

                g.FillRectangle(brs, new Rectangle(0, 0, _imgBtnMaximize.Width, _imgBtnMaximize.Height));
                g.DrawImage(Properties.Resources.btnMaximize, new Rectangle(0, 0,
                    Properties.Resources.btnMaximize.Width,
                    Properties.Resources.btnMaximize.Height));
            }
            //最大化禁用
            using (Graphics g = Graphics.FromImage(_imgBtnMaximizeDisabled))
            {
                LinearGradientBrush brs = new LinearGradientBrush(
                    new Point(-maxRect.X, btnPosY),
                    new Point(_imgBtnMaximizeDisabled.Width + _imgBtnClose.Width + ncInfo.BorderSize.Width, btnPosY),
                    cLeft, cRight);

                g.FillRectangle(brs, new Rectangle(0, 0, _imgBtnMaximizeDisabled.Width, _imgBtnMaximizeDisabled.Height));
                g.DrawImage(Properties.Resources.btnMaximizeDisabled, new Rectangle(0, 0,
                    Properties.Resources.btnMaximizeDisabled.Width,
                    Properties.Resources.btnMaximizeDisabled.Height));
            }
            //最大化恢复
            using (Graphics g = Graphics.FromImage(_imgBtnNormal))
            {
                LinearGradientBrush brs = new LinearGradientBrush(
                    new Point(-maxRect.X, btnPosY),
                    new Point(_imgBtnNormal.Width + _imgBtnClose.Width + ncInfo.BorderSize.Width, btnPosY),
                    cLeft, cRight);

                g.FillRectangle(brs, new Rectangle(0, 0, _imgBtnNormal.Width, _imgBtnNormal.Height));
                g.DrawImage(Properties.Resources.btnNormal, new Rectangle(0, 0,
                    Properties.Resources.btnNormal.Width,
                    Properties.Resources.btnNormal.Height));
            }
            //最大化恢复禁用
            using (Graphics g = Graphics.FromImage(_imgBtnNormalDisabled))
            {
                LinearGradientBrush brs = new LinearGradientBrush(
                    new Point(-maxRect.X, btnPosY),
                    new Point(_imgBtnNormalDisabled.Width + _imgBtnClose.Width + ncInfo.BorderSize.Width, btnPosY),
                    cLeft, cRight);

                g.FillRectangle(brs, new Rectangle(0, 0, _imgBtnNormalDisabled.Width, _imgBtnNormalDisabled.Height));
                g.DrawImage(Properties.Resources.btnNormalDisabled, new Rectangle(0, 0,
                    Properties.Resources.btnNormalDisabled.Width,
                    Properties.Resources.btnNormalDisabled.Height));
            }
            //关闭
            using (Graphics g = Graphics.FromImage(_imgBtnClose))
            {
                LinearGradientBrush brs = new LinearGradientBrush(
                    new Point(-closeRect.X, btnPosY),
                    new Point(_imgBtnClose.Width + ncInfo.BorderSize.Width, btnPosY),
                    cLeft, cRight);

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
        private void CreateHoveringImages(_NonClientSizeInfo ncInfo)
        {
            int closeBtnPosX = ncInfo.CaptionRect.Right - ncInfo.BorderSize.Width - ncInfo.CaptionButtonSize.Width;
            int maxBtnPosX = closeBtnPosX - ncInfo.CaptionButtonSize.Width;
            int minBtnPosX = maxBtnPosX - ncInfo.CaptionButtonSize.Width;
            int btnPosY = ncInfo.BorderSize.Height + (ncInfo.CaptionHeight - ncInfo.CaptionButtonSize.Height) / 2 - 0;

            Rectangle closeRect = new Rectangle(new Point(closeBtnPosX, btnPosY), ncInfo.CaptionButtonSize);
            Rectangle maxRect = new Rectangle(new Point(maxBtnPosX, btnPosY), ncInfo.CaptionButtonSize);
            Rectangle minRect = new Rectangle(new Point(minBtnPosX, btnPosY), ncInfo.CaptionButtonSize);

            Color cLeft = Utilities.GetLighterColor(Color.FromArgb(89, 98, 255));
            Color cRight = Utilities.GetLighterColor(Color.FromArgb(130, 101, 255));

            _imgBtnMinimizeHovering = new Bitmap(Properties.Resources.btnMinimize.Width, Properties.Resources.btnMinimize.Height);
            _imgBtnMaximizeHovering = new Bitmap(Properties.Resources.btnMaximize.Width, Properties.Resources.btnMaximize.Height);
            _imgBtnNormalHovering = new Bitmap(Properties.Resources.btnNormal.Width, Properties.Resources.btnNormal.Height);
            _imgBtnCloseHovering = new Bitmap(Properties.Resources.btnClose.Width, Properties.Resources.btnClose.Height);

            //最小化
            using (Graphics g = Graphics.FromImage(_imgBtnMinimizeHovering))
            {
                LinearGradientBrush brs = new LinearGradientBrush(
                    new Point(-minRect.X, btnPosY),
                    new Point(_imgBtnMinimizeHovering.Width + _imgBtnMaximizeHovering.Width + _imgBtnCloseHovering.Width + ncInfo.BorderSize.Width, btnPosY),
                    cLeft, cRight);

                g.FillRectangle(brs, new Rectangle(0, 0, _imgBtnMinimizeHovering.Width, _imgBtnMinimizeHovering.Height));
                g.DrawImage(Properties.Resources.btnMinimize, new Rectangle(0, 0,
                    Properties.Resources.btnMinimize.Width,
                    Properties.Resources.btnMinimize.Height));
            }
            //最大化
            using (Graphics g = Graphics.FromImage(_imgBtnMaximizeHovering))
            {
                LinearGradientBrush brs = new LinearGradientBrush(
                    new Point(-maxRect.X, btnPosY),
                    new Point(_imgBtnMaximizeHovering.Width + _imgBtnCloseHovering.Width + ncInfo.BorderSize.Width, btnPosY),
                    cLeft, cRight);

                g.FillRectangle(brs, new Rectangle(0, 0, _imgBtnMaximizeHovering.Width, _imgBtnMaximizeHovering.Height));
                g.DrawImage(Properties.Resources.btnMaximize, new Rectangle(0, 0,
                    Properties.Resources.btnMaximize.Width,
                    Properties.Resources.btnMaximize.Height));
            }
            //最大化恢复
            using (Graphics g = Graphics.FromImage(_imgBtnNormalHovering))
            {
                LinearGradientBrush brs = new LinearGradientBrush(
                    new Point(-maxRect.X, btnPosY),
                    new Point(_imgBtnNormalHovering.Width + _imgBtnCloseHovering.Width + ncInfo.BorderSize.Width, btnPosY),
                    cLeft, cRight);

                g.FillRectangle(brs, new Rectangle(0, 0, _imgBtnNormalHovering.Width, _imgBtnNormalHovering.Height));
                g.DrawImage(Properties.Resources.btnNormal, new Rectangle(0, 0,
                    Properties.Resources.btnNormal.Width,
                    Properties.Resources.btnNormal.Height));
            }
            //关闭
            using (Graphics g = Graphics.FromImage(_imgBtnCloseHovering))
            {
                LinearGradientBrush brs = new LinearGradientBrush(
                    new Point(-closeRect.X, btnPosY),
                    new Point(_imgBtnCloseHovering.Width + ncInfo.BorderSize.Width, btnPosY),
                    cLeft, cRight);

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
        private void CreateHoldingImages(_NonClientSizeInfo ncInfo)
        {
            int closeBtnPosX = ncInfo.CaptionRect.Right - ncInfo.BorderSize.Width - ncInfo.CaptionButtonSize.Width;
            int maxBtnPosX = closeBtnPosX - ncInfo.CaptionButtonSize.Width;
            int minBtnPosX = maxBtnPosX - ncInfo.CaptionButtonSize.Width;
            int btnPosY = ncInfo.BorderSize.Height + (ncInfo.CaptionHeight - ncInfo.CaptionButtonSize.Height) / 2 - 0;

            Rectangle closeRect = new Rectangle(new Point(closeBtnPosX, btnPosY), ncInfo.CaptionButtonSize);
            Rectangle maxRect = new Rectangle(new Point(maxBtnPosX, btnPosY), ncInfo.CaptionButtonSize);
            Rectangle minRect = new Rectangle(new Point(minBtnPosX, btnPosY), ncInfo.CaptionButtonSize);

            Color cLeft = Utilities.GetDeeperColor(Color.FromArgb(89, 98, 255));
            Color cRight = Utilities.GetDeeperColor(Color.FromArgb(130, 101, 255));

            _imgBtnMinimizeHolding = new Bitmap(Properties.Resources.btnMinimize.Width, Properties.Resources.btnMinimize.Height);
            _imgBtnMaximizeHolding = new Bitmap(Properties.Resources.btnMaximize.Width, Properties.Resources.btnMaximize.Height);
            _imgBtnNormalHolding = new Bitmap(Properties.Resources.btnNormal.Width, Properties.Resources.btnNormal.Height);
            _imgBtnCloseHolding = new Bitmap(Properties.Resources.btnClose.Width, Properties.Resources.btnClose.Height);

            //最小化
            using (Graphics g = Graphics.FromImage(_imgBtnMinimizeHolding))
            {
                LinearGradientBrush brs = new LinearGradientBrush(
                    new Point(-minRect.X, btnPosY),
                    new Point(_imgBtnMinimizeHolding.Width + _imgBtnMaximizeHolding.Width + _imgBtnCloseHolding.Width + ncInfo.BorderSize.Width, btnPosY),
                    cLeft, cRight);

                g.FillRectangle(brs, new Rectangle(0, 0, _imgBtnMinimizeHolding.Width, _imgBtnMinimizeHolding.Height));
                g.DrawImage(Properties.Resources.btnMinimize, new Rectangle(0, 0,
                    Properties.Resources.btnMinimize.Width,
                    Properties.Resources.btnMinimize.Height));
            }
            //最大化
            using (Graphics g = Graphics.FromImage(_imgBtnMaximizeHolding))
            {
                LinearGradientBrush brs = new LinearGradientBrush(
                    new Point(-maxRect.X, btnPosY),
                    new Point(_imgBtnMaximizeHolding.Width + _imgBtnCloseHolding.Width + ncInfo.BorderSize.Width, btnPosY),
                    cLeft, cRight);

                g.FillRectangle(brs, new Rectangle(0, 0, _imgBtnMaximizeHolding.Width, _imgBtnMaximizeHolding.Height));
                g.DrawImage(Properties.Resources.btnMaximize, new Rectangle(0, 0,
                    Properties.Resources.btnMaximize.Width,
                    Properties.Resources.btnMaximize.Height));
            }
            //最大化恢复
            using (Graphics g = Graphics.FromImage(_imgBtnNormalHolding))
            {
                LinearGradientBrush brs = new LinearGradientBrush(
                    new Point(-maxRect.X, btnPosY),
                    new Point(_imgBtnNormalHolding.Width + _imgBtnCloseHolding.Width + ncInfo.BorderSize.Width, btnPosY),
                    cLeft, cRight);

                g.FillRectangle(brs, new Rectangle(0, 0, _imgBtnNormalHolding.Width, _imgBtnNormalHolding.Height));
                g.DrawImage(Properties.Resources.btnNormal, new Rectangle(0, 0,
                    Properties.Resources.btnNormal.Width,
                    Properties.Resources.btnNormal.Height));
            }
            //关闭
            using (Graphics g = Graphics.FromImage(_imgBtnCloseHolding))
            {
                LinearGradientBrush brs = new LinearGradientBrush(
                    new Point(-closeRect.X, btnPosY),
                    new Point(_imgBtnCloseHolding.Width + ncInfo.BorderSize.Width, btnPosY),
                    cLeft, cRight);

                g.FillRectangle(brs, new Rectangle(0, 0, _imgBtnCloseHolding.Width, _imgBtnCloseHolding.Height));
                g.DrawImage(Properties.Resources.btnClose, new Rectangle(0, 0,
                    Properties.Resources.btnClose.Width,
                    Properties.Resources.btnClose.Height));
            }
        }

        #endregion 私有方法

        #region 字段

        //标题栏按钮图标
        private Image _imgBtnClose;
        private Image _imgBtnCloseHovering;
        private Image _imgBtnCloseHolding;
        private Image _imgBtnMinimize;
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

        struct _NonClientSizeInfo
        {
            public Size CaptionButtonSize;
            public Size BorderSize;
            public int CaptionHeight;
            public Rectangle CaptionRect;
            public Rectangle Rect;
            public Rectangle ClientRect;
            public int Width;
            public int Height;
        }

        private void DrawCaption(IntPtr hwnd, bool active)
        {
            if (FormBorderStyle == FormBorderStyle.None)
                return;

            IntPtr dc;
            Graphics g;
            Size iconSize;
            _NonClientSizeInfo ncInfo;

            iconSize = SystemInformation.SmallIconSize;

            dc = GetWindowDC(hwnd);
            ncInfo = GetNonClientInfo(hwnd);
            g = Graphics.FromHdc(dc);

            g.PixelOffsetMode = PixelOffsetMode.HighQuality;
            g.SmoothingMode = SmoothingMode.HighQuality;

            Rectangle rect = ncInfo.CaptionRect;
            g.FillRectangle(new LinearGradientBrush(
                new PointF(rect.X - ncInfo.BorderSize.Width, rect.Y - ncInfo.BorderSize.Height + rect.Height / 2F),
                new PointF(rect.Right + ncInfo.BorderSize.Width, rect.Y - ncInfo.BorderSize.Height + rect.Height / 2F),
                Color.FromArgb(89, 98, 255),
                Color.FromArgb(130, 101, 255)),
                rect);

            DrawBorder(g, ncInfo, active);
            DrawTitle(g, ncInfo, active);
            DrawControlBox(g, ncInfo);

            g.Dispose();
            ReleaseDC(hwnd, dc);
        }

        private void DrawControlBox(Graphics g, _NonClientSizeInfo info)
        {
            if (ControlBox)
            {
                int closeBtnPosX = info.CaptionRect.Right - info.BorderSize.Width - info.CaptionButtonSize.Width;
                int maxBtnPosX = closeBtnPosX - info.CaptionButtonSize.Width;
                int minBtnPosX = maxBtnPosX - info.CaptionButtonSize.Width;
                int btnPosY = info.BorderSize.Height + (info.CaptionHeight - info.CaptionButtonSize.Height) / 2 - 0;

                Rectangle btnRect = new Rectangle(new Point(closeBtnPosX, btnPosY), info.CaptionButtonSize);
                Rectangle maxRect = new Rectangle(new Point(maxBtnPosX, btnPosY), info.CaptionButtonSize);
                Rectangle minRect = new Rectangle(new Point(minBtnPosX, btnPosY), info.CaptionButtonSize);

                g.DrawImage(_imgBtnClose, btnRect);

                if (MaximizeBox || MinimizeBox)
                {
                    if (FormBorderStyle != FormBorderStyle.FixedToolWindow &&
                        FormBorderStyle != FormBorderStyle.SizableToolWindow)
                    {
                        if (WindowState == FormWindowState.Maximized)
                            g.DrawImage(MaximizeBox ? _imgBtnNormal : _imgBtnNormalDisabled, maxRect);
                        else
                            g.DrawImage(MaximizeBox ? _imgBtnMaximize : _imgBtnMaximizeDisabled, maxRect);

                        g.DrawImage(_imgBtnMinimize, minRect);
                    }
                }
            }
        }

        private void DrawTitle(Graphics g, _NonClientSizeInfo ncInfo, bool active)
        {
            int titleX;

            if (ShowIcon &&
                FormBorderStyle != FormBorderStyle.FixedToolWindow &&
                FormBorderStyle != FormBorderStyle.SizableToolWindow)
            {
                Size iconSize = SystemInformation.SmallIconSize;
                g.DrawIcon(Icon, new Rectangle(
                    new Point(ncInfo.BorderSize.Width, ncInfo.BorderSize.Height + (ncInfo.CaptionHeight - iconSize.Height) / 2),
                    iconSize));
                titleX = ncInfo.BorderSize.Width + iconSize.Width + ncInfo.BorderSize.Width;
            }
            else
            {
                titleX = ncInfo.BorderSize.Width;
            }

            SizeF captionTitleSize = g.MeasureString(Text, SystemFonts.CaptionFont);
            g.DrawString(Text, SystemFonts.CaptionFont, new SolidBrush(Color.White),
                    new RectangleF(titleX,
                        ncInfo.BorderSize.Height + (ncInfo.CaptionHeight - captionTitleSize.Height) / 2,
                        ncInfo.CaptionRect.Width - ncInfo.BorderSize.Width * 2 - SystemInformation.MinimumWindowSize.Width,
                        ncInfo.CaptionRect.Height), StringFormat.GenericTypographic);
        }

        private void DrawBorder(Graphics g, _NonClientSizeInfo ncInfo, bool active)
        {
            Rectangle borderTop = new Rectangle(ncInfo.Rect.Left,
                    ncInfo.Rect.Top,
                    ncInfo.Rect.Left + ncInfo.Rect.Width,
                    ncInfo.Rect.Top + ncInfo.BorderSize.Height);
            Rectangle borderLeft = new Rectangle(
                    new Point(ncInfo.Rect.Location.X, ncInfo.Rect.Location.Y + ncInfo.BorderSize.Height),
                    new Size(ncInfo.BorderSize.Width, ncInfo.ClientRect.Height + ncInfo.CaptionHeight + ncInfo.BorderSize.Height));
            Rectangle borderRight = new Rectangle(ncInfo.Rect.Left + ncInfo.Rect.Width - ncInfo.BorderSize.Width,
                    ncInfo.Rect.Top + ncInfo.BorderSize.Height,
                    ncInfo.BorderSize.Width,
                    ncInfo.ClientRect.Height + ncInfo.CaptionHeight + ncInfo.BorderSize.Height);
            Rectangle borderBottom = new Rectangle(ncInfo.Rect.Left/* + ncInfo.BorderSize.Width*/,
                    ncInfo.Rect.Top + ncInfo.Rect.Height - ncInfo.BorderSize.Height,
                    ncInfo.Rect.Width/* - ncInfo.BorderSize.Width * 2*/,
                    ncInfo.BorderSize.Height);

            //上边框 
            Brush brs = new LinearGradientBrush(
                new Point(borderTop.X, borderTop.Y + borderTop.Height / 2),
                new Point(borderTop.Right, borderTop.Y + borderTop.Height / 2),
                Color.FromArgb(89, 98, 255),
                Color.FromArgb(130, 101, 255));
            g.FillRectangle(brs, borderTop);

            //左边框
            brs.Dispose();
            brs = new SolidBrush(BackColor);
            g.FillRectangle(brs, borderLeft);
            brs.Dispose();
            //重绘与标题栏相邻部分，融入标题栏
            brs = new LinearGradientBrush(
                new Point(borderLeft.X, borderLeft.Y + ncInfo.CaptionHeight / 2),
                new Point(borderLeft.Right + ncInfo.CaptionRect.Width + borderRight.Width, borderLeft.Y + ncInfo.CaptionHeight / 2),
                Color.FromArgb(89, 98, 255),
                Color.FromArgb(130, 101, 255));
            g.FillRectangle(brs, new RectangleF(borderLeft.X, borderLeft.Y, borderLeft.Width, ncInfo.CaptionRect.Height));

            //右边框
            brs.Dispose();
            brs = new SolidBrush(BackColor);
            g.FillRectangle(brs, borderRight);
            //重绘与标题栏相邻部分，融入标题栏
            brs = new LinearGradientBrush(
                new Point(borderLeft.X, borderLeft.Y + ncInfo.CaptionHeight / 2),
                new Point(borderLeft.Right + ncInfo.CaptionRect.Width + borderRight.Width, borderLeft.Y + ncInfo.CaptionHeight / 2),
                Color.FromArgb(89, 98, 255),
                Color.FromArgb(130, 101, 255));
            g.FillRectangle(brs, new RectangleF(borderRight.X, borderRight.Y, borderRight.Width, ncInfo.CaptionRect.Height));

            //底边框
            brs.Dispose();
            brs = new SolidBrush(BackColor);
            g.FillRectangle(brs, borderBottom);
        }

        private _NonClientSizeInfo GetNonClientInfo(IntPtr hwnd)
        {
            _NonClientSizeInfo info = new _NonClientSizeInfo();
            info.CaptionButtonSize = SystemInformation.CaptionButtonSize;
            info.CaptionHeight = SystemInformation.CaptionHeight;

            int borderSizeFactor = 1;
            //if (DwmIsCompositionEnabled(out bool isEnabled) == 0 && isEnabled)
            //    borderSizeFactor = 2;

            switch (FormBorderStyle)
            {
                case FormBorderStyle.Fixed3D:
                    info.BorderSize = SystemInformation.FixedFrameBorderSize;
                    info.BorderSize = new Size(
                        info.BorderSize.Width * borderSizeFactor,
                        info.BorderSize.Height * borderSizeFactor);
                    break;
                case FormBorderStyle.FixedDialog:
                    info.BorderSize = SystemInformation.FixedFrameBorderSize;
                    info.BorderSize = new Size(
                        info.BorderSize.Width * borderSizeFactor,
                        info.BorderSize.Height * borderSizeFactor);
                    break;
                case FormBorderStyle.FixedSingle:
                    info.BorderSize = SystemInformation.FixedFrameBorderSize;
                    info.BorderSize = new Size(
                        info.BorderSize.Width * borderSizeFactor,
                        info.BorderSize.Height * borderSizeFactor);
                    break;
                case FormBorderStyle.FixedToolWindow:
                    info.BorderSize = SystemInformation.FixedFrameBorderSize;
                    info.BorderSize = new Size(
                        info.BorderSize.Width * borderSizeFactor,
                        info.BorderSize.Height * borderSizeFactor);
                    info.CaptionButtonSize = SystemInformation.ToolWindowCaptionButtonSize;
                    info.CaptionHeight = SystemInformation.ToolWindowCaptionHeight;
                    break;
                case FormBorderStyle.Sizable:
                    info.BorderSize = SystemInformation.FrameBorderSize;
                    info.BorderSize = new Size(
                        info.BorderSize.Width * borderSizeFactor,
                        info.BorderSize.Height * borderSizeFactor);
                    break;
                case FormBorderStyle.SizableToolWindow:
                    info.CaptionButtonSize = SystemInformation.ToolWindowCaptionButtonSize;
                    info.BorderSize = SystemInformation.FrameBorderSize;
                    info.BorderSize = new Size(
                        info.BorderSize.Width * borderSizeFactor,
                        info.BorderSize.Height * borderSizeFactor);
                    info.CaptionHeight = SystemInformation.ToolWindowCaptionHeight;
                    break;
                default:
                    info.BorderSize = SystemInformation.BorderSize;
                    break;
            }

            RECT areatRect = new RECT();
            GetWindowRect(hwnd, ref areatRect);

            int width = areatRect.Right - areatRect.Left;
            int height = areatRect.Bottom - areatRect.Top;

            info.Width = width;
            info.Height = height;

            Point xy = new Point(areatRect.Left, areatRect.Top);
            xy.Offset(-areatRect.Left, -areatRect.Top);

            info.CaptionRect = new Rectangle(xy.X, xy.Y + info.BorderSize.Height, width /*+ info.BorderSize.Width * 2*/, info.CaptionHeight/* + info.BorderSize.Height*/);
            info.Rect = new Rectangle(xy.X, xy.Y, width, height);
            info.ClientRect = new Rectangle(xy.X + info.BorderSize.Width,
                xy.Y + info.CaptionHeight + info.BorderSize.Height,
                width - info.BorderSize.Width * 2,
                height - info.CaptionHeight - info.BorderSize.Height * 2);

            return info;
        }

        #region 重写

        protected override void OnHandleCreated(EventArgs e)
        {
            base.OnHandleCreated(e);

            if (OverrideIcon != null)
                Icon = OverrideIcon;
        }

        /// <summary>
        /// 背景色变更时发生
        /// </summary>
        /// <param name="e"></param>
        protected override void OnBackColorChanged(EventArgs e)
        {
            base.OnBackColorChanged(e);
            Invalidate();
        }

        //protected override void OnResize(EventArgs e)
        //{
        //    base.OnResize(e);
        //    CreateButtonImages();
        //    DrawCaption(Handle, ActiveForm == this);
        //}

        protected override void OnResizeEnd(EventArgs e)
        {
            base.OnResizeEnd(e);
            CreateButtonImages();
            DrawCaption(Handle, ActiveForm == this);
        }

        /// <summary>
        /// 文本变更时发生
        /// </summary>
        /// <param name="e">事件参数</param>
        protected override void OnTextChanged(EventArgs e)
        {
            base.OnTextChanged(e);
            DrawCaption(Handle, true);
        }

        /// <summary>
        /// 重绘时调用
        /// </summary>
        /// <param name="e">事件参数</param>
        protected override void OnPaint(PaintEventArgs e)
        {
            base.OnPaint(e);
            DrawCaption(Handle, true);
        }

        private static int LOBYTE(long p) { return (int)(p & 0x0000FFFF); }
        private static int HIBYTE(long p) { return (int)(p >> 16); }
        
        /// <summary>
        /// 窗口过程
        /// </summary>
        /// <param name="m">消息</param>
        protected override void WndProc(ref Message m)
        {
            if (FormBorderStyle != FormBorderStyle.None)
            {
                switch (m.Msg)
                {
                    case WM_NCPAINT:
                        DrawCaption(m.HWnd, ActiveForm == this);
                        m.Result = new IntPtr(1);
                        return;
                    case WM_NCACTIVATE:
                        if (DesignMode) break;
                        DrawCaption(m.HWnd, m.WParam.ToInt32() > 0);
                        m.Result = new IntPtr(1);
                        return;
                    //case WM_NCRBUTTONDOWN:
                    //    {
                    //        int posX, posY;
                    //        int wp = m.WParam.ToInt32();
                    //        long lp = m.LParam.ToInt64();
                    //        posX = LOBYTE(lp);
                    //        posY = HIBYTE(lp);

                    //        if (wp == HTCAPTION)
                    //        {
                    //            Point pt = this.PointToClient(new Point(posX, posY));
                    //            if (this.CaptionContextMenu != null)
                    //            {
                    //                this.CaptionContextMenu.Show(posX, posY);
                    //                return;
                    //            }
                    //        }
                    //        break;
                    //    }
                    case WM_SETCURSOR:
                        if (ControlBox)
                        {
                            int posX, posY;
                            int wp = m.WParam.ToInt32();
                            long lp = m.LParam.ToInt64();
                            posX = LOBYTE(lp);
                            posY = HIBYTE(lp);

                            _NonClientSizeInfo ncInfo = GetNonClientInfo(m.HWnd);
                            IntPtr dc = GetWindowDC(m.HWnd);

                            Graphics g = Graphics.FromHdc(dc);
                            int closeBtnPosX = ncInfo.CaptionRect.Left + ncInfo.CaptionRect.Width - ncInfo.BorderSize.Width - ncInfo.CaptionButtonSize.Width;
                            int maxBtnPosX, minBtnPosX;
                            maxBtnPosX = closeBtnPosX - ncInfo.CaptionButtonSize.Width;
                            minBtnPosX = maxBtnPosX - ncInfo.CaptionButtonSize.Width;

                            int btnPosY = ncInfo.BorderSize.Height + (ncInfo.CaptionHeight - ncInfo.CaptionButtonSize.Height) / 2 - 1;

                            Rectangle btnRect = new Rectangle(new Point(closeBtnPosX, btnPosY), ncInfo.CaptionButtonSize);
                            Rectangle maxRect = new Rectangle(new Point(maxBtnPosX, btnPosY), ncInfo.CaptionButtonSize);
                            Rectangle minRect = new Rectangle(new Point(minBtnPosX, btnPosY), ncInfo.CaptionButtonSize);

                            if (posX != HTCLOSE)
                                g.DrawImage(_imgBtnClose, btnRect);
                            else if (MouseButtons != MouseButtons.Left)
                                g.DrawImage(_imgBtnCloseHovering, btnRect);
                            else
                                g.DrawImage(_imgBtnCloseHolding, btnRect);

                            if (MaximizeBox || MinimizeBox)
                            {
                                if (FormBorderStyle != FormBorderStyle.FixedToolWindow &&
                                    FormBorderStyle != FormBorderStyle.SizableToolWindow)
                                {
                                    if (WindowState == FormWindowState.Maximized)
                                    {
                                        if (MaximizeBox)
                                        {
                                            if (posX != HTMAXBUTTON)
                                            {
                                                g.DrawImage(_imgBtnNormal, maxRect);
                                            }
                                            else if (MouseButtons != MouseButtons.Left)
                                            {
                                                g.DrawImage(_imgBtnNormalHovering, maxRect);
                                            }
                                            else
                                            {
                                                g.DrawImage(_imgBtnNormalHolding, maxRect);
                                            }
                                        }
                                        else
                                        {
                                            g.DrawImage(_imgBtnNormalDisabled, maxRect);
                                        }
                                    }
                                    else
                                    {
                                        if (MaximizeBox)
                                        {
                                            if (posX != HTMAXBUTTON)
                                            {
                                                g.DrawImage(_imgBtnMaximize, maxRect);
                                            }
                                            else if (MouseButtons != MouseButtons.Left)
                                            {
                                                g.DrawImage(_imgBtnMaximizeHovering, maxRect);
                                            }
                                            else
                                            {
                                                g.DrawImage(_imgBtnMaximizeHolding, maxRect);
                                            }
                                        }
                                        else
                                        {
                                            g.DrawImage(_imgBtnMaximizeDisabled, maxRect);
                                        }
                                    }

                                    if (MinimizeBox)
                                    {
                                        if (posX != HTMINBUTTON)
                                        {
                                            g.DrawImage(_imgBtnMinimize, minRect);
                                        }
                                        else if (MouseButtons != MouseButtons.Left)
                                        {
                                            g.DrawImage(_imgBtnMinimizeHovering, minRect);
                                        }
                                        else
                                        {
                                            g.DrawImage(_imgBtnMinimizeHolding, minRect);
                                        }
                                    }
                                    else
                                    {
                                        g.DrawImage(_imgBtnMinimize, minRect);
                                    }
                                }
                            }
                            //else if (this.HelpButton)
                            //{
                            //    if (this.FormBorderStyle != System.Windows.Forms.FormBorderStyle.FixedToolWindow &&
                            //        this.FormBorderStyle != System.Windows.Forms.FormBorderStyle.SizableToolWindow)
                            //    {
                            //        if (posX != HTHELP)
                            //        {
                            //            g.DrawImage(HelpButtonImage, maxRect);
                            //        }
                            //        else if (MouseButtons != System.Windows.Forms.MouseButtons.Left)
                            //        {
                            //            g.DrawImage(HelpButtonHoverImage, maxRect);
                            //        }
                            //        else
                            //        {
                            //            g.DrawImage(HelpButtonPressDownImage, maxRect);
                            //        }
                            //    }
                            //}

                            g.Dispose();
                            ReleaseDC(m.HWnd, dc);
                            //return;
                        }
                        break;
                    case WM_NCLBUTTONUP:
                        {
                            int wp = m.WParam.ToInt32();
                            switch (wp)
                            {
                                case HTCLOSE:
                                    m.Msg = WM_SYSCOMMAND;
                                    m.WParam = new IntPtr(SC_CLOSE);
                                    break;
                                case HTMAXBUTTON:
                                    if (MaximizeBox)
                                    {
                                        m.Msg = WM_SYSCOMMAND;
                                        if (WindowState == FormWindowState.Maximized)
                                            m.WParam = new IntPtr(SC_RESTORE);
                                        else
                                            m.WParam = new IntPtr(SC_MAXIMIZE);
                                    }
                                    break;
                                case HTMINBUTTON:
                                    if (MinimizeBox)
                                    {
                                        m.Msg = WM_SYSCOMMAND;
                                        m.WParam = new IntPtr(SC_MINIMIZE);
                                    }
                                    break;
                                default:
                                    break;
                            }
                            break;
                        }

                    case WM_NCLBUTTONDOWN:
                        if (ControlBox)
                        {
                            bool ret = false;
                            int posX, posY;
                            int wp = m.WParam.ToInt32();

                            if (wp == HTCAPTION)
                            {
                                SendMessage(Handle, WM_SYSCOMMAND, SC_MOVE | HTCAPTION, 0);
                                return;
                            }

                            long lp = m.LParam.ToInt64();
                            posX = LOBYTE(lp);
                            posY = HIBYTE(lp);

                            _NonClientSizeInfo ncInfo = GetNonClientInfo(m.HWnd);
                            IntPtr dc = GetWindowDC(m.HWnd);
                            //Brush backgroundColor = new SolidBrush(CaptionBackgroundColor);

                            Graphics g = Graphics.FromHdc(dc);
                            int closeBtnPosX = ncInfo.CaptionRect.Left + ncInfo.CaptionRect.Width - ncInfo.BorderSize.Width - ncInfo.CaptionButtonSize.Width;
                            int maxBtnPosX, minBtnPosX;
                            int btnPosY = ncInfo.BorderSize.Height + (ncInfo.CaptionHeight - ncInfo.CaptionButtonSize.Height) / 2 - 1;
                            maxBtnPosX = closeBtnPosX - ncInfo.CaptionButtonSize.Width;
                            minBtnPosX = maxBtnPosX - ncInfo.CaptionButtonSize.Width;

                            Rectangle btnRect = new Rectangle(new Point(closeBtnPosX, btnPosY), ncInfo.CaptionButtonSize);
                            Rectangle maxRect = new Rectangle(new Point(maxBtnPosX, btnPosY), ncInfo.CaptionButtonSize);
                            Rectangle minRect = new Rectangle(new Point(minBtnPosX, btnPosY), ncInfo.CaptionButtonSize);

                            if (wp == HTCLOSE)
                            {
                                g.DrawImage(_imgBtnCloseHolding, btnRect);
                                ret = true;
                            }
                            else
                                g.DrawImage(_imgBtnClose, btnRect);

                            if (MaximizeBox || MinimizeBox)
                            {
                                if (FormBorderStyle != FormBorderStyle.SizableToolWindow &&
                                    FormBorderStyle != FormBorderStyle.FixedToolWindow)
                                {
                                    if (WindowState == FormWindowState.Maximized)
                                    {
                                        if (wp == HTMAXBUTTON && MaximizeBox)
                                        {
                                            minBtnPosX = maxBtnPosX - ncInfo.CaptionButtonSize.Width;
                                            g.DrawImage(_imgBtnNormalHolding, maxRect);
                                            ret = true;
                                        }
                                        else
                                            g.DrawImage(MaximizeBox ? _imgBtnNormal : _imgBtnNormalDisabled, maxRect);
                                    }
                                    else
                                    {
                                        if (wp == HTMAXBUTTON && MaximizeBox)
                                        {
                                            minBtnPosX = maxBtnPosX - ncInfo.CaptionButtonSize.Width;
                                            g.DrawImage(_imgBtnMaximizeHolding, maxRect);
                                            ret = true;
                                        }
                                        else
                                            g.DrawImage(MaximizeBox ? _imgBtnMaximize : _imgBtnMaximizeDisabled, maxRect);
                                    }
                                    if (wp == HTMINBUTTON && MinimizeBox)
                                    {
                                        g.DrawImage(_imgBtnMinimizeHolding, minRect);
                                        ret = true;
                                    }
                                    else
                                        g.DrawImage(_imgBtnMinimize, minRect);
                                }
                            }

                            g.Dispose();
                            ReleaseDC(m.HWnd, dc);

                            if (ret) return;
                        }
                        break;
                }
            }

            base.WndProc(ref m);
        }

        #endregion 重写
    }
}
