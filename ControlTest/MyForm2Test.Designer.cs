namespace ControlTest
{
    partial class MyForm2Test
    {
        /// <summary>
        /// Required designer variable.
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary>
        /// Clean up any resources being used.
        /// </summary>
        /// <param name="disposing">true if managed resources should be disposed; otherwise, false.</param>
        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        #region Windows Form Designer generated code

        /// <summary>
        /// Required method for Designer support - do not modify
        /// the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
            this.projectFlowChart1 = new PowerControl.IrregularControls.ProjectFlowChart();
            this.SuspendLayout();
            // 
            // projectFlowChart1
            // 
            this.projectFlowChart1.BackColor = System.Drawing.Color.White;
            this.projectFlowChart1.ComputeApplications = ((PowerControl.IrregularControls.ComputeApplications)(((PowerControl.IrregularControls.ComputeApplications.Mike21Ladtap | PowerControl.IrregularControls.ComputeApplications.HyDrus_1D) 
            | PowerControl.IrregularControls.ComputeApplications.PavanDose)));
            this.projectFlowChart1.IsComputeBySingle = false;
            this.projectFlowChart1.Location = new System.Drawing.Point(36, 237);
            this.projectFlowChart1.Name = "projectFlowChart1";
            this.projectFlowChart1.Size = new System.Drawing.Size(1350, 345);
            this.projectFlowChart1.TabIndex = 0;
            this.projectFlowChart1.Text = "projectFlowChart1";
            // 
            // MyForm2Test
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 12F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(1428, 730);
            this.Controls.Add(this.projectFlowChart1);
            this.Name = "MyForm2Test";
            this.Text = "MyForm2Test";
            this.Load += new System.EventHandler(this.MyForm2Test_Load);
            this.ResumeLayout(false);

        }

        #endregion

        private PowerControl.IrregularControls.ProjectFlowChart projectFlowChart1;
    }
}