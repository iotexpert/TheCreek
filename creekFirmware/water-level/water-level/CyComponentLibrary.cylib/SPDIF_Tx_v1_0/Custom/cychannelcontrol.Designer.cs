namespace SPDIF_Tx_v1_0
{
    partial class CyChannelControl
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
            this.m_groupBox3 = new System.Windows.Forms.GroupBox();
            this.m_numChannelNumber = new System.Windows.Forms.NumericUpDown();
            this.m_numSourceNumber = new System.Windows.Forms.NumericUpDown();
            this.m_lblChannelNumber = new System.Windows.Forms.Label();
            this.m_lblSourceNumber = new System.Windows.Forms.Label();
            this.m_groupBox2 = new System.Windows.Forms.GroupBox();
            this.m_cbCategory = new System.Windows.Forms.ComboBox();
            this.m_cbClockAccuracy = new System.Windows.Forms.ComboBox();
            this.m_cbPreEmphasis = new System.Windows.Forms.ComboBox();
            this.m_cbCopyright = new System.Windows.Forms.ComboBox();
            this.m_cbDataType = new System.Windows.Forms.ComboBox();
            this.m_lblClockAccuracy = new System.Windows.Forms.Label();
            this.m_lblCategory = new System.Windows.Forms.Label();
            this.m_lblPreEmphasis = new System.Windows.Forms.Label();
            this.m_lblCopyright = new System.Windows.Forms.Label();
            this.m_lblDataType = new System.Windows.Forms.Label();
            this.m_errorProvider = new System.Windows.Forms.ErrorProvider(this.components);
            this.m_groupBox3.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.m_numChannelNumber)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.m_numSourceNumber)).BeginInit();
            this.m_groupBox2.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.m_errorProvider)).BeginInit();
            this.SuspendLayout();
            // 
            // m_groupBox3
            // 
            this.m_groupBox3.Controls.Add(this.m_numChannelNumber);
            this.m_groupBox3.Controls.Add(this.m_numSourceNumber);
            this.m_groupBox3.Controls.Add(this.m_lblChannelNumber);
            this.m_groupBox3.Controls.Add(this.m_lblSourceNumber);
            this.m_groupBox3.Dock = System.Windows.Forms.DockStyle.Top;
            this.m_groupBox3.Location = new System.Drawing.Point(0, 141);
            this.m_groupBox3.Name = "m_groupBox3";
            this.m_groupBox3.Size = new System.Drawing.Size(353, 43);
            this.m_groupBox3.TabIndex = 4;
            this.m_groupBox3.TabStop = false;
            // 
            // m_numChannelNumber
            // 
            this.m_numChannelNumber.Location = new System.Drawing.Point(287, 14);
            this.m_numChannelNumber.Maximum = new decimal(new int[] {
            -1981284353,
            -1966660860,
            0,
            0});
            this.m_numChannelNumber.Name = "m_numChannelNumber";
            this.m_numChannelNumber.Size = new System.Drawing.Size(53, 20);
            this.m_numChannelNumber.TabIndex = 3;
            this.m_numChannelNumber.KeyPress += new System.Windows.Forms.KeyPressEventHandler(this.numericUpDown_KeyPress);
            this.m_numChannelNumber.KeyDown += new System.Windows.Forms.KeyEventHandler(this.numericUpDown_KeyDown);
            // 
            // m_numSourceNumber
            // 
            this.m_numSourceNumber.Location = new System.Drawing.Point(106, 14);
            this.m_numSourceNumber.Maximum = new decimal(new int[] {
            -1981284353,
            -1966660860,
            0,
            0});
            this.m_numSourceNumber.Name = "m_numSourceNumber";
            this.m_numSourceNumber.Size = new System.Drawing.Size(53, 20);
            this.m_numSourceNumber.TabIndex = 2;
            this.m_numSourceNumber.KeyPress += new System.Windows.Forms.KeyPressEventHandler(this.numericUpDown_KeyPress);
            this.m_numSourceNumber.KeyDown += new System.Windows.Forms.KeyEventHandler(this.numericUpDown_KeyDown);
            // 
            // m_lblChannelNumber
            // 
            this.m_lblChannelNumber.AutoSize = true;
            this.m_lblChannelNumber.Location = new System.Drawing.Point(192, 16);
            this.m_lblChannelNumber.Name = "m_lblChannelNumber";
            this.m_lblChannelNumber.Size = new System.Drawing.Size(89, 13);
            this.m_lblChannelNumber.TabIndex = 1;
            this.m_lblChannelNumber.Text = "Channel Number:";
            // 
            // m_lblSourceNumber
            // 
            this.m_lblSourceNumber.AutoSize = true;
            this.m_lblSourceNumber.Location = new System.Drawing.Point(16, 16);
            this.m_lblSourceNumber.Name = "m_lblSourceNumber";
            this.m_lblSourceNumber.Size = new System.Drawing.Size(84, 13);
            this.m_lblSourceNumber.TabIndex = 0;
            this.m_lblSourceNumber.Text = "Source Number:";
            // 
            // m_groupBox2
            // 
            this.m_groupBox2.Controls.Add(this.m_cbCategory);
            this.m_groupBox2.Controls.Add(this.m_cbClockAccuracy);
            this.m_groupBox2.Controls.Add(this.m_cbPreEmphasis);
            this.m_groupBox2.Controls.Add(this.m_cbCopyright);
            this.m_groupBox2.Controls.Add(this.m_cbDataType);
            this.m_groupBox2.Controls.Add(this.m_lblClockAccuracy);
            this.m_groupBox2.Controls.Add(this.m_lblCategory);
            this.m_groupBox2.Controls.Add(this.m_lblPreEmphasis);
            this.m_groupBox2.Controls.Add(this.m_lblCopyright);
            this.m_groupBox2.Controls.Add(this.m_lblDataType);
            this.m_groupBox2.Dock = System.Windows.Forms.DockStyle.Top;
            this.m_groupBox2.Location = new System.Drawing.Point(0, 0);
            this.m_groupBox2.Name = "m_groupBox2";
            this.m_groupBox2.Size = new System.Drawing.Size(353, 141);
            this.m_groupBox2.TabIndex = 3;
            this.m_groupBox2.TabStop = false;
            // 
            // m_cbCategory
            // 
            this.m_cbCategory.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.m_cbCategory.FormattingEnabled = true;
            this.m_cbCategory.Location = new System.Drawing.Point(106, 88);
            this.m_cbCategory.Name = "m_cbCategory";
            this.m_cbCategory.Size = new System.Drawing.Size(235, 21);
            this.m_cbCategory.TabIndex = 9;
            this.m_cbCategory.SelectedIndexChanged += new System.EventHandler(this.m_cbCategory_SelectedIndexChanged);
            // 
            // m_cbClockAccuracy
            // 
            this.m_cbClockAccuracy.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.m_cbClockAccuracy.FormattingEnabled = true;
            this.m_cbClockAccuracy.Location = new System.Drawing.Point(106, 113);
            this.m_cbClockAccuracy.Name = "m_cbClockAccuracy";
            this.m_cbClockAccuracy.Size = new System.Drawing.Size(235, 21);
            this.m_cbClockAccuracy.TabIndex = 8;
            this.m_cbClockAccuracy.SelectedIndexChanged += new System.EventHandler(this.m_cbClockAccuracy_SelectedIndexChanged);
            // 
            // m_cbPreEmphasis
            // 
            this.m_cbPreEmphasis.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.m_cbPreEmphasis.FormattingEnabled = true;
            this.m_cbPreEmphasis.Location = new System.Drawing.Point(106, 63);
            this.m_cbPreEmphasis.Name = "m_cbPreEmphasis";
            this.m_cbPreEmphasis.Size = new System.Drawing.Size(235, 21);
            this.m_cbPreEmphasis.TabIndex = 7;
            this.m_cbPreEmphasis.SelectedIndexChanged += new System.EventHandler(this.m_cbPreEmphasis_SelectedIndexChanged);
            // 
            // m_cbCopyright
            // 
            this.m_cbCopyright.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.m_cbCopyright.FormattingEnabled = true;
            this.m_cbCopyright.Location = new System.Drawing.Point(106, 38);
            this.m_cbCopyright.Name = "m_cbCopyright";
            this.m_cbCopyright.Size = new System.Drawing.Size(235, 21);
            this.m_cbCopyright.TabIndex = 6;
            this.m_cbCopyright.SelectedIndexChanged += new System.EventHandler(this.m_cbCopyright_SelectedIndexChanged);
            // 
            // m_cbDataType
            // 
            this.m_cbDataType.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.m_cbDataType.FormattingEnabled = true;
            this.m_cbDataType.Location = new System.Drawing.Point(106, 13);
            this.m_cbDataType.Name = "m_cbDataType";
            this.m_cbDataType.Size = new System.Drawing.Size(235, 21);
            this.m_cbDataType.TabIndex = 5;
            this.m_cbDataType.SelectedIndexChanged += new System.EventHandler(this.m_cbDataType_SelectedIndexChanged);
            // 
            // m_lblClockAccuracy
            // 
            this.m_lblClockAccuracy.AutoSize = true;
            this.m_lblClockAccuracy.Location = new System.Drawing.Point(15, 116);
            this.m_lblClockAccuracy.Name = "m_lblClockAccuracy";
            this.m_lblClockAccuracy.Size = new System.Drawing.Size(85, 13);
            this.m_lblClockAccuracy.TabIndex = 4;
            this.m_lblClockAccuracy.Text = "Clock Accuracy:";
            // 
            // m_lblCategory
            // 
            this.m_lblCategory.AutoSize = true;
            this.m_lblCategory.Location = new System.Drawing.Point(15, 91);
            this.m_lblCategory.Name = "m_lblCategory";
            this.m_lblCategory.Size = new System.Drawing.Size(52, 13);
            this.m_lblCategory.TabIndex = 3;
            this.m_lblCategory.Text = "Category:";
            // 
            // m_lblPreEmphasis
            // 
            this.m_lblPreEmphasis.AutoSize = true;
            this.m_lblPreEmphasis.Location = new System.Drawing.Point(15, 66);
            this.m_lblPreEmphasis.Name = "m_lblPreEmphasis";
            this.m_lblPreEmphasis.Size = new System.Drawing.Size(73, 13);
            this.m_lblPreEmphasis.TabIndex = 2;
            this.m_lblPreEmphasis.Text = "Pre-emphasis:";
            // 
            // m_lblCopyright
            // 
            this.m_lblCopyright.AutoSize = true;
            this.m_lblCopyright.Location = new System.Drawing.Point(15, 41);
            this.m_lblCopyright.Name = "m_lblCopyright";
            this.m_lblCopyright.Size = new System.Drawing.Size(54, 13);
            this.m_lblCopyright.TabIndex = 1;
            this.m_lblCopyright.Text = "Copyright:";
            // 
            // m_lblDataType
            // 
            this.m_lblDataType.AutoSize = true;
            this.m_lblDataType.Location = new System.Drawing.Point(15, 16);
            this.m_lblDataType.Name = "m_lblDataType";
            this.m_lblDataType.Size = new System.Drawing.Size(60, 13);
            this.m_lblDataType.TabIndex = 0;
            this.m_lblDataType.Text = "Data Type:";
            // 
            // m_errorProvider
            // 
            this.m_errorProvider.ContainerControl = this;
            // 
            // CyChannelControl
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.Controls.Add(this.m_groupBox3);
            this.Controls.Add(this.m_groupBox2);
            this.Name = "CyChannelControl";
            this.Size = new System.Drawing.Size(353, 187);
            this.m_groupBox3.ResumeLayout(false);
            this.m_groupBox3.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.m_numChannelNumber)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.m_numSourceNumber)).EndInit();
            this.m_groupBox2.ResumeLayout(false);
            this.m_groupBox2.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.m_errorProvider)).EndInit();
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.GroupBox m_groupBox3;
        private System.Windows.Forms.NumericUpDown m_numChannelNumber;
        private System.Windows.Forms.NumericUpDown m_numSourceNumber;
        private System.Windows.Forms.Label m_lblChannelNumber;
        private System.Windows.Forms.Label m_lblSourceNumber;
        private System.Windows.Forms.GroupBox m_groupBox2;
        private System.Windows.Forms.ComboBox m_cbCategory;
        private System.Windows.Forms.ComboBox m_cbClockAccuracy;
        private System.Windows.Forms.ComboBox m_cbPreEmphasis;
        private System.Windows.Forms.ComboBox m_cbCopyright;
        private System.Windows.Forms.ComboBox m_cbDataType;
        private System.Windows.Forms.Label m_lblClockAccuracy;
        private System.Windows.Forms.Label m_lblCategory;
        private System.Windows.Forms.Label m_lblPreEmphasis;
        private System.Windows.Forms.Label m_lblCopyright;
        private System.Windows.Forms.Label m_lblDataType;
        private System.Windows.Forms.ErrorProvider m_errorProvider;
    }
}
