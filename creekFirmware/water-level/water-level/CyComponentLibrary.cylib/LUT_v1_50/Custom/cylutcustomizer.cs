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
using System.Diagnostics;
using CyDesigner.Extensions.Common;
using CyDesigner.Extensions.Gde;
using LUT_v1_50;

namespace LUT_v1_50
{
    [CyCompDevCustomizer()]
    public class CyCustomizer : ICyParamEditHook_v1, ICyVerilogCustomize_v1
    {

        #region ICyParamEditHook_v1 Members
        public ICyInstEdit_v1 m_Component = null;

        public DialogResult EditParams(ICyInstEdit_v1 edit, ICyTerminalQuery_v1 termQuery, ICyExpressMgr_v1 mgr)
        {
            m_Component = edit;
            ICyTabbedParamEditor editor = edit.CreateTabbedParamEditor();

            CyParamExprDelegate ParamCommitted = delegate(ICyParamEditor custEditor, CyCompDevParam comp)
            {
                //For right now it needs to atleast set a default BitField of the correct Length
                //The correct length is (2^NumInputs + 2^NumOutputs) / 4 because it is represented
                //in hex which defines each nibble as a character
                if (comp.Name == "NumInputs" || comp.Name == "NumOutputs")
                {
                    int inputs = Convert.ToInt16(m_Component.GetCommittedParam("NumInputs").Value);
                    int outputs = Convert.ToInt16(m_Component.GetCommittedParam("NumOutputs").Value);
                    string temp = "0".PadLeft(Convert.ToInt16((Math.Pow(2.0, inputs) + Math.Pow(2.0, outputs)) / 4), '0');
                    m_Component.SetParamExpr("BitField", temp);
                }
            };

            //TODO: hookup correctly to get expression view
            editor.AddCustomPage("Configure", new CyBitFieldEditingControl(edit), null, "Basic");
            editor.AddDefaultPage("Built-in", "Built-in");
            DialogResult result = editor.ShowDialog();
            editor.ParamExprCommittedDelegate = ParamCommitted;
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

        
        #region ICyVerilogCustomize_v1 Members

        public CyCustErr CustomizeVerilog(ICyInstQuery_v1 query, ICyTerminalQuery_v1 termQuery, out string codeSnippet)
        {
            Debug.Assert(query != null);
            if (query == null)
            {
                codeSnippet = string.Empty;
                return new CyCustErr("Invalid instance query parameter");
            }

            CyCompDevParam NumInputs_param = query.GetCommittedParam("NumInputs");
            int NumInputs = int.Parse(NumInputs_param.Value);

            CyCompDevParam NumOutputs_param = query.GetCommittedParam("NumOutputs");
            int NumOutputs = int.Parse(NumOutputs_param.Value);

            int validNoBits = (int)Math.Pow(2, NumInputs) * NumOutputs;

            CyCompDevParam BitField_param = query.GetCommittedParam("BitField");
            string BitField = CreateBinaryBitFieldFromString(BitField_param.Value);
            int validIndex = BitField.Length - validNoBits;
            BitField = BitField.Substring(validIndex);

            CyCompDevParam RegdOutputs = query.GetCommittedParam("RegisterOutputs");

            // Collect the signal segment names for each of the instance terminals
            List<string> inSigSegNames = new List<string>();
            List<string> outTermSigSegName = new List<string>();

            // For Each Input find out what terminal is connected to it.
            string InSigSegName = termQuery.GetTermSigSegName("in0");
            inSigSegNames.Add(InSigSegName);
            if (NumInputs > 1)
            {
                InSigSegName = termQuery.GetTermSigSegName("in1");
                inSigSegNames.Add(InSigSegName);
            }
            if (NumInputs > 2)
            {
                InSigSegName = termQuery.GetTermSigSegName("in2");
                inSigSegNames.Add(InSigSegName);
            }
            if (NumInputs > 3)
            {
                InSigSegName = termQuery.GetTermSigSegName("in3");
                inSigSegNames.Add(InSigSegName);
            }
            if (NumInputs > 4)
            {
                InSigSegName = termQuery.GetTermSigSegName("in4");
                inSigSegNames.Add(InSigSegName);
            }
            List<string> ClockNames = new List<string>();
            if (RegdOutputs.Value.ToString() == "true")
            {
                string clockname = termQuery.GetTermSigSegName("clock");
                ClockNames.Add(clockname);
            }

            // For Each Output find out what terminal is connected to it.oh well
            string outputConcatString = "{";
            for (int j = (NumOutputs - 1); j >= 0; j--)
            {
                string OutSigSegName = termQuery.GetTermSigSegName("out" + j);
                outputConcatString += OutSigSegName;
                if (j > 0)
                    outputConcatString += ",";
            }
            outputConcatString += "}";

            // Get selector terminal name and width
            string instanceName = query.InstanceName;

            CyVerilogBuilder vBuilder = new CyVerilogBuilder();
            vBuilder.AddComment("-- LUT " + instanceName + " start --");
            vBuilder.AddIfGenerateStmt("1", instanceName);
            string tempregsuffix = "[" + (NumOutputs - 1) + ":0]";
            string outputregname = "tmp__" + instanceName + "_reg";
            vBuilder.DeclareReg(outputregname, tempregsuffix);
            
            string sensitivity = null;
            switch (NumInputs)
            {
                case 1: sensitivity = "{" + inSigSegNames[0] + "}"; break;
                case 2: sensitivity = "{" + inSigSegNames[1] + "," + inSigSegNames[0] + "}"; break;
                case 3: sensitivity = "{" + inSigSegNames[2] + "," + inSigSegNames[1] + "," + inSigSegNames[0] + "}"; break;
                case 4: sensitivity = "{" + inSigSegNames[3] + "," + inSigSegNames[2] + "," + inSigSegNames[1] + "," + inSigSegNames[0] + "}"; break;
                case 5: sensitivity = "{" + inSigSegNames[4] + "," + inSigSegNames[3] + "," + inSigSegNames[2] + "," + inSigSegNames[1] + "," + inSigSegNames[0] + "}"; break;
            }

            if (RegdOutputs.Value.ToString() == "true")
            {
                vBuilder.DeclareWire("tmp__" + instanceName + "_ins", "[" + (NumInputs - 1) + ":0]");
                vBuilder.AddAssignStatement("tmp__" + instanceName + "_ins", sensitivity);
                vBuilder.DefineAlways(ClockNames, CyVerilogBuilder.EdgeTypeEnum.POSITIVE);
            }
            else
            {
                vBuilder.DeclareWire("tmp__" + instanceName + "_ins", "[" + (NumInputs - 1) + ":0]");
                vBuilder.AddAssignStatement("tmp__" + instanceName + "_ins", sensitivity);
                List<string> instnamelist = new List<string>();
                instnamelist.Add("tmp__" + instanceName + "_ins");
                vBuilder.DefineAlways(instnamelist, CyVerilogBuilder.EdgeTypeEnum.NONE);
            }
            vBuilder.BeginBlock();

            vBuilder.AddCaseStmt("tmp__" + instanceName + "_ins");
            for (int i = 0; i <= (int)(Math.Pow(2, NumInputs) - 1); i++)
            {
                string caseOption = NumInputs + "'b" + Convert.ToString(i, 2).PadLeft(NumInputs, '0');
                string assignment = "";
                for (int x = ((NumOutputs * (i + 1)) - 1); x >= (NumOutputs * i); x--)
                {
                    assignment = BitField[x].ToString() + assignment;
                }
                assignment = NumOutputs + "'b" + assignment;
                vBuilder.AddCaseStmtCase(caseOption, outputregname, assignment);
            }

            vBuilder.AddEndCaseStmt();
            vBuilder.EndBlock(); // End of always
            vBuilder.AddAssignStatement(outputConcatString, outputregname);
            vBuilder.AddEndIfStmt();
            vBuilder.AddComment("-- LUT " + instanceName + " end --");

            codeSnippet = vBuilder.VerilogString;

            return CyCustErr.OK;
        }

        private string CreateBinaryBitFieldFromString(string bitField)
        {
            //Reverse Parse the bit-FieldText
            string binaryRep = "";
            while (bitField.Length > 0)
            {
                switch (bitField[0])
                {
                    case '0': binaryRep += "0000"; break;
                    case '1': binaryRep += "0001"; break;
                    case '2': binaryRep += "0010"; break;
                    case '3': binaryRep += "0011"; break;
                    case '4': binaryRep += "0100"; break;
                    case '5': binaryRep += "0101"; break;
                    case '6': binaryRep += "0110"; break;
                    case '7': binaryRep += "0111"; break;
                    case '8': binaryRep += "1000"; break;
                    case '9': binaryRep += "1001"; break;
                    case 'A': binaryRep += "1010"; break;
                    case 'B': binaryRep += "1011"; break;
                    case 'C': binaryRep += "1100"; break;
                    case 'D': binaryRep += "1101"; break;
                    case 'E': binaryRep += "1110"; break;
                    case 'F': binaryRep += "1111"; break;
                }
                bitField = bitField.Substring(1);
            }
            return binaryRep;
        }

        #endregion
    }




    //Create a new control to add to a tab
    public class CyBitFieldEditingControl : ICyParamEditingControl
    {
        CyBitfieldControl m_control;

        public CyBitFieldEditingControl(ICyInstEdit_v1 inst)
        {
            m_control = new CyBitfieldControl(inst);
            m_control.Dock = DockStyle.Fill;
        }

        #region ICyParamEditingControl Members
        public Control DisplayControl
        {
            get { return m_control; }
        }

        public IEnumerable<CyCustErr> GetErrors()
        {
            return new CyCustErr[] { };    //return an empty array
        }

        #endregion
    }
}
