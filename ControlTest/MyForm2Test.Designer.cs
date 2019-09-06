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
            this.projectFlowChart1.Location = new System.Drawing.Point(22, 197);
            this.projectFlowChart1.Name = "projectFlowChart1";
            this.projectFlowChart1.Size = new System.Drawing.Size(1350, 115);
            this.projectFlowChart1.TabIndex = 0;
            // 
            // MyForm2Test
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 12F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(1428, 730);
            this.Controls.Add(this.projectFlowChart1);
            this.Name = "MyForm2Test";
            this.Text = "MyForm2Test";
            this.ResumeLayout(false);

        }

        #endregion

        private PowerControl.IrregularControls.ProjectFlowChart projectFlowChart1;
    }
}