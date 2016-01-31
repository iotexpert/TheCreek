/*******************************************************************************
* Copyright 2008-2012, Cypress Semiconductor Corporation.  All rights reserved.
* You may use this file only in accordance with the license, terms, conditions, 
* disclaimers, and limitations in the end user license agreement accompanying 
* the software package with which this file was provided.
********************************************************************************/

using System;
using System.Collections.Generic;
using System.Text;
using System.Windows.Forms;
using CyDesigner.Extensions.Gde;
using CyDesigner.Extensions.Common;
using System.Drawing;
using BoostConv_v2_10.Properties;

namespace BoostConv_v2_10
{
    enum EditorMode
    {
        Loading,
        CustomEditing,
        ExpressionEditing
    }

    public class CyConfigurationTab : UserControl, ICyParamEditingControl
    {
        #region Private members
        const string STR_MAX_OUTPUT_CURRENT = "Max output current (mA): ";
        private CyParameters m_parameters;
        private ErrorProvider errorProvider;
        private System.ComponentModel.IContainer components;
        private CheckBox cbDisablesAutoBattery;
        private EditorMode m_mode;
        #endregion

        #region Properties
        public double InputVoltage
        {
            get { return GetComboBoxValue(combo_vIn); }
            set { combo_vIn.SelectedItem = value.ToString(); }
        }

        public byte OutVoltage
        {
            get { return (combo_vOut.SelectedIndex == 0) ? (byte)0 : (byte)(combo_vOut.SelectedIndex + 2); }
            set
            {
                int index = (value == 0) ? 0 : (int)(value - 2);
                SetComboBoxIndex(combo_vOut, index);
            }
        }
        #endregion

        #region Ctors
        public CyConfigurationTab(CyParameters parameters)
        {
            InitializeComponent();

            m_mode = EditorMode.Loading;
            m_parameters = parameters;

            Dock = DockStyle.Fill;
            combo_vOut.Items.Clear();
            combo_vOut.Items.AddRange(CyParameters.m_outputValuesRange);
            combo_vOut.Items[0] = "Off";

            UpdateParam();

            //This field will be enabled when characterization data will be obtained
            label_outCurrent.Visible = false;
            updown_outCurrent.Visible = false;

            m_mode = EditorMode.CustomEditing;

        }
        #endregion

        #region Public methods
        public void UpdateParam()
        {
            InputVoltage = m_parameters.InputVoltage;
            //updown_outCurrent.Value = m_parameters.OutCurrent;
            OutVoltage = m_parameters.OutVoltage;
            SetComboBoxIndex(combo_Freq, m_parameters.Frequency);

            SetComboBoxIndex(combo_exClockSrc, m_parameters.ExternalClockSrc);

            cbDisablesAutoBattery.Checked = m_parameters.DisableAutoBattery;

            CheckMaxCurrent();
            SelectSchema();
        }
        #endregion

        #region Private tool methods

        private double GetComboBoxValue(ComboBox combo)
        {
            double result = 0;
            bool is_parsible = false;
            if (combo.SelectedItem != null)
                is_parsible = double.TryParse(combo.SelectedItem.ToString(), out result);
            System.Diagnostics.Debug.Assert(is_parsible);
            return result;
        }
        private void SetComboBoxIndex(ComboBox combo, int index)
        {
            if (combo.Items.Count > index && index > -2)
                combo.SelectedIndex = index;
            else
                System.Diagnostics.Debug.Assert(false);
        }
        private double GetVOutValue()
        {
            //First value is not double
            if (combo_vOut.SelectedIndex == 0)
                return 0;

            double res = GetComboBoxValue(combo_vOut);
            return res;
        }

        private void CheckMaxCurrent()
        {
            double vBatIn = InputVoltage;
            double vOut = GetVOutValue();
            int maxCurrent = 50;

            if ((vBatIn >= 0.5) && (vBatIn < 0.8) && (vOut >= 1.6) && (vOut < 3.6))
            {
                maxCurrent = 15;
            }
            else if ((vBatIn >= 0.8) && (vBatIn < 1.6) && (vOut >= 3.6))
            {
                maxCurrent = 20;
            }
            else if ((vBatIn >= 0.8) && (vBatIn < 1.6) && (vOut >= 1.6) && (vOut < 3.6))
            {
                maxCurrent = 30;
            }
            else if ((vBatIn >= 1.6) && (vBatIn <= 3.6) && (vOut >= 1.6) && (vOut <= 3.6))
            {
                maxCurrent = 75;
            }
            else if ((vBatIn >= 1.6) && (vBatIn <= 3.6) && (vOut >= 3.6))
            {
                maxCurrent = 50;
            }

            updown_outCurrent.Maximum = maxCurrent;

            label_Current.Visible = (vOut <= 4.0 * vBatIn) && (vOut != 0);
            label_Current.Text = STR_MAX_OUTPUT_CURRENT + maxCurrent.ToString();
        }

        private void SelectSchema()
        {
            pictureB_schema.Image = (GetVOutValue() > 3.6) ? Resources.boost : Resources.boost_diodeless;
        }
        Icon m_iconError = Icon.FromHandle(Properties.Resources.Symbol_Error.GetHicon());
        Icon m_iconWarning = Icon.FromHandle(Properties.Resources.WarningHS.GetHicon());
        private void CheckVoltageLimitation()
        {
            double vBatIn = InputVoltage;
            double vOut = GetVOutValue();
            
            //At boost frequency of 2 MHz, VBOOST is limited to 2 x VBAT. At 400 kHz, VBOOST is limited to 4 x VBAT
            if (((CyBoostFrequency)m_parameters.Frequency == CyBoostFrequency.SWITCH_FREQ_2MHZ) &&
                (vOut > 2.0 * vBatIn))
            {
                errorProvider.SetError(combo_vIn, Properties.Resources.VoltageErrorMessage2MHz);
                errorProvider.SetError(combo_vOut, Properties.Resources.VoltageErrorMessage2MHz);
                errorProvider.SetError(combo_Freq, Properties.Resources.VoltageErrorMessage2MHz);
                this.errorProvider.Icon = m_iconError;
            }
            else if (((CyBoostFrequency)m_parameters.Frequency == CyBoostFrequency.SWITCH_FREQ_400KHZ) &&
                 (vOut > 4.0 *vBatIn))
            {
                errorProvider.SetError(combo_vIn, Properties.Resources.VoltageErrorMessage400kHz);
                errorProvider.SetError(combo_vOut, Properties.Resources.VoltageErrorMessage400kHz);
                errorProvider.SetError(combo_Freq, Properties.Resources.VoltageErrorMessage400kHz);
                this.errorProvider.Icon = m_iconError;
            }
            else if ((vBatIn >= 0.5 && vBatIn < 0.8) && vOut > 3.6)
            {
                errorProvider.SetError(combo_vIn, Properties.Resources.InputVoltageErrorMessage);
                errorProvider.SetError(combo_vOut, Properties.Resources.InputVoltageErrorMessage);
                this.errorProvider.Icon = m_iconError;
            }
            else if (vBatIn > vOut && vOut != 0)
            {
                errorProvider.SetError(combo_vIn, Properties.Resources.VoltageWarningMessage);
                errorProvider.SetError(combo_vOut, Properties.Resources.VoltageWarningMessage);
                this.errorProvider.Icon = m_iconWarning;
            }
            else
            {
                errorProvider.SetError(combo_vIn, string.Empty);
                errorProvider.SetError(combo_vOut, string.Empty);
                errorProvider.SetError(combo_Freq, string.Empty);
            }

        }
        #endregion

        #region Event Handlers
        private void updown_outCurrent_ValueChanged(object sender, EventArgs e)
        {
            if (m_mode != EditorMode.CustomEditing) return;

            m_parameters.OutCurrent = byte.Parse(updown_outCurrent.Value.ToString());
        }

        private void combo_vOut_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (m_mode != EditorMode.CustomEditing) return;

            CheckMaxCurrent();
            SelectSchema();

            m_parameters.OutVoltage = OutVoltage;
            CheckVoltageLimitation();
        }

        private void combo_vIn_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (m_mode != EditorMode.CustomEditing) return;

            CheckMaxCurrent();
            m_parameters.InputVoltage = InputVoltage;
            CheckVoltageLimitation();
        }

        private void combo_Freq_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (m_mode != EditorMode.CustomEditing) return;

            m_parameters.Frequency = combo_Freq.SelectedIndex;
            
            CheckVoltageLimitation();
        }

        private void combo_exClockSrc_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (m_mode != EditorMode.CustomEditing) return;

            m_parameters.ExternalClockSrc = combo_exClockSrc.SelectedIndex;
        }

        private void CyConfigurationTab_VisibleChanged(object sender, EventArgs e)
        {
            m_mode = Visible ? EditorMode.CustomEditing : EditorMode.ExpressionEditing;
        }

        private void cbDisablesAutoBattery_CheckedChanged(object sender, EventArgs e)
        {
            if (m_mode != EditorMode.CustomEditing) return;

            m_parameters.DisableAutoBattery = cbDisablesAutoBattery.Checked;
        }
        #endregion

        #region ICyParamEditingControl Members

        public Control DisplayControl
        {
            get { return this; }
        }

        /// <summary>
        /// Gets any errors that exist for parameters on the DisplayControl.
        /// </summary>
        /// <returns></returns>
        public IEnumerable<CyCustErr> GetErrors()
        {
            List<CyCustErr> errs = new List<CyCustErr>();

            ICyInstEdit_v1 edit = m_parameters.m_edit;

            if (edit != null)
                foreach (string paramName in edit.GetParamNames())
                {
                    CyCompDevParam param = edit.GetCommittedParam(paramName);
                    if (param.IsVisible && param.TabName == CyParameters.P_CONFIG_PARAMETERS_TAB_NAME)
                    {
                        if (param.ErrorCount > 0)
                        {
                            foreach (string errMsg in param.Errors)
                            {
                                errs.Add(new CyCustErr(errMsg));
                            }
                        }
                    }
                }

            return errs;
        }


        #endregion

        #region Component Designer generated code
        private Label label_vOut;
        private ComboBox combo_vOut;
        private Label label_outCurrent;
        private Label label_vBatIn;
        private NumericUpDown updown_outCurrent;
        private ComboBox combo_vIn;
        private Label label_Freq;
        private ComboBox combo_Freq;
        private Label label_Current;
        private PictureBox pictureB_schema;
        private Label label_exClockSrc;
        private ComboBox combo_exClockSrc;

        private void InitializeComponent()
        {
            this.components = new System.ComponentModel.Container();
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(CyConfigurationTab));
            this.label_vBatIn = new System.Windows.Forms.Label();
            this.label_vOut = new System.Windows.Forms.Label();
            this.combo_vOut = new System.Windows.Forms.ComboBox();
            this.label_outCurrent = new System.Windows.Forms.Label();
            this.updown_outCurrent = new System.Windows.Forms.NumericUpDown();
            this.combo_vIn = new System.Windows.Forms.ComboBox();
            this.pictureB_schema = new System.Windows.Forms.PictureBox();
            this.label_Freq = new System.Windows.Forms.Label();
            this.combo_Freq = new System.Windows.Forms.ComboBox();
            this.label_Current = new System.Windows.Forms.Label();
            this.label_exClockSrc = new System.Windows.Forms.Label();
            this.combo_exClockSrc = new System.Windows.Forms.ComboBox();
            this.errorProvider = new System.Windows.Forms.ErrorProvider(this.components);
            this.cbDisablesAutoBattery = new System.Windows.Forms.CheckBox();
            ((System.ComponentModel.ISupportInitialize)(this.updown_outCurrent)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.pictureB_schema)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.errorProvider)).BeginInit();
            this.SuspendLayout();
            // 
            // label_vBatIn
            // 
            this.label_vBatIn.AutoSize = true;
            this.label_vBatIn.Location = new System.Drawing.Point(3, 6);
            this.label_vBatIn.Name = "label_vBatIn";
            this.label_vBatIn.Size = new System.Drawing.Size(112, 13);
            this.label_vBatIn.TabIndex = 0;
            this.label_vBatIn.Text = "Vbat input voltage (V):";
            // 
            // label_vOut
            // 
            this.label_vOut.AutoSize = true;
            this.label_vOut.Location = new System.Drawing.Point(3, 33);
            this.label_vOut.Name = "label_vOut";
            this.label_vOut.Size = new System.Drawing.Size(96, 13);
            this.label_vOut.TabIndex = 2;
            this.label_vOut.Text = "Output voltage (V):";
            // 
            // combo_vOut
            // 
            this.combo_vOut.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.combo_vOut.FormattingEnabled = true;
            this.combo_vOut.Items.AddRange(new object[] {
            "Off",
            "1.8",
            "1.9",
            "2",
            "2.1",
            "2.2",
            "2.3",
            "2.4",
            "2.5",
            "2.6",
            "2.7",
            "2.8",
            "2.9",
            "3",
            "3.1",
            "3.2",
            "3.3",
            "3.4",
            "3.5",
            "3.6",
            "4",
            "4.25",
            "4.5",
            "4.75",
            "5",
            "5.25"});
            this.combo_vOut.Location = new System.Drawing.Point(148, 30);
            this.combo_vOut.Name = "combo_vOut";
            this.combo_vOut.Size = new System.Drawing.Size(101, 21);
            this.combo_vOut.TabIndex = 3;
            this.combo_vOut.SelectedIndexChanged += new System.EventHandler(this.combo_vOut_SelectedIndexChanged);
            // 
            // label_outCurrent
            // 
            this.label_outCurrent.Location = new System.Drawing.Point(3, 182);
            this.label_outCurrent.Name = "label_outCurrent";
            this.label_outCurrent.Size = new System.Drawing.Size(138, 41);
            this.label_outCurrent.TabIndex = 6;
            this.label_outCurrent.Text = "Estimated output current (mA):";
            // 
            // updown_outCurrent
            // 
            this.updown_outCurrent.Location = new System.Drawing.Point(148, 180);
            this.updown_outCurrent.Maximum = new decimal(new int[] {
            50,
            0,
            0,
            0});
            this.updown_outCurrent.Name = "updown_outCurrent";
            this.updown_outCurrent.Size = new System.Drawing.Size(101, 20);
            this.updown_outCurrent.TabIndex = 7;
            this.updown_outCurrent.ValueChanged += new System.EventHandler(this.updown_outCurrent_ValueChanged);
            // 
            // combo_vIn
            // 
            this.combo_vIn.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.combo_vIn.FormattingEnabled = true;
            this.combo_vIn.Items.AddRange(new object[] {
            "0.5",
            "0.6",
            "0.7",
            "0.8",
            "0.9",
            "1",
            "1.1",
            "1.2",
            "1.3",
            "1.4",
            "1.5",
            "1.6",
            "1.7",
            "1.8",
            "1.9",
            "2",
            "2.1",
            "2.2",
            "2.3",
            "2.4",
            "2.5",
            "2.6",
            "2.7",
            "2.8",
            "2.9",
            "3",
            "3.1",
            "3.2",
            "3.3",
            "3.4",
            "3.5",
            "3.6",
            "3.7",
            "3.8",
            "3.9",
            "4",
            "4.1",
            "4.2",
            "4.3",
            "4.4",
            "4.5",
            "4.6",
            "4.7",
            "4.8",
            "4.9",
            "5",
            "5.1",
            "5.2",
            "5.3",
            "5.4",
            "5.5"});
            this.combo_vIn.Location = new System.Drawing.Point(148, 3);
            this.combo_vIn.Name = "combo_vIn";
            this.combo_vIn.Size = new System.Drawing.Size(101, 21);
            this.combo_vIn.TabIndex = 1;
            this.combo_vIn.SelectedIndexChanged += new System.EventHandler(this.combo_vIn_SelectedIndexChanged);
            // 
            // pictureB_schema
            // 
            this.pictureB_schema.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom)
                        | System.Windows.Forms.AnchorStyles.Left)
                        | System.Windows.Forms.AnchorStyles.Right)));
            this.pictureB_schema.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.pictureB_schema.Location = new System.Drawing.Point(269, 15);
            this.pictureB_schema.Name = "pictureB_schema";
            this.pictureB_schema.Size = new System.Drawing.Size(305, 245);
            this.pictureB_schema.SizeMode = System.Windows.Forms.PictureBoxSizeMode.StretchImage;
            this.pictureB_schema.TabIndex = 13;
            this.pictureB_schema.TabStop = false;
            // 
            // label_Freq
            // 
            this.label_Freq.AutoSize = true;
            this.label_Freq.Location = new System.Drawing.Point(3, 60);
            this.label_Freq.Name = "label_Freq";
            this.label_Freq.Size = new System.Drawing.Size(106, 13);
            this.label_Freq.TabIndex = 4;
            this.label_Freq.Text = "Switching frequency:";
            // 
            // combo_Freq
            // 
            this.combo_Freq.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.combo_Freq.FormattingEnabled = true;
            this.combo_Freq.Items.AddRange(new object[] {
            "2 MHz",
            "400 kHz",
            "100 kHz",
            "External 32 kHz"});
            this.combo_Freq.Location = new System.Drawing.Point(148, 57);
            this.combo_Freq.Name = "combo_Freq";
            this.combo_Freq.Size = new System.Drawing.Size(101, 21);
            this.combo_Freq.TabIndex = 5;
            this.combo_Freq.SelectedIndexChanged += new System.EventHandler(this.combo_Freq_SelectedIndexChanged);
            // 
            // label_Current
            // 
            this.label_Current.AutoSize = true;
            this.label_Current.Location = new System.Drawing.Point(3, 117);
            this.label_Current.Name = "label_Current";
            this.label_Current.Size = new System.Drawing.Size(132, 13);
            this.label_Current.TabIndex = 6;
            this.label_Current.Text = "Max output current (mA):   ";
            // 
            // label_exClockSrc
            // 
            this.label_exClockSrc.AutoSize = true;
            this.label_exClockSrc.Location = new System.Drawing.Point(3, 87);
            this.label_exClockSrc.Name = "label_exClockSrc";
            this.label_exClockSrc.Size = new System.Drawing.Size(112, 13);
            this.label_exClockSrc.TabIndex = 2224;
            this.label_exClockSrc.Text = "External clock source:";
            // 
            // combo_exClockSrc
            // 
            this.combo_exClockSrc.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.combo_exClockSrc.FormattingEnabled = true;
            this.combo_exClockSrc.Items.AddRange(new object[] {
            "None",
            "ECO 32kHz",
            "ILO 32kHz"});
            this.combo_exClockSrc.Location = new System.Drawing.Point(148, 84);
            this.combo_exClockSrc.Name = "combo_exClockSrc";
            this.combo_exClockSrc.Size = new System.Drawing.Size(101, 21);
            this.combo_exClockSrc.TabIndex = 7;
            this.combo_exClockSrc.SelectedIndexChanged += new System.EventHandler(this.combo_exClockSrc_SelectedIndexChanged);
            // 
            // errorProvider
            // 
            this.errorProvider.BlinkStyle = System.Windows.Forms.ErrorBlinkStyle.NeverBlink;
            this.errorProvider.ContainerControl = this;
            this.errorProvider.Icon = ((System.Drawing.Icon)(resources.GetObject("errorProvider.Icon")));
            // 
            // cbDisablesAutoBattery
            // 
            this.cbDisablesAutoBattery.Location = new System.Drawing.Point(6, 138);
            this.cbDisablesAutoBattery.Name = "cbDisablesAutoBattery";
            this.cbDisablesAutoBattery.Size = new System.Drawing.Size(243, 36);
            this.cbDisablesAutoBattery.TabIndex = 30;
            this.cbDisablesAutoBattery.Text = "Disable auto battery connect to output when Vin = Vsel";
            this.cbDisablesAutoBattery.UseVisualStyleBackColor = true;
            this.cbDisablesAutoBattery.CheckedChanged += new System.EventHandler(this.cbDisablesAutoBattery_CheckedChanged);
            // 
            // CyConfigurationTab
            // 
            this.AutoScroll = true;
            this.Controls.Add(this.cbDisablesAutoBattery);
            this.Controls.Add(this.combo_exClockSrc);
            this.Controls.Add(this.label_exClockSrc);
            this.Controls.Add(this.pictureB_schema);
            this.Controls.Add(this.combo_vIn);
            this.Controls.Add(this.updown_outCurrent);
            this.Controls.Add(this.label_Current);
            this.Controls.Add(this.label_outCurrent);
            this.Controls.Add(this.combo_Freq);
            this.Controls.Add(this.label_Freq);
            this.Controls.Add(this.combo_vOut);
            this.Controls.Add(this.label_vOut);
            this.Controls.Add(this.label_vBatIn);
            this.Name = "CyConfigurationTab";
            this.Size = new System.Drawing.Size(577, 280);
            this.VisibleChanged += new System.EventHandler(this.CyConfigurationTab_VisibleChanged);
            ((System.ComponentModel.ISupportInitialize)(this.updown_outCurrent)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.pictureB_schema)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.errorProvider)).EndInit();
            this.ResumeLayout(false);
            this.PerformLayout();

        }
        #endregion
    }
}
