/*******************************************************************************
* Copyright 2008-2012, Cypress Semiconductor Corporation.  All rights reserved.
* You may use this file only in accordance with the license, terms, conditions, 
* disclaimers, and limitations in the end user license agreement accompanying 
* the software package with which this file was provided.
********************************************************************************/



using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Globalization;
using System.Windows.Forms;

namespace USBFS_v2_40
{
    public partial class CyReportCustom : CyReportBase
    {
        public CyReportCustom(CyHidReportItem item, bool edit) 
            : base (item, edit)
        {
            InitializeComponent();
            Init();
        }

        protected override void InitValues()
        {
            List<byte> tmpArr = new List<byte>(m_item.m_value);
            if (tmpArr.Count > 0)
                tmpArr.RemoveAt(0);
            Int64 val = CyUSBFSParameters.ConvertByteArrayToInt(tmpArr, 
                                CyUSBFSParameters.IsHidItemUnsigned(m_item.Name));
            
            textBoxValue.Text = val.ToString();

            numUpDownPrefix.Value = m_item.m_prefix >> 2;
        }

        public override bool Apply()
        {
            bool res = true;
            long val = 0;
            string descr = "";
            try
            {
                if (textBoxValue.Text == "")
                {
                    descr += "00";
                    m_item.m_value.Clear();
                    m_item.m_prefix = (byte)(Convert.ToByte(numUpDownPrefix.Value) << 2);
                    m_item.m_value.Add(m_item.m_prefix);
                }
                else
                {
                    val = radioButtonDec.Checked
                              ? Convert.ToInt64(textBoxValue.Text)
                              : Convert.ToInt64(textBoxValue.Text.Replace("0x", ""), 16);

                    List<byte> byteList = CyUSBFSParameters.ConvertIntToByteArray(val, 
                                                CyUSBFSParameters.IsHidItemUnsigned(m_item.Name));

                    m_item.m_value.Clear();
                    m_item.m_prefix = (byte)(Convert.ToByte(numUpDownPrefix.Value) << 2);
                    m_item.m_value.Add(m_item.m_prefix);
                    m_item.m_value.AddRange(byteList.ToArray());

                    for (int i = byteList.Count - 1; i >= 0; i--)
                    {
                        descr += byteList[i].ToString("X2");
                    }
                }

                byte size = (byte)(m_item.m_value.Count - 1);//(byte)(Math.Log(val, 2) / 8 + 1);
                if (size == 0)
                    size = 1;
                if (size == 4)
                    size = 3;
                m_item.m_size = size;
                m_item.m_value[0] |= size;
                m_item.m_description = string.Format("(0x{0})", descr);
            }
            catch (Exception)
            {
                MessageBox.Show(Properties.Resources.MSG_INCORRECT_VALUE, CyUSBFSParameters.MSG_TITLE_WARNING,
                                MessageBoxButtons.OK, MessageBoxIcon.Warning);
                res = false;
            }
            return res;
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
            SetErrProvider();
        }

        private void textBoxValue_Validated(object sender, EventArgs e)
        {
            if (m_editMode)
            {
                bool res = CyUSBFSParameters.CheckIntValue(textBoxValue.Text, radioButtonHex.Checked);
                if (res && Apply())
                {
                    OnChanged();
                }
            }
        }

        private void textBoxValue_TextChanged(object sender, EventArgs e)
        {
            SetErrProvider();
        }

        private void SetErrProvider()
        {
            bool res = CyUSBFSParameters.CheckIntValue(textBoxValue.Text, radioButtonHex.Checked);
            errorProvider.SetError(textBoxValue, res ? "" : Properties.Resources.MSG_INCORRECT_VALUE);
        }
    }
}
