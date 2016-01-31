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

namespace  CapSense_v0_5.API
{
    public partial class M_APIGenerator
    {
        #region Parameters
        string instanceName;

        public M_APIGenerator(string instanceName)
        {
            this.instanceName = instanceName;
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
        public string base_CLK = "Clock";
        public string baseSW_CLK_UDB = "UDBPrescaler";
        public string baseSW_CLK_FF = "FFPrescaler";
        public string baseDIG_CLK = "UDBSpeed";
        public string csRawCnt = "cRAW";
        //public string csPRS = "cPRS";
        public string csPWM = "cPWM";
        public string csComp = "cComp";
        public string csAmux = "cAMux";
        public string csIdac = "cIDAC";
        public string csPort = "cPort";
        public string csISR = "cISR";
        public string csVref = "cvRef";
        public string csBuf = "cBuf";
        public string csControl = "cControl";
        public string csShield = "cShield";
		public string CapS_clock = "CapS_clock";
        public string csWorkAround = "cPWMwa";
        

        #endregion

        #region Service Functions
        string SymbolGenerate(CyGeneralParams packparams, CyAmuxBParams sbParametr, eMConfiguration conf)
        {
            return packparams.GetPrefixForSchematic(sbParametr) + "b" + sbParametr.Method;
        }
        string GetSideName(eElSide side, eMConfiguration Configuration)
        {
            if (Configuration == eMConfiguration.emSerial) return "";
            string res = "";
            res += side.ToString();
            return res;
        }
        string GetSideNameUpper(eElSide side, eMConfiguration Configuration)
        {
            if (Configuration == eMConfiguration.emSerial) return "";
            string res = "_";
            res += side.ToString().ToUpper();
            return res;
        }

        string Idac_PolarityGenerate(CyAmuxBParams sbParametr)
        {
            string strT = instanceName + "_IDAC_IDIR_";
            if (sbParametr.csdSubMethod == CSDSubMethods.IDACSourcing)
            {
                strT += "SRC";
            }
            else if (sbParametr.csdSubMethod == CSDSubMethods.IDACSinking)
            {
                strT += "SINK";
            }
            return strT;
        }
        string GetSW_CLK(CyGeneralParams packParams, CyAmuxBParams item)
        {
            string res = packParams.GetPrefixForSchematic(item) + baseSW_CLK_UDB;
            if (item.Prescaler == enPrescaler.HW)
            {
                res = packParams.GetPrefixForSchematic(item) + baseSW_CLK_FF;
            }

            return res;
        }
        string Get_CLK(CyGeneralParams packParams, CyAmuxBParams item)
        {
            return packParams.GetPrefixForClock(item) + base_CLK;
        }
        bool bNotLastItem(object[] arr, object item)
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
        public void CollectApiHFile(ICyInstQuery_v1 query, ICyTerminalQuery_v1 termQuery, CyGeneralParams packParams, ref Dictionary<string, string> paramDict)
        {
            TextWriter writer;
            #region ParalelFunctionPrototypes
            writer = new StringWriter();
            writer.NewLine = "\n";
            writer.WriteLine(" void " + instanceName + "_Start(void);");
            writer.WriteLine(" void " + instanceName + "_Stop(void);");
            writer.WriteLine(" void " + instanceName + "_ScanAllSlots(void);");
            #endregion
            paramDict.Add(pa_ParalelFunctionPrototypes, writer.ToString());

            if (packParams.localParams.isIdacInSystem())
            {
                #region DefineIdacEnable
                writer = new StringWriter();
                writer.NewLine = "\n";
                writer.WriteLine("/* CR0 iDac Control Register 0 definitions */  ");
                writer.WriteLine("");
                writer.WriteLine("/* Bit Field  DAC_MODE                  */");
                writer.WriteLine("#define " + instanceName + "_IDAC_MODE_MASK      0x10u");
                writer.WriteLine("#define " + instanceName + "_IDAC_MODE_V         0x00u");
                writer.WriteLine("#define " + instanceName + "_IDAC_MODE_I         0x10u");
                writer.WriteLine("");
                writer.WriteLine("/* CR1 iDac Control Register 1 definitions */");
                writer.WriteLine("");
                writer.WriteLine("/* Bit Field  DAC_I_DIR                  */");
                writer.WriteLine("/* Register control of current direction      */");
                writer.WriteLine("#define " + instanceName + "_IDAC_IDIR_MASK      0x04u   ");
                writer.WriteLine("#define " + instanceName + "_IDAC_IDIR_SINK      0x04u");
                writer.WriteLine("#define " + instanceName + "_IDAC_IDIR_SRC       0x00u");
                writer.WriteLine("");
                writer.WriteLine("/* Bit Field  DAC_MX_IOFF_SRC                  */");
                writer.WriteLine("/* Selects source of IOFF control, reg or UDB  */");
                writer.WriteLine("#define " + instanceName + "_IDAC_IDIR_CTL_MASK  0x02u");
                writer.WriteLine("#define " + instanceName + "_IDAC_IDIR_CTL_REG   0x00u");
                writer.WriteLine("#define " + instanceName + "_IDAC_IDIR_CTL_UDB   0x02u");
                writer.WriteLine("");                
                #endregion
                paramDict.Add(pa_DefineIdacEnable, writer.ToString());
            }

            //Parallel Functions
            foreach (CyAmuxBParams item in packParams.localParams.listCsHalfs)
                if (packParams.localParams.bCsHalfIsEnable(item))
                {

                    eElSide side = item.side;
                    string Symbol = SymbolGenerate(packParams, item, packParams.Configuration);
                    string Method = item.Method.ToString();
                    string strSideName = GetSideName(side, packParams.Configuration);
                    string strSideNameUpper = GetSideNameUpper(side, packParams.Configuration);
                    string csPRS = "cPRS" + item.GetPRSResolution();

                    string SW_CLK = GetSW_CLK(packParams, item);
                    string CLK = Get_CLK(packParams, item);

                    string DIG_CLK = packParams.GetPrefixForSchematic(item) + baseDIG_CLK;

                    #region Include

                    writer = new StringWriter();
                    writer.NewLine = "\n";
                    writer.WriteLine("/* ***********" + strSideName + "*********** */");

                    if ((item.side == eElSide.Left) || (packParams.Configuration != eMConfiguration.emParallelSynchron))
                        writer.WriteLine("#include \"" + instanceName + "_" + CLK + ".h\"");

                    if (item.IsPrescaler())
                    {
                        writer.WriteLine("#include \"" + instanceName + "_" + SW_CLK + ".h\"");
                    }
                    writer.WriteLine("#include \"" + instanceName + "_" + DIG_CLK + ".h\"");
                    writer.WriteLine("#include \"" + instanceName + "_" + Symbol + "_" + csRawCnt + ".h\"");
                    writer.WriteLine("#include \"" + instanceName + "_" + Symbol + "_" + csPWM + ".h\"");
                    writer.WriteLine("#include \"" + instanceName + "_" + Symbol + "_" + csComp + ".h\"");
                    writer.WriteLine("#include \"" + instanceName + "_" + Symbol + "_" + csAmux + ".h\"");
                    writer.WriteLine("#include \"" + instanceName + "_" + Symbol + "_" + csISR + ".h\"");
                    writer.WriteLine("#include \"" + instanceName + "_" + Symbol + "_" + CapS_clock + ".h\"");
                    if (item.IsPRS())
                    {
                        writer.WriteLine("#include \"" + instanceName + "_" + Symbol + "_" + csPRS + ".h\"");
                    }

                    if (item.IsWorkAround())
                    {
                        //writer.WriteLine("#include \"" + instanceName + "_" + Symbol + "_" + csWorkAround + ".h\"");
                    }
                    #endregion
                    paramDict.Add(pa_Include + item.side, writer.ToString());

                    #region FunctionPrototypes
                    writer = new StringWriter();
                    writer.NewLine = "\n";
                    writer.WriteLine("/* ***********" + strSideName + "*********** */");
                    writer.WriteLine("void " + instanceName + "_" + Method + "_Start" + strSideName + "(void);");
                    writer.WriteLine("void " + instanceName + "_" + Method + "_Stop" + strSideName + "(void);");
                    writer.WriteLine("void " + instanceName + "_" + Method + "_ScanSlot" + strSideName + "(uint8 slot);");
                    writer.WriteLine("void " + instanceName + "_" + Method + "_ScanAllSlots" + strSideName + "(void);");
                    writer.WriteLine("void " + instanceName + "_" + Method + "_SetSlotSettings" + strSideName + "(uint8 slot);");
                    //writer.WriteLine("void " + instanceName + "_" + Method + "_SetRBleedSettings" + strSideName + "(uint8 rbleed);");
                    writer.WriteLine("void " + instanceName + "_" + Method + "_ClearSlots" + strSideName + "(void);");
                    writer.WriteLine("void " + instanceName + "_" + Method + "_EnableSensor" + strSideName + "(uint8 sensor);");
                    writer.WriteLine("void " + instanceName + "_" + Method + "_DisableSensor" + strSideName + "(uint8 sensor);");
                    if (item.isRbEnable)
                        writer.WriteLine("void " + instanceName + "_" + Method + "_SetRb" + strSideName + "(uint8 rbeed);");
                    writer.WriteLine("uint16 " + instanceName + "_" + Method + "_ReadSlot" + strSideName + "(uint8 slot);");
                    //if (false)
                    //    writer.WriteLine(""+instanceName+"_PortMask* " + instanceName + "_" + Method + "_GetPortPin" + strSideName + "(uint8 sensor);");
                    writer.WriteLine("");
                    writer.WriteLine("/* Interrupt handler */");
                    writer.WriteLine("CY_ISR_PROTO(" + instanceName + "_ISR" + strSideName + ");");
                    writer.WriteLine("");
                    writer.WriteLine("extern uint8 " + instanceName + "_Status" + strSideName + ";");
                    #endregion
                    paramDict.Add(pa_FunctionPrototypes + item.side, writer.ToString());

                    #region Define
                    writer = new StringWriter();
                    writer.NewLine = "\n";
                    writer.WriteLine("/* ***********" + strSideName + "*********** */");
                    writer.WriteLine("#define " + instanceName + "_TOTAL_SENSOR_COUNT" + strSideNameUpper + "\t" + packParams.cyWidgetsList.GetListTerminalsFromSide(side).Count);
                    writer.WriteLine("#define " + instanceName + "_TOTAL_SCANSLOT_COUNT" + strSideNameUpper + "\t" + packParams.cyScanSlotsList.GetListFromSide(side).Count);
                    writer.WriteLine("#define " + instanceName + "_TOTAL_GENERIC_SCANSLOT_COUNT" + strSideNameUpper + "\t" + packParams.cyWidgetsList.GetCountWidgetsSameType(sensorType.Generic, side));

                    if (item.countShieldElectrode > 0)
                    {
                        writer.WriteLine("#define " + instanceName + "_TOTAL_SHIELD_COUNT" + strSideNameUpper + "\t" + item.countShieldElectrode);
                    }
                    writer.WriteLine("");
                    writer.WriteLine("/* Define Sensors */");
                    //Define Sensors
                    for (int i = 0; i < packParams.cyWidgetsList.GetListTerminalsFromSide(side).Count; i++)
                    {
                        string str = "#define " + instanceName + "_SENSOR_" + packParams.cyWidgetsList.GetListTerminalsFromSide(side)[i].ToString().ToUpper() + strSideNameUpper + "\t" + i;
                        writer.WriteLine(str);
                    }
                    writer.WriteLine("");
                    writer.WriteLine("/* Define ScanSlots */");
                    //Define ScanSlots
                    for (int i = 0; i < packParams.cyScanSlotsList.GetListFromSide(side).Count; i++)
                    {
                        string str = "#define " + instanceName + "_SCANSLOT_" + packParams.cyScanSlotsList.GetListFromSide(side)[i].GetHeader().ToUpper() + strSideNameUpper + "\t" + i;
                        writer.WriteLine(str);
                    }

                    writer.WriteLine("");
                    writer.WriteLine("/* Chanels of CapSense */");

                    int intSh = 0;
                    if (item.IsShieldElectrode())
                    {
                        intSh = item.countShieldElectrode - 1;
                    }

                    writer.WriteLine("#define " + instanceName + "_CMOD_CHANEL" + strSideNameUpper + "		" + (packParams.cyWidgetsList.GetListTerminalsFromSide(side).Count + intSh).ToString());
                    writer.WriteLine("#define " + instanceName + "_VREF_CHANEL" + strSideNameUpper + "		" + (packParams.cyWidgetsList.GetListTerminalsFromSide(side).Count + intSh + 1).ToString());
                    if (item.isIdac)
                    {
                        writer.WriteLine("#define " + instanceName + "_IDAC_CHANEL" + strSideNameUpper + "		" + (packParams.cyWidgetsList.GetListTerminalsFromSide(side).Count + intSh + 2).ToString());
                    }
                    writer.WriteLine("");
                    writer.WriteLine("/* Control Register */");
                    writer.WriteLine("#define " + instanceName + "_CONTROL" + strSideNameUpper + "      		(* (reg8 *) " + instanceName + "_" + Symbol + "_" + csControl + "_ctrl_reg__CONTROL_REG )");
                    writer.WriteLine("");
                    writer.WriteLine("/* cs Buffer */");
                    writer.WriteLine("#define " + instanceName + "_CAPS_CFG0" + strSideNameUpper + "      	(* (reg8 *) " + instanceName + "_" + Symbol + "_" + csBuf + "__CFG0 )");
                    writer.WriteLine("#define " + instanceName + "_CAPS_CFG1" + strSideNameUpper + "      	(* (reg8 *) " + instanceName + "_" + Symbol + "_" + csBuf + "__CFG1 )");
                    writer.WriteLine("#define " + instanceName + "_CSBUF_PWRMGR" + strSideNameUpper + "      	(* (reg8 *) " + instanceName + "_" + Symbol + "_" + csBuf + "__PM_ACT_CFG )");
                    writer.WriteLine("#define " + instanceName + "_CSBUF_ENABLE" + strSideNameUpper + "      	" + instanceName + "_" + Symbol + "_" + csBuf + "__PM_ACT_MSK");

                    writer.WriteLine("");
                    writer.WriteLine("/* csVREF */");
                    writer.WriteLine("/* #define " + instanceName + "_VREF_PWRMGR" + strSideNameUpper + "   	(* (reg8 *) " + instanceName + "_" + Symbol + "_" + csVref + "__PM_ACT_CFG )*/");
                    writer.WriteLine("/* #define " + instanceName + "_VREF_ENABLE" + strSideNameUpper + "   	" + instanceName + "_" + Symbol + "_" + csVref + "__PM_ACT_MSK */");
                    writer.WriteLine("");
                    if (item.isIdac)
                    {
                        writer.WriteLine("/* csIdac */");
                        writer.WriteLine("#define " + instanceName + "_IDAC_CR0" + strSideNameUpper + "    		(* (reg8 *) " + instanceName + "_" + Symbol + "_" + csIdac + "__CR0 )");
                        writer.WriteLine("#define " + instanceName + "_IDAC_CR1" + strSideNameUpper + "    		(* (reg8 *) " + instanceName + "_" + Symbol + "_" + csIdac + "__CR1 )");
                        writer.WriteLine("#define " + instanceName + "_IDAC_DATA" + strSideNameUpper + "   		(* (reg8 *) " + instanceName + "_" + Symbol + "_" + csIdac + "__D )");
                        writer.WriteLine("#define " + instanceName + "_IDAC_STROBE" + strSideNameUpper + " 		(* (reg8 *) " + instanceName + "_" + Symbol + "_" + csIdac + "__STROBE )");
                        writer.WriteLine("#define " + instanceName + "_IDAC_PWRMGR" + strSideNameUpper + " 		(* (reg8 *) " + instanceName + "_" + Symbol + "_" + csIdac + "__PM_ACT_CFG )");
                        writer.WriteLine("/* TODO: Remove this line after changes to DAC */");
                        writer.WriteLine("#define " + instanceName + "_IDAC_FIRST_SILICON" + strSideNameUpper);
                        writer.WriteLine("");
                        writer.WriteLine("/* PM_ACT_CFG (Active Power Mode CFG Register)     */ ");
                        writer.WriteLine("#if !defined(" + instanceName + "_IDAC_FIRST_SILICON" + strSideNameUpper + ")");
                        writer.WriteLine("#define " + instanceName + "_IDAC_ACT_PWR_EN" + strSideNameUpper + "   " + instanceName + "_" + Symbol + "_" + csIdac + "__PM_ACT_MSK");
                        writer.WriteLine("#else");
                        writer.WriteLine("#define " + instanceName + "_IDAC_ACT_PWR_EN" + strSideNameUpper + "   0xFF /* TODO: Work around for incorrect power enable */");
                        writer.WriteLine("#endif");
                        writer.WriteLine("");
                    }
                    writer.WriteLine("");
                    writer.WriteLine("/* Mask of the " + instanceName + "_isr interrupt. */");
                    writer.WriteLine("#define " + instanceName + "_INTC_PRIORITY" + strSideNameUpper + " 		" + instanceName + "_" + Symbol + "_" + "cISR__INTC_PRIOR_NUM");
                    #endregion

                    //if (packParams.Configuration != eMConfiguration.emSerial)
                        paramDict.Add(pa_Define + item.side, writer.ToString());

                    if (packParams.cyWidgetsList.GetListWithFullWidgetsHL().Count > 0)
                    {
                        #region CHLFunctionPrototypes
                        writer = new StringWriter();
                        writer.WriteLine("void " + instanceName + "_CSHL_InitializeSlotBaseline" + strSideName + "(uint8 slot);");
                        writer.WriteLine("void " + instanceName + "_CSHL_InitializeAllBaselines" + strSideName + "(void);");
                        writer.WriteLine("void " + instanceName + "_CSHL_UpdateSlotBaseline" + strSideName + "(uint8 slot);");
                        writer.WriteLine("void " + instanceName + "_CSHL_UpdateAllBaselines" + strSideName + "(void);");
                        writer.WriteLine("uint8 " + instanceName + "_CSHL_CheckIsSlotActive" + strSideName + "(uint8 slot);");
                        writer.WriteLine("uint8 " + instanceName + "_CSHL_CheckIsAnySlotActive" + strSideName + "(void);");
                        writer.NewLine = "\n";
                        #endregion
                        paramDict.Add(pa_writerCHLFunctionPrototypes + item.side, writer.ToString());
                    }
                    //#region
                    //writer = new StringWriter();
                    //writer.NewLine = "\n";

                    //#endregion
                }
			         
			writer = new StringWriter();
            writer.NewLine = "\n";
            if (packParams.Configuration != eMConfiguration.emSerial)
            {
                writer.WriteLine("/* ScanSlots Count */");
                writer.WriteLine("#define " + instanceName + "_TOTAL_SCANSLOT_COUNT" + "\t" + packParams.cyScanSlotsList.GetScanSlotCount());
                writer.WriteLine("#define " + instanceName + "_TOTAL_GENERIC_SCANSLOT_COUNT" + "\t" + packParams.cyWidgetsList.GetCountWidgetsSameType(sensorType.Generic));
            }
            paramDict.Add(pa_Define, writer.ToString());

            if (packParams.cyWidgetsList.GetListWithFullWidgetsHL().Count > 0)
            {
                #region writerCHLFunctionPrototypes
                writer = new StringWriter();
                writer.NewLine = "\n";
                if (packParams.Configuration != eMConfiguration.emSerial)
                {
                    //writer.WriteLine("#define " + instanceName + "_TOTAL_SCANSLOT_COUNT" + "\t" + packParams.cyScanSlotsList.GetScanSlotCount());

                    writer.WriteLine("void " + instanceName + "_CSHL_InitializeAllBaselines(void);");
                    writer.WriteLine("uint8 " + instanceName + "_CSHL_CheckIsAnySlotActive(void);");
                    writer.WriteLine("void " + instanceName + "_CSHL_UpdateAllBaselines(void);");
                    writer.NewLine = "\n";
                }

                writer.WriteLine("");
                if (packParams.cyWidgetsList.GetCountWidgetsSameType(sensorType.Linear_Slider) > 0)
                    writer.WriteLine("uint16 " + instanceName + "_CSHL_GetCentroidPos(uint8 widget);");
                if (packParams.cyWidgetsList.GetCountWidgetsSameType(sensorType.Radial_Slider) > 0)
                    writer.WriteLine("uint16 " + instanceName + "_CSHL_GetRadialCentroidPos(uint8 widget);");
                if (packParams.cyWidgetsList.GetCountDoubleFullWidgets() > 0)
                    writer.WriteLine("uint16 " + instanceName + "_CSHL_GetDoubleCentroidPos(uint8 widget);");

                writer.WriteLine("");
                writer.WriteLine("#define " + instanceName + "_CSHL_TOTAL_WIDGET_COUNT" + "\t" + packParams.cyWidgetsList.GetCountWidgetsHL());
                writer.WriteLine("#define " + instanceName + "_CSHL_NUMBER_OF_DOUBLE_STRUCTS\t" + packParams.cyWidgetsList.GetCountDoubleFullWidgets());                

                writer.WriteLine("");
                
                foreach (ElWidget wi in packParams.cyWidgetsList.GetListWithFullWidgetsHL())
                {
                    if(cySensorType.IsTouchPad(wi.type))
                        //Add Heder Widget
                    {
                        ElWidget part = packParams.cyWidgetsList.GetBothParts(wi)[0];
                        string str = "#define " + instanceName + "_CSHL_" + ((ElUnTouchPad)part).ToHeaderString().ToUpper() + "\t" + packParams.cyWidgetsList.GetIndexInListWithFullWidgetsHL(part);
                        writer.WriteLine(str);
                    }

                    for (int i = 0; i < packParams.cyWidgetsList.GetBothParts(wi).Count; i++)
                    {
                        ElWidget part = packParams.cyWidgetsList.GetBothParts(wi)[i];
                        string str= "#define " + instanceName + "_CSHL_" + part.ToString().ToUpper() + "\t" + packParams.cyWidgetsList.GetIndexInListWithFullWidgetsHL(part);
                        writer.WriteLine(str);
                    }
                    
                }

                #endregion
                paramDict.Add(pa_writerCHLFunctionPrototypes, writer.ToString());
            }
            

            #region StructSettingsPrototypes
            writer = new StringWriter();
            writer.NewLine = "\n";
            foreach (CyAmuxBParams item in packParams.localParams.listCsHalfs)
                if (packParams.localParams.bCsHalfIsEnable(item))
                {
                    eElSide side = item.side;
                    string Method = item.Method.ToString();
                    string strSideName = GetSideName(side, packParams.Configuration);
                    writer.WriteLine("/* Settings for " + Method + " " + strSideName + " */");
                    writer.WriteLine("typedef struct _" + instanceName + "_" + Method + "_Settings" + strSideName + "");
                    writer.WriteLine("{");
                    if (item.isIdac)
                    {
                        writer.WriteLine("	uint8 IdacRange;				/* 1-3 range exist, multiplier for IDAC */");
                        writer.WriteLine("	uint8 IdacSettings;				/* IDAC settings for measurament */");
                    }
                    if (item.Method == eCapSenseMode.CSA)
                    {
                        writer.WriteLine("	uint8 ScanLength; 				/* IDAC measure = SA - ScanLength */");
                        writer.WriteLine("	uint8 SettlingTime;				/* SettlingTime */");
                        writer.WriteLine("	uint8 IdacCode;					/* IdacCode */");
                    }
                    if (item.IsPrescaler())
                    {
                        writer.WriteLine("	uint8 PrescalerPeriod;			/* Prescaler for SW_CLK */");
                    }
                    if (item.Method == eCapSenseMode.CSD)
                    {
                        writer.WriteLine("	uint8 Resolution;				/* PWM Resolution */");
                        writer.WriteLine("	uint8 ScanSpeed;				/* Speed of scanning  DIG_CLK */");
                        if (item.IsPRS())
                        {
                            writer.WriteLine("	uint16 PrsPolynomial;			/* Prs signal polinomial to \"Break befor Make\" */");
                        }
                    }
                    writer.WriteLine("");
                    writer.WriteLine("} " + instanceName+"_"+Method + "_Settings" + strSideName + ";");
                    writer.WriteLine("");
                    writer.WriteLine("");
                }
            #endregion

            paramDict.Add(pa_StructSettingsPrototypes, writer.ToString());
        }
        #endregion

    }
}
