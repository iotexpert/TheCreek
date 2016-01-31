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
using System.Xml;
using System.Xml.Serialization;

namespace TrimMargin_v1_10
{
    public interface ICyIntTable
    {
        void InitializeTable(int count);
    }

    public interface ICyIntRow
    {
    }

    [Serializable]
    public class CyVoltagesTableRow
    {
        public string m_converterName;
        public double? m_nominalVoltage;
        public double? m_minVoltage;
        public double? m_maxVoltage;
        public double? m_marginLow;
        public double? m_marginHigh;
        [XmlIgnore]
        private double? m_marginLowPercentage;
        [XmlIgnore]
        private double? m_marginHighPercentage;
        [XmlIgnore]
        public double? MarginLow
        {
            get
            {
                if (m_marginLow == null)
                    m_marginLow = MarginPercentageToVoltage(this.m_nominalVoltage, this.m_marginLowPercentage);
                return m_marginLow;
            }
            set { m_marginLow = value; }
        }
        [XmlIgnore]
        public double? MarginHigh
        {
            get
            {
                if (m_marginHigh == null)
                    m_marginHigh = MarginPercentageToVoltage(this.m_nominalVoltage, this.m_marginHighPercentage);
                return m_marginHigh;
            }
            set { m_marginHigh = value; }
        }
        [XmlIgnore]
        public double? MarginLowPercentage
        {
            get
            {
                if (m_marginLowPercentage == null)
                    m_marginLowPercentage = GetMarginPercentage(this.m_nominalVoltage, this.m_marginLow);
                return m_marginLowPercentage;
            }
            set { m_marginLowPercentage = value; }
        }
        [XmlIgnore]
        public double? MarginHighPercentage
        {
            get
            {
                if (m_marginHighPercentage == null)
                    m_marginHighPercentage = GetMarginPercentage(this.m_nominalVoltage, this.m_marginHigh);
                return m_marginHighPercentage;
            }
            set { m_marginHighPercentage = value; }
        }

        private double? GetMarginPercentage(double? nominalVoltage, double? margin)
        {
            double? percentage;
            try
            {
                percentage = margin * 100 / nominalVoltage - 100;
                percentage = Math.Round(percentage.Value, 3);
            }
            catch
            {
                percentage = null;
            }
            return percentage;
        }

        public static double? MarginPercentageToVoltage(double? nominalVoltage, double? marginPercent)
        {
            return nominalVoltage + nominalVoltage * marginPercent / 100;
        }

        public CyVoltagesTableRow()
        {
            this.m_converterName = string.Empty;
        }

        public static CyVoltagesTableRow CreateDefaultRow()
        {
            CyVoltagesTableRow row = new CyVoltagesTableRow();

            return row;
        }
    }

    [Serializable]
    public class CyVoltagesTable : ICyIntTable
    {
        public static int COL_COUNT = 9;
        public List<CyVoltagesTableRow> m_voltagesTable;

        public CyVoltagesTable() { }

        public void InitializeTable(int count)
        {
            m_voltagesTable = new List<CyVoltagesTableRow>();
            for (int i = 0; i < count; i++)
            {
                CyVoltagesTableRow row = CyVoltagesTableRow.CreateDefaultRow();
                m_voltagesTable.Add(row);

                row.m_converterName = string.Format("Converter {0}", i + 1);
                row.m_nominalVoltage = 2.25;
                row.m_minVoltage = row.m_nominalVoltage * 0.8;
                row.m_maxVoltage = row.m_nominalVoltage * 1.2;
                row.MarginLow = row.m_nominalVoltage * 0.9;
                row.MarginHigh = row.m_nominalVoltage * 1.1;
            }
        }
    }

    [Serializable]
    public class CyHardwareTableRow
    {
        public CyPWMPolarityType m_polarity;
        public double? m_vddio;
        public double? m_controlVoltage;
        public double? m_r1;
        public double? m_r2;
        public double? m_calculatedR3;
        public double? m_r3;
        public double? m_maxRipple;
        public double? m_calculatedR4;
        public double? m_calculatedC1;
        public double? m_calculatedResolution;

        public CyHardwareTableRow()
        {
        }

        public static CyHardwareTableRow CreateDefaultRow()
        {
            CyHardwareTableRow row = new CyHardwareTableRow();

            row.m_polarity = CyPWMPolarityType.Negative;
            row.m_vddio = 3.3;
            row.m_maxRipple = 1;

            return row;
        }
    }

    [Serializable]
    public class CyHardwareTable : ICyIntTable
    {
        public static int COL_COUNT = 14;
        public List<CyHardwareTableRow> m_hardwareTable;

        public CyHardwareTable() { }

        public void InitializeTable(int count)
        {
            m_hardwareTable = new List<CyHardwareTableRow>();
            for (int i = 0; i < count; i++)
            {
                CyHardwareTableRow row = CyHardwareTableRow.CreateDefaultRow();
                m_hardwareTable.Add(row);

                row.m_polarity = CyPWMPolarityType.Negative;
                row.m_vddio = 3.3;
                row.m_controlVoltage = 0.8;
                row.m_r1 = 1;
                row.m_r2 = 1;
                row.m_r3 = 10;
                row.m_maxRipple = 1;
            }
        }
    }
}
