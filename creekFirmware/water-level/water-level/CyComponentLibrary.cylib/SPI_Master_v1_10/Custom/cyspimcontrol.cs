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
using CyCustomizer.SPI_Master_v1_10;

namespace CyCustomizer.SPI_Master_v1_10
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
            //Trace.Assert(Debugger.IsAttached);
            InitializeComponent();

            InitializeFormComponents(inst);
            UpdateFormFromParams(m_Component);           
        }

        /// <summary>
        /// Need to add detection of when the parent form is closing allowing me to cancel if there are errors in the parameters
        /// Also need to handle CyNumericUpDowns to override the UpButton and DownButtons before the value is changed
        /// </summary>
        protected override void OnCreateControl()
        {
            base.OnCreateControl();
            this.ParentForm.FormClosing += new FormClosingEventHandler(ParentForm_FormClosing);
            m_numBitRateHertz.UpEvent += new UpButtonEvent(m_numBitRateHertz_UpEvent);
            m_numBitRateHertz.DownEvent += new DownButtonEvent(m_numBitRateHertz_DownEvent);
            m_numDataBits.UpEvent += new UpButtonEvent(m_numDataBits_UpEvent);
            m_numDataBits.DownEvent += new DownButtonEvent(m_numDataBits_DownEvent);
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

            m_numDataBits.Minimum = Decimal.MinValue;
            m_numDataBits.Maximum = Decimal.MaxValue;
            m_numBitRateHertz.Minimum = Decimal.MinValue;
            m_numBitRateHertz.Maximum = Decimal.MaxValue;
            m_numBitRateHertz.ThousandsSeparator = false;
            m_numBitRateHertz.Increment = 1;
        }

        void UnhookUpdateEvents()
        {
            m_cbBitRateHertz.SelectedIndexChanged -= m_cbBitRateHertz_SelectedIndexChanged;
            m_cbShiftDir.SelectedIndexChanged -= m_cbShiftDir_SelectedIndexChanged;
            m_cbspimMode.SelectedIndexChanged -= m_cbspimMode_SelectedIndexChanged;
            m_numBitRateHertz.ValueChanged -= m_numBitRateHertz_ValueChanged;
            m_numDataBits.ValueChanged -= m_numDataBits_ValueChanged;
            /* Radio buttons are not auto mode so they don't need to be disabled */
        }

        void HookupUpdateEvents()
        {
            m_cbBitRateHertz.SelectedIndexChanged += m_cbBitRateHertz_SelectedIndexChanged;
            m_cbShiftDir.SelectedIndexChanged += m_cbShiftDir_SelectedIndexChanged;
            m_cbspimMode.SelectedIndexChanged += m_cbspimMode_SelectedIndexChanged;
            m_numBitRateHertz.ValueChanged += m_numBitRateHertz_ValueChanged;
            m_numDataBits.ValueChanged += m_numDataBits_ValueChanged;
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
                m_numBitRateHertz.Value = Convert.ToDecimal(bitrate / 1000000d);
                m_cbBitRateHertz.SelectedIndex = 1;
            }
            else
            {
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
                //Trace.Assert(Debugger.IsAttached);
                m_cbBitRateHertz.Enabled = false;
                m_cbBitRateHertz.Visible = false;
                m_numBitRateHertz.Enabled = false;
                m_numBitRateHertz.Visible = false;
                m_lblCalculatedBitRate.Visible = true;


                /* 
                 * The following code was intended to display the calculated bit rate from an
                 * external clock if it could be found.  Turns out the Clock Query stuff doesn't 
                 * allow this so I've commented the code out waiting for this to be implemented 
                 * in the interface
                 */
                //List<CyClockData> clkdata = new List<CyClockData>();
                //string temp = m_Component.InstanceName;
                //clkdata = m_TermQuery.GetClockData(m_Component.InstanceIdPath,"clock", 0);
                //if (clkdata.Count != 0)
                //{
                //    if (clkdata[0].IsFrequencyKnown)
                //    {
                //        double infreq = clkdata[0].Frequency / 2d;
                //        string units = "kHz";
                //        switch (clkdata[0].Unit)
                //        {
                //            case CyClockUnit.kHz: units = "kHz"; break;
                //            case CyClockUnit.MHz: units = "MHz"; break;
                //        }
                //        m_lblCalculatedBitRate.Text = string.Format("1/2 Input Clock Frequeny = {0}{1}", Math.Round(infreq, 3), units);
                //    }
                //    else
                //    {
                //        m_lblCalculatedBitRate.Text = "1/2 Input Clock Frequency = {Unknown Source}";
                //    }
                //}
                //else
                //{
                    m_lblCalculatedBitRate.Text = "1/2 Input Clock Frequency";
                //}
            }

            UpdateDrawing(prms);
            HookupUpdateEvents();
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
                    //case "MISO":
                    //    break;
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
        private void m_numDataBits_ValueChanged(object sender, EventArgs e)
        {
            CancelEventArgs ce = new CancelEventArgs();
            m_numDataBits_Validating(sender, ce);
            if (!ce.Cancel)
            {
                SetAParameter("NumberOfDataBits", m_numDataBits.Value.ToString(), true);
                UpdateDrawing(new SPIMParameters(m_Component));
            }
        }
        private void m_numDataBits_Validating(object sender, CancelEventArgs e)
        {
            if ( (m_numDataBits.Value < SPIMNUMBITSMINIMUM) || (m_numDataBits.Value > SPIMNUMBITSMAXIMUM) )
            {
                ep_Errors.SetError(m_numDataBits, string.Format("Number Of Data Bits must be between {0} and {1}", SPIMNUMBITSMINIMUM, SPIMNUMBITSMAXIMUM));
                e.Cancel = true;
            }
            else
            {
                ep_Errors.SetError(m_numDataBits, "");
            }
        }

        private void m_numDataBits_KeyUp(object sender, KeyEventArgs e)
        {
            m_numDataBits_Validating(sender, new CancelEventArgs());
        }

        void m_numDataBits_DownEvent(object sender, DownButtonEventArgs e)
        {
            if (m_numDataBits.Value == SPIMNUMBITSMINIMUM)
                e.Cancel = true;
        }

        void m_numDataBits_UpEvent(object sender, UpButtonEventArgs e)
        {
            if (m_numDataBits.Value == SPIMNUMBITSMAXIMUM)
                e.Cancel = true;
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
                    MessageBox.Show(string.Format("Error Setting Parameter {0} with value {1}\n\nErrors:\n{2}", parameter, value, errors),
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
        private void m_numBitRateHertz_ValueChanged(object sender, EventArgs e)
        {
            SetFrequencyParameter(sender);
        }

        private void SetFrequencyParameter(object sender)
        {
            CancelEventArgs ce = new CancelEventArgs();
            m_numBitRateHertz_Validating(sender, ce);
            if (!ce.Cancel)
            {
                double Freq = Convert.ToDouble(m_numBitRateHertz.Value) * ((m_cbBitRateHertz.SelectedIndex == 0) ? 1000d : 1000000d);
                SetAParameter("DesiredBitRate", Freq.ToString(), true);
            }
        }
        private void m_numBitRateHertz_Validating(object sender, CancelEventArgs e)
        {
            double Freq = Convert.ToDouble(m_numBitRateHertz.Value) * ((m_cbBitRateHertz.SelectedIndex == 0) ? 1000d : 1000000d);
            if ((Freq < SPIMFREQUENCYMIMIMUM) || (Freq > SPIMFREQUENCYMAXIMUM))
            {
                ep_Errors.SetError(m_numBitRateHertz, string.Format("Frequency Must Be Positive and Cannot Exceed 33MHz"));
                e.Cancel = true;
            }
            else
            {
                ep_Errors.SetError(m_numBitRateHertz, "");
            }
        }
        private void m_numBitRateHertz_KeyUp(object sender, KeyEventArgs e)
        {
            if (e.KeyData != Keys.Decimal)
            {
                String temp = m_numBitRateHertz.Text;//Convert.ToString(m_numBitRateHertz.Value);
                if (temp.Contains("."))
                {
                    m_numBitRateHertz.DecimalPlaces = temp.Length - temp.IndexOf('.') - 1;
                    //m_numBitRateHertz.Increment = 1 / (10 ^ m_numBitRateHertz.DecimalPlaces);
                }
                else
                {
                    m_numBitRateHertz.DecimalPlaces = 0;
                    //m_numBitRateHertz.Increment = 1;
                }
                m_numBitRateHertz_Validating(sender, new CancelEventArgs());
                m_numBitRateHertz.Select(temp.Length, 0);
            }
            
        }

        void m_numBitRateHertz_DownEvent(object sender, DownButtonEventArgs e)
        {
            if (m_numBitRateHertz.Value == 0)
                e.Cancel = true;
        }

        void m_numBitRateHertz_UpEvent(object sender, UpButtonEventArgs e)
        {
            //Nothing to do except validate
        } 
        #endregion


        private void m_cbBitRateHertz_SelectedIndexChanged(object sender, EventArgs e)
        {
            //SPIMParameters prms = new SPIMParameters(m_Component);
            //if (m_cbBitRateHertz.SelectedIndex == 0) /* KHZ */
            //    m_numBitRateHertz.Maximum = 33000;
            //else
            //    m_numBitRateHertz.Maximum = 33;
            //double calcfreq = Convert.ToDouble(m_numBitRateHertz.Value) * ((m_cbBitRateHertz.SelectedIndex == 0) ? 1000d : 1000000d);
            //if (calcfreq != Convert.ToDouble(prms.DesiredBitRate.Value))
            //{

            //    double bitrate = Convert.ToDouble(prms.DesiredBitRate.Value);
            //    if(m_cbBitRateHertz.SelectedIndex == 0)
            //        m_numBitRateHertz.Value = Convert.ToDecimal(bitrate / 1000d);
            //    else
            //        m_numBitRateHertz.Value = Convert.ToDecimal(bitrate / 1000000d);
            //}


            //double Freq = Convert.ToDouble(m_numBitRateHertz.Value) * ((m_cbBitRateHertz.SelectedIndex == 0) ? 1000d : 1000000d);
            //SetAParameter("DesiredBitRate", Freq.ToString(), true);
            SetFrequencyParameter(sender);
        }
       
    }

    /// <summary>
    /// Declaration of the UpButtonEvent must be outside of the class which is CyNumericUpDown
    /// </summary>
    /// <param name="sender"></param>
    /// <param name="e"></param>
    public delegate void UpButtonEvent(object sender, UpButtonEventArgs e);

    /// <summary>
    /// Declaration of the DownButtonEvent must be outside of the class which is CyNumericUpDown
    /// </summary>
    /// <param name="sender"></param>
    /// <param name="e"></param>
    public delegate void DownButtonEvent(object sender, DownButtonEventArgs e);

    /// <summary>
    /// Custom Event Args for the UpButtonEvent Delegate needs to allow the event to be cancelled before the value is changed
    /// </summary>
    public class UpButtonEventArgs : EventArgs
    {
        public bool Cancel;

        public UpButtonEventArgs()
        {
            Cancel = false;
        }
    }
    /// <summary>
    /// Custom Event Args for the DownButtonEvent Delegate needs to allow the event to be cancelled before the value is changed
    /// </summary>
    public class DownButtonEventArgs : EventArgs
    {
        public bool Cancel;

        public DownButtonEventArgs()
        {
            Cancel = false;
        }
    }

    /// <summary>
    /// Ovverride the base numeric up down so that enter key strokes aren't passed to the parent form.
    /// </summary>
    public class CyNumericUpDown : NumericUpDown
    {
        public event UpButtonEvent UpEvent;
        public event DownButtonEvent DownEvent;

        protected virtual void OnUpEvent(UpButtonEventArgs e)
        {
            UpEvent(this, e);
        }

        protected virtual void OnDownEvent(DownButtonEventArgs e)
        {
            DownEvent(this, e);
        }

        protected override bool ProcessCmdKey(ref Message msg, Keys keyData)
        {
            if (keyData == Keys.Enter)
            {
                this.ValidateEditText();
                SendKeys.Send("{TAB}");
                return true;
            }
            else
                return base.ProcessCmdKey(ref msg, keyData);
        }

        public override void UpButton()
        {

            UpButtonEventArgs e = new UpButtonEventArgs();
            OnUpEvent(e);
            if (!e.Cancel)
                base.UpButton();
        }

        public override void DownButton()
        {
            DownButtonEventArgs e = new DownButtonEventArgs();
            OnDownEvent(e);
            if (!e.Cancel)
                base.DownButton();
        }

    }
}
