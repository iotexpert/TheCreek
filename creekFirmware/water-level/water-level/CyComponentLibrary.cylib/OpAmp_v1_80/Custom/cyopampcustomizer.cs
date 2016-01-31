/*******************************************************************************
* Copyright 2008-2012, Cypress Semiconductor Corporation.  All rights reserved.
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

namespace OpAmp_v1_80
{
    [CyCompDevCustomizer()]
    public class CyCustomizer : ICyParamEditHook_v1, ICyDRCProvider_v1
    {
        #region ICyParamEditHook_v1 Members
        public ICyInstEdit_v1 m_Component = null;
        CyCompEditingControl CompEditingControl = null;
        OPAMPcontrol m_control = null;
        const string PSOC3 = "PSoC3A";
        const string PSOC5LP = "PSoC5LP";
        const string PSOC5A = "PSoC5A";
        const int LPOC = 0;
        const int LOWPOWER = 1;
        const int MEDPOWER = 2;
        const string VerifyPSOC5PowerModeErrMsg = "DRC CHECK: This power mode is not supported on PSoC5A. Please select High Power Mode.";

        public DialogResult EditParams(ICyInstEdit_v1 edit, ICyTerminalQuery_v1 termQuery, ICyExpressMgr_v1 mgr)
        {
            m_control = new OPAMPcontrol(edit);
            m_Component = edit;

            ICyTabbedParamEditor editor = edit.CreateTabbedParamEditor();
            CompEditingControl = new CyCompEditingControl(edit, termQuery, m_control);

            CyParamExprDelegate ExprDelegate = delegate(ICyParamEditor custEditor, CyCompDevParam param)
            {
                m_control.UpdateFormFromParams(edit);
            };

            editor.AddCustomPage("Configure", CompEditingControl, new CyParamExprDelegate(ExprDelegate), "Basic");
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

        //Create a new control to add to a tab
        public class CyCompEditingControl : ICyParamEditingControl
        {
            OPAMPcontrol m_control;
            Panel displayControl = new Panel();

            public CyCompEditingControl(ICyInstEdit_v1 inst, ICyTerminalQuery_v1 termquery, OPAMPcontrol control)
            {
                m_control = control;
                displayControl.Dock = DockStyle.Fill;
                displayControl.AutoScroll = true;
                displayControl.AutoScrollMinSize = m_control.Size;

                m_control.Dock = DockStyle.Fill;
                displayControl.Controls.Add(m_control);
            }

            #region ICyParamEditingControl Members
            public Control DisplayControl
            {
                get { return displayControl; }
            }

            public IEnumerable<CyCustErr> GetErrors()
            {
                return new CyCustErr[] { };    //return an empty array
            }

            #endregion
        }
       
        #region ICyDRCProvider_v1 Members
        public IEnumerable<CyDRCInfo_v1> GetDRCs(ICyDRCProviderArgs_v1 args)
        {
            CyCustErr err = VerifyPowerMode(args);
            if (err.IsOk == false)
                yield return new CyDRCInfo_v1(CyDRCInfo_v1.CyDRCType_v1.Error, err.Message);
                       
                CyCompDevParam powerMode = args.InstQueryV1.GetCommittedParam(OPAMPParameters.POWER);
                if ((Convert.ToInt32(powerMode.Value) == LOWPOWER) || (Convert.ToInt32(powerMode.Value) == LPOC))
                {
                    CyCustErr error = VerifyPowerMode(args);

                    if (error.IsOk == false)
                        yield return new CyDRCInfo_v1(CyDRCInfo_v1.CyDRCType_v1.Error, error.Message);
                }            
        }

        CyCustErr VerifyPowerMode(ICyDRCProviderArgs_v1 drcQuery)
        {
                CyCompDevParam powerMode = drcQuery.InstQueryV1.GetCommittedParam(OPAMPParameters.POWER);
                if ((drcQuery.DeviceQueryV1.IsPSoC5 == true) && (drcQuery.DeviceQueryV1.ArchFamilyMember == PSOC5A))
                {
                    if ((Convert.ToInt32(powerMode.Value) == LOWPOWER) || (Convert.ToInt32(powerMode.Value) == MEDPOWER) || (Convert.ToInt32(powerMode.Value) == LPOC))
                    {
                        return new CyCustErr(OpAmp_v1_80.Properties.Resources.VerifyPSOC5PowerModeErrMsg);
                    }
                }
                return CyCustErr.OK;
        }
    }
        #endregion
}



