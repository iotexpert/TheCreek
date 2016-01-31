// ========================================
//
// Copyright Cypress Semiconductor Corporation, 2009
// All Rights Reserved
// UNPUBLISHED, LICENSED SOFTWARE.
//
// CONFIDENTIAL AND PROPRIETARY INFORMATION
// WHICH IS THE PROPERTY OF CYPRESS.
//
// Use of this file is governed
// by the license agreement included in the file
//
//     <install>/license/license.txt
//
// where <install> is the Cypress software
// installation root directory path.
//
// ========================================

using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Diagnostics;
using System.Drawing;
using System.Drawing.Drawing2D;
using System.Text.RegularExpressions;
using System.Windows.Forms;
using System.Collections;

namespace StaticSegLCD_v1_30
{
    public enum CyHelperKind {Segment7, Segment14, Segment16, Bar, Empty};

    public partial class CyHelpers : UserControl
    {
        #region Variables

        private CyTextBox m_editBox;

        public CyLCDParameters m_Parameters;

        private Size m_SymbolSize = new Size(60, 90);
        private Size m_SymbolBarSize = new Size(15, 90);
        private int m_SymbolSpace = 3;

        int m_currentHelperIndex = -1;
        private int m_selectedSymbolIndex = -1;
        private int m_selectedSegmentIndex = -1;

        ArrayList m_Symbols;

        #endregion Variables
        //------------------------------------------------------------------------------------------------------

        #region Constructors

        public CyHelpers()
        {
            InitializeComponent();
            InitDataGridMapCellStyle();
            m_Symbols = new ArrayList();
        }

        public CyHelpers(CyLCDParameters parameters)
        {
            InitializeComponent();
            InitDataGridMapCellStyle();
            m_Symbols = new ArrayList();
            this.m_Parameters = parameters;
            InitDataGridMap();
            LoadHelpers();
        }
        #endregion Constructors

        #region Choose Helper

        private void buttonAddHelper_Click(object sender, EventArgs e)
        {
            if (listBoxAvailHelpers.SelectedIndex < 0)
                return;

            // If number of helpers is 8 - don't create a new helper
            if (m_Parameters.m_HelpersConfig.Count - 1 >= 8)
            {
                MessageBox.Show("Maximum number of helpers is 8.\n Helper could not be added.", "Warning", 
                    MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
                return;
            }

            // Check if there are enough commons and segments to handle a helper
            bool fitInSpace = true;
            switch (listBoxAvailHelpers.SelectedIndex)
            {
                case 0:
                    if (m_Parameters.NumCommonLines*m_Parameters.NumSegmentLines - 
                        HelperInfo.GetTotalPixelNumber(m_Parameters) < 7)
                    {
                        fitInSpace = false;
                        break;
                    }
                    HelperInfo.CreateHelper(CyHelperKind.Segment7, m_Parameters);   
                    break;
                case 1:
                    if (m_Parameters.NumCommonLines * m_Parameters.NumSegmentLines - 
                        HelperInfo.GetTotalPixelNumber(m_Parameters) < 14)
                    {
                        fitInSpace = false;
                        break;
                    }
                    HelperInfo.CreateHelper(CyHelperKind.Segment14, m_Parameters);  
                    break;
                case 2:
                    if (m_Parameters.NumCommonLines * m_Parameters.NumSegmentLines - 
                        HelperInfo.GetTotalPixelNumber(m_Parameters) < 16)
                    {
                        fitInSpace = false;
                        break;
                    }
                    HelperInfo.CreateHelper(CyHelperKind.Segment16, m_Parameters);
                    break;
                case 3:
                    if (m_Parameters.NumCommonLines * m_Parameters.NumSegmentLines - 
                        HelperInfo.GetTotalPixelNumber(m_Parameters) < 1)
                    {
                        fitInSpace = false;
                        break;
                    }
                    HelperInfo.CreateHelper(CyHelperKind.Bar, m_Parameters);
                    break;
                default:
                    break;
            }

            // If number of common and segment lines is not enough - don't create a new helper
            if (!fitInSpace)
            {
                MessageBox.Show("Not enough segment and common lines to manage helpers.\n Helper could not be added.",
                                "Warning", MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
                return;
            }

            listBoxAddedHelpers.Items.Add(m_Parameters.m_HelpersConfig[m_Parameters.m_HelpersConfig.Count - 1]);
            m_Parameters.m_HelpersConfig[m_Parameters.m_HelpersConfig.Count - 1].AddSymbol(
                AddNextSymbolIndex(m_Parameters.m_HelpersConfig[m_Parameters.m_HelpersConfig.Count - 1].Kind));

            m_Parameters.SerializedHelpers = HelperInfo.SerializeHelpers(m_Parameters.m_HelpersConfig);
            listBoxAddedHelpers.SelectedIndex = listBoxAddedHelpers.Items.Count - 1;
        }

        public void LoadHelpers()
        {
            for (int i = 1; i < m_Parameters.m_HelpersConfig.Count; i++)
            {
                listBoxAddedHelpers.Items.Add(m_Parameters.m_HelpersConfig[i]);
            }
            if (listBoxAddedHelpers.Items.Count > 0)
                listBoxAddedHelpers.SelectedIndex = 0;
            // Remove used colors from the list
            foreach (HelperInfo helper in m_Parameters.m_HelpersConfig)
                m_Parameters.m_ColorChooser.PopCl(helper.HelperColor);
        }

        private void listBoxAddedHelpers_SelectedIndexChanged(object sender, EventArgs e)
        {
            ResetSelection();

            if ((listBoxAddedHelpers.Items.Count == 0) || (listBoxAddedHelpers.SelectedIndex < 0))
            {
                panelHelperConfig.Visible = false;
                m_currentHelperIndex = -1;
            }
            else
            {
                panelHelperConfig.Visible = true;
                for (int i = 0; i < m_Parameters.m_HelpersConfig.Count; i++)
                {
                    if (m_Parameters.m_HelpersConfig[i].Name == listBoxAddedHelpers.SelectedItem.ToString())
                    {
                        if (m_currentHelperIndex != i)
                        {
                            m_currentHelperIndex = i;
                            OpenNewHelper();
                            break;
                        }
                    }
                }
            }

            UpdateDataGridMapValues();
            UpdateLCDChars();
        }

        private void ResetSelection()
        {
            // Reset selection
            m_selectedSymbolIndex = -1;
            m_selectedSegmentIndex = -1;
            textBoxSegmentTitle.Enabled = false;
            textBoxSegmentTitle.Text = "";
        }

        #endregion Choose Helper

        //------------------------------------------------------------------------------------------------------

        #region Helper Configuration

        private void OpenNewHelper()
        {
            for (int i = m_Symbols.Count - 1; i >= 0; i--)
            {
                RemoveSymbol(i);
            }

            if (m_Parameters.m_HelpersConfig[m_currentHelperIndex].SymbolsCount > 0)
            {
                for (int i = 0; i < m_Parameters.m_HelpersConfig[m_currentHelperIndex].SymbolsCount; i++)
                    AddSymbol();
            }
            else
            {
                if (AddSymbol())
                    m_Parameters.m_HelpersConfig[m_currentHelperIndex].SymbolsCount++;
            }
        }

       

        //------------------------------------------------------------------------------------------------------

        private void buttonAddSymbol_Click(object sender, EventArgs e)
        {
            if (m_currentHelperIndex < 0)
                return;

            if ((m_Parameters.m_HelpersConfig[m_currentHelperIndex].SymbolsCount >=
                m_Parameters.m_HelpersConfig[m_currentHelperIndex].MaxSymbolsCount) || 
                (m_Parameters.m_HelpersConfig[m_currentHelperIndex].Kind == CyHelperKind.Empty))
                return;

            bool fitInSpace = true;
            switch (m_Parameters.m_HelpersConfig[m_currentHelperIndex].Kind)
            {
                case CyHelperKind.Segment7:
                    if (m_Parameters.NumCommonLines * m_Parameters.NumSegmentLines - HelperInfo.GetTotalPixelNumber(m_Parameters) < 7)
                    {
                        fitInSpace = false;
                        break;
                    }
                    break;
                case CyHelperKind.Segment14:
                    if (m_Parameters.NumCommonLines * m_Parameters.NumSegmentLines - HelperInfo.GetTotalPixelNumber(m_Parameters) < 14)
                    {
                        fitInSpace = false;
                        break;
                    }
                    break;
                case CyHelperKind.Segment16:
                    if (m_Parameters.NumCommonLines * m_Parameters.NumSegmentLines - HelperInfo.GetTotalPixelNumber(m_Parameters) < 16)
                    {
                        fitInSpace = false;
                        break;
                    }
                    break;
                case CyHelperKind.Bar:
                    if (m_Parameters.NumCommonLines * m_Parameters.NumSegmentLines - HelperInfo.GetTotalPixelNumber(m_Parameters) < 1)
                    {
                        fitInSpace = false;
                        break;
                    }
                    break;
                default:
                    break;
            }

            // If number of common and segment lines is not enough - don't add a new symbol
            if (!fitInSpace)
            {
                MessageBox.Show("Not enough segment and common lines to manage helpers.\n Symbol could not be added.", "Information", MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
                return;
            }

            m_Parameters.m_HelpersConfig[m_currentHelperIndex].AddSymbol(AddNextSymbolIndex(m_Parameters.m_HelpersConfig[m_currentHelperIndex].Kind));
            m_Parameters.ParametersChanged = true;
            AddSymbol();
        }

        private int AddNextSymbolIndex(CyHelperKind kind)
        {
            int index = 0;
            switch (kind)
            {
                case CyHelperKind.Segment7:
                    while (m_Parameters.m_SymbolIndexes_7SEG.Contains(index))
                        index++;
                    m_Parameters.m_SymbolIndexes_7SEG.Add(index);
                    break;
                case CyHelperKind.Segment14:
                    while (m_Parameters.m_SymbolIndexes_14SEG.Contains(index))
                        index++;
                    m_Parameters.m_SymbolIndexes_14SEG.Add(index);
                    break;
                case CyHelperKind.Segment16:
                    while (m_Parameters.m_SymbolIndexes_16SEG.Contains(index))
                        index++;
                    m_Parameters.m_SymbolIndexes_16SEG.Add(index);
                    break;
                case CyHelperKind.Bar:
                    while (m_Parameters.m_SymbolIndexes_BAR.Contains(index))
                        index++;
                    m_Parameters.m_SymbolIndexes_BAR.Add(index);
                    break;
                default:
                    break;
            }
            return index;
        }

        private void RemoveSymbolIndex(int index, CyHelperKind kind)
        {
            switch (kind)
            {
                case CyHelperKind.Segment7:
                    m_Parameters.m_SymbolIndexes_7SEG.Remove(index);
                    break;
                case CyHelperKind.Segment14:
                    m_Parameters.m_SymbolIndexes_14SEG.Remove(index);
                    break;
                case CyHelperKind.Segment16:
                    m_Parameters.m_SymbolIndexes_16SEG.Remove(index);
                    break;
                case CyHelperKind.Bar:
                    m_Parameters.m_SymbolIndexes_BAR.Remove(index);
                    break;
                default:
                    break;
            }
        }

        private bool AddSymbol()
        {
            int segCount = m_Parameters.m_HelpersConfig[m_currentHelperIndex].SegmentCount;
            string[] segmentTitles = new string[segCount];
            string pixelName, defaultName;
            for (int i = 0; i < segCount; i++)
            {
                pixelName = m_Parameters.m_HelpersConfig[m_currentHelperIndex].GetPixelBySymbolSegment(m_Symbols.Count,i).Name;
                defaultName = m_Parameters.m_HelpersConfig[m_currentHelperIndex].GetDefaultSymbolName(0);
                Regex defaultNamePattern = new Regex(defaultName.TrimEnd('_', '0') + "[0-9]+_");
                if (m_Parameters.m_HelpersConfig[m_currentHelperIndex].Kind == CyHelperKind.Bar)
                    defaultNamePattern = new Regex(defaultName);
                
                if (defaultNamePattern.IsMatch(pixelName))
                    segmentTitles[i] = pixelName.Remove(0, defaultName.Length);
                else
                    segmentTitles[i] = pixelName;
            }
            if (m_Symbols.Count < m_Parameters.m_HelpersConfig[m_currentHelperIndex].MaxSymbolsCount)
            {
                Size symSize = m_SymbolSize;
                //Size depends on symbol kind
                if (segCount == 1)
                    symSize = m_SymbolBarSize;
                CyLCDCharacter symbol = new CyLCDCharacter(symSize, m_Parameters.m_HelpersConfig[m_currentHelperIndex].Kind, segmentTitles, m_Parameters.m_HelpersConfig[m_currentHelperIndex].HelperColor);
                symbol.m_PBox.Location = new Point(m_SymbolSpace + (m_SymbolSpace + symSize.Width) * m_Symbols.Count, m_SymbolSpace);
                symbol.SegmentSelected += new SelectSegmentDelegate(symbol_SegmentSelected);
                symbol.m_PBox.ContextMenuStrip = contextMenuPixels;
                m_Symbols.Add(symbol);
                labelCharsNum.Text = m_Symbols.Count.ToString();
                panelDisplay.Controls.Add(symbol.m_PBox);
                symbol.RedrawAll();
            }
            return true;
        }

        void symbol_SegmentSelected(object sender)
        {
            if (sender == null)
            {
                for (int i = 0; i < m_Symbols.Count; i++)
                {
                    if (((CyLCDCharacter)m_Symbols[i]).m_SelectedSegment >= 0)
                    {
                        ((CyLCDCharacter)m_Symbols[i]).DeselectSegment();
                    }
                }

                textBoxSegmentTitle.Text = "";
                textBoxSegmentTitle.Enabled = false;
            }
            else
            {
                m_selectedSymbolIndex = m_Symbols.IndexOf(sender);
                m_selectedSegmentIndex = ((CyLCDCharacter) sender).m_SelectedSegment;
                for (int i = 0; i < m_Symbols.Count; i++)
                {
                    if ((((CyLCDCharacter) m_Symbols[i]).m_SelectedSegment >= 0) && (i != m_selectedSymbolIndex))
                    {
                        ((CyLCDCharacter) m_Symbols[i]).DeselectSegment();
                    }
                }
                if (m_selectedSegmentIndex < 0)
                {
                    textBoxSegmentTitle.Text = "";
                    textBoxSegmentTitle.Enabled = false;
                }
                else
                {
                    textBoxSegmentTitle.Enabled = true;
                    if ((m_currentHelperIndex >= 0) && (m_currentHelperIndex < m_Parameters.m_HelpersConfig.Count))
                    {
                        CyHelperSegmentInfo pixel = m_Parameters.m_HelpersConfig[m_currentHelperIndex].GetPixelBySymbolSegment(
                            m_selectedSymbolIndex, m_selectedSegmentIndex);
                        if (pixel != null)
                            textBoxSegmentTitle.Text = pixel.Name;
                    }
                }
            }
        }

        //------------------------------------------------------------------------------------------------------

        private void buttonRemoveSymbol_Click(object sender, EventArgs e)
        {
            if (m_currentHelperIndex < 0)
                return;

            if (m_Symbols.Count > 1)
            {
                for (int i = 0; i < m_Parameters.m_HelpersConfig[m_currentHelperIndex].HelpSegInfo.Count; i++)
                {
                    if (m_Parameters.m_HelpersConfig[m_currentHelperIndex].HelpSegInfo[i].DigitNum ==
                        m_Parameters.m_HelpersConfig[m_currentHelperIndex].SymbolsCount - 1)
                    {
                        RemoveSymbolIndex(m_Parameters.m_HelpersConfig[m_currentHelperIndex].HelpSegInfo[i].GlobalDigitNum,
                                          m_Parameters.m_HelpersConfig[m_currentHelperIndex].Kind);
                        m_Parameters.m_HelpersConfig[m_currentHelperIndex].HelpSegInfo.RemoveAt(i--);
                    }
                }
            
                m_Parameters.m_HelpersConfig[m_currentHelperIndex].SymbolsCount--;
                RemoveSymbol(m_Symbols.Count - 1);
                UpdateDataGridMapValues();
                m_Parameters.ParametersChanged = true;
            }
        }

        private void RemoveSymbol(int k)
        {
            //Check if it was selected
            if (((CyLCDCharacter)m_Symbols[k]).m_SelectedSegment >= 0)
                symbol_SegmentSelected(null);

            ((CyLCDCharacter)m_Symbols[k]).m_PBox.Dispose();
            m_Symbols.RemoveAt(k);
            labelCharsNum.Text = m_Symbols.Count.ToString();
        }

        private void textBoxSegmentTitle_TextChanged(object sender, EventArgs e)
        {
            if (textBoxSegmentTitle.Text == "" || m_currentHelperIndex < 0) return;
            CySegmentInfo pixel = m_Parameters.m_HelpersConfig[m_currentHelperIndex].GetPixelBySymbolSegment(m_selectedSymbolIndex,
                                                                                 m_selectedSegmentIndex);
            if ((pixel != null) && (pixel.Name != textBoxSegmentTitle.Text))
            {
                pixel.Name = textBoxSegmentTitle.Text;
               
                //Update title of LCD character's segments. (For Matrix helper - always empty).
                string defaultName = m_Parameters.m_HelpersConfig[m_currentHelperIndex].GetDefaultSymbolName(0);
                Regex defaultNamePattern = new Regex(defaultName.TrimEnd('_', '0') + "[0-9]+_");
                if (m_Parameters.m_HelpersConfig[m_currentHelperIndex].Kind == CyHelperKind.Bar)
                    defaultNamePattern = new Regex(defaultName);

                if (defaultNamePattern.IsMatch(pixel.Name))
                    ((CyLCDCharacter) m_Symbols[m_selectedSymbolIndex]).Segments[m_selectedSegmentIndex].Title =
                        pixel.Name.Remove(0, defaultName.Length);
                else
                    ((CyLCDCharacter) m_Symbols[m_selectedSymbolIndex]).Segments[m_selectedSegmentIndex].Title = pixel.Name;
           
                ((CyLCDCharacter)m_Symbols[m_selectedSymbolIndex]).DrawSegment(m_selectedSegmentIndex);
            }
        }

        private void buttonRemoveHelper_Click(object sender, EventArgs e)
        {
            if (listBoxAddedHelpers.SelectedIndex < 0)
                return;

            if (m_Parameters.m_HelpersConfig.Count > listBoxAddedHelpers.SelectedIndex + 1)
            {
                m_Parameters.m_ColorChooser.PushCl(m_Parameters.m_HelpersConfig[listBoxAddedHelpers.SelectedIndex + 1].HelperColor);
                for (int i = 0; i < m_Parameters.m_HelpersConfig[listBoxAddedHelpers.SelectedIndex + 1].HelpSegInfo.Count; i++)
                {
                    RemoveSymbolIndex(m_Parameters.m_HelpersConfig[listBoxAddedHelpers.SelectedIndex + 1].HelpSegInfo[i].GlobalDigitNum,
                                      m_Parameters.m_HelpersConfig[listBoxAddedHelpers.SelectedIndex + 1].Kind);
                }
                HelperInfo.RemoveHelperIndex(m_Parameters.m_HelpersConfig[listBoxAddedHelpers.SelectedIndex + 1].GlobalHelperIndex,
                    m_Parameters.m_HelpersConfig[listBoxAddedHelpers.SelectedIndex + 1].Kind, m_Parameters);
                m_Parameters.m_HelpersConfig.RemoveAt(listBoxAddedHelpers.SelectedIndex + 1);
                listBoxAddedHelpers.Items.RemoveAt(listBoxAddedHelpers.SelectedIndex);

                m_Parameters.SerializedHelpers = HelperInfo.SerializeHelpers(m_Parameters.m_HelpersConfig);

                if (listBoxAddedHelpers.Items.Count > 0)
                    listBoxAddedHelpers.SelectedIndex = 0;
            }
        }

        private void textBoxSegmentTitle_Validated(object sender, EventArgs e)
        {
            UpdateDataGridMapValues();
            m_Parameters.ParametersChanged = true;
        }

        //------------------------------------------------------------------------------------------------------

        #endregion Helper Configuration

        //------------------------------------------------------------------------------------------------------

        #region DataGridMap

        private DataGridViewCellStyle CellCommonStyle;
        private DataGridViewCellStyle CellSegmentStyle;
        private DataGridViewCellStyle cellDisabledStyle;
        private DataGridViewCellStyle CellBusyStyle;
        private DataGridViewCellStyle CellAssignedStyle;
        private DataGridViewCellStyle CellHighlightedStyle;

        private void InitDataGridMapCellStyle()
        {
            dataGridMap.DefaultCellStyle.Font = new Font("Microsoft Sans Serif", 7);
            dataGridMap.DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleCenter;

            CellCommonStyle = new DataGridViewCellStyle();
            CellCommonStyle.BackColor = Color.LightSteelBlue;

            CellSegmentStyle = new DataGridViewCellStyle();
            CellSegmentStyle.BackColor = Color.LightSteelBlue;

            cellDisabledStyle = new DataGridViewCellStyle();
            cellDisabledStyle.BackColor = Color.FromArgb(200, 200, 200);
            cellDisabledStyle.ForeColor = Color.Gray;
            
            CellBusyStyle = new DataGridViewCellStyle();
            CellBusyStyle.BackColor = Color.LightGreen;

            CellAssignedStyle = new DataGridViewCellStyle();
            CellAssignedStyle.BackColor = Color.Lime;

            CellHighlightedStyle = new DataGridViewCellStyle();
            CellHighlightedStyle.BackColor = Color.Yellow;
        }

        private void InitDataGridMap()
        {
            dataGridMap.Rows.Clear();
            dataGridMap.Columns.Clear();
            for (int i = 0; i < m_Parameters.NumCommonLines + 1; i++)
            {
                dataGridMap.Columns.Add(i.ToString(), "-");
            }
            for (int j = 0; j < m_Parameters.NumSegmentLines + 1; j++)
            {
                dataGridMap.Rows.Add();
            }

            dataGridMap.Rows[0].Frozen = true;
            dataGridMap.Columns[0].Frozen = true;

            dataGridMap.Columns[0].Width = 40;
            for (int i = 0; i < m_Parameters.NumCommonLines; i++)
            {
                //CheckBoxColumnHeader cbCell = new CheckBoxColumnHeader();
                //dataGridMap[i + 1, 0] = cbCell;
                dataGridMap.Columns[i + 1].Width = 65;
                dataGridMap[i + 1, 0].Value = "Com" + (m_Parameters.NumCommonLines - i - 1);

                //if (m_Parameters.DisabledCommons.Contains(m_Parameters.NumCommonLines - i - 1))
                //    cbCell.IsChecked = false;
                dataGridMap[i + 1, 0].Style = CellCommonStyle;
            }
            for (int j = 0; j < m_Parameters.NumSegmentLines; j++)
            {
                dataGridMap[0, j + 1].Value = "Seg" + (j).ToString();
                dataGridMap[0, j + 1].Style = CellSegmentStyle;
            }

            //Change dataGridMap size
            int w = 0;
            for (int i = 0; i < dataGridMap.ColumnCount; i++)
                w += dataGridMap.Columns[i].Width;
            dataGridMap.Width = w + 10;

            int h = 0;
            for (int i = 0; i < dataGridMap.RowCount; i++)
                h += dataGridMap.Rows[i].Height;
            dataGridMap.Height = h + 10;
        }

        private void UpdateDataGridMapValues()
        {
            for (int i = 1; i < dataGridMap.ColumnCount; i++)
                for (int j = 1; j < dataGridMap.RowCount; j++)
                {
                    dataGridMap[i, j].Value = null;
                    dataGridMap[i, j].Style = dataGridMap.DefaultCellStyle;
                }
            for (int i = 0; i < m_Parameters.m_HelpersConfig.Count; i++)
                for (int j = 0; j < m_Parameters.m_HelpersConfig[i].HelpSegInfo.Count; j++)
                {
                    if ((m_Parameters.m_HelpersConfig[i].HelpSegInfo[j].Common >= 0) &&
                        (m_Parameters.m_HelpersConfig[i].HelpSegInfo[j].Segment >= 0))
                    {
                        dataGridMap[m_Parameters.NumCommonLines - m_Parameters.m_HelpersConfig[i].HelpSegInfo[j].Common,
                                    m_Parameters.m_HelpersConfig[i].HelpSegInfo[j].Segment + 1].Value =
                            m_Parameters.m_HelpersConfig[i].HelpSegInfo[j].Name;
                        if (i != 0)
                        {
                            dataGridMap[
                                m_Parameters.NumCommonLines - m_Parameters.m_HelpersConfig[i].HelpSegInfo[j].Common,
                                m_Parameters.m_HelpersConfig[i].HelpSegInfo[j].Segment + 1].Style =
                                new DataGridViewCellStyle(CellBusyStyle);
                            dataGridMap[
                                m_Parameters.NumCommonLines - m_Parameters.m_HelpersConfig[i].HelpSegInfo[j].Common,
                                m_Parameters.m_HelpersConfig[i].HelpSegInfo[j].Segment + 1].Style.BackColor =
                                m_Parameters.m_HelpersConfig[i].HelperColor;
                        }
                    }
                }
        }

        private void CyHelpers_VisibleChanged(object sender, EventArgs e)
        {
            // Common, segment mapping
            if (Visible)
            {
                //Update Empty helper
                HelperInfo.UpdateEmptyHelper(m_Parameters);

                InitDataGridMap();
                UpdateDataGridMapValues();
            }
        }

        private void dataGridMap_CellMouseDown(object sender, DataGridViewCellMouseEventArgs e)
        {
            if ((e.ColumnIndex > 0) && (e.RowIndex > 0) && (dataGridMap[e.ColumnIndex, e.RowIndex].Value != null))
            {
                string pixelName = (string) dataGridMap[e.ColumnIndex, e.RowIndex].Value;
                foreach (HelperInfo helper in m_Parameters.m_HelpersConfig)
                {
                    foreach (CyHelperSegmentInfo pixel in helper.HelpSegInfo)
                    {
                        if (pixel.Name == pixelName)
                        {
                            if (m_Parameters.m_HelpersConfig.IndexOf(helper) > 0)
                            {
                                listBoxAddedHelpers.SelectedIndex = m_Parameters.m_HelpersConfig.IndexOf(helper) - 1;
                                //Select segment
                                ((CyLCDCharacter) m_Symbols[pixel.DigitNum]).SelectSegment(pixel.RelativePos);

                            }
                            else // Empty helper
                            {

                            }
                        }
                    }
                }
            }
            //else if ((e.ColumnIndex > 0) && (e.RowIndex == 0))
            //{
            //    if (!((CheckBoxColumnHeader)dataGridMap[e.ColumnIndex, e.RowIndex]).IsChecked)
            //        EnableCommon(m_Parameters.NumCommonLines - e.ColumnIndex);
            //    else
            //        DisableCommon(m_Parameters.NumCommonLines - e.ColumnIndex);
            //}
        } 


        private void dataGridMap_DragLeave(object sender, EventArgs e)
        {
            for (int i = 1; i < dataGridMap.ColumnCount; i++)
                for (int j = 1; j < dataGridMap.RowCount; j++)
                {
                    if (dataGridMap[i, j].Style == CellHighlightedStyle)
                        dataGridMap[i, j].Style = dataGridMap.DefaultCellStyle;
                }
        }

        private void listBoxAddedHelpers_DoubleClick(object sender, EventArgs e)
        {
            CreateEditBox(sender);
        }

        private void dataGridMap_DragOver(object sender, DragEventArgs e)
        {
            Point localPoint = dataGridMap.PointToClient(new Point(e.X, e.Y));
            DataGridView.HitTestInfo hit = dataGridMap.HitTest(localPoint.X, localPoint.Y);

            //Remove selection
            for (int i = 1; i < dataGridMap.ColumnCount; i++)
                for (int j = 1; j < dataGridMap.RowCount; j++)
                {
                    if (dataGridMap[i, j].Style == CellHighlightedStyle)
                        dataGridMap[i, j].Style = dataGridMap.DefaultCellStyle;
                }

            if (hit.Type == DataGridViewHitTestType.Cell)
            {
                if ((hit.RowIndex > 0) && (hit.ColumnIndex > 0) && dataGridMap[hit.ColumnIndex, hit.RowIndex].Style != 
                                                                   cellDisabledStyle)
                {
                    e.Effect = e.Data.GetDataPresent(typeof (CyLCDCharacter))
                                   ? DragDropEffects.Move : DragDropEffects.None;
                    if (dataGridMap[hit.ColumnIndex, hit.RowIndex].Style != CellHighlightedStyle)
                    {
                        for (int i = 1; i < dataGridMap.ColumnCount; i++)
                            for (int j = 1; j < dataGridMap.RowCount; j++)
                            {
                                if (dataGridMap[i, j].Style == CellHighlightedStyle)
                                    dataGridMap[i, j].Style = dataGridMap.DefaultCellStyle;
                            }
                        if (dataGridMap[hit.ColumnIndex, hit.RowIndex].Style == dataGridMap.DefaultCellStyle)
                            dataGridMap[hit.ColumnIndex, hit.RowIndex].Style = CellHighlightedStyle;
                    }
                }
            }

            //Scroll vertical
            if (localPoint.Y >= dataGridMap.Height - 10) //if moving downwards
            {
                dataGridMap.CurrentCell =
                    dataGridMap[dataGridMap.CurrentCell.ColumnIndex, dataGridMap.CurrentCell.RowIndex + 1];
            }
            else if (localPoint.Y < 10) //if moving upwards
            {
                dataGridMap.CurrentCell =
                    dataGridMap[dataGridMap.CurrentCell.ColumnIndex, dataGridMap.CurrentCell.RowIndex - 1];
            }
            //Scroll horizontal
            if (localPoint.X >= dataGridMap.Width - 20) //if moving to the right 
            {
                dataGridMap.CurrentCell =
                    dataGridMap[dataGridMap.CurrentCell.ColumnIndex + 1, dataGridMap.CurrentCell.RowIndex];
            }
            else if (localPoint.X < 20) //if moving to the left
            {
                dataGridMap.CurrentCell =
                    dataGridMap[dataGridMap.CurrentCell.ColumnIndex - 1, dataGridMap.CurrentCell.RowIndex];
            }
        }

        private void dataGridMap_DragDrop(object sender, DragEventArgs e)
        {
            Point localPoint = dataGridMap.PointToClient(new Point(e.X, e.Y));
            DataGridView.HitTestInfo hit = dataGridMap.HitTest(localPoint.X, localPoint.Y);
            if (hit.Type == DataGridViewHitTestType.Cell)
            {
                if ((hit.RowIndex > 0) && (hit.ColumnIndex > 0) && (dataGridMap[hit.ColumnIndex, hit.RowIndex].Style != 
                                                                    cellDisabledStyle))
                {
                    CyLCDCharacter lcdChar = (CyLCDCharacter)e.Data.GetData(typeof(CyLCDCharacter));
                    CySegmentInfo pixel1 =
                       m_Parameters.m_HelpersConfig[m_currentHelperIndex].GetPixelBySymbolSegment(m_selectedSymbolIndex,
                                                                                            m_selectedSegmentIndex);
                    ResetPixelCommonSegment(pixel1.Name);
                    lcdChar.DrawSegment(lcdChar.m_SelectedSegment, true);
                    dataGridMap[hit.ColumnIndex, hit.RowIndex].Style = new DataGridViewCellStyle(CellBusyStyle);
                    dataGridMap[hit.ColumnIndex, hit.RowIndex].Style.BackColor =
                        m_Parameters.m_HelpersConfig[m_currentHelperIndex].HelperColor;

                    //Set common and segment for pixel
                    pixel1.Common = m_Parameters.NumCommonLines - hit.ColumnIndex;
                    pixel1.Segment = hit.RowIndex - 1;
                    if (dataGridMap[hit.ColumnIndex, hit.RowIndex].Value != null)
                    {
                        ResetPixelCommonSegment(dataGridMap[hit.ColumnIndex, hit.RowIndex].Value.ToString());
                    }
                    dataGridMap[hit.ColumnIndex, hit.RowIndex].Value = pixel1.Name;

                    m_Parameters.ParametersChanged = true;
                }
            }
        }

        private void ResetPixelCommonSegment(string name)
        {
            // Omit first helper (Empty helper). Its values never reset.
            for (int i = 1; i < m_Parameters.m_HelpersConfig.Count; i++)
            {
                {
                    CySegmentInfo pixel2 =
                        m_Parameters.m_HelpersConfig[i].GetPixelByName(name);
                    if (pixel2 != null)
                    {
                        pixel2.Common = -1;
                        pixel2.Segment = -1;
                        m_Parameters.ParametersChanged = true;
                        UpdateLCDChars();
                        UpdateDataGridMapValues();
                        break;
                    }
                }
            }
        }

        private void UpdateLCDChars()
        {
            if (m_currentHelperIndex < 0) return;

            for (int i = 0; i < m_Symbols.Count; i++)
                for (int j = 0; j < ((CyLCDCharacter)m_Symbols[i]).SegmentsCount; j++)
                {
                    CySegmentInfo pixel1 =
                       m_Parameters.m_HelpersConfig[m_currentHelperIndex].GetPixelBySymbolSegment(i,j);
                    if ((pixel1 != null) && ((pixel1.Common >= 0) && (pixel1.Segment >= 0)))
                        ((CyLCDCharacter)m_Symbols[i]).DrawSegment(j, true);
                    else
                        ((CyLCDCharacter)m_Symbols[i]).DrawSegment(j, false);
                }
        }

        private void dataGridMap_SelectionChanged(object sender, EventArgs e)
        {
            if (dataGridMap.SelectedCells.Count > 0)
                dataGridMap.SelectedCells[0].Selected = false;
        }

        private void contextMenuPixels_Opening(object sender, CancelEventArgs e)
        {
            gangCommonLineToolStripMenuItem.Visible = false;
            resetToolStripMenuItem.Visible = true;

            if (contextMenuPixels.SourceControl is PictureBox)
            {
                bool isPixelSelected = false;
                for (int i = 0; i < m_Symbols.Count; i++)
                {
                    if (((CyLCDCharacter)m_Symbols[i]).m_SelectedSegment >= 0)
                    {
                        isPixelSelected = true;
                        break;
                    }
                }
                if (!isPixelSelected)
                {
                    e.Cancel = true;
                }
            }
            //else if (contextMenuPixels.SourceControl is DataGridView)
            //{
            //    Point localPoint = dataGridMap.PointToClient(contextMenuPixels.Location);
            //    DataGridView.HitTestInfo hit = dataGridMap.HitTest(localPoint.X, localPoint.Y);

            //    if (hit.Type == DataGridViewHitTestType.Cell)
            //    {
            //        if ((hit.RowIndex == 0) && (hit.ColumnIndex > 0))
            //        {
            //            gangCommonLineToolStripMenuItem.Visible = true;
            //            resetToolStripMenuItem.Visible = false;
            //        }
            //    }
            //}
        }

        private void resetToolStripMenuItem_Click(object sender, EventArgs e)
        {
            if (contextMenuPixels.SourceControl is PictureBox)
            {
                for (int i = 0; i < m_Symbols.Count; i++)
                {
                    if (((CyLCDCharacter)m_Symbols[i]).m_SelectedSegment >= 0)
                    {
                        CySegmentInfo pixel1 =
                            m_Parameters.m_HelpersConfig[m_currentHelperIndex].GetPixelBySymbolSegment(i,
                            ((CyLCDCharacter)m_Symbols[i]).m_SelectedSegment);
                        if (pixel1 != null)
                            ResetPixelCommonSegment(pixel1.Name);
                    }
                }
            }
            else if (contextMenuPixels.SourceControl is DataGridView)
            {
                Point localPoint = dataGridMap.PointToClient(contextMenuPixels.Location);
                DataGridView.HitTestInfo hit = dataGridMap.HitTest(localPoint.X, localPoint.Y);

                if (hit.Type == DataGridViewHitTestType.Cell)
                {
                    if ((hit.RowIndex > 0) && (hit.ColumnIndex > 0) && 
                        (dataGridMap[hit.ColumnIndex, hit.RowIndex].Value != null))
                    {
                        ResetPixelCommonSegment((string)dataGridMap[hit.ColumnIndex,hit.RowIndex].Value);
                    }
                }
            }
        }

        private void dataGridMap_MouseMove(object sender, MouseEventArgs e)
        {
            if ((e.Button & MouseButtons.Left) > 0)
            {
                Point localPoint = new Point(e.X, e.Y);
                DataGridView.HitTestInfo hit = dataGridMap.HitTest(localPoint.X, localPoint.Y);
                if (hit.Type == DataGridViewHitTestType.Cell)
                {
                    if ((hit.RowIndex > 0) && (hit.ColumnIndex > 0) && (dataGridMap[hit.ColumnIndex, hit.RowIndex].Style !=
                                                                        cellDisabledStyle))
                    {
                        string pixelName = (string)dataGridMap[hit.ColumnIndex, hit.RowIndex].Value;
                        foreach (HelperInfo helper in m_Parameters.m_HelpersConfig)
                        {
                            foreach (CyHelperSegmentInfo pixel in helper.HelpSegInfo)
                            {
                                if (pixel.Name == pixelName)
                                {
                                    if (m_Parameters.m_HelpersConfig.IndexOf(helper) > 0)
                                    {
                                        CyLCDCharacter lcdChar = ((CyLCDCharacter) m_Symbols[pixel.DigitNum]);
                                        dataGridMap.DoDragDrop(lcdChar, DragDropEffects.Move);
                                    }
                                }
                            }
                        }
                    }
                }
            }
        }

        private void dataGridMap_CellEndEdit(object sender, DataGridViewCellEventArgs e)
        {
            dataGridMap[e.ColumnIndex, e.RowIndex].Value = dataGridMap[e.ColumnIndex, e.RowIndex].Value.ToString().ToUpper();

            int common = m_Parameters.NumCommonLines - e.ColumnIndex;
            int segment = e.RowIndex - 1;
            if ((e.ColumnIndex > 0) && (e.RowIndex > 0) && (dataGridMap[e.ColumnIndex, e.RowIndex].Value != null))
            {
                bool found = false;
                for (int i = 1; i < m_Parameters.m_HelpersConfig.Count; i++)
                {
                    HelperInfo helper = m_Parameters.m_HelpersConfig[i];
                    foreach (CyHelperSegmentInfo pixel in helper.HelpSegInfo)
                    {
                        if ((pixel.Common == common) && (pixel.Segment == segment))
                        {
                            textBoxSegmentTitle.Text = dataGridMap[e.ColumnIndex, e.RowIndex].Value.ToString();
                            m_Parameters.ParametersChanged = true;
                            found = true;
                            break;
                        }
                    }
                    if (found) break;
                }
                if (!found)
                {
                    HelperInfo helper = m_Parameters.m_HelpersConfig[0];
                    foreach (CyHelperSegmentInfo pixel in helper.HelpSegInfo)
                    {
                        if ((pixel.Common == common) && (pixel.Segment == segment))
                        {
                            pixel.Name = dataGridMap[e.ColumnIndex, e.RowIndex].Value.ToString();
                            m_Parameters.ParametersChanged = true;
                        }
                    }
                }
            }
        }

        private void dataGridMap_CellBeginEdit(object sender, DataGridViewCellCancelEventArgs e)
        {
            if ((e.ColumnIndex == 0) || (e.RowIndex == 0))
            {
                e.Cancel = true;
            }
        }

        private void dataGridMap_CellValidating(object sender, DataGridViewCellValidatingEventArgs e)
        {
            if (dataGridMap[e.ColumnIndex, e.RowIndex].IsInEditMode)
            {
                try
                {
                    string value = e.FormattedValue.ToString();
                    e.Cancel = !ValidatePixelName(value);
                    if (!HelperInfo.CheckPixelUniqueName(m_Parameters, value, 0) &&
                        (dataGridMap[e.ColumnIndex, e.RowIndex].Value.ToString() != value))
                    {
                        MessageBox.Show("The name " + value + " is not unique. Please, enter another name.",
                                        "Warning", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                        e.Cancel = true;
                    }
                    if (e.FormattedValue.ToString() != value.ToUpper())
                    {
                        dataGridMap[e.ColumnIndex, e.RowIndex].Value = value.ToUpper();
                    }
                }
                catch
                {
                    e.Cancel = true;
                }
                dataGridMap.Refresh();
            }
        }

        private void dataGridMap_CellDoubleClick(object sender, DataGridViewCellEventArgs e)
        {
            dataGridMap.BeginEdit(true);
        }  

        #endregion

        #region Other events

        private void CyHelpers_Leave(object sender, EventArgs e)
        {
            // Save changes
            m_Parameters.SerializedHelpers = HelperInfo.SerializeHelpers(m_Parameters.m_HelpersConfig);          
        }

        private void textBoxSegmentTitle_Validating(object sender, CancelEventArgs e)
        {
            try
            {
                e.Cancel = !ValidatePixelName(textBoxSegmentTitle.Text);
                if (textBoxSegmentTitle.Text != textBoxSegmentTitle.Text.ToUpper())
                {
                    textBoxSegmentTitle.Text = textBoxSegmentTitle.Text.ToUpper();
                }
            }
            catch
            {
                e.Cancel = true;
            }
        }

        private bool ValidatePixelName(string value)
        {
            bool res = true;

            if (!HelperInfo.CheckPixelUniqueName(m_Parameters, value, 1))
            {
                MessageBox.Show("The name " + value + " is not unique. Please, enter another name.",
                                "Warning", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                res = false;
            }
            bool wrongChars = false;
            for (int i = 0; i < value.Length; i++)
            {
                if (!(Char.IsLetterOrDigit(value[i]) || (value[i] == '_')))
                {
                    wrongChars = true;
                    break;
                }
            }
            if (wrongChars)
            {
                MessageBox.Show("The name can contain only letters, digits and '_' symbol.",
                                "Warning", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                res = false;
            }
            return res;
        }
        #endregion Other events

        //------------------------------------------------------------------------------------------------------
        #region Printing

        private enum CyPixelCellStyle { HEADER, VALUE } ;

        private void buttonPrint_Click(object sender, EventArgs e)
        {
            printDocumentPixelMap.DefaultPageSettings.Landscape = true;

            if (printDialog1.ShowDialog() == DialogResult.OK)
            {
                //if (printPreviewDialog1.ShowDialog() == DialogResult.OK)
                {
                    printDocumentPixelMap.Print();
                }
            }

        }

        private int m_lastPrintedColumn = 0, m_lastPrintedRow = 0;
        private int m_columnsPerPage = 0, m_rowsPerPage = 0;
        private void printDocumentPixelMap_PrintPage(object sender, System.Drawing.Printing.PrintPageEventArgs e)
        {
            e.PageSettings.Landscape = true;

            try
            {
                // Initial calculations
                if (m_columnsPerPage == 0)
                {
                    m_columnsPerPage =
                        (int) ((e.MarginBounds.Right - e.MarginBounds.Left)/(dataGridMap[1, 1].Size.Width*1.5)) - 1;
                    m_rowsPerPage =
                        (int) ((e.MarginBounds.Bottom - e.MarginBounds.Top)/(dataGridMap[1, 1].Size.Height*1.3)) - 1;
                    m_lastPrintedColumn = 0;
                    m_lastPrintedRow = 0;
                }

                int x1, x2, y1, y2;
                x1 = m_lastPrintedRow + 1;
                x2 = Math.Min(x1 + m_rowsPerPage, dataGridMap.RowCount - 1);
                y1 = m_lastPrintedColumn + 1;
                y2 = Math.Min(y1 + m_columnsPerPage, dataGridMap.ColumnCount - 1);

                PrintBlockTable(x1, x2, y1, y2, e.Graphics);

                if (x2 < dataGridMap.RowCount - 1)
                {
                    m_lastPrintedRow = x2;
                    e.HasMorePages = true;
                }
                else if (y2 < dataGridMap.ColumnCount - 1)
                {
                    m_lastPrintedColumn = y2;
                    m_lastPrintedRow = 0;
                    e.HasMorePages = true;
                }
                else
                {
                    m_columnsPerPage = 0;
                    m_rowsPerPage = 0;
                    m_lastPrintedColumn = 0;
                    m_lastPrintedRow = 0;
                    e.HasMorePages = false;
                }
            }
            catch(Exception)
            {
                m_columnsPerPage = 0;
                m_rowsPerPage = 0;
                m_lastPrintedColumn = 0;
                m_lastPrintedRow = 0;
                e.HasMorePages = false;
                MessageBox.Show("An error during printing occured", "SegLCD", MessageBoxButtons.OK, 
                                MessageBoxIcon.Error);
            }
        }

        private void PrintBlockTable(int x1, int x2, int y1, int y2, Graphics g)
        {
            Point cellOrigin = new Point(0, 0);
            Size cellSize = new Size();
            int verticalShift = 0;
            Rectangle boundRect;
            for (int i = 0; i <= y2; i++)
            {
                if (i == 1) 
                    i = y1;

                cellOrigin.Y = verticalShift;
                cellSize.Width = (int)(dataGridMap.Columns[i].Width * 1.5);
                for (int j = 0; j <= x2; j++)
                {
                    if (j == 1)
                        j = x1;

                    cellSize.Height = (int)(dataGridMap[i, j].Size.Height * 1.3);

                    if (dataGridMap[i, j].Value != null)
                    {
                        if ((i == 0) || (j == 0))
                            PrintCell(g, cellOrigin, cellSize, dataGridMap[i, j].Value.ToString(), 
                                        CyPixelCellStyle.HEADER);
                        else
                            PrintCell(g, cellOrigin, cellSize, dataGridMap[i, j].Value.ToString(), 
                                        CyPixelCellStyle.VALUE);
                    }
                    cellOrigin.Y += cellSize.Height;
                }
                cellOrigin.X += cellSize.Width;
            }
            boundRect = new Rectangle(0, verticalShift, cellOrigin.X, cellOrigin.Y - verticalShift);
            g.DrawRectangle(new Pen(Color.LightGray, 2), boundRect);
        }

        private void PrintCell(Graphics g, Point origin, Size size, string text, CyPixelCellStyle style)
        {
            Font font = new Font("Arial", 10);
            StringFormat format = new StringFormat();
            format.Alignment = StringAlignment.Center;
            format.LineAlignment = StringAlignment.Center;
            Rectangle boundRect = new Rectangle(origin, size);
            
            if (style == CyPixelCellStyle.HEADER)
                g.FillRectangle(new SolidBrush(Color.LightSteelBlue),boundRect);
            else if (style == CyPixelCellStyle.VALUE)
                g.FillRectangle(new SolidBrush(Color.White), boundRect);
            g.DrawRectangle(new Pen(Color.LightGray, 2), boundRect);
            //g.DrawRectangle(new Pen(Color.White, 1), boundRect);
            g.DrawString(text, font, new SolidBrush(Color.Black), boundRect, format);
        }

        #endregion

        #region ColorListMaking
        private void CyHelpers_Load(object sender, EventArgs e)
        {
            listBoxAddedHelpers.DrawMode =DrawMode.OwnerDrawVariable;
            listBoxAddedHelpers.DrawItem += new DrawItemEventHandler(DrawItemHandler);
            listBoxAddedHelpers.MeasureItem += new MeasureItemEventHandler(MeasureItemHandler);

            //EditBox
            m_editBox = new CyTextBox();
			m_editBox.Location = new Point(0,0);
			m_editBox.Size = new Size(0,0);
			m_editBox.Hide();
			listBoxAddedHelpers.Controls.Add(m_editBox);			
			m_editBox.Text = "";
			m_editBox.BorderStyle = BorderStyle.FixedSingle;
            m_editBox.KeyPress += new KeyPressEventHandler(editBox_KeyPress);
            m_editBox.LostFocus += new EventHandler(editBox_LostFocus);
            m_editBox.Validating +=new CancelEventHandler(editBox_Validating);
        }

        void  editBox_Validating(object sender, CancelEventArgs e)
        {
            if (!HelperInfo.CheckHelperUniqueName(m_Parameters, m_editBox.Text))
            {
                // Omit case when the name wasn't changed
                if (((HelperInfo)listBoxAddedHelpers.Items[listBoxAddedHelpers.SelectedIndex]).Name != m_editBox.Text)
                {
                    MessageBox.Show("The name " + m_editBox.Text + " is not unique. Please, enter another name.",
                                    "Warning", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                    e.Cancel = true;
                    m_editBox.Show();
                }
            }
        }

        void editBox_LostFocus(object sender, EventArgs e)
        {
            ((HelperInfo)listBoxAddedHelpers.Items[listBoxAddedHelpers.SelectedIndex]).Name = m_editBox.Text;
            m_Parameters.ParametersChanged = true;
            m_editBox.Hide();
            listBoxAddedHelpers.Refresh();
        }

        void editBox_KeyPress(object sender, KeyPressEventArgs e)
        {
            if (e.KeyChar == 13)
            {
                ((HelperInfo)listBoxAddedHelpers.Items[listBoxAddedHelpers.SelectedIndex]).Name = m_editBox.Text;
                m_Parameters.ParametersChanged = true;
                m_editBox.Hide();
                listBoxAddedHelpers.Refresh();
            }
        }

        private void CreateEditBox(object sender)
		{
			listBoxAddedHelpers = (ListBox)sender ; 
			int itemSelected = listBoxAddedHelpers.SelectedIndex ;
			Rectangle r = listBoxAddedHelpers.GetItemRectangle(itemSelected);
            int delta = 5;
			string itemText = listBoxAddedHelpers.Items[itemSelected].ToString();
			
			m_editBox.Location = new System.Drawing.Point(r.X + delta , r.Y + delta ) ;
			m_editBox.Size 
                 = new System.Drawing.Size(r.Width -10 , r.Height- delta);
			m_editBox.Show();
			m_editBox.Text = itemText;
			m_editBox.Focus();
			m_editBox.SelectAll(); 
		}


        private void DrawItemHandler(object sender, DrawItemEventArgs e)
        {
            try
            {
                HelperInfo item = (HelperInfo)(((ListBox)sender).Items[e.Index]);

                Color foreColor = e.ForeColor;
                if ((e.State & DrawItemState.Selected) == DrawItemState.Selected)
                {
                    e.DrawBackground();
                }
                else
                {
                    using (SolidBrush bbr = new SolidBrush(item.HelperColor))
                        e.Graphics.FillRectangle(bbr, e.Bounds);
                    foreColor = Color.Black;
                }
                using (SolidBrush solidBrush = new SolidBrush(foreColor))
                    e.Graphics.DrawString(item.ToString(), e.Font, solidBrush, e.Bounds, StringFormat.GenericDefault);
                e.DrawFocusRectangle();
                e.Graphics.DrawRectangle(new Pen(Color.White), e.Bounds);
            }
            catch (Exception)
            {
            }
        }

        private void MeasureItemHandler(object sender, MeasureItemEventArgs e)
        {
            e.ItemHeight = 16;
        }

        #endregion

        //------------------------------------------------------------------------------------------------------
        # region CheckBoxCell

        class CheckBoxColumnHeader : DataGridViewTextBoxCell
        {
            private Rectangle CheckBoxRegion;
            private bool isChecked = true;

            protected override void Paint(Graphics graphics,
                Rectangle clipBounds, Rectangle cellBounds, int rowIndex,
                DataGridViewElementStates dataGridViewElementState,
                object value, object formattedValue, string errorText,
                DataGridViewCellStyle cellStyle, DataGridViewAdvancedBorderStyle advancedBorderStyle,
                DataGridViewPaintParts paintParts)
            {
                base.Paint(graphics, clipBounds, cellBounds, rowIndex, dataGridViewElementState, value,
                    formattedValue, errorText, cellStyle, advancedBorderStyle, paintParts);

                graphics.FillRectangle(new SolidBrush(cellStyle.BackColor), cellBounds);

                CheckBoxRegion = new Rectangle(
                    cellBounds.Location.X + 1,
                    cellBounds.Location.Y + 2,
                    25, cellBounds.Size.Height - 4);


                if (isChecked)
                    ControlPaint.DrawCheckBox(graphics, CheckBoxRegion, ButtonState.Checked);
                else
                    ControlPaint.DrawCheckBox(graphics, CheckBoxRegion, ButtonState.Normal);

                Rectangle normalRegion =
                    new Rectangle(
                    cellBounds.Location.X + 1 + 25,
                    cellBounds.Location.Y,
                    cellBounds.Size.Width - 26,
                    cellBounds.Size.Height);

                graphics.DrawString(value.ToString(), cellStyle.Font, new SolidBrush(cellStyle.ForeColor), normalRegion);
            }

            protected override void OnMouseClick(DataGridViewCellMouseEventArgs e)
            {
                Rectangle rec = new Rectangle(new Point(0, 0), CheckBoxRegion.Size);
                isChecked = !isChecked;
                if (rec.Contains(e.Location))
                {
                    this.DataGridView.Invalidate();
                }
                base.OnMouseClick(e);
            }

            public bool IsChecked
            {
                get { return this.isChecked; }
                set { this.isChecked = value; }
            }
        }

        #endregion CheckBoxCell

           
    }

    #region CyLCDCharacter class

    public delegate void SelectSegmentDelegate(object sender);

    class CyLCDCharacter
    {
        public event SelectSegmentDelegate SegmentSelected;
        
        public int SegmentsCount;
        public CyLCDSegment[] Segments;
        private Bitmap m_Bmp;
        public PictureBox m_PBox;
        public int m_SelectedSegment = -1;
        public Point m_MouseClickLocation;

        private Size m_CharacterSize;

        public CyLCDCharacter(Size characterSize, CyHelperKind kind, string[] titles, Color highlightedColor)
        {
            this.m_CharacterSize = characterSize;

            m_PBox = new PictureBox();
            m_Bmp = new Bitmap(m_CharacterSize.Width, m_CharacterSize.Height);
            m_PBox.Size = m_CharacterSize;
            m_PBox.BorderStyle = BorderStyle.None;
            m_PBox.Image = m_Bmp;

            switch (kind)
            {
                case CyHelperKind.Segment7:
                    this.SegmentsCount = 7;
                    this.Segments = new CyLCDSegment[SegmentsCount];
                    for (int i = 0; i < SegmentsCount; i++)
                    {
                        CyLCDSegment newSegment = new CyLCDSegment(CreateSegment7(i), titles[i], highlightedColor);
                        Segments[i] = newSegment;
                    }
                    break;
                case CyHelperKind.Segment14:
                    this.SegmentsCount = 14;
                    this.Segments = new CyLCDSegment[SegmentsCount];
                    for (int i = 0; i < SegmentsCount; i++)
                    {
                        CyLCDSegment newSegment = new CyLCDSegment(CreateSegment14(i), titles[i], highlightedColor);
                        Segments[i] = newSegment;
                    }
                    break;
                case CyHelperKind.Segment16:
                    this.SegmentsCount = 16;
                    this.Segments = new CyLCDSegment[SegmentsCount];
                    for (int i = 0; i < SegmentsCount; i++)
                    {
                        CyLCDSegment newSegment = new CyLCDSegment(CreateSegment16(i), titles[i], highlightedColor);
                        Segments[i] = newSegment;
                    }
                    break;
                case CyHelperKind.Bar:
                    this.SegmentsCount = 1;
                    this.Segments = new CyLCDSegment[SegmentsCount];
                    for (int i = 0; i < SegmentsCount; i++)
                    {
                        CyLCDSegment newSegment = new CyLCDSegment(CreateSegmentBar(), titles[i], highlightedColor);
                        Segments[i] = newSegment;
                    }
                    break;
                default:
                    break;
            }

            m_PBox.MouseDown += new MouseEventHandler(PBox_MouseDown);
            m_PBox.MouseMove += new MouseEventHandler(PBox_MouseMove);
            m_PBox.Show();
            

        }

        public void DrawSegment(int num, bool isLight)
        {
            if (num < SegmentsCount)
            {
                Segments[num].IsHighlighted = isLight;
                DrawSegment(num);
            }
        }

        public void DrawSegment(int num)
        {
            if (num < SegmentsCount)
            {
                Segments[num].DrawSegment(m_Bmp);
                if (num == m_SelectedSegment)
                    Segments[num].DrawSegmentBorder(m_Bmp, true);
                m_PBox.Invalidate();
            }
        }

        public void RedrawAll()
        {
            Color backColor = Color.WhiteSmoke;
            Graphics g = Graphics.FromImage(m_Bmp);
            g.SmoothingMode = SmoothingMode.AntiAlias;
            g.Clear(backColor);
            for (int i = 0; i < SegmentsCount; i++)
            {
                Segments[i].DrawSegment(m_Bmp);
            }
            m_PBox.Invalidate();

        }

        //------------------------------------------------------------------------------------------------------

        void PBox_MouseDown(object sender, MouseEventArgs e)
        {
            m_PBox.Select();
            if (m_PBox.Focused)
            {
                m_SelectedSegment = SelectSegment(e.Location);
                SegmentSelected(this);

                m_MouseClickLocation = e.Location;
            }
        }

        void PBox_MouseMove(object sender, MouseEventArgs e)
        {
            if ((e.Button & MouseButtons.Left) > 0)
            {
                if (Math.Abs(e.X - m_MouseClickLocation.X) + Math.Abs(e.Y - m_MouseClickLocation.Y) > 3)
                {
                    if (m_SelectedSegment >= 0) //Drag only if any segment is selected
                        m_PBox.DoDragDrop(this, DragDropEffects.Move);
                }
            }
        }

        public void DeselectSegment()
        {
            m_SelectedSegment = -1;
            RedrawAll();
        }

        int SelectSegment(Point p)
        {
            int selectedIndex = -1;

            for (int i = 0; i < SegmentsCount; i++)
            {
                if (Segments[i].IsPointInSegment(p))
                {
                    selectedIndex = i;
                    for (int j = 0; j < SegmentsCount; j++)
                    {
                        if ((Segments[j].IsSelected) && (i != j))
                        {
                            RedrawAll();
                        }
                    }
                    Segments[i].IsSelected = true;
                    Segments[i].DrawSegmentBorder(m_Bmp, true);
                    m_PBox.Invalidate();
                    break;
                }
                else
                {
                    //Deselect all
                    RedrawAll();
                    m_PBox.Invalidate();
                }
            }

            return selectedIndex;
        }

        public void SelectSegment(int segmentNum)
        {
            m_SelectedSegment = segmentNum;
            for (int j = 0; j < SegmentsCount; j++)
            {
                if ((Segments[j].IsSelected) && (segmentNum != j))
                {
                    RedrawAll();
                }
            }
            Segments[segmentNum].IsSelected = true;
            Segments[segmentNum].DrawSegmentBorder(m_Bmp, true);
            m_PBox.Invalidate();
            SegmentSelected(this);
        }

        //------------------------------------------------------------------------------------------------------

        private GraphicsPath CreateSegment7(int segment)
        {
            int segWidth = m_Bmp.Width / 6;
            int fullWidth = m_Bmp.Width - 2 - segWidth/2;
            int fullHeight = m_Bmp.Height - 2;
            int space = 1;

            GraphicsPath path = new GraphicsPath();
            Point[] points = new Point[6];

            switch (segment)
            {
                //Top
                case 0:
                    points[0] = new Point(segWidth / 2 + space, segWidth / 2);
                    points[1] = new Point(segWidth + space, 0);
                    points[2] = new Point(fullWidth - (segWidth + space), 0);
                    points[3] = new Point(fullWidth - (segWidth / 2 + space), segWidth / 2);
                    points[4] = new Point(fullWidth - (segWidth + space), segWidth);
                    points[5] = new Point(segWidth + space, segWidth);
                    break;
                //Right upper
                case 1:
                    points[0] = new Point(fullWidth - segWidth / 2, segWidth / 2 + space);
                    points[1] = new Point(fullWidth, segWidth + space);
                    points[2] = new Point(fullWidth, fullHeight / 2 - (segWidth / 2 + space));
                    points[3] = new Point(fullWidth - segWidth / 2, fullHeight / 2 - space);
                    points[4] = new Point(fullWidth - segWidth, fullHeight / 2 - (segWidth / 2 + space));
                    points[5] = new Point(fullWidth - segWidth, segWidth + space);
                    break;
                //Right lower
                case 2:
                    points[0] = new Point(fullWidth - segWidth / 2, fullHeight / 2 + space);
                    points[1] = new Point(fullWidth, fullHeight / 2 + segWidth / 2 + space);
                    points[2] = new Point(fullWidth, fullHeight - (segWidth + space));
                    points[3] = new Point(fullWidth - segWidth / 2, fullHeight - (segWidth / 2 + space));
                    points[4] = new Point(fullWidth - segWidth, fullHeight - (segWidth + space));
                    points[5] = new Point(fullWidth - segWidth, fullHeight / 2 + segWidth / 2 + space);
                    break;
                //Bottom
                case 3:
                    points[0] = new Point(segWidth / 2 + space, fullHeight - segWidth / 2);
                    points[1] = new Point(segWidth + space, fullHeight - segWidth);
                    points[2] = new Point(fullWidth - (segWidth + space), fullHeight - segWidth);
                    points[3] = new Point(fullWidth - (segWidth / 2 + space), fullHeight - segWidth / 2);
                    points[4] = new Point(fullWidth - (segWidth + space), fullHeight);
                    points[5] = new Point(segWidth + space, fullHeight);
                    break;
                //Left lower
                case 4:
                    points[0] = new Point(segWidth / 2, fullHeight / 2 + space);
                    points[1] = new Point(segWidth, fullHeight / 2 + segWidth / 2 + space);
                    points[2] = new Point(segWidth, fullHeight - (segWidth + space));
                    points[3] = new Point(segWidth / 2, fullHeight - (segWidth / 2 + space));
                    points[4] = new Point(0, fullHeight - (segWidth + space));
                    points[5] = new Point(0, fullHeight / 2 + segWidth / 2 + space);
                    break;
                //Left upper
                case 5:
                    points[0] = new Point(segWidth / 2, segWidth / 2 + space);
                    points[1] = new Point(segWidth, segWidth + space);
                    points[2] = new Point(segWidth, fullHeight / 2 - (segWidth / 2 + space));
                    points[3] = new Point(segWidth / 2, fullHeight / 2 - space);
                    points[4] = new Point(0, fullHeight / 2 - (segWidth / 2 + space));
                    points[5] = new Point(0, segWidth + space);
                    break;
                //Horizontal
                case 6:
                    points[0] = new Point(segWidth / 2 + space, fullHeight / 2);
                    points[1] = new Point(segWidth + space, fullHeight / 2 - segWidth / 2);
                    points[2] = new Point(fullWidth - (segWidth + space), fullHeight / 2 - segWidth / 2);
                    points[3] = new Point(fullWidth - (segWidth / 2 + space), fullHeight / 2);
                    points[4] = new Point(fullWidth - (segWidth + space), fullHeight / 2 + segWidth / 2);
                    points[5] = new Point(segWidth + space, fullHeight / 2 + segWidth / 2);
                    break;
               
                default:
                    break;
            }
            // Circle
            if (segment == 7)
            {
                fullWidth += segWidth/2;
                path.AddEllipse(fullWidth - segWidth, fullHeight - segWidth, segWidth, segWidth);
            }
            else
                path.AddPolygon(points);
            
            return path;
        }

        //------------------------------------------------------------------------------------------------------

        private GraphicsPath CreateSegment14(int segment)
        {
            int segWidth = m_Bmp.Width / 7;
            int diagsegWidth = (int)(segWidth * Math.Sqrt(2));
            int fullWidth = m_Bmp.Width - 2;
            int fullHeight = m_Bmp.Height - 2;
            int space = 1;

            GraphicsPath path = new GraphicsPath();
            Point[] points = new Point[6];

            switch (segment)
            {
                //Top
                case 0:
                    points[0] = new Point(segWidth / 2 + space, segWidth / 2);
                    points[1] = new Point(segWidth + space, 0);
                    points[2] = new Point(fullWidth - (segWidth + space), 0);
                    points[3] = new Point(fullWidth - (segWidth / 2 + space), segWidth / 2);
                    points[4] = new Point(fullWidth - (segWidth + space), segWidth);
                    points[5] = new Point(segWidth + space, segWidth);
                    break;
                //Right upper
                case 1:
                    points[0] = new Point(fullWidth - segWidth / 2, segWidth / 2 + space);
                    points[1] = new Point(fullWidth, segWidth + space);
                    points[2] = new Point(fullWidth, fullHeight / 2 - (segWidth / 2 + space));
                    points[3] = new Point(fullWidth - segWidth / 2, fullHeight / 2 - space);
                    points[4] = new Point(fullWidth - segWidth, fullHeight / 2 - (segWidth / 2 + space));
                    points[5] = new Point(fullWidth - segWidth, segWidth + space);
                    break;
                //Right lower
                case 2:
                    points[0] = new Point(fullWidth - segWidth / 2, fullHeight / 2 + space);
                    points[1] = new Point(fullWidth, fullHeight / 2 + segWidth / 2 + space);
                    points[2] = new Point(fullWidth, fullHeight - (segWidth + space));
                    points[3] = new Point(fullWidth - segWidth / 2, fullHeight - (segWidth / 2 + space));
                    points[4] = new Point(fullWidth - segWidth, fullHeight - (segWidth + space));
                    points[5] = new Point(fullWidth - segWidth, fullHeight / 2 + segWidth / 2 + space);
                    break;
                //Bottom
                case 3:
                    points[0] = new Point(segWidth / 2 + space, fullHeight - segWidth / 2);
                    points[1] = new Point(segWidth + space, fullHeight - segWidth);
                    points[2] = new Point(fullWidth - (segWidth + space), fullHeight - segWidth);
                    points[3] = new Point(fullWidth - (segWidth / 2 + space), fullHeight - segWidth / 2);
                    points[4] = new Point(fullWidth - (segWidth + space), fullHeight);
                    points[5] = new Point(segWidth + space, fullHeight);
                    break;
                //Left lower
                case 4:
                    points[0] = new Point(segWidth / 2, fullHeight / 2 + space);
                    points[1] = new Point(segWidth, fullHeight / 2 + segWidth / 2 + space);
                    points[2] = new Point(segWidth, fullHeight - (segWidth + space));
                    points[3] = new Point(segWidth / 2, fullHeight - (segWidth / 2 + space));
                    points[4] = new Point(0, fullHeight - (segWidth + space));
                    points[5] = new Point(0, fullHeight / 2 + segWidth / 2 + space);
                    break;
                //Left upper
                case 5:
                    points[0] = new Point(segWidth / 2, segWidth / 2 + space);
                    points[1] = new Point(segWidth, segWidth + space);
                    points[2] = new Point(segWidth, fullHeight / 2 - (segWidth / 2 + space));
                    points[3] = new Point(segWidth / 2, fullHeight / 2 - space);
                    points[4] = new Point(0, fullHeight / 2 - (segWidth / 2 + space));
                    points[5] = new Point(0, segWidth + space);
                    break;
                //Horizontal left
                case 6:
                    points[0] = new Point(segWidth / 2 + space, fullHeight / 2);
                    points[1] = new Point(segWidth + space, fullHeight / 2 - segWidth / 2);
                    points[2] = new Point(fullWidth / 2 - (segWidth / 2 + space), fullHeight / 2 - segWidth / 2);
                    points[3] = new Point(fullWidth / 2 - space, fullHeight / 2);
                    points[4] = new Point(fullWidth / 2 - (segWidth / 2 + space), fullHeight / 2 + segWidth / 2);
                    points[5] = new Point(segWidth + space, fullHeight / 2 + segWidth / 2);
                    break;
                //Diagonal upper left
                case 7:
                    points[0] = new Point(fullWidth/2 - segWidth/2 - space, fullHeight/2 - (segWidth/2 + space));
                    points[1] = new Point(fullWidth/2 - segWidth/2 - space,
                                          fullHeight/2 - (segWidth/2 + space) - diagsegWidth);
                    points[2] = new Point(segWidth + space, segWidth + space);
                    points[3] = new Point(segWidth + space, segWidth + space + diagsegWidth);
                    points[4] = points[0];
                    points[5] = points[0];
                    break;
                //Vertical upper
                case 8:
                    points[0] = new Point(fullWidth / 2 + segWidth / 2, segWidth + space);
                    points[1] = new Point(fullWidth / 2 + segWidth / 2, fullHeight / 2 - (segWidth / 2 + space));
                    points[2] = new Point(fullWidth / 2, fullHeight / 2 - space);
                    points[3] = new Point(fullWidth / 2 - segWidth / 2, fullHeight / 2 - (segWidth / 2 + space));
                    points[4] = new Point(fullWidth / 2 - segWidth / 2, segWidth + space);
                    points[5] = points[0];
                    break;
                //Diagonal upper right
                case 9:
                    points[0] = new Point(fullWidth/2 + segWidth/2 + space, fullHeight/2 - (segWidth/2 + space));
                    points[1] = new Point(fullWidth/2 + segWidth/2 + space,
                                          fullHeight/2 - (segWidth/2 + space) - diagsegWidth);
                    points[2] = new Point(fullWidth - (segWidth + space), segWidth + space);
                    points[3] = new Point(fullWidth - (segWidth + space), segWidth + space + diagsegWidth);
                    points[4] = points[0];
                    points[5] = points[0];
                    break;
                //Horizontal right
                case 10:
                    points[0] = new Point(fullWidth / 2 + space, fullHeight / 2);
                    points[1] = new Point(fullWidth / 2 + segWidth / 2 + space, fullHeight / 2 - segWidth / 2);
                    points[2] = new Point(fullWidth - (segWidth + space), fullHeight / 2 - segWidth / 2);
                    points[3] = new Point(fullWidth - (segWidth / 2 + space), fullHeight / 2);
                    points[4] = new Point(fullWidth - (segWidth + space), fullHeight / 2 + segWidth / 2);
                    points[5] = new Point(fullWidth / 2 + segWidth / 2 + space, fullHeight / 2 + segWidth / 2);
                    break;
                //Diagonal lower right
                case 11:
                    points[0] = new Point(fullWidth/2 + segWidth/2 + space, fullHeight/2 + (segWidth/2 + space));
                    points[1] = new Point(fullWidth/2 + segWidth/2 + space,
                                          fullHeight/2 + (segWidth/2 + space) + diagsegWidth);
                    points[2] = new Point(fullWidth - (segWidth + space), fullHeight - (segWidth + space));
                    points[3] = new Point(fullWidth - (segWidth + space), fullHeight - 
                                          (segWidth + space + diagsegWidth));
                    points[4] = points[0];
                    points[5] = points[0];
                    break;
                //Vertical lower
                case 12:
                    points[0] = new Point(fullWidth / 2, fullHeight / 2 + space);
                    points[1] = new Point(fullWidth / 2 + segWidth / 2, fullHeight / 2 + segWidth / 2 + space);
                    points[2] = new Point(fullWidth / 2 + segWidth / 2, fullHeight - (segWidth + space));
                    points[3] = new Point(fullWidth / 2 - segWidth / 2, fullHeight - (segWidth + space));
                    points[4] = new Point(fullWidth / 2 - segWidth / 2, fullHeight / 2 + segWidth / 2 + space);
                    points[5] = points[0];
                    break;
                //Diagonal lower left
                case 13:
                    points[0] = new Point(fullWidth/2 - segWidth/2 - space, fullHeight/2 + (segWidth/2 + space));
                    points[1] = new Point(fullWidth/2 - segWidth/2 - space,
                                          fullHeight/2 + (segWidth/2 + space) + diagsegWidth);
                    points[2] = new Point(segWidth + space, fullHeight - (segWidth + space));
                    points[3] = new Point(segWidth + space, fullHeight - (segWidth + space + diagsegWidth));
                    points[4] = points[0];
                    points[5] = points[0];
                    break;
                default:
                    break;
            }
            path.AddPolygon(points);
            return path;
        }

        //------------------------------------------------------------------------------------------------------

        private GraphicsPath CreateSegment16(int segment)
        {
            int segWidth = m_Bmp.Width / 7;
            int diagsegWidth = (int)(segWidth * Math.Sqrt(2));
            int fullWidth = m_Bmp.Width - 2;
            int fullHeight = m_Bmp.Height - 2;
            int space = 1;

            GraphicsPath path = new GraphicsPath();
            Point[] points = new Point[6];

            switch (segment)
            {
                //Top left
                case 0:
                    points[0] = new Point(segWidth / 2 + space, segWidth / 2);
                    points[1] = new Point(segWidth + space, 0);
                    points[2] = new Point(fullWidth / 2 - (segWidth / 2 + space), 0);
                    points[3] = new Point(fullWidth / 2 - space, segWidth / 2);
                    points[4] = new Point(fullWidth / 2 - (segWidth / 2 + space), segWidth);
                    points[5] = new Point(segWidth + space, segWidth);
                    break;
                //Top right
                case 1:
                    points[0] = new Point(fullWidth / 2 + space, segWidth / 2);
                    points[1] = new Point(fullWidth / 2 + segWidth / 2 + space, 0);
                    points[2] = new Point(fullWidth - (segWidth + space), 0);
                    points[3] = new Point(fullWidth - (segWidth / 2 + space), segWidth / 2);
                    points[4] = new Point(fullWidth - (segWidth + space), segWidth);
                    points[5] = new Point(fullWidth / 2 + segWidth / 2 + space, segWidth);
                    break;
                //Right upper
                case 2:
                    points[0] = new Point(fullWidth - segWidth / 2, segWidth / 2 + space);
                    points[1] = new Point(fullWidth, segWidth + space);
                    points[2] = new Point(fullWidth, fullHeight / 2 - (segWidth / 2 + space));
                    points[3] = new Point(fullWidth - segWidth / 2, fullHeight / 2 - space);
                    points[4] = new Point(fullWidth - segWidth, fullHeight / 2 - (segWidth / 2 + space));
                    points[5] = new Point(fullWidth - segWidth, segWidth + space);
                    break;
                //Right lower
                case 3:
                    points[0] = new Point(fullWidth - segWidth / 2, fullHeight / 2 + space);
                    points[1] = new Point(fullWidth, fullHeight / 2 + segWidth / 2 + space);
                    points[2] = new Point(fullWidth, fullHeight - (segWidth + space));
                    points[3] = new Point(fullWidth - segWidth / 2, fullHeight - (segWidth / 2 + space));
                    points[4] = new Point(fullWidth - segWidth, fullHeight - (segWidth + space));
                    points[5] = new Point(fullWidth - segWidth, fullHeight / 2 + segWidth / 2 + space);
                    break;
                //Bottom right
                case 4:
                    points[0] = new Point(fullWidth / 2 + space, fullHeight - segWidth / 2);
                    points[1] = new Point(fullWidth / 2 + segWidth / 2 + space, fullHeight - segWidth);
                    points[2] = new Point(fullWidth - (segWidth + space), fullHeight - segWidth);
                    points[3] = new Point(fullWidth - (segWidth / 2 + space), fullHeight - segWidth / 2);
                    points[4] = new Point(fullWidth - (segWidth + space), fullHeight);
                    points[5] = new Point(fullWidth / 2 + segWidth / 2 + space, fullHeight);
                    break;
                //Bottom left
                case 5:
                    points[0] = new Point(segWidth / 2 + space, fullHeight - segWidth / 2);
                    points[1] = new Point(segWidth + space, fullHeight - segWidth);
                    points[2] = new Point(fullWidth / 2 - (segWidth / 2 + space), fullHeight - segWidth);
                    points[3] = new Point(fullWidth / 2 - space, fullHeight - segWidth / 2);
                    points[4] = new Point(fullWidth / 2 - (segWidth / 2 + space), fullHeight);
                    points[5] = new Point(segWidth + space, fullHeight);
                    break;
                //Left lower
                case 6:
                    points[0] = new Point(segWidth / 2, fullHeight / 2 + space);
                    points[1] = new Point(segWidth, fullHeight / 2 + segWidth / 2 + space);
                    points[2] = new Point(segWidth, fullHeight - (segWidth + space));
                    points[3] = new Point(segWidth / 2, fullHeight - (segWidth / 2 + space));
                    points[4] = new Point(0, fullHeight - (segWidth + space));
                    points[5] = new Point(0, fullHeight / 2 + segWidth / 2 + space);
                    break;
                //Left upper
                case 7:
                    points[0] = new Point(segWidth / 2, segWidth / 2 + space);
                    points[1] = new Point(segWidth, segWidth + space);
                    points[2] = new Point(segWidth, fullHeight / 2 - (segWidth / 2 + space));
                    points[3] = new Point(segWidth / 2, fullHeight / 2 - space);
                    points[4] = new Point(0, fullHeight / 2 - (segWidth / 2 + space));
                    points[5] = new Point(0, segWidth + space);
                    break;
                //Horizontal left
                case 8:
                    points[0] = new Point(segWidth / 2 + space, fullHeight / 2);
                    points[1] = new Point(segWidth + space, fullHeight / 2 - segWidth / 2);
                    points[2] = new Point(fullWidth / 2 - (segWidth / 2 + space), fullHeight / 2 - segWidth / 2);
                    points[3] = new Point(fullWidth / 2 - space, fullHeight / 2);
                    points[4] = new Point(fullWidth / 2 - (segWidth / 2 + space), fullHeight / 2 + segWidth / 2);
                    points[5] = new Point(segWidth + space, fullHeight / 2 + segWidth / 2);
                    break;
                //Diagonal upper left
                case 9:
                    points[0] = new Point(fullWidth/2 - segWidth/2 - space, fullHeight/2 - (segWidth/2 + space));
                    points[1] = new Point(fullWidth/2 - segWidth/2 - space,
                                          fullHeight/2 - (segWidth/2 + space) - diagsegWidth);
                    points[2] = new Point(segWidth + space, segWidth + space);
                    points[3] = new Point(segWidth + space, segWidth + space + diagsegWidth);
                    points[4] = points[0];
                    points[5] = points[0];
                    break;
                //Vertical upper
                case 10:
                    points[0] = new Point(fullWidth / 2, segWidth / 2 + space);
                    points[1] = new Point(fullWidth / 2 + segWidth / 2, segWidth + space);
                    points[2] = new Point(fullWidth / 2 + segWidth / 2, fullHeight / 2 - (segWidth / 2 + space));
                    points[3] = new Point(fullWidth / 2, fullHeight / 2 - space);
                    points[4] = new Point(fullWidth / 2 - segWidth / 2, fullHeight / 2 - (segWidth / 2 + space));
                    points[5] = new Point(fullWidth / 2 - segWidth / 2, segWidth + space);
                    break;
                //Diagonal upper right
                case 11:
                    points[0] = new Point(fullWidth/2 + segWidth/2 + space, fullHeight/2 - (segWidth/2 + space));
                    points[1] = new Point(fullWidth/2 + segWidth/2 + space,
                                          fullHeight/2 - (segWidth/2 + space) - diagsegWidth);
                    points[2] = new Point(fullWidth - (segWidth + space), segWidth + space);
                    points[3] = new Point(fullWidth - (segWidth + space), segWidth + space + diagsegWidth);
                    points[4] = points[0];
                    points[5] = points[0];
                    break;
                //Horizontal right
                case 12:
                    points[0] = new Point(fullWidth / 2 + space, fullHeight / 2);
                    points[1] = new Point(fullWidth / 2 + segWidth / 2 + space, fullHeight / 2 - segWidth / 2);
                    points[2] = new Point(fullWidth - (segWidth + space), fullHeight / 2 - segWidth / 2);
                    points[3] = new Point(fullWidth - (segWidth / 2 + space), fullHeight / 2);
                    points[4] = new Point(fullWidth - (segWidth + space), fullHeight / 2 + segWidth / 2);
                    points[5] = new Point(fullWidth / 2 + segWidth / 2 + space, fullHeight / 2 + segWidth / 2);
                    break;
                //Diagonal lower right
                case 13:
                    points[0] = new Point(fullWidth/2 + segWidth/2 + space, fullHeight/2 + (segWidth/2 + space));
                    points[1] = new Point(fullWidth/2 + segWidth/2 + space,
                                          fullHeight/2 + (segWidth/2 + space) + diagsegWidth);
                    points[2] = new Point(fullWidth - (segWidth + space), fullHeight - (segWidth + space));
                    points[3] = new Point(fullWidth - (segWidth + space), fullHeight - (
                                          segWidth + space + diagsegWidth));
                    points[4] = points[0];
                    points[5] = points[0];
                    break;
                //Vertical lower
                case 14:
                    points[0] = new Point(fullWidth / 2, fullHeight / 2 + space);
                    points[1] = new Point(fullWidth / 2 + segWidth / 2, fullHeight / 2 + segWidth / 2 + space);
                    points[2] = new Point(fullWidth / 2 + segWidth / 2, fullHeight - (segWidth + space));
                    points[3] = new Point(fullWidth / 2, fullHeight - (segWidth / 2 + space));
                    points[4] = new Point(fullWidth / 2 - segWidth / 2, fullHeight - (segWidth + space));
                    points[5] = new Point(fullWidth / 2 - segWidth / 2, fullHeight / 2 + segWidth / 2 + space);
                    break;
                //Diagonal lower left
                case 15:
                    points[0] = new Point(fullWidth/2 - segWidth/2 - space, fullHeight/2 + (segWidth/2 + space));
                    points[1] = new Point(fullWidth/2 - segWidth/2 - space,
                                          fullHeight/2 + (segWidth/2 + space) + diagsegWidth);
                    points[2] = new Point(segWidth + space, fullHeight - (segWidth + space));
                    points[3] = new Point(segWidth + space, fullHeight - (segWidth + space + diagsegWidth));
                    points[4] = points[0];
                    points[5] = points[0];
                    break;

                default:
                    break;
            }
            path.AddPolygon(points);
            return path;
        }

        //------------------------------------------------------------------------------------------------------

        private GraphicsPath CreateSegmentBar()
        {
            int fullWidth = m_Bmp.Width - 1;
            int fullHeight = m_Bmp.Height - 1;
            int segWidth = fullWidth-2;
            int segHeight = fullHeight-2;

            GraphicsPath path = new GraphicsPath();
            Point[] points = new Point[4];

            points[0] = new Point(0, 0);
            points[1] = new Point(segWidth, 0);
            points[2] = new Point(segWidth, segHeight);
            points[3] = new Point(0, segHeight);

            path.AddPolygon(points);
            return path;
        }
    }

    #endregion

    //==========================================================================================================

    #region CyLCDSegment class

    class CyLCDSegment
    {
        public string Title;
        public GraphicsPath SegmentPath;
        private bool m_IsHighlighted;
        private bool m_IsSelected;
        private bool m_IsMouseOver;
        private Color m_SegOnColor = Color.LightGreen;

        public bool IsHighlighted
        {
            get { return m_IsHighlighted; }
            set
            {
                m_IsHighlighted = value;
            }
        }

        public bool IsSelected
        {
            get { return m_IsSelected; }
            set
            {
                m_IsSelected = value;
            }
        }

        public bool IsMouseOver
        {
            get { return m_IsMouseOver; }
            set
            {
                m_IsMouseOver = value;
            }

        }

        public CyLCDSegment(GraphicsPath path, string title, Color highlightedColor)
        {
            this.SegmentPath = path;
            this.Title = title;
            m_SegOnColor = highlightedColor;
            IsHighlighted = false;
            IsSelected = false;
        }

        public void DrawSegment(Bitmap bmp)
        {
            Graphics g = Graphics.FromImage(bmp);
            g.SmoothingMode = SmoothingMode.AntiAlias;

            Color SegOffColor = Color.LightGray;

            if (IsHighlighted)
            {
                g.FillPath(new SolidBrush(m_SegOnColor), SegmentPath);
                IsHighlighted = true;
            }
            else
            {
                g.FillPath(new SolidBrush(SegOffColor), SegmentPath);
                IsHighlighted = false;
            }
            DrawTitle(g);
        }

        public void DrawSegmentBorder(Bitmap bmp, bool isLight)
        {
            Graphics g = Graphics.FromImage(bmp);
            g.SmoothingMode = SmoothingMode.AntiAlias;

            Color SegPressColor = Color.FromArgb(70,70,70);

            if (isLight)
            {
                g.DrawPath(new Pen(SegPressColor, 2), SegmentPath);
                IsSelected = true;
            }
            else
            {
                g.DrawPath(new Pen(m_SegOnColor, 2), SegmentPath);
                IsSelected = false;
            }
        }

        private void DrawTitle(Graphics g)
        {
            Color textColor = IsHighlighted ? Color.Black : Color.DarkGray;

            Font fnt = new Font("Arial", 8);
            RectangleF segBoundRect = SegmentPath.GetBounds();
            Size textSize = TextRenderer.MeasureText(Title, fnt);
            StringFormat drawFormat = new StringFormat();
            drawFormat.Alignment = StringAlignment.Center;
            drawFormat.LineAlignment = StringAlignment.Center;
            g.DrawString(Title, fnt, new SolidBrush(textColor), segBoundRect, drawFormat);

        }

        public bool IsPointInSegment(Point p)
        {
            bool result = false;
            Region segRegion = new Region(SegmentPath);
            if (segRegion.IsVisible(p))
            {
                result = true;
            }
            return result;
        }
    }

    #endregion

}