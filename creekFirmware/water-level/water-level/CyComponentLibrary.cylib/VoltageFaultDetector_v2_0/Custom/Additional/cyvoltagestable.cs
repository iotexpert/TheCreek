/*******************************************************************************
* Copyright 2012, Cypress Semiconductor Corporation.  All rights reserved.
* You may use this file only in accordance with the license, terms, conditions, 
* disclaimers, and limitations in the end user license agreement accompanying 
* the software package with which this file was provided. 
********************************************************************************/

using System;
using System.Collections.Generic;
using System.Text;
using System.ComponentModel;
using System.Reflection;
using System.Xml.Serialization;

namespace VoltageFaultDetector_v2_0
{
    public interface ICyTable
    {
        void InitializeTable(int count);
    }

    [Serializable]
    public class CyVoltagesTableRow
    {
        private const string PREFIX = "v";
        public const int COL_COUNT = 6;

        public string m_voltageName;
        public double? m_nominalVoltage;
        public double? m_uvFaultThreshold;
        public double? m_ovFaultThreshold;
        public double? m_inputScalingFactor;

        public CyVoltagesTableRow() 
        {
            this.m_voltageName = string.Empty;
        }

        public static CyVoltagesTableRow CreateDefaultRow(int index)
        {
            CyVoltagesTableRow row = new CyVoltagesTableRow();
            row.m_voltageName = "";
            row.m_nominalVoltage = null;
            row.m_uvFaultThreshold = null;
            row.m_ovFaultThreshold = null;
            row.m_inputScalingFactor = 1;

            return row;
        }

        public static string GetVoltageIndexStr(int i)
        {
            return PREFIX + i.ToString();
        }
    }

    [Serializable]
    public class CyVoltagesTable : ICyTable
    {
        public List<CyVoltagesTableRow> m_voltagesTable;

        public CyVoltagesTable() { }

        public void InitializeTable(int count)
        {
            m_voltagesTable = new List<CyVoltagesTableRow>();
            for (int i = 0; i < count; i++)
            {
                m_voltagesTable.Add(CyVoltagesTableRow.CreateDefaultRow(i+1));
            }
            for (int i = 0; i < m_voltagesTable.Count; i++)
            {
                m_voltagesTable[i].m_nominalVoltage = 0.05;
                m_voltagesTable[i].m_uvFaultThreshold = 0.05;
                m_voltagesTable[i].m_ovFaultThreshold = 0.05;
            }
        }
    }
}
