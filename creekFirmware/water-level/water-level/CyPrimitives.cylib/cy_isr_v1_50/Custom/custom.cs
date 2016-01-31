/*******************************************************************************
* Copyright 2008-2009, Cypress Semiconductor Corporation.  All rights reserved.
* You may use this file only in accordance with the license, terms, conditions, 
* disclaimers, and limitations in the end user license agreement accompanying 
* the software package with which this file was provided.
********************************************************************************/

using System;
using System.Collections.Generic;
using System.Text;
using System.Drawing;
using System.Windows.Forms;
using System.ComponentModel;
using System.Diagnostics;
using System.Runtime.InteropServices;
using System.IO;

using ActiproSoftware.SyntaxEditor;

using CyDesigner.Common.Base;
using CyDesigner.Common.Base.Controls;
using CyDesigner.Common.Base.Dialogs;
using CyDesigner.Extensions.Common;
using CyDesigner.Extensions.Gde;

using CyDesigner.PSoC.PSoC3.Fitter.DesignWideResources;

namespace Cypress.Components.System.cy_isr_v1_50
{
    public class CyCustomizer :
        ICyVerilogCustomize_v1
    {
        //-----------------------------

        #region ICyVerilogCustomize_v1 Members

        public CyCustErr CustomizeVerilog(ICyInstQuery_v1 query, ICyTerminalQuery_v1 termQuery, out string codeSnippet)

        {

            CyCustErr err = CyCustErr.Ok;

            codeSnippet = string.Empty;

            CyVerilogWriter vw = new CyVerilogWriter("cy_isr_v1_0", query.InstanceName);

            //Add Generics.

            foreach (string paramName in query.GetParamNames())

            {

                CyCompDevParam param = query.GetCommittedParam(paramName);

                if (param.IsHardware)

                    {

                        vw.AddGeneric(param.Name, param.Value);

                    }

            }

            //Add Ports

            foreach (string termName in termQuery.GetTerminalNames())

            {

                string termBaseName = termQuery.GetTermBaseName(termName);

                CyCompDevTermDir termDirection = termQuery.GetTermDirection(termName);

                bool termHasNoDrivers = termQuery.GetHasNoDrivers(termName);

                string value;

                if (termDirection != CyCompDevTermDir.OUTPUT && termHasNoDrivers)

                    {

                        value = termQuery.GetTermDefaultVlogExpr(termName);

                    }

                    else

                        {

                            value = termQuery.GetTermSigSegName(termName);

                        }

                        vw.AddPort(termBaseName, value);

            }

            codeSnippet = vw.ToString();

            return err;

        }

        #endregion
    }
}
