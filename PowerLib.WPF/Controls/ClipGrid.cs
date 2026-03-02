using System.Windows;
using System.Windows.Controls;
using System.Windows.Media;

namespace PowerLib.WPF.Controls;

internal class ClipGrid : Grid
{
    public static readonly DependencyProperty CornerRadiusProperty =
        DependencyProperty.Register(nameof(CornerRadius), typeof(CornerRadius), typeof(ClipGrid), new PropertyMetadata(new CornerRadius(0), OnCornerRadiusChanged));

    public CornerRadius CornerRadius
    {
        get => (CornerRadius)GetValue(CornerRadiusProperty);
        set => SetValue(CornerRadiusProperty, value);
    }

    private static void OnCornerRadiusChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
    {
        ((ClipGrid)d).UpdateClip();
    }

    protected override void OnRenderSizeChanged(SizeChangedInfo sizeInfo)
    {
        base.OnRenderSizeChanged(sizeInfo);
        UpdateClip();
    }

    private void UpdateClip()
    {
        RectangleGeometry rect = new RectangleGeometry(
            new Rect(0, 0, ActualWidth, ActualHeight),
            CornerRadius.TopLeft,
            CornerRadius.TopLeft);

        Clip = rect;

        foreach (UIElement child in InternalChildren)
        {
            child.Clip = rect;
            
            if (child is Border)
                break;
        }
    }
}