/*******************************************************************************
* Copyright 2008-2009, Cypress Semiconductor Corporation.  All rights reserved.
* You may use this file only in accordance with the license, terms, conditions, 
* disclaimers, and limitations in the end user license agreement accompanying 
* the software package with which this file was provided.
********************************************************************************/
using System;
using System.Collections.Generic;
using System.Text;

using System.Diagnostics;
using System.Drawing;
using System.IO;
using System.Windows.Forms;

using CyDesigner.Extensions.Common;
using CyDesigner.Extensions.Gde;

using Cypress.Semiconductor.CyDesigner.cy_logic_gate_customizer;

namespace Cypress.Semiconductor.CyDesigner.and_v1_0
{
    public class CyCustomizer : 
        ICyVerilogCustomize_v1, 
        ICyShapeCustomize_v1
    {
        const string GENERATED_SHAPE = "SymbolShape";

        #region ICyShapeCustomize_v1 Members

        public CyCustErr CustomizeShapes(
            ICyInstQuery_v1 instQuery,
            ICySymbolShapeEdit_v1 shapeEdit, 
            ICyTerminalEdit_v1 termEdit)
        {
            return CyShapeCustomizer.CustomizeGateShapesForAnd(instQuery, shapeEdit, termEdit, CyGateType.AND);
        }
        #endregion

        #region ICyVerilogCustomize_v1 Members

        public CyCustErr CustomizeVerilog(
            ICyInstQuery_v1 instQuery,
            ICyTerminalQuery_v1 termQuery,
            out string codeSnippet)
        {
            Debug.Assert(instQuery != null);
            if (instQuery == null)
            {
                codeSnippet = string.Empty;
                return new CyCustErr("Invalid instance query parameter");
            }
            
            // Collect the signal segment names for each of the instance terminals
            List<string> inSigSegNames = new List<string>();
            string outTermSigSegName = string.Empty;

            string sigSegName;

            foreach (string termName in termQuery.GetTerminalNames())
            {
                CyCompDevTermDir termDir = termQuery.GetTermDirection(termName);
				sigSegName = termQuery.GetTermSigSegName(termName);
                if (termDir == CyCompDevTermDir.INPUT)
                    inSigSegNames.Add(sigSegName);
                if (termDir == CyCompDevTermDir.OUTPUT)
                    outTermSigSegName = sigSegName;
            }

            // Generate the verilog code snippet,  example, "assign out_wire = in1 & in2 & in3;"
            StringBuilder sb = new StringBuilder();
            sb.Append(Environment.NewLine);
            sb.Append("    ");
            sb.Append(string.Format("assign {0} = ", outTermSigSegName));

            int i = 1;
            foreach (string sigName in inSigSegNames)
            {
                sb.Append(sigName);
                if (i < inSigSegNames.Count)
                    sb.Append(" & ");
                i++;
            }

            sb.Append(";");
            sb.Append(Environment.NewLine);
            codeSnippet = sb.ToString();

            return CyCustErr.OK;
        }

        #endregion
    }

}
//[]//
