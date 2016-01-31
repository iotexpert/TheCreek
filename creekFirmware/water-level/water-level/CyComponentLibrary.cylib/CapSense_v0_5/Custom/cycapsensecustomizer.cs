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


namespace  CapSense_v0_5
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
            //Debugger.Launch();
            M_ICyParamEditTemplate.FillAll = 1;            
            this.m_Component = edit;
           
            //Get parameters with XML Data
            string XMLSerializ = edit.GetCommittedParam(CyGeneralParams.p_XMLMainData).Value;

            //Deserialization
            CyGeneralParams packParams = (CyGeneralParams)CyGeneralParams.Deserialize(XMLSerializ);

            packParams.edit = edit;

            packParams.cyGeneralTab = new CyGeneralTab(packParams);
            packParams.cyClockSource = new CyClockSwitch(packParams);   
            
            ICyTabbedParamEditor editor = edit.CreateTabbedParamEditor();

            List<string> str = new List<string>();
            //editor.AddAllDefaultPages();
            editor.AddCustomPage("General", packParams.cyGeneralTab, null, (IEnumerable<string>)(str));
            editor.AddCustomPage("Clock Source", packParams.cyClockSource, null, (IEnumerable<string>)(str));  
            editor.AddCustomPage("Buttons", packParams.cyButtons = new CyButtons(), null, (IEnumerable<string>)(str));
            editor.AddCustomPage("Sliders", packParams.cySliders = new CySliders(), null, (IEnumerable<string>)(str));
            editor.AddCustomPage("Touch Pads", packParams.cyTouchPads = new CyTouchPads(), null, (IEnumerable<string>)(str));
            editor.AddCustomPage("Matrix Buttons", packParams.cyMatrixButtons = new CyMatrixButtons(), null, (IEnumerable<string>)(str));
            editor.AddCustomPage("Proximity Sensors", packParams.cyProximity = new CyProximity(), null, (IEnumerable<string>)(str));
            editor.AddCustomPage("Generic Sensors", packParams.cyGeneric = new CyGeneric(), null, (IEnumerable<string>)(str));
            editor.AddCustomPage("Scan Slots", packParams.createScanSlots(), null, (IEnumerable<string>)(str));

            editor.AddDefaultPage("Built-in", "Built-in");

            packParams.GetParams(edit);

            //Loading Action
            packParams.cyGeneralTab.LoadFormGeneralParams();
            packParams.cyScanSlots.LoadFormGeneralParams();

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

        #region ICyShapeCustomize_v1 Members

        public CyCustErr CustomizeShapes(
            ICyInstQuery_v1 instQuery,
            ICySymbolShapeEdit_v1 shapeEdit,
            ICyTerminalEdit_v1 termEdit)
        {
            if (instQuery.IsPreviewCanvas)
                return CyCustErr.OK;            
            //Debugger.Launch();

            //Get parameters with XML Data
            string XMLSerializ = instQuery.GetCommittedParam(CyGeneralParams.p_XMLMainData).Value;

            //Deserialization
            CyGeneralParams packParams = (CyGeneralParams)CyGeneralParams.Deserialize(XMLSerializ);
            foreach (CyAmuxBParams item in packParams.localParams.listCsHalfs)
                if (packParams.localParams.bCsHalfIsEnable(item))
                {
                    if (item.currectClk == null) return new CyCustErr("Please define clocks for both Sides");
                }
            //return new CyCustErr("Please define clocks for both Sides");
            return CyCustErr.OK;
        }
        #endregion

        bool InterceptHelp()
        {
            //Do whatever you want here.
            return true;
        }
    }

    #region M_ICyParamEditTemplate
    public class M_ICyParamEditTemplate : UserControl, ICyParamEditingControl
    {
        public static int FillAll = 0;
        public event EventHandler actGlobalEditorGetErrors;

        public M_ICyParamEditTemplate()
        {
            actGlobalEditorGetErrors += new EventHandler(M_ICyParamEditTemplate_actGlobalEditorGetErrors);
            if (FillAll > 0)
                this.Dock = DockStyle.Fill;
        }

        void M_ICyParamEditTemplate_actGlobalEditorGetErrors(object sender, EventArgs e)
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
            actGlobalEditorGetErrors(this, null);
            return new CyCustErr[] { };    //return an empty array
        }

        #endregion
    }
    #endregion
}


