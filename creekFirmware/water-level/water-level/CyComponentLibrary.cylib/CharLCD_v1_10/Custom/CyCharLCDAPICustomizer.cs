/*******************************************************************************
* Copyright 2008-2009, Cypress Semiconductor Corporation.  All rights reserved.
* You may use this file only in accordance with the license, terms, conditions, 
* disclaimers, and limitations in the end user license agreement accompanying 
* the software package with which this file was provided.
********************************************************************************/



using System;
using System.Collections.Generic;
using System.Text;
using System.Diagnostics;
using CyDesigner.Extensions.Common;
using CyDesigner.Extensions.Gde;
using System.Windows.Forms;
using System.IO;
using System.Collections;
using CharLCD_v1_10;

namespace CharLCD_v1_10
{
    partial class CyCustomizer : ICyAPICustomize_v1
    {
        #region ICyAPICustomize_v1 Members
        static string LCD_CFile_Name = "CharLCD.c";
        static string LCD_HFile_Name = "CharLCD.h";
        static string CustChars_CFile_Name = "CustChars.c";
        static string BarGraph_CFile_Name = "BarGraph.c";
        static string CustCharMacroName = "CUST_CHAR";

        string instanceNameParam = "INSTANCE_NAME";
        static string customCharacterSetParam = "CustomCharacterSet";
        static string conversionRoutineParam = "ConversionRoutines";
        static string udParam = "CUSTOM";

        CyAPICustomizer LCD_CFile;
        CyAPICustomizer LCD_HFile;
        CyAPICustomizer CustChar_CFile;
        CyAPICustomizer BarGraph_CFile;

        CustomCharacterSetTypes customCharacterSet;
        bool conversionRoutines = false;

        public IEnumerable<CyAPICustomizer> CustomizeAPIs(
            ICyInstQuery_v1 query,
            ICyTerminalQuery_v1 termQuery,
            IEnumerable<CyAPICustomizer> apis)
        {
            List<CyAPICustomizer> customizers = new List<CyAPICustomizer>(apis);
            Dictionary<string, string> paramDict = new Dictionary<string, string>();
            string instanceName = "";

            // Get the parameters from the characterLCD.c file customizer
            foreach (CyAPICustomizer api in customizers)
            {
                // Get dict from main file. 
                if (api.OriginalName.EndsWith(LCD_CFile_Name))
                {
                    LCD_CFile = api;
                    paramDict = api.MacroDictionary;
                    paramDict.TryGetValue(instanceNameParam, out instanceName);
                }
                else if (api.OriginalName.EndsWith(LCD_HFile_Name))
                {
                    LCD_HFile = api;
                }
                else if (api.OriginalName.EndsWith(CustChars_CFile_Name))
                {
                    CustChar_CFile = api;
                }
                else if (api.OriginalName.EndsWith(BarGraph_CFile_Name))
                {
                    BarGraph_CFile = api;
                }
            }

            // Determine Custom Character Set
            string value;
            paramDict.TryGetValue(customCharacterSetParam, out value);
            customCharacterSet = (CustomCharacterSetTypes)(int.Parse(value));

            // Determine existence of ASCII Routines
            paramDict.TryGetValue(conversionRoutineParam, out value);
            if (value == "1")
                conversionRoutines = true;
            else
                conversionRoutines = false;

            #region New Code Substitution Values
            string customCharDefinesMacro = "CustomCharDefines_API_GEN";
            string customCharPrototypeMacro = "CustomChar_Prototypes_API_GEN";
            string bargraphPrototypeMacro = "Bargraph_Prototypes_API_GEN";
            string conversionRoutinePrototypeMacro = "Conversion_Routine_Prototypes_API_GEN";
            string conversionRoutineMacro = "ConversionRoutines_API_GEN";

            string customCharDeleted = "";
            string customCharDefined =
                "/* Custom Character References */              " + Environment.NewLine +
                "#define " + instanceName + "_CUSTOM_0     0    " + Environment.NewLine +
                "#define " + instanceName + "_CUSTOM_1     1    " + Environment.NewLine +
                "#define " + instanceName + "_CUSTOM_2     2    " + Environment.NewLine +
                "#define " + instanceName + "_CUSTOM_3     3    " + Environment.NewLine +
                "#define " + instanceName + "_CUSTOM_4     4    " + Environment.NewLine +
                "#define " + instanceName + "_CUSTOM_5     5    " + Environment.NewLine +
                "#define " + instanceName + "_CUSTOM_6     6    " + Environment.NewLine +
                "#define " + instanceName + "_CUSTOM_7     7    " + Environment.NewLine +
                Environment.NewLine +
                "#define " + instanceName + "_BARGRAPH_TYPE    " + ((byte)customCharacterSet).ToString();

            string bargraphPrototypes =
                "/* Bargraph Function Prototypes */                                                                     " + Environment.NewLine +
                "void " + instanceName + "_LoadCustomFonts(const uint8 * customData);                                   " + Environment.NewLine +
                "void " + instanceName + "_DrawHorizontalBG(uint8 row, uint8 column, uint8 maxCharacters, uint8 value); " + Environment.NewLine +
                "void " + instanceName + "_DrawVerticalBG(uint8 row, uint8 column, uint8 maxCharacters, uint8 value);   " + Environment.NewLine;

            string customCharPrototypes = "void " + instanceName + "_LoadCustomFonts(const uint8 * customData);";

            string conversionRoutinePrototypes =
                "/* ASCII Conversion Routines */                        " + Environment.NewLine +
                "void " + instanceName + "_PrintHexUint8(uint8 value);      " + Environment.NewLine +
                "void " + instanceName + "_PrintHexUint16(uint16 value);    " + Environment.NewLine +
                "void " + instanceName + "_PrintDecUint16(uint16 value);   " + Environment.NewLine + 
                "#define " + instanceName + "_PrintNumber(x)   " + instanceName + "_PrintDecUint16(x)" + Environment.NewLine + 
                "#define " + instanceName + "_PrintInt8(x)     " + instanceName + "_PrintHexUint8(x)" + Environment.NewLine +
                "#define " + instanceName + "_PrintInt16(x)    " + instanceName + "_PrintHexUint16(x)" + Environment.NewLine;


            #region Literal String Code for Conversion Routines
            string conversionRoutineDeleted = "";

            string conversionRoutineDefined = @"

/*******************************************************************************
*  Function Name: " + instanceName + @"_PrintHexUint8
********************************************************************************
* Summary:
*  Print a byte as two ASCII characters
*
* Parameters:  
*  value:   The byte to be printed out as ASCII characters.
*
* Return:
*  void
*
*******************************************************************************/
void " + instanceName + @"_PrintHexUint8(uint8 value)
{
    static char8 const hex[16] = ""0123456789ABCDEF"";
    
    " + instanceName + @"_PutChar((char8)hex[value>>4]);
    " + instanceName + @"_PutChar((char8)hex[value&0x0F]);
}


/*******************************************************************************
*  Function Name: " + instanceName + @"_PrintHexUint16
********************************************************************************
* Summary:
*  Print a uint16 as four ASCII characters.
*
* Parameters:  
*  value:   The uint16 to be printed out as ASCII characters.
*
* Return:
*  void
*
*******************************************************************************/
void " + instanceName + @"_PrintHexUint16(uint16 value)
{
    " + instanceName + @"_PrintHexUint8(value >> 8);
    " + instanceName + @"_PrintHexUint8(value & 0xFF);
}


/*******************************************************************************
*  Function Name: " + instanceName + @"_PrintDecUint16
********************************************************************************
* Summary:
*  Print an uint32 value as a left-justified decimal value.
*
* Parameters:  
*  value:   The byte to be printed out as ASCII characters.
*
* Return:
*  void
*
*******************************************************************************/
void " + instanceName + @"_PrintDecUint16(uint16 value)
{
    #define " + instanceName + @"_NUMBER_OF_REMAINDERS 5
    #define " + instanceName + @"_TEN 10

    char8 number[" + instanceName + @"_NUMBER_OF_REMAINDERS];
    char8 temp[" + instanceName + @"_NUMBER_OF_REMAINDERS]; 

    uint8 index = 0;
    uint8 numDigits = 0;

    
    /* Load these in reverse order */ 
    while(value >= " + instanceName + @"_TEN)
    {
        temp[index]= (value % " + instanceName + @"_TEN) + '0';
        value /= " + instanceName + @"_TEN;
        index++;
    }
    
    temp[index] = (value % " + instanceName + @"_TEN) + '0';
    numDigits = index;

    /* While index is greater than or equal to zero */
    while (index != 0xFFu)
    {
        number[numDigits - index] = temp[index];
        index--;
    }
    
    /* Null Termination */
    number[numDigits + 1] = (char8) 0;

    /* Print out number */
    " + instanceName + @"_PrintString(number);
}
";
            #endregion
            #endregion

            // If a character set is selected, build c file with data in it.
            switch (customCharacterSet)
            {
                case CustomCharacterSetTypes.USERDEFINED:
                    // Unpackage Cy Strings into array list from LoadUserDefinedCharcters
                    ConvertCharacters(LoadUserDefinedCharacters(paramDict), paramDict);
                    paramDict.Add(customCharDefinesMacro, customCharDefined);
                    paramDict.Add(customCharPrototypeMacro, customCharPrototypes);
                    customizers.Remove(BarGraph_CFile);
                    break;
                case CustomCharacterSetTypes.VERTICAL:
                    //ConvertCharacters(vbgCharacters, paramDict);
                    GenerateVerticalBargraph(paramDict);
                    paramDict.Add(customCharDefinesMacro, customCharDefined);
                    paramDict.Add(bargraphPrototypeMacro, bargraphPrototypes);
                    customizers.Remove(CustChar_CFile);
                    break;
                case CustomCharacterSetTypes.HORIZONTAL:
                    //ConvertCharacters(hbgCharacters, paramDict);
                    GenerateHorizontalBargraph(paramDict);
                    paramDict.Add(customCharDefinesMacro, customCharDefined);
                    paramDict.Add(bargraphPrototypeMacro, bargraphPrototypes);
                    customizers.Remove(CustChar_CFile);
                    break;
                default:
                    paramDict.Add(customCharDefinesMacro, customCharDeleted);
                    customizers.Remove(BarGraph_CFile);
                    customizers.Remove(CustChar_CFile);
                    break;
            }

            // If conversion routines are selected, import them
            if (conversionRoutines)
            {
                paramDict.Add(conversionRoutinePrototypeMacro, conversionRoutinePrototypes);
                paramDict.Add(conversionRoutineMacro, conversionRoutineDefined);
            }
            else
            {
                paramDict.Add(conversionRoutineMacro, conversionRoutineDeleted);
            }
            // Replace macro dictionaries with paramDict
            foreach (CyAPICustomizer api in customizers)
            {
                api.MacroDictionary = paramDict;
            }

            return customizers;
        }
        #endregion

        #region CustomCharacter Helper Methods
        // Load the User Defined Characters into CharLCDCustomizer ArrayList userDefinedCharacters.
        ArrayList LoadUserDefinedCharacters(Dictionary<string, string> parameters)
        {
            int index = 0;
            string temp = "";
            ArrayList characters = new ArrayList();
            while (parameters.TryGetValue(udParam + index.ToString(), out temp))
            {
                CustomCharacter character = new CustomCharacter();
                temp = temp.Trim('"').TrimEnd(',');
                CyParameterToCustomCharacter(character, temp);
                characters.Add(character);
                index++;
            }
            return characters;
        }

        // Given an ArrayList of Characters, convert array list into a C Data File.
        private void ConvertCharacters(ArrayList customCharacters, Dictionary<string, string> dict)
        {
            int stringIndex = 0;
            string key = "";
            string value = "";



            foreach (CustomCharacter character in customCharacters)
            {
                // Define Value
                value = CharacterToCCode(character);

                // Define Key Name
                key = CustCharMacroName + stringIndex.ToString();

                dict.Add(key, value);

                stringIndex++;
            }
        }

        // Given a CustomCharacter, convert it into the lines of code to build an LCD custom char Array.
        string CharacterToCCode(CustomCharacter customCharacter)
        {
            // "characterString" represents the character after conversion to C Code
            String characterString = "";

            CharLCD_v1_10.Box[,] pixelArray = customCharacter.GetBoxArray();

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
                // Convert to 2 digit hex. Build Code for this row of the character. 
                string temp = rowValue.ToString("X");
                if (temp.Length != 2)
                    characterString += "    0x0" + rowValue.ToString("X");
                else
                    characterString += "    0x" + rowValue.ToString("X");
                // No Comma after the last hex value
                if (row != customCharacter.Rows - 1)
                    characterString += ",";
            }
            return characterString;
        }

        /// <summary>
        /// This method takes a string with each char (byte) representing the locations of active 
        /// on a CyCustomCharacterArray. 
        /// </summary>
        /// <param name="userDefined"> The instance of a character to be updated from CyDesigner data.</param>
        /// <param name="cyParameterString"> The string representing byte array from CyDesigner.</param>
        public void CyParameterToCustomCharacter(CustomCharacter customCharacter, string cyParameterString)
        {
            char[] chars = new char[customCharacter.Rows];
            int index = 0;

            // Remove last comma and seperate into indivudual strings. 
            string[] hexCharacterArray = cyParameterString.TrimEnd(',').Split(',');
            foreach (string rowValue in hexCharacterArray)
            {
                chars[index++] = (char)byte.Parse(rowValue, System.Globalization.NumberStyles.HexNumber);
            }

            CharLCD_v1_10.Box[,] boxes = customCharacter.GetBoxArray();

            for (int row = 0; row < customCharacter.Rows; row++)
            {
                for (int column = 0; column < customCharacter.Columns; column++)
                {
                    if ((((byte)chars[row]) & getExponent(customCharacter.Columns - 1 - column)) != 0)
                        boxes[row, column].IsActive = true;
                }
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
        #region Hard Coded Bargraph Character Values
        void GenerateVerticalBargraph(Dictionary<string, string> dict)
        {
            /* Character LCD_1_CUSTOM_0   */
            dict.Add(CustCharMacroName + "0", "0x00,    0x00,    0x00,    0x00,    0x00,    0x00,    0x00,    0x1F");
            /* Character LCD_1_CUSTOM_1    */
            dict.Add(CustCharMacroName + "1", "0x00,    0x00,    0x00,    0x00,    0x00,    0x00,    0x1F,    0x1F");
            /* Character LCD_1_CUSTOM_2    */
            dict.Add(CustCharMacroName + "2", "0x00,    0x00,    0x00,    0x00,    0x00,    0x1F,    0x1F,    0x1F");
            /* Character LCD_1_CUSTOM_3    */
            dict.Add(CustCharMacroName + "3", "0x00,    0x00,    0x00,    0x00,    0x1F,    0x1F,    0x1F,    0x1F");
            /* Character LCD_1_CUSTOM_4    */
            dict.Add(CustCharMacroName + "4", "0x00,    0x00,    0x00,    0x1F,    0x1F,    0x1F,    0x1F,    0x1F");
            /* Character LCD_1_CUSTOM_5    */
            dict.Add(CustCharMacroName + "5", "0x00,    0x00,    0x1F,    0x1F,    0x1F,    0x1F,    0x1F,    0x1F");
            /* Character LCD_1_CUSTOM_6    */
            dict.Add(CustCharMacroName + "6", "0x00,    0x1F,    0x1F,    0x1F,    0x1F,    0x1F,    0x1F,    0x1F");
            /* Character LCD_1_CUSTOM_7    */
            dict.Add(CustCharMacroName + "7", "0x1F,    0x1F,    0x1F,    0x1F,    0x1F,    0x1F,    0x1F,    0x1F");
        }

        void GenerateHorizontalBargraph(Dictionary<string, string> dict)
        {
            dict.Add(CustCharMacroName + "0", "0x00,    0x00,    0x00,    0x00,    0x00,    0x00,    0x00,    0x00");
            /* Character LCD_1_CUSTOM_1     */
            dict.Add(CustCharMacroName + "1", "0x10,    0x10,    0x10,    0x10,    0x10,    0x10,    0x10,    0x10");
            /* Character LCD_1_CUSTOM_2     */
            dict.Add(CustCharMacroName + "2", "0x18,    0x18,    0x18,    0x18,    0x18,    0x18,    0x18,    0x18");
            /* Character LCD_1_CUSTOM_3     */
            dict.Add(CustCharMacroName + "3", "0x1C,    0x1C,    0x1C,    0x1C,    0x1C,    0x1C,    0x1C,    0x1C");
            /* Character LCD_1_CUSTOM_4     */
            dict.Add(CustCharMacroName + "4", "0x1E,    0x1E,    0x1E,    0x1E,    0x1E,    0x1E,    0x1E,    0x1E");
            /* Character LCD_1_CUSTOM_5     */
            dict.Add(CustCharMacroName + "5", "0x1F,    0x1F,    0x1F,    0x1F,    0x1F,    0x1F,    0x1F,    0x1F");
            /* Character LCD_1_CUSTOM_6     */
            dict.Add(CustCharMacroName + "6", "0x00,    0x00,    0x00,    0x00,    0x00,    0x00,    0x00,    0x00");
            /* Character LCD_1_CUSTOM_7     */
            dict.Add(CustCharMacroName + "7", "0x00,    0x00,    0x00,    0x00,    0x00,    0x00,    0x00,    0x00");
        }
        #endregion
    }
}
