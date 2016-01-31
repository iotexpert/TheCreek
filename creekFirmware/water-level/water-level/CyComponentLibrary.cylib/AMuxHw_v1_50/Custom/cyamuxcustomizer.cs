/*******************************************************************************
* Copyright 2008-2010, Cypress Semiconductor Corporation.  All rights reserved.
* You may use this file only in accordance with the license, terms, conditions, 
* disclaimers, and limitations in the end user license agreement accompanying 
* the software package with which this file was provided.
********************************************************************************/



using System;
using System.Collections.Generic;
using System.Text;
using System.Windows.Forms;
using CyDesigner.Extensions.Common;
using System.Diagnostics;
using System.Drawing;
using CyDesigner.Extensions.Gde;

namespace CyCustomizer.AMuxHw_v1_50
{
    [CyCompDevCustomizer()]
    public class CyCustomizer :
        ICyShapeCustomize_v1,
        ICyVerilogCustomize_v1
    {
        private const string SYMBOL_PARAM_CHANNEL = "Channels";
        private const string SYMBOL_PARAM_TYPE = "MuxType";
        private const string SYMBOL_PARAM_MODE = "Mode";
        private const string SYMBOL_PARAM_ENABLE = "ShowEnable";

        private const string OUTPUT_A_TERM_BASE_NAME = "AoutTerm";
        private const string OUTPUT_B_TERM_BASE_NAME = "BoutTerm";
        private const string GENERATED_SHAPE = "SymbolShape";
        private const string INPUT_A_TERM_BASE_NAME = "AinTerm";
        private const string INPUT_B_TERM_BASE_NAME = "BinTerm";
        private const string INPUT_SEL_BUS_BASE_NAME = "selectorBus";
        private const string INPUT_CLOCK_NAME = "clock";
        private const string INPUT_ENABLE_NAME = "enable";

        private const uint SINGLE_MUX = 1;
        private const uint DIFFERENTIAL_MUX = 2;
        private const uint MUX_MODE = 1;
        private const uint SWITCH_MODE = 2;

        private const string AMUX_PRIM_NAME = "cy_psoc3_amux_v1_0";

        private const string DECODER_OUTPUT = "one_hot";
        private const string DECODER_REG_OLD_ID = "old_id";
        private const string DECODER_WIRE_ACTIVE = "is_active";

        private Color AMUX_COLOR = Color.Gainsboro;

        #region ICyShapeCustomize_v1 Members

        public CyCustErr CustomizeShapes(
            ICyInstQuery_v1 instQuery,
            ICySymbolShapeEdit_v1 shapeEdit,
            ICyTerminalEdit_v1 termEdit)
        {
            // We leave the symbol as it is for symbol preview
            //if (instQuery.IsPreviewCanvas)
            //    return CyCustErr.OK;

            shapeEdit.ShapesRemove("dummyTerminal");
            shapeEdit.ShapesRemove("symbolName");
            shapeEdit.ShapesRemove("preview_shape");
            termEdit.RemoveAllTerminals();

            // Add term shapes
            List<string> shapeTags = new List<string>();
            shapeTags.Add(GENERATED_SHAPE);

            // Read Parameters
            uint numTerminals, channelWidth, mode;
            bool showEnable;
            CyCustErr err = ReadParamter(instQuery, out numTerminals, out channelWidth, out mode, out showEnable);
            if (err.IsNotOk) return err;

            float pinSpace = (channelWidth == SINGLE_MUX) ? 12f : 18f;
            float pinOffset = ((numTerminals%2) == 0)
                                  ? (((numTerminals/2f)*-(pinSpace)) + 6f)
                                  : (((numTerminals + 1)/2f)*-(pinSpace) + 12f);

            // Place Input terminals
            PlaceInputTerminals(termEdit, channelWidth, numTerminals, pinSpace, pinOffset);

            // Place Output Terminal/s
            PlaceOutputTerminals(termEdit, channelWidth);

            // Place symbol shape
            float topY, bottomY;
            if (numTerminals == 1)
            {
                topY = pinOffset - 18f;
                bottomY = pinOffset + 24f;
            }
            else if (numTerminals == 2)
            {
                topY = pinOffset - 12f;
                bottomY = ((numTerminals - 1)*pinSpace) + pinOffset + 18f;
            }
            else
            {
                topY = pinOffset - 6f;
                bottomY = ((numTerminals - 1)*pinSpace) + pinOffset + 12f;
            }

            PlaceSymbolShape(shapeEdit, channelWidth, topY, bottomY, shapeTags);

            // Selector / clock / enable handling
            float shapeBottomLeftY = (channelWidth == SINGLE_MUX) ? bottomY : (bottomY + 6f);

            // Handle Selector terminals, its label and its extension line.
            HandleSelectorTerminals(shapeEdit, termEdit, mode, numTerminals, shapeBottomLeftY);

            // ============== Mux mode only ===================
            if (mode == MUX_MODE)
            {
                HandleClockEnableTerminal(shapeEdit, termEdit,
                                          channelWidth, showEnable, topY, bottomY, shapeBottomLeftY);
            }

            // Move Instance Name / Min Channel number / Max Channel number / 
            MoveLabels(shapeEdit, mode, showEnable, channelWidth, numTerminals, topY, pinSpace);

            // Move the Insider
            MoveSymbolInsider(shapeEdit, channelWidth, mode);

            return CyCustErr.OK;
        }

        private void PlaceInputTerminals(ICyTerminalEdit_v1 termEdit, uint channelWidth,
                                         uint numTerminals, float pinSpace, float pinOffset)
        {
            for (int index = 0; index < numTerminals; index++)
            {
                float pinY = (index*pinSpace) + pinOffset;

                AddInputTerminal(termEdit, INPUT_A_TERM_BASE_NAME, index, pinY);

                if (channelWidth == DIFFERENTIAL_MUX)
                {
                    AddInputTerminal(termEdit, INPUT_B_TERM_BASE_NAME, index, (pinY + 6f));
                }
            }
        }

        private void AddInputTerminal(ICyTerminalEdit_v1 termEdit, string baseName, int index, float Ylocation)
        {
            string termName = GetTermName(baseName, index, true);
            termEdit.TerminalAdd(termName, CyCompDevTermDir.INOUT, CyCompDevTermType.ANALOG,
                                 new PointF(0f, Ylocation), string.Empty, 0f, false);
        }

        private void PlaceOutputTerminals(ICyTerminalEdit_v1 termEdit, uint channelWidth)
        {
            if (channelWidth == SINGLE_MUX) // Single ended Mux
            {
                AddOutputTerminal(termEdit, OUTPUT_A_TERM_BASE_NAME, 0f);
            }
            else // Differential Mux
            {
                AddOutputTerminal(termEdit, OUTPUT_A_TERM_BASE_NAME, -6f);
                AddOutputTerminal(termEdit, OUTPUT_B_TERM_BASE_NAME, 6f);
            }
        }

        private void AddOutputTerminal(ICyTerminalEdit_v1 termEdit, string baseName, float YLocation)
        {
            termEdit.TerminalAdd(baseName, CyCompDevTermDir.INOUT, CyCompDevTermType.ANALOG,
                                 new PointF(36f, YLocation), string.Empty, 180f, false);
        }

        private void PlaceSymbolShape(ICySymbolShapeEdit_v1 shapeEdit, uint channelWidth,
                                      float topY, float bottomY, List<string> shapeTags)
        {
            float bottomLeftY, bottomRightY;
            if (channelWidth == SINGLE_MUX)
            {
                bottomLeftY = bottomY;
                bottomRightY = bottomY - 12f;
            }
            else
            {
                bottomLeftY = bottomY + 6f;
                bottomRightY = bottomY - 6f;
            }

            PointF topLeft = new PointF(6f, topY - 6f);
            PointF bottomLeft = new PointF(6f, bottomLeftY);
            PointF topRight = new PointF(30f, topY + 6f);
            PointF bottomRight = new PointF(30f, bottomRightY);

            shapeEdit.CreatePolyline(shapeTags, topLeft, topRight, bottomRight, bottomLeft, topLeft);
            shapeEdit.ShapesConvertToClosed(shapeTags, shapeTags);
            shapeEdit.SetFillColor(shapeTags[shapeTags.Count - 1], AMUX_COLOR);
            shapeEdit.SetOutlineWidth(shapeTags[shapeTags.Count - 1], 1F);
            shapeEdit.SendToBack(shapeTags[shapeTags.Count - 1]);
        }

        private void HandleSelectorTerminals(ICySymbolShapeEdit_v1 shapeEdit, ICyTerminalEdit_v1 termEdit,
                                             uint mode, uint numTerminals, float shapeBottomLeftY)
        {
            // (1) Place selector terminal
            uint numOfSelBits =
                (mode == MUX_MODE) ? (GetNumSelectorBits(numTerminals)) : numTerminals;
            string termNameSel = INPUT_SEL_BUS_BASE_NAME + GetTermSuffix(numOfSelBits);
            termEdit.TerminalAdd(termNameSel, CyCompDevTermDir.INPUT, CyCompDevTermType.DIGITAL,
                                 new PointF(24f, (shapeBottomLeftY + 12f)), string.Empty, -90f, false);

            // (2) Place extension selector line
            List<string> extendLines = new List<string>();
            extendLines.Add("extendLines");
            shapeEdit.CreateLine(extendLines,
                                 new PointF(24f, (shapeBottomLeftY - 9f)), new PointF(24f, (shapeBottomLeftY + 6f)));

            // (3) Remove dummy bus width slash (dummy_bus_slash)
            shapeEdit.ShapesRemove("dummy_bus_slash");

            // (4) Create new bus width slash
            List<string> selBusSlash = new List<string>();
            selBusSlash.Add("selector_bus_slash");
            shapeEdit.CreatePolyline(selBusSlash,
                                     new PointF(21f, shapeBottomLeftY + 3f),
                                     new PointF(27f, shapeBottomLeftY - 3f));
            shapeEdit.SetOutlineWidth(selBusSlash[0], 0f);

            // (5) Move bus selector annotation (selectorExpr)
            shapeEdit.ShapesMoveTo("selectorExpr", new PointF(27f, shapeBottomLeftY + 9f));
        }

        private void HandleClockEnableTerminal(ICySymbolShapeEdit_v1 shapeEdit, ICyTerminalEdit_v1 termEdit,
                                               uint channelWidth, bool showEnable, float topY, float bottomY,
                                               float shapeBottomLeftY)
        {
            // (1) Place clock terminal
            float clockTermY = (channelWidth == SINGLE_MUX) ? (bottomY + 12f) : (bottomY + 18f);
            termEdit.TerminalAdd(INPUT_CLOCK_NAME, CyCompDevTermDir.INPUT, CyCompDevTermType.DIGITAL,
                                 new PointF(12f, clockTermY), string.Empty, -90f, false);

            // (2) Place extension line for clock
            List<string> extendLines = new List<string>();
            extendLines.Add("extendLines");
            shapeEdit.CreateLine(extendLines, new PointF(12f, (shapeBottomLeftY - 3f)),
                                 new PointF(12f, (shapeBottomLeftY + 6f)));

            // (3) Move clock annotation (clockLabel)
            RectangleF clockLabelBounds = shapeEdit.GetShapeBounds("clockLabel");
            shapeEdit.ShapesMoveTo("clockLabel", new PointF(0f, (shapeBottomLeftY + clockLabelBounds.Height)));

            // By default, we hide the "enable" terminal and set "enable" to true. 
            // We only show this terminal when the user when to control the "enable" signal.
            if (showEnable)
            {
                // (4) Place enable terminal
                termEdit.TerminalAdd(INPUT_ENABLE_NAME, CyCompDevTermDir.INPUT, CyCompDevTermType.DIGITAL,
                                     new PointF(18f, topY - 18f), string.Empty, 90f, false);

                // (5) Place extension line for enable
                shapeEdit.CreateLine(extendLines, new PointF(18f, topY), new PointF(18f, (topY - 12f)));

                // (6) Move enable annotation (enableLabel)
                shapeEdit.ShapesMoveTo("enableLabel", new PointF(12f, (topY - 30f)));
            }
        }

        private void MoveLabels(ICySymbolShapeEdit_v1 shapeEdit, uint mode, bool showEnable, uint channelWidth,
                                uint numTerminals, float topY, float pinSpace)
        {
            // This is when the top terminal will shown, so we need to move the instance name further away.
            if (mode == MUX_MODE && showEnable)
            {
                shapeEdit.ShapesMoveTo("theInstanceName", new PointF(6f, (topY - 42f)));
            }
            else
            {
                shapeEdit.ShapesMoveTo("theInstanceName", new PointF(6f, (topY - 18f)));
            }

            shapeEdit.ShapesMoveTo("theMinChannel", new PointF(9f, ((numTerminals)*-(pinSpace/2)) + 2f));
            shapeEdit.BringToFront("theMinChannel");

            float maxAdjust = (channelWidth == SINGLE_MUX) ? 11f : 17f;
            shapeEdit.ShapesMoveTo("theMaxChannel", new PointF(9f, ((numTerminals)*(pinSpace/2)) - maxAdjust));
            // One terminal only needs 1 label.
            if (numTerminals == 1)
            {
                shapeEdit.ShapesRemove("theMaxChannel");
            }
            else
            {
                shapeEdit.BringToFront("theMaxChannel");
            }
        }

        private void MoveSymbolInsider(ICySymbolShapeEdit_v1 shapeEdit, uint channelWidth, uint mode)
        {
            PointF shapeCenter = GetRectCenter(shapeEdit.GetShapeBounds(GENERATED_SHAPE));
            if (channelWidth == SINGLE_MUX)
            {
                PointF DestPoint = new PointF((shapeCenter.X + 6f), shapeCenter.Y);

                if (mode == MUX_MODE)
                {
                    MoveInsider(shapeEdit, "centerInsiderMux", DestPoint, "singleMuxInsider");
                }
                else
                {
                    MoveInsider(shapeEdit, "centerInsiderSwitch", DestPoint, "singleSwitchInsider");
                }
            }
            else
            {
                if (mode == MUX_MODE)
                {
                    PointF DestPoint = new PointF((shapeCenter.X + 6f), (shapeCenter.Y - 6f));
                    MoveInsider(shapeEdit, "upInsiderMux", DestPoint, "diffMuxInsider");
                }
                else
                {
                    PointF DestPoint = new PointF((shapeCenter.X + 5f), (shapeCenter.Y - 5f));
                    MoveInsider(shapeEdit, "upInsiderSwitch", DestPoint, "diffSwitchInsider");
                }
            }
        }

        private void MoveInsider(ICySymbolShapeEdit_v1 shapeEdit, string pillarTag, PointF DestPoint, string targetTag)
        {
            PointF pillarLine = GetLineLeftPoint(shapeEdit.GetShapeBounds(pillarTag));

            PointF offSet = new PointF((DestPoint.X - pillarLine.X), (DestPoint.Y - pillarLine.Y));

            shapeEdit.ShapesMoveBy(targetTag, offSet);
        }

        private PointF GetLineLeftPoint(RectangleF bound)
        {
            return new PointF(bound.X, (bound.Y + bound.Height/2));
        }

        private PointF GetRectCenter(RectangleF rect)
        {
            return new PointF((rect.X + rect.Width/2), (rect.Y + rect.Height/2));
        }

        private string GetTermSuffix(uint terminalWidth)
        {
            string termSuffix = string.Empty;
            if (terminalWidth > 1)
            {
                termSuffix = string.Format("[{0}:0]", terminalWidth - 1);
            }

            return termSuffix;
        }

        #endregion

        #region ICyVerilogCustomize_v1 Members

        public CyCustErr CustomizeVerilog(ICyInstQuery_v1 instQuery, ICyTerminalQuery_v1 termQuery,
                                          out string codeSnippet)
        {
            // Sanity check
            Debug.Assert(instQuery != null);
            if (instQuery == null)
            {
                codeSnippet = string.Empty;
                return new CyCustErr("Invalid instance query parameter");
            }

            // Get parameters
            uint numTerminals, channelWidth, mode;
            bool showEnable;
            CyCustErr err = ReadParamter(instQuery, out numTerminals, out channelWidth, out mode, out showEnable);
            if (err.IsNotOk)
            {
                codeSnippet = string.Empty;
                return err;
            }

            uint numSelectorTerms = (mode == MUX_MODE) ? (GetNumSelectorBits(numTerminals)) : numTerminals;

            string mux_A_suffix = string.Empty;
            string mux_B_suffix = string.Empty;
            if (channelWidth == DIFFERENTIAL_MUX)
            {
                mux_A_suffix = "_CYAMUXSIDE_A";
                mux_B_suffix = "_CYAMUXSIDE_B";
            }

            // Collect all input/output signals
            string outASigSegName = string.Empty;
            string outBSigSegName = string.Empty;
            string selSigSegNames = string.Empty;

            // Collect the signal segment names for each of the instance terminals
            List<string> inASigSegNames = new List<string>();
            List<string> inBSigSegNames = new List<string>();

            // Note: This works because the term names are generated by us in
            // CustomizeShapes(). 
            for (int index = 0; index < numTerminals; index++)
            {
                inASigSegNames.Add(termQuery.GetTermSigSegScalarName(GetTermName(INPUT_A_TERM_BASE_NAME, index, true)));
            }
            selSigSegNames =
                termQuery.GetTermSigSegName(INPUT_SEL_BUS_BASE_NAME + GetTermSuffix(numSelectorTerms));
            outASigSegName = termQuery.GetTermSigSegScalarName(OUTPUT_A_TERM_BASE_NAME);

            // Check if this is a differential mux
            if (channelWidth == DIFFERENTIAL_MUX)
            {
                for (int index = 0; index < numTerminals; index++)
                {
                    inBSigSegNames.Add(termQuery.GetTermSigSegScalarName(
                        GetTermName(INPUT_B_TERM_BASE_NAME, index, true)));
                }
                outBSigSegName = termQuery.GetTermSigSegScalarName(OUTPUT_B_TERM_BASE_NAME);
            }

            // Collect Clock and enable signal if this is a mux.
            string clockSigSegName = string.Empty;
            string enableSigSegName = string.Empty;
            if (mode == MUX_MODE)
            {
                clockSigSegName = termQuery.GetTermSigSegScalarName(INPUT_CLOCK_NAME);

                // We only add "enalbe" terminal when it is required by the user.
                if (showEnable)
                    enableSigSegName = termQuery.GetTermSigSegScalarName(INPUT_ENABLE_NAME);
            }

            string instanceName = instQuery.InstanceName;

            CyVerilogBuilder vBuilder = new CyVerilogBuilder();
            vBuilder.AddComment("-- AMuxHw " + instanceName + " start -- ***");
            vBuilder.WriteLine("");

            string decoderOutputSuffix = string.Empty;
            string decoderOutputName = string.Empty;

            // Write out AmuxHw Decoder if it is a mux.
            if (mode == MUX_MODE)
            {
                WriteOutDecoderVerilog(vBuilder, instanceName, numSelectorTerms, numTerminals, showEnable,
                                       enableSigSegName, selSigSegNames, clockSigSegName, out decoderOutputSuffix,
                                       out decoderOutputName);
            }

            // Build Mux A verilog code
            vBuilder.AddComment("-- AMuxHw Primitive A --");
            vBuilder.WriteLine("");

            WriteAMuxHwPrimitiveVerilog(vBuilder, numTerminals, mode, instanceName, mux_A_suffix,
                                        inASigSegNames, outASigSegName, selSigSegNames, decoderOutputSuffix,
                                        decoderOutputName);

            // Build Mux B verilog code if it is Differential.
            if (channelWidth == DIFFERENTIAL_MUX)
            {
                // Build  Mux B verilog code
                vBuilder.WriteLine("");
                vBuilder.AddComment("-- AMuxHw Primitive B --");
                vBuilder.WriteLine("");

                WriteAMuxHwPrimitiveVerilog(vBuilder, numTerminals, mode, instanceName, mux_B_suffix,
                                            inBSigSegNames, outBSigSegName, selSigSegNames, decoderOutputSuffix,
                                            decoderOutputName);
            }

            vBuilder.AddComment("-- AMuxHw " + instanceName + " end -- ***");
            codeSnippet = vBuilder.VerilogString;

            return CyCustErr.OK;
        }

        /// <summary>
        /// This method generates the decoder verilog code on the fly. 
        /// One example code it generates is as below:
        /// -- AMuxHw Decoder Start--
        /// reg [3:0] AMuxHw_1_Decoder_one_hot;
        /// reg [1:0] AMuxHw_1_Decoder_old_id;
        /// wire  AMuxHw_1_Decoder_is_active;
        /// wire  AMuxHw_1_Decoder_enable;
        /// assign AMuxHw_1_Decoder_enable = 1'b1;
        /// genvar AMuxHw_1_Decoder_i;
        /// assign AMuxHw_1_Decoder_is_active = (Net_121[1:0] == AMuxHw_1_Decoder_old_id) && AMuxHw_1_Decoder_enable;
        /// always @(posedge Net_122)
        /// begin
        ///    AMuxHw_1_Decoder_old_id = Net_121[1:0];
        /// end
        /// generate
        ///     for (AMuxHw_1_Decoder_i = 0; AMuxHw_1_Decoder_i < 4; AMuxHw_1_Decoder_i = AMuxHw_1_Decoder_i + 1 )
        ///     begin : AMuxHw_1_OutBit
        ///         always @(posedge Net_122)
        ///         begin
        ///             AMuxHw_1_Decoder_one_hot[AMuxHw_1_Decoder_i] <= (AMuxHw_1_Decoder_old_id == AMuxHw_1_Decoder_i) && AMuxHw_1_Decoder_is_active;
        ///         end
        ///     end
        /// endgenerate
        /// -- AMuxHw Decoder End--
        /// </summary>
        /// <param name="vBuilder"></param>
        /// <param name="instanceName"></param>
        /// <param name="numSelectorTerms"></param>
        /// <param name="numTerminals"></param>
        /// <param name="showEnable"></param>
        /// <param name="enableSigSegName"></param>
        /// <param name="selSigSegNames"></param>
        /// <param name="clockSigSegName"></param>
        /// <param name="decoderOutputSuffix"></param>
        /// <param name="decoderOutputName"></param>
        private void WriteOutDecoderVerilog(CyVerilogBuilder vBuilder, string instanceName,
                                            uint numSelectorTerms, uint numTerminals, bool showEnable,
                                            string enableSigSegName,
                                            string selSigSegNames, string clockSigSegName,
                                            out string decoderOutputSuffix, out string decoderOutputName)
        {
            string decoderInputSuffix = string.Format("[{0}:0]", numSelectorTerms - 1);
            decoderOutputSuffix = string.Format("[{0}:0]", numTerminals - 1);

            string amuxhwDecoderName = instanceName + "_Decoder";
            decoderOutputName = amuxhwDecoderName + "_" + DECODER_OUTPUT;
            string decoderRegOldIdName = amuxhwDecoderName + "_" + DECODER_REG_OLD_ID;
            string decoderWireIsActiveName = amuxhwDecoderName + "_" + DECODER_WIRE_ACTIVE;
            string decoderEnableName = amuxhwDecoderName + "_" + INPUT_ENABLE_NAME;
            string decoderVariableName = amuxhwDecoderName + "_" + "i";

            // Build Decoder component
            vBuilder.AddComment("-- AMuxHw Decoder Start--");
            vBuilder.WriteLine("");

            // Declaration for all register and wire
            vBuilder.DeclareReg(decoderOutputName, decoderOutputSuffix);
            vBuilder.DeclareReg(decoderRegOldIdName, decoderInputSuffix);
            vBuilder.DeclareWire(decoderWireIsActiveName, string.Empty);
            vBuilder.DeclareWire(decoderEnableName, string.Empty);
            vBuilder.WriteLine("");

            // Assign enalbe signal according to the "showEnable" parameter.
            string assginEnableRight = (!showEnable) ? "1'b1" : enableSigSegName;
            vBuilder.AddAssignStatement(decoderEnableName, assginEnableRight);
            vBuilder.WriteLine("");

            string genvarDec = string.Format("genvar {0};", decoderVariableName);
            vBuilder.WriteLine(genvarDec);
            vBuilder.WriteLine("");

            string assignIsActiveRight =
                string.Format("({0} == {1}) && {2}", selSigSegNames, decoderRegOldIdName, decoderEnableName);
            vBuilder.AddAssignStatement(decoderWireIsActiveName, assignIsActiveRight);
            vBuilder.WriteLine("");

            List<string> clockSignals = new List<string>();
            clockSignals.Add(clockSigSegName);
            List<CyVerilogBuilder.EdgeTypeEnum> clockEdges = new List<CyVerilogBuilder.EdgeTypeEnum>();
            clockEdges.Add(CyVerilogBuilder.EdgeTypeEnum.POSITIVE);
            vBuilder.DefineAlways(clockSignals, clockEdges);
            vBuilder.BeginBlock();
            vBuilder.AddStatement(decoderRegOldIdName, selSigSegNames);
            vBuilder.EndBlock();
            vBuilder.WriteLine("");

            vBuilder.AddGenerate();
            string forLoop =
                string.Format("for ({0} = 0; {0} < {1}; {0} = {0} + 1 )", decoderVariableName, numTerminals);
            vBuilder.WriteLine(forLoop);
            vBuilder.BeginBlock(instanceName + "_OutBit");
            vBuilder.DefineAlways(clockSignals, clockEdges);
            vBuilder.BeginBlock();
            string statementLeft = string.Format(decoderOutputName + "[{0}]", decoderVariableName);
            string statementRight =
                string.Format("({0} == {1}) && {2}",
                              decoderRegOldIdName, decoderVariableName, decoderWireIsActiveName);
            vBuilder.AddUnblockingStatement(statementLeft, statementRight);
            vBuilder.EndBlock();
            vBuilder.EndBlock();
            vBuilder.AddEndGenerate();
            vBuilder.WriteLine("");

            vBuilder.AddComment("-- AMuxHw Decoder End--");
            vBuilder.WriteLine("");
        }


        /// <summary>
        /// This method generates the AMux prmitive verilog code which matches the AMux primitive definition in
        /// warp.
        /// One example code it generates is as below:
        /// // -- AMuxHw Primitive A --
        /// cy_psoc3_amux_v1_0 #(
        ///     .muxin_width(4),
        ///     .hw_control(1),
        ///     .one_active(1),
        ///     .init_mux_sel(4'h0),
        ///     .api_type(2'b10))
        ///     AMuxHw_1_Primitive(
        ///     .muxin({
        ///         Net_119,
        ///         Net_118,
        ///         Net_117,
        ///         Net_116
        ///         }),
        ///     .hw_ctrl_en(AMuxHw_1_Decoder_one_hot[3:0]),
        ///     .vout(Net_120)
        ///     );
        ///  -- AMuxHw AMuxHw_1 end -- ***
        /// </summary>
        /// <param name="vBuilder"></param>
        /// <param name="numTerminals"></param>
        /// <param name="mode"></param>
        /// <param name="instanceName"></param>
        /// <param name="muxSuffix"></param>
        /// <param name="inSigSegNames"></param>
        /// <param name="outSigSegName"></param>
        /// <param name="selSigSegName"></param>
        /// <param name="decoderOutputSuffix"></param>
        /// <param name="decoderOutputName"></param>
        private void WriteAMuxHwPrimitiveVerilog(CyVerilogBuilder vBuilder, uint numTerminals, uint mode,
                                                 string instanceName,
                                                 string muxSuffix, List<string> inSigSegNames, string outSigSegName,
                                                 string selSigSegName,
                                                 string decoderOutputSuffix, string decoderOutputName)
        {
            vBuilder.WriteLine(AMUX_PRIM_NAME + " " + "#(");
            vBuilder.IncreaseIndent();

            // Param Define for AmuxHw Primitive.
            // Note: We prefer named paramter passing style for param def.
            vBuilder.WriteLine(".muxin_width(" + numTerminals.ToString() + "),");
            vBuilder.WriteLine(".hw_control(" + "1" + "),");
            if (mode == MUX_MODE)
            {
                vBuilder.WriteLine(".one_active(" + "1" + "),");
            }
            else
            {
                vBuilder.WriteLine(".one_active(" + "0" + "),");
            }
            vBuilder.WriteLine(".init_mux_sel(" + numTerminals.ToString() + "'h0" + "),");
            vBuilder.WriteLine(".api_type(" + "2'b10" + "))");

            // Port Define
            string amuxhwPrimitiveName = instanceName;
            vBuilder.WriteLine(amuxhwPrimitiveName + muxSuffix + "(");

            vBuilder.WriteLine(".muxin({");
            vBuilder.IncreaseIndent();
            // Add the signal names for the muxin input
            for (int i = (inSigSegNames.Count - 1); i >= 0; i--)
            {
                if (i > 0)
                {
                    vBuilder.WriteLine(inSigSegNames[i] + ",");
                }
                else
                {
                    vBuilder.WriteLine(inSigSegNames[i]);
                }
            }
            vBuilder.WriteLine("}),");
            vBuilder.DecreaseIndent();

            // Add the signal for .hw_ctrl_en port
            // This signal is connected to decoder's output for a mux. Otherwise, it directly connected to the 
            // input port.
            if (mode == MUX_MODE)
            {
                vBuilder.WriteLine(".hw_ctrl_en(" + decoderOutputName + decoderOutputSuffix + "),");
            }
            else
            {
                vBuilder.WriteLine(".hw_ctrl_en(" + selSigSegName + "),");
            }

            // Add the signal for .vout port.
            vBuilder.WriteLine(".vout(" + outSigSegName + ")");
            vBuilder.WriteLine(");");
            vBuilder.DecreaseIndent();
            vBuilder.WriteLine("");
        }

        #endregion

        #region shared private method

        private string GetTermName(string baseName, int index, bool isScale)
        {
            if (isScale)
            {
                return baseName + index;
            }
            else
            {
                return string.Format(baseName + "[{0}]", index);
            }
        }

        private uint GetNumSelectorBits(uint n)
        {
            return (n == 1) ? 1 : (uint) Math.Ceiling(Math.Log((double) n, 2));
        }

        private CyCustErr ReadParamter(ICyInstQuery_v1 instQuery,
                                       out uint numTerminals, out uint channelWidth, out uint mode, out bool showEnable)
        {
            CyCustErr err = CyCustErr.Ok;
            try
            {
                CyCompDevParam numTerminals_param = instQuery.GetCommittedParam(SYMBOL_PARAM_CHANNEL);
                numTerminals = uint.Parse(numTerminals_param.Value);

                CyCompDevParam ChannelWidth_param = instQuery.GetCommittedParam(SYMBOL_PARAM_TYPE);
                channelWidth = uint.Parse(ChannelWidth_param.Value);

                CyCompDevParam mode_param = instQuery.GetCommittedParam(SYMBOL_PARAM_MODE);
                mode = uint.Parse(mode_param.Value);

                CyCompDevParam showEnable_param = instQuery.GetCommittedParam(SYMBOL_PARAM_ENABLE);
                showEnable = bool.Parse(showEnable_param.Value);
            }
            catch (Exception e)
            {
                numTerminals = channelWidth = mode = 0;
                showEnable = false;
                err = new CyCustErr(e);
            }
            return err;
        }

        #endregion
    }
}
