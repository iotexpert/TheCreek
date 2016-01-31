/*******************************************************************************
* Copyright 2008-2011, Cypress Semiconductor Corporation.  All rights reserved.
* You may use this file only in accordance with the license, terms, conditions, 
* disclaimers, and limitations in the end user license agreement accompanying 
* the software package with which this file was provided.
********************************************************************************/

using System;
using System.Collections.Generic;
using System.Text;
using CyDesigner.Extensions.Common;
using CyDesigner.Extensions.Gde;
using System.Windows.Forms;

namespace ShiftReg_v2_0
{
    #region SRParameters

    public class CySRParameters
    {
        public const string P_LENGTH = "Length";
        const string P_DIRECTION = "Direction";
        const string P_USE_SHIFT_IN = "UseShiftIn";
        const string P_USE_SHIFT_OUT = "UseShiftOut";
        const string P_USE_INPUT_FIFO = "UseInputFifo";
        const string P_USE_OUTPUT_FIFO = "UseOutputFifo";
        const string P_USE_INTERRUPT = "UseInterrupt";
        const string P_DEF_SI = "DefSi";
        const string P_INTERRUPT_SOURCE = "InterruptSource";

        public const string P_GENERAL_PARMETERS_TAB_NAME = "Basic";

        public int m_Length = 0;
        public int m_Direction = 0;
        public bool m_UseShiftIn = false;
        public bool m_UseShiftOut = false;
        public bool m_UseInputFifo = false;
        public bool m_UseOutputFifo = false;
        public bool m_UseInterrupt = false;
        public int m_DefSi = 0;
        public int m_InterruptSource = 0;

        public ICyInstEdit_v1 m_inst;
        public CyGeneralParamsTab m_generalParamsTab;

        public static bool GLOBAL_EDIT_MODE = false;

        public CySRParameters(ICyInstEdit_v1 inst)
        {
            this.m_inst = inst;
        }
        private void GetBool(ICyInstQuery_v1 inst, string paramName, CyCompDevParam param, ref bool value)
        {
            bool res;
            Boolean.TryParse(inst.GetCommittedParam(paramName).Value, out res);
            if (CheckParamName(paramName, param))
            {
                value = res;
            }
        }
        private void GetInt32(ICyInstQuery_v1 inst, string paramName, CyCompDevParam param, ref Int32 value)
        {
            Int32 res;
            Int32.TryParse(inst.GetCommittedParam(paramName).Value, out res);
            if (CheckParamName(paramName, param))
            {
                value = res;
            }
        }
        bool CheckParamName(string paramName, CyCompDevParam param)
        {
            //Check if thre are errorsin parameter
            if ((param != null) && (param.ErrorCount > 0))
                return false;

            return ((param == null) || (param.Name == paramName));
        }
        public void GetParams(ICyInstEdit_v1 inst, CyCompDevParam param)
        {
            this.m_inst = inst;
            if (inst != null)
            {
                GetInt32(inst, P_LENGTH, param, ref m_Length);
                GetInt32(inst, P_DIRECTION, param, ref m_Direction);

                GetBool(inst, P_USE_SHIFT_IN, param, ref m_UseShiftIn);
                GetBool(inst, P_USE_SHIFT_OUT, param, ref m_UseShiftOut);
                GetBool(inst, P_USE_INPUT_FIFO, param, ref m_UseInputFifo);
                GetBool(inst, P_USE_OUTPUT_FIFO, param, ref m_UseOutputFifo);
                GetBool(inst, P_USE_INTERRUPT, param, ref m_UseInterrupt);

                GetInt32(inst, P_DEF_SI, param, ref m_DefSi);
                GetInt32(inst, P_INTERRUPT_SOURCE, param, ref m_InterruptSource);

                m_generalParamsTab.GetParameters(this);

                GLOBAL_EDIT_MODE = true;
            }
        }

        public void SetParams(ICyInstEdit_v1 inst)
        {
            if (inst != null)
            {
                inst.SetParamExpr(P_LENGTH, m_Length.ToString());
                inst.SetParamExpr(P_DIRECTION, m_Direction.ToString());

                inst.SetParamExpr(P_USE_SHIFT_IN, m_UseShiftIn.ToString());
                inst.SetParamExpr(P_USE_SHIFT_OUT, m_UseShiftOut.ToString());
                inst.SetParamExpr(P_USE_INPUT_FIFO, m_UseInputFifo.ToString());
                inst.SetParamExpr(P_USE_OUTPUT_FIFO, m_UseOutputFifo.ToString());
                inst.SetParamExpr(P_USE_INTERRUPT, m_UseInterrupt.ToString());

                inst.SetParamExpr(P_DEF_SI, m_DefSi.ToString());
                inst.SetParamExpr(P_INTERRUPT_SOURCE, m_InterruptSource.ToString());
            }
        }

        public void SetCommitParams()
        {
            if (GLOBAL_EDIT_MODE)
            {
                SetParams(m_inst);
                m_inst.CommitParamExprs();
            }
        }
        /// <summary>
        /// Assign Action to all editable controls in the container control  
        /// </summary>
        /// <param name="control"></param>
        public static void AssignEventHandler(Control control, EventHandler handler)
        {
            for (int i = 0; i < control.Controls.Count; i++)
            {
                Control cnt = control.Controls[i];

                //Go insight component
                if ((cnt.GetType() == typeof(Panel)) || (cnt.GetType() == typeof(GroupBox)))
                    AssignEventHandler(cnt, handler);

                if (cnt.GetType() == typeof(TextBox))
                    ((TextBox)cnt).TextChanged += handler;
                if (cnt.GetType() == typeof(ComboBox))
                    ((ComboBox)cnt).TextChanged += handler;
                if (cnt.GetType() == typeof(CheckBox))
                    ((CheckBox)cnt).CheckedChanged += handler;
                if (cnt.GetType() == typeof(RadioButton))
                    ((RadioButton)cnt).CheckedChanged += handler;
            }
        }
    }
    #endregion

    //Class which helps to work with ComboBox
    public static class CyCBConverter
    {
        public static string GetValue(ComboBox ff)
        {
            if (ff.SelectedIndex == -1)
            {
                return ff.Text;
            }
            return ff.Items[ff.SelectedIndex].ToString();
        }
        public static void SetValue(ComboBox ff, string str)
        {
            for (int i = 0; i < ff.Items.Count; i++)

                if (ff.Items[i].ToString() == str)
                {
                    ff.SelectedIndex = i;
                    return;
                }
        }
    }
}