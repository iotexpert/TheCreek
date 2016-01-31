/******************************************************************************
* File Name: IDACControl.cs
* *****************************************************************************
* Copyright 2008-2011, Cypress Semiconductor Corporation.  All rights reserved.
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
using IDAC8_v1_80;


namespace IDAC8_v1_80
{
    /// <summary>
    /// IDAC design edit control.
    /// </summary>
    public partial class CyIDACControl : UserControl
    {
        #region Enumerated Type Strings
        const string CURRENT_SOURCE = "Current_Source";
        const string CURRENT_SINK = "Current_Sink";
        const string HARDWARE_CONTROLLED = "Hw_Controlled";
        const string HARDWARE_ENABLE = "Hardware_Enable";
        const string RANGE_32uA = "fs_32uA";
        const string RANGE_255uA = "fs_255uA";
        const string RANGE_2040uA = "fs_2040uA";
        const string HIGH_SPEED = "HighSpeed";
        const string LOW_SPEED = "LowSpeed";
        const string DAC_BUS = "DAC_Bus";
        const string CPU_OR_DMA = "CPU_or_DMA";
        const string EXTERNAL = "External";
        const string REGISTER_WRITE = "Register_Write";
        const string IDAC_RANGE = "IDAC_Range";
        const string IDAC_SPEED = "IDAC_Speed";
        const string IDAC_POLARITY = "Polarity";
        const string DATA_SOURCE = "Data_Source";
        const string STROBE_MODE = "Strobe_Mode";
        const string IDAC_CURRENT = "Current";
        const string IDAC_DATA_VALUE = "Initial_Value";
        #endregion

        #region global varialbes
        private const double CURRENTMIN = 0.0;
        private const double CURRENTMAX1 = 31.8750f;
        private const double CURRENTMAX2 = 255.0f;
        private const double CURRENTMAX3 = 2040.000f;
        private const int CURRENT_HEX_MIN = 0;
        private const int CURRENT_HEX_MAX = 0xFF;

        
        private static int ZeroHex = 0;
        private static int EndH = 0;
        private string Bytes = "";
       
        private static string hold = "";
        private static string EnterNewHex = "Hex value is not in a standard form";
        private string CurrentOverflow = "Current value exceeds MAX Current hence reseted to 0";
        private string EnterNumber = "Please enter a number";
        private string NotInHexFormat = "Characters should be between 0-9 or A-F";
        private string NumError = "Characters should be between 0-9";
        private string CurrentRangeErr = "Current Value must be between 0 and {0}";
        private string DataValueErr = "Byte value must be between {0} and {1}";
        private string Int16RangeErr = "{0} is outside the range of Int16 type ";
        private string DoubleRangeErr = "{0} is outside the range of Double type";
        private string Int16FormatErr = "{0} is not in a recognizable format. ";
        public ICyInstEdit_v1 m_Component = null;
        #endregion
        //When object of this class is instantiated it updates form with
        //symbol saved values.
        public CyIDACControl(ICyInstEdit_v1 inst)
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
            if (ep_Errors.GetError(AmpsValue) != "")
            {
                AmpsValue.Focus();
                e.Cancel = true;
            }
            if (ep_Errors.GetError(BytesValue) != "")
            {
                BytesValue.Focus();
                e.Cancel = true;
            }
        }

        private void HookEvents()
        {
            this.AmpsValue.TextChanged += new System.EventHandler(this.AmpsValue_TextChanged);
            this.AmpsValue.Validating += new System.ComponentModel.CancelEventHandler(this.AmpsValue_Validating);
            this.BytesValue.TextChanged += new System.EventHandler(this.BytesValue_TextChanged);
            this.BytesValue.Validating += new System.ComponentModel.CancelEventHandler(this.Bytes_Validating);

        }
        private void UnhookEvents()
        {
            this.AmpsValue.TextChanged -= new System.EventHandler(this.AmpsValue_TextChanged);
            this.AmpsValue.Validating -= new System.ComponentModel.CancelEventHandler(this.AmpsValue_Validating);
            this.BytesValue.TextChanged -= new System.EventHandler(this.BytesValue_TextChanged);
            this.BytesValue.Validating -= new System.ComponentModel.CancelEventHandler(this.Bytes_Validating);
        }
        #region Update Routines
        //This function updates the form symbol saved parameter values.
        public void UpdateFormFromParams(ICyInstEdit_v1 inst)
        {
            CyIDACParameters prms = new CyIDACParameters(inst);
            const string polarity1 = "0";
            const string polarity2 = "4";
            const string polarity3 = "2";
            const string outputrange1 = "0";
            const string outputrange2 = "4";
            const string outputrange3 = "8";
            const string slow = "0";
            const string high = "2";
            const string DataSource1 = "1";
            const string DataSource2 = "0";
            const string StrobeMode1 = "0";
            const string StrobeMode2 = "1";
           //Updates IDAC Polarity radio button with symbol saved value
            switch (prms.m_Comp_IDAC_Polarity.Value)
            {
                case polarity1: PolarityPositive.Checked = true;
                    break;
                case polarity2: PolarityNegative.Checked = true;
                    break;
                case polarity3: Hardware_Controlled.Checked = true;
                    break;
            }
            //Updates IDAC range radio button with symbol saved value
            switch (prms.m_Comp_IDAC_Range.Value)
            {
                case outputrange1: OutPutRange1.Checked = true;
                    break;
                case outputrange2: OutPutRange2.Checked = true;
                    break;
                case outputrange3: OutPutRange3.Checked = true;
                    break;
            }

            //Updates IDAC speed radio button with symbol saved value
            switch (prms.m_Comp_IDAC_Speed.Value)
            {
                case slow: Slow.Checked = true;
                    break;
                case high: High.Checked = true;
                    break;
            }
            //Updates IDAC data source radio button with symbol saved value
            switch (prms.m_Comp_Data_Source.Value)
            {
                case DataSource2: rButtonDataBus.Checked = true;
                    break;
                case DataSource1: rButtonDacBus.Checked = true;
                    break;
            }

            //Updates IDAC strobe mode radio button with symbol saved value
            switch (prms.m_Comp_Strobe_Mode.Value)
            {
                case StrobeMode1: rButtonRegister.Checked = true;
                    break;
                case StrobeMode2: rButtonExternal.Checked = true;
                    break;
            }
            //Updates Hardware Enable checkbox with symbol saved value
            if (prms.m_Comp_Hardware_Enable.Value.ToString() == "true")
            {
                Hardware_Enable.Checked = true;
                //MessageBox.Show("True");
            }
            else
            {
                Hardware_Enable.Checked = false;
                //MessageBox.Show("false");
            }
            //Hardware_Enable.Checked = prms.m_Comp_Hardware_Enable.Expr.Equals(Boolean.TrueString);
               
            UnhookEvents();
            //Updates Current and Hex value with symbol saved value
            UpdateByteControl(m_Component);
            Bytes = BytesValue.Text;
            
            UpdateByteValue();
            Current_Calculation();
            HookEvents();

        }
        #endregion

        //This function is invoked when user selects Output range 32uA
        private void OutPutRange1_CheckChanged(object sender, EventArgs e)
        {
            double tmp = 0;
            if (OutPutRange1.Checked)
            {
                SetAParameter(IDAC_RANGE, RANGE_32uA, true);
                AmpsValue.MaxLength = 8;
                UpdateByteControl(m_Component);
                Current_Calculation();

                try
                {
                    tmp = double.Parse(AmpsValue.Text);
                   
                }
                catch (OverflowException)
                {
                    string error_str = String.Format(DoubleRangeErr + AmpsValue.Text);
                    ep_Errors.SetError(AmpsValue, error_str);
                }
                catch (FormatException)
                {
                    string error_str = String.Format(Int16FormatErr + AmpsValue.Text);
                    ep_Errors.SetError(AmpsValue, error_str);
                }
                if (tmp > CURRENTMAX1)
                {
                    AmpsValue.Text = "0";
                    ep_Errors.SetError(AmpsValue, CurrentOverflow);
                }
            }
            else if (OutPutRange2.Checked)
            {
                SetAParameter(IDAC_RANGE, RANGE_255uA, true);
                
                AmpsValue.MaxLength = 8;
                UpdateByteControl(m_Component);
                Current_Calculation();
            }
            else 
            {
                SetAParameter(IDAC_RANGE, RANGE_2040uA, true);
                AmpsValue.MaxLength = 8;
                UpdateByteControl(m_Component);
                Current_Calculation();
            }
        }
        //This function is invoked when user selects Output range 255uA
        private void OutPutRange2_CheckChanged(object sender, EventArgs e)
        {
            double tmp = 0;
            if (OutPutRange2.Checked)
            {
                SetAParameter(IDAC_RANGE, RANGE_255uA, true);
                AmpsValue.MaxLength = 8;
                UpdateByteControl(m_Component);
                Current_Calculation();

                try
                {
                    tmp = double.Parse(AmpsValue.Text);
                 
                }
                catch (OverflowException)
                {
                    string error_str = String.Format(DoubleRangeErr + AmpsValue.Text);
                    ep_Errors.SetError(AmpsValue, error_str);
                }
                catch (FormatException)
                {
                    string error_str = String.Format(Int16FormatErr + AmpsValue.Text);
                    ep_Errors.SetError(AmpsValue, error_str);
                }
                if (tmp > CURRENTMAX2)
                {
                    AmpsValue.Text = "0";
                    ep_Errors.SetError(AmpsValue, CurrentOverflow);
                }
            }
            else if (OutPutRange1.Checked)
            {
                SetAParameter(IDAC_RANGE, RANGE_32uA, true);
                AmpsValue.MaxLength = 8;
                UpdateByteControl(m_Component);
                Current_Calculation();
            }
            else 
            {
                SetAParameter(IDAC_RANGE, RANGE_2040uA, true);
                AmpsValue.MaxLength = 8;
                UpdateByteControl(m_Component);
                Current_Calculation();
            }
        }

        //This function is invoked when user selects Output range 2040uA
        private void OutPutRange3_CheckChanged(object sender, EventArgs e)
        {
            double tmp = 0;
            if (OutPutRange3.Checked)
            {
                SetAParameter(IDAC_RANGE, RANGE_2040uA, true);
                AmpsValue.MaxLength = 8;
                UpdateByteControl(m_Component);
                Current_Calculation();

                try
                {
                    tmp = double.Parse(AmpsValue.Text);
              
                }
                catch (OverflowException)
                {
                    string error_str = String.Format(DoubleRangeErr + AmpsValue.Text);
                    ep_Errors.SetError(AmpsValue, error_str);
                }
                catch (FormatException)
                {
                    string error_str = String.Format(Int16FormatErr + AmpsValue.Text);
                    ep_Errors.SetError(AmpsValue, error_str);
                }
                if (tmp > CURRENTMAX3)
                {
                    AmpsValue.Text = "0";
                    ep_Errors.SetError(AmpsValue, CurrentOverflow);
                }
            }
            else if (OutPutRange1.Checked)
            {
                SetAParameter(IDAC_RANGE, RANGE_32uA, true);
                AmpsValue.MaxLength = 8;
                UpdateByteControl(m_Component);
                Current_Calculation();
            }
            else 
            {
                SetAParameter(IDAC_RANGE, RANGE_255uA, true);
                AmpsValue.MaxLength = 8;
                UpdateByteControl(m_Component);
                Current_Calculation();
            }
        }


        //This function is invoked when user selects PositivePolarity as Polarity
        private void PolarityPositive_CheckChanged(object sender, EventArgs e)
        {
            SetAParameter(IDAC_POLARITY, CURRENT_SOURCE, true);
        }
        //This function is invoked when user selects NegativePolarity as Polarity
        private void PolarityNegative_CheckChanged(object sender, EventArgs e)
        {
            SetAParameter(IDAC_POLARITY, CURRENT_SINK, true);
        }
        //This function is invoked when user selects HardwareControlled as Polarity
        private void Hardware_Controlled_CheckedChanged(object sender, EventArgs e)
        {
            SetAParameter(IDAC_POLARITY, HARDWARE_CONTROLLED, true);
        }
        //This function is invoked when user selects speed parameter as Slow
        private void Slow_CheckChanged(object sender, EventArgs e)
        {
            SetAParameter(IDAC_SPEED, LOW_SPEED, true);
        }

        //This function is invoked when user selects speed parameter as High
        private void High_CheckChanged(object sender, EventArgs e)
        {
            SetAParameter(IDAC_SPEED, HIGH_SPEED, true);
        }

        //This function is invoked when user selects data source as dac bus
        private void rButtonDacBus_CheckedChanged(object sender, EventArgs e)
        {

            if (rButtonDacBus.Checked == true)
            {
                rButtonExternal.Checked = true;
                StrobeMode.Enabled = false;
                SetAParameter(DATA_SOURCE, DAC_BUS, true);
                SetAParameter(STROBE_MODE, EXTERNAL, true);
            }
                      
            
        }
        //This function is invoked when user selects data source as CPU or DMA
        private void rButtonDataBus_CheckedChanged(object sender, EventArgs e)
        {
            if (rButtonDataBus.Checked == true)
            {
                rButtonRegister.Checked = true;
                StrobeMode.Enabled = true;
                SetAParameter(DATA_SOURCE, CPU_OR_DMA, true);
                SetAParameter(STROBE_MODE, REGISTER_WRITE, true);
            }
        }
        //This function is invoked when user selects strobe mode  as external
        private void rButtonExternal_CheckedChanged(object sender, EventArgs e)
        {
            
            SetAParameter(STROBE_MODE, EXTERNAL, true);
        }
        //This function is invoked when user selects strobe mode  as Register 
        private void rButtonRegister_CheckedChanged(object sender, EventArgs e)
        {
            SetAParameter(STROBE_MODE, REGISTER_WRITE, true);
        }
		//This function is invoked when user selects Hardware Enable as external
		private void Hardware_Enable_CheckedChanged(object sender, EventArgs e)
        {
            
            if (Hardware_Enable.Checked == true)
            {
                SetAParameter(HARDWARE_ENABLE, "1", true);
                
            }
            else
            {
                SetAParameter(HARDWARE_ENABLE, "0", true);

            }
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
       

        //This function validates the entered Current value
        private void AmpsValue_Validating(object sender, CancelEventArgs e)
        {
            e.Cancel = Current_Checking();
            if (e.Cancel != true)
            {
                Correct_Current();
            }
       
        }

        private bool Current_Checking()
        {
            int tmp;
            bool error = false;
            //UpdateByteControl(m_Component);
            UnhookEvents();
            if (CheckCurrent())
            {
                error = true;
            }
            else
            {
                SetCurrentValue();
                tmp = ByteCalculation();
                SetValue(tmp);
            }
            HookEvents();
            return error;

        }
        //This function validates the entered Hex value
        private void Bytes_Validating(object sender, CancelEventArgs e)
        {
            UpdateByteValue();
            UnhookEvents();
            if (Bytes_Check())
            {
                e.Cancel = true;
            }
            else
            {
                SetByteValue();
                Current_Calculation();
                SetCurrentValue();
            }
            HookEvents();
           

        }
        //This function calcualtes the current value which is equivalent to Hex
        //value.
        private void Current_Calculation()
        {
            double tmp = 0;

            try
            {
                tmp = Int16.Parse(Bytes, System.Globalization.NumberStyles.HexNumber);
            }

            catch (OverflowException)
            {
                string error_str = String.Format(Int16RangeErr + BytesValue.Text);
                ep_Errors.SetError(BytesValue, error_str);
            }
            catch (FormatException)
            {
                string error_str = String.Format(Int16FormatErr + BytesValue.Text);
                ep_Errors.SetError(BytesValue, error_str);
            }
            if (OutPutRange1.Checked)
            {
                tmp = (tmp * CURRENTMAX1)/CURRENT_HEX_MAX;
            }
            else if (OutPutRange2.Checked)
            {
                tmp = (tmp * CURRENTMAX2) / CURRENT_HEX_MAX;
            }
            else if (OutPutRange3.Checked)
            {
                tmp = (tmp * CURRENTMAX3) / CURRENT_HEX_MAX;
            }
            AmpsValue.Text = tmp.ToString();
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
                string error_str = String.Format(Int16RangeErr + BytesValue.Text);
                ep_Errors.SetError(BytesValue, error_str);
            }
            catch (FormatException)
            {
                string error_str = String.Format(Int16FormatErr + BytesValue.Text);
                ep_Errors.SetError(BytesValue, error_str);
            }
            value = tmp.ToString();
            SetAParameter(IDAC_DATA_VALUE, value, false);
        }
        /// <summary>
        /// This function checks for the entered hex value
        /// </summary>
        private bool Bytes_Check()
        {
            int maxallowed = CURRENT_HEX_MAX;
            int minallowed = CURRENT_HEX_MIN;
            bool error = false;
            int tmp = 0;

            ep_Errors.SetError(BytesValue, "");
            if (BytesValue.Text.StartsWith("0x") && BytesValue.Text.EndsWith("h"))
            {
                ep_Errors.SetError(BytesValue, EnterNewHex);
                error = true;
                
            }
            else if (BytesValue.Text == "")
            {
                ep_Errors.SetError(BytesValue, EnterNumber);
                error = true;
            }
            //Checks whether entered string hexdecimal or not.
            else if (!OnlyHexInString(Bytes))
            {
                ep_Errors.SetError(BytesValue, NotInHexFormat);
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
                    string error_str = String.Format(Int16RangeErr + BytesValue.Text);
                    ep_Errors.SetError(BytesValue, error_str);
                }
                catch (FormatException)
                {
                    string error_str = String.Format(Int16FormatErr
                                                    + BytesValue.Text);
                    ep_Errors.SetError(BytesValue, error_str);
                }

                if (tmp < minallowed || tmp > maxallowed)
                {
                    ep_Errors.SetError(BytesValue, string.Format(DataValueErr, minallowed,
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
            return System.Text.RegularExpressions.Regex.IsMatch(str, @"^\A\b[0-9a-fA-F]+\b\Z?$");
        }

        public bool OnlyNum(string str)
        {

            float result;
            if (float.TryParse(str, out result))
                return true;
            else
                return false; 
          
            //return System.Text.RegularExpressions.Regex.IsMatch(str, @"\A[0-9]+.+\b\Z");
        }

        /// <summary>
        /// This function updates Byte control
        /// </summary>
        /// <param name="inst"></param>
        private void UpdateByteControl(ICyInstEdit_v1 inst)
        {
            CyIDACParameters prms = new CyIDACParameters(inst);
            int tmp = 0;
            // Displays Hex value ending with "h" or preceeding with "0x"
            try
            {
                tmp = Int16.Parse(prms.m_Comp_Initial_Value.Value);
            }
            catch (OverflowException)
            {
                string error_str = String.Format(Int16RangeErr + BytesValue.Text);
                ep_Errors.SetError(BytesValue, error_str);
            }
            catch (FormatException)
            {
                string error_str = String.Format(Int16FormatErr
                                                + BytesValue.Text);
                ep_Errors.SetError(BytesValue, error_str);
            }
            Bytes = tmp.ToString("X");
            BytesValue.Text = tmp.ToString("X");
            if (ZeroHex == 1)
            {
                BytesValue.Text = "0x" + BytesValue.Text;
            }
            if (EndH == 1)
            {
                BytesValue.AppendText("h");
            }
        }
        
        // This function updates Hex value
        private void UpdateByteValue()
        {
            ep_Errors.SetError(BytesValue, "");
            //Fixes the text box size to 4 when string starting with 0x
            
            if (BytesValue.Text.StartsWith("0x"))
            {
                BytesValue.MaxLength = 4;

            }
           
            //Fixes the text box size to 3 when string starting with h
            else if (BytesValue.Text.EndsWith("h"))
            {
                BytesValue.MaxLength = 3;
                hold = BytesValue.Text;
                EndH = 1;
            }
           
            Bytes = BytesValue.Text;
            //Removes h if string ends with "h"
            if (BytesValue.Text.EndsWith("h"))
            {
                Bytes = Bytes.Remove(Bytes.Length - 1);
            }
            // Checks for Byte value starting with 0x and removes it before
            //moving into the next stage.

            if (BytesValue.Text.StartsWith("0x") && BytesValue.Text.Length > 2)
            {
                
                ZeroHex = 1;
                hold = BytesValue.Text;
                Bytes = Bytes.Substring(2);
                Bytes_Check();
            }
           
            //When string is Ox and length of the string is 2 make the 
            // string as zero
            else if (BytesValue.Text.StartsWith("0x") && BytesValue.Text.Length == 2)
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
                hold = BytesValue.Text;
                Bytes_Check();
            }
        }
         
        // This function checks the entered Current value
        private bool CheckCurrent()
        {
           
            double maxallowed = CURRENTMIN;
            bool error = false;
            double tmp = 0;

            ep_Errors.SetError(AmpsValue, "");
            // Checks whether Current text box is zero or not.
            if (AmpsValue.Text == "")
            {
                ep_Errors.SetError(AmpsValue, EnterNumber);
                error = true;
            }
            // Checks for characters 0-9 
            else if (!OnlyNum(AmpsValue.Text))
            {
                ep_Errors.SetError(AmpsValue, NumError);
                error = true;

            }
            // checks for Current limit when range = 32uA is selected
            else if (OutPutRange1.Checked)
            {    
                maxallowed =  CURRENTMAX1;

                try
                {
                    tmp = double.Parse(AmpsValue.Text);
                }
                catch (OverflowException)
                {
                    string error_str = String.Format(DoubleRangeErr + AmpsValue.Text);
                    ep_Errors.SetError(AmpsValue, error_str);
                }
                catch (FormatException)
                {
                    string error_str = String.Format(Int16FormatErr
                                                    + AmpsValue.Text);
                    ep_Errors.SetError(AmpsValue, error_str);
                }

                if (tmp < CURRENTMIN || tmp > maxallowed)
                {
                    ep_Errors.SetError(AmpsValue,
                           string.Format(CurrentRangeErr, maxallowed));
                    error = true;
                }
            }
            // checks for Current limit when range = 255uA is selected
            else if (OutPutRange2.Checked)
            {
                maxallowed =(double) CURRENTMAX2;

                try
                {
                    tmp = double.Parse(AmpsValue.Text);
                }
                catch (OverflowException)
                {
                    string error_str = String.Format(DoubleRangeErr + AmpsValue.Text);
                    ep_Errors.SetError(AmpsValue, error_str);
                }
                catch (FormatException)
                {
                    string error_str = String.Format(Int16FormatErr
                                                    + AmpsValue.Text);
                    ep_Errors.SetError(AmpsValue, error_str);
                }
                if (tmp < CURRENTMIN || tmp > maxallowed)
                {
                    ep_Errors.SetError(AmpsValue, string.Format(CurrentRangeErr, maxallowed));
                    error = true;
                }
            }
            // checks for Current limit when range = 2.04mA is selected
            else
            {
                maxallowed = (double)CURRENTMAX3;

                try
                {
                    tmp = double.Parse(AmpsValue.Text);
                }
                catch (OverflowException)
                {
                    string error_str = String.Format(DoubleRangeErr + AmpsValue.Text);
                    ep_Errors.SetError(AmpsValue, error_str);
                }
                catch (FormatException)
                {
                    string error_str = String.Format(Int16FormatErr
                                                    + AmpsValue.Text);
                    ep_Errors.SetError(AmpsValue, error_str);
                }
                if (tmp < CURRENTMIN || tmp > maxallowed)
                {
                    ep_Errors.SetError(AmpsValue, string.Format(CurrentRangeErr, maxallowed));
                    error = true;
                }
            }
        
            return error;
        }
    
        /// <summary>
        /// This function calculate the Current equivalent Hex value
        /// </summary>
        /// <returns>tmp</returns>
        private int ByteCalculation()
        {
            double tmp = 0;

            try
            {
                tmp = double.Parse(AmpsValue.Text);
            }

            catch (OverflowException)
            {
                string error_str = String.Format(DoubleRangeErr + AmpsValue.Text);
                ep_Errors.SetError(BytesValue, error_str);
            }
            catch (FormatException)
            {
                string error_str = String.Format(Int16FormatErr
                                                + AmpsValue.Text);
                ep_Errors.SetError(BytesValue, error_str);
            }

            //Calculates the equivalent hex value when range = 32uA is selected
            if (OutPutRange1.Checked)
            {
                tmp = (tmp / CURRENTMAX1) * CURRENT_HEX_MAX;
            }
            //Calculates the equivalent hex value when range = 255uA is selected
            else if (OutPutRange2.Checked)
            {
                tmp = (tmp / CURRENTMAX2) * CURRENT_HEX_MAX;
            }
            //Calculates the equivalent hex value when range = 2.04mA is selected
            else 
            {
                tmp = (tmp / CURRENTMAX3) * CURRENT_HEX_MAX;
            }
            //Updates the Bytes text box with latest Hex value.

            BytesValue.Text = ((int)tmp).ToString("X");
            if (ZeroHex == 1)
            {
                BytesValue.Text = "0x" + ((int)tmp).ToString("X");
                EndH = 0;
            }
            if (EndH == 1)
            {
                BytesValue.Text = ((int)tmp).ToString("X") + "h";
                ZeroHex = 0;
            }

            return ((int)tmp);

        }
        //Saves the Current value into the symbole file
        private void SetCurrentValue()
        {
            SetAParameter(IDAC_CURRENT, AmpsValue.Text, false);
        }

        //Saves the IDAC data value into the symbol file.
        private void SetValue(double tmp)
        {
            string value = "";
            value = tmp.ToString();
            SetAParameter(IDAC_DATA_VALUE, value, false);
        }

        //private void 
        //{
        //    double rem;
          
        //    double tmp = double.Parse(textBox1.Text);
        //    rem = tmp /(0.125);
          
          
        //    if (rem - (int)rem > 0.5)
        //        rem = Math.Ceiling(rem);
        //    else
        //    {
        //        rem = Math.Floor(rem);
        //    }


        private void BytesValue_TextChanged(object sender, EventArgs e)
        {
            UnhookEvents();
            UpdateByteValue();
            if (!Bytes_Check())
            {
                SetByteValue();
                Current_Calculation();
                SetCurrentValue();
            }
            HookEvents();
        }

        private void AmpsValue_TextChanged(object sender, EventArgs e)
        {
            int tmp;
            UnhookEvents();
            UpdateByteControl(m_Component);
            if (!CheckCurrent())
            {
                SetCurrentValue();
                tmp = ByteCalculation();
                SetValue(tmp);
            }
            HookEvents();
        }

        private void Correct_Current()
        {
            double tmp = 0;
            double rem = 0;

            try
            {
                tmp = double.Parse(AmpsValue.Text);
            }

            catch (OverflowException)
            {
                string error_str = String.Format(DoubleRangeErr + AmpsValue.Text);
                ep_Errors.SetError(BytesValue, error_str);
            }
            catch (FormatException)
            {
                string error_str = String.Format(Int16FormatErr
                                                + AmpsValue.Text);
                ep_Errors.SetError(BytesValue, error_str);
            }


            if (OutPutRange1.Checked)
            {
                rem = tmp / (0.125f);
                if (rem - (int)rem > 0.5f)
                    rem = Math.Ceiling(rem);
                else
                {
                    rem = Math.Floor(rem);
                }
                tmp = rem * (0.125f);
            }
            else if (OutPutRange3.Checked)
            {
                rem = tmp / (8.000f);
                if (rem - (int)rem >= 0.5f)
                    rem = Math.Ceiling(rem);
                else
                {
                    rem = Math.Floor(rem);
                }
                tmp = rem * (8.000f);
            }
            AmpsValue.Text = tmp.ToString();
        }

       
        

        
    }
}







