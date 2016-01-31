/*******************************************************************************
* Copyright 2008-2009, Cypress Semiconductor Corporation.  All rights reserved.
* You may use this file only in accordance with the license, terms, conditions,
* disclaimers, and limitations in the end user license agreement accompanying
* the software package with which this file was provided.
********************************************************************************/



using System;
using System.Collections.Generic;
using System.Text;
using System.Windows.Forms;
using System.Diagnostics;
using CyDesigner.Extensions.Common;
using CyDesigner.Extensions.Gde;
using System.Runtime.InteropServices;


namespace  CapSense_v1_30
{
    [CyCompDevCustomizer()]
    public partial class CyCustomizer : ICyParamEditHook_v1
    {
        CyCompDevParamEditorMode m_Mode = CyCompDevParamEditorMode.COMPLETE;

        #region ICyParamEditHook_v1 Members

        public DialogResult EditParams(ICyInstEdit_v1 edit, ICyTerminalQuery_v1 termQuery, ICyExpressMgr_v1 mgr)
        {
            CyMyICyParamEditTemplate.m_RunMode = true;

            //Get parameters with XML Data
            string XMLSerializ = edit.GetCommittedParam(CyGeneralParams.C_XML_MAIN_DATA).Value;

            //Deserialization
            CyGeneralParams packParams = (CyGeneralParams)CyGeneralParams.Deserialize(XMLSerializ, true);

            packParams.m_edit = edit;

            packParams.m_cyGeneralTab = new CyGeneralTab(packParams);
            packParams.m_cyClockSourceTab = new CyClockSwitchTab(packParams);

            ICyTabbedParamEditor editor = edit.CreateTabbedParamEditor();

            CyParamExprDelegate configureExpressionViewDataChanged =
            delegate(ICyParamEditor custEditor, CyCompDevParam param)
            {
                packParams.GetGeneralParameters(edit, param);
            };

            editor.AddCustomPage("General", packParams.m_cyGeneralTab, configureExpressionViewDataChanged, "General");
            editor.AddCustomPage("Clock Source", packParams.m_cyClockSourceTab,
                configureExpressionViewDataChanged, "Clock Source");
            editor.AddCustomPage("Buttons", packParams.m_cyButtonsTab = new CyButtonsTab(), null, "");
            editor.AddCustomPage("Sliders", packParams.m_cySlidersTab = new CySlidersTab(), null, "");
            editor.AddCustomPage("Touch Pads", packParams.m_cyTouchPadsTab = new CyTouchPadsTab(), null, "");
            editor.AddCustomPage("Matrix Buttons", packParams.m_cyMatrixButtonsTab = new CyMatrixButtonsTab(), null, 
                "");
            editor.AddCustomPage("Proximity Sensors", packParams.m_cyProximityTab = new CyProximityTab(), null, "");
            editor.AddCustomPage("Generic Sensors", packParams.m_cyGenericTab = new CyGenericTab(), null, "");
            editor.AddCustomPage("Scan Slots", packParams.m_cyScanSlotTab = new CyScanSlotsTab(packParams), null, "");
            editor.AddDefaultPage("Built-in", "Built-in");

            packParams.GetParams(edit,null);

            packParams.SetAllActions();
            DialogResult result = editor.ShowDialog();
            return result;
        }

        public bool EditParamsOnDrop
        {
            get
            {
                return false;
            }
        }

        public CyCompDevParamEditorMode GetEditorMode()
        {
            return m_Mode;
        }

        #endregion
    }

    #region CyMyICyParamEditTemplate
    public class CyMyICyParamEditTemplate : UserControl, ICyParamEditingControl
    {
        public event EventHandler m_actGlobalEditorGetErrors;
        public static bool m_RunMode=false;

        public CyMyICyParamEditTemplate()
        {
            m_actGlobalEditorGetErrors += new EventHandler(CyMyICyParamEditTemplate_actGlobalEditorGetErrors);
            this.Load += new EventHandler(CyMyICyParamEditTemplate_Load);
        }

        void CyMyICyParamEditTemplate_Load(object sender, EventArgs e)
        {
            this.Dock = DockStyle.Fill;
        }

        void CyMyICyParamEditTemplate_actGlobalEditorGetErrors(object sender, EventArgs e)
        { }
        protected override bool ProcessCmdKey(ref Message msg, Keys keyData)
        {
            if (keyData == Keys.Enter)
            {
                SendKeys.Send("{TAB}");
                return true;
            }
            else
                return base.ProcessCmdKey(ref msg, keyData);
        }
        #region ICyParamEditingControl Members
        public Control DisplayControl
        {
            get
            {
                return this;
            }
        }

        public IEnumerable<CyCustErr> GetErrors()
        {
            m_actGlobalEditorGetErrors(this, null);
            return new CyCustErr[] { };    //return an empty array
        }

        #endregion
    }
    #endregion
}


