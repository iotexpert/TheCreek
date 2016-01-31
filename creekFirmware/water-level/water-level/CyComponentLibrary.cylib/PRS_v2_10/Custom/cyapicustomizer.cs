/*******************************************************************************
* Copyright 2008-2012, Cypress Semiconductor Corporation.  All rights reserved.
* You may use this file only in accordance with the license, terms, conditions, 
* disclaimers, and limitations in the end user license agreement accompanying 
* the software package with which this file was provided.
********************************************************************************/



using System.Collections.Generic;
using CyDesigner.Extensions.Common;
using CyDesigner.Extensions.Gde;

namespace PRS_v2_10
{
    public partial class CyCustomizer : ICyAPICustomize_v1
    {
        #region ICyAPICustomize_v1 Members
        public IEnumerable<CyAPICustomizer> CustomizeAPIs(
            ICyInstQuery_v1 query,
            ICyTerminalQuery_v1 termQuery, 
            IEnumerable<CyAPICustomizer> apis)
        {
            List<CyAPICustomizer> customizers = new List<CyAPICustomizer>(apis);
            Dictionary<string, string> paramDict = new Dictionary<string, string>();
            

            // Get the parameters from the .c file customizer
            IEnumerator<CyAPICustomizer> i = customizers.GetEnumerator();
            while(i.MoveNext())
            {
                // Get dict from main file. 
                CyAPICustomizer api = i.Current;
                if ((api.OriginalName.EndsWith("PRS.c"))||
                    (api.OriginalName.EndsWith("PRS.h")))
                {
                    paramDict = api.MacroDictionary;
                }                
            }

            GenerateAPIParameters(ref paramDict);

            //If No Data in Main object than no data will be generate
            i.Reset();
            while (i.MoveNext())
            {
                i.Current.MacroDictionary = paramDict;
            }
            return customizers;
        }
        #endregion

    }
}
