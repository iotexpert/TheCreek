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

namespace CapSense_v1_30.API
{
    public partial class CyAPIGenerator
    {
        #region Parameters
        string m_instanceName;

        //API parameters
        const string pa_writerCVariables = "writerCVariables";
        const string pa_writerCFunctions = "writerCFunctions";
        const string pa_writerCIntFunctions = "writerCIntFunctions";
        const string pa_ParallelFunctionPrototypes = "ParalelFunctionPrototypes";
        const string pa_DefineIdacEnable = "DefineIdacEnable";
        const string pa_Include = "Include";
        const string pa_FunctionPrototypes = "FunctionPrototypes";
        const string pa_StructSettingsPrototypes = "StructSettingsPrototypes";
        const string pa_Define = "Define";
        const string pa_DefineRegs = "DefineRegs";
        const string pa_DefineTotal = "DefineTotal";

        const string pa_writerCHLVariables = "writerCHLVariables";
        const string pa_writerCHLFunctions = "writerCHLFunctions";
        const string pa_writerCHLFunctionsCentroid = "writerCHLFunctionsCentroid";
        const string pa_writerCHLFunctionPrototypes = "writerCHLFunctionPrototypes";

        //schematic consts
        const string S_CLK = "ClockAuto";
        const string S_SW_CLK_UDB = "UDBPrescaler";
        const string S_SW_CLK_FF = "FFPrescaler";
        const string S_DIG_CLK = "UDBSpeed";
        const string S_RAW = "cRAW";
        const string S_PWM = "cPWM";
        const string S_COMP = "cComp";
        const string S_AMUX = "cAMux";
        const string S_IDAC = "cIDAC";
        const string S_PORT = "cPort";
        const string S_ISR = "cISR";
        const string S_VREF = "cvRef";
        const string S_BUF = "cBuf";
        const string S_CONTROL = "cControl";
        #endregion

        #region Service Functions

        public CyAPIGenerator(string instanceName)
        {
            this.m_instanceName = instanceName;
        }

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
            string res = m_instanceName + "_IDAC_IDIR_";
            if (sbItem.m_csdSubMethod == E_CSD_SUB_METHODS.IDACSourcing)
            {
                res += "SRC";
            }
            else if (sbItem.m_csdSubMethod == E_CSD_SUB_METHODS.IDACSinking)
            {
                res += "SINK";
            }
            return res;
        }
        string GetSW_CLK(CyGeneralParams packParams, CyAmuxBParams item)
        {
            string res = packParams.GetPrefixForSchematic(item) + S_SW_CLK_UDB;
            if (item.m_Prescaler == E_PRESCALER.HW)
            {
                res = packParams.GetPrefixForSchematic(item) + S_SW_CLK_FF;
            }

            return res;
        }
        string Get_CLK(CyGeneralParams packParams, CyAmuxBParams item)
        {
            return packParams.GetPrefixForClock(item) + S_CLK;
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
            #region ParallelFunctionPrototypes
            writer = new StringWriter();
            writer.WriteLine("void " + m_instanceName + "_Start(void);");
            writer.WriteLine("void " + m_instanceName + "_Stop(void);");
            if (packParams.m_localParams.IsParallelFull())
            {
                writer.WriteLine("void " + m_instanceName + "_ScanSlotLeft(uint8 slot);");
                writer.WriteLine("void " + m_instanceName + "_ScanSlotRight(uint8 slot);");
            }
            else
            {
                writer.WriteLine("void " + m_instanceName + "_ScanSlot(uint8 slot);");
            }
            writer.WriteLine("void " + m_instanceName + "_ScanAllSlots(void);");

            #endregion
            paramDict.Add(pa_ParallelFunctionPrototypes, writer.ToString());

            if (packParams.m_localParams.IsIdacInSystem())
            {
                #region DefineIdacEnable
                writer = new StringWriter();
                writer.NewLine = "\n";
                writer.WriteLine("");
                writer.WriteLine("/* CR0 Idac Control Register 0 definitions */");
                writer.WriteLine("");
                writer.WriteLine("/* Bit Field  DAC_MODE */");
                writer.WriteLine("#define " + m_instanceName + "_IDAC_MODE_MASK         0x10u");
                writer.WriteLine("#define " + m_instanceName + "_IDAC_MODE_V            0x00u");
                writer.WriteLine("#define " + m_instanceName + "_IDAC_MODE_I            0x10u");
                writer.WriteLine("");
                writer.WriteLine("/* CR1 Idac Control Register 1 definitions */");
                writer.WriteLine("");
                writer.WriteLine("/* Bit Field  DAC_I_DIR */");
                writer.WriteLine("/* Register control of current direction */");
                writer.WriteLine("#define " + m_instanceName + "_IDAC_IDIR_MASK         0x04u");
                writer.WriteLine("#define " + m_instanceName + "_IDAC_IDIR_SINK         0x04u");
                writer.WriteLine("#define " + m_instanceName + "_IDAC_IDIR_SRC          0x00u");
                writer.WriteLine("");
                writer.WriteLine("/* Bit Field  DAC_MX_IOFF_SRC */");
                writer.WriteLine("/* Selects source of IOFF control, reg or UDB */");
                writer.WriteLine("#define " + m_instanceName + "_IDAC_IDIR_CTL_MASK     0x02u");
                writer.WriteLine("#define " + m_instanceName + "_IDAC_IDIR_CTL_REG      0x00u");
                writer.WriteLine("#define " + m_instanceName + "_IDAC_IDIR_CTL_UDB      0x02u");
                #endregion
                paramDict.Add(pa_DefineIdacEnable, writer.ToString());
            }

            //Parallel Functions
            foreach (CyAmuxBParams sbItem in packParams.m_localParams.m_listCsHalfs)
                if (packParams.m_localParams.IsAmuxBusEnable(sbItem))
                {
                    E_EL_SIDE side = sbItem.m_side;
                    string Symbol = SymbolGenerate(packParams, sbItem, packParams.Configuration);
                    string Method = sbItem.m_Method.ToString();
                    string sSide = GetSideName(side, packParams.Configuration);
                    string sSideUp = GetSideNameUpper(side, packParams.Configuration);
                    string csPRS = "cPRS" + sbItem.GetPRSResolution();

                    string SW_CLK = GetSW_CLK(packParams, sbItem);
                    string CLK = Get_CLK(packParams, sbItem);

                    string DIG_CLK = packParams.GetPrefixForSchematic(sbItem) + S_DIG_CLK;

                    int tsens_count = packParams.m_cyWidgetsList.GetListTerminals(side).Count;
                    int tscanslot_count = packParams.m_cyScanSlotsList.GetSSList(side).Count;
                    int tscanslot_gen_count = packParams.m_cyWidgetsList.
                        GetWidgetsCount(E_SENSOR_TYPE.Generic, sbItem.m_side);

                    #region Include

                    writer = new StringWriter();
                    writer.WriteLine("");
                    if (sbItem.m_currectClk != null)
                        if (sbItem.m_currectClk.IsDirectClock() == false)
                        {
                            if ((sbItem.m_side == E_EL_SIDE.Left) ||
                                (packParams.Configuration != E_MAIN_CONFIG.emParallelSynchron))
                                writer.WriteLine("#include \"" + m_instanceName + "_" + CLK + ".h\"");
                        }

                    if (sbItem.IsPrescaler())
                    {
                        writer.WriteLine("#include \"" + m_instanceName + "_" + SW_CLK + ".h\"");
                    }
                    if (sbItem.m_Method == E_CAPSENSE_MODE.CSA)
                    {
                        writer.WriteLine("#include \"" + m_instanceName + "_" + Symbol + "_" + S_IDAC + ".h\"");
                    }
                    writer.WriteLine("#include \"" + m_instanceName + "_" + Symbol + "_" + S_RAW + ".h\"");
                    writer.WriteLine("#include \"" + m_instanceName + "_" + Symbol + "_" + S_COMP + ".h\"");
                    writer.WriteLine("#include \"" + m_instanceName + "_" + Symbol + "_" + S_AMUX + ".h\"");

                    if (sbItem.m_Method == E_CAPSENSE_MODE.CSD)
                    {
                        writer.WriteLine("#include \"" + m_instanceName + "_" + Symbol + "_" + S_PWM + ".h\"");
                        writer.WriteLine("#include \"" + m_instanceName + "_" + DIG_CLK + ".h\"");
                        if (sbItem.IsPRS())
                        {
                            writer.WriteLine("#include \"" + m_instanceName + "_" + Symbol + "_" + csPRS + ".h\"");
                        }
                    }

                    if (sbItem.m_Method == E_CAPSENSE_MODE.CSA)
                    {
                        // svoz_debug
                        //writer.WriteLine("#include \"sw_tp.h\"");

                        writer.WriteLine("#include \"" + m_instanceName + "_CSHL.h\"");
                    }

                    if (sbItem.m_VrefOptions == E_VREF_OPTIONS.Ref_Vdac)
                    {
                        writer.WriteLine("#include \"" + m_instanceName + "_" + Symbol + "_cRefVdac.h\"");
                    }
                    #endregion
                    paramDict.Add(pa_Include + sbItem.m_side, writer.ToString());

                    #region FunctionPrototypes
                    writer = new StringWriter();
                    writer.WriteLine("");
                    writer.WriteLine("void " + m_instanceName + "_" + Method + "_Start" + sSide + "(void);");
                    writer.WriteLine("void " + m_instanceName + "_" + Method + "_Stop" + sSide + "(void);");
                    writer.WriteLine("void " + m_instanceName + "_" + Method + "_ScanSlot" + sSide + "(uint8 slot);");
                    writer.WriteLine("void " + m_instanceName + "_" + Method + "_ScanAllSlots" + sSide + "(void);");
                    writer.WriteLine("void " + m_instanceName + "_" + Method + "_SetSlotSettings" + sSide + "(uint8 slot);");
                    writer.WriteLine("void " + m_instanceName + "_" + Method + "_ClearSlots" + sSide + "(void);");
                    writer.WriteLine("void " + m_instanceName + "_" + Method + "_EnableSensor" + sSide + "(uint8 sensor);");
                    writer.WriteLine("void " + m_instanceName + "_" + Method + "_DisableSensor" + sSide + "(uint8 sensor, uint8 state);");
                    writer.WriteLine("uint16 " + m_instanceName + "_" + Method + "_ReadSlot" + sSide + "(uint8 slot);");
                    if (sbItem.m_Method == E_CAPSENSE_MODE.CSA)
                    {
                        writer.WriteLine("uint8 " + m_instanceName + "_" + Method + "_FindIdacCodeBaseline" + sSide + "(uint8 slot);");
                    }
                    if (sbItem.IsRbAvailible())
                        writer.WriteLine("void " + m_instanceName + "_" + Method + "_SetRBleed" + sSide + "(uint8 rbeed);");

                    writer.WriteLine("/* Interrupt handler */");
                    writer.WriteLine("CY_ISR_PROTO(" + m_instanceName + "_ISR" + sSide + ");");
                    writer.WriteLine("");
                    writer.WriteLine("extern uint8 " + m_instanceName + "_status" + sSide + ";");
                    #endregion
                    paramDict.Add(pa_FunctionPrototypes + sbItem.m_side, writer.ToString());

                    #region Define
                    writer = new StringWriter();
                    writer.NewLine = "\n";
                    writer.WriteLine("");
                    if (packParams.IsAmuxBusBlank(sbItem.m_side))
                    {
                        //Fix compile bug with 0 values
                        tsens_count = 1;
                        tscanslot_count = 1;
                    }
                    writer.WriteLine("#define " + m_instanceName + "_TOTAL_SENSOR_COUNT" + sSideUp +
                        "              " + tsens_count);
                    writer.WriteLine("#define " + m_instanceName + "_TOTAL_SCANSLOT_COUNT" + sSideUp +
                        "            " + tscanslot_count);
                    writer.WriteLine("#define " + m_instanceName + "_TOTAL_GENERIC_SCANSLOT_COUNT" +
                        sSideUp + "    " + tscanslot_gen_count);

                    if (sbItem.CountShieldElectrode > 0)
                    {
                        writer.WriteLine("#define " + m_instanceName + "_TOTAL_SHIELD_COUNT" + sSideUp +
                            "               " + sbItem.CountShieldElectrode);
                    }
                    writer.WriteLine("");
                    writer.WriteLine("/* Define Sensors */");
                    //Define Sensors
                    for (int i = 0; i < packParams.m_cyWidgetsList.GetListTerminals(side).Count; i++)
                    {
                        string str = "#define " + m_instanceName + "_SENSOR_" +
                            packParams.m_cyWidgetsList.GetListTerminals(side)[i].ToString().ToUpper() +
                            sSideUp + "    " + i;
                        writer.WriteLine(str);
                    }
                    writer.WriteLine("");
                    writer.WriteLine("/* Define ScanSlots */");
                    //Define ScanSlots
                    for (int i = 0; i < packParams.m_cyScanSlotsList.GetSSList(side).Count; i++)
                    {
                        string str = "#define " + m_instanceName + "_SCANSLOT_" +
                            packParams.m_cyScanSlotsList.GetSSList(side)[i].GetHeader().ToUpper() +
                            sSideUp + "    " + i;
                        writer.WriteLine(str);
                    }

                    writer.WriteLine("");
                    writer.WriteLine("/* Chanels of CapSense */");

                    int intSh = 0;
                    if (sbItem.IsShieldElectrode())
                    {
                        intSh = sbItem.CountShieldElectrode;
                    }

                    writer.WriteLine("#define " + m_instanceName + "_CMOD_CHANNEL" + sSideUp + "          " +
                        (packParams.m_cyWidgetsList.GetListTerminals(side).Count + intSh).ToString());
                    writer.WriteLine("#define " + m_instanceName + "_CMP_VP_CHANNEL" + sSideUp + "        " +
                        (packParams.m_cyWidgetsList.GetListTerminals(side).Count + intSh + 1).ToString());
                    if (sbItem.IsIdacAvailible())
                    {
                        writer.WriteLine("#define " + m_instanceName + "_IDAC_CHANNEL" + sSideUp + "          " +
                            (packParams.m_cyWidgetsList.GetListTerminals(side).Count + intSh + 2).ToString());
                    }
                    writer.WriteLine("");

                    // PATCH for ISR
                    if (sbItem.m_Method == E_CAPSENSE_MODE.CSD)
                    {
                        writer.WriteLine("#define " + m_instanceName + "_CSD_METHOD" + sSideUp);
                    }
                    else if (sbItem.m_Method == E_CAPSENSE_MODE.CSA)
                    {
                        writer.WriteLine("#define " + m_instanceName + "_CSA_METHOD" + sSideUp);
                    }

                    //Vref Vdac Value
                    writer.WriteLine("#define " + m_instanceName + "_VREF_VDAC_VALUE" + sSideUp + "        " +
                        sbItem.m_VrefVdacValue.ToString());
                    #endregion
                    paramDict.Add(pa_Define + sbItem.m_side, writer.ToString());

                    #region DefineRegs
                    writer = new StringWriter();
                    writer.WriteLine("");
                    writer.WriteLine("/* Control Register */");
                    writer.WriteLine("#define " + m_instanceName + "_CONTROL" + sSideUp + "             (* (reg8 *) " + m_instanceName + "_" + Symbol + "_" + S_CONTROL + "_ctrl_reg__CONTROL_REG )");
                    writer.WriteLine("");
                    writer.WriteLine("/* csBuffer */");
                    writer.WriteLine("#define " + m_instanceName + "_CAPS_CFG0" + sSideUp + "           (* (reg8 *) " + m_instanceName + "_" + Symbol + "_" + S_BUF + "__CFG0 )");
                    writer.WriteLine("#define " + m_instanceName + "_CAPS_CFG1" + sSideUp + "           (* (reg8 *) " + m_instanceName + "_" + Symbol + "_" + S_BUF + "__CFG1 )");
                    writer.WriteLine("#define " + m_instanceName + "_CSBUF_PWRMGR" + sSideUp + "        (* (reg8 *) " + m_instanceName + "_" + Symbol + "_" + S_BUF + "__PM_ACT_CFG )");
                    writer.WriteLine("#define " + m_instanceName + "_CSBUF_PWR_ENABLE" + sSideUp + "    " + m_instanceName + "_" + Symbol + "_" + S_BUF + "__PM_ACT_MSK");
                    writer.WriteLine("");
                    if (sbItem.IsIdacAvailible() && (sbItem.m_Method == E_CAPSENSE_MODE.CSD))
                    {
                        writer.WriteLine("/* csIdac */");
                        writer.WriteLine("#define " + m_instanceName + "_IDAC_CR0" + sSideUp + "        (* (reg8 *) " + m_instanceName + "_" + Symbol + "_" + S_IDAC + "__CR0 )");
                        writer.WriteLine("#define " + m_instanceName + "_IDAC_CR1" + sSideUp + "        (* (reg8 *) " + m_instanceName + "_" + Symbol + "_" + S_IDAC + "__CR1 )");
                        writer.WriteLine("#define " + m_instanceName + "_IDAC_DATA" + sSideUp + "       (* (reg8 *) " + m_instanceName + "_" + Symbol + "_" + S_IDAC + "__D )");
                        writer.WriteLine("#define " + m_instanceName + "_IDAC_STROBE" + sSideUp + "     (* (reg8 *) " + m_instanceName + "_" + Symbol + "_" + S_IDAC + "__STROBE )");
                        writer.WriteLine("#define " + m_instanceName + "_IDAC_TR" + sSideUp + "         (* (reg8 *) " + m_instanceName + "_" + Symbol + "_" + S_IDAC + "__TR )");
                        writer.WriteLine("#define " + m_instanceName + "_IDAC_PWRMGR" + sSideUp + "     (* (reg8 *) " + m_instanceName + "_" + Symbol + "_" + S_IDAC + "__PM_ACT_CFG )");
                        writer.WriteLine("");
                        writer.WriteLine("/* TODO: Remove this line after changes to DAC */");
                        writer.WriteLine("#define " + m_instanceName + "_IDAC_FIRST_SILICON" + sSideUp);
                        writer.WriteLine("");
                        writer.WriteLine("/* PM_ACT_CFG (Active Power Mode CFG Register) */ ");
                        writer.WriteLine("#if !defined(" + m_instanceName + "_IDAC_FIRST_SILICON" + sSideUp + ")");
                        writer.WriteLine("    #define " + m_instanceName + "_IDAC_ACT_PWR_EN" + sSideUp + "    " + m_instanceName + "_" + Symbol + "_" + S_IDAC + "__PM_ACT_MSK");
                        writer.WriteLine("#else");
                        writer.WriteLine("    #define " + m_instanceName + "_IDAC_ACT_PWR_EN" + sSideUp + "    0xFFu /* TODO: Work around for incorrect power enable */");
                        writer.WriteLine("#endif");
                        writer.WriteLine("");
                        writer.WriteLine("/* DAC offset address from CYDEV_FLSHID_BASE */");
                        writer.WriteLine("#define " + m_instanceName + "_DAC0_OFFSET    0x00011Cu");
                        writer.WriteLine("#define " + m_instanceName + "_DAC1_OFFSET    0x000112u");
                        writer.WriteLine("#define " + m_instanceName + "_DAC2_OFFSET    0x000124u");
                        writer.WriteLine("#define " + m_instanceName + "_DAC3_OFFSET    0x000134u");
                        writer.WriteLine("");
                        writer.WriteLine("#define " + m_instanceName + "_DAC_POS" + sSideUp + "  (" + m_instanceName + "_" + Symbol + "_" + S_IDAC + "__D - CYDEV_ANAIF_WRK_DAC0_BASE)");
                        writer.WriteLine("");
                        writer.WriteLine("#if(" + m_instanceName + "_DAC_POS" + sSideUp + " == 0)");
                        writer.WriteLine("    #define " + m_instanceName + "_DAC_TRIM_BASE" + sSideUp + "    CYDEV_FLSHID_BASE + " + m_instanceName + "_DAC0_OFFSET");
                        writer.WriteLine("#endif");
                        writer.WriteLine("");
                        writer.WriteLine("#if(" + m_instanceName + "_DAC_POS" + sSideUp + " == 1)");
                        writer.WriteLine("    #define " + m_instanceName + "_DAC_TRIM_BASE" + sSideUp + "    CYDEV_FLSHID_BASE + " + m_instanceName + "_DAC1_OFFSET");
                        writer.WriteLine("#endif");
                        writer.WriteLine("");
                        writer.WriteLine("#if(" + m_instanceName + "_DAC_POS" + sSideUp + " == 2)");
                        writer.WriteLine("    #define " + m_instanceName + "_DAC_TRIM_BASE" + sSideUp + "    CYDEV_FLSHID_BASE + " + m_instanceName + "_DAC2_OFFSET");
                        writer.WriteLine("#endif");
                        writer.WriteLine("");
                        writer.WriteLine("#if(" + m_instanceName + "_DAC_POS" + sSideUp + " == 3)");
                        writer.WriteLine("    #define " + m_instanceName + "_DAC_TRIM_BASE" + sSideUp + "    CYDEV_FLSHID_BASE + " + m_instanceName + "_DAC3_OFFSET");
                        writer.WriteLine("#endif");
                        writer.WriteLine("");
                    }
                    writer.WriteLine("/* ISR Number and Priority to work with CyLib functions */");
                    writer.WriteLine("#define " + m_instanceName + "_ISR_NUMBER" + sSideUp + "        " + m_instanceName + "_" + Symbol + "_" + S_ISR + "__INTC_NUMBER");
                    writer.WriteLine("#define " + m_instanceName + "_ISR_PRIORITY" + sSideUp + "      " + m_instanceName + "_" + Symbol + "_" + S_ISR + "__INTC_PRIOR_NUM");
                    writer.WriteLine("");
                    writer.WriteLine("#if(CYDEV_CHIP_DIE_EXPECT == CYDEV_CHIP_DIE_LEOPARD)");
                    writer.WriteLine("    #if((CYDEV_CHIP_REV_EXPECT <= CYDEV_CHIP_REV_LEOPARD_ES2) && (" + m_instanceName + "_" + Symbol + "_cISR__ES2_PATCH))");
                    writer.WriteLine("        #include <intrins.h>");
                    writer.WriteLine("        #define " + m_instanceName + "_ISR_PATCH() _nop_(); _nop_(); _nop_(); _nop_(); _nop_(); _nop_(); _nop_(); _nop_();");
                    writer.WriteLine("    #endif");
                    writer.WriteLine("#endif");
                    #endregion
                    paramDict.Add(pa_DefineRegs + sbItem.m_side, writer.ToString());

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
                }

            writer = new StringWriter();
            if (packParams.Configuration != E_MAIN_CONFIG.emSerial)
            {
                writer.WriteLine("");
                writer.WriteLine("/* ScanSlots Count */");
                int tscanslot_count = packParams.m_cyScanSlotsList.GetScanSlotCount();
                //Fix error during blank parameters
                if (tscanslot_count == 0) tscanslot_count = 1;

                writer.WriteLine("#define " + m_instanceName + "_TOTAL_SCANSLOT_COUNT" + "            " +
                    tscanslot_count);
                writer.WriteLine("#define " + m_instanceName + "_TOTAL_GENERIC_SCANSLOT_COUNT" + "    " +
                    packParams.m_cyWidgetsList.GetWidgetsCount(E_SENSOR_TYPE.Generic));
            }
            paramDict.Add(pa_DefineTotal, writer.ToString());


            #region writerCHLFunctionPrototypes
            writer = new StringWriter();
            writer.NewLine = "\n";
            if (packParams.Configuration != E_MAIN_CONFIG.emSerial)
            {
                writer.WriteLine("void " + m_instanceName + "_CSHL_InitializeAllBaselines(void);");
                writer.WriteLine("uint8 " + m_instanceName + "_CSHL_CheckIsAnySlotActive(void);");
                writer.WriteLine("void " + m_instanceName + "_CSHL_UpdateAllBaselines(void);");
                writer.NewLine = "\n";
            }

            writer.WriteLine("");
            if (packParams.m_cyWidgetsList.GetWidgetsCount(E_SENSOR_TYPE.Linear_Slider) > 0)
                writer.WriteLine("uint16 " + m_instanceName + "_CSHL_GetCentroidPos(uint8 widget);");
            if (packParams.m_cyWidgetsList.GetWidgetsCount(E_SENSOR_TYPE.Radial_Slider) > 0)
                writer.WriteLine("uint16 " + m_instanceName + "_CSHL_GetRadialCentroidPos(uint8 widget);");
            if (packParams.m_cyWidgetsList.GetDoubleFullWidgetsCount() > 0)
                writer.WriteLine("uint16 " + m_instanceName + "_CSHL_GetDoubleCentroidPos(uint8 widget);");
            
            writer.WriteLine("");
            writer.WriteLine("");
            writer.WriteLine("/***************************************");
            writer.WriteLine("*           API Constants");
            writer.WriteLine("***************************************/");

            writer.WriteLine("");
            writer.WriteLine("/* Widgets constants definition */");
            foreach (CyElWidget wi in packParams.m_cyWidgetsList.GetListWithFullWidgetsHL())
            {
                if (CySensorType.IsTouchPad(wi.m_type))
                /* Add Header Widget */
                {
                    CyElWidget part = packParams.m_cyWidgetsList.GetBothParts(wi)[0];
                    string str = "#define " + m_instanceName + "_CSHL_" + ((CyElUnTouchPad)part).ToHeaderString().
                        ToUpper() + "        " + packParams.m_cyWidgetsList.GetIndexInListWithFullWidgetsHL(part);
                    writer.WriteLine(str);
                }

                for (int i = 0; i < packParams.m_cyWidgetsList.GetBothParts(wi).Count; i++)
                {
                    CyElWidget part = packParams.m_cyWidgetsList.GetBothParts(wi)[i];
                    string str = "#define " + m_instanceName + "_CSHL_" + part.ToString().ToUpper() + "        " +
                        packParams.m_cyWidgetsList.GetIndexInListWithFullWidgetsHL(part);
                    writer.WriteLine(str);
                }

            }

            writer.WriteLine("");
            writer.WriteLine("#define " + m_instanceName + "_CSHL_TOTAL_WIDGET_COUNT" + "          " +
                packParams.m_cyWidgetsList.GetCountWidgetsHL());
            writer.WriteLine("#define " + m_instanceName + "_CSHL_NUMBER_OF_DOUBLE_STRUCTS" + "    " +
                packParams.m_cyWidgetsList.GetDoubleFullWidgetsCount());


            #endregion
            paramDict.Add(pa_writerCHLFunctionPrototypes, writer.ToString());

            #region StructSettingsPrototypes
            writer = new StringWriter();
            writer.NewLine = "\n";
            foreach (CyAmuxBParams sbItem in packParams.m_localParams.m_listCsHalfs)
                if (packParams.m_localParams.IsAmuxBusEnable(sbItem))
                {
                    E_EL_SIDE side = sbItem.m_side;
                    string Method = sbItem.m_Method.ToString();
                    string sSide = GetSideName(side, packParams.Configuration);
                    writer.WriteLine("");
                    writer.WriteLine("/*        Settings for " + Method + " " + sSide + "        */");
                    writer.WriteLine("typedef struct _" + m_instanceName + "_" + Method + "_Settings" + sSide + "");
                    writer.WriteLine("{");
                    if (sbItem.IsIdacAvailible())
                    {
                        writer.WriteLine("    uint8 IdacRange;            /* 1-3 range exist, multiplier for IDAC */");
                        writer.WriteLine("    uint8 IdacSettings;         /* IDAC settings for measurament */");
                    }
                    if (sbItem.m_Method == E_CAPSENSE_MODE.CSA)
                    {
                        writer.WriteLine("    uint8 ScanLength;           /* IDAC measure = SA - ScanLength */");
                        writer.WriteLine("    uint8 SettlingTime;         /* SettlingTime */");
                        writer.WriteLine("    uint8 IdacCodeBaseline;     /* IdacCodeBaseline */");
                    }
                    if (sbItem.IsPrescaler())
                    {
                        writer.WriteLine("    uint8 PrescalerPeriod;      /* Prescaler for SW_CLK */");
                    }
                    if (sbItem.m_Method == E_CAPSENSE_MODE.CSD)
                    {
                        writer.WriteLine("    uint8 Resolution;           /* PWM Resolution */");
                        writer.WriteLine("    uint8 ScanSpeed;            /* Speed of scanning  DIG_CLK */");
                        if (sbItem.IsPRS())
                        {
                            writer.WriteLine("    uint16 PrsPolynomial;       /* Prs signal polinomial to \"Break befor Make\" */");
                        }
                    }
                    writer.WriteLine("    uint8 DisableState;         /* Disable state of ScanSlot */");
                    writer.WriteLine("");
                    writer.WriteLine("} " + m_instanceName + "_" + Method + "_Settings" + sSide + ";");
                }
            #endregion

            paramDict.Add(pa_StructSettingsPrototypes, writer.ToString());
        }
        #endregion

    }
}
