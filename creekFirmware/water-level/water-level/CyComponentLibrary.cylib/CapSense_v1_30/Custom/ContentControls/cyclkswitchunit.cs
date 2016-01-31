/*******************************************************************************
* Copyright 2008-2009, Cypress Semiconductor Corporation.  All rights reserved.
* You may use this file only in accordance with the license, terms, conditions,
* disclaimers, and limitations in the end user license agreement accompanying
* the software package with which this file was provided.
********************************************************************************/



﻿using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Data;
using System.Text;
using System.Windows.Forms;
using System.Xml.Serialization;
using System.Text.RegularExpressions;

namespace CapSense_v1_30
{
    public partial class CyClkSwitchUnit : UserControl
    {
        bool m_Loading = false;
        CyAmuxBParams m_currentCSHalf = null;
        List<CyClkItem> m_listStClk = new List<CyClkItem>();
        CyClkItem m_CustomClock;
        CyGeneralParams m_PackParam = null;
        Regex objNumberPattern;

        public CyClkSwitchUnit()
        {
            m_Loading = true;
            InitializeComponent();
            m_CustomClock = new CyClkItem(CyClkItem.CustomClockName, 12);
            cbPrescaler.Items.Clear();
            cbPrescaler.Items.AddRange(Enum.GetNames(typeof(E_PRESCALER)));
            cbPrescaler.SelectedIndex = 0;
            m_listStClk.Add(new CyClkItem(12));
            m_listStClk.Add(new CyClkItem(24));
            m_listStClk.Add(new CyClkItem(48));
            m_listStClk.Add(new CyClkItem(92));

            //Pattern implementation
            string ds = System.Globalization.NumberFormatInfo.CurrentInfo.CurrencyDecimalSeparator;
            string temp = " ";
            string frResPattern = "[" + CyClkItem.FreqResolution + "]";
            for (int i = 0; i < 2; i++)
            {
                frResPattern += "|" + "[" + temp + CyClkItem.FreqResolution + "]";
                temp += " ";
            }
            String strValidRealPattern = "(^[0-9]*[" + ds + "]*[0-9]*(" + frResPattern + ")+$)";
            String strValidIntegerPattern = "(^[0-9]*(" + frResPattern + ")+$)";
            objNumberPattern = new Regex("(" + strValidRealPattern + ")|(" + strValidIntegerPattern + ")"
                , RegexOptions.IgnoreCase | RegexOptions.IgnorePatternWhitespace);

            cbCPS_CLK.SelectedIndexChanged += new EventHandler(SendProperties);
            cbCPS_CLK.Validated += new EventHandler(SendProperties);
            cbCPS_CLK.Validating += new CancelEventHandler(cbCPS_CLK_Validating);
            this.Load += new EventHandler(cntClkSwitch_Load);
            m_Loading = false;
        }

        void cntClkSwitch_Load(object sender, EventArgs e)
        {
            SetImage();
        }

        public void SendProperties(object sender, EventArgs e)
        {
            if (!m_Loading)
            {
                if (m_currentCSHalf != null)
                {
                    m_currentCSHalf.m_Prescaler = (E_PRESCALER)Enum.Parse
                        (typeof(E_PRESCALER), CyIntConverter.CyGetValue(cbPrescaler));

                    if (cbCPS_CLK.SelectedIndex > -1)
                        m_currentCSHalf.m_currectClk = (CyClkItem)CyIntConverter.CyGetObject(cbCPS_CLK);
                    else
                    {
                        m_currentCSHalf.m_currectClk = m_CustomClock;
                    }

                    CyClkItem busClk = GetBusClk(m_PackParam);
                    if (busClk != null)
                    {
                        if (m_currentCSHalf.m_currectClk.m_ActualFreq > busClk.m_ActualFreq)
                        {
                            m_currentCSHalf.m_currectClk = busClk;
                            CyIntConverter.CySetValue(cbCPS_CLK, busClk.ToString());
                            MessageBox.Show(CyIntMessages.STR_CLK_COMPARER, "Warning",
                                MessageBoxButtons.OK, MessageBoxIcon.Warning);
                        }
                    }
                    m_PackParam.SetCommitParams(null, null);
                }
            }
        }

        public void GetProperties(CyAmuxBParams cCSHalf, CyGeneralParams packParam)
        {
            m_Loading = true;
            m_currentCSHalf = cCSHalf;
            this.m_PackParam = packParam;
            cbCPS_CLK.Items.Clear();

            CyClkItem busClk = GetBusClk(packParam);
            if (busClk != null)
                cbCPS_CLK.Items.Add(busClk);
            foreach (CyClkItem item in m_listStClk)
                if ((busClk==null)||(item.m_ActualFreq <= busClk.m_ActualFreq))
                {
                    cbCPS_CLK.Items.Add(item);
                }
            cbCPS_CLK.SelectedIndex = 0;

            //Chose current clok
            if (cCSHalf.m_currectClk != null)
            {
                foreach (object item in cbCPS_CLK.Items)
                    if (((CyClkItem)item).IsSame(cCSHalf.m_currectClk))
                    {
                        CyIntConverter.CySetValue(cbCPS_CLK, item.ToString());
                        break;
                    }
                CyIntConverter.CySetValue(cbPrescaler, m_currentCSHalf.m_Prescaler.ToString());
                if (cCSHalf.m_currectClk.m_ClockName == CyClkItem.CustomClockName)
                    if ((busClk == null) || (cCSHalf.m_currectClk.m_ActualFreq <= busClk.m_ActualFreq))
                    {
                        cbCPS_CLK.SelectedIndex = -1;
                        cbCPS_CLK.Text = cCSHalf.m_currectClk.ToString();
                        m_CustomClock = cCSHalf.m_currectClk;
                    }
            }
            m_Loading = false;

            //For correct settings
            SendProperties(null, null);
            SetImage();
        }

        private CyClkItem GetBusClk(CyGeneralParams packParam)
        {
            if (packParam.m_edit != null)
            {
                List<string> listClkStr;
                if (packParam.m_edit.DesignQuery != null)
                    listClkStr = new List<string>(packParam.m_edit.DesignQuery.ClockIDs);
                else
                    listClkStr = new List<string>();
                foreach (string item in listClkStr)
                {
                    if (packParam.m_edit.DesignQuery.IsClockLocal(item)) continue;
                    CyClkItem clkItem = new CyClkItem();
                    byte exp = 0;
                    clkItem.m_ClockID = item;
                    clkItem.m_ClockName = packParam.m_edit.DesignQuery.GetClockName(item);
                    packParam.m_edit.DesignQuery.GetClockActualFreq(item, out clkItem.m_ActualFreq, out exp);
                    if ((clkItem.m_ClockName == "BUS_CLK"))
                        return clkItem;
                }
            }
            return null;
        }

        private void cbPrescaler_SelectedIndexChanged(object sender, EventArgs e)
        {
            SendProperties(null, null);
            //Prescaler Property( for ScanSlot) Visible Change
            if (!m_Loading)
            {
                if (m_PackParam != null)
                    m_PackParam.m_localParams.DoEventGlobalParamsChange();
                SetImage();
            }
        }

        void cbCPS_CLK_Validating(object sender, CancelEventArgs e)
        {
            if (cbCPS_CLK.SelectedIndex == -1)
            {
                string input = cbCPS_CLK.Text.ToLower();
                try
                {
                    if (objNumberPattern.IsMatch(input))
                    {
                        input = input.Substring(0, input.IndexOf(CyClkItem.FreqResolution.ToLower()));
                    }
                    double fr = Convert.ToDouble(input);
                    if (fr < 1) throw new Exception();
                    if (fr > 100) throw new Exception();
                    m_CustomClock.m_ActualFreq = fr;
                    cbCPS_CLK.Text = m_CustomClock.ToString();
                }
                catch
                {
                    MessageBox.Show("Please input correct Value!", "Warning", MessageBoxButtons.OK,
                        MessageBoxIcon.Warning);
                    cbCPS_CLK.SelectedItem = m_listStClk[0];
                }
            }
        }

        public void SetImage()
        {
            string strStart = "";
            string strMethodPrefix = "";
            if (m_currentCSHalf != null)
            {
                strStart = m_PackParam.GetSidePrefix(m_currentCSHalf);
                strMethodPrefix = m_PackParam.GetMethodPrefixForImage(m_currentCSHalf);
                if (m_PackParam.Configuration == E_MAIN_CONFIG.emParallelSynchron)
                {
                    strStart = "l";
                }
                bool vis = m_currentCSHalf.m_side == E_EL_SIDE.Left;
                vis = vis || m_PackParam.Configuration != E_MAIN_CONFIG.emParallelSynchron;
                lCPS_CLK.Visible = vis;
                cbCPS_CLK.Visible = vis;
                switch (m_currentCSHalf.m_Prescaler)
                {
                    case E_PRESCALER.UDB:
                        pbGraph.Image = ((System.Drawing.Image)(
                            global:: CapSense_v1_30.Properties.Resources.ResourceManager.
                        GetObject(strStart + "prescalerudb" + strMethodPrefix)));
                        break;
                    case E_PRESCALER.HW:
                        pbGraph.Image = ((System.Drawing.Image)(
                            global:: CapSense_v1_30.Properties.Resources.ResourceManager.
                        GetObject(strStart + "prescalerhw" + strMethodPrefix)));
                        break;
                    case E_PRESCALER.None:
                        pbGraph.Image = ((System.Drawing.Image)(
                            global:: CapSense_v1_30.Properties.Resources.ResourceManager.
                        GetObject(strStart + "prescalernone" + strMethodPrefix)));
                        break;
                    default:
                        break;
                }
            }
        }
    }

    [Serializable()]
    public class CyClkItem
    {
        [XmlAttribute("ClockID")]
        public string m_ClockID = "";
        [XmlAttribute("ActualFreq")]
        public double m_ActualFreq = 0;
        [XmlAttribute("ClockName")]
        public string m_ClockName = "";
        [XmlIgnore]
        public const string CustomClockName = "internalCustomClock";
        public const string FreqResolution = "MHz";

        public CyClkItem()
        {
        }
        public CyClkItem(double ActualFreq)
        {
            m_ClockName = CustomClockName;
            this.m_ActualFreq = ActualFreq;
        }
        public CyClkItem(string name, double ActualFreq)
            : this(ActualFreq)
        {
            this.m_ClockName = name;
        }
        public override string ToString()
        {
            string str = "";
            if (m_ClockName != "") str = m_ClockName + " : ";
            if (m_ClockName == CustomClockName) str = "";
            return str + m_ActualFreq + " " + FreqResolution;
        }
        public bool IsSame(CyClkItem item)
        {
            if (item.m_ActualFreq != m_ActualFreq) return false;
            if (item.m_ClockName != m_ClockName) return false;
            if (item.m_ClockID != m_ClockID) return false;
            return true;
        }
        public bool IsDirectClock()
        {
            return m_ClockID != "";
        }
    }
}
