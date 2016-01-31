/******************************************************************************
* File Name: IDACParameter.cs
* *****************************************************************************
* Copyright 2008-2011, Cypress Semiconductor Corporation.  All rights reserved.
* You may use this file only in accordance with the license, terms, conditions, 
* disclaimers, and limitations in the end user license agreement accompanying 
* the software package with which this file was provided.
*******************************************************************************/

using System;
using System.Collections.Generic;
using System.Text;
using CyDesigner.Extensions.Gde;

namespace IDAC8_v1_70
{
   public class CyIDACParameters
    {
        public CyCompDevParam m_Comp_IDAC_Polarity = null;
        public CyCompDevParam m_Comp_IDAC_Range = null;
       
        public CyCompDevParam m_Comp_Initial_Value = null;

        public CyCompDevParam m_Comp_Hex = null;
        public CyCompDevParam m_Comp_Byte = null;
        public CyCompDevParam m_Comp_Current = null;
        public CyCompDevParam m_Comp_Data_Source = null;
        public CyCompDevParam m_Comp_IDAC_Speed = null;
        public CyCompDevParam m_Comp_Strobe_Mode = null;

        public CyIDACParameters(ICyInstEdit_v1 inst)
        {
            GetParams(inst);
        }

        //Get parameter values
        private void GetParams(ICyInstEdit_v1 inst)
        {
            m_Comp_IDAC_Polarity = inst.GetCommittedParam("Polarity");
            m_Comp_IDAC_Range = inst.GetCommittedParam("IDAC_Range");
            m_Comp_Initial_Value = inst.GetCommittedParam("Initial_Value");
            m_Comp_Current = inst.GetCommittedParam("Current");

            m_Comp_Hex = inst.GetCommittedParam("Initial_Value");
            m_Comp_Data_Source = inst.GetCommittedParam("Data_Source");
            m_Comp_IDAC_Speed = inst.GetCommittedParam("IDAC_Speed");
            m_Comp_Strobe_Mode = inst.GetCommittedParam("Strobe_Mode");
        }
    }

    
}

//[] END OF FILE
