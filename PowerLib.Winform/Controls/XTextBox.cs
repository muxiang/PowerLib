using System;
using System.ComponentModel;
using System.Drawing;
using System.Drawing.Drawing2D;
using System.Windows.Forms;

using static PowerLib.NativeCodes.NativeConstants;

namespace PowerLib.Winform.Controls
{
    /// <summary>
    /// 表示Windows文本框控件
    /// </summary>
    public sealed partial class XTextBox : TextBox
    {
        private Color _borderColor;
        private Color _borderColorHighLight;

        private Pen _penBorder = Pens.Black;
        private Pen _penBorderHighLight = Pens.Black;

        // 文本框无内容时的背景提示
        private string _backgroundText;

        // 边框是否高亮
        private bool _borderHighLight;

        /// <summary>
        /// 初始化<see cref="XTextBox"/>的实例
        /// </summary>
        public XTextBox()
        {
            InitializeComponent();

            SetStyle(ControlStyles.OptimizedDoubleBuffer, true);

            BorderColor = Color.FromArgb(184, 184, 184);
            HighLightBorderColor = Color.FromArgb(66, 215, 250);
            ForeColor = Color.FromArgb(80, 80, 80);
        }

        /// <summary>
        /// 边框颜色
        /// </summary>
        [Browsable(true)]
        [Category("Appearance")]
        [Description("边框颜色")]
        [DefaultValue(typeof(Color), "184, 184, 184")]
        public Color BorderColor
        {
            get => _borderColor;
            set
            {
                _borderColor = value;
                _penBorder = new Pen(value, 1);
                Refresh();
            }
        }

        /// <summary>
        /// 高亮边框颜色
        /// </summary>
        [Browsable(true)]
        [Category("Appearance")]
        [Description("高亮边框颜色")]
        [DefaultValue(typeof(Color), "66, 215, 250")]
        public Color HighLightBorderColor
        {
            get => _borderColorHighLight;
            set
            {
                _borderColorHighLight = value;
                _penBorderHighLight = new Pen(value, 1.5F);
                Refresh();
            }
        }

        /// <summary>
        /// 文本框无内容时的背景文字
        /// </summary>
        [Browsable(true)]
        [Category("Appearance")]
        [Description("文本框无内容时的背景文字")]
        [DefaultValue("")]
        public string BackgroundText
        {
            get => _backgroundText;
            set
            {
                _backgroundText = value;
                Refresh();
            }
        }

        /// <inheritdoc />
        protected override void OnGotFocus(EventArgs e)
        {
            base.OnGotFocus(e);

            _borderHighLight = true;
            Refresh();
        }

        /// <inheritdoc />
        protected override void OnLostFocus(EventArgs e)
        {
            base.OnLostFocus(e);

            _borderHighLight = false;
            Refresh();
        }

        /// <inheritdoc />
        protected override void OnMouseEnter(EventArgs e)
        {
            base.OnMouseEnter(e);

            _borderHighLight = true;
            Refresh();
        }

        /// <inheritdoc />
        protected override void OnMouseLeave(EventArgs e)
        {
            base.OnMouseLeave(e);

            if (Focused)
                return;

            _borderHighLight = false;
            Invalidate();
        }

        /// <inheritdoc />
        protected override void OnTextChanged(EventArgs e)
        {
            base.OnTextChanged(e);

            if(!string.IsNullOrEmpty(_backgroundText))
                Refresh();
        }

        /// <inheritdoc />
        protected override void WndProc(ref Message m)
        {
            base.WndProc(ref m);

            if (m.Msg != WM_PAINT || BorderStyle == BorderStyle.None)
                return;

            using Graphics g = Graphics.FromHwnd(Handle);
            g.InterpolationMode = InterpolationMode.HighQualityBicubic;
            g.SmoothingMode = SmoothingMode.AntiAlias;
            g.DrawRectangle(_borderHighLight ? _penBorderHighLight : _penBorder,
                new Rectangle(0, 0, Width - 1, Height - 1));

            if (TextLength == 0 && !string.IsNullOrEmpty(_backgroundText))
            {
                StringFormat sf = new StringFormat { LineAlignment = StringAlignment.Center };
                g.DrawString(_backgroundText, Font, Brushes.DarkGray, DisplayRectangle, sf);
            }
        }
    }
}
