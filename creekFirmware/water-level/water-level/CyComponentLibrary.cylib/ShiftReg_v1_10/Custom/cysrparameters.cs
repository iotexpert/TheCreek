/*******************************************************************************
* Copyright 2008-2009, Cypress Semiconductor Corporation.  All rights reserved.
* You may use this file only in accordance with the license, terms, conditions, 
* disclaimers, and limitations in the end user license agreement accompanying 
* the software package with which this file was provided.
********************************************************************************/

using System;
using System.Collections.Generic;
using System.Text;
using CyDesigner.Extensions.Gde;
using System.Windows.Forms;

namespace ShiftReg_v1_10
{
    #region SRParameters

    public class CySRParameters
    {
        ICyInstEdit_v1 m_inst;
        public CyGeneralParamsTab m_GeneralParams;
        bool m_firstStart = true;

        public CySRParameters(ICyInstEdit_v1 inst)
        {
            this.m_inst = inst;

        }
        public void GetParams(ICyInstEdit_v1 inst)
        {
            m_GeneralParams.cbLength.SelectedIndex = Convert.ToInt32(inst.GetCommittedParam("Length").Value) - 1;
            m_GeneralParams.cbDir.SelectedIndex = Convert.ToInt32(inst.GetCommittedParam("Direction").Value);
            cyCBConverter.cySetValue(m_GeneralParams.cbFIFOS, inst.GetCommittedParam("FifoSize").Value.ToString());

            m_GeneralParams.cbShiftIn.Checked = Convert.ToBoolean(inst.GetCommittedParam("UseShiftIn").Value);
            m_GeneralParams.cbShiftOut.Checked = Convert.ToBoolean(inst.GetCommittedParam("UseShiftOut").Value);
            m_GeneralParams.cbLoad.Checked = Convert.ToBoolean(inst.GetCommittedParam("UseInputFifo").Value);
            m_GeneralParams.cbStore.Checked = Convert.ToBoolean(inst.GetCommittedParam("UseOutputFifo").Value);
            m_GeneralParams.cbInterrupt.Checked = Convert.ToBoolean(inst.GetCommittedParam("UseInterrupt").Value);

            if (Convert.ToInt32(inst.GetCommittedParam("DefSi").Value) == 1) m_GeneralParams.rbsi1.Checked = true;
            else m_GeneralParams.rbsi0.Checked = true;

            int intsor = Convert.ToInt32(inst.GetCommittedParam("InterruptSource").Value);
            if ((intsor & 1) == 1) m_GeneralParams.cb_int_onLoad.Checked = true;
            if ((intsor & 2) == 2) m_GeneralParams.cb_int_st.Checked = true;
            if ((intsor & 4) == 4) m_GeneralParams.cb_int_reset.Checked = true;

            m_firstStart = false;
        }

        public void SetParam(ICyInstEdit_v1 inst, string ParamName, string value)
        {
        }

        public void SetParams(ICyInstEdit_v1 inst)
        {
            if (inst != null)
            {
                ////Interapt Tab params
                inst.SetParamExpr("UseShiftIn", m_GeneralParams.cbShiftIn.Checked.ToString());

                inst.SetParamExpr("Length", (m_GeneralParams.cbLength.SelectedIndex + 1).ToString());
                inst.SetParamExpr("Direction", m_GeneralParams.cbDir.SelectedIndex.ToString());
                inst.SetParamExpr("FifoSize", cyCBConverter.cyGetValue(m_GeneralParams.cbFIFOS));

                inst.SetParamExpr("UseShiftIn", (m_GeneralParams.cbShiftIn.Checked).ToString());
                inst.SetParamExpr("UseShiftOut", (m_GeneralParams.cbShiftOut.Checked).ToString());
                inst.SetParamExpr("UseInputFifo", (m_GeneralParams.cbLoad.Checked).ToString());
                inst.SetParamExpr("UseOutputFifo", (m_GeneralParams.cbStore.Checked).ToString());
                inst.SetParamExpr("UseInterrupt", (m_GeneralParams.cbInterrupt.Checked).ToString());

                int DefSi = 0;
                if (m_GeneralParams.rbsi1.Checked) DefSi = 1;
                inst.SetParamExpr("DefSi", DefSi.ToString());

                int intsor = 0;
                if (m_GeneralParams.cb_int_onLoad.Checked) intsor += 1;
                if (m_GeneralParams.cb_int_st.Checked) intsor += 2;
                if (m_GeneralParams.cb_int_reset.Checked) intsor += 4;

                inst.SetParamExpr("InterruptSource", intsor.ToString());
            }
        }

        public void CommitParams(ICyInstEdit_v1 inst)
        {
            //inst.CommitParamExprs();

            if (!inst.CommitParamExprs())
            {
                if (inst.GetCommittedParam("UseShiftIn").ErrorCount > 0)
                { MessageBox.Show("Error in Committing UseShiftIn"); return; }
                if (inst.GetCommittedParam("Length").ErrorCount > 0)
                { MessageBox.Show("Error in Committing Length"); return; }
                MessageBox.Show("Error in Committing Parameters");

            }
        }
        public void SetCommitParams(object sender, EventArgs e)
        {
            if (!m_firstStart)
            {
                SetParams(m_inst);
                CommitParams(m_inst);
            }
        }
        public void AssigneComit(Control cnt)
        {

            if (cnt.GetType() == typeof(TextBox)) ((TextBox)cnt).TextChanged += new EventHandler(SetCommitParams);
            if (cnt.GetType() == typeof(ComboBox)) ((ComboBox)cnt).TextChanged += new EventHandler(SetCommitParams);
            if (cnt.GetType() == typeof(CheckBox)) ((CheckBox)cnt).CheckedChanged += new EventHandler(SetCommitParams);

        }

    }
    #endregion

    public static class cyCBConverter
    {
        public static string cyGetValue(ComboBox ff)
        {
            if (ff.SelectedIndex == -1)
            {
                return ff.Text;
            }
            return ff.Items[ff.SelectedIndex].ToString();
        }
        public static void cySetValue(ComboBox ff, string str)
        {
            for (int i = 0; i < ff.Items.Count; i++)

                if (ff.Items[i].ToString() == str)
                {
                    ff.SelectedIndex = i;
                    return;
                }
            //return ff.Items[ff.SelectedIndex].ToString();
        }
    }

}
