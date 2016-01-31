namespace PrISM_v2_0
{
    partial class CyPRISMControl
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
            this.textBoxResult = new System.Windows.Forms.TextBox();
            this.label2 = new System.Windows.Forms.Label();
            this.textBoxSeed = new System.Windows.Forms.TextBox();
            this.label3 = new System.Windows.Forms.Label();
            this.groupBoxPolynom = new System.Windows.Forms.GroupBox();
            this.textBoxLFSR = new System.Windows.Forms.TextBox();
            this.label5 = new System.Windows.Forms.Label();
            this.checkBoxCustom = new System.Windows.Forms.CheckBox();
            this.label1 = new System.Windows.Forms.Label();
            this.numUpDownResolution = new System.Windows.Forms.NumericUpDown();
            this.label4 = new System.Windows.Forms.Label();
            this.label6 = new System.Windows.Forms.Label();
            this.toolTipDescription = new System.Windows.Forms.ToolTip(this.components);
            this.dataGridViewTextBoxColumn1 = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.dataGridViewTextBoxColumn2 = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.dataGridViewTextBoxColumn3 = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.dataGridViewTextBoxColumn4 = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.dataGridViewTextBoxColumn5 = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.dataGridViewTextBoxColumn6 = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.dataGridViewTextBoxColumn7 = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.dataGridViewTextBoxColumn8 = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.dataGridViewTextBoxColumn9 = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.dataGridViewTextBoxColumn10 = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.dataGridViewTextBoxColumn11 = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.dataGridViewTextBoxColumn12 = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.dataGridViewTextBoxColumn13 = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.dataGridViewTextBoxColumn14 = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.dataGridViewTextBoxColumn15 = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.dataGridViewTextBoxColumn16 = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.groupBoxValues = new System.Windows.Forms.GroupBox();
            this.groupBoxPulse = new System.Windows.Forms.GroupBox();
            this.checkBoxPulseTypeHardcoded = new System.Windows.Forms.CheckBox();
            this.label8 = new System.Windows.Forms.Label();
            this.label7 = new System.Windows.Forms.Label();
            this.label10 = new System.Windows.Forms.Label();
            this.label9 = new System.Windows.Forms.Label();
            this.comboBoxPulse1 = new System.Windows.Forms.ComboBox();
            this.comboBoxPulse0 = new System.Windows.Forms.ComboBox();
            this.comboBoxCompare1 = new System.Windows.Forms.ComboBox();
            this.comboBoxCompare0 = new System.Windows.Forms.ComboBox();
            this.errorProvider = new System.Windows.Forms.ErrorProvider(this.components);
            this.groupBoxPolynom.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.numUpDownResolution)).BeginInit();
            this.groupBoxValues.SuspendLayout();
            this.groupBoxPulse.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.errorProvider)).BeginInit();
            this.SuspendLayout();
            // 
            // textBoxResult
            // 
            this.textBoxResult.ForeColor = System.Drawing.SystemColors.WindowText;
            this.textBoxResult.Location = new System.Drawing.Point(31, 83);
            this.textBoxResult.Name = "textBoxResult";
            this.textBoxResult.ReadOnly = true;
            this.textBoxResult.Size = new System.Drawing.Size(99, 20);
            this.textBoxResult.TabIndex = 3;
            this.textBoxResult.TabStop = false;
            this.textBoxResult.TextChanged += new System.EventHandler(this.textBoxResult_TextChanged);
            // 
            // label2
            // 
            this.label2.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left)
                        | System.Windows.Forms.AnchorStyles.Right)));
            this.label2.AutoSize = true;
            this.label2.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(204)));
            this.label2.Location = new System.Drawing.Point(7, 67);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(103, 13);
            this.label2.TabIndex = 3;
            this.label2.Text = "Polynomial Value";
            // 
            // textBoxSeed
            // 
            this.textBoxSeed.Location = new System.Drawing.Point(34, 19);
            this.textBoxSeed.Name = "textBoxSeed";
            this.textBoxSeed.Size = new System.Drawing.Size(99, 20);
            this.textBoxSeed.TabIndex = 4;
            this.textBoxSeed.TextChanged += new System.EventHandler(this.textBoxSeed_TextChanged);
            this.textBoxSeed.Validating += new System.ComponentModel.CancelEventHandler(this.textBoxSeed_Validating);
            // 
            // label3
            // 
            this.label3.AutoSize = true;
            this.label3.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(204)));
            this.label3.Location = new System.Drawing.Point(134, 23);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(72, 13);
            this.label3.TabIndex = 5;
            this.label3.Text = "Seed Value";
            this.label3.Visible = false;
            // 
            // groupBoxPolynom
            // 
            this.groupBoxPolynom.Controls.Add(this.textBoxLFSR);
            this.groupBoxPolynom.Controls.Add(this.label5);
            this.groupBoxPolynom.Controls.Add(this.checkBoxCustom);
            this.groupBoxPolynom.Controls.Add(this.label1);
            this.groupBoxPolynom.Controls.Add(this.numUpDownResolution);
            this.groupBoxPolynom.Controls.Add(this.label2);
            this.groupBoxPolynom.Controls.Add(this.textBoxResult);
            this.groupBoxPolynom.Controls.Add(this.label4);
            this.groupBoxPolynom.Location = new System.Drawing.Point(3, 3);
            this.groupBoxPolynom.Name = "groupBoxPolynom";
            this.groupBoxPolynom.Size = new System.Drawing.Size(206, 109);
            this.groupBoxPolynom.TabIndex = 6;
            this.groupBoxPolynom.TabStop = false;
            // 
            // textBoxLFSR
            // 
            this.textBoxLFSR.Location = new System.Drawing.Point(76, 37);
            this.textBoxLFSR.Name = "textBoxLFSR";
            this.textBoxLFSR.ReadOnly = true;
            this.textBoxLFSR.Size = new System.Drawing.Size(126, 20);
            this.textBoxLFSR.TabIndex = 2;
            this.textBoxLFSR.TabStop = false;
            this.textBoxLFSR.TextChanged += new System.EventHandler(this.textBoxLFSR_TextChanged);
            this.textBoxLFSR.Validating += new System.ComponentModel.CancelEventHandler(this.textBoxLFSR_Validating);
            // 
            // label5
            // 
            this.label5.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left)
                        | System.Windows.Forms.AnchorStyles.Right)));
            this.label5.AutoSize = true;
            this.label5.Location = new System.Drawing.Point(10, 86);
            this.label5.Name = "label5";
            this.label5.Size = new System.Drawing.Size(18, 13);
            this.label5.TabIndex = 9;
            this.label5.Text = "0x";
            // 
            // checkBoxCustom
            // 
            this.checkBoxCustom.Anchor = System.Windows.Forms.AnchorStyles.Left;
            this.checkBoxCustom.AutoSize = true;
            this.checkBoxCustom.Location = new System.Drawing.Point(143, 12);
            this.checkBoxCustom.Name = "checkBoxCustom";
            this.checkBoxCustom.Size = new System.Drawing.Size(61, 17);
            this.checkBoxCustom.TabIndex = 1;
            this.checkBoxCustom.Text = "Custom";
            this.checkBoxCustom.UseVisualStyleBackColor = true;
            this.checkBoxCustom.CheckedChanged += new System.EventHandler(this.checkBoxCustom_CheckedChanged);
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(204)));
            this.label1.Location = new System.Drawing.Point(7, 40);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(34, 13);
            this.label1.TabIndex = 18;
            this.label1.Text = "LFSR";
            // 
            // numUpDownResolution
            // 
            this.numUpDownResolution.Location = new System.Drawing.Point(76, 11);
            this.numUpDownResolution.Name = "numUpDownResolution";
            this.numUpDownResolution.Size = new System.Drawing.Size(54, 20);
            this.numUpDownResolution.TabIndex = 0;
            this.numUpDownResolution.Value = new decimal(new int[] {
            8,
            0,
            0,
            0});
            this.numUpDownResolution.Validating += new System.ComponentModel.CancelEventHandler(this.numUpDownResolution_Validating);
            // 
            // label4
            // 
            this.label4.AutoSize = true;
            this.label4.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(204)));
            this.label4.Location = new System.Drawing.Point(3, 13);
            this.label4.Name = "label4";
            this.label4.Size = new System.Drawing.Size(67, 13);
            this.label4.TabIndex = 8;
            this.label4.Text = "Resolution";
            // 
            // label6
            // 
            this.label6.AutoSize = true;
            this.label6.Location = new System.Drawing.Point(12, 22);
            this.label6.Name = "label6";
            this.label6.Size = new System.Drawing.Size(18, 13);
            this.label6.TabIndex = 11;
            this.label6.Text = "0x";
            // 
            // toolTipDescription
            // 
            this.toolTipDescription.AutoPopDelay = 5000;
            this.toolTipDescription.InitialDelay = 500;
            this.toolTipDescription.ReshowDelay = 100;
            // 
            // dataGridViewTextBoxColumn1
            // 
            this.dataGridViewTextBoxColumn1.HeaderText = "Column1";
            this.dataGridViewTextBoxColumn1.Name = "dataGridViewTextBoxColumn1";
            this.dataGridViewTextBoxColumn1.ReadOnly = true;
            this.dataGridViewTextBoxColumn1.Resizable = System.Windows.Forms.DataGridViewTriState.True;
            this.dataGridViewTextBoxColumn1.SortMode = System.Windows.Forms.DataGridViewColumnSortMode.NotSortable;
            this.dataGridViewTextBoxColumn1.Width = 5;
            // 
            // dataGridViewTextBoxColumn2
            // 
            this.dataGridViewTextBoxColumn2.HeaderText = "Column16";
            this.dataGridViewTextBoxColumn2.Name = "dataGridViewTextBoxColumn2";
            this.dataGridViewTextBoxColumn2.ReadOnly = true;
            this.dataGridViewTextBoxColumn2.Width = 5;
            // 
            // dataGridViewTextBoxColumn3
            // 
            this.dataGridViewTextBoxColumn3.HeaderText = "Column5";
            this.dataGridViewTextBoxColumn3.Name = "dataGridViewTextBoxColumn3";
            this.dataGridViewTextBoxColumn3.ReadOnly = true;
            this.dataGridViewTextBoxColumn3.Width = 5;
            // 
            // dataGridViewTextBoxColumn4
            // 
            this.dataGridViewTextBoxColumn4.HeaderText = "Column6";
            this.dataGridViewTextBoxColumn4.Name = "dataGridViewTextBoxColumn4";
            this.dataGridViewTextBoxColumn4.ReadOnly = true;
            this.dataGridViewTextBoxColumn4.Width = 5;
            // 
            // dataGridViewTextBoxColumn5
            // 
            this.dataGridViewTextBoxColumn5.HeaderText = "Column7";
            this.dataGridViewTextBoxColumn5.Name = "dataGridViewTextBoxColumn5";
            this.dataGridViewTextBoxColumn5.ReadOnly = true;
            this.dataGridViewTextBoxColumn5.Width = 5;
            // 
            // dataGridViewTextBoxColumn6
            // 
            this.dataGridViewTextBoxColumn6.HeaderText = "Column8";
            this.dataGridViewTextBoxColumn6.Name = "dataGridViewTextBoxColumn6";
            this.dataGridViewTextBoxColumn6.ReadOnly = true;
            this.dataGridViewTextBoxColumn6.Width = 5;
            // 
            // dataGridViewTextBoxColumn7
            // 
            this.dataGridViewTextBoxColumn7.HeaderText = "Column9";
            this.dataGridViewTextBoxColumn7.Name = "dataGridViewTextBoxColumn7";
            this.dataGridViewTextBoxColumn7.ReadOnly = true;
            this.dataGridViewTextBoxColumn7.Width = 5;
            // 
            // dataGridViewTextBoxColumn8
            // 
            this.dataGridViewTextBoxColumn8.HeaderText = "Column10";
            this.dataGridViewTextBoxColumn8.Name = "dataGridViewTextBoxColumn8";
            this.dataGridViewTextBoxColumn8.ReadOnly = true;
            this.dataGridViewTextBoxColumn8.Width = 5;
            // 
            // dataGridViewTextBoxColumn9
            // 
            this.dataGridViewTextBoxColumn9.HeaderText = "Column11";
            this.dataGridViewTextBoxColumn9.Name = "dataGridViewTextBoxColumn9";
            this.dataGridViewTextBoxColumn9.ReadOnly = true;
            this.dataGridViewTextBoxColumn9.Width = 5;
            // 
            // dataGridViewTextBoxColumn10
            // 
            this.dataGridViewTextBoxColumn10.HeaderText = "Column12";
            this.dataGridViewTextBoxColumn10.Name = "dataGridViewTextBoxColumn10";
            this.dataGridViewTextBoxColumn10.ReadOnly = true;
            this.dataGridViewTextBoxColumn10.Width = 5;
            // 
            // dataGridViewTextBoxColumn11
            // 
            this.dataGridViewTextBoxColumn11.HeaderText = "Column13";
            this.dataGridViewTextBoxColumn11.Name = "dataGridViewTextBoxColumn11";
            this.dataGridViewTextBoxColumn11.ReadOnly = true;
            this.dataGridViewTextBoxColumn11.Width = 5;
            // 
            // dataGridViewTextBoxColumn12
            // 
            this.dataGridViewTextBoxColumn12.HeaderText = "Column14";
            this.dataGridViewTextBoxColumn12.Name = "dataGridViewTextBoxColumn12";
            this.dataGridViewTextBoxColumn12.ReadOnly = true;
            this.dataGridViewTextBoxColumn12.Width = 5;
            // 
            // dataGridViewTextBoxColumn13
            // 
            this.dataGridViewTextBoxColumn13.HeaderText = "Column15";
            this.dataGridViewTextBoxColumn13.Name = "dataGridViewTextBoxColumn13";
            this.dataGridViewTextBoxColumn13.ReadOnly = true;
            this.dataGridViewTextBoxColumn13.Width = 5;
            // 
            // dataGridViewTextBoxColumn14
            // 
            this.dataGridViewTextBoxColumn14.HeaderText = "Column2";
            this.dataGridViewTextBoxColumn14.Name = "dataGridViewTextBoxColumn14";
            this.dataGridViewTextBoxColumn14.ReadOnly = true;
            this.dataGridViewTextBoxColumn14.Width = 5;
            // 
            // dataGridViewTextBoxColumn15
            // 
            this.dataGridViewTextBoxColumn15.HeaderText = "Column3";
            this.dataGridViewTextBoxColumn15.Name = "dataGridViewTextBoxColumn15";
            this.dataGridViewTextBoxColumn15.ReadOnly = true;
            this.dataGridViewTextBoxColumn15.Width = 5;
            // 
            // dataGridViewTextBoxColumn16
            // 
            this.dataGridViewTextBoxColumn16.HeaderText = "Column4";
            this.dataGridViewTextBoxColumn16.Name = "dataGridViewTextBoxColumn16";
            this.dataGridViewTextBoxColumn16.ReadOnly = true;
            this.dataGridViewTextBoxColumn16.Width = 5;
            // 
            // groupBoxValues
            // 
            this.groupBoxValues.Controls.Add(this.label3);
            this.groupBoxValues.Controls.Add(this.label6);
            this.groupBoxValues.Controls.Add(this.textBoxSeed);
            this.groupBoxValues.Location = new System.Drawing.Point(3, 118);
            this.groupBoxValues.Name = "groupBoxValues";
            this.groupBoxValues.Size = new System.Drawing.Size(206, 46);
            this.groupBoxValues.TabIndex = 12;
            this.groupBoxValues.TabStop = false;
            this.groupBoxValues.Text = "Seed value";
            // 
            // groupBoxPulse
            // 
            this.groupBoxPulse.Controls.Add(this.checkBoxPulseTypeHardcoded);
            this.groupBoxPulse.Controls.Add(this.label8);
            this.groupBoxPulse.Controls.Add(this.label7);
            this.groupBoxPulse.Controls.Add(this.label10);
            this.groupBoxPulse.Controls.Add(this.label9);
            this.groupBoxPulse.Controls.Add(this.comboBoxPulse1);
            this.groupBoxPulse.Controls.Add(this.comboBoxPulse0);
            this.groupBoxPulse.Controls.Add(this.comboBoxCompare1);
            this.groupBoxPulse.Controls.Add(this.comboBoxCompare0);
            this.groupBoxPulse.Location = new System.Drawing.Point(215, 3);
            this.groupBoxPulse.Name = "groupBoxPulse";
            this.groupBoxPulse.Size = new System.Drawing.Size(224, 136);
            this.groupBoxPulse.TabIndex = 13;
            this.groupBoxPulse.TabStop = false;
            this.groupBoxPulse.Text = "Pulse Mode";
            // 
            // checkBoxPulseTypeHardcoded
            // 
            this.checkBoxPulseTypeHardcoded.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.checkBoxPulseTypeHardcoded.AutoSize = true;
            this.checkBoxPulseTypeHardcoded.Location = new System.Drawing.Point(6, 115);
            this.checkBoxPulseTypeHardcoded.Name = "checkBoxPulseTypeHardcoded";
            this.checkBoxPulseTypeHardcoded.Size = new System.Drawing.Size(132, 17);
            this.checkBoxPulseTypeHardcoded.TabIndex = 11;
            this.checkBoxPulseTypeHardcoded.Text = "PulseType Hardcoded";
            this.checkBoxPulseTypeHardcoded.UseVisualStyleBackColor = true;
            this.checkBoxPulseTypeHardcoded.CheckedChanged += new System.EventHandler(this.controlsValue_Changed);
            // 
            // label8
            // 
            this.label8.AutoSize = true;
            this.label8.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(204)));
            this.label8.Location = new System.Drawing.Point(96, 67);
            this.label8.Name = "label8";
            this.label8.Size = new System.Drawing.Size(63, 13);
            this.label8.TabIndex = 10;
            this.label8.Text = "PulseType1";
            // 
            // label7
            // 
            this.label7.AutoSize = true;
            this.label7.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(204)));
            this.label7.Location = new System.Drawing.Point(97, 20);
            this.label7.Name = "label7";
            this.label7.Size = new System.Drawing.Size(63, 13);
            this.label7.TabIndex = 9;
            this.label7.Text = "PulseType0";
            // 
            // label10
            // 
            this.label10.AutoSize = true;
            this.label10.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(204)));
            this.label10.Location = new System.Drawing.Point(3, 20);
            this.label10.Name = "label10";
            this.label10.Size = new System.Drawing.Size(74, 13);
            this.label10.TabIndex = 7;
            this.label10.Text = "PulseDensity0";
            // 
            // label9
            // 
            this.label9.AutoSize = true;
            this.label9.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(204)));
            this.label9.Location = new System.Drawing.Point(3, 67);
            this.label9.Name = "label9";
            this.label9.Size = new System.Drawing.Size(74, 13);
            this.label9.TabIndex = 6;
            this.label9.Text = "PulseDensity1";
            // 
            // comboBoxPulse1
            // 
            this.comboBoxPulse1.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.comboBoxPulse1.FormattingEnabled = true;
            this.comboBoxPulse1.Location = new System.Drawing.Point(6, 83);
            this.comboBoxPulse1.Name = "comboBoxPulse1";
            this.comboBoxPulse1.Size = new System.Drawing.Size(90, 21);
            this.comboBoxPulse1.TabIndex = 7;
            this.comboBoxPulse1.SelectedIndexChanged += new System.EventHandler(this.controlsValue_Changed);
            // 
            // comboBoxPulse0
            // 
            this.comboBoxPulse0.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.comboBoxPulse0.FormattingEnabled = true;
            this.comboBoxPulse0.Location = new System.Drawing.Point(6, 37);
            this.comboBoxPulse0.Name = "comboBoxPulse0";
            this.comboBoxPulse0.Size = new System.Drawing.Size(90, 21);
            this.comboBoxPulse0.TabIndex = 5;
            this.comboBoxPulse0.SelectedIndexChanged += new System.EventHandler(this.controlsValue_Changed);
            // 
            // comboBoxCompare1
            // 
            this.comboBoxCompare1.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.comboBoxCompare1.FormattingEnabled = true;
            this.comboBoxCompare1.Items.AddRange(new object[] {
            "Less Than or Equal",
            "Greater Than or Equal"});
            this.comboBoxCompare1.Location = new System.Drawing.Point(99, 83);
            this.comboBoxCompare1.Name = "comboBoxCompare1";
            this.comboBoxCompare1.Size = new System.Drawing.Size(118, 21);
            this.comboBoxCompare1.TabIndex = 8;
            this.comboBoxCompare1.SelectedIndexChanged += new System.EventHandler(this.controlsValue_Changed);
            // 
            // comboBoxCompare0
            // 
            this.comboBoxCompare0.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.comboBoxCompare0.FormattingEnabled = true;
            this.comboBoxCompare0.Items.AddRange(new object[] {
            "Less Than or Equal",
            "Greater Than or Equal"});
            this.comboBoxCompare0.Location = new System.Drawing.Point(99, 37);
            this.comboBoxCompare0.Name = "comboBoxCompare0";
            this.comboBoxCompare0.Size = new System.Drawing.Size(118, 21);
            this.comboBoxCompare0.TabIndex = 6;
            this.comboBoxCompare0.SelectedIndexChanged += new System.EventHandler(this.controlsValue_Changed);
            // 
            // errorProvider
            // 
            this.errorProvider.BlinkStyle = System.Windows.Forms.ErrorBlinkStyle.NeverBlink;
            this.errorProvider.ContainerControl = this;
            // 
            // CyPRISMControl
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.AutoScroll = true;
            this.AutoSize = true;
            this.Controls.Add(this.groupBoxPulse);
            this.Controls.Add(this.groupBoxValues);
            this.Controls.Add(this.groupBoxPolynom);
            this.Name = "CyPRISMControl";
            this.Size = new System.Drawing.Size(472, 174);
            this.groupBoxPolynom.ResumeLayout(false);
            this.groupBoxPolynom.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.numUpDownResolution)).EndInit();
            this.groupBoxValues.ResumeLayout(false);
            this.groupBoxValues.PerformLayout();
            this.groupBoxPulse.ResumeLayout(false);
            this.groupBoxPulse.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.errorProvider)).EndInit();
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.TextBox textBoxResult;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.TextBox textBoxSeed;
        private System.Windows.Forms.Label label3;
        private System.Windows.Forms.GroupBox groupBoxPolynom;
        private System.Windows.Forms.Label label4;
        private System.Windows.Forms.Label label5;
        private System.Windows.Forms.Label label6;
        private System.Windows.Forms.ToolTip toolTipDescription;
        private System.Windows.Forms.DataGridViewTextBoxColumn dataGridViewTextBoxColumn1;
        private System.Windows.Forms.DataGridViewTextBoxColumn dataGridViewTextBoxColumn2;
        private System.Windows.Forms.DataGridViewTextBoxColumn dataGridViewTextBoxColumn3;
        private System.Windows.Forms.DataGridViewTextBoxColumn dataGridViewTextBoxColumn4;
        private System.Windows.Forms.DataGridViewTextBoxColumn dataGridViewTextBoxColumn5;
        private System.Windows.Forms.DataGridViewTextBoxColumn dataGridViewTextBoxColumn6;
        private System.Windows.Forms.DataGridViewTextBoxColumn dataGridViewTextBoxColumn7;
        private System.Windows.Forms.DataGridViewTextBoxColumn dataGridViewTextBoxColumn8;
        private System.Windows.Forms.DataGridViewTextBoxColumn dataGridViewTextBoxColumn9;
        private System.Windows.Forms.DataGridViewTextBoxColumn dataGridViewTextBoxColumn10;
        private System.Windows.Forms.DataGridViewTextBoxColumn dataGridViewTextBoxColumn11;
        private System.Windows.Forms.DataGridViewTextBoxColumn dataGridViewTextBoxColumn12;
        private System.Windows.Forms.DataGridViewTextBoxColumn dataGridViewTextBoxColumn13;
        private System.Windows.Forms.DataGridViewTextBoxColumn dataGridViewTextBoxColumn14;
        private System.Windows.Forms.DataGridViewTextBoxColumn dataGridViewTextBoxColumn15;
        private System.Windows.Forms.DataGridViewTextBoxColumn dataGridViewTextBoxColumn16;
        private System.Windows.Forms.GroupBox groupBoxValues;
        private System.Windows.Forms.NumericUpDown numUpDownResolution;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.CheckBox checkBoxCustom;
        private System.Windows.Forms.TextBox textBoxLFSR;
        private System.Windows.Forms.GroupBox groupBoxPulse;
        private System.Windows.Forms.ComboBox comboBoxCompare0;
        private System.Windows.Forms.ComboBox comboBoxCompare1;
        private System.Windows.Forms.ComboBox comboBoxPulse1;
        private System.Windows.Forms.ComboBox comboBoxPulse0;
        private System.Windows.Forms.Label label10;
        private System.Windows.Forms.Label label9;
        private System.Windows.Forms.Label label7;
        private System.Windows.Forms.Label label8;
        private System.Windows.Forms.CheckBox checkBoxPulseTypeHardcoded;
        private System.Windows.Forms.ErrorProvider errorProvider;
    }
}
