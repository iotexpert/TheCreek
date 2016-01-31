namespace I2S_v2_30
{
    partial class CyI2SBasic
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
            this.components = new System.ComponentModel.Container();
            this.panelTop = new System.Windows.Forms.Panel();
            this.comboBoxWordSelectPeriod = new System.Windows.Forms.ComboBox();
            this.comboBoxDataBits = new System.Windows.Forms.ComboBox();
            this.label7 = new System.Windows.Forms.Label();
            this.label6 = new System.Windows.Forms.Label();
            this.radioButtonRxAndTx = new System.Windows.Forms.RadioButton();
            this.radioButtonTxOnly = new System.Windows.Forms.RadioButton();
            this.radioButtonRxOnly = new System.Windows.Forms.RadioButton();
            this.label5 = new System.Windows.Forms.Label();
            this.panelBottom = new System.Windows.Forms.Panel();
            this.pictureBox = new System.Windows.Forms.PictureBox();
            this.errorProvider = new System.Windows.Forms.ErrorProvider(this.components);
            this.panelTop.SuspendLayout();
            this.panelBottom.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.pictureBox)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.errorProvider)).BeginInit();
            this.SuspendLayout();
            // 
            // panelTop
            // 
            this.panelTop.Controls.Add(this.comboBoxWordSelectPeriod);
            this.panelTop.Controls.Add(this.comboBoxDataBits);
            this.panelTop.Controls.Add(this.label7);
            this.panelTop.Controls.Add(this.label6);
            this.panelTop.Controls.Add(this.radioButtonRxAndTx);
            this.panelTop.Controls.Add(this.radioButtonTxOnly);
            this.panelTop.Controls.Add(this.radioButtonRxOnly);
            this.panelTop.Controls.Add(this.label5);
            this.panelTop.Dock = System.Windows.Forms.DockStyle.Top;
            this.panelTop.Location = new System.Drawing.Point(0, 0);
            this.panelTop.Name = "panelTop";
            this.panelTop.Size = new System.Drawing.Size(454, 111);
            this.panelTop.TabIndex = 29;
            // 
            // comboBoxWordSelectPeriod
            // 
            this.comboBoxWordSelectPeriod.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.comboBoxWordSelectPeriod.FormattingEnabled = true;
            this.errorProvider.SetIconAlignment(this.comboBoxWordSelectPeriod, System.Windows.Forms.ErrorIconAlignment.MiddleLeft);
            this.comboBoxWordSelectPeriod.Items.AddRange(new object[] {
            "16",
            "32",
            "48",
            "64"});
            this.comboBoxWordSelectPeriod.Location = new System.Drawing.Point(141, 75);
            this.comboBoxWordSelectPeriod.Name = "comboBoxWordSelectPeriod";
            this.comboBoxWordSelectPeriod.Size = new System.Drawing.Size(110, 21);
            this.comboBoxWordSelectPeriod.TabIndex = 4;
            this.comboBoxWordSelectPeriod.Validating += new System.ComponentModel.CancelEventHandler(this.WordSelectPeriodValidating);
            this.comboBoxWordSelectPeriod.SelectedIndexChanged += new System.EventHandler(this.comboBoxWordSelectPeriod_SelectedIndexChanged);
            // 
            // comboBoxDataBits
            // 
            this.comboBoxDataBits.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.comboBoxDataBits.FormattingEnabled = true;
            this.comboBoxDataBits.Items.AddRange(new object[] {
            "8",
            "9",
            "10",
            "11",
            "12",
            "13",
            "14",
            "15",
            "16",
            "17",
            "18",
            "19",
            "20",
            "21",
            "22",
            "23",
            "24",
            "25",
            "26",
            "27",
            "28",
            "29",
            "30",
            "31",
            "32"});
            this.comboBoxDataBits.Location = new System.Drawing.Point(91, 48);
            this.comboBoxDataBits.Name = "comboBoxDataBits";
            this.comboBoxDataBits.Size = new System.Drawing.Size(160, 21);
            this.comboBoxDataBits.TabIndex = 3;
            this.comboBoxDataBits.SelectedIndexChanged += new System.EventHandler(this.comboBoxDataBits_SelectedIndexChanged);
            // 
            // label7
            // 
            this.label7.AutoSize = true;
            this.label7.Location = new System.Drawing.Point(19, 78);
            this.label7.Name = "label7";
            this.label7.Size = new System.Drawing.Size(102, 13);
            this.label7.TabIndex = 34;
            this.label7.Text = "Word Select Period:";
            // 
            // label6
            // 
            this.label6.AutoSize = true;
            this.label6.Location = new System.Drawing.Point(19, 51);
            this.label6.Name = "label6";
            this.label6.Size = new System.Drawing.Size(53, 13);
            this.label6.TabIndex = 33;
            this.label6.Text = "Data Bits:";
            // 
            // radioButtonRxAndTx
            // 
            this.radioButtonRxAndTx.AutoSize = true;
            this.radioButtonRxAndTx.Location = new System.Drawing.Point(230, 21);
            this.radioButtonRxAndTx.Name = "radioButtonRxAndTx";
            this.radioButtonRxAndTx.Size = new System.Drawing.Size(74, 17);
            this.radioButtonRxAndTx.TabIndex = 2;
            this.radioButtonRxAndTx.TabStop = true;
            this.radioButtonRxAndTx.Text = "Rx and Tx";
            this.radioButtonRxAndTx.UseVisualStyleBackColor = true;
            this.radioButtonRxAndTx.CheckedChanged += new System.EventHandler(this.radioButtonDirection_CheckedChanged);
            // 
            // radioButtonTxOnly
            // 
            this.radioButtonTxOnly.AutoSize = true;
            this.radioButtonTxOnly.Location = new System.Drawing.Point(161, 21);
            this.radioButtonTxOnly.Name = "radioButtonTxOnly";
            this.radioButtonTxOnly.Size = new System.Drawing.Size(61, 17);
            this.radioButtonTxOnly.TabIndex = 1;
            this.radioButtonTxOnly.TabStop = true;
            this.radioButtonTxOnly.Text = "Tx Only";
            this.radioButtonTxOnly.UseVisualStyleBackColor = true;
            this.radioButtonTxOnly.CheckedChanged += new System.EventHandler(this.radioButtonDirection_CheckedChanged);
            // 
            // radioButtonRxOnly
            // 
            this.radioButtonRxOnly.AutoSize = true;
            this.radioButtonRxOnly.Location = new System.Drawing.Point(91, 21);
            this.radioButtonRxOnly.Name = "radioButtonRxOnly";
            this.radioButtonRxOnly.Size = new System.Drawing.Size(62, 17);
            this.radioButtonRxOnly.TabIndex = 0;
            this.radioButtonRxOnly.TabStop = true;
            this.radioButtonRxOnly.Text = "Rx Only";
            this.radioButtonRxOnly.UseVisualStyleBackColor = true;
            this.radioButtonRxOnly.CheckedChanged += new System.EventHandler(this.radioButtonDirection_CheckedChanged);
            // 
            // label5
            // 
            this.label5.AutoSize = true;
            this.label5.Location = new System.Drawing.Point(19, 23);
            this.label5.Name = "label5";
            this.label5.Size = new System.Drawing.Size(52, 13);
            this.label5.TabIndex = 29;
            this.label5.Text = "Direction:";
            // 
            // panelBottom
            // 
            this.panelBottom.Controls.Add(this.pictureBox);
            this.panelBottom.Dock = System.Windows.Forms.DockStyle.Fill;
            this.panelBottom.Location = new System.Drawing.Point(0, 111);
            this.panelBottom.Name = "panelBottom";
            this.panelBottom.Size = new System.Drawing.Size(454, 135);
            this.panelBottom.TabIndex = 30;
            // 
            // pictureBox
            // 
            this.pictureBox.BackColor = System.Drawing.SystemColors.Window;
            this.pictureBox.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.pictureBox.Dock = System.Windows.Forms.DockStyle.Fill;
            this.pictureBox.Location = new System.Drawing.Point(0, 0);
            this.pictureBox.Name = "pictureBox";
            this.pictureBox.Size = new System.Drawing.Size(454, 135);
            this.pictureBox.SizeMode = System.Windows.Forms.PictureBoxSizeMode.Zoom;
            this.pictureBox.TabIndex = 0;
            this.pictureBox.TabStop = false;
            // 
            // errorProvider
            // 
            this.errorProvider.ContainerControl = this;
            // 
            // CyI2SBasic
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.AutoScroll = true;
            this.Controls.Add(this.panelBottom);
            this.Controls.Add(this.panelTop);
            this.Name = "CyI2SBasic";
            this.Size = new System.Drawing.Size(454, 246);
            this.VisibleChanged += new System.EventHandler(this.CyI2SBasic_VisibleChanged);
            this.panelTop.ResumeLayout(false);
            this.panelTop.PerformLayout();
            this.panelBottom.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.pictureBox)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.errorProvider)).EndInit();
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.Panel panelTop;
        public System.Windows.Forms.ComboBox comboBoxWordSelectPeriod;
        public System.Windows.Forms.ComboBox comboBoxDataBits;
        private System.Windows.Forms.Label label7;
        private System.Windows.Forms.Label label6;
        public System.Windows.Forms.RadioButton radioButtonRxAndTx;
        public System.Windows.Forms.RadioButton radioButtonTxOnly;
        public System.Windows.Forms.RadioButton radioButtonRxOnly;
        private System.Windows.Forms.Label label5;
        private System.Windows.Forms.Panel panelBottom;
        private System.Windows.Forms.PictureBox pictureBox;
        private System.Windows.Forms.ErrorProvider errorProvider;

    }
}
