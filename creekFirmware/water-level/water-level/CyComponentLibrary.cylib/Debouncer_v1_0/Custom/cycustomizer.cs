/*******************************************************************************
* Copyright 2012, Cypress Semiconductor Corporation.  All rights reserved.
* You may use this file only in accordance with the license, terms, conditions, 
* disclaimers, and limitations in the end user license agreement accompanying 
* the software package with which this file was provided. 
********************************************************************************/

using System;
using System.Collections.Generic;
using System.Text;
using CyDesigner.Extensions.Gde;
using CyDesigner.Extensions.Common;

namespace Debouncer_v1_0
{
    [CyCompDevCustomizer()]
    class CyCustomizer : ICyParamEditHook_v1, ICyShapeCustomize_v1
    {
        #region ICyParamEditHook_v1 members
        public System.Windows.Forms.DialogResult EditParams(ICyInstEdit_v1 edit, ICyTerminalQuery_v1 termQuery,
            ICyExpressMgr_v1 mgr)
        {
            CyParameters parameters = new CyParameters(edit);

            parameters.GlobalEditMode = false;

            ICyTabbedParamEditor editor = edit.CreateTabbedParamEditor();
            parameters.m_editor = editor;

            CyGeneralTab generalTab = new CyGeneralTab(parameters);

            CyParamExprDelegate dataChanged =
            delegate(ICyParamEditor custEditor, CyCompDevParam param)
            {
                parameters.GlobalEditMode = false;
                generalTab.UpdateUI();
                parameters.GlobalEditMode = true;
            };

            editor.AddCustomPage(Resources.GeneralTabDisplayName, generalTab, dataChanged, generalTab.TabName);

            editor.AddDefaultPage(Resources.BuiltInTabDisplayName, "Built-in");

            parameters.GlobalEditMode = true;

            return editor.ShowDialog();
        }

        public bool EditParamsOnDrop
        {
            get { return false; }
        }

        public CyCompDevParamEditorMode GetEditorMode()
        {
            return CyCompDevParamEditorMode.COMPLETE;
        }
        #endregion

        #region ICyShapeCustomize_v1 members
        readonly string[] TERM_BASE_NAME_LIST = new string[]
        {
            "d",
            "q",
            "pos",
            "neg",
            "either"
        };
        const string TERM_PATERN = "{0}[{1}:0]";

        public CyCustErr CustomizeShapes(ICyInstQuery_v1 instQuery, ICySymbolShapeEdit_v1 shapeEdit,
            ICyTerminalEdit_v1 termEdit)
        {
            CyCustErr err;

            // Read Parameter(s)
            CyCompDevParam numTerminalsParam = instQuery.GetCommittedParam(CyParamNames.SIGNAL_WIDTH);
            byte busWidth = byte.Parse(numTerminalsParam.Value);
            busWidth--;

            // Rename terminal(s)
            foreach (string termBaseName in TERM_BASE_NAME_LIST)
            {
                err = SetTermName(termEdit, termBaseName, busWidth);
                if (err.IsNotOK)
                    return err;
            }
 
            return CyCustErr.OK;
        }

        private CyCustErr SetTermName(ICyTerminalEdit_v1 termEdit, string termBaseName, byte width)
        {
            string busName = termEdit.GetTermName(termBaseName);
            CyCustErr err = termEdit.TerminalRename(busName,
                (width > 0) ? string.Format(TERM_PATERN, termBaseName, width) : termBaseName);

            return err;
        }
        #endregion
    }
}
