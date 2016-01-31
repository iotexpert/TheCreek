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
        #region CollectApiCFile
        public void CollectApiCFile(ICyInstQuery_v1 query, ICyTerminalQuery_v1 termQuery, CyGeneralParams packParams, ref Dictionary<string, string> paramDict)
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
                if (packParams.m_localParams.BCsHalfIsEnable(sbItem))
                {
                    ApiCollectCVariables(ref writerCVariables, packParams, sbItem);
                    ApiCollectCFunctionForSide(ref writerCFunctions, packParams, sbItem);
                    ApiCollectCIntFunctionForSide(ref writerCIntFunctions, packParams, sbItem);

                    ApiCollectCHLFunctionForSide(ref writerCHLFunctions, packParams, sbItem);
                }

            writerCVariables.WriteLine("uint16 " + m_instanceName + "_SlotResult[" + m_instanceName + "_TOTAL_SCANSLOT_COUNT] = {0};");

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
            int i_IndexTable = 0;

            writer.WriteLine("");
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
                List<CyElScanSlot> listWork = packParams.m_cyScanSlotsList.GetListFromSide(side);
                List<CyElTerminal> _listTerm = packParams.m_cyWidgetsList.GetListTerminalsFromSide(side);
                for (int i = 0; i < listWork.Count; i++)
                {
                    //Calculate offset in RawCount array
                    int offset = 0;
                    if (sbItem.m_side == E_EL_SIDE.Right)
                        offset = packParams.m_cyScanSlotsList.GetListFromSide(E_EL_SIDE.Left).Count;

                    CyElScanSlot ss = listWork[i];
                    string widgetNumber = packParams.m_cyWidgetsList.GetIndexInListWithFullWidgetsHL(ss.m_Widget).ToString();
                    
                    if (ss.m_Widget.m_type == E_SENSOR_TYPE.Generic) widgetNumber = "0xffu";

                    string str = "	{" + (offset + i).ToString() + ", " + i_IndexTable + ", " + ss.GetTerminals().Count + ", " +
                        widgetNumber + ", " + "0" + "}";
                    if (i < packParams.m_cyScanSlotsList.GetListFromSide(side).Count - 1)
                        str += ",";
                    writer.WriteLine(str);

                    //Filling Index_Table
                    foreach (CyElTerminal item in ss.GetTerminals())
                    {
                        i_IndexTable++;
                        strIndexTable += _listTerm.IndexOf(item).ToString() + ", ";
                    }
                }
                writer.WriteLine("};");//Close array

                //Filling Index_Table
                if (strIndexTable.Length > 1)
                    strIndexTable = strIndexTable.Remove(strIndexTable.Length - 2);
                writerIndex_Table.Write(strIndexTable + "};");
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
                for (int i = 0; i < packParams.m_cyScanSlotsList.GetListFromSide(side).Count; i++)
                {
                    CyElScanSlot ss = packParams.m_cyScanSlotsList.GetListFromSide(side)[i];
                    writer.Write("\t{");

                    if (sbItem.m_isIdac)
                    {
                        writer.Write(m_instanceName + GetIDACRangeStr(ss.m_SSProperties.m_IDACRange));				/* 1-3 range exist, multiplier for IDAC */
                        writer.Write(", ");
                        writer.Write(ss.m_SSProperties.m_IDACSettings.m_Value);						/* IDAC settings for measurament */
                        writer.Write(", ");
                    }
                    //CSA Properties
                    if (sbItem.m_Method == E_CAPSENSE_MODE.CSA)
                    {
                        writer.Write(ss.m_SSProperties.m_baseProps.m_Scanlength.m_Value);					/* IDAC measure = SA - ScanLength */
                        writer.Write(", ");
                        writer.Write(ss.m_SSProperties.m_baseProps.m_SettlingTime.m_Value);					/*SettlingTime */
                        writer.Write(", ");
                        writer.Write("0x00");			 		/* IdacCode */
                        writer.Write(", ");
                    }
                    if (sbItem.IsPrescaler())
                    {

                        writer.Write(ss.m_SSProperties.m_baseProps.m_PrescPer.m_Value - 1);			/* Prescaler for CapsClock */
                        writer.Write(", ");
                    }

                    //CSD Properties
                    if (sbItem.m_Method == E_CAPSENSE_MODE.CSD)
                    {
                        writer.Write(ss.m_SSProperties.GetResolution(m_instanceName));				/* PWM Resolution */
                        writer.Write(", ");
                        writer.Write(ss.m_SSProperties.GetScanSpeed(m_instanceName));				/* Speed of scanning */
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
                    writer.Write(m_instanceName + GetCISStr(ss.m_SSProperties.m_CIS)); 			/* (Hi-Z;GND; Shield) */
                    writer.Write(", ");

                    //Removing ", "
                    if (writer.Get0Length() > 4)
                        writer.Remove(writer.Get0Length() - 2);

                    if (i < packParams.m_cyScanSlotsList.GetListFromSide(side).Count - 1)
                        writer.WriteLine("},");
                    else
                        writer.WriteLine("}");
                }
                writer.WriteLine("};");
            }
            writer.WriteLine("");
            #endregion

            #region PortShift  Generation
            writer.WriteLine("const " + m_instanceName + "_PortShift " + m_instanceName + "_PortShiftTable" + sSide + "[] = ");
            if (packParams.IsAmuxBusBlank(sbItem.m_side))
            {
                writer.WriteLine("{0};");
            }
            else
            {
                writer.WriteLine("\n{");
                for (int i = 0; i < packParams.m_cyWidgetsList.GetListTerminalsFromSide(side).Count; i++)
                {
                    string str = m_instanceName + "_" + Symbol + "_" + m_csPort + "__" + packParams.m_cyWidgetsList.GetListTerminalsFromSide(side)[i].ToString();
                    str = "	{" + str + "__PORT" + ", " + str + "__SHIFT" + "}";
                    if (i < packParams.m_cyWidgetsList.GetListTerminalsFromSide(side).Count - 1)
                        str += ",";
                    writer.WriteLine(str);
                }
                writer.WriteLine("};");
            }
            writer.WriteLine("");
            #endregion

            #region Rb Gen
            if (sbItem.m_isRbEnable)
            {
                writer.WriteLine("/* Different for Left and Right sides */");
                writer.WriteLine("reg8 *" + m_instanceName + "_RbPortPinTable" + sSide + "[] = ");
                writer.WriteLine("{");
                for (int i = 0; i < sbItem.CountRb; i++)
                {
                    writer.Write("  " + m_instanceName + "_" + Symbol + "_cRb" + i + "__" + packParams.GetAliasRbByIndex(sbItem, i) + "__PC");
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
                    string str = m_instanceName + "_" + Symbol + "_" + m_csPort + "__" + packParams.GetAliasShieldByIndex(sbItem, i);
                    str = "	{" + str + "__PORT" + ", " + str + "__SHIFT" + "}";
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

            writer.WriteLine("");
            writer.WriteLine("uint8 " + m_instanceName + "_idacInitVar" + sSide + " = 0;");
            writer.WriteLine("");
        }
        #endregion

        #region  apiCollectCFunctionForParallelMode
        public void ApiCollectCFunctionForParallelMode(ref TextWriter writer, CyGeneralParams packParams)
        {
            //if (packParams.localParams.isParallel())
            {

                string MethodL = packParams.m_localParams.m_listCsHalfs[0].m_Method.ToString();
                string MethodR = packParams.m_localParams.m_listCsHalfs[1].m_Method.ToString();

                #region Start
                writer.WriteLine("/*-----------------------------------------------------------------------------");
                writer.WriteLine(" * FUNCTION NAME:void " + m_instanceName + "_Start(void)");
                writer.WriteLine(" *-----------------------------------------------------------------------------");
                writer.WriteLine(" * Summary: ");
                writer.WriteLine(" *  Initializes registers and starts the CapSense Component. This function ");
                writer.WriteLine(" *  should be called prior to calling any other CapSense Component functions. ");
                writer.WriteLine(" *	");
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
                writer.WriteLine("void " + m_instanceName + "_Start(void)");
                writer.WriteLine("{");
                foreach (CyAmuxBParams sbItem in packParams.m_localParams.m_listCsHalfs)
                    if (packParams.m_localParams.BCsHalfIsEnable(sbItem))
                    {
                        string sSide = GetSideName(sbItem.m_side, packParams.Configuration);
                        string Method = sbItem.m_Method.ToString();
                        writer.WriteLine("	" + m_instanceName + "_" + Method + "_Start" + sSide + "();");
                    }
                writer.WriteLine("}");
                writer.WriteLine("");

                #endregion

                #region Stop
                writer.WriteLine("/*-----------------------------------------------------------------------------");
                writer.WriteLine(" * FUNCTION NAME: void " + m_instanceName + "_Stop(void)");
                writer.WriteLine(" *-----------------------------------------------------------------------------");
                writer.WriteLine(" * Summary: ");
                writer.WriteLine(" *  Initializes registers and starts the CapSense Component. This function ");
                writer.WriteLine(" *  should be called prior to calling any other CapSense Component functions. ");
                writer.WriteLine(" *	");
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
                writer.WriteLine("void " + m_instanceName + "_Stop(void)");
                writer.WriteLine("{");
                foreach (CyAmuxBParams sbItem in packParams.m_localParams.m_listCsHalfs)
                    if (packParams.m_localParams.BCsHalfIsEnable(sbItem))
                    {
                        string sSide = GetSideName(sbItem.m_side, packParams.Configuration);
                        string Method = sbItem.m_Method.ToString();
                        writer.WriteLine("	" + m_instanceName + "_" + Method + "_Stop" + sSide + "();");

                    }
                writer.WriteLine("}");
                writer.WriteLine("");
                #endregion

                if (packParams.m_localParams.IsParallelFull())
                {

                    #region ScanSlot Left
                    writer.WriteLine("/*-----------------------------------------------------------------------------");
                    writer.WriteLine(" * FUNCTION NAME: void " + m_instanceName + "_ScanSlotLeft(uint8 slot)");
                    writer.WriteLine(" *-----------------------------------------------------------------------------");
                    writer.WriteLine(" * Summary: ");
                    writer.WriteLine(" *  Scans the selected slot. The order of scanning and number of sensors to scan ");
                    writer.WriteLine(" *  defined in slot position of terminal. ");
                    writer.WriteLine(" *	");
                    writer.WriteLine(" * Parameters: ");
                    writer.WriteLine(" *  (uint8) slot: Slot number.");
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
                    writer.WriteLine("void " + m_instanceName + "_ScanSlotLeft(uint8 slot)");
                    writer.WriteLine("{");
                    writer.WriteLine("      " + m_instanceName + "_" + MethodL + "_ScanSlotLeft(slot);");
                    writer.WriteLine("}");
                    writer.WriteLine("");
                    #endregion

                    #region ScanSlot Right
                    writer.WriteLine("/*-----------------------------------------------------------------------------");
                    writer.WriteLine(" * FUNCTION NAME: void " + m_instanceName + "_ScanSlotRight(uint8 slot)");
                    writer.WriteLine(" *-----------------------------------------------------------------------------");
                    writer.WriteLine(" * Summary: ");
                    writer.WriteLine(" *  Scans the selected slot. The order of scanning and number of sensors to scan ");
                    writer.WriteLine(" *  defined in slot position of terminal. ");
                    writer.WriteLine(" *	");
                    writer.WriteLine(" * Parameters: ");
                    writer.WriteLine(" *  (uint8) slot: Slot number.");
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
                    writer.WriteLine("void " + m_instanceName + "_ScanSlotRight(uint8 slot)");
                    writer.WriteLine("{");
                    writer.WriteLine("      " + m_instanceName + "_" + MethodR + "_ScanSlotRight(slot);");
                    writer.WriteLine("}");
                    writer.WriteLine("");
                    #endregion

                }
                else
                {
                    //Not Paralle Full and Serial
                    foreach (CyAmuxBParams sbItem in packParams.m_localParams.m_listCsHalfs)
                        if (packParams.m_localParams.BCsHalfIsEnable(sbItem))
                        {
                            #region ScanSlot
                            writer.WriteLine("/*-----------------------------------------------------------------------------");
                            writer.WriteLine(" * FUNCTION NAME: void " + m_instanceName + "_ScanSlot(uint8 slot)");
                            writer.WriteLine(" *-----------------------------------------------------------------------------");
                            writer.WriteLine(" * Summary: ");
                            writer.WriteLine(" *  Scans the selected slot. The order of scanning and number of sensors to scan ");
                            writer.WriteLine(" *  defined in slot position of terminal. ");
                            writer.WriteLine(" *	");
                            writer.WriteLine(" * Parameters: ");
                            writer.WriteLine(" *  (uint8) slot: Slot number.");
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
                            writer.WriteLine("void " + m_instanceName + "_ScanSlot(uint8 slot)");
                            writer.WriteLine("{");
                            string sSide = GetSideName(sbItem.m_side, packParams.Configuration);
                            string Method = sbItem.m_Method.ToString();
                            writer.WriteLine("      " + m_instanceName + "_" + Method + "_ScanSlot(slot);");
                            writer.WriteLine("}");
                            writer.WriteLine("");
                            #endregion
                        }

                }

                if (packParams.m_localParams.IsParallelFull())
                {
                    // Parallel Async or Sync
                    if (packParams.Configuration == E_MAIN_CONFIG.emParallelAsynchron)
                    {
                        #region ScanAllSlots Asynchron
                        writer.WriteLine("/*-----------------------------------------------------------------------------");
                        writer.WriteLine(" * FUNCTION NAME: void " + m_instanceName + "_ScanAllSlots(void)");
                        writer.WriteLine(" *-----------------------------------------------------------------------------");
                        writer.WriteLine(" * Summary: ");
                        writer.WriteLine(" *  Scans the selected slot. The order of scanning and number of sensors to scan ");
                        writer.WriteLine(" *  defined in slot position of terminal. ");
                        writer.WriteLine(" *	");
                        writer.WriteLine(" * Parameters: ");
                        writer.WriteLine(" *  (uint8) slot: Slot number.");
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
                        writer.WriteLine("void " + m_instanceName + "_ScanAllSlots(void)");
                        writer.WriteLine("{");
                        writer.WriteLine("	uint8 l = 0, r = 0;");
                        writer.WriteLine("	uint8 k, s, state, maxSns, startIndex, rawIndex, leftDone = 1, rightDone = 1;");
                        writer.WriteLine("  ");
                        writer.WriteLine("	while ((l < " + m_instanceName + "_TOTAL_SCANSLOT_COUNT_LEFT) || (r < " + m_instanceName + "_TOTAL_SCANSLOT_COUNT_RIGHT))");
                        writer.WriteLine("	{");
                        writer.WriteLine("		if ((" + m_instanceName + "_statusLeft != " + m_instanceName + "_START_CAPSENSING) && (l < " + m_instanceName + "_TOTAL_SCANSLOT_COUNT_LEFT) && leftDone) /* Start new scan of Left half */");
                        writer.WriteLine("		{");
                        writer.WriteLine("	        ");
                        writer.WriteLine("			/* Set Slot Settings  for Left  */");
                        writer.WriteLine("			" + m_instanceName + "_" + MethodL + "_SetSlotSettingsLeft(l);");
                        writer.WriteLine("			");
                        writer.WriteLine("			/* Disable csBuf Left */");
                        writer.WriteLine("			" + m_instanceName + "_CAPS_CFG0_LEFT &= ~" + m_instanceName + "_CSBUF_ENABLE;");
                        writer.WriteLine("          ");
                        writer.WriteLine("			maxSns = " + m_instanceName + "_ScanSlotTableLeft[l].SnsCnt;");
                        writer.WriteLine("			startIndex = " + m_instanceName + "_ScanSlotTableLeft[l].IndexOffset;");
                        writer.WriteLine("          ");

                        if (packParams.m_localParams.m_listCsHalfs[0].m_csdSubMethod == E_CSD_SUB_METHODS.IDACSinking)
                        {
                            writer.WriteLine("      /* Connect Left IDAC */");
                            writer.WriteLine("      " + m_instanceName + "_lb" + MethodR + "_" + m_csAmux + "_Connect(" + m_instanceName + "_IDAC_CHANNEL_LEFT);");
                            writer.WriteLine("      ");
                        }
                        else if (packParams.m_localParams.m_listCsHalfs[0].m_csdSubMethod == E_CSD_SUB_METHODS.IDACDisable_RB)
                        {
                            writer.WriteLine("      /* Connect Left DSI output to Rb */");
                            writer.WriteLine("      *" + m_instanceName + "_RbPortPinTableLeft[" + m_instanceName + "_currentRbleedLeft] |= " + m_instanceName + "_BYP_MASK;");
                            writer.WriteLine("      ");
                        }

                        writer.WriteLine("			/* Enable Sensors Left */");
                        writer.WriteLine("			for(k=0; k < maxSns; k++)");
                        writer.WriteLine("			{");
                        writer.WriteLine("			    s = " + m_instanceName + "_IndexTableLeft[startIndex+k];");
                        writer.WriteLine("				" + m_instanceName + "_" + MethodL + "_EnableSensorLeft(s);");
                        writer.WriteLine("			}	");
                        writer.WriteLine("          ");
                        writer.WriteLine("			/* Set Flag that CapSensing in progres */");
                        writer.WriteLine("			" + m_instanceName + "_statusLeft |= " + m_instanceName + "_START_CAPSENSING;");
                        writer.WriteLine("          ");
                        writer.WriteLine("			/* Start PWM One Shout and PRS at one time */");
                        writer.WriteLine("			" + m_instanceName + "_CONTROL_LEFT |= " + m_instanceName + "_START_CAPSENSING;");
                        writer.WriteLine("			leftDone = 0;");
                        writer.WriteLine("		}");
                        writer.WriteLine("      ");
                        writer.WriteLine("		if ((" + m_instanceName + "_statusRight != " + m_instanceName + "_START_CAPSENSING) && (r < " + m_instanceName + "_TOTAL_SCANSLOT_COUNT_RIGHT) && rightDone )");
                        writer.WriteLine("		{");
                        writer.WriteLine("			");
                        writer.WriteLine("			/* Set Slot Settings  for Right  */");
                        writer.WriteLine("			" + m_instanceName + "_" + MethodR + "_SetSlotSettingsRight(r);");
                        writer.WriteLine("	        ");
                        writer.WriteLine("			/* Disable csBuf Right */");
                        writer.WriteLine("			" + m_instanceName + "_CAPS_CFG0_RIGHT &= ~" + m_instanceName + "_CSBUF_ENABLE;");
                        writer.WriteLine("          ");
                        writer.WriteLine("			maxSns = " + m_instanceName + "_ScanSlotTableRight[r].SnsCnt;");
                        writer.WriteLine("			startIndex = " + m_instanceName + "_ScanSlotTableRight[r].IndexOffset;");
                        writer.WriteLine("          ");

                        if (packParams.m_localParams.m_listCsHalfs[1].m_csdSubMethod == E_CSD_SUB_METHODS.IDACSinking)
                        {
                            writer.WriteLine("          /* Connect Right IDAC */");
                            writer.WriteLine("          " + m_instanceName + "_rb" + MethodR + "_" + m_csAmux + "_Connect(" + m_instanceName + "_IDAC_CHANNEL_RIGHT);");
                            writer.WriteLine("          ");
                        }
                        else if (packParams.m_localParams.m_listCsHalfs[1].m_csdSubMethod == E_CSD_SUB_METHODS.IDACDisable_RB)
                        {
                            writer.WriteLine("          /* Connect Right DSI output to Rb */");
                            writer.WriteLine("          *" + m_instanceName + "_RbPortPinTableRight[" + m_instanceName + "_currentRbleedRight] |= " + m_instanceName + "_BYP_MASK;");
                            writer.WriteLine("          ");
                        }

                        writer.WriteLine("			/* Enable Sensors Right */");
                        writer.WriteLine("			for(k=0; k < maxSns; k++)");
                        writer.WriteLine("			{");
                        writer.WriteLine("				s = " + m_instanceName + "_IndexTableRight[startIndex+k];");
                        writer.WriteLine("				" + m_instanceName + "_" + MethodR + "_EnableSensorRight(s);");
                        writer.WriteLine("			}");
                        writer.WriteLine("          ");
                        writer.WriteLine("			/* Set Flag that CapSensing in progres */");
                        writer.WriteLine("			" + m_instanceName + "_statusRight |= " + m_instanceName + "_START_CAPSENSING;");
                        writer.WriteLine("          ");
                        writer.WriteLine("			/* Start PWM One Shout and PRS at one time */");
                        writer.WriteLine("			" + m_instanceName + "_CONTROL_RIGHT |= " + m_instanceName + "_START_CAPSENSING;");
                        writer.WriteLine("			rightDone = 0;");
                        writer.WriteLine("		}");
                        writer.WriteLine("		");
                        writer.WriteLine("		/* Wait finish of CapSensing */");
                        writer.WriteLine("		while (1)");
                        writer.WriteLine("		{");
                        writer.WriteLine("			if ((" + m_instanceName + "_statusLeft != " + m_instanceName + "_START_CAPSENSING) && (l < " + m_instanceName + "_TOTAL_SCANSLOT_COUNT_LEFT))");
                        writer.WriteLine("			{");
                        writer.WriteLine("				break;");
                        writer.WriteLine("			}");
                        writer.WriteLine("			");
                        writer.WriteLine("			if ((" + m_instanceName + "_statusRight != " + m_instanceName + "_START_CAPSENSING) && (r < " + m_instanceName + "_TOTAL_SCANSLOT_COUNT_RIGHT))");
                        writer.WriteLine("			{");
                        writer.WriteLine("				break;");
                        writer.WriteLine("			}");
                        writer.WriteLine("		}");
                        writer.WriteLine("		");
                        writer.WriteLine("		if ((" + m_instanceName + "_statusLeft != " + m_instanceName + "_START_CAPSENSING) && (l < " + m_instanceName + "_TOTAL_SCANSLOT_COUNT_LEFT))");
                        writer.WriteLine("		{");
                        writer.WriteLine("			/* Stop Capsensing */");
                        writer.WriteLine("			" + m_instanceName + "_CONTROL_LEFT &= ~" + m_instanceName + "_START_CAPSENSING;");
                        writer.WriteLine("			");
                        writer.WriteLine("			/* Read SlotResult from RwaCnt for Left side */");
                        writer.WriteLine("			rawIndex = " + m_instanceName + "_ScanSlotTableLeft[l].RawIndex;");
                        writer.WriteLine("			");
                        writer.WriteLine("			/* Read SlotResult from RwaCnt for Left side */");
                        writer.WriteLine("			" + m_instanceName + "_SlotResult[rawIndex] = " + m_instanceName + "_lb" + MethodL + "_" + m_csRawCnt + "_ReadCounter();");
                        writer.WriteLine("	        ");
                        writer.WriteLine("			startIndex = " + m_instanceName + "_ScanSlotTableLeft[l].IndexOffset;");
                        writer.WriteLine("			maxSns = " + m_instanceName + "_ScanSlotTableLeft[l].SnsCnt;");
                        writer.WriteLine("	        ");
                        writer.WriteLine("			/* Reset Raw Counter and PWM Left*/");
                        writer.WriteLine("			" + m_instanceName + "_CONTROL_LEFT |= " + m_instanceName + "_RESET_PWM_CNTR;");
                        writer.WriteLine("	        ");
                        writer.WriteLine("			/* Disable Sensors Right */");
                        writer.WriteLine("			for(k=0; k < maxSns; k++)");
                        writer.WriteLine("			{");
                        writer.WriteLine("				s = " + m_instanceName + "_IndexTableLeft[startIndex+k];");
                        writer.WriteLine("		        state = " + m_instanceName + "_SettingsTableLeft[l].DisableState;");
                        writer.WriteLine("				" + m_instanceName + "_" + MethodL + "_DisableSensorLeft(s, state);");
                        writer.WriteLine("			}");
                        writer.WriteLine("	        ");
                        writer.WriteLine("			" + m_instanceName + "_CONTROL_LEFT &= ~" + m_instanceName + "_RESET_PWM_CNTR;");
                        writer.WriteLine("	        ");
                        if (packParams.m_localParams.m_listCsHalfs[0].m_csdSubMethod == E_CSD_SUB_METHODS.IDACSinking)
                        {
                            writer.WriteLine("	         /* Disconnect Left IDAC */");
                            writer.WriteLine("	          " + m_instanceName + "_lb" + MethodL + "_" + m_csAmux + "_Disconnect(" + m_instanceName + "_IDAC_CHANNEL_LEFT);");
                            writer.WriteLine("	          ");
                        }
                        else if (packParams.m_localParams.m_listCsHalfs[0].m_csdSubMethod == E_CSD_SUB_METHODS.IDACDisable_RB)
                        {
                            writer.WriteLine("	         /* Disconnect Left DSI output to Rb */");
                            writer.WriteLine("            *" + m_instanceName + "_RbPortPinTableLeft[" + m_instanceName + "_currentRbleedLeft] &= ~" + m_instanceName + "_BYP_MASK;");
                            writer.WriteLine("	          ");
                        }
                        writer.WriteLine("			/* Enable Vref on AMUX */");
                        writer.WriteLine("			" + m_instanceName + "_CAPS_CFG0_LEFT |= " + m_instanceName + "_CSBUF_ENABLE;");
                        writer.WriteLine("			");
                        writer.WriteLine("			/* Scan enother scanslot */");
                        writer.WriteLine("			l++;");
                        writer.WriteLine("			leftDone = 1;");
                        writer.WriteLine("		}");
                        writer.WriteLine("  ");
                        writer.WriteLine("		if ((" + m_instanceName + "_statusRight != " + m_instanceName + "_START_CAPSENSING) && (r < " + m_instanceName + "_TOTAL_SCANSLOT_COUNT_RIGHT))");
                        writer.WriteLine("		{");
                        writer.WriteLine("			/* Stop Capsensing */");
                        writer.WriteLine("			" + m_instanceName + "_CONTROL_RIGHT &= ~" + m_instanceName + "_START_CAPSENSING;");
                        writer.WriteLine("			");
                        writer.WriteLine("		    /* Read SlotResult from RwaCnt  for Right side */");
                        writer.WriteLine("			rawIndex = " + m_instanceName + "_ScanSlotTableRight[r].RawIndex;");
                        writer.WriteLine("			");
                        writer.WriteLine("		    /* Read SlotResult from RwaCnt for Right side */");
                        writer.WriteLine("		    " + m_instanceName + "_SlotResult[rawIndex] = " + m_instanceName + "_rb" + MethodR + "_" + m_csRawCnt + "_ReadCounter();");
                        writer.WriteLine("			");
                        writer.WriteLine("		    startIndex = " + m_instanceName + "_ScanSlotTableRight[r].IndexOffset;");
                        writer.WriteLine("		    maxSns = " + m_instanceName + "_ScanSlotTableRight[r].SnsCnt;");
                        writer.WriteLine("	        ");
                        writer.WriteLine("			/* Reset Raw Counter and PWM Right*/");
                        writer.WriteLine("			" + m_instanceName + "_CONTROL_RIGHT |= " + m_instanceName + "_RESET_PWM_CNTR;");
                        writer.WriteLine("	        ");
                        writer.WriteLine("		    /* Disable Sensors Right */");
                        writer.WriteLine("			for(k=0; k < maxSns; k++)");
                        writer.WriteLine("			{");
                        writer.WriteLine("				s = " + m_instanceName + "_IndexTableRight[startIndex+k];");
                        writer.WriteLine("		        state = " + m_instanceName + "_SettingsTableRight[r].DisableState;");
                        writer.WriteLine("				" + m_instanceName + "_" + MethodR + "_DisableSensorRight(s, state);");
                        writer.WriteLine("			}");
                        writer.WriteLine("	        ");
                        writer.WriteLine("			" + m_instanceName + "_CONTROL_RIGHT &= ~" + m_instanceName + "_RESET_PWM_CNTR;");
                        writer.WriteLine("	        ");
                        if (packParams.m_localParams.m_listCsHalfs[1].m_csdSubMethod == E_CSD_SUB_METHODS.IDACSinking)
                        {
                            writer.WriteLine("	      /* Disconnect Right IDAC */");
                            writer.WriteLine("	      " + m_instanceName + "_rb" + MethodR + "_" + m_csAmux + "_Disconnect(" + m_instanceName + "_IDAC_CHANNEL_RIGHT);");
                            writer.WriteLine("	      ");
                        }
                        else if (packParams.m_localParams.m_listCsHalfs[1].m_csdSubMethod == E_CSD_SUB_METHODS.IDACDisable_RB)
                        {
                            writer.WriteLine("	      /* Disconnect Right DSI output to Rb */");
                            writer.WriteLine("        *" + m_instanceName + "_RbPortPinTableRight[" + m_instanceName + "_currentRbleedRight] &= ~" + m_instanceName + "_BYP_MASK;");
                            writer.WriteLine("	      ");
                        }
                        writer.WriteLine("			/* Enable Vref on AMUX */");
                        writer.WriteLine("			" + m_instanceName + "_CAPS_CFG0_RIGHT |= " + m_instanceName + "_CSBUF_ENABLE;");
                        writer.WriteLine("			");
                        writer.WriteLine("			/* Scan enother scanslot */");
                        writer.WriteLine("			r++;");
                        writer.WriteLine("			rightDone = 1;");
                        writer.WriteLine("	    }");
                        writer.WriteLine("	}");
                        writer.WriteLine("}");
                        writer.WriteLine("");

                        #endregion
                    }
                    else
                    {
                        #region ScanAllSlots Synchron
                        writer.WriteLine("/*-----------------------------------------------------------------------------");
                        writer.WriteLine(" * FUNCTION NAME: void " + m_instanceName + "_ScanAllSlots(void)");
                        writer.WriteLine(" *-----------------------------------------------------------------------------");
                        writer.WriteLine(" * Summary: ");
                        writer.WriteLine(" *  Scans the selected slot. The order of scanning and number of sensors to scan ");
                        writer.WriteLine(" *  defined in slot position of terminal. ");
                        writer.WriteLine(" *	");
                        writer.WriteLine(" * Parameters: ");
                        writer.WriteLine(" *  (uint8) slot: Slot number.");
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
                        writer.WriteLine("void " + m_instanceName + "_ScanAllSlots(void)");
                        writer.WriteLine("{");
                        writer.WriteLine("	uint8 l = 0, r = 0;");
                        writer.WriteLine("	uint8 k, s, state, maxSns, startIndex, rawIndex;");
                        writer.WriteLine("");
                        writer.WriteLine("	while ((l < " + m_instanceName + "_TOTAL_SCANSLOT_COUNT_LEFT) || (r < " + m_instanceName + "_TOTAL_SCANSLOT_COUNT_RIGHT))");
                        writer.WriteLine("	{");
                        writer.WriteLine("		if (l < " + m_instanceName + "_TOTAL_SCANSLOT_COUNT_LEFT) /* Start new scan of Left half */");
                        writer.WriteLine("		{");
                        writer.WriteLine("	        ");
                        writer.WriteLine("			/* Set Slot Settings  for Left  */");
                        writer.WriteLine("			" + m_instanceName + "_" + MethodL + "_SetSlotSettingsLeft(l);");
                        writer.WriteLine("			");
                        writer.WriteLine("			/* Disable csBuf Left */");
                        writer.WriteLine("			" + m_instanceName + "_CAPS_CFG0_LEFT &= ~" + m_instanceName + "_CSBUF_ENABLE;");
                        writer.WriteLine("          ");
                        writer.WriteLine("			maxSns = " + m_instanceName + "_ScanSlotTableLeft[l].SnsCnt;");
                        writer.WriteLine("			startIndex = " + m_instanceName + "_ScanSlotTableLeft[l].IndexOffset;");
                        writer.WriteLine("          ");

                        if (packParams.m_localParams.m_listCsHalfs[0].m_csdSubMethod == E_CSD_SUB_METHODS.IDACSinking)
                        {
                            writer.WriteLine("      /* Connect Left IDAC */");
                            writer.WriteLine("      " + m_instanceName + "_lb" + MethodR + "_" + m_csAmux + "_Connect(" + m_instanceName + "_IDAC_CHANNEL_LEFT);");
                            writer.WriteLine("      ");
                        }
                        else if (packParams.m_localParams.m_listCsHalfs[0].m_csdSubMethod == E_CSD_SUB_METHODS.IDACDisable_RB)
                        {
                            writer.WriteLine("      /* Connect Left DSI output to Rb */");
                            writer.WriteLine("      *" + m_instanceName + "_RbPortPinTableLeft[" + m_instanceName + "_currentRbleedLeft] |= " + m_instanceName + "_BYP_MASK;");
                            writer.WriteLine("      ");
                        }

                        writer.WriteLine("			/* Enable Sensors Left */");
                        writer.WriteLine("			for(k=0; k < maxSns; k++)");
                        writer.WriteLine("			{");
                        writer.WriteLine("			    s = " + m_instanceName + "_IndexTableLeft[startIndex+k];");
                        writer.WriteLine("				" + m_instanceName + "_" + MethodL + "_EnableSensorLeft(s);");
                        writer.WriteLine("			}	");
                        writer.WriteLine("          ");
                        writer.WriteLine("			/* Set Flag that CapSensing in progres */");
                        writer.WriteLine("			" + m_instanceName + "_statusLeft |= " + m_instanceName + "_START_CAPSENSING;");
                        writer.WriteLine("          ");
                        writer.WriteLine("			/* Start PWM One Shout and PRS at one time */");
                        writer.WriteLine("			" + m_instanceName + "_CONTROL_LEFT |= " + m_instanceName + "_START_CAPSENSING;");
                        writer.WriteLine("		}");
                        writer.WriteLine("      ");
                        writer.WriteLine("		if (r < " + m_instanceName + "_TOTAL_SCANSLOT_COUNT_RIGHT)");
                        writer.WriteLine("		{");
                        writer.WriteLine("			");
                        writer.WriteLine("			/* Set Slot Settings  for Right  */");
                        writer.WriteLine("			" + m_instanceName + "_" + MethodR + "_SetSlotSettingsRight(r);");
                        writer.WriteLine("	        ");
                        writer.WriteLine("			/* Disable csBuf Right */");
                        writer.WriteLine("			" + m_instanceName + "_CAPS_CFG0_RIGHT &= ~" + m_instanceName + "_CSBUF_ENABLE;");
                        writer.WriteLine("          ");
                        writer.WriteLine("			maxSns = " + m_instanceName + "_ScanSlotTableRight[r].SnsCnt;");
                        writer.WriteLine("			startIndex = " + m_instanceName + "_ScanSlotTableRight[r].IndexOffset;");
                        writer.WriteLine("          ");

                        if (packParams.m_localParams.m_listCsHalfs[1].m_csdSubMethod == E_CSD_SUB_METHODS.IDACSinking)
                        {
                            writer.WriteLine("          /* Connect Right IDAC */");
                            writer.WriteLine("          " + m_instanceName + "_rb" + MethodR + "_" + m_csAmux + "_Connect(" + m_instanceName + "_IDAC_CHANNEL_RIGHT);");
                            writer.WriteLine("          ");
                        }
                        else if (packParams.m_localParams.m_listCsHalfs[1].m_csdSubMethod == E_CSD_SUB_METHODS.IDACDisable_RB)
                        {
                            writer.WriteLine("          /* Connect Right DSI output to Rb */");
                            writer.WriteLine("          *" + m_instanceName + "_RbPortPinTableRight[" + m_instanceName + "_currentRbleedRight] |= " + m_instanceName + "_BYP_MASK;");
                            writer.WriteLine("          ");
                        }

                        writer.WriteLine("			/* Enable Sensors Right */");
                        writer.WriteLine("			for(k=0; k < maxSns; k++)");
                        writer.WriteLine("			{");
                        writer.WriteLine("				s = " + m_instanceName + "_IndexTableRight[startIndex+k];");
                        writer.WriteLine("				" + m_instanceName + "_" + MethodR + "_EnableSensorRight(s);");
                        writer.WriteLine("			}");
                        writer.WriteLine("          ");
                        writer.WriteLine("			/* Set Flag that CapSensing in progres */");
                        writer.WriteLine("			" + m_instanceName + "_statusRight |= " + m_instanceName + "_START_CAPSENSING;");
                        writer.WriteLine("          ");
                        writer.WriteLine("			/* Start PWM One Shout and PRS at one time */");
                        writer.WriteLine("			" + m_instanceName + "_CONTROL_RIGHT |= " + m_instanceName + "_START_CAPSENSING;");
                        writer.WriteLine("		}");
                        writer.WriteLine("	    ");
                        writer.WriteLine("		/* Wait finish of CapSensing */");
                        writer.WriteLine("		while (1)");
                        writer.WriteLine("		{");
                        writer.WriteLine("			if ( (" + m_instanceName + "_statusLeft != " + m_instanceName + "_START_CAPSENSING) &&");
                        writer.WriteLine("				 (" + m_instanceName + "_statusRight != " + m_instanceName + "_START_CAPSENSING) )");
                        writer.WriteLine("			{");
                        writer.WriteLine("				break;");
                        writer.WriteLine("			}");
                        writer.WriteLine("		}");
                        writer.WriteLine("		");
                        writer.WriteLine("		if (l < " + m_instanceName + "_TOTAL_SCANSLOT_COUNT_LEFT)");
                        writer.WriteLine("		{");
                        writer.WriteLine("			/* Stop Capsensing */");
                        writer.WriteLine("			" + m_instanceName + "_CONTROL_LEFT &= ~" + m_instanceName + "_START_CAPSENSING;");
                        writer.WriteLine("			");
                        writer.WriteLine("			/* Read SlotResult from RwaCnt for Left side */");
                        writer.WriteLine("			rawIndex = " + m_instanceName + "_ScanSlotTableLeft[l].RawIndex;");
                        writer.WriteLine("			");
                        writer.WriteLine("			/* Read SlotResult from RwaCnt for Left side */");
                        writer.WriteLine("			" + m_instanceName + "_SlotResult[rawIndex] = " + m_instanceName + "_lb" + MethodL + "_" + m_csRawCnt + "_ReadCounter();");
                        writer.WriteLine("	        ");
                        writer.WriteLine("			startIndex = " + m_instanceName + "_ScanSlotTableLeft[l].IndexOffset;");
                        writer.WriteLine("			maxSns = " + m_instanceName + "_ScanSlotTableLeft[l].SnsCnt;");
                        writer.WriteLine("	        ");
                        writer.WriteLine("			/* Reset Raw Counter and PWM Left*/");
                        writer.WriteLine("			" + m_instanceName + "_CONTROL_LEFT |= " + m_instanceName + "_RESET_PWM_CNTR;");
                        writer.WriteLine("	        ");
                        writer.WriteLine("			/* Disable Sensors Right */");
                        writer.WriteLine("			for(k=0; k < maxSns; k++)");
                        writer.WriteLine("			{");
                        writer.WriteLine("				s = " + m_instanceName + "_IndexTableLeft[startIndex+k];");
                        writer.WriteLine("		        state = " + m_instanceName + "_SettingsTableLeft[l].DisableState;");
                        writer.WriteLine("				" + m_instanceName + "_" + MethodL + "_DisableSensorLeft(s, state);");
                        writer.WriteLine("			}");
                        writer.WriteLine("	        ");
                        writer.WriteLine("			" + m_instanceName + "_CONTROL_LEFT &= ~" + m_instanceName + "_RESET_PWM_CNTR;");
                        writer.WriteLine("	        ");
                        if (packParams.m_localParams.m_listCsHalfs[0].m_csdSubMethod == E_CSD_SUB_METHODS.IDACSinking)
                        {
                            writer.WriteLine("	         /* Disconnect Left IDAC */");
                            writer.WriteLine("	          " + m_instanceName + "_lb" + MethodL + "_" + m_csAmux + "_Disconnect(" + m_instanceName + "_IDAC_CHANNEL_LEFT);");
                            writer.WriteLine("	          ");
                        }
                        else if (packParams.m_localParams.m_listCsHalfs[0].m_csdSubMethod == E_CSD_SUB_METHODS.IDACDisable_RB)
                        {
                            writer.WriteLine("	         /* Disconnect Left DSI output to Rb */");
                            writer.WriteLine("            *" + m_instanceName + "_RbPortPinTableLeft[" + m_instanceName + "_currentRbleedLeft] &= ~" + m_instanceName + "_BYP_MASK;");
                            writer.WriteLine("	          ");
                        }
                        writer.WriteLine("			/* Enable Vref on AMUX */");
                        writer.WriteLine("			" + m_instanceName + "_CAPS_CFG0_LEFT |= " + m_instanceName + "_CSBUF_ENABLE;");
                        writer.WriteLine("			");
                        writer.WriteLine("			/* Scan enother scanslot */");
                        writer.WriteLine("			l++;");
                        writer.WriteLine("		}");
                        writer.WriteLine("      ");
                        writer.WriteLine("		if (r < " + m_instanceName + "_TOTAL_SCANSLOT_COUNT_RIGHT)");
                        writer.WriteLine("		{");
                        writer.WriteLine("			/* Stop Capsensing */");
                        writer.WriteLine("			" + m_instanceName + "_CONTROL_RIGHT &= ~" + m_instanceName + "_START_CAPSENSING;");
                        writer.WriteLine("			");
                        writer.WriteLine("		    /* Read SlotResult from RwaCnt  for Right side */");
                        writer.WriteLine("			rawIndex = " + m_instanceName + "_ScanSlotTableRight[r].RawIndex;");
                        writer.WriteLine("			");
                        writer.WriteLine("		    /* Read SlotResult from RwaCnt for Right side */");
                        writer.WriteLine("		    " + m_instanceName + "_SlotResult[rawIndex] = " + m_instanceName + "_rb" + MethodR + "_" + m_csRawCnt + "_ReadCounter();");
                        writer.WriteLine("	        ");
                        writer.WriteLine("		    startIndex = " + m_instanceName + "_ScanSlotTableRight[r].IndexOffset;");
                        writer.WriteLine("		    maxSns = " + m_instanceName + "_ScanSlotTableRight[r].SnsCnt;");
                        writer.WriteLine("	        ");
                        writer.WriteLine("			/* Reset Raw Counter and PWM Right*/");
                        writer.WriteLine("			" + m_instanceName + "_CONTROL_RIGHT |= " + m_instanceName + "_RESET_PWM_CNTR;");
                        writer.WriteLine("	        ");
                        writer.WriteLine("		    /* Disable Sensors Right */");
                        writer.WriteLine("			for(k=0; k < maxSns; k++)");
                        writer.WriteLine("			{");
                        writer.WriteLine("				s = " + m_instanceName + "_IndexTableRight[startIndex+k];");
                        writer.WriteLine("		        state = " + m_instanceName + "_SettingsTableRight[r].DisableState;");
                        writer.WriteLine("				" + m_instanceName + "_" + MethodR + "_DisableSensorRight(s, state);");
                        writer.WriteLine("			}");
                        writer.WriteLine("	        ");
                        writer.WriteLine("			" + m_instanceName + "_CONTROL_RIGHT &= ~" + m_instanceName + "_RESET_PWM_CNTR;");
                        writer.WriteLine("	        ");
                        if (packParams.m_localParams.m_listCsHalfs[1].m_csdSubMethod == E_CSD_SUB_METHODS.IDACSinking)
                        {
                            writer.WriteLine("	      /* Disconnect Right IDAC */");
                            writer.WriteLine("	      " + m_instanceName + "_rb" + MethodR + "_" + m_csAmux + "_Disconnect(" + m_instanceName + "_IDAC_CHANNEL_RIGHT);");
                            writer.WriteLine("	      ");
                        }
                        else if (packParams.m_localParams.m_listCsHalfs[1].m_csdSubMethod == E_CSD_SUB_METHODS.IDACDisable_RB)
                        {
                            writer.WriteLine("	      /* Disconnect Right DSI output to Rb */");
                            writer.WriteLine("        *" + m_instanceName + "_RbPortPinTableRight[" + m_instanceName + "_currentRbleedRight] &= ~" + m_instanceName + "_BYP_MASK;");
                            writer.WriteLine("	      ");
                        }
                        writer.WriteLine("			/* Enable Vref on AMUX */");
                        writer.WriteLine("			" + m_instanceName + "_CAPS_CFG0_RIGHT |= " + m_instanceName + "_CSBUF_ENABLE;");
                        writer.WriteLine("			");
                        writer.WriteLine("			/* Scan enother scanslot */");
                        writer.WriteLine("			r++;");
                        writer.WriteLine("	    }");
                        writer.WriteLine("	}");
                        writer.WriteLine("");
                        writer.WriteLine("}");
                        writer.WriteLine("");
                        #endregion
                    }
                }
                else
                {
                    //Not Paralle Full and Serial
                    foreach (CyAmuxBParams sbItem in packParams.m_localParams.m_listCsHalfs)
                        if (packParams.m_localParams.BCsHalfIsEnable(sbItem))
                        {
                            #region ScanAllSlots
                            writer.WriteLine("/*-----------------------------------------------------------------------------");
                            writer.WriteLine(" * FUNCTION NAME: void " + m_instanceName + "_ScanAllSlots(void)");
                            writer.WriteLine(" *-----------------------------------------------------------------------------");
                            writer.WriteLine(" * Summary: ");
                            writer.WriteLine(" *  Scans all of the configured Slots by calling CSA/CSD_ScanSlot for");
                            writer.WriteLine(" *  each slot. The slot order defined in Customizer.");
                            writer.WriteLine(" *	");
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
                            writer.WriteLine("void " + m_instanceName + "_ScanAllSlots" + "(void)");
                            writer.WriteLine("{");

                            string sSide = GetSideName(sbItem.m_side, packParams.Configuration);
                            string Method = sbItem.m_Method.ToString();
                            writer.WriteLine("    " + m_instanceName + "_" + Method + "_ScanAllSlots" + sSide + "();");

                            writer.WriteLine("}");
                            writer.WriteLine("");
                            #endregion
                        }

                }
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
            string SW_CLK = GetSW_CLK(packParams, sbItem); ;
            string DIG_CLK = packParams.GetPrefixForSchematic(sbItem) + m_baseDIG_CLK;
            string csPRS = "cPRS" + sbItem.GetPRSResolution();
            string CLK = Get_CLK(packParams, sbItem);

            #region Idac
            if (sbItem.m_isIdac)
            {
                #region Idac_SetValue
                writer.WriteLine("/*******************************************************************************");
                writer.WriteLine("* Function Name: void " + m_instanceName + "_" + Symbol + "_" + m_csIdac + "_SetValue" + sSide + "(uint8 value)");
                writer.WriteLine("********************************************************************************");
                writer.WriteLine("* Summary:");
                writer.WriteLine("*  Set DAC value");
                writer.WriteLine("*");
                writer.WriteLine("* Parameters:  ");
                writer.WriteLine("*  value:  Sets DAC value between 0 and 255.");
                writer.WriteLine("*");
                writer.WriteLine("* Return: ");
                writer.WriteLine("*  (void) ");
                writer.WriteLine("*");
                writer.WriteLine("* Theory: ");
                writer.WriteLine("*");
                writer.WriteLine("* Side Effects:");
                writer.WriteLine("*");
                writer.WriteLine("*******************************************************************************/");
                writer.WriteLine("void " + m_instanceName + "_" + Symbol + "_" + m_csIdac + "_SetValue" + sSide + "(uint8 value)");
                writer.WriteLine("{");
                writer.WriteLine("    " + m_instanceName + "_IDAC_DATA" + sSideUp + " = value;         /*  Set Value  */");
                writer.WriteLine("   ");
                writer.WriteLine("   #if defined(" + m_instanceName + "_IDAC_FIRST_SILICON" + sSideUp + ")");
                writer.WriteLine("    " + m_instanceName + "_IDAC_DATA" + sSideUp + " = value;        /*  TODO: Need for first version of silicon */");
                writer.WriteLine("   #endif");
                writer.WriteLine("");
                writer.WriteLine("}");

                #endregion

                #region
                writer.WriteLine("/*******************************************************************************");
                writer.WriteLine("* Function Name: " + m_instanceName + "_" + Symbol + "_" + m_csIdac + "_DacTrim" + sSide + "(void)");
                writer.WriteLine("********************************************************************************");
                writer.WriteLine("* Summary:");
                writer.WriteLine("*  Set the trim value for the given range.");
                writer.WriteLine("*");
                writer.WriteLine("* Parameters:  ");
                writer.WriteLine("*  value:  Sets DAC value between 0 and 255.");
                writer.WriteLine("*");
                writer.WriteLine("* Return: ");
                writer.WriteLine("*  (void) ");
                writer.WriteLine("*");
                writer.WriteLine("* Theory: ");
                writer.WriteLine("*");
                writer.WriteLine("* Side Effects:");
                writer.WriteLine("*");
                writer.WriteLine("*******************************************************************************/");
                writer.WriteLine("void " + m_instanceName + "_" + Symbol + "_" + m_csIdac + "_DacTrim" + sSide + "(void)");
                writer.WriteLine("{");
                writer.WriteLine(" 	uint8 mode;");
                writer.WriteLine("	");
                writer.WriteLine("	mode = ((" + m_instanceName + "_IDAC_CR0" + sSideUp + " & " + m_instanceName + "_IDAC_RANGE_MASK) >> 1);");
                writer.WriteLine("	if((" + m_instanceName + "_IDAC_IDIR_MASK & " + m_instanceName + "_IDAC_CR1" + sSideUp + ") == " + m_instanceName + "_IDAC_IDIR_SINK)");
                writer.WriteLine("	{");
                writer.WriteLine("		mode++;");
                writer.WriteLine("	}");
                writer.WriteLine("	");
                writer.WriteLine("	" + m_instanceName + "_IDAC_TR" + sSideUp + " = CY_GET_XTND_REG8((uint8 *)(" + m_instanceName + "_DAC_TRIM_BASE" + sSideUp + " + mode));");
                writer.WriteLine("}");

                #endregion

                #region Idac_Start

                writer.WriteLine("/*******************************************************************************");
                writer.WriteLine("* Function Name: void " + m_instanceName + "_" + Symbol + "_" + m_csIdac + "_Start" + sSide + "(void) ");
                writer.WriteLine("********************************************************************************");
                writer.WriteLine("* Summary:");
                writer.WriteLine("*  Set power level then turn on IDAC8.");
                writer.WriteLine("*");
                writer.WriteLine("* Parameters:  ");
                writer.WriteLine("*  power:   Sets power level between off (0) and (3) high power");
                writer.WriteLine("*");
                writer.WriteLine("* Return: ");
                writer.WriteLine("*  (void) ");
                writer.WriteLine("*");
                writer.WriteLine("* Theory: ");
                writer.WriteLine("*");
                writer.WriteLine("* Side Effects:");
                writer.WriteLine("*");
                writer.WriteLine("*******************************************************************************/");
                writer.WriteLine("void " + m_instanceName + "_" + Symbol + "_" + m_csIdac + "_Start" + sSide + "(void) ");
                writer.WriteLine("{");
                writer.WriteLine("	/* Hardware initiazation only needs to occure the first time */");
                writer.WriteLine("	if ( " + m_instanceName + "_idacInitVar" + sSide + " == 0)");
                writer.WriteLine("  	{");
                writer.WriteLine("    	    " + m_instanceName + "_idacInitVar" + sSide + " = 1;");
                writer.WriteLine("   	    ");
                writer.WriteLine("      	" + m_instanceName + "_IDAC_CR0" + sSideUp + " = " + m_instanceName + "_IDAC_MODE_I;");
                writer.WriteLine("          ");
                writer.WriteLine("      	" + m_instanceName + "_IDAC_CR1" + sSideUp + " = " + Idac_Polarity + " | " + m_instanceName + "_IDAC_IDIR_CTL_UDB;");
                writer.WriteLine("          ");
                writer.WriteLine("          " + m_instanceName + "_" + Symbol + "_" + m_csIdac + "_SetValue" + sSide + "(0x00);");
                writer.WriteLine("   	    ");
                writer.WriteLine("          " + m_instanceName + "_" + Symbol + "_" + m_csIdac + "_DacTrim" + sSide + "();");
                writer.WriteLine("   	}");
                writer.WriteLine("      ");
                writer.WriteLine("   	/* Enable power to DAC */");
                writer.WriteLine("   	" + m_instanceName + "_IDAC_PWRMGR" + sSideUp + " |= " + m_instanceName + "_IDAC_ACT_PWR_EN" + sSideUp + ";");
                writer.WriteLine("}");
                writer.WriteLine("");

                #endregion

                #region Idac_Stop
                writer.WriteLine("/*******************************************************************************");
                writer.WriteLine("* Function Name: void " + m_instanceName + "_" + Symbol + "_" + m_csIdac + "_Stop" + sSide + "(void) ");
                writer.WriteLine("********************************************************************************");
                writer.WriteLine("* Summary:");
                writer.WriteLine("*  Powers down IDAC8 to lowest power state.");
                writer.WriteLine("*");
                writer.WriteLine("* Parameters:  ");
                writer.WriteLine("*   (void)");
                writer.WriteLine("*");
                writer.WriteLine("* Return: ");
                writer.WriteLine("*  (void)");
                writer.WriteLine("*");
                writer.WriteLine("* Theory: ");
                writer.WriteLine("*");
                writer.WriteLine("* Side Effects:");
                writer.WriteLine("*");
                writer.WriteLine("*******************************************************************************/");
                writer.WriteLine("void " + m_instanceName + "_" + Symbol + "_" + m_csIdac + "_Stop" + sSide + "(void) ");
                writer.WriteLine("{");
                writer.WriteLine("   /* Disble power to DAC */");
                writer.WriteLine("   	" + m_instanceName + "_IDAC_PWRMGR" + sSideUp + " &= ~" + m_instanceName + "_IDAC_ACT_PWR_EN" + sSideUp + ";");
                writer.WriteLine("}");
                #endregion

                #region Idac_SetRange
                writer.WriteLine("/*******************************************************************************");
                writer.WriteLine("* Function Name: void " + m_instanceName + "_" + Symbol + "_" + m_csIdac + "_SetRange" + sSide + "(uint8 range)");
                writer.WriteLine("********************************************************************************");
                writer.WriteLine("* Summary:");
                writer.WriteLine("*  Set current range");
                writer.WriteLine("*");
                writer.WriteLine("* Parameters:  ");
                writer.WriteLine("*  Range:  Sets on of four valid ranges.");
                writer.WriteLine("*");
                writer.WriteLine("* Return: ");
                writer.WriteLine("*  (void) ");
                writer.WriteLine("*");
                writer.WriteLine("* Theory: ");
                writer.WriteLine("*");
                writer.WriteLine("* Side Effects:");
                writer.WriteLine("*");
                writer.WriteLine("*******************************************************************************/");
                writer.WriteLine("void " + m_instanceName + "_" + Symbol + "_" + m_csIdac + "_SetRange" + sSide + "(uint8 range)");
                writer.WriteLine("{");
                writer.WriteLine("   " + m_instanceName + "_IDAC_CR0" + sSideUp + " &= ~" + m_instanceName + "_IDAC_RANGE_MASK;       		/* Clear existing mode */");
                writer.WriteLine("   " + m_instanceName + "_IDAC_CR0" + sSideUp + " |= ( range & " + m_instanceName + "_IDAC_RANGE_MASK );  		/*  Set Range  */");
                writer.WriteLine("   " + m_instanceName + "_" + Symbol + "_" + m_csIdac + "_DacTrim" + sSide + "();");
                writer.WriteLine("}");
                writer.WriteLine("");
                #endregion

            }
            #endregion

            #region Rb
            if (sbItem.m_isRbEnable)
            {
                #region InitRb
                writer.WriteLine("/*-----------------------------------------------------------------------------");
                writer.WriteLine(" * FUNCTION NAME: void " + m_instanceName + "_" + Method + "_InitRb" + sSide + "(void)");
                writer.WriteLine(" *-----------------------------------------------------------------------------");
                writer.WriteLine(" * Summary: ");
                writer.WriteLine(" *  Set all Rbleed resistor to High-Z mode. Sets current Rbleed as first.");
                writer.WriteLine(" *	");
                writer.WriteLine(" * Parameters: ");
                writer.WriteLine(" *  (uint8) rbleed: Rbleed number.");
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
                writer.WriteLine("void " + m_instanceName + "_" + Method + "_InitRb" + sSide + "(void)");
                writer.WriteLine("{");
                writer.WriteLine("	uint8 i;");
                //writer.WriteLine("	reg8 *portreg;");
                writer.WriteLine("    uint8  rbCount = sizeof(" + m_instanceName + "_RbPortPinTable" + sSide + ")/sizeof(reg8 *);");
                writer.WriteLine("  ");
                writer.WriteLine("    /* Disable all Rb */");
                writer.WriteLine("    for(i=0; i<rbCount; i++)");
                writer.WriteLine("    {");
                //writer.WriteLine("      portreg = " + instanceName + "_RbPortPinTable" + sSideUp + "[i];");
                //writer.WriteLine("      ");
                //writer.WriteLine("      /* Make High-Z */");
                //writer.WriteLine("      CY_SET_REG8(portreg, (CY_GET_REG8(portreg) | " + instanceName + "_PRT_SET_TO_HIGH));");
                //writer.WriteLine("      ");
                //writer.WriteLine("      /* Disconnect from DSI output */");
                //writer.WriteLine("      CY_SET_REG8(portreg, (CY_GET_REG8(portreg) & (~" + instanceName + "_PRT_BYP_ENABLE)));");
                //writer.WriteLine("        /* Make High-Z */");
                //writer.WriteLine("        *" + m_instanceName + "_RbPortPinTable" + sSideUp + "[i] |= " + m_instanceName + "_PRT_DR_MASK;");
                //writer.WriteLine("        ");
                //writer.WriteLine("        /* Disconnect from DSI output */");
                //writer.WriteLine("        *" + m_instanceName + "_RbPortPinTable" + sSideUp + "[i] &= ~" + m_instanceName + "_BYP_MASK;");
                writer.WriteLine("        /* Make High-Z */");
                writer.WriteLine("        *" + m_instanceName + "_RbPortPinTable" + sSide + "[i] = " + m_instanceName + "_PRT_PC_HIGHZ;");
                writer.WriteLine("    }");
                writer.WriteLine("");
                writer.WriteLine("    " + m_instanceName + "_currentRbleed" + sSide + " = " + m_instanceName + "_RBLEED1;");
                writer.WriteLine("}");
                writer.WriteLine("");
                #endregion

                #region SetRb
                writer.WriteLine("/*-----------------------------------------------------------------------------");
                writer.WriteLine(" * FUNCTION NAME: void " + m_instanceName + "_" + Method + "_SetRBleed" + sSide + "(uint8 rbeed)");
                writer.WriteLine(" *-----------------------------------------------------------------------------");
                writer.WriteLine(" * Summary: ");
                writer.WriteLine(" *  Set Rbleed resistor from numbers varaints.");
                writer.WriteLine(" *	");
                writer.WriteLine(" * Parameters: ");
                writer.WriteLine(" *  (uint8) rbleed: Rbleed number.");
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
                writer.WriteLine("void " + m_instanceName + "_" + Method + "_SetRBleed" + sSide + "(uint8 rbleed)");
                writer.WriteLine("{");
                writer.WriteLine("    " + m_instanceName + "_currentRbleed" + sSide + " = rbleed;");
                writer.WriteLine("}");
                writer.WriteLine("");
                #endregion
            }
            #endregion

            #region Shield_Init
            if (sbItem.IsShieldElectrode())
            {
                writer.WriteLine("/*******************************************************************************");
                writer.WriteLine("* Function Name: void " + m_instanceName + "_" + Method + "_Shield_Init" + sSide + "(void)");
                writer.WriteLine("********************************************************************************");
                writer.WriteLine("* Summary:");
                writer.WriteLine("*  Set appropraite settings for all Shield electrodes.");
                writer.WriteLine("*");
                writer.WriteLine("* Parameters:  ");
                writer.WriteLine("*  None");
                writer.WriteLine("*");
                writer.WriteLine("* Return: ");
                writer.WriteLine("*  None");
                writer.WriteLine("*");
                writer.WriteLine("* Theory: ");
                writer.WriteLine("*");
                writer.WriteLine("* Side Effects:");
                writer.WriteLine("*");
                writer.WriteLine("*******************************************************************************/");
                writer.WriteLine("void " + m_instanceName + "_" + Method + "_Shield_Init" + sSide + "(void)");
                writer.WriteLine("{");
                writer.WriteLine("  uint8 port, shift, mask, i;");
                writer.WriteLine("	");
                writer.WriteLine("	for(i=0; i<" + m_instanceName + "_TOTAL_SHIELD_COUNT" + sSideUp + "; i++)");
                writer.WriteLine("    {	       ");
                writer.WriteLine("      port = " + m_instanceName + "_ShieldPortShiftTable" + sSide + "[i].port;");
                writer.WriteLine("	    shift = " + m_instanceName + "_ShieldPortShiftTable" + sSide + "[i].shift;");
                writer.WriteLine("	    mask = 1;");
                writer.WriteLine("        ");
                writer.WriteLine("	    mask <<= shift;");
                writer.WriteLine("        /* Disable dig_glbl_ctl to sensor */");
                writer.WriteLine("        *(" + m_instanceName + "_BASE_PRTDSI_CAPS + (" + m_instanceName + "_PRTDSI_OFFSET*port)) &= ~mask;");
                writer.WriteLine("    	");
                writer.WriteLine("      /* Set drive mode Strong, bypass enable */    ");
                writer.WriteLine("      *(" + m_instanceName + "_BASE_PRT_PC + (" + m_instanceName + "_PRT_PC_OFFSET*port) + shift) = " + m_instanceName + "_PRT_PC_SHIELD;");
                writer.WriteLine("    }");
                writer.WriteLine("}");
            }
            #endregion

            #region Start
            writer.WriteLine("/*-----------------------------------------------------------------------------");
            writer.WriteLine(" * FUNCTION NAME: void " + m_instanceName + "_" + Method + "_Start" + sSide + "(void)");
            writer.WriteLine(" *-----------------------------------------------------------------------------");
            writer.WriteLine(" * Summary: ");
            writer.WriteLine(" *  Initializes registers and starts the CapSense Component. This function ");
            writer.WriteLine(" *  should be called prior to calling any other CapSense Component functions. ");
            writer.WriteLine(" *	");
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
            writer.WriteLine("void " + m_instanceName + "_" + Method + "_Start" + sSide + "(void)");
            writer.WriteLine("{");
            if (sbItem.m_isRbEnable)
            {
                writer.WriteLine("	/* Workaround: Enable analog routing in Rb configuration*/");
                writer.WriteLine("	CY_SET_REG8(CYDEV_PM_ACT_CFG8, 0xFF);");
            }
            //(packParams.localParams.IsShowPrescalerCode(sbItem)
            if (sbItem.IsPrescaler())
            {
                writer.WriteLine("   ");
                writer.WriteLine("	/* Setup SW_CLK_Div_Serial */");
                writer.WriteLine("	" + m_instanceName + "_" + SW_CLK + "_Start();");
                writer.WriteLine("	");
            }
            writer.WriteLine("	/* Setup DIG_CLK_DivSerial */");
            writer.WriteLine("	" + m_instanceName + "_" + DIG_CLK + "_Start();");
            writer.WriteLine("	");
            writer.WriteLine("	/* Setup " + Symbol + "_csRawCnt */");
            writer.WriteLine("	" + m_instanceName + "_" + Symbol + "_" + m_csRawCnt + "_Start();");
            writer.WriteLine("		");
            writer.WriteLine("	/* Setup " + Symbol + "_csPWM */");
            writer.WriteLine("	" + m_instanceName + "_" + Symbol + "_" + m_csPWM + "_Start();");
            writer.WriteLine("	");
            writer.WriteLine("	/* Enable Comparator */");
            writer.WriteLine("	" + m_instanceName + "_" + Symbol + "_" + m_csComp + "_Start();");
            writer.WriteLine("	");
            writer.WriteLine("	/* Initialize AMux */");
            writer.WriteLine("	" + m_instanceName + "_" + Symbol + "_" + m_csAmux + "_Start();");
            writer.WriteLine("	");

            if (sbItem.m_isIdac)
            {
                writer.WriteLine("	/* Start Idac */");
                writer.WriteLine("	" + m_instanceName + "_" + Symbol + "_" + m_csIdac + "_Start" + sSide + "();");
                writer.WriteLine("	");
            }

            if (sbItem.IsPRS())
            {
                writer.WriteLine("	/* Start PRS */");
                writer.WriteLine("	" + m_instanceName + "_" + Symbol + "_" + csPRS + "_Start();");
                writer.WriteLine("	");
            }

            writer.WriteLine("	/* Connect Cmod, Cmp, Idac */");
            writer.WriteLine("	" + m_instanceName + "_" + Symbol + "_" + m_csAmux + "_Connect(" + m_instanceName + "_CMOD_CHANNEL" + sSideUp + ");");
            writer.WriteLine("	" + m_instanceName + "_" + Symbol + "_" + m_csAmux + "_Connect(" + m_instanceName + "_CMP_VP_CHANNEL" + sSideUp + ");");
            if (sbItem.m_csdSubMethod == E_CSD_SUB_METHODS.IDACSourcing)
            {
                writer.WriteLine("	" + m_instanceName + "_" + Symbol + "_" + m_csAmux + "_Connect(" + m_instanceName + "_IDAC_CHANNEL" + sSideUp + ");");
            }
            writer.WriteLine("");

            if (sbItem.m_isRbEnable)
            {
                writer.WriteLine("    /* Enable Rbleed */");
                writer.WriteLine("	" + m_instanceName + "_" + Method + "_InitRb" + sSide + "();");
                writer.WriteLine("		");
            }

            if (sbItem.IsShieldElectrode())
            {
                writer.WriteLine("    /* Set N Shield electrodes */");
                writer.WriteLine("    " + m_instanceName + "_" + Method + "_Shield_Init" + sSide + "();");
                writer.WriteLine("    ");
            }
            writer.WriteLine("    /* Clear all Sensor */");
            writer.WriteLine("	" + m_instanceName + "_" + Method + "_ClearSlots" + sSide + "();");
            writer.WriteLine("		");

            writer.WriteLine("    /* Setup ISR Component for CapSense */");
            writer.WriteLine("    " + m_instanceName + "_" + Symbol + "_" + m_csISR + "_Start();");
            writer.WriteLine("  ");
            writer.WriteLine("	  /* Set vector of " + m_instanceName + "_isr */ ");
            writer.WriteLine("    " + m_instanceName + "_" + Symbol + "_" + m_csISR + "_SetVector(" + m_instanceName + "_ISR" + sSide + ");");
            writer.WriteLine("	    ");
            writer.WriteLine("    /* Enable csBuf */");
            writer.WriteLine("  " + m_instanceName + "_CSBUF_PWRMGR" + sSideUp + " |= " + m_instanceName + "_CSBUF_PWR_ENABLE" + sSideUp + ";");
            writer.WriteLine("  " + m_instanceName + "_CAPS_CFG0" + sSideUp + " |= " + m_instanceName + "_CSBUF_ENABLE;");
            writer.WriteLine("	");
            if (sbItem.m_currectClk != null)
                if (sbItem.m_currectClk.IsDirectClock() == false)
                {
                    writer.WriteLine("  /* Enable the " + m_instanceName + " Clock */");
                    writer.WriteLine("  " + m_instanceName + "_" + CLK + "_Start();");
                }
            writer.WriteLine("}");
            writer.WriteLine("");
            #endregion

            #region Stop
            writer.WriteLine("/*-----------------------------------------------------------------------------");
            writer.WriteLine(" * FUNCTION NAME: void " + m_instanceName + "_" + Method + "_Stop" + sSide + "(void)");
            writer.WriteLine(" *-----------------------------------------------------------------------------");
            writer.WriteLine(" * Summary: ");
            writer.WriteLine(" *  Stops the sensor scanner, disables internal interrupts, and calls");
            writer.WriteLine(" *  CSA/CSD_ClearSensors() to reset all sensors to an inactive state.(Disconnect ");
            writer.WriteLine(" *  from AMUXBUS and put Open-Drain Drives-Low)");
            writer.WriteLine(" *	");
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
            writer.WriteLine("void " + m_instanceName + "_" + Method + "_Stop" + sSide + "(void)");
            writer.WriteLine("{");
            writer.WriteLine("	/* Stop Capsensing */");
            writer.WriteLine("	" + m_instanceName + "_CONTROL" + sSideUp + " &= ~" + m_instanceName + "_START_CAPSENSING;");
            writer.WriteLine("	");
            writer.WriteLine("	/* Disable the CapSense interrupt */");
            writer.WriteLine("    " + m_instanceName + "_" + Symbol + "_" + m_csISR + "_Disable();");
            writer.WriteLine("	");
            writer.WriteLine("	/* Clear all Sensor */");
            writer.WriteLine("	" + m_instanceName + "_" + Method + "_ClearSlots" + sSide + "();");
            writer.WriteLine("	");
            writer.WriteLine("	/* Disable Comparator */	");
            writer.WriteLine("	" + m_instanceName + "_" + Symbol + "_" + m_csComp + "_Stop();");
            writer.WriteLine("	");
            writer.WriteLine("	/* Disable csBuf */");
            writer.WriteLine("	" + m_instanceName + "_CAPS_CFG0" + sSideUp + " &= ~" + m_instanceName + "_CSBUF_ENABLE;");
            writer.WriteLine("	" + m_instanceName + "_CSBUF_PWRMGR" + sSideUp + " &= ~" + m_instanceName + "_CSBUF_PWR_ENABLE" + sSideUp + ";");
            if (sbItem.m_isIdac)
            {
                writer.WriteLine("	");
                writer.WriteLine("	/* Disable power to IDAC */");
                writer.WriteLine("	" + m_instanceName + "_" + Symbol + "_" + m_csIdac + "_Stop" + sSide + "();");
            }
            writer.WriteLine("}");
            writer.WriteLine("");
            #endregion

            #region ScanSlot
            writer.WriteLine("/*-----------------------------------------------------------------------------");
            writer.WriteLine(" * FUNCTION NAME: void " + m_instanceName + "_" + Method + "_ScanSlot" + sSide + "(uint8 slot)");
            writer.WriteLine(" *-----------------------------------------------------------------------------");
            writer.WriteLine(" * Summary: ");
            writer.WriteLine(" *  Scans the selected slot. The order of scanning and number of sensors to scan ");
            writer.WriteLine(" *  defined in slot position of terminal. ");
            writer.WriteLine(" *	");
            writer.WriteLine(" * Parameters: ");
            writer.WriteLine(" *  (uint8) slot: Slot number.");
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
            writer.WriteLine("void " + m_instanceName + "_" + Method + "_ScanSlot" + sSide + "(uint8 slot)");
            writer.WriteLine("{");
            writer.WriteLine("	uint8 i, s, state;");
            writer.WriteLine("	uint8 maxSns = " + m_instanceName + "_ScanSlotTable" + sSide + "[slot].SnsCnt;");
            writer.WriteLine("	uint8 startIndex = " + m_instanceName + "_ScanSlotTable" + sSide + "[slot].IndexOffset;");
            writer.WriteLine("	uint8 rawIndex = " + m_instanceName + "_ScanSlotTable" + sSide + "[slot].RawIndex; ");
            writer.WriteLine("");
            writer.WriteLine("	/* Set Slot Settings */");
            writer.WriteLine("	" + m_instanceName + "_" + Method + "_SetSlotSettings" + sSide + "(slot);");
            writer.WriteLine("	");
            writer.WriteLine("	/* Disable csBuf */");
            writer.WriteLine("	" + m_instanceName + "_CAPS_CFG0" + sSideUp + " &= ~" + m_instanceName + "_CSBUF_ENABLE;	");
            writer.WriteLine("  ");
            if (sbItem.m_csdSubMethod == E_CSD_SUB_METHODS.IDACSinking)
            {
                writer.WriteLine("	/* Connect IDAC */");
                writer.WriteLine("	" + m_instanceName + "_" + Symbol + "_" + m_csAmux + "_Connect(" + m_instanceName + "_IDAC_CHANNEL" + sSideUp + ");");
            }
            else if (sbItem.m_csdSubMethod == E_CSD_SUB_METHODS.IDACDisable_RB)
            {
                writer.WriteLine("	/* Connect DSI output to Rb */");
                writer.WriteLine("    *" + m_instanceName + "_RbPortPinTable" + sSide + "[" + m_instanceName + "_currentRbleed" + sSide + "] |= " + m_instanceName + "_BYP_MASK;");
            }
            writer.WriteLine("	");
            writer.WriteLine("	/* Enable Sensors */");
            writer.WriteLine("	for(i=0; i < maxSns; i++)");
            writer.WriteLine("	{");
            writer.WriteLine("		s = " + m_instanceName + "_IndexTable" + sSide + "[startIndex+i];");
            writer.WriteLine("		" + m_instanceName + "_" + Method + "_EnableSensor" + sSide + "(s);");
            writer.WriteLine("	}");
            writer.WriteLine("	");
            writer.WriteLine("	/* Set Flag that CapSensing in progres */");
            writer.WriteLine("	" + m_instanceName + "_status" + sSide + " |= " + m_instanceName + "_START_CAPSENSING;");
            writer.WriteLine("	");
            writer.WriteLine("	/* Start PWM One Shout and PRS at one time */");
            writer.WriteLine("	" + m_instanceName + "_CONTROL" + sSideUp + " |= " + m_instanceName + "_START_CAPSENSING;");
            writer.WriteLine("			");
            writer.WriteLine("	/* Wait finish of CapSensing */");
            writer.WriteLine("	while (" + m_instanceName + "_status" + sSide + " == " + m_instanceName + "_START_CAPSENSING)");
            writer.WriteLine("	{ ; }");
            writer.WriteLine("	");
            writer.WriteLine("	/* Stop Capsensing */");
            writer.WriteLine("	" + m_instanceName + "_CONTROL" + sSideUp + " &= ~" + m_instanceName + "_START_CAPSENSING;");
            writer.WriteLine("	");
            writer.WriteLine("	/* Read SlotResult from RawCnt */");
            writer.WriteLine("	" + m_instanceName + "_SlotResult[rawIndex] = " + m_instanceName + "_" + Symbol + "_" + m_csRawCnt + "_ReadCounter();");
            writer.WriteLine("	");
            writer.WriteLine("	for(i=0; i < maxSns; i++)");
            writer.WriteLine("	{");
            writer.WriteLine("		s = " + m_instanceName + "_IndexTable" + sSide + "[startIndex+i];");
            writer.WriteLine("		state = " + m_instanceName + "_SettingsTable" + sSide + "[slot].DisableState;");
            writer.WriteLine("		" + m_instanceName + "_" + Method + "_DisableSensor" + sSide + "(s, state);");
            writer.WriteLine("	}");
            writer.WriteLine("	");
            if (sbItem.m_csdSubMethod == E_CSD_SUB_METHODS.IDACSinking)
            {
                writer.WriteLine("	/* Disconnect IDAC */");
                writer.WriteLine("	" + m_instanceName + "_" + Symbol + "_" + m_csAmux + "_Disconnect(" + m_instanceName + "_IDAC_CHANNEL" + sSideUp + ");");
            }
            else if (sbItem.m_csdSubMethod == E_CSD_SUB_METHODS.IDACDisable_RB)
            {
                writer.WriteLine("	/* Disconnect DSI output to Rb */");
                writer.WriteLine("    *" + m_instanceName + "_RbPortPinTable" + sSide + "[" + m_instanceName + "_currentRbleed" + sSide + "] &= ~" + m_instanceName + "_BYP_MASK;");
            }
            writer.WriteLine("	");
            writer.WriteLine("	/* Enable Vref on AMUX */");
            writer.WriteLine("    " + m_instanceName + "_CAPS_CFG0" + sSideUp + " |= " + m_instanceName + "_CSBUF_ENABLE;");
            writer.WriteLine("}");
            #endregion

            #region ScanAllSlots
            writer.WriteLine("/*-----------------------------------------------------------------------------");
            writer.WriteLine(" * FUNCTION NAME: void " + m_instanceName + "_" + Method + "_ScanAllSlots" + sSide + "(void)");
            writer.WriteLine(" *-----------------------------------------------------------------------------");
            writer.WriteLine(" * Summary: ");
            writer.WriteLine(" *  Scans all of the configured Slots by calling CSA/CSD_ScanSlot for");
            writer.WriteLine(" *  each slot. The slot order defined in Customizer.");
            writer.WriteLine(" *	");
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
            writer.WriteLine("void " + m_instanceName + "_" + Method + "_ScanAllSlots" + sSide + "(void)");
            writer.WriteLine("{");
            writer.WriteLine("	uint8 i;");
            writer.WriteLine("	for(i=0; i < " + m_instanceName + "_TOTAL_SCANSLOT_COUNT" + sSideUp + ";i++)");
            writer.WriteLine("	{");
            writer.WriteLine("		" + m_instanceName + "_" + Method + "_ScanSlot" + sSide + "(i);");
            writer.WriteLine("	}");
            writer.WriteLine("}");
            writer.WriteLine("");
            #endregion

            #region SetSlotSettings
            writer.WriteLine("/*-----------------------------------------------------------------------------");
            writer.WriteLine(" * FUNCTION NAME: void " + m_instanceName + "_" + Method + "_SetSlotSettings" + sSide + "(uint8 slot)");
            writer.WriteLine(" *-----------------------------------------------------------------------------");
            writer.WriteLine(" * Summary: ");
            writer.WriteLine(" *  Sets the all Slot settings provided in Customizer. After srttings is changed ");
            writer.WriteLine(" *  the this funciotns look for commom settings and change values in ");
            writer.WriteLine(" *  SettingsChangeTable. ");
            writer.WriteLine(" *	");
            writer.WriteLine(" * Parameters: ");
            writer.WriteLine(" *  (Slot_Settings_XXX *) settings: Settings for CSA\\CSD(Different for those methods).");
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
            writer.WriteLine("void " + m_instanceName + "_" + Method + "_SetSlotSettings" + sSide + "(uint8 slot)");
            writer.WriteLine("{");
            if (sbItem.IsPrescaler())
            {
                writer.WriteLine("	uint8 prescalerPeriod = " + m_instanceName + "_SettingsTable" + sSide + "[slot].PrescalerPeriod;");
            }

            writer.WriteLine("	uint8 scanSpeed = " + m_instanceName + "_SettingsTable" + sSide + "[slot].ScanSpeed;");
            writer.WriteLine("	uint16 resolution = ( (((uint16) " + m_instanceName + "_SettingsTable" + sSide + "[slot].Resolution) << " + m_instanceName + "_PWM_WINDOW_SHIFT) | 0x00FFu );");

            if (sbItem.IsPRS())
            {
                writer.WriteLine("	uint8 prsPolynomial = " + m_instanceName + "_SettingsTable" + sSide + "[slot].PrsPolynomial;");
            }

            if (sbItem.m_isIdac)
            {
                writer.WriteLine("	uint8 idacRange = " + m_instanceName + "_SettingsTable" + sSide + "[slot].IdacRange;");
                writer.WriteLine("	uint8 idacSettings = " + m_instanceName + "_SettingsTable" + sSide + "[slot].IdacSettings;");
            }
            writer.WriteLine("	");

            if (sbItem.IsPrescaler())
            {
                writer.WriteLine("	/* Set Prescaller */");
                writer.WriteLine("	" + m_instanceName + "_" + SW_CLK + "_Stop();");
                writer.WriteLine("	" + m_instanceName + "_" + SW_CLK + "_WriteCounter(prescalerPeriod);");
                writer.WriteLine("	" + m_instanceName + "_" + SW_CLK + "_WritePeriod(prescalerPeriod);");
                if (sbItem.m_Prescaler == E_PRESCALER.UDB)
                {
                    writer.WriteLine("	" + m_instanceName + "_" + SW_CLK + "_WriteCompare(prescalerPeriod/2);");
                }
                writer.WriteLine("	" + m_instanceName + "_" + SW_CLK + "_Start();");
                writer.WriteLine("	");
            }

            writer.WriteLine("	/* PWM Resolution */");
            writer.WriteLine("	" + m_instanceName + "_" + Symbol + "_" + m_csPWM + "_WritePeriod(resolution);");
            writer.WriteLine("	" + m_instanceName + "_" + Symbol + "_" + m_csPWM + "_WriteCompare(resolution);");
            writer.WriteLine("	");

            writer.WriteLine("	/* Reset RawCnt and rearm PWM */");
            writer.WriteLine("	" + m_instanceName + "_CONTROL" + sSideUp + " |= " + m_instanceName + "_RESET_PWM_CNTR;");
            writer.WriteLine("	");
            writer.WriteLine("	/* Set ScanSpeed */");
            writer.WriteLine("	" + m_instanceName + "_" + DIG_CLK + "_Stop();");
            writer.WriteLine("	" + m_instanceName + "_" + DIG_CLK + "_WriteCounter(scanSpeed);");
            writer.WriteLine("	" + m_instanceName + "_" + DIG_CLK + "_WritePeriod(scanSpeed);");
            writer.WriteLine("	" + m_instanceName + "_" + DIG_CLK + "_Start();");
            writer.WriteLine("	");
            writer.WriteLine("	" + m_instanceName + "_CONTROL" + sSideUp + " &= ~" + m_instanceName + "_RESET_PWM_CNTR;");
            writer.WriteLine("	");
            

            if (sbItem.IsPRS())
            {
                writer.WriteLine("	/* PRS Polynomial */");
                writer.WriteLine("	" + m_instanceName + "_" + Symbol + "_" + csPRS + "_WritePolynomial(prsPolynomial);");
                writer.WriteLine("	" + m_instanceName + "_" + Symbol + "_" + csPRS + "_WriteSeed(0x01u);");
                writer.WriteLine("	");
            }

            if (sbItem.m_isIdac)
            {
                writer.WriteLine("	/* Set Idac */");
                writer.WriteLine("	" + m_instanceName + "_" + Symbol + "_" + m_csIdac + "_SetRange" + sSide + "(idacRange);");
                writer.WriteLine("	" + m_instanceName + "_" + Symbol + "_" + m_csIdac + "_SetValue" + sSide + "(idacSettings);");
            }
            writer.WriteLine("}");
            writer.WriteLine("");
            #endregion

            #region ClearSlots
            writer.WriteLine("/*-----------------------------------------------------------------------------");
            writer.WriteLine(" * FUNCTION NAME: void " + m_instanceName + "_" + Method + "_ClearSlots" + sSide + "(void)");
            writer.WriteLine(" *-----------------------------------------------------------------------------");
            writer.WriteLine(" * Summary: ");
            writer.WriteLine(" *  Clears all sensors to the non-sampling state by calling " + Method + "_DisableSensor() ");
            writer.WriteLine(" *  for each of the sensors.");
            writer.WriteLine(" *	");
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
            writer.WriteLine("void " + m_instanceName + "_" + Method + "_ClearSlots" + sSide + "(void)");
            writer.WriteLine("{");
            writer.WriteLine("	uint8 i, j, s;");
            writer.WriteLine("	uint8 startIndex, maxSns, state;");
            writer.WriteLine("  ");
            writer.WriteLine("	for (i=0; i < " + m_instanceName + "_TOTAL_SCANSLOT_COUNT" + sSideUp + "; i++)");
            writer.WriteLine("	{");
            writer.WriteLine("		startIndex = " + m_instanceName + "_ScanSlotTable" + sSide + "[i].IndexOffset;");
            writer.WriteLine("		maxSns = " + m_instanceName + "_ScanSlotTable" + sSide + "[i].SnsCnt;");
            writer.WriteLine("		state = " + m_instanceName + "_SettingsTable" + sSide + "[i].DisableState;");
            writer.WriteLine("	    ");
            writer.WriteLine("		/* Disable Sensors Right */");
            writer.WriteLine("		for(j=0; j < maxSns; j++)");
            writer.WriteLine("		{");
            writer.WriteLine("			s = " + m_instanceName + "_IndexTable" + sSide + "[startIndex+j];");
            writer.WriteLine("			" + m_instanceName + "_" + Method + "_DisableSensor" + sSide + "(s, state);");
            writer.WriteLine("		}");
            writer.WriteLine("	}");
            writer.WriteLine("}");
            #endregion

            #region ReadSlot
            writer.WriteLine("/*-----------------------------------------------------------------------------");
            writer.WriteLine(" * FUNCTION NAME: uint16 " + m_instanceName + "_" + Method + "_ReadSlot" + sSide + "(uint8 slot)");
            writer.WriteLine(" *-----------------------------------------------------------------------------");
            writer.WriteLine(" * Summary: ");
            writer.WriteLine(" *	Read Raw Data of scan slot");
            writer.WriteLine(" *	");
            writer.WriteLine(" * Parameters: ");
            writer.WriteLine(" *  (uint8) slot: Scan slot number.");
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
            writer.WriteLine("uint16 " + m_instanceName + "_" + Method + "_ReadSlot" + sSide + "(uint8 slot)");
            writer.WriteLine("{");
            writer.WriteLine("	uint8 index = " + m_instanceName + "_ScanSlotTable" + sSide + "[slot].RawIndex;");
            writer.WriteLine("	");
            writer.WriteLine("	return " + m_instanceName + "_SlotResult[index];");
            writer.WriteLine("}");
            writer.WriteLine("");
            #endregion

            #region EnableSensor
            writer.WriteLine("/*-----------------------------------------------------------------------------");
            writer.WriteLine(" * FUNCTION NAME: void " + m_instanceName + "_EnableSensor" + sSide + "(uint8 sensor, uint8 state)");
            writer.WriteLine(" *-----------------------------------------------------------------------------");
            writer.WriteLine(" * Summary: ");
            writer.WriteLine(" *  Configures the selected sensor to measure during the next measurement cycle. ");
            writer.WriteLine(" *  The port and sensor can be selected using the define provided in Customizerfor ");
            writer.WriteLine(" *  each sensor. Drive modes are modified to place the selected port and pin into ");
            writer.WriteLine(" *  Analog High-Z mode and connect to AMUXBUS. This also enables the comparator");
            writer.WriteLine(" *  function.");
            writer.WriteLine(" *	");
            writer.WriteLine(" * Parameters: ");
            writer.WriteLine(" *  (uint8) sensor: Sensor number.");
            writer.WriteLine(" *  (uint8) state: Inactive state of Sensor.");
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
            writer.WriteLine("void " + m_instanceName + "_" + Method + "_EnableSensor" + sSide + "(uint8 sensor)");
            writer.WriteLine("{");
            writer.WriteLine("	uint8 port = " + m_instanceName + "_PortShiftTable" + sSide + "[sensor].port;");
            writer.WriteLine("	uint8 shift = " + m_instanceName + "_PortShiftTable" + sSide + "[sensor].shift;");
            writer.WriteLine("	uint8 mask = 1;");
            writer.WriteLine("	");
            writer.WriteLine("	mask <<= shift;");
            writer.WriteLine("	");
            writer.WriteLine("	*(" + m_instanceName + "_BASE_PRT_PC + (" + m_instanceName + "_PRT_PC_OFFSET*port) + shift) = " + m_instanceName + "_PRT_PC_HIGHZ;");
            writer.WriteLine("  ");
            writer.WriteLine("	/* Connect from DSI output */");
            writer.WriteLine("	*(" + m_instanceName + "_BASE_PRTDSI_CAPS + (" + m_instanceName + "_PRTDSI_OFFSET*port)) |= mask;");
            writer.WriteLine("	");
            writer.WriteLine("	/* Connect to AMUX */");
            writer.WriteLine("	" + m_instanceName + "_" + Symbol + "_" + m_csAmux + "_Connect(sensor);	");
            writer.WriteLine("	    ");
            writer.WriteLine("}");
            #endregion

            #region DisableSensor
            writer.WriteLine("/*-----------------------------------------------------------------------------");
            writer.WriteLine(" * FUNCTION NAME: void " + m_instanceName + "_DisableSensor" + sSide + "(uint8 sensor, uint8 state)");
            writer.WriteLine(" *-----------------------------------------------------------------------------");
            writer.WriteLine(" * Summary: ");
            writer.WriteLine(" *  Disables the sensor selected by the user(The port and sensor can be selected ");
            writer.WriteLine(" *  using the define provided in Customizerfor each sensor). Thedrive mode is ");
            writer.WriteLine(" *  changed to Strong (001) and set to zero. This effectively grounds the sensor.");
            writer.WriteLine(" *  The connection from the port pin to the AMUXBUS is turned off. ");
            writer.WriteLine(" *	");
            writer.WriteLine(" * Parameters: ");
            writer.WriteLine(" *  (uint8) sensor: Sensor number.");
            writer.WriteLine(" *  (uint8) state: Inactive state of Sensor.");
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
            writer.WriteLine("void " + m_instanceName + "_" + Method + "_DisableSensor" + sSide + "(uint8 sensor, uint8 state)");
            writer.WriteLine("{");
            writer.WriteLine("	uint8 port = " + m_instanceName + "_PortShiftTable" + sSide + "[sensor].port;");
            writer.WriteLine("	uint8 shift = " + m_instanceName + "_PortShiftTable" + sSide + "[sensor].shift;");
            writer.WriteLine("	uint8 mask = 1;");
            writer.WriteLine("	");
            writer.WriteLine("	mask <<= shift;");
            writer.WriteLine("	");
            writer.WriteLine("	/* Disconnect from AMUX */");
            writer.WriteLine("	" + m_instanceName + "_" + Symbol + "_" + m_csAmux + "_Disconnect(sensor);");
            writer.WriteLine("		");
            writer.WriteLine("	/* Connect from DSI output */");
            writer.WriteLine("	*(" + m_instanceName + "_BASE_PRTDSI_CAPS + (" + m_instanceName + "_PRTDSI_OFFSET*port)) &= ~mask;");
            writer.WriteLine("		");
            writer.WriteLine("		switch(state)");
            writer.WriteLine("		{");
            writer.WriteLine("		    case " + m_instanceName + "_DISABLE_STATE_GND:");
            writer.WriteLine("	            *(" + m_instanceName + "_BASE_PRT_PC + (" + m_instanceName + "_PRT_PC_OFFSET*port) + shift) = " + m_instanceName + "_PRT_PC_GND;");
            writer.WriteLine("		    break;");
            writer.WriteLine("  ");
            writer.WriteLine("		    case " + m_instanceName + "_DISABLE_STATE_HIGHZ:");
            writer.WriteLine("	            *(" + m_instanceName + "_BASE_PRT_PC + (" + m_instanceName + "_PRT_PC_OFFSET*port) + shift) = " + m_instanceName + "_PRT_PC_HIGHZ;");
            writer.WriteLine("		    break;");
            writer.WriteLine("  ");
            writer.WriteLine("		    case " + m_instanceName + "_DISABLE_STATE_SHIELD:");
            writer.WriteLine("	            *(" + m_instanceName + "_BASE_PRT_PC + (" + m_instanceName + "_PRT_PC_OFFSET*port) + shift) = " + m_instanceName + "_PRT_PC_SHIELD;");
            writer.WriteLine("		    break;");
            writer.WriteLine("		    ");
            writer.WriteLine("		    default:");
            writer.WriteLine("	            *(" + m_instanceName + "_BASE_PRT_PC + (" + m_instanceName + "_PRT_PC_OFFSET*port) + shift) = " + m_instanceName + "_PRT_PC_GND;");
            writer.WriteLine("		    break;");
            writer.WriteLine("		}");
            writer.WriteLine("	    ");
            writer.WriteLine("}");
            #endregion

            #region GetPortPin
            //if (false)
            //{
            //    #region GetPortPin
            //    writer.WriteLine("/*-----------------------------------------------------------------------------");
            //    writer.WriteLine(" * FUNCTION NAME: PortPin* " + instanceName + "_" + Method + "_GetPortPin" + sSideUp + "(uint8 sensor)");
            //    writer.WriteLine(" *-----------------------------------------------------------------------------");
            //    writer.WriteLine(" * Summary: ");
            //    writer.WriteLine(" *	Get Port and Pin from Port Pin Table of requered sensor. ");
            //    writer.WriteLine(" *	");
            //    writer.WriteLine(" * Parameters: ");
            //    writer.WriteLine(" *  (uint8) sensor: Sensor number.");
            //    writer.WriteLine(" *");
            //    writer.WriteLine(" * Return:");
            //    writer.WriteLine(" *  None");
            //    writer.WriteLine(" *");
            //    writer.WriteLine(" * Theory:");
            //    writer.WriteLine(" *  See summary");
            //    writer.WriteLine(" *");
            //    writer.WriteLine(" * Side Effects:");
            //    writer.WriteLine(" *  None");
            //    writer.WriteLine(" *");
            //    writer.WriteLine(" *---------------------------------------------------------------------------*/");
            //    writer.WriteLine(""+instanceName+"_PortShift* " + instanceName + "_" + Method + "_GetPortPin" + sSideUp + "(uint8 sensor)");
            //    writer.WriteLine("{");
            //    writer.WriteLine("	"+instanceName+"_PortShift *ptr;");
            //    writer.WriteLine("	");
            //    writer.WriteLine("	if (sensor < " + instanceName + "_TOTAL_SENSOR_COUNT" + sSideUp + ")");
            //    writer.WriteLine("	{");
            //    writer.WriteLine("		ptr = &" + instanceName + "_PortShiftTable" + sSideUp + "[sensor];");
            //    writer.WriteLine("	}");
            //    writer.WriteLine("	else");
            //    writer.WriteLine("	{");
            //    writer.WriteLine("		ptr = 0;");
            //    writer.WriteLine("	}");
            //    writer.WriteLine("		");
            //    writer.WriteLine("	return ptr;");
            //    writer.WriteLine("}");
            //    #endregion
            //}
            #endregion

            #region

            #endregion
        }
        #endregion

        #region apiCollectCIntFunctionForSide
        public void ApiCollectCIntFunctionForSide(ref TextWriter writer, CyGeneralParams packParams, CyAmuxBParams sbItem)
        {
            string sSide = GetSideName(sbItem.m_side, packParams.Configuration);
            writer.WriteLine("/*******************************************************************************");
            writer.WriteLine("*  Place your includes, defines and code here");
            writer.WriteLine("*******************************************************************************/");
            writer.WriteLine("/* `#START " + m_instanceName + "_ISR" + sSide + "_intc` */");
            writer.WriteLine("  ");
            writer.WriteLine("/* `#END` */  ");
            writer.WriteLine("  ");
            writer.WriteLine("/*******************************************************************************");
            writer.WriteLine("* Function Name: " + m_instanceName + "_ISR" + sSide);
            writer.WriteLine("********************************************************************************");
            writer.WriteLine("* Summary:");
            writer.WriteLine("*  This ISR is executed when CAN core generate interrupt on one of evetns:");
            writer.WriteLine("*  Arb_lost, Overload, Bit_err, Stuff_err, Ack_err, Form_err, Crc_err, ");
            writer.WriteLine("*  Buss_off, Rxmsg_lost, Tx_msg or Rx_msg. The interrupt sources depend ");
            writer.WriteLine("*  on Wirard inputs.");
            writer.WriteLine("*");
            writer.WriteLine("* Parameters:  ");
            writer.WriteLine("*  void:  ");
            writer.WriteLine("*");
            writer.WriteLine("* Return: ");
            writer.WriteLine("*  (void)");
            writer.WriteLine("*");
            writer.WriteLine("* Theory: ");
            writer.WriteLine("*");
            writer.WriteLine("* Side Effects:");
            writer.WriteLine("*");
            writer.WriteLine("*******************************************************************************/");
            writer.WriteLine("CY_ISR(" + m_instanceName + "_ISR" + sSide + ")");
            writer.WriteLine("{");
            writer.WriteLine("  /*  Place your Interrupt code here. */");
            writer.WriteLine("  /* `#START " + m_instanceName + "_ISR" + sSide + "` */");
            writer.WriteLine("  ");
            writer.WriteLine("  /* `#END` */");
            writer.WriteLine("	" + m_instanceName + "_status" + sSide + " &= ~" + m_instanceName + "_START_CAPSENSING;");
            writer.WriteLine("}");
            writer.WriteLine("  ");
        }
        #endregion
    }
}
