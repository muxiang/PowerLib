using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace PowerControl
{
    public partial class XForm2 : Form
    {
        public XForm2()
        {
            InitializeComponent();
        }
        #region 自定义属性
        private int _MaxBtnWidth = 22;
        private int _MaxBtnHeight = 20;
        private int _MaxLeft = 7;
        private int _MaxBorderWidth = 2;
        private Color _BorderColor = Color.Gray;
        [Description("边框颜色"), Category("UserSet")]
        public Color BorderColor
        {
            set { this.BorderColor = value; }
            get { return this._BorderColor; }
        }
        #endregion
        #region WindowsAPI
        [DllImport("user32.dll")]
        private static extern IntPtr GetWindowDC(IntPtr hWnd);
        [DllImport("user32.dll")]
        private static extern int ReleaseDC(IntPtr hWnd, IntPtr hDC);
        [DllImport("dwmapi.dll")]
        private static extern int DwmDefWindowProc(IntPtr hwnd, int msg, IntPtr wParam, IntPtr lParam, out IntPtr result);
        [DllImport("user32.dll")]
        public static extern bool ReleaseCapture();
        [DllImport("user32.dll")]
        public static extern bool SendMessage(IntPtr hwnd, int wMsg, int wParam, int lParam);
        #endregion
        #region Windows系统消息
        private const int WM_PAINT = 0x000F;
        private const int WM_CREATE = 0x0001;
        private const int WM_NCCREATE = 0x0081;
        private const int WM_NCCALCSIZE = 0x0083;
        private const int WM_NCPAINT = 0x0085;//绘制客户区
        private const int WM_MOUSEMOVE = 0x0200;
        private const int WM_NCMOUSEMOVE = 0x00A0;
        private const int WM_NCACTIVATE = 0x0086;//绘制非客户区（标题栏）
        private const int WM_NCLBUTTONDOWN = 0x00A1;//鼠标点击
        private const int WM_NCHITTEST = 0x84;//命中测试
        private const int WM_SYSCOMMAND = 0x112;//系统菜单消息
        private const int SC_CLOSE = 0xF060;//关闭
        private const int SC_MINIMIZE = 0xF020;//最小化
        private const int SC_MAXIMIZE = 0xF030;//最大化
        private const int SC_MOVE = 0xF010;
        private const int SC_RESTORE = 0xF120;//恢复窗口
        //鼠标命中测试命中区域
        private const int HTCLIENT = 1;
        private const int HTCAPTION = 2;
        private const int HTLEFT = 10;
        private const int HTRIGHT = 11;
        private const int HTTOP = 12;
        private const int HTLEFTTOP = 13;
        private const int HTRIGHTTOP = 14;
        private const int HTBOTTOM = 15;
        private const int HTLEFTBOTTOM = 16;
        private const int HTRIGHTBOTTOM = 17;
        //拖动窗口实现缩放
        private const int WMSZ_LEFT = 0xF001;
        private const int WMSZ_RIGHT = 0xF002;
        private const int WMSZ_TOP = 0xF003;
        private const int WMSZ_LEFTTOP = 0xF004;
        private const int WMSZ_RIGHTTOP = 0xF005;
        private const int WMSZ_BOTTOM = 0xF006;
        private const int WMSZ_LEFTBOTTOM = 0xF007;
        private const int WMSZ_RIGHTBOTTOM = 0xF008;
        #endregion
        #region 自定义窗体边框大小
        int _Top = 30;
        int _Left = 8;
        int _Right = 8;
        int _Bottom = 8;
        #endregion
        #region 重写WndProc方法，完成窗体重绘
        protected override void WndProc(ref Message m)
        {
            switch (m.Msg)
            {
                case WM_NCPAINT:
                case WM_NCACTIVATE://重绘标题栏
                    IntPtr vHandle = GetWindowDC(m.HWnd);
                    Graphics vGraphics = Graphics.FromHdc(vHandle);
                    DrawTitle(vGraphics);
                    ReleaseDC(m.HWnd, vHandle);
                    break;
                //case WM_NCHITTEST://命中测试，即获取鼠标点击的区域
                //    m.Result = HitTestNCA();
                //    break;
                case WM_NCLBUTTONDOWN://鼠标点击事件
                    #region 点击事件（实现窗体拖动、缩放、关闭、最大化、最小化）
                    switch (getMousePosition())
                    {
                        case SC_CLOSE://点击了关闭按钮
                            this.Close();
                            break;
                        case SC_MAXIMIZE://点击了最大化按钮
                            this.WindowState = this.WindowState != FormWindowState.Maximized ? FormWindowState.Maximized : FormWindowState.Normal;
                            break;
                        case SC_MINIMIZE://点击了最小化按钮
                            if (this.WindowState != FormWindowState.Minimized)
                            {
                                this.WindowState = FormWindowState.Minimized;
                            }
                            break;
                        case HTCAPTION: //点击标题栏拖动窗体
                            ReleaseCapture();//释放label1对鼠标的捕捉
                            SendMessage(this.Handle, WM_SYSCOMMAND, SC_MOVE + HTCAPTION, 0);
                            break;
                        case HTLEFT://拖动左边改变窗体大小
                            ReleaseCapture();
                            SendMessage(this.Handle, WM_SYSCOMMAND, WMSZ_LEFT, 0);
                            break;
                        case HTRIGHT://拖动右边改变窗体大小
                            ReleaseCapture();
                            SendMessage(this.Handle, WM_SYSCOMMAND, WMSZ_RIGHT, 0);
                            this.Invalidate();
                            break;
                        case HTTOP://拖动上面改变窗体大小
                            ReleaseCapture();
                            SendMessage(this.Handle, WM_SYSCOMMAND, WMSZ_TOP, 0);
                            break;
                        case HTBOTTOM://拖动底部改变窗体大小
                            ReleaseCapture();
                            SendMessage(this.Handle, WM_SYSCOMMAND, WMSZ_BOTTOM, 0);
                            break;
                        case HTLEFTTOP://拖动左上角改变大小
                            ReleaseCapture();
                            SendMessage(this.Handle, WM_SYSCOMMAND, WMSZ_LEFTTOP, 0);
                            break;
                        case HTLEFTBOTTOM://拖动左下角改变大小
                            ReleaseCapture();
                            SendMessage(this.Handle, WM_SYSCOMMAND, WMSZ_LEFTBOTTOM, 0);
                            break;
                        case HTRIGHTTOP://拖动右上角改变大小
                            ReleaseCapture();
                            SendMessage(this.Handle, WM_SYSCOMMAND, WMSZ_RIGHTTOP, 0);
                            break;
                        case HTRIGHTBOTTOM://拖动右下角改变大小
                            ReleaseCapture();
                            SendMessage(this.Handle, WM_SYSCOMMAND, WMSZ_RIGHTBOTTOM, 0);
                            break;
                    }
                    #endregion
                    break;
                case WM_NCMOUSEMOVE:
                    //鼠标移动事件
                    vHandle = GetWindowDC(m.HWnd);
                    vGraphics = Graphics.FromHdc(vHandle);
                    Pen pen = new Pen(new SolidBrush(Color.White), _MaxBorderWidth);
                    Pen newPen = new Pen(new SolidBrush(Color.Red), _MaxBorderWidth);
                    switch (getMousePosition())
                    {
                        case SC_CLOSE:
                            DrawFunBtn(vGraphics, pen, pen, newPen, "Close");
                            break;
                        case SC_MAXIMIZE:
                            DrawFunBtn(vGraphics, newPen, pen, pen, "Max");
                            break;
                        case SC_MINIMIZE:
                            DrawFunBtn(vGraphics, pen, newPen, pen, "Min");
                            break;
                        default:
                            DrawFunBtn(vGraphics, pen, pen, pen, "Max");
                            break;
                    }
                    ReleaseDC(m.HWnd, vHandle);
                    break;
                default:
                    base.WndProc(ref m);
                    break;
            }
        }
        #endregion
        #region 绘制最大化、最小化、关闭按钮
        private void DrawFunBtn(Graphics vGraphics, Pen MaxPen, Pen MinPen, Pen ClosePen, string lastBtnName)
        {
            //Rectangle FunRect=new Rectangle ()
            int BaseX = Location.X;
            int BaseY = Location.Y;
            Rectangle rectClose = new Rectangle(Width - _Right - _MaxBtnWidth, _Bottom, _MaxBtnWidth, _MaxBtnHeight);
            Rectangle rectMax = new Rectangle(Width - _Right - _MaxBtnWidth * 2, _Bottom, _MaxBtnWidth, _MaxBtnHeight);
            Rectangle rectMin = new Rectangle(Width - _Right - _MaxBtnWidth * 3, _Bottom, _MaxBtnWidth, _MaxBtnHeight);
            if (lastBtnName == "Max")
            {
                vGraphics.DrawRectangle(MinPen, rectMin);
                vGraphics.DrawRectangle(ClosePen, rectClose);
                vGraphics.DrawRectangle(MaxPen, rectMax);
            }
            else if (lastBtnName == "Min")
            {
                vGraphics.DrawRectangle(ClosePen, rectClose);
                vGraphics.DrawRectangle(MaxPen, rectMax);
                vGraphics.DrawRectangle(MinPen, rectMin);
            }
            else
            {
                vGraphics.DrawRectangle(MinPen, rectMin);
                vGraphics.DrawRectangle(MaxPen, rectMax);
                vGraphics.DrawRectangle(ClosePen, rectClose);
            }
            //绘制最小化按钮
            vGraphics.DrawLine(MinPen, Width - _Right - _MaxBtnWidth * 3 + _MaxLeft, _Bottom + _MaxBtnHeight / 2, Width - _Right - _MaxLeft - _MaxBtnWidth * 2, _Bottom + _MaxBtnHeight / 2);
            //绘制关闭按钮
            Point plt = new Point(Width - _Right - _MaxBtnWidth + _MaxLeft, _Bottom + _MaxBtnHeight / 4);
            Point plb = new Point(Width - _Right - _MaxBtnWidth + _MaxLeft, _Bottom + _MaxBtnHeight * 3 / 4);
            Point prt = new Point(Width - _Right - _MaxLeft, _Bottom + _MaxBtnHeight / 4);
            Point prb = new Point(Width - _Right - _MaxLeft, _Bottom + _MaxBtnHeight * 3 / 4);
            vGraphics.DrawLine(ClosePen, plt, prb);
            vGraphics.DrawLine(ClosePen, plb, prt);
            plt.Offset(-_MaxBtnWidth, 0);
            plb.Offset(-_MaxBtnWidth, 0);
            prt.Offset(-_MaxBtnWidth, 0);
            prb.Offset(-_MaxBtnWidth, 0);
            //绘制最大化按钮             
            if (this.WindowState != FormWindowState.Maximized)
            {
                vGraphics.DrawLine(MaxPen, plt, plb);
                vGraphics.DrawLine(MaxPen, prt, prb);
                vGraphics.DrawLine(MaxPen, plt, prt);
                vGraphics.DrawLine(MaxPen, plb, prb);
            }
            else
            {
                plt.Offset(-1, 1);
                plb.Offset(-1, 1);
                prt.Offset(-1, 1);
                prb.Offset(-1, 1);
                vGraphics.DrawLine(MaxPen, plt, plb);
                vGraphics.DrawLine(MaxPen, prt, prb);
                vGraphics.DrawLine(MaxPen, plt, prt);
                vGraphics.DrawLine(MaxPen, plb, prb);
                plt.Offset(3, -3);
                plb.Offset(3, -3);
                prt.Offset(3, -3);
                prb.Offset(3, -3);
                vGraphics.DrawLine(MaxPen, plt, plb);
                vGraphics.DrawLine(MaxPen, prt, prb);
                vGraphics.DrawLine(MaxPen, plt, prt);
                vGraphics.DrawLine(MaxPen, plb, prb);
            }
        }
        #endregion
        #region 绘制标题栏
        private void DrawTitle(Graphics vGraphics)
        {
            #region 绘制边框
            vGraphics.FillRectangle(new SolidBrush(_BorderColor), new Rectangle(0, 0, Width, _Top)); //top
            vGraphics.FillRectangle(new SolidBrush(_BorderColor), new Rectangle(0, 0, _Left, Height));//left
            vGraphics.FillRectangle(new SolidBrush(_BorderColor), new Rectangle(0, Height - _Bottom, Width, _Bottom));//bottom
            vGraphics.FillRectangle(new SolidBrush(_BorderColor), new Rectangle(Width - _Right, 0, _Right, Height));//right
            #endregion
            #region 绘制标题栏文字、图标
            int left = 0, top = 0;
            if (ShowIcon)
            {
                int x = _Top - Icon.Height > 0 ? (_Top - Icon.Height) / 2 : _Bottom;
                Rectangle e = new Rectangle(x, x, _Top - _Bottom, _Top - _Bottom);
                vGraphics.DrawIcon(Icon, e);
                left += (_Top - Icon.Width) / 2 + Icon.Width;
            }
            top += (_Top - Convert.ToInt32(Font.Size)) / 2;
            left += 10;
            Rectangle TitleRectangle = new Rectangle(left, top, Width, _Top);
            vGraphics.DrawString(Text, Font, Brushes.BlanchedAlmond, TitleRectangle);
            #endregion
            #region 绘制最大化、最小化、关闭
            using (Pen pen = new Pen(new SolidBrush(Color.White), _MaxBorderWidth))
            {
                DrawFunBtn(vGraphics, pen, pen, pen, "Max");
            }
            #endregion
            vGraphics.Dispose();
        }
        #endregion
        #region 判断鼠标所在区域
        private int getMousePosition()
        {
            int BaseX = Location.X;
            int BaseY = Location.Y;
            //获取鼠标位置
            Point p = new Point(MousePosition.X, MousePosition.Y);
            Point p2 = PointToScreen(MousePosition);
            //以下待时式判断鼠标处于何处，并返回响应的值，优先级从高到低位为：边角-边缘-标题
            #region 边角判断
            //左上角判断
            Rectangle rectTopLeft = new Rectangle(BaseX, BaseY, _Left, _Left);

            if (rectTopLeft.Contains(p))
            {
                return HTLEFTTOP;
            }
            //右上角
            Rectangle rectTopRight = new Rectangle(BaseX + Width - _Right, BaseY, _Right, _Right);
            if (rectTopRight.Contains(p))
            {
                return HTRIGHTTOP;
            }
            //左下角
            Rectangle rectLeftBottom = new Rectangle(BaseX, BaseY + Height - _Bottom, _Left, _Bottom);
            if (rectLeftBottom.Contains(p))
            {
                return HTLEFTBOTTOM;
            }
            //右下角
            Rectangle rectRightBottom = new Rectangle(BaseX + Width - _Left, BaseY + Height - _Bottom, _Right, _Bottom);
            if (rectRightBottom.Contains(p))
            {
                return HTRIGHTBOTTOM;
            }
            #endregion
            #region 关闭、最大化、最小化按钮判断
            Rectangle rectClose = new Rectangle(BaseX + Width - _Right - _MaxBtnWidth, BaseY + _Bottom, _MaxBtnWidth, _MaxBtnHeight);
            if (rectClose.Contains(p))
            {
                return SC_CLOSE;
            }
            Rectangle rectMax = new Rectangle(BaseX + Width - _Right - _MaxBtnWidth * 2, BaseY + _Bottom, _MaxBtnWidth, _MaxBtnHeight);
            if (rectMax.Contains(p))
            {
                return SC_MAXIMIZE;
            }
            Rectangle rectMin = new Rectangle(BaseX + Width - _Right - _MaxBtnWidth * 3, BaseY + _Bottom, _MaxBtnWidth, _MaxBtnHeight);
            if (rectMin.Contains(p))
            {
                return SC_MINIMIZE;
            }
            #endregion
            #region 边缘判断
            //上边缘
            Rectangle rectTop = new Rectangle(BaseX, BaseY, Width, _Bottom);
            if (rectTop.Contains(p))
            {
                return HTTOP;
            }
            //左边媛
            Rectangle rectLeft = new Rectangle(BaseX, BaseY + _Bottom, _Left, Height);
            if (rectLeft.Contains(p))
            {
                return HTLEFT;
            }
            //右边媛
            Rectangle rectRight = new Rectangle(BaseX + Width - _Right, BaseY + _Bottom, _Right, Height);
            if (rectRight.Contains(p))
            {
                return HTRIGHT;
            }
            //下边缘
            Rectangle rectBottom = new Rectangle(BaseX, BaseY + Height - _Bottom, Width, _Bottom);
            if (rectBottom.Contains(p))
            {
                return HTBOTTOM;
            }
            #endregion
            #region 标题
            //标题栏
            Rectangle rectCaption = new Rectangle(BaseX, BaseY + _Bottom, Width, _Top - _Bottom);
            if (rectCaption.Contains(p))
            {
                return HTCAPTION;
            }
            #endregion
            return HTCLIENT;//返回客户区消息
        }
        #endregion
        #region 根据鼠标在窗体上的位置返回不同消息的值，用于模拟非客户消息
        private IntPtr HitTestNCA()
        {
            return new IntPtr(getMousePosition());
        }
        #endregion
    }
}
