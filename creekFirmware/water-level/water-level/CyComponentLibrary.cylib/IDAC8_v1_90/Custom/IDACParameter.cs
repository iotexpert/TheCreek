/*******************************************************************************
* Copyright 2008-2012, Cypress Semiconductor Corporation.  All rights reserved.
* You may use this file only in accordance with the license, terms, conditions, 
* disclaimers, and limitations in the end user license agreement accompanying 
* the software package with which this file was provided.
********************************************************************************/

using System;
using System.Collections.Generic;
using System.Text;
using CyDesigner.Extensions.Gde;

namespace IDAC8_v1_90
{
   public class CyIDACParameters
    {
        public CyCompDevParam m_comp_IDAC_Polarity = null;
        public CyCompDevParam m_comp_IDAC_Range = null;
        public CyCompDevParam m_comp_Initial_Value = null;
        public CyCompDevParam m_comp_Hex = null;
        public CyCompDevParam m_comp_Byte = null;
        public CyCompDevParam m_comp_Current = null;
        public CyCompDevParam m_comp_Data_Source = null;
        public CyCompDevParam m_comp_IDAC_Speed = null;
        public CyCompDevParam m_comp_Hardware_Enable = null;
        public CyCompDevParam m_comp_Strobe_Mode = null;

        public CyIDACParameters(ICyInstEdit_v1 inst)
        {
            GetParams(inst);
        }

        //Get parameter values
        private void GetParams(ICyInstEdit_v1 inst)
        {
            m_comp_IDAC_Polarity = inst.GetCommittedParam("Polarity");
            m_comp_IDAC_Range = inst.GetCommittedParam("IDAC_Range");
            m_comp_Initial_Value = inst.GetCommittedParam("Initial_Value");
            m_comp_Current = inst.GetCommittedParam("Current");
            m_comp_Hardware_Enable = inst.GetCommittedParam("Hardware_Enable");
            m_comp_Hex = inst.GetCommittedParam("Initial_Value");
            m_comp_Data_Source = inst.GetCommittedParam("Data_Source");
            m_comp_IDAC_Speed = inst.GetCommittedParam("IDAC_Speed");
            m_comp_Strobe_Mode = inst.GetCommittedParam("Strobe_Mode");
        }
    }
}


//[] END OF FILE
