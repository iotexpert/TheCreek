/*******************************************************************************
* Copyright 2008-2001, Cypress Semiconductor Corporation.  All rights reserved.
* You may use this file only in accordance with the license, terms, conditions, 
* disclaimers, and limitations in the end user license agreement accompanying 
* the software package with which this file was provided.
********************************************************************************/


using System;
using System.Collections.Generic;
using System.Text;
using CyDesigner.Extensions.Gde;

namespace VDAC8_v1_70
{
    // To retrieve the parameter from symbol.
    public class CyVDACParameters
    {
        #region Member variables.
        public CyCompDevParam m_Comp_VDAC_Range = null;
        public CyCompDevParam m_Comp_VDAC_Speed = null;
        public CyCompDevParam m_Comp_Initial_Value = null;
        public CyCompDevParam m_Comp_Voltage= null;
        public CyCompDevParam m_Comp_Byte = null;
        public CyCompDevParam m_Comp_Data_Source = null;
        public CyCompDevParam m_Comp_Strobe_Mode = null;
        #endregion

        
        public CyVDACParameters(ICyInstEdit_v1 inst)
        {
            GetParams(inst);
        }

        //Get parameter values
        private void GetParams(ICyInstEdit_v1 inst)
        {
            m_Comp_VDAC_Range = inst.GetCommittedParam("VDAC_Range");
            m_Comp_VDAC_Speed = inst.GetCommittedParam("VDAC_Speed");
            m_Comp_Initial_Value = inst.GetCommittedParam("Initial_Value");
            m_Comp_Voltage = inst.GetCommittedParam("Voltage");
            m_Comp_Byte = inst.GetCommittedParam("Initial_Value");
            m_Comp_Data_Source = inst.GetCommittedParam("Data_Source");
            m_Comp_Strobe_Mode = inst.GetCommittedParam("Strobe_Mode");
        }

    }
}
