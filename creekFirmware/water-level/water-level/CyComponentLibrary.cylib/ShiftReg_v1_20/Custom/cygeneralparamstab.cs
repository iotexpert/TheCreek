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
using CyDesigner.Extensions.Gde;
using CyDesigner.Extensions.Common;

namespace ShiftReg_v1_20
{
    public partial class CyGeneralParamsTab : UserControl, ICyParamEditingControl
    {
        CySRParameters m_Parameters;
        Control m_control;
        string[] m_strFifoSize = new string[2] { "1", "4" };

        public CyGeneralParamsTab(CySRParameters inst)
        {
            InitializeComponent();

            for (int i = 1; i <= 32; i++)
                cbLength.Items.Add(i);
            this.Leave += new System.EventHandler(Compon_Leave);


            //InputDefault
            cbLength.SelectedIndex = 0;
            cbDir.SelectedIndex = 0;
            cbFIFOS.SelectedIndex = 0;

            ((CySRParameters)inst).m_GeneralParams = this;
            m_Parameters = ((CySRParameters)inst);
            m_control = this;
            m_control.Dock = DockStyle.Fill;

            //Assign Commit Action to all editable controls in the form
            foreach (Control cnt in gbCustParams.Controls)
            {
                inst.AssigneComit(cnt);
            }
            foreach (Control cnt in gbGeneral.Controls)
            {
                inst.AssigneComit(cnt);
            }
            foreach (Control cnt in gbShIn.Controls)
            {
                inst.AssigneComit(cnt);
            }
            foreach (Control cnt in gbInter.Controls)
            {
                inst.AssigneComit(cnt);
            }
        }

        private void cbShiftIn_CheckedChanged(object sender, EventArgs e)
        {
            if ((!cbShiftIn.Checked) && (!cbShiftOut.Checked))
                ((CheckBox)sender).Checked = true;
            gbShIn.Enabled = !cbShiftIn.Checked;

        }
        private void Compon_Leave(object sender, EventArgs e)
        {
            m_Parameters.SetCommitParams(null, null);
        }

        #region ICyParamEditingControl Members
        public Control DisplayControl
        {
            get { return m_control; }
        }

        public IEnumerable<CyCustErr> GetErrors()
        {
            return new CyCustErr[] { };    //return an empty array
        }
        #endregion

        private void cbInterrupt_CheckedChanged(object sender, EventArgs e)
        {
            gbInter.Enabled = cbInterrupt.Checked;
        }

        private void cbLoad_CheckedChanged(object sender, EventArgs e)
        {
            //Option of selecting FIFO size is disabled when load and store are not used (CDT 59463)
            cbFIFOS.Enabled = cbLoad.Checked || cbStore.Checked;
            lFIFOSize.Enabled = cbLoad.Checked || cbStore.Checked;
        }
    }
}

