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
using System.Drawing;


namespace FanController_v2_20
{
    [CyCompDevCustomizer()]
    public class CyCustomizer : ICyParamEditHook_v1, ICyShapeCustomize_v1, ICyAPICustomize_v2, ICyInstValidateHook_v1
    {
        #region ICyParamEditHook_v1 Members

        public DialogResult EditParams(ICyInstEdit_v1 edit, ICyTerminalQuery_v1 termQuery, ICyExpressMgr_v1 mgr)
        {
            CyParameters parameters = new CyParameters();
            parameters.m_inst = edit;

            CyEditingWrapperControl.RUN_MODE = true;

            parameters.m_controlfans = new CyFansTab(parameters);
            CyBasicTab m_control_basic = new CyBasicTab(parameters);

            ICyTabbedParamEditor editor = edit.CreateTabbedParamEditor();
            editor.UseBigEditor = true;

            CyParamExprDelegate ExprDelegate = delegate(ICyParamEditor custEditor, CyCompDevParam param)
            {
                parameters.m_globalEditMode = false;
                m_control_basic.UpdateFormFromParams();
                parameters.m_controlfans.UpdateFormFromParams();
                parameters.m_globalEditMode = true;
            };

            editor.AddCustomPage(Resources.BasicTab, m_control_basic, new CyParamExprDelegate(ExprDelegate),
                m_control_basic.TabName);
            editor.AddCustomPage(Resources.FansTab, parameters.m_controlfans,
                new CyParamExprDelegate(ExprDelegate), parameters.m_controlfans.TabName);
            editor.AddDefaultPage(Resources.BuiltInTab, "Built-in");


            parameters.m_globalEditMode = true;
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

        #region ICyShapeCustomize_v1

        const string TACH_BUS = "tach_bus";
        const string FAN_BUS = "fan_bus";
        const string BANK_BUS = "bank_bus";
        const string TERM_PATERN = "{0}[{1}:1]";
        public CyCustErr CustomizeShapes(ICyInstQuery_v1 instQuery, ICySymbolShapeEdit_v1 shapeEdit,
    ICyTerminalEdit_v1 termEdit)
        {
            CyCustErr err;
            // We leave the symbol as it is for symbol preview
            if (instQuery.IsPreviewCanvas)
                return CyCustErr.OK;

            // Read Parameters
            CyCompDevParam numTerminals_param = instQuery.GetCommittedParam(CyParameters.P_NUMBER_OF_FANS);
            uint numFans = uint.Parse(numTerminals_param.Value);
            numTerminals_param = instQuery.GetCommittedParam(CyParameters.P_NUMBER_OF_BANKS);
            uint numBanks = uint.Parse(numTerminals_param.Value);

            string tach_bus = termEdit.GetTermName(TACH_BUS);
            string fan_bus = termEdit.GetTermName(FAN_BUS);
			string bank_bus = termEdit.GetTermName(BANK_BUS);

            uint maxBitIndex = numFans;
            err = termEdit.TerminalRename(tach_bus, string.Format(TERM_PATERN, TACH_BUS, maxBitIndex));
            if (err.IsNotOK) return err;
            if (numBanks == 0)
                err = termEdit.TerminalRename(fan_bus, string.Format(TERM_PATERN, FAN_BUS, maxBitIndex));
            else
                err = termEdit.TerminalRename(bank_bus, string.Format(TERM_PATERN, BANK_BUS, (numBanks)));

            if (err.IsNotOK) return err;

            return CyCustErr.OK;
        }
        #endregion

        #region ICyAPICustomize_v1
        public IEnumerable<CyAPICustomizer> CustomizeAPIs(ICyAPICustomizeArgs_v2 args,
            IEnumerable<CyAPICustomizer> apis)
        {
            Dictionary<string, string> m_dict = new Dictionary<string, string>();
            List<CyAPICustomizer> customizers = new List<CyAPICustomizer>(apis);
            if (customizers.Count == 0) return customizers;
            m_dict = customizers[0].MacroDictionary;
            CyParameters prm = new CyParameters();
            prm.m_inst = args.InstQuery;

            //Calculate and add INITIAL_DUTY_CYCLE parameter
            for (int ii = 0; ii < CyParameters.MAX_FANS; ii++)
            {
                double dutyRPMSlope = ((double)(prm.GetMaxDuty(ii) - prm.GetMinDuty(ii))) / (prm.GetMaxRPM(ii)
                    - prm.GetMinRPM(ii));

                int diff = Math.Abs(prm.GetInitialRPM(ii) - prm.GetMinRPM(ii));

                double diffDuty = diff * dutyRPMSlope;

                int initDuty = 0;
                if (prm.GetInitialRPM(ii) > prm.GetMinRPM(ii))
                    initDuty = Convert.ToInt32(prm.GetMinDuty(ii) + diffDuty);
                else
                    initDuty = Convert.ToInt32(prm.GetMinDuty(ii) - diffDuty);

                m_dict.Add(CyParameters.GetName(CyParameters.P_INITIAL_DUTY_CYCLE, ii), initDuty.ToString());
            }


            foreach (CyAPICustomizer cust in customizers)
            {
                cust.MacroDictionary = m_dict;
            }

            return customizers;
        }
        #endregion

        #region ICyInstValidateHook_v1
        public void ValidateParams(ICyInstValidate_v1 instVal)
        {
            CyParameters prms = new CyParameters();
            prms.m_instVal = instVal;

            bool bankMode = prms.NumberOfBanks > 0 && prms.FanMode == CyFanModeType.FIRMWARE;
            Byte fanCount = bankMode ? prms.NumberOfBanks : prms.NumberOfFans;
            string msg = "";
            string pName = "";
            for (int i = 0; i < fanCount; i++)
            {
                if ((prms.GetMinRPM(i) < CyParameters.MIN_RPM_NUD) ||
                    (prms.GetMinRPM(i) > CyParameters.MAX_RPM_NUD))
                {
                    pName = CyParameters.GetName(CyParameters.P_MIN_RPM, i);
                    msg = String.Format(Resources.ErrorDrcValueRange, pName,
                                        CyParameters.MIN_RPM_NUD, CyParameters.MAX_RPM_NUD);
                    instVal.AddError(pName, new CyCustErr(msg));
                }

                if ((prms.GetMaxRPM(i) < CyParameters.MIN_RPM_NUD) ||
                    (prms.GetMaxRPM(i) > CyParameters.MAX_RPM_NUD))
                {
                    pName = CyParameters.GetName(CyParameters.P_MAX_RPM, i);
                    msg = String.Format(Resources.ErrorDrcValueRange, pName,
                                        CyParameters.MIN_RPM_NUD, CyParameters.MAX_RPM_NUD);
                    instVal.AddError(pName, new CyCustErr(msg));
                }

                if ((prms.GetMinDuty(i) < CyParameters.MIN_DUTY_NUD) ||
                    (prms.GetMinDuty(i) > CyParameters.MAX_DUTY_NUD))
                {
                    pName = CyParameters.GetName(CyParameters.P_MIN_DUTY, i);
                    msg = String.Format(Resources.ErrorDrcValueRange, pName,
                                        CyParameters.MIN_DUTY_NUD, CyParameters.MAX_DUTY_NUD);
                    instVal.AddError(pName, new CyCustErr(msg));
                }

                if ((prms.GetMaxDuty(i) < CyParameters.MIN_DUTY_NUD) ||
                    (prms.GetMaxDuty(i) > CyParameters.MAX_DUTY_NUD))
                {
                    pName = CyParameters.GetName(CyParameters.P_MAX_DUTY, i);
                    msg = String.Format(Resources.ErrorDrcValueRange, pName,
                                        CyParameters.MIN_DUTY_NUD, CyParameters.MAX_DUTY_NUD);
                    instVal.AddError(pName, new CyCustErr(msg));
                }

                if ((prms.GetInitialRPM(i) < CyParameters.MIN_RPM_NUD) ||
                    (prms.GetInitialRPM(i) > CyParameters.MAX_RPM_NUD))
                {
                    pName = CyParameters.GetName(CyParameters.P_INITIAL_RPM, i);
                    msg = String.Format(Resources.ErrorDrcValueRange, pName,
                                        CyParameters.MIN_RPM_NUD, CyParameters.MAX_RPM_NUD);
                    instVal.AddError(pName, new CyCustErr(msg));
                }

                if (prms.GetMaxRPM(i) <= prms.GetMinRPM(i))
                {
                    CyCustErr err = new CyCustErr(string.Format(Resources.ErrorRPMVal, 
                        (i + 1).ToString().PadLeft(2, '0')));
                    instVal.AddError(CyParameters.GetName(CyParameters.P_MAX_RPM, i), err);
                }

                if (prms.GetMaxDuty(i) <= prms.GetMinDuty(i))
                {
                    CyCustErr err = new CyCustErr(string.Format(Resources.ErrorDutyVal, 
                        (i + 1).ToString().PadLeft(2, '0')));
                    instVal.AddError(CyParameters.GetName(CyParameters.P_MAX_DUTY, i), err);
                }
            }
        }
        #endregion

    }

    #region Wrapper for Tabs
    public class CyEditingWrapperControl : UserControl, ICyParamEditingControl
    {
        public static bool RUN_MODE = false;
        protected CyParameters m_prms = null;


        public virtual string TabName
        {
            get { return "Empty"; }
        }

        public CyEditingWrapperControl()
        {
            this.Load += delegate(object sender, EventArgs e)
            {
                this.AutoScroll = true;
                if (RUN_MODE)
                    this.Dock = DockStyle.Fill;
            };
        }
        public CyEditingWrapperControl(CyParameters param)
            : this()
        {
            m_prms = param;
        }


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
            get { return this; }
        }

        // Gets any errors that exist for parameters on the DisplayControl.
        public virtual IEnumerable<CyCustErr> GetErrors()
        {
            foreach (string paramName in m_prms.m_inst.GetParamNames())
            {
                CyCompDevParam param = m_prms.m_inst.GetCommittedParam(paramName);
                if (param.TabName.Equals(TabName))
                {
                    if (param.ErrorCount > 0)
                    {
                        foreach (string errMsg in param.Errors)
                        {
                            yield return new CyCustErr(errMsg);
                        }
                    }
                }
            }

        }
        #endregion
    }
    #endregion
}
