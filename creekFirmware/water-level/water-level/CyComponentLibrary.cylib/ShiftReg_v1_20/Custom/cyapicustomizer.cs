/*******************************************************************************
* Copyright 2008-2009, Cypress Semiconductor Corporation.  All rights reserved.
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


namespace ShiftReg_v1_20
{
    public partial class CyCustomizer : ICyAPICustomize_v1
    {
        #region ICyAPICustomize_v1 Members
        static string m_CS_CFile_Name = "ShiftReg.c";
        static string m_CS_HFile_Name = "ShiftReg.h";
        const string m_instanceNameParam = "INSTANCE_NAME";
        string m_instanceName = "";

        public IEnumerable<CyAPICustomizer> CustomizeAPIs(ICyInstQuery_v1 query, ICyTerminalQuery_v1 termQuery,
            IEnumerable<CyAPICustomizer> apis)
        {
            CyAPICustomizer CS_CFile; 
            CyAPICustomizer CS_HFile;
            List<CyAPICustomizer> customizers = new List<CyAPICustomizer>(apis);
            Dictionary<string, string> paramDict = new Dictionary<string, string>();


            // Get the parameters from the .c file customizer
            foreach (CyAPICustomizer api in customizers)
            {
                // Get dict from main file. 
                if (api.OriginalName.EndsWith(m_CS_CFile_Name))
                {
                    CS_CFile = api;
                    paramDict = api.MacroDictionary;
                    paramDict.TryGetValue(m_instanceNameParam, out m_instanceName);
                }
                else if (api.OriginalName.EndsWith(m_CS_HFile_Name))
                {
                    CS_HFile = api;
                }
            }
            GenerateParameters(ref paramDict);

            //If No Data in Main object than no data will be generate
            foreach (CyAPICustomizer api in customizers)
            {
                api.MacroDictionary = paramDict;
            }
            return customizers;
        }
        #endregion

        public void GenerateParameters(ref Dictionary<string, string> paramDict)
        {
            string m_Length;           
            paramDict.TryGetValue("Length", out m_Length);            
            UInt16 SR_SIZE = Convert.ToUInt16(m_Length);
            UInt32 SrMask;

            #region  SR_Mask

            SrMask = 0;

            for (int j = 0; j < SR_SIZE; j++)
            {
                SrMask = (UInt32)(SrMask | (UInt32)(1 << j));
            }

            paramDict.Add("SR_MASK", "0x"+SrMask.ToString("X")+"U");
            #endregion
        }
    }
}
           