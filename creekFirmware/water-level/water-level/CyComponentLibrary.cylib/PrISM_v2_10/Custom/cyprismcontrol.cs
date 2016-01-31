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
using System.Collections;
using System.Diagnostics;
using CyDesigner.Extensions.Common;
using CyDesigner.Extensions.Gde;

namespace PrISM_v2_10
{
    public partial class CyPRISMControl : UserControl, ICyParamEditingControl
    {
        #region Variables

        enum State
        {
            CUSTOM_VIEW,
            EXPRESSION_VIEW
        }

        public CyPRISMParameters m_parameters;
        static int[,] STANDARD_POLYNOMIALS = new int[31, 4] { 
        { 2, 1, 0, 0 }, 
        { 3, 2, 0, 0 }, 
        { 4, 3, 0, 0}, 
        { 5, 4, 3, 2},
        { 6, 5, 3, 2},
        { 7, 6, 5, 4},
        { 8, 6, 5, 4},
        { 9, 8, 6, 5},
        { 10, 9, 7, 6},
        { 11, 10, 9, 7},
        { 12, 11, 8, 6},
        { 13, 12, 10, 9},
        { 14, 13, 11, 9 },
        { 15, 14, 13, 11},
        { 16, 14, 13, 11},
        { 17, 16, 15, 14},
        { 18, 17, 16, 13},
        { 19, 18, 17, 14},
        { 20, 19, 16, 14},
        { 21, 20, 19, 16},
        { 22, 19, 18, 17},
        { 23, 22, 20, 18},
        { 24, 23, 21, 20},
        { 25, 24, 23, 22},
        { 26, 25, 24, 20},
        { 27, 26, 25, 22},
        { 28, 27, 24, 22},
        { 29, 28, 27, 25},
        { 30, 29, 26, 24},
        { 31, 30, 29, 28},
        { 32, 30, 26, 25}};

        Int32[] m_customPolinomial;
        State m_state;

        bool m_setDefaultSeed = false;

        #endregion

        public uint Resolution
        {
            get { return m_parameters.Resolution; }
            set
            {
                m_parameters.Resolution = value;

                FillPulseComboBoxes();
                FillLFSR(false);

                UpdatePolyValueTextBox(CalcPolynomial(checkBoxCustom.Checked));
                if (m_setDefaultSeed)
                {
                    string seed = ((((UInt64)1 << (UInt16)Resolution)) - 1).ToString("X").ToUpper();
                    textBoxSeed.Text = seed;
                    m_parameters.SeedValue = Convert.ToUInt32(textBoxSeed.Text, 16);
                    m_setDefaultSeed = false;
                }
            }
        }

        public CyPRISMControl()
        {
            InitializeComponent();
        }

        public CyPRISMControl(CyPRISMParameters parameters)
            : this()
        {
            this.m_parameters = parameters;
            this.DoubleBuffered = true;
            this.Dock = DockStyle.Fill;
            numUpDownResolution.Minimum = CyPRISMParameters.RESOLUTION_MIN;
            numUpDownResolution.Maximum = CyPRISMParameters.RESOLUTION_MAX;
            numUpDownResolution.TextChanged += new EventHandler(numUpDownResolution_TextChanged);

            UpdateFromParam();
        }

        #region ICyParamEditingControl Members

        public Control DisplayControl
        {
            get { return this; }
        }

        /// <summary>
        /// Gets any errors that exist for parameters on the DisplayControl.
        /// </summary>
        /// <returns></returns>
        public IEnumerable<CyCustErr> GetErrors()
        {
            List<CyCustErr> errs = new List<CyCustErr>();
            ICyInstEdit_v1 m_edit = m_parameters.m_inst;
            if (m_edit != null)
                foreach (string paramName in m_edit.GetParamNames())
                {
                    CyCompDevParam param = m_edit.GetCommittedParam(paramName);
                    if (param.IsVisible && Resource1.TAB_NAME_GENERAL.Contains(param.TabName))
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

        public void UpdateFromParam()
        {
            m_state = State.EXPRESSION_VIEW;
            
            textBoxSeed.Text = m_parameters.SeedValue.ToString("X");
            
            numUpDownResolution.Value = m_parameters.Resolution;
            UpdatePolyValue();

            FillPulseComboBoxes();

            comboBoxCompare0.SelectedIndex = (byte)m_parameters.CompareType0;
            comboBoxCompare1.SelectedIndex = (byte)m_parameters.CompareType1;

            checkBoxPulseTypeHardcoded.Checked = m_parameters.PulseTypeHardcoded;

            m_state = State.CUSTOM_VIEW;
        }

        private void UpdatePolyValue()
        {
            UInt32 loadedPolyValue = m_parameters.PolyValue;
            if (loadedPolyValue != CalcPolynomial(false))
            {
                checkBoxCustom.Checked = true;
                m_customPolinomial = new int[m_parameters.Resolution];

                string bitMask;
                if ((loadedPolyValue & ((ulong)1 << 31)) != 0)
                {
                    bitMask = Convert.ToString(Convert.ToInt64(loadedPolyValue & (ulong.MaxValue >> 1)), 2);
                }
                else
                {
                    bitMask = Convert.ToString(Convert.ToInt32(loadedPolyValue), 2);
                    for (int i = bitMask.Length; i < Resolution; i++)
                    {
                        bitMask = "0" + bitMask;
                    }
                }
                
                // If length of bitmask is greater than Resolution, change Resolution (Expression View support)
                if (bitMask.Length > Resolution)
                {
                    checkBoxCustom.Checked = false;
                    FillLFSR(false);
                    //m_parameters.Resolution = (uint)bitMask.Length;
                    //numUpDownResolution.Value = m_parameters.Resolution;
                }
                else
                {
                    for (int i = 0; i < Resolution; i++)
                    {
                        if ((bitMask.Length >= i) && (bitMask[i] == '1'))
                            m_customPolinomial[i] = (int)(Resolution - i);
                    }

                    FillLFSR(true);
                }
            }
            else
            {
                checkBoxCustom.Checked = false;
                FillLFSR(false);
            }

            UpdatePolyValueTextBox(CalcPolynomial(checkBoxCustom.Checked));
        }


        UInt32 CalcPolynomial(bool custom)
        {
            uint resolution = Resolution;
            UInt32 result = 0;
            if (custom == false)
            {
                int lenght = resolution > 4 ? 4 : 2;
                for (int i = 0; i < lenght; i++)
                {
                    result += Convert.ToUInt32(Math.Pow(2, STANDARD_POLYNOMIALS[resolution - 2, i] - 1));
                }
            }
            else
            {
                for (int i = 0; i < m_customPolinomial.Length; i++)
                {
                    result += Convert.ToUInt32(Math.Pow(2, m_customPolinomial[i] - 1));
                }
            }

            return result;
        }

        ulong GetMaxSeed()
        {
            return (((UInt64)1 << (UInt16)m_parameters.Resolution) - 1);            
        }

        private void UpdatePolyValueTextBox(UInt32 polyValue)
        {
            uint val = Resolution;

            //if (m_state == State.CUSTOM_VIEW) 
            //    m_parameters.PolyValue = polyValue;

            string polyTextHexValue = polyValue.ToString("X");
            for (int i = polyTextHexValue.Length; i < (val - 1) / 4 + 1; i++)
            {
                polyTextHexValue = "0" + polyTextHexValue;
            }
            textBoxResult.Text = polyTextHexValue.ToUpper();
        }

        void FillLFSR(bool custom)
        {
            List<int> list = new List<int>();
            textBoxLFSR.Text = string.Empty;
            if (custom)
            {
                list = new List<int>(m_customPolinomial);
                for (int i = 0; i < list.Count; i++)
                {
                    if (list[i] > 0)
                        textBoxLFSR.Text += list[i].ToString() + ", ";
                }
                textBoxLFSR.Text = textBoxLFSR.Text.TrimEnd(' ', ',');
            }
            else
            {
                for (int i = 0; i < STANDARD_POLYNOMIALS.GetLength(1); i++)
                {
                    list.Add(STANDARD_POLYNOMIALS[Resolution - 2, i]);
                }
                if (Resolution > 4)
                {
                    for (int j = 0; j < 3; j++)
                    {
                        textBoxLFSR.Text += list[j].ToString() + ", ";
                    }
                    textBoxLFSR.Text += list[3].ToString();
                }
                else
                {
                    textBoxLFSR.Text = list[0].ToString() + ", " + list[1];
                }
            }
        }

        void FillPulseComboBoxes()
        {
            comboBoxPulse0.Items.Clear();
            comboBoxPulse1.Items.Clear();

            for (int i = 0; i < Resolution; i++)
            {
                comboBoxPulse0.Items.Add((UInt32)Math.Pow(2, i));
                comboBoxPulse1.Items.Add((UInt32)Math.Pow(2, i));
            }
            try
            {
                if ((m_parameters.Density0 > (UInt32)Math.Pow(2, Resolution - 1)) || (m_parameters.Density0 == 0))
                {
                    comboBoxPulse0.Text = ((UInt32)Math.Pow(2, Resolution - 1)).ToString();                    
                }
                else
                {
                    comboBoxPulse0.Text = m_parameters.Density0.ToString();
                }

                if ((m_parameters.Density1 > (UInt32)Math.Pow(2, Resolution - 1) || (m_parameters.Density1 == 0)))
                {
                    comboBoxPulse1.Text = ((UInt32)Math.Pow(2, Resolution - 1)).ToString();
                }
                else
                {
                    comboBoxPulse1.Text = m_parameters.Density1.ToString();
                }
            }
            catch
            {

            }
        }        

        #region Events
        private void numUpDownResolution_TextChanged(object sender, EventArgs e)
        {
            numUpDownResolution_Validating(sender, new CancelEventArgs());
        }
        private void numUpDownResolution_Validating(object sender, CancelEventArgs e)
        {
            if (m_state != State.CUSTOM_VIEW) return;
            uint min = CyPRISMParameters.RESOLUTION_MIN;
            uint max = CyPRISMParameters.RESOLUTION_MAX;

            string errMsg = String.Format(Resource1.VALUE_LIMIT, min, max);

            uint upDownValue;
            string value = numUpDownResolution.Text;
            if (UInt32.TryParse(value, out upDownValue))
                if (upDownValue >= min && upDownValue <= max)
                {
                    if (upDownValue != m_parameters.Resolution)
                    {
                        m_setDefaultSeed = true;
                        Resolution = upDownValue;
                    }
                    textBoxLFSR_Validating(textBoxLFSR, new CancelEventArgs());
                    errMsg = string.Empty;
                }
            e.Cancel = errMsg != string.Empty;
            errorProvider.SetError((Control)sender, errMsg);
        }
        private void textBoxLFSR_TextChanged(object sender, EventArgs e)
        {
            textBoxLFSR_Validating(sender, new CancelEventArgs());
        }
        private void textBoxLFSR_Validating(object sender, CancelEventArgs e)
        {
            if (m_state != State.CUSTOM_VIEW) return;
            string errMsg = Resource1.LFSR_FORMAT;
            string s = textBoxLFSR.Text;
            try
            {
                // Parse polynomials
                string[] arrayOfNums = s.Split(',');
                m_customPolinomial = new int[arrayOfNums.Length];
                for (int i = 0; i < arrayOfNums.Length; i++)                    
                {
                    m_customPolinomial[i] = Convert.ToInt32(arrayOfNums[i]);

                    if (m_customPolinomial[i] == 0) 
                        throw new Exception();
                    if ((m_customPolinomial.Length > 0) && (m_customPolinomial[i] > Resolution))
                    {
                        errMsg = Resource1.LFSR_ITEM_LIMIT;
                        throw new Exception();
                    }
                }

                UpdatePolyValueTextBox(CalcPolynomial(checkBoxCustom.Checked));
                errMsg = string.Empty;
            }
            catch
            {
            }
            e.Cancel = errMsg != string.Empty;
            errorProvider.SetError((Control)sender, errMsg);
        }

        private void textBoxSeed_TextChanged(object sender, EventArgs e)
        {
            CancelEventArgs ex = new CancelEventArgs();
            textBoxSeed_Validating(sender, ex);
        }

        private void textBoxSeed_Validating(object sender, CancelEventArgs e)
        {
            if (m_state != State.CUSTOM_VIEW) return;
            ulong maxseed = GetMaxSeed();

            string errMsg = String.Format(Resource1.SEED_VALUE_LIMIT,
                   maxseed.ToString("X") );

            UInt32 res;
            if (UInt32.TryParse(textBoxSeed.Text, System.Globalization.NumberStyles.HexNumber, null, out res))
            {
                if (res > 0 && res <= maxseed)
                {
                    m_parameters.SeedValue = res;
                    errMsg = string.Empty;
                }
            }

            e.Cancel = errMsg != string.Empty;
            errorProvider.SetError((Control)sender, errMsg);
        }

        private void textBoxResult_TextChanged(object sender, EventArgs e)
        {
            if (m_state != State.CUSTOM_VIEW) return;

            string errMsg = String.Format(Resource1.VALUE_LIMIT,
                UInt32.MinValue.ToString("X"), UInt32.MaxValue.ToString("X"));

            UInt32 res;
            if (UInt32.TryParse(textBoxResult.Text, System.Globalization.NumberStyles.HexNumber, null, out res))
            {                
                m_parameters.PolyValue = res;
                errMsg = string.Empty;
            }
            errorProvider.SetError((Control)sender, errMsg);
        }

        private void checkBoxCustom_CheckedChanged(object sender, EventArgs e)
        {
            textBoxLFSR.ReadOnly = checkBoxCustom.Checked == false;
            textBoxLFSR.TabStop = checkBoxCustom.Checked;

            if (m_state != State.CUSTOM_VIEW) return;

            if (checkBoxCustom.Checked)
                textBoxLFSR_TextChanged(textBoxLFSR, null);
            else
                Resolution = Resolution;
        }

        private void controlsValue_Changed(object sender, EventArgs e)
        {
            if (m_state != State.CUSTOM_VIEW) return;

            if (sender == comboBoxPulse0)
            {
                m_parameters.Density0 = Convert.ToUInt32(comboBoxPulse0.Text);
            }
            else if (sender == comboBoxPulse1)
            {
                m_parameters.Density1 = Convert.ToUInt32(comboBoxPulse1.Text);
            }
            else if (sender == comboBoxCompare0)
            {
                m_parameters.CompareType0 = (E_CompareType)Convert.ToByte(comboBoxCompare0.SelectedIndex);
            }
            else if (sender == comboBoxCompare1)
            {
                m_parameters.CompareType1 = (E_CompareType)Convert.ToByte(comboBoxCompare1.SelectedIndex);
            }
            else if (sender == checkBoxPulseTypeHardcoded)
            {
                m_parameters.PulseTypeHardcoded = checkBoxPulseTypeHardcoded.Checked;
            }
        }

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
