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
using VDAC8_v1_60;


namespace VDAC8_v1_60
{
    public partial class VDACControl : UserControl
    {
        private const int VOLTMIN = 0;
        private const int VOLTMAX1 = 1020;
        private const int VOLTMAX2 = 4080;
        private const int VOLT_HEX_MIN = 0;
        private const int VOLT_HEX_MAX = 255;
        private const int Range1 = 4;
        private const int Range2 = 16;
        public ICyInstEdit_v1 m_Component = null;
        public VDACControl(ICyInstEdit_v1 inst)
        {
            m_Component = inst;
            InitializeComponent();
            InitializeFormComponents(inst);
            if (m_Component != null)
            {
                UpdateFormFromParams(m_Component);
            }
        }

        protected override void OnCreateControl()
        {
            base.OnCreateControl();
            VoltageValueUpDown.UpEvent += new UpButtonEvent(Voltage_UPEvent);
            VoltageValueUpDown.DownEvent += new DownButtonEvent(Voltage_DownEvent);
            Bytes_UpDown.UpEvent += new UpButtonEvent(Byte_UpEvent);
            Bytes_UpDown.DownEvent += new DownButtonEvent(Byte_DownEvent);
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

        #region Form Initialization
        protected void InitializeFormComponents(ICyInstEdit_v1 inst)
        {
            VoltageValueUpDown.Minimum = VOLTMIN;
            if (OutPutRange1.Checked)
            {
                VoltageValueUpDown.Maximum = VOLTMAX1;
            }
            else
            {
                VoltageValueUpDown.Maximum = VOLTMAX2;
            }
            Bytes_UpDown.Minimum = VOLT_HEX_MIN;
            Bytes_UpDown.Maximum = VOLT_HEX_MAX;

            //Initialise Data Source Combo Box with Enumerated types
            //Set the Data Source Combo Box from enum
            IEnumerable<string> DataSourceEnums = inst.GetPossibleEnumValues("Data_Source");
            foreach (string str in DataSourceEnums)
            {
                m_cbDataSource.Items.Add(str);
            }

            //Initialise Strobe Mode Combo Box with Enumerated types
            //Set the Strobe Mode Combo Box from enum
            IEnumerable<string> StrobeModeEnums = inst.GetPossibleEnumValues("Strobe_Mode");
            foreach (string str in StrobeModeEnums)
            {
                m_cb_StrobeMode.Items.Add(str);
            }
        }
        #endregion

        #region Update Routines
        void UnhookUpdateEvents()
        {           
            VoltageValueUpDown.ValueChanged -= Voltage_ValueChanged;
            Bytes_UpDown.ValueChanged -= Bytes_UpDown_ValueChanged;            
            m_cbDataSource.ValueMemberChanged -= m_cbDataSource_SelectedIndexChanged;
            m_cb_StrobeMode.ValueMemberChanged -= m_cb_StrobeMode_SelectedIndexChanged;
        }

        void HookupUpdateEvents()
        {
            VoltageValueUpDown.ValueChanged += Voltage_ValueChanged;
            Bytes_UpDown.ValueChanged += Bytes_UpDown_ValueChanged;            
            m_cbDataSource.ValueMemberChanged += m_cbDataSource_SelectedIndexChanged;
            m_cb_StrobeMode.ValueMemberChanged += m_cb_StrobeMode_SelectedIndexChanged;
        }
      
        public void UpdateFormFromParams(ICyInstEdit_v1 inst)
        {  
            VDACParameters prms = new VDACParameters(inst);
            const string outputrange1 = "0";
            const string outputrange2 = "4";
            const string slow = "0";
            const string fast = "2";
           
            switch (prms.Comp_VDAC_Range.Value)
            {
                case outputrange1: OutPutRange1.Checked = true;
                                   break;
                case outputrange2: OutPutRange2.Checked = true;
                                   break;
            }
            
            switch (prms.Comp_VDAC_Speed.Value)
            {
                case slow: Slow.Checked = true;
                           break;
                case fast: Fast.Checked = true;
                           break;
            }

            try
            {
                VoltageValueUpDown.Value = Convert.ToInt16(prms.Comp_Voltage.Value);               
            }
            catch (OverflowException)
            {
                string error_str = String.Format("{0} is outside the range of Int32 type " + VoltageValueUpDown.Value.ToString());
                ep_Errors.SetError(VoltageValueUpDown, error_str);
            }
            catch (FormatException)
            {
                string error_str = String.Format("{0} is not in a recognizable format." + VoltageValueUpDown.Value.ToString());
                ep_Errors.SetError(VoltageValueUpDown, error_str);
            }

            try
            {
                Bytes_UpDown.Value = Convert.ToInt16(prms.Comp_Byte.Value);
            }
            catch (OverflowException)
            {
                string error_str = String.Format("{0} is outside the range of Int32 type " + Bytes_UpDown.Value.ToString());
                ep_Errors.SetError(Bytes_UpDown, error_str);
            }
            catch (FormatException)
            {
                string error_str = String.Format("{0} is not in a recognizable format. " + Bytes_UpDown.Value.ToString());
                ep_Errors.SetError(Bytes_UpDown, error_str);
            }  
         
            //Set the Data Source Combo Box from Enums
            IEnumerable<string> DataSourceEnums = inst.GetPossibleEnumValues("Data_Source");
            bool DataSourceFound = false;
            foreach (string str in DataSourceEnums)
            {
                
                if (!DataSourceFound)
                {
                    string paramcompare = m_Component.ResolveEnumIdToDisplay("Data_Source", prms.Comp_Data_Source.Expr);
                    if (paramcompare == str)
                    {
                        m_cbDataSource.SelectedItem = paramcompare;
                        DataSourceFound = true;
                    }
                }
            }

            //Set the Strobe Mode Combo Box from Enums
            IEnumerable<string> StrobeModeEnums = inst.GetPossibleEnumValues("Strobe_Mode");
            bool StrobeModeFound = false;
            foreach (string str in StrobeModeEnums)
            {
                if (!StrobeModeFound)
                {
                    string paramcompare = m_Component.ResolveEnumIdToDisplay("Strobe_Mode", prms.Comp_Strobe_Mode.Expr);
                    if (paramcompare == str)
                    {
                        m_cb_StrobeMode.SelectedItem = paramcompare;
                        StrobeModeFound = true;
                    }
                }
            }
         
        }
        #endregion

        private void OutPutRange1_CheckChanged(object sender, EventArgs e)
        {
           if (OutPutRange1.Checked)
            {
                SetAParameter("VDAC_Range", "Range_1_Volt", true);
                UpdateFormFromParams(m_Component);
                
                if (VoltageValueUpDown.Value > VOLTMAX1)
                {
                    VoltageValueUpDown.Value = 0;
                    MessageBox.Show(" Voltage value exceeds MAX Voltage hence reseted to 0");                    
                }
            }
           else if (OutPutRange2.Checked)
           {
               SetAParameter("VDAC_Range", "Range_4_Volt", true);
               UpdateFormFromParams(m_Component);
           }
            Bytes_UpDown_ValueChanged(sender, e);
            Voltage_ValueChanged(sender, e);
            
        }

        private void OutPutRange2_CheckChanged(object sender, EventArgs e)
        {
           if (OutPutRange2.Checked)
            {
                SetAParameter("VDAC_Range", "Range_4_Volt", true);
                UpdateFormFromParams(m_Component);
                if (VoltageValueUpDown.Value > VOLTMAX2)
                {
                    VoltageValueUpDown.Value = 0;
                    MessageBox.Show(" Voltage value exceeds MAX Voltage hence reseted to 0");
                }
            }
           else if (OutPutRange1.Checked)
           {
               SetAParameter("VDAC_Range", "Range_1_Volt", true);
               UpdateFormFromParams(m_Component);
           }

            Bytes_UpDown_ValueChanged(sender, e);
            Voltage_ValueChanged(sender, e);
        }

        private void Slow_CheckChanged(object sender, EventArgs e)
        {
            if (Slow.Checked)
            {
                VDACParameters prms = new VDACParameters(m_Component);
                SetAParameter("VDAC_Speed", "LowSpeed", true);
            }        
        }

        private void Fast_CheckedAhanged(object sender, EventArgs e)
        {
            if (Fast.Checked)
            {
                VDACParameters prms = new VDACParameters(m_Component);
                SetAParameter("VDAC_Speed", "HighSpeed", true);
            }
            else
                SetAParameter("VDAC_Speed", "LowSpeed", true);
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

        private void Voltage_ValueChanged(object sender, EventArgs e)
        {            
            CancelEventArgs ce = new CancelEventArgs();
            Voltage_Validating(sender, ce);
            UInt16 tmp = VOLTMAX1;
            try
            {
                tmp = Convert.ToUInt16(VoltageValueUpDown.Value);
            }
            catch (OverflowException)
            {
                string error_str = String.Format("{0} is outside the range of UInt16 type " + VoltageValueUpDown.Value.ToString());
                ep_Errors.SetError(VoltageValueUpDown, error_str);
            }
            catch (FormatException)
            {
                string error_str = String.Format("{0} is not in a recognizable format." + VoltageValueUpDown.Value.ToString());
                ep_Errors.SetError(VoltageValueUpDown, error_str);
            }

            if (!ce.Cancel)
            {
                string tmp_val = VoltageValueUpDown.Value.ToString();
                SetAParameter("Voltage", VoltageValueUpDown.Value.ToString(), false);
            }         
        }


        private void Voltage_UPEvent(object sender, UpButtonEventArgs e)
        {            
            if (OutPutRange1.Checked)
            {
                if (VoltageValueUpDown.Value == VOLTMAX1)
                    e.Cancel = true;
            }
                else
            {
                if (VoltageValueUpDown.Value == VOLTMAX2)
                    e.Cancel = true;
            }
        }

        private void Voltage_DownEvent(object sender, DownButtonEventArgs e)
        {         
            if (VoltageValueUpDown.Value == VOLTMIN)
                e.Cancel = true;                      
        }


        private void Voltage_Validating(object sender, CancelEventArgs e)
        {
            UnhookUpdateEvents();
            int tmp1 = Convert.ToUInt16(VoltageValueUpDown.Value);

            if (OutPutRange1.Checked)
            {
                VoltageValueUpDown.Increment = Range1;         
                tmp1 = tmp1 / Range1;
            }
            else
            {
                VoltageValueUpDown.Increment = Range2;
                tmp1 = tmp1 / Range2;                
            }
            try
            {
                Bytes_UpDown.Value = tmp1;
            }
            catch (Exception ex)
            {
                ep_Errors.SetError(VoltageValueUpDown, string.Format(ex.Message));
                e.Cancel = true;
            }

            SetAParameter("Voltage", VoltageValueUpDown.Value.ToString(), false);
            SetAParameter("Initial_Value", Bytes_UpDown.Value.ToString(), false);

            HookupUpdateEvents();
            int maxallowed = VOLTMIN;
            
            if (OutPutRange1.Checked)
            {
                maxallowed = VOLTMAX1;
                if (VoltageValueUpDown.Value < VOLTMIN || VoltageValueUpDown.Value > maxallowed)
                {
                    ep_Errors.SetError(VoltageValueUpDown, string.Format(" Voltage Value must be between 0 and {0}", maxallowed));
                    e.Cancel = true;
                }
                else
                {
                    ep_Errors.SetError(VoltageValueUpDown, "");
                }
            }
            else
            {
                maxallowed = VOLTMAX2;
                if (VoltageValueUpDown.Value < VOLTMIN || VoltageValueUpDown.Value > maxallowed)
                {
                    ep_Errors.SetError(VoltageValueUpDown, string.Format("Voltage Value must be between 0 and {0}", maxallowed));
                    e.Cancel = true;
                }
                else
                {
                    ep_Errors.SetError(VoltageValueUpDown, "");
                }
            }
            
            if (Bytes_UpDown.Value < VOLT_HEX_MIN || Bytes_UpDown.Value > VOLT_HEX_MAX)
            {
                ep_Errors.SetError(Bytes_UpDown, string.Format(" Byte value must be between {0} and {1}", VOLT_HEX_MIN, VOLT_HEX_MAX));
                e.Cancel = true;
            }
            else
            {
                ep_Errors.SetError(Bytes_UpDown, "");
            }            
        }

        private void Bytes_UpDown_ValueChanged(object sender, EventArgs e)
        {
            CancelEventArgs ce = new CancelEventArgs();
            Bytes_Validating(sender, ce);
            UInt16 tmp = VOLT_HEX_MAX;
           
            try
            {
                tmp = Convert.ToUInt16(Bytes_UpDown.Value);
            }

            catch (OverflowException)
            {
                string error_str = String.Format("{0} is outside the range of UInt16 type " + Bytes_UpDown.Value.ToString());
                ep_Errors.SetError(Bytes_UpDown, error_str);
            }

            catch (FormatException)
            {
                string error_str = String.Format("{0} is not in a recognizable format." + Bytes_UpDown.Value.ToString());
                ep_Errors.SetError(Bytes_UpDown, error_str);
            }
           
            if (!ce.Cancel)
            {
                SetAParameter("Initial_Value", Bytes_UpDown.Value.ToString(), false);
            }         
        }

        private void Bytes_Validating(object sender, CancelEventArgs e)
        {
            UnhookUpdateEvents();
            int tmp1 = Convert.ToUInt16(Bytes_UpDown.Value);
            if (OutPutRange1.Checked)
            {
                tmp1 = tmp1 * Range1;
                VoltageValueUpDown.Value = tmp1;
            }
            else
            {
                tmp1 = tmp1 * Range2;
                VoltageValueUpDown.Value = tmp1;
            }
            SetAParameter("Voltage", VoltageValueUpDown.Value.ToString(), false);
            SetAParameter("Initial_Value", Bytes_UpDown.Value.ToString(), false);
                        
            HookupUpdateEvents();
            int maxallowed = VOLT_HEX_MAX;
            int minallowed = VOLT_HEX_MIN;

            if (Bytes_UpDown.Value < minallowed || Bytes_UpDown.Value > maxallowed)
            {
                ep_Errors.SetError(Bytes_UpDown, string.Format(" Byte value must be between {0} and {1}", minallowed, maxallowed));
                e.Cancel = true;
            }
            else
            {
                ep_Errors.SetError(Bytes_UpDown, "");
            }
            Voltage_Validating(sender, e);
            
        }

        private void Byte_UpEvent(object sender, UpButtonEventArgs e)
        {          
            if (Bytes_UpDown.Value == VOLT_HEX_MAX)
                e.Cancel = true;            
        }

        private void Byte_DownEvent(object sender, DownButtonEventArgs e)
        {           
            if (Bytes_UpDown.Value == VOLT_HEX_MIN)
                e.Cancel = true;
        }

        private void Voltage_UpDown_KeyUp(object sender, KeyEventArgs e)
        {
            UnhookUpdateEvents();
            if (OutPutRange1.Checked)
            {
                if (VoltageValueUpDown.Value > 1020)
                {
                    ep_Errors.SetError(VoltageValueUpDown, string.Format("Voltage value must be between {0} and {1}", VOLTMIN, VOLTMAX1));
                }
            }
            Voltage_Validating(sender, new CancelEventArgs());
            HookupUpdateEvents();
        }

        private void Byte_UpDown_KeyUp(object sender, KeyEventArgs e)
        {
            UnhookUpdateEvents();            
            Bytes_Validating(sender, new CancelEventArgs());
            HookupUpdateEvents();
        }        
       
        private void m_cb_StrobeMode_SelectedIndexChanged(object sender, EventArgs e)
        {
            string prm = m_Component.ResolveEnumDisplayToId("Strobe_Mode", m_cb_StrobeMode.Text);
            SetAParameter("Strobe_Mode", prm, true);
        }

        private void m_cbDataSource_SelectedIndexChanged(object sender, EventArgs e)
        {
            string prm = m_Component.ResolveEnumDisplayToId("Data_Source", m_cbDataSource.Text);
            SetAParameter("Data_Source", prm, true);
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
