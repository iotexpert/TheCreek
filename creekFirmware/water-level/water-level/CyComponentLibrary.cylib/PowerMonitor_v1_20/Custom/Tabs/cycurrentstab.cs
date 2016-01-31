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

namespace PowerMonitor_v1_20
{
    public partial class CyCurrentsTab : CyEditingWrapperControl
    {
        private int m_dataGridRowCount;
        private List<int> m_sanityCheckCols;
        private List<int> m_rangeCheckCols;

        #region Class properties
        public override string TabName
        {
            get { return "Power Converter Currents"; }
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
        public CyCurrentsTab(CyParameters param)
            : base()
        {
            // Initialize parameters objects
            m_params = param;
            m_params.m_currentsTab = this;

            InitializeComponent();

            // Initialize toolstrip
            cyToolStrip1.SetParameters(param);
            cyToolStrip1.m_dgv = dataGridView1;

            // Initialize wrapper objects
            m_wrapperToolStrip = cyToolStrip1;
            m_wrapperDataGridView = dataGridView1;
            m_currentsTableErrors = new List<string[]>();
            for (int i = 0; i < CyParamRanges.NUM_CONVERTERS_MAX; i++)
                m_currentsTableErrors.Add(new string[dataGridView1.ColumnCount]);

            // Initialize data grid view
            dataGridView1.EditMode = DataGridViewEditMode.EditOnEnter;
            this.colCurrentMeasurementType.Items.AddRange(CyEnumConverter.GetEnumDescList(typeof(
                CyECurrentMeasurementInternalType)));
            dataGridView1.AllowUserToOrderColumns = false;
            dataGridView1.AllowUserToResizeRows = false;
            dataGridView1.AllowUserToAddRows = false;
            foreach (DataGridViewColumn item in dataGridView1.Columns)
                item.SortMode = DataGridViewColumnSortMode.Programmatic;

            m_sanityCheckCols = new List<int>();
            m_sanityCheckCols.Add(colOcWarningThreshold.Index);
            m_sanityCheckCols.Add(colOcFaultThreshold.Index);

            m_rangeCheckCols = new List<int>();
            m_rangeCheckCols.Add(colCurrentMeasurementType.Index);
            m_rangeCheckCols.Add(colOcWarningThreshold.Index);
            m_rangeCheckCols.Add(colOcFaultThreshold.Index);
            m_rangeCheckCols.Add(colShuntResistorValue.Index);
            m_rangeCheckCols.Add(colCsaGain.Index);

            // Fill data grid view
            UpdateUIFromTable();
        }
        #endregion

        #region Event Handlers
        private void dataGridView1_CellValueChanged(object sender, DataGridViewCellEventArgs e)
        {
            if (CyParameters.GlobalEditMode)
            {                
                UpdateTableFromUI();
                m_params.SetCurrentsTable();

                ValidateRow(e);
            }
        }

        private void ValidateRow(DataGridViewCellEventArgs e)
        {
            if (e.ColumnIndex != colConverterName.Index &&
                e.ColumnIndex != colNominalOutputVoltage.Index) // do not check when readonly cells changed
            {
                string errorMessage = string.Empty;

                UpdateRowVisibility(e.RowIndex);

                // Range check
                bool rangeIsValid = true;
                for (int i = 0; i < m_rangeCheckCols.Count; i++)
                {
                    errorMessage = RangeCheck(e.RowIndex, m_rangeCheckCols[i]);
                    if (errorMessage != string.Empty)
                        rangeIsValid = false;
                    m_currentsTableErrors[e.RowIndex].SetValue(errorMessage, m_rangeCheckCols[i]);
                }
                if (rangeIsValid)
                {
                    // Range is OK. Do sanity check
                    for (int i = 0; i < m_sanityCheckCols.Count; i++)
                    {
                        errorMessage = SanityCheck(e.RowIndex, m_sanityCheckCols[i]);
                        m_currentsTableErrors[e.RowIndex].SetValue(errorMessage, m_sanityCheckCols[i]);
                    }
                }
            }
        } 

        private void dataGridView1_CurrentCellDirtyStateChanged(object sender, EventArgs e)
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
            string currentMeasurementType;

            m_dataGridRowCount = m_params.NumConverters;
            dataGridView1.Rows.Clear();

            for (int i = 0; i < m_dataGridRowCount; i++)
            {
                if (m_params.VoltagesTable[i].m_voltageMeasurementType == CyEVInternalType.Differential)
                {
                    currentMeasurementType = CyEnumConverter.GetEnumDesc(CyECurrentMeasurementInternalType.None);
                }
                else
                {
                    currentMeasurementType = CyEnumConverter.GetEnumDesc(
                        m_params.CurrentsTable[i].m_currentMeasurementType);
                }

                object[] row = new object[] { 
                    CyCurrentsTableRow.GetPowerConverterNumber(i), 
                    m_params.VoltagesTable[i].m_converterName, 
                    m_params.VoltagesTable[i].m_nominalOutputVoltage, 
                    currentMeasurementType, 
                    m_params.CurrentsTable[i].m_ocWarningThreshold,
                    m_params.CurrentsTable[i].m_ocFaulthTreshold,
                    m_params.CurrentsTable[i].m_shuntResistorValue,
                    m_params.CurrentsTable[i].m_csaGain,
                };

                dataGridView1.Rows.Add(row);

                UpdateRowVisibility(i);
            }
        }

        public void UpdateTableFromUI()
        {
            for (int i = 0; i < m_dataGridRowCount; i++)
            {
                m_params.CurrentsTable[i].m_currentMeasurementType = (CyECurrentMeasurementInternalType)
                    CyEnumConverter.GetEnumValue(dataGridView1.Rows[i].Cells[colCurrentMeasurementType.Index].Value,
                    typeof(CyECurrentMeasurementInternalType));

                m_params.CurrentsTable[i].m_ocWarningThreshold = CyParameters.ParseNullableDouble(dataGridView1.Rows[i].Cells[
                        colOcWarningThreshold.Index].Value);


                m_params.CurrentsTable[i].m_ocFaulthTreshold = CyParameters.ParseNullableDouble(dataGridView1.Rows[i].Cells[
                    colOcFaultThreshold.Index].Value);


                m_params.CurrentsTable[i].m_shuntResistorValue = CyParameters.ParseNullableDouble(dataGridView1.Rows[i].Cells[
                    colShuntResistorValue.Index].Value);
                m_params.CurrentsTable[i].m_csaGain = CyParameters.ParseNullableDouble(dataGridView1.Rows[i].Cells[
                    colCsaGain.Index].Value);
            }
        }

        void UpdateRowVisibility(int row)
        {
            bool readOnly=false;
            //Any converter that was set to VType = Differential in the voltages tab forfeits 
            //the capability to measure current. The associated row in this table will be grayed out and
            //current measurement type column entry will set to "None"
            CyParameters.SetCellReadOnlyState(dataGridView1, row, colCurrentMeasurementType.Index,
                m_params.VoltagesTable[row].m_voltageMeasurementType == CyEVInternalType.Differential);

            //CSAGainp is grayed out if the associated IType[x] is set to None or Direct.
            CyParameters.SetCellReadOnlyState(dataGridView1, row, colCsaGain.Index,
                m_params.CurrentsTable[row].m_currentMeasurementType != CyECurrentMeasurementInternalType.CSA);

            //This entry is grayed out if the associated IType[x] is set to None ot WarningSourcesOC is false. 
            readOnly = !m_params.WarningSourcesOC ||
                m_params.CurrentsTable[row].m_currentMeasurementType == CyECurrentMeasurementInternalType.None;
            CyParameters.SetCellReadOnlyState(dataGridView1, row, colOcWarningThreshold.Index, readOnly);

            //This entry is grayed out if the associated IType[x] is set to None or FaultSourcesOC is false. 
            readOnly = !m_params.FaultSourcesOC || 
                m_params.CurrentsTable[row].m_currentMeasurementType == CyECurrentMeasurementInternalType.None;
            CyParameters.SetCellReadOnlyState(dataGridView1, row, colOcFaultThreshold.Index, readOnly);

            //This entry is grayed out if the associated IType[x] is set to None. 
            CyParameters.SetCellReadOnlyState(dataGridView1, row, colShuntResistorValue.Index,
                m_params.CurrentsTable[row].m_currentMeasurementType == CyECurrentMeasurementInternalType.None);
        }

        public void SelectDataGridViewRows(int fromRow, int toRow)
        {
            SelectRows(dataGridView1, fromRow, toRow);
        }

        public void  UpdateColumnsState()
        {
            for (int i = 0; i < dataGridView1.RowCount; i++)
                UpdateRowVisibility(i);
        }
        #endregion

        #region Validation
        private string RangeCheck(int rowIndex, int colIndex)
        {
            string columnName = dataGridView1.Columns[colIndex].Name;
            double min;
            double max;
            string message = string.Empty;

            if (columnName == colOcWarningThreshold.Name)
            {
                min = CyParamRanges.OC_WARNING_TRESHOLD_MIN;
                max = CyParamRanges.OC_WARNING_TRESHOLD_MAX;
                message = Resources.OcWarningThresholdError;
                message = (CyParameters.IsCellValueValid(dataGridView1, rowIndex, colIndex, min, max, message)) ?
                string.Empty : string.Format(message, min, max);
            }
            else if (columnName == colOcFaultThreshold.Name)
            {
                min = CyParamRanges.OC_FAULT_TRESHOLD_MIN;
                max = CyParamRanges.OC_FAULT_TRESHOLD_MAX;
                message = Resources.OcFaultThresholdError;
                message = (CyParameters.IsCellValueValid(dataGridView1, rowIndex, colIndex, min, max, message)) ?
                string.Empty : string.Format(message, min, max);
            }
            else if (columnName == colShuntResistorValue.Name)
            {
                min = CyParamRanges.SHUNT_RESISTOR_MIN;
                max = CyParamRanges.SHUNT_RESISTOR_MAX;
                message = Resources.ShuntResistorError;
                message = (CyParameters.IsCellValueValid(dataGridView1, rowIndex, colIndex, min, max, message)) ?
                    string.Empty : string.Format(message, min, max);
            }
            else if (columnName == colCsaGain.Name)
            {
                min = CyParamRanges.CSA_GAIN_MIN;
                max = CyParamRanges.CSA_GAIN_MAX;
                message = Resources.CsaGainError;
                message = (CyParameters.IsCellValueValid(dataGridView1, rowIndex, colIndex, min, max, message)) ?
                  string.Empty : string.Format(message, min, max);
            }

            return message;
        }

        private string SanityCheck(int row, int colIndex)
        {
            string message = string.Empty;

            //Do nothing if Measurement Type is None
            if (m_params.CurrentsTable[row].m_currentMeasurementType == CyECurrentMeasurementInternalType.None)
                return message;

            try
            {
                double ocWarningThreshold = double.Parse(dataGridView1.Rows[row].Cells[
                    colOcWarningThreshold.Index].Value.ToString());
                double ocFaultThreshold = double.Parse(dataGridView1.Rows[row].Cells[
                    colOcFaultThreshold.Index].Value.ToString());
                double shunt = double.Parse(dataGridView1.Rows[row].Cells[
                    colShuntResistorValue.Index].Value.ToString());

                double csagain = 0;
                if (m_params.CurrentsTable[row].m_currentMeasurementType == CyECurrentMeasurementInternalType.CSA)
                    csagain = double.Parse(dataGridView1.Rows[row].Cells[colCsaGain.Index].Value.ToString());

                if (dataGridView1.Columns[colIndex].Name == colOcWarningThreshold.Name)
                {
                    //OCWarn[x] <= OCFault[x]
                    if (ocWarningThreshold > ocFaultThreshold)
                    {
                        message = Resources.OcWarningThresholdSanityCheck;
                        dataGridView1.Rows[row].Cells[colOcWarningThreshold.Index].ErrorText = message;
                    }
                    else
                    {
                        message = string.Empty;
                        dataGridView1.Rows[row].Cells[colOcWarningThreshold.Index].ErrorText = message;
                    }
                }
                else if (dataGridView1.Columns[colIndex].Name == colOcFaultThreshold.Name)
                {
                    message = string.Empty;
                    if (m_params.CurrentsTable[row].m_currentMeasurementType == 
                        CyECurrentMeasurementInternalType.Direct)
                    {
                        if ((ocFaultThreshold * shunt) > CyParamRanges.AdcRange)
                            message = string.Format(Resources.OcFaultThresholdSanityCheck, CyParamRanges.AdcRange);
                    }
                    else if (m_params.CurrentsTable[row].m_currentMeasurementType == 
                        CyECurrentMeasurementInternalType.CSA)
                    {
                        if ((ocFaultThreshold * shunt * csagain) > CyParamRanges.ADC_RANGE_CSA_CHECK)
                            message = Resources.OcFaultThresholdCSASanityCheck;
                    }

                    dataGridView1.Rows[row].Cells[colOcFaultThreshold.Index].ErrorText = message;
                }
            }
            catch { }
            return message;
        }

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
        #endregion
    }
}
