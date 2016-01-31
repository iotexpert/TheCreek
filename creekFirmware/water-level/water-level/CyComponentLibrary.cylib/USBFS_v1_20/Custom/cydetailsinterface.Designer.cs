namespace USBFS_v1_20
{
    partial class CyDetailsInterface
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
            this.labelAlternateSettings = new System.Windows.Forms.Label();
            this.labelInterfaceNumber = new System.Windows.Forms.Label();
            this.comboBoxProtocol = new System.Windows.Forms.ComboBox();
            this.labelProtocol = new System.Windows.Forms.Label();
            this.comboBoxInterfaceString = new System.Windows.Forms.ComboBox();
            this.label5 = new System.Windows.Forms.Label();
            this.comboBoxSubclass = new System.Windows.Forms.ComboBox();
            this.label9 = new System.Windows.Forms.Label();
            this.comboBoxClass = new System.Windows.Forms.ComboBox();
            this.label4 = new System.Windows.Forms.Label();
            this.label3 = new System.Windows.Forms.Label();
            this.label1 = new System.Windows.Forms.Label();
            this.groupBoxParams.SuspendLayout();
            this.SuspendLayout();
            // 
            // groupBoxParams
            // 
            this.groupBoxParams.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left)
                        | System.Windows.Forms.AnchorStyles.Right)));
            this.groupBoxParams.Controls.Add(this.labelAlternateSettings);
            this.groupBoxParams.Controls.Add(this.labelInterfaceNumber);
            this.groupBoxParams.Controls.Add(this.comboBoxProtocol);
            this.groupBoxParams.Controls.Add(this.labelProtocol);
            this.groupBoxParams.Controls.Add(this.comboBoxInterfaceString);
            this.groupBoxParams.Controls.Add(this.label5);
            this.groupBoxParams.Controls.Add(this.comboBoxSubclass);
            this.groupBoxParams.Controls.Add(this.label9);
            this.groupBoxParams.Controls.Add(this.comboBoxClass);
            this.groupBoxParams.Controls.Add(this.label4);
            this.groupBoxParams.Controls.Add(this.label3);
            this.groupBoxParams.Controls.Add(this.label1);
            this.groupBoxParams.Location = new System.Drawing.Point(0, 9);
            this.groupBoxParams.Name = "groupBoxParams";
            this.groupBoxParams.Size = new System.Drawing.Size(309, 184);
            this.groupBoxParams.TabIndex = 1;
            this.groupBoxParams.TabStop = false;
            this.groupBoxParams.Text = "Interface Attributes";
            // 
            // labelAlternateSettings
            // 
            this.labelAlternateSettings.BackColor = System.Drawing.Color.Transparent;
            this.labelAlternateSettings.BorderStyle = System.Windows.Forms.BorderStyle.Fixed3D;
            this.labelAlternateSettings.Location = new System.Drawing.Point(119, 70);
            this.labelAlternateSettings.Name = "labelAlternateSettings";
            this.labelAlternateSettings.Size = new System.Drawing.Size(121, 21);
            this.labelAlternateSettings.TabIndex = 14;
            this.labelAlternateSettings.Text = "0";
            this.labelAlternateSettings.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            // 
            // labelInterfaceNumber
            // 
            this.labelInterfaceNumber.BackColor = System.Drawing.Color.Transparent;
            this.labelInterfaceNumber.BorderStyle = System.Windows.Forms.BorderStyle.Fixed3D;
            this.labelInterfaceNumber.Location = new System.Drawing.Point(119, 43);
            this.labelInterfaceNumber.Name = "labelInterfaceNumber";
            this.labelInterfaceNumber.Size = new System.Drawing.Size(121, 21);
            this.labelInterfaceNumber.TabIndex = 13;
            this.labelInterfaceNumber.Text = "0";
            this.labelInterfaceNumber.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            // 
            // comboBoxProtocol
            // 
            this.comboBoxProtocol.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.comboBoxProtocol.FormattingEnabled = true;
            this.comboBoxProtocol.Items.AddRange(new object[] {
            "None",
            "Keyboard",
            "Mouse"});
            this.comboBoxProtocol.Location = new System.Drawing.Point(119, 148);
            this.comboBoxProtocol.Name = "comboBoxProtocol";
            this.comboBoxProtocol.Size = new System.Drawing.Size(121, 21);
            this.comboBoxProtocol.TabIndex = 6;
            this.comboBoxProtocol.SelectedIndexChanged += new System.EventHandler(this.comboBoxProtocol_SelectedIndexChanged);
            // 
            // labelProtocol
            // 
            this.labelProtocol.AutoSize = true;
            this.labelProtocol.Location = new System.Drawing.Point(6, 152);
            this.labelProtocol.Name = "labelProtocol";
            this.labelProtocol.Size = new System.Drawing.Size(46, 13);
            this.labelProtocol.TabIndex = 9;
            this.labelProtocol.Text = "Protocol";
            // 
            // comboBoxInterfaceString
            // 
            this.comboBoxInterfaceString.FormattingEnabled = true;
            this.comboBoxInterfaceString.Location = new System.Drawing.Point(119, 19);
            this.comboBoxInterfaceString.Name = "comboBoxInterfaceString";
            this.comboBoxInterfaceString.Size = new System.Drawing.Size(121, 21);
            this.comboBoxInterfaceString.TabIndex = 1;
            this.comboBoxInterfaceString.Validated += new System.EventHandler(this.comboBoxInterfaceString_Validated);
            // 
            // label5
            // 
            this.label5.AutoSize = true;
            this.label5.Location = new System.Drawing.Point(6, 74);
            this.label5.Name = "label5";
            this.label5.Size = new System.Drawing.Size(90, 13);
            this.label5.TabIndex = 12;
            this.label5.Text = "Alternate Settings";
            // 
            // comboBoxSubclass
            // 
            this.comboBoxSubclass.AutoCompleteSource = System.Windows.Forms.AutoCompleteSource.ListItems;
            this.comboBoxSubclass.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.comboBoxSubclass.DropDownWidth = 170;
            this.comboBoxSubclass.FormattingEnabled = true;
            this.comboBoxSubclass.Items.AddRange(new object[] {
            "No subclass"});
            this.comboBoxSubclass.Location = new System.Drawing.Point(119, 122);
            this.comboBoxSubclass.Name = "comboBoxSubclass";
            this.comboBoxSubclass.Size = new System.Drawing.Size(121, 21);
            this.comboBoxSubclass.TabIndex = 5;
            this.comboBoxSubclass.SelectedIndexChanged += new System.EventHandler(this.comboBoxSubclass_SelectedIndexChanged);
            // 
            // label9
            // 
            this.label9.AutoSize = true;
            this.label9.Location = new System.Drawing.Point(6, 48);
            this.label9.Name = "label9";
            this.label9.Size = new System.Drawing.Size(89, 13);
            this.label9.TabIndex = 11;
            this.label9.Text = "Interface Number";
            // 
            // comboBoxClass
            // 
            this.comboBoxClass.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.comboBoxClass.FormattingEnabled = true;
            this.comboBoxClass.Items.AddRange(new object[] {
            "Undefined",
            "HID",
            "Vendor-Specific"});
            this.comboBoxClass.Location = new System.Drawing.Point(119, 96);
            this.comboBoxClass.Name = "comboBoxClass";
            this.comboBoxClass.Size = new System.Drawing.Size(121, 21);
            this.comboBoxClass.TabIndex = 4;
            this.comboBoxClass.SelectedIndexChanged += new System.EventHandler(this.comboBoxClass_SelectedIndexChanged);
            // 
            // label4
            // 
            this.label4.AutoSize = true;
            this.label4.Location = new System.Drawing.Point(6, 126);
            this.label4.Name = "label4";
            this.label4.Size = new System.Drawing.Size(50, 13);
            this.label4.TabIndex = 3;
            this.label4.Text = "Subclass";
            // 
            // label3
            // 
            this.label3.AutoSize = true;
            this.label3.Location = new System.Drawing.Point(6, 100);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(32, 13);
            this.label3.TabIndex = 2;
            this.label3.Text = "Class";
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(6, 22);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(79, 13);
            this.label1.TabIndex = 0;
            this.label1.Text = "Interface String";
            // 
            // CyDetailsInterface
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.Controls.Add(this.groupBoxParams);
            this.Name = "CyDetailsInterface";
            this.Size = new System.Drawing.Size(312, 200);
            this.groupBoxParams.ResumeLayout(false);
            this.groupBoxParams.PerformLayout();
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.GroupBox groupBoxParams;
        private System.Windows.Forms.ComboBox comboBoxSubclass;
        private System.Windows.Forms.ComboBox comboBoxClass;
        private System.Windows.Forms.Label label4;
        private System.Windows.Forms.Label label3;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.ComboBox comboBoxInterfaceString;
        private System.Windows.Forms.ComboBox comboBoxProtocol;
        private System.Windows.Forms.Label labelProtocol;
        private System.Windows.Forms.Label label5;
        private System.Windows.Forms.Label label9;
        private System.Windows.Forms.Label labelInterfaceNumber;
        private System.Windows.Forms.Label labelAlternateSettings;
    }
}
