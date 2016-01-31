namespace USBFS_v0_2
{
    partial class CyDetailsAudio
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
            this.numUpDownLength = new System.Windows.Forms.NumericUpDown();
            this.labelLength = new System.Windows.Forms.Label();
            this.groupBoxEdits = new System.Windows.Forms.GroupBox();
            this.buttonApply = new System.Windows.Forms.Button();
            this.labelSubtype = new System.Windows.Forms.Label();
            this.comboBoxSybtype = new System.Windows.Forms.ComboBox();
            this.groupBoxSybtype = new System.Windows.Forms.GroupBox();
            this.labelSubclass = new System.Windows.Forms.Label();
            this.labelSubclassTitle = new System.Windows.Forms.Label();
            ((System.ComponentModel.ISupportInitialize)(this.numUpDownLength)).BeginInit();
            this.groupBoxSybtype.SuspendLayout();
            this.SuspendLayout();
            // 
            // numUpDownLength
            // 
            this.numUpDownLength.Location = new System.Drawing.Point(54, 93);
            this.numUpDownLength.Name = "numUpDownLength";
            this.numUpDownLength.Size = new System.Drawing.Size(58, 20);
            this.numUpDownLength.TabIndex = 0;
            this.numUpDownLength.Value = new decimal(new int[] {
            10,
            0,
            0,
            0});
            this.numUpDownLength.ValueChanged += new System.EventHandler(this.numUpDownLength_ValueChanged);
            // 
            // labelLength
            // 
            this.labelLength.AutoSize = true;
            this.labelLength.Location = new System.Drawing.Point(8, 95);
            this.labelLength.Name = "labelLength";
            this.labelLength.Size = new System.Drawing.Size(40, 13);
            this.labelLength.TabIndex = 1;
            this.labelLength.Text = "Length";
            // 
            // groupBoxEdits
            // 
            this.groupBoxEdits.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left)
                        | System.Windows.Forms.AnchorStyles.Right)));
            this.groupBoxEdits.Location = new System.Drawing.Point(3, 119);
            this.groupBoxEdits.Name = "groupBoxEdits";
            this.groupBoxEdits.Size = new System.Drawing.Size(281, 129);
            this.groupBoxEdits.TabIndex = 2;
            this.groupBoxEdits.TabStop = false;
            // 
            // buttonApply
            // 
            this.buttonApply.Location = new System.Drawing.Point(117, 255);
            this.buttonApply.Name = "buttonApply";
            this.buttonApply.Size = new System.Drawing.Size(75, 23);
            this.buttonApply.TabIndex = 6;
            this.buttonApply.Text = "Apply";
            this.buttonApply.UseVisualStyleBackColor = true;
            this.buttonApply.Visible = false;
            this.buttonApply.Click += new System.EventHandler(this.buttonApply_Click);
            // 
            // labelSubtype
            // 
            this.labelSubtype.AutoSize = true;
            this.labelSubtype.Location = new System.Drawing.Point(5, 42);
            this.labelSubtype.Name = "labelSubtype";
            this.labelSubtype.Size = new System.Drawing.Size(97, 13);
            this.labelSubtype.TabIndex = 7;
            this.labelSubtype.Text = "Descriptor Subtype";
            // 
            // comboBoxSybtype
            // 
            this.comboBoxSybtype.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left)
                        | System.Windows.Forms.AnchorStyles.Right)));
            this.comboBoxSybtype.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.comboBoxSybtype.FormattingEnabled = true;
            this.comboBoxSybtype.Location = new System.Drawing.Point(136, 39);
            this.comboBoxSybtype.MaximumSize = new System.Drawing.Size(140, 0);
            this.comboBoxSybtype.Name = "comboBoxSybtype";
            this.comboBoxSybtype.Size = new System.Drawing.Size(139, 21);
            this.comboBoxSybtype.TabIndex = 8;
            this.comboBoxSybtype.SelectedIndexChanged += new System.EventHandler(this.comboBoxSybtype_SelectedIndexChanged);
            // 
            // groupBoxSybtype
            // 
            this.groupBoxSybtype.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left)
                        | System.Windows.Forms.AnchorStyles.Right)));
            this.groupBoxSybtype.Controls.Add(this.labelSubclass);
            this.groupBoxSybtype.Controls.Add(this.labelSubclassTitle);
            this.groupBoxSybtype.Controls.Add(this.comboBoxSybtype);
            this.groupBoxSybtype.Controls.Add(this.labelSubtype);
            this.groupBoxSybtype.Location = new System.Drawing.Point(3, 3);
            this.groupBoxSybtype.Name = "groupBoxSybtype";
            this.groupBoxSybtype.Size = new System.Drawing.Size(281, 71);
            this.groupBoxSybtype.TabIndex = 9;
            this.groupBoxSybtype.TabStop = false;
            // 
            // labelSubclass
            // 
            this.labelSubclass.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left)
                        | System.Windows.Forms.AnchorStyles.Right)));
            this.labelSubclass.BorderStyle = System.Windows.Forms.BorderStyle.Fixed3D;
            this.labelSubclass.Location = new System.Drawing.Point(136, 12);
            this.labelSubclass.MaximumSize = new System.Drawing.Size(140, 21);
            this.labelSubclass.Name = "labelSubclass";
            this.labelSubclass.Size = new System.Drawing.Size(139, 21);
            this.labelSubclass.TabIndex = 10;
            this.labelSubclass.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            // 
            // labelSubclassTitle
            // 
            this.labelSubclassTitle.AutoSize = true;
            this.labelSubclassTitle.Location = new System.Drawing.Point(5, 16);
            this.labelSubclassTitle.Name = "labelSubclassTitle";
            this.labelSubclassTitle.Size = new System.Drawing.Size(125, 13);
            this.labelSubclassTitle.TabIndex = 9;
            this.labelSubclassTitle.Text = "Audio Interface Subclass";
            // 
            // CyDetailsAudio
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.AutoScroll = true;
            this.Controls.Add(this.groupBoxSybtype);
            this.Controls.Add(this.buttonApply);
            this.Controls.Add(this.groupBoxEdits);
            this.Controls.Add(this.labelLength);
            this.Controls.Add(this.numUpDownLength);
            this.Name = "CyDetailsAudio";
            this.Size = new System.Drawing.Size(287, 279);
            this.Leave += new System.EventHandler(this.buttonApply_Click);
            this.SizeChanged += new System.EventHandler(this.CyDetailsAudio_SizeChanged);
            ((System.ComponentModel.ISupportInitialize)(this.numUpDownLength)).EndInit();
            this.groupBoxSybtype.ResumeLayout(false);
            this.groupBoxSybtype.PerformLayout();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.NumericUpDown numUpDownLength;
        private System.Windows.Forms.Label labelLength;
        private System.Windows.Forms.GroupBox groupBoxEdits;
        private System.Windows.Forms.Button buttonApply;
        private System.Windows.Forms.Label labelSubtype;
        private System.Windows.Forms.ComboBox comboBoxSybtype;
        private System.Windows.Forms.GroupBox groupBoxSybtype;
        private System.Windows.Forms.Label labelSubclass;
        private System.Windows.Forms.Label labelSubclassTitle;
    }
}
