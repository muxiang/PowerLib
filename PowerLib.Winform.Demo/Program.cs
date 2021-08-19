using System;
using System.Drawing;
using System.Reflection;
using System.Windows.Forms;

namespace PowerLib.Winform.Demo
{
    static class Program
    {
        /// <summary>
        /// 应用程序的主入口点。
        /// </summary>
        [STAThread]
        static void Main()
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
