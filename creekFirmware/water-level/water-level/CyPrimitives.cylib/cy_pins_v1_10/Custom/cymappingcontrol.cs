/*******************************************************************************
* Copyright 2008-2009, Cypress Semiconductor Corporation.  All rights reserved.
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

using CyDesigner.Common.Base;
using CyDesigner.Common.Base.Controls;
using CyDesigner.Extensions.Common;
using CyDesigner.Extensions.Gde;
using Cypress.Comps.PinsAndPorts.Common;

namespace Cypress.Comps.PinsAndPorts.cy_pins_v1_10
{
    public partial class CyMappingControl : UserControl, ICyParamEditingControl
    {
        ICyInstEdit_v1 m_edit;

        public CyMappingControl()
        {
            InitializeComponent();

            this.Dock = DockStyle.Fill;
            m_spanningCheckBox.Enabled = false;

            m_displayAsBusCheckBox.CheckedChanged += new EventHandler(m_displayAsBusCheckBox_CheckedChanged);
            m_contCheckBox.CheckedChanged += new EventHandler(m_contCheckBox_CheckedChanged);
        }

        void m_contCheckBox_CheckedChanged(object sender, EventArgs e)
        {
            string layout = (m_contCheckBox.Checked) ?
                CyPortConstants.LayoutModeValue_CONT_NONSPANNING :
                CyPortConstants.LayoutModeValue_NONCONT_SPANNING;

            Edit.SetParamExpr(CyParamInfo.Formal_ParamName_LayoutMode, layout);
            Edit.CommitParamExprs();

            CyCompDevParam param = Edit.GetCommittedParam(CyParamInfo.Formal_ParamName_LayoutMode);
            m_errorProvider.SetError(m_contCheckBox, (param.ErrorCount > 0) ? param.ErrorMsgs : string.Empty);

            m_spanningCheckBox.Checked = !m_contCheckBox.Checked;
        }

        void m_displayAsBusCheckBox_CheckedChanged(object sender, EventArgs e)
        {
            Edit.SetParamExpr(CyParamInfo.Formal_ParamName_DisplayAsBus, m_displayAsBusCheckBox.Checked.ToString());
            Edit.CommitParamExprs();

            CyCompDevParam param = Edit.GetCommittedParam(CyParamInfo.Formal_ParamName_DisplayAsBus);
            m_errorProvider.SetError(m_displayAsBusCheckBox, (param.ErrorCount > 0) ? param.ErrorMsgs : string.Empty);
        }

        public ICyInstEdit_v1 Edit
        {
            get { return m_edit; }
            set
            {
                m_edit = value;
                UpdateFromExprs();
            }
        }

        internal void UpdateFromExprs()
        {
            m_displayAsBusCheckBox.CheckedChanged -= m_displayAsBusCheckBox_CheckedChanged;
            m_contCheckBox.CheckedChanged -= m_contCheckBox_CheckedChanged;

            bool contiguous;
            CyCustErr err = CyParamInfo.GetIsContiguousValue(Edit, out contiguous);
            m_contCheckBox.Checked = contiguous;
            m_spanningCheckBox.Checked = !contiguous;
            m_errorProvider.SetError(m_contCheckBox, (err.IsNotOk) ? err.Message : string.Empty);

            bool displayAsBus;
            err = CyParamInfo.GetDisplayAsBusValue(Edit, out displayAsBus);
            m_displayAsBusCheckBox.Checked = displayAsBus;
            m_errorProvider.SetError(m_displayAsBusCheckBox, (err.IsNotOk) ? err.Message : string.Empty);

            m_displayAsBusCheckBox.CheckedChanged += m_displayAsBusCheckBox_CheckedChanged;
            m_contCheckBox.CheckedChanged += m_contCheckBox_CheckedChanged;
        }

        #region ICyParamEditingControl Members

        public Control DisplayControl
        {
            get { return this; }
        }

        public IEnumerable<CyCustErr> GetErrors()
        {
            List<CyCustErr> errs = new List<CyCustErr>();

            foreach (string paramName in m_edit.GetParamNames())
            {
                CyCompDevParam param = m_edit.GetCommittedParam(paramName);
                if (param.TabName == CyCustomizer.MappingTabName)
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
