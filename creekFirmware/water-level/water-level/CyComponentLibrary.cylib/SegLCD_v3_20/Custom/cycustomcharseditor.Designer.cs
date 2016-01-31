/*******************************************************************************
* Copyright 2008-2012, Cypress Semiconductor Corporation.  All rights reserved.
* You may use this file only in accordance with the license, terms, conditions, 
* disclaimers, and limitations in the end user license agreement accompanying 
* the software package with which this file was provided.
********************************************************************************/

namespace SegLCD_v3_20
{
    partial class CyCustomCharsEditor
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(CyCustomCharsEditor));
            this.groupBoxEditor = new System.Windows.Forms.GroupBox();
            this.labelSelectedChar = new System.Windows.Forms.Label();
            this.currentEditableCharacter = new System.Windows.Forms.Label();
            this.cyCustomCharacter1 = new SegLCD_v3_20.CyCustomCharacter();
            this.listBoxChars = new System.Windows.Forms.ListBox();
            this.groupBoxCharsList = new System.Windows.Forms.GroupBox();
            this.toolStripChars = new System.Windows.Forms.ToolStrip();
            this.toolStripButtonLoad = new System.Windows.Forms.ToolStripButton();
            this.toolStripButtonSave = new System.Windows.Forms.ToolStripButton();
            this.toolStripButtonDefaultList = new System.Windows.Forms.ToolStripButton();
            this.toolStripSeparator1 = new System.Windows.Forms.ToolStripSeparator();
            this.toolStripButtonResetAll = new System.Windows.Forms.ToolStripButton();
            this.toolStripButtonReset = new System.Windows.Forms.ToolStripButton();
            this.toolStripSeparator2 = new System.Windows.Forms.ToolStripSeparator();
            this.groupBoxEditor.SuspendLayout();
            this.groupBoxCharsList.SuspendLayout();
            this.toolStripChars.SuspendLayout();
            this.SuspendLayout();
            // 
            // groupBoxEditor
            // 
            resources.ApplyResources(this.groupBoxEditor, "groupBoxEditor");
            this.groupBoxEditor.Controls.Add(this.labelSelectedChar);
            this.groupBoxEditor.Controls.Add(this.currentEditableCharacter);
            this.groupBoxEditor.Controls.Add(this.cyCustomCharacter1);
            this.groupBoxEditor.ForeColor = System.Drawing.Color.RoyalBlue;
            this.groupBoxEditor.Name = "groupBoxEditor";
            this.groupBoxEditor.TabStop = false;
            // 
            // labelSelectedChar
            // 
            resources.ApplyResources(this.labelSelectedChar, "labelSelectedChar");
            this.labelSelectedChar.ForeColor = System.Drawing.SystemColors.WindowText;
            this.labelSelectedChar.Name = "labelSelectedChar";
            // 
            // currentEditableCharacter
            // 
            resources.ApplyResources(this.currentEditableCharacter, "currentEditableCharacter");
            this.currentEditableCharacter.ForeColor = System.Drawing.Color.RoyalBlue;
            this.currentEditableCharacter.Name = "currentEditableCharacter";
            // 
            // cyCustomCharacter1
            // 
            this.cyCustomCharacter1.Activate = false;
            this.cyCustomCharacter1.BorderWidth = 1;
            this.cyCustomCharacter1.Columns = 5;
            this.cyCustomCharacter1.DisplayName = "Custom Character";
            resources.ApplyResources(this.cyCustomCharacter1, "cyCustomCharacter1");
            this.cyCustomCharacter1.MinimumSize = new System.Drawing.Size(5, 5);
            this.cyCustomCharacter1.Name = "cyCustomCharacter1";
            this.cyCustomCharacter1.Rows = 8;
            this.cyCustomCharacter1.Selected = true;
            // 
            // listBoxChars
            // 
            resources.ApplyResources(this.listBoxChars, "listBoxChars");
            this.listBoxChars.FormattingEnabled = true;
            this.listBoxChars.Name = "listBoxChars";
            this.listBoxChars.SelectedIndexChanged += new System.EventHandler(this.listBoxChars_SelectedIndexChanged);
            this.listBoxChars.Format += new System.Windows.Forms.ListControlConvertEventHandler(this.listBoxChars_Format);
            // 
            // groupBoxCharsList
            // 
            this.groupBoxCharsList.Controls.Add(this.listBoxChars);
            resources.ApplyResources(this.groupBoxCharsList, "groupBoxCharsList");
            this.groupBoxCharsList.ForeColor = System.Drawing.Color.RoyalBlue;
            this.groupBoxCharsList.Name = "groupBoxCharsList";
            this.groupBoxCharsList.TabStop = false;
            // 
            // toolStripChars
            // 
            resources.ApplyResources(this.toolStripChars, "toolStripChars");
            this.toolStripChars.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.toolStripButtonLoad,
            this.toolStripButtonSave,
            this.toolStripButtonDefaultList,
            this.toolStripSeparator1,
            this.toolStripButtonResetAll,
            this.toolStripButtonReset,
            this.toolStripSeparator2});
            this.toolStripChars.LayoutStyle = System.Windows.Forms.ToolStripLayoutStyle.VerticalStackWithOverflow;
            this.toolStripChars.Name = "toolStripChars";
            // 
            // toolStripButtonLoad
            // 
            this.toolStripButtonLoad.Image = global::SegLCD_v3_20.Properties.Resources.imimport;
            resources.ApplyResources(this.toolStripButtonLoad, "toolStripButtonLoad");
            this.toolStripButtonLoad.Name = "toolStripButtonLoad";
            this.toolStripButtonLoad.Click += new System.EventHandler(this.toolStripButtonLoad_Click);
            // 
            // toolStripButtonSave
            // 
            this.toolStripButtonSave.Image = global::SegLCD_v3_20.Properties.Resources.imsave;
            resources.ApplyResources(this.toolStripButtonSave, "toolStripButtonSave");
            this.toolStripButtonSave.Name = "toolStripButtonSave";
            this.toolStripButtonSave.Click += new System.EventHandler(this.toolStripButtonSave_Click);
            // 
            // toolStripButtonDefaultList
            // 
            resources.ApplyResources(this.toolStripButtonDefaultList, "toolStripButtonDefaultList");
            this.toolStripButtonDefaultList.Image = global::SegLCD_v3_20.Properties.Resources.immenu;
            this.toolStripButtonDefaultList.Name = "toolStripButtonDefaultList";
            this.toolStripButtonDefaultList.Click += new System.EventHandler(this.toolStripButtonDefaultList_Click);
            // 
            // toolStripSeparator1
            // 
            this.toolStripSeparator1.Name = "toolStripSeparator1";
            resources.ApplyResources(this.toolStripSeparator1, "toolStripSeparator1");
            // 
            // toolStripButtonResetAll
            // 
            resources.ApplyResources(this.toolStripButtonResetAll, "toolStripButtonResetAll");
            this.toolStripButtonResetAll.Image = global::SegLCD_v3_20.Properties.Resources.imrefresh;
            this.toolStripButtonResetAll.Name = "toolStripButtonResetAll";
            this.toolStripButtonResetAll.Click += new System.EventHandler(this.toolStripButtonResetAll_Click);
            // 
            // toolStripButtonReset
            // 
            resources.ApplyResources(this.toolStripButtonReset, "toolStripButtonReset");
            this.toolStripButtonReset.Image = global::SegLCD_v3_20.Properties.Resources.imundo;
            this.toolStripButtonReset.Name = "toolStripButtonReset";
            this.toolStripButtonReset.Click += new System.EventHandler(this.toolStripButtonReset_Click);
            // 
            // toolStripSeparator2
            // 
            this.toolStripSeparator2.Name = "toolStripSeparator2";
            resources.ApplyResources(this.toolStripSeparator2, "toolStripSeparator2");
            // 
            // CyCustomCharsEditor
            // 
            resources.ApplyResources(this, "$this");
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.Controls.Add(this.toolStripChars);
            this.Controls.Add(this.groupBoxCharsList);
            this.Controls.Add(this.groupBoxEditor);
            this.Name = "CyCustomCharsEditor";
            this.Load += new System.EventHandler(this.CyCharsCustomizer_Load);
            this.groupBoxEditor.ResumeLayout(false);
            this.groupBoxEditor.PerformLayout();
            this.groupBoxCharsList.ResumeLayout(false);
            this.toolStripChars.ResumeLayout(false);
            this.toolStripChars.PerformLayout();
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.GroupBox groupBoxEditor;
        private CyCustomCharacter cyCustomCharacter1;
        private System.Windows.Forms.Label currentEditableCharacter;
        private System.Windows.Forms.ListBox listBoxChars;
        private System.Windows.Forms.GroupBox groupBoxCharsList;
        private System.Windows.Forms.Label labelSelectedChar;
        private System.Windows.Forms.ToolStrip toolStripChars;
        private System.Windows.Forms.ToolStripButton toolStripButtonLoad;
        private System.Windows.Forms.ToolStripButton toolStripButtonSave;
        private System.Windows.Forms.ToolStripSeparator toolStripSeparator1;
        private System.Windows.Forms.ToolStripButton toolStripButtonResetAll;
        private System.Windows.Forms.ToolStripButton toolStripButtonReset;
        private System.Windows.Forms.ToolStripSeparator toolStripSeparator2;
        private System.Windows.Forms.ToolStripButton toolStripButtonDefaultList;

    }
}
