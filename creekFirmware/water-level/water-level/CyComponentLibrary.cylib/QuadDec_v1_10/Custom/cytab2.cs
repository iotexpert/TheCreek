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
    class CyTab2 : UserControl, ICyParamEditingControl
    {
        private CyParameters m_params;

        public CyTab2()
        {
            InitializeComponent();
        }

        public CyTab2(CyParameters newParams)
        {
            InitializeComponent();

            m_params = newParams;
            this.Dock = DockStyle.Fill;
            SetCheckboxes(m_params.CountRes);
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

        public void SetCheckboxes(byte countRes)
        {
            switch (countRes)
            {
                case (1):
                    {
                        x1RB.Checked = true;
                        x2RB.Checked = false;
                        x4RB.Checked = false;
                        break;
                    }
                case (2):
                    {
                        x1RB.Checked = false;
                        x2RB.Checked = true;
                        x4RB.Checked = false;
                        break;
                    }
                case (4):
                    {
                        x1RB.Checked = false;
                        x2RB.Checked = false;
                        x4RB.Checked = true;
                        break;
                    }
                default:
                    {
                        throw new Exception("Illegal value");
                    }
            }

            cyDiagram1.CounterResolution = countRes;
        }

        public byte GetCheckboxes()
        {
            byte result = 0;
            if (x1RB.Checked)
            {
                result = 1;
            }
            if (x2RB.Checked)
            {
                result = 2;
            }
            if (x4RB.Checked)
            {
                result = 4;
            }
            cyDiagram1.CounterResolution = result;

            return result;
        }      

        private void xRB_CheckedChanged(object sender, EventArgs e)
        {
            m_params.CountRes = GetCheckboxes();
        }

        #region Auto-generated code

        private Label descriptionLabel;
        private RadioButton x1RB;
        private RadioButton x2RB;
        private RadioButton x4RB;
        private CyDiagram cyDiagram1;

        private void InitializeComponent()
        {
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(CyTab2));
            this.descriptionLabel = new System.Windows.Forms.Label();
            this.x1RB = new System.Windows.Forms.RadioButton();
            this.x2RB = new System.Windows.Forms.RadioButton();
            this.x4RB = new System.Windows.Forms.RadioButton();
            this.cyDiagram1 = new QuadDec_v1_10.CyDiagram();
            this.SuspendLayout();
            // 
            // descriptionLabel
            // 
            this.descriptionLabel.Location = new System.Drawing.Point(3, 3);
            this.descriptionLabel.Name = "descriptionLabel";
            this.descriptionLabel.Size = new System.Drawing.Size(475, 58);
            this.descriptionLabel.TabIndex = 1;
            this.descriptionLabel.Text = resources.GetString("descriptionLabel.Text");
            // 
            // x1RB
            // 
            this.x1RB.AutoSize = true;
            this.x1RB.Location = new System.Drawing.Point(25, 51);
            this.x1RB.Name = "x1RB";
            this.x1RB.Size = new System.Drawing.Size(36, 17);
            this.x1RB.TabIndex = 2;
            this.x1RB.TabStop = true;
            this.x1RB.Text = "1x";
            this.x1RB.UseVisualStyleBackColor = true;
            this.x1RB.CheckedChanged += new System.EventHandler(this.xRB_CheckedChanged);
            // 
            // x2RB
            // 
            this.x2RB.AutoSize = true;
            this.x2RB.Location = new System.Drawing.Point(83, 51);
            this.x2RB.Name = "x2RB";
            this.x2RB.Size = new System.Drawing.Size(36, 17);
            this.x2RB.TabIndex = 3;
            this.x2RB.TabStop = true;
            this.x2RB.Text = "2x";
            this.x2RB.UseVisualStyleBackColor = true;
            this.x2RB.CheckedChanged += new System.EventHandler(this.xRB_CheckedChanged);
            // 
            // x4RB
            // 
            this.x4RB.AutoSize = true;
            this.x4RB.Location = new System.Drawing.Point(141, 51);
            this.x4RB.Name = "x4RB";
            this.x4RB.Size = new System.Drawing.Size(36, 17);
            this.x4RB.TabIndex = 4;
            this.x4RB.TabStop = true;
            this.x4RB.Text = "4x";
            this.x4RB.UseVisualStyleBackColor = true;
            this.x4RB.CheckedChanged += new System.EventHandler(this.xRB_CheckedChanged);
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
            this.cyDiagram1.Name = "cyDiagram1";
            this.cyDiagram1.Size = new System.Drawing.Size(475, 173);
            this.cyDiagram1.TabIndex = 9;
            this.cyDiagram1.UseFiltering = false;
            this.cyDiagram1.UseIndex = false;
            // 
            // CyTab2
            // 
            this.AutoScroll = true;
            this.Controls.Add(this.cyDiagram1);
            this.Controls.Add(this.x4RB);
            this.Controls.Add(this.x2RB);
            this.Controls.Add(this.x1RB);
            this.Controls.Add(this.descriptionLabel);
            this.Name = "CyTab2";
            this.Size = new System.Drawing.Size(482, 251);
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion
    }   
}
