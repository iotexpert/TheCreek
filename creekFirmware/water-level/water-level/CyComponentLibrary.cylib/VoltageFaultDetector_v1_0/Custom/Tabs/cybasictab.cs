/*******************************************************************************
* Copyright 2012, Cypress Semiconductor Corporation.  All rights reserved.
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

namespace VoltageFaultDetector_v1_0
{
    public partial class CyBasicTab : CyEditingWrapperControl
    {
        private byte m_previousNumVoltages;

        public override string TabName
        {
            get { return CyCustomizer.BASIC_TAB_NAME; }
        }

        #region Constructor(s)
        public CyBasicTab(CyParameters param)
        {
            m_params = param;
            m_params.m_basicTab = this;
            m_previousNumVoltages = m_params.NumVoltages;

            InitializeComponent();

            m_errorProvider = new ErrorProvider();
            m_errorProvider.BlinkStyle = ErrorBlinkStyle.NeverBlink;

            numNumVoltages.Minimum = 0;
            numNumVoltages.Maximum = decimal.MaxValue;
            numGFLength.Minimum = 0;
            numGFLength.Maximum = decimal.MaxValue;

            numNumVoltages.TextChanged += new EventHandler(numNumVoltages_TextChanged);
            numGFLength.TextChanged += new EventHandler(numGFLength_TextChanged);

            InitComboBoxes();

            m_params.UpdateClock(m_params.m_inst, m_params.m_termQuery);
        }

        private void InitComboBoxes()
        {
            comboBoxCompareType.Items.Clear();
            comboBoxCompareType.Items.AddRange(CyEnumConverter.GetEnumDescList(typeof(CyCompareType)));
            comboBoxDACRange.Items.Clear();
            comboBoxDACRange.Items.AddRange(CyEnumConverter.GetEnumDescList(typeof(CyDACRangeType)));
            comboBoxAnalogBus.Items.Clear();
            comboBoxAnalogBus.Items.AddRange(CyEnumConverter.GetEnumDescList(typeof(CyAnalogBusType)));
        }

        private void InitComboPhysicalPlacement()
        {
            comboBoxPhysicalPlacement.Items.Clear();
            List<string> items = new List<string>();
            switch (m_params.AnalogBus)
            {
                case CyAnalogBusType.AMUXBUSR:
                    if (m_params.CompareType == CyCompareType.OV_UV)
                    {
                        items.Add(CyEnumConverter.GetEnumDesc(CyPhysicalPlacementType.Comp13));
                        comboBoxPhysicalPlacement.Enabled = false;
                    }
                    else
                    {
                        items.Add(CyEnumConverter.GetEnumDesc(CyPhysicalPlacementType.Comp1));
                        items.Add(CyEnumConverter.GetEnumDesc(CyPhysicalPlacementType.Comp3));
                        comboBoxPhysicalPlacement.Enabled = true;
                    }
                    break;
                case CyAnalogBusType.AMUXBUSL:
                    if (m_params.CompareType == CyCompareType.OV_UV)
                    {
                        items.Add(CyEnumConverter.GetEnumDesc(CyPhysicalPlacementType.Comp02));
                        comboBoxPhysicalPlacement.Enabled = false;
                    }
                    else
                    {
                        items.Add(CyEnumConverter.GetEnumDesc(CyPhysicalPlacementType.Comp0));
                        items.Add(CyEnumConverter.GetEnumDesc(CyPhysicalPlacementType.Comp2));
                        comboBoxPhysicalPlacement.Enabled = true;
                    }
                    break;
                case CyAnalogBusType.Unconstrained:
                    items.AddRange(CyEnumConverter.GetEnumDescList(typeof(CyPhysicalPlacementType)));
                    for (int i = 0; i < items.Count; i++)
                    {
                        if (m_params.CompareType == CyCompareType.OV_UV)
                        {
                            if (items[i].Contains("+") == false)
                            {
                                items.RemoveAt(i--);
                            }
                        }
                        else
                        {
                            if (items[i].Contains("+"))
                            {
                                items.RemoveAt(i--);
                            }
                        }
                    }
                    comboBoxPhysicalPlacement.Enabled = true;
                    break;
                default:
                    break;
            }
            comboBoxPhysicalPlacement.Items.AddRange(items.ToArray());
            if (comboBoxPhysicalPlacement.Items.Contains(CyEnumConverter.GetEnumDesc(m_params.PhysicalPlacement)))
            {
                comboBoxPhysicalPlacement.SelectedItem = CyEnumConverter.GetEnumDesc(m_params.PhysicalPlacement);
            }
            else
            {
                comboBoxPhysicalPlacement.SelectedIndex = 0;
            }
        }
        #endregion

        #region Assigning parameters values to controls
        public void UpdateUI()
        {
            numNumVoltages.Value = m_params.NumVoltages;
            numGFLength.Value = m_params.GFLength;
            comboBoxCompareType.SelectedItem = CyEnumConverter.GetEnumDesc(m_params.CompareType);
            comboBoxDACRange.SelectedItem = CyEnumConverter.GetEnumDesc(m_params.DACRange);
            comboBoxAnalogBus.SelectedItem = CyEnumConverter.GetEnumDesc(m_params.AnalogBus);
            checkBoxExtRef.Checked = m_params.ExternalRef;
            InitComboPhysicalPlacement();
        }

        public void UpdateTimeUnitsLabel(double? time)
        {
            lblTimeUnits.Text = time.HasValue ? "(" + Math.Round(time.Value, 3) + " us/step)" : " (unknown)";
        }
        #endregion

        #region Event Handlers
        void numNumVoltages_TextChanged(object sender, EventArgs e)
        {
            NumericUpDown currentControl = (NumericUpDown)sender;
            byte value = 0;
            try
            {
                value = byte.Parse(currentControl.Text);
                if (value < CyParamRanges.NUM_VOLTAGES_MIN || value > CyParamRanges.NUM_VOLTAGES_MAX)
                    throw new Exception();
                m_errorProvider.SetError(currentControl, string.Empty);
                m_params.NumVoltages = value;
                m_params.UpdateClock(m_params.m_inst, m_params.m_termQuery);
            }
            catch (Exception)
            {
                m_errorProvider.SetError(currentControl, string.Format(Resources.NumVoltagesError,
                    CyParamRanges.NUM_VOLTAGES_MIN, CyParamRanges.NUM_VOLTAGES_MAX));
            }

            // Create new row in Voltages table
            if (string.IsNullOrEmpty(m_errorProvider.GetError(currentControl)))
            {
                if (m_previousNumVoltages >= CyParamRanges.NUM_VOLTAGES_MIN &&
                    m_previousNumVoltages <= CyParamRanges.NUM_VOLTAGES_MAX)
                {
                    if (value > m_previousNumVoltages)
                    {
                        int missingItemsCount = value - m_params.VoltagesTable.Count;
                        for (int i = 0; i < missingItemsCount; i++)
                        {
                            m_params.VoltagesTable.Add(CyVoltagesTableRow.CreateDefaultRow(
                                i + m_params.VoltagesTable.Count + 1));
                        }
                    }

                    m_params.SetVoltagesTable();
                    m_params.m_voltagesTab.UpdateUIFromTable();

                    if (value > m_previousNumVoltages)
                    {
                        m_params.m_voltagesTab.ValidateAllTable();
                    }
                }
                m_previousNumVoltages = value;
            }
        }

        void numGFLength_TextChanged(object sender, EventArgs e)
        {
            NumericUpDown currentControl = (NumericUpDown)sender;
            byte value = 0;
            try
            {
                value = byte.Parse(currentControl.Text);
                if (value < CyParamRanges.GFLENGTH_MIN || value > CyParamRanges.GFLENGTH_MAX)
                    throw new Exception();
                m_errorProvider.SetError(currentControl, string.Empty);
                m_params.GFLength = value;
            }
            catch (Exception)
            {
                m_errorProvider.SetError(currentControl, string.Format(Resources.GFLengthError,
                    CyParamRanges.GFLENGTH_MIN, CyParamRanges.GFLENGTH_MAX));
            }
        }

        private void CyBasicTab_Load(object sender, EventArgs e)
        {
            if (m_params.IsVoltagesTableDefault)
                m_params.SetVoltagesTable();
        }

        private void comboBoxCompareType_SelectedIndexChanged(object sender, EventArgs e)
        {
            m_params.CompareType = (CyCompareType)CyEnumConverter.GetEnumValue(comboBoxCompareType.Text,
                                                                               typeof(CyCompareType));
            InitComboPhysicalPlacement();
            m_params.m_voltagesTab.SetColumnsStyle();
        }

        private void comboBoxDACRange_SelectedIndexChanged(object sender, EventArgs e)
        {
            m_params.DACRange = (CyDACRangeType)CyEnumConverter.GetEnumValue(comboBoxDACRange.Text,
                                                                              typeof(CyDACRangeType));
            m_params.m_voltagesTab.ValidateAllTable();
        }

        private void comboBoxPhysicalPlacement_SelectedIndexChanged(object sender, EventArgs e)
        {
            m_params.PhysicalPlacement = (CyPhysicalPlacementType)CyEnumConverter.GetEnumValue(
                comboBoxPhysicalPlacement.Text, typeof(CyPhysicalPlacementType));
        }

        private void comboBoxAnalogBus_SelectedIndexChanged(object sender, EventArgs e)
        {
            m_params.AnalogBus = (CyAnalogBusType)CyEnumConverter.GetEnumValue(comboBoxAnalogBus.Text,
                                                                              typeof(CyAnalogBusType));
            InitComboPhysicalPlacement();
        }

        private void checkBoxExtRef_CheckedChanged(object sender, EventArgs e)
        {
            m_params.ExternalRef = checkBoxExtRef.Checked;
            comboBoxDACRange.Enabled = !checkBoxExtRef.Checked;

            m_params.m_voltagesTab.SetColumnsStyle();
        }
        #endregion
    }
}
