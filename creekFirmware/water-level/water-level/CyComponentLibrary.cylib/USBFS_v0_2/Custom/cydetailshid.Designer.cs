namespace USBFS_v0_2
{
    partial class CyDetailsHID
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
            this.groupBoxParams = new System.Windows.Forms.GroupBox();
            this.comboBoxCountryCode = new System.Windows.Forms.ComboBox();
            this.label7 = new System.Windows.Forms.Label();
            this.comboBoxReport = new System.Windows.Forms.ComboBox();
            this.comboBoxType = new System.Windows.Forms.ComboBox();
            this.label4 = new System.Windows.Forms.Label();
            this.label3 = new System.Windows.Forms.Label();
            this.buttonApply = new System.Windows.Forms.Button();
            this.groupBoxParams.SuspendLayout();
            this.SuspendLayout();
            // 
            // groupBoxParams
            // 
            this.groupBoxParams.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left)
                        | System.Windows.Forms.AnchorStyles.Right)));
            this.groupBoxParams.Controls.Add(this.comboBoxCountryCode);
            this.groupBoxParams.Controls.Add(this.label7);
            this.groupBoxParams.Controls.Add(this.comboBoxReport);
            this.groupBoxParams.Controls.Add(this.comboBoxType);
            this.groupBoxParams.Controls.Add(this.label4);
            this.groupBoxParams.Controls.Add(this.label3);
            this.groupBoxParams.Location = new System.Drawing.Point(0, 9);
            this.groupBoxParams.Name = "groupBoxParams";
            this.groupBoxParams.Size = new System.Drawing.Size(313, 121);
            this.groupBoxParams.TabIndex = 1;
            this.groupBoxParams.TabStop = false;
            this.groupBoxParams.Text = "Device Attributes";
            // 
            // comboBoxCountryCode
            // 
            this.comboBoxCountryCode.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.comboBoxCountryCode.FormattingEnabled = true;
            this.comboBoxCountryCode.Location = new System.Drawing.Point(119, 52);
            this.comboBoxCountryCode.Name = "comboBoxCountryCode";
            this.comboBoxCountryCode.Size = new System.Drawing.Size(121, 21);
            this.comboBoxCountryCode.TabIndex = 13;
            this.comboBoxCountryCode.Validated += new System.EventHandler(this.comboBoxCountryCode_Validated);
            // 
            // label7
            // 
            this.label7.AutoSize = true;
            this.label7.Location = new System.Drawing.Point(6, 55);
            this.label7.Name = "label7";
            this.label7.Size = new System.Drawing.Size(71, 13);
            this.label7.TabIndex = 12;
            this.label7.Text = "Country Code";
            // 
            // comboBoxReport
            // 
            this.comboBoxReport.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.comboBoxReport.FormattingEnabled = true;
            this.comboBoxReport.Location = new System.Drawing.Point(119, 79);
            this.comboBoxReport.Name = "comboBoxReport";
            this.comboBoxReport.Size = new System.Drawing.Size(121, 21);
            this.comboBoxReport.TabIndex = 7;
            this.comboBoxReport.Validated += new System.EventHandler(this.comboBoxReport_Validated);
            this.comboBoxReport.DropDown += new System.EventHandler(this.comboBoxReport_DropDown);
            // 
            // comboBoxType
            // 
            this.comboBoxType.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.comboBoxType.FormattingEnabled = true;
            this.comboBoxType.Items.AddRange(new object[] {
            "Report",
            "Physical"});
            this.comboBoxType.Location = new System.Drawing.Point(119, 25);
            this.comboBoxType.Name = "comboBoxType";
            this.comboBoxType.Size = new System.Drawing.Size(121, 21);
            this.comboBoxType.TabIndex = 4;
            this.comboBoxType.Validated += new System.EventHandler(this.comboBoxType_Validated);
            // 
            // label4
            // 
            this.label4.AutoSize = true;
            this.label4.Location = new System.Drawing.Point(6, 82);
            this.label4.Name = "label4";
            this.label4.Size = new System.Drawing.Size(61, 13);
            this.label4.TabIndex = 3;
            this.label4.Text = "HID Report";
            // 
            // label3
            // 
            this.label3.AutoSize = true;
            this.label3.Location = new System.Drawing.Point(6, 28);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(82, 13);
            this.label3.TabIndex = 2;
            this.label3.Text = "Descriptor Type";
            // 
            // buttonApply
            // 
            this.buttonApply.Location = new System.Drawing.Point(122, 136);
            this.buttonApply.Name = "buttonApply";
            this.buttonApply.Size = new System.Drawing.Size(75, 23);
            this.buttonApply.TabIndex = 5;
            this.buttonApply.Text = "Apply";
            this.buttonApply.UseVisualStyleBackColor = true;
            this.buttonApply.Visible = false;
            this.buttonApply.Click += new System.EventHandler(this.buttonApply_Click);
            // 
            // CyDetailsHID
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.Controls.Add(this.buttonApply);
            this.Controls.Add(this.groupBoxParams);
            this.Name = "CyDetailsHID";
            this.Size = new System.Drawing.Size(319, 169);
            this.groupBoxParams.ResumeLayout(false);
            this.groupBoxParams.PerformLayout();
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.GroupBox groupBoxParams;
        private System.Windows.Forms.ComboBox comboBoxCountryCode;
        private System.Windows.Forms.Label label7;
        private System.Windows.Forms.ComboBox comboBoxReport;
        private System.Windows.Forms.ComboBox comboBoxType;
        private System.Windows.Forms.Label label4;
        private System.Windows.Forms.Label label3;
        private System.Windows.Forms.Button buttonApply;
    }
}
