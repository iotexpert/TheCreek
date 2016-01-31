namespace  CapSense_v1_30
{
    partial class CyScanSlotsTab
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

        #region Windows Form Designer generated code

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
            this.sCMainSS = new System.Windows.Forms.SplitContainer();
            this.sCLeftHalf = new System.Windows.Forms.SplitContainer();
            this.dgScanSlotsL = new CapSense_v1_30.CySSDataGrid();
            this.LabelHeadLeft = new System.Windows.Forms.Label();
            this.SSPropsLeft = new CapSense_v1_30.ContentControls.CySSPropertyUnit();
            this.panelPropsHeaderL = new System.Windows.Forms.Panel();
            this.cmdPromoteL = new System.Windows.Forms.Button();
            this.cmdDemoteL = new System.Windows.Forms.Button();
            this.lCSLeft = new System.Windows.Forms.Label();
            this.sCRightHalf = new System.Windows.Forms.SplitContainer();
            this.dgScanSlotsR = new CapSense_v1_30.CySSDataGrid();
            this.LabelHeadRight = new System.Windows.Forms.Label();
            this.SSPropsRight = new CapSense_v1_30.ContentControls.CySSPropertyUnit();
            this.panelPropsHeaderR = new System.Windows.Forms.Panel();
            this.lCSRight = new System.Windows.Forms.Label();
            this.cmdPromoteR = new System.Windows.Forms.Button();
            this.cmdDemoteR = new System.Windows.Forms.Button();
            this.lHeaderMain = new System.Windows.Forms.Label();
            this.sCMainSS.Panel1.SuspendLayout();
            this.sCMainSS.Panel2.SuspendLayout();
            this.sCMainSS.SuspendLayout();
            this.sCLeftHalf.Panel1.SuspendLayout();
            this.sCLeftHalf.Panel2.SuspendLayout();
            this.sCLeftHalf.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.dgScanSlotsL)).BeginInit();
            this.panelPropsHeaderL.SuspendLayout();
            this.sCRightHalf.Panel1.SuspendLayout();
            this.sCRightHalf.Panel2.SuspendLayout();
            this.sCRightHalf.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.dgScanSlotsR)).BeginInit();
            this.panelPropsHeaderR.SuspendLayout();
            this.SuspendLayout();
            // 
            // sCMainSS
            // 
            this.sCMainSS.Dock = System.Windows.Forms.DockStyle.Fill;
            this.sCMainSS.Location = new System.Drawing.Point(0, 19);
            this.sCMainSS.Name = "sCMainSS";
            // 
            // sCMainSS.Panel1
            // 
            this.sCMainSS.Panel1.Controls.Add(this.sCLeftHalf);
            // 
            // sCMainSS.Panel2
            // 
            this.sCMainSS.Panel2.Controls.Add(this.sCRightHalf);
            this.sCMainSS.Size = new System.Drawing.Size(704, 437);
            this.sCMainSS.SplitterDistance = 341;
            this.sCMainSS.TabIndex = 37;
            // 
            // sCLeftHalf
            // 
            this.sCLeftHalf.BackColor = System.Drawing.Color.Transparent;
            this.sCLeftHalf.Dock = System.Windows.Forms.DockStyle.Fill;
            this.sCLeftHalf.FixedPanel = System.Windows.Forms.FixedPanel.Panel2;
            this.sCLeftHalf.Location = new System.Drawing.Point(0, 0);
            this.sCLeftHalf.Name = "sCLeftHalf";
            this.sCLeftHalf.Orientation = System.Windows.Forms.Orientation.Horizontal;
            // 
            // sCLeftHalf.Panel1
            // 
            this.sCLeftHalf.Panel1.Controls.Add(this.dgScanSlotsL);
            this.sCLeftHalf.Panel1.Controls.Add(this.LabelHeadLeft);
            // 
            // sCLeftHalf.Panel2
            // 
            this.sCLeftHalf.Panel2.AutoScroll = true;
            this.sCLeftHalf.Panel2.BackColor = System.Drawing.Color.Transparent;
            this.sCLeftHalf.Panel2.Controls.Add(this.SSPropsLeft);
            this.sCLeftHalf.Panel2.Controls.Add(this.panelPropsHeaderL);
            this.sCLeftHalf.Size = new System.Drawing.Size(341, 437);
            this.sCLeftHalf.SplitterDistance = 247;
            this.sCLeftHalf.TabIndex = 36;
            // 
            // dgScanSlotsL
            // 
            this.dgScanSlotsL.AllowDrop = true;
            this.dgScanSlotsL.AllowUserToAddRows = false;
            dataGridViewCellStyle1.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(224)))), ((int)(((byte)(224)))), ((int)(((byte)(224)))));
            this.dgScanSlotsL.AlternatingRowsDefaultCellStyle = dataGridViewCellStyle1;
            this.dgScanSlotsL.CausesValidation = false;
            dataGridViewCellStyle2.Alignment = System.Windows.Forms.DataGridViewContentAlignment.MiddleLeft;
            dataGridViewCellStyle2.BackColor = System.Drawing.SystemColors.Control;
            dataGridViewCellStyle2.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            dataGridViewCellStyle2.ForeColor = System.Drawing.SystemColors.WindowText;
            dataGridViewCellStyle2.SelectionBackColor = System.Drawing.SystemColors.Highlight;
            dataGridViewCellStyle2.SelectionForeColor = System.Drawing.SystemColors.HighlightText;
            dataGridViewCellStyle2.WrapMode = System.Windows.Forms.DataGridViewTriState.True;
            this.dgScanSlotsL.ColumnHeadersDefaultCellStyle = dataGridViewCellStyle2;
            this.dgScanSlotsL.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            dataGridViewCellStyle3.Alignment = System.Windows.Forms.DataGridViewContentAlignment.MiddleLeft;
            dataGridViewCellStyle3.BackColor = System.Drawing.SystemColors.Window;
            dataGridViewCellStyle3.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            dataGridViewCellStyle3.ForeColor = System.Drawing.SystemColors.ControlText;
            dataGridViewCellStyle3.SelectionBackColor = System.Drawing.SystemColors.Highlight;
            dataGridViewCellStyle3.SelectionForeColor = System.Drawing.SystemColors.HighlightText;
            dataGridViewCellStyle3.WrapMode = System.Windows.Forms.DataGridViewTriState.False;
            this.dgScanSlotsL.DefaultCellStyle = dataGridViewCellStyle3;
            this.dgScanSlotsL.Dock = System.Windows.Forms.DockStyle.Fill;
            this.dgScanSlotsL.Location = new System.Drawing.Point(0, 19);
            this.dgScanSlotsL.Name = "dgScanSlotsL";
            dataGridViewCellStyle4.Alignment = System.Windows.Forms.DataGridViewContentAlignment.MiddleLeft;
            dataGridViewCellStyle4.BackColor = System.Drawing.SystemColors.Control;
            dataGridViewCellStyle4.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            dataGridViewCellStyle4.ForeColor = System.Drawing.SystemColors.WindowText;
            dataGridViewCellStyle4.NullValue = "0";
            dataGridViewCellStyle4.SelectionBackColor = System.Drawing.SystemColors.Highlight;
            dataGridViewCellStyle4.SelectionForeColor = System.Drawing.SystemColors.HighlightText;
            dataGridViewCellStyle4.WrapMode = System.Windows.Forms.DataGridViewTriState.True;
            this.dgScanSlotsL.RowHeadersDefaultCellStyle = dataGridViewCellStyle4;
            this.dgScanSlotsL.RowHeadersWidthSizeMode = System.Windows.Forms.DataGridViewRowHeadersWidthSizeMode.AutoSizeToAllHeaders;
            this.dgScanSlotsL.RowTemplate.Height = 24;
            this.dgScanSlotsL.SelectionMode = System.Windows.Forms.DataGridViewSelectionMode.FullRowSelect;
            this.dgScanSlotsL.Size = new System.Drawing.Size(341, 228);
            this.dgScanSlotsL.TabIndex = 37;
            this.dgScanSlotsL.MouseDown += new System.Windows.Forms.MouseEventHandler(this.dgScanSlots_MouseDown);
            this.dgScanSlotsL.RowEnter += new System.Windows.Forms.DataGridViewCellEventHandler(this.dgScanSlots_RowEnter);
            this.dgScanSlotsL.RowsAdded += new System.Windows.Forms.DataGridViewRowsAddedEventHandler(this.dgScanSlots_RowsAdded);
            this.dgScanSlotsL.KeyDown += new System.Windows.Forms.KeyEventHandler(this.dgScanSlots_KeyDown);
            this.dgScanSlotsL.RowsRemoved += new System.Windows.Forms.DataGridViewRowsRemovedEventHandler(this.dgScanSlots_RowsRemoved);
            // 
            // LabelHeadLeft
            // 
            this.LabelHeadLeft.BackColor = System.Drawing.Color.LightSteelBlue;
            this.LabelHeadLeft.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.LabelHeadLeft.Dock = System.Windows.Forms.DockStyle.Top;
            this.LabelHeadLeft.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.LabelHeadLeft.Location = new System.Drawing.Point(0, 0);
            this.LabelHeadLeft.Name = "LabelHeadLeft";
            this.LabelHeadLeft.Size = new System.Drawing.Size(341, 19);
            this.LabelHeadLeft.TabIndex = 38;
            this.LabelHeadLeft.Text = "Left AMUX Bus Scan Slot Assignment";
            this.LabelHeadLeft.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            // 
            // SSPropsLeft
            // 
            this.SSPropsLeft.Dock = System.Windows.Forms.DockStyle.Fill;
            this.SSPropsLeft.Location = new System.Drawing.Point(0, 25);
            this.SSPropsLeft.Margin = new System.Windows.Forms.Padding(4);
            this.SSPropsLeft.Method = CapSense_v1_30.E_CAPSENSE_MODE.CSA;
            this.SSPropsLeft.Name = "SSPropsLeft";
            this.SSPropsLeft.Size = new System.Drawing.Size(341, 161);
            this.SSPropsLeft.TabIndex = 10;
            // 
            // panelPropsHeaderL
            // 
            this.panelPropsHeaderL.Controls.Add(this.cmdPromoteL);
            this.panelPropsHeaderL.Controls.Add(this.cmdDemoteL);
            this.panelPropsHeaderL.Controls.Add(this.lCSLeft);
            this.panelPropsHeaderL.Dock = System.Windows.Forms.DockStyle.Top;
            this.panelPropsHeaderL.Location = new System.Drawing.Point(0, 0);
            this.panelPropsHeaderL.Name = "panelPropsHeaderL";
            this.panelPropsHeaderL.Size = new System.Drawing.Size(341, 25);
            this.panelPropsHeaderL.TabIndex = 9;
            // 
            // cmdPromoteL
            // 
            this.cmdPromoteL.Anchor = System.Windows.Forms.AnchorStyles.Right;
            this.cmdPromoteL.Location = new System.Drawing.Point(289, 1);
            this.cmdPromoteL.Name = "cmdPromoteL";
            this.cmdPromoteL.Size = new System.Drawing.Size(22, 21);
            this.cmdPromoteL.TabIndex = 3;
            this.cmdPromoteL.UseVisualStyleBackColor = true;
            this.cmdPromoteL.Click += new System.EventHandler(this.cmdPromoteL_Click);
            // 
            // cmdDemoteL
            // 
            this.cmdDemoteL.Anchor = System.Windows.Forms.AnchorStyles.Right;
            this.cmdDemoteL.Location = new System.Drawing.Point(313, 1);
            this.cmdDemoteL.Name = "cmdDemoteL";
            this.cmdDemoteL.Size = new System.Drawing.Size(23, 21);
            this.cmdDemoteL.TabIndex = 4;
            this.cmdDemoteL.UseVisualStyleBackColor = true;
            this.cmdDemoteL.Click += new System.EventHandler(this.cmdDemoteL_Click);
            // 
            // lCSLeft
            // 
            this.lCSLeft.AutoSize = true;
            this.lCSLeft.Location = new System.Drawing.Point(1, 3);
            this.lCSLeft.Name = "lCSLeft";
            this.lCSLeft.Size = new System.Drawing.Size(123, 13);
            this.lCSLeft.TabIndex = 17;
            this.lCSLeft.Text = "CapSense Method: CSD";
            // 
            // sCRightHalf
            // 
            this.sCRightHalf.BackColor = System.Drawing.Color.Transparent;
            this.sCRightHalf.Dock = System.Windows.Forms.DockStyle.Fill;
            this.sCRightHalf.FixedPanel = System.Windows.Forms.FixedPanel.Panel2;
            this.sCRightHalf.Location = new System.Drawing.Point(0, 0);
            this.sCRightHalf.Name = "sCRightHalf";
            this.sCRightHalf.Orientation = System.Windows.Forms.Orientation.Horizontal;
            // 
            // sCRightHalf.Panel1
            // 
            this.sCRightHalf.Panel1.Controls.Add(this.dgScanSlotsR);
            this.sCRightHalf.Panel1.Controls.Add(this.LabelHeadRight);
            // 
            // sCRightHalf.Panel2
            // 
            this.sCRightHalf.Panel2.AutoScroll = true;
            this.sCRightHalf.Panel2.BackColor = System.Drawing.Color.Transparent;
            this.sCRightHalf.Panel2.Controls.Add(this.SSPropsRight);
            this.sCRightHalf.Panel2.Controls.Add(this.panelPropsHeaderR);
            this.sCRightHalf.Size = new System.Drawing.Size(359, 437);
            this.sCRightHalf.SplitterDistance = 247;
            this.sCRightHalf.TabIndex = 36;
            // 
            // dgScanSlotsR
            // 
            this.dgScanSlotsR.AllowDrop = true;
            this.dgScanSlotsR.AllowUserToAddRows = false;
            dataGridViewCellStyle5.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(224)))), ((int)(((byte)(224)))), ((int)(((byte)(224)))));
            this.dgScanSlotsR.AlternatingRowsDefaultCellStyle = dataGridViewCellStyle5;
            this.dgScanSlotsR.CausesValidation = false;
            dataGridViewCellStyle6.Alignment = System.Windows.Forms.DataGridViewContentAlignment.MiddleLeft;
            dataGridViewCellStyle6.BackColor = System.Drawing.SystemColors.Control;
            dataGridViewCellStyle6.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            dataGridViewCellStyle6.ForeColor = System.Drawing.SystemColors.WindowText;
            dataGridViewCellStyle6.SelectionBackColor = System.Drawing.SystemColors.Highlight;
            dataGridViewCellStyle6.SelectionForeColor = System.Drawing.SystemColors.HighlightText;
            dataGridViewCellStyle6.WrapMode = System.Windows.Forms.DataGridViewTriState.True;
            this.dgScanSlotsR.ColumnHeadersDefaultCellStyle = dataGridViewCellStyle6;
            this.dgScanSlotsR.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            dataGridViewCellStyle7.Alignment = System.Windows.Forms.DataGridViewContentAlignment.MiddleLeft;
            dataGridViewCellStyle7.BackColor = System.Drawing.SystemColors.Window;
            dataGridViewCellStyle7.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            dataGridViewCellStyle7.ForeColor = System.Drawing.SystemColors.ControlText;
            dataGridViewCellStyle7.SelectionBackColor = System.Drawing.SystemColors.Highlight;
            dataGridViewCellStyle7.SelectionForeColor = System.Drawing.SystemColors.HighlightText;
            dataGridViewCellStyle7.WrapMode = System.Windows.Forms.DataGridViewTriState.False;
            this.dgScanSlotsR.DefaultCellStyle = dataGridViewCellStyle7;
            this.dgScanSlotsR.Dock = System.Windows.Forms.DockStyle.Fill;
            this.dgScanSlotsR.Location = new System.Drawing.Point(0, 19);
            this.dgScanSlotsR.Name = "dgScanSlotsR";
            dataGridViewCellStyle8.Alignment = System.Windows.Forms.DataGridViewContentAlignment.MiddleLeft;
            dataGridViewCellStyle8.BackColor = System.Drawing.SystemColors.Control;
            dataGridViewCellStyle8.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            dataGridViewCellStyle8.ForeColor = System.Drawing.SystemColors.WindowText;
            dataGridViewCellStyle8.NullValue = "0";
            dataGridViewCellStyle8.SelectionBackColor = System.Drawing.SystemColors.Highlight;
            dataGridViewCellStyle8.SelectionForeColor = System.Drawing.SystemColors.HighlightText;
            dataGridViewCellStyle8.WrapMode = System.Windows.Forms.DataGridViewTriState.True;
            this.dgScanSlotsR.RowHeadersDefaultCellStyle = dataGridViewCellStyle8;
            this.dgScanSlotsR.RowHeadersWidthSizeMode = System.Windows.Forms.DataGridViewRowHeadersWidthSizeMode.AutoSizeToAllHeaders;
            this.dgScanSlotsR.RowTemplate.Height = 24;
            this.dgScanSlotsR.SelectionMode = System.Windows.Forms.DataGridViewSelectionMode.FullRowSelect;
            this.dgScanSlotsR.Size = new System.Drawing.Size(359, 228);
            this.dgScanSlotsR.TabIndex = 37;
            this.dgScanSlotsR.MouseDown += new System.Windows.Forms.MouseEventHandler(this.dgScanSlots_MouseDown);
            this.dgScanSlotsR.RowEnter += new System.Windows.Forms.DataGridViewCellEventHandler(this.dgScanSlots_RowEnter);
            this.dgScanSlotsR.RowsAdded += new System.Windows.Forms.DataGridViewRowsAddedEventHandler(this.dgScanSlots_RowsAdded);
            this.dgScanSlotsR.KeyDown += new System.Windows.Forms.KeyEventHandler(this.dgScanSlots_KeyDown);
            this.dgScanSlotsR.RowsRemoved += new System.Windows.Forms.DataGridViewRowsRemovedEventHandler(this.dgScanSlots_RowsRemoved);
            // 
            // LabelHeadRight
            // 
            this.LabelHeadRight.BackColor = System.Drawing.Color.Khaki;
            this.LabelHeadRight.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.LabelHeadRight.Dock = System.Windows.Forms.DockStyle.Top;
            this.LabelHeadRight.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.LabelHeadRight.Location = new System.Drawing.Point(0, 0);
            this.LabelHeadRight.Name = "LabelHeadRight";
            this.LabelHeadRight.Size = new System.Drawing.Size(359, 19);
            this.LabelHeadRight.TabIndex = 38;
            this.LabelHeadRight.Text = "Right AMUX Bus Scan Slot Assignment";
            this.LabelHeadRight.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            // 
            // SSPropsRight
            // 
            this.SSPropsRight.Dock = System.Windows.Forms.DockStyle.Fill;
            this.SSPropsRight.Location = new System.Drawing.Point(0, 25);
            this.SSPropsRight.Margin = new System.Windows.Forms.Padding(4);
            this.SSPropsRight.Method = CapSense_v1_30.E_CAPSENSE_MODE.CSD;
            this.SSPropsRight.Name = "SSPropsRight";
            this.SSPropsRight.Size = new System.Drawing.Size(359, 161);
            this.SSPropsRight.TabIndex = 10;
            // 
            // panelPropsHeaderR
            // 
            this.panelPropsHeaderR.Controls.Add(this.lCSRight);
            this.panelPropsHeaderR.Controls.Add(this.cmdPromoteR);
            this.panelPropsHeaderR.Controls.Add(this.cmdDemoteR);
            this.panelPropsHeaderR.Dock = System.Windows.Forms.DockStyle.Top;
            this.panelPropsHeaderR.Location = new System.Drawing.Point(0, 0);
            this.panelPropsHeaderR.Name = "panelPropsHeaderR";
            this.panelPropsHeaderR.Size = new System.Drawing.Size(359, 25);
            this.panelPropsHeaderR.TabIndex = 9;
            // 
            // lCSRight
            // 
            this.lCSRight.AutoSize = true;
            this.lCSRight.Location = new System.Drawing.Point(1, 3);
            this.lCSRight.Name = "lCSRight";
            this.lCSRight.Size = new System.Drawing.Size(95, 13);
            this.lCSRight.TabIndex = 17;
            this.lCSRight.Text = "CapSense Method";
            // 
            // cmdPromoteR
            // 
            this.cmdPromoteR.Anchor = System.Windows.Forms.AnchorStyles.Right;
            this.cmdPromoteR.Location = new System.Drawing.Point(307, 1);
            this.cmdPromoteR.Name = "cmdPromoteR";
            this.cmdPromoteR.Size = new System.Drawing.Size(22, 21);
            this.cmdPromoteR.TabIndex = 3;
            this.cmdPromoteR.UseVisualStyleBackColor = true;
            this.cmdPromoteR.Click += new System.EventHandler(this.cmdPromoteR_Click);
            // 
            // cmdDemoteR
            // 
            this.cmdDemoteR.Anchor = System.Windows.Forms.AnchorStyles.Right;
            this.cmdDemoteR.Location = new System.Drawing.Point(331, 1);
            this.cmdDemoteR.Name = "cmdDemoteR";
            this.cmdDemoteR.Size = new System.Drawing.Size(23, 21);
            this.cmdDemoteR.TabIndex = 4;
            this.cmdDemoteR.UseVisualStyleBackColor = true;
            this.cmdDemoteR.Click += new System.EventHandler(this.cmdDemoteR_Click);
            // 
            // lHeaderMain
            // 
            this.lHeaderMain.BackColor = System.Drawing.SystemColors.ButtonShadow;
            this.lHeaderMain.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.lHeaderMain.Dock = System.Windows.Forms.DockStyle.Top;
            this.lHeaderMain.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lHeaderMain.Location = new System.Drawing.Point(0, 0);
            this.lHeaderMain.Name = "lHeaderMain";
            this.lHeaderMain.Size = new System.Drawing.Size(704, 19);
            this.lHeaderMain.TabIndex = 34;
            this.lHeaderMain.Text = "Scan Slot Assignment";
            this.lHeaderMain.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            this.lHeaderMain.Visible = false;
            // 
            // CyScanSlotsTab
            // 
            this.Controls.Add(this.sCMainSS);
            this.Controls.Add(this.lHeaderMain);
            this.Name = "CyScanSlotsTab";
            this.Size = new System.Drawing.Size(704, 456);
            this.Load += new System.EventHandler(this.CyScanSlots_Load);
            this.sCMainSS.Panel1.ResumeLayout(false);
            this.sCMainSS.Panel2.ResumeLayout(false);
            this.sCMainSS.ResumeLayout(false);
            this.sCLeftHalf.Panel1.ResumeLayout(false);
            this.sCLeftHalf.Panel2.ResumeLayout(false);
            this.sCLeftHalf.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.dgScanSlotsL)).EndInit();
            this.panelPropsHeaderL.ResumeLayout(false);
            this.panelPropsHeaderL.PerformLayout();
            this.sCRightHalf.Panel1.ResumeLayout(false);
            this.sCRightHalf.Panel2.ResumeLayout(false);
            this.sCRightHalf.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.dgScanSlotsR)).EndInit();
            this.panelPropsHeaderR.ResumeLayout(false);
            this.panelPropsHeaderR.PerformLayout();
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.SplitContainer sCLeftHalf;
        private System.Windows.Forms.Button cmdDemoteL;
        private System.Windows.Forms.Button cmdPromoteL;
        private System.Windows.Forms.Label lHeaderMain;
        private System.Windows.Forms.Panel panelPropsHeaderL;
        private System.Windows.Forms.Label lCSLeft;
        public CySSDataGrid dgScanSlotsL;
        private System.Windows.Forms.SplitContainer sCMainSS;
        private System.Windows.Forms.SplitContainer sCRightHalf;
        public CySSDataGrid dgScanSlotsR;
        private System.Windows.Forms.Panel panelPropsHeaderR;
        private System.Windows.Forms.Label lCSRight;
        private System.Windows.Forms.Button cmdPromoteR;
        private System.Windows.Forms.Button cmdDemoteR;
        private System.Windows.Forms.Label LabelHeadLeft;
        private System.Windows.Forms.Label LabelHeadRight;
        private ContentControls.CySSPropertyUnit SSPropsRight;
        private ContentControls.CySSPropertyUnit SSPropsLeft;




    }
}
