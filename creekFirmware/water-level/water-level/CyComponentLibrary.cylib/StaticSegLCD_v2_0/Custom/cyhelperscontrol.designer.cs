/*******************************************************************************
* Copyright 2008-2011, Cypress Semiconductor Corporation.  All rights reserved.
* You may use this file only in accordance with the license, terms, conditions, 
* disclaimers, and limitations in the end user license agreement accompanying 
* the software package with which this file was provided.
********************************************************************************/

using System.Drawing;

namespace StaticSegLCD_v2_0
{
    partial class CyHelpers
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
            this.components = new System.ComponentModel.Container();
            this.panelDisplay = new System.Windows.Forms.Panel();
            this.contextMenuPixels = new System.Windows.Forms.ContextMenuStrip(this.components);
            this.resetToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.buttonRemoveSymbol = new System.Windows.Forms.Button();
            this.labelCharsNum = new System.Windows.Forms.Label();
            this.listBoxAddedHelpers = new System.Windows.Forms.ListBox();
            this.labelAddedHelpers = new System.Windows.Forms.Label();
            this.labelHelpers = new System.Windows.Forms.Label();
            this.listBoxAvailHelpers = new System.Windows.Forms.ListBox();
            this.groupBoxHelperConfig = new System.Windows.Forms.GroupBox();
            this.panelHelperConfig = new System.Windows.Forms.Panel();
            this.textBoxSegmentTitle = new System.Windows.Forms.TextBox();
            this.label2 = new System.Windows.Forms.Label();
            this.label1 = new System.Windows.Forms.Label();
            this.buttonAddSymbol = new System.Windows.Forms.Button();
            this.buttonRemoveHelper = new System.Windows.Forms.Button();
            this.dataGridMap = new System.Windows.Forms.DataGridView();
            this.groupBoxMap = new System.Windows.Forms.GroupBox();
            this.printDocumentPixelMap = new System.Drawing.Printing.PrintDocument();
            this.printDialog1 = new System.Windows.Forms.PrintDialog();
            this.buttonPrint = new System.Windows.Forms.Button();
            this.panelHide = new System.Windows.Forms.Panel();
            this.buttonAddHelper = new System.Windows.Forms.Button();
            this.toolTipPixels = new System.Windows.Forms.ToolTip(this.components);
            this.errorProvider = new System.Windows.Forms.ErrorProvider(this.components);
            this.contextMenuPixels.SuspendLayout();
            this.groupBoxHelperConfig.SuspendLayout();
            this.panelHelperConfig.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.dataGridMap)).BeginInit();
            this.groupBoxMap.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.errorProvider)).BeginInit();
            this.SuspendLayout();
            // 
            // panelDisplay
            // 
            this.panelDisplay.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left)
                        | System.Windows.Forms.AnchorStyles.Right)));
            this.panelDisplay.AutoScroll = true;
            this.panelDisplay.BorderStyle = System.Windows.Forms.BorderStyle.Fixed3D;
            this.panelDisplay.Cursor = System.Windows.Forms.Cursors.Default;
            this.panelDisplay.Location = new System.Drawing.Point(5, 26);
            this.panelDisplay.Name = "panelDisplay";
            this.panelDisplay.Size = new System.Drawing.Size(428, 97);
            this.panelDisplay.TabIndex = 0;
            // 
            // contextMenuPixels
            // 
            this.contextMenuPixels.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.resetToolStripMenuItem});
            this.contextMenuPixels.Name = "contextMenuPixels";
            this.contextMenuPixels.Size = new System.Drawing.Size(153, 48);
            this.contextMenuPixels.Opening += new System.ComponentModel.CancelEventHandler(this.contextMenuPixels_Opening);
            // 
            // resetToolStripMenuItem
            // 
            this.resetToolStripMenuItem.Name = "resetToolStripMenuItem";
            this.resetToolStripMenuItem.Size = new System.Drawing.Size(152, 22);
            this.resetToolStripMenuItem.Text = "Reset";
            this.resetToolStripMenuItem.Click += new System.EventHandler(this.resetToolStripMenuItem_Click);
            // 
            // buttonRemoveSymbol
            // 
            this.buttonRemoveSymbol.Image = global::StaticSegLCD_v2_0.Properties.Resources.im_delete;
            this.buttonRemoveSymbol.Location = new System.Drawing.Point(40, 3);
            this.buttonRemoveSymbol.Name = "buttonRemoveSymbol";
            this.buttonRemoveSymbol.Size = new System.Drawing.Size(29, 23);
            this.buttonRemoveSymbol.TabIndex = 3;
            this.buttonRemoveSymbol.UseVisualStyleBackColor = true;
            this.buttonRemoveSymbol.Click += new System.EventHandler(this.buttonRemoveSymbol_Click);
            // 
            // labelCharsNum
            // 
            this.labelCharsNum.AutoSize = true;
            this.labelCharsNum.Font = new System.Drawing.Font("Microsoft Sans Serif", 10F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(204)));
            this.labelCharsNum.Location = new System.Drawing.Point(174, 6);
            this.labelCharsNum.Name = "labelCharsNum";
            this.labelCharsNum.Size = new System.Drawing.Size(17, 17);
            this.labelCharsNum.TabIndex = 4;
            this.labelCharsNum.Text = "0";
            // 
            // listBoxAddedHelpers
            // 
            this.listBoxAddedHelpers.FormattingEnabled = true;
            this.listBoxAddedHelpers.Location = new System.Drawing.Point(280, 23);
            this.listBoxAddedHelpers.Name = "listBoxAddedHelpers";
            this.listBoxAddedHelpers.Size = new System.Drawing.Size(148, 69);
            this.listBoxAddedHelpers.TabIndex = 8;
            this.listBoxAddedHelpers.SelectedIndexChanged += new System.EventHandler(this.listBoxAddedHelpers_SelectedIndexChanged);
            this.listBoxAddedHelpers.DoubleClick += new System.EventHandler(this.listBoxAddedHelpers_DoubleClick);
            // 
            // labelAddedHelpers
            // 
            this.labelAddedHelpers.AutoSize = true;
            this.labelAddedHelpers.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(204)));
            this.labelAddedHelpers.Location = new System.Drawing.Point(278, 7);
            this.labelAddedHelpers.Name = "labelAddedHelpers";
            this.labelAddedHelpers.Size = new System.Drawing.Size(104, 13);
            this.labelAddedHelpers.TabIndex = 6;
            this.labelAddedHelpers.Text = "Selected Helpers";
            // 
            // labelHelpers
            // 
            this.labelHelpers.AutoSize = true;
            this.labelHelpers.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(204)));
            this.labelHelpers.Location = new System.Drawing.Point(11, 7);
            this.labelHelpers.Name = "labelHelpers";
            this.labelHelpers.Size = new System.Drawing.Size(50, 13);
            this.labelHelpers.TabIndex = 7;
            this.labelHelpers.Text = "Helpers";
            // 
            // listBoxAvailHelpers
            // 
            this.listBoxAvailHelpers.FormattingEnabled = true;
            this.listBoxAvailHelpers.Items.AddRange(new object[] {
            "7 Segment",
            "14 Segment",
            "16 Segment",
            "Bargraph and Dial"});
            this.listBoxAvailHelpers.Location = new System.Drawing.Point(11, 23);
            this.listBoxAvailHelpers.Name = "listBoxAvailHelpers";
            this.listBoxAvailHelpers.Size = new System.Drawing.Size(150, 69);
            this.listBoxAvailHelpers.TabIndex = 5;
            // 
            // groupBoxHelperConfig
            // 
            this.groupBoxHelperConfig.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left)
                        | System.Windows.Forms.AnchorStyles.Right)));
            this.groupBoxHelperConfig.Controls.Add(this.panelHelperConfig);
            this.groupBoxHelperConfig.Location = new System.Drawing.Point(11, 98);
            this.groupBoxHelperConfig.Name = "groupBoxHelperConfig";
            this.groupBoxHelperConfig.Size = new System.Drawing.Size(442, 144);
            this.groupBoxHelperConfig.TabIndex = 11;
            this.groupBoxHelperConfig.TabStop = false;
            this.groupBoxHelperConfig.Text = "Helper function configuration";
            // 
            // panelHelperConfig
            // 
            this.panelHelperConfig.Controls.Add(this.textBoxSegmentTitle);
            this.panelHelperConfig.Controls.Add(this.label2);
            this.panelHelperConfig.Controls.Add(this.label1);
            this.panelHelperConfig.Controls.Add(this.buttonRemoveSymbol);
            this.panelHelperConfig.Controls.Add(this.panelDisplay);
            this.panelHelperConfig.Controls.Add(this.labelCharsNum);
            this.panelHelperConfig.Controls.Add(this.buttonAddSymbol);
            this.panelHelperConfig.Dock = System.Windows.Forms.DockStyle.Fill;
            this.panelHelperConfig.Location = new System.Drawing.Point(3, 16);
            this.panelHelperConfig.Name = "panelHelperConfig";
            this.panelHelperConfig.Size = new System.Drawing.Size(436, 125);
            this.panelHelperConfig.TabIndex = 5;
            this.panelHelperConfig.Visible = false;
            // 
            // textBoxSegmentTitle
            // 
            this.textBoxSegmentTitle.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left)
                        | System.Windows.Forms.AnchorStyles.Right)));
            this.textBoxSegmentTitle.Location = new System.Drawing.Point(323, 5);
            this.textBoxSegmentTitle.MinimumSize = new System.Drawing.Size(76, 4);
            this.textBoxSegmentTitle.Name = "textBoxSegmentTitle";
            this.textBoxSegmentTitle.Size = new System.Drawing.Size(110, 20);
            this.textBoxSegmentTitle.TabIndex = 0;
            this.textBoxSegmentTitle.TextChanged += new System.EventHandler(this.textBoxSegmentTitle_TextChanged);
            this.textBoxSegmentTitle.Validated += new System.EventHandler(this.textBoxSegmentTitle_Validated);
            this.textBoxSegmentTitle.Validating += new System.ComponentModel.CancelEventHandler(this.textBoxSegmentTitle_Validating);
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Location = new System.Drawing.Point(215, 8);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(102, 13);
            this.label2.TabIndex = 7;
            this.label2.Text = "Selected pixel name";
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(75, 8);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(99, 13);
            this.label1.TabIndex = 6;
            this.label1.Text = "Number of symbols:";
            // 
            // buttonAddSymbol
            // 
            this.buttonAddSymbol.Image = global::StaticSegLCD_v2_0.Properties.Resources.im_add;
            this.buttonAddSymbol.Location = new System.Drawing.Point(5, 3);
            this.buttonAddSymbol.Name = "buttonAddSymbol";
            this.buttonAddSymbol.Size = new System.Drawing.Size(29, 23);
            this.buttonAddSymbol.TabIndex = 2;
            this.buttonAddSymbol.UseVisualStyleBackColor = true;
            this.buttonAddSymbol.Click += new System.EventHandler(this.buttonAddSymbol_Click);
            // 
            // buttonRemoveHelper
            // 
            this.buttonRemoveHelper.Image = global::StaticSegLCD_v2_0.Properties.Resources.im_arrowleft;
            this.buttonRemoveHelper.Location = new System.Drawing.Point(185, 66);
            this.buttonRemoveHelper.Name = "buttonRemoveHelper";
            this.buttonRemoveHelper.Size = new System.Drawing.Size(70, 23);
            this.buttonRemoveHelper.TabIndex = 10;
            this.buttonRemoveHelper.UseVisualStyleBackColor = true;
            this.buttonRemoveHelper.Click += new System.EventHandler(this.buttonRemoveHelper_Click);
            // 
            // dataGridMap
            // 
            this.dataGridMap.AllowDrop = true;
            this.dataGridMap.AllowUserToAddRows = false;
            this.dataGridMap.AllowUserToDeleteRows = false;
            this.dataGridMap.AllowUserToResizeColumns = false;
            this.dataGridMap.AllowUserToResizeRows = false;
            this.dataGridMap.BackgroundColor = System.Drawing.SystemColors.Control;
            this.dataGridMap.BorderStyle = System.Windows.Forms.BorderStyle.None;
            this.dataGridMap.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            this.dataGridMap.ColumnHeadersVisible = false;
            this.dataGridMap.ContextMenuStrip = this.contextMenuPixels;
            this.dataGridMap.Dock = System.Windows.Forms.DockStyle.Fill;
            this.dataGridMap.GridColor = System.Drawing.SystemColors.Control;
            this.dataGridMap.Location = new System.Drawing.Point(3, 16);
            this.dataGridMap.MultiSelect = false;
            this.dataGridMap.Name = "dataGridMap";
            this.dataGridMap.RowHeadersVisible = false;
            this.dataGridMap.RowTemplate.Height = 18;
            this.dataGridMap.ShowEditingIcon = false;
            this.dataGridMap.Size = new System.Drawing.Size(436, 91);
            this.dataGridMap.TabIndex = 14;
            this.dataGridMap.TabStop = false;
            this.dataGridMap.CellBeginEdit += new System.Windows.Forms.DataGridViewCellCancelEventHandler(this.dataGridMap_CellBeginEdit);
            this.dataGridMap.CellDoubleClick += new System.Windows.Forms.DataGridViewCellEventHandler(this.dataGridMap_CellDoubleClick);
            this.dataGridMap.MouseMove += new System.Windows.Forms.MouseEventHandler(this.dataGridMap_MouseMove);
            this.dataGridMap.CellMouseDown += new System.Windows.Forms.DataGridViewCellMouseEventHandler(this.dataGridMap_CellMouseDown);
            this.dataGridMap.DragOver += new System.Windows.Forms.DragEventHandler(this.dataGridMap_DragOver);
            this.dataGridMap.CellValidating += new System.Windows.Forms.DataGridViewCellValidatingEventHandler(this.dataGridMap_CellValidating);
            this.dataGridMap.CellEndEdit += new System.Windows.Forms.DataGridViewCellEventHandler(this.dataGridMap_CellEndEdit);
            this.dataGridMap.DragLeave += new System.EventHandler(this.dataGridMap_DragLeave);
            this.dataGridMap.SelectionChanged += new System.EventHandler(this.dataGridMap_SelectionChanged);
            this.dataGridMap.DragDrop += new System.Windows.Forms.DragEventHandler(this.dataGridMap_DragDrop);
            // 
            // groupBoxMap
            // 
            this.groupBoxMap.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom)
                        | System.Windows.Forms.AnchorStyles.Left)
                        | System.Windows.Forms.AnchorStyles.Right)));
            this.groupBoxMap.Controls.Add(this.dataGridMap);
            this.groupBoxMap.Location = new System.Drawing.Point(11, 245);
            this.groupBoxMap.MinimumSize = new System.Drawing.Size(0, 50);
            this.groupBoxMap.Name = "groupBoxMap";
            this.groupBoxMap.Size = new System.Drawing.Size(442, 110);
            this.groupBoxMap.TabIndex = 15;
            this.groupBoxMap.TabStop = false;
            this.groupBoxMap.Text = "Pixel Mapping Table";
            // 
            // printDocumentPixelMap
            // 
            this.printDocumentPixelMap.OriginAtMargins = true;
            this.printDocumentPixelMap.PrintPage += new System.Drawing.Printing.PrintPageEventHandler(this.printDocumentPixelMap_PrintPage);
            // 
            // printDialog1
            // 
            this.printDialog1.Document = this.printDocumentPixelMap;
            this.printDialog1.UseEXDialog = true;
            // 
            // buttonPrint
            // 
            this.buttonPrint.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)));
            this.buttonPrint.Location = new System.Drawing.Point(11, 357);
            this.buttonPrint.Name = "buttonPrint";
            this.buttonPrint.Size = new System.Drawing.Size(75, 23);
            this.buttonPrint.TabIndex = 16;
            this.buttonPrint.Text = "Print";
            this.buttonPrint.UseVisualStyleBackColor = true;
            this.buttonPrint.Click += new System.EventHandler(this.buttonPrint_Click);
            // 
            // panelHide
            // 
            this.panelHide.Location = new System.Drawing.Point(95, 365);
            this.panelHide.Name = "panelHide";
            this.panelHide.Size = new System.Drawing.Size(10, 13);
            this.panelHide.TabIndex = 17;
            // 
            // buttonAddHelper
            // 
            this.buttonAddHelper.Image = global::StaticSegLCD_v2_0.Properties.Resources.im_arrowright;
            this.buttonAddHelper.Location = new System.Drawing.Point(185, 37);
            this.buttonAddHelper.Name = "buttonAddHelper";
            this.buttonAddHelper.Size = new System.Drawing.Size(70, 23);
            this.buttonAddHelper.TabIndex = 9;
            this.buttonAddHelper.UseVisualStyleBackColor = true;
            this.buttonAddHelper.Click += new System.EventHandler(this.buttonAddHelper_Click);
            // 
            // errorProvider
            // 
            this.errorProvider.BlinkStyle = System.Windows.Forms.ErrorBlinkStyle.NeverBlink;
            this.errorProvider.ContainerControl = this;
            // 
            // CyHelpers
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.AutoScroll = true;
            this.Controls.Add(this.buttonPrint);
            this.Controls.Add(this.groupBoxMap);
            this.Controls.Add(this.groupBoxHelperConfig);
            this.Controls.Add(this.labelHelpers);
            this.Controls.Add(this.buttonRemoveHelper);
            this.Controls.Add(this.buttonAddHelper);
            this.Controls.Add(this.listBoxAddedHelpers);
            this.Controls.Add(this.listBoxAvailHelpers);
            this.Controls.Add(this.labelAddedHelpers);
            this.Controls.Add(this.panelHide);
            this.Name = "CyHelpers";
            this.Size = new System.Drawing.Size(456, 383);
            this.Load += new System.EventHandler(this.CyHelpers_Load);
            this.contextMenuPixels.ResumeLayout(false);
            this.groupBoxHelperConfig.ResumeLayout(false);
            this.panelHelperConfig.ResumeLayout(false);
            this.panelHelperConfig.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.dataGridMap)).EndInit();
            this.groupBoxMap.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.errorProvider)).EndInit();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.Panel panelDisplay;
        private System.Windows.Forms.Button buttonAddSymbol;
        private System.Windows.Forms.Button buttonRemoveSymbol;
        private System.Windows.Forms.Label labelCharsNum;
        private System.Windows.Forms.Button buttonRemoveHelper;
        private System.Windows.Forms.Button buttonAddHelper;
        private System.Windows.Forms.ListBox listBoxAddedHelpers;
        private System.Windows.Forms.Label labelAddedHelpers;
        private System.Windows.Forms.Label labelHelpers;
        private System.Windows.Forms.ListBox listBoxAvailHelpers;
        private System.Windows.Forms.GroupBox groupBoxHelperConfig;
        private System.Windows.Forms.Panel panelHelperConfig;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.DataGridView dataGridMap;
        private System.Windows.Forms.ContextMenuStrip contextMenuPixels;
        private System.Windows.Forms.ToolStripMenuItem resetToolStripMenuItem;
        private System.Windows.Forms.GroupBox groupBoxMap;
        private System.Drawing.Printing.PrintDocument printDocumentPixelMap;
        private System.Windows.Forms.PrintDialog printDialog1;
        private System.Windows.Forms.Button buttonPrint;
        private System.Windows.Forms.TextBox textBoxSegmentTitle;
        private System.Windows.Forms.Panel panelHide;
        private System.Windows.Forms.ToolTip toolTipPixels;
        private System.Windows.Forms.ErrorProvider errorProvider;
    }
}