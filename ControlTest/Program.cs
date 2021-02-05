using System;
using System.Drawing;
using System.Reflection;
using System.Windows.Forms;

using PowerControl;

namespace ControlTest
{
    internal static class Program
    {
        /// <summary>
        /// 应用程序的主入口点。
        /// </summary>
        [STAThread]
        private static void Main()
        {
            Application.EnableVisualStyles();
            Application.SetCompatibleTextRenderingDefault(false);
            XForm.OverrideIcon = Icon.ExtractAssociatedIcon(Assembly.GetEntryAssembly()?.Location ?? throw new InvalidOperationException());
            XForm.DefaultTitleBarStartColor = Color.PaleVioletRed;
            XForm.DefaultTitleBarEndColor = Color.Pink;
            XForm.DefaultTitleBarForeColor = Color.White;

            Application.Run(new FrmXFormTest());
        }
    }
}
