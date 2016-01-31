/*******************************************************************************
* Copyright 2008-2010, Cypress Semiconductor Corporation.  All rights reserved.
* You may use this file only in accordance with the license, terms, conditions,
* disclaimers, and limitations in the end user license agreement accompanying
* the software package with which this file was provided.
********************************************************************************/



using System;
using System.Collections;
using System.Collections.Generic;
using System.Text;
using System.Windows.Forms;
using CyDesigner.Extensions.Common;
using CyDesigner.Extensions.Gde;
using System.Xml.Serialization;
using System.IO;
using System.Drawing;
using System.Runtime.Serialization.Formatters.Binary;
using System.Reflection;
using System.ComponentModel;
using System.Diagnostics;


namespace CapSense_CSD_v2_0
{
    #region CyCSParameters
    [XmlRootAttribute("CyCSParameters")]
    [Serializable()]
    public class CyCSParameters
    {
        #region Head
        private System.Xml.Serialization.XmlSerializer m_widgets_serializer;

        static int MAX_CAPSENSE_PIN_COUNT = 61;

        [NonSerialized()]
        [XmlIgnore()]
        public CyCSInstParameters m_edit;

        [NonSerialized()]
        [XmlIgnore()]
        public ICyTerminalQuery_v1 m_termQuery;

        public static bool GLOBAL_EDIT_MODE = false;

        [XmlElement("Widgets")]
        public CyWidgetsList m_widgets;

        [XmlElement("Settings")]
        public CyCSSettings m_settings;

        [XmlIgnore()]
        public EventHandler m_updateAll;
        [XmlIgnore()]
        public CyAdvancedTab m_advancedTab;
        [XmlIgnore()]
        public Tabs.CyWidgetsTab m_widgetsTab;

        public void ExecuteUpdateAll()
        {
            if (m_updateAll != null && m_updateAll.GetInvocationList().Length > 0)
                m_updateAll(null, null);
        }

        public CyCSParameters()
        {
            m_widgets = new CyWidgetsList();
            m_settings = new CyCSSettings();
            m_widgets_serializer = new XmlSerializer(typeof(CyWidgetsList));
            m_settings.FirstInitialszation();            
        }

        #endregion                

        #region XML Serialization
        public string Serialize(object obj)
        {
            System.Xml.Serialization.XmlSerializer x = null;
            if (obj is CyWidgetsList)
                x = m_widgets_serializer;
            else
                x = new System.Xml.Serialization.XmlSerializer(obj.GetType());

            //Custom namespaces for serialization
            XmlSerializerNamespaces customNamespace = new XmlSerializerNamespaces();
            customNamespace.Add("version", "v1.00");

            StringWriter tw = new StringWriter();
            x.Serialize(tw, obj, customNamespace);
            return tw.ToString();
        }

        public object Deserialize(string serializedXml, Type _type, bool showError)
        {
            object res = Activator.CreateInstance(_type);
            if (String.IsNullOrEmpty(serializedXml))
            {
                CyClassPropsComparer.PostSerializationSearch(res);
                return res;
            }
            try
            {

                System.Xml.Serialization.XmlSerializer x = null;
                if (_type == typeof(CyWidgetsList))
                    x = m_widgets_serializer;
                else
                    x = new System.Xml.Serialization.XmlSerializer(_type);

                TextReader tr = new StringReader(serializedXml);
                res = x.Deserialize(tr);
            }
            catch
            {
                if (showError)
                    MessageBox.Show(CyCsResource.XmlError,CyCsResource.WarningText, 
                        MessageBoxButtons.OK, MessageBoxIcon.Warning);
                res = Activator.CreateInstance(_type);
            }
            finally
            {
                CyClassPropsComparer.PostSerializationSearch(res);
            }
            return res;
        }
        public static void SaveToFile(string content,string filter)
        {
            SaveFileDialog filedialog = new SaveFileDialog();
            filedialog.Filter = filter;
            filedialog.AddExtension = true;
            if (filedialog.ShowDialog() == DialogResult.OK)
            {
                string _file = filedialog.FileName;
                // create a writer and open the file
                TextWriter tw = new StreamWriter(_file);
                // write a line of text to the file
                tw.WriteLine(content);

                // close the stream
                tw.Close();
            }
        }
        public static bool ReadFromFile(string filter, out string content)
        {
            content = "";
            OpenFileDialog filedialog = new OpenFileDialog();
            filedialog.Filter = filter;
            filedialog.AddExtension = true;
            if (filedialog.ShowDialog() == DialogResult.OK)
            {
                try
                {
                    // create reader & open file
                    TextReader tr = new StreamReader(filedialog.FileName);

                    content = tr.ReadToEnd();

                    // close the stream
                    tr.Close();
                    return true;
                }
                catch (Exception ex)
                {
                    MessageBox.Show(ex.ToString(), CyCsResource.FileError, MessageBoxButtons.OK, MessageBoxIcon.Error);
                    return false;
                }
            }
            return false;
        }
        #endregion

        #region Get/Set params
        public void GetParams(CyCSInstParameters inst)
        {
            if (inst != null)
            {
                //Get XML Data
                string xmlData = inst.GetCommittedParam(CyCsConst.P_WIDGETS_DATA).Value;
                m_widgets = (CyWidgetsList)Deserialize(xmlData, typeof(CyWidgetsList), true);

                int wt;
                if (int.TryParse(inst.GetCommittedParam("MaximumSensors").Value, out wt))
                    MAX_CAPSENSE_PIN_COUNT = wt;
                else Debug.Assert(false);

                m_settings.GetParams(inst, null);
            }
        }

        public void SetParams(CyCSInstParameters inst)
        {
            if (inst != null)
            {
                //Insert Widgets data
                string strXML = Serialize(m_widgets).Replace("\r\n", " "); ;
                inst.SetParamExpr(CyCsConst.P_WIDGETS_DATA, strXML);

                m_settings.SetParams(inst);

                bool one_channel = m_settings.Configuration == CyChannelConfig.ONE_CHANNEL;
                List<CyTerminal> listTerminals = m_widgets.GetListTerminalsSortedForAmux(m_settings);

                for (int k = 0; k < m_settings.m_listChannels.Count; k++)
                {
                    string pinInfo = "";
                    int term_count = 0;
                    CyChannelProperties part = m_settings.m_listChannels[k];
                    //Set Alias
                    for (int i = 0; i < listTerminals.Count; i++)
                        if (i < MAX_CAPSENSE_PIN_COUNT)
                            if (one_channel || listTerminals[i].m_widget.m_channel == part.m_channel)
                            {
                                pinInfo += String.Format(CyCsConst.PATTERN_PIN_LIST, term_count,
                                    listTerminals[i].ToString());
                                term_count++;
                            }
                    pinInfo = string.Format(CyCsConst.PATTERN_PORT_GENERAL, "PinAliases", pinInfo);
                    if (term_count > 0)
                    {
                        inst.SetParamExpr("SnsAlias" + part.GetSufix(), pinInfo);
                    }

                    //Set Terminal Count   
                    inst.SetParamExpr(CyCsConst.P_SENSOR_NUMBER + part.GetSufix(), term_count.ToString());

                    //Finish work if one channel
                    if (one_channel) break;
                }
            }
        }
        public void SetCommitParams(object sender, EventArgs e)
        {
            if (m_edit != null)
                if (GLOBAL_EDIT_MODE)
                {
                    GLOBAL_EDIT_MODE = false;
                    SetParams(m_edit);
                    m_edit.CommitParamExprs();
                    GLOBAL_EDIT_MODE = true;
                }
        }
        public void EnableWaterProofing()
        {
            m_settings.m_waterProofing = true;
            m_settings.m_shieldEnable = true;
            GuardSensorEnableChange(true);
            m_advancedTab.GetProperties(null, null);
        }
        public void GuardSensorEnableChange(bool enable)
        {
            m_settings.m_guardSensorEnable = enable;
            m_widgetsTab.GuardSensorVisibleChange(enable);
            m_settings.ConfigurationChanged();
        }
        #endregion

        #region Service Functions
        public static void AssingActions(Control cnt, EventHandler handler)
        {
            for (int i = 0; i < cnt.Controls.Count; i++)
            {
                Control item = cnt.Controls[i];
                if ((item.GetType() == typeof(TextBox)))
                {
                    item.Validated += handler;
                }
                if (item.GetType() == typeof(ComboBox))
                {
                    ((ComboBox)item).SelectedIndexChanged += handler;
                }
                if (item.GetType() == typeof(RadioButton))
                {
                    ((RadioButton)item).CheckedChanged += handler;
                }
                if (item.GetType() == typeof(CheckBox))
                {
                    ((CheckBox)item).CheckedChanged += handler;
                }
                if (item.GetType() == typeof(GroupBox) || item.GetType() == typeof(Panel))
                {
                    AssingActions(item, handler);
                }
            }
        }
        #endregion

        #region Clock Settings
        public double GetClockFrequency()
        {
            if (m_settings.m_clockType == CyClockSourceOptions.Internal)
            {
                return m_settings.m_clockFr;
            }
            else if (m_settings.m_clockType == CyClockSourceOptions.External)
            {
                //Get Clock Frequency from pin
                return GetClockValueInMHz("clock");
            }
            else if (m_settings.m_clockType == CyClockSourceOptions.BusClk && m_edit != null)
            {
                //Get BUS_CLK frequency
                CyClkItem clk = GetBusClockInMHz(m_edit.InstQuery);
                if (GetBusClockInMHz(m_edit.InstQuery) != null)
                    return clk.m_actualFreq;
                else
                    return -1;
            }
            Debug.Assert(false);
            return m_settings.m_clockFr;
        }
        public static CyClkItem GetBusClockInMHz(ICyInstQuery_v1 edit)
        {
            CyClkItem res = null;
            if (edit != null)
            {
                try
                {
                    List<string> listClkStr = new List<string>();
                    if (edit.DesignQuery != null)
                        listClkStr = new List<string>(edit.DesignQuery.ClockIDs);

                    for (int i = 0; i < listClkStr.Count; i++)
                        if (edit.DesignQuery. GetClockName(listClkStr[i]) == CyClkItem.BUS_CLOCK_NAME)
                        {
                            byte out_b = 0;
                            double clockfr;
                            edit.DesignQuery.GetClockActualFreq(listClkStr[i], out clockfr, out out_b);
                            clockfr = (int)(clockfr * Math.Pow(10, out_b) / 1000000);

                            res = new CyClkItem(CyClkItem.BUS_CLOCK_NAME, clockfr);
                            res.m_clockID = listClkStr[i];
                            return res;
                        }
                }
                catch
                {
                    System.Diagnostics.Debug.Assert(false);
                }
            }
            return null;
        }
        public double GetClockValueInMHz(string clockName)
        {
            List<CyClockData> clkdata = new List<CyClockData>();
            try
            {
                clkdata = m_termQuery.GetClockData(clockName, 0);
                if (clkdata[0].IsFrequencyKnown)
                {
                    double infreq = clkdata[0].Frequency;
                    switch (clkdata[0].Unit)
                    {
                        case CyClockUnit.kHz: infreq = infreq / 1000; break;
                    }
                    return infreq;
                }
            }
            catch {}
            return -1;
        }
        #endregion
    }
    #endregion

    #region CyCSInstParameters
    public class CyCSInstParameters
    { 
        protected ICyInstQuery_v1 m_instQuery = null;

        public CyCSInstParameters() { }
        public CyCSInstParameters(ICyInstQuery_v1 instQuery)
        {
            m_instQuery = instQuery;
        }

        public ICyInstQuery_v1 InstQuery
        { get { return m_instQuery; } }

        #region ICyInstEdit_v1 Members

        public bool CommitParamExprs()
        {
            if (m_instQuery != null && m_instQuery is ICyInstEdit_v1)
                return (m_instQuery as ICyInstEdit_v1).CommitParamExprs();
            return true;
        }

        public virtual bool SetParamExpr(string paramName, string expr)
        {
            if (m_instQuery != null && m_instQuery is ICyInstEdit_v1)
                return (m_instQuery as ICyInstEdit_v1).SetParamExpr(paramName, expr);
            return false;
        }

        #endregion

        #region ICyInstQuery_v1 Members
        public virtual CyCompDevParam GetCommittedParam(string paramName)
        {
            if (m_instQuery != null)
            {
                return m_instQuery.GetCommittedParam(paramName);
            }
            return null;
        }

        public virtual IEnumerable<string> GetParamNames()
        {
            if (m_instQuery != null)
                return m_instQuery.GetParamNames();
            return null;
        }
        #endregion
    }
    #endregion 
}


