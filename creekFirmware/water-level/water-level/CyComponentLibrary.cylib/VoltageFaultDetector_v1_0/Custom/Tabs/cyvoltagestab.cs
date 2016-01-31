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

namespace VoltageFaultDetector_v1_0
{
    public partial class CyVoltagesTab : CyEditingWrapperControl
    {
        private int m_dataGridRowCount;
        private DataGridViewCellStyle m_disabledStyle;

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

        public int DataGridActiveRowIndex
        {
            get { return dgvVoltages.CurrentRow.Index; }
        }

        public string ColNamelInputScalingFactor
        {
            get { return colInputScalingFactor.Name; }
        }

        public string ColNamelUVFaultThreshold
        {
            get { return colUVFaultThreshold.Name; }
        }

        public string ColNamelOVFaultThreshold
        {
            get { return colOVFaultThreshold.Name; }
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

            m_disabledStyle = new DataGridViewCellStyle();
            m_disabledStyle.BackColor = SystemColors.ControlLight;


            // Initialize toolstrip
            cyToolStrip1.SetParameters(param);
            cyToolStrip1.m_dgv = dgvVoltages;

            // Initialize wrapper objects
            m_wrapperToolStrip = cyToolStrip1;
            m_wrapperDataGridView = dgvVoltages;
            //m_voltagesTableErrors = new List<string[]>();
            //for (int i = 0; i < CyParamRanges.NUM_VOLTAGES_MAX; i++)
            //    m_voltagesTableErrors.Add(new string[dgvVoltages.ColumnCount]);

            // Initialize data grid view
            dgvVoltages.EditMode = DataGridViewEditMode.EditOnEnter;

            // Prevent data grid from sorting
            foreach (DataGridViewColumn item in dgvVoltages.Columns)
                item.SortMode = DataGridViewColumnSortMode.NotSortable;

            // Fill data grid view
            UpdateUIFromTable();
        }
        #endregion

        #region dgvVoltages events
        private void dgvVoltages_CellValueChanged(object sender, DataGridViewCellEventArgs e)
        {
            if ((e.ColumnIndex >= 0) && (e.RowIndex >= 0) && dgvVoltages[e.ColumnIndex, e.RowIndex].ReadOnly)
                return;

            if (m_params.GlobalEditMode)
            {
                UpdateTableFromUI();
                m_params.SetVoltagesTable();

                if ((e.ColumnIndex == colNominalVoltage.Index) || (e.ColumnIndex == colUVFaultThreshold.Index) ||
                    (e.ColumnIndex == colOVFaultThreshold.Index) || (e.ColumnIndex == colInputScalingFactor.Index))
                {
                    ValidateRow(e.RowIndex);
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

        private void dgvVoltages_CellFormatting(object sender, DataGridViewCellFormattingEventArgs e)
        {
            if ((e.ColumnIndex == colNominalVoltage.Index) || (e.ColumnIndex == colUVFaultThreshold.Index) ||
                (e.ColumnIndex == colOVFaultThreshold.Index) || (e.ColumnIndex == colInputScalingFactor.Index))
            {
                try
                {
                    string format = (e.ColumnIndex == colInputScalingFactor.Index) ? "N3" : "N2";
                    e.Value = Double.Parse(e.Value.ToString()).ToString(format);
                }
                catch
                {
                }
            }
        }
        #endregion dgvVoltages events

        #region Validation
        public bool ValidateAllTable()
        {
            bool prev_edit_mode = m_params.GlobalEditMode;
            m_params.GlobalEditMode = false;
            bool isOk = true;
            for (int i = 0; i < m_params.NumVoltages; i++)
            {
                isOk &= ValidateRow(i);
            }
            m_params.GlobalEditMode = prev_edit_mode;
            return isOk;
        }

        private bool ValidateRow(int rowIndex)
        {
            bool isOk = true;
            // Range check
            isOk &= RangeCheck(rowIndex, colNominalVoltage.Index);
            isOk &= RangeCheck(rowIndex, colUVFaultThreshold.Index);
            isOk &= RangeCheck(rowIndex, colOVFaultThreshold.Index);
            isOk &= RangeCheck(rowIndex, colInputScalingFactor.Index);
            if (isOk)
            {
                isOk = SanityCheck(rowIndex);
                //m_voltagesTableErrors[e.RowIndex].SetValue(errorMessage, e.ColumnIndex); //  fix column
            }
            else
            {
               // m_voltagesTableErrors[e.RowIndex].SetValue(errorMessage, e.ColumnIndex);
            }
            return isOk;
        }

        private bool RangeCheck(int rowIndex, int colIndex)
        {
            double min;
            double max;
            string message = string.Empty;

            if (colIndex == colNominalVoltage.Index)
            {
                min = CyParamRanges.NOMINAL_VOLTAGE_MIN;
                max = CyParamRanges.NOMINAL_VOLTAGE_MAX;
                message = Resources.NominalOutputVoltageError;
                message = (IsValueInRange(rowIndex, colIndex, min, max, message)) ? 
                    string.Empty : string.Format(message, min, max);
            }
            else if ((colIndex == colUVFaultThreshold.Index) && (colUVFaultThreshold.ReadOnly == false))
            {
                min = CyParamRanges.UV_FAULT_THRESHOLD_MIN;
                max = CyParamRanges.UV_FAULT_THRESHOLD_MAX;
                message = Resources.UvFaultThresholdError;
                message = (IsValueInRange(rowIndex, colIndex, min, max, message)) ? 
                    string.Empty : string.Format(message, min, max);
            }
            else if ((colIndex == colOVFaultThreshold.Index) && (colOVFaultThreshold.ReadOnly == false))
            {
                min = CyParamRanges.OV_FAULT_THRESHOLD_MIN;
                max = CyParamRanges.OV_FAULT_THRESHOLD_MAX;
                message = Resources.OvFaultThresholdError;
                message = (IsValueInRange(rowIndex, colIndex, min, max, message)) ? 
                    string.Empty : string.Format(message, min, max);
            }
            else if ((colIndex == colInputScalingFactor.Index) && (colInputScalingFactor.ReadOnly == false))
            {
                min = CyParamRanges.INPUT_SCALING_FACTOR_MIN;
                max = CyParamRanges.INPUT_SCALING_FACTOR_MAX;
                message = Resources.InputScalingFactorError;
                message = (IsValueInRange(rowIndex, colIndex, min, max, message)) ? 
                    string.Empty : string.Format(message, min, max);
            }

            return String.IsNullOrEmpty(message);
        }

        private bool IsValueInRange(int rowIndex, int colIndex, double min, double max, string errorMessage)
        {
            bool isCellValid = false;
            string message = string.Format(errorMessage, min, max);
            try
            {
                double? currCellValue = null;
                if (colIndex == colNominalVoltage.Index)
                    currCellValue = m_params.VoltagesTable[rowIndex].m_nominalVoltage;
                else if (colIndex == colUVFaultThreshold.Index)
                    currCellValue = m_params.VoltagesTable[rowIndex].m_uvFaultThreshold;
                else if (colIndex == colOVFaultThreshold.Index)
                    currCellValue = m_params.VoltagesTable[rowIndex].m_ovFaultThreshold;
                else if (colIndex == colInputScalingFactor.Index)
                    currCellValue = m_params.VoltagesTable[rowIndex].m_inputScalingFactor;

                if ((currCellValue.HasValue) && (currCellValue < min || currCellValue > max))
                {
                    throw new Exception();
                }
                else if ((currCellValue == null) && (dgvVoltages[colIndex, rowIndex].ReadOnly == false) &&
                         (IsCellEmpty(colIndex, rowIndex) == false))
                {
                    throw new Exception();
                }
                else
                {
                    dgvVoltages[colIndex, rowIndex].ErrorText = string.Empty;
                    isCellValid = true;
                }
            }
            catch (Exception)
            {
                dgvVoltages[colIndex, rowIndex].ErrorText = message;
            }
            return isCellValid;
        }

        private bool SanityCheck(int rowIndex)
        {
            double eps = 0.000001;
            string message = string.Empty;
            bool check1bPassed = true;
            try
            {
                double? uvFaultThreshold = m_params.VoltagesTable[rowIndex].m_uvFaultThreshold;
                double? nominalVoltage = m_params.VoltagesTable[rowIndex].m_nominalVoltage;
                double? ovFaultThreshold = m_params.VoltagesTable[rowIndex].m_ovFaultThreshold;
                double? scale = m_params.VoltagesTable[rowIndex].m_inputScalingFactor;

                if (IsColumnDisabled(colInputScalingFactor.Name))
                    scale = 1;

                #region Check1a: UVFault[x] <= VNom[x] <= OVFault[x]
                {
                    message = Resources.VoltagesCompareSanityCheck;

                    if (nominalVoltage.HasValue)
                    {
                        if ((uvFaultThreshold.HasValue && !IsColumnDisabled(colUVFaultThreshold.Name) && 
                            (nominalVoltage < uvFaultThreshold - eps)) ||
                            (ovFaultThreshold.HasValue && !IsColumnDisabled(colOVFaultThreshold.Name) &&
                            (nominalVoltage > ovFaultThreshold + eps)))
                        {
                            dgvVoltages.Rows[rowIndex].Cells[colNominalVoltage.Index].ErrorText = message;
                        }
                    }
                    else
                    {
                        dgvVoltages.Rows[rowIndex].Cells[colNominalVoltage.Index].ErrorText = string.Empty;
                        message = string.Empty;
                    }
                }
                #endregion Check1a

                #region Check1b: UVFault[x] <= OVFault[x]
                {
                    message = Resources.VoltagesCompareSanityCheck;

                    if (!nominalVoltage.HasValue && uvFaultThreshold.HasValue && ovFaultThreshold.HasValue)
                    {
                        message = Resources.OvUvVoltagesCompareSanityCheck;

                        if ((!IsColumnDisabled(colUVFaultThreshold.Name) && !IsColumnDisabled(colOVFaultThreshold.Name)
                            && (uvFaultThreshold > ovFaultThreshold + eps)))
                        {
                            dgvVoltages.Rows[rowIndex].Cells[colUVFaultThreshold.Index].ErrorText = message;
                            check1bPassed = false;
                        }
                    }
                    else
                    {
                        dgvVoltages.Rows[rowIndex].Cells[colUVFaultThreshold.Index].ErrorText = string.Empty;
                        message = string.Empty;
                    }
                }
                #endregion Check1b
                
                #region Check2: OVFault[x] multipled by Scale[x] is <= DACRange when ExtRef==False
                {
                    message = Resources.OvFaultMultScaleSanityCheck;
                    double dacRange = (m_params.DACRange == CyDACRangeType.V1) ? 1.024 : 4.096;
                    if ((m_params.ExternalRef == false) && ovFaultThreshold.HasValue && scale.HasValue &&
                        ((ovFaultThreshold * scale) > dacRange + eps))
                    {
                        dgvVoltages.Rows[rowIndex].Cells[colOVFaultThreshold.Index].ErrorText = message;
                    }
                    else
                    {
                        dgvVoltages.Rows[rowIndex].Cells[colOVFaultThreshold.Index].ErrorText = string.Empty;
                        message = string.Empty;
                    }
                }
                #endregion Check2

                #region Check3: UVFault[x] multiplied by Scale[x] is >= 50 mV
                if (check1bPassed)
                {
                    message = Resources.UvFaultMultScaleSanityCheck;
                    if (uvFaultThreshold.HasValue && scale.HasValue && ((uvFaultThreshold * scale) < 0.05 - eps))
                    {
                        dgvVoltages.Rows[rowIndex].Cells[colUVFaultThreshold.Index].ErrorText = message;
                    }
                    else
                    {
                        dgvVoltages.Rows[rowIndex].Cells[colUVFaultThreshold.Index].ErrorText = string.Empty;
                        message = string.Empty;
                    }
                }
                #endregion Check3
            }
            catch
            {
                Debug.Assert(false);
            }
            return String.IsNullOrEmpty(message);
        }

        public List<string> GetAllDataGridErrors()
        {
            List<string> errorList = new List<string>();
            for (int i = 0; i < dgvVoltages.RowCount; i++)
                for (int j = 0; j < dgvVoltages.ColumnCount; j++)
                {
                    if (String.IsNullOrEmpty(dgvVoltages[j, i].ErrorText) == false)
                    {
                        errorList.Add(dgvVoltages[j, i].ErrorText);
                    }
                }
            return errorList;
        }
        #endregion Validation

        #region Helper functions
        public void UpdateUIFromTable()
        {
            bool prev_edit_mode = m_params.GlobalEditMode;
            m_params.GlobalEditMode = false;
            m_dataGridRowCount = m_params.NumVoltages;
            dgvVoltages.Rows.Clear();
            for (int i = 0; i < m_dataGridRowCount; i++)
            {
                dgvVoltages.Rows.Add(new object[] { 
                    CyVoltagesTableRow.GetVoltageIndexStr(i+1), 
                    m_params.VoltagesTable[i].m_voltageName, 
                    m_params.VoltagesTable[i].m_nominalVoltage, 
                    m_params.VoltagesTable[i].m_uvFaultThreshold,
                    m_params.VoltagesTable[i].m_ovFaultThreshold,
                    m_params.VoltagesTable[i].m_inputScalingFactor
                });
            }
            SetColumnsStyle();
            m_params.GlobalEditMode = prev_edit_mode;
        }

        public void UpdateTableFromUI()
        {
            for (int i = 0; i < m_dataGridRowCount; i++)
            {
                m_params.VoltagesTable[i].m_voltageName = 
                            CyParameters.CellToString(dgvVoltages.Rows[i].Cells[colVoltageName.Name].Value);

                m_params.VoltagesTable[i].m_nominalVoltage = CyParameters.ParseNullableDouble(
                            CyParameters.CellToString(dgvVoltages.Rows[i].Cells[colNominalVoltage.Name].Value));

                m_params.VoltagesTable[i].m_uvFaultThreshold = CyParameters.ParseNullableDouble(
                            CyParameters.CellToString(dgvVoltages.Rows[i].Cells[colUVFaultThreshold.Name].Value));

                m_params.VoltagesTable[i].m_ovFaultThreshold = CyParameters.ParseNullableDouble(
                            CyParameters.CellToString(dgvVoltages.Rows[i].Cells[colOVFaultThreshold.Name].Value));
               
                m_params.VoltagesTable[i].m_inputScalingFactor = CyParameters.ParseNullableDouble(
                            CyParameters.CellToString(dgvVoltages.Rows[i].Cells[colInputScalingFactor.Name].Value));
            }
        }

        #region Disabling columns
        public void SetColumnsStyle()
        {
            bool prev_edit_mode = m_params.GlobalEditMode;
            m_params.GlobalEditMode = false;

            switch (m_params.CompareType)
            {
                case CyCompareType.OV:
                    m_params.m_voltagesTab.DisableColumn(m_params.m_voltagesTab.ColNamelUVFaultThreshold);
                    m_params.m_voltagesTab.EnableColumn(m_params.m_voltagesTab.ColNamelOVFaultThreshold);
                    break;
                case CyCompareType.UV:
                    m_params.m_voltagesTab.DisableColumn(m_params.m_voltagesTab.ColNamelOVFaultThreshold);
                    m_params.m_voltagesTab.EnableColumn(m_params.m_voltagesTab.ColNamelUVFaultThreshold);
                    break;
                case CyCompareType.OV_UV:
                    m_params.m_voltagesTab.EnableColumn(m_params.m_voltagesTab.ColNamelOVFaultThreshold);
                    m_params.m_voltagesTab.EnableColumn(m_params.m_voltagesTab.ColNamelUVFaultThreshold);
                    break;
            }

            if (m_params.ExternalRef)
            {
                m_params.m_voltagesTab.DisableColumn(m_params.m_voltagesTab.ColNamelInputScalingFactor);
            }
            else
            {
                m_params.m_voltagesTab.EnableColumn(m_params.m_voltagesTab.ColNamelInputScalingFactor);
            }
            m_params.m_voltagesTab.ValidateAllTable();
            m_params.GlobalEditMode = prev_edit_mode;
        }

        private bool IsColumnDisabled(string colName)
        {
            return (dgvVoltages.Columns[colName].ReadOnly);
        }

        private void DisableColumn(string colName)
        {
            dgvVoltages.Columns[colName].DefaultCellStyle = m_disabledStyle;
            dgvVoltages.Columns[colName].ReadOnly = true;

            string cellValue = "---";
            if (colName == colInputScalingFactor.Name)
            {
                cellValue = "1.000";
            }
            for (int i = 0; i < dgvVoltages.RowCount; i++)
            {
                dgvVoltages.Rows[i].Cells[colName].Value = cellValue;
            }
        }

        private void EnableColumn(string colName)
        {
            dgvVoltages.Columns[colName].DefaultCellStyle = dgvVoltages.DefaultCellStyle.Clone();
            dgvVoltages.Columns[colName].ReadOnly = false;

            for (int i = 0; i < dgvVoltages.RowCount; i++)
            {
                object cellValue = 0;
                if (colName == colUVFaultThreshold.Name)
                {
                    cellValue = m_params.VoltagesTable[i].m_uvFaultThreshold;
                    dgvVoltages.Columns[colName].DefaultCellStyle.Format = "N2";
                }
                else if (colName == colOVFaultThreshold.Name)
                {
                    cellValue = m_params.VoltagesTable[i].m_ovFaultThreshold;
                    dgvVoltages.Columns[colName].DefaultCellStyle.Format = "N2";
                }
                else if (colName == colInputScalingFactor.Name)
                {
                    cellValue = m_params.VoltagesTable[i].m_inputScalingFactor;
                    dgvVoltages.Columns[colName].DefaultCellStyle.Format = "N3";
                }
                dgvVoltages.Rows[i].Cells[colName].Value = cellValue;
            }
        }

        private void ApplyDisabledColValues()
        {
            if (dgvVoltages.Columns[colUVFaultThreshold.Index].ReadOnly)
            {
                for (int i = 0; i < m_params.NumVoltages; i++)
                {
                    m_params.VoltagesTable[i].m_uvFaultThreshold = null;
                }
            }
            if (dgvVoltages.Columns[colOVFaultThreshold.Index].ReadOnly)
            {
                for (int i = 0; i < m_params.NumVoltages; i++)
                {
                    m_params.VoltagesTable[i].m_ovFaultThreshold = null;
                }
            }
            if (dgvVoltages.Columns[colInputScalingFactor.Index].ReadOnly)
            {
                for (int i = 0; i < m_params.NumVoltages; i++)
                {
                    m_params.VoltagesTable[i].m_inputScalingFactor = 1;
                }
            }
        }
        #endregion Disabling columns

        public List<string> GetVoltageColNames()
        {
            List<string> colNames = new List<string>();
            for (int i = 0; i < dgvVoltages.ColumnCount; i++)
            {
                colNames.Add(dgvVoltages.Columns[i].HeaderText);
            }
            return colNames;
        }

        private bool IsCellEmpty(int colIndex, int rowIndex)
        {
            if ((dgvVoltages[colIndex, rowIndex].Value == null) || 
                    ((dgvVoltages[colIndex, rowIndex].Value != null) && 
                    (String.IsNullOrEmpty(dgvVoltages[colIndex, rowIndex].Value.ToString()))))
            {
                return true;
            }
            return false;
        }

        public void SelectRow(int row)
        {
            if ((row >= 0) && (row < dgvVoltages.RowCount))
            {
                dgvVoltages.Rows[row].Selected = true;
                dgvVoltages.CurrentCell = dgvVoltages[0, row];
            }
        }
        #endregion Helper functions

        #region GetErrors()
        public override IEnumerable<CyCustErr> GetErrors()
        {
            List<CyCustErr> errs = new List<CyCustErr>(base.GetErrors());

            List<string> dataGridErrors = GetAllDataGridErrors();
            if (dataGridErrors.Count > 0)
            {
                errs.Add(new CyCustErr(Resources.DataGridError));
            }
            else
            {
                ApplyDisabledColValues();
            }

            return errs;
        }
        #endregion GetErrors()
    }
}
