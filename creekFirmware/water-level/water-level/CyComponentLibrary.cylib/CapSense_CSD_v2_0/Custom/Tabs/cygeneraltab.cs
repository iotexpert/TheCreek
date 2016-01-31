/*******************************************************************************
* Copyright 2008-2010, Cypress Semiconductor Corporation.  All rights reserved.
* You may use this file only in accordance with the license, terms, conditions,
* disclaimers, and limitations in the end user license agreement accompanying
* the software package with which this file was provided.
********************************************************************************/



using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Text;
using System.Windows.Forms;
using System.IO;
using System.Text.RegularExpressions;
using System.Xml.Serialization;
using CyDesigner.Extensions.Gde;

namespace CapSense_CSD_v2_0
{
    public partial class CyGeneralTab : CyCSParamEditTemplate
    {
        public override string TabName
        {
            get { return "General"; }
        }
        bool m_loading = false;
        Regex m_objNumberPattern;
        List<CyClkItem> m_listStClk = new List<CyClkItem>();
        CyClkItem m_customClock;
        CyClkItem m_busClock;

        #region Load
        public CyGeneralTab(CyCSParameters packParams)
            : base()
        {
            InitializeComponent();
            //Initialization PackParams
            this.m_packParams = packParams;

            cbRawDataNoiseFilter.Items.Clear();
            cbRawDataNoiseFilter.Items.AddRange(CyCsEnumConverter.GetDescriptionList(typeof(CyRawDataFilterOptions)));

            cbTuningMethod.Items.Clear();
            cbTuningMethod.Items.AddRange(CyCsEnumConverter.GetDescriptionList(typeof(CyTuningMethodOptions)));

            cbNumberChannels.Items.Clear();
            cbNumberChannels.Items.AddRange(CyCsEnumConverter.GetDescriptionList(typeof(CyChannelConfig)));

            cbEnableClkInput_CheckedChanged(null, null);
            CyCSParameters.AssingActions(this, new EventHandler(SendProperties));
            packParams.m_updateAll += new EventHandler(GetProperties);

            m_customClock = new CyClkItem(CyClkItem.CUSTOM_CLOCK_NAME, 12);
            m_busClock = new CyClkItem(CyClkItem.BUS_CLOCK_NAME, -1);
            m_listStClk.Add(new CyClkItem(3));
            m_listStClk.Add(new CyClkItem(6));
            m_listStClk.Add(new CyClkItem(12));
            m_listStClk.Add(new CyClkItem(24));

            //Pattern implementation
            string ds = System.Globalization.NumberFormatInfo.CurrentInfo.CurrencyDecimalSeparator;
            string temp = " ";
            string frResPattern = "[" + CyClkItem.FREQ_RESOLUTION + "]";
            for (int i = 0; i < 2; i++)
            {
                frResPattern += "|" + "[" + temp + CyClkItem.FREQ_RESOLUTION + "]";
                temp += " ";
            }
            String strValidRealPattern = "(^[0-9]*[" + ds + "]*[0-9]*(" + frResPattern + ")+$)";
            String strValidIntegerPattern = "(^[0-9]*(" + frResPattern + ")+$)";
            m_objNumberPattern = new Regex("(" + strValidRealPattern + ")|(" + strValidIntegerPattern + ")"
                , RegexOptions.IgnoreCase | RegexOptions.IgnorePatternWhitespace);

            cbScanClock.Validated += new EventHandler(SendProperties);
            m_packParams.m_settings.m_configurationChanged += new EventHandler(cbScanClock_TextChanged);
        }
        #endregion

        #region Functions
        void SendProperties(object sender, EventArgs e)
        {
            if (m_packParams == null || CyCSParameters.GLOBAL_EDIT_MODE == false || m_loading) return;
            CyCSSettings settings = m_packParams.m_settings;            

            m_loading = true;

            CyTuningMethodOptions tun_m = (CyTuningMethodOptions)CyCsEnumConverter.GetValue(cbTuningMethod.Text,
                         typeof(CyTuningMethodOptions));
            if (settings.TuningMethod != tun_m)
            {
                settings.TuningMethod = tun_m;
                if (tun_m == CyTuningMethodOptions.Tuning_Auto)
                {
                    cbEnableClkInput.Checked = false;
                    m_customClock.m_actualFreq = CyCsConst.C_AUTO_TUNING_ALLOWED_CLOCK_VALUE;
                    cbScanClock.Text = m_customClock.ToString();
                }
            }

            //If Error commit nothing
            if (errorProvider.GetError(cbScanClock) != string.Empty)
            {
                m_loading = false;
                return;
            }

            settings.m_clockType = cbEnableClkInput.Checked ? CyClockSourceOptions.External :
                CyClockSourceOptions.Internal;
            settings.m_rawDataFilterType = (CyRawDataFilterOptions)CyCsEnumConverter
                .GetValue(cbRawDataNoiseFilter.Text, typeof(CyRawDataFilterOptions));
            settings.Configuration = (CyChannelConfig)CyCsEnumConverter.GetValue(cbNumberChannels.Text,
                typeof(CyChannelConfig));

            if (settings.m_clockType != CyClockSourceOptions.External && BusClkSelected())
                settings.m_clockType = CyClockSourceOptions.BusClk;
            else
                settings.m_clockFr = m_customClock.m_actualFreq;


            if (settings.m_waterProofing == false && cbWaterProofing.Checked)
                m_packParams.EnableWaterProofing();
            else
                settings.m_waterProofing = cbWaterProofing.Checked;
            m_packParams.SetCommitParams(null, null);
            UpdateTuningAutoLimitations();
            m_loading = false;
        }

        private void UpdateTuningAutoLimitations()
        {
            if (m_packParams != null)
            {
                CyCSSettings settings = m_packParams.m_settings;
                cbEnableClkInput.Enabled = settings.m_tuningMethod != CyTuningMethodOptions.Tuning_Auto;

                cbScanClock.Enabled = settings.m_clockType != CyClockSourceOptions.External;
                cbScanClock.DropDownStyle = settings.m_tuningMethod != CyTuningMethodOptions.Tuning_Auto
                    ? ComboBoxStyle.DropDown : ComboBoxStyle.DropDownList;
            }
        }
        public void GetProperties(object sender, EventArgs e)
        {
            if (m_packParams == null) return;
            m_loading = true;
            CyCSSettings settings = m_packParams.m_settings;
            CyCsEnumConverter.SetValue(cbNumberChannels, CyCsEnumConverter.GetDescription(settings.Configuration));
            cbEnableClkInput.Checked = settings.m_clockType == CyClockSourceOptions.External;
            cbWaterProofing.Checked = settings.m_waterProofing;
            CyCsEnumConverter.SetValue(cbTuningMethod, CyCsEnumConverter.GetDescription(settings.m_tuningMethod));
            CyCsEnumConverter.SetValue(cbRawDataNoiseFilter, CyCsEnumConverter.GetDescription(
                settings.m_rawDataFilterType));

            #region Clock Selection
            cbScanClock.Items.Clear();
            for (int i = 0; i < m_listStClk.Count; i++)
                cbScanClock.Items.Add(m_listStClk[i]);
            if (m_packParams.m_edit != null)
                UpdateClock(m_packParams.m_edit.InstQuery, null);

            //Chose current clok
            if (m_busClock.m_clockID != string.Empty && settings.m_clockType == CyClockSourceOptions.BusClk)
                cbScanClock.SelectedItem = m_busClock;
            else
            {
                m_customClock.m_actualFreq = settings.m_clockFr;
                cbScanClock.SelectedIndex = -1;
                cbScanClock.Text = m_customClock.ToString();
            }
            #endregion

            UpdateTuningAutoLimitations();
            m_loading = false;
        }
        public void UpdateClock(ICyInstQuery_v1 query, ICyTerminalQuery_v1 termQuery)
        {
            if (m_packParams != null)
            {
                CyClkItem busClk = CyCSParameters.GetBusClockInMHz(query);
                if (busClk != null)
                {
                    m_busClock.GetProperties(busClk);
                    if (cbScanClock.Items.IndexOf(m_busClock) == -1)
                        cbScanClock.Items.Add(m_busClock);
                }
            }
        }
        #endregion

        private void cbEnableClkInput_CheckedChanged(object sender, EventArgs e)
        {
            lScanClock.Enabled = !cbEnableClkInput.Checked;
            cbScanClock.Enabled = !cbEnableClkInput.Checked;
        }

        private void toolStripButtonSave_Click(object sender, EventArgs e)
        {
            string content = m_packParams.Serialize(m_packParams);
            CyCSParameters.SaveToFile(content, CyCsConst.P_SAVED_GENERAL_FILTER);
        }

        private void toolStripButtonLoad_Click(object sender, EventArgs e)
        {
            string content;
            if (CyCSParameters.ReadFromFile(CyCsConst.P_SAVED_GENERAL_FILTER, out content))
            {
                CyCSParameters param = (CyCSParameters)m_packParams.Deserialize(content, typeof(CyCSParameters), true);
                CyCSParameters.GLOBAL_EDIT_MODE = false;
                //Save parameters
                param.m_settings.m_configurationChanged = m_packParams.m_settings.m_configurationChanged;
                m_packParams.m_settings = param.m_settings;
                m_packParams.m_widgets = param.m_widgets;
                m_packParams.ExecuteUpdateAll();
                CyCSParameters.GLOBAL_EDIT_MODE = true;
                m_packParams.SetCommitParams(null, null);
            }
        }

        private void cbScanClock_TextChanged(object sender, EventArgs e)
        {
            CancelEventArgs ex = new CancelEventArgs();
            cbScanClock_Validating(cbScanClock, ex);
        }

        private void cbScanClock_Validating(object sender, CancelEventArgs e)
        {
            if (m_packParams != null)
            {
                CyCSSettings set = m_packParams.m_settings;
                try
                {
                    double fr;
                    if (BusClkSelected())
                        fr = m_busClock.m_actualFreq;
                    else
                    {
                        string input = cbScanClock.Text.ToLower();
                        if (m_objNumberPattern.IsMatch(input))
                        {
                            input = input.Substring(0, input.IndexOf(CyClkItem.FREQ_RESOLUTION.ToLower()));
                        }
                        fr = Convert.ToDouble(input);
                    }
                    if (CyCsConst.C_SCAN_CLOCK.CheckRange(fr) == false)
                        throw new Exception();

                    if (set.m_tuningMethod == CyTuningMethodOptions.Tuning_Auto
                        && fr != CyCsConst.C_AUTO_TUNING_ALLOWED_CLOCK_VALUE)
                    {
                        errorProvider.SetError((Control)sender, CyCsResource.MESSAGE_ERROR_BUS_CLK_FREQUNC_LIMIT_AUTO);
                        return;
                    }
                    if (BusClkSelected() == false)
                    {
                        m_customClock.m_actualFreq = fr;

                        //Check  FF configuration limitation
                        if (m_busClock != null && m_busClock.m_actualFreq <= m_customClock.m_actualFreq)
                            if (set.m_prescaler == CyPrescalerOptions.Prescaler_FF
                                    || set.m_listChannels[0].m_implementation == CyMeasureImplemetation.FF_Based
                                    || (set.Configuration == CyChannelConfig.TWO_CHANNELS
                                       && set.m_listChannels[1].m_implementation == CyMeasureImplemetation.FF_Based))
                            {
                                errorProvider.SetError((Control)sender, CyCsResource.MESSAGE_ERROR_FF_BASE_LIMITATION);
                                return;
                            }
                    }
                    errorProvider.SetError((Control)sender, string.Empty);
                }
                catch
                {
                    errorProvider.SetError((Control)sender, String.Format(CyCsResource.ValueLitation,
                        CyCsConst.C_SCAN_CLOCK.m_min, CyCsConst.C_SCAN_CLOCK.m_max));
                }
            }
        }

        private void cbTuningMethod_Validating(object sender, CancelEventArgs e)
        {
            if (m_packParams.m_settings.m_currentSource == CyCurrentSourceOptions.Idac_None &&
                  cbTuningMethod.SelectedIndex == (int)CyTuningMethodOptions.Tuning_Auto)
            {
                errorProvider.SetError(cbTuningMethod, CyCsResource.AutoTuningLimitation);
                e.Cancel = true;
            }
            else
                errorProvider.SetError(cbTuningMethod, string.Empty);
        }

        private void cbTuningMethod_SelectedIndexChanged(object sender, EventArgs e)
        {
            CancelEventArgs ex = new CancelEventArgs();
            cbTuningMethod_Validating(null, ex);
        }
        bool BusClkSelected()
        {
            return m_busClock != null
                && (cbScanClock.SelectedItem == m_busClock || cbScanClock.Text == m_busClock.ToString());
        }
    }
    [Serializable()]
    public class CyClkItem
    {
        [XmlAttribute("ClockID")]
        public string m_clockID = string.Empty;
        [XmlAttribute("ActualFreq")]
        public double m_actualFreq = 0;
        [XmlAttribute("ClockName")]
        public string m_clockName = string.Empty;

        public const string CUSTOM_CLOCK_NAME = "internalCustomClock";
        public const string BUS_CLOCK_NAME = "BUS_CLK";

        public const string FREQ_RESOLUTION = "MHz";

        public CyClkItem()
        {
        }
        public CyClkItem(double actualFreq)
        {
            m_clockName = CUSTOM_CLOCK_NAME;
            this.m_actualFreq = actualFreq;
        }
        public CyClkItem(string name, double ActualFreq)
            : this(ActualFreq)
        {
            this.m_clockName = name;
        }
        public override string ToString()
        {
            string str = string.Empty;
            if (m_clockName != string.Empty) str = m_clockName + " : ";
            if (m_clockName == CUSTOM_CLOCK_NAME) str = string.Empty;
            return str + m_actualFreq + " " + FREQ_RESOLUTION;
        }
        public void GetProperties(CyClkItem clk)
        {
            m_actualFreq = clk.m_actualFreq;
            m_clockName = clk.m_clockName;
            m_clockID = clk.m_clockID;
        }

        public bool IsSame(CyClkItem item)
        {
            if (item.m_actualFreq != m_actualFreq) return false;
            if (item.m_clockName != m_clockName) return false;
            if (item.m_clockID != m_clockID) return false;
            return true;
        }
        public bool IsDirectClock()
        {
            return m_clockID != string.Empty;
        }
    }
}