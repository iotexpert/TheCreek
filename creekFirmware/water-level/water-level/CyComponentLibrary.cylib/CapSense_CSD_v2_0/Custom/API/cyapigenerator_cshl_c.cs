/*******************************************************************************
* Copyright 2008-2010, Cypress Semiconductor Corporation.  All rights reserved.
* You may use this file only in accordance with the license, terms, conditions,
* disclaimers, and limitations in the end user license agreement accompanying
* the software package with which this file was provided.
********************************************************************************/



using System;
using System.Collections.Generic;
using System.Text;
using System.IO;

namespace CapSense_CSD_v2_0
{
    public partial class CyAPIGenerator
    {
        public void CollectApiCSHLCFile(ref Dictionary<string, string> paramDict)
        {
            StringBuilder writer = new StringBuilder();
            ApiCollectCSHLVariables(ref writer);
            paramDict.Add("writerCSHLCVariables", writer.ToString());

            writer = new StringBuilder();
            ApiCollectCSHLDebounceInit(ref writer);
            paramDict.Add("writerCSHLDebounceInit", writer.ToString());

            writer = new StringBuilder();
            ApiCollectCSHLEnWidget(ref writer);
            paramDict.Add("writerCSHLEnWidget", writer.ToString());

            writer = new StringBuilder();
            ApiCollectCSHLDisWidget(ref writer);
            paramDict.Add("writerCSHLDisWidget", writer.ToString());

            writer = new StringBuilder();
            ApiCollectCSHLIsSensorActive(ref writer);
            paramDict.Add("writerCSHLIsSensor", writer.ToString());

            writer = new StringBuilder();
            ApiCollectCSHLIsWidget(ref writer);
            paramDict.Add("writerCSHLIsWidget", writer.ToString());

            writer = new StringBuilder();
            ApiCollectCSHLIsAnyWidget(ref writer);
            paramDict.Add("writerCSHLIsAnyWidget", writer.ToString());

            writer = new StringBuilder();
            ApiCollectCSHLFunctionCentroid(ref writer);
            paramDict.Add("writerCSHLCCode", writer.ToString());
        }

        #region apiCollectCSHLVariables
        void ApiCollectCSHLVariables(ref StringBuilder writer)
        {
            CyTuningProperties props;

            #region fingerThreshold__TABLE
            writer.AppendLine(m_constTuner + "uint8" + m_cycodeTuner + m_instanceName + "_fingerThreshold[] = {");
            writer.Append("    ");
            for (int i = 0; i < m_listWidgetCSHL.Count; i++)
            {
                props = m_packParams.m_widgets.GetWidgetsProperties(m_listWidgetCSHL[i]);
                string value = props.m_fingerThreshold.ToString();

                writer.AppendFormat("{0}u, ", value);
            }
            writer.AppendLine("");
            writer.AppendLine("};");
            writer.AppendLine("");
            #endregion

            #region noiseThreshold__TABLE
            writer.AppendLine(m_constTuner + "uint8" + m_cycodeTuner + m_instanceName + "_noiseThreshold[] = {");
            writer.Append("    ");
            for (int i = 0; i < m_listWidgetCSHL.Count; i++)
            {
                props = m_packParams.m_widgets.GetWidgetsProperties(m_listWidgetCSHL[i]);
                string value = props.m_noiseThreshold.ToString();

                writer.AppendFormat("{0}u, ", value);
            }
            writer.AppendLine("");
            writer.AppendLine("};");
            writer.AppendLine("");
            #endregion

            #region hysteresis__TABLE
            writer.AppendLine(m_constTuner + "uint8" + m_cycodeTuner + m_instanceName + "_hysteresis[] = {");
            writer.Append("    ");
            for (int i = 0; i < m_listWidgetCSHL.Count; i++)
            {
                props = m_packParams.m_widgets.GetWidgetsProperties(m_listWidgetCSHL[i]);
                string value = props.m_hysteresis.ToString();
                if (i < m_numberOfCentroids)
                    value = "0";
                writer.AppendFormat("{0}u, ", value);
            }
            writer.AppendLine("");
            writer.AppendLine("};");
            writer.AppendLine("");
            #endregion

            #region debounce__TABLE
            writer.AppendLine(m_constTuner + "uint8" + m_cycodeTuner + m_instanceName + "_debounce[] = {");
            writer.Append("    ");
            for (int i = 0; i < m_listWidgetCSHL.Count; i++)
            {
                props = m_packParams.m_widgets.GetWidgetsProperties(m_listWidgetCSHL[i]);
                string value = props.m_debounce.ToString();
                if (i < m_numberOfCentroids)
                    value = "0";
                writer.AppendFormat("{0}u, ", value);
            }
            writer.AppendLine("");
            writer.AppendLine("};");
            writer.AppendLine("");
            #endregion

            #region debounceCounter__TABLE
            writer.AppendLine("uint8 " + m_instanceName + "_debounceCounter[] = {");
            writer.Append("    ");

            int non_centroidWidegetsCount = m_listWidgetCSHL.Count - m_numberOfCentroids;
            int offset = non_centroidWidegetsCount;
            for (int i = m_numberOfCentroids; i < m_listWidgetCSHL.Count; i++)
            {
                int count = m_listWidgetCSHL[i].GetCount();
                //Calculate value
                int value = count > 1 ? offset : 0;
                //move offset
                if (count > 1)
                    offset += count;
                //Add data to api
                writer.AppendFormat("{0}u, ", value);
            }
            //Fill array till end
            for (int i = 0; i < offset - non_centroidWidegetsCount + 1; i++)
            {
                writer.AppendFormat("{0}u, ", 0);
            }
            System.Diagnostics.Debug.Assert(offset == m_debounceMaxOffset);
            writer.AppendLine("");
            writer.AppendLine("};");
            writer.AppendLine("");
            #endregion

            #region rawDataIndex__TABLE
            writer.AppendLine("const uint8 CYCODE " + m_instanceName + "_rawDataIndex[] = {");
            for (int i = 0; i < m_listWidget.Count; i++)
            {
                int j = 0;
                while (m_listSS[j].m_widget != m_listWidget[i]) j++;
                System.Diagnostics.Debug.Assert(j < m_listSS.Count);
                writer.AppendFormat("    {0}u, /* {1} */", j, m_listWidget[i].ToString());
                writer.AppendLine();
            }
            writer.AppendLine("");
            writer.AppendLine("};");
            writer.AppendLine("");
            #endregion

            #region numberOfSensors__TABLE
            writer.AppendLine("const uint8 CYCODE " + m_instanceName + "_numberOfSensors[] = {");
            for (int i = 0; i < m_listWidget.Count; i++)
            {
                writer.AppendFormat("    {0}u, /* {1} */", m_listWidget[i].GetCount(), m_listWidget[i].ToString());
                writer.AppendLine();
            }
            writer.AppendLine("");
            writer.AppendLine("};");
            writer.AppendLine("");
            #endregion

            if (m_isAnyCentroid)
            {
                #region centroidMult__TABLE
                writer.AppendLine("const uint16 CYCODE " + m_instanceName + "_centroidMult[] = {");
                writer.Append("    ");
                for (int i = 0; i < m_numberOfCentroids; i++)
                {
                    int res = m_listWidgetCSHL[i].GetFullResolution();

                    writer.AppendFormat("{0}u, ", res);
                }
                writer.AppendLine("");
                writer.AppendLine("};");
                writer.AppendLine("");
                #endregion
            }

            #region __posFiltersData
            if (m_posFiltersMask != 0)
            {
                m_posFiltersMask = m_posFiltersMask | (int)CyPosFilterOptions.Averaging;

                #region PosFilterMask__TABLE
                writer.AppendLine("const uint8 CYCODE " + m_instanceName + "_posFiltersMask[] = {");
                writer.Append("    ");
                for (int i = 0; i < m_numberOfCentroids; i++)
                {
                    //Get filter
                    int filter = 0;
                    if (m_listWidgetCSHL[i] is CySlider)
                        filter = (int)(m_listWidgetCSHL[i] as CySlider).m_filterPropertiesPos;
                    else if (m_listWidgetCSHL[i] is CyTouchPad)
                        filter = (int)(m_listWidgetCSHL[i] as CyTouchPad).m_positionFilter;
                    //Add data to api
                    writer.AppendFormat("0x{0}u, ", filter);
                }
                writer.AppendLine("");
                writer.AppendLine("};");
                writer.AppendLine("");
                #endregion

                #region PosFilterData__TABLE
                writer.AppendLine("uint8 " + m_instanceName + "_posFiltersData[] = {");
                writer.Append("    ");
                int[] listIndexes = new int[m_numberOfCentroids * 4];
                int id = m_numberOfCentroids;
                for (int i = 0; i < m_numberOfCentroids; i++)
                {
                    //Take filter data
                    CyPosFilterOptions filter = 0;
                    if (m_listWidgetCSHL[i] is CySlider)
                        filter = (m_listWidgetCSHL[i] as CySlider).m_filterPropertiesPos;
                    else if (m_listWidgetCSHL[i] is CyTouchPad)
                        filter = (m_listWidgetCSHL[i] as CyTouchPad).m_positionFilter;

                    //Set Offset
                    listIndexes[i] = id;

                    if (CyCsConst.IsMainPartOfWidget(m_listWidgetCSHL[i].m_type))
                    {
                        ////Set First Time flag(Testing purpose)
                        //listIndexes[id] = 1;
                        //Move offset for First Time flag
                        id++;
                    }

                    //Move offset
                    switch (filter)
                    {
                        case CyPosFilterOptions.Median:
                        case CyPosFilterOptions.Averaging:
                            id += 2;
                            break;
                        case CyPosFilterOptions.FirstOrderIIR0_5:
                        case CyPosFilterOptions.FirstOrderIIR0_75:
                        case CyPosFilterOptions.Jitter:
                            id += 1;
                            break;
                    }
                }
                //Add data to api
                for (int i = 0; i < id; i++)
                    writer.AppendFormat("{0}u, ", listIndexes[i]);

                writer.AppendLine("");
                writer.AppendLine("};");
                writer.AppendLine("");
                #endregion
            }
            #endregion

            if (m_isDiplexSlider)
            {
                #region Diplex__TABLE
                int countDiplexed = GetWidgetCount(CyWidgetAPITypes.SliderLinearDiplexed);

                offset = countDiplexed;
                string[] listLines = new string[countDiplexed + 1];
                for (int i = 0; i < countDiplexed; i++)
                {
                    //Add offset to line 
                    listLines[0] += string.Format("{0}u, ", offset);
                    //Add diplexed arrat to line 
                    listLines[i + 1] = AddDiplexing(m_listWidgetCSHL[i]);
                    //Move offset
                    offset += m_listWidgetCSHL[i].GetCount() * 2;
                }
                writer.AppendLine("const uint8 CYCODE " + m_instanceName + "_diplexTable[] = {");

                //Add data to api
                for (int i = 0; i < listLines.Length; i++)
                {
                    writer.AppendLine(string.Format("    {0} /*{1}*/", listLines[i],
                        i == 0 ? "Offsets" : m_listWidgetCSHL[i - 1].ToString()));
                }
                writer.AppendLine("");
                writer.AppendLine("};");
                writer.AppendLine("");
                #endregion
            }
        }
        #endregion

        #region apiCollectCSHLDebounceInit
        void ApiCollectCSHLDebounceInit(ref StringBuilder writer)
        {

            #region Tabs
            string tab1, tab2, tab3;
            /* Add Tab if centroid */
            if (IsAnyCentroid())
            {
                tab1 = "    ";
            }
            else
            {
                tab1 = "";
            }
            /* Add Tab if Button, Prox and MB exists */
            if (IsWidgetCSHL(CyWidgetAPITypes.Button) || IsWidgetCSHL(CyWidgetAPITypes.Proximity) || IsWidgetCSHL(CyWidgetAPITypes.MatrixButton))
            {
                tab2 = "    ";
            }
            else
            {
                tab2 = "";
            }

            /* Remove Tab if only one exist: (Button and Prox) OR MB */
            if (((IsWidgetCSHL(CyWidgetAPITypes.Button) || IsWidgetCSHL(CyWidgetAPITypes.Proximity)) && (IsWidgetCSHL(CyWidgetAPITypes.MatrixButton) == false)) ||
                 (((IsWidgetCSHL(CyWidgetAPITypes.Button) || IsWidgetCSHL(CyWidgetAPITypes.Proximity)) == false) && IsWidgetCSHL(CyWidgetAPITypes.MatrixButton)))
            {
                tab3 = "";
            }
            else
            {
                tab3 = "    ";
            }
            if (IsWidgetCSHL(CyWidgetAPITypes.Button) || IsWidgetCSHL(CyWidgetAPITypes.Proximity) || IsWidgetCSHL(CyWidgetAPITypes.MatrixButton))
            #endregion

                #region functionCode
                if (IsAnyCentroid() &&
                    (IsWidgetCSHL(CyWidgetAPITypes.Button) || IsWidgetCSHL(CyWidgetAPITypes.Proximity) || IsWidgetCSHL(CyWidgetAPITypes.MatrixButton)))
                {
                    writer.AppendLine(tab1 + "if(widget >= " + m_instanceName + "_TOTAL_CENTROIDS_COUNT)");
                    writer.AppendLine(tab1 + "{");
                }
            if (IsWidgetCSHL(CyWidgetAPITypes.Button) || IsWidgetCSHL(CyWidgetAPITypes.Proximity) || IsWidgetCSHL(CyWidgetAPITypes.MatrixButton))
            {
                writer.AppendLine(tab1 + tab2 + "debounce = " + m_instanceName + "_debounce[widget];");
                if (IsAnyCentroid())
                {
                    writer.AppendLine(tab1 + tab2 + "widget -=  " + m_instanceName + "_TOTAL_CENTROIDS_COUNT;");
                }

                /* Generate for Button and Prox */
                if (IsWidgetCSHL(CyWidgetAPITypes.Button) || IsWidgetCSHL(CyWidgetAPITypes.Proximity))
                {
                    if (IsWidgetCSHL(CyWidgetAPITypes.MatrixButton))
                    {
                        writer.AppendLine(tab1 + tab2 + "if (widget < " + m_instanceName + "_TOTAL_BUTTONS_COUNT) /* Handle buttons */");
                        writer.AppendLine(tab1 + tab2 + "{");
                    }
                    writer.AppendLine(tab1 + tab2 + tab3 + "" + m_instanceName + "_debounceCounter[widget] = debounce;");

                    if (IsWidgetCSHL(CyWidgetAPITypes.MatrixButton))
                    {
                        writer.AppendLine(tab1 + tab2 + "}");
                    }
                }

                /* Generate for Matrix button */
                if (IsWidgetCSHL(CyWidgetAPITypes.MatrixButton))
                {
                    if (IsWidgetCSHL(CyWidgetAPITypes.Button) || IsWidgetCSHL(CyWidgetAPITypes.Proximity))
                    {
                        writer.AppendLine(tab1 + tab2 + "else     /* Handle matrix buttons */");
                        writer.AppendLine(tab1 + tab2 + "{ ");
                    }

                    writer.AppendLine(tab1 + tab2 + tab3 + "debounceIndex  = " + m_instanceName + "_debounceCounter[widget];");
                    writer.AppendLine(tab1 + tab2 + tab3);
                    writer.AppendLine(tab1 + tab2 + tab3 + "/* Calculate position within MB */");
                    writer.Append(tab1 + tab2 + tab3 + "rawDataIndex = " + m_instanceName + "_rawDataIndex[widget");
                    if (IsAnyCentroid())
                    {
                        writer.Append(" + " + m_instanceName + "_TOTAL_CENTROIDS_COUNT");
                    }
                    writer.AppendLine("];");
                    writer.AppendLine(tab1 + tab2 + tab3 + "debounceIndex += (sensor - rawDataIndex);");
                    writer.AppendLine(tab1 + tab2 + tab3 + "");
                    writer.AppendLine(tab1 + tab2 + tab3 + "" + m_instanceName + "_debounceCounter[debounceIndex] = debounce;");

                    if (IsWidgetCSHL(CyWidgetAPITypes.Button) || IsWidgetCSHL(CyWidgetAPITypes.Proximity))
                    {
                        writer.AppendLine(tab1 + tab2 + "}");
                    }
                }
            }
            if (IsAnyCentroid() &&
                (IsWidgetCSHL(CyWidgetAPITypes.Button) || IsWidgetCSHL(CyWidgetAPITypes.Proximity) || IsWidgetCSHL(CyWidgetAPITypes.MatrixButton)))
            {
                writer.AppendLine(tab1 + "}");
            }
                #endregion
        }
        #endregion

        #region apiCollectCSHLIsSensorActive
        void ApiCollectCSHLIsSensorActive(ref StringBuilder writer)
        {

            #region Tabs
            string tab1, tab2, tab3;
            /* Add Tab if centroid */
            if (IsAnyCentroid())
            {
                tab1 = "    ";
            }
            else
            {
                tab1 = "";
            }
            /* Add Tab if Button, Prox and MB exists */
            if (IsWidgetCSHL(CyWidgetAPITypes.Button) || IsWidgetCSHL(CyWidgetAPITypes.Proximity) || IsWidgetCSHL(CyWidgetAPITypes.MatrixButton))
            {
                tab2 = "    ";
            }
            else
            {
                tab2 = "";
            }

            /* Remove Tab if only one exist: (Button and Prox) OR MB */
            if (((IsWidgetCSHL(CyWidgetAPITypes.Button) || IsWidgetCSHL(CyWidgetAPITypes.Proximity)) && (IsWidgetCSHL(CyWidgetAPITypes.MatrixButton) == false)) ||
                 (((IsWidgetCSHL(CyWidgetAPITypes.Button) || IsWidgetCSHL(CyWidgetAPITypes.Proximity)) == false) && IsWidgetCSHL(CyWidgetAPITypes.MatrixButton)))
            {
                tab3 = "";
            }
            else
            {
                tab3 = "    ";
            }
            if (IsWidgetCSHL(CyWidgetAPITypes.Button) || IsWidgetCSHL(CyWidgetAPITypes.Proximity) || IsWidgetCSHL(CyWidgetAPITypes.MatrixButton))
            #endregion

                #region functionCode
                if (IsAnyCentroid() &&
                   (IsWidgetCSHL(CyWidgetAPITypes.Button) || IsWidgetCSHL(CyWidgetAPITypes.Proximity) || IsWidgetCSHL(CyWidgetAPITypes.MatrixButton)))
                {
                    writer.AppendLine(tab1 + "if(widget < " + m_instanceName + "_TOTAL_CENTROIDS_COUNT)");
                    writer.AppendLine(tab1 + "{");
                    writer.AppendLine(tab1 + tab2 + "/* Sliders and TP */");
                    writer.AppendLine(tab1 + tab2 + "debounceIndex = " + m_instanceName + "_UNUSED_DEBOUNCE_COUNTER_INDEX;");
                    writer.AppendLine(tab1 + tab2 + "/* Need to clear common index */");
                    writer.AppendLine(tab1 + tab2 + "" + m_instanceName + "_debounceCounter[debounceIndex] = 0u;");
                    writer.AppendLine(tab1 + "}");
                    writer.AppendLine(tab1 + "else");
                    writer.AppendLine(tab1 + "{");
                    writer.AppendLine(tab1 + tab2 + "widget -= " + m_instanceName + "_TOTAL_CENTROIDS_COUNT;");
                }

            /* Generate for Button and Prox */
            if (IsWidgetCSHL(CyWidgetAPITypes.Button) || IsWidgetCSHL(CyWidgetAPITypes.Proximity))
            {
                if (IsWidgetCSHL(CyWidgetAPITypes.MatrixButton))
                {
                    writer.AppendLine(tab1 + tab2 + "if (widget < " + m_instanceName + "_TOTAL_BUTTONS_COUNT) /* Handle buttons */");
                    writer.AppendLine(tab1 + tab2 + "{");
                }
                writer.AppendLine(tab1 + tab2 + tab3 + "debounceIndex = widget;");

                if (IsWidgetCSHL(CyWidgetAPITypes.MatrixButton))
                {
                    writer.AppendLine(tab1 + tab2 + "}");
                }
            }

            /* Generate for Matrix button */
            if (IsWidgetCSHL(CyWidgetAPITypes.MatrixButton))
            {
                if (IsWidgetCSHL(CyWidgetAPITypes.Button) || IsWidgetCSHL(CyWidgetAPITypes.Proximity))
                {
                    writer.AppendLine(tab1 + tab2 + "else     /* Handle matrix buttons */");
                    writer.AppendLine(tab1 + tab2 + "{ ");
                }

                writer.AppendLine(tab1 + tab2 + tab3 + "debounceIndex  = " + m_instanceName + "_debounceCounter[widget];");
                writer.AppendLine(tab1 + tab2 + tab3);
                writer.AppendLine(tab1 + tab2 + tab3 + "/* Calculate position within MB */");
                writer.Append(tab1 + tab2 + tab3 + "rawDataIndex = " + m_instanceName + "_rawDataIndex[widget");
                if (IsAnyCentroid())
                {
                    writer.Append(" + " + m_instanceName + "_TOTAL_CENTROIDS_COUNT");
                }
                writer.AppendLine("];");
                writer.AppendLine(tab1 + tab2 + tab3 + "debounceIndex += (sensor - rawDataIndex);");

                if (IsWidgetCSHL(CyWidgetAPITypes.Button) || IsWidgetCSHL(CyWidgetAPITypes.Proximity))
                {
                    writer.AppendLine(tab1 + tab2 + "}");
                }
            }

            if (IsAnyCentroid() &&
               (IsWidgetCSHL(CyWidgetAPITypes.Button) || IsWidgetCSHL(CyWidgetAPITypes.Proximity) || IsWidgetCSHL(CyWidgetAPITypes.MatrixButton)))
            {
                writer.AppendLine(tab1 + "}");
            }
                #endregion
        }
        #endregion

        #region apiCollectCSHLEnWidget
        void ApiCollectCSHLEnWidget(ref StringBuilder writer)
        {
            string str1 = "", str2 = "", str = "";
            // Add Touchpad part
            #region TouchPad
            if (IsWidgetCSHL(CyWidgetAPITypes.Touchpad))
            {
                if (IsWidgetCSHL(CyWidgetAPITypes.SliderLinearDiplexed) || IsWidgetCSHL(CyWidgetAPITypes.SliderLinear) ||
                    IsWidgetCSHL(CyWidgetAPITypes.SliderRadial))
                {
                    str1 = "(widget > " + m_instanceName + "_END_OF_SLIDERS_INDEX)";
                }

                if (IsWidgetCSHL(CyWidgetAPITypes.Button) || IsWidgetCSHL(CyWidgetAPITypes.Proximity))
                {
                    str2 = "(widget < " + m_instanceName + "_END_OF_TOUCH_PAD_INDEX)";
                }

                if (str1 != "" && str2 != "")
                {
                    str = "(" + str1 + " && " + str2 + ")";
                }
                else if (str1 != "")
                {
                    str = str1;
                }
                else
                {
                    str = str2;
                }

            }
            #endregion

            // Add Matrix Button part
            #region Matrix Button
            if (IsWidgetCSHL(CyWidgetAPITypes.MatrixButton))
            {
                // Any widget before Matrix Buttons
                if (IsWidgetCSHL(CyWidgetAPITypes.SliderLinearDiplexed) ||
                    IsWidgetCSHL(CyWidgetAPITypes.SliderLinear) ||
                    IsWidgetCSHL(CyWidgetAPITypes.SliderRadial) ||
                    IsWidgetCSHL(CyWidgetAPITypes.Button) ||
                    IsWidgetCSHL(CyWidgetAPITypes.Proximity))
                {
                    str1 = "(widget > " + m_instanceName + "_END_OF_BUTTONS_INDEX)";
                }
                else
                {
                    str1 = "";
                }

                // Generic Widget after Matrix Buttons
                if (IsWidgetCSHL(CyWidgetAPITypes.Generic))
                {
                    if (str1 != "")
                    {
                        str1 = "(" + str1 + " && (widget <= " + m_instanceName + "_TOTAL_WIDGET_COUNT))";
                    }
                    else
                    {
                        str1 = "(widget <= " + m_instanceName + "_TOTAL_WIDGET_COUNT)";
                    }
                }

                // Concatenate string
                if (str != "" && str1 != "")
                {
                    str += " || \\" + "\r\n         " + str1;
                    if (str.Contains("&&"))
                    {
                        str = "( " + str + " )";
                    }
                }
                else if (str1 != "")
                {
                    str = str1;
                }
                else
                {
                    // Do nothing empty str
                }
            }


            #endregion

            #region functionCode
            if (str != "")
            {
                writer.AppendLine("    if " + str + "");
                writer.AppendLine("    {");
                writer.AppendLine("        numberOfSensors = " + m_instanceName + "_numberOfSensors[widget + 1u];");
                writer.AppendLine("        rawIndex = " + m_instanceName + "_rawDataIndex[widget + 1u];");
                writer.AppendLine("        numberOfSensors += rawIndex;");
                writer.AppendLine("        ");
                writer.AppendLine("        /* Disable all sensors of the widget */");
                writer.AppendLine("        do");
                writer.AppendLine("        {");
                writer.AppendLine("            pos = (rawIndex >> 3u);");
                writer.AppendLine("            enMask = 0x01u << (rawIndex & 0x07u);");
                writer.AppendLine("            ");
                writer.AppendLine("            " + m_instanceName + "_SensorEnableMask[pos] |= enMask;");
                writer.AppendLine("            rawIndex++;");
                writer.AppendLine("        }");
                writer.AppendLine("        while(rawIndex < numberOfSensors);");
                writer.AppendLine("    }");
            }
            else if ((IsWidgetCSHL(CyWidgetAPITypes.MatrixButton)) || IsWidgetCSHL(CyWidgetAPITypes.Touchpad))
            {
                writer.AppendLine("        numberOfSensors = " + m_instanceName + "_numberOfSensors[widget + 1u];");
                writer.AppendLine("        rawIndex = " + m_instanceName + "_rawDataIndex[widget + 1u];");
                writer.AppendLine("        numberOfSensors += rawIndex;");
                writer.AppendLine("        ");
                writer.AppendLine("        /* Disable all sensors of the widget */");
                writer.AppendLine("        do");
                writer.AppendLine("        {");
                writer.AppendLine("            pos = (rawIndex >> 3u);");
                writer.AppendLine("            enMask = 0x01u << (rawIndex & 0x07u);");
                writer.AppendLine("            ");
                writer.AppendLine("            " + m_instanceName + "_SensorEnableMask[pos] |= enMask;");
                writer.AppendLine("            rawIndex++;");
                writer.AppendLine("        }");
                writer.AppendLine("        while(rawIndex < numberOfSensors);");
            }
            else
            {
                // Generate nothing
            }
            #endregion
        }
        #endregion

        #region apiCollectCSHLDisWidget
        void ApiCollectCSHLDisWidget(ref StringBuilder writer)
        {
            string str1 = "", str2 = "", str = "";
            // Add Touchpad part
            #region TouchPad
            if (IsWidgetCSHL(CyWidgetAPITypes.Touchpad))
            {
                if (IsWidgetCSHL(CyWidgetAPITypes.SliderLinearDiplexed) || IsWidgetCSHL(CyWidgetAPITypes.SliderLinear) ||
                    IsWidgetCSHL(CyWidgetAPITypes.SliderRadial))
                {
                    str1 = "(widget > " + m_instanceName + "_END_OF_SLIDERS_INDEX)";
                }

                if (IsWidgetCSHL(CyWidgetAPITypes.Button) || IsWidgetCSHL(CyWidgetAPITypes.Proximity))
                {
                    str2 = "(widget < " + m_instanceName + "_END_OF_TOUCH_PAD_INDEX)";
                }

                if (str1 != "" && str2 != "")
                {
                    str = "(" + str1 + " && " + str2 + ")";
                }
                else if (str1 != "")
                {
                    str = str1;
                }
                else
                {
                    str = str2;
                }

            }
            #endregion

            // Add Matrix Button part
            #region Matrix Button
            if (IsWidgetCSHL(CyWidgetAPITypes.MatrixButton))
            {
                if (IsWidgetCSHL(CyWidgetAPITypes.SliderLinearDiplexed) ||
                    IsWidgetCSHL(CyWidgetAPITypes.SliderLinear) ||
                    IsWidgetCSHL(CyWidgetAPITypes.SliderRadial) ||
                    IsWidgetCSHL(CyWidgetAPITypes.Button) ||
                    IsWidgetCSHL(CyWidgetAPITypes.Proximity))
                {
                    str1 = "(widget > " + m_instanceName + "_END_OF_BUTTONS_INDEX)";
                }
                else
                {
                    str1 = "";
                }

                // Generic Widget after Matrix Buttons
                if (IsWidgetCSHL(CyWidgetAPITypes.Generic))
                {
                    if (str1 != "")
                    {
                        str1 = "(" + str1 + " && (widget <= " + m_instanceName + "_TOTAL_WIDGET_COUNT))";
                    }
                    else
                    {
                        str1 = "(widget <= " + m_instanceName + "_TOTAL_WIDGET_COUNT)";
                    }
                }

                // Concatenate string
                if (str != "" && str1 != "")
                {
                    str += " || \\" + "\r\n         " + str1;
                    if (str.Contains("&&"))
                    {
                        str = "( " + str + " )";
                    }
                }
                else if (str1 != "")
                {
                    str = str1;
                }
                else
                {
                    // Do nothing empty str
                }
            }


            #endregion

            #region functionCode
            if (str != "")
            {
                writer.AppendLine("    if " + str + "");
                writer.AppendLine("    {");
                writer.AppendLine("        numberOfSensors = " + m_instanceName + "_numberOfSensors[widget + 1u];");
                writer.AppendLine("        rawIndex = " + m_instanceName + "_rawDataIndex[widget + 1u];");
                writer.AppendLine("        numberOfSensors += rawIndex;");
                writer.AppendLine("        ");
                writer.AppendLine("        /* Disable all sensors of the widget */");
                writer.AppendLine("        do");
                writer.AppendLine("        {");
                writer.AppendLine("            pos = (rawIndex >> 3u);");
                writer.AppendLine("            enMask = 0x01u << (rawIndex & 0x07u);");
                writer.AppendLine("            ");
                writer.AppendLine("            " + m_instanceName + "_SensorEnableMask[pos] &= ~enMask;");
                writer.AppendLine("            rawIndex++;");
                writer.AppendLine("        }");
                writer.AppendLine("        while(rawIndex < numberOfSensors);");
                writer.AppendLine("    }");
            }
            else if ((IsWidgetCSHL(CyWidgetAPITypes.MatrixButton)) || IsWidgetCSHL(CyWidgetAPITypes.Touchpad))
            {
                writer.AppendLine("        numberOfSensors = " + m_instanceName + "_numberOfSensors[widget + 1u];");
                writer.AppendLine("        rawIndex = " + m_instanceName + "_rawDataIndex[widget + 1u];");
                writer.AppendLine("        numberOfSensors += rawIndex;");
                writer.AppendLine("        ");
                writer.AppendLine("        /* Disable all sensors of the widget */");
                writer.AppendLine("        do");
                writer.AppendLine("        {");
                writer.AppendLine("            pos = (rawIndex >> 3u);");
                writer.AppendLine("            enMask = 0x01u << (rawIndex & 0x07u);");
                writer.AppendLine("            ");
                writer.AppendLine("            " + m_instanceName + "_SensorEnableMask[pos] &= ~enMask;");
                writer.AppendLine("            rawIndex++;");
                writer.AppendLine("        }");
                writer.AppendLine("        while(rawIndex < numberOfSensors);");
            }
            else
            {
                // Generate nothing
            }
            #endregion
        }
        #endregion

        #region apiCollectCSHLIsWidget
        void ApiCollectCSHLIsWidget(ref StringBuilder writer)
        {
            string str1 = "", str2 = "", str = "";

            /* Add Touchpad part */
            #region TouchPad
            if (IsWidgetCSHL(CyWidgetAPITypes.Touchpad))
            {
                if (IsWidgetCSHL(CyWidgetAPITypes.SliderLinearDiplexed) || IsWidgetCSHL(CyWidgetAPITypes.SliderLinear) ||
                    IsWidgetCSHL(CyWidgetAPITypes.SliderRadial))
                {
                    str1 = "(widget > " + m_instanceName + "_END_OF_SLIDERS_INDEX)";
                }

                if (IsWidgetCSHL(CyWidgetAPITypes.Button) || IsWidgetCSHL(CyWidgetAPITypes.Proximity))
                {
                    str2 = "(widget < " + m_instanceName + "_END_OF_TOUCH_PAD_INDEX)";
                }

                if (str1 != "" && str2 != "")
                {
                    str = "(" + str1 + " && " + str2 + ")";
                }
                else if (str1 != "")
                {
                    str = str1;
                }
                else
                {
                    str = str2;
                }

            }
            #endregion

            /* Add Matrix Button part */
            #region Matrix Button
            if (IsWidgetCSHL(CyWidgetAPITypes.MatrixButton))
            {
                if (IsWidgetCSHL(CyWidgetAPITypes.SliderLinearDiplexed) ||
                    IsWidgetCSHL(CyWidgetAPITypes.SliderLinear) ||
                    IsWidgetCSHL(CyWidgetAPITypes.SliderRadial) ||
                    IsWidgetCSHL(CyWidgetAPITypes.Button) ||
                    IsWidgetCSHL(CyWidgetAPITypes.Proximity))
                {
                    str1 = "(widget > " + m_instanceName + "_END_OF_BUTTONS_INDEX)";
                }
                else
                {
                    str1 = "";
                }

                if (str != "" && str1 != "")
                {
                    str += " || \\" + "\r\n             " + str1;
                    if (str.Contains("&&"))
                    {
                        str = "( " + str + " )";
                    }
                }
                else if (str1 != "")
                {
                    str = str1;
                }
                else
                {
                    // Do nothing empty str
                }
            }
            #endregion

            #region functionCode

            if (str != "")
            {
                writer.AppendLine("    if " + str + "");
                writer.AppendLine("    {");
                str1 = "    ";
            }
            else
            {
                str1 = "";
            }

            if ((str != "") || ((str == "") && IsTwoDoubleWidgets()))
            {
                writer.AppendLine(str1 + "    numberOfSensors = " + m_instanceName + "_numberOfSensors[widget + 1u];");
                writer.AppendLine(str1 + "    rawIndex = " + m_instanceName + "_rawDataIndex[widget + 1u];");
                writer.AppendLine(str1 + "    numberOfSensors += rawIndex;");
                writer.AppendLine(str1 + "    ");
                writer.AppendLine(str1 + "    /* Check all sensors of the widget */");
                writer.AppendLine(str1 + "    do ");
                writer.AppendLine(str1 + "    {");
                writer.AppendLine(str1 + "        if(" + m_instanceName + "_CheckIsSensorActive(rawIndex) != 0u)");
                writer.AppendLine(str1 + "        {");
                writer.AppendLine(str1 + "            state |= " + m_instanceName + "_SENSOR_2_IS_ACTIVE;");
                writer.AppendLine(str1 + "        }");
                writer.AppendLine(str1 + "        rawIndex++;");
                writer.AppendLine(str1 + "    }");
                writer.AppendLine(str1 + "    while(rawIndex < numberOfSensors);");
                writer.AppendLine(str1 + "    ");
                writer.AppendLine(str1 + "    /* Define active as Row and Col cross */");
                writer.AppendLine(str1 + "    if ( ((state & " + m_instanceName + "_SENSOR_1_IS_ACTIVE) != 0u) && \\");
                writer.AppendLine(str1 + "         ((state & " + m_instanceName + "_SENSOR_2_IS_ACTIVE) != 0u) ) ");
                writer.AppendLine(str1 + "    {");
                writer.AppendLine(str1 + "        state = " + m_instanceName + "_WIDGET_IS_ACTIVE;");
                writer.AppendLine(str1 + "    }");
                writer.AppendLine(str1 + "    else");
                writer.AppendLine(str1 + "    {");
                writer.AppendLine(str1 + "        state = 0u;");
                writer.AppendLine(str1 + "    }");
            }

            if (str != "")
            {
                writer.AppendLine("    }");
            }
            #endregion
        }
        #endregion

        #region apiCollectCSHLIsAnyWidget
        void ApiCollectCSHLIsAnyWidget(ref StringBuilder writer)
        {
            string str1 = "", str2 = "", str = "";

            /* Add Touchpad part */
            #region TouchPad
            if (IsWidgetCSHL(CyWidgetAPITypes.Touchpad))
            {
                if (IsWidgetCSHL(CyWidgetAPITypes.SliderLinearDiplexed) ||
                    IsWidgetCSHL(CyWidgetAPITypes.SliderLinear) ||
                    IsWidgetCSHL(CyWidgetAPITypes.SliderRadial))
                {
                    str1 = "(i > " + m_instanceName + "_END_OF_SLIDERS_INDEX)";
                }

                if (IsWidgetCSHL(CyWidgetAPITypes.Button) || IsWidgetCSHL(CyWidgetAPITypes.Proximity))
                {
                    str2 = "(i < " + m_instanceName + "_END_OF_TOUCH_PAD_INDEX)";
                }

                if (str1 != "" && str2 != "")
                {
                    str = "(" + str1 + " && " + str2 + ")";
                }
                else if (str1 != "")
                {
                    str = str1;
                }
                else
                {
                    str = str2;
                }

            }
            #endregion

            /* Add Matrix Button part */
            #region Matrix Button
            if (IsWidgetCSHL(CyWidgetAPITypes.MatrixButton))
            {
                if (IsWidgetCSHL(CyWidgetAPITypes.SliderLinearDiplexed) ||
                    IsWidgetCSHL(CyWidgetAPITypes.SliderLinear) ||
                    IsWidgetCSHL(CyWidgetAPITypes.SliderRadial) ||
                    IsWidgetCSHL(CyWidgetAPITypes.Button) ||
                    IsWidgetCSHL(CyWidgetAPITypes.Proximity))
                {
                    str1 = "(i > " + m_instanceName + "_END_OF_BUTTONS_INDEX)";
                }
                else
                {
                    str1 = "";
                }

                if (str != "" && str1 != "")
                {
                    str += " || \\" + "\r\n             " + str1;
                    if (str.Contains("&&"))
                    {
                        str = "( " + str + " )";
                    }
                }
                else if (str1 != "")
                {
                    str = str1;
                }
                else
                {
                    // Do nothing empty str
                }
            }
            #endregion

            #region functionCode
            if (str != "")
            {
                writer.AppendLine("    uint8 realIndex, state, i;");
                writer.AppendLine("    ");
                writer.AppendLine("    state = 0u;");
                writer.AppendLine("    realIndex = 0u;");
                writer.AppendLine("    ");
                writer.AppendLine("    for(i = 0u; i < " + m_instanceName + "_TOTAL_WIDGET_COUNT; i++)");
                writer.AppendLine("    {");
                writer.AppendLine("        if (" + m_instanceName + "_CheckIsWidgetActive(realIndex) != 0u)");
                writer.AppendLine("        {");
                writer.AppendLine("            state = " + m_instanceName + "_WIDGET_IS_ACTIVE;");
                writer.AppendLine("        }");
                writer.AppendLine("        ");
                writer.AppendLine("        if " + str + "");
                writer.AppendLine("        {");
                writer.AppendLine("            realIndex++;");
                writer.AppendLine("        }");
                writer.AppendLine("        ");
                writer.AppendLine("        realIndex++;");
                writer.AppendLine("    }");
                writer.AppendLine("    ");
            }
            else if (IsTwoDoubleWidgets())
            {
                writer.AppendLine("    uint8 realIndex, state, i;");
                writer.AppendLine("    ");
                writer.AppendLine("    state = 0u;");
                writer.AppendLine("    realIndex = 0u;");
                writer.AppendLine("    ");
                writer.AppendLine("    for(i = 0u; i < " + m_instanceName + "_TOTAL_WIDGET_COUNT; i++)");
                writer.AppendLine("    {");
                writer.AppendLine("        if (" + m_instanceName + "_CheckIsWidgetActive(realIndex) != 0u)");
                writer.AppendLine("        {");
                writer.AppendLine("            state = " + m_instanceName + "_WIDGET_IS_ACTIVE;");
                writer.AppendLine("        }");
                writer.AppendLine("            realIndex += 2u;");
                writer.AppendLine("    }");
                writer.AppendLine("    ");
            }
            else
            {
                writer.AppendLine("    uint8 state, i;");
                writer.AppendLine("    ");
                writer.AppendLine("    state = 0u;");
                writer.AppendLine("    ");
                writer.AppendLine("    for(i = 0u; i < " + m_instanceName + "_TOTAL_WIDGET_COUNT; i++)");
                writer.AppendLine("    {");
                writer.AppendLine("        if (" + m_instanceName + "_CheckIsWidgetActive(i) != 0u)");
                writer.AppendLine("        {");
                writer.AppendLine("            state = " + m_instanceName + "_WIDGET_IS_ACTIVE;");
                writer.AppendLine("        }");
                writer.AppendLine("    }");
                writer.AppendLine("    ");
            }
            #endregion
        }
        #endregion

        #region  apiCollectCSHLFunctionCentroid
        public void ApiCollectCSHLFunctionCentroid(ref StringBuilder writer)
        {
            string curPos, tab, tab2;

            /* Calculates centroid position */
            if (m_isAnyCentroid)
            {
                #region _FindMaximum

                if (m_isDiplexSlider)
                {
                    tab = "    ";
                    curPos = "curPos";
                }
                else
                {
                    tab = "";
                    curPos = "i";
                }

                writer.AppendLine("/*******************************************************************************");
                writer.AppendLine("* Function Name: " + m_instanceName + "_FindMaximum");
                writer.AppendLine("********************************************************************************");
                writer.AppendLine("*");
                writer.AppendLine("* Summary:");
                writer.AppendLine("*  Finds local maximum of defined centroid.");
                writer.AppendLine("* ");
                writer.AppendLine("* Parameters:");
                writer.AppendLine("*  offset:  Start index of cetroid in " + m_instanceName + "_SensorSignal[] array");
                writer.AppendLine("*  count:  Length of centroid");
                writer.AppendLine("*  fingerThreshold:  Finger threshould of centroid");
                if (m_isDiplexSlider)
                {
                    writer.AppendLine("*  diplex:  pointer to diplex table");
                }
                writer.AppendLine("* ");
                writer.AppendLine("* Return:");
                writer.AppendLine("*  Returns maximum within defined range");
                writer.AppendLine("* ");
                writer.AppendLine("* Note:");
                writer.AppendLine("*  If noise counts on the slider segments are greater than the noise");
                writer.AppendLine("*  threshold, this subroutine may generate a false local maximum of centroid.");
                writer.AppendLine("*  The noise threshold should be set carefully (high enough above the noise");
                writer.AppendLine("*  level) so that noise will not generate a false local maximum.");
                writer.AppendLine("* ");
                writer.AppendLine("*******************************************************************************/");
                writer.Append("uint8 " + m_instanceName + "_FindMaximum(uint16 offset, uint8 count, uint8 fingerThreshold");
                if (m_isDiplexSlider)
                {
                    writer.Append(", const uint8 CYCODE *diplex");
                }
                writer.AppendLine(")");

                writer.AppendLine("{");
                writer.AppendLine("    uint8 maximum, i;");
                if (m_isDiplexSlider)
                {
                    writer.AppendLine("    uint8 curPos, curCtrdStartPos, curCntrdSize, biggestCtrdSize, biggestCtrdStartPos;");
                }
                writer.AppendLine("    uint" + m_packParams.m_settings.m_widgetResolution.ToString() + " *startOfSlider;");
                writer.AppendLine("    uint" + m_packParams.m_settings.m_widgetResolution.ToString() + " temp;");
                writer.AppendLine("    ");
                writer.AppendLine("    maximum = 0xFFu; temp = 0u;");
                writer.AppendLine("    startOfSlider = &" + m_instanceName + "_SensorSignal[offset];");
                writer.AppendLine("    ");

                if (m_isDiplexSlider)
                {
                    #region __Find Diplxed Centriod
                    writer.AppendLine("    if(diplex != 0u)");
                    writer.AppendLine("    {");
                    writer.AppendLine("        /* Initialize the diplex cycle */    ");
                    writer.AppendLine("        i = 0; curPos   = 0u;    /* index and current position */");
                    writer.AppendLine("        curCntrdSize    = 0u; curCtrdStartPos = 0xFFu;    /* No centroid at the Start */");
                    writer.AppendLine("        biggestCtrdSize = 0u; biggestCtrdStartPos = 0u;    /* The biggset centroid is zero */");
                    writer.AppendLine("        ");
                    writer.AppendLine("        /* Make slider x2 as Diplexed */");
                    writer.AppendLine("        count <<= 1u;");
                    writer.AppendLine("        while(1u)");
                    writer.AppendLine("        { ");
                    writer.AppendLine("            if (startOfSlider[curPos] > 0u)    /* Looking for centroids */");
                    writer.AppendLine("            {        ");
                    writer.AppendLine("                if (curCtrdStartPos == 0xFFu)");
                    writer.AppendLine("                {");
                    writer.AppendLine("                    /* Start of centroid */");
                    writer.AppendLine("                    curCtrdStartPos = i;");
                    writer.AppendLine("                    curCntrdSize++;");
                    writer.AppendLine("                }");
                    writer.AppendLine("                else");
                    writer.AppendLine("                {");
                    writer.AppendLine("                    curCntrdSize++;");
                    writer.AppendLine("                }");
                    writer.AppendLine("            }");
                    writer.AppendLine("            else   /* Select the bigest and indicate zero start */");
                    writer.AppendLine("            {          ");
                    writer.AppendLine("                if(curCntrdSize > 0)");
                    writer.AppendLine("                {");
                    writer.AppendLine("                    /* We are in the end of current */");
                    writer.AppendLine("                    if(curCntrdSize > biggestCtrdSize)");
                    writer.AppendLine("                    {");
                    writer.AppendLine("                        biggestCtrdSize = curCntrdSize;");
                    writer.AppendLine("                        biggestCtrdStartPos = curCtrdStartPos;");
                    writer.AppendLine("                    }");
                    writer.AppendLine("                    ");
                    writer.AppendLine("                    curCntrdSize = 0u;");
                    writer.AppendLine("                    curCtrdStartPos = 0xFFu;");
                    writer.AppendLine("                }");
                    writer.AppendLine("            }");
                    writer.AppendLine("            ");
                    writer.AppendLine("            i++; ");
                    writer.AppendLine("            curPos = diplex[i];");
                    writer.AppendLine("            if(i == count)");
                    writer.AppendLine("            {");
                    writer.AppendLine("                break;");
                    writer.AppendLine("            }            ");
                    writer.AppendLine("        }");
                    writer.AppendLine("        ");
                    writer.AppendLine("        /* Find the biggest centroid if two are the same size, last one wins");
                    writer.AppendLine("           We are in the end of current */");
                    writer.AppendLine("        if (curCntrdSize >= biggestCtrdSize) ");
                    writer.AppendLine("        {");
                    writer.AppendLine("            biggestCtrdSize = curCntrdSize;");
                    writer.AppendLine("            biggestCtrdStartPos = curCtrdStartPos;");
                    writer.AppendLine("        }");
                    writer.AppendLine("    }");
                    writer.AppendLine("    else");
                    writer.AppendLine("    {");
                    writer.AppendLine("        /* without diplexing */ ");
                    writer.AppendLine("        biggestCtrdSize = count;");
                    writer.AppendLine("        biggestCtrdStartPos = 0u;");
                    writer.AppendLine("    }");
                    writer.AppendLine("    ");
                    #endregion

                    writer.AppendLine("    /* Check centroid size */");
                    if (m_isUnDiplexSlider)
                    {
                        writer.AppendLine("    if((biggestCtrdSize >= 2u) || ((biggestCtrdSize == 1u) && (diplex == 0u)))");
                    }
                    else
                    {
                        writer.AppendLine("    if(biggestCtrdSize >= 2u)");
                    }
                    writer.AppendLine("    {");
                    writer.AppendLine("        for (i = biggestCtrdStartPos; i < (biggestCtrdStartPos + biggestCtrdSize); i++)");
                }
                else
                {
                    writer.AppendLine(tab + "    for (i = 0u; i < count; i++)");
                }

                #region __Find Maximum
                writer.AppendLine(tab + "    {");

                if (m_isDiplexSlider && m_isUnDiplexSlider)
                {
                    writer.AppendLine(tab + "        if (diplex == 0u)");
                    writer.AppendLine(tab + "        {");
                    writer.AppendLine(tab + "            curPos = i;");
                    writer.AppendLine(tab + "        }");
                    writer.AppendLine(tab + "        else");
                    writer.AppendLine(tab + "        {");
                    writer.AppendLine(tab + "            curPos = diplex[i];");
                    writer.AppendLine(tab + "        }");
                    writer.AppendLine(tab + "         ");
                }
                else if (m_isDiplexSlider)
                {
                    writer.AppendLine(tab + "            curPos = diplex[i];");
                }
                else
                {
                    /* Do nothing */
                }
                writer.AppendLine(tab + "        /* Looking for the grater element within centroid */");
                writer.AppendLine(tab + "        if(startOfSlider[" + curPos + "] > fingerThreshold)");
                writer.AppendLine(tab + "        {");
                writer.AppendLine(tab + "            if(startOfSlider[" + curPos + "] > temp)");
                writer.AppendLine(tab + "            {");
                writer.AppendLine(tab + "                maximum = i;");
                writer.AppendLine(tab + "                temp = startOfSlider[" + curPos + "];");
                writer.AppendLine(tab + "            }");
                writer.AppendLine(tab + "        }");
                writer.AppendLine(tab + "    }");
                if (m_isDiplexSlider && m_isUnDiplexSlider)
                {
                    writer.AppendLine("    }");
                }
                else if (m_isDiplexSlider)
                {
                    writer.AppendLine("    }");
                }
                writer.AppendLine("");
                #endregion

                writer.AppendLine("    return maximum;");
                writer.AppendLine("}");
                writer.AppendLine("");
                writer.AppendLine("");
                #endregion

                #region _CalcCentroid
                if (m_isLinearAlone)
                {
                    tab = "";
                }
                else
                {
                    tab = "    ";
                }

                if (m_isDiplexSlider && m_isUnDiplexSlider)
                {
                    tab2 = "    ";
                }
                else
                {
                    tab2 = "";
                }

                writer.AppendLine("/*******************************************************************************");
                writer.AppendLine("* Function Name: " + m_instanceName + "_CalcCentroid");
                writer.AppendLine("********************************************************************************");
                writer.AppendLine("*");
                writer.AppendLine("* Summary:");
                writer.AppendLine("*  Finds local maximum of defined centroid.");
                writer.AppendLine("*");
                writer.AppendLine("* Parameters:");
                if (m_isLinearAlone == false)
                {
                    writer.AppendLine("*  type:  Define the widget type");
                }
                if (m_isDiplexSlider)
                {
                    writer.AppendLine("*  diplex:  pointer to diplex table");
                }
                writer.AppendLine("*  maximum:  Defined maximum position within array");
                writer.AppendLine("*  offset:  Start index of cetroid in " + m_instanceName + "_SensorSignal[] array");
                writer.AppendLine("*  count:  Length of centroid");
                writer.AppendLine("*  resolution:  Finger threshould of centroid");
                writer.AppendLine("*  noiseThreshold:  Noise threshould of centroid");
                writer.AppendLine("* ");
                writer.AppendLine("* Return:");
                writer.AppendLine("*  Returns position within centroid");
                writer.AppendLine("* ");
                writer.AppendLine("* Side Effects:");
                writer.AppendLine("*  If no sensors are active, the function returns -1. If an error");
                writer.AppendLine("*  occurs during execution of the centroid algorithm, the function");
                writer.AppendLine("*  returns -1.");
                writer.AppendLine("* ");
                writer.AppendLine("* Note:");
                writer.AppendLine("*  If noise counts on the slider segments are greater than the noise");
                writer.AppendLine("*  threshold, this subroutine may generate a false local maximum of centroid.");
                writer.AppendLine("*  The noise threshold should be set carefully (high enough above the noise");
                writer.AppendLine("*  level) so that noise will not generate a false local maximum.");
                writer.AppendLine("* ");
                writer.AppendLine("*******************************************************************************/");
                writer.Append("uint16 " + m_instanceName + "_CalcCentroid(");
                if (m_isLinearAlone == false)
                {
                    writer.Append("uint8 type, ");
                }

                if (m_isDiplexSlider)
                {
                    writer.Append("const uint8 CYCODE *diplex, ");
                }
                writer.AppendLine("uint8 maximum, uint8 offset, uint8 count, uint16 resolution, uint8 noiseThreshold)");

                writer.AppendLine("{");
                if (IsWidgetCSHL(CyWidgetAPITypes.SliderLinear) || IsWidgetCSHL(CyWidgetAPITypes.Touchpad))
                {
                    writer.AppendLine("    uint8 posPrev, posNext;");
                }

                //Add define of position
                #region posVar
                if (m_isDiplexSlider && m_isUnDiplexSlider)
                {
                    writer.AppendLine("    uint8 pos;");
                }
                else if (m_isDiplexSlider)
                {
                    writer.AppendLine("    uint8 pos;");
                }
                else
                {
                    // Do nothing
                }
                #endregion

                writer.AppendLine("    uint" + m_packParams.m_settings.m_widgetResolution.ToString() + " *startOfSlider;");
                writer.AppendLine("    uint16 position;");
                writer.AppendLine("    int32 numerator, denominator;");
                writer.AppendLine("    ");
                writer.AppendLine("    startOfSlider = &" + m_instanceName + "_SensorSignal[offset];");
                writer.AppendLine("    ");

                if (m_isLinearAlone == false)
                {
                    writer.AppendLine("    if(type == " + m_instanceName + "_TYPE_RADIAL_SLIDER)");
                    writer.AppendLine("    {");
                }

                #region __Radial Slider
                if (IsWidgetCSHL(CyWidgetAPITypes.SliderRadial))
                {
                    writer.AppendLine(tab + "    /* Copy Signal for found centriod */");
                    writer.AppendLine(tab + "    " + m_instanceName + "_centroid[" + m_instanceName + "_POS] = startOfSlider[maximum];");
                    writer.AppendLine("     ");
                    writer.AppendLine(tab + "    /* Check borders for ROTARY Slider */");
                    writer.AppendLine(tab + "    if (maximum == 0u)                   /* Start of centroid */");
                    writer.AppendLine(tab + "    { ");
                    writer.AppendLine(tab + "        " + m_instanceName + "_centroid[" + m_instanceName + "_POS_PREV] = startOfSlider[count - 1u];");
                    writer.AppendLine(tab + "        " + m_instanceName + "_centroid[" + m_instanceName + "_POS_NEXT] = startOfSlider[maximum + 1u];");
                    writer.AppendLine(tab + "    }");
                    writer.AppendLine(tab + "    else if (maximum == (count - 1u))    /* End of centroid */");
                    writer.AppendLine(tab + "    {");
                    writer.AppendLine(tab + "        " + m_instanceName + "_centroid[" + m_instanceName + "_POS_PREV] = startOfSlider[maximum - 1u];");
                    writer.AppendLine(tab + "        " + m_instanceName + "_centroid[" + m_instanceName + "_POS_NEXT] = startOfSlider[0u];");
                    writer.AppendLine(tab + "    }");
                    writer.AppendLine(tab + "    else                                /* Not first Not last */");
                    writer.AppendLine(tab + "    {");
                    writer.AppendLine(tab + "        " + m_instanceName + "_centroid[" + m_instanceName + "_POS_PREV] = startOfSlider[maximum - 1u];");
                    writer.AppendLine(tab + "        " + m_instanceName + "_centroid[" + m_instanceName + "_POS_NEXT] = startOfSlider[maximum + 1u];");
                    writer.AppendLine(tab + "    }");
                }
                #endregion

                if (m_isLinearAlone == false)
                {
                    writer.AppendLine("    }");
                    writer.AppendLine("    else");
                    writer.AppendLine("    {");
                }

                #region __Linear PosCalc
                if (IsWidgetCSHL(CyWidgetAPITypes.SliderLinear) || IsWidgetCSHL(CyWidgetAPITypes.Touchpad))
                {
                    if (m_isDiplexSlider && m_isUnDiplexSlider)
                    {
                        writer.AppendLine(tab + "    /* Calculate next and previous near to maximum */");
                        writer.AppendLine(tab + "    if(diplex == 0u)");
                        writer.AppendLine(tab + "    {");
                        writer.AppendLine(tab + tab2 + "    pos     = maximum;");
                        writer.AppendLine(tab + tab2 + "    posPrev = maximum - 1u;");
                        writer.AppendLine(tab + tab2 + "    posNext = maximum + 1u; ");
                        writer.AppendLine(tab + "    }");
                        writer.AppendLine(tab + "    else");
                        writer.AppendLine(tab + "    {");
                        writer.AppendLine(tab + tab2 + "    pos     = diplex[maximum];");
                        writer.AppendLine(tab + tab2 + "    posPrev = diplex[maximum - 1u];");
                        writer.AppendLine(tab + tab2 + "    posNext = diplex[maximum + 1u];");
                        writer.AppendLine(tab + tab2 + "    count <<= 1u;");

                        writer.AppendLine(tab + "    }");
                    }
                    else if (m_isDiplexSlider)
                    {
                        writer.AppendLine(tab + tab2 + "    /* Calculate next and previous near to maximum */");
                        writer.AppendLine(tab + tab2 + "    pos     = diplex[maximum];");
                        writer.AppendLine(tab + tab2 + "    posPrev = diplex[maximum - 1u];");
                        writer.AppendLine(tab + tab2 + "    posNext = diplex[maximum + 1u];");
                        writer.AppendLine(tab + tab2 + "    count <<= 1u;");
                    }
                    else
                    {
                        writer.AppendLine(tab + tab2 + "    /* Calculate next and previous near to maximum */");
                        writer.AppendLine(tab + tab2 + "    posPrev = maximum - 1u;");
                        writer.AppendLine(tab + tab2 + "    posNext = maximum + 1u; ");
                    }
                }
                #endregion

                #region __Linear Slider
                if (IsWidgetCSHL(CyWidgetAPITypes.SliderLinear) || IsWidgetCSHL(CyWidgetAPITypes.Touchpad))
                {
                    writer.AppendLine("    ");
                    writer.AppendLine(tab + "    /* Copy Signal for found centriod */");
                    if (m_isDiplexSlider && m_isUnDiplexSlider)
                    {
                        writer.AppendLine(tab + "    " + m_instanceName + "_centroid[" + m_instanceName + "_POS] = startOfSlider[pos];");
                    }
                    else if (m_isDiplexSlider)
                    {
                        writer.AppendLine(tab + "    " + m_instanceName + "_centroid[" + m_instanceName + "_POS] = startOfSlider[pos];");
                    }
                    else
                    {
                        writer.AppendLine(tab + "    " + m_instanceName + "_centroid[" + m_instanceName + "_POS] = startOfSlider[maximum];");
                    }
                    writer.AppendLine(tab + "    ");
                    writer.AppendLine(tab + "    /* Check borders for LINEAR Slider */");
                    writer.AppendLine(tab + "    if (maximum == 0u)                   /* Start of centroid */");
                    writer.AppendLine(tab + "    { ");
                    writer.AppendLine(tab + "        " + m_instanceName + "_centroid[" + m_instanceName + "_POS_PREV] = 0u;");
                    writer.AppendLine(tab + "        " + m_instanceName + "_centroid[" + m_instanceName + "_POS_NEXT] = startOfSlider[posNext];");
                    writer.AppendLine(tab + "    }");
                    writer.AppendLine(tab + "    else if (maximum == ((count) - 1u)) /* End of centroid */");
                    writer.AppendLine(tab + "    {");
                    writer.AppendLine(tab + "        " + m_instanceName + "_centroid[" + m_instanceName + "_POS_PREV] = startOfSlider[posPrev];");
                    writer.AppendLine(tab + "        " + m_instanceName + "_centroid[" + m_instanceName + "_POS_NEXT] = 0u;");
                    writer.AppendLine(tab + "    }");
                    writer.AppendLine(tab + "    else                                /* Not first Not last */");
                    writer.AppendLine(tab + "    {");
                    writer.AppendLine(tab + "        " + m_instanceName + "_centroid[" + m_instanceName + "_POS_PREV] = startOfSlider[posPrev];");
                    writer.AppendLine(tab + "        " + m_instanceName + "_centroid[" + m_instanceName + "_POS_NEXT] = startOfSlider[posNext];");
                    writer.AppendLine(tab + "    }");
                }
                #endregion

                if (m_isLinearAlone == false)
                {
                    writer.AppendLine("    }");
                }

                #region __Cenroid Calculation
                writer.AppendLine("    ");
                writer.AppendLine("    /* Subtract noiseThreshold */");
                writer.AppendLine("    if(" + m_instanceName + "_centroid[" + m_instanceName + "_POS_PREV] > noiseThreshold)");
                writer.AppendLine("    {");
                writer.AppendLine("        " + m_instanceName + "_centroid[" + m_instanceName + "_POS_PREV] -= noiseThreshold;");
                writer.AppendLine("    }");
                writer.AppendLine("    else");
                writer.AppendLine("    {");
                writer.AppendLine("        " + m_instanceName + "_centroid[" + m_instanceName + "_POS_PREV] = 0u;");
                writer.AppendLine("    }");
                writer.AppendLine("    ");
                writer.AppendLine("    /* Maximum always grater than fingerThreshold, so grate than noiseThreshold */");
                writer.AppendLine("    " + m_instanceName + "_centroid[" + m_instanceName + "_POS] -= noiseThreshold;");
                writer.AppendLine("    ");
                writer.AppendLine("    /* Subtract noiseThreshold */");
                writer.AppendLine("    if(" + m_instanceName + "_centroid[" + m_instanceName + "_POS_NEXT] > noiseThreshold)");
                writer.AppendLine("    {");
                writer.AppendLine("        " + m_instanceName + "_centroid[" + m_instanceName + "_POS_NEXT] -= noiseThreshold;");
                writer.AppendLine("    }");
                writer.AppendLine("    else");
                writer.AppendLine("    {");
                writer.AppendLine("        " + m_instanceName + "_centroid[" + m_instanceName + "_POS_NEXT] = 0u;");
                writer.AppendLine("    }");
                writer.AppendLine("    ");
                writer.AppendLine("    ");
                writer.AppendLine("    /* Si+1 - Si-1 */");
                writer.AppendLine("    numerator = (int32) " + m_instanceName + "_centroid[" + m_instanceName + "_POS_NEXT] - \\");
                writer.AppendLine("                (int32) " + m_instanceName + "_centroid[" + m_instanceName + "_POS_PREV];");
                writer.AppendLine("    ");
                writer.AppendLine("    /* Si+1 + Si + Si-1 */");
                writer.AppendLine("    denominator = (int32) " + m_instanceName + "_centroid[" + m_instanceName + "_POS_PREV] + \\");
                writer.AppendLine("                  (int32) " + m_instanceName + "_centroid[" + m_instanceName + "_POS] + \\");
                writer.AppendLine("                  (int32) " + m_instanceName + "_centroid[" + m_instanceName + "_POS_NEXT];");
                writer.AppendLine("    ");
                writer.AppendLine("    /* (numerator/denominator) + maximum */");
                writer.AppendLine("    denominator = (numerator << 8u)/denominator + ((uint16) maximum << 8u);");
                writer.AppendLine("    ");
                writer.AppendLine("    /* Only required for RADIAL Slider */");
                writer.AppendLine("    if(denominator < 0)");
                writer.AppendLine("    {");
                writer.AppendLine("        denominator += ((uint16) count << 8u);");
                writer.AppendLine("    }");
                writer.AppendLine("    ");
                writer.AppendLine("    denominator *= resolution;");
                writer.AppendLine("    ");
                writer.AppendLine("    /* */");
                writer.AppendLine("    position = HI16(denominator + 0x7F00u);");
                writer.AppendLine("    ");
                #endregion

                writer.AppendLine("    return position;");
                writer.AppendLine("}");
                writer.AppendLine("");
                writer.AppendLine("");
                #endregion
            }

            if (IsWidgetCSHL(CyWidgetAPITypes.SliderLinear))
            {
                #region _GetCentroidPos
                writer.AppendLine("/*******************************************************************************");
                writer.AppendLine("* Function Name: " + m_instanceName + "_GetCentroidPos");
                writer.AppendLine("********************************************************************************");
                writer.AppendLine("*");
                writer.AppendLine("* Summary:");
                writer.AppendLine("*  Checks the " + m_instanceName + "_Signal[ ] array for a centroid within");
                writer.AppendLine("*  slider specified range. The centroid position is calculated to the resolution");
                writer.AppendLine("*  specified in the CapSense customizer. The position filters are applied to the");
                writer.AppendLine("*  result if enabled.");
                writer.AppendLine("*");
                writer.AppendLine("* Parameters:");
                writer.AppendLine("*  widget:  Widget number.");
                writer.AppendLine("*  For every linear slider widget there are defines in this format:");
                writer.AppendLine("*  #define " + m_instanceName + "_LS__\"widget_name\"            5");
                writer.AppendLine("* ");
                writer.AppendLine("* Return:");
                writer.AppendLine("*  Returns position value of the linear slider.");
                writer.AppendLine("*");
                writer.AppendLine("* Side Effects:");
                writer.AppendLine("*  If any slider slot is active, the function returns values from zero to");
                writer.AppendLine("*  the resolution value set in the CapSense customizer. If no sensors");
                writer.AppendLine("*  are active, the function returns -1. If an error occurs during");
                writer.AppendLine("*  execution of the centroid/diplexing algorithm, the function returns -1.");
                writer.AppendLine("*  You can use the _ChecklsSensorActive() routine to determine which");
                writer.AppendLine("*  slider segments are touched, if required.");
                writer.AppendLine("*");
                writer.AppendLine("* Note:");
                writer.AppendLine("*  If noise counts on the slider segments are greater than the noise");
                writer.AppendLine("*  threshold, this subroutine may generate a false centroid result. The noise");
                writer.AppendLine("*  threshold should be set carefully (high enough above the noise level) so");
                writer.AppendLine("*  that noise will not generate a false centroid.");
                writer.AppendLine("*");
                writer.AppendLine("*******************************************************************************/");
                writer.AppendLine("uint16 " + m_instanceName + "_GetCentroidPos(uint8 widget)");
                writer.AppendLine("{");

                #region Diplex Ptr
                if (m_isDiplexSlider)
                {
                    writer.AppendLine("    const uint8 CYCODE *diplex;");
                }
                #endregion

                #region PosFilter Var
                if (IsFilterForWidget(CyWidgetAPITypes.SliderLinear, CyFilterType.AnyPos) ||
                     IsFilterForWidget(CyWidgetAPITypes.SliderLinearDiplexed, CyFilterType.AnyPos))
                {
                    writer.AppendLine("    uint8 firstTimeIndex;");
                    writer.AppendLine("    uint8 posIndex;");
                    writer.AppendLine("    uint8 posFiltersMask;");

                }
                if (IsFilterForWidget(CyWidgetAPITypes.SliderLinear, CyFilterType.MedianFilterPos) ||
                    IsFilterForWidget(CyWidgetAPITypes.SliderLinearDiplexed, CyFilterType.MedianFilterPos) ||
                    IsFilterForWidget(CyWidgetAPITypes.SliderLinear, CyFilterType.AveragingFilterPos) ||
                    IsFilterForWidget(CyWidgetAPITypes.SliderLinearDiplexed, CyFilterType.AveragingFilterPos) ||
                    IsFilterForWidget(CyWidgetAPITypes.SliderLinear, CyFilterType.JitterFilterPos) ||
                    IsFilterForWidget(CyWidgetAPITypes.SliderLinearDiplexed, CyFilterType.JitterFilterPos))
                {
                    writer.AppendLine("    uint16 tempPos;");
                }
                #endregion

                writer.AppendLine("    uint8 offset, count;");
                writer.AppendLine("    uint8 fingerThreshold, noiseThreshold;");
                writer.AppendLine("    uint32 resolution;");
                writer.AppendLine("    uint8 maximum;");
                writer.AppendLine("    uint16 position;");
                writer.AppendLine("    ");
                
                #region PosFilter Var Init
                if (IsFilterForWidget(CyWidgetAPITypes.SliderLinear, CyFilterType.AnyPos) ||
                     IsFilterForWidget(CyWidgetAPITypes.SliderLinearDiplexed, CyFilterType.AnyPos))
                {
                    writer.AppendLine("    posFiltersMask = " + m_instanceName + "_posFiltersMask[widget];");
                    writer.AppendLine("    firstTimeIndex = " + m_instanceName + "_posFiltersData[widget];");
                }
                #endregion

                writer.AppendLine("    offset = " + m_instanceName + "_rawDataIndex[widget];");
                writer.AppendLine("    count = " + m_instanceName + "_numberOfSensors[widget];");
                writer.AppendLine("    fingerThreshold = " + m_instanceName + "_fingerThreshold[widget];");
                writer.AppendLine("    noiseThreshold = " + m_instanceName + "_noiseThreshold[widget];");
                writer.AppendLine("    resolution = " + m_instanceName + "_centroidMult[widget];");
                writer.AppendLine("    ");

                #region __DiplexPtr calculation
                if (m_isDiplexSlider)
                {
                    writer.AppendLine("    if(widget < " + m_instanceName + "_TOTAL_DIPLEXED_SLIDERS_COUNT)");
                    writer.AppendLine("    {");
                    writer.AppendLine("        maximum = " + m_instanceName + "_diplexTable[widget];");
                    writer.AppendLine("        diplex = &" + m_instanceName + "_diplexTable[maximum];");
                    writer.AppendLine("    }");
                    writer.AppendLine("    else");
                    writer.AppendLine("    {");
                    writer.AppendLine("        diplex = 0u;");
                    writer.AppendLine("    }");
                }
                #endregion

                // Find Maximum within centroid
                #region __Call FindMaximum maximum
                writer.AppendLine("    /* Find Maximum within centroid */");
                writer.Append("    maximum = " + m_instanceName + "_FindMaximum(offset, count, fingerThreshold");
                if (m_isDiplexSlider)
                {
                    writer.Append(", diplex");
                }
                writer.AppendLine(");");
                writer.AppendLine("");
                #endregion

                writer.AppendLine("    if (maximum != 0xFFu)");
                writer.AppendLine("    {");

                // Calculate centroid
                #region __Call CalcCentriod position
                writer.AppendLine("        /* Calculate centroid */");
                writer.Append("        position = " + m_instanceName + "_CalcCentroid(");
                if (IsWidgetCSHL(CyWidgetAPITypes.SliderRadial))
                {
                    writer.Append(m_instanceName + "_TYPE_LINEAR_SLIDER, ");
                }
                if (m_isDiplexSlider)
                {
                    writer.Append("diplex, ");
                }
                writer.AppendLine("maximum, offset, count, resolution, noiseThreshold);");
                writer.AppendLine("");
                #endregion

                // Filtering process
                #region __Position_Filters
                if (IsFilterForWidget(CyWidgetAPITypes.SliderLinear, CyFilterType.AnyPos) ||
                     IsFilterForWidget(CyWidgetAPITypes.SliderLinearDiplexed, CyFilterType.AnyPos))
                {
                    writer.AppendLine("        /* Check if this linear slider has enabled filters */");
                    writer.AppendLine("        if ((posFiltersMask & " + m_instanceName + "_ANY_POS_FILTER) != 0u)");
                    writer.AppendLine("        {");
                    writer.AppendLine("            /* Caluclate position to store filters data */");
                    writer.AppendLine("            posIndex  = firstTimeIndex + 1u;");
                    writer.AppendLine("            ");
                    writer.AppendLine("            if (0u == " + m_instanceName + "_posFiltersData[firstTimeIndex])");
                    writer.AppendLine("            {");
                    writer.AppendLine("                /* Init filters */");
                    writer.AppendLine("                " + m_instanceName + "_posFiltersData[posIndex] = position;");
                    if (IsFilterForWidget(CyWidgetAPITypes.SliderLinear, CyFilterType.MedianFilterPos) ||
                         IsFilterForWidget(CyWidgetAPITypes.SliderLinear, CyFilterType.AveragingFilterPos) ||
                         IsFilterForWidget(CyWidgetAPITypes.SliderLinearDiplexed, CyFilterType.MedianFilterPos) ||
                         IsFilterForWidget(CyWidgetAPITypes.SliderLinearDiplexed, CyFilterType.AveragingFilterPos))
                    {
                        writer.AppendLine("                if ( ((posFiltersMask & " + m_instanceName + "_MEDIAN_FILTER) != 0u) || ");
                        writer.AppendLine("                     ((posFiltersMask & " + m_instanceName + "_AVERAGING_FILTER) != 0u) )");
                        writer.AppendLine("                {");
                        writer.AppendLine("                    " + m_instanceName + "_posFiltersData[posIndex + 1u] = position;");
                        writer.AppendLine("                }");
                    }
                    writer.AppendLine("                ");
                    writer.AppendLine("                " + m_instanceName + "_posFiltersData[firstTimeIndex] = 1u;");
                    writer.AppendLine("            }");
                    writer.AppendLine("            else");
                    writer.AppendLine("            {");
                    writer.AppendLine("                /* Do filtering */");
                    if (IsFilterForWidget(CyWidgetAPITypes.SliderLinear, CyFilterType.MedianFilterPos) ||
                         IsFilterForWidget(CyWidgetAPITypes.SliderLinearDiplexed, CyFilterType.MedianFilterPos))
                    {
                        writer.AppendLine("                if ((posFiltersMask & " + m_instanceName + "_MEDIAN_FILTER) != 0u)");
                        writer.AppendLine("                {");
                        writer.AppendLine("                    tempPos = position;");
                        writer.AppendLine("                    position = " + m_instanceName + "_MedianFilter(position, " + m_instanceName + "_posFiltersData[posIndex], " + m_instanceName + "_posFiltersData[posIndex + 1u]);");
                        writer.AppendLine("                    " + m_instanceName + "_posFiltersData[posIndex + 1u] = " + m_instanceName + "_posFiltersData[posIndex];");
                        writer.AppendLine("                    " + m_instanceName + "_posFiltersData[posIndex] = tempPos;");
                        writer.AppendLine("                }");
                        writer.AppendLine("                ");
                    }

                    if (IsFilterForWidget(CyWidgetAPITypes.SliderLinear, CyFilterType.AveragingFilterPos) ||
                         IsFilterForWidget(CyWidgetAPITypes.SliderLinearDiplexed, CyFilterType.AveragingFilterPos))
                    {
                        writer.AppendLine("                if ((posFiltersMask & " + m_instanceName + "_AVERAGING_FILTER) != 0u) ");
                        writer.AppendLine("                {");
                        writer.AppendLine("                    tempPos = position;");
                        writer.AppendLine("                    position = " + m_instanceName + "_AveragingFilter(position, " + m_instanceName + "_posFiltersData[posIndex], " + m_instanceName + "_posFiltersData[posIndex + 1u]);");
                        writer.AppendLine("                    " + m_instanceName + "_posFiltersData[posIndex + 1u] = " + m_instanceName + "_posFiltersData[posIndex];");
                        writer.AppendLine("                    " + m_instanceName + "_posFiltersData[posIndex] = tempPos;");
                        writer.AppendLine("                }");
                        writer.AppendLine("                ");
                    }

                    if (IsFilterForWidget(CyWidgetAPITypes.SliderLinear, CyFilterType.FirstOrderIIRFilter0_5Pos) ||
                         IsFilterForWidget(CyWidgetAPITypes.SliderLinearDiplexed, CyFilterType.FirstOrderIIRFilter0_5Pos))
                    {
                        writer.AppendLine("                if ((posFiltersMask & " + m_instanceName + "_IIR2_FILTER) != 0u) ");
                        writer.AppendLine("                {");
                        writer.AppendLine("                    position = " + m_instanceName + "_IIR2Filter(position, " + m_instanceName + "_posFiltersData[posIndex]);");
                        writer.AppendLine("                    " + m_instanceName + "_posFiltersData[posIndex] = position;");
                        writer.AppendLine("                }");
                        writer.AppendLine("                ");
                    }

                    if (IsFilterForWidget(CyWidgetAPITypes.SliderLinear, CyFilterType.FirstOrderIIRFilter0_75Pos) ||
                         IsFilterForWidget(CyWidgetAPITypes.SliderLinearDiplexed, CyFilterType.FirstOrderIIRFilter0_75Pos))
                    {
                        writer.AppendLine("                if ((posFiltersMask & " + m_instanceName + "_IIR4_FILTER) != 0u)");
                        writer.AppendLine("                {");
                        writer.AppendLine("                    position = " + m_instanceName + "_IIR4Filter(position, " + m_instanceName + "_posFiltersData[posIndex]);");
                        writer.AppendLine("                    " + m_instanceName + "_posFiltersData[posIndex] = position;");
                        writer.AppendLine("                }");
                        writer.AppendLine("                ");
                    }

                    if (IsFilterForWidget(CyWidgetAPITypes.SliderLinear, CyFilterType.JitterFilterPos) ||
                         IsFilterForWidget(CyWidgetAPITypes.SliderLinearDiplexed, CyFilterType.JitterFilterPos))
                    {
                        writer.AppendLine("                if ((posFiltersMask & " + m_instanceName + "_JITTER_FILTER) != 0u)");
                        writer.AppendLine("                {");
                        writer.AppendLine("                    tempPos = position;");
                        writer.AppendLine("                    position = " + m_instanceName + "_JitterFilter(position, " + m_instanceName + "_posFiltersData[posIndex]);");
                        writer.AppendLine("                    " + m_instanceName + "_posFiltersData[posIndex] = tempPos;");
                        writer.AppendLine("                }");
                    }
                    writer.AppendLine("            }");
                    writer.AppendLine("        }");
                }
                #endregion

                writer.AppendLine("    }");
                writer.AppendLine("    else");
                writer.AppendLine("    {");
                writer.AppendLine("        /* The maximum didn't find */");
                writer.AppendLine("        position = 0xFFFFu;");

                // Need to reset filter
                #region __Position_Filters
                if (IsFilterForWidget(CyWidgetAPITypes.SliderLinear, CyFilterType.AnyPos) ||
                     IsFilterForWidget(CyWidgetAPITypes.SliderLinearDiplexed, CyFilterType.AnyPos))
                {
                    writer.AppendLine("        ");
                    writer.AppendLine("        /* Reset the filters */");
                    writer.AppendLine("        if((posFiltersMask & " + m_instanceName + "_ANY_POS_FILTER) != 0u)");
                    writer.AppendLine("        {");
                    writer.AppendLine("            " + m_instanceName + "_posFiltersData[firstTimeIndex] = 0u;");
                    writer.AppendLine("        }");
                }
                writer.AppendLine("    }");
                #endregion

                #region TuningCopyResults
                if (m_packParams.m_settings.m_tuningMethod != CyTuningMethodOptions.Tuning_None)
                {
                    writer.AppendLine("    " + m_instanceName + "_position[widget] = position;");
                }
                #endregion

                writer.AppendLine("    ");
                writer.AppendLine("    return position;");
                writer.AppendLine("}");
                writer.AppendLine("");
                writer.AppendLine("");
                #endregion
            }

            if (IsWidgetCSHL(CyWidgetAPITypes.SliderRadial))
            {
                #region _GetRadialCentroidPos
                writer.AppendLine("/*******************************************************************************");
                writer.AppendLine("* Function Name: " + m_instanceName + "_GetRadialCentroidPos");
                writer.AppendLine("********************************************************************************");
                writer.AppendLine("*");
                writer.AppendLine("* Summary:");
                writer.AppendLine("*  Checks the " + m_instanceName + "_Signal[ ] array for a centroid within");
                writer.AppendLine("*  slider specified range. The centroid position is calculated to the resolution");
                writer.AppendLine("*  specified in the CapSense customizer. The position filters are applied to the");
                writer.AppendLine("*  result if enabled.");
                writer.AppendLine("*");
                writer.AppendLine("* Parameters:");
                writer.AppendLine("*  widget:  Widget number. ");
                writer.AppendLine("*  For every linear slider widget there are defines in this format:");
                writer.AppendLine("*  #define " + m_instanceName + "_RS_\"widget_name\"            5");
                writer.AppendLine("* ");
                writer.AppendLine("* Return:");
                writer.AppendLine("*  Returns position value of the radial slider.");
                writer.AppendLine("*");
                writer.AppendLine("* Side Effects:");
                writer.AppendLine("*  If any slider slot is active, the function returns values from zero to ");
                writer.AppendLine("*  the resolution value set in the CapSense customizer. If no sensors ");
                writer.AppendLine("*  are active, the function returns -1. If an error occurs during ");
                writer.AppendLine("*  execution of the centroid/diplexing algorithm, the function returns -1.");
                writer.AppendLine("*  You can use the _ChecklsSlotActive() routine to determine which ");
                writer.AppendLine("*  slider segments are touched, if required.");
                writer.AppendLine("*");
                writer.AppendLine("* Note:");
                writer.AppendLine("*  If noise counts on the slider segments are greater than the noise");
                writer.AppendLine("*  threshold, this subroutine may generate a false centroid result. The noise");
                writer.AppendLine("*  threshold should be set carefully (high enough above the noise level) so ");
                writer.AppendLine("*  that noise will not generate a false centroid.");
                writer.AppendLine("*");
                writer.AppendLine("*******************************************************************************/");
                writer.AppendLine(" uint16 " + m_instanceName + "_GetRadialCentroidPos(uint8 widget)");
                writer.AppendLine("{");

                #region PosFilter Var
                if (IsFilterForWidget(CyWidgetAPITypes.SliderRadial, CyFilterType.AnyPos))
                {
                    writer.AppendLine("    uint8 firstTimeIndex;");
                    writer.AppendLine("    uint8 posIndex;");
                    writer.AppendLine("    uint8 posFiltersMask;");

                }
                if (IsFilterForWidget(CyWidgetAPITypes.SliderRadial, CyFilterType.MedianFilterPos) ||
                     IsFilterForWidget(CyWidgetAPITypes.SliderRadial, CyFilterType.AveragingFilterPos) ||
                     IsFilterForWidget(CyWidgetAPITypes.SliderRadial, CyFilterType.JitterFilterPos))
                {
                    writer.AppendLine("    uint16 tempPos;");
                }
                #endregion

                writer.AppendLine("    uint8 offset, count;");
                writer.AppendLine("    uint8 fingerThreshold, noiseThreshold;");
                writer.AppendLine("    uint32 resolution;");
                writer.AppendLine("    uint8 maximum;");
                writer.AppendLine("    uint16 position;");
                writer.AppendLine("    ");
                
                #region PosFilter Var Init
                if (IsFilterForWidget(CyWidgetAPITypes.SliderRadial, CyFilterType.AnyPos))
                {
                    writer.AppendLine("    posFiltersMask = " + m_instanceName + "_posFiltersMask[widget];");
                    writer.AppendLine("    firstTimeIndex = " + m_instanceName + "_posFiltersData[widget];");
                }
                #endregion

                writer.AppendLine("    offset = " + m_instanceName + "_rawDataIndex[widget];");
                writer.AppendLine("    count = " + m_instanceName + "_numberOfSensors[widget];");
                writer.AppendLine("    fingerThreshold = " + m_instanceName + "_fingerThreshold[widget];");
                writer.AppendLine("    noiseThreshold = " + m_instanceName + "_noiseThreshold[widget];");
                writer.AppendLine("    resolution = " + m_instanceName + "_centroidMult[widget];");
                writer.AppendLine("    ");

                // Find Maximum within centroid
                #region __Call FindMaximum maximum
                writer.AppendLine("    /* Find Maximum within centroid */");
                writer.Append("    maximum = " + m_instanceName + "_FindMaximum(offset, count, fingerThreshold");
                if (m_isDiplexSlider)
                {
                    writer.Append(", 0u");
                }
                writer.AppendLine(");");
                writer.AppendLine("");
                #endregion

                writer.AppendLine("    if (maximum != 0xFFu)");
                writer.AppendLine("    {");

                // Calculate centroid
                #region __Call CalcCentroid position
                writer.AppendLine("        /* Calculate centroid */");
                writer.Append("        position = " + m_instanceName + "_CalcCentroid(");
                if (IsWidgetCSHL(CyWidgetAPITypes.SliderLinear) ||
                     IsWidgetCSHL(CyWidgetAPITypes.Touchpad))
                {
                    writer.Append(m_instanceName + "_TYPE_RADIAL_SLIDER, ");
                }
                if (m_isDiplexSlider)
                {
                    writer.Append("0, ");
                }
                writer.AppendLine("maximum, offset, count, resolution, noiseThreshold);");
                writer.AppendLine("");
                #endregion

                // Filtering process
                #region __Position_Filters
                if (IsFilterForWidget(CyWidgetAPITypes.SliderRadial, CyFilterType.AnyPos))
                {
                    writer.AppendLine("        /* Check if this Radial slider has enabled filters */");
                    writer.AppendLine("        if ((posFiltersMask & " + m_instanceName + "_ANY_POS_FILTER) != 0u)");
                    writer.AppendLine("        {");
                    writer.AppendLine("            /* Caluclate position to store filters data */");
                    writer.AppendLine("            posIndex  = firstTimeIndex + 1u;");
                    writer.AppendLine("            ");
                    writer.AppendLine("            if (0u == " + m_instanceName + "_posFiltersData[firstTimeIndex])");
                    writer.AppendLine("            {");
                    writer.AppendLine("                /* Init filters */");
                    writer.AppendLine("                " + m_instanceName + "_posFiltersData[posIndex] = position;");
                    if (IsFilterForWidget(CyWidgetAPITypes.SliderRadial, CyFilterType.MedianFilterPos) ||
                         IsFilterForWidget(CyWidgetAPITypes.SliderRadial, CyFilterType.AveragingFilterPos))
                    {
                        writer.AppendLine("                if ( ((posFiltersMask & " + m_instanceName + "_MEDIAN_FILTER) != 0u)  || ");
                        writer.AppendLine("                     ((posFiltersMask & " + m_instanceName + "_AVERAGING_FILTER) != 0u) )");
                        writer.AppendLine("                {");
                        writer.AppendLine("                    " + m_instanceName + "_posFiltersData[posIndex + 1u] = position;");
                        writer.AppendLine("                }");
                    }
                    writer.AppendLine("                ");
                    writer.AppendLine("                " + m_instanceName + "_posFiltersData[firstTimeIndex] = 1u;");
                    writer.AppendLine("            }");
                    writer.AppendLine("            else");
                    writer.AppendLine("            {");
                    writer.AppendLine("                /* Do filtering */");
                    if (IsFilterForWidget(CyWidgetAPITypes.SliderRadial, CyFilterType.MedianFilterPos))
                    {
                        writer.AppendLine("                if ((posFiltersMask & " + m_instanceName + "_MEDIAN_FILTER) != 0u)");
                        writer.AppendLine("                {");
                        writer.AppendLine("                    tempPos = position;");
                        writer.AppendLine("                    position = " + m_instanceName + "_MedianFilter(position, " + m_instanceName + "_posFiltersData[posIndex], " + m_instanceName + "_posFiltersData[posIndex + 1u]);");
                        writer.AppendLine("                    " + m_instanceName + "_posFiltersData[posIndex + 1u] = " + m_instanceName + "_posFiltersData[posIndex];");
                        writer.AppendLine("                    " + m_instanceName + "_posFiltersData[posIndex] = tempPos;");
                        writer.AppendLine("                }");
                        writer.AppendLine("                ");
                    }

                    if (IsFilterForWidget(CyWidgetAPITypes.SliderRadial, CyFilterType.AveragingFilterPos))
                    {
                        writer.AppendLine("                if ((posFiltersMask & " + m_instanceName + "_AVERAGING_FILTER) != 0u)");
                        writer.AppendLine("                {");
                        writer.AppendLine("                    tempPos = position;");
                        writer.AppendLine("                    position = " + m_instanceName + "_AveragingFilter(position, " + m_instanceName + "_posFiltersData[posIndex], " + m_instanceName + "_posFiltersData[posIndex + 1u]);");
                        writer.AppendLine("                    " + m_instanceName + "_posFiltersData[posIndex + 1u] = " + m_instanceName + "_posFiltersData[posIndex];");
                        writer.AppendLine("                    " + m_instanceName + "_posFiltersData[posIndex] = tempPos;");
                        writer.AppendLine("                }");
                        writer.AppendLine("                ");
                    }

                    if (IsFilterForWidget(CyWidgetAPITypes.SliderRadial, CyFilterType.FirstOrderIIRFilter0_5Pos))
                    {
                        writer.AppendLine("                if ((posFiltersMask & " + m_instanceName + "_IIR2_FILTER) != 0u)");
                        writer.AppendLine("                {");
                        writer.AppendLine("                    position = " + m_instanceName + "_IIR2Filter(position, " + m_instanceName + "_posFiltersData[posIndex]);");
                        writer.AppendLine("                    " + m_instanceName + "_posFiltersData[posIndex] = position;");
                        writer.AppendLine("                }");
                        writer.AppendLine("                ");
                    }

                    if (IsFilterForWidget(CyWidgetAPITypes.SliderRadial, CyFilterType.FirstOrderIIRFilter0_75Pos))
                    {
                        writer.AppendLine("                if ((posFiltersMask & " + m_instanceName + "_IIR4_FILTER) != 0u)");
                        writer.AppendLine("                {");
                        writer.AppendLine("                    position = " + m_instanceName + "_IIR4Filter(position, " + m_instanceName + "_posFiltersData[posIndex]);");
                        writer.AppendLine("                    " + m_instanceName + "_posFiltersData[posIndex] = position;");
                        writer.AppendLine("                }");
                        writer.AppendLine("                ");
                    }

                    if (IsFilterForWidget(CyWidgetAPITypes.SliderRadial, CyFilterType.JitterFilterPos))
                    {
                        writer.AppendLine("                if ((posFiltersMask & " + m_instanceName + "_JITTER_FILTER) != 0u)");
                        writer.AppendLine("                {");
                        writer.AppendLine("                    tempPos = position;");
                        writer.AppendLine("                    position = " + m_instanceName + "_JitterFilter(position, " + m_instanceName + "_posFiltersData[posIndex]);");
                        writer.AppendLine("                    " + m_instanceName + "_posFiltersData[posIndex] = tempPos;");
                        writer.AppendLine("                }");
                    }
                    writer.AppendLine("            }");
                    writer.AppendLine("        }");
                }
                #endregion

                writer.AppendLine("    }");
                writer.AppendLine("    else");
                writer.AppendLine("    {");
                writer.AppendLine("        /* The maximum didn't find */");
                writer.AppendLine("        position = 0xFFFFu;");

                // Need to reset the filter 
                #region __Position_Filters
                if (IsFilterForWidget(CyWidgetAPITypes.SliderRadial, CyFilterType.AnyPos))
                {
                    writer.AppendLine("        ");
                    writer.AppendLine("        /* Reset the filters */");
                    writer.AppendLine("        if((posFiltersMask & " + m_instanceName + "_ANY_POS_FILTER) != 0u)");
                    writer.AppendLine("        {");
                    writer.AppendLine("            " + m_instanceName + "_posFiltersData[firstTimeIndex] = 0u;");
                    writer.AppendLine("        }");
                }
                #endregion

                writer.AppendLine("    }");

                #region TunningCopyResults
                if (m_packParams.m_settings.m_tuningMethod != CyTuningMethodOptions.Tuning_None)
                {
                    writer.AppendLine("    " + m_instanceName + "_position[widget] = position;");
                }
                #endregion

                writer.AppendLine("    ");
                writer.AppendLine("    return position;");
                writer.AppendLine("}");
                writer.AppendLine("");
                writer.AppendLine("");
                #endregion
            }

            if (IsWidgetCSHL(CyWidgetAPITypes.Touchpad))
            {
                #region _GetTouchCentroidPos
                writer.AppendLine("/*******************************************************************************");
                writer.AppendLine("* Function Name: " + m_instanceName + "_GetTouchCentroidPos");
                writer.AppendLine("********************************************************************************");
                writer.AppendLine("*");
                writer.AppendLine("* Summary:");
                writer.AppendLine("*  If a finger is present on touch pad, this function calculates the X and Y ");
                writer.AppendLine("*  position of the finger by calculating the centroids within touch pad specified");
                writer.AppendLine("*  range. The X and Y positions are calculated to the resolutions set in the");
                writer.AppendLine("*  CapSense customizer. The position filters are applied to the result if enabled.");
                writer.AppendLine("*");
                writer.AppendLine("* Parameters:");
                writer.AppendLine("*  widget:  Widget number. ");
                writer.AppendLine("*  For every touchpad widget there are defines in this format:");
                writer.AppendLine("*  #define " + m_instanceName + "_TP_\"widget_name\"            5");
                writer.AppendLine("*");
                writer.AppendLine("* Return:");
                writer.AppendLine("*  Returns a 1 if a finger is on the touchpad.");
                writer.AppendLine("*");
                writer.AppendLine("* Side Effects:");
                writer.AppendLine("*  The result of calculation of X and Y position store in global arrays. ");
                writer.AppendLine("*  The arrays name are:");
                writer.AppendLine("*  " + m_instanceName + "_TPCol_\"widget_name\"_Results position of X");
                writer.AppendLine("*  " + m_instanceName + "_TPRow_\"widget_name\"_Results position of Y");
                writer.AppendLine("*");
                writer.AppendLine("*******************************************************************************/");
                writer.AppendLine("uint16 " + m_instanceName + "_GetTouchCentroidPos(uint8 widget)");
                writer.AppendLine("{");

                #region PosFilter Var
                if (IsFilterForWidget(CyWidgetAPITypes.Touchpad, CyFilterType.AnyPos))
                {
                    writer.AppendLine("    uint8 firstTimeIndex;");
                    writer.AppendLine("    uint8 posXIndex, posYIndex;");
                    writer.AppendLine("    uint8 posFiltersMask;");

                }
                if (IsFilterForWidget(CyWidgetAPITypes.Touchpad, CyFilterType.MedianFilterPos) ||
                     IsFilterForWidget(CyWidgetAPITypes.Touchpad, CyFilterType.AveragingFilterPos) ||
                     IsFilterForWidget(CyWidgetAPITypes.Touchpad, CyFilterType.JitterFilterPos))
                {
                    writer.AppendLine("    uint16 tempPos;");
                }
                #endregion

                writer.AppendLine("    uint8 touch;");
                writer.AppendLine("    uint8 MaxX, MaxY;");
                writer.AppendLine("    uint16 posX, posY;");

                writer.AppendLine("    uint8 offset, count;");
                writer.AppendLine("    uint8 fingerThreshold, noiseThreshold;");
                writer.AppendLine("    uint32 resolution;");
                writer.AppendLine("    ");

                #region PosFilter Var Init
                if (IsFilterForWidget(CyWidgetAPITypes.Touchpad, CyFilterType.AnyPos))
                {
                    writer.AppendLine("    posFiltersMask = " + m_instanceName + "_posFiltersMask[widget];");
                    writer.AppendLine("    firstTimeIndex = " + m_instanceName + "_posFiltersData[widget];");
                }
                #endregion

                writer.AppendLine("    offset = " + m_instanceName + "_rawDataIndex[widget];");
                writer.AppendLine("    count = " + m_instanceName + "_numberOfSensors[widget];");
                writer.AppendLine("    fingerThreshold = " + m_instanceName + "_fingerThreshold[widget];");
                writer.AppendLine("    touch = 0u;");
                writer.AppendLine("    ");

                // Find Maximum within X centroid
                #region __Call_FindMaximum MaxX
                writer.AppendLine("    /* Find Maximum within X centroid */");
                writer.Append("    MaxX = " + m_instanceName + "_FindMaximum(offset, count, fingerThreshold");
                if (m_isDiplexSlider)
                {
                    writer.Append(", 0u");
                }
                writer.AppendLine(");");
                writer.AppendLine("");
                #endregion

                writer.AppendLine("    if (MaxX != 0xFFu)");
                writer.AppendLine("    {");
                writer.AppendLine("        offset = " + m_instanceName + "_rawDataIndex[widget + 1u];");
                writer.AppendLine("        count = " + m_instanceName + "_numberOfSensors[widget + 1u];");
                writer.AppendLine("        fingerThreshold = " + m_instanceName + "_fingerThreshold[widget + 1u];");
                writer.AppendLine("        ");

                // Find Maximum within Y centroid
                #region __Call_FindMaximum MaxY
                writer.AppendLine("        /* Find Maximum within Y centroid */");
                writer.Append("        MaxY = " + m_instanceName + "_FindMaximum(offset, count, fingerThreshold");
                if (m_isDiplexSlider)
                {
                    writer.Append(", 0u");
                }
                writer.AppendLine(");");
                writer.AppendLine("");
                #endregion

                writer.AppendLine("        if (MaxY != 0xFFu)");
                writer.AppendLine("        {");
                writer.AppendLine("            /* X and Y maximums are found = true touch */");
                writer.AppendLine("            touch = 1u;");
                writer.AppendLine("            ");
                writer.AppendLine("            /* Calculate Y centroid */");
                writer.AppendLine("            noiseThreshold = " + m_instanceName + "_noiseThreshold[widget + 1u];");
                writer.AppendLine("            resolution = " + m_instanceName + "_centroidMult[widget + 1u];");

                // Calculate Y centroid
                #region __Call_CalcCentroid posY
                writer.Append("            posY = " + m_instanceName + "_CalcCentroid(");
                if (IsWidgetCSHL(CyWidgetAPITypes.SliderRadial))
                {
                    writer.Append(m_instanceName + "_TYPE_LINEAR_SLIDER, ");
                }
                if (m_isDiplexSlider)
                {
                    writer.Append("0, ");
                }
                writer.AppendLine("MaxY, offset, count, resolution, noiseThreshold);");
                writer.AppendLine("            ");
                #endregion

                writer.AppendLine("            /* Calculate X centroid */");
                writer.AppendLine("            offset = " + m_instanceName + "_rawDataIndex[widget];");
                writer.AppendLine("            count = " + m_instanceName + "_numberOfSensors[widget];");
                writer.AppendLine("            ");
                writer.AppendLine("            noiseThreshold = " + m_instanceName + "_noiseThreshold[widget];");
                writer.AppendLine("            resolution = " + m_instanceName + "_centroidMult[widget];");

                // Calculate X centroid
                #region __Call_CalcCentroid posX
                writer.Append("            posX = " + m_instanceName + "_CalcCentroid(");
                if (IsWidgetCSHL(CyWidgetAPITypes.SliderRadial))
                {
                    writer.Append(m_instanceName + "_TYPE_LINEAR_SLIDER, ");
                }
                if (m_isDiplexSlider)
                {
                    writer.Append("0, ");
                }
                writer.AppendLine("MaxX, offset, count, resolution, noiseThreshold);");
                writer.AppendLine("");
                #endregion

                // Filtering process
                #region __Position_Filters
                if (IsFilterForWidget(CyWidgetAPITypes.Touchpad, CyFilterType.AnyPos))
                {
                    writer.AppendLine("            /* Check if this TP has enabled filters */");
                    writer.AppendLine("            if ((posFiltersMask & " + m_instanceName + "_ANY_POS_FILTER) != 0u)");
                    writer.AppendLine("            {");
                    writer.AppendLine("                /* Caluclate position to store filters data */");
                    writer.AppendLine("                posXIndex  = firstTimeIndex + 1u;");
                    writer.AppendLine("                posYIndex  = " + m_instanceName + "_posFiltersData[widget + 1u];");
                    writer.AppendLine("                ");
                    writer.AppendLine("                if (0u == " + m_instanceName + "_posFiltersData[firstTimeIndex])");
                    writer.AppendLine("                {");
                    writer.AppendLine("                    /* Init filters */");
                    writer.AppendLine("                    " + m_instanceName + "_posFiltersData[posXIndex] = posX;");
                    writer.AppendLine("                    " + m_instanceName + "_posFiltersData[posYIndex] = posY;");
                    if (IsFilterForWidget(CyWidgetAPITypes.Touchpad, CyFilterType.MedianFilterPos) ||
                         IsFilterForWidget(CyWidgetAPITypes.Touchpad, CyFilterType.AveragingFilterPos))
                    {
                        writer.AppendLine("                    if ( ((posFiltersMask & " + m_instanceName + "_MEDIAN_FILTER) != 0u) || ");
                        writer.AppendLine("                         ((posFiltersMask & " + m_instanceName + "_AVERAGING_FILTER) != 0u) )");
                        writer.AppendLine("                    {");
                        writer.AppendLine("                        " + m_instanceName + "_posFiltersData[posXIndex + 1u] = posX;");
                        writer.AppendLine("                        " + m_instanceName + "_posFiltersData[posYIndex + 1u] = posY;");
                        writer.AppendLine("                    }");
                    }
                    writer.AppendLine("                    ");
                    writer.AppendLine("                    " + m_instanceName + "_posFiltersData[firstTimeIndex] = 1u;");
                    writer.AppendLine("                }");
                    writer.AppendLine("                else");
                    writer.AppendLine("                {");
                    writer.AppendLine("                    /* Do filtering */");
                    if (IsFilterForWidget(CyWidgetAPITypes.Touchpad, CyFilterType.MedianFilterPos))
                    {
                        writer.AppendLine("                    if ((posFiltersMask & " + m_instanceName + "_MEDIAN_FILTER) != 0u)");
                        writer.AppendLine("                    {");
                        writer.AppendLine("                        tempPos = posX;");
                        writer.AppendLine("                        posX = " + m_instanceName + "_MedianFilter(posX, " + m_instanceName + "_posFiltersData[posXIndex], " + m_instanceName + "_posFiltersData[posXIndex + 1u]);");
                        writer.AppendLine("                        " + m_instanceName + "_posFiltersData[posXIndex + 1u] = " + m_instanceName + "_posFiltersData[posXIndex];");
                        writer.AppendLine("                        " + m_instanceName + "_posFiltersData[posXIndex] = tempPos;");
                        writer.AppendLine("                        ");
                        writer.AppendLine("                        tempPos = posY;");
                        writer.AppendLine("                        posY = " + m_instanceName + "_MedianFilter(posY, " + m_instanceName + "_posFiltersData[posYIndex], " + m_instanceName + "_posFiltersData[posYIndex + 1u]);");
                        writer.AppendLine("                        " + m_instanceName + "_posFiltersData[posYIndex + 1u] = " + m_instanceName + "_posFiltersData[posYIndex];");
                        writer.AppendLine("                        " + m_instanceName + "_posFiltersData[posYIndex] = tempPos;");
                        writer.AppendLine("                    }");
                        writer.AppendLine("                    ");
                    }

                    if (IsFilterForWidget(CyWidgetAPITypes.Touchpad, CyFilterType.AveragingFilterPos))
                    {
                        writer.AppendLine("                    if ((posFiltersMask & " + m_instanceName + "_AVERAGING_FILTER) != 0u)");
                        writer.AppendLine("                    {");
                        writer.AppendLine("                        tempPos = posX;");
                        writer.AppendLine("                        posX = " + m_instanceName + "_AveragingFilter(posX, " + m_instanceName + "_posFiltersData[posXIndex], " + m_instanceName + "_posFiltersData[posXIndex + 1u]);");
                        writer.AppendLine("                        " + m_instanceName + "_posFiltersData[posXIndex + 1u] = " + m_instanceName + "_posFiltersData[posXIndex];");
                        writer.AppendLine("                        " + m_instanceName + "_posFiltersData[posXIndex] = tempPos;");
                        writer.AppendLine("                        ");
                        writer.AppendLine("                        tempPos = posY;");
                        writer.AppendLine("                        posY = " + m_instanceName + "_AveragingFilter(posY, " + m_instanceName + "_posFiltersData[posYIndex], " + m_instanceName + "_posFiltersData[posYIndex + 1u]);");
                        writer.AppendLine("                        " + m_instanceName + "_posFiltersData[posYIndex + 1u] = " + m_instanceName + "_posFiltersData[posYIndex];");
                        writer.AppendLine("                        " + m_instanceName + "_posFiltersData[posYIndex] = tempPos;");
                        writer.AppendLine("                    }");
                        writer.AppendLine("                    ");
                    }

                    if (IsFilterForWidget(CyWidgetAPITypes.Touchpad, CyFilterType.FirstOrderIIRFilter0_5Pos))
                    {
                        writer.AppendLine("                    if ((posFiltersMask & " + m_instanceName + "_IIR2_FILTER) != 0u)");
                        writer.AppendLine("                    {");
                        writer.AppendLine("                        posX = " + m_instanceName + "_IIR2Filter(posX, " + m_instanceName + "_posFiltersData[posXIndex]);");
                        writer.AppendLine("                        " + m_instanceName + "_posFiltersData[posXIndex] = posX;");
                        writer.AppendLine("                        ");
                        writer.AppendLine("                        posY = " + m_instanceName + "_IIR2Filter(posY, " + m_instanceName + "_posFiltersData[posYIndex]);");
                        writer.AppendLine("                        " + m_instanceName + "_posFiltersData[posYIndex] = posY;");
                        writer.AppendLine("                    }");
                        writer.AppendLine("                    ");
                    }

                    if (IsFilterForWidget(CyWidgetAPITypes.Touchpad, CyFilterType.FirstOrderIIRFilter0_75Pos))
                    {
                        writer.AppendLine("                    if ((posFiltersMask & " + m_instanceName + "_IIR4_FILTER) != 0u)");
                        writer.AppendLine("                    {");
                        writer.AppendLine("                        posX = " + m_instanceName + "_IIR4Filter(posX, " + m_instanceName + "_posFiltersData[posXIndex]);");
                        writer.AppendLine("                        " + m_instanceName + "_posFiltersData[posXIndex] = posX;");
                        writer.AppendLine("                        ");
                        writer.AppendLine("                        posY = " + m_instanceName + "_IIR4Filter(posY, " + m_instanceName + "_posFiltersData[posYIndex]);");
                        writer.AppendLine("                        " + m_instanceName + "_posFiltersData[posYIndex] = posY;");
                        writer.AppendLine("                    }");
                        writer.AppendLine("                    ");
                    }

                    if (IsFilterForWidget(CyWidgetAPITypes.Touchpad, CyFilterType.JitterFilterPos))
                    {
                        writer.AppendLine("                    if ((posFiltersMask & " + m_instanceName + "_JITTER_FILTER) != 0u)");
                        writer.AppendLine("                    {");
                        writer.AppendLine("                        tempPos = posX;");
                        writer.AppendLine("                        posX = " + m_instanceName + "_JitterFilter(posX, " + m_instanceName + "_posFiltersData[posXIndex]);");
                        writer.AppendLine("                        " + m_instanceName + "_posFiltersData[posXIndex] = tempPos;");
                        writer.AppendLine("                        ");
                        writer.AppendLine("                        tempPos = posY;");
                        writer.AppendLine("                        posY = " + m_instanceName + "_JitterFilter(posY, " + m_instanceName + "_posFiltersData[posYIndex]);");
                        writer.AppendLine("                        " + m_instanceName + "_posFiltersData[posYIndex] = tempPos;");
                        writer.AppendLine("                    }");
                    }
                    writer.AppendLine("                }");
                    writer.AppendLine("            }");
                }
                #endregion

                writer.AppendLine("            ");
                writer.AppendLine("            /* Save positions */");
                writer.AppendLine("            " + m_instanceName + "_position[widget] = posX;");
                writer.AppendLine("            " + m_instanceName + "_position[widget + 1u] = posY;");
                writer.AppendLine("        }");
                writer.AppendLine("    }");

                // Need to reset the filter
                #region __Position_Filters
                if (IsFilterForWidget(CyWidgetAPITypes.Touchpad, CyFilterType.AnyPos))
                {
                    writer.AppendLine("    ");
                    writer.AppendLine("    if(touch == 0u)");
                    writer.AppendLine("    {");


                    writer.AppendLine("        /* Reset the filters */");
                    writer.AppendLine("        if ((posFiltersMask & " + m_instanceName + "_ANY_POS_FILTER) != 0u)");
                    writer.AppendLine("        {");
                    writer.AppendLine("            " + m_instanceName + "_posFiltersData[firstTimeIndex] = 0u;");
                    writer.AppendLine("        }");
                    writer.AppendLine("    }");

                }
                #endregion

                #region TunningCopyResults
                if (m_packParams.m_settings.m_tuningMethod != CyTuningMethodOptions.Tuning_None)
                {
                    writer.AppendLine("    if (touch == 0u)");
                    writer.AppendLine("    {");
                    writer.AppendLine("        " + m_instanceName + "_position[widget] = 0xFFFFu;");
                    writer.AppendLine("        " + m_instanceName + "_position[widget + 1u] = 0xFFFFu;");
                    writer.AppendLine("    }");
                }
                #endregion

                writer.AppendLine("    ");
                writer.AppendLine("    return touch;");
                writer.AppendLine("}");
                writer.AppendLine("");
                writer.AppendLine("");
                #endregion
            }
        }

        #endregion
    }
}
