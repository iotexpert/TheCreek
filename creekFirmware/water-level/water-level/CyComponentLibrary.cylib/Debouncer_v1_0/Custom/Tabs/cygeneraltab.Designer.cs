/*******************************************************************************
* Copyright 2011-2012, Cypress Semiconductor Corporation.  All rights reserved.
* You may use this file only in accordance with the license, terms, conditions, 
* disclaimers, and limitations in the end user license agreement accompanying 
* the software package with which this file was provided.
********************************************************************************/

namespace Debouncer_v1_0
{
    partial class CyGeneralTab
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
            this.m_lblSignalWidth = new System.Windows.Forms.Label();
            this.m_numSignalWidth = new System.Windows.Forms.NumericUpDown();
            this.m_groupOutputType = new System.Windows.Forms.GroupBox();
            this.m_chbEitherEdge = new System.Windows.Forms.CheckBox();
            this.m_chbNegativeEdge = new System.Windows.Forms.CheckBox();
            this.m_chbPositiveEdge = new System.Windows.Forms.CheckBox();
            this.m_toolTipInfo = new System.Windows.Forms.ToolTip(this.components);
            ((System.ComponentModel.ISupportInitialize)(this.m_numSignalWidth)).BeginInit();
            this.m_groupOutputType.SuspendLayout();
            this.SuspendLayout();
            // 
            // m_lblSignalWidth
            // 
            this.m_lblSignalWidth.AutoSize = true;
            this.m_lblSignalWidth.Location = new System.Drawing.Point(3, 10);
            this.m_lblSignalWidth.Name = "m_lblSignalWidth";
            this.m_lblSignalWidth.Size = new System.Drawing.Size(92, 13);
            this.m_lblSignalWidth.TabIndex = 0;
            this.m_lblSignalWidth.Text = "Signal width (bits):";
            // 
            // m_numSignalWidth
            // 
            this.m_numSignalWidth.Location = new System.Drawing.Point(101, 8);
            this.m_numSignalWidth.Maximum = new decimal(new int[] {
            -1981284352,
            -1966660860,
            0,
            0});
            this.m_numSignalWidth.Name = "m_numSignalWidth";
            this.m_numSignalWidth.Size = new System.Drawing.Size(41, 20);
            this.m_numSignalWidth.TabIndex = 1;
            this.m_toolTipInfo.SetToolTip(this.m_numSignalWidth, "Determines the bus width of d and output terminals.");
            // 
            // m_groupOutputType
            // 
            this.m_groupOutputType.Controls.Add(this.m_chbEitherEdge);
            this.m_groupOutputType.Controls.Add(this.m_chbNegativeEdge);
            this.m_groupOutputType.Controls.Add(this.m_chbPositiveEdge);
            this.m_groupOutputType.Location = new System.Drawing.Point(6, 34);
            this.m_groupOutputType.Name = "m_groupOutputType";
            this.m_groupOutputType.Size = new System.Drawing.Size(136, 87);
            this.m_groupOutputType.TabIndex = 2;
            this.m_groupOutputType.TabStop = false;
            this.m_groupOutputType.Text = "Edge type";
            // 
            // m_chbEitherEdge
            // 
            this.m_chbEitherEdge.AutoSize = true;
            this.m_chbEitherEdge.Location = new System.Drawing.Point(6, 65);
            this.m_chbEitherEdge.Name = "m_chbEitherEdge";
            this.m_chbEitherEdge.Size = new System.Drawing.Size(80, 17);
            this.m_chbEitherEdge.TabIndex = 3;
            this.m_chbEitherEdge.Text = "Either edge";
            this.m_toolTipInfo.SetToolTip(this.m_chbEitherEdge, "Specifies whether the positive or negative edge detection is enabled for the comp" +
                    "onent.");
            this.m_chbEitherEdge.UseVisualStyleBackColor = true;
            // 
            // m_chbNegativeEdge
            // 
            this.m_chbNegativeEdge.AutoSize = true;
            this.m_chbNegativeEdge.Location = new System.Drawing.Point(6, 42);
            this.m_chbNegativeEdge.Name = "m_chbNegativeEdge";
            this.m_chbNegativeEdge.Size = new System.Drawing.Size(96, 17);
            this.m_chbNegativeEdge.TabIndex = 2;
            this.m_chbNegativeEdge.Text = "Negative edge";
            this.m_toolTipInfo.SetToolTip(this.m_chbNegativeEdge, "Specifies whether the negative edge detection is enabled for the component.");
            this.m_chbNegativeEdge.UseVisualStyleBackColor = true;
            // 
            // m_chbPositiveEdge
            // 
            this.m_chbPositiveEdge.AutoSize = true;
            this.m_chbPositiveEdge.Location = new System.Drawing.Point(6, 19);
            this.m_chbPositiveEdge.Name = "m_chbPositiveEdge";
            this.m_chbPositiveEdge.Size = new System.Drawing.Size(90, 17);
            this.m_chbPositiveEdge.TabIndex = 1;
            this.m_chbPositiveEdge.Text = "Positive edge";
            this.m_toolTipInfo.SetToolTip(this.m_chbPositiveEdge, "Specifies whether the positive edge detection is enabled for the component.");
            this.m_chbPositiveEdge.UseVisualStyleBackColor = true;
            // 
            // CyGeneralTab
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.Controls.Add(this.m_groupOutputType);
            this.Controls.Add(this.m_numSignalWidth);
            this.Controls.Add(this.m_lblSignalWidth);
            this.Name = "CyGeneralTab";
            this.Size = new System.Drawing.Size(145, 124);
            ((System.ComponentModel.ISupportInitialize)(this.m_numSignalWidth)).EndInit();
            this.m_groupOutputType.ResumeLayout(false);
            this.m_groupOutputType.PerformLayout();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.Label m_lblSignalWidth;
        private System.Windows.Forms.NumericUpDown m_numSignalWidth;
        private System.Windows.Forms.GroupBox m_groupOutputType;
        private System.Windows.Forms.CheckBox m_chbEitherEdge;
        private System.Windows.Forms.CheckBox m_chbNegativeEdge;
        private System.Windows.Forms.CheckBox m_chbPositiveEdge;
        private System.Windows.Forms.ToolTip m_toolTipInfo;
    }
}
