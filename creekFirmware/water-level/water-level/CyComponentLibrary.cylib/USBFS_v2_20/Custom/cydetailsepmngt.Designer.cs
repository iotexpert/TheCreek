namespace USBFS_v2_20
{
    partial class CyDetailsEPMngt
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
            this.groupBoxMemoryMgmt = new System.Windows.Forms.GroupBox();
            this.groupBoxAllocation = new System.Windows.Forms.GroupBox();
            this.rbDynamicAllocation = new System.Windows.Forms.RadioButton();
            this.rbStaticAllocation = new System.Windows.Forms.RadioButton();
            this.rbDMAAutomatic = new System.Windows.Forms.RadioButton();
            this.rbDMAManual = new System.Windows.Forms.RadioButton();
            this.rbManual = new System.Windows.Forms.RadioButton();
            this.groupBoxMemoryMgmt.SuspendLayout();
            this.groupBoxAllocation.SuspendLayout();
            this.SuspendLayout();
            // 
            // groupBoxMemoryMgmt
            // 
            this.groupBoxMemoryMgmt.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left)
                        | System.Windows.Forms.AnchorStyles.Right)));
            this.groupBoxMemoryMgmt.Controls.Add(this.groupBoxAllocation);
            this.groupBoxMemoryMgmt.Controls.Add(this.rbDMAAutomatic);
            this.groupBoxMemoryMgmt.Controls.Add(this.rbDMAManual);
            this.groupBoxMemoryMgmt.Controls.Add(this.rbManual);
            this.groupBoxMemoryMgmt.Location = new System.Drawing.Point(3, 9);
            this.groupBoxMemoryMgmt.Name = "groupBoxMemoryMgmt";
            this.groupBoxMemoryMgmt.Size = new System.Drawing.Size(330, 100);
            this.groupBoxMemoryMgmt.TabIndex = 6;
            this.groupBoxMemoryMgmt.TabStop = false;
            this.groupBoxMemoryMgmt.Text = "Endpoint Memory Management";
            // 
            // groupBoxAllocation
            // 
            this.groupBoxAllocation.Controls.Add(this.rbDynamicAllocation);
            this.groupBoxAllocation.Controls.Add(this.rbStaticAllocation);
            this.groupBoxAllocation.Location = new System.Drawing.Point(193, 10);
            this.groupBoxAllocation.Name = "groupBoxAllocation";
            this.groupBoxAllocation.Size = new System.Drawing.Size(131, 61);
            this.groupBoxAllocation.TabIndex = 1;
            this.groupBoxAllocation.TabStop = false;
            // 
            // rbDynamicAllocation
            // 
            this.rbDynamicAllocation.AutoSize = true;
            this.rbDynamicAllocation.Location = new System.Drawing.Point(6, 35);
            this.rbDynamicAllocation.Name = "rbDynamicAllocation";
            this.rbDynamicAllocation.Size = new System.Drawing.Size(115, 17);
            this.rbDynamicAllocation.TabIndex = 1;
            this.rbDynamicAllocation.Text = "Dynamic Allocation";
            this.rbDynamicAllocation.UseVisualStyleBackColor = true;
            // 
            // rbStaticAllocation
            // 
            this.rbStaticAllocation.AutoSize = true;
            this.rbStaticAllocation.Checked = true;
            this.rbStaticAllocation.Location = new System.Drawing.Point(6, 12);
            this.rbStaticAllocation.Name = "rbStaticAllocation";
            this.rbStaticAllocation.Size = new System.Drawing.Size(101, 17);
            this.rbStaticAllocation.TabIndex = 0;
            this.rbStaticAllocation.TabStop = true;
            this.rbStaticAllocation.Text = "Static Allocation";
            this.rbStaticAllocation.UseVisualStyleBackColor = true;
            // 
            // rbDMAAutomatic
            // 
            this.rbDMAAutomatic.AutoSize = true;
            this.rbDMAAutomatic.Location = new System.Drawing.Point(6, 65);
            this.rbDMAAutomatic.Name = "rbDMAAutomatic";
            this.rbDMAAutomatic.Size = new System.Drawing.Size(181, 17);
            this.rbDMAAutomatic.TabIndex = 0;
            this.rbDMAAutomatic.Text = "DMA w/Automatic Memory Mgmt";
            this.rbDMAAutomatic.UseVisualStyleBackColor = true;
            // 
            // rbDMAManual
            // 
            this.rbDMAManual.AutoSize = true;
            this.rbDMAManual.Location = new System.Drawing.Point(6, 42);
            this.rbDMAManual.Name = "rbDMAManual";
            this.rbDMAManual.Size = new System.Drawing.Size(169, 17);
            this.rbDMAManual.TabIndex = 0;
            this.rbDMAManual.Text = "DMA w/Manual Memory Mgmt";
            this.rbDMAManual.UseVisualStyleBackColor = true;
            // 
            // rbManual
            // 
            this.rbManual.AutoSize = true;
            this.rbManual.Checked = true;
            this.rbManual.Location = new System.Drawing.Point(6, 19);
            this.rbManual.Name = "rbManual";
            this.rbManual.Size = new System.Drawing.Size(101, 17);
            this.rbManual.TabIndex = 0;
            this.rbManual.TabStop = true;
            this.rbManual.Text = "Manual (default)";
            this.rbManual.UseVisualStyleBackColor = true;
            // 
            // CyDetailsEPMngt
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.AutoScroll = true;
            this.Controls.Add(this.groupBoxMemoryMgmt);
            this.Name = "CyDetailsEPMngt";
            this.Size = new System.Drawing.Size(336, 350);
            this.groupBoxMemoryMgmt.ResumeLayout(false);
            this.groupBoxMemoryMgmt.PerformLayout();
            this.groupBoxAllocation.ResumeLayout(false);
            this.groupBoxAllocation.PerformLayout();
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.GroupBox groupBoxMemoryMgmt;
        private System.Windows.Forms.GroupBox groupBoxAllocation;
        private System.Windows.Forms.RadioButton rbDynamicAllocation;
        private System.Windows.Forms.RadioButton rbStaticAllocation;
        private System.Windows.Forms.RadioButton rbDMAAutomatic;
        private System.Windows.Forms.RadioButton rbDMAManual;
        private System.Windows.Forms.RadioButton rbManual;
    }
}
