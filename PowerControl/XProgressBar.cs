using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Drawing.Drawing2D;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace PowerControl
{
    /// <summary>
    /// 表示一个支持显示文本的进度条
    /// </summary>
    public sealed partial class XProgressBar : Control
    {
        private int _min;
        private int _max = 100;
        private int _value;

        private Brush _brsBack;
        private Pen _penFore;
        
        public int Min
        {
            get => _min;
            set
            {
                _min = value;
                Invalidate();
            }
        }

        public int Max
        {
            get => _max;
            set
            {
                _max = value;
                Invalidate();
            }
        }

        public int Value
        {
            get => _value;
            set
            {
                _value = value;
                Invalidate();
            }
        }

        /// <summary>
        /// 初始化进度条的实例
        /// </summary>
        public XProgressBar()
        {
            InitializeComponent();

            SetStyle(ControlStyles.DoubleBuffer | ControlStyles.AllPaintingInWmPaint | ControlStyles.UserPaint, true);

            BackColor = Color.White;
            ForeColor = Color.FromArgb(12, 70, 222);

            Font = new Font("微软雅黑", 10, FontStyle.Bold);
        }

        protected override void OnBackColorChanged(EventArgs e)
        {
            base.OnBackColorChanged(e);
            _brsBack = new SolidBrush(BackColor);
            Invalidate();
        }

        protected override void OnForeColorChanged(EventArgs e)
        {
            base.OnForeColorChanged(e);
            _penFore = new Pen(ForeColor, 1.5F);
            Invalidate();
        }

        protected override void OnTextChanged(EventArgs e)
        {
            base.OnTextChanged(e);
            Invalidate();
        }

        protected override void OnPaint(PaintEventArgs pe)
        {
            pe.Graphics.SmoothingMode = SmoothingMode.HighQuality;
            pe.Graphics.PixelOffsetMode = PixelOffsetMode.HighQuality;

            pe.Graphics.Clear(BackColor);

            double percentage = Value / (double)Max * 100;

            double drawWidth = Width / 100D * percentage;

            if (drawWidth > 0)
            {
                Brush brs = new LinearGradientBrush(new PointF(0, Height / 2F),
                    new PointF((float)drawWidth, Height / 2F),
                    ForeColor, Utilities.GetLighterColor(ForeColor, 60));
                
                pe.Graphics.FillRectangle(brs, 0, 0, (float)drawWidth, Height);
            }

            using (GraphicsPath path = new GraphicsPath())
            {
                string text = $"{Text}   {Math.Round(percentage, 2)}%";
                SizeF szText = pe.Graphics.MeasureString(text, Font, Size, StringFormat.GenericTypographic);

                path.AddString(text,
                    Font.FontFamily,
                    (int)Font.Style,
                    Font.SizeInPoints / 72 * pe.Graphics.DpiX,
                    new PointF(Width / 2F - szText.Width / 2F, Height / 2F - szText.Height / 2F),
                    StringFormat.GenericTypographic);

                pe.Graphics.FillPath(_brsBack, path);
                pe.Graphics.DrawPath(_penFore, path);
            }

            base.OnPaint(pe);
        }
    }
}
