using System;
using System.ComponentModel;
using System.Drawing;
using System.Drawing.Drawing2D;
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

        /// <summary>
        /// 获取或设置进度条的最小值
        /// </summary>
        public int Min
        {
            get => _min;
            set
            {
                _min = value;
                Invalidate();
            }
        }

        /// <summary>
        /// 获取或设置进度条的最大值
        /// </summary>
        public int Max
        {
            get => _max;
            set
            {
                _max = value;
                Invalidate();
            }
        }
        
        /// <summary>
        /// 获取或设置进度条的值
        /// </summary>
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
        /// 获取或设置一个表示进度条文本是否描边的值
        /// </summary>
        [DefaultValue(false)]
        public bool TextOutLine { get; set; } = false;

        /// <summary>
        /// 初始化进度条的实例
        /// </summary>
        public XProgressBar()
        {
            InitializeComponent();

            SetStyle(ControlStyles.DoubleBuffer | ControlStyles.AllPaintingInWmPaint | ControlStyles.UserPaint, true);

            BackColor = Color.FromArgb(0x3C, 0x3C, 0x3C);
            ForeColor = Color.FromArgb(0, 150, 170);

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
                    Utilities.GetLighterColor(ForeColor, 60), ForeColor);

                pe.Graphics.FillRectangle(brs, 0, 0, (float)drawWidth, Height);
            }

            string text = $"{Text}   {Math.Round(percentage, 2)}%";
            SizeF szText = pe.Graphics.MeasureString(text, Font, Size, StringFormat.GenericTypographic);

            if (TextOutLine)
            {
                using (GraphicsPath path = new GraphicsPath())
                {
                    path.AddString(text,
                        Font.FontFamily,
                        (int)Font.Style,
                        Font.SizeInPoints / 72 * pe.Graphics.DpiX,
                        new PointF(Width / 2F - szText.Width / 2F, Height / 2F - szText.Height / 2F),
                        StringFormat.GenericTypographic);

                    pe.Graphics.FillPath(_brsBack, path);
                    pe.Graphics.DrawPath(_penFore, path);
                }
            }
            else
            {
                pe.Graphics.DrawString(Text, Font,
                    Brushes.White,
                    new PointF(Width / 2F - szText.Width / 2F, Height / 2F - szText.Height / 2F),
                    StringFormat.GenericTypographic);
            }

            base.OnPaint(pe);
        }
    }
}
