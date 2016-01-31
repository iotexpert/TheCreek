/*******************************************************************************
* Copyright 2008-2009, Cypress Semiconductor Corporation.  All rights reserved.
* You may use this file only in accordance with the license, terms, conditions, 
* disclaimers, and limitations in the end user license agreement accompanying 
* the software package with which this file was provided.
********************************************************************************/



using System;
using System.Collections.Generic;
using System.Text;
using System.IO;
//using System.Windows.Forms;
using System.Diagnostics;

namespace CRC_v1_20
{
    public partial class CyCustomizer
    {
        public void GenerateParameters(ref Dictionary<string, string> paramDict)
        {
            string m_CRCSize;
            uint CRCMask;
            int j;
            uint uRegSize = 8;

            string PolyValueLower;
            string PolyValueUpper;
            string SeedValueUpper;
            string SeedValueLower;

            paramDict.TryGetValue("Resolution", out m_CRCSize);
            uint CRCSize = uint.Parse(m_CRCSize);

            //Create parameters in HEX format
            paramDict.TryGetValue("PolyValueLower", out PolyValueLower);
            paramDict.TryGetValue("PolyValueUpper", out PolyValueUpper);
            paramDict.TryGetValue("SeedValueUpper", out SeedValueUpper);
            paramDict.TryGetValue("SeedValueLower", out SeedValueLower);

            paramDict.Add("PolyValueLowerHex", "0x" + Convert.ToUInt32(PolyValueLower).ToString("X") + "u");
            paramDict.Add("PolyValueUpperHex", "0x" + Convert.ToUInt32(PolyValueUpper).ToString("X") + "u");
            paramDict.Add("SeedValueLowerHex", "0x" + Convert.ToUInt32(SeedValueLower).ToString("X") + "u");
            paramDict.Add("SeedValueUpperHex", "0x" + Convert.ToUInt32(SeedValueUpper).ToString("X") + "u");

            #region  CRC_Mask

            CRCMask = 0;

            for (j = 0; j < CRCSize; j++)
            {
                CRCMask = (uint)(CRCMask | (uint)(1 << j));
            }

            #endregion

            paramDict.Add("CRCMaskHex", "0x" + CRCMask.ToString("X") + "u");
            if (CRCSize > 16)
                uRegSize = 32;
            else if (CRCSize > 8)
                uRegSize = 16;
            paramDict.Add("RegSize", "uint"+uRegSize);
        }
    }
}
