/*******************************************************************************
* Copyright 2011-2012, Cypress Semiconductor Corporation.  All rights reserved.
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

namespace emFile_v1_10
{
    public partial class CyEmFileGeneralTab : UserControl, ICyParamEditingControl
    {
        CyEmFileParameters m_params;

        #region Constructor(s)
        public CyEmFileGeneralTab(CyEmFileParameters inst)
        {
            m_params = inst;
            m_params.m_generalTab = this;
            InitializeComponent();

            // Set numerics min & max values
            numMaxSPIFrequency.Minimum = int.MinValue;
            numMaxSPIFrequency.Maximum = int.MaxValue;

            // Event handlers declaration
            numMaxSPIFrequency.TextChanged += new EventHandler(numMaxSPIFrequency_TextChanged);
            //cbSDCardsNumber.TextChanged += new EventHandler(cbSDCardsNumber_TextChanged);
            cbSDCardsNumber.SelectedIndexChanged += new EventHandler(cbSDCardsNumber_SelectedIndexChanged);
            chbSDCard0WP.CheckedChanged += new EventHandler(chbSDCard0WP_CheckedChanged);
            chbSDCard1WP.CheckedChanged += new EventHandler(chbSDCard1WP_CheckedChanged);
            chbSDCard2WP.CheckedChanged += new EventHandler(chbSDCard2WP_CheckedChanged);
            chbSDCard3WP.CheckedChanged += new EventHandler(chbSDCard3WP_CheckedChanged);

            // Fill number SD cards combobox
            for (int i = CyParamRanges.SD_CARD_NUMBER_MIN; i <= CyParamRanges.SD_CARD_NUMBER_MAX; i++)
                cbSDCardsNumber.Items.Add(i.ToString());
        }
        #endregion

        #region ICyParamEditingControl Members
        public Control DisplayControl
        {
            get { return this; }
        }

        // Gets any errors that exist for parameters on the DisplayControl.
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
            // Updating Frequency
            numMaxSPIFrequency.Text = m_params.MaxSPIFrequency.ToString();

            // Updating SD Cards Number and Write Protection checkboxes visibility respectively
            cbSDCardsNumber.SelectedIndex = m_params.NumberSDCards - 1;
            SetSDCardsWPCheckboxesVisibility(m_params.NumberSDCards);

            // Updating Write Protection
            chbSDCard0WP.Checked = m_params.WP0Enable;
            chbSDCard1WP.Checked = m_params.WP1Enable;
            chbSDCard2WP.Checked = m_params.WP2Enable;
            chbSDCard3WP.Checked = m_params.WP3Enable;
        }
        #endregion

        #region Event Handlers
        void numMaxSPIFrequency_TextChanged(object sender, EventArgs e)
        {
            Control control = (Control)sender;
            try
            {
                // Assign control value to parameter
                if (control.Text.Trim() != string.Empty)
                    m_params.MaxSPIFrequency = Convert.ToInt32(control.Text);

                // Highlight error provider if MaxSPIFrequency value is invalid
                ShowErrorProviderOnError(control, CyParamRanges.MAX_SPI_FREQUENCY_MIN,
                    CyParamRanges.MAX_SPI_FREQUENCY_MAX, Resources.InvalidMaxSPIFrequency);
            }
            catch (Exception)
            {
                Debug.Assert(false);
            }
        }

        void cbSDCardsNumber_SelectedIndexChanged(object sender, EventArgs e)
        {
            ComboBox control = (ComboBox)sender;
            try
            {
                // Assign control value to parameter
                if (control.Text != string.Empty)
                    m_params.NumberSDCards = Convert.ToInt32(control.Text);

                // Highlight error provider if SDCardsNumber value is invalid
                ShowErrorProviderOnError(control, CyParamRanges.SD_CARD_NUMBER_MIN, CyParamRanges.SD_CARD_NUMBER_MAX,
                    Resources.InvalidSDCardsNumber);

                // Change SD Card Write Protection checkboxes enabling
                if (m_params.m_globalEditMode)
                    SetSDCardsWPCheckboxesVisibility(m_params.NumberSDCards);
            }
            catch (Exception)
            {
                Debug.Assert(false);
            }
        }

        private void ShowErrorProviderOnError(Control control, int minRange, int maxRange, string errorMessage)
        {
            int currentControlValue;

            // Get current control value
            if (control.Text.Trim() == string.Empty)
                currentControlValue = -1; // assign some invalid value if empty
            else
                currentControlValue = Convert.ToInt32(control.Text);

            // Highlight error provider message if current control value is invalid
            if (currentControlValue < minRange || currentControlValue > maxRange)
                errorProvider.SetError(control, string.Format(errorMessage, minRange, maxRange));
            else
                errorProvider.SetError(control, string.Empty);
        }

        private void SetSDCardsWPCheckboxesVisibility(int sdCardsNumber)
        {
            bool[] wpArray = new bool[CyParamRanges.SD_CARD_NUMBER_MAX];

            for (int i = 0; i < sdCardsNumber; i++)
            {
                wpArray[i] = true;
            }

            chbSDCard0WP.Visible = wpArray[0];
            chbSDCard1WP.Visible = wpArray[1];
            chbSDCard2WP.Visible = wpArray[2];
            chbSDCard3WP.Visible = wpArray[3];
        }

        void chbSDCard0WP_CheckedChanged(object sender, EventArgs e)
        {
            m_params.WP0Enable = ((CheckBox)sender).Checked;
        }

        void chbSDCard1WP_CheckedChanged(object sender, EventArgs e)
        {
            m_params.WP1Enable = ((CheckBox)sender).Checked;
        }

        void chbSDCard2WP_CheckedChanged(object sender, EventArgs e)
        {
            m_params.WP2Enable = ((CheckBox)sender).Checked;
        }

        void chbSDCard3WP_CheckedChanged(object sender, EventArgs e)
        {
            m_params.WP3Enable = ((CheckBox)sender).Checked;
        }
        #endregion
    }
}
