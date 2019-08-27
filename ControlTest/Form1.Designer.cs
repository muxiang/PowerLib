namespace ControlTest
{
    partial class Form1
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
            System.Windows.Forms.TreeNode treeNode1 = new System.Windows.Forms.TreeNode("L2,0");
            System.Windows.Forms.TreeNode treeNode2 = new System.Windows.Forms.TreeNode("L1,0", new System.Windows.Forms.TreeNode[] {
            treeNode1});
            System.Windows.Forms.TreeNode treeNode3 = new System.Windows.Forms.TreeNode("L1,1");
            System.Windows.Forms.TreeNode treeNode4 = new System.Windows.Forms.TreeNode("L1,2");
            System.Windows.Forms.TreeNode treeNode5 = new System.Windows.Forms.TreeNode("L0,0", new System.Windows.Forms.TreeNode[] {
            treeNode2,
            treeNode3,
            treeNode4});
            System.Windows.Forms.TreeNode treeNode6 = new System.Windows.Forms.TreeNode("L1,0");
            System.Windows.Forms.TreeNode treeNode7 = new System.Windows.Forms.TreeNode("L0,1", new System.Windows.Forms.TreeNode[] {
            treeNode6});
            this.ddi1 = new PowerControl.DropDownButtonItem();
            this.xButton2 = new PowerControl.XButton();
            this.ddi2 = new PowerControl.DropDownButtonItem();
            this.panel1 = new System.Windows.Forms.Panel();
            this.xCheckBox1 = new PowerControl.XCheckBox();
            this.xRadioButton2 = new PowerControl.XRadioButton();
            this.xRadioButton1 = new PowerControl.XRadioButton();
            this.xComboBox1 = new PowerControl.XComboBox();
            this.textBox1 = new System.Windows.Forms.TextBox();
            this.xTextBox1 = new PowerControl.XTextBox();
            this.ipTextBox1 = new PowerControl.IPTextBox();
            this.xButton7 = new PowerControl.XButton();
            this.loadingCircle1 = new PowerControl.LoadingCircle();
            this.xButton3 = new PowerControl.XButton();
            this.checkBox1 = new System.Windows.Forms.CheckBox();
            this.radioButton2 = new System.Windows.Forms.RadioButton();
            this.radioButton1 = new System.Windows.Forms.RadioButton();
            this.xButton6 = new PowerControl.XButton();
            this.xButton5 = new PowerControl.XButton();
            this.xButton8 = new PowerControl.XButton();
            this.xButton4 = new PowerControl.XButton();
            this.xButton1 = new PowerControl.XButton();
            this.menuStrip1 = new System.Windows.Forms.MenuStrip();
            this.文件OToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.button1 = new System.Windows.Forms.Button();
            this.treeView1 = new PowerControl.XTreeView();
            this.panel1.SuspendLayout();
            this.menuStrip1.SuspendLayout();
            this.SuspendLayout();
            // 
            // ddi1
            // 
            this.ddi1.Caption = "下拉项1";
            this.ddi1.Location = new System.Drawing.Point(0, 0);
            this.ddi1.Margin = new System.Windows.Forms.Padding(3, 4, 3, 4);
            this.ddi1.Name = "ddi1";
            this.ddi1.ParentBtn = this.xButton2;
            this.ddi1.Size = new System.Drawing.Size(159, 80);
            this.ddi1.TabIndex = 0;
            this.ddi1.Click += new System.EventHandler(this.ddi1_Click);
            // 
            // xButton2
            // 
            this.xButton2.BorderColor = System.Drawing.Color.White;
            this.xButton2.CheckedEndColor = System.Drawing.Color.FromArgb(((int)(((byte)(157)))), ((int)(((byte)(153)))), ((int)(((byte)(155)))));
            this.xButton2.CheckedForeColor = System.Drawing.Color.Empty;
            this.xButton2.CheckedStartColor = System.Drawing.Color.FromArgb(((int)(((byte)(186)))), ((int)(((byte)(185)))), ((int)(((byte)(181)))));
            this.xButton2.DropDownItems.Add(this.ddi1);
            this.xButton2.DropDownItems.Add(this.ddi2);
            this.xButton2.EndColor = System.Drawing.Color.FromArgb(((int)(((byte)(90)))), ((int)(((byte)(107)))), ((int)(((byte)(125)))));
            this.xButton2.Font = new System.Drawing.Font("微软雅黑", 10F, System.Drawing.FontStyle.Bold);
            this.xButton2.ForeColor = System.Drawing.Color.White;
            this.xButton2.Image = null;
            this.xButton2.Location = new System.Drawing.Point(293, 4);
            this.xButton2.Margin = new System.Windows.Forms.Padding(3, 4, 3, 4);
            this.xButton2.Name = "xButton2";
            this.xButton2.Shortcut = 'C';
            this.xButton2.Size = new System.Drawing.Size(159, 80);
            this.xButton2.StartColor = System.Drawing.Color.FromArgb(((int)(((byte)(126)))), ((int)(((byte)(135)))), ((int)(((byte)(142)))));
            this.xButton2.TabIndex = 7;
            this.xButton2.Text = "xButton2";
            this.xButton2.Click += new System.EventHandler(this.xButton2_Click_1);
            // 
            // ddi2
            // 
            this.ddi2.Caption = "下拉项2";
            this.ddi2.Location = new System.Drawing.Point(0, 80);
            this.ddi2.Margin = new System.Windows.Forms.Padding(3, 4, 3, 4);
            this.ddi2.Name = "ddi2";
            this.ddi2.ParentBtn = this.xButton2;
            this.ddi2.Size = new System.Drawing.Size(159, 80);
            this.ddi2.TabIndex = 0;
            this.ddi2.Click += new System.EventHandler(this.ddi2_Click);
            // 
            // panel1
            // 
            this.panel1.Controls.Add(this.xCheckBox1);
            this.panel1.Controls.Add(this.xRadioButton2);
            this.panel1.Controls.Add(this.xRadioButton1);
            this.panel1.Controls.Add(this.xComboBox1);
            this.panel1.Controls.Add(this.textBox1);
            this.panel1.Controls.Add(this.xTextBox1);
            this.panel1.Controls.Add(this.ipTextBox1);
            this.panel1.Controls.Add(this.xButton7);
            this.panel1.Controls.Add(this.loadingCircle1);
            this.panel1.Controls.Add(this.xButton3);
            this.panel1.Controls.Add(this.checkBox1);
            this.panel1.Controls.Add(this.radioButton2);
            this.panel1.Controls.Add(this.radioButton1);
            this.panel1.Controls.Add(this.xButton2);
            this.panel1.Controls.Add(this.xButton6);
            this.panel1.Controls.Add(this.xButton5);
            this.panel1.Controls.Add(this.xButton8);
            this.panel1.Controls.Add(this.xButton4);
            this.panel1.Controls.Add(this.xButton1);
            this.panel1.Location = new System.Drawing.Point(12, 40);
            this.panel1.Margin = new System.Windows.Forms.Padding(3, 4, 3, 4);
            this.panel1.Name = "panel1";
            this.panel1.Size = new System.Drawing.Size(871, 807);
            this.panel1.TabIndex = 7;
            // 
            // xCheckBox1
            // 
            this.xCheckBox1.AutoSize = true;
            this.xCheckBox1.BoxBorderColor = System.Drawing.SystemColors.ControlText;
            this.xCheckBox1.Checked = true;
            this.xCheckBox1.CheckState = System.Windows.Forms.CheckState.Checked;
            this.xCheckBox1.InnerColor = System.Drawing.Color.FromArgb(((int)(((byte)(89)))), ((int)(((byte)(98)))), ((int)(((byte)(255)))));
            this.xCheckBox1.Location = new System.Drawing.Point(106, 568);
            this.xCheckBox1.MouseHoveringForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(83)))), ((int)(((byte)(128)))), ((int)(((byte)(252)))));
            this.xCheckBox1.Name = "xCheckBox1";
            this.xCheckBox1.Size = new System.Drawing.Size(94, 20);
            this.xCheckBox1.TabIndex = 21;
            this.xCheckBox1.Text = "xCheckBox1";
            this.xCheckBox1.UseVisualStyleBackColor = true;
            // 
            // xRadioButton2
            // 
            this.xRadioButton2.AutoSize = true;
            this.xRadioButton2.InnerCircleColor = System.Drawing.Color.FromArgb(((int)(((byte)(89)))), ((int)(((byte)(98)))), ((int)(((byte)(255)))));
            this.xRadioButton2.Location = new System.Drawing.Point(624, 605);
            this.xRadioButton2.MouseHoveringForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(83)))), ((int)(((byte)(128)))), ((int)(((byte)(252)))));
            this.xRadioButton2.Name = "xRadioButton2";
            this.xRadioButton2.OuterCircleColor = System.Drawing.SystemColors.ControlText;
            this.xRadioButton2.Size = new System.Drawing.Size(109, 20);
            this.xRadioButton2.TabIndex = 20;
            this.xRadioButton2.TabStop = true;
            this.xRadioButton2.Text = "xRadioButton2";
            this.xRadioButton2.UseVisualStyleBackColor = true;
            // 
            // xRadioButton1
            // 
            this.xRadioButton1.AutoSize = true;
            this.xRadioButton1.Checked = true;
            this.xRadioButton1.InnerCircleColor = System.Drawing.Color.FromArgb(((int)(((byte)(89)))), ((int)(((byte)(98)))), ((int)(((byte)(255)))));
            this.xRadioButton1.Location = new System.Drawing.Point(37, 551);
            this.xRadioButton1.MouseHoveringForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(83)))), ((int)(((byte)(128)))), ((int)(((byte)(252)))));
            this.xRadioButton1.Name = "xRadioButton1";
            this.xRadioButton1.OuterCircleColor = System.Drawing.SystemColors.ControlText;
            this.xRadioButton1.Size = new System.Drawing.Size(48, 20);
            this.xRadioButton1.TabIndex = 19;
            this.xRadioButton1.TabStop = true;
            this.xRadioButton1.Text = "直角";
            this.xRadioButton1.UseVisualStyleBackColor = true;
            // 
            // xComboBox1
            // 
            this.xComboBox1.BorderColor = System.Drawing.Color.FromArgb(((int)(((byte)(184)))), ((int)(((byte)(184)))), ((int)(((byte)(184)))));
            this.xComboBox1.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.xComboBox1.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(184)))), ((int)(((byte)(184)))), ((int)(((byte)(184)))));
            this.xComboBox1.FormattingEnabled = true;
            this.xComboBox1.Items.AddRange(new object[] {
            "111",
            "222",
            "333"});
            this.xComboBox1.Location = new System.Drawing.Point(714, 551);
            this.xComboBox1.MouseHoveringForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(83)))), ((int)(((byte)(128)))), ((int)(((byte)(252)))));
            this.xComboBox1.Name = "xComboBox1";
            this.xComboBox1.Size = new System.Drawing.Size(121, 24);
            this.xComboBox1.TabIndex = 18;
            // 
            // textBox1
            // 
            this.textBox1.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.textBox1.ForeColor = System.Drawing.SystemColors.MenuHighlight;
            this.textBox1.Location = new System.Drawing.Point(733, 384);
            this.textBox1.Name = "textBox1";
            this.textBox1.Size = new System.Drawing.Size(100, 22);
            this.textBox1.TabIndex = 17;
            // 
            // xTextBox1
            // 
            this.xTextBox1.BackColor = System.Drawing.Color.White;
            this.xTextBox1.BorderColor = System.Drawing.Color.FromArgb(((int)(((byte)(184)))), ((int)(((byte)(184)))), ((int)(((byte)(184)))));
            this.xTextBox1.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.xTextBox1.Font = new System.Drawing.Font("微软雅黑", 8F);
            this.xTextBox1.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(184)))), ((int)(((byte)(184)))), ((int)(((byte)(184)))));
            this.xTextBox1.Location = new System.Drawing.Point(714, 480);
            this.xTextBox1.Name = "xTextBox1";
            this.xTextBox1.Size = new System.Drawing.Size(100, 22);
            this.xTextBox1.TabIndex = 16;
            // 
            // ipTextBox1
            // 
            this.ipTextBox1.BackColor = System.Drawing.Color.White;
            this.ipTextBox1.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.ipTextBox1.Location = new System.Drawing.Point(497, 421);
            this.ipTextBox1.Margin = new System.Windows.Forms.Padding(3, 4, 3, 4);
            this.ipTextBox1.Name = "ipTextBox1";
            this.ipTextBox1.Size = new System.Drawing.Size(127, 18);
            this.ipTextBox1.TabIndex = 15;
            // 
            // xButton7
            // 
            this.xButton7.BorderColor = System.Drawing.Color.White;
            this.xButton7.CheckedEndColor = System.Drawing.Color.FromArgb(((int)(((byte)(157)))), ((int)(((byte)(153)))), ((int)(((byte)(155)))));
            this.xButton7.CheckedForeColor = System.Drawing.Color.Empty;
            this.xButton7.CheckedStartColor = System.Drawing.Color.FromArgb(((int)(((byte)(186)))), ((int)(((byte)(185)))), ((int)(((byte)(181)))));
            this.xButton7.EndColor = System.Drawing.Color.FromArgb(((int)(((byte)(90)))), ((int)(((byte)(107)))), ((int)(((byte)(125)))));
            this.xButton7.Font = new System.Drawing.Font("微软雅黑", 8F, System.Drawing.FontStyle.Bold);
            this.xButton7.ForeColor = System.Drawing.Color.White;
            this.xButton7.Image = null;
            this.xButton7.Location = new System.Drawing.Point(251, 462);
            this.xButton7.Name = "xButton7";
            this.xButton7.Size = new System.Drawing.Size(75, 40);
            this.xButton7.StartColor = System.Drawing.Color.FromArgb(((int)(((byte)(126)))), ((int)(((byte)(135)))), ((int)(((byte)(142)))));
            this.xButton7.TabIndex = 14;
            this.xButton7.Text = "查询";
            this.xButton7.Click += new System.EventHandler(this.xButton7_Click);
            // 
            // loadingCircle1
            // 
            this.loadingCircle1.Color = System.Drawing.Color.White;
            this.loadingCircle1.Location = new System.Drawing.Point(150, 213);
            this.loadingCircle1.Name = "loadingCircle1";
            this.loadingCircle1.Size = new System.Drawing.Size(302, 302);
            this.loadingCircle1.TabIndex = 12;
            this.loadingCircle1.Text = "loadingCircle1";
            // 
            // xButton3
            // 
            this.xButton3.BorderColor = System.Drawing.Color.White;
            this.xButton3.CheckedEndColor = System.Drawing.Color.FromArgb(((int)(((byte)(157)))), ((int)(((byte)(153)))), ((int)(((byte)(155)))));
            this.xButton3.CheckedForeColor = System.Drawing.Color.Empty;
            this.xButton3.CheckedStartColor = System.Drawing.Color.FromArgb(((int)(((byte)(186)))), ((int)(((byte)(185)))), ((int)(((byte)(181)))));
            this.xButton3.DisplayStyle = PowerControl.XButtonDisplayStyle.ImageAndText;
            this.xButton3.EndColor = System.Drawing.Color.FromArgb(((int)(((byte)(90)))), ((int)(((byte)(107)))), ((int)(((byte)(125)))));
            this.xButton3.Font = new System.Drawing.Font("微软雅黑", 10F, System.Drawing.FontStyle.Bold);
            this.xButton3.ForeColor = System.Drawing.Color.White;
            this.xButton3.Image = global::ControlTest.Properties.Resources._123;
            this.xButton3.Location = new System.Drawing.Point(463, 168);
            this.xButton3.Margin = new System.Windows.Forms.Padding(3, 4, 3, 4);
            this.xButton3.Name = "xButton3";
            this.xButton3.Shortcut = 'P';
            this.xButton3.Size = new System.Drawing.Size(329, 131);
            this.xButton3.StartColor = System.Drawing.Color.FromArgb(((int)(((byte)(126)))), ((int)(((byte)(135)))), ((int)(((byte)(142)))));
            this.xButton3.TabIndex = 11;
            this.xButton3.Text = "xButton3";
            this.xButton3.Click += new System.EventHandler(this.xButton3_Click_1);
            // 
            // checkBox1
            // 
            this.checkBox1.AutoSize = true;
            this.checkBox1.Checked = true;
            this.checkBox1.CheckState = System.Windows.Forms.CheckState.Checked;
            this.checkBox1.Location = new System.Drawing.Point(37, 592);
            this.checkBox1.Margin = new System.Windows.Forms.Padding(3, 4, 3, 4);
            this.checkBox1.Name = "checkBox1";
            this.checkBox1.Size = new System.Drawing.Size(49, 20);
            this.checkBox1.TabIndex = 10;
            this.checkBox1.Text = "渐变";
            this.checkBox1.UseVisualStyleBackColor = true;
            this.checkBox1.CheckedChanged += new System.EventHandler(this.checkBox1_CheckedChanged);
            // 
            // radioButton2
            // 
            this.radioButton2.AutoSize = true;
            this.radioButton2.Location = new System.Drawing.Point(37, 524);
            this.radioButton2.Margin = new System.Windows.Forms.Padding(3, 4, 3, 4);
            this.radioButton2.Name = "radioButton2";
            this.radioButton2.Size = new System.Drawing.Size(48, 20);
            this.radioButton2.TabIndex = 8;
            this.radioButton2.Text = "直角";
            this.radioButton2.UseVisualStyleBackColor = true;
            this.radioButton2.CheckedChanged += new System.EventHandler(this.radioButton2_CheckedChanged);
            // 
            // radioButton1
            // 
            this.radioButton1.AutoSize = true;
            this.radioButton1.Location = new System.Drawing.Point(37, 495);
            this.radioButton1.Margin = new System.Windows.Forms.Padding(3, 4, 3, 4);
            this.radioButton1.Name = "radioButton1";
            this.radioButton1.Size = new System.Drawing.Size(48, 20);
            this.radioButton1.TabIndex = 9;
            this.radioButton1.Text = "圆角";
            this.radioButton1.UseVisualStyleBackColor = true;
            this.radioButton1.CheckedChanged += new System.EventHandler(this.radioButton1_CheckedChanged);
            // 
            // xButton6
            // 
            this.xButton6.BorderColor = System.Drawing.Color.FromArgb(((int)(((byte)(212)))), ((int)(((byte)(212)))), ((int)(((byte)(212)))));
            this.xButton6.CheckedEndColor = System.Drawing.Color.FromArgb(((int)(((byte)(83)))), ((int)(((byte)(128)))), ((int)(((byte)(252)))));
            this.xButton6.CheckedForeColor = System.Drawing.Color.Empty;
            this.xButton6.CheckedStartColor = System.Drawing.Color.FromArgb(((int)(((byte)(109)))), ((int)(((byte)(169)))), ((int)(((byte)(255)))));
            this.xButton6.EndColor = System.Drawing.Color.FromArgb(((int)(((byte)(232)))), ((int)(((byte)(232)))), ((int)(((byte)(232)))));
            this.xButton6.Font = new System.Drawing.Font("微软雅黑", 10F);
            this.xButton6.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(80)))), ((int)(((byte)(80)))), ((int)(((byte)(80)))));
            this.xButton6.Image = null;
            this.xButton6.Location = new System.Drawing.Point(527, 51);
            this.xButton6.Margin = new System.Windows.Forms.Padding(3, 4, 3, 4);
            this.xButton6.Name = "xButton6";
            this.xButton6.Shortcut = 'O';
            this.xButton6.Size = new System.Drawing.Size(87, 33);
            this.xButton6.StartColor = System.Drawing.Color.White;
            this.xButton6.TabIndex = 6;
            this.xButton6.Text = "确定";
            this.xButton6.Click += new System.EventHandler(this.xButton1_Click_1);
            // 
            // xButton5
            // 
            this.xButton5.BorderColor = System.Drawing.Color.White;
            this.xButton5.CheckedEndColor = System.Drawing.Color.FromArgb(((int)(((byte)(157)))), ((int)(((byte)(153)))), ((int)(((byte)(155)))));
            this.xButton5.CheckedForeColor = System.Drawing.Color.Empty;
            this.xButton5.CheckedStartColor = System.Drawing.Color.FromArgb(((int)(((byte)(186)))), ((int)(((byte)(185)))), ((int)(((byte)(181)))));
            this.xButton5.EndColor = System.Drawing.Color.FromArgb(((int)(((byte)(90)))), ((int)(((byte)(107)))), ((int)(((byte)(125)))));
            this.xButton5.Font = new System.Drawing.Font("微软雅黑", 10F, System.Drawing.FontStyle.Bold);
            this.xButton5.ForeColor = System.Drawing.Color.White;
            this.xButton5.Image = null;
            this.xButton5.Location = new System.Drawing.Point(163, 412);
            this.xButton5.Margin = new System.Windows.Forms.Padding(3, 4, 3, 4);
            this.xButton5.Name = "xButton5";
            this.xButton5.Shortcut = 'O';
            this.xButton5.Size = new System.Drawing.Size(159, 80);
            this.xButton5.StartColor = System.Drawing.Color.FromArgb(((int)(((byte)(126)))), ((int)(((byte)(135)))), ((int)(((byte)(142)))));
            this.xButton5.TabIndex = 6;
            this.xButton5.Text = "确定";
            this.xButton5.Click += new System.EventHandler(this.xButton1_Click_1);
            // 
            // xButton8
            // 
            this.xButton8.BorderColor = System.Drawing.Color.White;
            this.xButton8.CheckedEndColor = System.Drawing.Color.FromArgb(((int)(((byte)(255)))), ((int)(((byte)(128)))), ((int)(((byte)(128)))));
            this.xButton8.CheckedForeColor = System.Drawing.Color.Empty;
            this.xButton8.CheckedStartColor = System.Drawing.Color.FromArgb(((int)(((byte)(186)))), ((int)(((byte)(185)))), ((int)(((byte)(181)))));
            this.xButton8.EndColor = System.Drawing.Color.Lime;
            this.xButton8.Font = new System.Drawing.Font("微软雅黑", 10F, System.Drawing.FontStyle.Bold);
            this.xButton8.ForeColor = System.Drawing.Color.White;
            this.xButton8.Image = null;
            this.xButton8.Location = new System.Drawing.Point(458, 475);
            this.xButton8.Margin = new System.Windows.Forms.Padding(3, 4, 3, 4);
            this.xButton8.Name = "xButton8";
            this.xButton8.Shortcut = 'A';
            this.xButton8.Size = new System.Drawing.Size(122, 102);
            this.xButton8.StartColor = System.Drawing.Color.Red;
            this.xButton8.TabIndex = 6;
            this.xButton8.Text = "关于";
            this.xButton8.Click += new System.EventHandler(this.xButton8_Click);
            // 
            // xButton4
            // 
            this.xButton4.BorderColor = System.Drawing.Color.White;
            this.xButton4.CheckedEndColor = System.Drawing.Color.FromArgb(((int)(((byte)(157)))), ((int)(((byte)(153)))), ((int)(((byte)(155)))));
            this.xButton4.CheckedForeColor = System.Drawing.Color.Empty;
            this.xButton4.CheckedStartColor = System.Drawing.Color.FromArgb(((int)(((byte)(186)))), ((int)(((byte)(185)))), ((int)(((byte)(181)))));
            this.xButton4.EndColor = System.Drawing.Color.FromArgb(((int)(((byte)(90)))), ((int)(((byte)(107)))), ((int)(((byte)(125)))));
            this.xButton4.Font = new System.Drawing.Font("微软雅黑", 10F, System.Drawing.FontStyle.Bold);
            this.xButton4.ForeColor = System.Drawing.Color.White;
            this.xButton4.Image = null;
            this.xButton4.Location = new System.Drawing.Point(12, 259);
            this.xButton4.Margin = new System.Windows.Forms.Padding(3, 4, 3, 4);
            this.xButton4.Name = "xButton4";
            this.xButton4.Shortcut = 'O';
            this.xButton4.Size = new System.Drawing.Size(122, 102);
            this.xButton4.StartColor = System.Drawing.Color.FromArgb(((int)(((byte)(126)))), ((int)(((byte)(135)))), ((int)(((byte)(142)))));
            this.xButton4.TabIndex = 6;
            this.xButton4.Text = "确定";
            this.xButton4.Click += new System.EventHandler(this.xButton1_Click_1);
            // 
            // xButton1
            // 
            this.xButton1.BorderColor = System.Drawing.Color.White;
            this.xButton1.CheckedEndColor = System.Drawing.Color.FromArgb(((int)(((byte)(157)))), ((int)(((byte)(153)))), ((int)(((byte)(155)))));
            this.xButton1.CheckedForeColor = System.Drawing.Color.Empty;
            this.xButton1.CheckedStartColor = System.Drawing.Color.FromArgb(((int)(((byte)(186)))), ((int)(((byte)(185)))), ((int)(((byte)(181)))));
            this.xButton1.EndColor = System.Drawing.Color.FromArgb(((int)(((byte)(90)))), ((int)(((byte)(107)))), ((int)(((byte)(125)))));
            this.xButton1.Font = new System.Drawing.Font("微软雅黑", 10F, System.Drawing.FontStyle.Bold);
            this.xButton1.ForeColor = System.Drawing.Color.White;
            this.xButton1.Image = null;
            this.xButton1.Location = new System.Drawing.Point(91, 4);
            this.xButton1.Margin = new System.Windows.Forms.Padding(3, 4, 3, 4);
            this.xButton1.Name = "xButton1";
            this.xButton1.Size = new System.Drawing.Size(159, 80);
            this.xButton1.StartColor = System.Drawing.Color.FromArgb(((int)(((byte)(126)))), ((int)(((byte)(135)))), ((int)(((byte)(142)))));
            this.xButton1.TabIndex = 6;
            this.xButton1.Text = "确定";
            this.xButton1.Click += new System.EventHandler(this.xButton1_Click_1);
            // 
            // menuStrip1
            // 
            this.menuStrip1.BackColor = System.Drawing.Color.Transparent;
            this.menuStrip1.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.文件OToolStripMenuItem});
            this.menuStrip1.Location = new System.Drawing.Point(0, 0);
            this.menuStrip1.Name = "menuStrip1";
            this.menuStrip1.Size = new System.Drawing.Size(1316, 27);
            this.menuStrip1.TabIndex = 9;
            this.menuStrip1.Text = "menuStrip1";
            // 
            // 文件OToolStripMenuItem
            // 
            this.文件OToolStripMenuItem.Font = new System.Drawing.Font("微软雅黑", 10F, System.Drawing.FontStyle.Bold);
            this.文件OToolStripMenuItem.ForeColor = System.Drawing.Color.Black;
            this.文件OToolStripMenuItem.Name = "文件OToolStripMenuItem";
            this.文件OToolStripMenuItem.Size = new System.Drawing.Size(70, 23);
            this.文件OToolStripMenuItem.Text = "文件(&O)";
            // 
            // button1
            // 
            this.button1.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(45)))), ((int)(((byte)(52)))), ((int)(((byte)(61)))));
            this.button1.Location = new System.Drawing.Point(946, 101);
            this.button1.Name = "button1";
            this.button1.Size = new System.Drawing.Size(85, 23);
            this.button1.TabIndex = 10;
            this.button1.Text = "button1(&O)";
            this.button1.UseVisualStyleBackColor = false;
            // 
            // treeView1
            // 
            this.treeView1.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(45)))), ((int)(((byte)(52)))), ((int)(((byte)(60)))));
            this.treeView1.DrawMode = System.Windows.Forms.TreeViewDrawMode.OwnerDrawAll;
            this.treeView1.ForeColor = System.Drawing.Color.White;
            this.treeView1.Location = new System.Drawing.Point(926, 149);
            this.treeView1.Name = "treeView1";
            treeNode1.Name = "L2,0";
            treeNode1.Text = "L2,0";
            treeNode2.Name = "L1,0";
            treeNode2.Text = "L1,0";
            treeNode3.Name = "L1,1";
            treeNode3.Text = "L1,1";
            treeNode4.Name = "L1,2";
            treeNode4.Text = "L1,2";
            treeNode5.Name = "L0,0";
            treeNode5.Text = "L0,0";
            treeNode6.Name = "L1,0";
            treeNode6.Text = "L1,0";
            treeNode7.Name = "L0,1";
            treeNode7.Text = "L0,1";
            this.treeView1.Nodes.AddRange(new System.Windows.Forms.TreeNode[] {
            treeNode5,
            treeNode7});
            this.treeView1.SelectedBackColor = System.Drawing.Color.FromArgb(((int)(((byte)(0)))), ((int)(((byte)(150)))), ((int)(((byte)(170)))));
            this.treeView1.Size = new System.Drawing.Size(239, 383);
            this.treeView1.TabIndex = 12;
            // 
            // Form1
            // 
            this.AcceptButton = this.xButton1;
            this.AutoScaleDimensions = new System.Drawing.SizeF(7F, 16F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(252)))), ((int)(((byte)(252)))), ((int)(((byte)(252)))));
            this.ClientSize = new System.Drawing.Size(1316, 787);
            this.Controls.Add(this.treeView1);
            this.Controls.Add(this.button1);
            this.Controls.Add(this.panel1);
            this.Controls.Add(this.menuStrip1);
            this.Font = new System.Drawing.Font("微软雅黑", 8F);
            this.Location = new System.Drawing.Point(0, 0);
            this.MainMenuStrip = this.menuStrip1;
            this.Margin = new System.Windows.Forms.Padding(3, 4, 3, 4);
            this.Name = "Form1";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
            this.Text = "Form1";
            this.WindowState = System.Windows.Forms.FormWindowState.Maximized;
            this.KeyDown += new System.Windows.Forms.KeyEventHandler(this.Form1_KeyDown);
            this.PreviewKeyDown += new System.Windows.Forms.PreviewKeyDownEventHandler(this.Form1_PreviewKeyDown);
            this.panel1.ResumeLayout(false);
            this.panel1.PerformLayout();
            this.menuStrip1.ResumeLayout(false);
            this.menuStrip1.PerformLayout();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion
        private PowerControl.DropDownButtonItem ddi1;
        private PowerControl.DropDownButtonItem ddi2;
        private PowerControl.XButton xButton2;
        private System.Windows.Forms.Panel panel1;
        private PowerControl.XButton xButton3;
        private System.Windows.Forms.CheckBox checkBox1;
        private System.Windows.Forms.RadioButton radioButton2;
        private System.Windows.Forms.RadioButton radioButton1;
        private PowerControl.XButton xButton1;
        private PowerControl.LoadingCircle loadingCircle1;
        private PowerControl.XButton xButton6;
        private PowerControl.XButton xButton5;
        private PowerControl.XButton xButton4;
        private System.Windows.Forms.MenuStrip menuStrip1;
        private System.Windows.Forms.ToolStripMenuItem 文件OToolStripMenuItem;
        private System.Windows.Forms.Button button1;
        private PowerControl.XButton xButton7;
        private PowerControl.IPTextBox ipTextBox1;
        private PowerControl.XTreeView treeView1;
        private PowerControl.XButton xButton8;
        private PowerControl.XTextBox xTextBox1;
        private System.Windows.Forms.TextBox textBox1;
        private PowerControl.XComboBox xComboBox1;
        private PowerControl.XRadioButton xRadioButton1;
        private PowerControl.XRadioButton xRadioButton2;
        private PowerControl.XCheckBox xCheckBox1;
    }
}