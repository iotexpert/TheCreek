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

namespace  CapSense_v0_5
{
    public partial class CyScanSlots
    {
        #region Header
        //int sendError = 0;
        //object[] LastRow=null;
        //sensorType lastType;
        string strNewName = "NewName";
        int defResolutions = 100;

        #region ColLocation
        List<CyColLocation> massColLocation = new List<CyColLocation>();
        void AddColLocation(DataGridView item,string name,string strHeader)
        {
            CyColLocation ColLocation = new CyColLocation(name, strHeader);
            ColLocation.Visible = false;
            massColLocation.Add(ColLocation);
            item.Columns.Add(ColLocation);            
        }
        void SetColLocation()
        {
            IEnumerable<DataGridView> resAllDG = GetAllDataGrids();
            foreach (DataGridView item in resAllDG)
            {                 
                ((System.ComponentModel.ISupportInitialize)(item)).BeginInit();
                if ((item == dgMatrixButtons) || (item == dgTouchpads))
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
                    AddColLocation(item, "ColLocation", "");
                }
               
                ((System.ComponentModel.ISupportInitialize)(item)).EndInit();

            }
        }
        void ColLocationVisible()
        {
            bool isVisible = packParams.Configuration != eMConfiguration.emSerial;

            //Fixing problem with unvisible current cell
            IEnumerable<DataGridView> resAllDG = GetAllDataGrids();
            foreach (DataGridView item in resAllDG)
            {
                //
                item.CurrentCell = item[0, 0];
            }

            foreach (CyColLocation item in massColLocation)
            {
                item.Items.Clear();
                int m_id = 0;
                foreach (CyAmuxBParams ab in packParams.localParams.listCsHalfs)
                {
                    if (packParams.localParams.bCsHalfIsEnable(ab))
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

            dgButtons.RowValidating += new System.Windows.Forms.DataGridViewCellCancelEventHandler(dgButtons_RowValidating);
            dgGeneric.RowValidating += new DataGridViewCellCancelEventHandler(dgGenProx_RowValidating);
            dgProximity.RowValidating += new DataGridViewCellCancelEventHandler(dgGenProx_RowValidating);

            dgMatrixButtons.RowValidating += new DataGridViewCellCancelEventHandler(dgMatrixButtons_RowValidating);
            dgTouchpads.RowValidating += new DataGridViewCellCancelEventHandler(dgTouchpads_RowValidating);
            dgSliders.RowValidating += new DataGridViewCellCancelEventHandler(dgSliders_RowValidating);

            dgButtons.RowHeaderMouseDoubleClick += new DataGridViewCellMouseEventHandler(dgButtons_RowHeaderMouseDoubleClick);
            dgGeneric.RowHeaderMouseDoubleClick += new DataGridViewCellMouseEventHandler(dgGeneric_RowHeaderMouseDoubleClick);
            dgProximity.RowHeaderMouseDoubleClick += new DataGridViewCellMouseEventHandler(dgProximity_RowHeaderMouseDoubleClick);
            dgMatrixButtons.RowHeaderMouseDoubleClick += new DataGridViewCellMouseEventHandler(dgMatrixButtons_RowHeaderMouseDoubleClick);
            dgTouchpads.RowHeaderMouseDoubleClick += new DataGridViewCellMouseEventHandler(dgTouchpads_RowHeaderMouseDoubleClick);
            dgSliders.RowHeaderMouseDoubleClick += new DataGridViewCellMouseEventHandler(dgSliders_RowHeaderMouseDoubleClick);

            foreach (DataGridView item in resAllDG)
            {
                item.RowValidated += new DataGridViewCellEventHandler(dgRowValidated);
            }

            //Modify Splitter
            packParams.cyButtons.Load += new EventHandler(cyButtons_Load);
            packParams.cySliders.Load += new EventHandler(cySliders_Load);
            packParams.cyTouchPads.Load += new EventHandler(cyTouchPads_Load);
            packParams.cyMatrixButtons.Load += new EventHandler(cyMatrixButtons_Load);
            packParams.cyProximity.Load += new EventHandler(cyProximity_Load);

        }

        IEnumerable<DataGridView> GetAllDataGrids()
        {
            yield return dgButtons;
            yield return dgGeneric;
            yield return dgProximity;
            yield return dgMatrixButtons;
            yield return dgTouchpads;
            yield return dgSliders;
        }

        public void UpdateAllWidgetsInDataGrids()
        {
            packParams.localParams.internalProcess = true;
            foreach (DataGridView item in GetAllDataGrids())
            {
                for (int i = 0; i < item.RowCount; i++)
                {
                    item.CurrentCell = item[0, i];
                }
            }
            packParams.localParams.internalProcess = false;
        }
        public void UpdateWidgetsDataGrids(object sender)
        {
            foreach (DataGridView item in GetAllDataGrids())
            {
                if (item.CurrentCell != null)
                {
                    DataGridViewCell cell = item.CurrentCell;
                    item.CurrentCell = item[0, item.RowCount-1];
                    item.CurrentCell = cell;
                }
            }
        }

        public sensorType[] GetWidegetTypes(DataGridView sender)
        {
            if (sender == dgButtons)
            {
                return new sensorType[] { sensorType.Button };
            }
            else if (sender == dgSliders)
            {
                return new sensorType[] { sensorType.Linear_Slider };
            }
            else if (sender == dgTouchpads)
            {
                return new sensorType[] { sensorType.Touchpad_Row, sensorType.Touchpad_Col};
            }
            else if (sender == dgMatrixButtons)
            {
                return new sensorType[] {sensorType.Matrix_Buttons_Row, sensorType.Matrix_Buttons_Col};
            }
            else if (sender == dgProximity)
            {
                return new sensorType[] { sensorType.Proximity };
            }
            else if (sender == dgGeneric)
            {
                return new sensorType[] { sensorType.Generic };
            }
            return null;

        }
        #endregion

        #region Load
        public void LoadFormGeneralParams()
        {
            //LoadingObject = true;
            //Load Widgets
            foreach (ElWidget item in packParams.cyWidgetsList.getListWidgets())
            {
                switch (item.type)
                {
                    case sensorType.Button:
                        packParams.cyButtons.dgButtons.Rows.Add(new object[] { item.Name, item, CyColLocation.getString(item.side) });
                        break;
                    case sensorType.Linear_Slider:
                    case sensorType.Radial_Slider:
                        AddSliderRow(item);
                        break;
                    case sensorType.Touchpad_Col:
                        //case sensorType.Touchpads_Row:
                        AddTouchPadRow(item);                        
                        break;
                    case sensorType.Matrix_Buttons_Col:
                        //case sensorType.Matrix_Buttons_Row:
                        AddMatrizButtonRow(item);
                        break;
                    case sensorType.Proximity:
                        packParams.cyProximity.dgProximity.Rows.Add(new object[] { item.Name, ((ElUnButton)item).Count, item, CyColLocation.getString(item.side) });
                        break;
                    case sensorType.Generic:
                        packParams.cyGeneric.dgGeneric.Rows.Add(new object[] { item.Name, ((ElUnButton)item).Count, item, CyColLocation.getString(item.side) });
                        break;
                    default:
                        break;
                }
            }
            List<ElScanSlot> tListSS = packParams.cyScanSlotsList.listScanSlotsL;
            //Load ScanSlots
            for (int i = 0; i < tListSS.Count; i++)
			{
                ShowScanSlot(tListSS[i], GetSide(tListSS[i].side));
            }
            tListSS = packParams.cyScanSlotsList.listScanSlotsR;
            for (int i = 0; i < tListSS.Count; i++)
            {
                ShowScanSlot(tListSS[i], GetSide(tListSS[i].side));
            }

            //UpdateAllWidgetsDataGrids();
        }
        void AddTouchPadRow(ElWidget item)
        {
            ElUnTouchPad wiC = (ElUnTouchPad)packParams.cyWidgetsList.FindWidget(sensorType.Touchpad_Col, item.Name);
            ElUnTouchPad wiR = (ElUnTouchPad)packParams.cyWidgetsList.FindWidget(sensorType.Touchpad_Row, item.Name);
            packParams.cyTouchPads.dgTouchpads.Rows.Add(new object[] { wiC.Name, wiC.numElRow, wiC.numElCol, wiC.resElRow, wiC.resElCol, wiR, wiC, CyColLocation.getString(wiR.side), CyColLocation.getString(wiC.side) });

        }
        void AddMatrizButtonRow(ElWidget item)
        {
            ElUnMatrixButton wiC = (ElUnMatrixButton)packParams.cyWidgetsList.FindWidget(sensorType.Matrix_Buttons_Col, item.Name); ;
            ElUnMatrixButton wiR = (ElUnMatrixButton)packParams.cyWidgetsList.FindWidget(sensorType.Matrix_Buttons_Row, item.Name);
            packParams.cyMatrixButtons.dgMatrixButtons.Rows.Add(new object[] { wiC.Name, wiC.numElRow, wiC.numElCol, wiR, wiC,  CyColLocation.getString(wiR.side), CyColLocation.getString(wiC.side) });
        }
        void AddSliderRow(ElWidget item)
        {            
            ElUnSlider wi = (ElUnSlider)item;
            packParams.cySliders.dgSliders.Rows.Add(new object[] { item.Name, CyGeneralParams.GetStrSliderType(wi.type), wi.Count, wi.Resolution, wi.diplexing, item, CyColLocation.getString(item.side) });
        }


        void spliterMove(SplitContainer sp, int h)
        {
            sp.SplitterDistance = sp.Height-h;
        }
        void cyProximity_Load(object sender, EventArgs e)
        {
            spliterMove(packParams.cyProximity.splitContainer1, 130);//162);
        }

        void cyMatrixButtons_Load(object sender, EventArgs e)
        {
            spliterMove(packParams.cyMatrixButtons.splitContainer1, 130);//162);      
        }

        void cyTouchPads_Load(object sender, EventArgs e)
        {
            spliterMove(packParams.cyTouchPads.splitContainer1, 180);
        }

        void cySliders_Load(object sender, EventArgs e)
        {
            spliterMove(packParams.cySliders.splitContainer1, 162);
        }

        void cyButtons_Load(object sender, EventArgs e)
        {
            spliterMove(packParams.cyButtons.splitContainer1, 130);//162);
        }

        #endregion

        #region DClick


        void dgDClickButton(object sender, DataGridViewCellMouseEventArgs e, sensorType[] type)
        {
            
            if (e.RowIndex != ((DataGridView)sender).RowCount - 1)//not last
            {
                if (CyGeneralParams.GetDGString(sender, e.RowIndex, 0) != "")
                {
                    ElWidget[] curWi = packParams.cyWidgetsList.GetWidget(GetCellsArrayFromDG(sender, e.RowIndex), type[0]);
                    if (curWi[0] != null)
                    {
                        packParams.cyWidgetsList.DeleteWidget(curWi);
                    }
                    ((DataGridView)sender).Rows.RemoveAt(e.RowIndex);
                }
            }
           }
           void dgSliders_RowHeaderMouseDoubleClick(object sender, DataGridViewCellMouseEventArgs e)
           {
               dgDClickButton(sender, e, new sensorType[] { sensorType.Linear_Slider});
           }

           void dgTouchpads_RowHeaderMouseDoubleClick(object sender, DataGridViewCellMouseEventArgs e)
           {
               dgDClickButton(sender, e, new sensorType[] {sensorType.Touchpad_Row, sensorType.Touchpad_Col});
           }

           void dgMatrixButtons_RowHeaderMouseDoubleClick(object sender, DataGridViewCellMouseEventArgs e)
           {
               dgDClickButton(sender, e,new sensorType[] {sensorType.Matrix_Buttons_Row, sensorType.Matrix_Buttons_Col});
           }

           void dgProximity_RowHeaderMouseDoubleClick(object sender, DataGridViewCellMouseEventArgs e)
           {
               dgDClickButton(sender, e, new sensorType[] { sensorType.Proximity});
           }

           void dgGeneric_RowHeaderMouseDoubleClick(object sender, DataGridViewCellMouseEventArgs e)
           {
               dgDClickButton(sender, e, new sensorType[] { sensorType.Generic});
           }

           void dgButtons_RowHeaderMouseDoubleClick(object sender, DataGridViewCellMouseEventArgs e)
           {
               dgDClickButton(sender, e, new sensorType[] { sensorType.Button });
           }
        #endregion

        #region Row Validated
        public void dgRowValidated(object sender, DataGridViewCellEventArgs e)
        {
            sensorType[] type = GetWidegetTypes((DataGridView)sender);
            int col = 0;
            DataGridView dgInput=((DataGridView)sender);

            if (e.RowIndex != ((DataGridView)sender).RowCount - 1)//not last
            {
                if (CyGeneralParams.GetDGString(sender, e.RowIndex, col) != "")
                {
                    object[] CurRowData = GetCellsArrayFromDG(sender, e.RowIndex);

                    List<ElWidget>res=new List<ElWidget>(packParams.cyWidgetsList.ValidateWidgets(type, CurRowData));
                    if (res != null)
                    {
                        for (int i = 0; i < res.Count; i++)
                            if( res[i] != null)
                        {
                            //Calculate Column
                            col = dgInput.ColumnCount - (res.Count - i + res.Count);
                            dgInput[col, e.RowIndex].Value = res[i];
                        }
                    }
                    dgEnter(sender, e);
                }
            }
        }
        #endregion

        #region Row Validating
        void colLocationValidation(object sender, System.Windows.Forms.DataGridViewCellCancelEventArgs e, bool twoHalf)
        {
            DataGridView dgInput = ((DataGridView)sender);
            int col1=dgInput.ColumnCount - 1;
            if (CyGeneralParams.GetDGString(sender, e.RowIndex, col1) == "")
                dgInput[col1, e.RowIndex].Value = CyColLocation.strItems[packParams.localParams.GetFirstAvailibleSide()];

            //For Single Mode
            //if(packParams.Configuration== eMConfiguration.emSerial)            
            if (packParams.localParams.GetCountAvailibleSides() < 2)//Not Full Mode
                dgInput[col1, e.RowIndex].Value = CyColLocation.strItems[packParams.localParams.GetFirstAvailibleSide()];

            if (twoHalf)
            {
                int col2 = dgInput.ColumnCount - 2;
                if (CyGeneralParams.GetDGString(sender, e.RowIndex, col2) == "")
                    dgInput[col2, e.RowIndex].Value = dgInput[col1, e.RowIndex].Value.ToString();

                //For Serial Mode
                //if (packParams.Configuration == eMConfiguration.emSerial)
                if (packParams.localParams.GetCountAvailibleSides() < 2)
                    dgInput[col2, e.RowIndex].Value = CyColLocation.strItems[packParams.localParams.GetFirstAvailibleSide()];

                if (packParams.Configuration == eMConfiguration.emParallelAsynchron)
                {
                    //Different amux bus can't be
                    if (dgInput[col1, e.RowIndex].Value.ToString() != dgInput[col2, e.RowIndex].Value.ToString())
                    {
                        if(!packParams.localParams.internalProcess)
                        MessageBox.Show("In Parallel Asynchronous mode Analog Mux Bus for both rows and columns must be the same.", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
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
                    while (TestIdentityInColumn(sender, col, strNewName + p) != 0) p++;
                    ((DataGridView)sender)[col, row].Value = strNewName + p;                    
                }
                colLocationValidation(sender, e,false);

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
                    while (TestIdentityInColumn(sender, col, strNewName + p) != 0) p++;
                    ((DataGridView)sender)[col, e.RowIndex].Value = strNewName + p;
                    //e.Cancel = true;
                }
                col = 1;
                int minValue = 0;
                res = dgNumberValidating(sender, e.RowIndex, col, minValue);
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
                    while (TestIdentityInColumn(sender, col, strNewName + p) != 0) p++;
                    ((DataGridView)sender)[col, e.RowIndex].Value = strNewName + p;
                    //e.Cancel = true;
                }
                col =1;
                if (CyGeneralParams.GetDGString(sender, e.RowIndex, col) == "")
                {
                    ((DataGridView)sender)[col, e.RowIndex].Value = CyGeneralParams.strSliderType[0];
                }
                col = 2;
                int minValue = 2;
                res = dgNumberValidating(sender, e.RowIndex, col, minValue);
                if ((res == 2) || (res == 1))
                {
                    ((DataGridView)sender)[col, e.RowIndex].Value = minValue;
                }
                col = 3;
                res = dgNumberValidating(sender, e.RowIndex, col, minValue);
                if ((res == 2) || (res == 1))
                {
                    ((DataGridView)sender)[col, e.RowIndex].Value = defResolutions;
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
                int res = dgNumberValidating(sender, e.RowIndex, col, minValue);
                if ((res == 2) || (res == 1))
                {
                    ((DataGridView)sender)[col, e.RowIndex].Value = defResolutions;
                }
                col = 4;
                res = dgNumberValidating(sender, e.RowIndex, col, minValue);
                if ((res == 2) || (res == 1))
                {
                    ((DataGridView)sender)[col, e.RowIndex].Value = defResolutions;
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
                    while (TestIdentityInColumn(sender, col, strNewName + p) != 0) p++;
                    ((DataGridView)sender)[col, e.RowIndex].Value = strNewName + p;
                    //e.Cancel = true;
                }
                int minValue = 2;
                col = 1;
                res = dgNumberValidating(sender, e.RowIndex, col, minValue);
                if ((res == 2) || (res == 1))
                {
                    ((DataGridView)sender)[col, e.RowIndex].Value = minValue;
                }
                col = 2;
                res = dgNumberValidating(sender, e.RowIndex, col, minValue);
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
                if (res != 2)
                    MessageBox.Show("Please input correct Name", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
            return res;

        }
        int dgNumberValidating(object sender,int row,int col,int minValue)
        {
            int res=2;//null value
            try
            {
                if((((DataGridView)sender)[col, row].Value==null))
                    throw new Exception();
                if((((DataGridView)sender)[col, row].Value.ToString()==""))
                    throw new Exception();
                res=1;//wrong Number
                string str=((DataGridView)sender)[col, row].Value.ToString();
                                    //Test Characters
                foreach (char ch in str)
                {
                    if ((ch >= '0') && (ch <= '9')) continue;
                    throw new Exception();
                }
                int val;
                int.TryParse(str,out val);
                if (val >= minValue)
                {
                    res = 0;
                }
                else new Exception();
            }
            catch
            {
                if(res!=2)
                MessageBox.Show("Please input correct Number", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
            return res;

        }
        #endregion

        #region Row Enter
        void dgEnter(object sender, System.Windows.Forms.DataGridViewCellEventArgs e)
        {
            int t_cRow = e.RowIndex;// GetCurrentRowFromDG(((DataGridView)sender));
            if ((t_cRow != -1) && (t_cRow < ((DataGridView)sender).RowCount - 1))
            {
                string name = CyGeneralParams.GetDGString(sender, t_cRow, 0);
                //Select Object Processing
                if (sender == dgButtons)
                {
                    packParams.cyButtons.UnitPropertyGrid.GetProperties(packParams.cyWidgetsList.GetWidget(name, sensorType.Button));
                    //lastType = sensorType.Buttons;
                }
                else if (sender == dgSliders)
                {
                    packParams.cySliders.UnitPropertyGrid.GetProperties(packParams.cyWidgetsList.GetWidget(name, sensorType.Linear_Slider));
                    //lastType = sensorType.Linear_Slider;
                }
                else if (sender == dgTouchpads)
                {
                    packParams.cyTouchPads.UnitPropertyGrid.GetProperties(packParams.cyWidgetsList.GetWidget(name, sensorType.Touchpad_Col));
                    //lastType = sensorType.Touchpads_Col;
                }
                else if (sender == dgMatrixButtons)
                {
                    packParams.cyMatrixButtons.UnitPropertyGrid.GetProperties(packParams.cyWidgetsList.GetWidget(name, sensorType.Matrix_Buttons_Col));
                    //lastType = sensorType.Matrix_Buttons_Col;
                }
                else if (sender == dgProximity)
                {
                    packParams.cyProximity.UnitPropertyGrid.GetProperties(packParams.cyWidgetsList.GetWidget(name, sensorType.Proximity));
                    //lastType = sensorType.Proximity;
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
            if (sender == dgButtons)
            {
                packParams.cyButtons.UnitPropertyGrid.GetProperties(null);
            }
            else if (sender == dgSliders)
            {
                packParams.cySliders.UnitPropertyGrid.GetProperties(null);
            }
            else if (sender == dgTouchpads)
            {
                packParams.cyTouchPads.UnitPropertyGrid.GetProperties(null);
            }
            else if (sender == dgMatrixButtons)
            {
                packParams.cyMatrixButtons.UnitPropertyGrid.GetProperties(null);
            }
            else if (sender == dgProximity)
            {
                packParams.cyButtons.UnitPropertyGrid.GetProperties(null);
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
            //if (e.ColumnIndex == colType.Index)
            {
                dgw.BeginEdit(true);
                if (dgw.EditingControl != null)
                {
                    if (dgw.EditingControl.GetType() == typeof(DataGridViewComboBoxEditingControl))
                    {
                        DataGridViewComboBoxEditingControl cb_dlc = (DataGridViewComboBoxEditingControl)dgw.EditingControl;
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


        int TestIdentityInColumn(object sender,int col, string str)
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

        public static eElSide getSide(object obj)
        {
            eElSide res = eElSide.None;
            if (Convert.ToString(obj) == strItems[0]) res = eElSide.Left;
            if (Convert.ToString(obj) == strItems[1]) res = eElSide.Right;

            return res;
        }
        public static string getString(eElSide obj)
        {
            string res = strItems[0];
            if (obj == eElSide.Right) res = strItems[1];
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


