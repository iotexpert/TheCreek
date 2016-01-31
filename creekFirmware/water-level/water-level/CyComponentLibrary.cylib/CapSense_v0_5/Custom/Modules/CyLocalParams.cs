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

namespace  CapSense_v0_5
{
    #region cyLocalParams
    public enum eMConfiguration { emSerial=0, emParallelSynchron=1, emParallelAsynchron=2 }
    [Serializable()]
    public class CyLocalParams
    {
        public bool internalProcess = false;
        public eMConfiguration configuration;

        public delegate void m_NullDelegate();
        public event m_NullDelegate actGlobalParamsChange;

        [XmlArray("ListScanSlots")]
        public List<CyAmuxBParams> listCsHalfs = new List<CyAmuxBParams>();

        public int GetCountAvailibleSides()
        {
            int res = 0;
            foreach (CyAmuxBParams item in listCsHalfs)
            {
                if (bCsHalfIsEnable(item)) res++;
            }
            return res;
        }
        public int GetFirstAvailibleSide()
        {       
            foreach (CyAmuxBParams item in listCsHalfs)
            {
                if (bCsHalfIsEnable(item)) return (int)(item.side);
            }
            return 0;
        }
        public CyLocalParams()
        { actGlobalParamsChange += new m_NullDelegate(CyLocalParams_actChangeHalfVisibility); }

        void CyLocalParams_actChangeHalfVisibility()
        {
            //throw new NotImplementedException();
        }
        public void DoEventGlobalParamsChange()
        {
            actGlobalParamsChange();
        }
        public bool bCsHalfIsEnable(CyAmuxBParams param)
        {
            if (param.side == eElSide.Right)
            {
                if (configuration == eMConfiguration.emSerial) return false;
            }
            return param.Method != eCapSenseMode.None;
        }

        public CyAmuxBParams GetAmuxBusBySide(eElSide side)
        {
            if (side == eElSide.Left) return listCsHalfs[0];
            if (side == eElSide.Right) return listCsHalfs[1];
            return listCsHalfs[0];
        }
        public void FirstInitialszation()
        {
            listCsHalfs.Clear();
            listCsHalfs.Add(new CyAmuxBParams());
            listCsHalfs.Add(new CyAmuxBParams());
            listCsHalfs[0].side = eElSide.Left;
            listCsHalfs[1].side = eElSide.Right;
        }
        public bool isIdacInSystem()
        {
            foreach (CyAmuxBParams item in listCsHalfs)
                if (bCsHalfIsEnable(item))
                {
                    if (item.isIdac) return true;
                }
            return false;
        }

        public bool isParallel()
        {
            return configuration != eMConfiguration.emSerial;
        }
        public bool isParallelFull()
        {
            return isParallel() && (GetCountAvailibleSides() == 2);
        }
        //public bool IsShowPrescalerCode(CyAmuxBParams bus)
        //{
        //    if (configuration == eMConfiguration.emParallelSynchron)
        //    {
        //        if (bus.side == eElSide.Right) return false;
        //    }
        //    return bus.Prescaler != enPrescaler.None;
        //}
    }
    #endregion

    #region CyAmuxBParams
    public enum CSDSubMethods
    {
        IDACSourcing,IDACSinking,IDACDisable_RB
    }
    public enum enPrescaler
    {
        UDB,
        HW,
        None
    };
    [Serializable()]
    public class CyAmuxBParams : IMyPostSerialization
    {
        [XmlAttribute("Method")]
        public eCapSenseMode Method = eCapSenseMode.CSD;
        [XmlAttribute("Side")]
        public eElSide side = eElSide.Left;
        [XmlAttribute("isRbEnable")]
        public bool isRbEnable = false;

        [XmlAttribute("countRb")]
        public int countRb
        {
            get { return cRb.Value; }
        }
        public IntElement cRb = new IntElement(1);

        [XmlAttribute("isIdac")]
        public bool isIdac = true;
        [XmlAttribute("countShieldElectrode")]
        public int countShieldElectrode
        {
            get { return cShieldElectrode.Value; }
        }

        public IntElement cShieldElectrode = new IntElement(0);

        [XmlAttribute("cis")]
        public int cis = 0;
        [XmlAttribute("prs")]
        public int prs = 0;
        [XmlAttribute("csdSubMethod")]
        public CSDSubMethods csdSubMethod =  CSDSubMethods.IDACSourcing;

        [XmlAttribute("Prescaler")]
        public enPrescaler Prescaler = enPrescaler.UDB;


        public cyClkItem currectClk;//=new cyClkItem();

        [NonSerialized()]
        [XmlIgnore()]
        public Dictionary<String, int> strPRS = new Dictionary<string, int>();

        public void ExecutePostSerialization()
        {
            cRb.takeMinMax(1, 3);
        }
        public CyAmuxBParams()
        {
            ExecutePostSerialization();
            strPRS.Add("None", 0);
            strPRS.Add("8 bits", 8);
            //strPRS.Add("9 bits",
            //strPRS.Add("10 bits",
            //strPRS.Add("11 bits",
            //strPRS.Add("12 bits",
            //strPRS.Add("13 bits",
            //strPRS.Add("14 bits",
            //strPRS.Add("15 bits",
            strPRS.Add("16 bits", 16);
        }
        public string GetPRSResolution()
        {
            return (new List<int>(strPRS.Values))[prs].ToString();
        }
        public bool IsPRS()
        {
            return prs != 0;
        }
        public bool IsPrescaler()
        {
            return Prescaler != enPrescaler.None;
        }
        public bool IsShieldElectrode()
        {
            return countShieldElectrode != 0;
        }
        public int ShieldElectrodeCount()
        {
            return countShieldElectrode;
        }
        public bool IsWorkAround()
        {
            return true;
        }
        public bool IsIDACSinkingOrRb()
        {
            if (Method != eCapSenseMode.CSD) return false;
            if (csdSubMethod == CSDSubMethods.IDACSinking) return true;
            if (csdSubMethod == CSDSubMethods.IDACDisable_RB) return true;
            return false;
        }

    }
    #endregion
}
