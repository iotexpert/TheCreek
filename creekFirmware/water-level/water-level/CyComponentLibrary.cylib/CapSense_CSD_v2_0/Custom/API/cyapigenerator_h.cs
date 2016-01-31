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

namespace CapSense_CSD_v2_0
{
    public partial class CyAPIGenerator
    {
        #region CollectApiHFile
        public void CollectApiHFile(ref Dictionary<string, string> paramDict)
        {
            StringBuilder writer;

            writer = new StringBuilder();
            ApiCollectH__Includes(ref writer);
            paramDict.Add(pa_Include, writer.ToString());

            writer = new StringBuilder();
            ApiCollectH__DefineCommon(ref writer);
            ApiCollectH__DefineSns(ref writer);
            paramDict.Add(pa_DefineConstants, writer.ToString());

            paramDict.Add("CheckSum", m_checkSum.ToString());
        }
        #endregion

        #region ApiCollectH__DefineCommon
        void ApiCollectH__DefineCommon(ref StringBuilder writer)
        {
            writer.AppendFormat("#define " + m_instanceName + "_TOTAL_SENSOR_COUNT            ({0}u)\r\n",
                m_listSS.Count);

            if (m_settings.m_configuration == CyChannelConfig.TWO_CHANNELS)
            {
                writer.AppendFormat("#define " + m_instanceName + "_TOTAL_SENSOR_COUNT__CH0            ({0}u)\r\n",
                    m_packParams.m_widgets.m_scanSlots.GetSSList(CyChannelNumber.First).Count);
                writer.AppendFormat("#define " + m_instanceName + "_TOTAL_SENSOR_COUNT__CH1            ({0}u)\r\n",
                    m_packParams.m_widgets.m_scanSlots.GetSSList(CyChannelNumber.Second).Count);
            }

            string rb_config = "";
            int rb_count = 0;
            foreach (CyChannelProperties item in m_settings.m_listChannels)
            {
                rb_config += string.Format("#define " + m_instanceName + "_TOTAL_RB_NUMBER{0}            ({1}u)\r\n",
                    item.GetChannelSufixAPI(), item.m_rb);
                rb_count += item.m_rb;
            }
            rb_config = string.Format("#define " + m_instanceName + "_TOTAL_RB_NUMBER            ({0}u)\r\n", rb_count) 
                + rb_config;
            writer.Append(rb_config);
        }
        #endregion

        #region ApiCollectH__DefineSns
        void ApiCollectH__DefineSns(ref StringBuilder writer)
        {            
            writer.AppendLine("");
            writer.AppendLine("/* Define Sensors */");
            //Define Sensors
            for (int i = 0; i < m_listSS.Count; i++)
            {
                string str = "#define " + m_instanceName + "_SENSOR_" + m_listSS[i].GenHeader().ToUpper() + "    ("
                    + i + "u)";
                writer.AppendLine(str);
            }
            
            /* AMux Cmod, Comparator and Idac Channels definitions */
            writer.AppendLine("/* AMux Cmod, Comparator and Idac Channels definitions */");
            foreach (CyChannelProperties item in m_settings.m_listChannels)
            {
                string ch_number = item.GetChannelNumber().ToString();
                int offset = m_packParams.m_widgets.GetListTerminals(item.m_channel).Count;
                if (m_settings.m_guardSensorEnable)
                    offset++;
                writer.AppendLine("#define " + m_instanceName + "_" + SH_AMUX + ch_number + "_CMOD_CHANNEL          (" +
                    (offset).ToString() + "u)");
                writer.AppendLine("#define " + m_instanceName + "_" + SH_AMUX + ch_number + "_CMP_VP_CHANNEL        (" +
                    (offset + 1).ToString() + "u)");
                if (m_isIdacInSystem)
                {
                    writer.AppendLine("#define " + m_instanceName + "_" + SH_AMUX + ch_number + "_IDAC_CHANNEL          (" +
                        (offset + 2).ToString() + "u)");
                }
                writer.AppendLine("");
            }

            if (m_settings.m_vrefOption == CyVrefOptions.Ref_Vdac)
            {
                /* Vref Vdac Value */
                writer.AppendLine("#define " + m_instanceName + "_VREF_VDAC_VALUE        (" +
                    m_settings.m_vrefVdacValue.ToString() + "u)");
            }
        }
        #endregion

        #region ApiCollectH__Includes
        void ApiCollectH__Includes(ref StringBuilder writer)
        {
            /* Include internal clock if selected */
            writer.AppendLine("");
            if (m_settings.m_clockType == CyClockSourceOptions.Internal)
            {
                writer.AppendLine("#include \"" + m_instanceName + "_" + SH_INT_CLK + ".h\"");
            }


            /* Include Pwm, Counter, AMux and Comparator for each Channel */
            foreach (CyChannelProperties item in m_settings.m_listChannels)
            {
                string ch_number = item.GetChannelNumber().ToString();
                writer.AppendLine("");
                writer.AppendLine("#include \"" + m_instanceName + "_" + SH_AMUX + ch_number + ".h\"");
                writer.AppendLine("#include \"" + m_instanceName + "_" + SH_COMP + ch_number + ".h\"");
                /* Include VDAC as Reference if selected */
                if (m_settings.m_vrefOption == CyVrefOptions.Ref_Vdac)
                {
                    writer.AppendLine("#include \"" + m_instanceName + "_" + SH_REF_VDAC + ch_number + ".h\"");
                }
                writer.AppendLine("");
            }
        }
        #endregion
    }
}
