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
    public partial class XGroupBox : GroupBox
    {
        //缓存画笔画刷
        private SolidBrush _brsTitle;
        private SolidBrush _brsTitleMark;

        private Font _titleFont;
        private bool _showTitleMark = true;
        private Color _titleMarkColor = Color.FromArgb(66, 215, 250);

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
                Invalidate();
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
                Invalidate();
            }
        }

        /// <summary>
        /// 标题标记的颜色
        /// </summary>
        [Browsable(true)]
        [Category("Appearance")]
        [Description("指定标题标记的颜色")]
        public Color TitleMarkColor
        {
            get => _titleMarkColor;
            set
            {
                _titleMarkColor = value;
                _brsTitleMark = new SolidBrush(_titleMarkColor);
                Invalidate();
            }
        }


        public XGroupBox()
        {
            InitializeComponent();

            _titleFont = new Font(Font.FontFamily, Font.SizeInPoints * 1.5F, FontStyle.Bold);
            _brsTitle = new SolidBrush(ForeColor);
            _brsTitleMark = new SolidBrush(_titleMarkColor);
        }

        protected override void OnPaint(PaintEventArgs pe)
        {
            pe.Graphics.PixelOffsetMode = PixelOffsetMode.HighQuality;
            pe.Graphics.SmoothingMode = SmoothingMode.AntiAlias;

            pe.Graphics.FillRectangle(_brsTitleMark, new Rectangle(0, 0, 4, _titleFont.Height));
            pe.Graphics.DrawString(Text, _titleFont, _brsTitle, new PointF(10, 0));
        }
    }
}
