using System;
using System.Collections.Generic;
using System.Linq;
using System.Windows.Forms;

using PowerControl;

namespace SignalerTool.Forms
{
    public partial class FrmDetectorsSelect : XForm3
    {
        public FrmDetectorsSelect()
        {
            InitializeComponent();
        }

        public List<int> SelectedDetectorIds { get; } = new List<int>();

        private void FrmDetectorsSelect_Load(object sender, EventArgs e)
        {
        }

        private void btnOK_Click(object sender, EventArgs e)
        {
            SelectedDetectorIds.Clear();

            SelectedDetectorIds.AddRange(chklstDetectors.CheckedItems.OfType<object>().Select(Convert.ToInt32));

            DialogResult = DialogResult.OK;
        }

        private void btnCancel_Click(object sender, EventArgs e)
        {
            DialogResult = DialogResult.Cancel;
        }
    }
}
