/*******************************************************************************
* Copyright 2008-2010, Cypress Semiconductor Corporation.  All rights reserved.
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

namespace QuadDec_v1_50
{
    class CyTab3 : UserControl, ICyParamEditingControl
    {
        private CyParameters m_params;

        public CyTab3(CyParameters newParams)
        {
            InitializeComponent();
            m_params = newParams;
            Dock = DockStyle.Fill;
            UpdateFromParam();           
        }

        #region ICyParamEditingControl Members
        public Control DisplayControl
        {
            get { return this; }
        }

        /// <summary>
        /// Gets any errors that exist for parameters on the DisplayControl.
        /// </summary>
        /// <returns></returns>
        public IEnumerable<CyCustErr> GetErrors()
        {
            List<CyCustErr> errs = new List<CyCustErr>();

            ICyInstEdit_v1 edit = m_params.m_instance;

            if (edit != null)
                foreach (string paramName in edit.GetParamNames())
                {
                    CyCompDevParam param = edit.GetCommittedParam(paramName);
                    if (param.IsVisible && Properties.Resources.TabUseIndexInput.Contains(param.TabName))
                    {
                        if (param.ErrorCount > 0)
                        {
                            foreach (string errMsg in param.Errors)
                            {
                                errs.Add(new CyCustErr(errMsg));
                            }
                        }
                    }
                }

            return errs;
        }
        #endregion

        public void UpdateFromParam()
        {
            m_chbUseIndex.Checked = m_params.UseIndex;
            cyDiagram1.CounterResolution = m_params.CounterResolution;
            cyDiagram1.UseIndex = m_params.UseIndex;
        }   

        private void useIndexInputChB_CheckedChanged(object sender, EventArgs e)
        {
            cyDiagram1.UseIndex = m_chbUseIndex.Checked;
            m_params.UseIndex = m_chbUseIndex.Checked;
        }

        #region Component Designer generated code

        private Label descriptionLabel;
        private CyDiagram cyDiagram1;
        private CheckBox m_chbUseIndex;

        private void InitializeComponent()
        {
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(CyTab3));
            this.descriptionLabel = new System.Windows.Forms.Label();
            this.m_chbUseIndex = new System.Windows.Forms.CheckBox();
            this.cyDiagram1 = new QuadDec_v1_50.CyDiagram();
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
            // m_chbUseIndex
            // 
            this.m_chbUseIndex.AutoSize = true;
            this.m_chbUseIndex.Location = new System.Drawing.Point(6, 81);
            this.m_chbUseIndex.Name = "m_chbUseIndex";
            this.m_chbUseIndex.Size = new System.Drawing.Size(124, 21);
            this.m_chbUseIndex.TabIndex = 2;
            this.m_chbUseIndex.Text = "Use index input";
            this.m_chbUseIndex.UseVisualStyleBackColor = true;
            this.m_chbUseIndex.CheckedChanged += new System.EventHandler(this.useIndexInputChB_CheckedChanged);
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
            this.cyDiagram1.Font = new System.Drawing.Font("Microsoft Sans Serif", 11F);
            this.cyDiagram1.Location = new System.Drawing.Point(3, 109);
            this.cyDiagram1.Margin = new System.Windows.Forms.Padding(5);
            this.cyDiagram1.MinimumSize = new System.Drawing.Size(475, 0);
            this.cyDiagram1.Name = "cyDiagram1";
            this.cyDiagram1.Size = new System.Drawing.Size(475, 196);
            this.cyDiagram1.TabIndex = 3;
            this.cyDiagram1.UseFiltering = false;
            this.cyDiagram1.UseIndex = false;
            // 
            // CyTab3
            // 
            this.AutoScroll = true;
            this.Controls.Add(this.cyDiagram1);
            this.Controls.Add(this.m_chbUseIndex);
            this.Controls.Add(this.descriptionLabel);
            this.Name = "CyTab3";
            this.Size = new System.Drawing.Size(482, 309);
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion


    }

}
