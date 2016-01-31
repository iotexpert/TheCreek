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
using CyDesigner.Extensions.Common;
using CyDesigner.Extensions.Gde;
using CyCustomizer.UART_v1_20;

namespace CyCustomizer.UART_v1_20
{
    public partial class cyuartadvancedcontrol : UserControl
    {
        public ICyInstEdit_v1 m_Component = null;
        public ICyTerminalQuery_v1 m_TermQuery = null;
        public event ClockChangeDelegate ClockSelected;
        public event ClockFrequencyChangeDelegate ExClockFrequencyChanged;

        private UARTParameters m_Parameters;
        private List<NumericUpDown> m_ListNumerics = new List<NumericUpDown>();

        public cyuartadvancedcontrol(ICyInstEdit_v1 inst, ICyTerminalQuery_v1 termquery, UARTParameters parameters)
        {
            m_Component = inst;
            m_TermQuery = termquery;
            m_Parameters = parameters;

            InitializeComponent();
            //Set the RX Address Modes Combo Box
            IEnumerable<string> RXAddressModes = inst.GetPossibleEnumValues("RXAddressMode");
            foreach (string str in RXAddressModes)
            {
                m_cbRXAddressMode.Items.Add(str);
            }
            SetFrequencyLabel();
            UpdateFormFromParams(inst);            
            m_ListNumerics.Add(m_numRXBufferSize);
            m_ListNumerics.Add(m_numTXBufferSize);
            foreach (NumericUpDown item in m_ListNumerics)
            {
                item.TextChanged += new EventHandler(m_numeric_TextChanged);
                item.Validating += new CancelEventHandler(m_numeric_Validating);
            }
                      
        }  
        void m_numeric_Validating(object sender, CancelEventArgs e)
        {
            if (sender is NumericUpDown)
            {
                NumericUpDown num = sender as NumericUpDown;
                bool error;
                double val = GetNumericUpDownText(sender, out error);
                double minimum = (double)num.Minimum;
                double maximum = (double)num.Maximum;
                string message = String.Format("Value must be in range [{0}, {1}]", minimum, maximum);
                if ((error) || ((val < minimum) || (val > maximum)))
                {
                    ep_Errors.SetError((NumericUpDown)sender, string.Format(message));
                    e.Cancel = true;
                }
                else
                {
                    ep_Errors.SetError((NumericUpDown)sender, "");
                }
            }
        }
        private void m_numeric_TextChanged(object sender, EventArgs e)
        {
            m_numeric_Validating(sender, new CancelEventArgs());
        }
        private double GetNumericUpDownText(object numericUpDown, out bool error)
        {
            error = false;
            double val = 0;
            try
            {
                val = Convert.ToDouble(((NumericUpDown)numericUpDown).Text);
            }
            catch
            { error = true; }
            return val;
        }

        public void UpdateFormFromParams(ICyInstEdit_v1 inst)
        {
            //Set the Break Enable checkbox
            m_chbBreakDetected.Checked = m_Parameters.BreakDetect;

            //Set the 2OutOf3Votting checkbox
            m_chbUse23Polling.Checked = m_Parameters.Use23Polling;

            //Set the 2OutOf3Votting checkbox
            m_chbCRCoutputsEn.Checked = m_Parameters.CRCoutputsEn;

            //Set the Clock Selection Radio Buttons
            if (m_Parameters.InternalClock.Expr == "true")
            {
                m_rbInternalClock.Enabled = true;
                m_rbExternalClock.Enabled = true;
                m_rbInternalClock.Checked = true;
                m_rbExternalClock.Checked = false;
            }
            else if (m_Parameters.InternalClock.Expr == "false")
            {
                m_rbInternalClock.Enabled = true;
                m_rbExternalClock.Enabled = true;
                m_rbInternalClock.Checked = false;
                m_rbExternalClock.Checked = true;
            }
            else
            {
                m_rbInternalClock.Enabled = false;
                m_rbExternalClock.Enabled = false;
                m_rbInternalClock.Checked = false;
                m_rbExternalClock.Checked = false;
            }

            SetFrequencyLabel();

            //Set the Hardware TX Enable Check Box
            if ((m_Parameters.TXEnable == true) || (m_Parameters.HalfDuplexEnable == true))
            {
                if (m_Parameters.HwTXEnSignal.Expr == "true")
                {
                    m_chbxHWTxEnable.Checked = true;
                    m_chbxHWTxEnable.Enabled = true;
                }
                else if (m_Parameters.HwTXEnSignal.Expr == "false")
                {
                    m_chbxHWTxEnable.Checked = false;
                    m_chbxHWTxEnable.Enabled = true;
                }
                else
                {
                    m_chbxHWTxEnable.Checked = false;
                    m_chbxHWTxEnable.Enabled = false;
                }
            }
            else
            {
                m_chbxHWTxEnable.Enabled = false;
            }

            //Set the RX Buffer Size Numeric Up/Down
            m_numRXBufferSize.Minimum = 1;
            m_numRXBufferSize.Maximum = m_Parameters.RXBufferSizeMax;

            if (m_Parameters.RXEnable == true)
            {
                Int64 rxbufsize = 0;
                if (Int64.TryParse(m_Parameters.RXBufferSize.Expr, out rxbufsize))
                {
                    m_numRXBufferSize.Value = rxbufsize;
                    m_numRXBufferSize.Enabled = true;
                }
                else
                {
                    m_numRXBufferSize.Value = 0;
                    m_numRXBufferSize.Enabled = false;
                }
            }
            else
            {
                m_numRXBufferSize.Enabled = false;
            }
            m_lblRXStatusLabel.Enabled = m_lblRXStatus.Enabled = m_Parameters.RXEnable;

            
            //Set the TX Buffer Size Numeric Up/Down
            m_numTXBufferSize.Minimum = 1;
            m_numTXBufferSize.Maximum = m_Parameters.TXBufferSizeMax;
          
            if ((m_Parameters.TXEnable == true) || (m_Parameters.HalfDuplexEnable == true))
            {
                Int64 txbufsize = 0;
                if (Int64.TryParse(m_Parameters.TXBufferSize.Expr, out txbufsize))
                {
                    m_numTXBufferSize.Value = txbufsize;
                    m_numTXBufferSize.Enabled = true;
                }
                else
                {
                    m_numTXBufferSize.Value = 0;
                    m_numTXBufferSize.Enabled = false;
                }
            }
            else
            {
                m_numTXBufferSize.Enabled = false;
            }
            m_lblTXStatusLabel.Enabled = m_lblTXStatus.Enabled = m_Parameters.TXEnable;

            if (m_Parameters.RXEnable == true)
            {
                m_chbxInterruptOnAddressDetect.Enabled = true;
                if (m_Parameters.IntOnAddressDetect.Expr == "true") m_chbxInterruptOnAddressDetect.Checked = true;
                else m_chbxInterruptOnAddressDetect.Checked = false;

                m_chbxInterruptOnAddressMatch.Enabled = true;
                if (m_Parameters.IntOnAddressMatch.Expr == "true") m_chbxInterruptOnAddressMatch.Checked = true;
                else m_chbxInterruptOnAddressMatch.Checked = false;

                m_chbxInterruptOnBreak.Enabled = m_Parameters.BreakDetect;
                if (m_Parameters.IntOnBreak.Expr == "true") m_chbxInterruptOnBreak.Checked = true;
                else m_chbxInterruptOnBreak.Checked = false;

                m_chbxInterruptOnByteReceived.Enabled = true;
                if (m_Parameters.IntOnByteRcvd.Expr == "true") m_chbxInterruptOnByteReceived.Checked = true;
                else m_chbxInterruptOnByteReceived.Checked = false;

                m_chbxInterruptOnOverrunError.Enabled = true;
                if (m_Parameters.IntOnOverrunError.Expr == "true") m_chbxInterruptOnOverrunError.Checked = true;
                else m_chbxInterruptOnOverrunError.Checked = false;

                m_chbxInterruptOnParityError.Enabled = true;
                if (m_Parameters.IntOnParityError.Expr == "true") m_chbxInterruptOnParityError.Checked = true;
                else m_chbxInterruptOnParityError.Checked = false;

                m_chbxInterruptOnStopError.Enabled = true;
                if (m_Parameters.IntOnStopError.Expr == "true") m_chbxInterruptOnStopError.Checked = true;
                else m_chbxInterruptOnStopError.Checked = false;
            }
            else
            {
                m_chbxInterruptOnAddressDetect.Enabled = false;
                m_chbxInterruptOnAddressMatch.Enabled = false;
                m_chbxInterruptOnBreak.Enabled = false;
                m_chbxInterruptOnByteReceived.Enabled = false;
                m_chbxInterruptOnOverrunError.Enabled = false;
                m_chbxInterruptOnParityError.Enabled = false;
                m_chbxInterruptOnStopError.Enabled = false;
            }


            if (m_Parameters.TXEnable == true)
            {
                m_chbxInterruptOnTXComplete.Enabled = true;
                if (m_Parameters.InterruptOnTXComplete.Expr == "true") m_chbxInterruptOnTXComplete.Checked = true;
                else m_chbxInterruptOnTXComplete.Checked = false;

                m_chbxInterruptOnTXFifoEmpty.Enabled = true;
                if (m_Parameters.InterruptOnTXFifoEmpty.Expr == "true") m_chbxInterruptOnTXFifoEmpty.Checked = true;
                else m_chbxInterruptOnTXFifoEmpty.Checked = false;

                m_chbxInterruptOnTXFifoFull.Enabled = true;
                if (m_Parameters.InterruptOnTXFifoFull.Expr == "true") m_chbxInterruptOnTXFifoFull.Checked = true;
                else m_chbxInterruptOnTXFifoFull.Checked = false;

                m_chbxInterruptOnTXFifoNotFull.Enabled = true;
                if (m_Parameters.InterruptOnTXFifoNotFull.Expr == "true") m_chbxInterruptOnTXFifoNotFull.Checked = true;
                else m_chbxInterruptOnTXFifoNotFull.Checked = false;

            }
            else
            {
                m_chbxInterruptOnTXComplete.Enabled = false;
                m_chbxInterruptOnTXFifoEmpty.Enabled = false;
                m_chbxInterruptOnTXFifoFull.Enabled = false;
                m_chbxInterruptOnTXFifoNotFull.Enabled = false;
            }



            Int64 address1 = 0;
            if (Int64.TryParse(m_Parameters.Address1.Expr, out address1))
            {
                m_numRXAddress1.Value = address1;
                //m_pbTXBufferSizeHier.Visible = false;
            }
            else
            {
                m_numRXAddress1.Value = 0;
                //m_pbTXBufferSizeHier.Visible = true;
            }

            Int64 address2 = 0;
            if (Int64.TryParse(m_Parameters.Address2.Expr, out address2))
            {
                m_numRXAddress2.Value = address2;
                //m_pbTXBufferSizeHier.Visible = false;
            }
            else
            {
                m_numRXAddress2.Value = 0;
                //m_pbTXBufferSizeHier.Visible = true;
            }

            if (m_Parameters.RXEnable == true)
            {
                //Set the RXAddressmode Combo Box
                IEnumerable<string> RXAddressModes = inst.GetPossibleEnumValues("RXAddressMode");
                bool addmode = false;
                foreach (string str in RXAddressModes)
                {
                    if (!addmode)
                    {
                        string paramcompare = m_Component.ResolveEnumIdToDisplay(
                            "RXAddressMode",
                            m_Parameters.RXAddressMode.Expr);
                        if (paramcompare == str)
                        {
                            m_cbRXAddressMode.SelectedItem = paramcompare;
                            addmode = true;
                        }
                    }
                }
                if (!addmode)
                {
                    m_cbRXAddressMode.Text = m_Parameters.RXAddressMode.Expr;
                }

                if (m_cbRXAddressMode.Text != "None")
                {
                    m_numRXAddress1.Enabled = true;
                    m_numRXAddress2.Enabled = true;
                }
                else
                {
                    m_numRXAddress1.Enabled = false;
                    m_numRXAddress2.Enabled = false;
                }

                m_cbRXAddressMode.Enabled = true;
            }
            else
            {
                m_numRXAddress1.Enabled = false;
                m_numRXAddress2.Enabled = false;
                m_cbRXAddressMode.Enabled = false;
            }
            SetFrequencyLabel();
        }

        private void SetAParameter(string parameter, string value)
        {
            m_Component.SetParamExpr(parameter, value);
            m_Component.CommitParamExprs();
            if (m_Component.GetCommittedParam(parameter).ErrorCount != 0)
            {
                string errors = null;
                foreach (string err in m_Component.GetCommittedParam(parameter).Errors)
                {
                    errors = errors + err + "\n";
                }
                MessageBox.Show(
                    string.Format(
                        "Error Setting Parameter {0} with value {1}\n\nErrors:\n{2}",
                        parameter,
                        value,
                        errors),
                    "Error Setting Parameter",
                    MessageBoxButtons.OK,
                    MessageBoxIcon.Error);
            }
        }

        private void m_rbInternalClock_Click(object sender, EventArgs e)
        {
            if (!m_rbInternalClock.Checked)
            {
                m_rbInternalClock.Checked = true;
                m_rbExternalClock.Checked = false;
                SetAParameter("InternalClock", "true");
            }
        }

        private void m_rbExternalClock_Click(object sender, EventArgs e)
        {
            if (!m_rbExternalClock.Checked)
            {
                m_rbInternalClock.Checked = false;
                m_rbExternalClock.Checked = true;
                SetAParameter("InternalClock", "false");
            }
        }

        private void m_chbxHWTxEnable_CheckedChanged(object sender, EventArgs e)
        {
            if (m_chbxHWTxEnable.Checked)
            {
                SetAParameter("HwTXEnSignal", "true");
            }
            else
            {
                SetAParameter("HwTXEnSignal", "false");
            }
        }

        private void m_chbxInterruptOnByteReceived_CheckedChanged(object sender, EventArgs e)
        {
            if (m_chbxInterruptOnByteReceived.Checked)
            {
                SetAParameter("IntOnByteRcvd", "true");
            }
            else
            {
                SetAParameter("IntOnByteRcvd", "false");
            }
        }

        private void m_chbxInterruptOnParityError_CheckedChanged(object sender, EventArgs e)
        {
            if (m_chbxInterruptOnParityError.Checked)
            {
                SetAParameter("IntOnParityError", "true");
            }
            else
            {
                SetAParameter("IntOnParityError", "false");
            }
        }

        private void m_chbxInterruptOnStopError_CheckedChanged(object sender, EventArgs e)
        {
            if (m_chbxInterruptOnStopError.Checked)
            {
                SetAParameter("IntOnStopError", "true");
            }
            else
            {
                SetAParameter("IntOnStopError", "false");
            }
        }

        private void m_chbxInterruptOnBreak_CheckedChanged(object sender, EventArgs e)
        {
            if (m_chbxInterruptOnBreak.Checked)
            {
                SetAParameter("IntOnBreak", "true");
            }
            else
            {
                SetAParameter("IntOnBreak", "false");
            }
        }

        private void m_chbxInterruptOnOverrunError_CheckedChanged(object sender, EventArgs e)
        {
            if (m_chbxInterruptOnOverrunError.Checked)
            {
                SetAParameter("IntOnOverrunError", "true");
            }
            else
            {
                SetAParameter("IntOnOverrunError", "false");
            }
        }

        private void m_chbxInterruptOnAddressMatch_CheckedChanged(object sender, EventArgs e)
        {
            if (m_chbxInterruptOnAddressMatch.Checked)
            {
                SetAParameter("IntOnAddressMatch", "true");
            }
            else
            {
                SetAParameter("IntOnAddressMatch", "false");
            }
        }

        private void m_chbxInterruptOnAddressDetect_CheckedChanged(object sender, EventArgs e)
        {
            if (m_chbxInterruptOnAddressDetect.Checked)
            {
                SetAParameter("IntOnAddressDetect", "true");
            }
            else
            {
                SetAParameter("IntOnAddressDetect", "false");
            }
        }

        private void m_numRXAddress1_ValueChanged(object sender, EventArgs e)
        {
            SetAParameter("Address1", m_numRXAddress1.Value.ToString());
        }

        private void m_numRXAddress2_ValueChanged(object sender, EventArgs e)
        {
            SetAParameter("Address2", m_numRXAddress2.Value.ToString());
        }

        private void m_cbRXAddressMode_SelectedIndexChanged(object sender, EventArgs e)
        {
            string prm = m_Component.ResolveEnumDisplayToId("RXAddressMode", m_cbRXAddressMode.Text);
            SetAParameter("RXAddressMode", prm);
            if (m_cbRXAddressMode.Text != "None")
            {
                m_numRXAddress1.Enabled = true;
                m_numRXAddress2.Enabled = true;
            }
            else
            {
                m_numRXAddress1.Enabled = false;
                m_numRXAddress2.Enabled = false;
            }
        }

        private void m_chbxInterruptOnTXComplete_CheckedChanged(object sender, EventArgs e)
        {
            if (m_chbxInterruptOnTXComplete.Checked)
            {
                SetAParameter("InterruptOnTXComplete", "true");
            }
            else
            {
                SetAParameter("InterruptOnTXComplete", "false");
            }
        }

        private void m_chbxInterruptOnTXFifoEmpty_CheckedChanged(object sender, EventArgs e)
        {
            if (m_chbxInterruptOnTXFifoEmpty.Checked)
            {
                SetAParameter("InterruptOnTXFifoEmpty", "true");
            }
            else
            {
                SetAParameter("InterruptOnTXFifoEmpty", "false");
            }
        }

        private void m_chbxInterruptOnTXFifoFull_CheckedChanged(object sender, EventArgs e)
        {
            if (m_chbxInterruptOnTXFifoFull.Checked)
            {
                SetAParameter("InterruptOnTXFifoFull", "true");
            }
            else
            {
                SetAParameter("InterruptOnTXFifoFull", "false");
            }
        }

        private void m_chbxInterruptOnTXFifoNotFull_CheckedChanged(object sender, EventArgs e)
        {
            if (m_chbxInterruptOnTXFifoNotFull.Checked)
            {
                SetAParameter("InterruptOnTXFifoNotFull", "true");
            }
            else
            {
                SetAParameter("InterruptOnTXFifoNotFull", "false");
            }
        }

        private void m_cbInternalInterruptsRXTX_EnableChanged(object sender, EventArgs e)
        {
            CheckBox cb = (CheckBox)sender;

            if (!cb.Enabled)
                cb.Checked = false;
        }

        private void m_cbInterrupts_EnableChanged(object sender, EventArgs e)
        {
            CheckBox cb = (CheckBox)sender;

            if (!cb.Enabled)
                cb.Checked = false;
        }

        private void m_rbExternalClock_CheckedChanged(object sender, EventArgs e)
        {
            if (ClockSelected != null)
            {
                ClockSelected(!m_rbExternalClock.Checked);
            }
            SetFrequencyLabel();
        }

        private void m_chbBreakDetected_CheckedChanged(object sender, EventArgs e)
        {
            bool isChecked = m_chbBreakDetected.Checked;
            m_Parameters.BreakDetect = isChecked;
            m_chbxInterruptOnBreak.Enabled = isChecked;
            if (!isChecked)
            {
                m_chbxInterruptOnBreak.Checked = false;
                SetAParameter("IntOnBreak", "false");
            }
        }

        private void m_chbUse23Polling_CheckedChanged(object sender, EventArgs e)
        {
            m_Parameters.Use23Polling = m_chbUse23Polling.Checked;
        }

        private void m_chbCRCoutputsEn_CheckedChanged(object sender, EventArgs e)
        {
            m_Parameters.CRCoutputsEn = m_chbCRCoutputsEn.Checked;
        }

        private void SetFrequencyLabel()//(int period)
        {
            if (m_rbExternalClock.Checked)
            {
                m_lblCalcFrequency.Visible = true;
                List<CyClockData> clkdata = new List<CyClockData>();
                clkdata = m_TermQuery.GetClockData("clock", 0);
                if (clkdata[0].IsFrequencyKnown)
                {
                    double infreq = clkdata[0].Frequency;
                    switch (clkdata[0].Unit)
                    {
                        case CyClockUnit.kHz: infreq = infreq * 1000; break;
                        case CyClockUnit.MHz: infreq = infreq * 1000000; break;
                    }
                    m_Parameters.ExternalClockFrequency = (int)infreq;
                    if (ExClockFrequencyChanged != null)
                    {
                        ExClockFrequencyChanged((int)infreq);
                    }
                    m_lblCalcFrequency.Text = string.Format("SOURCE FREQ = {0} {1}", Math.Round(clkdata[0].Frequency, 3)
                        , clkdata[0].Unit);
                    //Set the Tooltip m_lblCalcFrequency.To
                }
                else
                {
                    m_lblCalcFrequency.Text = "UNKNOWN SOURCE FREQ";
                }
            }
            else
                m_lblCalcFrequency.Visible = false;
        }

        protected override bool ProcessCmdKey(ref Message msg, Keys keyData)
        {
            if (keyData == Keys.Enter)
            {
                SendKeys.Send("{TAB}");
                return true;
            }
            else
                return base.ProcessCmdKey(ref msg, keyData);
        }

        private void m_numRXBufferSize_Validated(object sender, EventArgs e)
        {
            string str = ep_Errors.GetError((NumericUpDown)sender);
            if (String.IsNullOrEmpty(str))
            {
                SetAParameter("RXBufferSize", m_numRXBufferSize.Value.ToString());
                if (m_numRXBufferSize.Value < 5)
                {
                    SetAParameter("EnIntRXInterrupt", "false");
                }
                else
                {
                    SetAParameter("EnIntRXInterrupt", "true");
                }
            }
        }

        private void m_numTXBufferSize_Validated(object sender, EventArgs e)
        {
            string str = ep_Errors.GetError((NumericUpDown)sender);
            if (String.IsNullOrEmpty(str))
            {
                SetAParameter("TXBufferSize", m_numTXBufferSize.Value.ToString());
                if (m_numTXBufferSize.Value < 5)
                {
                    SetAParameter("EnIntTXInterrupt", "false");
                }
                else
                {
                    SetAParameter("EnIntTXInterrupt", "true");
                }
            }
        }

        private void BufferSizeChanged(object sender, EventArgs e)
        {
            m_lblTXStatus.Text = (m_numTXBufferSize.Value >= 5) ? "enabled" : "disabled";
            m_lblRXStatus.Text = (m_numRXBufferSize.Value >= 5) ? "enabled" : "disabled";
        }

    }
}
