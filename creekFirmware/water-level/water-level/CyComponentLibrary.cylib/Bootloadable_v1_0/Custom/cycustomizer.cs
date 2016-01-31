/*******************************************************************************
* Copyright 2012, Cypress Semiconductor Corporation.  All rights reserved.
* You may use this file only in accordance with the license, terms, conditions, 
* disclaimers, and limitations in the end user license agreement accompanying 
* the software package with which this file was provided. 
********************************************************************************/

using System;
using System.Collections.Generic;
using System.Text;
using System.Windows.Forms;
using CyDesigner.Extensions.Gde;
using CyDesigner.Extensions.Common;
using System.IO;

namespace Bootloadable_v1_0
{
    [CyCompDevCustomizer]
    public partial class CyCustomizer : ICyParamEditHook_v1, ICyBootloadableProvider_v1
    {
        #region ICyParamEditHook_v1 Members
        DialogResult ICyParamEditHook_v1.EditParams(ICyInstEdit_v1 edit, ICyTerminalQuery_v1 termQuery,
                                                    ICyExpressMgr_v1 mgr)
        {
            const string PAPAM_TAB_NAME_GENERAL = "General";
            const string PAPAM_TAB_NAME_BUILTIN = "Built-in";
            CyEditingWrapperControl.RUN_MODE = true;
            CyParameters prms = new CyParameters(edit);
            CyGeneralPage generalPage = new CyGeneralPage(prms);
            CyDependenciesPage dependPage = new CyDependenciesPage(prms);
            ICyTabbedParamEditor editor = edit.CreateTabbedParamEditor();

            CyParamExprDelegate exprDelegate = delegate(ICyParamEditor custEditor, CyCompDevParam param)
            {
                prms.m_globalEditMode = false;
                if (param.TabName == PAPAM_TAB_NAME_GENERAL)
                    generalPage.InitFields();
                prms.m_globalEditMode = true;
            };

            editor.AddCustomPage(Properties.Resources.PageTitleGeneral, generalPage, exprDelegate,
                                 PAPAM_TAB_NAME_GENERAL);
            editor.AddCustomPage(Properties.Resources.PageTitleDependencies, dependPage, exprDelegate,
                                 dependPage.TabName);
            editor.AddDefaultPage(Properties.Resources.PageTitleBuiltIn, PAPAM_TAB_NAME_BUILTIN);

            prms.m_globalEditMode = true;

            return editor.ShowDialog();
        }

        bool ICyParamEditHook_v1.EditParamsOnDrop
        {
            get { return false; }
        }

        CyCompDevParamEditorMode ICyParamEditHook_v1.GetEditorMode()
        {
            return CyCompDevParamEditorMode.COMPLETE;
        }
        #endregion

        #region ICyBootloadableProvider_v1 Members
        public CyCustErr GetBootloadableData(ICyBootloadableProviderArgs_v1 args, out CyBootloadableData_v1 data)
        {
            CyParameters prms = new CyParameters(args.InstQuery);
            data = new CyBootloadableData_v1(prms.HexFilePath, Path.ChangeExtension(prms.HexFilePath, "elf"), 
                                             prms.AutoPlacement, prms.PlacementAddress, prms.AppID, 
                                             prms.AppVersion, prms.AppCustomID);
            return CyCustErr.Ok;
        }
        #endregion
    }
}
