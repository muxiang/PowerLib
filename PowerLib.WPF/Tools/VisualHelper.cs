using System.Windows;

namespace PowerLib.WPF.Tools
{
    /// <summary>
    /// 可视化辅助类
    /// </summary>
    public static class VisualHelper
    {
        /// <summary>
        /// 控制控件在状态改变（如 MouseOver, IsPressed）时是否执行动画过渡（如果支持），默认值是false。
        /// </summary>
        public static readonly DependencyProperty IsVisualStateAnimatedProperty
            = DependencyProperty.RegisterAttached("IsVisualStateAnimated", typeof(bool), typeof(VisualHelper),
                new FrameworkPropertyMetadata(false, FrameworkPropertyMetadataOptions.Inherits));

        public static void SetIsVisualStateAnimated(DependencyObject element, bool value)
            => element.SetValue(IsVisualStateAnimatedProperty, value);

        public static bool GetIsVisualStateAnimated(DependencyObject element)
            => (bool)element.GetValue(IsVisualStateAnimatedProperty);
    }
}
