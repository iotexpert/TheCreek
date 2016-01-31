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
using System.Diagnostics;

namespace TrimMargin_v1_10
{
    public partial class CyVoltagesTab : CyEditingWrapperControl
    {
        private int m_dataGridRowCount;
        private List<int> m_editableCols = new List<int>();

        private byte m_previousNumConverters;

        #region Class properties
        public override string TabName
        {
            get { return CyCustomizer.VOLTAGE_TAB_NAME; }
        }

        public int DataGridRowCount
        {
            get { return m_dataGridRowCount; }
            set
            {
                m_dataGridRowCount = value;
                dgvVoltages.RowCount = m_dataGridRowCount;
            }
        }

        public int DataGridFirstSelectedRow
        {
            get { return GetLessSelectedRow(dgvVoltages.SelectedRows); }
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
            cyToolStrip1.m_dgv = dgvVoltages;

            // Initialize wrapper objects
            m_wrapperToolStrip = cyToolStrip1;
            m_wrapperDataGridView = dgvVoltages;

            // Initialize data grid view
            CyParameters.InitializeDataGrid(dgvVoltages);

            m_editableCols = new List<int>(new int[]
            {
                colNominalOutputVoltage.Index,
                colMinVoltage.Index,
                colMaxVoltage.Index,
                colMarginLowPercent .Index,
                colMarginHighPercent.Index,
                colConverterName.Index
            });

            m_errorProvider = new ErrorProvider();
            m_errorProvider.BlinkStyle = ErrorBlinkStyle.NeverBlink;

            numNumConverters.Minimum = 0;
            numNumConverters.Maximum = decimal.MaxValue;

            numNumConverters.TextChanged += new EventHandler(numNumConverters_TextChanged);

            this.Load += delegate(object sender, EventArgs e)
            {
                if (m_params.IsVoltagesTableDefault)
                    m_params.SetVoltagesTable();
            };

            m_previousNumConverters = m_params.NumConverters;
            CyParameters.DGDisabledStyle = dgvVoltages.Columns[0].DefaultCellStyle;
            CyParameters.DGEnabledStyle = dgvVoltages.Columns[1].DefaultCellStyle;
            CyParameters.VoltagesHeader = CyParameters.GetColNames(dgvVoltages);

            UpdateUI();
            // Fill data grid view
            UpdateUIFromTable();
        }
        #endregion

        #region Event Handlers
        public void UpdateNumConvertersControl()
        {
            numNumConverters_TextChanged(numNumConverters, new EventArgs());
        }

        void numNumConverters_TextChanged(object sender, EventArgs e)
        {
            NumericUpDown currentControl = (NumericUpDown)sender;
            byte value = 0;
            bool allowAddingRows = true;
            try
            {
                allowAddingRows = byte.TryParse(currentControl.Text, out value);
                if (allowAddingRows == false) throw new Exception(string.Empty);
                if (value < CyParamRanges.NUM_CONVERTERS_MIN || value > CyParamRanges.NUM_CONVERTERS_MAX)
                {
                    allowAddingRows = false;
                    throw new Exception(string.Format(Resources.NumVoltagesError, CyParamRanges.NUM_CONVERTERS_MIN,
                        CyParamRanges.NUM_CONVERTERS_MAX));
                }
                m_params.NumConverters = value;
                if (value > CyParamRanges.NUM_CONVERTERS_WITH_PWM_RESOLUTION_MAX &&
                    m_params.PWMResolution > CyParamRanges.PWM_RESOLUTION_WITH_NUM_CONVERTERS_MIN)
                {
                    allowAddingRows = true;
                    throw new Exception(string.Format(Resources.NumVoltagesWithPWMResolutionError,
                        CyParamRanges.NUM_CONVERTERS_MIN,
                        CyParamRanges.NUM_CONVERTERS_WITH_PWM_RESOLUTION_MAX,
                        CyParamRanges.PWM_RESOLUTION_WITH_NUM_CONVERTERS_MIN + 1,
                        CyParamRanges.PWM_RESOLUTION_MAX));
                }
                m_errorProvider.SetError(currentControl, string.Empty);
            }
            catch (Exception ex)
            {
                m_errorProvider.SetError(currentControl, ex.Message);
            }

            // Value validated because it could be changed in expression view
            // Do not allow to change table if not edit mode
            if (m_params.GlobalEditMode == false) return;

            // Create new row in Voltages table
            if (allowAddingRows)
            {
                if (m_previousNumConverters >= CyParamRanges.NUM_CONVERTERS_MIN &&
                    m_previousNumConverters <= CyParamRanges.NUM_CONVERTERS_MAX)
                {
                    if (value > m_previousNumConverters)
                    {
                        int missingItemsCount = value - m_params.VoltagesTable.Count;
                        for (int i = 0; i < missingItemsCount; i++)
                        {
                            m_params.VoltagesTable.Add(CyVoltagesTableRow.CreateDefaultRow());
                            m_params.HardwareTable.Add(CyHardwareTableRow.CreateDefaultRow());
                        }
                    }

                    m_params.SetVoltagesTable();
                    m_params.SetHardwareTable();
                    m_params.m_voltagesTab.UpdateUIFromTable();
                    m_params.m_hardwareTab.UpdateUIFromTable();

                    if (value > m_previousNumConverters)
                    {
                        m_params.m_voltagesTab.ValidateAllTable();
                        m_params.m_hardwareTab.ValidateAllTable();
                    }
                }
                m_previousNumConverters = value;
            }
        }
        #endregion

        #region dgvVoltages events
        private void dgvVoltages_CellValueChanged(object sender, DataGridViewCellEventArgs e)
        {
            if (m_params.GlobalEditMode)
            {
                if (m_editableCols.Contains(e.ColumnIndex))
                {
                    UpdateTableFromUI();
                    m_params.SetVoltagesTable();

                    ValidateRow(e.RowIndex);

                    m_params.m_hardwareTab.UpdateUIFromTable(e.RowIndex);
                }
            }
        }

        private void dgvVoltages_CurrentCellDirtyStateChanged(object sender, EventArgs e)
        {
            CommitCellValueImmediately(dgvVoltages);
        }

        private void dgvVoltages_SelectionChanged(object sender, EventArgs e)
        {
            cyToolStrip1.ChangeCopyPasteEnabling(dgvVoltages.SelectedRows.Count > 0);
        }
        #endregion dgvVoltages events

        #region Validation
        public void ValidateAllTable()
        {
            bool prev_edit_mode = m_params.GlobalEditMode;
            m_params.GlobalEditMode = false;
            for (int i = 0; i < m_params.NumConverters; i++)
            {
                ValidateRow(i);
            }
            m_params.GlobalEditMode = prev_edit_mode;
        }

        private void ValidateRow(int rowIndex)
        {
            bool isOk = true;

            // Range check
            foreach (int index in m_editableCols)
            {
                isOk &= RangeCheck(rowIndex, index);
            }
            if (isOk)
            {
                SanityCheck(rowIndex);
            }
        }

        private bool RangeCheck(int rowIndex, int colIndex)
        {
            if (colIndex == colConverterName.Index) return true;

            double min = CyParamRanges.VOLTAGE_MIN;
            double max = CyParamRanges.VOLTAGE_MAX;
            string message = string.Empty;
            double? currCellValue = null;

            if (colIndex == colNominalOutputVoltage.Index)
            {
                message = Resources.NominalOutputVoltageError;
                currCellValue = m_params.VoltagesTable[rowIndex].m_nominalVoltage;
            }
            if (colIndex == colMinVoltage.Index)
            {
                message = Resources.MinVoltageError;
                currCellValue = m_params.VoltagesTable[rowIndex].m_minVoltage;
            }
            if (colIndex == colMaxVoltage.Index)
            {
                message = Resources.MaxVoltageError;
                currCellValue = m_params.VoltagesTable[rowIndex].m_maxVoltage;
            }
            if (colIndex == colMarginLowPercent.Index)
            {
                message = Resources.MarginLowPercentageError;
                min = CyParamRanges.MARGIN_LOW_PERCENT_MIN;
                max = CyParamRanges.MARGIN_LOW_PERCENT_MAX;
                currCellValue = CyParameters.ParseNullableDouble(CyParameters.CellToString(dgvVoltages[colIndex,
                    rowIndex].Value));
            }
            if (colIndex == colMarginHighPercent.Index)
            {
                message = Resources.MarginHighPercentageError;
                min = CyParamRanges.MARGIN_HIGH_PERCENT_MIN;
                max = CyParamRanges.MARGIN_HIGH_PERCENT_MAX;
                currCellValue = CyParameters.ParseNullableDouble(CyParameters.CellToString(dgvVoltages[colIndex,
                    rowIndex].Value));
            }
            if (colIndex == colMarginLow.Index)
            {
                message = Resources.MarginLowVoltageError;
                currCellValue = m_params.VoltagesTable[rowIndex].MarginLow;
            }
            if (colIndex == colMarginHigh.Index)
            {
                message = Resources.MarginHighVoltageError;
                currCellValue = m_params.VoltagesTable[rowIndex].MarginHigh;
            }

            if (m_editableCols.Contains(colIndex) && dgvVoltages.Columns[colIndex].ReadOnly == false)
            {
                message = CyParameters.IsValueInRange(currCellValue, dgvVoltages[colIndex, rowIndex].Value, min, max,
                    message);
            }
            dgvVoltages[colIndex, rowIndex].ErrorText = message;
            return String.IsNullOrEmpty(message);
        }

        private bool SanityCheck(int rowIndex)
        {
            string message = string.Empty;
            try
            {
                double? nominalVoltage = m_params.VoltagesTable[rowIndex].m_nominalVoltage;
                double? minVoltage = m_params.VoltagesTable[rowIndex].m_minVoltage;
                double? maxVoltage = m_params.VoltagesTable[rowIndex].m_maxVoltage;
                double? marginLow = m_params.VoltagesTable[rowIndex].MarginLow;
                double? marginHigh = m_params.VoltagesTable[rowIndex].MarginHigh;

                //VMin[x] <= VMarginLow[x] <= VNom[x] <= VMarginHi[x] <= VMax[x]
                //VMin[x] <= VMarginLow[x]
                dgvVoltages.Rows[rowIndex].Cells[colMinVoltage.Index].ErrorText =
                    CyParameters.ValueNotGreaterThan(minVoltage, marginLow, Resources.MinVoltageSanityCheck);
                //VMarginLow[x] <= VNom[x]
                dgvVoltages.Rows[rowIndex].Cells[colMarginLow.Index].ErrorText =
                    CyParameters.ValueNotGreaterThan(marginLow, nominalVoltage, Resources.MarginHighVoltageSanityCheck);
                //VNom[x] <= VMarginHi[x]
                dgvVoltages.Rows[rowIndex].Cells[colNominalOutputVoltage.Index].ErrorText =
                    CyParameters.ValueNotGreaterThan(nominalVoltage, marginHigh,
                    Resources.NominalOutputVoltageSanityCheck);
                //VMarginHi[x] <= VMax[x]
                dgvVoltages.Rows[rowIndex].Cells[colMarginHigh.Index].ErrorText =
                   CyParameters.ValueNotGreaterThan(marginHigh, maxVoltage, Resources.MarginHighVoltageSanityCheck);
            }
            catch
            {
                Debug.Assert(false);
            }
            return String.IsNullOrEmpty(message);
        }
        #endregion Validation

        public void UpdateUI()
        {
            numNumConverters.Value = m_params.NumConverters;
        }

        public void UpdateUIFromTable()
        {
            bool prevGEM = m_params.GlobalEditMode;
            m_params.GlobalEditMode = false;

            bool dgSizeNeedsUpdate = false;

            m_dataGridRowCount = m_params.NumConverters;
            if (dgvVoltages.Rows.Count != m_params.NumConverters)
                dgSizeNeedsUpdate = true;

            dgvVoltages.Rows.Clear();
            for (int i = 0; i < m_dataGridRowCount; i++)
            {
                dgvVoltages.Rows.Add(new object[] {
                    i+1,
                    m_params.VoltagesTable[i].m_converterName,
                    m_params.VoltagesTable[i].m_nominalVoltage,
                    m_params.VoltagesTable[i].m_minVoltage,
                    m_params.VoltagesTable[i].m_maxVoltage,
                    m_params.VoltagesTable[i].MarginLowPercentage,
                    m_params.VoltagesTable[i].MarginHighPercentage,
                    m_params.VoltagesTable[i].MarginLow,
                    m_params.VoltagesTable[i].MarginHigh
                });
            }
            if (dgSizeNeedsUpdate)
                CyParameters.UpdateDGVHeight(dgvVoltages);

            m_params.GlobalEditMode = prevGEM;
        }

        public void UpdateTableFromUI()
        {
            bool currGEM;
            for (int i = 0; i < m_dataGridRowCount; i++)
            {
                try
                {
                    m_params.VoltagesTable[i].m_converterName =
                        dgvVoltages.Rows[i].Cells[colConverterName.Name].Value.ToString();
                }
                catch
                {
                    m_params.VoltagesTable[i].m_converterName = string.Empty;
                }

                m_params.VoltagesTable[i].m_nominalVoltage = CyParameters.ParseNullableDouble(
                            CyParameters.CellToString(dgvVoltages.Rows[i].Cells[colNominalOutputVoltage.Name].Value));

                m_params.VoltagesTable[i].m_minVoltage = CyParameters.ParseNullableDouble(
                            CyParameters.CellToString(dgvVoltages.Rows[i].Cells[colMinVoltage.Name].Value));

                m_params.VoltagesTable[i].m_maxVoltage = CyParameters.ParseNullableDouble(
                            CyParameters.CellToString(dgvVoltages.Rows[i].Cells[colMaxVoltage.Name].Value));

                double? nominalVoltage = m_params.VoltagesTable[i].m_nominalVoltage;

                // Margin low voltage equal to nominal voltage minus percent from nominal voltage
                double? marginLow;
                try
                {
                    m_params.VoltagesTable[i].MarginLowPercentage = CyParameters.ParseNullableDouble(
                        CyParameters.CellToString(dgvVoltages.Rows[i].Cells[colMarginLowPercent.Name].Value));
                    marginLow = CyVoltagesTableRow.MarginPercentageToVoltage(nominalVoltage,
                        m_params.VoltagesTable[i].MarginLowPercentage);
                }
                catch (Exception)
                {
                    marginLow = null;
                }
                m_params.VoltagesTable[i].MarginLow = marginLow;
                currGEM = m_params.GlobalEditMode;
                m_params.GlobalEditMode = false;
                dgvVoltages.Rows[i].Cells[colMarginLow.Name].Value = marginLow;
                m_params.GlobalEditMode = currGEM;

                // Margin high voltage equal to nominal voltage plus percent from nominal voltage
                double? marginHigh;
                try
                {
                    m_params.VoltagesTable[i].MarginHighPercentage = CyParameters.ParseNullableDouble(
                        CyParameters.CellToString(dgvVoltages.Rows[i].Cells[colMarginHighPercent.Name].Value));
                    marginHigh = CyVoltagesTableRow.MarginPercentageToVoltage(nominalVoltage,
                        m_params.VoltagesTable[i].MarginHighPercentage);
                }
                catch (Exception)
                {
                    marginHigh = null;
                }
                m_params.VoltagesTable[i].MarginHigh = marginHigh;
                currGEM = m_params.GlobalEditMode;
                m_params.GlobalEditMode = false;
                dgvVoltages.Rows[i].Cells[colMarginHigh.Name].Value = marginHigh;
                m_params.GlobalEditMode = currGEM;
            }
        }

        public void SelectDataGridViewRows(int fromRow, int toRow)
        {
            SelectRows(dgvVoltages, fromRow, toRow);
        }
    }
}
