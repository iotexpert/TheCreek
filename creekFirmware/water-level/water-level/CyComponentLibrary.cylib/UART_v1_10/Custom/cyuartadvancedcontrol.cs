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
using CyCustomizer.UART_v1_10;

namespace CyCustomizer.UART_v1_10
{
    public partial class cyuartadvancedcontrol : UserControl
    {
        public ICyInstEdit_v1 m_Component = null;
        public ICyTerminalQuery_v1 m_TermQuery = null;
        public event ClockChangeDelegate ClockSelected;

        private UARTParameters m_Parameters;


        public cyuartadvancedcontrol(ICyInstEdit_v1 inst, ICyTerminalQuery_v1 termquery)
        {
            m_Component = inst;
            m_TermQuery = termquery;
            m_Parameters = new UARTParameters(inst);
            InitializeComponent();
            //Set the RX Address Modes Combo Box
            IEnumerable<string> RXAddressModes = inst.GetPossibleEnumValues("RXAddressMode");
            foreach (string str in RXAddressModes)
            {
                m_cbRXAddressMode.Items.Add(str);
            }
            UpdateFormFromParams(inst);
        }

        public void UpdateFormFromParams(ICyInstEdit_v1 inst)
        {
            //Set the Break Enable checkbox
            m_chbBreakDetected.Checked = m_Parameters.BreakDetect;

            //Set the 2OutOf3Votting checkbox
            m_chbUse23Polling.Checked = m_Parameters.Use23Polling;

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

            //Set the TX Internal ISR Enable Check Box
            UpdateTXEnable(m_Parameters);

            //Set the RX Internal ISR Enable Check Box
            UpdateRXEnable(m_Parameters);

            //Set the Hardware TX Enable Check Box
            if (m_Parameters.TXEnable.Expr == "true")
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
            
            if (m_Parameters.RXEnable.Expr == "true")
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


            //Set the TX Buffer Size Numeric Up/Down
            m_numTXBufferSize.Minimum = 1;
            m_numTXBufferSize.Maximum = m_Parameters.TXBufferSizeMax;

            if (m_Parameters.TXEnable.Expr == "true")
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

            if (m_Parameters.RXEnable.Expr == "true")
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


            if (m_Parameters.TXEnable.Expr == "true")
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

            if (m_Parameters.RXEnable.Expr == "true")
            {
                //Set the RXAddressmode Combo Box
                IEnumerable<string> RXAddressModes = inst.GetPossibleEnumValues("RXAddressMode");
                bool addmode = false;
                foreach (string str in RXAddressModes)
                {
                    if (!addmode)
                    {
                        string paramcompare = m_Component.ResolveEnumIdToDisplay("RXAddressMode", m_Parameters.RXAddressMode.Expr);
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
        }

        private void UpdateRXEnable(UARTParameters m_Parameters)
        {
            if (m_Parameters.RXEnable.Expr == "true" && m_numRXBufferSize.Value >= 5)
            {
                if (m_Parameters.EnIntRXInterrupt.Expr == "true")
                {
                    m_cbRXInternalInterrupt.Checked = true;
                }
                else if (m_Parameters.EnIntRXInterrupt.Expr == "false")
                {
                    m_cbRXInternalInterrupt.Checked = false;
                }
                else
                {
                    m_cbRXInternalInterrupt.Checked = false;
                }
            }
        }

        private void UpdateTXEnable(UARTParameters m_Parameters)
        {
            if ( (m_Parameters.TXEnable.Expr == "true") && (m_numTXBufferSize.Value >= 5) )
            {
                if (m_Parameters.EnIntTXInterrupt.Expr == "true")
                {
                    m_cbTXInternalInterrupt.Checked = true;
                }
                else if (m_Parameters.EnIntTXInterrupt.Expr == "false")
                {
                    m_cbTXInternalInterrupt.Checked = false;
                }
                else
                {
                    m_cbTXInternalInterrupt.Checked = false;
                }
            }
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
                MessageBox.Show(string.Format("Error Setting Parameter {0} with value {1}\n\nErrors:\n{2}", parameter, value, errors),
                    "Error Setting Parameter", MessageBoxButtons.OK, MessageBoxIcon.Error);
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

        private void m_cbTXInternalInterrupt_CheckedChanged(object sender, EventArgs e)
        {
            CheckBox cb = (CheckBox)sender;

            if (m_numTXBufferSize.Value >= 5 && cb.Enabled)
            {
                m_cbTXInternalInterrupt.Checked = true;
            }
        }

        private void m_cbRXInternalInterrupt_CheckedChanged(object sender, EventArgs e)
        {
            CheckBox cb = (CheckBox)sender;

            if (m_numRXBufferSize.Value >= 5 && cb.Enabled)
            {
                m_cbRXInternalInterrupt.Checked = true;
            }
        }

        private void m_numRXBufferSize_ValueChanged(object sender, EventArgs e)
        {
            SetAParameter("RXBufferSize", m_numRXBufferSize.Value.ToString());
            if (m_numRXBufferSize.Value < 5)
            {
                m_cbRXInternalInterrupt.Checked = false;
                SetAParameter("EnIntRXInterrupt", "false");
            }
            else
            {
                m_cbRXInternalInterrupt.Checked = true;
                SetAParameter("EnIntRXInterrupt", "true");
            }
        }

        private void m_numTXBufferSize_ValueChanged(object sender, EventArgs e)
        {
            SetAParameter("TXBufferSize", m_numTXBufferSize.Value.ToString());
            if (m_numTXBufferSize.Value < 5)
            {
                m_cbTXInternalInterrupt.Checked = false;
                SetAParameter("EnIntTXInterrupt", "false");
            }
            else
            {
                m_cbTXInternalInterrupt.Checked = true;
                SetAParameter("EnIntTXInterrupt", "true");
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
        }

        private void m_numRXBufferSize_KeyUp(object sender, KeyEventArgs e)
        {
            if (m_numRXBufferSize.Value > m_Parameters.RXBufferSizeMax)
            {
                m_numRXBufferSize.Value = m_Parameters.RXBufferSizeMax;
            }
        }

        private void m_numTXBufferSize_KeyUp(object sender, KeyEventArgs e)
        {
            if (m_numTXBufferSize.Value > m_Parameters.TXBufferSizeMax)
            {
                m_numTXBufferSize.Value = m_Parameters.TXBufferSizeMax;
            }
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

    }
}
