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

namespace CyCustomizer.PRS_v0_5
{
    public partial class CyPRSControl : UserControl
    {
        #region Variables

        public PRSParameters Parameters;
        int[,] StandartPolynomials = new int[63,4] { 
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

        int[] CustomPolinomial;

        bool SetDefaultSeed = false;
        int _Resolution;

        #endregion

        public int Resolution
        {
            get { return _Resolution; }
            set 
            {
                if ((value >= 2) && (value <= 32))
                {
                    _Resolution = value;
                    Parameters.Resolution = _Resolution;

                    FillTextBoxWithStandartPolynom();

                    CalcPolynomial();

                    if (SetDefaultSeed)
                    {
                        string seed = (((ulong) 1 << Resolution) - 1).ToString("X");
                        textBoxSeed.Text = seed;
                        Parameters.SeedValue = Convert.ToUInt32(textBoxSeed.Text, 16);
                        Parameters.SetParam("SeedValue");
                        Parameters.CommitParams();

                        SetDefaultSeed = false;
                    }
                }
            }
        }

        public CyPRSControl()
        {
            InitializeComponent();

            Parameters = new PRSParameters();
            Resolution = 8;
        }

        public CyPRSControl(PRSParameters parameters)
        {
            InitializeComponent();

            this.Parameters = parameters;
            try
            {
                if (Parameters.SeedValue > 0)
                {
                    textBoxSeed.Text = Parameters.SeedValue.ToString("X");
                }
                else
                {
                    SetDefaultSeed = true;
                }

                if (Parameters.RunMode == 0)
                {
                    radioButton1.Checked = true;
                }
                else
                {
                    radioButton2.Checked = true;
                }

                UInt64 loadedResult = Parameters.PolyValue;
                numUpDownResolution.Value = Parameters.Resolution;
                if (Resolution != Parameters.Resolution)
                {
                    Resolution = Parameters.Resolution;
                }
                if ((loadedResult != Parameters.PolyValue) && (loadedResult > 0))
                {
                    checkBoxCustom.Checked = true;
                    CustomPolinomial = new int[Resolution];

                    if (loadedResult == UInt64.MaxValue)
                    {
                        
                        for (int i = 0; i < Resolution; i++)
                        {
                            CustomPolinomial[i] = Resolution - i;
                        }
                    }
                    else
                    {
                        string s = Convert.ToString(Convert.ToInt64(loadedResult), 2);
                        for (int i = s.Length; i < Resolution; i++)
                        {
                            s = "0" + s;
                        }

                            for (int i = 0; i < Resolution; i++)
                            {
                                if ((s.Length >= i) && (s[i] == '1'))
                                    CustomPolinomial[i] = Resolution - i;
                            }
                    }

                    textBoxLFSR.Text = "";
                    for (int i = 0; i < CustomPolinomial.Length; i++)
                    {
                        if (CustomPolinomial[i] > 0)
                        textBoxLFSR.Text += CustomPolinomial[i].ToString() + ", ";
                    }
                    textBoxLFSR.Text = textBoxLFSR.Text.TrimEnd(' ', ',');

                    CalcPolynomial();
                }    
            }
            catch
            {
                MessageBox.Show("Problem with parameters initialization.");
            }
        }

        void FillTextBoxWithStandartPolynom()
        {
            textBoxLFSR.Text = "";
            if (Resolution > 4)
            {
                for (int j = 0; j < 3; j++)
                {
                    textBoxLFSR.Text += StandartPolynomials[Resolution - 2, j].ToString() + ", ";
                }
                textBoxLFSR.Text += StandartPolynomials[Resolution - 2, 3].ToString();
            }
            else
            {
                textBoxLFSR.Text = StandartPolynomials[Resolution - 2, 0].ToString() + ", " + StandartPolynomials[Resolution - 2, 1];
            }
            textBoxLFSR_Validated(textBoxLFSR, new EventArgs());
        }

        UInt64 CalcPolynomial()
        {
            UInt64 result = 0;
            if ((Resolution >= 2) && (Resolution <= 32))
            {
                if (!checkBoxCustom.Checked)
                {
                    if (Resolution > 4)
                    {
                        for (int i = 0; i < 4; i++)
                        {
                            result += Convert.ToUInt64(Math.Pow(2, StandartPolynomials[Resolution - 2, i] - 1));
                        }
                    }
                    else
                    {
                        for (int i = 0; i < 2; i++)
                        {
                            result += Convert.ToUInt64(Math.Pow(2, StandartPolynomials[Resolution - 2, i] - 1));
                        }
                    }
                }
                else
                {
                    for (int i = 0; i < CustomPolinomial.Length; i++)
                    {
                        result += Convert.ToUInt64(Math.Pow(2, CustomPolinomial[i] - 1));
                    }
                }


                Parameters.PolyValue = result;

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
            if (Convert.ToInt32(numUpDownResolution.Value) != Parameters.Resolution)
                SetDefaultSeed = true;

            if (Resolution != Convert.ToInt32(numUpDownResolution.Value))
            {
                Resolution = Convert.ToInt32(numUpDownResolution.Value);
                Parameters.SetParam("Resolution");
                Parameters.CommitParams();
            }
            
        }

        #endregion

        private void textBoxSeed_TextChanged(object sender, EventArgs e)
        {
            try
            {
                if (textBoxSeed.Text == "")
                {
                    Parameters.SeedValue = 0;
                }
                else
                {
                    Parameters.SeedValue = Convert.ToUInt64(textBoxSeed.Text, 16);
                }
                Parameters.SetParam("SeedValue");
                Parameters.CommitParams();
            }
            catch
            {
                //MessageBox.Show("Wrong symbol");
            }
        }

        private void textBoxResult_TextChanged(object sender, EventArgs e)
        {
            try
            {
                if (textBoxResult.Text == "")
                {
                    Parameters.PolyValue = 0;
                }
                else
                {
                    Parameters.PolyValue = Convert.ToUInt64(textBoxResult.Text, 16);
                }
                Parameters.SetParam("PolyValue");
                Parameters.CommitParams();
            }
            catch
            {
                //MessageBox.Show("Wrong symbol");
            }
        }

        private void textBoxSeed_KeyPress(object sender, KeyPressEventArgs e)
        {
            if ((e.KeyChar != (char)Keys.Enter) && (e.KeyChar != (char)Keys.Back))
            {
                if (((e.KeyChar < '0') || (e.KeyChar > '9')) && ((e.KeyChar < 'A') || (e.KeyChar > 'F')) && ((e.KeyChar < 'a') || (e.KeyChar > 'f')))
                    e.Handled = true;
                if (textBoxSeed.Text.Length >= 16)
                    e.Handled = true;
            }
        }

        private void radioButton1_CheckedChanged(object sender, EventArgs e)
        {
            if (radioButton1.Checked)
            {
                Parameters.RunMode = 0;
            }
            else
            {
                Parameters.RunMode = 1;
            }
            Parameters.SetParam("RunMode");
            Parameters.CommitParams();
        }

        private void checkBoxCustom_CheckedChanged(object sender, EventArgs e)
        {
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
                CustomPolinomial = new int[arrayOfNums.Length];
                for (int i = 0; i < arrayOfNums.Length; i++)
                {
                    CustomPolinomial[i] = Convert.ToInt32(arrayOfNums[i]);

                    if ((CustomPolinomial.Length > 0) && (CustomPolinomial[i] > Resolution))
                    {
                        MessageBox.Show("LFSR items could not be greater than Resolution.", "PRS", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                        return;
                    }
                }

                CalcPolynomial();
            }
            catch
            {
                MessageBox.Show("Invalid format for input data. Please, enter comma-separated values.", "PRS", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                FillTextBoxWithStandartPolynom();
                CalcPolynomial();
            }
        }
    }
}
