using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Drawing.Drawing2D;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

using static PowerLib.NativeCodes.NativeMethods;
using static PowerLib.NativeCodes.NativeConstants;

namespace PowerLib.Winform.Sample
{
    public partial class FrmXFormDockTest : XForm
    {
        public FrmXFormDockTest()
        {
            InitializeComponent();
        }

        private void xButton1_Click(object sender, EventArgs e)
        {
            PostMessage(Handle, WM_NCPAINT, 0, 0);

            //GraphicsPath gp = new GraphicsPath();
            //gp.AddRectangle(new Rectangle(0, Height - 2, Width, 2));
            //Region region = new Region(gp);
            //Graphics g = CreateGraphics();
            //IntPtr hRgn = region.GetHrgn(g);
            //SendMessage(Handle, WM_NCPAINT, (int)hRgn, 0);
        }
    }
}
