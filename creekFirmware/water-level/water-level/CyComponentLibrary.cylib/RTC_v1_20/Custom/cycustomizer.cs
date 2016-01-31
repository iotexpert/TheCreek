/*******************************************************************************
* Copyright 2008-2009, Cypress Semiconductor Corporation.  All rights reserved.
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

namespace RTC_v1_20
{
    public partial class CyCustomizer : ICyAPICustomize_v1, ICyParamEditHook_v1
    {
        #region ICyAPICustomize_v1 Members
        string m_CS_CFile_Name = "RTC.c";
        string m_CS_HFile_Name = "RTC.h";
        public const string m_instanceNameParam = "INSTANCE_NAME";
        string m_instanceName = "";

        CyAPICustomizer m_CS_CFile;
        CyAPICustomizer m_CS_HFile;


        public IEnumerable<CyAPICustomizer> CustomizeAPIs(ICyInstQuery_v1 query, ICyTerminalQuery_v1 termQuery, 
            IEnumerable<CyAPICustomizer> apis)
        {
            //Debugger.Launch(); 
            List<CyAPICustomizer> customizers = new List<CyAPICustomizer>(apis);
            Dictionary<string, string> paramDict = new Dictionary<string, string>();


            // Get the parameters from the .c file customizer
            foreach (CyAPICustomizer api in customizers)
            {
                // Get dict from main file. 
                if (api.OriginalName.EndsWith(m_CS_CFile_Name))
                {
                    m_CS_CFile = api;
                    paramDict = api.MacroDictionary;
                    paramDict.TryGetValue(m_instanceNameParam, out m_instanceName);
                }
                else if (api.OriginalName.EndsWith(m_CS_HFile_Name))
                {
                    m_CS_HFile = api;
                }
            }

            GeneratePameters(ref paramDict);

            //If No Data in Main object than no data will be generate
            foreach (CyAPICustomizer api in customizers)
            {
                api.MacroDictionary = paramDict;
            }
            return customizers;
        }
        #endregion


        #region ICyParamEditHook_v1 Members

        public DialogResult EditParams(ICyInstEdit_v1 edit, ICyTerminalQuery_v1 termQuery, ICyExpressMgr_v1 mgr)
        {
            CyParameters parameters = new CyParameters(edit);
            CyBasicConfigurationControl basicForm=new CyBasicConfigurationControl(parameters);

            CyParamExprDelegate ParamCommitted = delegate(ICyParamEditor custEditor, CyCompDevParam param)
            {
                parameters.LoadParameters();
                basicForm.UpdateForm();
            };

            ICyTabbedParamEditor editor = edit.CreateTabbedParamEditor();
            editor.AddCustomPage("Basic Configuration", basicForm, ParamCommitted, "Basic Configuration");
            editor.AddDefaultPage("Built-in", "Built-in");
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
