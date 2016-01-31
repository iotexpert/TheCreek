/*******************************************************************************
* Copyright 2008-2010, Cypress Semiconductor Corporation.  All rights reserved.
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
using System.Diagnostics;

namespace CapSense_CSD_v2_0
{
    public partial class CyAPIGenerator
    {
        #region CollectApiCFile
        public void CollectApiCFile(ref Dictionary<string, string> paramDict)
        {
            StringBuilder writer = new StringBuilder();

            ApiCollectCVariablesCommon(ref writer);

            paramDict.Add("writerCVariables", writer.ToString());

            writer = new StringBuilder();
            apiCollectC__Function(ref writer);
            paramDict.Add("writerCCode", writer.ToString());

            #region Pins Collect
            writer = new StringBuilder();
            List<CyTerminal> terminals = new List<CyTerminal>(m_packParams.m_widgets.m_listTerminals);
            if (m_settings.m_guardSensorEnable)
                terminals.Add(m_packParams.m_widgets.m_guardSensorTerminal);
            int chCount = m_settings.Configuration == CyChannelConfig.TWO_CHANNELS ? 2 : 1;
            //Define Sensors
            for (int i = 0; i < terminals.Count; i++)
            {
                string sensor = terminals[i].ToString();
                int ch = m_settings.GetChannelNumber(terminals[i].Channel);
                string str = string.Format("#define {0}_PortCH{1}__{2}  {0}_PortCH{1}__{2}__PC", m_instanceName, ch, sensor);
                if (m_isEmptyCapSense)
                    str = string.Format("#define {0}_PortCH{1}__{2}  0", m_instanceName, ch, sensor);
                writer.AppendLine(str);
            }
            writer.AppendLine("/* For Cmods*/");
            for (int ch = 0; ch < chCount; ch++)
            {
                writer.AppendLine(string.Format("#define {0}_CmodCH{1}_Cmod_CH{1}       {0}_CmodCH{1}__Cmod_CH{1}__PC", m_instanceName, ch));
            }
            if (m_settings.m_currentSource == CyCurrentSourceOptions.Idac_None)
            {
                writer.AppendLine("/* For Rb*/");
                for (int ch = 0; ch < chCount; ch++)
                    for (int i = 0; i < m_settings.m_listChannels[ch].m_rb; i++)
                        writer.AppendLine(string.Format("#define {0}_Rb{2}CH{1}         {0}_Rb{2}CH{1}__Rb{2}_CH{1}__PC", m_instanceName, ch, i));
            }
            paramDict.Add("writerHpinsDefines", writer.ToString());

            writer = new StringBuilder();
            for (int i = 0; i < terminals.Count; i++)
            {
                string sensor = terminals[i].ToString();
                int ch = m_settings.GetChannelNumber(terminals[i].Channel);
                string str = string.Format("    CyPins_SetPinDriveMode({0}_PortCH{1}__{2}, mode);", m_instanceName, ch, sensor);
                writer.AppendLine(str);
            }
            paramDict.Add("writerHsensorsDM", writer.ToString());

            writer = new StringBuilder();
            for (int ch = 0; ch < chCount; ch++)
            {
                string str = string.Format("    CyPins_SetPinDriveMode({0}_CmodCH{1}_Cmod_CH{1}, mode);", m_instanceName, ch);
                writer.AppendLine(str);
            }
            paramDict.Add("writerHcmodsDM", writer.ToString());

            if (m_settings.m_currentSource == CyCurrentSourceOptions.Idac_None)
            {
                writer = new StringBuilder();
                for (int ch = 0; ch < chCount; ch++)
                    for (int i = 0; i < m_settings.m_listChannels[ch].m_rb; i++)
                    {
                        string str = string.Format("    CyPins_SetPinDriveMode({0}_Rb{2}CH{1}, mode);", m_instanceName, ch, i);
                        writer.AppendLine(str);
                    }
                paramDict.Add("writerHrbsDM", writer.ToString());
            }
            #endregion

            #region Auto Tuning Collect
            writer = new StringBuilder();
            writer.AppendLine("const uint8 CYCODE "  + m_instanceName + "_SensorSensitivity[] = {");
            writer.AppendFormat("    ");
            for (int i = 0; i < m_listSS.Count; i++)
            {
                writer.AppendFormat("{0}, ", m_listSS[i].m_sensitivity);
            }
            writer.AppendLine("");
            writer.AppendLine("};");
            writer.AppendLine("");
            paramDict.Add("writerCAutoSensitivity", writer.ToString());
            #endregion
        }
        #endregion

        #region ApiCollectC__Variables
        public void ApiCollectCVariablesCommon(ref StringBuilder writer)
        {

            #region __SensorEnableMask
            writer.AppendLine("uint8 " + m_instanceName + "_SensorEnableMask[(((" + m_instanceName + "_TOTAL_SENSOR_COUNT - 1u) / 8u) + 1u)] = {");
            uint bitFiel = 0;
            for (int i = 0; i < m_listSS.Count; i++)
            {
                int ind = i % 8;
                if (m_listSS[i].WidgetType != CySensorType.Proximity)
                    CyBitOperations.AddBit(ref bitFiel, ind);

                if (ind == 7 || i == m_listSS.Count - 1)
                {
                    writer.AppendFormat("0x{0}u, ", bitFiel.ToString("X"));
                    bitFiel = 0;
                }
            }
            writer.AppendLine("};");
            writer.AppendLine("");
            #endregion

            List<string> listPcTable = new List<string>();
            List<string> listMaskTable = new List<string>();
            List<string> listPortTable = new List<string>();
            List<string> listIndexTable = new List<string>();
            string value_type = "uint8 CYXDATA *";
            int offset = 0;


            #region PRT_PC__TABLE + PRT_MASK__TABLE + PRT_PORT__TABLE + indexTable Calculation
            for (int i = 0; i < m_listSS.Count + m_listDedicatedTerminals.Count; i++)
            {
                int channel = i < m_listSS.Count ? (int)m_listSS[i].Channel : (int)m_listDedicatedTerminals[i - m_listSS.Count].Channel;
                string val = i < m_listSS.Count ? m_listSS[i].GetHeaderTerminalName() : m_listDedicatedTerminals[i - m_listSS.Count].ToString();

                string pcLine = string.Format("    ({0}){1}_{2}{3}__{4}__PC, ", value_type, m_instanceName, SH_CSPORT, channel, val);
                string maskLine = "    " + m_instanceName + "_" + SH_CSPORT + channel + "__" + val + "__MASK,";

                if ((i < m_listSS.Count) && m_listSS[i].IsScanSlotComplex())
                {
                    //Last bit must be 1
                    maskLine = string.Format("    {0}, /*{1}*/", m_listSS[i].GetTerminals().Count, m_listSS[i].WidgetName);
                    pcLine = "    0, ";
                }

                string portLine = string.Format("    {0}_{1}{2}__{3}__PORT, ", m_instanceName, SH_CSPORT, channel, val);
                if (i < m_listSS.Count)
                {
                    //Add Count data
                    if (m_listSS[i].IsScanSlotComplex())
                    {
                        //Add offset to API
                        portLine = string.Format("    {0} | 0x80u, /* {1} */", offset, m_listSS[i].WidgetName);
                        string indexLine = "";
                        List<CyTerminal> listTerm = m_listSS[i].GetTerminals();
                        for (int j = 0; j < listTerm.Count; j++)
                        {
                            CyTerminal term = listTerm[j];
                            int index = 0;
                            if (m_listDedicatedTerminals.IndexOf(term) != -1)
                            {
                                index = m_listSS.Count + m_listDedicatedTerminals.IndexOf(term);
                            }
                            else
                                for (int k = 0; k < m_listSS.Count; k++)
                                    if (m_listSS[k].GetHeaderTerminal() == term)
                                    {
                                        //Found index
                                        index = k;
                                        break;
                                    }
                            indexLine += string.Format("{0}, ", index);
                        }
                        //Add data to list
                        listIndexTable.Add(string.Format("    {0} /*{1}*/", indexLine, m_listSS[i].WidgetName));

                        //Move offset
                        offset += m_listSS[i].GetTerminals().Count;
                    }
                }

                if (m_isEmptyCapSense)
                {
                    portLine = "0,";
                    pcLine = "0,";
                    maskLine = "0,";
                }

                //Add data to API
                listPortTable.Add(portLine);
                listPcTable.Add(pcLine);
                listMaskTable.Add(maskLine);
            }
            #endregion

            #region PRT_PC__TABLE + PRT_MASK__TABLE + PRT_PORT__TABLE + indexTable
            writer.AppendLine("uint8 CYXDATA * const CYCODE " + m_instanceName + "_pcTable[] = {");
            for (int i = 0; i < listPcTable.Count; i++)
            {
                writer.AppendLine(listPcTable[i]);
            }
            writer.AppendLine("};");
            writer.AppendLine("");

            writer.AppendLine("const uint8 CYCODE " + m_instanceName + "_portTable[] = {");
            for (int i = 0; i < listPortTable.Count; i++)
            {
                writer.AppendLine(listPortTable[i]);
            }
            writer.AppendLine("};");
            writer.AppendLine("");

            //Add to API IndexTable
            if (listIndexTable.Count > 0)
            {
                writer.AppendLine("const uint8 CYCODE " + m_instanceName + "_indexTable[] = {");
                for (int i = 0; i < listIndexTable.Count; i++)
                    writer.AppendLine(listIndexTable[i]);
                writer.AppendLine("};");
                writer.AppendLine("");
            }

            writer.AppendLine("const uint8 CYCODE " + m_instanceName + "_maskTable[] = {");
            for (int i = 0; i < listMaskTable.Count; i++)
            {
                writer.AppendLine(listMaskTable[i]);
            }
            writer.AppendLine("};");
            writer.AppendLine("");
            #endregion

            #region AmuxIndex__TABLE
            if (m_isComplexSS)
            {
                writer.AppendLine("const uint8 CYCODE " + m_instanceName + "_amuxIndex[] = {");
                writer.Append("    ");
                List<CyTerminal> listTerm = m_packParams.m_widgets.GetListTerminalsSortedForAmux(m_settings);
                for (int i = 0; i < m_listSS.Count + m_listDedicatedTerminals.Count; i++)
                {
                    int index = 0;
                    if (i < m_listSS.Count)
                        index = m_listSS[i].IsScanSlotComplex() ? 0 :
                             listTerm.IndexOf(m_listSS[i].GetHeaderTerminal());
                    else
                        index = listTerm.IndexOf(m_listDedicatedTerminals[i - m_listSS.Count]);

                    Debug.Assert(index != -1);
                    string channel_info = "";
                    if (index != 0)
                        if (m_settings.Configuration == CyChannelConfig.TWO_CHANNELS && listTerm[index].m_widget.m_channel == CyChannelNumber.Second)
                        {
                            channel_info = " | 0x80u";
                            index -= m_firstChannelSSCount - 1;
                            Debug.Assert(index >= 0);
                        }
                    writer.AppendFormat("{0}u{1}, ", index, channel_info);
                }
                writer.AppendLine("");
                writer.AppendLine("};");
                writer.AppendLine("");
            }
            #endregion

            #region PRT_CAP_SEL__TABLE
            writer.AppendLine("uint8 CYXDATA * const CYCODE " + m_instanceName + "_csTable[] = {");
            writer.AppendLine("    (uint8 CYXDATA *)CYREG_PRT0_CAPS_SEL, (uint8 CYXDATA *)CYREG_PRT1_CAPS_SEL, (uint8 CYXDATA *)CYREG_PRT2_CAPS_SEL,");
            writer.AppendLine("    (uint8 CYXDATA *)CYREG_PRT3_CAPS_SEL, (uint8 CYXDATA *)CYREG_PRT4_CAPS_SEL, (uint8 CYXDATA *)CYREG_PRT5_CAPS_SEL,");
            writer.AppendLine("    (uint8 CYXDATA *)CYREG_PRT6_CAPS_SEL, (uint8 CYXDATA *)CYREG_PRT15_CAPS_SEL,");
            writer.AppendLine("};");
            writer.AppendLine("");
            #endregion

            #region idacSettings__TABLE
            if (m_isIdacInSystem)
            {
                writer.AppendLine(m_constTuner + "uint8" + m_cycodeTuner + m_instanceName + "_idacSettings[] = {");
                writer.Append("    ");
                for (int i = 0; i < m_listSS.Count; i++)
                {
                    writer.AppendFormat("{0}u,", m_listSS[i].m_idacSettings);
                }
                writer.AppendLine("");
                writer.AppendLine("};");
                writer.AppendLine("");
            }
            #endregion

            #region widgetResolution
            writer.AppendLine(m_constTuner + "uint8" + m_cycodeTuner + m_instanceName + "_widgetResolution[] = {");
            for (int i = 0; i < m_listWidget.Count; i++)
            {
                CyTuningProperties props = m_packParams.m_widgets.GetWidgetsProperties(m_listWidget[i]);
                writer.AppendFormat("    {0}{1},", m_instanceName,
                    GetResolution(props.m_scanResolution));
                writer.AppendLine("");
            }
            writer.AppendLine("};");
            writer.AppendLine("");
            #endregion

            #region RB_PC__TABLE
            if (m_isRbAvailible)
            {
                writer.AppendLine("uint8 CYXDATA * const CYCODE " + m_instanceName + "_rbTable[] = {");
                foreach (CyChannelProperties item in m_settings.m_listChannels)
                {
                    int ch_number = (int)item.m_channel;

                    for (int i = 0; i < item.m_rb; i++)
                    {
                        string str = "    (uint8 CYXDATA *)" + m_instanceName + "_Rb" + i + "CH" + ch_number +
                            "__Rb" + i + item.GetSufix() + "__PC,";
                        writer.AppendLine(str);
                    }
                }
                writer.AppendLine("};");
                writer.AppendLine("");
            }
            #endregion

            #region _widgetNumber__TABLE
            writer.AppendLine("const uint8 CYCODE " + m_instanceName + "_widgetNumber[] = {");
            writer.Append("    ");
            for (int i = 0; i < m_listSS.Count; i++)
            {
                int index = m_listWidget.IndexOf(m_listSS[i].m_widget);
                Debug.Assert(index > -1);
                writer.AppendFormat("{0}u, ", index.ToString());
                //Add description
                AddDescription(ref writer, i);
            }
            writer.AppendLine("");
            writer.AppendLine("};");
            writer.AppendLine("");
            #endregion
        }
        #endregion

        #region apiCollectC__Function

        public void apiCollectC__Function(ref StringBuilder writer)
        {
            //==========================
            string PRS = SH_PRS + m_prsResolution;
            CyChannelProperties sbItem = new CyChannelProperties();

            #region __PreScan
            writer.AppendLine("/*******************************************************************************");
            writer.AppendLine("* Function Name: " + m_instanceName + "_PreScan");
            writer.AppendLine("********************************************************************************");
            writer.AppendLine("*");
            writer.AppendLine("* Summary:");
            writer.AppendLine("*  Sets scan sensor in sampling state and start the scanning.");
            writer.AppendLine("*");
            writer.AppendLine("*");
            writer.AppendLine("* Parameters:");
            writer.AppendLine("*  sensor:  Scan sensor number");
            writer.AppendLine("*");
            writer.AppendLine("* Return:");
            writer.AppendLine("*  void");
            writer.AppendLine("*");
            writer.AppendLine("*******************************************************************************/");
            writer.AppendLine("void " + m_instanceName + "_PreScan(uint8 sensor)");
            writer.AppendLine("{");

            if (m_isComplexSS)
            {
                writer.AppendLine("    uint8 i, snsType, snsNumber;");
                writer.AppendLine("    uint8 *index;");
            }

            if (m_settings.m_configuration == CyChannelConfig.ONE_CHANNEL)
            {
                #region __ONE_CHANNEL_DESIGN
                writer.AppendLine("    /* Set Slot Settings */");
                writer.AppendLine("    " + m_instanceName + "_SetSlotSettings(sensor);");
                writer.AppendLine("    ");

                string ch_number = sbItem.GetChannelNumber().ToString();

                if (m_settings.m_currentSource == CyCurrentSourceOptions.Idac_Source)
                {

                    writer.AppendLine("    /* Disable CapSense Buffer */");
                    writer.AppendLine("    " + m_instanceName + "_" + SH_CSBUF + ch_number + "_CAPS_CFG0 &= ~" + m_instanceName + "_CSBUF_ENABLE;");
                    writer.AppendLine("    ");

                    #region __EnableSensor
                    if (m_isComplexSS)
                    {
                        writer.AppendLine("    /* Look for complex scan sensor */");
                        writer.AppendLine("    snsType = " + m_instanceName + "_portTable[sensor];");
                        writer.AppendLine("    if((snsType & " + m_instanceName + "_COMPLEX_SS_FLAG) == 0u)");
                        writer.AppendLine("    {");
                        writer.AppendLine("        /* Enable sensor */");
                        writer.AppendLine("        " + m_instanceName + "_EnableSensor(sensor);");
                        writer.AppendLine("    }");
                        writer.AppendLine("    else");
                        writer.AppendLine("    {");
                        writer.AppendLine("        /* Enable complex scan sensor */");
                        writer.AppendLine("        snsType &= ~" + m_instanceName + "_COMPLEX_SS_FLAG;");
                        writer.AppendLine("        index = &" + m_instanceName + "_indexTable[snsType];");
                        writer.AppendLine("        snsNumber = " + m_instanceName + "_maskTable[sensor];");
                        writer.AppendLine("                        ");
                        writer.AppendLine("        for (i=0; i < snsNumber; i++)");
                        writer.AppendLine("        {");
                        writer.AppendLine("            /* Enable sensor */");
                        writer.AppendLine("            " + m_instanceName + "_EnableSensor(index[i]);");
                        writer.AppendLine("        }");
                        writer.AppendLine("    }");
                    }
                    else
                    {
                        writer.AppendLine("    /* Enable Sensor */");
                        writer.AppendLine("    " + m_instanceName + "_EnableSensor(sensor);");
                    }
                    #endregion
                }
                else
                {
                    if (m_settings.m_currentSource == CyCurrentSourceOptions.Idac_Sink)
                    {
                        writer.AppendLine("    /* Connect IDAC */");
                        writer.AppendLine("    " + m_instanceName + "_" + SH_AMUX + ch_number + "_Connect(" + m_instanceName + "_" + SH_AMUX + ch_number + "_IDAC_CHANNEL);");
                        writer.AppendLine("    ");
                    }
                    else if (m_settings.m_currentSource == CyCurrentSourceOptions.Idac_None)
                    {
                        writer.AppendLine("    /* Connect DSI output to Rb */");
                        writer.AppendLine("    *" + m_instanceName + "_rbTable[" + m_instanceName + "_RbCh" + ch_number + "_cur] |= " + m_instanceName + "_BYP_MASK;");
                        writer.AppendLine("    ");
                    }

                    #region __EnableSensor
                    if (m_isComplexSS)
                    {
                        writer.AppendLine("    /* Look for complex scan sensor */");
                        writer.AppendLine("    snsType = " + m_instanceName + "_portTable[sensor];");
                        writer.AppendLine("    if((snsType & " + m_instanceName + "_COMPLEX_SS_FLAG) == 0u)");
                        writer.AppendLine("    {");
                        writer.AppendLine("        /* Enable sensor */");
                        writer.AppendLine("        " + m_instanceName + "_EnableSensor(sensor);");
                        writer.AppendLine("    }");
                        writer.AppendLine("    else");
                        writer.AppendLine("    {");
                        writer.AppendLine("        /* Enable complex scan sensor */");
                        writer.AppendLine("        snsType &= ~" + m_instanceName + "_COMPLEX_SS_FLAG;");
                        writer.AppendLine("        index = &" + m_instanceName + "_indexTable[snsType];");
                        writer.AppendLine("        snsNumber = " + m_instanceName + "_maskTable[sensor];");
                        writer.AppendLine("                        ");
                        writer.AppendLine("        for (i=0; i < snsNumber; i++)");
                        writer.AppendLine("        {");
                        writer.AppendLine("            /* Enable sensor */");
                        writer.AppendLine("            " + m_instanceName + "_EnableSensor(index[i]);");
                        writer.AppendLine("        }");
                        writer.AppendLine("    }");
                    }
                    else
                    {
                        writer.AppendLine("    /* Enable Sensor */");
                        writer.AppendLine("    " + m_instanceName + "_EnableSensor(sensor);");
                    }
                    #endregion

                    writer.AppendLine("    ");
                    writer.AppendLine("    /* Disable CapSense Buffer */");
                    writer.AppendLine("    " + m_instanceName + "_" + SH_CSBUF + ch_number + "_CAPS_CFG0 &= ~" + m_instanceName + "_CSBUF_ENABLE;");

                }
                #endregion
            }
            else
            {
                writer.AppendLine("    ");
                writer.AppendLine("    /* Set Sensor Settings */");
                writer.AppendLine("    " + m_instanceName + "_SetSlotSettings(sensor);");
                writer.AppendLine("    ");
                writer.AppendLine("    if((" + m_instanceName + "_csv & " + m_instanceName + "_SW_STS_SCAN_DONE__CH0) == 0u)");
                writer.AppendLine("    {");

                #region __CH0

                string ch_number = "0";
                if (m_settings.m_currentSource == CyCurrentSourceOptions.Idac_Source)
                {
                    writer.AppendLine("            /* Disable CapSense Buffer */");
                    writer.AppendLine("            " + m_instanceName + "_" + SH_CSBUF + ch_number + "_CAPS_CFG0 &= ~" + m_instanceName + "_CSBUF_ENABLE;");
                    writer.AppendLine("            ");

                    #region __EnableSensor
                    if (m_isComplexSS)
                    {
                        writer.AppendLine("            /* Look for complex scan sensor */");
                        writer.AppendLine("            snsType = " + m_instanceName + "_portTable[sensor];");
                        writer.AppendLine("            if((snsType & " + m_instanceName + "_COMPLEX_SS_FLAG) == 0u)");
                        writer.AppendLine("            {");
                        writer.AppendLine("                /* Enable sensor */");
                        writer.AppendLine("                " + m_instanceName + "_EnableSensor(sensor);");
                        writer.AppendLine("            }");
                        writer.AppendLine("            else");
                        writer.AppendLine("            {");
                        writer.AppendLine("                /* Enable complex scan sensor */");
                        writer.AppendLine("                snsType &= ~" + m_instanceName + "_COMPLEX_SS_FLAG;");
                        writer.AppendLine("                index = &" + m_instanceName + "_IndexTable[snsType];");
                        writer.AppendLine("                snsNumber = " + m_instanceName + "_maskTable[sensor];");
                        writer.AppendLine("                ");
                        writer.AppendLine("                for (i=0; i < snsNumber; i++)");
                        writer.AppendLine("                {");
                        writer.AppendLine("                    /* Enable sensor */");
                        writer.AppendLine("                    " + m_instanceName + "_EnableSensor(index[i]);");
                        writer.AppendLine("                }");
                        writer.AppendLine("            }");
                    }
                    else
                    {
                        writer.AppendLine("            /* Enable Sensor */");
                        writer.AppendLine("            " + m_instanceName + "_EnableSensor(sensor);");
                    }
                    #endregion

                }
                else
                {
                    if (m_settings.m_currentSource == CyCurrentSourceOptions.Idac_Sink)
                    {
                        writer.AppendLine("            /* Connect IDAC */");
                        writer.AppendLine("            " + m_instanceName + "_" + SH_AMUX + ch_number + "_Connect(" + m_instanceName + "_" + SH_AMUX + ch_number + "_IDAC_CHANNEL);");
                        writer.AppendLine("            ");
                    }
                    else if (m_settings.m_currentSource == CyCurrentSourceOptions.Idac_None)
                    {
                        writer.AppendLine("            /* Connect DSI output to Rb */");
                        writer.AppendLine("            *" + m_instanceName + "_rbTable[" + m_instanceName + "_RbCh" + ch_number + "_cur] |= " + m_instanceName + "_BYP_MASK;");
                        writer.AppendLine("            ");
                    }

                    #region __EnableSensor
                    if (m_isComplexSS)
                    {
                        writer.AppendLine("            /* Look for complex scan sensor */");
                        writer.AppendLine("            snsType = " + m_instanceName + "_portTable[sensor];");
                        writer.AppendLine("            if((snsType & " + m_instanceName + "_COMPLEX_SS_FLAG) == 0u)");
                        writer.AppendLine("            {");
                        writer.AppendLine("                /* Enable sensor */");
                        writer.AppendLine("                " + m_instanceName + "_EnableSensor(sensor);");
                        writer.AppendLine("            }");
                        writer.AppendLine("            else");
                        writer.AppendLine("            {");
                        writer.AppendLine("                /* Enable complex scan sensor */");
                        writer.AppendLine("                snsType &= ~" + m_instanceName + "_COMPLEX_SS_FLAG;");
                        writer.AppendLine("                index = &" + m_instanceName + "_IndexTable[snsType];");
                        writer.AppendLine("                snsNumber = " + m_instanceName + "_maskTable[i];");
                        writer.AppendLine("                ");
                        writer.AppendLine("                for (i=0; i < snsNumber; i++)");
                        writer.AppendLine("                {");
                        writer.AppendLine("                    /* Enable sensor */");
                        writer.AppendLine("                    " + m_instanceName + "_EnableSensor(index[i]);");
                        writer.AppendLine("                }");
                        writer.AppendLine("            }");
                    }
                    else
                    {
                        writer.AppendLine("            /* Enable Sensor */");
                        writer.AppendLine("            " + m_instanceName + "_EnableSensor(sensor);");
                    }
                    writer.AppendLine("            ");
                    #endregion

                    writer.AppendLine("            /* Disable CapSense Buffer */");
                    writer.AppendLine("            " + m_instanceName + "_" + SH_CSBUF + ch_number + "_CAPS_CFG0 &= ~" + m_instanceName + "_CSBUF_ENABLE;");
                    writer.AppendLine("            ");
                }
                #endregion

                writer.AppendLine("    }");
                writer.AppendLine("    ");
                writer.AppendLine("    if((" + m_instanceName + "_csv & " + m_instanceName + "_SW_STS_SCAN_DONE__CH1) == 0u)");
                writer.AppendLine("    {");
                writer.AppendLine("            sensor += " + m_instanceName + "_TOTAL_SENSOR_COUNT__CH0;");
                writer.AppendLine("            ");

                #region __CH1
                ch_number = "1";

                if (m_settings.m_currentSource == CyCurrentSourceOptions.Idac_Source)
                {
                    writer.AppendLine("            /* Disable CapSense Buffer */");
                    writer.AppendLine("            " + m_instanceName + "_" + SH_CSBUF + ch_number + "_CAPS_CFG0 &= ~" + m_instanceName + "_CSBUF_ENABLE;");
                    writer.AppendLine("            ");

                    #region EnableSensor
                    if (m_isComplexSS)
                    {
                        writer.AppendLine("            /* Look for complex scan sensor */");
                        writer.AppendLine("            snsType = " + m_instanceName + "_portTable[sensor];");
                        writer.AppendLine("            if((snsType & " + m_instanceName + "_COMPLEX_SS_FLAG) == 0u)");
                        writer.AppendLine("            {");
                        writer.AppendLine("                /* Enable sensor */");
                        writer.AppendLine("                " + m_instanceName + "_EnableSensor(sensor);");
                        writer.AppendLine("            }");
                        writer.AppendLine("            else");
                        writer.AppendLine("            {");
                        writer.AppendLine("                /* Enable complex scan sensor */");
                        writer.AppendLine("                snsType &= ~" + m_instanceName + "_COMPLEX_SS_FLAG;");
                        writer.AppendLine("                index = &" + m_instanceName + "_indexTable[snsType];");
                        writer.AppendLine("                snsNumber = " + m_instanceName + "_maskTable[i];");
                        writer.AppendLine("                ");
                        writer.AppendLine("                for (i=0; i < snsNumber; i++)");
                        writer.AppendLine("                {");
                        writer.AppendLine("                    /* Enable sensor */");
                        writer.AppendLine("                    " + m_instanceName + "_EnableSensor(index[i]);");
                        writer.AppendLine("                }");
                        writer.AppendLine("            }");
                    }
                    else
                    {
                        writer.AppendLine("            /* Enable Sensor */");
                        writer.AppendLine("            " + m_instanceName + "_EnableSensor(sensor);");
                    }
                    #endregion
                }
                else
                {
                    if (m_settings.m_currentSource == CyCurrentSourceOptions.Idac_Sink)
                    {
                        writer.AppendLine("            /* Connect IDAC */");
                        writer.AppendLine("            " + m_instanceName + "_" + SH_AMUX + ch_number + "_Connect(" + m_instanceName + "_" + SH_AMUX + ch_number + "_IDAC_CHANNEL);");
                        writer.AppendLine("            ");
                    }
                    else if (m_settings.m_currentSource == CyCurrentSourceOptions.Idac_None)
                    {
                        writer.AppendLine("            /* Connect DSI output to Rb */");
                        writer.AppendLine("            *" + m_instanceName + "_rbTable[" + m_instanceName + "_RbCh" + ch_number + "_cur] |= " + m_instanceName + "_BYP_MASK;");

                    }

                    #region EnableSensor
                    if (m_isComplexSS)
                    {
                        writer.AppendLine("            /* Look for complex scan sensor */");
                        writer.AppendLine("            snsType = " + m_instanceName + "_portTable[sensor];");
                        writer.AppendLine("            if((snsType & " + m_instanceName + "_COMPLEX_SS_FLAG) == 0u)");
                        writer.AppendLine("            {");
                        writer.AppendLine("                /* Enable sensor */");
                        writer.AppendLine("                " + m_instanceName + "_EnableSensor(sensor);");
                        writer.AppendLine("            }");
                        writer.AppendLine("            else");
                        writer.AppendLine("            {");
                        writer.AppendLine("                /* Enable complex scan sensor */");
                        writer.AppendLine("                snsType &= ~" + m_instanceName + "_COMPLEX_SS_FLAG;");
                        writer.AppendLine("                index = &" + m_instanceName + "_indexTable[snsType];");
                        writer.AppendLine("                snsNumber = " + m_instanceName + "_maskTable[sensor];");
                        writer.AppendLine("                ");
                        writer.AppendLine("                for (i=0; i < snsNumber; i++)");
                        writer.AppendLine("                {");
                        writer.AppendLine("                    /* Enable sensor */");
                        writer.AppendLine("                    " + m_instanceName + "_EnableSensor(index[i]);");
                        writer.AppendLine("                }");
                        writer.AppendLine("            }");
                    }
                    else
                    {
                        writer.AppendLine("            /* Enable Sensor */");
                        writer.AppendLine("            " + m_instanceName + "_EnableSensor(sensor);");
                    }
                    writer.AppendLine("            ");
                    #endregion

                    writer.AppendLine("            /* Disable CapSense Buffer */");
                    writer.AppendLine("            " + m_instanceName + "_" + SH_CSBUF + ch_number + "_CAPS_CFG0 &= ~" + m_instanceName + "_CSBUF_ENABLE;");
                    writer.AppendLine("            ");
                }
                #endregion

                writer.AppendLine("    }");
            }

            writer.AppendLine("    ");
            writer.AppendLine("    /* Start PWM One Shout and PRS at one time */");
            writer.AppendLine("    " + m_instanceName + "_CONTROL |= " + m_instanceName + "_CTRL_START;");
            writer.AppendLine("}");
            writer.AppendLine("");
            writer.AppendLine("");
            #endregion

            #region __PostScan
            writer.AppendLine("/*******************************************************************************");
            writer.AppendLine("* Function Name: " + m_instanceName + "_PostScan");
            writer.AppendLine("********************************************************************************");
            writer.AppendLine("*");
            writer.AppendLine("* Summary:");
            writer.AppendLine("*  Store results of measurament in " + m_instanceName + "_SensorResult[ ] array and");
            writer.AppendLine("*  sets scan sensor in none sampling state");
            writer.AppendLine("*");
            writer.AppendLine("*");
            writer.AppendLine("* Parameters:");
            writer.AppendLine("*  sensor:  Scan sensor number");
            writer.AppendLine("*");
            writer.AppendLine("* Return:");
            writer.AppendLine("*  void");
            writer.AppendLine("*");
            writer.AppendLine("*******************************************************************************/");
            writer.AppendLine("void " + m_instanceName + "_PostScan(uint8 sensor)");
            writer.AppendLine("{");
            if (m_isComplexSS)
            {
                writer.AppendLine("    uint8 i, snsType, snsNumber;");
                writer.AppendLine("    uint8 *index;");
                writer.AppendLine("    ");
            }

            if (m_settings.m_configuration == CyChannelConfig.ONE_CHANNEL)
            {
                #region __ONE_CHANNEL_DESIGN
                string ch_number = sbItem.GetChannelNumber().ToString();

                writer.AppendLine("    /* Stop Capsensing and rearm sync */");
                writer.AppendLine("    " + m_instanceName + "_CONTROL &= ~" + m_instanceName + "_CTRL_START;");
                writer.AppendLine("    " + m_instanceName + "_CONTROL &= ~" + m_instanceName + "_CTRL_SYNC_EN;");
                writer.AppendLine("    ");

                writer.AppendLine("    /* Read SlotResult from Raw Counter */");
                writer.AppendLine("    " + m_instanceName + "_SensorRaw[sensor] = CY_GET_REG16(" + m_instanceName + "_RawCH0_COUNTER_PTR);");

                #region Disable Sensor
                if (m_isComplexSS)
                {
                    writer.AppendLine("    /* Look for complex scan sensor */");
                    writer.AppendLine("    snsType = " + m_instanceName + "_portTable[sensor];");
                    writer.AppendLine("    if((snsType & " + m_instanceName + "_COMPLEX_SS_FLAG) == 0u)");
                    writer.AppendLine("    {");
                    writer.AppendLine("        /* Disable sensor */");
                    writer.AppendLine("        " + m_instanceName + "_DisableSensor(sensor);");
                    writer.AppendLine("    }");
                    writer.AppendLine("    else");
                    writer.AppendLine("    {");
                    writer.AppendLine("        /* Disable complex scan sensor */");
                    writer.AppendLine("        snsType &= ~" + m_instanceName + "_COMPLEX_SS_FLAG;");
                    writer.AppendLine("        index = &" + m_instanceName + "_indexTable[snsType];");
                    writer.AppendLine("        snsNumber = " + m_instanceName + "_maskTable[sensor];");
                    writer.AppendLine("                        ");
                    writer.AppendLine("        for (i=0; i < snsNumber; i++)");
                    writer.AppendLine("        {");
                    writer.AppendLine("            /* Disable sensor */");
                    writer.AppendLine("            " + m_instanceName + "_DisableSensor(index[i]);");
                    writer.AppendLine("        }");
                    writer.AppendLine("    }");
                }
                else
                {
                    writer.AppendLine("    /* Disable Sensor */");
                    writer.AppendLine("    " + m_instanceName + "_DisableSensor(sensor);");
                }
                writer.AppendLine("    ");
                #endregion

                if (m_isIdacInSystem)
                {
                    writer.AppendLine("    /* Turn off IDAC */");
                    writer.AppendLine("    " + m_instanceName + "_" + SH_IDAC + ch_number + "_SetValue(" + m_instanceName + "_TURN_OFF_IDAC);");
                    writer.AppendLine("    ");
                }

                if (m_settings.m_currentSource == CyCurrentSourceOptions.Idac_Sink)
                {
                    writer.AppendLine("    /* Disconnect IDAC */");
                    writer.AppendLine("    " + m_instanceName + "_" + SH_AMUX + ch_number + "_Disconnect(" + m_instanceName + "_" + SH_AMUX + ch_number + "_IDAC_CHANNEL);");
                    writer.AppendLine("    ");
                }
                else if (m_settings.m_currentSource == CyCurrentSourceOptions.Idac_None)
                {
                    writer.AppendLine("    /* Disconnect DSI output from Rb */");
                    writer.AppendLine("    *" + m_instanceName + "_rbTable[" + m_instanceName + "_RbCh" + ch_number + "_cur] &= ~" + m_instanceName + "_BYP_MASK;");
                    writer.AppendLine("    ");
                }

                writer.AppendLine("    /* Enable Vref on AMUX */");
                writer.AppendLine("    " + m_instanceName + "_" + SH_CSBUF + ch_number + "_CAPS_CFG0 |= " + m_instanceName + "_CSBUF_ENABLE;");
                #endregion
            }
            else
            {

                writer.AppendLine("    if( ((" + m_instanceName + "_csv & " + m_instanceName + "_SW_STS_SCAN_DONE__CH0) != 0u) &&");
                writer.AppendLine("        ((" + m_instanceName + "_csv & " + m_instanceName + "_SW_STS_SCAN_DONE__CH1) != 0u) )");
                writer.AppendLine("    {");
                writer.AppendLine("        /* Stop Capsensing */");
                writer.AppendLine("        " + m_instanceName + "_CONTROL &= ~" + m_instanceName + "_CTRL_START;");
                writer.AppendLine("        " + m_instanceName + "_CONTROL &= ~" + m_instanceName + "_CTRL_SYNC_EN;");
                writer.AppendLine("    }");
                writer.AppendLine("    ");
                writer.AppendLine("    if (sensor < " + m_instanceName + "_TOTAL_SENSOR_COUNT__CH0)");
                writer.AppendLine("    {");

                #region __CH0
                string ch_number = "0";

                writer.AppendLine("        /* Read SlotResult from Raw Counter */");
                writer.AppendLine("        " + m_instanceName + "_SensorRaw[sensor] = CY_GET_REG16(" + m_instanceName + "_RawCH0_COUNTER_PTR);");
                writer.AppendLine("        ");

                #region __DisableSensor
                if (m_isComplexSS)
                {
                    writer.AppendLine("        /* Look for complex scan sensor */");
                    writer.AppendLine("        snsType = " + m_instanceName + "_portTable[sensor];");
                    writer.AppendLine("        if((snsType & " + m_instanceName + "_COMPLEX_SS_FLAG) == 0u)");
                    writer.AppendLine("        {");
                    writer.AppendLine("            /* Disable sensor */");
                    writer.AppendLine("            " + m_instanceName + "_DisableSensor(sensor);");
                    writer.AppendLine("        }");
                    writer.AppendLine("        else");
                    writer.AppendLine("        {");
                    writer.AppendLine("            /* Disable complex scan sensor */");
                    writer.AppendLine("            snsType &= ~" + m_instanceName + "_COMPLEX_SS_FLAG;");
                    writer.AppendLine("            index = &" + m_instanceName + "_indexTable[snsType];");
                    writer.AppendLine("            snsNumber = " + m_instanceName + "_maskTable[sensor];");
                    writer.AppendLine("                        ");
                    writer.AppendLine("            for (i=0; i < snsNumber; i++)");
                    writer.AppendLine("            {");
                    writer.AppendLine("                /* Disable sensor */");
                    writer.AppendLine("                " + m_instanceName + "_DisableSensor(index[i]);");
                    writer.AppendLine("            }");
                    writer.AppendLine("        }");
                }
                else
                {
                    writer.AppendLine("        /* Disable Sensor */");
                    writer.AppendLine("        " + m_instanceName + "_DisableSensor(sensor);");
                }
                writer.AppendLine("        ");
                #endregion

                if (m_isIdacInSystem)
                {
                    writer.AppendLine("        /* Turn off IDAC */");
                    writer.AppendLine("        " + m_instanceName + "_" + SH_IDAC + ch_number + "_SetValue(" + m_instanceName + "_TURN_OFF_IDAC);");
                    writer.AppendLine("        ");
                }

                if (m_settings.m_currentSource == CyCurrentSourceOptions.Idac_Sink)
                {
                    writer.AppendLine("        /* Disconnect IDAC */");
                    writer.AppendLine("        " + m_instanceName + "_" + SH_AMUX + ch_number + "_Disconnect(" + m_instanceName + "_" + SH_AMUX + ch_number + "_IDAC_CHANNEL);");
                    writer.AppendLine("        ");
                }
                else if (m_settings.m_currentSource == CyCurrentSourceOptions.Idac_None)
                {
                    writer.AppendLine("        /* Disconnect DSI output from Rb */");
                    writer.AppendLine("        *" + m_instanceName + "_rbTable[" + m_instanceName + "_RbCh" + ch_number + "_cur] &= ~" + m_instanceName + "_BYP_MASK;");
                    writer.AppendLine("        ");
                }

                writer.AppendLine("        /* Enable Vref on AMUX */");
                writer.AppendLine("        " + m_instanceName + "_" + SH_CSBUF + ch_number + "_CAPS_CFG0 |= " + m_instanceName + "_CSBUF_ENABLE;");

                #endregion

                writer.AppendLine("    }");
                writer.AppendLine("    else");
                writer.AppendLine("    {");

                #region __CH1
                ch_number = "1";

                writer.AppendLine("        /* Read SlotResult from Raw Counter */");
                writer.AppendLine("        " + m_instanceName + "_SensorRaw[sensor] = CY_GET_REG16(" + m_instanceName + "_RawCH1_COUNTER_PTR);");
                writer.AppendLine("        ");

                #region __DisalbeSensor
                if (m_isComplexSS)
                {
                    writer.AppendLine("        /* Look for complex scan sensor */");
                    writer.AppendLine("        snsType = " + m_instanceName + "_portTable[sensor];");
                    writer.AppendLine("        if((snsType & " + m_instanceName + "_COMPLEX_SS_FLAG) == 0u)");
                    writer.AppendLine("        {");
                    writer.AppendLine("            /* Disable sensor */");
                    writer.AppendLine("            " + m_instanceName + "_DisableSensor(sensor);");
                    writer.AppendLine("        }");
                    writer.AppendLine("        else");
                    writer.AppendLine("        {");
                    writer.AppendLine("            /* Disable complex scan sensor */");
                    writer.AppendLine("            snsType &= ~" + m_instanceName + "_COMPLEX_SS_FLAG;");
                    writer.AppendLine("            index = &" + m_instanceName + "_indexTable[snsType];");
                    writer.AppendLine("            snsNumber = " + m_instanceName + "_maskTable[sensor];");
                    writer.AppendLine("                        ");
                    writer.AppendLine("            for (i=0; i < snsNumber; i++)");
                    writer.AppendLine("            {");
                    writer.AppendLine("                /* Disable sensor */");
                    writer.AppendLine("                " + m_instanceName + "_DisableSensor(index[i]);");
                    writer.AppendLine("            }");
                    writer.AppendLine("        }");
                }
                else
                {
                    writer.AppendLine("        /* Disable Sensor */");
                    writer.AppendLine("        " + m_instanceName + "_DisableSensor(sensor);");
                }
                writer.AppendLine("        ");
                #endregion

                if (m_isIdacInSystem)
                {
                    writer.AppendLine("        /* Turn off IDAC */");
                    writer.AppendLine("        " + m_instanceName + "_" + SH_IDAC + ch_number + "_SetValue(" + m_instanceName + "_TURN_OFF_IDAC);");
                    writer.AppendLine("        ");
                }

                if (m_settings.m_currentSource == CyCurrentSourceOptions.Idac_Sink)
                {
                    writer.AppendLine("        /* Disconnect IDAC */");
                    writer.AppendLine("        " + m_instanceName + "_" + SH_AMUX + ch_number + "_Disconnect(" + m_instanceName + "_" + SH_AMUX + ch_number + "_IDAC_CHANNEL);");
                    writer.AppendLine("        ");
                }
                else if (m_settings.m_currentSource == CyCurrentSourceOptions.Idac_None)
                {
                    writer.AppendLine("        /* Disconnect DSI output from Rb */");
                    writer.AppendLine("        *" + m_instanceName + "_rbTable[" + m_instanceName + "_RbCh" + ch_number + "_cur] &= ~" + m_instanceName + "_BYP_MASK;");
                    writer.AppendLine("        ");
                }

                writer.AppendLine("        /* Enable Vref on AMUX */");
                writer.AppendLine("        " + m_instanceName + "_" + SH_CSBUF + ch_number + "_CAPS_CFG0 |= " + m_instanceName + "_CSBUF_ENABLE;");
                #endregion

                writer.AppendLine("    }");
            }

            writer.AppendLine("}");
            writer.AppendLine("");
            writer.AppendLine("");
            #endregion

        }
        #endregion        
    }
}
