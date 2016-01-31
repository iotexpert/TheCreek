/*******************************************************************************
* Copyright 2008-2012, Cypress Semiconductor Corporation.  All rights reserved.
* You may use this file only in accordance with the license, terms, conditions, 
* disclaimers, and limitations in the end user license agreement accompanying 
* the software package with which this file was provided.
********************************************************************************/

using System;
using System.Collections.Generic;
using System.IO;
using System.Text;
using System.Windows.Forms;
using System.Diagnostics;
using CyDesigner.Extensions.Common;
using CyDesigner.Extensions.Gde;

namespace cy_boot_v3_0
{
    public class CyCustomizer : ICyAPICustomize_v2
    {
        #region ICyAPICustomize_v2 Members
        #region API Customizer File Constants and Variables

        // Startup code files
        const string STARTUP_8051_KEIL = "KeilStart.a51";
        const string STARTUP_CM0_GCC = "cm0gcc.ld";
        const string STARTUP_CM0_RVDS = "Cm0RealView.scat";
        const string STARTUP_CM3_GCC = "cm3gcc.ld";
        const string STARTUP_CM3_RVDS = "Cm3RealView.scat";

        // CMSIS files
        const string CMSIS_CORE_CM0_HFILE = "core_cm0.h";
        const string CMSIS_CORE_CM0_CFILE = "core_cm0.c";
        const string CMSIS_PSOC4_HFILE = "core_cm0_psoc4.h";
        const string CMSIS_CORE_CM3_HFILE = "core_cm3.h";
        const string CMSIS_CORE_CM3_CFILE = "core_cm3.c";
        const string CMSIS_PSOC5_HFILE = "core_cm3_psoc5.h";

        const string PROJ_TYPE = "CYDEV_PROJ_TYPE";
        const string PROJ_TYPE_STANDARD = "Standard";
        const string PROJ_TYPE_LOADER = "Bootloader";
        const string PROJ_TYPE_LOADER_MULTIAPP = "MultiAppBootloader";
        const string PROJ_TYPE_LOADABLE = "Loadable";
        const string PROJ_APP_COUNT = "CYDEV_APPLICATION_IMAGES";

        #endregion
        public IEnumerable<CyAPICustomizer> CustomizeAPIs(ICyAPICustomizeArgs_v2 args, 
                                                          IEnumerable<CyAPICustomizer> apis)
        {
            List<CyAPICustomizer> inputCustomizers = new List<CyAPICustomizer>(apis);
            List<CyAPICustomizer> outputCustomizers = new List<CyAPICustomizer>();
            Dictionary<string, string> paramDict = null;
            // CMSIS_PSOC5_HFILE or CMSIS_PSOC4_HFILE must be included BEFORE CMSIS_CORE file, 
            // to prevent build failures
            bool foundPsocCmsis = false;

            // Get a parameter dictionary.  Any one will do because they all come identical
            // (Yes I am assuming there will be at least one)
            paramDict = inputCustomizers[0].MacroDictionary;

            uint appCount;
            string projType, appCountStr;
            List<CyAPICustomizer> cmsisCustomizers = new List<CyAPICustomizer>();

            paramDict.TryGetValue(PROJ_TYPE, out projType);
            paramDict.TryGetValue(PROJ_APP_COUNT, out appCountStr);
            uint.TryParse(appCountStr, out appCount);

            // Now copy the customizers to the output list following the simple rules
            // If it is not a special file, copy it.  Otherwise the rules are based on PROJ_TYPE
            foreach (CyAPICustomizer api in inputCustomizers)
            {
                switch (api.OutputName)
                {
                    case CMSIS_CORE_CM0_CFILE:
                    case CMSIS_CORE_CM0_HFILE:
                    case CMSIS_CORE_CM3_CFILE:
                    case CMSIS_CORE_CM3_HFILE:
                        if (!foundPsocCmsis)
                            cmsisCustomizers.Add(api);
                        else
                            outputCustomizers.Add(api);
                        break;

                    case CMSIS_PSOC4_HFILE:
                    case CMSIS_PSOC5_HFILE:
                        foundPsocCmsis = true;
                        outputCustomizers.Add(api);
                        foreach (CyAPICustomizer c in cmsisCustomizers)
                            outputCustomizers.Add(c);
                        break;

                    case STARTUP_8051_KEIL:
                    case STARTUP_CM0_GCC:
                    case STARTUP_CM0_RVDS:
                    case STARTUP_CM3_GCC:
                    case STARTUP_CM3_RVDS:
                        if (projType == PROJ_TYPE_LOADABLE && appCount > 1)
                        {
                            CyAPICustomizer newApi;
                            for (int i = 1; i <= appCount; i++)
                            {
                                string newName = Path.GetFileNameWithoutExtension(api.OutputName);
                                newName += "_" + i;
                                newName += Path.GetExtension(api.OutputName);

                                string newContent = api.OriginalContent;
                                string appImageDefine = string.Format("APP_IMAGE_{0}_START", i);
                                newContent = newContent.Replace("PROJ_FIRST_FLS_BYTE", appImageDefine);
                                newApi = new CyAPICustomizer(
                                    api.OriginalName, newName, newContent, api.MacroDictionary, api.BuildType);
                                outputCustomizers.Add(newApi);
                            }
                        }
                        else
                            outputCustomizers.Add(api);
                        break;

                    default:
                        outputCustomizers.Add(api);
                        break;
                }
            }
            // Add the code here if any of the parameters need to be modified by the customizer
            // Make the changes to paramDict.  The completion code updates all of the customizer
            // to dictionaries with paramDict

            //Save result
            foreach (CyAPICustomizer api in outputCustomizers)
            {
                api.MacroDictionary = paramDict;
            }
            return outputCustomizers;
        }

        #endregion
    }
}
