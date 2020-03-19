using System;
using System.ComponentModel;
using System.Drawing;
using System.Diagnostics.CodeAnalysis;
using System.Drawing.Drawing2D;
using System.Linq;
using System.Net;
using System.Windows.Forms;

namespace PowerControl
{
    /// <summary>
    /// 表示一个支持IPV4格式化录入的文本框
    /// </summary>
    // [Designer(typeof(FixedSizeDesigner))]
    [SuppressMessage("ReSharper", "InconsistentNaming")]
    public sealed partial class IPTextBox : UserControl
    {
        private Color _borderColor;
        private Pen _borderPen = Pens.Black;

        private Color _focusedBorderColor;
        private Pen _focusedBorderPen = Pens.Black;

        /// <summary>
        /// 初始化IP输入文本框的实例
        /// </summary>
        public IPTextBox()
        {
            InitializeComponent();

            base.BorderStyle = BorderStyle.None;
            SetStyle(ControlStyles.OptimizedDoubleBuffer, true);

            BorderColor = Color.FromArgb(184, 184, 184);
            FocusedBorderColor = Color.FromArgb(66, 215, 250);
            ForeColor = Color.FromArgb(80, 80, 80);

            foreach (TextBox txt in Controls.OfType<TextBox>())
            {
                txt.PreviewKeyDown += (s1, e1) =>
                {
                    TextBox t = (TextBox)s1;
                    switch (e1.KeyCode)
                    {
                        case Keys.OemPeriod:
                        case Keys.Decimal:
                        case Keys.Right:
                            if (t.SelectionStart == t.TextLength)
                                Controls.OfType<TextBox>().FirstOrDefault(textBox =>
                                        textBox.Name.EndsWith((Convert.ToInt32(t.Name.Substring(3, 1)) + 1).ToString()))
                                    ?.Focus();
                            break;
                        case Keys.Left:
                            if (t.SelectionStart == 0)
                                Controls.OfType<TextBox>().FirstOrDefault(textBox =>
                                        textBox.Name.EndsWith((Convert.ToInt32(t.Name.Substring(3, 1)) - 1).ToString()))
                                    ?.Focus();
                            break;
                        case Keys.Back:
                            if (t.SelectionStart == 0)
                            {
                                TextBox txtPrev = Controls.OfType<TextBox>().FirstOrDefault(textBox =>
                                        textBox.Name.EndsWith((Convert.ToInt32(t.Name.Substring(3, 1)) - 1).ToString()));
                                if (txtPrev == null) return;
                                txtPrev.Focus();
                                if (txtPrev.TextLength > 0)
                                    txtPrev.Text = txtPrev.Text.Remove(txtPrev.TextLength - 1, 1);

                                txtPrev.SelectionStart = txtPrev.TextLength;
                            }
                            break;
                        case Keys.Tab:
                            SelectNextControl(this, true, true, false, true);
                            break;
                    }
                };

                txt.KeyPress += (sender, e1) =>
                {
                    if ((e1.KeyChar < '0' || e1.KeyChar > '9') && e1.KeyChar != 8)
                        e1.Handled = true;
                    else
                    {
                        if (e1.KeyChar == 8) return;

                        // 第三位数字按下后跳到下一段
                        TextBox t = (TextBox)sender;
                        if (t.SelectionStart == 2)
                            Controls.OfType<TextBox>().FirstOrDefault(textBox =>
                                    textBox.Name.EndsWith((Convert.ToInt32(t.Name.Substring(3, 1)) + 1).ToString()))
                                ?.Focus();
                    }
                };

                txt.GotFocus += (s1, e1) => Invalidate();
                txt.LostFocus += (s1, e1) => Invalidate();
            }
        }

        /// <inheritdoc cref="TextBox"/>
        public override string Text
        {
            get => $"{txt1.Text}.{txt2.Text}.{txt3.Text}.{txt4.Text}";
            set
            {
                if (!IPAddress.TryParse(value, out IPAddress ip))
                    return;

                byte[] bytes = ip.GetAddressBytes();
                txt1.Text = bytes[0].ToString();
                txt2.Text = bytes[1].ToString();
                txt3.Text = bytes[2].ToString();
                txt4.Text = bytes[3].ToString();
            }
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
                _borderPen = new Pen(value, 1);
                Invalidate();
            }
        }

        /// <summary>
        /// 焦点边框颜色
        /// </summary>
        [Browsable(true)]
        [Category("Appearance")]
        [Description("焦点边框颜色")]
        [DefaultValue(typeof(Color), "66, 215, 250")]
        public Color FocusedBorderColor
        {
            get => _focusedBorderColor;
            set
            {
                _focusedBorderColor = value;
                _focusedBorderPen = new Pen(value, 1.5F);
                Invalidate();
            }
        }

        /// <summary>
        /// 获取输入的IP地址
        /// </summary>
        public IPAddress Value => !IPAddress.TryParse(Text, out IPAddress ip) ? null : ip;

        public override bool Focused => txt1.Focused || txt2.Focused || txt3.Focused || txt4.Focused;

        public new  BorderStyle BorderStyle { get; set; }

        protected override void WndProc(ref Message m)
        {
            base.WndProc(ref m);

            switch (m.Msg)
            {
                case NativeConstants.WM_PAINT:
                    Graphics g = Graphics.FromHwnd(Handle);
                    foreach (TextBox txt in Controls.OfType<TextBox>().Where(t => t != txt1))
                    {
                        RectangleF rectDot = new RectangleF(txt.Location.X - 2, txt.Location.Y + txt.Height - 5, 1.5F, 1.5F);
                        g.FillEllipse(Brushes.Black, rectDot);
                    }

                    if (BorderStyle == BorderStyle.None)
                        return;

                    g.InterpolationMode = InterpolationMode.HighQualityBicubic;
                    g.SmoothingMode = SmoothingMode.AntiAlias;
                    g.DrawRectangle(Focused ? _focusedBorderPen : _borderPen, new Rectangle(0, 0, Width - 1, Height - 1));

                    break;
            }
        }
    }
}
