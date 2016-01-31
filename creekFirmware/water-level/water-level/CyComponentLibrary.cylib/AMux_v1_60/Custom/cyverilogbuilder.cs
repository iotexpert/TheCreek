/*******************************************************************************
* Copyright 2008-2009, Cypress Semiconductor Corporation.  All rights reserved.
* You may use this file only in accordance with the license, terms, conditions, 
* disclaimers, and limitations in the end user license agreement accompanying 
* the software package with which this file was provided.
********************************************************************************/



using System;
using System.Collections.Generic;
using System.Text;
using System.IO;
using System.Diagnostics;

namespace AMux_v1_60
{
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

            string edge = string.Empty;

            string alwaysStmt = string.Format("always @(");
            Write(alwaysStmt);
            for (int i = 0; i < signals.Count; i++)
            {
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
            DecreaseIndent();
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
