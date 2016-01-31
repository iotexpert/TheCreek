/*******************************************************************************
* Copyright 2008-2012, Cypress Semiconductor Corporation.  All rights reserved.
* You may use this file only in accordance with the license, terms, conditions, 
* disclaimers, and limitations in the end user license agreement accompanying 
* the software package with which this file was provided.
********************************************************************************/

using System.IO;

namespace SegDisplay_v1_10
{
    partial class CyAPIGenerator
    {
        // Lines below that have more then 120 characters are not wrapped for better readability

        #region Write7SegDisp
        void Write7SegDigit_n(TextWriter writer, TextWriter writer_h, int index)
        {
            writer_h.WriteLine("#define {0}_7SEG", m_instanceName);
            writer_h.WriteLine("void    {0}_Write7SegDigit_{1}(uint8 Digit, uint8 Position) `=ReentrantKeil($INSTANCE_NAME . \"_Write7SegDigit_{1}\")`;", m_instanceName, index);

            writer.WriteLine("/*******************************************************************************");
            writer.WriteLine("* Function Name: {0}_Write7SegDigit_{1}", m_instanceName, index);
            writer.WriteLine("********************************************************************************");
            writer.WriteLine("*");
            writer.WriteLine("* Summary:");
            writer.WriteLine("*  This function displays an 4-bit Hex digit in the range of 0-9 and A-F ");
            writer.WriteLine("*  7 segment display. The user must have defined what portion of the displays");
            writer.WriteLine("*  segments make up the 7 segment display portion in the component customizer.");
            writer.WriteLine("*  Multiple, separate 7 segment displays can be created in the frame buffer and");
            writer.WriteLine("*  are addressed through the index (n) in the function name. Function/s only");
            writer.WriteLine("*  included if component 7 segment customizerwizard defines the 7 segment option.");
            writer.WriteLine("*");
            writer.WriteLine("* Parameters:");
            writer.WriteLine("*  digit: unsigned integer value in the range of 0 to 16 to be displayed as");
            writer.WriteLine("*  a hexadecimal digit. The ASCII numbers of a hexadecimal characters are also");
            writer.WriteLine("*  valid. In case of a invalid digit value displays 0 in position specified.");
            writer.WriteLine("*  position: Position of the digit as counted right to left starting at 0 on the");
            writer.WriteLine("*  right. If the defined display does not contain a digit in the position then");
            writer.WriteLine("*  the digit will not be displayed.");
            writer.WriteLine("*");
            writer.WriteLine("* Return: ");
            writer.WriteLine("*  None.");
            writer.WriteLine("*");
            writer.WriteLine("* Global variables:");
            writer.WriteLine("*  {0}_7SegDigits[] - used as a look-up table for 7 Segment helper.", m_instanceName);
            writer.WriteLine("*  Holds decoded digit's pixel reflection for the helper.");
            writer.WriteLine("*");
            writer.WriteLine("*  {0}_disp{1}[][] - holds pixels locations for 7 Segment helper in ", m_instanceName, index);
            writer.WriteLine("*  the Frame Buffer.");
            writer.WriteLine("*");
            writer.WriteLine("*  {0}_digitNum_{1} - holds the maximum digit number for the helper.", m_instanceName, index);
            writer.WriteLine("*");            
            writer.WriteLine("* Reentrant:");
            writer.WriteLine("*  No.");
            writer.WriteLine("*");
            writer.WriteLine("*******************************************************************************/");

            writer.WriteLine("void {0}_Write7SegDigit_{1}(uint8 digit, uint8 position) `=ReentrantKeil($INSTANCE_NAME . \"_Write7SegDigit_{1}\")`", m_instanceName, index);
            writer.WriteLine("{");
            writer.WriteLine("    uint8 i;");
            writer.WriteLine("");
            writer.WriteLine("    /* if digit < 16 then do nothing (we have correct data) */");
            writer.WriteLine("    if(digit <= 16u)");
            writer.WriteLine("    {");
            writer.WriteLine("        /* nothing to do, as we have correct digit value */");
            writer.WriteLine("    }");
            writer.WriteLine("    /* if digit <= 0x39 then digit is ascii code of a number (0-9) */");
            writer.WriteLine("    else if(digit <= 0x39u)");
            writer.WriteLine("    {");
            writer.WriteLine("        digit -= 0x30u;");
            writer.WriteLine("    }");
            writer.WriteLine("    /* if digit <= 0x39 then digit is ascii code of a hex number (A-F) */");
            writer.WriteLine("    else if(digit <= 0x47u)");
            writer.WriteLine("    {");
            writer.WriteLine("        digit -= 0x37u;");
            writer.WriteLine("    }");
            writer.WriteLine("    /* else we have invalid digit, and we will print '8' instead */");
            writer.WriteLine("    else");
            writer.WriteLine("    {");
            writer.WriteLine("        digit = 0u;");
            writer.WriteLine("    }");
            writer.WriteLine("");
            writer.WriteLine("    if((position / {0}_digitNum_{1}) == 0u)", m_instanceName, index);
            writer.WriteLine("    {");
            writer.WriteLine("        position = {0}_digitNum_{1} - position - 1u;", m_instanceName, index);
            writer.WriteLine("        for(i = 0u; i < {0}_7SEG_PIX_NUM; i++)", m_instanceName);
            writer.WriteLine("        {");
            writer.WriteLine("            {0}_WRITE_PIXEL({0}_disp{1}[position][i], \\", m_instanceName,  index);
            writer.WriteLine("            ({0}_7SegDigits[digit] >> i) & {0}_PIXEL_STATE_ON);", m_instanceName); 
            writer.WriteLine("        }"); 
            writer.WriteLine("    }");
            writer.WriteLine("}");
            writer.WriteLine("");
            writer.WriteLine("");
            
        }

        void Write7SegNumber_n(TextWriter writer, TextWriter writer_h, int index)
        {
            writer_h.WriteLine("void    {0}_Write7SegNumber_{1}(uint16 value, uint8 position, uint8 mode) `=ReentrantKeil($INSTANCE_NAME . \"_Write7SegNumber_{1}\")`;", m_instanceName, index);

            writer.WriteLine("/*******************************************************************************");
            writer.WriteLine("* Function Name: {0}_Write7SegNumber_{1}", m_instanceName, index);
            writer.WriteLine("********************************************************************************");
            writer.WriteLine("*");
            writer.WriteLine("* Summary:");
            writer.WriteLine("*  This function displays a 16-bit integer value on a 1 to 5 digit on 7 segment");
            writer.WriteLine("*  display. The user must have defined what portion of the display's segments");
            writer.WriteLine("*  make up the 7 segment display portion in the customizer. Multiple, separate");
            writer.WriteLine("*  7 segment displays can be created in the frame buffer and are addressed");
            writer.WriteLine("*  through the index (n) in the function name. Function/s only included if");
            writer.WriteLine("*  component 7 segment customizer defines the 7 segment option.");
            writer.WriteLine("*");
            writer.WriteLine("* Parameters:  ");
            writer.WriteLine("*  value:    unsigned integer value to be displayed.");
            writer.WriteLine("*  position: Position of the least significant digit as counted right to left");
            writer.WriteLine("*            starting at 0 on the right. If the defined display contains fewer");
            writer.WriteLine("*            digits then the value requires for display for the most significant");
            writer.WriteLine("*            digit/s will not be displayed.");
            writer.WriteLine("*  mode:     0 - no leading zeroes, 1 - leading zeroes are displayed.");
            writer.WriteLine("*");
            writer.WriteLine("* Return: ");
            writer.WriteLine("*  None.");
            writer.WriteLine("*");
            writer.WriteLine("* Global variables:");
            writer.WriteLine("*  {0}_digitNum_{1} - holds the maximum digit number for the helper.", m_instanceName, index);
            writer.WriteLine("*");            
            writer.WriteLine("* Reentrant:");
            writer.WriteLine("*  No.");
            writer.WriteLine("*");
            writer.WriteLine("*******************************************************************************/");
            writer.WriteLine("void {0}_Write7SegNumber_{1}(uint16 value, uint8 position, uint8 mode) `=ReentrantKeil($INSTANCE_NAME . \"_Write7SegNumber_{1}\")`", m_instanceName, index);
            writer.WriteLine("{ ");
            writer.WriteLine("    int8 i;");
            writer.WriteLine("    int8 num;");
            writer.WriteLine("");
            writer.WriteLine("    position = position % {0}_digitNum_{1};", m_instanceName, index);
            writer.WriteLine("");
            writer.WriteLine("    /* While current digit position in the range of display keep processing the output */");
            writer.WriteLine("    for(i = position; i <= {0}_digitNum_{1}; i++)", m_instanceName, index);
            writer.WriteLine("    {");
            writer.WriteLine("        num = value % 10u;");
            writer.WriteLine("        {0}_Write7SegDigit_{1}(num, i);", m_instanceName, index);
            writer.WriteLine("        value /= 10u; ");
            writer.WriteLine("        if((value == 0u) && (mode == 0u)) ");
            writer.WriteLine("        {");
            writer.WriteLine("            break;");
            writer.WriteLine("        }");
            writer.WriteLine("    }");
            writer.WriteLine("}");
            writer.WriteLine("");
            writer.WriteLine("");

        }
        #endregion

        #region PutChar14SegDisp
        void PutChar14seg_n(TextWriter writer, TextWriter writer_h, int index)
        {
            writer_h.WriteLine("#define {0}_14SEG", m_instanceName);
            writer_h.WriteLine("#define ALPHANUMERIC");
            writer_h.WriteLine("void    {0}_PutChar14Seg_{1}(uint8 Character, uint8 Position) `=ReentrantKeil($INSTANCE_NAME . \"_PutChar14Seg_{1}\")`;", m_instanceName, index);

            writer.WriteLine("/*******************************************************************************");
            writer.WriteLine("* Function Name: {0}_PutChar14Seg_{1}", m_instanceName, index);
            writer.WriteLine("********************************************************************************");
            writer.WriteLine("*");
            writer.WriteLine("* Summary:");
            writer.WriteLine("*  This function displays an 8-bit char on an array of alphanumeric character");
            writer.WriteLine("*  displays. The user must have defined what portion of the displays segments");
            writer.WriteLine("*  make up the alphanumeric display portion in the customizer. Multiple, ");
            writer.WriteLine("*  separate alphanumeric displays can be created in the frame buffer and are");
            writer.WriteLine("*  addressed through the index (n) in the function name. Function/s only");
            writer.WriteLine("*  included if component alphanumeric cusomizer defines the alphanumeric option.");
            writer.WriteLine("*");
            writer.WriteLine("* Parameters:");
            writer.WriteLine("*  character: character to be displayed.");
            writer.WriteLine("*  position:  position of the character as counted left to right starting at 0");
            writer.WriteLine("*             on the left. If the position is outside the display range, the");
            writer.WriteLine("*             character will not be displayed.");
            writer.WriteLine("*");
            writer.WriteLine("* Return: ");
            writer.WriteLine("*  None.");
            writer.WriteLine("*");
            writer.WriteLine("* Global variables:");
            writer.WriteLine("*  {0}_digitNum_{1} - holds the maximum digit number for the helper.", m_instanceName, index);
            writer.WriteLine("*");
            writer.WriteLine("*  {0}_14SegChars[] - used as a look-up table for 14 Segment", m_instanceName);
            writer.WriteLine("*  helper. Holds decoded character's pixel reflection for the helper.");
            writer.WriteLine("*");
            writer.WriteLine("*  {0}_digitNum_{1} - holds the maximum digit number for the helper.", m_instanceName, index);
            writer.WriteLine("*");
            writer.WriteLine("* {0}_dispn{1}[][] - holds pixels locations for 14 Segment helper", m_instanceName, index);
            writer.WriteLine("*  in the Frame Buffer.");
            writer.WriteLine("*");            
            writer.WriteLine("* Reentrant:");
            writer.WriteLine("*  No.");
            writer.WriteLine("*");
            writer.WriteLine("*******************************************************************************/");
            writer.WriteLine("void {0}_PutChar14Seg_{1}(uint8 character, uint8 position) `=ReentrantKeil($INSTANCE_NAME . \"_PutChar14Seg_{1}\")`", m_instanceName, index);
            writer.WriteLine("{");
            writer.WriteLine("    uint8 i;");
            writer.WriteLine("");
            writer.WriteLine("    if((position / {0}_digitNum_{1}) == 0u)", m_instanceName, index);
            writer.WriteLine("    {");
            writer.WriteLine("        for(i = 0u; i < {0}_14SEG_PIX_NUM; i++)", m_instanceName);
            writer.WriteLine("        {");
            writer.WriteLine("            {0}_WRITE_PIXEL({0}_disp{1}[position][i], \\", m_instanceName, index);
            writer.WriteLine("            ({0}_14SegChars[character] >> i) & {0}_PIXEL_STATE_ON);", m_instanceName);
            writer.WriteLine("        }");
            writer.WriteLine("    }");
            writer.WriteLine("}");
            writer.WriteLine("");
            writer.WriteLine("");
        }
        void WriteString14seg_n(TextWriter writer, TextWriter writer_h, int index)
        {
            writer_h.WriteLine("void    {0}_WriteString14Seg_{1}(uint8* character, uint8 position) `=ReentrantKeil($INSTANCE_NAME . \"_WriteString14Seg_{1}\")`;", m_instanceName, index);
            
            writer.WriteLine("/*******************************************************************************");
            writer.WriteLine("* Function Name: {0}_WriteString14Seg_{1}", m_instanceName, index);
            writer.WriteLine("********************************************************************************");
            writer.WriteLine("*");
            writer.WriteLine("* Summary:");
            writer.WriteLine("*  This function displays an 8-bit null terminated character string on an array");
            writer.WriteLine("*  of alphanumeric character displays. The user must have defined what portion");
            writer.WriteLine("*  of the displays segments make up the alphanumeric display portion in the");
            writer.WriteLine("*  customizer. Multiple, separate alphanumeric displays can be created in the");
            writer.WriteLine("*  frame buffer and are addressed through the index (n) in the function name.");
            writer.WriteLine("*  Function/s only included if component alphanumeric customizer defines the");
            writer.WriteLine("*  alphanumeric option.");
            writer.WriteLine("*");
            writer.WriteLine("* Parameters:  ");
            writer.WriteLine("*  character: pointer to the null terminated character string.");
            writer.WriteLine("*  position:  the Position of the first character as counted left to right");
            writer.WriteLine("*             starting at 0 on the left. If the defined display contains fewer");
            writer.WriteLine("*             characters then the string requires for display, the extra");
            writer.WriteLine("*             characters will not be displayed.");
            writer.WriteLine("*");
            writer.WriteLine("* Return: ");
            writer.WriteLine("*  None.");
            writer.WriteLine("*");
            writer.WriteLine("* Global variables:");
            writer.WriteLine("*  {0}_digitNum_{1} - holds the maximum digit number for the helper.", m_instanceName, index);
            writer.WriteLine("*");            
            writer.WriteLine("* Reentrant:");
            writer.WriteLine("*  No.");
            writer.WriteLine("*");
            writer.WriteLine("*******************************************************************************/");
            writer.WriteLine("void {0}_WriteString14Seg_{1}(uint8* character, uint8 position) `=ReentrantKeil($INSTANCE_NAME . \"_WriteString14Seg_{1}\")`", m_instanceName, index);
            writer.WriteLine("{");
            writer.WriteLine("");
            writer.WriteLine("    while((*character != 0u) && (position != {0}_digitNum_{1}))", m_instanceName, index);
            writer.WriteLine("    {");
            writer.WriteLine("        {0}_PutChar14Seg_{1}(*character++, position++);", m_instanceName, index);
            writer.WriteLine("    }");
            writer.WriteLine("}");
            writer.WriteLine("");
            writer.WriteLine("");
        }
        #endregion

        #region PutChar16segDisp
        void PutChar16seg_n(TextWriter writer, TextWriter writer_h, int index)
        {
            writer_h.WriteLine("#define     {0}_16SEG", m_instanceName);
            writer_h.WriteLine("#define ALPHANUMERIC");
            writer_h.WriteLine("void    {0}_PutChar16Seg_{1}(uint8 character, uint8 position) `=ReentrantKeil($INSTANCE_NAME . \"_PutChar16Seg_{1}\")`;", m_instanceName, index);

            writer.WriteLine("/*******************************************************************************");
            writer.WriteLine("* Function Name: {0}_PutChar16Seg_{1}", m_instanceName, index);
            writer.WriteLine("********************************************************************************");
            writer.WriteLine("*");
            writer.WriteLine("* Summary:");
            writer.WriteLine("*  This function displays an 8-bit char on an array of alphanumeric character");
            writer.WriteLine("*  displays. The user must have defined what portion of the displays segments");
            writer.WriteLine("*  make up the alphanumeric display portion in the customizer. Multiple, ");
            writer.WriteLine("*  separate alphanumeric displays can be created in the frame buffer and are");
            writer.WriteLine("*  addressed through the index (n) in the function name. Function/s only");
            writer.WriteLine("*  included if component alphanumeric customizer defines the alphanumeric");
            writer.WriteLine("*  option.");
            writer.WriteLine("*");
            writer.WriteLine("* Parameters:  ");
            writer.WriteLine("*  character: character.");
            writer.WriteLine("*  position:  position of the character as counted left to right starting at 0");
            writer.WriteLine("*             on the left. If the position is outside the display range, the");
            writer.WriteLine("*             character will not be displayed.");
            writer.WriteLine("*");
            writer.WriteLine("* Return: ");
            writer.WriteLine("*  None.");
            writer.WriteLine("*");
            writer.WriteLine("* Global variables:");
            writer.WriteLine("*  {0}_digitNum_{1} - holds the maximum digit number for the helper.", m_instanceName, index);
            writer.WriteLine("*");
            writer.WriteLine("*  {0}_16SegChars[] - used as a look-up table for 16 Segment ", m_instanceName);
            writer.WriteLine("*  helper. Holds decoded character's pixel reflection for the helper.");
            writer.WriteLine("*");
            writer.WriteLine("*  {0}_disp{1}[][] - holds pixels locations for 16 Segment", m_instanceName, index);
            writer.WriteLine("*  helper in the Frame Buffer.");
            writer.WriteLine("*");            
            writer.WriteLine("* Reentrant:");
            writer.WriteLine("*  No.");
            writer.WriteLine("*");
            writer.WriteLine("*******************************************************************************/");
            writer.WriteLine("void {0}_PutChar16Seg_{1}(uint8 character, uint8 position) `=ReentrantKeil($INSTANCE_NAME . \"_PutChar16Seg_{1}\")`", m_instanceName, index);
            writer.WriteLine("{");
            writer.WriteLine("    uint8 i;");
            writer.WriteLine("");
            writer.WriteLine("    if((position / {0}_digitNum_{1}) == 0u)", m_instanceName, index);
            writer.WriteLine("    {");
            writer.WriteLine("        for(i = 0u; i < {0}_16SEG_PIX_NUM; i++)", m_instanceName);
            writer.WriteLine("        {");
            writer.WriteLine("            {0}_WRITE_PIXEL({0}_disp{1}[position][i], \\", m_instanceName, index);
            writer.WriteLine("            ({0}_16SegChars[character] >> i) & {0}_PIXEL_STATE_ON);", m_instanceName);
            writer.WriteLine("        }");
            writer.WriteLine("    }");
            writer.WriteLine("}");
            writer.WriteLine("");
            writer.WriteLine("");
        }
        void WriteString16seg_n(TextWriter writer, TextWriter writer_h, int index)
        {
            writer_h.WriteLine("void    {0}_WriteString16Seg_{1}(uint8* character, uint8 position) `=ReentrantKeil($INSTANCE_NAME . \"_WriteString16Seg_{1}\")`;", m_instanceName, index);
            
            writer.WriteLine("/*******************************************************************************");
            writer.WriteLine("* Function Name: {0}_WriteString16Seg_{1}", m_instanceName, index);
            writer.WriteLine("********************************************************************************");
            writer.WriteLine("*");
            writer.WriteLine("* Summary:");
            writer.WriteLine("*  This function displays an 8-bit null terminated character string on an array");
            writer.WriteLine("*  of alphanumeric character displays. The user must have defined what portion");
            writer.WriteLine("*  of the displays segments make up the alphanumeric display portion in the");
            writer.WriteLine("*  customizer. Multiple, separate alphanumeric displays can be created in the");
            writer.WriteLine("*  frame buffer nand are addressed through the index (n) in the  function name");
            writer.WriteLine("*  Function/s only included if component alphanumeric customizer defines the");
            writer.WriteLine("*  alphanumeric option.");
            writer.WriteLine("*");
            writer.WriteLine("* Parameters:  ");
            writer.WriteLine("*  character: pointer to the null terminated character string.");
            writer.WriteLine("*  position:  the Position of the first character as counted left to right");
            writer.WriteLine("*             starting at 0 on the left. If the defined display contains fewer");
            writer.WriteLine("*             characters then the string requires for display, the extra");
            writer.WriteLine("*             characters will not be displayed.");
            writer.WriteLine("*");
            writer.WriteLine("* Return: ");
            writer.WriteLine("*  None.");
            writer.WriteLine("*");
            writer.WriteLine("* Global variables:");
            writer.WriteLine("*  {0}_digitNum_{1} - holds the maximum digit number for the helper.", m_instanceName, index);
            writer.WriteLine("*");  
            writer.WriteLine("* Reentrant:");
            writer.WriteLine("*  No.");
            writer.WriteLine("*");
            writer.WriteLine("*******************************************************************************/");
            writer.WriteLine("void {0}_WriteString16Seg_{1}(uint8* character, uint8 position) `=ReentrantKeil($INSTANCE_NAME . \"_WriteString16Seg_{1}\")`", m_instanceName, index);
            writer.WriteLine("{");
            writer.WriteLine("");
            writer.WriteLine("    while((*character != 0u) && (position != {0}_digitNum_{1}))", m_instanceName, index);
            writer.WriteLine("    {");
            writer.WriteLine("        {0}_PutChar16Seg_{1}(*character++, position++);", m_instanceName, index);
            writer.WriteLine("    }");
            writer.WriteLine("}");
            writer.WriteLine("");
            writer.WriteLine("");

        }
        #endregion

        #region WriteStringDotMatrix_n
        void PutCharDotMatrix_n(TextWriter writer, TextWriter writer_h, int index)
        {
            writer_h.WriteLine("#define     {0}_DOT_MATRIX", m_instanceName);
            writer_h.WriteLine("#define ALPHANUMERIC");
            writer_h.WriteLine("void    {0}_PutCharDotMatrix_{1}(uint8 character, uint8 position) `=ReentrantKeil($INSTANCE_NAME . \"_PutCharDotMatrix_{1}\")`;", m_instanceName, index);

            writer.WriteLine("/*******************************************************************************");
            writer.WriteLine("* Function Name: {0}_PutCharDotMatrix_{1}", m_instanceName, index);
            writer.WriteLine("********************************************************************************");
            writer.WriteLine("*");
            writer.WriteLine("* Summary:");
            writer.WriteLine("*  This function displays an 8-bit char on an array of alphanumeric character");
            writer.WriteLine("*  displays. The user must have defined what portion of the displays segment");
            writer.WriteLine("*  make up the alphanumeric display portion in the customizer. Multiple, ");
            writer.WriteLine("*  separate alphanumeric displays can be created in the frame buffer and are");
            writer.WriteLine("*  addressed through the index (n) in the function name. Function/s only");
            writer.WriteLine("*  included if component alphanumeric customizer defines the alphanumeric");
            writer.WriteLine("*  option.");
            writer.WriteLine("*");
            writer.WriteLine("* Parameters:");
            writer.WriteLine("*  character: character.");
            writer.WriteLine("*  position:  the Position of the character as counted left to right starting");
            writer.WriteLine("*             at 0 on the left. If the position is outside the display range,");
            writer.WriteLine("*             the character will not be displayed.");
            writer.WriteLine("*");
            writer.WriteLine("* Return: ");
            writer.WriteLine("*  None.");
            writer.WriteLine("*");
            writer.WriteLine("* Global variables:");
            writer.WriteLine("*  {0}_digitNum_{1} - holds the maximum digit number for the helper.", m_instanceName, index);
            writer.WriteLine("*");
            writer.WriteLine("*  {0}_charDotMatrix[][] - used as a look-up table for Dot Matrix", m_instanceName);
            writer.WriteLine("*  helper. Holds decoded character's pixel reflection for the helper.");
            writer.WriteLine("*");
            writer.WriteLine("*  {0}_disp{1}[][] - holds pixels locations for 16 Segment helper ", m_instanceName, index);
            writer.WriteLine("*  in the Frame Buffer.");
            writer.WriteLine("*");
            writer.WriteLine("* Reentrant:");
            writer.WriteLine("*  No.");
            writer.WriteLine("*");
            writer.WriteLine("*******************************************************************************/");
            writer.WriteLine("void {0}_PutCharDotMatrix_{1}(uint8 character, uint8 position)  `=ReentrantKeil($INSTANCE_NAME . \"_PutCharDotMatrix_{1}\")`", m_instanceName, index);
            writer.WriteLine("{");
            writer.WriteLine("    uint8 i;");
            writer.WriteLine("    uint8 j;");
            writer.WriteLine("");
            writer.WriteLine("    if((position / {0}_digitNum_{1}) == 0u)", m_instanceName, index);
            writer.WriteLine("    {");
            writer.WriteLine("        for(j = 0u; j < {0}_DM_CHAR_WIDTH; j++)", m_instanceName);
            writer.WriteLine("        {");
            writer.WriteLine("            for(i = 0u; i < {0}_DM_CHAR_HEIGHT; i++)", m_instanceName);
            writer.WriteLine("            {");
            writer.WriteLine("                {0}_WRITE_PIXEL({0}_disp{1}[position][j + i * {0}_DM_CHAR_WIDTH], \\", m_instanceName, index);
            writer.WriteLine("                (({0}_charDotMatrix[character][j] >> i) & {0}_PIXEL_STATE_ON));", m_instanceName);
            writer.WriteLine("            }");
            writer.WriteLine("        }");
            writer.WriteLine("    }");
            writer.WriteLine("}");
            writer.WriteLine("");
            writer.WriteLine("");
        }
        void WriteStringDotMatrix_n(TextWriter writer, TextWriter writer_h, int index)
        {
            writer_h.WriteLine("void    {0}_WriteStringDotMatrix_{1}(uint8* character, uint8 position)  `=ReentrantKeil($INSTANCE_NAME . \"_WriteStringDotMatrix_{1}\")`;", m_instanceName, index);
            
            writer.WriteLine("/*******************************************************************************");
            writer.WriteLine("* Function Name: {0}_WriteStringDotMatrix_{1}", m_instanceName, index);
            writer.WriteLine("********************************************************************************");
            writer.WriteLine("*");
            writer.WriteLine("* Summary:");
            writer.WriteLine("*  This function displays an 8-bit null terminated character string on an array");
            writer.WriteLine("*  of alphanumeric character displays. The user must have defined what portion");
            writer.WriteLine("*  of the displays segments make up the alphanumeric display portion in the");
            writer.WriteLine("*  customizer. Multiple, separate alphanumeric displays can be created in the");
            writer.WriteLine("*  the frame buffer and are addressed through the index (n) in the function");
            writer.WriteLine("*  function name. Function/s only included if component alphanumeric customizer");
            writer.WriteLine("*  defines the alphanumeric option.");
            writer.WriteLine("*");
            writer.WriteLine("* Parameters:");
            writer.WriteLine("*  character: pointer to the null terminated character string.");
            writer.WriteLine("*  position:  the Position of the first character as counted left to right");
            writer.WriteLine("*             starting at 0 on the left. If the defined display contains fewer");
            writer.WriteLine("*             characters then the string requires for display, the extra");
            writer.WriteLine("*             characters will not be displayed.");
            writer.WriteLine("*");
            writer.WriteLine("* Return: ");
            writer.WriteLine("*  None.");
            writer.WriteLine("*");
            writer.WriteLine("* Global variables:");
            writer.WriteLine("*  {0}_digitNum_{1} - holds the maximum digit number for the helper.", m_instanceName, index);
            writer.WriteLine("*");
            writer.WriteLine("* Reentrant:");
            writer.WriteLine("*  No.");
            writer.WriteLine("*");
            writer.WriteLine("*******************************************************************************/");
            writer.WriteLine("void {0}_WriteStringDotMatrix_{1}(uint8* character, uint8 position) `=ReentrantKeil($INSTANCE_NAME . \"_WriteStringDotMatrix_{1}\")`", m_instanceName, index);
            writer.WriteLine("{");
            writer.WriteLine("");
            writer.WriteLine("    while((character != 0u) && (position  != {0}_digitNum_{1}))", m_instanceName, index);
            writer.WriteLine("    {");
            writer.WriteLine("        {0}_PutCharDotMatrix_{1}(*character++, position++);", m_instanceName, index);
            writer.WriteLine("    }");
            writer.WriteLine("}");
            writer.WriteLine("");
            writer.WriteLine("");
        }

        #endregion

        #region WriteBargraph_n
        void WriteBargraph_n(TextWriter writer, TextWriter writer_h, int index, int maxNumber)
        {
            writer_h.WriteLine("void    {0}_WriteBargraph_{1}(uint16 location, int8 mode) `=ReentrantKeil($INSTANCE_NAME . \"_WriteBargraph_{1}\")`;", m_instanceName, index);
            
            writer.WriteLine("/*******************************************************************************");
            writer.WriteLine("* Function Name: {0}_WriteBargraph_{1}", m_instanceName, index);
            writer.WriteLine("********************************************************************************");
            writer.WriteLine("*");
            writer.WriteLine("* Summary:");
            writer.WriteLine("*  This function displays an 8-bit integer location on a 1 to 255 segment");
            writer.WriteLine("*  bar-graph (numbered left to right). The bar graph may be any user defined");
            writer.WriteLine("*  size between 1 and 255 The bar graph may be any user defined created in a");
            writer.WriteLine("*  circle to display rotary position. The user defines what portion of the");
            writer.WriteLine("*  displays segments make up the bar-graph portion. Multiple, separate bargraph");
            writer.WriteLine("*  displays can be created in the frame buffer and are addressed through the");
            writer.WriteLine("*  index (n) in the function name. Function/s only included if component");
            writer.WriteLine("*  bar-graph customizer defines the 7 segment option");
            writer.WriteLine("*");
            writer.WriteLine("* Parameters:");
            writer.WriteLine("*  location: unsigned integer Location to be displayed. 0 - all bar-graph");
            writer.WriteLine("*            elements off. Max Value = the number of segments in the bar-graph.");
            writer.WriteLine("*            Locations greater then the number of segments in the bar-graph will");
            writer.WriteLine("*            be limited to the number of segments physically provided.");
            writer.WriteLine("*  mode:     0 - only the Location segment is turned on, 1 - The Location ");
            writer.WriteLine("*            segment and all segments to the left are turned on, -1 - the");
            writer.WriteLine("*            location segment and all segments to the right are turned on. 2-10");
            writer.WriteLine("*            display the location segment and 2-10 segments to the right to");
            writer.WriteLine("*            create wide indicators.");
            writer.WriteLine("*");
            writer.WriteLine("* Return: ");
            writer.WriteLine("*  None.");
            writer.WriteLine("*");
            writer.WriteLine("* Global variables:");
            writer.WriteLine("*  {0}_digitNum_{1} - holds the maximum digit number for the helper.", m_instanceName, index);
            writer.WriteLine("*");
            writer.WriteLine("*  {0}_disp{1}[][] - stores pixels locations for BarGraph helper ", m_instanceName, index);
            writer.WriteLine("*  in the Frame Buffer.");
            writer.WriteLine("*");
            writer.WriteLine("* Reentrant:");
            writer.WriteLine("*  No.");
            writer.WriteLine("*");
            writer.WriteLine("*******************************************************************************/");
            writer.WriteLine("void {0}_WriteBargraph_{1}(uint16 location, int8 mode) `=ReentrantKeil($INSTANCE_NAME . \"_WriteBargraph_{1}\")`", m_instanceName, index);
            writer.WriteLine("{");
            writer.WriteLine("    uint8 i;");
            writer.WriteLine("    uint16 maxValue = {0}u;", maxNumber);
            writer.WriteLine("    uint16 locationInt = location;");
            writer.WriteLine("    int8 modeInt = mode;");
            writer.WriteLine("");
			writer.WriteLine("    if (locationInt != 0u)");
			writer.WriteLine("    {");
            writer.WriteLine("        /* If location greater then the number of elements in bar graph then");
            writer.WriteLine("        set location to a maxvalue and set mode to 0.");
            writer.WriteLine("        */");
            writer.WriteLine("        if(locationInt > maxValue)");
            writer.WriteLine("        {");
            writer.WriteLine("            locationInt = 1u;");
            writer.WriteLine("            modeInt = -1;");
            writer.WriteLine("        }");
            writer.WriteLine("        ");
            writer.WriteLine("        switch(modeInt)");
            writer.WriteLine("        {");
            writer.WriteLine("            case 0:");
            writer.WriteLine("                {0}_WRITE_PIXEL({0}_disp{1}[locationInt][0u], {0}_PIXEL_STATE_ON);", m_instanceName, index);
            writer.WriteLine("                break;");
            writer.WriteLine("            case 1:");
            writer.WriteLine("                for(i = locationInt; i >= 1u; i--) ");
            writer.WriteLine("                {");
            writer.WriteLine("                    {0}_WRITE_PIXEL({0}_disp{1}[i][0u], {0}_PIXEL_STATE_ON);", m_instanceName, index);
            writer.WriteLine("                }");
            writer.WriteLine("                break;");            
            writer.WriteLine("            case -1:");
            writer.WriteLine("                for(i = locationInt; i <= maxValue; i++) ");
            writer.WriteLine("                {");
            writer.WriteLine("                    {0}_WRITE_PIXEL({0}_disp{1}[i][0u], {0}_PIXEL_STATE_ON);", m_instanceName, index);
            writer.WriteLine("                }");
            writer.WriteLine("                break;");
            writer.WriteLine("            case 2:");
            writer.WriteLine("            case 3:");
            writer.WriteLine("            case 4:");
            writer.WriteLine("            case 5:");
            writer.WriteLine("            case 6:");
            writer.WriteLine("            case 7:");
            writer.WriteLine("            case 8:");
            writer.WriteLine("            case 9:");
            writer.WriteLine("            case 10:");
            writer.WriteLine("                if((locationInt + modeInt - 1u) <= maxValue) ");
            writer.WriteLine("                {");
            writer.WriteLine("                    maxValue = locationInt + modeInt -1u;");
            writer.WriteLine("                }");
            writer.WriteLine("                for(i = locationInt; i <= maxValue; i++) ");
            writer.WriteLine("                {");
            writer.WriteLine("                    {0}_WRITE_PIXEL({0}_disp{1}[i][0u], {0}_PIXEL_STATE_ON);", m_instanceName, index);
            writer.WriteLine("                }");
            writer.WriteLine("                break;");
            writer.WriteLine("            default:");
            writer.WriteLine("                break;");
            writer.WriteLine("        }");
			writer.WriteLine("    }");
            writer.WriteLine("    else ");
            writer.WriteLine("    {");
            writer.WriteLine("        for (i = 1u; i <= maxValue; i++) ");
            writer.WriteLine("        {");
            writer.WriteLine("            {0}_WRITE_PIXEL({0}_disp{1}[i][0u], {0}_PIXEL_STATE_OFF);", m_instanceName, index);
            writer.WriteLine("        }");
            writer.WriteLine("    }");
            writer.WriteLine("}");
            writer.WriteLine("");
            writer.WriteLine("");
        }

        #endregion

    }
}
