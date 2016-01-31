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
using System.Globalization;
using System.Text;
using System.Windows.Forms;

namespace USBFS_v1_30
{
    public partial class CyReportCustom : UserControl
    {
        public HIDReportItem Item;
        private bool EditMode;
        public event EventHandler UpdatedItemEvent;

        public CyReportCustom(HIDReportItem item, bool edit)
        {
            InitializeComponent();
            Item = item;

            if (edit)
            {
                EditMode = true;
                InitValues();
            }
        }

        private void InitValues()
        {
            List<byte> tmpArr = new List<byte>(Item.Value);
            if (tmpArr.Count > 0)
                tmpArr.RemoveAt(0);
            Int64 val = CyUSBFSParameters.ConvertByteArrayToInt(tmpArr);
            
            textBoxValue.Text = val.ToString();

            numUpDownPrefix.Value = Item.Prefix >> 2;
        }

        private void radioButtonDec_CheckedChanged(object sender, EventArgs e)
        {
            if (!((RadioButton) sender).Checked)
                return;

            if (sender == radioButtonDec)
            {
                numUpDownPrefix.Hexadecimal = false;
                int val;
                if (Int32.TryParse(textBoxValue.Text, NumberStyles.HexNumber, null, out val))
                    textBoxValue.Text = val.ToString();
            }
            else if (sender == radioButtonHex)
            {
                numUpDownPrefix.Hexadecimal = true;
                int val;
                if (Int32.TryParse(textBoxValue.Text, out val))
                    textBoxValue.Text = val.ToString("X");
            }
        }

        public bool Apply()
        {
            bool res = true;
            long val = 0;
            string descr = "";
            try
            {
                if (textBoxValue.Text == "")
                {
                    descr += "00";
                    Item.Value.Clear();
                    Item.Prefix = (byte)(Convert.ToByte(numUpDownPrefix.Value) << 2);
                    Item.Value.Add(Item.Prefix);
                }
                else
                {
                    if (radioButtonDec.Checked)
                    {
                        val = Convert.ToInt64(textBoxValue.Text);
                    }
                    else
                    {
                        val = Convert.ToInt64(textBoxValue.Text, 16);
                    }

                    List<byte> byteList = CyUSBFSParameters.ConvertIntToByteArray(val);

                    Item.Value.Clear();
                    Item.Prefix = (byte)(Convert.ToByte(numUpDownPrefix.Value) << 2);
                    Item.Value.Add(Item.Prefix);
                    Item.Value.AddRange(byteList.ToArray());
                   
                    for (int i = byteList.Count-1; i >= 0; i--)
                    {
                        descr += byteList[i].ToString("X2");
                    }
                }

                byte size = (byte) (Math.Log(val, 2)/8 + 1);
                if (size == 0)
                    size = 1;
                Item.Size = size;
                Item.Value[0] |= size;
                Item.Description = "(0x" + descr + ")";
            }
            catch (Exception)
            {
                MessageBox.Show("Incorrect format of Input Value", "Warning", MessageBoxButtons.OK,
                                MessageBoxIcon.Warning);
                res = false;
            }
            return res;
        }

        private void textBoxValue_Validated(object sender, EventArgs e)
        {
            if (EditMode)
            {
                if (Apply())
                {
                    if (UpdatedItemEvent != null)
                        UpdatedItemEvent(this, new EventArgs());
                }
            }
        }
    }
}