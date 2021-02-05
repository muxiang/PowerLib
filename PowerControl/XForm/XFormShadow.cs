using System;
using System.ComponentModel;
using System.Drawing;
using System.Windows.Forms;

using static PowerControl.NativeMethods;
using static PowerControl.NativeConstants;
using static PowerControl.NativeStructures;

namespace PowerControl
{
    /// <summary>
    /// 为<see cref="XForm"/>阴影背景提供支持
    /// </summary>
    internal sealed class XFormShadow : Form
    {
        #region 字段

        // 背景图片
        private readonly Bitmap _background;

        #endregion 字段
        
        #region 构造

        public XFormShadow(Color backColor, Size size)
            : this(Utilities.CreateBitmap(127, backColor, size)) { }

        public XFormShadow(Bitmap bg)
        {
            if (bg == null)
                throw new ArgumentNullException(nameof(bg));

            Size = bg.Size;
            _background = bg;

            ShowInTaskbar = false;
        }

        #endregion 构造

        #region 方法

        private void InitializeStyles()
        {
            SetStyle(ControlStyles.AllPaintingInWmPaint | ControlStyles.UserPaint, true);
            UpdateStyles();
        }

        private void UpdateBmp(Bitmap bmp)
        {
            if (!IsHandleCreated) return;

            if (!Image.IsCanonicalPixelFormat(bmp.PixelFormat) || !Image.IsAlphaPixelFormat(bmp.PixelFormat))
                throw new ArgumentException(@"位图格式不正确", nameof(bmp));

            IntPtr oldBits = IntPtr.Zero;
            IntPtr screenDC = GetDC(IntPtr.Zero);
            IntPtr hBmp = IntPtr.Zero;
            IntPtr memDc = CreateCompatibleDC(screenDC);

            try
            {
                POINT formLocation = new POINT(Left, Top);
                SIZE bitmapSize = new SIZE(bmp.Width, bmp.Height);
                BLENDFUNCTION blendFunc = new BLENDFUNCTION(
                    AC_SRC_OVER,
                    0,
                    255,
                    AC_SRC_ALPHA);

                POINT srcLoc = new POINT(0, 0);

                hBmp = bmp.GetHbitmap(Color.FromArgb(0));
                oldBits = SelectObject(memDc, hBmp);

                UpdateLayeredWindow(
                    Handle,
                    screenDC,
                    ref formLocation,
                    ref bitmapSize,
                    memDc,
                    ref srcLoc,
                    0,
                    ref blendFunc,
                    ULW_ALPHA);
            }
            finally
            {
                if (hBmp != IntPtr.Zero)
                {
                    SelectObject(memDc, oldBits);
                    DeleteObject(hBmp);
                }

                ReleaseDC(IntPtr.Zero, screenDC);
                DeleteDC(memDc);
            }
        }

        #endregion 方法

        #region 重写

        protected override CreateParams CreateParams
        {
            get
            {
                CreateParams cParms = base.CreateParams;
                cParms.ExStyle |= (int)WS_EX_LAYERED | (int)WS_EX_NOACTIVATE;
                return DesignMode ? base.CreateParams : cParms;
            }
        }

        protected override void OnHandleCreated(EventArgs e)
        {
            InitializeStyles();
            base.OnHandleCreated(e);
        }

        protected override void OnHandleDestroyed(EventArgs e)
        {
            _background.Dispose();
            base.OnHandleDestroyed(e);
        }

        protected override void OnLoad(EventArgs e)
        {
            UpdateBmp(_background);
            base.OnLoad(e);
        }

        protected override void OnClosing(CancelEventArgs e)
        {
            e.Cancel = true;
            base.OnClosing(e);
        }

        protected override void OnClientSizeChanged(EventArgs e)
        {
            if (FormBorderStyle != FormBorderStyle.None)
                FormBorderStyle = FormBorderStyle.None;

            base.OnClientSizeChanged(e);
        }

        #endregion 重写
    }
}
