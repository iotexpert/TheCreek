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

namespace I2S_v1_0
{
    public partial class cyi2sbasictab : UserControl, ICyParamEditingControl
    {
        private Control m_control;
        private CyI2SParameters m_params;

        public cyi2sbasictab(CyI2SParameters inst)
        {
            InitializeComponent();

            ((CyI2SParameters)inst).m_basicParams = this;
            m_control = this;
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
            return new CyCustErr[] { };    //return an empty array
        }

        #endregion

        #region Assigning parameters values to controls

        public void GetParams()
        {
            // Direction
            switch (m_params.m_direction)
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
            comboBoxDataBits.Text = m_params.m_dataBits.ToString();

            // Word Select Period
            comboBoxWordSelectPeriod.Text = m_params.m_wordSelectPeriod.ToString();
        }

        #endregion

        #region Assigning controls values to parameters

        private void SetDirection()
        {
            if (radioButtonRxOnly.Checked)
                m_params.m_direction = E_DIRECTION.Rx;
            else if (radioButtonTxOnly.Checked)
                m_params.m_direction = E_DIRECTION.Tx;
            else if (radioButtonRxAndTx.Checked)
                m_params.m_direction = E_DIRECTION.Rx_and_Tx;
            else return;
            m_params.m_advancedParams.SetEnableDisable(m_params.m_direction);
            m_params.SetParams(CyParamNames.DIRECTION);
        }

        private void SetDataBits()
        {
            m_params.m_dataBits = int.Parse(comboBoxDataBits.Text);
            m_params.SetParams(CyParamNames.DATA_BITS);
        }

        private void SetWordSelectPeriod()
        {
            m_params.m_wordSelectPeriod = int.Parse(comboBoxWordSelectPeriod.Text);
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
            int dataBits = int.Parse(comboBoxDataBits.Text);
            string ws = comboBoxWordSelectPeriod.Text;
            string[] wsArray;

            if (dataBits == 8)
            {
                wsArray = new string[] { "16", "32", "48", "64" };
                comboBoxWordSelectPeriod.Items.Clear();
                comboBoxWordSelectPeriod.Items.AddRange(wsArray);
                SetPreviousWsToCombo(ws, wsArray);
            }
            if (dataBits >= 9 && dataBits <= 16)
            {
                wsArray = new string[] { "32", "48", "64" };
                comboBoxWordSelectPeriod.Items.Clear();
                comboBoxWordSelectPeriod.Items.AddRange(wsArray);
                SetPreviousWsToCombo(ws, wsArray);
            }
            if (dataBits >= 17 && dataBits <= 24)
            {
                wsArray = new string[] { "48", "64" };
                comboBoxWordSelectPeriod.Items.Clear();
                comboBoxWordSelectPeriod.Items.AddRange(wsArray);
                SetPreviousWsToCombo(ws, wsArray);
            }
            if (dataBits >= 25 && dataBits <= 32)
            {
                wsArray = new string[] { "64" };
                comboBoxWordSelectPeriod.Items.Clear();
                comboBoxWordSelectPeriod.Items.Add(wsArray[0]);
                SetPreviousWsToCombo(ws, wsArray);
            }
        }

        private void SetPreviousWsToCombo(string ws, string[] wsArray)
        {
            for (int i = 0; i < wsArray.Length; i++)
            {
                if (ws == wsArray[i])
                {
                    comboBoxWordSelectPeriod.Text = ws;
                    break;
                }
                else
                {
                    comboBoxWordSelectPeriod.SelectedIndex = 0;
                }
            }
        }

        private void comboBoxWordSelectPeriod_SelectedIndexChanged(object sender, EventArgs e)
        {
            SetWordSelectPeriod();
            ShowImage(int.Parse(comboBoxDataBits.Text), int.Parse(comboBoxWordSelectPeriod.Text));
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

            string title = "LSB";
            string bit0 = "bit0";

            bool dpi96 = false;

            switch (ws)
            {
                case 16:
                    pictureBox.Image = global::I2S_v1_0.Properties.Resources.WS_16;
                    break;
                case 32:
                case 48:
                case 64:
                    image = global::I2S_v1_0.Properties.Resources.WS;

                    bt = new Bitmap(image.Width, image.Height);
                    Graphics g = Graphics.FromImage(bt);
                    if (g.DpiX == 96.0f && g.DpiY == 96.0f) dpi96 = true;
                    g.TextRenderingHint = System.Drawing.Text.TextRenderingHint.AntiAlias;
                    FontFamily ff = new FontFamily("Arial");
                    System.Drawing.Font sizeFont = new System.Drawing.Font(ff, 11);
                    System.Drawing.Font lsbFont;
                    if (dpi96)
                        lsbFont = new System.Drawing.Font(ff, 12, FontStyle.Bold);
                    else
                        lsbFont = new System.Drawing.Font(ff, 10, FontStyle.Bold);

                    System.Drawing.Font bitFont = new System.Drawing.Font(ff, 14);
                    g.DrawImage(image, 0, 0);
                    System.Drawing.SolidBrush sizeBrush = new System.Drawing.SolidBrush(System.Drawing.Color.Black);
                    System.Drawing.SolidBrush lsbBrush = new System.Drawing.SolidBrush(
                        System.Drawing.Color.FromArgb(233,34,34));
                    System.Drawing.SolidBrush bitBrush = new System.Drawing.SolidBrush(System.Drawing.Color.Black);

                    // Mistake by Y axis for dpi 96
                    float ymst = 0.0f;
                    if (dpi96) ymst = 2.0f;

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
                        x = dpi96 ? 854.0f : 850.0f;
                        y = 218.0f + ymst;
                        g.DrawString(bit0, bitFont, bitBrush, x, y);
                        x = dpi96 ? 1541.0f : 1537.0f;
                        y = 218.0f + ymst;
                        g.DrawString(bit0, bitFont, bitBrush, x, y);
                        // bit n-1 under MSB
                        x = dpi96 ? 223.0f : 219.0f;
                        y = 218.0f + ymst;
                        g.DrawString(bitM1, bitFont, bitBrush, x, y);
                        x = dpi96 ? 910.0f : 906.0f;
                        y = 218.0f + ymst;
                        g.DrawString(bitM1, bitFont, bitBrush, x, y);

                        // Second Row (SDO)
                        // LSB
                        x = 855.0f; y = 285.0f;
                        g.DrawString(title, lsbFont, lsbBrush, x, y);
                        x = 1542.0f; y = 285.0f;
                        g.DrawString(title, lsbFont, lsbBrush, x, y);
                        // bit0 under LSB
                        x = dpi96 ? 854.0f : 850.0f;
                        y = 320.0f + ymst;
                        g.DrawString(bit0, bitFont, bitBrush, x, y);
                        x = dpi96 ? 1541.0f : 1537.0f;
                        y = 320.0f + ymst;
                        g.DrawString(bit0, bitFont, bitBrush, x, y);
                        // bit n-1 under MSB
                        x = dpi96 ? 223.0f : 219.0f;
                        y = 320.0f + ymst;
                        g.DrawString(bitM1, bitFont, bitBrush, x, y);
                        x = dpi96 ? 910.0f : 906.0f;
                        y = 320.0f + ymst;
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
                        x = dpi96 ? 792.0f : 789.0f;
                        y = 218.0f + ymst;
                        g.DrawString(bit0, bitFont, bitBrush, x, y);
                        x = dpi96 ? 1477.0f : 1471.0f;
                        y = 218.0f + ymst;
                        g.DrawString(bit0, bitFont, bitBrush, x, y);
                        // bit n-1 under MSB
                        x = dpi96 ? 223.0f : 219.0f;
                        y = 218.0f + ymst;
                        g.DrawString(bitM1, bitFont, bitBrush, x, y);
                        x = dpi96 ? 909.0f : 906.0f;
                        y = 218.0f + ymst;
                        g.DrawString(bitM1, bitFont, bitBrush, x, y);

                        // Second Row (SDO)
                        // LSB
                        x = 792.0f; y = 285.0f;
                        g.DrawString(title, lsbFont, lsbBrush, x, y);
                        x = 1479.0f; y = 285.0f;
                        g.DrawString(title, lsbFont, lsbBrush, x, y);
                        // bit0 under LSB
                        x = dpi96 ? 792.0f : 789.0f;
                        y = 320.0f + ymst;
                        g.DrawString(bit0, bitFont, bitBrush, x, y);
                        x = dpi96 ? 1477.0f : 1471.0f;
                        y = 320.0f + ymst;
                        g.DrawString(bit0, bitFont, bitBrush, x, y);
                        // bit n-1 under MSB
                        x = dpi96 ? 222.0f : 219.0f;
                        y = 320.0f + ymst;
                        g.DrawString(bitM1, bitFont, bitBrush, x, y);
                        x = dpi96 ? 909.0f : 906.0f;
                        y = 320.0f + ymst;
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
                        x = dpi96 ? 665.0f : 662.0f;
                        y = 218.0f + ymst;
                        g.DrawString(bit0, bitFont, bitBrush, x, y);
                        x = dpi96 ? 1352.0f : 1349.0f;
                        y = 218.0f + ymst;
                        g.DrawString(bit0, bitFont, bitBrush, x, y);
                        // bit n-1 under MSB
                        if (dataBits > 10) x = dpi96 ? 223.0f : 219.0f;
                        else x = dpi96 ? 227.0f : 224.0f;
                        y = 218.0f + ymst;
                        g.DrawString(bitM1, bitFont, bitBrush, x, y);
                        if (dataBits > 10) x = dpi96 ? 910.0f : 906.0f;
                        else x = dpi96 ? 914.0f : 911.0f;
                        y = 218.0f + ymst;
                        g.DrawString(bitM1, bitFont, bitBrush, x, y);
                        // Zeros between channels
                        x = dpi96 ? 802.0f : 799.0f;
                        y = 216.0f + ymst;
                        g.DrawString("0", bitFont, bitBrush, x, y);
                        x = dpi96 ? 865.0f : 862.0f;
                        y = 216.0f + ymst;
                        g.DrawString("0", bitFont, bitBrush, x, y);

                        // Second Row (SDO)
                        // LSB
                        x = 665.0f; y = 283.0f;
                        g.DrawString(title, lsbFont, lsbBrush, x, y);
                        x = 1352.0f; y = 283.0f;
                        g.DrawString(title, lsbFont, lsbBrush, x, y);
                        // bit0 under LSB
                        x = dpi96 ? 665.0f : 662.0f;
                        y = 320.0f + ymst;
                        g.DrawString(bit0, bitFont, bitBrush, x, y);
                        x = dpi96 ? 1352.0f : 1349.0f;
                        y = 320.0f + ymst;
                        g.DrawString(bit0, bitFont, bitBrush, x, y);
                        // bit n-1 under MSB
                        if (dataBits > 10) x = dpi96 ? 223.0f : 219.0f;
                        else x = dpi96 ? 227.0f : 224.0f;
                        y = 320.0f + ymst;
                        g.DrawString(bitM1, bitFont, bitBrush, x, y);
                        if (dataBits > 10) x = dpi96 ? 910.0f : 906.0f;
                        else x = dpi96 ? 914.0f : 911.0f;
                        y = 320.0f + ymst;
                        g.DrawString(bitM1, bitFont, bitBrush, x, y);
                        // Zeros between channels
                        x = dpi96 ? 804.0f : 801.0f;
                        y = 320.0f + ymst;
                        g.DrawString("0", bitFont, bitBrush, x, y);
                        x = dpi96 ? 867.0f : 864.0f;
                        y = 320.0f + ymst;
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

        #region Control Overrided Methods

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

        #endregion
    }
}
