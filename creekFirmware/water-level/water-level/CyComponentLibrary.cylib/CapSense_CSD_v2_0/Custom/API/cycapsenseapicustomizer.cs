/*******************************************************************************
* Copyright 2008-2010, Cypress Semiconductor Corporation.  All rights reserved.
* You may use this file only in accordance with the license, terms, conditions,
* disclaimers, and limitations in the end user license agreement accompanying
* the software package with which this file was provided.
********************************************************************************/



using System;
using System.Collections.Generic;
using System.Text;
using CyDesigner.Extensions.Common;
using CyDesigner.Extensions.Gde;
using System.IO;
using System.Diagnostics;

namespace CapSense_CSD_v2_0
{
    public partial class CyCustomizer : ICyAPICustomize_v1
    {
        #region ICyAPICustomize_v1 Members
        public IEnumerable<CyAPICustomizer> CustomizeAPIs(ICyInstQuery_v1 query, ICyTerminalQuery_v1 termQuery,
            IEnumerable<CyAPICustomizer> apis)
        {
            //Debugger.Launch();
            List<CyAPICustomizer> customizers = new List<CyAPICustomizer>(apis);
            Dictionary<string, string> paramDict = new Dictionary<string, string>();

            if (customizers.Count > 0)
            {
                paramDict = customizers[0].MacroDictionary;
            }

            CyCSParameters packParams = new CyCSParameters();
            CyCSInstParameters instParam = new CyCSInstParameters(query);
            packParams.GetParams(instParam);

            CyAPIGenerator apiGen = new CyAPIGenerator(packParams, instParam);
            apiGen.CollectApiHFile(ref paramDict);
            apiGen.CollectApiCFile(ref paramDict);
            apiGen.CollectApiCSHLHFile(ref paramDict);
            apiGen.CollectApiCSHLCFile(ref paramDict);

            paramDict.Add("IsComplexScanSlots", (Convert.ToInt16(apiGen.m_isComplexSS)).ToString());


            //If No Data in Main object than no data will be generate
            for (int i = 0; i < customizers.Count; i++)
            {
                if (customizers[i].OutputName == query.InstanceName + "_Auto.h" ||
                    customizers[i].OutputName == query.InstanceName + "_Auto.c")
                {
                    if (packParams.m_settings.m_tuningMethod == CyTuningMethodOptions.Tuning_None ||
                        packParams.m_settings.m_tuningMethod == CyTuningMethodOptions.Tuning_Manual)
                        continue;
                }
                if (customizers[i].OutputName == query.InstanceName + "_MBX.h" ||
                    customizers[i].OutputName == query.InstanceName + "_MBX.c")
                {
                    if (packParams.m_settings.m_tuningMethod == CyTuningMethodOptions.Tuning_None)
                        continue;
                }
                customizers[i].MacroDictionary = paramDict;
                yield return customizers[i];
            }
        }
        #endregion
    }
}
