//*******************************************************************************
// File Name: CyComparatorControl.cs
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
using CyDesigner.Extensions.Common;
using CyDesigner.Extensions.Gde;
using Comp_v1_50;


namespace Comp_v1_50
{
    public partial class CyComparatorControl : UserControl
    {

        public const int PB_COMP_TEXTWIDTH = 40;
        public const int PB_EXTENTS_BORDER = 5;
        public const int PB_POLYGON_WIDTH = 4;
        public ICyInstEdit_v1 m_Component = null;
        const string Hysteresis = null;
        const string EnableHy = null;
        const string DisableHy = null;
        const string Speed = null;
        const string Low_Power = null;
        const string Slow = null;
        const string Fast = null;
        const string Pd_Override = null;
        const string Enable_PD_OVerride = null;
        const string Disable_PD_Override = null;
        const string Polarity = null;
        const string Non_Inverting = null;
        const string Inverting = null;
        const string Sync = null;
        const string Normal = null;
        const string Bypass = null;

        public CyComparatorControl(ICyInstEdit_v1 inst)
        {
            m_Component = inst;
            InitializeComponent();
            
            if (m_Component != null)
            {
                UpdateFormFromParams(m_Component);
            }
            
        }

        
        public void UpdateDrawing(ComparatorParameters prms)
        {
            CancelEventArgs ce = new CancelEventArgs();
            if ((pictureBox1.Width == 0) || (pictureBox1.Height == 0))
                return;
            Image Waveform = new Bitmap(pictureBox1.Width, pictureBox1.Height);
            Graphics wfg;
            wfg = Graphics.FromImage(Waveform);
            wfg.Clear(Color.White);
            SolidBrush blkbrush = new SolidBrush(Color.Black);

            Pen extentspen = new Pen(blkbrush);
            extentspen.DashStyle = System.Drawing.Drawing2D.DashStyle.Dash;
            // Setup the input wave 
            wfg.DrawLine(extentspen, 10, 30, 400, 30);
            wfg.DrawLine(extentspen, 10, 60, 400, 60);
            extentspen.DashStyle = System.Drawing.Drawing2D.DashStyle.Solid;
            wfg.DrawLine(extentspen, 10, 80, 100, 20);
            wfg.DrawLine(extentspen, 100, 20, 200, 80);
            wfg.DrawLine(extentspen, 200, 80, 300, 20);
            wfg.DrawLine(extentspen, 300, 20, 400, 80);

            //Setup the output square wave for noninverting 
            if (m_rbPolarity_NonInve.Checked)
            {
                wfg.DrawLine(extentspen, 10, 140, 50, 140);
                wfg.DrawLine(extentspen, 50, 140, 50, 100);
                wfg.DrawLine(extentspen, 50, 100, 150, 100);
                wfg.DrawLine(extentspen, 150, 100, 150, 140);
                wfg.DrawLine(extentspen, 150, 140, 250, 140);
                wfg.DrawLine(extentspen, 250, 140, 250, 100);
                wfg.DrawLine(extentspen, 250, 100, 350, 100);
                wfg.DrawLine(extentspen, 350, 100, 350, 140);
                wfg.DrawLine(extentspen, 350, 140, 400, 140);
            }

            //Setup the output square wave for inverting
            if (m_rbPolarity_Inve.Checked)
            {
                wfg.DrawLine(extentspen, 10, 100, 50, 100);
                wfg.DrawLine(extentspen, 50, 100, 50, 140);
                wfg.DrawLine(extentspen, 50, 140, 150, 140);
                wfg.DrawLine(extentspen, 150, 140, 150, 100);
                wfg.DrawLine(extentspen, 150, 100, 250, 100);
                wfg.DrawLine(extentspen, 250, 100, 250, 140);
                wfg.DrawLine(extentspen, 250, 140, 350, 140);
                wfg.DrawLine(extentspen, 350, 140, 350, 100);
                wfg.DrawLine(extentspen, 350, 100, 400, 100);
            }

            Font perfont = new Font("Arial", 10, FontStyle.Regular, GraphicsUnit.Pixel);
            wfg.DrawString("Vth+Hy", perfont, blkbrush, new PointF(PB_COMP_TEXTWIDTH - wfg.MeasureString("Period", perfont).Width,
                PB_EXTENTS_BORDER + PB_POLYGON_WIDTH - (wfg.MeasureString("period", perfont).Height / 2)));
            wfg.DrawString("Vth+Hy", perfont, blkbrush,new PointF(10,35));
            pictureBox1.Image = Waveform;

            return;
                               
        }


        public void UpdateFormFromParams(ICyInstEdit_v1 inst)
        {
            ComparatorParameters prms = new ComparatorParameters(inst);
            const string HysteresisEnable = "1";
            const string HysteresisDisable = "0";
            const string SpeedOff = "0";
            const string SpeedSlow = "1";
            const string SpeedFast = "2";
            const string PDOverrideEnable = "1";
            const string PDOverrideDisable = "0";
            const string PolarityNonInv = "0";
            const string PolarityInv = "1";
            const string SyncNorm = "0";
            const string SyncBypass = "1";
            
            //Set the Hysteresis Radio Buttons
            switch (prms.Comp_Hysteresis.Value)
            {
                case HysteresisEnable  : m_rbHysteresis_Enable.Checked = true;
                                         break;
                case HysteresisDisable : m_rbHysteresis_Disable.Checked = true;
                                         break;
            }

            //Set the Speed Radio Buttons
            switch (prms.Comp_Speed.Value)
            {
                case SpeedOff  : m_rbPower_Slow.Checked = true;
                                 break;
                case SpeedSlow : m_rbPower_Fast.Checked = true;
                                 break;
                case SpeedFast : m_rbPower_Off.Checked = true;
                                 break;
            }
           
            //Set the PowerDownOverride Radio Button
            switch (prms.Comp_PDOverride.Value)
            {
                case PDOverrideEnable  : m_rbPDOverride_Enable.Checked = true;
                                         break;
                case PDOverrideDisable : m_rbPDOverride_Disable.Checked = true;
                                         break;
            }
            
            //Set the Polarity Radio Button
            switch (prms.Comp_Polarity.Value)
            {
                case PolarityNonInv : m_rbPolarity_NonInve.Checked = true;
                                      break;
                case PolarityInv    : m_rbPolarity_Inve.Checked = true;
                                      break;
            }
            
            //Set the Sync Radio Button
            switch (prms.Comp_Sync.Value)
            {
                case SyncNorm   : m_rbSync_Norm.Checked = true;
                                  break;
                case SyncBypass : m_rbSync_Bypass.Checked = true;
                                  break;
            }
                      
        }

        //Check if Hysteresis Enable Radio Button is checked and set the image accordingly 
        private void m_rbHysteresis_Enable_CheckedChanged(object sender, EventArgs e)
        {
            if (m_rbHysteresis_Enable.Checked)
            {
                ComparatorParameters prms = new ComparatorParameters(m_Component);
                SetAParameter("Hysteresis", "EnableHy", true);
                if (m_rbPolarity_NonInve.Checked && m_rbSync_Norm.Checked)
                    pictureBox2.Image = Comp_v1_50.Properties.Resources.hy_ninv_sync;
                if (m_rbHysteresis_Enable.Checked && m_rbPolarity_NonInve.Checked && m_rbSync_Bypass.Checked)
                    pictureBox2.Image = Comp_v1_50.Properties.Resources.hysteresis;
                if (m_rbPolarity_Inve.Checked && m_rbSync_Norm.Checked)
                    pictureBox2.Image = Comp_v1_50.Properties.Resources.hy_inv_syn;
                if (m_rbPolarity_Inve.Checked && m_rbSync_Bypass.Checked)
                    pictureBox2.Image = Comp_v1_50.Properties.Resources.hy_inv;
             }
        }
       
        //Check if Hysteresis Disable Radiobutton checked and set the image accordingly 
        private void m_rbHysteresis_Disable_CheckedChanged(object sender, EventArgs e)
        {
            if (m_rbHysteresis_Disable.Checked)
            {
                ComparatorParameters prms = new ComparatorParameters(m_Component);
                SetAParameter("Hysteresis", "DisableHy", true);
                if (m_rbPolarity_NonInve.Checked && m_rbSync_Norm.Checked)
                    pictureBox2.Image = Comp_v1_50.Properties.Resources.sync;
                if(m_rbPolarity_NonInve.Checked && m_rbSync_Bypass.Checked)
                    pictureBox2.Image = Comp_v1_50.Properties.Resources.comp;
                if (m_rbPolarity_Inve.Checked && m_rbSync_Norm.Checked)
                    pictureBox2.Image = Comp_v1_50.Properties.Resources.inv_sync;
                if (m_rbPolarity_Inve.Checked && m_rbSync_Bypass.Checked)
                    pictureBox2.Image = Comp_v1_50.Properties.Resources.inverting;
            }
        }

        //Check if Power Off Radio Button is checked 
        private void m_rbPower_Off_CheckedChanged(object sender, EventArgs e)
        {
            if (m_rbPower_Off.Checked)
            {
                ComparatorParameters prms = new ComparatorParameters(m_Component);
                SetAParameter("Speed", "Low_Power", true);
            }
        }

        //Check if Power Slow Radio Button is checked 
        private void m_rbPower_Slow_CheckedChanged(object sender, EventArgs e)
        {
            if (m_rbPower_Slow.Checked)
            {
                ComparatorParameters prms = new ComparatorParameters(m_Component);
                SetAParameter("Speed", "Slow", true);
            } 
        }

        //Check if Power Fast Radio Button is checked 
        private void m_rbPower_Fast_CheckedChanged(object sender, EventArgs e)
        {
            if (m_rbPower_Fast.Checked)
            {
                ComparatorParameters prms = new ComparatorParameters(m_Component);
                SetAParameter("Speed", "Fast", true);
            }
        }

        //Check if PowerDownOverride Enable  Radio Button is checked
        private void m_rbPDOverride_Enable_CheckedChanged(object sender, EventArgs e)
        {
            if (m_rbPDOverride_Enable.Checked)
            {
                ComparatorParameters prms = new ComparatorParameters(m_Component);
                SetAParameter("Pd_Override", "Enable_PD_OVerride", true);
            }
        }

        //Check if POwerDownOverride Radio Button is checked
        private void m_rbPDOverride_Disable_CheckedChanged(object sender, EventArgs e)
        {
            if (m_rbPDOverride_Disable.Checked)
            {
                ComparatorParameters prms = new ComparatorParameters(m_Component);
                SetAParameter("Pd_Override", "Disable_PD_Override", true);
            }
        }

        //Check if Polarity Noninverting Radio Button is checked and set the image and waveform accordingly 
        private void m_rbPolarity_NonInve_CheckedChanged(object sender, EventArgs e)
        {
            if (m_rbPolarity_NonInve.Checked)
            {
                ComparatorParameters prms = new ComparatorParameters(m_Component);
                SetAParameter("Polarity", "Non_Inverting", true);
                if (m_rbHysteresis_Enable.Checked && m_rbSync_Norm.Checked)
                    pictureBox2.Image = Comp_v1_50.Properties.Resources.hy_ninv_sync;
                if (m_rbHysteresis_Enable.Checked && m_rbSync_Bypass.Checked)
                    pictureBox2.Image = Comp_v1_50.Properties.Resources.hysteresis;
                if (m_rbHysteresis_Disable.Checked && m_rbSync_Norm.Checked)
                    pictureBox2.Image = Comp_v1_50.Properties.Resources.sync;
                if (m_rbHysteresis_Disable.Checked && m_rbSync_Bypass.Checked)
                    pictureBox2.Image = Comp_v1_50.Properties.Resources.comp;
                UpdateDrawing(prms);
            }
        }

        //Check if Polaruty Inverting Radio Button is checked and set the image and waveform accordingly
        private void m_rbPolarity_Inve_CheckedChanged(object sender, EventArgs e)
        {
            if (m_rbPolarity_Inve.Checked)
            {
                ComparatorParameters prms = new ComparatorParameters(m_Component);
                SetAParameter("Polarity", "Inverting", true);
                if (m_rbHysteresis_Enable.Checked && m_rbSync_Norm.Checked)
                    pictureBox2.Image = Comp_v1_50.Properties.Resources.hy_inv_syn;
                if (m_rbHysteresis_Enable.Checked && m_rbSync_Bypass.Checked)
                    pictureBox2.Image = Comp_v1_50.Properties.Resources.hy_inv;
                if (m_rbHysteresis_Disable.Checked && m_rbSync_Norm.Checked)
                    pictureBox2.Image = Comp_v1_50.Properties.Resources.inv_sync;
                if (m_rbHysteresis_Disable.Checked && m_rbSync_Bypass.Checked)
                    pictureBox2.Image = Comp_v1_50.Properties.Resources.inverting;
                UpdateDrawing(prms);
            }
        }

        //Check if Sync Normal Radio Button is checked and set the image accordingly 
        private void m_rbSync_Norm_CheckedChanged(object sender, EventArgs e)
        {
            if (m_rbSync_Norm.Checked)
            {
                ComparatorParameters prms = new ComparatorParameters(m_Component);
                SetAParameter("Sync", "Normal", true);
                if (m_rbHysteresis_Enable.Checked && m_rbPolarity_NonInve.Checked)
                    pictureBox2.Image = Comp_v1_50.Properties.Resources.hy_ninv_sync;
                if (m_rbHysteresis_Enable.Checked && m_rbPolarity_Inve.Checked)
                   pictureBox2.Image = Comp_v1_50.Properties.Resources.hy_inv_syn;
                if (m_rbHysteresis_Disable.Checked && m_rbPolarity_NonInve.Checked)
                    pictureBox2.Image = Comp_v1_50.Properties.Resources.sync;
                if (m_rbHysteresis_Disable.Checked && m_rbPolarity_Inve.Checked)
                    pictureBox2.Image = Comp_v1_50.Properties.Resources.inv_sync;
            }
        }

        //Check if Sync bypass Radio Button is checked and set the image accordingly 
        private void m_rbSync_Bypass_CheckedChanged(object sender, EventArgs e)
        {
            if (m_rbSync_Bypass.Checked)
            {
                ComparatorParameters prms = new ComparatorParameters(m_Component);
                SetAParameter("Sync", "Bypass", true);
                if (m_rbHysteresis_Enable.Checked && m_rbPolarity_NonInve.Checked)
                    pictureBox2.Image = Comp_v1_50.Properties.Resources.hysteresis;
                if (m_rbHysteresis_Enable.Checked && m_rbPolarity_Inve.Checked)
                    pictureBox2.Image = Comp_v1_50.Properties.Resources.hy_inv;
                if (m_rbHysteresis_Disable.Checked && m_rbPolarity_NonInve.Checked)
                    pictureBox2.Image = Comp_v1_50.Properties.Resources.comp;
                if (m_rbHysteresis_Disable.Checked && m_rbPolarity_Inve.Checked)
                    pictureBox2.Image = Comp_v1_50.Properties.Resources.inverting;
            }
        }

        

        private void SetAParameter(string parameter, string value, bool CheckFocus)
        {
            if (this.ContainsFocus || !CheckFocus)
            {
                m_Component.SetParamExpr(parameter, value);
                m_Component.CommitParamExprs();
                if (m_Component.GetCommittedParam(parameter).ErrorCount != 0)
                {
                    string errors = null;
                    foreach (string err in m_Component.GetCommittedParam(parameter).Errors)
                    {
                        errors = errors + err + "\n";
                    }
                    MessageBox.Show(string.Format("Error Setting Parameter {0} with value {1}\n\nErrors:\n{2}", parameter, value, errors),
                        "Error Setting Parameter", MessageBoxButtons.OK, MessageBoxIcon.Error);
                }
            }
        }
    }
}
