/*******************************************************************************
* Copyright 2012, Cypress Semiconductor Corporation.  All rights reserved.
* You may use this file only in accordance with the license, terms, conditions,
* disclaimers, and limitations in the end user license agreement accompanying
* the software package with which this file was provided.
********************************************************************************/

using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Data;
using System.Text;
using System.Windows.Forms;
using System.IO;
using CyDesigner.Extensions.Common;
using CyDesigner.Extensions.Gde;

namespace TrimMargin_v1_10
{
    public class CyImportExport
    {
        private static char Separator = ',';
        public const string VOLTAGES_POSTFIX = "_voltages";
        public const string HARDWARE_POSTFIX = "_hardware";

        #region Export functions
        public static string Export(List<CyVoltagesTableRow> voltagesTable, List<string> colHeaders)
        {
            StringBuilder sb = new StringBuilder();

            sb.AppendLine(GetHeaders(colHeaders));

            string line;
            for (int i = 0; i < voltagesTable.Count; i++)
            {
                CyVoltagesTableRow item = voltagesTable[i];
                line =
                    (i + 1).ToString() + Separator +
                    item.m_converterName.ToString() + Separator +
                    CyParameters.NullableDoubleToString(item.m_nominalVoltage) + Separator +
                    CyParameters.NullableDoubleToString(item.m_minVoltage) + Separator +
                    CyParameters.NullableDoubleToString(item.m_maxVoltage) + Separator +
                    
                    CyParameters.NullableDoubleToString(item.MarginLowPercentage, 0) + Separator +
                    CyParameters.NullableDoubleToString(item.MarginHighPercentage, 0) + Separator +
                    
                    CyParameters.NullableDoubleToString(item.MarginLow, 3) + Separator +
                    CyParameters.NullableDoubleToString(item.MarginHigh, 3);
                sb.AppendLine(line);
            }
            return sb.ToString();
        }

        public static string Export(List<CyHardwareTableRow> hardwareTable, List<CyVoltagesTableRow> voltagesTable,
            List<string> colHeaders)
        {
            StringBuilder sb = new StringBuilder();
            sb.AppendLine(GetHeaders(colHeaders));

            string line;
            for (int i = 0; i < hardwareTable.Count; i++)
            {
                CyHardwareTableRow item = hardwareTable[i];
                line =
                    (i + 1).ToString() + Separator +
                    voltagesTable[i].m_converterName.ToString() + Separator +
                    CyParameters.NullableDoubleToString(voltagesTable[i].m_nominalVoltage) + Separator +
                    item.m_polarity.ToString() + Separator +
                    CyParameters.NullableDoubleToString(item.m_vddio) + Separator +
                    CyParameters.NullableDoubleToString(item.m_controlVoltage) + Separator +
                    CyParameters.NullableDoubleToString(item.m_r1) + Separator +
                    CyParameters.NullableDoubleToString(item.m_r2) + Separator +
                    CyParameters.NullableDoubleToString(item.m_calculatedR3) + Separator +
                    CyParameters.NullableDoubleToString(item.m_r3) + Separator +
                    CyParameters.NullableDoubleToString(item.m_calculatedR4) + Separator +
                    CyParameters.NullableDoubleToString(item.m_maxRipple) + Separator +
                    CyParameters.NullableDoubleToString(item.m_calculatedC1) + Separator +
                    CyParameters.NullableDoubleToString(item.m_calculatedResolution);
                sb.AppendLine(line);
            }
            return sb.ToString();
        }

        static string GetHeaders(List<string> colHeaders)
        {
            string header = "";
            for (int i = 0; i < colHeaders.Count; i++)
            {
                header += colHeaders[i] + Separator;
            }
            header = header.TrimEnd(Separator);

            return header;
        }
        #endregion

        #region Import functions
        public static List<CyVoltagesTableRow> ImportVoltagesTable(int numberOfRows, int columnCount,
            string fileContent, bool pasteMode)
        {
            string result = ValidateTextData(columnCount, fileContent, CyParamRanges.NUM_CONVERTERS_MIN,
                CyParamRanges.NUM_CONVERTERS_MAX);

            if (string.IsNullOrEmpty(result))
            {
                return null;
            }
            else
            {
                string[] rows = result.Split(new char[] { '\n' }, StringSplitOptions.None);
                string[] cells;

                List<CyVoltagesTableRow> importTable = new List<CyVoltagesTableRow>();

                for (int i = 1; i <= numberOfRows; i++)
                {
                    CyVoltagesTableRow row = new CyVoltagesTableRow();
                    if (i < rows.Length - 1)
                    {
                        try
                        {
                            cells = rows[i].Split(new char[] { Separator }, StringSplitOptions.None);
                            row.m_converterName = cells[1].Trim().ToString();
                            row.m_nominalVoltage = CyParameters.ParseNullableDouble(cells[2].Trim());
                            row.m_minVoltage = CyParameters.ParseNullableDouble(cells[3].Trim());
                            row.m_maxVoltage = CyParameters.ParseNullableDouble(cells[4].Trim());
                            row.MarginLowPercentage = CyParameters.ParseNullableDouble(cells[5].Trim());
                            row.MarginHighPercentage = CyParameters.ParseNullableDouble(cells[6].Trim());
                            //row.m_marginLow = CyParameters.ParseNullableDouble(cells[7].Trim()); // skip percentages
                            //row.m_marginHigh = CyParameters.ParseNullableDouble(cells[8].Trim());
                        }
                        catch (Exception)
                        {
                            importTable = null;
                            break;
                        }
                    }
                    else
                    {
                        if (pasteMode == false)
                            row = CyVoltagesTableRow.CreateDefaultRow();
                        else
                            break;
                    }
                    importTable.Add(row);
                }

                return importTable;
            }
        }

        public static List<CyHardwareTableRow> ImportHardwareTable(int numberOfRows, int columnCount,
           string fileContent, bool pasteMode)
        {
            string result = ValidateTextData(columnCount, fileContent, CyParamRanges.NUM_CONVERTERS_MIN,
                CyParamRanges.NUM_CONVERTERS_MAX);

            if (string.IsNullOrEmpty(result))
            {
                return null;
            }
            else
            {
                string[] rows = result.Split(new char[] { '\n' }, StringSplitOptions.None);
                string[] cells;

                List<CyHardwareTableRow> importTable = new List<CyHardwareTableRow>();

                for (int i = 1; i <= numberOfRows; i++)
                {
                    CyHardwareTableRow row = new CyHardwareTableRow();
                    if (i < rows.Length - 1)
                    {
                        try
                        {
                            int ind = 0;

                            cells = rows[i].Split(new char[] { Separator }, StringSplitOptions.None);
                            ind++;
                            ind++;
                            ind++;

                            row.m_polarity = (CyPWMPolarityType)CyEnumConverter.GetEnumValue(cells[ind++].Trim(),
                                typeof(CyPWMPolarityType));

                            row.m_vddio = CyParameters.ParseNullableDouble(cells[ind++].Trim());
                            row.m_controlVoltage = CyParameters.ParseNullableDouble(cells[ind++].Trim());
                            row.m_r1 = CyParameters.ParseNullableDouble(cells[ind++].Trim());
                            row.m_r2 = CyParameters.ParseNullableDouble(cells[ind++].Trim());
                            ind++;
                            row.m_r3 = CyParameters.ParseNullableDouble(cells[ind++].Trim());
                            row.m_calculatedR4 = CyParameters.ParseNullableDouble(cells[ind++].Trim());
                            row.m_maxRipple = CyParameters.ParseNullableDouble(cells[ind++].Trim());
                        }
                        catch (Exception)
                        {
                            importTable = null;
                            break;
                        }
                    }
                    else
                    {
                        if (pasteMode == false)
                            row = CyHardwareTableRow.CreateDefaultRow();
                        else
                            break;
                    }
                    importTable.Add(row);
                }

                return importTable;
            }

        }

        #endregion

        private static string ValidateTextData(int numberOfColumns, string content, byte rowCountMin, byte rowCountMax)
        {
            try
            {
                string[] rows = content.Split(new char[] { '\n' }, StringSplitOptions.None);

                int rowLength = (rows[rows.Length - 1].Trim() == string.Empty) ? rows.Length - 2 : rows.Length - 1;

                if (rowLength < rowCountMin || rowLength > rowCountMax)
                    throw new Exception(Resources.InvalidDataFormat);

                string[] cells;
                for (int i = 0; i < rowLength; i++)
                {
                    cells = rows[i].Split(new char[] { Separator }, StringSplitOptions.None);

                    //Stop if empty line occurs
                    if (cells.Length == 1)
                        break;

                    //Validate columns count
                    if (cells.Length != numberOfColumns)
                        throw new Exception(Resources.InvalidDataFormat);
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message, "Error", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                content = null;
            }
            return content;
        }

        public static void SaveToFile(string fileName, string fileContent)
        {
            try
            {
                using (StreamWriter writer = new StreamWriter(fileName))
                {
                    writer.Write(fileContent);
                }
            }
            catch
            {
                MessageBox.Show(Resources.FileWriteError, fileName, MessageBoxButtons.OK,
                   MessageBoxIcon.Error);
            }
        }

        public static string GetFileText(string fileName)
        {
            string result = string.Empty;
            try
            {
                using (StreamReader sr = new StreamReader(fileName))
                {
                    result = sr.ReadToEnd();
                    sr.Close();
                }
            }
            catch
            {
                MessageBox.Show(Resources.FileReadError, fileName, MessageBoxButtons.OK,
                    MessageBoxIcon.Error);
            }
            return result;
        }

        public static List<StringBuilder> GetTables(string fileContent)
        {
            StringReader data = new StringReader(fileContent);
            List<StringBuilder> listTables = new List<StringBuilder>();
            int pos = 0;
            string line = data.ReadLine();

            while (line != null)
            {
                listTables.Add(new StringBuilder());

                //Remove empty lines
                while (line == string.Empty) line = data.ReadLine();

                //Get One table
                while (string.IsNullOrEmpty(line) == false)
                {
                    listTables[pos].AppendLine(line);

                    line = data.ReadLine();
                }


                pos++;
                line = data.ReadLine();
            }


            return listTables;
        }
    }
}
