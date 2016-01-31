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
        public void CollectApiCSHLHFile(ref Dictionary<string, string> paramDict)
        {
            StringBuilder writer = new StringBuilder();            
            ApiCollectCSHLFunctionPrototypesCommon(ref writer);

            paramDict.Add(PA_CSHL_H_FILE, writer.ToString());

        }

        #region ApiCollectCSHLFunctionPrototypesCommon
        void ApiCollectCSHLFunctionPrototypesCommon(ref StringBuilder writer)
        {
            writer.AppendLine("");
            if (IsWidgetCSHL(CyWidgetAPITypes.SliderLinear))
                writer.AppendLine("uint16 " + m_instanceName + "_GetCentroidPos(uint8 widget);");
            if (IsWidgetCSHL(CyWidgetAPITypes.SliderRadial))
                writer.AppendLine("uint16 " + m_instanceName + "_GetRadialCentroidPos(uint8 widget);");
            if (IsWidgetCSHL(CyWidgetAPITypes.Touchpad) ||
                IsWidgetCSHL(CyWidgetAPITypes.Touchpad))
                writer.AppendLine("uint16 " + m_instanceName + "_GetTouchCentroidPos(uint8 widget);");

            writer.AppendLine("");
            writer.AppendLine("");
            writer.AppendLine("/***************************************");
            writer.AppendLine("*           API Constants");
            writer.AppendLine("***************************************/");

            writer.AppendLine("");
            writer.AppendLine("/* Widgets constants definition */");
            writer.AppendLine("");
            for (int i = 0; i < m_listWidget.Count; i++)
                if (CyCsConst.IsMainPartOfWidget(m_listWidget[i].m_type))
                {
                    CyWidget wi = m_packParams.m_widgets.GetBothParts(m_listWidget[i])[0];
                    writer.AppendFormat("#define {0}_{1}        ({2}u)\r\n", m_instanceName, wi.GetWidgetDefine().ToUpper(), i);
                }      

            writer.AppendLine("");
            writer.AppendFormat("#define {0}_TOTAL_DIPLEXED_SLIDERS_COUNT        ({1}u)\r\n", m_instanceName, GetWidgetCount(CyWidgetAPITypes.SliderLinearDiplexed));
            writer.AppendFormat("#define {0}_TOTAL_LINEAR_SLIDERS_COUNT          ({1}u)\r\n", m_instanceName, GetWidgetCount(CyWidgetAPITypes.SliderLinear));
            writer.AppendFormat("#define {0}_TOTAL_RADIAL_SLIDERS_COUNT          ({1}u)\r\n", m_instanceName, GetWidgetCount(CyWidgetAPITypes.SliderRadial));
            writer.AppendFormat("#define {0}_TOTAL_TOUCH_PADS_COUNT              ({1}u)\r\n", m_instanceName, (int)(GetWidgetCount(CyWidgetAPITypes.Touchpad) / 2));
            writer.AppendFormat("#define {0}_TOTAL_BUTTONS_COUNT                 ({1}u)\r\n", m_instanceName, GetWidgetCount(CyWidgetAPITypes.Proximity) + GetWidgetCount(CyWidgetAPITypes.Button));
            writer.AppendFormat("#define {0}_TOTAL_MATRIX_BUTTONS_COUNT          ({1}u)\r\n", m_instanceName, (int)(GetWidgetCount(CyWidgetAPITypes.MatrixButton) / 2));
            writer.AppendFormat("#define {0}_TOTAL_GENERICS_COUNT                ({1}u)\r\n", m_instanceName, (m_listWidget.Count - m_listWidgetCSHL.Count).ToString());
            writer.AppendLine("");
            writer.AppendLine("#define " + m_instanceName + "_POS_FILTERS_MASK" + "        (" + m_posFiltersMask +"u)");
            writer.AppendLine("");
            writer.AppendLine("#define " + m_instanceName + "_UNUSED_DEBOUNCE_COUNTER_INDEX   (" + m_debounceMaxOffset.ToString() +"u)");
            writer.AppendLine("");
        }
        #endregion
    }
}