/*******************************************************************************
* Copyright 2008-2009, Cypress Semiconductor Corporation.  All rights reserved.
* You may use this file only in accordance with the license, terms, conditions, 
* disclaimers, and limitations in the end user license agreement accompanying 
* the software package with which this file was provided.
********************************************************************************/



using System;
using System.Collections.Generic;
using System.Text;
using System.Xml.Serialization;
using System.ComponentModel;

namespace  CapSense_v1_20
{
    #region cyLocalParams
    [Serializable()]
    public class CyLocalParams
    {
        [XmlIgnore]
        public CyGeneralParams m_Base;

        [XmlElement("configuration")]
        public E_MAIN_CONFIG m_configuration;

        public event EventHandler m_actGlobalParamsChange;

        [XmlArray("ListSides")]
        [XmlArrayItem("CyAmuxBParams")]
        public List<CyAmuxBParams> m_listCsHalfs = new List<CyAmuxBParams>();

        public static string m_Conponent_prefix = "CapSense__";

        #region Functions
        public CyLocalParams()
        {           
        }
        public void FirstInitialszation()
        {
            m_listCsHalfs.Clear();
            m_listCsHalfs.Add(new CyAmuxBParams());
            m_listCsHalfs.Add(new CyAmuxBParams());
            m_listCsHalfs[0].m_side = E_EL_SIDE.Left;
            m_listCsHalfs[1].m_side = E_EL_SIDE.Right;
        }
        public void DoEventGlobalParamsChange()
        {
            m_actGlobalParamsChange(null, null);
        }
        public int GetCountAvailibleSides()
        {
            int res = 0;
            foreach (CyAmuxBParams item in m_listCsHalfs)
            {
                if (IsAmuxBusEnable(item)) res++;
            }
            return res;
        }
        public int GetFirstAvailibleSide()
        {       
            foreach (CyAmuxBParams item in m_listCsHalfs)
            {
                if (IsAmuxBusEnable(item)) return (int)(item.m_side);
            }
            return 0;
        }

        public bool IsAmuxBusEnable(CyAmuxBParams param)
        {
            if (param.m_side == E_EL_SIDE.Right)
            {
                if (m_configuration == E_MAIN_CONFIG.emSerial) return false;
            }
            return param.m_Method != E_CAPSENSE_MODE.None;
        }

        public CyAmuxBParams GetAmuxBusBySide(E_EL_SIDE side)
        {
            if (side == E_EL_SIDE.Left) return m_listCsHalfs[0];
            if (side == E_EL_SIDE.Right) return m_listCsHalfs[1];
            return m_listCsHalfs[0];
        }
        public bool IsIdacInSystem()
        {
            foreach (CyAmuxBParams item in m_listCsHalfs)
                if (IsAmuxBusEnable(item))
                {
                    if (item.IsIdacAvailible()) return true;
                }
            return false;
        }

        public bool IsParallel()
        {
            return m_configuration != E_MAIN_CONFIG.emSerial;
        }
        public bool IsParallelFull()
        {
            return IsParallel() && (GetCountAvailibleSides() == 2);
        }
        #endregion

        #region Service function for parameters commit
        public string GetScanModeOption()
        {
            if (m_configuration == E_MAIN_CONFIG.emSerial) return "Serial" + "_Mode";
            return "Parallel" + "_Mode";
        }
        public void SetSyncScanModeOption(string scanMode, string syncMode)
        {
            RemovePrefix(ref scanMode, m_Conponent_prefix);
            RemovePrefix(ref syncMode, m_Conponent_prefix);

            if (GetScanModeOption() != scanMode)
            {
                switch (m_configuration)
                {
                    case E_MAIN_CONFIG.emSerial:
                        m_configuration = E_MAIN_CONFIG.emParallelAsynchron;//Correct mode will verifide below
                        break;
                    case E_MAIN_CONFIG.emParallelSynchron:
                    case E_MAIN_CONFIG.emParallelAsynchron:
                        m_configuration = E_MAIN_CONFIG.emSerial;
                        break;
                    default:
                        break;
                }
            }
            if (m_configuration != E_MAIN_CONFIG.emSerial)
            {
                if (GetSyncModeOption() != syncMode)
                    //Choose correct parallel mode
                    m_configuration = m_configuration == E_MAIN_CONFIG.emParallelAsynchron ?
                        E_MAIN_CONFIG.emParallelSynchron : E_MAIN_CONFIG.emParallelAsynchron;
            }
        }
        public string GetSyncModeOption()
        {
            switch (m_configuration)
            {
                case E_MAIN_CONFIG.emSerial:
                    break;
                case E_MAIN_CONFIG.emParallelSynchron:
                    return "Sync" + "_Mode";
                case E_MAIN_CONFIG.emParallelAsynchron:
                    return "Async" + "_Mode";
                default:
                    break;
            }
            return "Sync" + "_Mode";
        }
        public static void RemovePrefix(ref string str, string prefix)
        {
            if (str.IndexOf(prefix) > -1)
            {
                str = str.Remove(0, prefix.Length);
            }
        }
        #endregion

    }
    #endregion

    #region CyAmuxBParams
    [Serializable()]
    public class CyAmuxBParams : ICyMyPostSerialization
    {
        [XmlAttribute("Method")]
        public E_CAPSENSE_MODE m_Method = E_CAPSENSE_MODE.CSD;

        [XmlAttribute("Side")]
        public E_EL_SIDE m_side = E_EL_SIDE.Left;

        [XmlElement("cRb")]
        public CyIntElement m_cRb = new CyIntElement(1);

        [XmlElement("cShieldElectrode")]
        public CyIntElement m_cShieldElectrode = new CyIntElement(0);

        //[XmlAttribute("prs")]
        [XmlIgnore()]
        public E_PRS_OPTIONS m_prs = 0;

        [XmlAttribute("csdSubMethod")]
        public E_CSD_SUB_METHODS m_csdSubMethod =  E_CSD_SUB_METHODS.IDACSourcing;

        [XmlAttribute("Prescaler")]
        public E_PRESCALER m_Prescaler = E_PRESCALER.UDB;

        [XmlElement("currectClk")]
        public CyClkItem m_currectClk;

        [XmlElement("Reference")]
        public E_REFERENCE_OPTIONS m_Reference= E_REFERENCE_OPTIONS.Val1_024V;

        [XmlIgnore()]
        public int CountShieldElectrode { get { return m_cShieldElectrode.m_Value; } private set { } }

        [XmlIgnore()]
        public int CountRb { get { return m_cRb.m_Value; } }

        public void ExecutePostSerialization()
        {
            m_cRb.TakeMinMax(1, 3);
        }
        public CyAmuxBParams()
        {
            ExecutePostSerialization();
        }
        public string GetPRSResolution()
        {            
            return ((byte)m_prs).ToString();
        }
        public string GetPRSResolutionOption()
        {
            string res =m_prs.ToString();
            if (m_prs == 0) res = "_" + res;
            return "Prs"+res;
        }
        public void SetPRSResolutionOption(string val)
        {
            CyLocalParams.RemovePrefix(ref val, "Prs_");
            if (val.IndexOf("bits") > 0)
                val = val.Insert(val.IndexOf("bits"), " ");
            m_prs =((E_PRS_OPTIONS)CyIntEnumConverter.GetEnumValue(val, typeof(E_PRS_OPTIONS)));
        }
        public string GetIdacOptions()
        {
            E_IDAC_OPTIONS res = E_IDAC_OPTIONS.None;
            if (m_csdSubMethod == E_CSD_SUB_METHODS.IDACSourcing) res = E_IDAC_OPTIONS.Source;
            if (m_csdSubMethod == E_CSD_SUB_METHODS.IDACSinking) res = E_IDAC_OPTIONS.Sink;

            return "Idac_"+res.ToString();
        }
        public void SetIdacOptions(string val)
        {
            CyLocalParams.RemovePrefix(ref val, "Idac_");
            E_IDAC_OPTIONS opt = (E_IDAC_OPTIONS)Enum.Parse(typeof(E_IDAC_OPTIONS), val);

            switch (opt)
            {
                case E_IDAC_OPTIONS.None:
                    m_csdSubMethod = E_CSD_SUB_METHODS.IDACDisable_RB;
                    break;
                case E_IDAC_OPTIONS.Source:
                    m_csdSubMethod = E_CSD_SUB_METHODS.IDACSourcing;
                    break;
                case E_IDAC_OPTIONS.Sink:
                    m_csdSubMethod = E_CSD_SUB_METHODS.IDACSinking;
                    break;
                default:
                    break;
            }
        }
        public string GetPrescalerOptions()
        {
            return "Prescaler_" + m_Prescaler.ToString();
        }
        public void SetPrescalerOptions(string val)
        {
            CyLocalParams.RemovePrefix(ref val, "Prescaler_");
            m_Prescaler = (E_PRESCALER)Enum.Parse(typeof(E_PRESCALER), val);
        }
        public bool IsPRS()
        {
            return m_prs != 0;
        }
        public bool IsPrescaler()
        {
            return m_Prescaler != E_PRESCALER.None;
        }
        public bool IsShieldElectrode()
        {
            //if (m_Method == E_CAPSENSE_MODE.CSD)
                //if (m_csdSubMethod == E_CSD_SUB_METHODS.IDACSourcing) return false;
            return m_cShieldElectrode.m_Value != 0;
        }
        public bool IsNotShieldElectrode()
        {
            return !IsShieldElectrode();
        }

        public bool IsIdacAvailible()
        {
            if (m_Method == E_CAPSENSE_MODE.CSA) return true;
            else if (m_Method == E_CAPSENSE_MODE.CSD)
            {
                if (m_csdSubMethod != E_CSD_SUB_METHODS.IDACDisable_RB) return true;
            }
            return false;
        }
        public bool IsRbAvailible()
        {
            if (m_Method == E_CAPSENSE_MODE.CSD)
            {
                if (m_csdSubMethod == E_CSD_SUB_METHODS.IDACDisable_RB) return true;
            }
            return false;
        }
    }
    #endregion
}
