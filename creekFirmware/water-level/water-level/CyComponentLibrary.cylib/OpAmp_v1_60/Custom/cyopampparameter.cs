
using System;
using System.Collections.Generic;
using System.Text;
using CyDesigner.Extensions.Gde;
using CyDesigner.Extensions.Common;
using System.Windows.Forms;

namespace OpAmp_v1_60
{
    class OPAMPParameters
    {
        public CyCompDevParam opampPower = null;
        public CyCompDevParam opampMode = null;
        public const string OPAMP_MODE = "OpAmp";
        public const string FOLLOWER_MODE = "Follower";
        public const string LPOC_POWER = "Low Power Over Compensated";
        public const string LOW_POWER = "Low Power";
        public const string MED_POWER = "Med Power";
        public const string HIGH_POWER = "High Power";

        public const string POWER = "Power";
        public const string MODE = "Mode";
        
        public OPAMPParameters(ICyInstEdit_v1 inst)
        {
            GetParams(inst);
        }

        //Get parameter values
        private void GetParams(ICyInstEdit_v1 inst)
        {
            opampPower = inst.GetCommittedParam(POWER);
            opampMode = inst.GetCommittedParam(MODE);            
        }

        static CyCustErr GetParamValue<T>(ICyInstQuery_v1 query, string paramName, out T value)
        {
            CyCustErr err = CyCustErr.Ok;
            value = default(T);

            CyCompDevParam param = query.GetCommittedParam(paramName);
            if (param == null)
            {
                return new CyCustErr(string.Format("Unknown Parameter '{0}'", paramName));
            }

            err = param.TryGetValueAs<T>(out value);
            if (err.IsNotOk)
            {
                value = default(T);
                return err;
            }

            if (param.ErrorCount != 0)
            {
                err = new CyCustErr(param.ErrorMsgs);
            }

            return err;
        }

       static CyCustErr GetEnumParamValues(ICyInstQuery_v1 query, string paramName, out int value,
       out string displayName, out string idName)
        {
            CyCustErr err = GetParamValue<int>(query, paramName, out value);

            CyCompDevParam param = query.GetCommittedParam(paramName);
            displayName = query.ResolveEnumIdToDisplay(paramName, param.Expr);
            idName = query.ResolveEnumDisplayToId(paramName, displayName);
            return err;
        }

       public static CyCustErr GetOpampModeValue(ICyInstQuery_v1 query, out string displayName)
       {
           int value;
           string idName;

           return GetEnumParamValues(query, MODE, out value, out displayName, out idName);
       }

        public static CyCustErr GetOpampPowerValue(ICyInstQuery_v1 query, out string displayName)
        {
            int value;
            string idName;

            return GetEnumParamValues(query, MODE, out value, out displayName, out idName);
        }
    }
}