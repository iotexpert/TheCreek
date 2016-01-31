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

namespace CRC_v1_10
{
    public partial class CyCRCControl : UserControl
    {
        public CyCRCParameters Parameters;
        bool[] checkedCells = new bool[64];
        ArrayList[] StandartPolynomials = new ArrayList[22];
        Bitmap PolynomBmp;
        bool FieldModified = false;
        int _N;

        DataGridViewCellStyle checkedCellsStyle;

        public int N
        {
            get { return _N; }
            set 
            {
                if ((value <= 64) && (value > 0))
                {
                    _N = value;
                    Parameters.m_Resolution = _N;
                    Parameters.SetParam("Resolution");
                    Parameters.CommitParams();
                    for (int i = 0; i < 4; i++)
                    {
                        for (int j = 0; j < dataGridCheck.ColumnCount; j++)
                        {
                            if (i * dataGridCheck.ColumnCount + j < N)
                            {
                                dataGridCheck[j, i].Value = N - (i * dataGridCheck.ColumnCount + j);
                            }
                            else
                            {
                                dataGridCheck[j, i].Value = "";

                            }
                            dataGridCheck[j, i].Style = dataGridCheck.DefaultCellStyle;
                            dataGridCheck[j, i].Style.SelectionBackColor = Color.White;
                            dataGridCheck[j, i].Style.SelectionForeColor = Color.Black;
                            checkedCells[i * dataGridCheck.ColumnCount + j] = false;
                        }
                    }

                    CalcPolynomial();
                    //comboBoxStandart.SelectedIndex = 0;
                    DrawPolynomBmp();
                    pictureBoxPolynom.Height = (_N / 16 + 1) * 22;
                    groupBoxPolynom.Height = pictureBoxPolynom.Top + pictureBoxPolynom.Height + 3;
                    pictureBoxPolynom.Invalidate();

                    string seed = "0";
                    if (Parameters.m_SeedValue.ToString("x").Length <= (N - 1) / 4 + 1)
                    {
                        seed = Parameters.m_SeedValue.ToString("x").ToUpper();
                    }
                    for (int i = seed.Length; i < (N - 1) / 4 + 1; i++)
                    {
                        seed = "0" + seed;
                    }
                    textBoxSeed.Text = seed;
                    Parameters.SetParam("SeedValue");
                    Parameters.CommitParams();
                }
            }
        }

        public CyCRCControl()
        {
            InitializeComponent();

            InitStandartPolynomials();
            InitDataGrid();
            InitBmp();
            Parameters = new CyCRCParameters();
        }

        public CyCRCControl(CyCRCParameters parameters)
        {
            InitializeComponent();

            InitStandartPolynomials();
            InitDataGrid();
            InitBmp();

            this.Parameters = parameters;
            try
            {
                if (Parameters.m_SeedValue > 0)
                {
                    textBoxSeed.Text = Parameters.m_SeedValue.ToString("x");
                }
                else
                {
                    textBoxSeed.Text = "0";
                }

                comboBoxStandart.Text = Parameters.m_PolyName;
                if ((Parameters.m_PolyValue > 0) && (comboBoxStandart.SelectedIndex < 1))
                {
                    FieldModified = true;
                    textBoxResult.Text = Parameters.m_PolyValue.ToString("x").ToUpper();
                    textBoxResult_Validated(textBoxResult, new EventArgs());
                }
                
            }
            catch
            {
                MessageBox.Show("Problem with parameters initialization.");
            }

        }

        void InitDataGrid()
        {
            checkedCellsStyle = new DataGridViewCellStyle();
            checkedCellsStyle.BackColor = Color.SkyBlue;
            dataGridCheck.Rows.Clear();
            for (int i = 0; i < 4; i++)
            {
                dataGridCheck.Rows.Add();
                for (int j = 0; j < dataGridCheck.ColumnCount; j++)
                {
                    //dataGridCheck[j, i].Value = i * dataGridCheck.ColumnCount + j + 1;
                }
            }
        }

        void InitStandartPolynomials()
        {
            for (int i = 0; i < StandartPolynomials.Length; i++)
            {
                StandartPolynomials[i] = new ArrayList();
            }
            //CRC-1
            StandartPolynomials[0].Add(1);
            StandartPolynomials[0].Add("Parity");
            //CRC-4-ITU
            StandartPolynomials[1].Add(4);
            StandartPolynomials[1].Add(1);
            StandartPolynomials[1].Add("ITU G.704");
            //CRC-5-ITU
            StandartPolynomials[2].Add(5);
            StandartPolynomials[2].Add(4);
            StandartPolynomials[2].Add(2);
            StandartPolynomials[2].Add("ITU G.704");
            //CRC-5-USB
            StandartPolynomials[3].Add(5);
            StandartPolynomials[3].Add(2);
            StandartPolynomials[3].Add("USB");
            //CRC-6-ITU
            StandartPolynomials[4].Add(6);
            StandartPolynomials[4].Add(1);
            StandartPolynomials[4].Add("ITU G.704");
            //CRC-7
            StandartPolynomials[5].Add(7);
            StandartPolynomials[5].Add(3);
            StandartPolynomials[5].Add("Telecom systems, MMC");
            //CRC-8-ATM
            StandartPolynomials[6].Add(8);
            StandartPolynomials[6].Add(2);
            StandartPolynomials[6].Add(1);
            StandartPolynomials[6].Add("ATM HEC");
            //CRC-8-CCITT
            StandartPolynomials[7].Add(8);
            StandartPolynomials[7].Add(7);
            StandartPolynomials[7].Add(3);
            StandartPolynomials[7].Add(2);
            StandartPolynomials[7].Add("1-Wire bus");
            //CRC-8-Maxim
            StandartPolynomials[8].Add(8);
            StandartPolynomials[8].Add(5);
            StandartPolynomials[8].Add(4);
            StandartPolynomials[8].Add("1-Wire bus");
            //CRC-8
            StandartPolynomials[9].Add(8);
            StandartPolynomials[9].Add(7);
            StandartPolynomials[9].Add(6);
            StandartPolynomials[9].Add(4);
            StandartPolynomials[9].Add(2);
            StandartPolynomials[9].Add("General");
            //CRC-8-SAE
            StandartPolynomials[10].Add(8);
            StandartPolynomials[10].Add(4);
            StandartPolynomials[10].Add(3);
            StandartPolynomials[10].Add(2);
            StandartPolynomials[10].Add("SAE J1850");
            //CRC-10
            StandartPolynomials[11].Add(10);
            StandartPolynomials[11].Add(9);
            StandartPolynomials[11].Add(5);
            StandartPolynomials[11].Add(4);
            StandartPolynomials[11].Add(1);
            StandartPolynomials[11].Add("General");
            //CRC-12
            StandartPolynomials[12].Add(12);
            StandartPolynomials[12].Add(11);
            StandartPolynomials[12].Add(3);
            StandartPolynomials[12].Add(2);
            StandartPolynomials[12].Add(1);
            StandartPolynomials[12].Add("Telecom systems");
            //CRC-15-CAN
            StandartPolynomials[13].Add(15);
            StandartPolynomials[13].Add(14);
            StandartPolynomials[13].Add(10);
            StandartPolynomials[13].Add(8);
            StandartPolynomials[13].Add(7);
            StandartPolynomials[13].Add(4);
            StandartPolynomials[13].Add(3);
            StandartPolynomials[13].Add("CAN");
            //CRC-16-CCITT
            StandartPolynomials[14].Add(16);
            StandartPolynomials[14].Add(12);
            StandartPolynomials[14].Add(5);
            StandartPolynomials[14].Add("XMODEM,X.25, V.41, Bluetooth, PPP, IrDA, CRC-CCITT");
            //CRC-16
            StandartPolynomials[15].Add(16);
            StandartPolynomials[15].Add(15);
            StandartPolynomials[15].Add(2);
            StandartPolynomials[15].Add("USB");
            //CRC-24-Radix64
            StandartPolynomials[16].Add(24);
            StandartPolynomials[16].Add(23);
            StandartPolynomials[16].Add(18);
            StandartPolynomials[16].Add(17);
            StandartPolynomials[16].Add(14);
            StandartPolynomials[16].Add(11);
            StandartPolynomials[16].Add(10);
            StandartPolynomials[16].Add(7);
            StandartPolynomials[16].Add(6);
            StandartPolynomials[16].Add(5);
            StandartPolynomials[16].Add(4);
            StandartPolynomials[16].Add(3);
            StandartPolynomials[16].Add(1);
            StandartPolynomials[16].Add("General");
            //CRC-32-IEEE802.3
            StandartPolynomials[17].Add(32);
            StandartPolynomials[17].Add(26);
            StandartPolynomials[17].Add(23);
            StandartPolynomials[17].Add(22);
            StandartPolynomials[17].Add(16);
            StandartPolynomials[17].Add(12);
            StandartPolynomials[17].Add(11);
            StandartPolynomials[17].Add(10);
            StandartPolynomials[17].Add(8);
            StandartPolynomials[17].Add(7);
            StandartPolynomials[17].Add(5);
            StandartPolynomials[17].Add(4);
            StandartPolynomials[17].Add(2);
            StandartPolynomials[17].Add(1);
            StandartPolynomials[17].Add("Ethernet, MPEG2");
            //CRC-32C
            StandartPolynomials[18].Add(32);
            StandartPolynomials[18].Add(28);
            StandartPolynomials[18].Add(27);
            StandartPolynomials[18].Add(26);
            StandartPolynomials[18].Add(25);
            StandartPolynomials[18].Add(23);
            StandartPolynomials[18].Add(22);
            StandartPolynomials[18].Add(20);
            StandartPolynomials[18].Add(19);
            StandartPolynomials[18].Add(18);
            StandartPolynomials[18].Add(14);
            StandartPolynomials[18].Add(13);
            StandartPolynomials[18].Add(11);
            StandartPolynomials[18].Add(10);
            StandartPolynomials[18].Add(9);
            StandartPolynomials[18].Add(8);
            StandartPolynomials[18].Add(6);
            StandartPolynomials[18].Add("General");
            //CRC-32K                  
            StandartPolynomials[19].Add(32);
            StandartPolynomials[19].Add(30);
            StandartPolynomials[19].Add(29);
            StandartPolynomials[19].Add(28);
            StandartPolynomials[19].Add(26);
            StandartPolynomials[19].Add(20);
            StandartPolynomials[19].Add(19);
            StandartPolynomials[19].Add(17);
            StandartPolynomials[19].Add(16);
            StandartPolynomials[19].Add(15);
            StandartPolynomials[19].Add(11);
            StandartPolynomials[19].Add(10);
            StandartPolynomials[19].Add(7);
            StandartPolynomials[19].Add(6);
            StandartPolynomials[19].Add(4);
            StandartPolynomials[19].Add(2);
            StandartPolynomials[19].Add(1);
            StandartPolynomials[19].Add("General");
            //CRC-64-ISO
            StandartPolynomials[20].Add(64);
            StandartPolynomials[20].Add(4);
            StandartPolynomials[20].Add(3);
            StandartPolynomials[20].Add(1);
            StandartPolynomials[20].Add("ISO 3309");
            //CRC-64-ECMA
            StandartPolynomials[21].Add(64);
            StandartPolynomials[21].Add(62);
            StandartPolynomials[21].Add(57);
            StandartPolynomials[21].Add(55);
            StandartPolynomials[21].Add(54);
            StandartPolynomials[21].Add(53);
            StandartPolynomials[21].Add(52);
            StandartPolynomials[21].Add(47);
            StandartPolynomials[21].Add(46);
            StandartPolynomials[21].Add(45);
            StandartPolynomials[21].Add(40);
            StandartPolynomials[21].Add(39);
            StandartPolynomials[21].Add(38);
            StandartPolynomials[21].Add(37);
            StandartPolynomials[21].Add(35);
            StandartPolynomials[21].Add(33);
            StandartPolynomials[21].Add(32);
            StandartPolynomials[21].Add(31);
            StandartPolynomials[21].Add(29);
            StandartPolynomials[21].Add(27);
            StandartPolynomials[21].Add(24);
            StandartPolynomials[21].Add(23);
            StandartPolynomials[21].Add(22);
            StandartPolynomials[21].Add(21);
            StandartPolynomials[21].Add(19);
            StandartPolynomials[21].Add(17);
            StandartPolynomials[21].Add(13);
            StandartPolynomials[21].Add(12);
            StandartPolynomials[21].Add(10);
            StandartPolynomials[21].Add(9);
            StandartPolynomials[21].Add(7);
            StandartPolynomials[21].Add(4);
            StandartPolynomials[21].Add(1);
            StandartPolynomials[21].Add("ECMA-182");
        }

        UInt64 CalcPolynomial()
        {
            UInt64 result = 0;

            for (int i = N; i > 0; i--)
            {
                if (checkedCells[i-1])
                {
                    result += Convert.ToUInt64(Math.Pow(2, i-1));
                }
            }

            if (Parameters.m_PolyValue != result)
            {
                Parameters.m_PolyValue = result;
                Parameters.SetParam("PolyValue");
                Parameters.CommitParams();
            }

            string PolyTextHexValue = result.ToString("x");
            for (int i = PolyTextHexValue.Length; i < (N-1)/4+1; i++)
            {
                PolyTextHexValue = "0" + PolyTextHexValue;
            }
            textBoxResult.Text = PolyTextHexValue.ToUpper();
            return result;
        }

        private void DrawPolynomBmp()
        {
            Graphics g = Graphics.FromImage(PolynomBmp);
            int space = PolynomBmp.Width / 16;/*28*/
            int spaceVert = 22;
            Point initPos = new Point(5, 3);
            Font xFont = new Font("Arial", 10, FontStyle.Bold);
            Font degreeFont = new Font("Arial", 7);
            StringBuilder str = new StringBuilder();
            int realCount = 0;

            Color bmpBackColor = Color.Gray;

            for (int i = 0; i < N; i++)
            {
                if (checkedCells[i])
                {
                    str.Append("X + ");
                    
                }
            }
            str.Append("1");

            g.Clear(Color.White);

            //g.DrawString(str.ToString(), xFont, new SolidBrush(Color.Black), initPos.X, initPos.Y);

            SizeF xSize = g.MeasureString("X", xFont);

            for (int i = N-1; i >= 0; i--)
            {
                if (checkedCells[i])
                {
                    g.DrawString("X", xFont, new SolidBrush(Color.Black), space * (realCount%16), initPos.Y + spaceVert*(realCount/16));
                    g.DrawString("+", xFont, new SolidBrush(Color.Black), space * (realCount%16) + space * 2 / 3, initPos.Y + spaceVert * (realCount / 16));
                    g.DrawString((i + 1).ToString(), degreeFont, new SolidBrush(Color.Black), space * (realCount%16) + (int)xSize.Width * 3 / 4, spaceVert * (realCount / 16));
                    realCount++;
                }
            }
            g.DrawString("1", xFont, new SolidBrush(Color.Black), space * (realCount % 16), initPos.Y + spaceVert * (realCount / 16));
        }

        void InitBmp()
        {
            pictureBoxPolynom.Image = null;
            PolynomBmp = new Bitmap(pictureBoxPolynom.Width, 120); /*450*/
            Graphics g = Graphics.FromImage(PolynomBmp);
            g.Clear(Color.Gray);
            pictureBoxPolynom.Image = PolynomBmp;
        }

        #region Events

        private void comboBoxStandart_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (comboBoxStandart.SelectedIndex > 0)
            {
                int length = StandartPolynomials[comboBoxStandart.SelectedIndex - 1].Count - 1;
                textBoxN.Text = StandartPolynomials[comboBoxStandart.SelectedIndex - 1][0].ToString();
                toolTipDescription.SetToolTip(comboBoxStandart, "Use: " + (string)(StandartPolynomials[comboBoxStandart.SelectedIndex - 1][length]));
                N = (int)StandartPolynomials[comboBoxStandart.SelectedIndex - 1][0];

                int deg = 0;
                for (int i = 0; i < length; i++)
                {
                    deg = (int)(StandartPolynomials[comboBoxStandart.SelectedIndex - 1][i]);
                    CellClick((N - deg) / dataGridCheck.ColumnCount, (N - deg) % dataGridCheck.ColumnCount);
                }
            }
            else
            {
                toolTipDescription.SetToolTip(comboBoxStandart, "");
            }
            Parameters.m_PolyName = comboBoxStandart.Text;
            Parameters.SetParam("PolyName");
            Parameters.CommitParams();
        }

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
                textBoxResult_Validated(sender, new EventArgs());
                e.Handled = true;
            }
        }

        private void textBoxN_KeyPress(object sender, KeyPressEventArgs e)
        {
            comboBoxStandart.SelectedIndex = 0;
        }

        private void textBoxN_KeyUp(object sender, KeyEventArgs e)
        {
            UInt16 res;
            if (UInt16.TryParse(textBoxN.Text, out res))
            {
                N = res;
                if ((res > 0) && (res <= 64))
                {
                    CellClick(0, 0);
                }
                
            }
            else if (textBoxN.Text == "")
            {
                N = 0;
            }
        }

        private void textBoxN_TextChanged(object sender, EventArgs e)
        {
            // ErrorProvider
            UInt16 res;
            string errorNrange = "N should be in range [1, 64]";
            if (UInt16.TryParse(textBoxN.Text, out res))
            {
                if ((res > 0) && (res <= 64))
                {
                    errorProvider.SetError(textBoxN, "");
                }
                else
                {
                    errorProvider.SetError(textBoxN, errorNrange);
                }
            }
            else if (textBoxN.Text == "")
            {
                errorProvider.SetError(textBoxN, "");
            }
            else
            {
                errorProvider.SetError(textBoxN, errorNrange);
            }
        }

#endregion

        private void dataGridCheck_CellClick(object sender, DataGridViewCellEventArgs e)
        {
            if (e.RowIndex * dataGridCheck.ColumnCount + e.ColumnIndex >= N)
            {
                return;
            }
            comboBoxStandart.SelectedIndex = 0;
            CellClick(e.RowIndex, e.ColumnIndex);
        }

        private void CellClick(int RowIndex, int ColumnIndex)
        {
            if (RowIndex * dataGridCheck.ColumnCount + ColumnIndex >= N)
            {
                return;
            }

            if (!checkedCells[N - (RowIndex * dataGridCheck.ColumnCount + ColumnIndex) - 1])
            {
                dataGridCheck[ColumnIndex, RowIndex].Style = checkedCellsStyle;
                checkedCells[N - (RowIndex * dataGridCheck.ColumnCount + ColumnIndex) - 1] = true;
            }
            else
            {
                dataGridCheck[ColumnIndex, RowIndex].Style = dataGridCheck.DefaultCellStyle;
                checkedCells[N - (RowIndex * dataGridCheck.ColumnCount + ColumnIndex) - 1] = false;
            }
            CalcPolynomial();
            DrawPolynomBmp();
            pictureBoxPolynom.Invalidate();
        }

        private void textBoxResult_TextChanged(object sender, EventArgs e)
        {
            FieldModified = true;

            try
            {
                if ((Convert.ToUInt64(textBoxResult.Text, 16) > 0) && (Convert.ToUInt64(textBoxResult.Text, 16) <= Math.Pow(2,64)))
                {
                    errorProvider.SetError(textBoxResult, "");
                }
                else
                {
                    if (Convert.ToUInt64(textBoxResult.Text, 16) > 0)
                        errorProvider.SetError(textBoxResult, "Ponymonial value is too large.");
                    else
                        errorProvider.SetError(textBoxResult, "Ponymonial value should be greater than zero.");
                }
            }
            catch
            {
                errorProvider.SetError(textBoxResult, "Invalid Ponymonial value.");
            }
        }

        private void textBoxResult_Enter(object sender, EventArgs e)
        {
            FieldModified = false;
        }

        private void textBoxResult_Validated(object sender, EventArgs e)
        {
            if (FieldModified)
            {
                FieldModified = false;
                try
                {
                    comboBoxStandart.SelectedIndex = 0;
                    if (textBoxResult.Text != "")
                    {
                        if (Convert.ToUInt64(textBoxResult.Text, 16) == UInt64.MaxValue)
                        {
                            N = 64;
                            for (int i = 0; i < N; i++)
                            {
                                CellClick(i / dataGridCheck.ColumnCount, i % dataGridCheck.ColumnCount);
                            }

                            Parameters.m_PolyValue = UInt64.MaxValue;
                            Parameters.SetParam("PolyValue");
                            Parameters.CommitParams();
                        }
                        else
                        {
                            string s = Convert.ToString(Convert.ToInt64(textBoxResult.Text, 16), 2);

                            if ((s.Length <= 32) && (Convert.ToInt64(textBoxResult.Text, 16) > 0))
                            {
                                N = s.Length;

                                textBoxN.Text = N.ToString();

                                for (int i = 0; i < N; i++)
                                {
                                    if (s[i] == '1')
                                        CellClick(i/dataGridCheck.ColumnCount, i%dataGridCheck.ColumnCount);
                                }

                                Parameters.m_PolyValue = Convert.ToUInt64(textBoxResult.Text, 16);
                                Parameters.SetParam("PolyValue");
                                Parameters.CommitParams();
                            }
                            else if (Convert.ToInt64(textBoxResult.Text, 16) == 0)
                            {
                                Parameters.m_PolyValue = Convert.ToUInt64(textBoxResult.Text, 16);
                                Parameters.SetParam("PolyValue");
                                Parameters.CommitParams();
                            }
                                
                        }

                    }
                }
                catch
                {
                    //MessageBox.Show("Something Wrong");
                }
            }
        }

        private void dataGridCheck_CellEnter(object sender, DataGridViewCellEventArgs e)
        {
            dataGridCheck.CurrentCell.Selected = false;
        }

        private void dataGridCheck_CellStateChanged(object sender, DataGridViewCellStateChangedEventArgs e)
        {
            dataGridCheck.CurrentCell.Selected = false;
        }

        #region textBoxSeed
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

        private void textBoxSeed_Validated(object sender, EventArgs e)
        {
            try
            {
                if (textBoxSeed.Text == "")
                {
                    Parameters.m_SeedValue = 0;
                }
                else
                {
                    //if (textBoxSeed.Text.Length <= 16)
                    Parameters.m_SeedValue = Convert.ToUInt64(textBoxSeed.Text, 16);
                }

                Parameters.SetParam("SeedValue");
                Parameters.CommitParams();
            }
            catch
            {
                MessageBox.Show("Invalid format for Seed value.");
            }
        }
        #endregion textBoxSeed

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

    }
}
