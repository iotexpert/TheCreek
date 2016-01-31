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
using System.Data;
using System.Text;
using System.Windows.Forms;
using System.Diagnostics;
using System.Text.RegularExpressions;
using CyDesigner.Extensions.Gde;

namespace LUT_v1_50
{
    public partial class CyBitfieldControl : UserControl
    {
        public const int MAX_INPUTS = 5;
        public const int MAX_OUTPUTS = 8;
        public const int HEX_INPUT_COLUMN = 0;
        public const int INPUT_COLUMNS_START = HEX_INPUT_COLUMN + 1;
        public const int INPUT_COLUMNS_END = MAX_INPUTS + INPUT_COLUMNS_START - 1;
        public const int OUTPUT_COLUMNS_START = INPUT_COLUMNS_END + 2; // need room for divider column.
        public const int OUTPUT_COLUMNS_END = OUTPUT_COLUMNS_START + MAX_OUTPUTS - 1;
        public const int HEX_OUTPUT_COLUMN = OUTPUT_COLUMNS_END + 1;

        public ICyInstEdit_v1 m_Component = null;
        public CyBitfieldControl(ICyInstEdit_v1 inst)
        {
            m_Component = inst;
            //Trace.Assert(Debugger.IsAttached);
            InitializeComponent();
            m_dgvBitFieldConfigure.RowHeadersVisible = false;
            m_dgvBitFieldConfigure.AllowUserToAddRows = false;
            m_dgvBitFieldConfigure.AllowUserToOrderColumns = false;
            m_dgvBitFieldConfigure.AllowUserToResizeColumns = false;
            m_dgvBitFieldConfigure.AllowUserToResizeRows = false;
            SetupColumns(inst);
            m_nudInputs.Minimum = 1;
            m_nudInputs.Maximum = MAX_INPUTS;
            m_nudOutputs.Minimum = 1;
            m_nudOutputs.Maximum = MAX_OUTPUTS;
            UpdateFormFromParams(inst);
        }

        void SetupColumns(ICyInstEdit_v1 inst)
        {
            DataGridViewTextBoxColumn hexColumn = new DataGridViewTextBoxColumn();
            hexColumn.HeaderText = String.Format("Input\nHex\nValue");
            hexColumn.AutoSizeMode = DataGridViewAutoSizeColumnMode.AllCells;
            hexColumn.ReadOnly = true;
            hexColumn.CellTemplate = new DataGridViewTextBoxCell();
            hexColumn.CellTemplate.Style.BackColor = Color.LightGray;
            m_dgvBitFieldConfigure.Columns.Add(hexColumn);

            //Create the Inputs Columns
            for (int i = (MAX_INPUTS - 1); i >= 0; i--)
            {
                DataGridViewTextBoxColumn inputColumn = new DataGridViewTextBoxColumn();
                inputColumn.HeaderText = String.Format("in{0}", i.ToString());
                inputColumn.AutoSizeMode = DataGridViewAutoSizeColumnMode.AllCells;
                inputColumn.ReadOnly = true;
                inputColumn.CellTemplate = new DataGridViewTextBoxCell();
                inputColumn.CellTemplate.Style.BackColor = Color.LightGray;
                m_dgvBitFieldConfigure.Columns.Add(inputColumn);
            }

            //Setup the Blank Divider Column
            DataGridViewTextBoxColumn dividerColumn = new DataGridViewTextBoxColumn();
            dividerColumn.HeaderText = String.Empty;
            dividerColumn.AutoSizeMode = DataGridViewAutoSizeColumnMode.None;
            dividerColumn.Width = 20;
            dividerColumn.ReadOnly = true;
            dividerColumn.CellTemplate = new DataGridViewTextBoxCell();
            dividerColumn.CellTemplate.Style.BackColor = Color.Black;
            m_dgvBitFieldConfigure.Columns.Add(dividerColumn);
            //Create the Outputs Columns
            for (int i = (MAX_OUTPUTS - 1); i >= 0; i--)
            {
                DataGridViewButtonColumn outputColumn = new DataGridViewButtonColumn();
                outputColumn.HeaderText = String.Format("out{0}", i.ToString());
                outputColumn.AutoSizeMode = DataGridViewAutoSizeColumnMode.AllCells;
                outputColumn.FlatStyle = FlatStyle.Flat;
                m_dgvBitFieldConfigure.Columns.Add(outputColumn);
            }

            hexColumn = new DataGridViewTextBoxColumn();
            hexColumn.HeaderText = String.Format("Output\nHex\nValue");
            hexColumn.AutoSizeMode = DataGridViewAutoSizeColumnMode.AllCells;
            hexColumn.ReadOnly = false;
            hexColumn.CellTemplate = new DataGridViewTextBoxCell();
            hexColumn.CellTemplate.Style.BackColor = Color.White;
            hexColumn.Visible = true;

            m_dgvBitFieldConfigure.Columns.Add(hexColumn);

            SetColumnsVisibility(inst);
        }

        private void SetColumnsVisibility(ICyInstEdit_v1 inst)
        {
            LutParameters prms = new LutParameters(inst);
            switch (Convert.ToInt16(prms.NumInputs.Value))
            {
                case 1:
                    m_dgvBitFieldConfigure.Columns[INPUT_COLUMNS_START + 0].Visible = false;
                    m_dgvBitFieldConfigure.Columns[INPUT_COLUMNS_START + 1].Visible = false;
                    m_dgvBitFieldConfigure.Columns[INPUT_COLUMNS_START + 2].Visible = false;
                    m_dgvBitFieldConfigure.Columns[INPUT_COLUMNS_START + 3].Visible = false;
                    m_dgvBitFieldConfigure.Columns[INPUT_COLUMNS_START + 4].Visible = true;
                    break;
                case 2:
                    m_dgvBitFieldConfigure.Columns[INPUT_COLUMNS_START + 0].Visible = false;
                    m_dgvBitFieldConfigure.Columns[INPUT_COLUMNS_START + 1].Visible = false;
                    m_dgvBitFieldConfigure.Columns[INPUT_COLUMNS_START + 2].Visible = false;
                    m_dgvBitFieldConfigure.Columns[INPUT_COLUMNS_START + 3].Visible = true;
                    m_dgvBitFieldConfigure.Columns[INPUT_COLUMNS_START + 4].Visible = true;
                    break;
                case 3:
                    m_dgvBitFieldConfigure.Columns[INPUT_COLUMNS_START + 0].Visible = false;
                    m_dgvBitFieldConfigure.Columns[INPUT_COLUMNS_START + 1].Visible = false;
                    m_dgvBitFieldConfigure.Columns[INPUT_COLUMNS_START + 2].Visible = true;
                    m_dgvBitFieldConfigure.Columns[INPUT_COLUMNS_START + 3].Visible = true;
                    m_dgvBitFieldConfigure.Columns[INPUT_COLUMNS_START + 4].Visible = true;
                    break;
                case 4:
                    m_dgvBitFieldConfigure.Columns[INPUT_COLUMNS_START + 0].Visible = false;
                    m_dgvBitFieldConfigure.Columns[INPUT_COLUMNS_START + 1].Visible = true;
                    m_dgvBitFieldConfigure.Columns[INPUT_COLUMNS_START + 2].Visible = true;
                    m_dgvBitFieldConfigure.Columns[INPUT_COLUMNS_START + 3].Visible = true;
                    m_dgvBitFieldConfigure.Columns[INPUT_COLUMNS_START + 4].Visible = true;
                    break;
                case 5:
                    m_dgvBitFieldConfigure.Columns[INPUT_COLUMNS_START + 0].Visible = true;
                    m_dgvBitFieldConfigure.Columns[INPUT_COLUMNS_START + 1].Visible = true;
                    m_dgvBitFieldConfigure.Columns[INPUT_COLUMNS_START + 2].Visible = true;
                    m_dgvBitFieldConfigure.Columns[INPUT_COLUMNS_START + 3].Visible = true;
                    m_dgvBitFieldConfigure.Columns[INPUT_COLUMNS_START + 4].Visible = true;
                    break;
            }
            SetInputColumnValues(Convert.ToInt16(prms.NumInputs.Value));

            for (int i = OUTPUT_COLUMNS_START; i <= (OUTPUT_COLUMNS_END); i++)
            {
                if (i <= (OUTPUT_COLUMNS_END - Convert.ToInt16(prms.NumOutputs.Value)))
                    m_dgvBitFieldConfigure.Columns[i].Visible = false;
                else
                    m_dgvBitFieldConfigure.Columns[i].Visible = true;
            }
            SetCheckBoxesFromBitField(prms.BitField.Value);

            UpdateOutputHexValues();
        }

        private void SetInputColumnValues(int NumInputs)
        {
            m_dgvBitFieldConfigure.Rows.Clear();
            for (double i = 0; i <= (Math.Pow(2.0, NumInputs) - 1); i++)
            {
                Object[] rowobs = new object[16];
                // rows 0 to 15
                rowobs[HEX_INPUT_COLUMN] = "0x" + ((int)i).ToString("X2");
                rowobs[INPUT_COLUMNS_START + 0] = (((int)i & 0x10) == 0x10) ? "1" : "0";
                rowobs[INPUT_COLUMNS_START + 1] = (((int)i & 0x08) == 0x08) ? "1" : "0";
                rowobs[INPUT_COLUMNS_START + 2] = (((int)i & 0x04) == 0x04) ? "1" : "0";
                rowobs[INPUT_COLUMNS_START + 3] = (((int)i & 0x02) == 0x02) ? "1" : "0";
                rowobs[INPUT_COLUMNS_START + 4] = (((int)i & 0x01) == 0x01) ? "1" : "0";
                rowobs[INPUT_COLUMNS_START + 5] = "";
                rowobs[INPUT_COLUMNS_START + 6] = true;
                rowobs[INPUT_COLUMNS_START + 7] = true;
                rowobs[INPUT_COLUMNS_START + 8] = true;
                rowobs[INPUT_COLUMNS_START + 9] = true;
                rowobs[INPUT_COLUMNS_START + 10] = true;
                rowobs[INPUT_COLUMNS_START + 11] = true;
                rowobs[INPUT_COLUMNS_START + 12] = true;
                rowobs[INPUT_COLUMNS_START + 13] = true;
                rowobs[HEX_OUTPUT_COLUMN] = "Not Defined";
                m_dgvBitFieldConfigure.Rows.Add(rowobs);
            }
        }

        private void UpdateFormFromParams(ICyInstEdit_v1 inst)
        {
            LutParameters prms = new LutParameters(inst);
            int inputs = Convert.ToInt16(prms.NumInputs.Value);
            m_nudInputs.Value = inputs;
            int outputs = Convert.ToInt16(prms.NumOutputs.Value);
            m_nudOutputs.Value = outputs;
            m_chbxRegisterOutputs.Checked = (prms.RegisterOutputs.Value == "true");
        }

        private void m_bSetAll_Click(object sender, EventArgs e)
        {
            SetAllButtonValues();
            UpdateOutputHexValues();
        }

        private void m_bClearAll_Click(object sender, EventArgs e)
        {
            ClearAllButtonValues();
            UpdateOutputHexValues();
        }

        private void SetAllButtonValues()
        {
            //set all the buttons to 1 value
            //When they are 1 value we use different Background Colors and Selection Colors.
            for (int i = m_dgvBitFieldConfigure.RowCount - 1; i >= 0; i--)
            {
                for (int j = OUTPUT_COLUMNS_END; j >= OUTPUT_COLUMNS_START; j--)
                {
                    ((DataGridViewButtonCell)m_dgvBitFieldConfigure.Rows[i].Cells[j]).Value = "1";
                    ((DataGridViewButtonCell)m_dgvBitFieldConfigure.Rows[i].Cells[j]).Style.BackColor = Color.White;
                    ((DataGridViewButtonCell)m_dgvBitFieldConfigure.Rows[i].Cells[j]).Style.SelectionBackColor = Color.White;
                    ((DataGridViewButtonCell)m_dgvBitFieldConfigure.Rows[i].Cells[j]).Style.SelectionForeColor = Color.Black;
                }
            }
        }

        private void ClearAllButtonValues()
        {
            //set all the buttons to 0 value
            for (int i = m_dgvBitFieldConfigure.RowCount - 1; i >= 0; i--)
            {
                for (int j = OUTPUT_COLUMNS_END; j >= OUTPUT_COLUMNS_START; j--)
                {
                    ((DataGridViewButtonCell)m_dgvBitFieldConfigure.Rows[i].Cells[j]).Value = "0";
                    ((DataGridViewButtonCell)m_dgvBitFieldConfigure.Rows[i].Cells[j]).Style.BackColor = Color.LightGray;
                    ((DataGridViewButtonCell)m_dgvBitFieldConfigure.Rows[i].Cells[j]).Style.SelectionBackColor = Color.LightGray;
                    ((DataGridViewButtonCell)m_dgvBitFieldConfigure.Rows[i].Cells[j]).Style.SelectionForeColor = Color.Black;
                }
            }
        }

        private void m_dgvBitFieldConfigure_CellContentClick(object sender, DataGridViewCellEventArgs e)
        {
            if (e.RowIndex != -1)
            {
                if ((e.ColumnIndex <= OUTPUT_COLUMNS_END) && (e.ColumnIndex >= OUTPUT_COLUMNS_START))
                {
                    if ((string)((DataGridViewButtonCell)m_dgvBitFieldConfigure.Rows[e.RowIndex].Cells[e.ColumnIndex]).Value == "1")
                    {
                        ((DataGridViewButtonCell)m_dgvBitFieldConfigure.Rows[e.RowIndex].Cells[e.ColumnIndex]).Value = "0";
                        ((DataGridViewButtonCell)m_dgvBitFieldConfigure.Rows[e.RowIndex].Cells[e.ColumnIndex]).Style.BackColor = Color.LightGray;
                        ((DataGridViewButtonCell)m_dgvBitFieldConfigure.Rows[e.RowIndex].Cells[e.ColumnIndex]).Style.SelectionBackColor = Color.LightGray;
                        ((DataGridViewButtonCell)m_dgvBitFieldConfigure.Rows[e.RowIndex].Cells[e.ColumnIndex]).Style.SelectionForeColor = Color.Black;
                    }
                    else
                    {
                        ((DataGridViewButtonCell)m_dgvBitFieldConfigure.Rows[e.RowIndex].Cells[e.ColumnIndex]).Value = "1";
                        ((DataGridViewButtonCell)m_dgvBitFieldConfigure.Rows[e.RowIndex].Cells[e.ColumnIndex]).Style.BackColor = Color.White;
                        ((DataGridViewButtonCell)m_dgvBitFieldConfigure.Rows[e.RowIndex].Cells[e.ColumnIndex]).Style.SelectionBackColor = Color.White;
                        ((DataGridViewButtonCell)m_dgvBitFieldConfigure.Rows[e.RowIndex].Cells[e.ColumnIndex]).Style.SelectionForeColor = Color.Black;
                    }
                    UpdateOutputHexValue(e.RowIndex);
                }
            }
        }

        /// <summary>
        /// Updates the Output Hex Value for a specified row 
        /// </summary>
        /// <param name="row"> The row to update</param>
        private void UpdateOutputHexValue(int row)
        {
            int[] array = GetVisibleRowValues();
            ((DataGridViewTextBoxCell)m_dgvBitFieldConfigure.Rows[row].Cells[HEX_OUTPUT_COLUMN]).Value = "0x" + array[row].ToString("X2");
        }

        /// <summary>
        /// Update the Hex value for each visible row of the output.
        /// </summary>
        private void UpdateOutputHexValues()
        {
            for (int i = m_dgvBitFieldConfigure.RowCount - 1; i >= 0; i--)
            {
                UpdateOutputHexValue(i);
            }
        }


        private void SetCheckBoxesFromBitField(string bitField)
        {
            string binaryRep = CreateBinaryBitFieldFromString(bitField);
            //set all the check boxes
            for (int i = m_dgvBitFieldConfigure.RowCount - 1; i >= 0; i--)
            {
                for (int j = OUTPUT_COLUMNS_END; j >= OUTPUT_COLUMNS_START; j--)
                {
                    if (m_dgvBitFieldConfigure.Columns[j].Visible)
                    {
                        ((DataGridViewButtonCell)m_dgvBitFieldConfigure.Rows[i].Cells[j]).Value = (binaryRep[binaryRep.Length - 1] == '1') ? "1" : "0";
                        if (binaryRep[binaryRep.Length - 1] == '1')
                            ((DataGridViewButtonCell)m_dgvBitFieldConfigure.Rows[i].Cells[j]).Style.BackColor = Color.White;
                        else
                            ((DataGridViewButtonCell)m_dgvBitFieldConfigure.Rows[i].Cells[j]).Style.BackColor = Color.LightGray;
                        binaryRep = binaryRep.Remove(binaryRep.Length - 1, 1);
                    }
                }
            }
        }

        private void UpdateRowCheckBoxes(int row, int value)
        {
            for (int j = OUTPUT_COLUMNS_END; j >= OUTPUT_COLUMNS_START; j--)
            {
                if (m_dgvBitFieldConfigure.Columns[j].Visible && (value & (1 << OUTPUT_COLUMNS_END - j)) != 0)
                {
                    ((DataGridViewButtonCell)m_dgvBitFieldConfigure.Rows[row].Cells[j]).Style.BackColor = Color.White;
                    ((DataGridViewButtonCell)m_dgvBitFieldConfigure.Rows[row].Cells[j]).Value = "1";
                }
                else if (m_dgvBitFieldConfigure.Columns[j].Visible)
                {
                    ((DataGridViewButtonCell)m_dgvBitFieldConfigure.Rows[row].Cells[j]).Style.BackColor = Color.LightGray;
                    ((DataGridViewButtonCell)m_dgvBitFieldConfigure.Rows[row].Cells[j]).Value = "0";
                }
            }
            // Update the hex value with the visible rows asking as a mask.
            UpdateOutputHexValue(row);
        }

        private string CreateBinaryBitFieldFromString(string bitField)
        {
            //Reverse Parse the bit-FieldText
            string binaryRep = "";
            while (bitField.Length > 0)
            {
                switch (bitField[0])
                {
                    case '0': binaryRep += "0000"; break;
                    case '1': binaryRep += "0001"; break;
                    case '2': binaryRep += "0010"; break;
                    case '3': binaryRep += "0011"; break;
                    case '4': binaryRep += "0100"; break;
                    case '5': binaryRep += "0101"; break;
                    case '6': binaryRep += "0110"; break;
                    case '7': binaryRep += "0111"; break;
                    case '8': binaryRep += "1000"; break;
                    case '9': binaryRep += "1001"; break;
                    case 'A': binaryRep += "1010"; break;
                    case 'B': binaryRep += "1011"; break;
                    case 'C': binaryRep += "1100"; break;
                    case 'D': binaryRep += "1101"; break;
                    case 'E': binaryRep += "1110"; break;
                    case 'F': binaryRep += "1111"; break;
                }
                bitField = bitField.Substring(1);
            }
            return binaryRep;
        }

        private void CyBitfieldControl_VisibleChanged(object sender, EventArgs e)
        {
            if (!this.Visible)
            {
                m_Component.SetParamExpr("NumInputs", m_nudInputs.Value.ToString());
                m_Component.SetParamExpr("NumOutputs", m_nudOutputs.Value.ToString());
                m_Component.CommitParamExprs();
            }
            else
            {

                m_nudInputs.Value = Convert.ToInt16(m_Component.GetCommittedParam("NumInputs").Value);
                m_nudOutputs.Value = Convert.ToInt16(m_Component.GetCommittedParam("NumOutputs").Value);
                UpdateFormFromParams(m_Component);
            }
        }

        private void CyBitfieldControl_Leave(object sender, EventArgs e)
        {
            m_Component.SetParamExpr("NumInputs", m_nudInputs.Value.ToString());
            m_Component.SetParamExpr("NumOutputs", m_nudOutputs.Value.ToString());
            m_Component.SetParamExpr("RegisterOutputs", m_chbxRegisterOutputs.Checked ? "true" : "false");
            string temp = GetExistingTableData();
            m_Component.SetParamExpr("BitField", ConvertBitFieldToHex(temp));
            m_Component.CommitParamExprs();
            SetColumnsVisibility(m_Component);
        }

        private string NewBitFieldNumInputsUp()
        {
            //if the user selected more inputs then duplicate the rows
            //onto the end of the table
            string str = GetExistingTableData();
            str = str + str;
            str = ConvertBitFieldToHex(str);
            return str;
        }
        private string NewBitFieldNumInputsDown()
        {
            //if the user selected less inputs then cut off 
            //the bottom half of the rows of the table
            string str = GetExistingTableData();
            str = str.Substring(str.Length / 2);
            str = ConvertBitFieldToHex(str);
            return str;
        }
        private string NewBitFieldNumOutputsUp()
        {
            //if the user selected more outputs then stuff bits in the bit-Field to the left
            string str = GetExistingTableData();
            for (int i = Convert.ToInt16(Math.Pow(2.0, Convert.ToDouble(m_nudInputs.Value))) * (Convert.ToInt16(m_nudOutputs.Value) - 1);
                i > 0; i -= (Convert.ToInt16(m_nudOutputs.Value) - 1))
            {
                str = str.Insert(i - Convert.ToInt16(m_nudOutputs.Value - 1), "0");
            }
            str = ConvertBitFieldToHex(str);
            return str;
        }
        private string NewBitFieldNumOutputsDown()
        {
            //if the user selected less outputs then remove columns of data from the left
            string str = GetExistingTableData();
            for (int i = Convert.ToInt16(Math.Pow(2.0, Convert.ToDouble(m_nudInputs.Value))) * (Convert.ToInt16(m_nudOutputs.Value) + 1);
                i > 0; i -= (Convert.ToInt16(m_nudOutputs.Value) + 1))
            {
                str = str.Remove(i - (Convert.ToInt16(m_nudOutputs.Value) + 1), 1);
            }
            str = ConvertBitFieldToHex(str);
            return str;
        }

        private string GetExistingTableData()
        {
            string value = "";
            for (int j = 0; j < m_dgvBitFieldConfigure.Rows.Count; j++)
            {
                for (int k = OUTPUT_COLUMNS_START; k <= OUTPUT_COLUMNS_END; k++)
                {
                    if (m_dgvBitFieldConfigure.Rows[j].Cells[k].Visible)
                    {
                        if (m_dgvBitFieldConfigure.Rows[j].Cells[k].Value.ToString() == "1")
                            value += "1";
                        else
                            value += "0";
                    }
                }
            }
            return value;
        }


        /// <summary>
        /// Return the value of each row as an byte.  Only count visible rows towards the value of a row. 
        /// </summary>
        /// <returns> Array containing value of each row. Hidden rows have a value of zero. </returns>
        private int[] GetVisibleRowValues()
        {
            int[] values = new int[32];

            for (int j = 0; j < m_dgvBitFieldConfigure.Rows.Count; j++)
            {
                int value = 0;

                for (int k = OUTPUT_COLUMNS_START; k <= OUTPUT_COLUMNS_END; k++)
                {
                    if (m_dgvBitFieldConfigure.Rows[j].Cells[k].Visible)
                    {
                        if (m_dgvBitFieldConfigure.Rows[j].Cells[k].Value.ToString() == "1")
                            value += 1 << (OUTPUT_COLUMNS_END - k);
                    }
                }
                values[j] = value;
            }
            return values;
        }

        private string ConvertBitFieldToHex(string str)
        {
            string svalue = "";

            while (str.Length > 0)
            {
                if (str.Length > 2)
                {
                    // Add newest value to front of string for encoding in hex
                    switch (str.Substring(str.Length - 4, 4).PadLeft(4, '0'))
                    {
                        case "0000": svalue = "0" + svalue; break;
                        case "0001": svalue = "1" + svalue; break;
                        case "0010": svalue = "2" + svalue; break;
                        case "0011": svalue = "3" + svalue; break;
                        case "0100": svalue = "4" + svalue; break;
                        case "0101": svalue = "5" + svalue; break;
                        case "0110": svalue = "6" + svalue; break;
                        case "0111": svalue = "7" + svalue; break;
                        case "1000": svalue = "8" + svalue; break;
                        case "1001": svalue = "9" + svalue; break;
                        case "1010": svalue = "A" + svalue; break;
                        case "1011": svalue = "B" + svalue; break;
                        case "1100": svalue = "C" + svalue; break;
                        case "1101": svalue = "D" + svalue; break;
                        case "1110": svalue = "E" + svalue; break;
                        case "1111": svalue = "F" + svalue; break;
                    }
                    str = str.Remove(str.Length - 4);
                }
                else
                {
                    switch (str.Substring(str.Length - 2, 2).PadLeft(2, '0'))
                    {
                        case "00": svalue = "0" + svalue; break;
                        case "01": svalue = "1" + svalue; break;
                        case "10": svalue = "2" + svalue; break;
                        case "11": svalue = "3" + svalue; break;
                    }
                    str = str.Remove(str.Length - 2);
                }
            }

            return svalue;
        }


        private void m_nudInputs_ValueChanged(object sender, EventArgs e)
        {
            int paraminputs = Convert.ToInt16(m_Component.GetCommittedParam("NumInputs").Value);
            if (paraminputs != m_nudInputs.Value)
            {
                if (paraminputs > m_nudInputs.Value)
                    m_Component.SetParamExpr("BitField", NewBitFieldNumInputsDown());
                else
                    m_Component.SetParamExpr("BitField", NewBitFieldNumInputsUp());
                m_Component.SetParamExpr("NumInputs", m_nudInputs.Value.ToString());
                m_Component.CommitParamExprs();
            }
            SetColumnsVisibility(m_Component);
        }

        private void m_nudOutputs_ValueChanged(object sender, EventArgs e)
        {
            int paramoutputs = Convert.ToInt16(m_Component.GetCommittedParam("NumOutputs").Value);
            if (paramoutputs != m_nudOutputs.Value)
            {
                if (paramoutputs > m_nudOutputs.Value)
                    m_Component.SetParamExpr("BitField", NewBitFieldNumOutputsDown());
                else
                    m_Component.SetParamExpr("BitField", NewBitFieldNumOutputsUp());
                m_Component.SetParamExpr("NumOutputs", m_nudOutputs.Value.ToString());
                m_Component.CommitParamExprs();
            }
            SetColumnsVisibility(m_Component);
        }

        private void m_chbxRegisterOutputs_CheckedChanged(object sender, EventArgs e)
        {
            m_Component.SetParamExpr("RegisterOutputs", m_chbxRegisterOutputs.Checked ? "true" : "false");
            m_Component.CommitParamExprs();
        }

        private void m_dgvBitFieldConfigure_CellEndEdit(object sender, EventArgs e)
        {
            bool successful = true;
            DataGridViewCell currentCell = ((DataGridView)sender).CurrentCell;

            if (currentCell is DataGridViewTextBoxCell)
            {
                DataGridViewTextBoxCell cell = (DataGridViewTextBoxCell)currentCell;
                string temp = cell.Value.ToString();
                if (temp.StartsWith("0x"))
                    temp = temp.Substring(2);

                Regex reg;
                reg = new Regex(@"[0-9a-fA-FxX]");

                // If the length of the cell value is not less than 2 
                // AND has non-hex characters in it.  Remove the change and recalculate 
                // based on the bits that are set with the DataGridViewButtonCells.
                if (temp.Length <= 2 && temp.Length > 0)
                {
                    for (int i = 0; i < temp.Length; i++)
                    {
                        if (!reg.Match(temp[i].ToString()).Success)
                        {
                            successful = false;
                        }
                    }
                }
                else
                {
                    successful = false;
                }

                // If invalid value, reset. 
                if (!successful)
                    UpdateOutputHexValue(cell.RowIndex);
                else
                {
                    UpdateRowCheckBoxes(cell.RowIndex, Convert.ToInt32(temp, 16));
                }
            }
        }
    }
}
