/*******************************************************************************
* Copyright 2008-2010, Cypress Semiconductor Corporation.  All rights reserved.
* You may use this file only in accordance with the license, terms, conditions, 
* disclaimers, and limitations in the end user license agreement accompanying 
* the software package with which this file was provided.
********************************************************************************/

using System;
using System.Collections.Generic;
using System.Text;
using CyDesigner.Extensions.Common;
using CyDesigner.Extensions.Gde;
using System.Windows.Forms;
using System.Diagnostics;

namespace RTC_v1_60
{
    public partial class CyCustomizer : ICyAPICustomize_v1, ICyParamEditHook_v1
    {
        public const string BASIC_TABNAME = "Basic Configuration";

        #region ICyAPICustomize_v1 Members
        public const string INSTANCE_NAME_PARAM = "INSTANCE_NAME";        

        public IEnumerable<CyAPICustomizer> CustomizeAPIs(ICyInstQuery_v1 query, ICyTerminalQuery_v1 termQuery,
            IEnumerable<CyAPICustomizer> apis)
        {
            List<CyAPICustomizer> apiList = new List<CyAPICustomizer>(apis);
            Dictionary<string, string> paramDict = new Dictionary<string, string>();
            // Get the parameters from the .c file customizer
            string m_instanceName = string.Empty;
            if (apiList.Count > 0)
            {
                // All the default MacroDictionary�s will be the same so just grab the first one to manipulate.
                paramDict = apiList[0].MacroDictionary;
                paramDict.TryGetValue(INSTANCE_NAME_PARAM, out m_instanceName);
            }

            GeneratePameters(ref paramDict, m_instanceName);

            //If No Data in Main object than no data will be generate
            for (int i = 0; i < apiList.Count; i++)
            {
                apiList[i].MacroDictionary = paramDict;
            }
            return apiList;
        }
        #endregion

        #region ICyParamEditHook_v1 Members
        public DialogResult EditParams(ICyInstEdit_v1 edit, ICyTerminalQuery_v1 termQuery, ICyExpressMgr_v1 mgr)
        {
            CyRTCParameters parameters = new CyRTCParameters(edit);
            CyBasicConfigurationControl basicForm = new CyBasicConfigurationControl(parameters);

            CyParamExprDelegate ParamCommitted = delegate(ICyParamEditor custEditor, CyCompDevParam param)
            {
                parameters.LoadParameters();
                basicForm.UpdateForm();
            };

            ICyTabbedParamEditor editor = edit.CreateTabbedParamEditor();
            editor.AddCustomPage(Resources.BasicTabCaption, basicForm, ParamCommitted, BASIC_TABNAME);
            editor.AddDefaultPage(Resources.BuiltInTabCaption, "Built-in");
            DialogResult result = editor.ShowDialog();
            return result;
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
    }
}
