using System;
using System.ComponentModel;
using System.Drawing;
using System.Drawing.Drawing2D;
using System.Windows.Forms;

namespace PowerControl
{
    public sealed partial class XTextBox : TextBox
    {
        private Color _borderColor;
        private Pen _borderPen = Pens.Black;

        private Color _focusedBorderColor;
        private Pen _focusedBorderPen = Pens.Black;

        public XTextBox()
        {
            InitializeComponent();

            SetStyle(ControlStyles.OptimizedDoubleBuffer, true);

            BorderColor = Color.FromArgb(184, 184, 184);
            FocusedBorderColor = Color.FromArgb(66, 215, 250);
            ForeColor = Color.FromArgb(80, 80, 80);
        }

        /// <summary>
        /// 边框颜色
        /// </summary>
        [Browsable(true)]
        [Category("Appearance")]
        [Description("边框颜色")]
        public Color BorderColor
        {
            get => _borderColor;
            set
            {
                _borderColor = value;
                _borderPen = new Pen(value, 1);
                Invalidate();
            }
        }

        /// <summary>
        /// 焦点边框颜色
        /// </summary>
        [Browsable(true)]
        [Category("Appearance")]
        [Description("焦点边框颜色")]
        public Color FocusedBorderColor
        {
            get => _focusedBorderColor;
            set
            {
                _focusedBorderColor = value;
                _focusedBorderPen = new Pen(value, 1.5F);
                Invalidate();
            }
        }

        protected override void OnGotFocus(EventArgs e)
        {
            base.OnGotFocus(e);
            Invalidate();
        }

        protected override void OnLostFocus(EventArgs e)
        {
            base.OnLostFocus(e);
            Invalidate();
        }

        protected override void WndProc(ref Message m)
        {
            base.WndProc(ref m);

            if (m.Msg != NativeConstants.WM_PAINT || BorderStyle == BorderStyle.None)
                return;

            Graphics g = Graphics.FromHwnd(Handle);

            g.InterpolationMode = InterpolationMode.HighQualityBicubic;
            g.SmoothingMode = SmoothingMode.AntiAlias;
            g.DrawRectangle(Focused ? _focusedBorderPen : _borderPen, new Rectangle(0, 0, Width - 1, Height - 1));
        }
    }
}
