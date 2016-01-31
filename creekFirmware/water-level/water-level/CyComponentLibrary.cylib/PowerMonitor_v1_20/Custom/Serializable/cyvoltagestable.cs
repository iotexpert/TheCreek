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

namespace PowerMonitor_v1_20
{
    public interface ICyTable
    {
        void InitializeTable(int count);
    }

    [Serializable]
    public class CyVoltagesTableRow
    {
        public const string PREFIX = "V";
        public const int COL_COUNT = 9;        
        public string m_converterName;
        public double? m_nominalOutputVoltage;
        public double? m_uvFaultTreshold;
        public double? m_uvWarningTreshold;
        public double? m_ovFaultTreshold;
        public double? m_ovWarningTreshold;
        public double? m_inputScalingFactor;
        public CyEVInternalType m_voltageMeasurementType;

        public static string GetConverterNumber(int index)
        {
            return PREFIX + (index + 1).ToString();
        }

        public CyVoltagesTableRow()
        {
            this.m_converterName = string.Empty;
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
            for (int i = 1; i <= count; i++)
            {
                CyVoltagesTableRow row = new CyVoltagesTableRow();
                row.m_converterName = "Converter " + i.ToString();
                row.m_nominalOutputVoltage = 2.25;
                row.m_uvFaultTreshold = 0.75;
                row.m_ovFaultTreshold = 3;
                row.m_uvWarningTreshold = 1.7;
                row.m_ovWarningTreshold = 2.825;
                row.m_inputScalingFactor = 1;
                row.m_voltageMeasurementType = CyEVInternalType.SingleEnded;                
                m_voltagesTable.Add(row);
            }
        }
    }
}
