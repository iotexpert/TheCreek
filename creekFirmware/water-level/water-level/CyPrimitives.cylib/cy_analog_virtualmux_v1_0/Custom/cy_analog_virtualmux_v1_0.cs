/*******************************************************************************
* Copyright 2008-2012, Cypress Semiconductor Corporation.  All rights reserved.
* You may use this file only in accordance with the license, terms, conditions, 
* disclaimers, and limitations in the end user license agreement accompanying 
* the software package with which this file was provided.
********************************************************************************/

using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Drawing;
using System.Text;
using System.Windows.Forms;

using CyDesigner.Extensions.Common;
using CyDesigner.Extensions.Gde;

using  Cypress.Semiconductor.CyDesigner.cy_logic_gate_customizer;

namespace Cypress.Semiconductor.CyDesigner.cy_analog_virtualmux_v1_0
{
    public class CyCustomizer :
        ICyVerilogCustomize_v1,
        ICyShapeCustomize_v1
    {
        const string INPUT_TERM_BASE_NAME = "i";
        const string OUTPUT_TERM_BASE_NAME = "lm_out";
        const string MUX_BASE_SHAPE_TAG = "mux_base_shape";
        const string ZERO_ANN_SHAPE_TAG = "zero_annotation";
        const string TERM_WIDTH_ANN_TAG = "term_width_annotation";

        const float ZERO_LABEL_X_POS = 7.5f;
        const float ZERO_LABEL_Y_OFFSET = -3.0f;
        const float ZERO_LABEL_HEIGHT = 6.0f;
        const float ZERO_LABEL_WIDTH = 6.0f;

        const string SelectedInput = "SelectedInput";
        const string NumInputTerminals = "NumInputTerminals";
        const string TerminalWidth = "TerminalWidth";

        static string GetInputTermFullName(uint termNum, uint termWidth)
        {
            return INPUT_TERM_BASE_NAME + termNum + CyBuilder.GetTermSuffix(termWidth);
        }

        string GetOutputTermName(uint termWidth)
        {
            return OUTPUT_TERM_BASE_NAME + CyBuilder.GetTermSuffix(termWidth);
        }

        #region ICyVerilogCustomize_v1 Members

        public CyCustErr CustomizeVerilog(
            ICyInstQuery_v1 instQuery,
            ICyTerminalQuery_v1 termQuery,
            out string codeSnippet)
        {
            CyCompDevParam termWidthParam = instQuery.GetCommittedParam(TerminalWidth);
            Debug.Assert(termWidthParam != null && termWidthParam.ErrorCount == 0);
            uint termWidth = uint.Parse(termWidthParam.Value);

            CyCompDevParam selectorParam = instQuery.GetCommittedParam(SelectedInput);
            Debug.Assert(selectorParam != null && selectorParam.ErrorCount == 0);
            uint selector = uint.Parse(selectorParam.Value);

            string selectedTermName = GetInputTermFullName(selector, termWidth);
            string outTermName = GetOutputTermName(termWidth);

            string selectedSig = termQuery.GetTermSigSegName(selectedTermName);
            string outputSig = termQuery.GetTermSigSegName(outTermName);

            string cyConnectName = String.Format("{0}_connect",
                                                 instQuery.InstanceName);

            StringBuilder sb = new StringBuilder();
            sb.Append("\t// ");
            sb.Append(instQuery.InstanceName);
            sb.Append(" (");
            sb.Append(instQuery.ComponentName);
            sb.Append(")");
            sb.Append(Environment.NewLine);
            sb.Append("\tcy_connect_v1_0 ");
            sb.Append(cyConnectName);
            sb.Append("(");
            sb.Append(outputSig);
            sb.Append(", ");
            sb.Append(selectedSig);
            sb.Append(");");
            sb.Append(Environment.NewLine);
            sb.Append("\tdefparam ");
            sb.Append(cyConnectName);
            sb.Append(".sig_width = ");
            sb.Append(termWidth);
            sb.Append(";");
            sb.Append(Environment.NewLine);

            codeSnippet = sb.ToString();

            return CyCustErr.OK;
        }

        #endregion

        #region ICyShapeCustomize_v1 Members

        public CyCustErr CustomizeShapes(
            ICyInstQuery_v1 instQuery,
            ICySymbolShapeEdit_v1 shapeEdit,
            ICyTerminalEdit_v1 termEdit)
        {
            shapeEdit.RemoveAllShapes();
            termEdit.RemoveAllTerminals();

            // Add term shapes

            CyCompDevParam numTerminals_param = instQuery.GetCommittedParam(NumInputTerminals);
            uint numTerminals = uint.Parse(numTerminals_param.Value);

            // make the trapezoid think there are a min num of terminals, else it becomes a triangle
            uint trapTermCnt = numTerminals;
            if (trapTermCnt <= 2) trapTermCnt = 3;
            CyBuilder helper = new CyBuilder(numTerminals, trapTermCnt, shapeEdit.UserBaseUnit);

            CyCompDevParam terminalWidth_param = instQuery.GetCommittedParam(TerminalWidth);
            uint terminalWidth = uint.Parse(terminalWidth_param.Value);

            // Input terminals
            uint index = 0;
            float i0PosY = 0;
            foreach (float offset in helper.GetTerminalOffsets())
            {
                if (i0PosY == 0)
                {
                    i0PosY = offset;
                }

                string termName = GetInputTermFullName(index, terminalWidth);
                termEdit.TerminalAdd(termName, CyCompDevTermDir.INOUT,
                    CyCompDevTermType.ANALOG, new PointF(0f, offset), string.Empty, 0f, false);
                index++;
            }

            // Output terminal
            termEdit.TerminalAdd(GetOutputTermName(terminalWidth), CyCompDevTermDir.INOUT,
                CyCompDevTermType.ANALOG, new PointF(36f, 0f),
                string.Empty, 180f, false);

            // Mux Shape
            PointF topLeft = new PointF(6f, helper.TopY - 6f);
            PointF bottomLeft = new PointF(6f, helper.BottomY + 6f);
            PointF topRight = new PointF(30f, helper.TopY + 12f);
            PointF bottomRight = new PointF(30f, helper.BottomY - 12f);
            PointF selectorTermLocation = new PointF(18f, helper.BottomY + 6f);

            // Mux trapezoid
            List<string> tags = new List<string>();
            tags.Add(MUX_BASE_SHAPE_TAG);
            shapeEdit.CreatePolyline(tags, topLeft, topRight, bottomRight, bottomLeft, topLeft);
            shapeEdit.ShapesConvertToClosed(tags, tags);
            shapeEdit.SetFillColor(MUX_BASE_SHAPE_TAG, instQuery.Preferences.SchematicAnalogTerminalColor);
			shapeEdit.SetOutlineWidth(MUX_BASE_SHAPE_TAG, 1f);

            // Terminal Width
            tags.Clear();
            tags.Add(TERM_WIDTH_ANN_TAG);
            float x = Math.Max(topLeft.X, bottomLeft.X);
            float y = Math.Max(topLeft.Y, topRight.Y);
            float width = Math.Min(topRight.X, bottomRight.X) - x;
            float height = Math.Min(bottomLeft.Y, bottomRight.Y) - y;
            RectangleF textBounds = new RectangleF(x, y, width, height);
            StringFormat fmt = new StringFormat();
            fmt.Alignment = StringAlignment.Center;
            fmt.LineAlignment = StringAlignment.Center;
            shapeEdit.CreateAnnotation(tags,
                "/" + terminalWidth.ToString(), textBounds,
                new Font(FontFamily.GenericSansSerif, 6, FontStyle.Regular), fmt);
            shapeEdit.SetFillColor(TERM_WIDTH_ANN_TAG, Color.Black);
            shapeEdit.ClearOutline(TERM_WIDTH_ANN_TAG);

            // Grey 0
            tags.Clear();
            tags.Add(ZERO_ANN_SHAPE_TAG);
            shapeEdit.CreateAnnotation(tags, "0",
                new RectangleF(ZERO_LABEL_X_POS, i0PosY + ZERO_LABEL_Y_OFFSET, ZERO_LABEL_WIDTH, ZERO_LABEL_HEIGHT),
                new Font(FontFamily.GenericSansSerif, 6, FontStyle.Regular), StringFormat.GenericDefault);
            shapeEdit.SetFillColor(ZERO_ANN_SHAPE_TAG, Color.FromArgb(190, Color.Black));
            shapeEdit.ClearOutline(ZERO_ANN_SHAPE_TAG);

            return CyCustErr.OK;
        }

        #endregion
    }

}
