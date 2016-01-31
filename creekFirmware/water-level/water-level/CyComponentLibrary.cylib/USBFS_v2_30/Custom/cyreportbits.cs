/*******************************************************************************
* Copyright 2008-2012, Cypress Semiconductor Corporation.  All rights reserved.
* You may use this file only in accordance with the license, terms, conditions, 
* disclaimers, and limitations in the end user license agreement accompanying 
* the software package with which this file was provided.
********************************************************************************/



using System;
using System.Collections.Generic;
using System.Drawing;
using System.Text;
using System.Windows.Forms;

namespace USBFS_v2_30
{
    public partial class CyReportBits : CyReportBase
    {
        private List<Label> m_labels;
        private List<GroupBox> m_groupBoxes;
        private List<RadioButton> m_radioButtons;
        private int m_bitsNum = 9;
        public ushort m_data;

        public CyReportBits(CyHidReportItem item, bool edit)
            : base(item, edit)
        {
            InitializeComponent();
            Init();
        }

        protected override void InitValues()
        {
            int value = m_item.m_value[1];
            if (m_item.m_value.Count > 2)
                value += m_item.m_value[2] << 8;

            for (int i = 0; i < m_bitsNum; i++)
            {
                if ((value & (1 << i)) > 0)
                {
                    m_radioButtons[i * 2 + 1].Checked = true;
                }
                else
                {
                    m_radioButtons[i * 2].Checked = true;
                }
            }
        }

        protected override void InitControls()
        {
            const int LEFT_SHIFT = 6;
            const int TOP_SHIFT = 10;
            const int FULL_HEIGHT = 26;
            const int LABEL_WIDTH = 34;

            m_labels = new List<Label>();
            m_groupBoxes = new List<GroupBox>();
            m_radioButtons = new List<RadioButton>();

            m_bitsNum = m_item.m_valuesList.Count / 2;
            for (int i = 0; i < m_bitsNum; i++)
            {
                Label lbl = new Label();
                lbl.Text = "Bit " + i;
                lbl.AutoSize = true;
                lbl.Location = new Point(LEFT_SHIFT, TOP_SHIFT + FULL_HEIGHT * i);
                this.Controls.Add(lbl);
                m_labels.Add(lbl);

                GroupBox groupBox = new GroupBox();
                groupBox.Padding = Padding.Empty;
                groupBox.Location = new Point(LEFT_SHIFT + LABEL_WIDTH, TOP_SHIFT - 12 + FULL_HEIGHT * i);
                groupBox.Size = new Size(this.Width - groupBox.Left - 5, 30);
                groupBox.Anchor = AnchorStyles.Left | AnchorStyles.Top | AnchorStyles.Right;
                groupBox.Resize += new EventHandler(groupBox_Resize);
                this.Controls.Add(groupBox);
                m_groupBoxes.Add(groupBox);

                RadioButton radio1 = new RadioButton();
                radio1.Text = m_item.m_valuesList[(byte)(i * 2)];
                radio1.Location = new Point(6, 10);
                radio1.AutoSize = true;
                radio1.Checked = true;
                radio1.CheckedChanged += new EventHandler(radio_Validated);

                if (radio1.Text == "")
                    radio1.Enabled = false;
                groupBox.Controls.Add(radio1);
                m_radioButtons.Add(radio1);

                RadioButton radio2 = new RadioButton();
                radio2.Text = m_item.m_valuesList[(byte)(i * 2 + 1)];
                radio2.Location = new Point(groupBox.Width / 2, 10);
                radio2.AutoSize = true;
                if (radio2.Text == "")
                    radio2.Enabled = false;
                groupBox.Controls.Add(radio2);
                m_radioButtons.Add(radio2);
                radio2.BringToFront();
            }
        }

        public override bool Apply()
        {
            StringBuilder description = new StringBuilder("(");
            byte result = 0;
            m_item.m_value.Clear();
            m_item.m_value.Add(m_item.m_prefix);

            //Add value
            byte size = 0;
            for (int i = 0; i < m_bitsNum; i++)
            {
                if (m_radioButtons[i * 2 + 1].Checked)
                {
                    result |= (byte)(1 << (i % 8));
                    description.Append(m_item.m_valuesList[(byte) (i*2 + 1)].Remove(3, 
                                                       m_item.m_valuesList[(byte) (i*2 + 1)].Length - 3) + ", ");
                }
                if ((i % 8 == 7) || (i == m_bitsNum - 1))
                {
                    m_item.m_value.Add(result);
                    result = 0;
                    size++;
                }
            }
            if (m_item.m_value[m_item.m_value.Count - 1] == 0)
            {
                m_item.m_value.RemoveAt(m_item.m_value.Count - 1);
                size--;
            }
            m_item.m_value[0] |= size;
            m_item.m_size = size;
            m_item.m_description = description.ToString().TrimEnd(',', ' ') + ")";

            return true;
        }

        void groupBox_Resize(object sender, EventArgs e)
        {
            GroupBox box = (GroupBox) sender;
            box.Controls[0].Left = box.Width/2;
        }

        private void radio_Validated(object sender, EventArgs e)
        {
            if (m_editMode)
            {
                Apply();
                OnChanged();
            }
        }  
    }
}
