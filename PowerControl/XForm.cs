using System;
using System.ComponentModel;
using System.Diagnostics;
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
            //关闭按钮
            _imgBtnClose = GetNormalImage(Properties.Resources.btnClose);
            _imgBtnCloseHovering = GetHoveringImage(Properties.Resources.btnClose);
            _imgBtnCloseHolding = GetHoldingImage(Properties.Resources.btnClose);

            //最小化按钮
            _imgBtnMinimize = GetNormalImage(Properties.Resources.btnMinimize);
            _imgBtnMinimizeHovering = GetHoveringImage(Properties.Resources.btnMinimize);
            _imgBtnMinimizeHolding = GetHoldingImage(Properties.Resources.btnMinimize);

            //最大化按钮
            _imgBtnMaximize = GetNormalImage(Properties.Resources.btnMaximize);
            _imgBtnMaximizeDisabled = GetNormalImage(Properties.Resources.btnMaximizeDisabled);
            _imgBtnMaximizeHovering = GetHoveringImage(Properties.Resources.btnMaximize);
            _imgBtnMaximizeHolding = GetHoldingImage(Properties.Resources.btnMaximize);

            //取消最大化按钮
            _imgBtnNormal = GetNormalImage(Properties.Resources.btnNormal);
            _imgBtnNormalDisabled = GetNormalImage(Properties.Resources.btnNormalDisabled);
            _imgBtnNormalHovering = GetHoveringImage(Properties.Resources.btnNormal);
            _imgBtnNormalHolding = GetHoldingImage(Properties.Resources.btnNormal);
        }

        /// <summary>
        /// 获取常规状态按钮图标
        /// </summary>
        /// <param name="bmp">基本图标</param>
        /// <returns></returns>
        private Image GetNormalImage(Bitmap bmp)
        {
            Bitmap bmpNew = new Bitmap(bmp.Width, bmp.Height);
            using (Graphics g = Graphics.FromImage(bmpNew))
            {
                LinearGradientBrush brs = new LinearGradientBrush(
                    new Point(bmp.Width / 2, bmp.Height),
                    new Point(bmp.Width / 2, 0),
                    BackColor,
                    Utilities.GetLighterColor(BackColor, 60));

                g.FillRectangle(brs, new Rectangle(0, 0, bmp.Width, bmp.Height));
                g.DrawImage(bmp, new Rectangle(0, 0, bmp.Width, bmp.Height));
            }

            return bmpNew;
        }

        /// <summary>
        /// 获取鼠标悬浮状态按钮图标
        /// </summary>
        /// <param name="bmp">基本图标</param>
        /// <returns></returns>
        private Image GetHoveringImage(Bitmap bmp)
        {
            Bitmap bmpNew = new Bitmap(bmp.Width, bmp.Height);
            using (Graphics g = Graphics.FromImage(bmpNew))
            {
                LinearGradientBrush brs = new LinearGradientBrush(
                    new Point(bmp.Width / 2, bmp.Height),
                    new Point(bmp.Width / 2, 0),
                    Utilities.GetLighterColor(BackColor, 30),
                    Utilities.GetLighterColor(BackColor, 90));

                g.FillRectangle(brs, new Rectangle(0, 0, bmp.Width, bmp.Height));
                g.DrawImage(bmp, new Rectangle(0, 0, bmp.Width, bmp.Height));
            }

            return bmpNew;
        }

        /// <summary>
        /// 获取鼠标按下状态按钮图标
        /// </summary>
        /// <param name="bmp">基本图标</param>
        /// <returns></returns>
        private Image GetHoldingImage(Bitmap bmp)
        {
            Bitmap bmpNew = new Bitmap(bmp.Width, bmp.Height);
            using (Graphics g = Graphics.FromImage(bmpNew))
            {
                LinearGradientBrush brs = new LinearGradientBrush(
                    new Point(bmp.Width / 2, bmp.Height),
                    new Point(bmp.Width / 2, 0),
                    Utilities.GetDeeperColor(BackColor, 60),
                    BackColor);

                g.FillRectangle(brs, new Rectangle(0, 0, bmp.Width, bmp.Height));
                g.DrawImage(bmp, new Rectangle(0, 0, bmp.Width, bmp.Height));
            }

            return bmpNew;
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
            Invalidate();

            IntPtr dc;
            Graphics g;
            Size iconSize;
            _NonClientSizeInfo ncInfo;

            iconSize = SystemInformation.SmallIconSize;

            dc = GetWindowDC(hwnd);
            ncInfo = GetNonClientInfo(hwnd);
            g = Graphics.FromHdc(dc);

            Rectangle rect = ncInfo.CaptionRect;
            g.FillRectangle(new LinearGradientBrush(
                new Point(rect.Width / 2, rect.Height + rect.Y), new Point(rect.Width / 2, 0),
                BackColor,
                Utilities.GetLighterColor(BackColor, 60)),
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
                int closeBtnPosX = info.CaptionRect.Left + info.CaptionRect.Width - info.BorderSize.Width - info.CaptionButtonSize.Width;
                int maxBtnPosX = closeBtnPosX - info.CaptionButtonSize.Width;
                int minBtnPosX = maxBtnPosX - info.CaptionButtonSize.Width;
                int btnPosY = info.BorderSize.Height + (info.CaptionHeight - info.CaptionButtonSize.Height) / 2 - 1;

                Rectangle btnRect = new Rectangle(new Point(closeBtnPosX, btnPosY), info.CaptionButtonSize);
                Rectangle maxRect = new Rectangle(new Point(maxBtnPosX, btnPosY), info.CaptionButtonSize);
                Rectangle minRect = new Rectangle(new Point(minBtnPosX, btnPosY), info.CaptionButtonSize);

                Brush backgroundColor = new SolidBrush(BackColor);

                //g.FillRectangle(backgroundColor, btnRect);
                //g.FillRectangle(backgroundColor, maxRect);
                //g.FillRectangle(backgroundColor, minRect);

                g.DrawImage(_imgBtnClose, btnRect);

                if (this.MaximizeBox || this.MinimizeBox)
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
                g.DrawIcon(Icon, new Rectangle(new Point(ncInfo.BorderSize.Width, ncInfo.BorderSize.Height + (ncInfo.CaptionHeight - iconSize.Height) / 2), iconSize));
                titleX = ncInfo.BorderSize.Width + iconSize.Width + ncInfo.BorderSize.Width;
            }
            else
            {
                titleX = ncInfo.BorderSize.Width;
            }

            SizeF captionTitleSize = g.MeasureString(Text, SystemFonts.CaptionFont);
            g.DrawString(Text, SystemFonts.CaptionFont, new SolidBrush(Color.White),
                    new RectangleF(titleX,
                        (ncInfo.BorderSize.Height + ncInfo.CaptionHeight - captionTitleSize.Height) / 2,
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

            LinearGradientBrush brs = new LinearGradientBrush(
                new Point(borderTop.X + borderTop.Width / 2, borderTop.Y),
                new Point(borderTop.X + borderTop.Width / 2, borderTop.Bottom),
                Utilities.GetLighterColor(BackColor, 128),
                //BackColor);
                Utilities.GetLighterColor(BackColor, 40));

            //SolidBrush brs = new SolidBrush(Utilities.GetLighterColor(BackColor, 20));
            //LinearGradientBrush brs = new LinearGradientBrush(
            //    new Point(borderTop.Left + borderTop.Width / 2, 0),
            //    new Point(borderTop.Left + borderTop.Width / 2, borderTop.Height),
            //    BackColor, Utilities.GetLighterColor(BackColor, 60));

            // top border  
            g.FillRectangle(brs, borderTop);

            //brs = new LinearGradientBrush(
            //    new Point(borderLeft.Right, borderLeft.Top + borderLeft.Height / 2),
            //    new Point(borderLeft.Left, borderLeft.Top + borderLeft.Height / 2),
            //    BackColor, Utilities.GetLighterColor(BackColor, 60));
            // left border  
            brs.Dispose();
            brs = new LinearGradientBrush(
                new Point(borderLeft.X, borderLeft.Y + borderLeft.Height / 2),
                new Point(borderLeft.Right, borderLeft.Y + borderLeft.Height / 2),
                Utilities.GetLighterColor(BackColor, 128),
                BackColor);
            g.FillRectangle(brs, borderLeft);

            //brs = new LinearGradientBrush(
            //    new Point(borderRight.Left, borderRight.Top + borderRight.Height / 2),
            //    new Point(borderRight.Right, borderRight.Top + borderRight.Height / 2),
            //    BackColor, Utilities.GetLighterColor(BackColor, 60));
            // right border
            brs.Dispose();
            brs = new LinearGradientBrush(
                new Point(borderRight.X, borderRight.Y + borderRight.Height / 2),
                new Point(borderRight.Right, borderRight.Y + borderRight.Height / 2),
                BackColor,
                Utilities.GetLighterColor(BackColor, 128));
            g.FillRectangle(brs, borderRight);
            //brs = new LinearGradientBrush(
            //    new Point(borderBottom.Left + borderBottom.Width / 2, 0),
            //    new Point(borderBottom.Left + borderBottom.Width / 2, borderBottom.Height),
            //    BackColor, Utilities.GetLighterColor(BackColor, 60));
            // bottom border
            brs.Dispose();
            brs = new LinearGradientBrush(
                new Point(borderBottom.X + borderBottom.Width / 2, borderBottom.Y),
                new Point(borderBottom.X + borderBottom.Width / 2, borderBottom.Bottom),
                BackColor,
                Utilities.GetLighterColor(BackColor, 128));
            g.FillRectangle(brs, borderBottom);
        }

        private _NonClientSizeInfo GetNonClientInfo(IntPtr hwnd)
        {
            _NonClientSizeInfo info = new _NonClientSizeInfo();
            info.CaptionButtonSize = SystemInformation.CaptionButtonSize;
            info.CaptionHeight = SystemInformation.CaptionHeight;

            switch (FormBorderStyle)
            {
                case FormBorderStyle.Fixed3D:
                    info.BorderSize = SystemInformation.FixedFrameBorderSize;
                    info.BorderSize = new Size(
                        info.BorderSize.Width + 5,
                        info.BorderSize.Height + 5);
                    break;
                case FormBorderStyle.FixedDialog:
                    info.BorderSize = SystemInformation.FixedFrameBorderSize;
                    info.BorderSize = new Size(
                        info.BorderSize.Width + 5,
                        info.BorderSize.Height + 5);
                    break;
                case FormBorderStyle.FixedSingle:
                    info.BorderSize = SystemInformation.FixedFrameBorderSize;
                    info.BorderSize = new Size(
                        info.BorderSize.Width + 5,
                        info.BorderSize.Height + 5);
                    break;
                case FormBorderStyle.FixedToolWindow:
                    info.BorderSize = SystemInformation.FixedFrameBorderSize;
                    info.BorderSize = new Size(
                        info.BorderSize.Width + 5,
                        info.BorderSize.Height + 5);
                    info.CaptionButtonSize = SystemInformation.ToolWindowCaptionButtonSize;
                    info.CaptionHeight = SystemInformation.ToolWindowCaptionHeight;
                    break;
                case FormBorderStyle.Sizable:
                    info.BorderSize = SystemInformation.FrameBorderSize;
                    info.BorderSize = new Size(
                        info.BorderSize.Width + 4,
                        info.BorderSize.Height + 4);
                    break;
                case FormBorderStyle.SizableToolWindow:
                    info.CaptionButtonSize = SystemInformation.ToolWindowCaptionButtonSize;
                    info.BorderSize = SystemInformation.FrameBorderSize;
                    info.BorderSize = new Size(
                        info.BorderSize.Width + 4,
                        info.BorderSize.Height + 4);
                    info.CaptionHeight = SystemInformation.ToolWindowCaptionHeight;
                    break;
                default:
                    info.BorderSize = SystemInformation.BorderSize;
                    break;
            }

            //HACK: ri le gou le、
            //string strRuntimeVersion = Assembly.GetEntryAssembly().ImageRuntimeVersion;
            //if (Environment.Version >= new Version(4, 0, 30319, 42000))

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

            CreateButtonImages();
            Invalidate();
        }

        /// <summary>
        /// 文本变更时发生
        /// </summary>
        /// <param name="e"></param>
        protected override void OnTextChanged(EventArgs e)
        {
            base.OnTextChanged(e);
            DrawCaption(Handle, true);
        }

        private int LOBYTE(long p) { return (int)(p & 0x0000FFFF); }
        private int HIBYTE(long p) { return (int)(p >> 16); }

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

                    //        //if (wp == HTCAPTION)
                    //        //{
                    //        //    Point pt = this.PointToClient(new Point(posX, posY));
                    //        //    if (this.CaptionContextMenu != null)
                    //        //    {
                    //        //        this.CaptionContextMenu.Show(posX, posY);
                    //        //        return;
                    //        //    }
                    //        //}
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

                            Brush backgroundColor = new SolidBrush(BackColor);
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

                            //g.FillRectangle(backgroundColor, btnRect);
                            //g.FillRectangle(backgroundColor, maxRect);
                            //g.FillRectangle(backgroundColor, minRect);

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
                                        {
                                            m.WParam = new IntPtr(SC_RESTORE);
                                        }
                                        else
                                        {
                                            m.WParam = new IntPtr(SC_MAXIMIZE);
                                        }
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

                            //g.FillRectangle(backgroundColor, btnRect);
                            //g.FillRectangle(backgroundColor, maxRect);
                            //g.FillRectangle(backgroundColor, minRect);

                            if (wp == HTCLOSE)
                            {
                                g.DrawImage(_imgBtnCloseHolding, btnRect);
                                ret = true;
                            }
                            else
                            {
                                g.DrawImage(_imgBtnClose, btnRect);
                            }

                            if (MaximizeBox || MinimizeBox)
                            {
                                if (FormBorderStyle != FormBorderStyle.SizableToolWindow &&
                                    FormBorderStyle != FormBorderStyle.FixedToolWindow)
                                {
                                    if (this.WindowState == FormWindowState.Maximized)
                                    {
                                        if (wp == HTMAXBUTTON && MaximizeBox)
                                        {
                                            minBtnPosX = maxBtnPosX - ncInfo.CaptionButtonSize.Width;
                                            g.DrawImage(_imgBtnNormalHolding, maxRect);
                                            ret = true;
                                        }
                                        else
                                        {
                                            g.DrawImage(MaximizeBox ? _imgBtnNormal : _imgBtnNormalDisabled, maxRect);
                                        }
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
                                        {
                                            g.DrawImage(MaximizeBox ? _imgBtnMaximize : _imgBtnMaximizeDisabled, maxRect);
                                        }
                                    }
                                    if (wp == HTMINBUTTON && MinimizeBox)
                                    {
                                        g.DrawImage(_imgBtnMinimizeHolding, minRect);
                                        ret = true;
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
                            //        if (wp == HTHELP)
                            //        {
                            //            g.DrawImage(HelpButtonPressDownImage, maxRect);
                            //            ret = true;
                            //        }
                            //        else
                            //        {
                            //            g.DrawImage(HelpButtonImage, maxRect);
                            //        }
                            //    }
                            //}

                            g.Dispose();
                            ReleaseDC(m.HWnd, dc);

                            if (ret)
                                return;
                        }
                        break;
                }
            }

            base.WndProc(ref m);
        }

        #endregion 重写
    }
}
