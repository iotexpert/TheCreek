/*******************************************************************************
* Copyright 2008-2009, Cypress Semiconductor Corporation.  All rights reserved.
* You may use this file only in accordance with the license, terms, conditions, 
* disclaimers, and limitations in the end user license agreement accompanying 
* the software package with which this file was provided.
********************************************************************************/



using System;
using System.Collections.Generic;
using System.Text;
using System.IO;

namespace  CapSense_v0_5.API
{
    public partial class M_APIGenerator
    {
        #region apiCollectCHLVariablesApiBasePart
        void apiCollectCHLVariables(ref m_StringWriter writerResult, CyGeneralParams packParams)
        {
            m_StringWriter writerMain = new m_StringWriter();
            m_StringWriter writerValHead = new m_StringWriter();
            m_StringWriter writer;

            #region  Filling Widget_Table
            //Write _WidgetTable Header

            writer = writerMain;
            int PropertiesIndex = 0;
            int ArrayCaseWidgetsCount = packParams.cyWidgetsList.GetCountDoubleFullWidgets();
            string[] listProperties = new string[packParams.cyWidgetsList.GetCountWidgetsHL()];
            foreach (ElWidget wi in packParams.cyWidgetsList.GetListWithFullWidgetsHL())
            {
                for (int i = 0; i < packParams.cyWidgetsList.GetBothParts(wi).Count; i++)
                {
                    writer = new m_StringWriter();
                    ElWidget part = packParams.cyWidgetsList.GetBothParts(wi)[i];
                    HAProps props = packParams.cyWidgetsList.GetWidgetsProperties(part);
                    writer.Write("\t{");

                    writer.Write(instanceName + "_CSHL_TYPE_" + cySensorType.GetBaseType(part.type));  /* Type of widget element */
                    if (part.GetType() == typeof(ElUnSlider))
                    {
                        if (((ElUnSlider)part).diplexing)
                           writer.Write(" | "+instanceName + "_CSHL_IS_DIPLEX");
                    }
                    writer.Write(", ");

                    List<ElTerminal> w_listTerminals = packParams.cyWidgetsList.GetTerminalsWithOutHeader(part);
                    int t_IndexOffset = 0;
                    if (w_listTerminals.Count > 0)
                        t_IndexOffset = packParams.getRawDataOffset(part);

                    writer.Write(t_IndexOffset);	/* Offset in SlotResult array */
                    writer.Write(", ");
                    writer.Write(w_listTerminals.Count);/* Number of Slot elements */
                    writer.Write(", ");
                    if (cySensorType.IsTouchPad(part.type))
                    {
                        writer.Write(((ElUnTouchPad)part).GetSeparateProps().FingerThreshold);//FigerThresholds;
                        writer.Write(", ");
                        writer.Write(((ElUnTouchPad)part).GetSeparateProps().NoiseThreshold);// NoiseThresholds;
                        writer.Write(", ");

                    }
                    else
                    {
                        writer.Write(props.GetPropertyByName(typeof(HATrProperties), props.pTrProperties.FingerThreshold));//FigerThresholds;
                        writer.Write(", ");
                        writer.Write(props.GetPropertyByName(typeof(HATrProperties), props.pTrProperties.NoiseThreshold));// NoiseThresholds;
                        writer.Write(", ");
                    }
                    writer.Write(props.GetPropertyByName(typeof(HAMiscProperties), props.pMiscProperties.m_Debounce)); //uint8 Debounce;
                    writer.Write(", ");
                    writer.Write(props.GetPropertyByName(typeof(HAMiscProperties), props.pMiscProperties.m_Hysteresis));// uint8 Hysteresis;
                    writer.Write(", ");                    
                    writer.Write(props.GetFilterMask());//Filters;			/* Raw and Pos*/
                    writer.Write(", ");
                    if ((props.GetFilterMask() != 0) || (!cySensorType.IsButtonsStrc(part.GetType())))
                        writer.Write("&" + instanceName + "_" + ElWidget.GetPrefixFull(part.GetType()) + "SettingsTable[" + packParams.cyWidgetsList.GetIndexInTypedListHL(part) + "]");
                    else
                        writer.Write("0x00");
                    writer.Write("}");
                    //Write to Array
                    if (i == 0)
                        listProperties[PropertiesIndex] = writer.ToString();
                    else
                        listProperties[PropertiesIndex + ArrayCaseWidgetsCount] = writer.ToString();
                }

                //Calculate next Index
                PropertiesIndex++;
                if (PropertiesIndex == ArrayCaseWidgetsCount)
                    PropertiesIndex += ArrayCaseWidgetsCount;
            }
            writer = writerMain;
            #endregion

            #region  WidgetSettings Table
            List<string> listFiltersDecl = new List<string>();

            foreach (Type itemType in CyWidgetsList.GetListTypes())
            {
                List<ElWidget> listWi = packParams.cyWidgetsList.GetListWidgetsByTypeHL(itemType);
                if ((listWi.Count > 0) && ((!cySensorType.IsButtonsStrc(itemType)) || (packParams.cyWidgetsList.GetCountFilterOnHL(itemType) != 0)))
                {
                    writer.WriteLine(instanceName +"_"+ElWidget.GetPrefixFull(itemType) + "Settings " + instanceName + "_" + ElWidget.GetPrefixFull(itemType) +
                                            "SettingsTable[" + listWi.Count + "] = ");
                    writer.WriteLine("{");

                    foreach (ElWidget item in listWi)
                    {
                        if (itemType == typeof(ElUnButton))
                        {
                            writer.Write("\t{0, 0, 0, 0, 0}");
                        }
                        else //Not ElUnButton type
                        {
                            writer.Write("\t{");
                            if (itemType != typeof(ElUnMatrixButton))
                            {
                                writer.Write(item.GetFullResolution()); //Resolution
                                writer.Write(", ");
                            }
                            if (itemType == typeof(ElUnSlider))
                            {
                                if (((ElUnSlider)item).diplexing)
                                    writer.Write(instanceName + "_CSHL_" + "Diplexing_" + item.GetCount()); //DiplexTable
                                else
                                    writer.Write("0");
                                writer.Write(", ");
                            }

                            //Raw Data Filtering
                            HAProps props = packParams.cyWidgetsList.GetWidgetsProperties(item);
                            TEneDis isJitterRD = (TEneDis)Enum.Parse(typeof(TEneDis), props.GetPropertyByName(typeof(HAFilterPropertiesRwDt), props.pFilterPropertiesRwDt.JitterFilterRwDt));
                            TEneDis isMedianRD = (TEneDis)Enum.Parse(typeof(TEneDis), props.GetPropertyByName(typeof(HAFilterPropertiesRwDt), props.pFilterPropertiesRwDt.MedianFilterRwDt));
                            TEneDis isAveragingRD = (TEneDis)Enum.Parse(typeof(TEneDis), props.GetPropertyByName(typeof(HAFilterPropertiesRwDt), props.pFilterPropertiesRwDt.AveragingFilterRwDt));
                            int ArrayLength = item.GetCount();
                            writer.Write(AddElement(ref writerValHead, "RawJitterRD", item, isJitterRD, ArrayLength)); //
                            writer.Write(", ");

                            writer.Write(AddElement(ref writerValHead, "Raw1MedianRD", item, isMedianRD, ArrayLength)); //
                            writer.Write(", ");
                            writer.Write(AddElement(ref writerValHead, "Raw2MedianRD", item, isMedianRD, ArrayLength)); //
                            writer.Write(", ");

                            writer.Write(AddElement(ref writerValHead, "Raw1AveragingRD", item, isAveragingRD, ArrayLength)); //
                            writer.Write(", ");
                            writer.Write(AddElement(ref writerValHead, "Raw2AveragingRD", item, isAveragingRD, ArrayLength)); //                            


                            //Position Filtering
                            if (itemType == typeof(ElUnSlider))
                                writer.Write(", 0, 0, 0, 0, 0, 0");

                            if (itemType == typeof(ElUnTouchPad))
                            {
                                TEneDis isJitterPos = (TEneDis)Enum.Parse(typeof(TEneDis), props.GetPropertyByName(typeof(HAFilterPropertiesPos), props.pFilterPropertiesPos.JitterFilterPos));
                                TEneDis isMedianPos = (TEneDis)Enum.Parse(typeof(TEneDis), props.GetPropertyByName(typeof(HAFilterPropertiesPos), props.pFilterPropertiesPos.MedianFilterPos));
                                TEneDis isAveragingPos = (TEneDis)Enum.Parse(typeof(TEneDis), props.GetPropertyByName(typeof(HAFilterPropertiesPos), props.pFilterPropertiesPos.AveragingFilterPos));
                                ArrayLength = 1;
                                writer.Write(",  0");  //First Time       
                                writer.Write(", ");
                                writer.Write(AddElement(ref writerValHead, "RawJitterPos", item, isJitterPos, ArrayLength)); //
                                writer.Write(", ");

                                writer.Write(AddElement(ref writerValHead, "Raw1MedianPos", item, isMedianPos, ArrayLength)); //
                                writer.Write(", ");
                                writer.Write(AddElement(ref writerValHead, "Raw2MedianPos", item, isMedianPos, ArrayLength)); //
                                writer.Write(", ");

                                writer.Write(AddElement(ref writerValHead, "Raw1AveragingPos", item, isAveragingPos, ArrayLength)); //
                                writer.Write(", ");
                                writer.Write(AddElement(ref writerValHead, "Raw2AveragingPos", item, isAveragingPos, ArrayLength)); //
                                writer.Write(", ");
                                writer.Write(AddElement(ref writerValHead, "Results", item, TEneDis.Enabled, ArrayLength));
                            }
                            writer.Write("}");
                        }

                        //End char
                        if (bNotLastItem(listWi.ToArray(), item))
                            writer.WriteLine(",");
                        else writer.WriteLine("");
                    }

                    writer.WriteLine("};");
                    writer.WriteLine("");
                }
            }

            #endregion

            #region  LineOUT Widget Table
            writer.WriteLine(""+instanceName+"_Widget " + instanceName + "_CSHL_WidgetTable[] = ");
            writer.WriteLine("{");
            foreach (string item in listProperties)
            {
                writer.Write(item);
                if (bNotLastItem(listProperties, item))
                    writer.WriteLine(",");
                else writer.WriteLine("");
            }
            writer.WriteLine("};");
            #endregion

            foreach (CyAmuxBParams item in packParams.localParams.listCsHalfs)
                if (packParams.localParams.bCsHalfIsEnable(item))
                {
                    eElSide side = item.side;
                    string strSideName = GetSideName(side, packParams.Configuration);
                    string strSideNameUpper = GetSideNameUpper(side, packParams.Configuration);

                    writer.WriteLine("extern "+instanceName+"_ScanSlot " + instanceName + "_ScanSlotTable" + strSideName + "[" + instanceName + "_TOTAL_SCANSLOT_COUNT" + strSideNameUpper + "]; ");
                }

            #region Static CSHL Variables
            writer.WriteLine("extern uint16 " + instanceName + "_SlotResult[" + instanceName + "_TOTAL_SCANSLOT_COUNT];");
            writer.WriteLine("");
            writer.WriteLine("uint8 " + instanceName + "_CSHL_SlotOnMask[((((" + instanceName + "_TOTAL_SCANSLOT_COUNT - " + instanceName + "_TOTAL_GENERIC_SCANSLOT_COUNT) - 1) / 8) + 1)] = {0};");
            writer.WriteLine("uint16 " + instanceName + "_CSHL_SlotBaseline[(" + instanceName + "_TOTAL_SCANSLOT_COUNT - " + instanceName + "_TOTAL_GENERIC_SCANSLOT_COUNT)] = {0};");
            writer.WriteLine("uint8 " + instanceName + "_CSHL_SlotBaselineLow[(" + instanceName + "_TOTAL_SCANSLOT_COUNT  - " + instanceName + "_TOTAL_GENERIC_SCANSLOT_COUNT)] = {0};");
            writer.WriteLine("uint16 " + instanceName + "_CSHL_SlotSignal[(" + instanceName + "_TOTAL_SCANSLOT_COUNT - " + instanceName + "_TOTAL_GENERIC_SCANSLOT_COUNT)] = {0};");
            #endregion

            List<int> listDiplexingUse = new List<int>();
            foreach (ElWidget item in packParams.cyWidgetsList.GetListWidgets())
                if (cySensorType.IsSlider(item.type))
                {
                    if(((ElUnSlider)item).diplexing)
                    if (listDiplexingUse.IndexOf(item.GetCount())==-1)
                    {   
                            writerValHead.WriteLine(AddDiplexing(item));
                            listDiplexingUse.Add(item.GetCount());
                    }
                }
            writerResult.WriteLine(writerValHead.ToString());
            writerResult.WriteLine(writer.ToString());
        }
        #region Service Functions
        string AddElement(ref m_StringWriter writer, string header, ElWidget wi, TEneDis en, int ArrayLength)
        {
            string res = "0";
            if ((ArrayLength > 0) && (en == TEneDis.Enabled))
            {
                res =  instanceName+"_"+ wi.ToString() + header;
                writer.WriteLine("uint16 " + res + "[" + ArrayLength + "];");
            }
            return res;
        }
        string AddDiplexing(ElWidget wi)
        {
            string res = "";
            if (cySensorType.IsSlider(wi.type))
            {
                res = "uint8 " + instanceName + "_CSHL_" + "Diplexing_" + wi.GetCount() + "[" + wi.GetCount() * 2 + "]";
                res += "={";
                //      void GenereteDiplexSequence()
                int length = wi.GetCount();
                int i, k;
                int[] arrIndexs = new int[2 * length];

                i = 0;
                k = 0;
                for (i = 0; i < length; i++)
                {
                    arrIndexs[i] = i;
                }
                i = 0;
                while (k < length)
                {
                    arrIndexs[k + length] = i++;
                    k += 3;
                }
                k = 1;
                while (k < length)
                {
                    arrIndexs[k + length] = i++;
                    k += 3;
                }
                k = 2;
                while (k < length)
                {
                    arrIndexs[k + length] = i++;
                    k += 3;
                }
                for (int j = 0; j < arrIndexs.Length; j++)
                {
                    res += arrIndexs[j].ToString() + ", ";
                }
                res = res.Remove(res.Length - 2);
                res += "};";
            }
            return res;
        }
        #endregion
        #endregion

        #region apiCollectCHLFunctionForSide
        public void apiCollectCHLFunctionForSide(ref m_StringWriter writer, CyGeneralParams packParams, CyAmuxBParams sbParametr)
        {

            eElSide side = sbParametr.side;
            string strSideName = GetSideName(side, packParams.Configuration);
            string strSideNameUpper = GetSideNameUpper(side, packParams.Configuration);

            string Method = sbParametr.Method.ToString();
            string Symbol = SymbolGenerate(packParams, sbParametr, packParams.Configuration);

            #region CSHL_InitializeSlotBaseline Left/Right
            writer.WriteLine(" /*-----------------------------------------------------------------------------");
            writer.WriteLine(" * FUNCTION NAME: void " + instanceName + "_CSHL_InitializeSlotBaseline" + strSideName + "(uint8 slot)");
            writer.WriteLine(" *-----------------------------------------------------------------------------");
            writer.WriteLine(" * Summary: ");
            writer.WriteLine(" *");
            writer.WriteLine(" *");
            writer.WriteLine(" * Parameters: ");
            writer.WriteLine(" *");
            writer.WriteLine(" *");
            writer.WriteLine(" * Theory:");
            writer.WriteLine(" *  See summary");
            writer.WriteLine(" *");
            writer.WriteLine(" * Side Effects:");
            writer.WriteLine(" *  None");
            writer.WriteLine(" *");
            writer.WriteLine(" *---------------------------------------------------------------------------*/");
            writer.WriteLine("void " + instanceName + "_CSHL_InitializeSlotBaseline" + strSideName + "(uint8 slot)");
            writer.WriteLine("{");
            writer.WriteLine("	void *ptr;");
            writer.WriteLine("	uint8 RawIndex = " + instanceName + "_ScanSlotTable" + strSideName + "[slot].RawIndex;");
            writer.WriteLine("	uint8 widget = " + instanceName + "_ScanSlotTable" + strSideName + "[slot].WidgetNumber;");
            writer.WriteLine("	uint8 filter_pos = RawIndex - " + instanceName + "_CSHL_WidgetTable[widget].RawOffset;");
            writer.WriteLine("	");
            writer.WriteLine("  if( widget != " + instanceName + "_CSHL_NO_WIDGET)");
            writer.WriteLine("  {");
            writer.WriteLine("	    /* Scan slot to have raw data */");
            writer.WriteLine("	    " + instanceName + "_" + Method + "_ScanSlot" + strSideName + "(slot);");
            writer.WriteLine("	    ");
            writer.WriteLine("	    /* Initialize Baseline */");
            writer.WriteLine("	    " + instanceName + "_CSHL_SlotBaseline[RawIndex] = " + instanceName + "_SlotResult[RawIndex];");
            writer.WriteLine("	    " + instanceName + "_CSHL_SlotBaselineLow[RawIndex] = 0;");
			writer.WriteLine("	    " + instanceName + "_CSHL_SlotSignal[RawIndex] = 0;");
            writer.WriteLine("		" + instanceName + "_ScanSlotTable" + strSideName + "[slot].DebounceCount = " + instanceName + "_CSHL_WidgetTable[widget].Debounce;");
            writer.WriteLine("	    ");
            writer.WriteLine("	    switch ((" + instanceName + "_CSHL_WidgetTable[widget].Type & (~" + instanceName + "_CSHL_IS_DIPLEX)))");
            writer.WriteLine("	    {");
            writer.WriteLine("		    /* This case include BTN and PROX */");
            writer.WriteLine("		    case " + instanceName + "_CSHL_TYPE_BUTTON:");
            writer.WriteLine("		    case " + instanceName + "_CSHL_TYPE_PROXIMITY:		");
            writer.WriteLine("			    ptr = " + instanceName + "_CSHL_WidgetTable[widget].AdvancedSettings;");
            writer.WriteLine("			    ");
            writer.WriteLine("			    /* Jitter filter first time initialization */");
            writer.WriteLine("			    if(" + instanceName + "_CSHL_WidgetTable[widget].Filters & " + instanceName + "_CSHL_RAW_JITTER_FILTER)");
            writer.WriteLine("			    {");
            writer.WriteLine("			    	(("+instanceName+"_BtnSettings *) ptr)->RawJitter = " + instanceName + "_SlotResult[RawIndex];");
            writer.WriteLine("			    }");
            writer.WriteLine("			    ");
            writer.WriteLine("			    /* Median filter first time initialization */");
            writer.WriteLine("			    if(" + instanceName + "_CSHL_WidgetTable[widget].Filters & " + instanceName + "_CSHL_RAW_MEDIAN_FILTER)");
            writer.WriteLine("			    {");
            writer.WriteLine("				    (("+instanceName+"_BtnSettings *) ptr)->Raw2Median = " + instanceName + "_SlotResult[RawIndex];");
            writer.WriteLine("				    (("+instanceName+"_BtnSettings *) ptr)->Raw1Median = " + instanceName + "_SlotResult[RawIndex];");
            writer.WriteLine("			    }");
            writer.WriteLine("			    ");
            writer.WriteLine("			    /* Averaging filter first time initialization */");
            writer.WriteLine("			    if(" + instanceName + "_CSHL_WidgetTable[widget].Filters & " + instanceName + "_CSHL_RAW_AVERAGING_FILTER)");
            writer.WriteLine("			    {");
            writer.WriteLine("				    (("+instanceName+"_BtnSettings *) ptr)->Raw2Averaging = " + instanceName + "_SlotResult[RawIndex];");
            writer.WriteLine("				    (("+instanceName+"_BtnSettings *) ptr)->Raw1Averaging = " + instanceName + "_SlotResult[RawIndex];		");
            writer.WriteLine("			    }");
            writer.WriteLine("			    break;");
            writer.WriteLine("		    ");
            writer.WriteLine("		    /* This case include LINEAR and RADIAL SL */");
            writer.WriteLine("		    case " + instanceName + "_CSHL_TYPE_RADIAL_SLIDER :");
            writer.WriteLine("		    case " + instanceName + "_CSHL_TYPE_LINEAR_SLIDER :");
            writer.WriteLine("			    ptr = " + instanceName + "_CSHL_WidgetTable[widget].AdvancedSettings;");
            writer.WriteLine("			    (("+instanceName+"_SlSettings *) ptr)->FirstTime=0;");
            writer.WriteLine("			    /* Jitter filter first time initialization */");
            writer.WriteLine("			    if(" + instanceName + "_CSHL_WidgetTable[widget].Filters & " + instanceName + "_CSHL_RAW_JITTER_FILTER)");
            writer.WriteLine("			    {");
            writer.WriteLine("			    	(("+instanceName+"_SlSettings *) ptr)->RawJitter[filter_pos] = " + instanceName + "_SlotResult[RawIndex];");
            writer.WriteLine("			    }");
            writer.WriteLine("			    ");
            writer.WriteLine("			    /* Median filter first time initialization */");
            writer.WriteLine("			    if(" + instanceName + "_CSHL_WidgetTable[widget].Filters & " + instanceName + "_CSHL_RAW_MEDIAN_FILTER)");
            writer.WriteLine("			    {");
            writer.WriteLine("				    (("+instanceName+"_SlSettings *) ptr)->Raw2Median[filter_pos] = " + instanceName + "_SlotResult[RawIndex];");
            writer.WriteLine("				    (("+instanceName+"_SlSettings *) ptr)->Raw1Median[filter_pos] = " + instanceName + "_SlotResult[RawIndex];");
            writer.WriteLine("			    }");
            writer.WriteLine("			    ");
            writer.WriteLine("			    /* Averaging filter first time initialization */");
            writer.WriteLine("			    if(" + instanceName + "_CSHL_WidgetTable[widget].Filters & " + instanceName + "_CSHL_RAW_AVERAGING_FILTER)");
            writer.WriteLine("			    {");
            writer.WriteLine("				    (("+instanceName+"_SlSettings *) ptr)->Raw2Averaging[filter_pos] = " + instanceName + "_SlotResult[RawIndex];");
            writer.WriteLine("				    (("+instanceName+"_SlSettings *) ptr)->Raw1Averaging[filter_pos] = " + instanceName + "_SlotResult[RawIndex];		");
            writer.WriteLine("			    }");
            writer.WriteLine("			    break;");
            writer.WriteLine("			    ");
            writer.WriteLine("		    case " + instanceName + "_CSHL_TYPE_TOUCHPAD:");
            writer.WriteLine("			    ptr = " + instanceName + "_CSHL_WidgetTable[widget].AdvancedSettings;");
            writer.WriteLine("			    (("+instanceName+"_TPSettings *) ptr)->FirstTime=0;");
            writer.WriteLine("			    /* Jitter filter first time initialization */");
            writer.WriteLine("			    if(" + instanceName + "_CSHL_WidgetTable[widget].Filters & " + instanceName + "_CSHL_RAW_JITTER_FILTER)");
            writer.WriteLine("			    {");
            writer.WriteLine("				    (("+instanceName+"_TPSettings *) ptr)->RawJitter[filter_pos] = " + instanceName + "_SlotResult[RawIndex];");
            writer.WriteLine("			    }");
            writer.WriteLine("			    ");
            writer.WriteLine("			    /* Median filter first time initialization */");
            writer.WriteLine("			    if(" + instanceName + "_CSHL_WidgetTable[widget].Filters & " + instanceName + "_CSHL_RAW_MEDIAN_FILTER)");
            writer.WriteLine("			    {");
            writer.WriteLine("				    (("+instanceName+"_TPSettings *) ptr)->Raw2Median[filter_pos] = " + instanceName + "_SlotResult[RawIndex];");
            writer.WriteLine("				    (("+instanceName+"_TPSettings *) ptr)->Raw1Median[filter_pos] = " + instanceName + "_SlotResult[RawIndex];");
            writer.WriteLine("			    }");
            writer.WriteLine("			    ");
            writer.WriteLine("			    /* Averaging filter first time initialization */");
            writer.WriteLine("			    if(" + instanceName + "_CSHL_WidgetTable[widget].Filters & " + instanceName + "_CSHL_RAW_AVERAGING_FILTER)");
            writer.WriteLine("			    {");
            writer.WriteLine("				    (("+instanceName+"_TPSettings *) ptr)->Raw2Averaging[filter_pos] = " + instanceName + "_SlotResult[RawIndex];");
            writer.WriteLine("				    (("+instanceName+"_TPSettings *) ptr)->Raw1Averaging[filter_pos] = " + instanceName + "_SlotResult[RawIndex];		");
            writer.WriteLine("			    }");
            writer.WriteLine("			    break;");
            writer.WriteLine("			    ");
            writer.WriteLine("		    case " + instanceName + "_CSHL_TYPE_MATRIX_BUTTONS:");
            writer.WriteLine("			    ");
            writer.WriteLine("			    ptr = " + instanceName + "_CSHL_WidgetTable[widget].AdvancedSettings;");
            writer.WriteLine("			    ");
            writer.WriteLine("			    /* Jitter filter first time initialization */");
            writer.WriteLine("			    if(" + instanceName + "_CSHL_WidgetTable[widget].Filters & " + instanceName + "_CSHL_RAW_JITTER_FILTER)");
            writer.WriteLine("			    {");
            writer.WriteLine("			    	(("+instanceName+"_MBSettings *) ptr)->RawJitter[filter_pos] = " + instanceName + "_SlotResult[RawIndex];");
            writer.WriteLine("			    }");
            writer.WriteLine("			    ");
            writer.WriteLine("			    /* Median filter first time initialization */");
            writer.WriteLine("			    if(" + instanceName + "_CSHL_WidgetTable[widget].Filters & " + instanceName + "_CSHL_RAW_MEDIAN_FILTER)");
            writer.WriteLine("			    {");
            writer.WriteLine("			    	(("+instanceName+"_MBSettings *) ptr)->Raw2Median[filter_pos] = " + instanceName + "_SlotResult[RawIndex];");
            writer.WriteLine("			    	(("+instanceName+"_MBSettings *) ptr)->Raw1Median[filter_pos] = " + instanceName + "_SlotResult[RawIndex];");
            writer.WriteLine("			    }");
            writer.WriteLine("			    ");
            writer.WriteLine("			    /* Averaging filter first time initialization */");
            writer.WriteLine("			    if(" + instanceName + "_CSHL_WidgetTable[widget].Filters & " + instanceName + "_CSHL_RAW_AVERAGING_FILTER)");
            writer.WriteLine("			    {");
            writer.WriteLine("			    	(("+instanceName+"_MBSettings *) ptr)->Raw2Averaging[filter_pos] = " + instanceName + "_SlotResult[RawIndex];");
            writer.WriteLine("			    	(("+instanceName+"_MBSettings *) ptr)->Raw1Averaging[filter_pos] = " + instanceName + "_SlotResult[RawIndex];		");
            writer.WriteLine("			    }");
            writer.WriteLine("			    break;");
            writer.WriteLine("		    			");
            writer.WriteLine("		    default:");
            writer.WriteLine("		    ");
            writer.WriteLine("			    break;");
            writer.WriteLine("	    }");
            writer.WriteLine("	}");
            writer.WriteLine("");
            writer.WriteLine("}");
            writer.WriteLine("");
            #endregion

            #region CSHL_InitializeAllBaseline Left/Right
            writer.WriteLine(" /*-----------------------------------------------------------------------------");
            writer.WriteLine(" * FUNCTION NAME: void " + instanceName + "_CSHL_InitializeAllBaselines" + strSideName + "(void)");
            writer.WriteLine(" *-----------------------------------------------------------------------------");
            writer.WriteLine(" * Summary: ");
            writer.WriteLine(" *");
            writer.WriteLine(" *");
            writer.WriteLine(" * Parameters: ");
            writer.WriteLine(" *");
            writer.WriteLine(" *");
            writer.WriteLine(" * Theory:");
            writer.WriteLine(" *  See summary");
            writer.WriteLine(" *");
            writer.WriteLine(" * Side Effects:");
            writer.WriteLine(" *  None");
            writer.WriteLine(" *");
            writer.WriteLine(" *---------------------------------------------------------------------------*/");
            writer.WriteLine("void " + instanceName + "_CSHL_InitializeAllBaselines" + strSideName + "(void)");
            writer.WriteLine("{");
            writer.WriteLine("	uint8 i;");
            writer.WriteLine("	");
            writer.WriteLine("	for(i = 0; i < (" + instanceName + "_TOTAL_SCANSLOT_COUNT" + strSideNameUpper + " - " + instanceName + "_TOTAL_GENERIC_SCANSLOT_COUNT" + strSideNameUpper + "); i++)");
            writer.WriteLine("	{");
            writer.WriteLine("		" + instanceName + "_CSHL_InitializeSlotBaseline" + strSideName + "(i);");
            writer.WriteLine("	}");
            writer.WriteLine("}");
            writer.WriteLine("");
            #endregion

            #region CSHL_UpdateSlotBaseline Left/Right
            writer.WriteLine(" /*-----------------------------------------------------------------------------");
            writer.WriteLine(" * FUNCTION NAME:  void " + instanceName + "_CSHL_UpdateSlotBaseline" + strSideName + "(uint8 slot)");
            writer.WriteLine(" *-----------------------------------------------------------------------------");
            writer.WriteLine(" * Summary: ");
            writer.WriteLine(" *");
            writer.WriteLine(" *");
            writer.WriteLine(" * Parameters: ");
            writer.WriteLine(" *");
            writer.WriteLine(" *");
            writer.WriteLine(" * Theory:");
            writer.WriteLine(" *  See summary");
            writer.WriteLine(" *");
            writer.WriteLine(" * Side Effects:");
            writer.WriteLine(" *  None");
            writer.WriteLine(" *");
            writer.WriteLine(" *---------------------------------------------------------------------------*/");
            writer.WriteLine(" void " + instanceName + "_CSHL_UpdateSlotBaseline" + strSideName + "(uint8 slot)");
            writer.WriteLine("{");
            writer.WriteLine("	void *ptr;");
            writer.WriteLine("	uint8 RawIndex = " + instanceName + "_ScanSlotTable" + strSideName + "[slot].RawIndex;");
            writer.WriteLine("	uint8 widget = " + instanceName + "_ScanSlotTable" + strSideName + "[slot].WidgetNumber;");
            writer.WriteLine("	uint8 filter_pos = RawIndex - " + instanceName + "_CSHL_WidgetTable[widget].RawOffset;");
            writer.WriteLine("	uint16 FilteredRawData = " + instanceName + "_SlotResult[RawIndex];");
            writer.WriteLine("	uint32 temp;");
            writer.WriteLine("	uint16 BaseRawData;");
            writer.WriteLine("	");
            writer.WriteLine("  if( widget != " + instanceName + "_CSHL_NO_WIDGET)");
            writer.WriteLine("  {");
            writer.WriteLine("	    ");
            writer.WriteLine("	    /* Do filtering */");
            writer.WriteLine("	    switch ((" + instanceName + "_CSHL_WidgetTable[widget].Type & (~" + instanceName + "_CSHL_IS_DIPLEX)))");
            writer.WriteLine("	    {");
            writer.WriteLine("		    /* This case include BTN and PROX */");
            writer.WriteLine("		    case " + instanceName + "_CSHL_TYPE_BUTTON:");
            writer.WriteLine("		    case " + instanceName + "_CSHL_TYPE_PROXIMITY:		");
            writer.WriteLine("		    	ptr = " + instanceName + "_CSHL_WidgetTable[widget].AdvancedSettings;");
            writer.WriteLine("			    ");
            writer.WriteLine("			    /* Median filter*/");
            writer.WriteLine("			    if(" + instanceName + "_CSHL_WidgetTable[widget].Filters & " + instanceName + "_CSHL_RAW_MEDIAN_FILTER)");
            writer.WriteLine("			    {");
            writer.WriteLine("			        BaseRawData=FilteredRawData;");
            writer.WriteLine("			    	FilteredRawData = " + instanceName + "_CSHL_MedianFilter(FilteredRawData, (("+instanceName+"_BtnSettings *) ptr)->Raw1Median, (("+instanceName+"_BtnSettings *) ptr)->Raw2Median);");
            writer.WriteLine("			    	(("+instanceName+"_BtnSettings *) ptr)->Raw2Median = (("+instanceName+"_BtnSettings *) ptr)->Raw1Median;");
            writer.WriteLine("			    	(("+instanceName+"_BtnSettings *) ptr)->Raw1Median = BaseRawData;");
            writer.WriteLine("			    }");
            writer.WriteLine("			    ");
            writer.WriteLine("			    /* Averaging filter*/");
            writer.WriteLine("			    if(" + instanceName + "_CSHL_WidgetTable[widget].Filters & " + instanceName + "_CSHL_RAW_AVERAGING_FILTER)");
            writer.WriteLine("			    {");
            writer.WriteLine("			        BaseRawData=FilteredRawData;");
            writer.WriteLine("			    	FilteredRawData = " + instanceName + "_CSHL_AveragingFilter(FilteredRawData, (("+instanceName+"_BtnSettings *) ptr)->Raw1Averaging, (("+instanceName+"_BtnSettings *) ptr)->Raw2Averaging);");
            writer.WriteLine("			    	(("+instanceName+"_BtnSettings *) ptr)->Raw2Averaging = (("+instanceName+"_BtnSettings *) ptr)->Raw1Averaging;");
            writer.WriteLine("			    	(("+instanceName+"_BtnSettings *) ptr)->Raw1Averaging = BaseRawData;");
            writer.WriteLine("			    }");
            writer.WriteLine("		    	");
            writer.WriteLine("		    	/* Jitter filter*/");
            writer.WriteLine("		    	if(" + instanceName + "_CSHL_WidgetTable[widget].Filters & " + instanceName + "_CSHL_RAW_JITTER_FILTER)");
            writer.WriteLine("			    {");
            writer.WriteLine("			    	FilteredRawData = " + instanceName + "_CSHL_JitterFilter(FilteredRawData, (("+instanceName+"_BtnSettings *) ptr)->RawJitter);");
            writer.WriteLine("		    		(("+instanceName+"_BtnSettings *) ptr)->RawJitter = FilteredRawData;");
            writer.WriteLine("			    }");
            writer.WriteLine("			    break;");
            writer.WriteLine("		    ");
            writer.WriteLine("		    /* This case include LINEAR and RADIAL SL */");
            writer.WriteLine("		    case " + instanceName + "_CSHL_TYPE_RADIAL_SLIDER :");
            writer.WriteLine("		    case " + instanceName + "_CSHL_TYPE_LINEAR_SLIDER :");
            writer.WriteLine("			    ptr = " + instanceName + "_CSHL_WidgetTable[widget].AdvancedSettings;");
            writer.WriteLine("			    ");
            writer.WriteLine("			    /* Median filter  */");
            writer.WriteLine("			    if(" + instanceName + "_CSHL_WidgetTable[widget].Filters & " + instanceName + "_CSHL_RAW_MEDIAN_FILTER)");
            writer.WriteLine("			    {");
            writer.WriteLine("			        BaseRawData=FilteredRawData;");
            writer.WriteLine("		    		FilteredRawData = " + instanceName + "_CSHL_MedianFilter(FilteredRawData, (("+instanceName+"_SlSettings *) ptr)->Raw1Median[filter_pos], (("+instanceName+"_SlSettings *) ptr)->Raw2Median[filter_pos]);");
            writer.WriteLine("			    	(("+instanceName+"_SlSettings *) ptr)->Raw2Median[filter_pos] = (("+instanceName+"_SlSettings *) ptr)->Raw1Median[filter_pos];");
            writer.WriteLine("		    		(("+instanceName+"_SlSettings *) ptr)->Raw1Median[filter_pos] = BaseRawData;");
            writer.WriteLine("			    }");
            writer.WriteLine("			    ");
            writer.WriteLine("			    /* Averaging filter first time initialization */");
            writer.WriteLine("			    if(" + instanceName + "_CSHL_WidgetTable[widget].Filters & " + instanceName + "_CSHL_RAW_AVERAGING_FILTER)");
            writer.WriteLine("			    {");
            writer.WriteLine("			        BaseRawData=FilteredRawData;");
            writer.WriteLine("			       	FilteredRawData = " + instanceName + "_CSHL_AveragingFilter(FilteredRawData, (("+instanceName+"_SlSettings *) ptr)->Raw1Averaging[filter_pos], (("+instanceName+"_SlSettings *) ptr)->Raw2Averaging[filter_pos]);");
            writer.WriteLine("			    	(("+instanceName+"_SlSettings *) ptr)->Raw2Averaging[filter_pos] = (("+instanceName+"_SlSettings *) ptr)->Raw1Averaging[filter_pos];");
            writer.WriteLine("			    	(("+instanceName+"_SlSettings *) ptr)->Raw1Averaging[filter_pos] = BaseRawData;				");
            writer.WriteLine("		    	}");
            writer.WriteLine("			    ");
            writer.WriteLine("			    /* Jitter filter  */");
            writer.WriteLine("			    if(" + instanceName + "_CSHL_WidgetTable[widget].Filters & " + instanceName + "_CSHL_RAW_JITTER_FILTER)");
            writer.WriteLine("			    {");
            writer.WriteLine("			    	FilteredRawData = " + instanceName + "_CSHL_JitterFilter(FilteredRawData, (("+instanceName+"_SlSettings *) ptr)->RawJitter[filter_pos]);");
            writer.WriteLine("			    	(("+instanceName+"_SlSettings *) ptr)->RawJitter[filter_pos] = FilteredRawData;");
            writer.WriteLine("			    }");
            writer.WriteLine("		    	break;");
            writer.WriteLine("		    	");
            writer.WriteLine("		    case " + instanceName + "_CSHL_TYPE_TOUCHPAD:");
            writer.WriteLine("			    ptr = " + instanceName + "_CSHL_WidgetTable[widget].AdvancedSettings;");
            writer.WriteLine("			    ");
            writer.WriteLine("			    /* Median filter  */");
            writer.WriteLine("			    if(" + instanceName + "_CSHL_WidgetTable[widget].Filters & " + instanceName + "_CSHL_RAW_MEDIAN_FILTER)");
            writer.WriteLine("		    	{");
            writer.WriteLine("			        BaseRawData=FilteredRawData;");
            writer.WriteLine("		    		FilteredRawData = " + instanceName + "_CSHL_MedianFilter(FilteredRawData, (("+instanceName+"_TPSettings *) ptr)->Raw1Median[filter_pos], (("+instanceName+"_TPSettings *) ptr)->Raw2Median[filter_pos]);");
            writer.WriteLine("			       	(("+instanceName+"_TPSettings *) ptr)->Raw2Median[filter_pos] = (("+instanceName+"_TPSettings *) ptr)->Raw1Median[filter_pos];");
            writer.WriteLine("			    	(("+instanceName+"_TPSettings *) ptr)->Raw1Median[filter_pos] = BaseRawData;");
            writer.WriteLine("		    	}");
            writer.WriteLine("		    	");
            writer.WriteLine("		    	/* Averaging filter first time initialization */");
            writer.WriteLine("		    	if(" + instanceName + "_CSHL_WidgetTable[widget].Filters & " + instanceName + "_CSHL_RAW_AVERAGING_FILTER)");
            writer.WriteLine("		    	{");
            writer.WriteLine("			        BaseRawData=FilteredRawData;");
            writer.WriteLine("		    		FilteredRawData = " + instanceName + "_CSHL_AveragingFilter(FilteredRawData, (("+instanceName+"_TPSettings *) ptr)->Raw1Averaging[filter_pos], (("+instanceName+"_TPSettings *) ptr)->Raw2Averaging[filter_pos]);");
            writer.WriteLine("			    	(("+instanceName+"_TPSettings *) ptr)->Raw2Averaging[filter_pos] = (("+instanceName+"_TPSettings *) ptr)->Raw1Averaging[filter_pos];");
            writer.WriteLine("			    	(("+instanceName+"_TPSettings *) ptr)->Raw1Averaging[filter_pos] = BaseRawData;				");
            writer.WriteLine("			    }");
            writer.WriteLine("			    ");
            writer.WriteLine("			    /* Jitter filter  */");
            writer.WriteLine("			    if(" + instanceName + "_CSHL_WidgetTable[widget].Filters & " + instanceName + "_CSHL_RAW_JITTER_FILTER)");
            writer.WriteLine("			    {");
            writer.WriteLine("			    	FilteredRawData = " + instanceName + "_CSHL_JitterFilter(FilteredRawData, (("+instanceName+"_TPSettings *) ptr)->RawJitter[filter_pos]);");
            writer.WriteLine("			    	(("+instanceName+"_TPSettings *) ptr)->RawJitter[filter_pos] = FilteredRawData;");
            writer.WriteLine("			    }");
            writer.WriteLine("			    break;");
            writer.WriteLine("			    ");
            writer.WriteLine("		    case " + instanceName + "_CSHL_TYPE_MATRIX_BUTTONS:");
            writer.WriteLine("		    	ptr = " + instanceName + "_CSHL_WidgetTable[widget].AdvancedSettings;");
            writer.WriteLine("			    ");
            writer.WriteLine("			    /* Median filter  */");
            writer.WriteLine("		    	if(" + instanceName + "_CSHL_WidgetTable[widget].Filters & " + instanceName + "_CSHL_RAW_MEDIAN_FILTER)");
            writer.WriteLine("		    	{");
            writer.WriteLine("			        BaseRawData=FilteredRawData;");
            writer.WriteLine("		    		FilteredRawData = " + instanceName + "_CSHL_MedianFilter(FilteredRawData, (("+instanceName+"_MBSettings *) ptr)->Raw1Median[filter_pos], (("+instanceName+"_MBSettings *) ptr)->Raw2Median[filter_pos]);");
            writer.WriteLine("		    		(("+instanceName+"_MBSettings *) ptr)->Raw2Median[filter_pos] = (("+instanceName+"_MBSettings *) ptr)->Raw1Median[filter_pos];");
            writer.WriteLine("		    		(("+instanceName+"_MBSettings *) ptr)->Raw1Median[filter_pos] = BaseRawData;");
            writer.WriteLine("		    	}");
            writer.WriteLine("		    	");
            writer.WriteLine("		    	/* Averaging filter first time initialization */");
            writer.WriteLine("		    	if(" + instanceName + "_CSHL_WidgetTable[widget].Filters & " + instanceName + "_CSHL_RAW_AVERAGING_FILTER)");
            writer.WriteLine("		    	{");
            writer.WriteLine("			        BaseRawData=FilteredRawData;");
            writer.WriteLine("		    		FilteredRawData = " + instanceName + "_CSHL_AveragingFilter(FilteredRawData, (("+instanceName+"_MBSettings *) ptr)->Raw1Averaging[filter_pos], (("+instanceName+"_MBSettings *) ptr)->Raw2Averaging[filter_pos]);");
            writer.WriteLine("			    	(("+instanceName+"_MBSettings *) ptr)->Raw2Averaging[filter_pos] = (("+instanceName+"_MBSettings *) ptr)->Raw1Averaging[filter_pos];");
            writer.WriteLine("			    	(("+instanceName+"_MBSettings *) ptr)->Raw1Averaging[filter_pos] = BaseRawData;				");
            writer.WriteLine("			    }");
            writer.WriteLine("		    	");
            writer.WriteLine("		    	/* Jitter filter  */");
            writer.WriteLine("			    if(" + instanceName + "_CSHL_WidgetTable[widget].Filters & " + instanceName + "_CSHL_RAW_JITTER_FILTER)");
            writer.WriteLine("			    {");
            writer.WriteLine("			    	FilteredRawData = " + instanceName + "_CSHL_JitterFilter(FilteredRawData, (("+instanceName+"_MBSettings *) ptr)->RawJitter[filter_pos]);");
            writer.WriteLine("			    	(("+instanceName+"_MBSettings *) ptr)->RawJitter[filter_pos] = FilteredRawData;");
            writer.WriteLine("			    }");
            writer.WriteLine("			    break;");
            writer.WriteLine("		    			");
            writer.WriteLine("		    default:");
            writer.WriteLine("					");
            writer.WriteLine("			    break;");
            writer.WriteLine("	    }");
            writer.WriteLine("      ");
            writer.WriteLine("	    ");
            writer.WriteLine("	    /* Baseline calculation */");
            writer.WriteLine("		    ");
            writer.WriteLine("	    /* Find the Signal */");
            writer.WriteLine("	    temp = ((uint32) FilteredRawData) - ((uint32)" + instanceName + "_CSHL_SlotBaseline[RawIndex]);");
            writer.WriteLine("	    if (temp & " + instanceName + "_CSHL_IS_NEGATIVE)");
            writer.WriteLine("	    {");
            writer.WriteLine("	    	/* RawData less that Baseline */");
            writer.WriteLine("	    	" + instanceName + "_CSHL_SlotSignal[RawIndex] = 0;");
            writer.WriteLine("	    }");
            writer.WriteLine("	    else");
            writer.WriteLine("	    {");
            writer.WriteLine("	    	/* RawData grater that Baseline */");
            writer.WriteLine("		    if(temp < 0xFFu)");
            writer.WriteLine("		    {");
            writer.WriteLine("			    " + instanceName + "_CSHL_SlotSignal[RawIndex] = (uint8) temp;");
            writer.WriteLine("		    }");
            writer.WriteLine("		    else");
            writer.WriteLine("		    {");
            writer.WriteLine("			    " + instanceName + "_CSHL_SlotSignal[RawIndex] = 0xFFu;");
            writer.WriteLine("		    }");
            writer.WriteLine("	    }");
            writer.WriteLine("		");
            writer.WriteLine("	    /* Update Baseline if lower that NoiseThreshold */");
            writer.WriteLine("	    if ( " + instanceName + "_CSHL_SlotSignal[RawIndex] < " + instanceName + "_CSHL_WidgetTable[widget].NoiseThreshold )");
            writer.WriteLine("	    {");
            writer.WriteLine("		    /* make full Baseline 23 bits */;");
            writer.WriteLine("		    temp = ((((uint32) " + instanceName + "_CSHL_SlotBaseline[RawIndex]) << 8) | ((uint32) " + instanceName + "_CSHL_SlotBaselineLow[RawIndex]));");
            writer.WriteLine("		    ");
            writer.WriteLine("		    /* add Raw Data to Baseline */");
            writer.WriteLine("	      	temp += FilteredRawData;");
            writer.WriteLine("		    ");
            writer.WriteLine("		    /* sub the high Baseline */");
            writer.WriteLine("		    temp -= " + instanceName + "_CSHL_SlotBaseline[RawIndex];");
            writer.WriteLine("		    ");
            writer.WriteLine("		    /* Put Baseline and BaselineLow */");
            writer.WriteLine("		    " + instanceName + "_CSHL_SlotBaselineLow[RawIndex] = ((uint8) temp);");
            writer.WriteLine("	    	" + instanceName + "_CSHL_SlotBaseline[RawIndex] = ((uint16) (temp >> 8));");
            writer.WriteLine("	    	");
            writer.WriteLine("	    	/* Signal is zero */");
			writer.WriteLine("		    " + instanceName + "_CSHL_SlotSignal[RawIndex] = 0;");
            writer.WriteLine("	    }	");
//            writer.WriteLine("	    else");
//            writer.WriteLine("	    {");
//            writer.WriteLine("		    /* Signal grater that NoiseThreshold */");
//            writer.WriteLine("		    //" + instanceName + "_CSHL_SlotSignal[RawIndex] -= " + instanceName + "_CSHL_WidgetTable[widget].NoiseThreshold;");
//			writer.WriteLine("		    " + instanceName + "_CSHL_SlotSignal[RawIndex] = 80;");
//            writer.WriteLine("	    }");
            writer.WriteLine("  }");
            writer.WriteLine("}");
            writer.WriteLine("");
            #endregion

            #region CSHL_UpdateAllBaselines Left/Right
            writer.WriteLine(" /*-----------------------------------------------------------------------------");
            writer.WriteLine(" * FUNCTION NAME: void " + instanceName + "_CSHL_UpdateAllBaselines" + strSideName + "(void)");
            writer.WriteLine(" *-----------------------------------------------------------------------------");
            writer.WriteLine(" * Summary: ");
            writer.WriteLine(" *");
            writer.WriteLine(" *");
            writer.WriteLine(" * Parameters: ");
            writer.WriteLine(" *");
            writer.WriteLine(" *");
            writer.WriteLine(" * Theory:");
            writer.WriteLine(" *  See summary");
            writer.WriteLine(" *");
            writer.WriteLine(" * Side Effects:");
            writer.WriteLine(" *  None");
            writer.WriteLine(" *");
            writer.WriteLine(" *---------------------------------------------------------------------------*/");
            writer.WriteLine(" void " + instanceName + "_CSHL_UpdateAllBaselines" + strSideName + "(void)");
            writer.WriteLine("{");
            writer.WriteLine("	uint8 i;");
            writer.WriteLine("	");
            writer.WriteLine("	for(i = 0; i < (" + instanceName + "_TOTAL_SCANSLOT_COUNT" + strSideNameUpper + " - " + instanceName + "_TOTAL_GENERIC_SCANSLOT_COUNT" + strSideNameUpper + "); i++)");
            writer.WriteLine("	{");
            writer.WriteLine("		" + instanceName + "_CSHL_UpdateSlotBaseline" + strSideName + "(i);");
            writer.WriteLine("	}		");
            writer.WriteLine("}");
            writer.WriteLine("");
            #endregion

            #region CSHL_CheckIsSlotActive Left/Right
            writer.WriteLine(" /*-----------------------------------------------------------------------------");
            writer.WriteLine(" * FUNCTION NAME: uint8 " + instanceName + "_CSHL_CheckIsSlotActive" + strSideName + "(uint8 Slot)");
            writer.WriteLine(" *-----------------------------------------------------------------------------");
            writer.WriteLine(" * Summary: ");
            writer.WriteLine(" *");
            writer.WriteLine(" *");
            writer.WriteLine(" * Parameters: ");
            writer.WriteLine(" *");
            writer.WriteLine(" *");
            writer.WriteLine(" * Theory:");
            writer.WriteLine(" *  See summary");
            writer.WriteLine(" *");
            writer.WriteLine(" * Side Effects:");
            writer.WriteLine(" *  None");
            writer.WriteLine(" *");
            writer.WriteLine(" *---------------------------------------------------------------------------*/");
            writer.WriteLine("uint8 " + instanceName + "_CSHL_CheckIsSlotActive" + strSideName + "(uint8 slot)");
            writer.WriteLine("{");
            writer.WriteLine("	uint8 RawIndex = " + instanceName + "_ScanSlotTable" + strSideName + "[slot].RawIndex;");
            writer.WriteLine("	uint8 widget = " + instanceName + "_ScanSlotTable" + strSideName + "[slot].WidgetNumber;");
            writer.WriteLine("	uint8 onmask = 0x01u;");
            writer.WriteLine("	");
            writer.WriteLine("	");
            writer.WriteLine("  if( widget != " + instanceName + "_CSHL_NO_WIDGET)");
            writer.WriteLine("  {");
            writer.WriteLine("	    ");
            writer.WriteLine("	    /* Get On/Off mask */");
            writer.WriteLine("	    onmask <<= (RawIndex % 8);");
            writer.WriteLine("	    ");
            writer.WriteLine("	    /* Was on */");
            writer.WriteLine("	    if (" + instanceName + "_CSHL_SlotOnMask[(RawIndex)/8] & onmask)");
            writer.WriteLine("	    {");
            writer.WriteLine("		    /* Hysteresis plus */");
            writer.WriteLine("		    if (" + instanceName + "_CSHL_SlotSignal[RawIndex] < (" + instanceName + "_CSHL_WidgetTable[widget].FingerThreshold + " + instanceName + "_CSHL_WidgetTable[widget].Hysteresis))");
            writer.WriteLine("		    {");
            writer.WriteLine("			    /* Slot inactive */");
            writer.WriteLine("			    " + instanceName + "_ScanSlotTable" + strSideName + "[slot].DebounceCount = " + instanceName + "_CSHL_WidgetTable[widget].Debounce;");
            writer.WriteLine("			    ");
            writer.WriteLine("			    " + instanceName + "_CSHL_SlotOnMask[(RawIndex)/8] &= ~onmask;");
            writer.WriteLine("              ");
            writer.WriteLine("		    }");
            writer.WriteLine("		    else");
            writer.WriteLine("		    {");
            writer.WriteLine("			    /* Slot active */");
            writer.WriteLine("			    " + instanceName + "_ScanSlotTable" + strSideName + "[slot].DebounceCount--;");
            writer.WriteLine("			    if (" + instanceName + "_ScanSlotTable" + strSideName + "[slot].DebounceCount == 0)");
            writer.WriteLine("			    {");
            writer.WriteLine("			    	" + instanceName + "_CSHL_SlotOnMask[RawIndex/8] |= onmask;");
            writer.WriteLine("			    }");
            writer.WriteLine("		    }");
            writer.WriteLine("	    }");
            writer.WriteLine("	    else");
            writer.WriteLine("	    /* Was off */");
            writer.WriteLine("	    {");
            writer.WriteLine("		    /* Hysteresis minus */");
            writer.WriteLine("		    if (" + instanceName + "_CSHL_SlotSignal[RawIndex] < (" + instanceName + "_CSHL_WidgetTable[widget].FingerThreshold - " + instanceName + "_CSHL_WidgetTable[widget].Hysteresis))");
            writer.WriteLine("		    {");
            writer.WriteLine("			    /* Slot inactive */");
            writer.WriteLine("			    " + instanceName + "_ScanSlotTable" + strSideName + "[slot].DebounceCount = " + instanceName + "_CSHL_WidgetTable[widget].Debounce;");
            writer.WriteLine("			    ");
            writer.WriteLine("			    " + instanceName + "_CSHL_SlotOnMask[RawIndex/8] &= ~onmask;");
            writer.WriteLine("              ");
            writer.WriteLine("		    }");
            writer.WriteLine("		    else");
            writer.WriteLine("		    {");
            writer.WriteLine("			    /* Slot active */");
            writer.WriteLine("			    " + instanceName + "_ScanSlotTable" + strSideName + "[slot].DebounceCount--;");
            writer.WriteLine("			    if (" + instanceName + "_ScanSlotTable" + strSideName + "[slot].DebounceCount == 0)");
            writer.WriteLine("			    {");
            writer.WriteLine("			    	" + instanceName + "_CSHL_SlotOnMask[RawIndex/8] |= onmask;");
            writer.WriteLine("			    }");
            writer.WriteLine("		    }");
            writer.WriteLine("	    }");
            writer.WriteLine("	}");
            writer.WriteLine("	");
            writer.WriteLine("  return (" + instanceName + "_CSHL_SlotOnMask[RawIndex/8] & onmask) ? 1 : 0;");
            writer.WriteLine("}");
            writer.WriteLine("");
            #endregion

            #region CSHL_CheckIsAnySlotActive Left/Right
            writer.WriteLine(" /*-----------------------------------------------------------------------------");
            writer.WriteLine(" * FUNCTION NAME: uint8 " + instanceName + "_CSHL_CheckIsAnySlotActive" + strSideName + "(void)");
            writer.WriteLine(" *-----------------------------------------------------------------------------");
            writer.WriteLine(" * Summary: ");
            writer.WriteLine(" *");
            writer.WriteLine(" *");
            writer.WriteLine(" * Parameters: ");
            writer.WriteLine(" *");
            writer.WriteLine(" *");
            writer.WriteLine(" * Theory:");
            writer.WriteLine(" *  See summary");
            writer.WriteLine(" *");
            writer.WriteLine(" * Side Effects:");
            writer.WriteLine(" *  None");
            writer.WriteLine(" *");
            writer.WriteLine(" *---------------------------------------------------------------------------*/");
            writer.WriteLine("uint8 " + instanceName + "_CSHL_CheckIsAnySlotActive" + strSideName + "(void)");
            writer.WriteLine("{");
            writer.WriteLine("	uint8 i, result=0;");
            writer.WriteLine("	");
            writer.WriteLine("	for(i = 0; i< " + instanceName + "_TOTAL_SCANSLOT_COUNT" + strSideNameUpper + "; i++)");
            writer.WriteLine("	{");
            writer.WriteLine("	    if (" + instanceName + "_CSHL_CheckIsSlotActive" + strSideName + "(i) == " + instanceName + "_CSHL_SLOT_ACTIVE)");
            writer.WriteLine("      {");
            writer.WriteLine("          result = " + instanceName + "_CSHL_SLOT_ACTIVE;");
            writer.WriteLine("      }");
            writer.WriteLine("	}");
            writer.WriteLine("	return result;");
            writer.WriteLine("}");
            #endregion

        }
        #endregion

        #region  apiCollectCHLFunctionBase
        public void apiCollectCHLFunctionBase(ref m_StringWriter writer, CyGeneralParams packParams)
        {

            if (packParams.Configuration != eMConfiguration.emSerial)
            {
                #region CSHL_InitializeAllBaselines
                writer.WriteLine(" /*-----------------------------------------------------------------------------");
                writer.WriteLine(" * FUNCTION NAME: void " + instanceName + "_CSHL_InitializeAllBaselines(void)");
                writer.WriteLine(" *-----------------------------------------------------------------------------");
                writer.WriteLine(" * Summary: ");
                writer.WriteLine(" *");
                writer.WriteLine(" *");
                writer.WriteLine(" * Parameters: ");
                writer.WriteLine(" *");
                writer.WriteLine(" *");
                writer.WriteLine(" * Theory:");
                writer.WriteLine(" *  See summary");
                writer.WriteLine(" *");
                writer.WriteLine(" * Side Effects:");
                writer.WriteLine(" *  None");
                writer.WriteLine(" *");
                writer.WriteLine(" *---------------------------------------------------------------------------*/");
                writer.WriteLine("void " + instanceName + "_CSHL_InitializeAllBaselines(void)");
                writer.WriteLine("{");
                if (packParams.localParams.isParallelFull())
                {
                    #region PapallelFunc
                    writer.WriteLine("	void *ptr;");
                    writer.WriteLine("	uint8 RawIndex;");
                    writer.WriteLine("	uint8 widget;"); // = " + instanceName + "_ScanSlotTable" + strSideName + "[slot].WidgetNumber;");
                    writer.WriteLine("	uint8 filter_pos;");
                    writer.WriteLine("	uint8 i;");
                    writer.WriteLine("	");
                    writer.WriteLine("	");
                    writer.WriteLine("	/* Scan slot to have raw data */");
                    writer.WriteLine("	" + instanceName + "_ScanAllSlots();");
                    writer.WriteLine("	");
                    writer.WriteLine("	for(i = 0; i < " + instanceName + "_TOTAL_SCANSLOT_COUNT; i++)");
                    writer.WriteLine("	{");
                    writer.WriteLine("      if ( i < " + instanceName + "_TOTAL_SCANSLOT_COUNT_LEFT )");
                    writer.WriteLine("	    {");
                    writer.WriteLine("	        widget = " + instanceName + "_ScanSlotTableLeft[i].WidgetNumber;");
                    writer.WriteLine("	        RawIndex = " + instanceName + "_ScanSlotTableLeft[i].RawIndex;");
                    writer.WriteLine("	        filter_pos = RawIndex - " + instanceName + "_CSHL_WidgetTable[widget].RawOffset;");
                    writer.WriteLine("	        " + instanceName + "_CSHL_SlotBaseline[RawIndex] = " + instanceName + "_SlotResult[RawIndex];");
                    writer.WriteLine("	        " + instanceName + "_CSHL_SlotBaselineLow[RawIndex] = 0;");
                    writer.WriteLine("	    }");
                    writer.WriteLine("	    else");
                    writer.WriteLine("	    {");
                    writer.WriteLine("	        widget = " + instanceName + "_ScanSlotTableRight[i - " + instanceName + "_TOTAL_SCANSLOT_COUNT_LEFT].WidgetNumber;");
                    writer.WriteLine("	        RawIndex = " + instanceName + "_ScanSlotTableRight[i - " + instanceName + "_TOTAL_SCANSLOT_COUNT_LEFT].RawIndex;");
                    writer.WriteLine("	        filter_pos = RawIndex - " + instanceName + "_CSHL_WidgetTable[widget].RawOffset;");
                    writer.WriteLine("	        " + instanceName + "_CSHL_SlotBaseline[RawIndex] = " + instanceName + "_SlotResult[RawIndex];");
                    writer.WriteLine("	        " + instanceName + "_CSHL_SlotBaselineLow[RawIndex] = 0;");
                    writer.WriteLine("	    }");
                    writer.WriteLine("	    ");
                    writer.WriteLine("	    if( widget != " + instanceName + "_CSHL_NO_WIDGET)");
                    writer.WriteLine("	    {");
                    writer.WriteLine("	        /* Initialize filters */");
                    writer.WriteLine("	        switch ((" + instanceName + "_CSHL_WidgetTable[widget].Type & (~" + instanceName + "_CSHL_IS_DIPLEX)))");
                    writer.WriteLine("	        {");
                    writer.WriteLine("		        /* This case include BTN and PROX */");
                    writer.WriteLine("		        case " + instanceName + "_CSHL_TYPE_BUTTON:");
                    writer.WriteLine("		        case " + instanceName + "_CSHL_TYPE_PROXIMITY:		");
                    writer.WriteLine("			        ptr = " + instanceName + "_CSHL_WidgetTable[widget].AdvancedSettings;");
                    writer.WriteLine("			        ");
                    writer.WriteLine("			        /* Jitter filter first time initialization */");
                    writer.WriteLine("		    	    if(" + instanceName + "_CSHL_WidgetTable[widget].Filters & " + instanceName + "_CSHL_RAW_JITTER_FILTER)");
                    writer.WriteLine("		    	    {");
                    writer.WriteLine("		    		    (("+instanceName+"_BtnSettings *) ptr)->RawJitter = " + instanceName + "_SlotResult[RawIndex];");
                    writer.WriteLine("		    	    }");
                    writer.WriteLine("			    ");
                    writer.WriteLine("		    	    /* Median filter first time initialization */");
                    writer.WriteLine("		    	    if(" + instanceName + "_CSHL_WidgetTable[widget].Filters & " + instanceName + "_CSHL_RAW_MEDIAN_FILTER)");
                    writer.WriteLine("		    	    {");
                    writer.WriteLine("		    		    (("+instanceName+"_BtnSettings *) ptr)->Raw2Median = " + instanceName + "_SlotResult[RawIndex];");
                    writer.WriteLine("		    		    (("+instanceName+"_BtnSettings *) ptr)->Raw1Median = " + instanceName + "_SlotResult[RawIndex];");
                    writer.WriteLine("		    	    }");
                    writer.WriteLine("		    	    ");
                    writer.WriteLine("		    	    /* Averaging filter first time initialization */");
                    writer.WriteLine("		    	    if(" + instanceName + "_CSHL_WidgetTable[widget].Filters & " + instanceName + "_CSHL_RAW_AVERAGING_FILTER)");
                    writer.WriteLine("		    	    {");
                    writer.WriteLine("		    		    (("+instanceName+"_BtnSettings *) ptr)->Raw2Averaging = " + instanceName + "_SlotResult[RawIndex];");
                    writer.WriteLine("		    		    (("+instanceName+"_BtnSettings *) ptr)->Raw1Averaging = " + instanceName + "_SlotResult[RawIndex];		");
                    writer.WriteLine("		    	    }");
                    writer.WriteLine("	    		    break;");
                    writer.WriteLine("	    	    ");
                    writer.WriteLine("	    	    /* This case include LINEAR and RADIAL SL */");
                    writer.WriteLine("	    	    case " + instanceName + "_CSHL_TYPE_RADIAL_SLIDER :");
                    writer.WriteLine("	    	    case " + instanceName + "_CSHL_TYPE_LINEAR_SLIDER :");
                    writer.WriteLine("		    	    ptr = " + instanceName + "_CSHL_WidgetTable[widget].AdvancedSettings;");
                    writer.WriteLine("		    	    ");
                    writer.WriteLine("		    	    /* Jitter filter first time initialization */");
                    writer.WriteLine("		    	    if(" + instanceName + "_CSHL_WidgetTable[widget].Filters & " + instanceName + "_CSHL_RAW_JITTER_FILTER)");
                    writer.WriteLine("		    	    {");
                    writer.WriteLine("		    	    	(("+instanceName+"_SlSettings *) ptr)->RawJitter[filter_pos] = " + instanceName + "_SlotResult[RawIndex];");
                    writer.WriteLine("	    		    }");
                    writer.WriteLine("	    		    ");
                    writer.WriteLine("		    	    /* Median filter first time initialization */");
                    writer.WriteLine("	    		    if(" + instanceName + "_CSHL_WidgetTable[widget].Filters & " + instanceName + "_CSHL_RAW_MEDIAN_FILTER)");
                    writer.WriteLine("		    	    {");
                    writer.WriteLine("		    	    	(("+instanceName+"_SlSettings *) ptr)->Raw2Median[filter_pos] = " + instanceName + "_SlotResult[RawIndex];");
                    writer.WriteLine("		    	    	(("+instanceName+"_SlSettings *) ptr)->Raw1Median[filter_pos] = " + instanceName + "_SlotResult[RawIndex];");
                    writer.WriteLine("		    	    }");
                    writer.WriteLine("		    	    ");
                    writer.WriteLine("		    	    /* Averaging filter first time initialization */");
                    writer.WriteLine("		    	    if(" + instanceName + "_CSHL_WidgetTable[widget].Filters & " + instanceName + "_CSHL_RAW_AVERAGING_FILTER)");
                    writer.WriteLine("	    	    	{");
                    writer.WriteLine("		        		(("+instanceName+"_SlSettings *) ptr)->Raw2Averaging[filter_pos] = " + instanceName + "_SlotResult[RawIndex];");
                    writer.WriteLine("		    	    	(("+instanceName+"_SlSettings *) ptr)->Raw1Averaging[filter_pos] = " + instanceName + "_SlotResult[RawIndex];		");
                    writer.WriteLine("		    	    }");
                    writer.WriteLine("	    		    break;");
                    writer.WriteLine("		    	    ");
                    writer.WriteLine("		        case " + instanceName + "_CSHL_TYPE_TOUCHPAD:");
                    writer.WriteLine("		    	    ptr = " + instanceName + "_CSHL_WidgetTable[widget].AdvancedSettings;");
                    writer.WriteLine("		    	    ");
                    writer.WriteLine("		    	    /* Jitter filter first time initialization */");
                    writer.WriteLine("		    	    if(" + instanceName + "_CSHL_WidgetTable[widget].Filters & " + instanceName + "_CSHL_RAW_JITTER_FILTER)");
                    writer.WriteLine("		    	    {");
                    writer.WriteLine("		    		    (("+instanceName+"_TPSettings *) ptr)->RawJitter[filter_pos] = " + instanceName + "_SlotResult[RawIndex];");
                    writer.WriteLine("		    	    }");
                    writer.WriteLine("		    	    ");
                    writer.WriteLine("		    	    /* Median filter first time initialization */");
                    writer.WriteLine("		    	    if(" + instanceName + "_CSHL_WidgetTable[widget].Filters & " + instanceName + "_CSHL_RAW_MEDIAN_FILTER)");
                    writer.WriteLine("		    	    {");
                    writer.WriteLine("		    		    (("+instanceName+"_TPSettings *) ptr)->Raw2Median[filter_pos] = " + instanceName + "_SlotResult[RawIndex];");
                    writer.WriteLine("		    		    (("+instanceName+"_TPSettings *) ptr)->Raw1Median[filter_pos] = " + instanceName + "_SlotResult[RawIndex];");
                    writer.WriteLine("		    	    }");
                    writer.WriteLine("		    	    ");
                    writer.WriteLine("	    		    /* Averaging filter first time initialization */");
                    writer.WriteLine("		    	    if(" + instanceName + "_CSHL_WidgetTable[widget].Filters & " + instanceName + "_CSHL_RAW_AVERAGING_FILTER)");
                    writer.WriteLine("		    	    {");
                    writer.WriteLine("	    			    (("+instanceName+"_TPSettings *) ptr)->Raw2Averaging[filter_pos] = " + instanceName + "_SlotResult[RawIndex];");
                    writer.WriteLine("		    		    (("+instanceName+"_TPSettings *) ptr)->Raw1Averaging[filter_pos] = " + instanceName + "_SlotResult[RawIndex];		");
                    writer.WriteLine("		    	    }");
                    writer.WriteLine("		    	    break;");
                    writer.WriteLine("		    	    ");
                    writer.WriteLine("	    	    case " + instanceName + "_CSHL_TYPE_MATRIX_BUTTONS:");
                    writer.WriteLine("		    	    ");
                    writer.WriteLine("		    	    ptr = " + instanceName + "_CSHL_WidgetTable[widget].AdvancedSettings;");
                    writer.WriteLine("		    	    ");
                    writer.WriteLine("		    	    /* Jitter filter first time initialization */");
                    writer.WriteLine("		    	    if(" + instanceName + "_CSHL_WidgetTable[widget].Filters & " + instanceName + "_CSHL_RAW_JITTER_FILTER)");
                    writer.WriteLine("		    	    {");
                    writer.WriteLine("		    	    	(("+instanceName+"_MBSettings *) ptr)->RawJitter[filter_pos] = " + instanceName + "_SlotResult[RawIndex];");
                    writer.WriteLine("		    	    }");
                    writer.WriteLine("		    	    ");
                    writer.WriteLine("		    	    /* Median filter first time initialization */");
                    writer.WriteLine("		    	    if(" + instanceName + "_CSHL_WidgetTable[widget].Filters & " + instanceName + "_CSHL_RAW_MEDIAN_FILTER)");
                    writer.WriteLine("		    	    {");
                    writer.WriteLine("		    	    	(("+instanceName+"_MBSettings *) ptr)->Raw2Median[filter_pos] = " + instanceName + "_SlotResult[RawIndex];");
                    writer.WriteLine("		    	    	(("+instanceName+"_MBSettings *) ptr)->Raw1Median[filter_pos] = " + instanceName + "_SlotResult[RawIndex];");
                    writer.WriteLine("		    	    }");
                    writer.WriteLine("		    	    ");
                    writer.WriteLine("		    	    /* Averaging filter first time initialization */");
                    writer.WriteLine("		    	    if(" + instanceName + "_CSHL_WidgetTable[widget].Filters & " + instanceName + "_CSHL_RAW_AVERAGING_FILTER)");
                    writer.WriteLine("		    	    {");
                    writer.WriteLine("		    	    	(("+instanceName+"_MBSettings *) ptr)->Raw2Averaging[filter_pos] = " + instanceName + "_SlotResult[RawIndex];");
                    writer.WriteLine("		    	    	(("+instanceName+"_MBSettings *) ptr)->Raw1Averaging[filter_pos] = " + instanceName + "_SlotResult[RawIndex];		");
                    writer.WriteLine("		    	    }");
                    writer.WriteLine("		    	    break;");
                    writer.WriteLine("	    	    	");
                    writer.WriteLine("	    	    default:");
                    writer.WriteLine("	    	        ");
                    writer.WriteLine("		    	    break;");
                    writer.WriteLine("	        }");
                    writer.WriteLine("	    }");
                    writer.WriteLine("	}");
                    #endregion
                }
                else
                {
                    foreach (CyAmuxBParams item in packParams.localParams.listCsHalfs)
                        if (packParams.localParams.bCsHalfIsEnable(item))
                        {
                            eElSide side = item.side;
                            writer.WriteLine("	" + instanceName + "_CSHL_InitializeAllBaselines" + side + "();");
                        }
                }
                writer.WriteLine("}");
                writer.WriteLine("");
                #endregion

                #region CSHL_UpdateAllBaselines
                writer.WriteLine(" /*-----------------------------------------------------------------------------");
                writer.WriteLine(" * FUNCTION NAME: void " + instanceName + "_CSHL_UpdateAllBaselines(void)");
                writer.WriteLine(" *-----------------------------------------------------------------------------");
                writer.WriteLine(" * Summary: ");
                writer.WriteLine(" *");
                writer.WriteLine(" *");
                writer.WriteLine(" * Parameters: ");
                writer.WriteLine(" *");
                writer.WriteLine(" *");
                writer.WriteLine(" * Theory:");
                writer.WriteLine(" *  See summary");
                writer.WriteLine(" *");
                writer.WriteLine(" * Side Effects:");
                writer.WriteLine(" *  None");
                writer.WriteLine(" *");
                writer.WriteLine(" *---------------------------------------------------------------------------*/");
                writer.WriteLine("void " + instanceName + "_CSHL_UpdateAllBaselines(void)");
                writer.WriteLine("{");
                foreach (CyAmuxBParams item in packParams.localParams.listCsHalfs)
                    if (packParams.localParams.bCsHalfIsEnable(item))
                    {
                        eElSide side = item.side;
                        writer.WriteLine("	" + instanceName + "_CSHL_UpdateAllBaselines" + side + "();");
                    }
                writer.WriteLine("}");
                writer.WriteLine("");
                #endregion

                #region CSHL_CheckIsAnySlotActive
                writer.WriteLine(" /*-----------------------------------------------------------------------------");
                writer.WriteLine(" * FUNCTION NAME: void " + instanceName + "_CSHL_CheckIsAnySlotActive(void) ");
                writer.WriteLine(" *-----------------------------------------------------------------------------");
                writer.WriteLine(" * Summary: ");
                writer.WriteLine(" *");
                writer.WriteLine(" *");
                writer.WriteLine(" * Parameters: ");
                writer.WriteLine(" *");
                writer.WriteLine(" *");
                writer.WriteLine(" * Theory:");
                writer.WriteLine(" *  See summary");
                writer.WriteLine(" *");
                writer.WriteLine(" * Side Effects:");
                writer.WriteLine(" *  None");
                writer.WriteLine(" *");
                writer.WriteLine(" *---------------------------------------------------------------------------*/");
                writer.WriteLine("uint8 " + instanceName + "_CSHL_CheckIsAnySlotActive(void)");
                writer.WriteLine("{");
                foreach (CyAmuxBParams item in packParams.localParams.listCsHalfs)
                    if (packParams.localParams.bCsHalfIsEnable(item))
                    {
                        eElSide side = item.side;
                        writer.WriteLine("  uint8 Active" + side + ";");
                    }
                writer.WriteLine("");
                foreach (CyAmuxBParams item in packParams.localParams.listCsHalfs)
                    if (packParams.localParams.bCsHalfIsEnable(item))
                    {
                        eElSide side = item.side;
                        writer.WriteLine("  Active" + side + " = " + instanceName + "_CSHL_CheckIsAnySlotActive" + side + "();");
                    }

                writer.Write("  return (");
                string separator = "";
                foreach (CyAmuxBParams item in packParams.localParams.listCsHalfs)
                    if (packParams.localParams.bCsHalfIsEnable(item))
                    {
                        writer.Write(separator + "Active" + item.side);
                        separator = " | ";
                    }
                writer.WriteLine(");");


                writer.WriteLine("}");
                writer.WriteLine("");
                #endregion
            }
        }
        #endregion

        #region  apiCollectCHLFunctionCentroid
        public void apiCollectCHLFunctionCentroid(ref m_StringWriter writer, CyGeneralParams packParams)
        {
            if (packParams.cyWidgetsList.GetCountWidgetsSameType(sensorType.Linear_Slider)>0)
            {
                #region CSHL_GetCentroidPos
                writer.WriteLine(" /*-----------------------------------------------------------------------------");
                writer.WriteLine(" * FUNCTION NAME: uint16 " + instanceName + "_CSHL_GetCentroidPos(uint8 widget)");
                writer.WriteLine(" *-----------------------------------------------------------------------------");
                writer.WriteLine(" * Summary: ");
                writer.WriteLine(" *");
                writer.WriteLine(" *");
                writer.WriteLine(" * Parameters: ");
                writer.WriteLine(" *");
                writer.WriteLine(" *");
                writer.WriteLine(" * Theory:");
                writer.WriteLine(" *  See summary");
                writer.WriteLine(" *");
                writer.WriteLine(" * Side Effects:");
                writer.WriteLine(" *  None");
                writer.WriteLine(" *");
                writer.WriteLine(" *---------------------------------------------------------------------------*/");
                writer.WriteLine("uint16 " + instanceName + "_CSHL_GetCentroidPos(uint8 widget)");
                writer.WriteLine("{");
                writer.WriteLine("	"+instanceName+"_SlSettings *ptr = ("+instanceName+"_SlSettings *) " + instanceName + "_CSHL_WidgetTable[widget].AdvancedSettings;");
                writer.WriteLine("	uint8 *diplex = ptr->DiplexTable;");
                writer.WriteLine("	uint8 offset = " + instanceName + "_CSHL_WidgetTable[widget].RawOffset;");
                writer.WriteLine("	uint8 count = " + instanceName + "_CSHL_WidgetTable[widget].ScanSlotCount;");
                writer.WriteLine("	uint8 CurPos = 0;");
                writer.WriteLine("	uint8 Index = 0;");
                writer.WriteLine("	uint8 CurCtrdStartPos = 0xFFu;");
                writer.WriteLine("	uint8 CurCntrdSize = 0;");
                writer.WriteLine("	uint8 BiggestCtrdStartPos = 0;");
                writer.WriteLine("	uint8 BiggestCtrdSize = 0;");
                writer.WriteLine("	uint8 localmax = 0xFFu;");
                writer.WriteLine("	uint8 i;");
                writer.WriteLine("	uint8 posPrev, pos, posNext;");
                writer.WriteLine("	int32 Numerator = 0;");
                writer.WriteLine("	int32 Denominator  = 0;");
                writer.WriteLine("	uint16 Position;");
                writer.WriteLine("	uint16 BasePos;");
                writer.WriteLine("	");
                writer.WriteLine("	");
                writer.WriteLine("	if (" + instanceName + "_CSHL_WidgetTable[widget].Type & " + instanceName + "_CSHL_TYPE_LINEAR_SLIDER)");
                writer.WriteLine("	{");
                writer.WriteLine("		/***********************************************");
                writer.WriteLine("		");
                writer.WriteLine("			CENTROID CALCULATIONS");
                writer.WriteLine("		");
                writer.WriteLine("		**************************************************/");
                writer.WriteLine("      ");
                writer.WriteLine("		while(1)");
                writer.WriteLine("		{");
                writer.WriteLine("			if (" + instanceName + "_CSHL_SlotSignal[offset+CurPos] > 0)");
                writer.WriteLine("			{		");
                writer.WriteLine("				if (CurCtrdStartPos == 0xFFu)");
                writer.WriteLine("				{");
                writer.WriteLine("					/* Start of Centroid */");
                writer.WriteLine("					CurCtrdStartPos = Index;");
                writer.WriteLine("					CurCntrdSize++;");
                writer.WriteLine("				}");
                writer.WriteLine("				else");
                writer.WriteLine("				{");
                writer.WriteLine("					CurCntrdSize++;");
                writer.WriteLine("				}");
                writer.WriteLine("			}");
                writer.WriteLine("			else");
                writer.WriteLine("			{");
                writer.WriteLine("				if(CurCntrdSize > 0)");
                writer.WriteLine("				{");
                writer.WriteLine("					/* We are in the end of current */");
                writer.WriteLine("					if(CurCntrdSize > BiggestCtrdSize)");
                writer.WriteLine("					{");
                writer.WriteLine("						BiggestCtrdSize = CurCntrdSize;");
                writer.WriteLine("						BiggestCtrdStartPos = CurCtrdStartPos;");
                writer.WriteLine("						CurCntrdSize = 0;");
                writer.WriteLine("						CurCtrdStartPos = 0xFFu;");
                writer.WriteLine("					}");
                writer.WriteLine("					else");
                writer.WriteLine("					{");
                writer.WriteLine("						CurCntrdSize = 0;");
                writer.WriteLine("						CurCtrdStartPos = 0xFFu;");
                writer.WriteLine("					}");
                writer.WriteLine("					");
                writer.WriteLine("				}");
                writer.WriteLine("			}");
                writer.WriteLine("	        ");
                writer.WriteLine("			if(" + instanceName + "_CSHL_WidgetTable[widget].Type & " + instanceName + "_CSHL_IS_DIPLEX)");
                writer.WriteLine("			{");
                writer.WriteLine("				CurPos = diplex[Index+1];");
                writer.WriteLine("				if(Index == ((count*2)-1))");
                writer.WriteLine("				{");
                writer.WriteLine("					break;");
                writer.WriteLine("				}");
                writer.WriteLine("			}");
                writer.WriteLine("			else");
                writer.WriteLine("			{");
                writer.WriteLine("				if(Index == (count-1))");
                writer.WriteLine("				{");
                writer.WriteLine("					break;");
                writer.WriteLine("				}");
                writer.WriteLine("				CurPos++;");
                writer.WriteLine("			}		");
                writer.WriteLine("			");
                writer.WriteLine("			Index++;");
                writer.WriteLine("		}");
                writer.WriteLine("      ");
                writer.WriteLine("		/* Find the biggest Centroid ");
                writer.WriteLine("		* if two are the same size, last one wins");
                writer.WriteLine("		* We are in the end of current */");
                writer.WriteLine("		if(CurCntrdSize >= BiggestCtrdSize) ");
                writer.WriteLine("		{");
                writer.WriteLine("			BiggestCtrdSize = CurCntrdSize;");
                writer.WriteLine("			BiggestCtrdStartPos = CurCtrdStartPos;");
                writer.WriteLine("		}");
                writer.WriteLine("		");
                writer.WriteLine("		if (BiggestCtrdSize >= 2)");
                writer.WriteLine("		{");
                writer.WriteLine("			for (i = 0; i < BiggestCtrdSize; i++)");
                writer.WriteLine("			{");
                writer.WriteLine("				if(" + instanceName + "_CSHL_WidgetTable[widget].Type & " + instanceName + "_CSHL_IS_DIPLEX)");
                writer.WriteLine("				{");
                writer.WriteLine("					posPrev = diplex[BiggestCtrdStartPos + i-1];");
                writer.WriteLine("					pos = diplex[BiggestCtrdStartPos + i];");
                writer.WriteLine("					posNext = diplex[BiggestCtrdStartPos + i+1];");
                writer.WriteLine("				}");
                writer.WriteLine("				else");
                writer.WriteLine("				{");
                writer.WriteLine("					posPrev = BiggestCtrdStartPos + i-1;");
                writer.WriteLine("					pos = BiggestCtrdStartPos + i;");
                writer.WriteLine("					posNext = BiggestCtrdStartPos + i+1;");
                writer.WriteLine("				}");
                writer.WriteLine("			");
                writer.WriteLine("				/* Ignore if lower that FingerThreshold */");
                writer.WriteLine("				if(" + instanceName + "_CSHL_SlotSignal[offset + pos] > " + instanceName + "_CSHL_WidgetTable[widget].FingerThreshold)");
                writer.WriteLine("				{");
                writer.WriteLine("					if (i == 0)");
                writer.WriteLine("					{");
                writer.WriteLine("						/* First element pos > posNext */");
                writer.WriteLine("						if(" + instanceName + "_CSHL_SlotSignal[offset + pos] > " + instanceName + "_CSHL_SlotSignal[offset + posNext])");
                writer.WriteLine("						{");
                writer.WriteLine("							/* Find LocalMax */");
                writer.WriteLine("							localmax = i;");
                writer.WriteLine("							break;");
                writer.WriteLine("						}");
                writer.WriteLine("						else");
                writer.WriteLine("						{");
                writer.WriteLine("							if(" + instanceName + "_CSHL_SlotSignal[offset + pos] == " + instanceName + "_CSHL_SlotSignal[offset + posNext])");
                writer.WriteLine("							{");
                writer.WriteLine("								/* Compare BaselinesLow (i+1) and (i) */");
                writer.WriteLine("								if(" + instanceName + "_CSHL_SlotBaselineLow[offset + pos] < " + instanceName + "_CSHL_SlotBaselineLow[offset + posNext])");
                writer.WriteLine("								{");
                writer.WriteLine("									localmax = i;");
                writer.WriteLine("									break;");
                writer.WriteLine("								}");
                writer.WriteLine("							}");
                writer.WriteLine("						}");
                writer.WriteLine("					}");
                writer.WriteLine("					else if (i == (BiggestCtrdSize-1))");
                writer.WriteLine("					{");
                writer.WriteLine("						/* Last element posNext > pos */");
                writer.WriteLine("						if(" + instanceName + "_CSHL_SlotSignal[offset + pos] > " + instanceName + "_CSHL_SlotSignal[offset + posPrev])");
                writer.WriteLine("						{");
                writer.WriteLine("							/* Find LocalMax */");
                writer.WriteLine("							localmax = i;");
                writer.WriteLine("							break;");
                writer.WriteLine("						}");
                writer.WriteLine("						else");
                writer.WriteLine("						{");
                writer.WriteLine("							if(" + instanceName + "_CSHL_SlotSignal[offset + pos] == " + instanceName + "_CSHL_SlotSignal[offset + posPrev])");
                writer.WriteLine("							{");
                writer.WriteLine("								/* Compare BaselinesLow (i+1) and (i) */");
                writer.WriteLine("								if(" + instanceName + "_CSHL_SlotBaselineLow[offset + pos] < " + instanceName + "_CSHL_SlotBaselineLow[offset + posPrev])");
                writer.WriteLine("								{");
                writer.WriteLine("									localmax = i;");
                writer.WriteLine("									break;");
                writer.WriteLine("								}");
                writer.WriteLine("							}	");
                writer.WriteLine("						}");
                writer.WriteLine("						");
                writer.WriteLine("					}");
                writer.WriteLine("					else");
                writer.WriteLine("					{");
                writer.WriteLine("						/* point (i+1) > (i) and (i+1) > (i+2) */");
                writer.WriteLine("						if((" + instanceName + "_CSHL_SlotSignal[offset + pos] > " + instanceName + "_CSHL_SlotSignal[offset + posNext]) && (" + instanceName + "_CSHL_SlotSignal[offset + pos] > " + instanceName + "_CSHL_SlotSignal[offset + posPrev]))");
                writer.WriteLine("						{");
                writer.WriteLine("							localmax = i;");
                writer.WriteLine("							break;");
                writer.WriteLine("						}");
                writer.WriteLine("						else");
                writer.WriteLine("						{");
                writer.WriteLine("							/* (i+1) == (i), compare BaselinesLow */");
                writer.WriteLine("							if(" + instanceName + "_CSHL_SlotSignal[offset + pos] == " + instanceName + "_CSHL_SlotSignal[offset + posNext])");
                writer.WriteLine("							{");
                writer.WriteLine("								/* Compare BaselinesLow (i+1) and (i), if lower go next */");
                writer.WriteLine("								if(" + instanceName + "_CSHL_SlotBaselineLow[offset + pos] < " + instanceName + "_CSHL_SlotBaselineLow[offset + posNext])");
                writer.WriteLine("								{");
                writer.WriteLine("									if(" + instanceName + "_CSHL_SlotSignal[offset + pos] > " + instanceName + "_CSHL_SlotSignal[offset + posPrev])");
                writer.WriteLine("									{");
                writer.WriteLine("										localmax = i;");
                writer.WriteLine("										break;");
                writer.WriteLine("									}");
                writer.WriteLine("									else");
                writer.WriteLine("									{");
                writer.WriteLine("										/* (i+1) == (i+2), compare BaselinesLow */");
                writer.WriteLine("										if ((" + instanceName + "_CSHL_SlotSignal[offset + pos] == " + instanceName + "_CSHL_SlotSignal[offset + posPrev]) && (" + instanceName + "_CSHL_SlotBaselineLow[offset + pos] < " + instanceName + "_CSHL_SlotBaselineLow[offset + posPrev]))");
                writer.WriteLine("										{");
                writer.WriteLine("											localmax = i;");
                writer.WriteLine("											break;");
                writer.WriteLine("										}");
                writer.WriteLine("									}");
                writer.WriteLine("								}");
                writer.WriteLine("							}");
                writer.WriteLine("							if(" + instanceName + "_CSHL_SlotSignal[offset + pos] == " + instanceName + "_CSHL_SlotSignal[offset + posPrev])");
                writer.WriteLine("							{");
                writer.WriteLine("								/* Compare BaselinesLow (i+1) and (i), if lower go next */");
                writer.WriteLine("								if (" + instanceName + "_CSHL_SlotBaselineLow[offset + pos] < " + instanceName + "_CSHL_SlotBaselineLow[offset + posPrev])");
                writer.WriteLine("								{");
                writer.WriteLine("									if (" + instanceName + "_CSHL_SlotSignal[offset + pos] > " + instanceName + "_CSHL_SlotSignal[offset + posNext])");
                writer.WriteLine("									{");
                writer.WriteLine("										localmax = i;");
                writer.WriteLine("										break;");
                writer.WriteLine("									}");
                writer.WriteLine("									else");
                writer.WriteLine("									{");
                writer.WriteLine("										/* (i+1) == (i+2), compare BaselinesLow */");
                writer.WriteLine("										if((" + instanceName + "_CSHL_SlotSignal[offset + pos] == " + instanceName + "_CSHL_SlotSignal[offset + posNext]) && (" + instanceName + "_CSHL_SlotBaselineLow[offset + pos] < " + instanceName + "_CSHL_SlotBaselineLow[offset + posNext]))");
                writer.WriteLine("										{");
                writer.WriteLine("											localmax = i;");
                writer.WriteLine("											break;");
                writer.WriteLine("										}");
                writer.WriteLine("									}");
                writer.WriteLine("								}");
                writer.WriteLine("							}");
                writer.WriteLine("						}");
                writer.WriteLine("					}");
                writer.WriteLine("				}");
                writer.WriteLine("			}");
                writer.WriteLine("		}");
                writer.WriteLine("		else if	((!(" + instanceName + "_CSHL_WidgetTable[widget].Type & " + instanceName + "_CSHL_IS_DIPLEX)) && (BiggestCtrdSize == 1))");
                writer.WriteLine("		{");
                writer.WriteLine("			pos = BiggestCtrdStartPos;");
                writer.WriteLine("			/* Ignore if lower that FingerThreshold */");
                writer.WriteLine("			if(" + instanceName + "_CSHL_SlotSignal[offset + pos] > " + instanceName + "_CSHL_WidgetTable[widget].FingerThreshold)");
                writer.WriteLine("			{");
                writer.WriteLine("				localmax = 0;");
                writer.WriteLine("		    }");
                writer.WriteLine("		    else");
                writer.WriteLine("		    {");
                writer.WriteLine("				localmax = 0xFF;");
                writer.WriteLine("		    }");
                writer.WriteLine("		}");
                writer.WriteLine("		else");
                writer.WriteLine("		{");
                writer.WriteLine("			localmax = 0xFF;");
                writer.WriteLine("		}");
                writer.WriteLine("		");
                writer.WriteLine("		if (localmax < 0xFFu)");
                writer.WriteLine("		{");
                writer.WriteLine("			/* Find positions of localmax and near it */");
                writer.WriteLine("			if(" + instanceName + "_CSHL_WidgetTable[widget].Type & " + instanceName + "_CSHL_IS_DIPLEX)");
                writer.WriteLine("			{");
                writer.WriteLine("				posPrev = diplex[BiggestCtrdStartPos + localmax-1];");
                writer.WriteLine("				pos = diplex[BiggestCtrdStartPos + localmax];");
                writer.WriteLine("				posNext = diplex[BiggestCtrdStartPos + localmax+1];");
                writer.WriteLine("			}");
                writer.WriteLine("			else");
                writer.WriteLine("			{");
                writer.WriteLine("				posPrev = BiggestCtrdStartPos + localmax-1;");
                writer.WriteLine("				pos = BiggestCtrdStartPos + localmax;");
                writer.WriteLine("				posNext = BiggestCtrdStartPos + localmax+1;");
                writer.WriteLine("			}");
                writer.WriteLine("			");
                writer.WriteLine("			if (BiggestCtrdSize >= 2)		/* The Biggest Centriod Size is grater that 2, need interpolation */");
                writer.WriteLine("			{	");
                writer.WriteLine("				/* Calculate Sum(Si) */");
                writer.WriteLine("				if (pos == BiggestCtrdStartPos)");
                writer.WriteLine("				{");
                writer.WriteLine("					/* Start of Centroid */");
                writer.WriteLine("					Numerator = ((int32)" + instanceName + "_CSHL_SlotSignal[offset + posNext]);");
                writer.WriteLine("					Denominator  =  ((int32) " + instanceName + "_CSHL_SlotSignal[offset + pos]) + ((int32) " + instanceName + "_CSHL_SlotSignal[offset + posNext]);");
                writer.WriteLine("				}");
                writer.WriteLine("				else");
                writer.WriteLine("				{");
                writer.WriteLine("					if (pos == BiggestCtrdStartPos + (BiggestCtrdSize-1))");
                writer.WriteLine("					{");
                writer.WriteLine("						/* End of Centroid */");
                writer.WriteLine("						Numerator = ((int32) " + instanceName + "_CSHL_SlotSignal[offset + posPrev]) * (-1);");
                writer.WriteLine("						Denominator  = ((int32) " + instanceName + "_CSHL_SlotSignal[offset + pos]) + ((int32) " + instanceName + "_CSHL_SlotSignal[offset + posPrev]);");
                writer.WriteLine("					}");
                writer.WriteLine("					else");
                writer.WriteLine("					{");
                writer.WriteLine("						/* Not first Not last */");
                writer.WriteLine("						Numerator = ((int32) " + instanceName + "_CSHL_SlotSignal[offset + posNext]) - ((int32) " + instanceName + "_CSHL_SlotSignal[offset + posPrev]);");
                writer.WriteLine("						Denominator  = ((int32) " + instanceName + "_CSHL_SlotSignal[offset + posPrev]) + ((int32) " + instanceName + "_CSHL_SlotSignal[offset + pos]) + ((int32) " + instanceName + "_CSHL_SlotSignal[offset + posNext]);");
                writer.WriteLine("	                    ");
                writer.WriteLine("					}");
                writer.WriteLine("				}");
                writer.WriteLine("				");
                writer.WriteLine("				Numerator <<= 8;");
                writer.WriteLine("				/* Div (Numerator/Denominator) * Resolution */");
                writer.WriteLine("				Denominator  = ((Numerator/Denominator) + (((uint16) pos )<< 8)) *  ptr->Resolution;");
				writer.WriteLine("				");
				writer.WriteLine("				if(Denominator < 0)");
				writer.WriteLine("				{");
				writer.WriteLine("					Denominator = Denominator * (-1);");
				writer.WriteLine("				}");
				writer.WriteLine("				");
                writer.WriteLine("				Position = HI16(Denominator);");
                writer.WriteLine("			}");
                writer.WriteLine("		    else						/* The Biggest Centriod Size 1, NOT interpolation needed */");
                writer.WriteLine("			{");
                writer.WriteLine("				Denominator = (pos * ptr->Resolution) + 0x7Fu;");
                writer.WriteLine("				Position = HI8(Denominator);");
                writer.WriteLine("			}");
                writer.WriteLine("			");
                writer.WriteLine("			/*****************************");
                writer.WriteLine("				FILTERING");
                writer.WriteLine("			******************************/");
                writer.WriteLine("			if (ptr->FirstTime == 0)				/* Initialize the filters */");
                writer.WriteLine("			{");
                writer.WriteLine("				/* Jitter filter first time initialization */");
                writer.WriteLine("				if(" + instanceName + "_CSHL_WidgetTable[widget].Filters & " + instanceName + "_CSHL_POS_JITTER_FILTER)");
                writer.WriteLine("				{");
                writer.WriteLine("					ptr->PosJitter = Position;");
                writer.WriteLine("				}");
                writer.WriteLine("					");
                writer.WriteLine("				/* Median filter first time initialization */");
                writer.WriteLine("				if(" + instanceName + "_CSHL_WidgetTable[widget].Filters & " + instanceName + "_CSHL_POS_MEDIAN_FILTER)");
                writer.WriteLine("				{");
                writer.WriteLine("					ptr->Pos2Median = Position;");
                writer.WriteLine("					ptr->Pos1Median = Position;");
                writer.WriteLine("				}");
                writer.WriteLine("				");
                writer.WriteLine("				/* Averaging filter first time initialization */");
                writer.WriteLine("				if(" + instanceName + "_CSHL_WidgetTable[widget].Filters & " + instanceName + "_CSHL_POS_AVERAGING_FILTER)");
                writer.WriteLine("				{");
                writer.WriteLine("					ptr->Pos2Averaging = Position;");
                writer.WriteLine("					ptr->Pos1Averaging = Position;		");
                writer.WriteLine("				}");
                writer.WriteLine("				ptr->FirstTime = 1;");
                writer.WriteLine("			}");
                writer.WriteLine("			else							/* Do the filtering */");
                writer.WriteLine("			{");
                writer.WriteLine("				/* Median filter*/");
                writer.WriteLine("				if(" + instanceName + "_CSHL_WidgetTable[widget].Filters & " + instanceName + "_CSHL_POS_MEDIAN_FILTER)");
                writer.WriteLine("				{");
                writer.WriteLine("				    BasePos = Position;");
                writer.WriteLine("					Position = " + instanceName + "_CSHL_MedianFilter(Position, ptr->Pos1Median, ptr->Pos2Median);");
                writer.WriteLine("					ptr->Pos2Median = ptr->Pos1Median;");
                writer.WriteLine("						ptr->Pos1Median = BasePos;");
                writer.WriteLine("				}");
                writer.WriteLine("				");
                writer.WriteLine("				/* Averaging filter first time initialization */");
                writer.WriteLine("				if(" + instanceName + "_CSHL_WidgetTable[widget].Filters & " + instanceName + "_CSHL_POS_AVERAGING_FILTER)");
                writer.WriteLine("				{");
                writer.WriteLine("					BasePos = Position;");
                writer.WriteLine("					Position = " + instanceName + "_CSHL_AveragingFilter(Position, ptr->Pos1Averaging, ptr->Pos2Averaging);");
                writer.WriteLine("					ptr->Pos2Averaging = ptr->Pos1Averaging;");
                writer.WriteLine("					ptr->Pos1Averaging= BasePos;");
                writer.WriteLine("				}");
                writer.WriteLine("				");
                writer.WriteLine("				/* Jitter filter*/");
                writer.WriteLine("				if(" + instanceName + "_CSHL_WidgetTable[widget].Filters & " + instanceName + "_CSHL_POS_JITTER_FILTER)");
                writer.WriteLine("				{");
                writer.WriteLine("					Position = " + instanceName + "_CSHL_JitterFilter(Position, ptr->PosJitter);");
                writer.WriteLine("					ptr->PosJitter = Position;");
                writer.WriteLine("				}");
                writer.WriteLine("			}");
                writer.WriteLine("		}");
                writer.WriteLine("		else");
                writer.WriteLine("		{");
                writer.WriteLine("			/* Local max doesn't find*/");
                writer.WriteLine("			Position = 0x00FFu;");
                writer.WriteLine("		}");
                writer.WriteLine("	}");
                writer.WriteLine("	else");
                writer.WriteLine("	{");
                writer.WriteLine("		/* This Widget isn't Linear Slider Widget */");
                writer.WriteLine("		Position = 0x00FFu;");
                writer.WriteLine("	}");
                writer.WriteLine("  ");
                writer.WriteLine("	return Position;");
                writer.WriteLine("}");
                writer.WriteLine("");
                #endregion
            }

            if (packParams.cyWidgetsList.GetCountWidgetsSameType(sensorType.Radial_Slider) > 0)
            {
                #region CSHL_GetRadialCentroidPos
                #region CSHL_RotarySliderArcTan2
                writer.WriteLine("/*-----------------------------------------------------------------------------");
                writer.WriteLine(" * FUNCTION NAME: uint16 " + instanceName + "_CSHL_RotarySliderArcTan2(int32 X, int32 Y)");
                writer.WriteLine(" *-----------------------------------------------------------------------------");
                writer.WriteLine(" * Summary: ");
                writer.WriteLine(" *");
                writer.WriteLine(" *");
                writer.WriteLine(" * Parameters: ");
                writer.WriteLine(" *");
                writer.WriteLine(" *");
                writer.WriteLine(" * Theory:");
                writer.WriteLine(" *  See summary");
                writer.WriteLine(" *");
                writer.WriteLine(" * Side Effects:");
                writer.WriteLine(" *  None");
                writer.WriteLine(" *");
                writer.WriteLine("*---------------------------------------------------------------------------*/");
                writer.WriteLine("uint16 " + instanceName + "_CSHL_RotarySliderArcTan2(int32 X, int32 Y)");
                writer.WriteLine("{");
                writer.WriteLine("	int32 Tmp, TempCalc;");
                writer.WriteLine("	uint8 i;");
                writer.WriteLine("	uint16 Ang = 0;");
                writer.WriteLine("");
                writer.WriteLine("	if (Y == 0)");
                writer.WriteLine("	{");
                writer.WriteLine("		if (X < 0)");
                writer.WriteLine("		{");
                writer.WriteLine("			Ang = " + instanceName + "_CSHL_ROTARY_SLIDER_A180*4;");
                writer.WriteLine("		}");
                writer.WriteLine("		else");
                writer.WriteLine("		{");
                writer.WriteLine("			Ang = 0;  ");
                writer.WriteLine("		}");
                writer.WriteLine("		");
                writer.WriteLine("	}");
                writer.WriteLine("	else");
                writer.WriteLine("	{");
                writer.WriteLine("		if (X == 0)");
                writer.WriteLine("		{");
                writer.WriteLine("			if (Y > 0)");
                writer.WriteLine("			{");
                writer.WriteLine("				Ang = " + instanceName + "_CSHL_ROTARY_SLIDER_A90*4;");
                writer.WriteLine("			}");
                writer.WriteLine("			else");
                writer.WriteLine("			if (Y < 0)");
                writer.WriteLine("			{");
                writer.WriteLine("				Ang = " + instanceName + "_CSHL_ROTARY_SLIDER_A270*4;  /* 270 */");
                writer.WriteLine("			}");
                writer.WriteLine("	");
                writer.WriteLine("		}");
                writer.WriteLine("		else");
                writer.WriteLine("		{");
                writer.WriteLine("			if (X < 0)");
                writer.WriteLine("			{");
                writer.WriteLine("				X = -X;");
                writer.WriteLine("				Y = -Y;");
                writer.WriteLine("				Ang = " + instanceName + "_CSHL_ROTARY_SLIDER_A180*4;");
                writer.WriteLine("			}");
                writer.WriteLine("			else ");
                writer.WriteLine("			{	");
                writer.WriteLine("				if (Y > 0)");
                writer.WriteLine("				{");
                writer.WriteLine("					Ang = " + instanceName + "_CSHL_ROTARY_SLIDER_A360*4;");
                writer.WriteLine("				}");
                writer.WriteLine("			}			");
                writer.WriteLine("		");
                writer.WriteLine("			Tmp = X;");
                writer.WriteLine("			X = Y;");
                writer.WriteLine("			");
                writer.WriteLine("			if (Y < 0)");
                writer.WriteLine("			{");
                writer.WriteLine("				X = -X;");
                writer.WriteLine("				Y = Tmp;");
                writer.WriteLine("				Ang += " + instanceName + "_CSHL_ROTARY_SLIDER_A90*4;");
                writer.WriteLine("			}");
                writer.WriteLine("			else");
                writer.WriteLine("			{");
                writer.WriteLine("				Y = -Tmp;");
                writer.WriteLine("				Ang -= " + instanceName + "_CSHL_ROTARY_SLIDER_A90*4;");
                writer.WriteLine("			}");
                writer.WriteLine("		");
                writer.WriteLine("			for (i=0; i<9; i++)");
                writer.WriteLine("			{");
                writer.WriteLine("				if (Y == 0)");
                writer.WriteLine("				{");
                writer.WriteLine("					break;");
                writer.WriteLine("				}");
                writer.WriteLine("		");
                writer.WriteLine("				TempCalc = (Y >> i);		");
                writer.WriteLine("				Tmp = (X >> i);");
                writer.WriteLine("				");
                writer.WriteLine("				if (Y < 0)");
                writer.WriteLine("				{");
                writer.WriteLine("					X -= TempCalc;");
                writer.WriteLine("					Y += Tmp;");
                writer.WriteLine("					Ang += " + instanceName + "_CSHL_RotarySliderAngle[i];");
                writer.WriteLine("				}");
                writer.WriteLine("				else");
                writer.WriteLine("				{");
                writer.WriteLine("					X += TempCalc;");
                writer.WriteLine("					Y -= Tmp;");
                writer.WriteLine("					Ang -= " + instanceName + "_CSHL_RotarySliderAngle[i];");
                writer.WriteLine("				}");
                writer.WriteLine("			}");
                writer.WriteLine("		");
                writer.WriteLine("			Ang = (" + instanceName + "_CSHL_ROTARY_SLIDER_A360*4 - Ang)/4;");
                writer.WriteLine("		}");
                writer.WriteLine("	}");
                writer.WriteLine("	");
                writer.WriteLine("	return Ang;");
                writer.WriteLine("}");
                writer.WriteLine("");
                #endregion

                #region CSHL_RotarySlider_GetValue
                writer.WriteLine(" /*-----------------------------------------------------------------------------");
                writer.WriteLine(" * FUNCTION NAME: uint16 " + instanceName + "_CSHL_RotarySlider_GetValue(const uint8 ZeroOffset, ");
                writer.WriteLine(" *							int SegCount, uint8 firstElem, uint8 numElem, uint8 offset, ");
                writer.WriteLine(" *							uint8 *diplex, uint8 Type)");
                writer.WriteLine(" *-----------------------------------------------------------------------------");
                writer.WriteLine(" * Summary: ");
                writer.WriteLine(" *");
                writer.WriteLine(" *");
                writer.WriteLine(" * Parameters: ");
                writer.WriteLine(" *");
                writer.WriteLine(" *");
                writer.WriteLine(" * Theory:");
                writer.WriteLine(" *  See summary");
                writer.WriteLine(" *");
                writer.WriteLine(" * Side Effects:");
                writer.WriteLine(" *  None");
                writer.WriteLine(" *");
                writer.WriteLine(" *---------------------------------------------------------------------------*/");
                writer.WriteLine("uint16 " + instanceName + "_CSHL_RotarySlider_GetValue(const uint8 ZeroOffset, int SegCount, uint8 firstElem, uint8 numElem, uint8 offset, uint8 *diplex, uint8 Type)");
                writer.WriteLine("{");
                writer.WriteLine("	uint8 i, pos, Nsin, Ncos;");
                writer.WriteLine("	int32 Xc, Yc, Temp;");
                writer.WriteLine("	uint16 TempAngle, DGrad, Angle;");
                writer.WriteLine("	");
                writer.WriteLine("	Xc = 0;");
                writer.WriteLine("	Yc = 0;	");
                writer.WriteLine("	DGrad = 360 / SegCount;  /* Angle step */");
                writer.WriteLine("	Angle = (360 * firstElem / SegCount);");
                writer.WriteLine("	");
                writer.WriteLine("	for(i = firstElem; i  < firstElem + numElem; i++)			/* Loop through every slot segment */");
                writer.WriteLine("	{");
                writer.WriteLine("		if (Type & " + instanceName + "_CSHL_IS_DIPLEX)");
                writer.WriteLine("		{");
                writer.WriteLine("			pos = diplex[i%SegCount];");
                writer.WriteLine("		}");
                writer.WriteLine("		else ");
                writer.WriteLine("		{");
                writer.WriteLine("			pos = i%SegCount;");
                writer.WriteLine("		}");
                writer.WriteLine("");
                writer.WriteLine("		if (" + instanceName + "_CSHL_SlotSignal[offset + pos] > 0)");
                writer.WriteLine("		{");
                writer.WriteLine("			TempAngle = Angle ;								/* Real angle */");
                writer.WriteLine("			if (TempAngle > " + instanceName + "_CSHL_ROTARY_SLIDER_A360) TempAngle -= " + instanceName + "_CSHL_ROTARY_SLIDER_A360;");
                writer.WriteLine("			if (TempAngle > " + instanceName + "_CSHL_ROTARY_SLIDER_A270)				/* if angle is between 270...360 then SIN = + ; COS = - */");
                writer.WriteLine("			{");
                writer.WriteLine("				Nsin = 1;");
                writer.WriteLine("				Ncos = 0;");
                writer.WriteLine("				TempAngle = " + instanceName + "_CSHL_ROTARY_SLIDER_A360 - TempAngle;");
                writer.WriteLine("			}");
                writer.WriteLine("			else");
                writer.WriteLine("			{");
                writer.WriteLine("				if(TempAngle > " + instanceName + "_CSHL_ROTARY_SLIDER_A180)			/* If angle is between 180...270 then SIN = + ; COS = + */");
                writer.WriteLine("				{");
                writer.WriteLine("					Nsin = 1;");
                writer.WriteLine("					Ncos = 1;");
                writer.WriteLine("					TempAngle -= " + instanceName + "_CSHL_ROTARY_SLIDER_A180;		");
                writer.WriteLine("				}");
                writer.WriteLine("				else");
                writer.WriteLine("				{");
                writer.WriteLine("					Nsin = 0;");
                writer.WriteLine("					if(TempAngle > " + instanceName + "_CSHL_ROTARY_SLIDER_A90)		/* If angle is between 90...180 then SIN = - ; COS = + */");
                writer.WriteLine("					{");
                writer.WriteLine("						Ncos = 1;");
                writer.WriteLine("						TempAngle = " + instanceName + "_CSHL_ROTARY_SLIDER_A180 - TempAngle;");
                writer.WriteLine("					}");
                writer.WriteLine("					else						/* If angle is between 0...90 then SIN = - ; COS = - */");
                writer.WriteLine("					{");
                writer.WriteLine("						Ncos = 0;	");
                writer.WriteLine("					}");
                writer.WriteLine("				}	");
                writer.WriteLine("			}		");
                writer.WriteLine("			TempAngle = TempAngle>>1;");
                writer.WriteLine("			Temp = ((int32) " + instanceName + "_CSHL_SlotSignal[offset + pos]) * ((int32) " + instanceName + "_CSHL_RotarySliderAngleSin[TempAngle]);");
                writer.WriteLine("			if (Nsin)						/* Calculate vertical  divider */");
                writer.WriteLine("			{");
                writer.WriteLine("				Yc -= Temp;");
                writer.WriteLine("			}");
                writer.WriteLine("			else");
                writer.WriteLine("			{");
                writer.WriteLine("				Yc += Temp;");
                writer.WriteLine("			}");
                writer.WriteLine("				");
                writer.WriteLine("			Temp = ((int32) " + instanceName + "_CSHL_SlotSignal[offset + pos]) * ((int32) " + instanceName + "_CSHL_RotarySliderAngleSin[45 - TempAngle]);");
                writer.WriteLine("			if (Ncos)						/* Calculate horisontal divider */");
                writer.WriteLine("			{");
                writer.WriteLine("				Xc -= Temp;");
                writer.WriteLine("			}");
                writer.WriteLine("			else");
                writer.WriteLine("			{");
                writer.WriteLine("				Xc += Temp;");
                writer.WriteLine("			}");
                writer.WriteLine("		}");
                writer.WriteLine("				");
                writer.WriteLine("		Angle += DGrad; 						/* Step to next angle */");
                writer.WriteLine("	}	");
                writer.WriteLine("	");
                writer.WriteLine("	while ((Xc > 0x00003FFF) || (Yc > 0x00003FFF))	/* Convert long to integer tape */");
                writer.WriteLine("	{");
                writer.WriteLine("		Xc >>= 1;");
                writer.WriteLine("		Yc >>= 1;");
                writer.WriteLine("	}");
                writer.WriteLine("	while ((Xc < ~0x00003FFF) || (Yc < ~0x00003FFF))	/* Convert long to integer tape */");
                writer.WriteLine("	{");
                writer.WriteLine("		Xc >>= 1;");
                writer.WriteLine("		Yc >>= 1;");
                writer.WriteLine("	}");
                writer.WriteLine("	");
                writer.WriteLine("	Angle = 0;");
                writer.WriteLine("");
                writer.WriteLine("	if ((Xc != 0) || (Yc != 0))				/* If any slot active then calculate the angle */");
                writer.WriteLine("	{");
                writer.WriteLine("		Angle = " + instanceName + "_CSHL_RotarySliderArcTan2( (int32) Xc, (int32) Yc);");
                writer.WriteLine("		");
                writer.WriteLine("		Angle += ZeroOffset;");
                writer.WriteLine("		if (Angle > 360) Angle -= 360;");
                writer.WriteLine("	}");
                writer.WriteLine("	");
                writer.WriteLine("	return Angle;");
                writer.WriteLine("}");
                writer.WriteLine("");
                #endregion

                #region CSHL_GetRadialCentroidPos
                writer.WriteLine("/*-----------------------------------------------------------------------------");
                writer.WriteLine(" * FUNCTION NAME: uint16 " + instanceName + "_CSHL_GetRadialCentroidPos(uint8 widget)");
                writer.WriteLine(" *-----------------------------------------------------------------------------");
                writer.WriteLine(" * Summary: ");
                writer.WriteLine(" *");
                writer.WriteLine(" *");
                writer.WriteLine(" * Parameters: ");
                writer.WriteLine(" *");
                writer.WriteLine(" *");
                writer.WriteLine(" * Theory:");
                writer.WriteLine(" *  See summary");
                writer.WriteLine(" *");
                writer.WriteLine(" * Side Effects:");
                writer.WriteLine(" *  None");
                writer.WriteLine(" *");
                writer.WriteLine(" *---------------------------------------------------------------------------*/");
                writer.WriteLine(" uint16 " + instanceName + "_CSHL_GetRadialCentroidPos(uint8 widget)");
                writer.WriteLine("{");
                writer.WriteLine("	"+instanceName+"_SlSettings *ptr = ("+instanceName+"_SlSettings *) " + instanceName + "_CSHL_WidgetTable[widget].AdvancedSettings;");
                writer.WriteLine("	uint8 offset = " + instanceName + "_CSHL_WidgetTable[widget].RawOffset;");
                writer.WriteLine("	uint8 *diplex = ptr->DiplexTable;");
                writer.WriteLine("	uint16 Angle, Position;");
                writer.WriteLine("	uint8 signalsNumberX2, firstSns, firstMaxSns, maxLength, isCont, posPrev, pos, posNext, slot;");
                writer.WriteLine("	uint8 i;");
                writer.WriteLine("	uint8 startNum, endNum;");
                writer.WriteLine("	uint8 localMaxValue;");
                writer.WriteLine("	uint16 BasePos;");
                writer.WriteLine("");
                writer.WriteLine("	if (" + instanceName + "_CSHL_WidgetTable[widget].Type & " + instanceName + "_CSHL_TYPE_RADIAL_SLIDER)");
                writer.WriteLine("	{");
                writer.WriteLine("		/***********************************************");
                writer.WriteLine("		");
                writer.WriteLine("			CENTROID CALCULATIONS");
                writer.WriteLine("		");
                writer.WriteLine("		**************************************************/");
                writer.WriteLine("		if(" + instanceName + "_CSHL_WidgetTable[widget].Type & " + instanceName + "_CSHL_IS_DIPLEX)");
                writer.WriteLine("		{");
                writer.WriteLine("			signalsNumberX2 = " + instanceName + "_CSHL_WidgetTable[widget].ScanSlotCount * 2;");
                writer.WriteLine("		}");
                writer.WriteLine("		else");
                writer.WriteLine("		{");
                writer.WriteLine("			signalsNumberX2 = " + instanceName + "_CSHL_WidgetTable[widget].ScanSlotCount;");
                writer.WriteLine("		}");
                writer.WriteLine("		");
                writer.WriteLine("		firstSns = 0;");
                writer.WriteLine("		firstMaxSns = 0;");
                writer.WriteLine("		maxLength = 0;");
                writer.WriteLine("		isCont = 0;");
                writer.WriteLine("		localMaxValue = 0;");
                writer.WriteLine("		");
                writer.WriteLine("		/* Find interval */");
                writer.WriteLine("		for (i = 0; i < signalsNumberX2; i++)");
                writer.WriteLine("		{");
                writer.WriteLine("			if(" + instanceName + "_CSHL_WidgetTable[widget].Type & " + instanceName + "_CSHL_IS_DIPLEX)");
                writer.WriteLine("			{");
                writer.WriteLine("				pos = diplex[i];");
                writer.WriteLine("			}");
                writer.WriteLine("			else");
                writer.WriteLine("			{");
                writer.WriteLine("				pos = i;");
                writer.WriteLine("			}");
                writer.WriteLine("");
                writer.WriteLine("			if (" + instanceName + "_CSHL_SlotSignal[offset + pos] > 0)");
                writer.WriteLine("			{");
                writer.WriteLine("				if (!isCont)");
                writer.WriteLine("				{");
                writer.WriteLine("					firstSns = i;");
                writer.WriteLine("					isCont = 1;");
                writer.WriteLine("				}");
                writer.WriteLine("				// If it is the last element");
                writer.WriteLine("				else if (i == signalsNumberX2 - 1)");
                writer.WriteLine("				{");
                writer.WriteLine("					isCont = 0;");
                writer.WriteLine("					if (i - firstSns + 1 >= maxLength)");
                writer.WriteLine("					{");
                writer.WriteLine("						maxLength = i - firstSns + 1;");
                writer.WriteLine("						firstMaxSns = firstSns;");
                writer.WriteLine("					}");
                writer.WriteLine("				}");
                writer.WriteLine("			}");
                writer.WriteLine("			else");
                writer.WriteLine("			{");
                writer.WriteLine("				if (isCont)");
                writer.WriteLine("				{");
                writer.WriteLine("					isCont = 0;");
                writer.WriteLine("					if (i - firstSns >= maxLength)");
                writer.WriteLine("					{");
                writer.WriteLine("						maxLength = i - firstSns;");
                writer.WriteLine("						firstMaxSns = firstSns;");
                writer.WriteLine("					}");
                writer.WriteLine("				}");
                writer.WriteLine("			}");
                writer.WriteLine("		}");
                writer.WriteLine("		");
                writer.WriteLine("		/* Check borders for max interval */");
                writer.WriteLine("		if (" + instanceName + "_CSHL_WidgetTable[widget].Type & " + instanceName + "_CSHL_IS_DIPLEX)");
                writer.WriteLine("		{");
                writer.WriteLine("			pos = diplex[signalsNumberX2-1];");
                writer.WriteLine("		}");
                writer.WriteLine("		else");
                writer.WriteLine("		{");
                writer.WriteLine("			pos = signalsNumberX2-1;");
                writer.WriteLine("		}");
                writer.WriteLine("		");
                writer.WriteLine("		if ((" + instanceName + "_CSHL_SlotSignal[offset + 0] > 0) && (" + instanceName + "_CSHL_SlotSignal[offset + pos] > 0))");
                writer.WriteLine("		{");
                writer.WriteLine("			startNum = 0;");
                writer.WriteLine("			endNum = 0;");
                writer.WriteLine("			");
                writer.WriteLine("			if (" + instanceName + "_CSHL_WidgetTable[widget].Type & " + instanceName + "_CSHL_IS_DIPLEX)");
                writer.WriteLine("			{");
                writer.WriteLine("				pos = diplex[startNum];");
                writer.WriteLine("			}");
                writer.WriteLine("			else ");
                writer.WriteLine("			{");
                writer.WriteLine("				pos = startNum;");
                writer.WriteLine("			}");
                writer.WriteLine("			");
                writer.WriteLine("			while (" + instanceName + "_CSHL_SlotSignal[offset + pos] > 0)");
                writer.WriteLine("			{");
                writer.WriteLine("				startNum++;");
                writer.WriteLine("				if (" + instanceName + "_CSHL_WidgetTable[widget].Type & " + instanceName + "_CSHL_IS_DIPLEX)");
                writer.WriteLine("					pos = diplex[startNum];");
                writer.WriteLine("				else ");
                writer.WriteLine("					pos = startNum;");
                writer.WriteLine("			}");
                writer.WriteLine("");
                writer.WriteLine("			if (" + instanceName + "_CSHL_WidgetTable[widget].Type & " + instanceName + "_CSHL_IS_DIPLEX)");
                writer.WriteLine("			{");
                writer.WriteLine("				pos = diplex[signalsNumberX2 - 1 - endNum];");
                writer.WriteLine("			}");
                writer.WriteLine("			else ");
                writer.WriteLine("			{");
                writer.WriteLine("				pos = signalsNumberX2 - 1 - endNum;");
                writer.WriteLine("			}");
                writer.WriteLine("");
                writer.WriteLine("			while (" + instanceName + "_CSHL_SlotSignal[offset + pos] > 0)");
                writer.WriteLine("			{");
                writer.WriteLine("				endNum++;");
                writer.WriteLine("				if (" + instanceName + "_CSHL_WidgetTable[widget].Type & " + instanceName + "_CSHL_IS_DIPLEX)");
                writer.WriteLine("				{");
                writer.WriteLine("					pos = diplex[signalsNumberX2 - 1 - endNum];");
                writer.WriteLine("				}");
                writer.WriteLine("				else ");
                writer.WriteLine("				{");
                writer.WriteLine("					pos = signalsNumberX2 - 1 - endNum;");
                writer.WriteLine("				}");
                writer.WriteLine("			}");
                writer.WriteLine("			");
                writer.WriteLine("			if (startNum + endNum > maxLength)");
                writer.WriteLine("			{");
                writer.WriteLine("				maxLength = startNum + endNum;");
                writer.WriteLine("				firstMaxSns = signalsNumberX2 - endNum;");
                writer.WriteLine("			}");
                writer.WriteLine("		}");
                writer.WriteLine("");
                writer.WriteLine("		if (maxLength > 1) /* If there is positive interval */");
                writer.WriteLine("		{");
                writer.WriteLine("			/* Find local max */");
                writer.WriteLine("			/* Left side */");
                writer.WriteLine("			if (" + instanceName + "_CSHL_WidgetTable[widget].Type & " + instanceName + "_CSHL_IS_DIPLEX)");
                writer.WriteLine("			{");
                writer.WriteLine("				pos = diplex[firstMaxSns];");
                writer.WriteLine("				posNext = diplex[(firstMaxSns+1)%signalsNumberX2];");
                writer.WriteLine("			}");
                writer.WriteLine("			else ");
                writer.WriteLine("			{");
                writer.WriteLine("				pos = firstMaxSns;");
                writer.WriteLine("				posNext = (firstMaxSns+1)%signalsNumberX2;");
                writer.WriteLine("			}");
                writer.WriteLine("");
                writer.WriteLine("			if ((" + instanceName + "_CSHL_SlotSignal[offset + pos] > " + instanceName + "_CSHL_WidgetTable[widget].FingerThreshold) &&");
                writer.WriteLine("				(" + instanceName + "_CSHL_SlotSignal[offset + pos] > " + instanceName + "_CSHL_SlotSignal[offset + posNext]) )");
                writer.WriteLine("			{");
                writer.WriteLine("				localMaxValue = " + instanceName + "_CSHL_SlotSignal[offset + pos];");
                writer.WriteLine("				slot = firstMaxSns;");
                writer.WriteLine("			}");
                writer.WriteLine("			else ");
                writer.WriteLine("			{");
                writer.WriteLine("				if ((" + instanceName + "_CSHL_SlotSignal[offset + pos] > " + instanceName + "_CSHL_WidgetTable[widget].FingerThreshold) &&");
                writer.WriteLine("					(" + instanceName + "_CSHL_SlotSignal[offset + pos] == " + instanceName + "_CSHL_SlotSignal[offset + posNext]))");
                writer.WriteLine("				{");
                writer.WriteLine("					if(" + instanceName + "_CSHL_SlotBaselineLow[offset + pos] < " + instanceName + "_CSHL_SlotBaselineLow[offset + posNext])");
                writer.WriteLine("					{");
                writer.WriteLine("						localMaxValue = " + instanceName + "_CSHL_SlotSignal[offset + pos];");
                writer.WriteLine("						slot = firstMaxSns;");
                writer.WriteLine("					}");
                writer.WriteLine("				}");
                writer.WriteLine("			}");
                writer.WriteLine("			");
                writer.WriteLine("			/* Right side */");
                writer.WriteLine("			if (" + instanceName + "_CSHL_WidgetTable[widget].Type & " + instanceName + "_CSHL_IS_DIPLEX)");
                writer.WriteLine("			{");
                writer.WriteLine("				pos = diplex[(firstMaxSns + maxLength-1)%signalsNumberX2];");
                writer.WriteLine("				posNext = diplex[(firstMaxSns + maxLength-2)%signalsNumberX2];");
                writer.WriteLine("			}");
                writer.WriteLine("			else ");
                writer.WriteLine("			{");
                writer.WriteLine("				pos = (firstMaxSns + maxLength-1)%signalsNumberX2;");
                writer.WriteLine("				posNext = (firstMaxSns + maxLength-2)%signalsNumberX2;");
                writer.WriteLine("			}");
                writer.WriteLine("");
                writer.WriteLine("			if ((" + instanceName + "_CSHL_SlotSignal[offset + pos] > " + instanceName + "_CSHL_WidgetTable[widget].FingerThreshold) &&");
                writer.WriteLine("				(" + instanceName + "_CSHL_SlotSignal[offset + pos] > " + instanceName + "_CSHL_SlotSignal[offset + posNext]) )");
                writer.WriteLine("			{");
                writer.WriteLine("				localMaxValue = " + instanceName + "_CSHL_SlotSignal[offset +pos];");
                writer.WriteLine("				slot = (firstMaxSns + maxLength - 1)%signalsNumberX2;");
                writer.WriteLine("			}");
                writer.WriteLine("			else ");
                writer.WriteLine("			{	");
                writer.WriteLine("				if ((" + instanceName + "_CSHL_SlotSignal[offset + pos] > " + instanceName + "_CSHL_WidgetTable[widget].FingerThreshold) &&");
                writer.WriteLine("					(" + instanceName + "_CSHL_SlotSignal[offset + pos] == " + instanceName + "_CSHL_SlotSignal[offset + posNext]) )");
                writer.WriteLine("				{");
                writer.WriteLine("					if (" + instanceName + "_CSHL_SlotBaselineLow[offset + pos] < " + instanceName + "_CSHL_SlotBaselineLow[offset + posNext])");
                writer.WriteLine("					{");
                writer.WriteLine("						localMaxValue = " + instanceName + "_CSHL_SlotSignal[offset +pos];");
                writer.WriteLine("						slot = (firstMaxSns + maxLength - 1)%signalsNumberX2;");
                writer.WriteLine("					}");
                writer.WriteLine("				}");
                writer.WriteLine("				else");
                writer.WriteLine("				{");
                writer.WriteLine("					/* Inside */");
                writer.WriteLine("					for (i = firstMaxSns + 1; i < firstMaxSns + maxLength - 1; i++)");
                writer.WriteLine("					{");
                writer.WriteLine("						if (" + instanceName + "_CSHL_WidgetTable[widget].Type & " + instanceName + "_CSHL_IS_DIPLEX)");
                writer.WriteLine("						{");
                writer.WriteLine("							pos = diplex[i%signalsNumberX2];");
                writer.WriteLine("							posPrev = diplex[(i-1)%signalsNumberX2];");
                writer.WriteLine("							posNext = diplex[(i+1)%signalsNumberX2];");
                writer.WriteLine("						}");
                writer.WriteLine("						else ");
                writer.WriteLine("						{");
                writer.WriteLine("							pos = i%signalsNumberX2;");
                writer.WriteLine("							posPrev = (i-1)%signalsNumberX2;");
                writer.WriteLine("							posNext = (i+1)%signalsNumberX2;");
                writer.WriteLine("						}");
                writer.WriteLine("						");
                writer.WriteLine("						if (" + instanceName + "_CSHL_SlotSignal[offset + pos] > " + instanceName + "_CSHL_WidgetTable[widget].FingerThreshold)");
                writer.WriteLine("						{");
                writer.WriteLine("							if ((" + instanceName + "_CSHL_SlotSignal[offset + pos] > " + instanceName + "_CSHL_SlotSignal[offset + posPrev]) &&");
                writer.WriteLine("								(" + instanceName + "_CSHL_SlotSignal[offset + pos] > " + instanceName + "_CSHL_SlotSignal[offset +posNext]) )");
                writer.WriteLine("							{");
                writer.WriteLine("								localMaxValue = " + instanceName + "_CSHL_SlotSignal[offset +pos];");
                writer.WriteLine("								slot = i%signalsNumberX2;");
                writer.WriteLine("								break;");
                writer.WriteLine("							}");
                writer.WriteLine("							else ");
                writer.WriteLine("							{	");
                writer.WriteLine("								if ((" + instanceName + "_CSHL_SlotSignal[offset + pos] > " + instanceName + "_CSHL_SlotSignal[offset + posPrev]) &&");
                writer.WriteLine("									(" + instanceName + "_CSHL_SlotSignal[offset + pos] == " + instanceName + "_CSHL_SlotSignal[offset + posNext]))");
                writer.WriteLine("								{");
                writer.WriteLine("									if(" + instanceName + "_CSHL_SlotBaselineLow[offset + pos] < " + instanceName + "_CSHL_SlotBaselineLow[offset + posNext])");
                writer.WriteLine("									{");
                writer.WriteLine("										localMaxValue = " + instanceName + "_CSHL_SlotSignal[offset +pos];");
                writer.WriteLine("										slot = i%signalsNumberX2;");
                writer.WriteLine("										break;");
                writer.WriteLine("									}");
                writer.WriteLine("								}");
                writer.WriteLine("								else ");
                writer.WriteLine("								{");
                writer.WriteLine("									if ((" + instanceName + "_CSHL_SlotSignal[offset + pos] == " + instanceName + "_CSHL_SlotSignal[offset + posPrev]) &&");
                writer.WriteLine("										(" + instanceName + "_CSHL_SlotSignal[offset + pos] > " + instanceName + "_CSHL_SlotSignal[offset + posNext]))");
                writer.WriteLine("									{");
                writer.WriteLine("										if(" + instanceName + "_CSHL_SlotBaselineLow[offset + pos] < " + instanceName + "_CSHL_SlotBaselineLow[offset + posPrev])");
                writer.WriteLine("										{");
                writer.WriteLine("											localMaxValue = " + instanceName + "_CSHL_SlotSignal[offset +pos];");
                writer.WriteLine("											slot = i%signalsNumberX2;");
                writer.WriteLine("											break;");
                writer.WriteLine("										}");
                writer.WriteLine("									}");
                writer.WriteLine("									else");
                writer.WriteLine("									{");
                writer.WriteLine("										if ((" + instanceName + "_CSHL_SlotSignal[offset + pos] == " + instanceName + "_CSHL_SlotSignal[offset + posPrev]) &&");
                writer.WriteLine("											(" + instanceName + "_CSHL_SlotSignal[offset + pos] == " + instanceName + "_CSHL_SlotSignal[offset + posNext]))");
                writer.WriteLine("										{");
                writer.WriteLine("											if((" + instanceName + "_CSHL_SlotBaselineLow[offset + pos] < " + instanceName + "_CSHL_SlotBaselineLow[offset + posPrev]) && ");
                writer.WriteLine("											(" + instanceName + "_CSHL_SlotBaselineLow[offset + pos] < " + instanceName + "_CSHL_SlotBaselineLow[offset + posNext]))");
                writer.WriteLine("											{");
                writer.WriteLine("												localMaxValue = " + instanceName + "_CSHL_SlotSignal[offset +pos];");
                writer.WriteLine("												slot = i%signalsNumberX2;");
                writer.WriteLine("												break;");
                writer.WriteLine("											}");
                writer.WriteLine("										}");
                writer.WriteLine("									}");
                writer.WriteLine("								}");
                writer.WriteLine("							}");
                writer.WriteLine("						}");
                writer.WriteLine("					}");
                writer.WriteLine("");
                writer.WriteLine("				}");
                writer.WriteLine("			}");
                writer.WriteLine("");
                writer.WriteLine("");
                writer.WriteLine("			/* Find angle, position */");
                writer.WriteLine("			if (localMaxValue > 0)");
                writer.WriteLine("			{");
                writer.WriteLine("				/* Calculate angle */");
                writer.WriteLine("				Angle = " + instanceName + "_CSHL_RotarySlider_GetValue(0, signalsNumberX2, firstMaxSns, maxLength, offset, diplex, " + instanceName + "_CSHL_WidgetTable[widget].Type);");
                writer.WriteLine("				");
                writer.WriteLine("				/* Calculate position */");
                writer.WriteLine("				Position = Angle * ptr->Resolution / 360;");
                writer.WriteLine("");
                writer.WriteLine("		");
                writer.WriteLine("				/*****************************");
                writer.WriteLine("					FILTERING");
                writer.WriteLine("				******************************/");
                writer.WriteLine("				if (ptr->FirstTime == 0)				/* Initialize the filters */");
                writer.WriteLine("				{");
                writer.WriteLine("					/* Jitter filter first time initialization */");
                writer.WriteLine("					if(" + instanceName + "_CSHL_WidgetTable[widget].Filters & " + instanceName + "_CSHL_POS_JITTER_FILTER)");
                writer.WriteLine("					{");
                writer.WriteLine("						ptr->PosJitter = Position;");
                writer.WriteLine("					}");
                writer.WriteLine("					");
                writer.WriteLine("					/* Median filter first time initialization */");
                writer.WriteLine("					if(" + instanceName + "_CSHL_WidgetTable[widget].Filters & " + instanceName + "_CSHL_POS_MEDIAN_FILTER)");
                writer.WriteLine("					{");
                writer.WriteLine("						ptr->Pos2Median = Position;");
                writer.WriteLine("						ptr->Pos1Median = Position;");
                writer.WriteLine("					}");
                writer.WriteLine("					");
                writer.WriteLine("					/* Averaging filter first time initialization */");
                writer.WriteLine("					if(" + instanceName + "_CSHL_WidgetTable[widget].Filters & " + instanceName + "_CSHL_POS_AVERAGING_FILTER)");
                writer.WriteLine("					{");
                writer.WriteLine("						ptr->Pos2Averaging = Position;");
                writer.WriteLine("						ptr->Pos1Averaging = Position;		");
                writer.WriteLine("					}");
                writer.WriteLine("					ptr->FirstTime = 1;");
                writer.WriteLine("				}");
                writer.WriteLine("				else							/* Do the filtering */");
                writer.WriteLine("				{");                
                writer.WriteLine("					/* Median filter  */");
                writer.WriteLine("					if(" + instanceName + "_CSHL_WidgetTable[widget].Filters & " + instanceName + "_CSHL_POS_MEDIAN_FILTER)");
                writer.WriteLine("					{");
                writer.WriteLine("					    BasePos = Position;");
                writer.WriteLine("						Position = " + instanceName + "_CSHL_MedianFilter(Position, ptr->Pos1Median, ptr->Pos2Median);");
                writer.WriteLine("						ptr->Pos2Median = ptr->Pos1Median;");
                writer.WriteLine("						ptr->Pos1Median = BasePos;");
                writer.WriteLine("					}");
                writer.WriteLine("					");
                writer.WriteLine("					/* Averaging filter first time initialization */");
                writer.WriteLine("					if(" + instanceName + "_CSHL_WidgetTable[widget].Filters & " + instanceName + "_CSHL_POS_AVERAGING_FILTER)");
                writer.WriteLine("					{");
                writer.WriteLine("					    BasePos = Position;");
                writer.WriteLine("						Position = " + instanceName + "_CSHL_AveragingFilter(Position, ptr->Pos1Averaging, ptr->Pos2Averaging);");
                writer.WriteLine("						ptr->Pos2Averaging = ptr->Pos1Averaging;");
                writer.WriteLine("						ptr->Pos1Averaging= BasePos;				");
                writer.WriteLine("					}");
                writer.WriteLine("					");
                writer.WriteLine("					/* Jitter filter  */");
                writer.WriteLine("					if(" + instanceName + "_CSHL_WidgetTable[widget].Filters & " + instanceName + "_CSHL_POS_JITTER_FILTER)");
                writer.WriteLine("					{");
                writer.WriteLine("						Position = " + instanceName + "_CSHL_JitterFilter(Position, ptr->PosJitter);");
                writer.WriteLine("						ptr->PosJitter = Position;");
                writer.WriteLine("					}");                
                writer.WriteLine("				}");
                writer.WriteLine("			}");
                writer.WriteLine("			else");
                writer.WriteLine("			{");
                writer.WriteLine("				/* Local max doesn't find*/");
                writer.WriteLine("				Position = 0xFFFF;");
                writer.WriteLine("			}");
                writer.WriteLine("		}");
                writer.WriteLine("		else");
                writer.WriteLine("		{");
                writer.WriteLine("			/* The Diplex Centriod Size is less than 2 */");
                writer.WriteLine("			Position = 0xFFFF;");
                writer.WriteLine("		}");
                writer.WriteLine("	}");
                writer.WriteLine("	else");
                writer.WriteLine("	{");
                writer.WriteLine("		/* This Widget isn't Linear Slider Widget */");
                writer.WriteLine("		Position = 0xFFFF;");
                writer.WriteLine("	}");
                writer.WriteLine("");
                writer.WriteLine("	return Position;");
                writer.WriteLine("}");
                writer.WriteLine("");
                #endregion
                #endregion
            }
            if (packParams.cyWidgetsList.GetCountDoubleFullWidgets()>0)
            {
                #region CSHL_GetDoubleCentroidPos
                #region CSHL_CalcCentroid
                writer.WriteLine(" /*-----------------------------------------------------------------------------");
                writer.WriteLine(" * FUNCTION NAME: uint16 " + instanceName + "_CSHL_CalcCentroid(uint8 startIndex, ");
                writer.WriteLine(" *						uint8 length, uint8 pos, uint16 Mult)");
                writer.WriteLine(" *-----------------------------------------------------------------------------");
                writer.WriteLine(" * Summary: ");
                writer.WriteLine(" *");
                writer.WriteLine(" *");
                writer.WriteLine(" * Parameters: ");
                writer.WriteLine(" *");
                writer.WriteLine(" *");
                writer.WriteLine(" * Theory:");
                writer.WriteLine(" *  See summary");
                writer.WriteLine(" *");
                writer.WriteLine(" * Side Effects:");
                writer.WriteLine(" *  None");
                writer.WriteLine(" *");
                writer.WriteLine("*---------------------------------------------------------------------------*/");
                writer.WriteLine("uint16 " + instanceName + "_CSHL_CalcCentroid(uint8 startIndex, uint8 length, uint8 pos, uint16 Mult)");
                writer.WriteLine("{");
                writer.WriteLine("	uint32 Numerator = 0;");
                writer.WriteLine("	uint32 Denominator  = 0;");
                writer.WriteLine("");
                writer.WriteLine("	if (pos == startIndex)");
                writer.WriteLine("	{");
                writer.WriteLine("		//Start of Centroid");
                writer.WriteLine("		Numerator = (uint32) " + instanceName + "_CSHL_SlotSignal[pos + 1];");
                writer.WriteLine("		Denominator  = (uint32)" + instanceName + "_CSHL_SlotSignal[pos] + (uint32)" + instanceName + "_CSHL_SlotSignal[pos + 1];");
                writer.WriteLine("	}");
                writer.WriteLine("	else if (pos == startIndex + length - 1)");
                writer.WriteLine("	{");
                writer.WriteLine("		//End of Centroid");
                writer.WriteLine("		Numerator -= ((uint32) " + instanceName + "_CSHL_SlotSignal[pos - 1]);");
                writer.WriteLine("		Denominator  = (uint32) " + instanceName + "_CSHL_SlotSignal[pos] + (uint32) " + instanceName + "_CSHL_SlotSignal[pos - 1];");
                writer.WriteLine("	}");
                writer.WriteLine("	else");
                writer.WriteLine("	{");
                writer.WriteLine("		//Not first Not last");
                writer.WriteLine("		Numerator = ((uint32) " + instanceName + "_CSHL_SlotSignal[pos + 1]) - ((uint32) " + instanceName + "_CSHL_SlotSignal[pos - 1]);");
                writer.WriteLine("		Denominator  = ((uint32) " + instanceName + "_CSHL_SlotSignal[pos - 1]) + ((uint32) " + instanceName + "_CSHL_SlotSignal[pos]) + ((uint32) " + instanceName + "_CSHL_SlotSignal[pos + 1]);");
                writer.WriteLine("	}");
                writer.WriteLine("");
                writer.WriteLine("	Denominator  = ( (uint32) ((Numerator << 8) / (int32)Denominator ) + ((pos - startIndex) << 8)) * Mult;");
                writer.WriteLine("	");
                writer.WriteLine("	return HI16(Denominator);");
                writer.WriteLine("}");
                writer.WriteLine("");
                #endregion

                #region CSHL_FindLocalMax
                writer.WriteLine("/*-----------------------------------------------------------------------------");
                writer.WriteLine(" * FUNCTION NAME: uint8 " + instanceName + "_CSHL_FindLocalMax(uint8 startIndex, ");
                writer.WriteLine(" *						uint8 length, uint8 fingerthreshould)");
                writer.WriteLine(" *-----------------------------------------------------------------------------");
                writer.WriteLine(" * Summary: ");
                writer.WriteLine(" *");
                writer.WriteLine(" *");
                writer.WriteLine(" * Parameters: ");
                writer.WriteLine(" *");
                writer.WriteLine(" *");
                writer.WriteLine(" * Theory:");
                writer.WriteLine(" *  See summary");
                writer.WriteLine(" *");
                writer.WriteLine(" * Side Effects:");
                writer.WriteLine(" *  None");
                writer.WriteLine(" *");
                writer.WriteLine("*---------------------------------------------------------------------------*/");
                writer.WriteLine("uint8 " + instanceName + "_CSHL_FindLocalMax(uint8 startIndex, uint8 length, uint8 fingerthreshould)");
                writer.WriteLine("{");
                writer.WriteLine("	uint8 localmax = 0xFFu;");
                writer.WriteLine("	uint8 i;");
                writer.WriteLine("");
                writer.WriteLine("	for(i = startIndex; i < startIndex + length; i++)");
                writer.WriteLine("	{");
                writer.WriteLine("		/* Ignore if zero */");
                writer.WriteLine("		if(" + instanceName + "_CSHL_SlotSignal[i] > fingerthreshould)");
                writer.WriteLine("		{");
                writer.WriteLine("			if(i == startIndex)");
                writer.WriteLine("			{");
                writer.WriteLine("				/* First element pos > pos1 */");
                writer.WriteLine("				if(" + instanceName + "_CSHL_SlotSignal[i] > " + instanceName + "_CSHL_SlotSignal[i+1])");
                writer.WriteLine("				{");
                writer.WriteLine("					localmax = i;");
                writer.WriteLine("					break;");
                writer.WriteLine("				}");
                writer.WriteLine("				else");
                writer.WriteLine("				{");
                writer.WriteLine("					if(" + instanceName + "_CSHL_SlotSignal[i] == " + instanceName + "_CSHL_SlotSignal[i+1])");
                writer.WriteLine("					{");
                writer.WriteLine("						/* Compare BaselinesLow (i) and (i+1) */");
                writer.WriteLine("						if(" + instanceName + "_CSHL_SlotBaselineLow[i] < " + instanceName + "_CSHL_SlotBaselineLow[i+1])");
                writer.WriteLine("						{");
                writer.WriteLine("							localmax = i;");
                writer.WriteLine("							break;");
                writer.WriteLine("						}");
                writer.WriteLine("					}");
                writer.WriteLine("				}");
                writer.WriteLine("			}");
                writer.WriteLine("			else if (i == startIndex + length - 1)");
                writer.WriteLine("			{");
                writer.WriteLine("				/* Last element i > i-1 */");
                writer.WriteLine("				if(" + instanceName + "_CSHL_SlotSignal[i] > " + instanceName + "_CSHL_SlotSignal[i-1])");
                writer.WriteLine("				{");
                writer.WriteLine("					localmax = i;");
                writer.WriteLine("					break;");
                writer.WriteLine("				}");
                writer.WriteLine("				else");
                writer.WriteLine("				{");
                writer.WriteLine("					if(" + instanceName + "_CSHL_SlotSignal[i] == " + instanceName + "_CSHL_SlotSignal[i-1])");
                writer.WriteLine("					{");
                writer.WriteLine("						/* Compare BaselinesLow (i) and (i-1) */");
                writer.WriteLine("						if(" + instanceName + "_CSHL_SlotBaselineLow[i] < " + instanceName + "_CSHL_SlotBaselineLow[i-1])");
                writer.WriteLine("						{");
                writer.WriteLine("							localmax = i;");
                writer.WriteLine("							break;");
                writer.WriteLine("						}");
                writer.WriteLine("					}	");
                writer.WriteLine("				}");
                writer.WriteLine("			}");
                writer.WriteLine("			else");
                writer.WriteLine("			{");
                writer.WriteLine("				/* point (i-1) > (i) and (i) > (i+1) */");
                writer.WriteLine("				if((" + instanceName + "_CSHL_SlotSignal[i] > " + instanceName + "_CSHL_SlotSignal[i-1]) && (" + instanceName + "_CSHL_SlotSignal[i] > " + instanceName + "_CSHL_SlotSignal[i+1]))");
                writer.WriteLine("				{");
                writer.WriteLine("					localmax = i;");
                writer.WriteLine("					break;");
                writer.WriteLine("				}");
                writer.WriteLine("				else");
                writer.WriteLine("				{");
                writer.WriteLine("					/* (i-1) == (i), compare BaselinesLow */");
                writer.WriteLine("					if(" + instanceName + "_CSHL_SlotSignal[i] == " + instanceName + "_CSHL_SlotSignal[i-1])");
                writer.WriteLine("					{");
                writer.WriteLine("						if(" + instanceName + "_CSHL_SlotBaselineLow[i] < " + instanceName + "_CSHL_SlotBaselineLow[i-1])");
                writer.WriteLine("						{");
                writer.WriteLine("							if(" + instanceName + "_CSHL_SlotSignal[i] > " + instanceName + "_CSHL_SlotSignal[i+1])");
                writer.WriteLine("							{");
                writer.WriteLine("								localmax = i;");
                writer.WriteLine("								break;");
                writer.WriteLine("							}");
                writer.WriteLine("							else");
                writer.WriteLine("							{");
                writer.WriteLine("								/* (i) == (i+1), compare BaselinesLow */ ");
                writer.WriteLine("								if((" + instanceName + "_CSHL_SlotSignal[i] == " + instanceName + "_CSHL_SlotSignal[i+1]) && (" + instanceName + "_CSHL_SlotBaselineLow[i] < " + instanceName + "_CSHL_SlotBaselineLow[i+1]))");
                writer.WriteLine("								{");
                writer.WriteLine("									localmax = i;");
                writer.WriteLine("									break;");
                writer.WriteLine("								}");
                writer.WriteLine("							}");
                writer.WriteLine("						}");
                writer.WriteLine("					}");
                writer.WriteLine("					if(" + instanceName + "_CSHL_SlotSignal[i] == " + instanceName + "_CSHL_SlotSignal[i+1])");
                writer.WriteLine("					{");
                writer.WriteLine("						if(" + instanceName + "_CSHL_SlotBaselineLow[i] < " + instanceName + "_CSHL_SlotBaselineLow[i+1])");
                writer.WriteLine("						{");
                writer.WriteLine("							if(" + instanceName + "_CSHL_SlotSignal[i] > " + instanceName + "_CSHL_SlotSignal[i-1])");
                writer.WriteLine("							{");
                writer.WriteLine("								localmax = i;");
                writer.WriteLine("								break;");
                writer.WriteLine("							}");
                writer.WriteLine("							else");
                writer.WriteLine("							{");
                writer.WriteLine("								if((" + instanceName + "_CSHL_SlotSignal[i] == " + instanceName + "_CSHL_SlotSignal[i-1]) && (" + instanceName + "_CSHL_SlotBaselineLow[i] < " + instanceName + "_CSHL_SlotBaselineLow[i-1]))");
                writer.WriteLine("								{");
                writer.WriteLine("									localmax = i;");
                writer.WriteLine("									break;");
                writer.WriteLine("								}");
                writer.WriteLine("							}");
                writer.WriteLine("						}");
                writer.WriteLine("					}");
                writer.WriteLine("				}");
                writer.WriteLine("			}");
                writer.WriteLine("		}");
                writer.WriteLine("	}");
                writer.WriteLine("	");
                writer.WriteLine("	return localmax;");
                writer.WriteLine("}");
                writer.WriteLine("");
                #endregion

                #region CSHL_GetDoubleCentroidPos
                writer.WriteLine("/*-----------------------------------------------------------------------------");
                writer.WriteLine(" * FUNCTION NAME: uint16 " + instanceName + "_CSHL_GetDoubleCentroidPos(uint8 widget)");
                writer.WriteLine(" *-----------------------------------------------------------------------------");
                writer.WriteLine(" * Summary: ");
                writer.WriteLine(" *");
                writer.WriteLine(" *");
                writer.WriteLine(" * Parameters: ");
                writer.WriteLine(" *");
                writer.WriteLine(" *");
                writer.WriteLine(" * Theory:");
                writer.WriteLine(" *  See summary");
                writer.WriteLine(" *");
                writer.WriteLine(" * Side Effects:");
                writer.WriteLine(" *  None");
                writer.WriteLine(" *");
                writer.WriteLine(" *---------------------------------------------------------------------------*/");
                writer.WriteLine("uint16 " + instanceName + "_CSHL_GetDoubleCentroidPos(uint8 widget)");
                writer.WriteLine("{");
                writer.WriteLine("	"+instanceName+"_TPSettings *ptrX;");
                writer.WriteLine("	"+instanceName+"_TPSettings *ptrY;");
                writer.WriteLine("	uint16 PosX, PosY;");
                writer.WriteLine("	uint8 i, Touch;");
                writer.WriteLine("	uint16 BasePos;");
                writer.WriteLine("		");
                writer.WriteLine("	if (" + instanceName + "_CSHL_WidgetTable[widget].Type == " + instanceName + "_CSHL_TYPE_TOUCHPAD)");
                writer.WriteLine("	{");
                writer.WriteLine("		/* Find if Col or Row we have as widget */");
                writer.WriteLine("		if (widget > " + instanceName + "_CSHL_NUMBER_OF_DOUBLE_STRUCTS)");
                writer.WriteLine("		{");
                writer.WriteLine("			/* Always find Col widget */");
                writer.WriteLine("			widget -= " + instanceName + "_CSHL_NUMBER_OF_DOUBLE_STRUCTS;");
                writer.WriteLine("		}");
                writer.WriteLine("");
                writer.WriteLine("		ptrX = ("+instanceName+"_TPSettings *) " + instanceName + "_CSHL_WidgetTable[widget].AdvancedSettings;");
                writer.WriteLine("		ptrY = ("+instanceName+"_TPSettings *) " + instanceName + "_CSHL_WidgetTable[widget + " + instanceName + "_CSHL_NUMBER_OF_DOUBLE_STRUCTS].AdvancedSettings;");
                writer.WriteLine("");
                writer.WriteLine("		/***********************************************");
                writer.WriteLine("					CENTROID CALCULATIONS");
                writer.WriteLine("		**************************************************/");
                writer.WriteLine("		");
                writer.WriteLine("		/* Find local maximum */");
                writer.WriteLine("		PosX = " + instanceName + "_CSHL_FindLocalMax(" + instanceName + "_CSHL_WidgetTable[widget].RawOffset,");
                writer.WriteLine("												" + instanceName + "_CSHL_WidgetTable[widget].ScanSlotCount,");
                writer.WriteLine("												" + instanceName + "_CSHL_WidgetTable[widget].FingerThreshold);");
                writer.WriteLine("		");
                writer.WriteLine("		PosY = " + instanceName + "_CSHL_FindLocalMax(" + instanceName + "_CSHL_WidgetTable[widget+" + instanceName + "_CSHL_NUMBER_OF_DOUBLE_STRUCTS].RawOffset,");
                writer.WriteLine("												" + instanceName + "_CSHL_WidgetTable[widget + " + instanceName + "_CSHL_NUMBER_OF_DOUBLE_STRUCTS].ScanSlotCount,");
                writer.WriteLine("												" + instanceName + "_CSHL_WidgetTable[widget+" + instanceName + "_CSHL_NUMBER_OF_DOUBLE_STRUCTS].FingerThreshold);");
                writer.WriteLine("		");
                writer.WriteLine("		/* Calculate centroid */");
                writer.WriteLine("		if ((PosX < 0xFFu) && (PosY < 0xFFu))");
                writer.WriteLine("		{");
                writer.WriteLine("			for(i = 0; i < " + instanceName + "_CSHL_MAX_FINGERS; i++)");
                writer.WriteLine("			{");
                writer.WriteLine("				PosX = " + instanceName + "_CSHL_CalcCentroid(" + instanceName + "_CSHL_WidgetTable[widget].RawOffset,");
                writer.WriteLine("													" + instanceName + "_CSHL_WidgetTable[widget].ScanSlotCount, ");
                writer.WriteLine("													PosX, ");
                writer.WriteLine("													ptrX->Resolution);");
                writer.WriteLine("													");
                writer.WriteLine("				PosY = " + instanceName + "_CSHL_CalcCentroid(" + instanceName + "_CSHL_WidgetTable[widget+" + instanceName + "_CSHL_NUMBER_OF_DOUBLE_STRUCTS].RawOffset, ");
                writer.WriteLine("													" + instanceName + "_CSHL_WidgetTable[widget+" + instanceName + "_CSHL_NUMBER_OF_DOUBLE_STRUCTS].ScanSlotCount,");
                writer.WriteLine("													PosY, ");
                writer.WriteLine("													ptrY->Resolution);");
                writer.WriteLine("");
                writer.WriteLine("				/*****************************");
                writer.WriteLine("					FILTERING X and Y");
                writer.WriteLine("				******************************/");
                writer.WriteLine("				if (ptrX->FirstTime == 0)				/* Initialize the filters */");
                writer.WriteLine("				{");
                writer.WriteLine("					/* Jitter filter first time initialization */");
                writer.WriteLine("					if(" + instanceName + "_CSHL_WidgetTable[widget].Filters & " + instanceName + "_CSHL_POS_JITTER_FILTER)");
                writer.WriteLine("					{");
                writer.WriteLine("						ptrX->PosJitter[i] = PosX;");
                writer.WriteLine("					}");
                writer.WriteLine("					");
                writer.WriteLine("					/* Median filter first time initialization */");
                writer.WriteLine("					if(" + instanceName + "_CSHL_WidgetTable[widget].Filters & " + instanceName + "_CSHL_POS_MEDIAN_FILTER)");
                writer.WriteLine("					{");
                writer.WriteLine("						ptrX->Pos2Median[i] = PosX;");
                writer.WriteLine("						ptrX->Pos1Median[i] = PosX;");
                writer.WriteLine("					}");
                writer.WriteLine("					");
                writer.WriteLine("					/* Averaging filter first time initialization */");
                writer.WriteLine("					if(" + instanceName + "_CSHL_WidgetTable[widget].Filters & " + instanceName + "_CSHL_POS_AVERAGING_FILTER)");
                writer.WriteLine("					{");
                writer.WriteLine("						ptrX->Pos2Averaging[i] = PosX;");
                writer.WriteLine("						ptrX->Pos1Averaging[i] = PosX;");
                writer.WriteLine("					}");
                writer.WriteLine("					ptrX->FirstTime = 1;");
                writer.WriteLine("				}");
                writer.WriteLine("				else							/* Do the filtering */");
                writer.WriteLine("				{");
                writer.WriteLine("					/* Median filter */");
                writer.WriteLine("					if(" + instanceName + "_CSHL_WidgetTable[widget].Filters & " + instanceName + "_CSHL_POS_MEDIAN_FILTER)");
                writer.WriteLine("					{");
                writer.WriteLine("					    BasePos = PosX;");
                writer.WriteLine("						PosX = " + instanceName + "_CSHL_MedianFilter(PosX, ptrX->Pos1Median[i], ptrX->Pos2Median[i]);");
                writer.WriteLine("						ptrX->Pos2Median[i] = ptrX->Pos1Median[i];");
                writer.WriteLine("						ptrX->Pos1Median[i] = BasePos;");
                writer.WriteLine("					}");
                writer.WriteLine("					");
                writer.WriteLine("					/* Averaging filter first time initialization */");
                writer.WriteLine("					if(" + instanceName + "_CSHL_WidgetTable[widget].Filters & " + instanceName + "_CSHL_POS_AVERAGING_FILTER)");
                writer.WriteLine("					{");
                writer.WriteLine("					    BasePos = PosX;");
                writer.WriteLine("						PosX = " + instanceName + "_CSHL_AveragingFilter(PosX, ptrX->Pos1Averaging[i], ptrX->Pos2Averaging[i]);");
                writer.WriteLine("						ptrX->Pos2Averaging[i] = ptrX->Pos1Averaging[i];");
                writer.WriteLine("						ptrX->Pos1Averaging[i] = BasePos;");
                writer.WriteLine("					}");
                writer.WriteLine("					");
                writer.WriteLine("					/* Jitter filter */");
                writer.WriteLine("					if(" + instanceName + "_CSHL_WidgetTable[widget].Filters & " + instanceName + "_CSHL_POS_JITTER_FILTER)");
                writer.WriteLine("					{");
                writer.WriteLine("						PosX = " + instanceName + "_CSHL_JitterFilter(PosX, ptrX->PosJitter[i]);");
                writer.WriteLine("						ptrX->PosJitter[i] = PosX;");
                writer.WriteLine("					}");                
                writer.WriteLine("				}");
                writer.WriteLine("				");
                writer.WriteLine("				if (ptrY->FirstTime == 0)				/* Initialize the filters */");
                writer.WriteLine("				{");
                writer.WriteLine("					/* Jitter filter first time initialization */");
                writer.WriteLine("					if(" + instanceName + "_CSHL_WidgetTable[widget].Filters & " + instanceName + "_CSHL_POS_JITTER_FILTER)");
                writer.WriteLine("					{");
                writer.WriteLine("						ptrY->PosJitter[i] = PosY;");
                writer.WriteLine("					}");
                writer.WriteLine("					");
                writer.WriteLine("					/* Median filter first time initialization */");
                writer.WriteLine("					if(" + instanceName + "_CSHL_WidgetTable[widget].Filters & " + instanceName + "_CSHL_POS_MEDIAN_FILTER)");
                writer.WriteLine("					{");
                writer.WriteLine("						ptrY->Pos2Median[i] = PosY;");
                writer.WriteLine("						ptrY->Pos1Median[i]= PosY;");
                writer.WriteLine("					}");
                writer.WriteLine("					");
                writer.WriteLine("					/* Averaging filter first time initialization */");
                writer.WriteLine("					if(" + instanceName + "_CSHL_WidgetTable[widget].Filters & " + instanceName + "_CSHL_POS_AVERAGING_FILTER)");
                writer.WriteLine("					{");
                writer.WriteLine("						ptrY->Pos2Averaging[i] = PosY;");
                writer.WriteLine("						ptrY->Pos1Averaging[i] = PosY;		");
                writer.WriteLine("					}");
                writer.WriteLine("					ptrY->FirstTime = 1;");
                writer.WriteLine("				}");
                writer.WriteLine("				else							/* Do the filtering */");
                writer.WriteLine("				{");
                writer.WriteLine("					/* Median filter*/");
                writer.WriteLine("					if(" + instanceName + "_CSHL_WidgetTable[widget].Filters & " + instanceName + "_CSHL_POS_MEDIAN_FILTER)");
                writer.WriteLine("					{");
                writer.WriteLine("					    BasePos = PosY;");
                writer.WriteLine("						PosY = " + instanceName + "_CSHL_MedianFilter(PosY, ptrY->Pos1Median[i], ptrY->Pos2Median[i]);");
                writer.WriteLine("						ptrY->Pos2Median[i]  = ptrY->Pos1Median[i] ;");
                writer.WriteLine("						ptrY->Pos1Median[i]  = BasePos;");
                writer.WriteLine("					}");
                writer.WriteLine("					");
                writer.WriteLine("					/* Averaging filter first time initialization */");
                writer.WriteLine("					if(" + instanceName + "_CSHL_WidgetTable[widget].Filters & " + instanceName + "_CSHL_POS_AVERAGING_FILTER)");
                writer.WriteLine("					{");
                writer.WriteLine("					    BasePos = PosY;");
                writer.WriteLine("						PosY = " + instanceName + "_CSHL_AveragingFilter(PosY, ptrY->Pos1Averaging[i], ptrY->Pos2Averaging[i]);");
                writer.WriteLine("						ptrY->Pos2Averaging[i] = ptrY->Pos1Averaging[i];");
                writer.WriteLine("						ptrY->Pos1Averaging[i] = BasePos;");
                writer.WriteLine("					}");
                writer.WriteLine("					");
                writer.WriteLine("					/* Jitter filter */");
                writer.WriteLine("					if(" + instanceName + "_CSHL_WidgetTable[widget].Filters & " + instanceName + "_CSHL_POS_JITTER_FILTER)");
                writer.WriteLine("					{");
                writer.WriteLine("						PosY = " + instanceName + "_CSHL_JitterFilter(PosY, ptrY->PosJitter[i]);");
                writer.WriteLine("						ptrY->PosJitter[i] = PosY;");
                writer.WriteLine("					}");                
                writer.WriteLine("				}");
                writer.WriteLine("				");
                writer.WriteLine("				/* Save Results for X and Y */");
                writer.WriteLine("				ptrX->Position[i] = PosX;");
                writer.WriteLine("				ptrY->Position[i] = PosY;");
                writer.WriteLine("				");
                writer.WriteLine("			}");
                writer.WriteLine("");
                writer.WriteLine("			/* Now only for One finger */");
                writer.WriteLine("			Touch = 0x01;		");
                writer.WriteLine("		}");
                writer.WriteLine("		else");
                writer.WriteLine("		{");
                writer.WriteLine("			Touch = 0;");
                writer.WriteLine("		}");
                writer.WriteLine("	}");
                writer.WriteLine("	else");
                writer.WriteLine("	{");
                writer.WriteLine("		Touch = 0;");
                writer.WriteLine("	}	");
                writer.WriteLine("");
                writer.WriteLine("	return Touch;");
                writer.WriteLine("}");
                writer.WriteLine("");
                #endregion
                #endregion
            }
            
        }   

        #endregion
    }
}

