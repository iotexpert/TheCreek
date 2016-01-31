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

namespace Cypress.Components.System.Ports.cy_digital_port_v1_0
{
    public class CyCustomizer :
        ICyParamEditHook_v1,
        ICyVerilogCustomize_v1,
        ICyShapeCustomize_v1,
        ICyInstValidateHook_v1,
        ICyAPICustomize_v1
    {
        const string m_oeBaseTerminalName = "oe";
        const string m_oBaseTerminalName = "o";
        const string m_ioBaseTerminalName = "io";
        const string m_iBaseTerminalName = "i";
        const string m_irqBaseTerminalName = "irq";

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
            CyDigitalParamInfo info = new CyDigitalParamInfo();

            CyPortPinEditingControl pinsDGV = new CyPortPinEditingControl(edit);
            pinsDGV.Dock = DockStyle.Fill;
            ICyPortsPinColumnInfo[] columns = new ICyPortsPinColumnInfo[] {
                new CyPinAliasColumnInfo(edit),
                new CyPinModeColumnInfo(edit),
                new CySlewRateColumnInfo(edit),
                new CyInterruptModeColumnInfo(edit),
            };
            pinsDGV.SetupColumns(columns, info.GetWidth(edit));

            CyParamExprDelegate paramExprCommitted = delegate(ICyParamEditor custEditor, CyCompDevParam param)
            {
                //only update for valid data
                if (param.ErrorCount == 0)
                {
                    if (param.Name == CyDigitalParamInfo.ParamNameUseInterrupt ||
                             param.Name == CyDigitalParamInfo.ParamNameWidth ||
                             param.Name == CyDigitalParamInfo.ParamNameDirection)
                    {
                        byte width = info.GetWidth(edit);
                        pinsDGV.UpdatePins(width);
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
             *   .access_mode("SW_ONLY"),
             *   .layout_mode("CONTIGUOUS"),
             *   .port_mode("INPUT"),
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
            CyDigitalParamInfo info = new CyDigitalParamInfo();

            string termName_OE = termQuery.GetTermName(m_oeBaseTerminalName);
            string termName_I = termQuery.GetTermName(m_iBaseTerminalName);
            string termName_O = termQuery.GetTermName(m_oBaseTerminalName);
            string termName_IO = termQuery.GetTermName(m_ioBaseTerminalName);
            string termName_IRQ = termQuery.GetTermName(m_irqBaseTerminalName);

            string sigOEName = (termQuery.GetHasNoDrivers(termName_OE)) ?
               termQuery.GetTermDefaultVlogExpr(termName_OE) :
               termQuery.GetTermSigSegName(termName_OE);

            string sigOName = (termQuery.GetHasNoDrivers(termName_O)) ?
               termQuery.GetTermDefaultVlogExpr(termName_O) :
               termQuery.GetTermSigSegName(termName_O);

            //outputs and inouts don't need to be hooked up (they can be left open)----------
            string sigIName = (termQuery.GetHasNoDrivers(termName_I)) ?
               string.Empty : termQuery.GetTermSigSegName(termName_I);

            string sigIOName = (termQuery.GetHasNoDrivers(termName_IO)) ?
               string.Empty : termQuery.GetTermSigSegName(termName_IO);

            string sigIRQName = (termQuery.GetHasNoDrivers(termName_IRQ)) ?
               string.Empty : termQuery.GetTermSigSegName(termName_IRQ);
            //--------------------------------------------------------------------------------

            byte width = info.GetWidth(query);

            CyVerilogWriter vw = new CyVerilogWriter("cy_psoc3_port_v1_0", info.GetShortInstanceName(query));
            vw.AddGeneric("width", width.ToString());
            vw.AddGeneric("siorefwidth", ((width + 1) / 2).ToString());
            vw.AddGeneric("id", "\"" + query.InstanceIdPath + "\"");
            vw.AddGeneric("pin_aliases", info.GetPinAliasVerilog(query, width));
            vw.AddGeneric("access_mode", string.Format("\"{0}\"", info.GetVerilogAccessMode(query)));
            vw.AddGeneric("layout_mode", string.Format("\"{0}\"", info.GetVerilogLayoutMode(query)));
            vw.AddGeneric("port_mode", string.Format("\"{0}\"", info.GetVerilogDirection(query)));
            vw.AddGeneric("slew_rate", info.GetVerilogSlewRateBitArray(query, width));
            vw.AddGeneric("vtrip", info.GetVerilogStandardLogic(query));
            vw.AddGeneric("intr_mode", info.GetVerilogInterruptModeBitArray(query, width));
            vw.AddGeneric("drive_mode", info.GetVerilogPinModeBitArray(query, width));
            vw.AddGeneric("por_state", info.GetVerilogPORState(query));

            // POST PR4 NOTE:
            //     Prior to PR4, the OE signals are 'active low', meaning, a 0 will
            //     enable the output, a 1 would turn it off. CDT 32449 is tracking the
            //     PR4 change to the device to make it 'active high', as it should have
            //     been.
            //vw.AddPort("oe", sigOEName);
            string netOEName = "tmpOE__" + info.GetShortInstanceName(query) + "_net";
            vw.AddWire(netOEName, 0, 0);
            vw.AssignPreES3CondWire(netOEName, "~" + sigOEName, sigOEName);
            vw.AddPort("oe", netOEName); //input

            vw.AddPort("y", sigOName);  //y - INPUT

            if (!string.IsNullOrEmpty(sigIName)) vw.AddPort("fb", sigIName); //fb - OUTPUT
            if (!string.IsNullOrEmpty(sigIOName)) vw.AddPort("io", sigIOName); //io - InOut
            if (!string.IsNullOrEmpty(sigIRQName)) vw.AddPort("interrupt", sigIRQName); //ouput

            codeSnippet = vw.ToString();
            return CyCustErr.Ok;
        }

        #endregion

        //-----------------------------

        #region ICyShapeCustomize_v1 Members

        public CyCustErr CustomizeShapes(
            ICyInstQuery_v1 query, ICySymbolShapeEdit_v1 shapeEdit, ICyTerminalEdit_v1 termEdit)
        {
            CyDigitalParamInfo info = new CyDigitalParamInfo();
            CyCustErr err;

            Debug.Assert(termEdit.TerminalCount > 0);

            string termName_O = termEdit.GetTermName(m_oBaseTerminalName);
            string termName_I = termEdit.GetTermName(m_iBaseTerminalName);
            string termName_IO = termEdit.GetTermName(m_ioBaseTerminalName);
            string termName_OE = termEdit.GetTermName(m_oeBaseTerminalName);
            string termName_IRQ = termEdit.GetTermName(m_irqBaseTerminalName);

            shapeEdit.SetFillColor("body", query.Preferences.SchematicDigitalTerminalColor);

            string oDefV;
            string oeDefV;
            CyDigitalParamInfo.CyPinDirection dir = info.GetDirection(query);
            byte width = info.GetWidth(query);

            oDefV = string.Format("{0}'b0", width.ToString());

            if (dir == CyDigitalParamInfo.CyPinDirection.Input)
                oeDefV = "1'b0";
            else
                oeDefV = "1'b1";

            err = termEdit.ChangeTerminalDefVerilogValue(termName_O, oDefV);
            if (err.IsNotOK) return err;
            err = termEdit.ChangeTerminalDefVerilogValue(termName_OE, oeDefV);
            if (err.IsNotOK) return err;

            int maxBitIndex = width - 1;
            string newName = (maxBitIndex == 0) ? m_oBaseTerminalName :
                              string.Format("{0}[{1}:0]", m_oBaseTerminalName, maxBitIndex.ToString());
            err = termEdit.TerminalRename(termName_O, newName);
            if (err.IsNotOK) return err;

            newName = (maxBitIndex == 0) ? m_iBaseTerminalName :
                       string.Format("{0}[{1}:0]", m_iBaseTerminalName, maxBitIndex.ToString());
            err = termEdit.TerminalRename(termName_I, newName);
            if (err.IsNotOK) return err;

            newName = (maxBitIndex == 0) ? m_ioBaseTerminalName :
                       string.Format("{0}[{1}:0]", m_ioBaseTerminalName, maxBitIndex.ToString());
            err = termEdit.TerminalRename(termName_IO, newName);
            if (err.IsNotOK) return err;

            return CyCustErr.OK;
        }

        //static CyCustErr SetTerminalType(ICyTerminalEdit_v1 termEdit, CyCompDevTermType type, 
        //    params string[] terminalNames)
        //{
        //    CyCustErr err;
        //    foreach (string termName in terminalNames)
        //    {
        //        err = termEdit.ChangeTerminalType(termName, type);
        //        if (err.IsNotOK) return err;
        //    }
        //    return CyCustErr.Ok;
        //}

        #endregion

        //-----------------------------

        #region ICyInstValidateHook_v1 Members

        public void ValidateParams(ICyInstValidate_v1 instVal)
        {
            CyDigitalParamInfo info = new CyDigitalParamInfo();

            CyPortParamInfoWithPinModeParams.ValidatePinModes(info, instVal);
            CyPortParamInfo.ValidatePinAliases(info, instVal);
            CyDigitalParamInfo.ValidatePinInterruptModes(info, instVal);
            CyDigitalParamInfo.ValidateSlewRates(info, instVal);
        }

        #endregion

        //-----------------------------

        #region ICyAPICustomize_v1 Members

        public IEnumerable<CyAPICustomizer> CustomizeAPIs(ICyInstQuery_v1 query, ICyTerminalQuery_v1 termQuery, IEnumerable<CyAPICustomizer> apis)
        {
            CyDigitalParamInfo paramNames = new CyDigitalParamInfo();
            CyDigitalParamInfo.CyAccessMode accessMode = paramNames.GetAccessMode(query);
            CyDigitalParamInfo.CyPinDirection direction = paramNames.GetDirection(query);
            List<CyAPICustomizer> apiCustomizers = new List<CyAPICustomizer>(apis);

            string instanceName = paramNames.GetShortInstanceName(query);
            byte width = paramNames.GetWidth(query);

            #region API Gen Specific Params and Param Names

            string pinRegistersParamName = "PinConfigurationRegisters_API_GEN";
            string pinRegisters = "/* Pin Registers */" + Environment.NewLine;
            for (byte count = 0; count < width; count++)
            {
                pinRegisters += string.Format(
                    "#define {1}_PIN_{0}                       (* (reg8 *) {1}__{0}__PC)\n"
                    , count, instanceName);
            }

            string pinDriveModeMacrosParamName = "DriveModeMacros_API_GEN";
            string pinDriveModeMacros = "/* Drive Mode Macros */" + Environment.NewLine;
            for (byte count = 0; count < width; count++)
            {
                pinDriveModeMacros += string.Format(
                    "#define {1}_{0}_ReadDM()         (({1}_PIN_{0} & {1}_PC_DM_MASK) >> {1}_PC_DM_SHIFT)\n"
                    , count, instanceName);
            }
            
            #endregion
            
            Dictionary<string, string> paramDict = null;

            foreach (CyAPICustomizer file in apiCustomizers)
            {
                paramDict = file.MacroDictionary;
            }

            paramDict.Add(pinRegistersParamName, pinRegisters);
            paramDict.Add(pinDriveModeMacrosParamName, pinDriveModeMacros);

            // Remove any API Customizers if mode is not SW or SWHW
            if (!(accessMode == CyDigitalParamInfo.CyAccessMode.SW || accessMode == CyDigitalParamInfo.CyAccessMode.SWHW))
                apiCustomizers.Clear();

            // Provide ReadOnly for readonly ports

            // Add Aliases for Pins 

            // Update dictionaries and return customizers
            foreach (CyAPICustomizer file in apiCustomizers)
            {
                file.MacroDictionary = paramDict;
            }

            return apiCustomizers;

        }

        #endregion
    }

    #region Helper Classes

    internal class CyInterruptModeColumnInfo : CyPinColumnInfo
    {
        readonly CyDGVEnhancedTextBoxColumn m_column;

        public CyInterruptModeColumnInfo(ICyInstEdit_v1 edit)
            : base(edit, CyDigitalParamInfo.ParamNamePinInterruptModes, 1, CyDigitalParamInfo.DefaultInterruptMode)
        {
            m_column = new CyDGVEnhancedTextBoxColumn();
            m_column.HeaderText = "Interrupt Mode";
            m_column.ReadOnly = false;
            m_column.Name = "InterruptModeColumn";
            m_column.AutoSizeMode = DataGridViewAutoSizeColumnMode.Fill;
            m_column.SortMode = DataGridViewColumnSortMode.Automatic;
            m_column.MemberListDisplaying +=
                new EventHandler<CyMemberListEventArgs>(interruptModeCol_MemberListDisplaying);
        }

        void interruptModeCol_MemberListDisplaying(object sender, CyMemberListEventArgs e)
        {
            List<IntelliPromptMemberListItem> items = new List<IntelliPromptMemberListItem>();

            foreach (string enumName in EditObj.GetPossibleEnumValuesFromType(
                CyDigitalParamInfo.EnumTypeNameInterruptMode))
            {
                items.Add(new IntelliPromptMemberListItem(enumName,
                    (int)ActiproSoftware.Products.SyntaxEditor.IconResource.PublicEnumeration));
            }

            e.MemberListData = items.ToArray();
            e.Handled = true;
        }

        public override bool GetShouldBeReadonly(int pinNum)
        {
            CyDigitalParamInfo info = new CyDigitalParamInfo();
            bool useInterrupt;
            bool useInterruptValid = info.TryGetUseInterrupt(EditObj, out useInterrupt);
            return (!useInterruptValid || !useInterrupt);
        }

        public override object GetValueToDisplay(int pinNum)
        {
            bool readOnly = GetShouldBeReadonly(pinNum);
            if (readOnly)
            {
                return "-";
            }
            return base.GetValueToDisplay(pinNum);
        }

        public override DataGridViewColumn Column
        {
            get { return m_column; }
        }

        public override string ColumnName
        {
            get { return m_column.Name; }
        }
    }

    internal class CySlewRateColumnInfo : CyPinColumnInfo
    {
        readonly CyDGVEnhancedTextBoxColumn m_column;

        public CySlewRateColumnInfo(ICyInstEdit_v1 edit)
            : base(edit, CyDigitalParamInfo.ParamNameSlewRates, 1, CyDigitalParamInfo.DefaultSlewRate)
        {
            m_column = new CyDGVEnhancedTextBoxColumn();
            m_column.HeaderText = "Slew Rate";
            m_column.ReadOnly = false;
            m_column.Name = "SlewRateColumn";
            m_column.AutoSizeMode = DataGridViewAutoSizeColumnMode.AllCells;
            m_column.SortMode = DataGridViewColumnSortMode.Automatic;
            m_column.MemberListDisplaying +=
                new EventHandler<CyMemberListEventArgs>(slewRateCol_MemberListDisplaying);
        }

        void slewRateCol_MemberListDisplaying(object sender, CyMemberListEventArgs e)
        {
            List<IntelliPromptMemberListItem> items = new List<IntelliPromptMemberListItem>();

            foreach (string enumName in EditObj.GetPossibleEnumValuesFromType(
                CyDigitalParamInfo.EnumTypeNameSlewRate))
            {
                items.Add(new IntelliPromptMemberListItem(enumName,
                    (int)ActiproSoftware.Products.SyntaxEditor.IconResource.PublicEnumeration));
            }

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
    }

    internal class CyDigitalParamInfo : CyPortParamInfoWithPinModeParams
    {
        #region Access Mode Param

        //param name
        const string ParamNameAccessMode = "AccessMode";

        //PortAccessMode enum values
        const string EnumValuePortAccessMode_SW = "1";
        const string EnumValuePortAccessMode_HW = "2";
        const string EnumValuePortAccessMode_SWHW = "3";

        ////PortAccessMode enum names
        //const string EnumNamePortAccessMode_SW = "PortAccessMode_SW";
        //const string EnumNamePortAccessMode_HW = "PortAccessMode_HW";
        //const string EnumNamePortAccessMode_SWHW = "PortAccessMode_SWHW";

        public enum CyAccessMode
        {
            SW,
            HW,
            SWHW,

            Unknown,
        }

        public override string GetVerilogAccessMode(ICyInstQuery_v1 query)
        {
            CyAccessMode mode = GetAccessMode(query);

            switch (mode)
            {
                case CyAccessMode.SW:
                    return VerilogAccessMode_SW_ONLY;

                case CyAccessMode.HW:
                    return VerilogAccessMode_HW_ONLY;

                case CyAccessMode.SWHW:
                    return VerilogAccessMode_HW_SW;

                default:
                    Debug.Fail("unhandled access mode");
                    return string.Empty;
            }
        }

        public CyAccessMode GetAccessMode(ICyInstQuery_v1 query)
        {
            CyCompDevParam param = query.GetCommittedParam(ParamNameAccessMode);
            return GetAccessModeFromString(param.Value);
        }

        static CyAccessMode GetAccessModeFromString(string accessMode)
        {
            if (accessMode == EnumValuePortAccessMode_HW)
            {
                return CyAccessMode.HW;
            }
            if (accessMode == EnumValuePortAccessMode_SW)
            {
                return CyAccessMode.SW;
            }
            if (accessMode == EnumValuePortAccessMode_SWHW)
            {
                return CyAccessMode.SWHW;
            }

            Debug.Fail("unhandled access mode");
            return CyAccessMode.Unknown;
        }

        #endregion

        #region Direction Param

        internal const string ParamNameDirection = "Direction";

        //PortDirection enum values
        const string EnumValuePortDirection_Input = "1";
        const string EnumValuePortDirection_InOut = "2";
        const string EnumValuePortDirection_Output = "3";
        const string EnumValuePortDirection_Bidirectional = "4";

        ////PortDirection enum names
        //const string EnumNamePortDirection_Input = "PortDirection_Input";
        //const string EnumNamePortDirection_InOut = "PortDirection_InOut";
        //const string EnumNamePortDirection_Output = "PortDirection_Output";
        //const string EnumNamePortDirection_Bidirectional = "PortDirection_Bidirectional";

        public enum CyPinDirection
        {
            Input = 0,
            Output = 1,
            InOut = 2,
            Bidirectional = 3,

            Unknown,
        }

        public override string GetVerilogDirection(ICyInstQuery_v1 query)
        {
            CyPinDirection dir = GetDirection(query);

            switch (dir)
            {
                case CyPinDirection.Input:
                    return VerilogDirection_INPUT;

                case CyPinDirection.Output:
                    return VerilogDirection_OUTPUT;

                case CyPinDirection.InOut:
                    return VerilogDirection_INOUT;

                case CyPinDirection.Bidirectional:
                    return VerilogDirection_BIDIRECTIONAL;

                default:
                    Debug.Fail("unhandled direction");
                    return string.Empty;
            }
        }

        public CyPinDirection GetDirection(ICyInstQuery_v1 query)
        {
            CyCompDevParam param = query.GetCommittedParam(ParamNameDirection);
            return GetDirectionFromString(param.Value);
        }

        public CyPinDirection GetDirection(ICyInstValidate_v1 query)
        {
            CyCompDevParam param = query.GetCommittedParam(ParamNameDirection);
            return GetDirectionFromString(param.Value);
        }

        static CyPinDirection GetDirectionFromString(string dir)
        {
            if (dir == EnumValuePortDirection_InOut) return CyPinDirection.InOut;
            if (dir == EnumValuePortDirection_Input) return CyPinDirection.Input;
            if (dir == EnumValuePortDirection_Output) return CyPinDirection.Output;
            if (dir == EnumValuePortDirection_Bidirectional) return CyPinDirection.Bidirectional;
            return CyPinDirection.Unknown;
        }

        #endregion

        #region PinMode

        protected override string HiZVerilogValue
        {
            get { return VerilogPinMode_DigitalHiZ; }
        }

        #endregion

        #region Interrupt

        internal const string ParamNameUseInterrupt = "UseInterrupt";

        public const string ParamNamePinInterruptModes = "PinInterruptModes";
        public const string ParamNamePinInterruptModeValidator = "PinInterruptModeValidator";
        public const string EnumTypeNameInterruptMode = "PortInterruptMode";

        public const string DefaultInterruptMode = "PortInterruptMode_None";

        public bool TryGetUseInterrupt(ICyInstQuery_v1 query, out bool useInterrupt)
        {
            return TryGetBoolParamValue(query, ParamNameUseInterrupt, out useInterrupt);
        }

        protected override string VerilogInterruptMode(ICyInstQuery_v1 query, int pinNum)
        {
            bool useInterrupt;
            bool useInterruptValid = TryGetUseInterrupt(query, out useInterrupt);
            if (!useInterruptValid || !useInterrupt)
            {
                return VerilogInterruptMode_None;
            }

            string paramValue = GetPinInterruptModeValue(query, pinNum);

            if (paramValue == EnumValuePortInterruptMode_None) return VerilogInterruptMode_None;
            if (paramValue == EnumValuePortInterruptMode_RisingEdge) return VerilogInterruptMode_RisingEdge;
            if (paramValue == EnumValuePortInterruptMode_FallingEdge) return VerilogInterruptMode_FallingEdge;
            if (paramValue == EnumValuePortInterruptMode_OnChange) return VerilogInterruptMode_OnChange;

            Debug.Fail("unhandled interrupt mode");
            return string.Empty;
        }

        public string GetPinInterruptModeValue(ICyInstQuery_v1 query, int pinNum)
        {
            CyCompDevParam param = query.GetCommittedParam(ParamNamePinInterruptModes);
            CyPinInfo info;
            CyCustErr err = CyPinInfo.LoadInfo(param.Value, ParamNamePinInterruptModes, 1, true, out info);
            return info.GetPinInfo(pinNum);
        }

        public CyCustErr GetPinInterruptModeValue(ICyInstValidate_v1 query, int pinNum, out string value)
        {
            CyCompDevParam param = query.GetCommittedParam(ParamNamePinInterruptModes);
            CyPinInfo info;
            CyCustErr err = CyPinInfo.LoadInfo(param.Value, ParamNamePinInterruptModes, 1, true, out info);
            value = info.GetPinInfo(pinNum);
            return err;
        }

        internal static void ValidatePinInterruptModes(CyDigitalParamInfo info, ICyInstValidate_v1 instVal)
        {
            //validate pin Interrupt modes
            uint width = info.GetWidth(instVal);

            for (int i = 0; i < width; i++)
            {
                string mode;
                CyCustErr err = info.GetPinInterruptModeValue(instVal, i, out mode);
                int modeValue;

                if (err.IsNotOk)
                {
                    instVal.AddError(CyDigitalParamInfo.ParamNamePinInterruptModes,
                            new CyCustErr(string.Format("pin{0}: {1}", i, err.Message)));
                }
                else if (int.TryParse(mode, out modeValue))
                {
                    if (!(instVal.EnumValueWithinRange(
                        CyDigitalParamInfo.EnumTypeNameInterruptMode, modeValue)))
                    {
                        instVal.AddError(CyDigitalParamInfo.ParamNamePinInterruptModes,
                            new CyCustErr(string.Format("pin{0}: Value out of range for enumeration type '{1}'", i,
                                CyDigitalParamInfo.EnumTypeNameInterruptMode)));
                    }
                }
                else
                {
                    //the expression must have been replaced with an error msg
                    instVal.AddError(CyDigitalParamInfo.ParamNamePinInterruptModes,
                            new CyCustErr(string.Format("pin{0}: {1}", i, mode)));
                }
            }
        }

        #endregion

        #region Standard Logic (Vtrip)

        //param name
        const string ParamNameStandardLogic = "StandardLogic";

        //PortStandardLogic enum values
        const string EnumValuePortStandardLogic_CMOS = "0";
        const string EnumValuePortStandardLogic_LVTTL = "1";

        const string VerilogStandardLogic_CMOS = "0";
        const string VerilogStandardLogic_LVTTL = "1";

        public string GetVerilogStandardLogic(ICyInstQuery_v1 query)
        {
            CyCompDevParam param = query.GetCommittedParam(ParamNameStandardLogic);

            if (param.Value == EnumValuePortStandardLogic_CMOS) return VerilogStandardLogic_CMOS;
            if (param.Value == EnumValuePortStandardLogic_LVTTL) return VerilogStandardLogic_LVTTL;

            //unknow value
            return string.Empty;
        }

        #endregion

        #region Slew Rate

        public const string ParamNameSlewRates = "SlewRates";
        public const string EnumTypeNameSlewRate = "PortSlewRate";
        public const string DefaultSlewRate = "PortSlewRate_Fast";

        //PortSlewRate enum values
        const string EnumValuePortSlewRate_Fast = "0";
        const string EnumValuePortSlewRate_Slow = "1";

        const string VerilogSlewRate_Fast = "0";
        const string VerilogSlewRate_Slow = "1";

        internal static void ValidateSlewRates(CyDigitalParamInfo info, ICyInstValidate_v1 instVal)
        {
            //validate pin Interrupt modes
            uint width = info.GetWidth(instVal);

            for (int i = 0; i < width; i++)
            {
                string rate;
                CyCustErr err = info.GetSlewRateValue(instVal, i, out rate);
                int rateValue;
                if (err.IsNotOk)
                {
                    instVal.AddError(CyDigitalParamInfo.ParamNameSlewRates,
                            new CyCustErr(string.Format("pin{0}: {1}", i, err.Message)));
                }
                else if (int.TryParse(rate, out rateValue))
                {
                    if (!(instVal.EnumValueWithinRange(
                        CyDigitalParamInfo.EnumTypeNameSlewRate, rateValue)))
                    {
                        instVal.AddError(CyDigitalParamInfo.ParamNameSlewRates,
                            new CyCustErr(string.Format("pin{0}: Value out of range for enumeration type '{1}'", i,
                                CyDigitalParamInfo.EnumTypeNameSlewRate)));
                    }
                }
                else
                {
                    //the expression must have been replaced with an error msg
                    instVal.AddError(CyDigitalParamInfo.ParamNameSlewRates,
                            new CyCustErr(string.Format("pin{0}: {1}", i, rate)));
                }
            }
        }

        public string GetVerilogSlewRateBitArray(ICyInstQuery_v1 query, int width)
        {
            string bits = string.Empty;
            for (int i = 0; i < width; i++)
            {
                bits += VerilogSlewRate(query, i);
            }
            bits = bits.Trim();
            string slewRates = string.Format("{0}'b{1}", bits.Length, bits);
            return slewRates;
        }

        string VerilogSlewRate(ICyInstQuery_v1 query, int pinNum)
        {
            string paramValue = GetSlewRateValue(query, pinNum);

            if (paramValue == EnumValuePortSlewRate_Fast) return VerilogSlewRate_Fast;
            if (paramValue == EnumValuePortSlewRate_Slow) return VerilogSlewRate_Slow;

            //unknown value
            return string.Empty;
        }

        public string GetSlewRateValue(ICyInstQuery_v1 query, int pinNum)
        {
            CyCompDevParam param = query.GetCommittedParam(ParamNameSlewRates);
            CyPinInfo info;
            CyCustErr err = CyPinInfo.LoadInfo(param.Value, ParamNameSlewRates, 1, true, out info);
            return info.GetPinInfo(pinNum);
        }

        public CyCustErr GetSlewRateValue(ICyInstValidate_v1 query, int pinNum, out string value)
        {
            CyCompDevParam param = query.GetCommittedParam(ParamNameSlewRates);
            CyPinInfo info;
            CyCustErr err = CyPinInfo.LoadInfo(param.Value, ParamNameSlewRates, 1, true, out info);
            value = info.GetPinInfo(pinNum);
            return err;
        }

        #endregion
    }

    #endregion
}
