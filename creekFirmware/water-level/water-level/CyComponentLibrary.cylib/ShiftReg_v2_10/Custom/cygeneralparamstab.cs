/*******************************************************************************
* Copyright 2008-2012, Cypress Semiconductor Corporation.  All rights reserved.
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

namespace ShiftReg_v2_10
{
    public partial class CyGeneralParamsTab : UserControl, ICyParamEditingControl
    {
        CySRParameters m_inst;        

        public CyGeneralParamsTab()
        {
            InitializeComponent();
            for (int i = 2; i <= 32; i++)
                cbLength.Items.Add(i);
            
            Dock = DockStyle.Fill;

            //Assign SetParameters Action to all editable controls in the form            
            CySRParameters.AssignEventHandler(this, new EventHandler(SetParameters));
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

            ICyInstEdit_v1 edit = m_inst.m_inst;

            if (edit != null)
                foreach (string paramName in edit.GetParamNames())
                {
                    CyCompDevParam param = edit.GetCommittedParam(paramName);
                    if (param.IsVisible && CySRParameters.P_GENERAL_PARMETERS_TAB_NAME == param.TabName)
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

        public void GetParameters(CySRParameters inst)
        {
            this.m_inst = inst;

            CyCBConverter.SetValue(cbLength, m_inst.m_Length.ToString());
            cbDir.SelectedIndex = m_inst.m_Direction;

            cbShiftIn.Checked = m_inst.m_UseShiftIn;
            cbShiftOut.Checked = m_inst.m_UseShiftOut;
            cbLoad.Checked = m_inst.m_UseInputFifo;
            cbStore.Checked = m_inst.m_UseOutputFifo;
            cbUseInterrupt.Checked = m_inst.m_UseInterrupt;

            if (m_inst.m_DefSi == 1) rbsi1.Checked = true;
            else rbsi0.Checked = true;

            if ((m_inst.m_InterruptSource & 1) == 1) cb_int_onLoad.Checked = true;
            if ((m_inst.m_InterruptSource & 2) == 2) cb_int_st.Checked = true;
            if ((m_inst.m_InterruptSource & 4) == 4) cb_int_reset.Checked = true;
        }

        void SetParameters(object sender, EventArgs e)
        {
            if (CySRParameters.GLOBAL_EDIT_MODE)
                if (m_inst != null)
                {
                    m_inst.m_Length = Convert.ToInt32(CyCBConverter.GetValue(cbLength));
                    m_inst.m_Direction = cbDir.SelectedIndex;

                    m_inst.m_UseShiftIn = cbShiftIn.Checked;
                    m_inst.m_UseShiftOut = cbShiftOut.Checked;
                    m_inst.m_UseInputFifo = cbLoad.Checked;
                    m_inst.m_UseOutputFifo = cbStore.Checked;
                    m_inst.m_UseInterrupt = cbUseInterrupt.Checked;

                    m_inst.m_DefSi = 0;
                    if (rbsi1.Checked) m_inst.m_DefSi = 1;

                    m_inst.m_InterruptSource = 0;
                    if (cb_int_onLoad.Checked) m_inst.m_InterruptSource += 1;
                    if (cb_int_st.Checked) m_inst.m_InterruptSource += 2;
                    if (cb_int_reset.Checked) m_inst.m_InterruptSource += 4; ;

                    m_inst.SetCommitParams();
                }
        }

        private void cbShiftIn_CheckedChanged(object sender, EventArgs e)
        {
            if ((cbShiftIn.Checked == false) && (cbShiftOut.Checked == false))
                ((CheckBox)sender).Checked = true;
            gbShIn.Enabled = !cbShiftIn.Checked;
        }        

        private void cbInterrupt_CheckedChanged(object sender, EventArgs e)
        {
            gbInter.Enabled = cbUseInterrupt.Checked;
        }
    }
}
