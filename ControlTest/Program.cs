using System;
using System.Drawing;
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
            if(DwmIsCompositionEnabled(out bool isEnabled) == 0)
            MessageBox.Show("isEnabled="+isEnabled);
            Application.EnableVisualStyles();
            Application.SetCompatibleTextRenderingDefault(false);
            XForm.OverrideIcon = Icon.ExtractAssociatedIcon(Assembly.GetEntryAssembly().Location);
            Application.Run(new MyForm2Test());
        }
    }
}
