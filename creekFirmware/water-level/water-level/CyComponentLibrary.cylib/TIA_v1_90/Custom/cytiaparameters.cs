
using System;
using System.Collections.Generic;
using System.Text;
using CyDesigner.Extensions.Gde;
using CyDesigner.Extensions.Common;
using System.Windows.Forms;

namespace TIA_v1_90
{
    class TIAParameters
    {
        public CyCompDevParam tiaPower = null;
        public CyCompDevParam tiaCapFb = null;
        public CyCompDevParam tiaMinVdda = null;
        public CyCompDevParam tiaResFb = null;
        public CyCompDevParam tiaFcorner = null;
        public const string NONE = "None";
        public const string CFB1_3 = "1.3 pF";
        public const string CFB3_3 = "3.3 pF";
        public const string CFB4_6 = "4.6 pF";
        public const string GTE_2_7V = "2.7 V or greater";
        public const string LT_2_7V = "Less than 2.7 V";
        public const string MINPOW = "Minimum Power";
        public const string LOWPOW = "Low Power";
        public const string MIDPOW = "Medium Power";
        public const string HIGHPOW = "High Power";
        public const string RESFB_20k = "20k ohms";
        public const string RESFB_30k = "30k ohms";
        public const string RESFB_40k = "40k ohms";
        public const string RESFB_80k = "80k ohms";
        public const string RESFB_120k = "120k ohms";
        public const string RESFB_250k = "250k ohms";
        public const string RESFB_500k = "500k ohms";
        public const string RESFB_1000k = "1000k ohms";

        public const string CAP_FEEDBACK = "Capacitive_Feedback";
        public const string POWER = "Power";
        public const string RES_FEEDBACK = "Resistive_Feedback";
        public const string FCORNER = "Fcorner";

        public TIAParameters(ICyInstEdit_v1 inst)
        {
            GetParams(inst);
        }

        //Get parameter values
        private void GetParams(ICyInstEdit_v1 inst)
        {
            tiaPower = inst.GetCommittedParam(POWER);
            tiaCapFb = inst.GetCommittedParam(CAP_FEEDBACK);
            tiaResFb = inst.GetCommittedParam(RES_FEEDBACK);
            tiaFcorner = inst.GetCommittedParam(FCORNER);
        }
    }
}