/*******************************************************************************
* Copyright 2008-2009, Cypress Semiconductor Corporation.  All rights reserved.
* You may use this file only in accordance with the license, terms, conditions, 
* disclaimers, and limitations in the end user license agreement accompanying 
* the software package with which this file was provided.
********************************************************************************/

using System;
using System.Collections.Generic;
using System.Text;
using System.Windows.Forms;
using CyDesigner.Extensions.Common;
using System.Drawing;

namespace BoostConv_v1_10
{
    class CyConfigurationTab : UserControl, ICyParamEditingControl
    {
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

        private string GetComboValueAsString(ComboBox combo)
        {
            string result = string.Empty;
            int selectedIndex = combo.SelectedIndex;

            if (selectedIndex != -1)
            {
                result = combo.Items[selectedIndex].ToString();
                if (result == "Off") result = "0";
            }
            else
            {
                throw new Exception(combo.Name + ".SelectedIndex == -1 ");
            }

            return result;
        }

        private string GetVInString()
        {
            return GetComboValueAsString(combo_vIn);
        }

        private string GetVOutString()
        {
            return GetComboValueAsString(combo_vOut);
        }

        CyParameters _parameters;

        public CyConfigurationTab(CyParameters parameters)
        {
            _parameters = parameters;
            InitializeComponent();
            LoadParameters();
            AdditionalInitializeComponent();
        }

        private void LoadParameters()
        {
            combo_vIn.SelectedItem = _parameters.GetInputVoltage();
            updown_outCurrent.Value = decimal.Parse(_parameters.GetOutCurrent());
            combo_vOut.SelectedIndex = _parameters.GetOutVlotageIndex();

            int freq = _parameters.GetFrequency();
            switch (freq)
            {
                case 0x00:
                    {
                        combo_Freq.SelectedIndex = 0;
                        break;
                    }
                case 0x01:
                    {
                        combo_Freq.SelectedIndex = 1;
                        break;
                    }
                case 0x02:
                    {
                        combo_Freq.SelectedIndex = 2;
                        break;
                    }
                case 0x03:
                    {
                        combo_Freq.SelectedIndex = 3;
                        break;
                    }
                default: break;
            }


            ReSelectSchema();
            ReCalc();
            CheckMaxCurrent();
        }

        #region ICyParamEditingControl Members

        public Control DisplayControl
        {
            get { return this; }
        }

        public IEnumerable<CyCustErr> GetErrors()
        {
            return new List<CyCustErr>();
        }

        #endregion

        private void AdditionalInitializeComponent()
        {
            this.Dock = DockStyle.Fill;
            this.combo_vIn.SelectedIndexChanged += new System.EventHandler(this.combo_vIn_SelectedIndexChanged);
            this.combo_vOut.SelectedIndexChanged += new System.EventHandler(this.combo_vOut_SelectedIndexChanged);
            this.updown_outCurrent.ValueChanged += new System.EventHandler(this.updown_outCurrent_ValueChanged);
        }

        private void InitializeComponent()
        {
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
            ((System.ComponentModel.ISupportInitialize)(this.updown_outCurrent)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.pictureB_schema)).BeginInit();
            this.SuspendLayout();
            // 
            // label_vBatIn
            // 
            this.label_vBatIn.AutoSize = true;
            this.label_vBatIn.Location = new System.Drawing.Point(3, 18);
            this.label_vBatIn.Name = "label_vBatIn";
            this.label_vBatIn.Size = new System.Drawing.Size(149, 17);
            this.label_vBatIn.TabIndex = 0;
            this.label_vBatIn.Text = "Vbat input voltage (V):";
            // 
            // label_vOut
            // 
            this.label_vOut.AutoSize = true;
            this.label_vOut.Location = new System.Drawing.Point(3, 53);
            this.label_vOut.Name = "label_vOut";
            this.label_vOut.Size = new System.Drawing.Size(128, 17);
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
            this.combo_vOut.Location = new System.Drawing.Point(161, 50);
            this.combo_vOut.Name = "combo_vOut";
            this.combo_vOut.Size = new System.Drawing.Size(73, 24);
            this.combo_vOut.TabIndex = 2222;
            // 
            // label_outCurrent
            // 
            this.label_outCurrent.Location = new System.Drawing.Point(3, 187);
            this.label_outCurrent.Name = "label_outCurrent";
            this.label_outCurrent.Size = new System.Drawing.Size(112, 29);
            this.label_outCurrent.TabIndex = 6;
            this.label_outCurrent.Text = "Estimated output current (mA):";
            this.label_outCurrent.Visible = false;
            // 
            // updown_outCurrent
            // 
            this.updown_outCurrent.Location = new System.Drawing.Point(116, 187);
            this.updown_outCurrent.Maximum = new decimal(new int[] {
            50,
            0,
            0,
            0});
            this.updown_outCurrent.Name = "updown_outCurrent";
            this.updown_outCurrent.Size = new System.Drawing.Size(52, 22);
            this.updown_outCurrent.TabIndex = 7;
            this.updown_outCurrent.Visible = false;
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
            this.combo_vIn.Location = new System.Drawing.Point(161, 15);
            this.combo_vIn.Name = "combo_vIn";
            this.combo_vIn.Size = new System.Drawing.Size(73, 24);
            this.combo_vIn.TabIndex = 1;
            // 
            // pictureB_schema
            // 
            this.pictureB_schema.Location = new System.Drawing.Point(240, 15);
            this.pictureB_schema.Name = "pictureB_schema";
            this.pictureB_schema.Size = new System.Drawing.Size(334, 262);
            this.pictureB_schema.SizeMode = System.Windows.Forms.PictureBoxSizeMode.StretchImage;
            this.pictureB_schema.TabIndex = 13;
            this.pictureB_schema.TabStop = false;
            // 
            // label_Freq
            // 
            this.label_Freq.AutoSize = true;
            this.label_Freq.Location = new System.Drawing.Point(3, 84);
            this.label_Freq.Name = "label_Freq";
            this.label_Freq.Size = new System.Drawing.Size(138, 17);
            this.label_Freq.TabIndex = 4;
            this.label_Freq.Text = "Switching frequency:";
            // 
            // combo_Freq
            // 
            this.combo_Freq.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.combo_Freq.FormattingEnabled = true;
            this.combo_Freq.Items.AddRange(new object[] {
            "2MHz",
            "400kHz",
            "100kHz",
            "External"});
            this.combo_Freq.Location = new System.Drawing.Point(147, 84);
            this.combo_Freq.Name = "combo_Freq";
            this.combo_Freq.Size = new System.Drawing.Size(86, 24);
            this.combo_Freq.TabIndex = 5;
            this.combo_Freq.SelectedIndexChanged += new System.EventHandler(this.combo_Freq_SelectedIndexChanged);
            // 
            // label_Current
            // 
            this.label_Current.Location = new System.Drawing.Point(3, 120);
            this.label_Current.Name = "label_Current";
            this.label_Current.Size = new System.Drawing.Size(231, 29);
            this.label_Current.TabIndex = 6;
            this.label_Current.Text = "Max output current (mA):   ";
            // 
            // ConfigurationTab
            // 
            this.AutoScroll = true;
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
            this.Name = "ConfigurationTab";
            this.Size = new System.Drawing.Size(577, 280);
            this.Resize += new System.EventHandler(this.ConfigurationTab_Resize);
            ((System.ComponentModel.ISupportInitialize)(this.updown_outCurrent)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.pictureB_schema)).EndInit();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        private void updown_outCurrent_ValueChanged(object sender, EventArgs e)
        {
            ReCalc();
            _parameters.SetOutCurrent(updown_outCurrent.Value.ToString());
        }

        private void combo_vOut_SelectedIndexChanged(object sender, EventArgs e)
        {
            CheckMaxCurrent();
            ReSelectSchema();
            ReCalc();
            _parameters.SetOutVoltage(GetVOutString());
        }

        private void CheckMaxCurrent()
        {
            double vBatIn = double.Parse(GetVInString());
            double vOut = double.Parse(GetVOutString());
            int maxCurrent = 50;

            if ((vBatIn >= 0.5) && (vBatIn <= 0.8))
            {
                maxCurrent = 20;
            }
            else if ((vBatIn > 0.8) && (vBatIn <= 1.6))
            {
                maxCurrent = 30;
            }

            updown_outCurrent.Maximum = maxCurrent;
            label_Current.Text = "Max output current (mA):   " + maxCurrent.ToString();
        }

        private void combo_vIn_SelectedIndexChanged(object sender, EventArgs e)
        {
            CheckMaxCurrent();
            ReCalc();
            _parameters.SetInputVoltage(GetVInString());
        }

        private void ReSelectSchema()
        {
            double vOut = double.Parse(GetVOutString());
            if (vOut > 3.6)
            {
                pictureB_schema.Image = global::BoostConv_v1_10.Properties.Resources.boost;
            }
            else
            {
                pictureB_schema.Image = global::BoostConv_v1_10.Properties.Resources.boost_diodeless;
            }
        }

        public void ReCalc()
        {
        }

        private void ConfigurationTab_Resize(object sender, EventArgs e)
        {
            int h, w;
            h = this.Height - 20;
            w = this.Width - pictureB_schema.Left - 10;
            if (w > h * 1.4)
            {
                pictureB_schema.Height = h;
                pictureB_schema.Width = (int)(h * 1.4);
            }
            else
            {
                pictureB_schema.Height = (int)(w / 1.4);
                pictureB_schema.Width = w;
            }
        }

        private void combo_Freq_SelectedIndexChanged(object sender, EventArgs e)
        {
            switch (combo_Freq.SelectedIndex)
            {
                case 0:
                    {
                        _parameters.SetFrequency(0x00);
                        break;
                    }
                case 1:
                    {
                        _parameters.SetFrequency(0x01);
                        break;
                    }
                case 2:
                    {
                        _parameters.SetFrequency(0x02);
                        break;
                    }
                case 3:
                    {
                        _parameters.SetFrequency(0x03);
                        break;
                    }
            }
        }
    }
}
