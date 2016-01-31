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
    public partial class cntSSCSDProps : cntSSCSProps
    {
        public cntSSCSDProps()
        {
            InitializeComponent();
            cbIDACrangeL.SelectedIndex = 0;
            //cbRefValueL.SelectedIndex = 0;
            cbResolution.SelectedIndex = 0;
            cblScanSpeed.SelectedIndex = 0;
            cbCIS.SelectedIndex = 0;
            foreach (Control item in panelMain.Controls)
            {
                if ((item.GetType() == typeof(ComboBox))||(item.GetType() == typeof(TextBox)))
                {
                    item.Validated += new EventHandler(SendProperties);
                }
            }
            cbCustom.Validated += new EventHandler(SendProperties);
            isGeneration = true;
        }

        public override void ChangeState(CyAmuxBParams cch)
        {
            currectCch = cch;
            if (cch != null)
            {
                //Idac Visible
                bool vis = cch.isIdac;
                isIdac = vis;
                lIdacR.Visible = vis;
                lIdadS.Visible = vis;
                cbIDACrangeL.Visible = vis;
                cbIDACSettingL.Visible = vis;
                isGeneration = false;
                tbPrescPer.Enabled = cch.IsPrescaler();
                lPrescPer.Enabled = cch.IsPrescaler();
            }
        }

        public void SendProperties(object sender, EventArgs e)
        {
            if (!isGeneration)                
                for (int i = 0; i < listLastObj.Count; i++)
                {
                    SetValue(ref listLastObj[i].baseProps.Resolution, cbResolution.SelectedIndex);
                    SetValue(ref listLastObj[i].baseProps.ScanSpeed, cblScanSpeed.SelectedIndex);
                    SetValue(ref listLastObj[i].baseProps.CIS, cbCIS.SelectedIndex);
                    //SetSmProp(ref listLastObj[i].baseProps.RefVal, cbRefValueL.SelectedIndex);
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
                    cbResolution.SelectedIndex = lastObject.baseProps.Resolution;
                    cblScanSpeed.SelectedIndex = lastObject.baseProps.ScanSpeed;
                    cbCIS.SelectedIndex = lastObject.baseProps.CIS;
                    //cbRefValueL.SelectedIndex = lastObject.baseProps.RefVal;
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
