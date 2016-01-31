/*******************************************************************************
* Copyright 2008-2010, Cypress Semiconductor Corporation.  All rights reserved.
* You may use this file only in accordance with the license, terms, conditions, 
* disclaimers, and limitations in the end user license agreement accompanying 
* the software package with which this file was provided.
********************************************************************************/


using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Text;
using System.Windows.Forms;
using CyDesigner.Extensions.Common;
using CyDesigner.Extensions.Gde;
using PGA_v1_60;
using System.Diagnostics;

namespace PGA_v1_60
{
    public partial class CyPgaControl : UserControl
    {
        public ICyInstEdit_v1 m_Component = null;

        private const string MINIMUM_POWER = "Minimum Power";
        private const string LOW_POWER = "Low Power";
        private const string MEDIUM_POWER = "Medium Power";
        private const string HIGH_POWER = "High Power";
        private const int MINIMUM_POWER_GBW = 1000;
        private const int LOW_POWER_GBW = 2000;
        private const int MEDIUM_POWER_GBW = 4000;
        private const int HIGH_POWER_GBW = 8000;

		
		public CyPgaControl(ICyInstEdit_v1 inst)
        {
            m_Component = inst;
            InitializeComponent();
            
			// Sets the PGA Gain combo box from Enums
            IEnumerable<string> GainEnums = inst.GetPossibleEnumValues(PgaParameters.PGA_GAIN);
            foreach (string str in GainEnums)
            {
                m_cbGain.Items.Add(str);
            }
		
		    // Sets the PGA Power combo box from Enums
			IEnumerable<string> PowerEnums = inst.GetPossibleEnumValues(PgaParameters.PGA_POWER);
            foreach (string str in PowerEnums)
            {
                m_cbPower.Items.Add(str);
            }
			
			// Sets the PGA Vref combo box from Enums
			IEnumerable<string> VrefInputEnums = inst.GetPossibleEnumValues(PgaParameters.PGA_VREF_INPUT);
            foreach (string str in VrefInputEnums)
            {
                 m_cbVrefInput.Items.Add(str);
            }

            HookAllEvents();
            if (m_Component != null)
            {
                UpdateFormFromParams(m_Component);
            }
        }
			
			
            private void HookAllEvents()
            {
			     this.m_cbGain.SelectedIndexChanged += new System.EventHandler(this.m_cbGain_SelectedIndexChanged);
                 this.m_cbPower.SelectedIndexChanged += new System.EventHandler(this.m_cbPower_SelectedIndexChanged);
	 			 this.m_cbVrefInput.SelectedIndexChanged += new System.EventHandler(this.m_cbVrefInput_SelectedIndexChanged);
			}
			
			private void UnhookAllEvents()
			{
			     this.m_cbGain.SelectedIndexChanged -= new System.EventHandler(this.m_cbGain_SelectedIndexChanged);
                 this.m_cbPower.SelectedIndexChanged -= new System.EventHandler(this.m_cbPower_SelectedIndexChanged);
	 			 this.m_cbVrefInput.SelectedIndexChanged -= new System.EventHandler(this.m_cbVrefInput_SelectedIndexChanged);
			
			}

			     
        

    
		
	    public void UpdateFormFromParams(ICyInstEdit_v1 inst)
        {
		    UnhookAllEvents();
			PgaParameters prms = new PgaParameters(inst);

			string paramGain = m_Component.ResolveEnumIdToDisplay(PgaParameters.PGA_GAIN, 
                prms.Pga_Gain.Expr);
            if (m_cbGain.Items.Contains(paramGain))
            {
                m_cbGain.SelectedItem = paramGain;
                m_cbGain.Text = prms.Pga_Gain.Expr;
            }
			
			
			string paramPower = m_Component.ResolveEnumIdToDisplay(PgaParameters.PGA_POWER, 
               prms.Pga_Power.Expr);
            if (m_cbPower.Items.Contains(paramPower))
            {
                m_cbPower.SelectedItem = paramPower;
                m_cbPower.Text = prms.Pga_Power.Expr;
            }
			
			string paramVrefInput = m_Component.ResolveEnumIdToDisplay(PgaParameters.PGA_VREF_INPUT, 
               prms.Pga_Vref_Input.Expr);
            if (m_cbVrefInput.Items.Contains(paramVrefInput))
            {
                m_cbVrefInput.SelectedItem = paramVrefInput;
                m_cbVrefInput.Text = prms.Pga_Vref_Input.Expr;
            }

            HookAllEvents();
            UpdateGraph();
		}

        private void m_cbGain_SelectedIndexChanged(object sender, EventArgs e)
        {
		    string prm = m_Component.ResolveEnumDisplayToId(PgaParameters.PGA_GAIN, 
                m_cbGain.Text);
            SetAParameter(PgaParameters.PGA_GAIN, prm, true);
            UpdateGraph();
        }


        private void m_cbPower_SelectedIndexChanged(object sender, EventArgs e)
        {
		    string prm = m_Component.ResolveEnumDisplayToId(PgaParameters.PGA_POWER,
                m_cbPower.Text);
            SetAParameter(PgaParameters.PGA_POWER, prm, true);
            UpdateGraph();
        }

        private void m_cbVrefInput_SelectedIndexChanged(object sender, EventArgs e)
        {
		    string prm = m_Component.ResolveEnumDisplayToId(PgaParameters.PGA_VREF_INPUT,
                m_cbVrefInput.Text);
            SetAParameter(PgaParameters.PGA_VREF_INPUT, prm, true);		
 
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


        //Update the graph with the latest values.
        private void UpdateGraph()
        {
            /*
             * GBW is dependent on power.
             * Power = High, GBW = 8 MHz
             * Power = Medium, GBW = 4 MHz
             * Power = Low, GBW = 2 MHz
             * Power = TIA (or whatever minimum is called), GBW = 1 MHz
             * Numbers are approximate until PE is done.
             * 'G' is the value that the user selects.
             * */
            double GBW = 8000;
            decimal MAX_XAXIS = 10000000;
            decimal MIN_XAXIS = 1000;
            double STEP_SIZE = 10;
            int nFreqSamples = (int) (1 + STEP_SIZE * (Math.Log10((double)MAX_XAXIS) - Math.Log10((double)MIN_XAXIS)));
            double val, maxval, minval;
            int gain = 1;

            int width = TD_pictureBox.Width;
            int height = TD_pictureBox.Height;
            double[] y2 = new double[width];
            double[] x2 = new double[width];

            Bitmap b = new Bitmap(width, height);
            Bitmap TD_Waveform = new Bitmap(width, height);
            Graphics TD_wfg;
            TD_wfg = Graphics.FromImage(TD_Waveform);
            TD_wfg.Clear(Color.White);


            Int32.TryParse(m_cbGain.Text, out gain);
            switch (m_cbPower.Text)
            {
                case HIGH_POWER:
                    GBW = HIGH_POWER_GBW;
                    break;

                case LOW_POWER:
                    GBW = LOW_POWER_GBW;
                    break;

                case MEDIUM_POWER:
                    GBW = MEDIUM_POWER_GBW;
                    break;

                case MINIMUM_POWER:
                    GBW = MINIMUM_POWER_GBW;
                    break;

                default:
                    Debug.Fail(m_cbPower.Text);
                    break;
            }
            
            maxval = minval = 0;
            x2[0] = 1;
            double xPower = Math.Pow(10, 1/STEP_SIZE);
            for (int i = 1; i < nFreqSamples; i++)
            {
                x2[i] = x2[i-1] * xPower;
            }
            for (int i = 0; i < nFreqSamples; i++)
            {
                double dProd = (gain * x2[i])/GBW;
                double temp = gain / Math.Sqrt(1 + dProd * dProd);
                y2[i] = val = (20 * Math.Log(temp, 10));

                if (minval < val || i == 0)
                {
                    minval = val;
                }
            }
            
            minval = 10 * Math.Ceiling((minval + 1) / 10);
			if (minval < 10)
			{
				minval = 10;
			}
            maxval = minval - 30;
            int oldT = 0;
            int i2 = 0;
            double yAxisScale = (height / (minval - maxval));
            double offset = (minval * yAxisScale);
            for (int i = 0; i < nFreqSamples; i++)
            {
                int t = (int)((-1)*(y2[i] * yAxisScale) + offset - 1);

                if (t < height)
                {
                    TD_Waveform.SetPixel(width*i/nFreqSamples, t, Color.Black);
                }
                if (i >0 )
                {
                    i2 = 0;
                    //Smooth the curve.
                    for (int i1 = width * (i - 1) / nFreqSamples; i1 < width * i / nFreqSamples; i1++, i2++)
                    {
                        try
                        {
                            if ((oldT + ((oldT - t) * width / nFreqSamples * i2)) < height)
                            {
                                TD_Waveform.SetPixel(i1, oldT - (((oldT - t) * nFreqSamples * i2)/ width ), Color.Black);
                            }
                        }
                        catch (Exception )
                        {
                            break;
                        }
                        
                    }
                }
                oldT = t;
            }
            label_yMin.Text = maxval.ToString("F1");
            label_yMax1.Text = minval.ToString("F1");
            label_yMax2.Text = (minval - 10).ToString("F1");
            label_yMax3.Text = (minval - 20).ToString("F1");

            TD_pictureBox.Image = TD_Waveform;
        }

    }
}

