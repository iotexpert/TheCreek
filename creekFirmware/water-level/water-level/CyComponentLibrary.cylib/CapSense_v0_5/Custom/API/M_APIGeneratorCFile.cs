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
        #region CollectApiCFile
        public void CollectApiCFile(ICyInstQuery_v1 query, ICyTerminalQuery_v1 termQuery, CyGeneralParams packParams, ref Dictionary<string, string> paramDict)
        {

            m_StringWriter writerCVariables = new m_StringWriter();
            writerCVariables.NewLine = "\n";
            TextWriter writerCFunctions = new StringWriter();
            writerCFunctions.NewLine = "\n";

            m_StringWriter writerCHLFunctions = new m_StringWriter();
            writerCHLFunctions.NewLine = "\n";

            m_StringWriter writerCHLFunctionsCentroid = new m_StringWriter();
            writerCHLFunctionsCentroid.NewLine = "\n";

            m_StringWriter writerCHLVariables = new m_StringWriter();
            writerCHLVariables.NewLine = "\n";

            TextWriter writerCIntFunctions = new StringWriter();
            writerCIntFunctions.NewLine = "\n";


            apiCollectCFunctionForParallelMode(ref writerCFunctions, packParams);

            apiCollectCHLVariables(ref writerCHLVariables,packParams);

            //Collect Data for Sides
            foreach (CyAmuxBParams item in packParams.localParams.listCsHalfs)
                if (packParams.localParams.bCsHalfIsEnable(item))
                {
                    apiCollectCVariables(ref writerCVariables, packParams, item);
                    apiCollectCFunctionForSide(ref writerCFunctions, packParams, item);
                    apiCollectCIntFunctionForSide(ref writerCIntFunctions, packParams, item);

                    apiCollectCHLFunctionForSide(ref writerCHLFunctions, packParams, item);
                }

            writerCVariables.WriteLine("uint16 " + instanceName + "_SlotResult[" + instanceName + "_TOTAL_SCANSLOT_COUNT] = {0};");

            apiCollectCHLFunctionBase(ref writerCHLFunctions, packParams);
            apiCollectCHLFunctionCentroid(ref writerCHLFunctionsCentroid, packParams);


            if (packParams.cyWidgetsList.GetListWithFullWidgetsHL().Count > 0)
            {
                paramDict.Add(pa_writerCHLVariables, writerCHLVariables.ToString());
            }
            
            paramDict.Add(pa_writerCVariables, writerCVariables.ToString());
            paramDict.Add(pa_writerCFunctions, writerCFunctions.ToString());
            paramDict.Add(pa_writerCIntFunctions, writerCIntFunctions.ToString());


            if (packParams.cyWidgetsList.GetListWithFullWidgetsHL().Count > 0)
            {             
                paramDict.Add(pa_writerCHLFunctions, writerCHLFunctions.ToString());
                paramDict.Add(pa_writerCHLFunctionsCentroid, writerCHLFunctionsCentroid.ToString());
            }

            
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


        public void apiCollectCVariables(ref m_StringWriter writer, CyGeneralParams packParams, CyAmuxBParams sbParametr)
        {
            eElSide side = sbParametr.side;
            string Symbol = SymbolGenerate(packParams, sbParametr, packParams.Configuration);
            string strSideName = GetSideName(side, packParams.Configuration);
            string strSideNameUpper = GetSideNameUpper(side, packParams.Configuration);
            string Method = sbParametr.Method.ToString();
            TextWriter writerInternal = new StringWriter();
            writerInternal.NewLine = "\n";

            #region  Filling  ScanSlotTable & Index_Table
            writer.WriteLine("");
            writer.WriteLine("/* fields: Index, IndexOffset, SnsCnt */");
            writer.WriteLine(""+instanceName+"_ScanSlot " + instanceName + "_ScanSlotTable" + strSideName + "[] = ");
            writer.WriteLine("{");

            writerInternal.Write("uint8 " + instanceName + "_IndexTable" + strSideName + "[] = {");//0,1,2,0,1,2};");

            string strIndexTable = "";
            int i_IndexTable = 0;
            //{0, 0, 1},
            for (int i = 0; i < packParams.cyScanSlotsList.GetListFromSide(side).Count; i++)
            {
                //Calculate offset in RawCount array
                int offset = 0;
                if (sbParametr.side == eElSide.Right) offset = packParams.cyScanSlotsList.GetListFromSide(eElSide.Left).Count;

                ElScanSlot ss = packParams.cyScanSlotsList.GetListFromSide(side)[i];
                
                string widgetNumber = packParams.cyWidgetsList.GetIndexInListWithFullWidgetsHL(ss.Widget).ToString();
                if (ss.Widget.type == sensorType.Generic) widgetNumber = "0xffu";

                string str = "	{" + (offset + i).ToString() + ", " + i_IndexTable + ", " + ss.GetTerminals().Count + ", " + widgetNumber + ", " + "0" + "}";
                if (i < packParams.cyScanSlotsList.GetListFromSide(side).Count - 1)
                    str += ",";
                writer.WriteLine(str);

                //Filling Index_Table
                foreach (ElTerminal item in ss.GetTerminals())
                {
                    i_IndexTable++;
                    strIndexTable += packParams.cyWidgetsList.GetListTerminalsFromSide(side).IndexOf(item).ToString() + ", ";
                }
            }
            writer.WriteLine("};");
            writer.WriteLine("");

            //Filling Index_Table
            if (strIndexTable.Length > 1)
                strIndexTable = strIndexTable.Remove(strIndexTable.Length - 2);
            writerInternal.Write(strIndexTable + "};");
            writer.WriteLine(writerInternal.ToString());
            writer.WriteLine("");
            #endregion
            
            #region  SettingsTable

            writer.Write(instanceName+"_"+Method + "_Settings" + strSideName + " ");

            writer.WriteLine(instanceName + "_SettingsTable" + strSideName + "[] = ");
            writer.WriteLine("{");
            for (int i = 0; i < packParams.cyScanSlotsList.GetListFromSide(side).Count; i++)
            {
                ElScanSlot ss = packParams.cyScanSlotsList.GetListFromSide(side)[i];
                writer.Write("\t{");

                if (sbParametr.isIdac)
                {
                    writer.Write(instanceName+GetIDACRangeStr(ss.SSProperties.IDACRange));				/* 1-3 range exist, multiplier for IDAC */
                    writer.Write(", ");
                    writer.Write(ss.SSProperties.IDACSettings.Value);						/* IDAC settings for measurament */
                    writer.Write(", ");
                }
                //CSA Properties
                if (sbParametr.Method == eCapSenseMode.CSA)
                {
                    writer.Write(ss.SSProperties.baseProps.Scanlength.Value);					/* IDAC measure = SA - ScanLength */
                    writer.Write(", ");
                    writer.Write(ss.SSProperties.baseProps.SettlingTime.Value);					/*SettlingTime */
                    writer.Write(", ");
                    writer.Write("0x00");			 		/* IdacCode */
                    writer.Write(", ");
                }
                if (sbParametr.IsPrescaler())
                {
                    
                    writer.Write(ss.SSProperties.baseProps.PrescPer.Value-1);			/* Prescaler for CapsClock */
                    writer.Write(", ");
                }

                //CSD Properties
                if (sbParametr.Method == eCapSenseMode.CSD)
                {
                    writer.Write(ss.SSProperties.GetResolution(instanceName));				/* PWM Resolution */
                    writer.Write(", ");
                    writer.Write(ss.SSProperties.GetScanSpeed(instanceName));				/* Speed of scanning */
                    writer.Write(", ");
                    if (sbParametr.IsPRS())
                    {
                        writer.Write("0x00");			/* Prs signal polinomial to "Break befor Make" */
                        writer.Write(", ");
                    }
                }
                //writer.Write(ss.SSProperties.baseProps.CIS); 			/* (Hi-Z;GND; Shield) */

                //Removing ", "
                if (writer.Get0Length() > 4)
                    writer.Remove(writer.Get0Length() - 2);

                if (i < packParams.cyScanSlotsList.GetListFromSide(side).Count - 1)
                    writer.WriteLine("},");
                else
                    writer.WriteLine("}");
            }
            writer.WriteLine("};");
            writer.WriteLine("");
            #endregion

            #region PortMask Generation
            //PortMask Generation
            writer.WriteLine(""+instanceName+"_PortMask " + instanceName + "_PortMaskTable" + strSideName + "[] = ");
            writer.WriteLine("{");
            for (int i = 0; i < packParams.cyWidgetsList.GetListTerminalsFromSide(side).Count; i++)
            {
                string str = instanceName + "_" + Symbol + "_" + csPort + "__" + packParams.cyWidgetsList.GetListTerminalsFromSide(side)[i].ToString();
                str = "	{" + str + "__PORT" + ", " + str + "__MASK" + "}";
                if (i < packParams.cyWidgetsList.GetListTerminalsFromSide(side).Count - 1)
                    str += ",";
                writer.WriteLine(str);
            }
            writer.WriteLine("};");
            writer.WriteLine("");
            #endregion

            #region Rb Gen
            if (sbParametr.isRbEnable)
            {
                writer.WriteLine("/* Different for Left and Right sides */");
                writer.WriteLine("reg8 *" + instanceName + "_RbPortPinTable" + strSideName + "[] = ");
                writer.WriteLine("{");
                for (int i = 0; i < sbParametr.countRb; i++)
                {
                    writer.Write(instanceName + "_" + Symbol + "_cRb" + i + "__"+packParams.GetAliasRbByIndex(sbParametr,i)+"__PC");
                    if (i != sbParametr.countRb - 1)
                        writer.WriteLine(",");
                }
                writer.WriteLine("");
                writer.WriteLine("};");
            }
            writer.WriteLine("");
            if(sbParametr.isRbEnable)
            writer.WriteLine("uint8 " + instanceName + "_CurrentRbleed" + strSideName + " = " + instanceName + "_MAX_RB_NUMBER;");
            #endregion

            #region Shield Gen
            if (sbParametr.IsShieldElectrode())
            {
                writer.WriteLine(""+instanceName+"_PortMask " + instanceName + "_ShieldPortMaskTable" + strSideName + "[] = ");
                writer.WriteLine("{");
                for (int i = 0; i < sbParametr.ShieldElectrodeCount(); i++)
                {
                    string str = instanceName + "_" + Symbol + "_" + csShield + "__" + packParams.GetAliasShieldByIndex(sbParametr, i);
                    str = "	{" + str + "__PORT" + ", " + str + "__MASK" + "}";
                    if (i < sbParametr.ShieldElectrodeCount() - 1)
                        str += ",";
                    writer.WriteLine(str);
                }
                writer.WriteLine("};");
            }
            #endregion

            writer.WriteLine("");
            writer.WriteLine("uint8 " + instanceName + "_Status" + strSideName + " = 0;");
            writer.WriteLine("");

            writer.WriteLine("");
            writer.WriteLine("uint8 " + instanceName + "_IDAC_initVar" + strSideName + " = 0;");
            writer.WriteLine("");
        }
        #endregion

        #region  apiCollectCFunctionForParallelMode
        public void apiCollectCFunctionForParallelMode(ref TextWriter writer, CyGeneralParams packParams)
        {
            //if (packParams.localParams.isParallel())
            {
                string MethodL = packParams.localParams.listCsHalfs[0].Method.ToString();
                string MethodR = packParams.localParams.listCsHalfs[1].Method.ToString();

                #region Start
                writer.WriteLine("/*-----------------------------------------------------------------------------");
                writer.WriteLine(" * FUNCTION NAME:void " + instanceName + "_Start(void)");
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
                writer.WriteLine("void " + instanceName + "_Start(void)");
                writer.WriteLine("{");
                foreach (CyAmuxBParams item in packParams.localParams.listCsHalfs)
                    if (packParams.localParams.bCsHalfIsEnable(item))
                    {
                        string strSideName = GetSideName(item.side, packParams.Configuration);
                        string Method = item.Method.ToString();
                        writer.WriteLine("	" + instanceName + "_" + Method + "_Start" + strSideName + "();");
                    }
                writer.WriteLine("}");

                #endregion

                #region Stop
                writer.WriteLine("/*-----------------------------------------------------------------------------");
                writer.WriteLine(" * FUNCTION NAME: void " + instanceName + "_Stop(void)");
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
                writer.WriteLine("void " + instanceName + "_Stop(void)");
                writer.WriteLine("{");
                foreach (CyAmuxBParams item in packParams.localParams.listCsHalfs)
                    if (packParams.localParams.bCsHalfIsEnable(item))
                    {
                        string strSideName = GetSideName(item.side, packParams.Configuration);
                        string Method = item.Method.ToString();
                        writer.WriteLine("	" + instanceName + "_" + Method + "_Stop" + strSideName + "();");

                    }
                writer.WriteLine("}");
                #endregion


                if (packParams.localParams.isParallelFull())
                {
                    //Paralle Sync
                    if (packParams.Configuration == eMConfiguration.emParallelAsynchron)
                    {
                        #region ScanAllSlots Asynchron
                        writer.WriteLine("/*-----------------------------------------------------------------------------");
                        writer.WriteLine(" * FUNCTION NAME: void " + instanceName + "_ScanAllSlots(void)");
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
                        writer.WriteLine("void " + instanceName + "_ScanAllSlots(void)");
                        writer.WriteLine("{");
                        writer.WriteLine("	uint8 l = 0;");
                        writer.WriteLine("	uint8 r = 0;");
                        writer.WriteLine("	uint8 finish, k, s;/* if 1 - Left / 0 - Right */");
                        writer.WriteLine("	uint8 MaxSns = " + instanceName + "_ScanSlotTableLeft[l].SnsCnt;");
                        writer.WriteLine("	uint8 StartIndex = " + instanceName + "_ScanSlotTableLeft[l].IndexOffset;");
                        writer.WriteLine("	uint8 RawIndex;");
                        writer.WriteLine("");
                        writer.WriteLine("	while ((l < " + instanceName + "_TOTAL_SCANSLOT_COUNT_LEFT) || (r < " + instanceName + "_TOTAL_SCANSLOT_COUNT_RIGHT))");
                        writer.WriteLine("	{");
                        writer.WriteLine("		/* First time need to initialize both */");
                        writer.WriteLine("		if((l == 0) && (r == 0))");
                        writer.WriteLine("		{");
                        writer.WriteLine("			/* Set Slot Settings  for Left and Right */");
                        writer.WriteLine("			" + instanceName + "_" + MethodL + "_SetSlotSettingsLeft(l);");
                        writer.WriteLine("			" + instanceName + "_" + MethodR + "_SetSlotSettingsRight(r);			");
                        writer.WriteLine("			");
                        writer.WriteLine("			/* Reset Raw Counter and PWM Left*/");
                        //if (false)
                        //{
                        //writer.WriteLine("			k = " + instanceName + "_lb" + MethodL + "_" + csRawCnt + "_ReadControlRegister();");
                        //writer.WriteLine("			k |= " + instanceName + "_lb" + MethodL + "_" + csRawCnt + "_CTRL_RESET;");
                        //writer.WriteLine("			" + instanceName + "_lb" + MethodL + "_" + csRawCnt + "_WriteControlRegister(k);");
                        //writer.WriteLine("			");
                        //}
                        //else
						{
						writer.WriteLine("			" + instanceName + "_CONTROL_LEFT |= " + instanceName + "_RESET_PWM_CNTR;");
						writer.WriteLine("			" + instanceName + "_CONTROL_LEFT &= ~" + instanceName + "_RESET_PWM_CNTR;");
                        }
                        writer.WriteLine("			/* Reset Raw Counter and PWM Right*/");
                        //if(false)
                        //{
                        //writer.WriteLine("			k = " + instanceName + "_rb" + MethodR + "_" + csRawCnt + "_ReadControlRegister();");
                        //writer.WriteLine("			k |= " + instanceName + "_rb" + MethodR + "_" + csRawCnt + "_CTRL_RESET;");
                        //writer.WriteLine("			" + instanceName + "_rb" + MethodR + "_" + csRawCnt + "_WriteControlRegister(k);");
                        //}
                        //else
						{
						writer.WriteLine("			" + instanceName + "_CONTROL_RIGHT |= " + instanceName + "_RESET_PWM_CNTR;");
						writer.WriteLine("			" + instanceName + "_CONTROL_RIGHT &= ~" + instanceName + "_RESET_PWM_CNTR;");
						}
                        writer.WriteLine("		");
                        writer.WriteLine("			/* Disable csBuf Left and Right */");
                        writer.WriteLine("			" + instanceName + "_CAPS_CFG0_LEFT &= ~" + instanceName + "_CSBUF_CONNECT;");
                        writer.WriteLine("			" + instanceName + "_CAPS_CFG0_RIGHT &= ~" + instanceName + "_CSBUF_CONNECT;");
                        writer.WriteLine("			");
                        writer.WriteLine("			/* Enable Sensors Left */");
                        writer.WriteLine("			for(k=0; k < MaxSns; k++)");
                        writer.WriteLine("			{");
                        writer.WriteLine("				s = " + instanceName + "_IndexTableLeft[StartIndex+k];");
                        writer.WriteLine("				" + instanceName + "_" + MethodL + "_EnableSensorLeft(s);");
                        writer.WriteLine("			}	");
                        writer.WriteLine("			");
                        writer.WriteLine("			MaxSns = " + instanceName + "_ScanSlotTableRight[r].SnsCnt;");
                        writer.WriteLine("			StartIndex = " + instanceName + "_ScanSlotTableRight[r].IndexOffset;");
                        writer.WriteLine("			/* Enable Sensors Left */");
                        writer.WriteLine("			for(k=0; k < MaxSns; k++)");
                        writer.WriteLine("			{");
                        writer.WriteLine("				s = " + instanceName + "_IndexTableRight[StartIndex+k];");
                        writer.WriteLine("				" + instanceName + "_" + MethodR + "_EnableSensorRight(s);");
                        writer.WriteLine("			}	");
                        writer.WriteLine("   ");
                        writer.WriteLine("			/* Set Flag that CapSensing in progres */");
                        writer.WriteLine("			" + instanceName + "_StatusLeft |= " + instanceName + "_START_CAPSENSING;");
                        writer.WriteLine("			" + instanceName + "_StatusRight |= " + instanceName + "_START_CAPSENSING;");
                        writer.WriteLine("    ");
                        writer.WriteLine("			/* Start PWM One Shout and PRS at one time */");
                        writer.WriteLine("			" + instanceName + "_CONTROL_LEFT |= " + instanceName + "_START_CAPSENSING;");
                        writer.WriteLine("			" + instanceName + "_CONTROL_RIGHT |= " + instanceName + "_START_CAPSENSING;");
                        writer.WriteLine("		}");
                        writer.WriteLine("		else");
                        writer.WriteLine("		{");
                        writer.WriteLine("			if (finish && l < " + instanceName + "_TOTAL_SCANSLOT_COUNT_LEFT) /* Start new scan of Left half */");
                        writer.WriteLine("			{");
                        writer.WriteLine("				/* Set Slot Settings  for Left  */");
                        writer.WriteLine("				" + instanceName + "_" + MethodL + "_SetSlotSettingsLeft(l);");
                        writer.WriteLine("			");
                        //if (false)
                        //{
                        //writer.WriteLine("				/* Reset Raw Counter and PWM Left */");
                        //writer.WriteLine("				k = " + instanceName + "_lb" + MethodL + "_" + csRawCnt + "_ReadControlRegister();");
                        //writer.WriteLine("				k |= " + instanceName + "_lb" + MethodL + "_" + csRawCnt + "_CTRL_RESET;");
                        //writer.WriteLine("				" + instanceName + "_lb" + MethodL + "_" + csRawCnt + "_WriteControlRegister(k);");
                        //}
                        //else
						{
						writer.WriteLine("				" + instanceName + "_CONTROL_LEFT |= " + instanceName + "_RESET_PWM_CNTR;");
						writer.WriteLine("				" + instanceName + "_CONTROL_LEFT &= ~" + instanceName + "_RESET_PWM_CNTR;");
						}

                        writer.WriteLine("		");
                        writer.WriteLine("				/* Disable csBuf Left */");
                        writer.WriteLine("				" + instanceName + "_CAPS_CFG0_LEFT &= ~" + instanceName + "_CSBUF_CONNECT;");
                        writer.WriteLine("    ");
                        writer.WriteLine("				MaxSns = " + instanceName + "_ScanSlotTableLeft[l].SnsCnt;");
                        writer.WriteLine("				StartIndex = " + instanceName + "_ScanSlotTableLeft[l].IndexOffset;");
                        writer.WriteLine("				/* Enable Sensors Left */");
                        writer.WriteLine("				for(k=0; k < MaxSns; k++)");
                        writer.WriteLine("				{");
                        writer.WriteLine("					s = " + instanceName + "_IndexTableLeft[StartIndex+k];");
                        writer.WriteLine("					" + instanceName + "_" + MethodL + "_EnableSensorLeft(s);");
                        writer.WriteLine("				}	");
                        writer.WriteLine("    ");
                        writer.WriteLine("				/* Set Flag that CapSensing in progres */");
                        writer.WriteLine("				" + instanceName + "_StatusLeft |= " + instanceName + "_START_CAPSENSING;");
                        writer.WriteLine("");
                        writer.WriteLine("				/* Start PWM One Shout and PRS at one time */");
                        writer.WriteLine("				" + instanceName + "_CONTROL_LEFT |= " + instanceName + "_START_CAPSENSING;");
                        writer.WriteLine("				");
                        writer.WriteLine("			}");
                        writer.WriteLine("			else	/* Start new scan of Right half */");
                        writer.WriteLine("			{");
                        writer.WriteLine("				if (r < " + instanceName + "_TOTAL_SCANSLOT_COUNT_RIGHT)");
                        writer.WriteLine("				{");
                        writer.WriteLine("					/* Set Slot Settings  for Right  */");
                        writer.WriteLine("					" + instanceName + "_" + MethodR + "_SetSlotSettingsRight(r);");
                        writer.WriteLine("			");
                        //if (false)
                        //{
                        //writer.WriteLine("					/* Reset Raw Counter and PWM Right */");
                        //writer.WriteLine("					k = " + instanceName + "_rb" + MethodR + "_" + csRawCnt + "_ReadControlRegister();");
                        //writer.WriteLine("					k |= " + instanceName + "_rb" + MethodR + "_" + csRawCnt + "_CTRL_RESET;");
                        //writer.WriteLine("					" + instanceName + "_rb" + MethodR + "_" + csRawCnt + "_WriteControlRegister(k);");
                        //}
                        //else
						{
						writer.WriteLine("					" + instanceName + "_CONTROL_RIGHT |= " + instanceName + "_RESET_PWM_CNTR;");
						writer.WriteLine("					" + instanceName + "_CONTROL_RIGHT &= ~" + instanceName + "_RESET_PWM_CNTR;");
						}
                        writer.WriteLine("		");
                        writer.WriteLine("					/* Disable csBuf Right */");
                        writer.WriteLine("					" + instanceName + "_CAPS_CFG0_RIGHT &= ~" + instanceName + "_CSBUF_CONNECT;");
                        writer.WriteLine("    ");
                        writer.WriteLine("					MaxSns = " + instanceName + "_ScanSlotTableRight[r].SnsCnt;");
                        writer.WriteLine("					StartIndex = " + instanceName + "_ScanSlotTableRight[r].IndexOffset;");
                        writer.WriteLine("					/* Enable Sensors Right */");
                        writer.WriteLine("					for(k=0; k < MaxSns; k++)");
                        writer.WriteLine("					{");
                        writer.WriteLine("						s = " + instanceName + "_IndexTableRight[StartIndex+k];");
                        writer.WriteLine("						" + instanceName + "_" + MethodR + "_EnableSensorRight(s);");
                        writer.WriteLine("					}	");
                        writer.WriteLine("    ");
                        writer.WriteLine("					/* Set Flag that CapSensing in progres */");
                        writer.WriteLine("					" + instanceName + "_StatusRight |= " + instanceName + "_START_CAPSENSING;");
                        writer.WriteLine("");
                        writer.WriteLine("					/* Start PWM One Shout and PRS at one time */");
                        writer.WriteLine("					" + instanceName + "_CONTROL_RIGHT |= " + instanceName + "_START_CAPSENSING;");
                        writer.WriteLine("				}");
                        writer.WriteLine("			}");
                        writer.WriteLine("		}");
                        writer.WriteLine("		");
                        writer.WriteLine("		");
                        writer.WriteLine("		/* Wait finish of CapSensing */");
                        writer.WriteLine("		while (1)");
                        writer.WriteLine("		{");
                        writer.WriteLine("			if (" + instanceName + "_StatusLeft != " + instanceName + "_START_CAPSENSING)");
                        writer.WriteLine("			{");
                        writer.WriteLine("				finish = 1;");
                        writer.WriteLine("				break;");
                        writer.WriteLine("			}");
                        writer.WriteLine("			");
                        writer.WriteLine("			if (" + instanceName + "_StatusRight != " + instanceName + "_START_CAPSENSING)");
                        writer.WriteLine("			{");
                        writer.WriteLine("				finish = 0;");
                        writer.WriteLine("				break;");
                        writer.WriteLine("			}");
                        writer.WriteLine("		}");
                        writer.WriteLine("		");
                        writer.WriteLine("		if ((finish) && (l < " + instanceName + "_TOTAL_SCANSLOT_COUNT_LEFT))");
                        writer.WriteLine("		{");
                        writer.WriteLine("			/* Stop Capsensing */");
                        writer.WriteLine("			" + instanceName + "_CONTROL_LEFT &= ~" + instanceName + "_START_CAPSENSING;");
                        writer.WriteLine("			");
                        writer.WriteLine("			/* Read SlotResult from RwaCnt  for Left side */");
                        writer.WriteLine("			RawIndex = " + instanceName + "_ScanSlotTableLeft[l].RawIndex;");
                        writer.WriteLine("			");
                        writer.WriteLine("			/* Read SlotResult from RwaCnt for Left side */");
                        writer.WriteLine("			" + instanceName + "_SlotResult[RawIndex] = " + instanceName + "_lb" + MethodL + "_" + csRawCnt + "_ReadCounter();");
                        writer.WriteLine("	    ");
                        writer.WriteLine("			StartIndex = " + instanceName + "_ScanSlotTableLeft[l].IndexOffset;");
                        writer.WriteLine("			MaxSns = " + instanceName + "_ScanSlotTableLeft[l].SnsCnt;");
                        writer.WriteLine("			/* Disable Sensors Right */");
                        writer.WriteLine("			for(k=0; k < MaxSns; k++)");
                        writer.WriteLine("			{");
                        writer.WriteLine("				s = " + instanceName + "_IndexTableLeft[StartIndex+k];");
                        writer.WriteLine("				" + instanceName + "_" + MethodL + "_DisableSensorLeft(s);");
                        writer.WriteLine("			}");
                        writer.WriteLine("	");
                        writer.WriteLine("			/* Enable Vref on AMUX */");
                        writer.WriteLine("			" + instanceName + "_CAPS_CFG0_LEFT |= " + instanceName + "_CSBUF_CONNECT;");
                        writer.WriteLine("			");
                        writer.WriteLine("			/* Scan  enother scanslot */");
                        writer.WriteLine("			l++;");
                        writer.WriteLine("		}");
                        writer.WriteLine("		else");
                        writer.WriteLine("		{");
                        writer.WriteLine("			if (r < " + instanceName + "_TOTAL_SCANSLOT_COUNT_RIGHT)");
                        writer.WriteLine("			{");
                        writer.WriteLine("				/* Stop Capsensing */");
                        writer.WriteLine("				" + instanceName + "_CONTROL_RIGHT &= ~" + instanceName + "_START_CAPSENSING;");
                        writer.WriteLine("			");
                        writer.WriteLine("				/* Read SlotResult from RwaCnt  for Right side */");
                        writer.WriteLine("				RawIndex = " + instanceName + "_ScanSlotTableRight[r].RawIndex;");
                        writer.WriteLine("			");
                        writer.WriteLine("				/* Read SlotResult from RwaCnt for Right side */");
                        writer.WriteLine("				" + instanceName + "_SlotResult[RawIndex] = " + instanceName + "_rb" + MethodR + "_" + csRawCnt + "_ReadCounter();");
                        writer.WriteLine("	    ");
                        writer.WriteLine("				StartIndex = " + instanceName + "_ScanSlotTableRight[r].IndexOffset;");
                        writer.WriteLine("				MaxSns = " + instanceName + "_ScanSlotTableRight[r].SnsCnt;");
                        writer.WriteLine("				/* Disable Sensors Right */");
                        writer.WriteLine("				for(k=0; k < MaxSns; k++)");
                        writer.WriteLine("				{");
                        writer.WriteLine("					s = " + instanceName + "_IndexTableRight[StartIndex+k];");
                        writer.WriteLine("					" + instanceName + "_" + MethodR + "_DisableSensorRight(s);");
                        writer.WriteLine("				}");
                        writer.WriteLine("	");
                        writer.WriteLine("				/* Enable Vref on AMUX */");
                        writer.WriteLine("				" + instanceName + "_CAPS_CFG0_RIGHT |= " + instanceName + "_CSBUF_CONNECT;");
                        writer.WriteLine("				");
                        writer.WriteLine("				/* Scan  enother scanslot */");
                        writer.WriteLine("				r++;");
                        writer.WriteLine("			}");
                        writer.WriteLine("		}");
                        writer.WriteLine("");
                        writer.WriteLine("	}");
                        writer.WriteLine("");
                        writer.WriteLine("}");
                        #endregion
                    }
                    else
                    {
                        #region ScanAllSlots Synchron
                        writer.WriteLine("/*-----------------------------------------------------------------------------");
                        writer.WriteLine(" * FUNCTION NAME: void " + instanceName + "_ScanAllSlots(void)");
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
                        writer.WriteLine("void " + instanceName + "_ScanAllSlots(void)");
                        writer.WriteLine("{");
                        writer.WriteLine("	uint8 l = 0;");
                        writer.WriteLine("	uint8 r = 0;");
                        writer.WriteLine("	uint8 k, s;");
                        writer.WriteLine("	uint8 MaxSns = " + instanceName + "_ScanSlotTableLeft[l].SnsCnt;");
                        writer.WriteLine("	uint8 StartIndex = " + instanceName + "_ScanSlotTableLeft[l].IndexOffset;");
                        writer.WriteLine("	uint8 RawIndex;");
                        writer.WriteLine("");
                        writer.WriteLine("	while ((l < " + instanceName + "_TOTAL_SCANSLOT_COUNT_LEFT) || (r < " + instanceName + "_TOTAL_SCANSLOT_COUNT_RIGHT))");
                        writer.WriteLine("	{");
                        writer.WriteLine("");
                        writer.WriteLine("		if (l < " + instanceName + "_TOTAL_SCANSLOT_COUNT_LEFT)	/* Scan Left half */");
                        writer.WriteLine("		{");
                        writer.WriteLine("			/* Set Slot Settings  for Left  */");
                        writer.WriteLine("			CapSense_1_"+MethodL+"_SetSlotSettingsLeft(l);");
                        writer.WriteLine("		");
                        writer.WriteLine("			/* Reset Raw Counter and PWM Left */");
                        //if (false)
                        //{
                        //    writer.WriteLine("			k = CapSense_1_lb" + MethodL + "_cRAW_ReadControlRegister();");
                        //    writer.WriteLine("			k |= CapSense_1_lb" + MethodL + "_cRAW_CTRL_RESET;");
                        //    writer.WriteLine("			CapSense_1_lb" + MethodL + "_cRAW_WriteControlRegister(k);");
                        //}
                        //else
                        {
                            writer.WriteLine("				" + instanceName + "_CONTROL_LEFT |= " + instanceName + "_RESET_PWM_CNTR;");
                            writer.WriteLine("				" + instanceName + "_CONTROL_LEFT &= ~" + instanceName + "_RESET_PWM_CNTR;");
                        }
                        writer.WriteLine("	");
                        writer.WriteLine("			/* Disable csBuf Left */");
                        writer.WriteLine("			CapSense_1_CAPS_CFG0_LEFT &= ~CapSense_1_CSBUF_CONNECT;");
                        writer.WriteLine("	");
                        writer.WriteLine("			MaxSns = CapSense_1_ScanSlotTableLeft[l].SnsCnt;");
                        writer.WriteLine("			StartIndex = CapSense_1_ScanSlotTableLeft[l].IndexOffset;");
                        writer.WriteLine("			/* Enable Sensors Left */");
                        writer.WriteLine("			for(k=0; k < MaxSns; k++)");
                        writer.WriteLine("			{");
                        writer.WriteLine("				s = CapSense_1_IndexTableLeft[StartIndex+k];");
                        writer.WriteLine("				CapSense_1_" + MethodL + "_EnableSensorLeft(s);");
                        writer.WriteLine("			}	");
                        writer.WriteLine("	");
                        writer.WriteLine("			/* Set Flag that CapSensing in progres */");
                        writer.WriteLine("			CapSense_1_StatusLeft |= CapSense_1_START_CAPSENSING;");
                        writer.WriteLine("	");
                        writer.WriteLine("			/* Start PWM One Shout and PRS at one time */");
                        writer.WriteLine("			CapSense_1_CONTROL_LEFT |= CapSense_1_START_CAPSENSING;			");
                        writer.WriteLine("		}");
                        writer.WriteLine("		");
                        writer.WriteLine("		if (r < " + instanceName + "_TOTAL_SCANSLOT_COUNT_RIGHT)	/* Scan Right half */");
                        writer.WriteLine("		{");
                        writer.WriteLine("			/* Set Slot Settings  for Right  */");
                        writer.WriteLine("			CapSense_1_" + MethodR + "_SetSlotSettingsRight(r);");
                        writer.WriteLine("	");
                        writer.WriteLine("			/* Reset Raw Counter Right */");
                        //if (false)
                        //{
                        //writer.WriteLine("			k = CapSense_1_rb"+MethodR+"_cRAW_ReadControlRegister();");
                        //writer.WriteLine("			k |= CapSense_1_rb" + MethodR + "_cRAW_CTRL_RESET;");
                        //writer.WriteLine("			CapSense_1_rb" + MethodR + "_cRAW_WriteControlRegister(k);");
                        //}
                        //else
						{
						writer.WriteLine("			" + instanceName + "_CONTROL_RIGHT |= " + instanceName + "_RESET_PWM_CNTR;");
						writer.WriteLine("			" + instanceName + "_CONTROL_RIGHT &= ~" + instanceName + "_RESET_PWM_CNTR;");
						}
                        writer.WriteLine("	");
                        writer.WriteLine("			/* Disable csBuf Right */");
                        writer.WriteLine("			CapSense_1_CAPS_CFG0_RIGHT &= ~CapSense_1_CSBUF_CONNECT;");
                        writer.WriteLine("	");
                        writer.WriteLine("			MaxSns = CapSense_1_ScanSlotTableRight[r].SnsCnt;");
                        writer.WriteLine("			StartIndex = CapSense_1_ScanSlotTableRight[r].IndexOffset;");
                        writer.WriteLine("			/* Enable Sensors Right */");
                        writer.WriteLine("			for(k=0; k < MaxSns; k++)");
                        writer.WriteLine("			{");
                        writer.WriteLine("				s = CapSense_1_IndexTableRight[StartIndex+k];");
                        writer.WriteLine("				CapSense_1_" + MethodR + "_EnableSensorRight(s);");
                        writer.WriteLine("			}	");
                        writer.WriteLine("	");
                        writer.WriteLine("			/* Set Flag that CapSensing in progres */");
                        writer.WriteLine("			CapSense_1_StatusRight |= CapSense_1_START_CAPSENSING;");
                        writer.WriteLine("	");
                        writer.WriteLine("			/* Start PWM One Shout and PRS at one time */");
                        writer.WriteLine("			CapSense_1_CONTROL_RIGHT |= CapSense_1_START_CAPSENSING;");
                        writer.WriteLine("		}");
                        writer.WriteLine("");
                        writer.WriteLine("	");
                        writer.WriteLine("		/* Wait finish of CapSensing */");
                        writer.WriteLine("		while (1)");
                        writer.WriteLine("		{");
                        writer.WriteLine("			if ( (" + instanceName + "_StatusLeft != " + instanceName + "_START_CAPSENSING) &&");
                        writer.WriteLine("				 (" + instanceName + "_StatusRight != " + instanceName + "_START_CAPSENSING) )");
                        writer.WriteLine("			{");
                        writer.WriteLine("				break;				");
                        writer.WriteLine("			}");
                        writer.WriteLine("		}");
                        writer.WriteLine("		");
                        writer.WriteLine("		if (l < " + instanceName + "_TOTAL_SCANSLOT_COUNT_LEFT)");
                        writer.WriteLine("		{");
                        writer.WriteLine("			/* Stop Capsensing  Left */");
                        writer.WriteLine("			" + instanceName + "_CONTROL_LEFT &= ~" + instanceName + "_START_CAPSENSING;");
                        writer.WriteLine("		");
                        writer.WriteLine("			/* Read SlotResult from RwaCnt  for Left side */");
                        writer.WriteLine("			RawIndex = " + instanceName + "_ScanSlotTableLeft[l].RawIndex;");
                        writer.WriteLine("			");
                        writer.WriteLine("			/* Read SlotResult from RwaCnt for Left side */");
                        writer.WriteLine("			" + instanceName + "_SlotResult[RawIndex] = " + instanceName + "_lb" + MethodR + "_cRAW_ReadCounter();");
                        writer.WriteLine("		");
                        writer.WriteLine("			StartIndex = " + instanceName + "_ScanSlotTableLeft[l].IndexOffset;");
                        writer.WriteLine("			MaxSns = " + instanceName + "_ScanSlotTableLeft[l].SnsCnt;");
                        writer.WriteLine("			/* Disable Sensors Left */");
                        writer.WriteLine("			for(k=0; k < MaxSns; k++)");
                        writer.WriteLine("			{");
                        writer.WriteLine("				s = " + instanceName + "_IndexTableLeft[StartIndex+k];");
                        writer.WriteLine("				" + instanceName + "_" + MethodR + "_DisableSensorLeft(s);");
                        writer.WriteLine("			}");
                        writer.WriteLine("	");
                        writer.WriteLine("			/* Enable Vref on AMUX */");
                        writer.WriteLine("			" + instanceName + "_CAPS_CFG0_LEFT |= " + instanceName + "_CSBUF_CONNECT;");
                        writer.WriteLine("			");
                        writer.WriteLine("			/* Scan  enother scanslot */");
                        writer.WriteLine("			l++;");
                        writer.WriteLine("		}");
                        writer.WriteLine("		");
                        writer.WriteLine("		if (r < " + instanceName + "_TOTAL_SCANSLOT_COUNT_RIGHT)");
                        writer.WriteLine("		{");
                        writer.WriteLine("			/* Stop Capsensing  Right */");
                        writer.WriteLine("			" + instanceName + "_CONTROL_RIGHT &= ~" + instanceName + "_START_CAPSENSING;");
                        writer.WriteLine("			");
                        writer.WriteLine("			/* Read SlotResult from RwaCnt  for Right side */");
                        writer.WriteLine("			RawIndex = " + instanceName + "_ScanSlotTableRight[r].RawIndex;");
                        writer.WriteLine("		");
                        writer.WriteLine("			/* Read SlotResult from RwaCnt for Right side */");
                        writer.WriteLine("			" + instanceName + "_SlotResult[RawIndex] = " + instanceName + "_rb" + MethodR + "_cRAW_ReadCounter();");
                        writer.WriteLine("	");
                        writer.WriteLine("			StartIndex = " + instanceName + "_ScanSlotTableRight[r].IndexOffset;");
                        writer.WriteLine("			MaxSns = " + instanceName + "_ScanSlotTableRight[r].SnsCnt;");
                        writer.WriteLine("			/* Disable Sensors Right */");
                        writer.WriteLine("			for(k=0; k < MaxSns; k++)");
                        writer.WriteLine("			{");
                        writer.WriteLine("				s = " + instanceName + "_IndexTableRight[StartIndex+k];");
                        writer.WriteLine("				" + instanceName + "_" + MethodR + "_DisableSensorRight(s);");
                        writer.WriteLine("			}");
                        writer.WriteLine("	");
                        writer.WriteLine("			/* Enable Vref on AMUX */");
                        writer.WriteLine("			" + instanceName + "_CAPS_CFG0_RIGHT |= " + instanceName + "_CSBUF_CONNECT;");
                        writer.WriteLine("			");
                        writer.WriteLine("			/* Scan  enother scanslot */");
                        writer.WriteLine("			r++;");
                        writer.WriteLine("		}");
                        writer.WriteLine("	}");
                        writer.WriteLine("");
                        writer.WriteLine("}");
                        #endregion
                    }
                }
                else
                {
                    //Not Paralle Full and Serial
                    foreach (CyAmuxBParams item in packParams.localParams.listCsHalfs)
                        if (packParams.localParams.bCsHalfIsEnable(item))
                        {
                            #region ScanAllSlots
                            writer.WriteLine("/*-----------------------------------------------------------------------------");
                            writer.WriteLine(" * FUNCTION NAME: void " + instanceName + "_ScanAllSlots(void)");
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
                            writer.WriteLine("void " + instanceName + "_ScanAllSlots" + "(void)");
                            writer.WriteLine("{");

                            string strSideName = GetSideName(item.side, packParams.Configuration);
                            string Method = item.Method.ToString();
                            writer.WriteLine(instanceName + "_" + Method + "_ScanAllSlots" + strSideName + "();");

                            writer.WriteLine("}");
                            writer.WriteLine("");
                             #endregion
                        }

                }
            }
        }
        #endregion

        #region apiCollectCFunctionForSide
        public void apiCollectCFunctionForSide(ref TextWriter writer, CyGeneralParams packParams, CyAmuxBParams sbParametr)
        {

            eElSide side = sbParametr.side;
            string strSideName = GetSideName(side, packParams.Configuration);
            string strSideNameUpper = GetSideNameUpper(side, packParams.Configuration);

            string Method = sbParametr.Method.ToString();
            string Symbol = SymbolGenerate(packParams, sbParametr, packParams.Configuration);
            string Idac_Polarity = Idac_PolarityGenerate(sbParametr);
            //==========================
            string SW_CLK = GetSW_CLK(packParams, sbParametr); ;
            string DIG_CLK = packParams.GetPrefixForSchematic(sbParametr) + baseDIG_CLK;
            string csPRS = "cPRS" + sbParametr.GetPRSResolution();
            string CLK = Get_CLK(packParams, sbParametr);
            
            #region Idac
            if (sbParametr.isIdac)
            {
                #region Idac_Start

                writer.WriteLine("/*******************************************************************************");
                writer.WriteLine("* Function Name: void " + instanceName + "_" + Symbol + "_" + csIdac + "_Start" + strSideName + "(void) ");
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
                writer.WriteLine("void " + instanceName + "_" + Symbol + "_" + csIdac + "_Start" + strSideName + "(void) ");
                writer.WriteLine("{");
                writer.WriteLine("	/* Hardware initiazation only needs to occure the first time */");
                writer.WriteLine("	if ( " + instanceName + "_IDAC_initVar" + strSideName + " == 0)  ");
                writer.WriteLine("  	{     ");
                writer.WriteLine("    	" + instanceName + "_IDAC_initVar" + strSideName + " = 1;");
                writer.WriteLine("      	" + instanceName + "_IDAC_CR0" + strSideNameUpper + " = " + instanceName + "_IDAC_MODE_I;");
                writer.WriteLine("       ");
                writer.WriteLine("      	" + instanceName + "_IDAC_CR1" + strSideNameUpper + " = " + Idac_Polarity + " | " + instanceName + "_IDAC_IDIR_CTL_UDB;");
                writer.WriteLine("   	}");
                writer.WriteLine("");
                writer.WriteLine("   	/* Enable power to DAC */");
                writer.WriteLine("   	" + instanceName + "_IDAC_PWRMGR" + strSideNameUpper + " |= " + instanceName + "_IDAC_ACT_PWR_EN" + strSideNameUpper+ ";");
                writer.WriteLine("}");
                writer.WriteLine("");

                #endregion

                #region Idac_Stop
                writer.WriteLine("/*******************************************************************************");
                writer.WriteLine("* Function Name: void " + instanceName + "_" + Symbol + "_" + csIdac + "_Stop" + strSideName + "(void) ");
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
                writer.WriteLine("void " + instanceName + "_" + Symbol + "_" + csIdac + "_Stop" + strSideName + "(void) ");
                writer.WriteLine("{");
                writer.WriteLine("   /* Disble power to DAC */");
                writer.WriteLine("   	" + instanceName + "_IDAC_PWRMGR" + strSideNameUpper + " &= ~" + instanceName + "_IDAC_ACT_PWR_EN" + strSideNameUpper + ";");
                writer.WriteLine("}");
                #endregion

                #region Idac_SetRange
                writer.WriteLine("/*******************************************************************************");
                writer.WriteLine("* Function Name: void " + instanceName + "_" + Symbol + "_" + csIdac + "_SetRange" + strSideName + "(uint8 range)");
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
                writer.WriteLine("void " + instanceName + "_" + Symbol + "_" + csIdac + "_SetRange" + strSideName + "(uint8 range)");
                writer.WriteLine("{");
                writer.WriteLine("   " + instanceName + "_IDAC_CR0"+ strSideNameUpper + " &= ~" + instanceName + "_IDAC_RANGE_MASK;       		/* Clear existing mode */");
                writer.WriteLine("   " + instanceName + "_IDAC_CR0"+ strSideNameUpper + " |= ( range & " + instanceName + "_IDAC_RANGE_MASK );  		/*  Set Range  */");
                writer.WriteLine("}");
                writer.WriteLine("");
                #endregion

                #region Idac_SetValue
                writer.WriteLine("/*******************************************************************************");
                writer.WriteLine("* Function Name: void " + instanceName + "_" + Symbol + "_" + csIdac + "_SetValue" + strSideName + "(uint8 value)");
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
                writer.WriteLine("void " + instanceName + "_" + Symbol + "_" + csIdac + "_SetValue" + strSideName + "(uint8 value)");
                writer.WriteLine("{");
                writer.WriteLine("    " + instanceName + "_IDAC_DATA" + strSideNameUpper + " = value;         /*  Set Value  */");
                writer.WriteLine("   ");
                writer.WriteLine("   #if defined(" + instanceName + "_IDAC_FIRST_SILICON" + strSideNameUpper + ")");
                writer.WriteLine("    " + instanceName + "_IDAC_DATA" + strSideNameUpper + " = value;        /*  TODO: Need for first version of silicon */");
                writer.WriteLine("   #endif");
                writer.WriteLine("");
                writer.WriteLine("}");

                #endregion
            }
            #endregion
            
            #region SetRb
            if (sbParametr.isRbEnable)
            {
                writer.WriteLine("/*-----------------------------------------------------------------------------");
                writer.WriteLine(" * FUNCTION NAME: void " + instanceName + "_" + Method + "_SetRb" + strSideName + "(uint8 rbeed)");
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
                writer.WriteLine("void " + instanceName + "_" + Method + "_SetRb" + strSideName + "(uint8 rbleed)");
                writer.WriteLine("{");
                writer.WriteLine("	uint8 i;");
                writer.WriteLine("	reg8 *portreg;");
                writer.WriteLine("    	uint8  rbcount = sizeof(" + instanceName + "_RbPortPinTable" + strSideName + ")/sizeof(reg8 *);");
                writer.WriteLine("    ");
                writer.WriteLine("	if (" + instanceName + "_CurrentRbleed" + strSideName +" == " + instanceName + "_MAX_RB_NUMBER)");
                writer.WriteLine("    {    ");
                writer.WriteLine("        /* Disable all Rb*/");
                writer.WriteLine("        for(i=0; i<rbcount; i++)");
                writer.WriteLine("        {	");
                writer.WriteLine("            portreg = " + instanceName + "_RbPortPinTable" + strSideName + "[i];");
                writer.WriteLine("            ");
                writer.WriteLine("            /* Make High-Z */");
                writer.WriteLine("            CY_SET_REG8(portreg, (CY_GET_REG8(portreg) | " + instanceName + "_PRT_SET_TO_HIGH));");
                writer.WriteLine("                       ");
                writer.WriteLine("            /* Disconnect from DSI output */");
                writer.WriteLine("            CY_SET_REG8(portreg, (CY_GET_REG8(portreg) & (~" + instanceName + "_PRT_BYP_ENABLE)));");
                writer.WriteLine("            ");
                writer.WriteLine("        }       ");
                writer.WriteLine("        portreg = " + instanceName + "_RbPortPinTable" + strSideName + "[rbleed];");
                writer.WriteLine("        ");
                writer.WriteLine("        CY_SET_REG8(portreg, (CY_GET_REG8(portreg) | " + instanceName + "_PRT_BYP_ENABLE));");
                writer.WriteLine("        ");
                writer.WriteLine("        " + instanceName + "_CurrentRbleed" + strSideName + " = rbleed;");
                writer.WriteLine("    }");
                writer.WriteLine("	");
                writer.WriteLine("    if (" + instanceName + "_CurrentRbleed" + strSideName + " != rbleed)");
                writer.WriteLine("    {");
                writer.WriteLine("        portreg = " + instanceName + "_RbPortPinTable" + strSideName + "[" + instanceName + "_CurrentRbleed" + strSideName + "];");
                writer.WriteLine("	");
                writer.WriteLine("        /* Disconnect to DSI output */");
                writer.WriteLine("        CY_SET_REG8(portreg, (CY_GET_REG8(portreg) & (~" + instanceName + "_PRT_BYP_ENABLE))); ");
                writer.WriteLine("        ");
                writer.WriteLine("        portreg = " + instanceName + "_RbPortPinTable" + strSideName + "[rbleed];");
                writer.WriteLine("           ");
                writer.WriteLine("        /* Connect to DSI output */");
                writer.WriteLine("        CY_SET_REG8(portreg, (CY_GET_REG8(portreg) | " + instanceName + "_PRT_BYP_ENABLE));");
                writer.WriteLine("        ");
                writer.WriteLine("        " + instanceName + "_CurrentRbleed" + strSideName + " = rbleed;");
                writer.WriteLine("    }");
                writer.WriteLine("}");
                writer.WriteLine("");
            }
            #endregion

            #region Shield_Init
            if (sbParametr.IsShieldElectrode())
            {
                writer.WriteLine("/*******************************************************************************");
                writer.WriteLine("* Function Name: void " + instanceName + "_" + Method + "_Shield_Init" + strSideName + "(void)");
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
                writer.WriteLine("void " + instanceName + "_" + Method + "_Shield_Init" + strSideName + "(void)");
                writer.WriteLine("{");
                writer.WriteLine("    uint8 port, mask, i;");
                writer.WriteLine("	");
                writer.WriteLine("	for(i=0; i<" + instanceName + "_TOTAL_SHIELD_COUNT"+ strSideNameUpper + "; i++)");
                writer.WriteLine("    {	       ");
                writer.WriteLine("        port = " + instanceName + "_ShieldPortMaskTable" + strSideName + "[i].port;");
                writer.WriteLine("        mask = " + instanceName + "_ShieldPortMaskTable" + strSideName + "[i].mask;");
                writer.WriteLine("        ");
                writer.WriteLine("        /* Disable dig_glbl_ctl to sensor */");
                writer.WriteLine("        *(" + instanceName + "_BASE_PRTDSI_CAPS + (" + instanceName + "_PRTDSI_OFFSET*port)) &= ~mask;");
                writer.WriteLine("    	");
                writer.WriteLine("        /* State of pins == SHIELD,  Set drive mode Strong */    ");
                writer.WriteLine("        *(" + instanceName + "_BASE_PRT_DM2 + (" + instanceName + "_PRT_OFFSET*port)) |= mask;");
                writer.WriteLine("        *(" + instanceName + "_BASE_PRT_DM1 + (" + instanceName + "_PRT_OFFSET*port)) |= mask;");
                writer.WriteLine("        *(" + instanceName + "_BASE_PRT_DM0 + (" + instanceName + "_PRT_OFFSET*port)) &= ~mask;");
                writer.WriteLine("		");
                writer.WriteLine("        /* Connect to DSI output */");
                writer.WriteLine("        *(" + instanceName + "_BASE_PRT_BYP + (" + instanceName + "_PRT_OFFSET*port)) |= mask;");
                writer.WriteLine("    }");
                writer.WriteLine("}");
            }
            #endregion

            #region Start
            writer.WriteLine("/*-----------------------------------------------------------------------------");
            writer.WriteLine(" * FUNCTION NAME: void " + instanceName + "_" + Method + "_Start" + strSideName + "(void)");
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
            writer.WriteLine("void " + instanceName + "_" + Method + "_Start" + strSideName + "(void)");
            writer.WriteLine("{");
            writer.WriteLine("	/* High Level API Settings */");
            writer.WriteLine("	");//(packParams.localParams.IsShowPrescalerCode(sbParametr)
            if (sbParametr.IsPrescaler())
           {
               writer.WriteLine("	/* Setup SW_CLK_Div_Serial */");
               writer.WriteLine("	" + instanceName + "_" + SW_CLK + "_Start();");
               writer.WriteLine("	");
			}
          	writer.WriteLine("	/* Setup DIG_CLK_DivSerial */");
          	writer.WriteLine("	" + instanceName + "_" + DIG_CLK + "_Start();");
          	writer.WriteLine("	");
            writer.WriteLine("	/* Setup " + Symbol + "_csRawCnt */");
            writer.WriteLine("	" + instanceName + "_" + Symbol + "_" + csRawCnt + "_Start();");
            writer.WriteLine("		");
            writer.WriteLine("	/* Setup " + Symbol + "_csPWM */");
            writer.WriteLine("	" + instanceName + "_" + Symbol + "_" + csPWM + "_Start();");
            writer.WriteLine("	");
            if (sbParametr.IsWorkAround())
            {
                //if (false)
                //{
                //    writer.WriteLine("	/* Setup " + Symbol + "_csPWMwa */");
                //    writer.WriteLine("	//" + instanceName + "_" + Symbol + "_" + csWorkAround + "_Start();");
                //}
                //else
            }
            writer.WriteLine("	/* Enable Comparator */");
            writer.WriteLine("	" + instanceName + "_" + Symbol + "_" + csComp + "_Start();");
            writer.WriteLine("	");
            writer.WriteLine("	/* Initialize AMux */");
            writer.WriteLine("	" + instanceName + "_" + Symbol + "_" + csAmux + "_Start();");
            writer.WriteLine("	");
            if (sbParametr.isIdac)
            {
                writer.WriteLine("	/* Start Idac */");
                writer.WriteLine("	" + instanceName + "_" + Symbol + "_" + csIdac + "_Start" + strSideName + "();");
                writer.WriteLine("	");
            }
            writer.WriteLine("	/* Connect Cmod, Idac, Vref */");
            writer.WriteLine("	" + instanceName + "_" + Symbol + "_" + csAmux + "_Connect(" + instanceName + "_CMOD_CHANEL"+ strSideNameUpper + ");");
            writer.WriteLine("	" + instanceName + "_" + Symbol + "_" + csAmux + "_Connect(" + instanceName + "_VREF_CHANEL"+ strSideNameUpper + ");");
			writer.WriteLine("    /* Enable csBuf */");
			writer.WriteLine("	");
            writer.WriteLine("	" + instanceName + "_CSBUF_PWRMGR"+ strSideNameUpper + " |= " + instanceName + "_CSBUF_ENABLE"+ strSideNameUpper + ";");
            writer.WriteLine("	" + instanceName + "_CAPS_CFG0"+ strSideNameUpper + " |= " + instanceName + "_CSBUF_CONNECT;");
            writer.WriteLine("	");
            if (sbParametr.isIdac)
            {
                writer.WriteLine("    /* Connect IDAC */");
                writer.WriteLine("	" + instanceName + "_" + Symbol + "_" + csAmux + "_Connect(" + instanceName + "_IDAC_CHANEL" + strSideNameUpper + ");");
                writer.WriteLine("");
            }
            if (sbParametr.isRbEnable)
            {
                writer.WriteLine("    /* Enable Rbleed */");
                writer.WriteLine("	" + instanceName + "_" + Method + "_SetRb" + strSideName + "(" + instanceName + "_RBLEED1);");
                writer.WriteLine("		");
            }

            if (sbParametr.IsShieldElectrode())
            {
                writer.WriteLine("    /* Set N Shield electrodes */");
                writer.WriteLine("    " + instanceName + "_" + Method + "_Shield_Init" + strSideName + "();");
                writer.WriteLine("    ");
            }
            writer.WriteLine("    /* Clear all Sensor */");
            writer.WriteLine("	" + instanceName + "_" + Method + "_ClearSlots" + strSideName + "();");
            writer.WriteLine("		");
            writer.WriteLine("    /* Setup ISR Component for CapSense */");
            writer.WriteLine("    /* Disable the  CapSense interrupt */");
            writer.WriteLine("    " + instanceName + "_" + Symbol + "_" + csISR + "_Disable();");
            writer.WriteLine("	");
            writer.WriteLine("	  /* Set vector of " + instanceName + "_isr */ ");
            writer.WriteLine("    " + instanceName + "_" + Symbol + "_" + csISR + "_SetVector(" + instanceName + "_ISR" + strSideName + ");");
            writer.WriteLine("	");
            writer.WriteLine("	  /* Set pririty of " + instanceName + "_isr */");
            writer.WriteLine("    " + instanceName + "_" + Symbol + "_" + csISR + "_SetPriority(" + instanceName + "_INTC_PRIORITY" + strSideNameUpper + ");");
            writer.WriteLine("	    ");
            writer.WriteLine("    /* Enable the " + instanceName + " interrupt */");
            writer.WriteLine("    " + instanceName + "_" + Symbol + "_" + csISR + "_Enable();");
            writer.WriteLine("	    ");
            writer.WriteLine("    /* Enable the " + instanceName + " Clock */");
            writer.WriteLine("    " + instanceName + "_" + CLK + "_Enable();");
            writer.WriteLine("}");
            writer.WriteLine("");
            #endregion

            #region Stop
            writer.WriteLine("/*-----------------------------------------------------------------------------");
            writer.WriteLine(" * FUNCTION NAME: void " + instanceName + "_" + Method + "_Stop" + strSideName + "(void)");
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
            writer.WriteLine("void " + instanceName + "_" + Method + "_Stop" + strSideName + "(void)");
            writer.WriteLine("{");
            writer.WriteLine("	/* Stop Capsensing */");
            writer.WriteLine("	" + instanceName + "_CONTROL"+ strSideNameUpper + " &= ~" + instanceName + "_START_CAPSENSING;");
            writer.WriteLine("	");
            writer.WriteLine("	/* Disable the  CapSense interrupt */");
            writer.WriteLine("    " + instanceName + "_" + Symbol + "_" + csISR + "_Disable();");
            writer.WriteLine("	");
            writer.WriteLine("	/* Clear all Sensor */");
            writer.WriteLine("	" + instanceName + "_" + Method + "_ClearSlots" + strSideName + "();");
            writer.WriteLine("	");
            writer.WriteLine("	/* Disable Comparator */	");
            writer.WriteLine("	" + instanceName + "_" + Symbol + "_" + csComp + "_Stop();");
            writer.WriteLine("	");
            writer.WriteLine("	/* Disable csBuf */");
            writer.WriteLine("	" + instanceName + "_CAPS_CFG0"+ strSideNameUpper + " &= ~" + instanceName + "_CSBUF_CONNECT;");
            writer.WriteLine("	" + instanceName + "_CSBUF_PWRMGR"+ strSideNameUpper + " &= ~" + instanceName + "_CSBUF_ENABLE"+ strSideNameUpper + ";");
            writer.WriteLine("	");
            writer.WriteLine("	");
            if (sbParametr.isIdac)
            {
                writer.WriteLine("	/* Disable power to IDAC */");
                writer.WriteLine("   " + instanceName + "_IDAC_PWRMGR" + strSideNameUpper + "&= ~" + instanceName + "_IDAC_ACT_PWR_EN" + strSideNameUpper + ";");
                writer.WriteLine("	");
            }
            writer.WriteLine("}");
            writer.WriteLine("");
            #endregion

            #region ScanSlot
            writer.WriteLine("/*-----------------------------------------------------------------------------");
            writer.WriteLine(" * FUNCTION NAME: void " + instanceName + "_" + Method + "_ScanSlot" + strSideName + "(uint8 slot)");
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
            writer.WriteLine("void " + instanceName + "_" + Method + "_ScanSlot" + strSideName + "(uint8 slot)");
            writer.WriteLine("{");
            writer.WriteLine("	uint8 i, s;");
            writer.WriteLine("	uint8 MaxSns = " + instanceName + "_ScanSlotTable" + strSideName + "[slot].SnsCnt;");
            writer.WriteLine("	uint8 StartIndex = " + instanceName + "_ScanSlotTable" + strSideName + "[slot].IndexOffset;");
            writer.WriteLine("	uint8 RawIndex = " + instanceName + "_ScanSlotTable" + strSideName + "[slot].RawIndex; ");
            writer.WriteLine("");
			writer.WriteLine("  /* Work around for RAW counter reset */");
			writer.WriteLine("  " + instanceName + "_" + Symbol + "_" + csRawCnt + "_WriteCounter(0x0000);");
			writer.WriteLine("");
            writer.WriteLine("	/* Set Slot Settings */");
            writer.WriteLine("	" + instanceName + "_" + Method + "_SetSlotSettings" + strSideName + "(slot);");
            //if (false)
            //{
            //writer.WriteLine("	i = " + instanceName + "_" + Symbol + "_" + csRawCnt + "_ReadControlRegister();");
            //writer.WriteLine("	i |= " + instanceName + "_" + Symbol + "_" + csRawCnt + "_CTRL_RESET;");
            //writer.WriteLine("	" + instanceName + "_" + Symbol + "_" + csRawCnt + "_WriteControlRegister(i);");
            //}
            //else
            writer.WriteLine("	");
            writer.WriteLine("	/* Disable csBuf */");
            writer.WriteLine("	" + instanceName + "_CAPS_CFG0"+ strSideNameUpper + " &= ~" + instanceName + "_CSBUF_CONNECT;	");
            writer.WriteLine("			");
            writer.WriteLine("	/* Enable Sensors */");
            writer.WriteLine("	for(i=0; i < MaxSns; i++)");
            writer.WriteLine("	{");
            writer.WriteLine("		s = " + instanceName + "_IndexTable" + strSideName + "[StartIndex+i];");
            writer.WriteLine("		" + instanceName + "_" + Method + "_EnableSensor" + strSideName + "(s);");
            writer.WriteLine("	}");
            writer.WriteLine("	");
            writer.WriteLine("	/* Set Flag that CapSensing in progres */");
            writer.WriteLine("	" + instanceName + "_Status" + strSideName + " |= " + instanceName + "_START_CAPSENSING;");
            writer.WriteLine("	");
            writer.WriteLine("	/* Start PWM One Shout and PRS at one time */");
            writer.WriteLine("	" + instanceName + "_CONTROL"+ strSideNameUpper + " |= " + instanceName + "_START_CAPSENSING;");
            writer.WriteLine("			");
            writer.WriteLine("	/* Wait finish of CapSensing */");
            writer.WriteLine("	while (" + instanceName + "_Status" + strSideName + " == " + instanceName + "_START_CAPSENSING)");
            writer.WriteLine("	{ ; }");
            writer.WriteLine("	");
            writer.WriteLine("	/* Stop Capsensing */");
            writer.WriteLine("	" + instanceName + "_CONTROL"+ strSideNameUpper + " &= ~" + instanceName + "_START_CAPSENSING;");
            writer.WriteLine("	");
            writer.WriteLine("	/* Read SlotResult from RawCnt */");
            writer.WriteLine("	" + instanceName + "_SlotResult[RawIndex] = " + instanceName + "_" + Symbol + "_" + csRawCnt + "_ReadCounter();");
            writer.WriteLine("	");
			writer.WriteLine("	/* Reset RawCnt and rearm PWM */");
			writer.WriteLine("	" + instanceName + "_CONTROL" + strSideNameUpper + " |= " + instanceName + "_RESET_PWM_CNTR;");
            writer.WriteLine("	");
            writer.WriteLine("	for(i=0; i < MaxSns; i++)");
            writer.WriteLine("	{");
            writer.WriteLine("		s = " + instanceName + "_IndexTable" + strSideName + "[StartIndex+i];");
            writer.WriteLine("		" + instanceName + "_" + Method + "_DisableSensor" + strSideName + "(s);");
            writer.WriteLine("	}");
            writer.WriteLine("	");
			writer.WriteLine("	" + instanceName + "_CONTROL" + strSideNameUpper + " &= ~" + instanceName + "_RESET_PWM_CNTR;");
			writer.WriteLine("	");
            writer.WriteLine("	/* Enable Vref on AMUX */");
            writer.WriteLine("    " + instanceName + "_CAPS_CFG0"+ strSideNameUpper + " |= " + instanceName + "_CSBUF_CONNECT;");
            writer.WriteLine("}");
            #endregion

            #region ScanAllSlots
            writer.WriteLine("/*-----------------------------------------------------------------------------");
            writer.WriteLine(" * FUNCTION NAME: void " + instanceName + "_" + Method + "_ScanAllSlots" + strSideName + "(void)");
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
            writer.WriteLine("void " + instanceName + "_" + Method + "_ScanAllSlots" + strSideName + "(void)");
            writer.WriteLine("{");
            writer.WriteLine("	uint8 i;");
            writer.WriteLine("	for(i=0; i < " + instanceName + "_TOTAL_SCANSLOT_COUNT"+ strSideNameUpper + ";i++)");
            writer.WriteLine("	{");
            writer.WriteLine("		" + instanceName + "_" + Method + "_ScanSlot" + strSideName + "(i);");
            writer.WriteLine("	}");
            writer.WriteLine("}");
            writer.WriteLine("");
            #endregion

            #region SetSlotSettings
            writer.WriteLine("/*-----------------------------------------------------------------------------");
            writer.WriteLine(" * FUNCTION NAME: void " + instanceName + "_" + Method + "_SetSlotSettings" + strSideName + "(uint8 slot)");
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
            writer.WriteLine("void " + instanceName + "_" + Method + "_SetSlotSettings" + strSideName + "(uint8 slot)");
            writer.WriteLine("{");
            if (sbParametr.IsPrescaler())
            {
                writer.WriteLine("	uint8 prescalerperiod = " + instanceName + "_SettingsTable" + strSideName + "[slot].PrescalerPeriod;");
            }

            writer.WriteLine("	uint8 scanspeed = " + instanceName + "_SettingsTable" + strSideName + "[slot].ScanSpeed;");
            writer.WriteLine("	uint16 resolution = ( (((uint16) " + instanceName + "_SettingsTable" + strSideName + "[slot].Resolution) << 8) | 0x00FFu );");

            if (sbParametr.IsPRS())
            {
                writer.WriteLine("	uint8 prspolynomial = " + instanceName + "_SettingsTable" + strSideName + "[slot].PrsPolynomial;");
            }

            if (sbParametr.isIdac)
            {
                writer.WriteLine("	uint8 idacrange = " + instanceName + "_SettingsTable" + strSideName + "[slot].IdacRange;");
                writer.WriteLine("	uint8 idacsettings = " + instanceName + "_SettingsTable" + strSideName + "[slot].IdacSettings;");
                writer.WriteLine("	");
            }

            if (sbParametr.IsPrescaler())
            {
                writer.WriteLine("	/* Set Prescaller */");
                writer.WriteLine("	" + instanceName + "_" + SW_CLK + "_Stop();");
                writer.WriteLine("	" + instanceName + "_" + SW_CLK + "_WritePeriod(prescalerperiod);");
				writer.WriteLine("	" + instanceName + "_" + SW_CLK + "_WriteCompare(prescalerperiod/2);");
                writer.WriteLine("	" + instanceName + "_" + SW_CLK + "_Start();");
                writer.WriteLine("	");
            }

            writer.WriteLine("	/* Set ScanSpeed */");
            writer.WriteLine("	" + instanceName + "_" + DIG_CLK + "_Stop();");
            writer.WriteLine("	" + instanceName + "_" + DIG_CLK + "_WritePeriod(scanspeed);");
            writer.WriteLine("	" + instanceName + "_" + DIG_CLK + "_Start();");
            writer.WriteLine("	");

            writer.WriteLine("	/* PWM Resolution */");
			writer.WriteLine("	" + instanceName + "_" + Symbol + "_" + csPWM + "_WritePeriod(resolution);");
            writer.WriteLine("	" + instanceName + "_" + Symbol + "_" + csPWM + "_WriteCompare(resolution);");
			writer.WriteLine("	" + instanceName + "_" + Symbol + "_" + csPWM + "_WriteCounter(resolution);");
            writer.WriteLine("		");
            if (sbParametr.IsPRS())
            {
                writer.WriteLine("	/* PRS Polynomial */");
                writer.WriteLine("	" + instanceName + "_" + Symbol + "_" + csPRS + "_WritePolynomial(prspolynomial);");
                writer.WriteLine("	" + instanceName + "_" + Symbol + "_" + csPRS + "_WriteSeed(0x01u);");
                writer.WriteLine("	");
            }
            if (sbParametr.isIdac)
            {
                writer.WriteLine("	/* Set Idac */");
                writer.WriteLine("	" + instanceName + "_" + Symbol + "_" + csIdac + "_SetRange" + strSideName + "(idacrange);");
                writer.WriteLine("	" + instanceName + "_" + Symbol + "_" + csIdac + "_SetValue" + strSideName + "(idacsettings);");
            }
            writer.WriteLine("}");
            writer.WriteLine("");
            #endregion

            #region ClearSlots
            writer.WriteLine("/*-----------------------------------------------------------------------------");
            writer.WriteLine(" * FUNCTION NAME: void " + instanceName + "_" + Method + "_ClearSlots" + strSideName + "(void)");
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
            writer.WriteLine("void " + instanceName + "_" + Method + "_ClearSlots" + strSideName + "(void)");
            writer.WriteLine("{");
            writer.WriteLine("	uint8 i, j, s;");
            writer.WriteLine("	uint8 StartIndex;");
            writer.WriteLine("	uint8 MaxSns;");
            writer.WriteLine("	for (i=0; i < " + instanceName + "_TOTAL_SCANSLOT_COUNT"+ strSideNameUpper + "; i++)");
            writer.WriteLine("	{");
            writer.WriteLine("		StartIndex = " + instanceName + "_ScanSlotTable" + strSideName + "[i].IndexOffset;");
            writer.WriteLine("		MaxSns = " + instanceName + "_ScanSlotTable" + strSideName + "[i].SnsCnt;");
            writer.WriteLine("	");
            writer.WriteLine("		/* Disable Sensors Right */");
            writer.WriteLine("		for(j=0; j < MaxSns; j++)");
            writer.WriteLine("		{");
            writer.WriteLine("			s = " + instanceName + "_IndexTable" + strSideName + "[StartIndex+j];");
            writer.WriteLine("			" + instanceName + "_" + Method + "_DisableSensor" + strSideName + "(s);");
            writer.WriteLine("		}");
            writer.WriteLine("	}");
            writer.WriteLine("}");
            #endregion

            #region ReadSlot
            writer.WriteLine("/*-----------------------------------------------------------------------------");
            writer.WriteLine(" * FUNCTION NAME: uint16 " + instanceName + "_" + Method + "_ReadSlot" + strSideName + "(uint8 slot)");
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
            writer.WriteLine("uint16 " + instanceName + "_" + Method + "_ReadSlot" + strSideName + "(uint8 slot)");
            writer.WriteLine("{");
            writer.WriteLine("	uint8 index = " + instanceName + "_ScanSlotTable" + strSideName + "[slot].RawIndex;");
            writer.WriteLine("	");
            writer.WriteLine("	return " + instanceName + "_SlotResult[index];");
            writer.WriteLine("}");
            writer.WriteLine("");
            #endregion

            #region EnableSensor
            writer.WriteLine("/*-----------------------------------------------------------------------------");
            writer.WriteLine(" * FUNCTION NAME: void " + instanceName + "_EnableSensor" + strSideName + "(uint8 sensor, uint8 state)");
            writer.WriteLine(" *-----------------------------------------------------------------------------");
            writer.WriteLine(" * Summary: ");
            writer.WriteLine(" *  Configures the selected sensor to measure during the next measurement cycle. ");
            writer.WriteLine(" *  The port and sensor can be selected using the define provided in Customizerfor ");
            writer.WriteLine(" *  each sensor. Drive modes are modified to place the selected port and pin into ");
            writer.WriteLine(" *  Analog High-Z mode and connect to AMUXBUS. ??? This also enables the comparator");
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
            writer.WriteLine("void " + instanceName + "_" + Method + "_EnableSensor" + strSideName + "(uint8 sensor)");
            writer.WriteLine("{");
            writer.WriteLine("	uint8 port = " + instanceName + "_PortMaskTable" + strSideName + "[sensor].port;");
            writer.WriteLine("	uint8 mask = " + instanceName + "_PortMaskTable" + strSideName + "[sensor].mask; ");
            writer.WriteLine("	");
			if(sbParametr.IsIDACSinkingOrRb())
			{
			writer.WriteLine("	/* Set correct drive mode(Open Drain drives High) to switching from High-Z and Pull-Up */");
            writer.WriteLine("	*(" + instanceName + "_BASE_PRT_DM0 + (" + instanceName + "_PRT_OFFSET*port)) |= mask;");
			writer.WriteLine("	*(" + instanceName + "_BASE_PRT_DM1 + (" + instanceName + "_PRT_OFFSET*port)) &= ~mask;");
			writer.WriteLine("	*(" + instanceName + "_BASE_PRT_DM2 + (" + instanceName + "_PRT_OFFSET*port)) |= mask;");
			}
            writer.WriteLine("	/* Connect from DSI output */");
            writer.WriteLine("	*(" + instanceName + "_BASE_PRT_BYP + (" + instanceName + "_PRT_OFFSET*port)) |= mask;");
            writer.WriteLine("	");
            writer.WriteLine("	/* Connect to AMUX */");
            writer.WriteLine("	" + instanceName + "_" + Symbol + "_" + csAmux + "_Connect(sensor);	");
            writer.WriteLine("	    ");
            writer.WriteLine("	" + instanceName + "_" + Symbol + "_CapS_clock_Enable();");	
            writer.WriteLine("}");
            #endregion

            #region DisableSensor
            writer.WriteLine("/*-----------------------------------------------------------------------------");
            writer.WriteLine(" * FUNCTION NAME: void " + instanceName + "_DisableSensor" + strSideName + "(uint8 sensor, uint8 state)");
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
            writer.WriteLine("void " + instanceName + "_" + Method + "_DisableSensor" + strSideName + "(uint8 sensor)");
            writer.WriteLine("{");
            writer.WriteLine("	uint8 port = " + instanceName + "_PortMaskTable" + strSideName + "[sensor].port;");
            writer.WriteLine("	uint8 mask = " + instanceName + "_PortMaskTable" + strSideName + "[sensor].mask; ");
            writer.WriteLine("	");
            writer.WriteLine("	/* Disconnect from AMUX */");
            writer.WriteLine("	" + instanceName + "_" + Symbol + "_" + csAmux + "_Disconnect(sensor);");
            writer.WriteLine("		");
            if (sbParametr.IsIDACSinkingOrRb())
			{
			writer.WriteLine("	/* Set correct drive mode(Open Drain drives Low) to ground sensor */ ");
            writer.WriteLine("	*(" + instanceName + "_BASE_PRT_DM0 + (" + instanceName + "_PRT_OFFSET*port)) &= ~mask;");
			writer.WriteLine("	*(" + instanceName + "_BASE_PRT_DM1 + (" + instanceName + "_PRT_OFFSET*port)) &= ~mask;");
			writer.WriteLine("	*(" + instanceName + "_BASE_PRT_DM2 + (" + instanceName + "_PRT_OFFSET*port)) |= mask;");
			}
            writer.WriteLine("	/* Disconnect to DSI output */");
            writer.WriteLine("	*(" + instanceName + "_BASE_PRT_BYP + (" + instanceName + "_PRT_OFFSET*port)) &= ~mask;");
            writer.WriteLine("	    ");
            writer.WriteLine("	" + instanceName + "_" + Symbol + "_CapS_clock_Disable();");	
            writer.WriteLine("}");
            #endregion

            #region GetPortPin
            //if (false)
            //{
            //    #region GetPortPin
            //    writer.WriteLine("/*-----------------------------------------------------------------------------");
            //    writer.WriteLine(" * FUNCTION NAME: PortPin* " + instanceName + "_" + Method + "_GetPortPin" + strSideName + "(uint8 sensor)");
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
            //    writer.WriteLine(""+instanceName+"_PortMask* " + instanceName + "_" + Method + "_GetPortPin" + strSideName + "(uint8 sensor)");
            //    writer.WriteLine("{");
            //    writer.WriteLine("	"+instanceName+"_PortMask *ptr;");
            //    writer.WriteLine("	");
            //    writer.WriteLine("	if (sensor < " + instanceName + "_TOTAL_SENSOR_COUNT" + strSideNameUpper + ")");
            //    writer.WriteLine("	{");
            //    writer.WriteLine("		ptr = &" + instanceName + "_PortMaskTable" + strSideName + "[sensor];");
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
        public void apiCollectCIntFunctionForSide(ref TextWriter writer, CyGeneralParams packParams, CyAmuxBParams sbParametr)
        {
            string strSideName = GetSideName(sbParametr.side, packParams.Configuration);
            writer.WriteLine("/*******************************************************************************");
            writer.WriteLine("*  Place your includes, defines and code here");
            writer.WriteLine("*******************************************************************************/");
            writer.WriteLine("/* `#START " + instanceName + "_ISR"+ strSideName + "_intc` */");
            writer.WriteLine("  ");
            writer.WriteLine("/* `#END` */  ");
            writer.WriteLine("  ");
            writer.WriteLine("/*******************************************************************************");
            writer.WriteLine("* Function Name: " + instanceName + "_ISR" + strSideName);
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
            writer.WriteLine("CY_ISR(" + instanceName + "_ISR" + strSideName + ")");
            writer.WriteLine("{");
            writer.WriteLine("  /*  Place your Interrupt code here. */");
            writer.WriteLine("  /* `#START " + instanceName + "_ISR" + strSideName + "` */");
            writer.WriteLine("  ");
            writer.WriteLine("  /* `#END` */");
            writer.WriteLine("	" + instanceName + "_Status" + strSideName + " &= ~" + instanceName + "_START_CAPSENSING;");
            writer.WriteLine("}");
            writer.WriteLine("  ");
        }
        #endregion
    }
}
