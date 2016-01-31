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

namespace Cypress.Components.System.Ports.cy_lcd_port_v1_50
{
    public class CyCustomizer :
        ICyParamEditHook_v1,
        ICyVerilogCustomize_v1,
        ICyShapeCustomize_v1,
        ICyInstValidateHook_v1
    {
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
            CyLCDParamInfo info = new CyLCDParamInfo();

            CyPortPinEditingControl pinsDGV = new CyPortPinEditingControl(edit);
            pinsDGV.Dock = DockStyle.Fill;
            ICyPortsPinColumnInfo[] columns = new ICyPortsPinColumnInfo[] {
                new CyPinAliasColumnInfo(edit),
            };
            pinsDGV.SetupColumns(columns, info.GetWidth(edit));

            CyParamExprDelegate paramExprCommitted = delegate(ICyParamEditor custEditor, CyCompDevParam param)
            {
                //only update for valid data
                if (param.ErrorCount == 0)
                {
                    if (param.Name == CyLCDParamInfo.ParamNameWidth) //need to update num pins dispayed
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
             *   .port_mode("OUTPUT"),
             *   .intr_mode(16'b00 00 00 00 00 00 00 00),
             *   .drive_mode(16'b001 001 001 001 001 001 001 001))
             * port_1
             *  (.oe(wire_1),
             *   .y(Terminal_1),
             *   .fb(foo),
             *   .analog(8'b0),
             *   .interrupt(wire_2));
             */

            StringBuilder sb = new StringBuilder();
            CyLCDParamInfo info = new CyLCDParamInfo();
            byte width = info.GetWidth(query);
            string widthString = width.ToString();

            CyVerilogWriter vw = new CyVerilogWriter("cy_psoc3_port_v1_0", info.GetShortInstanceName(query));
            vw.AddGeneric("width", widthString);
            vw.AddGeneric("siorefwidth", ((width+1)/2).ToString());
            vw.AddGeneric("id", "\"" + query.InstanceIdPath + "\"");
            vw.AddGeneric("pin_aliases", info.GetPinAliasVerilog(query, width));
            vw.AddGeneric("access_mode", string.Format("\"{0}\"", info.GetVerilogAccessMode(query)));
            vw.AddGeneric("layout_mode", string.Format("\"{0}\"", info.GetVerilogLayoutMode(query)));
            vw.AddGeneric("port_mode", string.Format("\"{0}\"", info.GetVerilogDirection(query)));
            vw.AddGeneric("intr_mode", info.GetVerilogInterruptModeBitArray(query, width));
            vw.AddGeneric("drive_mode", info.GetVerilogPinModeBitArray(query, width));
            vw.AddGeneric("lcd_com_seg", info.GetVerilogLCDDriveBitArray(query));
            vw.AddGeneric("lcd_sw_drive", info.GetVerilogSWControl(query));
            vw.AddGeneric("por_state", info.GetVerilogPORState(query));
            vw.AddGeneric("port_alias_required", info.GetVerilogPortAliasRequired(query));
            vw.AddGeneric("port_alias_group", "\"" + info.GetVerilogPortAliasGroup(query) + "\"");

            // POST PR4 NOTE:
            //     Prior to PR4, the OE signals are 'active low', meaning, a 0 will
            //     enable the output, a 1 would turn it off. CDT 32449 is tracking the
            //     PR4 change to the device to make it 'active high', as it should have
            //     been.
            //vw.AddPort("oe", "1'b0");
            string netOEName = "tmpOE__" + info.GetShortInstanceName(query) + "_net";
            vw.AddWire(netOEName, 0, 0);
            vw.AssignPreES3CondWire(netOEName, "1'b1", "1'b0");
            vw.AddPort("oe", netOEName); //input

            vw.AddPort("y", string.Format("{0}'b0", widthString));  //y - INPUT

            codeSnippet = vw.ToString();
            return CyCustErr.Ok;
        }

        #endregion

        //-----------------------------

        #region ICyShapeCustomize_v1 Members

        public CyCustErr CustomizeShapes(
            ICyInstQuery_v1 query, ICySymbolShapeEdit_v1 shapeEdit, ICyTerminalEdit_v1 termEdit)
        {
            shapeEdit.SetFillColor("body", query.Preferences.SchematicDigitalTerminalColor);
            return CyCustErr.OK;
        }

        #endregion

        //-----------------------------

        #region ICyInstValidateHook_v1 Members

        public void ValidateParams(ICyInstValidate_v1 instVal)
        {
            //validate pin aliases
            CyLCDParamInfo info = new CyLCDParamInfo();
            CyLCDParamInfo.ValidatePinAliases(info, instVal);
        }

        #endregion
    }

    internal class CyLCDParamInfo : CyPortParamInfo
    {
        #region LCD Drive Mode

        //LCDDrive enum values
        public const string EnumValuePortLCDDrive_Common = "0";
        public const string EnumValuePortLCDDrive_Segment = "1";

        //LCDDrive enum names
        //public const string EnumNamePortLCDDrive_Common = "PortLCDDrive_Common";
        //public const string EnumNamePortLCDDrive_Segment = "PortLCDDrive_Segment";

        //LCDDrive param names
        const string ParamNameLCDDrive = "LCDDrive";

        public string GetVerilogLCDDriveBitArray(ICyInstQuery_v1 query)
        {
            string bits = string.Empty;
            for (int i = 0; i < GetWidth(query); i++)
            {
                bits += VerilogLCDDriveMode(query);
            }
            bits = bits.Trim();
            string lcd_com_seg = string.Format("{0}'b{1}", bits.Length, bits);
            return lcd_com_seg;
        }

        string VerilogLCDDriveMode(ICyInstQuery_v1 query)
        {
            CyCompDevParam param = query.GetCommittedParam(ParamNameLCDDrive);
            string paramValue = param.Value;

            if (paramValue == EnumValuePortLCDDrive_Common)
            {
                return "0";
            }
            if (paramValue == EnumValuePortLCDDrive_Segment)
            {
                return "1";
            }

            Debug.Fail("unhandled LCD drive");
            return string.Empty;
        }

        #endregion

        #region SW Control Mode

        //SWControl param names
        const string ParamNameSWControl = "SWControl";

        public string GetVerilogSWControl(ICyInstQuery_v1 query)
        {
            CyCompDevParam param = query.GetCommittedParam(ParamNameSWControl);
            bool swControl = bool.Parse(param.Value);

            if (swControl)
                return "1";
            else
                return "0";
        }

        #endregion

        #region Port Alias Required

        //PortAliasRequired param names
        const string ParamNamePortAliasRequired = "PortAliasRequired";

        public string GetVerilogPortAliasRequired(ICyInstQuery_v1 query)
        {
            CyCompDevParam param = query.GetCommittedParam(ParamNamePortAliasRequired);
            bool portAliasRequired = bool.Parse(param.Value);

            if (portAliasRequired)
                return "1";
            else
                return "0";
        }

        #endregion

        #region Port Alias Group

        //PortAliasGroup param names
        const string ParamNamePortAliasGroup = "PortAliasGroup";

        public string GetVerilogPortAliasGroup(ICyInstQuery_v1 query)
        {
            CyCompDevParam param = query.GetCommittedParam(ParamNamePortAliasGroup);

            return param.Value;
        }

        #endregion

        #region Verilog Values

        public override string GetVerilogAccessMode(ICyInstQuery_v1 query)
        {
            return VerilogAccessMode_LCD;
        }

        public override string GetVerilogDirection(ICyInstQuery_v1 query)
        {
            return VerilogDirection_OUTPUT;
        }

        protected override string VerilogInterruptMode(ICyInstQuery_v1 query, int pinNum)
        {
            return VerilogInterruptMode_None;
        }

        protected override string VerilogPinMode(ICyInstQuery_v1 query, int pinNum)
        {
            CyCompDevParam param = query.GetCommittedParam(ParamNameSWControl);
            bool swControl = bool.Parse(param.Value);

            if (swControl)
                return VerilogPinMode_CMOSOut;
            else
                return VerilogPinMode_AnalogHiZ;
        }

        #endregion
    }
}
