using System;
using System.ComponentModel;
using System.Diagnostics;
using System.Runtime.InteropServices;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using System.Windows.Interop;
using System.Windows.Media;

using PowerLib.WPF.Tools;

using static PowerLib.NativeCodes.NativeConstants;
using static PowerLib.NativeCodes.NativeMethods;
using static PowerLib.NativeCodes.NativeStructures;

namespace PowerLib.WPF.Controls;

/// <summary>
/// 表示一个具有自定义外观和阴影效果的窗口。该窗口提供了可选的最小化、最大化/还原和关闭按钮，并允许用户通过拖动标题栏来移动窗口。窗口的外观和行为可以通过依赖属性进行定制。
/// <see cref="Window.ActualWidth"/>和<see cref="Window.ActualHeight"/>属性返回的值包含阴影部分将包含阴影宽度，使用<see cref="Window.InnerActualWidth"/>和<see cref="Window.InnerActualHeight"/>属性获取不包含阴影部分的实际宽度和高度。
/// </summary>
[TemplatePart(Name = PART_SHADOW_AND_BORDER, Type = typeof(Border))]
[TemplatePart(Name = PART_TITLE_BAR, Type = typeof(UIElement))]
[TemplatePart(Name = PART_MINIMIZE_BUTTON, Type = typeof(Button))]
[TemplatePart(Name = PART_MAX_RESTORE_BUTTON, Type = typeof(Button))]
[TemplatePart(Name = PART_CLOSE_BUTTON, Type = typeof(Button))]
[TemplatePart(Name = PART_CONTENT_PRESENTER, Type = typeof(ContentPresenter))]
public class Window : System.Windows.Window
{
    #region 常量

    private const string PART_SHADOW_AND_BORDER = "PART_ShadowAndBorder";
    private const string PART_TITLE_BAR = "PART_TitleBar";
    private const string PART_MINIMIZE_BUTTON = "PART_MinimizeButton";
    private const string PART_MAX_RESTORE_BUTTON = "PART_MaxRestoreButton";
    private const string PART_CLOSE_BUTTON = "PART_CloseButton";
    private const string PART_CONTENT_PRESENTER = "PART_ContentPresenter";

    /// <summary>
    /// 允许标题栏高度的最小值，单位为像素。过小的标题栏可能会导致窗口无法正常显示和操作。默认值为 20。
    /// </summary>
    private const int MIN_TITLE_BAR_HEIGHT = 20;

    /// <summary>
    /// 允许阴影宽度的最小值，单位为像素。过窄的阴影无意义，并可能会导致窗口边界异常显示。默认值为 8。
    /// </summary>
    private const int MIN_SHADOW_WIDTH = 8;

    #endregion 常量

    static Window()
    {
        DefaultStyleKeyProperty.OverrideMetadata(typeof(Window), new FrameworkPropertyMetadata(typeof(Window)));
    }

    #region 依赖属性

    public static readonly DependencyProperty ShowTitleBarProperty =
        DependencyProperty.Register(nameof(ShowTitleBar), typeof(bool), typeof(Window), new PropertyMetadata(true, OnShowTitleBarChanged));

    public static readonly DependencyProperty ShowMinButtonProperty =
        DependencyProperty.Register(nameof(ShowMinButton), typeof(bool), typeof(Window), new PropertyMetadata(true));

    public static readonly DependencyProperty ShowMaxButtonProperty =
        DependencyProperty.Register(nameof(ShowMaxButton), typeof(bool), typeof(Window), new PropertyMetadata(true));

    public static readonly DependencyProperty ShowCloseButtonProperty =
        DependencyProperty.Register(nameof(ShowCloseButton), typeof(bool), typeof(Window), new PropertyMetadata(true));

    public static readonly DependencyProperty TitleBarHeightProperty =
        DependencyProperty.Register(nameof(TitleBarHeight), typeof(double), typeof(Window), new PropertyMetadata(36D, OnTitleBarHeightChanged, CoerceTitleBarHeight));

    public static readonly DependencyProperty BorderWidthProperty =
        DependencyProperty.Register(nameof(BorderWidth), typeof(double), typeof(Window), new FrameworkPropertyMetadata(1D, FrameworkPropertyMetadataOptions.AffectsMeasure, OnShadowWidthOrBorderWidthChanged));

    public static readonly DependencyProperty ShadowWidthProperty =
        DependencyProperty.Register(nameof(ShadowWidth), typeof(double), typeof(Window), new FrameworkPropertyMetadata(50D, FrameworkPropertyMetadataOptions.AffectsMeasure, OnShadowWidthOrBorderWidthChanged, CoerceShadowWidth));

    public static readonly DependencyProperty ShadowOpacityProperty =
        DependencyProperty.Register(nameof(ShadowOpacity), typeof(double), typeof(Window), new FrameworkPropertyMetadata(.7D));

    public static readonly DependencyProperty ShowShadowProperty =
        DependencyProperty.Register(nameof(ShowShadow), typeof(bool), typeof(Window), new PropertyMetadata(true));

    public static readonly DependencyProperty CornerRadiusProperty =
        DependencyProperty.Register(nameof(CornerRadius), typeof(double), typeof(Window), new PropertyMetadata(8D, OnCornerRadiusChanged, CoerceCornerRadius));

    public new static readonly DependencyProperty BackgroundProperty =
        DependencyProperty.Register(nameof(Background), typeof(Brush), typeof(Window), new PropertyMetadata(Brushes.White));

    public new static readonly DependencyProperty BorderBrushProperty =
        DependencyProperty.Register(nameof(BorderBrush), typeof(Brush), typeof(Window), new PropertyMetadata(Brushes.Black));

    public static readonly DependencyProperty EnableEscCloseProperty =
        DependencyProperty.Register(nameof(EnableEscClose), typeof(bool), typeof(Window), new PropertyMetadata(true));

    #endregion 依赖属性

    private struct STYLESTRUCT
    {
        public WindowStylesEx StyleOld;
        public WindowStylesEx StyleNew;
    }

    /// <summary>
    /// 初始化<see cref="Window"/>的实例
    /// </summary>
    public Window()
    {
        WindowStyle = WindowStyle.None;
        ResizeMode = ResizeMode.CanResize;
        AllowsTransparency = true;

        _hWnd = new WindowInteropHelper(this).EnsureHandle();
        Loaded += Window_Loaded;
        PreviewKeyDown += (s, e) =>
        {
            if (EnableEscClose && e.Key == Key.Escape)
                Close();
        };
    }

    #region 字段

    // 窗口句柄
    private readonly IntPtr _hWnd;
    // 全局鼠标钩子ID
    private IntPtr _hookId = IntPtr.Zero;
    // 全局鼠标钩子回调
    private HookProc _hookProc;
    // 记录最大化前的阴影宽度、圆角半径，以便在还原窗口时恢复
    private double _lastShadowWidth, _lastCornerRadius;
    // 标记当前是否正在执行最大化或还原操作，以避免在调整窗口大小时重复修改尺寸
    private bool _isMaximizingOrRestoring;
    // 记录上一次的窗口状态，以便在状态改变时进行相应处理
    private WindowState _lastWindowState = WindowState.Normal;

    #endregion 字段

    #region 属性

    /// <summary>
    /// 获取或设置是否显示标题栏。默认值为 true。
    /// </summary>
    public bool ShowTitleBar
    {
        get => (bool)GetValue(ShowTitleBarProperty);
        set => SetValue(ShowTitleBarProperty, value);
    }

    /// <summary>
    /// 获取或设置是否显示最小化按钮。默认值为 true。
    /// </summary>
    public bool ShowMinButton
    {
        get => (bool)GetValue(ShowMinButtonProperty);
        set => SetValue(ShowMinButtonProperty, value);
    }

    /// <summary>
    /// 获取或设置是否显示最大化/还原按钮。默认值为 true。
    /// </summary>
    public bool ShowMaxButton
    {
        get => (bool)GetValue(ShowMaxButtonProperty);
        set => SetValue(ShowMaxButtonProperty, value);
    }

    /// <summary>
    /// 获取或设置是否显示关闭按钮。默认值为 true。
    /// </summary>
    public bool ShowCloseButton
    {
        get => (bool)GetValue(ShowCloseButtonProperty);
        set => SetValue(ShowCloseButtonProperty, value);
    }

    /// <summary>
    /// 获取或设置标题栏高度。默认值为 36。
    /// </summary>
    public double TitleBarHeight
    {
        get => (double)GetValue(TitleBarHeightProperty);
        set => SetValue(TitleBarHeightProperty, value);
    }

    /// <summary>
    /// 获取或设置窗口边框宽度。默认值为 1。
    /// </summary>
    public double BorderWidth
    {
        get => (double)GetValue(BorderWidthProperty);
        set => SetValue(BorderWidthProperty, value);
    }

    /// <summary>
    /// 获取或设置窗口阴影宽度。默认值为 50。
    /// </summary>
    public double ShadowWidth
    {
        get => (double)GetValue(ShadowWidthProperty);
        set => SetValue(ShadowWidthProperty, value);
    }

    /// <summary>
    /// 获取或设置窗口阴影不透明度。默认值为 0.7。
    /// </summary>
    public double ShadowOpacity
    {
        get => (double)GetValue(ShadowOpacityProperty);
        set => SetValue(ShadowOpacityProperty, value);
    }

    /// <summary>
    /// 获取或设置是否显示窗口阴影。默认值为 true。
    /// </summary>
    public bool ShowShadow
    {
        get => (bool)GetValue(ShowShadowProperty);
        set => SetValue(ShowShadowProperty, value);
    }

    /// <summary>
    /// 获取或设置窗口圆角半径。默认值为 8。
    /// </summary>
    public double CornerRadius
    {
        get => (double)GetValue(CornerRadiusProperty);
        set => SetValue(CornerRadiusProperty, value);
    }

    /// <summary>
    /// 获取或设置窗口背景。默认值为白色。
    /// </summary>
    public new Brush Background
    {
        get => (Brush)GetValue(BackgroundProperty);
        set => SetValue(BackgroundProperty, value);
    }

    /// <summary>
    /// 获取或设置窗口边框颜色。默认值为黑色。
    /// </summary>
    public new Brush BorderBrush
    {
        get => (Brush)GetValue(BorderBrushProperty);
        set => SetValue(BorderBrushProperty, value);
    }

    /// <summary>
    /// 获取或设置是否启用按下 Esc 键关闭窗口的功能。默认值为 true。
    /// </summary>
    public bool EnableEscClose
    {
        get => (bool)GetValue(EnableEscCloseProperty);
        set => SetValue(EnableEscCloseProperty, value);
    }

    /// <summary>
    /// 获取窗口的实际宽度（不包含阴影部分）。
    /// </summary>
    public double InnerActualWidth => ActualWidth - 2 * ShadowWidth;

    /// <summary>
    /// 获取窗口的实际高度（不包含阴影部分）。
    /// </summary>
    public double InnerActualHeight => ActualHeight - 2 * ShadowWidth;

    #endregion 属性

    #region 私有方法

    private static object CoerceShadowWidth(DependencyObject d, object baseValue)
    {
        Window wnd = (Window)d;
        double shadowWidth = (double)baseValue;

        return shadowWidth < MIN_SHADOW_WIDTH
            ? MIN_SHADOW_WIDTH
            : shadowWidth;
    }

    private static object CoerceCornerRadius(DependencyObject d, object baseValue)
    {
        Window wnd = (Window)d;
        double cornerRadius = (double)baseValue;

        return cornerRadius > wnd.TitleBarHeight
            ? wnd.TitleBarHeight
            : cornerRadius;
    }

    private static object CoerceTitleBarHeight(DependencyObject d, object baseValue)
    {
        Window wnd = (Window)d;
        double height = (double)baseValue;
        double minHeight = Math.Max(MIN_TITLE_BAR_HEIGHT, wnd.CornerRadius);

        return height < minHeight
            ? minHeight
            : height;
    }

    private static void OnTitleBarHeightChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
    {
        d.CoerceValue(CornerRadiusProperty);
    }

    private static void OnCornerRadiusChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
    {
        Window wnd = (Window)d;
        wnd.UpdateContentPresenterClip();
    }

    private static void OnShowTitleBarChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
    {
        Window wnd = (Window)d;
        wnd.UpdateContentPresenterClip();
    }

    private static void OnShadowWidthOrBorderWidthChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
    {
        if (d is not Window window || !window.IsLoaded)
            return;

        if (window._isMaximizingOrRestoring)
            return;

        double delta = (double)e.NewValue - (double)e.OldValue;

        if (!double.IsNaN(window.Width))
            window.Width += 2 * delta;

        if (!double.IsNaN(window.Height))
            window.Height += 2 * delta;

        double left = window.Left - delta;
        double top = window.Top - delta;

        window.Left = left;
        window.Top = top;
    }

    private static IntPtr WndProc(IntPtr hWnd, int msg, IntPtr wParam, IntPtr lParam, ref bool handled)
    {
        // Add WS_EX_LAYERED style
        if (msg != WM_STYLECHANGING || (int)wParam != GWL_EXSTYLE)
            return IntPtr.Zero;

        STYLESTRUCT styleStruct = (STYLESTRUCT)Marshal.PtrToStructure(lParam, typeof(STYLESTRUCT));
        styleStruct.StyleNew |= WindowStylesEx.WS_EX_LAYERED;
        Marshal.StructureToPtr(styleStruct, lParam, false);
        handled = true;

        return IntPtr.Zero;
    }

    private void SetTransparentHitThrough(bool hitThrough)
    {
        int newLong = GetWindowLong(_hWnd, WindowLongFlags.GWL_EXSTYLE);

        if (hitThrough)
            newLong |= (int)WindowStylesEx.WS_EX_TRANSPARENT;
        else
            newLong &= ~(int)WindowStylesEx.WS_EX_TRANSPARENT;

        SetWindowLong(_hWnd, GWL_EXSTYLE, newLong);
    }

    #endregion 私有方法

    #region 事件处理

    private void Window_Loaded(object sender, RoutedEventArgs e)
    {
        Width += ShadowWidth * 2;
        Height += ShadowWidth * 2;

        ((HwndSource)PresentationSource.FromVisual(this))?.AddHook(WndProc);

        IntPtr hModule = GetModuleHandle(null);

        // 鼠标钩子回调，实现阴影穿透
        _hookProc = (nCode, wParam, lParam) =>
        {
            if (Debugger.IsAttached || nCode < 0)
                return CallNextHookEx(_hookId, nCode, wParam, lParam);

            object obj = Marshal.PtrToStructure(lParam, typeof(MSLLHOOKSTRUCT));

            if (obj is not MSLLHOOKSTRUCT info)
                return CallNextHookEx(_hookId, nCode, wParam, lParam);

            double dpiScale = DpiHelper.GetDpiScaleX(this);

            Point ptRelative = new Point(info.pt.X / dpiScale - Left, info.pt.Y / dpiScale - Top);

            if (GetTemplateChild(PART_SHADOW_AND_BORDER) is not Border shadowBorder)
                return CallNextHookEx(_hookId, nCode, wParam, lParam);

            if (WindowState == WindowState.Maximized)
            {
                SetTransparentHitThrough(false);
                return CallNextHookEx(_hookId, nCode, wParam, lParam);
            }

            double shadowBorderWidth = ShadowWidth + BorderWidth;
            Rect wndRect = new Rect(shadowBorderWidth, shadowBorderWidth, shadowBorder.ActualWidth, shadowBorder.ActualHeight);
            Geometry roundedWndRect = new RectangleGeometry(wndRect, CornerRadius, CornerRadius);
            SetTransparentHitThrough(!roundedWndRect.FillContains(ptRelative));

            return CallNextHookEx(_hookId, nCode, wParam, lParam);
        };

        _hookId = SetWindowsHookEx(HookType.WH_MOUSE_LL, _hookProc, hModule, 0);

        if (_hookId == IntPtr.Zero)
            throw new Win32Exception(Marshal.GetLastWin32Error());
    }
    
    #endregion 事件处理

    #region 重写FrameworkElement成员

    public override void OnApplyTemplate()
    {
        base.OnApplyTemplate();

        if (GetTemplateChild(PART_TITLE_BAR) is UIElement titleBar)
        {
            titleBar.MouseLeftButtonDown += (_, e) =>
            {
                if (e.LeftButton != MouseButtonState.Pressed)
                    return;

                if (e.ClickCount == 2 && ShowMaxButton)
                    WindowState = WindowState == WindowState.Maximized ? WindowState.Normal : WindowState.Maximized;
                else
                    DragMove();
            };

            titleBar.MouseRightButtonDown += (_, e) =>
            {
                if (e.RightButton != MouseButtonState.Pressed)
                    return;

                IntPtr hMenu = GetSystemMenu(_hWnd, false);
                Point screenPos = PointToScreen(e.GetPosition(this));

                uint command = TrackPopupMenuEx(hMenu,
                    TrackPopupMenuFlags.TPM_LEFTALIGN | TrackPopupMenuFlags.TPM_RETURNCMD,
                    (int)screenPos.X, (int)screenPos.Y,
                    _hWnd,
                    IntPtr.Zero);

                if (command > 0)
                    PostMessage(_hWnd, WM_SYSCOMMAND, new IntPtr(command), IntPtr.Zero);
            };
        }

        if (GetTemplateChild(PART_MINIMIZE_BUTTON) is Button minBtn)
            minBtn.Click += (_, _) => WindowState = WindowState.Minimized;

        if (GetTemplateChild(PART_MAX_RESTORE_BUTTON) is Button maxBtn)
            maxBtn.Click += (_, _) => WindowState = WindowState == WindowState.Maximized ? WindowState.Normal : WindowState.Maximized;

        if (GetTemplateChild(PART_CLOSE_BUTTON) is Button closeBtn)
            closeBtn.Click += (_, _) => Close();
    }

    #endregion 重写FrameworkElement成员

    #region 重写Window成员

    protected override void OnClosed(EventArgs e)
    {
        base.OnClosed(e);

        if (_hookId == IntPtr.Zero)
            return;

        UnhookWindowsHookEx(_hookId);
        _hookId = IntPtr.Zero;
    }

    protected override void OnRenderSizeChanged(SizeChangedInfo sizeInfo)
    {
        base.OnRenderSizeChanged(sizeInfo);
        UpdateContentPresenterClip();
    }

    private void UpdateContentPresenterClip()
    {
        double cornerRadius = ShowTitleBar ? 0 : CornerRadius;

        if (GetTemplateChild(PART_CONTENT_PRESENTER) is ContentPresenter contentPresenter)
            contentPresenter.Clip = new RectangleGeometry(new Rect(0, 0, contentPresenter.ActualWidth, contentPresenter.ActualHeight), cornerRadius, cornerRadius);
    }

    protected override void OnStateChanged(EventArgs e)
    {
        base.OnStateChanged(e);

        switch (WindowState)
        {
            case WindowState.Normal:
                if (_lastWindowState == WindowState.Maximized)
                {
                    _isMaximizingOrRestoring = true;
                    ShadowWidth = _lastShadowWidth;
                    CornerRadius = _lastCornerRadius;
                    _isMaximizingOrRestoring = false;
                }
                break;
            case WindowState.Maximized:// 最大化时取消阴影、圆角
                _lastShadowWidth = ShadowWidth;
                _lastCornerRadius = CornerRadius;

                _isMaximizingOrRestoring = true;
                ShadowWidth = 0;
                CornerRadius = 0;
                _isMaximizingOrRestoring = false;
                break;
        }

        _lastWindowState = WindowState;
    }

    #endregion 重写Window成员
}