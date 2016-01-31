/*******************************************************************************
* Copyright 2008-2009, Cypress Semiconductor Corporation.  All rights reserved.
* You may use this file only in accordance with the license, terms, conditions, 
* disclaimers, and limitations in the end user license agreement accompanying 
* the software package with which this file was provided.
********************************************************************************/

using System;
using System.Collections.Generic;
using System.Text;
using System.Windows.Forms;
using CyDesigner.Extensions.Common;
using CyDesigner.Extensions.Gde;
using System.ComponentModel;
using System.Drawing;

namespace QuadDec_v1_10
{
    class CyTab3 : UserControl, ICyParamEditingControl
    {
        private CyParameters m_params;

        public CyTab3()
        {
            InitializeComponent();
        }

        public CyTab3(CyParameters newParams)
        {
            InitializeComponent();

            m_params = newParams;
            this.Dock = DockStyle.Fill;
            SetCheckbox(m_params.UseIndex);
            cyDiagram1.CounterResolution = m_params.CountRes;
        }

        #region ICyParamEditingControl Members

        public Control DisplayControl
        {
            get { return this; }
        }

        public IEnumerable<CyCustErr> GetErrors()
        {
            return new CyCustErr[] { };
        }

        #endregion

       

        public void SetCheckbox(bool isUseIndex)
        {
            useIndexInputChB.Checked = isUseIndex;
            cyDiagram1.UseIndex = isUseIndex;
        }

        public bool GetCheckbox()
        {
            cyDiagram1.UseIndex = useIndexInputChB.Checked;
            return useIndexInputChB.Checked;
        }      

        private void useIndexInputChB_CheckedChanged(object sender, EventArgs e)
        {
            m_params.UseIndex= GetCheckbox();
        }

        private void CyTab3_VisibleChanged(object sender, EventArgs e)
        {
            cyDiagram1.CounterResolution = m_params.CountRes;
        }

        #region Auto-generated code

        private Label descriptionLabel;
        private CyDiagram cyDiagram1;
        private CheckBox useIndexInputChB;

        private void InitializeComponent()
        {
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(CyTab3));
            this.descriptionLabel = new System.Windows.Forms.Label();
            this.useIndexInputChB = new System.Windows.Forms.CheckBox();
            this.cyDiagram1 = new QuadDec_v1_10.CyDiagram();
            this.SuspendLayout();
            // 
            // descriptionLabel
            // 
            this.descriptionLabel.Location = new System.Drawing.Point(3, 3);
            this.descriptionLabel.Name = "descriptionLabel";
            this.descriptionLabel.Size = new System.Drawing.Size(475, 75);
            this.descriptionLabel.TabIndex = 1;
            this.descriptionLabel.Text = resources.GetString("descriptionLabel.Text");
            // 
            // useIndexInputChB
            // 
            this.useIndexInputChB.AutoSize = true;
            this.useIndexInputChB.Location = new System.Drawing.Point(25, 51);
            this.useIndexInputChB.Name = "useIndexInputChB";
            this.useIndexInputChB.Size = new System.Drawing.Size(99, 17);
            this.useIndexInputChB.TabIndex = 2;
            this.useIndexInputChB.Text = "Use index input";
            this.useIndexInputChB.UseVisualStyleBackColor = true;
            this.useIndexInputChB.CheckedChanged += new System.EventHandler(this.useIndexInputChB_CheckedChanged);
            // 
            // cyDiagram1
            // 
            this.cyDiagram1.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom)
                        | System.Windows.Forms.AnchorStyles.Left)
                        | System.Windows.Forms.AnchorStyles.Right)));
            this.cyDiagram1.AutoSize = true;
            this.cyDiagram1.BackColor = System.Drawing.Color.White;
            this.cyDiagram1.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.cyDiagram1.CounterResolution = ((byte)(4));
            this.cyDiagram1.Location = new System.Drawing.Point(3, 74);
            this.cyDiagram1.Margin = new System.Windows.Forms.Padding(5, 5, 5, 5);
            this.cyDiagram1.MinimumSize = new System.Drawing.Size(475, 0);
            this.cyDiagram1.Name = "cyDiagram1";
            this.cyDiagram1.Size = new System.Drawing.Size(475, 173);
            this.cyDiagram1.TabIndex = 3;
            this.cyDiagram1.UseFiltering = false;
            this.cyDiagram1.UseIndex = false;
            // 
            // CyTab3
            // 
            this.AutoScroll = true;
            this.Controls.Add(this.cyDiagram1);
            this.Controls.Add(this.useIndexInputChB);
            this.Controls.Add(this.descriptionLabel);
            this.Name = "CyTab3";
            this.Size = new System.Drawing.Size(482, 251);
            this.VisibleChanged += new System.EventHandler(this.CyTab3_VisibleChanged);
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion


    }
   
}
