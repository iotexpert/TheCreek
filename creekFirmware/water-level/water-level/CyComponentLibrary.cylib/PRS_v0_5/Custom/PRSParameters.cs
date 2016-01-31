/*******************************************************************************
* Copyright 2008-2009, Cypress Semiconductor Corporation.  All rights reserved.
* You may use this file only in accordance with the license, terms, conditions, 
* disclaimers, and limitations in the end user license agreement accompanying 
* the software package with which this file was provided.
********************************************************************************/



using System;
using System.Collections.Generic;
using System.Text;
using System.Diagnostics;
using System.Windows.Forms;
using CyDesigner.Extensions.Gde;

namespace CyCustomizer.PRS_v0_5
{
    public class PRSParameters
    {
        ICyInstEdit_v1 inst;

        public UInt64 SeedValue = 1;
        public UInt64 PolyValue = 0;
        public int RunMode = 1;
        public int Resolution = 2;

        public PRSParameters()
        {
        }

        public PRSParameters(ICyInstEdit_v1 inst)
        {
            GetParams(inst);
            this.inst = inst;
        }

        private void GetParams(ICyInstEdit_v1 inst)
        {
            Resolution = Convert.ToInt32(inst.GetCommittedParam("Resolution").Value);
            if (Resolution > 32)
            {
                SeedValue = (Convert.ToUInt32(inst.GetCommittedParam("SeedValueUpper").Value) << 32) +
                            Convert.ToUInt32(inst.GetCommittedParam("SeedValueLower").Value);
                PolyValue = (Convert.ToUInt32(inst.GetCommittedParam("PolyValueUpper").Value) << 32) +
                            Convert.ToUInt32(inst.GetCommittedParam("PolyValueLower").Value);
            }
            else
            {
                SeedValue = Convert.ToUInt32(inst.GetCommittedParam("SeedValueLower").Value);
                PolyValue = Convert.ToUInt32(inst.GetCommittedParam("PolyValueLower").Value);
            }
            RunMode = Convert.ToInt32(inst.GetCommittedParam("RunMode").Value);
            
        }

        public void SetParam(string paramName)
        {
            try
            {
                if (inst != null)
                {
                    switch (paramName)
                    {
                        case "PolyValue":
                            inst.SetParamExpr("PolyValueUpper", (PolyValue >> 32).ToString() + "u");
                            inst.SetParamExpr("PolyValueLower", (PolyValue & 0xFFFFFFFF).ToString() + "u");
                            break;
                        case "SeedValue":
                            inst.SetParamExpr("SeedValueUpper", (SeedValue >> 32).ToString() + "u");
                            inst.SetParamExpr("SeedValueLower", (SeedValue & 0xFFFFFFFF).ToString() + "u");
                            break;
                        case "Resolution":
                            inst.SetParamExpr("Resolution", Resolution.ToString());
                            break;
                        case "RunMode":
                            if (RunMode == 0)
                            {
                                inst.SetParamExpr("RunMode", "Clocked");
                            }
                            else
                            {
                                inst.SetParamExpr("RunMode", "APISingleStep");
                            }
                            break;
                        default:
                            break;
                    }
                }
            }
            catch
            {
            }
        }

        public void SetParams()
        {
            if (inst != null)
            {
                inst.SetParamExpr("SeedValueUpper", (SeedValue >> 32).ToString() + "u");
                inst.SetParamExpr("SeedValueLower", (SeedValue & 0xFFFFFFFF).ToString() + "u");
                inst.SetParamExpr("PolyValueUpper", (PolyValue >> 32).ToString() + "u");
                inst.SetParamExpr("PolyValueLower", (PolyValue & 0xFFFFFFFF).ToString() + "u");
                if (RunMode == 0)
                    inst.SetParamExpr("RunMode", "Clocked");
                else
                    inst.SetParamExpr("RunMode", "APISingleStep");
                inst.SetParamExpr("Resolution", Resolution.ToString());
            }
        }

        public void CommitParams()
        {
            if (inst != null)
            {
                if (!inst.CommitParamExprs())
                {
                    if (inst.GetCommittedParam("SeedValueLower").ErrorCount > 0)
                        MessageBox.Show(inst.GetCommittedParam("SeedValueLower").ErrorMsgs);

                    if (inst.GetCommittedParam("PolyValueLower").ErrorCount > 0)
                        MessageBox.Show(inst.GetCommittedParam("PolyValueLower").ErrorMsgs);

                    if (inst.GetCommittedParam("PolyValueUpper").ErrorCount > 0)
                        MessageBox.Show(inst.GetCommittedParam("PolyValueUpper").ErrorMsgs);
                }
            }
        }
    }
}
