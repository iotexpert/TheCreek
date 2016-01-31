/*******************************************************************************
* Copyright 2008-2009, Cypress Semiconductor Corporation.  All rights reserved.
* You may use this file only in accordance with the license, terms, conditions, 
* disclaimers, and limitations in the end user license agreement accompanying 
* the software package with which this file was provided.
********************************************************************************/



using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Data;
using System.Text;
using System.Windows.Forms;

namespace CapSense_v1_20
{
    public partial class CyGenCSDProperties : CyGenBaseProperties
    {
        public CyGenCSDProperties()
        {
            InitializeComponent();
            m_Method = E_CAPSENSE_MODE.CSD;

            this.cbPRS.Items.Clear();
            this.cbPRS.Items.AddRange(CyIntEnumConverter.
                GetEnumStringList(typeof(E_PRS_OPTIONS)));
            cbPRS.SelectedIndex = 0;
            this.cbReference.Items.Clear();
            this.cbReference.Items.AddRange(CyIntEnumConverter.
                GetEnumStringList(typeof(E_REFERENCE_OPTIONS)));
            cbReference.SelectedIndex = 0;

            pbGraph.SizeMode = PictureBoxSizeMode.Zoom;
            panelGraph.BackColor = Color.White;
            rbIDACSinking.CheckedChanged += new EventHandler(rbIDACSinking_CheckedChanged);
            rbRBEnable.CheckedChanged += new EventHandler(rbRBEnable_CheckedChanged);

            CyGeneralParams.AssingActions(panelMain, new EventHandler(SendProperties));
        }
        void GraphRePaint()
        {
            if (m_cCSHalf != null)
            {
                string strEnd = "";
                string strStart = "";

                //define Image string
                strStart = m_packParam.GetSidePrefix(m_cCSHalf);
                if (m_packParam.Configuration == E_MAIN_CONFIG.emParallelSynchron) strStart = "l";

                if (m_cCSHalf.IsPRS())
                    strEnd = "prs";
                //Assigne Image
                switch (m_cCSHalf.m_csdSubMethod)
                {
                    case E_CSD_SUB_METHODS.IDACSourcing:
                        pbGraph.Image = ((System.Drawing.Image)(
                            global:: CapSense_v1_20.Properties.Resources.ResourceManager.GetObject(strStart + "idacsourcing" + strEnd)));
                        break;
                    case E_CSD_SUB_METHODS.IDACSinking:
                        pbGraph.Image = ((System.Drawing.Image)(
                            global:: CapSense_v1_20.Properties.Resources.ResourceManager.GetObject(strStart + "idacsinking" + strEnd)));
                        break;
                    case E_CSD_SUB_METHODS.IDACDisable_RB:
                        pbGraph.Image = ((System.Drawing.Image)(
                            global:: CapSense_v1_20.Properties.Resources.ResourceManager.GetObject(strStart + "rb" + strEnd)));
                        break;
                    default:
                        break;
                }
            }
        }
        #region Get/Send
        public override void GetProperties(CyAmuxBParams cCSHalf, CyGeneralParams packParam)
        {
            this.m_cCSHalf = null;
            rbRBEnable.Checked = cCSHalf.IsRbAvailible();
            tbSEC.Text = cCSHalf.CountShieldElectrode.ToString();
            CyCBConverter.CySetValue(cbPRS,
                CyIntEnumConverter.GetEnumString((E_PRS_OPTIONS)cCSHalf.m_prs));
            tbRb.Text = cCSHalf.CountRb.ToString();
            CyCBConverter.CySetValue(cbReference, CyIntEnumConverter.GetEnumString(cCSHalf.m_Reference));

            switch (cCSHalf.m_csdSubMethod)
            {
                case E_CSD_SUB_METHODS.IDACSourcing:
                    rbIDACSourcing.Checked = true;
                    break;
                case E_CSD_SUB_METHODS.IDACSinking:
                    rbIDACSinking.Checked = true;
                    break;
                case E_CSD_SUB_METHODS.IDACDisable_RB:
                    rbRBEnable.Checked = true;
                    break;
                default:
                    break;
            }
            this.m_cCSHalf = cCSHalf;
            this.m_packParam = packParam;

            GraphRePaint();
        }
        public override void SendProperties(object sender, EventArgs e)
        {
            if (m_packParam != null)
                if ((m_cCSHalf != null) && (m_packParam.GlobalEditMode))
                {
                    m_cCSHalf.m_prs = (E_PRS_OPTIONS)CyIntEnumConverter.GetEnumValue(CyCBConverter.CyGetValue(cbPRS),
                        typeof(E_PRS_OPTIONS));

                    if (tbSEC.Visible)
                        m_cCSHalf.m_cShieldElectrode.Validate(tbSEC.Text);
                    else
                        m_cCSHalf.m_cShieldElectrode.Validate(0);

                    if (rbIDACSourcing.Checked) m_cCSHalf.m_csdSubMethod = E_CSD_SUB_METHODS.IDACSourcing;
                    else if (rbIDACSinking.Checked) m_cCSHalf.m_csdSubMethod = E_CSD_SUB_METHODS.IDACSinking;
                    else if (rbRBEnable.Checked) m_cCSHalf.m_csdSubMethod = E_CSD_SUB_METHODS.IDACDisable_RB;

                    m_cCSHalf.m_Reference = (E_REFERENCE_OPTIONS)CyIntEnumConverter.GetEnumValue
                        (CyCBConverter.CyGetValue(cbReference), typeof(E_REFERENCE_OPTIONS));

                    //Rb Count Validate
                    if (m_cCSHalf.IsRbAvailible())
                    {
                        m_cCSHalf.m_cRb.Validate(tbRb);
                    }
                    GraphRePaint();

                    //Validate parameters
                    m_packParam.ValidateParameters(m_cCSHalf);
                }
        }
        #endregion

        #region Actions
        //public void ShElVisibil(bool state)
        //{
        //    lSEC.Visible = state;
        //    tbSEC.Visible = state;
        //}
        public void RbVisibil(bool state)
        {
            lRb.Visible = state;
            tbRb.Visible = state;
        }

        private void rbRBEnable_CheckedChanged(object sender, EventArgs e)
        {
            RbVisibil(((RadioButton)sender).Checked);
            //ShElVisibil(((RadioButton)sender).Checked);
        }

        private void rbIDACSinking_CheckedChanged(object sender, EventArgs e)
        {
            //ShElVisibil(((RadioButton)sender).Checked);
        }

        private void tbSEC_Validating(object sender, CancelEventArgs e)
        {
            if (m_cCSHalf != null)
            {
                m_cCSHalf.m_cShieldElectrode.Validate(tbSEC);
            }
        }

        private void tbRb_Validating(object sender, CancelEventArgs e)
        {
            if (m_cCSHalf != null)
            {
                m_cCSHalf.m_cRb.Validate(tbRb);
            }
        }
        #endregion
    }


}
