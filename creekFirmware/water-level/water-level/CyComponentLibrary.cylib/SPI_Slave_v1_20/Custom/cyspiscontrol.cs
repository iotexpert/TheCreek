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
using System.Diagnostics;
using System.Data;
using System.Text;
using System.Windows.Forms;
using CyDesigner.Extensions.Common;
using CyDesigner.Extensions.Gde;
using SPI_Slave_v1_20;

namespace SPI_Slave_v1_20
{
    public partial class CySPISControl : UserControl
    {
        public const int PB_SPISTEXT_WIDTH = 40;
        public const int PB_EXTENTS_BORDER = 5;
        public const int PB_POLYGON_WIDTH = 4;
        public const int NUMWAVEFORMS = 5;
        public ICyInstEdit_v1 m_Component = null;
        public ICyTerminalQuery_v1 m_TermQuery = null;

        const int SPISNUMBITS_MAXIMUM = 16;
        const int SPISNUMBITS_MINIMUM = 2;

        public CySPISControl(ICyInstEdit_v1 inst, ICyTerminalQuery_v1 termquery)
        {
            m_Component = inst;
            m_TermQuery = termquery;
            InitializeComponent();

            InitializeFormComponents(inst);
            UpdateFormFromParams(m_Component);
            m_numDataBits.TextChanged += new EventHandler(m_numDataBits_TextChanged);
        }

        public void InitializeFormComponents(ICyInstEdit_v1 inst)
        {
            //Set the SPIS Mode Combo Box from Enums
            IEnumerable<string> SPISModeEnums = inst.GetPossibleEnumValues("Mode");
            foreach (string str in SPISModeEnums)
            {
                m_cbspisMode.Items.Add(str);
            }

            //Set the Compare Type Combo Box from Enums
            IEnumerable<string> ShiftDirEnums = inst.GetPossibleEnumValues("ShiftDir");
            foreach (string str in ShiftDirEnums)
            {
                m_cbShiftDir.Items.Add(str);
            }

            m_numDataBits.Minimum = 2;
            m_numDataBits.Maximum = 16;
        }

        protected override void OnCreateControl()
        {
            base.OnCreateControl();
            this.ParentForm.FormClosing += new FormClosingEventHandler(ParentForm_FormClosing);
        }

        void ParentForm_FormClosing(object sender, FormClosingEventArgs e)
        {
            if (((Form)sender).DialogResult == DialogResult.Cancel)
            {
                return;
            }
            if (ep_Errors.GetError(m_numDataBits) != "")
            {
                m_numDataBits.Focus();
                e.Cancel = true;
            }
        }

        public void UpdateFormFromParams(ICyInstEdit_v1 inst)
        {
            SPISParameters prms = new SPISParameters(inst);
            //Set the SPISMode Combo Box
            IEnumerable<string> SPISModeEnums = inst.GetPossibleEnumValues("Mode");
            bool SPISModeFound = false;
            foreach (string str in SPISModeEnums)
            {
                if (!SPISModeFound)
                {
                    string paramcompare = m_Component.ResolveEnumIdToDisplay("Mode", prms.Mode.Expr);
                    if (paramcompare == str)
                    {
                        m_cbspisMode.SelectedItem = paramcompare;
                        SPISModeFound = true;
                    }
                }
            }
            if (!SPISModeFound)
            {
                m_cbspisMode.Text = prms.Mode.Expr;
            }

            m_numDataBits.Value = Convert.ToInt16(prms.NumberOfDataBits.Value);


            //Set the Shift Directions Combo Box
            IEnumerable<string> ShiftDirModeEnums = inst.GetPossibleEnumValues("ShiftDir");
            bool ShiftDirFound = false;
            foreach (string str in ShiftDirModeEnums)
            {
                if (!ShiftDirFound)
                {
                    string paramcompare = m_Component.ResolveEnumIdToDisplay("ShiftDir", prms.ShiftDir.Expr);
                    if (paramcompare == str)
                    {
                        m_cbShiftDir.SelectedItem = paramcompare;
                        ShiftDirFound = true;
                    }
                }
            }
            if (!ShiftDirFound)
            {
                m_cbShiftDir.Text = prms.Mode.Expr;
            }

            UpdateDrawing(prms);
        }

        public void UpdateDrawing(SPISParameters prms)
        {
            if ((m_pbDrawing.Width == 0) || (m_pbDrawing.Height == 0))
                return;
            Image waveform = new Bitmap(m_pbDrawing.Width,m_pbDrawing.Height);
            Graphics wfg;
            wfg = Graphics.FromImage(waveform);
            wfg.Clear(Color.White);
            SolidBrush blkbrush = new SolidBrush(Color.Black);

            
            float extentsleft = PB_EXTENTS_BORDER + PB_SPISTEXT_WIDTH;
            float extentsright = m_pbDrawing.Width - PB_EXTENTS_BORDER;
            float padding = (extentsright - extentsleft) / 70;
            float startleft = extentsleft + padding;
            float endright = extentsright - padding;
            float startright = startleft + (endright - startleft) / 2;

            //Setup the right, left and center indicators
            Pen extentspen = new Pen(blkbrush);
            extentspen.DashStyle = System.Drawing.Drawing2D.DashStyle.Dash;
            //Draw the Left Extents Line
            wfg.DrawLine(extentspen, extentsleft, PB_EXTENTS_BORDER,
                extentsleft, m_pbDrawing.Height - PB_EXTENTS_BORDER);
            //Draw the Right Extents Line
            wfg.DrawLine(extentspen, extentsright, PB_EXTENTS_BORDER,
                extentsright, m_pbDrawing.Height - PB_EXTENTS_BORDER);

            extentspen.Dispose();


            //setup and draw all of the waveforms
            int numwaveforms = NUMWAVEFORMS;
            string[] wfnames = new string[NUMWAVEFORMS];

            wfnames[0] = "SS";
            wfnames[1] = "SCLK";
            wfnames[2] = "MOSI";
            wfnames[3] = "MISO";
            wfnames[4] = "Sample";
            //wfnames[5] = "Interrupt";

            Font perfont = new Font("Arial", 10, FontStyle.Regular, GraphicsUnit.Pixel);

            //Each waveform's height is dependent upon the drawing size minus a top and bottom border 
            //and the top period waveform which is the size of two polygon widths, and an bottom ticker tape of 2 polygon widths
            float wfheight = (m_pbDrawing.Height - (2 * PB_EXTENTS_BORDER) - (4 * PB_POLYGON_WIDTH))/ numwaveforms;
            //Fill in All Waveform Names
            for (int i = 0; i < numwaveforms; i++)
            {
                PointF pt = new PointF(extentsleft - wfg.MeasureString(wfnames[i],perfont).Width - PB_EXTENTS_BORDER,
                    PB_EXTENTS_BORDER + (2 * PB_POLYGON_WIDTH) + (wfheight * i) + (wfheight / 2) -  (wfg.MeasureString(wfnames[i],perfont).Height/2));
                wfg.DrawString(wfnames[i], perfont, blkbrush, pt);
            }

            //Draw Waveforms

            int numsegments = 2 + (Convert.ToInt16(prms.NumberOfDataBits.Value) * 2) + 3;

            for (int i = 0; i < numwaveforms; i++)
            {
                float HighY = PB_EXTENTS_BORDER + (2 * PB_POLYGON_WIDTH) + (wfheight * i) + (wfheight/8);
                float LowY = PB_EXTENTS_BORDER + (2 * PB_POLYGON_WIDTH) + (wfheight * (i+1));
                float segwidth = (extentsright - extentsleft)/numsegments;
                List<float> segsx = new List<float>();
                for(int x = 0; x < numsegments; x++)
                {
                    segsx.Add(extentsleft + (x * segwidth));
                }
                SolidBrush wfbrush = new SolidBrush(Color.Blue);
                Pen wfPen = new Pen(wfbrush);
                int NumDataBits = Convert.ToInt16(prms.NumberOfDataBits.Value);
                string val = null;
                bool ShiftDir = (Convert.ToInt16(prms.ShiftDir.Value) == 0) ? false : true;
                int j = 0;
                bool mode = ((Convert.ToInt16(prms.Mode.Value) == 0) || (Convert.ToInt16(prms.Mode.Value) == 1)) ? true : false ;
                bool starthigh = ((Convert.ToInt16(prms.Mode.Value) == 0) || (Convert.ToInt16(prms.Mode.Value) == 2)) ? false : true;
                switch (wfnames[i])
                {
                    case "SS":
                        wfg.DrawLine(wfPen, segsx[0], HighY, segsx[2], HighY);
                        wfg.DrawLine(wfPen, segsx[2], HighY, segsx[2], LowY);
                        wfg.DrawLine(wfPen, segsx[2], LowY, segsx[numsegments - 2], LowY);
                        wfg.DrawLine(wfPen, segsx[numsegments - 2], LowY, segsx[numsegments - 2], HighY);
                        wfg.DrawLine(wfPen, segsx[numsegments - 2], HighY, segsx[numsegments-1], HighY);
                        break;                  
                    case "MOSI":
                    case "MISO":
                        if (mode)
                        {
                            wfg.DrawLine(wfPen, segsx[0],HighY, segsx[2] - 2, HighY);   //Draw Bus to First Transition Point
                            wfg.DrawLine(wfPen, segsx[0], LowY, segsx[2] - 2, LowY);
                            wfg.DrawLine(wfPen, segsx[2] - 2 , HighY, segsx[2] + 2, LowY); //Draw Transition
                            wfg.DrawLine(wfPen, segsx[2] - 2, LowY, segsx[2] + 2, HighY);
                            for (j = 0; j < (NumDataBits*2); )
                            {
                                wfg.DrawLine(wfPen, segsx[2 + j] + 2, HighY, segsx[2 + (j + 2)] - 2, HighY);//Draw Bus to Transition Point
                                wfg.DrawLine(wfPen, segsx[2 + j] + 2, LowY, segsx[2 + (j + 2)] - 2, LowY);
                                

                                wfg.DrawLine(wfPen, segsx[2 + (j + 2)] - 2, HighY, segsx[2 + (j + 2)] + 2, LowY); //Draw Transition
                                wfg.DrawLine(wfPen, segsx[2 + (j + 2)] - 2, LowY, segsx[2 + (j + 2)] + 2, HighY);

                                if (ShiftDir)
                                    val = String.Format("D{0}", j / 2);
                                else
                                    val = String.Format("D{0}", NumDataBits - (j/2) - 1);

                                SizeF strsize = wfg.MeasureString(val, perfont);
                                float centerx = segsx[2 + j] + segwidth;
                                wfg.DrawString(val, perfont, new SolidBrush(Color.Black),
                                                new RectangleF(centerx - (strsize.Width / 2f), HighY + ((wfheight) / 2f) - (strsize.Height / 2f), strsize.Width, strsize.Height));
                                j+=2;
                            }
                            wfg.DrawLine(wfPen, segsx[2 + j] + 2, LowY, segsx[2 + (j + 2)], LowY);//Draw Bus to Transition Point
                            wfg.DrawLine(wfPen, segsx[2 + j] + 2, HighY, segsx[2 + (j + 2)], HighY);
                        }
                        else
                        {
                            wfg.DrawLine(wfPen, segsx[0], HighY, segsx[3] - 2, HighY);   //Draw Bus to First Transition Point
                            wfg.DrawLine(wfPen, segsx[0], LowY, segsx[3] - 2, LowY);
                            wfg.DrawLine(wfPen, segsx[3] - 2, HighY, segsx[3] + 2, LowY); //Draw Transition
                            wfg.DrawLine(wfPen, segsx[3] - 2, LowY, segsx[3] + 2, HighY);
                            for (j = 0; j < (NumDataBits * 2); )
                            {
                                wfg.DrawLine(wfPen, segsx[3 + j] + 2, HighY, segsx[3 + (j + 2)] - 2, HighY);//Draw Bus to Transition Point
                                wfg.DrawLine(wfPen, segsx[3 + j] + 2, LowY, segsx[3 + (j + 2)] - 2, LowY);


                                wfg.DrawLine(wfPen, segsx[3 + (j + 2)] - 2, HighY, segsx[3 + (j + 2)] + 2, LowY); //Draw Transition
                                wfg.DrawLine(wfPen, segsx[3 + (j + 2)] - 2, LowY, segsx[3 + (j + 2)] + 2, HighY);

                                if (ShiftDir)
                                    val = String.Format("D{0}", j / 2);
                                else
                                    val = String.Format("D{0}", NumDataBits - (j / 2) - 1);

                                SizeF strsize = wfg.MeasureString(val, perfont);
                                float centerx = segsx[3 + j] + segwidth;
                                wfg.DrawString(val, perfont, new SolidBrush(Color.Black),
                                                new RectangleF(centerx - (strsize.Width / 2f), HighY + ((wfheight) / 2f) - (strsize.Height / 2f), strsize.Width, strsize.Height));
                                j += 2;
                            }
                            wfg.DrawLine(wfPen, segsx[3 + j] + 2, LowY, segsx[3 + (j + 1)], LowY);//Draw Bus to Transition Point
                            wfg.DrawLine(wfPen, segsx[3 + j] + 2, HighY, segsx[3 + (j + 1)], HighY);
                        }
                        break;
                    case "SCLK":

                        wfg.DrawLine(wfPen, segsx[0], starthigh ? HighY : LowY, segsx[3], starthigh ? HighY : LowY);
                        wfg.DrawLine(wfPen, segsx[3], starthigh ? HighY : LowY, segsx[3], starthigh ? HighY : LowY);
                        for (j = 0; j < (NumDataBits*2); )
                        {
                            wfg.DrawLine(wfPen, segsx[3 + j], starthigh ? HighY : LowY, segsx[3 + j], starthigh ? LowY : HighY);
                            wfg.DrawLine(wfPen, segsx[3 + j++], starthigh ? LowY : HighY, segsx[3 + j], starthigh ? LowY : HighY);
                            wfg.DrawLine(wfPen, segsx[3 + j], starthigh ? LowY : HighY, segsx[3 + j], starthigh ? HighY : LowY);
                            wfg.DrawLine(wfPen, segsx[3 + j++], starthigh ? HighY : LowY, segsx[3 + j], starthigh ? HighY : LowY);
                        }
                        wfg.DrawLine(wfPen, segsx[3 + j++], starthigh ? HighY : LowY, segsx[3 + j], starthigh ? HighY : LowY);
                        break;
                    case "Sample":
                        if (mode)
                        {
                            wfg.DrawLine(wfPen, segsx[0], LowY, segsx[3] - 2, LowY);    //Go to first edge 
                            for (j = 0; j < (NumDataBits * 2); )
                            {
                                wfg.DrawLine(wfPen, segsx[3 + j] - 2, LowY, segsx[3 + j] - 2, HighY);
                                wfg.DrawLine(wfPen, segsx[3 + j] - 2, HighY, segsx[3 + j] + 2, HighY);
                                wfg.DrawLine(wfPen, segsx[3 + j] + 2, HighY, segsx[3 + j] + 2, LowY);
                                wfg.DrawLine(wfPen, segsx[3 + j] + 2, LowY, segsx[3 + (j + 2)] - 2, LowY);
                                j += 2;
                            }
                            wfg.DrawLine(wfPen, segsx[3 + j] - 2, LowY, segsx[3 + (j + 1)], LowY);
                        }
                        else
                        {
                            wfg.DrawLine(wfPen, segsx[0], LowY, segsx[4] - 2, LowY);    //Go to first edge 
                            for (j = 0; j < (NumDataBits * 2); )
                            {
                                wfg.DrawLine(wfPen, segsx[4 + j] - 2, LowY, segsx[4 + j] - 2, HighY);
                                wfg.DrawLine(wfPen, segsx[4 + j] - 2, HighY, segsx[4 + j] + 2, HighY);
                                wfg.DrawLine(wfPen, segsx[4 + j] + 2, HighY, segsx[4 + j] + 2, LowY);
                                wfg.DrawLine(wfPen, segsx[4 + j] + 2, LowY, segsx[4 + (j + 2)] - 2, LowY);
                                j += 2;
                            }
                            wfg.DrawLine(wfPen, segsx[4 + j] - 2, LowY, segsx[4 + j], LowY);
                        }
                        break;
                    case "Interrupt":
                            break;
                }
            }
            wfg.Dispose();
            m_pbDrawing.Image = waveform;
        }

        #region Num Data Bits Numeric Up_Down

        private void m_numDataBits_TextChanged(object sender, EventArgs e)
        {

            if (m_numDataBits.Text != "")
            {
                CancelEventArgs ce = new CancelEventArgs();
                m_numDataBits_Validating(sender, ce);
                if (!ce.Cancel)
                {
                    SetAParameter("NumberOfDataBits", m_numDataBits.Value.ToString(), true);
                    UpdateDrawing(new SPISParameters(m_Component));
                }
            }
            else
            {
                m_numDataBits.Text = "0";
            }
        }

        private void m_numDataBits_Validating(object sender, CancelEventArgs e)
        {
            bool error = false;
            double val = GetNumericUpDownText(sender, out error);

            if ((error) || ((val > SPISNUMBITS_MAXIMUM) || (val < SPISNUMBITS_MINIMUM)))
            {
                ep_Errors.SetError(m_numDataBits, string.Format(string.Format(
                    "Number Of Data Bits must be between {0} and {1}", SPISNUMBITS_MINIMUM, SPISNUMBITS_MAXIMUM)));
                e.Cancel = true;
            }
            else
            {
                ep_Errors.SetError(m_numDataBits, "");
            }
        }

        private int GetNumericUpDownText(object numericUpDown, out bool error)
        {
            error = false;
            int val = 0;
            try
            {
                val = Convert.ToInt32(((NumericUpDown)numericUpDown).Text);
            }
            catch
            { error = true; }
            return val;
        }

        #endregion

        private void CySPISControl_SizeChanged(object sender, EventArgs e)
        {
            UpdateDrawing(new SPISParameters(m_Component));
        }

        private void m_cbspisMode_SelectedIndexChanged(object sender, EventArgs e)
        {
            string prm = m_Component.ResolveEnumDisplayToId("Mode", m_cbspisMode.Text);
            SetAParameter("Mode", prm, true);
            UpdateDrawing(new SPISParameters(m_Component));
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

        private void m_cbShiftDir_SelectedIndexChanged(object sender, EventArgs e)
        {
            string prm = m_Component.ResolveEnumDisplayToId("ShiftDir", m_cbShiftDir.Text);
            SetAParameter("ShiftDir", prm, true);
            UpdateDrawing(new SPISParameters(m_Component));
        }

        #region Control Overrided Methods

        protected override bool ProcessCmdKey(ref Message msg, Keys keyData)
        {
            if (keyData == Keys.Enter)
            {
                SendKeys.Send("{TAB}");
                return true;
            }
            else
                return base.ProcessCmdKey(ref msg, keyData);
        }

        #endregion
    }
}
