namespace CyCustomizer.LUT_v1_0
{
    partial class CyBitfieldControl
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
            this.m_dgvBitFieldConfigure = new System.Windows.Forms.DataGridView();
            this.m_bClearAll = new System.Windows.Forms.Button();
            this.m_bSetAll = new System.Windows.Forms.Button();
            this.m_nudInputs = new System.Windows.Forms.NumericUpDown();
            this.m_lblInputs = new System.Windows.Forms.Label();
            this.m_lblOutputs = new System.Windows.Forms.Label();
            this.m_nudOutputs = new System.Windows.Forms.NumericUpDown();
            this.m_chbxRegisterOutputs = new System.Windows.Forms.CheckBox();
            ((System.ComponentModel.ISupportInitialize)(this.m_dgvBitFieldConfigure)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.m_nudInputs)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.m_nudOutputs)).BeginInit();
            this.SuspendLayout();
            // 
            // m_dgvBitFieldConfigure
            // 
            this.m_dgvBitFieldConfigure.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom)
                        | System.Windows.Forms.AnchorStyles.Left)
                        | System.Windows.Forms.AnchorStyles.Right)));
            this.m_dgvBitFieldConfigure.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            this.m_dgvBitFieldConfigure.Location = new System.Drawing.Point(3, 40);
            this.m_dgvBitFieldConfigure.Name = "m_dgvBitFieldConfigure";
            this.m_dgvBitFieldConfigure.RowTemplate.Height = 24;
            this.m_dgvBitFieldConfigure.Size = new System.Drawing.Size(375, 261);
            this.m_dgvBitFieldConfigure.TabIndex = 0;
            this.m_dgvBitFieldConfigure.CellContentClick += new System.Windows.Forms.DataGridViewCellEventHandler(this.m_dgvBitFieldConfigure_CellContentClick);
            this.m_dgvBitFieldConfigure.CellEndEdit += new System.Windows.Forms.DataGridViewCellEventHandler(this.m_dgvBitFieldConfigure_CellEndEdit);
            // 
            // m_bClearAll
            // 
            this.m_bClearAll.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
            this.m_bClearAll.Location = new System.Drawing.Point(303, 306);
            this.m_bClearAll.Name = "m_bClearAll";
            this.m_bClearAll.Size = new System.Drawing.Size(75, 23);
            this.m_bClearAll.TabIndex = 1;
            this.m_bClearAll.Text = "Clear All";
            this.m_bClearAll.UseVisualStyleBackColor = true;
            this.m_bClearAll.Click += new System.EventHandler(this.m_bClearAll_Click);
            // 
            // m_bSetAll
            // 
            this.m_bSetAll.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
            this.m_bSetAll.Location = new System.Drawing.Point(222, 306);
            this.m_bSetAll.Name = "m_bSetAll";
            this.m_bSetAll.Size = new System.Drawing.Size(75, 23);
            this.m_bSetAll.TabIndex = 2;
            this.m_bSetAll.Text = "Set All";
            this.m_bSetAll.UseVisualStyleBackColor = true;
            this.m_bSetAll.Click += new System.EventHandler(this.m_bSetAll_Click);
            // 
            // m_nudInputs
            // 
            this.m_nudInputs.BackColor = System.Drawing.Color.White;
            this.m_nudInputs.Location = new System.Drawing.Point(3, 18);
            this.m_nudInputs.Margin = new System.Windows.Forms.Padding(2);
            this.m_nudInputs.Maximum = new decimal(new int[] {
            5,
            0,
            0,
            0});
            this.m_nudInputs.Minimum = new decimal(new int[] {
            1,
            0,
            0,
            0});
            this.m_nudInputs.Name = "m_nudInputs";
            this.m_nudInputs.ReadOnly = true;
            this.m_nudInputs.Size = new System.Drawing.Size(34, 20);
            this.m_nudInputs.TabIndex = 3;
            this.m_nudInputs.Value = new decimal(new int[] {
            1,
            0,
            0,
            0});
            this.m_nudInputs.ValueChanged += new System.EventHandler(this.m_nudInputs_ValueChanged);
            // 
            // m_lblInputs
            // 
            this.m_lblInputs.AutoSize = true;
            this.m_lblInputs.Location = new System.Drawing.Point(2, 2);
            this.m_lblInputs.Margin = new System.Windows.Forms.Padding(2, 0, 2, 0);
            this.m_lblInputs.Name = "m_lblInputs";
            this.m_lblInputs.Size = new System.Drawing.Size(36, 13);
            this.m_lblInputs.TabIndex = 4;
            this.m_lblInputs.Text = "Inputs";
            // 
            // m_lblOutputs
            // 
            this.m_lblOutputs.AutoSize = true;
            this.m_lblOutputs.Location = new System.Drawing.Point(47, 2);
            this.m_lblOutputs.Margin = new System.Windows.Forms.Padding(2, 0, 2, 0);
            this.m_lblOutputs.Name = "m_lblOutputs";
            this.m_lblOutputs.Size = new System.Drawing.Size(44, 13);
            this.m_lblOutputs.TabIndex = 6;
            this.m_lblOutputs.Text = "Outputs";
            // 
            // m_nudOutputs
            // 
            this.m_nudOutputs.BackColor = System.Drawing.Color.White;
            this.m_nudOutputs.Location = new System.Drawing.Point(52, 18);
            this.m_nudOutputs.Margin = new System.Windows.Forms.Padding(2);
            this.m_nudOutputs.Maximum = new decimal(new int[] {
            8,
            0,
            0,
            0});
            this.m_nudOutputs.Minimum = new decimal(new int[] {
            1,
            0,
            0,
            0});
            this.m_nudOutputs.Name = "m_nudOutputs";
            this.m_nudOutputs.ReadOnly = true;
            this.m_nudOutputs.Size = new System.Drawing.Size(34, 20);
            this.m_nudOutputs.TabIndex = 5;
            this.m_nudOutputs.Value = new decimal(new int[] {
            1,
            0,
            0,
            0});
            this.m_nudOutputs.ValueChanged += new System.EventHandler(this.m_nudOutputs_ValueChanged);
            // 
            // m_chbxRegisterOutputs
            // 
            this.m_chbxRegisterOutputs.AutoSize = true;
            this.m_chbxRegisterOutputs.Location = new System.Drawing.Point(104, 19);
            this.m_chbxRegisterOutputs.Margin = new System.Windows.Forms.Padding(2);
            this.m_chbxRegisterOutputs.Name = "m_chbxRegisterOutputs";
            this.m_chbxRegisterOutputs.Size = new System.Drawing.Size(105, 17);
            this.m_chbxRegisterOutputs.TabIndex = 7;
            this.m_chbxRegisterOutputs.Text = "Register Outputs";
            this.m_chbxRegisterOutputs.UseVisualStyleBackColor = true;
            this.m_chbxRegisterOutputs.CheckedChanged += new System.EventHandler(this.m_chbxRegisterOutputs_CheckedChanged);
            // 
            // CyBitfieldControl
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.Controls.Add(this.m_chbxRegisterOutputs);
            this.Controls.Add(this.m_lblOutputs);
            this.Controls.Add(this.m_nudOutputs);
            this.Controls.Add(this.m_lblInputs);
            this.Controls.Add(this.m_nudInputs);
            this.Controls.Add(this.m_bSetAll);
            this.Controls.Add(this.m_bClearAll);
            this.Controls.Add(this.m_dgvBitFieldConfigure);
            this.Name = "CyBitfieldControl";
            this.Size = new System.Drawing.Size(381, 329);
            this.VisibleChanged += new System.EventHandler(this.CyBitfieldControl_VisibleChanged);
            this.Leave += new System.EventHandler(this.CyBitfieldControl_Leave);
            ((System.ComponentModel.ISupportInitialize)(this.m_dgvBitFieldConfigure)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.m_nudInputs)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.m_nudOutputs)).EndInit();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.DataGridView m_dgvBitFieldConfigure;
        private System.Windows.Forms.Button m_bClearAll;
        private System.Windows.Forms.Button m_bSetAll;
        private System.Windows.Forms.NumericUpDown m_nudInputs;
        private System.Windows.Forms.Label m_lblInputs;
        private System.Windows.Forms.Label m_lblOutputs;
        private System.Windows.Forms.NumericUpDown m_nudOutputs;
        private System.Windows.Forms.CheckBox m_chbxRegisterOutputs;
    }
}
