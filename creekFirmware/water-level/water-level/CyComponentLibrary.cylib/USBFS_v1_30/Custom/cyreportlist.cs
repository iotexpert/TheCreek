/*******************************************************************************
* Copyright 2008-2009, Cypress Semiconductor Corporation.  All rights reserved.
* You may use this file only in accordance with the license, terms, conditions, 
* disclaimers, and limitations in the end user license agreement accompanying 
* the software package with which this file was provided.
********************************************************************************/



using System;
using System.Collections.Generic;
using System.Drawing;
using System.Windows.Forms;

namespace USBFS_v1_30
{
    public partial class CyReportList : UserControl
    {
        public HIDReportItem Item;
        private bool EditMode;
        private bool m_internalChanges;
        public event EventHandler UpdatedItemEvent;

        public CyReportList(HIDReportItem item, bool edit)
        {
            InitializeComponent();
            Item = item;
            InitList();

            m_internalChanges = true;
            if (edit)
            {
                EditMode = true;
                InitValues();
            }
            m_internalChanges = false;
        }

        void InitList()
        {
            foreach (KeyValuePair<ushort, string> kvp in Item.ValuesList)
            {
                if (kvp.Value.Contains("Vendor") /*|| kvp.Value.Contains("Other")*/)
                {
                    DataGridViewCellStyle vendorCellStyle = new DataGridViewCellStyle();
                    vendorCellStyle.BackColor = Color.FromArgb(255,255, 130);
                    int k = dataGridViewValues.Rows.Add(kvp.Value, "0x" + kvp.Key.ToString("X4"));
                    dataGridViewValues["HexValue", k].Style = vendorCellStyle;
                    dataGridViewValues["ItemName", k].Style = vendorCellStyle;
                    dataGridViewValues["HexValue", k].ReadOnly = false;
                }
                else
                    dataGridViewValues.Rows.Add(kvp.Value, "0x" + kvp.Key.ToString("X2"));
            }
            if (dataGridViewValues.Rows.Count > 0)
                dataGridViewValues.Rows[0].Selected = true;
        }

        void InitValues()
        {
            if (Item.Value.Count > 1)
            {
                for (int i = 0; i < dataGridViewValues.Rows.Count; i++)
                {
                    // If Value is larger than 0xFF
                    if (Item.Value.Count == 3)
                    {
                        UInt16 itemVal = (UInt16)((Item.Value[2] << 8) | Item.Value[1]);

                        if (dataGridViewValues["ItemName", i].Value.ToString().Contains("Vendor"))
                        {
                            dataGridViewValues.CurrentCell = dataGridViewValues[0, i];
                            dataGridViewValues["HexValue", i].Value = "0x" + itemVal.ToString("X4");
                            break;
                        }
                        if (Convert.ToUInt16(
                        dataGridViewValues["HexValue", i].Value.ToString().Replace("0x", ""), 16) == itemVal)
                        {
                            dataGridViewValues.CurrentCell = dataGridViewValues[0, i];
                            break;
                        }
                    }
                    else if (Convert.ToByte(
                        dataGridViewValues["HexValue", i].Value.ToString().Replace("0x", ""), 16) == 
                        Item.Value[1])
                    {
                        dataGridViewValues.CurrentCell = dataGridViewValues[0, i];  
                        break;
                    }
                }
            }
        }

        public bool Apply()
        {
            bool res = true;
            try
            {
                byte size = 0;
                if (dataGridViewValues.SelectedRows.Count > 0)
                {
                    UInt16 hexVal = Convert.ToUInt16(
                        dataGridViewValues.SelectedRows[0].Cells["HexValue"].Value.ToString().Replace("0x", ""), 16);

                    Item.Value.Clear();
                    Item.Value.Add(Item.Prefix);
                    if (hexVal <= 0xFF)
                        Item.Value.Add((byte) hexVal);
                    else
                    {
                        // High byte
                        Item.Value.Add((byte) (hexVal & 0xFF));
                        // Low byte
                        Item.Value.Add((byte) ((hexVal >> 8) & 0xFF));
                    }
                    size = (byte) (Math.Log(hexVal, 2)/8 + 1);
                    Item.Description = "(" + dataGridViewValues.SelectedRows[0].Cells["ItemName"].Value + ")";
                    if (size == 0) size = 1;
                    Item.Size = size;
                    Item.Value[0] |= size;
                }
            }
            catch (FormatException)
            {
                MessageBox.Show("Incorrect format of Input Value", "Warning", MessageBoxButtons.OK,
                                MessageBoxIcon.Warning);
                res = false;
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.ToString(), "Warning", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                res = false;
            }

            return res;
        }

        private void UpdateChange()
        {
            if (EditMode && (!m_internalChanges))
            {
                if (Apply())
                {
                    if (UpdatedItemEvent != null)
                        UpdatedItemEvent(this, new EventArgs());
                }
            }
        }

        private void dataGridViewValues_SelectionChanged(object sender, EventArgs e)
        {
            UpdateChange();
        }

        private void dataGridViewValues_CellValueChanged(object sender, DataGridViewCellEventArgs e)
        {
            UpdateChange();
        }
    }
}
