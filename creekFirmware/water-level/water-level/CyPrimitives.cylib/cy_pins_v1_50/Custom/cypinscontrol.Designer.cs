namespace Cypress.Comps.PinsAndPorts.cy_pins_v1_50
{
    partial class CyPinsControl
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
            if (disposing)
            {
                PerformDispose();
            }

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
            CyDesigner.Common.Base.Controls.CyTableViewBase.CyImageList cyImageList1 = new CyDesigner.Common.Base.Controls.CyTableViewBase.CyImageList();
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(CyPinsControl));
            this.m_splitContainer = new System.Windows.Forms.SplitContainer();
            this.m_treeView = new CyDesigner.Common.Base.Controls.CyMultiSelectTreeView();
            this.m_toolStrip = new System.Windows.Forms.ToolStrip();
            this.m_splitContainer.Panel1.SuspendLayout();
            this.m_splitContainer.SuspendLayout();
            this.SuspendLayout();
            // 
            // m_splitContainer
            // 
            this.m_splitContainer.BorderStyle = System.Windows.Forms.BorderStyle.Fixed3D;
            this.m_splitContainer.Dock = System.Windows.Forms.DockStyle.Fill;
            this.m_splitContainer.FixedPanel = System.Windows.Forms.FixedPanel.Panel1;
            this.m_splitContainer.Location = new System.Drawing.Point(0, 0);
            this.m_splitContainer.Name = "m_splitContainer";
            // 
            // m_splitContainer.Panel1
            // 
            this.m_splitContainer.Panel1.Controls.Add(this.m_treeView);
            this.m_splitContainer.Size = new System.Drawing.Size(504, 424);
            this.m_splitContainer.SplitterDistance = 140;
            this.m_splitContainer.TabIndex = 0;
            // 
            // m_treeView
            // 
            this.m_treeView.BackColor = System.Drawing.SystemColors.Window;
            this.m_treeView.CheckBoxes = CyDesigner.Common.Base.Controls.CyTreeView.CheckBoxDisplay.SHOW_NONE;
            this.m_treeView.CyTreeViewNodeSorter = null;
            this.m_treeView.Dock = System.Windows.Forms.DockStyle.Fill;
            this.m_treeView.Font = new System.Drawing.Font("Microsoft Sans Serif", 7.8F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.m_treeView.ForeColor = System.Drawing.SystemColors.WindowText;
            this.m_treeView.FullRowSelect = false;
            this.m_treeView.HideSelection = true;
            this.m_treeView.HotTracking = false;
            this.m_treeView.HotTrackingFont = new System.Drawing.Font("Microsoft Sans Serif", 7.8F, System.Drawing.FontStyle.Underline);
            this.m_treeView.HotTrackingForeColor = System.Drawing.SystemColors.HotTrack;
            this.m_treeView.ImageIndex = -1;
            this.m_treeView.ImageKey = null;
            cyImageList1.ImageSize = new System.Drawing.Size(16, 16);
            this.m_treeView.ImageList = cyImageList1;
            this.m_treeView.Indent = 19;
            this.m_treeView.ItemHeight = 18;
            this.m_treeView.LineColor = System.Drawing.SystemColors.GrayText;
            this.m_treeView.Location = new System.Drawing.Point(0, 0);
            this.m_treeView.MakeId = ((CyDesigner.Common.Base.Controls.CyTreeView.MakeUniqueIdDelegate)(resources.GetObject("m_treeView.MakeId")));
            this.m_treeView.Name = "m_treeView";
            this.m_treeView.PathSeperator = "\\";
            this.m_treeView.Scrollable = true;
            this.m_treeView.SelectedNode = null;
            this.m_treeView.ShowHorizontalScrollBar = true;
            this.m_treeView.ShowLines = true;
            this.m_treeView.ShowNodeToolTips = false;
            this.m_treeView.ShowPlusMinus = true;
            this.m_treeView.ShowRootLines = true;
            this.m_treeView.Size = new System.Drawing.Size(164, 395);
            this.m_treeView.Sorted = false;
            this.m_treeView.TabIndex = 0;
            this.m_treeView.TabStop = false;
            this.m_treeView.Text = "cyTreeView1";
            // 
            // m_toolStrip
            // 
            this.m_toolStrip.Dock = System.Windows.Forms.DockStyle.Top;
            this.m_toolStrip.Location = new System.Drawing.Point(0, 395);
            this.m_toolStrip.Name = "m_toolStrip";
            this.m_toolStrip.Size = new System.Drawing.Size(164, 25);
            this.m_toolStrip.TabIndex = 1;
            this.m_toolStrip.Text = "toolStrip1";
            // 
            // CyPinsControl
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(8F, 16F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.Controls.Add(this.m_splitContainer);
            this.Controls.Add(this.m_toolStrip);
            this.Name = "CyPinsControl";
            this.Size = new System.Drawing.Size(504, 424);
            this.m_splitContainer.Panel1.ResumeLayout(false);
            this.m_splitContainer.Panel1.PerformLayout();
            this.m_splitContainer.ResumeLayout(false);
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.SplitContainer m_splitContainer;
        private CyDesigner.Common.Base.Controls.CyMultiSelectTreeView m_treeView;
        private System.Windows.Forms.ToolStrip m_toolStrip;
    }
}
