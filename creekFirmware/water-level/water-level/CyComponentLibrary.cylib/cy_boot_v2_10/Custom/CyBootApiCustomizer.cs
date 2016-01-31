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

namespace cy_boot_v2_10
{
    public class CyCustomizer : ICyAPICustomize_v2
    {
        #region ICyAPICustomize_v2 Members
        #region API Customizer File Constants and Variables

        // Shared Bootloader/Loadable files
        const string BTLDR_COMMON_CFILE_NAME = "cybtldr_common.c";
        const string BTLDR_COMMON_HFILE_NAME = "cybtldr_common.h";

        // Bootloader files
        const string BTLDR_CFILE_NAME = "cybtldr.c";
        const string BTLDR_HFILE_NAME = "cybtldr.h";
        const string BTLDR_HELPER_FILE_NAME = "cybtldrhelper.a51";

        // Boot Loadable files
        const string BTLDR_LOADABLE_CFILE_NAME = "cybtldr_loadable.c";
        const string BTLDR_LOADABLE_HFILE_NAME = "cybtldr_loadable.h";

        // CMSIS files
        const string CMSIS_CORE_HFILE = "core_cm3.h";
        const string CMSIS_CORE_CFILE = "core_cm3.c";
        const string CMSIS_PSOC_HFILE = "core_cm3_psoc5.h";

        const string PROJ_TYPE = "CYDEV_PROJ_TYPE";
        const string PROJ_TYPE_STANDARD = "Standard";
        const string PROJ_TYPE_LOADER   = "Bootloader";
        const string PROJ_TYPE_LOADABLE = "Loadable";

        #endregion
        public IEnumerable<CyAPICustomizer> CustomizeAPIs(ICyAPICustomizeArgs_v2 args, 
                                                          IEnumerable<CyAPICustomizer> apis)
        {
            List<CyAPICustomizer> inputCustomizers = new List<CyAPICustomizer>(apis);
            List<CyAPICustomizer> outputCustomizers = new List<CyAPICustomizer>();
            Dictionary<string, string> paramDict = null;
            // CMSIS_PSOC_HFILE must be included BEFORE CMSIS_CORE_HFILE, to prevent build failures
            bool foundPsocCmsis = false;

            // Get a parameter dictionary.  Any one will do because they all come identical
            // (Yes I am assuming there will be at least one)
            paramDict = inputCustomizers[0].MacroDictionary;

            string projType;
            List<CyAPICustomizer> cmsisCustomizers = new List<CyAPICustomizer>();

            paramDict.TryGetValue(PROJ_TYPE, out projType);

            // Now copy the customizers to the output list following the simple rules
            // If it is not a special file, copy it.  Otherwise the rules are based on PROJ_TYPE
            foreach (CyAPICustomizer api in inputCustomizers)
            {
                switch (api.OutputName)
                {
                    case BTLDR_CFILE_NAME:
                    case BTLDR_HFILE_NAME:
                    case BTLDR_HELPER_FILE_NAME:
                        if (projType == PROJ_TYPE_LOADER)
                            outputCustomizers.Add(api);
                        break;

                    case BTLDR_LOADABLE_CFILE_NAME:
                    case BTLDR_LOADABLE_HFILE_NAME:
                        if (projType == PROJ_TYPE_LOADABLE)
                            outputCustomizers.Add(api);
                        break;

                    case BTLDR_COMMON_CFILE_NAME:
                    case BTLDR_COMMON_HFILE_NAME:
                        if (projType != PROJ_TYPE_STANDARD)
                            outputCustomizers.Add(api);
                        break;

                    case CMSIS_CORE_CFILE:
                    case CMSIS_CORE_HFILE:
                        if (!foundPsocCmsis)
                            cmsisCustomizers.Add(api);
                        else
                            outputCustomizers.Add(api);
                        break;

                    case CMSIS_PSOC_HFILE:
                        foundPsocCmsis = true;
                        outputCustomizers.Add(api);
                        foreach (CyAPICustomizer c in cmsisCustomizers)
                            outputCustomizers.Add(c);
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
