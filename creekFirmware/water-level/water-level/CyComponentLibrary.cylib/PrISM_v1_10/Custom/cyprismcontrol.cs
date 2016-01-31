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
using System.Data;
using System.Text;
using System.Windows.Forms;
using System.Collections;
using System.Diagnostics;

namespace PrISM_v1_10
{
    public partial class CyPRISMControl : UserControl
    {
        #region Variables
        public CyPRISMParameters m_Parameters;
        int[,] m_StandartPolynomials = new int[31, 4] { 
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

        int[] m_CustomPolinomial;

        //bool FieldModified = false;
        bool m_SetDefaultSeed = false;

        #endregion

        public int Resolution
        {
            get { return m_Parameters.m_Resolution; }
            set
            {
                if ((value >= 2) && (value <= 32))
                {
                    m_Parameters.m_Resolution = value;

                    FillPulseComboBoxes();
                    FillTextBoxWithStandartPolynom();

                    CalcPolynomial();
                    if (m_SetDefaultSeed)
                    {
                        string seed = (((ulong)1 << Resolution) - 1).ToString("X").ToUpper();
                        textBoxSeed.Text = seed;
                        m_Parameters.m_SeedValue = Convert.ToUInt32(textBoxSeed.Text, 16);
                        m_Parameters.SetParam("SeedValue");
                        m_Parameters.CommitParams();

                        m_SetDefaultSeed = false;
                    }
                }
            }
        }

        public CyPRISMControl()
        {
            InitializeComponent();

            m_Parameters = new CyPRISMParameters();
            Resolution = 8;
            FillPulseComboBoxes();
        }

        public CyPRISMControl(CyPRISMParameters parameters)
        {
            InitializeComponent();


            this.m_Parameters = parameters;
            try
            {
                if (m_Parameters.m_SeedValue > 0)
                {
                    textBoxSeed.Text = m_Parameters.m_SeedValue.ToString("X");
                }
                else
                {
                    m_SetDefaultSeed = true;
                }

                numUpDownResolution.Value = m_Parameters.m_Resolution;
                if (Resolution != m_Parameters.m_Resolution)
                {
                    Resolution = m_Parameters.m_Resolution;
                }

                FillPulseComboBoxes();
                comboBoxPulse0.Text = m_Parameters.m_Density0.ToString();
                comboBoxPulse1.Text = m_Parameters.m_Density1.ToString();

                comboBoxCompare0.SelectedIndex = (byte)m_Parameters.m_CompareType0;                
                comboBoxCompare1.SelectedIndex = (byte)m_Parameters.m_CompareType1;

                UInt32 loadedResult = m_Parameters.m_PolyValue;
                if ((loadedResult != CalcPolynomial(m_Parameters.m_Resolution)) && (loadedResult > 0))
                {
                    checkBoxCustom.Checked = true;
                    m_CustomPolinomial = new int[Resolution];

                    string s;
                    if ((loadedResult & ((ulong)1 << 31)) != 0)
                    {
                        s = Convert.ToString(Convert.ToInt32(loadedResult & (ulong.MaxValue >> 1)), 2);
                        s = "1" + s;
                    }
                    else
                    {
                        s = Convert.ToString(Convert.ToInt32(loadedResult), 2);
                        for (int i = s.Length; i < Resolution; i++)
                        {
                            s = "0" + s;
                        }
                    }

                    for (int i = 0; i < Resolution; i++)
                    {
                        if ((s.Length >= i) && (s[i] == '1'))
                            m_CustomPolinomial[i] = Resolution - i;
                    }

                    textBoxLFSR.Text = "";
                    for (int i = 0; i < m_CustomPolinomial.Length; i++)
                    {
                        if (m_CustomPolinomial[i] > 0)
                            textBoxLFSR.Text += m_CustomPolinomial[i].ToString() + ", ";
                    }
                    textBoxLFSR.Text = textBoxLFSR.Text.TrimEnd(' ', ',');

                    CalcPolynomial();
                }
                else
                    FillTextBoxWithStandartPolynom();
                checkBoxPulseTypeHardcoded.Checked = m_Parameters.m_PulseTypeHardcoded;
            }
            catch
            {
                MessageBox.Show("Problem with parameters initialization.");
            }

        }

        void FillPulseComboBoxes()
        {
            comboBoxPulse0.Items.Clear();
            comboBoxPulse1.Items.Clear();
            //comboBoxPulse0.Items.Add(0);
            //comboBoxPulse1.Items.Add(0);
            for (int i = 0; i < Resolution; i++)
            {
                comboBoxPulse0.Items.Add((UInt32)Math.Pow(2, i));
                comboBoxPulse1.Items.Add((UInt32)Math.Pow(2, i));
            }
            try
            {
                if ((m_Parameters.m_Density0 > (UInt32)Math.Pow(2, Resolution - 1)) || (m_Parameters.m_Density0 == 0))
                {
                    comboBoxPulse0.Text = ((UInt32)Math.Pow(2, Resolution - 1)).ToString();
                    // Parameters.Density0 = Convert.ToUInt32(comboBoxPulse0.Text);
                }
                else
                {
                    comboBoxPulse0.Text = m_Parameters.m_Density0.ToString();
                }

                if ((m_Parameters.m_Density1 > (UInt32)Math.Pow(2, Resolution - 1) || (m_Parameters.m_Density1 == 0)))
                {
                    comboBoxPulse1.Text = ((UInt32)Math.Pow(2, Resolution - 1)).ToString();
                }
                else
                {
                    comboBoxPulse1.Text = m_Parameters.m_Density1.ToString();
                }
            }
            catch
            {

            }
        }

        void FillTextBoxWithStandartPolynom()
        {
            textBoxLFSR.Text = "";
            if (Resolution > 4)
            {
                for (int j = 0; j < 3; j++)
                {
                    textBoxLFSR.Text += m_StandartPolynomials[Resolution - 2, j].ToString() + ", ";
                }
                textBoxLFSR.Text += m_StandartPolynomials[Resolution - 2, 3].ToString();
            }
            else
            {
                textBoxLFSR.Text = m_StandartPolynomials[Resolution - 2, 0].ToString() + ", " + m_StandartPolynomials[Resolution - 2, 1];
            }
        }

        UInt32 CalcPolynomial()
        {
            return CalcPolynomial(Resolution);
        }
        UInt32 CalcPolynomial(int Resolution)
        {
            UInt32 result = 0;
            if ((Resolution >= 2) && (Resolution <= 32))
            {
                if (!checkBoxCustom.Checked)
                {
                    if (Resolution > 4)
                    {
                        for (int i = 0; i < 4; i++)
                        {
                            result += Convert.ToUInt32(Math.Pow(2, m_StandartPolynomials[Resolution - 2, i] - 1));
                        }
                    }
                    else
                    {
                        for (int i = 0; i < 2; i++)
                        {
                            result += Convert.ToUInt32(Math.Pow(2, m_StandartPolynomials[Resolution - 2, i] - 1));
                        }
                    }
                }
                else
                {
                    for (int i = 0; i < m_CustomPolinomial.Length; i++)
                    {
                        result += Convert.ToUInt32(Math.Pow(2, m_CustomPolinomial[i] - 1));
                    }
                }


                m_Parameters.m_PolyValue = result;

                string PolyTextHexValue = result.ToString("x");
                for (int i = PolyTextHexValue.Length; i < (Resolution - 1) / 4 + 1; i++)
                {
                    PolyTextHexValue = "0" + PolyTextHexValue;
                }
                textBoxResult.Text = PolyTextHexValue.ToUpper();
            }
            return result;
        }

        #region Events

        private void textBoxResult_KeyPress(object sender, KeyPressEventArgs e)
        {
            if ((e.KeyChar != (char)Keys.Enter) && (e.KeyChar != (char)Keys.Back))
            {
                if (((e.KeyChar < '0') || (e.KeyChar > '9')) && ((e.KeyChar < 'A') || (e.KeyChar > 'F')) && ((e.KeyChar < 'a') || (e.KeyChar > 'f')))
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
            if ((m_Parameters.GlobalEditMode))
            {
                if (Convert.ToInt32(numUpDownResolution.Value) != m_Parameters.m_Resolution)
                    m_SetDefaultSeed = true;
                if (Resolution != Convert.ToInt32(numUpDownResolution.Value))
                {
                    Resolution = Convert.ToInt32(numUpDownResolution.Value);
                    m_Parameters.SetParam("Resolution");
                    m_Parameters.CommitParams();
                }
            }
        }

        #endregion

        private void textBoxSeed_TextChanged(object sender, EventArgs e)
        {
            if (m_Parameters.GlobalEditMode)
                try
                {
                    if (textBoxSeed.Text == "")
                    {
                        m_Parameters.m_SeedValue = 0;
                    }
                    else
                    {
                        m_Parameters.m_SeedValue = Convert.ToUInt32(textBoxSeed.Text, 16);
                    }
                    m_Parameters.SetParam("SeedValue");
                    m_Parameters.CommitParams();
                }
                catch
                {
                    //MessageBox.Show("Wrong symbol");
                }
        }


        private void textBoxResult_TextChanged(object sender, EventArgs e)
        {
            if ((m_Parameters.GlobalEditMode))
            {
                try
                {
                    if (textBoxResult.Text == "")
                    {
                        m_Parameters.m_PolyValue = 0;
                    }
                    else
                    {
                        m_Parameters.m_PolyValue = Convert.ToUInt32(textBoxResult.Text, 16);
                    }
                    m_Parameters.SetParam("PolyValue");
                    m_Parameters.CommitParams();

                }
                catch
                {
                    //MessageBox.Show("Wrong symbol");
                }
            }
        }

        private void textBoxSeed_KeyPress(object sender, KeyPressEventArgs e)
        {
            if ((e.KeyChar != (char)Keys.Enter) && (e.KeyChar != (char)Keys.Back))
            {
                if (((e.KeyChar < '0') || (e.KeyChar > '9')) && ((e.KeyChar < 'A') || (e.KeyChar > 'F')) && ((e.KeyChar < 'a') || (e.KeyChar > 'f')))
                    e.Handled = true;
                if (textBoxSeed.Text.Length >= 8)
                    e.Handled = true;
            }
        }

        private void checkBoxCustom_CheckedChanged(object sender, EventArgs e)
        {
            if (checkBoxCustom.Checked)
            {
                textBoxLFSR.ReadOnly = false;
                textBoxLFSR.TabStop = true;
                textBoxLFSR_Validated(null, null);
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
            if (m_Parameters.GlobalEditMode)
                try
                {
                    string[] arrayOfNums = s.Split(',');
                    m_CustomPolinomial = new int[arrayOfNums.Length];
                    for (int i = 0; i < arrayOfNums.Length; i++)
                    {
                        m_CustomPolinomial[i] = Convert.ToInt32(arrayOfNums[i]);

                        if ((m_CustomPolinomial.Length > 0) && (m_CustomPolinomial[i] > Resolution))
                        {
                            MessageBox.Show("LFSR items could not be greater than Resolution.", "PRISM", MessageBoxButtons.OK,
                                            MessageBoxIcon.Warning);
                            return;
                        }
                    }

                    CalcPolynomial();
                }
                catch
                {
                    MessageBox.Show("Invalid format for input data. Please, enter comma-separated values.", "PRISM", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                    FillTextBoxWithStandartPolynom();
                    CalcPolynomial();
                }

        }

        private void comboBoxPulse0_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (sender == comboBoxPulse0)
            {
                if (m_Parameters.m_Density0 != Convert.ToUInt32(comboBoxPulse0.Text))
                {
                    m_Parameters.m_Density0 = Convert.ToUInt32(comboBoxPulse0.Text);
                    m_Parameters.SetParam("Density0");
                }
            }
            else if (sender == comboBoxPulse1)
            {
                if (m_Parameters.m_Density1 != Convert.ToUInt32(comboBoxPulse1.Text))
                {
                    m_Parameters.m_Density1 = Convert.ToUInt32(comboBoxPulse1.Text);
                    m_Parameters.SetParam("Density1");
                }
            }
            else if (sender == comboBoxCompare0)
            {
                if ((byte)m_Parameters.m_CompareType0 != Convert.ToByte(comboBoxCompare0.SelectedIndex))
                {
                    m_Parameters.m_CompareType0 = (E_CompareType)Convert.ToByte(comboBoxCompare0.SelectedIndex);
                    m_Parameters.SetParam("CompareType0");
                }
            }
            else if (sender == comboBoxCompare1)
            {
                if ((byte)m_Parameters.m_CompareType1 != Convert.ToByte(comboBoxCompare1.SelectedIndex))
                {
                    m_Parameters.m_CompareType1 = (E_CompareType)Convert.ToByte(comboBoxCompare1.SelectedIndex);
                    m_Parameters.SetParam("CompareType1");
                }
            }
            m_Parameters.CommitParams();
        }

        private void checkBoxPulseTypeHardcoded_CheckedChanged(object sender, EventArgs e)
        {
            if (m_Parameters.GlobalEditMode)
            {
                if (checkBoxPulseTypeHardcoded.Checked)
                {
                    m_Parameters.m_PulseTypeHardcoded = true;
                }
                else
                {
                    m_Parameters.m_PulseTypeHardcoded = false;
                }
                m_Parameters.SetParam("PulseTypeHardcoded");
                m_Parameters.CommitParams();
            }
        }
    }
}
