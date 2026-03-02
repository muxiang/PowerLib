using System;
using System.Collections.ObjectModel;
using System.Windows;

namespace PowerLib.WPF.Tools;

/// <summary>
/// 封装皮肤应用的工具类，提供方法来动态加载和切换 PowerLib.WPF 应用程序的皮肤资源字典。
/// </summary>
public static class SkinHelper
{
    public static readonly string SkinDefaultUri = "pack://application:,,,/PowerLib.WPF;component/Themes/SkinDefault.xaml";
    public static readonly string SkinDarkUri = "pack://application:,,,/PowerLib.WPF;component/Themes/SkinDark.xaml";
    public static readonly string SkinPinkUri = "pack://application:,,,/PowerLib.WPF;component/Themes/SkinPink.xaml";

    /// <summary>
    /// 应用指定的皮肤资源字典到当前应用程序。<br/>
    /// 该方法会从参数 <see cref="strSkinFileUri"/> 中加载指定的 XAML 文件，并将其作为新的资源字典应用到应用程序的资源集合中。<br/>
    /// 调用前应确保默认皮肤资源字典和主题资源字典分别位于 Application.Current.Resources.MergedDictionaries 的第一和第二位置，以便正确移除旧的皮肤资源字典。<br/>
    /// 通过创建自定义的颜色资源字典，并按照PowerLib.WPF/Themes/Basic/ColorsDefault.xaml中的Key值覆盖对应颜色值。<br/>
    /// 随后将创建的资源字典uri作为参数传递给此方法，可以实现对应用程序皮肤的个性化定制。
    /// </summary>
    /// <param name="strSkinFileUri"></param>
    public static void ApplySkin(string strSkinFileUri)
    {
        ResourceDictionary rdSkin = new() { Source = new Uri(strSkinFileUri) };
        ResourceDictionary rdTheme = new() { Source = new Uri("pack://application:,,,/PowerLib.WPF;component/Themes/Generic.xaml") };

        Collection<ResourceDictionary> appMergedDictionaries = Application.Current.Resources.MergedDictionaries;

        if (appMergedDictionaries.Count > 1)
        {
            appMergedDictionaries.RemoveAt(0);
            appMergedDictionaries.RemoveAt(0);
        }

        appMergedDictionaries.Insert(0, rdSkin);
        appMergedDictionaries.Insert(1, rdTheme);
    }
}