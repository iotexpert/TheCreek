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

namespace Debouncer_v1_0
{
    public partial class CyGeneralTab : UserControl, ICyParamEditingControl
    {
        protected CyParameters m_params = null;
        protected ErrorProvider m_errorProvider = null;

        public string TabName
        {
            get { return "Basic"; }
        }

        #region Constructor(s)
        public CyGeneralTab(CyParameters parameters)
        {
            m_params = parameters;

            InitializeComponent();

            #region Add event handlers
            m_numSignalWidth.TextChanged += new EventHandler(m_num_TextChanged);

            m_chbPositiveEdge.CheckedChanged += new EventHandler(m_chb_CheckedChanged);
            m_chbNegativeEdge.CheckedChanged += new EventHandler(m_chb_CheckedChanged);
            m_chbEitherEdge.CheckedChanged += new EventHandler(m_chb_CheckedChanged);
            #endregion

            m_errorProvider = new ErrorProvider();
            m_errorProvider.BlinkStyle = ErrorBlinkStyle.NeverBlink;

            Load += delegate(object sender, EventArgs e)
            {
                this.Dock = DockStyle.Fill;
                this.AutoScroll = true;
            };

            UpdateUI();
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

        #region Update UI
        public void UpdateUI()
        {
            m_numSignalWidth.Value = m_params.SignalWidth;

            m_chbPositiveEdge.Checked = m_params.PositiveEdge;
            m_chbNegativeEdge.Checked = m_params.NegativeEdge;
            m_chbEitherEdge.Checked = m_params.EitherEdge;
        }
        #endregion

        #region Event handlers
        void m_num_TextChanged(object sender, EventArgs e)
        {
            NumericUpDown currentControl = sender as NumericUpDown;

            if (currentControl != null)
            {
                if (currentControl == m_numSignalWidth)
                {
                    byte value;

                    m_errorProvider.SetError(m_numSignalWidth, string.Empty);

                    if (byte.TryParse(currentControl.Text, out value) &&
                        value >= CyParamRanges.MIN_SIGNAL_WIDTH && value <= CyParamRanges.MAX_SIGNAL_WIDTH)
                    {
                        m_params.SignalWidth = value;
                    }
                    else
                    {
                        m_errorProvider.SetError(m_numSignalWidth,
                            string.Format(Resources.SignalWidthValueErrorDescription, CyParamRanges.MIN_SIGNAL_WIDTH,
                            CyParamRanges.MAX_SIGNAL_WIDTH));
                    }
                }
            }
        }

        void m_chb_CheckedChanged(object sender, EventArgs e)
        {
            CheckBox currentControl = sender as CheckBox;

            if (currentControl != null)
            {
                if (currentControl == m_chbPositiveEdge)
                {
                    m_params.PositiveEdge = currentControl.Checked;
                }
                else if (currentControl == m_chbNegativeEdge)
                {
                    m_params.NegativeEdge = currentControl.Checked;
                }
                else if (currentControl == m_chbEitherEdge)
                {
                    m_params.EitherEdge = currentControl.Checked;
                }
            }
        }
        #endregion
    }
}
