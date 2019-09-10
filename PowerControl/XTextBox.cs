using System.ComponentModel;
using System.Drawing;
using System.Drawing.Drawing2D;
using System.Windows.Forms;

namespace PowerControl
{
    public partial class XTextBox : TextBox
    {
        private Color _borderColor;
        private Pen _borderPen = Pens.Black;

        public XTextBox()
        {
            InitializeComponent();
            BorderColor = Color.FromArgb(184, 184, 184);
            ForeColor = BorderColor;
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

        protected override void WndProc(ref Message m)
        {
            base.WndProc(ref m);

            if (m.Msg != NativeConstants.WM_PAINT || BorderStyle == BorderStyle.None)
                return;

            Graphics g = Graphics.FromHwnd(Handle);

            g.InterpolationMode = InterpolationMode.HighQualityBicubic;
            g.SmoothingMode = SmoothingMode.AntiAlias;
            g.DrawRectangle(_borderPen, new Rectangle(0, 0, Width - 1, Height - 1));
        }
    }
}
