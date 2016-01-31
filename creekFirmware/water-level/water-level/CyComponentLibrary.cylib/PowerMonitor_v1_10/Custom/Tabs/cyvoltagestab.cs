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
using CyDesigner.Extensions.Common;
using CyDesigner.Extensions.Gde;

namespace PowerMonitor_v1_10
{
    public partial class CyVoltagesTab : CyEditingWrapperControl
    {
        private int m_dataGridRowCount;
        private List<int> m_sanityCheckCols;
        private List<int> m_rangeCheckCols;

        #region Class properties
        public override string TabName
        {
            get { return "Power Converter Voltages"; }
        }

        public int DataGridRowCount
        {
            get { return m_dataGridRowCount; }
            set
            {
                m_dataGridRowCount = value;
                dataGridView1.RowCount = m_dataGridRowCount;
            }
        }

        public int DataGridFirstSelectedRow
        {
            get { return GetLessSelectedRow(dataGridView1.SelectedRows); }
        }
        #endregion

        #region Constructor(s)
        public CyVoltagesTab(CyParameters param)
            : base()
        {
            // Initialize parameters objects
            m_params = param;
            m_params.m_voltagesTab = this;

            InitializeComponent();

            // Initialize toolstrip
            cyToolStrip1.SetParameters(param);
            cyToolStrip1.m_dgv = dataGridView1;

            // Initialize wrapper objects
            m_wrapperToolStrip = cyToolStrip1;
            m_wrapperDataGridView = dataGridView1;
            m_voltagesTableErrors = new List<string[]>();
            for (int i = 0; i < CyParamRanges.NUM_CONVERTERS_MAX; i++)
                m_voltagesTableErrors.Add(new string[dataGridView1.ColumnCount]);

            // Initialize data grid view
            dataGridView1.EditMode = DataGridViewEditMode.EditOnEnter;
            this.colVoltageMeasurementType.Items.AddRange(CyEnumConverter.GetEnumDescList(typeof(CyEVInternalType)));
            dataGridView1.AllowUserToOrderColumns = false;
            dataGridView1.AllowUserToResizeRows = false;
            dataGridView1.AllowUserToAddRows = false;
            foreach (DataGridViewColumn item in dataGridView1.Columns)
                item.SortMode = DataGridViewColumnSortMode.Programmatic;

            // Arrays for validating parameters
            m_sanityCheckCols = new List<int>();
            m_sanityCheckCols.Add(colUVFaultThreshold.Index);
            m_sanityCheckCols.Add(colUVWarningThreshold.Index);
            m_sanityCheckCols.Add(colOVWarningThreshold.Index);
            m_sanityCheckCols.Add(colOVFaultThreshold.Index);
            m_sanityCheckCols.Add(colInputScalingFactor.Index);

            m_rangeCheckCols = new List<int>();
            m_rangeCheckCols.Add(colNominalOutputVoltage.Index);
            m_rangeCheckCols.Add(colVoltageMeasurementType.Index);
            m_rangeCheckCols.Add(colUVFaultThreshold.Index);
            m_rangeCheckCols.Add(colUVWarningThreshold.Index);
            m_rangeCheckCols.Add(colOVWarningThreshold.Index);
            m_rangeCheckCols.Add(colOVFaultThreshold.Index);
            m_rangeCheckCols.Add(colInputScalingFactor.Index);

            CyParameters.DGDisabledStyle = dataGridView1.Columns[0].DefaultCellStyle;
            CyParameters.DGEnabledStyle = dataGridView1.Columns[1].DefaultCellStyle;

            // Fill data grid view
            UpdateUIFromTable();
        }
        #endregion

        #region Event handlers
        private void dataGridView1_CellValueChanged(object sender, DataGridViewCellEventArgs e)
        {
            if (CyParameters.GlobalEditMode)
            {                
                UpdateTableRowFromUI(e.RowIndex);
                m_params.SetVoltagesTable();

                if (NeedSync(e.ColumnIndex))
                {
                    m_params.m_generalTab.SyncTables();
                    m_params.m_currentsTab.UpdateUIFromTable();
                }

                ValidateRow(e);
            }
        }

        private void ValidateRow(DataGridViewCellEventArgs e)
        {
            if (e.ColumnIndex != colConverterName.Index) // do not check when converter name changed
            {
                string errorMessage = string.Empty;

                // Range check
                bool rangeIsValid = true;
                for (int i = 0; i < m_rangeCheckCols.Count; i++)
                {
                    errorMessage = RangeCheck(e.RowIndex, m_rangeCheckCols[i]);
                    if (errorMessage != string.Empty)
                        rangeIsValid = false;
                    m_voltagesTableErrors[e.RowIndex].SetValue(errorMessage, m_rangeCheckCols[i]);
                }
                if (rangeIsValid)
                {
                    // Range is OK. Do sanity check
                    for (int i = 0; i < m_sanityCheckCols.Count; i++)
                    {
                        errorMessage = SanityCheck(e.RowIndex, m_sanityCheckCols[i]);
                        m_voltagesTableErrors[e.RowIndex].SetValue(errorMessage, m_sanityCheckCols[i]);
                    }
                }
            }
        }

        private void dataGridView1_CurrentCellDirtyStateChanged_1(object sender, EventArgs e)
        {
            CommitCellValueImmediately(dataGridView1);
        }

        private void dataGridView1_SelectionChanged(object sender, EventArgs e)
        {
            cyToolStrip1.ChangeCopyPasteEnabling(dataGridView1.SelectedRows.Count > 0);
        }
        #endregion

        #region Communication between object and UI
        public void UpdateUIFromTable()
        {
            string voltageMeasurementType;
            m_dataGridRowCount = m_params.NumConverters;
            dataGridView1.Rows.Clear();
            for (int i = 0; i < m_dataGridRowCount; i++)
            {
                voltageMeasurementType = CyEnumConverter.GetEnumDesc(m_params.VoltagesTable[i].
                    m_voltageMeasurementType);

                object[] row = new object[] { 
                    m_params.VoltagesTable[i].m_powerConverterNumber, 
                    m_params.VoltagesTable[i].m_converterName, 
                    m_params.VoltagesTable[i].m_nominalOutputVoltage, 
                    voltageMeasurementType, 
                    m_params.VoltagesTable[i].m_uvFaultTreshold,
                    m_params.VoltagesTable[i].m_uvWarningTreshold,
                    m_params.VoltagesTable[i].m_ovWarningTreshold,
                    m_params.VoltagesTable[i].m_ovFaultTreshold,
                    m_params.VoltagesTable[i].m_inputScalingFactor
                };

                CyParameters.SetRowEmptyValues(row);

                dataGridView1.Rows.Add(row);

            }
        }


        public void UpdateTableFromUI()
        {
            for (int i = 0; i < m_dataGridRowCount; i++)
                UpdateTableRowFromUI(i);
        }

        public void UpdateTableRowFromUI(int i)
        {
            if (i>=0 && i < m_dataGridRowCount)
            {
                m_params.VoltagesTable[i].m_powerConverterNumber = dataGridView1.Rows[i].Cells[
                    colPowerConverterNumber.Index].Value.ToString();
                try
                {
                    m_params.VoltagesTable[i].m_converterName = dataGridView1.Rows[i].Cells[
                        colConverterName.Index].Value.ToString();
                }
                catch
                {
                    m_params.VoltagesTable[i].m_converterName = string.Empty;
                }

                try
                {
                    m_params.VoltagesTable[i].m_nominalOutputVoltage = double.Parse(dataGridView1.Rows[i].Cells[
                        colNominalOutputVoltage.Index].Value.ToString());
                }
                catch
                {
                    m_params.VoltagesTable[i].m_nominalOutputVoltage = 0;
                }

                m_params.VoltagesTable[i].m_voltageMeasurementType = (CyEVInternalType)CyEnumConverter.GetEnumValue(
                    dataGridView1.Rows[i].Cells[colVoltageMeasurementType.Index].Value, typeof(CyEVInternalType));
                try
                {
                    m_params.VoltagesTable[i].m_uvFaultTreshold = double.Parse(dataGridView1.Rows[i].Cells[
                        colUVFaultThreshold.Index].Value.ToString());
                }
                catch
                {
                    m_params.VoltagesTable[i].m_uvFaultTreshold = 0;
                }

                try
                {
                    m_params.VoltagesTable[i].m_uvWarningTreshold = double.Parse(dataGridView1.Rows[i].Cells[
                        colUVWarningThreshold.Index].Value.ToString());
                }
                catch
                {
                    m_params.VoltagesTable[i].m_uvWarningTreshold = 0;
                }

                try
                {
                    m_params.VoltagesTable[i].m_ovFaultTreshold = double.Parse(dataGridView1.Rows[i].Cells[
                        colOVFaultThreshold.Index].Value.ToString());
                }
                catch
                {
                    m_params.VoltagesTable[i].m_ovFaultTreshold = 0;
                }

                try
                {
                    m_params.VoltagesTable[i].m_ovWarningTreshold = double.Parse(dataGridView1.Rows[i].Cells[
                        colOVWarningThreshold.Index].Value.ToString());
                }
                catch
                {
                    m_params.VoltagesTable[i].m_ovWarningTreshold = 0;
                }

                try
                {
                    m_params.VoltagesTable[i].m_inputScalingFactor = double.Parse(dataGridView1.Rows[i].Cells[
                        colInputScalingFactor.Index].Value.ToString());
                }
                catch
                {
                    m_params.VoltagesTable[i].m_inputScalingFactor = 0;
                }

            }
        }

        private bool NeedSync(int colIndex)
        {
            string colName = dataGridView1.Columns[colIndex].Name;
            return (colName == colConverterName.Name || colName == colVoltageMeasurementType.Name ||
                colName == colNominalOutputVoltage.Name);
        }

        public void SelectDataGridViewRows(int fromRow, int toRow)
        {
            SelectRows(dataGridView1, fromRow, toRow);
        }
        #endregion

        #region Validation
        public void ValidateAllCells()
        {
            bool prev_edit_mode = CyParameters.GlobalEditMode;
            CyParameters.GlobalEditMode = false;
            for (int i = 0; i < dataGridView1.Rows.Count; i++)
            {
                for (int j = 0; j < dataGridView1.Rows[i].Cells.Count; j++)
                {
                    ValidateRow(new DataGridViewCellEventArgs(j, i));
                }
            }
            CyParameters.GlobalEditMode = prev_edit_mode;
        }

        private string RangeCheck(int rowIndex, int colIndex)
        {
            string columnName = dataGridView1.Columns[colIndex].Name;
            double min;
            double max;
            string message = string.Empty;

            if (columnName == colNominalOutputVoltage.Name)
            {
                min = CyParamRanges.NOMINAL_OUTPUT_VOLTAGE_MIN;
                max = CyParamRanges.NOMINAL_OUTPUT_VOLTAGE_MAX;
                message = Resources.NominalOutputVoltageError;
                message = (CyParameters.IsCellValueValid(dataGridView1, rowIndex, colIndex, min, max, message)) ? 
                    string.Empty : string.Format(message, min, max);
            }
            else if (columnName == colUVFaultThreshold.Name)
            {
                min = CyParamRanges.UV_FAULT_TRESHOLD_MIN;
                max = CyParamRanges.UV_FAULT_TRESHOLD_MAX;
                message = Resources.UvFaultthresholdError;
                message = (CyParameters.IsCellValueValid(dataGridView1, rowIndex, colIndex, min, max, message)) ? 
                    string.Empty : string.Format(message, min, max);
            }
            else if (columnName == colUVWarningThreshold.Name)
            {
                min = CyParamRanges.UV_WARNING_TRESHOLD_MIN;
                max = CyParamRanges.UV_WARNING_TRESHOLD_MAX;
                message = Resources.UvWarningthresholdError;
                message = (CyParameters.IsCellValueValid(dataGridView1, rowIndex, colIndex, min, max, message)) ? 
                    string.Empty : string.Format(message, min, max);
            }
            else if (columnName == colOVWarningThreshold.Name)
            {
                min = CyParamRanges.OV_WARNING_TRESHOLD_MIN;
                max = CyParamRanges.OV_WARNING_TRESHOLD_MAX;
                message = Resources.OvWarningthresholdError;
                message = (CyParameters.IsCellValueValid(dataGridView1, rowIndex, colIndex, min, max, message)) ? 
                    string.Empty : string.Format(message, min, max);
            }
            else if (columnName == colOVFaultThreshold.Name)
            {
                min = CyParamRanges.OV_FAULT_TRESHOLD_MIN;
                max = CyParamRanges.OV_FAULT_TRESHOLD_MAX;
                message = Resources.OvFaultthresholdError;
                message = (CyParameters.IsCellValueValid(dataGridView1, rowIndex, colIndex, min, max, message)) ? 
                         string.Empty : string.Format(message, min, max);
            }
            else if (columnName == colInputScalingFactor.Name)
            {
                min = CyParamRanges.INPUT_SCALING_FACTOR_MIN;
                max = CyParamRanges.INPUT_SCALING_FACTOR_MAX;
                message = Resources.InputScalingFactorError;
                message = (CyParameters.IsCellValueValid(dataGridView1, rowIndex, colIndex, min, max, message)) ? 
                    string.Empty : string.Format(message, min, max);
            }

            return message;
        }

        private string SanityCheck(int rowIndex, int colIndex)
        {
            string message = string.Empty;
            try
            {
                double uvFaultThreshold = double.Parse(dataGridView1.Rows[rowIndex].
                    Cells[colUVFaultThreshold.Index].Value.ToString());
                double uvWarningThreshold = double.Parse(dataGridView1.Rows[rowIndex].
                    Cells[colUVWarningThreshold.Index].Value.ToString());
                double nominalOutVoltage = double.Parse(dataGridView1.Rows[rowIndex].
                    Cells[colNominalOutputVoltage.Index].Value.ToString());
                double ovWarningThreshold = double.Parse(dataGridView1.Rows[rowIndex].
                    Cells[colOVWarningThreshold.Index].Value.ToString());
                double ovFaultThreshold = double.Parse(dataGridView1.Rows[rowIndex].
                    Cells[colOVFaultThreshold.Index].Value.ToString());
                double scale = double.Parse(dataGridView1.Rows[rowIndex].
                    Cells[colInputScalingFactor.Index].Value.ToString());

                //UVFault[x] <= UVWarn[x] <= VNom[x] <= OVWarn[x] <= OVFault[x].

                if (dataGridView1.Columns[colIndex].Name == colUVFaultThreshold.Name)
                {
                    // Check UV fault threshold
                    //UVFault[x] <= UVWarn[x]
                    message = Resources.UvFaultSanityCheck;
                    if (uvFaultThreshold > uvWarningThreshold)
                    {
                        dataGridView1.Rows[rowIndex].Cells[colUVFaultThreshold.Index].ErrorText = message;
                        return message;
                    }
                    else
                    {
                        dataGridView1.Rows[rowIndex].Cells[colUVFaultThreshold.Index].ErrorText = string.Empty;
                        message = string.Empty;
                    }

                    //UVFault[x] multiplied by Scale[x] is >= 50 mV
                    message = string.Format(Resources.UvFaultMultScaleSanityCheck, 
                        CyParamRanges.UV_FAULT_MULT_SCALE_MIN);
                    if ((uvFaultThreshold * scale) < CyParamRanges.UV_FAULT_MULT_SCALE_MIN)
                    {
                        dataGridView1.Rows[rowIndex].Cells[colUVFaultThreshold.Index].ErrorText = message;
                        return message;
                    }
                    else
                    {
                        dataGridView1.Rows[rowIndex].Cells[colUVFaultThreshold.Index].ErrorText = string.Empty;
                        message = string.Empty;
                    }
                }
                else if (dataGridView1.Columns[colIndex].Name == colUVWarningThreshold.Name)
                {
                    // Check UV warning threshold
                    //UVWarn[x] <= VNom[x]
                    message = Resources.UvWarningSanityCheck;
                    if (uvWarningThreshold > nominalOutVoltage)
                    {
                        dataGridView1.Rows[rowIndex].Cells[colUVWarningThreshold.Index].ErrorText = message;
                        return message;
                    }
                    else
                    {
                        dataGridView1.Rows[rowIndex].Cells[colUVWarningThreshold.Index].ErrorText = string.Empty;
                        message = string.Empty;
                    }
                }
                else if (dataGridView1.Columns[colIndex].Name == colOVWarningThreshold.Name)
                {
                    // Check OV warning threshold
                    //VNom[x] <= OVWarn[x]
                    message = Resources.OvWarningSanityCheck;
                    if (ovWarningThreshold < nominalOutVoltage)
                    {
                        dataGridView1.Rows[rowIndex].Cells[colOVWarningThreshold.Index].ErrorText = message;
                        return message;
                    }
                    else
                    {
                        dataGridView1.Rows[rowIndex].Cells[colOVWarningThreshold.Index].ErrorText = string.Empty;
                        message = string.Empty;
                    }
                }
                else if (dataGridView1.Columns[colIndex].Name == colOVFaultThreshold.Name)
                {
                    // Check OV fault threshold
                    //OVWarn[x] <= OVFault[x]
                    message = Resources.OvFaultSanityCheck;
                    if (ovFaultThreshold < ovWarningThreshold)
                    {
                        dataGridView1.Rows[rowIndex].Cells[colOVFaultThreshold.Index].ErrorText = message;
                        return message;
                    }
                    else
                    {
                        dataGridView1.Rows[rowIndex].Cells[colOVFaultThreshold.Index].ErrorText = string.Empty;
                        message = string.Empty;
                    }

                    //OVFault[x] multipled by Scale[x] is <= 4 Volts
                    message = string.Format(Resources.OvFaultMultScaleSanityCheck,
                        CyParamRanges.OV_FAULT_MULT_SCALE_MAX);
                    if ((ovFaultThreshold * scale) > CyParamRanges.OV_FAULT_MULT_SCALE_MAX)
                    {
                        dataGridView1.Rows[rowIndex].Cells[colOVFaultThreshold.Index].ErrorText = message;
                        return message;
                    }
                    else
                    {
                        dataGridView1.Rows[rowIndex].Cells[colOVFaultThreshold.Index].ErrorText = string.Empty;
                        message = string.Empty;
                    }
                }
            }
            catch
            {

            }
            return message;
        }
        #endregion
    }
}
