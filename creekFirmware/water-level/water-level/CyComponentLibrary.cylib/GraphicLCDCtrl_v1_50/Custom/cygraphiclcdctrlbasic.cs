/*******************************************************************************
* Copyright 2008-2010, Cypress Semiconductor Corporation.  All rights reserved.
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

namespace GraphicLCDCtrl_v1_50
{
    public partial class CyGraphicLCDCtrlBasic : UserControl, ICyParamEditingControl
    {
        private CyGraphicLCDCtrlParameters m_params;

        #region Constructor(s)
        public CyGraphicLCDCtrlBasic(CyGraphicLCDCtrlParameters inst)
        {
            InitializeComponent(); 

            inst.m_basicTab = this;
            this.Dock = DockStyle.Fill;
            m_params = inst;

            numHSyncWidth.TextChanged += new EventHandler(numHSyncWidth_TextChanged);
            numHBackPorch.TextChanged += new EventHandler(numHBackPorch_TextChanged);
            numHActiveRegion.TextChanged += new EventHandler(numHActiveRegion_TextChanged);
            numHFrontPorch.TextChanged += new EventHandler(numHFrontPorch_TextChanged);
            numVSyncWidth.TextChanged += new EventHandler(numVSyncWidth_TextChanged);
            numVBackPorch.TextChanged += new EventHandler(numVBackPorch_TextChanged);
            numVActiveRegion.TextChanged += new EventHandler(numVActiveRegion_TextChanged);
            numVFrontPorch.TextChanged += new EventHandler(numVFrontPorch_TextChanged);
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

            foreach (string paramName in m_params.m_inst.GetParamNames())
            {
                CyCompDevParam param = m_params.m_inst.GetCommittedParam(paramName);
                if (param.TabName.Equals(CyCustomizer.BASIC_TAB_NAME))
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

        #region Assigning parameters values to controls
        public void GetParams()
        {
            // Sync Pulse Polarity
            rbActiveLow.Checked = m_params.m_syncPulsePolarityLow;
            rbActiveHigh.Checked = !rbActiveLow.Checked;

            // Transition Dotclk Edge
            rbFallingEdge.Checked = m_params.m_transitionDotclkFalling;
            rbRisingEdge.Checked = !rbFallingEdge.Checked;

            // Horizontal Timing
            numHSyncWidth.Value = (decimal)m_params.m_hSyncWidth;
            numHBackPorch.Value = (decimal)m_params.m_hBackPorch;
            numHActiveRegion.Value = (decimal)m_params.m_hActiveRegion;
            numHFrontPorch.Value = (decimal)m_params.m_hFrontPorch;

            // Vertical Timing
            numVSyncWidth.Value = (decimal)m_params.m_vSyncWidth;
            numVBackPorch.Value = (decimal)m_params.m_vBackPorch;
            numVActiveRegion.Value = (decimal)m_params.m_vActiveRegion;
            numVFrontPorch.Value = (decimal)m_params.m_vFrontPorch;

            // Interrupt Generation
            switch (m_params.m_interruptGen)
            {
                case 0:
                    rbNone.Checked = true;
                    break;
                case 1:
                    rbVBlanking.Checked = true;
                    break;
                case 2:
                    rbVHBlanking.Checked = true;
                    break;
                default:
                    break;
            }
        }
        #endregion

        #region Assigning controls values to parameters
        private void SetParameter(object sender, out UInt16 param, UInt16 min, UInt16 max, 
            string message, string paramName)
        {
            NumericUpDown currentNumeric = (NumericUpDown)sender;
            UInt16.TryParse(currentNumeric.Text, out param);
            if (param < min || param > max)
            {
                errorProvider.SetError(currentNumeric, message);
            }
            else
            {
                errorProvider.SetError(currentNumeric, string.Empty);
            }
            m_params.SetParams(paramName);
        }

        private void SetActiveRegionParameter(object sender, out UInt16 param, UInt16 min, UInt16 max,
            string message1, string message2, string paramName)
        {
            NumericUpDown currentNumeric = (NumericUpDown)sender;
            UInt16.TryParse(currentNumeric.Text, out param);
            if (param < min || param > max)
            {
                errorProvider.SetError(currentNumeric, message1);
            }
            else if ((param % 4) != 0)
            {
                errorProvider.SetError(currentNumeric, message2);
            }
            else
            {
                errorProvider.SetError(currentNumeric, string.Empty);
            }
            m_params.SetParams(paramName);
        }

        private void SetInterruptGen()
        {
            if (rbNone.Checked)
                m_params.m_interruptGen = 0;
            else if (rbVBlanking.Checked)
                m_params.m_interruptGen = 1;
            else
                m_params.m_interruptGen = 2;
            m_params.SetParams(CyParamNames.INTERRUPT_GEN);
        }
        #endregion

        #region Event Handlers
        private void rbActiveHigh_CheckedChanged(object sender, EventArgs e)
        {
            m_params.m_syncPulsePolarityLow = rbActiveLow.Checked;
            m_params.SetParams(CyParamNames.SYNC_PULSE_POLARITY_LOW);
        }

        private void rbFallingEdge_CheckedChanged(object sender, EventArgs e)
        {
            m_params.m_transitionDotclkFalling = rbFallingEdge.Checked;
            m_params.SetParams(CyParamNames.TRANSITION_DOTCLK_FALLING);
        }

        private void numHSyncWidth_TextChanged(object sender, EventArgs e)
        {
            SetParameter(sender, out m_params.m_hSyncWidth, CyParamRanges.H_SYNC_WIDTH_MIN, 
                CyParamRanges.H_SYNC_WIDTH_MAX, Properties.Resources.EPHSyncWidthMsg, CyParamNames.HORIZ_SYNC_WIDTH);
        }

        private void numHBackPorch_TextChanged(object sender, EventArgs e)
        {
            SetParameter(sender, out m_params.m_hBackPorch, CyParamRanges.H_BACK_PORCH_MIN, 
                CyParamRanges.H_BACK_PORCH_MAX, Properties.Resources.EPHBackPorchMsg, CyParamNames.HORIZ_BACK_PORCH);
        }

        private void numHActiveRegion_TextChanged(object sender, EventArgs e)
        {
            SetActiveRegionParameter(sender, out m_params.m_hActiveRegion, CyParamRanges.H_ACTIVE_REGION_MIN,
                CyParamRanges.H_ACTIVE_REGION_MAX, Properties.Resources.EPHActiveRegionMsg1,
                Properties.Resources.EPHActiveRegionMsg2, CyParamNames.HORIZ_ACTIVE_REGION);
        }

        private void numHFrontPorch_TextChanged(object sender, EventArgs e)
        {
            SetParameter(sender, out m_params.m_hFrontPorch, CyParamRanges.H_FRONT_PORCH_MIN, 
                CyParamRanges.H_FRONT_PORCH_MAX, Properties.Resources.EPHFrontPorchMsg, CyParamNames.HORIZ_FRONT_PORCH);
        }

        private void numVSyncWidth_TextChanged(object sender, EventArgs e)
        {
            SetParameter(sender, out m_params.m_vSyncWidth, CyParamRanges.V_SYNC_WIDTH_MIN,
                CyParamRanges.V_SYNC_WIDTH_MAX, Properties.Resources.EPVSyncWidthMsg, CyParamNames.VERT_SYNC_WIDTH);
        }

        private void numVBackPorch_TextChanged(object sender, EventArgs e)
        {
            SetParameter(sender, out m_params.m_vBackPorch, CyParamRanges.V_BACK_PORCH_MIN,
                CyParamRanges.V_BACK_PORCH_MAX, Properties.Resources.EPVBackPorchMsg, CyParamNames.VERT_BACK_PORCH);
        }

        private void numVActiveRegion_TextChanged(object sender, EventArgs e)
        {
            SetActiveRegionParameter(sender, out m_params.m_vActiveRegion, CyParamRanges.V_ACTIVE_REGION_MIN,
                CyParamRanges.V_ACTIVE_REGION_MAX, Properties.Resources.EPVActiveRegionMsg1,
                Properties.Resources.EPVActiveRegionMsg2, CyParamNames.VERT_ACTIVE_REGION);
        }

        private void numVFrontPorch_TextChanged(object sender, EventArgs e)
        {
            SetParameter(sender, out m_params.m_vFrontPorch, CyParamRanges.V_FRONT_PORCH_MIN,
                CyParamRanges.V_FRONT_PORCH_MAX, Properties.Resources.EPVFrontPorchMsg, CyParamNames.VERT_FRONT_PORCH);
        }

        private void rbNone_CheckedChanged(object sender, EventArgs e)
        {
            SetInterruptGen();
        }

        private void rbVBlanking_CheckedChanged(object sender, EventArgs e)
        {
            SetInterruptGen();
        }
        #endregion
    }
}
