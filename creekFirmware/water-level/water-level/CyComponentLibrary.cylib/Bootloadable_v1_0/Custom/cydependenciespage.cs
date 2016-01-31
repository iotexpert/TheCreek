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
using CyDesigner.Extensions.Common;
using CyDesigner.Extensions.Gde;

namespace Bootloadable_v1_0
{
    public partial class CyDependenciesPage : CyEditingWrapperControl
    {
        #region Constructors
        public CyDependenciesPage()
        {
            InitializeComponent();
        }

        public CyDependenciesPage(CyParameters parameters)
        {
            InitializeComponent();
            m_parameters = parameters;
            InitFields();
        }
        #endregion Constructors

        #region Initialization
        public void InitFields()
        {
            textBoxHexFilePath.Text = m_parameters.HexFilePath;
        }
        #endregion Initialization

        #region TabName override
        public override string TabName
        {
            get { return "Dependencies"; }
        }
        #endregion TabName override

        #region Event handlers
        private void textBoxHexFilePath_TextChanged(object sender, EventArgs e)
        {
            m_parameters.HexFilePath = ((ICyInstEdit_v1)m_parameters.m_inst).
                                            CreateDesignPersistantPath(textBoxHexFilePath.Text);
        }

        private void buttonBrowse_Click(object sender, EventArgs e)
        {
            using (OpenFileDialog openFileDialog1 = new OpenFileDialog())
            {
                string dir = "";
                if (!String.IsNullOrEmpty(m_parameters.HexFilePath) && File.Exists(m_parameters.HexFilePath))
                {
                    dir = Path.GetDirectoryName(m_parameters.HexFilePath);
                }
                if (!String.IsNullOrEmpty(dir))
                {
                    openFileDialog1.InitialDirectory = dir;
                }
                openFileDialog1.Filter = "Hex Files (*.hex)|*.hex";
                openFileDialog1.Title = Properties.Resources.OpenFileDialogTitle;
                if (openFileDialog1.ShowDialog() == DialogResult.OK)
                {
                    textBoxHexFilePath.Text = openFileDialog1.FileName;
                }
            }
        }
       
        private void CyDependenciesPage_Load(object sender, EventArgs e)
        {
            if (!String.IsNullOrEmpty(m_parameters.HexFilePath) && 
                (((ICyInstEdit_v1)m_parameters.m_inst).CreateDesignPersistantPath(textBoxHexFilePath.Text) != 
                  m_parameters.HexFilePath))
            {
                textBoxHexFilePath_TextChanged(textBoxHexFilePath, EventArgs.Empty);
            }
        }
        #endregion Event handlers
    }
}
