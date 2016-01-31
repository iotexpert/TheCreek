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
    public enum CyWidgetAPITypes
    {
        All = 0,
        Button = 1,
        SliderLinear = 2,
        SliderRadial = 3,
        Touchpad = 4,
        MatrixButton = 5,
        Proximity = 6,
        Generic = 7,
        SliderLinearDiplexed = 8
    }

    public partial class CyAPIGenerator
    {
        #region Const Parameters
        //API parameters                
        const string pa_Include = "Include";
        const string pa_DefineConstants = "DefineConstants";

        const string PA_CSHL_H_FILE = "writerCSHLHFile";
        const string PA_CSHL_C_FILE = "writerCSHLCFile";

        //Schematic consts
        const string SH_INT_CLK = "IntClock";
        const string SH_PRS = "Prs";
        const string SH_COMP = "CompCH";
        const string SH_AMUX = "AMuxCH";
        const string SH_IDAC = "IdacCH";
        const string SH_CSPORT = "PortCH";
        const string SH_CSBUF = "BufCH";
        const string SH_REF_VDAC = "VdacRefCH";

        const string DUMMY_WIDGET = "DummyWidget";

        #endregion

        public string m_instanceName;
        bool m_isUnDiplexSlider;
        bool m_isDiplexSlider;
        bool m_isAnyCentroid;
        bool m_isLinearAlone;
        bool m_isIdacInSystem;
        public bool m_isComplexSS;
        bool m_isRbAvailible;
        bool m_isPRS;
        bool m_isPrescaler;
        byte m_prsResolution;
        public int m_numberOfCentroids = 0;
        int m_posFiltersMask = 0;
        int m_debounceMaxOffset = -1;
        bool m_isEmptyCapSense = false;
        int m_firstChannelSSCount = 0;
        string m_constTuner = " ";
        string m_cycodeTuner = " ";
        public UInt16 m_checkSum = 0;

        public List<CyScanSlot> m_listSS;
        public List<CyWidget> m_listWidget;
        List<CyTerminal> m_listDedicatedTerminals = new List<CyTerminal>();
        public List<CyWidget> m_listWidgetCSHL;
        public CyCSSettings m_settings;
        CyCSParameters m_packParams = null;
        int[] m_widgetsCount = new int[Enum.GetNames(typeof(CyWidgetAPITypes)).Length];

        public CyAPIGenerator(CyCSParameters packParams, CyCSInstParameters query)
        {
            m_packParams = packParams;
            if (query != null)
            {
                m_instanceName = query.GetCommittedParam("INSTANCE_NAME").Value;
            }
            System.Diagnostics.Debug.Assert(m_packParams != null);
            m_settings = m_packParams.m_settings;

            //Remove Second channel
            if (m_packParams.m_settings.Configuration == CyChannelConfig.ONE_CHANNEL)
                if (m_packParams.m_settings.m_listChannels.Count > 1)
                    m_packParams.m_settings.m_listChannels.RemoveAt(1);

            #region Add Dummy Widget
            if (m_packParams.m_widgets.GetListWidgets().Count == 0)
            {
                //Add dummy widget for proper API generation
                m_packParams.m_widgets.AddWidget(DUMMY_WIDGET, CySensorType.Button, CyChannelNumber.First, false);
                m_isEmptyCapSense = true;
            }
            #endregion

            m_packParams.m_widgets.PrepareWidgetsForAPIGenerator(packParams.m_settings.Configuration);

            m_listWidget = m_packParams.m_widgets.GetListWidgets();
            List<CyWidget> lisGenerics = new List<CyWidget>();

            //Add Guard Widget
            if (m_settings.m_guardSensorEnable) m_listWidget.Add(m_packParams.m_widgets.m_guardSensorWidget);

            #region Fill m_listWidget m_listWidgetCSHL
            m_listWidgetCSHL = new List<CyWidget>(m_listWidget.Count);
            int ind = 0;
            while (ind < m_listWidget.Count)
            {
                CyWidget wi = m_listWidget[ind];
                if (wi.m_type == CySensorType.Generic)
                {
                    lisGenerics.Add(wi);
                    m_listWidget.RemoveAt(ind);
                    continue;
                }
                m_listWidgetCSHL.Add(wi);
                ind++;
            }
            for (int i = 0; i < lisGenerics.Count; i++)
                m_listWidget.Add(lisGenerics[i]);
            #endregion

            m_firstChannelSSCount = m_packParams.m_widgets.m_scanSlots.GetSSList(CyChannelNumber.First).Count;
            #region Fill m_listSS
            m_listSS =null;
            CyScanSlot guard = m_packParams.m_widgets.m_scanSlots.m_guardSensor;
            if (m_settings.Configuration == CyChannelConfig.TWO_CHANNELS)
            {
                m_listSS = new List<CyScanSlot>();
                m_listSS.AddRange(m_packParams.m_widgets.m_scanSlots.GetSSList(CyChannelNumber.First));
                //Add Guard ScanSlot
                if (m_settings.m_guardSensorEnable && guard.Channel == CyChannelNumber.First) m_listSS.Add(guard);

                m_listSS.AddRange(m_packParams.m_widgets.m_scanSlots.GetSSList(CyChannelNumber.Second));
                //Add Guard ScanSlot
                if (m_settings.m_guardSensorEnable && guard.Channel == CyChannelNumber.Second) m_listSS.Add(guard);
            }
            else
            {
                m_listSS = new List<CyScanSlot>(m_packParams.m_widgets.m_scanSlots.m_listScanSlots);
                if (m_settings.m_guardSensorEnable)
                    m_listSS.Add(guard);
            }

            #endregion

            #region Fill m_listDedicatedTerminals
            for (int i = 0; i < m_listSS.Count; i++)
                if (m_listSS[i].IsScanSlotComplex())
                    for (int k = 0; k < m_listSS[i].GetTerminals().Count; k++)
                    {
                        CyTerminal term = m_listSS[i].GetTerminals()[k];
                        if (CyCsConst.HasComplexScanSlot(term.WidgetType) &&
                                m_listDedicatedTerminals.IndexOf(term) == -1)
                            m_listDedicatedTerminals.Add(term);
                    }
            #endregion

            #region Fill widgets count

            List<CyWidget> list = m_listWidgetCSHL;
            m_widgetsCount[(int)CyWidgetAPITypes.All] = list.Count;
            m_widgetsCount[(int)CyWidgetAPITypes.Generic] = m_listWidget.Count - m_listWidgetCSHL.Count;

            for (int i = 0; i < list.Count; i++)
            {
                if (list[i] is CyButton) m_widgetsCount[(int)CyWidgetAPITypes.Button]++;
                if (list[i] is CyTouchPad) m_widgetsCount[(int)CyWidgetAPITypes.Touchpad]++;
                if (list[i] is CyMatrixButton) m_widgetsCount[(int)CyWidgetAPITypes.MatrixButton]++;
                if (list[i].m_type == CySensorType.SliderLinear)
                {
                    m_widgetsCount[(int)CyWidgetAPITypes.SliderLinear]++;
                    if ((list[i] as CySlider).m_diplexing)
                        m_widgetsCount[(int)CyWidgetAPITypes.SliderLinearDiplexed]++;
                }
                if (list[i].m_type == CySensorType.SliderRadial)
                    m_widgetsCount[(int)CyWidgetAPITypes.SliderRadial]++;
            }
            #endregion

            #region Calculate bool values
            m_isUnDiplexSlider = (((GetWidgetCount(CyWidgetAPITypes.SliderLinear) -
        GetWidgetCount(CyWidgetAPITypes.SliderLinearDiplexed)) > 0) || IsWidgetCSHL(CyWidgetAPITypes.Touchpad));
            m_isDiplexSlider = GetWidgetCount(CyWidgetAPITypes.SliderLinearDiplexed) > 0;
            m_isAnyCentroid = IsAnyCentroid();
            m_isLinearAlone = IsAloneSlider();
            m_isIdacInSystem = m_settings.IsIdacInSystem();
            m_isComplexSS = m_packParams.m_widgets.m_scanSlots.IsComplexScanSlots();
            m_isRbAvailible = (m_settings.m_currentSource == CyCurrentSourceOptions.Idac_None);
            m_isPRS = m_settings.m_prs != 0;
            m_isPrescaler = m_settings.m_prescaler != CyPrescalerOptions.Prescaler_None;
            #endregion

            #region GetPrs
            switch (m_settings.m_prs)
            {
                case CyPrsOptions.Prs_8bits:
                    m_prsResolution = 8;
                    break;
                case CyPrsOptions.Prs_16bits:
                case CyPrsOptions.Prs_16bits_0_25:
                    m_prsResolution = 16;
                    break;
                default:
                    m_prsResolution = 0;
                    break;
            }
            #endregion

            m_numberOfCentroids = GetWidgetCount(CyWidgetAPITypes.SliderLinear) +
                GetWidgetCount(CyWidgetAPITypes.SliderRadial) +
                GetWidgetCount(CyWidgetAPITypes.Touchpad);
            m_posFiltersMask = GetPosFiltersMask();

            #region Claculate m_debounceMaxOffset
            m_debounceMaxOffset = m_listWidgetCSHL.Count - m_numberOfCentroids;
            for (int i = m_numberOfCentroids; i < m_listWidgetCSHL.Count; i++)
            {
                int count = m_listWidgetCSHL[i].GetCount();
                if (count > 1) m_debounceMaxOffset += count;
            }
            #endregion

            m_constTuner = m_settings.m_tuningMethod == CyTuningMethodOptions.Tuning_None ? "const " : "";
            m_cycodeTuner = m_settings.m_tuningMethod == CyTuningMethodOptions.Tuning_None ? " CYCODE " : " ";

            #region Claculate CheckSum
            for (int i = 0; i < m_listWidget.Count; i++)
            {
                AddToCheckSum(m_listWidget[i].GetCount(), i);
                AddToCheckSum((int)m_listWidget[i].m_channel, i);
                AddToCheckSum(m_listWidget[i].GetFullResolution(), i);

                if (m_listWidget[i].GetAdditionalProperties().Length > 0)
                {
                    CyTuningProperties props = (CyTuningProperties)m_listWidget[i].GetAdditionalProperties()[0];
                    AddToCheckSum(props.m_debounce, i);
                    AddToCheckSum(props.m_hysteresis, i);
                    AddToCheckSum(props.m_noiseThreshold, i);
                    AddToCheckSum((int)props.m_scanResolution, i);
                }
            }
            for (int i = 0; i < m_listSS.Count; i++)
            {
                AddToCheckSum(m_listSS[i].IdacSettings, i);
                AddToCheckSum((int)m_listSS[i].WidgetType, i);
                AddToCheckSum(m_listWidget.IndexOf(m_listSS[i].m_widget), i);
            }
            AddToCheckSum(m_settings.m_widgetResolution, 0);
            #endregion
        }
        void AddToCheckSum(int val, int i)
        {
            m_checkSum = (UInt16)(m_checkSum + val * i + 1);
        }

        #region Functions

        #region Service Functions
        string Idac_PolarityGenerate()
        {
            string res = m_instanceName + "_IDAC_IDIR_";
            return m_settings.m_currentSource == CyCurrentSourceOptions.Idac_Source ? res += "SRC" : res += "SINK";
        }

        public string GetResolution(CyScanResolutionType resolution)
        {
            switch (resolution)
            {
                case CyScanResolutionType._8:
                    return "_PWM_RESOLUTION_8_BITS";
                case CyScanResolutionType._9:
                    return "_PWM_RESOLUTION_9_BITS";
                case CyScanResolutionType._10:
                    return "_PWM_RESOLUTION_10_BITS";
                case CyScanResolutionType._11:
                    return "_PWM_RESOLUTION_11_BITS";
                case CyScanResolutionType._12:
                    return "_PWM_RESOLUTION_12_BITS";
                case CyScanResolutionType._13:
                    return "_PWM_RESOLUTION_13_BITS";
                case CyScanResolutionType._14:
                    return "_PWM_RESOLUTION_14_BITS";
                case CyScanResolutionType._15:
                    return "_PWM_RESOLUTION_15_BITS";
                case CyScanResolutionType._16:
                    return "_PWM_RESOLUTION_16_BITS";
            }
            Debug.Assert(false);
            return "_PWM_RESOLUTION_8_BITS";
        }        

        void AddDescription(ref StringBuilder writer, int index_ss)
        {
            //Add description
            if (index_ss == m_listSS.Count - 1 || m_listSS[index_ss].WidgetName != m_listSS[index_ss + 1].WidgetName)
            {
                writer.AppendLine(string.Format("/* {0} */", m_listSS[index_ss].WidgetName));
                writer.Append("    ");
            }
        }
        string AddDiplexing(CyWidget wi)
        {
            string res = "";
            int[] arrIndexs = null;
            if (wi is CySlider) arrIndexs = (wi as CySlider).GenerateDiplexIndexes();
            if (arrIndexs != null)
                for (int j = 0; j < arrIndexs.Length; j++)
                {
                    res += arrIndexs[j].ToString() + "u, ";
                }
            return res;
        }
        #endregion

        public int GetWidgetCount(CyWidgetAPITypes stype)
        {
            return m_widgetsCount[(int)stype];
        }
        public bool IsWidgetCSHL(CyWidgetAPITypes stype)
        {
            return m_widgetsCount[(int)stype] > 0;
        }

        public bool IsAnyCentroid()
        {
            return (IsWidgetCSHL(CyWidgetAPITypes.SliderLinear) ||
                     IsWidgetCSHL(CyWidgetAPITypes.SliderRadial) ||
                     IsWidgetCSHL(CyWidgetAPITypes.Touchpad));
        }
        public bool IsAloneSlider()
        {

            if (IsWidgetCSHL(CyWidgetAPITypes.SliderRadial))
            {
                return !(IsWidgetCSHL(CyWidgetAPITypes.Touchpad) ||
                         IsWidgetCSHL(CyWidgetAPITypes.SliderLinear));
            }
            else
            {
                return true;
            }

        }

        public bool IsTwoDoubleWidgets()
        {

            if (((int)(GetWidgetCount(CyWidgetAPITypes.Touchpad) / 2) > 1) ||
                 ((int)(GetWidgetCount(CyWidgetAPITypes.MatrixButton) / 2) > 1) ||
                 (IsWidgetCSHL(CyWidgetAPITypes.Touchpad) && IsWidgetCSHL(CyWidgetAPITypes.MatrixButton))
               )
            {
                return true;
            }
            else
            {
                return false;
            }

        }

        #endregion

        #region Filters
        bool IsFilterForWidget(CyWidgetAPITypes stype, CyFilterType filterType)
        {
            List<CyWidget> list = m_listWidgetCSHL;
            for (int i = 0; i < list.Count; i++)
            {
                CyWidget wi = list[i];
                switch (stype)
                {
                    case CyWidgetAPITypes.SliderLinear:
                    case CyWidgetAPITypes.SliderRadial:
                    case CyWidgetAPITypes.SliderLinearDiplexed:
                        if (wi is CySlider == false) continue;
                        CySlider sl = wi as CySlider;

                        if ((stype == CyWidgetAPITypes.SliderLinearDiplexed && sl.m_diplexing == false)) continue;
                        if ((stype == CyWidgetAPITypes.SliderLinear && sl.m_diplexing)) continue;
                        if ((stype == CyWidgetAPITypes.SliderRadial && sl.m_type == CySensorType.SliderLinear))
                            continue;
                        if ((stype == CyWidgetAPITypes.SliderLinear && sl.m_type == CySensorType.SliderRadial))
                            continue;
                        break;
                    case CyWidgetAPITypes.Touchpad:
                        if (wi is CyTouchPad == false) continue;
                        break;
                }
                if (IsFilterForWidget(wi, filterType))
                    return true;
            }
            return false;
        }
        static bool IsFilterForWidget(CyWidget wi, CyFilterType filterType)
        {
            CyPosFilterOptions filterPropertiesPos = CyPosFilterOptions.None;
            if (wi is CySlider)
                filterPropertiesPos = (wi as CySlider).m_filterPropertiesPos;
            if (wi is CyTouchPad)
                filterPropertiesPos = (wi as CyTouchPad).m_positionFilter;

            switch (filterType)
            {
                case CyFilterType.AnyPos:
                    return filterPropertiesPos != CyPosFilterOptions.None;
                case CyFilterType.JitterFilterPos:
                    return filterPropertiesPos == CyPosFilterOptions.Jitter;
                case CyFilterType.MedianFilterPos:
                    return filterPropertiesPos == CyPosFilterOptions.Median;
                case CyFilterType.FirstOrderIIRFilter0_5Pos:
                    return filterPropertiesPos == CyPosFilterOptions.FirstOrderIIR0_5;
                case CyFilterType.FirstOrderIIRFilter0_75Pos:
                    return filterPropertiesPos == CyPosFilterOptions.FirstOrderIIR0_75;
                case CyFilterType.AveragingFilterPos:
                    return filterPropertiesPos == CyPosFilterOptions.Averaging;
                default:
                    break;
            }
            return false;
        }
        int GetPosFiltersMask()
        {
            List<CyWidget> list = m_listWidgetCSHL;
            int resMask = 0;
            int mask = 0;
            for (int i = 0; i < list.Count; i++)
            {
                if (list[i] is CySlider) 
                    mask = (int)(list[i] as CySlider).m_filterPropertiesPos;
                if (list[i] is CyTouchPad && CyCsConst.IsMainPartOfWidget(list[i].m_type))
                    mask = (int)(list[i] as CyTouchPad).m_positionFilter;

                resMask |= mask;
            }
            return resMask;
        }
        #endregion

    }
}