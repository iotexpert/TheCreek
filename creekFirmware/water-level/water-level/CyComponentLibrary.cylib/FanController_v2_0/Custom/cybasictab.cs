/*******************************************************************************
* Copyright 2008-2012, Cypress Semiconductor Corporation.  All rights reserved.
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
using FanController_v2_0;


namespace FanController_v2_0
{
    public partial class CyBasicTab : CyEditingWrapperControl
    {
        public override string TabName
        {
            get { return "Basic"; }
        }

        public CyBasicTab(CyParameters param)
            : base(param)
        {
            InitializeComponent();

            // fill in the percent values in the tolerance drop down
            for (int counti = 0; counti < 10; counti++)
            {
                m_cmbFanTolerance.Items.Add((counti + 1).ToString() + "%");
            }
            m_cbANR.CheckedChanged += delegate(object sender, EventArgs e)
            {
                m_prms.AcousticNoiseReduction = Convert.ToByte(m_cbANR.Checked);
            };
            m_cmbFanTolerance.SelectedIndexChanged += delegate(object sender, EventArgs e)
            {
                m_prms.FanTolerance = (byte)(m_cmbFanTolerance.SelectedIndex + 1);
            };
            m_externalClock.CheckedChanged+= delegate(object sender, EventArgs e)
            {
                m_prms.ExternalClock = m_externalClock.Checked ? (byte)1 : (byte)0;
                m_prms.m_controlfans.UpdateValues();
            };

            m_connection.CheckedChanged += delegate(object sender, EventArgs e)
            {
                m_prms.Connection = m_connection.Checked ? CyConnectinType.BUSSED  : CyConnectinType.WIRED;
            };


            UpdateFormFromParams();
        }

        #region Form Updating Routines
        public void UpdateFormFromParams()
        {
            if (m_prms == null) return;

            // Control Mode Radio Button
            m_rbModeOpenLoop.Checked = m_prms.FanMode == CyFanModeType.FIRMWARE;
            m_rbModeClosedLoop.Checked = m_prms.FanMode == CyFanModeType.HARDWARE;
            m_nudDampingFactor.Enabled = m_prms.FanMode == CyFanModeType.HARDWARE;
            m_lblDampingFactor.Enabled = m_prms.FanMode == CyFanModeType.HARDWARE;
            m_cmbFanTolerance.Enabled = m_prms.FanMode == CyFanModeType.HARDWARE;
            m_lblFanTolerance.Enabled = m_prms.FanMode == CyFanModeType.HARDWARE;
            m_cbANR.Enabled = m_prms.FanMode == CyFanModeType.HARDWARE;
            m_cbAltSpeedFailure.Enabled = m_prms.FanMode == CyFanModeType.HARDWARE;


            // Alert Enable Checkboxes
            if ((m_prms.AlertEnable & CyParameters.ALERT_FAN_STALL) != 0)
                m_cbAltFanStall.Checked = true;
            if ((m_prms.AlertEnable & CyParameters.ALERT_SPEED_FAIL) != 0)
                m_cbAltSpeedFailure.Checked = true;

            m_nudDampingFactor.Value = (decimal)m_prms.DampingFactor;

            int val = Convert.ToByte(m_prms.FanTolerance) - 1;
            if (val >= 0 && val < m_cmbFanTolerance.Items.Count)
                m_cmbFanTolerance.SelectedIndex = val;

            // Acoustic Noise Reduction Checkbox 
            if (m_prms.AcousticNoiseReduction != 0)
                m_cbANR.Checked = true;
            else
                m_cbANR.Checked = false;

            m_externalClock.Checked = m_prms.ExternalClock != 0;

            m_connection.Checked = m_prms.Connection == CyConnectinType.BUSSED;
        }

        #endregion



        #region event_handlers

        private void m_rbModeOpenLoop_Click(object sender, EventArgs e)
        {
            if (!m_rbModeOpenLoop.Checked)
            {
                m_rbModeOpenLoop.Checked = true;
                m_rbModeClosedLoop.Checked = false;
                m_prms.FanMode = CyFanModeType.FIRMWARE;
             
                m_nudDampingFactor.Enabled = false;
                m_lblDampingFactor.Enabled = false;
                m_cmbFanTolerance.Enabled = false;
                m_lblFanTolerance.Enabled = false;
                m_cbANR.Enabled = false;

                m_cbAltSpeedFailure.Checked = false;
                m_cbAltSpeedFailure.Enabled = false;

                // Call the Mode change function for the fan page so it can update the bank
                // mode enable/disable etc.
                m_prms.m_controlfans.UpdateValues();
            }
        }

        private void m_rbModeClosedLoop_Click(object sender, EventArgs e)
        {
            if (!m_rbModeClosedLoop.Checked)
            {
                m_rbModeOpenLoop.Checked = false;
                m_rbModeClosedLoop.Checked = true;
                m_prms.FanMode = CyFanModeType.HARDWARE;
                
                m_nudDampingFactor.Enabled = true;
                m_lblDampingFactor.Enabled = true;
                m_cmbFanTolerance.Enabled = true;
                m_lblFanTolerance.Enabled = true;
                m_cbANR.Enabled = true;

                m_cbAltSpeedFailure.Enabled = true;

                // Call the Mode change function for the fan page so it can update the bank
                // mode enable/disable etc.
                m_prms.m_controlfans.UpdateValues();

            }
        }

        private void m_cbAltFanStall_CheckedChanged(object sender, EventArgs e)
        {
            if (((System.Windows.Forms.CheckBox)sender).Checked == true)
                m_prms.AlertEnable |= CyParameters.ALERT_FAN_STALL;
            else
                m_prms.AlertEnable &= Convert.ToByte(~CyParameters.ALERT_FAN_STALL & 0xFF);
        }

        private void m_cbAltSpeedFailure_CheckedChanged(object sender, EventArgs e)
        {
            if (((System.Windows.Forms.CheckBox)sender).Checked == true)
                m_prms.AlertEnable |= CyParameters.ALERT_SPEED_FAIL;
            else
                m_prms.AlertEnable &= Convert.ToByte(~CyParameters.ALERT_SPEED_FAIL & 0xFF);
        }


        #endregion

        private void m_nudDampingFactor_ValueChanged(object sender, EventArgs e)
        {
            m_prms.DampingFactor = (double)m_nudDampingFactor.Value;
        }
    }
}

