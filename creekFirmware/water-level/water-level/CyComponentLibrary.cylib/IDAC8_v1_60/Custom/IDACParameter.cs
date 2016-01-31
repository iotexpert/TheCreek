/* ========================================
 *
 * Copyright YOUR COMPANY, THE YEAR
 * All Rights Reserved
 * UNPUBLISHED, LICENSED SOFTWARE.
 *
 * CONFIDENTIAL AND PROPRIETARY INFORMATION
 * WHICH IS THE PROPERTY OF your company.
 *
 * ========================================
*/

using System;
using System.Collections.Generic;
using System.Text;
using CyDesigner.Extensions.Gde;

namespace IDAC8_v1_60
{
    class IDACParameter
    {
        public CyCompDevParam Comp_IDAC_Polarity = null;
        public CyCompDevParam Comp_IDAC_Range = null;
        public CyCompDevParam Comp_Percentage = null;
        public CyCompDevParam Comp_Initial_Value = null;
        public CyCompDevParam Comp_Hex = null;
        public CyCompDevParam Comp_Byte = null;
        public CyCompDevParam Comp_Current = null;
        public CyCompDevParam Comp_IDAC_Data_Source = null;
        public CyCompDevParam Comp_IDAC_Speed = null;
        public CyCompDevParam Comp_Strobe_Mode = null;

        public IDACParameter(ICyInstEdit_v1 inst)
        {
            GetParams(inst);
        }

        //Get parameter values
        private void GetParams(ICyInstEdit_v1 inst)
        {
            Comp_IDAC_Polarity = inst.GetCommittedParam("Polarity");
            Comp_IDAC_Range = inst.GetCommittedParam("IDAC_Range");
            Comp_Initial_Value = inst.GetCommittedParam("Initial_value");
            Comp_Current = inst.GetCommittedParam("Current");
            Comp_Hex = inst.GetCommittedParam("Initial_Value");
            Comp_Percentage = inst.GetCommittedParam("Current_Percentage");
            Comp_IDAC_Data_Source = inst.GetCommittedParam("Data_Source");
            Comp_IDAC_Speed = inst.GetCommittedParam("IDAC_Speed");
            Comp_Strobe_Mode = inst.GetCommittedParam("Strobe_Mode");
        }
    }

    
}

//[] END OF FILE
