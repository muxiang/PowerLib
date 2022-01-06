using System;
using System.Threading;
using System.Windows.Forms;

using PowerLib.Winform.Controls;

namespace PowerLib.Winform.Sample
{
    public partial class FrmXFileProgressBarTest : XForm
    {
        private readonly Random _rnd = new Random();

        private int _rndMin;
        private int _rndMax;

        public FrmXFileProgressBarTest()
        {
            InitializeComponent();

            // 2GB
            xFileProgressBar1.TotalSizeInBytes = 2L * 1024 * 1024 * 1024;
            // 200KB
            numRandomLow.Value = 200 * 1024;
            // 800KB
            numRandomHigh.Value = 800 * 1024;
        }

        private void btnAuto_Click(object sender, EventArgs e)
        {
            _rndMin = (int)numRandomLow.Value;
            _rndMax = (int)numRandomHigh.Value;

            ThreadPool.QueueUserWorkItem(o =>
            {
                XFileProgressBar fpb = (XFileProgressBar)o;
                while (fpb.Percentage < 100 && !Disposing && !IsDisposed)
                {
                    int bytes = _rnd.Next(_rndMin, _rndMax);

                    fpb.BeginInvoke(new MethodInvoker(() =>
                    {
                        fpb.AddValue(bytes);
                    }));

                    Thread.Sleep(10);
                }
            }, xFileProgressBar1);
        }

        private void btnClear_Click(object sender, EventArgs e)
        {
            xFileProgressBar1.ClearData();
        }
    }
}
