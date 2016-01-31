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

namespace TrimMargin_v1_10
{
    public class CyEditingWrapperControl : UserControl, ICyParamEditingControl
    {
        public static bool m_runMode = false;
        protected CyParameters m_params = null;
        protected ErrorProvider m_errorProvider = null;
        protected DataGridView m_wrapperDataGridView = null;
        protected CyToolStrip m_wrapperToolStrip = null;

        public virtual string TabName
        {
            get { return "Empty"; }
        }

        public CyEditingWrapperControl()
        {
            this.Load += new EventHandler(CyEditingWrapperControl_Load);
        }

        void CyEditingWrapperControl_Load(object sender, EventArgs e)
        {
            if (m_runMode)
            {
                this.Dock = DockStyle.Fill;
                this.AutoScroll = true;
            }
        }

        protected void UpdateErrorList(bool isCellValid, List<CyCustErr> errorList, CyCustErr errObj)
        {
            if (isCellValid)
            {
                if (errorList.Contains(errObj))
                    errorList.Remove(errObj);
            }
            else
            {
                if (errorList.Contains(errObj) == false)
                    errorList.Add(errObj);
            }
        }

        #region Data grid methods
        protected void CommitCellValueImmediately(DataGridView dataGridView)
        {
            if (dataGridView.CurrentCell != null)
                if (dataGridView.CurrentCell.GetType() == typeof(DataGridViewCheckBoxCell))
                    if (dataGridView.IsCurrentCellDirty)
                    {
                        dataGridView.CommitEdit(DataGridViewDataErrorContexts.Commit);
                    }
        }

        protected int GetLessSelectedRow(DataGridViewSelectedRowCollection selectedRows)
        {
            // Get minimum value of selected rows
            int firstSelectedRow = 0;
            if (selectedRows.Count > 0)
            {
                int[] tmp = new int[selectedRows.Count];
                for (int i = 0; i < selectedRows.Count; i++)
                    tmp[i] = selectedRows[i].Index;

                firstSelectedRow = tmp[0];
                for (int i = 1; i < tmp.Length; i++)
                    if (tmp[i] < firstSelectedRow)
                        firstSelectedRow = tmp[i];
            }
            return firstSelectedRow;
        }

        protected void SelectRows(DataGridView dataGridView, int fromRow, int toRow)
        {
            if (fromRow >= 0 && toRow >= 0)
            {
                dataGridView.CurrentCell = dataGridView.Rows[toRow].Cells[0];
                for (int i = 0; i < dataGridView.Rows.Count; i++)
                {
                    if (i >= fromRow && i <= toRow)
                        dataGridView.Rows[i].Selected = true;
                }
            }
        }

        protected override bool ProcessCmdKey(ref Message msg, Keys keyData)
        {
            if (m_wrapperToolStrip == null || m_wrapperDataGridView == null)
                return base.ProcessCmdKey(ref msg, keyData);

            bool result = true;

            if (keyData == (Keys.Control | Keys.C))
            {
                if (m_wrapperDataGridView.SelectedRows.Count > 0)
                {
                    m_wrapperToolStrip.CopyRows();
                    result = true;
                }
                else
                    result = base.ProcessCmdKey(ref msg, keyData);
            }
            else if (keyData == (Keys.Control | Keys.V))
            {
                if (m_wrapperDataGridView.SelectedRows.Count > 0)
                {
                    m_wrapperToolStrip.PasteRows();
                    result = true;
                }
                else
                    result = base.ProcessCmdKey(ref msg, keyData);
            }
            else if (keyData == (Keys.Control | Keys.M))
            {
                m_wrapperToolStrip.ImportRows();
            }
            else if (keyData == (Keys.Control | Keys.R))
            {
                m_wrapperToolStrip.ExportRows();
            }
            else if (keyData == (Keys.Control | Keys.Alt | Keys.M))
            {
                m_wrapperToolStrip.tsbImportAll_Click(null, null);
            }
            else if (keyData == (Keys.Control | Keys.Alt | Keys.R))
            {
                m_wrapperToolStrip.tsbExportAll_Click(null, null);
            }
            else
            {
                result = base.ProcessCmdKey(ref msg, keyData);
            }

            return result;
        }
        #endregion

        #region ICyParamEditingControl Members
        public Control DisplayControl
        {
            get { return this; }
        }


        // Gets any errors that exist for parameters on the DisplayControl.
        public virtual IEnumerable<CyCustErr> GetErrors()
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
            for (int i = 0; i < m_wrapperDataGridView.RowCount; i++)
                for (int j = 0; j < m_wrapperDataGridView.ColumnCount; j++)
                {
                    if (String.IsNullOrEmpty(m_wrapperDataGridView[j, i].ErrorText) == false)
                    {
                        errorList.Add(m_wrapperDataGridView[j, i].ErrorText);
                    }
                }
            return errorList;
        }
        #endregion

        protected override Point ScrollToControl(Control activeControl)
        {
            if (activeControl.Height > ClientSize.Height
                || activeControl.Width > ClientSize.Width)
                return AutoScrollPosition;
            return base.ScrollToControl(activeControl);
        }
    }

    public class CyTMPanel : Panel
    {
        Label m_label;
        public string LabelText
        {
            get { return m_label.Text; }
            set { m_label.Text = value; }
        }
        public CyTMPanel()
            : base()
        {
            m_label = new Label();
            this.Controls.Add(m_label);
            m_label.AutoSize = false;
            m_label.Dock = DockStyle.Fill;
            m_label.TextAlign = ContentAlignment.MiddleCenter;
            BorderStyle = BorderStyle.FixedSingle;
        }
    }
}
