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
    public partial class XCheckBox : CheckBox
    {
        //缓存画笔画刷
        private SolidBrush _brsBackColor;
        private SolidBrush _brsDisabledBackColor = new SolidBrush(Color.Gray);
        private SolidBrush _brsForeColor;
        private SolidBrush _brsInner;
        private Pen _penBoxBorderColor;
        private Pen _penInner;
        private Pen _penMouseHoveringForeColor;
        private Pen _penDisabledBackColor = new Pen(Color.Gray);

        private Color _boxBorderColor;
        private Color _innerColor;
        private Color _MouseHoveringForeColor = Color.FromArgb(83, 128, 252);

        //鼠标正在停留
        private bool _isMouseHovering;

        /// <summary>
        /// 选框边框颜色
        /// </summary>
        [Browsable(true)]
        [Category("Appearance")]
        [Description("指定选框边框的颜色")]
        public Color BoxBorderColor
        {
            get => _boxBorderColor;
            set
            {
                _boxBorderColor = value;
                _penBoxBorderColor = new Pen(_boxBorderColor);
                Invalidate();
            }
        }

        /// <summary>
        /// 内部颜色
        /// </summary>
        [Browsable(true)]
        [Category("Appearance")]
        [Description("指定内部的颜色")]
        public Color InnerColor
        {
            get => _innerColor;
            set
            {
                _innerColor = value;
                _penInner = new Pen(_innerColor, 2F);
                _brsInner = new SolidBrush(_innerColor);
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
            get => _MouseHoveringForeColor;
            set
            {
                _MouseHoveringForeColor = value;
                _penMouseHoveringForeColor = new Pen(_MouseHoveringForeColor);
                Invalidate();
            }
        }

        public XCheckBox()
        {
            InitializeComponent();

            _boxBorderColor = ForeColor;
            _innerColor = Color.FromArgb(89, 98, 255);

            _penInner = new Pen(_innerColor, 2F);
            _brsInner = new SolidBrush(_innerColor);
            _penMouseHoveringForeColor = new Pen(_MouseHoveringForeColor);
            _penBoxBorderColor = new Pen(_boxBorderColor);
        }

        protected override void OnPaint(PaintEventArgs pe)
        {
            Graphics g = pe.Graphics;
            g.PixelOffsetMode = PixelOffsetMode.HighQuality;
            g.SmoothingMode = SmoothingMode.AntiAlias;

            //背景
            Rectangle rectControl = new Rectangle(-1, -1, Width + 2, Height + 2);
            g.FillRectangle(_brsBackColor, rectControl);

            //选框边框
            RectangleF rectBorder = new RectangleF(0, 0, Height, Height);
            rectBorder.Inflate(-Height / 6F, -Height / 6F);
            g.DrawRectangle(Enabled
                ? (_isMouseHovering ? _penMouseHoveringForeColor : _penBoxBorderColor)
                : _penDisabledBackColor
                , rectBorder.X, rectBorder.Y, rectBorder.Width, rectBorder.Height);

            switch (CheckState)
            {
                case CheckState.Unchecked:
                    break;
                case CheckState.Checked:
                    //对勾
                    rectBorder.Inflate(-(Height / 8F), -(Height / 8F));
                    g.DrawLines(_isMouseHovering ? _penMouseHoveringForeColor : _penInner, new[]
                    {
                        new PointF(rectBorder.X, Height / 2F),
                        new PointF(rectBorder.X + rectBorder.Width / 3F, Height / 5F * 3.5F),
                        new PointF(rectBorder.Right, Height / 3F)
                    });
                    break;
                case CheckState.Indeterminate:
                    //矩形
                    rectBorder.Inflate(-(Height / 8F), -(Height / 8F));
                    g.FillRectangle(Enabled ? _brsInner : _brsDisabledBackColor, rectBorder);
                    break;
            }

            //文字
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
