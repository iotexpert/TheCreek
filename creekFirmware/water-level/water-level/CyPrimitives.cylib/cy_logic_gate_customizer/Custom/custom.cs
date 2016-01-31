/*******************************************************************************
* Copyright 2008-2012, Cypress Semiconductor Corporation.  All rights reserved.
* You may use this file only in accordance with the license, terms, conditions,
* disclaimers, and limitations in the end user license agreement accompanying 
* the software package with which this file was provided.
*******************************************************************************/
using System;
using System.Collections.Generic;
using System.Text;

using System.Diagnostics;
using System.Drawing;
using System.IO;
using System.Windows.Forms;

using CyDesigner.Extensions.Common;
using CyDesigner.Extensions.Gde;

namespace Cypress.Semiconductor.CyDesigner.cy_logic_gate_customizer
{

    public class CyBuilder
    {
        public static float GRID_POINTS;
        uint m_actualTermCnt;
        uint m_sizeForTermCnt;
        const int SPACING_UNITS = 1;

        public CyBuilder(uint actualTermCnt, float gridPoints)
            : this(actualTermCnt, actualTermCnt, gridPoints)
        {
        }

        /// <summary>
        /// Provision for given number of terminals, but pretend there are sizeForTermCnt terminals 
        /// for the purpose of computing sizes.
        /// </summary>
        /// <param name="actualTermCnt"></param>
        /// <param name="sizeForTermCnt"></param>
        public CyBuilder(uint actualTermCnt, uint sizeForTermCnt, float gridPoints)
        {
            m_actualTermCnt = actualTermCnt;
            m_sizeForTermCnt = sizeForTermCnt;
            GRID_POINTS = gridPoints;
        }

        /// <summary>
        /// Gets a co-ordinate at 12pt spacing
        /// </summary>
        /// <returns></returns>
        IEnumerable<float> GetNextYCoordinate()
        {
            int i = 0;
            while (i < m_actualTermCnt)
            {
                float yc = i * 2 * GRID_POINTS;
                i++;

                yield return yc;
            }
        }

        public float TopY
        {
            get
            {
                float height = ((m_sizeForTermCnt - 1) * 2 + 2) * GRID_POINTS;
                return -(height / 2f);
            }
        }

        public float BottomY
        {
            get
            {
                float height = ((m_sizeForTermCnt - 1) * 2 + 2) * GRID_POINTS;
                return (height / 2f);
            }
        }

        public float InTermExtLineStart
        {
            get { return GRID_POINTS; }
        }

        public float InTermExtLineEnd
        {
            get { return (2 * GRID_POINTS); }
        }

        public float OutTermExtLineStart
        {
            get { return (6 * GRID_POINTS); }
        }

        public float OutTermExtLineEnd
        {
            get { return (7 * GRID_POINTS); }
        }

        public float OutTermContactPoint
        {
            get { return (8 * GRID_POINTS); }
        }

        public PointF TopLeft
        {
            get { return new PointF((2 * GRID_POINTS), -(2 * GRID_POINTS)); }
        }

        public PointF TopRight
        {
            get { return new PointF((4 * GRID_POINTS), -(2 * GRID_POINTS)); }
        }

        public PointF BottomLeft
        {
            get { return new PointF((2 * GRID_POINTS), (2 * GRID_POINTS)); }
        }

        public PointF BottomRight
        {
            get { return new PointF((4 * GRID_POINTS), (2 * GRID_POINTS)); }
        }

        public IEnumerable<PointF> GetNextLocation()
        {
            int i = 0;
            while (i < m_actualTermCnt)
            {
                float yc = i * 2 * GRID_POINTS;
                i++;
                yield return new PointF(0f, yc);
            }
        }

        public IEnumerable<float> GetTerminalOffsets()
        {
            List<float> terminalOffsets = new List<float>();

            if (m_actualTermCnt % 2 == 0)
            {
                for (int i = 2; i <= m_actualTermCnt; i += 2)
                {
                    float offset = getOffset(i);
                    terminalOffsets.Add(offset);
                    terminalOffsets.Add(-(offset));
                }
            }
            else
            {
                for (int i = 1; i <= m_actualTermCnt; i += 2)
                {
                    float offset = getOffset(i);
                    if (i == 1)
                    {
                        terminalOffsets.Add(offset);
                    }
                    else
                    {
                        terminalOffsets.Add(offset);
                        terminalOffsets.Add(-(offset));
                    }
                }
            }
            Debug.Assert(terminalOffsets.Count == m_actualTermCnt, "Incorrect number of terminals laid out");
            terminalOffsets.Sort();

            return terminalOffsets;
        }

        private float getOffset(int n)
        {
            return n > 0 ? ((n - 1) * (SPACING_UNITS * GRID_POINTS)) : 0;
        }

        public static string GetTermSuffix(uint terminalWidth)
        {
            string termSuffix = string.Empty;
            if (terminalWidth > 1)
            {
                termSuffix = string.Format("[{0}:0]", terminalWidth - 1);
            }

            return termSuffix;
        }

        public static string GetTermSuffix(int left, int right)
        {
            return string.Format("[{0}:{1}]", left, right);
        }

    }

    public class CyBinaryStringGenerator
    {
        uint m_max;
        uint m_numBits;

        public CyBinaryStringGenerator(uint n)
        {
            Debug.Assert(n < (uint)int.MaxValue);
            m_max = n;
            m_numBits = GetNumSelectorBits(n);
        }

        public static uint GetNumSelectorBits(uint n)
        {
            return (uint)Math.Log((double)n, 2);
        }


        public IEnumerable<string> GetBinaryStr()
        {
            for (int i = 0; i < m_max; i++)
            {
                string binStr = Convert.ToString(i, 2);
                yield return binStr.PadLeft((int)m_numBits, '0');
            }
        }

        public static string GetStringOf1s(uint n)
        {
            StringBuilder sb = new StringBuilder();
            for (uint i = 0; i < n; i++)
                sb.Append("1");
            return sb.ToString();
        }

        public static string GetStringOf0s(uint n)
        {
            StringBuilder sb = new StringBuilder();
            for (uint i = 0; i < n; i++)
                sb.Append("0");
            return sb.ToString();
        }
    }

    public enum CyGateType
    {
        AND,
        NAND,
        NOT,
        OR,
        NOR,
        XOR,
        XNOR
    }

    public class CyShapeCustomizer
    {
        const string GENERATED_SHAPE = "SymbolShape";

        public static CyCustErr CustomizeGateShapesWithArcs(
            ICyInstQuery_v1 instQuery,
            ICySymbolShapeEdit_v1 shapeEdit,
            ICyTerminalEdit_v1 termEdit,
            CyGateType gateType)
        {
            // We leave the symbol as it is for symbol preview
            if (instQuery.IsPreviewCanvas)
                return CyCustErr.OK;

            shapeEdit.ShapesRemove("DummyTerminal");
            shapeEdit.ShapesRemove("SymbolName");
            termEdit.RemoveAllTerminals();

            // Add term shapes
            List<string> shapeTags = new List<string>();
            shapeTags.Add(GENERATED_SHAPE);

            CyCompDevParam numTerminals_param = instQuery.GetCommittedParam("NumTerminals");
            uint numTerminals = uint.Parse(numTerminals_param.Value);
            CyBuilder helper = new CyBuilder(numTerminals, shapeEdit.UserBaseUnit);

            CyCompDevParam terminalWidth_param = instQuery.GetCommittedParam("TerminalWidth");
            uint terminalWidth = uint.Parse(terminalWidth_param.Value);
            string suffix = CyBuilder.GetTermSuffix(terminalWidth);

            float delta = 0;

            int index = 0;
            foreach (float offset in helper.GetTerminalOffsets())
            {
                // The input term extender lines for terminals are drawn only 
                // when required to ensure the terminals touch the gate shape
                // and not intrude into the gate shape
                if (gateType == CyGateType.OR || gateType == CyGateType.NOR)
                {
                    if ((numTerminals % 2 == 0) ||
                        (numTerminals == 5 && (index % 2 == 0)) ||
                        ((numTerminals == 3 || numTerminals == 7)&& (index % 2 != 0)))
                    {
                        delta = (numTerminals % 2) == 0 ? 1f : 2f;
                        shapeEdit.CreatePolyline(shapeTags,
                            new PointF(helper.InTermExtLineStart, offset),
                            new PointF(helper.InTermExtLineEnd + delta, offset));
                            
                    }
                    else if ((numTerminals == 5 && (index % 2 != 0)) ||
                            ((numTerminals == 3 || numTerminals == 7) && (index % 2 == 0)))
                        {
                            delta = 3f;
                            shapeEdit.CreatePolyline(shapeTags,
                                new PointF(helper.InTermExtLineStart, offset),
                                new PointF(helper.InTermExtLineEnd - delta, offset));
                        }
                }
                else
                {
                    if (numTerminals % 2 == 0)
                    {
                        shapeEdit.CreatePolyline(shapeTags,
                            new PointF(helper.InTermExtLineStart, offset),
                            new PointF(helper.InTermExtLineStart + 7f, offset));
                    }
                    else if (((numTerminals == 3 || numTerminals == 7) && (index % 2 != 0)) ||
                        (numTerminals == 5 && (index % 2 == 0)))
                    {
                        shapeEdit.CreatePolyline(shapeTags,
                        new PointF(helper.InTermExtLineStart, offset),
                        new PointF(helper.InTermExtLineStart + 8f, offset));
                    }
                }

                string termName = "term" + index.ToString() + suffix;
                termEdit.TerminalAdd(termName, CyCompDevTermDir.INPUT,
                    CyCompDevTermType.DIGITAL, new PointF(0f, offset), string.Empty, 0f, false);
                index++;
            }

            // Output Terminal

            // extender line
            if (gateType == CyGateType.OR || gateType == CyGateType.XOR)
            {
                shapeEdit.CreatePolyline(shapeTags,
                    new PointF(helper.OutTermExtLineStart + 3f, 0f),
                    new PointF(helper.OutTermExtLineEnd, 0f));
            }

            // terminal
            PointF outTermLoc = new PointF(helper.OutTermExtLineEnd + CyBuilder.GRID_POINTS, 0f);
            termEdit.TerminalAdd("gateOut" + suffix, CyCompDevTermDir.OUTPUT,
                CyCompDevTermType.DIGITAL, outTermLoc,
                string.Empty, 0f, false);

            // Gate shape
            if (numTerminals > 3)
            {
                CreateGateArc("arc00", new PointF(2f, -(6 * CyBuilder.GRID_POINTS)), shapeEdit);
                CreateGateArc("arc01", new PointF(2f, (2 * CyBuilder.GRID_POINTS)), shapeEdit);
                if (gateType == CyGateType.XOR || gateType == CyGateType.XNOR)
                {
                    CreateGateArc("arc10",
                        new PointF(-(CyBuilder.GRID_POINTS / 3), -(6 * CyBuilder.GRID_POINTS)), shapeEdit);
                    CreateGateArc("arc11",
                        new PointF(-(CyBuilder.GRID_POINTS / 3), (2 * CyBuilder.GRID_POINTS)), shapeEdit);
                }
            }
            if (numTerminals > 7)
            {
                CreateGateArc("arc20", new PointF(2f, -(10 * CyBuilder.GRID_POINTS)), shapeEdit);
                CreateGateArc("arc21", new PointF(2f, (6 * CyBuilder.GRID_POINTS)), shapeEdit);

                if (gateType == CyGateType.XOR || gateType == CyGateType.XNOR)
                {
                    CreateGateArc("arc30",
                        new PointF(-(CyBuilder.GRID_POINTS / 3), -(10 * CyBuilder.GRID_POINTS)), shapeEdit);
                    CreateGateArc("arc31",
                        new PointF(-(CyBuilder.GRID_POINTS / 3), (6 * CyBuilder.GRID_POINTS)), shapeEdit);
                }
            }

            // Note, the shape with the curved lines making up the gate is in 
            // the symbol file

            // Set the outline width for all the shapes created
            // width set to 0 to match the width of instance terminals
            shapeEdit.SetOutlineWidth(GENERATED_SHAPE, (0 * CyBuilder.GRID_POINTS));
            shapeEdit.SetOutlineWidth("arc", (0 * CyBuilder.GRID_POINTS));

            return CyCustErr.OK;
        }

        private static void CreateGateHalfArc(
            string tag,
            PointF location,
            ICySymbolShapeEdit_v1 shapeEdit,
            bool flipVertical)
        {
            List<string> st = new List<string>();
            st.Add("arc");
            st.Add(tag);
            RectangleF arcRect = new RectangleF(location,
                new SizeF((2 * CyBuilder.GRID_POINTS), (4 * CyBuilder.GRID_POINTS)));
            shapeEdit.CreateArc(st, arcRect, -(15 * CyBuilder.GRID_POINTS), (15 * CyBuilder.GRID_POINTS));
            if (flipVertical)
                shapeEdit.ShapesFlipVertical(tag);
        }

        private static void CreateGateArc(
            string tag,
            PointF location,
            ICySymbolShapeEdit_v1 shapeEdit)
        {
            List<string> st1 = new List<string>();
            st1.Add("arc");
            st1.Add(tag);
            st1.Add(tag + "_top_half");

            List<string> st2 = new List<string>();
            st1.Add("arc");
            st2.Add(tag);
            st2.Add(tag + "_bottom_half");

            PointF topArcLoc = location;
            PointF bottomArcLoc = new PointF(location.X, location.Y + (2 * CyBuilder.GRID_POINTS));

            RectangleF arcRect1 = new RectangleF(topArcLoc,
                new SizeF((2 * CyBuilder.GRID_POINTS), (4 * CyBuilder.GRID_POINTS)));
            shapeEdit.CreateArc(st1, arcRect1, -(15 * CyBuilder.GRID_POINTS), (15 * CyBuilder.GRID_POINTS));

            RectangleF arcRect2 = new RectangleF(bottomArcLoc,
                new SizeF((2 * CyBuilder.GRID_POINTS), (4 * CyBuilder.GRID_POINTS)));
            shapeEdit.CreateArc(st2, arcRect2, -(15 * CyBuilder.GRID_POINTS), (15 * CyBuilder.GRID_POINTS));
            shapeEdit.ShapesFlipVertical(tag + "_bottom_half");
        }

        public static CyCustErr CustomizeGateShapesForAnd(
            ICyInstQuery_v1 instQuery,
            ICySymbolShapeEdit_v1 shapeEdit,
            ICyTerminalEdit_v1 termEdit,
            CyGateType gateType)
        {
            // We leave the symbol as it is for symbol preview
            if (instQuery.IsPreviewCanvas)
                return CyCustErr.OK;

            shapeEdit.ShapesRemove("DummyTerminal");
            shapeEdit.ShapesRemove("SymbolName");
            termEdit.RemoveAllTerminals();

            // Add term shapes
            List<string> shapeTags = new List<string>();
            shapeTags.Add(GENERATED_SHAPE);

            CyCompDevParam numTerminals_param = instQuery.GetCommittedParam("NumTerminals");
            uint numTerminals = uint.Parse(numTerminals_param.Value);
            CyBuilder helper = new CyBuilder(numTerminals, shapeEdit.UserBaseUnit);

            CyCompDevParam terminalWidth_param = instQuery.GetCommittedParam("TerminalWidth");
            uint terminalWidth = uint.Parse(terminalWidth_param.Value);
            string suffix = CyBuilder.GetTermSuffix(terminalWidth);

            int index = 0;
            foreach (float offset in helper.GetTerminalOffsets())
            {
                shapeEdit.CreatePolyline(shapeTags,
                    new PointF(helper.InTermExtLineStart, offset),
                    new PointF(helper.InTermExtLineEnd, offset));

                string termName = "term" + index.ToString() + suffix;
                termEdit.TerminalAdd(termName, CyCompDevTermDir.INPUT,
                    CyCompDevTermType.DIGITAL, new PointF(0f, offset), string.Empty, 0f, false);
                index++;
            }

            // Output Terminal
            if (gateType == CyGateType.AND)
            {
                shapeEdit.CreatePolyline(shapeTags,
                    new PointF(helper.OutTermExtLineStart, 0f),
                    new PointF(helper.OutTermExtLineEnd, 0f));
            }
            termEdit.TerminalAdd("gateOut" + suffix, CyCompDevTermDir.OUTPUT,
            CyCompDevTermType.DIGITAL, new PointF(helper.OutTermContactPoint, 0f),
            string.Empty, 0f, false);

            // Gate shape

            // Vertical line after the input terminals
            shapeEdit.CreatePolyline(shapeTags,
                new PointF(helper.InTermExtLineEnd, helper.TopY),
                new PointF(helper.InTermExtLineEnd, helper.BottomY));

            // Note, the shape with the curved line making up AND gate is in 
            // the symbol file

            // Set the outline width for all the shapes created
            // width set to 0 to match the width of instance terminals
            shapeEdit.SetOutlineWidth(GENERATED_SHAPE, 0f);

            return CyCustErr.OK;
        }
    }

    public class CyGateVerilogBuilder
    {
        public static CyCustErr BuildVerilog(
            CyGateType gateType,
            ICyTerminalQuery_v1 termQuery,
            out string codeSnippet)
        {
            char operatorSymbol = ' ';
            if (gateType == CyGateType.OR || gateType == CyGateType.NOR)
                operatorSymbol = '|';
            else if (gateType == CyGateType.XOR || gateType == CyGateType.XNOR)
                operatorSymbol = '^';
            else
                Debug.Fail("Gate type not supported by this builder");

            bool invRequired = false;
            if (gateType == CyGateType.OR || gateType == CyGateType.XOR)
                invRequired = false;
            else if (gateType == CyGateType.NOR || gateType == CyGateType.XNOR)
                invRequired = true;

            Debug.Assert(termQuery != null);
            if (termQuery == null)
            {
                codeSnippet = string.Empty;
                return new CyCustErr("Invalid instance query parameter");
            }

            // Collect the signal segment names for each of the instance terminals
            List<string> inSigSegNames = new List<string>();
            string outTermSigSegName = string.Empty;
            foreach (string termName in termQuery.GetTerminalNames())
            {
                CyCompDevTermDir termDir = termQuery.GetTermDirection(termName);
                string sigSegName = termQuery.GetTermSigSegName(termName);
                if (termDir == CyCompDevTermDir.INPUT)
                    inSigSegNames.Add(sigSegName);
                if (termDir == CyCompDevTermDir.OUTPUT)
                    outTermSigSegName = sigSegName;
            }

            // Generate the verilog code snippet,  example, "assign out_wire = in1 ^ in2 ^ in3;"
            StringBuilder sb = new StringBuilder();
            sb.Append(Environment.NewLine);
            sb.Append("    ");
            sb.Append(string.Format("assign {0} = ", outTermSigSegName));

            if (invRequired)
                sb.Append("~(");
            int i = 1;
            foreach (string sigName in inSigSegNames)
            {
                sb.Append(sigName);
                if (i < inSigSegNames.Count)
                    sb.Append(string.Format(" {0} ", operatorSymbol));
                i++;
            }

            if (invRequired)
                sb.Append(")");
            sb.Append(";");
            sb.Append(Environment.NewLine);
            codeSnippet = sb.ToString();

            return CyCustErr.OK;
        }
    }

    public class CyVerilogBuilder
    {
        const int INDENT = 4;

        int m_indent;
        StringWriter m_stringWriter;

        public CyVerilogBuilder()
        {
            m_indent = INDENT;
            m_stringWriter = new StringWriter();
        }

        public string VerilogString
        {
            get { return m_stringWriter.ToString(); }
        }

        public void IncreaseIndent()
        {
            m_indent += INDENT;
        }

        public void DecreaseIndent()
        {
            m_indent -= INDENT;
            Debug.Assert(m_indent >= 0, "Incorrect indent");
        }

        public void AddComment(string comment)
        {
            WriteLine("// " + comment);
        }

        public void WriteLine(string text)
        {
            Write(text);
            m_stringWriter.WriteLine("");
        }

        public void Write(string text)
        {
            for (int i = 0; i < m_indent; i++)
                m_stringWriter.Write(" ");
            m_stringWriter.Write(text);
        }

        public void DeclareReg(string name, string suffix)
        {
            string str = string.Format("reg {0} {1};", suffix, name);
            WriteLine(str);
        }

        public void DeclareWire(string name, string suffix)
        {
            string str = string.Format("wire {0} {1};", suffix, name);
            WriteLine(str);
        }

        public enum EdgeTypeEnum
        {
            NONE,
            POSITIVE,
            NEGATIVE
        }

        public void DefineAlways(List<string> signals)
        {
             string alwaysStmt = string.Format("always @(");
            Write(alwaysStmt);
            for (int i = 0; i < signals.Count; i++)
            {
                m_stringWriter.Write(signals[i]);
                if (i < signals.Count - 1)
                    m_stringWriter.Write(" or ");
            }
            m_stringWriter.WriteLine(")");
        }

        public void DefineAlways(List<string> signals, List<EdgeTypeEnum> edgeTypes)
        {
            Debug.Assert(signals.Count == edgeTypes.Count);

            string alwaysStmt = string.Format("always @(");
            Write(alwaysStmt);
            for (int i = 0; i < signals.Count; i++)
            {
                string edge = string.Empty;
                EdgeTypeEnum e = edgeTypes[i];
                if (e == EdgeTypeEnum.POSITIVE)
                    edge = "posedge ";
                else if (e == EdgeTypeEnum.NEGATIVE)
                    edge = "negedge ";

                m_stringWriter.Write(edge);
                m_stringWriter.Write(signals[i]);
                if (i < signals.Count - 1)
                    m_stringWriter.Write(" or ");
            }
            m_stringWriter.WriteLine(")");
        }


        public void AddStatement(string left, string right)
        {
            string str = string.Format("{0} = {1};", left, right);
            WriteLine(str);
        }

        public void AddAssignStatement(string left, string right)
        {
            AddStatement("assign " + left, right);
        }

        public void BeginBlock()
        {
            WriteLine("begin");
            IncreaseIndent();
        }

        private void BeginBlock(string instanceName)
        {
            string str = string.Format("begin : {0}", instanceName);
            WriteLine(str);
            IncreaseIndent();
        }

        public void EndBlock()
        {
            DecreaseIndent();
            WriteLine("end");
        }

        public void DefineParam(string instance_name, string paramName, string val)
        {
            string paramDefinition = string.Format("defparam {0}.{1} = {2};", instance_name, paramName, val);
            WriteLine(paramDefinition);
        }

        public void AddIfStmt(string expression)
        {
            string ifStmt = string.Format("if ({0})", expression);
            WriteLine(ifStmt);
            BeginBlock();
        }

        internal void AddIfGenerateStmt(string expression, string instanceName)
        {
            string ifStmt = string.Format("if ({0})", expression);
            WriteLine(ifStmt);
            BeginBlock(instanceName);
        }

        public void InstantiateModule(string moduleName, string instanceName, List<string> parameters)
        {
            Write(moduleName);
            m_stringWriter.Write(" ");
            m_stringWriter.Write(instanceName);
            m_stringWriter.Write("(");

            for (int i = 0; i < parameters.Count; i++)
            {
                m_stringWriter.Write(parameters[i]);
                if (i != parameters.Count - 1)
                    m_stringWriter.Write(", ");
            }
            m_stringWriter.WriteLine(");");
        }

        public void AddEndIfStmt()
        {
            EndBlock();
        }

        public void AddElseStmt()
        {
            WriteLine("else");
            BeginBlock();
        }

        public void AddElseIfStmt(string expression)
        {
            string ifStmt = string.Format("else if ({0})", expression);
            WriteLine(ifStmt);
            BeginBlock();
        }

        public void AddCaseStmt(string selector)
        {
            string caseStmt = string.Format("case ({0})", selector);
            WriteLine(caseStmt);
            IncreaseIndent();
        }

        public void AddEndCaseStmt()
        {
            DecreaseIndent();
            WriteLine("endcase");
        }

        public void AddCaseStmtCase(string caseValue, string left, string right)
        {
            string stmt = string.Format("{0} :  {1} = {2};",
                    caseValue, left, right);
            WriteLine(stmt);
        }

        public void AddGenerate()
        {
            WriteLine("generate");
            IncreaseIndent();
        }

        public void AddEndGenerate()
        {
            DecreaseIndent();
            WriteLine("endgenerate");
        }

        public static string GetTmpName(string prefix, string instanceName, string suffix)
        {
            return string.Format("{0}__{1}_{2}", prefix, instanceName, suffix);
        }
    }

}
//[]//
