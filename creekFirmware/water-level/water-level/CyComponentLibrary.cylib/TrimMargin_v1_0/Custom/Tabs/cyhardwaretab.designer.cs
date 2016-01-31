namespace TrimMargin_v1_0
{
    partial class CyHardwareTab
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
            System.Windows.Forms.DataGridViewCellStyle dataGridViewCellStyle1 = new System.Windows.Forms.DataGridViewCellStyle();
            System.Windows.Forms.DataGridViewCellStyle dataGridViewCellStyle2 = new System.Windows.Forms.DataGridViewCellStyle();
            System.Windows.Forms.DataGridViewCellStyle dataGridViewCellStyle3 = new System.Windows.Forms.DataGridViewCellStyle();
            System.Windows.Forms.DataGridViewCellStyle dataGridViewCellStyle4 = new System.Windows.Forms.DataGridViewCellStyle();
            System.Windows.Forms.DataGridViewCellStyle dataGridViewCellStyle5 = new System.Windows.Forms.DataGridViewCellStyle();
            System.Windows.Forms.DataGridViewCellStyle dataGridViewCellStyle6 = new System.Windows.Forms.DataGridViewCellStyle();
            System.Windows.Forms.DataGridViewCellStyle dataGridViewCellStyle7 = new System.Windows.Forms.DataGridViewCellStyle();
            System.Windows.Forms.DataGridViewCellStyle dataGridViewCellStyle8 = new System.Windows.Forms.DataGridViewCellStyle();
            System.Windows.Forms.DataGridViewCellStyle dataGridViewCellStyle9 = new System.Windows.Forms.DataGridViewCellStyle();
            System.Windows.Forms.DataGridViewCellStyle dataGridViewCellStyle10 = new System.Windows.Forms.DataGridViewCellStyle();
            System.Windows.Forms.DataGridViewCellStyle dataGridViewCellStyle11 = new System.Windows.Forms.DataGridViewCellStyle();
            System.Windows.Forms.DataGridViewCellStyle dataGridViewCellStyle12 = new System.Windows.Forms.DataGridViewCellStyle();
            System.Windows.Forms.DataGridViewCellStyle dataGridViewCellStyle13 = new System.Windows.Forms.DataGridViewCellStyle();
            System.Windows.Forms.DataGridViewCellStyle dataGridViewCellStyle14 = new System.Windows.Forms.DataGridViewCellStyle();
            this.dgvHardware = new System.Windows.Forms.DataGridView();
            this.cyToolStrip1 = new TrimMargin_v1_0.CyToolStrip();
            this.pictureBox1 = new System.Windows.Forms.PictureBox();
            this.panelGraph = new System.Windows.Forms.Panel();
            this.numPWMResolution = new System.Windows.Forms.NumericUpDown();
            this.labelPWMResolution = new System.Windows.Forms.Label();
            this.colConverterOutput = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.colConverterName = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.colNominalVoltage = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.colPolarity = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.colVddio = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.colControlVoltage = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.colR1 = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.colR2 = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.colCalcR3 = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.colR3 = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.colCalcR4 = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.colMaxRipple = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.colCalcC1 = new System.Windows.Forms.DataGridViewTextBoxColumn();
            ((System.ComponentModel.ISupportInitialize)(this.dgvHardware)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.pictureBox1)).BeginInit();
            this.panelGraph.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.numPWMResolution)).BeginInit();
            this.SuspendLayout();
            // 
            // dgvHardware
            // 
            this.dgvHardware.AllowUserToAddRows = false;
            this.dgvHardware.AllowUserToDeleteRows = false;
            this.dgvHardware.AutoSizeColumnsMode = System.Windows.Forms.DataGridViewAutoSizeColumnsMode.Fill;
            dataGridViewCellStyle1.Alignment = System.Windows.Forms.DataGridViewContentAlignment.MiddleCenter;
            dataGridViewCellStyle1.BackColor = System.Drawing.SystemColors.Control;
            dataGridViewCellStyle1.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(204)));
            dataGridViewCellStyle1.ForeColor = System.Drawing.SystemColors.WindowText;
            dataGridViewCellStyle1.SelectionBackColor = System.Drawing.SystemColors.Highlight;
            dataGridViewCellStyle1.SelectionForeColor = System.Drawing.SystemColors.HighlightText;
            dataGridViewCellStyle1.WrapMode = System.Windows.Forms.DataGridViewTriState.True;
            this.dgvHardware.ColumnHeadersDefaultCellStyle = dataGridViewCellStyle1;
            this.dgvHardware.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            this.dgvHardware.Columns.AddRange(new System.Windows.Forms.DataGridViewColumn[] {
            this.colConverterOutput,
            this.colConverterName,
            this.colNominalVoltage,
            this.colPolarity,
            this.colVddio,
            this.colControlVoltage,
            this.colR1,
            this.colR2,
            this.colCalcR3,
            this.colR3,
            this.colCalcR4,
            this.colMaxRipple,
            this.colCalcC1});
            this.dgvHardware.Location = new System.Drawing.Point(0, 58);
            this.dgvHardware.Name = "dgvHardware";
            this.dgvHardware.RowHeadersWidth = 20;
            this.dgvHardware.RowTemplate.Height = 24;
            this.dgvHardware.ScrollBars = System.Windows.Forms.ScrollBars.Vertical;
            this.dgvHardware.Size = new System.Drawing.Size(866, 317);
            this.dgvHardware.TabIndex = 0;
            this.dgvHardware.CellValueChanged += new System.Windows.Forms.DataGridViewCellEventHandler(this.dgvHardware_CellValueChanged);
            this.dgvHardware.CurrentCellDirtyStateChanged += new System.EventHandler(this.dgvHardware_CurrentCellDirtyStateChanged);
            this.dgvHardware.SelectionChanged += new System.EventHandler(this.dgvHardware_SelectionChanged);
            // 
            // cyToolStrip1
            // 
            this.cyToolStrip1.Dock = System.Windows.Forms.DockStyle.Top;
            this.cyToolStrip1.Location = new System.Drawing.Point(0, 0);
            this.cyToolStrip1.Margin = new System.Windows.Forms.Padding(4, 4, 4, 4);
            this.cyToolStrip1.Name = "cyToolStrip1";
            this.cyToolStrip1.Size = new System.Drawing.Size(1077, 25);
            this.cyToolStrip1.TabIndex = 1;
            // 
            // pictureBox1
            // 
            this.pictureBox1.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom)
                        | System.Windows.Forms.AnchorStyles.Left)));
            this.pictureBox1.Location = new System.Drawing.Point(0, 0);
            this.pictureBox1.Name = "pictureBox1";
            this.pictureBox1.Size = new System.Drawing.Size(215, 191);
            this.pictureBox1.TabIndex = 2;
            this.pictureBox1.TabStop = false;
            // 
            // panelGraph
            // 
            this.panelGraph.BackColor = System.Drawing.Color.White;
            this.panelGraph.Controls.Add(this.pictureBox1);
            this.panelGraph.Location = new System.Drawing.Point(0, 374);
            this.panelGraph.Name = "panelGraph";
            this.panelGraph.Size = new System.Drawing.Size(215, 191);
            this.panelGraph.TabIndex = 3;
            // 
            // numPWMResolution
            // 
            this.numPWMResolution.Location = new System.Drawing.Point(120, 32);
            this.numPWMResolution.Maximum = new decimal(new int[] {
            16,
            0,
            0,
            0});
            this.numPWMResolution.Minimum = new decimal(new int[] {
            8,
            0,
            0,
            0});
            this.numPWMResolution.Name = "numPWMResolution";
            this.numPWMResolution.Size = new System.Drawing.Size(60, 22);
            this.numPWMResolution.TabIndex = 22;
            this.numPWMResolution.Value = new decimal(new int[] {
            8,
            0,
            0,
            0});
            // 
            // labelPWMResolution
            // 
            this.labelPWMResolution.AutoSize = true;
            this.labelPWMResolution.Location = new System.Drawing.Point(3, 34);
            this.labelPWMResolution.Name = "labelPWMResolution";
            this.labelPWMResolution.Size = new System.Drawing.Size(111, 17);
            this.labelPWMResolution.TabIndex = 21;
            this.labelPWMResolution.Text = "PWM resolution:";
            // 
            // colConverterOutput
            // 
            this.colConverterOutput.AutoSizeMode = System.Windows.Forms.DataGridViewAutoSizeColumnMode.None;
            dataGridViewCellStyle2.BackColor = System.Drawing.SystemColors.ControlLight;
            this.colConverterOutput.DefaultCellStyle = dataGridViewCellStyle2;
            this.colConverterOutput.HeaderText = "Power converter number";
            this.colConverterOutput.Name = "colConverterOutput";
            this.colConverterOutput.ReadOnly = true;
            this.colConverterOutput.Width = 69;
            // 
            // colConverterName
            // 
            this.colConverterName.AutoSizeMode = System.Windows.Forms.DataGridViewAutoSizeColumnMode.None;
            dataGridViewCellStyle3.BackColor = System.Drawing.SystemColors.ControlLight;
            this.colConverterName.DefaultCellStyle = dataGridViewCellStyle3;
            this.colConverterName.HeaderText = "Power converter name";
            this.colConverterName.MaxInputLength = 16;
            this.colConverterName.Name = "colConverterName";
            this.colConverterName.ReadOnly = true;
            this.colConverterName.Width = 105;
            // 
            // colNominalVoltage
            // 
            this.colNominalVoltage.AutoSizeMode = System.Windows.Forms.DataGridViewAutoSizeColumnMode.None;
            dataGridViewCellStyle4.BackColor = System.Drawing.SystemColors.ControlLight;
            dataGridViewCellStyle4.Format = "N2";
            dataGridViewCellStyle4.NullValue = null;
            this.colNominalVoltage.DefaultCellStyle = dataGridViewCellStyle4;
            this.colNominalVoltage.HeaderText = "Nominal voltage (V)";
            this.colNominalVoltage.Name = "colNominalVoltage";
            this.colNominalVoltage.ReadOnly = true;
            this.colNominalVoltage.Width = 65;
            // 
            // colPolarity
            // 
            this.colPolarity.AutoSizeMode = System.Windows.Forms.DataGridViewAutoSizeColumnMode.None;
            dataGridViewCellStyle5.BackColor = System.Drawing.SystemColors.ControlLight;
            this.colPolarity.DefaultCellStyle = dataGridViewCellStyle5;
            this.colPolarity.HeaderText = "Polarity";
            this.colPolarity.Name = "colPolarity";
            this.colPolarity.ReadOnly = true;
            this.colPolarity.Resizable = System.Windows.Forms.DataGridViewTriState.True;
            this.colPolarity.Visible = false;
            this.colPolarity.Width = 80;
            // 
            // colVddio
            // 
            this.colVddio.AutoSizeMode = System.Windows.Forms.DataGridViewAutoSizeColumnMode.None;
            dataGridViewCellStyle6.Format = "N2";
            this.colVddio.DefaultCellStyle = dataGridViewCellStyle6;
            this.colVddio.HeaderText = "PWM output pin Vddio (V)";
            this.colVddio.Name = "colVddio";
            this.colVddio.Width = 85;
            // 
            // colControlVoltage
            // 
            this.colControlVoltage.AutoSizeMode = System.Windows.Forms.DataGridViewAutoSizeColumnMode.None;
            dataGridViewCellStyle7.Format = "N3";
            dataGridViewCellStyle7.NullValue = null;
            this.colControlVoltage.DefaultCellStyle = dataGridViewCellStyle7;
            this.colControlVoltage.HeaderText = "Vadj voltage at Vnom (V)";
            this.colControlVoltage.Name = "colControlVoltage";
            this.colControlVoltage.Width = 75;
            // 
            // colR1
            // 
            this.colR1.AutoSizeMode = System.Windows.Forms.DataGridViewAutoSizeColumnMode.None;
            dataGridViewCellStyle8.Format = "N2";
            this.colR1.DefaultCellStyle = dataGridViewCellStyle8;
            this.colR1.HeaderText = "R1 (kOhm)";
            this.colR1.Name = "colR1";
            this.colR1.Width = 53;
            // 
            // colR2
            // 
            this.colR2.AutoSizeMode = System.Windows.Forms.DataGridViewAutoSizeColumnMode.None;
            dataGridViewCellStyle9.Format = "N2";
            this.colR2.DefaultCellStyle = dataGridViewCellStyle9;
            this.colR2.HeaderText = "R2 (kOhm)";
            this.colR2.Name = "colR2";
            this.colR2.Width = 53;
            // 
            // colCalcR3
            // 
            this.colCalcR3.AutoSizeMode = System.Windows.Forms.DataGridViewAutoSizeColumnMode.None;
            dataGridViewCellStyle10.BackColor = System.Drawing.SystemColors.ControlLight;
            dataGridViewCellStyle10.Format = "N2";
            this.colCalcR3.DefaultCellStyle = dataGridViewCellStyle10;
            this.colCalcR3.HeaderText = "Calculated  R3 + R4 (kOhm)";
            this.colCalcR3.Name = "colCalcR3";
            this.colCalcR3.ReadOnly = true;
            this.colCalcR3.Width = 75;
            // 
            // colR3
            // 
            this.colR3.AutoSizeMode = System.Windows.Forms.DataGridViewAutoSizeColumnMode.None;
            dataGridViewCellStyle11.Format = "N2";
            this.colR3.DefaultCellStyle = dataGridViewCellStyle11;
            this.colR3.HeaderText = "Actual R3 (kOhm)";
            this.colR3.Name = "colR3";
            this.colR3.Width = 53;
            // 
            // colCalcR4
            // 
            this.colCalcR4.AutoSizeMode = System.Windows.Forms.DataGridViewAutoSizeColumnMode.None;
            dataGridViewCellStyle12.BackColor = System.Drawing.Color.White;
            dataGridViewCellStyle12.Format = "N2";
            this.colCalcR4.DefaultCellStyle = dataGridViewCellStyle12;
            this.colCalcR4.HeaderText = "Actual R4 (kOhm)";
            this.colCalcR4.Name = "colCalcR4";
            this.colCalcR4.Width = 53;
            // 
            // colMaxRipple
            // 
            this.colMaxRipple.AutoSizeMode = System.Windows.Forms.DataGridViewAutoSizeColumnMode.None;
            dataGridViewCellStyle13.Format = "N2";
            this.colMaxRipple.DefaultCellStyle = dataGridViewCellStyle13;
            this.colMaxRipple.HeaderText = "Max Ripple on Vadj (mV)";
            this.colMaxRipple.Name = "colMaxRipple";
            this.colMaxRipple.Width = 85;
            // 
            // colCalcC1
            // 
            this.colCalcC1.AutoSizeMode = System.Windows.Forms.DataGridViewAutoSizeColumnMode.None;
            dataGridViewCellStyle14.BackColor = System.Drawing.SystemColors.ControlLight;
            dataGridViewCellStyle14.Format = "N3";
            dataGridViewCellStyle14.NullValue = null;
            this.colCalcC1.DefaultCellStyle = dataGridViewCellStyle14;
            this.colCalcC1.HeaderText = "Calculated C1 (uF)";
            this.colCalcC1.Name = "colCalcC1";
            this.colCalcC1.ReadOnly = true;
            this.colCalcC1.Width = 75;
            // 
            // CyHardwareTab
            // 
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.None;
            this.AutoScroll = true;
            this.Controls.Add(this.numPWMResolution);
            this.Controls.Add(this.labelPWMResolution);
            this.Controls.Add(this.dgvHardware);
            this.Controls.Add(this.panelGraph);
            this.Controls.Add(this.cyToolStrip1);
            this.Name = "CyHardwareTab";
            this.Size = new System.Drawing.Size(1077, 565);
            ((System.ComponentModel.ISupportInitialize)(this.dgvHardware)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.pictureBox1)).EndInit();
            this.panelGraph.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.numPWMResolution)).EndInit();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.DataGridView dgvHardware;
        private CyToolStrip cyToolStrip1;
        private System.Windows.Forms.PictureBox pictureBox1;
        private System.Windows.Forms.Panel panelGraph;
        private System.Windows.Forms.NumericUpDown numPWMResolution;
        private System.Windows.Forms.Label labelPWMResolution;
        private System.Windows.Forms.DataGridViewTextBoxColumn colConverterOutput;
        private System.Windows.Forms.DataGridViewTextBoxColumn colConverterName;
        private System.Windows.Forms.DataGridViewTextBoxColumn colNominalVoltage;
        private System.Windows.Forms.DataGridViewTextBoxColumn colPolarity;
        private System.Windows.Forms.DataGridViewTextBoxColumn colVddio;
        private System.Windows.Forms.DataGridViewTextBoxColumn colControlVoltage;
        private System.Windows.Forms.DataGridViewTextBoxColumn colR1;
        private System.Windows.Forms.DataGridViewTextBoxColumn colR2;
        private System.Windows.Forms.DataGridViewTextBoxColumn colCalcR3;
        private System.Windows.Forms.DataGridViewTextBoxColumn colR3;
        private System.Windows.Forms.DataGridViewTextBoxColumn colCalcR4;
        private System.Windows.Forms.DataGridViewTextBoxColumn colMaxRipple;
        private System.Windows.Forms.DataGridViewTextBoxColumn colCalcC1;
    }
}
