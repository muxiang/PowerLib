using System;
using System.ComponentModel;
using System.Drawing;
using System.Drawing.Drawing2D;
using System.Windows.Forms;

namespace PowerLib.Winform.Controls
{
    public partial class XRadioButton : RadioButton
    {
        // 缓存画笔画刷
        private SolidBrush _brsBackColor;
        private SolidBrush _brsDisabledBackColor = new SolidBrush(Color.Gray);
        private SolidBrush _brsForeColor;
        private SolidBrush _brsInnerCircle;
        private Pen _penOuterCircle;
        private Pen _penMouseHoveringForeColor;
        private Pen _penDisabledBackColor = new Pen(Color.Gray);

        private Color _outerCircleColor;
        private Color _innerCircleColor;
        private Color _mouseHoveringForeColor = Color.FromArgb(83, 128, 252);

        // 鼠标正在停留
        private bool _isMouseHovering;

        public XRadioButton()
        {
            InitializeComponent();

            _outerCircleColor = ForeColor;
            _penOuterCircle = new Pen(_outerCircleColor);
            _innerCircleColor = Color.FromArgb(89, 98, 255);
            _penMouseHoveringForeColor = new Pen(_mouseHoveringForeColor);

            _brsBackColor = new SolidBrush(BackColor);
            _brsForeColor = new SolidBrush(ForeColor);
            _brsInnerCircle = new SolidBrush(_innerCircleColor);
        }

        /// <summary>
        /// 外圆颜色
        /// </summary>
        [Browsable(true)]
        [Category("Appearance")]
        [Description("指定外圆的颜色")]
        public Color OuterCircleColor
        {
            get => _outerCircleColor;
            set
            {
                _outerCircleColor = value;
                _penOuterCircle = new Pen(_outerCircleColor);
                Invalidate();
            }
        }

        /// <summary>
        /// 内圆颜色
        /// </summary>
        [Browsable(true)]
        [Category("Appearance")]
        [Description("指定内圆的颜色")]
        public Color InnerCircleColor
        {
            get => _innerCircleColor;
            set
            {
                _innerCircleColor = value;
                _brsInnerCircle = new SolidBrush(_innerCircleColor);
                Invalidate();
            }
        }

        /// <summary>
        /// 鼠标停留时的前景色
        /// </summary>
        [Browsable(true)]
        [Category("Appearance")]
        [Description("指定鼠标停留时的前景色")]
        public Color MouseHoveringForeColor
        {
            get => _mouseHoveringForeColor;
            set
            {
                _mouseHoveringForeColor = value;
                _penMouseHoveringForeColor = new Pen(_mouseHoveringForeColor);
                Invalidate();
            }
        }

        protected override void OnPaint(PaintEventArgs pe)
        {
            Graphics g = pe.Graphics;
            g.PixelOffsetMode = PixelOffsetMode.HighQuality;
            g.SmoothingMode = SmoothingMode.AntiAlias;

            // 背景
            Rectangle rectControl = new Rectangle(-1, -1, Width + 2, Height + 2);
            g.FillRectangle(_brsBackColor, rectControl);

            // 外圆
            RectangleF rectCircle = new RectangleF(0, 0, Height, Height);
            rectCircle.Inflate(-Height / 6F, -Height / 6F);
            g.DrawEllipse(Enabled
                ? _isMouseHovering ? _penMouseHoveringForeColor : _penOuterCircle
                : _penDisabledBackColor
                , rectCircle);

            // 选中
            if (Checked)
            {
                rectCircle.Inflate(-(Height / 8F), -(Height / 8F));
                g.FillEllipse(Enabled ? _brsInnerCircle : _brsDisabledBackColor, rectCircle);
            }

            // 文字
            g.DrawString(Text, Font, _brsForeColor, Height, (Height - Font.Height) / 2);
        }

        protected override void OnBackColorChanged(EventArgs e)
        {
            base.OnBackColorChanged(e);
            _brsBackColor = new SolidBrush(BackColor);
        }

        protected override void OnForeColorChanged(EventArgs e)
        {
            base.OnForeColorChanged(e);
            _brsForeColor = new SolidBrush(ForeColor);
        }

        /// <summary>
        /// 鼠标进入时调用
        /// </summary>
        /// <param name="e">事件参数</param>
        protected override void OnMouseEnter(EventArgs e)
        {
            _isMouseHovering = true;
            Invalidate();
            base.OnMouseEnter(e);
        }

        /// <summary>
        /// 鼠标离开时调用
        /// </summary>
        /// <param name="e">事件参数</param>
        protected override void OnMouseLeave(EventArgs e)
        {
            _isMouseHovering = false;
            Invalidate();
            base.OnMouseLeave(e);
        }

        protected override void OnEnabledChanged(EventArgs e)
        {
            base.OnEnabledChanged(e);
            Invalidate();
        }
    }
}
