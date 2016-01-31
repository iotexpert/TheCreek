/*******************************************************************************
* Copyright 2008-2012, Cypress Semiconductor Corporation.  All rights reserved.
* You may use this file only in accordance with the license, terms, conditions, 
* disclaimers, and limitations in the end user license agreement accompanying 
* the software package with which this file was provided.
********************************************************************************/



using System;
using System.Diagnostics;
using System.Windows.Forms;

namespace PRS_v2_10
{
    public partial class CyPRSControl : UserControl
    {
        #region Variables

        private enum CyState
        {
            LOADING,
            CUSTOM_VIEW,
            EXPRESSION_VIEW
        }

        private CyState m_state;
        public CyPRSParameters m_parameters;
        private static int[,] STANDARD_POLYNOMIALS = new int[63, 4] { 
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
        { 32, 30, 26, 25},
        { 33, 32, 29, 27},
        { 34, 31, 30, 26},
        { 35, 34, 28, 27},
        { 36, 35, 29, 28},
        { 37, 36, 33, 31},
        { 38, 37, 33, 32},
        { 39, 38, 35, 32},
        { 40, 37, 36, 35},
        { 41, 40, 39, 38},
        { 42, 40, 37, 35},
        { 43, 42, 38, 37},
        { 44, 42, 39, 38},
        { 45, 44, 42, 41},
        { 46, 40, 39, 38},
        { 47, 46, 43, 42},
        { 48, 44, 41, 39},
        { 49, 45, 44, 43},
        { 50, 48, 47, 46},
        { 51, 50, 48, 45},
        { 52, 51, 49, 46},
        { 53, 52, 51, 47},
        { 54, 51, 48, 46},
        { 55, 54, 53, 49},
        { 56, 54, 52, 49},
        { 57, 55, 54, 52},
        { 58, 57, 53, 52},
        { 59, 57, 55, 52},
        { 60, 58, 56, 55},
        { 61, 60, 59, 56},
        { 62, 59, 57, 56},
        { 63, 62, 59, 58},
        { 64, 63, 61, 60}};

        private int[] m_customPolinomial;

        private bool m_setDefaultSeed = false;
        private uint m_resolution;

        #endregion

        #region Properties

        private uint Resolution
        {
            get { return m_resolution; }
            set 
            {
                if ((value >= 2) && (value <= 64))
                {
                    m_resolution = value;

                    if (m_state == CyState.CUSTOM_VIEW)
                    {
                        m_parameters.m_resolution = m_resolution;
                        m_parameters.SetParam(CyPRSParameters.PARAM_RESOLUTION);
                    }

                    FillTextBoxWithStandartPolynom();
                    CalcPolynomial();

                    if (m_setDefaultSeed)
                    {
                        string seed = (((ulong) 1 << (int)Resolution) - 1).ToString("X");
                        if (value == 64) seed = ulong.MaxValue.ToString("X");
                        textBoxSeed.Text = seed;

                        m_setDefaultSeed = false;
                    }

                    if (m_parameters.m_advancedPage != null)
                    {
                        m_parameters.m_advancedPage.UpdateForm();
                    }
                }
            }
        }

        #endregion Properties

        #region Constructors

        public CyPRSControl(CyPRSParameters parameters)
        {
            InitializeComponent();
            this.m_parameters = parameters;
            UpdateForm();
        }

        #endregion Constructors

        public void UpdateForm()
        {
            m_state = CyState.EXPRESSION_VIEW;
            UpdateResolution();
            UpdatePolyValue();
            UpdateSeedValue();
            UpdateRunMode();
            m_state = CyState.CUSTOM_VIEW;
        }

        #region Private functions

        private void UpdateResolution()
        {
            string errMsg = string.Empty;

            if (m_parameters.m_resolution >= numUpDownResolution.Minimum &&
                m_parameters.m_resolution <= numUpDownResolution.Maximum)
            {
                numUpDownResolution.Value = m_parameters.m_resolution;
            }
            else if (m_parameters.m_resolution < numUpDownResolution.Minimum)
            {
                numUpDownResolution.Value = numUpDownResolution.Minimum;
                errMsg = "Value must be between 2 and 64";
            }
            else if (m_parameters.m_resolution > numUpDownResolution.Maximum)
            {
                numUpDownResolution.Value = numUpDownResolution.Maximum;
                errMsg = "Value must be between 2 and 64";
            }
            errorProvider1.SetError(numUpDownResolution, errMsg);
        }

        private void UpdateSeedValue()
        {
            //CDT 54994. Fix binding error when PRS[v0.5] is updated to newer version
            if (m_parameters.SeedValue == 0) m_parameters.SeedValue = 0x000000FF;
            if (m_parameters.SeedValue > 0)
            {
                textBoxSeed.Text = m_parameters.SeedValue.ToString("X");
            }
            else
            {
                m_setDefaultSeed = true;
            }
        }

        private void UpdatePolyValue()
        {           
                UInt64 loadedPolyValue = m_parameters.m_polyValue;
                Resolution = m_parameters.m_resolution;

                if ((loadedPolyValue != m_parameters.m_polyValue) && (loadedPolyValue > 0))
                {
                    checkBoxCustom.Checked = true;
                    m_customPolinomial = new int[Resolution];

                    string s;
                    if ((loadedPolyValue & ((ulong)1 << 63)) != 0)
                    {
                        s = Convert.ToString(Convert.ToInt64(loadedPolyValue & (ulong.MaxValue >> 1)), 2);
                        s = "1" + s;
                    }
                    else
                    {
                        s = Convert.ToString(Convert.ToInt64(loadedPolyValue), 2);
                        for (int i = s.Length; i < Resolution; i++)
                        {
                            s = "0" + s;
                        }
                    }

                    for (int i = 0; i < Resolution; i++)
                    {
                        if ((s.Length >= i) && (s[i] == '1'))
                            m_customPolinomial[i] = ((int)Resolution - i);
                    }

                    textBoxLFSR.Text = "";
                    for (int i = 0; i < m_customPolinomial.Length; i++)
                    {
                        if (m_customPolinomial[i] > 0)
                            textBoxLFSR.Text += m_customPolinomial[i].ToString() + ", ";
                    }
                    textBoxLFSR.Text = textBoxLFSR.Text.TrimEnd(' ', ',');
                    textBoxLFSR.ReadOnly = false;

                    CalcPolynomial();
                }
        }

        private void UpdateRunMode()
        {
            if (m_parameters.m_runMode == 0)
            {
                radioButton1.Checked = true;
            }
            else
            {
                radioButton2.Checked = true;
            }
        }

        private void FillTextBoxWithStandartPolynom()
        {
            textBoxLFSR.Text = "";
            if (Resolution > 4)
            {
                for (int j = 0; j < 3; j++)
                {
                    textBoxLFSR.Text += STANDARD_POLYNOMIALS[Resolution - 2, j].ToString() + ", ";
                }
                textBoxLFSR.Text += STANDARD_POLYNOMIALS[Resolution - 2, 3].ToString();
            }
            else
            {
                textBoxLFSR.Text = STANDARD_POLYNOMIALS[Resolution - 2, 0].ToString() + ", " +
                                   STANDARD_POLYNOMIALS[Resolution - 2, 1];
            }
            textBoxLFSR_Validated(textBoxLFSR, new EventArgs());
        }

        private UInt64 CalcPolynomial()
        {
            UInt64 result = 0;
            if ((Resolution >= 2) && (Resolution <= 64))
            {
                if (!checkBoxCustom.Checked)
                {
                    if (Resolution > 4)
                    {
                        for (int i = 0; i < 4; i++)
                        {
                            result += Convert.ToUInt64(Math.Pow(2, STANDARD_POLYNOMIALS[Resolution - 2, i] - 1));
                        }
                    }
                    else
                    {
                        for (int i = 0; i < 2; i++)
                        {
                            result += Convert.ToUInt64(Math.Pow(2, STANDARD_POLYNOMIALS[Resolution - 2, i] - 1));
                        }
                    }
                }
                else
                {
                    for (int i = 0; i < m_customPolinomial.Length; i++)
                    {
                        result += Convert.ToUInt64(Math.Pow(2, m_customPolinomial[i] - 1));
                    }
                }

                m_parameters.m_polyValue = result;

                string PolyTextHexValue = result.ToString("x");
                for (int i = PolyTextHexValue.Length; i < (Resolution - 1) / 4 + 1; i++)
                {
                    PolyTextHexValue = "0" + PolyTextHexValue;
                }
                textBoxResult.Text = PolyTextHexValue.ToUpper();
            }
            return result;
        }

        #endregion Private functions

        #region Events

        private void textBoxResult_KeyPress(object sender, KeyPressEventArgs e)
        {
            if ((e.KeyChar != (char)Keys.Enter) && (e.KeyChar != (char)Keys.Back))
            {
                if (((e.KeyChar < '0') || (e.KeyChar > '9')) && ((e.KeyChar < 'A') || (e.KeyChar > 'F')) && 
                    ((e.KeyChar < 'a') || (e.KeyChar > 'f')))
                    e.Handled = true;
                if (textBoxResult.Text.Length >= 16)
                    e.Handled = true; 
            }
            if (e.KeyChar == (char)Keys.Enter)
            {
                e.Handled = true;
            }
        }

        private void numUpDownResolution_ValueChanged(object sender, EventArgs e)
        {
            if (m_state != CyState.CUSTOM_VIEW) return;

            if (Convert.ToInt32(numUpDownResolution.Value) != m_parameters.m_resolution)
                m_setDefaultSeed = true;

            if (Resolution != Convert.ToUInt16(numUpDownResolution.Value))
            {
                Resolution = Convert.ToUInt16(numUpDownResolution.Value);
            }

            errorProvider1.SetError(numUpDownResolution, string.Empty);
        }

        private void textBoxSeed_TextChanged(object sender, EventArgs e)
        {
            if (m_state != CyState.CUSTOM_VIEW) return;

            if (textBoxSeed.Text == "")
            {
                m_parameters.SeedValue = 0;
            }
            else
            {
                m_parameters.SeedValue = Convert.ToUInt64(textBoxSeed.Text, 16);
            }

        }

        private void textBoxResult_TextChanged(object sender, EventArgs e)
        {
            if (m_state != CyState.CUSTOM_VIEW) return;

            try
            {
                if (textBoxResult.Text == "")
                {
                    m_parameters.m_polyValue = 0;
                }
                else
                {
                    m_parameters.m_polyValue = Convert.ToUInt64(textBoxResult.Text, 16);
                }
                m_parameters.SetParam(CyPRSParameters.PARAM_POLYVALUE);
            }
            catch(Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
        }

        private void textBoxSeed_KeyPress(object sender, KeyPressEventArgs e)
        {
            if ((e.KeyChar != (char)Keys.Enter) && (e.KeyChar != (char)Keys.Back))
            {
                if (((e.KeyChar < '0') || (e.KeyChar > '9')) && ((e.KeyChar < 'A') || (e.KeyChar > 'F')) && 
                    ((e.KeyChar < 'a') || (e.KeyChar > 'f')))
                    e.Handled = true;
                if (textBoxSeed.Text.Length >= 16)
                    e.Handled = true;
            }
        }

        private void radioButton1_CheckedChanged(object sender, EventArgs e)
        {
            if (m_state != CyState.CUSTOM_VIEW) return;

            if (radioButton1.Checked)
            {
                m_parameters.m_runMode = 0;
            }
            else
            {
                m_parameters.m_runMode = 1;
            }
            m_parameters.SetParam(CyPRSParameters.PARAM_RUNMODE);
        }

        private void checkBoxCustom_CheckedChanged(object sender, EventArgs e)
        {
            if (m_state != CyState.CUSTOM_VIEW) return;

            if (checkBoxCustom.Checked)
            {
                textBoxLFSR.ReadOnly = false;
                textBoxLFSR.TabStop = true;
            }
            else
            {
                textBoxLFSR.ReadOnly = true;
                textBoxLFSR.TabStop = false;
                Resolution = Resolution;
            }
        }

        private void textBoxLFSR_Validated(object sender, EventArgs e)
        {
            // Parse polynomials
            string s = textBoxLFSR.Text;
            try
            {
                string[] arrayOfNums = s.Split(',');
                m_customPolinomial = new int[arrayOfNums.Length];
                for (int i = 0; i < arrayOfNums.Length; i++)
                {
                    m_customPolinomial[i] = Convert.ToInt32(arrayOfNums[i]);

                    if ((m_customPolinomial.Length > 0) && (m_customPolinomial[i] > Resolution))
                    {
                        MessageBox.Show("LFSR items could not be greater than Resolution.", "PRS", MessageBoxButtons.OK,
                                        MessageBoxIcon.Warning);
                        return;
                    }
                }

                CalcPolynomial();
            }
            catch
            {
                MessageBox.Show("Invalid format for input data. Please, enter comma-separated values.", "PRS",
                                MessageBoxButtons.OK, MessageBoxIcon.Warning);
                FillTextBoxWithStandartPolynom();
                CalcPolynomial();
            }
        }

        #endregion

        #region 'Enter' key override
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
        #endregion 'Enter' key override
    }
}
