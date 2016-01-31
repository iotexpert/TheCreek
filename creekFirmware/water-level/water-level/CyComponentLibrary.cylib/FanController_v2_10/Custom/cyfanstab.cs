/*******************************************************************************
* Copyright 2008-2012, Cypress Semiconductor Corporation.  All rights reserved.
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
using FanController_v2_10;


namespace FanController_v2_10
{
    public partial class CyFansTab : CyEditingWrapperControl
    {
        private System.Windows.Forms.NumericUpDown[] m_nudMinRPMArray;
        private System.Windows.Forms.NumericUpDown[] m_nudMaxRPMArray;
        private System.Windows.Forms.NumericUpDown[] m_nudMinDutyArray;
        private System.Windows.Forms.NumericUpDown[] m_nudMaxDutyArray;
        private System.Windows.Forms.Label[] m_lblFanRowLabels;
        private System.Windows.Forms.NumericUpDown[] m_nudInitialRPMArray;
        private List<System.Windows.Forms.NumericUpDown[]> m_nodsArray;

        public bool BankMode
        {
            get { return m_prms.NumberOfBanks > 0 && m_prms.FanMode == CyFanModeType.FIRMWARE; }
        }

        public override string TabName
        {
            get { return "Fans"; }
        }


        public CyFansTab(CyParameters param)
            : base(param)
        {

            InitializeComponent();

            GenerateNods(m_pnlMinRPM, ref m_nudMinRPMArray);
            GenerateNods(m_pnlMaxRPM, ref m_nudMaxRPMArray);
            GenerateNods(m_pnlMinDuty, ref m_nudMinDutyArray);
            GenerateNods(m_pnlMaxDuty, ref m_nudMaxDutyArray);
            GenerateNods(m_pnlInitialRPM, ref m_nudInitialRPMArray);

            m_nodsArray = new List<System.Windows.Forms.NumericUpDown[]>();

            m_nodsArray.Add(m_nudMinRPMArray);
            m_nodsArray.Add(m_nudMaxRPMArray);
            m_nodsArray.Add(m_nudMinDutyArray);
            m_nodsArray.Add(m_nudMaxDutyArray);
            m_nodsArray.Add(m_nudInitialRPMArray);

            GenerateFanRowLabels();

            CyParameters.FillEnum(m_cmbPWMFreq, typeof(CyPWMFreqType));
            CyParameters.FillEnum(m_cmbPWMRes, typeof(CyPWMResType));

            m_cmbPWMFreq.SelectedIndexChanged += delegate(object sender, EventArgs e)
            {
                m_prms.FanPWMFreq = CyParameters.GetEnum<CyPWMFreqType>(m_cmbPWMFreq.Text);
            };
            m_cmbPWMRes.SelectedIndexChanged += delegate(object sender, EventArgs e)
            {
                m_prms.FanPWMRes = CyParameters.GetEnum<CyPWMResType>(m_cmbPWMRes.Text);
                UpdateMaxFansAndPWMRes();
            };
            m_cmbBankSelect.SelectedIndexChanged += delegate(object sender, EventArgs e)
            {
                byte banks = Convert.ToByte(m_cmbBankSelect.Text);
                string message = string.Empty;
                int rowerror = GetIndexLastRowWithError();

                if (banks != 0 && rowerror > banks)
                    message = String.Format(Resources.ErrorFanNumberDPTRow, rowerror);


                if (message != string.Empty)
                    ep_Errors.SetError(m_cmbBankSelect, message);
                else
                {
                    ep_Errors.SetError(m_cmbBankSelect, string.Empty);

                    m_prms.NumberOfBanks = banks;
                    DisplayActiveFans();
                }
            };
            m_nudNumFans.TextChanged += new EventHandler(m_nudNumFans_TextChanged);
            m_nudNumFans.Validating += new CancelEventHandler(m_nudNumFans_Validating);

            EventHandler motorTtpeDelegate = delegate(object sender, EventArgs e)
            {
                if (m_rbFourPole.Checked)
                    m_prms.MotorType = CyMotorType.FOUR_POLE;
                else if (m_rbSixPole.Checked)
                    m_prms.MotorType = CyMotorType.SIX_POLE;
            };

            m_rbFourPole.CheckedChanged += motorTtpeDelegate;
            m_rbSixPole.CheckedChanged += motorTtpeDelegate;



            UpdateFormFromParams();
        }
        

        #region Form Initialization

        private void GenerateNods(Panel baseCnt, ref NumericUpDown[] array)
        {
            array = new System.Windows.Forms.NumericUpDown[CyParameters.MAX_FANS];
            int xPos = CyParameters.XPOS_NUD;
            int yPos = CyParameters.YPOS_NUD;

            for (int ii = 0; ii < CyParameters.MAX_FANS; ii++)
            {
                array[ii] = new System.Windows.Forms.NumericUpDown();
                array[ii].Tag = ii;
                array[ii].Width = CyParameters.WIDTH_10K_NUD;
                array[ii].Height = CyParameters.HEIGHT_10K_NUD;
                array[ii].Minimum = 0;
                array[ii].Maximum = decimal.MaxValue;
                array[ii].Left = xPos;
                array[ii].Top = yPos;
                array[ii].TextChanged += new System.EventHandler(m_nud_TextChanged);
                array[ii].Validating += new CancelEventHandler(m_nud_Validating);
                baseCnt.Controls.Add(array[ii]);

                yPos += CyParameters.VSPC_DEF;
            }

        }

        private void GenerateFanRowLabels()
        {
            m_lblFanRowLabels = new System.Windows.Forms.Label[CyParameters.MAX_FANS];
            int xPos = CyParameters.XPOS_FAN_NUM_LBL;
            int yPos = CyParameters.YPOS_FAN_NUM_LBL;

            for (int ii = 0; ii < CyParameters.MAX_FANS; ii++)
            {
                m_lblFanRowLabels[ii] = new System.Windows.Forms.Label();
                m_lblFanRowLabels[ii].Left = xPos;
                m_lblFanRowLabels[ii].Top = yPos;
                m_lblFanRowLabels[ii].Text = (ii + 1).ToString();
                this.m_pnlNumCol.Controls.Add(m_lblFanRowLabels[ii]);
                yPos += CyParameters.VSPC_DEF;
            }
        }

        #endregion

        #region Form Updating Routines

        public void UpdateFormFromParams()
        {
            if (m_prms == null) return;

            for (int ii = 0; ii < CyParameters.MAX_FANS; ii++)
            {
                m_nudMaxRPMArray[ii].Value = m_prms.GetMaxRPM(ii);
                m_nudMinRPMArray[ii].Value = m_prms.GetMinRPM(ii);
                m_nudMaxDutyArray[ii].Value = m_prms.GetMaxDuty(ii);
                m_nudMinDutyArray[ii].Value = m_prms.GetMinDuty(ii);
                m_nudInitialRPMArray[ii].Value = m_prms.GetInitialRPM(ii);
            }

            m_nudNumFans.Value = m_prms.NumberOfFans;

            UpdateBankList();  // this will refresh the list of bank options and select 0 banks            

            m_cmbBankSelect.SelectedItem = m_prms.NumberOfBanks.ToString();
            if (m_prms.NumberOfBanks == 0)
            {
                // clear out the Bank Description label.
                m_lblBankDesc.Text = "";
            }

            //Set the PWMFreq Combo Box
            CyParameters.SetValue(m_cmbPWMFreq, m_prms.FanPWMFreq);

            //Set the PWMRes Combo Box
            CyParameters.SetValue(m_cmbPWMRes, m_prms.FanPWMRes);

            UpdateMaxFansAndPWMRes();

            DisplayActiveFans();

            m_rbFourPole.Checked = m_prms.MotorType == CyMotorType.FOUR_POLE;
            m_rbSixPole.Checked = m_prms.MotorType == CyMotorType.SIX_POLE;
        }

        public void UpdateValues()
        {
            // if the switch is to closed loop mode, we need to disable fan banking
            if (m_prms.FanMode == CyFanModeType.HARDWARE)
            {
                UpdateBankList();  // this will refresh the list of bank options and select 0 banks

                m_prms.NumberOfBanks = 0;
            }

            // this function will update the fan count limit, which is different for
            // open and closed loop mode
            UpdateMaxFansAndPWMRes();

            // the refresh function will handle displaying/hiding the bank select box
            DisplayActiveFans();

            m_cmbPWMFreq.Enabled = m_prms.ExternalClock == 0;
        }

        private void DisplayActiveFans()
        {
            Byte fan_count;

            fan_count = BankMode ? m_prms.NumberOfBanks : m_prms.NumberOfFans;

            m_lblBankDesc.Visible = BankMode;
            m_lblNumCol.Text = BankMode ? Resources.TxtBankNumber : Resources.TxtFanNumber;

            if (BankMode)
                m_lblBankDesc.Text = string.Format(Resources.TxtFanBank, m_prms.NumberOfFans / m_prms.NumberOfBanks);

            m_lblNumBanksCombo.Visible = m_prms.FanMode == CyFanModeType.FIRMWARE;
            m_cmbBankSelect.Visible = m_prms.FanMode == CyFanModeType.FIRMWARE;

            for (int fan_index = 0; fan_index < CyParameters.MAX_FANS; fan_index++)
            {
                bool c_visible = fan_index < fan_count;
                m_nudMinRPMArray[fan_index].Visible = c_visible;
                m_nudMaxRPMArray[fan_index].Visible = c_visible;
                m_nudMinDutyArray[fan_index].Visible = c_visible;
                m_nudMaxDutyArray[fan_index].Visible = c_visible;
                m_lblFanRowLabels[fan_index].Visible = c_visible;
            }


            // resize the panels to the correct size -- size of each row member plus a
            // little padding at either end
            int Height = fan_count * CyParameters.VSPC_DEF + CyParameters.YPOS_NUD;

            // m_pnlNumCol.Height = Height;
            m_pnlNumCol.Height = Height;
            m_pnlMinRPM.Height = Height;
            m_pnlMaxRPM.Height = Height;
            m_pnlMinDuty.Height = Height;
            m_pnlMaxDuty.Height = Height;
            m_pnlInitialRPM.Height = Height;
        }

        // This function figures out which bankcounts are valid based on the number of fans.
        // A system requirement is that there be an equal number of fans in each bank. This
        // function populates the m_cmbBankSelect combo box.  Upon exit, the selected index
        // will be 0, the selection for 0 banks (which is always a member of the list).
        private void UpdateBankList()
        {
            m_cmbBankSelect.Items.Clear();
            m_cmbBankSelect.Items.Add("0");
            m_cmbBankSelect.SelectedIndex = 0;

            // Banking is only valid for 2 or more fans
            if (m_prms.NumberOfFans >= 2)
            {
                for (int counti = 1; counti < m_prms.NumberOfFans; counti++)
                {
                    if ((m_prms.NumberOfFans % counti) == 0)
                    {
                        m_cmbBankSelect.Items.Add(counti.ToString());
                    }
                }
            }
        }

        #endregion


        #region event_handlers

        private void m_nudNumFans_TextChanged(object sender, EventArgs e)
        {
            CancelEventArgs ce = new CancelEventArgs();
            m_nudNumFans_Validating(sender, ce);

            if (!ce.Cancel)
            {
                m_prms.NumberOfFans = Convert.ToByte(m_nudNumFans.Value);

                // Whenever the user changes the fan count, reset the bank count to
                // zero.  Changing the fan count will always change the fan banking options
                // if only temporarily as the user incs/decs the fan count.  The only reasonable
                // thing to do is to reset the bank count and have the user re-select it once
                // the fan count has been updated.
                UpdateBankList();  // this will refresh the list of bank options and select 0 banks

                m_prms.NumberOfBanks = 0;

                DisplayActiveFans();
            }
        }

        private void m_nudNumFans_Validating(object sender, CancelEventArgs e)
        {
            bool error = false;
            int val = CyParameters.GetNumericUpDownText(m_nudNumFans, out error);
            bool invalid = false;
            string message = "";

            // check for errors
            if (m_prms.FanMode == CyFanModeType.FIRMWARE)
            {
                if (val > CyParameters.MAX_FANS)
                {
                    message = String.Format(Resources.MsgMaxFanLoop, CyParameters.MAX_FANS);
                    invalid = true;
                }
            }
            else if (m_prms.FanMode == CyFanModeType.HARDWARE)
            {
                if ((m_prms.FanPWMRes == CyPWMResType.EIGHT_BIT) && (val > CyParameters.MAX_FANS_CLOSED_8B))
                {
                    message = String.Format(Resources.MsgMaxFan8bClosedLoop, CyParameters.MAX_FANS_CLOSED_8B);
                    invalid = true;
                }
                else if ((m_prms.FanPWMRes == CyPWMResType.TEN_BIT) && (val > CyParameters.MAX_FANS_CLOSED_10B))
                {
                    message = String.Format(Resources.MsgMaxFan10bClosedLoop, CyParameters.MAX_FANS_CLOSED_10B);
                    invalid = true;
                }                
            }

            int rowerror = GetIndexLastRowWithError();
            if (rowerror > val)
            {
                message = String.Format(Resources.ErrorFanNumberDPTRow, rowerror);
                invalid = true;
            }
   

            if ((error) || (invalid))
            {
                ep_Errors.SetError(m_nudNumFans, string.Format(message));

                    e.Cancel = true;
            }
            else
            {
                ep_Errors.SetError(m_nudNumFans, string.Empty);
            }
        }
        int GetIndexLastRowWithError()
        {
            for (int ii = m_prms.NumberOfFans - 1; ii >= 0; ii--)
                foreach (NumericUpDown[] node in m_nodsArray)         
                    if (ep_Errors.GetError(node[ii]) != string.Empty)
                        return ii+1;
            return 0;
        }

        private void m_nud_TextChanged(object sender, EventArgs e)
        {
            CancelEventArgs ce = new CancelEventArgs();
            m_nud_Validating(sender, ce);
            if (!ce.Cancel)
            {
                int index = Convert.ToInt32(((System.Windows.Forms.NumericUpDown)sender).Tag);

                if ((sender as Control).Parent == m_pnlMaxDuty)
                    m_prms.SetMaxDuty(index, Convert.ToUInt16(m_nudMaxDutyArray[index].Value));
                else if ((sender as Control).Parent == m_pnlMinDuty)
                    m_prms.SetMinDuty(index, Convert.ToUInt16(m_nudMinDutyArray[index].Value));
                else if ((sender as Control).Parent == m_pnlMaxRPM)
                    m_prms.SetMaxRPM(index, Convert.ToUInt16(m_nudMaxRPMArray[index].Value));
                else if ((sender as Control).Parent == m_pnlInitialRPM)
                    m_prms.SetInitialRPM(index, Convert.ToUInt16(m_nudInitialRPMArray[index].Value));
                else if ((sender as Control).Parent == m_pnlMinRPM)
                    m_prms.SetMinRPM(index, Convert.ToUInt16(m_nudMinRPMArray[index].Value));

            }
        }

        bool m_lockRow = false;
        private void m_nud_Validating(object sender, CancelEventArgs e)
        {
            bool error = false;
            bool invalid = false;
            string message = "";
            if (sender is NumericUpDown == false) return;

            int min = 0;
            int max = 0;
            if ((sender as Control).Parent == m_pnlMinDuty || (sender as Control).Parent == m_pnlMaxDuty)
            {
                min = CyParameters.MIN_DUTY_NUD;
                max = CyParameters.MAX_DUTY_NUD;
            }
            else if ((sender as Control).Parent == m_pnlMinRPM || (sender as Control).Parent == m_pnlMaxRPM )
            {
                min = CyParameters.MIN_RPM_NUD;
                max = CyParameters.MAX_RPM_NUD;
            }
            else if ((sender as Control).Parent == m_pnlInitialRPM)
            {
                min = CyParameters.MIN_RPM_NUD;
                max = CyParameters.MAX_RPM_NUD;
            }

            int val = CyParameters.GetNumericUpDownText(sender, out error);
            int fanIndex = (int)((NumericUpDown)sender).Tag;

            if (m_lockRow == false)
            {
                CancelEventArgs ee = new CancelEventArgs();
                m_lockRow = true;


                m_nud_Validating(m_nudMinRPMArray[fanIndex], ee);
                m_nud_Validating(m_nudMaxRPMArray[fanIndex], ee);
                m_nud_Validating(m_nudMinDutyArray[fanIndex], ee);
                m_nud_Validating(m_nudMaxDutyArray[fanIndex], ee);
                m_nud_Validating(m_nudInitialRPMArray[fanIndex], ee);

                m_lockRow = false;
            }

            // check for bad values
            if ((sender as Control).Parent == m_pnlMaxDuty)
            {
                if (val <= m_nudMinDutyArray[fanIndex].Value)
                {
                    message = Resources.ErrorDutyMaxLimit;
                    invalid = true;
                }
            }
            else if ((sender as Control).Parent == m_pnlMinRPM)
            {
                if (val >= m_nudMaxRPMArray[fanIndex].Value)
                {
                    message = Resources.ErrorRPMMinLimit;
                    invalid = true;
                }
            }
            else if ((sender as Control).Parent == m_pnlMaxRPM)
            {
                if (val <= m_nudMinRPMArray[fanIndex].Value)
                {
                    message = Resources.ErrorRPMMaxLimit;
                    invalid = true;
                }
            }
            else if ((sender as Control).Parent == m_pnlMinDuty)
            {
                if (val >= m_nudMaxDutyArray[fanIndex].Value)
                {
                    message = Resources.ErrorDutyMinLimit;
                    invalid = true;
                }
            }

            if (val < min || val > max)
            {
                message = string.Format(Resources.ErrorValueLimit, min, max);
                invalid = true;
            }

            if ((error) || (invalid))
            {
                ep_Errors.SetError((NumericUpDown)sender, string.Format(message));
            }
            else
            {
                ep_Errors.SetError((NumericUpDown)sender, string.Empty);
            }
        }

        private void UpdateMaxFansAndPWMRes()
        {
            if (m_prms.FanPWMRes == CyPWMResType.TEN_BIT)
            {
                if (m_prms.FanMode == CyFanModeType.FIRMWARE)
                    m_nudNumFans.Maximum = CyParameters.MAX_FANS;
                else
                    m_nudNumFans.Maximum = CyParameters.MAX_FANS_CLOSED_10B;
                m_cmbPWMFreq.SelectedIndex = 0;
                m_cmbPWMFreq.Enabled = false;
            }
            else if (m_prms.FanPWMRes == CyPWMResType.EIGHT_BIT)
            {
                if (m_prms.FanMode == CyFanModeType.FIRMWARE)
                    m_nudNumFans.Maximum = CyParameters.MAX_FANS;
                else
                    m_nudNumFans.Maximum = CyParameters.MAX_FANS_CLOSED_8B;
                m_cmbPWMFreq.Enabled = true;
            }
        }

        #endregion
    }

}
