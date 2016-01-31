/*******************************************************************************
* File Name: `$INSTANCE_NAME`.c
* Version `$CY_MAJOR_VERSION`.`$CY_MINOR_VERSION`
*
* Description:
*  This file provides source code for the Character LCD component's API.
*
* Note:
* 
*******************************************************************************
* Copyright 2008-2009, Cypress Semiconductor Corporation.  All rights reserved.
* You may use this file only in accordance with the license, terms, conditions, 
* disclaimers, and limitations in the end user license agreement accompanying 
* the software package with which this file was provided.
********************************************************************************/



#include "`$INSTANCE_NAME`.h"


/*******************************************************************************
* Function Name: `$INSTANCE_NAME`_Start
********************************************************************************
* Summary:
*  This method does the prep work necessary to start the LCD Hardware Module.  
*
* Parameters:  
*  void
*
* Return: 
*  void
* 
*******************************************************************************/
void `$INSTANCE_NAME`_Start()
{
    /* INIT CODE */
    /*  Delay 15 ms                 */
    CyDelay(15);
    `$INSTANCE_NAME`_WrCntrlNib(`$INSTANCE_NAME`_DISPLAY_8_BIT_INIT); /*  Selects 8-bit mode  */
    /*  Delay 5 ms                  */
    CyDelay(5);
    `$INSTANCE_NAME`_WrCntrlNib(`$INSTANCE_NAME`_DISPLAY_8_BIT_INIT); /*  Selects 8-bit mode  */
    /*  Delay .15 ms                */
    `$INSTANCE_NAME`_DelayUS(1);
    `$INSTANCE_NAME`_WrCntrlNib(`$INSTANCE_NAME`_DISPLAY_8_BIT_INIT); /*  Selects 8-bit mode  */
    /*  Poll ready Flag             */
    `$INSTANCE_NAME`_WrCntrlNib(`$INSTANCE_NAME`_DISPLAY_4_BIT_INIT); /*  Selects 4-bit mode  */
    /*  Delay 4.5 ms                */  /*  PSoC1 Wait time     */
    CyDelay(5);
    /************************************************************\
    **      Can now write full bytes to LCD in 4-bit mode       **
    \************************************************************/
    `$INSTANCE_NAME`_WriteControl(`$INSTANCE_NAME`_DISPLAY_CURSOR_OFF);      /*  Turn Display, Cursor OFF    */
    /*  Delay 4.5 ms                    */                      /*          Wait                */
    CyDelay(5);
    `$INSTANCE_NAME`_WriteControl(`$INSTANCE_NAME`_CLEAR_DISPLAY);           /*  Clear LCD Screen            */
    /*  Delay 4.5 ms                    */                      /*          Wait                */
    CyDelay(5);
    `$INSTANCE_NAME`_WriteControl(`$INSTANCE_NAME`_CURSOR_AUTO_INCR_ON);     /*  Incr Cursor After Writes    */
    `$INSTANCE_NAME`_WriteControl(`$INSTANCE_NAME`_DISPLAY_CURSOR_ON);       /*  Turn Display, Cursor ON     */
    `$INSTANCE_NAME`_WriteControl(`$INSTANCE_NAME`_DISPLAY_2_LINES_5x10);    /*  2 Lines by 5x10 Characters  */
    `$INSTANCE_NAME`_WriteControl(`$INSTANCE_NAME`_DISPLAY_CURSOR_OFF);      /*  Turn Display, Cursor OFF    */
    `$INSTANCE_NAME`_WriteControl(`$INSTANCE_NAME`_DISPLAY_ON_CURSOR_OFF);   /*  Turn Display ON, Cursor OFF */
    `$INSTANCE_NAME`_WriteControl(`$INSTANCE_NAME`_RESET_CURSOR_POSITION);   /*  Set Cursor to 0,0           */
    /*  Delay 4.5 ms                    */                      /*          Wait                */
    CyDelay(5);
}


/*******************************************************************************
* Function Name: `$INSTANCE_NAME`_Stop
********************************************************************************
* Summary:
*  Turns off the display of the LCD screen.
*
* Parameters:  
*  void
*
* Return: 
*  void
*
*******************************************************************************/
void `$INSTANCE_NAME`_Stop()
{
    /* Calls LCD Off Macro */
    `$INSTANCE_NAME`_DisplayOff();
}


/*******************************************************************************
*  Function Name: `$INSTANCE_NAME`_Position
********************************************************************************
* Summary:
*  Moves active cursor location to a point specified by the input arguments 
*
* Parameters:
*  row:     Specific row of LCD module to be written
*  column:  Column of LCD module to be written
*
* Return: 
*  void
*
* Note:
*  This only applies for LCD displays which use the 2X40 address mode.  
*  This results in Row 2 offset from row one by 0x28.  
*  When there are more than 2 rows, each row must be fewer than 20 characters.
*
*******************************************************************************/
void `$INSTANCE_NAME`_Position(uint8 row, uint8 column)
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
* Summary:
*  Writes a zero terminated string to the LCD. 
*
* Parameters:  
*  string:  pointer to head of char8 array to be written to the LCD module
*
* Return: 
*  void
*
*******************************************************************************/
void `$INSTANCE_NAME`_PrintString(char8 * string)
{
    uint8 index_u8 = 1;
    char8 current = *string;
    /* Until null is reached, print next character */
    while(current != (char8)'\0')
    {
        `$INSTANCE_NAME`_WriteData(current);
        current = *(string+index_u8);
        index_u8++;
    }
}


/*******************************************************************************
*  Function Name: `$INSTANCE_NAME`_PutChar
********************************************************************************
* Summary:
*  Writes a single character to the current cursor position of the LCD module.
*  Custom character names (`$INTANCE_NAME`_CUSTOM_0 through 
*  `$INTANCE_NAME`_CUSTOM_7) are acceptable as inputs.  
*
* Parameters:
*  character:   character to be written to the LCD 
*
* Return:
*  void
*
*******************************************************************************/
void `$INSTANCE_NAME`_PutChar(char8 character)
{
    `$INSTANCE_NAME`_WriteData(character);
}


/*******************************************************************************
*  Function Name: `$INSTANCE_NAME`_WriteData
********************************************************************************
* Summary:
*  Writes a data byte to the LCD module's Data Display RAM.
*
* Parameters:
*  dByte:   byte to be written to LCD module.
*
* Return:
*  void
*
*******************************************************************************/
void `$INSTANCE_NAME`_WriteData(uint8 dByte)
{
    uint8 nibble;
    `$INSTANCE_NAME`_IsReady();
    nibble = dByte >> `$INSTANCE_NAME`_NIBBLE_SHIFT;
    nibble &= `$INSTANCE_NAME`_DATA_MASK;
    /* Write high nibble */
    `$INSTANCE_NAME`_WrDatNib(nibble);
    
    nibble = dByte & `$INSTANCE_NAME`_DATA_MASK;
    /* Write low nibble */
    `$INSTANCE_NAME`_WrDatNib(nibble);
}


/*******************************************************************************
*  Function Name: `$INSTANCE_NAME`_WriteControl
********************************************************************************
* Summary:
*  Writes a command byte to the LCD module.
*
* Parameters:
*  cByte:   byte to be written to LCD module.
*
* Return: 
*  void
*
*******************************************************************************/
void `$INSTANCE_NAME`_WriteControl(uint8 cByte)
{
    uint8 nibble = cByte >> `$INSTANCE_NAME`_NIBBLE_SHIFT;
    `$INSTANCE_NAME`_IsReady();
    nibble &= `$INSTANCE_NAME`_DATA_MASK;
    /*  WrCntrlNib(High Nibble) */
    `$INSTANCE_NAME`_WrCntrlNib(nibble);
    
    nibble = cByte & `$INSTANCE_NAME`_DATA_MASK;
    /*  WrCntrlNib(Low Nibble)  */
    `$INSTANCE_NAME`_WrCntrlNib(nibble);
}


/*******************************************************************************
* Function Name: `$INSTANCE_NAME`_IsReady
********************************************************************************
* Summary:
*  Polls LCD until the ready bit is set.
*
* Parameters:
*  void
*
* Return: 
*  void
*
* Note:
*  Changes pins to High-Z
*
*******************************************************************************/
void `$INSTANCE_NAME`_IsReady()
{
    /*  Change Port to High-Z Status on data pins */
    uint8 value = 0;
    uint8 readyValue = 0;
    
    /* Clear the LCD port*/
    `$INSTANCE_NAME`_PORT_DR = 0x00u;
        
    /*  Mask off data pins to clear old values out  */
    value = `$INSTANCE_NAME`_PORT_DM_0 & ~(`$INSTANCE_NAME`_DATA_MASK << `$INSTANCE_NAME`_PORT_SHIFT);
    /* Load in high Z values for data pins, others unchanged */
    `$INSTANCE_NAME`_PORT_DM_0 = value | ((`$INSTANCE_NAME`_HIGH_Z_DM_0 & `$INSTANCE_NAME`_DATA_MASK) << `$INSTANCE_NAME`_PORT_SHIFT); 
    
    /*  Mask off data pins to clear old values out  */
    value = `$INSTANCE_NAME`_PORT_DM_1 & ~(`$INSTANCE_NAME`_DATA_MASK << `$INSTANCE_NAME`_PORT_SHIFT);
    /* Load in high Z values for data pins, others unchanged */
    `$INSTANCE_NAME`_PORT_DM_1 = value | ((`$INSTANCE_NAME`_HIGH_Z_DM_1 & `$INSTANCE_NAME`_DATA_MASK) << `$INSTANCE_NAME`_PORT_SHIFT); 
    
    /*  Mask off data pins to clear old values out  */
    value = `$INSTANCE_NAME`_PORT_DM_2 & ~(`$INSTANCE_NAME`_DATA_MASK << `$INSTANCE_NAME`_PORT_SHIFT);
    /* Load in high Z values for data pins, others unchanged */
    `$INSTANCE_NAME`_PORT_DM_2 = value | ((`$INSTANCE_NAME`_HIGH_Z_DM_2 & `$INSTANCE_NAME`_DATA_MASK) << `$INSTANCE_NAME`_PORT_SHIFT); 

    /*  Set R/W high to read, could be next to do while loop, delay needed then */
    `$INSTANCE_NAME`_PORT_DR |= `$INSTANCE_NAME`_RW;
    `$INSTANCE_NAME`_DelayUS(1);
    
    do
    {
        /*  Set E high  */
        `$INSTANCE_NAME`_PORT_DR |= `$INSTANCE_NAME`_E << `$INSTANCE_NAME`_PORT_SHIFT;
        /* Delay the setup time for data pins */
        value ++;
        value --;
        value ++;
        value --;
        
        readyValue = `$INSTANCE_NAME`_PORT_PS << `$INSTANCE_NAME`_PORT_SHIFT; 
        
        `$INSTANCE_NAME`_PORT_DR &= ~`$INSTANCE_NAME`_E << `$INSTANCE_NAME`_PORT_SHIFT;
        
        value ++;
        value --;
        value ++;
        value --;
                
        /*  Set E high  */
        `$INSTANCE_NAME`_PORT_DR |= `$INSTANCE_NAME`_E << `$INSTANCE_NAME`_PORT_SHIFT;
        /* Delay the setup time for data pins */
        value ++;
        value --;
        value ++;
        value --;
        
        `$INSTANCE_NAME`_PORT_DR &= ~`$INSTANCE_NAME`_E << `$INSTANCE_NAME`_PORT_SHIFT;
        /*  Repeat until bit 4 is zero. */
        readyValue &= `$INSTANCE_NAME`_READY_BIT;
    } while (readyValue != 0);

    /*  Set R/W low to write, could be next to do while loop, delay needed then */
    `$INSTANCE_NAME`_PORT_DR &= ~`$INSTANCE_NAME`_RW; 
    `$INSTANCE_NAME`_DelayUS(1);
    
    /*  Change Port to Output (Strong) on data pins */
    /*  Mask off data pins to clear high z values out  */
    value = `$INSTANCE_NAME`_PORT_DM_0 & ~(`$INSTANCE_NAME`_DATA_MASK << `$INSTANCE_NAME`_PORT_SHIFT);
    /* Load in high Z values for data pins, others unchanged */
    `$INSTANCE_NAME`_PORT_DM_0 = value | ((`$INSTANCE_NAME`_STRONG_DM_0 & `$INSTANCE_NAME`_DATA_MASK) << `$INSTANCE_NAME`_PORT_SHIFT); 
    
    /*  Mask off data pins to clear high z values out  */
    value = `$INSTANCE_NAME`_PORT_DM_1 & ~(`$INSTANCE_NAME`_DATA_MASK << `$INSTANCE_NAME`_PORT_SHIFT);
    /* Load in high Z values for data pins, others unchanged */
    `$INSTANCE_NAME`_PORT_DM_1 = value | ((`$INSTANCE_NAME`_STRONG_DM_1 & `$INSTANCE_NAME`_DATA_MASK) << `$INSTANCE_NAME`_PORT_SHIFT); 
    
    /*  Mask off data pins to clear high z values out  */
    value = `$INSTANCE_NAME`_PORT_DM_2 & ~(`$INSTANCE_NAME`_DATA_MASK << `$INSTANCE_NAME`_PORT_SHIFT);
    /* Load in high Z values for data pins, others unchanged */
    `$INSTANCE_NAME`_PORT_DM_2 = value | ((`$INSTANCE_NAME`_STRONG_DM_2 & `$INSTANCE_NAME`_DATA_MASK) << `$INSTANCE_NAME`_PORT_SHIFT); 
}


/* Non API Functions */

/*******************************************************************************
*  Function Name: `$INSTANCE_NAME`_WrDatNib
********************************************************************************
* Summary:
*  Writes a data nibble to the LCD module.
*
* Parameters:  
*  nibble:  byte containing nibble in least significant nibble to be written 
*           to LCD module.
*
* Return: 
*  void
*
*******************************************************************************/
void `$INSTANCE_NAME`_WrDatNib(uint8 nibble)
{
    uint8 dataNibble = nibble & `$INSTANCE_NAME`_DATA_MASK;
    
    /*  RS High (DATA) 140ns delay ahead of raising E          */
    `$INSTANCE_NAME`_DelayUS(1);
    `$INSTANCE_NAME`_PORT_DR |= `$INSTANCE_NAME`_RS << `$INSTANCE_NAME`_PORT_SHIFT;
    `$INSTANCE_NAME`_PORT_DR &= ~(`$INSTANCE_NAME`_E | `$INSTANCE_NAME`_DATA_MASK) << `$INSTANCE_NAME`_PORT_SHIFT;
    /*  Assign Data To Port (Watch out for extra Pin Value) and bring E High */
    `$INSTANCE_NAME`_PORT_DR |= (`$INSTANCE_NAME`_E | dataNibble) << `$INSTANCE_NAME`_PORT_SHIFT;/* Write in data, bring E high*/
    /*  Minimum of 450 ns delay (30 Instructions at 66 MHz), Bring E low */
    `$INSTANCE_NAME`_DelayUS(1);
    `$INSTANCE_NAME`_PORT_DR &= ~`$INSTANCE_NAME`_E << `$INSTANCE_NAME`_PORT_SHIFT;
}


/*******************************************************************************
*  Function Name: `$INSTANCE_NAME`_WrCntrlNib
********************************************************************************
* Summary:
*  Writes a control nibble to the LCD module.
*
* Parameters:  
*  nibble:  byte containing nibble in least significant nibble to be written 
*           to LCD module.
*
* Return: 
*  void
*
*******************************************************************************/
void `$INSTANCE_NAME`_WrCntrlNib(uint8 nibble)
{
    uint8 dataNibble = nibble & `$INSTANCE_NAME`_DATA_MASK;

    /* E should already be low, clear data and bring RS low */
    `$INSTANCE_NAME`_PORT_DR &= ~(`$INSTANCE_NAME`_E | `$INSTANCE_NAME`_RS | `$INSTANCE_NAME`_DATA_MASK) << `$INSTANCE_NAME`_PORT_SHIFT ;
    /*  Delay 140 ns before bringing E high  (10 instructions at 66MHz), Write in Data */
    `$INSTANCE_NAME`_DelayUS(1);
    `$INSTANCE_NAME`_PORT_DR |= (`$INSTANCE_NAME`_E | dataNibble) << `$INSTANCE_NAME`_PORT_SHIFT ;
    /*  Minimum of 450 ns delay (30 Instructions at 66 MHz), Bring E low */
    `$INSTANCE_NAME`_DelayUS(1);
    `$INSTANCE_NAME`_PORT_DR &= ~`$INSTANCE_NAME`_E << `$INSTANCE_NAME`_PORT_SHIFT ;
}


/*******************************************************************************
*  Function Name: `$INSTANCE_NAME`_Delay
********************************************************************************
* Summary:
*  Delays for 15 micro second blocks.  
*  
* Parameters:  
*  blocks:  number of 15us segments to delay for. 
*
* Return:
*  void
*
* Note:
*  With blocks = 0 , there is an overhead of 6 to 45us.  That is, at slow 
*  clock speeds, the minimum delay is 45us.  Calling this function with 
*  blocks = 3 or blocks = 1 will result in the same delay of approximately 45us. 
*  With blocks = 4, the delay would be 60us.
*
*******************************************************************************/
void `$INSTANCE_NAME`_DelayUS(uint8 blocks)
{
    #define `$INSTANCE_NAME`_LOOP_CYCLES 96
    #define `$INSTANCE_NAME`_OVERHEAD 150

    uint32 cycles;
    uint16 count16;

    cycles = BCLK__BUS_CLK__HZ;
    cycles /= 1 + CY_GET_XTND_REG8(CYDEV_SFR_USER_CPUCLK_DIV);
    
    /* cycles per 10us */
    cycles <<= 16;

    count16 = cycles * blocks;
    if(count16 > `$INSTANCE_NAME`_OVERHEAD)
    {
        count16 -= `$INSTANCE_NAME`_OVERHEAD;
        while(count16 > `$INSTANCE_NAME`_LOOP_CYCLES)
        {
            count16 -= `$INSTANCE_NAME`_LOOP_CYCLES;
        }
    }
}
`$ConversionRoutines_API_GEN`

/* [] END OF FILE */