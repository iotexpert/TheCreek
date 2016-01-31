/*******************************************************************************
* Copyright 2008-2009, Cypress Semiconductor Corporation.  All rights reserved.
* You may use this file only in accordance with the license, terms, conditions, 
* disclaimers, and limitations in the end user license agreement accompanying 
* the software package with which this file was provided.
********************************************************************************/

using System;
using System.Collections.Generic;
using System.Text;
using System.Drawing;
using System.Windows.Forms;
using System.ComponentModel;
using System.Diagnostics;

using CyDesigner.Common.Base;
using CyDesigner.Common.Base.Dialogs;
using CyDesigner.Extensions.Common;
using CyDesigner.Extensions.Gde;
using CyDesigner.Framework;

namespace Cypress.Components.System.cy_clock_v1_0
{
    public class CyCustomizer :
        ICyParamEditHook_v1,
        ICyToolTipCustomize_v1,
        ICyVerilogCustomize_v1,
        ICyClockDataProvider_v1,
        ICyAPICustomize_v1,
        ICyDesignClient_v1,
        ICyShapeCustomize_v1
    {

        const string AdvTabNoParamsMsg = "There are no Advanced parameters available for the current " +
                                "clock configuration. Only 'New' clocks have Advanced parameters to set.";

        #region ICyParamEditHook_v1 Members

        public CyCompDevParamEditorMode GetEditorMode()
        {
            return CyCompDevParamEditorMode.COMPLETE;
        }

        public DialogResult EditParams(ICyInstEdit_v1 edit, ICyTerminalQuery_v1 termQuery, ICyExpressMgr_v1 mgr)
        {
            CyClockParamInfo info = new CyClockParamInfo(edit);
            ICyDesignQuery_v1 design = edit.DesignQuery;

            CyClockParamEditingControl clkControl = new CyClockParamEditingControl(
                edit,
                GetClkType(info),
                GetTolInfo(info),
                CreateNewLocalClockSourceOptions(design),
                CreateExistingLocalClockSourceOptions(design),
                GetDefaultNewSrcClk(),
                GetDefaultExistingSrcClk(design),
                new CyFrequency(info.DesiredFreq, info.DesiredFreqUnit),
                info.Divider,
                GetInputClk(design, info.InputClkId, info.InputClkName));

            SetHelpProvider(clkControl);
            if (design.DesignInfoAvailable == false)
            {
                clkControl.Message = "Only 'Auto' clocks are supported in this schematic. This can happen when " +
                    "editing a device independant schematic of a component in a library project. All components, " +
                    "inculding clock components, must be able to work in all devices and families. If you need to " +
                    "access a device/family specific source clock, add a device/family specific implementation " +
                    "schematic to your library component.";
            }

            clkControl.FreqValidator = delegate(string value)
            {
                //the string coming in is the expression for the freq
                return SetAndValidateParam(edit, CyClockParamInfo.DesiredFreqParamName, value);
            };

            clkControl.FreqUnitValidator = delegate(string value)
            {
                return SetAndValidateParam(edit, CyClockParamInfo.DesiredFreqUnitParamName, value);
            };

            clkControl.DividerValidator = delegate(string value)
            {
                //the string coming in is the expression for the divider
                return SetAndValidateParam(edit, CyClockParamInfo.DivisorParamName, value);
            };

            clkControl.SourceClkIdValidator = delegate(string value)
            {
                return SetAndValidateParam(edit, CyClockParamInfo.SourceClockIdParamName, value);
            };

            clkControl.MinusValidator = delegate(string value)
            {
                return SetAndValidateParam(edit, CyClockParamInfo.MinusToleranceParamName, value);
            };

            clkControl.PlusValidator = delegate(string value)
            {
                return SetAndValidateParam(edit, CyClockParamInfo.PlusToleranceParamName, value);
            };

            CyAdvancedClockParamEditingControl advControl = new CyAdvancedClockParamEditingControl(edit,
                info.DirectConnect);
            advControl.SynchWithBusClk = info.SyncWithBusClk;

            CyParamExprDelegate configureExpressionViewDataChanged =
                delegate(ICyParamEditor custEditor, CyCompDevParam param)
                {
                    UpdateConfigureClkControl(edit, clkControl, param);
                };

            CyParamExprDelegate advExpressionViewDataChanged =
                delegate(ICyParamEditor custEditor, CyCompDevParam param)
                {
                    UpdateAdvClkControl(edit, advControl, param);
                };

            ICyTabbedParamEditor editor = edit.CreateTabbedParamEditor();

            CyParamExprDelegate updateAdvancedTab =
                delegate(ICyParamEditor custEditor, CyCompDevParam param)
                {
                    if (param.Name == CyClockParamInfo.IsDirectParamName ||
                        param.Name == CyClockParamInfo.IsDividerParamName ||
                        param.Name == CyClockParamInfo.IsDesiredFreqParamName)
                    {
                        bool isDirectConnect = CyClockParamInfo.GetDirectConnect(edit);
                        if (isDirectConnect)
                        {
                            editor.HideCustomPage("Advanced");
                        }
                        else
                        {
                            editor.ShowCustomPage("Advanced");
                        }
                    }
                };

            editor.ParamExprCommittedDelegate = updateAdvancedTab;
            editor.AddCustomPage("Configure Clock", clkControl, configureExpressionViewDataChanged, "Configure Clock");
            editor.AddCustomPage("Advanced", advControl, advExpressionViewDataChanged, "Advanced");
            editor.AddDefaultPage("Built-in", "Built-in");

            //initialize
            if (info.DirectConnect) editor.HideCustomPage("Advanced");

            DialogResult result = editor.ShowDialog();

            return result;
        }

        private void SetHelpProvider(CyClockParamEditingControl clkControl)
        {
            CyDocumentationPlugin plugin = CyDocumentationPlugin.GetAppPlugin();
            Debug.Assert(plugin != null);
            Debug.Assert(CyApp.GetCurrentApp() != null);
            if (plugin != null)
            {
                clkControl.HelpProvider =
                    CyApp.GetCurrentApp().CreateHelpProvider(clkControl, plugin.Topics.ConfigureClock);
            }
        }

        private CyInputClock GetDefaultExistingSrcClk(ICyDesignQuery_v1 design)
        {
            CyInputClock srcClk = null;
            if (design.DesignInfoAvailable)
            {
                string id = design.GetDefaultExistingClkID();
                srcClk = GetInputClk(design, id, design.GetClockName(id));

                if (srcClk == null)
                {
                    //return the first clock from the list if no BUS_CLK
                    foreach (CyInputClock clk in CreateExistingLocalClockSourceOptions(design))
                    {
                        return clk;
                    }
                }
            }
            return srcClk;
        }

        private CyInputClock GetDefaultNewSrcClk()
        {
            return CyInputClock.CreateAutoClock();
        }

        private CyClockControl.CyClockToleranceInfo GetTolInfo(CyClockParamInfo info)
        {
            return new CyClockControl.CyClockToleranceInfo(
                info.PlusTolerancePercent,
                info.MinusTolerancePercent,
                info.CheckTolerance);
        }

        private CyClockControl.CyClockType GetClkType(CyClockParamInfo info)
        {
            if (info.UseDesiredFreq)
            {
                Debug.Assert(info.UseDivider == false && info.DirectConnect == false);
                return CyClockControl.CyClockType.Freq;
            }
            else if (info.UseDivider)
            {
                Debug.Assert(info.UseDesiredFreq == false && info.DirectConnect == false);
                return CyClockControl.CyClockType.Divider;
            }
            else
            {
                Debug.Assert(info.DirectConnect == true);
                Debug.Assert(info.UseDesiredFreq == false && info.UseDivider == false);
                return CyClockControl.CyClockType.Existing;
            }
        }

        private CyInputClock GetInputClk(ICyDesignQuery_v1 design, string clkID, string clkName)
        {
            if (design.DesignInfoAvailable && design.IsValidClockID(clkID))
            {
                double freq;
                byte unit;
                design.GetClockActualFreq(clkID, out freq, out unit);

                return new CyInputClock(
                    clkName,
                    clkID,
                    new CyFrequency(freq, unit),
                    design.GetClockDivider(clkID),
                    design.GetClockPlusAccuracyPercent(clkID),
                    design.GetClockMinusAccuracyPercent(clkID),
                    design.GetClockAPIPrefix(clkID),
                    design.GetClockEnabled(clkID));
            }
            else
            {
                return new CyInputClock(clkName, clkID, CyFrequency.UndefinedFreq, 0, 0, 0, string.Empty, false);
            }
        }

        IEnumerable<CyInputClock> CreateNewLocalClockSourceOptions(ICyDesignQuery_v1 design)
        {
            /* New local clocks can chhose from the following:
             * 1. Auto
             * 2. System clocks (except BUS_CLK)
             * 3. Design-wide clocks
             * 
             * The default is Auto.
             */

            List<CyInputClock> clocks = new List<CyInputClock>();
            CyInputClock autoClk = CyInputClock.CreateAutoClock();
            clocks.Add(autoClk);

            if (design != null && design.DesignInfoAvailable)
            {
                _addClocksFromDesign(clocks, design, design.IsValidNewLocalReference);
            }

            return clocks;
        }

        IEnumerable<CyInputClock> CreateExistingLocalClockSourceOptions(ICyDesignQuery_v1 design)
        {
            /* Existing local clocks can choose from the following:
             * 1. System clocks that allow direct connect
             * 2. Design-wide clocks
             * 
             * The default is BUS_CLK.
             */

            List<CyInputClock> clocks = new List<CyInputClock>();
            if (design != null && design.DesignInfoAvailable)
            {
                _addClocksFromDesign(clocks, design, design.IsValidExistingLocalReference);
            }
            return clocks;
        }

        delegate bool CyFilter(string id);

        void _addClocksFromDesign(List<CyInputClock> clocks, ICyDesignQuery_v1 design, CyFilter clkFilter)
        {
            double freqValue;
            byte freqUnit;
            CyFrequency freq;
            uint divider;
            double plusPercent;
            double minusPercent;
            string apiPrefix;
            foreach (string clkID in design.ClockIDs)
            {
                if (clkFilter(clkID))
                {
                    string clkName = design.GetClockName(clkID);

                    if (design.GetClockActualFreq(clkID, out freqValue, out freqUnit))
                    {
                        freq = new CyFrequency(freqValue, freqUnit);
                    }
                    else
                    {
                        freq = CyFrequency.UndefinedFreq;
                    }

                    divider = design.GetClockDivider(clkID);
                    plusPercent = design.GetClockPlusAccuracyPercent(clkID);
                    minusPercent = design.GetClockMinusAccuracyPercent(clkID);
                    apiPrefix = design.GetClockAPIPrefix(clkID);

                    clocks.Add(new CyInputClock(clkName, clkID, freq, divider, plusPercent, minusPercent,
                        apiPrefix, design.GetClockEnabled(clkID)));
                }
            }
        }

        void UpdateAdvClkControl(ICyInstQuery_v1 query, CyAdvancedClockParamEditingControl ctrl, CyCompDevParam param)
        {
            CyClockParamInfo info = new CyClockParamInfo(query);
            CyCompDevParam tempParam;

            switch (param.Name)
            {
                case CyClockParamInfo.SyncWithBusClkParamName:
                    tempParam = query.GetCommittedParam(CyClockParamInfo.SyncWithBusClkParamName);
                    if (tempParam.ErrorCount == 0)
                    {
                        ctrl.SynchWithBusClk = tempParam.GetValueAs<bool>();
                    }
                    break;

                default:
                    Debug.Fail("unhandled param change");
                    break;
            }
        }

        /// <summary>
        /// Updated custom clock age when data is changed on the expression view of the page.
        /// </summary>
        /// <param name="query"></param>
        /// <param name="ctrl"></param>
        /// <param name="param"></param>
        void UpdateConfigureClkControl(ICyInstQuery_v1 query, CyClockParamEditingControl ctrl, CyCompDevParam param)
        {
            CyClockParamInfo info = new CyClockParamInfo(query);
            CyCompDevParam tempParam;

            switch (param.Name)
            {
                case CyClockParamInfo.DesiredFreqParamName:
                case CyClockParamInfo.DesiredFreqUnitParamName:
                    tempParam = query.GetCommittedParam(CyClockParamInfo.DesiredFreqUnitParamName);
                    if (tempParam.ErrorCount == 0)
                    {
                        byte freqUnit = tempParam.GetValueAs<byte>();

                        tempParam = query.GetCommittedParam(CyClockParamInfo.DesiredFreqParamName);
                        if (tempParam.ErrorCount == 0)
                        {
                            double freq = tempParam.GetValueAs<double>();
                            ctrl.Frequency = new CyFrequency(freq, freqUnit);
                        }
                    }
                    break;

                case CyClockParamInfo.DivisorParamName:
                    tempParam = query.GetCommittedParam(CyClockParamInfo.DivisorParamName);
                    if (tempParam.ErrorCount == 0)
                    {
                        ctrl.Divider = tempParam.GetValueAs<uint>();
                    }
                    break;

                case CyClockParamInfo.IsDesiredFreqParamName:
                case CyClockParamInfo.IsDirectParamName:
                case CyClockParamInfo.IsDividerParamName:
                    ctrl.ClockType = GetClkType(info);
                    break;

                case CyClockParamInfo.MinusToleranceParamName:
                case CyClockParamInfo.PlusToleranceParamName:
                case CyClockParamInfo.CheckToleranceParamName:
                    ctrl.ToleranceInfo = GetTolInfo(info);
                    break;

                case CyClockParamInfo.SourceClockIdParamName:
                case CyClockParamInfo.SourceClockNameParamName:
                    CyCompDevParam inputIdParam = query.GetCommittedParam(CyClockParamInfo.SourceClockIdParamName);
                    CyCompDevParam inputNameParam = query.GetCommittedParam(CyClockParamInfo.SourceClockNameParamName);

                    ctrl.SetSourceClk(inputIdParam.Value, inputNameParam.Value);
                    break;

                default:
                    Debug.Fail("unhandled param change");
                    break;
            }
        }

        CyErr SetAndValidateParam(ICyInstEdit_v1 edit, string paramName, string value)
        {
            edit.SetParamExpr(paramName, value);
            if (edit.CommitParamExprs())
                return CyErr.OK;
            CyCompDevParam param = edit.GetCommittedParam(paramName);

            if (param.ErrorCount > 0)
            {
                return new CyErr(param.ErrorMsgs);
            }
            return CyErr.OK;
        }

        public bool EditParamsOnDrop
        {
            get { return false; }
        }

        #endregion

        #region ICyToolTipCustomize_v1 Members

        /// <summary>
        /// Adds the input clock name, and the desired frequency or the divider 
        /// (which ever one is used) to the default tooltip text.
        /// </summary>
        /// <param name="query"></param>
        /// <returns></returns>
        public string CustomizeToolTip(ICyInstQuery_v1 query, ICyTerminalQuery_v1 termQuery)
        {
            string newToolTip = query.DefaultToolTipText;
            CyClockParamInfo info = new CyClockParamInfo(query);

            //Show Input Clock
            string inputClkName = (string.IsNullOrEmpty(info.InputClkId)) ? "<Auto>" : info.InputClkName;
            newToolTip += string.Format("{0}: {1}\n", "Input Clock", inputClkName);

            //Show Desired Frequency
            if (info.UseDesiredFreq)
            {
                newToolTip += string.Format("{0}: {1}\n", "Desired Frequency", info.PrettyFreq);
            }

            //Show Divider
            if (info.UseDivider)
            {
                newToolTip += string.Format("{0}: {1}\n", "Divider", info.Divider.ToString());
            }

            //Show that the clock is a direct connect
            if (info.DirectConnect)
            {
                newToolTip += string.Format("{0}\n", "Existing Clock");
            }

            return newToolTip;
        }

        #endregion

        #region Inner Class

        public class CyAdvancedClockParamEditingControl : CyAdvancedClkControl, ICyParamEditingControl
        {
            ICyInstEdit_v1 m_editObj;

            public CyAdvancedClockParamEditingControl(ICyInstEdit_v1 editObj, bool isDirectConnect)
                : base()
            {
                this.Dock = DockStyle.Fill;
                m_editObj = editObj;
            }

            #region Set Expr Values When Changes Made

            protected override void OnSyncWithBusClkChanged()
            {
                if (this.Visible)
                {
                    if (m_editObj != null)
                    {
                        m_editObj.SetParamExpr(CyClockParamInfo.SyncWithBusClkParamName,
                            (SynchWithBusClk) ? "true" : "false");
                        m_editObj.CommitParamExprs();
                    }
                }
                base.OnSyncWithBusClkChanged();
            }

            #endregion

            #region ICyParamEditingControl Members

            public Control DisplayControl
            {
                get { return this; }
            }

            public IEnumerable<CyCustErr> GetErrors()
            {
                string[] paramNames = new string[]{
                    CyClockParamInfo.SyncWithBusClkParamName,
                };

                List<CyCustErr> errs = new List<CyCustErr>();

                foreach (string paramName in paramNames)
                {
                    CyCompDevParam param = m_editObj.GetCommittedParam(paramName);
                    if (param.ErrorCount > 0)
                    {
                        foreach (string errMsg in param.Errors)
                        {
                            errs.Add(new CyCustErr(errMsg));
                        }
                    }
                }

                return errs;
            }

            #endregion
        }

        public class CyClockParamEditingControl : CyClockControl, ICyParamEditingControl
        {
            ICyInstEdit_v1 m_editObj = null;

            public CyClockParamEditingControl(ICyInstEdit_v1 editObj, CyClockType clkType, CyClockToleranceInfo tolInfo,
                IEnumerable<CyInputClock> newClkSrcOptions, IEnumerable<CyInputClock> existingClkSrcOptions,
                CyInputClock defaultNewClkSrc, CyInputClock defaultExistingClkSrc, CyFrequency freq, uint divider,
                CyInputClock srcClk)
                : base()
            {
                m_editObj = editObj;
                this.Dock = DockStyle.Fill;

                Initialize(clkType, tolInfo, newClkSrcOptions, existingClkSrcOptions,
                    new CyPair<object, CyClockControl.CyGetClockDelegate>[] { }, defaultNewClkSrc,
                    defaultExistingClkSrc, freq, divider, srcClk);
            }

            #region Set Expr Values When Changes Made

            protected override void OnFreqChanged()
            {
                if (this.Visible)
                {
                    if (m_editObj != null)
                    {
                        m_editObj.SetParamExpr(CyClockParamInfo.DesiredFreqParamName, Frequency.FreqValue.ToString());
                        m_editObj.SetParamExpr(CyClockParamInfo.DesiredFreqUnitParamName, Frequency.Unit.ToString());
                    }
                }
                base.OnFreqChanged();
            }

            protected override void OnDividerChanged()
            {
                if (this.Visible)
                {
                    if (m_editObj != null)
                    {
                        m_editObj.SetParamExpr(CyClockParamInfo.DivisorParamName, Divider.ToString());
                    }
                }
                base.OnDividerChanged();
            }

            protected override void OnSourceClkChanged()
            {
                if (this.Visible)
                {
                    if (m_editObj != null)
                    {
                        m_editObj.SetParamExpr(CyClockParamInfo.SourceClockIdParamName, SourceClk.Id);
                        m_editObj.SetParamExpr(CyClockParamInfo.SourceClockNameParamName, SourceClk.Name);
                    }
                }
                base.OnSourceClkChanged();
            }

            protected override void OnUseFreqChanged()
            {
                if (this.Visible)
                {
                    if (m_editObj != null)
                    {
                        m_editObj.SetParamExpr(CyClockParamInfo.IsDesiredFreqParamName,
                            (ClockType == CyClockType.Freq) ? "true" : "false");

                        //All other param changes will be committed when they are validated.
                        m_editObj.CommitParamExprs();
                    }
                }
                base.OnUseFreqChanged();
            }

            protected override void OnIsNewChanged()
            {
                if (this.Visible)
                {
                    if (m_editObj != null)
                    {
                        m_editObj.SetParamExpr(CyClockParamInfo.IsDirectParamName,
                            (ClockType == CyClockType.Existing) ? "true" : "false");

                        //All other param changes will be committed when they are validated.
                        m_editObj.CommitParamExprs();
                    }
                }
                base.OnIsNewChanged();
            }

            protected override void OnUseDividerChanged()
            {
                if (this.Visible)
                {
                    if (m_editObj != null)
                    {
                        m_editObj.SetParamExpr(CyClockParamInfo.IsDividerParamName,
                            (ClockType == CyClockType.Divider) ? "true" : "false");

                        //All other param changes will be committed when they are validated.
                        m_editObj.CommitParamExprs();
                    }
                }
                base.OnUseDividerChanged();
            }

            protected override void OnPlusToleranceChanged()
            {
                if (this.Visible)
                {
                    if (m_editObj != null)
                    {
                        m_editObj.SetParamExpr(CyClockParamInfo.PlusToleranceParamName,
                            ToleranceInfo.PlusTolerancePercent.ToString());
                    }
                }
                base.OnPlusToleranceChanged();
            }

            protected override void OnMinusToleranceChanged()
            {
                if (this.Visible)
                {
                    if (m_editObj != null)
                    {
                        m_editObj.SetParamExpr(CyClockParamInfo.MinusToleranceParamName,
                            ToleranceInfo.MinusTolerancePercent.ToString());
                    }
                }
                base.OnMinusToleranceChanged();
            }

            protected override void OnCheckToleranceChanged()
            {
                if (this.Visible)
                {
                    if (m_editObj != null)
                    {
                        m_editObj.SetParamExpr(CyClockParamInfo.CheckToleranceParamName,
                            (ToleranceInfo.UseToleranceChecked) ? "true" : "false");

                        //All other param changes will be committed when they are validated.
                        m_editObj.CommitParamExprs();
                    }
                }
                base.OnCheckToleranceChanged();
            }

            #endregion

            #region ICyParamEditingControl Members

            public Control DisplayControl
            {
                get { return this; }
            }

            public IEnumerable<CyCustErr> GetErrors()
            {
                string[] paramNames = new string[]{
                    CyClockParamInfo.CheckToleranceParamName,
                    CyClockParamInfo.DesiredFreqParamName,
                    CyClockParamInfo.DesiredFreqUnitParamName,
                    CyClockParamInfo.DivisorParamName,
                    CyClockParamInfo.IsDesiredFreqParamName,
                    CyClockParamInfo.IsDirectParamName,
                    CyClockParamInfo.IsDividerParamName,
                    CyClockParamInfo.MinusToleranceParamName,
                    CyClockParamInfo.PlusToleranceParamName,
                    CyClockParamInfo.SourceClockIdParamName,
                    CyClockParamInfo.SourceClockNameParamName
                };

                List<CyCustErr> errs = new List<CyCustErr>();

                foreach (string paramName in paramNames)
                {
                    CyCompDevParam param = m_editObj.GetCommittedParam(paramName);
                    if (param.ErrorCount > 0)
                    {
                        foreach (string errMsg in param.Errors)
                        {
                            errs.Add(new CyCustErr(errMsg));
                        }
                    }
                }

                return errs;
            }

            #endregion
        }

        class CyClockParamInfo
        {
            //------------------------------------

            #region Param Names

            public const string SyncWithBusClkParamName = "SyncWithBusClk";
            public const string InstanceNameParamName = "INSTANCE_NAME";
            public const string ShortInstanceNameParamName = "CY_INSTANCE_SHORT_NAME";
            public const string IsDesiredFreqParamName = "is_freq";
            public const string DesiredFreqParamName = "desired_freq";
            public const string DesiredFreqUnitParamName = "desired_freq_unit";
            public const string IsDividerParamName = "is_divider";
            public const string DivisorParamName = "divisor";
            public const string SourceClockIdParamName = "source_clock_id";
            public const string SourceClockNameParamName = "source_clock_name";
            public const string IsDirectParamName = "is_direct";
            public const string MinusToleranceParamName = "minus_tolerance";
            public const string PlusToleranceParamName = "plus_tolerance";
            public const string CheckToleranceParamName = "CheckTolerance";

            #endregion

            //------------------------------------

            #region Member Variables

            bool m_useDesiredFreq;
            double m_desiredFreq;
            byte m_desiredFreqUnit;
            bool m_useDivider;
            string m_dividerExpr;
            uint m_divider;
            string m_inputClkId;
            string m_inputClkName;
            string m_desiredFreqExpr;
            bool m_directConnect;
            double m_minusTolerancePercent;
            double m_plusTolerancePercent;
            bool m_checkTolerance;
            string m_instanceName;
            bool m_syncWithBusClk;

            #endregion

            //------------------------------------

            #region Properties

            public bool SyncWithBusClk
            {
                get { return m_syncWithBusClk; }
            }

            public string ShortInstanceName
            {
                get { return m_instanceName; }
                private set { m_instanceName = value; }
            }

            public bool CheckTolerance
            {
                get { return m_checkTolerance; }
            }

            public double MinusTolerancePercent
            {
                get { return m_minusTolerancePercent; }
            }

            public double PlusTolerancePercent
            {
                get { return m_plusTolerancePercent; }
            }

            public bool DirectConnect
            {
                get { return m_directConnect; }
            }

            public bool UseDesiredFreq
            {
                get { return m_useDesiredFreq; }
            }

            public double DesiredFreq
            {
                get { return m_desiredFreq; }
            }

            public string DesiredFreqExpr
            {
                get { return m_desiredFreqExpr; }
            }

            public byte DesiredFreqUnit
            {
                get { return m_desiredFreqUnit; }
            }

            public bool UseDivider
            {
                get { return m_useDivider; }
            }

            public string DividerExpr
            {
                get { return m_dividerExpr; }
            }

            public uint Divider
            {
                get { return m_divider; }
            }

            public string InputClkId
            {
                get { return m_inputClkId; }
            }

            public string InputClkName
            {
                get { return m_inputClkName; }
            }

            public string PrettyFreq
            {
                get
                {
                    CyFrequency freq = new CyFrequency(DesiredFreq, DesiredFreqUnit);
                    return freq.ToString();
                }
            }

            #endregion

            //------------------------------------

            #region Constructors

            public CyClockParamInfo(ICyInstQuery_v1 query)
            {
                CyCompDevParam instNameParam = query.GetCommittedParam(ShortInstanceNameParamName);
                Debug.Assert(instNameParam.ErrorCount == 0);
                ShortInstanceName = instNameParam.Value;

                CyCompDevParam useDesiredFreqParam = query.GetCommittedParam(IsDesiredFreqParamName);
                if (useDesiredFreqParam.ErrorCount == 0)
                {
                    m_useDesiredFreq = useDesiredFreqParam.GetValueAs<bool>();
                }
                else
                {
                    m_useDesiredFreq = default(bool);
                }

                CyCompDevParam checkToleranceFreqParam = query.GetCommittedParam(CheckToleranceParamName);
                if (checkToleranceFreqParam.ErrorCount == 0)
                {
                    m_checkTolerance = checkToleranceFreqParam.GetValueAs<bool>();
                }
                else
                {
                    m_checkTolerance = default(bool);
                }

                CyCompDevParam syncWithBusClkParam = query.GetCommittedParam(SyncWithBusClkParamName);
                if (syncWithBusClkParam.ErrorCount == 0)
                {
                    m_syncWithBusClk = syncWithBusClkParam.GetValueAs<bool>();
                }
                else
                {
                    m_syncWithBusClk = default(bool);
                }


                CyCompDevParam desiredFreqParam = query.GetCommittedParam(DesiredFreqParamName);
                if (desiredFreqParam.ErrorCount == 0)
                {
                    m_desiredFreq = desiredFreqParam.GetValueAs<double>();
                }
                else
                {
                    double desiredFreqVal;
                    if (double.TryParse(desiredFreqParam.Value, out desiredFreqVal))
                    {
                        m_desiredFreq = desiredFreqVal;
                    }
                    else
                    {
                        m_desiredFreq = default(double);
                    }
                }
                m_desiredFreqExpr = desiredFreqParam.Expr;

                CyCompDevParam desiredFreqUnitParam = query.GetCommittedParam(DesiredFreqUnitParamName);
                if (desiredFreqUnitParam.ErrorCount == 0)
                {
                    m_desiredFreqUnit = desiredFreqUnitParam.GetValueAs<byte>();
                }
                else
                {
                    byte desiredFreqUnit;
                    if (byte.TryParse(desiredFreqUnitParam.Value, out desiredFreqUnit))
                    {
                        m_desiredFreqUnit = desiredFreqUnit;
                    }
                    else
                    {
                        m_desiredFreqUnit = default(byte);
                    }
                }

                CyCompDevParam useDividerParam = query.GetCommittedParam(IsDividerParamName);
                if (useDividerParam.ErrorCount == 0)
                {
                    m_useDivider = useDividerParam.GetValueAs<bool>();
                }
                else
                {
                    m_useDivider = default(bool);
                }

                CyCompDevParam dividerParam = query.GetCommittedParam(DivisorParamName);
                if (dividerParam.ErrorCount == 0)
                {
                    m_divider = dividerParam.GetValueAs<uint>();
                }
                else
                {
                    uint dividerVal;
                    if (uint.TryParse(dividerParam.Value, out dividerVal))
                    {
                        m_divider = dividerVal;
                    }
                    else
                    {
                        m_divider = default(uint);
                    }
                }
                m_dividerExpr = dividerParam.Expr;

                CyCompDevParam inputClkIdParam = query.GetCommittedParam(SourceClockIdParamName);
                if (inputClkIdParam.ErrorCount == 0)
                    m_inputClkId = inputClkIdParam.Value;

                CyCompDevParam inputClkNameParam = query.GetCommittedParam(SourceClockNameParamName);
                m_inputClkName = inputClkNameParam.Value;

                m_directConnect = GetDirectConnect(query);

                CyCompDevParam plusTolerenceParam = query.GetCommittedParam(PlusToleranceParamName);
                if (plusTolerenceParam.ErrorCount == 0)
                {
                    m_plusTolerancePercent = plusTolerenceParam.GetValueAs<double>();
                }
                else
                {
                    double plusTolerancePercentVal;
                    if (double.TryParse(plusTolerenceParam.Value, out plusTolerancePercentVal))
                    {
                        m_plusTolerancePercent = plusTolerancePercentVal;
                    }
                    else
                    {
                        m_plusTolerancePercent = default(double);
                    }
                }

                CyCompDevParam minusTolerenceParam = query.GetCommittedParam(MinusToleranceParamName);
                if (minusTolerenceParam.ErrorCount == 0)
                {
                    m_minusTolerancePercent = minusTolerenceParam.GetValueAs<double>();
                }
                else
                {
                    m_minusTolerancePercent = default(double);

                    double minusTolerancePercentVal;
                    if (double.TryParse(minusTolerenceParam.Value, out minusTolerancePercentVal))
                    {
                        m_minusTolerancePercent = minusTolerancePercentVal;
                    }
                    else
                    {
                        m_minusTolerancePercent = default(double);
                    }
                }

#if DEBUG
                if (UseDesiredFreq)
                {
                    Debug.Assert(!UseDivider && !DirectConnect, "only one should be set at a time");
                }
                else if (UseDivider)
                {
                    Debug.Assert(!UseDesiredFreq && !DirectConnect, "only one should be set at a time");
                }
                else if (DirectConnect)
                {
                    Debug.Assert(!UseDesiredFreq && !UseDivider, "only one should be set at a time");
                }
                else
                {
                    Debug.Fail("one of these should be true");
                }
#endif
            }

            #endregion

            public static bool GetDirectConnect(ICyInstQuery_v1 query)
            {
                CyCompDevParam directConnectParam = query.GetCommittedParam(IsDirectParamName);
                if (directConnectParam.ErrorCount == 0)
                {
                    return directConnectParam.GetValueAs<bool>();
                }
                else
                {
                    return default(bool);
                }
            }

            //------------------------------------
        }

        #endregion

        #region ICyVerilogCustomize_v1 Members

        CyCustErr ICyVerilogCustomize_v1.CustomizeVerilog(
            ICyInstQuery_v1 query, ICyTerminalQuery_v1 termQuery, out string codeSnippet)
        {
            ICyDesignQuery_v1 design = query.DesignQuery;
            Debug.Assert(design.DesignInfoAvailable);

            StringBuilder sb = new StringBuilder();

            string srcClkID;
            double desiredFreq;
            byte desiredUnit;
            uint divider;
            bool isFreq, isDivider, isDirect;

            string compName, instName, sigName;

            try
            {
                sigName = termQuery.GetTermSigSegName("clock_out");

                CyCompDevParam compNameParam = query.GetCommittedParam("CY_COMPONENT_NAME");
                compName = compNameParam.Value;

                instName = query.InstanceName;

                CyCompDevParam srcClkIDParam = query.GetCommittedParam(CyClockParamInfo.SourceClockIdParamName);
                srcClkID = srcClkIDParam.Value;

                CyCompDevParam desiredFreqParam = query.GetCommittedParam(CyClockParamInfo.DesiredFreqParamName);
                desiredFreq = double.Parse(desiredFreqParam.Value);

                CyCompDevParam desiredUnitParam = query.GetCommittedParam(CyClockParamInfo.DesiredFreqUnitParamName);
                desiredUnit = byte.Parse(desiredUnitParam.Value);

                CyCompDevParam dividerParam = query.GetCommittedParam(CyClockParamInfo.DivisorParamName);
                divider = uint.Parse(dividerParam.Value);

                CyCompDevParam useDesiredFreqParam = query.GetCommittedParam(CyClockParamInfo.IsDesiredFreqParamName);
                isFreq = bool.Parse(useDesiredFreqParam.Value);

                CyCompDevParam useDividerParam = query.GetCommittedParam(CyClockParamInfo.IsDividerParamName);
                isDivider = bool.Parse(useDividerParam.Value);

                CyCompDevParam directConnectParam = query.GetCommittedParam(CyClockParamInfo.IsDirectParamName);
                isDirect = bool.Parse(directConnectParam.Value);
            }
            catch (Exception e)
            {
                codeSnippet = "";
                return new CyCustErr(e, "Error in clock customizer (malformed parameter): ", e.Message);
            }

            // N.B., The weird shift by 31 then by 1 works around the fact that shifts
            // are performed modulo 32, and we need to shift a full 32 bits.
            double period = CyFrequency.FreqToPeriod(desiredFreq, desiredUnit);

            sb.Append(compName);
            sb.Append(" ");
            sb.Append(instName);
            sb.Append("(");
            sb.Append(".clock_out(");
            sb.Append(sigName);
            sb.Append("));");
            sb.Append(Environment.NewLine);

            bool isAuto = String.IsNullOrEmpty(srcClkID);
            bool isDigital = design.IsClockDigital(query.InstanceIdPath);

            const string periodName = "period";
            const string idName = "id";
            const string isDigitalName = "is_digital";

            if (isAuto)
            {
                GenerateDefParam(sb, instName, idName, "\"" + query.InstanceIdPath + "\"");
                GenerateDefParam(sb, instName, CyClockParamInfo.SourceClockIdParamName, "\"\"");
                GenerateDefParam(sb, instName, CyClockParamInfo.IsDirectParamName, "0");
                GenerateDefParam(sb, instName, CyClockParamInfo.DivisorParamName, "0");
                GenerateDefParam(sb, instName, periodName, "\"" + period.ToString() + "\"");
                GenerateDefParam(sb, instName, isDigitalName, isDigital ? "1" : "0");
            }
            else if (isDivider)
            {
                GenerateDefParam(sb, instName, idName, "\"" + query.InstanceIdPath + "\"");
                GenerateDefParam(sb, instName, CyClockParamInfo.SourceClockIdParamName, "\"" + srcClkID + "\"");
                GenerateDefParam(sb, instName, CyClockParamInfo.IsDirectParamName, "0");
                GenerateDefParam(sb, instName, CyClockParamInfo.DivisorParamName, divider.ToString());
                GenerateDefParam(sb, instName, periodName, "\"0\"");
                GenerateDefParam(sb, instName, isDigitalName, isDigital ? "1" : "0");
            }
            else if (isDirect)
            {
                GenerateDefParam(sb, instName, idName, "\"" + query.InstanceIdPath + "\"");
                GenerateDefParam(sb, instName, CyClockParamInfo.SourceClockIdParamName, "\"" + srcClkID + "\"");
                GenerateDefParam(sb, instName, CyClockParamInfo.IsDirectParamName, "1");
                GenerateDefParam(sb, instName, CyClockParamInfo.DivisorParamName, "0");
                GenerateDefParam(sb, instName, periodName, "\"0\"");
                GenerateDefParam(sb, instName, isDigitalName, isDigital ? "1" : "0");
            }
            else if (isFreq)
            {
                GenerateDefParam(sb, instName, idName, "\"" + query.InstanceIdPath + "\"");
                GenerateDefParam(sb, instName, CyClockParamInfo.SourceClockIdParamName, "\"" + srcClkID + "\"");
                GenerateDefParam(sb, instName, CyClockParamInfo.IsDirectParamName, "0");
                GenerateDefParam(sb, instName, CyClockParamInfo.DivisorParamName, "0");
                GenerateDefParam(sb, instName, periodName, "\"" + period.ToString() + "\"");
                GenerateDefParam(sb, instName, isDigitalName, isDigital ? "1" : "0");
            }
            else
            {
                Debug.Fail("unexpected configuration of a clock component");
                codeSnippet = "";
                return new CyCustErr(
                    "Unexpected clock component configuration (not auto, direct, named+divider, or named+frequency");
            }

            codeSnippet = sb.ToString();

            return CyCustErr.Ok;
        }

        void GenerateDefParam(StringBuilder sb, string instName, string paramName, string value)
        {
            sb.Append("defparam ");
            sb.Append(instName);
            sb.Append(".");
            sb.Append(paramName);
            sb.Append(" = ");
            sb.Append(value);
            sb.Append(";");
            sb.Append(Environment.NewLine);
        }

        #endregion

        #region ICyClockDataProvider_v1 Members

        public CyClockData GetClockInfo(ICyInstQuery_v1 query, ICyTerminalQuery_v1 termQuery, string termName, int ix)
        {
            if (termQuery.DoesTermExist(termName) == false)
                return CyClockData.GetUnknownClock();

            if (termQuery.GetTermMinIdx(termName) < ix || ix > termQuery.GetTermMaxIdx(termName))
                return CyClockData.GetUnknownClock();

            ICyDesignQuery_v1 design = query.DesignQuery;
            if (design.DesignInfoAvailable == false)
                return CyClockData.GetUnknownClock();

            double freq;
            byte exp;

            bool exists = design.GetClockActualFreq(query.InstanceIdPath, out freq, out exp);
            if (!exists)
                return CyClockData.GetUnknownClock();

            return new CyClockData(freq, exp);
        }

        #endregion

        #region ICyAPICustomize_v1 Members

        public IEnumerable<CyAPICustomizer> CustomizeAPIs(ICyInstQuery_v1 query, ICyTerminalQuery_v1 termQuery,
            IEnumerable<CyAPICustomizer> apis)
        {
            //For design-wide clocks (which are always new clocks) query and termQuery will be null.
            //This is the ONLY case in which query and termQuery will be null.

            //Only generate an API for 'new' clocks. 'Existing' clocks do not get an API.

            if (query == null)
            {
                //Design-wide new clock, return the APIs unaltered.
                return apis;
            }

            CyClockParamInfo info = new CyClockParamInfo(query);
            if (info.DirectConnect)
            {
                //Existing clock, supress API generation.
                return new CyAPICustomizer[] { };
            }

            //The clock is a 'new' clock, return the APIs unaltered.
            return apis;
        }

        #endregion

        #region ICyShapeCustomize_v1 Members
        const string TitleShapeTage = "Title";
        const string BodyShapeTage = "Body";
        const string WaveformShapeTage = "waveform";

        public CyCustErr CustomizeShapes(ICyInstQuery_v1 query, ICySymbolShapeEdit_v1 shapeEdit,
            ICyTerminalEdit_v1 termEdit)
        {
            CyCustErr err = CyCustErr.OK;

            //Waveform Color----------------------------------------

            err = shapeEdit.SetOutlineWidth(WaveformShapeTage, 0);
            if (err.IsNotOk) return err;

            //These default colors represent no design info available, set to white to indicate the domain is unknown.
            Color waveFormFillColor = Color.White;
            Color waveFormOutlineColor = Color.White;

            if (query.DesignQuery.DesignInfoAvailable)
            {
                //Get all the Ids assocaited with this clock by using the clock's terminal.
                List<string> ids = termEdit.GetClockIds("clock_out", 0);

                bool containsDigital = false;
                bool containsAnalog = false;

                foreach (string id in ids)
                {
                    Debug.Assert(query.DesignQuery.IsValidClockID(id), 
                        "termEdit.GetClockIds is returning invalid data");

                    if (query.DesignQuery.IsValidClockID(id))
                    {
                        bool isDigital = query.DesignQuery.IsClockDigital(id);
                        containsDigital |= isDigital;
                        containsAnalog |= (!isDigital);
                    }
                }

                //This clock can be used in multiple places due to heirarchy.
                //If all the domains for this clock are the same, it can be displayed; otherwise the domain is
                //unknown and the default colors should be used.
                if (containsDigital && !containsAnalog)
                {
                    waveFormFillColor = query.Preferences.DigitalWireColor;
                    waveFormOutlineColor = Color.Black;
                }
                else if (!containsDigital && containsAnalog)
                {
                    waveFormFillColor = query.Preferences.AnalogWireColor;
                    waveFormOutlineColor = Color.Black;
                }
            }

            err = shapeEdit.SetFillColor(WaveformShapeTage, waveFormFillColor);
            if (err.IsNotOk) return err;

            err = shapeEdit.SetOutlineColor(WaveformShapeTage, waveFormOutlineColor);
            if (err.IsNotOk) return err;

            err = shapeEdit.SetOutlineWidth(WaveformShapeTage, 0);
            if (err.IsNotOk) return err;

            //Instance Name-----------------------------------------
            
            CyClockParamInfo info = new CyClockParamInfo(query);
            string srcName;

            if (query.DesignQuery.DesignInfoAvailable && query.DesignQuery.IsValidClockID(info.InputClkId))
            {
                //Get the most up-to-date name if available.
                srcName = query.DesignQuery.GetClockName(info.InputClkId);
            }
            else
            {
                srcName = "Unknown Source Clock";
            }

            RectangleF bounds = shapeEdit.GetShapeBounds(BodyShapeTage);
            StringFormat fmt = new StringFormat();
            fmt.Alignment = StringAlignment.Far;
            fmt.LineAlignment = StringAlignment.Center;

            err = shapeEdit.CreateAnnotation(new string[] { TitleShapeTage },
                string.Format("`=($is_direct) ? \"{0}\" : $INSTANCE_NAME`", srcName),
                new PointF(bounds.Left, bounds.Top + (bounds.Height / 2.0f)),
                new Font(FontFamily.GenericSansSerif, 10, FontStyle.Regular),
                fmt);
            if (err.IsNotOk) return err;

            err = shapeEdit.SetFillColor(TitleShapeTage, Color.Black);
            if (err.IsNotOk) return err;

            err = shapeEdit.ClearOutline(TitleShapeTage);
            if (err.IsNotOk) return err;

            return err;
        }

        #endregion

        #region ICyDesignClient_v1 Members

        public bool IsToolTipDesignSensitive
        {
            get { return false; }
        }

        //The color is updated to show the domain results once calculated (cdt 46691).
        //The text is updated in the case where direct connect to a global clock that has been renamed (cdt 50021).
        public bool IsShapeDesignSensitive
        {
            get { return true; }
        }

        #endregion
    }
}
