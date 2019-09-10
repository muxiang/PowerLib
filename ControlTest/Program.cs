using System;
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
            //Application.EnableVisualStyles();
            //Application.SetCompatibleTextRenderingDefault(false);
            //XForm.OverrideIcon = Icon.ExtractAssociatedIcon(Assembly.GetEntryAssembly().Location);
            //Application.Run(new FrmShadowPanelTest());

            const string str = "ass5*-swSDAds23";
            MessageBox.Show(new string(str.Select(c => c >= 'A' && c <= 'Z' ? (char)(c + 32) : (c >= 'a' && c <= 'z' ? (char)(c - 32) : c)).ToArray()));


        }

        private static readonly Random RandomSeed = new Random();

        private static int R(int x,int y)
        {
            return RandomSeed.Next(x + 1, y + 1);
        }
    }
}
