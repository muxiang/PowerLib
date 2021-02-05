using System;
using System.ComponentModel;
using System.Drawing;
using System.Drawing.Drawing2D;
using System.Windows.Forms;

using static PowerControl.NativeMethods;

namespace PowerControl
{
    /// <summary>
    /// 表示组合框控件
    /// </summary>
    public sealed partial class XComboBox : ComboBox
    {
        private Color _borderColor = Color.FromArgb(184, 184, 184);
        private Color _borderColor_HighLight = Color.FromArgb(66, 215, 250);
        private ButtonBorderStyle _borderStyle = ButtonBorderStyle.Solid;

        /* 缓存画笔画刷 */
        // 前景画笔
        private Pen _penForeColor;
        // 边框画笔
        private Pen _penBorderColor;
        // 高亮边框画笔
        private Pen _penBorderColor_HighLight;
        // 前景画刷
        private SolidBrush _brsForeColor;
        // 背景画刷
        private SolidBrush _brsBackColor;
        // 下拉箭头画刷
        private SolidBrush _brsArrow;
        // 高亮下拉箭头画刷
        private SolidBrush _brsArrow_HighLight;
        // 禁用状态背景画刷
        private readonly SolidBrush _brsDisabledBackColor = new SolidBrush(Color.LightGray);

        // 边框是否高亮
        private bool _borderHighLight;

        /// <summary>
        /// 初始化<see cref="XComboBox"/>的实例
        /// </summary>
        public XComboBox()
        {
            InitializeComponent();
            SetStyle(ControlStyles.DoubleBuffer
                | ControlStyles.UserPaint
                | ControlStyles.AllPaintingInWmPaint, true);

            _penForeColor = new Pen(ForeColor);
            _penBorderColor = new Pen(_borderColor);
            _penBorderColor_HighLight = new Pen(_borderColor_HighLight);

            _brsBackColor = new SolidBrush(BackColor);
            _brsForeColor = new SolidBrush(ForeColor);
            _brsArrow = new SolidBrush(_borderColor);
            _brsArrow_HighLight = new SolidBrush(_borderColor_HighLight);
        }

        #region 属性

        /// <summary>
        /// 边框颜色
        /// </summary>
        [Browsable(true)]
        [Category("Appearance")]
        [Description("指定边框的颜色")]
        [DefaultValue(typeof(Color), "184, 184, 184")]
        public Color BorderColor
        {
            get => _borderColor;
            set
            {
                _borderColor = value;
                _penBorderColor = new Pen(_borderColor);
                _brsArrow = new SolidBrush(_borderColor);
                Invalidate();
            }
        }

        /// <summary>
        /// 高亮边框颜色
        /// </summary>
        [Browsable(true)]
        [Category("Appearance")]
        [Description("指定高亮边框颜色")]
        [DefaultValue(typeof(Color), "66, 215, 250")]
        public Color HighLightBorderColor
        {
            get => _borderColor_HighLight;
            set
            {
                _borderColor_HighLight = value;
                _penBorderColor_HighLight = new Pen(_borderColor_HighLight);
                _brsArrow_HighLight = new SolidBrush(_borderColor_HighLight);
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

        /// <inheritdoc />
        protected override void WndProc(ref Message m)
        {
            base.WndProc(ref m);

            if (m.Msg != NativeConstants.WM_PAINT && m.Msg != NativeConstants.WM_CTLCOLOREDIT)
                return;

            IntPtr hDC = GetWindowDC(m.HWnd);
            if (hDC == IntPtr.Zero) return;

            using (Graphics g = Graphics.FromHdc(hDC))
            {
                g.PixelOffsetMode = PixelOffsetMode.HighQuality;
                g.SmoothingMode = SmoothingMode.AntiAlias;
                g.Clear(BackColor);

                Rectangle rect = new Rectangle(0, 0, Width, Height);
                Rectangle rectBg = new Rectangle(1, 1, Width - 2, Height - 2);

                // 边框
                ControlPaint.DrawBorder(g, rect, _borderHighLight ? _borderColor_HighLight : _borderColor, _borderStyle);
                // 背景
                g.FillRectangle(Enabled ? _brsBackColor : _brsDisabledBackColor, rectBg);

                string selValue = SelectedItem == null ? "" : SelectedItem.ToString();
                string text = Enabled ? selValue : "";

                // 下拉箭头
                g.FillPolygon(_borderHighLight ? _brsArrow_HighLight : _brsArrow, new[]
                {
                    new PointF(Width - 11, Height / 2F),
                    new PointF(Width - 8, Height / 3F * 2),
                    new PointF(Width - 5, Height / 2F)
                });

                SizeF szText = g.MeasureString(text, Font);

                g.DrawString(text, Font, _brsForeColor, 2, (Height - szText.Height) / 2);

                ReleaseDC(m.HWnd, hDC);
            }
        }

        /// <inheritdoc />
        protected override void OnDrawItem(DrawItemEventArgs e)
        {
            int ItemsCount = Items.Count;
            Rectangle rect = new Rectangle(0, 0, e.Bounds.Width - 1, e.Bounds.Height * ItemsCount - 2);

            e.Graphics.PixelOffsetMode = PixelOffsetMode.HighQuality;
            e.Graphics.SmoothingMode = SmoothingMode.AntiAlias;
            e.Graphics.FillRectangle(_brsBackColor, rect);
            e.Graphics.DrawRectangle(_borderHighLight ? _penBorderColor_HighLight : _penBorderColor, rect);

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

        /// <inheritdoc />
        protected override void OnBackColorChanged(EventArgs e)
        {
            base.OnBackColorChanged(e);
            _brsBackColor = new SolidBrush(BackColor);
        }

        /// <inheritdoc />
        protected override void OnForeColorChanged(EventArgs e)
        {
            base.OnForeColorChanged(e);
            _brsForeColor = new SolidBrush(ForeColor);
            _penForeColor = new Pen(ForeColor);
        }

        /// <inheritdoc />
        protected override void OnMouseEnter(EventArgs e)
        {
            base.OnMouseEnter(e);

            _borderHighLight = true;
            Invalidate();
        }

        /// <inheritdoc />
        protected override void OnMouseLeave(EventArgs e)
        {
            base.OnMouseLeave(e);

            if (Focused) return;

            _borderHighLight = false;
            Invalidate();
        }

        /// <inheritdoc />
        protected override void OnGotFocus(EventArgs e)
        {
            base.OnGotFocus(e);

            _borderHighLight = true;
            Invalidate();
        }

        /// <inheritdoc />
        protected override void OnLostFocus(EventArgs e)
        {
            base.OnLostFocus(e);

            _borderHighLight = false;
            Invalidate();
        }

        /// <inheritdoc />
        protected override void OnEnabledChanged(EventArgs e)
        {
            base.OnEnabledChanged(e);

            _borderHighLight = false;
            Invalidate();
        }
    }
}
