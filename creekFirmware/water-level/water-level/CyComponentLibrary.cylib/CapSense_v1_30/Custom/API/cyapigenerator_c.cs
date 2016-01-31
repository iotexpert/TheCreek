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
        #region CollectApiCFile
        public void CollectApiCFile(ICyInstQuery_v1 query, ICyTerminalQuery_v1 termQuery, CyGeneralParams packParams, 
            ref Dictionary<string, string> paramDict)
        {

            CyMyStringWriter writerCVariables = new CyMyStringWriter();
            writerCVariables.NewLine = "\n";
            TextWriter writerCFunctions = new StringWriter();
            writerCFunctions.NewLine = "\n";

            CyMyStringWriter writerCHLFunctions = new CyMyStringWriter();
            writerCHLFunctions.NewLine = "\n";

            CyMyStringWriter writerCHLFunctionsCentroid = new CyMyStringWriter();
            writerCHLFunctionsCentroid.NewLine = "\n";

            CyMyStringWriter writerCHLVariables = new CyMyStringWriter();
            writerCHLVariables.NewLine = "\n";

            TextWriter writerCIntFunctions = new StringWriter();
            writerCIntFunctions.NewLine = "\n";

            ApiCollectCFunctionForParallelMode(ref writerCFunctions, packParams);

            ApiCollectCHLVariables(ref writerCHLVariables, packParams);

            //Collect Data for Sides
            foreach (CyAmuxBParams sbItem in packParams.m_localParams.m_listCsHalfs)
                if (packParams.m_localParams.IsAmuxBusEnable(sbItem))
                {
                    ApiCollectCVariables(ref writerCVariables, packParams, sbItem);
                    ApiCollectCFunctionForSide(ref writerCFunctions, packParams, sbItem);

                    ApiCollectCHLFunctionForSide(ref writerCHLFunctions, packParams, sbItem);
                }

            writerCVariables.WriteLine("uint16 " + m_instanceName + "_SlotResult[" + m_instanceName + 
                "_TOTAL_SCANSLOT_COUNT] = {0};");
            writerCVariables.WriteLine("uint16 *" + m_instanceName + "_SlotRaw = " + m_instanceName + "_SlotResult;");

            ApiCollectCHLFunctionBase(ref writerCHLFunctions, packParams);
            ApiCollectCHLFunctionCentroid(ref writerCHLFunctionsCentroid, packParams);


            paramDict.Add(pa_writerCHLVariables, writerCHLVariables.ToString());
            paramDict.Add(pa_writerCVariables, writerCVariables.ToString());
            paramDict.Add(pa_writerCFunctions, writerCFunctions.ToString());
            paramDict.Add(pa_writerCIntFunctions, writerCIntFunctions.ToString());

            paramDict.Add(pa_writerCHLFunctions, writerCHLFunctions.ToString());
            paramDict.Add(pa_writerCHLFunctionsCentroid, writerCHLFunctionsCentroid.ToString());
        }
        #endregion

        #region apiCollectCVariables Parallel part
        string GetIDACRangeStr(int i)
        {
            switch (i)
            {
                case 0: return "_IDAC_RANGE_32uA";
                case 1: return "_IDAC_RANGE_255uA";
                case 2: return "_IDAC_RANGE_2mA";
                default:
                    break;
            }
            return "";
        }
        string GetCISStr(int i)
        {
            switch (i)
            {
                case 0: return "_DISABLE_STATE_GND";
                case 1: return "_DISABLE_STATE_HIGHZ";
                case 2: return "_DISABLE_STATE_SHIELD";
                default:
                    break;
            }
            return "";
        }

        public void ApiCollectCVariables(ref CyMyStringWriter writer, CyGeneralParams packParams, CyAmuxBParams sbItem)
        {
            E_EL_SIDE side = sbItem.m_side;
            string Symbol = SymbolGenerate(packParams, sbItem, packParams.Configuration);
            string sSide = GetSideName(side, packParams.Configuration);
            string sSideUp = GetSideNameUpper(side, packParams.Configuration);
            string Method = sbItem.m_Method.ToString();

            #region  Filling  ScanSlotTable & Index_Table
            TextWriter writerIndex_Table = new StringWriter();
            writerIndex_Table.NewLine = "\n";
            string strIndexTable = "";
            int ind_for_IndexTable = 0;
            int ind_for_PortMasTable = 0;

            writer.Write("" + m_instanceName + "_Slot " + m_instanceName + "_ScanSlotTable" + sSide + "[] = ");
            writerIndex_Table.Write("const uint8 " + m_instanceName + "_IndexTable" + sSide + "[] = ");

            if (packParams.IsAmuxBusBlank(sbItem.m_side))
            {
                writer.WriteLine("{0};");
                writerIndex_Table.WriteLine("{0};");
            }
            else
            {
                writerIndex_Table.Write("{");

                writer.WriteLine("\n{"); ;//Open array
                List<CyElScanSlot> listSS = packParams.m_cyScanSlotsList.GetSSList(side);
                List<CyElTerminal> listTerm = packParams.m_cyWidgetsList.GetListTerminals(side);
                for (int i = 0; i < listSS.Count; i++)
                {
                    //Calculate offset in RawCount array
                    int offset_row_count = 0;
                    if (sbItem.m_side == E_EL_SIDE.Right)
                        offset_row_count = packParams.m_cyScanSlotsList.GetSSList(E_EL_SIDE.Left).Count;

                    CyElScanSlot ss = listSS[i];
                    string widgetNumber = packParams.m_cyWidgetsList.
                        GetIndexInListWithFullWidgetsHL(ss.m_Widget).ToString();

                    //No properies for Generic
                    if (ss.m_Widget.m_type == E_SENSOR_TYPE.Generic) widgetNumber = "0xffu";

                    //Get terminal index
                    ind_for_PortMasTable = 0;
                    if (ss.GetTerminals().Count == 1)
                        ind_for_PortMasTable = listTerm.IndexOf(ss.GetTerminals()[0]);
                    else if (ss.GetTerminals().Count > 1)
                        ind_for_PortMasTable = ind_for_IndexTable;

                    //Collect Sacn Slot parameters
                    string str = "    {" + (offset_row_count + i).ToString() + ", " + ind_for_PortMasTable + ", " +
                        ss.GetTerminals().Count + ", " + widgetNumber + ", " + "0" + "}";

                    //Last char
                    if (i < packParams.m_cyScanSlotsList.GetSSList(side).Count - 1)
                        str += ",";

                    writer.WriteLine(str);

                    //Filling Index_Table
                    if (ss.GetTerminals().Count > 1)
                        foreach (CyElTerminal item in ss.GetTerminals())
                        {
                            ind_for_IndexTable++;
                            strIndexTable += listTerm.IndexOf(item).ToString() + ", ";
                        }
                }
                writer.WriteLine("};");//Close array

                //Filling Index_Table
                if (strIndexTable.Length > 1)
                    strIndexTable = strIndexTable.Remove(strIndexTable.Length - 2);
                if (ind_for_IndexTable > 0)
                    writerIndex_Table.Write(strIndexTable + "};");
                else
                    writerIndex_Table.Write(strIndexTable + " 0x00 };");

            }
            writer.WriteLine("");
            writer.WriteLine(writerIndex_Table.ToString());
            writer.WriteLine("");
            #endregion

            #region  SettingsTable
            writer.Write(m_instanceName + "_" + Method + "_Settings" + sSide + " ");

            writer.Write(m_instanceName + "_SettingsTable" + sSide + "[] = ");
            if (packParams.IsAmuxBusBlank(sbItem.m_side))
            {
                writer.WriteLine("{0};");
            }
            else
            {
                writer.WriteLine("\n{");
                for (int i = 0; i < packParams.m_cyScanSlotsList.GetSSList(side).Count; i++)
                {
                    CyElScanSlot ss = packParams.m_cyScanSlotsList.GetSSList(side)[i];
                    writer.Write("    {");

                    if (sbItem.IsIdacAvailible())
                    {
                        // 1-3 range exist, multiplier for IDAC 
                        writer.Write(m_instanceName + GetIDACRangeStr(ss.m_SSProperties.m_IDACRange));                
                        writer.Write(", ");
                        writer.Write(ss.m_SSProperties.m_IDACSettings.m_Value);// IDAC settings for measurament
                        writer.Write(", ");
                    }
                    //CSA Properties
                    if (sbItem.m_Method == E_CAPSENSE_MODE.CSA)
                    {
                        //IDAC measure = SA - ScanLength
                        writer.Write(ss.m_SSProperties.m_baseProps.m_Scanlength.m_Value);
                        writer.Write(", ");
                        writer.Write(ss.m_SSProperties.m_baseProps.m_SettlingTime.m_Value);//SettlingTime
                        writer.Write(", ");
                        writer.Write("0x00");// IdacCode
                        writer.Write(", ");
                    }
                    if (sbItem.IsPrescaler())
                    {

                        writer.Write(ss.m_SSProperties.m_baseProps.m_PrescPer.m_Value);//Prescaler for CapsClock
                        writer.Write(", ");
                    }

                    //CSD Properties
                    if (sbItem.m_Method == E_CAPSENSE_MODE.CSD)
                    {
                        writer.Write(ss.m_SSProperties.GetResolution(m_instanceName));//PWM Resolution
                        writer.Write(", ");
                        writer.Write(ss.m_SSProperties.GetScanSpeed(m_instanceName));//Speed of scanning
                        writer.Write(", ");
                        if (sbItem.IsPRS())
                        {
                            /* Prs signal polinomial to "Break befor Make" */
                            switch (sbItem.GetPRSResolution())
                            {
                                case "8": writer.Write("0xB8"); break;
                                case "16": writer.Write("0xB400"); break;
                                default: writer.Write("0xB8");
                                    break;
                            }
                            writer.Write(", ");
                        }
                    }
                    writer.Write(m_instanceName + GetCISStr(ss.m_SSProperties.m_CIS));//(Hi-Z;GND; Shield)
                    writer.Write(", ");

                    //Removing ", "
                    if (writer.Get0Length() > 4)
                        writer.Remove(writer.Get0Length() - 2);

                    if (i < packParams.m_cyScanSlotsList.GetSSList(side).Count - 1)
                        writer.WriteLine("},");
                    else
                        writer.WriteLine("}");
                }
                writer.WriteLine("};");
            }
            writer.WriteLine("");
            #endregion

            #region PortShift  Generation
            writer.WriteLine("const " + m_instanceName + "_PortShift " + m_instanceName + 
            "_PortShiftTable" + sSide + "[] = ");
            if (packParams.IsAmuxBusBlank(sbItem.m_side))
            {
                writer.WriteLine("{0};");
            }
            else
            {
                writer.WriteLine("{");
                for (int i = 0; i < packParams.m_cyWidgetsList.GetListTerminals(side).Count; i++)
                {
                    string str = m_instanceName + "_" + Symbol + "_" + S_PORT + "__" + 
                        packParams.m_cyWidgetsList.GetListTerminals(side)[i].ToString();
                    str = "    {" + str + "__PORT" + ", " + str + "__SHIFT" + "}";
                    if (i < packParams.m_cyWidgetsList.GetListTerminals(side).Count - 1)
                        str += ",";
                    writer.WriteLine(str);
                }
                writer.WriteLine("};");
            }
            writer.WriteLine("");
            #endregion

            #region Rb Gen
            if (sbItem.IsRbAvailible())
            {
                writer.WriteLine("/* Different for Left and Right sides */");
                writer.WriteLine("reg8 *" + m_instanceName + "_RbPortPinTable" + sSide + "[] = ");
                writer.WriteLine("{");
                for (int i = 0; i < sbItem.CountRb; i++)
                {
                    writer.Write("    (reg8 *) " + m_instanceName + "_" + Symbol + "_cRb" + i + "__" + 
                    packParams.GetAliasRbByIndex(sbItem, i) + "__PC");
                    if (i != sbItem.CountRb - 1)
                        writer.WriteLine(",");
                }
                writer.WriteLine("");
                writer.WriteLine("};");

                writer.WriteLine("");
                writer.WriteLine("uint8 " + m_instanceName + "_currentRbleed" + sSide + " = " + m_instanceName + "_MAX_RB_NUMBER;");
                writer.WriteLine("");
            }
            #endregion

            #region Shield Gen
            if (sbItem.IsShieldElectrode())
            {
                writer.WriteLine("const " + m_instanceName + "_PortShift " + m_instanceName + "_ShieldPortShiftTable" + sSide + "[] = ");
                writer.WriteLine("{");
                for (int i = 0; i < sbItem.CountShieldElectrode; i++)
                {
                    string str = m_instanceName + "_" + Symbol + "_" + S_PORT + "__" + 
                        packParams.GetAliasShieldByIndex(sbItem, i);
                    str = "    {" + str + "__PORT" + ", " + str + "__SHIFT" + "}";
                    if (i < sbItem.CountShieldElectrode - 1)
                        str += ",";
                    writer.WriteLine(str);
                }
                writer.WriteLine("};");

                writer.WriteLine("");
            }
            #endregion

            writer.WriteLine("uint8 " + m_instanceName + "_status" + sSide + " = 0;");
            writer.WriteLine("");
            writer.WriteLine("uint8 " + m_instanceName + "_idacInitVar" + sSide + " = 0;");
            writer.WriteLine("");
        }
        #endregion

        #region  apiCollectCFunctionForParallelMode
        public void ApiCollectCFunctionForParallelMode(ref TextWriter writer, CyGeneralParams packParams)
        {

            string MethodL = packParams.m_localParams.m_listCsHalfs[0].m_Method.ToString();
            string MethodR = packParams.m_localParams.m_listCsHalfs[1].m_Method.ToString();

            #region Start
            writer.WriteLine("/*******************************************************************************");
            writer.WriteLine("* Function Name: " + m_instanceName + "_Start");
            writer.WriteLine("********************************************************************************");
            writer.WriteLine("*");
            writer.WriteLine("* Summary:");
            writer.WriteLine("*  Initializes registers for each module and starts the component. This function");
            writer.WriteLine("*  calls functions automatically depending on modules selected in the");
            writer.WriteLine("*  customizer. This function should be called prior to calling any other");
            writer.WriteLine("*  component functions.");
            writer.WriteLine("*");
            writer.WriteLine("* Parameters:");
            writer.WriteLine("*  void");
            writer.WriteLine("*");
            writer.WriteLine("* Return:");
            writer.WriteLine("*  void");
            writer.WriteLine("*");
            writer.WriteLine("*******************************************************************************/");
            writer.WriteLine("void " + m_instanceName + "_Start(void)");
            writer.WriteLine("{");
            foreach (CyAmuxBParams sbItem in packParams.m_localParams.m_listCsHalfs)
                if (packParams.m_localParams.IsAmuxBusEnable(sbItem))
                {
                    string sSide = GetSideName(sbItem.m_side, packParams.Configuration);
                    string Method = sbItem.m_Method.ToString();
                    writer.WriteLine("    " + m_instanceName + "_" + Method + "_Start" + sSide + "();");
                }
            writer.WriteLine("}");
            writer.WriteLine("");
            writer.WriteLine("");
            #endregion

            #region Stop
            writer.WriteLine("/*******************************************************************************");
            writer.WriteLine("* Function Name: " + m_instanceName + "_Stop");
            writer.WriteLine("********************************************************************************");
            writer.WriteLine("*");
            writer.WriteLine("* Summary:");
            writer.WriteLine("*  Stops the slot scanner, disables interrupts, and resets all slots to an ");
            writer.WriteLine("*  inactive state");
            writer.WriteLine("*");
            writer.WriteLine("* Parameters:");
            writer.WriteLine("*  void");
            writer.WriteLine("*");
            writer.WriteLine("* Return:");
            writer.WriteLine("*  void");
            writer.WriteLine("*");
            writer.WriteLine("*******************************************************************************/");
            writer.WriteLine("void " + m_instanceName + "_Stop(void)");
            writer.WriteLine("{");
            foreach (CyAmuxBParams sbItem in packParams.m_localParams.m_listCsHalfs)
                if (packParams.m_localParams.IsAmuxBusEnable(sbItem))
                {
                    string sSide = GetSideName(sbItem.m_side, packParams.Configuration);
                    string Method = sbItem.m_Method.ToString();
                    writer.WriteLine("    " + m_instanceName + "_" + Method + "_Stop" + sSide + "();");

                }
            writer.WriteLine("}");
            writer.WriteLine("");
            writer.WriteLine("");
            #endregion

            if (packParams.m_localParams.IsParallelFull())
            {

                #region ScanSlot Left
                writer.WriteLine("/*******************************************************************************");
                writer.WriteLine("* Function Name: " + m_instanceName + "_ScanSlotLeft");
                writer.WriteLine("********************************************************************************");
                writer.WriteLine("*");
                writer.WriteLine("* Summary:");
                writer.WriteLine("*  Calls the function " + m_instanceName + "_" + MethodL + "_ScanSlot and scans defined scan slot");
                writer.WriteLine("*  from Left side.");
                writer.WriteLine("*");
                writer.WriteLine("* Parameters:");
                writer.WriteLine("*  slot:  Scan slot number");
                writer.WriteLine("*");
                writer.WriteLine("* Return:");
                writer.WriteLine("*  void");
                writer.WriteLine("*");
                writer.WriteLine("*******************************************************************************/");
                writer.WriteLine("void " + m_instanceName + "_ScanSlotLeft(uint8 slot)");
                writer.WriteLine("{");
                writer.WriteLine("    " + m_instanceName + "_" + MethodL + "_ScanSlotLeft(slot);");
                writer.WriteLine("}");
                writer.WriteLine("");
                writer.WriteLine("");
                #endregion

                #region ScanSlot Right
                writer.WriteLine("/*******************************************************************************");
                writer.WriteLine("* Function Name: " + m_instanceName + "_ScanSlotRight");
                writer.WriteLine("********************************************************************************");
                writer.WriteLine("*");
                writer.WriteLine("* Summary:");
                writer.WriteLine("*  Calls the function " + m_instanceName + "_" + MethodR + "_ScanSlot and scans defined scan slot");
                writer.WriteLine("*  from Right side.");
                writer.WriteLine("*");
                writer.WriteLine("* Parameters:");
                writer.WriteLine("*  slot:  Scan slot number");
                writer.WriteLine("*");
                writer.WriteLine("* Return:");
                writer.WriteLine("*  void");
                writer.WriteLine("*");
                writer.WriteLine("*******************************************************************************/");
                writer.WriteLine("void " + m_instanceName + "_ScanSlotRight(uint8 slot)");
                writer.WriteLine("{");
                writer.WriteLine("    " + m_instanceName + "_" + MethodR + "_ScanSlotRight(slot);");
                writer.WriteLine("}");
                writer.WriteLine("");
                writer.WriteLine("");
                #endregion

            }
            else
            {
                //Not Paralle Full and Serial
                foreach (CyAmuxBParams sbItem in packParams.m_localParams.m_listCsHalfs)
                    if (packParams.m_localParams.IsAmuxBusEnable(sbItem))
                    {
                        #region ScanSlot
                        string Method = sbItem.m_Method.ToString();
                        writer.WriteLine("/*******************************************************************************");
                        writer.WriteLine("* Function Name: " + m_instanceName + "_ScanSlot");
                        writer.WriteLine("********************************************************************************");
                        writer.WriteLine("*");
                        writer.WriteLine("* Summary:");
                        writer.WriteLine("*  Calls the function " + m_instanceName + "_" + Method + "_ScanSlot and scans defined scan slot.");
                        writer.WriteLine("*");
                        writer.WriteLine("* Parameters:");
                        writer.WriteLine("*  slot:  Scan slot number");
                        writer.WriteLine("*");
                        writer.WriteLine("* Return:");
                        writer.WriteLine("*  void");
                        writer.WriteLine("*");
                        writer.WriteLine("*******************************************************************************/");
                        writer.WriteLine("void " + m_instanceName + "_ScanSlot(uint8 slot)");
                        writer.WriteLine("{");
                        writer.WriteLine("      " + m_instanceName + "_" + Method + "_ScanSlot(slot);");
                        writer.WriteLine("}");
                        writer.WriteLine("");
                        writer.WriteLine("");
                        #endregion
                    }

            }

            if (packParams.m_localParams.IsParallelFull())
            {
                #region Turn Off Idac
                foreach (CyAmuxBParams sbItem in packParams.m_localParams.m_listCsHalfs)
                {
                    string sSide = GetSideName(sbItem.m_side, packParams.Configuration);
                    string sSideUp = GetSideNameUpper(sbItem.m_side, packParams.Configuration);
                    string Symbol = SymbolGenerate(packParams, sbItem, packParams.Configuration);

                    if (sbItem.IsIdacAvailible() && (sbItem.m_Method == E_CAPSENSE_MODE.CSD))
                    {
                        #region Idac_SetValue
                        writer.WriteLine("/*******************************************************************************");
                        writer.WriteLine("* Function Name: " + m_instanceName + "_" + Symbol + "_" + S_IDAC + "_SetValue");
                        writer.WriteLine("********************************************************************************");
                        writer.WriteLine("*");
                        writer.WriteLine("* Summary:");
                        writer.WriteLine("*  Sets DAC value");
                        writer.WriteLine("*");
                        writer.WriteLine("* Parameters:");
                        writer.WriteLine("*  value:  Sets DAC value between 0 and 255.");
                        writer.WriteLine("*");
                        writer.WriteLine("* Return:");
                        writer.WriteLine("*  void");
                        writer.WriteLine("*");
                        writer.WriteLine("*******************************************************************************/");
                        writer.WriteLine("void " + m_instanceName + "_" + Symbol + "_" + S_IDAC + "_SetValue(uint8 value)");
                        writer.WriteLine("{");
                        writer.WriteLine("    /*  Set Value  */");
                        writer.WriteLine("    " + m_instanceName + "_IDAC_DATA" + sSideUp + " = value;");
                        writer.WriteLine("    ");
                        writer.WriteLine("    #if defined(" + m_instanceName + "_IDAC_FIRST_SILICON" + sSideUp + ")");
                        writer.WriteLine("        " + m_instanceName + "_IDAC_DATA" + sSideUp + " = value;        /*  TODO: Need for first version of silicon */");
                        writer.WriteLine("    #endif");
                        writer.WriteLine("}");
                        writer.WriteLine("");
                        writer.WriteLine("");
                        #endregion
                    }
                }
                #endregion
            }


            if (packParams.m_localParams.IsParallelFull())
            {
                // Parallel Async or Sync
                if (packParams.Configuration == E_MAIN_CONFIG.emParallelAsynchron)
                {
                    #region ScanAllSlots Asynchron
                    writer.WriteLine("/*******************************************************************************");
                    writer.WriteLine("* Function Name: " + m_instanceName + "_ScanAllSlots");
                    writer.WriteLine("********************************************************************************");
                    writer.WriteLine("*");
                    writer.WriteLine("* Summary:");
                    writer.WriteLine("*  Scans slots from both sides in parallel. One slot from the right side and ");
                    writer.WriteLine("*  one slot from the left side are scanned at a time without synchronization.");
                    writer.WriteLine("*  If one side has more slots than the other then the remaining slots on ");
                    writer.WriteLine("*  the side with more are scanned singly. The scanning ends when all slots ");
                    writer.WriteLine("*  are scanned.");
                    writer.WriteLine("*");
                    writer.WriteLine("* Parameters:");
                    writer.WriteLine("*  void");
                    writer.WriteLine("*");
                    writer.WriteLine("* Return:");
                    writer.WriteLine("*  void");
                    writer.WriteLine("*");
                    writer.WriteLine("*******************************************************************************/");
                    writer.WriteLine("void " + m_instanceName + "_ScanAllSlots(void)");
                    writer.WriteLine("{");
                    writer.WriteLine("    uint8 l = 0, r = 0;");
                    writer.WriteLine("    uint8 k, s, state, maxSns, startIndex, rawIndex, leftDone, rightDone;");
                    if ((packParams.m_localParams.m_listCsHalfs[0].m_Method == E_CAPSENSE_MODE.CSA) ||
                       (packParams.m_localParams.m_listCsHalfs[1].m_Method == E_CAPSENSE_MODE.CSA))
                    {
                        writer.WriteLine("    uint8 settlingTime, idacSettings, status;");
                    }
                    writer.WriteLine("    ");
                    writer.WriteLine("    leftDone = " + m_instanceName + "_START_CAPSENSING;");
                    writer.WriteLine("    rightDone = " + m_instanceName + "_START_CAPSENSING;");
                    writer.WriteLine("    ");
                    writer.WriteLine("    while ((l < " + m_instanceName + "_TOTAL_SCANSLOT_COUNT_LEFT) || (r < " + m_instanceName + "_TOTAL_SCANSLOT_COUNT_RIGHT))");
                    writer.WriteLine("    {");
                    writer.WriteLine("        if (leftDone)");
                    writer.WriteLine("        {");
                    if (packParams.m_localParams.m_listCsHalfs[0].m_Method == E_CAPSENSE_MODE.CSD)
                    {
                        writer.WriteLine("            /* Set Slot Settings  for Left  */");
                        writer.WriteLine("            " + m_instanceName + "_" + MethodL + "_SetSlotSettingsLeft(l);");
                        writer.WriteLine("            ");
                        writer.WriteLine("            /* Disable csBuf Left */");
                        writer.WriteLine("            " + m_instanceName + "_CAPS_CFG0_LEFT &= ~" + m_instanceName + "_CSBUF_ENABLE;");
                        writer.WriteLine("            ");
                        writer.WriteLine("            maxSns = " + m_instanceName + "_ScanSlotTableLeft[l].SnsCnt;");
                        writer.WriteLine("            startIndex = " + m_instanceName + "_ScanSlotTableLeft[l].IndexOffset;");
                        writer.WriteLine("            ");

                        if (packParams.m_localParams.m_listCsHalfs[0].m_csdSubMethod == E_CSD_SUB_METHODS.IDACSinking)
                        {
                            writer.WriteLine("            /* Connect Left IDAC */");
                            writer.WriteLine("            " + m_instanceName + "_lb" + MethodL + "_" + S_AMUX + "_Connect(" + m_instanceName + "_IDAC_CHANNEL_LEFT);");
                            writer.WriteLine("            ");
                        }
                        else if (packParams.m_localParams.m_listCsHalfs[0].m_csdSubMethod == E_CSD_SUB_METHODS.IDACDisable_RB)
                        {
                            writer.WriteLine("            /* Connect Left DSI output to Rb */");
                            writer.WriteLine("            *" + m_instanceName + "_RbPortPinTableLeft[" + m_instanceName + "_currentRbleedLeft] |= " + m_instanceName + "_BYP_MASK;");
                            writer.WriteLine("            ");
                        }
                        writer.WriteLine("            /* Enable Sensors */");
                        writer.WriteLine("            if (maxSns == " + m_instanceName + "_ALONE_SENSOR)");
                        writer.WriteLine("            {");
                        writer.WriteLine("                " + m_instanceName + "_" + MethodL + "_EnableSensorLeft(startIndex);");
                        writer.WriteLine("            }");
                        writer.WriteLine("            else");
                        writer.WriteLine("            {");
                        writer.WriteLine("                for(k=0; k < maxSns; k++)");
                        writer.WriteLine("                {");
                        writer.WriteLine("                    s = " + m_instanceName + "_IndexTableLeft[startIndex+k];");
                        writer.WriteLine("                    " + m_instanceName + "_" + MethodL + "_EnableSensorLeft(s);");
                        writer.WriteLine("                }");
                        writer.WriteLine("            }");
                        writer.WriteLine("            ");
                        writer.WriteLine("            /* Set Flag that CapSensing in progres */");
                        writer.WriteLine("            " + m_instanceName + "_statusLeft |= " + m_instanceName + "_START_CAPSENSING;");
                        writer.WriteLine("            ");
                        writer.WriteLine("            /* Start PWM One Shout and PRS at one time */");
                        writer.WriteLine("            " + m_instanceName + "_CONTROL_LEFT |= " + m_instanceName + "_START_CAPSENSING;");
                    }
                    else
                    {
                        writer.WriteLine("            /* Set counter to zero */");
                        writer.WriteLine("            " + m_instanceName + "_lb" + MethodL + "_" + S_RAW + "_WriteCounter(0x0000u);");
                        writer.WriteLine("            ");
                        writer.WriteLine("            settlingTime = " + m_instanceName + "_SettingsTableLeft[l].SettlingTime;");
                        writer.WriteLine("            idacSettings = " + m_instanceName + "_SettingsTableLeft[l].IdacSettings;");
                        writer.WriteLine("            ");
                        writer.WriteLine("            /* Set Slot Settings */");
                        writer.WriteLine("            " + m_instanceName + "_" + MethodL + "_SetSlotSettingsLeft(l);");
                        writer.WriteLine("            ");
                        writer.WriteLine("            maxSns = " + m_instanceName + "_ScanSlotTableLeft[l].SnsCnt;");
                        writer.WriteLine("            startIndex = " + m_instanceName + "_ScanSlotTableLeft[l].IndexOffset;");
                        writer.WriteLine("            ");
                        writer.WriteLine("            /* Enable Sensors */");
                        writer.WriteLine("            if (maxSns == " + m_instanceName + "_ALONE_SENSOR)");
                        writer.WriteLine("            {");
                        writer.WriteLine("               " + m_instanceName + "_" + MethodL + "_EnableSensorLeft(startIndex);");
                        writer.WriteLine("            }");
                        writer.WriteLine("            else");
                        writer.WriteLine("            {");
                        writer.WriteLine("                for(k=0; k < maxSns; k++)");
                        writer.WriteLine("                {   ");
                        writer.WriteLine("                    s = " + m_instanceName + "_IndexTableLeft[startIndex+k];");
                        writer.WriteLine("                    " + m_instanceName + "_" + MethodL + "_EnableSensorLeft(s);");
                        writer.WriteLine("                }");
                        writer.WriteLine("            }");
                        writer.WriteLine("            ");
                        writer.WriteLine("            /* Disable csBuf */");
                        writer.WriteLine("            " + m_instanceName + "_CAPS_CFG0_LEFT &= ~" + m_instanceName + "_CSBUF_ENABLE;");
                        writer.WriteLine("            ");
                        writer.WriteLine("            /* Wait setling time */");
                        writer.WriteLine("            while(settlingTime--)");
                        writer.WriteLine("            {;}");
                        writer.WriteLine("          ");
                        writer.WriteLine("          /* Disconnect Idac and Sensor from AMUX */");
                        writer.WriteLine("          " + m_instanceName + "_lb" + MethodL + "_" + S_AMUX + "_Disconnect(l);");
                        writer.WriteLine("          " + m_instanceName + "_lb" + MethodL + "_" + S_IDAC + "_Stop();");
                        writer.WriteLine("          ");
                        writer.WriteLine("            /* Disable Sensors */");
                        writer.WriteLine("          state = " + m_instanceName + "_SettingsTableLeft[l].DisableState;");
                        writer.WriteLine("          if (maxSns == " + m_instanceName + "_ALONE_SENSOR)");
                        writer.WriteLine("            {");
                        writer.WriteLine("                " + m_instanceName + "_" + MethodL + "_DisableSensorLeft(startIndex, state);");
                        writer.WriteLine("            }");
                        writer.WriteLine("            else");
                        writer.WriteLine("            {");
                        writer.WriteLine("                for(k=0; k < maxSns; k++)");
                        writer.WriteLine("                {");
                        writer.WriteLine("                    s = " + m_instanceName + "_IndexTableLeft[startIndex+k];");
                        writer.WriteLine("                    " + m_instanceName + "_" + MethodL + "_DisableSensorLeft(s, state);");
                        writer.WriteLine("                }");
                        writer.WriteLine("            }");
                        writer.WriteLine("            ");
                        writer.WriteLine("            /* */");
                        writer.WriteLine("            " + m_instanceName + "_statusLeft |= " + m_instanceName + "_START_CAPSENSING;");
                        writer.WriteLine("            ");
                        writer.WriteLine("            " + m_instanceName + "_lb" + MethodL + "_" + S_IDAC + "_SetRange(" + m_instanceName + "_IDAC_RANGE_32uA);");
                        writer.WriteLine("            " + m_instanceName + "_lb" + MethodL + "_" + S_IDAC + "_SetValue(idacSettings);");
                        writer.WriteLine("            ");
                        writer.WriteLine("            ");
                        writer.WriteLine("          " + m_instanceName + "_lb" + MethodL + "_" + S_IDAC + "_Start();");
                        writer.WriteLine("            ");
                        writer.WriteLine("            /* Start Counter */");
                        writer.WriteLine("            " + m_instanceName + "_CONTROL_LEFT |= " + m_instanceName + "_START_CAPSENSING;");
                    }
                    writer.WriteLine("            ");
                    writer.WriteLine("            leftDone &= ~" + m_instanceName + "_START_CAPSENSING;");
                    writer.WriteLine("        }");
                    writer.WriteLine("        ");
                    writer.WriteLine("        if (rightDone)");
                    writer.WriteLine("        {");
                    // CSD RIGHT
                    if (packParams.m_localParams.m_listCsHalfs[1].m_Method == E_CAPSENSE_MODE.CSD)
                    {
                        writer.WriteLine("            /* Set Slot Settings  for Right  */");
                        writer.WriteLine("            " + m_instanceName + "_" + MethodR + "_SetSlotSettingsRight(r);");
                        writer.WriteLine("            ");
                        writer.WriteLine("            /* Disable csBuf Right */");
                        writer.WriteLine("            " + m_instanceName + "_CAPS_CFG0_RIGHT &= ~" + m_instanceName + "_CSBUF_ENABLE;");
                        writer.WriteLine("            ");
                        writer.WriteLine("            maxSns = " + m_instanceName + "_ScanSlotTableRight[r].SnsCnt;");
                        writer.WriteLine("            startIndex = " + m_instanceName + "_ScanSlotTableRight[r].IndexOffset;");
                        writer.WriteLine("            ");

                        if (packParams.m_localParams.m_listCsHalfs[1].m_csdSubMethod == E_CSD_SUB_METHODS.IDACSinking)
                        {
                            writer.WriteLine("            /* Connect Right IDAC */");
                            writer.WriteLine("            " + m_instanceName + "_rb" + MethodR + "_" + S_AMUX + "_Connect(" + m_instanceName + "_IDAC_CHANNEL_RIGHT);");
                            writer.WriteLine("            ");
                        }
                        else if (packParams.m_localParams.m_listCsHalfs[1].m_csdSubMethod == E_CSD_SUB_METHODS.IDACDisable_RB)
                        {
                            writer.WriteLine("            /* Connect Right DSI output to Rb */");
                            writer.WriteLine("            *" + m_instanceName + "_RbPortPinTableRight[" + m_instanceName + "_currentRbleedRight] |= " + m_instanceName + "_BYP_MASK;");
                            writer.WriteLine("            ");
                        }

                        writer.WriteLine("            /* Enable Sensors */");
                        writer.WriteLine("            if (maxSns == " + m_instanceName + "_ALONE_SENSOR)");
                        writer.WriteLine("            {");
                        writer.WriteLine("                " + m_instanceName + "_" + MethodR + "_EnableSensorRight(startIndex);");
                        writer.WriteLine("            }");
                        writer.WriteLine("            else");
                        writer.WriteLine("            {");
                        writer.WriteLine("                for(k=0; k < maxSns; k++)");
                        writer.WriteLine("                {");
                        writer.WriteLine("                    s = " + m_instanceName + "_IndexTableRight[startIndex+k];");
                        writer.WriteLine("                    " + m_instanceName + "_" + MethodR + "_EnableSensorRight(s);");
                        writer.WriteLine("                }");
                        writer.WriteLine("            }");
                        writer.WriteLine("            ");
                        writer.WriteLine("            /* Set Flag that CapSensing in progres */");
                        writer.WriteLine("            " + m_instanceName + "_statusRight |= " + m_instanceName + "_START_CAPSENSING;");
                        writer.WriteLine("            ");
                        writer.WriteLine("            /* Start PWM One Shout and PRS at one time */");
                        writer.WriteLine("            " + m_instanceName + "_CONTROL_RIGHT |= " + m_instanceName + "_START_CAPSENSING;");
                    }
                    else //CSA RIGHT
                    {
                        writer.WriteLine("            /* Set counter to zero */");
                        writer.WriteLine("            " + m_instanceName + "_rb" + MethodR + "_" + S_RAW + "_WriteCounter(0x0000u);");
                        writer.WriteLine("            ");
                        writer.WriteLine("            settlingTime = " + m_instanceName + "_SettingsTableRight[r].SettlingTime;");
                        writer.WriteLine("            idacSettings = " + m_instanceName + "_SettingsTableRight[r].IdacSettings;");
                        writer.WriteLine("            ");
                        writer.WriteLine("            /* Set Slot Settings */");
                        writer.WriteLine("            " + m_instanceName + "_" + MethodR + "_SetSlotSettingsRight(r);");
                        writer.WriteLine("            ");
                        writer.WriteLine("            maxSns = " + m_instanceName + "_ScanSlotTableRight[r].SnsCnt;");
                        writer.WriteLine("            startIndex = " + m_instanceName + "_ScanSlotTableRight[r].IndexOffset;");
                        writer.WriteLine("            ");
                        writer.WriteLine("            /* Enable Sensors */");
                        writer.WriteLine("            if (maxSns == " + m_instanceName + "_ALONE_SENSOR)");
                        writer.WriteLine("            {");
                        writer.WriteLine("               " + m_instanceName + "_" + MethodR + "_EnableSensorRight(startIndex);");
                        writer.WriteLine("            }");
                        writer.WriteLine("            else");
                        writer.WriteLine("            {");
                        writer.WriteLine("                for(k=0; k < maxSns; k++)");
                        writer.WriteLine("                {   ");
                        writer.WriteLine("                    s = " + m_instanceName + "_IndexTableRight[startIndex+k];");
                        writer.WriteLine("                    " + m_instanceName + "_" + MethodR + "_EnableSensorRight(s);");
                        writer.WriteLine("                }");
                        writer.WriteLine("            }");
                        writer.WriteLine("            ");
                        writer.WriteLine("            /* Disable csBuf */");
                        writer.WriteLine("            " + m_instanceName + "_CAPS_CFG0_RIGHT &= ~" + m_instanceName + "_CSBUF_ENABLE;");
                        writer.WriteLine("            ");
                        writer.WriteLine("            /* Wait setling time */");
                        writer.WriteLine("            while(settlingTime--)");
                        writer.WriteLine("            {;}");
                        writer.WriteLine("          ");
                        writer.WriteLine("          /* Disconnect Idac and Sensor from AMUX */");
                        writer.WriteLine("          " + m_instanceName + "_rb" + MethodR + "_" + S_AMUX + "_Disconnect(r);");
                        //writer.WriteLine("          " + m_instanceName + "_rb" + MethodR + m_csAmux + "_Disconnect(" + m_instanceName + "_IDAC_CHANNEL);");
                        writer.WriteLine("          " + m_instanceName + "_rb" + MethodR + "_" + S_IDAC + "_Stop();");
                        writer.WriteLine("          ");
                        writer.WriteLine("            /* Disable Sensors */");
                        writer.WriteLine("          state = " + m_instanceName + "_SettingsTableRight[r].DisableState;");
                        writer.WriteLine("          if (maxSns == " + m_instanceName + "_ALONE_SENSOR)");
                        writer.WriteLine("            {");
                        writer.WriteLine("                " + m_instanceName + "_" + MethodR + "_DisableSensorRight(startIndex, state);");
                        writer.WriteLine("            }");
                        writer.WriteLine("            else");
                        writer.WriteLine("            {");
                        writer.WriteLine("                for(k=0; k < maxSns; k++)");
                        writer.WriteLine("                {");
                        writer.WriteLine("                    s = " + m_instanceName + "_IndexTableRight[startIndex+k];");
                        writer.WriteLine("                    " + m_instanceName + "_" + MethodR + "_DisableSensorRight(s, state);");
                        writer.WriteLine("                }");
                        writer.WriteLine("            }");
                        writer.WriteLine("            ");
                        writer.WriteLine("            /* */");
                        writer.WriteLine("            " + m_instanceName + "_statusRight |= " + m_instanceName + "_START_CAPSENSING;");
                        writer.WriteLine("            ");
                        writer.WriteLine("            " + m_instanceName + "_rb" + MethodR + "_" + S_IDAC + "_SetRange(" + m_instanceName + "_IDAC_RANGE_32uA);");
                        writer.WriteLine("            " + m_instanceName + "_rb" + MethodR + "_" + S_IDAC + "_SetValue(idacSettings);");
                        writer.WriteLine("            ");
                        //writer.WriteLine("          " + m_instanceName + "_" + Symbol + "_" + m_csAmux + "_Connect(" + m_instanceName + "_IDAC_CHANNEL);");
                        writer.WriteLine("          " + m_instanceName + "_rb" + MethodR + "_" + S_IDAC + "_Start();");
                        writer.WriteLine("            ");
                        writer.WriteLine("            /* Start Counter */");
                        writer.WriteLine("            " + m_instanceName + "_CONTROL_RIGHT |= " + m_instanceName + "_START_CAPSENSING;");
                    }
                    writer.WriteLine("            ");
                    writer.WriteLine("            rightDone &= ~" + m_instanceName + "_START_CAPSENSING;");
                    writer.WriteLine("        }");
                    writer.WriteLine("          ");
                    writer.WriteLine("        /* Wait finish of CapSensing */");
                    writer.WriteLine("        while ((" + m_instanceName + "_statusLeft & " + m_instanceName + "_START_CAPSENSING) &&");
                    writer.WriteLine("               (" + m_instanceName + "_statusRight & " + m_instanceName + "_START_CAPSENSING) )");
                    writer.WriteLine("            { ; }");
                    writer.WriteLine("            ");
                    writer.WriteLine("        if (!(" + m_instanceName + "_statusLeft & " + m_instanceName + "_START_CAPSENSING))");
                    writer.WriteLine("        {");
                    // CSD LEFT complete
                    if (packParams.m_localParams.m_listCsHalfs[0].m_Method == E_CAPSENSE_MODE.CSD)
                    {
                        writer.WriteLine("            /* Stop Capsensing */");
                        writer.WriteLine("            " + m_instanceName + "_CONTROL_LEFT &= ~" + m_instanceName + "_START_CAPSENSING;");
                        writer.WriteLine("            ");
                        writer.WriteLine("            /* Read SlotResult from RwaCnt for Left side */");
                        writer.WriteLine("            rawIndex = " + m_instanceName + "_ScanSlotTableLeft[l].RawIndex;");
                        writer.WriteLine("            ");
                        writer.WriteLine("            /* Read SlotResult from RawCnt for Left side */");
                        writer.WriteLine("            " + m_instanceName + "_SlotResult[rawIndex] = " + m_instanceName + "_lb" + MethodL + "_" + S_RAW + "_ReadCounter();");
                        writer.WriteLine("            ");
                        writer.WriteLine("            startIndex = " + m_instanceName + "_ScanSlotTableLeft[l].IndexOffset;");
                        writer.WriteLine("            maxSns = " + m_instanceName + "_ScanSlotTableLeft[l].SnsCnt;");
                        writer.WriteLine("            ");
                        writer.WriteLine("            /* Disable Sensors */");
                        writer.WriteLine("            state = " + m_instanceName + "_SettingsTableLeft[l].DisableState;");
                        writer.WriteLine("            if (maxSns == " + m_instanceName + "_ALONE_SENSOR)");
                        writer.WriteLine("            {");
                        writer.WriteLine("                " + m_instanceName + "_" + MethodL + "_DisableSensorLeft(startIndex, state);");
                        writer.WriteLine("            }");
                        writer.WriteLine("            else");
                        writer.WriteLine("            {");
                        writer.WriteLine("                for(k=0; k < maxSns; k++)");
                        writer.WriteLine("                {");
                        writer.WriteLine("                    s = " + m_instanceName + "_IndexTableLeft[startIndex+k];");
                        writer.WriteLine("                    " + m_instanceName + "_" + MethodL + "_DisableSensorLeft(s, state);");
                        writer.WriteLine("                }");
                        writer.WriteLine("            }");
                        writer.WriteLine("             ");
                        if (packParams.m_localParams.m_listCsHalfs[0].IsIdacAvailible())
                        {
                            writer.WriteLine("            /* Turn off IDAC */");
                            writer.WriteLine("            " + m_instanceName + "_lb" + MethodL + "_" + S_IDAC + "_SetValue(" + m_instanceName + "_TURN_OFF_IDAC);");
                            writer.WriteLine("            ");
                        }
                        if (packParams.m_localParams.m_listCsHalfs[0].m_csdSubMethod == E_CSD_SUB_METHODS.IDACSinking)
                        {
                            writer.WriteLine("            /* Disconnect Left IDAC */");
                            writer.WriteLine("            " + m_instanceName + "_lb" + MethodL + "_" + S_AMUX + "_Disconnect(" + m_instanceName + "_IDAC_CHANNEL_LEFT);");
                            writer.WriteLine("            ");
                        }
                        else if (packParams.m_localParams.m_listCsHalfs[0].m_csdSubMethod == E_CSD_SUB_METHODS.IDACDisable_RB)
                        {
                            writer.WriteLine("            /* Disconnect Left DSI output to Rb */");
                            writer.WriteLine("            *" + m_instanceName + "_RbPortPinTableLeft[" + m_instanceName + "_currentRbleedLeft] &= ~" + m_instanceName + "_BYP_MASK;");
                            writer.WriteLine("            ");
                        }
                        writer.WriteLine("            /* Enable Vref on AMUX */");
                        writer.WriteLine("            " + m_instanceName + "_CAPS_CFG0_LEFT |= " + m_instanceName + "_CSBUF_ENABLE;");
                        writer.WriteLine("            ");
                        writer.WriteLine("            /* Scan enother scanslot */");
                        writer.WriteLine("            l++;");
                    }
                    else // CSA LEFT Scan Complete
                    {
                        writer.WriteLine("            /* Stop Counter */");
                        writer.WriteLine("            " + m_instanceName + "_CONTROL_LEFT &= ~" + m_instanceName + "_START_CAPSENSING;");
                        writer.WriteLine("            ");
                        writer.WriteLine("            /* Read SlotResult from RwaCnt for Left side */");
                        writer.WriteLine("            rawIndex = " + m_instanceName + "_ScanSlotTableLeft[l].RawIndex;");
                        writer.WriteLine("            ");
                        writer.WriteLine("            /* Read SlotResult from RawCnt */");
                        writer.WriteLine("            " + m_instanceName + "_SlotResult[rawIndex] = " + m_instanceName + "_lb" + MethodL + "_" + S_RAW + "_ReadCapture();");
                        writer.WriteLine("            status = " + m_instanceName + "_lb" + MethodL + "_" + S_RAW + "_ReadStatusRegister();");
                        writer.WriteLine("            ");
                        writer.WriteLine("            if ( status & " + m_instanceName + "_lb" + MethodL + "_" + S_RAW + "_STATUS_UNDERFLOW)");
                        writer.WriteLine("            {");
                        writer.WriteLine("                " + m_instanceName + "_SettingsTableLeft[l].IdacCodeBaseline = " + m_instanceName + "_" + MethodL + "_FindIdacCodeBaselineLeft(l);");
                        writer.WriteLine("                " + m_instanceName + "_statusLeft |= " + m_instanceName + "_RAW_OVERFLOW;");
                        writer.WriteLine("            }");
                        writer.WriteLine("            else if (" + m_instanceName + "_statusLeft & " + m_instanceName + "_RAW_OVERFLOW)");
                        writer.WriteLine("            {");
                        writer.WriteLine("                " + m_instanceName + "_CSHL_InitializeSlotBaselineLeft(l);");
                        writer.WriteLine("                " + m_instanceName + "_statusLeft &= ~" + m_instanceName + "_RAW_OVERFLOW;");
                        writer.WriteLine("            }");
                        writer.WriteLine("            else");
                        writer.WriteLine("            {");
                        writer.WriteLine("                " + m_instanceName + "_statusLeft &= ~" + m_instanceName + "_RAW_OVERFLOW;");
                        writer.WriteLine("                /* Scan enother scanslot */");
                        writer.WriteLine("                l++;");
                        writer.WriteLine("            }");
                        writer.WriteLine("            ");
                        writer.WriteLine("            /* Enable Vref on AMUX */");
                        writer.WriteLine("            " + m_instanceName + "_CAPS_CFG0_LEFT |= " + m_instanceName + "_CSBUF_ENABLE;");
                    }
                    writer.WriteLine("            ");
                    writer.WriteLine("            /* End of scanning */");
                    writer.WriteLine("            if (l < " + m_instanceName + "_TOTAL_SCANSLOT_COUNT_LEFT)");
                    writer.WriteLine("            {");
                    writer.WriteLine("                leftDone |= " + m_instanceName + "_START_CAPSENSING;");
                    writer.WriteLine("            }");
                    writer.WriteLine("            else");
                    writer.WriteLine("            {");
                    writer.WriteLine("                " + m_instanceName + "_statusLeft |= " + m_instanceName + "_START_CAPSENSING;");
                    writer.WriteLine("            }");
                    writer.WriteLine("        }");
                    writer.WriteLine("        ");
                    writer.WriteLine("        if (!(" + m_instanceName + "_statusRight & " + m_instanceName + "_START_CAPSENSING))");
                    writer.WriteLine("        {");
                    // CSD RIGHT complete
                    if (packParams.m_localParams.m_listCsHalfs[1].m_Method == E_CAPSENSE_MODE.CSD)
                    {
                        writer.WriteLine("            /* Stop Capsensing */");
                        writer.WriteLine("            " + m_instanceName + "_CONTROL_RIGHT &= ~" + m_instanceName + "_START_CAPSENSING;");
                        writer.WriteLine("            ");
                        writer.WriteLine("            /* Read SlotResult from RwaCnt  for Right side */");
                        writer.WriteLine("            rawIndex = " + m_instanceName + "_ScanSlotTableRight[r].RawIndex;");
                        writer.WriteLine("            ");
                        writer.WriteLine("            /* Read SlotResult from RwaCnt for Right side */");
                        writer.WriteLine("            " + m_instanceName + "_SlotResult[rawIndex] = " + m_instanceName + "_rb" + MethodR + "_" + S_RAW + "_ReadCounter();");
                        writer.WriteLine("              ");
                        writer.WriteLine("            startIndex = " + m_instanceName + "_ScanSlotTableRight[r].IndexOffset;");
                        writer.WriteLine("            maxSns = " + m_instanceName + "_ScanSlotTableRight[r].SnsCnt;");
                        writer.WriteLine("            ");
                        writer.WriteLine("            /* Disable Sensors */");
                        writer.WriteLine("            state = " + m_instanceName + "_SettingsTableRight[r].DisableState;");
                        writer.WriteLine("            if (maxSns == " + m_instanceName + "_ALONE_SENSOR)");
                        writer.WriteLine("            {");
                        writer.WriteLine("                " + m_instanceName + "_" + MethodR + "_DisableSensorRight(startIndex, state);");
                        writer.WriteLine("            }");
                        writer.WriteLine("            else");
                        writer.WriteLine("            {");
                        writer.WriteLine("                for(k=0; k < maxSns; k++)");
                        writer.WriteLine("                {");
                        writer.WriteLine("                    s = " + m_instanceName + "_IndexTableRight[startIndex+k];");
                        writer.WriteLine("                    " + m_instanceName + "_" + MethodR + "_DisableSensorRight(s, state);");
                        writer.WriteLine("                }");
                        writer.WriteLine("            }");
                        writer.WriteLine("            ");
                        if (packParams.m_localParams.m_listCsHalfs[1].IsIdacAvailible())
                        {
                            writer.WriteLine("            /* Turn off IDAC */");
                            writer.WriteLine("            " + m_instanceName + "_rb" + MethodR + "_" + S_IDAC + "_SetValue(" + m_instanceName + "_TURN_OFF_IDAC);");
                            writer.WriteLine("            ");
                        }
                        if (packParams.m_localParams.m_listCsHalfs[1].m_csdSubMethod == E_CSD_SUB_METHODS.IDACSinking)
                        {
                            writer.WriteLine("            /* Disconnect Right IDAC */");
                            writer.WriteLine("            " + m_instanceName + "_rb" + MethodR + "_" + S_AMUX + "_Disconnect(" + m_instanceName + "_IDAC_CHANNEL_RIGHT);");
                            writer.WriteLine("            ");
                        }
                        else if (packParams.m_localParams.m_listCsHalfs[1].m_csdSubMethod == E_CSD_SUB_METHODS.IDACDisable_RB)
                        {
                            writer.WriteLine("            /* Disconnect Right DSI output to Rb */");
                            writer.WriteLine("            *" + m_instanceName + "_RbPortPinTableRight[" + m_instanceName + "_currentRbleedRight] &= ~" + m_instanceName + "_BYP_MASK;");
                            writer.WriteLine("            ");
                        }
                        writer.WriteLine("            /* Enable Vref on AMUX */");
                        writer.WriteLine("            " + m_instanceName + "_CAPS_CFG0_RIGHT |= " + m_instanceName + "_CSBUF_ENABLE;");
                        writer.WriteLine("              ");
                        writer.WriteLine("            /* Scan enother scanslot */");
                        writer.WriteLine("            r++;");
                    }
                    else // CSA RIGHT Scan Complete
                    {
                        writer.WriteLine("            /* Stop Counter */");
                        writer.WriteLine("            " + m_instanceName + "_CONTROL_RIGHT &= ~" + m_instanceName + "_START_CAPSENSING;");
                        writer.WriteLine("            ");
                        writer.WriteLine("            /* Read SlotResult from RwaCnt  for Right side */");
                        writer.WriteLine("            rawIndex = " + m_instanceName + "_ScanSlotTableRight[r].RawIndex;");
                        writer.WriteLine("            ");
                        writer.WriteLine("            /* Read SlotResult from RawCnt */");
                        writer.WriteLine("            " + m_instanceName + "_SlotResult[rawIndex] = " + m_instanceName + "_rb" + MethodR + "_" + S_RAW + "_ReadCapture();");
                        writer.WriteLine("            status = " + m_instanceName + "_rb" + MethodR + "_" + S_RAW + "_ReadStatusRegister();");
                        writer.WriteLine("            ");
                        writer.WriteLine("            if ( status &" + m_instanceName + "_rb" + MethodR + "_" + S_RAW + "_STATUS_UNDERFLOW)");
                        writer.WriteLine("            {");
                        writer.WriteLine("                " + m_instanceName + "_SettingsTableRight[r].IdacCodeBaseline = " + m_instanceName + "_" + MethodR + "_FindIdacCodeBaselineRight(r);");
                        writer.WriteLine("                " + m_instanceName + "_statusRight |= " + m_instanceName + "_RAW_OVERFLOW;");
                        writer.WriteLine("            }");
                        writer.WriteLine("            else if (" + m_instanceName + "_statusRight & " + m_instanceName + "_RAW_OVERFLOW)");
                        writer.WriteLine("            {");
                        writer.WriteLine("                " + m_instanceName + "_CSHL_InitializeSlotBaselineRight(r);");
                        writer.WriteLine("                " + m_instanceName + "_statusRight &= ~" + m_instanceName + "_RAW_OVERFLOW;");
                        writer.WriteLine("            }");
                        writer.WriteLine("            else");
                        writer.WriteLine("            {");
                        writer.WriteLine("                " + m_instanceName + "_statusRight &= ~" + m_instanceName + "_RAW_OVERFLOW;");
                        writer.WriteLine("                /* Scan enother scanslot */");
                        writer.WriteLine("                r++;");
                        writer.WriteLine("            }");
                        writer.WriteLine("            ");
                        writer.WriteLine("            /* Enable Vref on AMUX */");
                        writer.WriteLine("            " + m_instanceName + "_CAPS_CFG0_RIGHT |= " + m_instanceName + "_CSBUF_ENABLE;");
                    }
                    writer.WriteLine("            ");
                    writer.WriteLine("            /* End of scanning */");
                    writer.WriteLine("            if (r < " + m_instanceName + "_TOTAL_SCANSLOT_COUNT_RIGHT)");
                    writer.WriteLine("            {");
                    writer.WriteLine("                rightDone |= " + m_instanceName + "_START_CAPSENSING;");
                    writer.WriteLine("            }");
                    writer.WriteLine("            else");
                    writer.WriteLine("            {");
                    writer.WriteLine("                " + m_instanceName + "_statusRight |= " + m_instanceName + "_START_CAPSENSING;");
                    writer.WriteLine("            }");
                    writer.WriteLine("        }");
                    writer.WriteLine("    }");
                    writer.WriteLine("    ");
                    writer.WriteLine("    " + m_instanceName + "_statusLeft &= ~" + m_instanceName + "_START_CAPSENSING;");
                    writer.WriteLine("    " + m_instanceName + "_statusRight &= ~" + m_instanceName + "_START_CAPSENSING;");
                    writer.WriteLine("}");
                    writer.WriteLine("");
                    writer.WriteLine("");
                    #endregion
                }
                else
                {
                    #region ScanAllSlots Synchron
                    writer.WriteLine("/*******************************************************************************");
                    writer.WriteLine("* Function Name: " + m_instanceName + "_ScanAllSlots");
                    writer.WriteLine("********************************************************************************");
                    writer.WriteLine("*");
                    writer.WriteLine("* Summary:");
                    writer.WriteLine("*  Scans slots from both sides in parallel. One slot from the right side and ");
                    writer.WriteLine("*  one slot from the left side are scanned at a time with synchronization.");
                    writer.WriteLine("*  If one side has more slots than the other then the remaining slots on ");
                    writer.WriteLine("*  the side with more are scanned singly. The scanning ends when all slots ");
                    writer.WriteLine("*  are scanned.");
                    writer.WriteLine("*");
                    writer.WriteLine("* Parameters:");
                    writer.WriteLine("*  void");
                    writer.WriteLine("*");
                    writer.WriteLine("* Return:");
                    writer.WriteLine("*  void");
                    writer.WriteLine("*");
                    writer.WriteLine("*******************************************************************************/");
                    writer.WriteLine("void " + m_instanceName + "_ScanAllSlots(void)");
                    writer.WriteLine("{");
                    writer.WriteLine("    uint8 l = 0, r = 0;");
                    writer.WriteLine("    uint8 k, s, state, maxSns, startIndex, rawIndex;");
                    if (packParams.m_localParams.m_listCsHalfs[0].m_Method == E_CAPSENSE_MODE.CSA)
                    {
                        writer.WriteLine("    uint8 settlingTime, idacSettings, status;");
                    }
                    writer.WriteLine("    ");
                    writer.WriteLine("    while ((l < " + m_instanceName + "_TOTAL_SCANSLOT_COUNT_LEFT) || (r < " + m_instanceName + "_TOTAL_SCANSLOT_COUNT_RIGHT))");
                    writer.WriteLine("    {");
                    writer.WriteLine("        if (l < " + m_instanceName + "_TOTAL_SCANSLOT_COUNT_LEFT) /* Start new scan of Left half */");
                    writer.WriteLine("        {");
                    writer.WriteLine("        ");
                    // CSD LEFT
                    if (packParams.m_localParams.m_listCsHalfs[0].m_Method == E_CAPSENSE_MODE.CSD)
                    {
                        writer.WriteLine("            /* Set Slot Settings  for Left */");
                        writer.WriteLine("            " + m_instanceName + "_" + MethodL + "_SetSlotSettingsLeft(l);");
                        writer.WriteLine("            ");
                        writer.WriteLine("            /* Disable csBuf Left */");
                        writer.WriteLine("            " + m_instanceName + "_CAPS_CFG0_LEFT &= ~" + m_instanceName + "_CSBUF_ENABLE;");
                        writer.WriteLine("            ");
                        writer.WriteLine("            maxSns = " + m_instanceName + "_ScanSlotTableLeft[l].SnsCnt;");
                        writer.WriteLine("            startIndex = " + m_instanceName + "_ScanSlotTableLeft[l].IndexOffset;");
                        writer.WriteLine("            ");

                        if (packParams.m_localParams.m_listCsHalfs[0].m_csdSubMethod == E_CSD_SUB_METHODS.IDACSinking)
                        {
                            writer.WriteLine("            /* Connect Left IDAC */");
                            writer.WriteLine("            " + m_instanceName + "_lb" + MethodL + "_" + S_AMUX + "_Connect(" + m_instanceName + "_IDAC_CHANNEL_LEFT);");
                            writer.WriteLine("            ");
                        }
                        else if (packParams.m_localParams.m_listCsHalfs[0].m_csdSubMethod == E_CSD_SUB_METHODS.IDACDisable_RB)
                        {
                            writer.WriteLine("            /* Connect Left DSI output to Rb */");
                            writer.WriteLine("            *" + m_instanceName + "_RbPortPinTableLeft[" + m_instanceName + "_currentRbleedLeft] |= " + m_instanceName + "_BYP_MASK;");
                            writer.WriteLine("            ");
                        }

                        writer.WriteLine("            /* Enable Sensors */");
                        writer.WriteLine("            if (maxSns == " + m_instanceName + "_ALONE_SENSOR)");
                        writer.WriteLine("            {");
                        writer.WriteLine("                " + m_instanceName + "_" + MethodL + "_EnableSensorLeft(startIndex);");
                        writer.WriteLine("            }");
                        writer.WriteLine("            else");
                        writer.WriteLine("            {");
                        writer.WriteLine("                for(k=0; k < maxSns; k++)");
                        writer.WriteLine("                {");
                        writer.WriteLine("                    s = " + m_instanceName + "_IndexTableLeft[startIndex+k];");
                        writer.WriteLine("                    " + m_instanceName + "_" + MethodL + "_EnableSensorLeft(s);");
                        writer.WriteLine("                }");
                        writer.WriteLine("            }");
                        writer.WriteLine("            ");
                        writer.WriteLine("            /* Set Flag that CapSensing in progres */");
                        writer.WriteLine("            " + m_instanceName + "_statusLeft |= " + m_instanceName + "_START_CAPSENSING;");
                        writer.WriteLine("            ");
                        writer.WriteLine("            /* Start PWM One Shout and PRS at one time */");
                        writer.WriteLine("            " + m_instanceName + "_CONTROL_LEFT |= " + m_instanceName + "_START_CAPSENSING;");
                    }
                    else // CSA LEFT
                    {
                        writer.WriteLine("            /* Set counter to zero */");
                        writer.WriteLine("            " + m_instanceName + "_lb" + MethodL + "_" + S_RAW + "_WriteCounter(0x0000u);");
                        writer.WriteLine("            ");
                        writer.WriteLine("            settlingTime = " + m_instanceName + "_SettingsTableLeft[l].SettlingTime;");
                        writer.WriteLine("            idacSettings = " + m_instanceName + "_SettingsTableLeft[l].IdacSettings;");
                        writer.WriteLine("            ");
                        writer.WriteLine("            ");
                        writer.WriteLine("            /* Set Slot Settings */");
                        writer.WriteLine("            " + m_instanceName + "_" + MethodL + "_SetSlotSettingsLeft(l);");
                        writer.WriteLine("            ");
                        writer.WriteLine("            maxSns = " + m_instanceName + "_ScanSlotTableLeft[l].SnsCnt;");
                        writer.WriteLine("            startIndex = " + m_instanceName + "_ScanSlotTableLeft[l].IndexOffset;");
                        writer.WriteLine("            ");
                        writer.WriteLine("            /* Enable Sensors Left */");
                        writer.WriteLine("            if (maxSns == " + m_instanceName + "_ALONE_SENSOR)");
                        writer.WriteLine("            {");
                        writer.WriteLine("               " + m_instanceName + "_" + MethodL + "_EnableSensorLeft(startIndex);");
                        writer.WriteLine("            }");
                        writer.WriteLine("            else");
                        writer.WriteLine("            {");
                        writer.WriteLine("                for(k=0; k < maxSns; k++)");
                        writer.WriteLine("                {   ");
                        writer.WriteLine("                    s = " + m_instanceName + "_IndexTableLeft[startIndex+k];");
                        writer.WriteLine("                    " + m_instanceName + "_" + MethodL + "_EnableSensorLeft(s);");
                        writer.WriteLine("                }");
                        writer.WriteLine("            }");
                        writer.WriteLine("            ");
                        writer.WriteLine("            /* Disable csBuf */");
                        writer.WriteLine("            " + m_instanceName + "_CAPS_CFG0_LEFT &= ~" + m_instanceName + "_CSBUF_ENABLE;");
                        writer.WriteLine("            ");
                        writer.WriteLine("            /* Wait setling time */");
                        writer.WriteLine("            while(settlingTime--)");
                        writer.WriteLine("            {;}");
                        writer.WriteLine("          ");
                        writer.WriteLine("          /* Disconnect Idac and Sensor from AMUX */");
                        writer.WriteLine("          " + m_instanceName + "_lb" + MethodL + "_" + S_AMUX + "_Disconnect(l);");
                        //writer.WriteLine("          " + m_instanceName + "_" + Symbol + "_" + m_csAmux + "_Disconnect(" + m_instanceName + "_IDAC_CHANNEL);");
                        writer.WriteLine("          " + m_instanceName + "_lb" + MethodL + "_" + S_IDAC + "_Stop();");
                        writer.WriteLine("          ");
                        writer.WriteLine("            /* Disable Sensors */");
                        writer.WriteLine("          state = " + m_instanceName + "_SettingsTableLeft[l].DisableState;");
                        writer.WriteLine("          if (maxSns == " + m_instanceName + "_ALONE_SENSOR)");
                        writer.WriteLine("            {");
                        writer.WriteLine("                " + m_instanceName + "_" + MethodL + "_DisableSensorLeft(startIndex, state);");
                        writer.WriteLine("            }");
                        writer.WriteLine("            else");
                        writer.WriteLine("            {");
                        writer.WriteLine("                for(k=0; k < maxSns; k++)");
                        writer.WriteLine("                {");
                        writer.WriteLine("                    s = " + m_instanceName + "_IndexTableLeft[startIndex+k];");
                        writer.WriteLine("                    " + m_instanceName + "_" + MethodL + "_DisableSensorLeft(s, state);");
                        writer.WriteLine("                }");
                        writer.WriteLine("            }");
                        writer.WriteLine("            ");
                        writer.WriteLine("            /* */");
                        writer.WriteLine("            " + m_instanceName + "_statusLeft |= " + m_instanceName + "_START_CAPSENSING;");
                        writer.WriteLine("            ");
                        writer.WriteLine("            " + m_instanceName + "_lb" + MethodL + "_" + S_IDAC + "_SetRange(" + m_instanceName + "_IDAC_RANGE_32uA);");
                        writer.WriteLine("            " + m_instanceName + "_lb" + MethodL + "_" + S_IDAC + "_SetValue(idacSettings);");
                        writer.WriteLine("            ");
                        writer.WriteLine("            ");
                        //writer.WriteLine("          " + m_instanceName + "_" + Symbol + "_" + m_csAmux + "_Connect(" + m_instanceName + "_IDAC_CHANNEL);");
                        writer.WriteLine("          " + m_instanceName + "_lb" + MethodL + "_" + S_IDAC + "_Start();");
                        writer.WriteLine("            ");
                        writer.WriteLine("            /* Start Counter */");
                        writer.WriteLine("            " + m_instanceName + "_CONTROL_LEFT |= " + m_instanceName + "_START_CAPSENSING;");
                        writer.WriteLine("            ");
                    }
                    writer.WriteLine("        }");
                    writer.WriteLine("        ");
                    writer.WriteLine("        if (r < " + m_instanceName + "_TOTAL_SCANSLOT_COUNT_RIGHT)");
                    writer.WriteLine("        {");
                    // CSD RIGHT
                    if (packParams.m_localParams.m_listCsHalfs[1].m_Method == E_CAPSENSE_MODE.CSD)
                    {
                        writer.WriteLine("            ");
                        writer.WriteLine("            /* Set Slot Settings  for Right  */");
                        writer.WriteLine("            " + m_instanceName + "_" + MethodR + "_SetSlotSettingsRight(r);");
                        writer.WriteLine("            ");
                        writer.WriteLine("            /* Disable csBuf Right */");
                        writer.WriteLine("            " + m_instanceName + "_CAPS_CFG0_RIGHT &= ~" + m_instanceName + "_CSBUF_ENABLE;");
                        writer.WriteLine("            ");
                        writer.WriteLine("            maxSns = " + m_instanceName + "_ScanSlotTableRight[r].SnsCnt;");
                        writer.WriteLine("            startIndex = " + m_instanceName + "_ScanSlotTableRight[r].IndexOffset;");
                        writer.WriteLine("            ");

                        if (packParams.m_localParams.m_listCsHalfs[1].m_csdSubMethod == E_CSD_SUB_METHODS.IDACSinking)
                        {
                            writer.WriteLine("            /* Connect Right IDAC */");
                            writer.WriteLine("            " + m_instanceName + "_rb" + MethodR + "_" + S_AMUX + "_Connect(" + m_instanceName + "_IDAC_CHANNEL_RIGHT);");
                            writer.WriteLine("            ");
                        }
                        else if (packParams.m_localParams.m_listCsHalfs[1].m_csdSubMethod == E_CSD_SUB_METHODS.IDACDisable_RB)
                        {
                            writer.WriteLine("            /* Connect Right DSI output to Rb */");
                            writer.WriteLine("            *" + m_instanceName + "_RbPortPinTableRight[" + m_instanceName + "_currentRbleedRight] |= " + m_instanceName + "_BYP_MASK;");
                            writer.WriteLine("            ");
                        }

                        writer.WriteLine("            /* Enable Sensors Right */");
                        writer.WriteLine("            if (maxSns == " + m_instanceName + "_ALONE_SENSOR)");
                        writer.WriteLine("            {");
                        writer.WriteLine("                " + m_instanceName + "_" + MethodR + "_EnableSensorRight(startIndex);");
                        writer.WriteLine("            }");
                        writer.WriteLine("            else");
                        writer.WriteLine("            {");
                        writer.WriteLine("                for(k=0; k < maxSns; k++)");
                        writer.WriteLine("                {");
                        writer.WriteLine("                    s = " + m_instanceName + "_IndexTableRight[startIndex+k];");
                        writer.WriteLine("                    " + m_instanceName + "_" + MethodR + "_EnableSensorRight(s);");
                        writer.WriteLine("                }");
                        writer.WriteLine("            }");
                        writer.WriteLine("            ");
                        writer.WriteLine("            /* Set Flag that CapSensing in progres */");
                        writer.WriteLine("            " + m_instanceName + "_statusRight |= " + m_instanceName + "_START_CAPSENSING;");
                        writer.WriteLine("            ");
                        writer.WriteLine("            /* Start PWM One Shout and PRS at one time */");
                        writer.WriteLine("            " + m_instanceName + "_CONTROL_RIGHT |= " + m_instanceName + "_START_CAPSENSING;");
                    }
                    else // CSA RIGHT
                    {
                        writer.WriteLine("            /* Set counter to zero */");
                        writer.WriteLine("            " + m_instanceName + "_rb" + MethodR + "_" + S_RAW + "_WriteCounter(0x0000u);");
                        writer.WriteLine("            ");
                        writer.WriteLine("            settlingTime = " + m_instanceName + "_SettingsTableRight[r].SettlingTime;");
                        writer.WriteLine("            idacSettings = " + m_instanceName + "_SettingsTableRight[r].IdacSettings;");
                        writer.WriteLine("            ");
                        writer.WriteLine("            ");
                        writer.WriteLine("            /* Set Slot Settings */");
                        writer.WriteLine("            " + m_instanceName + "_" + MethodR + "_SetSlotSettingsRight(r);");
                        writer.WriteLine("            ");
                        writer.WriteLine("            maxSns = " + m_instanceName + "_ScanSlotTableRight[r].SnsCnt;");
                        writer.WriteLine("            startIndex = " + m_instanceName + "_ScanSlotTableRight[r].IndexOffset;");
                        writer.WriteLine("            ");
                        writer.WriteLine("            /* Enable Sensors */");
                        writer.WriteLine("            if (maxSns == " + m_instanceName + "_ALONE_SENSOR)");
                        writer.WriteLine("            {");
                        writer.WriteLine("               " + m_instanceName + "_" + MethodR + "_EnableSensorRight(startIndex);");
                        writer.WriteLine("            }");
                        writer.WriteLine("            else");
                        writer.WriteLine("            {");
                        writer.WriteLine("                for(k=0; k < maxSns; k++)");
                        writer.WriteLine("                {   ");
                        writer.WriteLine("                    s = " + m_instanceName + "_IndexTableRight[startIndex+k];");
                        writer.WriteLine("                    " + m_instanceName + "_" + MethodR + "_EnableSensorRight(s);");
                        writer.WriteLine("                }");
                        writer.WriteLine("            }");
                        writer.WriteLine("            ");
                        writer.WriteLine("            /* Disable csBuf */");
                        writer.WriteLine("            " + m_instanceName + "_CAPS_CFG0_RIGHT &= ~" + m_instanceName + "_CSBUF_ENABLE;");
                        writer.WriteLine("            ");
                        writer.WriteLine("            /* Wait setling time */");
                        writer.WriteLine("            while(settlingTime--)");
                        writer.WriteLine("                {;}");
                        writer.WriteLine("          ");
                        writer.WriteLine("          /* Disconnect Idac and Sensor from AMUX */");
                        writer.WriteLine("          " + m_instanceName + "_rb" + MethodR + "_" + S_AMUX + "_Disconnect(r);");
                        //writer.WriteLine("          " + m_instanceName + "_rb" + MethodR + m_csAmux + "_Disconnect(" + m_instanceName + "_IDAC_CHANNEL);");
                        writer.WriteLine("          " + m_instanceName + "_rb" + MethodR + "_" + S_IDAC + "_Stop();");
                        writer.WriteLine("          ");
                        writer.WriteLine("            /* Disable Sensors */");
                        writer.WriteLine("          state = " + m_instanceName + "_SettingsTableRight[r].DisableState;");
                        writer.WriteLine("          if (maxSns == " + m_instanceName + "_ALONE_SENSOR)");
                        writer.WriteLine("            {");
                        writer.WriteLine("                " + m_instanceName + "_" + MethodR + "_DisableSensorRight(startIndex, state);");
                        writer.WriteLine("            }");
                        writer.WriteLine("            else");
                        writer.WriteLine("            {");
                        writer.WriteLine("                for(k=0; k < maxSns; k++)");
                        writer.WriteLine("                {");
                        writer.WriteLine("                    s = " + m_instanceName + "_IndexTableRight[startIndex+k];");
                        writer.WriteLine("                    " + m_instanceName + "_" + MethodR + "_DisableSensorRight(s, state);");
                        writer.WriteLine("                }");
                        writer.WriteLine("            }");
                        writer.WriteLine("            ");
                        writer.WriteLine("            /* */");
                        writer.WriteLine("            " + m_instanceName + "_statusRight |= " + m_instanceName + "_START_CAPSENSING;");
                        writer.WriteLine("            ");
                        writer.WriteLine("            " + m_instanceName + "_rb" + MethodR + "_" + S_IDAC + "_SetRange(" + m_instanceName + "_IDAC_RANGE_32uA);");
                        writer.WriteLine("            " + m_instanceName + "_rb" + MethodR + "_" + S_IDAC + "_SetValue(idacSettings);");
                        writer.WriteLine("            ");
                        writer.WriteLine("            ");
                        //writer.WriteLine("          " + m_instanceName + "_" + Symbol + "_" + m_csAmux + "_Connect(" + m_instanceName + "_IDAC_CHANNEL);");
                        writer.WriteLine("          " + m_instanceName + "_rb" + MethodR + "_" + S_IDAC + "_Start();");
                        writer.WriteLine("            ");
                        writer.WriteLine("            /* Start Counter */");
                        writer.WriteLine("            " + m_instanceName + "_CONTROL_RIGHT |= " + m_instanceName + "_START_CAPSENSING;");
                        writer.WriteLine("            ");
                    }
                    writer.WriteLine("        }");
                    writer.WriteLine("        ");
                    writer.WriteLine("        /* Wait finish of CapSensing */");
                    writer.WriteLine("        while ((" + m_instanceName + "_statusLeft & " + m_instanceName + "_START_CAPSENSING) ||");
                    writer.WriteLine("               (" + m_instanceName + "_statusRight & " + m_instanceName + "_START_CAPSENSING) )");
                    writer.WriteLine("        { ; }");
                    writer.WriteLine("        ");
                    writer.WriteLine("        if (l < " + m_instanceName + "_TOTAL_SCANSLOT_COUNT_LEFT)");
                    writer.WriteLine("        {");
                    // CSD LEFT Scan Complete
                    if (packParams.m_localParams.m_listCsHalfs[0].m_Method == E_CAPSENSE_MODE.CSD)
                    {
                        writer.WriteLine("            /* Stop Capsensing */");
                        writer.WriteLine("            " + m_instanceName + "_CONTROL_LEFT &= ~" + m_instanceName + "_START_CAPSENSING;");
                        writer.WriteLine("            ");
                        writer.WriteLine("            /* Read SlotResult from RwaCnt for Left side */");
                        writer.WriteLine("            rawIndex = " + m_instanceName + "_ScanSlotTableLeft[l].RawIndex;");
                        writer.WriteLine("            ");
                        writer.WriteLine("            /* Read SlotResult from RwaCnt for Left side */");
                        writer.WriteLine("            " + m_instanceName + "_SlotResult[rawIndex] = " + m_instanceName + "_lb" + MethodL + "_" + S_RAW + "_ReadCounter();");
                        writer.WriteLine("            ");
                        writer.WriteLine("            startIndex = " + m_instanceName + "_ScanSlotTableLeft[l].IndexOffset;");
                        writer.WriteLine("            maxSns = " + m_instanceName + "_ScanSlotTableLeft[l].SnsCnt;");
                        writer.WriteLine("            ");
                        writer.WriteLine("            /* Disable Sensors */");
                        writer.WriteLine("            state = " + m_instanceName + "_SettingsTableLeft[l].DisableState;");
                        writer.WriteLine("            if (maxSns == " + m_instanceName + "_ALONE_SENSOR)");
                        writer.WriteLine("            {");
                        writer.WriteLine("                " + m_instanceName + "_" + MethodL + "_DisableSensorLeft(startIndex, state);");
                        writer.WriteLine("            }");
                        writer.WriteLine("            else");
                        writer.WriteLine("            {");
                        writer.WriteLine("                for(k=0; k < maxSns; k++)");
                        writer.WriteLine("                {");
                        writer.WriteLine("                    s = " + m_instanceName + "_IndexTableLeft[startIndex+k];");
                        writer.WriteLine("                    " + m_instanceName + "_" + MethodL + "_DisableSensorLeft(s, state);");
                        writer.WriteLine("                }");
                        writer.WriteLine("            }");
                        writer.WriteLine("            ");
                        if (packParams.m_localParams.m_listCsHalfs[0].IsIdacAvailible())
                        {
                            writer.WriteLine("            /* Turn off IDAC */");
                            writer.WriteLine("            " + m_instanceName + "_lb" + MethodL + "_" + S_IDAC + "_SetValue(" + m_instanceName + "_TURN_OFF_IDAC);");
                            writer.WriteLine("            ");
                        }
                        if (packParams.m_localParams.m_listCsHalfs[0].m_csdSubMethod == E_CSD_SUB_METHODS.IDACSinking)
                        {
                            writer.WriteLine("            /* Disconnect Left IDAC */");
                            writer.WriteLine("            " + m_instanceName + "_lb" + MethodL + "_" + S_AMUX + "_Disconnect(" + m_instanceName + "_IDAC_CHANNEL_LEFT);");
                            writer.WriteLine("            ");
                        }
                        else if (packParams.m_localParams.m_listCsHalfs[0].m_csdSubMethod == E_CSD_SUB_METHODS.IDACDisable_RB)
                        {
                            writer.WriteLine("            /* Disconnect Left DSI output to Rb */");
                            writer.WriteLine("            *" + m_instanceName + "_RbPortPinTableLeft[" + m_instanceName + "_currentRbleedLeft] &= ~" + m_instanceName + "_BYP_MASK;");
                            writer.WriteLine("            ");
                        }
                        writer.WriteLine("            /* Enable Vref on AMUX */");
                        writer.WriteLine("            " + m_instanceName + "_CAPS_CFG0_LEFT |= " + m_instanceName + "_CSBUF_ENABLE;");
                        writer.WriteLine("            ");
                        writer.WriteLine("            /* Scan enother scanslot */");
                        writer.WriteLine("            l++;");
                    }
                    else // CSA LEFT Scan Complete
                    {
                        writer.WriteLine("            /* Stop Counter */");
                        writer.WriteLine("            " + m_instanceName + "_CONTROL_LEFT &= ~" + m_instanceName + "_START_CAPSENSING;");
                        writer.WriteLine("            ");
                        writer.WriteLine("            /* Read SlotResult from RwaCnt for Left side */");
                        writer.WriteLine("            rawIndex = " + m_instanceName + "_ScanSlotTableLeft[l].RawIndex;");
                        writer.WriteLine("            ");
                        writer.WriteLine("            /* Read SlotResult from RawCnt */");
                        writer.WriteLine("            " + m_instanceName + "_SlotResult[rawIndex] = " + m_instanceName + "_lb" + MethodL + "_" + S_RAW + "_ReadCapture();");
                        writer.WriteLine("            status = " + m_instanceName + "_lb" + MethodL + "_" + S_RAW + "_ReadStatusRegister();");
                        writer.WriteLine("            ");
                        writer.WriteLine("            if ( status & " + m_instanceName + "_lb" + MethodL + "_" + S_RAW + "_STATUS_UNDERFLOW)");
                        writer.WriteLine("            {");
                        writer.WriteLine("                   " + m_instanceName + "_SettingsTableLeft[l].IdacCodeBaseline = " + m_instanceName + "_" + MethodL + "_FindIdacCodeBaselineLeft(l);");
                        writer.WriteLine("                " + m_instanceName + "_statusLeft |= " + m_instanceName + "_RAW_OVERFLOW;");
                        writer.WriteLine("            }");
                        writer.WriteLine("            else if (" + m_instanceName + "_statusLeft & " + m_instanceName + "_RAW_OVERFLOW)");
                        writer.WriteLine("            {");
                        writer.WriteLine("                " + m_instanceName + "_CSHL_InitializeSlotBaselineLeft(l);");
                        writer.WriteLine("                " + m_instanceName + "_statusLeft &= ~" + m_instanceName + "_RAW_OVERFLOW;");
                        writer.WriteLine("            }");
                        writer.WriteLine("            else");
                        writer.WriteLine("            {");
                        writer.WriteLine("                " + m_instanceName + "_statusLeft &= ~" + m_instanceName + "_RAW_OVERFLOW;");
                        writer.WriteLine("                /* Scan enother scanslot */");
                        writer.WriteLine("                l++;");
                        writer.WriteLine("            }");
                        writer.WriteLine("            ");
                        writer.WriteLine("            /* Enable Vref on AMUX */");
                        writer.WriteLine("            " + m_instanceName + "_CAPS_CFG0_LEFT |= " + m_instanceName + "_CSBUF_ENABLE;");
                    }
                    writer.WriteLine("        }");
                    writer.WriteLine("        ");
                    writer.WriteLine("        if (r < " + m_instanceName + "_TOTAL_SCANSLOT_COUNT_RIGHT)");
                    writer.WriteLine("        {");
                    // CSD Right Scan Complete
                    if (packParams.m_localParams.m_listCsHalfs[1].m_Method == E_CAPSENSE_MODE.CSD)
                    {
                        writer.WriteLine("            /* Stop Capsensing */");
                        writer.WriteLine("            " + m_instanceName + "_CONTROL_RIGHT &= ~" + m_instanceName + "_START_CAPSENSING;");
                        writer.WriteLine("            ");
                        writer.WriteLine("            /* Read SlotResult from RwaCnt  for Right side */");
                        writer.WriteLine("            rawIndex = " + m_instanceName + "_ScanSlotTableRight[r].RawIndex;");
                        writer.WriteLine("            ");
                        writer.WriteLine("            /* Read SlotResult from RwaCnt for Right side */");
                        writer.WriteLine("            " + m_instanceName + "_SlotResult[rawIndex] = " + m_instanceName + "_rb" + MethodR + "_" + S_RAW + "_ReadCounter();");
                        writer.WriteLine("            ");
                        writer.WriteLine("            startIndex = " + m_instanceName + "_ScanSlotTableRight[r].IndexOffset;");
                        writer.WriteLine("            maxSns = " + m_instanceName + "_ScanSlotTableRight[r].SnsCnt;");
                        writer.WriteLine("            ");
                        writer.WriteLine("            /* Disable Sensors */");
                        writer.WriteLine("            state = " + m_instanceName + "_SettingsTableRight[r].DisableState;");
                        writer.WriteLine("            if (maxSns == " + m_instanceName + "_ALONE_SENSOR)");
                        writer.WriteLine("            {");
                        writer.WriteLine("                " + m_instanceName + "_" + MethodR + "_DisableSensorRight(startIndex, state);");
                        writer.WriteLine("            }");
                        writer.WriteLine("            else");
                        writer.WriteLine("            {");
                        writer.WriteLine("                for(k=0; k < maxSns; k++)");
                        writer.WriteLine("                {");
                        writer.WriteLine("                    s = " + m_instanceName + "_IndexTableRight[startIndex+k];");
                        writer.WriteLine("                    " + m_instanceName + "_" + MethodR + "_DisableSensorRight(s, state);");
                        writer.WriteLine("                }");
                        writer.WriteLine("            }");
                        writer.WriteLine("            ");
                        if (packParams.m_localParams.m_listCsHalfs[1].IsIdacAvailible())
                        {
                            writer.WriteLine("            /* Turn off IDAC */");
                            writer.WriteLine("            " + m_instanceName + "_rb" + MethodR + "_" + S_IDAC + "_SetValue(" + m_instanceName + "_TURN_OFF_IDAC);");
                            writer.WriteLine("            ");
                        }
                        if (packParams.m_localParams.m_listCsHalfs[1].m_csdSubMethod == E_CSD_SUB_METHODS.IDACSinking)
                        {
                            writer.WriteLine("            /* Disconnect Right IDAC */");
                            writer.WriteLine("            " + m_instanceName + "_rb" + MethodR + "_" + S_AMUX + "_Disconnect(" + m_instanceName + "_IDAC_CHANNEL_RIGHT);");
                            writer.WriteLine("            ");
                        }
                        else if (packParams.m_localParams.m_listCsHalfs[1].m_csdSubMethod == E_CSD_SUB_METHODS.IDACDisable_RB)
                        {
                            writer.WriteLine("            /* Disconnect Right DSI output to Rb */");
                            writer.WriteLine("            *" + m_instanceName + "_RbPortPinTableRight[" + m_instanceName + "_currentRbleedRight] &= ~" + m_instanceName + "_BYP_MASK;");
                            writer.WriteLine("            ");
                        }
                        writer.WriteLine("            /* Enable Vref on AMUX */");
                        writer.WriteLine("            " + m_instanceName + "_CAPS_CFG0_RIGHT |= " + m_instanceName + "_CSBUF_ENABLE;");
                        writer.WriteLine("            ");
                        writer.WriteLine("            /* Scan enother scanslot */");
                        writer.WriteLine("            r++;");
                    }
                    else // CSA RIGHT Scan Complete
                    {
                        writer.WriteLine("            /* Stop Counter */");
                        writer.WriteLine("            " + m_instanceName + "_CONTROL_RIGHT &= ~" + m_instanceName + "_START_CAPSENSING;");
                        writer.WriteLine("            ");
                        writer.WriteLine("            /* Read SlotResult from RwaCnt  for Right side */");
                        writer.WriteLine("            rawIndex = " + m_instanceName + "_ScanSlotTableRight[r].RawIndex;");
                        writer.WriteLine("            ");
                        writer.WriteLine("            /* Read SlotResult from RawCnt */");
                        writer.WriteLine("            " + m_instanceName + "_SlotResult[rawIndex] = " + m_instanceName + "_rb" + MethodR + "_" + S_RAW + "_ReadCapture();");
                        writer.WriteLine("            status = " + m_instanceName + "_rb" + MethodR + "_" + S_RAW + "_ReadStatusRegister();");
                        writer.WriteLine("            ");
                        writer.WriteLine("            if ( status & " + m_instanceName + "_rb" + MethodR + "_" + S_RAW + "_STATUS_UNDERFLOW)");
                        writer.WriteLine("            {");
                        writer.WriteLine("                " + m_instanceName + "_SettingsTableRight[r].IdacCodeBaseline = " + m_instanceName + "_" + MethodR + "_FindIdacCodeBaselineRight(r);");
                        writer.WriteLine("                " + m_instanceName + "_statusRight |= " + m_instanceName + "_RAW_OVERFLOW;");
                        writer.WriteLine("            }");
                        writer.WriteLine("            else if (" + m_instanceName + "_statusRight & " + m_instanceName + "_RAW_OVERFLOW)");
                        writer.WriteLine("            {");
                        writer.WriteLine("                " + m_instanceName + "_CSHL_InitializeSlotBaselineRight(r);");
                        writer.WriteLine("                " + m_instanceName + "_statusRight &= ~" + m_instanceName + "_RAW_OVERFLOW;");
                        writer.WriteLine("            }");
                        writer.WriteLine("            else");
                        writer.WriteLine("            {");
                        writer.WriteLine("                " + m_instanceName + "_statusRight &= ~" + m_instanceName + "_RAW_OVERFLOW;");
                        writer.WriteLine("                /* Scan enother scanslot */");
                        writer.WriteLine("                r++;");
                        writer.WriteLine("            }");
                        writer.WriteLine("            ");
                        writer.WriteLine("            /* Enable Vref on AMUX */");
                        writer.WriteLine("            " + m_instanceName + "_CAPS_CFG0_RIGHT |= " + m_instanceName + "_CSBUF_ENABLE;");
                    }
                    writer.WriteLine("        }");
                    writer.WriteLine("    }");
                    writer.WriteLine("}");
                    writer.WriteLine("");
                    writer.WriteLine("");
                    #endregion
                }
            }
            else
            {
                //Serial
                string Method = packParams.m_localParams.m_listCsHalfs[0].m_Method.ToString();
                #region ScanAllSlots
                writer.WriteLine("/*******************************************************************************");
                writer.WriteLine("* Function Name: " + m_instanceName + "_ScanAllSlots");
                writer.WriteLine("********************************************************************************");
                writer.WriteLine("*");
                writer.WriteLine("* Summary:");
                writer.WriteLine("*  Scans all slots by calling " + m_instanceName + "_" + Method + "_ScanAllSlots function for method");
                writer.WriteLine("*  selected in customizer.");
                writer.WriteLine("*");
                writer.WriteLine("* Parameters:");
                writer.WriteLine("*  void");
                writer.WriteLine("*");
                writer.WriteLine("* Return:");
                writer.WriteLine("*  void");
                writer.WriteLine("*");
                writer.WriteLine("*******************************************************************************/");
                writer.WriteLine("void " + m_instanceName + "_ScanAllSlots" + "(void)");
                writer.WriteLine("{");
                writer.WriteLine("    " + m_instanceName + "_" + Method + "_ScanAllSlots();");
                writer.WriteLine("}");
                writer.WriteLine("");
                writer.WriteLine("");
                #endregion

            }

        }
        #endregion

        #region apiCollectCFunctionForSide
        public void ApiCollectCFunctionForSide(ref TextWriter writer, CyGeneralParams packParams, CyAmuxBParams sbItem)
        {

            E_EL_SIDE side = sbItem.m_side;
            string sSide = GetSideName(side, packParams.Configuration);
            string sSideUp = GetSideNameUpper(side, packParams.Configuration);

            string Method = sbItem.m_Method.ToString();
            string Symbol = SymbolGenerate(packParams, sbItem, packParams.Configuration);
            string Idac_Polarity = Idac_PolarityGenerate(sbItem);
            //==========================
            string SW_CLK = GetSW_CLK(packParams, sbItem);
            string DIG_CLK = packParams.GetPrefixForSchematic(sbItem) + S_DIG_CLK;
            string csPRS = "cPRS" + sbItem.GetPRSResolution();
            string CLK = Get_CLK(packParams, sbItem);

            #region Idac
            if (sbItem.IsIdacAvailible() && (sbItem.m_Method == E_CAPSENSE_MODE.CSD))
            {

                if (packParams.Configuration == E_MAIN_CONFIG.emSerial)
                {
                    #region Idac_SetValue
                    writer.WriteLine("/*******************************************************************************");
                    writer.WriteLine("* Function Name: " + m_instanceName + "_" + Symbol + "_" + S_IDAC + "_SetValue");
                    writer.WriteLine("********************************************************************************");
                    writer.WriteLine("*");
                    writer.WriteLine("* Summary:");
                    writer.WriteLine("*  Sets DAC value");
                    writer.WriteLine("*");
                    writer.WriteLine("* Parameters:");
                    writer.WriteLine("*  value:  Sets DAC value between 0 and 255.");
                    writer.WriteLine("*");
                    writer.WriteLine("* Return:");
                    writer.WriteLine("*  void");
                    writer.WriteLine("*");
                    writer.WriteLine("*******************************************************************************/");
                    writer.WriteLine("void " + m_instanceName + "_" + Symbol + "_" + S_IDAC + "_SetValue(uint8 value)");
                    writer.WriteLine("{");
                    writer.WriteLine("    /*  Set Value  */");
                    writer.WriteLine("    " + m_instanceName + "_IDAC_DATA" + sSideUp + " = value;");
                    writer.WriteLine("    ");
                    writer.WriteLine("    #if defined(" + m_instanceName + "_IDAC_FIRST_SILICON" + sSideUp + ")");
                    writer.WriteLine("        " + m_instanceName + "_IDAC_DATA" + sSideUp + " = value;        /*  TODO: Need for first version of silicon */");
                    writer.WriteLine("    #endif");
                    writer.WriteLine("}");
                    writer.WriteLine("");
                    writer.WriteLine("");
                    #endregion
                }

                #region Trim
                writer.WriteLine("/*******************************************************************************");
                writer.WriteLine("* Function Name: " + m_instanceName + "_" + Symbol + "_" + S_IDAC + "_DacTrim");
                writer.WriteLine("********************************************************************************");
                writer.WriteLine("*");
                writer.WriteLine("* Summary:");
                writer.WriteLine("*  Sets the trim value for the given range.");
                writer.WriteLine("*");
                writer.WriteLine("* Parameters:");
                writer.WriteLine("*  value:  Sets DAC value between 0 and 255.");
                writer.WriteLine("* ");
                writer.WriteLine("* Return:");
                writer.WriteLine("*  void");
                writer.WriteLine("*");
                writer.WriteLine("*******************************************************************************/");
                writer.WriteLine("void " + m_instanceName + "_" + Symbol + "_" + S_IDAC + "_DacTrim(void)");
                writer.WriteLine("{");
                writer.WriteLine("    uint8 mode;");
                writer.WriteLine("    ");
                writer.WriteLine("    mode = ((" + m_instanceName + "_IDAC_CR0" + sSideUp + " & " + m_instanceName + "_IDAC_RANGE_MASK) >> 1);");
                writer.WriteLine("    if((" + m_instanceName + "_IDAC_IDIR_MASK & " + m_instanceName + "_IDAC_CR1" + sSideUp + ") == " + m_instanceName + "_IDAC_IDIR_SINK)");
                writer.WriteLine("    {");
                writer.WriteLine("        mode++;");
                writer.WriteLine("    }");
                writer.WriteLine("    ");
                writer.WriteLine("    " + m_instanceName + "_IDAC_TR" + sSideUp + " = CY_GET_XTND_REG8((uint8 *)(" + m_instanceName + "_DAC_TRIM_BASE" + sSideUp + " + mode));");
                writer.WriteLine("}");
                writer.WriteLine("");
                writer.WriteLine("");
                #endregion

                #region Idac_Start
                writer.WriteLine("/*******************************************************************************");
                writer.WriteLine("* Function Name: " + m_instanceName + "_" + Symbol + "_" + S_IDAC + "_Start");
                writer.WriteLine("********************************************************************************");
                writer.WriteLine("*");
                writer.WriteLine("* Summary:");
                writer.WriteLine("*  Sets power level then turn on IDAC8.");
                writer.WriteLine("*");
                writer.WriteLine("* Parameters:");
                writer.WriteLine("*  power:  Sets power level between off (0) and (3) high power");
                writer.WriteLine("*");
                writer.WriteLine("* Return:");
                writer.WriteLine("*  void");
                writer.WriteLine("*");
                writer.WriteLine("*******************************************************************************/");
                writer.WriteLine("void " + m_instanceName + "_" + Symbol + "_" + S_IDAC + "_Start(void)");
                writer.WriteLine("{");
                writer.WriteLine("    /* Hardware initiazation only needs to occure the first time */");
                writer.WriteLine("    if ( " + m_instanceName + "_idacInitVar" + sSide + " == 0)");
                writer.WriteLine("    {");
                writer.WriteLine("        " + m_instanceName + "_idacInitVar" + sSide + " = 1;");
                writer.WriteLine("        ");
                writer.WriteLine("        " + m_instanceName + "_IDAC_CR0" + sSideUp + " = " + m_instanceName + "_IDAC_MODE_I;");
                writer.WriteLine("        ");
                writer.WriteLine("        " + m_instanceName + "_IDAC_CR1" + sSideUp + " = " + Idac_Polarity + " | " + m_instanceName + "_IDAC_IDIR_CTL_UDB;");
                writer.WriteLine("        ");
                writer.WriteLine("        " + m_instanceName + "_" + Symbol + "_" + S_IDAC + "_SetValue(" + m_instanceName + "_TURN_OFF_IDAC);");
                writer.WriteLine("        ");
                writer.WriteLine("        " + m_instanceName + "_" + Symbol + "_" + S_IDAC + "_DacTrim();");
                writer.WriteLine("    }");
                writer.WriteLine("    ");
                writer.WriteLine("    /* Enable power to DAC */");
                writer.WriteLine("    " + m_instanceName + "_IDAC_PWRMGR" + sSideUp + " |= " + m_instanceName + "_IDAC_ACT_PWR_EN" + sSideUp + ";");
                writer.WriteLine("}");
                writer.WriteLine("");
                writer.WriteLine("");
                #endregion

                #region Idac_Stop
                writer.WriteLine("/*******************************************************************************");
                writer.WriteLine("* Function Name: " + m_instanceName + "_" + Symbol + "_" + S_IDAC + "_Stop");
                writer.WriteLine("********************************************************************************");
                writer.WriteLine("*");
                writer.WriteLine("* Summary:");
                writer.WriteLine("*  Powers down IDAC8 to lowest power state.");
                writer.WriteLine("*");
                writer.WriteLine("* Parameters:");
                writer.WriteLine("*   void");
                writer.WriteLine("*");
                writer.WriteLine("* Return:");
                writer.WriteLine("*  void");
                writer.WriteLine("*");
                writer.WriteLine("*******************************************************************************/");
                writer.WriteLine("void " + m_instanceName + "_" + Symbol + "_" + S_IDAC + "_Stop(void)");
                writer.WriteLine("{");
                writer.WriteLine("    #if defined(" + m_instanceName + "_IDAC_FIRST_SILICON" + sSideUp + ")");
                writer.WriteLine("        /* TODO: Work around for IDAC */");
                writer.WriteLine("        /* De-initiaize the IDAC */");
                writer.WriteLine("        " + m_instanceName + "_idacInitVar" + sSide + " = 0;");
                writer.WriteLine("        ");
                writer.WriteLine("        /* Change IDAC to VDAC mode */");
                writer.WriteLine("        " + m_instanceName + "_IDAC_CR0" + sSideUp + " &= ~" + m_instanceName + "_IDAC_MODE_I;");
                writer.WriteLine("    #endif");
                writer.WriteLine("    ");
                writer.WriteLine("    /* Disble power to DAC */");
                writer.WriteLine("    " + m_instanceName + "_IDAC_PWRMGR" + sSideUp + " &= ~" + m_instanceName + "_IDAC_ACT_PWR_EN" + sSideUp + ";");
                writer.WriteLine("}");
                writer.WriteLine("");
                writer.WriteLine("");
                #endregion

                #region Idac_SetRange
                writer.WriteLine("/*******************************************************************************");
                writer.WriteLine("* Function Name: " + m_instanceName + "_" + Symbol + "_" + S_IDAC + "_SetRange");
                writer.WriteLine("********************************************************************************");
                writer.WriteLine("*");
                writer.WriteLine("* Summary:");
                writer.WriteLine("*  Sets current range");
                writer.WriteLine("*");
                writer.WriteLine("* Parameters:");
                writer.WriteLine("*  Range:  Sets on of four valid ranges.");
                writer.WriteLine("*");
                writer.WriteLine("* Return:");
                writer.WriteLine("*  void");
                writer.WriteLine("*");
                writer.WriteLine("*******************************************************************************/");
                writer.WriteLine("void " + m_instanceName + "_" + Symbol + "_" + S_IDAC + "_SetRange(uint8 range)");
                writer.WriteLine("{");
                writer.WriteLine("    /* Clear existing mode */");
                writer.WriteLine("    " + m_instanceName + "_IDAC_CR0" + sSideUp + " &= ~" + m_instanceName + "_IDAC_RANGE_MASK; ");
                writer.WriteLine("    ");
                writer.WriteLine("    /*  Set Range  */");
                writer.WriteLine("    " + m_instanceName + "_IDAC_CR0" + sSideUp + " |= ( range & " + m_instanceName + "_IDAC_RANGE_MASK );");
                writer.WriteLine("    " + m_instanceName + "_" + Symbol + "_" + S_IDAC + "_DacTrim();");
                writer.WriteLine("}");
                writer.WriteLine("");
                writer.WriteLine("");
                #endregion

            }
            #endregion

            #region Rb
            if (sbItem.IsRbAvailible())
            {
                #region InitRb
                writer.WriteLine("/*******************************************************************************");
                writer.WriteLine("* Function Name: " + m_instanceName + "_" + Symbol + "_InitRb");
                writer.WriteLine("********************************************************************************");
                writer.WriteLine("*");
                writer.WriteLine("* Summary:");
                writer.WriteLine("*  Sets all Rbleed resistor to High-Z mode. Sets current Rbleed as first.");
                writer.WriteLine("*");
                writer.WriteLine("* Parameters:");
                writer.WriteLine("*  void");
                writer.WriteLine("*");
                writer.WriteLine("* Return:");
                writer.WriteLine("*  void");
                writer.WriteLine("*");
                writer.WriteLine("********************************************************************************/");
                writer.WriteLine("void " + m_instanceName + "_" + Symbol + "_InitRb(void)");
                writer.WriteLine("{");
                writer.WriteLine("    uint8 i;");
                writer.WriteLine("    uint8 rbCount = sizeof(" + m_instanceName + "_RbPortPinTable" + sSide + ")/sizeof(reg8 *);");
                writer.WriteLine("    ");
                writer.WriteLine("    /* Disable all Rb */");
                writer.WriteLine("    for(i=0; i < rbCount; i++)");
                writer.WriteLine("    {");
                writer.WriteLine("        /* Make High-Z */");
                writer.WriteLine("        *" + m_instanceName + "_RbPortPinTable" + sSide + "[i] = " + m_instanceName + "_PRT_PC_HIGHZ;");
                writer.WriteLine("    }");
                writer.WriteLine("    ");
                writer.WriteLine("    " + m_instanceName + "_currentRbleed" + sSide + " = " + m_instanceName + "_RBLEED1;");
                writer.WriteLine("}");
                writer.WriteLine("");
                writer.WriteLine("");
                #endregion

                #region SetRb
                writer.WriteLine("/*******************************************************************************");
                writer.WriteLine("* Function Name: " + m_instanceName + "_" + Method + "_SetRBleed" + sSide);
                writer.WriteLine("********************************************************************************");
                writer.WriteLine("*");
                writer.WriteLine("* Summary:");
                writer.WriteLine("*  Sets the pin to use for the bleed resistor (Rb) connection. The function");
                writer.WriteLine("*  overwrites the component parameter setting. This function is available only");
                writer.WriteLine("*  if Rb configuration is defined by the CapSense customizer.");
                writer.WriteLine("* ");
                writer.WriteLine("* Parameters:");
                writer.WriteLine("*  rbleed:  Ordering number for bleed resistor terminal defined in CapSense");
                writer.WriteLine("*  customizer.");
                writer.WriteLine("*");
                writer.WriteLine("* Return:");
                writer.WriteLine("*  void");
                writer.WriteLine("*");
                writer.WriteLine("*******************************************************************************/");
                writer.WriteLine("void " + m_instanceName + "_" + Method + "_SetRBleed" + sSide + "(uint8 rbleed)");
                writer.WriteLine("{");
                writer.WriteLine("    " + m_instanceName + "_currentRbleed" + sSide + " = rbleed;");
                writer.WriteLine("}");
                writer.WriteLine("");
                writer.WriteLine("");
                #endregion
            }
            #endregion

            #region Shield_Init
            if (sbItem.IsShieldElectrode())
            {
                writer.WriteLine("/*******************************************************************************");
                writer.WriteLine("* Function Name: " + m_instanceName + "_" + Method + "_Shield_Init");
                writer.WriteLine("********************************************************************************");
                writer.WriteLine("*");
                writer.WriteLine("* Summary:");
                writer.WriteLine("*  Sets appropraite pin settings for all Shield electrodes.");
                writer.WriteLine("*");
                writer.WriteLine("* Parameters:");
                writer.WriteLine("*  void");
                writer.WriteLine("*");
                writer.WriteLine("* Return:");
                writer.WriteLine("*  void");
                writer.WriteLine("*");
                writer.WriteLine("*******************************************************************************/");
                writer.WriteLine("void " + m_instanceName + "_" + Symbol + "_Shield_Init(void)");
                writer.WriteLine("{");
                writer.WriteLine("    uint8 port, shift, mask, i;");
                writer.WriteLine("    ");
                writer.WriteLine("    for(i=0; i<" + m_instanceName + "_TOTAL_SHIELD_COUNT" + sSideUp + "; i++)");
                writer.WriteLine("    {");
                writer.WriteLine("        port = " + m_instanceName + "_ShieldPortShiftTable" + sSide + "[i].port;");
                writer.WriteLine("        shift = " + m_instanceName + "_ShieldPortShiftTable" + sSide + "[i].shift;");
                writer.WriteLine("        mask = 1;");
                writer.WriteLine("        ");
                writer.WriteLine("        mask <<= shift;");
                writer.WriteLine("       /* Disable dig_glbl_ctl to sensor */");
                writer.WriteLine("       *(" + m_instanceName + "_BASE_PRTDSI_CAPS + (" + m_instanceName + "_PRTDSI_OFFSET*port)) &= ~mask;");
                writer.WriteLine("       ");
                writer.WriteLine("       /* Set drive mode Strong, bypass enable */    ");
                writer.WriteLine("       *(" + m_instanceName + "_BASE_PRT_PC + (" + m_instanceName + "_PRT_PC_OFFSET*port) + shift) = " + m_instanceName + "_PRT_PC_SHIELD;");
                writer.WriteLine("    }");
                writer.WriteLine("}");
                writer.WriteLine("");
                writer.WriteLine("");
            }
            #endregion

            #region FindIdacCodeBaseline
            if (sbItem.m_Method == E_CAPSENSE_MODE.CSA)
            {
                writer.WriteLine("/*-----------------------------------------------------------------------------");
                writer.WriteLine(" * FUNCTION NAME: void " + m_instanceName + "_" + Method + "_FindIdacCodeBaseline" + sSide + "(uint8 slot)");
                writer.WriteLine(" *-----------------------------------------------------------------------------");
                writer.WriteLine(" * Summary: ");
                writer.WriteLine(" *  Initializes registers and starts the CapSense Component. This function ");
                writer.WriteLine(" *  should be called prior to calling any other CapSense Component functions. ");
                writer.WriteLine(" *    ");
                writer.WriteLine(" * Parameters: ");
                writer.WriteLine(" *  None");
                writer.WriteLine(" *");
                writer.WriteLine(" * Return:");
                writer.WriteLine(" *  None");
                writer.WriteLine(" *");
                writer.WriteLine(" * Theory:");
                writer.WriteLine(" *  See summary");
                writer.WriteLine(" *");
                writer.WriteLine(" * Side Effects:");
                writer.WriteLine(" *  None");
                writer.WriteLine(" *");
                writer.WriteLine(" *---------------------------------------------------------------------------*/");
                writer.WriteLine("uint8 " + m_instanceName + "_" + Method + "_FindIdacCodeBaseline" + sSide + "(uint8 slot)");
                writer.WriteLine("{");
                writer.WriteLine("    uint8 settlingTime = " + m_instanceName + "_SettingsTable" + sSide + "[slot].SettlingTime;");
                writer.WriteLine("    uint8 idacRange = " + m_instanceName + "_SettingsTable" + sSide + "[slot].IdacRange;");
                if (sbItem.IsPrescaler())
                {
                    writer.WriteLine("    uint8 prescalerPeriod = " + m_instanceName + "_SettingsTable" + sSide + "[slot].PrescalerPeriod;");
                }
                writer.WriteLine("    uint8 maxSns = " + m_instanceName + "_ScanSlotTable" + sSide + "[slot].SnsCnt;");
                writer.WriteLine("    uint8 startIndex = " + m_instanceName + "_ScanSlotTable" + sSide + "[slot].IndexOffset;");
                writer.WriteLine("    uint8 i, s, state, tempIdacSettings = 0x80, scanWieght = 0x80;");
                writer.WriteLine("    ");
                writer.WriteLine("    /* Set Idac Range */");
                writer.WriteLine("    " + m_instanceName + "_" + Symbol + "_" + S_IDAC + "_SetRange(idacRange);");
                writer.WriteLine("    ");
                // Prescaler available for CSD and CSA
                if (sbItem.IsPrescaler())
                {
                    writer.WriteLine("    /* Set Prescaller */");
                    writer.WriteLine("    " + m_instanceName + "_" + SW_CLK + "_Stop();");
                    writer.WriteLine("    " + m_instanceName + "_" + SW_CLK + "_WriteCounter(prescalerPeriod);");
                    writer.WriteLine("    " + m_instanceName + "_" + SW_CLK + "_WritePeriod(prescalerPeriod);");
                    if (sbItem.m_Prescaler == E_PRESCALER.UDB)
                    {
                        writer.WriteLine("    " + m_instanceName + "_" + SW_CLK + "_WriteCompare(prescalerPeriod/2);");
                    }
                    writer.WriteLine("    " + m_instanceName + "_" + SW_CLK + "_Start();");
                    writer.WriteLine("    ");
                }
                writer.WriteLine("    do");
                writer.WriteLine("    {");
                writer.WriteLine("        /* Set Idac Value */");
                writer.WriteLine("        " + m_instanceName + "_" + Symbol + "_" + S_IDAC + "_SetValue(tempIdacSettings);");
                writer.WriteLine("        ");
                writer.WriteLine("        /* Disable csBuf */");
                writer.WriteLine("        " + m_instanceName + "_CAPS_CFG0" + sSideUp + " &= ~" + m_instanceName + "_CSBUF_ENABLE;");
                writer.WriteLine("        ");
                writer.WriteLine("        /* Enable Sensors */");
                writer.WriteLine("        if (maxSns == " + m_instanceName + "_ALONE_SENSOR)");
                writer.WriteLine("        {");
                writer.WriteLine("            " + m_instanceName + "_" + Method + "_EnableSensor" + sSide + "(startIndex);");
                writer.WriteLine("        }");
                writer.WriteLine("        else");
                writer.WriteLine("        {");
                writer.WriteLine("            for(i=0; i < maxSns; i++)");
                writer.WriteLine("            {");
                writer.WriteLine("                s = " + m_instanceName + "_IndexTable" + sSide + "[startIndex+i];");
                writer.WriteLine("                " + m_instanceName + "_" + Method + "_EnableSensor" + sSide + "(s);");
                writer.WriteLine("            }");
                writer.WriteLine("        }");
                writer.WriteLine("        ");
                writer.WriteLine("        /* Wait setling time */");
                writer.WriteLine("        while(settlingTime--)");
                writer.WriteLine("        {;}");
                writer.WriteLine("        ");
                writer.WriteLine("        /* Set/Clear bit */");
                writer.WriteLine("        if (" + m_instanceName + "_" + Symbol + "_" + S_COMP + "_WRK & " + m_instanceName + "_" + Symbol + "_" + S_COMP + "_CMP_OUT_MASK)");
                writer.WriteLine("        {");
                writer.WriteLine("            tempIdacSettings &= ~scanWieght;");
                writer.WriteLine("        }");
                writer.WriteLine("        ");
                writer.WriteLine("        scanWieght >>= 1;");
                writer.WriteLine("        ");
                writer.WriteLine("        tempIdacSettings |= scanWieght;");
                writer.WriteLine("        ");
                writer.WriteLine("        /* Disable Sensors */");
                writer.WriteLine("      state = " + m_instanceName + "_SettingsTable" + sSide + "[i].DisableState;");
                writer.WriteLine("      if (maxSns == " + m_instanceName + "_ALONE_SENSOR)");
                writer.WriteLine("        {");
                writer.WriteLine("            " + m_instanceName + "_" + Method + "_DisableSensor" + sSide + "(startIndex, state);");
                writer.WriteLine("        }");
                writer.WriteLine("        else");
                writer.WriteLine("        {");
                writer.WriteLine("            for(i=0; i < maxSns; i++)");
                writer.WriteLine("            {");
                writer.WriteLine("                s = " + m_instanceName + "_IndexTable" + sSide + "[startIndex+i];");
                writer.WriteLine("                " + m_instanceName + "_" + Method + "_DisableSensor" + sSide + "(s, state);");
                writer.WriteLine("            }");
                writer.WriteLine("        }");
                writer.WriteLine("        ");
                writer.WriteLine("        /* Enable Vref on AMUX */");
                writer.WriteLine("        " + m_instanceName + "_CAPS_CFG0" + sSideUp + " |= " + m_instanceName + "_CSBUF_ENABLE;");
                writer.WriteLine("    }    ");
                writer.WriteLine("    while(scanWieght);");
                writer.WriteLine("  ");
                writer.WriteLine("    return tempIdacSettings;");
                writer.WriteLine("}");
                writer.WriteLine("");
                writer.WriteLine("");
            }
            #endregion

            #region Start
            writer.WriteLine("/*******************************************************************************");
            writer.WriteLine("* Function Name: " + m_instanceName + "_" + Method + "_Start" + sSide);
            writer.WriteLine("********************************************************************************");
            writer.WriteLine("*");
            writer.WriteLine("* Summary:");
            writer.WriteLine("*  Initializes registers and starts the CapSense Component.");
            writer.WriteLine("*");
            writer.WriteLine("* Parameters:");
            writer.WriteLine("*  void");
            writer.WriteLine("*");
            writer.WriteLine("* Return:");
            writer.WriteLine("*  void");
            writer.WriteLine("*");
            writer.WriteLine("*******************************************************************************/");
            writer.WriteLine("void " + m_instanceName + "_" + Method + "_Start" + sSide + "(void)");
            writer.WriteLine("{");
            if (sbItem.m_Method == E_CAPSENSE_MODE.CSD) // CSD
            {
                if (sbItem.IsPrescaler())
                {
                    writer.WriteLine("    ");
                    writer.WriteLine("    /* Setup SW_CLK_Div_Serial */");
                    writer.WriteLine("    " + m_instanceName + "_" + SW_CLK + "_Start();");
                    writer.WriteLine("    ");
                }
                writer.WriteLine("    /* Setup DIG_CLK_DivSerial */");
                writer.WriteLine("    " + m_instanceName + "_" + DIG_CLK + "_Start();");
                writer.WriteLine("    ");
                writer.WriteLine("    /* Setup " + Symbol + "_csRawCnt */");
                writer.WriteLine("    " + m_instanceName + "_" + Symbol + "_" + S_RAW + "_Start();");
                writer.WriteLine("    ");
                writer.WriteLine("    /* Setup " + Symbol + "_csPWM */");
                writer.WriteLine("    " + m_instanceName + "_" + Symbol + "_" + S_PWM + "_Start();");
                writer.WriteLine("    ");
                writer.WriteLine("    /* Enable Comparator */");
                writer.WriteLine("    " + m_instanceName + "_" + Symbol + "_" + S_COMP + "_Start();");
                writer.WriteLine("    ");
                writer.WriteLine("    /* Initialize AMux */");
                writer.WriteLine("    " + m_instanceName + "_" + Symbol + "_" + S_AMUX + "_Start();");
                writer.WriteLine("    ");

                if (sbItem.IsIdacAvailible())
                {
                    writer.WriteLine("    /* Start Idac */");
                    writer.WriteLine("    " + m_instanceName + "_" + Symbol + "_" + S_IDAC + "_Start();");
                    writer.WriteLine("    ");
                }

                if (sbItem.IsPRS())
                {
                    writer.WriteLine("    /* Start PRS */");
                    writer.WriteLine("    " + m_instanceName + "_" + Symbol + "_" + csPRS + "_Start();");
                    writer.WriteLine("    ");
                }

                writer.WriteLine("    /* Connect Cmod, Cmp, Idac */");
                writer.WriteLine("    " + m_instanceName + "_" + Symbol + "_" + S_AMUX + "_Connect(" + m_instanceName + "_CMOD_CHANNEL" + sSideUp + ");");
                writer.WriteLine("    " + m_instanceName + "_" + Symbol + "_" + S_AMUX + "_Connect(" + m_instanceName + "_CMP_VP_CHANNEL" + sSideUp + ");");
                writer.WriteLine("    ");
                if (sbItem.m_csdSubMethod == E_CSD_SUB_METHODS.IDACSourcing)
                {
                    writer.WriteLine("    " + m_instanceName + "_" + Symbol + "_" + S_AMUX + "_Connect(" + m_instanceName + "_IDAC_CHANNEL" + sSideUp + ");");
                    writer.WriteLine("    ");
                }

                if (sbItem.IsRbAvailible())
                {
                    writer.WriteLine("    /* Enable Rbleed */");
                    writer.WriteLine("    " + m_instanceName + "_" + Symbol + "_InitRb();");
                    writer.WriteLine("    ");
                }

                if (sbItem.IsShieldElectrode())
                {
                    writer.WriteLine("    /* Set N Shield electrodes */");
                    writer.WriteLine("    " + m_instanceName + "_" + Symbol + "_Shield_Init();");
                    writer.WriteLine("    ");
                }
                writer.WriteLine("    /* Clear all Sensor */");
                writer.WriteLine("    " + m_instanceName + "_" + Method + "_ClearSlots" + sSide + "();");
                writer.WriteLine("    ");

                writer.WriteLine("    /* Setup ISR Component for CapSense */");
                writer.WriteLine("    CyIntDisable(" + m_instanceName + "_ISR_NUMBER" + sSideUp + ");");
                writer.WriteLine("    CyIntSetVector(" + m_instanceName + "_ISR_NUMBER" + sSideUp + ", " + m_instanceName + "_ISR" + sSide + ");");
                writer.WriteLine("    CyIntSetPriority(" + m_instanceName + "_ISR_NUMBER" + sSideUp + ", " + m_instanceName + "_ISR_PRIORITY" + sSideUp+ ");");
                writer.WriteLine("    CyIntEnable(" + m_instanceName + "_ISR_NUMBER" + sSideUp + ");");
                writer.WriteLine("    ");

                if (sbItem.m_VrefOptions == E_VREF_OPTIONS.Ref_Vdac)
                {
                    writer.WriteLine("    " + m_instanceName + "_" + Symbol + "_cRefVdac_Start();");
                    writer.WriteLine("    " + m_instanceName + "_" + Symbol + "_cRefVdac_SetValue(" + m_instanceName + "_VREF_VDAC_VALUE" + sSideUp + ");");
                    writer.WriteLine("    " + m_instanceName + "_CAPS_CFG0" + sSideUp + " |= " + m_instanceName + "_CSBUF_BOOST_ENABLE;");
                    writer.WriteLine("    ");
                }

                writer.WriteLine("    /* Enable csBuf */");
                writer.WriteLine("    " + m_instanceName + "_CSBUF_PWRMGR" + sSideUp + " |= " + m_instanceName + "_CSBUF_PWR_ENABLE" + sSideUp + ";");
                writer.WriteLine("    " + m_instanceName + "_CAPS_CFG0" + sSideUp + " |= " + m_instanceName + "_CSBUF_ENABLE;");

                // Clock start need only if it is Auto clock
                if (sbItem.m_currectClk != null)
                    if (sbItem.m_currectClk.IsDirectClock() == false)
                    {
                        writer.WriteLine("    ");
                        writer.WriteLine("    /* Enable the " + m_instanceName + " Clock */");
                        writer.WriteLine("    " + m_instanceName + "_" + CLK + "_Start();");
                    }

            }
            else // CSA 
            {
                writer.WriteLine("    uint8 i;");
                if (sbItem.IsPrescaler())
                {
                    writer.WriteLine("   ");
                    writer.WriteLine("    /* Setup SW_CLK_Div_Serial */");
                    writer.WriteLine("    " + m_instanceName + "_" + SW_CLK + "_Start();");
                    writer.WriteLine("    ");
                }

                writer.WriteLine("    /* Setup " + Symbol + "_csRawCnt */");
                writer.WriteLine("    " + m_instanceName + "_" + Symbol + "_" + S_RAW + "_Start();");
                writer.WriteLine("    " + m_instanceName + "_" + Symbol + "_" + S_RAW + "_ReadStatusRegister();");
                writer.WriteLine("    ");
                writer.WriteLine("    /* Enable Comparator */");
                writer.WriteLine("    " + m_instanceName + "_" + Symbol + "_" + S_COMP + "_Start();");
                writer.WriteLine("    ");
                writer.WriteLine("    /* Initialize AMux */");
                writer.WriteLine("    " + m_instanceName + "_" + Symbol + "_" + S_AMUX + "_Start();");
                writer.WriteLine("    ");
                writer.WriteLine("    /* Start Idac */");
                writer.WriteLine("    " + m_instanceName + "_" + Symbol + "_" + S_IDAC + "_Start();");
                writer.WriteLine("    ");
                writer.WriteLine("    /* Connect Cmod, Cmp, Idac */");
                writer.WriteLine("    " + m_instanceName + "_" + Symbol + "_" + S_AMUX + "_Connect(" + m_instanceName + "_CMOD_CHANNEL" + sSideUp + ");");
                writer.WriteLine("    " + m_instanceName + "_" + Symbol + "_" + S_AMUX + "_Connect(" + m_instanceName + "_CMP_VP_CHANNEL" + sSideUp + ");");
                writer.WriteLine("    " + m_instanceName + "_" + Symbol + "_" + S_AMUX + "_Connect(" + m_instanceName + "_IDAC_CHANNEL" + sSideUp + ");");
                writer.WriteLine("");

                if (sbItem.IsShieldElectrode())
                {
                    writer.WriteLine("    /* Set N Shield electrodes */");
                    writer.WriteLine("    " + m_instanceName + "_" + Method + "_Shield_Init" + sSide + "();");
                    writer.WriteLine("    ");
                }

                writer.WriteLine("    " + m_instanceName + "_" + Symbol + "_" + S_IDAC + "_Start();");
                writer.WriteLine("  ");
                writer.WriteLine("  /* Clear all Sensor */");
                writer.WriteLine("    " + m_instanceName + "_" + Method + "_ClearSlots" + sSide + "();");
                writer.WriteLine("  ");
                writer.WriteLine("    /* Setup ISR Component for CapSense */");
                writer.WriteLine("    CyIntSetPriority(" + m_instanceName + "_ISR_NUMBER" + sSideUp + ", " + m_instanceName + "_ISR_PRIORITY" + sSideUp + ");");
                writer.WriteLine("    CyIntSetVector(" + m_instanceName + "_ISR_NUMBER" + sSideUp + ", " + m_instanceName + "_ISR" + sSide + ");");
                writer.WriteLine("    CyIntClearPending(" + m_instanceName + "_ISR_NUMBER" + sSideUp + ");");
                writer.WriteLine("    CyIntEnable(" + m_instanceName + "_ISR_NUMBER" + sSideUp + ");");
                writer.WriteLine("  ");
                writer.WriteLine("  /* Enable csBuf */");
                writer.WriteLine("  " + m_instanceName + "_CSBUF_PWRMGR" + sSideUp + " |= " + m_instanceName + "_CSBUF_PWR_ENABLE" + sSideUp + ";");
                writer.WriteLine("  " + m_instanceName + "_CAPS_CFG0" + sSideUp + " |= " + m_instanceName + "_CSBUF_ENABLE;");

                // Clock start need only if it is Auto clock
                if (sbItem.m_currectClk != null)
                    if (sbItem.m_currectClk.IsDirectClock() == false)
                    {
                        writer.WriteLine("    ");
                        writer.WriteLine("  /* Enable the " + m_instanceName + " Clock */");
                        writer.WriteLine("  " + m_instanceName + "_" + CLK + "_Start();");
                    }

                writer.WriteLine("    ");
                writer.WriteLine("  for(i = 0; i< " + m_instanceName + "_TOTAL_SCANSLOT_COUNT" + sSideUp + "; i++)");
                writer.WriteLine("  {");
                writer.WriteLine("      " + m_instanceName + "_SettingsTable" + sSide + "[i].IdacCodeBaseline = " + m_instanceName + "_" + Method + "_FindIdacCodeBaseline" + sSide + "(i);");
                writer.WriteLine("  }");
            }
            writer.WriteLine("}");
            writer.WriteLine("");
            writer.WriteLine("");
            #endregion

            #region Stop
            writer.WriteLine("/*******************************************************************************");
            writer.WriteLine("* Function Name: " + m_instanceName + "_" + Method + "_Stop" + sSide);
            writer.WriteLine("********************************************************************************");
            writer.WriteLine("*");
            writer.WriteLine("* Summary:");
            writer.WriteLine("*  Stops the slot scanner, disables internal interrupts, and calls ");
            writer.WriteLine("*  CSD_ClearSlots() to reset all slots to an inactive state.");
            writer.WriteLine("*");
            writer.WriteLine("* Parameters:");
            writer.WriteLine("*  void");
            writer.WriteLine("*");
            writer.WriteLine("* Return:");
            writer.WriteLine("*  void");
            writer.WriteLine("*");
            writer.WriteLine("*******************************************************************************/");
            writer.WriteLine("void " + m_instanceName + "_" + Method + "_Stop" + sSide + "(void)");
            writer.WriteLine("{");
            if (sbItem.m_Method == E_CAPSENSE_MODE.CSD)
            {
                writer.WriteLine("    /* Stop Capsensing */");
                writer.WriteLine("    " + m_instanceName + "_CONTROL" + sSideUp + " &= ~" + m_instanceName + "_START_CAPSENSING;");
                writer.WriteLine("    ");
                writer.WriteLine("    /* Disable the CapSense interrupt */");
                writer.WriteLine("    CyIntEnable(" + m_instanceName + "_ISR_NUMBER" + sSideUp + ");");
                writer.WriteLine("    ");
                writer.WriteLine("    /* Clear all Sensor */");
                writer.WriteLine("    " + m_instanceName + "_" + Method + "_ClearSlots" + sSide + "();");
                writer.WriteLine("    ");
                writer.WriteLine("    /* Disable Comparator */");
                writer.WriteLine("    " + m_instanceName + "_" + Symbol + "_" + S_COMP + "_Stop();");
                writer.WriteLine("    ");
                if (sbItem.IsIdacAvailible())
                {
                    writer.WriteLine("    /* Disable power to IDAC */");
                    writer.WriteLine("    " + m_instanceName + "_" + Symbol + "_" + S_IDAC + "_Stop();");
                    writer.WriteLine("    ");
                }
                writer.WriteLine("    /* Disable csBuf */");
                writer.WriteLine("    " + m_instanceName + "_CAPS_CFG0" + sSideUp + " &= ~" + m_instanceName + "_CSBUF_ENABLE;");
                writer.WriteLine("    " + m_instanceName + "_CSBUF_PWRMGR" + sSideUp + " &= ~" + m_instanceName + "_CSBUF_PWR_ENABLE" + sSideUp + ";");
            }
            else
            {
                writer.WriteLine("    /* Stop Capsensing */");
                writer.WriteLine("    " + m_instanceName + "_CONTROL" + sSideUp + " &= ~" + m_instanceName + "_START_CAPSENSING;");
                writer.WriteLine("    ");
                writer.WriteLine("    /* Disable the CapSense interrupt */");
                writer.WriteLine("    CyIntEnable(" + m_instanceName + "_ISR_NUMBER" + sSideUp + ");");
                writer.WriteLine("    /* Clear all Sensor */");
                writer.WriteLine("    " + m_instanceName + "_" + Method + "_ClearSlots" + sSide + "();");
                writer.WriteLine("    ");
                writer.WriteLine("    /* Disable Comparator */");
                writer.WriteLine("    " + m_instanceName + "_" + Symbol + "_" + S_COMP + "_Stop();");
                writer.WriteLine("    ");
                writer.WriteLine("    /* Stop Idac */");
                writer.WriteLine("    " + m_instanceName + "_" + Symbol + "_" + S_IDAC + "_Stop();");
                writer.WriteLine("    ");
                writer.WriteLine("    /* Disable csBuf */");
                writer.WriteLine("    " + m_instanceName + "_CAPS_CFG0" + sSideUp + " &= ~" + m_instanceName + "_CSBUF_ENABLE;");
                writer.WriteLine("    " + m_instanceName + "_CSBUF_PWRMGR" + sSideUp + " &= ~" + m_instanceName + "_CSBUF_PWR_ENABLE" + sSideUp + ";");
            }
            writer.WriteLine("}");
            writer.WriteLine("");
            writer.WriteLine("");
            #endregion

            #region ScanSlot
            writer.WriteLine("/*******************************************************************************");
            writer.WriteLine("* Function Name: " + m_instanceName + "_" + Method + "_ScanSlot" + sSide);
            writer.WriteLine("********************************************************************************");
            writer.WriteLine("*");
            writer.WriteLine("* Summary:");
            writer.WriteLine("*  Sets scan settings and scans the selected scan slot. Each scan slot has");
            writer.WriteLine("*  a unique number within the slot array. This number is assigned by the");
            writer.WriteLine("*  CapSense customizer in sequence.");
            writer.WriteLine("*");
            writer.WriteLine("* Parameters:");
            writer.WriteLine("*  slot:  Scan slot number");
            writer.WriteLine("*");
            writer.WriteLine("* Return:");
            writer.WriteLine("*  void");
            writer.WriteLine("*");
            writer.WriteLine("*******************************************************************************/");
            writer.WriteLine("void " + m_instanceName + "_" + Method + "_ScanSlot" + sSide + "(uint8 slot)");
            writer.WriteLine("{");
            // CSD
            if (sbItem.m_Method == E_CAPSENSE_MODE.CSD)
            {
                writer.WriteLine("    uint8 i, s, state;");
                writer.WriteLine("    uint8 maxSns = " + m_instanceName + "_ScanSlotTable" + sSide + "[slot].SnsCnt;");
                writer.WriteLine("    uint8 startIndex = " + m_instanceName + "_ScanSlotTable" + sSide + "[slot].IndexOffset;");
                writer.WriteLine("    uint8 rawIndex = " + m_instanceName + "_ScanSlotTable" + sSide + "[slot].RawIndex; ");
                writer.WriteLine("    ");
                writer.WriteLine("    /* Set Slot Settings */");
                writer.WriteLine("    " + m_instanceName + "_" + Method + "_SetSlotSettings" + sSide + "(slot);");
                writer.WriteLine("    ");
                writer.WriteLine("    /* Disable CapSense Buffer */");
                writer.WriteLine("    " + m_instanceName + "_CAPS_CFG0" + sSideUp + " &= ~" + m_instanceName + "_CSBUF_ENABLE;    ");
                writer.WriteLine("    ");
                if (sbItem.m_csdSubMethod == E_CSD_SUB_METHODS.IDACSinking)
                {
                    writer.WriteLine("    /* Connect IDAC */");
                    writer.WriteLine("    " + m_instanceName + "_" + Symbol + "_" + S_AMUX + "_Connect(" + m_instanceName + "_IDAC_CHANNEL" + sSideUp + ");");
                    writer.WriteLine("    ");
                }
                else if (sbItem.m_csdSubMethod == E_CSD_SUB_METHODS.IDACDisable_RB)
                {
                    writer.WriteLine("    /* Connect DSI output to Rb */");
                    writer.WriteLine("    *" + m_instanceName + "_RbPortPinTable" + sSide + "[" + m_instanceName + "_currentRbleed" + sSide + "] |= " + m_instanceName + "_BYP_MASK;");
                    writer.WriteLine("    ");
                }
                writer.WriteLine("    /* Enable Sensors */");
                writer.WriteLine("    if (maxSns == " + m_instanceName + "_ALONE_SENSOR)");
                writer.WriteLine("    {");
                writer.WriteLine("        " + m_instanceName + "_" + Method + "_EnableSensor" + sSide + "(startIndex);");
                writer.WriteLine("    }");
                writer.WriteLine("    else");
                writer.WriteLine("    {");
                writer.WriteLine("        for(i=0; i < maxSns; i++)");
                writer.WriteLine("        {");
                writer.WriteLine("            s = " + m_instanceName + "_IndexTable" + sSide + "[startIndex+i];");
                writer.WriteLine("            " + m_instanceName + "_" + Method + "_EnableSensor" + sSide + "(s);");
                writer.WriteLine("        }");
                writer.WriteLine("    }");
                writer.WriteLine("    ");
                writer.WriteLine("    /* Set Flag CapSensing in progres */");
                writer.WriteLine("    " + m_instanceName + "_status" + sSide + " |= " + m_instanceName + "_START_CAPSENSING;");
                writer.WriteLine("    ");
                writer.WriteLine("    /* Start PWM One Shout and PRS at one time */");
                writer.WriteLine("    " + m_instanceName + "_CONTROL" + sSideUp + " |= " + m_instanceName + "_START_CAPSENSING;");
                writer.WriteLine("    ");
                writer.WriteLine("    /* Wait for finish CapSensing */");
                writer.WriteLine("    while (" + m_instanceName + "_status" + sSide + " == " + m_instanceName + "_START_CAPSENSING)");
                writer.WriteLine("    { ; }");
                writer.WriteLine("    ");
                writer.WriteLine("    /* Stop Capsensing */");
                writer.WriteLine("    " + m_instanceName + "_CONTROL" + sSideUp + " &= ~" + m_instanceName + "_START_CAPSENSING;");
                writer.WriteLine("    ");
                writer.WriteLine("    /* Read SlotResult from RawCnt */");
                writer.WriteLine("    " + m_instanceName + "_SlotResult[rawIndex] = " + m_instanceName + "_" + Symbol + "_" + S_RAW + "_ReadCounter();");
                writer.WriteLine("    ");
                writer.WriteLine("    /* Disable Sensors */");
                writer.WriteLine("    state = " + m_instanceName + "_SettingsTable" + sSide + "[slot].DisableState;");
                writer.WriteLine("    if (maxSns == " + m_instanceName + "_ALONE_SENSOR)");
                writer.WriteLine("    {");
                writer.WriteLine("        " + m_instanceName + "_" + Method + "_DisableSensor" + sSide + "(startIndex, state);");
                writer.WriteLine("    }");
                writer.WriteLine("    else");
                writer.WriteLine("    {");
                writer.WriteLine("        for(i=0; i < maxSns; i++)");
                writer.WriteLine("        {");
                writer.WriteLine("            s = " + m_instanceName + "_IndexTable" + sSide + "[startIndex+i];");
                writer.WriteLine("            " + m_instanceName + "_" + Method + "_DisableSensor" + sSide + "(s, state);");
                writer.WriteLine("        }");
                writer.WriteLine("    }");
                writer.WriteLine("    ");
                if (sbItem.IsIdacAvailible())
                {
                    writer.WriteLine("    /* Turn off IDAC */");
                    writer.WriteLine("    " + m_instanceName + "_" + Symbol + "_" + S_IDAC + "_SetValue(" + m_instanceName + "_TURN_OFF_IDAC);");
                    writer.WriteLine("    ");
                }
                if (sbItem.m_csdSubMethod == E_CSD_SUB_METHODS.IDACSinking)
                {
                    writer.WriteLine("    /* Disconnect IDAC */");
                    writer.WriteLine("    " + m_instanceName + "_" + Symbol + "_" + S_AMUX + "_Disconnect(" + m_instanceName + "_IDAC_CHANNEL" + sSideUp + ");");
                    writer.WriteLine("    ");
                }
                else if (sbItem.m_csdSubMethod == E_CSD_SUB_METHODS.IDACDisable_RB)
                {
                    writer.WriteLine("    /* Disconnect DSI output from Rb */");
                    writer.WriteLine("    *" + m_instanceName + "_RbPortPinTable" + sSide + "[" + m_instanceName + "_currentRbleed" + sSide + "] &= ~" + m_instanceName + "_BYP_MASK;");
                    writer.WriteLine("    ");
                }

                writer.WriteLine("    /* Enable Vref on AMUX */");
                writer.WriteLine("    " + m_instanceName + "_CAPS_CFG0" + sSideUp + " |= " + m_instanceName + "_CSBUF_ENABLE;");
            }
            else  //CSA
            {
                writer.WriteLine("  uint8 i, s, state, status;");
                writer.WriteLine("    uint8 maxSns = " + m_instanceName + "_ScanSlotTable" + sSide + "[slot].SnsCnt;");
                writer.WriteLine("    uint8 startIndex = " + m_instanceName + "_ScanSlotTable" + sSide + "[slot].IndexOffset;");
                writer.WriteLine("    uint8 rawIndex = " + m_instanceName + "_ScanSlotTable" + sSide + "[slot].RawIndex;");
                writer.WriteLine("    uint8 settlingTime, idacSettings;");
                writer.WriteLine("    ");
                writer.WriteLine("    do");
                writer.WriteLine("    {");
                // svoz_debug
                //writer.WriteLine("  sw_tp_Write(0x01); ");
                writer.WriteLine("        ");
                writer.WriteLine("        /* Set counter to zero */");
                writer.WriteLine("        " + m_instanceName + "_" + Symbol + "_" + S_RAW + "_WriteCounter(0x0000u);");
                writer.WriteLine("        ");
                writer.WriteLine("        settlingTime = " + m_instanceName + "_SettingsTable" + sSide + "[slot].SettlingTime;");
                writer.WriteLine("        idacSettings = " + m_instanceName + "_SettingsTable" + sSide + "[slot].IdacSettings;");
                writer.WriteLine("        ");
                writer.WriteLine("        ");
                writer.WriteLine("        /* Set Slot Settings */");
                writer.WriteLine("        " + m_instanceName + "_" + Method + "_SetSlotSettings" + sSide + "(slot);");
                writer.WriteLine("        ");
                writer.WriteLine("        /* Enable Sensors */");
                writer.WriteLine("        if (maxSns == " + m_instanceName + "_ALONE_SENSOR)");
                writer.WriteLine("        {");
                writer.WriteLine("            " + m_instanceName + "_" + Method + "_EnableSensor" + sSide + "(startIndex);");
                writer.WriteLine("        }");
                writer.WriteLine("        else");
                writer.WriteLine("        {");
                writer.WriteLine("            for(i=0; i < maxSns; i++)");
                writer.WriteLine("            {   ");
                writer.WriteLine("                s = " + m_instanceName + "_IndexTable" + sSide + "[startIndex+i];");
                writer.WriteLine("                " + m_instanceName + "_" + Method + "_EnableSensor" + sSide + "(s);");
                writer.WriteLine("            }");
                writer.WriteLine("        }");
                writer.WriteLine("        ");
                writer.WriteLine("        /* Disable csBuf */");
                writer.WriteLine("        " + m_instanceName + "_CAPS_CFG0" + sSideUp + " &= ~" + m_instanceName + "_CSBUF_ENABLE;");
                writer.WriteLine("      ");
                writer.WriteLine("        ");
                writer.WriteLine("        /* Wait setling time */");
                writer.WriteLine("        while(settlingTime--)");
                writer.WriteLine("        {;}");
                writer.WriteLine("      ");
                writer.WriteLine("      /* Disconnect Idac and Sensor from AMUX */");
                writer.WriteLine("      " + m_instanceName + "_" + Symbol + "_" + S_AMUX + "_Disconnect(slot);");
                //writer.WriteLine("      " + m_instanceName + "_" + Symbol + "_" + m_csAmux + "_Disconnect(" + m_instanceName + "_IDAC_CHANNEL);");
                writer.WriteLine("      " + m_instanceName + "_" + Symbol + "_" + S_IDAC + "_Stop();");
                writer.WriteLine("      ");
                writer.WriteLine("        /* Disable Sensors */");
                writer.WriteLine("      state = " + m_instanceName + "_SettingsTable" + sSide + "[slot].DisableState;");
                writer.WriteLine("      if (maxSns == " + m_instanceName + "_ALONE_SENSOR)");
                writer.WriteLine("        {");
                writer.WriteLine("            " + m_instanceName + "_" + Method + "_DisableSensor" + sSide + "(startIndex, state);");
                writer.WriteLine("        }");
                writer.WriteLine("        else");
                writer.WriteLine("        {");
                writer.WriteLine("            for(i=0; i < maxSns; i++)");
                writer.WriteLine("            {");
                writer.WriteLine("                s = " + m_instanceName + "_IndexTable" + sSide + "[startIndex+i];");
                writer.WriteLine("                " + m_instanceName + "_" + Method + "_DisableSensor" + sSide + "(s, state);");
                writer.WriteLine("            }");
                writer.WriteLine("        }");
                writer.WriteLine("        ");
                writer.WriteLine("        /* */");
                writer.WriteLine("        " + m_instanceName + "_status" + sSide + " |= " + m_instanceName + "_START_CAPSENSING;");
                writer.WriteLine("        ");
                writer.WriteLine("        " + m_instanceName + "_" + Symbol + "_" + S_IDAC + "_SetRange(" + m_instanceName + "_IDAC_RANGE_32uA);");
                writer.WriteLine("        " + m_instanceName + "_" + Symbol + "_" + S_IDAC + "_SetValue(idacSettings);");
                writer.WriteLine("        ");
                writer.WriteLine("        ");
                //writer.WriteLine("      " + m_instanceName + "_" + Symbol + "_" + m_csAmux + "_Connect(" + m_instanceName + "_IDAC_CHANNEL);");
                writer.WriteLine("      " + m_instanceName + "_" + Symbol + "_" + S_IDAC + "_Start();");
                writer.WriteLine("        ");
                writer.WriteLine("        /* Start Counter */");
                writer.WriteLine("        " + m_instanceName + "_CONTROL" + sSideUp + " |= " + m_instanceName + "_START_CAPSENSING;");
                writer.WriteLine("        ");
                writer.WriteLine("        /* Wait finish of CapSensing */");
                writer.WriteLine("        while (" + m_instanceName + "_status" + sSide + " == " + m_instanceName + "_START_CAPSENSING)");
                writer.WriteLine("        {;}");
                writer.WriteLine("        ");
                writer.WriteLine("        /* Stop Counter */");
                writer.WriteLine("        " + m_instanceName + "_CONTROL" + sSideUp + " &= ~" + m_instanceName + "_START_CAPSENSING;");
                writer.WriteLine("        ");
                writer.WriteLine("        /* Read SlotResult from RawCnt */");
                writer.WriteLine("        " + m_instanceName + "_SlotResult[rawIndex] = " + m_instanceName + "_" + Symbol + "_" + S_RAW + "_ReadCapture();");
                writer.WriteLine("        status = " + m_instanceName + "_" + Symbol + "_" + S_RAW + "_ReadStatusRegister();");
                writer.WriteLine("        ");
                writer.WriteLine("        if ( status &" + m_instanceName + "_" + Symbol + "_" + S_RAW + "_STATUS_UNDERFLOW)");
                writer.WriteLine("        {            ");
                writer.WriteLine("            " + m_instanceName + "_SettingsTable" + sSide + "[slot].IdacCodeBaseline = " + m_instanceName + "_" + Method + "_FindIdacCodeBaseline" + sSide + "(slot);");
                writer.WriteLine("            " + m_instanceName + "_status" + sSide + " |= " + m_instanceName + "_RAW_OVERFLOW;");
                writer.WriteLine("        }");
                writer.WriteLine("        else if (" + m_instanceName + "_status" + sSide + " & " + m_instanceName + "_RAW_OVERFLOW)");
                writer.WriteLine("        {");
                writer.WriteLine("            " + m_instanceName + "_CSHL_InitializeSlotBaseline" + sSide + "(slot);");
                writer.WriteLine("            " + m_instanceName + "_status" + sSide + " &= ~" + m_instanceName + "_RAW_OVERFLOW;");
                writer.WriteLine("        }");
                writer.WriteLine("        else");
                writer.WriteLine("        {");
                writer.WriteLine("            " + m_instanceName + "_status" + sSide + " &= ~" + m_instanceName + "_RAW_OVERFLOW;");
                writer.WriteLine("        }");
                writer.WriteLine("        ");
                writer.WriteLine("        /* Enable Vref on AMUX */");
                writer.WriteLine("        " + m_instanceName + "_CAPS_CFG0" + sSideUp + " |= " + m_instanceName + "_CSBUF_ENABLE;");
                writer.WriteLine("    }");
                writer.WriteLine("    while(" + m_instanceName + "_status" + sSide + " & " + m_instanceName + "_RAW_OVERFLOW);");
                writer.WriteLine("    ");
                // svoz_debug
                //writer.WriteLine("  sw_tp_Write(0x010); ");
            }
            writer.WriteLine("}");
            writer.WriteLine("");
            writer.WriteLine("");
            #endregion

            #region ScanAllSlots
            writer.WriteLine("/*******************************************************************************");
            writer.WriteLine("* Function Name: " + m_instanceName + "_" + Method + "_ScanAllSlots" + sSide);
            writer.WriteLine("********************************************************************************");
            writer.WriteLine("*");
            writer.WriteLine("* Summary:");
            writer.WriteLine("*  Scans all scan slots by calling" + m_instanceName + "_" + Method + "_ScanSlot" + sSide + " for each");
            writer.WriteLine("*  slot index.");
            writer.WriteLine("*");
            writer.WriteLine("* Parameters:");
            writer.WriteLine("*  void");
            writer.WriteLine("*");
            writer.WriteLine("* Return:");
            writer.WriteLine("*  void");
            writer.WriteLine("*");
            writer.WriteLine("*******************************************************************************/");
            writer.WriteLine("void " + m_instanceName + "_" + Method + "_ScanAllSlots" + sSide + "(void)");
            writer.WriteLine("{");
            writer.WriteLine("    uint8 i;");
            writer.WriteLine("    ");
            writer.WriteLine("    for(i=0; i < " + m_instanceName + "_TOTAL_SCANSLOT_COUNT" + sSideUp + ";i++)");
            writer.WriteLine("    {");
            writer.WriteLine("         " + m_instanceName + "_" + Method + "_ScanSlot" + sSide + "(i);");
            writer.WriteLine("    }");
            writer.WriteLine("}");
            writer.WriteLine("");
            writer.WriteLine("");
            #endregion

            #region SetSlotSettings
            writer.WriteLine("/*******************************************************************************");
            writer.WriteLine("* Function Name: " + m_instanceName + "_" + Method + "_SetSlotSettings" + sSide);
            writer.WriteLine("********************************************************************************");
            writer.WriteLine("*");
            writer.WriteLine("* Summary:");
            writer.WriteLine("*  Sets the scan settings of selected scan slot. Each setting has a unique");
            writer.WriteLine("*  number within the settings array and is connected to corresponding scan slot.");
            writer.WriteLine("*  This number is assigned by the CapSense customizer in sequence.");
            writer.WriteLine("*");
            writer.WriteLine("* Parameters:");
            writer.WriteLine("*  slot:  Scan slot number");
            writer.WriteLine("*");
            writer.WriteLine("* Return:");
            writer.WriteLine("*  void");
            writer.WriteLine("*");
            writer.WriteLine("*******************************************************************************/");
            writer.WriteLine("void " + m_instanceName + "_" + Method + "_SetSlotSettings" + sSide + "(uint8 slot)");
            writer.WriteLine("{");
            /* CSD only paramters: scanSpeed, resolution, prsPolynomial */
            if (sbItem.m_Method == E_CAPSENSE_MODE.CSD)
            {
                writer.WriteLine("    uint8 scanSpeed = " + m_instanceName + "_SettingsTable" + sSide + "[slot].ScanSpeed;");
                writer.WriteLine("    uint16 resolution = ((uint16) " + m_instanceName + "_SettingsTable" + sSide + "[slot].Resolution << " + m_instanceName + @"_PWM_WINDOW_SHIFT)\");
                writer.WriteLine("                        | " + m_instanceName + "_PWM_RESOLUTION_16_BITS;");

                if (sbItem.IsPRS())
                {
                    string strPrsVal = sbItem.GetPRSResolution();
                    writer.WriteLine("    uint" + strPrsVal + " prsPolynomial = " + m_instanceName + "_SettingsTable" + sSide + "[slot].PrsPolynomial;");
                }
            }

            /* Prescaler period */
            if (sbItem.IsPrescaler())
            {
                writer.WriteLine("    uint8 prescalerPeriod = " + m_instanceName + "_SettingsTable" + sSide + "[slot].PrescalerPeriod;");
            }

            /* Idac settings CSD and CSA */
            if ((sbItem.IsIdacAvailible()) && (sbItem.m_Method == E_CAPSENSE_MODE.CSD))
            {
                writer.WriteLine("    uint8 idacRange = " + m_instanceName + "_SettingsTable" + sSide + "[slot].IdacRange;");
                writer.WriteLine("    uint8 idacSettings = " + m_instanceName + "_SettingsTable" + sSide + "[slot].IdacSettings;");
            }
            else if (sbItem.m_Method == E_CAPSENSE_MODE.CSA)
            {
                writer.WriteLine("    uint8 idacRange = " + m_instanceName + "_SettingsTable" + sSide + "[slot].IdacRange;");
                writer.WriteLine("    uint8 idacCode = " + m_instanceName + "_SettingsTable" + sSide + "[slot].IdacCodeBaseline - " + m_instanceName + "_SettingsTable" + sSide + "[slot].ScanLength;");
            }

            writer.WriteLine("    ");

            /* CSD only settings - PWM Resolution, DIG_CLK Devider, PRS Polynomial */
            if (sbItem.m_Method == E_CAPSENSE_MODE.CSD)
            {
                writer.WriteLine("    /* PWM Resolution */");
                writer.WriteLine("    " + m_instanceName + "_" + Symbol + "_" + S_PWM + "_WritePeriod(resolution);");
                writer.WriteLine("    " + m_instanceName + "_" + Symbol + "_" + S_PWM + "_WriteCompare(resolution);");
                writer.WriteLine("    ");

                writer.WriteLine("    /* Reset RawCnt and rearm PWM */");
                writer.WriteLine("    " + m_instanceName + "_CONTROL" + sSideUp + " |= " + m_instanceName + "_RESET_PWM_CNTR;");
                writer.WriteLine("    ");
                writer.WriteLine("    /* Set ScanSpeed */");
                writer.WriteLine("    " + m_instanceName + "_" + DIG_CLK + "_Stop();");
                writer.WriteLine("    " + m_instanceName + "_" + DIG_CLK + "_WriteCounter(scanSpeed);");
                writer.WriteLine("    " + m_instanceName + "_" + DIG_CLK + "_WritePeriod(scanSpeed);");
                writer.WriteLine("    " + m_instanceName + "_" + DIG_CLK + "_Start();");
                writer.WriteLine("    ");
                writer.WriteLine("    " + m_instanceName + "_CONTROL" + sSideUp + " &= ~" + m_instanceName + "_RESET_PWM_CNTR;");
                writer.WriteLine("    ");

                if (sbItem.IsPRS())
                {
                    writer.WriteLine("    /* PRS Polynomial */");
                    writer.WriteLine("    " + m_instanceName + "_" + Symbol + "_" + csPRS + "_WritePolynomial(prsPolynomial);");
                    string strPrsVal="FF";
                    if (sbItem.m_prs == E_PRS_OPTIONS._16bits) strPrsVal += "FF";
                    writer.WriteLine("    " + m_instanceName + "_" + Symbol + "_" + csPRS + "_WriteSeed(0x" + strPrsVal + "u);");
                    writer.WriteLine("    ");
                }
            }

            /* Prescaler available for CSD and CSA*/
            if (sbItem.IsPrescaler())
            {
                writer.WriteLine("    /* Set Prescaller */");
                writer.WriteLine("    " + m_instanceName + "_" + SW_CLK + "_Stop();");
                writer.WriteLine("    " + m_instanceName + "_" + SW_CLK + "_WriteCounter(prescalerPeriod);");
                writer.WriteLine("    " + m_instanceName + "_" + SW_CLK + "_WritePeriod(prescalerPeriod);");
                if (sbItem.m_Prescaler == E_PRESCALER.UDB)
                {
                    writer.WriteLine("    " + m_instanceName + "_" + SW_CLK + "_WriteCompare(prescalerPeriod/2);");
                }
                writer.WriteLine("    " + m_instanceName + "_" + SW_CLK + "_Start();");
                writer.WriteLine("    ");
            }

            /* Idac settings for CSD and CSA */
            if ((sbItem.IsIdacAvailible()) && (sbItem.m_Method == E_CAPSENSE_MODE.CSD))
            {
                writer.WriteLine("    /* Set Idac */");
                writer.WriteLine("    " + m_instanceName + "_" + Symbol + "_" + S_IDAC + "_SetRange(idacRange);");
                writer.WriteLine("    " + m_instanceName + "_" + Symbol + "_" + S_IDAC + "_SetValue(idacSettings);");
            }
            else if (sbItem.m_Method == E_CAPSENSE_MODE.CSA)
            {
                writer.WriteLine("    " + m_instanceName + "_" + Symbol + "_" + S_IDAC + "_SetRange(idacRange);");
                writer.WriteLine("    " + m_instanceName + "_" + Symbol + "_" + S_IDAC + "_SetValue(idacCode);");
            }

            writer.WriteLine("}");
            writer.WriteLine("");
            writer.WriteLine("");
            #endregion

            #region ClearSlots
            writer.WriteLine("/*******************************************************************************");
            writer.WriteLine("* Function Name: " + m_instanceName + "_" + Method + "_ClearSlots" + sSide);
            writer.WriteLine("********************************************************************************");
            writer.WriteLine("*");
            writer.WriteLine("* Summary:");
            writer.WriteLine("*  Clears all slots to the non-sampling state by sequentially disconnecting all");
            writer.WriteLine("*  slots from Analog MUX Bus, disable pin global control and connecting them to");
            writer.WriteLine("*  GND or Shield electrode.");
            writer.WriteLine("*");
            writer.WriteLine("* Parameters:");
            writer.WriteLine("*  void");
            writer.WriteLine("*");
            writer.WriteLine("* Return:");
            writer.WriteLine("*  void");
            writer.WriteLine("*");
            writer.WriteLine("*******************************************************************************/");
            writer.WriteLine("void " + m_instanceName + "_" + Method + "_ClearSlots" + sSide + "(void)");
            writer.WriteLine("{");
            writer.WriteLine("    uint8 i, s, j;");
            writer.WriteLine("    uint8 startIndex, maxSns, state;");
            writer.WriteLine("    ");
            writer.WriteLine("    for (i=0; i < " + m_instanceName + "_TOTAL_SCANSLOT_COUNT" + sSideUp + "; i++)");
            writer.WriteLine("    {");
            writer.WriteLine("        startIndex = " + m_instanceName + "_ScanSlotTable" + sSide + "[i].IndexOffset;");
            writer.WriteLine("        maxSns = " + m_instanceName + "_ScanSlotTable" + sSide + "[i].SnsCnt;");
            writer.WriteLine("        state = " + m_instanceName + "_SettingsTable" + sSide + "[i].DisableState;");
            writer.WriteLine("        ");
            writer.WriteLine("        /* Disable Sensors */");
            writer.WriteLine("        if (maxSns == " + m_instanceName + "_ALONE_SENSOR)");
            writer.WriteLine("        {");
            writer.WriteLine("            " + m_instanceName + "_" + Method + "_DisableSensor" + sSide + "(startIndex, state);");
            writer.WriteLine("        }");
            writer.WriteLine("        else");
            writer.WriteLine("        {");
            writer.WriteLine("            for(j=0; j < maxSns; j++)");
            writer.WriteLine("            {");
            writer.WriteLine("                s = " + m_instanceName + "_IndexTable" + sSide + "[startIndex+j];");
            writer.WriteLine("                " + m_instanceName + "_" + Method + "_DisableSensor" + sSide + "(s, state);");
            writer.WriteLine("            }");
            writer.WriteLine("        }");
            writer.WriteLine("    }");
            writer.WriteLine("}");
            writer.WriteLine("");
            writer.WriteLine("");
            #endregion

            #region ReadSlot
            writer.WriteLine("/*******************************************************************************");
            writer.WriteLine("* Function Name: " + m_instanceName + "_" + Method + "_ReadSlot" + sSide);
            writer.WriteLine("********************************************************************************");
            writer.WriteLine("*");
            writer.WriteLine("* Summary:");
            writer.WriteLine("*  Returns scan slot raw data from the SlotResult[] array. Each scan slot has a");
            writer.WriteLine("*  unique number within the slot array. This number is assigned by the CapSense");
            writer.WriteLine("*  customizer in sequence.");
            writer.WriteLine("*");
            writer.WriteLine("* Parameters:");
            writer.WriteLine("*  slot:  Scan slot number");
            writer.WriteLine("*");
            writer.WriteLine("* Return:");
            writer.WriteLine("*  Returns current raw data value for defined scan slot");
            writer.WriteLine("*");
            writer.WriteLine("*******************************************************************************/");
            writer.WriteLine("uint16 " + m_instanceName + "_" + Method + "_ReadSlot" + sSide + "(uint8 slot)");
            writer.WriteLine("{");
            writer.WriteLine("    uint8 index = " + m_instanceName + "_ScanSlotTable" + sSide + "[slot].RawIndex;");
            writer.WriteLine("    ");
            writer.WriteLine("    return " + m_instanceName + "_SlotResult[index];");
            writer.WriteLine("}");
            writer.WriteLine("");
            writer.WriteLine("");
            #endregion

            #region EnableSensor
            writer.WriteLine("/*******************************************************************************");
            writer.WriteLine("* Function Name: " + m_instanceName + "_" + Method + "_EnableSensor" + sSide);
            writer.WriteLine("********************************************************************************");
            writer.WriteLine("*");
            writer.WriteLine("* Summary:");
            writer.WriteLine("*  Configures the selected sensor to measure during the next measurement cycle.");
            writer.WriteLine("*  The corresponding pins are set to Analog High-Z mode and connected to the");         
            writer.WriteLine("*  Analog Mux Bus. This also enables the comparator function.");
            writer.WriteLine("*");
            writer.WriteLine("* Parameters:");
            writer.WriteLine("*  sensor:  Sensor number");
            writer.WriteLine("*");
            writer.WriteLine("* Return:");
            writer.WriteLine("*  void");
            writer.WriteLine("*");
            writer.WriteLine("*******************************************************************************/");
            writer.WriteLine("void " + m_instanceName + "_" + Method + "_EnableSensor" + sSide + "(uint8 sensor)");
            writer.WriteLine("{");
            writer.WriteLine("    uint8 port = " + m_instanceName + "_PortShiftTable" + sSide + "[sensor].port;");
            writer.WriteLine("    uint8 shift = " + m_instanceName + "_PortShiftTable" + sSide + "[sensor].shift;");
            writer.WriteLine("    uint8 mask = 1;");
            writer.WriteLine("    ");
            writer.WriteLine("    mask <<= shift;");
            writer.WriteLine("    ");
            writer.WriteLine("    /* Make sensor High-Z */");
            writer.WriteLine("    *(" + m_instanceName + "_BASE_PRT_PC + (" + m_instanceName + "_PRT_PC_OFFSET*port) + shift) = " + m_instanceName + "_PRT_PC_HIGHZ;");
            writer.WriteLine("    ");
            writer.WriteLine("    /* Connect from DSI output */");
            writer.WriteLine("    *(" + m_instanceName + "_BASE_PRTDSI_CAPS + (" + m_instanceName + "_PRTDSI_OFFSET*port)) |= mask;");
            writer.WriteLine("    ");
            writer.WriteLine("    /* Connect to AMUX */");
            writer.WriteLine("    " + m_instanceName + "_" + Symbol + "_" + S_AMUX + "_Connect(sensor);    ");
            writer.WriteLine("}");
            writer.WriteLine("");
            writer.WriteLine("");
            #endregion

            #region DisableSensor
            writer.WriteLine("/*******************************************************************************");
            writer.WriteLine("* Function Name: " + m_instanceName + "_" + Method + "_DisableSensor" + sSide);
            writer.WriteLine("********************************************************************************");
            writer.WriteLine("*");
            writer.WriteLine("* Summary:");
            writer.WriteLine("*  Disables the selected sensor. The corresponding pin is disconnected from the");
            writer.WriteLine("*  Analog Mux Bus and connected to GND, High_Z or Shield electrode.");
            writer.WriteLine("*");
            writer.WriteLine("* Parameters:");
            writer.WriteLine("*  sensor:  Sensor number");
            writer.WriteLine("*  state:  State of sensor when disabled");
            writer.WriteLine("*");
            writer.WriteLine("* Return:");
            writer.WriteLine("*  void");
            writer.WriteLine("*");
            writer.WriteLine("*******************************************************************************/");
            writer.WriteLine("void " + m_instanceName + "_" + Method + "_DisableSensor" + sSide + "(uint8 sensor, uint8 state)");
            writer.WriteLine("{");
            writer.WriteLine("    uint8 port = " + m_instanceName + "_PortShiftTable" + sSide + "[sensor].port;");
            writer.WriteLine("    uint8 shift = " + m_instanceName + "_PortShiftTable" + sSide + "[sensor].shift;");
            writer.WriteLine("    uint8 mask = 1;");
            writer.WriteLine("    ");
            writer.WriteLine("    mask <<= shift;");
            writer.WriteLine("    ");
            writer.WriteLine("    /* Disconnect from AMUX */");
            writer.WriteLine("    " + m_instanceName + "_" + Symbol + "_" + S_AMUX + "_Disconnect(sensor);");
            writer.WriteLine("    ");
            writer.WriteLine("    /* Connect from DSI output */");
            writer.WriteLine("    *(" + m_instanceName + "_BASE_PRTDSI_CAPS + (" + m_instanceName + "_PRTDSI_OFFSET*port)) &= ~mask;");
            writer.WriteLine("    ");
            writer.WriteLine("    switch(state)");
            writer.WriteLine("    {");
            writer.WriteLine("        case " + m_instanceName + "_DISABLE_STATE_GND:");
            writer.WriteLine("            *(" + m_instanceName + "_BASE_PRT_PC + (" + m_instanceName + "_PRT_PC_OFFSET*port) + shift) = " + m_instanceName + "_PRT_PC_GND;");
            writer.WriteLine("        break;");
            writer.WriteLine("        ");
            writer.WriteLine("        case " + m_instanceName + "_DISABLE_STATE_HIGHZ:");
            writer.WriteLine("            *(" + m_instanceName + "_BASE_PRT_PC + (" + m_instanceName + "_PRT_PC_OFFSET*port) + shift) = " + m_instanceName + "_PRT_PC_HIGHZ;");
            writer.WriteLine("        break;");
            writer.WriteLine("        ");
            writer.WriteLine("        case " + m_instanceName + "_DISABLE_STATE_SHIELD:");
            writer.WriteLine("            *(" + m_instanceName + "_BASE_PRT_PC + (" + m_instanceName + "_PRT_PC_OFFSET*port) + shift) = " + m_instanceName + "_PRT_PC_SHIELD;");
            writer.WriteLine("        break;");
            writer.WriteLine("        ");
            writer.WriteLine("        default:");
            writer.WriteLine("            *(" + m_instanceName + "_BASE_PRT_PC + (" + m_instanceName + "_PRT_PC_OFFSET*port) + shift) = " + m_instanceName + "_PRT_PC_GND;");
            writer.WriteLine("        break;");
            writer.WriteLine("    }");
            writer.WriteLine("}");
            #endregion
        }
        #endregion
    }
}
