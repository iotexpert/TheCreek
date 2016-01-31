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
using System.Diagnostics;
using System.Data;
using System.Text;
using System.Windows.Forms;
using CyDesigner.Extensions.Common;
using CyDesigner.Extensions.Gde;
using CyCustomizer.SPI_Slave_v1_0;

namespace CyCustomizer.SPI_Slave_v1_0
{
    public partial class CySPISControlAdv : UserControl
    {
        public ICyInstEdit_v1 m_Component = null;
        public ICyTerminalQuery_v1 m_TermQuery = null;
        
        public CySPISControlAdv(ICyInstEdit_v1 inst, ICyTerminalQuery_v1 termquery)
        {
            m_Component = inst;
            m_TermQuery = termquery;
            //Trace.Assert(Debugger.IsAttached);
            InitializeComponent();
            InitializeFormComponents(inst);
            UpdateFormFromParams(m_Component);
        }

        public void InitializeFormComponents(ICyInstEdit_v1 inst)
        {
            //Nothing to Initialize
        }

        public void UpdateFormFromParams(ICyInstEdit_v1 inst)
        {
            SPISParameters prms = new SPISParameters(inst);

            m_numRXBufferSize.Value = Convert.ToInt16(prms.RxBufferSize.Value);
            m_numTXBufferSize.Value = Convert.ToInt16(prms.TxBufferSize.Value);

            m_chbxIntOnByteComp.Checked = prms.InterruptOnByteComplete.Value == "true" ? true : false;
            m_chbxIntOnRXEmpty.Checked = prms.InterruptOnRXEmpty.Value == "true" ? true : false;
            m_chbxIntOnRXOver.Checked = prms.InterruptOnRXOverrun.Value == "true" ? true : false;
            m_chbxIntOnSPIDone.Checked = prms.InterruptOnDone.Value == "true" ? true : false;

            m_chbxEnableInternalInterrupt.Checked = prms.UseInternalInterrupt.Value == "true" ? true : false;

            if (Convert.ToInt32(prms.TxBufferSize.Value) > 4)
                m_chbxIntOnTXNotFull.Checked = true;
            else
                m_chbxIntOnTXNotFull.Checked = prms.InterruptOnTXNotFull.Value == "true" ? true : false;

            m_chbxIntOnTXFull.Checked = prms.InterruptOnTXFull.Value == "true" ? true : false;

            if (Convert.ToInt32(prms.RxBufferSize.Value) > 4)
                m_chbxIntOnRXNotEmpty.Checked = true;
            else
                m_chbxIntOnRXNotEmpty.Checked = prms.InterruptOnRXNotEmpty.Value == "true" ? true : false;
        }

        private void m_numRXBufferSize_ValueChanged(object sender, EventArgs e)
        {
            if (m_numRXBufferSize.Value > 4 || m_numTXBufferSize.Value > 4)
            {
                m_chbxEnableInternalInterrupt.Checked = true;
                m_chbxEnableInternalInterrupt.Enabled = false;
                if (m_numRXBufferSize.Value > 4)
                {
                    m_chbxIntOnRXNotEmpty.Checked = true;
                    m_chbxIntOnRXNotEmpty.Enabled = false;
                }
                else
                {
                    m_chbxIntOnRXNotEmpty.Enabled = true;
                }
            }
            else
            {
                m_chbxEnableInternalInterrupt.Enabled = true;
                m_chbxIntOnRXNotEmpty.Enabled = true;
            }
            SetAParameter("RxBufferSize", m_numRXBufferSize.Value.ToString(), false);
        }

        private void m_numTXBufferSize_ValueChanged(object sender, EventArgs e)
        {
            if (m_numRXBufferSize.Value > 4 || m_numTXBufferSize.Value > 4)
            {
                m_chbxEnableInternalInterrupt.Checked = true;
                m_chbxEnableInternalInterrupt.Enabled = false;
                if (m_numTXBufferSize.Value > 4)
                {
                    m_chbxIntOnTXNotFull.Checked = true;
                    m_chbxIntOnTXNotFull.Enabled = false;
                }
                else
                {
                    m_chbxIntOnTXNotFull.Enabled = true;
                }
            }
            else
            {
                m_chbxEnableInternalInterrupt.Enabled = true;
                m_chbxIntOnTXNotFull.Enabled = true;
            }
            SetAParameter("TxBufferSize", m_numTXBufferSize.Value.ToString(), false);
        }


        private void SetAParameter(string parameter, string value, bool CheckFocus)
        {
            if (this.ContainsFocus || !CheckFocus)
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
        }

        private void m_chbxIntOnTXFull_CheckedChanged(object sender, EventArgs e)
        {
            SetAParameter("InterruptOnTXFull", m_chbxIntOnTXFull.Checked ? "true" : "false", false);
        }

        private void m_chbxIntOnSPIDone_CheckedChanged(object sender, EventArgs e)
        {
            SetAParameter("InterruptOnDone", m_chbxIntOnSPIDone.Checked ? "true" : "false", false);
        }

        private void m_chbxIntOnRXOver_CheckedChanged(object sender, EventArgs e)
        {
            SetAParameter("InterruptOnRXOverrun", m_chbxIntOnRXOver.Checked ? "true" : "false", false);
        }

        private void m_chbxIntOnRXEmpty_CheckedChanged(object sender, EventArgs e)
        {
            SetAParameter("InterruptOnRXEmpty", m_chbxIntOnRXEmpty.Checked ? "true" : "false", false);
        }

        private void m_chbxIntOnByteComp_CheckedChanged(object sender, EventArgs e)
        {
            SetAParameter("InterruptOnByteComplete", m_chbxIntOnByteComp.Checked ? "true" : "false", false);
        }

        private void m_chbxEnableInternalInterrupt_CheckedChanged(object sender, EventArgs e)
        {
            SetAParameter("UseInternalInterrupt", m_chbxEnableInternalInterrupt.Checked ? "true" : "false", false);
        }

        private void m_chbxIntOnTXNotFull_CheckedChanged(object sender, EventArgs e)
        {
            SetAParameter("InterruptOnTXNotFull", m_chbxIntOnTXNotFull.Checked ? "true" : "false", false);
        }

        private void m_chbxIntOnRXNotEmpty_CheckedChanged(object sender, EventArgs e)
        {
            SetAParameter("InterruptOnRXNotEmpty", m_chbxIntOnRXNotEmpty.Checked ? "true" : "false", false);
        }
    }
}
