/*******************************************************************************
* Copyright 2010, Cypress Semiconductor Corporation.  All rights reserved.
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

using CyDesigner.Extensions.Common;
using CyDesigner.Extensions.Gde;
using CyDesigner.Common.Base.Controls;

using Cypress.Components.Primitives.cy_net_constraint_v1_50;

namespace Cypress.Components.Primitives.cy_analog_reserve_v1_50
{
    public class CyCustomizer :
        ICyParamEditHook_v1,
        ICyVerilogCustomize_v1,
        ICyShapeCustomize_v1,
        ICyToolTipCustomize_v1
    {
        CyGlobalAnalogLocCustomizerHelper m_helper = new CyGlobalAnalogLocCustomizerHelper();

        //-----------------------------

        #region ICyParamEditHook_v1 Members

        public CyCompDevParamEditorMode GetEditorMode()
        {
            return CyCompDevParamEditorMode.COMPLETE;
        }

        /// <summary>
        /// Displays a custom parameter editor to the user to allow them to edit the params.
        /// </summary>
        /// <param name="edit"></param>
        /// <param name="mgr"></param>
        /// <returns></returns>
        public DialogResult EditParams(ICyInstEdit_v1 edit, ICyTerminalQuery_v1 termQuery, ICyExpressMgr_v1 mgr)
        {
            return m_helper.EditParams(CyGlobalAnalogLocType.Reserve, edit, termQuery, mgr);
        }

        /// <summary>
        /// Gets whether or not EditParams should be called when initailly
        /// dropped onto a canvas.
        /// </summary>
        public bool EditParamsOnDrop
        {
            get { return false; }
        }

        #endregion

        //-----------------------------

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
            return m_helper.CustomizeVerilog(CyGlobalAnalogLocType.Reserve, query, termQuery, out codeSnippet);
        }

        #endregion

        //-----------------------------

        #region ICyShapeCustomize_v1 Members

        /// <summary>
        /// Add the short location name inside the box.
        /// </summary>
        /// <param name="query"></param>
        /// <param name="shapeEdit"></param>
        /// <param name="termEdit"></param>
        /// <returns></returns>
        public CyCustErr CustomizeShapes(
            ICyInstQuery_v1 query,
            ICySymbolShapeEdit_v1 shapeEdit,
            ICyTerminalEdit_v1 termEdit)
        {
            return m_helper.CustomizeShapes(CyGlobalAnalogLocType.Reserve, query, shapeEdit, termEdit);
        }

        #endregion

        //-----------------------------

        #region ICyToolTipCustomize_v1

        /// <summary>
        /// Looks up the human readable name for the location.
        /// </summary>
        /// <param name="query"></param>
        /// <returns></returns>
        public string CustomizeToolTip(ICyInstQuery_v1 query, ICyTerminalQuery_v1 termQuery)
        {
            return m_helper.CustomizeToolTip(CyGlobalAnalogLocType.Reserve, query, termQuery);
        }

        #endregion

        //-----------------------------
    }
}
//[]//
