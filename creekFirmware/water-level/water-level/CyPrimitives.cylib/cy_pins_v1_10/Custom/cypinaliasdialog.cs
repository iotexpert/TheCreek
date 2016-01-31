/*******************************************************************************
* Copyright 2008-2009, Cypress Semiconductor Corporation.  All rights reserved.
* You may use this file only in accordance with the license, terms, conditions, 
* disclaimers, and limitations in the end user license agreement accompanying 
* the software package with which this file was provided.
********************************************************************************/

using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Text;
using System.Windows.Forms;

using Cypress.Comps.PinsAndPorts.Common;
using CyDesigner.Extensions.Common;

namespace Cypress.Comps.PinsAndPorts.cy_pins_v1_10
{
    public partial class CyPinAliasDialog : Form
    {
        CyStringArrayParamData m_data;
        int m_index ;

        internal CyPinAliasDialog(CyStringArrayParamData aliasData, int index)
        {
            InitializeComponent();

            if (aliasData == null) throw new ArgumentNullException();

            m_data = aliasData;
            m_index = index;
            m_aliasTextBox.Text = m_data.GetValue(m_index);
            m_errorProvider.SetIconAlignment(m_aliasTextBox, ErrorIconAlignment.MiddleLeft);
            m_errorProvider.SetIconPadding(m_aliasTextBox, 3);
            UpdateError();

            m_aliasTextBox.TextChanged += new EventHandler(m_aliasTextBox_TextChanged);
            m_aliasTextBox.Validating += new CancelEventHandler(m_aliasTextBox_Validating);
        }

        void m_aliasTextBox_Validating(object sender, CancelEventArgs e)
        {
            string errMsg = m_errorProvider.GetError(m_aliasTextBox);
            e.Cancel = (string.IsNullOrEmpty(errMsg) == false);
        }

        void m_aliasTextBox_TextChanged(object sender, EventArgs e)
        {
            m_data.SetValue(m_index, m_aliasTextBox.Text);
            UpdateError();
        }

        void UpdateError()
        {
            CyCustErr err = m_data.Validate();
            if (err.IsNotOk)
            {
                m_errorProvider.SetError(m_aliasTextBox, err.Message);
                m_okButton.Enabled = false;
            }
            else
            {
                m_errorProvider.SetError(m_aliasTextBox, string.Empty);
                m_okButton.Enabled = true;
            }
        }

        internal CyStringArrayParamData GetAlias()
        {
            return m_data;
        }
    }
}
