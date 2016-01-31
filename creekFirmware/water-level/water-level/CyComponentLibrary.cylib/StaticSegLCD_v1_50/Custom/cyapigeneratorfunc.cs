/*******************************************************************************
* Copyright 2008-2010, Cypress Semiconductor Corporation.  All rights reserved.
* You may use this file only in accordance with the license, terms, conditions, 
* disclaimers, and limitations in the end user license agreement accompanying 
* the software package with which this file was provided.
********************************************************************************/

using System.IO;

namespace StaticSegLCD_v1_50
{
    partial class CyAPIGenerator
    {
        // Lines below that have more then 120 characters are not wrapped for better readability

        #region Write7SegDisp
        void Write7SegDigit_n(TextWriter writer, TextWriter writer_h, int index)
        {
            writer_h.WriteLine("#define {0}_7SEG", m_instanceName);
            writer_h.WriteLine("void {0}_Write7SegDigit_{1}(uint8 digit, uint8 position);", m_instanceName, index);

            writer.WriteLine("/*******************************************************************************");
            writer.WriteLine("* Function Name: {0}_Write7SegDigit_{1}", m_instanceName, index);
            writer.WriteLine("********************************************************************************");
            writer.WriteLine("* ");
            writer.WriteLine("* Summary:");
            writer.WriteLine("*  This function displays an 4-bit Hex digit in the range of");
            writer.WriteLine("*  0-9 and A-F 7 segment display. The user must have defined what");
            writer.WriteLine("*  portion of the displays segments make up the 7 segment display");
            writer.WriteLine("*  portion in the component wizard. Multiple, separate 7 segment");
            writer.WriteLine("*  displays can be created in the frame buffer and are addressed");
            writer.WriteLine("*  through the index (n) in the function name. Function/s only included");
            writer.WriteLine("*  if component 7 segment wizard defines the 7 segment option.");
            writer.WriteLine("*");
            writer.WriteLine("* Parameters:  ");
            writer.WriteLine("*  digit : unsigned integer value in the range of 0 to 15 to");
            writer.WriteLine("*  be displayed as a hexadecimal digit. ");
            writer.WriteLine("*  position : Position of the digit as counted right to left");
            writer.WriteLine("*  starting at 0 on the right. If the defined display does not");
            writer.WriteLine("*  contain a digit in the position then the digit will not be");
            writer.WriteLine("*  displayed");
            writer.WriteLine("*");
            writer.WriteLine("* Return: ");
            writer.WriteLine("*  None.");
            writer.WriteLine("*");
            writer.WriteLine("* Reentrant:");
            writer.WriteLine("*  No.");
            writer.WriteLine("*");
            writer.WriteLine("*******************************************************************************/");

            writer.WriteLine("void {0}_Write7SegDigit_{1}(uint8 digit, uint8 position)", m_instanceName, index);
            writer.WriteLine("{");
            writer.WriteLine("    uint8 i, val;");
            writer.WriteLine("");
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
            writer.WriteLine("        digit = 8u;");
            writer.WriteLine("    }");
            writer.WriteLine("");
			writer.WriteLine("    position = position % {0}_digitNum_{1};", m_instanceName, index);
			writer.WriteLine("    position = {0}_digitNum_{1} - position - 1u;", m_instanceName, index);
            writer.WriteLine("    for (i = 0u; i < 7u; i++)");
            writer.WriteLine("    {");
            writer.WriteLine("        val = (({0}_7SegDigits[digit] >> i) & 0x01u); ", m_instanceName);
            writer.WriteLine("        (void) {0}_WRITE_PIXEL({1}_disp{2}[position][i], val);", m_instanceName, m_instanceName, index);
            writer.WriteLine("    }");
            writer.WriteLine("}");
        }

        void Write7SegNumber_n(TextWriter writer, TextWriter writer_h, int index)
        {
            writer_h.WriteLine("void {0}_Write7SegNumber_{1}(uint16 value, uint8 position, uint8 mode);", m_instanceName, index);
            writer.WriteLine("/*******************************************************************************");
            writer.WriteLine("* Function Name: {0}_Write7SegNumber_{1}", m_instanceName, index);
            writer.WriteLine("********************************************************************************");
            writer.WriteLine("*");            
            writer.WriteLine("* Summary:");
            writer.WriteLine("*  This function displays a 16-bit integer value on a 1 to 5");
            writer.WriteLine("*  digit 7 segment display. The user must have defined what portion of");
            writer.WriteLine("*  the displays segments make up the 7 segment display portion In");
            writer.WriteLine("*  the wizard. Multiple, separate 7 segment displays can be created in");
            writer.WriteLine("*  the frame buffer and are addressed through the index (n) in the");
            writer.WriteLine("*  function name. Function/s only included if component 7 segment");
            writer.WriteLine("*  wizard defines the 7 segment option.");
            writer.WriteLine("*");
            writer.WriteLine("* Parameters:  ");
            writer.WriteLine("*  Value : unsigned integer value to be displayed.");
            writer.WriteLine("*  position : Position of the least significant digit as");
            writer.WriteLine("*  counted right to left starting at 0 on the right. If the defined");
            writer.WriteLine("*  display contains fewer digits then the Value requires for");
            writer.WriteLine("*  display for the most significant digit/s will not be displayed");
            writer.WriteLine("*  mode : 0=no leading 0s are displayed, 1= leading 0s are  displayed.");
            writer.WriteLine("*");
            writer.WriteLine("* Return: ");
            writer.WriteLine("*  None.");
            writer.WriteLine("*");
            writer.WriteLine("* Reentrant:");
            writer.WriteLine("*  No.");
            writer.WriteLine("*");
            writer.WriteLine("*******************************************************************************/");
            writer.WriteLine("void {0}_Write7SegNumber_{1}(uint16 value, uint8 position, uint8 mode)", m_instanceName, index);
            writer.WriteLine("{ ");
            writer.WriteLine("    int8 i, num;");
            writer.WriteLine("");
            writer.WriteLine("    if (value == 0u)");
            writer.WriteLine("    {");
            writer.WriteLine("        if (mode == 0u) {0}_Write7SegDigit_{1}(0u, position);", m_instanceName, index);
            writer.WriteLine("        else");
            writer.WriteLine("        {");
            writer.WriteLine("            for (i = position; i < {0}_digitNum_{1}; i++) ", m_instanceName, index);
            writer.WriteLine("            {");
            writer.WriteLine("                {0}_Write7SegDigit_{1}(0u, i);", m_instanceName, index);
            writer.WriteLine("            }");
            writer.WriteLine("        }");
            writer.WriteLine("    }");
            writer.WriteLine("    else");
            writer.WriteLine("    {");
            writer.WriteLine("        for (i = position; i <= {0}_digitNum_{1}; i++)", m_instanceName, index);
            writer.WriteLine("        {");
            writer.WriteLine("            num = value % 10u;");
            writer.WriteLine("            {0}_Write7SegDigit_{1}(num, i);", m_instanceName, index);
            writer.WriteLine("            value = (value - num) / 10u; ");
            writer.WriteLine("            if ((value == 0u) && (mode == 0u))");
            writer.WriteLine("            {");
            writer.WriteLine("                break;");
            writer.WriteLine("            }");
            writer.WriteLine("        }");
            writer.WriteLine("    }");
            writer.WriteLine("}");

        }
        #endregion

        #region PutChar14SegDisp
        void PutChar14seg_n(TextWriter writer, TextWriter writer_h, int index)
        {
            writer_h.WriteLine("#define {0}_14SEG", m_instanceName);
            writer_h.WriteLine("#define ALPHANUMERIC");
            writer_h.WriteLine("void {0}_PutChar14Seg_{1}(uint8 character, uint8 position);", m_instanceName, index);

            writer.WriteLine("/*******************************************************************************");
            writer.WriteLine("* Function Name: {0}_PutChar14Seg_{1}", m_instanceName, index);
            writer.WriteLine("********************************************************************************");
            writer.WriteLine("*");
            writer.WriteLine("* Summary:");
            writer.WriteLine("*  This function displays an 8-bit char on an array of");
            writer.WriteLine("*  alphanumeric character displays. The user must have defined what");
            writer.WriteLine("*  portion of the displays segments make up the alphanumeric display");
            writer.WriteLine("*  portion in the wizard. Multiple, separate alphanumeric displays can");
            writer.WriteLine("*  be created in the frame buffer and are addressed through the index");
            writer.WriteLine("*  (n) in the function name. Function/s only included if component");
            writer.WriteLine("*  alphanumeric wizard defines the alphanumeric option.");
            writer.WriteLine("*");
            writer.WriteLine("* Parameters:  ");
            writer.WriteLine("*  character : Character.");
            writer.WriteLine("*  position : Position of the character as counted left to");
            writer.WriteLine("*  right starting at 0 on the left. If the position is outside the");
            writer.WriteLine("*  display range, the character will not be displayed.");
            writer.WriteLine("*");
            writer.WriteLine("* Return: ");
            writer.WriteLine("*  None.");
            writer.WriteLine("*");
            writer.WriteLine("* Reentrant:");
            writer.WriteLine("*  No.");
            writer.WriteLine("*");
            writer.WriteLine("*******************************************************************************/");
            writer.WriteLine("void {0}_PutChar14Seg_{1}(uint8 character, uint8 position)", m_instanceName, index);
            writer.WriteLine("{");
            writer.WriteLine("    uint8 i, val;");
            writer.WriteLine("");
            writer.WriteLine("    for (i = 0u; i < 14u; i++)");
            writer.WriteLine("    {");
            writer.WriteLine("        val = ((uint8)({0}_14SegChars[character] >> i) & 0x01u);", m_instanceName);
            writer.WriteLine("        (void) {0}_WRITE_PIXEL({1}_disp{2}[position][i], val);", m_instanceName, m_instanceName, index);
            writer.WriteLine("    }");
            writer.WriteLine("}");
        }
        void WriteString14seg_n(TextWriter writer, TextWriter writer_h, int index)
        {
            writer_h.WriteLine("void {0}_WriteString14Seg_{1}(uint8* character, uint8 position);", m_instanceName, index);
            writer.WriteLine("/*******************************************************************************");
            writer.WriteLine("* Function Name: {0}_WriteString14Seg_{1}", m_instanceName, index);
            writer.WriteLine("********************************************************************************");
            writer.WriteLine("*");
            writer.WriteLine("* Summary:");
            writer.WriteLine("*  This function displays an 8-bit null terminated");
            writer.WriteLine("*  character string on an array of alphanumeric character displays.");
            writer.WriteLine("*  The user must have defined what portion of the displays segments");
            writer.WriteLine("*  make up the alphanumeric display portion in the wizard. Multiple,");
            writer.WriteLine("*  separate alphanumeric displays can be created in the frame buffer");
            writer.WriteLine("*  and are addressed through the index (n) in the function name.");
            writer.WriteLine("*  Function/s only included if component alphanumeric wizard defines");
            writer.WriteLine("*  the alphanumeric option.");
            writer.WriteLine("*");
            writer.WriteLine("* Parameters:  ");
            writer.WriteLine("*  character : Pointer to the null terminated character string.");
            writer.WriteLine("*  position : The Position of the first character as counted left");
            writer.WriteLine("*  to right starting at 0 on the left. If the defined display contains");
            writer.WriteLine("*  fewer characters then the string requires for display, the");
            writer.WriteLine("*  extra characters will not be displayed.");
            writer.WriteLine("*");
            writer.WriteLine("* Return: ");
            writer.WriteLine("*  None.");
            writer.WriteLine("*");
            writer.WriteLine("* Reentrant:");
            writer.WriteLine("*  No.");
            writer.WriteLine("*");
            writer.WriteLine("*******************************************************************************/");
            writer.WriteLine("void {0}_WriteString14Seg_{1}(uint8* character, uint8 position)", m_instanceName, index);
            writer.WriteLine("{");
            writer.WriteLine("    uint8 c, i;");
            writer.WriteLine("    i = 0u;");
            writer.WriteLine("");
            writer.WriteLine("    while ((character[i] != 0u) && ((position + i) != {0}_digitNum_{1}))", m_instanceName, index);
            writer.WriteLine("    {");
            writer.WriteLine("        c = character[i];");
            writer.WriteLine("        {0}_PutChar14Seg_{1}(c, position + i);", m_instanceName, index);
            writer.WriteLine("        i++;");
            writer.WriteLine("    }");
            writer.WriteLine("}");
        }
        #endregion

        #region PutChar16segDisp
        void PutChar16seg_n(TextWriter writer, TextWriter writer_h, int index)
        {
            writer_h.WriteLine("#define {0}_16SEG", m_instanceName);
            writer_h.WriteLine("#define ALPHANUMERIC");
            writer_h.WriteLine("void {0}_PutChar16Seg_{1}(uint8 character, uint8 position);", m_instanceName, index);

            writer.WriteLine("/*******************************************************************************");
            writer.WriteLine("* Function Name: {0}_PutChar16Seg_{1}", m_instanceName, index);
            writer.WriteLine("********************************************************************************");
            writer.WriteLine("*");
            writer.WriteLine("* Summary:");
            writer.WriteLine("*  This function displays an 8-bit char on an array of");
            writer.WriteLine("*  alphanumeric character displays. The user must have defined what");
            writer.WriteLine("*  portion of the displays segments make up the alphanumeric display");
            writer.WriteLine("*  portion in the wizard. Multiple, separate alphanumeric displays can");
            writer.WriteLine("*  be created in the frame buffer and are addressed through the index");
            writer.WriteLine("*  (n) in the function name. Function/s only included if component");
            writer.WriteLine("*  alphanumeric wizard defines the alphanumeric option.");
            writer.WriteLine("*");
            writer.WriteLine("* Parameters:  ");
            writer.WriteLine("*  character : Character.");
            writer.WriteLine("*  position : Position of the character as counted left to");
            writer.WriteLine("*  right starting at 0 on the left. If the position is outside the");
            writer.WriteLine("*  display range, the character will not be displayed.");
            writer.WriteLine("*");
            writer.WriteLine("* Return: ");
            writer.WriteLine("*  None.");
            writer.WriteLine("*");
            writer.WriteLine("* Reentrant:");
            writer.WriteLine("*  No.");
            writer.WriteLine("*");
            writer.WriteLine("*******************************************************************************/");
            writer.WriteLine("void {0}_PutChar16Seg_{1}(uint8 character, uint8 position)", m_instanceName, index);
            writer.WriteLine("{");
            writer.WriteLine("    uint8 i, val;");
            writer.WriteLine("");
            writer.WriteLine("    for (i = 0u; i < 16; i++)");
            writer.WriteLine("    {");
            writer.WriteLine("        val = ((uint8)({0}_16SegChars[character] >> i) & 0x01u); ", m_instanceName);
            writer.WriteLine("        (void) {0}_WRITE_PIXEL({1}_disp{2}[position][i], val); ", m_instanceName, m_instanceName, index);
            writer.WriteLine("    }");
            writer.WriteLine("}");
        }
        void WriteString16seg_n(TextWriter writer, TextWriter writer_h, int index)
        {
            writer_h.WriteLine("void {0}_WriteString16Seg_{1}(uint8* character, uint8 position);", m_instanceName, index);

            writer.WriteLine("/*******************************************************************************");
            writer.WriteLine("* Function Name: {0}_WriteString16Seg_{1}", m_instanceName, index);
            writer.WriteLine("********************************************************************************");
            writer.WriteLine("*");
            writer.WriteLine("* Summary:");
            writer.WriteLine("*  This function displays an 8-bit null terminated");
            writer.WriteLine("*  character string on an array of alphanumeric character displays.");
            writer.WriteLine("*  The user must have defined what portion of the displays segments");
            writer.WriteLine("*  make up the alphanumeric display portion in the wizard. Multiple,");
            writer.WriteLine("*  separate alphanumeric displays can be created in the frame buffer");
            writer.WriteLine("*  nand are addressed through the index (n) in the function name.");
            writer.WriteLine("*  Function/s only included if component alphanumeric wizard defines");
            writer.WriteLine("*  the alphanumeric option.");
            writer.WriteLine("*");
            writer.WriteLine("* Parameters:  ");
            writer.WriteLine("*  character : Pointer to the null terminated character string.");
            writer.WriteLine("*  position : The Position of the first character as counted left");
            writer.WriteLine("*  to right starting at 0 on the left. If the defined display contains");
            writer.WriteLine("*  fewer characters then the string requires for display, the");
            writer.WriteLine("*  extra characters will not be displayed.");
            writer.WriteLine("*");
            writer.WriteLine("* Return: ");
            writer.WriteLine("*  None.");
            writer.WriteLine("*");
            writer.WriteLine("* Reentrant:");
            writer.WriteLine("*  No.");
            writer.WriteLine("*");
            writer.WriteLine("*******************************************************************************/");
            writer.WriteLine("void {0}_WriteString16Seg_{1}(uint8* character, uint8 position)", m_instanceName, index);
            writer.WriteLine("{");
            writer.WriteLine("    uint8 c, i;");
            writer.WriteLine("    i = 0u;");
            writer.WriteLine("");
            writer.WriteLine("    while((character[i] != 0u) && ((position + i) != {0}_digitNum_{1}))", m_instanceName, index);
            writer.WriteLine("    {");
            writer.WriteLine("        c = character[i];");
            writer.WriteLine("        {0}_PutChar16Seg_{1}(c, position + i);", m_instanceName, index);
            writer.WriteLine("        i++;");
            writer.WriteLine("    }");
            writer.WriteLine("}");
            
        }
        #endregion

        #region WriteBargraph_n
        void WriteBargraph_n(TextWriter writer, TextWriter writer_h, int index, int maxNumber)
        {
            writer_h.WriteLine("void {0}_WriteBargraph_{1}(uint16 location, int8 mode);", m_instanceName, index);
            writer.WriteLine("");
            writer.WriteLine("");
            writer.WriteLine("/*******************************************************************************");
            writer.WriteLine("* Function Name: {0}_WriteBargraph_{1}", m_instanceName, index);
            writer.WriteLine("********************************************************************************");
            writer.WriteLine("*");
            writer.WriteLine("* Summary:");
            writer.WriteLine("*  This function displays an 8-bit integer");
            writer.WriteLine("*  Location on a 1 to 255 segment bar-graph (numbered left to right).");
            writer.WriteLine("*  The bar graph may be any user defined size between 1 and 255");
            writer.WriteLine("*  segments. A bar graph may also be created in a circle to display");
            writer.WriteLine("*  rotary position. The user defines what portion of the displays");
            writer.WriteLine("*  segments make up the bar-graph portion. Multiple, separate bargraph");
            writer.WriteLine("*  displays can be created in the frame buffer and are");
            writer.WriteLine("*  addressed through the index (n) in the function name. Function/s");
            writer.WriteLine("*  only included if component bar-graph wizard defines the 7 segment");
            writer.WriteLine("*  option");
            writer.WriteLine("*");
            writer.WriteLine("* Parameters:  ");
            writer.WriteLine("*  location : unsigned integer location to be displayed. 0 - all");
            writer.WriteLine("*  bar-graph elements off. Max Value = the number of");
            writer.WriteLine("*  segments in the bar-graph. Locations greater then the");
            writer.WriteLine("*  number of segments in the bar-graph will be limited to the");
            writer.WriteLine("*  number of segments physically provided.");
            writer.WriteLine("*  mode : 0 - only the location segment is turned on, 1=The              ");
            writer.WriteLine("*  location segment all segments to the left are turned on, -");
            writer.WriteLine("*  1 - the location segment and all segments to the right are");
            writer.WriteLine("*  turned on. 2-10 display the location segment and 2-254");
            writer.WriteLine("*  segments to the right to create wide indicators.");
            writer.WriteLine("*");
            writer.WriteLine("* Return: ");
            writer.WriteLine("*  None.");
            writer.WriteLine("*");
            writer.WriteLine("* Reentrant:");
            writer.WriteLine("*  No.");
            writer.WriteLine("*");
            writer.WriteLine("*******************************************************************************/");
            writer.WriteLine("void {0}_WriteBargraph_{1}(uint16 location, int8 mode)", m_instanceName, index);
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
            writer.WriteLine("            case -1:");
            writer.WriteLine("                for(i = locationInt; i <= maxValue; i++) ");
            writer.WriteLine("                {");
            writer.WriteLine("                    (void) {0}_WRITE_PIXEL({1}_disp{2}[i][0u], {3}_PIXEL_STATE_ON);", m_instanceName, m_instanceName, index, m_instanceName);
            writer.WriteLine("                }");
            writer.WriteLine("                break;");
            writer.WriteLine("            case 0:");
            writer.WriteLine("                (void) {0}_WRITE_PIXEL({1}_disp{2}[locationInt][0u], {3}_PIXEL_STATE_ON);", m_instanceName, m_instanceName, index, m_instanceName);
            writer.WriteLine("                break;");
            writer.WriteLine("            case 1:");
            writer.WriteLine("                for(i = locationInt; i >= 1u; i--) ");
            writer.WriteLine("                {");
            writer.WriteLine("                    (void) {0}_WRITE_PIXEL({1}_disp{2}[i][0u], {3}_PIXEL_STATE_ON);", m_instanceName, m_instanceName, index, m_instanceName);
            writer.WriteLine("                }");
            writer.WriteLine("                break;");
            writer.WriteLine("            case 2:");
            writer.WriteLine("                if(locationInt + 1u < maxValue) ");
            writer.WriteLine("                {");
            writer.WriteLine("                    maxValue = locationInt + 1u;");
            writer.WriteLine("                }");
            writer.WriteLine("                for(i = locationInt; i <= maxValue; i++) ");
            writer.WriteLine("                {");
            writer.WriteLine("                    (void) {0}_WRITE_PIXEL({1}_disp{2}[i][0u], {3}_PIXEL_STATE_ON);", m_instanceName, m_instanceName, index, m_instanceName);
            writer.WriteLine("                }");
            writer.WriteLine("                break;");
            writer.WriteLine("            case 3:");
            writer.WriteLine("                if(locationInt + 2u < maxValue) ");
            writer.WriteLine("                {");
            writer.WriteLine("                    maxValue = locationInt + 2u;");
            writer.WriteLine("                }");
            writer.WriteLine("                for(i = locationInt; i <= maxValue; i++) ");
            writer.WriteLine("                {");
            writer.WriteLine("                    (void) {0}_WRITE_PIXEL({1}_disp{2}[i][0u], {3}_PIXEL_STATE_ON);", m_instanceName, m_instanceName, index, m_instanceName);
            writer.WriteLine("                }");
            writer.WriteLine("                break;");
            writer.WriteLine("            case 4:");
            writer.WriteLine("                if(locationInt + 3u < maxValue) ");
            writer.WriteLine("                {");
            writer.WriteLine("                    maxValue = locationInt + 3u;");
            writer.WriteLine("                }");
            writer.WriteLine("                for(i = locationInt; i <= maxValue; i++) ");
            writer.WriteLine("                {");
            writer.WriteLine("                    (void) {0}_WRITE_PIXEL({1}_disp{2}[i][0u], {3}_PIXEL_STATE_ON);", m_instanceName, m_instanceName, index, m_instanceName);
            writer.WriteLine("                }");
            writer.WriteLine("                break;");
            writer.WriteLine("            case 5:");
            writer.WriteLine("                if(locationInt + 4u < maxValue) ");
            writer.WriteLine("                {");
            writer.WriteLine("                    maxValue = locationInt + 4u;");
            writer.WriteLine("                }");
            writer.WriteLine("                for(i = locationInt; i <= maxValue; i++) ");
            writer.WriteLine("                {");
            writer.WriteLine("                    (void) {0}_WRITE_PIXEL({1}_disp{2}[i][0u], {3}_PIXEL_STATE_ON);", m_instanceName, m_instanceName, index, m_instanceName);
            writer.WriteLine("                }");
            writer.WriteLine("                break;");
            writer.WriteLine("            case 6:");
            writer.WriteLine("                if(locationInt + 5u < maxValue) ");
            writer.WriteLine("                {");
            writer.WriteLine("                    maxValue = locationInt + 5u;");
            writer.WriteLine("                }");
            writer.WriteLine("                for(i = locationInt; i <= maxValue; i++) ");
            writer.WriteLine("                {");
            writer.WriteLine("                    (void) {0}_WRITE_PIXEL({1}_disp{2}[i][0u], {3}_PIXEL_STATE_ON);", m_instanceName, m_instanceName, index, m_instanceName);
            writer.WriteLine("                }");
            writer.WriteLine("                break;");
            writer.WriteLine("            case 7:");
            writer.WriteLine("                if(locationInt + 6u < maxValue) ");
            writer.WriteLine("                {");
            writer.WriteLine("                    maxValue = locationInt + 6u;");
            writer.WriteLine("                }");
            writer.WriteLine("                for(i = locationInt; i <= maxValue; i++) ");
            writer.WriteLine("                {");
            writer.WriteLine("                    (void) {0}_WRITE_PIXEL({1}_disp{2}[i][0u], {3}_PIXEL_STATE_ON);", m_instanceName, m_instanceName, index, m_instanceName);
            writer.WriteLine("                }");
            writer.WriteLine("                break;");
            writer.WriteLine("            case 8:");
            writer.WriteLine("                if(locationInt + 7u < maxValue) ");
            writer.WriteLine("                {");
            writer.WriteLine("                    maxValue = locationInt + 7u;");
            writer.WriteLine("                }");
            writer.WriteLine("                for(i = locationInt; i <= maxValue; i++) ");
            writer.WriteLine("                {");
            writer.WriteLine("                    (void) {0}_WRITE_PIXEL({1}_disp{2}[i][0u], {3}_PIXEL_STATE_ON);", m_instanceName, m_instanceName, index, m_instanceName);
            writer.WriteLine("                }");
            writer.WriteLine("                break;");
            writer.WriteLine("            case 9:");
            writer.WriteLine("                if(locationInt + 8u < maxValue) ");
            writer.WriteLine("                {");
            writer.WriteLine("                    maxValue = locationInt + 8u;");
            writer.WriteLine("                }");
            writer.WriteLine("                for(i = locationInt; i <= maxValue; i++) ");
            writer.WriteLine("                {");
            writer.WriteLine("                    (void) {0}_WRITE_PIXEL({1}_disp{2}[i][0u], {3}_PIXEL_STATE_ON);", m_instanceName, m_instanceName, index, m_instanceName);
            writer.WriteLine("                }");
            writer.WriteLine("                break;");
            writer.WriteLine("            case 10:");
            writer.WriteLine("                if(locationInt + 9u <= maxValue) ");
            writer.WriteLine("                {");
            writer.WriteLine("                    maxValue = locationInt + 9u;");
            writer.WriteLine("                }");
            writer.WriteLine("                for(i = locationInt; i < maxValue; i++) ");
            writer.WriteLine("                {");
            writer.WriteLine("                    (void) {0}_WRITE_PIXEL({1}_disp{2}[i][0u], {3}_PIXEL_STATE_ON);", m_instanceName, m_instanceName, index, m_instanceName);
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
            writer.WriteLine("            {0}_WRITE_PIXEL({1}_disp{2}[i][0u], {3}_PIXEL_STATE_OFF);", m_instanceName, m_instanceName, index, m_instanceName);
            writer.WriteLine("        }");
            writer.WriteLine("    }");
            writer.WriteLine("}");
        }

        #endregion

    }
}
