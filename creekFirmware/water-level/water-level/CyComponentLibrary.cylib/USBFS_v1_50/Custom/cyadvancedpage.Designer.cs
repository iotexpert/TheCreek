/*******************************************************************************
* Copyright 2008-2010, Cypress Semiconductor Corporation.  All rights reserved.
* You may use this file only in accordance with the license, terms, conditions, 
* disclaimers, and limitations in the end user license agreement accompanying 
* the software package with which this file was provided.
********************************************************************************/



namespace USBFS_v1_50
{
    partial class CyAdvancedPage
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
            this.groupBox1 = new System.Windows.Forms.GroupBox();
            this.tableLayoutPanel1 = new System.Windows.Forms.TableLayoutPanel();
            this.checkBoxExtClass = new System.Windows.Forms.CheckBox();
            this.label2 = new System.Windows.Forms.Label();
            this.label1 = new System.Windows.Forms.Label();
            this.checkBoxExtVendor = new System.Windows.Forms.CheckBox();
            this.checkBoxSofOutput = new System.Windows.Forms.CheckBox();
            this.checkBoxVBusMon = new System.Windows.Forms.CheckBox();
            this.label3 = new System.Windows.Forms.Label();
            this.label4 = new System.Windows.Forms.Label();
            this.groupBox1.SuspendLayout();
            this.tableLayoutPanel1.SuspendLayout();
            this.SuspendLayout();
            // 
            // groupBox1
            // 
            this.groupBox1.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom)
                        | System.Windows.Forms.AnchorStyles.Left)
                        | System.Windows.Forms.AnchorStyles.Right)));
            this.groupBox1.Controls.Add(this.tableLayoutPanel1);
            this.groupBox1.Location = new System.Drawing.Point(3, 3);
            this.groupBox1.MinimumSize = new System.Drawing.Size(165, 110);
            this.groupBox1.Name = "groupBox1";
            this.groupBox1.Size = new System.Drawing.Size(394, 362);
            this.groupBox1.TabIndex = 0;
            this.groupBox1.TabStop = false;
            this.groupBox1.Text = "Parameters";
            // 
            // tableLayoutPanel1
            // 
            this.tableLayoutPanel1.CellBorderStyle = System.Windows.Forms.TableLayoutPanelCellBorderStyle.InsetDouble;
            this.tableLayoutPanel1.ColumnCount = 2;
            this.tableLayoutPanel1.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Absolute, 150F));
            this.tableLayoutPanel1.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 100F));
            this.tableLayoutPanel1.Controls.Add(this.checkBoxExtClass, 0, 0);
            this.tableLayoutPanel1.Controls.Add(this.label2, 1, 1);
            this.tableLayoutPanel1.Controls.Add(this.label1, 1, 0);
            this.tableLayoutPanel1.Controls.Add(this.checkBoxExtVendor, 0, 1);
            this.tableLayoutPanel1.Controls.Add(this.checkBoxSofOutput, 0, 3);
            this.tableLayoutPanel1.Controls.Add(this.checkBoxVBusMon, 0, 2);
            this.tableLayoutPanel1.Controls.Add(this.label3, 1, 2);
            this.tableLayoutPanel1.Controls.Add(this.label4, 1, 3);
            this.tableLayoutPanel1.Dock = System.Windows.Forms.DockStyle.Top;
            this.tableLayoutPanel1.Location = new System.Drawing.Point(3, 16);
            this.tableLayoutPanel1.Name = "tableLayoutPanel1";
            this.tableLayoutPanel1.RowCount = 4;
            this.tableLayoutPanel1.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 25F));
            this.tableLayoutPanel1.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 25F));
            this.tableLayoutPanel1.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 25F));
            this.tableLayoutPanel1.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 25F));
            this.tableLayoutPanel1.Size = new System.Drawing.Size(388, 242);
            this.tableLayoutPanel1.TabIndex = 0;
            // 
            // checkBoxExtClass
            // 
            this.checkBoxExtClass.CheckAlign = System.Drawing.ContentAlignment.MiddleRight;
            this.checkBoxExtClass.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(204)));
            this.checkBoxExtClass.Location = new System.Drawing.Point(6, 6);
            this.checkBoxExtClass.Name = "checkBoxExtClass";
            this.checkBoxExtClass.Size = new System.Drawing.Size(143, 17);
            this.checkBoxExtClass.TabIndex = 0;
            this.checkBoxExtClass.Text = "External Class";
            this.checkBoxExtClass.UseVisualStyleBackColor = true;
            this.checkBoxExtClass.CheckedChanged += new System.EventHandler(this.checkBoxExtClass_CheckedChanged);
            // 
            // label2
            // 
            this.label2.Dock = System.Windows.Forms.DockStyle.Fill;
            this.label2.ForeColor = System.Drawing.Color.RoyalBlue;
            this.label2.Location = new System.Drawing.Point(159, 62);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(223, 56);
            this.label2.TabIndex = 5;
            this.label2.Text = "This parameter allows for user or other component to implement his own handler fo" +
                "r Vendor specific requests.";
            // 
            // label1
            // 
            this.label1.Dock = System.Windows.Forms.DockStyle.Fill;
            this.label1.ForeColor = System.Drawing.Color.RoyalBlue;
            this.label1.Location = new System.Drawing.Point(159, 3);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(223, 56);
            this.label1.TabIndex = 4;
            this.label1.Text = "This parameter allows for user or other component to implement his own handler fo" +
                "r Class requests.";
            // 
            // checkBoxExtVendor
            // 
            this.checkBoxExtVendor.CheckAlign = System.Drawing.ContentAlignment.MiddleRight;
            this.checkBoxExtVendor.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(204)));
            this.checkBoxExtVendor.Location = new System.Drawing.Point(6, 65);
            this.checkBoxExtVendor.Name = "checkBoxExtVendor";
            this.checkBoxExtVendor.Size = new System.Drawing.Size(143, 17);
            this.checkBoxExtVendor.TabIndex = 1;
            this.checkBoxExtVendor.Text = "External Vendor";
            this.checkBoxExtVendor.UseVisualStyleBackColor = true;
            this.checkBoxExtVendor.CheckedChanged += new System.EventHandler(this.checkBoxExtClass_CheckedChanged);
            // 
            // checkBoxSofOutput
            // 
            this.checkBoxSofOutput.CheckAlign = System.Drawing.ContentAlignment.MiddleRight;
            this.checkBoxSofOutput.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(204)));
            this.checkBoxSofOutput.Location = new System.Drawing.Point(6, 183);
            this.checkBoxSofOutput.Name = "checkBoxSofOutput";
            this.checkBoxSofOutput.Size = new System.Drawing.Size(143, 17);
            this.checkBoxSofOutput.TabIndex = 3;
            this.checkBoxSofOutput.Text = "Enable SOF Output";
            this.checkBoxSofOutput.UseVisualStyleBackColor = true;
            this.checkBoxSofOutput.CheckedChanged += new System.EventHandler(this.checkBoxExtClass_CheckedChanged);
            // 
            // checkBoxVBusMon
            // 
            this.checkBoxVBusMon.CheckAlign = System.Drawing.ContentAlignment.MiddleRight;
            this.checkBoxVBusMon.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(204)));
            this.checkBoxVBusMon.Location = new System.Drawing.Point(6, 124);
            this.checkBoxVBusMon.Name = "checkBoxVBusMon";
            this.checkBoxVBusMon.Size = new System.Drawing.Size(143, 17);
            this.checkBoxVBusMon.TabIndex = 2;
            this.checkBoxVBusMon.Text = "Enable VBUS Monitoring";
            this.checkBoxVBusMon.UseVisualStyleBackColor = true;
            this.checkBoxVBusMon.CheckedChanged += new System.EventHandler(this.checkBoxExtClass_CheckedChanged);
            // 
            // label3
            // 
            this.label3.Dock = System.Windows.Forms.DockStyle.Fill;
            this.label3.ForeColor = System.Drawing.Color.RoyalBlue;
            this.label3.Location = new System.Drawing.Point(159, 121);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(223, 56);
            this.label3.TabIndex = 6;
            this.label3.Text = "This parameter adds a single VBUS monitor pin to the design. This pin must be con" +
                "nected to VBUS and must be assigned in the pin editor.";
            // 
            // label4
            // 
            this.label4.Dock = System.Windows.Forms.DockStyle.Fill;
            this.label4.ForeColor = System.Drawing.Color.RoyalBlue;
            this.label4.Location = new System.Drawing.Point(159, 180);
            this.label4.Name = "label4";
            this.label4.Size = new System.Drawing.Size(223, 59);
            this.label4.TabIndex = 7;
            this.label4.Text = "This parameter enables Start-of-Frame output.";
            // 
            // CyAdvancedPage
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.AutoScroll = true;
            this.Controls.Add(this.groupBox1);
            this.Name = "CyAdvancedPage";
            this.Size = new System.Drawing.Size(400, 382);
            this.groupBox1.ResumeLayout(false);
            this.tableLayoutPanel1.ResumeLayout(false);
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.GroupBox groupBox1;
        private System.Windows.Forms.CheckBox checkBoxSofOutput;
        private System.Windows.Forms.CheckBox checkBoxVBusMon;
        private System.Windows.Forms.CheckBox checkBoxExtVendor;
        private System.Windows.Forms.CheckBox checkBoxExtClass;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.TableLayoutPanel tableLayoutPanel1;
        private System.Windows.Forms.Label label3;
        private System.Windows.Forms.Label label4;
    }
}
