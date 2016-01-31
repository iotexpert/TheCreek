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

namespace PowerMonitor_v1_10
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
            saveFileDialog.DefaultExt = FILE_EXTENSION;
            saveFileDialog.Filter = FILE_FILTER;
            saveFileDialog.FilterIndex = 1;
            saveFileDialog.RestoreDirectory = true;

            if (saveFileDialog.ShowDialog() == DialogResult.OK)
            {
                if (this.Parent is CyVoltagesTab)
                    CyImportExport.SaveToFile(saveFileDialog.FileName, CyImportExport.Export(m_params.VoltagesTable));
                if (this.Parent is CyCurrentsTab)
                    CyImportExport.SaveToFile(saveFileDialog.FileName, CyImportExport.Export(m_params.CurrentsTable));
                if (this.Parent is CyAuxiliaryTab)
                    CyImportExport.SaveToFile(saveFileDialog.FileName, CyImportExport.Export(m_params.AuxTable));
            }
        }

        public void ExportAll()
        {
            saveFileDialog.Title = Resources.ExportAllTitle;
            saveFileDialog.DefaultExt = FILE_EXTENSION;
            saveFileDialog.Filter = FILE_FILTER;
            saveFileDialog.FilterIndex = 1;
            saveFileDialog.RestoreDirectory = true;

            if (saveFileDialog.ShowDialog() == DialogResult.OK)
            {
                foreach (string item in Directory.GetFiles(Path.GetDirectoryName(saveFileDialog.FileName)))
                {
                    if (Path.GetFileNameWithoutExtension(item).Replace(CyImportExport.VOLTAGES_POSTFIX, 
                        string.Empty).Replace(CyImportExport.CURRENTS_POSTFIX, string.Empty).Replace(
                        CyImportExport.AUX_POSTFIX, string.Empty) == 
                        Path.GetFileNameWithoutExtension(saveFileDialog.FileName))
                    {
                        if (MessageBox.Show(Resources.FileExists, Resources.ComponentName, MessageBoxButtons.YesNo,
                            MessageBoxIcon.Warning) == DialogResult.Yes)
                            break;
                        else
                            return;
                    }
                }
                string fileName = Path.GetDirectoryName(saveFileDialog.FileName) + "\\" +
                    Path.GetFileNameWithoutExtension(saveFileDialog.FileName) + CyImportExport.VOLTAGES_POSTFIX +
                    Path.GetExtension(saveFileDialog.FileName);
                CyImportExport.SaveToFile(fileName, CyImportExport.Export(m_params.VoltagesTable));

                fileName = Path.GetDirectoryName(saveFileDialog.FileName) + "\\" +
                    Path.GetFileNameWithoutExtension(saveFileDialog.FileName) + CyImportExport.CURRENTS_POSTFIX +
                    Path.GetExtension(saveFileDialog.FileName);
                CyImportExport.SaveToFile(fileName, CyImportExport.Export(m_params.CurrentsTable));

                fileName = Path.GetDirectoryName(saveFileDialog.FileName) + "\\" +
                    Path.GetFileNameWithoutExtension(saveFileDialog.FileName) + CyImportExport.AUX_POSTFIX +
                    Path.GetExtension(saveFileDialog.FileName);
                CyImportExport.SaveToFile(fileName, CyImportExport.Export(m_params.AuxTable));
            }
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
                Clipboard.SetText(CyImportExport.Export(exportTable));
            }
            if (this.Parent is CyCurrentsTab)
            {
                List<CyCurrentsTableRow> exportTable = new List<CyCurrentsTableRow>();
                for (int i = 0; i < m_dgv.RowCount; i++)
                {
                    if (m_dgv.Rows[i].Selected == true)
                        exportTable.Add(m_params.CurrentsTable[i]);
                }
                Clipboard.SetText(CyImportExport.Export(exportTable));
            }
            if (this.Parent is CyAuxiliaryTab)
            {
                List<CyAuxTableRow> exportTable = new List<CyAuxTableRow>();
                for (int i = 0; i < m_dgv.RowCount; i++)
                {
                    if (m_dgv.Rows[i].Selected == true)
                        exportTable.Add(m_params.AuxTable[i]);
                }
                Clipboard.SetText(CyImportExport.Export(exportTable));
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
            openFileDialog.DefaultExt = FILE_EXTENSION;
            openFileDialog.Filter = FILE_FILTER;
            openFileDialog.FilterIndex = 1;
            openFileDialog.RestoreDirectory = true;
            openFileDialog.Multiselect = false;

            if (openFileDialog.ShowDialog() == DialogResult.OK)
            {
                DialogResult dr = MessageBox.Show(Resources.ImportConfirmation, Resources.ComponentName,
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
            openFileDialog.DefaultExt = FILE_EXTENSION;
            openFileDialog.Filter = FILE_FILTER;
            openFileDialog.FilterIndex = 1;
            openFileDialog.RestoreDirectory = true;
            openFileDialog.Multiselect = true;

            if (openFileDialog.ShowDialog() == DialogResult.OK)
            {
                DialogResult dr = MessageBox.Show(Resources.ImportConfirmation, Resources.ComponentName,
                    MessageBoxButtons.YesNo, MessageBoxIcon.Warning);

                if (dr == DialogResult.Yes)
                {
                    string fileNameWithoutPostfix = string.Empty;
                    if (Path.GetFileNameWithoutExtension(openFileDialog.FileName).EndsWith(
                        CyImportExport.VOLTAGES_POSTFIX))
                    {
                        fileNameWithoutPostfix = Path.GetFullPath(openFileDialog.FileName);
                        fileNameWithoutPostfix = fileNameWithoutPostfix.Replace(CyImportExport.VOLTAGES_POSTFIX, 
                            string.Empty);
                    }
                    else if (Path.GetFileNameWithoutExtension(openFileDialog.FileName).EndsWith(
                        CyImportExport.CURRENTS_POSTFIX))
                    {
                        fileNameWithoutPostfix = Path.GetFullPath(openFileDialog.FileName);
                        fileNameWithoutPostfix = fileNameWithoutPostfix.Replace(CyImportExport.CURRENTS_POSTFIX, 
                            string.Empty);
                    }
                    else if (Path.GetFileNameWithoutExtension(openFileDialog.FileName).EndsWith(
                        CyImportExport.AUX_POSTFIX))
                    {
                        fileNameWithoutPostfix = Path.GetFullPath(openFileDialog.FileName);
                        fileNameWithoutPostfix = fileNameWithoutPostfix.Replace(CyImportExport.AUX_POSTFIX, 
                            string.Empty);
                    }

                    if (string.IsNullOrEmpty(fileNameWithoutPostfix) == false)
                    {
                        string fileToOpen = Path.GetDirectoryName(fileNameWithoutPostfix) + "\\" +
                            Path.GetFileNameWithoutExtension(fileNameWithoutPostfix) + CyImportExport.VOLTAGES_POSTFIX +
                            Path.GetExtension(openFileDialog.FileName);
                        string fileContent = CyImportExport.GetFileText(fileToOpen);
                        m_params.Import(m_params.m_voltagesTab, fileContent, false);

                        fileToOpen = Path.GetDirectoryName(fileNameWithoutPostfix) + "\\" +
                            Path.GetFileNameWithoutExtension(fileNameWithoutPostfix) + CyImportExport.CURRENTS_POSTFIX +
                            Path.GetExtension(openFileDialog.FileName);
                        fileContent = CyImportExport.GetFileText(fileToOpen);
                        m_params.Import(m_params.m_currentsTab, fileContent, false);

                        fileToOpen = Path.GetDirectoryName(fileNameWithoutPostfix) + "\\" +
                            Path.GetFileNameWithoutExtension(fileNameWithoutPostfix) + CyImportExport.AUX_POSTFIX +
                            Path.GetExtension(openFileDialog.FileName);
                        fileContent = CyImportExport.GetFileText(fileToOpen);
                        m_params.Import(m_params.m_auxTab, fileContent, false);
                    }
                }
            }
        }

        private void tsbExportAll_Click(object sender, EventArgs e)
        {
            ExportAll();
        }

        
    }
}
