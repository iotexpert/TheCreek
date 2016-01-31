/*******************************************************************************
* Copyright 2008-2009, Cypress Semiconductor Corporation.  All rights reserved.
* You may use this file only in accordance with the license, terms, conditions, 
* disclaimers, and limitations in the end user license agreement accompanying 
* the software package with which this file was provided.
********************************************************************************/



using System;
using System.Collections.Generic;
using System.Text;
using System.IO;

namespace SegLCD_v1_10
{
    partial class M_APIGenerator
    {
        #region Write7SegDisp
        void Write7SegDigit_n(ref TextWriter writer, ref TextWriter writer_h, int index)
        {
            //writer_h.WriteLine("#ifndef " + m_instanceName + "_7SEG");
            writer_h.WriteLine("#define " + m_instanceName + "_7SEG");
            writer_h.WriteLine("void " + m_instanceName + "_Write7SegDigit_" + index + "(uint8 Digit, uint8 Position);");

            writer.WriteLine("/*******************************************************************************");
            writer.WriteLine("* Function Name: " + m_instanceName + "_Write7SegDigit_" + index + "");
            writer.WriteLine("********************************************************************************");
            writer.WriteLine("* Summary:");
            writer.WriteLine("*   This function displays an 4-bit Hex digit in the range of");
            writer.WriteLine("*   0-9 and A-F 7 segment display. The user must have defined what");
            writer.WriteLine("*   portion of the displays segments make up the 7 segment display");
            writer.WriteLine("*   portion in the component wizard. Multiple, separate 7 segment");
            writer.WriteLine("*   displays can be created in the frame buffer and are addressed");
            writer.WriteLine("*   through the index (n) in the function name. Function/s only included");
            writer.WriteLine("*   if component 7 segment wizard defines the 7 segment option.");
            writer.WriteLine("*");
            writer.WriteLine("* m_Parameters:  ");
            writer.WriteLine("*  Digit : unsigned integer value in the range of 0 to 15 to");
            writer.WriteLine("*  be displayed as a hexadecimal digit. ");
            writer.WriteLine("*  Position : Position of the digit as counted right to left");
            writer.WriteLine("*  starting at 0 on the right. If the defined display does not");
            writer.WriteLine("*  contain a digit in the Position then the digit will not be");
            writer.WriteLine("*  displayed");
            writer.WriteLine("*");
            writer.WriteLine("* Return: ");
            writer.WriteLine("*  (void)");
            writer.WriteLine("*");
            writer.WriteLine("* Theory: ");
            writer.WriteLine("*");
            writer.WriteLine("* Side Effects:");
            writer.WriteLine("*");
            writer.WriteLine("*******************************************************************************/");

            writer.WriteLine("void " + m_instanceName + "_Write7SegDigit_" + index + "(uint8 Digit, uint8 Position)");
            writer.WriteLine("{");
            writer.WriteLine("\tuint8 i, Val;");
            writer.WriteLine("");
            writer.WriteLine("\tif(Digit <= 16)  /* Do nothing: Digit = Digit */ ;");
            writer.WriteLine("");
            writer.WriteLine("\telse if(Digit <= 0x39)");
            writer.WriteLine("\t\tDigit -= 0x30;");
            writer.WriteLine("\telse if(Digit <= 0x47)");
            writer.WriteLine("\t\tDigit -= 0x37;");
            writer.WriteLine("\telse");
            writer.WriteLine("\t\tDigit = 8;");
            writer.WriteLine("");
			writer.WriteLine("\tif((Position / " + m_instanceName + "_DigitNum_" + index + ") == 0)");
			writer.WriteLine("\t{");
//			writer.WriteLine("\tPosition = Position % " + m_instanceName + "_DigitNum_" + index + ";");
			writer.WriteLine("\tPosition = " + m_instanceName + "_DigitNum_" + index + " - Position - 1;");
            writer.WriteLine("\tfor (i = 0; i < 7; i++)");
            writer.WriteLine("\t{");
            writer.WriteLine("\t\tVal = ((" + m_instanceName + "_7SegDigits[Digit] >> i) & 0x01); ");
            writer.WriteLine("\t\t" + m_instanceName + "_WRITE_PIXEL(" + m_instanceName + "_Disp" + index + "[Position][i], Val);");
            writer.WriteLine("\t}");
			writer.WriteLine("\t}");
            writer.WriteLine("}");
        }

        void Write7SegNumber_n(ref TextWriter writer, ref TextWriter writer_h, int index)
        {
            writer_h.WriteLine("void " + m_instanceName + "_Write7SegNumber_" + index + "(uint16 Value, uint8 Position, uint8 Mode);");
            //writer_h.WriteLine("#endif /* " + m_instanceName + "_7SEG */");

            writer.WriteLine("/*******************************************************************************");
            writer.WriteLine("* Function Name: " + m_instanceName + "_Write7SegNumber_" + index + "");
            writer.WriteLine("********************************************************************************");
            writer.WriteLine("* Summary:");
            writer.WriteLine("*   This function displays a 16-bit integer value on a 1 to 5");
            writer.WriteLine("*   digit 7 segment display. The user must have defined what portion of");
            writer.WriteLine("*   the displays segments make up the 7 segment display portion In");
            writer.WriteLine("*   the wizard. Multiple, separate 7 segment displays can be created in");
            writer.WriteLine("*   the frame buffer and are addressed through the index (n) in the");
            writer.WriteLine("*   function name. Function/s only included if component 7 segment");
            writer.WriteLine("*   wizard defines the 7 segment option.");
            writer.WriteLine("*");
            writer.WriteLine("* m_Parameters:  ");
            writer.WriteLine("*  Value : unsigned integer value to be displayed.");
            writer.WriteLine("*  Position : Position of the least significant digit as");
            writer.WriteLine("*  counted right to left starting at 0 on the right. If the defined");
            writer.WriteLine("*  display contains fewer digits then the Value requires for");
            writer.WriteLine("*  display for the most significant digit/s will not be displayed");
            writer.WriteLine("*  Mode : 0=no leading 0s are displayed, 1= leading 0s are  displayed.");
            writer.WriteLine("*");
            writer.WriteLine("* Return: ");
            writer.WriteLine("*  (void)");
            writer.WriteLine("*");
            writer.WriteLine("* Theory: ");
            writer.WriteLine("*");
            writer.WriteLine("* Side Effects:");
            writer.WriteLine("*");
            writer.WriteLine("*******************************************************************************/");
            writer.WriteLine("void " + m_instanceName + "_Write7SegNumber_" + index + "(uint16 Value, uint8 Position, uint8 Mode)");
            writer.WriteLine("{ ");
            writer.WriteLine("\tint8 i, Num;");
            writer.WriteLine("");
			writer.WriteLine("\tPosition = Position % " + m_instanceName + "_DigitNum_" + index + ";");
            writer.WriteLine("\tif (Value == 0)");
            writer.WriteLine("\t{");
            writer.WriteLine("\t\tif (Mode == 0) " + m_instanceName + "_Write7SegDigit_" + index + "(0, Position);");
            writer.WriteLine("\t\telse");
            writer.WriteLine("\t\t{");
            writer.WriteLine("\t\t\tfor (i = Position; i < " + m_instanceName + "_DigitNum_" + index + "; i++) " + m_instanceName + "_Write7SegDigit_" + index + "(0,i); 	     ");
            writer.WriteLine("\t\t}");
            writer.WriteLine("\t}");
            writer.WriteLine("\telse");
            writer.WriteLine("\t{");
            writer.WriteLine("\t\tfor (i = Position; i <= " + m_instanceName + "_DigitNum_" + index + "; i++)");
            writer.WriteLine("\t\t{");
            writer.WriteLine("\t\t\tNum = Value % 10;");
            writer.WriteLine("\t\t\t" + m_instanceName + "_Write7SegDigit_" + index + "(Num, i);");
            writer.WriteLine("\t\t\tValue = (Value - Num)/10; ");
            writer.WriteLine("\t\t\tif ((Value == 0) && (Mode == 0)) break;");
            writer.WriteLine("\t\t}");
            writer.WriteLine("\t}");
            writer.WriteLine("}");

        }
        #endregion

        #region PutChar14SegDisp
        void PutChar14seg_n(ref TextWriter writer, ref TextWriter writer_h, int index)
        {
            //writer_h.WriteLine("#ifndef " + m_instanceName + "_14SEG");
            writer_h.WriteLine("#define " + m_instanceName + "_14SEG");
            writer_h.WriteLine("#define ALPHANUMERIC");
            writer_h.WriteLine("void " + m_instanceName + "_PutChar14Seg_" + index + "(uint8 Character, uint8 Position);");

            writer.WriteLine("/*******************************************************************************");
            writer.WriteLine("* Function Name: " + m_instanceName + "_PutChar14Seg_" + index + "");
            writer.WriteLine("********************************************************************************");
            writer.WriteLine("* Summary:");
            writer.WriteLine("*   This function displays an 8-bit char on an array of");
            writer.WriteLine("*   alphanumeric character displays. The user must have defined what");
            writer.WriteLine("*   portion of the displays segments make up the alphanumeric display");
            writer.WriteLine("*   portion in the wizard. Multiple, separate alphanumeric displays can");
            writer.WriteLine("*   be created in the frame buffer and are addressed through the index");
            writer.WriteLine("*   (n) in the function name. Function/s only included if component");
            writer.WriteLine("*   alphanumeric wizard defines the alphanumeric option.");
            writer.WriteLine("*");
            writer.WriteLine("* m_Parameters:  ");
            writer.WriteLine("*  Character : Character.");
            writer.WriteLine("*  Position : Position of the character as counted left to");
            writer.WriteLine("*  right starting at 0 on the left. If the position is outside the");
            writer.WriteLine("*  display range, the character will not be displayed.");
            writer.WriteLine("*");
            writer.WriteLine("* Return: ");
            writer.WriteLine("*  (void)");
            writer.WriteLine("*");
            writer.WriteLine("* Theory: ");
            writer.WriteLine("*");
            writer.WriteLine("* Side Effects:");
            writer.WriteLine("*");
            writer.WriteLine("*******************************************************************************/");
            writer.WriteLine("void " + m_instanceName + "_PutChar14Seg_" + index + "(uint8 Character, uint8 Position)");
            writer.WriteLine("{");
            writer.WriteLine("\tuint8 i, Val;");
            writer.WriteLine("");
            writer.WriteLine("\tif((Position / " + m_instanceName + "_DigitNum_" + index + ") == 0)");
			writer.WriteLine("\t{");
            writer.WriteLine("\t\tfor (i = 0; i < 14; i++)");
            writer.WriteLine("\t\t{");
            writer.WriteLine("\t\t\tVal = ((uint8)(" + m_instanceName + "_14SegChars[Character] >> i) & 0x01); ");
            writer.WriteLine("\t\t\t" + m_instanceName + "_WRITE_PIXEL(" + m_instanceName + "_Disp" + index + "[Position][i], Val);	 ");
            writer.WriteLine("\t\t}   ");
            writer.WriteLine("\t}");
			writer.WriteLine("}");
        }
        void WriteString14seg_n(ref TextWriter writer, ref TextWriter writer_h, int index)
        {
            writer_h.WriteLine("void " + m_instanceName + "_WriteString14Seg_" + index + "(uint8* Character, uint8 Position);");
            //writer_h.WriteLine("#endif /* " + m_instanceName + "_14SEG */");

            writer.WriteLine("/*******************************************************************************");
            writer.WriteLine("* Function Name: " + m_instanceName + "_WriteString14Seg_" + index + "");
            writer.WriteLine("********************************************************************************");
            writer.WriteLine("* Summary:");
            writer.WriteLine("*   This function displays an 8-bit null terminated");
            writer.WriteLine("*   character string on an array of alphanumeric character displays.");
            writer.WriteLine("*   The user must have defined what portion of the displays segments");
            writer.WriteLine("*   make up the alphanumeric display portion in the wizard. Multiple,");
            writer.WriteLine("*   separate alphanumeric displays can be created in the frame buffer");
            writer.WriteLine("*   and are addressed through the index (n) in the function name.");
            writer.WriteLine("*   Function/s only included if component alphanumeric wizard defines");
            writer.WriteLine("*   the alphanumeric option.");
            writer.WriteLine("*");
            writer.WriteLine("* m_Parameters:  ");
            writer.WriteLine("*  Character : Pointer to the null terminated character string.");
            writer.WriteLine("*  Position : The Position of the first character as counted left");
            writer.WriteLine("*  to right starting at 0 on the left. If the defined display contains");
            writer.WriteLine("*  fewer characters then the string requires for display, the");
            writer.WriteLine("*  extra characters will not be displayed.");
            writer.WriteLine("*");
            writer.WriteLine("* Return: ");
            writer.WriteLine("*  (void)");
            writer.WriteLine("*");
            writer.WriteLine("* Theory: ");
            writer.WriteLine("*");
            writer.WriteLine("* Side Effects:");
            writer.WriteLine("*");
            writer.WriteLine("*******************************************************************************/");
            writer.WriteLine("void " + m_instanceName + "_WriteString14Seg_" + index + "(uint8* Character, uint8 Position)");
            writer.WriteLine("{");
            writer.WriteLine("\tuint8 c, i;");
            writer.WriteLine("\ti = 0;");
            writer.WriteLine("");
            writer.WriteLine("\twhile ((Character[i] != 0) && ((Position + i) != " + m_instanceName + "_DigitNum_" + index + "))");
            writer.WriteLine("\t{");
            writer.WriteLine("\t\tc = Character[i];");
            writer.WriteLine("\t\t" + m_instanceName + "_PutChar14Seg_" + index + "(c, Position+i);");
            writer.WriteLine("\t\ti++;");
            writer.WriteLine("\t}");
            writer.WriteLine("}");
        }
        #endregion

        #region PutChar16segDisp
        void PutChar16seg_n(ref TextWriter writer, ref TextWriter writer_h, int index)
        {
            //writer_h.WriteLine("#ifndef " + m_instanceName + "_16SEG");
            writer_h.WriteLine("#define " + m_instanceName + "_16SEG");
            writer_h.WriteLine("#define ALPHANUMERIC");
            writer_h.WriteLine("void " + m_instanceName + "_PutChar16Seg_" + index + "(uint8 Character, uint8 Position);");

            writer.WriteLine("/*******************************************************************************");
            writer.WriteLine("* Function Name: " + m_instanceName + "_PutChar16Seg_" + index + "");
            writer.WriteLine("********************************************************************************");
            writer.WriteLine("* Summary:");
            writer.WriteLine("*   This function displays an 8-bit char on an array of");
            writer.WriteLine("*   alphanumeric character displays. The user must have defined what");
            writer.WriteLine("*   portion of the displays segments make up the alphanumeric display");
            writer.WriteLine("*   portion in the wizard. Multiple, separate alphanumeric displays can");
            writer.WriteLine("*   be created in the frame buffer and are addressed through the index");
            writer.WriteLine("*   (n) in the function name. Function/s only included if component");
            writer.WriteLine("*   alphanumeric wizard defines the alphanumeric option.");
            writer.WriteLine("*");
            writer.WriteLine("* m_Parameters:  ");
            writer.WriteLine("*  Character : Character.");
            writer.WriteLine("*  Position : Position of the character as counted left to");
            writer.WriteLine("*  right starting at 0 on the left. If the position is outside the");
            writer.WriteLine("*  display range, the character will not be displayed.");
            writer.WriteLine("*");
            writer.WriteLine("* Return: ");
            writer.WriteLine("*  (void)");
            writer.WriteLine("*");
            writer.WriteLine("* Theory: ");
            writer.WriteLine("*");
            writer.WriteLine("* Side Effects:");
            writer.WriteLine("*");
            writer.WriteLine("*******************************************************************************/");
            writer.WriteLine("void " + m_instanceName + "_PutChar16Seg_" + index + "(uint8 Character, uint8 Position)");
            writer.WriteLine("{");
            writer.WriteLine("\tuint8 i, Val;");
            writer.WriteLine("");
		    writer.WriteLine("\tif((Position / " + m_instanceName + "_DigitNum_" + index + ") == 0)");
			writer.WriteLine("\t{");
            writer.WriteLine("\t\tfor (i = 0; i < 16; i++)");
            writer.WriteLine("\t\t{");
            writer.WriteLine("\t\t\tVal = ((uint8)(" + m_instanceName + "_16SegChars[Character] >> i) & 0x01); ");
            writer.WriteLine("\t\t\t" + m_instanceName + "_WRITE_PIXEL(" + m_instanceName + "_Disp" + index + "[Position][i], Val); ");
            writer.WriteLine("\t\t}");
		    writer.WriteLine("\t}");
            writer.WriteLine("}");
        }
        void WriteString16seg_n(ref TextWriter writer, ref TextWriter writer_h, int index)
        {
            writer_h.WriteLine("void " + m_instanceName + "_WriteString16Seg_" + index + "(uint8* Character, uint8 Position);");
            //writer_h.WriteLine("#endif /* " + m_instanceName + "_16SEG */");

            writer.WriteLine("/*******************************************************************************");
            writer.WriteLine("* Function Name: " + m_instanceName + "_WriteString16Seg_" + index + "");
            writer.WriteLine("********************************************************************************");
            writer.WriteLine("* Summary:");
            writer.WriteLine("*   This function displays an 8-bit null terminated");
            writer.WriteLine("*   character string on an array of alphanumeric character displays.");
            writer.WriteLine("*   The user must have defined what portion of the displays segments");
            writer.WriteLine("*   make up the alphanumeric display portion in the wizard. Multiple,");
            writer.WriteLine("*   separate alphanumeric displays can be created in the frame buffer");
            writer.WriteLine("*   nand are addressed through the index (n) in the function name.");
            writer.WriteLine("*   Function/s only included if component alphanumeric wizard defines");
            writer.WriteLine("*   the alphanumeric option.");
            writer.WriteLine("*");
            writer.WriteLine("* m_Parameters:  ");
            writer.WriteLine("*  Character : Pointer to the null terminated character string.");
            writer.WriteLine("*  Position : The Position of the first character as counted left");
            writer.WriteLine("*  to right starting at 0 on the left. If the defined display contains");
            writer.WriteLine("*  fewer characters then the string requires for display, the");
            writer.WriteLine("*  extra characters will not be displayed.");
            writer.WriteLine("*");
            writer.WriteLine("* Return: ");
            writer.WriteLine("*  (void)");
            writer.WriteLine("*");
            writer.WriteLine("* Theory: ");
            writer.WriteLine("*");
            writer.WriteLine("* Side Effects:");
            writer.WriteLine("*");
            writer.WriteLine("*******************************************************************************/");
            writer.WriteLine("void " + m_instanceName + "_WriteString16Seg_" + index + "(uint8* Character, uint8 Position)");
            writer.WriteLine("{");
            writer.WriteLine("\tuint8 c, i;");
            writer.WriteLine("\ti = 0;");
            writer.WriteLine("");
            writer.WriteLine("\twhile ((Character[i] != 0) && ((Position + i) != " + m_instanceName + "_DigitNum_" + index + "))");
            writer.WriteLine("\t{");
            writer.WriteLine("\t\tc = Character[i];");
            writer.WriteLine("\t\t" + m_instanceName + "_PutChar16Seg_" + index + "(c, Position+i);");
            writer.WriteLine("\t\ti++;");
            writer.WriteLine("\t}");
            writer.WriteLine("}");
            
        }
        #endregion

        #region WriteStringDotMatrix_n
        void PutCharDotMatrix_n(ref TextWriter writer, ref TextWriter writer_h, int index)
        {
            //writer_h.WriteLine("#ifndef " + m_instanceName + "_DOT_MATRIX");
            writer_h.WriteLine("#define " + m_instanceName + "_DOT_MATRIX");
            writer_h.WriteLine("#define ALPHANUMERIC");
            writer_h.WriteLine("void " + m_instanceName + "_PutCharDotMatrix_" + index + "(uint8 Character, uint8 Position);");

            writer.WriteLine("/*******************************************************************************");
            writer.WriteLine("* Function Name: " + m_instanceName + "_PutCharDotMatrix_" + index + "");
            writer.WriteLine("********************************************************************************");
            writer.WriteLine("* Summary:");
            writer.WriteLine("*   This function displays an 8-bit char on an array of");
            writer.WriteLine("*   alphanumeric character displays. The user must have defined what");
            writer.WriteLine("*   portion of the displays segments make up the alphanumeric display");
            writer.WriteLine("*   portion in the wizard. Multiple, separate alphanumeric displays can");
            writer.WriteLine("*   be created in the frame buffer and are addressed through the index");
            writer.WriteLine("*   (n) in the function name. Function/s only included if component");
            writer.WriteLine("*   alphanumeric wizard defines the alphanumeric option.");
            writer.WriteLine("*");
            writer.WriteLine("* m_Parameters:  ");
            writer.WriteLine("*  Character : Character.");
            writer.WriteLine("*  Position : The Position of the character as counted left to");
            writer.WriteLine("*  right starting at 0 on the left. If the position is outside the");
            writer.WriteLine("*  display range, the character will not be displayed.");
            writer.WriteLine("*");
            writer.WriteLine("* Return: ");
            writer.WriteLine("*  (void)");
            writer.WriteLine("*");
            writer.WriteLine("* Theory: ");
            writer.WriteLine("*");
            writer.WriteLine("* Side Effects:");
            writer.WriteLine("*");
            writer.WriteLine("*******************************************************************************/");
            writer.WriteLine("void " + m_instanceName + "_PutCharDotMatrix_" + index + "(uint8 Character, uint8 Position)");
            writer.WriteLine("{");
            writer.WriteLine("\tuint8 i, j, Val;");
            writer.WriteLine("");
		    writer.WriteLine("\tif((Position / " + m_instanceName + "_DigitNum_" + index + ") == 0)");
			writer.WriteLine("\t{");
            writer.WriteLine("\t\tfor (j = 0; j < 5; j++)");
            writer.WriteLine("\t\t");
            writer.WriteLine("\t\tfor (i = 0; i < 8; i++)");
            writer.WriteLine("\t\t{");
            writer.WriteLine("\t\t\tVal = ((" + m_instanceName + "_CharDotMatrix[Character][j] >> i) & 0x01); ");
            writer.WriteLine("\t\t\t" + m_instanceName + "_WRITE_PIXEL(" + m_instanceName + "_Disp" + index + "[Position][j+i*5], Val);");
            writer.WriteLine("\t\t}");
			writer.WriteLine("\t}");
            writer.WriteLine("}");
        }
        void WriteStringDotMatrix_n(ref TextWriter writer, ref TextWriter writer_h, int index)
        {
           
            writer_h.WriteLine("void " + m_instanceName + "_WriteStringDotMatrix_" + index + "(uint8* Character, uint8 Position);");
            //writer_h.WriteLine("#endif /* " + m_instanceName + "_DOT_MATRIX */");

            writer.WriteLine("/* Function Name: " + m_instanceName + "_WriteStringDotMatrix_" + index + "");
            writer.WriteLine("********************************************************************************");
            writer.WriteLine("* Summary:");
            writer.WriteLine("*   This function displays an 8-bit null terminated");
            writer.WriteLine("*   character string on an array of alphanumeric character displays.");
            writer.WriteLine("*   The user must have defined what portion of the displays segments");
            writer.WriteLine("*   make up the alphanumeric display portion in the wizard. Multiple,");
            writer.WriteLine("*   separate alphanumeric displays can be created in the frame buffer");
            writer.WriteLine("*   and are addressed through the index (n) in the function name.");
            writer.WriteLine("*   Function/s only included if component alphanumeric wizard defines");
            writer.WriteLine("*   the alphanumeric option.");
            writer.WriteLine("*");
            writer.WriteLine("* m_Parameters:  ");
            writer.WriteLine("*  Character : Pointer to the null terminated character string.");
            writer.WriteLine("*  Position : The Position of the first character as counted left");
            writer.WriteLine("*  to right starting at 0 on the left. If the defined display contains");
            writer.WriteLine("*  fewer characters then the string requires for display, the");
            writer.WriteLine("*  extra characters will not be displayed.");
            writer.WriteLine("*");
            writer.WriteLine("* Return: ");
            writer.WriteLine("*  (void)");
            writer.WriteLine("*");
            writer.WriteLine("* Theory: ");
            writer.WriteLine("*");
            writer.WriteLine("* Side Effects:");
            writer.WriteLine("*");
            writer.WriteLine("*******************************************************************************/");
            writer.WriteLine("void " + m_instanceName + "_WriteStringDotMatrix_" + index + "(uint8* Character, uint8 Position)");
            writer.WriteLine("{");
            writer.WriteLine("\tuint8 i, c;");
            writer.WriteLine("\ti = 0;");
            writer.WriteLine("");
            writer.WriteLine("\twhile((Character[i] != 0) && ((Position + i) != " + m_instanceName + "_DigitNum_" + index + "))");
            writer.WriteLine("\t{");
            writer.WriteLine("\t\tc = Character[i];");
            writer.WriteLine("\t\t" + m_instanceName + "_PutCharDotMatrix_" + index + "(c, Position+i);");
            writer.WriteLine("\t\ti++;");
            writer.WriteLine("\t}");
            writer.WriteLine("}");
        }

        #endregion

        #region WriteBargraph_n
        void WriteBargraph_n(ref TextWriter writer, ref TextWriter writer_h, int index, int maxNumber)
        {
            writer_h.WriteLine("void " + m_instanceName + "_WriteBargraph_" + index + "(uint16 Location, int8 Mode);");
            writer.WriteLine("");
            writer.WriteLine("");
            writer.WriteLine("/*******************************************************************************");
            writer.WriteLine("* Function Name: " + m_instanceName + "_WriteBargraph_" + index + "");
            writer.WriteLine("********************************************************************************");
            writer.WriteLine("* Summary:");
            writer.WriteLine("*   This function displays an 8-bit integer");
            writer.WriteLine("*   Location on a 1 to 255 segment bar-graph (numbered left to right).");
            writer.WriteLine("*   The bar graph may be any user defined size between 1 and 255");
            writer.WriteLine("*   segments. A bar graph may also be created in a circle to display");
            writer.WriteLine("*   rotary position. The user defines what portion of the displays");
            writer.WriteLine("*   segments make up the bar-graph portion. Multiple, separate bargraph");
            writer.WriteLine("*   displays can be created in the frame buffer and are");
            writer.WriteLine("*   addressed through the index (n) in the function name. Function/s");
            writer.WriteLine("*   only included if component bar-graph wizard defines the 7 segment");
            writer.WriteLine("*   option");
            writer.WriteLine("*");
            writer.WriteLine("* m_Parameters:  ");
            writer.WriteLine("*  Location : unsigned integer Location to be displayed. 0 - all");
            writer.WriteLine("*  bar-graph elements off. Max Value = the number of");
            writer.WriteLine("*  segments in the bar-graph. Locations greater then the");
            writer.WriteLine("*  number of segments in the bar-graph will be limited to the");
            writer.WriteLine("*  number of segments physically provided.");
            writer.WriteLine("*  Mode : 0 - only the Location segment is turned on, 1=The              ");
            writer.WriteLine("*  Location segment all segments to the left are turned on, -");
            writer.WriteLine("*  1 - the Location segment and all segments to the right are");
            writer.WriteLine("*  turned on. 2-10 display the Location segment and 2-254");
            writer.WriteLine("*  segments to the right to create wide indicators.");
            writer.WriteLine("*");
            writer.WriteLine("* Return: ");
            writer.WriteLine("*  (void)");
            writer.WriteLine("*");
            writer.WriteLine("* Theory: ");
            writer.WriteLine("*");
            writer.WriteLine("* Side Effects:");
            writer.WriteLine("*");
            writer.WriteLine("*******************************************************************************/");
            writer.WriteLine("void " + m_instanceName + "_WriteBargraph_" + index + "(uint16 Location, int8 Mode)");
            writer.WriteLine("{");
            writer.WriteLine("\tint8 i;");
            writer.WriteLine("\tuint16 MaxValue = " + maxNumber + ";");
            writer.WriteLine("");
			writer.WriteLine("\tif ((Location != 0) && (Location <= MaxValue))");
			writer.WriteLine("\t{");
            writer.WriteLine("\t\tswitch (Mode)");
            writer.WriteLine("\t\t{");
            writer.WriteLine("\t\t\tcase -1:");
            writer.WriteLine("\t\t\tfor (i = Location; i <= MaxValue; i++) ");
            writer.WriteLine("\t\t\t{");
            writer.WriteLine("\t\t\t\t" + m_instanceName + "_WRITE_PIXEL(" + m_instanceName + "_Disp" + index + "[i][0], 1);");
            writer.WriteLine("\t\t\t}");
            writer.WriteLine("\t\t\tbreak;");
            writer.WriteLine("\t\t\tcase 0:");
            writer.WriteLine("\t\t\t\t" + m_instanceName + "_WRITE_PIXEL(" + m_instanceName + "_Disp" + index + "[Location][0], 1);");
            writer.WriteLine("\t\t\tbreak;");
            writer.WriteLine("\t\t\tcase 1:");
            writer.WriteLine("\t\t\tfor (i = Location; i >= 1; i--) ");
            writer.WriteLine("\t\t\t{");
            writer.WriteLine("\t\t\t\t" + m_instanceName + "_WRITE_PIXEL(" + m_instanceName + "_Disp" + index + "[i][0], 1);");
            writer.WriteLine("\t\t\t}");
            writer.WriteLine("\t\t\tbreak;");
            writer.WriteLine("\t\t\tcase 2:");
            writer.WriteLine("\t\t\tif (Location + 2 < MaxValue) ");
            writer.WriteLine("\t\t\t{");
            writer.WriteLine("\t\t\t\tMaxValue = Location + 2;");
            writer.WriteLine("\t\t\t}");
            writer.WriteLine("\t\t\tfor (i = Location; i <= MaxValue; i++) ");
            writer.WriteLine("\t\t\t{");
            writer.WriteLine("\t\t\t\t" + m_instanceName + "_WRITE_PIXEL(" + m_instanceName + "_Disp" + index + "[i][0], 1);");
            writer.WriteLine("\t\t\t}");
            writer.WriteLine("\t\t\tbreak;");
            writer.WriteLine("\t\t\tcase 3:");
            writer.WriteLine("\t\t\tif (Location + 3 < MaxValue) ");
            writer.WriteLine("\t\t\t{");
            writer.WriteLine("\t\t\t\tMaxValue = Location + 3;");
            writer.WriteLine("\t\t\t}");
            writer.WriteLine("\t\t\tfor (i = Location; i <= MaxValue; i++) ");
            writer.WriteLine("\t\t\t{");
            writer.WriteLine("\t\t\t\t" + m_instanceName + "_WRITE_PIXEL(" + m_instanceName + "_Disp" + index + "[i][0], 1);");
            writer.WriteLine("\t\t\t}");
            writer.WriteLine("\t\t\tbreak;");
            writer.WriteLine("\t\t\tcase 4:");
            writer.WriteLine("\t\t\tif (Location + 4 < MaxValue) ");
            writer.WriteLine("\t\t\t{");
            writer.WriteLine("\t\t\t\tMaxValue = Location + 4;");
            writer.WriteLine("\t\t\t}");
            writer.WriteLine("\t\t\tfor (i = Location; i <= MaxValue; i++) ");
            writer.WriteLine("\t\t\t{");
            writer.WriteLine("\t\t\t\t" + m_instanceName + "_WRITE_PIXEL(" + m_instanceName + "_Disp" + index + "[i][0], 1);");
            writer.WriteLine("\t\t\t}");
            writer.WriteLine("\t\t\tbreak;");
            writer.WriteLine("\t\t\tcase 5:");
            writer.WriteLine("\t\t\tif (Location + 5 < MaxValue) ");
            writer.WriteLine("\t\t\t{");
            writer.WriteLine("\t\t\t\tMaxValue = Location + 5;");
            writer.WriteLine("\t\t\t}");
            writer.WriteLine("\t\t\tfor (i = Location; i <= MaxValue; i++) ");
            writer.WriteLine("\t\t\t{");
            writer.WriteLine("\t\t\t\t" + m_instanceName + "_WRITE_PIXEL(" + m_instanceName + "_Disp" + index + "[i][0], 1);");
            writer.WriteLine("\t\t\t}");
            writer.WriteLine("\t\t\tbreak;");
            writer.WriteLine("\t\t\tcase 6:");
            writer.WriteLine("\t\t\tif (Location + 6 < MaxValue) ");
            writer.WriteLine("\t\t\t{");
            writer.WriteLine("\t\t\t\tMaxValue = Location + 6;");
            writer.WriteLine("\t\t\t}");
            writer.WriteLine("\t\t\tfor (i = Location; i <= MaxValue; i++) ");
            writer.WriteLine("\t\t\t{");
            writer.WriteLine("\t\t\t\t" + m_instanceName + "_WRITE_PIXEL(" + m_instanceName + "_Disp" + index + "[i][0], 1);");
            writer.WriteLine("\t\t\t}");
            writer.WriteLine("\t\t\tbreak;");
            writer.WriteLine("\t\t\tcase 7:");
            writer.WriteLine("\t\t\tif (Location + 7 < MaxValue) ");
            writer.WriteLine("\t\t\t{");
            writer.WriteLine("\t\t\t\tMaxValue = Location + 7;");
            writer.WriteLine("\t\t\t}");
            writer.WriteLine("\t\t\tfor (i = Location; i <= MaxValue; i++) ");
            writer.WriteLine("\t\t\t{");
            writer.WriteLine("\t\t\t\t" + m_instanceName + "_WRITE_PIXEL(" + m_instanceName + "_Disp" + index + "[i][0], 1);");
            writer.WriteLine("\t\t\t}");
            writer.WriteLine("\t\t\tbreak;");
            writer.WriteLine("\t\t\tcase 8:");
            writer.WriteLine("\t\t\tif (Location + 8 < MaxValue) ");
            writer.WriteLine("\t\t\t{");
            writer.WriteLine("\t\t\t\tMaxValue = Location + 8;");
            writer.WriteLine("\t\t\t}");
            writer.WriteLine("\t\t\tfor(i = Location;i <= MaxValue; i++) ");
            writer.WriteLine("\t\t\t{");
            writer.WriteLine("\t\t\t\t" + m_instanceName + "_WRITE_PIXEL(" + m_instanceName + "_Disp" + index + "[i][0], 1);");
            writer.WriteLine("\t\t\t}");
            writer.WriteLine("\t\t\tbreak;");
            writer.WriteLine("\t\t\tcase 9:");
            writer.WriteLine("\t\t\tif (Location + 9 < MaxValue) ");
            writer.WriteLine("\t\t\t{");
            writer.WriteLine("\t\t\t\tMaxValue = Location + 9;");
            writer.WriteLine("\t\t\t}");
            writer.WriteLine("\t\t\tfor (i = Location; i <= MaxValue; i++) ");
            writer.WriteLine("\t\t\t{");
            writer.WriteLine("\t\t\t\t" + m_instanceName + "_WRITE_PIXEL(" + m_instanceName + "_Disp" + index + "[i][0], 1);");
            writer.WriteLine("\t\t\t}");
            writer.WriteLine("\t\t\tbreak;");
            writer.WriteLine("\t\t\tcase 10:");
            writer.WriteLine("\t\t\tif (Location + 10 <= MaxValue) ");
            writer.WriteLine("\t\t\t{");
            writer.WriteLine("\t\t\t\tMaxValue = Location + 10;");
            writer.WriteLine("\t\t\t}");
            writer.WriteLine("\t\t\tfor (i = Location; i < MaxValue; i++) ");
            writer.WriteLine("\t\t\t{");
            writer.WriteLine("\t\t\t\t" + m_instanceName + "_WRITE_PIXEL(" + m_instanceName + "_Disp" + index + "[i][0], 1);");
            writer.WriteLine("\t\t\t}");
            writer.WriteLine("\t\t\tbreak;");
            writer.WriteLine("\t\t\tdefault:");
            writer.WriteLine("\t\t\tbreak;");
            writer.WriteLine("\t\t}");
			writer.WriteLine("\t}");
            writer.WriteLine("\telse ");
            writer.WriteLine("\t\tfor (i = 0; i <= MaxValue; i++) ");
            writer.WriteLine("\t\t{");
            writer.WriteLine("\t\t\t" + m_instanceName + "_WRITE_PIXEL(" + m_instanceName + "_Disp" + index + "[i][0], 0);");
            writer.WriteLine("\t\t}");
            writer.WriteLine("}");
        }

        #endregion

    }
}
