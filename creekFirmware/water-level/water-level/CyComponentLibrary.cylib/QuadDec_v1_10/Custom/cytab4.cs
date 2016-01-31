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
    class CyTab4 : UserControl, ICyParamEditingControl
    {
        private CyParameters m_params;

        public CyTab4()
        {
            InitializeComponent();

        }

        public CyTab4(CyParameters newParams)
        {
            InitializeComponent();

            m_params = newParams;
            this.Dock = DockStyle.Fill;
            SetCheckbox(m_params.UseGF);
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

       

       

        public void SetCheckbox(bool isEnabledGF)
        {
            enableChB.Checked = isEnabledGF;
            cyDiagram1.UseFiltering = isEnabledGF;
        }

        public bool GetCheckbox()
        {
            cyDiagram1.UseFiltering = enableChB.Checked; 
            return enableChB.Checked;
        }    

        private void enableChB_CheckedChanged(object sender, EventArgs e)
        {
            m_params.UseGF = GetCheckbox();
        }

        private void Tab4_VisibleChanged(object sender, EventArgs e)
        {
            SetCheckbox(m_params.UseGF);
            cyDiagram1.CounterResolution = m_params.CountRes;
        }


        #region Auto-generated code

        private Label descriptionLabel;
        private CyDiagram cyDiagram1;
        private CheckBox enableChB;

        private void InitializeComponent()
        {
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(CyTab4));
            this.descriptionLabel = new System.Windows.Forms.Label();
            this.enableChB = new System.Windows.Forms.CheckBox();
            this.cyDiagram1 = new QuadDec_v1_10.CyDiagram();
            this.SuspendLayout();
            // 
            // descriptionLabel
            // 
            this.descriptionLabel.Location = new System.Drawing.Point(3, 3);
            this.descriptionLabel.Name = "descriptionLabel";
            this.descriptionLabel.Size = new System.Drawing.Size(463, 58);
            this.descriptionLabel.TabIndex = 1;
            this.descriptionLabel.Text = resources.GetString("descriptionLabel.Text");
            // 
            // enableChB
            // 
            this.enableChB.AutoSize = true;
            this.enableChB.Location = new System.Drawing.Point(25, 51);
            this.enableChB.Name = "enableChB";
            this.enableChB.Size = new System.Drawing.Size(123, 17);
            this.enableChB.TabIndex = 2;
            this.enableChB.Text = "Enable glitch filtering";
            this.enableChB.UseVisualStyleBackColor = true;
            this.enableChB.CheckedChanged += new System.EventHandler(this.enableChB_CheckedChanged);
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
            this.cyDiagram1.Margin = new System.Windows.Forms.Padding(4);
            this.cyDiagram1.MinimumSize = new System.Drawing.Size(475, 0);
            this.cyDiagram1.Name = "cyDiagram1";
            this.cyDiagram1.Size = new System.Drawing.Size(475, 173);
            this.cyDiagram1.TabIndex = 3;
            this.cyDiagram1.UseFiltering = false;
            this.cyDiagram1.UseIndex = false;
            // 
            // CyTab4
            // 
            this.AutoScroll = true;
            this.AutoSizeMode = System.Windows.Forms.AutoSizeMode.GrowAndShrink;
            this.Controls.Add(this.cyDiagram1);
            this.Controls.Add(this.enableChB);
            this.Controls.Add(this.descriptionLabel);
            this.Name = "CyTab4";
            this.Size = new System.Drawing.Size(482, 251);
            this.VisibleChanged += new System.EventHandler(this.Tab4_VisibleChanged);
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

    }
    
}
