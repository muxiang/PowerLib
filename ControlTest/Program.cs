using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Reflection;
using System.Windows.Forms;
using PowerControl;
using SignalerTool.Forms;

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
            //Application.EnableVisualStyles();
            //Application.SetCompatibleTextRenderingDefault(false);
            //XForm.OverrideIcon = Icon.ExtractAssociatedIcon(Assembly.GetEntryAssembly().Location);
            //Application.Run(new FrmDetectorsSelect());

            new FrmDetectorsSelect().ShowDialog();
        }
    }
}
