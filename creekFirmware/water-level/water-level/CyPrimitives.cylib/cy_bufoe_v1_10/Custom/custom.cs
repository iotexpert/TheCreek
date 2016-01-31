// ========================================
//
// Copyright Cypress Semiconductor Corporation, 2009
// All Rights Reserved
// UNPUBLISHED, LICENSED SOFTWARE.
//
// CONFIDENTIAL AND PROPRIETARY INFORMATION
// WHICH IS THE PROPERTY OF CYPRESS.
//
// Use of this file is governed
// by the license agreement included in the file
//
//     <install>/license/license.txt
//
// where <install> is the Cypress software
// installation root directory path.
//
// ========================================
/* ========================================
 *
 * Copyright YOUR COMPANY, THE YEAR
 * All Rights Reserved
 * UNPUBLISHED, LICENSED SOFTWARE.
 *
 * CONFIDENTIAL AND PROPRIETARY INFORMATION
 * WHICH IS THE PROPERTY OF your company.
 *
 * ========================================
*/

using System;
using System.Collections.Generic;
using System.Text;
using System.Drawing;
using System.Windows.Forms;
using System.ComponentModel;
using System.Diagnostics;
using System.Text.RegularExpressions;
using System.IO;

using CyDesigner.Extensions.Common;
using CyDesigner.Extensions.Gde;

namespace Cypress.Digital.Logic.cy_bufoe_v1_10
{
    public class CyCustomizer : ICyVerilogCustomize_v1
    {
		#region ICyVerilogCustomize_v1 Members

        public CyCustErr CustomizeVerilog(ICyInstQuery_v1 query, ICyTerminalQuery_v1 termQuery, out string codeSnippet)
        {
            CyCustErr err = CyCustErr.Ok;
            codeSnippet = string.Empty;
            CyVerilogWriter vw = new CyVerilogWriter("cy_bufoe", query.InstanceName);

            //Add Generics.
            foreach (string paramName in query.GetParamNames())
            {
                CyCompDevParam param = query.GetCommittedParam(paramName);
                if (param.IsHardware)
                {
                	vw.AddGeneric(param.Name, param.Value);
                }
            }

            //Add Ports.
            foreach (string termCanonicalName in termQuery.GetTerminalNames())
            {
                string value = termQuery.GetTermSigSegName(termCanonicalName);

                CyCompDevTermDir dir = termQuery.GetTermDirection(termCanonicalName);
                bool hasNoDrivers = termQuery.GetHasNoDrivers(termCanonicalName);
                if (dir != CyCompDevTermDir.OUTPUT && hasNoDrivers)
                {
                    value = termQuery.GetTermDefaultVlogExpr(termCanonicalName);
                }

                string baseTermName = termQuery.GetTermBaseName(termCanonicalName);
				
				if(baseTermName == "oe")
				{
				    // POST PR4 NOTE:
                	//     Prior to PR4, the OE signals are 'active low', meaning, a 0 will
                	//     enable the output, a 1 would turn it off. CDT 32449 is tracking the
                	//     PR4 change to the device to make it 'active high', as it should have
                	//     been.
                	string netOEName = "tmpOE__" + query.InstanceName + "_net";
                	vw.AddWire(netOEName, 0, 0);
                	vw.AssignPreES3CondWire(netOEName, "~" + "{" + value + "}", "{" + value + "}");
                    vw.AddPort(baseTermName, netOEName);
				}
				else
				{
                	vw.AddPort(baseTermName, value);
				}
            }

            codeSnippet = vw.ToString();
            return err;
        }
	
		#endregion
	}
}

//[] END OF FILE
