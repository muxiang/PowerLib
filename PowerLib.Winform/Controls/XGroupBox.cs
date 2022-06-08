using System;
using System.ComponentModel;
using System.Drawing;
using System.Drawing.Drawing2D;
using System.Windows.Forms;

namespace PowerLib.Winform.Controls
{
    /// <summary>
    /// 表示一个 Windows 控件，该控件显示围绕一组具有可选标题的控件的框架。
    /// </summary>
    public sealed partial class XGroupBox : GroupBox
    {
        // 缓存画笔画刷
        private SolidBrush _brsTitle;
        private SolidBrush _brsTitleMark;

        private Font _titleFont;
        private bool _showTitleMark = true;
        private Color _titleMarkColor = Color.FromArgb(66, 215, 250);
        private Color _borderColor = Color.Silver;
        private Pen _penBorder;
        private int _borderWidth;
        
        /// <summary>
        /// 标题字体
        /// </summary>
        [Browsable(true)]
        [Category("Appearance")]
        [Description("指定标题的字体")]
        public Font TitleFont
        {
            get => _titleFont;
            set
            {
                _titleFont = value;
                Refresh();
            }
        }

        /// <summary>
        /// 指定一个值标识是否显示标题标记
        /// </summary>
        [Browsable(true)]
        [Category("Appearance")]
        [Description("指定一个值标识是否显示标题标记")]
        [DefaultValue(true)]
        public bool ShowTitleMark
        {
            get => _showTitleMark;
            set
            {
                _showTitleMark = value;
                Refresh();
            }
        }

        /// <summary>
        /// 标题标记的颜色
        /// </summary>
        [Browsable(true)]
        [Category("Appearance")]
        [Description("指定标题标记的颜色")]
        [DefaultValue(typeof(Color), "66, 215, 250")]
        public Color TitleMarkColor
        {
            get => _titleMarkColor;
            set
            {
                _titleMarkColor = value;
                _brsTitleMark = new SolidBrush(_titleMarkColor);
                Refresh();
            }
        }

        /// <summary>
        /// 边框颜色
        /// </summary>
        [Browsable(true)]
        [Category("Appearance")]
        [Description("边框颜色")]
        [DefaultValue(typeof(Color), "212, 212, 212")]
        public Color BorderColor
        {
            get => _borderColor;
            set
            {
                _borderColor = value;
                _penBorder = new Pen(value, _borderWidth);
                Refresh();
            }
        }

        /// <summary>
        /// 边框宽度
        /// </summary>
        [Browsable(true)]
        [Category("Appearance")]
        [Description("边框宽度")]
        [DefaultValue(0)]
        public int BorderWidth
        {
            get => _borderWidth;
            set
            {
                _borderWidth = value;
                if (value > 0)
                    _penBorder = new Pen(_borderColor, value);

                Refresh();
            }
        }

        /// <summary>
        /// 初始化<see cref="XGroupBox"/>的实例
        /// </summary>
        public XGroupBox()
        {
            InitializeComponent();

            _titleFont = new Font(Font.FontFamily, Font.SizeInPoints * 1.5F, FontStyle.Bold);
            _brsTitle = new SolidBrush(ForeColor);
            _brsTitleMark = new SolidBrush(_titleMarkColor);
        }

        /// <inheritdoc />
        public override Rectangle DisplayRectangle
        {
            get
            {
                Size size = ClientSize;
                Padding padding = Padding;
                return new Rectangle(
                    padding.Left,
                    _titleFont.Height + padding.Top,
                    Math.Max(size.Width - padding.Horizontal, 0),
                    Math.Max(size.Height - _titleFont.Height - padding.Vertical, 0));
            }
        }

        /// <inheritdoc />
        protected override void OnPaint(PaintEventArgs pe)
        {
            pe.Graphics.PixelOffsetMode = PixelOffsetMode.HighQuality;
            pe.Graphics.SmoothingMode = SmoothingMode.AntiAlias;

            string strText = Text;

            const int titleX = 10;

            pe.Graphics.FillRectangle(_brsTitleMark, new Rectangle(0, 0, 4, _titleFont.Height));
            pe.Graphics.DrawString(strText, _titleFont, _brsTitle, new PointF(titleX, 0));

            int width = Width;
            int height = Height;

            if (_borderWidth > 0)
            {
                // left
                pe.Graphics.DrawLine(_penBorder, 0, _titleFont.Height, 0, height - _borderWidth);
                // bottom
                pe.Graphics.DrawLine(_penBorder, 0, height - _borderWidth, width - _borderWidth, height - _borderWidth);
                // right
                pe.Graphics.DrawLine(_penBorder, width - _borderWidth, _titleFont.Height / 2,
                    width - _borderWidth, height - _borderWidth);

                SizeF szTitle = pe.Graphics.MeasureString(strText, _titleFont);

                // top
                pe.Graphics.DrawLine(_penBorder,
                    titleX + szTitle.Width,
                    _titleFont.Height / 2F,
                    width - _borderWidth,
                    _titleFont.Height / 2F);
            }
        }
    }
}
