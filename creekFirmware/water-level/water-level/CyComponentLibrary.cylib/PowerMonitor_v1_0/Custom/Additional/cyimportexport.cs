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

namespace PowerMonitor_v1_0
{
    public class CyImportExport
    {
        private static char Separator = ',';
        public const string VOLTAGES_POSTFIX = "_voltages";
        public const string CURRENTS_POSTFIX = "_currents";
        public const string AUX_POSTFIX = "_aux";

        #region Export functions
        public static string Export(List<CyVoltagesTableRow> voltagesTable)
        {
            StringBuilder sb = new StringBuilder();

            string header = "Power Converter Number" + Separator + "Converter Name" + Separator +
                "Nominal Output Voltage" + Separator + "Voltage Measurement Type" + Separator +
                "UV Fault Threshold" + Separator + "UV Warning Threshold" + Separator +
                "OV Warning Threshold" + Separator + "OV Fault Threshold" + Separator + "Input Scaling Factor";
            sb.AppendLine(header);

            string line;
            foreach (CyVoltagesTableRow item in voltagesTable)
            {
                line =
                    item.m_powerConverterNumber.ToString() + Separator +
                    item.m_converterName.ToString() + Separator +
                    "\"" + item.m_nominalOutputVoltage.ToString() + "\"" + Separator +
                    item.m_voltageMeasurementType.ToString() + Separator +
                    "\"" + item.m_uvFaultTreshold.ToString() + "\"" + Separator +
                    "\"" + item.m_uvWarningTreshold.ToString() + "\"" + Separator +
                    "\"" + item.m_ovWarningTreshold.ToString() + "\"" + Separator +
                    "\"" + item.m_ovFaultTreshold.ToString() + "\"" + Separator +
                    "\"" + item.m_inputScalingFactor.ToString() + "\"";
                sb.AppendLine(line);
            }
            return sb.ToString();
        }

        public static string Export(List<CyCurrentsTableRow> currentsTable)
        {
            StringBuilder sb = new StringBuilder();

            string header = "Power Converter Number" + Separator + "Converter Name" + Separator +
                "Nominal Output Voltage" + Separator + "Current Measurement Type" + Separator +
                "OC Warning Threshold" + Separator + "OC Fault Threshold" + Separator +
                "Shunt Resistor Value" + Separator + "CSA Gain";
            sb.AppendLine(header);

            string line;
            foreach (CyCurrentsTableRow item in currentsTable)
            {
                line =
                    item.m_powerConverterNumber.ToString() + Separator +
                    item.m_converterName.ToString() + Separator +
                    "\"" + item.m_nominalOutputVoltage.ToString() + "\"" + Separator +
                    item.m_currentMeasurementType.ToString() + Separator +
                    "\"" + item.m_ocWarningThreshold.ToString() + "\"" + Separator +
                    "\"" + item.m_ocFaulthTreshold.ToString() + "\"" + Separator +
                    "\"" + item.m_shuntResistorValue.ToString() + "\"" + Separator +
                    "\"" + item.m_csaGain.ToString() + "\"";
                sb.AppendLine(line);
            }
            return sb.ToString();
        }

        public static string Export(List<CyAuxTableRow> auxTable)
        {
            StringBuilder sb = new StringBuilder();

            string header = "Aux input number" + Separator + "Aux input name" + Separator +
                "Voltage measurement type";
            sb.AppendLine(header);

            string line;
            foreach (CyAuxTableRow item in auxTable)
            {
                line =
                    item.m_auxInputNumber.ToString() + Separator +
                    item.m_auxInputName.ToString() + Separator +
                    item.m_adcRange.ToString();
                sb.AppendLine(line);
            }
            return sb.ToString();
        }
        #endregion

        #region Import functions
        public static List<CyVoltagesTableRow> ImportVoltagesTable(int numberOfConverters, int columnCount,
            string fileContent, bool pasteMode)
        {
            List<CyVoltagesTableRow> importTable = new List<CyVoltagesTableRow>();

            string result = CyImportExport.ValidateTextData(columnCount, fileContent, CyParamRanges.NUM_CONVERTERS_MIN,
                CyParamRanges.NUM_CONVERTERS_MAX);

            if (string.IsNullOrEmpty(result))
            {
                importTable = null;
            }
            else
            {
                string[] rows = result.Split(new char[] { '\n' }, StringSplitOptions.None);
                int rowLength = (rows[rows.Length - 1].Trim() == string.Empty) ? rows.Length - 1 : rows.Length;
                string[] cells;
                for (int i = 1; i <= numberOfConverters; i++)
                {
                    CyVoltagesTableRow row = new CyVoltagesTableRow();
                    if (i < rowLength)
                    {
                        try
                        {
                            cells = rows[i].Split(new char[] { Separator }, StringSplitOptions.None);
                            row.m_powerConverterNumber = CyVoltagesTableRow.PREFIX + (i).ToString();
                            row.m_converterName = cells[1].Trim().ToString();
                            row.m_nominalOutputVoltage = double.Parse(cells[2].Replace("\"", "").Trim());
                            if (cells[3].Trim() == CyEVInternalType.SingleEnded.ToString())
                                row.m_voltageMeasurementType = CyEVInternalType.SingleEnded;
                            else if (cells[3].Trim() == CyEVInternalType.Differential.ToString())
                                row.m_voltageMeasurementType = CyEVInternalType.Differential;
                            else
                                throw new Exception();
                            row.m_uvFaultTreshold = double.Parse(cells[4].Replace("\"", "").Trim());
                            row.m_uvWarningTreshold = double.Parse(cells[5].Replace("\"", "").Trim());
                            row.m_ovWarningTreshold = double.Parse(cells[6].Replace("\"", "").Trim());
                            row.m_ovFaultTreshold = double.Parse(cells[7].Replace("\"", "").Trim());
                            row.m_inputScalingFactor = double.Parse(cells[8].Replace("\"", "").Trim());
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
                            row = new CyVoltagesTableRow(i);
                        else
                            break;
                    }
                    importTable.Add(row);
                }
            }

            return importTable;
        }

        public static List<CyCurrentsTableRow> ImportCurrentsTable(int numberOfConverters, int columnCount,
            string fileContent, bool pasteMode)
        {
            List<CyCurrentsTableRow> importTable = new List<CyCurrentsTableRow>();

            string result = CyImportExport.ValidateTextData(columnCount, fileContent, CyParamRanges.NUM_CONVERTERS_MIN,
                CyParamRanges.NUM_CONVERTERS_MAX);

            if (string.IsNullOrEmpty(result))
            {
                importTable = null;
            }
            else
            {
                string[] rows = result.Split(new char[] { '\n' }, StringSplitOptions.None);
                int rowLength = (rows[rows.Length - 1].Trim() == string.Empty) ? rows.Length - 1 : rows.Length;
                string[] cells;
                for (int i = 1; i <= numberOfConverters; i++)
                {
                    CyCurrentsTableRow row = new CyCurrentsTableRow();
                    if (i < rowLength)
                    {
                        try
                        {
                            cells = rows[i].Split(new char[] { Separator }, StringSplitOptions.None);
                            row.m_powerConverterNumber = CyCurrentsTableRow.PREFIX + (i).ToString();
                            row.m_converterName = cells[1].Trim().ToString();
                            row.m_nominalOutputVoltage = double.Parse(cells[2].Replace("\"", "").Trim());
                            if (cells[3].Trim() == CyECurrentMeasurementInternalType.CSA.ToString())
                                row.m_currentMeasurementType = CyECurrentMeasurementInternalType.CSA;
                            else if (cells[3].Trim() == CyECurrentMeasurementInternalType.Direct.ToString())
                                row.m_currentMeasurementType = CyECurrentMeasurementInternalType.Direct;
                            else if (cells[3].Trim() == CyECurrentMeasurementInternalType.None.ToString())
                                row.m_currentMeasurementType = CyECurrentMeasurementInternalType.None;
                            else
                                throw new Exception();
                            row.m_ocWarningThreshold = double.Parse(cells[4].Replace("\"", "").Trim());
                            row.m_ocFaulthTreshold = double.Parse(cells[5].Replace("\"", "").Trim());
                            row.m_shuntResistorValue = double.Parse(cells[6].Replace("\"", "").Trim());
                            row.m_csaGain = double.Parse(cells[7].Replace("\"", "").Trim());                            
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
                            row = new CyCurrentsTableRow(i);
                        else
                            break;
                    }
                    importTable.Add(row);
                }
            }

            return importTable;
        }

        public static List<CyAuxTableRow> ImportAuxTable(int numberOfAuxChannels, int columnCount, string fileContent,
            bool pasteMode)
        {
            List<CyAuxTableRow> importTable = new List<CyAuxTableRow>();

            string result = CyImportExport.ValidateTextData(columnCount, fileContent,
                CyParamRanges.NUM_AUX_CHANNELS_MIN, CyParamRanges.NUM_AUX_CHANNELS_MAX);

            if (string.IsNullOrEmpty(result))
            {
                importTable = null;
            }
            else
            {
                string[] rows = result.Split(new char[] { '\n' }, StringSplitOptions.None);
                int rowLength = (rows[rows.Length - 1].Trim() == string.Empty) ? rows.Length - 1 : rows.Length;
                string[] cells;
                for (int i = 1; i <= numberOfAuxChannels; i++)
                {
                    CyAuxTableRow row = new CyAuxTableRow();
                    if (i < rowLength)
                    {
                        try
                        {
                            cells = rows[i].Split(new char[] { Separator }, StringSplitOptions.None);
                            row.m_auxInputNumber = CyAuxTableRow.PREFIX + (i).ToString();
                            row.m_auxInputName = cells[1].Trim().ToString();
                            if (cells[2].Trim() == CyEAdcRangeInternalType.SignleEnded_4V.ToString())
                                row.m_adcRange = CyEAdcRangeInternalType.SignleEnded_4V;
                            else if (cells[2].Trim() == CyEAdcRangeInternalType.Differential_64mV.ToString())
                                row.m_adcRange = CyEAdcRangeInternalType.Differential_64mV;
                            else if (cells[2].Trim() == CyEAdcRangeInternalType.Differential_128mV.ToString())
                                row.m_adcRange = CyEAdcRangeInternalType.Differential_128mV;
                            else if (cells[2].Trim() == CyEAdcRangeInternalType.Differential_2048mV.ToString())
                                row.m_adcRange = CyEAdcRangeInternalType.Differential_2048mV;
                            else
                                throw new Exception();
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
                            row = new CyAuxTableRow(i);
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
                int rowLength = (rows[rows.Length - 1].Trim() == string.Empty) ? rows.Length - 2 : rows.Length - 1;
                if (rowLength < rowCountMin || rowLength > rowCountMax)
                    throw new Exception(Resources.InvalidDataFormat);

                string[] cells;
                for (int i = 0; i <= rowLength; i++)
                {
                    cells = rows[i].Split(new char[] { Separator }, StringSplitOptions.None);
                    if (cells.Length != numberOfColumns)
                        throw new Exception(Resources.InvalidDataFormat);
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message, Resources.ComponentName, MessageBoxButtons.OK, MessageBoxIcon.Warning);
                content = null;
            }
            return content;
        }

        public static void SaveToFile(string fileName, string fileContent)
        {
            using (StreamWriter writer = new StreamWriter(fileName))
            {
                writer.Write(fileContent);
            }
        }

        public static string GetFileText(string fileName)
        {
            string result = string.Empty;
            using (StreamReader sr = new StreamReader(fileName))
            {
                result = sr.ReadToEnd();
            }

            return result;
        }
    }
}