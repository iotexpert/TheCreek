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

namespace PowerMonitor_v1_20
{
    [Serializable]
    public class CyCurrentsTableRow
    {
        public const string PREFIX = "I";
        public const int COL_COUNT = 8;
        public double? m_ocWarningThreshold;
        public double? m_ocFaulthTreshold;
        public double? m_shuntResistorValue;
        public double? m_csaGain;
        public CyECurrentMeasurementInternalType m_currentMeasurementType;

        public static string GetPowerConverterNumber(int index)
        {
            return PREFIX + (index + 1).ToString();
        }

        public CyCurrentsTableRow() { }
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
