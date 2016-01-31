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

namespace VoltageFaultDetector_v1_0
{
    public partial class CyToolStrip : UserControl
    {
        public CyParameters m_params;
        public DataGridView m_dgv;

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

        public void ExportRows()
        {
            saveFileDialog.Title = Resources.FilterDialogExportTitle;
            saveFileDialog.DefaultExt = Resources.FileDialogExt;
            saveFileDialog.Filter = Resources.FileDialogFilter;
            saveFileDialog.FilterIndex = 1;
            saveFileDialog.RestoreDirectory = true;

            if (saveFileDialog.ShowDialog() == DialogResult.OK)
            {
                CyImportExport.SaveToFile(saveFileDialog.FileName, CyImportExport.Export(m_params.VoltagesTable, 
                    m_params.m_voltagesTab.GetVoltageColNames()));
            }
        }

        private void tsbImport_Click(object sender, EventArgs e)
        {
            ImportRows();
        }

        public void ImportRows()
        {
            openFileDialog.Title = Resources.FilterDialogImportTitle;
            openFileDialog.DefaultExt = Resources.FileDialogExt;
            openFileDialog.Filter = Resources.FileDialogFilter;
            openFileDialog.FilterIndex = 1;
            openFileDialog.RestoreDirectory = true;
            openFileDialog.CheckFileExists = true;
            openFileDialog.CheckPathExists = true;
            openFileDialog.Multiselect = false;

            if (openFileDialog.ShowDialog() == DialogResult.OK)
            {
                DialogResult dr = MessageBox.Show(Resources.ImportConfirmation, Resources.MsgWarningTitle,
                    MessageBoxButtons.YesNo, MessageBoxIcon.Warning);
                if (dr == DialogResult.Yes)
                {
                    string fileContent = CyImportExport.GetFileText(openFileDialog.FileName);

                    m_params.Import(this.Parent, fileContent, false);
                }
            }
        }

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
                Clipboard.SetText(CyImportExport.Export(exportTable, m_params.m_voltagesTab.GetVoltageColNames()));
            }
        }

        private void tsbPaste_Click(object sender, EventArgs e)
        {
            PasteRows();
        }

        public void PasteRows()
        {
            string content = Clipboard.GetText();

            if (m_params.Import(this.Parent, content, true) == false)
            {
                MessageBox.Show(Resources.InvalidDataInClipboard, Resources.MsgErrorTitle, MessageBoxButtons.OK,
                    MessageBoxIcon.Error);
            }
        }

        public void ChangeCopyPasteEnabling(bool copyPasteButtonsEnabled)
        {
            tsbCopy.Enabled = copyPasteButtonsEnabled;
            tsbPaste.Enabled = copyPasteButtonsEnabled;
        }
    }
}
