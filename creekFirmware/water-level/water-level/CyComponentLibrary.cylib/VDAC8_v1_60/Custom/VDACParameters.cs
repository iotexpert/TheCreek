using System;
using System.Collections.Generic;
using System.Text;
using CyDesigner.Extensions.Gde;

namespace VDAC8_v1_60
{
    public class VDACParameters
    {
        public CyCompDevParam Comp_VDAC_Range = null;
        public CyCompDevParam Comp_VDAC_Speed = null;
        public CyCompDevParam Comp_Initial_Value = null;
        public CyCompDevParam Comp_Voltage= null;
        public CyCompDevParam Comp_Byte = null;
        public CyCompDevParam Comp_Data_Source = null;
        public CyCompDevParam Comp_Strobe_Mode = null;

        public VDACParameters(ICyInstEdit_v1 inst)
        {
            GetParams(inst);
        }

        //Get parameter values
        private void GetParams(ICyInstEdit_v1 inst)
        {
            Comp_VDAC_Range = inst.GetCommittedParam("VDAC_Range");
            Comp_VDAC_Speed = inst.GetCommittedParam("VDAC_Speed");
            Comp_Initial_Value = inst.GetCommittedParam("Initial_Value");
            Comp_Voltage = inst.GetCommittedParam("Voltage");
            Comp_Byte = inst.GetCommittedParam("Initial_Value");
            Comp_Data_Source = inst.GetCommittedParam("Data_Source");
            Comp_Strobe_Mode = inst.GetCommittedParam("Strobe_Mode");
        }

    }
}
