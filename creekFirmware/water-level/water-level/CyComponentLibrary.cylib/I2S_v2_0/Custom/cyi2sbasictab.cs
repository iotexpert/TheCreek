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
using System.Data;
using System.Text;
using System.Windows.Forms;
using CyDesigner.Extensions.Common;
using CyDesigner.Extensions.Gde;

namespace I2S_v2_0
{
    public partial class CyI2SBasic : UserControl, ICyParamEditingControl
    {
        private CyI2SParameters m_params;

        public CyI2SBasic(CyI2SParameters inst)
        {
            InitializeComponent();

            inst.m_basicParams = this;
            this.Dock = DockStyle.Fill;
            m_params = inst;
        }

        #region ICyParamEditingControl Members
        public Control DisplayControl
        {
            get { return this; }
        }

        public IEnumerable<CyCustErr> GetErrors()
        {
            List<CyCustErr> errs = new List<CyCustErr>();

            foreach (string paramName in m_params.m_inst.GetParamNames())
            {
                CyCompDevParam param = m_params.m_inst.GetCommittedParam(paramName);
                if (param.TabName.Equals("Basic"))
                {
                    if (param.ErrorCount > 0)
                    {
                        foreach (string errMsg in param.Errors)
                        {
                            errs.Add(new CyCustErr(errMsg));
                        }
                    }
                }
            }

            return errs;
        }
        #endregion

        #region Assigning parameters values to controls
        public void GetParams()
        {
            // Direction
            switch (m_params.Direction)
            {
                case E_DIRECTION.Rx:
                    radioButtonRxOnly.Checked = true;
                    break;
                case E_DIRECTION.Tx:
                    radioButtonTxOnly.Checked = true;
                    break;
                case E_DIRECTION.Rx_and_Tx:
                    radioButtonRxAndTx.Checked = true;
                    break;
            }

            // DataBits
            comboBoxDataBits.Text = m_params.DataBits.ToString();

            // Word Select Period
            comboBoxWordSelectPeriod.Text = m_params.WordSelectPeriod.ToString();
        }
        #endregion

        #region Assigning controls values to parameters
        private void SetDirection()
        {
            if (radioButtonRxOnly.Checked)
                m_params.Direction = E_DIRECTION.Rx;
            else if (radioButtonTxOnly.Checked)
                m_params.Direction = E_DIRECTION.Tx;
            else if (radioButtonRxAndTx.Checked)
                m_params.Direction = E_DIRECTION.Rx_and_Tx;
            else return;
            m_params.m_advancedParams.SetEnableDisable(m_params.Direction);
            m_params.SetParams(CyParamNames.DIRECTION);
        }

        private void SetDataBits()
        {
            m_params.DataBits = int.Parse(comboBoxDataBits.Text);
            m_params.SetParams(CyParamNames.DATA_BITS);
        }

        private void SetWordSelectPeriod()
        {
            m_params.WordSelectPeriod = int.Parse(comboBoxWordSelectPeriod.Text);
            m_params.SetParams(CyParamNames.WORD_SELECT);
        }
        #endregion

        #region Event Handlers
        private void radioButtonDirection_CheckedChanged(object sender, EventArgs e)
        {
            SetDirection();
        }

        private void comboBoxDataBits_SelectedIndexChanged(object sender, EventArgs e)
        {
            SetDataBits();
            // Add or Remove SelectPeriod dropdown list items, depending on DataBits value
            if (this.Visible)
            {
                if ((int.Parse(comboBoxDataBits.Text)) > (m_params.WordSelectPeriod / 2))
                {
                    comboBoxWordSelectPeriod.SelectedIndex += 1;
                }
                else
                {
                    ShowImage(int.Parse(comboBoxDataBits.Text), int.Parse(comboBoxWordSelectPeriod.Text));
                }
            }
        }

        private void comboBoxWordSelectPeriod_SelectedIndexChanged(object sender, EventArgs e)
        {
            CancelEventArgs ce = new CancelEventArgs();
            WordSelectPeriodValidating(sender, ce);
            if (ce.Cancel == false)
            {
                SetWordSelectPeriod();
                // Changing image, according to values selected in dropdown lists
                ShowImage(int.Parse(comboBoxDataBits.Text), int.Parse(comboBoxWordSelectPeriod.Text));
            }
        }

        private void WordSelectPeriodValidating(object sender, CancelEventArgs e)
        {
            if ((int.Parse(comboBoxDataBits.Text)) <= (int.Parse(comboBoxWordSelectPeriod.Text) / 2))
            {
                errorProvider.SetError(comboBoxWordSelectPeriod, "");
            }
            else
            {
                errorProvider.SetError(comboBoxWordSelectPeriod,
                    Properties.Resources.WordSelectEPMsg);
                e.Cancel = true;
            }
        }

        private void CyI2SBasic_VisibleChanged(object sender, EventArgs e)
        {
            CancelEventArgs ce = new CancelEventArgs();
            if ((int.Parse(comboBoxDataBits.Text)) > (int.Parse(comboBoxWordSelectPeriod.Text) / 2))
                comboBoxWordSelectPeriod.Focus();
            WordSelectPeriodValidating(comboBoxWordSelectPeriod, ce);
            if (ce.Cancel == false)
            {
                // Changing image, according to values selected in dropdown lists
                ShowImage(int.Parse(comboBoxDataBits.Text), int.Parse(comboBoxWordSelectPeriod.Text));
            }
        }
        #endregion

        #region Drawing
        /// <summary>
        /// Draws image according to selected parameters
        /// </summary>
        /// <param name="dataBits">Data Bits Count</param>
        /// <param name="ws">Word Select Period Value</param>
        private void ShowImage(int dataBits, int ws)
        {
            Image image = null;
            Bitmap bt;

            float x = 0;
            float y = 0;
            float koef = 0.0f;

            string title = "LSB";
            string bit0 = "bit0";

            switch (ws)
            {
                case 16:
                    pictureBox.Image = Properties.Resources.WS_16;
                    break;
                case 32:
                case 48:
                case 64:
                    image = Properties.Resources.WS;

                    bt = new Bitmap(image.Width, image.Height);
                    Graphics g = Graphics.FromImage(bt);
                    koef = 96 / g.DpiX; // Calculating coefficient to miniaturize font size depending on DPI
                    g.TextRenderingHint = System.Drawing.Text.TextRenderingHint.AntiAlias;
                    FontFamily ff = new FontFamily("Arial");
                    
                    Font sizeFont = new Font(ff, 11 * koef);
                    Font lsbFont = new Font(ff, 13 * koef, FontStyle.Bold);
                    Font bitFont = new Font(ff, 14 * koef);
                    
                    g.DrawImage(image, 0, 0);
                    SolidBrush sizeBrush = new SolidBrush(Color.Black);
                    SolidBrush lsbBrush = new SolidBrush(Color.FromArgb(233,34,34));
                    SolidBrush bitBrush = new SolidBrush(Color.Black);

                    if (dataBits == (ws / 2))
                    {
                        string bitM1 = "bit" + (ws / 2 - 1).ToString();

                        // Number
                        x = 190.0f; y = 5.0f;
                        g.DrawString((ws - 1).ToString(), sizeFont, sizeBrush, x, y);
                        // First Row (SDI)
                        // LSB
                        x = 854.0f; y = 181.0f;
                        g.DrawString(title, lsbFont, lsbBrush, x, y);
                        x = 1541.0f; y = 181.0f;
                        g.DrawString(title, lsbFont, lsbBrush, x, y);
                        // bit0 under LSB
                        x = 854.0f;
                        y = 218.0f;
                        g.DrawString(bit0, bitFont, bitBrush, x, y);
                        x = 1541.0f;
                        y = 218.0f;
                        g.DrawString(bit0, bitFont, bitBrush, x, y);
                        // bit n-1 under MSB
                        x = 223.0f;
                        y = 218.0f;
                        g.DrawString(bitM1, bitFont, bitBrush, x, y);
                        x = 910.0f;
                        y = 218.0f;
                        g.DrawString(bitM1, bitFont, bitBrush, x, y);

                        // Second Row (SDO)
                        // LSB
                        x = 855.0f; y = 285.0f;
                        g.DrawString(title, lsbFont, lsbBrush, x, y);
                        x = 1542.0f; y = 285.0f;
                        g.DrawString(title, lsbFont, lsbBrush, x, y);
                        // bit0 under LSB
                        x = 854.0f;
                        y = 320.0f;
                        g.DrawString(bit0, bitFont, bitBrush, x, y);
                        x = 1541.0f;
                        y = 320.0f;
                        g.DrawString(bit0, bitFont, bitBrush, x, y);
                        // bit n-1 under MSB
                        x = 223.0f;
                        y = 320.0f;
                        g.DrawString(bitM1, bitFont, bitBrush, x, y);
                        x = 910.0f;
                        y = 320.0f;
                        g.DrawString(bitM1, bitFont, bitBrush, x, y);

                    }
                    else if (dataBits == (ws / 2 - 1))
                    {
                        string bitM1 = "bit" + (dataBits - 1).ToString();

                        // Number
                        x = 190.0f; y = 5.0f;
                        g.DrawString((ws - 1).ToString(), sizeFont, sizeBrush, x, y);
                        // First Row (SDI)
                        // LSB
                        x = 792.0f; y = 183.0f;
                        g.DrawString(title, lsbFont, lsbBrush, x, y);
                        x = 1479.0f; y = 183.0f;
                        g.DrawString(title, lsbFont, lsbBrush, x, y);
                        // bit0 under LSB
                        x = 792.0f;
                        y = 218.0f;
                        g.DrawString(bit0, bitFont, bitBrush, x, y);
                        x = 1477.0f;
                        y = 218.0f;
                        g.DrawString(bit0, bitFont, bitBrush, x, y);
                        // bit n-1 under MSB
                        x = 223.0f;
                        y = 218.0f;
                        g.DrawString(bitM1, bitFont, bitBrush, x, y);
                        x = 909.0f;
                        y = 218.0f;
                        g.DrawString(bitM1, bitFont, bitBrush, x, y);

                        // Second Row (SDO)
                        // LSB
                        x = 792.0f; y = 285.0f;
                        g.DrawString(title, lsbFont, lsbBrush, x, y);
                        x = 1479.0f; y = 285.0f;
                        g.DrawString(title, lsbFont, lsbBrush, x, y);
                        // bit0 under LSB
                        x = 792.0f;
                        y = 320.0f;
                        g.DrawString(bit0, bitFont, bitBrush, x, y);
                        x = 1477.0f;
                        y = 320.0f;
                        g.DrawString(bit0, bitFont, bitBrush, x, y);
                        // bit n-1 under MSB
                        x = 222.0f;
                        y = 320.0f;
                        g.DrawString(bitM1, bitFont, bitBrush, x, y);
                        x = 909.0f;
                        y = 320.0f;
                        g.DrawString(bitM1, bitFont, bitBrush, x, y);
                    }
                    else if (dataBits < (ws / 2 - 1))
                    {
                        string bitM1 = "bit" + (dataBits - 1).ToString();

                        // Number
                        x = 190.0f; y = 5.0f;
                        g.DrawString((ws - 1).ToString(), sizeFont, sizeBrush, x, y);
                        // First Row (SDI)
                        // LSB
                        x = 665.0f; y = 180.0f;
                        g.DrawString(title, lsbFont, lsbBrush, x, y);
                        x = 1352.0f; y = 180.0f;
                        g.DrawString(title, lsbFont, lsbBrush, x, y);
                        // bit0 under LSB
                        x = 665.0f;
                        y = 218.0f;
                        g.DrawString(bit0, bitFont, bitBrush, x, y);
                        x = 1352.0f;
                        y = 218.0f;
                        g.DrawString(bit0, bitFont, bitBrush, x, y);
                        // bit n-1 under MSB
                        x = dataBits > 10 ? 223.0f : 227.0f;
                        y = 218.0f;
                        g.DrawString(bitM1, bitFont, bitBrush, x, y);
                        x = dataBits > 10 ? 910.0f : 914.0f;
                        y = 218.0f;
                        g.DrawString(bitM1, bitFont, bitBrush, x, y);
                        // Zeros between channels
                        x = 802.0f;
                        y = 216.0f;
                        g.DrawString("0", bitFont, bitBrush, x, y);
                        x = 865.0f;
                        y = 216.0f;
                        g.DrawString("0", bitFont, bitBrush, x, y);

                        // Second Row (SDO)
                        // LSB
                        x = 665.0f; y = 283.0f;
                        g.DrawString(title, lsbFont, lsbBrush, x, y);
                        x = 1352.0f; y = 283.0f;
                        g.DrawString(title, lsbFont, lsbBrush, x, y);
                        // bit0 under LSB
                        x = 665.0f;
                        y = 320.0f;
                        g.DrawString(bit0, bitFont, bitBrush, x, y);
                        x = 1352.0f;
                        y = 320.0f;
                        g.DrawString(bit0, bitFont, bitBrush, x, y);
                        // bit n-1 under MSB
                        x = dataBits > 10 ? 223.0f : 227.0f;
                        y = 320.0f;
                        g.DrawString(bitM1, bitFont, bitBrush, x, y);
                        x = dataBits > 10 ? 910.0f : 914.0f;
                        y = 320.0f;
                        g.DrawString(bitM1, bitFont, bitBrush, x, y);
                        // Zeros between channels
                        x = 804.0f;
                        y = 320.0f;
                        g.DrawString("0", bitFont, bitBrush, x, y);
                        x = 867.0f;
                        y = 320.0f;
                        g.DrawString("0", bitFont, bitBrush, x, y);
                    }
                    lsbFont.Dispose();
                    lsbBrush.Dispose();
                    bitFont.Dispose();
                    bitBrush.Dispose();
                    g.Dispose();
                    pictureBox.Image = bt;
                    break;
                default:
                    image = null;
                    break;
            }
        }
        #endregion
    }
}
