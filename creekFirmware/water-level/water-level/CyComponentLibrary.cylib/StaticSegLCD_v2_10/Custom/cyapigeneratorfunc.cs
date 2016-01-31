/*******************************************************************************
* Copyright 2008-2012, Cypress Semiconductor Corporation.  All rights reserved.
* You may use this file only in accordance with the license, terms, conditions, 
* disclaimers, and limitations in the end user license agreement accompanying 
* the software package with which this file was provided.
********************************************************************************/

using System.IO;

namespace StaticSegLCD_v2_10
{
    partial class CyAPIGenerator
    {
        // Lines below that have more then 120 characters are not wrapped for better readability

        #region Write7SegDisp
        void Write7SegDigit_n(TextWriter writer, TextWriter writer_h, int index)
        {
            writer_h.WriteLine("#define {0}_7SEG", m_instanceName);
            writer_h.WriteLine("void {0}_Write7SegDigit_{1}(uint8 digit, uint8 position) `=ReentrantKeil($INSTANCE_NAME . \"_Write7SegDigit_{1}\")`;", m_instanceName, index);

            writer.WriteLine("/*******************************************************************************");
            writer.WriteLine("* Function Name: {0}_Write7SegDigit_{1}", m_instanceName, index);
            writer.WriteLine("********************************************************************************");
            writer.WriteLine("* ");
            writer.WriteLine("* Summary:");
            writer.WriteLine("*  This function displays an 4-bit Hex digit in the range of 0-9 and A-F on 7 ");
            writer.WriteLine("*  segment display. The user must have defined what portion of the displays");
            writer.WriteLine("*  segments make up the 7 segment display portion in the component customizer.");
            writer.WriteLine("*  Multiple, separate 7 segment displays can be created in the frame buffer");
            writer.WriteLine("*  and are addressed through the index (n) in the function name. Function/s only");
            writer.WriteLine("*  included if component 7 segment customizer defines the 7 segment option.");
            writer.WriteLine("*");
            writer.WriteLine("* Parameters:  ");
            writer.WriteLine("*  digit : unsigned integer value in the range of 0 to 16 to be displayed as a");
            writer.WriteLine("*  hexadecimal digit. The ASCII numbers of a hexadecimal characters are also");
            writer.WriteLine("*  valid. In case of a invalid digit value displays 0 in position specified.");
            writer.WriteLine("*  position : Position of the digit as counted right to left starting at 0 on the");
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
            writer.WriteLine("    uint8 val;");
            writer.WriteLine("");
            writer.WriteLine("    /* Check if digit is valid */");
            writer.WriteLine("    if(digit <= 16u)");
            writer.WriteLine("    {");
            writer.WriteLine("        /* Do nothing: digit = digit */ ;");
            writer.WriteLine("    }");
            writer.WriteLine("    else if(digit <= 0x39u)");
            writer.WriteLine("    {");
            writer.WriteLine("        digit -= 0x30u;");
            writer.WriteLine("    }");
            writer.WriteLine("    else if(digit <= 0x47u)");
            writer.WriteLine("    {");
            writer.WriteLine("        digit -= 0x37u;");
            writer.WriteLine("    }");
            writer.WriteLine("    else");
            writer.WriteLine("    {");
            writer.WriteLine("        digit = 0u;");
            writer.WriteLine("    }");
            writer.WriteLine("");
            writer.WriteLine("    /* Check if position<{0}_digitNum_{1}, if not then do 'mod' operation */", m_instanceName, index);
			writer.WriteLine("    position = position % {0}_digitNum_{1};", m_instanceName, index);
            writer.WriteLine("");
            writer.WriteLine("    /* Find position counted right to left */");
			writer.WriteLine("    position = {0}_digitNum_{1} - position - 1u;", m_instanceName, index);
            writer.WriteLine("    for(i = 0u; i < {0}_7SEG_PIX_NUM; i++)", m_instanceName);
            writer.WriteLine("    {");
            writer.WriteLine("        /* Select pixel statefor the digit from the look-up table */");
            writer.WriteLine("        val = (({0}_7SegDigits[digit] >> i) & {1}_PIXEL_STATE_ON); ", m_instanceName, m_instanceName);
            writer.WriteLine("        {0}_WRITE_PIXEL({1}_disp{2}[position][i], val);", m_instanceName, m_instanceName, index);
            writer.WriteLine("    }");
            writer.WriteLine("}");
            writer.WriteLine("");
            writer.WriteLine("");
        }

        void Write7SegNumber_n(TextWriter writer, TextWriter writer_h, int index)
        {
            writer_h.WriteLine("void {0}_Write7SegNumber_{1}(uint16 value, uint8 position, uint8 mode) `=ReentrantKeil($INSTANCE_NAME . \"_Write7SegNumber_{1}\")`;", m_instanceName, index);
            writer.WriteLine("/*******************************************************************************");
            writer.WriteLine("* Function Name: {0}_Write7SegNumber_{1}", m_instanceName, index);
            writer.WriteLine("********************************************************************************");
            writer.WriteLine("*");            
            writer.WriteLine("* Summary:");
            writer.WriteLine("*  This function displays a 16-bit integer value on a 1 to 5 digit 7 segment");
            writer.WriteLine("*  display. The user must have defined what portion of the displays segments");
            writer.WriteLine("*  make up the 7 segment display portion in the customizer. Multiple, separate 7");
            writer.WriteLine("*  segment displays can be created in the frame buffer and are addressed through");
            writer.WriteLine("*  the index (n) in the function name. Function/s only included if component 7");
            writer.WriteLine("*  segment wizard defines the 7 segment option.");
            writer.WriteLine("*");
            writer.WriteLine("* Parameters:  ");
            writer.WriteLine("*  value : unsigned integer value to be displayed.");
            writer.WriteLine("*  position : Position of the least significant digit as counted right to left");
            writer.WriteLine("*  starting at 0 on the right. If the defined display contains fewer digits");
            writer.WriteLine("*  then the value requires for display for the most significant digit/s will");
            writer.WriteLine("*  not be displayed.");
            writer.WriteLine("*  mode : 0 = no leading 0s are displayed, 1 = leading 0s are displayed.");
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
            writer.WriteLine("    for(i = position; i < {0}_digitNum_{1}; i++)", m_instanceName, index);
            writer.WriteLine("    {");
            writer.WriteLine("        num = value % 10u;");
            writer.WriteLine("        {0}_Write7SegDigit_{1}(num, i);", m_instanceName, index);
            writer.WriteLine("        value /= 10u; ");
            writer.WriteLine("        if((value == 0u) && (mode == 0u))");
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
            writer_h.WriteLine("void {0}_PutChar14Seg_{1}(uint8 character, uint8 position) `=ReentrantKeil($INSTANCE_NAME . \"_PutChar14Seg_{1}\")`;", m_instanceName, index);

            writer.WriteLine("/*******************************************************************************");
            writer.WriteLine("* Function Name: {0}_PutChar14Seg_{1}", m_instanceName, index);
            writer.WriteLine("********************************************************************************");
            writer.WriteLine("*");
            writer.WriteLine("* Summary:");
            writer.WriteLine("*  This function displays an 8-bit char on an array of alphanumeric character");
            writer.WriteLine("*  displays. The user must have defined what portion of the displays segments");
            writer.WriteLine("*  make up the alphanumeric display portion in the customizer. Multiple,");
            writer.WriteLine("*  separate alphanumeric displays can be created in the frame buffer and are");
            writer.WriteLine("*  addressed through the index (n) in the function name. Function/s only");
            writer.WriteLine("*  included if component alphanumeric wizard defines the alphanumeric option.");
            writer.WriteLine("*");
            writer.WriteLine("* Parameters:  ");
            writer.WriteLine("*  character : Character.");
            writer.WriteLine("*  position : Position of the character as counted left to right starting at 0");
            writer.WriteLine("*  on the left. If the position is outside the display range, the character will");
            writer.WriteLine("*  not be displayed.");
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
            writer.WriteLine("            /* Select pixel state for the character from the look-up table */");
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
            writer_h.WriteLine("void {0}_WriteString14Seg_{1}(uint8* character, uint8 position) `=ReentrantKeil($INSTANCE_NAME . \"_WriteString14Seg_{1}\")`;", m_instanceName, index);
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
            writer.WriteLine("*  Function/s only included if component alphanumeric wizard defines the");
            writer.WriteLine("*  alphanumeric option.");
            writer.WriteLine("*");
            writer.WriteLine("* Parameters:  ");
            writer.WriteLine("*  character : Pointer to the null terminated character string.");
            writer.WriteLine("*  position : The Position of the first character as counted left to right");
            writer.WriteLine("*  starting at 0 on the left. If the defined display contains fewer characters");
            writer.WriteLine("*  then the string requires for display, the extra characters will not be");
            writer.WriteLine("*  displayed.");
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
            writer.WriteLine("    while((*character != 0) && (position != {0}_digitNum_{1}))", m_instanceName, index);
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
            writer_h.WriteLine("#define {0}_16SEG", m_instanceName);
            writer_h.WriteLine("#define ALPHANUMERIC");
            writer_h.WriteLine("void {0}_PutChar16Seg_{1}(uint8 character, uint8 position) `=ReentrantKeil($INSTANCE_NAME . \"_PutChar16Seg_{1}\")`;", m_instanceName, index);

            writer.WriteLine("/*******************************************************************************");
            writer.WriteLine("* Function Name: {0}_PutChar16Seg_{1}", m_instanceName, index);
            writer.WriteLine("********************************************************************************");
            writer.WriteLine("*");
            writer.WriteLine("* Summary:");
            writer.WriteLine("*  This function displays an 8-bit char on an array of alphanumeric character");
            writer.WriteLine("*  displays. The user must have defined what portion of the displays segments");
            writer.WriteLine("*  make up the alphanumeric display portion in the wizard. Multiple, separate");
            writer.WriteLine("*  alphanumeric displays can be created in the frame buffer and are addressed");
            writer.WriteLine("*  through the index (n) in the function name. Function/s  only included if");
            writer.WriteLine("*  component alphanumeric wizard defines the alphanumeric option.");
            writer.WriteLine("*");
            writer.WriteLine("* Parameters:  ");
            writer.WriteLine("*  character : Character.");
            writer.WriteLine("*  position : Position of the character as counted left to right starting at 0");
            writer.WriteLine("*  on the left. If the position is outside the display range, the character will");
            writer.WriteLine("*  will not be displayed.");
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
        }
        void WriteString16seg_n(TextWriter writer, TextWriter writer_h, int index)
        {
            writer_h.WriteLine("void {0}_WriteString16Seg_{1}(uint8* character, uint8 position) `=ReentrantKeil($INSTANCE_NAME . \"_WriteString16Seg_{1}\")`;", m_instanceName, index);

            writer.WriteLine("/*******************************************************************************");
            writer.WriteLine("* Function Name: {0}_WriteString16Seg_{1}", m_instanceName, index);
            writer.WriteLine("********************************************************************************");
            writer.WriteLine("*");
            writer.WriteLine("* Summary:");
            writer.WriteLine("*  This function displays an 8-bit null terminated character string on an array");
            writer.WriteLine("*  of alphanumeric character displays. The user must have defined what portion");
            writer.WriteLine("*  of the displays segments make up the alphanumeric display portion in the");
            writer.WriteLine("*  customizer. Multiple, separate alphanumeric displays can be created in the");
            writer.WriteLine("*  frame buffer nand are addressed through the index (n) in the function name.");
            writer.WriteLine("*  Function/s only included if component alphanumeric wizard defines the ");
            writer.WriteLine("*  alphanumeric option.");
            writer.WriteLine("*");
            writer.WriteLine("* Parameters:  ");
            writer.WriteLine("*  character : Pointer to the null terminated character string.");
            writer.WriteLine("*  position : The Position of the first character as counted left to right");
            writer.WriteLine("*  starting at 0 on the left. If the defined display contains fewer characters");
            writer.WriteLine("*  then the string requires for display, the extra characters will not be");
            writer.WriteLine("*  displayed.");
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
            writer.WriteLine("    while((character != 0) && (position != {0}_digitNum_{1}))", m_instanceName, index);
            writer.WriteLine("    {");
            writer.WriteLine("        {0}_PutChar16Seg_{1}(*character++, position++);", m_instanceName, index);
            writer.WriteLine("    }");
            writer.WriteLine("}");
            writer.WriteLine("");
            writer.WriteLine("");
            
        }
        #endregion

        #region WriteBargraph_n
        void WriteBargraph_n(TextWriter writer, TextWriter writer_h, int index, int maxNumber)
        {
            writer_h.WriteLine("void {0}_WriteBargraph_{1}(uint16 location, int8 mode) `=ReentrantKeil($INSTANCE_NAME . \"_WriteBargraph_{1}\")`;", m_instanceName, index);
            writer.WriteLine("");
            writer.WriteLine("");
            writer.WriteLine("/*******************************************************************************");
            writer.WriteLine("* Function Name: {0}_WriteBargraph_{1}", m_instanceName, index);
            writer.WriteLine("********************************************************************************");
            writer.WriteLine("*");
            writer.WriteLine("* Summary:");
            writer.WriteLine("*  This function displays an 8-bit integer Location on a 1 to 255 segment");
            writer.WriteLine("*  bar-graph (numbered left to right). The bar graph may be any user defined");
            writer.WriteLine("*  size between 1 and 255 segments. A bar graph may also be created in a circle");
            writer.WriteLine("*  to display rotary position. The user defines what portion of the displays");
            writer.WriteLine("*  segments make up the bar-graph portion. Multiple, separate bargraph displays");
            writer.WriteLine("*  can be created in the frame buffer and are addressed through the index (n) in");
            writer.WriteLine("*  the function name. Function/s only included if component bar-graph customizer");
            writer.WriteLine("*  defines the 7 segment option.");
            writer.WriteLine("*");
            writer.WriteLine("* Parameters:  ");
            writer.WriteLine("*  location : unsigned integer location to be displayed. 0 - all bar-graph");
            writer.WriteLine("*  elements off. Max Value = the number of segments in the bar-graph. Locations");
            writer.WriteLine("*  greater then the number of segments in the bar-graph will be limited to the");
            writer.WriteLine("*  number of segments physically provided.");
            writer.WriteLine("*  mode : 0 - only the location segment is turned on, 1 = The location segment");
            writer.WriteLine("*  all segments to the left are turned on, - 1 - the location segment and all");
            writer.WriteLine("*  segments to the right are turned on. 2-10 display the location segment and");
            writer.WriteLine("*  2-254 segments to the right to create wide indicators.");
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
