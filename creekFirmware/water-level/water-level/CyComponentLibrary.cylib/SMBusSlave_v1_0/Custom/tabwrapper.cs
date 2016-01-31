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

namespace SMBusSlave_v1_0
{
    public partial class CyTabWrapper : UserControl, ICyParamEditingControl
    {
        #region Header
        protected CyParameters m_params = null;
        protected ErrorProvider m_errorProvider = null;
        protected DataGridView m_wrapperDataGridView = null;
        protected int m_wrapperDataGridKeyColumIndex = 0;

        protected CyToolStrip m_wrapperToolStrip = null;

        public List<int> m_editableCols = new List<int>();

        public virtual string TabName
        {
            get { return ""; }
        }

        public ICyRow DataGridFirstSelectedRow
        {
            get { return GetFirstSelectedRow(m_wrapperDataGridView.SelectedRows); }
        }

        public CyTabWrapper() { }
        public CyTabWrapper(CyParameters packParams)
        {
            m_params = packParams;
            m_errorProvider = new ErrorProvider();
            m_errorProvider.BlinkStyle = ErrorBlinkStyle.NeverBlink;
            this.Load += new EventHandler(this.CyTabControlWrapper_Load);
        }

        private void CyTabControlWrapper_Load(object sender, EventArgs e)
        {
            this.Dock = DockStyle.Fill;
            this.AutoScroll = true;
        }
        #endregion

        #region ICyParamEditingControl Members
        public Control DisplayControl
        {
            get { return this; }
        }

        public IEnumerable<CyCustErr> GetErrors()
        {
            List<CyCustErr> errs = new List<CyCustErr>();
            string errorMessage = string.Empty;

            if (m_errorProvider != null)
            {
                // Check controls for errors
                foreach (Control control in this.Controls)
                {
                    errorMessage = m_errorProvider.GetError(control);
                    if (string.IsNullOrEmpty(errorMessage) == false)
                        errs.Add(new CyCustErr(errorMessage));

                    // Check controls inside groupbox
                    foreach (Control internalControl in control.Controls)
                    {
                        errorMessage = m_errorProvider.GetError(internalControl);
                        if (string.IsNullOrEmpty(errorMessage) == false)
                            errs.Add(new CyCustErr(errorMessage));
                    }
                }
            }

            List<string> dataGridErrors = GetAllDataGridErrors();
            if (dataGridErrors.Count > 0)
            {
                errs.Add(new CyCustErr(string.Format(Resources.DataGridError, TabName)));
            }

            foreach (string paramName in m_params.m_inst.GetParamNames())
            {
                CyCompDevParam param = m_params.m_inst.GetCommittedParam(paramName);
                if (param.TabName.Equals(TabName))
                {
                    if (param.ErrorCount > 0)
                    {
                        foreach (string errMsg in param.Errors)
                        {
                            errs.Add(new CyCustErr(errMsg));
                        }
                    }
                }
            }

            return errs;
        }

        public List<string> GetAllDataGridErrors()
        {
            List<string> errorList = new List<string>();
            if (m_wrapperDataGridView != null)
            {
                for (int i = 0; i < m_wrapperDataGridView.RowCount; i++)
                    for (int j = 0; j < m_wrapperDataGridView.ColumnCount; j++)
                    {
                        if (String.IsNullOrEmpty(m_wrapperDataGridView[j, i].ErrorText) == false)
                        {
                            errorList.Add(m_wrapperDataGridView[j, i].ErrorText);
                        }
                    }
            }
            return errorList;
        }
        #endregion

        #region UpdateToFromTable
        public virtual void UpdateUIFromTable(int row) { }

        public virtual void UpdateTableRowFromUI(int row) { }

        public virtual void UpdateUIFromTable() { }

        public virtual void ValidateCodes() { }
        public virtual void ValidateNames() { }

        #endregion

        #region Validation
        public void ValidateAllTable()
        {
            bool prev_edit_mode = m_params.m_globalEditMode;
            m_params.m_globalEditMode = false;
            for (int i = 0; i < m_wrapperDataGridView.RowCount; i++)
            {
                ValidateRow(i);
            }

            ValidateNames();

            ValidateCodes();

            m_params.m_globalEditMode = prev_edit_mode;
        }

        protected bool ValidateRow(int rowIndex)
        {
            bool isOk = true;

            // Range check
            foreach (int index in m_editableCols)
            {
                isOk &= RangeCheck(rowIndex, index);
            }
            if (isOk)
            {
                isOk &= SanityCheck(rowIndex);
            }
            return isOk;
        }
        protected virtual bool SanityCheck(int rowIndex)
        {
            return true;
        }

        protected virtual bool RangeCheck(int rowIndex, int colIndex)
        {
            return true;
        }

        protected virtual bool UpdateErrorProvider(object sender, string text, int min, int max, string message)
        {
            return UpdateErrorProvider(sender, text, min, max, message, false);
        }

        protected virtual bool UpdateErrorProvider(object sender, string text, int min, int max, string message,
            bool hex)
        {
            int value;
            if (hex)
            {
                try
                {
                    value = Convert.ToInt32(text, 16);
                }
                catch (Exception)
                {
                    value = -1;
                }
            }
            else
            {
                if (int.TryParse(text, out value) == false)
                    value = -1;
            }
            return UpdateErrorProvider(sender, value, min, max, message, hex);
        }

        protected virtual bool UpdateErrorProvider(object sender, double value, int min, int max, string message)
        {
            return UpdateErrorProvider(sender, value, min, max, message, false);
        }

        protected virtual bool UpdateErrorProvider(object sender, double value, int min, int max, string message,
            bool hex)
        {
            string displayMin;
            string displayMax;

            if (hex)
            {
                displayMin = "0x" + min.ToString("X");
                displayMax = "0x" + max.ToString("X");
            }
            else
            {
                displayMin = min.ToString();
                displayMax = max.ToString();
            }

            bool isValid;
            if (value >= min && value <= max)
            {
                m_errorProvider.SetError((Control)sender, string.Empty);
                isValid = true;
            }
            else
            {
                m_errorProvider.SetError((Control)sender, string.Format(message, displayMin, displayMax));
                isValid = false;
            }
            return isValid;
        }
        #endregion

        #region DataGridView
        public IEnumerable<ICyRow> GetObjectsFromDataGrid()
        {
            for (int i = 0; i < m_wrapperDataGridView.RowCount; i++)
            {
                ICyRow row = (ICyRow)m_wrapperDataGridView[m_wrapperDataGridKeyColumIndex, i].Value;

                if (m_wrapperDataGridView[m_wrapperDataGridKeyColumIndex, i].Value != null)
                {
                    yield return row;
                }
            }
        }

        protected void CommitCellValueImmediately(DataGridView dataGridView)
        {
            if (dataGridView.CurrentCell != null)
                if (dataGridView.CurrentCell.GetType() == typeof(DataGridViewCheckBoxCell))
                    if (dataGridView.IsCurrentCellDirty)
                    {
                        dataGridView.CommitEdit(DataGridViewDataErrorContexts.Commit);
                    }
        }

        public void SelectRows(List<ICyRow> rows)
        {
            for (int i = 0; i < m_wrapperDataGridView.Rows.Count; i++)
                for (int k = 0; k < rows.Count; k++)
                    if (m_wrapperDataGridView[m_wrapperDataGridKeyColumIndex, i].Value == rows[k])
                    {
                        m_wrapperDataGridView.Rows[i].Selected = true;
                    }
        }

        protected ICyRow GetFirstSelectedRow(DataGridViewSelectedRowCollection selectedRows)
        {
            // Get minimum value of selected rows
            ICyRow row = null;
            if (selectedRows.Count > 0)
            {
                Dictionary<int, ICyRow> rowDict = new Dictionary<int, ICyRow>();
                for (int i = 0; i < selectedRows.Count; i++)
                    rowDict.Add(selectedRows[i].Index, (ICyRow)selectedRows[i].Cells[
                        m_wrapperDataGridKeyColumIndex].Value);

                int firstSelectedRow = new List<int>(rowDict.Keys)[0];
                foreach (int line in rowDict.Keys)
                    if (line < firstSelectedRow)
                        firstSelectedRow = line;

                row = rowDict[firstSelectedRow];
            }
            return row;
        }
        #endregion

        protected override bool ProcessCmdKey(ref Message msg, Keys keyData)
        {
            if (m_wrapperToolStrip == null)
                return base.ProcessCmdKey(ref msg, keyData);

            bool result = true;

            if (keyData == (Keys.Control | Keys.C))
            {
                if ((m_wrapperDataGridView != null) && (m_wrapperDataGridView.SelectedRows.Count > 0))
                {
                    m_wrapperToolStrip.tsbCopy.PerformClick();
                    result = true;
                }
                else
                    result = base.ProcessCmdKey(ref msg, keyData);
            }
            else if (keyData == (Keys.Control | Keys.V))
            {
                if (m_wrapperDataGridView != null)
                {
                    m_wrapperToolStrip.tsbPaste.PerformClick();
                    result = true;
                }
                else
                    result = base.ProcessCmdKey(ref msg, keyData);
            }
            else if (keyData == (Keys.Control | Keys.S))
            {
                m_wrapperToolStrip.tsbSave.PerformClick();
            }
            else if (keyData == (Keys.Control | Keys.O))
            {
                m_wrapperToolStrip.tsbLoad.PerformClick();
            }
            else if (keyData == (Keys.Control | Keys.M))
            {
                m_wrapperToolStrip.tsbImport.PerformClick();
            }
            else if (keyData == (Keys.Control | Keys.R))
            {
                m_wrapperToolStrip.tsbExport.PerformClick();
            }
            else if (keyData == (Keys.Control | Keys.Alt | Keys.M))
            {
                m_wrapperToolStrip.tsbImportAll.PerformClick();
            }
            else if (keyData == (Keys.Control | Keys.Alt | Keys.R))
            {
                m_wrapperToolStrip.tsbExportAll.PerformClick();
            }
            else
            {
                result = base.ProcessCmdKey(ref msg, keyData);
            }

            return result;
        }
    }
}
