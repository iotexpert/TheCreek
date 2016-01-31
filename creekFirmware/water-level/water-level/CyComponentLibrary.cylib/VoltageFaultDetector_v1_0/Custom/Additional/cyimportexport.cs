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

namespace VoltageFaultDetector_v1_0
{
    public class CyImportExport
    {
        private static char Separator = ';';

        #region Export functions
        public static string Export(List<CyVoltagesTableRow> voltagesTable, List<string> colHeaders)
        {
            StringBuilder sb = new StringBuilder();

            string header = "";
            for (int i = 0; i < colHeaders.Count; i++)
            {
                header += colHeaders[i] + Separator;
            }
            header = header.TrimEnd(Separator);
            sb.AppendLine(header);

            string line;
            for (int i = 0; i < voltagesTable.Count; i++)
            {
                CyVoltagesTableRow item = voltagesTable[i];
                line =
                    CyVoltagesTableRow.GetVoltageIndexStr(i + 1) + Separator +
                    item.m_voltageName.ToString() + Separator +
                    CyParameters.NullableDoubleToString(item.m_nominalVoltage, "f2") + Separator +
                    CyParameters.NullableDoubleToString(item.m_uvFaultThreshold, "f2") + Separator +
                    CyParameters.NullableDoubleToString(item.m_ovFaultThreshold, "f2") + Separator +
                    CyParameters.NullableDoubleToString(item.m_inputScalingFactor, "f3");
                sb.AppendLine(line);
            }
            return sb.ToString();
        }
        #endregion

        #region Import functions
        public static List<CyVoltagesTableRow> ImportVoltagesTable(int numberOfVoltages, int columnCount,
            string fileContent, bool pasteMode)
        {
            List<CyVoltagesTableRow> importTable = new List<CyVoltagesTableRow>();

            string result = CyImportExport.ValidateTextData(columnCount, fileContent, CyParamRanges.NUM_VOLTAGES_MIN,
                CyParamRanges.NUM_VOLTAGES_MAX);

            if (string.IsNullOrEmpty(result))
            {
                importTable = null;
            }
            else
            {
                string[] rows = result.Split(new char[] { '\n' }, StringSplitOptions.None);
                string[] cells;
                for (int i = 1; i <= numberOfVoltages; i++)
                {
                    CyVoltagesTableRow row = new CyVoltagesTableRow();
                    if (i < rows.Length - 1)
                    {
                        try
                        {
                            cells = rows[i].Split(new char[] { Separator }, StringSplitOptions.None);
                            row.m_voltageName = cells[1].Trim().ToString();
                            row.m_nominalVoltage = CyParameters.ParseNullableDouble(cells[2].Trim());
                            row.m_uvFaultThreshold = CyParameters.ParseNullableDouble(cells[3].Trim());
                            row.m_ovFaultThreshold = CyParameters.ParseNullableDouble(cells[4].Trim());
                            row.m_inputScalingFactor = CyParameters.ParseNullableDouble(cells[5].Trim());
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
                            row = CyVoltagesTableRow.CreateDefaultRow(i);
                        else
                            break;
                    }
                    importTable.Add(row);
                }
            }

            return importTable;
        }
        #endregion

        private static string ValidateTextData(int numberOfColumns, string content, byte rowCountMin, byte rowCountMax)
        {
            try
            {
                string[] rows = content.Split(new char[] { '\n' }, StringSplitOptions.None);
                if (rows.Length < rowCountMin || rows.Length > rowCountMax)
                    throw new Exception(Resources.InvalidDataFormat);

                string[] cells;
                for (int i = 0; i < rows.Length - 1; i++)
                {
                    cells = rows[i].Split(new char[] { Separator }, StringSplitOptions.None);
                    if (cells.Length != numberOfColumns)
                        throw new Exception(Resources.InvalidDataFormat);
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message, Resources.MsgWarningTitle, MessageBoxButtons.OK, MessageBoxIcon.Warning);
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
                MessageBox.Show(Resources.FileWriteError, Resources.MsgErrorTitle, MessageBoxButtons.OK,
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
                MessageBox.Show(Resources.FileReadError, Resources.MsgErrorTitle, MessageBoxButtons.OK,
                    MessageBoxIcon.Error);
            }
            return result;
        }
    }
}