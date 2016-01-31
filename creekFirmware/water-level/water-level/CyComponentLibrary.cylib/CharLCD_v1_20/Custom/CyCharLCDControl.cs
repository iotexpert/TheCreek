/*******************************************************************************
* Copyright 2008-2009, Cypress Semiconductor Corporation.  All rights reserved.
* You may use this file only in accordance with the license, terms, conditions, 
* disclaimers, and limitations in the end user license agreement accompanying 
* the software package with which this file was provided.
********************************************************************************/



using System;
using System.Collections.Generic;
using System.Collections;
using System.ComponentModel;
using System.Drawing;
using System.Data;
using System.Text;
using System.Windows.Forms;
using CharLCD_v1_20;
using CyDesigner.Extensions.Gde;

namespace CharLCD_v1_20
{
    public partial class CyCharLCDControl : UserControl
    {

        // Object passed in by CyDesigner containing parameter values
        ICyInstEdit_v1 component;

        // CyDesigner Component Names
        string crParam = "ConversionRoutines";
        string customCharacterSetParam = "CustomCharacterSet";
        string characterParamName = "CUSTOM";

        const int VERTICAL_CHARACTER_OFFSET = 2;
        ArrayList cyCharacterStrings = new ArrayList();

        public CyCharLCDControl()
        {
            InitializeComponent();
        }
        
        public CyCharLCDControl(ICyInstEdit_v1 component)
        {
            this.component = component;
            InitializeComponent();

            // Add Arrow Key Event Handling For customCharacterGroupBox
            customCharacterGroupBox.arrowPressedEvent += new ArrowKeyPressEvent(customCharacterGroupBox_arrowPressedEvent);


            // Collections of each character set.
            userDefinedCharacters = new System.Collections.ArrayList();
            hbgCharacters = new System.Collections.ArrayList();
            vbgCharacters = new System.Collections.ArrayList();
            noneCharacters = new System.Collections.ArrayList();


            #region Populate Characters

            highlighted = this.character0;
            characterEditor.Match(highlighted);

            #region User Defined Characters

            userDefinedCharacters.Add(character0);
            userDefinedCharacters.Add(character1);
            userDefinedCharacters.Add(character2);
            userDefinedCharacters.Add(character3);
            userDefinedCharacters.Add(character4);
            userDefinedCharacters.Add(character5);
            userDefinedCharacters.Add(character6);
            userDefinedCharacters.Add(character7);
            #endregion

            LoadBarGraphs();
            CreateBarGraphs();

            #region Empty Characters for "None" character set
            noneCharacters.Add(customCharacter1);
            noneCharacters.Add(customCharacter2);
            noneCharacters.Add(customCharacter3);
            noneCharacters.Add(customCharacter4);
            noneCharacters.Add(customCharacter5);
            noneCharacters.Add(customCharacter6);
            noneCharacters.Add(customCharacter7);
            noneCharacters.Add(customCharacter8);
            #endregion
            #endregion
        }

        #region Generic Events (Button Clicks)
        // Event Handler when LCD Control is opened up.
        private void CyCharLCDControl_Load(object sender, EventArgs e)
        {
            InitializeParameters();
        }
                       
        // Event Handler for check box to include conversion routine.
        private void ConversionRoutines_Checked(object sender, EventArgs e)
        {
            CheckBox cb = (CheckBox)sender;
            if (cb.Checked)
            {
                component.SetParamExpr(crParam, "true");
                component.CommitParamExprs();

            }
            else
            {
                component.SetParamExpr(crParam, "false");
                component.CommitParamExprs();
            }
        }

        // Event handler for a change in character set selection.  <Horizontal, vertical Bargraphs and custom chars>
        private void CharacterSetSelection_Changed(object sender, EventArgs e)
        {
            RadioButton selectedButton = (RadioButton)sender;

            switch (selectedButton.Name)
            {
                case "noneRadioButton":
                    ShowCharacters(noneCharacters);
                    CustomCharactersOff();
                    HideCharacterEditor();
                    characterSet = CustomCharacterSetTypes.NONE;
                    currentEditableCharacter.Visible = false;
                    CharacterSetPostProcess();
                    break;
                case "vbgRadioButton":
                    ShowCharacters(vbgCharacters);
                    CustomCharactersOn();
                    HideCharacterEditor();
                    characterSet = CustomCharacterSetTypes.VERTICAL;
                    currentEditableCharacter.Visible = false;
                    CharacterSetPostProcess();
                    break;
                case "hbgRadioButton":
                    ShowCharacters(hbgCharacters);
                    CustomCharactersOn();
                    HideCharacterEditor();
                    characterSet = CustomCharacterSetTypes.HORIZONTAL;
                    currentEditableCharacter.Visible = false;
                    CharacterSetPostProcess();
                    break;
                case "udRadioButton":
                    CustomCharactersOn();
                    ShowCharacters(userDefinedCharacters);
                    characterSet = CustomCharacterSetTypes.USERDEFINED;
                    ShowCharacterEditor();
                    currentEditableCharacter.Text = highlighted.DisplayName;
                    currentEditableCharacter.Visible = true;
                    CharacterSetPostProcess();
                    break;
                default:
                    break;
            }
        }

        // Toggles for mouse down events.
        bool activate = false;
        bool buttonDown = false;

        #region Mouse Events on Custom Characters
        private void custArray_MouseDown(object sender, MouseEventArgs e)
        {
            CharLCD_v1_20.CustomCharacter current = (CharLCD_v1_20.CustomCharacter)sender;
            CharLCD_v1_20.Box clickedBox = current.GetBoxByLocation(e.X, e.Y);
            if (clickedBox != null)
            {
                clickedBox.IsActive = !clickedBox.IsActive;
                activate = clickedBox.IsActive;
                buttonDown = true;
                current.Invalidate();
            }
            this.highlighted.Invalidate();

        }

        private void custArray_MouseMove(object sender, MouseEventArgs e)
        {
            if (buttonDown)
            {
                CharLCD_v1_20.CustomCharacter current = (CharLCD_v1_20.CustomCharacter)sender;
                CharLCD_v1_20.Box currentBox = current.GetBoxByLocation(e.X, e.Y);
                if (currentBox != null)
                {
                    currentBox.IsActive = activate;
                    current.Invalidate();
                }
            }
            this.highlighted.Invalidate();
        }

        private void custArray_MouseUp(object sender, MouseEventArgs e)
        {
            highlighted.Invalidate();
            buttonDown = false;
        }

        #endregion

        #region Events on Custom Characters to change editable character
        private void custArrayHighlight_Click(object sender, MouseEventArgs e)
        {
            CharLCD_v1_20.CustomCharacter selectedCharacter = (CharLCD_v1_20.CustomCharacter)sender;
            UpdateHighlighted(selectedCharacter);
        }

        private void customCharacterGroupBox_arrowPressedEvent(ArrowArgs e)
        {
            switch (e.ArrowDirection)
            {
                case ArrowArgs.Direction.LEFT:
                    HighlightPrevious();
                    break;
                case ArrowArgs.Direction.RIGHT:
                    HighlightNext();
                    break;
                case ArrowArgs.Direction.UP:
                    HighlightAbove();
                    break;
                case ArrowArgs.Direction.DOWN:
                    HighlightBelow();
                    break;
                default:
                    break;
            }
        }
        #endregion
        
        #endregion
      
        #region Data Processing for the CyDesigner to Customizer Interface (Initialize, Post-Process, CheckErrors)

        private void InitializeParameters()
        {
            // Update Customizer controls to match parameters
            crCheckBox.Checked = Convert.ToBoolean(component.GetCommittedParam(crParam).Value);
            
            switch ((CustomCharacterSetTypes)byte.Parse(component.GetCommittedParam(customCharacterSetParam).Value))
            {
                case CustomCharacterSetTypes.HORIZONTAL:
                    hbgRadioButton.Checked = true;
                    break;
                case CustomCharacterSetTypes.VERTICAL:
                    vbgRadioButton.Checked = true;
                    break;
                case CustomCharacterSetTypes.USERDEFINED:
                    udRadioButton.Checked = true;
                    break;
                default:
                    noneRadioButton.Checked = true;
                    break;
            }

            UnpackageCyStrings();
        }

        /// <summary>
        /// Iterate through each parameter and check for errors.  If errors found. Report them with a message box.
        /// No Errors should be found in any parameter for this componenent as every paramater option is preselected.
        /// </summary>
        private void CheckParamErrors()
        {
            IEnumerable<String> names = component.GetParamNames();
            List<String> errorList = new List<String>();

            foreach (String paramName in names)
            {
                CyCompDevParam param = component.GetCommittedParam(paramName);
                if (param.ErrorCount != 0)
                {
                    foreach (String err in param.Errors)
                    {
                        MessageBox.Show(err);
                    }
                }
            }
        }

        #region Custom Character Processing for CyDesigner Parameters
        // These Methods handle the requirements of cydesigner strings.  CyStrings are not arrays of bytes,
        // but are arrays of a comma seperated hex values. 

        // Set appropriate parameters based on character set selection.
        private void CharacterSetPostProcess()
        {
            component.SetParamExpr(customCharacterSetParam, ((int)characterSet).ToString());
            bool rawr = component.CommitParamExprs();
        }

        // Calls CyStringToCharacter Foreach Character
        private void UnpackageCyStrings()
        {
            int stringIndex = 0;
            string value;

            for (int index = 0; index < 8; index++)
            {
                value = component.GetCommittedParam(characterParamName + index).Value.Trim('"');
                cyCharacterStrings.Add(new KeyValuePair<string, string>(characterParamName + index, value));
            }
            
            if (userDefinedCharacters.Count != cyCharacterStrings.Count)
                throw new Exception("Number of custom characters in CyDesigner does not match number supported by the the LCD Wizard");

            foreach (CustomCharacter character in userDefinedCharacters)
            {
                KeyValuePair<string, string> kvp = (KeyValuePair<string, string>)cyCharacterStrings[stringIndex++];
                string cyCharacterString = kvp.Value;

                if (cyCharacterString != null)
                    CYStringToCharacter(character, cyCharacterString);
            }
        }

        // Convert userDefined Characters to CyStrings
        private void ConvertUserDefinedCharactersToCY()
        {
            int stringIndex = 0;
            foreach (CustomCharacter character in userDefinedCharacters)
            {
                KeyValuePair<string, string> kvp = (KeyValuePair<string, string>)cyCharacterStrings[stringIndex++];
                string cyStringName = kvp.Key;
                string cyStringValue = "\"" + CharacterToCYString(character) + "\"";
                component.SetParamExpr(cyStringName, cyStringValue);
            }
        }

        // Return an 2 to the power of "power"  Helper Function.
        private int getExponent(int power)
        {
            if (power > 0)
                return 2 << power - 1;
            else
                return 1;
        }
        #endregion

        #region CY string parameter methods
        /// <summary>
        /// This method takes a string with each char (byte) representing the locations of active 
        /// on a CyCustomCharacterArray. 
        /// </summary>
        /// <param name="userDefined"> The instance of a character to be updated from CyDesigner data.</param>
        /// <param name="cyCharacter"> The string representing byte array from CyDesigner.</param>
        public void CYStringToCharacter(CustomCharacter customCharacter, string cyCharacterString)
        {
            char[] chars = new char[customCharacter.Rows];
            int index = 0;

            // Remove last comma and seperate into indivudual strings. 
            string[] hexCharacterArray = cyCharacterString.TrimEnd(',').Split(',');
            foreach (string rowValue in hexCharacterArray)
            {
                chars[index++] = (char)byte.Parse(rowValue, System.Globalization.NumberStyles.HexNumber);
            }

            CharLCD_v1_20.Box[,] boxes = customCharacter.GetBoxArray();

            for (int row = 0; row < customCharacter.Rows; row++)
            {
                for (int column = 0; column < customCharacter.Columns; column++)
                {
                    if ((((byte)chars[row]) & getExponent(customCharacter.Columns - 1 - column)) != 0)
                        boxes[row, column].IsActive = true;
                }
            }
        }

        /// <summary>
        /// This method takes a user defined custom character and converts the locations of active 
        /// pixels to an array of bytes, cast into a string to be stored on the LCD component object in CyDesigner.
        /// </summary>
        /// <param name="cyCharacter"> The string representing byte array from CyDesigner.</param>
        public string CharacterToCYString(CustomCharacter customCharacter)
        {
            // Comma Seperated String of Hex Values
            string cyCharacterString = "";
            CharLCD_v1_20.Box[,] pixelArray = customCharacter.GetBoxArray();

            // Indivudual row value to be calculated 
            byte rowValue;

            for (int row = 0; row < customCharacter.Rows; row++)
            {
                rowValue = 0;

                for (int column = 0; column < customCharacter.Columns; column++)
                {
                    if (pixelArray[row, column].IsActive)
                    {
                        // If active find out which pixel it is.  The 0th is the furthest right
                        // and has a value of 1.  'customCharacter.Columns - 1' is the furthest left
                        // and has a value of 2^(customCharacter.Columns -1).  Values in between are
                        // Exponential powers of 2 based on position. 
                        rowValue += (byte)getExponent(customCharacter.Columns - 1 - column);
                    }
                }
                cyCharacterString += rowValue.ToString("X") + ",";
            }
            // Convert Char arry to string and return it.
            return cyCharacterString;
        }
        #endregion

        private void custArray_Leave(object sender, EventArgs e)
        {
            ConvertUserDefinedCharactersToCY();
            component.CommitParamExprs();
        }

        #endregion

        #region
        /// <summary>
        ///  Populate the appropriate arraylists with the bargraph characters.
        /// </summary>
        private void LoadBarGraphs()
        {
            #region Horizontal Bar Graph Characters
            hbgCharacters.Add(hbg0);
            hbgCharacters.Add(hbg1);
            hbgCharacters.Add(hbg2);
            hbgCharacters.Add(hbg3);
            hbgCharacters.Add(hbg4);
            hbgCharacters.Add(hbg5);
            hbgCharacters.Add(hbg6);
            hbgCharacters.Add(hbg7);
            #endregion

            #region Vertical Bargraph Characters
            vbgCharacters.Add(vbg0);
            vbgCharacters.Add(vbg1);
            vbgCharacters.Add(vbg2);
            vbgCharacters.Add(vbg3);
            vbgCharacters.Add(vbg4);
            vbgCharacters.Add(vbg5);
            vbgCharacters.Add(vbg6);
            vbgCharacters.Add(vbg7);
            #endregion
        }

        #region Bar Graph Generation and Custom Character arraylist definitions/declarations

        // Character Sets
        System.Collections.ArrayList userDefinedCharacters;
        System.Collections.ArrayList hbgCharacters;
        System.Collections.ArrayList vbgCharacters;
        System.Collections.ArrayList noneCharacters;

        CustomCharacterSetTypes characterSet;
        // Pointer to the selected character.
        CustomCharacter highlighted;

        /*
         * Create bar graphs activates the appropriate pixels on two bargraph 
         * Sets, horizontal and vertical: May need adjustments in future.
         */
        private void CreateBarGraphs()
        {
            #region Horizontal Bargraph
            int largestColumn = 0;

            // foreach returns items in the order they were added to the ArrayList. 
            foreach (CustomCharacter character in hbgCharacters)
            {
                for (int index = 0; index < largestColumn; index++)
                {
                    character.SetColumn(index);
                }

                largestColumn++;

                if (largestColumn > character.Columns)
                    break;
            }
            #endregion

            #region Vertical Bargraph
            // Does not leave an empty character in the Custom Fonts
            int lastRow = 7;

            foreach (CustomCharacter character in vbgCharacters)
            {
                for (int index = 7; index >= lastRow; index--)
                {
                    character.SetRow(index);
                }
                lastRow--;
            }
            #endregion
        }
        #endregion




        #region Custom Character Highlighting Helper Methods

        private void HighlightNext()
        {
            // Find next.
            int next = userDefinedCharacters.IndexOf(highlighted) + 1;
            next = next % userDefinedCharacters.Count;
            // Cast from Object to CustomCharacter
            UpdateHighlighted((CharLCD_v1_20.CustomCharacter)userDefinedCharacters[next]);
        }

        private void HighlightPrevious()
        {
            int previous = userDefinedCharacters.IndexOf(highlighted) - 1;
            // Protects against '-1'.  if current is 7 previous should be 6 : 
            //7-1 = 6.  6+8 =14. 14%8 = 6.
            previous = (previous + userDefinedCharacters.Count) % userDefinedCharacters.Count;
            // Cast from Object to CustomCharacter
            UpdateHighlighted((CharLCD_v1_20.CustomCharacter)userDefinedCharacters[previous]);
        }

        private void HighlightAbove()
        {
            int above = userDefinedCharacters.IndexOf(highlighted) - VERTICAL_CHARACTER_OFFSET;
            // Protects against '-1' or '-2'.  if current is 7 above should be 5 : 
            // 7-2 = 5.  5+8 =13. 13%8 = 5. 
            // If current is '1' above should be 7.  1 -2 = -1.  -1 + 8 % 8 = 7.    
            above = (above + userDefinedCharacters.Count) % userDefinedCharacters.Count;
            // Cast from Object to CustomCharacter
            UpdateHighlighted((CharLCD_v1_20.CustomCharacter)userDefinedCharacters[above]);
        }
        private void HighlightBelow()
        {
            int below = userDefinedCharacters.IndexOf(highlighted) + VERTICAL_CHARACTER_OFFSET;
            // Valid range is from zero to number of characters.
            below = (below + userDefinedCharacters.Count) % userDefinedCharacters.Count;
            // Cast from Object to CustomCharacter
            UpdateHighlighted((CharLCD_v1_20.CustomCharacter)userDefinedCharacters[below]);
        }

        private void UpdateHighlighted(CustomCharacter selectedCharacter)
        {
            highlighted.Match(characterEditor);
            highlighted.Selected = false;
            highlighted.Invalidate();
            highlighted = selectedCharacter;
            highlighted.Selected = true;
            highlighted.Invalidate();
            currentEditableCharacter.Text = highlighted.DisplayName;
            this.characterEditor.Match(selectedCharacter);
            this.characterEditor.Invalidate();
        }
        #endregion

        #region Show and Hide methods for Character Sets
        private void ShowCharacters(System.Collections.ArrayList characters)
        {
            foreach (CustomCharacter character in this.panel1.Controls)
            {
                character.Visible = false;
            }

            foreach (CustomCharacter character in characters)
            {
                character.Visible = true;
            }
        }

        // Greys out the character editor and prevents user interaction with it
        // by making actual editor invisible, and replacing with an empty one.
        private void HideCharacterEditor()
        {
            characterEditor.Visible = false;
            emptyCharacterEditor.Visible = true;
            emptyCharacterEditor.ActiveBrush = new SolidBrush(Color.LightGray);
            emptyCharacterEditor.InactiveBrush = new SolidBrush(Color.WhiteSmoke);
            emptyCharacterEditor.BorderBrush = new SolidBrush(Color.LightGray);
            emptyCharacterEditor.Invalidate();
        }

        // Activates the character editor for user defined characters and hides 
        // the empty place holder editor, "emptyCharacterEditor"
        private void ShowCharacterEditor()
        {
            emptyCharacterEditor.Visible = false;
            characterEditor.Visible = true;
        }

        private void CustomCharactersOff()
        {
            highlighted.Selected = false;
            foreach (CharLCD_v1_20.CustomCharacter character in this.panel1.Controls)
            {
                if (character.Visible == true)
                {
                    character.ActiveBrush = new SolidBrush(Color.LightGray);
                    character.InactiveBrush = new SolidBrush(Color.WhiteSmoke);
                    character.BorderBrush = new SolidBrush(Color.LightGray);
                    character.Invalidate();
                }
            }
            customCharacterGroupBox.Enabled = false;
        }

        private void CustomCharactersOn()
        {
            customCharacterGroupBox.Enabled = true;

            highlighted.Selected = true;
            foreach (CharLCD_v1_20.CustomCharacter character in this.panel1.Controls)
            {
                character.BorderBrush = new SolidBrush(Color.LightGray);
                character.ActiveBrush = new SolidBrush(Color.Black);
                character.InactiveBrush = new SolidBrush(Color.White);
                character.Invalidate();
            }
        }
        #endregion

        #endregion
    
    }


    enum CustomCharacterSetTypes : byte
    {
        NONE, HORIZONTAL, VERTICAL, USERDEFINED
    }
  
    #region Arrow Key Press Group Box Code
    public delegate void ArrowKeyPressEvent(ArrowArgs e);

    public class CustomCharacterGroupBox : System.Windows.Forms.GroupBox
    {
        public event ArrowKeyPressEvent arrowPressedEvent;

        protected override bool ProcessCmdKey(ref Message msg, Keys keyData)
        {
            const int WM_KEYDOWN = 0x100;
            const int WM_SYSKEYDOWN = 0x104;

            if ((msg.Msg == WM_KEYDOWN) || (msg.Msg == WM_SYSKEYDOWN))
            {
                switch (keyData)
                {
                    case Keys.Left:
                        RaiseArrowEvent(ArrowArgs.Direction.LEFT);
                        break;

                    case Keys.Right:
                        RaiseArrowEvent(ArrowArgs.Direction.RIGHT);
                        break;
                    case Keys.Up:
                        RaiseArrowEvent(ArrowArgs.Direction.UP);
                        break;
                    case Keys.Down:
                        RaiseArrowEvent(ArrowArgs.Direction.DOWN);
                        break;
                    default:
                        break;
                }
            }
            return base.ProcessCmdKey(ref msg, keyData);
        }

        private void RaiseArrowEvent(ArrowArgs.Direction keyDirection)
        {
            if (arrowPressedEvent != null)
                arrowPressedEvent(new ArrowArgs(keyDirection));
        }
    }

    public class ArrowArgs : EventArgs
    {
        public enum Direction { LEFT, RIGHT, UP, DOWN };
        private Direction arrowDirection;

        public Direction ArrowDirection
        {
            get { return arrowDirection; }
            set { arrowDirection = value; }
        }

        public ArrowArgs(Direction keyDirection)
        {
            this.arrowDirection = keyDirection;
        }

    }
    #endregion

}
