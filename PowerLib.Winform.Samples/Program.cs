using System;
using System.Drawing;
using System.Reflection;
using System.Windows.Forms;

namespace PowerLib.Winform.Samples
{
    static class Program
    {
        /// <summary>
        /// 应用程序的主入口点。
        /// </summary>
        [STAThread]
        static void Main()
        {
            XForm.OverrideIcon = Icon.ExtractAssociatedIcon(Assembly.GetEntryAssembly()?.Location ?? throw new InvalidOperationException());
            XForm.DefaultTitleBarStartColor = Color.PaleVioletRed;
            XForm.DefaultTitleBarEndColor = Color.Pink;
            XForm.DefaultTitleBarForeColor = Color.White;

            Application.EnableVisualStyles();
            Application.SetCompatibleTextRenderingDefault(false);
            Application.Run(new FrmXFormTest());
        }
    }
}
