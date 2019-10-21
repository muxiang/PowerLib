using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using PowerControl;

namespace ControlTest
{
    public partial class FrmDrawTest : Form
    {
        public FrmDrawTest()
        {
            InitializeComponent();

            SetStyle(ControlStyles.DoubleBuffer | ControlStyles.UserPaint | ControlStyles.AllPaintingInWmPaint, true);
        }

        protected override void OnPaint(PaintEventArgs e)
        {
            base.OnPaint(e);

            Point ptCenter = new Point(300, 300);
            float radius = 200;

            PointF[] polygon = Utilities.CreateRegularPolygon(ptCenter, radius, 1000, 30);

            //e.Graphics.DrawEllipse(Pens.Red, ptCenter.X - radius, ptCenter.Y - radius, radius * 2, radius * 2);

            //foreach (PointF pt in polygon)
            //    e.Graphics.DrawLine(Pens.Red, ptCenter, pt);

            e.Graphics.DrawPolygon(Pens.Black, polygon);
        }
    }
}
