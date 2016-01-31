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

using CyDesigner.Extensions.Common;
using CyDesigner.Extensions.Gde;

namespace Cypress.Components.System.cy_analog_noconnect_v1_0
{
    public class CyCustomizer : 
        ICyShapeCustomize_v1
    {
		#region ICyShapeCustomize_v1 Members

        public CyCustErr CustomizeShapes(ICyInstQuery_v1 query, ICySymbolShapeEdit_v1 shapeEdit, 
            ICyTerminalEdit_v1 termEdit)
        {
			shapeEdit.SetFillColor("body", query.Preferences.SchematicAnalogTerminalColor);
            return CyCustErr.Ok;
        }
		
		#endregion
	}
}