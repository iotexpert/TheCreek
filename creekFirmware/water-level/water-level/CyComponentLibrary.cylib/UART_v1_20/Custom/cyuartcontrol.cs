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
using System.Diagnostics;
using CyDesigner.Extensions.Common;
using CyDesigner.Extensions.Gde;
using CyCustomizer.UART_v1_20;

namespace CyCustomizer.UART_v1_20
{
    public partial class cyuartcontrol : UserControl
    {
        public ICyInstEdit_v1 m_Component = null;
        public ICyTerminalQuery_v1 m_TermQuery = null;
        public delegate void paramsUpdated();
        public cyuartadvancedcontrol m_control_adv;

        private UARTParameters m_Parameters;

        public cyuartcontrol(
            ICyInstEdit_v1 inst,
            ICyTerminalQuery_v1 termquery,
            UARTParameters parameters,
            cyuartadvancedcontrol control_adv)
        {
            m_Component = inst;
            m_TermQuery = termquery;
            m_control_adv = control_adv;
            m_Parameters = parameters;

            InitializeComponent();

            //Set the Baud Rate (BPS) Combo Box
            bool isInternalClockSelected = bool.Parse(m_Parameters.InternalClock.Value);
            CheckInternalClock(isInternalClockSelected);            

            IEnumerable<string> BaudRateEnums = inst.GetPossibleEnumValues("BaudRate");
            foreach (string str in BaudRateEnums)
            {
                m_cbBitsPerSecond.Items.Add(str);
            }
            //Set the Data Bits Combo Box
            IEnumerable<string> DataBitsEnums = inst.GetPossibleEnumValues("NumDataBits");
            foreach (string str in DataBitsEnums)
            {
                m_cbDataBits.Items.Add(str);
            }
            //Set the Parity Combo Box
            IEnumerable<string> ParityEnums = inst.GetPossibleEnumValues("ParityType");
            foreach (string str in ParityEnums)
            {
                m_cbParity.Items.Add(str);
            }
            //Set the Stop Bits Combo Box
            IEnumerable<string> StopBitsEnums = inst.GetPossibleEnumValues("NumStopBits");
            foreach (string str in StopBitsEnums)
            {
                m_cbStopBits.Items.Add(str);
            }
            //Set the Flow Control Combo Box
            IEnumerable<string> FlowControlEnums = inst.GetPossibleEnumValues("FlowControl");
            foreach (string str in FlowControlEnums)
            {
                m_cbFlowControl.Items.Add(str);
            }
            if(m_Component != null)
                UpdateFormFromParams(inst);
        }

        public void CheckInternalClock(bool isInternalClockSelected)
        {
            m_lblExternalClockMessage.Visible = !isInternalClockSelected;
            m_cbBitsPerSecond.Visible = isInternalClockSelected;
        }

        public void UpdateExternalClockFrequency(float externalClockFrequency)
        {
            decimal frq = 0;
            string units = string.Empty;

            if (externalClockFrequency > 999999)
            {
                frq = Decimal.Divide(
                    (Decimal)(externalClockFrequency),
                    (Decimal)(1000000));
                units = "MBaud";
            }
            else
            {
                frq = Decimal.Divide((Decimal)(externalClockFrequency), (Decimal)(1000));
                units = "KBaud";
            }

            frq /= 8;

            m_lblExternalClockMessage.Text =
                   "1/8 Input Clock Frequency (" +
                   frq.ToString() +
                   " "+
                   units +
                   ")";
        }

        public void UpdateFormFromParams(ICyInstEdit_v1 inst)
        {
            //Set the RX/TX Radio Buttons
            if (m_Parameters.HalfDuplexEnable == true)
            {
                m_rbHALF.Checked = true;
            }
            else
            if (m_Parameters.RXEnable == true)
            {
                if (m_Parameters.TXEnable == true)
                    m_rbRXTX.Checked = true;
                else
                    m_rbRXOnly.Checked = true;
            }
            else
                m_rbTXOnly.Checked = true;

            //Set the Baud Rate (BPS) Combo Box
            

            IEnumerable<string> BaudRateEnums = inst.GetPossibleEnumValues("BaudRate");
            bool baudfound = false;
            foreach (string str in BaudRateEnums)
            {
                if (!baudfound)
                {
                    string paramcompare = m_Component.ResolveEnumIdToDisplay("BaudRate", m_Parameters.BaudRate.Expr);
                    if (paramcompare == str)
                    {
                        m_cbBitsPerSecond.SelectedItem = paramcompare;
                        baudfound = true;
                    }
                }
            }
            if (!baudfound)
            {
                m_cbBitsPerSecond.Text = m_Parameters.BaudRate.Expr;
            }

            //Set the Data Bits Combo Box
            IEnumerable<string> DataBitsEnums = inst.GetPossibleEnumValues("NumDataBits");
            bool DBfound = false;
            foreach (string str in DataBitsEnums)
            {
                if (!DBfound)
                {
                    string paramcompare = m_Component.ResolveEnumIdToDisplay("NumDataBits", m_Parameters.NumDataBits.Expr);
                    if (paramcompare  == str)
                    {
                        m_cbDataBits.SelectedItem = paramcompare;
                        DBfound = true;
                    }
                }
            }
            if (!DBfound)
            {
                m_cbDataBits.Text = m_Parameters.NumDataBits.Expr;
            }
            //Set the Parity Combo Box
            IEnumerable<string> ParityEnums = inst.GetPossibleEnumValues("ParityType");
            bool Parityfound = false;
            foreach (string str in ParityEnums)
            {
                if (!Parityfound)
                {
                    string paramcompare = m_Component.ResolveEnumIdToDisplay("ParityType", m_Parameters.ParityType.Expr);
                    if (str == paramcompare)
                    {
                        m_cbParity.SelectedItem = paramcompare;
                        Parityfound = true;
                    }
                }
            }
            if (!Parityfound)
            {
                m_cbParity.Text = m_Parameters.ParityType.Expr;
            }
            //Set the Stop Bits Combo Box
            IEnumerable<string> StopBitsEnums = inst.GetPossibleEnumValues("NumStopBits");
            bool SBfound = false;
            foreach (string str in StopBitsEnums)
            {
                if (!SBfound)
                {
                    string paramcompare = m_Component.ResolveEnumIdToDisplay("NumStopBits", m_Parameters.NumStopBits.Expr);
                    if (paramcompare == str)
                    {
                        m_cbStopBits.SelectedItem = paramcompare;
                        SBfound = true;
                    }
                }
            }
            if (!SBfound)
            {
                m_cbStopBits.Text = m_Parameters.NumStopBits.Expr;
            }
            
            //Set the Flow Control Combo Box
            IEnumerable<string> FlowControlEnums = inst.GetPossibleEnumValues("FlowControl");
            bool FCfound = false;
            foreach (string str in FlowControlEnums)
            {
                if (!FCfound)
                {
                    string paramcompare = m_Component.ResolveEnumIdToDisplay("FlowControl", m_Parameters.FlowControl.Expr);
                    if (paramcompare == str)
                    {
                        m_cbFlowControl.SelectedItem = paramcompare;
                        FCfound = true;
                    }
                }
            }
            if (!FCfound)
            {
                m_cbFlowControl.Text = m_Parameters.FlowControl.Expr;
            }

            UpdateExternalClockFrequency(m_Parameters.ExternalClockFrequency);
        }

        private void m_cbBitsPerSecond_SelectedIndexChanged(object sender, EventArgs e)
        {
            string prm = m_Component.ResolveEnumDisplayToId("BaudRate", m_cbBitsPerSecond.Text);
            SetAParameter("BaudRate", prm);
        }

        private void m_cbDataBits_SelectedIndexChanged(object sender, EventArgs e)
        {
            string prm = m_Component.ResolveEnumDisplayToId("NumDataBits", m_cbDataBits.Text);
            SetAParameter("NumDataBits", prm);
        }

        private void m_cbParity_SelectedIndexChanged(object sender, EventArgs e)
        {
            string prm = m_Component.ResolveEnumDisplayToId("ParityType", m_cbParity.Text);
            SetAParameter("ParityType", prm);
        }

        private void m_cbStopBits_SelectedIndexChanged(object sender, EventArgs e)
        {
            string prm = m_Component.ResolveEnumDisplayToId("NumStopBits", m_cbStopBits.Text);
            SetAParameter("NumStopBits", prm);
        }

        private void m_cbFlowControl_SelectedIndexChanged(object sender, EventArgs e)
        {
            string prm = m_Component.ResolveEnumDisplayToId("FlowControl", m_cbFlowControl.Text);
            SetAParameter("FlowControl", prm);
        }

        private void SetAParameter(string parameter, string value)
        {
            if (this.ContainsFocus)
            {
                //TODO: Verify that the parameter was set correctly.
                m_Component.SetParamExpr(parameter, value);
                m_Component.CommitParamExprs();
                if (m_Component.GetCommittedParam(parameter).ErrorCount != 0)
                {
                    string errors = null;
                    foreach (string err in m_Component.GetCommittedParam(parameter).Errors)
                    {
                        errors = errors + err + "\n";
                    }
                    MessageBox.Show(string.Format("Error Setting Parameter {0} with value {1}\n\nErrors:\n{2}",parameter,value,errors), 
                        "Error Setting Parameter", MessageBoxButtons.OK, MessageBoxIcon.Error);
                }
            }
        }

        private void m_rbRXOnly_Click(object sender, EventArgs e)
        {
            if (!m_rbRXOnly.Checked)
            {
                m_rbRXOnly.Checked = true;
                m_rbRXTX.Checked = false;
                m_rbTXOnly.Checked = false;
                m_rbHALF.Checked = false;

                m_Parameters.TXEnable = false;
                m_Parameters.RXEnable = true;
                m_Parameters.HalfDuplexEnable = false;

                m_control_adv.UpdateFormFromParams(m_Component);
            }
        }

        private void m_rbTXOnly_Click(object sender, EventArgs e)
        {
            if (!m_rbTXOnly.Checked)
            {
                m_rbRXOnly.Checked = false;
                m_rbRXTX.Checked = false;
                m_rbTXOnly.Checked = true;
                m_rbHALF.Checked = false;

                m_Parameters.TXEnable = true;
                m_Parameters.RXEnable = false;
                m_Parameters.HalfDuplexEnable = false;

                m_control_adv.UpdateFormFromParams(m_Component);
            }
        }

        private void m_rbRXTX_Click(object sender, EventArgs e)
        {
            if (!m_rbRXTX.Checked)
            {
                m_rbRXOnly.Checked = false;
                m_rbRXTX.Checked = true;
                m_rbTXOnly.Checked = false;
                m_rbHALF.Checked = false;

                m_Parameters.TXEnable = true;
                m_Parameters.RXEnable = true;
                m_Parameters.HalfDuplexEnable = false;

                m_control_adv.UpdateFormFromParams(m_Component);
            }
        }


        private void m_rbHALF_Click(object sender, EventArgs e)
        {
            if (!m_rbHALF.Checked)
            {
                m_rbRXOnly.Checked = false;
                m_rbRXTX.Checked = false;
                m_rbTXOnly.Checked = false;
                m_rbHALF.Checked = true;

                m_Parameters.TXEnable = false;
                m_Parameters.RXEnable = true;
                m_Parameters.HalfDuplexEnable = true;

                m_control_adv.UpdateFormFromParams(m_Component);
            }

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
    }
}
