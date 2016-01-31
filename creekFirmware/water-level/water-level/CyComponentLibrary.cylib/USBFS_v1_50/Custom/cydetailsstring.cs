/*******************************************************************************
* Copyright 2008-2010, Cypress Semiconductor Corporation.  All rights reserved.
* You may use this file only in accordance with the license, terms, conditions, 
* disclaimers, and limitations in the end user license agreement accompanying 
* the software package with which this file was provided.
********************************************************************************/



using System;
using System.Windows.Forms;

namespace USBFS_v1_50
{
    public partial class CyDetailsString : UserControl
    {
        public CyStringDescriptor m_descriptor;
        private readonly CyUSBFSParameters m_parameters;
        public event EventHandler UpdateNodeEvent;

        public CyDetailsString(CyStringDescriptor descriptor, CyUSBFSParameters parameters)
        {
            InitializeComponent();
            m_descriptor = descriptor;
            m_parameters = parameters;
        }

        protected virtual void OnUpdateNodeEvent(EventArgs e)
        {
            EventHandler Handler = UpdateNodeEvent;
            if (Handler != null) Handler(this, e);
        }


        public void SetGroupBoxVisible()
        {
            groupBoxOption.Visible = true;
            switch (m_descriptor.snType)
            {
                case CyStringGenerationType.USER_ENTERED_TEXT:
                    radioButtonUser.Checked = true;
                    break;
                case CyStringGenerationType.USER_CALL_BACK:
                    radioButtonCallback.Checked = true;
                    break;
                case CyStringGenerationType.SILICON_NUMBER:
                    radioButtonSilicon.Checked = true;
                    break;
                default:
                    radioButtonUser.Checked = true;
                    break;
            }
        }

        public void SetTextBoxDefault()
        {
            textBoxString.Enabled = false;
        }

        private void CyDetailsString_Load(object sender, EventArgs e)
        {
            textBoxString.Text = m_descriptor.bString;
        }

        private void textBoxString_Validated(object sender, EventArgs e)
        {
            m_descriptor.bString = textBoxString.Text;
            m_descriptor.bLength = (byte)(m_descriptor.bString.Length + 2);
            m_parameters.ParamStringTreeChanged = true;
            OnUpdateNodeEvent(EventArgs.Empty);
        }

        private void radioButtonUser_CheckedChanged(object sender, EventArgs e)
        {
            if (!((RadioButton)sender).Checked)
                return;

            if (sender == radioButtonUser)
            {
                textBoxString.Enabled = true;
                m_descriptor.snType = CyStringGenerationType.USER_ENTERED_TEXT;
            }
            else if (sender == radioButtonCallback)
            {
                textBoxString.Enabled = false;
                m_descriptor.snType = CyStringGenerationType.USER_CALL_BACK;
            }
            else if (sender == radioButtonSilicon)
            {
                textBoxString.Enabled = false;
                m_descriptor.snType = CyStringGenerationType.SILICON_NUMBER;
            }
            m_parameters.ParamStringTreeChanged = true;
        }
    }
}
