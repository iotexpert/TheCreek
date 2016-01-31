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

namespace USBFS_v1_30
{
    public partial class CyReportUnit : UserControl
    {
        private const int UNITS_COUNT = 20;
        private string[] Units =  
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

        private int [,] UnitsTable = 
        {
            {1,0,0,0,0,0},/*"Length",*/
            {0,1,0,0,0,0},/*"Mass",*/
            {0,0,1,0,0,0},/*"Time",*/
            {0,0,0,1,0,0},/*"Temperature",*/
            {0,0,0,0,1,0},/*"Current",*/
            {0,0,0,0,0,1},/*"Luminous intensity"*/
            {1,0,-1,0,0,0},/*"Velocity",*/
            {1,1,-1,0,0,0},/*"Momentum",*/
            {1,0,-2,0,0,0},/*"Acceleration",*/
            {1,1,-2,0,0,0},/*"Force",*/
            {2,1,-2,0,0,0},/*"Energy",*/
            {1,0,0,0,0,0},/*"Angular position",*/
            {1,0,-1,0,0,0},/*"Angular velocity",*/
            {1,0,-2,0,0,0},/*"Angular Acceleration"*/
            {2,1,-3,0,-1,0},/*"Volts",*/
            {0,0,0,0,1,0},/*"Amps",*/
            {0,0,-1,0,0,0},/*"Hertz",*/
            {2,1,-3,0,0,0},/*"Power",*/
            {0,0,1,0,1,0},/*"Battery Capacity"*/
        };

        public HIDReportItem Item;
        private bool InternalChange;
        private bool EditMode;
        public event EventHandler UpdatedItemEvent;

        public CyReportUnit(HIDReportItem item, bool edit)
        {
            InitializeComponent();
            Item = item;
            InitData();

            if (edit)
            {
                EditMode = true;
                InitValues();
            }
        }

        void InitData()
        {
            comboBoxQuickUnit.Items.AddRange(Units);
            comboBoxQuickUnit.SelectedIndex = 0; // change
            comboBoxSystem.SelectedIndex = 0; // change
        }

        void InitValues()
        {
            comboBoxSystem.SelectedIndex = Item.Value[1] & 0xF;
            int[] bits = new int[6];
            //bits[0] = Item.Value[1] & 0xF0;
            //bits[1] = Item.Value[2] & 0xF;
            //bits[2] = Item.Value[2] & 0xF0;
            //bits[3] = Item.Value[3] & 0xF;
            //bits[4] = Item.Value[3] & 0xF0;
            //bits[5] = Item.Value[4] & 0xF;

            for (int i = 0; i < 6; i++)
            {
                if (Item.Value.Count >= (i+1)/2 + 1 + 1)
                    bits[i] = (Item.Value[(i + 1) / 2 + 1] >> (((i + 1) % 2) * 4)) & 0xF;
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
            for (int i = 0; i < UnitsTable.Length/6; i++)
            {
                isQuickUnit = true;
                for (int j = 0; j < 6; j++)
                {
                    if (UnitsTable[i,j] != bits[j])
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
                numUpDown1.Value = bits[0];
                numUpDown2.Value = bits[1];
                numUpDown3.Value = bits[2];
                numUpDown4.Value = bits[3];
                numUpDown5.Value = bits[4];
                numUpDown6.Value = bits[5];
            }
        }

        public bool Apply()
        {
            bool result = true;
            uint unitRes = 0;
            int[] bits = new int[7];
            bits[0] = comboBoxSystem.SelectedIndex;
            bits[1] = (int)numUpDown1.Value;
            bits[2] = (int)numUpDown2.Value;
            bits[3] = (int)numUpDown3.Value;
            bits[4] = (int)numUpDown4.Value;
            bits[5] = (int)numUpDown5.Value;
            bits[6] = (int)numUpDown6.Value;
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

                Item.Value.Clear();
                Item.Value.Add(Item.Prefix);
                Item.Value.AddRange(byteList.ToArray());

                byte size = (byte)(Math.Log(unitRes, 2) / 8 + 1);
                if (size == 0)
                    size = 1;
                Item.Size = size;
                Item.Value[0] |= size;
                Item.Description = "(" + "0x" + unitRes.ToString("X") + ")";
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.ToString(), "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                result = false;
            }
            return result;
        }

        private void numericUpDown1_ValueChanged(object sender, EventArgs e)
        {
            if (InternalChange) return;

            comboBoxQuickUnit.SelectedIndex = 0;
        }

        private void comboBoxQuickUnit_SelectedIndexChanged(object sender, EventArgs e)
        {
            InternalChange = true;
            int index = Array.IndexOf(Units, comboBoxQuickUnit.Text);
            if (index > 0)
            {
                numUpDown1.Value = UnitsTable[index - 1, 0];
                numUpDown1.Value = UnitsTable[index - 1, 0];
                numUpDown2.Value = UnitsTable[index - 1, 1];
                numUpDown3.Value = UnitsTable[index - 1, 2];
                numUpDown4.Value = UnitsTable[index - 1, 3];
                numUpDown5.Value = UnitsTable[index - 1, 4];
                numUpDown6.Value = UnitsTable[index - 1, 5];
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
            InternalChange = false;
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
        }

        private void numUpDown1_Validated(object sender, EventArgs e)
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
