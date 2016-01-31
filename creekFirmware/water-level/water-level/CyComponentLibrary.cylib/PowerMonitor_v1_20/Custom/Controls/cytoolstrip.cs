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
using System.IO;

namespace PowerMonitor_v1_20
{
    public partial class CyToolStrip : UserControl
    {
        public CyParameters m_params;
        public DataGridView m_dgv;

        private const string FILE_EXTENSION = "csv";
        private const string FILE_FILTER = "CSV files (*.csv)|*.csv|All files (*.*)|*.*";

        public CyToolStrip()
        {
            InitializeComponent();
        }
        public void SetParameters(CyParameters param)
        {
            m_params = param;
        }

        private void tsbExport_Click(object sender, EventArgs e)
        {
            ExportRows();
        }

        #region Export
        public void ExportRows()
        {
            saveFileDialog.Title = Resources.ExportToFileTitle;
            SetFileDialogData(saveFileDialog);

            if (saveFileDialog.ShowDialog() == DialogResult.OK)
            {
                if (this.Parent is CyVoltagesTab)
                    CyImportExport.SaveToFile(saveFileDialog.FileName, CyImportExport.Export(m_params, 
                        m_params.VoltagesTable));
                if (this.Parent is CyCurrentsTab)
                    CyImportExport.SaveToFile(saveFileDialog.FileName, CyImportExport.Export(m_params,
                        m_params.CurrentsTable));
                if (this.Parent is CyAuxiliaryTab)
                    CyImportExport.SaveToFile(saveFileDialog.FileName, CyImportExport.Export(m_params,
                        m_params.AuxTable));
            }
        }

        public void ExportAll()
        {
            saveFileDialog.Title = Resources.ExportAllTitle;
            SetFileDialogData(saveFileDialog);

            if (saveFileDialog.ShowDialog() == DialogResult.OK)
            {
                string voltageTab = CyImportExport.Export(m_params, m_params.VoltagesTable);
                string currentsTab = CyImportExport.Export(m_params, m_params.CurrentsTable);
                string auxTab = CyImportExport.Export(m_params, m_params.AuxTable);

                string fileContent = string.Format("{0}{3}{1}{3}{2}", voltageTab, currentsTab, auxTab, 
                    Environment.NewLine);

                CyImportExport.SaveToFile(saveFileDialog.FileName, fileContent);
            }
        }

        private void SetFileDialogData(FileDialog dialog)
        {
            dialog.DefaultExt = FILE_EXTENSION;
            dialog.Filter = FILE_FILTER;
            dialog.FilterIndex = 0;
            dialog.RestoreDirectory = true;
            dialog.FileName = m_params.m_inst.InstanceName;
        }
        #endregion

        private void tsbCopy_Click(object sender, EventArgs e)
        {
            CopyRows();
        }

        public void CopyRows()
        {
            if (this.Parent is CyVoltagesTab)
            {
                List<CyVoltagesTableRow> exportTable = new List<CyVoltagesTableRow>();
                for (int i = 0; i < m_dgv.RowCount; i++)
                {
                    if (m_dgv.Rows[i].Selected == true)
                        exportTable.Add(m_params.VoltagesTable[i]);
                }
                Clipboard.SetText(CyImportExport.Export(m_params, exportTable));
            }
            if (this.Parent is CyCurrentsTab)
            {
                List<CyCurrentsTableRow> exportTable = new List<CyCurrentsTableRow>();
                for (int i = 0; i < m_dgv.RowCount; i++)
                {
                    if (m_dgv.Rows[i].Selected == true)
                        exportTable.Add(m_params.CurrentsTable[i]);
                }
                Clipboard.SetText(CyImportExport.Export(m_params, exportTable));
            }
            if (this.Parent is CyAuxiliaryTab)
            {
                List<CyAuxTableRow> exportTable = new List<CyAuxTableRow>();
                for (int i = 0; i < m_dgv.RowCount; i++)
                {
                    if (m_dgv.Rows[i].Selected == true)
                        exportTable.Add(m_params.AuxTable[i]);
                }
                Clipboard.SetText(CyImportExport.Export(m_params, exportTable));
            }
        }

        private void tsbImport_Click(object sender, EventArgs e)
        {
            ImportRows();
        }

        public void ImportRows()
        {
            openFileDialog.Title = Resources.ImportFromFileTitle;
            openFileDialog.CheckFileExists = true;
            openFileDialog.CheckPathExists = true;
            SetFileDialogData(openFileDialog);

            if (openFileDialog.ShowDialog() == DialogResult.OK)
            {
                DialogResult dr = MessageBox.Show(Resources.ImportConfirmation, typeof(CyCustomizer).Namespace,
                    MessageBoxButtons.YesNo, MessageBoxIcon.Warning);
                if (dr == DialogResult.Yes)
                {
                    string fileContent = CyImportExport.GetFileText(openFileDialog.FileName);

                    m_params.Import(this.Parent, fileContent, false);
                }
            }
        }

        private void tsbPaste_Click(object sender, EventArgs e)
        {
            PasteRows();
        }

        public void PasteRows()
        {
            string content = Clipboard.GetText();
            m_params.Import(this.Parent, content, true);
        }

        public void ChangeCopyPasteEnabling(bool copyPasteButtonsEnabled)
        {
            tsbCopy.Enabled = copyPasteButtonsEnabled;
            tsbPaste.Enabled = copyPasteButtonsEnabled;
        }

        private void tsbImportAll_Click(object sender, EventArgs e)
        {
            ImportAll();
        }

        public void ImportAll()
        {
            openFileDialog.Title = Resources.ImportAllTitle;
            openFileDialog.CheckFileExists = true;
            openFileDialog.CheckPathExists = true;
            SetFileDialogData(openFileDialog);

            if (openFileDialog.ShowDialog() == DialogResult.OK)
            {
                DialogResult dr = MessageBox.Show(Resources.ImportConfirmation, typeof(CyCustomizer).Namespace,
                    MessageBoxButtons.YesNo, MessageBoxIcon.Warning);

                if (dr == DialogResult.Yes)
                {
                    List<StringBuilder> listTables = CyImportExport.GetTables(CyImportExport.GetFileText(openFileDialog.FileName));

                    if (listTables.Count >= 3)
                    {
                        m_params.Import(m_params.m_voltagesTab, listTables[0].ToString(), false);

                        m_params.Import(m_params.m_currentsTab, listTables[1].ToString(), false);

                        m_params.Import(m_params.m_auxTab, listTables[2].ToString(), false);
                    }
                    else
                        MessageBox.Show(Resources.InvalidDataFormat, typeof(CyCustomizer).Namespace, 
                            MessageBoxButtons.OK,  MessageBoxIcon.Warning);                    
                }
            }
        }

        private void tsbExportAll_Click(object sender, EventArgs e)
        {
            ExportAll();
        }

        
    }
}
