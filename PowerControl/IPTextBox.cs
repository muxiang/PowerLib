using System;
using System.ComponentModel;
using System.Drawing;
using System.Diagnostics.CodeAnalysis;
using System.Linq;
using System.Net;
using System.Windows.Forms;

using PowerControl.Design;

namespace PowerControl
{
    /// <summary>
    /// 表示一个支持IPV4格式化录入的文本框
    /// </summary>
    [Designer(typeof(FixedSizeDesigner))]
    [SuppressMessage("ReSharper", "InconsistentNaming")]
    public sealed partial class IPTextBox : UserControl
    {
        /// <summary>
        /// 初始化IP输入文本框的实例
        /// </summary>
        public IPTextBox()
        {
            InitializeComponent();

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

                        //第三位数字按下后跳到下一段
                        TextBox t = (TextBox)sender;
                        if (t.SelectionStart == 2)
                            Controls.OfType<TextBox>().FirstOrDefault(textBox =>
                                    textBox.Name.EndsWith((Convert.ToInt32(t.Name.Substring(3, 1)) + 1).ToString()))
                                ?.Focus();
                    }
                };
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
        /// 获取输入的IP地址
        /// </summary>
        public IPAddress Value => !IPAddress.TryParse(Text, out IPAddress ip) ? null : ip;

        protected override void OnPaint(PaintEventArgs e)
        {
            foreach (TextBox txt in Controls.OfType<TextBox>().SkipWhile(t => t == txt1))
            {
                RectangleF rectDot =
                    new RectangleF(txt.Location.X - 2, txt.Location.Y + txt.Height - 5, 1.5F, 1.5F);
                e.Graphics.FillEllipse(Brushes.Black, rectDot);
            }

            base.OnPaint(e);
        }
    }
}
