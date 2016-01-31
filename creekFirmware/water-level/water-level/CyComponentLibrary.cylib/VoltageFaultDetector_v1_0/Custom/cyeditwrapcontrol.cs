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

namespace VoltageFaultDetector_v1_0
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
        #endregion
    }
}
