/*******************************************************************************
* Copyright 2012, Cypress Semiconductor Corporation.  All rights reserved.
* You may use this file only in accordance with the license, terms, conditions, 
* disclaimers, and limitations in the end user license agreement accompanying 
* the software package with which this file was provided. 
********************************************************************************/

using System;
using System.Collections.Generic;
using System.Text;
using System.Windows.Forms;
using CyDesigner.Extensions.Common;
using CyDesigner.Extensions.Gde;

namespace ThermistorCalc_v1_0
{
    public class CyCustomizer : ICyParamEditHook_v1, ICyAPICustomize_v1
    {
        #region ICyParamEditHook_v1 implementation
        public DialogResult EditParams(ICyInstEdit_v1 edit, ICyTerminalQuery_v1 termQuery, ICyExpressMgr_v1 mgr)
        {
            CyParameters parameters = new CyParameters(edit);

            parameters.GlobalEditMode = false;

            ICyTabbedParamEditor editor = edit.CreateTabbedParamEditor();
            //editor.UseBigEditor = true;

            CyGeneralTab basicTab = new CyGeneralTab(parameters);

            CyParamExprDelegate dataChanged =
            delegate(ICyParamEditor custEditor, CyCompDevParam param)
            {
                parameters.GlobalEditMode = false;
                basicTab.UpdateUI();
                parameters.GlobalEditMode = true;
            };

            editor.AddCustomPage(Resources.GeneralTabDisplayName, basicTab, dataChanged, basicTab.TabName);
            editor.AddDefaultPage(Resources.BuiltInTabDisplayName, "Built-in");

            basicTab.UpdateUI();
            parameters.GlobalEditMode = true;
            basicTab.RunMode = true;

            return editor.ShowDialog();
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

        #region ICyAPICustomize_v1 implementation
        private const byte MAX_LINE_LENGTH = 120;
        private const string UNSIGNED = "u";
        private const string DEFINE_FORMAT = "#define {0}{1}               ({2})";

        public IEnumerable<CyAPICustomizer> CustomizeAPIs(ICyInstQuery_v1 query, ICyTerminalQuery_v1 termQuery,
            IEnumerable<CyAPICustomizer> apis)
        {
            List<CyAPICustomizer> customizers = new List<CyAPICustomizer>(apis);
            Dictionary<string, string> paramDict = new Dictionary<string, string>();

            if (customizers.Count > 0)
            {
                paramDict = customizers[0].MacroDictionary;
            }

            CyParameters parameters = new CyParameters(query);
            string instanceName = query.InstanceName;

            if (parameters.Implementation == CyEImplementation.Equation)
            {
                double a, b, c;
                parameters.CalculateSteinhartHartCoefficients(out a, out b, out c);

                string definesString = string.Format(DEFINE_FORMAT, instanceName, "_THA", (float)a);
                definesString += Environment.NewLine;
                definesString += string.Format(DEFINE_FORMAT, instanceName, "_THB", (float)b);
                definesString += Environment.NewLine;
                definesString += string.Format(DEFINE_FORMAT, instanceName, "_THC", (float)c);

                paramDict.Add("definesString", WordWrap(definesString));
            }
            else
            {
                double[,] LUT = parameters.GenerateLUT();
                int lutSize = parameters.LUTSize;

                string definesString = string.Format(DEFINE_FORMAT, instanceName, "_LUT_SIZE",
                    lutSize.ToString() + UNSIGNED);

                string lookUpTableStr = "const uint32 CYCODE " + instanceName + "_LUT[] = { ";
                
                for (int i = 0; i < lutSize; i++)
                {
                    lookUpTableStr += ((i > 0) ? ", " + ((UInt32)Math.Round(LUT[i, 0])).ToString() :
                        ((UInt32)Math.Round(LUT[i, 0])).ToString()) + UNSIGNED;
                }
                lookUpTableStr += " };";

                paramDict.Add("definesString", WordWrap(definesString));
                paramDict.Add("lookUpTableString", WordWrap(lookUpTableStr));
            }

            for (int i = 0; i < customizers.Count; i++)
            {
                customizers[i].MacroDictionary = paramDict;
            }

            return customizers;
        }

        private string WordWrap(string str)
        {
            string result = string.Empty;
            string line = string.Empty;
            string[] splitted = str.Split(',');
            for (int i = 0; i < splitted.Length; i++)
            {
                if ((line.Length + splitted[i].Length + 1) <= MAX_LINE_LENGTH)
                {
                    line += splitted[i] + ",";
                }
                else
                {
                    line += Environment.NewLine;
                    result += line;
                    line = string.Empty;
                    line += splitted[i] + ",";
                }
            }
            result += line.Remove(line.Length - 1);

            return result;
        }
        #endregion
    }
}
