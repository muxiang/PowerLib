using System;
using System.Collections.Generic;
using System.Threading;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Media;
using System.Windows.Threading;

using NativeWindow = System.Windows.Window;

namespace PowerLib.WPF.Controls
{
    /// <summary>
    /// 提供在任意 WPF 窗口上显示加载遮罩层的静态方法。
    /// 通过动态注入覆盖层实现，适用于任何 WPF 窗口，无论其根元素类型。
    /// </summary>
    public static class LoadingLayer
    {
        /// <summary>
        /// 表示单个窗口的遮罩层状态
        /// </summary>
        private struct LayerState
        {
            public Grid WrapperGrid;
            public Grid OverlayGrid;
            public UIElement OriginalContent;
            public LoadingCircle Circle;
            public EnhancedProgressBar ProgressBar;
        }

        // 按窗口实例追踪遮罩层状态，确保同一窗口同一时刻只有一个遮罩层
        private static readonly Dictionary<NativeWindow, LayerState> States = new();

        #region 公开方法

        /// <summary>
        /// 在指定窗口上显示遮罩层
        /// </summary>
        /// <param name="owner">目标窗口</param>
        /// <param name="opacity">遮罩层不透明度（0~1），默认 0.5</param>
        /// <param name="showProgressBar">是否显示进度条</param>
        /// <param name="overlayColor">遮罩层背景色，默认黑色</param>
        public static void Show(
            NativeWindow owner,
            double opacity = 0.5,
            bool showProgressBar = false,
            Color? overlayColor = null)
        {
            if (owner == null)
                throw new ArgumentNullException(nameof(owner));

            Color color = overlayColor ?? Colors.Black;

            InvokeOnUI(owner.Dispatcher, () =>
            {
                if (States.ContainsKey(owner))
                    return;

                LayerState state = new()
                {
                    // 保存原始内容
                    OriginalContent = owner.Content as UIElement,
                    // 创建包装 Grid，将原始内容和覆盖层放在同一格中实现叠加
                    WrapperGrid = new Grid()
                };

                owner.Content = null;
                if (state.OriginalContent != null)
                    state.WrapperGrid.Children.Add(state.OriginalContent);

                // 构建覆盖层
                state.OverlayGrid = new Grid
                {
                    Background = new SolidColorBrush(color) { Opacity = opacity },
                    Cursor = System.Windows.Input.Cursors.Wait
                };

                StackPanel centerPanel = new()
                {
                    HorizontalAlignment = HorizontalAlignment.Center,
                    VerticalAlignment = VerticalAlignment.Center
                };

                double circleSize = Math.Min(300, Math.Min(owner.ActualWidth, owner.ActualHeight) / 2);

                state.Circle = new LoadingCircle
                {
                    Width = circleSize,
                    Height = circleSize,
                    IsActive = true
                };

                centerPanel.Children.Add(state.Circle);

                if (showProgressBar)
                {
                    state.ProgressBar = new EnhancedProgressBar
                    {
                        Width = circleSize,
                        Height = 40,
                        Minimum = 0,
                        Maximum = 100,
                        Value = 0,
                        Margin = new Thickness(0, 16, 0, 0),
                        ShowTextOutline = true,
                        TextOutlineThickness = 0.9
                    };

                    centerPanel.Children.Add(state.ProgressBar);
                }

                state.OverlayGrid.Children.Add(centerPanel);
                state.WrapperGrid.Children.Add(state.OverlayGrid);

                owner.Content = state.WrapperGrid;
                States[owner] = state;
            });
        }

        /// <summary>
        /// 关闭指定窗口上的遮罩层，还原窗口原始内容
        /// </summary>
        /// <param name="owner">目标窗口</param>
        public static void Close(NativeWindow owner)
        {
            if (owner == null)
                throw new ArgumentNullException(nameof(owner));

            InvokeOnUI(owner.Dispatcher, () =>
            {
                if (!States.TryGetValue(owner, out LayerState state))
                    return;

                if (state.Circle != null)
                    state.Circle.IsActive = false;

                if (state.OriginalContent != null && state.WrapperGrid != null)
                    state.WrapperGrid.Children.Remove(state.OriginalContent);

                owner.Content = state.OriginalContent;
                States.Remove(owner);
            });
        }

        /// <summary>
        /// 显示遮罩层，在线程池线程执行工作项委托，并在执行完毕后自动关闭遮罩层
        /// </summary>
        /// <param name="owner">目标窗口</param>
        /// <param name="action">遮罩层显示期间需要执行的委托，参数为目标窗口</param>
        /// <param name="opacity">遮罩层不透明度（0~1），默认 0.5</param>
        /// <param name="showProgressBar">是否显示进度条</param>
        /// <param name="overlayColor">遮罩层背景色，默认黑色</param>
        public static void ShowAutoClose(
            NativeWindow owner,
            Action<NativeWindow> action,
            double opacity = 0.5,
            bool showProgressBar = false,
            Color? overlayColor = null)
        {
            if (owner == null)
                throw new ArgumentNullException(nameof(owner));
            if (action == null)
                throw new ArgumentNullException(nameof(action));

            Show(owner, opacity, showProgressBar, overlayColor);

            ThreadPool.QueueUserWorkItem(_ =>
            {
                try
                {
                    action(owner);
                }
                finally
                {
                    Close(owner);
                }
            });
        }

        /// <summary>
        /// 更新指定窗口上遮罩层的进度（线程安全）
        /// </summary>
        /// <param name="owner">目标窗口</param>
        /// <param name="value">进度值（0-100）</param>
        /// <param name="text">进度文本</param>
        public static void UpdateProgress(NativeWindow owner, int value, string text = null)
        {
            if (owner == null)
                return;

            value = Math.Max(0, Math.Min(100, value));

            InvokeOnUI(owner.Dispatcher, () =>
            {
                if (!States.TryGetValue(owner, out LayerState state))
                    return;

                if (state.ProgressBar == null)
                    return;

                state.ProgressBar.Value = value;

                if (text != null)
                    state.ProgressBar.Text = text;
            });
        }

        #endregion 公开方法

        #region 辅助方法

        private static void InvokeOnUI(Dispatcher dispatcher, Action action)
        {
            if (dispatcher.CheckAccess())
                action();
            else
                dispatcher.Invoke(action);
        }

        #endregion 辅助方法
    }
}