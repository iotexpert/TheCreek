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

namespace CapSense_v1_20.API
{
    public partial class CyAPIGenerator
    {
        #region apiCollectCHLVariablesApiBasePart
        void ApiCollectCHLVariables(ref CyMyStringWriter writerResult, CyGeneralParams packParams)
        {
            CyMyStringWriter writerMain = new CyMyStringWriter();
            CyMyStringWriter writerValHead = new CyMyStringWriter();
            CyMyStringWriter writer;

            #region  Filling Widget_Table
            //Write _WidgetTable Header

            writer = writerMain;
            int PropertiesIndex = 0;
            int ArrayCaseWidgetsCount = packParams.m_cyWidgetsList.GetDoubleFullWidgetsCount();
            string[] listProperties = new string[packParams.m_cyWidgetsList.GetCountWidgetsHL()];
            foreach (CyElWidget wi in packParams.m_cyWidgetsList.GetListWithFullWidgetsHL())
            {
                for (int i = 0; i < packParams.m_cyWidgetsList.GetBothParts(wi).Count; i++)
                {
                    writer = new CyMyStringWriter();
                    CyElWidget part = packParams.m_cyWidgetsList.GetBothParts(wi)[i];
                    CyHAProps props = packParams.m_cyWidgetsList.GetWidgetsProperties(part);
                    writer.Write("\t{");

                    writer.Write(m_instanceName + "_CSHL_TYPE_" + CySensorType.GetBaseType(part.m_type));  /* Type of widget element */
                    if (part.GetType() == typeof(CyElUnSlider))
                    {
                        if (((CyElUnSlider)part).m_diplexing)
                            writer.Write(" | " + m_instanceName + "_CSHL_IS_DIPLEX");
                    }
                    writer.Write(", ");

                    List<CyElTerminal> w_listTerminals = packParams.m_cyWidgetsList.GetTerminalsWithOutHeader(part);
                    int t_IndexOffset = 0;
                    if (w_listTerminals.Count > 0)
                        t_IndexOffset = packParams.GetRawDataOffset(part);

                    writer.Write(t_IndexOffset);    /* Offset in SlotResult array */
                    writer.Write(", ");
                    writer.Write(w_listTerminals.Count);/* Number of Slot elements */
                    writer.Write(", ");
                    if (CySensorType.IsTouchPad(part.m_type))
                    {
                        writer.Write(((CyElUnTouchPad)part).GetSeparateProperties().FingerThreshold);//FigerThresholds;
                        writer.Write(", ");
                        writer.Write(((CyElUnTouchPad)part).GetSeparateProperties().NoiseThreshold);// NoiseThresholds;
                        writer.Write(", ");

                    }
                    else
                    {
                        writer.Write(props.GetPropertyByName(typeof(CyHATrProperties), props.m_TrProperties.FingerThreshold));//FigerThresholds;
                        writer.Write(", ");
                        writer.Write(props.GetPropertyByName(typeof(CyHATrProperties), props.m_TrProperties.NoiseThreshold));// NoiseThresholds;
                        writer.Write(", ");
                    }
                    writer.Write(props.GetPropertyByName(typeof(CyHAMiscProperties), props.m_MiscProperties.m_Debounce)); //uint8 Debounce;
                    writer.Write(", ");
                    writer.Write(props.GetPropertyByName(typeof(CyHAMiscProperties), props.m_MiscProperties.m_Hysteresis));// uint8 Hysteresis;
                    writer.Write(", ");
                    writer.Write(props.GetFilterMask());//Filters;            /* Raw and Pos*/
                    writer.Write(", ");
                    if ((props.GetFilterMask() != 0) || (CySensorType.IsButtonsStrc(part.GetType())==false))
                        writer.Write("&" + m_instanceName + "_" + CyElWidget.GetPrefixCSHL(part.GetType()) + "SettingsTable[" + packParams.m_cyWidgetsList.GetIndexInTypedListHL(part) + "]");
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
                List<CyElWidget> listWi = packParams.m_cyWidgetsList.GetListWidgetsByTypeHL(itemType);
                if ((listWi.Count > 0) && ((CySensorType.IsButtonsStrc(itemType)==false) || (packParams.m_cyWidgetsList.GetCountFilterOnHL(itemType) != 0)))
                {
                    writer.WriteLine(m_instanceName + "_" + CyElWidget.GetPrefixCSHL(itemType) + "Settings " + m_instanceName + "_" + CyElWidget.GetPrefixCSHL(itemType) +
                                            "SettingsTable[" + listWi.Count + "] = ");
                    writer.WriteLine("{");

                    foreach (CyElWidget item in listWi)
                    {
                        if (itemType == typeof(CyElUnButton))
                        {
                            writer.Write("\t{0, 0, 0, 0, 0, 0}");
                        }
                        else //Not ElUnButton type
                        {
                            writer.Write("\t{");
                            if (itemType != typeof(CyElUnMatrixButton))
                            {
                                writer.Write(item.GetFullResolution()); //Resolution
                                writer.Write(", ");
                            }
                            if (itemType == typeof(CyElUnSlider))
                            {
                                if (((CyElUnSlider)item).m_diplexing)
                                    writer.Write(m_instanceName + "_CSHL_" + "Diplexing_" + item.GetCount()); //DiplexTable
                                else
                                    writer.Write("0");
                                writer.Write(", ");
                            }

                            //Raw Data Filtering
                            CyHAProps props = packParams.m_cyWidgetsList.GetWidgetsProperties(item);
                            E_ENE_DIS isJitterRD = (E_ENE_DIS)Enum.Parse(typeof(E_ENE_DIS), 
                                props.GetPropertyByName(typeof(CyHAFilterPropertiesRwDt), 
                                props.m_FilterPropertiesRwDt.JitterFilterRwDt));

                            E_ENE_DIS isMedianRD = (E_ENE_DIS)Enum.Parse(typeof(E_ENE_DIS), 
                                props.GetPropertyByName(typeof(CyHAFilterPropertiesRwDt), 
                                props.m_FilterPropertiesRwDt.MedianFilterRwDt));

                            E_ENE_DIS isAveragingRD = (E_ENE_DIS)Enum.Parse(typeof(E_ENE_DIS), 
                                props.GetPropertyByName(typeof(CyHAFilterPropertiesRwDt), 
                                props.m_FilterPropertiesRwDt.AveragingFilterRwDt));

                            E_FO_FILTER isFirstOrdertIirRDfo = (E_FO_FILTER)Enum.Parse(typeof(E_FO_FILTER), 
                                props.GetPropertyByName(typeof(CyHAFilterPropertiesRwDt), 
                                props.m_FilterPropertiesRwDt.FirstOrderIIRFilterRwDt));

                            E_ENE_DIS isFirstOrdertIirRD = isFirstOrdertIirRDfo == E_FO_FILTER.Disabled ?
                                E_ENE_DIS.Disabled : E_ENE_DIS.Enabled;
                            int ArrayLength = item.GetCount();                            

                            writer.Write(AddElement(ref writerValHead, "Raw1Median", item, isMedianRD, ArrayLength)); //
                            writer.Write(", ");
                            writer.Write(AddElement(ref writerValHead, "Raw2Median", item, isMedianRD, ArrayLength)); //
                            writer.Write(", ");

                            writer.Write(AddElement(ref writerValHead, "Raw1Averaging", item, isAveragingRD, ArrayLength)); //
                            writer.Write(", ");
                            writer.Write(AddElement(ref writerValHead, "Raw2Averaging", item, isAveragingRD, ArrayLength)); //                            
                            writer.Write(", ");
                            writer.Write(AddElement(ref writerValHead, "RawIIR", item, isFirstOrdertIirRD, ArrayLength));
                            writer.Write(", ");
                            writer.Write(AddElement(ref writerValHead, "RawJitter", item, isJitterRD, ArrayLength)); //
                            

                            //Position Filtering
                            if (itemType == typeof(CyElUnSlider))
                                writer.Write(", 0, 0, 0, 0, 0, 0, 0");

                            if (itemType == typeof(CyElUnTouchPad))
                            {
                                E_ENE_DIS isJitterPos = (E_ENE_DIS)Enum.Parse(typeof(E_ENE_DIS), 
                                    props.GetPropertyByName(typeof(CyHAFilterPropertiesPos), 
                                    props.m_FilterPropertiesPos.JitterFilterPos));

                                E_ENE_DIS isMedianPos = (E_ENE_DIS)Enum.Parse(typeof(E_ENE_DIS), 
                                    props.GetPropertyByName(typeof(CyHAFilterPropertiesPos), 
                                    props.m_FilterPropertiesPos.MedianFilterPos));

                                E_ENE_DIS isAveragingPos = (E_ENE_DIS)Enum.Parse(typeof(E_ENE_DIS), 
                                    props.GetPropertyByName(typeof(CyHAFilterPropertiesPos), 
                                    props.m_FilterPropertiesPos.AveragingFilterPos));
                                E_FO_FILTER isFirstOrdertIirPosFo = (E_FO_FILTER)Enum.Parse(typeof(E_FO_FILTER), 
                                    props.GetPropertyByName(typeof(CyHAFilterPropertiesPos), 
                                    props.m_FilterPropertiesPos.FirstOrderIIRFilterPos));

                                E_ENE_DIS isFirstOrdertIirPos = isFirstOrdertIirPosFo == E_FO_FILTER.Disabled ?
                                    E_ENE_DIS.Disabled : E_ENE_DIS.Enabled;

                                ArrayLength = 1;
                                writer.Write(",  0");  //First Time       
                                writer.Write(", ");
                                writer.Write(AddElement(ref writerValHead, "Pos1Median", item, isMedianPos, ArrayLength));
                                writer.Write(", ");
                                writer.Write(AddElement(ref writerValHead, "Pos2Median", item, isMedianPos, ArrayLength));
                                writer.Write(", ");
                                writer.Write(AddElement(ref writerValHead, "Pos1Averaging", item, isAveragingPos, ArrayLength));
                                writer.Write(", ");
                                writer.Write(AddElement(ref writerValHead, "Pos2Averaging", item, isAveragingPos, ArrayLength));
                                writer.Write(", ");
                                writer.Write(AddElement(ref writerValHead, "PosIIR", item, isFirstOrdertIirPos, ArrayLength));
                                writer.Write(", ");
                                writer.Write(AddElement(ref writerValHead, "PosJitter", item, isJitterPos, ArrayLength));
                                writer.Write(", ");
                                writer.Write(AddElement(ref writerValHead, "Results", item, E_ENE_DIS.Enabled, ArrayLength));
                            }
                            writer.Write("}");
                        }

                        //End char
                        if (BNotLastItem(listWi.ToArray(), item))
                            writer.WriteLine(",");
                        else writer.WriteLine("");
                    }

                    writer.WriteLine("};");
                    writer.WriteLine("");
                }
            }

            #endregion

            #region  LineOUT Widget Table
            writer.WriteLine("" + m_instanceName + "_Widget " + m_instanceName + "_CSHL_WidgetTable[] = ");
            writer.WriteLine("{");
            foreach (string item in listProperties)
            {
                writer.Write(item);
                if (BNotLastItem(listProperties, item))
                    writer.WriteLine(",");
                else writer.WriteLine("");
            }
            if (listProperties.Length == 0) writer.WriteLine("0");
            writer.WriteLine("};");
            #endregion

            foreach (CyAmuxBParams sbItem in packParams.m_localParams.m_listCsHalfs)
                if (packParams.m_localParams.IsAmuxBusEnable(sbItem))
                {
                    E_EL_SIDE side = sbItem.m_side;
                    string sSide = GetSideName(side, packParams.Configuration);
                    string sSideUp = GetSideNameUpper(side, packParams.Configuration);

                    writer.WriteLine("extern " + m_instanceName + "_Slot " + m_instanceName + "_ScanSlotTable" + sSide + "[" + m_instanceName + "_TOTAL_SCANSLOT_COUNT" + sSideUp + "]; ");
                }

            #region Static CSHL Variables
            writer.WriteLine("extern uint16 " + m_instanceName + "_SlotResult[" + m_instanceName + "_TOTAL_SCANSLOT_COUNT];");
            writer.WriteLine("");
            writer.WriteLine("uint8 " + m_instanceName + "_CSHL_SlotOnMask[((((" + m_instanceName + "_TOTAL_SCANSLOT_COUNT - " + m_instanceName + "_TOTAL_GENERIC_SCANSLOT_COUNT) - 1) / 8) + 1)] = {0};");
            writer.WriteLine("uint16 " + m_instanceName + "_CSHL_SlotBaseline[(" + m_instanceName + "_TOTAL_SCANSLOT_COUNT - " + m_instanceName + "_TOTAL_GENERIC_SCANSLOT_COUNT)] = {0};");
            writer.WriteLine("uint8 " + m_instanceName + "_CSHL_SlotBaselineLow[(" + m_instanceName + "_TOTAL_SCANSLOT_COUNT  - " + m_instanceName + "_TOTAL_GENERIC_SCANSLOT_COUNT)] = {0};");
            writer.WriteLine("uint8 " + m_instanceName + "_CSHL_SlotSignal[(" + m_instanceName + "_TOTAL_SCANSLOT_COUNT - " + m_instanceName + "_TOTAL_GENERIC_SCANSLOT_COUNT)] = {0};");
            #endregion

            List<int> listDiplexingUse = new List<int>();
            foreach (CyElWidget item in packParams.m_cyWidgetsList.GetListWidgets())
                if (CySensorType.IsSlider(item.m_type))
                {
                    if (((CyElUnSlider)item).m_diplexing)
                        if (listDiplexingUse.IndexOf(item.GetCount()) == -1)
                        {
                            writerValHead.WriteLine(AddDiplexing(item));
                            listDiplexingUse.Add(item.GetCount());
                        }
                }
            writerResult.WriteLine(writerValHead.ToString());

            #region Radial Slider Consts
            if (packParams.m_cyWidgetsList.GetWidgetsCount(E_SENSOR_TYPE.Radial_Slider) > 0)
            {
                writer.WriteLine("const uint8 " + m_instanceName + "_CSHL_RotarySliderAngle[9] = {180, 106, 56, 29, 14, 7, 4, 2, 1};");
                writer.WriteLine("");
                writer.WriteLine("/* Array of SIN Table */");
                writer.WriteLine("const uint8 " + m_instanceName + "_CSHL_RotarySliderAngleSin[46] = {");
                writer.WriteLine("           4,       13,       22,       31,       40,");
                writer.WriteLine("          49,       58,       66,       75,       83,");
                writer.WriteLine("          92,      100,      108,      116,      124, ");
                writer.WriteLine("         132,      139,      147,      154,      161, ");
                writer.WriteLine("         168,      175,      181,      187,      193, ");
                writer.WriteLine("         199,      204,      210,      215,      219, ");
                writer.WriteLine("         224,      228,      232,      236,      239, ");
                writer.WriteLine("         242,      245,      247,      249,      251, ");
                writer.WriteLine("         253,      254,      255,      255,      255, 255 ");
                writer.WriteLine("        };");
            }
            #endregion

            writerResult.WriteLine(writer.ToString());
        }
        #region Service Functions
        string AddElement(ref CyMyStringWriter writer, string header, CyElWidget wi, E_ENE_DIS en, int ArrayLength)
        {
            string res = "0";
            if ((ArrayLength > 0) && (en == E_ENE_DIS.Enabled))
            {
                res = m_instanceName + "_" + "CSHL" + "_" + wi.ToString() + "_" + header;
                writer.WriteLine("uint16 " + res + "[" + ArrayLength + "];");
            }
            return res;
        }
        string AddDiplexing(CyElWidget wi)
        {
            string res = "";
            if (CySensorType.IsSlider(wi.m_type))
            {
                res = "uint8 " + m_instanceName + "_CSHL_" + "Diplexing_" + wi.GetCount() + "[" + wi.GetCount() * 2 + "]";
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
                //New algorithm
                i = length;
                for (int j = 0; j < 3; j++)
                {
                    k = j;
                    while (k < length)
                    {
                        arrIndexs[i++] = k;
                        k += 3;
                    }
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
        public void ApiCollectCHLFunctionForSide(ref CyMyStringWriter writer, CyGeneralParams packParams, CyAmuxBParams sbItem)
        {

            E_EL_SIDE side = sbItem.m_side;
            string sSide = GetSideName(side, packParams.Configuration);
            string sSideUp = GetSideNameUpper(side, packParams.Configuration);

            string Method = sbItem.m_Method.ToString();
            string Symbol = SymbolGenerate(packParams, sbItem, packParams.Configuration);

            #region CSHL_InitializeSlotBaseline Left/Right
            writer.WriteLine("/*******************************************************************************");
            writer.WriteLine("* Function Name:   " + m_instanceName + "_CSHL_InitializeSlotBaseline" + sSide);
            writer.WriteLine("********************************************************************************");
            writer.WriteLine("* ");
            writer.WriteLine("* Summary:");
            writer.WriteLine("*  Loads the " + m_instanceName + "_CSHL_SlotBaseline[slot] array element with an initial");
            writer.WriteLine("*  value by scanning the selected slot. The raw count value is copied into");
            writer.WriteLine("*  the baseline array for each slot. The raw data filters are initialized");
            writer.WriteLine("*  if enabled.");
            writer.WriteLine("* ");
            writer.WriteLine("* Parameters:");
            writer.WriteLine("*  slot:  Scan slot number");
            writer.WriteLine("* ");
            writer.WriteLine("* Return:");
            writer.WriteLine("*  void");
            writer.WriteLine("* ");
            writer.WriteLine("**********************************************************************************/");
            writer.WriteLine("void " + m_instanceName + "_CSHL_InitializeSlotBaseline" + sSide + "(uint8 slot)");
            writer.WriteLine("{");
            writer.WriteLine("    void *ptr;");
            writer.WriteLine("    uint8 rawIndex = " + m_instanceName + "_ScanSlotTable" + sSide + "[slot].RawIndex;");
            writer.WriteLine("    uint8 widget = " + m_instanceName + "_ScanSlotTable" + sSide + "[slot].WidgetNumber;");
            writer.WriteLine("    uint8 filterPos = rawIndex - " + m_instanceName + "_CSHL_WidgetTable[widget].RawOffset;");
            writer.WriteLine("    ");
            writer.WriteLine("    if( widget != " + m_instanceName + "_CSHL_NO_WIDGET)");
            writer.WriteLine("    {");
            writer.WriteLine("        /* Scan slot to have raw data */");
            writer.WriteLine("        " + m_instanceName + "_" + Method + "_ScanSlot" + sSide + "(slot);");
            writer.WriteLine("        ");
            writer.WriteLine("        /* Initialize Baseline */");
            writer.WriteLine("        " + m_instanceName + "_CSHL_SlotBaseline[rawIndex] = " + m_instanceName + "_SlotResult[rawIndex];");
            writer.WriteLine("        " + m_instanceName + "_CSHL_SlotBaselineLow[rawIndex] = 0;");
            writer.WriteLine("        " + m_instanceName + "_CSHL_SlotSignal[rawIndex] = 0;");
            writer.WriteLine("        " + m_instanceName + "_ScanSlotTable" + sSide + "[slot].DebounceCount = " + m_instanceName + "_CSHL_WidgetTable[widget].Debounce;");
            writer.WriteLine("        ");
            writer.WriteLine("        switch ((" + m_instanceName + "_CSHL_WidgetTable[widget].Type & (~" + m_instanceName + "_CSHL_IS_DIPLEX)))");
            writer.WriteLine("        {");
            writer.WriteLine("            /* This case include BTN and PROX */");
            writer.WriteLine("            case " + m_instanceName + "_CSHL_TYPE_BUTTON:");
            writer.WriteLine("            case " + m_instanceName + "_CSHL_TYPE_PROXIMITY:");
            writer.WriteLine("                if (" + m_instanceName + "_CSHL_WidgetTable[widget].AdvancedSettings)");
            writer.WriteLine("                {");
            writer.WriteLine("                    ptr = " + m_instanceName + "_CSHL_WidgetTable[widget].AdvancedSettings;");
            writer.WriteLine("                    ");
            writer.WriteLine("                    /* Median filter first time initialization */");
            writer.WriteLine("                    if(" + m_instanceName + "_CSHL_WidgetTable[widget].Filters & " + m_instanceName + "_CSHL_RAW_MEDIAN_FILTER)");
            writer.WriteLine("                    {");
            writer.WriteLine("                        ((" + m_instanceName + "_BtnSettings *) ptr)->Raw2Median = " + m_instanceName + "_SlotResult[rawIndex];");
            writer.WriteLine("                        ((" + m_instanceName + "_BtnSettings *) ptr)->Raw1Median = " + m_instanceName + "_SlotResult[rawIndex];");
            writer.WriteLine("                    }");
            writer.WriteLine("                    ");
            writer.WriteLine("                    /* Averaging filter first time initialization */");
            writer.WriteLine("                    if(" + m_instanceName + "_CSHL_WidgetTable[widget].Filters & " + m_instanceName + "_CSHL_RAW_AVERAGING_FILTER)");
            writer.WriteLine("                    {");
            writer.WriteLine("                        ((" + m_instanceName + "_BtnSettings *) ptr)->Raw2Averaging = " + m_instanceName + "_SlotResult[rawIndex];");
            writer.WriteLine("                        ((" + m_instanceName + "_BtnSettings *) ptr)->Raw1Averaging = " + m_instanceName + "_SlotResult[rawIndex];");
            writer.WriteLine("                    }");
            writer.WriteLine("                    ");
            writer.WriteLine("                    /* IIR filter first time initialization */");
            writer.WriteLine("                    if((" + m_instanceName + "_CSHL_WidgetTable[widget].Filters & " + m_instanceName + "_CSHL_RAW_IIR_FILTER_0) ||");
            writer.WriteLine("                        (" + m_instanceName + "_CSHL_WidgetTable[widget].Filters & " + m_instanceName + "_CSHL_RAW_IIR_FILTER_1))");
            writer.WriteLine("                    {");
            writer.WriteLine("                        ((" + m_instanceName + "_BtnSettings *) ptr)->RawIIR = " + m_instanceName + "_SlotResult[rawIndex];");
            writer.WriteLine("                    }");
            writer.WriteLine("                    ");
            writer.WriteLine("                    /* Jitter filter first time initialization */");
            writer.WriteLine("                    if(" + m_instanceName + "_CSHL_WidgetTable[widget].Filters & " + m_instanceName + "_CSHL_RAW_JITTER_FILTER)");
            writer.WriteLine("                    {");
            writer.WriteLine("                        ((" + m_instanceName + "_BtnSettings *) ptr)->RawJitter = " + m_instanceName + "_SlotResult[rawIndex];");
            writer.WriteLine("                    }");
            writer.WriteLine("                }");
            writer.WriteLine("                break;");
            writer.WriteLine("            ");
            writer.WriteLine("            /* This case include LINEAR and RADIAL Slider */");
            writer.WriteLine("            case " + m_instanceName + "_CSHL_TYPE_RADIAL_SLIDER :");
            writer.WriteLine("            case " + m_instanceName + "_CSHL_TYPE_LINEAR_SLIDER :");
            writer.WriteLine("                ");
            writer.WriteLine("                ptr = " + m_instanceName + "_CSHL_WidgetTable[widget].AdvancedSettings;");
            writer.WriteLine("                ((" + m_instanceName + "_SlSettings *) ptr)->FirstTime=0;");
            writer.WriteLine("                ");
            writer.WriteLine("                /* Median filter first time initialization */");
            writer.WriteLine("                if(" + m_instanceName + "_CSHL_WidgetTable[widget].Filters & " + m_instanceName + "_CSHL_RAW_MEDIAN_FILTER)");
            writer.WriteLine("                {");
            writer.WriteLine("                    ((" + m_instanceName + "_SlSettings *) ptr)->Raw2Median[filterPos] = " + m_instanceName + "_SlotResult[rawIndex];");
            writer.WriteLine("                    ((" + m_instanceName + "_SlSettings *) ptr)->Raw1Median[filterPos] = " + m_instanceName + "_SlotResult[rawIndex];");
            writer.WriteLine("                }");
            writer.WriteLine("                ");
            writer.WriteLine("                /* Averaging filter first time initialization */");
            writer.WriteLine("                if(" + m_instanceName + "_CSHL_WidgetTable[widget].Filters & " + m_instanceName + "_CSHL_RAW_AVERAGING_FILTER)");
            writer.WriteLine("                {");
            writer.WriteLine("                    ((" + m_instanceName + "_SlSettings *) ptr)->Raw2Averaging[filterPos] = " + m_instanceName + "_SlotResult[rawIndex];");
            writer.WriteLine("                    ((" + m_instanceName + "_SlSettings *) ptr)->Raw1Averaging[filterPos] = " + m_instanceName + "_SlotResult[rawIndex];");
            writer.WriteLine("                }");
            writer.WriteLine("                ");
            writer.WriteLine("                /* IIR filter first time initialization */");
            writer.WriteLine("                if((" + m_instanceName + "_CSHL_WidgetTable[widget].Filters & " + m_instanceName + "_CSHL_RAW_IIR_FILTER_0) ||");
            writer.WriteLine("                    (" + m_instanceName + "_CSHL_WidgetTable[widget].Filters & " + m_instanceName + "_CSHL_RAW_IIR_FILTER_1))");
            writer.WriteLine("                {");
            writer.WriteLine("                    ((" + m_instanceName + "_SlSettings *) ptr)->RawIIR[filterPos] = " + m_instanceName + "_SlotResult[rawIndex];");
            writer.WriteLine("                }");
            writer.WriteLine("                ");
            writer.WriteLine("                /* Jitter filter first time initialization */");
            writer.WriteLine("                if(" + m_instanceName + "_CSHL_WidgetTable[widget].Filters & " + m_instanceName + "_CSHL_RAW_JITTER_FILTER)");
            writer.WriteLine("                {");
            writer.WriteLine("                    ((" + m_instanceName + "_SlSettings *) ptr)->RawJitter[filterPos] = " + m_instanceName + "_SlotResult[rawIndex];");
            writer.WriteLine("                }");
            writer.WriteLine("                break;");
            writer.WriteLine("                ");
            writer.WriteLine("            case " + m_instanceName + "_CSHL_TYPE_TOUCHPAD:");
            writer.WriteLine("                ");
            writer.WriteLine("                ptr = " + m_instanceName + "_CSHL_WidgetTable[widget].AdvancedSettings;");
            writer.WriteLine("                ((" + m_instanceName + "_TPSettings *) ptr)->FirstTime=0;");
            writer.WriteLine("                ");
            writer.WriteLine("                /* Median filter first time initialization */");
            writer.WriteLine("                if(" + m_instanceName + "_CSHL_WidgetTable[widget].Filters & " + m_instanceName + "_CSHL_RAW_MEDIAN_FILTER)");
            writer.WriteLine("                {");
            writer.WriteLine("                    ((" + m_instanceName + "_TPSettings *) ptr)->Raw2Median[filterPos] = " + m_instanceName + "_SlotResult[rawIndex];");
            writer.WriteLine("                    ((" + m_instanceName + "_TPSettings *) ptr)->Raw1Median[filterPos] = " + m_instanceName + "_SlotResult[rawIndex];");
            writer.WriteLine("                }");
            writer.WriteLine("                ");
            writer.WriteLine("                /* Averaging filter first time initialization */");
            writer.WriteLine("                if(" + m_instanceName + "_CSHL_WidgetTable[widget].Filters & " + m_instanceName + "_CSHL_RAW_AVERAGING_FILTER)");
            writer.WriteLine("                {");
            writer.WriteLine("                    ((" + m_instanceName + "_TPSettings *) ptr)->Raw2Averaging[filterPos] = " + m_instanceName + "_SlotResult[rawIndex];");
            writer.WriteLine("                    ((" + m_instanceName + "_TPSettings *) ptr)->Raw1Averaging[filterPos] = " + m_instanceName + "_SlotResult[rawIndex];");
            writer.WriteLine("                }");
            writer.WriteLine("                ");
            writer.WriteLine("                /* IIR filter first time initialization */");
            writer.WriteLine("                if((" + m_instanceName + "_CSHL_WidgetTable[widget].Filters & " + m_instanceName + "_CSHL_RAW_IIR_FILTER_0) ||");
            writer.WriteLine("                    (" + m_instanceName + "_CSHL_WidgetTable[widget].Filters & " + m_instanceName + "_CSHL_RAW_IIR_FILTER_1))");
            writer.WriteLine("                {");
            writer.WriteLine("                    ((" + m_instanceName + "_TPSettings *) ptr)->RawIIR[filterPos] = " + m_instanceName + "_SlotResult[rawIndex];");
            writer.WriteLine("                }");
            writer.WriteLine("                ");
            writer.WriteLine("                /* Jitter filter first time initialization */");
            writer.WriteLine("                if(" + m_instanceName + "_CSHL_WidgetTable[widget].Filters & " + m_instanceName + "_CSHL_RAW_JITTER_FILTER)");
            writer.WriteLine("                {");
            writer.WriteLine("                    ((" + m_instanceName + "_TPSettings *) ptr)->RawJitter[filterPos] = " + m_instanceName + "_SlotResult[rawIndex];");
            writer.WriteLine("                }");
            writer.WriteLine("                break;");
            writer.WriteLine("                ");
            writer.WriteLine("            case " + m_instanceName + "_CSHL_TYPE_MATRIX_BUTTONS:");
            writer.WriteLine("                ");
            writer.WriteLine("                if (" + m_instanceName + "_CSHL_WidgetTable[widget].AdvancedSettings)");
            writer.WriteLine("                {");
            writer.WriteLine("                    ptr = " + m_instanceName + "_CSHL_WidgetTable[widget].AdvancedSettings;");
            writer.WriteLine("                    ");
            writer.WriteLine("                    /* Median filter first time initialization */");
            writer.WriteLine("                    if(" + m_instanceName + "_CSHL_WidgetTable[widget].Filters & " + m_instanceName + "_CSHL_RAW_MEDIAN_FILTER)");
            writer.WriteLine("                    {");
            writer.WriteLine("                        ((" + m_instanceName + "_MBSettings *) ptr)->Raw2Median[filterPos] = " + m_instanceName + "_SlotResult[rawIndex];");
            writer.WriteLine("                    (   (" + m_instanceName + "_MBSettings *) ptr)->Raw1Median[filterPos] = " + m_instanceName + "_SlotResult[rawIndex];");
            writer.WriteLine("                    }");
            writer.WriteLine("                    ");
            writer.WriteLine("                    /* Averaging filter first time initialization */");
            writer.WriteLine("                    if(" + m_instanceName + "_CSHL_WidgetTable[widget].Filters & " + m_instanceName + "_CSHL_RAW_AVERAGING_FILTER)");
            writer.WriteLine("                    {");
            writer.WriteLine("                        ((" + m_instanceName + "_MBSettings *) ptr)->Raw2Averaging[filterPos] = " + m_instanceName + "_SlotResult[rawIndex];");
            writer.WriteLine("                        ((" + m_instanceName + "_MBSettings *) ptr)->Raw1Averaging[filterPos] = " + m_instanceName + "_SlotResult[rawIndex];");
            writer.WriteLine("                    }");
            writer.WriteLine("                    ");
            writer.WriteLine("                    /* IIR filter first time initialization */");
            writer.WriteLine("                    if((" + m_instanceName + "_CSHL_WidgetTable[widget].Filters & " + m_instanceName + "_CSHL_RAW_IIR_FILTER_0) ||");
            writer.WriteLine("                        (" + m_instanceName + "_CSHL_WidgetTable[widget].Filters & " + m_instanceName + "_CSHL_RAW_IIR_FILTER_1))");
            writer.WriteLine("                    {");
            writer.WriteLine("                        ((" + m_instanceName + "_MBSettings *) ptr)->RawIIR[filterPos] = " + m_instanceName + "_SlotResult[rawIndex];");
            writer.WriteLine("                    }");
            writer.WriteLine("                    ");
            writer.WriteLine("                    /* Jitter filter first time initialization */");
            writer.WriteLine("                    if(" + m_instanceName + "_CSHL_WidgetTable[widget].Filters & " + m_instanceName + "_CSHL_RAW_JITTER_FILTER)");
            writer.WriteLine("                    {");
            writer.WriteLine("                        ((" + m_instanceName + "_MBSettings *) ptr)->RawJitter[filterPos] = " + m_instanceName + "_SlotResult[rawIndex];");
            writer.WriteLine("                    }");
            writer.WriteLine("                }");
            writer.WriteLine("                break;");
            writer.WriteLine("                        ");
            writer.WriteLine("            default:");
            writer.WriteLine("            ");
            writer.WriteLine("                break;");
            writer.WriteLine("        }");
            writer.WriteLine("    }");
            writer.WriteLine("}");
            writer.WriteLine("");
            writer.WriteLine("");
            #endregion

            #region CSHL_InitializeAllBaseline Left/Right
            writer.WriteLine("/*******************************************************************************");
            writer.WriteLine("* Function Name:   " + m_instanceName + "_CSHL_InitializeAllBaselines" + sSide);
            writer.WriteLine("********************************************************************************");
            writer.WriteLine("* ");
            writer.WriteLine("* Summary:");
            writer.WriteLine("*  Uses the " + m_instanceName + "_CSHL_InitializeSlotBaselines" + sSide + " function to loads the");
            writer.WriteLine("*  " + m_instanceName + "_CSHL_SlotBaseline[ ] array with an initial values by scanning");
            writer.WriteLine("*  all slots. The raw count values are copied into the baseline array for");
            writer.WriteLine("*  all slots. The raw data filters are initialized if enabled.");
            writer.WriteLine("* ");
            writer.WriteLine("* Parameters:");
            writer.WriteLine("*  void");
            writer.WriteLine("* ");
            writer.WriteLine("* Return:");
            writer.WriteLine("*  void");
            writer.WriteLine("* ");
            writer.WriteLine("**********************************************************************************/");
            writer.WriteLine("void " + m_instanceName + "_CSHL_InitializeAllBaselines" + sSide + "(void)");
            writer.WriteLine("{");
            writer.WriteLine("    uint8 i;");
            writer.WriteLine("    ");
            writer.WriteLine("    for(i = 0; i < (" + m_instanceName + "_TOTAL_SCANSLOT_COUNT" + sSideUp + " - " + m_instanceName + "_TOTAL_GENERIC_SCANSLOT_COUNT" + sSideUp + "); i++)");
            writer.WriteLine("    {");
            writer.WriteLine("        " + m_instanceName + "_CSHL_InitializeSlotBaseline" + sSide + "(i);");
            writer.WriteLine("    }");
            writer.WriteLine("}");
            writer.WriteLine("");
            writer.WriteLine("");
            #endregion

            #region CSHL_UpdateSlotBaseline Left/Right
            writer.WriteLine("/*******************************************************************************");
            writer.WriteLine("* Function Name:   " + m_instanceName + "_CSHL_UpdateSlotBaseline" + sSide);
            writer.WriteLine("********************************************************************************");
            writer.WriteLine("* ");
            writer.WriteLine("* Summary:");
            writer.WriteLine("*  Updates the " + m_instanceName + "_CSHL_SlotBaseline[ ] array using the LP filter with");
            writer.WriteLine("*  k = 256. The signal calculates the difference between raw count and");
            writer.WriteLine("*  baseline. The baseline stops updating if signal is greater that zero.");
            writer.WriteLine("*  Raw data filters are applied to the values if enabled.");
            writer.WriteLine("* ");
            writer.WriteLine("* Parameters:");
            writer.WriteLine("*  slot:  Scan slot number");
            writer.WriteLine("* ");
            writer.WriteLine("* Return:");
            writer.WriteLine("*  void");
            writer.WriteLine("* ");
            writer.WriteLine("**********************************************************************************/");
            writer.WriteLine(" void " + m_instanceName + "_CSHL_UpdateSlotBaseline" + sSide + "(uint8 slot)");
            writer.WriteLine("{");
            writer.WriteLine("    void *ptr;");
            writer.WriteLine("    uint8 rawIndex = " + m_instanceName + "_ScanSlotTable" + sSide + "[slot].RawIndex;");
            writer.WriteLine("    uint8 widget = " + m_instanceName + "_ScanSlotTable" + sSide + "[slot].WidgetNumber;");
            writer.WriteLine("    uint8 filterPos = rawIndex - " + m_instanceName + "_CSHL_WidgetTable[widget].RawOffset;");
            writer.WriteLine("    uint16 filteredRawData = " + m_instanceName + "_SlotResult[rawIndex];");
            writer.WriteLine("    int32 temp;");
            writer.WriteLine("    uint16 baseRawData;");
            writer.WriteLine("    ");
            writer.WriteLine("    if( widget != " + m_instanceName + "_CSHL_NO_WIDGET)");
            writer.WriteLine("    {");
            writer.WriteLine("        ");
            writer.WriteLine("        /* Do filtering */");
            writer.WriteLine("        switch ((" + m_instanceName + "_CSHL_WidgetTable[widget].Type & (~" + m_instanceName + "_CSHL_IS_DIPLEX)))");
            writer.WriteLine("        {");
            writer.WriteLine("            /* This case include BTN and PROX */");
            writer.WriteLine("            case " + m_instanceName + "_CSHL_TYPE_BUTTON:");
            writer.WriteLine("            case " + m_instanceName + "_CSHL_TYPE_PROXIMITY:");
            writer.WriteLine("                if(" + m_instanceName + "_CSHL_WidgetTable[widget].AdvancedSettings)");
            writer.WriteLine("                {");
            writer.WriteLine("                    ptr = " + m_instanceName + "_CSHL_WidgetTable[widget].AdvancedSettings;");
            writer.WriteLine("                    ");
            writer.WriteLine("                    /* Median filter */");
            writer.WriteLine("                    if(" + m_instanceName + "_CSHL_WidgetTable[widget].Filters & " + m_instanceName + "_CSHL_RAW_MEDIAN_FILTER)");
            writer.WriteLine("                    {");
            writer.WriteLine("                        baseRawData = filteredRawData;");
            writer.WriteLine("                        filteredRawData = " + m_instanceName + "_CSHL_MedianFilter(filteredRawData, ((" + m_instanceName + "_BtnSettings *) ptr)->Raw1Median, ((" + m_instanceName + "_BtnSettings *) ptr)->Raw2Median);");
            writer.WriteLine("                        ((" + m_instanceName + "_BtnSettings *) ptr)->Raw2Median = ((" + m_instanceName + "_BtnSettings *) ptr)->Raw1Median;");
            writer.WriteLine("                        ((" + m_instanceName + "_BtnSettings *) ptr)->Raw1Median = baseRawData;");
            writer.WriteLine("                    }");
            writer.WriteLine("                    ");
            writer.WriteLine("                    /* Averaging filter */");
            writer.WriteLine("                    if(" + m_instanceName + "_CSHL_WidgetTable[widget].Filters & " + m_instanceName + "_CSHL_RAW_AVERAGING_FILTER)");
            writer.WriteLine("                    {");
            writer.WriteLine("                        baseRawData = filteredRawData;");
            writer.WriteLine("                        filteredRawData = " + m_instanceName + "_CSHL_AveragingFilter(filteredRawData, ((" + m_instanceName + "_BtnSettings *) ptr)->Raw1Averaging, ((" + m_instanceName + "_BtnSettings *) ptr)->Raw2Averaging);");
            writer.WriteLine("                        ((" + m_instanceName + "_BtnSettings *) ptr)->Raw2Averaging = ((" + m_instanceName + "_BtnSettings *) ptr)->Raw1Averaging;");
            writer.WriteLine("                        ((" + m_instanceName + "_BtnSettings *) ptr)->Raw1Averaging = baseRawData;");
            writer.WriteLine("                    }");
            writer.WriteLine("                    ");
            writer.WriteLine("                    /* IIR filter */");
            writer.WriteLine("                    if(" + m_instanceName + "_CSHL_WidgetTable[widget].Filters & " + m_instanceName + "_CSHL_RAW_IIR_FILTER_0)");
            writer.WriteLine("                    {");
            writer.WriteLine("                        baseRawData = filteredRawData;");
            writer.WriteLine("                        filteredRawData = " + m_instanceName + "_CSHL_IIRFilter(filteredRawData, ((" + m_instanceName + "_BtnSettings *) ptr)->RawIIR, " + m_instanceName + "_CSHL_IIR_FILTER_0);");
            writer.WriteLine("                        ((" + m_instanceName + "_BtnSettings *) ptr)->RawIIR = baseRawData;");
            writer.WriteLine("                    }");
            writer.WriteLine("                    else if(" + m_instanceName + "_CSHL_WidgetTable[widget].Filters & " + m_instanceName + "_CSHL_RAW_IIR_FILTER_1)");
            writer.WriteLine("                    {");
            writer.WriteLine("                        baseRawData = filteredRawData;");
            writer.WriteLine("                        filteredRawData = " + m_instanceName + "_CSHL_IIRFilter(filteredRawData, ((" + m_instanceName + "_BtnSettings *) ptr)->RawIIR, " + m_instanceName + "_CSHL_IIR_FILTER_1);");
            writer.WriteLine("                        ((" + m_instanceName + "_BtnSettings *) ptr)->RawIIR = baseRawData;");
            writer.WriteLine("                    }");
            writer.WriteLine("                    ");
            writer.WriteLine("                    /* Jitter filter*/");
            writer.WriteLine("                    if(" + m_instanceName + "_CSHL_WidgetTable[widget].Filters & " + m_instanceName + "_CSHL_RAW_JITTER_FILTER)");
            writer.WriteLine("                    {");
            writer.WriteLine("                        baseRawData = filteredRawData;");
            writer.WriteLine("                        filteredRawData = " + m_instanceName + "_CSHL_JitterFilter(filteredRawData, ((" + m_instanceName + "_BtnSettings *) ptr)->RawJitter);");
            writer.WriteLine("                        ((" + m_instanceName + "_BtnSettings *) ptr)->RawJitter = baseRawData;");
            writer.WriteLine("                    }");
            writer.WriteLine("                }");
            writer.WriteLine("                break;");
            writer.WriteLine("            ");
            writer.WriteLine("            /* This case include LINEAR and RADIAL Slider */");
            writer.WriteLine("            case " + m_instanceName + "_CSHL_TYPE_RADIAL_SLIDER :");
            writer.WriteLine("            case " + m_instanceName + "_CSHL_TYPE_LINEAR_SLIDER :");
            writer.WriteLine("                ptr = " + m_instanceName + "_CSHL_WidgetTable[widget].AdvancedSettings;");
            writer.WriteLine("                ");
            writer.WriteLine("                /* Median filter */");
            writer.WriteLine("                if(" + m_instanceName + "_CSHL_WidgetTable[widget].Filters & " + m_instanceName + "_CSHL_RAW_MEDIAN_FILTER)");
            writer.WriteLine("                {");
            writer.WriteLine("                    baseRawData = filteredRawData;");
            writer.WriteLine("                    filteredRawData = " + m_instanceName + "_CSHL_MedianFilter(filteredRawData, ((" + m_instanceName + "_SlSettings *) ptr)->Raw1Median[filterPos], ((" + m_instanceName + "_SlSettings *) ptr)->Raw2Median[filterPos]);");
            writer.WriteLine("                    ((" + m_instanceName + "_SlSettings *) ptr)->Raw2Median[filterPos] = ((" + m_instanceName + "_SlSettings *) ptr)->Raw1Median[filterPos];");
            writer.WriteLine("                    ((" + m_instanceName + "_SlSettings *) ptr)->Raw1Median[filterPos] = baseRawData;");
            writer.WriteLine("                }");
            writer.WriteLine("                ");
            writer.WriteLine("                /* Averaging filter */");
            writer.WriteLine("                if(" + m_instanceName + "_CSHL_WidgetTable[widget].Filters & " + m_instanceName + "_CSHL_RAW_AVERAGING_FILTER)");
            writer.WriteLine("                {");
            writer.WriteLine("                    baseRawData = filteredRawData;");
            writer.WriteLine("                       filteredRawData = " + m_instanceName + "_CSHL_AveragingFilter(filteredRawData, ((" + m_instanceName + "_SlSettings *) ptr)->Raw1Averaging[filterPos], ((" + m_instanceName + "_SlSettings *) ptr)->Raw2Averaging[filterPos]);");
            writer.WriteLine("                    ((" + m_instanceName + "_SlSettings *) ptr)->Raw2Averaging[filterPos] = ((" + m_instanceName + "_SlSettings *) ptr)->Raw1Averaging[filterPos];");
            writer.WriteLine("                    ((" + m_instanceName + "_SlSettings *) ptr)->Raw1Averaging[filterPos] = baseRawData;");
            writer.WriteLine("                }");
            writer.WriteLine("                ");
            writer.WriteLine("                /* IIR filter */");
            writer.WriteLine("                if(" + m_instanceName + "_CSHL_WidgetTable[widget].Filters & " + m_instanceName + "_CSHL_RAW_IIR_FILTER_0)");
            writer.WriteLine("                {");
            writer.WriteLine("                    baseRawData = filteredRawData;");
            writer.WriteLine("                    filteredRawData = " + m_instanceName + "_CSHL_IIRFilter(filteredRawData, ((" + m_instanceName + "_SlSettings *) ptr)->RawIIR[filterPos], " + m_instanceName + "_CSHL_IIR_FILTER_0);");
            writer.WriteLine("                    ((" + m_instanceName + "_SlSettings *) ptr)->RawIIR[filterPos] = baseRawData;");
            writer.WriteLine("                }");
            writer.WriteLine("                else if(" + m_instanceName + "_CSHL_WidgetTable[widget].Filters & " + m_instanceName + "_CSHL_RAW_IIR_FILTER_1)");
            writer.WriteLine("                {");
            writer.WriteLine("                    baseRawData = filteredRawData;");
            writer.WriteLine("                    filteredRawData = " + m_instanceName + "_CSHL_IIRFilter(filteredRawData, ((" + m_instanceName + "_SlSettings *) ptr)->RawIIR[filterPos], " + m_instanceName + "_CSHL_IIR_FILTER_1);");
            writer.WriteLine("                    ((" + m_instanceName + "_SlSettings *) ptr)->RawIIR[filterPos] = baseRawData;");
            writer.WriteLine("                }");
            writer.WriteLine("                ");
            writer.WriteLine("                /* Jitter filter */");
            writer.WriteLine("                if(" + m_instanceName + "_CSHL_WidgetTable[widget].Filters & " + m_instanceName + "_CSHL_RAW_JITTER_FILTER)");
            writer.WriteLine("                {");
            writer.WriteLine("                    baseRawData = filteredRawData;");
            writer.WriteLine("                    filteredRawData = " + m_instanceName + "_CSHL_JitterFilter(filteredRawData, ((" + m_instanceName + "_SlSettings *) ptr)->RawJitter[filterPos]);");
            writer.WriteLine("                    ((" + m_instanceName + "_SlSettings *) ptr)->RawJitter[filterPos] = baseRawData;");
            writer.WriteLine("                }");
            writer.WriteLine("                break;");
            writer.WriteLine("                ");
            writer.WriteLine("            case " + m_instanceName + "_CSHL_TYPE_TOUCHPAD:");
            writer.WriteLine("                ptr = " + m_instanceName + "_CSHL_WidgetTable[widget].AdvancedSettings;");
            writer.WriteLine("                ");
            writer.WriteLine("                /* Median filter */");
            writer.WriteLine("                if(" + m_instanceName + "_CSHL_WidgetTable[widget].Filters & " + m_instanceName + "_CSHL_RAW_MEDIAN_FILTER)");
            writer.WriteLine("                {");
            writer.WriteLine("                    baseRawData = filteredRawData;");
            writer.WriteLine("                    filteredRawData = " + m_instanceName + "_CSHL_MedianFilter(filteredRawData, ((" + m_instanceName + "_TPSettings *) ptr)->Raw1Median[filterPos], ((" + m_instanceName + "_TPSettings *) ptr)->Raw2Median[filterPos]);");
            writer.WriteLine("                       ((" + m_instanceName + "_TPSettings *) ptr)->Raw2Median[filterPos] = ((" + m_instanceName + "_TPSettings *) ptr)->Raw1Median[filterPos];");
            writer.WriteLine("                    ((" + m_instanceName + "_TPSettings *) ptr)->Raw1Median[filterPos] = baseRawData;");
            writer.WriteLine("                }");
            writer.WriteLine("                ");
            writer.WriteLine("                /* Averaging filter */");
            writer.WriteLine("                if(" + m_instanceName + "_CSHL_WidgetTable[widget].Filters & " + m_instanceName + "_CSHL_RAW_AVERAGING_FILTER)");
            writer.WriteLine("                {");
            writer.WriteLine("                    baseRawData = filteredRawData;");
            writer.WriteLine("                    filteredRawData = " + m_instanceName + "_CSHL_AveragingFilter(filteredRawData, ((" + m_instanceName + "_TPSettings *) ptr)->Raw1Averaging[filterPos], ((" + m_instanceName + "_TPSettings *) ptr)->Raw2Averaging[filterPos]);");
            writer.WriteLine("                    ((" + m_instanceName + "_TPSettings *) ptr)->Raw2Averaging[filterPos] = ((" + m_instanceName + "_TPSettings *) ptr)->Raw1Averaging[filterPos];");
            writer.WriteLine("                    ((" + m_instanceName + "_TPSettings *) ptr)->Raw1Averaging[filterPos] = baseRawData;");
            writer.WriteLine("                }");
            writer.WriteLine("                ");
            writer.WriteLine("                /* IIR filter */");
            writer.WriteLine("                if(" + m_instanceName + "_CSHL_WidgetTable[widget].Filters & " + m_instanceName + "_CSHL_RAW_IIR_FILTER_0)");
            writer.WriteLine("                {");
            writer.WriteLine("                    baseRawData = filteredRawData;");
            writer.WriteLine("                    filteredRawData = " + m_instanceName + "_CSHL_IIRFilter(filteredRawData, ((" + m_instanceName + "_TPSettings *) ptr)->RawIIR[filterPos], " + m_instanceName + "_CSHL_IIR_FILTER_0);");
            writer.WriteLine("                    ((" + m_instanceName + "_TPSettings *) ptr)->RawIIR[filterPos] = baseRawData;");
            writer.WriteLine("                }");
            writer.WriteLine("                else if(" + m_instanceName + "_CSHL_WidgetTable[widget].Filters & " + m_instanceName + "_CSHL_RAW_IIR_FILTER_1)");
            writer.WriteLine("                {");
            writer.WriteLine("                    baseRawData = filteredRawData;");
            writer.WriteLine("                    filteredRawData = " + m_instanceName + "_CSHL_IIRFilter(filteredRawData, ((" + m_instanceName + "_TPSettings *) ptr)->RawIIR[filterPos], " + m_instanceName + "_CSHL_IIR_FILTER_1);");
            writer.WriteLine("                    ((" + m_instanceName + "_TPSettings *) ptr)->RawIIR[filterPos] = baseRawData;");
            writer.WriteLine("                }");
            writer.WriteLine("                ");
            writer.WriteLine("                /* Jitter filter */");
            writer.WriteLine("                if(" + m_instanceName + "_CSHL_WidgetTable[widget].Filters & " + m_instanceName + "_CSHL_RAW_JITTER_FILTER)");
            writer.WriteLine("                {");
            writer.WriteLine("                    baseRawData = filteredRawData;");
            writer.WriteLine("                    filteredRawData = " + m_instanceName + "_CSHL_JitterFilter(filteredRawData, ((" + m_instanceName + "_TPSettings *) ptr)->RawJitter[filterPos]);");
            writer.WriteLine("                    ((" + m_instanceName + "_TPSettings *) ptr)->RawJitter[filterPos] = baseRawData;");
            writer.WriteLine("                }");
            writer.WriteLine("                break;");
            writer.WriteLine("                ");
            writer.WriteLine("            case " + m_instanceName + "_CSHL_TYPE_MATRIX_BUTTONS:");
            writer.WriteLine("                if(" + m_instanceName + "_CSHL_WidgetTable[widget].AdvancedSettings)");
            writer.WriteLine("                {");
            writer.WriteLine("                ptr = " + m_instanceName + "_CSHL_WidgetTable[widget].AdvancedSettings;");
            writer.WriteLine("                ");
            writer.WriteLine("                    /* Median filter */");
            writer.WriteLine("                    if(" + m_instanceName + "_CSHL_WidgetTable[widget].Filters & " + m_instanceName + "_CSHL_RAW_MEDIAN_FILTER)");
            writer.WriteLine("                    {");
            writer.WriteLine("                        baseRawData = filteredRawData;");
            writer.WriteLine("                        filteredRawData = " + m_instanceName + "_CSHL_MedianFilter(filteredRawData, ((" + m_instanceName + "_MBSettings *) ptr)->Raw1Median[filterPos], ((" + m_instanceName + "_MBSettings *) ptr)->Raw2Median[filterPos]);");
            writer.WriteLine("                        ((" + m_instanceName + "_MBSettings *) ptr)->Raw2Median[filterPos] = ((" + m_instanceName + "_MBSettings *) ptr)->Raw1Median[filterPos];");
            writer.WriteLine("                        ((" + m_instanceName + "_MBSettings *) ptr)->Raw1Median[filterPos] = baseRawData;");
            writer.WriteLine("                    }");
            writer.WriteLine("                    ");
            writer.WriteLine("                    /* Averaging filter */");
            writer.WriteLine("                    if(" + m_instanceName + "_CSHL_WidgetTable[widget].Filters & " + m_instanceName + "_CSHL_RAW_AVERAGING_FILTER)");
            writer.WriteLine("                    {");
            writer.WriteLine("                        baseRawData = filteredRawData;");
            writer.WriteLine("                        filteredRawData = " + m_instanceName + "_CSHL_AveragingFilter(filteredRawData, ((" + m_instanceName + "_MBSettings *) ptr)->Raw1Averaging[filterPos], ((" + m_instanceName + "_MBSettings *) ptr)->Raw2Averaging[filterPos]);");
            writer.WriteLine("                        ((" + m_instanceName + "_MBSettings *) ptr)->Raw2Averaging[filterPos] = ((" + m_instanceName + "_MBSettings *) ptr)->Raw1Averaging[filterPos];");
            writer.WriteLine("                        ((" + m_instanceName + "_MBSettings *) ptr)->Raw1Averaging[filterPos] = baseRawData;                ");
            writer.WriteLine("                    }");
            writer.WriteLine("                    ");
            writer.WriteLine("                    /* IIR filter */");
            writer.WriteLine("                    if(" + m_instanceName + "_CSHL_WidgetTable[widget].Filters & " + m_instanceName + "_CSHL_RAW_IIR_FILTER_0)");
            writer.WriteLine("                    {");
            writer.WriteLine("                        baseRawData = filteredRawData;");
            writer.WriteLine("                        filteredRawData = " + m_instanceName + "_CSHL_IIRFilter(filteredRawData, ((" + m_instanceName + "_MBSettings *) ptr)->RawIIR[filterPos], " + m_instanceName + "_CSHL_IIR_FILTER_0);");
            writer.WriteLine("                        ((" + m_instanceName + "_MBSettings *) ptr)->RawIIR[filterPos] = baseRawData;");
            writer.WriteLine("                    }");
            writer.WriteLine("                    else if(" + m_instanceName + "_CSHL_WidgetTable[widget].Filters & " + m_instanceName + "_CSHL_RAW_IIR_FILTER_1)");
            writer.WriteLine("                    {");
            writer.WriteLine("                        baseRawData = filteredRawData;");
            writer.WriteLine("                        filteredRawData = " + m_instanceName + "_CSHL_IIRFilter(filteredRawData, ((" + m_instanceName + "_MBSettings *) ptr)->RawIIR[filterPos], " + m_instanceName + "_CSHL_IIR_FILTER_1);");
            writer.WriteLine("                        ((" + m_instanceName + "_MBSettings *) ptr)->RawIIR[filterPos] = baseRawData;");
            writer.WriteLine("                    }");
            writer.WriteLine("                    ");
            writer.WriteLine("                    /* Jitter filter */");
            writer.WriteLine("                    if(" + m_instanceName + "_CSHL_WidgetTable[widget].Filters & " + m_instanceName + "_CSHL_RAW_JITTER_FILTER)");
            writer.WriteLine("                    {");
            writer.WriteLine("                        baseRawData = filteredRawData;");
            writer.WriteLine("                        filteredRawData = " + m_instanceName + "_CSHL_JitterFilter(filteredRawData, ((" + m_instanceName + "_MBSettings *) ptr)->RawJitter[filterPos]);");
            writer.WriteLine("                        ((" + m_instanceName + "_MBSettings *) ptr)->RawJitter[filterPos] = baseRawData;");
            writer.WriteLine("                    }");
            writer.WriteLine("                }");
            writer.WriteLine("                break;");
            writer.WriteLine("                ");
            writer.WriteLine("            default:");
            writer.WriteLine("                ");
            writer.WriteLine("                break;");
            writer.WriteLine("        }");
            writer.WriteLine("        ");
            writer.WriteLine("        /* Baseline calculation */");
            writer.WriteLine("        " + m_instanceName + "_SlotResult[rawIndex] = filteredRawData;");
            writer.WriteLine("        ");
            writer.WriteLine("        /* Find the Signal */");
            writer.WriteLine("        temp = ( (int32) filteredRawData) - ( (int32) " + m_instanceName + "_CSHL_SlotBaseline[rawIndex]);");
            writer.WriteLine("        if (temp < 0)");
            writer.WriteLine("        {");
            writer.WriteLine("            /* RawData less that Baseline */");
            writer.WriteLine("            " + m_instanceName + "_CSHL_SlotSignal[rawIndex] = 0;");
            writer.WriteLine("        }");
            writer.WriteLine("        ");
            writer.WriteLine("        /* Update Baseline if lower that NoiseThreshold */");
            writer.WriteLine("        if (temp < " + m_instanceName + "_CSHL_WidgetTable[widget].NoiseThreshold)");
            writer.WriteLine("        {");
            writer.WriteLine("            /* make full Baseline 23 bits */;");
            writer.WriteLine("            temp = ((((uint32) " + m_instanceName + "_CSHL_SlotBaseline[rawIndex]) << 8) | ((uint32) " + m_instanceName + "_CSHL_SlotBaselineLow[rawIndex]));");
            writer.WriteLine("            ");
            writer.WriteLine("            /* add Raw Data to Baseline */");
            writer.WriteLine("              temp += filteredRawData;");
            writer.WriteLine("            ");
            writer.WriteLine("            /* sub the high Baseline */");
            writer.WriteLine("            temp -= " + m_instanceName + "_CSHL_SlotBaseline[rawIndex];");
            writer.WriteLine("            ");
            writer.WriteLine("            /* Put Baseline and BaselineLow */");
            writer.WriteLine("            " + m_instanceName + "_CSHL_SlotBaselineLow[rawIndex] = ((uint8) temp);");
            writer.WriteLine("            " + m_instanceName + "_CSHL_SlotBaseline[rawIndex] = ((uint16) (temp >> 8));");
            writer.WriteLine("            ");
            writer.WriteLine("            /* Signal is zero */");
            writer.WriteLine("            " + m_instanceName + "_CSHL_SlotSignal[rawIndex] = 0;");
            writer.WriteLine("        }");
            writer.WriteLine("        else");
            writer.WriteLine("        {");
            writer.WriteLine("            temp -= " + m_instanceName + "_CSHL_WidgetTable[widget].NoiseThreshold;");
            writer.WriteLine("            ");
            writer.WriteLine("            if(temp < 0xFFu)");
            writer.WriteLine("            {");
            writer.WriteLine("                " + m_instanceName + "_CSHL_SlotSignal[rawIndex] = (uint8) temp;");
            writer.WriteLine("            }");
            writer.WriteLine("            else");
            writer.WriteLine("            {");
            writer.WriteLine("                " + m_instanceName + "_CSHL_SlotSignal[rawIndex] = 0xFFu;");
            writer.WriteLine("            }");
            writer.WriteLine("        }");
            writer.WriteLine("    }");
            writer.WriteLine("}");
            writer.WriteLine("");
            writer.WriteLine("");
            #endregion

            #region CSHL_UpdateAllBaselines Left/Right
            writer.WriteLine("/*******************************************************************************");
            writer.WriteLine("* Function Name:   " + m_instanceName + "_CSHL_UpdateAllBaselines" + sSide + "");
            writer.WriteLine("********************************************************************************");
            writer.WriteLine("* ");
            writer.WriteLine("* Summary:");
            writer.WriteLine("*  Uses the " + m_instanceName + "_CSHL_UpdateSlotBaselines" + sSide + " function to update the baselines");
            writer.WriteLine("*  for all slots. Raw data filters are applied to the values if enabled.");
            writer.WriteLine("* ");
            writer.WriteLine("* Parameters:");
            writer.WriteLine("*  void");
            writer.WriteLine("* ");
            writer.WriteLine("* Return:");
            writer.WriteLine("*  void");
            writer.WriteLine("* ");
            writer.WriteLine("**********************************************************************************/");
            writer.WriteLine(" void " + m_instanceName + "_CSHL_UpdateAllBaselines" + sSide + "(void)");
            writer.WriteLine("{");
            writer.WriteLine("    uint8 i;");
            writer.WriteLine("    ");
            writer.WriteLine("    for(i = 0; i < (" + m_instanceName + "_TOTAL_SCANSLOT_COUNT" + sSideUp + " - " + m_instanceName + "_TOTAL_GENERIC_SCANSLOT_COUNT" + sSideUp + "); i++)");
            writer.WriteLine("    {");
            writer.WriteLine("        " + m_instanceName + "_CSHL_UpdateSlotBaseline" + sSide + "(i);");
            writer.WriteLine("    }");
            writer.WriteLine("}");
            writer.WriteLine("");
            writer.WriteLine("");
            #endregion

            #region CSHL_CheckIsSlotActive Left/Right
            writer.WriteLine("/*******************************************************************************");
            writer.WriteLine("* Function Name:   " + m_instanceName + "_CSHL_CheckIsSlotActive" + sSide);
            writer.WriteLine("********************************************************************************");
            writer.WriteLine("* ");
            writer.WriteLine("* Summary:");
            writer.WriteLine("*  Compares the selected slot of the CapSense_CSHL_Signal[ ] array to its finger");
            writer.WriteLine("*  threshold. Hysteresis is taken into account. The Hysteresis value is added");
            writer.WriteLine("*  or subtracted from the finger threshold based on whether the slot is");
            writer.WriteLine("*  currently active. If the slot is active, the threshold is lowered by the");
            writer.WriteLine("*  hysteresis amount. If it is inactive, the threshold is raised by the");
            writer.WriteLine("*  hysteresis amount. This function also updates the slot's bit in the");
            writer.WriteLine("*  " + m_instanceName + "_CSHL_SlotOnMask[ ] array");
            writer.WriteLine("* ");
            writer.WriteLine("* Parameters:");
            writer.WriteLine("*  slot:  Scan slot number");
            writer.WriteLine("* ");
            writer.WriteLine("* Return:");
            writer.WriteLine("*  Returns scan slot state 1 if active, 0 if inactive");
            writer.WriteLine("* ");
            writer.WriteLine("**********************************************************************************/");
            writer.WriteLine("uint8 " + m_instanceName + "_CSHL_CheckIsSlotActive" + sSide + "(uint8 slot)");
            writer.WriteLine("{");
            writer.WriteLine("    uint8 rawIndex = " + m_instanceName + "_ScanSlotTable" + sSide + "[slot].RawIndex;");
            writer.WriteLine("    uint8 widget = " + m_instanceName + "_ScanSlotTable" + sSide + "[slot].WidgetNumber;");
            writer.WriteLine("    uint8 onMask = 0x01u;");
            writer.WriteLine("    ");
            writer.WriteLine("    if( widget != " + m_instanceName + "_CSHL_NO_WIDGET)");
            writer.WriteLine("    {");
            writer.WriteLine("        /* Get On/Off mask */");
            writer.WriteLine("        onMask <<= (rawIndex % 8);");
            writer.WriteLine("        ");
            writer.WriteLine("        /* Was on */");
            writer.WriteLine("        if (" + m_instanceName + "_CSHL_SlotOnMask[(rawIndex)/8] & onMask)");
            writer.WriteLine("        {");
            writer.WriteLine("            /* Hysteresis minus */");
            writer.WriteLine("            if (" + m_instanceName + "_CSHL_SlotSignal[rawIndex] < (" + m_instanceName + "_CSHL_WidgetTable[widget].FingerThreshold - " + m_instanceName + "_CSHL_WidgetTable[widget].Hysteresis))");
            writer.WriteLine("            {");
            writer.WriteLine("                " + m_instanceName + "_CSHL_SlotOnMask[(rawIndex)/8] &= ~onMask;");
            writer.WriteLine("                " + m_instanceName + "_ScanSlotTable" + sSide + "[slot].DebounceCount = " + m_instanceName + "_CSHL_WidgetTable[widget].Debounce;");
            writer.WriteLine("            }");
            writer.WriteLine("        }");
            writer.WriteLine("        else    /* Was off */");
            writer.WriteLine("        {");
            writer.WriteLine("            /* Hysteresis plus */");
            writer.WriteLine("            if (" + m_instanceName + "_CSHL_SlotSignal[rawIndex] > (" + m_instanceName + "_CSHL_WidgetTable[widget].FingerThreshold + " + m_instanceName + "_CSHL_WidgetTable[widget].Hysteresis))");
            writer.WriteLine("            {");
            writer.WriteLine("                if (" + m_instanceName + "_ScanSlotTable" + sSide + "[slot].DebounceCount-- == 0)");
            writer.WriteLine("                {");
            writer.WriteLine("                    /* Slot active */");
            writer.WriteLine("                    " + m_instanceName + "_CSHL_SlotOnMask[rawIndex/8] |= onMask;");
            writer.WriteLine("                }");
            writer.WriteLine("            }");
            writer.WriteLine("            else");
            writer.WriteLine("            {");
            writer.WriteLine("                /* Slot inactive - reset Debounce count  */");
            writer.WriteLine("                " + m_instanceName + "_ScanSlotTable" + sSide + "[slot].DebounceCount = " + m_instanceName + "_CSHL_WidgetTable[widget].Debounce;");
            writer.WriteLine("            }");
            writer.WriteLine("        }");
            writer.WriteLine("    }");
            writer.WriteLine("    ");
            writer.WriteLine("    return (" + m_instanceName + "_CSHL_SlotOnMask[rawIndex/8] & onMask) ? 1 : 0;");
            writer.WriteLine("}");
            writer.WriteLine("");
            writer.WriteLine("");
            #endregion

            #region CSHL_CheckIsAnySlotActive Left/Right
            writer.WriteLine("/*******************************************************************************");
            writer.WriteLine("* Function Name:   " + m_instanceName + "_CSHL_CheckIsAnySlotActive" + sSide);
            writer.WriteLine("********************************************************************************");
            writer.WriteLine("* ");
            writer.WriteLine("* Summary:");
            writer.WriteLine("*  Compares all slots of the CapSense_CSHL_Signal[ ] array to their finger");
            writer.WriteLine("*  threshold. Calls " + m_instanceName + "_CSHL_CheckIsAnySlotActive" + sSide + " for each slot so");
            writer.WriteLine("*  the CapSense_CSHL_SlotOnMask[ ] array is up to date after calling this");
            writer.WriteLine("*  function.");
            writer.WriteLine("* ");
            writer.WriteLine("* Parameters:");
            writer.WriteLine("*  slot:  Scan slot number");
            writer.WriteLine("* ");
            writer.WriteLine("* Return:");
            writer.WriteLine("*  Returns 1 if any scan slot is active, 0 none of scan slots are active");
            writer.WriteLine("* ");
            writer.WriteLine("**********************************************************************************/");
            writer.WriteLine("uint8 " + m_instanceName + "_CSHL_CheckIsAnySlotActive" + sSide + "(void)");
            writer.WriteLine("{");
            writer.WriteLine("    uint8 i, result=0;");
            writer.WriteLine("    ");
            writer.WriteLine("    for(i = 0; i< " + m_instanceName + "_TOTAL_SCANSLOT_COUNT" + sSideUp + "; i++)");
            writer.WriteLine("    {");
            writer.WriteLine("        if (" + m_instanceName + "_CSHL_CheckIsSlotActive" + sSide + "(i) == " + m_instanceName + "_CSHL_SLOT_ACTIVE)");
            writer.WriteLine("        {");
            writer.WriteLine("            result = " + m_instanceName + "_CSHL_SLOT_ACTIVE;");
            writer.WriteLine("        }");
            writer.WriteLine("    }");
            writer.WriteLine("    ");
            writer.WriteLine("    return result;");
            writer.WriteLine("}");
            writer.WriteLine("");
            writer.WriteLine("");
            #endregion

        }
        #endregion

        #region  apiCollectCHLFunctionBase
        public void ApiCollectCHLFunctionBase(ref CyMyStringWriter writer, CyGeneralParams packParams)
        {

            if (packParams.Configuration != E_MAIN_CONFIG.emSerial)
            {
                #region CSHL_InitializeAllBaselines
                writer.WriteLine("/*******************************************************************************");
                writer.WriteLine("* Function Name:   " + m_instanceName + "_CSHL_InitializeAllBaselines");
                writer.WriteLine("********************************************************************************");
                writer.WriteLine("* ");
                writer.WriteLine("* Summary:");
                writer.WriteLine("*  Loads the " + m_instanceName + "_CSHL_SlotBaseline[] array with an initial values");
                writer.WriteLine("*  by scanning in parallel mode all slots. The raw count values are copied");
                writer.WriteLine("*  into the baseline array for all slots. The raw data filters are ");
                writer.WriteLine("*  initialized if enabled.");
                writer.WriteLine("* ");
                writer.WriteLine("* Parameters:");
                writer.WriteLine("*  void");
                writer.WriteLine("* ");
                writer.WriteLine("* Return:");
                writer.WriteLine("*  void");
                writer.WriteLine("* ");
                writer.WriteLine("**********************************************************************************/");
                writer.WriteLine("void " + m_instanceName + "_CSHL_InitializeAllBaselines(void)");
                writer.WriteLine("{");
                if (packParams.m_localParams.IsParallelFull())
                {
                    #region PapallelFunc
                    writer.WriteLine("    void *ptr;");
                    writer.WriteLine("    uint8 rawIndex, widget, filterPos, i;");
                    writer.WriteLine("    i = 0;");
                    writer.WriteLine("    ");
                    writer.WriteLine("    " + m_instanceName + "_ScanAllSlots();");
                    writer.WriteLine("    ");
                    writer.WriteLine("    while ((i < " + m_instanceName + "_TOTAL_SCANSLOT_COUNT_LEFT) || (i < " + m_instanceName + "_TOTAL_SCANSLOT_COUNT_RIGHT))");
                    writer.WriteLine("    {");
                    writer.WriteLine("        if (i < " + m_instanceName + "_TOTAL_SCANSLOT_COUNT_LEFT)");
                    writer.WriteLine("        {");
                    writer.WriteLine("            rawIndex = " + m_instanceName + "_ScanSlotTableLeft[i].RawIndex;");
                    writer.WriteLine("            widget = " + m_instanceName + "_ScanSlotTableLeft[i].WidgetNumber;");
                    writer.WriteLine("            filterPos = rawIndex - " + m_instanceName + "_CSHL_WidgetTable[widget].RawOffset;");
                    writer.WriteLine("            ");
                    writer.WriteLine("            if( widget != " + m_instanceName + "_CSHL_NO_WIDGET)");
                    writer.WriteLine("            {");
                    writer.WriteLine("                /* Initialize Baseline */");
                    writer.WriteLine("                " + m_instanceName + "_CSHL_SlotBaseline[rawIndex] = " + m_instanceName + "_SlotResult[rawIndex];");
                    writer.WriteLine("                " + m_instanceName + "_CSHL_SlotBaselineLow[rawIndex] = 0;");
                    writer.WriteLine("                " + m_instanceName + "_CSHL_SlotSignal[rawIndex] = 0;");
                    writer.WriteLine("                " + m_instanceName + "_ScanSlotTableLeft[i].DebounceCount = " + m_instanceName + "_CSHL_WidgetTable[widget].Debounce;");
                    writer.WriteLine("                ");
                    writer.WriteLine("                switch ((" + m_instanceName + "_CSHL_WidgetTable[widget].Type & (~" + m_instanceName + "_CSHL_IS_DIPLEX)))");
                    writer.WriteLine("                {");
                    writer.WriteLine("                    /* This case include BTN and PROX */");
                    writer.WriteLine("                    case " + m_instanceName + "_CSHL_TYPE_BUTTON:");
                    writer.WriteLine("                    case " + m_instanceName + "_CSHL_TYPE_PROXIMITY:");
                    writer.WriteLine("                        if(" + m_instanceName + "_CSHL_WidgetTable[widget].AdvancedSettings)");
                    writer.WriteLine("                        {");
                    writer.WriteLine("                            ptr = " + m_instanceName + "_CSHL_WidgetTable[widget].AdvancedSettings;");
                    writer.WriteLine("                            ");
                    writer.WriteLine("                            /* Median filter first time initialization */");
                    writer.WriteLine("                            if(" + m_instanceName + "_CSHL_WidgetTable[widget].Filters & " + m_instanceName + "_CSHL_RAW_MEDIAN_FILTER)");
                    writer.WriteLine("                            {");
                    writer.WriteLine("                                ((" + m_instanceName + "_BtnSettings *) ptr)->Raw2Median = " + m_instanceName + "_SlotResult[rawIndex];");
                    writer.WriteLine("                                ((" + m_instanceName + "_BtnSettings *) ptr)->Raw1Median = " + m_instanceName + "_SlotResult[rawIndex];");
                    writer.WriteLine("                            }");
                    writer.WriteLine("                            ");
                    writer.WriteLine("                            /* Averaging filter first time initialization */");
                    writer.WriteLine("                            if(" + m_instanceName + "_CSHL_WidgetTable[widget].Filters & " + m_instanceName + "_CSHL_RAW_AVERAGING_FILTER)");
                    writer.WriteLine("                            {");
                    writer.WriteLine("                                ((" + m_instanceName + "_BtnSettings *) ptr)->Raw2Averaging = " + m_instanceName + "_SlotResult[rawIndex];");
                    writer.WriteLine("                                ((" + m_instanceName + "_BtnSettings *) ptr)->Raw1Averaging = " + m_instanceName + "_SlotResult[rawIndex];        ");
                    writer.WriteLine("                            }");
                    writer.WriteLine("                            ");
                    writer.WriteLine("                            /* IIR filter first time initialization */");
                    writer.WriteLine("                            if((" + m_instanceName + "_CSHL_WidgetTable[widget].Filters & " + m_instanceName + "_CSHL_RAW_IIR_FILTER_0) ||");
                    writer.WriteLine("                            (" + m_instanceName + "_CSHL_WidgetTable[widget].Filters & " + m_instanceName + "_CSHL_RAW_IIR_FILTER_1))");
                    writer.WriteLine("                            {");
                    writer.WriteLine("                                ((" + m_instanceName + "_BtnSettings *) ptr)->RawIIR = " + m_instanceName + "_SlotResult[rawIndex];");
                    writer.WriteLine("                            }");
                    writer.WriteLine("                            ");
                    writer.WriteLine("                            /* Jitter filter first time initialization */");
                    writer.WriteLine("                            if(" + m_instanceName + "_CSHL_WidgetTable[widget].Filters & " + m_instanceName + "_CSHL_RAW_JITTER_FILTER)");
                    writer.WriteLine("                            {");
                    writer.WriteLine("                                ((" + m_instanceName + "_BtnSettings *) ptr)->RawJitter = " + m_instanceName + "_SlotResult[rawIndex];");
                    writer.WriteLine("                            }");
                    writer.WriteLine("                        }");
                    writer.WriteLine("                        break;");
                    writer.WriteLine("                    ");
                    writer.WriteLine("                    /* This case include LINEAR and RADIAL SL */");
                    writer.WriteLine("                    case " + m_instanceName + "_CSHL_TYPE_RADIAL_SLIDER :");
                    writer.WriteLine("                    case " + m_instanceName + "_CSHL_TYPE_LINEAR_SLIDER :");
                    writer.WriteLine("                        ptr = " + m_instanceName + "_CSHL_WidgetTable[widget].AdvancedSettings;");
                    writer.WriteLine("                        ((" + m_instanceName + "_SlSettings *) ptr)->FirstTime=0;");
                    writer.WriteLine("                        ");
                    writer.WriteLine("                        /* Median filter first time initialization */");
                    writer.WriteLine("                        if(" + m_instanceName + "_CSHL_WidgetTable[widget].Filters & " + m_instanceName + "_CSHL_RAW_MEDIAN_FILTER)");
                    writer.WriteLine("                        {");
                    writer.WriteLine("                            ((" + m_instanceName + "_SlSettings *) ptr)->Raw2Median[filterPos] = " + m_instanceName + "_SlotResult[rawIndex];");
                    writer.WriteLine("                            ((" + m_instanceName + "_SlSettings *) ptr)->Raw1Median[filterPos] = " + m_instanceName + "_SlotResult[rawIndex];");
                    writer.WriteLine("                        }");
                    writer.WriteLine("                        ");
                    writer.WriteLine("                        /* Averaging filter first time initialization */");
                    writer.WriteLine("                        if(" + m_instanceName + "_CSHL_WidgetTable[widget].Filters & " + m_instanceName + "_CSHL_RAW_AVERAGING_FILTER)");
                    writer.WriteLine("                        {");
                    writer.WriteLine("                            ((" + m_instanceName + "_SlSettings *) ptr)->Raw2Averaging[filterPos] = " + m_instanceName + "_SlotResult[rawIndex];");
                    writer.WriteLine("                            ((" + m_instanceName + "_SlSettings *) ptr)->Raw1Averaging[filterPos] = " + m_instanceName + "_SlotResult[rawIndex];        ");
                    writer.WriteLine("                        }");
                    writer.WriteLine("                        ");
                    writer.WriteLine("                        /* IIR filter first time initialization */");
                    writer.WriteLine("                        if((" + m_instanceName + "_CSHL_WidgetTable[widget].Filters & " + m_instanceName + "_CSHL_RAW_IIR_FILTER_0) ||");
                    writer.WriteLine("                        (" + m_instanceName + "_CSHL_WidgetTable[widget].Filters & " + m_instanceName + "_CSHL_RAW_IIR_FILTER_1))");
                    writer.WriteLine("                        {");
                    writer.WriteLine("                            ((" + m_instanceName + "_SlSettings *) ptr)->RawIIR[filterPos] = " + m_instanceName + "_SlotResult[rawIndex];");
                    writer.WriteLine("                        }");
                    writer.WriteLine("                        ");
                    writer.WriteLine("                        /* Jitter filter first time initialization */");
                    writer.WriteLine("                        if(" + m_instanceName + "_CSHL_WidgetTable[widget].Filters & " + m_instanceName + "_CSHL_RAW_JITTER_FILTER)");
                    writer.WriteLine("                        {");
                    writer.WriteLine("                            ((" + m_instanceName + "_SlSettings *) ptr)->RawJitter[filterPos] = " + m_instanceName + "_SlotResult[rawIndex];");
                    writer.WriteLine("                        }");
                    writer.WriteLine("                        break;");
                    writer.WriteLine("                        ");
                    writer.WriteLine("                    case " + m_instanceName + "_CSHL_TYPE_TOUCHPAD:");
                    writer.WriteLine("                        ptr = " + m_instanceName + "_CSHL_WidgetTable[widget].AdvancedSettings;");
                    writer.WriteLine("                        ((" + m_instanceName + "_TPSettings *) ptr)->FirstTime=0;");
                    writer.WriteLine("                        ");
                    writer.WriteLine("                        /* Median filter first time initialization */");
                    writer.WriteLine("                        if(" + m_instanceName + "_CSHL_WidgetTable[widget].Filters & " + m_instanceName + "_CSHL_RAW_MEDIAN_FILTER)");
                    writer.WriteLine("                        {");
                    writer.WriteLine("                            ((" + m_instanceName + "_TPSettings *) ptr)->Raw2Median[filterPos] = " + m_instanceName + "_SlotResult[rawIndex];");
                    writer.WriteLine("                            ((" + m_instanceName + "_TPSettings *) ptr)->Raw1Median[filterPos] = " + m_instanceName + "_SlotResult[rawIndex];");
                    writer.WriteLine("                        }");
                    writer.WriteLine("                        ");
                    writer.WriteLine("                        /* Averaging filter first time initialization */");
                    writer.WriteLine("                        if(" + m_instanceName + "_CSHL_WidgetTable[widget].Filters & " + m_instanceName + "_CSHL_RAW_AVERAGING_FILTER)");
                    writer.WriteLine("                        {");
                    writer.WriteLine("                            ((" + m_instanceName + "_TPSettings *) ptr)->Raw2Averaging[filterPos] = " + m_instanceName + "_SlotResult[rawIndex];");
                    writer.WriteLine("                            ((" + m_instanceName + "_TPSettings *) ptr)->Raw1Averaging[filterPos] = " + m_instanceName + "_SlotResult[rawIndex];        ");
                    writer.WriteLine("                        }");
                    writer.WriteLine("                        ");
                    writer.WriteLine("                        /* IIR filter first time initialization */");
                    writer.WriteLine("                        if((" + m_instanceName + "_CSHL_WidgetTable[widget].Filters & " + m_instanceName + "_CSHL_RAW_IIR_FILTER_0) ||");
                    writer.WriteLine("                        (" + m_instanceName + "_CSHL_WidgetTable[widget].Filters & " + m_instanceName + "_CSHL_RAW_IIR_FILTER_1))");
                    writer.WriteLine("                        {");
                    writer.WriteLine("                            ((" + m_instanceName + "_TPSettings *) ptr)->RawIIR[filterPos] = " + m_instanceName + "_SlotResult[rawIndex];");
                    writer.WriteLine("                        }");
                    writer.WriteLine("                        ");
                    writer.WriteLine("                        /* Jitter filter first time initialization */");
                    writer.WriteLine("                        if(" + m_instanceName + "_CSHL_WidgetTable[widget].Filters & " + m_instanceName + "_CSHL_RAW_JITTER_FILTER)");
                    writer.WriteLine("                        {");
                    writer.WriteLine("                            ((" + m_instanceName + "_TPSettings *) ptr)->RawJitter[filterPos] = " + m_instanceName + "_SlotResult[rawIndex];");
                    writer.WriteLine("                        }");
                    writer.WriteLine("                        break;");
                    writer.WriteLine("                        ");
                    writer.WriteLine("                    case " + m_instanceName + "_CSHL_TYPE_MATRIX_BUTTONS:");
                    writer.WriteLine("                        if(" + m_instanceName + "_CSHL_WidgetTable[widget].AdvancedSettings)");
                    writer.WriteLine("                        {");
                    writer.WriteLine("                            ptr = " + m_instanceName + "_CSHL_WidgetTable[widget].AdvancedSettings;");
                    writer.WriteLine("                            ");
                    writer.WriteLine("                            /* Median filter first time initialization */");
                    writer.WriteLine("                            if(" + m_instanceName + "_CSHL_WidgetTable[widget].Filters & " + m_instanceName + "_CSHL_RAW_MEDIAN_FILTER)");
                    writer.WriteLine("                            {");
                    writer.WriteLine("                                ((" + m_instanceName + "_MBSettings *) ptr)->Raw2Median[filterPos] = " + m_instanceName + "_SlotResult[rawIndex];");
                    writer.WriteLine("                                ((" + m_instanceName + "_MBSettings *) ptr)->Raw1Median[filterPos] = " + m_instanceName + "_SlotResult[rawIndex];");
                    writer.WriteLine("                            }");
                    writer.WriteLine("                            ");
                    writer.WriteLine("                            /* Averaging filter first time initialization */");
                    writer.WriteLine("                            if(" + m_instanceName + "_CSHL_WidgetTable[widget].Filters & " + m_instanceName + "_CSHL_RAW_AVERAGING_FILTER)");
                    writer.WriteLine("                            {");
                    writer.WriteLine("                                ((" + m_instanceName + "_MBSettings *) ptr)->Raw2Averaging[filterPos] = " + m_instanceName + "_SlotResult[rawIndex];");
                    writer.WriteLine("                                ((" + m_instanceName + "_MBSettings *) ptr)->Raw1Averaging[filterPos] = " + m_instanceName + "_SlotResult[rawIndex];        ");
                    writer.WriteLine("                            }");
                    writer.WriteLine("                            ");
                    writer.WriteLine("                            /* IIR filter first time initialization */");
                    writer.WriteLine("                            if((" + m_instanceName + "_CSHL_WidgetTable[widget].Filters & " + m_instanceName + "_CSHL_RAW_IIR_FILTER_0) ||");
                    writer.WriteLine("                            (" + m_instanceName + "_CSHL_WidgetTable[widget].Filters & " + m_instanceName + "_CSHL_RAW_IIR_FILTER_1))");
                    writer.WriteLine("                            {");
                    writer.WriteLine("                                ((" + m_instanceName + "_MBSettings *) ptr)->RawIIR[filterPos] = " + m_instanceName + "_SlotResult[rawIndex];");
                    writer.WriteLine("                            }");
                    writer.WriteLine("                            ");
                    writer.WriteLine("                            /* Jitter filter first time initialization */");
                    writer.WriteLine("                            if(" + m_instanceName + "_CSHL_WidgetTable[widget].Filters & " + m_instanceName + "_CSHL_RAW_JITTER_FILTER)");
                    writer.WriteLine("                            {");
                    writer.WriteLine("                                ((" + m_instanceName + "_MBSettings *) ptr)->RawJitter[filterPos] = " + m_instanceName + "_SlotResult[rawIndex];");
                    writer.WriteLine("                            }");
                    writer.WriteLine("                        }");
                    writer.WriteLine("                        break;");
                    writer.WriteLine("                                ");
                    writer.WriteLine("                    default:");
                    writer.WriteLine("                    ");
                    writer.WriteLine("                        break;");
                    writer.WriteLine("                }");
                    writer.WriteLine("            }");
                    writer.WriteLine("        }");
                    writer.WriteLine("        ");
                    writer.WriteLine("        if (i < " + m_instanceName + "_TOTAL_SCANSLOT_COUNT_RIGHT)");
                    writer.WriteLine("        {");
                    writer.WriteLine("            rawIndex = " + m_instanceName + "_ScanSlotTableRight[i].RawIndex;");
                    writer.WriteLine("            widget = " + m_instanceName + "_ScanSlotTableRight[i].WidgetNumber;");
                    writer.WriteLine("            filterPos = rawIndex - " + m_instanceName + "_CSHL_WidgetTable[widget].RawOffset;");
                    writer.WriteLine("            ");
                    writer.WriteLine("            if( widget != " + m_instanceName + "_CSHL_NO_WIDGET)");
                    writer.WriteLine("            {");
                    writer.WriteLine("                ");
                    writer.WriteLine("                /* Initialize Baseline */");
                    writer.WriteLine("                " + m_instanceName + "_CSHL_SlotBaseline[rawIndex] = " + m_instanceName + "_SlotResult[rawIndex];");
                    writer.WriteLine("                " + m_instanceName + "_CSHL_SlotBaselineLow[rawIndex] = 0;");
                    writer.WriteLine("                " + m_instanceName + "_CSHL_SlotSignal[rawIndex] = 0;");
                    writer.WriteLine("                " + m_instanceName + "_ScanSlotTableLeft[i].DebounceCount = " + m_instanceName + "_CSHL_WidgetTable[widget].Debounce;");
                    writer.WriteLine("                ");
                    writer.WriteLine("                switch ((" + m_instanceName + "_CSHL_WidgetTable[widget].Type & (~" + m_instanceName + "_CSHL_IS_DIPLEX)))");
                    writer.WriteLine("                {");
                    writer.WriteLine("                    /* This case include BTN and PROX */");
                    writer.WriteLine("                    case " + m_instanceName + "_CSHL_TYPE_BUTTON:");
                    writer.WriteLine("                    case " + m_instanceName + "_CSHL_TYPE_PROXIMITY:");
                    writer.WriteLine("                        if(" + m_instanceName + "_CSHL_WidgetTable[widget].AdvancedSettings)");
                    writer.WriteLine("                        {");
                    writer.WriteLine("                            ptr = " + m_instanceName + "_CSHL_WidgetTable[widget].AdvancedSettings;");
                    writer.WriteLine("                            ");
                    writer.WriteLine("                            /* Median filter first time initialization */");
                    writer.WriteLine("                            if(" + m_instanceName + "_CSHL_WidgetTable[widget].Filters & " + m_instanceName + "_CSHL_RAW_MEDIAN_FILTER)");
                    writer.WriteLine("                            {");
                    writer.WriteLine("                                ((" + m_instanceName + "_BtnSettings *) ptr)->Raw2Median = " + m_instanceName + "_SlotResult[rawIndex];");
                    writer.WriteLine("                                ((" + m_instanceName + "_BtnSettings *) ptr)->Raw1Median = " + m_instanceName + "_SlotResult[rawIndex];");
                    writer.WriteLine("                            }");
                    writer.WriteLine("                            ");
                    writer.WriteLine("                            /* Averaging filter first time initialization */");
                    writer.WriteLine("                            if(" + m_instanceName + "_CSHL_WidgetTable[widget].Filters & " + m_instanceName + "_CSHL_RAW_AVERAGING_FILTER)");
                    writer.WriteLine("                            {");
                    writer.WriteLine("                                ((" + m_instanceName + "_BtnSettings *) ptr)->Raw2Averaging = " + m_instanceName + "_SlotResult[rawIndex];");
                    writer.WriteLine("                                ((" + m_instanceName + "_BtnSettings *) ptr)->Raw1Averaging = " + m_instanceName + "_SlotResult[rawIndex];");
                    writer.WriteLine("                            }");
                    writer.WriteLine("                            ");
                    writer.WriteLine("                            /* IIR filter first time initialization */");
                    writer.WriteLine("                            if((" + m_instanceName + "_CSHL_WidgetTable[widget].Filters & " + m_instanceName + "_CSHL_RAW_IIR_FILTER_0) ||");
                    writer.WriteLine("                            (" + m_instanceName + "_CSHL_WidgetTable[widget].Filters & " + m_instanceName + "_CSHL_RAW_IIR_FILTER_1))");
                    writer.WriteLine("                            {");
                    writer.WriteLine("                                ((" + m_instanceName + "_MBSettings *) ptr)->RawIIR[filterPos] = " + m_instanceName + "_SlotResult[rawIndex];");
                    writer.WriteLine("                            }");
                    writer.WriteLine("                            ");
                    writer.WriteLine("                            /* Jitter filter first time initialization */");
                    writer.WriteLine("                            if(" + m_instanceName + "_CSHL_WidgetTable[widget].Filters & " + m_instanceName + "_CSHL_RAW_JITTER_FILTER)");
                    writer.WriteLine("                            {");
                    writer.WriteLine("                                ((" + m_instanceName + "_BtnSettings *) ptr)->RawJitter = " + m_instanceName + "_SlotResult[rawIndex];");
                    writer.WriteLine("                            }");
                    writer.WriteLine("                        }");
                    writer.WriteLine("                        break;");
                    writer.WriteLine("                    ");
                    writer.WriteLine("                    /* This case include LINEAR and RADIAL SL */");
                    writer.WriteLine("                    case " + m_instanceName + "_CSHL_TYPE_RADIAL_SLIDER :");
                    writer.WriteLine("                    case " + m_instanceName + "_CSHL_TYPE_LINEAR_SLIDER :");
                    writer.WriteLine("                        ");
                    writer.WriteLine("                        ptr = " + m_instanceName + "_CSHL_WidgetTable[widget].AdvancedSettings;");
                    writer.WriteLine("                        ((" + m_instanceName + "_SlSettings *) ptr)->FirstTime=0;");
                    writer.WriteLine("                        ");
                    writer.WriteLine("                        /* Median filter first time initialization */");
                    writer.WriteLine("                        if(" + m_instanceName + "_CSHL_WidgetTable[widget].Filters & " + m_instanceName + "_CSHL_RAW_MEDIAN_FILTER)");
                    writer.WriteLine("                        {");
                    writer.WriteLine("                            ((" + m_instanceName + "_SlSettings *) ptr)->Raw2Median[filterPos] = " + m_instanceName + "_SlotResult[rawIndex];");
                    writer.WriteLine("                            ((" + m_instanceName + "_SlSettings *) ptr)->Raw1Median[filterPos] = " + m_instanceName + "_SlotResult[rawIndex];");
                    writer.WriteLine("                        }");
                    writer.WriteLine("                        ");
                    writer.WriteLine("                        /* Averaging filter first time initialization */");
                    writer.WriteLine("                        if(" + m_instanceName + "_CSHL_WidgetTable[widget].Filters & " + m_instanceName + "_CSHL_RAW_AVERAGING_FILTER)");
                    writer.WriteLine("                        {");
                    writer.WriteLine("                            ((" + m_instanceName + "_SlSettings *) ptr)->Raw2Averaging[filterPos] = " + m_instanceName + "_SlotResult[rawIndex];");
                    writer.WriteLine("                            ((" + m_instanceName + "_SlSettings *) ptr)->Raw1Averaging[filterPos] = " + m_instanceName + "_SlotResult[rawIndex];");
                    writer.WriteLine("                        }");
                    writer.WriteLine("                        ");
                    writer.WriteLine("                        /* IIR filter first time initialization */");
                    writer.WriteLine("                        if((" + m_instanceName + "_CSHL_WidgetTable[widget].Filters & " + m_instanceName + "_CSHL_RAW_IIR_FILTER_0) ||");
                    writer.WriteLine("                            (" + m_instanceName + "_CSHL_WidgetTable[widget].Filters & " + m_instanceName + "_CSHL_RAW_IIR_FILTER_1))");
                    writer.WriteLine("                        {");
                    writer.WriteLine("                            ((" + m_instanceName + "_MBSettings *) ptr)->RawIIR[filterPos] = " + m_instanceName + "_SlotResult[rawIndex];");
                    writer.WriteLine("                        }");
                    writer.WriteLine("                        ");
                    writer.WriteLine("                        /* Jitter filter first time initialization */");
                    writer.WriteLine("                        if(" + m_instanceName + "_CSHL_WidgetTable[widget].Filters & " + m_instanceName + "_CSHL_RAW_JITTER_FILTER)");
                    writer.WriteLine("                        {");
                    writer.WriteLine("                            ((" + m_instanceName + "_SlSettings *) ptr)->RawJitter[filterPos] = " + m_instanceName + "_SlotResult[rawIndex];");
                    writer.WriteLine("                        }");
                    writer.WriteLine("                        break;");
                    writer.WriteLine("                        ");
                    writer.WriteLine("                    case " + m_instanceName + "_CSHL_TYPE_TOUCHPAD:");
                    writer.WriteLine("                        ");
                    writer.WriteLine("                        ptr = " + m_instanceName + "_CSHL_WidgetTable[widget].AdvancedSettings;");
                    writer.WriteLine("                        ((" + m_instanceName + "_TPSettings *) ptr)->FirstTime=0;");
                    writer.WriteLine("                        ");
                    writer.WriteLine("                        /* Median filter first time initialization */");
                    writer.WriteLine("                        if(" + m_instanceName + "_CSHL_WidgetTable[widget].Filters & " + m_instanceName + "_CSHL_RAW_MEDIAN_FILTER)");
                    writer.WriteLine("                        {");
                    writer.WriteLine("                            ((" + m_instanceName + "_TPSettings *) ptr)->Raw2Median[filterPos] = " + m_instanceName + "_SlotResult[rawIndex];");
                    writer.WriteLine("                            ((" + m_instanceName + "_TPSettings *) ptr)->Raw1Median[filterPos] = " + m_instanceName + "_SlotResult[rawIndex];");
                    writer.WriteLine("                        }");
                    writer.WriteLine("                        ");
                    writer.WriteLine("                        /* Averaging filter first time initialization */");
                    writer.WriteLine("                        if(" + m_instanceName + "_CSHL_WidgetTable[widget].Filters & " + m_instanceName + "_CSHL_RAW_AVERAGING_FILTER)");
                    writer.WriteLine("                        {");
                    writer.WriteLine("                            ((" + m_instanceName + "_TPSettings *) ptr)->Raw2Averaging[filterPos] = " + m_instanceName + "_SlotResult[rawIndex];");
                    writer.WriteLine("                            ((" + m_instanceName + "_TPSettings *) ptr)->Raw1Averaging[filterPos] = " + m_instanceName + "_SlotResult[rawIndex];");
                    writer.WriteLine("                        }");
                    writer.WriteLine("                        ");
                    writer.WriteLine("                        /* IIR filter first time initialization */");
                    writer.WriteLine("                        if((" + m_instanceName + "_CSHL_WidgetTable[widget].Filters & " + m_instanceName + "_CSHL_RAW_IIR_FILTER_0) ||");
                    writer.WriteLine("                            (" + m_instanceName + "_CSHL_WidgetTable[widget].Filters & " + m_instanceName + "_CSHL_RAW_IIR_FILTER_1))");
                    writer.WriteLine("                        {");
                    writer.WriteLine("                            ((" + m_instanceName + "_MBSettings *) ptr)->RawIIR[filterPos] = " + m_instanceName + "_SlotResult[rawIndex];");
                    writer.WriteLine("                        }");
                    writer.WriteLine("                        ");
                    writer.WriteLine("                        /* Jitter filter first time initialization */");
                    writer.WriteLine("                        if(" + m_instanceName + "_CSHL_WidgetTable[widget].Filters & " + m_instanceName + "_CSHL_RAW_JITTER_FILTER)");
                    writer.WriteLine("                        {");
                    writer.WriteLine("                            ((" + m_instanceName + "_TPSettings *) ptr)->RawJitter[filterPos] = " + m_instanceName + "_SlotResult[rawIndex];");
                    writer.WriteLine("                        }");
                    writer.WriteLine("                        ");
                    writer.WriteLine("                        break;");
                    writer.WriteLine("                        ");
                    writer.WriteLine("                    case " + m_instanceName + "_CSHL_TYPE_MATRIX_BUTTONS:");
                    writer.WriteLine("                        if(" + m_instanceName + "_CSHL_WidgetTable[widget].AdvancedSettings)");
                    writer.WriteLine("                        {");
                    writer.WriteLine("                            ptr = " + m_instanceName + "_CSHL_WidgetTable[widget].AdvancedSettings;");
                    writer.WriteLine("                            ");
                    writer.WriteLine("                            /* Median filter first time initialization */");
                    writer.WriteLine("                            if(" + m_instanceName + "_CSHL_WidgetTable[widget].Filters & " + m_instanceName + "_CSHL_RAW_MEDIAN_FILTER)");
                    writer.WriteLine("                            {");
                    writer.WriteLine("                                ((" + m_instanceName + "_MBSettings *) ptr)->Raw2Median[filterPos] = " + m_instanceName + "_SlotResult[rawIndex];");
                    writer.WriteLine("                                ((" + m_instanceName + "_MBSettings *) ptr)->Raw1Median[filterPos] = " + m_instanceName + "_SlotResult[rawIndex];");
                    writer.WriteLine("                            }");
                    writer.WriteLine("                            ");
                    writer.WriteLine("                            /* Averaging filter first time initialization */");
                    writer.WriteLine("                            if(" + m_instanceName + "_CSHL_WidgetTable[widget].Filters & " + m_instanceName + "_CSHL_RAW_AVERAGING_FILTER)");
                    writer.WriteLine("                            {");
                    writer.WriteLine("                                ((" + m_instanceName + "_MBSettings *) ptr)->Raw2Averaging[filterPos] = " + m_instanceName + "_SlotResult[rawIndex];");
                    writer.WriteLine("                                ((" + m_instanceName + "_MBSettings *) ptr)->Raw1Averaging[filterPos] = " + m_instanceName + "_SlotResult[rawIndex];");
                    writer.WriteLine("                            }");
                    writer.WriteLine("                            ");
                    writer.WriteLine("                            /* IIR filter first time initialization */");
                    writer.WriteLine("                            if((" + m_instanceName + "_CSHL_WidgetTable[widget].Filters & " + m_instanceName + "_CSHL_RAW_IIR_FILTER_0) ||");
                    writer.WriteLine("                                (" + m_instanceName + "_CSHL_WidgetTable[widget].Filters & " + m_instanceName + "_CSHL_RAW_IIR_FILTER_1))");
                    writer.WriteLine("                            {");
                    writer.WriteLine("                                ((" + m_instanceName + "_MBSettings *) ptr)->RawIIR[filterPos] = " + m_instanceName + "_SlotResult[rawIndex];");
                    writer.WriteLine("                            }");
                    writer.WriteLine("                            ");
                    writer.WriteLine("                            /* Jitter filter first time initialization */");
                    writer.WriteLine("                            if(" + m_instanceName + "_CSHL_WidgetTable[widget].Filters & " + m_instanceName + "_CSHL_RAW_JITTER_FILTER)");
                    writer.WriteLine("                            {");
                    writer.WriteLine("                                ((" + m_instanceName + "_MBSettings *) ptr)->RawJitter[filterPos] = " + m_instanceName + "_SlotResult[rawIndex];");
                    writer.WriteLine("                            }");
                    writer.WriteLine("                        }");
                    writer.WriteLine("                        break;");
                    writer.WriteLine("                    default:");
                    writer.WriteLine("                    ");
                    writer.WriteLine("                        break;");
                    writer.WriteLine("                }");
                    writer.WriteLine("            }");
                    writer.WriteLine("        }");
                    writer.WriteLine("        ");
                    writer.WriteLine("        /* go to the next slot */");
                    writer.WriteLine("        i++;");
                    writer.WriteLine("        ");
                    writer.WriteLine("    }");
                    #endregion
                }
                else
                {
                    foreach (CyAmuxBParams sbItem in packParams.m_localParams.m_listCsHalfs)
                        if (packParams.m_localParams.IsAmuxBusEnable(sbItem))
                        {
                            E_EL_SIDE side = sbItem.m_side;
                            writer.WriteLine("    " + m_instanceName + "_CSHL_InitializeAllBaselines" + side + "();");
                        }
                }
                writer.WriteLine("}");
                writer.WriteLine("");
                writer.WriteLine("");
                #endregion

                #region CSHL_UpdateAllBaselines
                writer.WriteLine("/*******************************************************************************");
                writer.WriteLine("* Function Name:   " + m_instanceName + "_CSHL_UpdateAllBaselines");
                writer.WriteLine("********************************************************************************");
                writer.WriteLine("* ");
                writer.WriteLine("* Summary:");
                writer.WriteLine("*  Uses the " + m_instanceName + "_CSHL_UpdateAllSBaselinesLeft and ");
                writer.WriteLine("*  " + m_instanceName + "_CSHL_UpdateAllSBaselinesRight functions to ");
                writer.WriteLine("*  update the baselines for all slots. Raw data filters are applied to ");
                writer.WriteLine("*  the values if enabled.");
                writer.WriteLine("* ");
                writer.WriteLine("* Parameters:");
                writer.WriteLine("*  void");
                writer.WriteLine("* ");
                writer.WriteLine("* Return:");
                writer.WriteLine("*  void");
                writer.WriteLine("* ");
                writer.WriteLine("**********************************************************************************/");
                writer.WriteLine("void " + m_instanceName + "_CSHL_UpdateAllBaselines(void)");
                writer.WriteLine("{");
                foreach (CyAmuxBParams sbItem in packParams.m_localParams.m_listCsHalfs)
                    if (packParams.m_localParams.IsAmuxBusEnable(sbItem))
                    {
                        E_EL_SIDE side = sbItem.m_side;
                        writer.WriteLine("    " + m_instanceName + "_CSHL_UpdateAllBaselines" + side + "();");
                    }
                writer.WriteLine("}");
                writer.WriteLine("");
                writer.WriteLine("");
                #endregion

                #region CSHL_CheckIsAnySlotActive
                writer.WriteLine("/*******************************************************************************");
                writer.WriteLine("* Function Name:   " + m_instanceName + "_CSHL_CheckIsAnySlotActive");
                writer.WriteLine("********************************************************************************");
                writer.WriteLine("* ");
                writer.WriteLine("* Summary:");
                writer.WriteLine("*  Compares all slots of the " + m_instanceName + "_CSHL_Signal[ ] array to their finger ");
                writer.WriteLine("*  threshold. Uses the " + m_instanceName + "_CSHL_CheckIsSlotActiveLeft");
                writer.WriteLine("*  and " + m_instanceName + "_CSHL_CheckIsSlotActiveRight functions to ");
                writer.WriteLine("*  to chech if any of all scan slots is active. ");
                writer.WriteLine("*  The + m_instanceName + _CSHL_SlotOnMask[ ] array is up to date after calling ");
                writer.WriteLine("*  this function.");
                writer.WriteLine("* ");
                writer.WriteLine("* Parameters:");
                writer.WriteLine("*  void");
                writer.WriteLine("* ");
                writer.WriteLine("* Return:");
                writer.WriteLine("*  Returns 1 if any scan slot is active, 0 none of scan slots are active");
                writer.WriteLine("* ");
                writer.WriteLine("**********************************************************************************/");
                writer.WriteLine("uint8 " + m_instanceName + "_CSHL_CheckIsAnySlotActive(void)");
                writer.WriteLine("{");
                foreach (CyAmuxBParams sbItem in packParams.m_localParams.m_listCsHalfs)
                    if (packParams.m_localParams.IsAmuxBusEnable(sbItem))
                    {
                        E_EL_SIDE side = sbItem.m_side;
                        writer.WriteLine("    uint8 active" + side + ";");
                    }
                writer.WriteLine("");
                foreach (CyAmuxBParams sbItem in packParams.m_localParams.m_listCsHalfs)
                    if (packParams.m_localParams.IsAmuxBusEnable(sbItem))
                    {
                        E_EL_SIDE side = sbItem.m_side;
                        writer.WriteLine("    active" + side + " = " + m_instanceName + "_CSHL_CheckIsAnySlotActive" + side + "();");
                    }
                writer.Write("    ");
                writer.Write("    return (");
                string separator = "";
                foreach (CyAmuxBParams sbItem in packParams.m_localParams.m_listCsHalfs)
                    if (packParams.m_localParams.IsAmuxBusEnable(sbItem))
                    {
                        writer.Write(separator + "active" + sbItem.m_side);
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
        public void ApiCollectCHLFunctionCentroid(ref CyMyStringWriter writer, CyGeneralParams packParams)
        {
            if (packParams.m_cyWidgetsList.GetWidgetsCount(E_SENSOR_TYPE.Linear_Slider) > 0)
            {
                #region CSHL_GetCentroidPos
                writer.WriteLine("/*******************************************************************************");
                writer.WriteLine("* Function Name:   " + m_instanceName + "_CSHL_GetCentroidPos");
                writer.WriteLine("********************************************************************************");
                writer.WriteLine("* ");
                writer.WriteLine("* Summary:");
                writer.WriteLine("*  Checks the " + m_instanceName + "_CSHL_Signal[ ] array for a centroid. The centroid ");
                writer.WriteLine("*  position is calculated to the resolution specified in the CapSense ");
                writer.WriteLine("*  customizer. The position filters are applied to the result if enabled.");
                writer.WriteLine("* ");
                writer.WriteLine("* Parameters:");
                writer.WriteLine("*  widget:  Widget number.");
                writer.WriteLine("*  For every linear slider widget there are defines in this format:");
                writer.WriteLine("*  #define " + m_instanceName + "_CSHL_LS__\"widget_name\"            5");
                writer.WriteLine("* ");
                writer.WriteLine("* Return:");
                writer.WriteLine("*  Returns position value of the slider");
                writer.WriteLine("* ");
                writer.WriteLine("* Side Effects:");
                writer.WriteLine("*  If any slider slot is active, the function returns values from zero to");
                writer.WriteLine("*  the resolution value set in the CapSense customizer. If no sensors");
                writer.WriteLine("*  are active, the function returns -1. If an error occurs during");
                writer.WriteLine("*  execution of the centroid/diplexing algorithm, the function returns -1.");
                writer.WriteLine("*  You can use the CSHL_ChecklsSlotActive() routine to determine which");
                writer.WriteLine("*  slider segments are touched, if required.");
                writer.WriteLine("*  ");
                writer.WriteLine("*  Note: If noise counts on the slider segments are greater than the noise");
                writer.WriteLine("*  threshold, this subroutine may generate a false centroid result. The noise");
                writer.WriteLine("*  threshold should be set carefully (high enough above the noise level) so");
                writer.WriteLine("*  that noise will not generate a false centroid.");
                writer.WriteLine("* ");
                writer.WriteLine("**********************************************************************************/");
                writer.WriteLine("uint16 " + m_instanceName + "_CSHL_GetCentroidPos(uint8 widget)");
                writer.WriteLine("{");
                writer.WriteLine("    " + m_instanceName + "_SlSettings *ptr = (" + m_instanceName + "_SlSettings *) " + m_instanceName + "_CSHL_WidgetTable[widget].AdvancedSettings;");
                writer.WriteLine("    uint8 *diplex = ptr->DiplexTable;");
                writer.WriteLine("    uint8 offset = " + m_instanceName + "_CSHL_WidgetTable[widget].RawOffset;");
                writer.WriteLine("    uint8 count = " + m_instanceName + "_CSHL_WidgetTable[widget].ScanSlotCount;");
                writer.WriteLine("    uint8 curPos = 0;");
                writer.WriteLine("    uint8 index = 0;");
                writer.WriteLine("    uint8 curCtrdStartPos = 0xFFu;");
                writer.WriteLine("    uint8 curCntrdSize = 0;");
                writer.WriteLine("    uint8 biggestCtrdStartPos = 0;");
                writer.WriteLine("    uint8 biggestCtrdSize = 0;");
                writer.WriteLine("    uint8 localMax = 0xFFu;");
                writer.WriteLine("    uint8 i;");
                writer.WriteLine("    uint8 posPrev, pos, posNext;");
                writer.WriteLine("    int32 numerator = 0;");
                writer.WriteLine("    int32 denominator  = 0;");
                writer.WriteLine("    uint16 position;");
                writer.WriteLine("    uint16 basePos;");
                writer.WriteLine("    ");
                writer.WriteLine("    if (" + m_instanceName + "_CSHL_WidgetTable[widget].Type & " + m_instanceName + "_CSHL_TYPE_LINEAR_SLIDER)");
                writer.WriteLine("    {");
                writer.WriteLine("      /* Centoroid Calculations */");
                writer.WriteLine("      ");
                writer.WriteLine("        while(1)");
                writer.WriteLine("        {");
                writer.WriteLine("            if (" + m_instanceName + "_CSHL_SlotSignal[offset+curPos] > 0)");
                writer.WriteLine("            {        ");
                writer.WriteLine("                if (curCtrdStartPos == 0xFFu)");
                writer.WriteLine("                {");
                writer.WriteLine("                    /* Start of Centroid */");
                writer.WriteLine("                    curCtrdStartPos = index;");
                writer.WriteLine("                    curCntrdSize++;");
                writer.WriteLine("                }");
                writer.WriteLine("                else");
                writer.WriteLine("                {");
                writer.WriteLine("                    curCntrdSize++;");
                writer.WriteLine("                }");
                writer.WriteLine("            }");
                writer.WriteLine("            else");
                writer.WriteLine("            {");
                writer.WriteLine("                if(curCntrdSize > 0)");
                writer.WriteLine("                {");
                writer.WriteLine("                    /* We are in the end of current */");
                writer.WriteLine("                    if(curCntrdSize > biggestCtrdSize)");
                writer.WriteLine("                    {");
                writer.WriteLine("                        biggestCtrdSize = curCntrdSize;");
                writer.WriteLine("                        biggestCtrdStartPos = curCtrdStartPos;");
                writer.WriteLine("                        curCntrdSize = 0;");
                writer.WriteLine("                        curCtrdStartPos = 0xFFu;");
                writer.WriteLine("                    }");
                writer.WriteLine("                    else");
                writer.WriteLine("                    {");
                writer.WriteLine("                        curCntrdSize = 0;");
                writer.WriteLine("                        curCtrdStartPos = 0xFFu;");
                writer.WriteLine("                    }");
                writer.WriteLine("                    ");
                writer.WriteLine("                }");
                writer.WriteLine("            }");
                writer.WriteLine("            ");
                writer.WriteLine("            if(" + m_instanceName + "_CSHL_WidgetTable[widget].Type & " + m_instanceName + "_CSHL_IS_DIPLEX)");
                writer.WriteLine("            {");
                writer.WriteLine("                curPos = diplex[index+1];");
                writer.WriteLine("                if(index == ((count*2)-1))");
                writer.WriteLine("                {");
                writer.WriteLine("                    break;");
                writer.WriteLine("                }");
                writer.WriteLine("            }");
                writer.WriteLine("            else");
                writer.WriteLine("            {");
                writer.WriteLine("                if(index == (count-1))");
                writer.WriteLine("                {");
                writer.WriteLine("                    break;");
                writer.WriteLine("                }");
                writer.WriteLine("                curPos++;");
                writer.WriteLine("            }        ");
                writer.WriteLine("            ");
                writer.WriteLine("            index++;");
                writer.WriteLine("        }");
                writer.WriteLine("      ");
                writer.WriteLine("        /* Find the biggest Centroid ");
                writer.WriteLine("        * if two are the same size, last one wins");
                writer.WriteLine("        * We are in the end of current */");
                writer.WriteLine("        if(curCntrdSize >= biggestCtrdSize) ");
                writer.WriteLine("        {");
                writer.WriteLine("            biggestCtrdSize = curCntrdSize;");
                writer.WriteLine("            biggestCtrdStartPos = curCtrdStartPos;");
                writer.WriteLine("        }");
                writer.WriteLine("        ");
                writer.WriteLine("        if (biggestCtrdSize >= 2)");
                writer.WriteLine("        {");
                writer.WriteLine("            for (i = 0; i < biggestCtrdSize; i++)");
                writer.WriteLine("            {");
                writer.WriteLine("                if(" + m_instanceName + "_CSHL_WidgetTable[widget].Type & " + m_instanceName + "_CSHL_IS_DIPLEX)");
                writer.WriteLine("                {");
                writer.WriteLine("                    posPrev = diplex[biggestCtrdStartPos + i-1];");
                writer.WriteLine("                    pos = diplex[biggestCtrdStartPos + i];");
                writer.WriteLine("                    posNext = diplex[biggestCtrdStartPos + i+1];");
                writer.WriteLine("                }");
                writer.WriteLine("                else");
                writer.WriteLine("                {");
                writer.WriteLine("                    posPrev = biggestCtrdStartPos + i-1;");
                writer.WriteLine("                    pos = biggestCtrdStartPos + i;");
                writer.WriteLine("                    posNext = biggestCtrdStartPos + i+1;");
                writer.WriteLine("                }");
                writer.WriteLine("                ");
                writer.WriteLine("                /* Ignore if lower that FingerThreshold */");
                writer.WriteLine("                if(" + m_instanceName + "_CSHL_SlotSignal[offset + pos] > " + m_instanceName + "_CSHL_WidgetTable[widget].FingerThreshold)");
                writer.WriteLine("                {");
                writer.WriteLine("                    if (i == 0)");
                writer.WriteLine("                    {");
                writer.WriteLine("                        /* First element pos > posNext */");
                writer.WriteLine("                        if(" + m_instanceName + "_CSHL_SlotSignal[offset + pos] > " + m_instanceName + "_CSHL_SlotSignal[offset + posNext])");
                writer.WriteLine("                        {");
                writer.WriteLine("                            /* Find localMax */");
                writer.WriteLine("                            localMax = i;");
                writer.WriteLine("                            break;");
                writer.WriteLine("                        }");
                writer.WriteLine("                        else");
                writer.WriteLine("                        {");
                writer.WriteLine("                            if(" + m_instanceName + "_CSHL_SlotSignal[offset + pos] == " + m_instanceName + "_CSHL_SlotSignal[offset + posNext])");
                writer.WriteLine("                            {");
                writer.WriteLine("                                /* Compare BaselinesLow (i+1) and (i) */");
                writer.WriteLine("                                if(" + m_instanceName + "_CSHL_SlotBaselineLow[offset + pos] < " + m_instanceName + "_CSHL_SlotBaselineLow[offset + posNext])");
                writer.WriteLine("                                {");
                writer.WriteLine("                                    localMax = i;");
                writer.WriteLine("                                    break;");
                writer.WriteLine("                                }");
                writer.WriteLine("                            }");
                writer.WriteLine("                        }");
                writer.WriteLine("                    }");
                writer.WriteLine("                    else if (i == (biggestCtrdSize-1))");
                writer.WriteLine("                    {");
                writer.WriteLine("                        /* Last element posNext > pos */");
                writer.WriteLine("                        if(" + m_instanceName + "_CSHL_SlotSignal[offset + pos] > " + m_instanceName + "_CSHL_SlotSignal[offset + posPrev])");
                writer.WriteLine("                        {");
                writer.WriteLine("                            /* Find localMax */");
                writer.WriteLine("                            localMax = i;");
                writer.WriteLine("                            break;");
                writer.WriteLine("                        }");
                writer.WriteLine("                        else");
                writer.WriteLine("                        {");
                writer.WriteLine("                            if(" + m_instanceName + "_CSHL_SlotSignal[offset + pos] == " + m_instanceName + "_CSHL_SlotSignal[offset + posPrev])");
                writer.WriteLine("                            {");
                writer.WriteLine("                                /* Compare BaselinesLow (i+1) and (i) */");
                writer.WriteLine("                                if(" + m_instanceName + "_CSHL_SlotBaselineLow[offset + pos] < " + m_instanceName + "_CSHL_SlotBaselineLow[offset + posPrev])");
                writer.WriteLine("                                {");
                writer.WriteLine("                                    localMax = i;");
                writer.WriteLine("                                    break;");
                writer.WriteLine("                                }");
                writer.WriteLine("                            }    ");
                writer.WriteLine("                        }");
                writer.WriteLine("                        ");
                writer.WriteLine("                    }");
                writer.WriteLine("                    else");
                writer.WriteLine("                    {");
                writer.WriteLine("                        /* point (i+1) > (i) and (i+1) > (i+2) */");
                writer.WriteLine("                        if((" + m_instanceName + "_CSHL_SlotSignal[offset + pos] > " + m_instanceName + "_CSHL_SlotSignal[offset + posNext]) && (" + m_instanceName + "_CSHL_SlotSignal[offset + pos] > " + m_instanceName + "_CSHL_SlotSignal[offset + posPrev]))");
                writer.WriteLine("                        {");
                writer.WriteLine("                            localMax = i;");
                writer.WriteLine("                            break;");
                writer.WriteLine("                        }");
                writer.WriteLine("                        else");
                writer.WriteLine("                        {");
                writer.WriteLine("                            /* (i+1) == (i), compare BaselinesLow */");
                writer.WriteLine("                            if(" + m_instanceName + "_CSHL_SlotSignal[offset + pos] == " + m_instanceName + "_CSHL_SlotSignal[offset + posNext])");
                writer.WriteLine("                            {");
                writer.WriteLine("                                /* Compare BaselinesLow (i+1) and (i), if lower go next */");
                writer.WriteLine("                                if(" + m_instanceName + "_CSHL_SlotBaselineLow[offset + pos] < " + m_instanceName + "_CSHL_SlotBaselineLow[offset + posNext])");
                writer.WriteLine("                                {");
                writer.WriteLine("                                    if(" + m_instanceName + "_CSHL_SlotSignal[offset + pos] > " + m_instanceName + "_CSHL_SlotSignal[offset + posPrev])");
                writer.WriteLine("                                    {");
                writer.WriteLine("                                        localMax = i;");
                writer.WriteLine("                                        break;");
                writer.WriteLine("                                    }");
                writer.WriteLine("                                    else");
                writer.WriteLine("                                    {");
                writer.WriteLine("                                        /* (i+1) == (i+2), compare BaselinesLow */");
                writer.WriteLine("                                        if ((" + m_instanceName + "_CSHL_SlotSignal[offset + pos] == " + m_instanceName + "_CSHL_SlotSignal[offset + posPrev]) && (" + m_instanceName + "_CSHL_SlotBaselineLow[offset + pos] < " + m_instanceName + "_CSHL_SlotBaselineLow[offset + posPrev]))");
                writer.WriteLine("                                        {");
                writer.WriteLine("                                            localMax = i;");
                writer.WriteLine("                                            break;");
                writer.WriteLine("                                        }");
                writer.WriteLine("                                    }");
                writer.WriteLine("                                }");
                writer.WriteLine("                            }");
                writer.WriteLine("                            if(" + m_instanceName + "_CSHL_SlotSignal[offset + pos] == " + m_instanceName + "_CSHL_SlotSignal[offset + posPrev])");
                writer.WriteLine("                            {");
                writer.WriteLine("                                /* Compare BaselinesLow (i+1) and (i), if lower go next */");
                writer.WriteLine("                                if (" + m_instanceName + "_CSHL_SlotBaselineLow[offset + pos] < " + m_instanceName + "_CSHL_SlotBaselineLow[offset + posPrev])");
                writer.WriteLine("                                {");
                writer.WriteLine("                                    if (" + m_instanceName + "_CSHL_SlotSignal[offset + pos] > " + m_instanceName + "_CSHL_SlotSignal[offset + posNext])");
                writer.WriteLine("                                    {");
                writer.WriteLine("                                        localMax = i;");
                writer.WriteLine("                                        break;");
                writer.WriteLine("                                    }");
                writer.WriteLine("                                    else");
                writer.WriteLine("                                    {");
                writer.WriteLine("                                        /* (i+1) == (i+2), compare BaselinesLow */");
                writer.WriteLine("                                        if((" + m_instanceName + "_CSHL_SlotSignal[offset + pos] == " + m_instanceName + "_CSHL_SlotSignal[offset + posNext]) && (" + m_instanceName + "_CSHL_SlotBaselineLow[offset + pos] < " + m_instanceName + "_CSHL_SlotBaselineLow[offset + posNext]))");
                writer.WriteLine("                                        {");
                writer.WriteLine("                                            localMax = i;");
                writer.WriteLine("                                            break;");
                writer.WriteLine("                                        }");
                writer.WriteLine("                                    }");
                writer.WriteLine("                                }");
                writer.WriteLine("                            }");
                writer.WriteLine("                        }");
                writer.WriteLine("                    }");
                writer.WriteLine("                }");
                writer.WriteLine("            }");
                writer.WriteLine("        }");
                writer.WriteLine("        else if    ((!(" + m_instanceName + "_CSHL_WidgetTable[widget].Type & " + m_instanceName + "_CSHL_IS_DIPLEX)) && (biggestCtrdSize == 1))");
                writer.WriteLine("        {");
                writer.WriteLine("            pos = biggestCtrdStartPos;");
                writer.WriteLine("            /* Ignore if lower that FingerThreshold */");
                writer.WriteLine("            if(" + m_instanceName + "_CSHL_SlotSignal[offset + pos] > " + m_instanceName + "_CSHL_WidgetTable[widget].FingerThreshold)");
                writer.WriteLine("            {");
                writer.WriteLine("                localMax = 0;");
                writer.WriteLine("            }");
                writer.WriteLine("            else");
                writer.WriteLine("            {");
                writer.WriteLine("                localMax = 0xFF;");
                writer.WriteLine("            }");
                writer.WriteLine("        }");
                writer.WriteLine("        else");
                writer.WriteLine("        {");
                writer.WriteLine("            localMax = 0xFF;");
                writer.WriteLine("        }");
                writer.WriteLine("        ");
                writer.WriteLine("        if (localMax < 0xFFu)");
                writer.WriteLine("        {");
                writer.WriteLine("            /* Find positions of localMax and near it */");
                writer.WriteLine("            if(" + m_instanceName + "_CSHL_WidgetTable[widget].Type & " + m_instanceName + "_CSHL_IS_DIPLEX)");
                writer.WriteLine("            {");
                writer.WriteLine("                posPrev = diplex[biggestCtrdStartPos + localMax-1];");
                writer.WriteLine("                pos = diplex[biggestCtrdStartPos + localMax];");
                writer.WriteLine("                posNext = diplex[biggestCtrdStartPos + localMax+1];");
                writer.WriteLine("            }");
                writer.WriteLine("            else");
                writer.WriteLine("            {");
                writer.WriteLine("                posPrev = biggestCtrdStartPos + localMax-1;");
                writer.WriteLine("                pos = biggestCtrdStartPos + localMax;");
                writer.WriteLine("                posNext = biggestCtrdStartPos + localMax+1;");
                writer.WriteLine("            }");
                writer.WriteLine("            ");
                writer.WriteLine("            if (biggestCtrdSize >= 2)        /* The Biggest Centriod Size is grater that 2, need interpolation */");
                writer.WriteLine("            {    ");
                writer.WriteLine("                /* Calculate Sum(Si) */");
                writer.WriteLine("                if (localMax == 0)");
                writer.WriteLine("                {");
                writer.WriteLine("                    /* Start of Centroid */");
                writer.WriteLine("                    numerator = ((int32)" + m_instanceName + "_CSHL_SlotSignal[offset + posNext]);");
                writer.WriteLine("                    denominator  =  ((int32) " + m_instanceName + "_CSHL_SlotSignal[offset + pos]) + ((int32) " + m_instanceName + "_CSHL_SlotSignal[offset + posNext]);");
                writer.WriteLine("                }");
                writer.WriteLine("                else");
                writer.WriteLine("                {");
                writer.WriteLine("                    if (localMax == (biggestCtrdSize-1))");
                writer.WriteLine("                    {");
                writer.WriteLine("                        /* End of Centroid */");
                writer.WriteLine("                        numerator = ((int32) " + m_instanceName + "_CSHL_SlotSignal[offset + posPrev]) * (-1);");
                writer.WriteLine("                        denominator  = ((int32) " + m_instanceName + "_CSHL_SlotSignal[offset + pos]) + ((int32) " + m_instanceName + "_CSHL_SlotSignal[offset + posPrev]);");
                writer.WriteLine("                    }");
                writer.WriteLine("                    else");
                writer.WriteLine("                    {");
                writer.WriteLine("                        /* Not first Not last */");
                writer.WriteLine("                        numerator = ((int32) " + m_instanceName + "_CSHL_SlotSignal[offset + posNext]) - ((int32) " + m_instanceName + "_CSHL_SlotSignal[offset + posPrev]);");
                writer.WriteLine("                        denominator  = ((int32) " + m_instanceName + "_CSHL_SlotSignal[offset + posPrev]) + ((int32) " + m_instanceName + "_CSHL_SlotSignal[offset + pos]) + ((int32) " + m_instanceName + "_CSHL_SlotSignal[offset + posNext]);");
                writer.WriteLine("                        ");
                writer.WriteLine("                    }");
                writer.WriteLine("                }");
                writer.WriteLine("                ");
                writer.WriteLine("                numerator <<= 8;");
                writer.WriteLine("                /* Div (numerator/denominator) * Resolution */");
                writer.WriteLine("                denominator  = ((numerator/denominator) + (((uint16) (biggestCtrdStartPos + localMax) )<< 8)) *  ptr->Resolution;");
                writer.WriteLine("                ");
                writer.WriteLine("                if(denominator < 0)");
                writer.WriteLine("                {");
                writer.WriteLine("                    denominator = denominator * (-1);");
                writer.WriteLine("                }");
                writer.WriteLine("                ");
                writer.WriteLine("                position = HI16(denominator);");
                writer.WriteLine("            }");
                writer.WriteLine("            else        /* The Biggest Centriod Size 1, NOT interpolation needed */");
                writer.WriteLine("            {");
                writer.WriteLine("                denominator = (pos * ptr->Resolution) + 0x7Fu;");
                writer.WriteLine("                position = HI8(denominator);");
                writer.WriteLine("            }");
                writer.WriteLine("            ");
                writer.WriteLine("            /* Filtering */");
                writer.WriteLine("            ");
                writer.WriteLine("            if (ptr->FirstTime == 0)        /* Initialize the filters */");
                writer.WriteLine("            {");
                writer.WriteLine("                /* Median filter first time initialization */");
                writer.WriteLine("                if(" + m_instanceName + "_CSHL_WidgetTable[widget].Filters & " + m_instanceName + "_CSHL_POS_MEDIAN_FILTER)");
                writer.WriteLine("                {");
                writer.WriteLine("                    ptr->Pos2Median = position;");
                writer.WriteLine("                    ptr->Pos1Median = position;");
                writer.WriteLine("                }");
                writer.WriteLine("                ");
                writer.WriteLine("                /* Averaging filter first time initialization */");
                writer.WriteLine("                if(" + m_instanceName + "_CSHL_WidgetTable[widget].Filters & " + m_instanceName + "_CSHL_POS_AVERAGING_FILTER)");
                writer.WriteLine("                {");
                writer.WriteLine("                    ptr->Pos2Averaging = position;");
                writer.WriteLine("                    ptr->Pos1Averaging = position;");
                writer.WriteLine("                }");
                writer.WriteLine("                /* IIR filter first time initialization */");
                writer.WriteLine("                if((" + m_instanceName + "_CSHL_WidgetTable[widget].Filters & " + m_instanceName + "_CSHL_POS_IIR_FILTER_0) ||");
                writer.WriteLine("                    (" + m_instanceName + "_CSHL_WidgetTable[widget].Filters & " + m_instanceName + "_CSHL_POS_IIR_FILTER_1))");
                writer.WriteLine("                {");
                writer.WriteLine("                    ptr->PosIIR = position;");
                writer.WriteLine("                }");
                writer.WriteLine("                ");
                writer.WriteLine("                /* Jitter filter first time initialization */");
                writer.WriteLine("                if(" + m_instanceName + "_CSHL_WidgetTable[widget].Filters & " + m_instanceName + "_CSHL_POS_JITTER_FILTER)");
                writer.WriteLine("                {");
                writer.WriteLine("                    ptr->PosJitter = position;");
                writer.WriteLine("                }");
                writer.WriteLine("                ptr->FirstTime = 1;");
                writer.WriteLine("            }");
                writer.WriteLine("            else            /* Do the filtering */");
                writer.WriteLine("            {");
                writer.WriteLine("                /* Median filter */");
                writer.WriteLine("                if(" + m_instanceName + "_CSHL_WidgetTable[widget].Filters & " + m_instanceName + "_CSHL_POS_MEDIAN_FILTER)");
                writer.WriteLine("                {");
                writer.WriteLine("                    basePos = position;");
                writer.WriteLine("                    position = " + m_instanceName + "_CSHL_MedianFilter(position, ptr->Pos1Median, ptr->Pos2Median);");
                writer.WriteLine("                    ptr->Pos2Median = ptr->Pos1Median;");
                writer.WriteLine("                        ptr->Pos1Median = basePos;");
                writer.WriteLine("                }");
                writer.WriteLine("                ");
                writer.WriteLine("                /* Averaging filter */");
                writer.WriteLine("                if(" + m_instanceName + "_CSHL_WidgetTable[widget].Filters & " + m_instanceName + "_CSHL_POS_AVERAGING_FILTER)");
                writer.WriteLine("                {");
                writer.WriteLine("                    basePos = position;");
                writer.WriteLine("                    position = " + m_instanceName + "_CSHL_AveragingFilter(position, ptr->Pos1Averaging, ptr->Pos2Averaging);");
                writer.WriteLine("                    ptr->Pos2Averaging = ptr->Pos1Averaging;");
                writer.WriteLine("                    ptr->Pos1Averaging= basePos;");
                writer.WriteLine("                }");
                writer.WriteLine("                /* IIR filter */");
                writer.WriteLine("                if(" + m_instanceName + "_CSHL_WidgetTable[widget].Filters & " + m_instanceName + "_CSHL_POS_IIR_FILTER_0)");
                writer.WriteLine("                {");
                writer.WriteLine("                    basePos = position;");
                writer.WriteLine("                    position = " + m_instanceName + "_CSHL_IIRFilter(position, ptr->PosIIR," + m_instanceName + "_CSHL_IIR_FILTER_0);");
                writer.WriteLine("                    ptr->PosJitter = basePos;");
                writer.WriteLine("                }");
                writer.WriteLine("                else if (" + m_instanceName + "_CSHL_WidgetTable[widget].Filters & " + m_instanceName + "_CSHL_POS_IIR_FILTER_1)");
                writer.WriteLine("                {");
                writer.WriteLine("                    basePos = position;");
                writer.WriteLine("                    position = " + m_instanceName + "_CSHL_IIRFilter(position, ptr->PosIIR," + m_instanceName + "_CSHL_IIR_FILTER_1);");
                writer.WriteLine("                    ptr->PosJitter = basePos;");
                writer.WriteLine("                }");
                writer.WriteLine("                ");
                writer.WriteLine("                /* Jitter filter */");
                writer.WriteLine("                if(" + m_instanceName + "_CSHL_WidgetTable[widget].Filters & " + m_instanceName + "_CSHL_POS_JITTER_FILTER)");
                writer.WriteLine("                {");
                writer.WriteLine("                    basePos = position;");
                writer.WriteLine("                    position = " + m_instanceName + "_CSHL_JitterFilter(position, ptr->PosJitter);");
                writer.WriteLine("                    ptr->PosJitter = basePos;");
                writer.WriteLine("                }");
                writer.WriteLine("            }");
                writer.WriteLine("        }");
                writer.WriteLine("        else");
                writer.WriteLine("        {");
                writer.WriteLine("            /* Local max doesn't find*/");
                writer.WriteLine("            position = 0x00FFu;");
                writer.WriteLine("            ptr->FirstTime = 0;");
                writer.WriteLine("        }");
                writer.WriteLine("    }");
                writer.WriteLine("    else");
                writer.WriteLine("    {");
                writer.WriteLine("        /* This Widget isn't Linear Slider Widget */");
                writer.WriteLine("        position = 0x00FFu;");
                writer.WriteLine("    }");
                writer.WriteLine("    ");
                writer.WriteLine("    return position;");
                writer.WriteLine("}");
                #endregion
            }

            if (packParams.m_cyWidgetsList.GetWidgetsCount(E_SENSOR_TYPE.Radial_Slider) > 0)
            {
                #region CSHL_GetRadialCentroidPos

                #region CSHL_RotarySliderArcTan2
                writer.WriteLine("/*******************************************************************************");
                writer.WriteLine("* Function Name:   " + m_instanceName + "_CSHL_RotarySliderArcTan2");
                writer.WriteLine("********************************************************************************");
                writer.WriteLine("* ");
                writer.WriteLine("* Summary:");
                writer.WriteLine("*  This function calculates the angle in radians between the positive x-axis of");
                writer.WriteLine("*  a plane and the point given by the coordinates (x, y) on it.");
                writer.WriteLine("*");
                writer.WriteLine("* Parameters:");
                writer.WriteLine("*  X:  X coordinate");
                writer.WriteLine("*  Y:  Y coordinate");
                writer.WriteLine("*");
                writer.WriteLine("* Return:");
                writer.WriteLine("*  Returns ArcTan.");
                writer.WriteLine("* ");
                writer.WriteLine("**********************************************************************************/");
                writer.WriteLine("uint16 " + m_instanceName + "_CSHL_RotarySliderArcTan2(int32 X, int32 Y)");
                writer.WriteLine("{");
                writer.WriteLine("    int32 tmp, tempCalc;");
                writer.WriteLine("    uint8 i;");
                writer.WriteLine("    uint16 ang = 0;");
                writer.WriteLine("    ");
                writer.WriteLine("    if (Y == 0)");
                writer.WriteLine("    {");
                writer.WriteLine("        if (X < 0)");
                writer.WriteLine("        {");
                writer.WriteLine("            ang = " + m_instanceName + "_CSHL_ROTARY_SLIDER_A180*4;");
                writer.WriteLine("        }");
                writer.WriteLine("        else");
                writer.WriteLine("        {");
                writer.WriteLine("            ang = 0;  ");
                writer.WriteLine("        }");
                writer.WriteLine("        ");
                writer.WriteLine("    }");
                writer.WriteLine("    else");
                writer.WriteLine("    {");
                writer.WriteLine("        if (X == 0)");
                writer.WriteLine("        {");
                writer.WriteLine("            if (Y > 0)");
                writer.WriteLine("            {");
                writer.WriteLine("                ang = " + m_instanceName + "_CSHL_ROTARY_SLIDER_A90*4;");
                writer.WriteLine("            }");
                writer.WriteLine("            else");
                writer.WriteLine("            if (Y < 0)");
                writer.WriteLine("            {");
                writer.WriteLine("                ang = " + m_instanceName + "_CSHL_ROTARY_SLIDER_A270*4;  /* 270 */");
                writer.WriteLine("            }");
                writer.WriteLine("        ");
                writer.WriteLine("        }");
                writer.WriteLine("        else");
                writer.WriteLine("        {");
                writer.WriteLine("            if (X < 0)");
                writer.WriteLine("            {");
                writer.WriteLine("                X = -X;");
                writer.WriteLine("                Y = -Y;");
                writer.WriteLine("                ang = " + m_instanceName + "_CSHL_ROTARY_SLIDER_A180*4;");
                writer.WriteLine("            }");
                writer.WriteLine("            else ");
                writer.WriteLine("            {    ");
                writer.WriteLine("                if (Y > 0)");
                writer.WriteLine("                {");
                writer.WriteLine("                    ang = " + m_instanceName + "_CSHL_ROTARY_SLIDER_A360*4;");
                writer.WriteLine("                }");
                writer.WriteLine("            }");
                writer.WriteLine("            ");
                writer.WriteLine("            tmp = X;");
                writer.WriteLine("            X = Y;");
                writer.WriteLine("            ");
                writer.WriteLine("            if (Y < 0)");
                writer.WriteLine("            {");
                writer.WriteLine("                X = -X;");
                writer.WriteLine("                Y = tmp;");
                writer.WriteLine("                ang += " + m_instanceName + "_CSHL_ROTARY_SLIDER_A90*4;");
                writer.WriteLine("            }");
                writer.WriteLine("            else");
                writer.WriteLine("            {");
                writer.WriteLine("                Y = -tmp;");
                writer.WriteLine("                ang -= " + m_instanceName + "_CSHL_ROTARY_SLIDER_A90*4;");
                writer.WriteLine("            }");
                writer.WriteLine("            ");
                writer.WriteLine("            for (i=0; i<9; i++)");
                writer.WriteLine("            {");
                writer.WriteLine("                if (Y == 0)");
                writer.WriteLine("                {");
                writer.WriteLine("                    break;");
                writer.WriteLine("                }");
                writer.WriteLine("                ");
                writer.WriteLine("                tempCalc = (Y >> i);");
                writer.WriteLine("                tmp = (X >> i);");
                writer.WriteLine("                ");
                writer.WriteLine("                if (Y < 0)");
                writer.WriteLine("                {");
                writer.WriteLine("                    X -= tempCalc;");
                writer.WriteLine("                    Y += tmp;");
                writer.WriteLine("                    ang += " + m_instanceName + "_CSHL_RotarySliderAngle[i];");
                writer.WriteLine("                }");
                writer.WriteLine("                else");
                writer.WriteLine("                {");
                writer.WriteLine("                    X += tempCalc;");
                writer.WriteLine("                    Y -= tmp;");
                writer.WriteLine("                    ang -= " + m_instanceName + "_CSHL_RotarySliderAngle[i];");
                writer.WriteLine("                }");
                writer.WriteLine("            }");
                writer.WriteLine("            ");
                writer.WriteLine("            ang = (" + m_instanceName + "_CSHL_ROTARY_SLIDER_A360*4 - ang)/4;");
                writer.WriteLine("        }");
                writer.WriteLine("    }");
                writer.WriteLine("    ");
                writer.WriteLine("    return ang;");
                writer.WriteLine("}");
                writer.WriteLine("");
                writer.WriteLine("");
                #endregion

                #region CSHL_RotarySlider_GetValue
                writer.WriteLine("/*******************************************************************************");
                writer.WriteLine("* Function Name:   " + m_instanceName + "_CSHL_RotarySlider_GetValue");
                writer.WriteLine("********************************************************************************");
                writer.WriteLine("* ");
                writer.WriteLine("* Summary:");
                writer.WriteLine("*  Calculates an angle in degrees of defined centroid on the radial slider.");
                writer.WriteLine("* ");
                writer.WriteLine("* Parameters:");
                writer.WriteLine("*  ZeroOffset:  Offset of the angle");
                writer.WriteLine("*  SegCount:  Number of elements in the slider ");
                writer.WriteLine("*  firstElem:  Start index of centroid in " + m_instanceName + "_CSHL_SlotSignal[] array");
                writer.WriteLine("*  numElem:  Length of centroid ");
                writer.WriteLine("*  offset: Raw offset in " + m_instanceName + "_CSHL_SlotSignal[] array");
                writer.WriteLine("*  diplex:  Pointer to diplex table of slider (if exists)");
                writer.WriteLine("*  Type:  Defines if there is diplexing");
                writer.WriteLine("* ");
                writer.WriteLine("* Return:");
                writer.WriteLine("*  Returns angle of centroid (in degrees).");
                writer.WriteLine("* ");
                writer.WriteLine("**********************************************************************************/");
                writer.WriteLine("uint16 " + m_instanceName + "_CSHL_RotarySlider_GetValue(const uint8 ZeroOffset, int SegCount, uint8 firstElem, uint8 numElem, uint8 offset, uint8 *diplex, uint8 Type)");
                writer.WriteLine("{");
                writer.WriteLine("    uint8 i, pos, Nsin, Ncos;");
                writer.WriteLine("    int32 Xc, Yc, temp;");
                writer.WriteLine("    uint16 tempAngle, DGrad, angle;");
                writer.WriteLine("    ");
                writer.WriteLine("    Xc = 0;");
                writer.WriteLine("    Yc = 0;    ");
                writer.WriteLine("    DGrad = 360 / SegCount;    /* Angle step */");
                writer.WriteLine("    angle = (360 * firstElem / SegCount);");
                writer.WriteLine("    ");
                writer.WriteLine("    for(i = firstElem; i  < firstElem + numElem; i++)    /* Loop through every slot segment */");
                writer.WriteLine("    {");
                writer.WriteLine("        if (Type & " + m_instanceName + "_CSHL_IS_DIPLEX)");
                writer.WriteLine("        {");
                writer.WriteLine("            pos = diplex[i%SegCount];");
                writer.WriteLine("        }");
                writer.WriteLine("        else ");
                writer.WriteLine("        {");
                writer.WriteLine("            pos = i%SegCount;");
                writer.WriteLine("        }");
                writer.WriteLine("        ");
                writer.WriteLine("        if (" + m_instanceName + "_CSHL_SlotSignal[offset + pos] > 0)");
                writer.WriteLine("        {");
                writer.WriteLine("            tempAngle = angle ;  /* Real angle */");
                writer.WriteLine("            if (tempAngle > " + m_instanceName + "_CSHL_ROTARY_SLIDER_A360)");
                writer.WriteLine("            {");
                writer.WriteLine("                tempAngle -= " + m_instanceName + "_CSHL_ROTARY_SLIDER_A360;");
                writer.WriteLine("            }");
                writer.WriteLine("            if (tempAngle > " + m_instanceName + "_CSHL_ROTARY_SLIDER_A270)    /* if angle is between 270...360 then SIN = + ; COS = - */");
                writer.WriteLine("            {");
                writer.WriteLine("                Nsin = 1;");
                writer.WriteLine("                Ncos = 0;");
                writer.WriteLine("                tempAngle = " + m_instanceName + "_CSHL_ROTARY_SLIDER_A360 - tempAngle;");
                writer.WriteLine("            }");
                writer.WriteLine("            else");
                writer.WriteLine("            {");
                writer.WriteLine("                if(tempAngle > " + m_instanceName + "_CSHL_ROTARY_SLIDER_A180)    /* If angle is between 180...270 then SIN = + ; COS = + */");
                writer.WriteLine("                {");
                writer.WriteLine("                    Nsin = 1;");
                writer.WriteLine("                    Ncos = 1;");
                writer.WriteLine("                    tempAngle -= " + m_instanceName + "_CSHL_ROTARY_SLIDER_A180;        ");
                writer.WriteLine("                }");
                writer.WriteLine("                else");
                writer.WriteLine("                {");
                writer.WriteLine("                    Nsin = 0;");
                writer.WriteLine("                    if(tempAngle > " + m_instanceName + "_CSHL_ROTARY_SLIDER_A90)    /* If angle is between 90...180 then SIN = - ; COS = + */");
                writer.WriteLine("                    {");
                writer.WriteLine("                        Ncos = 1;");
                writer.WriteLine("                        tempAngle = " + m_instanceName + "_CSHL_ROTARY_SLIDER_A180 - tempAngle;");
                writer.WriteLine("                    }");
                writer.WriteLine("                    else                        /* If angle is between 0...90 then SIN = - ; COS = - */");
                writer.WriteLine("                    {");
                writer.WriteLine("                        Ncos = 0;");
                writer.WriteLine("                    }");
                writer.WriteLine("                }");
                writer.WriteLine("            }");
                writer.WriteLine("            ");
                writer.WriteLine("            tempAngle = tempAngle>>1;");
                writer.WriteLine("            temp = ((int32) " + m_instanceName + "_CSHL_SlotSignal[offset + pos]) * ((int32) " + m_instanceName + "_CSHL_RotarySliderAngleSin[tempAngle]);");
                writer.WriteLine("            if (Nsin)        /* Calculate vertical divider */");
                writer.WriteLine("            {");
                writer.WriteLine("                Yc -= temp;");
                writer.WriteLine("            }");
                writer.WriteLine("            else");
                writer.WriteLine("            {");
                writer.WriteLine("                Yc += temp;");
                writer.WriteLine("            }");
                writer.WriteLine("             ");
                writer.WriteLine("            temp = ((int32) " + m_instanceName + "_CSHL_SlotSignal[offset + pos]) * ((int32) " + m_instanceName + "_CSHL_RotarySliderAngleSin[45 - tempAngle]);");
                writer.WriteLine("            if (Ncos)        /* Calculate horisontal divider */");
                writer.WriteLine("            {");
                writer.WriteLine("                Xc -= temp;");
                writer.WriteLine("            }");
                writer.WriteLine("            else");
                writer.WriteLine("            {");
                writer.WriteLine("                Xc += temp;");
                writer.WriteLine("            }");
                writer.WriteLine("        }");
                writer.WriteLine("        ");
                writer.WriteLine("        angle += DGrad;        /* Step to next angle */");
                writer.WriteLine("    }");
                writer.WriteLine("    ");
                writer.WriteLine("    while ((Xc > 0x00003FFF) || (Yc > 0x00003FFF))    /* Convert long to integer tape */");
                writer.WriteLine("    {");
                writer.WriteLine("        Xc >>= 1;");
                writer.WriteLine("        Yc >>= 1;");
                writer.WriteLine("    }");
                writer.WriteLine("    while ((Xc < ~0x00003FFF) || (Yc < ~0x00003FFF))    /* Convert long to integer tape */");
                writer.WriteLine("    {");
                writer.WriteLine("        Xc >>= 1;");
                writer.WriteLine("        Yc >>= 1;");
                writer.WriteLine("    }");
                writer.WriteLine("    ");
                writer.WriteLine("    angle = 0;");
                writer.WriteLine("    ");
                writer.WriteLine("    if ((Xc != 0) || (Yc != 0))        /* If any slot active then calculate the angle */");
                writer.WriteLine("    {");
                writer.WriteLine("        angle = " + m_instanceName + "_CSHL_RotarySliderArcTan2( (int32) Xc, (int32) Yc);");
                writer.WriteLine("        ");
                writer.WriteLine("        angle += ZeroOffset;");
                writer.WriteLine("        if (angle > 360)");
                writer.WriteLine("        {");
                writer.WriteLine("            angle -= 360;");
                writer.WriteLine("        }");
                writer.WriteLine("    }");
                writer.WriteLine("    ");
                writer.WriteLine("    return angle;");
                writer.WriteLine("}");
                writer.WriteLine("");
                writer.WriteLine("");
                #endregion

                #region CSHL_GetRadialCentroidPos
                writer.WriteLine("/*******************************************************************************");
                writer.WriteLine("* Function Name:   " + m_instanceName + "_CSHL_GetRadialCentroidPos");
                writer.WriteLine("********************************************************************************");
                writer.WriteLine("* ");
                writer.WriteLine("* Summary:");
                writer.WriteLine("*  Checks the " + m_instanceName + "_CSHL_Signal[ ] array for a centroid. The centroid ");
                writer.WriteLine("*  position is calculated to the resolution specified in the CapSense ");
                writer.WriteLine("*  customizer. The position filters are applied to the result if enabled.");
                writer.WriteLine("* ");
                writer.WriteLine("* Parameters:");
                writer.WriteLine("*  widget:  Widget number. ");
                writer.WriteLine("*  For every linear slider widget there are defines in this format:");
                writer.WriteLine("*  #define " + m_instanceName + "_CSHL_RS_\"widget_name\"            5");
                writer.WriteLine("* ");
                writer.WriteLine("* Return:");
                writer.WriteLine("*  Returns position value of the slider");
                writer.WriteLine("* ");
                writer.WriteLine("* Side Effects:");
                writer.WriteLine("*  If any slider slot is active, the function returns values from zero to ");
                writer.WriteLine("*  the resolution value set in the CapSense customizer. If no sensors ");
                writer.WriteLine("*  are active, the function returns -1. If an error occurs during ");
                writer.WriteLine("*  execution of the centroid/diplexing algorithm, the function returns -1.");
                writer.WriteLine("*  You can use the CSHL_ChecklsSlotActive() routine to determine which ");
                writer.WriteLine("*  slider segments are touched, if required.");
                writer.WriteLine("* ");
                writer.WriteLine("*  Note: If noise counts on the slider segments are greater than the noise");
                writer.WriteLine("*  threshold, this subroutine may generate a false centroid result. The noise");
                writer.WriteLine("*  threshold should be set carefully (high enough above the noise level) so ");
                writer.WriteLine("*  that noise will not generate a false centroid.");
                writer.WriteLine("*");
                writer.WriteLine("**********************************************************************************/");
                writer.WriteLine(" uint16 " + m_instanceName + "_CSHL_GetRadialCentroidPos(uint8 widget)");
                writer.WriteLine("{");
                writer.WriteLine("    " + m_instanceName + "_SlSettings *ptr = (" + m_instanceName + "_SlSettings *) " + m_instanceName + "_CSHL_WidgetTable[widget].AdvancedSettings;");
                writer.WriteLine("    uint8 offset = " + m_instanceName + "_CSHL_WidgetTable[widget].RawOffset;");
                writer.WriteLine("    uint8 *diplex = ptr->DiplexTable;");
                writer.WriteLine("    uint16 angle, position;");
                writer.WriteLine("    uint8 signalsNumberX2, firstSns, firstMaxSns, maxLength, isCont, posPrev, pos, posNext;");
                writer.WriteLine("    uint8 i;");
                writer.WriteLine("    uint8 startNum, endNum;");
                writer.WriteLine("    uint8 localMaxValue;");
                writer.WriteLine("    uint16 basePos;");
                writer.WriteLine("    ");
                writer.WriteLine("    if (" + m_instanceName + "_CSHL_WidgetTable[widget].Type & " + m_instanceName + "_CSHL_TYPE_RADIAL_SLIDER)");
                writer.WriteLine("    {");
                writer.WriteLine("        /* Centoroid Calculations */");
                writer.WriteLine("        ");
                writer.WriteLine("        if(" + m_instanceName + "_CSHL_WidgetTable[widget].Type & " + m_instanceName + "_CSHL_IS_DIPLEX)");
                writer.WriteLine("        {");
                writer.WriteLine("            signalsNumberX2 = " + m_instanceName + "_CSHL_WidgetTable[widget].ScanSlotCount * 2;");
                writer.WriteLine("        }");
                writer.WriteLine("        else");
                writer.WriteLine("        {");
                writer.WriteLine("            signalsNumberX2 = " + m_instanceName + "_CSHL_WidgetTable[widget].ScanSlotCount;");
                writer.WriteLine("        }");
                writer.WriteLine("        ");
                writer.WriteLine("        firstSns = 0;");
                writer.WriteLine("        firstMaxSns = 0;");
                writer.WriteLine("        maxLength = 0;");
                writer.WriteLine("        isCont = 0;");
                writer.WriteLine("        localMaxValue = 0;");
                writer.WriteLine("        ");
                writer.WriteLine("        /* Find interval */");
                writer.WriteLine("        for (i = 0; i < signalsNumberX2; i++)");
                writer.WriteLine("        {");
                writer.WriteLine("            if(" + m_instanceName + "_CSHL_WidgetTable[widget].Type & " + m_instanceName + "_CSHL_IS_DIPLEX)");
                writer.WriteLine("            {");
                writer.WriteLine("                pos = diplex[i];");
                writer.WriteLine("            }");
                writer.WriteLine("            else");
                writer.WriteLine("            {");
                writer.WriteLine("                pos = i;");
                writer.WriteLine("            }");
                writer.WriteLine("            ");
                writer.WriteLine("            if (" + m_instanceName + "_CSHL_SlotSignal[offset + pos] > 0)");
                writer.WriteLine("            {");
                writer.WriteLine("                if (!isCont)");
                writer.WriteLine("                {");
                writer.WriteLine("                    firstSns = i;");
                writer.WriteLine("                    isCont = 1;");
                writer.WriteLine("                }");
                writer.WriteLine("                /* If it is the last element */");
                writer.WriteLine("                if (i == (signalsNumberX2 - 1))");
                writer.WriteLine("                {");
                writer.WriteLine("                    isCont = 0;");
                writer.WriteLine("                    if ((i - firstSns + 1) >= maxLength)");
                writer.WriteLine("                    {");
                writer.WriteLine("                        maxLength = i - firstSns + 1;");
                writer.WriteLine("                        firstMaxSns = firstSns;");
                writer.WriteLine("                    }");
                writer.WriteLine("                }");
                writer.WriteLine("            }");
                writer.WriteLine("            else");
                writer.WriteLine("            {");
                writer.WriteLine("                if (isCont)");
                writer.WriteLine("                {");
                writer.WriteLine("                    isCont = 0;");
                writer.WriteLine("                    if ((i - firstSns) >= maxLength)");
                writer.WriteLine("                    {");
                writer.WriteLine("                        maxLength = i - firstSns;");
                writer.WriteLine("                        firstMaxSns = firstSns;");
                writer.WriteLine("                    }");
                writer.WriteLine("                }");
                writer.WriteLine("            }");
                writer.WriteLine("        }");
                writer.WriteLine("        ");
                writer.WriteLine("        /* Check borders for max interval */");
                writer.WriteLine("        if (" + m_instanceName + "_CSHL_WidgetTable[widget].Type & " + m_instanceName + "_CSHL_IS_DIPLEX)");
                writer.WriteLine("        {");
                writer.WriteLine("            pos = diplex[signalsNumberX2-1];");
                writer.WriteLine("        }");
                writer.WriteLine("        else");
                writer.WriteLine("        {");
                writer.WriteLine("            pos = signalsNumberX2-1;");
                writer.WriteLine("        }");
                writer.WriteLine("        ");
                writer.WriteLine("        if ((" + m_instanceName + "_CSHL_SlotSignal[offset + 0] > 0) && (" + m_instanceName + "_CSHL_SlotSignal[offset + pos] > 0))");
                writer.WriteLine("        {");
                writer.WriteLine("            startNum = 0;");
                writer.WriteLine("            endNum = 0;");
                writer.WriteLine("            ");
                writer.WriteLine("            if (" + m_instanceName + "_CSHL_WidgetTable[widget].Type & " + m_instanceName + "_CSHL_IS_DIPLEX)");
                writer.WriteLine("            {");
                writer.WriteLine("                pos = diplex[startNum];");
                writer.WriteLine("            }");
                writer.WriteLine("            else ");
                writer.WriteLine("            {");
                writer.WriteLine("                pos = startNum;");
                writer.WriteLine("            }");
                writer.WriteLine("            ");
                writer.WriteLine("            while (" + m_instanceName + "_CSHL_SlotSignal[offset + pos] > 0)");
                writer.WriteLine("            {");
                writer.WriteLine("                startNum++;");
                writer.WriteLine("                if (" + m_instanceName + "_CSHL_WidgetTable[widget].Type & " + m_instanceName + "_CSHL_IS_DIPLEX)");
                writer.WriteLine("                {");
                writer.WriteLine("                    pos = diplex[startNum];");
                writer.WriteLine("                }");
                writer.WriteLine("                else ");
                writer.WriteLine("                {");
                writer.WriteLine("                    pos = startNum;");
                writer.WriteLine("                }");
                writer.WriteLine("            }");
                writer.WriteLine("            ");
                writer.WriteLine("            if (" + m_instanceName + "_CSHL_WidgetTable[widget].Type & " + m_instanceName + "_CSHL_IS_DIPLEX)");
                writer.WriteLine("            {");
                writer.WriteLine("                pos = diplex[signalsNumberX2 - 1 - endNum];");
                writer.WriteLine("            }");
                writer.WriteLine("            else ");
                writer.WriteLine("            {");
                writer.WriteLine("                pos = signalsNumberX2 - 1 - endNum;");
                writer.WriteLine("            }");
                writer.WriteLine("            ");
                writer.WriteLine("            while (" + m_instanceName + "_CSHL_SlotSignal[offset + pos] > 0)");
                writer.WriteLine("            {");
                writer.WriteLine("                endNum++;");
                writer.WriteLine("                if (" + m_instanceName + "_CSHL_WidgetTable[widget].Type & " + m_instanceName + "_CSHL_IS_DIPLEX)");
                writer.WriteLine("                {");
                writer.WriteLine("                    pos = diplex[signalsNumberX2 - 1 - endNum];");
                writer.WriteLine("                }");
                writer.WriteLine("                else ");
                writer.WriteLine("                {");
                writer.WriteLine("                    pos = signalsNumberX2 - 1 - endNum;");
                writer.WriteLine("                }");
                writer.WriteLine("            }");
                writer.WriteLine("            ");
                writer.WriteLine("            if (startNum + endNum > maxLength)");
                writer.WriteLine("            {");
                writer.WriteLine("                maxLength = startNum + endNum;");
                writer.WriteLine("                firstMaxSns = signalsNumberX2 - endNum;");
                writer.WriteLine("            }");
                writer.WriteLine("        }");
                writer.WriteLine("        ");
                writer.WriteLine("        if ((maxLength > 1) || ((maxLength == 1) && (!(" + m_instanceName + "_CSHL_WidgetTable[widget].Type & " + m_instanceName + "_CSHL_IS_DIPLEX)))) /* If there is positive interval */");
                writer.WriteLine("        {");
                writer.WriteLine("            if (maxLength > 1)");
                writer.WriteLine("            {");
                writer.WriteLine("                /* Find local max */");
                writer.WriteLine("                /* Left side */");
                writer.WriteLine("                if (" + m_instanceName + "_CSHL_WidgetTable[widget].Type & " + m_instanceName + "_CSHL_IS_DIPLEX)");
                writer.WriteLine("                {");
                writer.WriteLine("                    pos = diplex[firstMaxSns];");
                writer.WriteLine("                    posNext = diplex[(firstMaxSns+1)%signalsNumberX2];");
                writer.WriteLine("                }");
                writer.WriteLine("                else ");
                writer.WriteLine("                {");
                writer.WriteLine("                    pos = firstMaxSns;");
                writer.WriteLine("                    posNext = (firstMaxSns+1)%signalsNumberX2;");
                writer.WriteLine("                }");
                writer.WriteLine("                ");
                writer.WriteLine("                if ((" + m_instanceName + "_CSHL_SlotSignal[offset + pos] > " + m_instanceName + "_CSHL_WidgetTable[widget].FingerThreshold) &&");
                writer.WriteLine("                    (" + m_instanceName + "_CSHL_SlotSignal[offset + pos] > " + m_instanceName + "_CSHL_SlotSignal[offset + posNext]) )");
                writer.WriteLine("                {");
                writer.WriteLine("                    localMaxValue = " + m_instanceName + "_CSHL_SlotSignal[offset + pos];");
                writer.WriteLine("                }");
                writer.WriteLine("                else ");
                writer.WriteLine("                {");
                writer.WriteLine("                    if ((" + m_instanceName + "_CSHL_SlotSignal[offset + pos] > " + m_instanceName + "_CSHL_WidgetTable[widget].FingerThreshold) &&");
                writer.WriteLine("                        (" + m_instanceName + "_CSHL_SlotSignal[offset + pos] == " + m_instanceName + "_CSHL_SlotSignal[offset + posNext]))");
                writer.WriteLine("                    {");
                writer.WriteLine("                        if(" + m_instanceName + "_CSHL_SlotBaselineLow[offset + pos] < " + m_instanceName + "_CSHL_SlotBaselineLow[offset + posNext])");
                writer.WriteLine("                        {");
                writer.WriteLine("                            localMaxValue = " + m_instanceName + "_CSHL_SlotSignal[offset + pos];");
                writer.WriteLine("                        }");
                writer.WriteLine("                    }");
                writer.WriteLine("                }");
                writer.WriteLine("                ");
                writer.WriteLine("                /* Right side */");
                writer.WriteLine("                if (" + m_instanceName + "_CSHL_WidgetTable[widget].Type & " + m_instanceName + "_CSHL_IS_DIPLEX)");
                writer.WriteLine("                {");
                writer.WriteLine("                    pos = diplex[(firstMaxSns + maxLength-1)%signalsNumberX2];");
                writer.WriteLine("                    posNext = diplex[(firstMaxSns + maxLength-2)%signalsNumberX2];");
                writer.WriteLine("                }");
                writer.WriteLine("                else ");
                writer.WriteLine("                {");
                writer.WriteLine("                    pos = (firstMaxSns + maxLength-1)%signalsNumberX2;");
                writer.WriteLine("                    posNext = (firstMaxSns + maxLength-2)%signalsNumberX2;");
                writer.WriteLine("                }");
                writer.WriteLine("                ");
                writer.WriteLine("                if ((" + m_instanceName + "_CSHL_SlotSignal[offset + pos] > " + m_instanceName + "_CSHL_WidgetTable[widget].FingerThreshold) &&");
                writer.WriteLine("                    (" + m_instanceName + "_CSHL_SlotSignal[offset + pos] > " + m_instanceName + "_CSHL_SlotSignal[offset + posNext]) )");
                writer.WriteLine("                {");
                writer.WriteLine("                    localMaxValue = " + m_instanceName + "_CSHL_SlotSignal[offset +pos];");
                writer.WriteLine("                }");
                writer.WriteLine("                else ");
                writer.WriteLine("                {    ");
                writer.WriteLine("                    if ((" + m_instanceName + "_CSHL_SlotSignal[offset + pos] > " + m_instanceName + "_CSHL_WidgetTable[widget].FingerThreshold) &&");
                writer.WriteLine("                        (" + m_instanceName + "_CSHL_SlotSignal[offset + pos] == " + m_instanceName + "_CSHL_SlotSignal[offset + posNext]) )");
                writer.WriteLine("                    {");
                writer.WriteLine("                        if (" + m_instanceName + "_CSHL_SlotBaselineLow[offset + pos] < " + m_instanceName + "_CSHL_SlotBaselineLow[offset + posNext])");
                writer.WriteLine("                        {");
                writer.WriteLine("                            localMaxValue = " + m_instanceName + "_CSHL_SlotSignal[offset +pos];");
                writer.WriteLine("                        }");
                writer.WriteLine("                    }");
                writer.WriteLine("                    else");
                writer.WriteLine("                    {");
                writer.WriteLine("                        /* Inside */");
                writer.WriteLine("                        for (i = firstMaxSns + 1; i < firstMaxSns + maxLength - 1; i++)");
                writer.WriteLine("                        {");
                writer.WriteLine("                            if (" + m_instanceName + "_CSHL_WidgetTable[widget].Type & " + m_instanceName + "_CSHL_IS_DIPLEX)");
                writer.WriteLine("                            {");
                writer.WriteLine("                                pos = diplex[i%signalsNumberX2];");
                writer.WriteLine("                                posPrev = diplex[(i-1)%signalsNumberX2];");
                writer.WriteLine("                                posNext = diplex[(i+1)%signalsNumberX2];");
                writer.WriteLine("                            }");
                writer.WriteLine("                            else ");
                writer.WriteLine("                            {");
                writer.WriteLine("                                pos = i%signalsNumberX2;");
                writer.WriteLine("                                posPrev = (i-1)%signalsNumberX2;");
                writer.WriteLine("                                posNext = (i+1)%signalsNumberX2;");
                writer.WriteLine("                            }");
                writer.WriteLine("                            ");
                writer.WriteLine("                            if (" + m_instanceName + "_CSHL_SlotSignal[offset + pos] > " + m_instanceName + "_CSHL_WidgetTable[widget].FingerThreshold)");
                writer.WriteLine("                            {");
                writer.WriteLine("                                if ((" + m_instanceName + "_CSHL_SlotSignal[offset + pos] > " + m_instanceName + "_CSHL_SlotSignal[offset + posPrev]) &&");
                writer.WriteLine("                                    (" + m_instanceName + "_CSHL_SlotSignal[offset + pos] > " + m_instanceName + "_CSHL_SlotSignal[offset +posNext]) )");
                writer.WriteLine("                                {");
                writer.WriteLine("                                    localMaxValue = " + m_instanceName + "_CSHL_SlotSignal[offset +pos];");
                writer.WriteLine("                                    break;");
                writer.WriteLine("                                }");
                writer.WriteLine("                                else ");
                writer.WriteLine("                                {    ");
                writer.WriteLine("                                    if ((" + m_instanceName + "_CSHL_SlotSignal[offset + pos] > " + m_instanceName + "_CSHL_SlotSignal[offset + posPrev]) &&");
                writer.WriteLine("                                        (" + m_instanceName + "_CSHL_SlotSignal[offset + pos] == " + m_instanceName + "_CSHL_SlotSignal[offset + posNext]))");
                writer.WriteLine("                                    {");
                writer.WriteLine("                                        if(" + m_instanceName + "_CSHL_SlotBaselineLow[offset + pos] < " + m_instanceName + "_CSHL_SlotBaselineLow[offset + posNext])");
                writer.WriteLine("                                        {");
                writer.WriteLine("                                            localMaxValue = " + m_instanceName + "_CSHL_SlotSignal[offset +pos];");
                writer.WriteLine("                                            break;");
                writer.WriteLine("                                        }");
                writer.WriteLine("                                    }");
                writer.WriteLine("                                    else ");
                writer.WriteLine("                                    {");
                writer.WriteLine("                                        if ((" + m_instanceName + "_CSHL_SlotSignal[offset + pos] == " + m_instanceName + "_CSHL_SlotSignal[offset + posPrev]) &&");
                writer.WriteLine("                                            (" + m_instanceName + "_CSHL_SlotSignal[offset + pos] > " + m_instanceName + "_CSHL_SlotSignal[offset + posNext]))");
                writer.WriteLine("                                        {");
                writer.WriteLine("                                            if(" + m_instanceName + "_CSHL_SlotBaselineLow[offset + pos] < " + m_instanceName + "_CSHL_SlotBaselineLow[offset + posPrev])");
                writer.WriteLine("                                            {");
                writer.WriteLine("                                                localMaxValue = " + m_instanceName + "_CSHL_SlotSignal[offset +pos];");
                writer.WriteLine("                                                break;");
                writer.WriteLine("                                            }");
                writer.WriteLine("                                        }");
                writer.WriteLine("                                        else");
                writer.WriteLine("                                        {");
                writer.WriteLine("                                            if ((" + m_instanceName + "_CSHL_SlotSignal[offset + pos] == " + m_instanceName + "_CSHL_SlotSignal[offset + posPrev]) &&");
                writer.WriteLine("                                                (" + m_instanceName + "_CSHL_SlotSignal[offset + pos] == " + m_instanceName + "_CSHL_SlotSignal[offset + posNext]))");
                writer.WriteLine("                                            {");
                writer.WriteLine("                                                if((" + m_instanceName + "_CSHL_SlotBaselineLow[offset + pos] < " + m_instanceName + "_CSHL_SlotBaselineLow[offset + posPrev]) && ");
                writer.WriteLine("                                                (" + m_instanceName + "_CSHL_SlotBaselineLow[offset + pos] < " + m_instanceName + "_CSHL_SlotBaselineLow[offset + posNext]))");
                writer.WriteLine("                                                {");
                writer.WriteLine("                                                    localMaxValue = " + m_instanceName + "_CSHL_SlotSignal[offset +pos];");
                writer.WriteLine("                                                    break;");
                writer.WriteLine("                                                }");
                writer.WriteLine("                                            }");
                writer.WriteLine("                                        }");
                writer.WriteLine("                                    }");
                writer.WriteLine("                                }");
                writer.WriteLine("                            }");
                writer.WriteLine("                        }");
                writer.WriteLine("                    }");
                writer.WriteLine("                }");
                writer.WriteLine("            }");
                writer.WriteLine("            else /* Only one active sensor */ ");
                writer.WriteLine("            {");
                writer.WriteLine("                pos = firstMaxSns;");
                writer.WriteLine("                if (" + m_instanceName + "_CSHL_SlotSignal[offset + pos] > " + m_instanceName + "_CSHL_WidgetTable[widget].FingerThreshold)");
                writer.WriteLine("                {");
                writer.WriteLine("                    localMaxValue = " + m_instanceName + "_CSHL_SlotSignal[offset + pos];");
                writer.WriteLine("                }");
                writer.WriteLine("            }");
                writer.WriteLine("            ");
                writer.WriteLine("            /* Find angle, position */");
                writer.WriteLine("            if (localMaxValue > 0)");
                writer.WriteLine("            {");
                writer.WriteLine("                /* Calculate angle */");
                writer.WriteLine("                angle = " + m_instanceName + "_CSHL_RotarySlider_GetValue(0, signalsNumberX2, firstMaxSns, maxLength, offset, diplex, " + m_instanceName + "_CSHL_WidgetTable[widget].Type);");
                writer.WriteLine("                ");
                writer.WriteLine("                /* Calculate position */");
                writer.WriteLine("                position = angle * ptr->Resolution / 360;");
                writer.WriteLine("                ");
                writer.WriteLine("                /* Filtering */");
                writer.WriteLine("                ");
                writer.WriteLine("                if (ptr->FirstTime == 0)        /* Initialize the filters */");
                writer.WriteLine("                {");
                writer.WriteLine("                    /* Median filter first time initialization */");
                writer.WriteLine("                    if(" + m_instanceName + "_CSHL_WidgetTable[widget].Filters & " + m_instanceName + "_CSHL_POS_MEDIAN_FILTER)");
                writer.WriteLine("                    {");
                writer.WriteLine("                        ptr->Pos2Median = position;");
                writer.WriteLine("                        ptr->Pos1Median = position;");
                writer.WriteLine("                    }");
                writer.WriteLine("                    ");
                writer.WriteLine("                    /* Averaging filter first time initialization */");
                writer.WriteLine("                    if(" + m_instanceName + "_CSHL_WidgetTable[widget].Filters & " + m_instanceName + "_CSHL_POS_AVERAGING_FILTER)");
                writer.WriteLine("                    {");
                writer.WriteLine("                        ptr->Pos2Averaging = position;");
                writer.WriteLine("                        ptr->Pos1Averaging = position;");
                writer.WriteLine("                    }");
                writer.WriteLine("                    ");
                writer.WriteLine("                    /* IIR filter first time initialization */");
                writer.WriteLine("                    if((" + m_instanceName + "_CSHL_WidgetTable[widget].Filters & " + m_instanceName + "_CSHL_POS_IIR_FILTER_0) ||");
                writer.WriteLine("                        (" + m_instanceName + "_CSHL_WidgetTable[widget].Filters & " + m_instanceName + "_CSHL_POS_IIR_FILTER_1))");
                writer.WriteLine("                    {");
                writer.WriteLine("                        ptr->PosIIR = position;");
                writer.WriteLine("                    }");
                writer.WriteLine("                    ");
                writer.WriteLine("                    /* Jitter filter first time initialization */");
                writer.WriteLine("                    if(" + m_instanceName + "_CSHL_WidgetTable[widget].Filters & " + m_instanceName + "_CSHL_POS_JITTER_FILTER)");
                writer.WriteLine("                    {");
                writer.WriteLine("                        ptr->PosJitter = position;");
                writer.WriteLine("                    }");
                writer.WriteLine("                    ");
                writer.WriteLine("                    ptr->FirstTime = 1;");
                writer.WriteLine("                }");
                writer.WriteLine("                else            /* Do the filtering */");
                writer.WriteLine("                {");
                writer.WriteLine("                    /* Median filter */");
                writer.WriteLine("                    if(" + m_instanceName + "_CSHL_WidgetTable[widget].Filters & " + m_instanceName + "_CSHL_POS_MEDIAN_FILTER)");
                writer.WriteLine("                    {");
                writer.WriteLine("                        basePos = position;");
                writer.WriteLine("                        position = " + m_instanceName + "_CSHL_MedianFilter(position, ptr->Pos1Median, ptr->Pos2Median);");
                writer.WriteLine("                        ptr->Pos2Median = ptr->Pos1Median;");
                writer.WriteLine("                        ptr->Pos1Median = basePos;");
                writer.WriteLine("                    }");
                writer.WriteLine("                    ");
                writer.WriteLine("                    /* Averaging filter */");
                writer.WriteLine("                    if(" + m_instanceName + "_CSHL_WidgetTable[widget].Filters & " + m_instanceName + "_CSHL_POS_AVERAGING_FILTER)");
                writer.WriteLine("                    {");
                writer.WriteLine("                        basePos = position;");
                writer.WriteLine("                        position = " + m_instanceName + "_CSHL_AveragingFilter(position, ptr->Pos1Averaging, ptr->Pos2Averaging);");
                writer.WriteLine("                        ptr->Pos2Averaging = ptr->Pos1Averaging;");
                writer.WriteLine("                        ptr->Pos1Averaging= basePos;");
                writer.WriteLine("                    }");
                writer.WriteLine("                    ");
                writer.WriteLine("                    /* IIR filter */");
                writer.WriteLine("                    if(" + m_instanceName + "_CSHL_WidgetTable[widget].Filters & " + m_instanceName + "_CSHL_POS_IIR_FILTER_0)");
                writer.WriteLine("                    {");
                writer.WriteLine("                        basePos = position;");
                writer.WriteLine("                        position = " + m_instanceName + "_CSHL_IIRFilter(position, ptr->PosIIR," + m_instanceName + "_CSHL_IIR_FILTER_0);");
                writer.WriteLine("                        ptr->PosJitter = basePos;");
                writer.WriteLine("                    }");
                writer.WriteLine("                    else if (" + m_instanceName + "_CSHL_WidgetTable[widget].Filters & " + m_instanceName + "_CSHL_POS_IIR_FILTER_1)");
                writer.WriteLine("                    {");
                writer.WriteLine("                        basePos = position;");
                writer.WriteLine("                        position = " + m_instanceName + "_CSHL_IIRFilter(position, ptr->PosIIR," + m_instanceName + "_CSHL_IIR_FILTER_1);");
                writer.WriteLine("                        ptr->PosJitter = basePos;");
                writer.WriteLine("                    }");
                writer.WriteLine("                    ");
                writer.WriteLine("                    /* Jitter filter */");
                writer.WriteLine("                    if(" + m_instanceName + "_CSHL_WidgetTable[widget].Filters & " + m_instanceName + "_CSHL_POS_JITTER_FILTER)");
                writer.WriteLine("                    {");
                writer.WriteLine("                        basePos = position;");
                writer.WriteLine("                        position = " + m_instanceName + "_CSHL_JitterFilter(position, ptr->PosJitter);");
                writer.WriteLine("                        ptr->PosJitter = basePos;");
                writer.WriteLine("                    }");
                writer.WriteLine("                }");
                writer.WriteLine("            }");
                writer.WriteLine("            else");
                writer.WriteLine("            {");
                writer.WriteLine("                /* Local max doesn't find*/");
                writer.WriteLine("                position = 0xFFFF;");
                writer.WriteLine("            }");
                writer.WriteLine("        }");
                writer.WriteLine("        else");
                writer.WriteLine("        {");
                writer.WriteLine("            /* The Diplex Centriod Size is less than 2 */");
                writer.WriteLine("            position = 0xFFFF;");
                writer.WriteLine("        }");
                writer.WriteLine("    }");
                writer.WriteLine("    else");
                writer.WriteLine("    {");
                writer.WriteLine("        /* This Widget isn't Linear Slider Widget */");
                writer.WriteLine("        position = 0xFFFF;");
                writer.WriteLine("    }");
                writer.WriteLine("    ");
                writer.WriteLine("    return position;");
                writer.WriteLine("}");
                #endregion

                #endregion
            }
            if (packParams.m_cyWidgetsList.GetWidgetsCount(E_SENSOR_TYPE.Touchpad_Col) > 0)
            {
                #region CSHL_GetDoubleCentroidPos

                #region CSHL_CalcCentroid
                writer.WriteLine("/*******************************************************************************");
                writer.WriteLine("* Function Name:   " + m_instanceName + "_CSHL_CalcCentroid");
                writer.WriteLine("********************************************************************************");
                writer.WriteLine("* ");
                writer.WriteLine("* Summary:");
                writer.WriteLine("*  Finds position value of defined centroid.");
                writer.WriteLine("* ");
                writer.WriteLine("* Parameters:");
                writer.WriteLine("*  startIndex:  Start index of cetroid in " + m_instanceName + "_CSHL_SlotSignal[] array");
                writer.WriteLine("*  length: uint8 - Length of centroid");
                writer.WriteLine("*  pos: uint8 - local max of centriod");
                writer.WriteLine("*  Mult: uint16 - multiplier calculated accordingly centriod resolution");
                writer.WriteLine("* ");
                writer.WriteLine("* Return:");
                writer.WriteLine("*  Returns position value of centroid");
                writer.WriteLine("* ");
                writer.WriteLine("* Side Effects:");
                writer.WriteLine("*  If an error occurs during execution of the centroid algorithm, the function");
                writer.WriteLine("*  returns -1.");
                writer.WriteLine("* ");
                writer.WriteLine("**********************************************************************************/");
                writer.WriteLine("uint16 " + m_instanceName + "_CSHL_CalcCentroid(uint8 startIndex, uint8 length, uint8 pos, uint16 Mult)");
                writer.WriteLine("{");
                writer.WriteLine("    int32 numerator = 0;");
                writer.WriteLine("    int32 denominator  = 0;");
                writer.WriteLine("    ");
                writer.WriteLine("    if (pos == startIndex)");
                writer.WriteLine("    {");
                writer.WriteLine("        /* Start of Centroid */");
                writer.WriteLine("        numerator = (int32) " + m_instanceName + "_CSHL_SlotSignal[pos + 1];");
                writer.WriteLine("        denominator  = (int32)" + m_instanceName + "_CSHL_SlotSignal[pos] + (int32)" + m_instanceName + "_CSHL_SlotSignal[pos + 1];");
                writer.WriteLine("    }");
                writer.WriteLine("    else if (pos == startIndex + length - 1)");
                writer.WriteLine("    {");
                writer.WriteLine("        /* End of Centroid */");
                writer.WriteLine("        numerator -= ((int32) " + m_instanceName + "_CSHL_SlotSignal[pos - 1]);");
                writer.WriteLine("        denominator  = (int32) " + m_instanceName + "_CSHL_SlotSignal[pos] + (int32) " + m_instanceName + "_CSHL_SlotSignal[pos - 1];");
                writer.WriteLine("    }");
                writer.WriteLine("    else");
                writer.WriteLine("    {");
                writer.WriteLine("        /* Not first Not last */");
                writer.WriteLine("        numerator = ((int32) " + m_instanceName + "_CSHL_SlotSignal[pos + 1]) - ((int32) " + m_instanceName + "_CSHL_SlotSignal[pos - 1]);");
                writer.WriteLine("        denominator  = ((int32) " + m_instanceName + "_CSHL_SlotSignal[pos - 1]) + ((int32) " + m_instanceName + "_CSHL_SlotSignal[pos]) + ((int32) " + m_instanceName + "_CSHL_SlotSignal[pos + 1]);");
                writer.WriteLine("    }");
                writer.WriteLine("    ");
                writer.WriteLine("    denominator  = (((numerator << 8) / denominator) + ((int32)(pos - startIndex) << 8)) * Mult;");
                writer.WriteLine("    ");
                writer.WriteLine("    return HI16(denominator + 0x7F0u);");
                writer.WriteLine("}");
                writer.WriteLine("");
                writer.WriteLine("");
                #endregion

                #region CSHL_FindLocalMax
                writer.WriteLine("/*******************************************************************************");
                writer.WriteLine("* Function Name:   " + m_instanceName + "_CSHL_FindLocalMax");
                writer.WriteLine("********************************************************************************");
                writer.WriteLine("* ");
                writer.WriteLine("* Summary:");
                writer.WriteLine("*  Finds local maximum of defined centroid.");
                writer.WriteLine("* ");
                writer.WriteLine("* Parameters:");
                writer.WriteLine("*  startIndex:  Start index of cetroid in \" + m_instanceName + \"_CSHL_SlotSignal[] array");
                writer.WriteLine("*  length:  Length of centroid");
                writer.WriteLine("*  fingerthreshould:  Finger threshould of centroid");
                writer.WriteLine("* ");
                writer.WriteLine("* Return:");
                writer.WriteLine("*  Returns local maximum of centroid");
                writer.WriteLine("* ");
                writer.WriteLine("* Side Effects:");
                writer.WriteLine("*   If no sensors are active, the function returns -1. If an error");
                writer.WriteLine("*  occurs during execution of the centroid algorithm, the function");
                writer.WriteLine("*  returns -1.");
                writer.WriteLine("* ");
                writer.WriteLine("*  Note: If noise counts on the slider segments are greater than the noise");
                writer.WriteLine("*  threshold, this subroutine may generate a false local maximum of centroid.");
                writer.WriteLine("*  The noise threshold should be set carefully (high enough above the noise"); 
                writer.WriteLine("*  level) so that noise will not generate a false local maximum.");
                writer.WriteLine("* ");
                writer.WriteLine("**********************************************************************************/");
                writer.WriteLine("uint8 " + m_instanceName + "_CSHL_FindLocalMax(uint8 startIndex, uint8 length, uint8 fingerthreshould)");
                writer.WriteLine("{");
                writer.WriteLine("    uint8 localMax = 0xFFu;");
                writer.WriteLine("    uint8 i;");
                writer.WriteLine("    ");
                writer.WriteLine("    for(i = startIndex; i < startIndex + length; i++)");
                writer.WriteLine("    {");
                writer.WriteLine("        /* Ignore if zero */");
                writer.WriteLine("        if(" + m_instanceName + "_CSHL_SlotSignal[i] > fingerthreshould)");
                writer.WriteLine("        {");
                writer.WriteLine("            if(i == startIndex)");
                writer.WriteLine("            {");
                writer.WriteLine("                /* First element pos > pos1 */");
                writer.WriteLine("                if(" + m_instanceName + "_CSHL_SlotSignal[i] > " + m_instanceName + "_CSHL_SlotSignal[i+1])");
                writer.WriteLine("                {");
                writer.WriteLine("                    localMax = i;");
                writer.WriteLine("                    break;");
                writer.WriteLine("                }");
                writer.WriteLine("                else");
                writer.WriteLine("                {");
                writer.WriteLine("                    if(" + m_instanceName + "_CSHL_SlotSignal[i] == " + m_instanceName + "_CSHL_SlotSignal[i+1])");
                writer.WriteLine("                    {");
                writer.WriteLine("                        /* Compare BaselinesLow (i) and (i+1) */");
                writer.WriteLine("                        if(" + m_instanceName + "_CSHL_SlotBaselineLow[i] < " + m_instanceName + "_CSHL_SlotBaselineLow[i+1])");
                writer.WriteLine("                        {");
                writer.WriteLine("                            localMax = i;");
                writer.WriteLine("                            break;");
                writer.WriteLine("                        }");
                writer.WriteLine("                    }");
                writer.WriteLine("                }");
                writer.WriteLine("            }");
                writer.WriteLine("            else if (i == startIndex + length - 1)");
                writer.WriteLine("            {");
                writer.WriteLine("                /* Last element i > i-1 */");
                writer.WriteLine("                if(" + m_instanceName + "_CSHL_SlotSignal[i] > " + m_instanceName + "_CSHL_SlotSignal[i-1])");
                writer.WriteLine("                {");
                writer.WriteLine("                    localMax = i;");
                writer.WriteLine("                    break;");
                writer.WriteLine("                }");
                writer.WriteLine("                else");
                writer.WriteLine("                {");
                writer.WriteLine("                    if(" + m_instanceName + "_CSHL_SlotSignal[i] == " + m_instanceName + "_CSHL_SlotSignal[i-1])");
                writer.WriteLine("                    {");
                writer.WriteLine("                        /* Compare BaselinesLow (i) and (i-1) */");
                writer.WriteLine("                        if(" + m_instanceName + "_CSHL_SlotBaselineLow[i] < " + m_instanceName + "_CSHL_SlotBaselineLow[i-1])");
                writer.WriteLine("                        {");
                writer.WriteLine("                            localMax = i;");
                writer.WriteLine("                            break;");
                writer.WriteLine("                        }");
                writer.WriteLine("                    }    ");
                writer.WriteLine("                }");
                writer.WriteLine("            }");
                writer.WriteLine("            else");
                writer.WriteLine("            {");
                writer.WriteLine("                /* point (i-1) > (i) and (i) > (i+1) */");
                writer.WriteLine("                if((" + m_instanceName + "_CSHL_SlotSignal[i] > " + m_instanceName + "_CSHL_SlotSignal[i-1]) && (" + m_instanceName + "_CSHL_SlotSignal[i] > " + m_instanceName + "_CSHL_SlotSignal[i+1]))");
                writer.WriteLine("                {");
                writer.WriteLine("                    localMax = i;");
                writer.WriteLine("                    break;");
                writer.WriteLine("                }");
                writer.WriteLine("                else");
                writer.WriteLine("                {");
                writer.WriteLine("                    /* (i-1) == (i), compare BaselinesLow */");
                writer.WriteLine("                    if(" + m_instanceName + "_CSHL_SlotSignal[i] == " + m_instanceName + "_CSHL_SlotSignal[i-1])");
                writer.WriteLine("                    {");
                writer.WriteLine("                        if(" + m_instanceName + "_CSHL_SlotBaselineLow[i] < " + m_instanceName + "_CSHL_SlotBaselineLow[i-1])");
                writer.WriteLine("                        {");
                writer.WriteLine("                            if(" + m_instanceName + "_CSHL_SlotSignal[i] > " + m_instanceName + "_CSHL_SlotSignal[i+1])");
                writer.WriteLine("                            {");
                writer.WriteLine("                                localMax = i;");
                writer.WriteLine("                                break;");
                writer.WriteLine("                            }");
                writer.WriteLine("                            else");
                writer.WriteLine("                            {");
                writer.WriteLine("                                /* (i) == (i+1), compare BaselinesLow */ ");
                writer.WriteLine("                                if((" + m_instanceName + "_CSHL_SlotSignal[i] == " + m_instanceName + "_CSHL_SlotSignal[i+1]) && (" + m_instanceName + "_CSHL_SlotBaselineLow[i] < " + m_instanceName + "_CSHL_SlotBaselineLow[i+1]))");
                writer.WriteLine("                                {");
                writer.WriteLine("                                    localMax = i;");
                writer.WriteLine("                                    break;");
                writer.WriteLine("                                }");
                writer.WriteLine("                            }");
                writer.WriteLine("                        }");
                writer.WriteLine("                    }");
                writer.WriteLine("                    if(" + m_instanceName + "_CSHL_SlotSignal[i] == " + m_instanceName + "_CSHL_SlotSignal[i+1])");
                writer.WriteLine("                    {");
                writer.WriteLine("                        if(" + m_instanceName + "_CSHL_SlotBaselineLow[i] < " + m_instanceName + "_CSHL_SlotBaselineLow[i+1])");
                writer.WriteLine("                        {");
                writer.WriteLine("                            if(" + m_instanceName + "_CSHL_SlotSignal[i] > " + m_instanceName + "_CSHL_SlotSignal[i-1])");
                writer.WriteLine("                            {");
                writer.WriteLine("                                localMax = i;");
                writer.WriteLine("                                break;");
                writer.WriteLine("                            }");
                writer.WriteLine("                            else");
                writer.WriteLine("                            {");
                writer.WriteLine("                                if((" + m_instanceName + "_CSHL_SlotSignal[i] == " + m_instanceName + "_CSHL_SlotSignal[i-1]) && (" + m_instanceName + "_CSHL_SlotBaselineLow[i] < " + m_instanceName + "_CSHL_SlotBaselineLow[i-1]))");
                writer.WriteLine("                                {");
                writer.WriteLine("                                    localMax = i;");
                writer.WriteLine("                                    break;");
                writer.WriteLine("                                }");
                writer.WriteLine("                            }");
                writer.WriteLine("                        }");
                writer.WriteLine("                    }");
                writer.WriteLine("                }");
                writer.WriteLine("            }");
                writer.WriteLine("        }");
                writer.WriteLine("    }");
                writer.WriteLine("    ");
                writer.WriteLine("    return localMax;");
                writer.WriteLine("}");
                writer.WriteLine("");
                writer.WriteLine("");
                #endregion

                #region CSHL_GetDoubleCentroidPos
                writer.WriteLine("/*******************************************************************************");
                writer.WriteLine("* Function Name:   " + m_instanceName + "_CSHL_GetDoubleCentroidPos");
                writer.WriteLine("********************************************************************************");
                writer.WriteLine("* ");
                writer.WriteLine("* Summary:");
                writer.WriteLine("*  If a finger is present on touch pad, this function calculates the X and Y ");
                writer.WriteLine("*  position of the finger by calculating the centroids. The X and Y positions");
                writer.WriteLine("*  are calculated to the resolutions set in the CapSense customizer. ");
                writer.WriteLine("*  The position filters are applied to the result if enabled.");
                writer.WriteLine("* ");
                writer.WriteLine("* Parameters:");
                writer.WriteLine("*  widget:  Widget number. ");
                writer.WriteLine("*  For every touchpad widget there are defines in this format:");
                writer.WriteLine("*  #define " + m_instanceName + "_CSHL_TP_”widget_name”            5");
                writer.WriteLine("* ");
                writer.WriteLine("* Return:");
                writer.WriteLine("*  Returns a 1 if a finger is on the touchpad");
                writer.WriteLine("* ");
                writer.WriteLine("* Side Effects:");
                writer.WriteLine("*  The result of calculation of X and Y position store in global arrays. ");
                writer.WriteLine("*  The arrays name are:");
                writer.WriteLine("*  " + m_instanceName + "_CSHL_TPCol_\"widget_name\"_Results – position of X");
                writer.WriteLine("*  " + m_instanceName + "_CSHL_TPRow_\"widget_name\"_Results – position of Y");
                writer.WriteLine("* ");
                writer.WriteLine("**********************************************************************************/");
                writer.WriteLine("uint16 " + m_instanceName + "_CSHL_GetDoubleCentroidPos(uint8 widget)");
                writer.WriteLine("{");
                writer.WriteLine("    " + m_instanceName + "_TPSettings *ptrX = (" + m_instanceName + "_TPSettings *) " + m_instanceName + "_CSHL_WidgetTable[widget].AdvancedSettings;");
                writer.WriteLine("    " + m_instanceName + "_TPSettings *ptrY = (" + m_instanceName + "_TPSettings *) " + m_instanceName + "_CSHL_WidgetTable[widget + " + m_instanceName + "_CSHL_NUMBER_OF_DOUBLE_STRUCTS].AdvancedSettings;");
                writer.WriteLine("    uint16 posX, posY;");
                writer.WriteLine("    uint8 i, touch = 0;");
                writer.WriteLine("    uint16 basePos;");
                writer.WriteLine("        ");
                writer.WriteLine("    if (" + m_instanceName + "_CSHL_WidgetTable[widget].Type == " + m_instanceName + "_CSHL_TYPE_TOUCHPAD)");
                writer.WriteLine("    {");
                writer.WriteLine("        /* Find if Col or Row we have as widget */");
                writer.WriteLine("        if (widget > " + m_instanceName + "_CSHL_NUMBER_OF_DOUBLE_STRUCTS)");
                writer.WriteLine("        {");
                writer.WriteLine("            /* Always find Col widget */");
                writer.WriteLine("            widget -= " + m_instanceName + "_CSHL_NUMBER_OF_DOUBLE_STRUCTS;");
                writer.WriteLine("        }");
                writer.WriteLine("        ");
                writer.WriteLine("        /* Centroid calculations */");
                writer.WriteLine("        ");
                writer.WriteLine("        for(i = 0; i < " + m_instanceName + "_CSHL_MAX_FINGERS; i++)");
                writer.WriteLine("        {");
                writer.WriteLine("            ");
                writer.WriteLine("            /* Find local maximum for X */");
                writer.WriteLine("            posX = " + m_instanceName + "_CSHL_FindLocalMax(" + m_instanceName + "_CSHL_WidgetTable[widget].RawOffset,");
                writer.WriteLine("                                                " + m_instanceName + "_CSHL_WidgetTable[widget].ScanSlotCount,");
                writer.WriteLine("                                                " + m_instanceName + "_CSHL_WidgetTable[widget].FingerThreshold);");
                writer.WriteLine("            ");
                writer.WriteLine("            if (posX < 0xFFu)");
                writer.WriteLine("            {");
                writer.WriteLine("                posX = " + m_instanceName + "_CSHL_CalcCentroid(" + m_instanceName + "_CSHL_WidgetTable[widget].RawOffset,");
                writer.WriteLine("                                                    " + m_instanceName + "_CSHL_WidgetTable[widget].ScanSlotCount, ");
                writer.WriteLine("                                                    posX, ");
                writer.WriteLine("                                                    ptrX->Resolution);");
                writer.WriteLine("                ");
                writer.WriteLine("                /* Filtering */");
                writer.WriteLine("                ");
                writer.WriteLine("                if (ptrX->FirstTime == 0)                /* Initialize the filters */");
                writer.WriteLine("                {");
                writer.WriteLine("                    /* Median filter first time initialization */");
                writer.WriteLine("                    if(" + m_instanceName + "_CSHL_WidgetTable[widget].Filters & " + m_instanceName + "_CSHL_POS_MEDIAN_FILTER)");
                writer.WriteLine("                    {");
                writer.WriteLine("                        ptrX->Pos2Median[i] = posX;");
                writer.WriteLine("                        ptrX->Pos1Median[i] = posX;");
                writer.WriteLine("                    }");
                writer.WriteLine("                    ");
                writer.WriteLine("                    /* Averaging filter first time initialization */");
                writer.WriteLine("                    if(" + m_instanceName + "_CSHL_WidgetTable[widget].Filters & " + m_instanceName + "_CSHL_POS_AVERAGING_FILTER)");
                writer.WriteLine("                    {");
                writer.WriteLine("                        ptrX->Pos2Averaging[i] = posX;");
                writer.WriteLine("                        ptrX->Pos1Averaging[i] = posX;");
                writer.WriteLine("                    }");
                writer.WriteLine("                    ");
                writer.WriteLine("                    /* IIR filter first time initialization */");
                writer.WriteLine("                    if((" + m_instanceName + "_CSHL_WidgetTable[widget].Filters & " + m_instanceName + "_CSHL_POS_IIR_FILTER_0) ||");
                writer.WriteLine("                        (" + m_instanceName + "_CSHL_WidgetTable[widget].Filters & " + m_instanceName + "_CSHL_POS_IIR_FILTER_1))");
                writer.WriteLine("                    {");
                writer.WriteLine("                        ptrX->PosIIR[i] = posX;");
                writer.WriteLine("                    }");
                writer.WriteLine("                    ");
                writer.WriteLine("                    /* Jitter filter first time initialization */");
                writer.WriteLine("                    if(" + m_instanceName + "_CSHL_WidgetTable[widget].Filters & " + m_instanceName + "_CSHL_POS_JITTER_FILTER)");
                writer.WriteLine("                    {");
                writer.WriteLine("                        ptrX->PosJitter[i] = posX;");
                writer.WriteLine("                    }");
                writer.WriteLine("                    ");
                writer.WriteLine("                    ptrX->FirstTime = 1;");
                writer.WriteLine("                }");
                writer.WriteLine("                else                            /* Do the filtering */");
                writer.WriteLine("                {");
                writer.WriteLine("                    /* Median filter */");
                writer.WriteLine("                    if(" + m_instanceName + "_CSHL_WidgetTable[widget].Filters & " + m_instanceName + "_CSHL_POS_MEDIAN_FILTER)");
                writer.WriteLine("                    {");
                writer.WriteLine("                        basePos = posX;");
                writer.WriteLine("                        posX = " + m_instanceName + "_CSHL_MedianFilter(posX, ptrX->Pos1Median[i], ptrX->Pos2Median[i]);");
                writer.WriteLine("                        ptrX->Pos2Median[i] = ptrX->Pos1Median[i];");
                writer.WriteLine("                        ptrX->Pos1Median[i] = basePos;");
                writer.WriteLine("                    }");
                writer.WriteLine("                    ");
                writer.WriteLine("                    /* Averaging filter first time initialization */");
                writer.WriteLine("                    if(" + m_instanceName + "_CSHL_WidgetTable[widget].Filters & " + m_instanceName + "_CSHL_POS_AVERAGING_FILTER)");
                writer.WriteLine("                    {");
                writer.WriteLine("                        basePos = posX;");
                writer.WriteLine("                        posX = " + m_instanceName + "_CSHL_AveragingFilter(posX, ptrX->Pos1Averaging[i], ptrX->Pos2Averaging[i]);");
                writer.WriteLine("                        ptrX->Pos2Averaging[i] = ptrX->Pos1Averaging[i];");
                writer.WriteLine("                        ptrX->Pos1Averaging[i] = basePos;");
                writer.WriteLine("                    }");
                writer.WriteLine("                    ");
                writer.WriteLine("                    /* IIR filter */");
                writer.WriteLine("                    if(" + m_instanceName + "_CSHL_WidgetTable[widget].Filters & " + m_instanceName + "_CSHL_POS_IIR_FILTER_0)");
                writer.WriteLine("                    {");
                writer.WriteLine("                        basePos = posX;");
                writer.WriteLine("                        posX = " + m_instanceName + "_CSHL_IIRFilter(posX, ptrX->PosIIR[i], " + m_instanceName + "_CSHL_IIR_FILTER_0);");
                writer.WriteLine("                        ptrX->PosJitter[i] = basePos;");
                writer.WriteLine("                    }");
                writer.WriteLine("                    else if (" + m_instanceName + "_CSHL_WidgetTable[widget].Filters & " + m_instanceName + "_CSHL_POS_IIR_FILTER_1)");
                writer.WriteLine("                    {");
                writer.WriteLine("                        basePos = posX;");
                writer.WriteLine("                        posX = " + m_instanceName + "_CSHL_IIRFilter(posX, ptrX->PosIIR[i], " + m_instanceName + "_CSHL_IIR_FILTER_1);");
                writer.WriteLine("                        ptrX->PosJitter[i] = basePos;");
                writer.WriteLine("                    }");
                writer.WriteLine("                    ");
                writer.WriteLine("                    /* Jitter filter */");
                writer.WriteLine("                    if(" + m_instanceName + "_CSHL_WidgetTable[widget].Filters & " + m_instanceName + "_CSHL_POS_JITTER_FILTER)");
                writer.WriteLine("                    {");
                writer.WriteLine("                        basePos = posX;");
                writer.WriteLine("                        posX = " + m_instanceName + "_CSHL_JitterFilter(posX, ptrX->PosJitter[i]);");
                writer.WriteLine("                        ptrX->PosJitter[i] = basePos;");
                writer.WriteLine("                    }");
                writer.WriteLine("                }");
                writer.WriteLine("                /* Save Results for X */");
                writer.WriteLine("                ptrX->Position[i] = posX;");
                writer.WriteLine("                touch = 0x01u;");
                writer.WriteLine("            }");
                writer.WriteLine("            else");
                writer.WriteLine("            {");
                writer.WriteLine("                ptrX->FirstTime = 0;");
                writer.WriteLine("            }    ");
                writer.WriteLine("            ");
                writer.WriteLine("            /* Find local maximum for Y */");
                writer.WriteLine("            posY = " + m_instanceName + "_CSHL_FindLocalMax(" + m_instanceName + "_CSHL_WidgetTable[widget+" + m_instanceName + "_CSHL_NUMBER_OF_DOUBLE_STRUCTS].RawOffset,");
                writer.WriteLine("                                                    " + m_instanceName + "_CSHL_WidgetTable[widget + " + m_instanceName + "_CSHL_NUMBER_OF_DOUBLE_STRUCTS].ScanSlotCount,");
                writer.WriteLine("                                                    " + m_instanceName + "_CSHL_WidgetTable[widget+" + m_instanceName + "_CSHL_NUMBER_OF_DOUBLE_STRUCTS].FingerThreshold);");
                writer.WriteLine("            if (posY < 0xFFu)");
                writer.WriteLine("            {");
                writer.WriteLine("                /* Calculate centroid */");
                writer.WriteLine("                posY = " + m_instanceName + "_CSHL_CalcCentroid(" + m_instanceName + "_CSHL_WidgetTable[widget+" + m_instanceName + "_CSHL_NUMBER_OF_DOUBLE_STRUCTS].RawOffset, ");
                writer.WriteLine("                                                    " + m_instanceName + "_CSHL_WidgetTable[widget+" + m_instanceName + "_CSHL_NUMBER_OF_DOUBLE_STRUCTS].ScanSlotCount,");
                writer.WriteLine("                                                    posY, ");
                writer.WriteLine("                                                    ptrY->Resolution);");
                writer.WriteLine("                ");
                writer.WriteLine("                if (ptrY->FirstTime == 0)        /* Initialize the filters */");
                writer.WriteLine("                {");
                writer.WriteLine("                    /* Median filter first time initialization */");
                writer.WriteLine("                    if(" + m_instanceName + "_CSHL_WidgetTable[widget].Filters & " + m_instanceName + "_CSHL_POS_MEDIAN_FILTER)");
                writer.WriteLine("                    {");
                writer.WriteLine("                        ptrY->Pos2Median[i] = posY;");
                writer.WriteLine("                        ptrY->Pos1Median[i]= posY;");
                writer.WriteLine("                    }");
                writer.WriteLine("                    ");
                writer.WriteLine("                    /* Averaging filter first time initialization */");
                writer.WriteLine("                    if(" + m_instanceName + "_CSHL_WidgetTable[widget].Filters & " + m_instanceName + "_CSHL_POS_AVERAGING_FILTER)");
                writer.WriteLine("                    {");
                writer.WriteLine("                        ptrY->Pos2Averaging[i] = posY;");
                writer.WriteLine("                        ptrY->Pos1Averaging[i] = posY;");
                writer.WriteLine("                    }");
                writer.WriteLine("                    /* IIR filter first time initialization */");
                writer.WriteLine("                    if((" + m_instanceName + "_CSHL_WidgetTable[widget].Filters & " + m_instanceName + "_CSHL_POS_IIR_FILTER_0) ||");
                writer.WriteLine("                        (" + m_instanceName + "_CSHL_WidgetTable[widget].Filters & " + m_instanceName + "_CSHL_POS_IIR_FILTER_1))");
                writer.WriteLine("                    {");
                writer.WriteLine("                        ptrY->PosIIR[i] = posY;");
                writer.WriteLine("                    }");
                writer.WriteLine("                    ");
                writer.WriteLine("                    /* Jitter filter first time initialization */");
                writer.WriteLine("                    if(" + m_instanceName + "_CSHL_WidgetTable[widget].Filters & " + m_instanceName + "_CSHL_POS_JITTER_FILTER)");
                writer.WriteLine("                    {");
                writer.WriteLine("                        ptrY->PosJitter[i] = posY;");
                writer.WriteLine("                    }");
                writer.WriteLine("                    ");
                writer.WriteLine("                    ptrY->FirstTime = 1;");
                writer.WriteLine("                }");
                writer.WriteLine("                else            /* Do the filtering */");
                writer.WriteLine("                {");
                writer.WriteLine("                    /* Median filter */");
                writer.WriteLine("                    if(" + m_instanceName + "_CSHL_WidgetTable[widget].Filters & " + m_instanceName + "_CSHL_POS_MEDIAN_FILTER)");
                writer.WriteLine("                    {");
                writer.WriteLine("                        basePos = posY;");
                writer.WriteLine("                        posY = " + m_instanceName + "_CSHL_MedianFilter(posY, ptrY->Pos1Median[i], ptrY->Pos2Median[i]);");
                writer.WriteLine("                        ptrY->Pos2Median[i]  = ptrY->Pos1Median[i];");
                writer.WriteLine("                        ptrY->Pos1Median[i]  = basePos;");
                writer.WriteLine("                    }");
                writer.WriteLine("                    ");
                writer.WriteLine("                    /* Averaging filter */");
                writer.WriteLine("                    if(" + m_instanceName + "_CSHL_WidgetTable[widget].Filters & " + m_instanceName + "_CSHL_POS_AVERAGING_FILTER)");
                writer.WriteLine("                    {");
                writer.WriteLine("                        basePos = posY;");
                writer.WriteLine("                        posY = " + m_instanceName + "_CSHL_AveragingFilter(posY, ptrY->Pos1Averaging[i], ptrY->Pos2Averaging[i]);");
                writer.WriteLine("                        ptrY->Pos2Averaging[i] = ptrY->Pos1Averaging[i];");
                writer.WriteLine("                        ptrY->Pos1Averaging[i] = basePos;");
                writer.WriteLine("                    }");
                writer.WriteLine("                    ");
                writer.WriteLine("                    /* IIR filter */");
                writer.WriteLine("                    if(" + m_instanceName + "_CSHL_WidgetTable[widget].Filters & " + m_instanceName + "_CSHL_POS_IIR_FILTER_0)");
                writer.WriteLine("                    {");
                writer.WriteLine("                        basePos = posY;");
                writer.WriteLine("                        posY = " + m_instanceName + "_CSHL_IIRFilter(posY, ptrY->PosIIR[i], " + m_instanceName + "_CSHL_IIR_FILTER_0);");
                writer.WriteLine("                        ptrY->PosIIR[i] = basePos;");
                writer.WriteLine("                    }");
                writer.WriteLine("                    else if (" + m_instanceName + "_CSHL_WidgetTable[widget].Filters & " + m_instanceName + "_CSHL_POS_IIR_FILTER_1)");
                writer.WriteLine("                    {");
                writer.WriteLine("                        basePos = posY;");
                writer.WriteLine("                        posY = " + m_instanceName + "_CSHL_IIRFilter(posY, ptrY->PosIIR[i], " + m_instanceName + "_CSHL_IIR_FILTER_1);");
                writer.WriteLine("                        ptrY->PosIIR[i] = basePos;");
                writer.WriteLine("                    }");
                writer.WriteLine("                    ");
                writer.WriteLine("                    /* Jitter filter */");
                writer.WriteLine("                    if(" + m_instanceName + "_CSHL_WidgetTable[widget].Filters & " + m_instanceName + "_CSHL_POS_JITTER_FILTER)");
                writer.WriteLine("                    {");
                writer.WriteLine("                        basePos = posY;");
                writer.WriteLine("                        posY = " + m_instanceName + "_CSHL_JitterFilter(posY, ptrY->PosJitter[i]);");
                writer.WriteLine("                        ptrY->PosJitter[i] = basePos;");
                writer.WriteLine("                    }");
                writer.WriteLine("                }");
                writer.WriteLine("                ");
                writer.WriteLine("                /* Save Results for Y */");
                writer.WriteLine("                ptrY->Position[i] = posY;");
                writer.WriteLine("                touch = 0x01u;");
                writer.WriteLine("            }");
                writer.WriteLine("            else");
                writer.WriteLine("            {");
                writer.WriteLine("                ptrY->FirstTime = 0;");
                writer.WriteLine("            }");
                writer.WriteLine("        }");
                writer.WriteLine("    }");
                writer.WriteLine("    else");
                writer.WriteLine("    {");
                writer.WriteLine("        touch = 0;");
                writer.WriteLine("    }");
                writer.WriteLine("    ");
                writer.WriteLine("    return touch;");
                writer.WriteLine("}");
                #endregion

                #endregion
            }

        }

        #endregion
    }
}

