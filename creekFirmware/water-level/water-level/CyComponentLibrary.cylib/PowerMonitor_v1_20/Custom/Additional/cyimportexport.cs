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

namespace PowerMonitor_v1_20
{
    public class CyImportExport
    {
        private static char separator = ',';

        #region Export functions
        public static string Export(CyParameters prms, List<CyVoltagesTableRow> voltagesTable)
        {
            StringBuilder sb = new StringBuilder();

            string header = "Power Converter Number" + separator + "Converter Name" + separator +
                "Nominal Output Voltage" + separator + "Voltage Measurement Type" + separator +
                "UV Fault Threshold" + separator + "UV Warning Threshold" + separator +
                "OV Warning Threshold" + separator + "OV Fault Threshold" + separator + "Input Scaling Factor";
            sb.AppendLine(header);

            string line;

            for (int i = 0; i < prms.NumConverters; i++)
            {
                if (i >= voltagesTable.Count) break;

                CyVoltagesTableRow item = voltagesTable[i];
                line =
                    CyVoltagesTableRow.GetConverterNumber(i) + separator +
                    item.m_converterName.ToString() + separator +
                     CyParameters.NullableDoubleToString(item.m_nominalOutputVoltage) + separator +
                    item.m_voltageMeasurementType.ToString() + separator +
                     CyParameters.NullableDoubleToString(item.m_uvFaultTreshold) + separator +
                     CyParameters.NullableDoubleToString(item.m_uvWarningTreshold) + separator +
                     CyParameters.NullableDoubleToString(item.m_ovWarningTreshold) + separator +
                     CyParameters.NullableDoubleToString(item.m_ovFaultTreshold) + separator +
                     CyParameters.NullableDoubleToString(item.m_inputScalingFactor);
                sb.AppendLine(line);
            }
            return sb.ToString();
        }

        public static string Export(CyParameters prms, List<CyCurrentsTableRow> currentsTable)
        {
            StringBuilder sb = new StringBuilder();

            string header = "Power Converter Number" + separator + "Converter Name" + separator +
                "Nominal Output Voltage" + separator + "Current Measurement Type" + separator +
                "OC Warning Threshold" + separator + "OC Fault Threshold" + separator +
                "Shunt Resistor Value" + separator + "CSA Gain";
            sb.AppendLine(header);

            string line;
            for (int i = 0; i < prms.NumConverters; i++)                
            {
                if (i >= currentsTable.Count) break;

                CyCurrentsTableRow item = currentsTable[i];
                line =
                    CyCurrentsTableRow.GetPowerConverterNumber(i) + separator +
                    prms.VoltagesTable[i].m_converterName.ToString() + separator +
                     CyParameters.NullableDoubleToString(prms.VoltagesTable[i].m_nominalOutputVoltage) + separator +
                    item.m_currentMeasurementType.ToString() + separator +
                     CyParameters.NullableDoubleToString(item.m_ocWarningThreshold) +  separator +
                     CyParameters.NullableDoubleToString(item.m_ocFaulthTreshold) +  separator +
                     CyParameters.NullableDoubleToString(item.m_shuntResistorValue) +  separator +
                     CyParameters.NullableDoubleToString(item.m_csaGain);
                sb.AppendLine(line);
            }
            return sb.ToString();
        }

        public static string Export(CyParameters prms, List<CyAuxTableRow> auxTable)
        {
            StringBuilder sb = new StringBuilder();

            string header = "Aux input number" + separator + "Aux input name" + separator +
                "Voltage measurement type";
            sb.AppendLine(header);

            string line;

            for (int i = 0; i < prms.NumAuxChannels; i++)
            {
                if (i >= auxTable.Count) break;
                CyAuxTableRow item = auxTable[i];
                line =
                    CyAuxTableRow.GetAuxInputNumber(i) + separator +
                    item.m_auxInputName.ToString() + separator +
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

            if (string.IsNullOrEmpty(result)) return null;

            string[] rows = result.Split(new char[] { '\n' }, StringSplitOptions.None);
            int rowLength = (rows[rows.Length - 1].Trim() == string.Empty) ? rows.Length - 1 : rows.Length;

            try
            {
                for (int i = 1; i <= numberOfConverters; i++)
                {
                    CyVoltagesTableRow row = new CyVoltagesTableRow();
                    if (i < rowLength)
                    {
                        string[] cells = rows[i].Split(new char[] { separator }, StringSplitOptions.None);
                        //cells[0] first column with number is ignored
                        row.m_converterName = cells[1].Trim().ToString();
                        row.m_nominalOutputVoltage = CyParameters.ParseNullableDouble(cells[2]);

                        string mesurement = cells[3].Trim();
                        if (mesurement == CyEVInternalType.SingleEnded.ToString())
                            row.m_voltageMeasurementType = CyEVInternalType.SingleEnded;
                        else if (mesurement == CyEVInternalType.Differential.ToString())
                            row.m_voltageMeasurementType = CyEVInternalType.Differential;
                        else
                            throw new Exception();

                        row.m_uvFaultTreshold = CyParameters.ParseNullableDouble(cells[4]);
                        row.m_uvWarningTreshold = CyParameters.ParseNullableDouble(cells[5]);
                        row.m_ovWarningTreshold = CyParameters.ParseNullableDouble(cells[6]);
                        row.m_ovFaultTreshold = CyParameters.ParseNullableDouble(cells[7]);
                        row.m_inputScalingFactor = CyParameters.ParseNullableDouble(cells[8]);

                    }
                    else
                    {
                        if (pasteMode) break;

                        row = new CyVoltagesTableRow();
                    }
                    importTable.Add(row);
                }
            }
            catch (Exception)
            {
                importTable = null;
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
                            cells = rows[i].Split(new char[] { separator }, StringSplitOptions.None);
                            //cells[0] ignore Converter Number
                            //cells[1] ignore Converter Name
                            //cells[2] ignore Converter Voltage
                            if (cells[3].Trim() == CyECurrentMeasurementInternalType.CSA.ToString())
                                row.m_currentMeasurementType = CyECurrentMeasurementInternalType.CSA;
                            else if (cells[3].Trim() == CyECurrentMeasurementInternalType.Direct.ToString())
                                row.m_currentMeasurementType = CyECurrentMeasurementInternalType.Direct;
                            else if (cells[3].Trim() == CyECurrentMeasurementInternalType.None.ToString())
                                row.m_currentMeasurementType = CyECurrentMeasurementInternalType.None;
                            else
                                throw new Exception();
                            row.m_ocWarningThreshold = CyParameters.ParseNullableDouble(cells[4]);
                            row.m_ocFaulthTreshold = CyParameters.ParseNullableDouble(cells[5]);
                            row.m_shuntResistorValue = CyParameters.ParseNullableDouble(cells[6]);
                            row.m_csaGain = CyParameters.ParseNullableDouble(cells[7]);
                        }
                        catch (Exception)
                        {
                            importTable = null;
                            break;
                        }
                    }
                    else
                    {
                        if (pasteMode) break;

                        row = new CyCurrentsTableRow();
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
                            cells = rows[i].Split(new char[] { separator }, StringSplitOptions.None);
                            //cells[0] ignore Aux Input Number
                            row.m_auxInputName = cells[1].Trim();
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
                        if (pasteMode) break;

                        row = new CyAuxTableRow();
                    }
                    importTable.Add(row);
                }
            }

            return importTable;
        }
        #endregion

        private static string ValidateTextData(int numberOfColumns, string content, byte rowCountMin, byte rowCountMax)
        {
            string[] rows;
            try
            {
                rows = content.Split(new char[] { '\n' }, StringSplitOptions.None);
                int rowsCount = (rows[rows.Length - 1].Trim() == string.Empty) ? rows.Length - 2 : rows.Length - 1;
                if (rowsCount < rowCountMin || rowsCount > rowCountMax)
                    throw new Exception(Resources.InvalidDataFormat);

                
                string[] cells;
                for (int i = 0; i <= rowsCount; i++)
                {
                    cells = rows[i].Split(new char[] { separator }, StringSplitOptions.None);
                    if (cells.Length != numberOfColumns)
                        throw new Exception(Resources.InvalidDataFormat);
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message, typeof(CyCustomizer).Namespace, MessageBoxButtons.OK, 
                    MessageBoxIcon.Warning);
                content = null;
            }

            return content;
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
