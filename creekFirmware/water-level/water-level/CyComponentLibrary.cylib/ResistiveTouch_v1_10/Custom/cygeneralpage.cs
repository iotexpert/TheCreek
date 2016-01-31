/*******************************************************************************
* Copyright 2011-2012, Cypress Semiconductor Corporation.  All rights reserved.
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
using CyDesigner.Extensions.Gde;
using CyDesigner.Extensions.Common;

namespace ResistiveTouch_v1_10
{
    public partial class CyGeneralPage : CyEditingWrapperControl
    {
        public CyGeneralPage()
        {
            InitializeComponent();
        }

        public CyGeneralPage(CyParameters parameters)
        {
            InitializeComponent();
            m_parameters = parameters;
            InitFields();
        }

        public void InitFields()
        {
            rbDeltaSigma.Checked = (m_parameters.SarAdc == CyParameters.CyEResistiveTouchADCSelType.DelSig_ADC);
            rbSAR.Checked = (m_parameters.SarAdc == CyParameters.CyEResistiveTouchADCSelType.SAR_ADC);
        }

        private void rbADC_CheckedChanged(object sender, EventArgs e)
        {
            RadioButton rb = (RadioButton)sender;
            if (rb.Checked)
            {
                if (rb == rbSAR)
                    m_parameters.SarAdc = CyParameters.CyEResistiveTouchADCSelType.SAR_ADC;
                else
                    m_parameters.SarAdc = CyParameters.CyEResistiveTouchADCSelType.DelSig_ADC;

                errProvider.SetError(rbSAR, (m_parameters.m_inst.DeviceQuery.IsPSoC3
                    && m_parameters.SarAdc == CyParameters.CyEResistiveTouchADCSelType.SAR_ADC) ?
                    Properties.Resources.ErrSARvsPSoC3 : "");
            }
        }

        public override IEnumerable<CyCustErr> GetErrors()
        {
            List<CyCustErr> errorList = new List<CyCustErr>();
            errorList.AddRange(base.GetErrors());
            if (m_parameters.m_inst.DeviceQuery.IsPSoC3 
                && m_parameters.SarAdc == CyParameters.CyEResistiveTouchADCSelType.SAR_ADC)
            {
                errorList.Add(new CyCustErr(Properties.Resources.ErrSARvsPSoC3));
            }
            return errorList.ToArray();
        }
    }
}
