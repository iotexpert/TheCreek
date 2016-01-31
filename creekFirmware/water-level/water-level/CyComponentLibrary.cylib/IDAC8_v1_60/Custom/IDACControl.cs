/* ========================================
 *
 * Copyright YOUR COMPANY, THE YEAR
 * All Rights Reserved
 * UNPUBLISHED, LICENSED SOFTWARE.
 *
 * CONFIDENTIAL AND PROPRIETARY INFORMATION
 * WHICH IS THE PROPERTY OF your company.
 *
 * ========================================
*/

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
using IDAC8_v1_60;


namespace IDAC8_v1_60
{
    public partial class IDACControl : UserControl
    {
        private const int CURRENTMIN = 0;
        private const int CURRENTMAX1 = 32;
        private const int CURRENTMAX2 = 255;
        private const int CURRENTMAX3 = 2040;
        private const int CURRENT_HEX_MIN = 0;
        private const int CURRENT_HEX_MAX = 255;
        private const int CURRENT_PERCENTAGE_MIN = 0;
        private const int CURRENT_PERCENTAGE_MAX = 100;
        private const int MULT_FACT1 = 1;
        private const int MULT_FACT2 = 8;
        private const int Range1 = 4;
        private const int Range2 = 16;
        private const int Range3 = 16;

        public ICyInstEdit_v1 m_Component = null;
        public IDACControl(ICyInstEdit_v1 inst)
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
            CurrentValueUpDown.UpEvent += new UpButtonEvent(Current_UPEvent);
            CurrentValueUpDown.DownEvent += new DownButtonEvent(Current_DownEvent);
            CurrentPercentageUpDown.UpEvent += new UpButtonEvent(CurrentPercentage_UPEvent);
            CurrentPercentageUpDown.DownEvent += new DownButtonEvent(CurrentPercentage_DownEvent);
            CurrentByteUpDown.UpEvent += new UpButtonEvent(CurrentByte_UPEvent);
            CurrentByteUpDown.DownEvent += new DownButtonEvent(CurrentByte_DownEvent);
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

        private void PolarityPositive_CheckedChanged(object sender, EventArgs e)
        {
            // Setting the polarity to Current Source
            if (PolarityPositive.Checked)
            {
                SetAParameter("Polarity", "Current_Source", true); 
                UpdateFormFromParams(m_Component);                
            }
        }        

        private void PolarityNegative_CheckedChanged(object sender, EventArgs e)
        {
            // Setting the polarity to Current Sink 
            if (PolarityNegative.Checked)
            {
                SetAParameter("Polarity", "Current_Sink", true); 
                UpdateFormFromParams(m_Component);
            }            
        }

        private void rButtonRange1_CheckedChanged(object sender, EventArgs e)
        {
            // Setting the IDAC range to 32uA
            if (rButtonRange1.Checked)
            {
                SetAParameter("IDAC_Range", "fs_32uA", true); 
                UpdateFormFromParams(m_Component);
                CurrentValueUpDown.Maximum = 32;
                
            }
            CurrentByteUpDown_ValueChanged(sender, e);
        }

        private void rButtonRange2_CheckedChanged(object sender, EventArgs e)
        {  
            // Setting the IDAC range to 255uA
            if (rButtonRange2.Checked)
            {
                SetAParameter("IDAC_Range", "fs_255uA", true); 
                UpdateFormFromParams(m_Component);
                CurrentValueUpDown.Maximum = 255;
                
            }
            CurrentByteUpDown_ValueChanged(sender, e);
           
        }

        private void rButtonRange3_CheckedChanged(object sender, EventArgs e)
        {
            // Setting the IDAC range to 2040uA
            if (rButtonRange3.Checked)
            {
                SetAParameter("IDAC_Range", "fs_2040uA", true); 
                UpdateFormFromParams(m_Component);
                CurrentValueUpDown.Maximum = 2040;

                
            }

            CurrentByteUpDown_ValueChanged(sender, e);
        }

        public void UpdateFormFromParams(ICyInstEdit_v1 inst)
        {
            IDACParameter prms = new IDACParameter(inst);            
            const string polarity1 = "0";
            const string polarity2 = "4";
            const string outputrange1 = "0";
            const string outputrange2 = "4";
            const string outputrange3 = "8";
            const string DataSource1 = "1";
            const string DataSource2 = "0";
            const string Speed1 = "0";
            const string Speed2 = "2";
            const string StrobeMode1 = "0";
            const string StrobeMode2 = "1";

            switch (prms.Comp_IDAC_Polarity.Value)
            {

                case polarity1:
                    PolarityPositive.Checked = true;
                    break;
                case polarity2:
                    PolarityNegative.Checked = true;
                    break;
            }
            switch (prms.Comp_IDAC_Data_Source.Value)
            {

                case DataSource1:
                    rButtonDacBus.Checked = true;
                    break;
                case DataSource2:
                    rButtonDataBus.Checked = true;
                    break;
            }
            switch (prms.Comp_IDAC_Speed.Value)
            {

                case Speed1:
                    rButtonLowSpeed.Checked = true;
                    break;
                case Speed2:
                    rButtonHighSpeed.Checked = true;
                    break;
            }
            switch (prms.Comp_Strobe_Mode.Value)
            {

                case StrobeMode1:
                    rButtonRegister.Checked = true;
                    break;
                case StrobeMode2:
                    rButtonExternal.Checked = true;
                    break;
            }
            switch (prms.Comp_IDAC_Range.Value)
            {

                case outputrange1:
                    rButtonRange1.Checked = true;
                    break;
                case outputrange2:
                    rButtonRange2.Checked = true;
                    break;
                case outputrange3:
                    rButtonRange3.Checked = true;
                    break;
            }

            try
            {
               
                //CurrentValueUpDown.Value = Convert.ToInt16(prms.Comp_Current.Value);
            }
            catch (OverflowException)
            {
                string error_str = String.Format("{0} is outside the range of Int32 type " + CurrentValueUpDown.Value.ToString());
                ep_Errors.SetError(CurrentValueUpDown, error_str);
            }
            catch (FormatException)
            {
                string error_str = String.Format("{0} is not in a recognizable format." + CurrentValueUpDown.Value.ToString());
                ep_Errors.SetError(CurrentValueUpDown, error_str);
            }

            try
            {
                CurrentByteUpDown.Value = Convert.ToInt16(prms.Comp_Hex.Value);
            }
            catch (OverflowException)
            {
                string error_str = String.Format("{0} is outside the range of Int32 type " + CurrentByteUpDown.Value.ToString());
                ep_Errors.SetError(CurrentByteUpDown, error_str);
            }
            catch (FormatException)
            {
                string error_str = String.Format("{0} is not in a recognizable format. " + CurrentByteUpDown.Value.ToString());
                ep_Errors.SetError(CurrentByteUpDown, error_str);
            }

            try
            {
                CurrentPercentageUpDown.Value = Convert.ToInt16(prms.Comp_Percentage.Value);
            }
            catch (OverflowException)
            {
                string error_str = String.Format("{0} is outside the range of Int32 type " + CurrentPercentageUpDown.Value.ToString());
                ep_Errors.SetError(CurrentPercentageUpDown, error_str);
            }
            catch (FormatException)
            {
                string error_str = String.Format("{0} is not in a recognizable format. " + CurrentPercentageUpDown.Value.ToString());
                ep_Errors.SetError(CurrentPercentageUpDown, error_str);
            }
        }

        void ParentForm_FormClosing(object sender, FormClosingEventArgs e)
        {
            if (((Form)sender).DialogResult == DialogResult.Cancel)
            {
                return;
            }

            UpdateFormFromParams(m_Component);
        }

        private void CurrentPercentageUpDown_ValueChanged(object sender, EventArgs e)
        {
            CancelEventArgs ce = new CancelEventArgs();

            // Validating function for the Percentage updown button
            Current_Validating(sender, ce);

            UInt16 tmp = CURRENTMAX1;
            try
            {
                tmp = Convert.ToUInt16(CurrentPercentageUpDown.Value);
            }
            catch (OverflowException)
            {
                string error_str = String.Format("{0} is outside the range of UInt16 type " + CurrentPercentageUpDown.Value.ToString());
                ep_Errors.SetError(CurrentPercentageUpDown, error_str);
            }
            catch (FormatException)
            {
                string error_str = String.Format("{0} is not in a recognizable format." + CurrentPercentageUpDown.Value.ToString());
                ep_Errors.SetError(CurrentPercentageUpDown, error_str);
            }

            if (!ce.Cancel)
            {
                string tmp_val = CurrentPercentageUpDown.Value.ToString();
                SetAParameter("Current_Percentage", CurrentPercentageUpDown.Value.ToString(), false);
            }
        }

        private void Current_Validating(object sender, CancelEventArgs e)
        {
            UnhookUpdateEvents();
            int tmp1 = Convert.ToUInt16(CurrentPercentageUpDown.Value);
            int tmp2 = Convert.ToUInt16(CurrentPercentageUpDown.Value);

            // Current value and Hex values are calculated depending on the ranges selected
            if (rButtonRange1.Checked)
            {
                CurrentPercentageUpDown.Increment = 1;
                tmp1 = (tmp1 * CURRENT_HEX_MAX) / CURRENT_PERCENTAGE_MAX;
                tmp2 = (tmp2 * CURRENTMAX1) / CURRENT_PERCENTAGE_MAX;
            }
            else if (rButtonRange2.Checked)
            {
                CurrentPercentageUpDown.Increment = 1;
                tmp1 = (tmp1 * CURRENT_HEX_MAX) / CURRENT_PERCENTAGE_MAX;
                tmp2 = (tmp2 * CURRENTMAX2) / CURRENT_PERCENTAGE_MAX;
            }
            else
            {
                CurrentPercentageUpDown.Increment = 1;
                tmp1 = (tmp1 * CURRENT_HEX_MAX) / CURRENT_PERCENTAGE_MAX;
                tmp2 = (tmp2 * CURRENTMAX3) / CURRENT_PERCENTAGE_MAX;                
            }

            try
            {
                CurrentByteUpDown.Value = tmp1;
                CurrentValueUpDown.Value = tmp2;
            }
            catch (Exception )
            {
                //TODO: Where to display this error?
                ep_Errors.SetError(CurrentPercentageUpDown, string.Format(" Voltage value must be between 0 and {0}", CURRENTMAX1));
                e.Cancel = true;
            }

            SetAParameter("Current", CurrentValueUpDown.Value.ToString(), false);
            SetAParameter("Initial_Value", CurrentByteUpDown.Value.ToString(), false);
            SetAParameter("Current_Percentage", CurrentPercentageUpDown.Value.ToString(), false);

            HookupUpdateEvents();
            int maxallowed = CURRENTMIN;

            if (rButtonRange1.Checked)
            {
                maxallowed = CURRENTMAX1;
                if (CurrentValueUpDown.Value < CURRENTMIN || CurrentValueUpDown.Value > maxallowed)
                {
                    ep_Errors.SetError(CurrentValueUpDown, string.Format(" Voltage Value must be between 0 and {0}", maxallowed));
                    e.Cancel = true;
                }
                else
                {
                    ep_Errors.SetError(CurrentValueUpDown, "");
                }
            }
            else if (rButtonRange2.Checked)
            {
                maxallowed = CURRENTMAX2;
                if (CurrentValueUpDown.Value < CURRENTMIN || CurrentValueUpDown.Value > maxallowed)
                {
                    ep_Errors.SetError(CurrentValueUpDown, string.Format("Voltage Value must be between 0 and {0}", maxallowed));
                    e.Cancel = true;
                }
                else
                {
                    ep_Errors.SetError(CurrentValueUpDown, "");
                }
            }
            else if (rButtonRange3.Checked)
            {
                maxallowed = CURRENTMAX3;
                if (CurrentValueUpDown.Value < CURRENTMIN || CurrentValueUpDown.Value > maxallowed)
                {
                    ep_Errors.SetError(CurrentValueUpDown, string.Format("Voltage Value must be between 0 and {0}", maxallowed));
                    e.Cancel = true;
                }
                else
                {
                    ep_Errors.SetError(CurrentValueUpDown, "");
                }
            }

            if (CurrentByteUpDown.Value < CURRENT_HEX_MIN || CurrentByteUpDown.Value > CURRENT_HEX_MAX)
            {
                ep_Errors.SetError(CurrentByteUpDown, string.Format(" Byte value must be between {0} and {1}", CURRENT_HEX_MIN, CURRENT_HEX_MAX));
                e.Cancel = true;
            }
            else
            {
                ep_Errors.SetError(CurrentByteUpDown, "");
            }

            if (CurrentPercentageUpDown.Value < CURRENT_HEX_MIN || CurrentPercentageUpDown.Value > 100)
            {
                ep_Errors.SetError(CurrentPercentageUpDown, string.Format(" Byte value must be between {0} and {1}", CURRENT_HEX_MIN, 100));
                e.Cancel = true;
            }
            else
            {
                ep_Errors.SetError(CurrentPercentageUpDown, "");
            }          
        }

        private void Current_UpDown_KeyUp(object sender, KeyEventArgs e)
        { 
            // Calling the validating function for the percentage up down button
            Current_Validating(sender, new CancelEventArgs());
        }

        void UnhookUpdateEvents()
        {
            CurrentPercentageUpDown.ValueChanged -= CurrentPercentageUpDown_ValueChanged;
            CurrentByteUpDown.ValueChanged -= CurrentByteUpDown_ValueChanged;
            CurrentValueUpDown.ValueChanged -= CurrentValueUpDown_ValueChanged;
        }
        void HookupUpdateEvents()
        {
            CurrentPercentageUpDown.ValueChanged += CurrentPercentageUpDown_ValueChanged;
            CurrentByteUpDown.ValueChanged += CurrentByteUpDown_ValueChanged;
            CurrentValueUpDown.ValueChanged += CurrentValueUpDown_ValueChanged;
        }

        private void CurrentByteUpDown_ValueChanged(object sender, EventArgs e)
        {
            CancelEventArgs ce = new CancelEventArgs();

            //Validating function for the Byte updown button
            CurrentByteUpDown_Validating(sender, ce);

            UInt16 tmp = CURRENT_HEX_MAX;

            try
            {
                tmp = Convert.ToUInt16(CurrentByteUpDown.Value);
            }

            catch (OverflowException)
            {
                string error_str = String.Format("{0} is outside the range of UInt16 type " + CurrentByteUpDown.Value.ToString());
                ep_Errors.SetError(CurrentByteUpDown, error_str);
            }

            catch (FormatException)
            {
                string error_str = String.Format("{0} is not in a recognizable format." + CurrentByteUpDown.Value.ToString());
                ep_Errors.SetError(CurrentByteUpDown, error_str);
            }

            if (!ce.Cancel)
            {
                SetAParameter("Initial_Value", CurrentByteUpDown.Value.ToString(), false);
            }
        }

        private void CurrentByteUpDown_KeyUp(object sender, KeyEventArgs e)
        {            
            CurrentByteUpDown_Validating(sender, new CancelEventArgs());
        }

        private void CurrentByteUpDown_Validating(object sender, CancelEventArgs e)
        {
            UnhookUpdateEvents();
            int tmp1 = Convert.ToUInt16(CurrentByteUpDown.Value);
            int tmp2 = Convert.ToUInt16(CurrentByteUpDown.Value);

            // Current value and Percentage values are calculated depending on the IDAC range selected
            if (rButtonRange1.Checked)
            {
                CurrentByteUpDown.Increment = 1;
                tmp1 = (tmp1 * CURRENTMAX1) / CURRENT_HEX_MAX;
                tmp2 = (tmp2 * CURRENT_PERCENTAGE_MAX) / CURRENT_HEX_MAX;                            
            }
            else if (rButtonRange2.Checked)
            {
                CurrentByteUpDown.Increment = 1;
                tmp1 = tmp1 * MULT_FACT1;
                tmp2 = (tmp2 * CURRENT_PERCENTAGE_MAX) / CURRENT_HEX_MAX;             
            }
            else
            {
                CurrentByteUpDown.Increment = 1;
                tmp1 = tmp1 * MULT_FACT2;
                tmp2 = (tmp2 * CURRENT_PERCENTAGE_MAX) / CURRENT_HEX_MAX;              
            }

            // Assigning the current and percentage values
            try
            {
                CurrentValueUpDown.Value = tmp1;
                CurrentPercentageUpDown.Value = tmp2;
            }
            catch (Exception )
            {
                //TODO: Where to display this error?
                ep_Errors.SetError(CurrentValueUpDown, string.Format(" Voltage value must be between 0 and {0}", CURRENTMAX2));
                e.Cancel = true;
            }
            SetAParameter("Current", CurrentValueUpDown.Value.ToString(), false);
            SetAParameter("Current_Percentage", CurrentPercentageUpDown.Value.ToString(), false);
            SetAParameter("Initial_Value", CurrentByteUpDown.Value.ToString(), false);

            HookupUpdateEvents();
            int maxallowed = CURRENT_HEX_MAX;
            int minallowed = CURRENT_HEX_MIN;

            if (CurrentByteUpDown.Value < minallowed || CurrentByteUpDown.Value > maxallowed)
            {
                ep_Errors.SetError(CurrentByteUpDown, string.Format(" Byte value must be between {0} and {1}", minallowed, maxallowed));
                e.Cancel = true;
            }
            else
            {
                ep_Errors.SetError(CurrentByteUpDown, "");
            }

            if (CurrentPercentageUpDown.Value < minallowed || CurrentPercentageUpDown.Value > 100)
            {
                ep_Errors.SetError(CurrentPercentageUpDown, string.Format(" Byte value must be between {0} and {1}", minallowed, maxallowed));
                e.Cancel = true;
            }
            else
            {
                ep_Errors.SetError(CurrentPercentageUpDown, "");
            }

            if (rButtonRange1.Checked)
            {
                if (CurrentValueUpDown.Value < CURRENT_HEX_MIN || CurrentValueUpDown.Value > CURRENTMAX1)
                {
                    ep_Errors.SetError(CurrentValueUpDown, string.Format(" Voltage value must be between {0} and {1}", CURRENT_HEX_MIN, CURRENTMAX1));
                    e.Cancel = true;
                }
                else
                {
                    ep_Errors.SetError(CurrentValueUpDown, "");
                }
            }
            else if (rButtonRange1.Checked)
            {
                if (CurrentValueUpDown.Value > CURRENTMAX2)
                {
                    ep_Errors.SetError(CurrentValueUpDown, string.Format("Voltage value must be between {0} and {1}", CURRENTMIN, CURRENTMAX2));
                    e.Cancel = true;
                }
                else
                {
                    ep_Errors.SetError(CurrentValueUpDown, "");
                }
            }
            else
            {
                if (CurrentValueUpDown.Value > CURRENTMAX3)
                {
                    ep_Errors.SetError(CurrentValueUpDown, string.Format("Voltage value must be between {0} and {1}", CURRENTMIN, CURRENTMAX3));
                    e.Cancel = true;
                }
                else
                {
                    ep_Errors.SetError(CurrentValueUpDown, "");
                }
            }
        }

        private void CurrentValueUpDown_ValueChanged(object sender, EventArgs e)
        {
            CancelEventArgs ce = new CancelEventArgs();

            //Validating function for the current value updown button
            CurrentValue_Validating(sender, ce);

            UInt16 tmp = CURRENTMAX1;

            try
            {
                tmp = Convert.ToUInt16(CurrentValueUpDown.Value);
            }
            catch (OverflowException)
            {
                string error_str = String.Format("{0} is outside the range of UInt16 type " + CurrentValueUpDown.Value.ToString());
                ep_Errors.SetError(CurrentValueUpDown, error_str);
            }
            catch (FormatException)
            {
                string error_str = String.Format("{0} is not in a recognizable format." + CurrentValueUpDown.Value.ToString());
                ep_Errors.SetError(CurrentValueUpDown, error_str);
            }

            if (!ce.Cancel)
            {
                string tmp_val = CurrentValueUpDown.Value.ToString();
                SetAParameter("Current", CurrentValueUpDown.Value.ToString(), false);
            }
        }

        private void CurrentValue_UpDown_KeyUp(object sender, KeyEventArgs e)
        {
            CurrentValue_Validating(sender, new CancelEventArgs());
        }

        private void CurrentValue_Validating(object sender, CancelEventArgs e)
        {
            UnhookUpdateEvents();
            int tmp1 = Convert.ToUInt16(CurrentValueUpDown.Value);
            int tmp2 = Convert.ToUInt16(CurrentValueUpDown.Value);

            // Hex value and Percentage values are calculated depending on the IDAC range selected
            if (rButtonRange1.Checked)
            {
                CurrentValueUpDown.Increment = 1;
                tmp1 = (tmp1 * CURRENT_HEX_MAX) / CURRENTMAX1;
                tmp2 = (tmp2 * CURRENT_PERCENTAGE_MAX) / CURRENTMAX1;
            }
            else if (rButtonRange2.Checked)
            {
                CurrentValueUpDown.Increment = 1;
                tmp2 = (tmp1 * CURRENT_PERCENTAGE_MAX) / CURRENTMAX2;
            }
            else
            {
                CurrentValueUpDown.Increment = 1;
                tmp1 = (tmp1 * CURRENT_HEX_MAX) / CURRENTMAX3;
                tmp2 = (tmp2 * CURRENT_PERCENTAGE_MAX) / CURRENTMAX3;

            }

            // Assigning the Byte and Percentage values
            try
            {
                CurrentByteUpDown.Value = tmp1;
                CurrentPercentageUpDown.Value = tmp2;
            }
            catch (Exception )
            {
                //TODO: Where to display this error?
                ep_Errors.SetError(CurrentValueUpDown, string.Format(" Voltage value must be between 0 and {0}", CURRENTMAX1));
                e.Cancel = true;
            }

            SetAParameter("Current", CurrentValueUpDown.Value.ToString(), false);
            SetAParameter("Initial_Value", CurrentByteUpDown.Value.ToString(), false);
            SetAParameter("Current_Percentage", CurrentPercentageUpDown.Value.ToString(), false);

            HookupUpdateEvents();
            int maxallowed = CURRENTMIN;

            if (rButtonRange1.Checked)
            {
                maxallowed = CURRENTMAX1;
                if (CurrentValueUpDown.Value < CURRENTMIN || CurrentValueUpDown.Value > maxallowed)
                {
                    ep_Errors.SetError(CurrentValueUpDown, string.Format(" Voltage Value must be between 0 and {0}", maxallowed));
                    e.Cancel = true;
                }
                else
                {
                    ep_Errors.SetError(CurrentValueUpDown, "");
                }
            }
            else if (rButtonRange2.Checked)
            {
                maxallowed = CURRENTMAX2;
                if (CurrentValueUpDown.Value < CURRENTMIN || CurrentValueUpDown.Value > maxallowed)
                {
                    ep_Errors.SetError(CurrentValueUpDown, string.Format("Voltage Value must be between 0 and {0}", maxallowed));
                    e.Cancel = true;
                }
                else
                {
                    ep_Errors.SetError(CurrentValueUpDown, "");
                }
            }
            else if (rButtonRange3.Checked)
            {
                maxallowed = CURRENTMAX3;
                if (CurrentValueUpDown.Value < CURRENTMIN || CurrentValueUpDown.Value > maxallowed)
                {
                    ep_Errors.SetError(CurrentValueUpDown, string.Format("Voltage Value must be between 0 and {0}", maxallowed));
                    e.Cancel = true;
                }
                else
                {
                    ep_Errors.SetError(CurrentValueUpDown, "");
                }
            }

            if (CurrentByteUpDown.Value < CURRENT_HEX_MIN || CurrentByteUpDown.Value > CURRENT_HEX_MAX)
            {
                ep_Errors.SetError(CurrentByteUpDown, string.Format(" Byte value must be between {0} and {1}", CURRENT_HEX_MIN, CURRENT_HEX_MAX));
                e.Cancel = true;
            }
            else
            {
                ep_Errors.SetError(CurrentByteUpDown, "");
            }

            if (CurrentPercentageUpDown.Value < CURRENT_HEX_MIN || CurrentPercentageUpDown.Value > CURRENT_HEX_MAX)
            {
                ep_Errors.SetError(CurrentPercentageUpDown, string.Format(" Byte value must be between {0} and {1}", CURRENT_HEX_MIN, CURRENT_HEX_MAX));
                e.Cancel = true;
            }
            else
            {
                ep_Errors.SetError(CurrentPercentageUpDown, "");
            }
        }

        private void Current_UPEvent(object sender, UpButtonEventArgs e)
        {
            if (rButtonRange1.Checked)
            {
                if (CurrentValueUpDown.Value == CURRENTMAX1)
                    e.Cancel = true;
            }
            else if (rButtonRange2.Checked)
            {
                if (CurrentValueUpDown.Value == CURRENTMAX2)
                    e.Cancel = true;
            }
            else
            {
                if (CurrentValueUpDown.Value == CURRENTMAX3)
                    e.Cancel = true;
            }
        }

        private void Current_DownEvent(object sender, DownButtonEventArgs e)
        {
            if (CurrentValueUpDown.Value == CURRENTMIN)
                e.Cancel = true;
        }

        private void CurrentPercentage_UPEvent(object sender, UpButtonEventArgs e)
        {
            if (CurrentPercentageUpDown.Value == CURRENT_PERCENTAGE_MAX)
                e.Cancel = true;
        }

        private void CurrentPercentage_DownEvent(object sender, DownButtonEventArgs e)
        {
            if (CurrentPercentageUpDown.Value == CURRENTMIN)
                e.Cancel = true;
        }

        private void CurrentByte_UPEvent(object sender, UpButtonEventArgs e)
        {
            if (CurrentByteUpDown.Value == CURRENT_HEX_MAX)
                e.Cancel = true;
        }

        private void CurrentByte_DownEvent(object sender, DownButtonEventArgs e)
        {
            if (CurrentByteUpDown.Value == CURRENTMIN)
                e.Cancel = true;
        }

        private void rButtonDacBus_CheckedChanged(object sender, EventArgs e)
        {
            // setting DAC bus as a data source
            if (rButtonDacBus.Checked)
            {
                SetAParameter("Data_Source", "DAC_Bus", true);
                UpdateFormFromParams(m_Component);
            }
        }

        private void rButtonDataBus_CheckedChanged(object sender, EventArgs e)
        {
            // setting CPU bus as a data source
            if (rButtonDataBus.Checked)
            {
                SetAParameter("Data_Source", "CPU_or_DMA", true);
                UpdateFormFromParams(m_Component);
            }
        }

        private void rButtonLowSpeed_CheckedChanged(object sender, EventArgs e)
        {
            // setting speed as Low Speed
            if (rButtonLowSpeed.Checked)
            {
                SetAParameter("IDAC_Speed", "LowSpeed", true);
                UpdateFormFromParams(m_Component);
            }
        }

        private void rButtonHighSpeed_CheckedChanged(object sender, EventArgs e)
        {
            // setting speed as High Speed
            if (rButtonHighSpeed.Checked)
            {
                SetAParameter("IDAC_Speed", "HighSpeed", true);
                UpdateFormFromParams(m_Component);
            }
        }

        private void rButtonExternal_CheckedChanged(object sender, EventArgs e)
        {
            // setting Strobe mode as External
            if (rButtonExternal.Checked)
            {
                SetAParameter("Strobe_Mode", "External", true);
                UpdateFormFromParams(m_Component);
            }
        }

        private void rButtonRegister_CheckedChanged(object sender, EventArgs e)
        {
            // setting Strobe mode as Register Write
            if (rButtonRegister.Checked)
            {
                SetAParameter("Strobe_Mode", "Register_Write", true);
                UpdateFormFromParams(m_Component);
            }
        }
    }

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

//[] END OF FILE
