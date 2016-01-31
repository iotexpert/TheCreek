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
using PWM_v1_0;

namespace PWM_v1_0
{
    public partial class CyPWMControl : UserControl
    {
        public const int PB_PWMTEXT_WIDTH = 40;
        public const int PB_EXTENTS_BORDER = 5;
        public const int PB_POLYGON_WIDTH = 4;
        public ICyInstEdit_v1 m_Component = null;
        public ICyTerminalQuery_v1 m_TermQuery = null;

        public CyPWMControl(ICyInstEdit_v1 inst, ICyTerminalQuery_v1 termquery)
        {
            m_Component = inst;
            m_TermQuery = termquery;
            //Trace.Assert(Debugger.IsAttached);
            InitializeComponent();

            InitializeFormComponents(inst);
            UpdateFormFromParams(inst);
        }

        public void InitializeFormComponents(ICyInstEdit_v1 inst)
        {
            //Set the PWM Mode Combo Box from Enums
            IEnumerable<string> PWMModeEnums = inst.GetPossibleEnumValues("PWMMode");
            foreach (string str in PWMModeEnums)
            {
                m_cbPWMMode.Items.Add(str);
            }

            //Set the Compare Type Combo Box from Enums
            IEnumerable<string> CmpType1Enums = inst.GetPossibleEnumValues("CompareType1");
            foreach (string str in CmpType1Enums)
            {
                m_cbCompareType1.Items.Add(str);
            }

            //Set the Compare Type Combo Box from Enums
            IEnumerable<string> CmpType2Enums = inst.GetPossibleEnumValues("CompareType2");
            foreach (string str in CmpType2Enums)
            {
                m_cbCompareType2.Items.Add(str);
            }

            //Set the Dead Band Mode Combo Box from Enums
            IEnumerable<string> DBModeEnums = inst.GetPossibleEnumValues("DeadBand");
            foreach (string str in DBModeEnums)
            {
                m_cbDeadBandMode.Items.Add(str);
            }

            //TODO: Dither should use Enumerated Types?
            m_cbDitherAlign.Items.Add("Left Aligned");
            m_cbDitherAlign.Items.Add("Right Aligned");
            //Set the Dither Mode Alignment Combo Box from Enums
            //IEnumerable<string> DitherModeEnums = inst.GetPossibleEnumValues("DeadBand");
            //foreach (string str in DitherModeEnums)
            //{
            //    m_cbDitherAlign.Items.Add(str);
            //}

            //Set the Dither Offsets Combo Box from Enums
            IEnumerable<string> DitherOffsetsEnums = inst.GetPossibleEnumValues("DitherOffset");
            foreach (string str in DitherOffsetsEnums)
            {
                m_cbDitherOffset.Items.Add(str);
            }

            m_cbFFDeadBandMode.Items.Add("Disabled");
            m_cbFFDeadBandMode.Items.Add("0-3 Counts");
            m_cbFFDeadBandMode.SelectedIndex = 0;
            m_cbFFDeadBandMode.Visible = false;
        }

        public void MyUpdate()
        {
            UpdateFormFromParams(m_Component);
        }

        public void UpdateFormFromParams(ICyInstEdit_v1 inst)
        {
            PWMParameters prms = new PWMParameters(inst);
            //Set the PWMMode Combo Box
            IEnumerable<string> PWMModeEnums = inst.GetPossibleEnumValues("PWMMode");
            bool PWMModeFound = false;
            foreach (string str in PWMModeEnums)
            {
                if (!PWMModeFound)
                {
                    string paramcompare = m_Component.ResolveEnumIdToDisplay("PWMMode", prms.PWMMode.Expr);
                    if (paramcompare == str)
                    {
                        m_cbPWMMode.SelectedItem = paramcompare;
                        PWMModeFound = true;
                    }
                }
            }
            if (!PWMModeFound)
            {
                m_cbPWMMode.Text = prms.PWMMode.Expr;
            }

            if (prms.FixedFunction.Value == "true")
                m_cbPWMMode.Enabled = true;
            else
                m_cbPWMMode.Enabled = false;


            //Set the Resolution Radio Buttons
            switch (prms.Resolution.Value)
            {
                case "8": m_rbResolution8.Checked = true; m_numPeriod.Maximum = 255; break;
                case "16": m_rbResolution16.Checked = true; m_numPeriod.Maximum = 65535; break;
            }

            //Set the Period Numeric Up/Down
            m_numPeriod.Value = Convert.ToInt32(prms.Period.Value);

            //Set the Capture value Numeric Up/Downs
            m_numCompare1.Value = Convert.ToInt32(prms.CompareValue1.Value);
            m_numCompare2.Value = Convert.ToInt32(prms.CompareValue2.Value);

            //Set the Compare Type  and Dither Combo Boxes
            if (prms.CompareType1.Expr.Contains("Greater"))
                m_cbDitherAlign.SelectedIndex = 0;
            else
                m_cbDitherAlign.SelectedIndex = 1;

            //Set the Dither Offset Combo Box
            IEnumerable<string> DitherOffsetEnums = inst.GetPossibleEnumValues("DitherOffset");
            bool DithOffFound = false;
            foreach (string str in DitherOffsetEnums)
            {
                if (!DithOffFound)
                {
                    string paramcompare = m_Component.ResolveEnumIdToDisplay("DitherOffset", prms.DitherOffset.Expr);
                    if (paramcompare == str)
                    {
                        m_cbDitherOffset.SelectedItem = paramcompare;
                        DithOffFound = true;
                    }
                }
            }
            if (!DithOffFound)
            {
                m_cbDitherOffset.Text = prms.DitherOffset.Expr;
            }

            //Set the CompareType1 Offset Combo Box
            IEnumerable<string> CT1Enums = inst.GetPossibleEnumValues("CompareType1");
            bool CT1found = false;
            foreach (string str in CT1Enums)
            {
                if (!CT1found)
                {
                    string paramcompare = m_Component.ResolveEnumIdToDisplay("CompareType1", prms.CompareType1.Expr);
                    if (paramcompare == str)
                    {
                        m_cbCompareType1.SelectedItem = paramcompare;
                        CT1found = true;
                    }
                }
            }
            if (!CT1found)
            {
                m_cbCompareType1.Text = prms.CompareType1.Expr;
            }

            //Set the CompareType2 Offset Combo Box
            IEnumerable<string> CT2Enums = inst.GetPossibleEnumValues("CompareType2");
            bool CT2found = false;
            foreach (string str in CT2Enums)
            {
                if (!CT2found)
                {
                    string paramcompare = m_Component.ResolveEnumIdToDisplay("CompareType2", prms.CompareType2.Expr);
                    if (paramcompare == str)
                    {
                        m_cbCompareType2.SelectedItem = paramcompare;
                        CT2found = true;
                    }
                }
            }
            if (!CT2found)
            {
                m_cbCompareType2.Text = prms.CompareType2.Expr;
            }

            //Set the Dead Band Mode Combo Box
            IEnumerable<string> DBModeEnums = inst.GetPossibleEnumValues("DeadBand");
            bool DBMfound = false;
            foreach (string str in DBModeEnums)
            {
                if (!DBMfound)
                {
                    string paramcompare = m_Component.ResolveEnumIdToDisplay("DeadBand", prms.DeadBand.Expr);
                    if (paramcompare == str)
                    {
                        m_cbDeadBandMode.SelectedItem = paramcompare;
                        if (paramcompare.Contains("Disabled"))

                            m_cbFFDeadBandMode.SelectedIndex = 0;
                        else
                            m_cbFFDeadBandMode.SelectedIndex = 1;
                        DBMfound = true;
                    }
                }
            }
            if (!DBMfound)
            {
                m_cbDeadBandMode.Text = prms.DeadBand.Expr;
            }

            switch (prms.FixedFunction.Value)
            {
                case "true": SetFixedFunction(); m_numDeadBandCounts.Value = Convert.ToInt32(prms.DeadTime.Value); break;
                case "false":
                    ClearFixedFunction();
                    if (Convert.ToInt32(prms.DeadTime.Value) + 1 > m_numDeadBandCounts.Maximum)
                        m_numDeadBandCounts.Value = m_numDeadBandCounts.Maximum;
                    else
                        m_numDeadBandCounts.Value = Convert.ToInt32(prms.DeadTime.Value) + 1;
                    break;
            }
            SetFrequencyLabel(Convert.ToUInt16(m_numPeriod.Value));
            UpdateDrawing(prms);
        }

        private void SetFrequencyLabel(int period)
        {
            List<CyClockData> clkdata = new List<CyClockData>();
            clkdata = m_TermQuery.GetClockData("clock", 0);
            if (clkdata[0].IsFrequencyKnown)
            {
                double infreq = clkdata[0].Frequency;
                switch (clkdata[0].Unit)
                {
                    case CyClockUnit.kHz: infreq = infreq * 1000; break;
                    case CyClockUnit.MHz: infreq = infreq * 1000000; break;
                }
                double periodfreq = infreq / period;
                double periodtime = 1 / periodfreq;

                int i = 0;
                while (periodtime < 1)
                {
                    periodtime = periodtime * 1000;
                    i += 3;
                }
                string time = "s";
                switch (i)
                {
                    case 0: time = "s"; break;
                    case 3: time = "ms"; break;
                    case 6: time = "us"; break;
                    case 9: time = "ns"; break;
                    case 12: time = "ps"; break;
                }
                m_lblCalcFrequency.Text = string.Format("Period = {0}{1}", Math.Round(periodtime, 3), time);
                //Set the Tooltip m_lblCalcFrequency.To
            }
            else
            {
                m_lblCalcFrequency.Text = "Period = UNKNOWN SOURCE FREQ";
            }
        }

        public void UpdateDrawing(PWMParameters prms)
        {
            if ((m_pbDrawing.Width == 0) || (m_pbDrawing.Height == 0))
                return;
            Image waveform = new Bitmap(m_pbDrawing.Width, m_pbDrawing.Height);
            Graphics wfg;
            wfg = Graphics.FromImage(waveform);
            wfg.Clear(Color.White);
            SolidBrush blkbrush = new SolidBrush(Color.Black);


            float extentsleft = PB_EXTENTS_BORDER + PB_PWMTEXT_WIDTH;
            float extentsright = m_pbDrawing.Width - PB_EXTENTS_BORDER;
            float padding = (extentsright - extentsleft) / 70;
            float startleft = extentsleft + padding;
            float endright = extentsright - padding;
            float startright = startleft + (endright - startleft) / 2;
            float cacenter1 = startleft + (startright - startleft) / 2;
            float cacenter2 = startright + (endright - startright) / 2;

            //Setup the right, left and center indicators
            Pen extentspen = new Pen(blkbrush);
            extentspen.DashStyle = System.Drawing.Drawing2D.DashStyle.Dash;
            //Draw the Left Extents Line
            wfg.DrawLine(extentspen, extentsleft, PB_EXTENTS_BORDER,
                extentsleft, m_pbDrawing.Height - PB_EXTENTS_BORDER);
            //Draw the Right Extents Line
            wfg.DrawLine(extentspen, extentsright, PB_EXTENTS_BORDER,
                extentsright, m_pbDrawing.Height - PB_EXTENTS_BORDER);
            //Draw the Left Start line
            wfg.DrawLine(extentspen, startleft, PB_EXTENTS_BORDER,
                startleft, m_pbDrawing.Height - PB_EXTENTS_BORDER);
            //Draw the Right Start Line
            wfg.DrawLine(extentspen, startright, PB_EXTENTS_BORDER,
                startright, m_pbDrawing.Height - PB_EXTENTS_BORDER);
            //Draw the Right End Line
            wfg.DrawLine(extentspen, endright, PB_EXTENTS_BORDER,
                endright, m_pbDrawing.Height - PB_EXTENTS_BORDER);
            //Fill the Left Padding Rectangle
            wfg.FillRectangle(new SolidBrush(Color.FromArgb(100, Color.LightGray)),
                extentsleft, PB_EXTENTS_BORDER, padding, m_pbDrawing.Height - PB_EXTENTS_BORDER);
            //Fill the Right Padding Rectangle
            wfg.FillRectangle(new SolidBrush(Color.FromArgb(100, Color.LightGray)),
                endright, PB_EXTENTS_BORDER, padding, m_pbDrawing.Height - PB_EXTENTS_BORDER);

            if (prms.PWMMode.Expr.Contains("Center"))
            {
                wfg.DrawLine(extentspen, cacenter1, PB_EXTENTS_BORDER,
                cacenter1, m_pbDrawing.Height - PB_EXTENTS_BORDER);
                wfg.DrawLine(extentspen, cacenter2, PB_EXTENTS_BORDER,
                cacenter2, m_pbDrawing.Height - PB_EXTENTS_BORDER);
            }
            extentspen.Dispose();

            //Draw the Line with the Period Value
            Pen periodspen = new Pen(blkbrush);
            wfg.DrawLine(periodspen, startleft, PB_EXTENTS_BORDER + PB_POLYGON_WIDTH,
                endright, PB_EXTENTS_BORDER + PB_POLYGON_WIDTH);

            //Draw the Left Side Left Arrow
            PointF[] arrowpts = new PointF[3];
            arrowpts[0] = new PointF(startleft, PB_EXTENTS_BORDER + PB_POLYGON_WIDTH);
            arrowpts[1] = new PointF(startleft + PB_POLYGON_WIDTH, PB_EXTENTS_BORDER);
            arrowpts[2] = new PointF(startleft + PB_POLYGON_WIDTH,
                PB_EXTENTS_BORDER + (2 * PB_POLYGON_WIDTH));
            wfg.FillPolygon(blkbrush, arrowpts);

            //Draw the Right Side Left Arrow
            arrowpts = new PointF[3];
            arrowpts[0] = new PointF(startright, PB_EXTENTS_BORDER + PB_POLYGON_WIDTH);
            arrowpts[1] = new PointF(startright + PB_POLYGON_WIDTH, PB_EXTENTS_BORDER);
            arrowpts[2] = new PointF(startright + PB_POLYGON_WIDTH,
                PB_EXTENTS_BORDER + (2 * PB_POLYGON_WIDTH));
            wfg.FillPolygon(blkbrush, arrowpts);

            //Draw the Left Side Right Arrow
            arrowpts[0] = new PointF(startright, PB_EXTENTS_BORDER + PB_POLYGON_WIDTH);
            arrowpts[1] = new PointF(startright - PB_POLYGON_WIDTH, PB_EXTENTS_BORDER);
            arrowpts[2] = new PointF(startright - PB_POLYGON_WIDTH,
                PB_EXTENTS_BORDER + (2 * PB_POLYGON_WIDTH));
            wfg.FillPolygon(blkbrush, arrowpts);

            //Draw the Right Side Right Arrow
            arrowpts[0] = new PointF(endright, PB_EXTENTS_BORDER + PB_POLYGON_WIDTH);
            arrowpts[1] = new PointF(endright - PB_POLYGON_WIDTH, PB_EXTENTS_BORDER);
            arrowpts[2] = new PointF(endright - PB_POLYGON_WIDTH,
                PB_EXTENTS_BORDER + (2 * PB_POLYGON_WIDTH));
            wfg.FillPolygon(blkbrush, arrowpts);

            if (prms.PWMMode.Expr.Contains("Center"))
            {
                arrowpts[0] = new PointF(cacenter1, PB_EXTENTS_BORDER + PB_POLYGON_WIDTH);
                arrowpts[1] = new PointF(cacenter1 - PB_POLYGON_WIDTH, PB_EXTENTS_BORDER);
                arrowpts[2] = new PointF(cacenter1 - PB_POLYGON_WIDTH,
                    PB_EXTENTS_BORDER + (2 * PB_POLYGON_WIDTH));
                wfg.FillPolygon(blkbrush, arrowpts);
                arrowpts[0] = new PointF(cacenter1, PB_EXTENTS_BORDER + PB_POLYGON_WIDTH);
                arrowpts[1] = new PointF(cacenter1 + PB_POLYGON_WIDTH, PB_EXTENTS_BORDER);
                arrowpts[2] = new PointF(cacenter1 + PB_POLYGON_WIDTH,
                    PB_EXTENTS_BORDER + (2 * PB_POLYGON_WIDTH));
                wfg.FillPolygon(blkbrush, arrowpts);
                arrowpts[0] = new PointF(cacenter2, PB_EXTENTS_BORDER + PB_POLYGON_WIDTH);
                arrowpts[1] = new PointF(cacenter2 - PB_POLYGON_WIDTH, PB_EXTENTS_BORDER);
                arrowpts[2] = new PointF(cacenter2 - PB_POLYGON_WIDTH,
                    PB_EXTENTS_BORDER + (2 * PB_POLYGON_WIDTH));
                wfg.FillPolygon(blkbrush, arrowpts);
                arrowpts[0] = new PointF(cacenter2, PB_EXTENTS_BORDER + PB_POLYGON_WIDTH);
                arrowpts[1] = new PointF(cacenter2 + PB_POLYGON_WIDTH, PB_EXTENTS_BORDER);
                arrowpts[2] = new PointF(cacenter2 + PB_POLYGON_WIDTH,
                    PB_EXTENTS_BORDER + (2 * PB_POLYGON_WIDTH));
                wfg.FillPolygon(blkbrush, arrowpts);
            }

            //Draw the Period Name String
            Font perfont = new Font("Arial", 10, FontStyle.Regular, GraphicsUnit.Pixel);
            wfg.DrawString("period", perfont, blkbrush, new PointF(PB_PWMTEXT_WIDTH - wfg.MeasureString("Period", perfont).Width,
                PB_EXTENTS_BORDER + PB_POLYGON_WIDTH - (wfg.MeasureString("period", perfont).Height / 2)));

            //Draw the Period Text at the start and at the end of the waveform
            if (!prms.PWMMode.Expr.Contains("Center"))
            {

                //Draw the Period Values at the Left
                SizeF pervalsize = wfg.MeasureString(prms.Period.Value, perfont);
                RectangleF pervalrect1 = new RectangleF(startleft + (2 * PB_POLYGON_WIDTH),
                    PB_EXTENTS_BORDER + PB_POLYGON_WIDTH - (pervalsize.Height / 2), pervalsize.Width, pervalsize.Height);
                wfg.FillRectangle(new SolidBrush(Color.White), pervalrect1);
                wfg.DrawString(prms.Period.Value, perfont, blkbrush, pervalrect1);

                RectangleF pervalrect2 = new RectangleF(startright + (2 * PB_POLYGON_WIDTH),
                    PB_EXTENTS_BORDER + PB_POLYGON_WIDTH - (pervalsize.Height / 2), pervalsize.Width, pervalsize.Height);
                wfg.FillRectangle(new SolidBrush(Color.White), pervalrect2);
                wfg.DrawString(prms.Period.Value, perfont, blkbrush, pervalrect2);

                //Draw the zero Values at the Right
                SizeF perzerosize = wfg.MeasureString("0", perfont);
                RectangleF perzerorect1 = new RectangleF(startright - (2 * PB_POLYGON_WIDTH) - perzerosize.Width,
                    PB_EXTENTS_BORDER + PB_POLYGON_WIDTH - (perzerosize.Height / 2), perzerosize.Width, perzerosize.Height);
                wfg.FillRectangle(new SolidBrush(Color.White), perzerorect1);
                wfg.DrawString("0", perfont, blkbrush, perzerorect1);

                RectangleF perzerorect2 = new RectangleF(endright - (2 * PB_POLYGON_WIDTH) - perzerosize.Width,
                    PB_EXTENTS_BORDER + PB_POLYGON_WIDTH - (perzerosize.Height / 2), perzerosize.Width, perzerosize.Height);
                wfg.FillRectangle(new SolidBrush(Color.White), perzerorect2);
                wfg.DrawString("0", perfont, blkbrush, perzerorect2);
            }
            else
            {
                //If Center Aligned mode then count from zero up to period and then back down
                //Draw the 0 text in the three locations
                SizeF zerosize = wfg.MeasureString("0", perfont);
                RectangleF zero1rect = new RectangleF(startleft + (2 * PB_POLYGON_WIDTH),
                    PB_EXTENTS_BORDER + PB_POLYGON_WIDTH - (zerosize.Height / 2), zerosize.Width, zerosize.Height);
                wfg.FillRectangle(new SolidBrush(Color.White), zero1rect);
                wfg.DrawString("0", perfont, blkbrush, zero1rect);

                RectangleF zero2rect = new RectangleF(startright - (2 * PB_POLYGON_WIDTH) - zerosize.Width,
                    PB_EXTENTS_BORDER + PB_POLYGON_WIDTH - (zerosize.Height / 2), zerosize.Width, zerosize.Height);
                wfg.FillRectangle(new SolidBrush(Color.White), zero2rect);
                wfg.DrawString("0", perfont, blkbrush, zero2rect);

                RectangleF zero3rect = new RectangleF(endright - (2 * PB_POLYGON_WIDTH) - zerosize.Width,
                    PB_EXTENTS_BORDER + PB_POLYGON_WIDTH - (zerosize.Height / 2), zerosize.Width, zerosize.Height);
                wfg.FillRectangle(new SolidBrush(Color.White), zero3rect);
                wfg.DrawString("0", perfont, blkbrush, zero3rect);



                SizeF periodsize = wfg.MeasureString(prms.Period.Value, perfont);
                RectangleF per1rect = new RectangleF(cacenter1 + (2 * PB_POLYGON_WIDTH),
                    PB_EXTENTS_BORDER + PB_POLYGON_WIDTH - (periodsize.Height / 2), periodsize.Width, periodsize.Height);
                wfg.FillRectangle(new SolidBrush(Color.White), per1rect);
                wfg.DrawString(prms.Period.Value, perfont, blkbrush, per1rect);

                RectangleF per2rect = new RectangleF(cacenter2 + (2 * PB_POLYGON_WIDTH),
                    PB_EXTENTS_BORDER + PB_POLYGON_WIDTH - (periodsize.Height / 2), periodsize.Width, periodsize.Height);
                wfg.FillRectangle(new SolidBrush(Color.White), per2rect);
                wfg.DrawString(prms.Period.Value, perfont, blkbrush, per2rect);
            }

            //setup and draw all of the waveforms
            int numwaveforms = 1;
            //Number of waveforms is dependant upon the PWM mode and Dead Band
            //If Mode = One Output
            //  If DeadBand then 3 waveforms
            //  else 1 waveform
            //Else If Mode == Two Outputs
            //  If DeadBand then 4 waveforms
            //  else 2 waveforms
            //Else (Mode == Center Aligned, Dither, or Dual Mode)
            //  If DeadBand then 2 waveforms
            //  Else 1 Waveform 
            string[] wfnames = new string[5];

            if (prms.PWMMode.Expr.Contains("One") && prms.PWMMode.Expr.Contains("Output"))
            {
                if (prms.DeadBand.Value == "1" || prms.DeadBand.Value == "2")
                {
                    numwaveforms = 3;
                    wfnames[0] = "pwm";
                    wfnames[1] = "ph1";
                    wfnames[2] = "ph2";
                }
                else
                {
                    numwaveforms = 1;
                    wfnames[0] = "pwm";
                }
            }
            else if (prms.PWMMode.Expr.Contains("Two") && prms.PWMMode.Expr.Contains("Output"))
            {
                if (prms.DeadBand.Value == "1" || prms.DeadBand.Value == "2")
                {
                    numwaveforms = 4;
                    wfnames[0] = "pwm1";
                    wfnames[1] = "pwm2";
                    wfnames[2] = "ph1";
                    wfnames[3] = "ph2";
                }
                else
                {
                    numwaveforms = 2;
                    wfnames[0] = "pwm1";
                    wfnames[1] = "pwm2";
                }
            }
            else if (prms.PWMMode.Expr.Contains("Dual") && prms.PWMMode.Expr.Contains("Edge"))
            {
                if (prms.DeadBand.Value == "1" || prms.DeadBand.Value == "2")
                {
                    numwaveforms = 5;
                    wfnames[0] = "pwm1";
                    wfnames[1] = "pwm2";
                    wfnames[2] = "pwm";
                    wfnames[3] = "ph1";
                    wfnames[4] = "ph2";
                }
                else
                {
                    numwaveforms = 3;
                    wfnames[0] = "pwm1";
                    wfnames[1] = "pwm2";
                    wfnames[2] = "pwm";
                }
            }
            else if (prms.PWMMode.Expr.Contains("Center"))
            {
                if (prms.DeadBand.Value == "1" || prms.DeadBand.Value == "2")
                {
                    numwaveforms = 3;
                    wfnames[0] = "pwm";
                    wfnames[1] = "ph1";
                    wfnames[2] = "ph2";
                }
                else
                {
                    numwaveforms = 1;
                    wfnames[0] = "pwm";
                }
            }
            else if (prms.PWMMode.Expr.Contains("Hardware"))
            {
                if (prms.DeadBand.Value == "1" || prms.DeadBand.Value == "2")
                {
                    numwaveforms = 5;
                    wfnames[0] = "pwm1";
                    wfnames[1] = "pwm2";
                    wfnames[2] = "pwm";
                    wfnames[3] = "ph1";
                    wfnames[4] = "ph2";
                }
                else
                {
                    numwaveforms = 3;
                    wfnames[0] = "pwm1";
                    wfnames[1] = "pwm2";
                    wfnames[2] = "pwm";
                }
            }
            else if (prms.PWMMode.Expr.Contains("Dither"))
            {
                if (prms.DeadBand.Value == "1" || prms.DeadBand.Value == "2")
                {
                    numwaveforms = 3;
                    wfnames[0] = "pwm";
                    wfnames[1] = "ph1";
                    wfnames[2] = "ph2";
                }
                else
                {
                    numwaveforms = 1;
                    wfnames[0] = "pwm";
                }
            }

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
            bool StartHigh = false;
            float Edge1 = 0;
            float Edge1_2 = 0;
            float Edge2 = 0;
            float Edge2_2 = 0;
            float bitwidth = 0;
            if (prms.PWMMode.Expr.Contains("Center"))
                bitwidth = ((float)(startright - startleft) / (float)Convert.ToInt32(prms.Period.Value)) / 2f;
            else
                bitwidth = (float)(startright - startleft) / (float)Convert.ToInt32(prms.Period.Value);

            float dbcounts = (float)Convert.ToInt32(prms.DeadTime.Value) * bitwidth;
            PointF[] pts = new PointF[16]; //Maximum is 16 values.
            PointF[] ptsDBph1 = new PointF[12];
            PointF[] ptsDBph2 = new PointF[12];
            for (int i = 0; i < numwaveforms; i++)
            {
                float HighY = PB_EXTENTS_BORDER + (2 * PB_POLYGON_WIDTH) + (wfheight * i) + (wfheight / 8);
                float LowY = PB_EXTENTS_BORDER + (2 * PB_POLYGON_WIDTH) + (wfheight * (i + 1));
                float CenterY = HighY + ((LowY - HighY) / 2);
                string comparestring1 = null;
                switch (wfnames[i])
                {
                    #region PWM1
                    case "pwm1":
                        //Find the parameters for PWM1
                        Edge1 = startleft + (bitwidth * (float)(Convert.ToInt32(prms.Period.Value) - Convert.ToInt32(prms.CompareValue1.Value)));
                        Edge1_2 = startright + (bitwidth * (float)(Convert.ToInt32(prms.Period.Value) - Convert.ToInt32(prms.CompareValue1.Value)));
                        if (prms.CompareType1.Expr.Contains("Less"))
                        {
                            if (prms.CompareType1.Expr.Contains("Equal"))
                            {
                                StartHigh = false;
                                comparestring1 = " <= " + prms.CompareValue1.Value;
                            }
                            else
                            {
                                StartHigh = false;
                                Edge1 += bitwidth;
                                Edge1_2 += bitwidth;
                                comparestring1 = " < " + prms.CompareValue1.Value;
                            }
                        }
                        else if (prms.CompareType1.Expr.Contains("Greater"))
                        {
                            if (prms.CompareType1.Expr.Contains("Equal"))
                            {
                                StartHigh = true;
                                comparestring1 = " >= " + prms.CompareValue1.Value;
                            }
                            else
                            {
                                StartHigh = true;
                                Edge1 -= bitwidth;
                                Edge1_2 -= bitwidth;
                                comparestring1 = " > " + prms.CompareValue1.Value;
                            }
                        }
                        else
                        {
                            StartHigh = false;
                            Edge1 += bitwidth;
                            Edge1_2 += bitwidth;
                            comparestring1 = " < " + prms.CompareValue1.Value;
                        }
                        //Draw the first version of the waveform from StartLeft to StartRight
                        pts[0] = new PointF(startleft, StartHigh ? HighY : LowY);
                        pts[1] = new PointF(Edge1, StartHigh ? HighY : LowY);
                        pts[2] = new PointF(Edge1, StartHigh ? LowY : HighY);
                        pts[3] = new PointF(startright, StartHigh ? LowY : HighY);
                        pts[4] = new PointF(startright, StartHigh ? HighY : LowY);
                        pts[5] = new PointF(Edge1_2, StartHigh ? HighY : LowY);
                        pts[6] = new PointF(Edge1_2, StartHigh ? LowY : HighY);
                        pts[7] = new PointF(endright, StartHigh ? LowY : HighY);
                        pts[8] = pts[7];
                        pts[9] = pts[7];
                        pts[10] = pts[7];
                        pts[11] = pts[7];
                        pts[12] = pts[7];
                        pts[13] = pts[7];
                        pts[14] = pts[7];
                        pts[15] = pts[7];
                        if (StartHigh)
                        {
                            ptsDBph1[0] = new PointF(startleft, 0f);
                            ptsDBph1[1] = new PointF(startleft + dbcounts, 0f);
                            ptsDBph1[2] = new PointF(startleft + dbcounts, 1f);
                            ptsDBph1[3] = new PointF(Edge1, 1f);
                            ptsDBph1[4] = new PointF(Edge1, 0f);
                            ptsDBph1[5] = new PointF(startright, 0f);
                            ptsDBph1[6] = new PointF(startright, 0f);
                            ptsDBph1[7] = new PointF(startright + dbcounts, 0f);
                            ptsDBph1[8] = new PointF(startright + dbcounts, 1f);
                            ptsDBph1[9] = new PointF(Edge1_2, 1f);
                            ptsDBph1[10] = new PointF(Edge1_2, 0f);
                            ptsDBph1[11] = new PointF(endright, 0f);

                            ptsDBph2[0] = new PointF(startleft, 0f);
                            ptsDBph2[1] = new PointF(Edge1 + dbcounts, 0f);
                            ptsDBph2[2] = new PointF(Edge1 + dbcounts, 1f);
                            ptsDBph2[3] = new PointF(startright, 1f);
                            ptsDBph2[4] = new PointF(startright, 0f);
                            ptsDBph2[5] = new PointF(Edge1_2 + dbcounts, 0f);
                            ptsDBph2[6] = new PointF(Edge1_2 + dbcounts, 1f);
                            ptsDBph2[7] = new PointF(endright, 1f);
                            ptsDBph2[8] = ptsDBph2[7];
                            ptsDBph2[9] = ptsDBph2[7];
                            ptsDBph2[10] = ptsDBph2[7];
                            ptsDBph2[11] = ptsDBph2[7];
                        }
                        else
                        {
                            ptsDBph1[0] = new PointF(startleft, 0f);
                            ptsDBph1[1] = new PointF(Edge1 + dbcounts, 0f);
                            ptsDBph1[2] = new PointF(Edge1 + dbcounts, 1f);
                            ptsDBph1[3] = new PointF(startright, 1f);
                            ptsDBph1[4] = new PointF(startright, 0f);
                            ptsDBph1[5] = new PointF(Edge1_2 + dbcounts, 0f);
                            ptsDBph1[6] = new PointF(Edge1_2 + dbcounts, 1f);
                            ptsDBph1[7] = new PointF(endright, 1f);
                            ptsDBph1[8] = ptsDBph1[7];
                            ptsDBph1[9] = ptsDBph1[7];
                            ptsDBph1[10] = ptsDBph1[7];
                            ptsDBph1[11] = ptsDBph1[7];


                            ptsDBph2[0] = new PointF(startleft, 0f);
                            ptsDBph2[1] = new PointF(startleft + dbcounts, 0f);
                            ptsDBph2[2] = new PointF(startleft + dbcounts, 1f);
                            ptsDBph2[3] = new PointF(Edge1, 1f);
                            ptsDBph2[4] = new PointF(Edge1, 0f);
                            ptsDBph2[5] = new PointF(startright, 0f);
                            ptsDBph2[6] = new PointF(startright, 0f);
                            ptsDBph2[7] = new PointF(startright + dbcounts, 0f);
                            ptsDBph2[8] = new PointF(startright + dbcounts, 1f);
                            ptsDBph2[9] = new PointF(Edge1_2, 1f);
                            ptsDBph2[10] = new PointF(Edge1_2, 0f);
                            ptsDBph2[11] = new PointF(endright, 0f);
                        }
                        break;
                    #endregion
                    #region PWM2
                    case "pwm2":
                        //Find the parameters for PWM2
                        Edge1 = startleft + (bitwidth * (float)(Convert.ToInt32(prms.Period.Value) - Convert.ToInt32(prms.CompareValue2.Value)));
                        Edge1_2 = startright + (bitwidth * (float)(Convert.ToInt32(prms.Period.Value) - Convert.ToInt32(prms.CompareValue2.Value)));
                        if (prms.CompareType2.Expr.Contains("Less"))
                        {
                            if (prms.CompareType2.Expr.Contains("Equal"))
                            {
                                StartHigh = false;
                                comparestring1 = " <= " + prms.CompareValue2.Value;
                            }
                            else
                            {
                                StartHigh = false;
                                Edge1 += bitwidth;
                                Edge1_2 += bitwidth;
                                comparestring1 = " < " + prms.CompareValue2.Value;
                            }
                        }
                        else if (prms.CompareType2.Expr.Contains("Greater"))
                        {
                            if (prms.CompareType2.Expr.Contains("Equal"))
                            {
                                StartHigh = true;
                                comparestring1 = " >= " + prms.CompareValue2.Value;
                            }
                            else
                            {
                                StartHigh = true;
                                Edge1 -= bitwidth;
                                Edge1_2 -= bitwidth;
                                comparestring1 = " > " + prms.CompareValue2.Value;
                            }
                        }
                        else
                        {
                            StartHigh = false;
                            Edge1 += bitwidth;
                            Edge1_2 += bitwidth;
                            comparestring1 = " < " + prms.CompareValue2.Value;
                        }


                        pts[0] = new PointF(startleft, StartHigh ? HighY : LowY);
                        pts[1] = new PointF(Edge1, StartHigh ? HighY : LowY);
                        pts[2] = new PointF(Edge1, StartHigh ? LowY : HighY);
                        pts[3] = new PointF(startright, StartHigh ? LowY : HighY);
                        pts[4] = new PointF(startright, StartHigh ? HighY : LowY);
                        pts[5] = new PointF(Edge1_2, StartHigh ? HighY : LowY);
                        pts[6] = new PointF(Edge1_2, StartHigh ? LowY : HighY);
                        pts[7] = new PointF(endright, StartHigh ? LowY : HighY);
                        pts[8] = pts[7];
                        pts[9] = pts[7];
                        pts[10] = pts[7];
                        pts[11] = pts[7];
                        pts[12] = pts[7];
                        pts[13] = pts[7];
                        pts[14] = pts[7];
                        pts[15] = pts[7];
                        break;
#endregion
                    #region PWM
                    case "pwm":
                        //Find the parameters for PWM
                        #region PWM-ONE
                        if (prms.PWMMode.Expr.Contains("One"))
                        {
                            Edge1 = startleft + (bitwidth * (float)(Convert.ToInt32(prms.Period.Value) - Convert.ToInt32(prms.CompareValue1.Value)));
                            Edge1_2 = startright + (bitwidth * (float)(Convert.ToInt32(prms.Period.Value) - Convert.ToInt32(prms.CompareValue1.Value)));
                            if (prms.CompareType1.Expr.Contains("Less"))
                            {
                                if (prms.CompareType1.Expr.Contains("Equal"))
                                {
                                    StartHigh = false;
                                    comparestring1 = " <= " + prms.CompareValue1.Value;
                                }
                                else
                                {
                                    StartHigh = false;
                                    Edge1 += bitwidth;
                                    Edge1_2 += bitwidth;
                                    comparestring1 = " < " + prms.CompareValue1.Value;
                                }
                            }
                            else if (prms.CompareType1.Expr.Contains("Greater"))
                            {
                                if (prms.CompareType1.Expr.Contains("Equal"))
                                {
                                    StartHigh = true;
                                    comparestring1 = " >= " + prms.CompareValue1.Value;
                                }
                                else
                                {
                                    StartHigh = true;
                                    Edge1 -= bitwidth;
                                    Edge1_2 -= bitwidth;
                                    comparestring1 = " > " + prms.CompareValue1.Value;
                                }
                            }
                            else
                            {
                                StartHigh = false;
                                Edge1 += bitwidth;
                                Edge1_2 += bitwidth;
                                comparestring1 = " < " + prms.CompareValue1.Value;
                            }

                            pts[0] = new PointF(startleft, StartHigh ? HighY : LowY);
                            pts[1] = new PointF(Edge1, StartHigh ? HighY : LowY);
                            pts[2] = new PointF(Edge1, StartHigh ? LowY : HighY);
                            pts[3] = new PointF(startright, StartHigh ? LowY : HighY);
                            pts[4] = new PointF(startright, StartHigh ? HighY : LowY);
                            pts[5] = new PointF(Edge1_2, StartHigh ? HighY : LowY);
                            pts[6] = new PointF(Edge1_2, StartHigh ? LowY : HighY);
                            pts[7] = new PointF(endright, StartHigh ? LowY : HighY);
                            pts[8] = pts[7];
                            pts[9] = pts[7];
                            pts[10] = pts[7];
                            pts[11] = pts[7];
                            pts[12] = pts[7];
                            pts[13] = pts[7];
                            pts[14] = pts[7];
                            pts[15] = pts[7];
                            if (StartHigh)
                            {
                                ptsDBph1[0] = new PointF(startleft, 0f);
                                ptsDBph1[1] = new PointF(startleft + dbcounts, 0f);
                                ptsDBph1[2] = new PointF(startleft + dbcounts, 1f);
                                ptsDBph1[3] = new PointF(Edge1, 1f);
                                ptsDBph1[4] = new PointF(Edge1, 0f);
                                ptsDBph1[5] = new PointF(startright, 0f);
                                ptsDBph1[6] = new PointF(startright, 0f);
                                ptsDBph1[7] = new PointF(startright + dbcounts, 0f);
                                ptsDBph1[8] = new PointF(startright + dbcounts, 1f);
                                ptsDBph1[9] = new PointF(Edge1_2, 1f);
                                ptsDBph1[10] = new PointF(Edge1_2, 0f);
                                ptsDBph1[11] = new PointF(endright, 0f);

                                ptsDBph2[0] = new PointF(startleft, 0f);
                                ptsDBph2[1] = new PointF(Edge1 + dbcounts, 0f);
                                ptsDBph2[2] = new PointF(Edge1 + dbcounts, 1f);
                                ptsDBph2[3] = new PointF(startright, 1f);
                                ptsDBph2[4] = new PointF(startright, 0f);
                                ptsDBph2[5] = new PointF(Edge1_2 + dbcounts, 0f);
                                ptsDBph2[6] = new PointF(Edge1_2 + dbcounts, 1f);
                                ptsDBph2[7] = new PointF(endright, 1f);
                                ptsDBph2[8] = ptsDBph2[7];
                                ptsDBph2[9] = ptsDBph2[7];
                                ptsDBph2[10] = ptsDBph2[7];
                                ptsDBph2[11] = ptsDBph2[7];
                            }
                            else
                            {
                                ptsDBph1[0] = new PointF(startleft, 0f);
                                ptsDBph1[1] = new PointF(Edge1 + dbcounts, 0f);
                                ptsDBph1[2] = new PointF(Edge1 + dbcounts, 1f);
                                ptsDBph1[3] = new PointF(startright, 1f);
                                ptsDBph1[4] = new PointF(startright, 0f);
                                ptsDBph1[5] = new PointF(Edge1_2 + dbcounts, 0f);
                                ptsDBph1[6] = new PointF(Edge1_2 + dbcounts, 1f);
                                ptsDBph1[7] = new PointF(endright, 1f);
                                ptsDBph1[8] = ptsDBph1[7];
                                ptsDBph1[9] = ptsDBph1[7];
                                ptsDBph1[10] = ptsDBph1[7];
                                ptsDBph1[11] = ptsDBph1[7];


                                ptsDBph2[0] = new PointF(startleft, 0f);
                                ptsDBph2[1] = new PointF(startleft + dbcounts, 0f);
                                ptsDBph2[2] = new PointF(startleft + dbcounts, 1f);
                                ptsDBph2[3] = new PointF(Edge1, 1f);
                                ptsDBph2[4] = new PointF(Edge1, 0f);
                                ptsDBph2[5] = new PointF(startright, 0f);
                                ptsDBph2[6] = new PointF(startright, 0f);
                                ptsDBph2[7] = new PointF(startright + dbcounts, 0f);
                                ptsDBph2[8] = new PointF(startright + dbcounts, 1f);
                                ptsDBph2[9] = new PointF(Edge1_2, 1f);
                                ptsDBph2[10] = new PointF(Edge1_2, 0f);
                                ptsDBph2[11] = new PointF(endright, 0f);
                            }
                        }
                        #endregion
                        #region PWM-DUAL
                        if (prms.PWMMode.Expr.Contains("Dual"))
                        {
                            //string comparetype1 = "0";
                            //string comparetype2 = "0";
                            string cmptype1 = "0";
                            string cmptype2 = "0";
                            //Situations:
                            // PWM1 = Less Than, PWM2 = Less Than, 
                            //      PWM = less than MIN(PWM1.value, PWM2.value)
                            // PWM1 = Less Than, PWM2 = Greater Than
                            //      PWM1.value less than PWM2.value
                            //          PWM = 0
                            //      PWM1.value greater than PWM2.value
                            //          PWM = Low to PWM1.Value, High to PWM2.Value, Low to end
                            // PWM1 = Greater Than, PWM2 = Less Than
                            //      PWM2.value less than PWM1.value
                            //          PWM = 0
                            //      PWM2.value greater than PWM1.value
                            //          PWM = Low to PWM2.Value, High to PWM1.Value, Low to end
                            // PWM1 = Greater Than, PWM2 = Greater Than
                            //      PWM = Greater than MAX(PWM1.value, PWM2.value)
                            // PWM1 = Equal To, PWM2 = Less Than
                            //      if(PWM2.Value > PWM1.Value) PWM = PWM1 only
                            //      else PWM = 0
                            // PWM1 = Equal To, PWM2 = Greater Than
                            //      if(PWM2.Value < PWM1.value) PWM = PWM1 only
                            //      else PWM = 0;

                            // Edge1 = PWM1 Edge
                            // Edge2 = PWM2 Edge

                            Edge1 = startleft + (bitwidth * (float)(Convert.ToInt32(prms.Period.Value) - Convert.ToInt32(prms.CompareValue1.Value)));
                            Edge1_2 = startright + (bitwidth * (float)(Convert.ToInt32(prms.Period.Value) - Convert.ToInt32(prms.CompareValue1.Value)));
                            Edge2 = startleft + (bitwidth * (float)(Convert.ToInt32(prms.Period.Value) - Convert.ToInt32(prms.CompareValue2.Value)));
                            Edge2_2 = startright + (bitwidth * (float)(Convert.ToInt32(prms.Period.Value) - Convert.ToInt32(prms.CompareValue2.Value)));

                            //if (Convert.ToInt32(prms.CompareValue1.Value) >= Convert.ToInt32(prms.CompareValue2.Value))
                            //{
                            //    /* PWM between Compare1 and Compare2 */
                            //    Edge1 = startleft + (bitwidth * (float)(Convert.ToInt32(prms.Period.Value) - Convert.ToInt32(prms.CompareValue1.Value)));
                            //    Edge1_2 = startright + (bitwidth * (float)(Convert.ToInt32(prms.Period.Value) - Convert.ToInt32(prms.CompareValue1.Value)));
                            //    Edge2 = startleft + (bitwidth * (float)(Convert.ToInt32(prms.Period.Value) - Convert.ToInt32(prms.CompareValue2.Value)));
                            //    Edge2_2 = startright + (bitwidth * (float)(Convert.ToInt32(prms.Period.Value) - Convert.ToInt32(prms.CompareValue2.Value)));
                            //    comparetype1 = prms.CompareType1.Value;
                            //    comparetype2 = prms.CompareType2.Value;
                            //}
                            //else
                            //{
                            //    /* PWM between Compare2 and Compare1 */
                            //    Edge1 = startleft + (bitwidth * (float)(Convert.ToInt32(prms.Period.Value) - Convert.ToInt32(prms.CompareValue2.Value)));
                            //    Edge1_2 = startright + (bitwidth * (float)(Convert.ToInt32(prms.Period.Value) - Convert.ToInt32(prms.CompareValue2.Value)));
                            //    Edge2 = startleft + (bitwidth * (float)(Convert.ToInt32(prms.Period.Value) - Convert.ToInt32(prms.CompareValue1.Value)));
                            //    Edge2_2 = startright + (bitwidth * (float)(Convert.ToInt32(prms.Period.Value) - Convert.ToInt32(prms.CompareValue1.Value)));
                            //    comparetype1 = prms.CompareType2.Value;
                            //    comparetype2 = prms.CompareType1.Value;
                            //}

                            //Get Compare Type Information for Compare Type 1
                            if (prms.CompareType1.Expr.Contains("Less"))
                            {
                                if (prms.CompareType1.Expr.Contains("Equal"))
                                {
                                    cmptype1 = "0";
                                }
                                else
                                {
                                    cmptype1 = "0";
                                    Edge1 += bitwidth;
                                    Edge1_2 += bitwidth;
                                }
                            }
                            else if (prms.CompareType1.Expr.Contains("Greater"))
                            {
                                if (prms.CompareType1.Expr.Contains("Equal"))
                                {
                                    cmptype1 = "1";
                                }
                                else
                                {
                                    cmptype1 = "1";
                                    Edge1 -= bitwidth;
                                    Edge1_2 -= bitwidth;
                                }
                            }
                            else
                            {
                                StartHigh = false;
                                Edge1 += bitwidth;
                                Edge1_2 += bitwidth;
                                comparestring1 = " < " + prms.CompareValue1.Value;
                            }

                            //Get Compare Type Information for Compare Type 2
                            if (prms.CompareType2.Expr.Contains("Less"))
                            {
                                if (prms.CompareType2.Expr.Contains("Equal"))
                                {
                                    cmptype2 = "0";
                                }
                                else
                                {
                                    cmptype2 = "0";
                                    Edge2 += bitwidth;
                                    Edge2_2 += bitwidth;
                                }
                            }
                            else if (prms.CompareType2.Expr.Contains("Greater"))
                            {
                                if (prms.CompareType2.Expr.Contains("Equal"))
                                {
                                    cmptype2 = "1";
                                }
                                else
                                {
                                    cmptype2 = "1";
                                    Edge2 -= bitwidth;
                                    Edge2_2 -= bitwidth;
                                }
                            }
                            else
                            {
                                cmptype2 = "0";
                                Edge2 += bitwidth;
                                Edge2_2 += bitwidth;
                            }

                            switch (cmptype1 + cmptype2)
                            {
                                case "00": // Compare1 = Less Than, Compare2 = Less Than
                                    StartHigh = false;
                                    pts[0] = new PointF(startleft, LowY);
                                    pts[1] = new PointF(Math.Max(Edge1, Edge2), LowY);
                                    pts[2] = new PointF(Math.Max(Edge1, Edge2), HighY);
                                    pts[3] = new PointF(startright, HighY);
                                    pts[4] = new PointF(startright, LowY);
                                    pts[5] = new PointF(Math.Max(Edge1_2, Edge2_2), LowY);
                                    pts[6] = new PointF(Math.Max(Edge1_2, Edge2_2), HighY);
                                    pts[7] = new PointF(endright, HighY);
                                    pts[8] = pts[7];
                                    pts[9] = pts[7];
                                    pts[10] = pts[7];
                                    pts[11] = pts[7];
                                    pts[12] = pts[7];
                                    pts[13] = pts[7];
                                    pts[14] = pts[7];
                                    pts[15] = pts[7];


                                    if ((Math.Max(Edge1, Edge2) + dbcounts) < startright)
                                    {
                                        ptsDBph1[0] = new PointF(startleft, 0f);
                                        ptsDBph1[1] = new PointF(Math.Max(Edge1, Edge2) + dbcounts, 0f);
                                        ptsDBph1[2] = new PointF(Math.Max(Edge1, Edge2) + dbcounts, 1f);
                                        ptsDBph1[3] = new PointF(startright, 1f);
                                        ptsDBph1[4] = new PointF(startright, 0f);
                                        ptsDBph1[5] = new PointF(Math.Max(Edge1_2, Edge2_2) + dbcounts, 0f);
                                        ptsDBph1[6] = new PointF(Math.Max(Edge1_2, Edge2_2) + dbcounts, 1f);
                                        ptsDBph1[7] = new PointF(endright, 1f);
                                        ptsDBph1[8] = ptsDBph1[7];
                                        ptsDBph1[9] = ptsDBph1[7];
                                        ptsDBph1[10] = ptsDBph1[7];
                                        ptsDBph1[11] = ptsDBph1[7];
                                    }
                                    else
                                    {
                                        ptsDBph1[0] = new PointF(startleft, 0f);
                                        ptsDBph1[1] = new PointF(endright, 0f);
                                        ptsDBph1[2] = ptsDBph1[1];
                                        ptsDBph1[3] = ptsDBph1[1];
                                        ptsDBph1[4] = ptsDBph1[1];
                                        ptsDBph1[5] = ptsDBph1[1];
                                        ptsDBph1[6] = ptsDBph1[1];
                                        ptsDBph1[7] = ptsDBph1[1];
                                        ptsDBph1[8] = ptsDBph1[1];
                                        ptsDBph1[9] = ptsDBph1[1];
                                        ptsDBph1[10] = ptsDBph1[1];
                                        ptsDBph1[11] = ptsDBph1[1];
                                    }

                                    if ((startleft + dbcounts) < Math.Max(Edge1, Edge2))
                                    {
                                        ptsDBph2[0] = new PointF(startleft, 0f);
                                        ptsDBph2[1] = new PointF(startleft + dbcounts, 0f);
                                        ptsDBph2[2] = new PointF(startleft + dbcounts, 1f);
                                        ptsDBph2[3] = new PointF(Math.Max(Edge1, Edge2), 1f);
                                        ptsDBph2[4] = new PointF(Math.Max(Edge1, Edge2), 0f);
                                        ptsDBph2[5] = new PointF(startright, 0f);
                                        ptsDBph2[6] = new PointF(startright, 0f);
                                        ptsDBph2[7] = new PointF(startright + dbcounts, 0f);
                                        ptsDBph2[8] = new PointF(startright + dbcounts, 1f);
                                        ptsDBph2[9] = new PointF(Math.Max(Edge1_2, Edge2_2), 1f);
                                        ptsDBph2[10] = new PointF(Math.Max(Edge1_2, Edge2_2), 0f);
                                        ptsDBph2[11] = new PointF(endright, 0f);
                                    }
                                    else
                                    {
                                        ptsDBph2[0] = new PointF(startleft, 0f);
                                        ptsDBph2[1] = new PointF(endright, 0f);
                                        ptsDBph2[2] = ptsDBph2[1];
                                        ptsDBph2[3] = ptsDBph2[1];
                                        ptsDBph2[4] = ptsDBph2[1];
                                        ptsDBph2[5] = ptsDBph2[1];
                                        ptsDBph2[6] = ptsDBph2[1];
                                        ptsDBph2[7] = ptsDBph2[1];
                                        ptsDBph2[8] = ptsDBph2[1];
                                        ptsDBph2[9] = ptsDBph2[1];
                                        ptsDBph2[10] = ptsDBph2[1];
                                        ptsDBph2[11] = ptsDBph2[1];
                                    }
                                    break;
                                case "01":  
                                case "10"://Low to High To Low
                                    StartHigh = false;
                                    if ((((cmptype1 + cmptype2) == "01") && (Edge1 < Edge2)) ||
                                        (((cmptype1 + cmptype2) == "10") && (Edge2 < Edge1)))
                                    {
                                        pts[0] = new PointF(startleft, LowY);
                                        pts[1] = new PointF(Math.Min(Edge1, Edge2), LowY);
                                        pts[2] = new PointF(Math.Min(Edge1, Edge2), HighY);
                                        pts[3] = new PointF(Math.Max(Edge1, Edge2), HighY);
                                        pts[4] = new PointF(Math.Max(Edge1, Edge2), LowY);
                                        pts[5] = new PointF(startright, LowY);
                                        pts[6] = new PointF(startright, LowY);
                                        pts[7] = new PointF(Math.Min(Edge1_2, Edge2_2), LowY);
                                        pts[8] = new PointF(Math.Min(Edge1_2, Edge2_2), HighY);
                                        pts[9] = new PointF(Math.Max(Edge1_2, Edge2_2), HighY);
                                        pts[10] = new PointF(Math.Max(Edge1_2, Edge2_2), LowY);
                                        pts[11] = new PointF(endright, LowY);
                                        pts[12] = pts[11];
                                        pts[13] = pts[11];
                                        pts[14] = pts[11];
                                        pts[15] = pts[11];
                                    }
                                    else
                                    {
                                        pts[0] = new PointF(startleft, LowY);
                                        pts[1] = new PointF(endright, LowY);
                                        pts[2] = pts[1];
                                        pts[3] = pts[1];
                                        pts[4] = pts[1];
                                        pts[5] = pts[1];
                                        pts[6] = pts[1];
                                        pts[7] = pts[1];
                                        pts[8] = pts[1];
                                        pts[9] = pts[1];
                                        pts[10] = pts[1];
                                        pts[11] = pts[1];
                                        pts[12] = pts[11];
                                        pts[13] = pts[11];
                                        pts[14] = pts[11];
                                        pts[15] = pts[11];
                                    }

                                    if ((((cmptype1 + cmptype2) == "01") && (Edge1 < Edge2)) ||
                                        (((cmptype1 + cmptype2) == "10") && (Edge2 < Edge1)))
                                    {
                                        if ((Math.Min(Edge1, Edge2) + dbcounts) < Math.Max(Edge1, Edge2))
                                        {
                                            ptsDBph1[0] = new PointF(startleft, 0f);
                                            ptsDBph1[1] = new PointF(Math.Min(Edge1, Edge2) + dbcounts, 0f);
                                            ptsDBph1[2] = new PointF(Math.Min(Edge1, Edge2) + dbcounts, 1f);
                                            ptsDBph1[3] = new PointF(Math.Max(Edge1, Edge2), 1f);
                                            ptsDBph1[4] = new PointF(Math.Max(Edge1, Edge2), 0f);
                                            ptsDBph1[5] = new PointF(startright, 0f);
                                            ptsDBph1[6] = new PointF(startright, 0f);
                                            ptsDBph1[7] = new PointF(Math.Min(Edge1_2, Edge2_2) + dbcounts, 0f);
                                            ptsDBph1[8] = new PointF(Math.Min(Edge1_2, Edge2_2) + dbcounts, 1f);
                                            ptsDBph1[9] = new PointF(Math.Max(Edge1_2, Edge2_2), 1f);
                                            ptsDBph1[10] = new PointF(Math.Max(Edge1_2, Edge2_2), 0f);
                                            ptsDBph1[11] = new PointF(endright, 0f);
                                        }
                                        else
                                        {
                                            ptsDBph1[0] = new PointF(startleft, 0f);
                                            ptsDBph1[1] = new PointF(endright, 0f);
                                            ptsDBph1[2] = ptsDBph1[1];
                                            ptsDBph1[3] = ptsDBph1[1];
                                            ptsDBph1[4] = ptsDBph1[1];
                                            ptsDBph1[5] = ptsDBph1[1];
                                            ptsDBph1[6] = ptsDBph1[1];
                                            ptsDBph1[7] = ptsDBph1[1];
                                            ptsDBph1[8] = ptsDBph1[1];
                                            ptsDBph1[9] = ptsDBph1[1];
                                            ptsDBph1[10] = ptsDBph1[1];
                                            ptsDBph1[11] = ptsDBph1[1];
                                        }
                                    }
                                    else
                                    {
                                        ptsDBph1[0] = new PointF(startleft, 0f);
                                        ptsDBph1[1] = new PointF(endright, 0f);
                                        ptsDBph1[2] = ptsDBph1[1];
                                        ptsDBph1[3] = ptsDBph1[1];
                                        ptsDBph1[4] = ptsDBph1[1];
                                        ptsDBph1[5] = ptsDBph1[1];
                                        ptsDBph1[6] = ptsDBph1[1];
                                        ptsDBph1[7] = ptsDBph1[1];
                                        ptsDBph1[8] = ptsDBph1[1];
                                        ptsDBph1[9] = ptsDBph1[1];
                                        ptsDBph1[10] = ptsDBph1[1];
                                        ptsDBph1[11] = ptsDBph1[1];
                                    }

                                    if ((((cmptype1 + cmptype2) == "01") && (Edge1 < Edge2)) ||
                                        (((cmptype1 + cmptype2) == "10") && (Edge2 < Edge1)))
                                    {
                                        ptsDBph2[0] = new PointF(startleft, 1f);
                                        ptsDBph2[1] = new PointF(Math.Min(Edge1, Edge2), 1f);
                                        ptsDBph2[2] = new PointF(Math.Min(Edge1, Edge2), 0f);
                                        ptsDBph2[3] = new PointF(Math.Max(Edge1, Edge2) + dbcounts, 0f);
                                        ptsDBph2[4] = new PointF(Math.Max(Edge1, Edge2) + dbcounts, 1f);
                                        ptsDBph2[5] = new PointF(startright, 1f);
                                        ptsDBph2[6] = new PointF(startright, 1f);
                                        ptsDBph2[7] = new PointF(Math.Min(Edge1_2, Edge2_2), 1f);
                                        ptsDBph2[8] = new PointF(Math.Min(Edge1_2, Edge2_2), 0f);
                                        ptsDBph2[9] = new PointF(Math.Max(Edge1_2, Edge2_2) + dbcounts, 0f);
                                        ptsDBph2[10] = new PointF(Math.Max(Edge1_2, Edge2_2) + dbcounts, 1f);
                                        ptsDBph2[11] = new PointF(endright, 1f);
                                    }
                                    else
                                    {
                                        ptsDBph2[0] = new PointF(startleft, 0f);
                                        ptsDBph2[1] = new PointF(endright, 0f);
                                        ptsDBph2[2] = ptsDBph2[1];
                                        ptsDBph2[3] = ptsDBph2[1];
                                        ptsDBph2[4] = ptsDBph2[1];
                                        ptsDBph2[5] = ptsDBph2[1];
                                        ptsDBph2[6] = ptsDBph2[1];
                                        ptsDBph2[7] = ptsDBph2[1];
                                        ptsDBph2[8] = ptsDBph2[1];
                                        ptsDBph2[9] = ptsDBph2[1];
                                        ptsDBph2[10] = ptsDBph2[1];
                                        ptsDBph2[11] = ptsDBph2[1];
                                    }
                                    break;
                                case "11": // High to Low
                                    StartHigh = true;
                                    pts[0] = new PointF(startleft, HighY);
                                    pts[1] = new PointF(Math.Min(Edge1,Edge2), HighY);
                                    pts[2] = new PointF(Math.Min(Edge1, Edge2), LowY);
                                    pts[3] = new PointF(startright, LowY);
                                    pts[4] = new PointF(startright, HighY);
                                    pts[5] = new PointF(Math.Min(Edge1_2, Edge2_2), HighY);
                                    pts[6] = new PointF(Math.Min(Edge1_2, Edge2_2), LowY);
                                    pts[7] = new PointF(endright, LowY);
                                    pts[8] = pts[7];
                                    pts[9] = pts[7];
                                    pts[10] = pts[7];
                                    pts[11] = pts[7];
                                    pts[12] = pts[7];
                                    pts[13] = pts[7];
                                    pts[14] = pts[7];
                                    pts[15] = pts[7];

                                    if ((startleft + dbcounts) < Edge1)
                                    {
                                        ptsDBph1[0] = new PointF(startleft, 0f);
                                        ptsDBph1[1] = new PointF(startleft + dbcounts, 0f);
                                        ptsDBph1[2] = new PointF(startleft + dbcounts, 1f);
                                        ptsDBph1[3] = new PointF(Math.Min(Edge1, Edge2), 1f);
                                        ptsDBph1[4] = new PointF(Math.Min(Edge1, Edge2), 0f);
                                        ptsDBph1[5] = new PointF(startright, 0f);
                                        ptsDBph1[6] = new PointF(startright, 0f);
                                        ptsDBph1[7] = new PointF(startright + dbcounts, 0f);
                                        ptsDBph1[8] = new PointF(startright + dbcounts, 1f);
                                        ptsDBph1[9] = new PointF(Math.Min(Edge1_2, Edge2_2), 1f);
                                        ptsDBph1[10] = new PointF(Math.Min(Edge1_2, Edge2_2), 0f);
                                        ptsDBph1[11] = new PointF(endright, 0f);
                                    }
                                    else
                                    {
                                        ptsDBph1[0] = new PointF(startleft, 0F);
                                        ptsDBph1[1] = new PointF(endright, 0F);
                                        ptsDBph1[2] = ptsDBph1[1];
                                        ptsDBph1[3] = ptsDBph1[1];
                                        ptsDBph1[4] = ptsDBph1[1];
                                        ptsDBph1[5] = ptsDBph1[1];
                                        ptsDBph1[6] = ptsDBph1[1];
                                        ptsDBph1[7] = ptsDBph1[1];
                                        ptsDBph1[8] = ptsDBph1[1];
                                        ptsDBph1[9] = ptsDBph1[1];
                                        ptsDBph1[10] = ptsDBph1[1];
                                        ptsDBph1[11] = ptsDBph1[1];
                                    }

                                    ptsDBph2[0] = new PointF(startleft, 0f);
                                    ptsDBph2[1] = new PointF(Math.Min(Edge1, Edge2) + dbcounts, 0f);
                                    ptsDBph2[2] = new PointF(Math.Min(Edge1, Edge2) + dbcounts, 1f);
                                    ptsDBph2[3] = new PointF(startright, 1f);
                                    ptsDBph2[4] = new PointF(startright, 0f);
                                    ptsDBph2[5] = new PointF(Math.Min(Edge1_2, Edge2_2) + dbcounts, 0f);
                                    ptsDBph2[6] = new PointF(Math.Min(Edge1_2, Edge2_2) + dbcounts, 1f);
                                    ptsDBph2[7] = new PointF(endright, 1f);
                                    ptsDBph2[8] = ptsDBph2[7];
                                    ptsDBph2[9] = ptsDBph2[7];
                                    ptsDBph2[10] = ptsDBph2[7];
                                    ptsDBph2[11] = ptsDBph2[7];
                                    break;
                            }
                        }
                        #endregion
                        #region PWM-CENTER
                        if (prms.PWMMode.Expr.Contains("Center"))
                        {
                            //1 = _____|------|_____    = Greater Than
                            //2 = -----|______|-----    = Less Than

                            //Get Compare Type Information for Compare Type 1
                            if (prms.CompareType1.Expr.Contains("Less"))
                            {
                                if (prms.CompareType1.Expr.Contains("Equal"))
                                {
                                    comparestring1 = "<=" + prms.CompareValue1.Value;
                                    StartHigh = true;
                                }
                                else
                                {
                                    comparestring1 = "<" + prms.CompareValue1.Value;
                                    StartHigh = true;
                                }
                            }
                            else if (prms.CompareType1.Expr.Contains("Greater"))
                            {
                                if (prms.CompareType1.Expr.Contains("Equal"))
                                {
                                    comparestring1 = ">=" + prms.CompareValue1.Value;
                                    StartHigh = false;
                                }
                                else
                                {
                                    comparestring1 = ">" + prms.CompareValue1.Value;
                                    StartHigh = false;
                                }
                            }
                            else
                            {
                                comparestring1 = "<" + prms.CompareValue1.Value;
                                StartHigh = true;
                            }

                            //Offset 1 = 0 location + compare * bitwidth
                            //offset 2 = end location minus compare * bitwidth
                            Edge1 = startleft + (bitwidth * (float)(Convert.ToInt32(prms.CompareValue1.Value)));
                            Edge1_2 = startright + (bitwidth * (float)(Convert.ToInt32(prms.CompareValue1.Value)));
                            Edge2 = startright - (bitwidth * (float)(Convert.ToInt32(prms.CompareValue1.Value)));
                            Edge2_2 = endright - (bitwidth * (float)(Convert.ToInt32(prms.CompareValue1.Value)));
                            pts[0] = new PointF(startleft, StartHigh ? HighY : LowY);
                            pts[1] = new PointF(Edge1, StartHigh ? HighY : LowY);
                            pts[2] = new PointF(Edge1, StartHigh ? LowY : HighY);
                            pts[3] = new PointF(Edge2, StartHigh ? LowY : HighY);
                            pts[4] = new PointF(Edge2, StartHigh ? HighY : LowY);
                            pts[5] = new PointF(startright, StartHigh ? HighY : LowY);
                            pts[6] = new PointF(startright, StartHigh ? HighY : LowY);
                            pts[7] = new PointF(Edge1_2, StartHigh ? HighY : LowY);
                            pts[8] = new PointF(Edge1_2, StartHigh ? LowY : HighY);
                            pts[9] = new PointF(Edge2_2, StartHigh ? LowY : HighY);
                            pts[10] = new PointF(Edge2_2, StartHigh ? HighY : LowY);
                            pts[11] = new PointF(endright, StartHigh ? HighY : LowY);
                            pts[12] = pts[11];
                            pts[13] = pts[11];
                            pts[14] = pts[11];
                            pts[15] = pts[11];

                            if (StartHigh)
                            {
                                ptsDBph1[0] = new PointF(startleft, 1f);
                                ptsDBph1[1] = new PointF(Edge1, 1f);
                                ptsDBph1[2] = new PointF(Edge1, 0f);
                                ptsDBph1[3] = new PointF(Edge2 + dbcounts, 0f);
                                ptsDBph1[4] = new PointF(Edge2 + dbcounts, 1f);
                                ptsDBph1[5] = new PointF(startright, 1f);
                                ptsDBph1[6] = new PointF(startright, 1f);
                                ptsDBph1[7] = new PointF(Edge1_2, 1f);
                                ptsDBph1[8] = new PointF(Edge1_2, 0f);
                                ptsDBph1[9] = new PointF(Edge2_2 + dbcounts, 0f);
                                ptsDBph1[10] = new PointF(Edge2_2 + dbcounts, 1f);
                                ptsDBph1[11] = new PointF(endright, 1f);

                                ptsDBph2[0] = new PointF(startleft, 0f);
                                ptsDBph2[1] = new PointF(Edge1 + dbcounts, 0f);
                                ptsDBph2[2] = new PointF(Edge1 + dbcounts, 1f);
                                ptsDBph2[3] = new PointF(Edge2, 1f);
                                ptsDBph2[4] = new PointF(Edge2, 0f);
                                ptsDBph2[5] = new PointF(startright, 0f);
                                ptsDBph2[6] = new PointF(startright, 0f);
                                ptsDBph2[7] = new PointF(Edge1_2 + dbcounts, 0f);
                                ptsDBph2[8] = new PointF(Edge1_2 + dbcounts, 1f);
                                ptsDBph2[9] = new PointF(Edge2_2, 1f);
                                ptsDBph2[10] = new PointF(Edge2_2, 0f);
                                ptsDBph2[11] = new PointF(endright, 0f);
                            }
                            else
                            {
                                ptsDBph1[0] = new PointF(startleft, 0f);
                                ptsDBph1[1] = new PointF(Edge1 + dbcounts, 0f);
                                ptsDBph1[2] = new PointF(Edge1 + dbcounts, 1f);
                                ptsDBph1[3] = new PointF(Edge2, 1f);
                                ptsDBph1[4] = new PointF(Edge2, 0f);
                                ptsDBph1[5] = new PointF(startright, 0f);
                                ptsDBph1[6] = new PointF(startright, 0f);
                                ptsDBph1[7] = new PointF(Edge1_2 + dbcounts, 0f);
                                ptsDBph1[8] = new PointF(Edge1_2 + dbcounts, 1f);
                                ptsDBph1[9] = new PointF(Edge2_2, 1f);
                                ptsDBph1[10] = new PointF(Edge2_2, 0f);
                                ptsDBph1[11] = new PointF(endright, 0f);

                                ptsDBph2[0] = new PointF(startleft, 1f);
                                ptsDBph2[1] = new PointF(Edge1, 1f);
                                ptsDBph2[2] = new PointF(Edge1, 0f);
                                ptsDBph2[3] = new PointF(Edge2 + dbcounts, 0f);
                                ptsDBph2[4] = new PointF(Edge2 + dbcounts, 1f);
                                ptsDBph2[5] = new PointF(startright, 1f);
                                ptsDBph2[6] = new PointF(startright, 1f);
                                ptsDBph2[7] = new PointF(Edge1_2, 1f);
                                ptsDBph2[8] = new PointF(Edge1_2, 0f);
                                ptsDBph2[9] = new PointF(Edge2_2 + dbcounts, 0f);
                                ptsDBph2[10] = new PointF(Edge2_2 + dbcounts, 1f);
                                ptsDBph2[11] = new PointF(endright, 1f);
                            }
                        }
                        #endregion
                        #region PWM-HARDWARE
                        if (prms.PWMMode.Expr.Contains("Hardware"))
                        {
                            Edge1 = (2 * PB_EXTENTS_BORDER) + PB_PWMTEXT_WIDTH;
                            comparestring1 = "If cmp_sel == 1 pwm == pwm1 else pwm == pwm2";
                            pts[0] = new PointF(0f, 0f);
                            pts[1] = new PointF(0f, 0f);
                            pts[2] = new PointF(0f, 0f);
                            pts[3] = new PointF(0f, 0f);
                            pts[4] = new PointF(0f, 0f);
                            pts[5] = new PointF(0f, 0f);
                            pts[6] = new PointF(0f, 0f);
                            pts[7] = new PointF(0f, 0f);
                            pts[8] = pts[7];
                            pts[9] = pts[7];
                            pts[10] = pts[7];
                            pts[11] = pts[7];
                            pts[12] = pts[7];
                            pts[13] = pts[7];
                            pts[14] = pts[7];
                            pts[15] = pts[7];

                            ptsDBph1[0] = new PointF(0f, 0f);
                            ptsDBph1[1] = new PointF(0f, 0f);
                            ptsDBph1[2] = new PointF(0f, 0f);
                            ptsDBph1[3] = new PointF(0f, 0f);
                            ptsDBph1[4] = new PointF(0f, 0f);
                            ptsDBph1[5] = new PointF(0f, 0f);
                            ptsDBph1[6] = new PointF(0f, 0f);
                            ptsDBph1[7] = new PointF(0f, 0f);
                            ptsDBph1[8] = new PointF(0f, 0f);
                            ptsDBph1[9] = new PointF(0f, 0f);
                            ptsDBph1[10] = new PointF(0f, 0f);
                            ptsDBph1[11] = new PointF(0f, 0f);

                            ptsDBph2[0] = new PointF(0f, 0f);
                            ptsDBph2[1] = new PointF(0f, 0f);
                            ptsDBph2[2] = new PointF(0f, 0f);
                            ptsDBph2[3] = new PointF(0f, 0f);
                            ptsDBph2[4] = new PointF(0f, 0f);
                            ptsDBph2[5] = new PointF(0f, 0f);
                            ptsDBph2[6] = new PointF(0f, 0f);
                            ptsDBph2[7] = new PointF(0f, 0f);
                            ptsDBph2[8] = new PointF(0f, 0f);
                            ptsDBph2[9] = new PointF(0f, 0f);
                            ptsDBph2[10] = new PointF(0f, 0f);
                            ptsDBph2[11] = new PointF(0f, 0f);
                        }
                        #endregion
                        #region PWM-DITHER
                        if (prms.PWMMode.Expr.Contains("Dither"))
                        {
                            Edge1 = startleft + (bitwidth * (float)(Convert.ToInt32(prms.Period.Value) - Convert.ToInt32(prms.CompareValue1.Value) + 1));
                            Edge1_2 = startright + (bitwidth * (float)(Convert.ToInt32(prms.Period.Value) - Convert.ToInt32(prms.CompareValue1.Value) + 1));
                            Edge2 = Edge1 - bitwidth;
                            Edge2_2 = Edge1_2 - bitwidth;
                            StartHigh = true;
                            string tmp = ".0";
                            switch (prms.DitherOffset.Value)
                            {
                                case "0": tmp = ".00"; break;
                                case "1": tmp = ".25"; break;
                                case "2": tmp = ".50"; break;
                                case "3": tmp = ".75"; break;
                            }

                            if (Convert.ToInt32(prms.CompareType1.Value) > 3) /* Left Aligned */
                            {
                                StartHigh = true;
                                comparestring1 = "> " + prms.CompareValue1.Value + tmp;
                            }
                            else
                            {
                                StartHigh = false;
                                comparestring1 = "< " + prms.CompareValue1.Value + tmp;
                            }
                            pts[0] = new PointF(startleft, StartHigh ? HighY : LowY);
                            pts[1] = new PointF(Edge1, StartHigh ? HighY : LowY);
                            pts[2] = new PointF(Edge1, StartHigh ? LowY : HighY);
                            pts[3] = new PointF(Edge2, StartHigh ? LowY : HighY);
                            pts[4] = new PointF(Edge2, StartHigh ? HighY : LowY);
                            pts[5] = new PointF(Edge1, StartHigh ? HighY : LowY);
                            pts[6] = new PointF(Edge1, StartHigh ? LowY : HighY);
                            pts[7] = new PointF(startright, StartHigh ? LowY : HighY);
                            pts[8] = new PointF(startright, StartHigh ? HighY : LowY);
                            pts[9] = new PointF(Edge1_2, StartHigh ? HighY : LowY);
                            pts[10] = new PointF(Edge1_2, StartHigh ? LowY : HighY);
                            pts[11] = new PointF(Edge2_2, StartHigh ? LowY : HighY);
                            pts[12] = new PointF(Edge2_2, StartHigh ? HighY : LowY);
                            pts[13] = new PointF(Edge1_2, StartHigh ? HighY : LowY);
                            pts[14] = new PointF(Edge1_2, StartHigh ? LowY : HighY);
                            pts[15] = new PointF(endright, StartHigh ? LowY : HighY);

                            PointF[] pts2 = new PointF[4];
                            pts2[0] = pts[1];
                            pts2[1] = pts[2];
                            pts2[2] = pts[3];
                            pts2[3] = pts[4];
                            wfg.FillPolygon(new SolidBrush(Color.FromArgb(120, Color.Red)), pts2);
                            pts2[0] = pts[9];
                            pts2[1] = pts[10];
                            pts2[2] = pts[11];
                            pts2[3] = pts[12];
                            wfg.FillPolygon(new SolidBrush(Color.FromArgb(120, Color.Red)), pts2);
                            if (StartHigh)
                            {
                                if ((startleft + dbcounts) < Edge1)
                                {
                                    ptsDBph1[0] = new PointF(startleft, 0f);
                                    ptsDBph1[1] = new PointF(startleft + dbcounts, 0f);
                                    ptsDBph1[2] = new PointF(startleft + dbcounts, 1f);
                                    ptsDBph1[3] = new PointF(Edge1, 1f);
                                    ptsDBph1[4] = new PointF(Edge1, 0f);
                                    ptsDBph1[5] = new PointF(startright, 0f);
                                    ptsDBph1[6] = new PointF(startright, 0f);
                                    ptsDBph1[7] = new PointF(startright + dbcounts, 0f);
                                    ptsDBph1[8] = new PointF(startright + dbcounts, 1f);
                                    ptsDBph1[9] = new PointF(Edge1_2, 1f);
                                    ptsDBph1[10] = new PointF(Edge1_2, 0f);
                                    ptsDBph1[11] = new PointF(endright, 0f);
                                }
                                else
                                {
                                    ptsDBph1[0] = new PointF(startleft, 0f);
                                    ptsDBph1[1] = new PointF(endright, 0f);
                                    ptsDBph1[2] = ptsDBph1[1];
                                    ptsDBph1[3] = ptsDBph1[1];
                                    ptsDBph1[4] = ptsDBph1[1];
                                    ptsDBph1[5] = ptsDBph1[1];
                                    ptsDBph1[6] = ptsDBph1[1];
                                    ptsDBph1[7] = ptsDBph1[1];
                                    ptsDBph1[8] = ptsDBph1[1];
                                    ptsDBph1[9] = ptsDBph1[1];
                                    ptsDBph1[10] = ptsDBph1[1];
                                    ptsDBph1[11] = ptsDBph1[1];
                                }

                                if ((Edge1 + dbcounts) < startright)
                                {
                                    ptsDBph2[0] = new PointF(startleft, 0f);
                                    ptsDBph2[1] = new PointF(Edge1 + dbcounts, 0f);
                                    ptsDBph2[2] = new PointF(Edge1 + dbcounts, 1f);
                                    ptsDBph2[3] = new PointF(startright, 1f);
                                    ptsDBph2[4] = new PointF(startright, 0f);
                                    ptsDBph2[5] = new PointF(Edge1_2 + dbcounts, 0f);
                                    ptsDBph2[6] = new PointF(Edge1_2 + dbcounts, 1f);
                                    ptsDBph2[7] = new PointF(endright, 1f);
                                    ptsDBph2[8] = new PointF(endright, 1f);
                                    ptsDBph2[9] = new PointF(endright, 1f);
                                    ptsDBph2[10] = new PointF(endright, 1f);
                                    ptsDBph2[11] = new PointF(endright, 1f);
                                }
                                else
                                {
                                    ptsDBph2[0] = new PointF(startleft, 0f);
                                    ptsDBph2[1] = new PointF(endright, 0f);
                                    ptsDBph2[2] = ptsDBph1[1];
                                    ptsDBph2[3] = ptsDBph1[1];
                                    ptsDBph2[4] = ptsDBph1[1];
                                    ptsDBph2[5] = ptsDBph1[1];
                                    ptsDBph2[6] = ptsDBph1[1];
                                    ptsDBph2[7] = ptsDBph1[1];
                                    ptsDBph2[8] = ptsDBph1[1];
                                    ptsDBph2[9] = ptsDBph1[1];
                                    ptsDBph2[10] = ptsDBph1[1];
                                    ptsDBph2[11] = ptsDBph1[1];
                                }
                            }
                            else
                            {
                                if ((Edge1 + dbcounts) > startright)
                                {
                                    ptsDBph1[0] = new PointF(startleft, 0f);
                                    ptsDBph1[1] = new PointF(endright, 0f);
                                    ptsDBph1[2] = new PointF(endright, 0f);
                                    ptsDBph1[3] = new PointF(endright, 0f);
                                    ptsDBph1[4] = new PointF(endright, 0f);
                                    ptsDBph1[5] = new PointF(endright, 0f);
                                    ptsDBph1[6] = new PointF(endright, 0f);
                                    ptsDBph1[7] = new PointF(endright, 0f);
                                    ptsDBph1[8] = new PointF(endright, 0f);
                                    ptsDBph1[9] = new PointF(endright, 0f);
                                    ptsDBph1[10] = new PointF(endright, 0f);
                                    ptsDBph1[11] = new PointF(endright, 0f);
                                }
                                else
                                {
                                    ptsDBph1[0] = new PointF(startleft, 0f);
                                    ptsDBph1[1] = new PointF(Edge1 + dbcounts, 0f);
                                    ptsDBph1[2] = new PointF(Edge1 + dbcounts, 1f);
                                    ptsDBph1[3] = new PointF(startright, 1f);
                                    ptsDBph1[4] = new PointF(startright, 0f);
                                    ptsDBph1[5] = new PointF(Edge1_2 + dbcounts, 0f);
                                    ptsDBph1[6] = new PointF(Edge1_2 + dbcounts, 1f);
                                    ptsDBph1[7] = new PointF(endright, 1f);
                                    ptsDBph1[8] = new PointF(endright, 1f);
                                    ptsDBph1[9] = new PointF(endright, 1f);
                                    ptsDBph1[10] = new PointF(endright, 1f);
                                    ptsDBph1[11] = new PointF(endright, 1f);

                                }

                                if ((startleft + dbcounts) < Edge1)
                                {
                                    ptsDBph2[0] = new PointF(startleft, 0f);
                                    ptsDBph2[1] = new PointF(startleft + dbcounts, 0f);
                                    ptsDBph2[2] = new PointF(startleft + dbcounts, 1f);
                                    ptsDBph2[3] = new PointF(Edge1, 1f);
                                    ptsDBph2[4] = new PointF(Edge1, 0f);
                                    ptsDBph2[5] = new PointF(startright, 0f);
                                    ptsDBph2[6] = new PointF(startright, 0f);
                                    ptsDBph2[7] = new PointF(startright + dbcounts, 0f);
                                    ptsDBph2[8] = new PointF(startright + dbcounts, 1f);
                                    ptsDBph2[9] = new PointF(Edge1_2, 1f);
                                    ptsDBph2[10] = new PointF(Edge1_2, 0f);
                                    ptsDBph2[11] = new PointF(endright, 0f);
                                }
                                else
                                {
                                    ptsDBph2[0] = new PointF(startleft, 0f);
                                    ptsDBph2[1] = new PointF(endright, 0f);
                                    ptsDBph2[2] = new PointF(endright, 0f);
                                    ptsDBph2[3] = new PointF(endright, 0f);
                                    ptsDBph2[4] = new PointF(endright, 0f);
                                    ptsDBph2[5] = new PointF(endright, 0f);
                                    ptsDBph2[6] = new PointF(endright, 0f);
                                    ptsDBph2[7] = new PointF(endright, 0f);
                                    ptsDBph2[8] = new PointF(endright, 0f);
                                    ptsDBph2[9] = new PointF(endright, 0f);
                                    ptsDBph2[10] = new PointF(endright, 0f);
                                    ptsDBph2[11] = new PointF(endright, 0f);
                                }
                            }
                        }
                        break;
                        #endregion
                    #endregion
                    #region PH1
                    case "ph1":
                        int j = 0;
                        foreach (PointF p in ptsDBph1)
                        {
                            if (p.Y == 0f)
                                pts[j++] = new PointF(p.X, LowY);
                            else
                                pts[j++] = new PointF(p.X, HighY);
                        }
                        pts[12] = pts[11];
                        pts[13] = pts[11];
                        pts[14] = pts[11];
                        pts[15] = pts[11];
                        break;
                    #endregion
                    #region PH2
                    case "ph2":
                        int k = 0;
                        foreach (PointF p in ptsDBph2)
                        {
                            if (p.Y == 0f)
                                pts[k++] = new PointF(p.X, LowY);
                            else
                                pts[k++] = new PointF(p.X, HighY);
                        }
                        pts[12] = pts[11];
                        pts[13] = pts[11];
                        pts[14] = pts[11];
                        pts[15] = pts[11];
                        break;
                    #endregion
                }
                //Draw the Waveform
                SolidBrush wfbrush = new SolidBrush(Color.Blue);
                Pen wfPen = new Pen(wfbrush);
                wfg.DrawLines(wfPen, pts);

                //if (comparestring1 != null)
                //{
                //    SolidBrush wfHglghtbrush = new SolidBrush(Color.Green);
                //    Pen wfHighlightPen = new Pen(wfHglghtbrush);
                //    wfg.DrawLine(wfHighlightPen, new PointF(Edge1, CenterY), new PointF(Edge1 + (3 * PB_POLYGON_WIDTH), CenterY));
                //    wfg.DrawString(comparestring1, perfont, wfHglghtbrush, new PointF(Edge1 + (3 * PB_POLYGON_WIDTH), CenterY - ((wfg.MeasureString(comparestring1, perfont).Height) / 2)));
                //    PointF[] plypts = new PointF[3];
                //    plypts[0] = new PointF(Edge1, CenterY);
                //    plypts[1] = new PointF(Edge1 + PB_POLYGON_WIDTH, CenterY + PB_POLYGON_WIDTH);
                //    plypts[2] = new PointF(Edge1 + PB_POLYGON_WIDTH, CenterY - PB_POLYGON_WIDTH);
                //    wfg.FillPolygon(wfHglghtbrush, plypts);
                //}

            }
            wfg.Dispose();
            m_pbDrawing.Image = waveform;
        }

        private void m_numPeriod_ValueChanged(object sender, EventArgs e)
        {
            SetAParameter("Period", m_numPeriod.Value.ToString(), false);

            UpdateDrawing(new PWMParameters(m_Component));
            m_numCompare1.Maximum = m_numPeriod.Value;
            m_numCompare2.Maximum = m_numPeriod.Value;
            if (m_numPeriod.Value - m_numCompare1.Value < m_numDeadBandCounts.Value)
            {
                if ((m_numPeriod.Value - m_numCompare1.Value) >= m_numDeadBandCounts.Minimum)
                    m_numDeadBandCounts.Maximum = m_numPeriod.Value - m_numCompare1.Value;
                else
                    m_numDeadBandCounts.Maximum = m_numDeadBandCounts.Minimum;
            }
            else
            {
                if (m_cbDeadBandMode.Text.Contains("4"))
                {
                    if (m_numPeriod.Value - m_numCompare1.Value < m_numDeadBandCounts.Value)
                        m_numDeadBandCounts.Maximum = m_numPeriod.Value - m_numCompare1.Value;
                    else
                        m_numDeadBandCounts.Maximum = 4;
                }
                else
                {
                    if (m_numPeriod.Value - m_numCompare1.Value < m_numDeadBandCounts.Value)
                        m_numDeadBandCounts.Maximum = m_numPeriod.Value - m_numCompare1.Value;
                    else
                        m_numDeadBandCounts.Maximum = 256;
                }
            }
            SetFrequencyLabel(Convert.ToUInt16(m_numPeriod.Value));
        }

        private void CyPWMControl_SizeChanged(object sender, EventArgs e)
        {
            UpdateDrawing(new PWMParameters(m_Component));
        }

        private void m_cbPWMMode_SelectedIndexChanged(object sender, EventArgs e)
        {
            string prm = m_Component.ResolveEnumDisplayToId("PWMMode", m_cbPWMMode.Text);
            SetAParameter("PWMMode", prm, true);

            if (m_cbPWMMode.Text.Contains("Two") || m_cbPWMMode.Text.Contains("Dual") || m_cbPWMMode.Text.Contains("Hardware"))
            {
                m_numCompare2.Visible = true;
                m_lblCmpValue2.Visible = true;
                m_lblCmpType2.Visible = true;
                m_cbCompareType2.Visible = true;
                m_cbDitherOffset.Visible = false;
                m_cbDitherAlign.Visible = false;
                m_cbCompareType1.Visible = true;
                m_lblCmpType1.Text = "CMP Type 1:";
            }
            else if (m_cbPWMMode.Text.Contains("One") || m_cbPWMMode.Text.Contains("Center"))
            {
                if (m_cbPWMMode.Text.Contains("Center"))
                {
                    if (m_rbResolution8.Checked)
                    {
                        m_numCompare1.Maximum = 254;
                        m_numPeriod.Maximum = 254;
                    }
                    else
                    {
                        m_numCompare1.Maximum = 65534;
                        m_numPeriod.Maximum = 65534;
                    }
                }
                m_numCompare2.Visible = false;
                m_lblCmpValue2.Visible = false;
                m_lblCmpType2.Visible = false;
                m_cbCompareType2.Visible = false;
                m_cbDitherOffset.Visible = false;
                m_cbDitherAlign.Visible = false;
                m_cbCompareType1.Visible = true;
                m_lblCmpType1.Text = "CMP Type 1:";
            }
            else if (m_cbPWMMode.Text.Contains("Dither"))
            {
                m_numCompare2.Visible = false;
                m_lblCmpValue2.Visible = false;
                m_lblCmpType2.Visible = false;
                m_cbCompareType2.Visible = false;
                m_cbDitherOffset.Visible = true;
                m_cbDitherAlign.Visible = true;
                m_cbCompareType1.Visible = false;
                m_lblCmpType1.Text = "Alignment:";
                SetAParameter("CompareValue2", Convert.ToString(m_numCompare1.Value + 1), false);
                m_numCompare2.Value = m_numCompare1.Value + 1;
            }

            UpdateDrawing(new PWMParameters(m_Component));
        }

        private void m_numCompare1_ValueChanged(object sender, EventArgs e)
        {
            SetAParameter("CompareValue1", m_numCompare1.Value.ToString(), false);
            UpdateDrawing(new PWMParameters(m_Component));
        }

        private void m_numCompare2_ValueChanged(object sender, EventArgs e)
        {
            SetAParameter("CompareValue2", m_numCompare2.Value.ToString(), false);
            UpdateDrawing(new PWMParameters(m_Component));
        }

        private void m_cbCompareType1_SelectedIndexChanged(object sender, EventArgs e)
        {
            string prm = m_Component.ResolveEnumDisplayToId("CompareType1", m_cbCompareType1.Text);
            SetAParameter("CompareType1", prm, true);
            UpdateDrawing(new PWMParameters(m_Component));
        }

        private void m_cbCompareType2_SelectedIndexChanged(object sender, EventArgs e)
        {
            string prm = m_Component.ResolveEnumDisplayToId("CompareType2", m_cbCompareType2.Text);
            SetAParameter("CompareType2", prm, true);
            UpdateDrawing(new PWMParameters(m_Component));
        }

        private void m_rbResolution8_Click(object sender, EventArgs e)
        {
            if (!m_rbResolution8.Checked)
            {
                m_rbResolution8.Checked = true;
                m_rbResolution16.Checked = false;
                if (m_cbPWMMode.Text.Contains("Center"))
                {
                    if (m_numCompare1.Maximum != 254)
                        m_numCompare1.Maximum = 254;
                    if (m_numPeriod.Maximum != 254)
                        m_numPeriod.Maximum = 254;
                }
                else
                {
                    if (m_numCompare1.Maximum != 255)
                        m_numCompare1.Maximum = 255;
                    if (m_numPeriod.Maximum != 255)
                        m_numPeriod.Maximum = 255;
                }
                if (m_numCompare2.Maximum != 255)
                    m_numCompare2.Maximum = 255;
                SetAParameter("Resolution", "8", true); //TODO: Use Enumerate Type
                UpdateDrawing(new PWMParameters(m_Component));
            }
        }

        private void m_rbResolution16_Click(object sender, EventArgs e)
        {
            if (!m_rbResolution16.Checked)
            {
                m_rbResolution8.Checked = false;
                m_rbResolution16.Checked = true;
                if (m_cbPWMMode.Text.Contains("Center"))
                {
                    if (m_numCompare1.Maximum != 65534)
                        m_numCompare1.Maximum = 65534;
                    if (m_numPeriod.Maximum != 65534)
                        m_numPeriod.Maximum = 65534;
                }
                else
                {
                    if (m_numCompare1.Maximum != 65535)
                        m_numCompare1.Maximum = 65535;
                    if (m_numPeriod.Maximum != 65535)
                        m_numPeriod.Maximum = 65535;
                }
                if (m_numCompare2.Maximum != 65535)
                    m_numCompare2.Maximum = 65535;
                SetAParameter("Resolution", "16", true); //TODO: Use Enumerate Type
                UpdateDrawing(new PWMParameters(m_Component));
            }
        }

        private void m_cbDitherAlign_SelectedIndexChanged(object sender, EventArgs e)
        {
            PWMParameters prms = new PWMParameters(m_Component);
            if (prms.PWMMode.Expr.Contains("Dither"))
            {
                //Get the Enumerated strings for LessThanOrEqual an GreaterThanOrEqual
                IEnumerable<string> CmpTypeEnums = m_Component.GetPossibleEnumValues("CompareType1");
                string LessThan = null;
                string GreaterThan = null;
                foreach (string str in CmpTypeEnums)
                {
                    if (str.Contains("Equal") && str.Contains("Less"))
                        LessThan = str;
                    if (str.Contains("Equal") && str.Contains("Greater"))
                        GreaterThan = str;
                }
                if (m_cbDitherAlign.SelectedIndex == 0) /*Left Aligned*/
                {
                    SetAParameter("CompareType1", m_Component.ResolveEnumDisplayToId("CompareType1", GreaterThan), false);
                    SetAParameter("CompareType2", m_Component.ResolveEnumDisplayToId("CompareType1", GreaterThan), false);
                }
                else /* Right Aligned*/
                {
                    SetAParameter("CompareType1", m_Component.ResolveEnumDisplayToId("CompareType2", LessThan), false);
                    SetAParameter("CompareType2", m_Component.ResolveEnumDisplayToId("CompareType2", LessThan), false);
                }
                UpdateDrawing(new PWMParameters(m_Component));
            }
        }

        private void m_cbDitherOffset_SelectedIndexChanged(object sender, EventArgs e)
        {
            string prm = m_Component.ResolveEnumDisplayToId("DitherOffset", m_cbDitherOffset.Text);
            SetAParameter("DitherOffset", prm, false);
            UpdateDrawing(new PWMParameters(m_Component));
        }

        private void SetFixedFunction()
        {
            //Get the Enumerated strings for PWMMode One Output an DeadBandDisabled
            IEnumerable<string> PWMModeEnums = m_Component.GetPossibleEnumValues("PWMMode");
            string PWMOneOutput = null;
            foreach (string str in PWMModeEnums)
            {
                if (str.Contains("One"))
                    PWMOneOutput = str;
            }
            IEnumerable<string> DeadBandEnums = m_Component.GetPossibleEnumValues("DeadBand");
            string DeadBandFF = null;
            foreach (string str in DeadBandEnums)
            {
                if (str.Contains("Disable"))
                    DeadBandFF = str;
            }

            //Hide all of the fixed function block limitations
            m_cbPWMMode.SelectedIndex = 0;
            m_cbPWMMode.Enabled = false;
            SetAParameter("PWMMode", PWMOneOutput, true);
            if (m_cbDeadBandMode.SelectedIndex == 0)
            {
                SetAParameter("DeadBand", DeadBandFF, true);
                m_cbFFDeadBandMode.SelectedIndex = 0;
            }
            m_cbDeadBandMode.Enabled = false;
            m_cbDeadBandMode.Visible = false;
            m_cbFFDeadBandMode.Visible = true;
            m_cbDeadBandMode.Visible = false;
            m_numDeadBandCounts.Minimum = 0;
            m_numDeadBandCounts.Maximum = 3;
            m_numDeadBandCounts.Enabled = true;
        }

        private void ClearFixedFunction()
        {
            m_cbPWMMode.Enabled = true;
            m_cbDeadBandMode.Enabled = true;
            m_cbFFDeadBandMode.Visible = false;
            m_cbDeadBandMode.Visible = true;
            if (m_cbDeadBandMode.Text.Contains("Disable"))
                m_numDeadBandCounts.Enabled = false;
            else
                m_numDeadBandCounts.Enabled = true;
        }

        private void m_cbDeadBandMode_SelectedIndexChanged(object sender, EventArgs e)
        {
            string prm = m_Component.ResolveEnumDisplayToId("DeadBand", m_cbDeadBandMode.Text);
            SetAParameter("DeadBand", prm, true);

            UpdateDrawing(new PWMParameters(m_Component));
            if (m_cbDeadBandMode.Text.Contains("Disable"))
            {
                m_numDeadBandCounts.Enabled = false;
            }
            else if (m_cbDeadBandMode.Text.Contains("4"))
            {
                m_numDeadBandCounts.Enabled = true;
                if (m_numPeriod.Value - m_numCompare1.Value < m_numDeadBandCounts.Value)
                    m_numDeadBandCounts.Maximum = m_numPeriod.Value - m_numCompare1.Value;
                else
                    m_numDeadBandCounts.Maximum = 4;
                m_numDeadBandCounts.Minimum = 1;
            }
            else
            {
                m_numDeadBandCounts.Enabled = true;
                if (m_numPeriod.Value - m_numCompare1.Value < m_numDeadBandCounts.Value)
                    m_numDeadBandCounts.Maximum = m_numPeriod.Value - m_numCompare1.Value;
                else
                    m_numDeadBandCounts.Maximum = 256;
                m_numDeadBandCounts.Minimum = 1;
            }
        }

        private void m_numDeadBandCounts_ValueChanged(object sender, EventArgs e)
        {
            decimal newval = m_numPeriod.Value - m_numCompare1.Value;
            if (m_cbDeadBandMode.Enabled)
            {
                if (m_cbDeadBandMode.Text.Contains("Disable"))
                    return;
                if (m_cbDeadBandMode.Text.Contains("4"))    /* If using 1-4 DB Counts mode */
                {
                    if (newval < m_numDeadBandCounts.Value)
                    {
                        if (newval >= m_numDeadBandCounts.Minimum)
                        {
                            if (m_numDeadBandCounts.Maximum != newval)
                                m_numDeadBandCounts.Maximum = newval;
                        }
                        else
                        {
                            m_numDeadBandCounts.Maximum = m_numDeadBandCounts.Minimum;
                        }
                    }
                    else
                    {
                        if (m_numDeadBandCounts.Maximum != 4)
                            m_numDeadBandCounts.Maximum = 4;
                    }
                }
                else
                {
                    if (newval < m_numDeadBandCounts.Value)
                    {
                        if (newval < m_numDeadBandCounts.Maximum)
                        {
                            m_numDeadBandCounts.Maximum = newval;
                        }
                    }
                    else
                    {
                        if (m_numDeadBandCounts.Maximum != 256)
                            m_numDeadBandCounts.Maximum = 256;
                    }
                }
                SetAParameter("DeadTime", (m_numDeadBandCounts.Value - 1).ToString(), false);
            }
            else
                SetAParameter("DeadTime", (m_numDeadBandCounts.Value).ToString(), false);
            UpdateDrawing(new PWMParameters(m_Component));
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

        private void m_cbFFDeadBandMode_SelectedIndexChanged(object sender, EventArgs e)
        {
            IEnumerable<string> DeadBandEnums = m_Component.GetPossibleEnumValues("DeadBand");
            string DeadBandDisabled = null;
            string DeadBand24 = null;
            foreach (string str in DeadBandEnums)
            {
                if (str.Contains("Disable"))
                    DeadBandDisabled = str;
                if (str.Contains("4"))
                    DeadBand24 = str;
            }
            if (m_cbFFDeadBandMode.SelectedIndex == 0)
                SetAParameter("DeadBand", m_Component.ResolveEnumDisplayToId("DeadBand", DeadBandDisabled), true);
            else
                SetAParameter("DeadBand", m_Component.ResolveEnumDisplayToId("DeadBand", DeadBand24), true);
            UpdateDrawing(new PWMParameters(m_Component));
        }
    }

    /// <summary>
    /// Ovverride the base numeric up down so that enter key strokes aren't passed to the parent form.
    /// </summary>
    public class CyNumericUpDown : NumericUpDown
    {
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
    }
}
