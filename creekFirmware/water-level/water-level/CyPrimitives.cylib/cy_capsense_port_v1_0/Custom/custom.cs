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
using System.Runtime.InteropServices;
using System.IO;

using ActiproSoftware.SyntaxEditor;

using CyDesigner.Common.Base;
using CyDesigner.Common.Base.Controls;
using CyDesigner.Common.Base.Dialogs;
using CyDesigner.Extensions.Common;
using CyDesigner.Extensions.Gde;
using CyDesigner.PSoC.PSoC3.Fitter.DesignWideResources;

using Cypress.Components.System.Ports.CommonPortCode;

namespace Cypress.Components.System.Ports.cy_capsense_port_v1_0
{
    public class CyCustomizer :
        ICyParamEditHook_v1,
        ICyVerilogCustomize_v1,
        ICyShapeCustomize_v1,
        ICyInstValidateHook_v1
    {
        /// <summary>
        /// Workaround to shielding not working on current silicon.
        /// </summary>
        const string m_iBaseTerminalName = "i";
        const string m_aBaseTerminalName = "a";
        const string m_clockBaseTerminalName = "clock";

        const string m_builtinTabName = "Built-in";
        const string m_basicTabName = "Basic";

        //-----------------------------

        #region ICyParamEditHook_v1 Members

        public CyCompDevParamEditorMode GetEditorMode()
        {
            return CyCompDevParamEditorMode.COMPLETE;
        }

        /// <summary>
        /// Displays a custom parameter editor to the user to allow them to edit the port params.
        /// </summary>
        /// <param name="edit"></param>
        /// <param name="mgr"></param>
        /// <returns></returns>
        public DialogResult EditParams(ICyInstEdit_v1 edit, ICyTerminalQuery_v1 termQuery, ICyExpressMgr_v1 mgr)
        {
            CyCapSenseParamInfo info = new CyCapSenseParamInfo();

            CyPortPinEditingControl pinsDGV = new CyPortPinEditingControl(edit);
            pinsDGV.Dock = DockStyle.Fill;

            ICyPortsPinColumnInfo[] columns = new ICyPortsPinColumnInfo[] {
                    new CyPinAliasColumnInfo(edit),
                    new CyEnableShieldingColumnInfo(edit),
                };

            pinsDGV.SetupColumns(columns, info.GetWidth(edit));
            pinsDGV.Columns[CyEnableShieldingColumnInfo.ColName].Visible = info.GetShieldingAvailable(edit);

            CyParamExprDelegate paramExprCommitted = delegate(ICyParamEditor custEditor, CyCompDevParam param)
            {
                //only update for valid data
                if (param.ErrorCount == 0)
                {
                    if (param.Name == CyPortParamInfo.ParamNameWidth)
                    {
                        byte width = info.GetWidth(edit);
                        pinsDGV.UpdatePins(width);
                    }
                    if (param.Name == CyCapSenseParamInfo.ParamNameShieldingAvailable)
                    {
                        pinsDGV.Columns[CyEnableShieldingColumnInfo.ColName].Visible = param.GetValueAs<bool>();
                    }
                }
            };

            CyParamExprDelegate expressionViewDataChanged = delegate(ICyParamEditor custEditor, CyCompDevParam param)
            {
                byte width = info.GetWidth(edit);
                pinsDGV.UpdatePins(width);
            };

            ICyTabbedParamEditor editor = edit.CreateTabbedParamEditor();
            editor.ParamExprCommittedDelegate = paramExprCommitted;
            editor.AddDefaultPage(m_basicTabName, m_basicTabName);
            editor.AddCustomPage("Pins", pinsDGV, expressionViewDataChanged, "Pins");
            editor.AddDefaultPage(m_builtinTabName, m_builtinTabName);
            return editor.ShowDialog();
        }

        /// <summary>
        /// Gets whether or not EditParams should be called when a port is initailly
        /// dropped onto a canvas.
        /// </summary>
        public bool EditParamsOnDrop
        {
            get { return false; }
        }

        #endregion

        //-----------------------------

        #region ICyVerilogCustomize_v1 Members

        public CyCustErr CustomizeVerilog(ICyInstQuery_v1 query, ICyTerminalQuery_v1 termQuery, out string codeSnippet)
        {
            /* Example:
             * 
             * cy_psoc3_port_v1_0
             * #(.width(8),
             *   .id("7fb63523-3721-4e66-848c-192f0924dfc3"),
             *   .pin_aliases("pin0name,pin1name,,,,pin5name,,"),
             *   .access_mode("HW_ONLY"),
             *   .layout_mode("CONTIGUOUS"),
             *   .port_mode("INOUT"),
             *   .intr_mode(16'b00 00 00 00 00 00 00 00),
             *   .drive_mode(16'b00 00 00 00 00 00 00 00))
             * port_1
             *  (.oe(wire_1),
             *   .y(Terminal_1),
             *   .fb(foo),
             *   .analog(8'b0),
             *   .interrupt(wire_2));
             */

            StringBuilder sb = new StringBuilder();
            CyCapSenseParamInfo info = new CyCapSenseParamInfo();

            string termName_a = termQuery.GetTermName(m_aBaseTerminalName);
            string termName_clock = termQuery.GetTermName(m_clockBaseTerminalName);
            string termName_i = termQuery.GetTermName(m_iBaseTerminalName);

            //outputs and inouts don't need to be hooked up (they can be left open)----------
            string sigAName = (termQuery.GetHasNoDrivers(termName_a)) ?
               string.Empty : termQuery.GetTermSigSegName(termName_a);
            //-------------------------------------------------------------------------------

            string sigClockName = (termQuery.GetHasNoDrivers(termName_clock)) ?
              termQuery.GetTermDefaultVlogExpr(termName_clock) :
              termQuery.GetTermSigSegName(termName_clock);

            string sigIName = (info.GetShieldingAvailable(query)) ?
             string.Empty : termQuery.GetTermSigSegName(termName_i);

            byte width = info.GetWidth(query);

            CyVerilogWriter vw = new CyVerilogWriter("cy_psoc3_port_v1_0", info.GetShortInstanceName(query));
            vw.AddGeneric("width", width.ToString());
            vw.AddGeneric("siorefwidth", ((width + 1) / 2).ToString());
            vw.AddGeneric("id", "\"" + query.InstanceIdPath + "\"");
            vw.AddGeneric("pin_aliases", info.GetPinAliasVerilog(query, width));
            vw.AddGeneric("access_mode", string.Format("\"{0}\"", info.GetVerilogAccessMode(query)));
            vw.AddGeneric("layout_mode", string.Format("\"{0}\"", info.GetVerilogLayoutMode(query)));
            vw.AddGeneric("port_mode", string.Format("\"{0}\"", info.GetVerilogDirection(query)));
            vw.AddGeneric("intr_mode", info.GetVerilogInterruptModeBitArray(query, width));
            vw.AddGeneric("drive_mode", info.GetVerilogPinModeBitArray(query, width));
            vw.AddGeneric("por_state", info.GetVerilogPORState(query));
            vw.AddGeneric("enable_shielding", info.GetVerilogEnableShieldingBitArray(query, width));
            vw.AddGeneric("cs_mode", info.VerilogIOMode(query));

            // POST PR4 NOTE:
            //     Prior to PR4, the OE signals are 'active low', meaning, a 0 will
            //     enable the output, a 1 would turn it off. CDT 32449 is tracking the
            //     PR4 change to the device to make it 'active high', as it should have
            //     been.
            // vw.AddPort("oe", "1'b0");
            string netOEName = "tmpOE__" + info.GetShortInstanceName(query) + "_net";
            vw.AddWire(netOEName, 0, 0);
            vw.AssignPreES3CondWire(netOEName, "1'b1", "1'b0");
            vw.AddPort("oe", netOEName); //input

            //This is a workaround (i.e. complete HACK) for an issue in the silicon. See cdt 35808.
            //y is just used as a way to get the info to the otherside.
            if (string.IsNullOrEmpty(sigIName) == false)
            {  
                //y - INPUT
                string yValues = "";

                if (width > 1)
                    yValues = "{";

                for (int i = 0; i < width; i++)
                {
                    if (i == 0)
                        yValues += sigIName;
                    else
                        yValues = yValues + ", " + sigIName;
                }

                if (width > 1)
                    yValues += "}";

                vw.AddPort("y", yValues);
            }
            else
            {
                vw.AddPort("y", string.Format("{0}'b0", width.ToString()));  //y - INPUT
            }

            if (!string.IsNullOrEmpty(sigAName)) vw.AddPort("analog", sigAName); //inout
            vw.AddPort("precharge", sigClockName); // input

            codeSnippet = vw.ToString();
            return CyCustErr.Ok;
        }

        #endregion

        //-----------------------------

        #region ICyShapeCustomize_v1 Members

        public CyCustErr CustomizeShapes(
            ICyInstQuery_v1 query, ICySymbolShapeEdit_v1 shapeEdit, ICyTerminalEdit_v1 termEdit)
        {
            CyCapSenseParamInfo info = new CyCapSenseParamInfo();
            CyCustErr err;

            Debug.Assert(termEdit.TerminalCount > 0);

            string termName_a = termEdit.GetTermName(m_aBaseTerminalName);

            shapeEdit.SetFillColor("body", query.Preferences.SchematicAnalogTerminalColor);

            byte width = info.GetWidth(query);

            int maxBitIndex = width - 1;
            string newName = (maxBitIndex == 0) ? m_aBaseTerminalName :
                              string.Format("{0}[{1}:0]", m_aBaseTerminalName, maxBitIndex.ToString());
            err = termEdit.TerminalRename(termName_a, newName);
            if (err.IsNotOK) return err;

            return CyCustErr.OK;
        }

        public bool DoesShapeIncludeDesignInformation
        {
            get { return false; }
        }

        #endregion

        //-----------------------------

        #region ICyInstValidateHook_v1 Members

        public void ValidateParams(ICyInstValidate_v1 instVal)
        {
            CyCapSenseParamInfo info = new CyCapSenseParamInfo();

            //validate pin aliases
            CyPortParamInfo.ValidatePinAliases(info, instVal);
            CyCapSenseParamInfo.ValidateEnableShielding(info, instVal);
        }

        #endregion
    }

    #region Helper Classes

    internal class CyEnableShieldingColumnInfo : CyPinColumnInfo
    {
        public const string ColName = "EnableShieldingColumn";

        readonly CyDGVEnhancedTextBoxColumn m_column;

        public CyEnableShieldingColumnInfo(ICyInstEdit_v1 edit)
            : base(edit, CyCapSenseParamInfo.ParamNamePinEnableShielding, 1, "false")
        {
            m_column = new CyDGVEnhancedTextBoxColumn();
            m_column.HeaderText = "Enable Shielding";
            m_column.ReadOnly = false;
            m_column.Name = ColName;
            m_column.AutoSizeMode = DataGridViewAutoSizeColumnMode.AllCells;
            m_column.SortMode = DataGridViewColumnSortMode.Automatic;
            m_column.MemberListDisplaying +=
                new EventHandler<CyMemberListEventArgs>(m_column_MemberListDisplaying);
        }

        void m_column_MemberListDisplaying(object sender, CyMemberListEventArgs e)
        {
            List<IntelliPromptMemberListItem> items = new List<IntelliPromptMemberListItem>();

            items.Add(new IntelliPromptMemberListItem("false",
                (int)ActiproSoftware.Products.SyntaxEditor.IconResource.Keyword));

            items.Add(new IntelliPromptMemberListItem("true",
                (int)ActiproSoftware.Products.SyntaxEditor.IconResource.Keyword));

            e.MemberListData = items.ToArray();
            e.Handled = true;
        }

        public override DataGridViewColumn Column
        {
            get { return m_column; }
        }

        public override string ColumnName
        {
            get { return m_column.Name; }
        }

        public override bool AddExpressionWrapper
        {
            get { return true; }
        }
    }

    internal class CyCapSenseParamInfo : CyPortParamInfo
    {
        #region Verilog Values

        public override string GetVerilogAccessMode(ICyInstQuery_v1 query)
        {
            return VerilogAccessMode_HW_ONLY;
        }

        public override string GetVerilogDirection(ICyInstQuery_v1 query)
        {
            return VerilogDirection_ANALOG;
        }

        protected override string VerilogInterruptMode(ICyInstQuery_v1 query, int pinNum)
        {
            return VerilogInterruptMode_None;
        }

        protected override string VerilogPinMode(ICyInstQuery_v1 query, int pinNum)
        {
            if (GetShieldingAvailable(query) == false)
                return VerilogPinMode_OpenDrainLo;
            else if (VerilogEnableShielding(query, pinNum) == VerilogEnableShielding_True)
                return VerilogPinMode_CMOSOut;

            return VerilogPinMode_OpenDrainLo;
        }

        public override string GetVerilogPORState(ICyInstQuery_v1 query)
        {
            return GetVerilogPORState(CyPortParamInfo.CyPORState.InDisabledOutHiZ);
        }

        #endregion

        #region IO Mode Control

        //LCDDrive enum values
        public const string EnumValueIOModeControl_None = "1";
        public const string EnumValueIOModeControl_PullUp = "2";
        public const string EnumValueIOModeControl_PullDown = "3";

        //LCDDrive param names
        const string ParamNameIOMode = "IOMode";

        public string VerilogIOMode(ICyInstQuery_v1 query)
        {
            CyCompDevParam param = query.GetCommittedParam(ParamNameIOMode);
            string paramValue = param.Value;

            if (paramValue == EnumValueIOModeControl_PullUp)
            {
                return "2'b01"; //1
            }
            if (paramValue == EnumValueIOModeControl_None)
            {
                return "2'b10"; //2
            }
            if (paramValue == EnumValueIOModeControl_PullDown)
            {
                return "2'b11"; //3
            }

            Debug.Fail("unhandled IOMode (Capsense Mode)");
            return "2'b00"; //0 (Neither i.e. not capsense)
        }

        #endregion

        #region Enable Shielding

        public const string ParamNamePinEnableShielding = "PinEnableShielding";
        const string VerilogEnableShielding_False = "0";
        const string VerilogEnableShielding_True = "1";

        internal static void ValidateEnableShielding(CyCapSenseParamInfo info, ICyInstValidate_v1 instVal)
        {
            //validate pin Interrupt modes
            uint width = info.GetWidth(instVal);

            for (int i = 0; i < width; i++)
            {
                string enabled;
                CyCustErr err = info.GetEnableShieldingValue(instVal, i, out enabled);
                bool enabledValue;
                if (err.IsNotOk)
                {
                    instVal.AddError(CyCapSenseParamInfo.ParamNamePinEnableShielding,
                            new CyCustErr(string.Format("pin{0}: {1}", i, err.Message)));
                }
                else if (!bool.TryParse(enabled, out enabledValue))
                {
                    //the expression must have been replaced with an error msg
                    instVal.AddError(CyCapSenseParamInfo.ParamNamePinEnableShielding,
                            new CyCustErr(string.Format("pin{0}: {1}", i, enabled)));
                }
            }
        }

        public string GetVerilogEnableShieldingBitArray(ICyInstQuery_v1 query, int width)
        {
            string bits = string.Empty;
            for (int i = 0; i < width; i++)
            {
                bits += VerilogEnableShielding(query, i);
            }
            bits = bits.Trim();
            string bArray = string.Format("{0}'b{1}", bits.Length, bits);
            return bArray;
        }

        string VerilogEnableShielding(ICyInstQuery_v1 query, int pinNum)
        {
            if (GetShieldingAvailable(query) == false) return VerilogEnableShielding_False;

            string paramValue = GetEnableShieldingValue(query, pinNum);

            if (paramValue == "false") return VerilogEnableShielding_False;
            if (paramValue == "true") return VerilogEnableShielding_True;

            //unknown value
            return string.Empty;
        }

        public string GetEnableShieldingValue(ICyInstQuery_v1 query, int pinNum)
        {
            CyCompDevParam param = query.GetCommittedParam(ParamNamePinEnableShielding);
            CyPinInfo info;
            CyCustErr err = CyPinInfo.LoadInfo(param.Value, ParamNamePinEnableShielding, 1, true, out info);
            return info.GetPinInfo(pinNum);
        }

        public CyCustErr GetEnableShieldingValue(ICyInstValidate_v1 query, int pinNum, out string value)
        {
            CyCompDevParam param = query.GetCommittedParam(ParamNamePinEnableShielding);
            CyPinInfo info;
            CyCustErr err = CyPinInfo.LoadInfo(param.Value, ParamNamePinEnableShielding, 1, true, out info);
            value = info.GetPinInfo(pinNum);
            return err;
        }

        #endregion

        #region Shielind Available

        public const string ParamNameShieldingAvailable = "ShieldingAvailable";

        public bool GetShieldingAvailable(ICyInstQuery_v1 query)
        {
            CyCompDevParam param = query.GetCommittedParam(ParamNameShieldingAvailable);
            if (param.ErrorCount == 0)
            {
                return param.GetValueAs<bool>();
            }
            return false;
        }

        #endregion
    }

    #endregion
}
