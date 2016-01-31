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

namespace  CapSense_v1_10
{
    #region cyLocalParams
    public enum E_MAIN_CONFIG { emSerial=0, emParallelSynchron=1, emParallelAsynchron=2 }
    [Serializable()]
    public class CyLocalParams
    {
        [XmlIgnore]
        public bool m_internalProcess = false;

        [XmlIgnore]
        public CyGeneralParams m_Base;

        [XmlElement("configuration")]
        public E_MAIN_CONFIG m_configuration;

        public delegate void m_NullDelegate();

        public event m_NullDelegate m_actGlobalParamsChange;

        [XmlArray("ListSides")]
        [XmlArrayItem("CyAmuxBParams")]
        public List<CyAmuxBParams> m_listCsHalfs = new List<CyAmuxBParams>();

        public int GetCountAvailibleSides()
        {
            int res = 0;
            foreach (CyAmuxBParams item in m_listCsHalfs)
            {
                if (BCsHalfIsEnable(item)) res++;
            }
            return res;
        }
        public int GetFirstAvailibleSide()
        {       
            foreach (CyAmuxBParams item in m_listCsHalfs)
            {
                if (BCsHalfIsEnable(item)) return (int)(item.m_side);
            }
            return 0;
        }
        public CyLocalParams()
        { m_actGlobalParamsChange += new m_NullDelegate(CyLocalParams_actChangeHalfVisibility); }

        void CyLocalParams_actChangeHalfVisibility()
        {
            m_Base.SetCommitParams(null, null);
        }
        public void DoEventGlobalParamsChange()
        {
            m_actGlobalParamsChange();
        }
        public bool BCsHalfIsEnable(CyAmuxBParams param)
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
        public void FirstInitialszation()
        {
            m_listCsHalfs.Clear();
            m_listCsHalfs.Add(new CyAmuxBParams());
            m_listCsHalfs.Add(new CyAmuxBParams());
            m_listCsHalfs[0].m_side = E_EL_SIDE.Left;
            m_listCsHalfs[1].m_side = E_EL_SIDE.Right;
        }
        public bool IsIdacInSystem()
        {
            foreach (CyAmuxBParams item in m_listCsHalfs)
                if (BCsHalfIsEnable(item))
                {
                    if (item.m_isIdac) return true;
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
        public string GetScanModeOption()
        {
            if (m_configuration == E_MAIN_CONFIG.emSerial) return "Serial" + "_Mode";
            return "Parallel" + "_Mode";
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

    }
    #endregion

    #region CyAmuxBParams
    public enum E_CSD_SUB_METHODS
    {
        IDACSourcing,IDACSinking,IDACDisable_RB
    }
    public enum E_PRESCALER
    {
        None,
        UDB,
        HW
    };
    public enum E_IDAC_OPTIONS
    {
        None,
        Source,
        Sink
    };
    [Serializable()]
    public class CyAmuxBParams : ICyMyPostSerialization
    {
        [XmlAttribute("Method")]
        public E_CAPSENSE_MODE m_Method = E_CAPSENSE_MODE.CSD;
        [XmlAttribute("Side")]
        public E_EL_SIDE m_side = E_EL_SIDE.Left;
        [XmlAttribute("isRbEnable")]
        public bool m_isRbEnable = false;

        [XmlAttribute("countRb")]
        public int CountRb
        {
            get { return m_cRb.m_Value; }
        }
        [XmlElement("cRb")]
        public CyIntElement m_cRb = new CyIntElement(1);

        [XmlAttribute("isIdac")]
        public bool m_isIdac = true;
 
        [XmlElement("cShieldElectrode")]
        public CyIntElement m_cShieldElectrode = new CyIntElement(0);

        [XmlAttribute("cis")]
        public int m_cis = 0;
        [XmlAttribute("prs")]
        public int m_prs = 0;
        [XmlAttribute("csdSubMethod")]
        public E_CSD_SUB_METHODS m_csdSubMethod =  E_CSD_SUB_METHODS.IDACSourcing;

        [XmlAttribute("Prescaler")]
        public E_PRESCALER m_Prescaler = E_PRESCALER.UDB;

        [XmlElement("currectClk")]
        public CyClkItem m_currectClk;

        [NonSerialized()]
        [XmlIgnore()]
        public Dictionary<String, int> m_strPRS = new Dictionary<string, int>();

        [XmlIgnore()]
        public int CountShieldElectrode { get { return m_cShieldElectrode.m_Value; } private set { } }
        public void ExecutePostSerialization()
        {
            m_cRb.TakeMinMax(1, 3);
        }
        public CyAmuxBParams()
        {
            ExecutePostSerialization();
            m_strPRS.Add("None", 0);
            m_strPRS.Add("8 bits", 8);
            //strPRS.Add("9 bits",
            //strPRS.Add("10 bits",
            //strPRS.Add("11 bits",
            //strPRS.Add("12 bits",
            //strPRS.Add("13 bits",
            //strPRS.Add("14 bits",
            //strPRS.Add("15 bits",
            m_strPRS.Add("16 bits", 16);
        }
        public string GetPRSResolution()
        {
            string res = (new List<int>(m_strPRS.Values))[m_prs].ToString();
            return res;
        }
        public string GetPRSResolutionOption()
        {
            string res = (new List<string>(m_strPRS.Keys))[m_prs].ToString();
            while (res.IndexOf(' ') > 0)
            {
                res = res.Remove(res.IndexOf(' '), 1);
            }
            return "Prs_"+res;
        }
        public string GetIdacOptions()
        {
            E_IDAC_OPTIONS res = E_IDAC_OPTIONS.None;
            if (m_csdSubMethod == E_CSD_SUB_METHODS.IDACSourcing) res = E_IDAC_OPTIONS.Source;
            if (m_csdSubMethod == E_CSD_SUB_METHODS.IDACSinking) res = E_IDAC_OPTIONS.Sink;

            return "Idac_"+res.ToString();
        }
        public string GetPrescalerOptions()
        {
            return "Prescaler_" + m_Prescaler.ToString();
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
            if (m_Method == E_CAPSENSE_MODE.CSD)
                if (m_csdSubMethod == E_CSD_SUB_METHODS.IDACSourcing) return false;
            return m_cShieldElectrode.m_Value != 0;
        }
        public bool IsNotShieldElectrode()
        {
            return !IsShieldElectrode();
        }
        public bool IsWorkAround()
        {
            return true;
        }
        public bool IsIDACSinkingOrRb()
        {
            if (m_Method != E_CAPSENSE_MODE.CSD) return false;
            if (m_csdSubMethod == E_CSD_SUB_METHODS.IDACSinking) return true;
            if (m_csdSubMethod == E_CSD_SUB_METHODS.IDACDisable_RB) return true;
            return false;
        }

    }
    #endregion
}
