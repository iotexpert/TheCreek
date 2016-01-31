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

namespace SMBusSlave_v1_0
{
    public partial class CyPmBusCmdsTab : CyTabWrapper
    {
        ICyTabbedParamEditor m_editor;

        #region CyTabWrapper members
        public override string TabName
        {
            get { return "PMBus Commands"; }
        }
        #endregion

        #region Constructor(s)
        public CyPmBusCmdsTab(CyParameters parameters, ICyTabbedParamEditor editor)
            : base(parameters)
        {
            m_editor = editor;
            m_params.m_pmBusCmdsTab = this;
            InitializeComponent();

            // Initialize wrapper objects
            m_wrapperDataGridView = dgv;
            m_wrapperDataGridKeyColumIndex = colReference.Index;
            m_wrapperToolStrip = cyToolStrip1;

            // Initialize toolstrip            
            cyToolStrip1.m_params = parameters;
            cyToolStrip1.m_dgv = dgv;

            EventHandler hideCommandsHandler = delegate (object sender, EventArgs e)
            {
                if (m_params.HideDisabledPMBusCommands != cyToolStrip1.tsbHideUncheckedCommands.Checked)
                {
                    m_params.HideDisabledPMBusCommands = cyToolStrip1.tsbHideUncheckedCommands.Checked;
                    if (m_params.m_globalEditMode)
                        for (int i = 0; i < dgv.RowCount; i++)
                        {
                            dgv.SuspendLayout();
                            UpdateRowVisibility(i);
                            dgv.ResumeLayout();
                        }                  
                }
            };
            cyToolStrip1.SetPMBusView(hideCommandsHandler);

            CyParameters.PMBusCmdsTableHeader = CyParameters.GetColNames(dgv);

            // Fill grid comboboxes
            colType.Items.Clear();
            colType.Items.AddRange(CyEnumConverter.GetEnumDescList(typeof(CyECmdType)));
            colReadConfig.Items.Clear();
            colReadConfig.Items.AddRange(CyEnumConverter.GetEnumDescList(typeof(CyEReadWriteConfigType)));
            colWriteConfig.Items.Clear();
            colWriteConfig.Items.AddRange(CyEnumConverter.GetEnumDescList(typeof(CyEReadWriteConfigType)));
            colFormat.Items.Clear();
            colFormat.Items.AddRange(CyEnumConverter.GetEnumDescList(typeof(CyEFormatType)));

            // Initialize data grid view
            CyParameters.InitializeDataGrid(dgv);
            colCommandName.SortMode = DataGridViewColumnSortMode.Automatic;
            colCode.SortMode = DataGridViewColumnSortMode.Automatic;
            CyParameters.SetReadOnlyState(colCommandName, true);
            CyParameters.SetReadOnlyState(colCode, true);
            CyParameters.SetReadOnlyState(colType, true);
            CyParameters.SetReadOnlyState(colSize, true);

            m_editableCols = new List<int>(new int[]
            {
                colEnable.Index,
                colFormat.Index,
                colSize.Index,
                colPaged.Index,
                colReadConfig.Index,
                colWriteConfig.Index
            });

            dgv.CurrentCellDirtyStateChanged += delegate(object sender, EventArgs e)
            {
                CommitCellValueImmediately(sender as DataGridView);
            };
            dgv.CellValueChanged += new DataGridViewCellEventHandler(dataGridView_CellValueChanged);

            UpdateUIFromTable();
        }
        #endregion

        #region Tab visibility
        public void SetPMBusCmdsTabVisibility(CyEModeSelType selectedMode)
        {
            if (selectedMode == CyEModeSelType.SMBUS_SLAVE)
            {
                m_editor.HideCustomPage(Resources.PMBusCmdsTabDisplayName);
            }
            else
                m_editor.ShowCustomPage(Resources.PMBusCmdsTabDisplayName);

            // Hide Import All functionality if there is only one table visible
            m_params.m_customCmdsTab.ShowHideImportExportAllFeature(selectedMode != CyEModeSelType.SMBUS_SLAVE);
        }
        #endregion

        #region Update table/datagrid
        public override void UpdateUIFromTable()
        {
            bool prevGEM = m_params.m_globalEditMode;
            m_params.m_globalEditMode = false;

            cyToolStrip1.tsbHideUncheckedCommands.Checked = m_params.HideDisabledPMBusCommands;

            // It optimize refresh complexity
            bool fullRefresh = false;

            if (CyCmdData.PMBusCmdList.Length != dgv.Rows.Count)
            {
                dgv.Rows.Clear();
                fullRefresh = true;
            }

            for (int row = 0; row < CyCmdData.PMBusCmdList.Length; row++)
            {
                if (fullRefresh)
                {
                    object[] newRow = new object[dgv.ColumnCount];
                    dgv.Rows.Add(newRow);
                }

                UpdateUIFromTable(row);
            }

            m_params.m_globalEditMode = prevGEM;
        }

        public void UpdateTableFromUI()
        {
            for (int row = 0; row < CyCmdData.PMBusCmdList.Length; row++)
            {
                UpdateTableRowFromUI(row);
            }
        }


        public override void UpdateUIFromTable(int row)
        {
            dgv[colReference.Index, row].Value = m_params.PmBusTable[row];
            dgv[colEnable.Index, row].Value = m_params.PmBusTable[row].m_enable;
            dgv[colCode.Index, row].Value = m_params.PmBusTable[row].m_code;
            dgv[colCommandName.Index, row].Value = m_params.PmBusTable[row].Name;
            dgv[colType.Index, row].Value = CyEnumConverter.GetEnumDesc(m_params.PmBusTable[row].Type);
            dgv[colSize.Index, row].Value = m_params.PmBusTable[row].Size;
            dgv[colFormat.Index, row].Value = CyEnumConverter.GetEnumDesc(m_params.PmBusTable[row].m_format);
            dgv[colPaged.Index, row].Value = m_params.PmBusTable[row].m_paged;
            dgv[colReadConfig.Index, row].Value = CyEnumConverter.GetEnumDesc(m_params.PmBusTable[row].ReadConfig);
            dgv[colWriteConfig.Index, row].Value = CyEnumConverter.GetEnumDesc(m_params.PmBusTable[row].WriteConfig);

            // Set Size column as editable for Block and ProcessCall types
            if (CyCmdData.GetSpecDataByCode(m_params.PmBusTable[row].m_code).m_sizeUnlocked)
            {
                CyParameters.SetReadOnlyState(dgv, colSize, row, false);
            }

            DataGridViewComboBoxCell writeConfigCell =
                (DataGridViewComboBoxCell)dgv.Rows[row].Cells[colWriteConfig.Index];
            DataGridViewComboBoxCell readConfigCell =
                (DataGridViewComboBoxCell)dgv.Rows[row].Cells[colReadConfig.Index];

            // All fields of PAGE and QUERY commands should be readonly
            if (CyCustomTable.IsCmdPageOrQuery(m_params.PmBusTable[row].Name))
            {
                DataGridViewCheckBoxCell enableCell = (DataGridViewCheckBoxCell)dgv.Rows[row].Cells[colEnable.Index];
                DataGridViewComboBoxCell formatCell = (DataGridViewComboBoxCell)dgv.Rows[row].Cells[colFormat.Index];
                DataGridViewCheckBoxCell pagedCell = (DataGridViewCheckBoxCell)dgv.Rows[row].Cells[colPaged.Index];

                CyParameters.SetCellReadOnlyState(enableCell, true);
                CyParameters.SetCellReadOnlyState(formatCell, true);
                CyParameters.SetCellReadOnlyState(pagedCell, true);
                CyParameters.SetCellReadOnlyState(writeConfigCell, true);
                CyParameters.SetCellReadOnlyState(readConfigCell, true);
            }

            object group = CyCmdData.GetCmdGroupByCode(m_params.PmBusTable[row].m_code);

            if (group != null)
            {
                switch ((CyECmdGroup)group)
                {
                    case CyECmdGroup.GROUP0:
                        //  WRITE                READ
                        // (Auto,Manual,None  /  None)
                        readConfigCell.Items.Remove(CyEnumConverter.GetEnumDesc(CyEReadWriteConfigType.Auto));
                        readConfigCell.Items.Remove(CyEnumConverter.GetEnumDesc(CyEReadWriteConfigType.Manual));
                        break;
                    case CyECmdGroup.GROUP1:
                        // (Auto,Manual,None  /  Auto,Manual,None)
                        // Nothing to restrict in this group
                        break;
                    case CyECmdGroup.GROUP2:
                        // (Manual,None       /  None)
                        writeConfigCell.Items.Remove(CyEnumConverter.GetEnumDesc(CyEReadWriteConfigType.Auto));
                        readConfigCell.Items.Remove(CyEnumConverter.GetEnumDesc(CyEReadWriteConfigType.Auto));
                        readConfigCell.Items.Remove(CyEnumConverter.GetEnumDesc(CyEReadWriteConfigType.Manual));
                        break;
                    case CyECmdGroup.GROUP3:
                        // (None              /  Auto,Manual,None)
                        writeConfigCell.Items.Remove(CyEnumConverter.GetEnumDesc(CyEReadWriteConfigType.Auto));
                        writeConfigCell.Items.Remove(CyEnumConverter.GetEnumDesc(CyEReadWriteConfigType.Manual));
                        break;
                    case CyECmdGroup.GROUP4:
                        // (Auto,Manual,None  /  Manual,None)
                        readConfigCell.Items.Remove(CyEnumConverter.GetEnumDesc(CyEReadWriteConfigType.Auto));
                        break;
                    case CyECmdGroup.GROUP5:
                        // (None              /  Manual,None)
                        writeConfigCell.Items.Remove(CyEnumConverter.GetEnumDesc(CyEReadWriteConfigType.Auto));
                        writeConfigCell.Items.Remove(CyEnumConverter.GetEnumDesc(CyEReadWriteConfigType.Manual));
                        readConfigCell.Items.Remove(CyEnumConverter.GetEnumDesc(CyEReadWriteConfigType.Auto));
                        break;
                    case CyECmdGroup.SPECIFIC:
                        if (m_params.PmBusTable[row].Name == CyCustomTable.QUERY)
                        {
                            // (None              /  Auto,Manual,None)
                            writeConfigCell.Items.Remove(CyEnumConverter.GetEnumDesc(CyEReadWriteConfigType.Auto));
                            writeConfigCell.Items.Remove(CyEnumConverter.GetEnumDesc(CyEReadWriteConfigType.Manual));
                        }
                        break;
                    default:
                        break;
                }
            }

            UpdateRowVisibility(row);
        }

        private void UpdateRowVisibility(int row)
        {
            if(row<dgv.RowCount && row>-1)
            try
            {
                bool enabled = Convert.ToBoolean(dgv[colEnable.Index, row].Value);
                //Hide row in not enabled
                dgv.Rows[row].Visible = m_params.HideDisabledPMBusCommands ? enabled : true;
            }
            catch { dgv.Rows[row].Visible = true; }
        }

        public override void UpdateTableRowFromUI(int row)
        {
            byte code = Convert.ToByte(dgv[colCode.Index, row].Value);
            int index = CyCmdData.GetIndexByCode(code);

            m_params.PmBusTable[index].m_enable = CyParameters.CellToBool(dgv[colEnable.Index, row].Value);
            m_params.PmBusTable[index].m_format = (CyEFormatType)CyEnumConverter.GetEnumValue(dgv[colFormat.Index,
                row].Value, typeof(CyEFormatType));
            m_params.PmBusTable[index].Size = CyParameters.ParseNullableByte(dgv[colSize.Index, row].Value);
            m_params.PmBusTable[index].m_paged = CyParameters.CellToBool(dgv[colPaged.Index, row].Value);
            m_params.PmBusTable[index].ReadConfig = (CyEReadWriteConfigType)CyEnumConverter.GetEnumValue(
                dgv[colReadConfig.Index, row].Value, typeof(CyEReadWriteConfigType));
            m_params.PmBusTable[index].WriteConfig = (CyEReadWriteConfigType)CyEnumConverter.GetEnumValue(
                dgv[colWriteConfig.Index, row].Value, typeof(CyEReadWriteConfigType));
        }
        #endregion

        #region Event handlers
        private void dataGridView_CellValueChanged(object sender, DataGridViewCellEventArgs e)
        {
            if (m_params.m_globalEditMode)
            {
                UpdateTableRowFromUI(e.RowIndex);
                m_params.SetPmBusTable();
                ValidateRow(e.RowIndex);
                if ((e.ColumnIndex == colCode.Index) || (e.ColumnIndex == colEnable.Index))
                {
                    ValidateCodes();
                    m_params.m_customCmdsTab.ValidateCodes(); // update error providers on Custom tab
                }
            }
        }

        private void dataGridView_CellFormatting(object sender, DataGridViewCellFormattingEventArgs e)
        {
            if (e.Value != null)
            {
                if (e.ColumnIndex == colCode.Index)
                {
                    try
                    {
                        e.Value = CyParameters.CellConvertHex(e.Value);
                        e.FormattingApplied = true;
                    }
                    catch { }
                }
            }
        }
        #endregion

        #region Validation
        protected override bool RangeCheck(int rowIndex, int colIndex)
        {
            if (m_editableCols.Contains(colIndex) == false) return true;

            double min = 0;
            double max = 0;
            string message = string.Empty;
            double? currCellValue = null;
            if (dgv[colReference.Index, rowIndex].Value != null)
            {
                CyPMBusTableRow pmBusTableRow = (CyPMBusTableRow)dgv[colReference.Index, rowIndex].Value;

                if (colIndex == colSize.Index)
                {
                    min = byte.MinValue;
                    max = byte.MaxValue;
                    message = Resources.SizeOutOfRange;
                    currCellValue = pmBusTableRow.Size;
                }

                if (m_editableCols.Contains(colIndex) && dgv.Columns[colIndex].ReadOnly == false)
                {
                    message = CyParameters.IsValueInRange(currCellValue, dgv[colIndex, rowIndex].Value, min, max,
                        message, false);
                }
            }
            dgv[colIndex, rowIndex].ErrorText = message;
            return String.IsNullOrEmpty(message);
        }

        public override void ValidateCodes()
        {
            if (m_params.Mode == CyEModeSelType.PMBUS_SLAVE)
            {
                // Reset UI error providers
                for (int i = 0; i < dgv.RowCount; i++)
                {
                    if (dgv[colCode.Index, i].ErrorText != string.Empty)
                        dgv[colCode.Index, i].ErrorText = string.Empty;
                }

                // Compare to Custom table
                for (int i = 0; i < m_params.CustomTable.Count; i++)
                {
                    for (int j = 0; j < m_params.PmBusTable.Count; j++)
                    {
                        if ((m_params.CustomTable[i].m_code == m_params.PmBusTable[j].m_code) &&
                            (m_params.CustomTable[i].m_enable && m_params.PmBusTable[j].m_enable))
                        {
                            for (int k = 0; k < dgv.RowCount; k++)
                            {
                                if (dgv[colReference.Index, k].Value == m_params.PmBusTable[j])
                                    dgv[colCode.Index, k].ErrorText = string.Format(Resources.CmdCodeUsedPMBusMessage,
                                        Resources.CustomCmdsTabDisplayName);

                            }
                        }
                    }
                }
            }
        }
        #endregion
    }
}
