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

namespace Cypress.Components.System.Ports.cy_emif_port_v1_0
{
    public class CyCustomizer :
        ICyParamEditHook_v1,
        ICyVerilogCustomize_v1,
        ICyShapeCustomize_v1,
        ICyInstValidateHook_v1
    {
        const string m_oeBaseTerminalName = "oe";
        const string m_oBaseTerminalName = "o";

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
            CyEMIFParamInfo info = new CyEMIFParamInfo();

            CyPortPinEditingControl pinsDGV = new CyPortPinEditingControl(edit);
            pinsDGV.Dock = DockStyle.Fill;
            ICyPortsPinColumnInfo[] columns = new ICyPortsPinColumnInfo[] {
                new CyPinAliasColumnInfo(edit),
                new CyPinModeColumnInfo(edit),
            };
            pinsDGV.SetupColumns(columns, info.GetWidth(edit));

            CyParamExprDelegate paramExprCommitted = delegate(ICyParamEditor custEditor, CyCompDevParam param)
            {
                //only update for valid data
                if (param.ErrorCount == 0)
                {
                    //the number of pins is controled by the EMIFMode
                    if (param.Name == CyEMIFParamInfo.ParamNameEmifMode)
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
             *   .access_mode("EMIF"),
             *   .layout_mode("CONTIGUOUS"),
             *   .port_mode("OUTPUT"),
             *   .intr_mode(16'b00 00 00 00 00 00 00 00),
             *   .drive_mode(16'b00 00 00 00 00 00 00 00))
             * port_1
             *  (.oe(wire_1),
             *   .y(Terminal_1));
             */

            StringBuilder sb = new StringBuilder();
            CyEMIFParamInfo info = new CyEMIFParamInfo();

            string termName_OE = termQuery.GetTermName(m_oeBaseTerminalName);
            string termName_O = termQuery.GetTermName(m_oBaseTerminalName);

            string sigOEName = (termQuery.GetHasNoDrivers(termName_OE)) ?
               termQuery.GetTermDefaultVlogExpr(termName_OE) :
               termQuery.GetTermSigSegName(termName_OE);

            string sigOName = (termQuery.GetHasNoDrivers(termName_O)) ?
               termQuery.GetTermDefaultVlogExpr(termName_O) :
               termQuery.GetTermSigSegName(termName_O);

            byte width = info.GetWidth(query);

            CyVerilogWriter vw = new CyVerilogWriter("cy_psoc3_port_v1_0", info.GetShortInstanceName(query));
            vw.AddGeneric("width", width.ToString());
            vw.AddGeneric("siorefwidth", ((width+1)/2).ToString());
            vw.AddGeneric("id", "\"" + query.InstanceIdPath + "\"");
            vw.AddGeneric("pin_aliases", info.GetPinAliasVerilog(query, width));
            vw.AddGeneric("access_mode", string.Format("\"{0}\"", info.GetVerilogAccessMode(query)));
            vw.AddGeneric("layout_mode", string.Format("\"{0}\"", info.GetVerilogLayoutMode(query)));
            vw.AddGeneric("port_mode", string.Format("\"{0}\"", info.GetVerilogDirection(query)));
            vw.AddGeneric("intr_mode", info.GetVerilogInterruptModeBitArray(query, width));
            vw.AddGeneric("drive_mode", info.GetVerilogPinModeBitArray(query, width));
            vw.AddGeneric("por_state", info.GetVerilogPORState(query));
            vw.AddGeneric("emif_mode", string.Format("\"{0}\"", info.GetEmifMode(query)));

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

            codeSnippet = vw.ToString();
            return CyCustErr.Ok;
        }

        #endregion

        //-----------------------------

        #region ICyShapeCustomize_v1 Members

        public CyCustErr CustomizeShapes(
            ICyInstQuery_v1 query, ICySymbolShapeEdit_v1 shapeEdit, ICyTerminalEdit_v1 termEdit)
        {
            CyEMIFParamInfo info = new CyEMIFParamInfo();
            CyCustErr err;

            shapeEdit.SetFillColor("body", query.Preferences.SchematicDigitalTerminalColor);

            Debug.Assert(termEdit.TerminalCount > 0);

            string termName_O = termEdit.GetTermName(m_oBaseTerminalName);
            string termName_OE = termEdit.GetTermName(m_oeBaseTerminalName);

            string oDefV;
            string oeDefV;

            byte width = info.GetWidth(query);

            switch (info.GetEmifMode(query))
            {
                case CyEMIFParamInfo.CyEmifMode.MSB_ADDR:
                case CyEMIFParamInfo.CyEmifMode.MID_ADDR:
                case CyEMIFParamInfo.CyEmifMode.LSB_ADDR:
                    //no pins visible
                    oeDefV = "1'b0";
                    oDefV = string.Format("{0}'b0", width.ToString());
                    break;

                case CyEMIFParamInfo.CyEmifMode.MSB_DATA:
                case CyEMIFParamInfo.CyEmifMode.LSB_DATA:
                    //oe visible
                    oeDefV = string.Empty;
                    oDefV = string.Format("{0}'b0", width.ToString());
                    break;

                case CyEMIFParamInfo.CyEmifMode.CONTROL:
                    //o visible
                    oeDefV = "1'b0";
                    oDefV = string.Empty;
                    break;

                case CyEMIFParamInfo.CyEmifMode.Unknown:
                default:
                    Debug.Fail("unhandled emif mode");
                    oeDefV = string.Empty;
                    oDefV = string.Empty;
                    break;
            }

            err = termEdit.ChangeTerminalDefVerilogValue(termName_O, oDefV);
            if (err.IsNotOK) return err;
            err = termEdit.ChangeTerminalDefVerilogValue(termName_OE, oeDefV);
            if (err.IsNotOK) return err;

            int maxBitIndex = width - 1;
            string newName = (maxBitIndex == 0) ? m_oBaseTerminalName :
                              string.Format("{0}[{1}:0]", m_oBaseTerminalName, maxBitIndex.ToString());
            err = termEdit.TerminalRename(termName_O, newName);
            if (err.IsNotOK) return err;

            return CyCustErr.OK;
        }

        #endregion

        //-----------------------------

        #region ICyInstValidateHook_v1 Members

        public void ValidateParams(ICyInstValidate_v1 instVal)
        {
            //validate pin modes
            CyEMIFParamInfo info = new CyEMIFParamInfo();
            CyPortParamInfoWithPinModeParams.ValidatePinModes(info, instVal);

            //validate pin aliases
            CyPortParamInfo.ValidatePinAliases(info, instVal);
        }

        #endregion
    }

    #region Helper Classes

    internal class CyEMIFParamInfo : CyPortParamInfoWithPinModeParams
    {
        #region EMIF Mode Param

        public const string ParamNameEmifMode = "EmifMode";

        //PortEmifMode enum values
        const string EnumValuePortEmifMode_MSB_ADDR = "1";
        const string EnumValuePortEmifMode_MID_ADDR = "2";
        const string EnumValuePortEmifMode_LSB_ADDR = "3";
        const string EnumValuePortEmifMode_MSB_DATA = "4";
        const string EnumValuePortEmifMode_LSB_DATA = "5";
        const string EnumValuePortEmifMode_CONTROL = "6";

        ////PortEmifMode enum names
        //const string EnumNamePortEmifMode_MSB_ADDR = "PortEmifMode_MSB_ADDR";
        //const string EnumNamePortEmifMode_MID_ADDR = "PortEmifMode_MID_ADDR";
        //const string EnumNamePortEmifMode_LSB_ADDR = "PortEmifMode_LSB_ADDR";
        //const string EnumNamePortEmifMode_MSB_DATA = "PortEmifMode_MSB_DATA";
        //const string EnumNamePortEmifMode_LSB_DATA = "PortEmifMode_LSB_DATA";
        //const string EnumNamePortEmifMode_CONTROL = "PortEmifMode_CONTROL";

        public CyEmifMode GetEmifMode(ICyInstQuery_v1 query)
        {
            CyCompDevParam param = query.GetCommittedParam(ParamNameEmifMode);
            return GetEmifModeFromString(param.Value);
        }

        static CyEmifMode GetEmifModeFromString(string mode)
        {
            if (mode == EnumValuePortEmifMode_MSB_ADDR) return CyEmifMode.MSB_ADDR;
            if (mode == EnumValuePortEmifMode_MID_ADDR) return CyEmifMode.MID_ADDR;
            if (mode == EnumValuePortEmifMode_LSB_ADDR) return CyEmifMode.LSB_ADDR;
            if (mode == EnumValuePortEmifMode_MSB_DATA) return CyEmifMode.MSB_DATA;
            if (mode == EnumValuePortEmifMode_LSB_DATA) return CyEmifMode.LSB_DATA;
            if (mode == EnumValuePortEmifMode_CONTROL) return CyEmifMode.CONTROL;

            Debug.Fail("unhandled mode");
            return CyEmifMode.Unknown;
        }

        public enum CyEmifMode
        {
            MSB_ADDR,
            MID_ADDR,
            LSB_ADDR,
            MSB_DATA,
            LSB_DATA,
            CONTROL,

            Unknown,
        }

        #endregion

        #region Verilog Values

        public override string GetVerilogAccessMode(ICyInstQuery_v1 query)
        {
            return VerilogAccessMode_EMIF;
        }

        public override string GetVerilogDirection(ICyInstQuery_v1 query)
        {
            CyEmifMode mode = GetEmifMode(query);

            switch (mode)
            {
                case CyEmifMode.MSB_ADDR:
                case CyEmifMode.MID_ADDR:
                case CyEmifMode.LSB_ADDR:
                    return VerilogDirection_OUTPUT;

                case CyEmifMode.MSB_DATA:
                case CyEmifMode.LSB_DATA:
                    return VerilogDirection_INOUT;

                case CyEmifMode.CONTROL:
                    return VerilogDirection_OUTPUT;

                case CyEmifMode.Unknown:
                default:
                    Debug.Fail("unhandled emif mode");
                    return string.Empty;
            }
        }

        protected override string VerilogInterruptMode(ICyInstQuery_v1 query, int pinNum)
        {
            return VerilogInterruptMode_None;
        }

        protected override string HiZVerilogValue
        {
            get { return VerilogPinMode_DigitalHiZ; }
        }

        #endregion
    }

    #endregion
}
