/*******************************************************************************
* Copyright 2011-2012, Cypress Semiconductor Corporation.  All rights reserved.
* You may use this file only in accordance with the license, terms, conditions, 
* disclaimers, and limitations in the end user license agreement accompanying 
* the software package with which this file was provided. 
********************************************************************************/

using System;
using System.Collections.Generic;
using System.Text;
using System.Windows.Forms;
using CyDesigner.Extensions.Common;
using CyDesigner.Extensions.Gde;

namespace SPDIF_Tx_v1_10
{
    public class CyCustomizer : ICyParamEditHook_v1
    {
        public const string GENERAL_TAB_NAME = "General";
        public const string CHANNEL_0_STATUS_TAB_NAME = "Channel 0 Status";
        public const string CHANNEL_1_STATUS_TAB_NAME = "Channel 1 Status";

        #region ICyParamEditHook_v1 Members
        public DialogResult EditParams(ICyInstEdit_v1 edit, ICyTerminalQuery_v1 termQuery, ICyExpressMgr_v1 mgr)
        {
            CyEditingWrapperControl.m_runMode = true;

            CySPDifTxParameters parameters = new CySPDifTxParameters(edit);

            ICyTabbedParamEditor editor = edit.CreateTabbedParamEditor();

            CyParamExprDelegate exprDelegate = delegate(ICyParamEditor custEditor, CyCompDevParam param)
            {
                parameters.LoadParameters();
            };

            editor.AddCustomPage(Resource.GeneralTabCaption, new CySPDifTxBasicTab(parameters),
                exprDelegate, GENERAL_TAB_NAME);
            editor.AddCustomPage(Resource.Channel0StatusTabCaption, new CyChannel0StatusTab(parameters),
                exprDelegate, CHANNEL_0_STATUS_TAB_NAME);
            editor.AddCustomPage(Resource.Channel1StatusTabCaption, new CyChannel1StatusTab(parameters),
                exprDelegate, CHANNEL_1_STATUS_TAB_NAME);
            editor.AddDefaultPage(Resource.BuiltInTabCaption, "Built-in");

            parameters.LoadParameters();
            parameters.m_globalEditMode = true;
            return editor.ShowDialog();
        }

        bool ICyParamEditHook_v1.EditParamsOnDrop
        {
            get
            {
                return false;
            }
        }

        CyCompDevParamEditorMode ICyParamEditHook_v1.GetEditorMode()
        {
            return CyCompDevParamEditorMode.COMPLETE;
        }
        #endregion
    }
}
