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

namespace  CapSense_v0_5.ContentControls
{
    public partial class cntSSCSAProps : cntSSCSProps
    {
//        bool isIdac = false;
        public cntSSCSAProps()
        {
            InitializeComponent();

            cbIDACrangeL.SelectedIndex = 0;
            //cbIDACSettingL.SelectedIndex = 0;
            cbRefValueL.SelectedIndex = 0;
            cbCIS.SelectedIndex = 0;
            foreach (Control item in panelMain.Controls)
            {
                if ((item.GetType() == typeof(ComboBox)) || (item.GetType() == typeof(TextBox)))
                {
                    item.Validated += new EventHandler(ValidateProps);
                }
            }

            isGeneration = true;
        }
        public override void ChangeState(CyAmuxBParams cch)
        {
            //isGeneration = false;
            tbPrescPer.Enabled = cch.IsPrescaler();
            lPrescPer.Enabled = cch.IsPrescaler();
        }
        public void ValidateProps(object sender, EventArgs e)
        {
            if (!isGeneration)
                for (int i = 0; i < listLastObj.Count; i++)
                {
                    SetValue(ref listLastObj[i].baseProps.Scanlength, cbScanLengthL);
                    SetValue(ref listLastObj[i].baseProps.SettlingTime, tbSettlingTime);
                    SetValue(ref listLastObj[i].baseProps.CIS, cbCIS.SelectedIndex);
                    SetValue(ref listLastObj[i].baseProps.RefVal, cbRefValueL.SelectedIndex);
                    SetValue(ref listLastObj[i].baseProps.PrescPer, tbPrescPer);
                    if (isIdac)
                    {
                        SetValue(ref listLastObj[i].IDACRange, cbIDACrangeL.SelectedIndex);
                        SetValue(ref listLastObj[i].IDACSettings, cbIDACSettingL);
                    }

                    //If Somthith chage it mean we should change Custom for all items
                    listLastObj[i].Custom = cbCustom.Checked;
                }
        }

        public override void ShowInControl()
        {
            //if (!isGeneration)
            if (lastObject != null)
            {
                cbScanLengthL.Text = GetValue(lastObject.baseProps.Scanlength);
                tbSettlingTime.Text = GetValue(lastObject.baseProps.SettlingTime);
                cbCIS.SelectedIndex = lastObject.baseProps.CIS;
                cbRefValueL.SelectedIndex = lastObject.baseProps.RefVal;
                tbPrescPer.Text = GetValue(lastObject.baseProps.PrescPer);
                cbIDACrangeL.SelectedIndex = lastObject.IDACRange;
                cbIDACSettingL.Text = GetValue(lastObject.IDACSettings);

                cbCustom.Checked = lastObject.Custom;
            }
        }

        private void cbCustom_CheckedChanged(object sender, EventArgs e)
        {
            panelMain.Enabled = cbCustom.Checked;
            cbCustomProcessing(cbCustom);
        }
    }
}
