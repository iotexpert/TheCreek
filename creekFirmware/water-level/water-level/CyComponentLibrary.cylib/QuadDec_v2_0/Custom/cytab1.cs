/*******************************************************************************
* Copyright 2008-2011, Cypress Semiconductor Corporation.  All rights reserved.
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

namespace QuadDec_v2_0
{
    class CyTab1 : UserControl, ICyParamEditingControl
    {    
        private CyParameters m_params;

        public CyTab1(CyParameters newParams)
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
                    if (param.IsVisible && Properties.Resources.TabCounterSize.Contains(param.TabName))
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
            for (int i = 0; i < Controls.Count; i++)
            {
                if (Controls[i] is RadioButton)
                {
                    RadioButton currBox = (RadioButton)Controls[i];
                    currBox.Checked = (currBox.Name == "bit" + m_params.CounterSize.ToString() + "RB");
                }
            }
        }

        private byte GetCheckboxes()
        {
            byte result = 0;
            if (bit8RB.Checked) { result = 8; }
            if (bit16RB.Checked) { result = 16; }
            if (bit32RB.Checked) { result = 32; }
            return result;
        }

        private void bitRB_CheckedChanged(object sender, EventArgs e)
        {
            m_params.CounterSize = GetCheckboxes();
        }

        #region Component Designer generated code

        private RadioButton bit8RB;
        private RadioButton bit16RB;
        private Label descriptionLabel;
        private RadioButton bit32RB;

        private void InitializeComponent()
        {
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(CyTab1));
            this.bit8RB = new System.Windows.Forms.RadioButton();
            this.bit16RB = new System.Windows.Forms.RadioButton();
            this.bit32RB = new System.Windows.Forms.RadioButton();
            this.descriptionLabel = new System.Windows.Forms.Label();
            this.SuspendLayout();
            // 
            // bit8RB
            // 
            this.bit8RB.AutoSize = true;
            this.bit8RB.Location = new System.Drawing.Point(25, 51);
            this.bit8RB.Name = "bit8RB";
            this.bit8RB.Size = new System.Drawing.Size(117, 17);
            this.bit8RB.TabIndex = 0;
            this.bit8RB.TabStop = true;
            this.bit8RB.Text = "8 bit: (-128 to +127)";
            this.bit8RB.UseVisualStyleBackColor = true;
            this.bit8RB.CheckedChanged += new System.EventHandler(this.bitRB_CheckedChanged);
            // 
            // bit16RB
            // 
            this.bit16RB.AutoSize = true;
            this.bit16RB.Location = new System.Drawing.Point(25, 74);
            this.bit16RB.Name = "bit16RB";
            this.bit16RB.Size = new System.Drawing.Size(150, 17);
            this.bit16RB.TabIndex = 1;
            this.bit16RB.TabStop = true;
            this.bit16RB.Text = "16 bit (-32,768 to +32,767)";
            this.bit16RB.UseVisualStyleBackColor = true;
            this.bit16RB.CheckedChanged += new System.EventHandler(this.bitRB_CheckedChanged);
            // 
            // bit32RB
            // 
            this.bit32RB.AutoSize = true;
            this.bit32RB.Location = new System.Drawing.Point(25, 97);
            this.bit32RB.Name = "bit32RB";
            this.bit32RB.Size = new System.Drawing.Size(222, 17);
            this.bit32RB.TabIndex = 2;
            this.bit32RB.TabStop = true;
            this.bit32RB.Text = "32 bit (-2,147,483,648 to +2,147,483,647)";
            this.bit32RB.UseVisualStyleBackColor = true;
            this.bit32RB.CheckedChanged += new System.EventHandler(this.bitRB_CheckedChanged);
            // 
            // descriptionLabel
            // 
            this.descriptionLabel.Location = new System.Drawing.Point(3, 3);
            this.descriptionLabel.Name = "descriptionLabel";
            this.descriptionLabel.Size = new System.Drawing.Size(475, 49);
            this.descriptionLabel.TabIndex = 3;
            this.descriptionLabel.Text = resources.GetString("descriptionLabel.Text");
            // 
            // CyTab1
            // 
            this.AutoScroll = true;
            this.AutoSize = true;
            this.Controls.Add(this.descriptionLabel);
            this.Controls.Add(this.bit32RB);
            this.Controls.Add(this.bit16RB);
            this.Controls.Add(this.bit8RB);
            this.Name = "CyTab1";
            this.Size = new System.Drawing.Size(550, 317);
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion
    }
}
