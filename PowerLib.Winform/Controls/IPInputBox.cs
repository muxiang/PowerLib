﻿using System;
using System.ComponentModel;
using System.Drawing;
using System.Diagnostics.CodeAnalysis;
using System.Linq;
using System.Net;
using System.Windows.Forms;

using PowerLib.Winform.Design;

namespace PowerLib.Winform.Controls
{
    /// <summary>
    /// 表示一个支持IPV4格式化录入的文本框
    /// </summary>
    [Designer(typeof(FixedSizeDesigner))]
    [SuppressMessage("ReSharper", "InconsistentNaming")]
    public sealed partial class IPInputBox : UserControl
    {
        private IPAddress _value;

        /// <summary>
        /// 初始化IP输入文本框的实例
        /// </summary>
        public IPInputBox()
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

                txt.KeyPress += (s1, e1) =>
                {
                    if ((e1.KeyChar < '0' || e1.KeyChar > '9') && e1.KeyChar != 8)
                        e1.Handled = true;
                    else
                    {
                        if (e1.KeyChar == 8) return;

                        // 第三位数字按下后跳到下一段
                        TextBox t = (TextBox)s1;
                        if (t.SelectionStart == 2)
                            Controls.OfType<TextBox>().FirstOrDefault(textBox =>
                                    textBox.Name.EndsWith((Convert.ToInt32(t.Name.Substring(3, 1)) + 1).ToString()))
                                ?.Focus();
                    }
                };

                txt.TextChanged += InnerTxtOnTextChanged;
            }
        }

        private void InnerTxtOnTextChanged(object s1, EventArgs e1)
        {
            string strIp = $"{txt1.Text}.{txt2.Text}.{txt3.Text}.{txt4.Text}";

            if (!IPAddress.TryParse(strIp, out IPAddress ip))
                return;

            _value = ip;
        }

        /// <inheritdoc cref="TextBox"/>
        [Browsable(true)]
        public new string Text
        {
            get => Value == null ? string.Empty : Value.ToString();
            set
            {
                if (!IPAddress.TryParse(value, out IPAddress ip))
                    return;

                Value = ip;
            }
        }

        /// <summary>
        /// 获取输入的IP地址
        /// </summary>
        public IPAddress Value
        {
            get => _value;
            set
            {
                if (value == null)
                    return;

                byte[] bytes = value.GetAddressBytes();
                txt1.Text = bytes[0].ToString();
                txt2.Text = bytes[1].ToString();
                txt3.Text = bytes[2].ToString();
                txt4.Text = bytes[3].ToString();
            }
        }

        /// <inheritdoc />
        protected override void OnPaint(PaintEventArgs e)
        {
            foreach (TextBox txt in Controls.OfType<TextBox>().Where(t => t != txt1))
            {
                RectangleF rectDot =
                    new RectangleF(txt.Location.X - 2, txt.Location.Y + txt.Height - 5, 3F, 3F);
                e.Graphics.FillEllipse(Brushes.Gray, rectDot);
            }

            base.OnPaint(e);
        }
    }
}
