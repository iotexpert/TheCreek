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
using System.Text;
using System.Windows.Forms;
using System.Diagnostics;

namespace CapSense_CSD_v2_0.Modules
{
    public partial class CyAliasDialog : Form
    {
        private CyCSParameters m_packParams;
        private CyWidget m_widget = null;

        internal CyAliasDialog(CyCSParameters packParams, CyWidget wi)
        {
            InitializeComponent();
            m_packParams = packParams;
            m_widget = wi;
            Debug.Assert(packParams != null);
            Debug.Assert(wi != null);

            m_aliasTextBox.Text = wi.m_name;

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
            UpdateError();
        }

        void UpdateError()
        {
            string newName = m_aliasTextBox.Text;
            if (m_packParams != null)
            {
                bool isNotError = false;
                if (m_widget != null)
                    isNotError = m_widget.m_name == newName ;
                isNotError = isNotError || m_packParams.m_widgets.NameValidating(newName);
                
                if (isNotError)
                {
                    m_errorProvider.SetError(m_aliasTextBox, string.Empty);
                    m_okButton.Enabled = true;
                }
                else
                {
                    m_errorProvider.SetError(m_aliasTextBox, CyCsResource.InvalidNameError);
                    m_okButton.Enabled = false;
                }
            }
        }

        private void m_okButton_Click(object sender, EventArgs e)
        {
            //Save new Name
            if (m_widget != null)
            {
                m_packParams.m_widgets.RenameWidget(m_widget, m_aliasTextBox.Text);
            }
        }
    }
}
