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
    public class CyAuxTableRow
    {
        public const string PREFIX = "Aux";
        public const int COL_COUNT = 3;
        public string m_auxInputName;
        public CyEAdcRangeInternalType m_adcRange;
        public bool m_isDefault = true;

        public static string GetAuxInputNumber(int index)
        {
            return PREFIX + (index + 1).ToString();
        }

        public CyAuxTableRow()
        {
            m_auxInputName = string.Empty;
        }
    }

    [Serializable]
    public class CyAuxTable : ICyTable
    {
        public List<CyAuxTableRow> m_auxTable;

        public CyAuxTable() { }

        public void InitializeTable(int count)
        {
            m_auxTable = new List<CyAuxTableRow>();
            for (int i = 1; i <= 4; i++)
            {
                CyAuxTableRow row = new CyAuxTableRow();
                row.m_auxInputName = "Aux Input " + i.ToString();
                row.m_adcRange = CyEAdcRangeInternalType.SignleEnded_4V;
                row.m_isDefault = true;
                m_auxTable.Add(row);
            }
        }
    }
}
