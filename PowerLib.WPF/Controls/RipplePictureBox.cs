using System;
using System.ComponentModel;
using System.Diagnostics;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Threading;

namespace PowerLib.WPF.Controls;

/// <summary>
/// 表示一个带有水波特效的图片控件
/// </summary>
public class RipplePictureBox : Control
{
    // 常量
    private const int CLICK_DELAY = 1000;
    private const int AUTO_SPLASH_INTERVAL = 8000;
    private const int AUTO_SPLASH_STEP_DELAY = 30;
    
    static RipplePictureBox()
    {
        DefaultStyleKeyProperty.OverrideMetadata(typeof(RipplePictureBox), new FrameworkPropertyMetadata(typeof(RipplePictureBox)));
    }

    #region 依赖属性

    public static readonly DependencyProperty ImageProperty =
        DependencyProperty.Register(
            nameof(Image),
            typeof(ImageSource),
            typeof(RipplePictureBox),
            new PropertyMetadata(null, OnImageChanged));

    public static readonly DependencyProperty ClickSplashRadiusProperty =
        DependencyProperty.Register(
            nameof(ClickSplashRadius),
            typeof(int),
            typeof(RipplePictureBox),
            new FrameworkPropertyMetadata(12, FrameworkPropertyMetadataOptions.AffectsRender));

    public static readonly DependencyProperty DragSplashRadiusProperty =
        DependencyProperty.Register(
            nameof(DragSplashRadius),
            typeof(int),
            typeof(RipplePictureBox),
            new FrameworkPropertyMetadata(7, FrameworkPropertyMetadataOptions.AffectsRender));

    public static readonly DependencyProperty AnimationEnabledProperty =
        DependencyProperty.Register(
            nameof(AnimationEnabled),
            typeof(bool),
            typeof(RipplePictureBox),
            new FrameworkPropertyMetadata(true, OnAnimationEnabledChanged));

    public static readonly DependencyProperty RefreshRateProperty =
        DependencyProperty.Register(
            nameof(RefreshRate),
            typeof(int),
            typeof(RipplePictureBox),
            new FrameworkPropertyMetadata(60, OnRefreshRateChanged));

    public static readonly DependencyProperty AutoSplashProperty =
        DependencyProperty.Register(
            nameof(AutoSplash),
            typeof(bool),
            typeof(RipplePictureBox),
            new FrameworkPropertyMetadata(false, OnAutoSplashChanged));

    public static readonly DependencyProperty HoverSplashProperty =
        DependencyProperty.Register(
            nameof(HoverSplash),
            typeof(bool),
            typeof(RipplePictureBox),
            new FrameworkPropertyMetadata(false));

    #endregion 依赖属性

    /// <summary>
    /// 初始化<see cref="RipplePictureBox"/>的实例
    /// </summary>
    public RipplePictureBox()
    {
        MinWidth = 256;
        MinHeight = 256;

        // 渲染事件
        CompositionTarget.Rendering += OnCompositionRendering;

        // 鼠标事件
        MouseDown += OnMouseDown;
        MouseUp += OnMouseUp;
        MouseMove += OnMouseMove;

        // 初始化自动水波定时器
        _autoSplashTimer = new DispatcherTimer
        {
            Interval = TimeSpan.FromMilliseconds(AUTO_SPLASH_INTERVAL)
        };
        _autoSplashTimer.Tick += OnAutoSplashTick;
    }
    
    #region 字段

    private RippleEffect _effect;
    private readonly Stopwatch _swClick = new Stopwatch();
    private Point? _lastClick;
    private bool _isDragging;
    private WriteableBitmap _originalTexture; // 未缩放的原始纹理
    private Point[] _autoSplashPoints;
    private readonly DispatcherTimer _autoSplashTimer; // 自动水波定时器（WPF 线程安全）

    #endregion 字段

    #region 属性

    /// <summary>
    /// 获取或设置水波单击时的半径（像素）
    /// </summary>
    public int ClickSplashRadius
    {
        get => (int)GetValue(ClickSplashRadiusProperty);
        set => SetValue(ClickSplashRadiusProperty, value);
    }

    /// <summary>
    /// 获取或设置水波拖动/悬停时的半径（像素）
    /// </summary>
    public int DragSplashRadius
    {
        get => (int)GetValue(DragSplashRadiusProperty);
        set => SetValue(DragSplashRadiusProperty, value);
    }

    /// <summary>
    /// 获取或设置背景图像（必须为 BitmapSource）
    /// </summary>
    public ImageSource Image
    {
        get => (ImageSource)GetValue(ImageProperty);
        set => SetValue(ImageProperty, value);
    }

    /// <summary>
    /// 获取或设置是否启用动画
    /// </summary>
    public bool AnimationEnabled
    {
        get => (bool)GetValue(AnimationEnabledProperty);
        set => SetValue(AnimationEnabledProperty, value);
    }

    /// <summary>
    /// 获取或设置动画刷新率（1~60 FPS）
    /// </summary>
    public int RefreshRate
    {
        get => (int)GetValue(RefreshRateProperty);
        set => SetValue(RefreshRateProperty, value);
    }

    /// <summary>
    /// 获取或设置是否自动播放水波轨迹
    /// </summary>
    public bool AutoSplash
    {
        get => (bool)GetValue(AutoSplashProperty);
        set => SetValue(AutoSplashProperty, value);
    }

    /// <summary>
    /// 获取或设置是否在鼠标悬停时生成水波（无需按下）
    /// </summary>
    public bool HoverSplash
    {
        get => (bool)GetValue(HoverSplashProperty);
        set => SetValue(HoverSplashProperty, value);
    }

    #endregion 属性

    #region 依赖属性变更回调

    private static void OnImageChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
    {
        RipplePictureBox ctrl = (RipplePictureBox)d;
        ctrl.OnImageChanged((ImageSource)e.OldValue, (ImageSource)e.NewValue);
    }

    private static void OnAnimationEnabledChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
    {
        RipplePictureBox ctrl = (RipplePictureBox)d;
        if (!(bool)e.NewValue)
            ctrl.Clear();
    }

    private static void OnRefreshRateChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
    {
        int rate = (int)e.NewValue;

        if (rate is < 1 or > 60)
            throw new ArgumentOutOfRangeException(nameof(RefreshRate), "刷新率必须在 1 到 60 之间。");
    }

    private static void OnAutoSplashChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
    {
        RipplePictureBox ctrl = (RipplePictureBox)d;
        bool enabled = (bool)e.NewValue;

        if (DesignerProperties.GetIsInDesignMode(ctrl))
            return;

        if (enabled)
            ctrl._autoSplashTimer.Start();
        else
            ctrl._autoSplashTimer.Stop();
    }

    #endregion 依赖属性变更回调

    #region 公开方法

    /// <summary>
    /// 在指定位置产生水波（控件坐标 → 纹理坐标）
    /// </summary>
    public void Splash(double x, double y, int radius)
    {
        if (_effect == null || _originalTexture == null)
            return;

        // 将控件坐标映射到原始纹理坐标
        double scaleX = _originalTexture.PixelWidth / ActualWidth;
        double scaleY = _originalTexture.PixelHeight / ActualHeight;
        int tx = (int)Math.Round(x * scaleX);
        int ty = (int)Math.Round(y * scaleY);

        // 边界保护
        tx = Math.Max(0, Math.Min(tx, _originalTexture.PixelWidth - 1));
        ty = Math.Max(0, Math.Min(ty, _originalTexture.PixelHeight - 1));

        _effect.Splash(tx, ty, radius);
    }

    /// <summary>
    /// 清除所有水波
    /// </summary>
    public void Clear()
    {
        _effect?.Clear();
        InvalidateVisual();
    }

    #endregion 公开方法

    #region 重写UIElement

    protected override void OnRender(DrawingContext drawingContext)
    {
        base.OnRender(drawingContext);

        if (_effect == null || _effect.Texture == null)
        {
            drawingContext.DrawRectangle(Brushes.White, null, new Rect(0, 0, ActualWidth, ActualHeight));
            return;
        }

        // 渲染水波帧
        WriteableBitmap frame = _effect.Render();

        // 绘制到控件（拉伸填充）
        drawingContext.DrawImage(frame, new Rect(0, 0, ActualWidth, ActualHeight));
    }

    protected override void OnRenderSizeChanged(SizeChangedInfo sizeInfo)
    {
        base.OnRenderSizeChanged(sizeInfo);
        GenerateAutoSplashPoints();
    }

    #endregion 重写UIElement

    #region 私有方法

    #region 事件处理

    private void OnCompositionRendering(object sender, EventArgs e)
    {
        if (DesignerProperties.GetIsInDesignMode(this))
            return;

        if (!AnimationEnabled || _effect == null)
            return;

        _effect.Update();
        InvalidateVisual(); // 触发 OnRender
    }

    private void OnMouseDown(object sender, MouseButtonEventArgs e)
    {
        if (ComparePoint(_lastClick, e.GetPosition(this), 10) && _swClick.ElapsedMilliseconds < CLICK_DELAY)
            return;

        _swClick.Restart();
        _lastClick = e.GetPosition(this);
        Splash((int)_lastClick.Value.X, (int)_lastClick.Value.Y, ClickSplashRadius);
        _isDragging = true;
        e.Handled = true;
    }

    private void OnMouseUp(object sender, MouseButtonEventArgs e)
    {
        _isDragging = false;
        e.Handled = true;
    }

    private void OnMouseMove(object sender, MouseEventArgs e)
    {
        if (!_isDragging && !HoverSplash) 
            return;

        Point pos = e.GetPosition(this);
        Splash((int)pos.X, (int)pos.Y, DragSplashRadius);
    }

    private void OnAutoSplashTick(object sender, EventArgs e)
    {
        if (_autoSplashPoints == null || _effect == null)
            return;

        // 启动一个异步序列模拟自动水波
        _ = AutoSplashSequence();
    }
    
    #endregion 事件处理

    private void OnImageChanged(ImageSource oldImage, ImageSource newImage)
    {
        if (newImage == null)
        {
            _effect = null;
            _originalTexture = null;
            InvalidateVisual();
            return;
        }

        if (newImage is not BitmapSource bmp)
            throw new ArgumentException("仅支持 BitmapSource 类型的图像。", nameof(Image));

        // 转换为 Pbgra32 WriteableBitmap（确保格式兼容）
        FormatConvertedBitmap converted = new FormatConvertedBitmap(bmp, PixelFormats.Pbgra32, null, 0);
        _originalTexture = new WriteableBitmap(converted);

        _effect = new RippleEffect(_originalTexture);

        GenerateAutoSplashPoints();
        InvalidateVisual();
    }
    
    private async System.Threading.Tasks.Task AutoSplashSequence()
    {
        foreach (Point pt in _autoSplashPoints)
        {
            if (DesignerProperties.GetIsInDesignMode(this) || _effect == null)
                break;

            await Dispatcher.BeginInvoke(new Action(() =>
            {
                Splash(pt.X, pt.Y, DragSplashRadius);
            }));

            await System.Threading.Tasks.Task.Delay(AUTO_SPLASH_STEP_DELAY);
        }
    }

    private bool ComparePoint(Point? pt1, Point? pt2, int diff)
    {
        if (!pt1.HasValue || !pt2.HasValue)
            return false;

        return Math.Abs(pt1.Value.X - pt2.Value.X) < diff && Math.Abs(pt1.Value.Y - pt2.Value.Y) < diff;
    }

    /// <summary>
    /// 生成自动水波的预设点（基于原始纹理尺寸的相对坐标）
    /// </summary>
    private void GenerateAutoSplashPoints()
    {
        if (ActualWidth <= 0 || ActualHeight <= 0 || _originalTexture == null)
            return;

        double scaleX = ActualWidth / (double)_originalTexture.PixelWidth;
        double scaleY = ActualHeight / (double)_originalTexture.PixelHeight;
        
        Point[] arrPoints = new[]
        {
            new Point((1 / 11.6666666666667), (1 / 5.33333333333333)),
            new Point((1 / 11.6666666666667), (1 / 5.22448979591837)),
            new Point((1 / 11.6666666666667), (1 / 4.92307692307692)),
            new Point((1 / 11.25), (1 / 4.74074074074074)),
            new Point((1 / 11.25), (1 / 4.57142857142857)),
            new Point((1 / 10.8620689655172), (1 / 4.33898305084746)),
            new Point((1 / 10.8620689655172), (1 / 4.26666666666667)),
            new Point((1 / 10.1612903225806), (1 / 3.87878787878788)),
            new Point((1 / 9.26470588235294), (1 / 3.50684931506849)),
            new Point((1 / 9.26470588235294), (1 / 3.45945945945946)),
            new Point((1 / 8.75), (1 / 3.2)),
            new Point((1 / 8.75), (1 / 3.16049382716049)),
            new Point((1 / 8.28947368421053), (1 / 2.97674418604651)),
            new Point((1 / 8.28947368421053), (1 / 2.94252873563218)),
            new Point((1 / 7.77777777777778), (1 / 2.75268817204301)),
            new Point((1 / 7.41176470588235), (1 / 2.58585858585859)),
            new Point((1 / 7.32558139534884), (1 / 2.58585858585859)),
            new Point((1 / 6.84782608695652), (1 / 2.43809523809524)),
            new Point((1 / 6.84782608695652), (1 / 2.41509433962264)),
            new Point((1 / 6.49484536082474), (1 / 2.32727272727273)),
            new Point((1 / 6.42857142857143), (1 / 2.30630630630631)),
            new Point((1 / 5.94339622641509), (1 / 2.18803418803419)),
            new Point((1 / 5.57522123893805), (1 / 2.08130081300813)),
            new Point((1 / 5.43103448275862), (1 / 2.06451612903226)),
            new Point((1 / 5.16393442622951), 1 / 2D),
            new Point((1 / 5.08064516129032), (1 / 1.96923076923077)),
            new Point((1 / 4.56521739130435), (1 / 1.85507246376812)),
            new Point((1 / 4.5), (1 / 1.85507246376812)),
            new Point((1 / 4.17218543046358), (1 / 1.76551724137931)),
            new Point((1 / 4.09090909090909), (1 / 1.76551724137931)),
            new Point((1 / 3.88888888888889), (1 / 1.71812080536913)),
            new Point((1 / 3.68421052631579), (1 / 1.67320261437909)),
            new Point((1 / 3.6), (1 / 1.65161290322581)),
            new Point((1 / 3.5593220338983), (1 / 1.65161290322581)),
            new Point((1 / 3.38709677419355), (1 / 1.62025316455696)),
            new Point((1 / 3.33333333333333), (1 / 1.61006289308176)),
            new Point((1 / 3.23076923076923), (1 / 1.58024691358025)),
            new Point((1 / 3.19796954314721), (1 / 1.58024691358025)),
            new Point((1 / 3.01435406698565), (1 / 1.55151515151515)),
            new Point((1 / 2.95774647887324), (1 / 1.5421686746988)),
            new Point((1 / 2.82511210762332), (1 / 1.51479289940828)),
            new Point((1 / 2.78761061946903), (1 / 1.51479289940828)),
            new Point((1 / 2.625), (1 / 1.47126436781609)),
            new Point((1 / 2.59259259259259), (1 / 1.46285714285714)),
            new Point((1 / 2.5609756097561), (1 / 1.46285714285714)),
            new Point((1 / 2.4901185770751), (1 / 1.43820224719101)),
            new Point((1 / 2.47058823529412), (1 / 1.43820224719101)),
            new Point((1 / 2.45136186770428), (1 / 1.43820224719101)),
            new Point((1 / 2.28260869565217), (1 / 1.41436464088398)),
            new Point((1 / 2.25), (1 / 1.40659340659341)),
            new Point((1 / 2.15017064846416), (1 / 1.38378378378378)),
            new Point((1 / 2.13559322033898), (1 / 1.38378378378378)),
            new Point((1 / 2.02572347266881), (1 / 1.36898395721925)),
            new Point((1 / 2.00636942675159), (1 / 1.36170212765957)),
            new Point((1 / 1.94444444444444), (1 / 1.35449735449735)),
            new Point((1 / 1.92660550458716), (1 / 1.35449735449735)),
            new Point((1 / 1.86390532544379), (1 / 1.33333333333333)),
            new Point((1 / 1.85840707964602), (1 / 1.33333333333333)),
            new Point((1 / 1.81556195965418), (1 / 1.31958762886598)),
            new Point((1 / 1.80515759312321), (1 / 1.31958762886598)),
            new Point((1 / 1.73553719008264), (1 / 1.2994923857868)),
            new Point((1 / 1.71662125340599), (1 / 1.28643216080402)),
            new Point((1 / 1.66666666666667), (1 / 1.27363184079602)),
            new Point((1 / 1.6622691292876), (1 / 1.27363184079602)),
            new Point((1 / 1.62790697674419), (1 / 1.24878048780488)),
            new Point((1 / 1.62371134020619), (1 / 1.24878048780488)),
            new Point((1 / 1.58291457286432), (1 / 1.24271844660194)),
            new Point((1 / 1.571072319202), (1 / 1.23076923076923)),
            new Point((1 / 1.51442307692308), (1 / 1.22488038277512)),
            new Point((1 / 1.5), (1 / 1.22488038277512)),
            new Point((1 / 1.441647597254), (1 / 1.21904761904762)),
            new Point((1 / 1.42857142857143), (1 / 1.21327014218009)),
            new Point((1 / 1.33757961783439), (1 / 1.18518518518519)),
            new Point((1 / 1.32631578947368), (1 / 1.18518518518519)),
            new Point((1 / 1.27016129032258), (1 / 1.18518518518519)),
            new Point((1 / 1.25498007968127), (1 / 1.18518518518519)),
            new Point((1 / 1.21153846153846), (1 / 1.17972350230415)),
            new Point((1 / 1.20458891013384), (1 / 1.17972350230415)),
            new Point((1 / 1.17537313432836), (1 / 1.17972350230415)),
            new Point((1 / 1.17100371747212), (1 / 1.17972350230415)),
            new Point((1 / 1.13924050632911), (1 / 1.17972350230415)),
            new Point((1 / 1.11111111111111), (1 / 1.17972350230415)),
            new Point((1 / 1.10720562390158), (1 / 1.17972350230415)),
            new Point((1 / 1.10332749562172), (1 / 1.17972350230415)),
            new Point((1 / 4.96062992125984), (1 / 3.65714285714286)),
            new Point((1 / 4.921875), (1 / 3.65714285714286)),
            new Point((1 / 4.77272727272727), (1 / 3.65714285714286)),
            new Point((1 / 4.73684210526316), (1 / 3.65714285714286)),
            new Point((1 / 4.56521739130435), (1 / 3.65714285714286)),
            new Point((1 / 4.28571428571429), (1 / 3.76470588235294)),
            new Point((1 / 3.91304347826087), (1 / 3.87878787878788)),
            new Point((1 / 3.31578947368421), (1 / 3.87878787878788)),
            new Point((1 / 2.98578199052133), (1 / 3.87878787878788)),
            new Point((1 / 2.76315789473684), (1 / 3.87878787878788)),
            new Point((1 / 2.58196721311475), (1 / 3.87878787878788)),
            new Point((1 / 2.5609756097561), (1 / 3.87878787878788)),
            new Point((1 / 2.4609375), (1 / 3.87878787878788)),
            new Point((1 / 2.42307692307692), (1 / 3.87878787878788)),
            new Point((1 / 2.29090909090909), (1 / 3.87878787878788)),
            new Point((1 / 2.26618705035971), (1 / 3.87878787878788)),
            new Point((1 / 2.17241379310345), (1 / 3.87878787878788)),
            new Point((1 / 2.15753424657534), (1 / 3.87878787878788)),
            new Point((1 / 2.11409395973154), (1 / 3.87878787878788)),
            new Point((1 / 2.10702341137124), (1 / 3.87878787878788)),
            new Point((1 / 2.04545454545455), (1 / 3.82089552238806)),
            new Point((1 / 2.03883495145631), (1 / 3.82089552238806)),
            new Point((1 / 1.93846153846154), (1 / 3.6056338028169)),
            new Point((1 / 1.90332326283988), (1 / 3.50684931506849)),
            new Point((1 / 1.81034482758621), (1 / 3.41333333333333)),
            new Point((1 / 1.80515759312321), (1 / 3.41333333333333)),
            new Point((1 / 1.76470588235294), (1 / 3.32467532467532)),
            new Point((1 / 1.71195652173913), (1 / 3.1219512195122)),
            new Point((1 / 1.61538461538462), (1 / 2.81318681318681)),
            new Point((1 / 1.5989847715736), (1 / 2.81318681318681)),
            new Point((1 / 1.52173913043478), (1 / 2.63917525773196)),
            new Point((1 / 1.50717703349282), (1 / 2.58585858585859)),
            new Point((1 / 1.46171693735499), (1 / 2.48543689320388)),
            new Point((1 / 1.45496535796767), (1 / 2.46153846153846)),
            new Point((1 / 1.41891891891892), (1 / 2.37037037037037)),
            new Point((1 / 1.40939597315436), (1 / 2.34862385321101)),
            new Point((1 / 1.37855579868709), (1 / 2.26548672566372)),
            new Point((1 / 1.37254901960784), (1 / 2.22608695652174)),
            new Point((1 / 1.34903640256959), (1 / 2.15126050420168)),
            new Point((1 / 1.30705394190871), (1 / 2.048)),
            new Point((1 / 1.26760563380282), (1 / 1.93939393939394)),
            new Point((1 / 1.24505928853755), (1 / 1.86861313868613)),
            new Point((1 / 1.21856866537718), (1 / 1.77777777777778)),
            new Point((1 / 1.21387283236994), (1 / 1.77777777777778)),
            new Point((1 / 1.19092627599244), (1 / 1.6953642384106)),
            new Point((1 / 1.18867924528302), (1 / 1.67320261437909)),
            new Point((1 / 1.18421052631579), (1 / 1.66233766233766)),
            new Point((1 / 1.17100371747212), (1 / 1.61006289308176)),
            new Point((1 / 1.16883116883117), (1 / 1.61006289308176)),
            new Point((1 / 1.15808823529412), (1 / 1.5609756097561)),
            new Point((1 / 1.15596330275229), (1 / 1.5421686746988)),
            new Point((1 / 1.13924050632911), (1 / 1.48837209302326)),
            new Point((1 / 1.11504424778761), (1 / 1.37634408602151)),
            new Point((1 / 1.11111111111111), (1 / 1.36170212765957)),
            new Point((1 / 1.09565217391304), (1 / 1.30612244897959)),
            new Point((1 / 1.09375), (1 / 1.2994923857868)),
            new Point((1 / 1.08247422680412), (1 / 1.25490196078431)),
            new Point((1 / 1.07692307692308), (1 / 1.22488038277512)),
            new Point((1 / 1.07508532423208), (1 / 1.21904761904762)),
            new Point((1 / 1.07142857142857), (1 / 1.1906976744186)),
            new Point((1 / 1.06779661016949), (1 / 1.17972350230415)),
            new Point((1 / 1.06418918918919), (1 / 1.16363636363636)),
            new Point((1 / 2.33333333333333), (1 / 15.0588235294118)),
            new Point((1 / 2.29090909090909), (1 / 13.4736842105263)),
            new Point((1 / 2.29090909090909), (1 / 11.1304347826087)),
            new Point((1 / 2.27436823104693), (1 / 10.6666666666667)),
            new Point((1 / 2.27436823104693), (1 / 10.24)),
            new Point((1 / 2.27436823104693), (1 / 8.53333333333333)),
            new Point((1 / 2.25806451612903), (1 / 7.75757575757576)),
            new Point((1 / 2.25806451612903), (1 / 5.56521739130435)),
            new Point((1 / 2.25806451612903), (1 / 5.33333333333333)),
            new Point((1 / 2.24199288256228), (1 / 4.12903225806452)),
            new Point((1 / 2.24199288256228), (1 / 3.93846153846154)),
            new Point((1 / 2.24199288256228), (1 / 3.82089552238806)),
            new Point((1 / 2.24199288256228), (1 / 3.36842105263158)),
            new Point((1 / 2.24199288256228), (1 / 3.28205128205128)),
            new Point((1 / 2.24199288256228), (1 / 2.87640449438202)),
            new Point((1 / 2.24199288256228), (1 / 2.78260869565217)),
            new Point((1 / 2.29090909090909), (1 / 2.39252336448598)),
            new Point((1 / 2.31617647058824), (1 / 2.28571428571429)),
            new Point((1 / 2.40458015267176), (1 / 1.96923076923077)),
            new Point((1 / 2.47058823529412), (1 / 1.81560283687943)),
            new Point((1 / 2.47058823529412), (1 / 1.79020979020979)),
            new Point((1 / 2.53012048192771), (1 / 1.70666666666667)),
            new Point((1 / 2.54032258064516), (1 / 1.68421052631579)),
            new Point((1 / 2.61410788381743), (1 / 1.59006211180124)),
            new Point((1 / 2.63598326359833), (1 / 1.57055214723926)),
            new Point((1 / 2.69230769230769), (1 / 1.52380952380952)),
            new Point((1 / 2.70386266094421), (1 / 1.51479289940828)),
            new Point((1 / 2.75109170305677), (1 / 1.47976878612717)),
            new Point((1 / 2.77533039647577), (1 / 1.47976878612717)),
            new Point((1 / 2.85067873303167), (1 / 1.44632768361582)),
            new Point((1 / 2.87671232876712), (1 / 1.44632768361582)),
            new Point((1 / 2.94392523364486), (1 / 1.43016759776536)),
            new Point((1 / 2.95774647887324), (1 / 1.43016759776536)),
            new Point((1 / 3.10344827586207), (1 / 1.40659340659341)),
            new Point((1 / 3.13432835820896), (1 / 1.40659340659341)),
            new Point((1 / 3.46153846153846), (1 / 1.39130434782609)),
            new Point((1 / 3.75), (1 / 1.36898395721925)),
            new Point((1 / 4.01273885350319), (1 / 1.36170212765957)),
            new Point((1 / 4.06451612903226), (1 / 1.36170212765957)),
            new Point((1 / 4.22818791946309), (1 / 1.36170212765957)),
            new Point((1 / 4.25675675675676), (1 / 1.36170212765957)),
            new Point((1 / 4.375), (1 / 1.36170212765957)),
            new Point((1 / 4.5), (1 / 1.36170212765957)),
        };
        
        _autoSplashPoints = new Point[arrPoints.Length];

        for (int i = 0; i < arrPoints.Length; i++)
        {
            double u = arrPoints[i].X; // 相对宽度比例 (0~1)
            double v = arrPoints[i].Y; // 相对高度比例 (0~1)
            _autoSplashPoints[i] = new Point(u * ActualWidth, v * ActualHeight);
        }
    }

    #endregion 私有方法
}