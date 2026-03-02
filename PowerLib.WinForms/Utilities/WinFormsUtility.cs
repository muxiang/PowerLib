using System.Windows.Forms;

namespace PowerLib.WinForms.Utilities;

internal class WinFormsUtility
{
    /// <summary>
    /// 递归获取控件所在的父窗口
    /// </summary>
    /// <param name="c">控件</param>
    /// <returns>父窗口引用</returns>
    public static Form GetParentForm(Control c)
    {
        return c.Parent switch
        {
            null => null,
            Form parent => parent,
            _ => GetParentForm(c.Parent),
        };
    }
}