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

namespace USBFS_v1_20
{
    public partial class CyReportBits : UserControl
    {
        public HIDReportItem Item;

        private List<Label> Labels;
        private List<GroupBox> GroupBoxes;
        private List<RadioButton> RadioButtons;
        private int BitsNum = 9;
        private bool EditMode;

        public ushort Data;

        public event EventHandler UpdatedItemEvent;

        public CyReportBits(HIDReportItem item, bool edit)
        {
            InitializeComponent();
            Item = item;
            InitControls();

            if (edit)
            {
                InitValues();
                EditMode = true;
            }
        }

        private void InitControls()
        {
            int leftShift = 6;
            int topShift = 10;
            int fullHeight = 26;
            int labelWidth = 34;

            Labels = new List<Label>();
            GroupBoxes = new List<GroupBox>();
            RadioButtons = new List<RadioButton>();

            BitsNum = Item.ValuesList.Count/2;
            for (int i = 0; i < BitsNum; i++)
            {
                Label lbl = new Label();
                lbl.Text = "Bit " + i;
                lbl.AutoSize = true;
                lbl.Location = new Point(leftShift, topShift + fullHeight*i);
                this.Controls.Add(lbl);
                Labels.Add(lbl);

                GroupBox groupBox = new GroupBox();
                groupBox.Padding = Padding.Empty;
                groupBox.Location = new Point(leftShift + labelWidth, topShift - 12 + fullHeight*i);
                groupBox.Size = new Size(this.Width - groupBox.Left - 5, 30);
                groupBox.Anchor = AnchorStyles.Left | AnchorStyles.Top | AnchorStyles.Right;
                groupBox.Resize += new EventHandler(groupBox_Resize);
                this.Controls.Add(groupBox);
                GroupBoxes.Add(groupBox);

                RadioButton radio1 = new RadioButton();
                radio1.Text = Item.ValuesList[(byte) (i*2)];
                radio1.Location = new Point(6, 10);
                radio1.AutoSize = true;
                radio1.Checked = true;
                radio1.CheckedChanged += new EventHandler(radio_Validated);

                if (radio1.Text == "")
                    radio1.Enabled = false;
                groupBox.Controls.Add(radio1);
                RadioButtons.Add(radio1);

                RadioButton radio2 = new RadioButton();
                radio2.Text = Item.ValuesList[(byte) (i*2 + 1)];
                radio2.Location = new Point(groupBox.Width/2, 10);
                radio2.AutoSize = true;
                //radio2.Validated += new EventHandler(radio_Validated);
                if (radio2.Text == "")
                    radio2.Enabled = false;
                groupBox.Controls.Add(radio2);
                RadioButtons.Add(radio2);
                radio2.BringToFront();
            }
        }

        void groupBox_Resize(object sender, EventArgs e)
        {
            GroupBox box = (GroupBox) sender;
            box.Controls[0].Left = box.Width/2;
        }

        private void radio_Validated(object sender, EventArgs e)
        {
            if (EditMode)
            {
                Apply();
                UpdatedItemEvent(this, new EventArgs());
            }
        }

        private void InitValues()
        {
            int value = Item.Value[1];
            if (Item.Value.Count > 2)
                value += Item.Value[2] << 8;

            for (int i = 0; i < BitsNum; i++)
            {
                if ((value & (1 << i)) > 0)
                {
                    RadioButtons[i*2 + 1].Checked = true;
                }
                else
                {
                    RadioButtons[i*2].Checked = true;
                }
            }
        }

        public void Apply()
        {
            string description = "(";
            byte result = 0;
            Item.Value.Clear();
            Item.Value.Add(Item.Prefix);

            //Add value
            byte size = 0;
            for (int i = 0; i < BitsNum; i++)
            {
                if (RadioButtons[i*2 + 1].Checked)
                {
                    result |= (byte) (1 << (i%8));
                    description +=
                        Item.ValuesList[(byte) (i*2 + 1)].Remove(3, Item.ValuesList[(byte) (i*2 + 1)].Length - 3) + ", ";
                }
                if ((i%8 == 7) || (i == BitsNum - 1))
                {
                    Item.Value.Add(result);
                    result = 0;
                    size++;
                }
            }
            if (Item.Value[Item.Value.Count - 1] == 0)
            {
                Item.Value.RemoveAt(Item.Value.Count - 1);
                size--;
            }
            Item.Value[0] |= size;
            Item.Size = size;
            description = description.TrimEnd(',', ' ') + ")";
            Item.Description = description;
        }
    }
}