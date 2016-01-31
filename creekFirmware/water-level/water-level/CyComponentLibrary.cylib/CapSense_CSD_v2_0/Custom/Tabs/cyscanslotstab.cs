/*******************************************************************************
* Copyright 2008-2010, Cypress Semiconductor Corporation.  All rights reserved.
* You may use this file only in accordance with the license, terms, conditions,
* disclaimers, and limitations in the end user license agreement accompanying
* the software package with which this file was provided.
********************************************************************************/



using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Text;
using System.Windows.Forms;
using CyDesigner.Extensions.Gde;

namespace CapSense_CSD_v2_0
{
    public partial class CyScanSlotsTab : CyCSParamEditTemplate
    {
        #region Header
        CySSCSDProperties m_properties = null;

        static Color ROW_COLOR1 = Color.Moccasin;
        static Color ROW_COLOR2 = Color.LightGray;
        static string STR_SCAN_TIME_EXT = " mS";

        CyScanSlot m_nullScanSlot = new CyScanSlot();
        bool m_singleChannel = true;
        bool m_editMode = false;

        public override string TabName
        {
            get { return CyCsConst.P_SCAN_ORDER_TAB_NAME; }
        }

        public CyScanSlotsTab(CyCSParameters packParams)
            : base()
        {
            InitializeComponent();
            this.m_packParams = packParams;

            m_properties = new CySSCSDProperties();
            m_properties.Dock = DockStyle.Fill;
            panelConteiner.Controls.Add(m_properties);

            //Set SaveChanges Events
            dgScanSlots.m_actSaveChanges += new EventHandler(m_packParams.SetCommitParams);
            m_properties.m_actSaveChanges += new EventHandler(m_packParams.SetCommitParams);
            m_properties.GetProperties(m_packParams);

            packParams.m_settings.m_configurationChanged += new EventHandler(UpdateConfiguration);
            packParams.m_updateAll += new EventHandler(UpdateConfiguration);
            m_editMode = true;
        }

        void UpdateConfiguration(object sender, EventArgs e)
        {
            m_singleChannel = m_packParams.m_settings.Configuration == CyChannelConfig.ONE_CHANNEL;
            tsbMoveToChFirst.Visible = m_singleChannel == false;
            tsbMoveToChSecond.Visible = m_singleChannel == false;
            tsbLastSeparetor.Visible = m_singleChannel == false;
            scMain.Panel2Collapsed = m_packParams.m_settings.IsIdacInSystem() == false;
            colCh0.AutoSizeMode = m_singleChannel ? DataGridViewAutoSizeColumnMode.Fill :
                DataGridViewAutoSizeColumnMode.NotSet;
            if (m_singleChannel == false)
            {
                colCh0.Width = (dgScanSlots.Width - colIndex.Width) / 2 - 15;
                colCh1.Width = (dgScanSlots.Width - colIndex.Width) / 2 - 15;
            }


            //CheckTerminalsChannel
            if (m_packParams != null && m_singleChannel == false)
                m_packParams.m_widgets.m_scanSlots.CheckTerminalsChannel();

            UpdateScanSlotsLists();           
        }

        #endregion

        #region Actions
        private void dgScanSlots_KeyDown(object sender, KeyEventArgs e)
        {
            if ((e.KeyData == Keys.Delete) || (e.KeyData == Keys.Back))
            {
                List<CyScanSlot> list = GetSelectedSS(false);
                if (list.Count > 0)
                    dgScanSlots.EraseTerminalList(list[0]);
            }
            else if (e.KeyData == Keys.Add)
                PromoteRow(true);
            else if (e.KeyData == Keys.Subtract)
                PromoteRow(false);
        }
        private void toolStripPromoteDemote_Click(object sender, EventArgs e)
        {
            PromoteRow(sender == toolStripDemote);
        }
        private void dgScanSlots_CellEnter(object sender, DataGridViewCellEventArgs e)
        {
            if (m_packParams != null && m_editMode)
                AssignSSProperties();
        }
        private void dgScanSlots_RowsRemoved(object sender, DataGridViewRowsRemovedEventArgs e)
        {
            if (dgScanSlots.RowCount == 0)
                AssignSSProperties();
        }
        private void dgScanSlots_MouseDown(object sender, System.Windows.Forms.MouseEventArgs e)
        {
            if (m_packParams != null && m_editMode)
            {
                CyChannelNumber channel = CyChannelNumber.First;
                if (dgScanSlots.HitTest(e.X, e.Y).ColumnIndex == colCh1.Index) channel = CyChannelNumber.Second;
                List<CyTerminal> list = m_packParams.m_settings.Configuration == CyChannelConfig.ONE_CHANNEL ?
                    m_packParams.m_widgets.m_listTerminals :
                    m_packParams.m_widgets.GetListTerminals(channel);
                dgScanSlots.MouseDownFunction(sender, e, list);
            }
        }
        private void tsbMoveToChannel_Click(object sender, EventArgs e)
        {
            List<CyScanSlot> list = GetSelectedSS(false);
            for (int i = 0; i < list.Count; i++)
                if (list[i].m_widget != null)
                {
                    list[i].m_widget.m_channel = tsbMoveToChFirst == sender ? CyChannelNumber.First : 
                        CyChannelNumber.Second;
                }
            if (m_packParams != null)
            {
                //CheckTerminalsChannel
                if (m_singleChannel == false)
                    m_packParams.m_widgets.m_scanSlots.CheckTerminalsChannel();
                UpdateScanSlotsLists();
                m_packParams.SetCommitParams(null, null);
            }


        }
        #endregion

        #region Addiotional Function
        private void PromoteRow(bool down)
        {
            if (m_packParams != null)
            {
                //Use only first value
                List<CyScanSlot> list = GetSelectedSS(false);
                if (list.Count == 0) return;
                CyScanSlot ss = list[0];

                //Promoting process
                m_packParams.m_widgets.m_scanSlots.PromoteWidget(ss, m_singleChannel, down);

                UpdateScanSlotsLists();

                //Commit Parameters
                m_packParams.SetCommitParams(null, null);
            }
        }
        public void AssignSSProperties()
        {
            List<CyScanSlot> list = GetSelectedSS(true);
            m_properties.ClearSSProperties();
            double val;
            if (list != null)
            {
                bool filled = list.Count != 0;
                bool has_top = true;
                bool has_botom = true;
                if (filled)
                {
                    CyScanSlot ss;
                    //Set Properties
                    for (int i = 0; i < list.Count; i++)
                    {
                        ss = list[i];
                        m_properties.AddSSProperties(ss);
                    }
                    m_properties.CombineSSProperties();

                    List<CyScanSlot> listSS = m_packParams.m_widgets.m_scanSlots.m_listScanSlots;
                    has_top = listSS.Count > 0 ? list.Contains(listSS[0]) : true;
                    has_botom = listSS.Count > 0 ? list.Contains(listSS[listSS.Count - 1]) : true;
                }
                m_properties.Visible = filled;
                toolStripPromote.Enabled = filled && !has_top;
                toolStripDemote.Enabled = filled && !has_botom;
                tsbMoveToChFirst.Enabled = filled ? list[0].Channel == CyChannelNumber.Second : false;
                tsbMoveToChSecond.Enabled = filled ? list[0].Channel == CyChannelNumber.First : false;
                
                if(list.Count==1)
                {
                    val= CalculateScanTime(list[0]);
                    tslSensorScanTime.Text = val != -1
                        ? Math.Round(val, 3).ToString() + STR_SCAN_TIME_EXT
                        : CyCsResource.MESSAGE_CLOCK_VALUE_UNKNOW;
                }
                else tslSensorScanTime.Text = string.Empty;
            }
            val = CalculateScanTime();
            tslTotalScanTime.Text = val != -1 ? Math.Round(val, 3).ToString() + STR_SCAN_TIME_EXT
                : CyCsResource.MESSAGE_CLOCK_VALUE_UNKNOW;
        }
        public void UpdateClock(ICyInstQuery_v1 query, ICyTerminalQuery_v1 termQuery)
        {
            AssignSSProperties();
        }

        double CalculateScanTime()
        {
            double res = 0;
            double val;
            if (m_packParams != null)
            {
                List<CyScanSlot> listSS;
                if (m_packParams.m_settings.Configuration == CyChannelConfig.ONE_CHANNEL)
                    listSS = m_packParams.m_widgets.m_scanSlots.m_listScanSlots;
                else
                {
                    listSS = m_packParams.m_widgets.m_scanSlots.GetSSList(CyChannelNumber.First);
                    List<CyScanSlot> listSSSec = m_packParams.m_widgets.m_scanSlots.GetSSList(CyChannelNumber.Second);
                    if (listSSSec.Count > listSS.Count)
                        listSS = listSSSec;
                }

                for (int i = 0; i < listSS.Count; i++)
                {
                    val = CalculateScanTime(listSS[i]);
                    if (val == -1) return val;
                    res += val;
                }
                //Add Guard Sensor
                if (m_packParams.m_settings.m_guardSensorEnable)
                {
                    val = CalculateScanTime(m_packParams.m_widgets.m_scanSlots.m_guardSensor);
                    if (val == -1) return val;
                    res += val;
                }
            }
            return res;
        }
        double CalculateScanTime(CyScanSlot ss)
        {            
            if (m_packParams == null) return -1;

            double res = 0;
            double clockFr = m_packParams.GetClockFrequency();
            if (clockFr == -1) return clockFr;

            CyTuningProperties p;
            if (m_packParams.m_settings.Configuration == CyChannelConfig.ONE_CHANNEL
                || ss == m_packParams.m_widgets.m_scanSlots.m_guardSensor)
                p = m_packParams.m_widgets.GetWidgetsProperties(ss.m_widget);
            else
            {
                p = (CyTuningProperties)ss.m_widget.GetAdditionalProperties()[0];

                int index = m_packParams.m_widgets.m_scanSlots.GetSSList(ss.m_widget.m_channel).IndexOf(ss);
                CyChannelNumber othereCh = ss.m_widget.m_channel == CyChannelNumber.First
                    ? CyChannelNumber.Second : CyChannelNumber.First;
                List<CyScanSlot> listSS = m_packParams.m_widgets.m_scanSlots.GetSSList(othereCh);
                if (listSS.Count > index)
                {
                    CyTuningProperties p1 = m_packParams.m_widgets.GetWidgetsProperties(listSS[index].m_widget);
                    if (p1.m_scanResolution > p.ScanResolution)
                        p = p1;
                }

            }
            int resolution = CyCsConst.GetResolutionBitsValue(p.m_scanResolution);
            res = Math.Pow(2, resolution) * ((int)m_packParams.m_settings.ScanSpeed + 1);
            res /= clockFr;
            res += 40;//setup Time + pre-processing Time
            res /= 1000;//mS            
            return res;
        }

        #endregion

        #region Update DG Rows
        void UpdateScanSlotsLists()
        {
            if (m_packParams != null)
            {
                m_editMode = false;

                List<CyScanSlot> listSS = m_packParams.m_widgets.m_scanSlots.m_listScanSlots;

                List<CyScanSlot> list = GetSelectedSS(false);
                CyScanSlot curSS = list.Count == 0 ? null : list[0];

                dgScanSlots.SuspendLayout();
                dgScanSlots.Rows.Clear();
                colCh1.Visible = m_singleChannel == false;

                int iFirst = 0;
                int iSecond = 0;
                int i = 0;
                while (i <= listSS.Count)
                {
                    CyScanSlot ss;
                    if (i < listSS.Count)
                        ss = listSS[i];
                    else
                    {
                        if (m_packParams.m_settings.m_guardSensorEnable == false) break;
                        ss = m_packParams.m_widgets.m_scanSlots.m_guardSensor;
                        m_packParams.m_widgets.AssignGuardChannel();
                    }

                    int rowIndex = m_singleChannel || ss.Channel == CyChannelNumber.First ? iFirst++ : iSecond++;
                    int colIndex = m_singleChannel || ss.Channel == CyChannelNumber.First ? colCh0.Index : colCh1.Index;

                    if (dgScanSlots.RowCount > rowIndex)
                        dgScanSlots[colIndex, rowIndex].Value = ss;
                    else
                    {
                        object[] row = new object[] { dgScanSlots.RowCount, m_nullScanSlot, m_nullScanSlot };
                        row[colIndex] = ss;

                        dgScanSlots.Rows.Add(row);
                    }
                    i++;
                }
                UpdateDGRowsColor(colCh0.Index);
                UpdateDGRowsColor(colCh1.Index);

                m_editMode = true;
                //Update current value
                if (curSS != null)
                {
                    int colIndex = curSS.Channel == CyChannelNumber.First ? colCh0.Index : colCh1.Index;
                    for (int j = 0; j < dgScanSlots.RowCount; j++)
                        if (dgScanSlots[colIndex, j].Value == curSS)
                        {
                            dgScanSlots.CurrentCell = dgScanSlots[colIndex, j];
                            break;
                        }
                }
                if (dgScanSlots.CurrentCell == null && dgScanSlots.RowCount > 0)
                    dgScanSlots.CurrentCell = dgScanSlots[0, 0];

                AssignSSProperties();

                dgScanSlots.ResumeLayout(false);
                dgScanSlots.PerformLayout();
            }
        }

        void UpdateDGRowsColor(int col)
        {
            Color iColor = ROW_COLOR1;
            int rowindex = 0;
            while (rowindex < dgScanSlots.RowCount)
            {
                //Paint Cell
                dgScanSlots[col, rowindex].Style.BackColor = GetScanSlot(col, rowindex) == null ? Color.White : iColor;

                if (rowindex < dgScanSlots.RowCount - 1)
                {
                    object valThis = dgScanSlots[col, rowindex].Value;
                    object valNext = dgScanSlots[col, rowindex + 1].Value;

                    if (valThis == null || (valThis is CyScanSlot == false))
                    { rowindex++; continue; }
                    if (valNext == null || (valNext is CyScanSlot == false))
                    { rowindex++; continue; }

                    if (((valThis as CyScanSlot).m_widget != (valNext as CyScanSlot).m_widget))
                    {
                        if (iColor == ROW_COLOR1) iColor = ROW_COLOR2;
                        else iColor = ROW_COLOR1;
                    }
                }
                rowindex++;
            }
        }
        List<CyScanSlot> GetSelectedSS(bool addGuard)
        {
            List<CyScanSlot> res = new List<CyScanSlot>();
            CyScanSlot ss;
            for (int i = 0; i < dgScanSlots.SelectedRows.Count; i++)
            {
                ss = GetScanSlot(colCh0.Index, dgScanSlots.SelectedRows[i].Index);
                if (ss != null)
                    res.Add(ss);
                ss = GetScanSlot(colCh1.Index, dgScanSlots.SelectedRows[i].Index);
                if (ss != null)
                    res.Add(ss);
            }
            for (int i = 0; i < dgScanSlots.SelectedCells.Count; i++)
            {
                ss = GetScanSlot(dgScanSlots.SelectedCells[i].ColumnIndex, dgScanSlots.SelectedCells[i].RowIndex);
                if (ss != null)
                    res.Add(ss);
            }
            if (addGuard==false)
                //Prevent any actions on Guard Sensor
                res.Remove(m_packParams.m_widgets.m_scanSlots.m_guardSensor);
            return res;
        }
        CyScanSlot GetScanSlot(int col, int row)
        {
            CyScanSlot res = null;
            if (col == -1 || row == -1 || col >= dgScanSlots.ColumnCount || row >= dgScanSlots.RowCount) return res;
            object valThis = dgScanSlots[col, row].Value;
            if (valThis is CyScanSlot)
                res = valThis as CyScanSlot;

            return res != m_nullScanSlot ? res : null;
        }
        #endregion

        private void dgScanSlots_CellPainting(object sender, DataGridViewCellPaintingEventArgs e)
        {
            if (m_packParams.m_widgets.m_needUpdate)
            {
                m_packParams.m_widgets.m_needUpdate = false;

                UpdateScanSlotsLists();
            }
        }
    }
}