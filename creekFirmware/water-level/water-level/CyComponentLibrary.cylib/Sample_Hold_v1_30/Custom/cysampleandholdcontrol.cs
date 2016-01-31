/*******************************************************************************
* Copyright 2008-2012, Cypress Semiconductor Corporation.  All rights reserved.
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
using CyDesigner.Extensions.Gde;
using CyDesigner.Extensions.Common;

namespace Sample_Hold_v1_30
{
    public partial class CySampleAndHoldControl : UserControl
    {

        public ICyInstEdit_v1 m_instEdit = null;
        public ICyTerminalQuery_v1 m_TermQuery = null;

        public CySampleAndHoldControl(ICyInstEdit_v1 inst, ICyTerminalQuery_v1 termquery)
        {
            m_instEdit = inst;
            m_TermQuery = termquery;
            InitializeComponent();

            // Sample_Hold Power
            IEnumerable<string> PowerEnums = inst.GetPossibleEnumValues(Cysampleandholdparameters.POWER);
            foreach (string str in PowerEnums)
            {
                m_cbPower.Items.Add(str);
            }

            // Sample_Hold Mode
            IEnumerable<string> SampleModeEnums = inst.GetPossibleEnumValues(Cysampleandholdparameters.SAMPLE_MODE);
            foreach (string str in SampleModeEnums)
            {
                m_cbSampleMode.Items.Add(str);
            }

            IEnumerable<string> VrefTypeEnums = inst.GetPossibleEnumValues(Cysampleandholdparameters.VREF_TYPE);
            foreach (string str in VrefTypeEnums)
            {
                m_cbVrefType.Items.Add(str);
            }


            HookAllEvents();

            if (m_instEdit != null)
            {
                UpdateFormFromParams(inst);
            }
        }

            public void UpdateFormFromParams(ICyInstEdit_v1 inst)
            {
                UnhookAllEvents();

                Cysampleandholdparameters prms = new Cysampleandholdparameters(inst);


                //Set the Sample Mode power
                string paramPower = m_instEdit.ResolveEnumIdToDisplay(Cysampleandholdparameters.POWER, 
                    prms.m_comp_Power.Expr);
                if (m_cbPower.Items.Contains(paramPower))
                {
                    m_cbPower.SelectedItem = paramPower;
                    m_cbPower.Text = prms.m_comp_Power.Expr;
                }

                // set the Sample Mode 
                string paramSampleMode = m_instEdit.ResolveEnumIdToDisplay(Cysampleandholdparameters.SAMPLE_MODE, 
                    prms.m_comp_Sample_Mode.Expr);

                if (m_cbSampleMode.Items.Contains(paramSampleMode))
                {
                    m_cbSampleMode.SelectedItem = paramSampleMode;
                    m_cbSampleMode.Text = prms.m_comp_Sample_Mode.Expr;
                }

                string paramVrefType = m_instEdit.ResolveEnumIdToDisplay(Cysampleandholdparameters.VREF_TYPE, 
                    prms.m_comp_Vref_Type.Expr);

                if (m_cbVrefType.Items.Contains(paramVrefType))
                {
                    m_cbVrefType.SelectedItem = paramVrefType;
                    m_cbVrefType.Text = prms.m_comp_Vref_Type.Expr;
                }

                // Update Sample clock edge radio buttons
                if (m_instEdit.ResolveEnumIdToDisplay(Cysampleandholdparameters.SAMPLE_CLOCK_EDGE, 
                                              prms.m_comp_Sample_Clock_Edge.Expr) == Cysampleandholdparameters.NEGATIVE)
                {
                    m_rbNegative.Checked = true;
                    m_rbPositiveAndNegative.Checked = false;
                }
                else
                {
                    m_rbNegative.Checked = false;
                    m_rbPositiveAndNegative.Checked = true;
                }

                if (m_cbSampleMode.Text == "Track and Hold")
                {
                    m_rbNegative.Checked = true;
                    m_rbNegative.Enabled = false;
                    m_rbPositiveAndNegative.Enabled = false;
                    m_cbVrefType.Enabled = false;
                }
                else if (m_cbSampleMode.Text == "Sample and Hold")
                {
                    m_rbNegative.Enabled = true;
                    m_rbPositiveAndNegative.Enabled = true;
                    m_cbVrefType.Enabled = true;
                }

                HookAllEvents();
        }

        private void HookAllEvents()
        {
            this.m_cbSampleMode.SelectedIndexChanged += new System.EventHandler(
                                                         this.m_cbSampleMode_SelectedIndexChanged);
            this.m_cbPower.SelectedIndexChanged += new System.EventHandler(this.m_cbPower_SelectedIndexChanged);
            this.m_cbVrefType.SelectedIndexChanged += new System.EventHandler(this.m_cbVrefType_SelectedIndexChanged);
            this.m_rbNegative.CheckedChanged += new System.EventHandler(this.m_rbNegative_CheckedChanged);
            this.m_rbPositiveAndNegative.CheckedChanged += new System.EventHandler(
                                                           this.m_rbPositiveAndNegative_CheckedChanged);
        }

        private void UnhookAllEvents()
        {
            this.m_cbSampleMode.SelectedIndexChanged -= new System.EventHandler(
                                                        this.m_cbSampleMode_SelectedIndexChanged);
            this.m_cbPower.SelectedIndexChanged -= new System.EventHandler(this.m_cbPower_SelectedIndexChanged);
            this.m_cbVrefType.SelectedIndexChanged -= new System.EventHandler(this.m_cbVrefType_SelectedIndexChanged);
            this.m_rbNegative.CheckedChanged -= new System.EventHandler(this.m_rbNegative_CheckedChanged);
            this.m_rbPositiveAndNegative.CheckedChanged -= new System.EventHandler(
                                                            this.m_rbPositiveAndNegative_CheckedChanged);
        }

        private void m_cbSampleMode_SelectedIndexChanged(object sender, EventArgs e)
        {
            string prm = m_instEdit.ResolveEnumDisplayToId(Cysampleandholdparameters.SAMPLE_MODE, m_cbSampleMode.Text);
            SetAParameter(Cysampleandholdparameters.SAMPLE_MODE, prm, true);
            if (m_cbSampleMode.Text == "Track and Hold")
            {
                m_rbNegative.Checked = true;
                m_rbNegative.Enabled = false;
                m_rbPositiveAndNegative.Enabled = false;
                m_cbVrefType.Enabled = false;
                
            }
            else if (m_cbSampleMode.Text == "Sample and Hold")
            {
                m_rbNegative.Enabled = true;
                m_rbPositiveAndNegative.Enabled = true;
                m_cbVrefType.Enabled = true;
            }
        }

        private void m_cbPower_SelectedIndexChanged(object sender, EventArgs e)
        {
            string prm = m_instEdit.ResolveEnumDisplayToId(Cysampleandholdparameters.POWER,
                m_cbPower.Text);
            SetAParameter(Cysampleandholdparameters.POWER, prm, true);
        }
        
        private void m_cbVrefType_SelectedIndexChanged(object sender, EventArgs e)
        {
            string prm = m_instEdit.ResolveEnumDisplayToId(Cysampleandholdparameters.VREF_TYPE,
                m_cbVrefType.Text);
            SetAParameter(Cysampleandholdparameters.VREF_TYPE, prm, true);
        }

        private void SetAParameter(string parameter, string value, bool checkFocus)
        {
            if (this.ContainsFocus || !checkFocus)
            {
                //Verify that the parameter was set correctly.
                m_instEdit.SetParamExpr(parameter, value);
                m_instEdit.CommitParamExprs();
                if (m_instEdit.GetCommittedParam(parameter).ErrorCount != 0)
                {
                    string errors = null;
                    foreach (string err in m_instEdit.GetCommittedParam(parameter).Errors)
                    {
                        errors = errors + err + "\n";
                    }
                    string errorMessage = string.Format("Error Setting Parameter {0} with value {1}\n\nErrors:\n{2}",
                        parameter, value, errors);
                }
            }
           
        }

        private void m_rbNegative_CheckedChanged(object sender, EventArgs e)
        {
            if (m_rbNegative.Checked)
            {
                SetAParameter(Cysampleandholdparameters.SAMPLE_CLOCK_EDGE, Cysampleandholdparameters.NEGATIVE, true);
            }
        }

        private void m_rbPositiveAndNegative_CheckedChanged(object sender, EventArgs e)
        {
            if (m_rbPositiveAndNegative.Checked)
            {
                SetAParameter(Cysampleandholdparameters.SAMPLE_CLOCK_EDGE, 
                              Cysampleandholdparameters.POSITIVE_AND_NEGATIVE, true);
            }
        }

        private void CySampleAndHoldControl_Load(object sender, EventArgs e)
        {
            Cysampleandholdparameters prms = new Cysampleandholdparameters(m_instEdit);
            
        }

        private void m_cbSampleMode_SelectedIndexChanged_1(object sender, EventArgs e)
        {

        }

        private void CySampleAndHoldControl_Load_1(object sender, EventArgs e)
        {

        }       

        }
}


// [] END OF FILE 