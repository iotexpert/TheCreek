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

namespace PowerMonitor_v1_0
{
    [Serializable]
    public class CyCurrentsTableRow
    {
        public const string PREFIX = "I";
        public const int COL_COUNT = 8;
        public string m_powerConverterNumber;
        public string m_converterName;
        public double m_nominalOutputVoltage;
        public double m_ocWarningThreshold;
        public double m_ocFaulthTreshold;
        public double m_shuntResistorValue;
        public double m_csaGain;
        public CyECurrentMeasurementInternalType m_currentMeasurementType;

        public CyCurrentsTableRow() { }

        public CyCurrentsTableRow(int index, string converterName, double nominalOutputVoltage)
        {
            this.m_powerConverterNumber = PREFIX + index.ToString();
            this.m_converterName = converterName;
            this.m_nominalOutputVoltage = nominalOutputVoltage;
        }

        public CyCurrentsTableRow(int index)
        {
            this.m_powerConverterNumber = PREFIX + index.ToString();
        }
    }

    [Serializable]
    public class CyCurrentsTable : ICyTable
    {
        public List<CyCurrentsTableRow> m_currentsTable;

        public CyCurrentsTable() { }

        public void InitializeTable(int count)
        {
            m_currentsTable = new List<CyCurrentsTableRow>();
            for (int i = 1; i <= count; i++)
            {
                CyCurrentsTableRow row = new CyCurrentsTableRow();
                row.m_powerConverterNumber = CyCurrentsTableRow.PREFIX + i.ToString();
                row.m_converterName = "Converter " + i.ToString();
                row.m_nominalOutputVoltage = 2.25;
                row.m_ocWarningThreshold = 9;
                row.m_ocFaulthTreshold = 12;
                row.m_shuntResistorValue = 5;
                row.m_csaGain = 10;
                row.m_currentMeasurementType = CyECurrentMeasurementInternalType.None;
                m_currentsTable.Add(row);
            }
        }
    }
}
