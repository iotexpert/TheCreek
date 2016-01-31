/*******************************************************************************
* File Name: `$INSTANCE_NAME`.c
* Version `$CY_MAJOR_VERSION`.`$CY_MINOR_VERSION`
*
* Description:
*  This file provides source code for the Character LCD component's API.
*
* Note:
*
********************************************************************************
* Copyright 2008-2012, Cypress Semiconductor Corporation.  All rights reserved.
* You may use this file only in accordance with the license, terms, conditions,
* disclaimers, and limitations in the end user license agreement accompanying
* the software package with which this file was provided.
*******************************************************************************/

#include "CyLib.h"
#include "`$INSTANCE_NAME`.h"


void `$INSTANCE_NAME`_Init(void) `=ReentrantKeil($INSTANCE_NAME . "_Init")`;
void `$INSTANCE_NAME`_Enable(void) `=ReentrantKeil($INSTANCE_NAME . "_Enable")`;


uint8 `$INSTANCE_NAME`_enableState = 0u;

uint8 `$INSTANCE_NAME`_initVar = 0u;

char8 const CYCODE `$INSTANCE_NAME`_hex[16u] = "0123456789ABCDEF";

extern uint8 const CYCODE `$INSTANCE_NAME`_customFonts[];


/*******************************************************************************
* Function Name: `$INSTANCE_NAME`_Init
********************************************************************************
*
* Summary:
*  Perform initialization required for components normal work.
*  This function initializes the LCD hardware module as follows:
*        Enable 4-bit interface
*        Clear the display
*        Enable auto cursor increment
*        Resets the cursor to start position
*  Also loads custom character set to LCD if it was defined in the customizer.
*
* Parameters:
*  None.
*
* Return:
*  None.
*
* Reentrant:
*  No.
*
*******************************************************************************/
void `$INSTANCE_NAME`_Init(void) `=ReentrantKeil($INSTANCE_NAME . "_Init")`
{
    /* INIT CODE */
    CyDelay(40u);                                                        /* Delay 40 ms */
    `$INSTANCE_NAME`_WrCntrlNib(`$INSTANCE_NAME`_DISPLAY_8_BIT_INIT);   /* Selects 8-bit mode */
    CyDelay(5u);                                                         /* Delay 5 ms */
    `$INSTANCE_NAME`_WrCntrlNib(`$INSTANCE_NAME`_DISPLAY_8_BIT_INIT);   /* Selects 8-bit mode */
    CyDelay(15u);                                                        /* Delay 15 ms */
    `$INSTANCE_NAME`_WrCntrlNib(`$INSTANCE_NAME`_DISPLAY_8_BIT_INIT);   /* Selects 8-bit mode */
    CyDelay(1u);                                                         /* Delay 1 ms */
    `$INSTANCE_NAME`_WrCntrlNib(`$INSTANCE_NAME`_DISPLAY_4_BIT_INIT);   /* Selects 4-bit mode */
    CyDelay(5u);                                                         /* Delay 5 ms */

    `$INSTANCE_NAME`_WriteControl(`$INSTANCE_NAME`_CURSOR_AUTO_INCR_ON);    /* Incr Cursor After Writes */
    `$INSTANCE_NAME`_WriteControl(`$INSTANCE_NAME`_DISPLAY_CURSOR_ON);      /* Turn Display, Cursor ON */
    `$INSTANCE_NAME`_WriteControl(`$INSTANCE_NAME`_DISPLAY_2_LINES_5x10);   /* 2 Lines by 5x10 Characters */
    `$INSTANCE_NAME`_WriteControl(`$INSTANCE_NAME`_DISPLAY_CURSOR_OFF);     /* Turn Display, Cursor OFF */
    `$INSTANCE_NAME`_WriteControl(`$INSTANCE_NAME`_CLEAR_DISPLAY);          /* Clear LCD Screen */
    `$INSTANCE_NAME`_WriteControl(`$INSTANCE_NAME`_DISPLAY_ON_CURSOR_OFF);  /* Turn Display ON, Cursor OFF */
    `$INSTANCE_NAME`_WriteControl(`$INSTANCE_NAME`_RESET_CURSOR_POSITION);  /* Set Cursor to 0,0 */
    CyDelay(5u);

    #if(`$INSTANCE_NAME`_CUSTOM_CHAR_SET != `$INSTANCE_NAME`_NONE)
        `$INSTANCE_NAME`_LoadCustomFonts(`$INSTANCE_NAME`_customFonts);
    #endif /* `$INSTANCE_NAME`_CUSTOM_CHAR_SET != `$INSTANCE_NAME`_NONE */
}


/*******************************************************************************
* Function Name: `$INSTANCE_NAME`_Enable
********************************************************************************
*
* Summary:
*  Turns on the display.
*
* Parameters:
*  None.
*
* Return:
*  None.
*
* Reentrant:
*  No.
*
* Theory:
*  This finction has no effect when it called first time as
*  `$INSTANCE_NAME`_Init() turns on the LCD.
*
*******************************************************************************/
void `$INSTANCE_NAME`_Enable(void) `=ReentrantKeil($INSTANCE_NAME . "_Enable")`
{
    `$INSTANCE_NAME`_DisplayOn();
    `$INSTANCE_NAME`_enableState = 1u;
}


/*******************************************************************************
* Function Name: `$INSTANCE_NAME`_Start
********************************************************************************
*
* Summary:
*  Perform initialization required for components normal work.
*  This function initializes the LCD hardware module as follows:
*        Enable 4-bit interface
*        Clear the display
*        Enable auto cursor increment
*        Resets the cursor to start position
*  Also loads custom character set to LCD if it was defined in the customizer.
*  If it was not the first call in this project then it just turns on the
*  display
*
*
* Parameters:
*  `$INSTANCE_NAME`_initVar - global variable.
*
* Return:
*  `$INSTANCE_NAME`_initVar - global variable.
*
* Reentrant:
*  No.
*
*******************************************************************************/
void `$INSTANCE_NAME`_Start() `=ReentrantKeil($INSTANCE_NAME . "_Start")`
{
    /* If not initialized then perform initialization */
    if(`$INSTANCE_NAME`_initVar == 0u)
    {
        `$INSTANCE_NAME`_Init();
        `$INSTANCE_NAME`_initVar = 1u;
    }

    /* Turn on the LCD */
    `$INSTANCE_NAME`_Enable();
}


/*******************************************************************************
* Function Name: `$INSTANCE_NAME`_Stop
********************************************************************************
*
* Summary:
*  Turns off the display of the LCD screen.
*
* Parameters:
*  None.
*
* Return:
*  None.
*
* Reentrant:
*  No.
*
*******************************************************************************/
void `$INSTANCE_NAME`_Stop() `=ReentrantKeil($INSTANCE_NAME . "_Stop")`
{
    /* Calls LCD Off Macro */
    `$INSTANCE_NAME`_DisplayOff();
    `$INSTANCE_NAME`_enableState = 0u;
}


/*******************************************************************************
*  Function Name: `$INSTANCE_NAME`_Position
********************************************************************************
*
* Summary:
*  Moves active cursor location to a point specified by the input arguments
*
* Parameters:
*  row:     Specific row of LCD module to be written
*  column:  Column of LCD module to be written
*
* Return:
*  None.
*
* Note:
*  This only applies for LCD displays which use the 2X40 address mode.
*  This results in Row 2 offset from row one by 0x28.
*  When there are more than 2 rows, each row must be fewer than 20 characters.
*
*******************************************************************************/
void `$INSTANCE_NAME`_Position(uint8 row, uint8 column) `=ReentrantKeil($INSTANCE_NAME . "_Position")`
{
    switch (row)
    {
        case 0:
            `$INSTANCE_NAME`_WriteControl(`$INSTANCE_NAME`_ROW_0_START + column);
            break;
        case 1:
            `$INSTANCE_NAME`_WriteControl(`$INSTANCE_NAME`_ROW_1_START + column);
            break;
        case 2:
            `$INSTANCE_NAME`_WriteControl(`$INSTANCE_NAME`_ROW_2_START + column);
            break;
        case 3:
            `$INSTANCE_NAME`_WriteControl(`$INSTANCE_NAME`_ROW_3_START + column);
            break;
        default:
            /* if default case is hit, invalid row argument was passed.*/
            break;
    }
}


/*******************************************************************************
* Function Name: `$INSTANCE_NAME`_PrintString
********************************************************************************
*
* Summary:
*  Writes a zero terminated string to the LCD.
*
* Parameters:
*  string:  pointer to head of char8 array to be written to the LCD module
*
* Return:
*  None.
*
*******************************************************************************/
void `$INSTANCE_NAME`_PrintString(char8 * string) `=ReentrantKeil($INSTANCE_NAME . "_PrintString")`
{
    uint8 indexU8 = 1u;
    char8 current = *string;

    /* Until null is reached, print next character */
    while(current != (char8) '\0')
    {
        `$INSTANCE_NAME`_WriteData(current);
        current = *(string + indexU8);
        indexU8++;
    }
}


/*******************************************************************************
*  Function Name: `$INSTANCE_NAME`_PutChar
********************************************************************************
*
* Summary:
*  Writes a single character to the current cursor position of the LCD module.
*  Custom character names (`$INTANCE_NAME`_CUSTOM_0 through
*  `$INTANCE_NAME`_CUSTOM_7) are acceptable as inputs.
*
* Parameters:
*  character:  character to be written to the LCD
*
* Return:
*  None.
*
*******************************************************************************/
void `$INSTANCE_NAME`_PutChar(char8 character) `=ReentrantKeil($INSTANCE_NAME . "_PutChar")`
{
    `$INSTANCE_NAME`_WriteData(character);
}


/*******************************************************************************
*  Function Name: `$INSTANCE_NAME`_WriteData
********************************************************************************
*
* Summary:
*  Writes a data byte to the LCD module's Data Display RAM.
*
* Parameters:
*  dByte:  byte to be written to LCD module.
*
* Return:
*  None.
*
*******************************************************************************/
void `$INSTANCE_NAME`_WriteData(uint8 dByte) `=ReentrantKeil($INSTANCE_NAME . "_WriteData")`
{
    uint8 nibble;

    `$INSTANCE_NAME`_IsReady();
    nibble = dByte >> `$INSTANCE_NAME`_NIBBLE_SHIFT;

    /* Write high nibble */
    `$INSTANCE_NAME`_WrDatNib(nibble);

    nibble = dByte & `$INSTANCE_NAME`_NIBBLE_MASK;
    /* Write low nibble */
    `$INSTANCE_NAME`_WrDatNib(nibble);
}


/*******************************************************************************
*  Function Name: `$INSTANCE_NAME`_WriteControl
********************************************************************************
*
* Summary:
*  Writes a command byte to the LCD module.
*
* Parameters:
*  cByte:   byte to be written to LCD module.
*
* Return:
*  None.
*
*******************************************************************************/
void `$INSTANCE_NAME`_WriteControl(uint8 cByte) `=ReentrantKeil($INSTANCE_NAME . "_WriteControl")`
{
    uint8 nibble;

    nibble = cByte >> `$INSTANCE_NAME`_NIBBLE_SHIFT;

    `$INSTANCE_NAME`_IsReady();
    nibble &= `$INSTANCE_NAME`_NIBBLE_MASK;

    /* WrCntrlNib(High Nibble) */
    `$INSTANCE_NAME`_WrCntrlNib(nibble);
    nibble = cByte & `$INSTANCE_NAME`_NIBBLE_MASK;

    /* WrCntrlNib(Low Nibble) */
    `$INSTANCE_NAME`_WrCntrlNib(nibble);
}


/*******************************************************************************
* Function Name: `$INSTANCE_NAME`_IsReady
********************************************************************************
*
* Summary:
*  Polls LCD until the ready bit is set.
*
* Parameters:
*  None.
*
* Return:
*  None.
*
* Note:
*  Changes pins to High-Z.
*
*******************************************************************************/
void `$INSTANCE_NAME`_IsReady() `=ReentrantKeil($INSTANCE_NAME . "_IsReady")`
{
    uint8 value = 0u;

    /* Clear the LCD port*/
    `$INSTANCE_NAME`_PORT_DR_REG &= ~`$INSTANCE_NAME`_PORT_MASK ;

    /* Change Port to High-Z Status on data pins */

    /* Mask off data pins to clear old values out */
    value = `$INSTANCE_NAME`_PORT_DM0_REG & ~`$INSTANCE_NAME`_DATA_MASK;
    /* Load in high Z values for data pins, others unchanged */
    `$INSTANCE_NAME`_PORT_DM0_REG = value | (`$INSTANCE_NAME`_HIGH_Z_DM0 & `$INSTANCE_NAME`_DATA_MASK);

    /* Mask off data pins to clear old values out */
    value = `$INSTANCE_NAME`_PORT_DM1_REG & ~`$INSTANCE_NAME`_DATA_MASK;
    /* Load in high Z values for data pins, others unchanged */
    `$INSTANCE_NAME`_PORT_DM1_REG = value | (`$INSTANCE_NAME`_HIGH_Z_DM1 & `$INSTANCE_NAME`_DATA_MASK);

    /* Mask off data pins to clear old values out */
    value = `$INSTANCE_NAME`_PORT_DM2_REG & ~`$INSTANCE_NAME`_DATA_MASK;
    /* Load in high Z values for data pins, others unchanged */
    `$INSTANCE_NAME`_PORT_DM2_REG = value | (`$INSTANCE_NAME`_HIGH_Z_DM2 & `$INSTANCE_NAME`_DATA_MASK);

    /* Make sure RS is low */
    `$INSTANCE_NAME`_PORT_DR_REG &= ~`$INSTANCE_NAME`_RS;

    /* Set R/W high to read */
    `$INSTANCE_NAME`_PORT_DR_REG |= `$INSTANCE_NAME`_RW;

    do
    {
        /* 40 ns delay required before rising Enable and 500ns between neighbour Enables */
        CyDelayUs(0u);

        /* Set E high */
        `$INSTANCE_NAME`_PORT_DR_REG |= `$INSTANCE_NAME`_E;

        /* 360 ns delay the setup time for data pins */
        CyDelayUs(1u);

        /* Get port state */
        value = `$INSTANCE_NAME`_PORT_PS_REG;

        /* Set enable low */
        `$INSTANCE_NAME`_PORT_DR_REG &= ~`$INSTANCE_NAME`_E;

        /* Extract ready bit */
        value &= `$INSTANCE_NAME`_READY_BIT;

        /* Set E high as we in 4-bit interface we need extra oparation */
        `$INSTANCE_NAME`_PORT_DR_REG |= `$INSTANCE_NAME`_E;

        /* 360 ns delay the setup time for data pins */
        CyDelayUs(1u);

        /* Set enable low */
        `$INSTANCE_NAME`_PORT_DR_REG &= ~`$INSTANCE_NAME`_E;

        /* Repeat until bit 4 is not zero. */

    } while (value != 0u);

    /* Set R/W low to write */
    `$INSTANCE_NAME`_PORT_DR_REG &= ~`$INSTANCE_NAME`_RW;

    /* Clear the LCD port*/
    `$INSTANCE_NAME`_PORT_DR_REG &= ~`$INSTANCE_NAME`_PORT_MASK ;

    /* Change Port to Output (Strong) on data pins */

    /* Mask off data pins to clear high z values out */
    value = `$INSTANCE_NAME`_PORT_DM0_REG & (~`$INSTANCE_NAME`_DATA_MASK);
    /* Load in high Z values for data pins, others unchanged */
    `$INSTANCE_NAME`_PORT_DM0_REG = value | (`$INSTANCE_NAME`_STRONG_DM0 & `$INSTANCE_NAME`_DATA_MASK);

    /* Mask off data pins to clear high z values out */
    value = `$INSTANCE_NAME`_PORT_DM1_REG & (~`$INSTANCE_NAME`_DATA_MASK);
    /* Load in high Z values for data pins, others unchanged */
    `$INSTANCE_NAME`_PORT_DM1_REG = value | (`$INSTANCE_NAME`_STRONG_DM1 & `$INSTANCE_NAME`_DATA_MASK);

    /* Mask off data pins to clear high z values out */
    value = `$INSTANCE_NAME`_PORT_DM2_REG & (~`$INSTANCE_NAME`_DATA_MASK);
    /* Load in high Z values for data pins, others unchanged */
    `$INSTANCE_NAME`_PORT_DM2_REG = value | (`$INSTANCE_NAME`_STRONG_DM2 & `$INSTANCE_NAME`_DATA_MASK);
}


/*******************************************************************************
*  Function Name: `$INSTANCE_NAME`_WrDatNib
********************************************************************************
*
* Summary:
*  Writes a data nibble to the LCD module.
*
* Parameters:
*  nibble:  byte containing nibble in least significant nibble to be written
*           to LCD module.
*
* Return:
*  None.
*
*******************************************************************************/
void `$INSTANCE_NAME`_WrDatNib(uint8 nibble) `=ReentrantKeil($INSTANCE_NAME . "_WrDatNib")`
{
    `$INSTANCE_NAME`_IsReady();

    /* RS shoul be low to select data register */
    `$INSTANCE_NAME`_PORT_DR_REG |= `$INSTANCE_NAME`_RS;
    /* Reset RW for write operation */
    `$INSTANCE_NAME`_PORT_DR_REG &= ~`$INSTANCE_NAME`_RW;

    /* Two following lines of code will provide us with 40ns delay */
    /* Clear data pins */
    `$INSTANCE_NAME`_PORT_DR_REG &= ~`$INSTANCE_NAME`_DATA_MASK;

    /* Write in data, bring E high*/
    `$INSTANCE_NAME`_PORT_DR_REG |= (`$INSTANCE_NAME`_E | (nibble << `$INSTANCE_NAME`_PORT_SHIFT));

    /* Minimum of 230 ns delay */
    CyDelayUs(1u);

    `$INSTANCE_NAME`_PORT_DR_REG &= ~`$INSTANCE_NAME`_E;
}


/*******************************************************************************
*  Function Name: `$INSTANCE_NAME`_WrCntrlNib
********************************************************************************
*
* Summary:
*  Writes a control nibble to the LCD module.
*
* Parameters:
*  nibble:  byte containing nibble in least significant nibble to be written
*           to LCD module.
*
* Return:
*  None.
*
*******************************************************************************/
void `$INSTANCE_NAME`_WrCntrlNib(uint8 nibble) `=ReentrantKeil($INSTANCE_NAME . "_WrCntrlNib")`
{
    /* RS and RW shoul be low to select instruction register and  write operation respectively */
    `$INSTANCE_NAME`_PORT_DR_REG &= ~(`$INSTANCE_NAME`_RS | `$INSTANCE_NAME`_RW);

    /* Two following lines of code will give provide ua with 40ns delay */
    /* Clear data pins */
    `$INSTANCE_NAME`_PORT_DR_REG &= ~`$INSTANCE_NAME`_DATA_MASK;

    /* Write control data and set enable signal */
    `$INSTANCE_NAME`_PORT_DR_REG |= (`$INSTANCE_NAME`_E | (nibble << `$INSTANCE_NAME`_PORT_SHIFT));

    /* Minimum of 230 ns delay */
    CyDelayUs(1u);

    `$INSTANCE_NAME`_PORT_DR_REG &= ~`$INSTANCE_NAME`_E;
}


#if(`$INSTANCE_NAME`_CONVERSION_ROUTINES == 1u)

    /*******************************************************************************
    *  Function Name: `$INSTANCE_NAME`_PrintHexUint8
    ********************************************************************************
    *
    * Summary:
    *  Print a byte as two ASCII characters.
    *
    * Parameters:
    *  value:  The byte to be printed out as ASCII characters.
    *
    * Return:
    *  None.
    *
    *******************************************************************************/
    void `$INSTANCE_NAME`_PrintHexUint8(uint8 value) `=ReentrantKeil($INSTANCE_NAME . "_PrintHexUint8")`
    {
        `$INSTANCE_NAME`_PutChar((char8) `$INSTANCE_NAME`_hex[value >> `$INSTANCE_NAME`_BYTE_UPPER_NIBBLE_SHIFT]);
        `$INSTANCE_NAME`_PutChar((char8) `$INSTANCE_NAME`_hex[value & `$INSTANCE_NAME`_BYTE_LOWER_NIBBLE_MASK]);
    }


    /*******************************************************************************
    *  Function Name: `$INSTANCE_NAME`_PrintHexUint16
    ********************************************************************************
    *
    * Summary:
    *  Print a uint16 as four ASCII characters.
    *
    * Parameters:
    *  value:   The uint16 to be printed out as ASCII characters.
    *
    * Return:
    *  None.
    *
    *******************************************************************************/
    void `$INSTANCE_NAME`_PrintHexUint16(uint16 value) `=ReentrantKeil($INSTANCE_NAME . "_PrintHexUint16")`
    {
        `$INSTANCE_NAME`_PrintHexUint8(value >> `$INSTANCE_NAME`_U16_UPPER_BYTE_SHIFT);
        `$INSTANCE_NAME`_PrintHexUint8(value & `$INSTANCE_NAME`_U16_LOWER_BYTE_MASK);
    }


    /*******************************************************************************
    *  Function Name: `$INSTANCE_NAME`_PrintDecUint16
    ********************************************************************************
    *
    * Summary:
    *  Print an uint32 value as a left-justified decimal value.
    *
    * Parameters:
    *  value:  The byte to be printed out as ASCII characters.
    *
    * Return:
    *  None.
    *
    *******************************************************************************/
    void `$INSTANCE_NAME`_PrintDecUint16(uint16 value) `=ReentrantKeil($INSTANCE_NAME . "_PrintDecUint16")`
    {

        char8 number[`$INSTANCE_NAME`_NUMBER_OF_REMAINDERS];
        char8 temp[`$INSTANCE_NAME`_NUMBER_OF_REMAINDERS];

        uint8 index = 0u;
        uint8 numDigits = 0u;


        /* Load these in reverse order */
        while(value >= `$INSTANCE_NAME`_TEN)
        {
            temp[index] = (value % `$INSTANCE_NAME`_TEN) + '0';
            value /= `$INSTANCE_NAME`_TEN;
            index++;
        }

        temp[index] = (value % `$INSTANCE_NAME`_TEN) + '0';
        numDigits = index;

        /* While index is greater than or equal to zero */
        while (index != 0xFFu)
        {
            number[numDigits - index] = temp[index];
            index--;
        }

        /* Null Termination */
        number[numDigits + 1u] = (char8) 0;

        /* Print out number */
        `$INSTANCE_NAME`_PrintString(number);
    }

#endif /* `$INSTANCE_NAME`_CONVERSION_ROUTINES == 1u */


/* [] END OF FILE */
