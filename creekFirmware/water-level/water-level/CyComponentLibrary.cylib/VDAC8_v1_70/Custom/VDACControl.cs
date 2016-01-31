/*******************************************************************************
* Copyright 2008-2001, Cypress Semiconductor Corporation.  All rights reserved.
* You may use this file only in accordance with the license, terms, conditions, 
* disclaimers, and limitations in the end user license agreement accompanying 
* the software package with which this file was provided.
*******************************************************************************/


using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Diagnostics;
using System.Text;
using System.Windows.Forms;
using CyDesigner.Extensions.Gde;
using CyDesigner.Extensions.Common;
using VDAC8_v1_70;


namespace VDAC8_v1_70
{
    /// <summary>
    /// VDAC design edit control.
    /// </summary>
    public partial class CyVDACControl : UserControl
    {
        #region Enumerated Type Strings
        const string RANGE_1_VOLT = "Range_1_Volt";
        const string RANGE_4_VOLT = "Range_4_Volt";
        const string HIGH_SPEED ="HighSpeed";
        const string LOW_SPEED = "LowSpeed";
        const string DAC_BUS = "DAC_Bus";
        const string CPU_OR_DMA = "CPU_or_DMA";
        const string EXTERNAL = "External";
        const string REGISTER_WRITE = "Register_Write";
        const string VDAC_RANGE = "VDAC_Range";
        const string VDAC_SPEED = "VDAC_Speed";
        const string VDAC_DATA_SOURCE = "Data_Source";
        const string VDAC_STROBE_MODE = "Strobe_Mode";
        const string VDAC_VOLTAGE = "Voltage";
        const string VDAC_DATA_VALUE = "Initial_Value";
        #endregion

        #region global varialbes
        private const int VOLTMIN = 0;
        private const int VOLTMAX1 = 1020;
        private const int VOLTMAX2 = 4080;
        private const int VOLT_HEX_MIN = 0;
        private const int VOLT_HEX_MAX = 0xFF;
        private const int Range1 = 4;
        private const int Range2 = 16;
        private static int ZeroHex = 0;
        private static int EndH = 0;
        private string Bytes = "";
        private static string hold = "";
        private string VoltageOverflow = "Voltage value exceeds MAX Voltage hence reseted to 0";
        private string EnterNumber = "Please enter a number";
        private string NotInHexFormat = "Characters should be between 0-9 or A-F";
        private string NumError = "Characters should be between 0-9";
        private string VoltRangeErr = "Voltage Value must be between 0 and {0}";
        private string DataValueErr = "Byte value must be between {0} and {1}";
        private string Int16RangeErr = "{0} is outside the range of Int16 type ";
        private string Int16FormatErr = "{0} is not in a recognizable format. ";
        public ICyInstEdit_v1 m_Component = null;
        #endregion

        //When object of this class is instantiated it updates form with
        //symbol saved values.
        public CyVDACControl(ICyInstEdit_v1 inst)
        {
            m_Component = inst;
            InitializeComponent();
            if (m_Component != null)
            {
                UpdateFormFromParams(m_Component);
            }
        }

        protected override void OnCreateControl()
        {
            base.OnCreateControl();
        }
        
        void ParentForm_FormClosing(object sender, FormClosingEventArgs e)
        {
            if (((Form)sender).DialogResult == DialogResult.Cancel)
            {
                return;
            }
            if (ep_Errors.GetError(VoltageValueUpDown) != "")
            {
                VoltageValueUpDown.Focus();
                e.Cancel = true;
            }
            if (ep_Errors.GetError(Bytes_UpDown) != "")
            {
                Bytes_UpDown.Focus();
                e.Cancel = true;
            }
        }

        #region Update Routines
        //This function updates the form symbol saved parameter values.
        public void UpdateFormFromParams(ICyInstEdit_v1 inst)
        {
            CyVDACParameters prms = new CyVDACParameters(inst);
            const string outputrange1 = "0";
            const string outputrange2 = "4";
            const string slow = "0";
            const string fast = "2";
            const string DataSource1 = "1";
            const string DataSource2 = "0";
            const string StrobeMode1 = "0";
            const string StrobeMode2 = "1";

            //Updates VDAC range radio button with symbol saved value
            switch (prms.m_Comp_VDAC_Range.Value)
            {
                case outputrange1: OutPutRange1.Checked = true;
                    break;
                case outputrange2: OutPutRange2.Checked = true;
                    break;
            }

            //Updates VDAC speed radio button with symbol saved value
            switch (prms.m_Comp_VDAC_Speed.Value)
            {
                case slow: Slow.Checked = true;
                    break;
                case fast: High.Checked = true;
                    break;
            }
            //Updates VDAC data source radio button with symbol saved value
            switch (prms.m_Comp_Data_Source.Value)
            {
                case DataSource2: rButtonDataBus.Checked = true;
                    break;
                case DataSource1: rButtonDacBus.Checked = true;
                    break;
            }

            //Updates VDAC strobe mode radio button with symbol saved value
            switch (prms.m_Comp_Strobe_Mode.Value)
            {
                case StrobeMode1: rButtonRegister.Checked = true;
                    break;
                case StrobeMode2: rButtonExternal.Checked = true;
                    break;
            }
            UnhookEvents();
            //Updates voltage and hex value with symbol saved value
            Bytes = Bytes_UpDown.Text;
            UpdateByteControl(m_Component);
            Voltage_Calculation();
            HookEvents();

        }
        #endregion


        private void HookEvents()
        {
            this.VoltageValueUpDown.TextChanged += new System.EventHandler(this.Voltage_ValueChanged);
            this.VoltageValueUpDown.Validating += new System.ComponentModel.CancelEventHandler(this.Voltage_Validating);
            this.Bytes_UpDown.TextChanged += new System.EventHandler(this.Bytes_UpDown_ValueChanged);
            this.Bytes_UpDown.Validating += new System.ComponentModel.CancelEventHandler(this.Bytes_Validating);
 
        }
        private void UnhookEvents()
        {
            this.VoltageValueUpDown.TextChanged -= new System.EventHandler(this.Voltage_ValueChanged);
            this.VoltageValueUpDown.Validating -= new System.ComponentModel.CancelEventHandler(this.Voltage_Validating);
            this.Bytes_UpDown.TextChanged -= new System.EventHandler(this.Bytes_UpDown_ValueChanged);
            this.Bytes_UpDown.Validating -= new System.ComponentModel.CancelEventHandler(this.Bytes_Validating);
        }
        //This function is invoked when user selects Output range 1V
        private void OutPutRange1_CheckChanged(object sender, EventArgs e)
        {
            int tmp = 0;

            if (OutPutRange1.Checked)
            {
                SetAParameter(VDAC_RANGE, RANGE_1_VOLT, true);
                UpdateByteControl(m_Component);
                Voltage_Calculation();

                try
                {
                    tmp = Int16.Parse(VoltageValueUpDown.Text);
                }
                catch (OverflowException)
                {
                    string error_str = String.Format(Int16RangeErr + VoltageValueUpDown.Text);
                    ep_Errors.SetError(VoltageValueUpDown, error_str);
                }
                catch (FormatException)
                {
                    string error_str = String.Format(Int16FormatErr + VoltageValueUpDown.Text);
                    ep_Errors.SetError(VoltageValueUpDown, error_str);
                }

                if (tmp > VOLTMAX1)
                {
                    VoltageValueUpDown.Text = "0";
                    ep_Errors.SetError(VoltageValueUpDown, VoltageOverflow);
                }
            }
            else if (OutPutRange2.Checked)
            {
                SetAParameter(VDAC_RANGE, RANGE_4_VOLT, true);
                UpdateByteControl(m_Component);
                Voltage_Calculation();
            }
 
        }

        //This function is invoked when user selects Output range 4V
        private void OutPutRange2_CheckChanged(object sender, EventArgs e)
        {
            int tmp = 0;

            if (OutPutRange2.Checked)
            {
                SetAParameter(VDAC_RANGE, RANGE_4_VOLT, true);
                UpdateByteControl(m_Component);
                Voltage_Calculation();

                try
                {
                    tmp = Int16.Parse(VoltageValueUpDown.Text);
                }
                catch (OverflowException)
                {
                    string error_str = String.Format(Int16RangeErr + VoltageValueUpDown.Text);
                    ep_Errors.SetError(VoltageValueUpDown, error_str);
                }
                catch (FormatException)
                {
                    string error_str = String.Format(Int16FormatErr + VoltageValueUpDown.Text);
                    ep_Errors.SetError(VoltageValueUpDown, error_str);
                }
                if (tmp > VOLTMAX2)
                {
                    VoltageValueUpDown.Text = "0";
                    ep_Errors.SetError(VoltageValueUpDown, VoltageOverflow);
                }
            }
            else if (OutPutRange1.Checked)
            {
                SetAParameter(VDAC_RANGE, RANGE_1_VOLT, true);
                UpdateByteControl(m_Component);
                Voltage_Calculation();
            }
        }

        //This function is invoked when user selects speed parameter as Slow
        private void Slow_CheckChanged(object sender, EventArgs e)
        {
            SetAParameter(VDAC_SPEED, LOW_SPEED, true);
        }

        //This function is invoked when user selects speed parameter as High
        private void Fast_CheckChanged(object sender, EventArgs e)
        {
            SetAParameter(VDAC_SPEED, HIGH_SPEED, true);
        }
        //This function is invoked when user selects data source as dac bus
        private void rButtonDacBus_CheckedChanged(object sender, EventArgs e)
        {
            //Making strobe mode as external when data source used as DAC Bus
            if (rButtonExternal.Checked == false)
            {
                rButtonExternal.Checked = true;
            }
            StrobeMode.Enabled = false;
            SetAParameter(VDAC_DATA_SOURCE, DAC_BUS, true);
        }
        //This function is invoked when user selects data source as CPU or DMA
        private void rButtonDataBus_CheckedChanged(object sender, EventArgs e)
        {
            if (StrobeMode.Enabled == false)
            {
                StrobeMode.Enabled = true;
            }
            SetAParameter(VDAC_DATA_SOURCE, CPU_OR_DMA, true);
        }
        //This function is invoked when user selects strobe mode  as external
        private void rButtonExternal_CheckedChanged(object sender, EventArgs e)
        {
            SetAParameter(VDAC_STROBE_MODE, EXTERNAL, true);
        }
        //This function is invoked when user selects strobe mode  as Register 
        private void rButtonRegister_CheckedChanged(object sender, EventArgs e)
        {
            SetAParameter(VDAC_STROBE_MODE, REGISTER_WRITE, true);
        }

        //Updates the symbol file with user selected parameter values.
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


        //This function is validates the entered voltage value
        private void Voltage_Validating(object sender, CancelEventArgs e)
        {
            e.Cancel = Voltage_Checking();
            if (!e.Cancel)
            {
                CorrectVoltage();
            }
        }

        private bool Voltage_Checking()
        {
            int tmp;
            UnhookEvents();
            bool error = false;
            if (CheckVoltage())
            {
                error = true;
            }
            else
            {
                SetVoltageValue();
                tmp = ByteCalculation();
                SetValue(tmp);
                
            }
            HookEvents();
            return error;
        }

        

        //This function is invoked when user changes the VDAC data value in 
        //customizer
        private void Bytes_UpDown_ValueChanged(object sender, EventArgs e)
        {
            UpdateByteValue();
            Bytes_Checking();
        }
        //This function validates the entere Hex value
        private void Bytes_Validating(object sender, CancelEventArgs e)
        {
            UpdateByteValue();
            e.Cancel = Bytes_Checking();
        }

        private bool Bytes_Checking()
        {
            bool error = false;

            UnhookEvents();
            if (Bytes_Check())
            {
                error = true;
            }
            else
            {
                SetByteValue();
                Voltage_Calculation();
                SetVoltageValue();
                
 
            }
            HookEvents();
            return error;
        }

        //This function calcualtes the voltage value which is equivalent to Hex
        //value.
        private void Voltage_Calculation()
        {
            int tmp = 0;

            try
            {
                tmp = Int16.Parse(Bytes, System.Globalization.NumberStyles.HexNumber);
            }

            catch (OverflowException)
            {
                string error_str = String.Format(Int16RangeErr + Bytes_UpDown.Text);
                ep_Errors.SetError(Bytes_UpDown, error_str);
            }
            catch (FormatException)
            {
                string error_str = String.Format(Int16FormatErr + Bytes_UpDown.Text);
                ep_Errors.SetError(Bytes_UpDown, error_str);
            }
            if (OutPutRange1.Checked)
            {
                tmp = tmp * Range1;
                VoltageValueUpDown.Text = tmp.ToString();
                
            }
            else
            {
                tmp = tmp * Range2;
                VoltageValueUpDown.Text = tmp.ToString();
            }
        }

        // This function saves the value to the symbol file
        private void SetByteValue()
        {
            int tmp = 0;
            string value = "";

            try
            {
                tmp = Int16.Parse(Bytes, System.Globalization.NumberStyles.HexNumber);
            }
            catch (OverflowException)
            {
                string error_str = String.Format(Int16RangeErr + Bytes_UpDown.Text);
                ep_Errors.SetError(Bytes_UpDown, error_str);
            }
            catch (FormatException)
            {
                string error_str = String.Format(Int16FormatErr + Bytes_UpDown.Text);
                ep_Errors.SetError(Bytes_UpDown, error_str);
            }
            value = tmp.ToString();
            SetAParameter(VDAC_DATA_VALUE, value, false);
        }

        /// <summary>
        /// This function checks for the entered hex value
        /// </summary>
        private bool Bytes_Check()
        {
            int maxallowed = VOLT_HEX_MAX;
            int minallowed = VOLT_HEX_MIN;
            bool error = false;
            int tmp = 0;

            ep_Errors.SetError(Bytes_UpDown, "");
            if (Bytes_UpDown.Text == "")
            {
                ep_Errors.SetError(Bytes_UpDown, EnterNumber);
                error = true;
            }
            //Checks whether entered string hexdecimal or not.
            else if (!OnlyHexInString(Bytes))
            {
                ep_Errors.SetError(Bytes_UpDown, NotInHexFormat);
                error = true;
            }
            else
            {
                try
                {
                    tmp = Int16.Parse(Bytes, System.Globalization.NumberStyles.HexNumber);
                }
                catch (OverflowException)
                {
                    string error_str = String.Format(Int16RangeErr + Bytes_UpDown.Text);
                    ep_Errors.SetError(Bytes_UpDown, error_str);
                }
                catch (FormatException)
                {
                    string error_str = String.Format(Int16FormatErr
                                                    + Bytes_UpDown.Text);
                    ep_Errors.SetError(Bytes_UpDown, error_str);
                }

                if (tmp < minallowed ||tmp > maxallowed)
                {
                    ep_Errors.SetError(Bytes_UpDown, string.Format(DataValueErr, minallowed,
                                                                  maxallowed));
                    error = true;
                }
                else
                {
                    error = false;
                }
            }
            return error;
            
        }

        //Checks whether string is hexdecimal or not.
        public bool OnlyHexInString(string str)
        {
            return System.Text.RegularExpressions.Regex.IsMatch(str, @"\A\b[0-9a-fA-F]+\b\Z");
        }

        public bool OnlyNum(string str)
        {
            return System.Text.RegularExpressions.Regex.IsMatch(str, @"\A[0-9]+\b\Z");
        }

        /// <summary>
        /// This function updates Byte control
        /// </summary>
        /// <param name="inst"></param>
        private void UpdateByteControl(ICyInstEdit_v1 inst)
        {
            CyVDACParameters prms = new CyVDACParameters(inst);
            int tmp = 0;
            // Displays Hex value ending with "h" or preceeding with "0x"
            try
            {
                tmp = Int16.Parse(prms.m_Comp_Byte.Value);
            }
            catch (OverflowException)
            {
                string error_str = String.Format(Int16RangeErr + Bytes_UpDown.Text);
                ep_Errors.SetError(Bytes_UpDown, error_str);
            }
            catch (FormatException)
            {
                string error_str = String.Format(Int16FormatErr
                                                + Bytes_UpDown.Text);
                ep_Errors.SetError(Bytes_UpDown, error_str);
            }
            Bytes = tmp.ToString("X");
            Bytes_UpDown.Text = tmp.ToString("X");
            if (ZeroHex == 1)
            {
                Bytes_UpDown.Text = "0x" + Bytes_UpDown.Text;
            }
            if (EndH == 1)
            {
                Bytes_UpDown.AppendText("h");
            }
        }

        // This function updates Hex value
        private void UpdateByteValue()
        {
             ep_Errors.SetError(Bytes_UpDown, "");
            //Fixes the text box size to 4 when string starting with 0x
            if (Bytes_UpDown.Text.StartsWith("0x"))
            {
                Bytes_UpDown.MaxLength = 4;
            }
            //Fixes the text box size to 3 when string starting with h
            else if (Bytes_UpDown.Text.EndsWith("h"))
            {
                Bytes_UpDown.MaxLength = 3;
                hold = Bytes_UpDown.Text;
                EndH = 1;
            }
            Bytes = Bytes_UpDown.Text;
            //Removes h if string ends with "h"
            if (Bytes_UpDown.Text.EndsWith("h"))
            {
                Bytes = Bytes.Remove(Bytes.Length - 1);
            }

            // Checks for Byte value starting with 0x and removes it before
            //moving into the next stage.
            if (Bytes_UpDown.Text.StartsWith("0x") && Bytes_UpDown.Text.Length > 2)
            {
                ZeroHex = 1;
                hold = Bytes_UpDown.Text;
                Bytes = Bytes.Substring(2);
                Bytes_Check();
            }
            //When string is Ox and length of the string is 2 make the 
            // string as zero
            else if (Bytes_UpDown.Text.StartsWith("0x") && Bytes_UpDown.Text.Length == 2)
            {
                Bytes = "0";
            }
            else
            {
                // Checks whether previous string is starting with Ox or not
                if (ZeroHex == 1 && !hold.StartsWith("0x"))
                {
                    ZeroHex = 0;
                }
                // Checks whether previous string is Ending  with h or not
                if (EndH == 1 && !hold.EndsWith("h"))
                {
                    EndH = 0;
                }
                hold = Bytes_UpDown.Text;
                Bytes_Check();
                
            }
        }

        // This function checks the entered voltage value
        private bool CheckVoltage()
        {
            int maxallowed = VOLTMIN;
            bool error = false;
            int tmp = 0;

            ep_Errors.SetError(VoltageValueUpDown, "");
            // Checks whether Voltage text box is zero or not.
            if (VoltageValueUpDown.Text == "")
            {
                ep_Errors.SetError(VoltageValueUpDown, EnterNumber);
                error = true;
            }
            // Checks for characters 0-9 
            else if (!OnlyNum(VoltageValueUpDown.Text))
            {
                ep_Errors.SetError(VoltageValueUpDown, NumError);
                error = true;
            }
            // checks for voltage limit when range = 1.020v is selected
            else if (OutPutRange1.Checked)
            {
                maxallowed = VOLTMAX1;

                try
                {
                    tmp = Int16.Parse(VoltageValueUpDown.Text);
                }
                catch (OverflowException)
                {
                    string error_str = String.Format(Int16RangeErr + VoltageValueUpDown.Text);
                    ep_Errors.SetError(VoltageValueUpDown, error_str);
                }
                catch (FormatException)
                {
                    string error_str = String.Format(Int16FormatErr
                                                    + VoltageValueUpDown.Text);
                    ep_Errors.SetError(VoltageValueUpDown, error_str);
                }
                
                if (tmp < VOLTMIN || tmp > maxallowed)
                {
                    ep_Errors.SetError(VoltageValueUpDown,
                           string.Format(VoltRangeErr, maxallowed));
                    error = true;
                }
            }
            // checks for voltage limit when range = 4.080v is selected
            else 
            {
                maxallowed = VOLTMAX2;

                try
                {
                    tmp = Int16.Parse(VoltageValueUpDown.Text);
                }
                catch (OverflowException)
                {
                    string error_str = String.Format(Int16RangeErr + VoltageValueUpDown.Text);
                    ep_Errors.SetError(VoltageValueUpDown, error_str);
                }
                catch (FormatException)
                {
                    string error_str = String.Format(Int16FormatErr
                                                    + VoltageValueUpDown.Text);
                    ep_Errors.SetError(VoltageValueUpDown, error_str);
                }
                if (tmp < VOLTMIN || tmp > maxallowed)
                {
                    ep_Errors.SetError(VoltageValueUpDown, string.Format(VoltRangeErr,maxallowed));
                    error = true;
                }
            }
            
            return error;
       }
        /// <summary>
        /// This function calculate the voltage equivalent Hex value
        /// </summary>
        /// <returns>tmp</returns>
        private int ByteCalculation()
        {
            int tmp = 0;

            try
            {
                tmp = Int16.Parse(VoltageValueUpDown.Text);
            }

            catch (OverflowException)
            {
                string error_str = String.Format(Int16RangeErr + Bytes_UpDown.Text);
                ep_Errors.SetError(Bytes_UpDown, error_str);
            }
            catch (FormatException)
            {
                string error_str = String.Format(Int16FormatErr
                                                + Bytes_UpDown.Text);
                ep_Errors.SetError(Bytes_UpDown, error_str);
            }

            //Calculates the equivalent hex value when range = 1.02V is selected
            if (OutPutRange1.Checked)
            {
                tmp = tmp / Range1;
            }
            //Calculates the equivalent hex value when range = 4.080V is selected
            else
            {
                 tmp = tmp / Range2;
            }
             //Updates the Bytes text box with latest Hex value.
            if (ZeroHex == 1)
            {
                Bytes_UpDown.Text = "0x" + tmp.ToString("X");
                EndH = 0;
            }
            else
            {
                Bytes_UpDown.Text = tmp.ToString("X");
            }
            if (EndH == 1)
            {
                Bytes_UpDown.Text = tmp.ToString("X") + "h";
                ZeroHex = 0;
            }
            
                
            return tmp;
            
        }
        //Saves the voltage value into the symbole file
        private void SetVoltageValue()
        {
            SetAParameter(VDAC_VOLTAGE, VoltageValueUpDown.Text, false);
        }

        //Saves the VDAC data value into the symbol file.
        private void SetValue(int tmp)
        {
            string value = "";
            value = tmp.ToString();
            SetAParameter(VDAC_DATA_VALUE, value, false);
        }

        private void Voltage_ValueChanged(object sender, EventArgs e)
        {
            Voltage_Checking();
        }

        private double CorrectVoltage()
        {
            double VoltVal;
            double ByteVal;

            VoltVal = Int16.Parse(VoltageValueUpDown.Text);
            if (OutPutRange1.Checked)
            {
                VoltVal = VoltVal / Range1;
                ByteVal = Math.Floor(VoltVal);
                VoltVal = ByteVal * Range1;
                VoltageValueUpDown.Text = VoltVal.ToString();
            }
            //Calculates the equivalent hex value when range = 4.080V is selected
            else
            {
                VoltVal = VoltVal / Range2;
                ByteVal = Math.Floor(VoltVal);
                VoltVal = ByteVal * Range2;
                VoltageValueUpDown.Text = VoltVal.ToString();
            }
            return ByteVal;
        }

        
    }
}









