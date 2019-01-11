using System.ComponentModel;
using System.Diagnostics;

namespace PowerControl.Design
{
    /// <summary>
    /// 设计器工具
    /// </summary>
    public static class DesignerUtil
    {
        /// <summary>
        /// 是否处于设计模式
        /// </summary>
        /// <returns></returns>
        public static bool IsDesignMode()
        {
            return LicenseManager.UsageMode == LicenseUsageMode.Designtime || Process.GetCurrentProcess().ProcessName == "devenv";
        }
    }
}
