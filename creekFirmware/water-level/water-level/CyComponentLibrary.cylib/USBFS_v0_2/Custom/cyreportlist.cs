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

namespace USBFS_v0_2
{
    public partial class CyReportList : UserControl
    {
        public HIDReportItem Item;
        private bool EditMode;
        public event EventHandler UpdatedItemEvent;

        public CyReportList(HIDReportItem item, bool edit)
        {
            InitializeComponent();
            Item = item;
            InitList();

            if (edit)
            {
                EditMode = true;
                InitValues();
            }
        }

        void InitList()
        {
            foreach (KeyValuePair<byte, string> kvp in Item.ValuesList)
            {
                listBoxValues.Items.Add(kvp);
            }
            if (listBoxValues.Items.Count > 0)
                listBoxValues.SelectedIndex = 0;
        }

        void InitValues()
        {
            if (Item.Value.Count > 1)
            {
                for (int i = 0; i < listBoxValues.Items.Count; i++)
                {
                    if (((KeyValuePair<byte, string>)listBoxValues.Items[i]).Key == Item.Value[1])
                    {
                        listBoxValues.SelectedIndex = i;
                        break;
                    }
                }
            }
        }

        public void Apply()
        {
            try
            {
                Item.Value.Clear();
                Item.Value.Add(Item.Prefix);
                if (listBoxValues.SelectedItem != null)
                    Item.Value.Add(((KeyValuePair<byte, string>)listBoxValues.SelectedItem).Key);
                byte size = (byte)(Math.Log(((KeyValuePair<byte, string>)listBoxValues.SelectedItem).Key, 2) / 8 + 1);
                if (size == 0) size = 1;
                Item.Size = size;
                Item.Value[0] |= size;
                Item.Description = "(" + ((KeyValuePair<byte, string>)listBoxValues.SelectedItem).Value + ")";
            }
            catch (Exception ex)
            {
                
                MessageBox.Show(ex.ToString());
            }
            
        }

        private void listBoxValues_Format(object sender, ListControlConvertEventArgs e)
        {
            e.Value = ((KeyValuePair<byte, string>) e.ListItem).Value.ToString();
        }

        private void listBoxValues_Validated(object sender, EventArgs e)
        {
            if (EditMode)
            {
                Apply();
                UpdatedItemEvent(this, new EventArgs());
            }
        }
    }
}
