/*******************************************************************************
* Copyright 2008-2010, Cypress Semiconductor Corporation.  All rights reserved.
* You may use this file only in accordance with the license, terms, conditions, 
* disclaimers, and limitations in the end user license agreement accompanying 
* the software package with which this file was provided.
********************************************************************************/

using System;
using System.Collections.Generic;
using System.Text;
using System.Windows.Forms;
using System.Diagnostics;
using CyDesigner.Extensions.Common;
using CyDesigner.Extensions.Gde;
using System.Runtime.InteropServices;


namespace ShiftReg_v1_50
{
    public partial class CyCustomizer : ICyAPICustomize_v1
    {
        #region ICyAPICustomize_v1 Members
        public IEnumerable<CyAPICustomizer> CustomizeAPIs(ICyInstQuery_v1 query, ICyTerminalQuery_v1 termQuery,
            IEnumerable<CyAPICustomizer> apis)
        {
            List<CyAPICustomizer> customizers = new List<CyAPICustomizer>(apis);
            Dictionary<string, string> paramDict = new Dictionary<string, string>();

            // Get the parameters from the customizer files            
            for (int i = 0; i < customizers.Count; i++)
            {
                CyAPICustomizer api = customizers[i];
                if ((api.OriginalName.EndsWith("ShiftReg.c")) || (api.OriginalName.EndsWith("ShiftReg.h")))
                {
                    paramDict = api.MacroDictionary;
                    break;
                }
            }

            string m_Length;
            paramDict.TryGetValue(CySRParameters.P_LENGTH, out m_Length);
            UInt16 SR_SIZE = Convert.ToUInt16(m_Length);
            UInt32 SrMask = 0;

            for (int j = 0; j < SR_SIZE; j++)
            {
                SrMask = (UInt32)(SrMask | (UInt32)(1 << j));
            }

            paramDict.Add("SR_MASK", "0x" + SrMask.ToString("X") + "u");


            //Save changes in parameters
            for (int i = 0; i < customizers.Count; i++)
            {
                customizers[i].MacroDictionary = paramDict;
            }
            return customizers;
        }
        #endregion
    }
}
           