/*******************************************************************************
* Copyright 2008-2009, Cypress Semiconductor Corporation.  All rights reserved.
* You may use this file only in accordance with the license, terms, conditions, 
* disclaimers, and limitations in the end user license agreement accompanying 
* the software package with which this file was provided.
********************************************************************************/



using System;
using System.Collections.Generic;
using System.Globalization;
using System.Windows.Forms;

namespace USBFS_v1_20
{
    public partial class CyReportNumber : UserControl
    {
        public HIDReportItem Item;
        private bool EditMode;
        public event EventHandler UpdatedItemEvent;

        public CyReportNumber(HIDReportItem item, bool edit)
        {
            InitializeComponent();
            Item = item;

            if (edit)
            {
                EditMode = true;
                InitValues();
            }
        }

        void InitValues()
        {
            ulong val = 0;
            for (int i = 1; i < Item.Value.Count; i++)
            {
                val += (ulong)(Item.Value[i] << ((i-1)*8));
            }
            textBoxValue.Text = val.ToString();
        }

        public bool Apply()
        {
            bool res = true;

            Item.Value.Clear();
            Item.Value.Add(Item.Prefix);

            long val;
            try
            {
                if (radioButtonDec.Checked)
                {
                    val = Convert.ToInt64(textBoxValue.Text);
                }
                else
                {
                    val = Convert.ToInt64(textBoxValue.Text, 16);
                }

                byte[] bVals = BitConverter.GetBytes(val);

                if (BitConverter.IsLittleEndian)
                    Array.Reverse(bVals);
                List<byte> byteList = new List<byte>(BitConverter.GetBytes(val));
                while ((byteList[byteList.Count - 1] == 0) && (byteList.Count > 1))
                    byteList.RemoveAt(byteList.Count - 1);
                Item.Value.AddRange(byteList.ToArray());

                byte size = (byte)(Math.Log(val, 2) / 8 + 1);
                if (size == 0)
                    size = 1;
                Item.Size = size;
                Item.Value[0] |= size;
                Item.Description = "(" + val + ")";
            }
            catch (Exception)
            {
                MessageBox.Show("Incorrect format of Input Value", "USBFS", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                res = false;
            }
            return res;
        }

        private void textBoxValue_Validated(object sender, EventArgs e)
        {
            if (EditMode)
            {
                Apply();
                UpdatedItemEvent(this, new EventArgs());
            }
        }

        private void radioButtonDec_CheckedChanged(object sender, EventArgs e)
        {
            if (!((RadioButton)sender).Checked)
                return;

            if (sender == radioButtonDec)
            {
                int val;
                if (Int32.TryParse(textBoxValue.Text, NumberStyles.HexNumber, null, out val))
                    textBoxValue.Text = val.ToString();
            }
            else if (sender == radioButtonHex)
            {
                int val;
                if (Int32.TryParse(textBoxValue.Text, out val))
                    textBoxValue.Text = val.ToString("X");
            }
        }
    }
}
