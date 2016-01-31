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

namespace StaticSegLCD_v1_30
{
    partial class M_APIGenerator
    {
        #region Header

        private string m_instanceName;

        public M_APIGenerator(string instanceName)
        {
            this.m_instanceName = instanceName;
        }
        #endregion

        #region Header Data
        public void CollectApiHeader(ICyInstQuery_v1 inst, ref List<HelperInfo> HelpersConfig, ref Dictionary<string, 
                                     string> paramDict, int pNumCommonLines, int pNumSegmentLines)
        {
            int index = 0;
            TextWriter writerCVariables = new StringWriter();
            writerCVariables.NewLine = "\n";

            TextWriter writerHPixelDef = new StringWriter();
            writerHPixelDef.NewLine = "\n";

            //GANG
            string gang;
            paramDict.TryGetValue("Gang", out gang);
            if (Convert.ToBoolean(Convert.ToInt32(gang)))
            {
                string defStr = "uint16 " + m_instanceName + "_GCommons[" + pNumCommonLines + "] = {";
                for (int i = 0; i < pNumCommonLines; i++)
                {
                    defStr += m_instanceName + "_GCom" + i + ", "; 
                }
                defStr = defStr.TrimEnd(' ', ',');
                defStr += "};";
                writerCVariables.WriteLine(defStr);
                writerCVariables.WriteLine("");

                writerHPixelDef.WriteLine("#define GANG");
                for (int i = 0; i < pNumCommonLines; i++)
                {
                    int comIndex = i;
                    writerHPixelDef.WriteLine("#define " + m_instanceName + "_GCom" + comIndex + "	" + m_instanceName + 
                        "_FIND_PIXEL(" + m_instanceName + "_GCom__LCD_COM_PORT__" + comIndex + ",  " + m_instanceName + 
                        "_GCom__LCD_COM_PIN__" + comIndex + ",  " + comIndex + ")");
                }
            }

            foreach (HelperInfo hi in HelpersConfig)
            {
                if (hi.Kind != CyHelperKind.Empty)
                {
                    writerCVariables.WriteLine("uint8 " + m_instanceName + "_DigitNum_" + index + " = " +
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
            writerCVariables.WriteLine("uint16 " + m_instanceName + "_Commons" + "[" + pNumCommonLines + "] = {" +
                                       commons + "};");

            writerHPixelDef.WriteLine("");

            //Common Generation Sample
            for (int i = 0; i < pNumCommonLines; i++) 
            {
                int comIndex = i;
                writerHPixelDef.WriteLine("#define " + m_instanceName + "_COM" + comIndex + "_PORT" + "                " +
                                          m_instanceName + "_Com__LCD_COM_PORT__" + comIndex);
                writerHPixelDef.WriteLine("#define " + m_instanceName + "_COM" + comIndex + "_PIN" + "                " +
                                          m_instanceName + "_Com__LCD_COM_PIN__" + comIndex);
            }
            writerHPixelDef.WriteLine("");

            for (int i = 0; i < pNumCommonLines; i++)
            {
                int comIndex = i;
                writerHPixelDef.WriteLine("#define " + m_instanceName + "_Com" + comIndex + "                " + m_instanceName +
                                          "_FIND_PIXEL(" + m_instanceName + "_COM" + comIndex + "_PORT,  " +
                                          m_instanceName + "_COM" + comIndex + "_PIN,  " + comIndex + ")");
              
            }
            writerHPixelDef.WriteLine("");

            //Segmet Generation Sample
            for (int i = 0; i < pNumSegmentLines; i++)
            {
                int segIndex = i;
                writerHPixelDef.WriteLine("#define " + m_instanceName + "_SEG" + segIndex + "_PORT" + "                " +
                                          m_instanceName + "_Seg__LCD_SEG_PORT__" + segIndex);
                writerHPixelDef.WriteLine("#define " + m_instanceName + "_SEG" + segIndex + "_PIN" + "                " +
                                          m_instanceName + "_Seg__LCD_SEG_PIN__" + segIndex);
            }
            writerHPixelDef.WriteLine("");

            List<Point> usedComSeg = new List<Point>();
            foreach (HelperInfo hi in HelpersConfig)
            {
                if (hi.Kind == CyHelperKind.Empty) continue;

                foreach (CyHelperSegmentInfo si in hi.HelpSegInfo)
                {
                    // Define only assigned pixels
                    if ((si.Common >= 0) && (si.Segment >= 0))
                    {
                        int segIndex = si.GetIndex(pNumCommonLines);
                        int choseData = si.Common;
                        writerHPixelDef.WriteLine("#define " + si.Name + "                " + m_instanceName +
                                                  "_FIND_PIXEL(" + m_instanceName + "_SEG" + si.Segment + "_PORT,  " +
                                                  m_instanceName + "_SEG" + si.Segment + "_PIN,  " + si.Common + ")");
                        usedComSeg.Add(new Point(si.Common, si.Segment));
                    }
                }              
            }
            writerHPixelDef.WriteLine("");
            //Add pixels of Empty helper that weren't added before
            foreach (CyHelperSegmentInfo si in HelpersConfig[0].HelpSegInfo)
            {
                if (!usedComSeg.Contains(new Point(si.Common, si.Segment)))
                {
                    int segIndex = si.GetIndex(pNumCommonLines);
                    int choseData = si.Common;
                    writerHPixelDef.WriteLine("#define " + si.Name + "                " + m_instanceName +
                                              "_FIND_PIXEL(" + m_instanceName + "_SEG" + si.Segment + "_PORT,  " +
                                              m_instanceName + "_SEG" + si.Segment + "_PIN,  " + si.Common + ")");
                }
            }   

            //Add not connected define
            writerHPixelDef.WriteLine();
            writerHPixelDef.WriteLine("#define NOT_CON            0xFFU");

            paramDict.Add("writerCVariables", writerCVariables.ToString());
            paramDict.Add("writerHPixelDef", writerHPixelDef.ToString());
        }
        #endregion

        #region Core
        public void CollectApiCore(ICyInstQuery_v1 inst, ref List<HelperInfo> HelpersConfig, 
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
                    case CyHelperKind.Segment7:
                        Write7SegDigit_n(ref writerCFunctions, ref writerHFuncDeclarations, p);
                        Write7SegNumber_n(ref writerCFunctions, ref writerHFuncDeclarations, p);
                        break;
                    case CyHelperKind.Segment14:
                        PutChar14seg_n(ref writerCFunctions, ref writerHFuncDeclarations, p);
                        WriteString14seg_n(ref writerCFunctions, ref writerHFuncDeclarations, p);
                        break;
                    case CyHelperKind.Segment16:
                        PutChar16seg_n(ref writerCFunctions, ref writerHFuncDeclarations, p);
                        WriteString16seg_n(ref writerCFunctions, ref writerHFuncDeclarations, p);
                        break;
                    case CyHelperKind.Bar:
                        WriteBargraph_n(ref writerCFunctions, ref writerHFuncDeclarations, p,
                                        HelpersConfig[i].SymbolsCount);
                        break;
                    case CyHelperKind.Empty:

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

        string MassToString(HelperInfo hi)
        {
            string str = "";
            string[,] p = new string[hi.SymbolsCount, hi.SegmentCount];

            foreach (CyHelperSegmentInfo hsi in hi.HelpSegInfo)
            {
                if ((hsi.Segment >= 0) && (hsi.Common >= 0))
                    p[hsi.DigitNum,hsi.RelativePos] = hsi.Name;
                else
                {
                    p[hsi.DigitNum, hsi.RelativePos] = "NOT_CON";
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

        void Write_HeaderMassiv(ref TextWriter writer, HelperInfo hi, int index)
        {
            string str = MassToString(hi);
            if (hi.Kind != CyHelperKind.Bar)
                writer.WriteLine("const uint16 " + m_instanceName + "_Disp" + index + "[" + hi.SymbolsCount + "][" +
                                 hi.SegmentCount + "] = {" + str + "};");
            else
            {
                writer.WriteLine("const uint16 " + m_instanceName + "_Disp" + index + "[" + (hi.SymbolsCount + 1) + "][" 
                                 + hi.SegmentCount + "] = {{0}, " + str + "};");
            }
        }
        #endregion
    }
}
