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


namespace  CapSense_v1_10
{
    [CyCompDevCustomizer()]
    public partial class CyCustomizer : ICyParamEditHook_v1//, ICyShapeCustomize_v1
    {
        bool m_EditParamsOnDrop = false;
        CyCompDevParamEditorMode m_Mode = CyCompDevParamEditorMode.COMPLETE;
        ICyInstEdit_v1 m_Component = null;

        #region ICyParamEditHook_v1 Members

        public DialogResult EditParams(ICyInstEdit_v1 edit, ICyTerminalQuery_v1 termQuery, ICyExpressMgr_v1 mgr)
        {
            //Debugger.Break();
            CyMyICyParamEditTemplate.m_FillAll = 1;            
            this.m_Component = edit;
           
            //Get parameters with XML Data
            string XMLSerializ = edit.GetCommittedParam(CyGeneralParams.p_XMLMainData).Value;

            //Deserialization
            CyGeneralParams packParams = (CyGeneralParams)CyGeneralParams.Deserialize(XMLSerializ,true);

            packParams.m_edit = edit;

            packParams.m_cyGeneralTab = new CyGeneralTab(packParams);
            packParams.m_cyClockSource = new CyClockSwitch(packParams);   
            
            ICyTabbedParamEditor editor = edit.CreateTabbedParamEditor();

            List<string> str = new List<string>();
            //editor.AddAllDefaultPages();
            editor.AddCustomPage("General", packParams.m_cyGeneralTab, null, (IEnumerable<string>)(str));
            editor.AddCustomPage("Clock Source", packParams.m_cyClockSource, null, (IEnumerable<string>)(str));  
            editor.AddCustomPage("Buttons", packParams.m_cyButtons = new CyButtons(),
                null, (IEnumerable<string>)(str));
            editor.AddCustomPage("Sliders", packParams.m_cySliders = new CySliders(), 
                null, (IEnumerable<string>)(str));
            editor.AddCustomPage("Touch Pads", packParams.m_cyTouchPads = new CyTouchPads(), 
                null, (IEnumerable<string>)(str));
            editor.AddCustomPage("Matrix Buttons", packParams.m_cyMatrixButtons = new CyMatrixButtons(), 
                null, (IEnumerable<string>)(str));
            editor.AddCustomPage("Proximity Sensors", packParams.m_cyProximity = new CyProximity(), 
                null, (IEnumerable<string>)(str));
            editor.AddCustomPage("Generic Sensors", packParams.m_cyGeneric = new CyGeneric(), 
                null, (IEnumerable<string>)(str));
            editor.AddCustomPage("Scan Slots", packParams.CreateScanSlots(), null, (IEnumerable<string>)(str));

            editor.AddDefaultPage("Built-in", "Built-in");

            packParams.GetParams(edit);

            //Loading Action
            packParams.m_cyGeneralTab.LoadFormGeneralParams();
            packParams.m_cyScanSlots.LoadFormGeneralParams();

            packParams.SetAllActions();
            DialogResult result = editor.ShowDialog();
            editor.InterceptHelpRequest = new CyHelpDelegate(InterceptHelp);
            return result;
        }

        public bool EditParamsOnDrop
        {
            get
            {
                return m_EditParamsOnDrop;
            }
        }

        public CyCompDevParamEditorMode GetEditorMode()
        {
            return m_Mode;
        } 
       
        #endregion

        bool InterceptHelp()
        {
            //Do whatever you want here.
            return true;
        }
    }

    #region CyMyICyParamEditTemplate
    public class CyMyICyParamEditTemplate : UserControl, ICyParamEditingControl
    {
        public static int m_FillAll = 0;
        public event EventHandler m_actGlobalEditorGetErrors;

        public CyMyICyParamEditTemplate()
        {
            m_actGlobalEditorGetErrors += new EventHandler(CyMyICyParamEditTemplate_actGlobalEditorGetErrors);
            if (m_FillAll > 0)
                this.Dock = DockStyle.Fill;
        }

        void CyMyICyParamEditTemplate_actGlobalEditorGetErrors(object sender, EventArgs e)
        { }

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


