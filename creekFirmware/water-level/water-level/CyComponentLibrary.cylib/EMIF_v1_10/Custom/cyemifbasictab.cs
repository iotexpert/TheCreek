/*******************************************************************************
* Copyright 2011, Cypress Semiconductor Corporation.  All rights reserved.
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

namespace EMIF_v1_10
{
    public partial class CyEMIFBasicTab : UserControl, ICyParamEditingControl
    {
        private CyEMIFParameters m_params = null;
        private ICyTerminalQuery_v1 m_termQuery;

        #region Constructor(s)
        public CyEMIFBasicTab(CyEMIFParameters inst, ICyTerminalQuery_v1 termQuery)
        {
            inst.m_basicTab = this;
            m_termQuery = termQuery;

            InitializeComponent();

            this.Dock = DockStyle.Fill;
            m_params = inst;

            // Create event handler for numericUpDown control
            m_numExtMemorySpeed.TextChanged += new EventHandler(m_numExtMemorySpeed_TextChanged);
        }
        #endregion

        #region ICyParamEditingControl Members
        public Control DisplayControl
        {
            get { return this; }
        }

        public IEnumerable<CyCustErr> GetErrors()
        {
            foreach (string paramName in m_params.m_inst.GetParamNames())
            {
                CyCompDevParam param = m_params.m_inst.GetCommittedParam(paramName);
                if (param.TabName.Equals(CyCustomizer.GENERAL_TAB_NAME))
                {
                    if (param.ErrorCount > 0)
                    {
                        foreach (string errMsg in param.Errors)
                        {
                            yield return new CyCustErr(errMsg);
                        }
                    }
                }
            }
        }
        #endregion

        #region Assigning parameters values to controls
        public void UpdateUI()
        {
            // External memory type
            if (m_params.Mode == CyESRAMMode.Asynch)
            {
                m_rbAsynchronous.Checked = true;
            }
            else if (m_params.Mode == CyESRAMMode.Synch)
            {
                m_rbSynchronous.Checked = true;
            }
            else
            {
                m_rbCustom.Checked = true;
            }

            // Address width
            if (m_params.AddressWidth == CyEAddrWidth.Address24)
            {
                m_rbAddressWidth24.Checked = true;
            }
            else if (m_params.AddressWidth == CyEAddrWidth.Address16)
            {
                m_rbAddressWidth16.Checked = true;
            }
            else
            {
                m_rbAddressWidth8.Checked = true;
            }

            // Data width
            if (m_params.DataWidth == CyEDataWidth.Data16)
            {
                m_rbDataWidth16.Checked = true;
            }
            else
            {
                m_rbDataWidth8.Checked = true;
            }

            // External memory speed
            m_numExtMemorySpeed.Text = m_params.MemorySpeed.ToString();
            UpdateCalculator();
        }
        #endregion

        #region Event Handlers
        private void externalMemoryType_CheckedChanged(object sender, EventArgs e)
        {
            if (m_rbAsynchronous.Checked)
            {
                m_params.Mode = CyESRAMMode.Asynch;
                lblExternalMemoryType.Text = CyEMIFRresource.LabelExtMemoryTypeAsync;
            }
            else if (m_rbSynchronous.Checked)
            {
                m_params.Mode = CyESRAMMode.Synch;
                lblExternalMemoryType.Text = CyEMIFRresource.LabelExtMemoryTypeSync;
            }
            else
            {
                m_params.Mode = CyESRAMMode.Custom;
                lblExternalMemoryType.Text = CyEMIFRresource.LabelExtMemoryTypeCustom;
            }

            bool calcVisibility = (m_params.Mode == CyESRAMMode.Custom) ? false : true;
            groupBox5.Visible = calcVisibility;
            m_lblExtMemorySpeed.Visible = calcVisibility;
            m_numExtMemorySpeed.Visible = calcVisibility;
            m_lblBusClockFrequency.Visible = calcVisibility;
            m_lblBusClockFrequencyCalc.Visible = calcVisibility;
        }

        private void addressWidth_CheckedChanged(object sender, EventArgs e)
        {
            if (m_rbAddressWidth24.Checked)
            {
                m_params.AddressWidth = CyEAddrWidth.Address24;
            }
            else if (m_rbAddressWidth16.Checked)
            {
                m_params.AddressWidth = CyEAddrWidth.Address16;
            }
            else
            {
                m_params.AddressWidth = CyEAddrWidth.Address8;
            }
        }

        private void dataWidth_CheckedChanged(object sender, EventArgs e)
        {
            if (m_rbDataWidth16.Checked)
            {
                m_params.DataWidth = CyEDataWidth.Data16;
            }
            else
            {
                m_params.DataWidth = CyEDataWidth.Data8;
            }
        }

        private void m_numExtMemorySpeed_TextChanged(object sender, EventArgs e)
        {
            try
            {
                byte value = Convert.ToByte(m_numExtMemorySpeed.Text);
                if (value >= CyParamRange.MEMORY_SPEED_MIN && value <= CyParamRange.MEMORY_SPEED_MAX)
                {
                    m_params.MemorySpeed = value;
                    m_errorProvider.SetError(m_numExtMemorySpeed, string.Empty);
                }
                else
                {
                    m_params.MemorySpeed = value;
                    m_errorProvider.SetError(m_numExtMemorySpeed, string.Format(
                        CyEMIFRresource.ErrorExternalMemorySpeed, CyParamRange.MEMORY_SPEED_MIN,
                        CyParamRange.MEMORY_SPEED_MAX));
                }
            }
            catch (Exception)
            {
                m_params.SetParameter<string>(CyParamNames.EMIF_MEMSPEED, m_numExtMemorySpeed.Text);
                m_errorProvider.SetError(m_numExtMemorySpeed, string.Format(CyEMIFRresource.ErrorExternalMemorySpeed,
                    CyParamRange.MEMORY_SPEED_MIN, CyParamRange.MEMORY_SPEED_MAX));
            }
            UpdateCalculator();
        }
        #endregion

        #region Calculator
        private void UpdateCalculator()
        {
            // Update Bus Clock Value
            double busClkFreq = CyClockReader.GetBusClkInMHz(m_termQuery);
            if (busClkFreq > 0)
                m_lblBusClockFrequencyCalc.Text = busClkFreq.ToString() + " MHz";
            else
                m_lblBusClockFrequencyCalc.Text = "UNKNOWN";

            // Update Memory Transactions values
            double writeCPUCyclesNs = 0;
            double readCPUCyclesNs = 0;
            double wenPulseWidth = 0;
            double oenPulseWidth = 0;
            double writeCPUCycles = 0;
            double readCPUCycles = 0;

            if (busClkFreq > 0)
            {
                byte freqDivider = CyCustomizer.GetFrequencyDivider(busClkFreq);
                byte clkDiv = (byte)(freqDivider - 1);

                int writeCycles = CyCustomizer.GetWriteCycles(freqDivider);
                double emifWpStates = CyCustomizer.GetWpWaitStates(m_params.MemorySpeed, busClkFreq, writeCycles);
                writeCPUCycles = writeCycles + emifWpStates;
                writeCPUCyclesNs = writeCPUCycles * (1 / (busClkFreq / 1000));

                int readCycles = CyCustomizer.GetReadCycles(freqDivider);
                double emifRpStates = CyCustomizer.GetRpWaitStates(m_params.MemorySpeed, busClkFreq, readCycles);
                readCPUCycles = readCycles + emifRpStates;
                readCPUCyclesNs = readCPUCycles * (1 / (busClkFreq / 1000));

                wenPulseWidth = Math.Round(1000 / busClkFreq * CyCustomizer.GetFrequencyDivider(busClkFreq), 1);
                oenPulseWidth = Math.Round(((1000 / busClkFreq * CyCustomizer.GetFrequencyDivider(busClkFreq)) * 2
                    + emifRpStates * (1 / (busClkFreq / 1000))), 1);
            }

            m_lblWriteCycleLengthCalc.Text = Math.Round(writeCPUCyclesNs, 1).ToString() + " ns (" 
                + writeCPUCycles.ToString() + " Cycles)";
            m_lblReadCycleLengthCalc.Text = Math.Round(readCPUCyclesNs, 1).ToString() + " ns (" 
                + readCPUCycles.ToString() + " Cycles)";

            m_lblWenPulseWidthCalc.Text = wenPulseWidth.ToString() + " ns";
            m_lblOenPulseWidthCalc.Text = oenPulseWidth.ToString() + " ns";
        }
        #endregion
    }
}
