/*******************************************************************************
* Copyright 2008-2009, Cypress Semiconductor Corporation.  All rights reserved.
* You may use this file only in accordance with the license, terms, conditions, 
* disclaimers, and limitations in the end user license agreement accompanying 
* the software package with which this file was provided.
********************************************************************************/



using System;
using System.Collections.Generic;
using System.Text;
using CyDesigner.Extensions.Common;
using CyDesigner.Extensions.Gde;
using System.IO;

namespace CapSense_v1_10.API
{
    public partial class CyAPIGenerator
    {
        #region Parameters
        string m_instanceName;

        public CyAPIGenerator(string instanceName)
        {
            this.m_instanceName = instanceName;
        }
        //API parameters
        public const string pa_writerCVariables = "writerCVariables";
        public const string pa_writerCFunctions = "writerCFunctions";
        public const string pa_writerCIntFunctions = "writerCIntFunctions";
        public const string pa_ParalelFunctionPrototypes = "ParalelFunctionPrototypes";
        public const string pa_DefineIdacEnable = "DefineIdacEnable";
        public const string pa_Include = "Include";
        public const string pa_FunctionPrototypes = "FunctionPrototypes";
        public const string pa_StructSettingsPrototypes = "StructSettingsPrototypes";
        public const string pa_Define = "Define";

        public const string pa_writerCHLVariables = "writerCHLVariables";
        public const string pa_writerCHLFunctions = "writerCHLFunctions";
        public const string pa_writerCHLFunctionsCentroid = "writerCHLFunctionsCentroid";
        public const string pa_writerCHLFunctionPrototypes = "writerCHLFunctionPrototypes";

        //public const string pa_ = "";

        //function parameters
        public string m_base_CLK = "ClockAuto";
        public string m_baseSW_CLK_UDB = "UDBPrescaler";
        public string m_baseSW_CLK_FF = "FFPrescaler";
        public string m_baseDIG_CLK = "UDBSpeed";
        public string m_csRawCnt = "cRAW";
        public string m_csPRS = "cPRS";
        public string m_csPWM = "cPWM";
        public string m_csComp = "cComp";
        public string m_csAmux = "cAMux";
        public string m_csIdac = "cIDAC";
        public string m_csPort = "cPort";
        public string m_csISR = "cISR";
        public string m_csVref = "cvRef";
        public string m_csBuf = "cBuf";
        public string m_csControl = "cControl";
        public string m_csShield = "cShield";
        public string m_CapS_clock = "CapS_clock";
        public string m_csWorkAround = "cPWMwa";


        #endregion

        #region Service Functions
        string SymbolGenerate(CyGeneralParams packparams, CyAmuxBParams sbItem, E_MAIN_CONFIG conf)
        {
            return packparams.GetPrefixForSchematic(sbItem) + "b" + sbItem.m_Method;
        }
        string GetSideName(E_EL_SIDE side, E_MAIN_CONFIG Configuration)
        {
            if (Configuration == E_MAIN_CONFIG.emSerial) return "";
            string res = "";
            res += side.ToString();
            return res;
        }
        string GetSideNameUpper(E_EL_SIDE side, E_MAIN_CONFIG Configuration)
        {
            if (Configuration == E_MAIN_CONFIG.emSerial) return "";
            string res = "_";
            res += side.ToString().ToUpper();
            return res;
        }

        string Idac_PolarityGenerate(CyAmuxBParams sbItem)
        {
            string strT = m_instanceName + "_IDAC_IDIR_";
            if (sbItem.m_csdSubMethod == E_CSD_SUB_METHODS.IDACSourcing)
            {
                strT += "SRC";
            }
            else if (sbItem.m_csdSubMethod == E_CSD_SUB_METHODS.IDACSinking)
            {
                strT += "SINK";
            }
            return strT;
        }
        string GetSW_CLK(CyGeneralParams packParams, CyAmuxBParams item)
        {
            string res = packParams.GetPrefixForSchematic(item) + m_baseSW_CLK_UDB;
            if (item.m_Prescaler == E_PRESCALER.HW)
            {
                res = packParams.GetPrefixForSchematic(item) + m_baseSW_CLK_FF;
            }

            return res;
        }
        string Get_CLK(CyGeneralParams packParams, CyAmuxBParams item)
        {
            return packParams.GetPrefixForClock(item) + m_base_CLK;
        }
        bool BNotLastItem(object[] arr, object item)
        {
            if ((item != null) && (arr.Length > 0))
            {
                if (arr[arr.Length - 1] == item)
                    return false;
            }
            return true;
        }
        #endregion

        #region CollectApiHFile
        public void CollectApiHFile(ICyInstQuery_v1 query, ICyTerminalQuery_v1 termQuery,
            CyGeneralParams packParams, ref Dictionary<string, string> paramDict)
        {
            TextWriter writer;
            #region ParalelFunctionPrototypes
            writer = new StringWriter();
            writer.NewLine = "\n";
            writer.WriteLine(" void " + m_instanceName + "_Start(void);");
            writer.WriteLine(" void " + m_instanceName + "_Stop(void);");
            if (packParams.m_localParams.IsParallelFull())
            {
                writer.WriteLine(" void " + m_instanceName + "_ScanSlotLeft(uint8 slot);");
                writer.WriteLine(" void " + m_instanceName + "_ScanSlotRight(uint8 slot);");
            }
            else
            {
                writer.WriteLine(" void " + m_instanceName + "_ScanSlot(uint8 slot);");
            }
            writer.WriteLine(" void " + m_instanceName + "_ScanAllSlots(void);");

            #endregion
            paramDict.Add(pa_ParalelFunctionPrototypes, writer.ToString());

            if (packParams.m_localParams.IsIdacInSystem())
            {
                #region DefineIdacEnable
                writer = new StringWriter();
                writer.NewLine = "\n";
                writer.WriteLine("/* CR0 iDac Control Register 0 definitions */  ");
                writer.WriteLine("");
                writer.WriteLine("/* Bit Field  DAC_MODE                  */");
                writer.WriteLine("#define " + m_instanceName + "_IDAC_MODE_MASK      0x10u");
                writer.WriteLine("#define " + m_instanceName + "_IDAC_MODE_V         0x00u");
                writer.WriteLine("#define " + m_instanceName + "_IDAC_MODE_I         0x10u");
                writer.WriteLine("");
                writer.WriteLine("/* CR1 iDac Control Register 1 definitions */");
                writer.WriteLine("");
                writer.WriteLine("/* Bit Field  DAC_I_DIR                  */");
                writer.WriteLine("/* Register control of current direction      */");
                writer.WriteLine("#define " + m_instanceName + "_IDAC_IDIR_MASK      0x04u   ");
                writer.WriteLine("#define " + m_instanceName + "_IDAC_IDIR_SINK      0x04u");
                writer.WriteLine("#define " + m_instanceName + "_IDAC_IDIR_SRC       0x00u");
                writer.WriteLine("");
                writer.WriteLine("/* Bit Field  DAC_MX_IOFF_SRC                  */");
                writer.WriteLine("/* Selects source of IOFF control, reg or UDB  */");
                writer.WriteLine("#define " + m_instanceName + "_IDAC_IDIR_CTL_MASK  0x02u");
                writer.WriteLine("#define " + m_instanceName + "_IDAC_IDIR_CTL_REG   0x00u");
                writer.WriteLine("#define " + m_instanceName + "_IDAC_IDIR_CTL_UDB   0x02u");
                writer.WriteLine("");
                #endregion
                paramDict.Add(pa_DefineIdacEnable, writer.ToString());
            }

            //Parallel Functions
            foreach (CyAmuxBParams sbItem in packParams.m_localParams.m_listCsHalfs)
                if (packParams.m_localParams.BCsHalfIsEnable(sbItem))
                {
                    E_EL_SIDE side = sbItem.m_side;
                    string Symbol = SymbolGenerate(packParams, sbItem, packParams.Configuration);
                    string Method = sbItem.m_Method.ToString();
                    string sSide = GetSideName(side, packParams.Configuration);
                    string sSideUp = GetSideNameUpper(side, packParams.Configuration);
                    string csPRS = "cPRS" + sbItem.GetPRSResolution();

                    string SW_CLK = GetSW_CLK(packParams, sbItem);
                    string CLK = Get_CLK(packParams, sbItem);

                    string DIG_CLK = packParams.GetPrefixForSchematic(sbItem) + m_baseDIG_CLK;

                    #region Include

                    writer = new StringWriter();
                    writer.NewLine = "\n";
                    writer.WriteLine("/*************** " + sSide + " ***************/");
                    if (sbItem.m_currectClk != null)
                        if (sbItem.m_currectClk.IsDirectClock() == false)
                        {
                            if ((sbItem.m_side == E_EL_SIDE.Left) || (packParams.Configuration != E_MAIN_CONFIG.emParallelSynchron))
                                writer.WriteLine("#include \"" + m_instanceName + "_" + CLK + ".h\"");
                        }

                    if (sbItem.IsPrescaler())
                    {
                        writer.WriteLine("#include \"" + m_instanceName + "_" + SW_CLK + ".h\"");
                    }
                    writer.WriteLine("#include \"" + m_instanceName + "_" + DIG_CLK + ".h\"");
                    writer.WriteLine("#include \"" + m_instanceName + "_" + Symbol + "_" + m_csRawCnt + ".h\"");
                    writer.WriteLine("#include \"" + m_instanceName + "_" + Symbol + "_" + m_csPWM + ".h\"");
                    writer.WriteLine("#include \"" + m_instanceName + "_" + Symbol + "_" + m_csComp + ".h\"");
                    writer.WriteLine("#include \"" + m_instanceName + "_" + Symbol + "_" + m_csAmux + ".h\"");
                    writer.WriteLine("#include \"" + m_instanceName + "_" + Symbol + "_" + m_csISR + ".h\"");
                    //writer.WriteLine("#include \"" + m_instanceName + "_" + Symbol + "_" + m_CapS_clock + ".h\"");
                    if (sbItem.IsPRS())
                    {
                        writer.WriteLine("#include \"" + m_instanceName + "_" + Symbol + "_" + csPRS + ".h\"");
                    }

                    if (sbItem.IsWorkAround())
                    {
                        //writer.WriteLine("#include \"" + instanceName + "_" + Symbol + "_" + csWorkAround + ".h\"");
                    }
                    #endregion
                    paramDict.Add(pa_Include + sbItem.m_side, writer.ToString());

                    #region FunctionPrototypes
                    writer = new StringWriter();
                    writer.NewLine = "\n";
                    writer.WriteLine("/*************** " + sSide + " ***************/");
                    writer.WriteLine("void " + m_instanceName + "_" + Method + "_Start" + sSide + "(void);");
                    writer.WriteLine("void " + m_instanceName + "_" + Method + "_Stop" + sSide + "(void);");
                    writer.WriteLine("void " + m_instanceName + "_" + Method + "_ScanSlot" + sSide + "(uint8 slot);");
                    writer.WriteLine("void " + m_instanceName + "_" + Method + "_ScanAllSlots" + sSide + "(void);");
                    writer.WriteLine("void " + m_instanceName + "_" + Method + "_SetSlotSettings" + sSide + "(uint8 slot);");
                    writer.WriteLine("void " + m_instanceName + "_" + Method + "_ClearSlots" + sSide + "(void);");
                    writer.WriteLine("void " + m_instanceName + "_" + Method + "_EnableSensor" + sSide + "(uint8 sensor);");
                    writer.WriteLine("void " + m_instanceName + "_" + Method + "_DisableSensor" + sSide + "(uint8 sensor, uint8 state);");
                    writer.WriteLine("uint16 " + m_instanceName + "_" + Method + "_ReadSlot" + sSide + "(uint8 slot);");
                    if (sbItem.m_isRbEnable)
                        writer.WriteLine("void " + m_instanceName + "_" + Method + "_SetRBleed" + sSide + "(uint8 rbeed);");

                    //if (false)
                    //    writer.WriteLine(""+instanceName+"_PortMask* " + instanceName + "_" + Method + "_GetPortPin" + sSideUp + "(uint8 sensor);");
                    writer.WriteLine("");
                    writer.WriteLine("/* Interrupt handler */");
                    writer.WriteLine("CY_ISR_PROTO(" + m_instanceName + "_ISR" + sSide + ");");
                    writer.WriteLine("");
                    writer.WriteLine("extern uint8 " + m_instanceName + "_status" + sSide + ";");
                    #endregion
                    paramDict.Add(pa_FunctionPrototypes + sbItem.m_side, writer.ToString());

                    #region Define
                    writer = new StringWriter();
                    writer.NewLine = "\n";
                    writer.WriteLine("/*************** " + sSide + " ***************/");
                    int tsens_count = packParams.m_cyWidgetsList.GetListTerminalsFromSide(side).Count;
                    int tscanslot_count = packParams.m_cyScanSlotsList.GetListFromSide(side).Count;
                    if (packParams.IsAmuxBusBlank(sbItem.m_side))
                    {
                        //Fix compile bug with 0 values
                        tsens_count = 1;
                        tscanslot_count = 1;
                    }
                    writer.WriteLine("#define " + m_instanceName + "_TOTAL_SENSOR_COUNT" + sSideUp +
                        "\t" + tsens_count);
                    writer.WriteLine("#define " + m_instanceName + "_TOTAL_SCANSLOT_COUNT" + sSideUp +
                        "\t" + tscanslot_count);
                    writer.WriteLine("#define " + m_instanceName + "_TOTAL_GENERIC_SCANSLOT_COUNT" +
                        sSideUp + "\t" +
                        packParams.m_cyWidgetsList.GetCountWidgetsSameType(E_SENSOR_TYPE.Generic, side));

                    if (sbItem.CountShieldElectrode > 0)
                    {
                        writer.WriteLine("#define " + m_instanceName + "_TOTAL_SHIELD_COUNT" + sSideUp +
                            "\t" + sbItem.CountShieldElectrode);
                    }
                    writer.WriteLine("");
                    writer.WriteLine("/* Define Sensors */");
                    //Define Sensors
                    for (int i = 0; i < packParams.m_cyWidgetsList.GetListTerminalsFromSide(side).Count; i++)
                    {
                        string str = "#define " + m_instanceName + "_SENSOR_" +
                            packParams.m_cyWidgetsList.GetListTerminalsFromSide(side)[i].ToString().ToUpper() +
                            sSideUp + "\t" + i;
                        writer.WriteLine(str);
                    }
                    writer.WriteLine("");
                    writer.WriteLine("/* Define ScanSlots */");
                    //Define ScanSlots
                    for (int i = 0; i < packParams.m_cyScanSlotsList.GetListFromSide(side).Count; i++)
                    {
                        string str = "#define " + m_instanceName + "_SCANSLOT_" +
                            packParams.m_cyScanSlotsList.GetListFromSide(side)[i].GetHeader().ToUpper() +
                            sSideUp + "\t" + i;
                        writer.WriteLine(str);
                    }

                    writer.WriteLine("");
                    writer.WriteLine("/* Chanels of CapSense */");

                    int intSh = 0;
                    if (sbItem.IsShieldElectrode())
                    {
                        intSh = sbItem.CountShieldElectrode;
                    }

                    writer.WriteLine("#define " + m_instanceName + "_CMOD_CHANNEL" + sSideUp + "		" +
                        (packParams.m_cyWidgetsList.GetListTerminalsFromSide(side).Count + intSh).ToString());
                    writer.WriteLine("#define " + m_instanceName + "_CMP_VP_CHANNEL" + sSideUp + "		" +
                        (packParams.m_cyWidgetsList.GetListTerminalsFromSide(side).Count + intSh + 1).ToString());
                    if (sbItem.m_isIdac)
                    {
                        writer.WriteLine("#define " + m_instanceName + "_IDAC_CHANNEL" + sSideUp + "		" +
                            (packParams.m_cyWidgetsList.GetListTerminalsFromSide(side).Count + intSh + 2).ToString());
                    }
                    writer.WriteLine("");
                    writer.WriteLine("/* Control Register */");
                    writer.WriteLine("#define " + m_instanceName + "_CONTROL" + sSideUp + "      		(* (reg8 *) " + m_instanceName + "_" + Symbol + "_" + m_csControl + "_ctrl_reg__CONTROL_REG )");
                    writer.WriteLine("");
                    writer.WriteLine("/* cs Buffer */");
                    writer.WriteLine("#define " + m_instanceName + "_CAPS_CFG0" + sSideUp + "      	(* (reg8 *) " + m_instanceName + "_" + Symbol + "_" + m_csBuf + "__CFG0 )");
                    writer.WriteLine("#define " + m_instanceName + "_CAPS_CFG1" + sSideUp + "      	(* (reg8 *) " + m_instanceName + "_" + Symbol + "_" + m_csBuf + "__CFG1 )");
                    writer.WriteLine("#define " + m_instanceName + "_CSBUF_PWRMGR" + sSideUp + "      	(* (reg8 *) " + m_instanceName + "_" + Symbol + "_" + m_csBuf + "__PM_ACT_CFG )");
                    writer.WriteLine("#define " + m_instanceName + "_CSBUF_PWR_ENABLE" + sSideUp + "      	" + m_instanceName + "_" + Symbol + "_" + m_csBuf + "__PM_ACT_MSK");

                    writer.WriteLine("");
                    if (sbItem.m_isIdac)
                    {
                        writer.WriteLine("/* csIdac */");
                        writer.WriteLine("#define " + m_instanceName + "_IDAC_CR0" + sSideUp + "    		(* (reg8 *) " + m_instanceName + "_" + Symbol + "_" + m_csIdac + "__CR0 )");
                        writer.WriteLine("#define " + m_instanceName + "_IDAC_CR1" + sSideUp + "    		(* (reg8 *) " + m_instanceName + "_" + Symbol + "_" + m_csIdac + "__CR1 )");
                        writer.WriteLine("#define " + m_instanceName + "_IDAC_DATA" + sSideUp + "   		(* (reg8 *) " + m_instanceName + "_" + Symbol + "_" + m_csIdac + "__D )");
                        writer.WriteLine("#define " + m_instanceName + "_IDAC_STROBE" + sSideUp + " 		(* (reg8 *) " + m_instanceName + "_" + Symbol + "_" + m_csIdac + "__STROBE )");
                        writer.WriteLine("#define " + m_instanceName + "_IDAC_TR" + sSideUp + "            (* (reg8 *) " + m_instanceName + "_" + Symbol + "_" + m_csIdac + "__TR )");
                        writer.WriteLine("#define " + m_instanceName + "_IDAC_PWRMGR" + sSideUp + " 		(* (reg8 *) " + m_instanceName + "_" + Symbol + "_" + m_csIdac + "__PM_ACT_CFG )");

                        writer.WriteLine("");
                        writer.WriteLine("/* TODO: Remove this line after changes to DAC */");
                        writer.WriteLine("#define " + m_instanceName + "_IDAC_FIRST_SILICON" + sSideUp);
                        writer.WriteLine("");
                        writer.WriteLine("/* PM_ACT_CFG (Active Power Mode CFG Register)     */ ");
                        writer.WriteLine("#if !defined(" + m_instanceName + "_IDAC_FIRST_SILICON" + sSideUp + ")");
                        writer.WriteLine("#define " + m_instanceName + "_IDAC_ACT_PWR_EN" + sSideUp + "   " + m_instanceName + "_" + Symbol + "_" + m_csIdac + "__PM_ACT_MSK");
                        writer.WriteLine("#else");
                        writer.WriteLine("#define " + m_instanceName + "_IDAC_ACT_PWR_EN" + sSideUp + "   0xFF /* TODO: Work around for incorrect power enable */");
                        writer.WriteLine("#endif");
                        writer.WriteLine("");
                        writer.WriteLine("#define " + m_instanceName + "_DAC_POS" + sSideUp + "  (" + m_instanceName + "_" + Symbol + "_" + m_csIdac + "__D - CYDEV_ANAIF_WRK_DAC0_BASE)");
                        writer.WriteLine("");
                        writer.WriteLine("#if(" + m_instanceName + "_DAC_POS" + sSideUp + " == 0)");
                        writer.WriteLine("#define " + m_instanceName + "_DAC_TRIM_BASE" + sSideUp + "  0x0C011Cu");
                        writer.WriteLine("#endif");
                        writer.WriteLine("");
                        writer.WriteLine("#if(" + m_instanceName + "_DAC_POS" + sSideUp + " == 1)");
                        writer.WriteLine("#define " + m_instanceName + "_DAC_TRIM_BASE" + sSideUp + "  0x0C012Cu");
                        writer.WriteLine("#endif");
                        writer.WriteLine("");
                        writer.WriteLine("#if(" + m_instanceName + "_DAC_POS" + sSideUp + " == 2)");
                        writer.WriteLine("#define " + m_instanceName + "_DAC_TRIM_BASE" + sSideUp + "  0x0C0124u");
                        writer.WriteLine("#endif");
                        writer.WriteLine("");
                        writer.WriteLine("#if(" + m_instanceName + "_DAC_POS" + sSideUp + " == 3)");
                        writer.WriteLine("#define " + m_instanceName + "_TRIM_BASE" + sSideUp + "  0x0C0134u");
                        writer.WriteLine(" #endif");
                        writer.WriteLine("");
                    }
                    #endregion


                    paramDict.Add(pa_Define + sbItem.m_side, writer.ToString());


                    #region CHLFunctionPrototypes
                    writer = new StringWriter();
                    writer.WriteLine("void " + m_instanceName + "_CSHL_InitializeSlotBaseline" + sSide + "(uint8 slot);");
                    writer.WriteLine("void " + m_instanceName + "_CSHL_InitializeAllBaselines" + sSide + "(void);");
                    writer.WriteLine("void " + m_instanceName + "_CSHL_UpdateSlotBaseline" + sSide + "(uint8 slot);");
                    writer.WriteLine("void " + m_instanceName + "_CSHL_UpdateAllBaselines" + sSide + "(void);");
                    writer.WriteLine("uint8 " + m_instanceName + "_CSHL_CheckIsSlotActive" + sSide + "(uint8 slot);");
                    writer.WriteLine("uint8 " + m_instanceName + "_CSHL_CheckIsAnySlotActive" + sSide + "(void);");
                    writer.NewLine = "\n";
                    #endregion
                    paramDict.Add(pa_writerCHLFunctionPrototypes + sbItem.m_side, writer.ToString());

                    //#region
                    //writer = new StringWriter();
                    //writer.NewLine = "\n";

                    //#endregion
                }

            writer = new StringWriter();
            writer.NewLine = "\n";
            if (packParams.Configuration != E_MAIN_CONFIG.emSerial)
            {
                writer.WriteLine("/* ScanSlots Count */");
                int tscanslot_count = packParams.m_cyScanSlotsList.GetScanSlotCount();
                //Fix error during blank parameters
                if (tscanslot_count == 0) tscanslot_count = 1;

                writer.WriteLine("#define " + m_instanceName + "_TOTAL_SCANSLOT_COUNT" + "\t" + tscanslot_count);
                writer.WriteLine("#define " + m_instanceName + "_TOTAL_GENERIC_SCANSLOT_COUNT" + "\t" + packParams.m_cyWidgetsList.GetCountWidgetsSameType(E_SENSOR_TYPE.Generic));
            }
            paramDict.Add(pa_Define, writer.ToString());


            #region writerCHLFunctionPrototypes
            writer = new StringWriter();
            writer.NewLine = "\n";
            if (packParams.Configuration != E_MAIN_CONFIG.emSerial)
            {
                //writer.WriteLine("#define " + instanceName + "_TOTAL_SCANSLOT_COUNT" + "\t" + packParams.cyScanSlotsList.GetScanSlotCount());

                writer.WriteLine("void " + m_instanceName + "_CSHL_InitializeAllBaselines(void);");
                writer.WriteLine("uint8 " + m_instanceName + "_CSHL_CheckIsAnySlotActive(void);");
                writer.WriteLine("void " + m_instanceName + "_CSHL_UpdateAllBaselines(void);");
                writer.NewLine = "\n";
            }

            writer.WriteLine("");
            if (packParams.m_cyWidgetsList.GetCountWidgetsSameType(E_SENSOR_TYPE.Linear_Slider) > 0)
                writer.WriteLine("uint16 " + m_instanceName + "_CSHL_GetCentroidPos(uint8 widget);");
            if (packParams.m_cyWidgetsList.GetCountWidgetsSameType(E_SENSOR_TYPE.Radial_Slider) > 0)
                writer.WriteLine("uint16 " + m_instanceName + "_CSHL_GetRadialCentroidPos(uint8 widget);");
            if (packParams.m_cyWidgetsList.GetCountDoubleFullWidgets() > 0)
                writer.WriteLine("uint16 " + m_instanceName + "_CSHL_GetDoubleCentroidPos(uint8 widget);");

            writer.WriteLine("");
            writer.WriteLine("#define " + m_instanceName + "_CSHL_TOTAL_WIDGET_COUNT" + "\t" + packParams.m_cyWidgetsList.GetCountWidgetsHL());
            writer.WriteLine("#define " + m_instanceName + "_CSHL_NUMBER_OF_DOUBLE_STRUCTS\t" + packParams.m_cyWidgetsList.GetCountDoubleFullWidgets());

            writer.WriteLine("");

            foreach (CyElWidget wi in packParams.m_cyWidgetsList.GetListWithFullWidgetsHL())
            {
                if (CySensorType.IsTouchPad(wi.m_type))
                //Add Heder Widget
                {
                    CyElWidget part = packParams.m_cyWidgetsList.GetBothParts(wi)[0];
                    string str = "#define " + m_instanceName + "_CSHL_" + ((CyElUnTouchPad)part).ToHeaderString().ToUpper() + "\t" + packParams.m_cyWidgetsList.GetIndexInListWithFullWidgetsHL(part);
                    writer.WriteLine(str);
                }

                for (int i = 0; i < packParams.m_cyWidgetsList.GetBothParts(wi).Count; i++)
                {
                    CyElWidget part = packParams.m_cyWidgetsList.GetBothParts(wi)[i];
                    string str = "#define " + m_instanceName + "_CSHL_" + part.ToString().ToUpper() + "\t" + packParams.m_cyWidgetsList.GetIndexInListWithFullWidgetsHL(part);
                    writer.WriteLine(str);
                }

            }

            #endregion
            paramDict.Add(pa_writerCHLFunctionPrototypes, writer.ToString());



            #region StructSettingsPrototypes
            writer = new StringWriter();
            writer.NewLine = "\n";
            foreach (CyAmuxBParams sbItem in packParams.m_localParams.m_listCsHalfs)
                if (packParams.m_localParams.BCsHalfIsEnable(sbItem))
                {
                    E_EL_SIDE side = sbItem.m_side;
                    string Method = sbItem.m_Method.ToString();
                    string sSide = GetSideName(side, packParams.Configuration);
                    writer.WriteLine("/* Settings for " + Method + " " + sSide + " */");
                    writer.WriteLine("typedef struct _" + m_instanceName + "_" + Method + "_Settings" + sSide + "");
                    writer.WriteLine("{");
                    if (sbItem.m_isIdac)
                    {
                        writer.WriteLine("	uint8 IdacRange;				/* 1-3 range exist, multiplier for IDAC */");
                        writer.WriteLine("	uint8 IdacSettings;				/* IDAC settings for measurament */");
                    }
                    if (sbItem.m_Method == E_CAPSENSE_MODE.CSA)
                    {
                        writer.WriteLine("	uint8 ScanLength; 				/* IDAC measure = SA - ScanLength */");
                        writer.WriteLine("	uint8 SettlingTime;				/* SettlingTime */");
                        writer.WriteLine("	uint8 IdacCode;					/* IdacCode */");
                    }
                    if (sbItem.IsPrescaler())
                    {
                        writer.WriteLine("	uint8 PrescalerPeriod;			/* Prescaler for SW_CLK */");
                    }
                    if (sbItem.m_Method == E_CAPSENSE_MODE.CSD)
                    {
                        writer.WriteLine("	uint8 Resolution;				/* PWM Resolution */");
                        writer.WriteLine("	uint8 ScanSpeed;				/* Speed of scanning  DIG_CLK */");
                        if (sbItem.IsPRS())
                        {
                            writer.WriteLine("	uint16 PrsPolynomial;			/* Prs signal polinomial to \"Break befor Make\" */");
                        }
                    }
                    writer.WriteLine("	uint8 DisableState ;			/* Disable state of ScanSlot */");
                    writer.WriteLine("");
                    writer.WriteLine("} " + m_instanceName + "_" + Method + "_Settings" + sSide + ";");
                    writer.WriteLine("");
                    writer.WriteLine("");
                }
            #endregion

            paramDict.Add(pa_StructSettingsPrototypes, writer.ToString());
        }
        #endregion

    }
}
