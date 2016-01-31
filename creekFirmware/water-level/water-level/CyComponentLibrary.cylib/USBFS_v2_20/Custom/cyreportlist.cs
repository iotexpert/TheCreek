/*******************************************************************************
* Copyright 2008-2012, Cypress Semiconductor Corporation.  All rights reserved.
* You may use this file only in accordance with the license, terms, conditions, 
* disclaimers, and limitations in the end user license agreement accompanying 
* the software package with which this file was provided.
********************************************************************************/



using System;
using System.Collections.Generic;
using System.Drawing;
using System.Windows.Forms;

namespace USBFS_v2_20
{
    public partial class CyReportList : CyReportBase
    {
        private const string VENDOR_VALUE_TEXT = "Custom";
        private const string COL_HEXVALUE = "HexValue";
        private const string COL_ITEMNAME = "ItemName";
        const int MIN = 0x00, MAX = 0xFFFF; // Min/Max for Vendor defined page

        public CyReportList(CyHidReportItem item, bool edit)
            : base(item, edit)
        {
            m_internalChanges = true;

            InitializeComponent();
            Init();
            
            m_internalChanges = false;
        }

        protected override void InitControls()
        {
            dataGridViewValues.Rows.Clear();
            foreach (KeyValuePair<ushort, string> kvp in m_item.m_valuesList)
            {
                if (kvp.Value == VENDOR_VALUE_TEXT)
                {
                    DataGridViewCellStyle vendorCellStyle = new DataGridViewCellStyle();
                    vendorCellStyle.BackColor = Color.FromArgb(255,255, 130);
                    int k = dataGridViewValues.Rows.Add(kvp.Value, "0x" + kvp.Key.ToString("X4"));
                    dataGridViewValues[COL_HEXVALUE, k].Style = vendorCellStyle;
                    dataGridViewValues[COL_ITEMNAME, k].Style = vendorCellStyle;
                    dataGridViewValues[COL_HEXVALUE, k].ReadOnly = false;
                }
                else
                    dataGridViewValues.Rows.Add(kvp.Value, "0x" + kvp.Key.ToString("X2"));
            }
            if (dataGridViewValues.Rows.Count > 0)
                dataGridViewValues.Rows[0].Selected = true;

            dataGridViewValues.CellValidating +=
                new DataGridViewCellValidatingEventHandler(dataGridViewValues_CellValidating);
        }

        protected override void InitValues()
        {
            if (m_item.m_value.Count > 1)
            {
                for (int i = 0; i < dataGridViewValues.Rows.Count; i++)
                {
                    // If Value is larger than 0xFF or the row is "Custom"
                    if ((m_item.m_value.Count == 3) || 
                        (dataGridViewValues[COL_ITEMNAME, i].Value.ToString() == VENDOR_VALUE_TEXT))
                    {
                        UInt16 itemVal = m_item.m_value[1];
                        if (m_item.m_value.Count == 3)
                            itemVal = (UInt16)((m_item.m_value[2] << 8) | m_item.m_value[1]);

                        if (dataGridViewValues[COL_ITEMNAME, i].Value.ToString() == VENDOR_VALUE_TEXT)
                        {
                            dataGridViewValues.CurrentCell = dataGridViewValues[0, i];
                            string hexFormat = (itemVal > 0xFF) ? "X4" : "X2";
                            dataGridViewValues[COL_HEXVALUE, i].Value = "0x" + itemVal.ToString(hexFormat);
                            break;
                        }
                        if (Convert.ToUInt16(
                        dataGridViewValues[COL_HEXVALUE, i].Value.ToString().Replace("0x", ""), 16) == itemVal)
                        {
                            dataGridViewValues.CurrentCell = dataGridViewValues[0, i];
                            break;
                        }
                    }
                    else if (Convert.ToUInt16(
                        dataGridViewValues[COL_HEXVALUE, i].Value.ToString().Replace("0x", ""), 16) == 
                        m_item.m_value[1])
                    {
                        dataGridViewValues.CurrentCell = dataGridViewValues[0, i];  
                        break;
                    }
                }
            }
        }

        public override bool Apply()
        {
            bool res = true;
            try
            {
                byte size = 0;
                if (dataGridViewValues.SelectedRows.Count > 0)
                {
                    UInt16 hexVal = Convert.ToUInt16(
                        dataGridViewValues.SelectedRows[0].Cells[COL_HEXVALUE].Value.ToString().Replace("0x", ""), 16);

                    m_item.m_value.Clear();
                    m_item.m_value.Add(m_item.m_prefix);
                    if (hexVal <= 0xFF)
                        m_item.m_value.Add((byte) hexVal);
                    else
                    {
                        // High byte
                        m_item.m_value.Add((byte) (hexVal & 0xFF));
                        // Low byte
                        m_item.m_value.Add((byte) ((hexVal >> 8) & 0xFF));
                    }
                    size = (byte)(m_item.m_value.Count - 1);
                    m_item.m_description = string.Format("({0})",
                                                         dataGridViewValues.SelectedRows[0].Cells[COL_ITEMNAME].Value);
                    if (size == 0) size = 1;
                    if (size == 4) size = 3;
                    m_item.m_size = size;
                    m_item.m_value[0] |= size;
                }
            }
            catch (FormatException)
            {
                MessageBox.Show(Properties.Resources.MSG_INCORRECT_VALUE, CyUSBFSParameters.MSG_TITLE_WARNING,
                                MessageBoxButtons.OK, MessageBoxIcon.Warning);
                res = false;
            }
            catch (OverflowException)
            {
                MessageBox.Show(String.Format(Properties.Resources.MSG_INCORRECT_VALUE_RANGE, MIN, MAX), CyUSBFSParameters.MSG_TITLE_WARNING,
                                MessageBoxButtons.OK, MessageBoxIcon.Warning);
                res = false;
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.ToString(), CyUSBFSParameters.MSG_TITLE_WARNING, MessageBoxButtons.OK,
                                MessageBoxIcon.Warning);
                res = false;
            }

            return res;
        }

        private void UpdateChange()
        {
            if (m_editMode && (!m_internalChanges))
            {
                if (Apply())
                {
                    OnChanged();
                }
            }
        }

        void dataGridViewValues_CellValidating(object sender, DataGridViewCellValidatingEventArgs e)
        {
            string dirtyVal = e.FormattedValue.ToString().Replace("0x", "");
            if ((dataGridViewValues[COL_ITEMNAME, e.RowIndex].Value.ToString() == VENDOR_VALUE_TEXT) &&
                e.ColumnIndex == 1)
            {
                // Check if string is hexadecimal integer
                bool res = CyUSBFSParameters.CheckIntValue(dirtyVal, true);
                if (res)
                {
                    // Check if value is between MIN and MAX
                    if (CyUSBFSParameters.CheckIntRange(Convert.ToInt32(dirtyVal, 16), MIN, MAX))
                    {
                        dataGridViewValues[e.ColumnIndex, e.RowIndex].ErrorText = String.Empty;
                    }
                    else
                    {
                        dataGridViewValues[e.ColumnIndex, e.RowIndex].ErrorText =
                            String.Format(Properties.Resources.MSG_INCORRECT_VALUE_RANGE, MIN, MAX);
                    }
                }
                else
                {
                    dataGridViewValues[e.ColumnIndex, e.RowIndex].ErrorText = Properties.Resources.MSG_INCORRECT_VALUE;
                }
            }
        }

        private void dataGridViewValues_SelectionChanged(object sender, EventArgs e)
        {
            UpdateChange();
        }

        private void dataGridViewValues_CellValueChanged(object sender, DataGridViewCellEventArgs e)
        {
            if ((e.RowIndex >= 0) && (e.ColumnIndex >= 0))
                UpdateChange();
        }
    }
}
