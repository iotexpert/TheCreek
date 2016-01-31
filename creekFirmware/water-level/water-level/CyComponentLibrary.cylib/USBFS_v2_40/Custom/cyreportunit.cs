/*******************************************************************************
* Copyright 2008-2012, Cypress Semiconductor Corporation.  All rights reserved.
* You may use this file only in accordance with the license, terms, conditions, 
* disclaimers, and limitations in the end user license agreement accompanying 
* the software package with which this file was provided.
********************************************************************************/



using System;
using System.Collections.Generic;
using System.Windows.Forms;

namespace USBFS_v2_40
{
    public partial class CyReportUnit : CyReportBase
    {
        private readonly string[] UNIT_NAMES =  
        {   "None", 
            "Length",
            "Mass",
            "Time",
            "Temperature",
            "Current",
            "Luminous intensity",
            "Velocity",
            "Momentum",
            "Acceleration",
            "Force",
            "Energy",
            "Angular position",
            "Angular velocity",
            "Angular Acceleration",
            "Volts",
            "Amps",
            "Hertz",
            "Power",
            "Battery Capacity"
        };

        private readonly int [,] UNITS_TABLE = 
        {
            {1,0,0,0,0,0},//"Length",
            {0,1,0,0,0,0},//"Mass",
            {0,0,1,0,0,0},//"Time",
            {0,0,0,1,0,0},//"Temperature",
            {0,0,0,0,1,0},//"Current",
            {0,0,0,0,0,1},//"Luminous intensity"
            {1,0,-1,0,0,0},//"Velocity",
            {1,1,-1,0,0,0},//"Momentum",
            {1,0,-2,0,0,0},//"Acceleration",
            {1,1,-2,0,0,0},//"Force",
            {2,1,-2,0,0,0},//"Energy",
            {1,0,0,0,0,0},//"Angular position",
            {1,0,-1,0,0,0},//"Angular velocity",
            {1,0,-2,0,0,0},//"Angular Acceleration"
            {2,1,-3,0,-1,0},//"Volts",
            {0,0,0,0,1,0},//"Amps",
            {0,0,-1,0,0,0},//"Hertz",
            {2,1,-3,0,0,0},//"Power",
            {0,0,1,0,1,0},//"Battery Capacity"
        };

        private List<NumericUpDown> m_numupdownList;
        private List<Label> m_unitLabelNamesList;
        private List<Label> m_unitLabelsList;

        public CyReportUnit(CyHidReportItem item, bool edit)
            : base(item, edit)
        {
            InitializeComponent();
            Init();
        }

        protected override void InitControls()
        {
            m_numupdownList = new List<NumericUpDown>();
            m_numupdownList.Add(numUpDown1);
            m_numupdownList.Add(numUpDown2);
            m_numupdownList.Add(numUpDown3);
            m_numupdownList.Add(numUpDown4);
            m_numupdownList.Add(numUpDown5);
            m_numupdownList.Add(numUpDown6);

            m_unitLabelNamesList = new List<Label>();
            m_unitLabelNamesList.Add(labelU1);
            m_unitLabelNamesList.Add(labelU2);
            m_unitLabelNamesList.Add(labelU3);
            m_unitLabelNamesList.Add(labelU4);
            m_unitLabelNamesList.Add(labelU5);
            m_unitLabelNamesList.Add(labelU6);

            m_unitLabelsList = new List<Label>();
            m_unitLabelsList.Add(labelU11);
            m_unitLabelsList.Add(labelU22);
            m_unitLabelsList.Add(labelU33);
            m_unitLabelsList.Add(labelU44);
            m_unitLabelsList.Add(labelU55);
            m_unitLabelsList.Add(labelU66);

            comboBoxQuickUnit.Items.AddRange(UNIT_NAMES);
            comboBoxQuickUnit.SelectedIndex = 0; // change
            comboBoxSystem.SelectedIndex = 0; // change
        }

        protected override void InitValues()
        {
            comboBoxSystem.SelectedIndex = m_item.m_value[1] & 0xF;
            int[] bits = new int[6];

            for (int i = 0; i < 6; i++)
            {
                if (m_item.m_value.Count >= (i+1)/2 + 1 + 1)
                    bits[i] = (m_item.m_value[(i + 1) / 2 + 1] >> (((i + 1) % 2) * 4)) & 0xF;
                else
                {
                    bits[i] = 0;
                }
            }

            for (int i = 0; i < bits.Length; i++)
            {
                if (bits[i] > 7)
                    bits[i] -= 16;
            }

            bool isQuickUnit = true;
            for (int i = 0; i < UNITS_TABLE.Length/6; i++)
            {
                isQuickUnit = true;
                for (int j = 0; j < 6; j++)
                {
                    if (UNITS_TABLE[i,j] != bits[j])
                        isQuickUnit = false;
                }
                if (isQuickUnit)
                {
                    comboBoxQuickUnit.SelectedIndex = i+1;
                    break;
                }
            }
            if (!isQuickUnit)
            {
                for (int i = 0; i < m_numupdownList.Count; i++)
                {
                    m_numupdownList[i].Value = bits[i];
                }
            }
        }

        public override bool Apply()
        {
            bool result = true;
            uint unitRes = 0;
            int[] bits = new int[7];
            bits[0] = comboBoxSystem.SelectedIndex;
            for (int i = 0; i < m_numupdownList.Count; i++)
            {
                bits[i + 1] = (int) m_numupdownList[i].Value;
            }
            for (byte i = 0; i < bits.Length; i++)
            {
                if (bits[i] >= 0)
                    unitRes += (uint)((byte)bits[i] << (i * 4));
                else
                {
                    unitRes += (uint)((byte)(bits[i]+16) << (i * 4));
                }
            }
            try
            {
                List<byte> byteList = new List<byte>(BitConverter.GetBytes(unitRes));
                while ((byteList[byteList.Count - 1] == 0) && (byteList.Count > 1))
                    byteList.RemoveAt(byteList.Count - 1);

                m_item.m_value.Clear();
                m_item.m_value.Add(m_item.m_prefix);
                m_item.m_value.AddRange(byteList.ToArray());

                byte size = (byte)(Math.Log(unitRes, 2) / 8 + 1);
                if (size == 0)
                    size = 1;
                if (size == 4)
                    size = 3;
                m_item.m_size = size;
                m_item.m_value[0] |= size;
                m_item.m_description = string.Format("(0x{0})", unitRes.ToString("X"));
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.ToString(), CyUSBFSParameters.MSG_TITLE_ERROR, MessageBoxButtons.OK,
                                MessageBoxIcon.Error);
                result = false;
            }
            return result;
        }

        private void numericUpDown1_ValueChanged(object sender, EventArgs e)
        {
            if (m_internalChanges) return;

            comboBoxQuickUnit.SelectedIndex = 0;
        }

        private void comboBoxQuickUnit_SelectedIndexChanged(object sender, EventArgs e)
        {
            m_internalChanges = true;
            int index = Array.IndexOf(UNIT_NAMES, comboBoxQuickUnit.Text);
            if (index > 0)
            {
                for (int i = 0; i < m_numupdownList.Count; i++)
                {
                    m_numupdownList[i].Value = UNITS_TABLE[index - 1, i];
                }
            }
            if ((index >= 12) && (index <= 14))
            {
                if (comboBoxSystem.SelectedIndex <= 1)
                    comboBoxSystem.SelectedIndex = 2;
                else if (comboBoxSystem.SelectedIndex == 3)
                    comboBoxSystem.SelectedIndex = 4;
            }
            else if (index > 0)
            {
                if ((comboBoxSystem.SelectedIndex == 2) || (comboBoxSystem.SelectedIndex == 0))
                    comboBoxSystem.SelectedIndex = 1;
                else if (comboBoxSystem.SelectedIndex == 4)
                    comboBoxSystem.SelectedIndex = 3;
            }
            m_internalChanges = false;
        }

        private void comboBoxSystem_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (comboBoxSystem.SelectedIndex == 0)
            {
                labelU33.Text = "";
                labelU44.Text = "";
                labelU55.Text = "";
                labelU66.Text = "";
            }
            else 
            {
                labelU33.Text = "sec";
                labelU44.Text = "K";
                labelU55.Text = "A";
                labelU66.Text = "cd";
            }

            switch (comboBoxSystem.SelectedIndex)
            {
                case 0:
                    labelU1.Text = "Length";
                    labelU11.Text = "";
                    labelU22.Text = "";
                    break;
                case 1:
                    labelU1.Text = "Length";
                    labelU11.Text = "cm";
                    labelU22.Text = "g";
                    break;
                case 2:
                    labelU1.Text = "Angle";
                    labelU11.Text = "rad";
                    labelU22.Text = "g";
                    break;
                case 3:
                    labelU1.Text = "Length";
                    labelU11.Text = "in";
                    labelU22.Text = "slug";
                    break;
                case 4:
                    labelU1.Text = "Angle";
                    labelU11.Text = "deg";
                    labelU22.Text = "slug";
                    break;
            }

            for (int i = 0; i < m_unitLabelNamesList.Count; i++)
            {
                if (!String.IsNullOrEmpty(m_unitLabelsList[i].Text))
                    m_unitLabelsList[i].Text = String.Format("({0})", m_unitLabelsList[i].Text);
                m_unitLabelsList[i].Left = m_unitLabelNamesList[i].Left + m_unitLabelNamesList[i].Width;
                m_unitLabelsList[i].Top = m_unitLabelNamesList[i].Top; 
            }
        }

        private void numUpDown1_Validated(object sender, EventArgs e)
        {
            if (m_editMode)
            {
                if (Apply())
                {
                    OnChanged();
                }
            }
        }
    }
}
