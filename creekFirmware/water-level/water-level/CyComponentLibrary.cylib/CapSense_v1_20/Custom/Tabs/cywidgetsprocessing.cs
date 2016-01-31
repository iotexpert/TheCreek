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
using System.Drawing;

namespace CapSense_v1_20
{
    public partial class CyScanSlotsTab
    {
        #region Header
        string m_strNewName = "NewName";
        int m_defResolutions = 100;

        #region ColLocation
        List<CyColLocation> m_massColLocation = new List<CyColLocation>();
        void AddColLocation(DataGridView item, string name, string strHeader)
        {
            CyColLocation ColLocation = new CyColLocation(name, strHeader);
            ColLocation.Visible = false;
            m_massColLocation.Add(ColLocation);
            item.Columns.Add(ColLocation);
        }
        void SetColLocation()
        {
            IEnumerable<DataGridView> resAllDG = GetAllDataGrids();
            foreach (DataGridView item in resAllDG)
            {
                ((System.ComponentModel.ISupportInitialize)(item)).BeginInit();
                if ((item == m_dgMatrixButtons) || (item == m_dgTouchpads))
                {
                    DataGridViewTextBoxColumn col = new DataGridViewTextBoxColumn();
                    col.Visible = false;
                    item.Columns.Add((DataGridViewColumn)(col));

                    col = new DataGridViewTextBoxColumn();
                    col.Visible = false;
                    item.Columns.Add((DataGridViewColumn)(col));

                    AddColLocation(item, "ColRowLocation", "Rows Location");
                    AddColLocation(item, "ColColLocation", "Columns Location");
                }
                else
                {
                    DataGridViewTextBoxColumn col = new DataGridViewTextBoxColumn();
                    col.Visible = false;
                    item.Columns.Add((DataGridViewColumn)(col));
                    AddColLocation(item, "ColLocation", "Location");
                }

                ((System.ComponentModel.ISupportInitialize)(item)).EndInit();

            }
        }
        void ColLocationVisible()
        {
            bool isVisible = m_packParams.Configuration != E_MAIN_CONFIG.emSerial;

            //Fixing problem with unvisible current cell
            IEnumerable<DataGridView> resAllDG = GetAllDataGrids();
            foreach (DataGridView item in resAllDG)
            {
                //
                item.CurrentCell = item[0, 0];
            }

            foreach (CyColLocation item in m_massColLocation)
            {
                item.Items.Clear();
                int m_id = 0;
                foreach (CyAmuxBParams ab in m_packParams.m_localParams.m_listCsHalfs)
                {
                    if (m_packParams.m_localParams.IsAmuxBusEnable(ab))
                        item.Items.Add(CyColLocation.strItems[m_id]);

                    m_id++;
                }

                item.Visible = isVisible;
            }
        }
        #endregion
        void AssigneActions()
        {
            SetColLocation();
            //Assigne Actions
            IEnumerable<DataGridView> resAllDG = GetAllDataGrids();

            foreach (DataGridView item in resAllDG)
            {
                item.CurrentCellDirtyStateChanged += new EventHandler(dg_CurrentCellDirtyStateChanged);
                item.CellClick += new DataGridViewCellEventHandler(dg_CellEnter);
                item.RowEnter += new System.Windows.Forms.DataGridViewCellEventHandler(dgEnter);
            }

            m_dgButtons.RowValidating +=
                new System.Windows.Forms.DataGridViewCellCancelEventHandler(dgButtons_RowValidating);
            m_dgGeneric.RowValidating += new DataGridViewCellCancelEventHandler(dgGenProx_RowValidating);
            m_dgProximity.RowValidating += new DataGridViewCellCancelEventHandler(dgGenProx_RowValidating);

            m_dgMatrixButtons.RowValidating +=
                new DataGridViewCellCancelEventHandler(dgMatrixButtons_RowValidating);
            m_dgTouchpads.RowValidating += new DataGridViewCellCancelEventHandler(dgTouchpads_RowValidating);
            m_dgSliders.RowValidating += new DataGridViewCellCancelEventHandler(dgSliders_RowValidating);

            m_dgButtons.RowHeaderMouseDoubleClick +=
                new DataGridViewCellMouseEventHandler(dgButtons_RowHeaderMouseDoubleClick);
            m_dgGeneric.RowHeaderMouseDoubleClick +=
                new DataGridViewCellMouseEventHandler(dgGeneric_RowHeaderMouseDoubleClick);
            m_dgProximity.RowHeaderMouseDoubleClick +=
                new DataGridViewCellMouseEventHandler(dgProximity_RowHeaderMouseDoubleClick);
            m_dgMatrixButtons.RowHeaderMouseDoubleClick +=
                new DataGridViewCellMouseEventHandler(dgMatrixButtons_RowHeaderMouseDoubleClick);
            m_dgTouchpads.RowHeaderMouseDoubleClick +=
                new DataGridViewCellMouseEventHandler(dgTouchpads_RowHeaderMouseDoubleClick);
            m_dgSliders.RowHeaderMouseDoubleClick +=
                new DataGridViewCellMouseEventHandler(dgSliders_RowHeaderMouseDoubleClick);

            foreach (DataGridView item in resAllDG)
            {
                item.RowValidated += new DataGridViewCellEventHandler(dgRowValidated);
            }

            //Modify Splitter
            m_packParams.m_cyButtonsTab.Load += new EventHandler(cyButtons_Load);
            m_packParams.m_cySlidersTab.Load += new EventHandler(cySliders_Load);
            m_packParams.m_cyTouchPadsTab.Load += new EventHandler(cyTouchPads_Load);
            m_packParams.m_cyMatrixButtonsTab.Load += new EventHandler(cyMatrixButtons_Load);
            m_packParams.m_cyProximityTab.Load += new EventHandler(cyProximity_Load);

        }

        IEnumerable<DataGridView> GetAllDataGrids()
        {
            yield return m_dgButtons;
            yield return m_dgGeneric;
            yield return m_dgProximity;
            yield return m_dgMatrixButtons;
            yield return m_dgTouchpads;
            yield return m_dgSliders;
        }

        public void UpdateAllWidgetsInDataGrids()
        {
            foreach (DataGridView item in GetAllDataGrids())
            {
                for (int i = 0; i < item.RowCount; i++)
                {
                    item.CurrentCell = item[0, i];
                }
            }
        }
        public void UpdateWidgetsDataGrids(object sender)
        {
            foreach (DataGridView item in GetAllDataGrids())
            {
                if (item.CurrentCell != null)
                {
                    DataGridViewCell cell = item.CurrentCell;
                    item.CurrentCell = item[0, item.RowCount - 1];
                    item.CurrentCell = cell;
                }
            }
        }

        public E_SENSOR_TYPE[] GetWidegetTypes(DataGridView sender)
        {
            if (sender == m_dgButtons)
            {
                return new E_SENSOR_TYPE[] { E_SENSOR_TYPE.Button };
            }
            else if (sender == m_dgSliders)
            {
                return new E_SENSOR_TYPE[] { E_SENSOR_TYPE.Linear_Slider };
            }
            else if (sender == m_dgTouchpads)
            {
                return new E_SENSOR_TYPE[] { E_SENSOR_TYPE.Touchpad_Row, E_SENSOR_TYPE.Touchpad_Col };
            }
            else if (sender == m_dgMatrixButtons)
            {
                return new E_SENSOR_TYPE[] { E_SENSOR_TYPE.Matrix_Buttons_Row, E_SENSOR_TYPE.Matrix_Buttons_Col };
            }
            else if (sender == m_dgProximity)
            {
                return new E_SENSOR_TYPE[] { E_SENSOR_TYPE.Proximity };
            }
            else if (sender == m_dgGeneric)
            {
                return new E_SENSOR_TYPE[] { E_SENSOR_TYPE.Generic };
            }
            return null;

        }
        #endregion

        #region Load
        public void LoadFormGeneralParams()
        {
            //LoadingObject = true;
            //Load Widgets
            foreach (CyElWidget item in m_packParams.m_cyWidgetsList.GetListWidgets())
            {
                switch (item.m_type)
                {
                    case E_SENSOR_TYPE.Button:
                        m_packParams.m_cyButtonsTab.dgButtons.Rows.
                            Add(new object[] { item.m_Name, item, CyColLocation.getString(item.m_side) });
                        break;
                    case E_SENSOR_TYPE.Linear_Slider:
                    case E_SENSOR_TYPE.Radial_Slider:
                        AddSliderRow(item);
                        break;
                    case E_SENSOR_TYPE.Touchpad_Col:
                        //case sensorType.Touchpads_Row:
                        AddTouchPadRow(item);
                        break;
                    case E_SENSOR_TYPE.Matrix_Buttons_Col:
                        //case sensorType.Matrix_Buttons_Row:
                        AddMatrizButtonRow(item);
                        break;
                    case E_SENSOR_TYPE.Proximity:
                        m_packParams.m_cyProximityTab.dgProximity.Rows.Add(new object[] { item.m_Name, 
                            ((CyElUnButton)item).m_Count, item, CyColLocation.getString(item.m_side) });
                        break;
                    case E_SENSOR_TYPE.Generic:
                        m_packParams.m_cyGenericTab.dgGeneric.Rows.Add(new object[] { item.m_Name, 
                            ((CyElUnButton)item).m_Count, item, CyColLocation.getString(item.m_side) });
                        break;
                    default:
                        break;
                }
            }
            List<CyElScanSlot> tListSS = m_packParams.m_cyScanSlotsList.m_listScanSlotsL;
            //Load ScanSlots
            for (int i = 0; i < tListSS.Count; i++)
            {
                ShowScanSlot(tListSS[i], tListSS[i].m_side);
            }
            tListSS = m_packParams.m_cyScanSlotsList.m_listScanSlotsR;
            for (int i = 0; i < tListSS.Count; i++)
            {
                ShowScanSlot(tListSS[i], tListSS[i].m_side);
            }

            //UpdateAllWidgetsDataGrids();
        }
        void AddTouchPadRow(CyElWidget item)
        {
            CyElUnTouchPad wiC = (CyElUnTouchPad)m_packParams.
                        m_cyWidgetsList.FindWidget(item.m_Name,E_SENSOR_TYPE.Touchpad_Col);
            CyElUnTouchPad wiR = (CyElUnTouchPad)m_packParams.
                        m_cyWidgetsList.FindWidget(item.m_Name,E_SENSOR_TYPE.Touchpad_Row);
            m_packParams.m_cyTouchPadsTab.dgTouchpads.Rows.Add(new object[] { wiC.m_Name, wiC.m_numElRow, 
                        wiC.m_numElCol, wiC.m_resElRow, wiC.m_resElCol, wiR, wiC, 
                        CyColLocation.getString(wiR.m_side), CyColLocation.getString(wiC.m_side) });

        }
        void AddMatrizButtonRow(CyElWidget item)
        {
            CyElUnMatrixButton wiC = (CyElUnMatrixButton)m_packParams.m_cyWidgetsList.FindWidget
                                                                (item.m_Name,E_SENSOR_TYPE.Matrix_Buttons_Col);
            CyElUnMatrixButton wiR = (CyElUnMatrixButton)m_packParams.m_cyWidgetsList.FindWidget
                                                                (item.m_Name, E_SENSOR_TYPE.Matrix_Buttons_Row);
            m_packParams.m_cyMatrixButtonsTab.dgMatrixButtons.Rows.Add(new object[] { wiC.m_Name, wiC.m_numElRow, 
                wiC.m_numElCol, wiR, wiC,  CyColLocation.getString(wiR.m_side), CyColLocation.getString(wiC.m_side) });
        }
        void AddSliderRow(CyElWidget item)
        {
            CyElUnSlider wi = (CyElUnSlider)item;
            m_packParams.m_cySlidersTab.dgSliders.Rows.Add(new object[] { item.m_Name, 
                CyGeneralParams.GetStrSliderType(wi.m_type), wi.m_Count, wi.m_Resolution, 
                wi.m_diplexing, item, CyColLocation.getString(item.m_side) });
        }


        void SpliterMove(SplitContainer sp, int h)
        {
            sp.SplitterDistance = sp.Height - h;
        }
        void cyProximity_Load(object sender, EventArgs e)
        {
            SpliterMove(m_packParams.m_cyProximityTab.splitContainer1, 130);//162);
        }

        void cyMatrixButtons_Load(object sender, EventArgs e)
        {
            SpliterMove(m_packParams.m_cyMatrixButtonsTab.splitContainer1, 130);//162);      
        }

        void cyTouchPads_Load(object sender, EventArgs e)
        {
            SpliterMove(m_packParams.m_cyTouchPadsTab.splitContainer1, 180);
        }

        void cySliders_Load(object sender, EventArgs e)
        {
            SpliterMove(m_packParams.m_cySlidersTab.splitContainer1, 162);
        }

        void cyButtons_Load(object sender, EventArgs e)
        {
            SpliterMove(m_packParams.m_cyButtonsTab.splitContainer1, 130);//162);
        }

        #endregion

        #region DClick
        void dgDClickButton(object sender, DataGridViewCellMouseEventArgs e, E_SENSOR_TYPE[] type)
        {
            if (e.RowIndex != ((DataGridView)sender).RowCount - 1)//not last
            {
                if (CyGeneralParams.GetDGString(sender, e.RowIndex, 0) != "")
                {
                    CyElWidget[] curWi = m_packParams.m_cyWidgetsList.GetWidgets
                        (GetCellsArrayFromDG(sender, e.RowIndex), type[0]);
                    if (curWi[0] != null)
                    {
                        m_packParams.m_cyWidgetsList.DeleteWidgets(curWi);
                    }
                    ((DataGridView)sender).Rows.RemoveAt(e.RowIndex);
                }
                //Commit parameters               
                m_packParams.SetCommitParams(null, null);
            }
        }
        void dgSliders_RowHeaderMouseDoubleClick(object sender, DataGridViewCellMouseEventArgs e)
        {
            dgDClickButton(sender, e, new E_SENSOR_TYPE[] { E_SENSOR_TYPE.Linear_Slider });
        }

        void dgTouchpads_RowHeaderMouseDoubleClick(object sender, DataGridViewCellMouseEventArgs e)
        {
            dgDClickButton(sender, e, new E_SENSOR_TYPE[] { E_SENSOR_TYPE.Touchpad_Row, E_SENSOR_TYPE.Touchpad_Col });
        }

        void dgMatrixButtons_RowHeaderMouseDoubleClick(object sender, DataGridViewCellMouseEventArgs e)
        {
            dgDClickButton(sender, e, new E_SENSOR_TYPE[] { E_SENSOR_TYPE.Matrix_Buttons_Row, E_SENSOR_TYPE.Matrix_Buttons_Col });
        }

        void dgProximity_RowHeaderMouseDoubleClick(object sender, DataGridViewCellMouseEventArgs e)
        {
            dgDClickButton(sender, e, new E_SENSOR_TYPE[] { E_SENSOR_TYPE.Proximity });
        }

        void dgGeneric_RowHeaderMouseDoubleClick(object sender, DataGridViewCellMouseEventArgs e)
        {
            dgDClickButton(sender, e, new E_SENSOR_TYPE[] { E_SENSOR_TYPE.Generic });
        }

        void dgButtons_RowHeaderMouseDoubleClick(object sender, DataGridViewCellMouseEventArgs e)
        {
            dgDClickButton(sender, e, new E_SENSOR_TYPE[] { E_SENSOR_TYPE.Button });
        }
        #endregion

        #region Row Validated
        public void dgRowValidated(object sender, DataGridViewCellEventArgs e)
        {
            DataGridView dgInput = ((DataGridView)sender);
            E_SENSOR_TYPE[] type = GetWidegetTypes(dgInput);
            int col = 0;

            if (e.RowIndex != dgInput.RowCount - 1)//not last
            {
                if (CyGeneralParams.GetDGString(sender, e.RowIndex, col) != "")
                {
                    object[] CurRowData = GetCellsArrayFromDG(sender, e.RowIndex);

                    List<CyElWidget> res = new List<CyElWidget>
                        (m_packParams.m_cyWidgetsList.ValidateWidgets(type, CurRowData));

                    if (res != null)
                    {
                        //Save Result in DataGrid Cells
                        for (int i = 0; i < res.Count; i++)
                            if (res[i] != null)
                            {
                                //Calculate Column
                                col = dgInput.ColumnCount - (res.Count - i + res.Count);
                                dgInput[col, e.RowIndex].Value = res[i];
                            }
                    }

                    //Show Property panel
                    dgEnter(sender, e);
                }

                //Commit parameters               
                m_packParams.SetCommitParams(null, null);
            }

        }
        #endregion

        #region Row Validating
        void colLocationValidation(object sender,
            System.Windows.Forms.DataGridViewCellCancelEventArgs e, bool twoHalf)
        {
            DataGridView dgInput = ((DataGridView)sender);
            int col1 = dgInput.ColumnCount - 1;
            if (CyGeneralParams.GetDGString(sender, e.RowIndex, col1) == "")
                dgInput[col1, e.RowIndex].Value =
                    CyColLocation.strItems[m_packParams.m_localParams.GetFirstAvailibleSide()];

            //For Single Mode
            if (m_packParams.m_localParams.GetCountAvailibleSides() < 2)//Not Full Mode
                dgInput[col1, e.RowIndex].Value =
                    CyColLocation.strItems[m_packParams.m_localParams.GetFirstAvailibleSide()];

            if (twoHalf)
            {
                int col2 = dgInput.ColumnCount - 2;
                if (CyGeneralParams.GetDGString(sender, e.RowIndex, col2) == "")
                    dgInput[col2, e.RowIndex].Value = dgInput[col1, e.RowIndex].Value.ToString();

                //For Serial Mode
                if (m_packParams.m_localParams.GetCountAvailibleSides() < 2)
                    dgInput[col2, e.RowIndex].Value =
                        CyColLocation.strItems[m_packParams.m_localParams.GetFirstAvailibleSide()];

                if (m_packParams.Configuration == E_MAIN_CONFIG.emParallelAsynchron)
                {
                    //Different amux bus can't be
                    if (dgInput[col1, e.RowIndex].Value.ToString() != dgInput[col2, e.RowIndex].Value.ToString())
                    {
                        if (m_packParams.GlobalEditMode)
                            MessageBox.Show(
                                "In Parallel Asynchronous mode Analog Mux Bus for both rows and columns must be the same.",
                                "Warning", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                        dgInput[col2, e.RowIndex].Value = dgInput[col1, e.RowIndex].Value.ToString();
                    }
                }
            }

        }

        void dgButtons_RowValidating(object sender, System.Windows.Forms.DataGridViewCellCancelEventArgs e)
        {

            int row = e.RowIndex;
            if (e.RowIndex != ((DataGridView)sender).RowCount - 1)//not last
            {
                int col = 0;
                int res = dgNameValidating(sender, e.RowIndex, col);

                if ((res == 2) || (res == 1))
                {
                    int p = 0;
                    while (TestIdentityInColumn(sender, col, m_strNewName + p) != 0) p++;
                    ((DataGridView)sender)[col, row].Value = m_strNewName + p;
                }
                colLocationValidation(sender, e, false);

            }

        }
        void dgGenProx_RowValidating(object sender, DataGridViewCellCancelEventArgs e)
        {
            if (e.RowIndex != ((DataGridView)sender).RowCount - 1)//not last
            {
                int col = 0;
                int res = dgNameValidating(sender, e.RowIndex, col);

                if ((res == 2) || (res == 1))
                {
                    int p = 0;
                    while (TestIdentityInColumn(sender, col, m_strNewName + p) != 0) p++;
                    ((DataGridView)sender)[col, e.RowIndex].Value = m_strNewName + p;
                    //e.Cancel = true;
                }
                col = 1;
                int minValue = 0;
                res = dgNumberValidating(sender, e.RowIndex, col, minValue, false);
                if ((res == 2) || (res == 1))
                {
                    ((DataGridView)sender)[col, e.RowIndex].Value = minValue;
                }

                colLocationValidation(sender, e, false);
            }
        }

        void dgSliders_RowValidating(object sender, DataGridViewCellCancelEventArgs e)
        {
            if (e.RowIndex != ((DataGridView)sender).RowCount - 1)//not last
            {
                int col = 0;
                int res = dgNameValidating(sender, e.RowIndex, col);

                if ((res == 2) || (res == 1))
                {
                    int p = 0;
                    while (TestIdentityInColumn(sender, col, m_strNewName + p) != 0) p++;
                    ((DataGridView)sender)[col, e.RowIndex].Value = m_strNewName + p;
                    //e.Cancel = true;
                }
                col = 1;
                if (CyGeneralParams.GetDGString(sender, e.RowIndex, col) == "")
                {
                    ((DataGridView)sender)[col, e.RowIndex].Value = CyGeneralParams.m_strSliderType[0];
                }
                col = 2;
                int minValue = 2;
                res = dgNumberValidating(sender, e.RowIndex, col, minValue, false);
                if ((res == 2) || (res == 1))
                {
                    ((DataGridView)sender)[col, e.RowIndex].Value = minValue;
                }
                col = 3;
                res = dgNumberValidating(sender, e.RowIndex, col, minValue, true);
                if ((res == 2) || (res == 1))
                {
                    ((DataGridView)sender)[col, e.RowIndex].Value = m_defResolutions;
                }

                colLocationValidation(sender, e, false);
            }
        }

        void dgTouchpads_RowValidating(object sender, DataGridViewCellCancelEventArgs e)
        {
            dgMatrixButtons_RowValidating(sender, e);
            if (e.RowIndex != ((DataGridView)sender).RowCount - 1)//not last
            {
                int minValue = 2;
                int col = 3;
                int res = dgNumberValidating(sender, e.RowIndex, col, minValue, true);
                if ((res == 2) || (res == 1))
                {
                    ((DataGridView)sender)[col, e.RowIndex].Value = m_defResolutions;
                }
                col = 4;
                res = dgNumberValidating(sender, e.RowIndex, col, minValue, true);
                if ((res == 2) || (res == 1))
                {
                    ((DataGridView)sender)[col, e.RowIndex].Value = m_defResolutions;
                }
            }
        }
        void dgMatrixButtons_RowValidating(object sender, DataGridViewCellCancelEventArgs e)
        {
            if (e.RowIndex != ((DataGridView)sender).RowCount - 1)//not last
            {
                int col = 0;
                int res = dgNameValidating(sender, e.RowIndex, col);

                if ((res == 2) || (res == 1))
                {
                    int p = 0;
                    while (TestIdentityInColumn(sender, col, m_strNewName + p) != 0) p++;
                    ((DataGridView)sender)[col, e.RowIndex].Value = m_strNewName + p;
                    //e.Cancel = true;
                }
                int minValue = 2;
                col = 1;
                res = dgNumberValidating(sender, e.RowIndex, col, minValue, false);
                if ((res == 2) || (res == 1))
                {
                    ((DataGridView)sender)[col, e.RowIndex].Value = minValue;
                }
                col = 2;
                res = dgNumberValidating(sender, e.RowIndex, col, minValue, false);
                if ((res == 2) || (res == 1))
                {
                    ((DataGridView)sender)[col, e.RowIndex].Value = minValue;
                }
                colLocationValidation(sender, e, true);
            }

        }

        int dgNameValidating(object sender, int row, int col)
        {
            int res = 2;//null value
            try
            {
                if ((((DataGridView)sender)[col, row].Value == null))
                    throw new Exception();
                if ((((DataGridView)sender)[col, row].Value.ToString() == ""))
                    throw new Exception();
                res = 1;//wrong name
                string str = ((DataGridView)sender)[col, row].Value.ToString();
                //Test Characters
                foreach (char ch in str)
                {
                    if ((ch >= 'A') && (ch <= 'Z')) continue;
                    if ((ch >= 'a') && (ch <= 'z')) continue;
                    if ((ch >= '0') && (ch <= '9')) continue;
                    if (ch == '_') continue;
                    throw new Exception();
                }

                //Test identity
                if (TestIdentityInColumn(sender, col, str) > 1) throw new Exception();

                res = 0;
            }
            catch
            {
                if (res != 2)//Remove message, if user hasn't define any value
                    MessageBox.Show("Please input correct Name!", "Warning", MessageBoxButtons.OK, MessageBoxIcon.Warning);
            }
            return res;

        }
        int dgNumberValidating(object sender, int row, int col, int minValue, bool isResolution)
        {
            int res = 2;//null value
            string valueType = "Count";
            if (isResolution) valueType = "Resolution";
            try
            {
                if ((((DataGridView)sender)[col, row].Value == null))
                    throw new Exception();
                if ((((DataGridView)sender)[col, row].Value.ToString() == ""))
                    throw new Exception();
                res = 1;//wrong Number
                string str = ((DataGridView)sender)[col, row].Value.ToString();
                //Test Characters
                foreach (char ch in str)
                {
                    if ((ch >= '0') && (ch <= '9')) continue;
                    throw new Exception();
                }
                int val = Convert.ToInt32(str);

                if (val >= minValue)
                {
                    res = 0;//Correct value
                }
                else new Exception();
            }
            catch
            {
                if (res != 2)//Remove message, if user hasn't define any value
                    MessageBox.Show("Please input correct " + valueType + "!", "Warning", MessageBoxButtons.OK, MessageBoxIcon.Warning);
            }
            return res;

        }
        #endregion

        #region Row Enter
        void dgEnter(object sender, System.Windows.Forms.DataGridViewCellEventArgs e)
        {
            int t_cRow = e.RowIndex;
            if ((t_cRow != -1) && (t_cRow < ((DataGridView)sender).RowCount - 1))
            {
                string name = CyGeneralParams.GetDGString(sender, t_cRow, 0);
                //Select Object Processing
                if (sender == m_dgButtons)
                {
                    m_packParams.m_cyButtonsTab.UnitPropertyGrid.GetProperties(m_packParams.
                        m_cyWidgetsList.FindWidget(name, E_SENSOR_TYPE.Button));
                }
                else if (sender == m_dgSliders)
                {
                    m_packParams.m_cySlidersTab.UnitPropertyGrid.GetProperties(m_packParams.
                        m_cyWidgetsList.FindWidget(name, E_SENSOR_TYPE.Linear_Slider));
                }
                else if (sender == m_dgTouchpads)
                {
                    m_packParams.m_cyTouchPadsTab.UnitPropertyGrid.GetProperties(m_packParams.
                        m_cyWidgetsList.FindWidget(name, E_SENSOR_TYPE.Touchpad_Col));
                }
                else if (sender == m_dgMatrixButtons)
                {
                    m_packParams.m_cyMatrixButtonsTab.UnitPropertyGrid.GetProperties(m_packParams.
                        m_cyWidgetsList.FindWidget(name, E_SENSOR_TYPE.Matrix_Buttons_Col));
                }
                else if (sender == m_dgProximity)
                {
                    m_packParams.m_cyProximityTab.UnitPropertyGrid.GetProperties(m_packParams.
                        m_cyWidgetsList.FindWidget(name, E_SENSOR_TYPE.Proximity));
                }
            }
            else
            {
                FreeUnitPropertyGrid(sender);
            }
        }


        void FreeUnitPropertyGrid(object sender)
        {
            //Select Object Processing
            if (sender == m_dgButtons)
            {
                m_packParams.m_cyButtonsTab.UnitPropertyGrid.GetProperties(null);
            }
            else if (sender == m_dgSliders)
            {
                m_packParams.m_cySlidersTab.UnitPropertyGrid.GetProperties(null);
            }
            else if (sender == m_dgTouchpads)
            {
                m_packParams.m_cyTouchPadsTab.UnitPropertyGrid.GetProperties(null);
            }
            else if (sender == m_dgMatrixButtons)
            {
                m_packParams.m_cyMatrixButtonsTab.UnitPropertyGrid.GetProperties(null);
            }
            else if (sender == m_dgProximity)
            {
                m_packParams.m_cyButtonsTab.UnitPropertyGrid.GetProperties(null);
            }
        }

        #endregion

        #region Othere functions
        void dg_CurrentCellDirtyStateChanged(object sender, EventArgs e)
        {
            DataGridView dgw = (DataGridView)sender;
            //if (dgw.CurrentCell. ColumnIndex == colType.Index)


            if (dgw.IsCurrentCellDirty)
            {
                dgw.CommitEdit(DataGridViewDataErrorContexts.Commit);
            }
        }

        void dg_CellEnter(object sender, DataGridViewCellEventArgs e)
        {
            DataGridView dgw = (DataGridView)sender;
            {
                dgw.BeginEdit(true);
                if (dgw.EditingControl != null)
                {
                    if (dgw.EditingControl.GetType() == typeof(DataGridViewComboBoxEditingControl))
                    {
                        DataGridViewComboBoxEditingControl cb_dlc =
                            (DataGridViewComboBoxEditingControl)dgw.EditingControl;
                        cb_dlc.DroppedDown = true;

                    }

                }
            }
        }
        object[] GetCellsArrayFromDG(object sender, int row)
        {
            object[] res = new object[((DataGridView)sender).ColumnCount];
            for (int i = 0; i < ((DataGridView)sender).ColumnCount; i++)
            {
                res[i] = ((DataGridView)sender)[i, row].Value;
            }
            return res;
        }

        int GetCurrentRowFromDG(DataGridView dg)
        {
            int SelectedRowIndex = -1;
            if (dg.SelectedRows.Count > 0)
            {
                SelectedRowIndex = dgScanSlotsL.SelectedRows[0].Index;
            }
            else if (dg.SelectedCells.Count > 0)
            {
                SelectedRowIndex = dg.SelectedCells[0].RowIndex;
            }
            return SelectedRowIndex;

        }


        int TestIdentityInColumn(object sender, int col, string str)
        {
            //Test identity
            int ccopy = 0;
            for (int i = 0; i < ((DataGridView)sender).RowCount - 1; i++)
            {
                if (((DataGridView)sender)[col, i].Value != null)
                {
                    if (((DataGridView)sender)[col, i].Value.ToString() == str)
                        ccopy++;
                }
            }
            return ccopy;
        }
        #endregion
    }

    #region CyColLocation
    public partial class CyColLocation : DataGridViewComboBoxColumn
    {
        //string 

        public static string[] strItems = new string[2] { "Left", "Right" };

        public static E_EL_SIDE getSide(object obj)
        {
            E_EL_SIDE res = E_EL_SIDE.None;
            if (Convert.ToString(obj) == strItems[0]) res = E_EL_SIDE.Left;
            if (Convert.ToString(obj) == strItems[1]) res = E_EL_SIDE.Right;

            return res;
        }
        public static string getString(E_EL_SIDE obj)
        {
            string res = strItems[0];
            if (obj == E_EL_SIDE.Right) res = strItems[1];
            return res;
        }

        public CyColLocation(string name, string strHeader)
        {
            this.HeaderText = strHeader;
            this.Name = name;
            this.AutoSizeMode = DataGridViewAutoSizeColumnMode.None;
            this.Width = 56;
            this.Items.AddRange(strItems);

        }
        public void ReAssigneItems()
        {
            Items.Clear();
            Items.AddRange(strItems);
        }
    }
    #endregion
}


