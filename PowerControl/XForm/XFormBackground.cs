using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;

using static PowerControl.NativeMethods;
using static PowerControl.NativeConstants;
using static PowerControl.NativeStructures;

namespace PowerControl
{
    /// <summary>
    /// 为<see cref="XForm"/>阴影背景提供支持
    /// </summary>
    internal sealed partial class XFormBackground : Form
    {
        #region Fields(Instance)

        //Background image
        private readonly Bitmap _background;

        #endregion Fields(Instance)

        #region Properties

        public bool HaveHandle { get; private set; }

        #endregion Properties

        #region Constructors

        public XFormBackground(Color backColor, Size size)
            : this(Utilities.CreateBitmap(127, backColor, size)) { }

        public XFormBackground(Bitmap bg)
        {
            if (bg == null)
                throw new ArgumentNullException(nameof(bg));

            Size = bg.Size;
            _background = bg;

            ShowInTaskbar = false;
        }

        #endregion Constructors

        #region Methods(Instance)

        private void InitializeStyles()
        {
            SetStyle(ControlStyles.AllPaintingInWmPaint | ControlStyles.UserPaint, true);
            UpdateStyles();
        }

        private void UpdateBmp(Bitmap bmp)
        {
            if (!HaveHandle) return;

            //Verify bitmap
            if (!Image.IsCanonicalPixelFormat(bmp.PixelFormat) || !Image.IsAlphaPixelFormat(bmp.PixelFormat))
                throw new ArgumentException(@"Required 32 bits Alpha Bitmap", nameof(bmp));

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

        #endregion Methods(Instance)

        #region Override

        protected override void OnClosing(CancelEventArgs e)
        {
            e.Cancel = true;
            base.OnClosing(e);
            HaveHandle = false;
        }

        protected override void OnHandleCreated(EventArgs e)
        {
            InitializeStyles();
            base.OnHandleCreated(e);
            HaveHandle = true;
        }

        protected override CreateParams CreateParams
        {
            get
            {
                CreateParams cParms = base.CreateParams;
                cParms.ExStyle |= WS_EX_LAYERED;
                return DesignMode ? base.CreateParams : cParms;
            }
        }

        protected override void OnClientSizeChanged(EventArgs e)
        {
            if (FormBorderStyle != FormBorderStyle.None)
                FormBorderStyle = FormBorderStyle.None;

            base.OnClientSizeChanged(e);
        }

        protected override void OnLoad(EventArgs e)
        {
            UpdateBmp(_background);

            base.OnLoad(e);
        }

        protected override void OnHandleDestroyed(EventArgs e)
        {
            _background.Dispose();
            base.OnHandleDestroyed(e);
        }

        #endregion Override
    }
}
