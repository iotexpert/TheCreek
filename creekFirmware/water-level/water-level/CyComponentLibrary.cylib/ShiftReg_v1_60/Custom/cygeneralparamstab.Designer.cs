namespace ShiftReg_v1_60
{
    partial class CyGeneralParamsTab
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

        #region Component Designer generated code

        /// <summary>
        /// Required method for Designer support - do not modify 
        /// the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
            this.gbCustParams = new System.Windows.Forms.GroupBox();
            this.gbInter = new System.Windows.Forms.GroupBox();
            this.cb_int_reset = new System.Windows.Forms.CheckBox();
            this.cb_int_st = new System.Windows.Forms.CheckBox();
            this.cb_int_onLoad = new System.Windows.Forms.CheckBox();
            this.gbShIn = new System.Windows.Forms.GroupBox();
            this.rbsi1 = new System.Windows.Forms.RadioButton();
            this.rbsi0 = new System.Windows.Forms.RadioButton();
            this.cbUseInterrupt = new System.Windows.Forms.CheckBox();
            this.cbShiftOut = new System.Windows.Forms.CheckBox();
            this.cbShiftIn = new System.Windows.Forms.CheckBox();
            this.cbLoad = new System.Windows.Forms.CheckBox();
            this.cbStore = new System.Windows.Forms.CheckBox();
            this.gbGeneral = new System.Windows.Forms.GroupBox();
            this.cbFIFOS = new System.Windows.Forms.ComboBox();
            this.cbDir = new System.Windows.Forms.ComboBox();
            this.cbLength = new System.Windows.Forms.ComboBox();
            this.lFIFOSize = new System.Windows.Forms.Label();
            this.label2 = new System.Windows.Forms.Label();
            this.label1 = new System.Windows.Forms.Label();
            this.gbCustParams.SuspendLayout();
            this.gbInter.SuspendLayout();
            this.gbShIn.SuspendLayout();
            this.gbGeneral.SuspendLayout();
            this.SuspendLayout();
            // 
            // gbCustParams
            // 
            this.gbCustParams.Controls.Add(this.gbInter);
            this.gbCustParams.Controls.Add(this.gbShIn);
            this.gbCustParams.Controls.Add(this.cbUseInterrupt);
            this.gbCustParams.Controls.Add(this.cbShiftOut);
            this.gbCustParams.Controls.Add(this.cbShiftIn);
            this.gbCustParams.Controls.Add(this.cbLoad);
            this.gbCustParams.Controls.Add(this.cbStore);
            this.gbCustParams.Dock = System.Windows.Forms.DockStyle.Fill;
            this.gbCustParams.Location = new System.Drawing.Point(0, 86);
            this.gbCustParams.Margin = new System.Windows.Forms.Padding(4);
            this.gbCustParams.Name = "gbCustParams";
            this.gbCustParams.Padding = new System.Windows.Forms.Padding(4);
            this.gbCustParams.Size = new System.Drawing.Size(614, 213);
            this.gbCustParams.TabIndex = 1;
            this.gbCustParams.TabStop = false;
            this.gbCustParams.Text = "Custom mode parameters";
            // 
            // gbInter
            // 
            this.gbInter.Controls.Add(this.cb_int_reset);
            this.gbInter.Controls.Add(this.cb_int_st);
            this.gbInter.Controls.Add(this.cb_int_onLoad);
            this.gbInter.Enabled = false;
            this.gbInter.Location = new System.Drawing.Point(434, 64);
            this.gbInter.Margin = new System.Windows.Forms.Padding(4);
            this.gbInter.Name = "gbInter";
            this.gbInter.Padding = new System.Windows.Forms.Padding(4);
            this.gbInter.Size = new System.Drawing.Size(161, 110);
            this.gbInter.TabIndex = 23;
            this.gbInter.TabStop = false;
            this.gbInter.Text = "Interrupt Sources";
            // 
            // cb_int_reset
            // 
            this.cb_int_reset.AutoSize = true;
            this.cb_int_reset.Location = new System.Drawing.Point(8, 80);
            this.cb_int_reset.Margin = new System.Windows.Forms.Padding(4);
            this.cb_int_reset.Name = "cb_int_reset";
            this.cb_int_reset.Size = new System.Drawing.Size(90, 21);
            this.cb_int_reset.TabIndex = 28;
            this.cb_int_reset.Text = "On Reset";
            this.cb_int_reset.UseVisualStyleBackColor = true;
            // 
            // cb_int_st
            // 
            this.cb_int_st.AutoSize = true;
            this.cb_int_st.Location = new System.Drawing.Point(8, 52);
            this.cb_int_st.Margin = new System.Windows.Forms.Padding(4);
            this.cb_int_st.Name = "cb_int_st";
            this.cb_int_st.Size = new System.Drawing.Size(87, 21);
            this.cb_int_st.TabIndex = 26;
            this.cb_int_st.Text = "On Store";
            this.cb_int_st.UseVisualStyleBackColor = true;
            // 
            // cb_int_onLoad
            // 
            this.cb_int_onLoad.AutoSize = true;
            this.cb_int_onLoad.Location = new System.Drawing.Point(8, 23);
            this.cb_int_onLoad.Margin = new System.Windows.Forms.Padding(4);
            this.cb_int_onLoad.Name = "cb_int_onLoad";
            this.cb_int_onLoad.Size = new System.Drawing.Size(85, 21);
            this.cb_int_onLoad.TabIndex = 24;
            this.cb_int_onLoad.Text = "On Load";
            this.cb_int_onLoad.UseVisualStyleBackColor = true;
            // 
            // gbShIn
            // 
            this.gbShIn.Controls.Add(this.rbsi1);
            this.gbShIn.Controls.Add(this.rbsi0);
            this.gbShIn.Location = new System.Drawing.Point(204, 64);
            this.gbShIn.Margin = new System.Windows.Forms.Padding(4);
            this.gbShIn.Name = "gbShIn";
            this.gbShIn.Padding = new System.Windows.Forms.Padding(4);
            this.gbShIn.Size = new System.Drawing.Size(153, 85);
            this.gbShIn.TabIndex = 18;
            this.gbShIn.TabStop = false;
            this.gbShIn.Text = "Default Shift Value";
            // 
            // rbsi1
            // 
            this.rbsi1.AutoSize = true;
            this.rbsi1.Location = new System.Drawing.Point(8, 52);
            this.rbsi1.Margin = new System.Windows.Forms.Padding(4);
            this.rbsi1.Name = "rbsi1";
            this.rbsi1.Size = new System.Drawing.Size(37, 21);
            this.rbsi1.TabIndex = 20;
            this.rbsi1.Text = "1";
            this.rbsi1.UseVisualStyleBackColor = true;
            // 
            // rbsi0
            // 
            this.rbsi0.AutoSize = true;
            this.rbsi0.Checked = true;
            this.rbsi0.Location = new System.Drawing.Point(8, 23);
            this.rbsi0.Margin = new System.Windows.Forms.Padding(4);
            this.rbsi0.Name = "rbsi0";
            this.rbsi0.Size = new System.Drawing.Size(37, 21);
            this.rbsi0.TabIndex = 19;
            this.rbsi0.TabStop = true;
            this.rbsi0.Text = "0";
            this.rbsi0.UseVisualStyleBackColor = true;
            // 
            // cbUseInterrupt
            // 
            this.cbUseInterrupt.AutoSize = true;
            this.cbUseInterrupt.Location = new System.Drawing.Point(431, 32);
            this.cbUseInterrupt.Margin = new System.Windows.Forms.Padding(4);
            this.cbUseInterrupt.Name = "cbUseInterrupt";
            this.cbUseInterrupt.Size = new System.Drawing.Size(112, 21);
            this.cbUseInterrupt.TabIndex = 22;
            this.cbUseInterrupt.Text = "Use Interrupt";
            this.cbUseInterrupt.UseVisualStyleBackColor = true;
            this.cbUseInterrupt.CheckedChanged += new System.EventHandler(this.cbInterrupt_CheckedChanged);
            // 
            // cbShiftOut
            // 
            this.cbShiftOut.AutoSize = true;
            this.cbShiftOut.Location = new System.Drawing.Point(32, 93);
            this.cbShiftOut.Margin = new System.Windows.Forms.Padding(4);
            this.cbShiftOut.Name = "cbShiftOut";
            this.cbShiftOut.Size = new System.Drawing.Size(114, 21);
            this.cbShiftOut.TabIndex = 15;
            this.cbShiftOut.Text = "Use Shift Out";
            this.cbShiftOut.UseVisualStyleBackColor = true;
            this.cbShiftOut.CheckedChanged += new System.EventHandler(this.cbShiftIn_CheckedChanged);
            // 
            // cbShiftIn
            // 
            this.cbShiftIn.AutoSize = true;
            this.cbShiftIn.Location = new System.Drawing.Point(204, 32);
            this.cbShiftIn.Margin = new System.Windows.Forms.Padding(4);
            this.cbShiftIn.Name = "cbShiftIn";
            this.cbShiftIn.Size = new System.Drawing.Size(102, 21);
            this.cbShiftIn.TabIndex = 17;
            this.cbShiftIn.Text = "Use Shift In";
            this.cbShiftIn.UseVisualStyleBackColor = true;
            this.cbShiftIn.CheckedChanged += new System.EventHandler(this.cbShiftIn_CheckedChanged);
            // 
            // cbLoad
            // 
            this.cbLoad.AutoSize = true;
            this.cbLoad.Location = new System.Drawing.Point(32, 32);
            this.cbLoad.Margin = new System.Windows.Forms.Padding(4);
            this.cbLoad.Name = "cbLoad";
            this.cbLoad.Size = new System.Drawing.Size(91, 21);
            this.cbLoad.TabIndex = 11;
            this.cbLoad.Text = "Use Load";
            this.cbLoad.UseVisualStyleBackColor = true;
            this.cbLoad.CheckedChanged += new System.EventHandler(this.cbLoad_CheckedChanged);
            // 
            // cbStore
            // 
            this.cbStore.AutoSize = true;
            this.cbStore.Location = new System.Drawing.Point(32, 61);
            this.cbStore.Margin = new System.Windows.Forms.Padding(4);
            this.cbStore.Name = "cbStore";
            this.cbStore.Size = new System.Drawing.Size(93, 21);
            this.cbStore.TabIndex = 13;
            this.cbStore.Text = "Use Store";
            this.cbStore.UseVisualStyleBackColor = true;
            this.cbStore.CheckedChanged += new System.EventHandler(this.cbLoad_CheckedChanged);
            // 
            // gbGeneral
            // 
            this.gbGeneral.Controls.Add(this.cbFIFOS);
            this.gbGeneral.Controls.Add(this.cbDir);
            this.gbGeneral.Controls.Add(this.cbLength);
            this.gbGeneral.Controls.Add(this.lFIFOSize);
            this.gbGeneral.Controls.Add(this.label2);
            this.gbGeneral.Controls.Add(this.label1);
            this.gbGeneral.Dock = System.Windows.Forms.DockStyle.Top;
            this.gbGeneral.Location = new System.Drawing.Point(0, 0);
            this.gbGeneral.Margin = new System.Windows.Forms.Padding(4);
            this.gbGeneral.MinimumSize = new System.Drawing.Size(0, 12);
            this.gbGeneral.Name = "gbGeneral";
            this.gbGeneral.Padding = new System.Windows.Forms.Padding(4);
            this.gbGeneral.Size = new System.Drawing.Size(614, 86);
            this.gbGeneral.TabIndex = 0;
            this.gbGeneral.TabStop = false;
            this.gbGeneral.Text = "General";
            // 
            // cbFIFOS
            // 
            this.cbFIFOS.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.cbFIFOS.Enabled = false;
            this.cbFIFOS.FormattingEnabled = true;
            this.cbFIFOS.Items.AddRange(new object[] {
            "1",
            "4"});
            this.cbFIFOS.Location = new System.Drawing.Point(338, 36);
            this.cbFIFOS.Margin = new System.Windows.Forms.Padding(4);
            this.cbFIFOS.Name = "cbFIFOS";
            this.cbFIFOS.Size = new System.Drawing.Size(65, 24);
            this.cbFIFOS.TabIndex = 3;
            // 
            // cbDir
            // 
            this.cbDir.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.cbDir.FormattingEnabled = true;
            this.cbDir.Items.AddRange(new object[] {
            "Left",
            "Right"});
            this.cbDir.Location = new System.Drawing.Point(524, 36);
            this.cbDir.Margin = new System.Windows.Forms.Padding(4);
            this.cbDir.Name = "cbDir";
            this.cbDir.Size = new System.Drawing.Size(71, 24);
            this.cbDir.TabIndex = 5;
            // 
            // cbLength
            // 
            this.cbLength.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.cbLength.FormattingEnabled = true;
            this.cbLength.Location = new System.Drawing.Point(110, 36);
            this.cbLength.Margin = new System.Windows.Forms.Padding(4);
            this.cbLength.Name = "cbLength";
            this.cbLength.Size = new System.Drawing.Size(65, 24);
            this.cbLength.TabIndex = 1;
            // 
            // lFIFOSize
            // 
            this.lFIFOSize.AutoSize = true;
            this.lFIFOSize.Enabled = false;
            this.lFIFOSize.Location = new System.Drawing.Point(192, 39);
            this.lFIFOSize.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            this.lFIFOSize.Name = "lFIFOSize";
            this.lFIFOSize.Size = new System.Drawing.Size(143, 17);
            this.lFIFOSize.TabIndex = 0;
            this.lFIFOSize.Text = "TX FIFO Size (bytes):";
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Location = new System.Drawing.Point(421, 39);
            this.label2.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(100, 17);
            this.label2.TabIndex = 0;
            this.label2.Text = "Shift Direction:";
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(14, 39);
            this.label1.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(92, 17);
            this.label1.TabIndex = 0;
            this.label1.Text = "Length (bits):";
            // 
            // CyGeneralParamsTab
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(8F, 16F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.AutoScroll = true;
            this.BackColor = System.Drawing.SystemColors.Control;
            this.Controls.Add(this.gbCustParams);
            this.Controls.Add(this.gbGeneral);
            this.Margin = new System.Windows.Forms.Padding(4);
            this.Name = "CyGeneralParamsTab";
            this.Size = new System.Drawing.Size(614, 299);
            this.gbCustParams.ResumeLayout(false);
            this.gbCustParams.PerformLayout();
            this.gbInter.ResumeLayout(false);
            this.gbInter.PerformLayout();
            this.gbShIn.ResumeLayout(false);
            this.gbShIn.PerformLayout();
            this.gbGeneral.ResumeLayout(false);
            this.gbGeneral.PerformLayout();
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.GroupBox gbCustParams;
        private System.Windows.Forms.CheckBox cbUseInterrupt;
        private System.Windows.Forms.CheckBox cbShiftOut;
        private System.Windows.Forms.CheckBox cbShiftIn;
        private System.Windows.Forms.CheckBox cbStore;
        private System.Windows.Forms.GroupBox gbGeneral;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.ComboBox cbFIFOS;
        private System.Windows.Forms.ComboBox cbDir;
        private System.Windows.Forms.ComboBox cbLength;
        private System.Windows.Forms.Label lFIFOSize;
        private System.Windows.Forms.CheckBox cbLoad;
        private System.Windows.Forms.GroupBox gbShIn;
        private System.Windows.Forms.GroupBox gbInter;
        private System.Windows.Forms.RadioButton rbsi1;
        private System.Windows.Forms.RadioButton rbsi0;
        private System.Windows.Forms.CheckBox cb_int_reset;
        private System.Windows.Forms.CheckBox cb_int_st;
        private System.Windows.Forms.CheckBox cb_int_onLoad;
    }
}
