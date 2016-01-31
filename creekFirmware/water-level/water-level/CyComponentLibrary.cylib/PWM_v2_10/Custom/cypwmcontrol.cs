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
using System.Diagnostics;
using System.Data;
using System.Text;
using System.Windows.Forms;
using CyDesigner.Extensions.Common;
using CyDesigner.Extensions.Gde;
using PWM_v2_10;

namespace PWM_v2_10
{
    public partial class CyPWMControl : UserControl
    {
        private const int PB_PWMTEXT_WIDTH = 40;
        private const int PB_EXTENTS_BORDER = 5;
        private const int PB_POLYGON_WIDTH = 4;
        private const int DEADBANDCOUNTS_MINIMUM = 2;
        private const int DEADBANDCOUNTS_4MAXIMUM = 4;
        private const int DEADBANDCOUNTS_256MAXIMUM = 256;
        private const int DEADBANDFFCOUNTS_MAXIMUM = 3;
        private const int DEADBANDFFCOUNTS_MINIMUM = 0;
        private const int PERIODMAX16CA = 65534;
        private const int PERIODMAX16 = 65535;
        private const int PERIODMIN = 0;
        private const int PERIODMAX8 = 255;
        private const int PERIODMAX8CA = 254;
        private const int DeadTimeDefaultValue = 1;
        private const int PeriodDefaultValue  = 255;
        private const int CompareDefaultValue1 = 127;
        private const int CompareDefaultValue2 = 67;
        private const string COMPARETYPE1 = "CompareType1";
        private const string PWMMODE = "PWMMode";
        private const string COMPARETYPE2 = "CompareType2";
        private const string DEADBAND = "DeadBand";
        private const string DITHEROFFSET = "DitherOffset";
        private const string LEFT_ALIGNED = "Left Aligned";
        private const string RIGHT_ALIGNED = "Right Aligned";
        private const string DISABLED = "Disabled";
        private const string COUNTS = "0-3 Counts";
        private const string CLOCK = "clock";
        private const string FREQUENCY_TEXT = "Period = UNKNOWN SOURCE FREQ";
        private const string S = "s";
        private const string MS = "ms";
        private const string US = "us";
        private const string NS = "ns";
        private const string PS = "ps";
        private const string PWM = "pwm";
        private const string PWM1 = "pwm1";
        private const string PWM2 = "pwm2";
        private const string PH1 = "ph1";
        private const string PH2 = "ph2";
        private const string COMPARESTRING = "If cmp_sel == 1 pwm == pwm1 else pwm == pwm2";
        private const string CMP_TYPE_1 = "CMP Type 1:";
        private const string ALIGNMENT = "Alignment:";
        private const string COMPAREVALUE2 = "CompareValue2";
        private const string COMPAREVALUE1 = "CompareValue1";
        private const string RESOLUTION = "Resolution";
        private const string EQUAL = "Equal";
        private const string LESS = "Less";
        private const string GREATER = "Greater";
        private const string DEADTIME = "DeadTime";
        private const string DISABLE = "Disable";
        private const string ONE = "One";
        private const string FIXEDFUNCTION = "FixedFunction";
        private const string One_Output_Str = "0";
        private const string Two_Outputs_Str = "1";
        private const string Dual_Edge_Str = "2";
        private const string Center_Align_Str = "3";
        private const string Dither_Str = "5";
        private const string Hardware_Select_Str = "4";
        private const string Equal_Str = "0";
        private const string Less_Str = "1";
        private const string Less_Equal_Str = "2";
        private const string Greater_Str = "3";
        private const string Greater_Equal_Str = "4";
        private const string FirmwareControl_Str = "5";
        private const int One_Output_Int = 0;
        private const int Two_Output_Int = 1;
        private const int Dual_Edge_Int = 2;
        private const int Center_Align_Int = 3;
        private const int Dither_Int = 4;
        private const int Hardware_Select_Int = 5;
        private const int Disabled = 0;
        private const int Clock1 = 1;
        private const int Clock2 = 2;
        private int value1;
        private int value2;
        private int value3;
        private int value4;
        private int value5;
        private int value6;
        public ICyInstEdit_v1 m_InstEdit = null;
        public ICyTerminalQuery_v1 m_TermQuery = null;
        public CyPWMControlAdv m_control_advanced;

        public CyPWMControl(ICyInstEdit_v1 inst, ICyTerminalQuery_v1 termquery)
        {
            m_InstEdit = inst;
            m_TermQuery = termquery;
            m_control_advanced = null;
            InitializeComponent();

            InitializeFormComponents(inst);
            UpdateFormFromParams(inst);
        }

        
        void ParentForm_FormClosing(object sender, FormClosingEventArgs e)
        {
            if (((Form)sender).DialogResult == DialogResult.Cancel)
            {
                return;
            }

            if (ep_Errors.GetError(m_numPeriod) != "")
            {
                m_numPeriod.Focus();
                e.Cancel = true;
            }

            if (ep_Errors.GetError(m_numCompare1) != "")
            {
                m_numCompare1.Focus();
                e.Cancel = true;
            }

            if (m_cbPWMMode.SelectedIndex != One_Output_Int && m_cbPWMMode.SelectedIndex !=Center_Align_Int)
            {
                if (ep_Errors.GetError(m_numCompare2) != "")
                {
                    m_numCompare2.Focus();
                    e.Cancel = true;
                }
            }

            if (ep_Errors.GetError(m_numDeadBandCounts) != "")
            {
                m_numDeadBandCounts.Focus();
                e.Cancel = true;
            }
        }

        
        #region Form Initialization
        protected void InitializeFormComponents(ICyInstEdit_v1 inst)
        {
            //Set the PWM Mode Combo Box from Enums
            IEnumerable<string> PWMModeEnums = inst.GetPossibleEnumValues(PWMMODE);
            foreach (string str in PWMModeEnums)
            {
                m_cbPWMMode.Items.Add(str);
            }

            //Set the Compare Type Combo Box from Enums
            IEnumerable<string> CmpType1Enums = inst.GetPossibleEnumValues(COMPARETYPE1);
            foreach (string str in CmpType1Enums)
            {
                m_cbCompareType1.Items.Add(str);
            }

            //Set the Compare Type Combo Box from Enums
            IEnumerable<string> CmpType2Enums = inst.GetPossibleEnumValues(COMPARETYPE2);
            foreach (string str in CmpType2Enums)
            {
                m_cbCompareType2.Items.Add(str);
            }

            //Set the Dead Band Mode Combo Box from Enums
            IEnumerable<string> DBModeEnums = inst.GetPossibleEnumValues(DEADBAND);
            foreach (string str in DBModeEnums)
            {
                m_cbDeadBandMode.Items.Add(str);
            }

            //TODO: Dither should use Enumerated Types?
            m_cbDitherAlign.Items.Add(LEFT_ALIGNED);
            m_cbDitherAlign.Items.Add(RIGHT_ALIGNED);
            

            //Set the Dither Offsets Combo Box from Enums
            IEnumerable<string> DitherOffsetsEnums = inst.GetPossibleEnumValues(DITHEROFFSET);
            foreach (string str in DitherOffsetsEnums)
            {
                m_cbDitherOffset.Items.Add(str);
            }

            m_cbFFDeadBandMode.Items.Add(DISABLED);
            m_cbFFDeadBandMode.Items.Add(COUNTS);
            m_cbFFDeadBandMode.SelectedIndex = 0;
            m_cbFFDeadBandMode.Visible = false;

            m_numPeriod.Minimum = Decimal.MinValue;
            m_numPeriod.Maximum = Decimal.MaxValue;
            m_numCompare1.Minimum = Decimal.MinValue;
            m_numCompare1.Maximum = Decimal.MaxValue;
            m_numCompare2.Minimum = Decimal.MinValue;
            m_numCompare2.Maximum = Decimal.MaxValue;
            m_numDeadBandCounts.Minimum = Decimal.MinValue;
            m_numDeadBandCounts.Maximum = Decimal.MaxValue;
        }
        #endregion

        #region Form Updating Routines

        void UnhookUpdateEvents()
        {
            //-----------------------------------------------------------------------------
            m_numPeriod.UpEvent -= new UpButtonEvent(m_numPeriod_UpEvent);
            m_numPeriod.DownEvent -= new DownButtonEvent(m_numPeriod_DownEvent);
            m_numCompare1.UpEvent -= new UpButtonEvent(m_numCompare1_UpEvent);
            m_numCompare1.DownEvent -= new DownButtonEvent(m_numCompare1_DownEvent);
            m_numCompare2.UpEvent -= new UpButtonEvent(m_numCompare2_UpEvent);
            m_numCompare2.DownEvent -= new DownButtonEvent(m_numCompare2_DownEvent);
            m_numDeadBandCounts.UpEvent -= new UpButtonEvent(m_numDeadBandCounts_UpEvent);
            m_numDeadBandCounts.DownEvent -= new DownButtonEvent(m_numDeadBandCounts_DownEvent);
            //-----------------------------------------------------------------------------
            m_cbPWMMode.SelectedIndexChanged -= m_cbPWMMode_SelectedIndexChanged;
            m_cbFFDeadBandMode.SelectedIndexChanged -= m_cbFFDeadBandMode_SelectedIndexChanged;
            m_cbDitherOffset.SelectedIndexChanged -= m_cbDitherOffset_SelectedIndexChanged;
            m_cbDitherAlign.SelectedIndexChanged -= m_cbDitherAlign_SelectedIndexChanged;
            m_cbDeadBandMode.SelectedIndexChanged -= m_cbDeadBandMode_SelectedIndexChanged;
            m_cbCompareType2.SelectedIndexChanged -= m_cbCompareType2_SelectedIndexChanged;
            m_cbCompareType1.SelectedIndexChanged -= m_cbCompareType1_SelectedIndexChanged;
            m_numCompare1.ValueChanged -= m_numCompare1_ValueChanged;
            m_numCompare2.ValueChanged -= m_numCompare2_ValueChanged;
            m_numDeadBandCounts.ValueChanged -= m_numDeadBandCounts_ValueChanged;
            m_numPeriod.ValueChanged -= m_numPeriod_ValueChanged;
            /* Radio buttons are not auto mode so they don't need to be disabled */
        }

        void HookupUpdateEvents()
        {
            //-----------------------------------------------------------------------------
            m_numPeriod.UpEvent += new UpButtonEvent(m_numPeriod_UpEvent);
            m_numPeriod.DownEvent += new DownButtonEvent(m_numPeriod_DownEvent);
            m_numCompare1.UpEvent += new UpButtonEvent(m_numCompare1_UpEvent);
            m_numCompare1.DownEvent += new DownButtonEvent(m_numCompare1_DownEvent);
            m_numCompare2.UpEvent += new UpButtonEvent(m_numCompare2_UpEvent);
            m_numCompare2.DownEvent += new DownButtonEvent(m_numCompare2_DownEvent);
            m_numDeadBandCounts.UpEvent += new UpButtonEvent(m_numDeadBandCounts_UpEvent);
            m_numDeadBandCounts.DownEvent += new DownButtonEvent(m_numDeadBandCounts_DownEvent);
            //-----------------------------------------------------------------------------
            m_cbPWMMode.SelectedIndexChanged += m_cbPWMMode_SelectedIndexChanged;
            m_cbFFDeadBandMode.SelectedIndexChanged += m_cbFFDeadBandMode_SelectedIndexChanged;
            m_cbDitherOffset.SelectedIndexChanged += m_cbDitherOffset_SelectedIndexChanged;
            m_cbDitherAlign.SelectedIndexChanged += m_cbDitherAlign_SelectedIndexChanged;
            m_cbDeadBandMode.SelectedIndexChanged += m_cbDeadBandMode_SelectedIndexChanged;
            m_cbCompareType2.SelectedIndexChanged += m_cbCompareType2_SelectedIndexChanged;
            m_cbCompareType1.SelectedIndexChanged += m_cbCompareType1_SelectedIndexChanged;
            m_numCompare1.ValueChanged += m_numCompare1_ValueChanged;
            m_numCompare2.ValueChanged += m_numCompare2_ValueChanged;
            m_numDeadBandCounts.ValueChanged += m_numDeadBandCounts_ValueChanged;
            m_numPeriod.ValueChanged += m_numPeriod_ValueChanged;
            /* Radio buttons are not auto mode so they don't need to be disabled */
        }

        public void UpdateFormFromParams(ICyInstEdit_v1 inst)
        {
            UnhookUpdateEvents();
            PWMParameters prms = new PWMParameters(inst);
            //Set the PWMMode Combo Box
            IEnumerable<string> PWMModeEnums = inst.GetPossibleEnumValues(PWMMODE);
            bool PWMModeFound = false;
            foreach (string str in PWMModeEnums)
            {
                if (!PWMModeFound)
                {
                    string paramcompare = m_InstEdit.ResolveEnumIdToDisplay(PWMMODE, prms.PWMMode.Expr);
                    if (paramcompare == str)
                    {
                        m_cbPWMMode.SelectedItem = paramcompare;
                    }
                }
            }

            PWMModeFound = true;
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
                case "8": m_rbResolution8.Checked = true; break;
                case "16": m_rbResolution16.Checked = true; break;
                default: m_rbResolution8.Checked = true; break;
            }

            //Set the Period Numeric Up/Down
            try
            {
                m_numPeriod.Value = Convert.ToInt32(prms.Period.Value);
            }

            catch (OverflowException)
            {
                string error_str = String.Format("{0} is outside the range of Int32 type "
                    + prms.Period.Value.ToString());
                ep_Errors.SetError(m_numPeriod, error_str);
            }

            catch (FormatException)
            {
                string error_str = String.Format("{0} is not in a recognizable format."
                    + prms.Period.Value.ToString());
                ep_Errors.SetError(m_numPeriod, error_str);
            }

            //Set the Capture value Numeric Up/Downs
            try
            {
                m_numCompare1.Value = Convert.ToInt32(prms.CompareValue1.Value);
            }

            catch (OverflowException)
            {
                string error_str = String.Format("{0} is outside the range of Int32 type "
                    + prms.CompareValue1.Value.ToString());
                ep_Errors.SetError(m_numCompare1, error_str);
            }

            catch (FormatException)
            {
                string error_str = String.Format("{0} is not in a recognizable format."
                    + prms.CompareValue1.Value.ToString());
                ep_Errors.SetError(m_numCompare1, error_str);
            }

            try
            {
                m_numCompare2.Value = Convert.ToInt32(prms.CompareValue2.Value);
            }

            catch (OverflowException)
            {
                string error_str = String.Format("{0} is outside the range of Int32 type "
                    + prms.CompareValue2.Value.ToString());
                ep_Errors.SetError(m_numCompare2, error_str);
            }

            catch (FormatException)
            {
                string error_str = String.Format("{0} is not in a recognizable format." 
                    + prms.CompareValue2.Value.ToString());
                ep_Errors.SetError(m_numCompare2, error_str);
            }

            //Set the Compare Type  and Dither Combo Boxes
            if (prms.CompareType1.Value == Greater_Equal_Str)
                m_cbDitherAlign.SelectedIndex = 0;
             else
                m_cbDitherAlign.SelectedIndex = 1;

            //Set the Dither Offset Combo Box
            IEnumerable<string> DitherOffsetEnums = inst.GetPossibleEnumValues(DITHEROFFSET);
            bool DithOffFound = false;
            foreach (string str in DitherOffsetEnums)
            {
                if (!DithOffFound)
                {
                    string paramcompare = m_InstEdit.ResolveEnumIdToDisplay(DITHEROFFSET, prms.DitherOffset.Expr);
                    if (paramcompare == str)
                    {
                        m_cbDitherOffset.SelectedItem = paramcompare;
                    }
                }
            }

            DithOffFound = true;
            if (!DithOffFound)
            {
                m_cbDitherOffset.Text = prms.DitherOffset.Expr;
            }

            //Set the CompareType1 Offset Combo Box
            IEnumerable<string> CT1Enums = inst.GetPossibleEnumValues(COMPARETYPE1);
            bool CT1found = false;
            foreach (string str in CT1Enums)
            {
                if (!CT1found)
                {
                    string paramcompare = m_InstEdit.ResolveEnumIdToDisplay(COMPARETYPE1, prms.CompareType1.Expr);
                    if (paramcompare == str)
                    {
                        m_cbCompareType1.SelectedItem = paramcompare;
                    }
                }
            }

            CT1found = true;
            if (!CT1found)
            {
                m_cbCompareType1.Text = prms.CompareType1.Expr;
            }

            //Set the CompareType2 Offset Combo Box
            IEnumerable<string> CT2Enums = inst.GetPossibleEnumValues(COMPARETYPE2);
            bool CT2found = false;
            foreach (string str in CT2Enums)
            {
                if (!CT2found)
                {
                    string paramcompare = m_InstEdit.ResolveEnumIdToDisplay(COMPARETYPE2, prms.CompareType2.Expr);
                    if (paramcompare == str)
                    {
                        m_cbCompareType2.SelectedItem = paramcompare;
                    }
                }
            }

            CT2found = true;
            if (!CT2found)
            {
                m_cbCompareType2.Text = prms.CompareType2.Expr;
            }

            //Set the Dead Band Mode Combo Box
            IEnumerable<string> DBModeEnums = inst.GetPossibleEnumValues(DEADBAND);
            bool DBMfound = false;
            foreach (string str in DBModeEnums)
            {
                if (!DBMfound)
                {
                    string paramcompare = m_InstEdit.ResolveEnumIdToDisplay(DEADBAND, prms.DeadBand.Expr);
                    if (paramcompare == str)
                    {
                        m_cbDeadBandMode.SelectedItem = paramcompare;
                        if (paramcompare == DISABLED)
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

            Int32 DeadTimeValue = DeadTimeDefaultValue;
            try
            {
                DeadTimeValue = Convert.ToInt32(prms.DeadTime.Value);
            }
            catch (OverflowException)
            {
                string error_str = String.Format("{0} is outside the range of Int32 type "
                    + prms.DeadTime.Value.ToString());
                ep_Errors.SetError(m_numDeadBandCounts, error_str);
            }
            catch (FormatException)
            {
                string error_str = String.Format("{0} is not in a recognizable format."
                    + prms.DeadTime.Value.ToString());
                ep_Errors.SetError(m_numDeadBandCounts, error_str);
            }

            switch (prms.FixedFunction.Value)
            {
                case "true":
                    m_rbFixedFunction.Checked = true;
                    SetFixedFunction();
                    m_numDeadBandCounts.Value = DeadTimeValue;
                    break;
                case "false":
                    m_rbUDB.Checked = true;
                    ClearFixedFunction();
                    m_numDeadBandCounts.Value = DeadTimeValue + 1;
                    break;
                default:
                    m_rbUDB.Checked = true;
                    ClearFixedFunction();
                    m_numDeadBandCounts.Value = DeadTimeValue + 1;
                    break;
            }

            UInt16 PeriodValue16 = PeriodDefaultValue;
            try
            {
                PeriodValue16 = Convert.ToUInt16(m_numPeriod.Value);
            }
            catch (OverflowException)
            {
                string error_str = String.Format("{0} is outside the range of UInt16 type "
                    + m_numPeriod.Value.ToString());
                ep_Errors.SetError(m_numPeriod, error_str);
            }
            catch (FormatException)
            {
                string error_str = String.Format("{0} is not in a recognizable format."
                    + m_numPeriod.Value.ToString());
                ep_Errors.SetError(m_numPeriod, error_str);
            }

            SetFrequencyLabel(PeriodValue16);
            UpdateDrawing(prms);
            SetControlVisibilityFromMode();
            Set_Control_Visibility_Compare_Type();
            HookupUpdateEvents();
        }

        private void SetControlVisibilityFromMode()
        {
            if (m_cbPWMMode.SelectedIndex == Dither_Int)
            {
                m_cbDitherAlign.Visible = true;
                m_cbCompareType1.Visible = false;
                m_cbDitherOffset.Visible = true;
            }
            else
            {
                m_cbDitherAlign.Visible = false;
                m_cbCompareType1.Visible = true;
                m_cbDitherOffset.Visible = false;
            }

            if (m_cbPWMMode.SelectedIndex == One_Output_Int || m_cbPWMMode.SelectedIndex == Center_Align_Int)
            {
                m_cbCompareType2.Visible = false;
                m_numCompare2.Visible = false;
                m_lblCmpType2.Visible = false;
                m_lblCmpValue2.Visible = false;
            }
            else
            {
                m_cbCompareType2.Visible = true;
                m_numCompare2.Visible = true;
                m_lblCmpType2.Visible = true;
                m_lblCmpValue2.Visible = true;
            }
            UInt16 tmp2 = PeriodDefaultValue;
            try
            {
                tmp2 = Convert.ToUInt16(m_numPeriod.Value);
            }
            catch (OverflowException)
            {
                string error_str = String.Format("{0} is outside the range of UInt16 type "
                    + m_numPeriod.Value.ToString());
                ep_Errors.SetError(m_numPeriod, error_str);
            }
            catch (FormatException)
            {
                string error_str = String.Format("{0} is not in a recognizable format."
                    + m_numPeriod.Value.ToString());
                ep_Errors.SetError(m_numPeriod, error_str);
            }
            SetFrequencyLabel(tmp2);
        }

        private void SetFrequencyLabel(int period)
        {
            List<CyClockData> clkdata = new List<CyClockData>();
            clkdata = m_TermQuery.GetClockData(CLOCK, 0);
            if (clkdata[0].IsFrequencyKnown)
            {
                double infreq = clkdata[0].Frequency;
                switch (clkdata[0].Unit)
                {
                    case CyClockUnit.kHz: infreq = infreq * 1000; break;
                    case CyClockUnit.MHz: infreq = infreq * 1000000; break;
                }
                double periodfreq = infreq / (period + 1);
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
                    case 0: time = S; break;
                    case 3: time = MS; break;
                    case 6: time = US; break;
                    case 9: time = NS; break;
                    case 12: time = PS; break;
                }
                m_lblCalcFrequency.Text = string.Format("Period = {0}{1}", Math.Round(periodtime, 3), time);
                //Set the Tooltip m_lblCalcFrequency.To
            }
            else
            {
                m_lblCalcFrequency.Text = FREQUENCY_TEXT;
            }
        }

        public void UpdateDrawing(PWMParameters prms)
        {
            #region Setup Waveform with Extents and Cushions etc
            CancelEventArgs ce = new CancelEventArgs();
            m_numPeriod_Validating(null, ce);
            if (ce.Cancel) return;
            m_numCompare1_Validating(null, ce);
            if (ce.Cancel) return;
            if (m_numCompare2.Visible)
            {
                m_numCompare2_Validating(null, ce);
                if (ce.Cancel) return;
            }
            if (m_numDeadBandCounts.Enabled)
            {
                m_numDeadBandCounts_Validating(null, ce);
                if (ce.Cancel) return;
            }
            if ((m_pbDrawing.Width == 0) || (m_pbDrawing.Height == 0))
                return;
            Image waveform = new Bitmap(m_pbDrawing.Width, m_pbDrawing.Height);
            Graphics wfg;
            wfg = Graphics.FromImage(waveform);
            wfg.Clear(Color.White);
            SolidBrush blkbrush = new SolidBrush(Color.Black);
            SolidBrush whitebrush = new SolidBrush(Color.White);
            SolidBrush greybrush = new SolidBrush(Color.LightGray);
            SolidBrush redbrush = new SolidBrush(Color.FromArgb(120, Color.Red));


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
            greybrush.Color = Color.FromArgb(100, Color.LightGray);
            wfg.FillRectangle(greybrush,
                extentsleft, PB_EXTENTS_BORDER, padding, m_pbDrawing.Height - PB_EXTENTS_BORDER);
            //Fill the Right Padding Rectangle
            wfg.FillRectangle(greybrush,
                endright, PB_EXTENTS_BORDER, padding, m_pbDrawing.Height - PB_EXTENTS_BORDER);

            if (prms.PWMMode.Value == Center_Align_Str)
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

            if (prms.PWMMode.Value == Center_Align_Str)
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
            if (prms.PWMMode.Value != Center_Align_Str)
            {

                //Draw the Period Values at the Left
                SizeF pervalsize = wfg.MeasureString(prms.Period.Value, perfont);
                RectangleF pervalrect1 = new RectangleF(startleft + (2 * PB_POLYGON_WIDTH),
                    PB_EXTENTS_BORDER + PB_POLYGON_WIDTH - (pervalsize.Height / 2), pervalsize.Width, pervalsize.Height);
                wfg.FillRectangle(whitebrush, pervalrect1);
                wfg.DrawString(prms.Period.Value, perfont, blkbrush, pervalrect1);

                RectangleF pervalrect2 = new RectangleF(startright + (2 * PB_POLYGON_WIDTH),
                    PB_EXTENTS_BORDER + PB_POLYGON_WIDTH - (pervalsize.Height / 2), pervalsize.Width, pervalsize.Height);
                wfg.FillRectangle(whitebrush, pervalrect2);
                wfg.DrawString(prms.Period.Value, perfont, blkbrush, pervalrect2);

                //Draw the zero Values at the Right
                SizeF perzerosize = wfg.MeasureString("0", perfont);
                RectangleF perzerorect1 = new RectangleF(startright - (2 * PB_POLYGON_WIDTH) - perzerosize.Width,
                    PB_EXTENTS_BORDER + PB_POLYGON_WIDTH - (perzerosize.Height / 2), perzerosize.Width,
                    perzerosize.Height);
                wfg.FillRectangle(whitebrush, perzerorect1);
                wfg.DrawString("0", perfont, blkbrush, perzerorect1);

                RectangleF perzerorect2 = new RectangleF(endright - (2 * PB_POLYGON_WIDTH) - perzerosize.Width,
                    PB_EXTENTS_BORDER + PB_POLYGON_WIDTH - (perzerosize.Height / 2), perzerosize.Width,
                    perzerosize.Height);
                wfg.FillRectangle(whitebrush, perzerorect2);
                wfg.DrawString("0", perfont, blkbrush, perzerorect2);
            }
            else
            {
                //If Center Aligned mode then count from zero up to period and then back down
                //Draw the 0 text in the three locations
                SizeF zerosize = wfg.MeasureString("0", perfont);
                RectangleF zero1rect = new RectangleF(startleft + (2 * PB_POLYGON_WIDTH),
                    PB_EXTENTS_BORDER + PB_POLYGON_WIDTH - (zerosize.Height / 2), zerosize.Width, zerosize.Height);
                wfg.FillRectangle(whitebrush, zero1rect);
                wfg.DrawString("0", perfont, blkbrush, zero1rect);

                RectangleF zero2rect = new RectangleF(startright - (2 * PB_POLYGON_WIDTH) - zerosize.Width,
                    PB_EXTENTS_BORDER + PB_POLYGON_WIDTH - (zerosize.Height / 2), zerosize.Width, zerosize.Height);
                wfg.FillRectangle(whitebrush, zero2rect);
                wfg.DrawString("0", perfont, blkbrush, zero2rect);

                RectangleF zero3rect = new RectangleF(endright - (2 * PB_POLYGON_WIDTH) - zerosize.Width,
                    PB_EXTENTS_BORDER + PB_POLYGON_WIDTH - (zerosize.Height / 2), zerosize.Width, zerosize.Height);
                wfg.FillRectangle(whitebrush, zero3rect);
                wfg.DrawString("0", perfont, blkbrush, zero3rect);



                SizeF periodsize = wfg.MeasureString(prms.Period.Value, perfont);
                RectangleF per1rect = new RectangleF(cacenter1 + (2 * PB_POLYGON_WIDTH),
                    PB_EXTENTS_BORDER + PB_POLYGON_WIDTH - (periodsize.Height / 2), periodsize.Width,
                    periodsize.Height);
                wfg.FillRectangle(whitebrush, per1rect);
                wfg.DrawString(prms.Period.Value, perfont, blkbrush, per1rect);

                RectangleF per2rect = new RectangleF(cacenter2 + (2 * PB_POLYGON_WIDTH),
                    PB_EXTENTS_BORDER + PB_POLYGON_WIDTH - (periodsize.Height / 2), periodsize.Width,
                    periodsize.Height);
                wfg.FillRectangle(whitebrush, per2rect);
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
            List<string> wfnames = new List<string>();
            if (prms.PWMMode.Value == One_Output_Str || (prms.PWMMode.Value == Center_Align_Str) ||
                (prms.PWMMode.Value == Dither_Str))
            {
                wfnames.Add(PWM);
            }
            else if (prms.PWMMode.Value == Two_Outputs_Str)
            {
                wfnames.Add(PWM1);
                wfnames.Add(PWM2);
            }
            else if (prms.PWMMode.Value == Dual_Edge_Str || (prms.PWMMode.Value == Hardware_Select_Str))
            {
                wfnames.Add(PWM1);
                wfnames.Add(PWM2);
                wfnames.Add(PWM);
            }
            else
            {
                ep_Errors.SetError(m_cbPWMMode, String.Format("Error in setting PWM Mode"));
            }

            if (prms.DeadBand.Value == "1" || prms.DeadBand.Value == "2")
            {
                wfnames.Add(PH1);
                wfnames.Add(PH2);
            }

            numwaveforms = wfnames.Count;

            //Each waveform's height is dependent upon the drawing size minus a top and bottom border 
            //and the top period waveform which is the size of two polygon widths, and an bottom ticker tape of 2 polygon widths
            float wfheight = (m_pbDrawing.Height - (2 * PB_EXTENTS_BORDER) - (4 * PB_POLYGON_WIDTH)) / numwaveforms;

            //Fill in All Waveform Names
            for (int i = 0; i < numwaveforms; i++)
            {
                PointF pt = new PointF(extentsleft - wfg.MeasureString(wfnames[i], perfont).Width - PB_EXTENTS_BORDER,
                    PB_EXTENTS_BORDER + (2 * PB_POLYGON_WIDTH) + (wfheight * i) + (wfheight / 2) - 
                    (wfg.MeasureString(wfnames[i], perfont).Height / 2));
                wfg.DrawString(wfnames[i], perfont, blkbrush, pt);
            }
            #endregion

            //Draw Waveforms
            bool StartHigh = false;
            float Edge1 = 0;
            float Edge1_2 = 0;
            float Edge2 = 0;
            float Edge2_2 = 0;
            float Edge3 = 0;
            float Edge3_2 = 0;
            float Edge4 = 0;
            float Edge4_2 = 0;
            float bitwidth = 0;
            Int32 PeriodValue32 = PeriodDefaultValue;
            try
            {
                PeriodValue32 = Convert.ToInt32(prms.Period.Value);
            }
            catch (OverflowException)
            {
                string error_str = String.Format("{0} is outside the range of UInt16 type "
                    + prms.Period.Value.ToString());
                ep_Errors.SetError(m_numPeriod, error_str);
            }
            catch (FormatException)
            {
                string error_str = String.Format("{0} is not in a recognizable format."
                    + prms.Period.Value.ToString());
                ep_Errors.SetError(m_numPeriod, error_str);
            }

            if (prms.PWMMode.Value == Center_Align_Str)
                bitwidth = ((float)(startright - startleft) / ((float)PeriodValue32)) / 2f;
            else
                bitwidth = (float)(startright - startleft) / ((float)PeriodValue32 + 1);

            Int32 DeadTimeValue32 = DeadTimeDefaultValue;
            try
            {
                DeadTimeValue32 = Convert.ToInt32(prms.DeadTime.Value);
            }
            catch (OverflowException)
            {
                string error_str = String.Format("{0} is outside the range of Int32 type "
                    + prms.DeadTime.Value.ToString());
                ep_Errors.SetError(m_numDeadBandCounts, error_str);
            }
            catch (FormatException)
            {
                string error_str = String.Format("{0} is not in a recognizable format."
                    + prms.DeadTime.Value.ToString());
                ep_Errors.SetError(m_numDeadBandCounts, error_str);
            }

            Int32 CompareValue32 = CompareDefaultValue1;
            try
            {
                CompareValue32 = Convert.ToInt32(prms.CompareValue1.Value);
            }
            catch (OverflowException)
            {
                string error_str = String.Format("{0} is outside the range of Int32 type "
                    + prms.CompareValue1.Value.ToString());
                ep_Errors.SetError(m_numCompare1, error_str);
            }
            catch (FormatException)
            {
                string error_str = String.Format("{0} is not in a recognizable format."
                    + prms.CompareValue1.Value.ToString());
                ep_Errors.SetError(m_numCompare1, error_str);
            }

            Int32 Compare2Value32 = CompareDefaultValue2;
            try
            {
                Compare2Value32  = Convert.ToInt32(prms.CompareValue2.Value);
            }
            catch (OverflowException)
            {
                string error_str = String.Format("{0} is outside the range of Int32 type "
                    + prms.CompareValue2.Value.ToString());
                ep_Errors.SetError(m_numCompare2, error_str);
            }
            catch (FormatException)
            {
                string error_str = String.Format("{0} is not in a recognizable format."
                    + prms.CompareValue2.Value.ToString());
                ep_Errors.SetError(m_numCompare2, error_str);
            }

            UInt16 Compare1Value16 = CompareDefaultValue1;
            try
            {
                Compare1Value16 = Convert.ToUInt16(prms.CompareValue1.Value);
            }
            catch (OverflowException)
            {
                string error_str = String.Format("{0} is outside the range of Int32 type "
                    + prms.CompareValue1.Value.ToString());
                ep_Errors.SetError(m_numCompare1, error_str);
            }
            catch (FormatException)
            {
                string error_str = String.Format("{0} is not in a recognizable format."
                    + prms.CompareValue1.Value.ToString());
                ep_Errors.SetError(m_numCompare1, error_str);
            }

            UInt16 Compare2Value16 = CompareDefaultValue1;
            try
            {
                Compare2Value16 = Convert.ToUInt16(prms.CompareValue2.Value);
            }
            catch (OverflowException)
            {
                string error_str = String.Format("{0} is outside the range of Int32 type "
                    + prms.CompareValue2.Value.ToString());
                ep_Errors.SetError(m_numCompare2, error_str);
            }
            catch (FormatException)
            {
                string error_str = String.Format("{0} is not in a recognizable format."
                    + prms.CompareValue2.Value.ToString());
                ep_Errors.SetError(m_numCompare2, error_str);
            }

            float dbcounts = (float)(DeadTimeValue32 + 1) * bitwidth;
            PointF[] pts = new PointF[18]; //Maximum is 16 values.
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
                    case PWM1:
                        //Find the parameters for PWM1
                        Edge1 = startleft + (bitwidth * (float)(PeriodValue32 - CompareValue32));
                        Edge1_2 = startright + (bitwidth * (float)(PeriodValue32 - CompareValue32));
                        
                       /* "Less" OR "Less than or equal" compare type */
                        if (prms.CompareType1.Value == Less_Str || prms.CompareType1.Value == Less_Equal_Str)
                        {
                            StartHigh = false;
                            comparestring1 = " <= " + prms.CompareValue1.Value;

                            if (prms.CompareType1.Value != Less_Equal_Str)
                            {
                                Edge1 += bitwidth;
                                Edge1_2 += bitwidth;
                                comparestring1 = " < " + prms.CompareValue1.Value;
                            }

                            if ((CompareValue32 > PeriodValue32) ||
                           ((CompareValue32 >= PeriodValue32) &&
                             prms.CompareType1.Value == Less_Equal_Str))
                            {
                                pts[0] = new PointF(startleft, StartHigh ? LowY : HighY);
                                pts[1] = pts[0];
                                pts[2] = pts[0];
                                pts[3] = pts[0];
                                pts[4] = pts[0];
                                pts[5] = pts[0];
                                pts[6] = pts[0];
                                pts[7] = pts[0];
                                pts[8] = pts[7];
                                pts[9] = pts[7];
                                pts[10] = pts[7];
                                pts[11] = pts[7];
                                pts[12] = pts[7];
                                pts[13] = pts[7];
                                pts[14] = pts[7];
                                pts[15] = pts[7];
                                pts[16] = pts[7];
                                pts[17] = new PointF(endright, StartHigh ? LowY : HighY);
                            }
                            else
                            {
                                //Draw the first version of the waveform from StartLeft to StartRight
                                pts[0] = new PointF(startleft, StartHigh ? HighY : LowY);
                                pts[1] = new PointF(Edge1, StartHigh ? HighY : LowY);
                                if (CompareValue32 == 0)
                                {
                                    if (prms.CompareType1.Value == Less_Equal_Str)
                                    {
                                        pts[2] = new PointF(Edge1, StartHigh ? LowY : HighY);
                                        pts[3] = new PointF(startright, StartHigh ? LowY : HighY);
                                        pts[6] = new PointF(Edge1_2, StartHigh ? LowY : HighY);
                                        pts[7] = new PointF(endright, StartHigh ? LowY : HighY);
                                    }
                                    else
                                    {
                                        pts[2] = new PointF(Edge1, StartHigh ? HighY : LowY);
                                        pts[3] = new PointF(Edge1, StartHigh ? HighY : LowY);
                                        //setting the last point of waveform
                                        pts[6] = new PointF(Edge1_2, StartHigh ? HighY : LowY);
                                        pts[7] = new PointF(endright, StartHigh ? HighY : LowY);
                                    }
                                }
                                else
                                {
                                    pts[2] = new PointF(Edge1, StartHigh ? LowY : HighY);
                                    pts[3] = new PointF(startright, StartHigh ? LowY : HighY);
                                    pts[6] = new PointF(Edge1_2, StartHigh ? LowY : HighY);
                                    pts[7] = new PointF(endright, StartHigh ? LowY : HighY);
                                }
                                pts[4] = new PointF(startright, StartHigh ? HighY : LowY);
                                pts[5] = new PointF(Edge1_2, StartHigh ? HighY : LowY);
                                pts[8] = pts[7];
                                pts[9] = pts[7];
                                pts[10] = pts[7];
                                pts[11] = pts[7];
                                pts[12] = pts[7];
                                pts[13] = pts[7];
                                pts[14] = pts[7];
                                pts[15] = pts[7];
                                pts[16] = pts[7];
                                pts[17] = pts[7];
                            }
                        }
                        /* "Greater" OR "Greater than or equal" compare type */
                        else if (prms.CompareType1.Value == Greater_Str || prms.CompareType1.Value == Greater_Equal_Str)
                        {
                            StartHigh = true;
                            comparestring1 = " > " + prms.CompareValue1.Value;
                            if (prms.CompareType1.Value == Greater_Equal_Str)
                            {
                                Edge1 += bitwidth;
                                Edge1_2 += bitwidth;
                                comparestring1 = " >= " + prms.CompareValue1.Value;
                            }

                            if (((CompareValue32 >= PeriodValue32) && prms.CompareType1.Value != Greater_Equal_Str) ||
                              ((CompareValue32 > PeriodValue32) && prms.CompareType1.Value == Greater_Equal_Str))
                            {
                                pts[0] = new PointF(startleft, StartHigh ? LowY : HighY);
                                pts[1] = pts[0];
                                pts[2] = pts[0];
                                pts[3] = pts[0];
                                pts[4] = pts[0];
                                pts[5] = pts[0];
                                pts[6] = pts[0];
                                pts[7] = pts[0];
                                pts[8] = pts[7];
                                pts[9] = pts[7];
                                pts[10] = pts[7];
                                pts[11] = pts[7];
                                pts[12] = pts[7];
                                pts[13] = pts[7];
                                pts[14] = pts[7];
                                pts[15] = pts[7];
                                pts[16] = pts[7];
                                pts[17] = new PointF(endright, StartHigh ? LowY : HighY);
                            }
                            else
                            {
                                //Draw the first version of the waveform from StartLeft to StartRight
                                pts[0] = new PointF(startleft, StartHigh ? HighY : LowY);
                                pts[1] = new PointF(Edge1, StartHigh ? HighY : LowY);
                                if (CompareValue32 == 0)
                                {
                                    if (prms.CompareType1.Value == Greater_Equal_Str)
                                    {
                                        pts[2] = new PointF(Edge1, StartHigh ? HighY : LowY);
                                        pts[3] = new PointF(startright, StartHigh ? HighY : LowY);
                                        pts[6] = new PointF(Edge1_2, StartHigh ? HighY : LowY);
                                        pts[7] = new PointF(endright, StartHigh ? HighY : LowY);
                                    }
                                    else
                                    {
                                        pts[2] = new PointF(Edge1, StartHigh ? LowY : HighY);
                                        pts[3] = new PointF(startright, StartHigh ? LowY : HighY);
                                        //setting the last point of waveform
                                        pts[6] = new PointF(Edge1_2, StartHigh ? LowY : HighY);
                                        pts[7] = new PointF(endright, StartHigh ? LowY : HighY);
                                    }
                                }
                                else
                                {
                                    pts[2] = new PointF(Edge1, StartHigh ? LowY : HighY);
                                    pts[3] = new PointF(startright, StartHigh ? LowY : HighY);
                                    pts[6] = new PointF(Edge1_2, StartHigh ? LowY : HighY);
                                    pts[7] = new PointF(endright, StartHigh ? LowY : HighY);
                                }
                                pts[4] = new PointF(startright, StartHigh ? HighY : LowY);
                                pts[5] = new PointF(Edge1_2, StartHigh ? HighY : LowY);
                                pts[8] = pts[7];
                                pts[9] = pts[7];
                                pts[10] = pts[7];
                                pts[11] = pts[7];
                                pts[12] = pts[7];
                                pts[13] = pts[7];
                                pts[14] = pts[7];
                                pts[15] = pts[7];
                                pts[16] = pts[7];
                                pts[17] = pts[7];
                            }
                        }
                        /* "Equal" compare type; cmp value > period value */
                        else if ((CompareValue32 > PeriodValue32)
                                  && (prms.CompareType1.Value == Equal_Str))
                        {

                            StartHigh = true;
                            Edge1 += bitwidth;
                            Edge1_2 += bitwidth;

                            pts[0] = new PointF(startleft, StartHigh ? LowY : HighY);
                            pts[1] = pts[0];
                            pts[2] = pts[0];
                            pts[3] = pts[0];
                            pts[4] = pts[0];
                            pts[5] = pts[0];
                            pts[6] = pts[0];
                            pts[7] = pts[0];
                            pts[8] = pts[7];
                            pts[9] = pts[7];
                            pts[10] = pts[7];
                            pts[11] = pts[7];
                            pts[12] = pts[7];
                            pts[13] = pts[7];
                            pts[14] = pts[7];
                            pts[15] = pts[7];
                            pts[16] = pts[7];
                            pts[17] = new PointF(endright, StartHigh ? LowY : HighY);
                        }
                        else
                        {
                            /*Here we consider "Firmware control" compare type */
                            StartHigh = false;
                            Edge2 = Edge1 + bitwidth;
                            Edge2_2 = Edge1_2 + bitwidth;
                            comparestring1 = " < " + prms.CompareValue1.Value;
                            //Draw the first version of the waveform from StartLeft to StartRight
                            pts[0] = new PointF(startleft, LowY);
                            pts[1] = new PointF(Edge1, LowY);
                            pts[2] = new PointF(Edge1, HighY);
                            pts[3] = new PointF(Edge2, HighY);
                            pts[4] = new PointF(Edge2, LowY);
                            pts[5] = new PointF(Edge1_2, LowY);
                            pts[6] = new PointF(Edge1_2, HighY);
                            pts[7] = new PointF(Edge2_2, HighY);
                            pts[8] = new PointF(Edge2_2, LowY);
                            pts[9] = new PointF(endright, LowY);
                            pts[10] = pts[9];
                            pts[11] = pts[9];
                            pts[12] = pts[9];
                            pts[13] = pts[9];
                            pts[14] = pts[9];
                            pts[15] = pts[9];
                            pts[16] = pts[9];
                            pts[17] = pts[9];
                        }

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
                    case PWM2:
                        //Find the parameters for PWM2
                        Edge1 = startleft + (bitwidth * (float)(PeriodValue32 - Compare2Value32 ));
                        Edge1_2 = startright + (bitwidth * (float)(PeriodValue32 - Compare2Value32 ));
                        if (prms.CompareType2.Value == Less_Str || prms.CompareType2.Value == Less_Equal_Str)
                        {
                            StartHigh = false;
                            comparestring1 = " <= " + prms.CompareValue2.Value;
                            if (prms.CompareType2.Value != Less_Equal_Str)
                            {
                                Edge1 += bitwidth;
                                Edge1_2 += bitwidth;
                                comparestring1 = " < " + prms.CompareValue2.Value;
                            }

                            if ((Compare2Value32  > PeriodValue32) ||
                               ((Compare2Value32  >= PeriodValue32) &&
                                 prms.CompareType2.Value == Less_Equal_Str))
                            {
                                pts[0] = new PointF(startleft, StartHigh ? LowY : HighY);
                                pts[1] = pts[0];
                                pts[2] = pts[0];
                                pts[3] = pts[0];
                                pts[4] = pts[0];
                                pts[5] = pts[0];
                                pts[6] = pts[0];
                                pts[7] = pts[0];
                                pts[8] = pts[7];
                                pts[9] = pts[7];
                                pts[10] = pts[7];
                                pts[11] = pts[7];
                                pts[12] = pts[7];
                                pts[13] = pts[7];
                                pts[14] = pts[7];
                                pts[15] = pts[7];
                                pts[16] = pts[7];
                                pts[17] = new PointF(endright, StartHigh ? LowY : HighY);
                            }
                            else
                            {
                                pts[0] = new PointF(startleft, StartHigh ? HighY : LowY);
                                pts[1] = new PointF(Edge1, StartHigh ? HighY : LowY);
                                if (Compare2Value32  == 0)
                                {
                                    if (prms.CompareType2.Value == Less_Equal_Str)
                                    {
                                        pts[2] = new PointF(Edge1, StartHigh ? LowY : HighY);
                                        pts[3] = new PointF(startright, StartHigh ? LowY : HighY);
                                        pts[6] = new PointF(Edge1_2, StartHigh ? LowY : HighY);
                                        pts[7] = new PointF(endright, StartHigh ? LowY : HighY);
                                    }
                                    else
                                    {
                                        pts[2] = new PointF(Edge1, StartHigh ? HighY : LowY);
                                        pts[3] = new PointF(Edge1, StartHigh ? HighY : LowY);
                                        //setting the last point of waveform
                                        pts[6] = new PointF(Edge1_2, StartHigh ? HighY : LowY);
                                        pts[7] = new PointF(endright, StartHigh ? HighY : LowY);
                                    }
                                }
                                else
                                {
                                    pts[2] = new PointF(Edge1, StartHigh ? LowY : HighY);
                                    pts[3] = new PointF(startright, StartHigh ? LowY : HighY);
                                    pts[6] = new PointF(Edge1_2, StartHigh ? LowY : HighY);
                                    pts[7] = new PointF(endright, StartHigh ? LowY : HighY);
                                }
                                pts[4] = new PointF(startright, StartHigh ? HighY : LowY);
                                pts[5] = new PointF(Edge1_2, StartHigh ? HighY : LowY);
                                pts[8] = pts[7];
                                pts[9] = pts[7];
                                pts[10] = pts[7];
                                pts[11] = pts[7];
                                pts[12] = pts[7];
                                pts[13] = pts[7];
                                pts[14] = pts[7];
                                pts[15] = pts[7];
                                pts[16] = pts[7];
                                pts[17] = pts[7];
                            }
                        }
                        else if (prms.CompareType2.Value == Greater_Str || prms.CompareType2.Value == Greater_Equal_Str)
                        {
                            StartHigh = true;
                            comparestring1 = " > " + prms.CompareValue2.Value;
                            if (prms.CompareValue2.Value == Greater_Equal_Str)
                            {
                                Edge1 += bitwidth;
                                Edge1_2 += bitwidth;
                                comparestring1 = " >= " + prms.CompareValue2.Value;
                            }

                            if (((Compare2Value32  >= PeriodValue32) && prms.CompareType2.Value != Greater_Equal_Str) ||
                              ((Compare2Value32  > PeriodValue32) && prms.CompareType2.Value == Greater_Equal_Str))
                            {
                                pts[0] = new PointF(startleft, StartHigh ? LowY : HighY);
                                pts[1] = pts[0];
                                pts[2] = pts[0];
                                pts[3] = pts[0];
                                pts[4] = pts[0];
                                pts[5] = pts[0];
                                pts[6] = pts[0];
                                pts[7] = pts[0];
                                pts[8] = pts[7];
                                pts[9] = pts[7];
                                pts[10] = pts[7];
                                pts[11] = pts[7];
                                pts[12] = pts[7];
                                pts[13] = pts[7];
                                pts[14] = pts[7];
                                pts[15] = pts[7];
                                pts[16] = pts[7];
                                pts[17] = new PointF(endright, StartHigh ? LowY : HighY);
                            }
                            else
                            {
                                pts[0] = new PointF(startleft, StartHigh ? HighY : LowY);
                                pts[1] = new PointF(Edge1, StartHigh ? HighY : LowY);
                                if (Compare2Value32  == 0)
                                {
                                    if (prms.CompareType2.Value == Greater_Equal_Str)
                                    {
                                        pts[2] = new PointF(Edge1, StartHigh ? HighY : LowY);
                                        pts[3] = new PointF(startright, StartHigh ? HighY : LowY);
                                        pts[6] = new PointF(Edge1_2, StartHigh ? HighY : LowY);
                                        pts[7] = new PointF(endright, StartHigh ? HighY : LowY);
                                    }
                                    else
                                    {
                                        pts[2] = new PointF(Edge1, StartHigh ? LowY : HighY);
                                        pts[3] = new PointF(startright, StartHigh ? LowY : HighY);
                                        //setting the last point of waveform
                                        pts[6] = new PointF(Edge1_2, StartHigh ? LowY : HighY);
                                        pts[7] = new PointF(endright, StartHigh ? LowY : HighY);
                                    }
                                }
                                else
                                {
                                    pts[2] = new PointF(Edge1, StartHigh ? LowY : HighY);
                                    pts[3] = new PointF(startright, StartHigh ? LowY : HighY);
                                    pts[6] = new PointF(Edge1_2, StartHigh ? LowY : HighY);
                                    pts[7] = new PointF(endright, StartHigh ? LowY : HighY);
                                }
                                pts[4] = new PointF(startright, StartHigh ? HighY : LowY);
                                pts[5] = new PointF(Edge1_2, StartHigh ? HighY : LowY);
                                pts[8] = pts[7];
                                pts[9] = pts[7];
                                pts[10] = pts[7];
                                pts[11] = pts[7];
                                pts[12] = pts[7];
                                pts[13] = pts[7];
                                pts[14] = pts[7];
                                pts[15] = pts[7];
                                pts[16] = pts[7];
                                pts[17] = pts[7];
                            }
                        }
                        else if ((Compare2Value32  > PeriodValue32)
                                  && (prms.CompareType2.Value == Equal_Str))
                        {
                            StartHigh = true;
                            Edge1 += bitwidth;
                            Edge1_2 += bitwidth;
                            pts[0] = new PointF(startleft, StartHigh ? LowY : HighY);
                            pts[1] = pts[0];
                            pts[2] = pts[0];
                            pts[3] = pts[0];
                            pts[4] = pts[0];
                            pts[5] = pts[0];
                            pts[6] = pts[0];
                            pts[7] = pts[0];
                            pts[8] = pts[7];
                            pts[9] = pts[7];
                            pts[10] = pts[7];
                            pts[11] = pts[7];
                            pts[12] = pts[7];
                            pts[13] = pts[7];
                            pts[14] = pts[7];
                            pts[15] = pts[7];
                            pts[16] = pts[7];
                            pts[17] = new PointF(endright, StartHigh ? LowY : HighY);
                        }
                        else
                        {
                            StartHigh = false;
                            Edge2 = Edge1 + bitwidth;
                            Edge2_2 = Edge1_2 + bitwidth;
                            comparestring1 = " < " + prms.CompareValue2.Value;
                            //Draw the first version of the waveform from StartLeft to StartRight
                            pts[0] = new PointF(startleft, LowY);
                            pts[1] = new PointF(Edge1, LowY);
                            pts[2] = new PointF(Edge1, HighY);
                            pts[3] = new PointF(Edge2, HighY);
                            pts[4] = new PointF(Edge2, LowY);
                            pts[5] = new PointF(Edge1_2, LowY);
                            pts[6] = new PointF(Edge1_2, HighY);
                            pts[7] = new PointF(Edge2_2, HighY);
                            pts[8] = new PointF(Edge2_2, LowY);
                            pts[9] = new PointF(endright, LowY);
                            pts[10] = pts[9];
                            pts[11] = pts[9];
                            pts[12] = pts[9];
                            pts[13] = pts[9];
                            pts[14] = pts[9];
                            pts[15] = pts[9];
                            pts[16] = pts[9];
                            pts[17] = pts[9];
                        }
                        break;
                    #endregion
                    #region PWM
                    case PWM:
                        //Find the parameters for PWM
                        #region PWM-ONE
                        if (prms.PWMMode.Value == One_Output_Str)
                        {
                            Edge1 = startleft + (bitwidth * (float)(PeriodValue32 - CompareValue32));
                            Edge1_2 = startright + (bitwidth * (float)(PeriodValue32 - CompareValue32));
                            if (prms.CompareType1.Value == Less_Str || prms.CompareType1.Value == Less_Equal_Str)
                            {
                                StartHigh = false;
                                comparestring1 = " <= " + prms.CompareValue1.Value;
                                if (prms.CompareType1.Value != Less_Equal_Str)
                                {
                                    Edge1 += bitwidth;
                                    Edge1_2 += bitwidth;
                                    comparestring1 = " < " + prms.CompareValue1.Value;
                                }

                                if ((CompareValue32 > PeriodValue32) ||
                                   ((CompareValue32 >= PeriodValue32) &&
                                     prms.CompareType1.Value == Less_Equal_Str))
                                {
                                    pts[0] = new PointF(startleft, StartHigh ? LowY : HighY);
                                    pts[1] = pts[0];
                                    pts[2] = pts[0];
                                    pts[3] = pts[0];
                                    pts[4] = pts[0];
                                    pts[5] = pts[0];
                                    pts[6] = pts[0];
                                    pts[7] = pts[0];
                                    pts[8] = pts[7];
                                    pts[9] = pts[7];
                                    pts[10] = pts[7];
                                    pts[11] = pts[7];
                                    pts[12] = pts[7];
                                    pts[13] = pts[7];
                                    pts[14] = pts[7];
                                    pts[15] = pts[7];
                                    pts[16] = pts[7];
                                    pts[17] = new PointF(endright, StartHigh ? LowY : HighY);
                                }
                                else
                                {
                                    pts[0] = new PointF(startleft, StartHigh ? HighY : LowY);
                                    pts[1] = new PointF(Edge1, StartHigh ? HighY : LowY);
                                    if (CompareValue32 == 0)
                                    {
                                        if (prms.CompareType1.Value == Less_Equal_Str)
                                        {
                                            pts[2] = new PointF(Edge1, StartHigh ? LowY : HighY);
                                            pts[3] = new PointF(startright, StartHigh ? LowY : HighY);
                                            pts[6] = new PointF(Edge1_2, StartHigh ? LowY : HighY);
                                            pts[7] = new PointF(endright, StartHigh ? LowY : HighY);
                                        }
                                        else
                                        {
                                            pts[2] = new PointF(Edge1, StartHigh ? HighY : LowY);
                                            pts[3] = new PointF(Edge1, StartHigh ? HighY : LowY);
                                            pts[6] = new PointF(Edge1_2, StartHigh ? HighY : LowY);
                                            pts[7] = new PointF(endright, StartHigh ? HighY : LowY);
                                        }
                                    }
                                    else
                                    {
                                        pts[2] = new PointF(Edge1, StartHigh ? LowY : HighY);
                                        pts[3] = new PointF(startright, StartHigh ? LowY : HighY);
                                        pts[6] = new PointF(Edge1_2, StartHigh ? LowY : HighY);
                                        pts[7] = new PointF(endright, StartHigh ? LowY : HighY);
                                    }
                                    pts[4] = new PointF(startright, StartHigh ? HighY : LowY);
                                    pts[5] = new PointF(Edge1_2, StartHigh ? HighY : LowY);
                                    pts[8] = pts[7];
                                    pts[9] = pts[7];
                                    pts[10] = pts[7];
                                    pts[11] = pts[7];
                                    pts[12] = pts[7];
                                    pts[13] = pts[7];
                                    pts[14] = pts[7];
                                    pts[15] = pts[7];
                                    pts[16] = pts[7];
                                    pts[17] = pts[7];
                                }
                            }
                            else if (prms.CompareType1.Value == Greater_Str ||
                                prms.CompareType1.Value == Greater_Equal_Str)
                            {
                                StartHigh = true;
                                comparestring1 = " > " + prms.CompareValue1.Value;
                                if (prms.CompareType1.Value == Greater_Equal_Str)
                                {
                                    Edge1 += bitwidth;
                                    Edge1_2 += bitwidth;
                                    comparestring1 = " >= " + prms.CompareValue1.Value;
                                }

                                if (((CompareValue32 >= PeriodValue32) && 
                                    prms.CompareType1.Value != Greater_Equal_Str) || ((CompareValue32 > PeriodValue32)
                                    && prms.CompareType1.Value == Greater_Equal_Str))
                                {
                                    pts[0] = new PointF(startleft, StartHigh ? LowY : HighY);
                                    pts[1] = pts[0];
                                    pts[2] = pts[0];
                                    pts[3] = pts[0];
                                    pts[4] = pts[0];
                                    pts[5] = pts[0];
                                    pts[6] = pts[0];
                                    pts[7] = pts[0];
                                    pts[8] = pts[7];
                                    pts[9] = pts[7];
                                    pts[10] = pts[7];
                                    pts[11] = pts[7];
                                    pts[12] = pts[7];
                                    pts[13] = pts[7];
                                    pts[14] = pts[7];
                                    pts[15] = pts[7];
                                    pts[16] = pts[7];
                                    pts[17] = new PointF(endright, StartHigh ? LowY : HighY);
                                }
                                else
                                {
                                    pts[0] = new PointF(startleft, StartHigh ? HighY : LowY);
                                    pts[1] = new PointF(Edge1, StartHigh ? HighY : LowY);
                                    if (CompareValue32 == 0)
                                    {
                                        if (prms.CompareType1.Value == Greater_Equal_Str)
                                        {
                                            pts[2] = new PointF(Edge1, StartHigh ? HighY : LowY);
                                            pts[3] = new PointF(startright, StartHigh ? HighY : LowY);
                                            pts[6] = new PointF(Edge1_2, StartHigh ? HighY : LowY);
                                            pts[7] = new PointF(endright, StartHigh ? HighY : LowY);
                                        }
                                        else
                                        {
                                            pts[2] = new PointF(Edge1, StartHigh ? LowY : HighY);
                                            pts[3] = new PointF(startright, StartHigh ? LowY : HighY);
                                            pts[6] = new PointF(Edge1_2, StartHigh ? LowY : HighY);
                                            pts[7] = new PointF(endright, StartHigh ? LowY : HighY);
                                        }
                                    }
                                    else
                                    {
                                        pts[2] = new PointF(Edge1, StartHigh ? LowY : HighY);
                                        pts[3] = new PointF(startright, StartHigh ? LowY : HighY);
                                        pts[6] = new PointF(Edge1_2, StartHigh ? LowY : HighY);
                                        pts[7] = new PointF(endright, StartHigh ? LowY : HighY);
                                    }
                                    pts[4] = new PointF(startright, StartHigh ? HighY : LowY);
                                    pts[5] = new PointF(Edge1_2, StartHigh ? HighY : LowY);
                                    pts[8] = pts[7];
                                    pts[9] = pts[7];
                                    pts[10] = pts[7];
                                    pts[11] = pts[7];
                                    pts[12] = pts[7];
                                    pts[13] = pts[7];
                                    pts[14] = pts[7];
                                    pts[15] = pts[7];
                                    pts[16] = pts[7];
                                    pts[17] = pts[7];
                                }
                            }
                            else if ((CompareValue32 > PeriodValue32)
                                      && (prms.CompareType1.Value == Equal_Str))
                            {
                                StartHigh = true;
                                Edge1 += bitwidth;
                                Edge1_2 += bitwidth;
                                pts[0] = new PointF(startleft, StartHigh ? LowY : HighY);
                                pts[1] = pts[0];
                                pts[2] = pts[0];
                                pts[3] = pts[0];
                                pts[4] = pts[0];
                                pts[5] = pts[0];
                                pts[6] = pts[0];
                                pts[7] = pts[0];
                                pts[8] = pts[7];
                                pts[9] = pts[7];
                                pts[10] = pts[7];
                                pts[11] = pts[7];
                                pts[12] = pts[7];
                                pts[13] = pts[7];
                                pts[14] = pts[7];
                                pts[15] = pts[7];
                                pts[16] = pts[7];
                                pts[17] = new PointF(endright, StartHigh ? LowY : HighY);
                            }
                            else
                            {
                                StartHigh = false;
                                Edge2 = Edge1 + bitwidth;
                                Edge2_2 = Edge1_2 + bitwidth;
                                comparestring1 = " < " + prms.CompareValue1.Value;
                                //Draw the first version of the waveform from StartLeft to StartRight
                                pts[0] = new PointF(startleft, LowY);
                                pts[1] = new PointF(Edge1, LowY);
                                pts[2] = new PointF(Edge1, HighY);
                                pts[3] = new PointF(Edge2, HighY);
                                pts[4] = new PointF(Edge2, LowY);
                                pts[5] = new PointF(Edge1_2, LowY);
                                pts[6] = new PointF(Edge1_2, HighY);
                                pts[7] = new PointF(Edge2_2, HighY);
                                pts[8] = new PointF(Edge2_2, LowY);
                                pts[9] = new PointF(endright, LowY);
                                pts[10] = pts[9];
                                pts[11] = pts[9];
                                pts[12] = pts[9];
                                pts[13] = pts[9];
                                pts[14] = pts[9];
                                pts[15] = pts[9];
                                pts[16] = pts[9];
                                pts[17] = pts[9];
                            }
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
                        if (prms.PWMMode.Value == Dual_Edge_Str)
                        {
                            string cmptype1 = "0";
                            string cmptype2 = "0";
                            Edge1 = startleft + (bitwidth * (float)(PeriodValue32 - CompareValue32));
                            Edge1_2 = startright + (bitwidth * (float)(PeriodValue32 - CompareValue32));
                            Edge2 = startleft + (bitwidth * (float)(PeriodValue32 - Compare2Value32 ));
                            Edge2_2 = startright + (bitwidth * (float)(PeriodValue32 - Compare2Value32 ));

                            if (prms.CompareType1.Value == Less_Str || prms.CompareType1.Value == Less_Equal_Str)
                            {
                                cmptype1 = "0";
                                if (prms.CompareType1.Value != Less_Equal_Str)
                                {
                                    Edge1 += bitwidth;
                                    Edge1_2 += bitwidth;
                                }
                            }

                            else if (prms.CompareType1.Value == Greater_Str ||
                                prms.CompareType1.Value == Greater_Equal_Str)
                            {
                                cmptype1 = "1";
                                if (prms.CompareType1.Value == Greater_Equal_Str)
                                {
                                    Edge1 += bitwidth;
                                    Edge1_2 += bitwidth;
                                }
                            }
                            else
                            {
                                cmptype1 = "X";
                                StartHigh = false;
                                Edge3 = Edge1 + bitwidth;
                                Edge3_2 = Edge1_2 + bitwidth;
                                comparestring1 = " < " + prms.CompareValue1.Value;
                            }

                            //Get Compare Type Information for Compare Type 2
                            if (prms.CompareType2.Value == Less_Str || prms.CompareType2.Value == Less_Equal_Str)
                            {
                                cmptype2 = "0";
                                if (prms.CompareType2.Value != Less_Equal_Str)
                                {
                                    Edge2 += bitwidth;
                                    Edge2_2 += bitwidth;
                                }
                            }
                            else if (prms.CompareType2.Value == Greater_Str ||
                                prms.CompareType2.Value == Greater_Equal_Str)
                            {
                                cmptype2 = "1";
                                if (prms.CompareType2.Value == Greater_Equal_Str)
                                {
                                    Edge2 += bitwidth;
                                    Edge2_2 += bitwidth;
                                }
                            }
                            else
                            {
                                cmptype2 = "X";
                                Edge4 = Edge2 + bitwidth;
                                Edge4_2 = Edge2_2 + bitwidth;
                            }

                            switch (cmptype1 + cmptype2)
                            {
                                case "00": // Compare1 = Less Than, Compare2 = Less Than
                                    if ((CompareValue32 > PeriodValue32) &&
                                        (Compare2Value32  > PeriodValue32))
                                    {
                                        StartHigh = true;
                                        Edge1 += bitwidth;
                                        Edge1_2 += bitwidth;
                                        pts[0] = new PointF(startleft, StartHigh ? HighY : LowY);
                                        pts[1] = pts[0];
                                        pts[2] = pts[0];
                                        pts[3] = pts[0];
                                        pts[4] = pts[0];
                                        pts[5] = pts[0];
                                        pts[6] = pts[0];
                                        pts[7] = pts[0];
                                        pts[8] = pts[7];
                                        pts[9] = pts[7];
                                        pts[10] = pts[7];
                                        pts[11] = pts[7];
                                        pts[12] = pts[7];
                                        pts[13] = pts[7];
                                        pts[14] = pts[7];
                                        pts[15] = pts[7];
                                        pts[16] = pts[7];
                                        pts[17] = new PointF(endright, StartHigh ? HighY : LowY);
                                    }
                                    else
                                    {
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
                                        pts[16] = pts[7];
                                        pts[17] = pts[7];
                                    }

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

                                    if (CompareValue32 > PeriodValue32)
                                    {
                                        StartHigh = true;
                                        Edge1 += bitwidth;
                                        Edge1_2 += bitwidth;
                                        pts[0] = new PointF(startleft, StartHigh ? LowY : HighY);
                                        pts[1] = pts[0];
                                        pts[2] = pts[0];
                                        pts[3] = pts[0];
                                        pts[4] = pts[0];
                                        pts[5] = pts[0];
                                        pts[6] = pts[0];
                                        pts[7] = pts[0];
                                        pts[8] = pts[7];
                                        pts[9] = pts[7];
                                        pts[10] = pts[7];
                                        pts[11] = pts[7];
                                        pts[12] = pts[7];
                                        pts[13] = pts[7];
                                        pts[14] = pts[7];
                                        pts[15] = pts[7];
                                        pts[16] = pts[7];
                                        pts[17] = new PointF(endright, StartHigh ? LowY : HighY);
                                    }
                                    else
                                    {
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
                                            pts[16] = pts[11];
                                            pts[17] = pts[11];
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
                                            pts[16] = pts[11];
                                            pts[17] = pts[11];
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
                                    }
                                    break;
                                case "11": // High to Low
                                    if ((CompareValue32 > PeriodValue32) ||
                                        (Compare2Value32  > PeriodValue32))
                                    {
                                        StartHigh = true;
                                        Edge1 += bitwidth;
                                        Edge1_2 += bitwidth;
                                        pts[0] = new PointF(startleft, StartHigh ? LowY : HighY);
                                        pts[1] = pts[0];
                                        pts[2] = pts[0];
                                        pts[3] = pts[0];
                                        pts[4] = pts[0];
                                        pts[5] = pts[0];
                                        pts[6] = pts[0];
                                        pts[7] = pts[0];
                                        pts[8] = pts[7];
                                        pts[9] = pts[7];
                                        pts[10] = pts[7];
                                        pts[11] = pts[7];
                                        pts[12] = pts[7];
                                        pts[13] = pts[7];
                                        pts[14] = pts[7];
                                        pts[15] = pts[7];
                                        pts[16] = pts[7];
                                        pts[17] = new PointF(endright, StartHigh ? LowY : HighY);
                                    }
                                    else
                                    {
                                        StartHigh = true;
                                        pts[0] = new PointF(startleft, HighY);
                                        pts[1] = new PointF(Math.Min(Edge1, Edge2), HighY);
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
                                        pts[16] = pts[7];
                                        pts[17] = pts[7];

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
                                    }
                                    break;
                                case "XX": //cmp1type = "Equal", cmp2type = "Equal"
                                    if ((CompareValue32 > PeriodValue32) ||
                                        (Compare2Value32  > PeriodValue32))
                                    {
                                        StartHigh = true;
                                        Edge1 += bitwidth;
                                        Edge1_2 += bitwidth;
                                        pts[0] = new PointF(startleft, StartHigh ? LowY : HighY);
                                        pts[1] = pts[0];
                                        pts[2] = pts[0];
                                        pts[3] = pts[0];
                                        pts[4] = pts[0];
                                        pts[5] = pts[0];
                                        pts[6] = pts[0];
                                        pts[7] = pts[0];
                                        pts[8] = pts[7];
                                        pts[9] = pts[7];
                                        pts[10] = pts[7];
                                        pts[11] = pts[7];
                                        pts[12] = pts[7];
                                        pts[13] = pts[7];
                                        pts[14] = pts[7];
                                        pts[15] = pts[7];
                                        pts[16] = pts[7];
                                        pts[17] = new PointF(endright, StartHigh ? LowY : HighY);
                                    }
                                    else
                                    {
                                        if (prms.CompareValue1.Value == prms.CompareValue2.Value)
                                        {
                                            pts[0] = new PointF(startleft, LowY);
                                            pts[1] = new PointF(Edge1, LowY);
                                            pts[2] = new PointF(Edge1, HighY);
                                            pts[3] = new PointF(Edge3, HighY);
                                            pts[4] = new PointF(Edge3, LowY);
                                            pts[5] = new PointF(Edge1_2, LowY);
                                            pts[6] = new PointF(Edge1_2, HighY);
                                            pts[7] = new PointF(Edge3_2, HighY);
                                            pts[8] = new PointF(Edge3_2, LowY);
                                            pts[9] = new PointF(endright, LowY);
                                            pts[10] = pts[9];
                                            pts[11] = pts[9];
                                            pts[12] = pts[9];
                                            pts[13] = pts[9];
                                            pts[14] = pts[9];
                                            pts[15] = pts[9];
                                            pts[16] = pts[9];
                                            pts[17] = pts[9];
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
                                            pts[12] = pts[1];
                                            pts[13] = pts[1];
                                            pts[14] = pts[1];
                                            pts[15] = pts[1];
                                            pts[16] = pts[1];
                                            pts[17] = pts[1];
                                        }
                                    }
                                    break;
                                case "X0": //cmp1type = "Equal", cmp2type = "Less Than"
                                    if (CompareValue32 > PeriodValue32)
                                    {
                                        StartHigh = true;
                                        Edge1 += bitwidth;
                                        Edge1_2 += bitwidth;
                                        pts[0] = new PointF(startleft, StartHigh ? LowY : HighY);
                                        pts[1] = pts[0];
                                        pts[2] = pts[0];
                                        pts[3] = pts[0];
                                        pts[4] = pts[0];
                                        pts[5] = pts[0];
                                        pts[6] = pts[0];
                                        pts[7] = pts[0];
                                        pts[8] = pts[7];
                                        pts[9] = pts[7];
                                        pts[10] = pts[7];
                                        pts[11] = pts[7];
                                        pts[12] = pts[7];
                                        pts[13] = pts[7];
                                        pts[14] = pts[7];
                                        pts[15] = pts[7];
                                        pts[16] = pts[7];
                                        pts[17] = new PointF(endright, StartHigh ? LowY : HighY);
                                    }
                                    else
                                    {
                                        if ((prms.CompareType2.Value == Equal_Str && 
                                            (Compare1Value16 <= Compare2Value16)) ||
                                            (Compare1Value16 <= Compare2Value16))
                                        {
                                            pts[0] = new PointF(startleft, LowY);
                                            pts[1] = new PointF(Edge1, LowY);
                                            pts[2] = new PointF(Edge1, HighY);
                                            pts[3] = new PointF(Edge3, HighY);
                                            pts[4] = new PointF(Edge3, LowY);
                                            pts[5] = new PointF(Edge1_2, LowY);
                                            pts[6] = new PointF(Edge1_2, HighY);
                                            pts[7] = new PointF(Edge3_2, HighY);
                                            pts[8] = new PointF(Edge3_2, LowY);
                                            pts[9] = new PointF(endright, LowY);
                                            pts[10] = pts[9];
                                            pts[11] = pts[9];
                                            pts[12] = pts[9];
                                            pts[13] = pts[9];
                                            pts[14] = pts[9];
                                            pts[15] = pts[9];
                                            pts[16] = pts[9];
                                            pts[17] = pts[9];
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
                                            pts[12] = pts[1];
                                            pts[13] = pts[1];
                                            pts[14] = pts[1];
                                            pts[15] = pts[1];
                                            pts[16] = pts[1];
                                            pts[17] = pts[1];
                                        }
                                    }
                                    break;
                                case "X1": //cmp1type = "Equal", cmp2type = "Great Than"
                                    if ((CompareValue32 > PeriodValue32) ||
                                        (Compare2Value32  > PeriodValue32))
                                    {
                                        StartHigh = true;
                                        Edge1 += bitwidth;
                                        Edge1_2 += bitwidth;
                                        pts[0] = new PointF(startleft, StartHigh ? LowY : HighY);
                                        pts[1] = pts[0];
                                        pts[2] = pts[0];
                                        pts[3] = pts[0];
                                        pts[4] = pts[0];
                                        pts[5] = pts[0];
                                        pts[6] = pts[0];
                                        pts[7] = pts[0];
                                        pts[8] = pts[7];
                                        pts[9] = pts[7];
                                        pts[10] = pts[7];
                                        pts[11] = pts[7];
                                        pts[12] = pts[7];
                                        pts[13] = pts[7];
                                        pts[14] = pts[7];
                                        pts[15] = pts[7];
                                        pts[16] = pts[7];
                                        pts[17] = new PointF(endright, StartHigh ? LowY : HighY);
                                    }
                                    else
                                    {
                                        if ((prms.CompareType2.Value == Equal_Str &&
                                            (Compare1Value16 >= Compare2Value16)) ||
                                            (Compare1Value16 > Compare2Value16))
                                        {
                                            pts[0] = new PointF(startleft, LowY);
                                            pts[1] = new PointF(Edge1, LowY);
                                            pts[2] = new PointF(Edge1, HighY);
                                            pts[3] = new PointF(Edge3, HighY);
                                            pts[4] = new PointF(Edge3, LowY);
                                            pts[5] = new PointF(Edge1_2, LowY);
                                            pts[6] = new PointF(Edge1_2, HighY);
                                            pts[7] = new PointF(Edge3_2, HighY);
                                            pts[8] = new PointF(Edge3_2, LowY);
                                            pts[9] = new PointF(endright, LowY);
                                            pts[10] = pts[9];
                                            pts[11] = pts[9];
                                            pts[12] = pts[9];
                                            pts[13] = pts[9];
                                            pts[14] = pts[9];
                                            pts[15] = pts[9];
                                            pts[16] = pts[9];
                                            pts[17] = pts[9];
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
                                            pts[12] = pts[1];
                                            pts[13] = pts[1];
                                            pts[14] = pts[1];
                                            pts[15] = pts[1];
                                            pts[16] = pts[1];
                                            pts[17] = pts[1];
                                        }
                                    }
                                    break;
                                case "0X": //cmp1type = "Less Than", cmp2type = "Equal"
                                    if (Compare2Value32  > PeriodValue32)
                                    {
                                        StartHigh = true;
                                        Edge1 += bitwidth;
                                        Edge1_2 += bitwidth;
                                        pts[0] = new PointF(startleft, StartHigh ? LowY : HighY);
                                        pts[1] = pts[0];
                                        pts[2] = pts[0];
                                        pts[3] = pts[0];
                                        pts[4] = pts[0];
                                        pts[5] = pts[0];
                                        pts[6] = pts[0];
                                        pts[7] = pts[0];
                                        pts[8] = pts[7];
                                        pts[9] = pts[7];
                                        pts[10] = pts[7];
                                        pts[11] = pts[7];
                                        pts[12] = pts[7];
                                        pts[13] = pts[7];
                                        pts[14] = pts[7];
                                        pts[15] = pts[7];
                                        pts[16] = pts[7];
                                        pts[17] = new PointF(endright, StartHigh ? LowY : HighY);
                                    }
                                    else
                                    {
                                        if ((prms.CompareType1.Value == Equal_Str &&
                                            (Compare1Value16 >= Compare2Value16)) ||
                                            (Compare1Value16 > Compare2Value16))
                                        {
                                            pts[0] = new PointF(startleft, LowY);
                                            pts[1] = new PointF(Edge2, LowY);
                                            pts[2] = new PointF(Edge2, HighY);
                                            pts[3] = new PointF(Edge4, HighY);
                                            pts[4] = new PointF(Edge4, LowY);
                                            pts[5] = new PointF(Edge2_2, LowY);
                                            pts[6] = new PointF(Edge2_2, HighY);
                                            pts[7] = new PointF(Edge4_2, HighY);
                                            pts[8] = new PointF(Edge4_2, LowY);
                                            pts[9] = new PointF(endright, LowY);
                                            pts[10] = pts[9];
                                            pts[11] = pts[9];
                                            pts[12] = pts[9];
                                            pts[13] = pts[9];
                                            pts[14] = pts[9];
                                            pts[15] = pts[9];
                                            pts[16] = pts[9];
                                            pts[17] = pts[9];
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
                                            pts[12] = pts[1];
                                            pts[13] = pts[1];
                                            pts[14] = pts[1];
                                            pts[15] = pts[1];
                                            pts[16] = pts[1];
                                            pts[17] = pts[1];
                                        }
                                    }
                                    break;
                                case "1X": //cmp1type = "Great Than", cmp2type = "Equal"
                                    if (Compare2Value32  > PeriodValue32)
                                    {
                                        StartHigh = true;
                                        Edge1 += bitwidth;
                                        Edge1_2 += bitwidth;
                                        pts[0] = new PointF(startleft, StartHigh ? LowY : HighY);
                                        pts[1] = pts[0];
                                        pts[2] = pts[0];
                                        pts[3] = pts[0];
                                        pts[4] = pts[0];
                                        pts[5] = pts[0];
                                        pts[6] = pts[0];
                                        pts[7] = pts[0];
                                        pts[8] = pts[7];
                                        pts[9] = pts[7];
                                        pts[10] = pts[7];
                                        pts[11] = pts[7];
                                        pts[12] = pts[7];
                                        pts[13] = pts[7];
                                        pts[14] = pts[7];
                                        pts[15] = pts[7];
                                        pts[16] = pts[7];
                                        pts[17] = new PointF(endright, StartHigh ? LowY : HighY);
                                    }
                                    else
                                    {
                                        if ((prms.CompareType1.Value == Equal_Str && 
                                            (Compare1Value16 <= Compare2Value16)) ||
                                            (Compare1Value16 < Compare2Value16))
                                        {
                                            pts[0] = new PointF(startleft, LowY);
                                            pts[1] = new PointF(Edge2, LowY);
                                            pts[2] = new PointF(Edge2, HighY);
                                            pts[3] = new PointF(Edge4, HighY);
                                            pts[4] = new PointF(Edge4, LowY);
                                            pts[5] = new PointF(Edge2_2, LowY);
                                            pts[6] = new PointF(Edge2_2, HighY);
                                            pts[7] = new PointF(Edge4_2, HighY);
                                            pts[8] = new PointF(Edge4_2, LowY);
                                            pts[9] = new PointF(endright, LowY);
                                            pts[10] = pts[9];
                                            pts[11] = pts[9];
                                            pts[12] = pts[9];
                                            pts[13] = pts[9];
                                            pts[14] = pts[9];
                                            pts[15] = pts[9];
                                            pts[16] = pts[9];
                                            pts[17] = pts[9];
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
                                            pts[12] = pts[1];
                                            pts[13] = pts[1];
                                            pts[14] = pts[1];
                                            pts[15] = pts[1];
                                            pts[16] = pts[1];
                                            pts[17] = pts[1];
                                        }
                                    }
                                    break;
                            }
                        }
                        #endregion
                        #region PWM-CENTER
                        if (prms.PWMMode.Value == Center_Align_Str)
                        {
                            //1 = _____|------|_____    = Greater Than
                            //2 = -----|______|-----    = Less Than
                            Edge1 = startleft + (bitwidth * (float)(CompareValue32));
                            Edge1_2 = startright + (bitwidth * (float)(CompareValue32));
                            Edge2 = startright - (bitwidth * (float)(CompareValue32));
                            Edge2_2 = endright - (bitwidth * (float)(CompareValue32));
                            //Get Compare Type Information for Compare Type 1
                            if (prms.CompareType1.Value == Less_Str || prms.CompareType1.Value == Less_Equal_Str)
                            {
                                StartHigh = true;
                                if (prms.CompareType1.Value == Less_Equal_Str)
                                {
                                    comparestring1 = "<=" + prms.CompareValue1.Value;
                                    Edge1 += (bitwidth / 2f);
                                    Edge1_2 += (bitwidth / 2f);
                                    Edge2 -= (bitwidth / 2f);
                                    Edge2_2 -= (bitwidth / 2f);
                                    //Deal with half bit cycle if equal to period
                                    if (prms.CompareValue1.Value == prms.Period.Value)
                                    {
                                        Edge1 -= (bitwidth / 2f);
                                        Edge1_2 -= (bitwidth / 2f);
                                        Edge2 += (bitwidth / 2f);
                                        Edge2_2 += (bitwidth / 2f);
                                    }
                                }
                                else
                                {
                                    comparestring1 = "<" + prms.CompareValue1.Value;
                                    Edge1 -= (bitwidth / 2f);
                                    Edge1_2 -= (bitwidth / 2f);
                                    Edge2 += (bitwidth / 2f);
                                    Edge2_2 += (bitwidth / 2f);
                                    //Deal with half bit cycle if equal to zero
                                    if (prms.CompareValue1.Value == "0")
                                    {
                                        Edge1 += (bitwidth / 2f);
                                        Edge1_2 += (bitwidth / 2f);
                                        Edge2 -= (bitwidth / 2f);
                                        Edge2_2 -= (bitwidth / 2f);
                                    }
                                }

                                if ((CompareValue32 > PeriodValue32) ||
                                  ((CompareValue32 >= PeriodValue32) &&
                                    prms.CompareType1.Value == Less_Equal_Str))
                                {
                                    pts[0] = new PointF(startleft, StartHigh ? HighY : LowY);
                                    pts[1] = pts[0];
                                    pts[2] = pts[0];
                                    pts[3] = pts[0];
                                    pts[4] = pts[0];
                                    pts[5] = pts[0];
                                    pts[6] = pts[0];
                                    pts[7] = pts[0];
                                    pts[8] = pts[7];
                                    pts[9] = pts[7];
                                    pts[10] = pts[7];
                                    pts[11] = pts[7];
                                    pts[12] = pts[7];
                                    pts[13] = pts[7];
                                    pts[14] = pts[7];
                                    pts[15] = pts[7];
                                    pts[16] = pts[7];
                                    pts[17] = new PointF(endright, StartHigh ? HighY : LowY);
                                }
                                else
                                {
                                    pts[0] = new PointF(startleft, StartHigh ? HighY : LowY);
                                    pts[1] = new PointF(Edge1, StartHigh ? HighY : LowY);
                                    if (CompareValue32 == 0)
                                    {
                                        if (prms.CompareType1.Value == Less_Equal_Str)
                                        {
                                            pts[2] = new PointF(Edge1, StartHigh ? LowY : HighY);
                                            pts[3] = new PointF(Edge2, StartHigh ? LowY : HighY);
                                        }
                                        else
                                        {
                                            pts[2] = new PointF(Edge1, StartHigh ? LowY : HighY);
                                            pts[3] = new PointF(Edge2, StartHigh ? LowY : HighY);
                                        }
                                    }
                                    else
                                    {
                                        pts[2] = new PointF(Edge1, StartHigh ? LowY : HighY);
                                        pts[3] = new PointF(Edge2, StartHigh ? LowY : HighY);
                                    }
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
                                    pts[16] = pts[11];
                                    pts[17] = pts[11];
                                }
                            }
                            else if (prms.CompareType1.Value == Greater_Str || 
                                prms.CompareType1.Value == Greater_Equal_Str)
                            {
                                StartHigh = false;
                                if (prms.CompareType1.Value == Greater_Equal_Str)
                                {
                                    comparestring1 = ">=" + prms.CompareValue1.Value;
                                    Edge1 -= (bitwidth / 2f);
                                    Edge1_2 -= (bitwidth / 2f);
                                    Edge2 += (bitwidth / 2f);
                                    Edge2_2 += (bitwidth / 2f);
                                    if (prms.CompareValue1.Value == "0")
                                    {
                                        Edge1 += (bitwidth / 2f);
                                        Edge1_2 += (bitwidth / 2f);
                                        Edge2 -= (bitwidth / 2f);
                                        Edge2_2 -= (bitwidth / 2f);
                                    }
                                }
                                else
                                {
                                    comparestring1 = ">" + prms.CompareValue1.Value;
                                    Edge1 += (bitwidth / 2f);
                                    Edge1_2 += (bitwidth / 2f);
                                    Edge2 -= (bitwidth / 2f);
                                    Edge2_2 -= (bitwidth / 2f);
                                    //Deal with half bit cycle if equal to period
                                    if (prms.CompareValue1.Value == prms.Period.Value)
                                    {
                                        Edge1 -= (bitwidth / 2f);
                                        Edge1_2 -= (bitwidth / 2f);
                                        Edge2 += (bitwidth / 2f);
                                        Edge2_2 += (bitwidth / 2f);
                                    }
                                }

                                if ((CompareValue32 >= PeriodValue32 && prms.CompareType1.Value != Greater_Equal_Str) ||
                                     ((CompareValue32 > PeriodValue32) && prms.CompareType1.Value == Greater_Equal_Str)
                                    )
                                {
                                    pts[0] = new PointF(startleft, StartHigh ? HighY : LowY);
                                    pts[1] = pts[0];
                                    pts[2] = pts[0];
                                    pts[3] = pts[0];
                                    pts[4] = pts[0];
                                    pts[5] = pts[0];
                                    pts[6] = pts[0];
                                    pts[7] = pts[0];
                                    pts[8] = pts[7];
                                    pts[9] = pts[7];
                                    pts[10] = pts[7];
                                    pts[11] = pts[7];
                                    pts[12] = pts[7];
                                    pts[13] = pts[7];
                                    pts[14] = pts[7];
                                    pts[15] = pts[7];
                                    pts[16] = pts[7];
                                    pts[17] = new PointF(endright, StartHigh ? HighY : LowY);
                                }
                                else
                                {
                                    pts[0] = new PointF(startleft, StartHigh ? HighY : LowY);
                                    pts[1] = new PointF(Edge1, StartHigh ? HighY : LowY);
                                    if (CompareValue32 == 0)
                                    {
                                        if (prms.CompareType1.Value == Greater_Equal_Str)
                                        {
                                            pts[2] = new PointF(Edge1, StartHigh ? LowY : HighY);
                                            pts[3] = new PointF(startright, StartHigh ? LowY : HighY);
                                        }
                                        else
                                        {
                                            pts[2] = new PointF(Edge1, StartHigh ? LowY : HighY);
                                            pts[3] = new PointF(Edge2, StartHigh ? LowY : HighY);
                                        }
                                    }
                                    else
                                    {
                                        pts[2] = new PointF(Edge1, StartHigh ? LowY : HighY);
                                        pts[3] = new PointF(Edge2, StartHigh ? LowY : HighY);
                                    }
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
                                    pts[16] = pts[11];
                                    pts[17] = pts[11];
                                }
                            }
                            else if ((CompareValue32 > PeriodValue32)
                                  && (prms.CompareType1.Value == Equal_Str))
                            {
                                StartHigh = true;
                                Edge1 += bitwidth;
                                Edge1_2 += bitwidth;
                                pts[0] = new PointF(startleft, StartHigh ? LowY : HighY);
                                pts[1] = pts[0];
                                pts[2] = pts[0];
                                pts[3] = pts[0];
                                pts[4] = pts[0];
                                pts[5] = pts[0];
                                pts[6] = pts[0];
                                pts[7] = pts[0];
                                pts[8] = pts[7];
                                pts[9] = pts[7];
                                pts[10] = pts[7];
                                pts[11] = pts[7];
                                pts[12] = pts[7];
                                pts[13] = pts[7];
                                pts[14] = pts[7];
                                pts[15] = pts[7];
                                pts[16] = pts[7];
                                pts[17] = new PointF(endright, StartHigh ? LowY : HighY);
                            }
                            else
                            {
                                comparestring1 = "==" + prms.CompareValue1.Value;
                                if (prms.CompareValue1.Value == prms.Period.Value)
                                {
                                    Edge1 -= (bitwidth / 2f);
                                    Edge2 = Edge1 + bitwidth;
                                    Edge3 = Edge2;
                                    Edge4 = Edge2;
                                    Edge1_2 -= (bitwidth / 2f);
                                    Edge2_2 = Edge1_2 + bitwidth;
                                    Edge3_2 = Edge2_2;
                                    Edge4_2 = Edge2_2;
                                }
                                else if (prms.CompareValue1.Value == "0")
                                {
                                    Edge1 = startleft;
                                    Edge3 = startleft + (bitwidth / 2f);
                                    Edge2 = startright - (bitwidth / 2f);
                                    Edge4 = startright + (bitwidth / 2f);
                                    Edge1_2 = startright + (bitwidth / 2f);
                                    Edge3_2 = startright + (bitwidth / 2f);
                                    Edge2_2 = endright - (bitwidth / 2f);
                                    Edge4_2 = endright;
                                }
                                else
                                {
                                    StartHigh = false;
                                    Edge1 -= (bitwidth / 2f);
                                    Edge1_2 -= (bitwidth / 2f);
                                    Edge2 -= (bitwidth / 2f);
                                    Edge2_2 -= (bitwidth / 2f);
                                    Edge3 = Edge1 + bitwidth;
                                    Edge3_2 = Edge1_2 + bitwidth;
                                    Edge4 = Edge2 + bitwidth;
                                    Edge4_2 = Edge2_2 + bitwidth;
                                }
                                pts[0] = new PointF(startleft, LowY);
                                pts[1] = new PointF(Edge1, LowY);
                                pts[2] = new PointF(Edge1, HighY);
                                pts[3] = new PointF(Edge3, HighY);
                                pts[4] = new PointF(Edge3, LowY);
                                pts[5] = new PointF(Edge2, LowY);
                                pts[6] = new PointF(Edge2, HighY);
                                pts[7] = new PointF(Edge4, HighY);
                                pts[8] = new PointF(Edge4, LowY);
                                pts[9] = new PointF(Edge1_2, LowY);
                                pts[10] = new PointF(Edge1_2, HighY);
                                pts[11] = new PointF(Edge3_2, HighY);
                                pts[12] = new PointF(Edge3_2, LowY);
                                pts[13] = new PointF(Edge2_2, LowY);
                                pts[14] = new PointF(Edge2_2, HighY);
                                pts[15] = new PointF(Edge4_2, HighY);
                                pts[16] = new PointF(Edge4_2, LowY);
                                pts[17] = new PointF(endright, LowY);
                            }

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
                        if (prms.PWMMode.Value == Hardware_Select_Str)
                        {
                            Edge1 = (2 * PB_EXTENTS_BORDER) + PB_PWMTEXT_WIDTH;
                            comparestring1 = COMPARESTRING;
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
                            pts[16] = pts[7];
                            pts[17] = pts[7];
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
                        if (prms.PWMMode.Value == Dither_Str)
                        {
                            Edge1 = startleft + (bitwidth * (float)(PeriodValue32 - CompareValue32 + 1));
                            Edge1_2 = startright + (bitwidth * (float)(PeriodValue32 - CompareValue32 + 1));
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

                            if (CompareValue32 > PeriodValue32)
                            {
                                StartHigh = true;
                                Edge1 += bitwidth;
                                Edge1_2 += bitwidth;
                                pts[0] = new PointF(startleft, StartHigh ? LowY : HighY);
                                pts[1] = pts[0];
                                pts[2] = pts[0];
                                pts[3] = pts[0];
                                pts[4] = pts[0];
                                pts[5] = pts[0];
                                pts[6] = pts[0];
                                pts[7] = pts[0];
                                pts[8] = pts[7];
                                pts[9] = pts[7];
                                pts[10] = pts[7];
                                pts[11] = pts[7];
                                pts[12] = pts[7];
                                pts[13] = pts[7];
                                pts[14] = pts[7];
                                pts[15] = pts[7];
                                pts[16] = pts[7];
                                pts[17] = new PointF(endright, StartHigh ? LowY : HighY);
                            }
                            else
                            {
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
                                pts[16] = new PointF(endright, StartHigh ? LowY : HighY);
                                pts[17] = new PointF(endright, StartHigh ? LowY : HighY);
                            }
                            PointF[] pts2 = new PointF[4];
                            pts2[0] = pts[1];
                            pts2[1] = pts[2];
                            pts2[2] = pts[3];
                            pts2[3] = pts[4];
                            wfg.FillPolygon(redbrush, pts2);
                            pts2[0] = pts[9];
                            pts2[1] = pts[10];
                            pts2[2] = pts[11];
                            pts2[3] = pts[12];
                            wfg.FillPolygon(redbrush, pts2);
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
                    case PH1:
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
                        pts[16] = pts[11];
                        pts[17] = pts[11];

                        //Check to see if any errors where X2 is less than X1
                        for (j = 0; j < pts.Length - 1; j++)
                        {
                            if (pts[j + 1].X < pts[j].X)
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
                                pts[12] = pts[1];
                                pts[13] = pts[1];
                                pts[14] = pts[1];
                                pts[15] = pts[1];
                                pts[16] = pts[1];
                                pts[17] = pts[1];
                                break;
                            }
                        }
                        break;
                    #endregion
                    #region PH2
                    case PH2:
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
                        pts[16] = pts[11];
                        pts[17] = pts[11];

                        //Check to see if any errors where X2 is less than X1
                        for (k = 0; k < pts.Length - 1; k++)
                        {
                            if (pts[k + 1].X < pts[k].X)
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
                                pts[12] = pts[1];
                                pts[13] = pts[1];
                                pts[14] = pts[1];
                                pts[15] = pts[1];
                                pts[16] = pts[1];
                                pts[17] = pts[1];
                                break;
                            }
                        }
                        break;
                    #endregion
                }
                //Draw the Waveform
                SolidBrush wfbrush = new SolidBrush(Color.Blue);
                Pen wfPen = new Pen(wfbrush);
                wfg.DrawLines(wfPen, pts);
            }
            wfg.Dispose();
            blkbrush.Dispose();
            whitebrush.Dispose();
            greybrush.Dispose();
            redbrush.Dispose();

            m_pbDrawing.Image = waveform;
        }

        private void CyPWMControl_SizeChanged(object sender, EventArgs e)
        {
            UpdateDrawing(new PWMParameters(m_InstEdit));
        }
        #endregion

        #region Control Event Handlers
        #region Period Numeric Up_Down
        private void m_numPeriod_ValueChanged(object sender, EventArgs e)
        {
            CancelEventArgs ce = new CancelEventArgs();
            m_numPeriod_Validating(sender, ce);
            UInt16 tmp = PeriodDefaultValue;
            try
            {
                tmp = Convert.ToUInt16(m_numPeriod.Value);
            }
            catch (OverflowException)
            {
                string error_str = String.Format("{0} is outside the range of UInt16 type "
                    + m_numPeriod.Value.ToString());
                ep_Errors.SetError(m_numPeriod, error_str);
            }
            catch (FormatException)
            {
                string error_str = String.Format("{0} is not in a recognizable format."
                    + m_numPeriod.Value.ToString());
                ep_Errors.SetError(m_numPeriod, error_str);
            }

            if (!ce.Cancel)
            {
                SetAParameter("Period", m_numPeriod.Value.ToString(), false);
                SetFrequencyLabel(tmp);
            }
            UpdateDrawing(new PWMParameters(m_InstEdit));
        }

        private void m_numPeriod_KeyUp(object sender, KeyEventArgs e)
        {
            m_numPeriod_Validating(sender, new CancelEventArgs());
            
        }

        private void m_numPeriod_Validating(object sender, CancelEventArgs e)
        {
            if (m_rbResolution16.Checked)
            {
                
                if (m_cbPWMMode.SelectedIndex == Center_Align_Int)
                {
                    if (m_numPeriod.Value < PERIODMIN || m_numPeriod.Value > PERIODMAX16CA)
                    {
                        ep_Errors.SetError(m_numPeriod, String.Format("Period must be between 0 and {0}",
                            PERIODMAX16CA));
                        e.Cancel = true;
                    }
                    else
                        ep_Errors.SetError(m_numPeriod, "");
                }
                else
                {
                    if (m_numPeriod.Value < PERIODMIN || m_numPeriod.Value > PERIODMAX16)
                    {
                        ep_Errors.SetError(m_numPeriod, String.Format("Period must be between 0 and {0}",
                            PERIODMAX16));
                        e.Cancel = true;
                    }
                    else
                        ep_Errors.SetError(m_numPeriod, "");
                }
            }
            else
            {
                if (m_cbPWMMode.SelectedIndex == Center_Align_Int)
                {
                    if (m_numPeriod.Value < PERIODMIN || m_numPeriod.Value > PERIODMAX8CA)
                    {
                        ep_Errors.SetError(m_numPeriod, String.Format("Period must be between 0 and {0}",
                            PERIODMAX8CA));
                        e.Cancel = true;
                    }
                    else
                        ep_Errors.SetError(m_numPeriod, "");
                }
                else
                {
                    if (m_numPeriod.Value < PERIODMIN || m_numPeriod.Value > PERIODMAX8)
                    {
                        ep_Errors.SetError(m_numPeriod, String.Format("Period must be between 0 and {0}",
                            PERIODMAX8));
                        e.Cancel = true;
                    }
                    else
                        ep_Errors.SetError(m_numPeriod, "");
                }
            }
            
        }

        void m_numPeriod_DownEvent(object sender, DownButtonEventArgs e)
        {
            if (m_numPeriod.Value == PERIODMIN)
                e.Cancel = true;
        }

        void m_numPeriod_UpEvent(object sender, UpButtonEventArgs e)
        {
            if (m_rbResolution16.Checked)
            {
                if (m_cbPWMMode.SelectedIndex == Center_Align_Int)
                {
                    if (m_numPeriod.Value == PERIODMAX16CA)
                        e.Cancel = true;
                }
                else
                {
                    if (m_numPeriod.Value == PERIODMAX16)
                        e.Cancel = true;
                }
            }
            if (m_rbResolution8.Checked)
            {
                if (m_cbPWMMode.SelectedIndex == Center_Align_Int)
                {
                    if (m_numPeriod.Value == PERIODMAX8CA)
                        e.Cancel = true;
                }
                else
                {
                    if (m_numPeriod.Value == PERIODMAX8)
                        e.Cancel = true;
                }
            }
        }
        #endregion
        private void Set_Control_Visibility_Compare_Type()
        {
            if (m_cbPWMMode.SelectedIndex == Two_Output_Int || m_cbPWMMode.SelectedIndex == Dual_Edge_Int
                || m_cbPWMMode.SelectedIndex == Hardware_Select_Int)
            {
                m_numCompare2.Visible = true;
                m_lblCmpValue2.Visible = true;
                m_lblCmpType2.Visible = true;
                m_cbCompareType2.Visible = true;
                m_cbDitherOffset.Visible = false;
                m_cbDitherAlign.Visible = false;
                m_cbCompareType1.Visible = true;
                m_lblCmpType1.Text = CMP_TYPE_1;
            }
            else if (m_cbPWMMode.SelectedIndex == One_Output_Int || m_cbPWMMode.SelectedIndex == Center_Align_Int)
            {
                m_numCompare2.Visible = false;
                m_lblCmpValue2.Visible = false;
                m_lblCmpType2.Visible = false;
                m_cbCompareType2.Visible = false;
                m_cbDitherOffset.Visible = false;
                m_cbDitherAlign.Visible = false;
                m_cbCompareType1.Visible = true;
                m_lblCmpType1.Text = CMP_TYPE_1;
            }
            else if (m_cbPWMMode.SelectedIndex == Dither_Int)
            {
                m_numCompare2.Visible = false;
                m_lblCmpValue2.Visible = false;
                m_lblCmpType2.Visible = false;
                m_cbCompareType2.Visible = false;
                m_cbDitherOffset.Visible = true;
                m_cbDitherAlign.Visible = true;
                m_cbCompareType1.Visible = false;
                m_lblCmpType1.Text = ALIGNMENT;
                SetAParameter(COMPAREVALUE2, Convert.ToString(m_numCompare1.Value + 1), false);
                m_numCompare2.Value = m_numCompare1.Value + 1;
            }
        }
        private void m_cbPWMMode_SelectedIndexChanged(object sender, EventArgs e)
        {
            string prm = m_InstEdit.ResolveEnumDisplayToId(PWMMODE, m_cbPWMMode.Text);
            SetAParameter(PWMMODE, prm, true);
            Set_Control_Visibility_Compare_Type();
            m_numPeriod_Validating(sender, new CancelEventArgs());
            m_numCompare1_Validating(sender, new CancelEventArgs());
            m_numCompare2_Validating(sender, new CancelEventArgs());
            UpdateDrawing(new PWMParameters(m_InstEdit));
        }

        #region Compare1 Numeric Up_Down
        private void m_numCompare1_ValueChanged(object sender, EventArgs e)
        {
            CancelEventArgs ce = new CancelEventArgs();
            m_numCompare1_Validating(sender, ce);
            if (!ce.Cancel)
            {
                SetAParameter(COMPAREVALUE1, m_numCompare1.Value.ToString(), false);
                UpdateDrawing(new PWMParameters(m_InstEdit));
            }
        }

        private void m_numCompare1_Validating(object sender, CancelEventArgs e)
        {
            int maxAllowed = PERIODMAX8;

            if (m_rbResolution16.Checked)
            {
                maxAllowed = PERIODMAX16;
            }

            if (m_numCompare1.Value < 0 || m_numCompare1.Value > maxAllowed)
            {
                ep_Errors.SetError(m_numCompare1, string.Format("Compare Value must be between 0 and {0}",
                    maxAllowed));
                e.Cancel = true;
            }
            else
            {
                ep_Errors.SetError(m_numCompare1, "");
            }
        }

        private void m_numCompare1_KeyUp(object sender, KeyEventArgs e)
        {
            m_numCompare1_Validating(sender, new CancelEventArgs());
        }

        void m_numCompare1_DownEvent(object sender, DownButtonEventArgs e)
        {
            if (m_numCompare1.Value == PERIODMIN)
                e.Cancel = true;
        }

        void m_numCompare1_UpEvent(object sender, UpButtonEventArgs e)
        {
            if (m_numCompare1.Value == m_numPeriod.Value)
            {
                e.Cancel = true;
            }
            else if (m_rbResolution16.Checked)
            {
                if (m_cbPWMMode.SelectedIndex == Center_Align_Int)
                {
                    if (m_numCompare1.Value == PERIODMAX16CA)
                        e.Cancel = true;
                }
                else
                {
                    if (m_numCompare1.Value == PERIODMAX16)
                        e.Cancel = true;
                }
            }
            else
            {
                if (m_cbPWMMode.SelectedIndex == Center_Align_Int)
                {
                    if (m_numCompare1.Value == PERIODMAX8CA)
                        e.Cancel = true;
                }
                else
                {
                    if (m_numCompare1.Value == PERIODMAX8)
                        e.Cancel = true;
                }
            }
        }
        #endregion

        #region Compare2 Numeric Up_Down
        private void m_numCompare2_ValueChanged(object sender, EventArgs e)
        {
            CancelEventArgs ce = new CancelEventArgs();
            m_numCompare2_Validating(sender, ce);
            if (!ce.Cancel)
            {
                SetAParameter(COMPAREVALUE2, m_numCompare2.Value.ToString(), false);
                UpdateDrawing(new PWMParameters(m_InstEdit));
            }
        }

        private void m_numCompare2_Validating(object sender, CancelEventArgs e)
        {
            if (m_numCompare2.Visible)
            {
                int maxAllowed = PERIODMAX8;

                if (m_rbResolution16.Checked)
                {
                    maxAllowed = PERIODMAX16;
                }

                if (m_numCompare2.Value < 0 || m_numCompare2.Value > maxAllowed)
                {
                    ep_Errors.SetError(m_numCompare2, string.Format("Compare Value must be between 0 and the {0}"
                        , maxAllowed));
                    e.Cancel = true;
                }
                else
                {
                    ep_Errors.SetError(m_numCompare2, "");
                }
            }
        }

        private void m_numCompare2_KeyUp(object sender, KeyEventArgs e)
        {
            m_numCompare2_Validating(sender, new CancelEventArgs());
        }

        void m_numCompare2_DownEvent(object sender, DownButtonEventArgs e)
        {
            if (m_numCompare2.Value == PERIODMIN)
                e.Cancel = true;
        }

        void m_numCompare2_UpEvent(object sender, UpButtonEventArgs e)
        {
            if (m_numCompare2.Value == m_numPeriod.Value)
            {
                e.Cancel = true;
            }
            else if (m_rbResolution16.Checked)
            {
                if (m_cbPWMMode.SelectedIndex == Center_Align_Int)
                {
                    if (m_numCompare2.Value == PERIODMAX16CA)
                        e.Cancel = true;
                }
                else
                {
                    if (m_numCompare2.Value == PERIODMAX16)
                        e.Cancel = true;
                }
            }
            else
            {
                if (m_cbPWMMode.SelectedIndex == Center_Align_Int)
                {
                    if (m_numCompare2.Value == PERIODMAX8CA)
                        e.Cancel = true;
                }
                else
                {
                    if (m_numCompare2.Value == PERIODMAX8)
                        e.Cancel = true;
                }
            }
        }
        #endregion

        private void m_cbCompareType1_SelectedIndexChanged(object sender, EventArgs e)
        {
            CancelEventArgs ce = new CancelEventArgs();
            string prm = m_InstEdit.ResolveEnumDisplayToId(COMPARETYPE1, m_cbCompareType1.Text);
            m_cbCompareType1_Validating(null,ce);
            if (ce.Cancel) return;
            if (m_rbFixedFunction.Checked == true)
                m_cbFFDeadBandMode_Validating(null,ce);
            else
                m_cbDeadBandMode_Validating(null,ce);
            SetAParameter(COMPARETYPE1, prm, true);
            UpdateDrawing(new PWMParameters(m_InstEdit));
        }

        private void m_cbCompareType2_SelectedIndexChanged(object sender, EventArgs e)
        {
            string prm = m_InstEdit.ResolveEnumDisplayToId(COMPARETYPE2, m_cbCompareType2.Text);
            SetAParameter(COMPARETYPE2, prm, true);
            UpdateDrawing(new PWMParameters(m_InstEdit));
        }

        private void m_rbResolution8_Click(object sender, EventArgs e)
        {
            if (!m_rbResolution8.Checked)
            {
                m_rbResolution8.Checked = true;
                m_rbResolution16.Checked = false;
                SetAParameter(RESOLUTION, "8", true); //TODO: Use Enumerate Type
                UpdateDrawing(new PWMParameters(m_InstEdit));
            }
            m_numPeriod_Validating(sender, new CancelEventArgs());
            m_numCompare1_Validating(sender, new CancelEventArgs());
            m_numCompare2_Validating(sender, new CancelEventArgs());
        }

        private void m_rbResolution16_Click(object sender, EventArgs e)
        {
            if (!m_rbResolution16.Checked)
            {
                m_rbResolution8.Checked = false;
                m_rbResolution16.Checked = true;
                SetAParameter(RESOLUTION, "16", true); //TODO: Use Enumerate Type
                UpdateDrawing(new PWMParameters(m_InstEdit));
            }
            m_numPeriod_Validating(sender, new CancelEventArgs());
            m_numCompare1_Validating(sender, new CancelEventArgs());
            m_numCompare2_Validating(sender, new CancelEventArgs());
        }

        private void m_cbDitherAlign_SelectedIndexChanged(object sender, EventArgs e)
        {
            PWMParameters prms = new PWMParameters(m_InstEdit);
            if (prms.PWMMode.Value == Dither_Str)
            {
                //Get the Enumerated strings for LessThanOrEqual an GreaterThanOrEqual
                IEnumerable<string> CmpTypeEnums = m_InstEdit.GetPossibleEnumValues(COMPARETYPE1);
                string LessThan = null;
                string GreaterThan = null;
                foreach (string str in CmpTypeEnums)
                {
                    if (str.Contains(EQUAL) && str.Contains(LESS))
                        LessThan = str;
                    if (str.Contains(EQUAL) && str.Contains(GREATER))
                        GreaterThan = str;
                }
                if (m_cbDitherAlign.SelectedIndex == 0) /*Left Aligned*/
                {
                    SetAParameter(COMPARETYPE1, m_InstEdit.ResolveEnumDisplayToId(COMPARETYPE1, GreaterThan), false);
                    SetAParameter(COMPARETYPE2, m_InstEdit.ResolveEnumDisplayToId(COMPARETYPE1, GreaterThan), false);
                }
                else /* Right Aligned*/
                {
                    SetAParameter(COMPARETYPE1, m_InstEdit.ResolveEnumDisplayToId(COMPARETYPE2, LessThan), false);
                    SetAParameter(COMPARETYPE2, m_InstEdit.ResolveEnumDisplayToId(COMPARETYPE2, LessThan), false);
                }
                UpdateDrawing(new PWMParameters(m_InstEdit));
            }
        }

        private void m_cbDitherOffset_SelectedIndexChanged(object sender, EventArgs e)
        {
            string prm = m_InstEdit.ResolveEnumDisplayToId(DITHEROFFSET, m_cbDitherOffset.Text);
            SetAParameter(DITHEROFFSET, prm, false);
            UpdateDrawing(new PWMParameters(m_InstEdit));
        }

        private void m_cbDeadBandMode_SelectedIndexChanged(object sender, EventArgs e)
        {
            CancelEventArgs ce = new CancelEventArgs();
            string prm = m_InstEdit.ResolveEnumDisplayToId(DEADBAND, m_cbDeadBandMode.Text);
            m_cbDeadBandMode_Validating(null,ce); 
            if (ce.Cancel) return;
            SetAParameter(DEADBAND, prm, true);
            if (m_cbDeadBandMode.Visible)
            {
                if (m_cbDeadBandMode.SelectedIndex == Disabled)
                {
                    m_numDeadBandCounts.Enabled = false;
                }
                else if (m_cbDeadBandMode.SelectedIndex == Clock1)
                {
                    m_numDeadBandCounts.Enabled = true;
                }
                else if (m_cbDeadBandMode.Visible)
                {
                    m_numDeadBandCounts.Enabled = true;
                }
            }
            UpdateDrawing(new PWMParameters(m_InstEdit));
            m_numDeadBandCounts_Validating(sender, new CancelEventArgs());
        }

        #region DeadBandCounts Numeric Up_Down
        private void m_numDeadBandCounts_ValueChanged(object sender, EventArgs e)
        {
            CancelEventArgs ce = new CancelEventArgs();
            m_numDeadBandCounts_Validating(sender, ce);
            if (!ce.Cancel)
            {
                if (m_cbDeadBandMode.Enabled)
                    SetAParameter(DEADTIME, (m_numDeadBandCounts.Value - 1).ToString(), false);
                else
                    SetAParameter(DEADTIME, (m_numDeadBandCounts.Value).ToString(), false);
            }
            UpdateDrawing(new PWMParameters(m_InstEdit));
        }

        private void m_numDeadBandCounts_Validating(object sender, CancelEventArgs e)
        {
            decimal newval = m_numPeriod.Value - m_numCompare1.Value;
            if (((newval <= m_numDeadBandCounts.Value) || (m_numDeadBandCounts.Value > m_numCompare1.Value))
                && (m_numDeadBandCounts.Enabled == true))
            {
                ep_Errors.SetError(m_numDeadBandCounts, "DeadBand counts is exceeding the difference between the period and the compare forcing an All Zero on PH1 or PH2");
                return;
            }
            else
            {
                ep_Errors.SetError(m_numDeadBandCounts, "");
            }

            if (m_cbFFDeadBandMode.Visible)
            {
                if (m_cbFFDeadBandMode.SelectedIndex == Disabled)
                {
                    ep_Errors.SetError(m_numDeadBandCounts, "");
                    e.Cancel = true;
                }
                else if (m_numDeadBandCounts.Value < 0 || m_numDeadBandCounts.Value > 3)
                {
                    ep_Errors.SetError(m_numDeadBandCounts, "Dead Band Counts Must Be Between 0 and 3 for Fixed Function Implementation");
                    e.Cancel = true;
                }
                else
                {
                    ep_Errors.SetError(m_numDeadBandCounts, "");
                }
                return;
            }
            if (m_cbDeadBandMode.SelectedIndex == Disabled)
            {
                ep_Errors.SetError(m_numDeadBandCounts, "");
                e.Cancel = true;
                return;
            }
            if (m_cbDeadBandMode.SelectedIndex == Clock1)
            {
                /* Verify not exceeding minimum or maximum of this mode */
                if (m_numDeadBandCounts.Value < DEADBANDCOUNTS_MINIMUM || m_numDeadBandCounts.Value > DEADBANDCOUNTS_4MAXIMUM)
                {
                    ep_Errors.SetError(m_numDeadBandCounts, string.Format("Dead Band Counts Must Be Between 1 and 4"));
                    e.Cancel = true;
                    return;
                }
            }
            else
            {
                /* Verify not exceeding minimum or maximum of this mode */
                if (m_numDeadBandCounts.Value < DEADBANDCOUNTS_MINIMUM || m_numDeadBandCounts.Value > DEADBANDCOUNTS_256MAXIMUM)
                {
                    ep_Errors.SetError(m_numDeadBandCounts, string.Format("Dead Band Counts Must Be Between 1 and 256"));
                    e.Cancel = true;
                    return;
                }
            }
            ep_Errors.SetError(m_numDeadBandCounts, "");
        }

        private void m_numDeadBandCounts_KeyUp(object sender, KeyEventArgs e)
        {
            m_numDeadBandCounts_Validating(sender, new CancelEventArgs());
        }

        void m_numDeadBandCounts_DownEvent(object sender, DownButtonEventArgs e)
        {
            if (m_cbFFDeadBandMode.Visible)
            {
                if (m_numDeadBandCounts.Value == DEADBANDFFCOUNTS_MINIMUM)
                {
                    e.Cancel = true;
                    return;
                }
                else
                    return;
            }
            if (m_numDeadBandCounts.Value == DEADBANDCOUNTS_MINIMUM)
            {
                e.Cancel = true;
                return;
            }
        }

        void m_numDeadBandCounts_UpEvent(object sender, UpButtonEventArgs e)
        {
            if (m_cbFFDeadBandMode.Visible)
            {
                if (m_numDeadBandCounts.Value == DEADBANDFFCOUNTS_MAXIMUM)
                {
                    e.Cancel = true;
                    return;
                }
                else
                    return;
            }
            if (m_cbDeadBandMode.SelectedIndex == Clock1)  /* If using 2-4 DB Counts mode */
            {
                /* Verify not exceeding minimum or maximum of this mode */
                if (m_numDeadBandCounts.Value == DEADBANDCOUNTS_4MAXIMUM)
                {
                    e.Cancel = true;
                    return;
                }
            }
            else
            {
                /* Verify not exceeding minimum or maximum of this mode */
                if (m_numDeadBandCounts.Value == DEADBANDCOUNTS_256MAXIMUM)
                {
                    e.Cancel = true;
                    return;
                }
            }
        }
        #endregion

        private void m_cbFFDeadBandMode_SelectedIndexChanged(object sender, EventArgs e)
        {
            CancelEventArgs ce = new CancelEventArgs();
            IEnumerable<string> DeadBandEnums = m_InstEdit.GetPossibleEnumValues(DEADBAND);
            string DeadBandDisabled = null;
            string DeadBand24 = null;
            m_cbFFDeadBandMode_Validating(null,ce);
            if (ce.Cancel) return;
            if (m_cbFFDeadBandMode.Visible)
            {
                if (m_cbFFDeadBandMode.SelectedIndex == Disabled)
                    m_numDeadBandCounts.Enabled = false;
                else
                    m_numDeadBandCounts.Enabled = true;
            }
            foreach (string str in DeadBandEnums)
            {
                if (str.Contains(DISABLE))
                    DeadBandDisabled = str;
                if (str.Contains("4"))
                    DeadBand24 = str;
            }
            if (m_cbFFDeadBandMode.SelectedIndex == 0)
                SetAParameter(DEADBAND, m_InstEdit.ResolveEnumDisplayToId(DEADBAND, DeadBandDisabled), true);
            else
                SetAParameter(DEADBAND, m_InstEdit.ResolveEnumDisplayToId(DEADBAND, DeadBand24), true);
            m_numDeadBandCounts_Validating(sender, new CancelEventArgs());
            UpdateDrawing(new PWMParameters(m_InstEdit));
        }
        #endregion

        private void SetFixedFunction()
        {
            //Get the Enumerated strings for PWMMode One Output an DeadBandDisabled
            IEnumerable<string> PWMModeEnums = m_InstEdit.GetPossibleEnumValues(PWMMODE);
            string PWMOneOutput = null;
            foreach (string str in PWMModeEnums)
            {
                if (str.Contains(ONE))
                    PWMOneOutput = str;
            }
            IEnumerable<string> DeadBandEnums = m_InstEdit.GetPossibleEnumValues(DEADBAND);
            string DeadBandFF = null;
            foreach (string str in DeadBandEnums)
            {
                if (str.Contains(DISABLE))
                    DeadBandFF = str;
            }
            //Hide all of the fixed function block limitations
            m_cbPWMMode.SelectedIndex = 0;
            m_cbPWMMode.Enabled = false;
            string prm = m_InstEdit.ResolveEnumDisplayToId(PWMMODE, PWMOneOutput);
            SetAParameter(PWMMODE, prm, true);
            if (m_cbDeadBandMode.SelectedIndex == 0)
            {
                prm = m_InstEdit.ResolveEnumDisplayToId(DEADBAND, DeadBandFF);
                SetAParameter(DEADBAND, prm, true);
                m_cbFFDeadBandMode.SelectedIndex = 0;
            }
            m_cbDeadBandMode.Enabled = false;
            m_cbDeadBandMode.Visible = false;
            m_cbFFDeadBandMode.Visible = true;
            m_cbDeadBandMode.Visible = false;
            if (m_cbFFDeadBandMode.SelectedIndex == Disabled)
            {
                m_numDeadBandCounts.Enabled = false;
            }
            else
            {
                CancelEventArgs ce = new CancelEventArgs();
                m_cbFFDeadBandMode_Validating(null,ce);
                if (ce.Cancel) return;
                m_numDeadBandCounts.Enabled = true;
            }
            m_numDeadBandCounts_Validating(new Object(), new CancelEventArgs());
        }

        private void ClearFixedFunction()
        {
            CancelEventArgs ce = new CancelEventArgs();
            m_cbPWMMode.Enabled = true;
            m_cbDeadBandMode.Enabled = true;
            m_cbFFDeadBandMode.Visible = false;
            m_cbDeadBandMode.Visible = true;
            if (m_cbDeadBandMode.SelectedIndex == Disabled)
                m_numDeadBandCounts.Enabled = false;
            else
            {
                m_cbDeadBandMode_Validating(null,ce);
                if (ce.Cancel) return;
                m_numDeadBandCounts.Enabled = true;
            }
            m_numDeadBandCounts_Validating(new Object(), new CancelEventArgs());
        }

        private void SetAParameter(string parameter, string value, bool CheckFocus)
        {
            if (this.ContainsFocus || !CheckFocus)
            {
                m_InstEdit.SetParamExpr(parameter, value);
                m_InstEdit.CommitParamExprs();
                if (m_InstEdit.GetCommittedParam(parameter).ErrorCount != 0)
                {
                    string errors = null;
                    foreach (string err in m_InstEdit.GetCommittedParam(parameter).Errors)
                    {
                        errors = errors + err + "\n";
                    }
                    MessageBox.Show(string.Format("Error Setting Parameter {0} with value {1}\n\nErrors:\n{2}", parameter, value, errors),
                        "Error Setting Parameter", MessageBoxButtons.OK, MessageBoxIcon.Error);
                }
            }
        }

        public bool IsFixedFunction()
        {
            return m_rbFixedFunction.Checked;
        }

        private void m_bMaxPeriod_Click(object sender, EventArgs e)
        {
            if (m_rbResolution8.Checked)
                m_numPeriod.Value = PERIODMAX8;
            else
                m_numPeriod.Value = PERIODMAX16;
        }

        private void m_rbFixedFunction_Click(object sender, EventArgs e)
        {
            value1 = m_control_advanced.m_cbEnableMode.SelectedIndex;
            value2 = m_control_advanced.m_cbRunMode.SelectedIndex;
            value3 = m_control_advanced.m_cbTriggerMode.SelectedIndex;
            value4 = m_control_advanced.m_cbKillMode.SelectedIndex;
            value5 = m_control_advanced.m_cbCaptureMode.SelectedIndex;
            value6 = m_cbPWMMode.SelectedIndex;
            if (!m_rbFixedFunction.Checked)
            {
                m_rbFixedFunction.Checked = true;
                m_rbUDB.Checked = false;
                SetAParameter(FIXEDFUNCTION, "true", false);
                if (m_control_advanced != null)
                {
                    m_control_advanced.SetFixedFunction();
                }
            }
        }

        private void m_rbUDB_Click(object sender, EventArgs e)
        {
            m_control_advanced.m_cbEnableMode.SelectedIndex = value1;
            m_control_advanced.m_cbRunMode.SelectedIndex = value2;
            m_control_advanced.m_cbTriggerMode.SelectedIndex = value3;
            m_control_advanced.m_cbKillMode.SelectedIndex = value4;
            m_control_advanced.m_cbCaptureMode.SelectedIndex = value5;
            m_cbPWMMode.SelectedIndex = value6;
            if (!m_rbUDB.Checked)
            {
                m_rbFixedFunction.Checked = false;
                m_rbUDB.Checked = true;
                SetAParameter(FIXEDFUNCTION, "false", false);
                if (m_control_advanced != null)
                {
                    m_control_advanced.ClearFixedFunction();
                }
            }
        }
        public void UpdateClock(ICyInstQuery_v1 edit, ICyTerminalQuery_v1 termQuery)
        {
            SetFrequencyLabel(Convert.ToInt16(m_numPeriod.Value));
        } 
     
        private void m_cbCompareType1_Validating(object sender, CancelEventArgs e)
        {   
            if (m_rbFixedFunction.Checked == true)
            {
                if (m_cbCompareType1.SelectedIndex == 4 && m_cbFFDeadBandMode.SelectedIndex != Disabled)
                {
                    ep_Errors.SetError(m_cbCompareType1, "CompareType1 Equal not supported for Dead Band Mode");
                    e.Cancel = true;
                }
                else
                {
                    ep_Errors.SetError(m_cbCompareType1, "");
                }
            }
            else
            {
                if (m_cbCompareType1.SelectedIndex == 4 && m_cbDeadBandMode.SelectedIndex != Disabled)
                {
                    ep_Errors.SetError(m_cbCompareType1, "CompareType1 Equal not supported for Dead Band Mode");
                    e.Cancel = true;
                }
                else
                {
                    ep_Errors.SetError(m_cbCompareType1, "");
                }
            }
        }

        private void m_cbFFDeadBandMode_Validating(object sender, CancelEventArgs e)
        {
            if (m_cbFFDeadBandMode.SelectedIndex != Disabled && m_cbCompareType1.SelectedIndex == 4 )
            {
                ep_Errors.SetError(m_cbFFDeadBandMode, "Dead Band mode not supported for CompareType1 set to Equal");
                e.Cancel = true;
            }
            else
            {
                ep_Errors.SetError(m_cbFFDeadBandMode, "");
            }
        }

        private void m_cbDeadBandMode_Validating(object sender, CancelEventArgs e)
        {
            if(m_cbCompareType1.SelectedIndex == 4 &&  m_cbDeadBandMode.SelectedIndex != Disabled)
            {
                ep_Errors.SetError(m_cbDeadBandMode, "Dead Band mode not supported for CompareType1 set to Equal");
                e.Cancel = true;
            }
            else
            {
                ep_Errors.SetError(m_cbDeadBandMode, "");
            }
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
