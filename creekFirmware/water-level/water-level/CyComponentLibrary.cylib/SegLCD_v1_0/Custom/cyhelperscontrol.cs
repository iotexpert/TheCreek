/*******************************************************************************
* Copyright 2008-2009, Cypress Semiconductor Corporation.  All rights reserved.
* You may use this file only in accordance with the license, terms, conditions, 
* disclaimers, and limitations in the end user license agreement accompanying 
* the software package with which this file was provided.
********************************************************************************/



using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Diagnostics;
using System.Drawing;
using System.Drawing.Drawing2D;
using System.Text.RegularExpressions;
using System.Windows.Forms;
using System.Collections;

namespace SegLCD_v1_0
{
    public enum HelperKind {Segment7, Segment14, Segment16, Bar, Matrix, Empty};

    public partial class CyHelpers : UserControl
    {
        private CustomTextBox editBox;

        public LCDParameters Parameters;

        private Size SymbolSize = new Size(60, 90);
        private Size SymbolBarSize = new Size(15, 90);
        private int SymbolSpace = 3;

        int currentHelperIndex = -1;
        private int selectedSymbolIndex = -1;
        private int selectedSegmentIndex = -1;

        ArrayList Symbols;

        public bool ParametersChanged = false;

        //------------------------------------------------------------------------------------------------------
        public CyHelpers()
        {
            InitializeComponent();
            InitDataGridMapCellStyle();
            Symbols = new ArrayList();
        }

        public CyHelpers(LCDParameters parameters)
        {
            InitializeComponent();
            InitDataGridMapCellStyle();
            Symbols = new ArrayList();
            this.Parameters = parameters;
            LoadHelpers();
        }


        #region Choose Helper

        private void buttonAddHelper_Click(object sender, EventArgs e)
        {
            if (listBoxAvailHelpers.SelectedIndex < 0)
                return;

            // If number of helpers is 8 - don't create a new helper
            if (Parameters.HelpersConfig.Count - 1 >= 8)
            {
                MessageBox.Show("Maximum number of helpers is 8.\n Helper could not be added.", "Warning", MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
                return;
            }

            // Check if there are enough commons and segments to handle a helper
            bool fitInSpace = true;
            switch (listBoxAvailHelpers.SelectedIndex)
            {
                case 0:
                    if (Parameters.NumCommonLines*Parameters.NumSegmentLines - HelperInfo.GetTotalPixelNumber(Parameters) < 8)
                    {
                        fitInSpace = false;
                        break;
                    }
                    HelperInfo.CreateHelper(HelperKind.Segment7, Parameters);   
                    break;
                case 1:
                    if (Parameters.NumCommonLines * Parameters.NumSegmentLines - HelperInfo.GetTotalPixelNumber(Parameters) < 14)
                    {
                        fitInSpace = false;
                        break;
                    }
                    HelperInfo.CreateHelper(HelperKind.Segment14, Parameters);  
                    break;
                case 2:
                    if (Parameters.NumCommonLines * Parameters.NumSegmentLines - HelperInfo.GetTotalPixelNumber(Parameters) < 16)
                    {
                        fitInSpace = false;
                        break;
                    }
                    HelperInfo.CreateHelper(HelperKind.Segment16, Parameters);
                    break;
                case 3:
                    if (Parameters.NumCommonLines * Parameters.NumSegmentLines - HelperInfo.GetTotalPixelNumber(Parameters) < 1)
                    {
                        fitInSpace = false;
                        break;
                    }
                    HelperInfo.CreateHelper(HelperKind.Bar, Parameters);
                    break;
                case 4:
                    if (Parameters.NumCommonLines * Parameters.NumSegmentLines - HelperInfo.GetTotalPixelNumber(Parameters) < 5*8)
                    {
                        fitInSpace = false;
                        break;
                    }
                    HelperInfo.CreateHelper(HelperKind.Matrix, Parameters);
                    break;
                default:
                    break;
            }

            // If number of common and segment lines is not enough - don't create a new helper
            if (!fitInSpace)
            {
                MessageBox.Show("Not enough segment and common lines to manage helpers.\n Helper could not be added.", "Warning", MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
                return;
            }

            listBoxAddedHelpers.Items.Add(Parameters.HelpersConfig[Parameters.HelpersConfig.Count - 1]);
            Parameters.HelpersConfig[Parameters.HelpersConfig.Count - 1].AddSymbol(AddNextSymbolIndex(Parameters.HelpersConfig[Parameters.HelpersConfig.Count - 1].Kind));

            Parameters.SerializedHelpers = HelperInfo.SerializeHelpers(Parameters.HelpersConfig);
        }

        public void LoadHelpers()
        {
            for (int i = 1; i < Parameters.HelpersConfig.Count; i++)
            {
                listBoxAddedHelpers.Items.Add(Parameters.HelpersConfig[i]);
            }

            // Remove used colors from the list
            foreach (HelperInfo helper in Parameters.HelpersConfig)
                Parameters.colorChooser.PopCl(helper.HelperColor);
        }

        private void listBoxAddedHelpers_SelectedIndexChanged(object sender, EventArgs e)
        {
            ResetSelection();

            if ((listBoxAddedHelpers.Items.Count == 0) || (listBoxAddedHelpers.SelectedIndex < 0))
            {
                panelHelperConfig.Visible = false;
                currentHelperIndex = -1;
            }
            else
            {
                panelHelperConfig.Visible = true;
                for (int i = 0; i < Parameters.HelpersConfig.Count; i++)
                {
                    if (Parameters.HelpersConfig[i].Name == listBoxAddedHelpers.SelectedItem.ToString())
                    {
                        if (currentHelperIndex != i)
                        {
                            currentHelperIndex = i;
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
            selectedSymbolIndex = -1;
            selectedSegmentIndex = -1;
            textBoxSegmentTitle.Enabled = false;
            textBoxSegmentTitle.Text = "";
        }

        #endregion Choose Helper

        //------------------------------------------------------------------------------------------------------

        #region Helper Configuration

        private void OpenNewHelper()
        {
            for (int i = Symbols.Count - 1; i >= 0; i--)
            {
                RemoveSymbol(i);
            }

            if (Parameters.HelpersConfig[currentHelperIndex].SymbolsCount > 0)
            {
                for (int i = 0; i < Parameters.HelpersConfig[currentHelperIndex].SymbolsCount; i++)
                    AddSymbol();
            }
            else
            {
                if (AddSymbol())
                    Parameters.HelpersConfig[currentHelperIndex].SymbolsCount++;
            }
        }

       

        //------------------------------------------------------------------------------------------------------

        private void buttonAddSymbol_Click(object sender, EventArgs e)
        {
            if (currentHelperIndex < 0)
                return;

            if ((Parameters.HelpersConfig[currentHelperIndex].SymbolsCount >=
                Parameters.HelpersConfig[currentHelperIndex].MaxSymbolsCount) || 
                (Parameters.HelpersConfig[currentHelperIndex].Kind == HelperKind.Empty))
                return;

            bool fitInSpace = true;
            switch (Parameters.HelpersConfig[currentHelperIndex].Kind)
            {
                case HelperKind.Segment7:
                    if (Parameters.NumCommonLines * Parameters.NumSegmentLines - HelperInfo.GetTotalPixelNumber(Parameters) < 8)
                    {
                        fitInSpace = false;
                        break;
                    }
                    break;
                case HelperKind.Segment14:
                    if (Parameters.NumCommonLines * Parameters.NumSegmentLines - HelperInfo.GetTotalPixelNumber(Parameters) < 14)
                    {
                        fitInSpace = false;
                        break;
                    }
                    break;
                case HelperKind.Segment16:
                    if (Parameters.NumCommonLines * Parameters.NumSegmentLines - HelperInfo.GetTotalPixelNumber(Parameters) < 16)
                    {
                        fitInSpace = false;
                        break;
                    }
                    break;
                case HelperKind.Bar:
                    if (Parameters.NumCommonLines * Parameters.NumSegmentLines - HelperInfo.GetTotalPixelNumber(Parameters) < 1)
                    {
                        fitInSpace = false;
                        break;
                    }
                    break;
                case HelperKind.Matrix:
                    if (Parameters.NumCommonLines * Parameters.NumSegmentLines - HelperInfo.GetTotalPixelNumber(Parameters) < 5 * 8)
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

            Parameters.HelpersConfig[currentHelperIndex].AddSymbol(AddNextSymbolIndex(Parameters.HelpersConfig[currentHelperIndex].Kind));
            ParametersChanged = true;
            AddSymbol();
        }

        private int AddNextSymbolIndex(HelperKind kind)
        {
            int index = 0;
            switch (kind)
            {
                case HelperKind.Segment7:
                    while (Parameters.SymbolIndexes_7SEG.Contains(index))
                        index++;
                    Parameters.SymbolIndexes_7SEG.Add(index);
                    break;
                case HelperKind.Segment14:
                    while (Parameters.SymbolIndexes_14SEG.Contains(index))
                        index++;
                    Parameters.SymbolIndexes_14SEG.Add(index);
                    break;
                case HelperKind.Segment16:
                    while (Parameters.SymbolIndexes_16SEG.Contains(index))
                        index++;
                    Parameters.SymbolIndexes_16SEG.Add(index);
                    break;
                case HelperKind.Bar:
                    while (Parameters.SymbolIndexes_BAR.Contains(index))
                        index++;
                    Parameters.SymbolIndexes_BAR.Add(index);
                    break;
                case HelperKind.Matrix:
                    while (Parameters.SymbolIndexes_MATRIX.Contains(index))
                        index++;
                    Parameters.SymbolIndexes_MATRIX.Add(index);
                    break;
                default:
                    break;
            }
            return index;
        }

        private void RemoveSymbolIndex(int index, HelperKind kind)
        {
            switch (kind)
            {
                case HelperKind.Segment7:
                    Parameters.SymbolIndexes_7SEG.Remove(index);
                    break;
                case HelperKind.Segment14:
                    Parameters.SymbolIndexes_14SEG.Remove(index);
                    break;
                case HelperKind.Segment16:
                    Parameters.SymbolIndexes_16SEG.Remove(index);
                    break;
                case HelperKind.Bar:
                    Parameters.SymbolIndexes_BAR.Remove(index);
                    break;
                case HelperKind.Matrix:
                    Parameters.SymbolIndexes_MATRIX.Remove(index);
                    break;
                default:
                    break;
            }
        }

        private bool AddSymbol()
        {
            int segCount = Parameters.HelpersConfig[currentHelperIndex].SegmentCount;
            string[] segmentTitles = new string[segCount];
            string pixelName, defaultName;
            for (int i = 0; i < segCount; i++)
            {
                pixelName = Parameters.HelpersConfig[currentHelperIndex].GetPixelBySymbolSegment(Symbols.Count,i).Name;
                defaultName = Parameters.HelpersConfig[currentHelperIndex].GetDefaultSymbolName(0);
                Regex defaultNamePattern = new Regex(defaultName.TrimEnd('_', '0') + "[0-9]+_");
                if (Parameters.HelpersConfig[currentHelperIndex].Kind == HelperKind.Bar)
                    defaultNamePattern = new Regex(defaultName);
                
                if (defaultNamePattern.IsMatch(pixelName))
                    segmentTitles[i] = pixelName.Remove(0, defaultName.Length);
                else
                    segmentTitles[i] = pixelName;
            }
            if (Symbols.Count < Parameters.HelpersConfig[currentHelperIndex].MaxSymbolsCount)
            {
                Size symSize = SymbolSize;
                //Size depends on symbol kind
                if (segCount == 1)
                    symSize = SymbolBarSize;
                LCDCharacter symbol = new LCDCharacter(symSize, Parameters.HelpersConfig[currentHelperIndex].Kind, segmentTitles, Parameters.HelpersConfig[currentHelperIndex].HelperColor);
                symbol.PBox.Location = new Point(SymbolSpace + (SymbolSpace + symSize.Width) * Symbols.Count, SymbolSpace);
                symbol.SegmentSelected += new SelectSegmentDelegate(symbol_SegmentSelected);
                symbol.PBox.ContextMenuStrip = contextMenuPixels;
                Symbols.Add(symbol);
                labelCharsNum.Text = Symbols.Count.ToString();
                panelDisplay.Controls.Add(symbol.PBox);
                symbol.RedrawAll();
            }
            return true;
        }

        void symbol_SegmentSelected(object sender)
        {
            if (sender == null)
            {
                for (int i = 0; i < Symbols.Count; i++)
                {
                    if (((LCDCharacter)Symbols[i]).SelectedSegment >= 0)
                    {
                        ((LCDCharacter)Symbols[i]).DeselectSegment();
                    }
                }

                textBoxSegmentTitle.Text = "";
                textBoxSegmentTitle.Enabled = false;
            }
            else
            {
                selectedSymbolIndex = Symbols.IndexOf(sender);
                selectedSegmentIndex = ((LCDCharacter) sender).SelectedSegment;
                for (int i = 0; i < Symbols.Count; i++)
                {
                    if ((((LCDCharacter) Symbols[i]).SelectedSegment >= 0) && (i != selectedSymbolIndex))
                    {
                        ((LCDCharacter) Symbols[i]).DeselectSegment();
                    }
                }
                if (selectedSegmentIndex < 0)
                {
                    textBoxSegmentTitle.Text = "";
                    textBoxSegmentTitle.Enabled = false;
                }
                else
                {
                    textBoxSegmentTitle.Enabled = true;
                    if ((currentHelperIndex >= 0) && (currentHelperIndex < Parameters.HelpersConfig.Count))
                    {
                        HelperSegmentInfo pixel = Parameters.HelpersConfig[currentHelperIndex].GetPixelBySymbolSegment(
                            selectedSymbolIndex, selectedSegmentIndex);
                        if (pixel != null)
                            textBoxSegmentTitle.Text = pixel.Name;
                    }
                }
            }
        }

        //------------------------------------------------------------------------------------------------------

        private void buttonRemoveSymbol_Click(object sender, EventArgs e)
        {
            if (currentHelperIndex < 0)
                return;

            if (Symbols.Count > 1)
            {
                for (int i = 0; i < Parameters.HelpersConfig[currentHelperIndex].HelpSegInfo.Count; i++)
                {
                    if (Parameters.HelpersConfig[currentHelperIndex].HelpSegInfo[i].DigitNum ==
                        Parameters.HelpersConfig[currentHelperIndex].SymbolsCount - 1)
                    {
                        RemoveSymbolIndex(Parameters.HelpersConfig[currentHelperIndex].HelpSegInfo[i].GlobalDigitNum,
                                          Parameters.HelpersConfig[currentHelperIndex].Kind);
                        Parameters.HelpersConfig[currentHelperIndex].HelpSegInfo.RemoveAt(i--);
                    }
                }
            
                Parameters.HelpersConfig[currentHelperIndex].SymbolsCount--;
                RemoveSymbol(Symbols.Count - 1);
                ParametersChanged = true;
            }
        }

        private void RemoveSymbol(int k)
        {
            //Check if it was selected
            if (((LCDCharacter)Symbols[k]).SelectedSegment >= 0)
                symbol_SegmentSelected(null);

            ((LCDCharacter)Symbols[k]).PBox.Dispose();
            Symbols.RemoveAt(k);
            labelCharsNum.Text = Symbols.Count.ToString();
        }

        private void textBoxSegmentTitle_TextChanged(object sender, EventArgs e)
        {
            if (textBoxSegmentTitle.Text == "" || currentHelperIndex < 0) return;
            SegmentInfo pixel = Parameters.HelpersConfig[currentHelperIndex].GetPixelBySymbolSegment(selectedSymbolIndex,
                                                                                 selectedSegmentIndex);
            if ((pixel != null) && (pixel.Name != textBoxSegmentTitle.Text))
            {
                pixel.Name = textBoxSegmentTitle.Text;

                if (Parameters.HelpersConfig[currentHelperIndex].Kind != HelperKind.Matrix)
                {
                    //Update title of LCD character's segments. (For Matrix helper - always empty).
                    string defaultName = Parameters.HelpersConfig[currentHelperIndex].GetDefaultSymbolName(0);
                    Regex defaultNamePattern = new Regex(defaultName.TrimEnd('_', '0') + "[0-9]+_");
                    if (Parameters.HelpersConfig[currentHelperIndex].Kind == HelperKind.Bar)
                        defaultNamePattern = new Regex(defaultName);

                    if (defaultNamePattern.IsMatch(pixel.Name))
                        ((LCDCharacter) Symbols[selectedSymbolIndex]).Segments[selectedSegmentIndex].Title =
                            pixel.Name.Remove(0, defaultName.Length);
                    else
                        ((LCDCharacter) Symbols[selectedSymbolIndex]).Segments[selectedSegmentIndex].Title = pixel.Name;
                }
                ((LCDCharacter)Symbols[selectedSymbolIndex]).DrawSegment(selectedSegmentIndex);
            }
        }

        private void buttonRemoveHelper_Click(object sender, EventArgs e)
        {
            if (listBoxAddedHelpers.SelectedIndex < 0)
                return;

            if (Parameters.HelpersConfig.Count > listBoxAddedHelpers.SelectedIndex + 1)
            {
                Parameters.colorChooser.PushCl(Parameters.HelpersConfig[listBoxAddedHelpers.SelectedIndex + 1].HelperColor);
                for (int i = 0; i < Parameters.HelpersConfig[listBoxAddedHelpers.SelectedIndex + 1].HelpSegInfo.Count; i++)
                {
                    RemoveSymbolIndex(Parameters.HelpersConfig[listBoxAddedHelpers.SelectedIndex + 1].HelpSegInfo[i].GlobalDigitNum,
                                      Parameters.HelpersConfig[listBoxAddedHelpers.SelectedIndex + 1].Kind);
                }
                HelperInfo.RemoveHelperIndex(Parameters.HelpersConfig[listBoxAddedHelpers.SelectedIndex + 1].GlobalHelperIndex,
                    Parameters.HelpersConfig[listBoxAddedHelpers.SelectedIndex + 1].Kind, Parameters);
                Parameters.HelpersConfig.RemoveAt(listBoxAddedHelpers.SelectedIndex + 1);
                listBoxAddedHelpers.Items.RemoveAt(listBoxAddedHelpers.SelectedIndex);

                Parameters.SerializedHelpers = HelperInfo.SerializeHelpers(Parameters.HelpersConfig);

                if (listBoxAddedHelpers.Items.Count > 0)
                    listBoxAddedHelpers.SelectedIndex = 0;
            }
        }

        private void textBoxSegmentTitle_Validated(object sender, EventArgs e)
        {
            UpdateDataGridMapValues();
            ParametersChanged = true;
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

        private void UpdateDataGridMapValues()
        {
            for (int i = 1; i < dataGridMap.ColumnCount; i++)
                for (int j = 1; j < dataGridMap.RowCount; j++)
                {
                    dataGridMap[i, j].Value = null;
                    dataGridMap[i, j].Style = dataGridMap.DefaultCellStyle;

                    if (Parameters.DisabledCommons.Contains(Parameters.NumCommonLines - i)) 
                        dataGridMap[i, j].Style = cellDisabledStyle;
                }
            for (int i = 0; i < Parameters.HelpersConfig.Count; i++)
                for (int j = 0; j < Parameters.HelpersConfig[i].HelpSegInfo.Count; j++)
                {
                    if ((Parameters.HelpersConfig[i].HelpSegInfo[j].Common >= 0) &&
                        (Parameters.HelpersConfig[i].HelpSegInfo[j].Segment >= 0))
                    {
                        dataGridMap[Parameters.NumCommonLines - Parameters.HelpersConfig[i].HelpSegInfo[j].Common,
                            Parameters.HelpersConfig[i].HelpSegInfo[j].Segment + 1].Value = Parameters.HelpersConfig[i].HelpSegInfo[j].Name;
                        if (i != 0)
                        {
                            dataGridMap[Parameters.NumCommonLines - Parameters.HelpersConfig[i].HelpSegInfo[j].Common,
                                        Parameters.HelpersConfig[i].HelpSegInfo[j].Segment + 1].Style = new DataGridViewCellStyle(CellBusyStyle);
                            dataGridMap[Parameters.NumCommonLines - Parameters.HelpersConfig[i].HelpSegInfo[j].Common,
                                        Parameters.HelpersConfig[i].HelpSegInfo[j].Segment + 1].Style.BackColor =
                                Parameters.HelpersConfig[i].HelperColor;
                        }
                    }
                }
        }

        private void CyHelpers_VisibleChanged(object sender, EventArgs e)
        {
            // Common, segment mapping
            if (Visible)
            {
                ParametersChanged = false;

                //Update Empty helper
                HelperInfo.UpdateEmptyHelper(Parameters);

                //Update Disabled Commons array
                foreach (int val in Parameters.DisabledCommons)
                {
                    if (val >= Parameters.NumCommonLines)
                    {
                        List<int> DisabledCommons = new List<int>(Parameters.DisabledCommons);
                        DisabledCommons.Remove(val);
                        Parameters.DisabledCommons = DisabledCommons;
                    }
                }

                //Update dataGridMap
                dataGridMap.Rows.Clear();
                dataGridMap.Columns.Clear();
                for (int i = 0; i < Parameters.NumCommonLines+1; i++)
                {
                    dataGridMap.Columns.Add(i.ToString(), "-");
                }
                for (int j = 0; j < Parameters.NumSegmentLines+1; j++)
                {
                    dataGridMap.Rows.Add();
                }

                dataGridMap.Rows[0].Frozen = true;
                dataGridMap.Columns[0].Frozen = true;

                dataGridMap.Columns[0].Width = 40;
                for (int i = 0; i < Parameters.NumCommonLines; i++)
                {
                    //CheckBoxColumnHeader cbCell = new CheckBoxColumnHeader();
                    //dataGridMap[i + 1, 0] = cbCell;
                    dataGridMap.Columns[i + 1].Width = 65;
                    dataGridMap[i + 1, 0].Value = "Com" + (Parameters.NumCommonLines - i - 1);
                    
                    //if (Parameters.DisabledCommons.Contains(Parameters.NumCommonLines - i - 1))
                    //    cbCell.IsChecked = false;
                    dataGridMap[i + 1, 0].Style = CellCommonStyle;
                }
                for (int j = 0; j < Parameters.NumSegmentLines; j++)
                {
                    dataGridMap[0, j + 1].Value = "Seg"+(j).ToString();
                    dataGridMap[0, j + 1].Style = CellSegmentStyle;
                }

                //Change dataGridMap size
                int w = 0;
                for (int i = 0; i < dataGridMap.ColumnCount; i++)
                    w += dataGridMap.Columns[i].Width;
                dataGridMap.Width = w+10;

                int h = 0;
                for (int i = 0; i < dataGridMap.RowCount; i++)
                    h += dataGridMap.Rows[i].Height;
                dataGridMap.Height = h+10;

                UpdateDataGridMapValues();
            }
        }

        

        private void dataGridMap_CellClick(object sender, DataGridViewCellEventArgs e)
        {
            if ((e.ColumnIndex > 0) && (e.RowIndex > 0) && (dataGridMap[e.ColumnIndex, e.RowIndex].Value != null))
            {
                string pixelName = (string)dataGridMap[e.ColumnIndex, e.RowIndex].Value;
                foreach (HelperInfo helper in Parameters.HelpersConfig)
                {
                    foreach (HelperSegmentInfo pixel in helper.HelpSegInfo)
                    {
                        if (pixel.Name == pixelName)
                        {
                            if (Parameters.HelpersConfig.IndexOf(helper) > 0)
                            {
                                listBoxAddedHelpers.SelectedIndex = Parameters.HelpersConfig.IndexOf(helper) - 1;
                                //Select segment
                                ((LCDCharacter) Symbols[pixel.DigitNum]).SelectSegment(pixel.RelativePos);
                                
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
            //        EnableCommon(Parameters.NumCommonLines - e.ColumnIndex);
            //    else
            //        DisableCommon(Parameters.NumCommonLines - e.ColumnIndex);
            //}
        }

        private void EnableCommon(int index)
        {
            List<int> DisabledCommons = new List<int>(Parameters.DisabledCommons);
            DisabledCommons.Remove(index);
            Parameters.DisabledCommons = DisabledCommons;
            for (int i = 1; i < dataGridMap.RowCount; i++)
            {
                dataGridMap[Parameters.NumCommonLines - index, i].Style = dataGridMap.DefaultCellStyle;
            }
            //dataGridMap[Parameters.NumCommonLines - index, 0].Style = CellCommonStyle;
        }

        private void DisableCommon(int index)
        {
            if (!Parameters.DisabledCommons.Contains(index))
            {
                List<int> DisabledCommons = new List<int>(Parameters.DisabledCommons);
                DisabledCommons.Add(index);
                Parameters.DisabledCommons = DisabledCommons;
            }
            for (int i = 1; i < dataGridMap.RowCount; i++)
            {
                ResetPixelCommonSegment(dataGridMap[Parameters.NumCommonLines - index, i].Value.ToString());
            }
            for (int i = 1; i < dataGridMap.RowCount; i++)
            {
                dataGridMap[Parameters.NumCommonLines - index, i].Style = cellDisabledStyle;
            }
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
                if ((hit.RowIndex > 0) && (hit.ColumnIndex > 0) && dataGridMap[hit.ColumnIndex, hit.RowIndex].Style != cellDisabledStyle)
                {
                    e.Effect = e.Data.GetDataPresent(typeof(LCDCharacter)) ? DragDropEffects.Move : DragDropEffects.None;
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
                dataGridMap.CurrentCell = dataGridMap[dataGridMap.CurrentCell.ColumnIndex, dataGridMap.CurrentCell.RowIndex+1];
            }
            else if (localPoint.Y < 10) //if moving upwards
            {
                dataGridMap.CurrentCell = dataGridMap[dataGridMap.CurrentCell.ColumnIndex, dataGridMap.CurrentCell.RowIndex - 1];
            }
            //Scroll horizontal
            if (localPoint.X >= dataGridMap.Width - 20) //if moving to the right 
            {
                dataGridMap.CurrentCell = dataGridMap[dataGridMap.CurrentCell.ColumnIndex + 1, dataGridMap.CurrentCell.RowIndex];
            }
            else if (localPoint.X < 20) //if moving to the left
            {
                dataGridMap.CurrentCell = dataGridMap[dataGridMap.CurrentCell.ColumnIndex - 1, dataGridMap.CurrentCell.RowIndex];
            }
        }

        private void dataGridMap_DragDrop(object sender, DragEventArgs e)
        {
            Point localPoint = dataGridMap.PointToClient(new Point(e.X, e.Y));
            DataGridView.HitTestInfo hit = dataGridMap.HitTest(localPoint.X, localPoint.Y);
            if (hit.Type == DataGridViewHitTestType.Cell)
            {
                if ((hit.RowIndex > 0) && (hit.ColumnIndex > 0) && (dataGridMap[hit.ColumnIndex, hit.RowIndex].Style != cellDisabledStyle))
                {
                    LCDCharacter lcdChar = (LCDCharacter)e.Data.GetData(typeof(LCDCharacter));
                    SegmentInfo pixel1 =
                       Parameters.HelpersConfig[currentHelperIndex].GetPixelBySymbolSegment(selectedSymbolIndex,
                                                                                            selectedSegmentIndex);
                    ResetPixelCommonSegment(pixel1.Name);
                    lcdChar.DrawSegment(lcdChar.SelectedSegment, true);
                    dataGridMap[hit.ColumnIndex, hit.RowIndex].Style = new DataGridViewCellStyle(CellBusyStyle);
                    dataGridMap[hit.ColumnIndex, hit.RowIndex].Style.BackColor = Parameters.HelpersConfig[currentHelperIndex].HelperColor;

                    //Set common and segment for pixel
                    pixel1.Common = Parameters.NumCommonLines - hit.ColumnIndex;
                    pixel1.Segment = hit.RowIndex - 1;
                    if (dataGridMap[hit.ColumnIndex, hit.RowIndex].Value != null)
                    {
                        ResetPixelCommonSegment(dataGridMap[hit.ColumnIndex, hit.RowIndex].Value.ToString());
                    }
                    dataGridMap[hit.ColumnIndex, hit.RowIndex].Value = pixel1.Name;

                    ParametersChanged = true;
                }
            }
        }

        private void ResetPixelCommonSegment(string name)
        {
            // Omit first helper (Empty helper). Its values never reset.
            for (int i = 1; i < Parameters.HelpersConfig.Count; i++)
            {
                {
                    SegmentInfo pixel2 =
                        Parameters.HelpersConfig[i].GetPixelByName(name);
                    if (pixel2 != null)
                    {
                        pixel2.Common = -1;
                        pixel2.Segment = -1;
                        ParametersChanged = true;
                        UpdateLCDChars();
                        UpdateDataGridMapValues();
                        break;
                    }
                }
            }
        }

        private void UpdateLCDChars()
        {
            if (currentHelperIndex < 0) return;

            for (int i = 0; i < Symbols.Count; i++)
                for (int j = 0; j < ((LCDCharacter)Symbols[i]).SegmentsCount; j++)
                {
                    SegmentInfo pixel1 =
                       Parameters.HelpersConfig[currentHelperIndex].GetPixelBySymbolSegment(i,j);
                    if ((pixel1 != null) && ((pixel1.Common >= 0) && (pixel1.Segment >= 0)))
                        ((LCDCharacter)Symbols[i]).DrawSegment(j, true);
                    else
                        ((LCDCharacter)Symbols[i]).DrawSegment(j, false);
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
                for (int i = 0; i < Symbols.Count; i++)
                {
                    if (((LCDCharacter)Symbols[i]).SelectedSegment < 0)
                    {
                        e.Cancel = true;
                        break;
                    }
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
                for (int i = 0; i < Symbols.Count; i++)
                {
                    if (((LCDCharacter)Symbols[i]).SelectedSegment >= 0)
                    {
                        SegmentInfo pixel1 =
                            Parameters.HelpersConfig[currentHelperIndex].GetPixelBySymbolSegment(i, ((LCDCharacter)Symbols[i]).SelectedSegment);
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
                    if ((hit.RowIndex > 0) && (hit.ColumnIndex > 0) && (dataGridMap[hit.ColumnIndex, hit.RowIndex].Value != null))
                    {
                        ResetPixelCommonSegment((string)dataGridMap[hit.ColumnIndex,hit.RowIndex].Value);
                    }
                }
            }
        }

        #endregion


        private void CyHelpers_Leave(object sender, EventArgs e)
        {
            // Save changes
            if (ParametersChanged)
                Parameters.SerializedHelpers = HelperInfo.SerializeHelpers(Parameters.HelpersConfig);
            ParametersChanged = false;
        }

        private void textBoxSegmentTitle_Validating(object sender, CancelEventArgs e)
        {
           if (!HelperInfo.CheckPixelUniqueName(Parameters, textBoxSegmentTitle.Text))
           {
               MessageBox.Show("The name " + textBoxSegmentTitle.Text + " is not unique. Please, enter another name.", "Warning",
                               MessageBoxButtons.OK, MessageBoxIcon.Warning);
               e.Cancel = true;
           }
        }

        //------------------------------------------------------------------------------------------------------
        #region Printing

        private enum PixelCellStyle { Header, Value } ;

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

        private void printDocumentPixelMap_PrintPage(object sender, System.Drawing.Printing.PrintPageEventArgs e)
        {
            e.PageSettings.Landscape = true;
            printDocumentPixelMap.OriginAtMargins = true;

            Point cellOrigin = new Point(0,0);
            Size cellSize = new Size();
            int verticalShift = 0;
            Rectangle boundRect;
            for (int i = 0; i < dataGridMap.ColumnCount; i++)
            {
                cellOrigin.Y = verticalShift;
                cellSize.Width = (int)(dataGridMap.Columns[i].Width * 1.5);
                for (int j = 0; j < dataGridMap.RowCount; j++)
                {
                    cellSize.Height = (int)(dataGridMap[i, j].Size.Height * 1.3);

                    if (dataGridMap[i, j].Value != null)
                    {
                        if ((i == 0) || (j == 0))
                            PrintCell(e.Graphics, cellOrigin, cellSize, dataGridMap[i, j].Value.ToString(), PixelCellStyle.Header);
                        else
                            PrintCell(e.Graphics, cellOrigin, cellSize, dataGridMap[i, j].Value.ToString(), PixelCellStyle.Value);
                    }
                    cellOrigin.Y += cellSize.Height;
                }
                cellOrigin.X += cellSize.Width;

                // if page is too small
                if (cellOrigin.X > e.MarginBounds.Right - e.MarginBounds.Left)
                {
                    boundRect = new Rectangle(0, 0, cellOrigin.X, cellOrigin.Y);
                    e.Graphics.DrawRectangle(new Pen(Color.LightGray, 2), boundRect);

                    verticalShift = cellOrigin.Y + cellSize.Height;
                    cellOrigin.X = 0;

                    //Draw first column again
                    cellSize.Width = (int)(dataGridMap.Columns[0].Width * 1.5);
                    cellOrigin.Y = verticalShift;
                    for (int j = 0; j < dataGridMap.RowCount; j++)
                    {
                        cellSize.Height = (int)(dataGridMap[0, j].Size.Height * 1.3);

                        if (dataGridMap[0, j].Value != null)
                        {
                             PrintCell(e.Graphics, cellOrigin, cellSize, dataGridMap[0, j].Value.ToString(), PixelCellStyle.Header);
                        }
                        cellOrigin.Y += cellSize.Height;
                    }
                    cellOrigin.X += cellSize.Width;
                }
            }
            boundRect = new Rectangle(0, verticalShift, cellOrigin.X, cellOrigin.Y-verticalShift);
            e.Graphics.DrawRectangle(new Pen(Color.LightGray, 2), boundRect);
        }

        private void PrintCell(Graphics g, Point origin, Size size, string text, PixelCellStyle style)
        {
            Font font = new Font("Arial", 10);
            StringFormat format = new StringFormat();
            format.Alignment = StringAlignment.Center;
            format.LineAlignment = StringAlignment.Center;
            Rectangle boundRect = new Rectangle(origin, size);
            
            if (style == PixelCellStyle.Header)
                g.FillRectangle(new SolidBrush(Color.LightSteelBlue),boundRect);
            else if (style == PixelCellStyle.Value)
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
            editBox = new CustomTextBox();
			editBox.Location = new Point(0,0);
			editBox.Size = new Size(0,0);
			editBox.Hide();
			listBoxAddedHelpers.Controls.Add(editBox);			
			editBox.Text = "";
			editBox.BorderStyle = BorderStyle.FixedSingle;
            editBox.KeyPress += new KeyPressEventHandler(editBox_KeyPress);
            editBox.LostFocus += new EventHandler(editBox_LostFocus);
            editBox.Validating +=new CancelEventHandler(editBox_Validating);
        }

        void  editBox_Validating(object sender, CancelEventArgs e)
        {
            if (!HelperInfo.CheckHelperUniqueName(Parameters, editBox.Text))
            {
                // Omit case when the name wasn't changed
                if (((HelperInfo)listBoxAddedHelpers.Items[listBoxAddedHelpers.SelectedIndex]).Name != editBox.Text)
                {
                    MessageBox.Show("The name " + editBox.Text + " is not unique. Please, enter another name.",
                                    "Warning", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                    e.Cancel = true;
                    editBox.Show();
                }
            }
        }

        void editBox_LostFocus(object sender, EventArgs e)
        {
            ((HelperInfo)listBoxAddedHelpers.Items[listBoxAddedHelpers.SelectedIndex]).Name = editBox.Text;
            ParametersChanged = true;
            editBox.Hide();
            listBoxAddedHelpers.Refresh();
        }

        void editBox_KeyPress(object sender, KeyPressEventArgs e)
        {
            if (e.KeyChar == 13)
            {
                ((HelperInfo)listBoxAddedHelpers.Items[listBoxAddedHelpers.SelectedIndex]).Name = editBox.Text;
                ParametersChanged = true;
                editBox.Hide();
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
			
			editBox.Location = new System.Drawing.Point(r.X + delta , r.Y + delta ) ;
			editBox.Size 
                 = new System.Drawing.Size(r.Width -10 , r.Height- delta);
			editBox.Show();
			editBox.Text = itemText;
			editBox.Focus();
			editBox.SelectAll(); 
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

        //======================================================================================================
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

    #region LCDCharacter class

    public delegate void SelectSegmentDelegate(object sender);

    class LCDCharacter
    {
        public event SelectSegmentDelegate SegmentSelected;
        
        public int SegmentsCount;
        public LCDSegment[] Segments;
        private Bitmap Bmp;
        public PictureBox PBox;
        public int SelectedSegment = -1;
        public Point MouseClickLocation;

        private Size CharacterSize;

        public LCDCharacter(Size characterSize, HelperKind kind, string[] titles, Color highlightedColor)
        {
            this.CharacterSize = characterSize;

            PBox = new PictureBox();
            Bmp = new Bitmap(CharacterSize.Width, CharacterSize.Height);
            PBox.Size = CharacterSize;
            PBox.BorderStyle = BorderStyle.None;
            PBox.Image = Bmp;

            switch (kind)
            {
                case HelperKind.Segment7:
                    this.SegmentsCount = 8;
                    this.Segments = new LCDSegment[SegmentsCount];
                    for (int i = 0; i < SegmentsCount; i++)
                    {
                        LCDSegment newSegment = new LCDSegment(CreateSegment7(i), titles[i], highlightedColor);
                        Segments[i] = newSegment;
                    }
                    break;
                case HelperKind.Segment14:
                    this.SegmentsCount = 14;
                    this.Segments = new LCDSegment[SegmentsCount];
                    for (int i = 0; i < SegmentsCount; i++)
                    {
                        LCDSegment newSegment = new LCDSegment(CreateSegment14(i), titles[i], highlightedColor);
                        Segments[i] = newSegment;
                    }
                    break;
                case HelperKind.Segment16:
                    this.SegmentsCount = 16;
                    this.Segments = new LCDSegment[SegmentsCount];
                    for (int i = 0; i < SegmentsCount; i++)
                    {
                        LCDSegment newSegment = new LCDSegment(CreateSegment16(i), titles[i], highlightedColor);
                        Segments[i] = newSegment;
                    }
                    break;
                case HelperKind.Bar:
                    this.SegmentsCount = 1;
                    this.Segments = new LCDSegment[SegmentsCount];
                    for (int i = 0; i < SegmentsCount; i++)
                    {
                        LCDSegment newSegment = new LCDSegment(CreateSegmentBar(), titles[i], highlightedColor);
                        Segments[i] = newSegment;
                    }
                    break;
                case HelperKind.Matrix:
                    this.SegmentsCount = 5*8;
                    this.Segments = new LCDSegment[SegmentsCount];
                    for (int i = 0; i < SegmentsCount; i++)
                    {
                        LCDSegment newSegment = new LCDSegment(CreateSegmentMatrix(5, 8, i), "", highlightedColor);
                        Segments[i] = newSegment;
                    }
                    break;
                default:
                    break;
            }

            PBox.MouseDown += new MouseEventHandler(PBox_MouseDown);
            PBox.MouseMove += new MouseEventHandler(PBox_MouseMove);
            PBox.Show();
            

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
                Segments[num].DrawSegment(Bmp);
                if (num == SelectedSegment)
                    Segments[num].DrawSegmentBorder(Bmp, true);
                PBox.Invalidate();
            }
        }

        public void RedrawAll()
        {
            Color backColor = Color.WhiteSmoke;
            Graphics g = Graphics.FromImage(Bmp);
            g.SmoothingMode = SmoothingMode.AntiAlias;
            g.Clear(backColor);
            for (int i = 0; i < SegmentsCount; i++)
            {
                Segments[i].DrawSegment(Bmp);
            }
            PBox.Invalidate();

        }

        //------------------------------------------------------------------------------------------------------

        void PBox_MouseDown(object sender, MouseEventArgs e)
        {
            PBox.Select();
            if (PBox.Focused)
            {
                SelectedSegment = SelectSegment(e.Location);
                SegmentSelected(this);

                MouseClickLocation = e.Location;
            }
        }

        void PBox_MouseMove(object sender, MouseEventArgs e)
        {
            if ((e.Button & MouseButtons.Left) > 0)
            {
                if (Math.Abs(e.X - MouseClickLocation.X) + Math.Abs(e.Y - MouseClickLocation.Y) > 3)
                {
                    if (SelectedSegment >= 0) //Drag only if any segment is selected
                        PBox.DoDragDrop(this, DragDropEffects.Move);
                }
            }
        }

        public void DeselectSegment()
        {
            SelectedSegment = -1;
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
                    Segments[i].DrawSegmentBorder(Bmp, true);
                    PBox.Invalidate();
                    break;
                }
                else
                {
                    //Deselect all
                    RedrawAll();
                    PBox.Invalidate();
                }
            }

            return selectedIndex;
        }

        public void SelectSegment(int segmentNum)
        {
            SelectedSegment = segmentNum;
            for (int j = 0; j < SegmentsCount; j++)
            {
                if ((Segments[j].IsSelected) && (segmentNum != j))
                {
                    RedrawAll();
                }
            }
            Segments[segmentNum].IsSelected = true;
            Segments[segmentNum].DrawSegmentBorder(Bmp, true);
            PBox.Invalidate();
            SegmentSelected(this);
        }

        //------------------------------------------------------------------------------------------------------

        private GraphicsPath CreateSegment7(int segment)
        {
            int segWidth = Bmp.Width / 6;
            int fullWidth = Bmp.Width - 2 - segWidth/2;
            int fullHeight = Bmp.Height - 2;
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
            int segWidth = Bmp.Width / 7;
            int diagsegWidth = (int)(segWidth * Math.Sqrt(2));
            int fullWidth = Bmp.Width - 2;
            int fullHeight = Bmp.Height - 2;
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
                    points[0] = new Point(fullWidth / 2 - segWidth / 2 - space, fullHeight / 2 - (segWidth / 2 + space));
                    points[1] = new Point(fullWidth / 2 - segWidth / 2 - space, fullHeight / 2 - (segWidth / 2 + space) - diagsegWidth);
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
                    points[0] = new Point(fullWidth / 2 + segWidth / 2 + space, fullHeight / 2 - (segWidth / 2 + space));
                    points[1] = new Point(fullWidth / 2 + segWidth / 2 + space, fullHeight / 2 - (segWidth / 2 + space) - diagsegWidth);
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
                    points[0] = new Point(fullWidth / 2 + segWidth / 2 + space, fullHeight / 2 + (segWidth / 2 + space));
                    points[1] = new Point(fullWidth / 2 + segWidth / 2 + space, fullHeight / 2 + (segWidth / 2 + space) + diagsegWidth);
                    points[2] = new Point(fullWidth - (segWidth + space), fullHeight - (segWidth + space));
                    points[3] = new Point(fullWidth - (segWidth + space), fullHeight - (segWidth + space + diagsegWidth));
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
                    points[0] = new Point(fullWidth / 2 - segWidth / 2 - space, fullHeight / 2 + (segWidth / 2 + space));
                    points[1] = new Point(fullWidth / 2 - segWidth / 2 - space, fullHeight / 2 + (segWidth / 2 + space) + diagsegWidth);
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
            int segWidth = Bmp.Width / 7;
            int diagsegWidth = (int)(segWidth * Math.Sqrt(2));
            int fullWidth = Bmp.Width - 2;
            int fullHeight = Bmp.Height - 2;
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
                    points[0] = new Point(fullWidth / 2 - segWidth / 2 - space, fullHeight / 2 - (segWidth / 2 + space));
                    points[1] = new Point(fullWidth / 2 - segWidth / 2 - space, fullHeight / 2 - (segWidth / 2 + space) - diagsegWidth);
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
                    points[0] = new Point(fullWidth / 2 + segWidth / 2 + space, fullHeight / 2 - (segWidth / 2 + space));
                    points[1] = new Point(fullWidth / 2 + segWidth / 2 + space, fullHeight / 2 - (segWidth / 2 + space) - diagsegWidth);
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
                    points[0] = new Point(fullWidth / 2 + segWidth / 2 + space, fullHeight / 2 + (segWidth / 2 + space));
                    points[1] = new Point(fullWidth / 2 + segWidth / 2 + space, fullHeight / 2 + (segWidth / 2 + space) + diagsegWidth);
                    points[2] = new Point(fullWidth - (segWidth + space), fullHeight - (segWidth + space));
                    points[3] = new Point(fullWidth - (segWidth + space), fullHeight - (segWidth + space + diagsegWidth));
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
                    points[0] = new Point(fullWidth / 2 - segWidth / 2 - space, fullHeight / 2 + (segWidth / 2 + space));
                    points[1] = new Point(fullWidth / 2 - segWidth / 2 - space, fullHeight / 2 + (segWidth / 2 + space) + diagsegWidth);
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
            int fullWidth = Bmp.Width - 1;
            int fullHeight = Bmp.Height - 1;
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

        //------------------------------------------------------------------------------------------------------

        private GraphicsPath CreateSegmentMatrix(int totalSegX, int totalSegY, int segment)
        {
            int fullWidth = Bmp.Width - 1;
            int fullHeight = Bmp.Height - 1;
            int space = 1;
            int segWidth = (fullWidth - space * (totalSegX - 1)) / totalSegX;
            int segHeight = (fullHeight - space * (totalSegY - 1)) / totalSegY;
            int segX = segment % totalSegX;
            int segY = segment / totalSegX;

            GraphicsPath path = new GraphicsPath();
            Point[] points = new Point[4];

            points[0] = new Point((segWidth + space) * segX, (segHeight + space) * segY);
            points[1] = new Point((segWidth + space) * segX + segWidth, (segHeight + space) * segY);
            points[2] = new Point((segWidth + space) * segX + segWidth, (segHeight + space) * segY + segHeight);
            points[3] = new Point((segWidth + space) * segX, (segHeight + space) * segY + segHeight);

            path.AddPolygon(points);
            return path;
        }

    }

    #endregion

    //==========================================================================================================

    #region LCDSegment class

    class LCDSegment
    {
        public string Title;
        public GraphicsPath SegmentPath;
        bool _IsHighlighted;
        bool _IsSelected;
        bool _IsMouseOver;
        private Color SegOnColor = Color.LightGreen;

        public bool IsHighlighted
        {
            get { return _IsHighlighted; }
            set
            {
                _IsHighlighted = value;
            }
        }

        public bool IsSelected
        {
            get { return _IsSelected; }
            set
            {
                _IsSelected = value;
            }
        }

        public bool IsMouseOver
        {
            get { return _IsMouseOver; }
            set
            {
                _IsMouseOver = value;
            }

        }

        public LCDSegment(GraphicsPath path, string title, Color highlightedColor)
        {
            this.SegmentPath = path;
            this.Title = title;
            SegOnColor = highlightedColor;
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
                g.FillPath(new SolidBrush(SegOnColor), SegmentPath);
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
                g.DrawPath(new Pen(SegOnColor, 2), SegmentPath);
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