namespace ShiftReg_v1_10
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
            this.cbInterrupt = new System.Windows.Forms.CheckBox();
            this.cbShiftOut = new System.Windows.Forms.CheckBox();
            this.cbShiftIn = new System.Windows.Forms.CheckBox();
            this.cbLoad = new System.Windows.Forms.CheckBox();
            this.cbStore = new System.Windows.Forms.CheckBox();
            this.gbGeneral = new System.Windows.Forms.GroupBox();
            this.cbFIFOS = new System.Windows.Forms.ComboBox();
            this.cbDir = new System.Windows.Forms.ComboBox();
            this.cbLength = new System.Windows.Forms.ComboBox();
            this.label3 = new System.Windows.Forms.Label();
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
            this.gbCustParams.Controls.Add(this.cbInterrupt);
            this.gbCustParams.Controls.Add(this.cbShiftOut);
            this.gbCustParams.Controls.Add(this.cbShiftIn);
            this.gbCustParams.Controls.Add(this.cbLoad);
            this.gbCustParams.Controls.Add(this.cbStore);
            this.gbCustParams.Dock = System.Windows.Forms.DockStyle.Fill;
            this.gbCustParams.Location = new System.Drawing.Point(0, 70);
            this.gbCustParams.Name = "gbCustParams";
            this.gbCustParams.Size = new System.Drawing.Size(483, 173);
            this.gbCustParams.TabIndex = 0;
            this.gbCustParams.TabStop = false;
            this.gbCustParams.Text = "Custom mode parameters";
            // 
            // gbInter
            // 
            this.gbInter.Controls.Add(this.cb_int_reset);
            this.gbInter.Controls.Add(this.cb_int_st);
            this.gbInter.Controls.Add(this.cb_int_onLoad);
            this.gbInter.Enabled = false;
            this.gbInter.Location = new System.Drawing.Point(336, 57);
            this.gbInter.Name = "gbInter";
            this.gbInter.Size = new System.Drawing.Size(123, 89);
            this.gbInter.TabIndex = 1;
            this.gbInter.TabStop = false;
            this.gbInter.Text = "Interrupt Sources";
            // 
            // cb_int_reset
            // 
            this.cb_int_reset.AutoSize = true;
            this.cb_int_reset.Location = new System.Drawing.Point(6, 65);
            this.cb_int_reset.Name = "cb_int_reset";
            this.cb_int_reset.Size = new System.Drawing.Size(71, 17);
            this.cb_int_reset.TabIndex = 0;
            this.cb_int_reset.Text = "On Reset";
            this.cb_int_reset.UseVisualStyleBackColor = true;
            // 
            // cb_int_st
            // 
            this.cb_int_st.AutoSize = true;
            this.cb_int_st.Location = new System.Drawing.Point(6, 42);
            this.cb_int_st.Name = "cb_int_st";
            this.cb_int_st.Size = new System.Drawing.Size(68, 17);
            this.cb_int_st.TabIndex = 0;
            this.cb_int_st.Text = "On Store";
            this.cb_int_st.UseVisualStyleBackColor = true;
            // 
            // cb_int_onLoad
            // 
            this.cb_int_onLoad.AutoSize = true;
            this.cb_int_onLoad.Location = new System.Drawing.Point(6, 19);
            this.cb_int_onLoad.Name = "cb_int_onLoad";
            this.cb_int_onLoad.Size = new System.Drawing.Size(67, 17);
            this.cb_int_onLoad.TabIndex = 0;
            this.cb_int_onLoad.Text = "On Load";
            this.cb_int_onLoad.UseVisualStyleBackColor = true;
            // 
            // gbShIn
            // 
            this.gbShIn.Controls.Add(this.rbsi1);
            this.gbShIn.Controls.Add(this.rbsi0);
            this.gbShIn.Location = new System.Drawing.Point(21, 57);
            this.gbShIn.Name = "gbShIn";
            this.gbShIn.Size = new System.Drawing.Size(115, 69);
            this.gbShIn.TabIndex = 1;
            this.gbShIn.TabStop = false;
            this.gbShIn.Text = "Default Shift Value";
            // 
            // rbsi1
            // 
            this.rbsi1.AutoSize = true;
            this.rbsi1.Location = new System.Drawing.Point(6, 42);
            this.rbsi1.Name = "rbsi1";
            this.rbsi1.Size = new System.Drawing.Size(31, 17);
            this.rbsi1.TabIndex = 0;
            this.rbsi1.Text = "1";
            this.rbsi1.UseVisualStyleBackColor = true;
            // 
            // rbsi0
            // 
            this.rbsi0.AutoSize = true;
            this.rbsi0.Checked = true;
            this.rbsi0.Location = new System.Drawing.Point(6, 19);
            this.rbsi0.Name = "rbsi0";
            this.rbsi0.Size = new System.Drawing.Size(31, 17);
            this.rbsi0.TabIndex = 0;
            this.rbsi0.TabStop = true;
            this.rbsi0.Text = "0";
            this.rbsi0.UseVisualStyleBackColor = true;
            // 
            // cbInterrupt
            // 
            this.cbInterrupt.AutoSize = true;
            this.cbInterrupt.Location = new System.Drawing.Point(334, 31);
            this.cbInterrupt.Name = "cbInterrupt";
            this.cbInterrupt.Size = new System.Drawing.Size(87, 17);
            this.cbInterrupt.TabIndex = 0;
            this.cbInterrupt.Text = "Use Interrupt";
            this.cbInterrupt.UseVisualStyleBackColor = true;
            this.cbInterrupt.CheckedChanged += new System.EventHandler(this.cbInterrupt_CheckedChanged);
            // 
            // cbShiftOut
            // 
            this.cbShiftOut.AutoSize = true;
            this.cbShiftOut.Location = new System.Drawing.Point(187, 31);
            this.cbShiftOut.Name = "cbShiftOut";
            this.cbShiftOut.Size = new System.Drawing.Size(89, 17);
            this.cbShiftOut.TabIndex = 0;
            this.cbShiftOut.Text = "Use Shift Out";
            this.cbShiftOut.UseVisualStyleBackColor = true;
            this.cbShiftOut.CheckedChanged += new System.EventHandler(this.cbShiftIn_CheckedChanged);
            // 
            // cbShiftIn
            // 
            this.cbShiftIn.AutoSize = true;
            this.cbShiftIn.Location = new System.Drawing.Point(21, 31);
            this.cbShiftIn.Name = "cbShiftIn";
            this.cbShiftIn.Size = new System.Drawing.Size(81, 17);
            this.cbShiftIn.TabIndex = 0;
            this.cbShiftIn.Text = "Use Shift In";
            this.cbShiftIn.UseVisualStyleBackColor = true;
            this.cbShiftIn.CheckedChanged += new System.EventHandler(this.cbShiftIn_CheckedChanged);
            // 
            // cbLoad
            // 
            this.cbLoad.AutoSize = true;
            this.cbLoad.Location = new System.Drawing.Point(187, 54);
            this.cbLoad.Name = "cbLoad";
            this.cbLoad.Size = new System.Drawing.Size(72, 17);
            this.cbLoad.TabIndex = 0;
            this.cbLoad.Text = "Use Load";
            this.cbLoad.UseVisualStyleBackColor = true;
            // 
            // cbStore
            // 
            this.cbStore.AutoSize = true;
            this.cbStore.Location = new System.Drawing.Point(187, 77);
            this.cbStore.Name = "cbStore";
            this.cbStore.Size = new System.Drawing.Size(73, 17);
            this.cbStore.TabIndex = 0;
            this.cbStore.Text = "Use Store";
            this.cbStore.UseVisualStyleBackColor = true;
            // 
            // gbGeneral
            // 
            this.gbGeneral.Controls.Add(this.cbFIFOS);
            this.gbGeneral.Controls.Add(this.cbDir);
            this.gbGeneral.Controls.Add(this.cbLength);
            this.gbGeneral.Controls.Add(this.label3);
            this.gbGeneral.Controls.Add(this.label2);
            this.gbGeneral.Controls.Add(this.label1);
            this.gbGeneral.Dock = System.Windows.Forms.DockStyle.Top;
            this.gbGeneral.Location = new System.Drawing.Point(0, 0);
            this.gbGeneral.MinimumSize = new System.Drawing.Size(0, 10);
            this.gbGeneral.Name = "gbGeneral";
            this.gbGeneral.Size = new System.Drawing.Size(483, 70);
            this.gbGeneral.TabIndex = 0;
            this.gbGeneral.TabStop = false;
            this.gbGeneral.Text = "General";
            // 
            // cbFIFOS
            // 
            this.cbFIFOS.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.cbFIFOS.FormattingEnabled = true;
            this.cbFIFOS.Items.AddRange(new object[] {
            "1",
            "4"});
            this.cbFIFOS.Location = new System.Drawing.Point(253, 29);
            this.cbFIFOS.Name = "cbFIFOS";
            this.cbFIFOS.Size = new System.Drawing.Size(50, 21);
            this.cbFIFOS.TabIndex = 1;
            // 
            // cbDir
            // 
            this.cbDir.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.cbDir.FormattingEnabled = true;
            this.cbDir.Items.AddRange(new object[] {
            "Left",
            "Right"});
            this.cbDir.Location = new System.Drawing.Point(405, 29);
            this.cbDir.Name = "cbDir";
            this.cbDir.Size = new System.Drawing.Size(54, 21);
            this.cbDir.TabIndex = 1;
            // 
            // cbLength
            // 
            this.cbLength.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.cbLength.FormattingEnabled = true;
            this.cbLength.Location = new System.Drawing.Point(86, 29);
            this.cbLength.Name = "cbLength";
            this.cbLength.Size = new System.Drawing.Size(50, 21);
            this.cbLength.TabIndex = 1;
            // 
            // label3
            // 
            this.label3.AutoSize = true;
            this.label3.Location = new System.Drawing.Point(157, 32);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(90, 13);
            this.label3.TabIndex = 0;
            this.label3.Text = "FIFO Size (bytes):";
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Location = new System.Drawing.Point(327, 32);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(73, 13);
            this.label2.TabIndex = 0;
            this.label2.Text = "Shift Direction";
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(14, 32);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(68, 13);
            this.label1.TabIndex = 0;
            this.label1.Text = "Length (bits):";
            // 
            // CyGeneralParamsTab
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.AutoScroll = true;
            this.BackColor = System.Drawing.SystemColors.Control;
            this.Controls.Add(this.gbCustParams);
            this.Controls.Add(this.gbGeneral);
            this.Name = "CyGeneralParamsTab";
            this.Size = new System.Drawing.Size(483, 243);
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

        public System.Windows.Forms.GroupBox gbCustParams;
        public System.Windows.Forms.CheckBox cbInterrupt;
        public System.Windows.Forms.CheckBox cbShiftOut;
        public System.Windows.Forms.CheckBox cbShiftIn;
        public System.Windows.Forms.CheckBox cbStore;
        public System.Windows.Forms.GroupBox gbGeneral;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.Label label1;
        public System.Windows.Forms.ComboBox cbFIFOS;
        public System.Windows.Forms.ComboBox cbDir;
        public System.Windows.Forms.ComboBox cbLength;
        private System.Windows.Forms.Label label3;
        public System.Windows.Forms.CheckBox cbLoad;
        private System.Windows.Forms.GroupBox gbShIn;
        private System.Windows.Forms.GroupBox gbInter;
        public System.Windows.Forms.RadioButton rbsi1;
        public System.Windows.Forms.RadioButton rbsi0;
        public System.Windows.Forms.CheckBox cb_int_reset;
        public System.Windows.Forms.CheckBox cb_int_st;
        public System.Windows.Forms.CheckBox cb_int_onLoad;
    }
}
