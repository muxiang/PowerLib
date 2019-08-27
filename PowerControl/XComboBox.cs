using System;
using System.ComponentModel;
using System.Drawing;
using System.Drawing.Drawing2D;
using System.Windows.Forms;

namespace PowerControl
{
    public partial class XComboBox : ComboBox
    {
        private Color _borderColor = Color.FromArgb(184, 184, 184);
        private Color _MouseHoveringForeColor = Color.FromArgb(83, 128, 252);
        private ButtonBorderStyle _borderStyle = ButtonBorderStyle.Solid;

        //缓存画笔画刷
        private SolidBrush _brsBackColor;
        private SolidBrush _brsDisabledBackColor = new SolidBrush(Color.Gray);
        private SolidBrush _brsForeColor;
        private Pen _penForeColor;
        private Pen _penBorderColor;
        private Pen _penMouseHoveringForeColor;

        //鼠标正在停留
        private bool _isMouseHovering;

        public XComboBox()
        {
            InitializeComponent();
            SetStyle(ControlStyles.DoubleBuffer
                | ControlStyles.UserPaint
                | ControlStyles.AllPaintingInWmPaint, true);

            _brsBackColor = new SolidBrush(BackColor);
            _brsForeColor = new SolidBrush(ForeColor);
            _penForeColor = new Pen(ForeColor);
            _penBorderColor = new Pen(_borderColor);
            _penMouseHoveringForeColor = new Pen(_MouseHoveringForeColor);
        }

        #region 属性

        /// <summary>
        /// 边框颜色
        /// </summary>
        [Browsable(true)]
        [Category("Appearance")]
        [Description("指定边框的颜色")]
        public Color BorderColor
        {
            get => _borderColor;
            set
            {
                _borderColor = value;
                _penBorderColor = new Pen(_borderColor);
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

        /// <summary>
        /// 边框样式
        /// </summary>
        [Browsable(true)]
        [Category("Appearance")]
        [Description("指定边框的样式")]
        [DefaultValue(ButtonBorderStyle.Solid)]
        public ButtonBorderStyle BorderStyle
        {
            get => _borderStyle;
            set
            {
                _borderStyle = value;
                Invalidate();
            }
        }

        #endregion 属性

        protected override void WndProc(ref Message m)
        {
            base.WndProc(ref m);

            if (m.Msg == NativeConstants.WM_PAINT || m.Msg == NativeConstants.WM_CTLCOLOREDIT)
            {
                IntPtr hDC = NativeMethods.GetWindowDC(m.HWnd);
                if (hDC == IntPtr.Zero) return;

                using (Graphics g = Graphics.FromHdc(hDC))
                {
                    g.PixelOffsetMode = PixelOffsetMode.HighQuality;
                    g.SmoothingMode = SmoothingMode.AntiAlias;

                    Rectangle rect = new Rectangle(0, 0, Width, Height);
                    Rectangle rectBg = new Rectangle(1, 1, Width - 2, Height - 2);

                    //边框
                    ControlPaint.DrawBorder(g, rect, _isMouseHovering ? _MouseHoveringForeColor : _borderColor, _borderStyle);
                    //背景
                    g.FillRectangle(Enabled ? _brsBackColor : _brsDisabledBackColor, rectBg);

                    string selValue = SelectedItem == null ? "" : SelectedItem.ToString();
                    string Text = Enabled ? selValue : "";

                    //下拉箭头
                    g.DrawLines(_isMouseHovering ? _penMouseHoveringForeColor : _penBorderColor, new[]
                    {
                        new PointF(Width - Height / 4 * 3, Height / 2),
                        new PointF(Width - (Height / 4 * 3 - Height / 8), Height / 3 * 2),
                        new PointF(Width - (Height / 4 * 3 - Height / 4), Height / 2)
                    });

                    g.DrawString(Text, Font, _brsForeColor, 1, 2);

                    NativeMethods.ReleaseDC(m.HWnd, hDC);
                }
            }
        }

        protected override void OnDrawItem(DrawItemEventArgs e)
        {
            int ItemsCount = Items.Count;
            Rectangle rect = new Rectangle(0, 0, e.Bounds.Width - 1, e.Bounds.Height * ItemsCount - 2);

            e.Graphics.PixelOffsetMode = PixelOffsetMode.HighQuality;
            e.Graphics.SmoothingMode = SmoothingMode.AntiAlias;
            e.Graphics.FillRectangle(_brsBackColor, rect);
            e.Graphics.DrawRectangle(_isMouseHovering ? _penMouseHoveringForeColor : _penBorderColor, rect);

            for (int i = 0; i < Items.Count; i++)
            {
                if (SelectedItem != null)
                {
                    if (SelectedItem.ToString() == Items[i].ToString())
                    {
                        Rectangle rectSel = new Rectangle(1, e.Bounds.Height * SelectedIndex, e.Bounds.Width - 2, e.Bounds.Height - 2);
                        e.Graphics.FillRectangle(_brsDisabledBackColor, rectSel);
                    }
                }
                e.Graphics.DrawString(Items[i].ToString(), Font, _brsForeColor, new PointF(0, i * (e.Bounds.Height == 0 ? 16 : e.Bounds.Height)));
            }
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
            _penForeColor = new Pen(ForeColor);
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
    }
}
