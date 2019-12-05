using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Drawing;
using System.Linq;
using System.Reflection;
using System.Runtime.InteropServices;
using System.Windows.Forms;

using PowerControl;

namespace ControlTest
{
    static class Program
    {
        [DllImport("Dwmapi.dll")]
        public static extern int DwmIsCompositionEnabled(out bool isEnabled);

        /// <summary>
        /// 应用程序的主入口点。
        /// </summary>
        [STAThread]
        static void Main(string[] args)
        {
            Application.EnableVisualStyles();
            Application.SetCompatibleTextRenderingDefault(false);
            XForm3.OverrideIcon = Icon.ExtractAssociatedIcon(Assembly.GetEntryAssembly()?.Location ?? throw new InvalidOperationException());
            Application.Run(new FrmXFormTest());
        }

    }
}
