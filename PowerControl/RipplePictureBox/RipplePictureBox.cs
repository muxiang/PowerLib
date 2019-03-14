using System;
using System.ComponentModel;
using System.Diagnostics;
using System.Drawing;
using System.Globalization;
using System.Threading;
using System.Windows.Forms;

using PowerControl.Design;

using static PowerControl.NativeConstants;
using static PowerControl.NativeStructures;
using static PowerControl.NativeMethods;

using Timer = System.Windows.Forms.Timer;

namespace PowerControl
{
    /// <summary>
    /// 表示附带水波特效的图片框
    /// </summary>
    public sealed class RipplePictureBox : PictureBox
    {
        #region 字段

        //特效实例
        private RippleEffect _effect;
        //渲染定时器
        private readonly Timer _timer;
        //自动划动定时器
        private readonly Timer _timerAutoSplash;
        //是否正在拖动
        private bool _dragging;
        //上一次单击点
        private Point? _lastClick;
        //单击计时
        private Stopwatch _swClick;
        //未缩放的原始底图
        private Bitmap _bmpOrigin;

        //自动轨迹
        private Point[] _autoSplashPoints;
        //自动划动
        private bool _autoSplash;

        #endregion 字段

        #region 常量

        //核定每秒帧数
        private const int FPS = 60;
        //单击延迟
        private const int ClickDelay = 1000;

        #endregion 常量

        /// <summary>
        /// 在渲染到缓冲区后与绘制到<see cref="RipplePictureBox"/>之前触发，
        /// 订阅该事件，使用绘图参数中的<see cref="Graphics"/>绘制额外内容
        /// </summary>
        public event EventHandler<PaintEventArgs> AfterDrawing;

        #region 构造器

        /// <summary>
        /// 初始化水波特效图片框
        /// </summary>
        public RipplePictureBox()
        {
            MinimumSize = new Size(256, 256);

            //缺省水波半径
            ClickSplashRadius = 12;
            DragSplashRadius = 10;

            _dragging = false;

            _timer = new Timer { Interval = 1000 / FPS };
            _timer.Tick += (s1, e1) => UpdateFrame();

            _timerAutoSplash = new Timer { Interval = 1 };
            _timerAutoSplash.Tick += (s1, e1) =>
            {
                if (_timerAutoSplash.Interval == 1)
                    _timerAutoSplash.Interval = 8000;

                ThreadPool.QueueUserWorkItem(o =>
                {
                    foreach (Point pt in _autoSplashPoints)
                    {
                        if (Disposing || IsDisposed)
                            return;

                        BeginInvoke(new MethodInvoker(() => { Splash(pt.X, pt.Y, DragSplashRadius); }));
                        Thread.Sleep(30);
                    }
                });
            };

            MouseDown += (s1, e1) =>
            {
                //由于算法缺陷,避免短时间内在同一点生成大量水波
                if (ComparePoint(_lastClick, e1.Location, 10) && _swClick != null && _swClick.ElapsedMilliseconds < ClickDelay) return;

                _swClick = Stopwatch.StartNew();
                _lastClick = new Point(e1.X, e1.Y);
                Splash(e1.Location.X, e1.Location.Y, ClickSplashRadius);
                _dragging = true;
            };

            MouseUp += (s1, e1) => { _dragging = false; };

            MouseMove += (s1, e1) =>
            {
                if (_dragging || HoverSplash)
                    Splash(e1.Location.X, e1.Location.Y, DragSplashRadius);
            };

            _timer.Start();
        }

        #endregion 构造器

        #region 属性

        /// <summary>
        /// 设置水波单击时半径
        /// </summary>
        [Browsable(true)]
        [Category("Appearance")]
        [DefaultValue(12)]
        [Description("获取或设置水波单击时半径")]
        public int ClickSplashRadius { get; set; }

        /// <summary>
        /// 设置水波拖动时半径
        /// </summary>
        [Browsable(true)]
        [Category("Appearance")]
        [DefaultValue(7)]
        [Description("获取或设置水波拖动时半径")]
        public int DragSplashRadius { get; set; }

        /// <summary>
        /// 设置水波背景图
        /// </summary>
        [Browsable(true)]
        [Category("Appearance")]
        [DefaultValue(null)]
        [Description("获取或设置水波背景图")]
        public new Image Image
        {
            get => _effect?.Texture;
            set
            {
                if (value == null) return;
                _bmpOrigin = new Bitmap(value);
                //Bitmap texture = Utilities.StretchBitmap(_bmpOrigin, Size);
                _effect?.Dispose();
                _effect = new RippleEffect(_bmpOrigin);
            }
        }

        /// <summary>
        /// 启用或禁用动画
        /// </summary>
        [Browsable(true)]
        [Category("Behavior")]
        [DefaultValue(true)]
        [Description("获取或设置启用或禁用动画")]
        public bool AnimationEnabled
        {
            get => _timer.Enabled;
            set
            {
                _timer.Enabled = value;
                if (!value) Clear();
            }
        }

        /// <summary>
        /// 自动划动水波
        /// </summary>
        [Browsable(true)]
        [Category("Behavior")]
        [DefaultValue(false)]
        [Description("获取或设置自动划动水波")]
        public bool AutoSplash
        {
            get => _autoSplash;
            set
            {
                _autoSplash = value;
                _timerAutoSplash.Enabled = !DesignerUtil.IsDesignMode() && value;
            }
        }

        /// <summary>
        /// 鼠标附上划动水波
        /// 不需要按下鼠标按键
        /// </summary>
        [Browsable(true)]
        [Category("Behavior")]
        [DefaultValue(false)]
        [Description("获取或设置鼠标附上时划动水波")]
        public bool HoverSplash { get; set; }

        #endregion 属性

        #region 公开方法

        /// <summary>
        /// 在指定区域创建水波
        /// </summary>
        /// <param name="x">X坐标</param>
        /// <param name="y">Y坐标</param>
        /// <param name="radius">半径</param>
        public void Splash(int x, int y, int radius)
        {
            Point lc = new Point(x, y);
            lc = Translate(lc);
            _effect?.Splash(lc.X, lc.Y, radius);
            //Debug.Print($"new Point((int)(Width / {Width / (double)x}), (int)(Height / {Height / (double)y})),");
        }

        /// <summary>
        /// 清除浪高数据
        /// </summary>
        public void Clear()
        {
            _effect?.Clear();
        }

        #endregion 公开方法

        #region 重写

        /// <summary>
        /// 窗口函数
        /// </summary>
        /// <param name="m"></param>
        protected override void WndProc(ref Message m)
        {
            switch (m.Msg)
            {
                //禁止擦除背景
                case WM_ERASEBKGND:
                    return;
                //绘图
                case WM_PAINT:
                    PAINTSTRUCT paintStruct = new PAINTSTRUCT();
                    //开始绘图,获取设备上下文(dc)指针
                    IntPtr wndHdc = BeginPaint(m.HWnd, ref paintStruct);

                    if (_effect?.Texture != null)
                    {
                        //生成一帧动画
                        Bitmap f = _effect.Render();

                        //绘制额外内容
                        using (Graphics g = Graphics.FromImage(f))
                            OnAfterDrawing(new PaintEventArgs(g, new Rectangle(Point.Empty, f.Size)));

                        //渲染到窗口
                        using (Graphics gWnd = Graphics.FromHdc(wndHdc))
                            gWnd.DrawImage(f, ClientRectangle);
                    }

                    //结束绘图
                    EndPaint(m.HWnd, ref paintStruct);
                    return;
                default:
                    base.WndProc(ref m);
                    break;
            }
        }

        /// <inheritdoc />
        protected override void OnSizeChanged(EventArgs e)
        {
            base.OnSizeChanged(e);

            _autoSplashPoints = new[]
            {
                new Point((int)(Width / 11.6666666666667), (int)(Height / 5.33333333333333)),
                new Point((int)(Width / 11.6666666666667), (int)(Height / 5.22448979591837)),
                new Point((int)(Width / 11.6666666666667), (int)(Height / 4.92307692307692)),
                new Point((int)(Width / 11.25), (int)(Height / 4.74074074074074)),
                new Point((int)(Width / 11.25), (int)(Height / 4.57142857142857)),
                new Point((int)(Width / 10.8620689655172), (int)(Height / 4.33898305084746)),
                new Point((int)(Width / 10.8620689655172), (int)(Height / 4.26666666666667)),
                new Point((int)(Width / 10.1612903225806), (int)(Height / 3.87878787878788)),
                new Point((int)(Width / 9.26470588235294), (int)(Height / 3.50684931506849)),
                new Point((int)(Width / 9.26470588235294), (int)(Height / 3.45945945945946)),
                new Point((int)(Width / 8.75), (int)(Height / 3.2)),
                new Point((int)(Width / 8.75), (int)(Height / 3.16049382716049)),
                new Point((int)(Width / 8.28947368421053), (int)(Height / 2.97674418604651)),
                new Point((int)(Width / 8.28947368421053), (int)(Height / 2.94252873563218)),
                new Point((int)(Width / 7.77777777777778), (int)(Height / 2.75268817204301)),
                new Point((int)(Width / 7.41176470588235), (int)(Height / 2.58585858585859)),
                new Point((int)(Width / 7.32558139534884), (int)(Height / 2.58585858585859)),
                new Point((int)(Width / 6.84782608695652), (int)(Height / 2.43809523809524)),
                new Point((int)(Width / 6.84782608695652), (int)(Height / 2.41509433962264)),
                new Point((int)(Width / 6.49484536082474), (int)(Height / 2.32727272727273)),
                new Point((int)(Width / 6.42857142857143), (int)(Height / 2.30630630630631)),
                new Point((int)(Width / 5.94339622641509), (int)(Height / 2.18803418803419)),
                new Point((int)(Width / 5.57522123893805), (int)(Height / 2.08130081300813)),
                new Point((int)(Width / 5.43103448275862), (int)(Height / 2.06451612903226)),
                new Point((int)(Width / 5.16393442622951), Height / 2),
                new Point((int)(Width / 5.08064516129032), (int)(Height / 1.96923076923077)),
                new Point((int)(Width / 4.56521739130435), (int)(Height / 1.85507246376812)),
                new Point((int)(Width / 4.5), (int)(Height / 1.85507246376812)),
                new Point((int)(Width / 4.17218543046358), (int)(Height / 1.76551724137931)),
                new Point((int)(Width / 4.09090909090909), (int)(Height / 1.76551724137931)),
                new Point((int)(Width / 3.88888888888889), (int)(Height / 1.71812080536913)),
                new Point((int)(Width / 3.68421052631579), (int)(Height / 1.67320261437909)),
                new Point((int)(Width / 3.6), (int)(Height / 1.65161290322581)),
                new Point((int)(Width / 3.5593220338983), (int)(Height / 1.65161290322581)),
                new Point((int)(Width / 3.38709677419355), (int)(Height / 1.62025316455696)),
                new Point((int)(Width / 3.33333333333333), (int)(Height / 1.61006289308176)),
                new Point((int)(Width / 3.23076923076923), (int)(Height / 1.58024691358025)),
                new Point((int)(Width / 3.19796954314721), (int)(Height / 1.58024691358025)),
                new Point((int)(Width / 3.01435406698565), (int)(Height / 1.55151515151515)),
                new Point((int)(Width / 2.95774647887324), (int)(Height / 1.5421686746988)),
                new Point((int)(Width / 2.82511210762332), (int)(Height / 1.51479289940828)),
                new Point((int)(Width / 2.78761061946903), (int)(Height / 1.51479289940828)),
                new Point((int)(Width / 2.625), (int)(Height / 1.47126436781609)),
                new Point((int)(Width / 2.59259259259259), (int)(Height / 1.46285714285714)),
                new Point((int)(Width / 2.5609756097561), (int)(Height / 1.46285714285714)),
                new Point((int)(Width / 2.4901185770751), (int)(Height / 1.43820224719101)),
                new Point((int)(Width / 2.47058823529412), (int)(Height / 1.43820224719101)),
                new Point((int)(Width / 2.45136186770428), (int)(Height / 1.43820224719101)),
                new Point((int)(Width / 2.28260869565217), (int)(Height / 1.41436464088398)),
                new Point((int)(Width / 2.25), (int)(Height / 1.40659340659341)),
                new Point((int)(Width / 2.15017064846416), (int)(Height / 1.38378378378378)),
                new Point((int)(Width / 2.13559322033898), (int)(Height / 1.38378378378378)),
                new Point((int)(Width / 2.02572347266881), (int)(Height / 1.36898395721925)),
                new Point((int)(Width / 2.00636942675159), (int)(Height / 1.36170212765957)),
                new Point((int)(Width / 1.94444444444444), (int)(Height / 1.35449735449735)),
                new Point((int)(Width / 1.92660550458716), (int)(Height / 1.35449735449735)),
                new Point((int)(Width / 1.86390532544379), (int)(Height / 1.33333333333333)),
                new Point((int)(Width / 1.85840707964602), (int)(Height / 1.33333333333333)),
                new Point((int)(Width / 1.81556195965418), (int)(Height / 1.31958762886598)),
                new Point((int)(Width / 1.80515759312321), (int)(Height / 1.31958762886598)),
                new Point((int)(Width / 1.73553719008264), (int)(Height / 1.2994923857868)),
                new Point((int)(Width / 1.71662125340599), (int)(Height / 1.28643216080402)),
                new Point((int)(Width / 1.66666666666667), (int)(Height / 1.27363184079602)),
                new Point((int)(Width / 1.6622691292876), (int)(Height / 1.27363184079602)),
                new Point((int)(Width / 1.62790697674419), (int)(Height / 1.24878048780488)),
                new Point((int)(Width / 1.62371134020619), (int)(Height / 1.24878048780488)),
                new Point((int)(Width / 1.58291457286432), (int)(Height / 1.24271844660194)),
                new Point((int)(Width / 1.571072319202), (int)(Height / 1.23076923076923)),
                new Point((int)(Width / 1.51442307692308), (int)(Height / 1.22488038277512)),
                new Point((int)(Width / 1.5), (int)(Height / 1.22488038277512)),
                new Point((int)(Width / 1.441647597254), (int)(Height / 1.21904761904762)),
                new Point((int)(Width / 1.42857142857143), (int)(Height / 1.21327014218009)),
                new Point((int)(Width / 1.33757961783439), (int)(Height / 1.18518518518519)),
                new Point((int)(Width / 1.32631578947368), (int)(Height / 1.18518518518519)),
                new Point((int)(Width / 1.27016129032258), (int)(Height / 1.18518518518519)),
                new Point((int)(Width / 1.25498007968127), (int)(Height / 1.18518518518519)),
                new Point((int)(Width / 1.21153846153846), (int)(Height / 1.17972350230415)),
                new Point((int)(Width / 1.20458891013384), (int)(Height / 1.17972350230415)),
                new Point((int)(Width / 1.17537313432836), (int)(Height / 1.17972350230415)),
                new Point((int)(Width / 1.17100371747212), (int)(Height / 1.17972350230415)),
                new Point((int)(Width / 1.13924050632911), (int)(Height / 1.17972350230415)),
                new Point((int)(Width / 1.11111111111111), (int)(Height / 1.17972350230415)),
                new Point((int)(Width / 1.10720562390158), (int)(Height / 1.17972350230415)),
                new Point((int)(Width / 1.10332749562172), (int)(Height / 1.17972350230415)),
                new Point((int)(Width / 4.96062992125984), (int)(Height / 3.65714285714286)),
                new Point((int)(Width / 4.921875), (int)(Height / 3.65714285714286)),
                new Point((int)(Width / 4.77272727272727), (int)(Height / 3.65714285714286)),
                new Point((int)(Width / 4.73684210526316), (int)(Height / 3.65714285714286)),
                new Point((int)(Width / 4.56521739130435), (int)(Height / 3.65714285714286)),
                new Point((int)(Width / 4.28571428571429), (int)(Height / 3.76470588235294)),
                new Point((int)(Width / 3.91304347826087), (int)(Height / 3.87878787878788)),
                new Point((int)(Width / 3.31578947368421), (int)(Height / 3.87878787878788)),
                new Point((int)(Width / 2.98578199052133), (int)(Height / 3.87878787878788)),
                new Point((int)(Width / 2.76315789473684), (int)(Height / 3.87878787878788)),
                new Point((int)(Width / 2.58196721311475), (int)(Height / 3.87878787878788)),
                new Point((int)(Width / 2.5609756097561), (int)(Height / 3.87878787878788)),
                new Point((int)(Width / 2.4609375), (int)(Height / 3.87878787878788)),
                new Point((int)(Width / 2.42307692307692), (int)(Height / 3.87878787878788)),
                new Point((int)(Width / 2.29090909090909), (int)(Height / 3.87878787878788)),
                new Point((int)(Width / 2.26618705035971), (int)(Height / 3.87878787878788)),
                new Point((int)(Width / 2.17241379310345), (int)(Height / 3.87878787878788)),
                new Point((int)(Width / 2.15753424657534), (int)(Height / 3.87878787878788)),
                new Point((int)(Width / 2.11409395973154), (int)(Height / 3.87878787878788)),
                new Point((int)(Width / 2.10702341137124), (int)(Height / 3.87878787878788)),
                new Point((int)(Width / 2.04545454545455), (int)(Height / 3.82089552238806)),
                new Point((int)(Width / 2.03883495145631), (int)(Height / 3.82089552238806)),
                new Point((int)(Width / 1.93846153846154), (int)(Height / 3.6056338028169)),
                new Point((int)(Width / 1.90332326283988), (int)(Height / 3.50684931506849)),
                new Point((int)(Width / 1.81034482758621), (int)(Height / 3.41333333333333)),
                new Point((int)(Width / 1.80515759312321), (int)(Height / 3.41333333333333)),
                new Point((int)(Width / 1.76470588235294), (int)(Height / 3.32467532467532)),
                new Point((int)(Width / 1.71195652173913), (int)(Height / 3.1219512195122)),
                new Point((int)(Width / 1.61538461538462), (int)(Height / 2.81318681318681)),
                new Point((int)(Width / 1.5989847715736), (int)(Height / 2.81318681318681)),
                new Point((int)(Width / 1.52173913043478), (int)(Height / 2.63917525773196)),
                new Point((int)(Width / 1.50717703349282), (int)(Height / 2.58585858585859)),
                new Point((int)(Width / 1.46171693735499), (int)(Height / 2.48543689320388)),
                new Point((int)(Width / 1.45496535796767), (int)(Height / 2.46153846153846)),
                new Point((int)(Width / 1.41891891891892), (int)(Height / 2.37037037037037)),
                new Point((int)(Width / 1.40939597315436), (int)(Height / 2.34862385321101)),
                new Point((int)(Width / 1.37855579868709), (int)(Height / 2.26548672566372)),
                new Point((int)(Width / 1.37254901960784), (int)(Height / 2.22608695652174)),
                new Point((int)(Width / 1.34903640256959), (int)(Height / 2.15126050420168)),
                new Point((int)(Width / 1.30705394190871), (int)(Height / 2.048)),
                new Point((int)(Width / 1.26760563380282), (int)(Height / 1.93939393939394)),
                new Point((int)(Width / 1.24505928853755), (int)(Height / 1.86861313868613)),
                new Point((int)(Width / 1.21856866537718), (int)(Height / 1.77777777777778)),
                new Point((int)(Width / 1.21387283236994), (int)(Height / 1.77777777777778)),
                new Point((int)(Width / 1.19092627599244), (int)(Height / 1.6953642384106)),
                new Point((int)(Width / 1.18867924528302), (int)(Height / 1.67320261437909)),
                new Point((int)(Width / 1.18421052631579), (int)(Height / 1.66233766233766)),
                new Point((int)(Width / 1.17100371747212), (int)(Height / 1.61006289308176)),
                new Point((int)(Width / 1.16883116883117), (int)(Height / 1.61006289308176)),
                new Point((int)(Width / 1.15808823529412), (int)(Height / 1.5609756097561)),
                new Point((int)(Width / 1.15596330275229), (int)(Height / 1.5421686746988)),
                new Point((int)(Width / 1.13924050632911), (int)(Height / 1.48837209302326)),
                new Point((int)(Width / 1.11504424778761), (int)(Height / 1.37634408602151)),
                new Point((int)(Width / 1.11111111111111), (int)(Height / 1.36170212765957)),
                new Point((int)(Width / 1.09565217391304), (int)(Height / 1.30612244897959)),
                new Point((int)(Width / 1.09375), (int)(Height / 1.2994923857868)),
                new Point((int)(Width / 1.08247422680412), (int)(Height / 1.25490196078431)),
                new Point((int)(Width / 1.07692307692308), (int)(Height / 1.22488038277512)),
                new Point((int)(Width / 1.07508532423208), (int)(Height / 1.21904761904762)),
                new Point((int)(Width / 1.07142857142857), (int)(Height / 1.1906976744186)),
                new Point((int)(Width / 1.06779661016949), (int)(Height / 1.17972350230415)),
                new Point((int)(Width / 1.06418918918919), (int)(Height / 1.16363636363636)),
                new Point((int)(Width / 2.33333333333333), (int)(Height / 15.0588235294118)),
                new Point((int)(Width / 2.29090909090909), (int)(Height / 13.4736842105263)),
                new Point((int)(Width / 2.29090909090909), (int)(Height / 11.1304347826087)),
                new Point((int)(Width / 2.27436823104693), (int)(Height / 10.6666666666667)),
                new Point((int)(Width / 2.27436823104693), (int)(Height / 10.24)),
                new Point((int)(Width / 2.27436823104693), (int)(Height / 8.53333333333333)),
                new Point((int)(Width / 2.25806451612903), (int)(Height / 7.75757575757576)),
                new Point((int)(Width / 2.25806451612903), (int)(Height / 5.56521739130435)),
                new Point((int)(Width / 2.25806451612903), (int)(Height / 5.33333333333333)),
                new Point((int)(Width / 2.24199288256228), (int)(Height / 4.12903225806452)),
                new Point((int)(Width / 2.24199288256228), (int)(Height / 3.93846153846154)),
                new Point((int)(Width / 2.24199288256228), (int)(Height / 3.82089552238806)),
                new Point((int)(Width / 2.24199288256228), (int)(Height / 3.36842105263158)),
                new Point((int)(Width / 2.24199288256228), (int)(Height / 3.28205128205128)),
                new Point((int)(Width / 2.24199288256228), (int)(Height / 2.87640449438202)),
                new Point((int)(Width / 2.24199288256228), (int)(Height / 2.78260869565217)),
                new Point((int)(Width / 2.29090909090909), (int)(Height / 2.39252336448598)),
                new Point((int)(Width / 2.31617647058824), (int)(Height / 2.28571428571429)),
                new Point((int)(Width / 2.40458015267176), (int)(Height / 1.96923076923077)),
                new Point((int)(Width / 2.47058823529412), (int)(Height / 1.81560283687943)),
                new Point((int)(Width / 2.47058823529412), (int)(Height / 1.79020979020979)),
                new Point((int)(Width / 2.53012048192771), (int)(Height / 1.70666666666667)),
                new Point((int)(Width / 2.54032258064516), (int)(Height / 1.68421052631579)),
                new Point((int)(Width / 2.61410788381743), (int)(Height / 1.59006211180124)),
                new Point((int)(Width / 2.63598326359833), (int)(Height / 1.57055214723926)),
                new Point((int)(Width / 2.69230769230769), (int)(Height / 1.52380952380952)),
                new Point((int)(Width / 2.70386266094421), (int)(Height / 1.51479289940828)),
                new Point((int)(Width / 2.75109170305677), (int)(Height / 1.47976878612717)),
                new Point((int)(Width / 2.77533039647577), (int)(Height / 1.47976878612717)),
                new Point((int)(Width / 2.85067873303167), (int)(Height / 1.44632768361582)),
                new Point((int)(Width / 2.87671232876712), (int)(Height / 1.44632768361582)),
                new Point((int)(Width / 2.94392523364486), (int)(Height / 1.43016759776536)),
                new Point((int)(Width / 2.95774647887324), (int)(Height / 1.43016759776536)),
                new Point((int)(Width / 3.10344827586207), (int)(Height / 1.40659340659341)),
                new Point((int)(Width / 3.13432835820896), (int)(Height / 1.40659340659341)),
                new Point((int)(Width / 3.46153846153846), (int)(Height / 1.39130434782609)),
                new Point((int)(Width / 3.75), (int)(Height / 1.36898395721925)),
                new Point((int)(Width / 4.01273885350319), (int)(Height / 1.36170212765957)),
                new Point((int)(Width / 4.06451612903226), (int)(Height / 1.36170212765957)),
                new Point((int)(Width / 4.22818791946309), (int)(Height / 1.36170212765957)),
                new Point((int)(Width / 4.25675675675676), (int)(Height / 1.36170212765957)),
                new Point((int)(Width / 4.375), (int)(Height / 1.36170212765957)),
                new Point((int)(Width / 4.5), (int)(Height / 1.36170212765957)),
            };
        }

        #endregion 重写

        #region 私有方法

        //更新一帧
        private void UpdateFrame()
        {
            if (DesignMode) return;
            if (_effect == null) return;
            //更新特效缓冲区浪高数据
            _effect.Update();
            //强制重绘
            Invalidate();
        }

        /// <summary>
        /// 判断两个点的X相差或Y相差是否大于指定值
        /// </summary>
        /// <param name="pt1"></param>
        /// <param name="pt2"></param>
        /// <param name="diff"></param>
        /// <returns></returns>
        private static bool ComparePoint(Point? pt1, Point? pt2, int diff)
        {
            if (pt1 == null || pt2 == null) return false;
            int xDiff = pt1.Value.X - pt2.Value.X;
            int yDiff = pt1.Value.Y - pt2.Value.Y;

            return Math.Abs(xDiff) < diff && Math.Abs(yDiff) < diff;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="point"></param>
        /// <returns></returns>
        private Point Translate(Point point)
        {
            double cx = (double)Image.Width / Width;
            double cy = (double)Image.Height / Height;
            return new Point((int)(cx * point.X), (int)(cy * point.Y));
        }

        #endregion 私有方法

        /// <summary>
        /// 引发<see cref="AfterDrawing"/>事件
        /// </summary>
        /// <param name="e">绘图时间参数</param>
        private void OnAfterDrawing(PaintEventArgs e)
        {
            AfterDrawing?.Invoke(this, e);
        }
    }
}
