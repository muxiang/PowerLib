using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Diagnostics;
using System.Drawing;
using System.Drawing.Printing;
using System.Linq;
using System.Text;
using System.Windows.Forms;

using static PowerControl.NativeConstants;

namespace PowerControl
{
    /// <summary>
    /// 表示用户界面窗口
    /// </summary>
    public class XForm : Form
    {
        #region 常量

        // 边框宽度
        private const int BorderWidth = 4;
        // 标题栏高度
        private const int TitleHeight = 40;

        #endregion 常量

        #region 字段

        // 阴影背景
        private XFormBackground _backWindow;

        private FormBorderStyle _formBorderStyle = FormBorderStyle.Sizable;

        #endregion 字段

        /// <summary>
        /// 初始化用户界面窗口
        /// </summary>
        public XForm()
        {
            base.FormBorderStyle = FormBorderStyle.None;
            InitializeComponent();
        }

        private void InitializeComponent()
        {
            ClientSize = new Size(284, 261);
            BackColor = Color.White;
            BorderColor = Color.Gray;
            Name = "XForm";
        }

        #region 属性

        /// <summary>
        /// 获取或设置窗体的边框样式
        /// </summary>
        [Browsable(true)]
        [Category("Appearance")]
        [Description("获取或设置窗体的边框样式")]
        [DefaultValue(FormBorderStyle.Sizable)]
        public new FormBorderStyle FormBorderStyle
        {
            get => _formBorderStyle;
            set
            {
                _formBorderStyle = value;
                Invalidate();
            }
        }

        /// <summary>
        /// 获取或设置窗体的边框颜色
        /// </summary>
        [Browsable(true)]
        [Category("Appearance")]
        [Description("获取或设置窗体的边框颜色")]
        [DefaultValue(typeof(Color), "Gray")]
        public Color BorderColor { get; set; }

        /// <summary>
        /// 获取或设置控件的内边距
        /// </summary>
        [Browsable(true)]
        [Category("Appearance")]
        [Description("获取或设置窗体的边框样式")]
        public new Padding Padding
        {
            get =>
                new Padding(
                    base.Padding.Left,
                    base.Padding.Top - TitleHeight,
                    base.Padding.Right,
                    base.Padding.Bottom);
            set =>
                base.Padding = new Padding(
                    value.Left,
                    value.Top + TitleHeight,
                    value.Right,
                    value.Bottom);
        }

        #endregion 属性

        #region 重写

        /// <inheritdoc />
        protected override Padding DefaultPadding =>
            new Padding(
                base.DefaultPadding.Left,
                base.DefaultPadding.Top + TitleHeight,
                base.DefaultPadding.Right,
                base.DefaultPadding.Bottom);

        /// <inheritdoc />
        protected override void OnPaint(PaintEventArgs e)
        {
            base.OnPaint(e);

            switch (FormBorderStyle)
            {
                case FormBorderStyle.None:
                    break;
                case FormBorderStyle.FixedSingle:
                case FormBorderStyle.FixedToolWindow:
                case FormBorderStyle.Fixed3D:
                case FormBorderStyle.FixedDialog:
                case FormBorderStyle.Sizable:
                case FormBorderStyle.SizableToolWindow:
                    //边框
                    using (Brush brsBorder = new SolidBrush(BorderColor))
                    {
                        //上
                        e.Graphics.FillRectangle(brsBorder, 0, 0, Width, BorderWidth);
                        //左
                        e.Graphics.FillRectangle(brsBorder, 0, 0, BorderWidth, Height);
                        //下
                        e.Graphics.FillRectangle(brsBorder, 0, Height - BorderWidth, Width, BorderWidth);
                        //右
                        e.Graphics.FillRectangle(brsBorder, Width - BorderWidth, 0, BorderWidth, Height);
                    }
                    //标题栏背景
                    e.Graphics.FillRectangle(Brushes.Red, 0, 0, Width, TitleHeight);
                    break;
                default:
                    throw new ArgumentOutOfRangeException();
            }
        }

        /// <inheritdoc />
        protected override void WndProc(ref Message m)
        {
            switch (m.Msg)
            {
                case WM_NCHITTEST:
                    {
                        base.WndProc(ref m);

                        Point pt = PointToClient(new Point((int)m.LParam & 0xFFFF, (int)m.LParam >> 16 & 0xFFFF));

                        bool bTop = pt.Y <= BorderWidth;
                        bool bBottom = pt.Y >= ClientSize.Height - BorderWidth;
                        bool bLeft = pt.X <= BorderWidth;
                        bool bRight = pt.X >= ClientSize.Width - BorderWidth;

                        if (bLeft)
                            if (bTop)
                                m.Result = (IntPtr)HTTOPLEFT;
                            else if (bBottom)
                                m.Result = (IntPtr)HTBOTTOMLEFT;
                            else m.Result = (IntPtr)HTLEFT;
                        else if (bRight)
                            if (bTop)
                                m.Result = (IntPtr)HTTOPRIGHT;
                            else if (bBottom)
                                m.Result = (IntPtr)HTBOTTOMRIGHT;
                            else m.Result = (IntPtr)HTRIGHT;
                        else if (bTop)
                            m.Result = (IntPtr)HTTOP;
                        else if (bBottom)
                            m.Result = (IntPtr)HTBOTTOM;
                        break;
                    }
                case WM_LBUTTONDOWN:
                    {
                        Point pt = new Point((int)m.LParam & 0xFFFF, (int)m.LParam >> 16 & 0xFFFF);

                        if (pt.Y < TitleHeight)
                        {
                            m.Msg = WM_NCLBUTTONDOWN;



                            m.WParam = (IntPtr)HTCAPTION;
                        }

                        base.WndProc(ref m);
                        break;
                    }
                //case WM_LBUTTONUP:
                //    {
                //        Point pt = PointToClient(new Point((int)m.LParam & 0xFFFF, (int)m.LParam >> 16 & 0xFFFF));

                //        if (pt.Y < TitleHeight)
                //        {
                //            m.Msg = WM_NCLBUTTONUP;



                //            m.WParam = (IntPtr)HTCAPTION;
                //        }
                //        base.WndProc(ref m);

                //        break;
                //    }
                //case WM_MOUSEMOVE:
                //    {

                //        Point pt = PointToClient(new Point((int)m.LParam & 0xFFFF, (int)m.LParam >> 16 & 0xFFFF));

                //        if (pt.Y < TitleHeight)
                //        {
                //            m.Msg = WM_NCMOUSEMOVE;


                //            m.WParam = (IntPtr)HTCAPTION;
                //        }
                //        base.WndProc(ref m);

                //        break;
                //    }
                //case WM_NCLBUTTONDOWN:
                //    {
                //        base.WndProc(ref m);

                //        Point pt = PointToClient(new Point((int)m.LParam & 0xFFFF, (int)m.LParam >> 16 & 0xFFFF));

                //        switch ((int)m.WParam)
                //        {
                //            case HTCAPTION:
                //                _xPadding = pt.X;
                //                _yPadding = pt.Y;
                //                _dragging = true;
                //                break;
                //        }

                //        break;
                //    }
                //case WM_NCLBUTTONUP:
                //    {
                //        base.WndProc(ref m);

                //        Point pt = PointToClient(new Point((int)m.LParam & 0xFFFF, (int)m.LParam >> 16 & 0xFFFF));

                //        switch ((int)m.WParam)
                //        {
                //            case HTCAPTION:
                //                _dragging = false;
                //                break;
                //        }

                //        break;
                //    }
                //case WM_NCMOUSEMOVE:
                //    {
                //        base.WndProc(ref m);

                //        Point pt = PointToClient(new Point((int)m.LParam & 0xFFFF, (int)m.LParam >> 16 & 0xFFFF));

                //        switch ((int)m.WParam)
                //        {
                //            case HTCAPTION:

                //                if (!_dragging) return;
                //                Location = new Point(pt.X - _xPadding, pt.Y - _yPadding);
                //                break;
                //        }

                //        break;
                //    }
                default:
                    base.WndProc(ref m);
                    break;
            }
        }

        #endregion 重写
    }
}
