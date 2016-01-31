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

namespace SMBusSlave_v1_0
{
    public partial class CyToolStrip : UserControl
    {
        public CyParameters m_params;
        public DataGridView m_dgv;

        const string FILTER_XML = "XML files (*.xml)|*.xml|All files (*.*)|*.*";
        const string FILE_EXTENSION_XML = "xml";

        const string FILTER_CSV = "CSV files (*.csv)|*.csv|All files (*.*)|*.*";
        const string FILE_EXTENSION_CSV = "csv";

        public bool GeneralView
        {
            get { return toolStripGeneral.Visible; }
            set
            {
                toolStripGeneral.Visible = value;
                toolStripExport.Visible = value == false;
            }
        }

        public CyToolStrip()
        {
            InitializeComponent();
            tsbLoad.Image = Resources.Load;
            tsbSave.Image = Resources.Save;

            tsbSave.Click += new EventHandler(tsbSave_Click);
            tsbLoad.Click += new EventHandler(tsbLoad_Click);

            tsbExport.Click += new EventHandler(tsbExport_Click);
            tsbImport.Click += new EventHandler(tsbImport_Click);

            tsbExportAll.Click += new EventHandler(tsbExportAll_Click);
            tsbImportAll.Click += new EventHandler(tsbImportAll_Click);

            tsbCopy.Click += new EventHandler(tsbCopy_Click);
            tsbPaste.Click += new EventHandler(tsbPaste_Click);

            GeneralView = true;
        }

        public void SetPMBusView(EventHandler handl)
        {
            tsbHideUncheckedCommands.Visible = true;
            tssPMBus.Visible = true;
            tsbHideUncheckedCommands.CheckedChanged += handl;
        }

        public void ShowHideImportExportAllFeature(bool show)
        {
            tsbImportAll.Visible = show;
            tsbExportAll.Visible = show;
        }

        void tsbPaste_Click(object sender, EventArgs e)
        {
            if ((sender as ToolStripItem).Visible == false) return;

            string content = Clipboard.GetText();

            CyImportExport.Import(this.Parent as CyTabWrapper, m_params, content, true);
        }

        void tsbCopy_Click(object sender, EventArgs e)
        {
            if ((sender as ToolStripItem).Visible == false) return;

            List<object> exportTable = new List<object>();

            for (int i = 0; i < m_dgv.RowCount; i++)
            {
                if (m_dgv.Rows[i].Selected == true)
                {
                    if (this.Parent is CyCustomCmdsTab && i < m_params.CustomTable.Count)
                        exportTable.Add(m_params.CustomTable[i]);
                    else if (this.Parent is CyPmBusCmdsTab)
                        exportTable.Add(m_params.PmBusTable[i]);
                }
            }
            string copyStr = CyImportExport.Export(m_params, exportTable, CyParameters.GetColNames(m_dgv));
            if (string.IsNullOrEmpty(copyStr) == false)
                Clipboard.SetText(copyStr);
        }

        void tsbImportAll_Click(object sender, EventArgs e)
        {
            if ((sender as ToolStripItem).Visible == false) return;
            openFileDialog.Title = Resources.ImportAllTitle;
            openFileDialog.CheckFileExists = true;
            openFileDialog.CheckPathExists = true;
            openFileDialog.DefaultExt = FILE_EXTENSION_CSV;
            openFileDialog.Filter = FILTER_CSV;
            openFileDialog.FilterIndex = 1;
            openFileDialog.RestoreDirectory = true;
            openFileDialog.Multiselect = true;

            if (openFileDialog.ShowDialog() == DialogResult.OK)
            {
                DialogResult dr = MessageBox.Show(Resources.ImportConfirmation, m_params.m_inst.ComponentName,
                    MessageBoxButtons.YesNo, MessageBoxIcon.Warning);

                if (dr == DialogResult.Yes)
                {
                    List<StringBuilder> listTables = GetTables(CyParameters.GetFileText(openFileDialog.FileName));

                    if (listTables.Count >= 2)
                    {
                        Cursor actualCursor = Cursor.Current;
                        Cursor.Current = Cursors.WaitCursor;
                        CyImportExport.Import(m_params.m_pmBusCmdsTab, m_params, listTables[0].ToString(), false);
                        CyImportExport.Import(m_params.m_customCmdsTab, m_params, listTables[1].ToString(), false);
                        Cursor.Current = actualCursor;
                    }
                    else
                        MessageBox.Show(Resources.InvalidDataFormat, "Error", MessageBoxButtons.OK,
                            MessageBoxIcon.Warning);
                }
            }
        }

        List<StringBuilder> GetTables(string fileContent)
        {
            StringReader data = new StringReader(fileContent);
            List<StringBuilder> listTables = new List<StringBuilder>();
            int pos = 0;
            string line = data.ReadLine();

            while (line != null)
            {
                listTables.Add(new StringBuilder());

                //Remove empty lines
                while (line == string.Empty) line = data.ReadLine();

                //Get One table
                while (string.IsNullOrEmpty(line) == false)
                {
                    listTables[pos].AppendLine(line);

                    line = data.ReadLine();
                }

                pos++;
                line = data.ReadLine();
            }

            return listTables;
        }

        void tsbExportAll_Click(object sender, EventArgs e)
        {
            if ((sender as ToolStripItem).Visible == false) return;

            saveFileDialog.Title = Resources.ExportAllTitle;
            saveFileDialog.DefaultExt = FILE_EXTENSION_CSV;
            saveFileDialog.Filter = FILTER_CSV;
            saveFileDialog.FilterIndex = 1;
            saveFileDialog.RestoreDirectory = true;
            saveFileDialog.FileName = m_params.m_inst.InstanceName;

            if (saveFileDialog.ShowDialog() == DialogResult.OK)
            {
                List<object> exportTable = new List<object>();

                exportTable = new List<object>();
                foreach (object item in m_params.PmBusTable)
                    exportTable.Add(item);
                string pmBusTable = CyImportExport.Export(m_params, exportTable, CyParameters.PMBusCmdsTableHeader);

                exportTable.Clear();
                foreach (object item in m_params.CustomTable)
                    exportTable.Add(item);
                string customTable = CyImportExport.Export(m_params, exportTable, CyParameters.CustomTableHeader);

                CyParameters.SaveToFile(saveFileDialog.FileName, string.Format("{0}\n{1}", pmBusTable, customTable));
            }
        }

        void tsbImport_Click(object sender, EventArgs e)
        {
            if ((sender as ToolStripItem).Visible == false) return;

            openFileDialog.Title = Resources.ImportDialogHeader;
            openFileDialog.CheckFileExists = true;
            openFileDialog.CheckPathExists = true;
            openFileDialog.DefaultExt = FILE_EXTENSION_CSV;
            openFileDialog.Filter = FILTER_CSV;
            openFileDialog.FilterIndex = 1;
            openFileDialog.RestoreDirectory = true;
            openFileDialog.Multiselect = false;

            if (openFileDialog.ShowDialog() == DialogResult.OK)
            {
                DialogResult dr = MessageBox.Show(Resources.ImportConfirmation, m_params.m_inst.ComponentName,
                    MessageBoxButtons.YesNo, MessageBoxIcon.Warning);
                if (dr == DialogResult.Yes)
                {
                    string fileContent = CyParameters.GetFileText(openFileDialog.FileName);

                    Cursor actualCursor = Cursor.Current;
                    Cursor.Current = Cursors.WaitCursor;
                    CyImportExport.Import(this.Parent as CyTabWrapper, m_params, fileContent, false);
                    Cursor.Current = actualCursor;
                }
            }
        }

        void tsbExport_Click(object sender, EventArgs e)
        {
            if ((sender as ToolStripItem).Visible == false) return;

            saveFileDialog.Title = Resources.ExportDialogHeader;
            saveFileDialog.DefaultExt = FILE_EXTENSION_CSV;
            saveFileDialog.Filter = FILTER_CSV;
            saveFileDialog.FilterIndex = 1;
            saveFileDialog.RestoreDirectory = true;
            saveFileDialog.FileName = m_params.m_inst.InstanceName;

            if (saveFileDialog.ShowDialog() == DialogResult.OK)
            {
                List<object> exportTable = new List<object>();
                if (this.Parent is CyCustomCmdsTab)
                {
                    foreach (object item in m_params.CustomTable)
                        exportTable.Add(item);
                }
                else if (this.Parent is CyPmBusCmdsTab)
                {
                    foreach (object item in m_params.PmBusTable)
                        exportTable.Add(item);
                }


                CyParameters.SaveToFile(saveFileDialog.FileName, CyImportExport.Export(m_params, exportTable,
                CyParameters.GetColNames(m_dgv)));
            }
        }

        void tsbLoad_Click(object sender, EventArgs e)
        {
            if ((sender as ToolStripItem).Visible == false) return;

            openFileDialog.Title = Resources.LoadDialogHeader;
            openFileDialog.FileName = string.Empty;
            openFileDialog.CheckFileExists = true;
            openFileDialog.CheckPathExists = true;
            openFileDialog.DefaultExt = FILE_EXTENSION_XML;
            openFileDialog.Filter = FILTER_XML;
            openFileDialog.FilterIndex = 1;
            openFileDialog.RestoreDirectory = true;
            openFileDialog.Multiselect = false;

            if (openFileDialog.ShowDialog() == DialogResult.OK)
            {
                DialogResult dr = MessageBox.Show(Resources.LoadConfirmation, m_params.m_inst.ComponentName,
                    MessageBoxButtons.YesNo, MessageBoxIcon.Warning);
                if (dr == DialogResult.Yes)
                {
                    string fileContent = CyParameters.GetFileText(openFileDialog.FileName);
                    m_params.LoadFromObject(fileContent);
                }
            }
        }

        void tsbSave_Click(object sender, EventArgs e)
        {
            if ((sender as ToolStripItem).Visible == false) return;

            saveFileDialog.Title = Resources.SaveDialogHeader;
            saveFileDialog.DefaultExt = FILE_EXTENSION_XML;
            saveFileDialog.Filter = FILTER_XML;
            saveFileDialog.FilterIndex = 1;
            saveFileDialog.RestoreDirectory = true;
            saveFileDialog.FileName = m_params.m_inst.InstanceName;

            if (saveFileDialog.ShowDialog() == DialogResult.OK)
            {
                CyParameters.SaveToFile(saveFileDialog.FileName, m_params.Serialize(m_params));
            }
        }
    }
}
