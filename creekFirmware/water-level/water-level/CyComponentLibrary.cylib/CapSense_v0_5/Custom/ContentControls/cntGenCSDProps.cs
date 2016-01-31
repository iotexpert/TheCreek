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

namespace  CapSense_v0_5
{

    public partial class cntGenCSDProps : cntGenCSBaseProps
    {

        CyAmuxBParams cCSHalf = null;
        CyGeneralParams packParam = null;
        //double fixedPictureKoef = (double)980/700;//height/width
        public cntGenCSDProps()
        {
            
            InitializeComponent();
            //cbPRS.SelectedIndex = 0;
            Method = eCapSenseMode.CSD;
            cbCIS.SelectedIndex = 0;
            pbGraph.SizeMode = PictureBoxSizeMode.Zoom;
            //panelGraph.SizeChanged += new EventHandler(panelGraph_SizeChanged);
            panelGraph.BackColor = Color.White;
            rbIDACSourcing.CheckedChanged+=new EventHandler(rbIDACSourcing_CheckedChanged);
            rbIDACSinking.CheckedChanged+=new EventHandler(rbIDACSinking_CheckedChanged);
            rbRBEnable.CheckedChanged+=new EventHandler(rbRBEnable_CheckedChanged);
        }
        private void rbMode_CheckedChanged(object sender, EventArgs e)
        {
            SendProperties(cCSHalf);
        }
        void graphRePaint()
        {
            if (cCSHalf != null)
            {              
                string strEnd = "";
                string strStart = "";

                //define Image string
                strStart = packParam.GetPrefixForParams(cCSHalf);
                if (packParam.Configuration == eMConfiguration.emParallelSynchron) strStart = "l";

                if (cCSHalf.IsPRS())
                    strEnd = "PRS";
                //Assigne Image
                switch (cCSHalf.csdSubMethod)
                {
                    case CSDSubMethods.IDACSourcing:
                        pbGraph.Image = ((System.Drawing.Image)(global:: CapSense_v0_5.ResAll.ResourceManager.GetObject(strStart + "IDACSourcing" + strEnd)));
                        break;
                    case CSDSubMethods.IDACSinking:
                        pbGraph.Image = ((System.Drawing.Image)(global:: CapSense_v0_5.ResAll.ResourceManager.GetObject(strStart + "IDACSinking" + strEnd)));
                        break;
                    case CSDSubMethods.IDACDisable_RB:
                        pbGraph.Image = ((System.Drawing.Image)(global:: CapSense_v0_5.ResAll.ResourceManager.GetObject(strStart + "Rb" + strEnd)));
                        break;
                    default:
                        break;
                }
            }
        }
        #region Get/Send
        public override void GetProperties(CyAmuxBParams cCSHalf,CyGeneralParams packParam)
        {           
            this.cCSHalf = null;
            this.cbPRS.Items.Clear();
            this.cbPRS.Items.AddRange((new List<String>(cCSHalf.strPRS.Keys)).ToArray());

           rbRBEnable.Checked=  cCSHalf.isRbEnable;
           cbCIS.SelectedIndex = cCSHalf.cis;
          tbSEC.Text= cCSHalf.countShieldElectrode.ToString();
          cbPRS.SelectedIndex = cCSHalf.prs;
          tbRb.Text = cCSHalf.countRb.ToString();

          switch (cCSHalf.csdSubMethod)
          {
              case CSDSubMethods.IDACSourcing:
                  rbIDACSourcing.Checked = true;
                  break;
              case CSDSubMethods.IDACSinking:
                  rbIDACSinking.Checked = true;
                  break;
              case CSDSubMethods.IDACDisable_RB:
                  rbRBEnable.Checked = true;
                  break;
              default:
                  break;
          }
          this.cCSHalf = cCSHalf;
          this.packParam = packParam;
         
          graphRePaint();
        }
        public override void SendProperties(CyAmuxBParams cCSHalf)
        {
            if(cCSHalf!=null)
            {
                cCSHalf.isRbEnable = rbRBEnable.Checked;
                cCSHalf.cis = cbCIS.SelectedIndex;
                cCSHalf.prs = cbPRS.SelectedIndex;
                cCSHalf.isIdac = rbIDACSourcing.Checked || rbIDACSinking.Checked;
                //cCSHalf.cRb.Validate(

                if (tbSEC.Visible)
                    cCSHalf.cShieldElectrode.Validate(tbSEC.Text);
                else
                    cCSHalf.cShieldElectrode.Validate(0);

                if (rbIDACSourcing.Checked) cCSHalf.csdSubMethod = CSDSubMethods.IDACSourcing;
                else if (rbIDACSinking.Checked) cCSHalf.csdSubMethod = CSDSubMethods.IDACSinking;
                else if (rbRBEnable.Checked) cCSHalf.csdSubMethod = CSDSubMethods.IDACDisable_RB;

                //Rb Count Validate
                if (cCSHalf.isRbEnable)
                {
                    cCSHalf.cRb.Validate(tbRb);
                }
                //else
                //    cCSHalf.cRb.Validate(0);
                graphRePaint();
            }
        }
        #endregion

        public void ShElVisibil(bool state)
        {
            lSEC.Visible = state;
            tbSEC.Visible = state;
            if(state)
                Add_RemoveItem("Driven Shield", cbCIS, state);
        }
        public void RbVisibil(bool state)
        {
            lRb.Visible = state;
            tbRb.Visible = state;
        }

        private void rbIDACSourcing_CheckedChanged(object sender, EventArgs e)
        {
            Add_RemoveItem("Driven Shield", cbCIS, false);
        }
        private void rbRBEnable_CheckedChanged(object sender, EventArgs e)
        {
            RbVisibil(((RadioButton)sender).Checked);
            ShElVisibil(((RadioButton)sender).Checked);
        }

        private void rbIDACSinking_CheckedChanged(object sender, EventArgs e)
        {
            ShElVisibil(((RadioButton)sender).Checked);
        }
        void Add_RemoveItem(string strItem, ComboBox cb,bool bAdd)
        {
            if (bAdd)
            {
                if(cb.Items.IndexOf(strItem)==-1)
                cb.Items.Add(strItem);
            }
            else
            {
                if (cb.Items[cb.SelectedIndex].ToString() == strItem)
                {
                    MessageBox.Show("Connect Inactive Sensors will change to " +cb.Items[0].ToString()+ ".", "Warning", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                    cb.SelectedIndex = 0;
                }
                cb.Items.Remove(strItem);
            }
        }

        private void tbSEC_Validating(object sender, CancelEventArgs e)
        {
            if (cCSHalf != null)
            {
                cCSHalf.cShieldElectrode.Validate(tbSEC);
            }

        }

        private void tbRb_Validating(object sender, CancelEventArgs e)
        {
            if (cCSHalf != null)
            {
                cCSHalf.cRb.Validate(tbRb);
            }
        }

     }


}
