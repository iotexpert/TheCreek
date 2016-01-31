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
using SPI_Master_v1_20;

namespace SPI_Master_v1_20
{
    public partial class CySPIMControl : UserControl
    {
        public const int PB_SPIMTEXT_WIDTH = 40;
        public const int PB_EXTENTS_BORDER = 5;
        public const int PB_POLYGON_WIDTH = 4;
        public const int NUMWAVEFORMS = 5;
        public const int SPIMNUMBITSMAXIMUM = 16;
        public const int SPIMNUMBITSMINIMUM = 2;
        public const double SPIMFREQUENCYMIMIMUM = 0f;
        public const double SPIMFREQUENCYMAXIMUM = 33000000f;
        public ICyInstEdit_v1 m_Component = null;
        public ICyTerminalQuery_v1 m_TermQuery = null;
        public CySPIMControlAdv m_advanced_control;

        public CySPIMControl(ICyInstEdit_v1 inst, ICyTerminalQuery_v1 termquery, CySPIMControlAdv advanced)
        {
            m_Component = inst;
            m_TermQuery = termquery;
            m_advanced_control = advanced;
            InitializeComponent();

            InitializeFormComponents(inst);
            UpdateFormFromParams(m_Component);
            m_numBitRateHertz.TextChanged += new EventHandler(m_numBitRateHertz_TextChanged);
            m_numDataBits.TextChanged += new EventHandler(m_numDataBits_TextChanged);
        }

        /// <summary>
        /// Need to add detection of when the parent form is closing allowing me to cancel if there are errors in the parameters
        /// Also need to handle CyNumericUpDowns to override the UpButton and DownButtons before the value is changed
        /// </summary>
        protected override void OnCreateControl()
        {
            base.OnCreateControl();
            this.ParentForm.FormClosing += new FormClosingEventHandler(ParentForm_FormClosing);
            ep_Errors.SetIconAlignment(m_numBitRateHertz, ErrorIconAlignment.MiddleLeft);
            this.GotFocus += new EventHandler(CySPIMControl_GotFocus);
        }

        void CySPIMControl_GotFocus(object sender, EventArgs e)
        {
            UpdateFormFromParams(m_Component);
        }

        void ParentForm_FormClosing(object sender, FormClosingEventArgs e)
        {
            if (((Form)sender).DialogResult == DialogResult.Cancel)
            {
                return;
            }
            if (ep_Errors.GetError(m_numBitRateHertz) != "")
            {
                m_numBitRateHertz.Focus();
                e.Cancel = true;
            }
            if(ep_Errors.GetError(m_numDataBits) != "")
            {
                m_numDataBits.Focus();
                e.Cancel = true;
            }
        }

        public void InitializeFormComponents(ICyInstEdit_v1 inst)
        {
            //Set the SPIM Mode Combo Box from Enums
            IEnumerable<string> SPIMModeEnums = m_Component.GetPossibleEnumValues("Mode");
            foreach (string str in SPIMModeEnums)
            {
                m_cbspimMode.Items.Add(str);
            }

            //Set the Compare Type Combo Box from Enums
            IEnumerable<string> ShiftDirEnums = m_Component.GetPossibleEnumValues("ShiftDir");
            foreach (string str in ShiftDirEnums)
            {
                m_cbShiftDir.Items.Add(str);
            }

            m_numDataBits.Minimum = 2;
            m_numDataBits.Maximum = 16;
            m_numBitRateHertz.Minimum = 0;
            m_numBitRateHertz.Maximum = Decimal.MaxValue;
            m_numBitRateHertz.ThousandsSeparator = false;
            m_numBitRateHertz.Increment = (decimal)0.1;
        }

        void UnhookUpdateEvents()
        {
            m_cbBitRateHertz.SelectedIndexChanged -= m_cbBitRateHertz_SelectedIndexChanged;
            m_cbShiftDir.SelectedIndexChanged -= m_cbShiftDir_SelectedIndexChanged;
            m_cbspimMode.SelectedIndexChanged -= m_cbspimMode_SelectedIndexChanged;
            /* Radio buttons are not auto mode so they don't need to be disabled */
        }

        void HookupUpdateEvents()
        {
            m_cbBitRateHertz.SelectedIndexChanged += m_cbBitRateHertz_SelectedIndexChanged;
            m_cbShiftDir.SelectedIndexChanged += m_cbShiftDir_SelectedIndexChanged;
            m_cbspimMode.SelectedIndexChanged += m_cbspimMode_SelectedIndexChanged;
        }

        public void UpdateFormFromParams(ICyInstEdit_v1 inst)
        {
            UnhookUpdateEvents();
            SPIMParameters prms = new SPIMParameters(m_Component);//inst);
            //Set the SPIMMode Combo Box
            IEnumerable<string> SPIMModeEnums = m_Component.GetPossibleEnumValues("Mode");
            bool SPIMModeFound = false;
            foreach (string str in SPIMModeEnums)
            {
                if (!SPIMModeFound)
                {
                    string paramcompare = m_Component.ResolveEnumIdToDisplay("Mode", prms.Mode.Expr);
                    if (paramcompare == str)
                    {
                        m_cbspimMode.SelectedItem = paramcompare;
                        SPIMModeFound = true;
                    }
                }
            }
            if (!SPIMModeFound)
            {
                m_cbspimMode.Text = prms.Mode.Expr;
            }

            m_numDataBits.Value = Convert.ToInt16(prms.NumberOfDataBits.Value);


            //Set the Shift Directions Combo Box
            IEnumerable<string> ShiftDirModeEnums = m_Component.GetPossibleEnumValues("ShiftDir");
            bool ShiftDirFound = false;
            foreach (string str in ShiftDirModeEnums)
            {
                if (!ShiftDirFound)
                {
                    string paramcompare = m_Component.ResolveEnumIdToDisplay("ShiftDir", prms.ShiftDir.Expr);
                    if (paramcompare == str)
                    {
                        m_cbShiftDir.SelectedItem = paramcompare;
                        ShiftDirFound = true;
                    }
                }
            }
            if (!ShiftDirFound)
            {
                m_cbShiftDir.Text = prms.Mode.Expr;
            }

            double bitrate = Convert.ToDouble(prms.DesiredBitRate.Value);
            if (bitrate > 1000000)
            {
                SetBitRateDecimalPlaces(bitrate, 1000000d);
                m_numBitRateHertz.Value = Convert.ToDecimal(bitrate / 1000000d);
                m_cbBitRateHertz.SelectedIndex = 1;
            }
            else
            {
                SetBitRateDecimalPlaces(bitrate, 1000d);
                m_numBitRateHertz.Value = Convert.ToDecimal(bitrate / 1000d);
                m_cbBitRateHertz.SelectedIndex = 0;
            }

            if (prms.ClockInternal.Value == "true")
            {
                m_cbBitRateHertz.Enabled = true;
                m_cbBitRateHertz.Visible = true;
                m_numBitRateHertz.Enabled = true;
                m_numBitRateHertz.Visible = true;
                m_lblCalculatedBitRate.Visible = false;
            }
            else
            {
                m_cbBitRateHertz.Enabled = false;
                m_cbBitRateHertz.Visible = false;
                m_numBitRateHertz.Enabled = false;
                m_numBitRateHertz.Visible = false;
                m_lblCalculatedBitRate.Visible = true;
                
                m_lblCalculatedBitRate.Text = "1/2 Input Clock Frequency";
            }

            UpdateDrawing(prms);
            HookupUpdateEvents();
        }

        private void SetBitRateDecimalPlaces(double bitrate, double denominator)
        {
            decimal bitrateDevided = Convert.ToDecimal(bitrate / denominator);
            string textBitrate = bitrateDevided.ToString();
            if (textBitrate.Contains("."))
            {
                int start = textBitrate.LastIndexOf(".");
                m_numBitRateHertz.DecimalPlaces = textBitrate.Substring(start + 1).Length;
            }
        }


        #region Form Drawing
        public void UpdateDrawing(SPIMParameters prms)
        {
            if ((m_pbDrawing.Width == 0) || (m_pbDrawing.Height == 0))
                return;
            Image waveform = new Bitmap(m_pbDrawing.Width, m_pbDrawing.Height);
            Graphics wfg;
            wfg = Graphics.FromImage(waveform);
            wfg.Clear(Color.White);
            SolidBrush blkbrush = new SolidBrush(Color.Black);


            float extentsleft = PB_EXTENTS_BORDER + PB_SPIMTEXT_WIDTH;
            float extentsright = m_pbDrawing.Width - PB_EXTENTS_BORDER;
            float padding = (extentsright - extentsleft) / 70;
            float startleft = extentsleft + padding;
            float endright = extentsright - padding;
            float startright = startleft + (endright - startleft) / 2;

            //Setup the right, left and center indicators
            Pen extentspen = new Pen(blkbrush);
            extentspen.DashStyle = System.Drawing.Drawing2D.DashStyle.Dash;
            //Draw the Left Extents Line
            wfg.DrawLine(extentspen, extentsleft, PB_EXTENTS_BORDER,
                extentsleft, m_pbDrawing.Height - PB_EXTENTS_BORDER);
            //Draw the Right Extents Line
            wfg.DrawLine(extentspen, extentsright, PB_EXTENTS_BORDER,
                extentsright, m_pbDrawing.Height - PB_EXTENTS_BORDER);

            extentspen.Dispose();


            //setup and draw all of the waveforms
            int numwaveforms = NUMWAVEFORMS;
            string[] wfnames = new string[NUMWAVEFORMS];

            wfnames[0] = "SS";
            wfnames[1] = "SCLK";
            wfnames[2] = "MOSI";
            wfnames[3] = "MISO";
            wfnames[4] = "Sample";
            //wfnames[5] = "Interrupt";

            Font perfont = new Font("Arial", 10, FontStyle.Regular, GraphicsUnit.Pixel);

            //Each waveform's height is dependent upon the drawing size minus a top and bottom border 
            //and the top period waveform which is the size of two polygon widths, and an bottom ticker tape of 2 polygon widths
            float wfheight = (m_pbDrawing.Height - (2 * PB_EXTENTS_BORDER) - (4 * PB_POLYGON_WIDTH)) / numwaveforms;
            //Fill in All Waveform Names
            for (int i = 0; i < numwaveforms; i++)
            {
                PointF pt = new PointF(extentsleft - wfg.MeasureString(wfnames[i], perfont).Width - PB_EXTENTS_BORDER,
                    PB_EXTENTS_BORDER + (2 * PB_POLYGON_WIDTH) + (wfheight * i) + (wfheight / 2) - (wfg.MeasureString(wfnames[i], perfont).Height / 2));
                wfg.DrawString(wfnames[i], perfont, blkbrush, pt);
            }

            //Draw Waveforms

            int numsegments = 2 + (Convert.ToInt16(prms.NumberOfDataBits.Value) * 2) + 3;

            for (int i = 0; i < numwaveforms; i++)
            {
                float HighY = PB_EXTENTS_BORDER + (2 * PB_POLYGON_WIDTH) + (wfheight * i) + (wfheight / 8);
                float LowY = PB_EXTENTS_BORDER + (2 * PB_POLYGON_WIDTH) + (wfheight * (i + 1));
                float segwidth = (extentsright - extentsleft) / numsegments;
                List<float> segsx = new List<float>();
                for (int x = 0; x < numsegments; x++)
                {
                    segsx.Add(extentsleft + (x * segwidth));
                }
                SolidBrush wfbrush = new SolidBrush(Color.Blue);
                Pen wfPen = new Pen(wfbrush);
                int NumDataBits = Convert.ToInt16(prms.NumberOfDataBits.Value);
                string val = null;
                bool ShiftDir = (Convert.ToInt16(prms.ShiftDir.Value) == 0) ? false : true;
                int j = 0;
                bool mode = ((Convert.ToInt16(prms.Mode.Value) == 1) || (Convert.ToInt16(prms.Mode.Value) == 2)) ? true : false;
                bool starthigh = ((Convert.ToInt16(prms.Mode.Value) == 1) || (Convert.ToInt16(prms.Mode.Value) == 3)) ? false : true;
                switch (wfnames[i])
                {
                    case "SS":
                        wfg.DrawLine(wfPen, segsx[0], HighY, segsx[2], HighY);
                        wfg.DrawLine(wfPen, segsx[2], HighY, segsx[2], LowY);
                        wfg.DrawLine(wfPen, segsx[2], LowY, segsx[numsegments - 2], LowY);
                        wfg.DrawLine(wfPen, segsx[numsegments - 2], LowY, segsx[numsegments - 2], HighY);
                        wfg.DrawLine(wfPen, segsx[numsegments - 2], HighY, segsx[numsegments - 1], HighY);
                        break;
                    case "MOSI":
                    case "MISO":
                        if (mode)
                        {
                            wfg.DrawLine(wfPen, segsx[0], HighY, segsx[2] - 2, HighY);   //Draw Bus to First Transition Point
                            wfg.DrawLine(wfPen, segsx[0], LowY, segsx[2] - 2, LowY);
                            wfg.DrawLine(wfPen, segsx[2] - 2, HighY, segsx[2] + 2, LowY); //Draw Transition
                            wfg.DrawLine(wfPen, segsx[2] - 2, LowY, segsx[2] + 2, HighY);
                            for (j = 0; j < (NumDataBits * 2); )
                            {
                                wfg.DrawLine(wfPen, segsx[2 + j] + 2, HighY, segsx[2 + (j + 2)] - 2, HighY);//Draw Bus to Transition Point
                                wfg.DrawLine(wfPen, segsx[2 + j] + 2, LowY, segsx[2 + (j + 2)] - 2, LowY);


                                wfg.DrawLine(wfPen, segsx[2 + (j + 2)] - 2, HighY, segsx[2 + (j + 2)] + 2, LowY); //Draw Transition
                                wfg.DrawLine(wfPen, segsx[2 + (j + 2)] - 2, LowY, segsx[2 + (j + 2)] + 2, HighY);

                                if (ShiftDir)
                                    val = String.Format("D{0}", j / 2);
                                else
                                    val = String.Format("D{0}", NumDataBits - (j / 2) - 1);

                                SizeF strsize = wfg.MeasureString(val, perfont);
                                float centerx = segsx[2 + j] + segwidth;
                                wfg.DrawString(val, perfont, new SolidBrush(Color.Black),
                                                new RectangleF(centerx - (strsize.Width / 2f), HighY + ((wfheight) / 2f) - (strsize.Height / 2f), strsize.Width, strsize.Height));
                                j += 2;
                            }
                            wfg.DrawLine(wfPen, segsx[2 + j] + 2, LowY, segsx[2 + (j + 2)], LowY);//Draw Bus to Transition Point
                            wfg.DrawLine(wfPen, segsx[2 + j] + 2, HighY, segsx[2 + (j + 2)], HighY);
                        }
                        else
                        {
                            wfg.DrawLine(wfPen, segsx[0], HighY, segsx[3] - 2, HighY);   //Draw Bus to First Transition Point
                            wfg.DrawLine(wfPen, segsx[0], LowY, segsx[3] - 2, LowY);
                            wfg.DrawLine(wfPen, segsx[3] - 2, HighY, segsx[3] + 2, LowY); //Draw Transition
                            wfg.DrawLine(wfPen, segsx[3] - 2, LowY, segsx[3] + 2, HighY);
                            for (j = 0; j < (NumDataBits * 2); )
                            {
                                wfg.DrawLine(wfPen, segsx[3 + j] + 2, HighY, segsx[3 + (j + 2)] - 2, HighY);//Draw Bus to Transition Point
                                wfg.DrawLine(wfPen, segsx[3 + j] + 2, LowY, segsx[3 + (j + 2)] - 2, LowY);


                                wfg.DrawLine(wfPen, segsx[3 + (j + 2)] - 2, HighY, segsx[3 + (j + 2)] + 2, LowY); //Draw Transition
                                wfg.DrawLine(wfPen, segsx[3 + (j + 2)] - 2, LowY, segsx[3 + (j + 2)] + 2, HighY);

                                if (ShiftDir)
                                    val = String.Format("D{0}", j / 2);
                                else
                                    val = String.Format("D{0}", NumDataBits - (j / 2) - 1);

                                SizeF strsize = wfg.MeasureString(val, perfont);
                                float centerx = segsx[3 + j] + segwidth;
                                wfg.DrawString(val, perfont, new SolidBrush(Color.Black),
                                                new RectangleF(centerx - (strsize.Width / 2f), HighY + ((wfheight) / 2f) - (strsize.Height / 2f), strsize.Width, strsize.Height));
                                j += 2;
                            }
                            wfg.DrawLine(wfPen, segsx[3 + j] + 2, LowY, segsx[3 + (j + 1)], LowY);//Draw Bus to Transition Point
                            wfg.DrawLine(wfPen, segsx[3 + j] + 2, HighY, segsx[3 + (j + 1)], HighY);
                        }
                        break;
                    case "SCLK":

                        wfg.DrawLine(wfPen, segsx[0], starthigh ? HighY : LowY, segsx[3], starthigh ? HighY : LowY);
                        wfg.DrawLine(wfPen, segsx[3], starthigh ? HighY : LowY, segsx[3], starthigh ? HighY : LowY);
                        for (j = 0; j < (NumDataBits * 2); )
                        {
                            wfg.DrawLine(wfPen, segsx[3 + j], starthigh ? HighY : LowY, segsx[3 + j], starthigh ? LowY : HighY);
                            wfg.DrawLine(wfPen, segsx[3 + j++], starthigh ? LowY : HighY, segsx[3 + j], starthigh ? LowY : HighY);
                            wfg.DrawLine(wfPen, segsx[3 + j], starthigh ? LowY : HighY, segsx[3 + j], starthigh ? HighY : LowY);
                            wfg.DrawLine(wfPen, segsx[3 + j++], starthigh ? HighY : LowY, segsx[3 + j], starthigh ? HighY : LowY);
                        }
                        wfg.DrawLine(wfPen, segsx[3 + j++], starthigh ? HighY : LowY, segsx[3 + j], starthigh ? HighY : LowY);
                        break;
                    case "Sample":
                        if (mode)
                        {
                            wfg.DrawLine(wfPen, segsx[0], LowY, segsx[3] - 2, LowY);    //Go to first edge 
                            for (j = 0; j < (NumDataBits * 2); )
                            {
                                wfg.DrawLine(wfPen, segsx[3 + j] - 2, LowY, segsx[3 + j] - 2, HighY);
                                wfg.DrawLine(wfPen, segsx[3 + j] - 2, HighY, segsx[3 + j] + 2, HighY);
                                wfg.DrawLine(wfPen, segsx[3 + j] + 2, HighY, segsx[3 + j] + 2, LowY);
                                wfg.DrawLine(wfPen, segsx[3 + j] + 2, LowY, segsx[3 + (j + 2)] - 2, LowY);
                                j += 2;
                            }
                            wfg.DrawLine(wfPen, segsx[3 + j] - 2, LowY, segsx[3 + (j + 1)], LowY);
                        }
                        else
                        {
                            wfg.DrawLine(wfPen, segsx[0], LowY, segsx[4] - 2, LowY);    //Go to first edge 
                            for (j = 0; j < (NumDataBits * 2); )
                            {
                                wfg.DrawLine(wfPen, segsx[4 + j] - 2, LowY, segsx[4 + j] - 2, HighY);
                                wfg.DrawLine(wfPen, segsx[4 + j] - 2, HighY, segsx[4 + j] + 2, HighY);
                                wfg.DrawLine(wfPen, segsx[4 + j] + 2, HighY, segsx[4 + j] + 2, LowY);
                                wfg.DrawLine(wfPen, segsx[4 + j] + 2, LowY, segsx[4 + (j + 2)] - 2, LowY);
                                j += 2;
                            }
                            wfg.DrawLine(wfPen, segsx[4 + j] - 2, LowY, segsx[4 + j], LowY);
                        }
                        break;
                    case "Interrupt":
                        break;
                }
            }
            wfg.Dispose();
            m_pbDrawing.Image = waveform;
        } 
        #endregion

        #region Data Bits Numeric Up/Down

        private void m_numDataBits_TextChanged(object sender, EventArgs e)
        {

            if (m_numDataBits.Text != "")
            {
                CancelEventArgs ce = new CancelEventArgs();
                numericUpDown_Validating(sender, ce);
                if (!ce.Cancel)
                {
                    SetAParameter("NumberOfDataBits", m_numDataBits.Value.ToString(), true);
                    UpdateDrawing(new SPIMParameters(m_Component));
                }
            }
            else
            {
                m_numDataBits.Text = "0";
            }
        }

        #endregion

        private void CySPIMControl_SizeChanged(object sender, EventArgs e)
        {
            UpdateDrawing(new SPIMParameters(m_Component));
        }

        private void m_cbspimMode_SelectedIndexChanged(object sender, EventArgs e)
        {
            string prm = m_Component.ResolveEnumDisplayToId("Mode", m_cbspimMode.Text);
            SetAParameter("Mode", prm, true);
            UpdateDrawing(new SPIMParameters(m_Component));
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
                    MessageBox.Show(string.Format("Error Setting Parameter {0} with value {1}\n\nErrors:\n{2}", 
                        parameter, value, errors),
                        "Error Setting Parameter", MessageBoxButtons.OK, MessageBoxIcon.Error);
                }
            }
        }

        private void m_cbShiftDir_SelectedIndexChanged(object sender, EventArgs e)
        {
            string prm = m_Component.ResolveEnumDisplayToId("ShiftDir", m_cbShiftDir.Text);
            SetAParameter("ShiftDir", prm, true);
            UpdateDrawing(new SPIMParameters(m_Component));
        }

        #region Bit Rate Numeric Up/Down

        private void SetFrequencyParameter(object sender)
        {
            if (m_numBitRateHertz.Text != "")
            {
                CancelEventArgs ce = new CancelEventArgs();
                numericUpDown_Validating(sender, ce);
                if (!ce.Cancel)
                {
                    bool error;
                    double val = GetNumericUpDownText(m_numBitRateHertz, out error);
                    double Freq = val * ((m_cbBitRateHertz.SelectedIndex == 0) ? 1000d : 1000000d);
                    SetAParameter("DesiredBitRate", Freq.ToString(), true);

                }
            }
            else
            {
                m_numBitRateHertz.Text = "0";
            }
        }

        private void m_numBitRateHertz_TextChanged(object sender, EventArgs e)
        {
            SetFrequencyParameter(sender);
            int start = m_numBitRateHertz.Text.LastIndexOf(".");
            if (start > 0 && m_numBitRateHertz.Text.Length > start + 1)
                m_numBitRateHertz.DecimalPlaces = m_numBitRateHertz.Text.Substring(start + 1).Length;
            else
                m_numBitRateHertz.DecimalPlaces = 3;
        }

        private void m_cbBitRateHertz_SelectedIndexChanged(object sender, EventArgs e)
        {
            SetFrequencyParameter(sender);
        }

        private void m_numBitRateHertz_Leave(object sender, EventArgs e)
        {
            char[] charsToTrim = { '0' };
            m_numBitRateHertz.Text = m_numBitRateHertz.Text.TrimEnd(charsToTrim);
        }

        #endregion

        private double GetNumericUpDownText(object numericUpDown,out bool error)
        {
            error = false;
            double val = 0;
            try
            {
                val = Convert.ToDouble(((NumericUpDown)numericUpDown).Text);
            }
            catch
            { error = true; }
            return val;
        }

        private void numericUpDown_Validating(object sender, CancelEventArgs e)
        {
            if (sender == m_cbBitRateHertz)
                sender = m_numBitRateHertz;
            bool error;
            double val = GetNumericUpDownText(sender,out error);
            double minimum = 0.0;
            double maximum = 0.0;
            string message = "";

            if (sender == m_numBitRateHertz)
            {
                val *= ((m_cbBitRateHertz.SelectedIndex == 0) ? 1000d : 1000000d);
                message = "Frequency Must Be Positive and Cannot Exceed 33MHz";
                minimum = SPIMFREQUENCYMIMIMUM;
                maximum = SPIMFREQUENCYMAXIMUM;
            }
            else if (sender == m_numDataBits)
            {
                message = string.Format("Number Of Data Bits must be between {0} and {1}", SPIMNUMBITSMINIMUM, 
                    SPIMNUMBITSMAXIMUM);
                minimum = SPIMNUMBITSMINIMUM;
                maximum = SPIMNUMBITSMAXIMUM;
            }

            if ((error)||((val < minimum) || (val > maximum)))
            {
                ep_Errors.SetError((NumericUpDown)sender, string.Format(message));
                e.Cancel = true;
            }
            else
            {
                ep_Errors.SetError((NumericUpDown)sender, "");
            }
        }

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
