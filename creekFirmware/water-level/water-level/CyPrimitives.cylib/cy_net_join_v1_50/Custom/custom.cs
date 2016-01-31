/*******************************************************************************
* Copyright 2010, Cypress Semiconductor Corporation.  All rights reserved.
* You may use this file only in accordance with the license, terms, conditions, 
* disclaimers, and limitations in the end user license agreement accompanying 
* the software package with which this file was provided.
********************************************************************************/

using System;
using System.Collections.Generic;
using System.Text;
using System.Diagnostics;
using System.Runtime.InteropServices;
using System.IO;

using CyDesigner.Extensions.Common;
using CyDesigner.Extensions.Gde;

namespace Cypress.Components.Primitives.cy_net_join_v1_50
{
    public class CyCustomizer : ICyVerilogCustomize_v1
    {

        #region ICyVerilogCustomize_v1 Members

        /// <summary>
        /// Needed to convert the default empty string to the default ID.
        /// </summary>
        /// <param name="query"></param>
        /// <param name="termQuery"></param>
        /// <param name="codeSnippet"></param>
        /// <returns></returns>
        public CyCustErr CustomizeVerilog(ICyInstQuery_v1 query, ICyTerminalQuery_v1 termQuery, out string codeSnippet)
        {
            CyCustErr err = CyCustErr.Ok;
            codeSnippet = string.Empty;
            CyVerilogWriter vw = new CyVerilogWriter("cy_connect_v1_0", query.InstanceName);

            // Add Generics.
            // Net_Join component is a special type of cy_connect_v1_0. 
            // Its signal width is always 1. 
            // We take the default sig_width = 1 for warp primitive.
            // We override the "is_net_join" generic from default false to true.

            vw.AddGeneric("is_net_join", "1");

            //Add Ports.
            int signalIndex = 0;
            string portBaseName = "signal";

            foreach (string termCanonicalName in termQuery.GetTerminalNames())
            {
                string value = termQuery.GetTermSigSegName(termCanonicalName);
                signalIndex++;
                CyCompDevTermDir dir = termQuery.GetTermDirection(termCanonicalName);
                bool hasNoDrivers = termQuery.GetHasNoDrivers(termCanonicalName);
                if (dir != CyCompDevTermDir.OUTPUT && hasNoDrivers)
                {
                    value = termQuery.GetTermDefaultVlogExpr(termCanonicalName);
                }

                string baseTermName = termQuery.GetTermBaseName(termCanonicalName);
                vw.AddPort(portBaseName + signalIndex.ToString(), value);
            }

            codeSnippet = vw.ToString();
            return err;
        }

        #endregion
    }
}