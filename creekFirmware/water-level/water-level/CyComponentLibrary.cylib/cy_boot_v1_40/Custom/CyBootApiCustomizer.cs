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
using cy_boot_v1_40;

namespace cy_boot_v1_40
{
    public class CyCustomizer : ICyAPICustomize_v1
    {
        #region ICyAPICustomize_v1 Members
        #region API Customizer File Constants and Variables

        // Bootloader files
        const string BTLDR_CFILE_NAME = "cybtldr.c";
        const string BTLDR_HFILE_NAME = "cybtldr.h";
        const string BTLDR_HELPER_FILE_NAME = "cybtldrhelper.a51";

        // Bootloader or Loadable files
        const string BTLDR_LOADABLE_CFILE_NAME = "cybtldr_loadable.c";
        const string BTLDR_LOADABLE_HFILE_NAME = "cybtldr_loadable.h";

        const string PROJ_TYPE = "CYDEV_PROJ_TYPE";
        const string PROJ_TYPE_STANDARD = "Standard";
        const string PROJ_TYPE_LOADER   = "Bootloader";
        const string PROJ_TYPE_LOADABLE = "Loadable";

        #endregion
        public IEnumerable<CyAPICustomizer> CustomizeAPIs(ICyInstQuery_v1 query, ICyTerminalQuery_v1 termQuery,
                                                          IEnumerable<CyAPICustomizer> apis)
        {
            List<CyAPICustomizer> inputCustomizers = new List<CyAPICustomizer>(apis);
            List<CyAPICustomizer> outputCustomizers = new List<CyAPICustomizer>();
            Dictionary<string, string> paramDict = null;

            // Get a parameter dictionary.  Any one will do because they all come identical
            // (Yes I am assuming there will be at least one)
            paramDict = inputCustomizers[0].MacroDictionary;

            string projType;
            paramDict.TryGetValue(PROJ_TYPE, out projType);

            // Now copy the customizers to the output list following the simple rules
            // If it is not a special file, copy it.  Otherwise the rules are based on PROJ_TYPE
            foreach (CyAPICustomizer api in inputCustomizers)
            {
                if (api.OriginalName.EndsWith(BTLDR_CFILE_NAME))
                {
                    if (projType == PROJ_TYPE_LOADER)
                    {
                        outputCustomizers.Add(api);
                    }
                }
                else if (api.OriginalName.EndsWith(BTLDR_HFILE_NAME))
                {
                    if (projType == PROJ_TYPE_LOADER)
                    {
                        outputCustomizers.Add(api);
                    }
                }
                else if (api.OriginalName.EndsWith(BTLDR_HELPER_FILE_NAME))
                {
                    if (projType == PROJ_TYPE_LOADER)
                    {
                        outputCustomizers.Add(api);
                    }
                }
                else if (api.OriginalName.EndsWith(BTLDR_LOADABLE_CFILE_NAME))
                {
                    if (projType == PROJ_TYPE_LOADABLE)
                    {
                        outputCustomizers.Add(api);
                    }
                }
                else if (api.OriginalName.EndsWith(BTLDR_LOADABLE_HFILE_NAME))
                {
                    if (projType == PROJ_TYPE_LOADABLE)
                    {
                        outputCustomizers.Add(api);
                    }
                }
                else
                {
                    outputCustomizers.Add(api);
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
