/*******************************************************************************
* Copyright 2008-2012, Cypress Semiconductor Corporation.  All rights reserved.
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

namespace GraphicLCDIntf_v1_70
{
    public partial class CyGraphicLCDIntfBasicTab : UserControl, ICyParamEditingControl
    {
        private CyGraphicLCDIntfParameters m_params;

        #region Constructor(s)
        public CyGraphicLCDIntfBasicTab(CyGraphicLCDIntfParameters inst)
        {
            InitializeComponent();

            ((CyGraphicLCDIntfParameters)inst).m_basicTab = this;
            m_params = inst;
            this.Dock = DockStyle.Fill;
            numReadLowPulseWidthTime.TextChanged += new EventHandler(numReadLowPulseWidthTime_TextChanged);
            numReadHighPulseWidthTime.TextChanged += new EventHandler(numReadHighPulseWidthTime_TextChanged);
        }
        #endregion

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
                if (param.TabName.Equals(Properties.Resources.ResourceManager.GetString("BasicTabName")))
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
            // BitWidth
            switch (m_params.BitWidth)
            {
                case 8:
                    rb8Bit.Checked = true;
                    break;
                case 16:
                    rb16Bit.Checked = true;
                    break;
                default:
                    break;
            }

            // ReadHiPulse
            numReadHighPulseWidthTime.Value = (decimal)m_params.ReadHiPulse;

            // ReadLoPulse
            numReadLowPulseWidthTime.Value = (decimal)m_params.ReadLoPulse;
        }
        #endregion

        #region Assigning controls values to parameters
        private void SetBitWidth()
        {
            m_params.BitWidth = rb8Bit.Checked ? 8 : 16;
            m_params.SetParams(CyParamNames.BIT_WIDTH);
            ShowImage();
        }

        private void SetReadHiPulse()
        {
            int tmpReadHiPulse = 0;
            if (int.TryParse(numReadHighPulseWidthTime.Text, out tmpReadHiPulse))
            {
                m_params.ReadHiPulse = tmpReadHiPulse;
                m_params.SetParams(CyParamNames.READ_HI_PULSE);
            }
        }

        private void SetReadLoPulse()
        {
            int tmpReadLoPulse = 0;
            if (int.TryParse(numReadLowPulseWidthTime.Text, out tmpReadLoPulse))
            {
                m_params.ReadLoPulse = tmpReadLoPulse;
                m_params.SetParams(CyParamNames.READ_LO_PULSE);
            }
        }
        #endregion

        #region Event Handlers
        private void rb8Bit_CheckedChanged(object sender, EventArgs e)
        {
            SetBitWidth();
        }

        private void numReadHighPulseWidthTime_TextChanged(object sender, EventArgs e)
        {
            if (NumUpDownValidated(sender))
            {
                SetReadHiPulse();
                ShowImage();
            }
        }

        private void numReadLowPulseWidthTime_TextChanged(object sender, EventArgs e)
        {
            if (NumUpDownValidated(sender))
            {
                SetReadLoPulse();
                ShowImage();
            }
        }
        #endregion

        #region Input Validators
        private bool NumUpDownValidated(object sender)
        {
            int value = 0;
            int min = 0;
            int max = 0;
            string message = "";
            bool result = false;
            if (sender == numReadHighPulseWidthTime)
            {
                min = CyParamRanges.READ_HI_PULSE_MIN;
                max = CyParamRanges.READ_HI_PULSE_MAX;
                message = string.Format(Properties.Resources.ResourceManager.GetString("ReadHighPulseEP"),
                    min.ToString(), max.ToString());
            }
            if (sender == numReadLowPulseWidthTime)
            {
                min = CyParamRanges.READ_LO_PULSE_MIN;
                max = CyParamRanges.READ_LO_PULSE_MAX;
                message = string.Format(Properties.Resources.ResourceManager.GetString("ReadLowPulseEP"),
                    min.ToString(), max.ToString());
            }
            NumericUpDown currentNumUpDown = (NumericUpDown)sender;
            if (int.TryParse(currentNumUpDown.Text, out value))
            {
                if (value < min || value > max)
                { errorProvider.SetError((NumericUpDown)sender, string.Format(message)); }
                else
                {
                    errorProvider.SetError((NumericUpDown)sender, "");
                    result = true;
                }
            }
            else
            { errorProvider.SetError((NumericUpDown)sender, string.Format(message)); }
            return result;
        }
        #endregion

        #region Drawing
        // Draws image according to selected parameters
        public void ShowImage()
        {
            float koef = 0.0f;
            float x = 0.0f;
            float y = 0.0f;

            Image image = Properties.Resources.timingdiagram;
            Bitmap bt = new Bitmap(image.Width, image.Height);
            Graphics g = Graphics.FromImage(bt);
            koef = 96 / g.DpiX; // Calculating coefficient to miniaturize font size depending on DPI

            g.TextRenderingHint = System.Drawing.Text.TextRenderingHint.AntiAlias;
            Font drawFont = new Font(new FontFamily("Arial"), 11 * koef, FontStyle.Bold);
            SolidBrush timeBrush = new SolidBrush(Color.Black);
            SolidBrush bitWidthBrush = new SolidBrush(Color.Red);

            string str1 = "";
            string str2 = "";
            string str3 = "";
            string str4 = "";

            if (m_params.m_clock != -1) // -1: clock frequency is not valid
            {
                double t = 1 / m_params.m_clock * 1000; // Input clock period in ns
                double tt = t * 2;                      // ncs width in ns
                double rhpt = t * m_params.ReadHiPulse; // Read High Pulse Width Time in ns
                double rlpt = t * m_params.ReadLoPulse; // Read Low Pulse Width Time in ns

                string outFormat = "{0:0.#}";
                str1 = String.Format(outFormat, t) + " ns";
                str2 = String.Format(outFormat, tt) + " ns";
                str3 = String.Format(outFormat, rlpt) + " ns";
                str4 = String.Format(outFormat, rhpt) + " ns";
            }
            else
            {
                string unknownLabel = "? ns";
                str1 = unknownLabel;
                str2 = unknownLabel;
                str3 = unknownLabel;
                str4 = unknownLabel;
            }

            g.DrawImage(image, 0, 0);

            x = 295.0f; y = 70.0f;
            g.DrawString(str1, drawFont, timeBrush, x, y);
            x = 340.0f; y = 45.0f;
            g.DrawString(str2, drawFont, timeBrush, x, y);
            x = 720.0f; y = 70.0f;
            g.DrawString(str3, drawFont, timeBrush, x, y);
            x = 915.0f; y = 70.0f;
            g.DrawString(str4, drawFont, timeBrush, x, y);

            // Drawing bitwidth
            x = 5.0f; y = 302.0f;
            string imageBitWidth = "[" + (m_params.BitWidth - 1).ToString() + "..0]";
            g.DrawString(imageBitWidth, drawFont, bitWidthBrush, x, y);
            x = 5.0f; y = 375.0f;
            g.DrawString(imageBitWidth, drawFont, bitWidthBrush, x, y);

            drawFont.Dispose();
            timeBrush.Dispose();
            g.Dispose();
            pictureBox.Image = bt;
            pictureBox.SizeMode = PictureBoxSizeMode.Zoom;
        }
        #endregion
    }
}
