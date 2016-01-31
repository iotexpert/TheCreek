/*******************************************************************************
 File Name: CyComparatorControl.cs
*******************************************************************************
* Copyright 2008-2011, Cypress Semiconductor Corporation.  All rights reserved.
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
using Comp_v1_80;


namespace Comp_v1_80
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
            
            //if (m_Component != null)
            //{
            //    UpdateFormFromParams(m_Component);
            //}
        }

        
        public void UpdateDrawing(CyComparatorParameters prms)
        {
            CancelEventArgs ce = new CancelEventArgs();
            if ((pictureBox1.Width == 0) || (pictureBox1.Height == 0))
                return;
            Image Waveform = new Bitmap(pictureBox1.Width, pictureBox1.Height);
            Graphics wfg;
            wfg = Graphics.FromImage(Waveform);
            wfg.Clear(Color.White);
            SolidBrush blkbrush = new SolidBrush(Color.Black);
            SolidBrush greenbrush = new SolidBrush(Color.Green);
            SolidBrush bluebrush = new SolidBrush(Color.Blue);
            SolidBrush orangebrush = new SolidBrush(Color.Orange);
            Pen extentspen = new Pen(blkbrush);
            Pen greenpen = new Pen(greenbrush);
            Pen bluepen = new Pen(bluebrush);
            Pen orangepen = new Pen(orangebrush);
            extentspen.DashStyle = System.Drawing.Drawing2D.DashStyle.Dash;
            // Setup the input wave 
            wfg.DrawLine(extentspen, 40, 20, 400, 20);
            wfg.DrawLine(greenpen, 40, 50, 400, 50);
            wfg.DrawLine(extentspen, 40, 75, 400, 75);
            extentspen.DashStyle = System.Drawing.Drawing2D.DashStyle.Solid;
            wfg.DrawLine(extentspen, 40, 10, 40, 150);
            wfg.DrawLine(bluepen, 40, 70, 130, 20);
            wfg.DrawLine(bluepen, 130, 20, 220, 70);
            wfg.DrawLine(bluepen, 220, 70, 310, 20);
            wfg.DrawLine(bluepen, 310, 20, 400, 70);


            if (m_rbHysteresis_Enable.Checked)
            {
                wfg.DrawLine(greenpen, 40, 45, 400, 45);
                extentspen.DashStyle = System.Drawing.Drawing2D.DashStyle.Dash;
                wfg.DrawLine(extentspen, 86, 96, 86, 35);
                wfg.DrawLine(extentspen, 183, 96, 183, 35);
                wfg.DrawLine(extentspen, 267, 96, 267, 35);
                wfg.DrawLine(extentspen, 364, 96, 364, 35);
                extentspen.StartCap = System.Drawing.Drawing2D.LineCap.ArrowAnchor;
                extentspen.EndCap = System.Drawing.Drawing2D.LineCap.Flat;
                wfg.DrawLine(extentspen, 45, 45, 45, 30);
                wfg.DrawLine(extentspen, 45, 50, 45, 65);
                Font arrow = new Font("Arial", 10, FontStyle.Regular, GraphicsUnit.Pixel);
                wfg.DrawString("10mV", arrow, blkbrush, 45, 30);
                //Setup the output square wave for noninverting 
                if (m_rbPolarity_NonInve.Checked)
                {
                    if (m_rbPower_Off.Checked)
                    {
                        wfg.DrawLine(orangepen, 40, 140, 95, 140);
                        wfg.DrawLine(orangepen, 95, 140, 113, 100);
                        wfg.DrawLine(orangepen, 113, 100, 189, 100);
                        wfg.DrawLine(orangepen, 189, 100, 208, 140);
                        wfg.DrawLine(orangepen, 208, 140, 278, 140);
                        wfg.DrawLine(orangepen, 278, 140, 296, 100);
                        wfg.DrawLine(orangepen, 296, 100, 370, 100);
                        wfg.DrawLine(orangepen, 370, 100, 389, 140);
                        wfg.DrawLine(orangepen, 389, 140, 405, 140);
                    }
                    else if (m_rbPower_Fast.Checked)
                    {
                        wfg.DrawLine(orangepen, 40, 140, 85, 140);
                        wfg.DrawLine(orangepen, 86, 140, 86, 100);
                        wfg.DrawLine(orangepen, 86, 100, 183, 100);
                        wfg.DrawLine(orangepen, 183, 100, 183, 140);
                        wfg.DrawLine(orangepen, 183, 140, 267, 140);
                        wfg.DrawLine(orangepen, 267, 140, 267, 100);
                        wfg.DrawLine(orangepen, 267, 100, 364, 100);
                        wfg.DrawLine(orangepen, 364, 100, 364, 140);
                        wfg.DrawLine(orangepen, 364, 140, 400, 140);
                    }
                    else //if (m_rbPower_Slow.Checked)
                    {
                        wfg.DrawLine(orangepen, 40, 140, 80, 140);
                        wfg.DrawLine(orangepen, 80, 140, 94, 100);
                        wfg.DrawLine(orangepen, 95, 100, 174, 100);
                        wfg.DrawLine(orangepen, 174, 100, 190, 140);
                        wfg.DrawLine(orangepen, 190, 140, 260, 140);
                        wfg.DrawLine(orangepen, 260, 140, 275, 100);
                        wfg.DrawLine(orangepen, 275, 100, 355, 100);
                        wfg.DrawLine(orangepen, 354, 100, 375, 140);
                        wfg.DrawLine(orangepen, 375, 140, 405, 140);
                    }
                }

                //Setup the output square wave for inverting
                if (m_rbPolarity_Inve.Checked)
                {
                    
                    if (m_rbPower_Off.Checked)
                    {
                        wfg.DrawLine(orangepen, 40, 100, 95, 100);
                        wfg.DrawLine(orangepen, 95, 100, 113, 140);
                        wfg.DrawLine(orangepen, 113, 140, 190, 140);
                        wfg.DrawLine(orangepen, 190, 140, 208, 100);
                        wfg.DrawLine(orangepen, 208, 100, 278, 100);
                        wfg.DrawLine(orangepen, 278, 100, 296, 140);
                        wfg.DrawLine(orangepen, 296, 140, 372, 140);
                        wfg.DrawLine(orangepen, 372, 140, 389, 100);
                        wfg.DrawLine(orangepen, 389, 100, 405, 100);
                    }
                    else if (m_rbPower_Fast.Checked)
                    {
                        wfg.DrawLine(orangepen, 40, 100, 86, 100);
                        wfg.DrawLine(orangepen, 86, 100, 86, 140);
                        wfg.DrawLine(orangepen, 86, 140, 183, 140);
                        wfg.DrawLine(orangepen, 183, 140, 183, 100);
                        wfg.DrawLine(orangepen, 183, 100, 267, 100);
                        wfg.DrawLine(orangepen, 267, 100, 267, 140);
                        wfg.DrawLine(orangepen, 267, 140, 364, 140);
                        wfg.DrawLine(orangepen, 364, 140, 364, 100);
                        wfg.DrawLine(orangepen, 364, 100, 400, 100);
                    }
                    else  //if (m_rbPower_Slow.Checked)
                    {
                        wfg.DrawLine(orangepen, 40, 100, 78, 100);
                        wfg.DrawLine(orangepen, 78, 100, 91, 140);
                        wfg.DrawLine(orangepen, 91, 140, 178, 140);
                        wfg.DrawLine(orangepen, 178, 140, 192, 100);
                        wfg.DrawLine(orangepen, 192, 100, 258, 100);
                        wfg.DrawLine(orangepen, 258, 100, 273, 140);
                        wfg.DrawLine(orangepen, 273, 140, 360, 140);
                        wfg.DrawLine(orangepen, 360, 140, 372, 100);
                        wfg.DrawLine(orangepen, 372, 100, 405, 100);
                    }
                }
            }

            if (m_rbHysteresis_Disable.Checked)
            {
                extentspen.DashStyle = System.Drawing.Drawing2D.DashStyle.Dash;
                wfg.DrawLine(extentspen, 76, 96, 76, 35);
                wfg.DrawLine(extentspen, 183, 96, 183, 35);
                wfg.DrawLine(extentspen, 257, 96, 257, 35);
                wfg.DrawLine(extentspen, 364, 96, 364, 35);
                if (m_rbPolarity_NonInve.Checked)
                {
                    if (m_rbPower_Off.Checked)
                    {
                        wfg.DrawLine(orangepen, 40, 140, 85, 140);
                        wfg.DrawLine(orangepen, 85, 140, 103, 100);
                        wfg.DrawLine(orangepen, 103, 100, 189, 100);
                        wfg.DrawLine(orangepen, 189, 100, 206, 140);
                        wfg.DrawLine(orangepen, 206, 140, 268, 140);
                        wfg.DrawLine(orangepen, 268, 140, 286, 100);
                        wfg.DrawLine(orangepen, 286, 100, 370, 100);
                        wfg.DrawLine(orangepen, 370, 100, 387, 140);
                        wfg.DrawLine(orangepen, 387, 140, 405, 140);
                    }
                    else if (m_rbPower_Fast.Checked)
                    {
                        wfg.DrawLine(orangepen, 40, 140, 76, 140);
                        wfg.DrawLine(orangepen, 76, 140, 76, 100);
                        wfg.DrawLine(orangepen, 76, 100, 183, 100);
                        wfg.DrawLine(orangepen, 183, 100, 183, 140);
                        wfg.DrawLine(orangepen, 183, 140, 257, 140);
                        wfg.DrawLine(orangepen, 257, 140, 257, 100);
                        wfg.DrawLine(orangepen, 257, 100, 364, 100);
                        wfg.DrawLine(orangepen, 364, 100, 364, 140);
                        wfg.DrawLine(orangepen, 364, 140, 405, 140);
                    }
                    else //if (m_rbPower_Slow.Checked)
                    {
                        wfg.DrawLine(orangepen, 40, 140, 70, 140);
                        wfg.DrawLine(orangepen, 70, 140, 84, 100);
                        wfg.DrawLine(orangepen, 84, 100, 173, 100);
                        wfg.DrawLine(orangepen, 173, 100, 191, 140);
                        wfg.DrawLine(orangepen, 191, 140, 251, 140);
                        wfg.DrawLine(orangepen, 251, 140, 266, 100);
                        wfg.DrawLine(orangepen, 266, 100, 354, 100);
                        wfg.DrawLine(orangepen, 354, 100, 372, 140);
                        wfg.DrawLine(orangepen, 372, 140, 405, 140);
                    }
                }
                if (m_rbPolarity_Inve.Checked)
                {
                    if (m_rbPower_Off.Checked)
                    {
                        wfg.DrawLine(orangepen, 40, 100, 85, 100);
                        wfg.DrawLine(orangepen, 85, 100, 103, 140);
                        wfg.DrawLine(orangepen, 103, 140, 189, 140);
                        wfg.DrawLine(orangepen, 189, 140, 206, 100);
                        wfg.DrawLine(orangepen, 206, 100, 268, 100);
                        wfg.DrawLine(orangepen, 268, 100, 286, 140);
                        wfg.DrawLine(orangepen, 286, 140, 370, 140);
                        wfg.DrawLine(orangepen, 370, 140, 387, 100);
                        wfg.DrawLine(orangepen, 387, 100, 405, 100);
                    }
                    else if (m_rbPower_Fast.Checked)
                    {
                        wfg.DrawLine(orangepen, 40, 100, 76, 100);
                        wfg.DrawLine(orangepen, 76, 100, 76, 140);
                        wfg.DrawLine(orangepen, 76, 140, 183, 140);
                        wfg.DrawLine(orangepen, 183, 140, 183, 100);
                        wfg.DrawLine(orangepen, 183, 100, 257, 100);
                        wfg.DrawLine(orangepen, 257, 100, 257, 140);
                        wfg.DrawLine(orangepen, 257, 140, 364, 140);
                        wfg.DrawLine(orangepen, 364, 140, 364, 100);
                        wfg.DrawLine(orangepen, 364, 100, 405, 100);
                    }
                    else //if (m_rbPower_Slow.Checked)
                    {
                        wfg.DrawLine(orangepen, 40, 100, 67, 100);
                        wfg.DrawLine(orangepen, 67, 100, 83, 140);
                        wfg.DrawLine(orangepen, 83, 140, 176, 140);
                        wfg.DrawLine(orangepen, 176, 140, 192, 100);
                        wfg.DrawLine(orangepen, 192, 100, 249, 100);
                        wfg.DrawLine(orangepen, 249, 100, 264, 140);
                        wfg.DrawLine(orangepen, 264, 140, 358, 140);
                        wfg.DrawLine(orangepen, 358, 140, 371, 100);
                        wfg.DrawLine(orangepen, 371, 100, 405, 100);
                    }
                }
            }

            Font perfont = new Font("Arial", 10, FontStyle.Regular, GraphicsUnit.Pixel);
            wfg.DrawString("Vdda", perfont, blkbrush, new PointF(7,10));
            wfg.DrawString("0V", perfont, blkbrush,new PointF(17,70));
            wfg.DrawString("Vddd", perfont, blkbrush, new PointF(7, 95));
            wfg.DrawString("Vssd", perfont, blkbrush, new PointF(7, 135));
            wfg.DrawString("V+", perfont, bluebrush, new PointF(17,60));
            wfg.DrawString("V-", perfont, greenbrush, new PointF(17, 40));
            wfg.DrawString("out", perfont, greenbrush, new PointF(17,115));
            pictureBox1.Image = Waveform;

            return;
                               
        }


        public void UpdateFormFromParams(ICyInstEdit_v1 inst)
        {
            CyComparatorParameters prms = new CyComparatorParameters(inst);
            const string HysteresisEnable = "1";
            const string HysteresisDisable = "0";
            const string SpeedSlow = "0";
            const string SpeedFast = "1";
            const string SpeedOff = "2";
            const string PDOverrideEnable = "1";
            const string PDOverrideDisable = "0";
            const string PolarityNonInv = "0";
            const string PolarityInv = "1";
            const string SyncNorm = "0";
            const string SyncBypass = "1";

            /* If PSoC5 ES1 silicon is selected, disable the PowerDown override
             * enable / disable group box as it is not supported for PSoC5 ES1.
            */
            if (inst.DeviceQuery.IsPSoC5 && inst.DeviceQuery.SiliconRevision < 2)
            {
                groupBox3.Enabled = false;
                m_rbPDOverride_Disable.Checked = true;
            }

            //Set the Hysteresis Radio Buttons
            switch (prms.m_Comp_Hysteresis.Value)
            {
                case HysteresisEnable  : m_rbHysteresis_Enable.Checked = true;
                                         break;
                case HysteresisDisable : m_rbHysteresis_Disable.Checked = true;
                                         break;
            }

            //Set the Speed Radio Buttons
            switch (prms.m_Comp_Speed.Value)
            {
                case SpeedSlow  : m_rbPower_Slow.Checked = true;
                                 break;
                case SpeedFast : m_rbPower_Fast.Checked = true;
                                 break;
                case SpeedOff : m_rbPower_Off.Checked = true;
                                 break;
            }
           
            //Set the PowerDownOverride Radio Button
            switch (prms.m_Comp_PDOverride.Value)
            {
                 case PDOverrideEnable  : m_rbPDOverride_Enable.Checked = true;
                                         break;
                 case PDOverrideDisable : m_rbPDOverride_Disable.Checked = true;
                                         break;
            }
            
            //Set the Polarity Radio Button
            switch (prms.m_Comp_Polarity.Value)
            {
                case PolarityNonInv : m_rbPolarity_NonInve.Checked = true;
                                      break;
                case PolarityInv    : m_rbPolarity_Inve.Checked = true;
                                      break;
            }
            
            //Set the Sync Radio Button
            switch (prms.m_Comp_Sync.Value)
            {
                case SyncNorm   : m_rbSync_Norm.Checked = true;
                                  break;
                case SyncBypass : m_rbSync_Bypass.Checked = true;
                                  break;
            }

            /* Disable the power down override group box */
            if (inst.DeviceQuery.IsPSoC5 && inst.DeviceQuery.SiliconRevision < 2)
            {
                m_rbPDOverride_Disable.Checked = true;
            }
        }

        //Check if Hysteresis Enable Radio Button is checked and set the image accordingly 
        private void m_rbHysteresis_Enable_CheckedChanged(object sender, EventArgs e)
        {
            if (m_rbHysteresis_Enable.Checked)
            {
                CyComparatorParameters prms = new CyComparatorParameters(m_Component);
                SetAParameter("Hysteresis", "EnableHy", true);
                if (m_rbPolarity_NonInve.Checked && m_rbSync_Norm.Checked)
                    pictureBox2.Image = Comp_v1_80.Properties.Resources.hy_ninv_sync;
                if (m_rbHysteresis_Enable.Checked && m_rbPolarity_NonInve.Checked && m_rbSync_Bypass.Checked)
                    pictureBox2.Image = Comp_v1_80.Properties.Resources.hysteresis;
                if (m_rbPolarity_Inve.Checked && m_rbSync_Norm.Checked)
                    pictureBox2.Image = Comp_v1_80.Properties.Resources.hy_inv_syn;
                if (m_rbPolarity_Inve.Checked && m_rbSync_Bypass.Checked)
                    pictureBox2.Image = Comp_v1_80.Properties.Resources.hy_inv;
                UpdateDrawing(prms);
             }
        }
       
        //Check if Hysteresis Disable Radiobutton checked and set the image accordingly 
        private void m_rbHysteresis_Disable_CheckedChanged(object sender, EventArgs e)
        {
            if (m_rbHysteresis_Disable.Checked)
            {
                CyComparatorParameters prms = new CyComparatorParameters(m_Component);
                SetAParameter("Hysteresis", "DisableHy", true);
                if (m_rbPolarity_NonInve.Checked && m_rbSync_Norm.Checked)
                    pictureBox2.Image = Comp_v1_80.Properties.Resources.sync;
                if (m_rbPolarity_NonInve.Checked && m_rbSync_Bypass.Checked)
                    pictureBox2.Image = Comp_v1_80.Properties.Resources.comp;
                if (m_rbPolarity_Inve.Checked && m_rbSync_Norm.Checked)
                    pictureBox2.Image = Comp_v1_80.Properties.Resources.inv_sync;
                if (m_rbPolarity_Inve.Checked && m_rbSync_Bypass.Checked)
                    pictureBox2.Image = Comp_v1_80.Properties.Resources.inverting;
                UpdateDrawing(prms);
            }
        }

        //Check if Power Off Radio Button is checked 
        private void m_rbPower_Off_CheckedChanged(object sender, EventArgs e)
        {
            if (m_rbPower_Off.Checked)
            {
                CyComparatorParameters prms = new CyComparatorParameters(m_Component);
                SetAParameter("Speed", "Low_Power", true);
                UpdateDrawing(prms);
            }
        }

        //Check if Power Slow Radio Button is checked 
        private void m_rbPower_Slow_CheckedChanged(object sender, EventArgs e)
        {
            if (m_rbPower_Slow.Checked)
            {
                CyComparatorParameters prms = new CyComparatorParameters(m_Component);
                SetAParameter("Speed", "Slow", true);
                UpdateDrawing(prms);
            } 
        }

        //Check if Power Fast Radio Button is checked 
        private void m_rbPower_Fast_CheckedChanged(object sender, EventArgs e)
        {
            if (m_rbPower_Fast.Checked)
            {
                CyComparatorParameters prms = new CyComparatorParameters(m_Component);
                SetAParameter("Speed", "Fast", true);
                UpdateDrawing(prms);
            }
        }

        //Check if PowerDownOverride Enable  Radio Button is checked
        private void m_rbPDOverride_Enable_CheckedChanged(object sender, EventArgs e)
        {
            if (m_rbPDOverride_Enable.Checked)
            {
                CyComparatorParameters prms = new CyComparatorParameters(m_Component);
                SetAParameter("Pd_Override", "Enable_PD_OVerride", true);
                if (groupBox3.Enabled)
                {
                    m_rbPower_Off.Checked = true;
                    m_rbPower_Fast.Enabled = false;
                    m_rbPower_Slow.Enabled = false;
                    SetAParameter("Speed", "Low_Power", false);
                }
            }
        }

        //Check if POwerDownOverride Radio Button is checked
        private void m_rbPDOverride_Disable_CheckedChanged(object sender, EventArgs e)
        {
            if (m_rbPDOverride_Disable.Checked)
            {
                CyComparatorParameters prms = new CyComparatorParameters(m_Component);
                SetAParameter("Pd_Override", "Disable_PD_Override", true);
                if (groupBox3.Enabled)
                {
                    m_rbPower_Slow.Enabled = true;
                    m_rbPower_Fast.Enabled = true;
                }
            }
        }

        //Check if Polarity Noninverting Radio Button is checked and set the image and waveform accordingly 
        private void m_rbPolarity_NonInve_CheckedChanged(object sender, EventArgs e)
        {
            if (m_rbPolarity_NonInve.Checked)
            {
                CyComparatorParameters prms = new CyComparatorParameters(m_Component);
                SetAParameter("Polarity", "Non_Inverting", true);
                if (m_rbHysteresis_Enable.Checked && m_rbSync_Norm.Checked)
                    pictureBox2.Image = Comp_v1_80.Properties.Resources.hy_ninv_sync;
                if (m_rbHysteresis_Enable.Checked && m_rbSync_Bypass.Checked)
                    pictureBox2.Image = Comp_v1_80.Properties.Resources.hysteresis;
                if (m_rbHysteresis_Disable.Checked && m_rbSync_Norm.Checked)
                    pictureBox2.Image = Comp_v1_80.Properties.Resources.sync;
                if (m_rbHysteresis_Disable.Checked && m_rbSync_Bypass.Checked)
                    pictureBox2.Image = Comp_v1_80.Properties.Resources.comp;
                UpdateDrawing(prms);
            }
        }

        //Check if Polaruty Inverting Radio Button is checked and set the image and waveform accordingly
        private void m_rbPolarity_Inve_CheckedChanged(object sender, EventArgs e)
        {
            if (m_rbPolarity_Inve.Checked)
            {
                CyComparatorParameters prms = new CyComparatorParameters(m_Component);
                SetAParameter("Polarity", "Inverting", true);
                if (m_rbHysteresis_Enable.Checked && m_rbSync_Norm.Checked)
                    pictureBox2.Image = Comp_v1_80.Properties.Resources.hy_inv_syn;
                if (m_rbHysteresis_Enable.Checked && m_rbSync_Bypass.Checked)
                    pictureBox2.Image = Comp_v1_80.Properties.Resources.hy_inv;
                if (m_rbHysteresis_Disable.Checked && m_rbSync_Norm.Checked)
                    pictureBox2.Image = Comp_v1_80.Properties.Resources.inv_sync;
                if (m_rbHysteresis_Disable.Checked && m_rbSync_Bypass.Checked)
                    pictureBox2.Image = Comp_v1_80.Properties.Resources.inverting;
                UpdateDrawing(prms);
            }
        }

        //Check if Sync Normal Radio Button is checked and set the image accordingly 
        private void m_rbSync_Norm_CheckedChanged(object sender, EventArgs e)
        {
            if (m_rbSync_Norm.Checked)
            {
                CyComparatorParameters prms = new CyComparatorParameters(m_Component);
                SetAParameter("Sync", "Normal", true);
                if (m_rbHysteresis_Enable.Checked && m_rbPolarity_NonInve.Checked)
                    pictureBox2.Image = Comp_v1_80.Properties.Resources.hy_ninv_sync;
                if (m_rbHysteresis_Enable.Checked && m_rbPolarity_Inve.Checked)
                   pictureBox2.Image = Comp_v1_80.Properties.Resources.hy_inv_syn;
                if (m_rbHysteresis_Disable.Checked && m_rbPolarity_NonInve.Checked)
                    pictureBox2.Image = Comp_v1_80.Properties.Resources.sync;
                if (m_rbHysteresis_Disable.Checked && m_rbPolarity_Inve.Checked)
                    pictureBox2.Image = Comp_v1_80.Properties.Resources.inv_sync;
            }
        }

        //Check if Sync bypass Radio Button is checked and set the image accordingly 
        private void m_rbSync_Bypass_CheckedChanged(object sender, EventArgs e)
        {
            if (m_rbSync_Bypass.Checked)
            {
                CyComparatorParameters prms = new CyComparatorParameters(m_Component);
                SetAParameter("Sync", "Bypass", true);
                if (m_rbHysteresis_Enable.Checked && m_rbPolarity_NonInve.Checked)
                    pictureBox2.Image = Comp_v1_80.Properties.Resources.hysteresis;
                if (m_rbHysteresis_Enable.Checked && m_rbPolarity_Inve.Checked)
                    pictureBox2.Image = Comp_v1_80.Properties.Resources.hy_inv;
                if (m_rbHysteresis_Disable.Checked && m_rbPolarity_NonInve.Checked)
                    pictureBox2.Image = Comp_v1_80.Properties.Resources.comp;
                if (m_rbHysteresis_Disable.Checked && m_rbPolarity_Inve.Checked)
                    pictureBox2.Image = Comp_v1_80.Properties.Resources.inverting;
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
                    MessageBox.Show(string.Format("Error Setting Parameter {0} with value {1}\n\nErrors:\n{2}", 
                                    parameter, value, errors), "Error Setting Parameter", MessageBoxButtons.OK, 
                                    MessageBoxIcon.Error);
                }
            }
        }

        private void CyComparatorControl_Load(object sender, EventArgs e)
        {
            if (m_Component != null)
            {
                UpdateFormFromParams(m_Component);
            }
        }
    }
}
