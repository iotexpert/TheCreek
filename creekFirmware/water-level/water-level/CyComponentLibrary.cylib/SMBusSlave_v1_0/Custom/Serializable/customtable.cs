/*******************************************************************************
* Copyright 2012, Cypress Semiconductor Corporation.  All rights reserved.
* You may use this file only in accordance with the license, terms, conditions, 
* disclaimers, and limitations in the end user license agreement accompanying 
* the software package with which this file was provided. 
********************************************************************************/

using System;
using System.Collections.Generic;
using System.Text;
using System.Xml.Serialization;

namespace SMBusSlave_v1_0
{
    [Serializable]
    public class CyCustomTableRow : ICyRow
    {
        public bool m_enable;
        public string m_name;
        public byte? m_code;
        public CyECmdType m_type;
        public CyEFormatType m_format;
        public byte? m_size;
        public bool m_paged;

        [XmlIgnore]
        public bool m_specific;

        public CyEReadWriteConfigType m_readConfig;
        public CyEReadWriteConfigType m_writeConfig;

        public CyCustomTableRow() { }

        public CyCustomTableRow(CyCustomTableRow row)
        {
            this.m_enable = row.m_enable;
            this.m_name = row.m_name;
            this.m_code = row.m_code;
            this.m_type = row.m_type;
            this.m_format = row.m_format;
            this.m_size = row.m_size;
            this.m_paged = row.m_paged;
            this.m_specific = row.m_specific;
            this.m_readConfig = row.m_readConfig;
            this.m_writeConfig = row.m_writeConfig;
        }

        public CyCustomTableRow(bool enable, string name, byte? code, CyECmdType type, CyEFormatType format, byte? size,
            bool paged, bool specific, CyEReadWriteConfigType readConfig, CyEReadWriteConfigType writeConfig)
        {
            this.m_enable = enable;
            this.m_name = name;
            this.m_code = code;
            this.m_type = type;
            this.m_format = format;
            this.m_size = size;
            this.m_paged = paged;
            this.m_specific = specific;
            this.m_readConfig = readConfig;
            this.m_writeConfig = writeConfig;
        }

        public static CyCustomTableRow CreateDefaultRow(int index)
        {
            CyCustomTableRow row = new CyCustomTableRow();
            row.m_enable = true;
            row.m_name = string.Empty;
            row.m_code = null;
            row.m_type = CyECmdType.ReadWriteByte;
            row.m_format = CyEFormatType.NonNumeric;
            row.m_size = 1;
            row.m_paged = false;
            row.m_specific = false;
            row.m_readConfig = CyEReadWriteConfigType.Manual;
            row.m_writeConfig = CyEReadWriteConfigType.Manual;

            return row;
        }
    }

    [Serializable]
    public class CyCustomTable : ICyTable
    {
        public const string BOOTLOAD_WRITE = "BOOTLOAD_WRITE";
        public const string BOOTLOAD_READ = "BOOTLOAD_READ";
        public const string PAGE = "PAGE";
        public const string QUERY = "QUERY";

        public List<CyCustomTableRow> m_customTable;

        public CyCustomTable() { }

        public void InitializeTable(int count)
        {
            m_customTable = new List<CyCustomTableRow>();
            for (int i = 0; i < count; i++)
            {
                m_customTable.Add(CyCustomTableRow.CreateDefaultRow(i));
            }

            m_customTable.InsertRange(0, CreateReservedRows());
        }

        public static void SetReservedRowsFlag(List<CyCustomTableRow> customTable)
        {
            if (customTable != null)
                for (int i = 0; i < customTable.Count; i++)
                    if (IsCmdSpecific(customTable[i].m_name))
                        customTable[i].m_specific = true;
        }

        public static List<CyCustomTableRow> CreateReservedRows()
        {
            List<CyCustomTableRow> reservedRows = new List<CyCustomTableRow>();

            reservedRows.Add(new CyCustomTableRow(true, BOOTLOAD_WRITE, 0xFC, CyECmdType.ReadWriteBlock,
                CyEFormatType.NonNumeric, CyParamRange.BOOTLOADER_SIZE, false, true, CyEReadWriteConfigType.None,
                CyEReadWriteConfigType.Auto));

            reservedRows.Add(new CyCustomTableRow(true, BOOTLOAD_READ, 0xFD, CyECmdType.ReadWriteBlock,
                CyEFormatType.NonNumeric, CyParamRange.BOOTLOADER_SIZE, false, true, CyEReadWriteConfigType.Auto,
                CyEReadWriteConfigType.None));

            return reservedRows;
        }

        public static CyCustomTableRow GetDefaultPageRow()
        {
            return new CyCustomTableRow(true, PAGE, 0x00, CyECmdType.ReadWriteByte, CyEFormatType.NonNumeric, 1, false,
                true, CyEReadWriteConfigType.Auto, CyEReadWriteConfigType.Auto);
        }

        public static CyCustomTableRow GetDefaultQueryRow()
        {
            return new CyCustomTableRow(true, QUERY, 0x1A, CyECmdType.BlockProcessCall, CyEFormatType.NonNumeric, 1,
                false, true, CyEReadWriteConfigType.Auto, CyEReadWriteConfigType.None);
        }

        public static bool IsCmdSpecific(string name)
        {
            return (name == CyCustomTable.PAGE || name == CyCustomTable.QUERY ||
                name == CyCustomTable.BOOTLOAD_READ || name == CyCustomTable.BOOTLOAD_WRITE);
        }

        public static bool IsCmdPageOrQuery(string name)
        {
            return (name == CyCustomTable.PAGE || name == CyCustomTable.QUERY);
        }

        public static bool IsCmdBootloader(string name)
        {
            return (name == CyCustomTable.BOOTLOAD_WRITE || name == CyCustomTable.BOOTLOAD_READ);
        }

        public static string GetString()
        {
            return Convert.ToString(new CyCustomTable());
        }
    }
}
