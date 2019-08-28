using System;
using System.Drawing;
using System.Reflection;
using System.Windows.Forms;
using PowerControl;

namespace ControlTest
{
    static class Program
    {
        /// <summary>
        /// 应用程序的主入口点。
        /// </summary>
        [STAThread]
        static void Main(string[] args)
        {
            Application.EnableVisualStyles();
            Application.SetCompatibleTextRenderingDefault(false);
            XForm.OverrideIcon = Icon.ExtractAssociatedIcon(Assembly.GetEntryAssembly().Location);
            Application.Run(new FrmShadowPanelTest());
        }
    }
}
