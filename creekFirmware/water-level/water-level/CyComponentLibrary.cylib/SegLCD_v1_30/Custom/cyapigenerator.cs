// ========================================
//
// Copyright Cypress Semiconductor Corporation, 2009
// All Rights Reserved
// UNPUBLISHED, LICENSED SOFTWARE.
//
// CONFIDENTIAL AND PROPRIETARY INFORMATION
// WHICH IS THE PROPERTY OF CYPRESS.
//
// Use of this file is governed
// by the license agreement included in the file
//
//     <install>/license/license.txt
//
// where <install> is the Cypress software
// installation root directory path.
//
// ========================================

using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Drawing;
using System.Text;
using System.IO;
using CyDesigner.Extensions.Gde;

namespace SegLCD_v1_30
{
    partial class CyAPIGenerator
    {
        #region Header

        private string m_instanceName;

        public CyAPIGenerator(string instanceName)
        {
            this.m_instanceName = instanceName;
        }
        #endregion

        #region Header Data
        public void CollectApiHeader(ICyInstQuery_v1 inst, ref List<CyHelperInfo> HelpersConfig, ref Dictionary<string,
                                     string> paramDict, int pNumCommonLines, int pNumSegmentLines)
        {
            int index = 0;
            TextWriter writerCVariables = new StringWriter();
            writerCVariables.NewLine = "\n";

            TextWriter writerHPixelDef = new StringWriter();
            writerHPixelDef.NewLine = "\n";

            //GANG
            string gang;
            paramDict.TryGetValue(CyLCDParameters.PARAM_GANG, out gang);
            if (Convert.ToBoolean(Convert.ToInt32(gang)))
            {
                string defStr = "uint16 " + m_instanceName + "_gCommons[" + pNumCommonLines + "] = {";
                for (int i = 0; i < pNumCommonLines; i++)
                {
                    defStr += m_instanceName + "_GCom" + i + ", ";
                }
                defStr = defStr.TrimEnd(' ', ',');
                defStr += "};";
                defStr = StringLineBreaking(defStr);
                writerCVariables.WriteLine(defStr);
                writerCVariables.WriteLine("");

                writerHPixelDef.WriteLine("#define " + m_instanceName + "_GANG");
                for (int i = 0; i < pNumCommonLines; i++)
                {
                    int comIndex = i;
                    writerHPixelDef.WriteLine("#define " + m_instanceName + "_GCom" + comIndex + "    " + m_instanceName
                       + "_FIND_PIXEL(" + m_instanceName + "_GCom__LCD_COM_PORT__" + comIndex + ",  " + m_instanceName +
                         "_GCom__LCD_COM_PIN__" + comIndex + ",  " + comIndex + ")");
                }
            }

            for (int i = 0; i < HelpersConfig.Count; i++)
            {
                CyHelperInfo hi = HelpersConfig[i];
                if (hi.Kind != CyHelperKind.EMPTY)
                {
                    writerCVariables.WriteLine("uint8 " + m_instanceName + "_digitNum_" + index + " = " +
                                               hi.SymbolsCount + ";");
                    Write_HeaderMassiv(ref writerCVariables, hi, index);
                    index++;
                }
            }



            // Array of commons
            writerCVariables.WriteLine("");
            string commons = "";
            for (int i = 0; i < pNumCommonLines; i++)
                commons += m_instanceName+"_Com" + i.ToString() + ", ";
            commons = commons.TrimEnd(' ', ',');
            commons = "uint16 " + m_instanceName + "_commons" + "[" + pNumCommonLines + "] = {" +
                      commons + "};";
            commons = StringLineBreaking(commons);
            writerCVariables.WriteLine(commons);

            writerHPixelDef.WriteLine("");

            //Common Generation Sample
            for (int i = 0; i < pNumCommonLines; i++)
            {
                int comIndex = i;
                writerHPixelDef.WriteLine("#define " + m_instanceName + "_COM" + comIndex + "_PORT" + "            " +
                                          m_instanceName + "_Com__LCD_COM_PORT__" + comIndex);
                writerHPixelDef.WriteLine("#define " + m_instanceName + "_COM" + comIndex + "_PIN" + "            " +
                                          m_instanceName + "_Com__LCD_COM_PIN__" + comIndex);
            }
            writerHPixelDef.WriteLine("");

            for (int i = 0; i < pNumCommonLines; i++)
            {
                int comIndex = i;
                writerHPixelDef.WriteLine("#define " + m_instanceName + "_Com" + comIndex + "            " +
                                          m_instanceName + "_FIND_PIXEL(" + m_instanceName + "_COM" + comIndex +
                                          "_PORT,  " +
                                          m_instanceName + "_COM" + comIndex + "_PIN,  " + comIndex + ")");

            }
            writerHPixelDef.WriteLine("");

            //Segmet Generation Sample
            for (int i = 0; i < pNumSegmentLines; i++)
            {
                int segIndex = i;
                writerHPixelDef.WriteLine("#define " + m_instanceName + "_SEG" + segIndex + "_PORT" + "            " +
                                          m_instanceName + "_Seg__LCD_SEG_PORT__" + segIndex);
                writerHPixelDef.WriteLine("#define " + m_instanceName + "_SEG" + segIndex + "_PIN" + "            " +
                                          m_instanceName + "_Seg__LCD_SEG_PIN__" + segIndex);
            }
            writerHPixelDef.WriteLine("");

            List<Point> usedComSeg = new List<Point>();
            for (int i = 0; i < HelpersConfig.Count; i++)
            {
                CyHelperInfo hi = HelpersConfig[i];
                if (hi.Kind == CyHelperKind.EMPTY) continue;

                for (int j = 0; j < hi.HelpSegInfo.Count; j++)
                {
                    CyHelperSegmentInfo si = hi.HelpSegInfo[j];
                
                    // Define only assigned pixels
                    if ((si.Common >= 0) && (si.Segment >= 0))
                    {
                        writerHPixelDef.WriteLine("#define " + si.Name + "            " + m_instanceName +
                                                  "_FIND_PIXEL(" + m_instanceName + "_SEG" + si.Segment + "_PORT,  " +
                                                  m_instanceName + "_SEG" + si.Segment + "_PIN,  " + si.Common + ")");
                        usedComSeg.Add(new Point(si.Common, si.Segment));
                    }
                }
            }
            writerHPixelDef.WriteLine("");
            //Add pixels of Empty helper that weren't added before
            for (int i = 0; i < HelpersConfig[0].HelpSegInfo.Count; i++)
            {
                CyHelperSegmentInfo si = HelpersConfig[0].HelpSegInfo[i];
            
                if (!usedComSeg.Contains(new Point(si.Common, si.Segment)))
                {
                    writerHPixelDef.WriteLine("#define " + si.Name + "            " + m_instanceName +
                                              "_FIND_PIXEL(" + m_instanceName + "_SEG" + si.Segment + "_PORT,  " +
                                              m_instanceName + "_SEG" + si.Segment + "_PIN,  " + si.Common + ")");
                }
            }

            //Add not connected define
            writerHPixelDef.WriteLine();
            writerHPixelDef.WriteLine("#define NOT_CON            99");

            paramDict.Add("writerCVariables", writerCVariables.ToString());
            paramDict.Add("writerHPixelDef", writerHPixelDef.ToString());
        }

        /// <summary>
        /// Breaks up string into lines with max length 120.
        /// </summary>
        /// <param name="str"></param>
        /// <returns></returns>
        private string StringLineBreaking(string str)
        {
            const int MAX_LEN = 120;
            int index = MAX_LEN;
            while (index < str.Length)
            {
                while ((str[index] != ' ') && (index > 0))
                    index--;
                if (index > 0)
                {
                    // Add "\r\n" at the place of the line breaking
                    str = str.Insert(index + 1, "\r\n");
                    // Remove ' '
                    str = str.Remove(index, 1);
                    // Add 
                    index += MAX_LEN + 2;
                }
            }
            return str;
        }

        #endregion

        #region Core
        public void CollectApiCore(ICyInstQuery_v1 inst, ref List<CyHelperInfo> HelpersConfig,
                                   ref Dictionary<string, string> paramDict, int pNumCommonLines, int pNumSegmentLines)
        {

            #region FuncDeclarations
            TextWriter writerHFuncDeclarations = new StringWriter();
            writerHFuncDeclarations.NewLine = "\n";

            TextWriter writerCFunctions = new StringWriter();
            writerCFunctions.NewLine = "\n";

            //first one is blank
            for (int i = 1; i < HelpersConfig.Count; i++)
            {
                int p = i - 1;
                switch (HelpersConfig[i].Kind)
                {
                    case CyHelperKind.SEGMENT_7:
                        Write7SegDigit_n(ref writerCFunctions, ref writerHFuncDeclarations, p);
                        Write7SegNumber_n(ref writerCFunctions, ref writerHFuncDeclarations, p);
                        break;
                    case CyHelperKind.SEGMENT_14:
                        PutChar14seg_n(ref writerCFunctions, ref writerHFuncDeclarations, p);
                        WriteString14seg_n(ref writerCFunctions, ref writerHFuncDeclarations, p);
                        break;
                    case CyHelperKind.SEGMENT_16:
                        PutChar16seg_n(ref writerCFunctions, ref writerHFuncDeclarations, p);
                        WriteString16seg_n(ref writerCFunctions, ref writerHFuncDeclarations, p);
                        break;
                    case CyHelperKind.BAR:
                        WriteBargraph_n(ref writerCFunctions, ref writerHFuncDeclarations, p,
                                        HelpersConfig[i].SymbolsCount);
                        break;
                    case CyHelperKind.MATRIX:
                        PutCharDotMatrix_n(ref writerCFunctions, ref writerHFuncDeclarations, p);
                        WriteStringDotMatrix_n(ref writerCFunctions, ref writerHFuncDeclarations, p);
                        break;
                    case CyHelperKind.EMPTY:

                        break;
                    default:
                        break;
                }
            }

            paramDict.Add("writerCFunctions", writerCFunctions.ToString());
            paramDict.Add("writerHFuncDeclarations", writerHFuncDeclarations.ToString());
            #endregion

        }
        #endregion

        #region Functions

        string MassToString(CyHelperInfo hi)
        {
            string str = "";
            string[,] p = new string[hi.SymbolsCount, hi.SegmentCount];

            for (int i = 0; i < hi.HelpSegInfo.Count; i++)
            {
                CyHelperSegmentInfo hsi = hi.HelpSegInfo[i];
                if ((hsi.Segment >= 0) && (hsi.Common >= 0))
                    p[hsi.m_DigitNum,hsi.m_RelativePos] = hsi.Name;
                else
                {
                    p[hsi.m_DigitNum, hsi.m_RelativePos] = "NOT_CON";
                }
            }
            for (int i = 0; i < hi.SymbolsCount; i++)
            {
                str += "{";
                for (int j = 0; j < hi.SegmentCount; j++)
                {
                    str += p[i, j];
                    if (j != hi.SegmentCount - 1) str += ", ";
                }
                str += "}";
                if (i != hi.SymbolsCount - 1) str += ", ";
            }
            return str;

        }

        void Write_HeaderMassiv(ref TextWriter writer, CyHelperInfo hi, int index)
        {
            string str = MassToString(hi);
            if (hi.Kind != CyHelperKind.BAR)
            {
                str = "const uint16 " + m_instanceName + "_disp" + index + "[" + hi.SymbolsCount + "][" +
                         hi.SegmentCount + "] = {" + str + "};";
                str = StringLineBreaking(str);
                writer.WriteLine(str);
            }
            else
            {
                str = "const uint16 " + m_instanceName + "_disp" + index + "[" + (hi.SymbolsCount + 1) + "]["
                         + hi.SegmentCount + "] = {{0}, " + str + "};";
                str = StringLineBreaking(str);
                writer.WriteLine(str);
            }
        }
        #endregion
    }
}
