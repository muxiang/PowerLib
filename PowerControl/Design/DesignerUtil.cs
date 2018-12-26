using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PowerControl.Design
{
    public static class DesignerUtil
    {
        public static bool IsDesignMode()
        {
            return LicenseManager.UsageMode == LicenseUsageMode.Designtime || Process.GetCurrentProcess().ProcessName == "devenv";
        }
    }
}
