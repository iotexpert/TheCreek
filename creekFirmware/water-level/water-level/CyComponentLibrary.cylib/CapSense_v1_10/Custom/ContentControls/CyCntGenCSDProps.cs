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

namespace  CapSense_v1_10
{
    public partial class CyCntGenCSDProps : CyCntGenCSBaseProps
    {
        public CyCntGenCSDProps()
        {
            InitializeComponent();            
            m_Method = E_CAPSENSE_MODE.CSD;

            cbCIS.SelectedIndex = 0;
            this.cbPRS.Items.Clear();
            this.cbPRS.Items.AddRange((new List<String>((new CyAmuxBParams()).m_strPRS.Keys)).ToArray());
            cbPRS.SelectedIndex = 0;

            pbGraph.SizeMode = PictureBoxSizeMode.Zoom;
            //panelGraph.SizeChanged += new EventHandler(panelGraph_SizeChanged);
            panelGraph.BackColor = Color.White;
            rbIDACSourcing.CheckedChanged+=new EventHandler(rbIDACSourcing_CheckedChanged);
            rbIDACSinking.CheckedChanged+=new EventHandler(rbIDACSinking_CheckedChanged);
            rbRBEnable.CheckedChanged+=new EventHandler(rbRBEnable_CheckedChanged);
        }
        void GraphRePaint()
        {
            if (m_cCSHalf != null)
            {              
                string strEnd = "";
                string strStart = "";

                //define Image string
                strStart = m_packParam.GetPrefixForParams(m_cCSHalf);
                if (m_packParam.Configuration == E_MAIN_CONFIG.emParallelSynchron) strStart = "l";

                if (m_cCSHalf.IsPRS())
                    strEnd = "PRS";
                //Assigne Image
                switch (m_cCSHalf.m_csdSubMethod)
                {
                    case E_CSD_SUB_METHODS.IDACSourcing:
                        pbGraph.Image = ((System.Drawing.Image)(
                            global:: CapSense_v1_10.ResAll.ResourceManager.GetObject(strStart + "IDACSourcing" + strEnd)));
                        break;
                    case E_CSD_SUB_METHODS.IDACSinking:
                        pbGraph.Image = ((System.Drawing.Image)(
                            global:: CapSense_v1_10.ResAll.ResourceManager.GetObject(strStart + "IDACSinking" + strEnd)));
                        break;
                    case E_CSD_SUB_METHODS.IDACDisable_RB:
                        pbGraph.Image = ((System.Drawing.Image)(
                            global:: CapSense_v1_10.ResAll.ResourceManager.GetObject(strStart + "Rb" + strEnd)));
                        break;
                    default:
                        break;
                }
            }
        }
        #region Get/Send
        public override void GetProperties(CyAmuxBParams cCSHalf,CyGeneralParams packParam)
        {           
            this.m_cCSHalf = null;
            this.cbPRS.Items.Clear();
            this.cbPRS.Items.AddRange((new List<String>(cCSHalf.m_strPRS.Keys)).ToArray());

           rbRBEnable.Checked=  cCSHalf.m_isRbEnable;
           cbCIS.SelectedIndex = cCSHalf.m_cis;
          tbSEC.Text= cCSHalf.CountShieldElectrode.ToString();
          cbPRS.SelectedIndex = cCSHalf.m_prs;
          tbRb.Text = cCSHalf.CountRb.ToString();

          switch (cCSHalf.m_csdSubMethod)
          {
              case E_CSD_SUB_METHODS.IDACSourcing:
                  rbIDACSourcing.Checked = true;
                  break;
              case E_CSD_SUB_METHODS.IDACSinking:
                  rbIDACSinking.Checked = true;
                  break;
              case E_CSD_SUB_METHODS.IDACDisable_RB:
                  rbRBEnable.Checked = true;
                  break;
              default:
                  break;
          }
          this.m_cCSHalf = cCSHalf;
          this.m_packParam = packParam;
         
          GraphRePaint();
        }
        public override void SendProperties(CyAmuxBParams cCSHalf)
        {
            if(cCSHalf!=null)
            {
                cCSHalf.m_isRbEnable = rbRBEnable.Checked;
                cCSHalf.m_cis = cbCIS.SelectedIndex;
                cCSHalf.m_prs = cbPRS.SelectedIndex;
                cCSHalf.m_isIdac = rbIDACSourcing.Checked || rbIDACSinking.Checked;
                //cCSHalf.cRb.Validate(

                if (tbSEC.Visible)
                    cCSHalf.m_cShieldElectrode.Validate(tbSEC.Text);
                else
                    cCSHalf.m_cShieldElectrode.Validate(0);

                if (rbIDACSourcing.Checked) cCSHalf.m_csdSubMethod = E_CSD_SUB_METHODS.IDACSourcing;
                else if (rbIDACSinking.Checked) cCSHalf.m_csdSubMethod = E_CSD_SUB_METHODS.IDACSinking;
                else if (rbRBEnable.Checked) cCSHalf.m_csdSubMethod = E_CSD_SUB_METHODS.IDACDisable_RB;

                //Rb Count Validate
                if (cCSHalf.m_isRbEnable)
                {
                    cCSHalf.m_cRb.Validate(tbRb);
                }
                //else
                //    cCSHalf.cRb.Validate(0);
                GraphRePaint();

                //Save Data
                if (m_packParam != null)
                    m_packParam.SetCommitParams(null, null);
            }
        }
        #endregion

        #region Actions
        private void rbMode_CheckedChanged(object sender, EventArgs e)
        {
            SendProperties(m_cCSHalf);
        }
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
                    MessageBox.Show("Connect Inactive Sensors will change to " +cb.Items[0].ToString()+ ".", 
                        "Warning", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                    cb.SelectedIndex = 0;
                }
                cb.Items.Remove(strItem);
            }
        }

        private void tbSEC_Validating(object sender, CancelEventArgs e)
        {
            if (m_cCSHalf != null)
            {
                m_cCSHalf.m_cShieldElectrode.Validate(tbSEC);
                SendProperties(m_cCSHalf);
            }
        }

        private void tbRb_Validating(object sender, CancelEventArgs e)
        {
            if (m_cCSHalf != null)
            {
                m_cCSHalf.m_cRb.Validate(tbRb);
                SendProperties(m_cCSHalf);
            }
        }
        #endregion

    }


}
