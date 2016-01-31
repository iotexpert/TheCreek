/*******************************************************************************
* File Name: `$INSTANCE_NAME`.c
* Version `$CY_MAJOR_VERSION`.`$CY_MINOR_VERSION`
*
* Description:
*  This file provides the API source code for the Static Segment LCD component.
*
* Note:
*
********************************************************************************
* Copyright 2008-2010, Cypress Semiconductor Corporation.  All rights reserved.
* You may use this file only in accordance with the license, terms, conditions,
* disclaimers, and limitations in the end user license agreement accompanying
* the software package with which this file was provided.
*******************************************************************************/

#include "`$INSTANCE_NAME`.h"
#include <`$INSTANCE_NAME`_InClock.h>
#include <`$INSTANCE_NAME`_LCD_ISR.h>
#include <`$INSTANCE_NAME``[Lcd_Dma]`dma.h>

uint8  `$INSTANCE_NAME`_DmaConfigure(void);
void   `$INSTANCE_NAME`_DmaDispose(void);


/* Look-up tables for different kinds of displays */
#ifdef `$INSTANCE_NAME`_7SEG

    const uint8 CYCODE `$INSTANCE_NAME`_7SegDigits[] = \
    /* '0'   '1'    '2'    '3'    '4'    '5'    '6'    '7' */
    {0x3fu, 0x06u, 0x5bu, 0x4fu, 0x66u, 0x6du, 0x7du, 0x07u, \
        /* '8'  '9'    'A'    'B'    'C'    'D'    'E'    'F'   null */
        0x7fu, 0x6fu, 0x77u, 0x7cu, 0x39u, 0x5eu, 0x79u, 0x71u, 0x00u};

#endif /* `$INSTANCE_NAME`_7SEG */

#ifdef ALPHANUMERIC

    #ifdef `$INSTANCE_NAME`_14SEG
    
        const uint16 CYCODE `$INSTANCE_NAME`_14SegChars[] = {
        /*------------------------------------------------------------*/
        /*                           Blank                            */
        /*------------------------------------------------------------*/
        0x0000u,0x0000u,0x0000u,0x0000u,0x0000u,0x0000u,0x0000u,0x0000u,
        0x0000u,0x0000u,0x0000u,0x0000u,0x0000u,0x0000u,0x0000u,0x0000u,
        0x0000u,0x0000u,0x0000u,0x0000u,0x0000u,0x0000u,0x0000u,0x0000u,
        0x0000u,0x0000u,0x0000u,0x0000u,0x0000u,0x0000u,0x0000u,0x0000u,
        /*------------------------------------------------------------*/
        /*         !       "       #       $       %       &       '  */
        /*------------------------------------------------------------*/
        0x0000u,0x0006u,0x0120u,0x3fffu,0x156du,0x2ee4u,0x2a8du,0x0200u,
        /*------------------------------------------------------------*/
        /* (       )       *       +       ,       -       .       /  */
        /*------------------------------------------------------------*/
        0x0a00u,0x2080u,0x3fc0u,0x1540u,0x2000u,0x0440u,0x1058u,0x2200u,
        /*------------------------------------------------------------*/
        /* 0       1       2       3       4       5       6       7  */
        /*------------------------------------------------------------*/
        0x223fu,0x0206u,0x045bu,0x040fu,0x0466u,0x0869u,0x047du,0x1201u,
        /*------------------------------------------------------------*/
        /* 8       9       :       ;       <       =       >       ?  */
        /*------------------------------------------------------------*/
        0x047fu,0x046fu,0x2200u,0x1100u,0x2100u,0x0a00u,0x0448u,0x2080u,
        /*------------------------------------------------------------*/
        /* @       A       B       C       D       E       F       G  */
        /*------------------------------------------------------------*/
        0x053bu,0x0477u,0x150fu,0x0039u,0x110Fu,0x0079u,0x0071u,0x043du,
        /*------------------------------------------------------------*/
        /* H       I       J       K       L       M       N       O  */
        /*------------------------------------------------------------*/
        0x0476u,0x1100u,0x001eu,0x0a70u,0x0038u,0x02b6u,0x08b6u,0x003fu,
        /*------------------------------------------------------------*/
        /* P       Q       R       S       T       U       V       W  */
        /*------------------------------------------------------------*/
        0x0473u,0x083fu,0x0C73u,0x046du,0x1101u,0x003eu,0x2230u,0x2836u,
        /*------------------------------------------------------------*/
        /* X       Y       Z       [       \       ]       ^       _  */
        /*------------------------------------------------------------*/
        0x2a80u,0x1280u,0x2209u,0x0039u,0x0880u,0x000fu,0x2203u,0x2008u,
        /*------------------------------------------------------------*/
        /* @       a       b       c       d       e       f       g  */
        /*------------------------------------------------------------*/
        0x0080u,0x0477u,0x150fu,0x0039u,0x110Fu,0x0079u,0x0071u,0x043du,
        /*------------------------------------------------------------*/
        /* h       i       j       k       l       m       n       o  */
        /*------------------------------------------------------------*/
        0x0476u,0x1100u,0x001eu,0x0a70u,0x0038u,0x02b6u,0x08b6u,0x003fu,
        /*------------------------------------------------------------*/
        /* p       q       r       s       t       u       v       w  */
        /*------------------------------------------------------------*/
        0x0473u,0x083fu,0x0C73u,0x046du,0x1101u,0x003eu,0x2230u,0x2836u,
        /*------------------------------------------------------------*/
        /* x       y       z       {       |       }        ~         */
        /*------------------------------------------------------------*/
        0x2a80u,0x1280u,0x2209u,0x0e00u,0x1100u,0x20C0u,0x0452u,0x003fu};
    
    #endif /* `$INSTANCE_NAME`_14SEG */

    #ifdef `$INSTANCE_NAME`_16SEG
    
        const uint16 CYCODE `$INSTANCE_NAME`_16SegChars[] = {
        /*------------------------------------------------------------*/
        /*                           Blank                            */
        /*------------------------------------------------------------*/
        0x0000u,0x0000u,0x0000u,0x0000u,0x0000u,0x0000u,0x0000u,0x0000u,
        0x0000u,0x0000u,0x0000u,0x0000u,0x0000u,0x0000u,0x0000u,0x0000u,
        0x0000u,0x0000u,0x0000u,0x0000u,0x0000u,0x0000u,0x0000u,0x0000u,
        0x0000u,0x0000u,0x0000u,0x0000u,0x0000u,0x0000u,0x0000u,0x0000u,
        /*------------------------------------------------------------*/
        /*         !       "       #       $       %       &       '  */
        /*------------------------------------------------------------*/
        0x0000u,0x000cu,0x0480u,0xffffu,0x55bbu,0xdd99u,0xaa3bu,0x0800u,
        /*------------------------------------------------------------*/
        /* (       )       *       +       ,       -       .       /  */
        /*------------------------------------------------------------*/
        0x2800u,0x8200u,0xff00u,0x5500u,0x8000u,0x1100u,0x4160u,0x8800u,
        /*------------------------------------------------------------*/
        /* 0       1       2       3       4       5       6       7  */
        /*------------------------------------------------------------*/
        0x88ffu,0x080cu,0x1177u,0x103fu,0x118cu,0x21b3u,0x11fbu,0x4803u,
        /*------------------------------------------------------------*/
        /* 8       9       :       ;       <       =       >       ?  */
        /*------------------------------------------------------------*/
        0x11ffu,0x11bfu,0x4400u,0x8400u,0x2800u,0x1130u,0x8200u,0x5006u,
        /*------------------------------------------------------------*/
        /* @       A       B       C       D       E       F       G  */
        /*------------------------------------------------------------*/
        0x14f7u,0x11cfu,0x543fu,0x00f3u,0x443fu,0x01f3u,0x01c3u,0x10fbu,
        /*------------------------------------------------------------*/
        /* H       I       J       K       L       M       N       O  */
        /*------------------------------------------------------------*/
        0x11ccu,0x4400u,0x007eu,0x29c0u,0x00f0u,0x0accu,0x22ccu,0x00ffu,
        /*------------------------------------------------------------*/
        /* P       Q       R       S       T       U       V       W  */
        /*------------------------------------------------------------*/
        0x11c7u,0x20ffu,0x31c7u,0x11bbu,0x4403u,0x00fcu,0x88c0u,0xa0ccu,
        /*------------------------------------------------------------*/
        /* X       Y       Z       [       \       ]       ^       _  */
        /*------------------------------------------------------------*/
        0xaa00u,0x4A00u,0x8833u,0x4412u,0x2200u,0x4421u,0x8806u,0x0030u,
        /*------------------------------------------------------------*/
        /* @       a       b       c       d       e       f       g  */
        /*------------------------------------------------------------*/
        0x0200u,0x11cfu,0x543fu,0x00f3u,0x443fu,0x01f3u,0x01c3u,0x10fbu,
        /*------------------------------------------------------------*/
        /* h       i       j       k       l       m       n       o  */
        /*------------------------------------------------------------*/
        0x11ccu,0x4400u,0x007eu,0x29c0u,0x00f0u,0x0accu,0x22ccu,0x00ffu,
        /*------------------------------------------------------------*/
        /* p       q       r       s       t       u       v       w  */
        /*------------------------------------------------------------*/
        0x11c7u,0x20ffu,0x31c7u,0x11bbu,0x4403u,0x00fcu,0x88c0u,0xa0ccu,
        /*------------------------------------------------------------*/
        /* x       y       z       {       |       }        ~         */
        /*------------------------------------------------------------*/
        0xaa00u,0x4A00u,0x8833u,0x3800u,0x4400u,0x8300u,0x1144u,0x0000u};
    
    #endif /* `$INSTANCE_NAME`_16SEG */

#endif /* ALPHANUMERIC */

/* Frame Buffer */
static uint8 `$INSTANCE_NAME`_buffer[`$INSTANCE_NAME`_BUFFER_LENGTH] ;

/* Array of common port TDs */
static uint8 `$INSTANCE_NAME`_lcdTd[`$INSTANCE_NAME`_LCD_TD_SIZE * `$INSTANCE_NAME`_COMM_NUM];
/* DMA channel dedicated for SegLCD commons */
static uint8 `$INSTANCE_NAME`_lcdChannel;

static uint8 `$INSTANCE_NAME`_termOut = (`$INSTANCE_NAME`_Lcd_Dma__TERMOUT0_EN ? TD_TERMOUT0_EN : 0u) | \
                                 (`$INSTANCE_NAME`_Lcd_Dma__TERMOUT1_EN ? TD_TERMOUT1_EN : 0u);

uint8 `$INSTANCE_NAME`_initVar = 0u;

uint8 `$INSTANCE_NAME`_enableState = 0u;

/* Start of customizer generated code */
`$writerCVariables`


/*******************************************************************************
* Function Name: `$INSTANCE_NAME`_Init
********************************************************************************
*
* Summary:
*  Configures every-frame ISR and initializes a Frame Buffer.
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
void `$INSTANCE_NAME`_Init(void)
{
    `$INSTANCE_NAME`_ClearDisplay();
    
    /*ISR initialization */
    CyIntDisable(`$INSTANCE_NAME`_ISR_NUMBER);

    /* Set the ISR to point to the RTC_SUT_isr Interrupt. */
    CyIntSetVector(`$INSTANCE_NAME`_ISR_NUMBER, `$INSTANCE_NAME`_ISR);

    /* Set the priority. */
    CyIntSetPriority(`$INSTANCE_NAME`_ISR_NUMBER, `$INSTANCE_NAME`_ISR_PRIORITY);
}


/*******************************************************************************
* Function Name: `$INSTANCE_NAME`_Enable
********************************************************************************
*
* Summary:
*  Enables clock generation for the component.
*
* Parameters:
*  None.
*
* Global variables:
*  `$INSTANCE_NAME`_enableState: stores the state of the component.
*
* Reentrant:
*  No.
*
*******************************************************************************/
uint8 `$INSTANCE_NAME`_Enable(void)
{
    uint8 status = CYRET_LOCKED;
    
    status = `$INSTANCE_NAME`_DmaConfigure();

    `$INSTANCE_NAME``[InClock]`Enable();
    
    `$INSTANCE_NAME`_enableState = 1u;
    
    return(status);
}


/*******************************************************************************
* Function Name: `$INSTANCE_NAME`_DmaConfigure
********************************************************************************
*
* Summary:
*  Configures DMA to transfer data from the frame buffer to port alliased 
*  memory.
*
* Parameters:
*  None.
*
* Return:
*  Status one of standard status for PSoC3 Component
*       CYRET_SUCCESS - Function completed successfully.
*       CYRET_LOCKED - The object was locked, already in use. Some of TDs or
*                      a channel already in use.
*
* Reentrant:
*  No.
*
*******************************************************************************/
uint8 `$INSTANCE_NAME`_DmaConfigure(void)
{
    uint8   status = CYRET_LOCKED;
    uint8   i = 0u, j, errorCnt = 0u;
    uint32  srcAddr, dstAddr;
    uint8   comTdCount;
    
    /* Initialization of TDs for the common lines port. 
    */
    srcAddr = `$INSTANCE_NAME`_DMA_ADDRESS_MASK & ((uint32) `$INSTANCE_NAME`_buffer);
    dstAddr = (dstAddr | 0xFFFFFFFFu) & `$INSTANCE_NAME`_ALIASED_AREA_PTR;        
    
    `$INSTANCE_NAME`_lcdChannel = `$INSTANCE_NAME``[Lcd_Dma]`DmaInitialize(0u, 0u, HI16(srcAddr), HI16(dstAddr));

    if(`$INSTANCE_NAME`_lcdChannel == DMA_INVALID_CHANNEL)
    {
        errorCnt++;
    }
       
    for(j = 0u; j < `$INSTANCE_NAME`_COMM_NUM; j++)
    {
        for(i = 0u; i < `$INSTANCE_NAME`_LCD_TD_SIZE; i++)
        {
            `$INSTANCE_NAME`_lcdTd[j * `$INSTANCE_NAME`_LCD_TD_SIZE + i] = CyDmaTdAllocate();       
            if(`$INSTANCE_NAME`_lcdTd[j * `$INSTANCE_NAME`_LCD_TD_SIZE + i] == DMA_INVALID_TD)
            {
                errorCnt++;
            }
        }
    }

    comTdCount = (`$INSTANCE_NAME`_COMM_NUM * `$INSTANCE_NAME`_LCD_TD_SIZE);
    
    for(j = 0u; j < `$INSTANCE_NAME`_COMM_NUM; j++)
    {
        for(i = 0u; i < `$INSTANCE_NAME`_LCD_TD_SIZE; i++)
        {
            /* The last TD need to point to TD[0] */
            if((j * `$INSTANCE_NAME`_LCD_TD_SIZE + i) == (comTdCount - 1u))
            {
                CyDmaTdSetConfiguration(`$INSTANCE_NAME`_lcdTd[j * `$INSTANCE_NAME`_LCD_TD_SIZE + i], \
                                        `$INSTANCE_NAME`_StSegLcdPort_DMA_TD_PROTO_BLOCK[i].length, \
                                        `$INSTANCE_NAME`_lcdTd[0u], \
                                        TD_INC_DST_ADR | TD_INC_SRC_ADR | `$INSTANCE_NAME`_termOut);
            }
            else if(i == (`$INSTANCE_NAME`_LCD_TD_SIZE - 1u))
            {
                CyDmaTdSetConfiguration(`$INSTANCE_NAME`_lcdTd[j * `$INSTANCE_NAME`_LCD_TD_SIZE + i], \
                                        `$INSTANCE_NAME`_StSegLcdPort_DMA_TD_PROTO_BLOCK[i].length, \
                                        `$INSTANCE_NAME`_lcdTd[j * `$INSTANCE_NAME`_LCD_TD_SIZE + i + 1u], \
                                        TD_INC_DST_ADR | TD_INC_SRC_ADR | `$INSTANCE_NAME`_termOut);
            }
            else
            {
                CyDmaTdSetConfiguration(`$INSTANCE_NAME`_lcdTd[j * `$INSTANCE_NAME`_LCD_TD_SIZE + i], \
                                        `$INSTANCE_NAME`_StSegLcdPort_DMA_TD_PROTO_BLOCK[i].length, \
                                        `$INSTANCE_NAME`_lcdTd[j * `$INSTANCE_NAME`_LCD_TD_SIZE + i + 1u], \
                                        TD_INC_DST_ADR | TD_INC_SRC_ADR | TD_AUTO_EXEC_NEXT);
            }
            CyDmaTdSetAddress(`$INSTANCE_NAME`_lcdTd[j * `$INSTANCE_NAME`_LCD_TD_SIZE + i], \
                              LO16(srcAddr) + j * `$INSTANCE_NAME`_ROW_LENGTH + \
                              `$INSTANCE_NAME`_StSegLcdPort_DMA_TD_PROTO_BLOCK[i].offset, \
                              LO16(dstAddr) + `$INSTANCE_NAME`_StSegLcdPort_DMA_TD_PROTO_BLOCK[i].offset);
        }
    }
    
    if(errorCnt == 0u)
    {
        CyDmaChSetInitialTd(`$INSTANCE_NAME`_lcdChannel, `$INSTANCE_NAME`_lcdTd[0u]);
        CyDmaChEnable(`$INSTANCE_NAME`_lcdChannel, 1u);
        status = CYRET_SUCCESS;
    }
    
    return(status);
}


/*******************************************************************************
* Function Name: `$INSTANCE_NAME`_DmaDispose
********************************************************************************
*
* Summary:
*  Releases transaction descriptors and DMA channel previously used by the 
*  component.
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
void `$INSTANCE_NAME`_DmaDispose(void)
{
    uint8 i;

    /* Release all allocated TDs */
    for(i = 0u; i < (`$INSTANCE_NAME`_LCD_TD_SIZE * `$INSTANCE_NAME`_COMM_NUM); i++)
    {
        CyDmaTdFree(`$INSTANCE_NAME`_lcdTd[i]);
    }
    /* Release DMA handle */
    `$INSTANCE_NAME``[Lcd_Dma]`DmaRelease();
        
}


/*******************************************************************************
* Function Name: `$INSTANCE_NAME`_Start
********************************************************************************
*
* Summary:
*  Starts the LCD component , DMA channels and initializes the frame buffer.
*
* Parameters:
*  `$INSTANCE_NAME`_initVar - Global variable.
*
* Return:
*  Status one of standard status for PSoC3 Component:
*       CYRET_SUCCESS - Function completed successfully.
*       CYRET_LOCKED - The object was locked, already in use. Some of TDs or
*                      channel already in use.
*
* Global variables:
*  `$INSTANCE_NAME`_initVar : used to check initial configuration, modified on 
*   first function call.
*
* Reentrant:
*  No.
*
*******************************************************************************/
uint8 `$INSTANCE_NAME`_Start()
{
    uint8 status = CYRET_LOCKED;
    
    /* If not Initialized then initialize all required hardware and software */
    if(`$INSTANCE_NAME`_initVar == 0u)
    {
        `$INSTANCE_NAME`_initVar = 1u;
        `$INSTANCE_NAME`_Init();
    }
     
    status = `$INSTANCE_NAME`_Enable();
    
    return(status);
}


/*******************************************************************************
* Function Name: `$INSTANCE_NAME`_Stop
********************************************************************************
*
* Summary:
*  Disables the LCD component and disables any required interrupts and DMA 
*  channels. Automatically blanks the display to avoid damage from DC offsets. 
*  Does not clear the frame buffer.
*
* Parameters:
*  None.
*
* Global variables:
*  `$INSTANCE_NAME`_enableState: stores the state of the component.
*
* Return:
*  None.
*
*******************************************************************************/
void `$INSTANCE_NAME`_Stop()
{
    CyIntDisable(`$INSTANCE_NAME`_ISR_NUMBER);
    `$INSTANCE_NAME``[InClock]`Disable();
    `$INSTANCE_NAME`_enableState = 0u;
}


/*******************************************************************************
* Function Name: `$INSTANCE_NAME`_EnableInt
********************************************************************************
*
* Summary:
*  Description: Enables the LCD interrupt/s. Not required if `$INSTANCE_NAME`
*  _Start called.
*
* Parameters:
*  None.
*
* Return:
*  None.
*
*******************************************************************************/
void `$INSTANCE_NAME`_EnableInt() `=ReentrantKeil($INSTANCE_NAME . "_EnableInt")`
{
    CyIntEnable(`$INSTANCE_NAME`_ISR_NUMBER);
}


/*******************************************************************************
* Function Name: `$INSTANCE_NAME`_DisableInt
********************************************************************************
* Summary:
*  Description: Disables the LCD interrupt. Not required if `$INSTANCE_NAME`
*  _Stop called
*
* Parameters:
*  None.
*
* Return:
*  None.
*
*******************************************************************************/
void `$INSTANCE_NAME`_DisableInt() `=ReentrantKeil($INSTANCE_NAME . "_DisableInt")`
{
   CyIntDisable(`$INSTANCE_NAME`_ISR_NUMBER);
}


/*******************************************************************************
* Function Name: `$INSTANCE_NAME`_ClearDisplay
********************************************************************************
*
* Summary:
*  This function clears the display RAM of the frame buffer.
*
* Parameters:
*  None.
*
* Return:
*  `$INSTANCE_NAME`_buffer[] - Global variable.
*
* Reentrant:
*  No.
*
*******************************************************************************/
void `$INSTANCE_NAME`_ClearDisplay()
{
    uint16 i;

    /* Clear entire frame buffer to all zeroes */
    for(i = 0u ; i < `$INSTANCE_NAME`_BUFFER_LENGTH; i++)
    {
        if(i < (`$INSTANCE_NAME`_BUFFER_LENGTH / 2u))
        {
            `$INSTANCE_NAME`_buffer[i] = 0x00u;
        }
        else
        {
            `$INSTANCE_NAME`_buffer[i] = 0xFFu;
        }    
    }

    /* Reinitialize the commons */
    (void) `$INSTANCE_NAME`_WRITE_PIXEL(`$INSTANCE_NAME`_commons[0u], `$INSTANCE_NAME`_PIXEL_STATE_OFF);
}


/*******************************************************************************
* Function Name: `$INSTANCE_NAME`_WritePixel
********************************************************************************
*
* Summary:
*  This function sets or clears a pixel based on pixelState
*  in a large frame buffer. The Pixel is addressed by a packed
*  number. Only included in 'large' pixel address mode compliant
*  configurations with more than 4 common drivers and/or more than
*  64 consecutive segment drivers. User code can also directly write
*  the frame buffer RAM to create optimized interactions.
*
* Parameters:
*  pixelNumber: is the packed number that points to the pixels
*  location in the frame buffer. The lowest three bits in the LSB
*  low nibble are the bit position in the byte, the LSB upper
*  nibble (4 bits) is the byte address in the multiplex Row and
*  the MSB low nibble (4 bits) is the multiplex Row number.
*  pixelState : 0 for pixel off,1 for pixel on, 2 for pixel invert.
*
* Return:
*  Status returns the standardized return value for pass
*  or fail on a range check of the byte address and multiplex Row
*  number:
*       CYRET_SUCCESS - Function completed successfully.
*       CYRET_BAD_PARAM - One of the parameters is invalid.
*
* Reentrant:
*  No.
*
*******************************************************************************/
uint8 `$INSTANCE_NAME`_WritePixel(uint8 pixelNumber, uint8 pixelState)
{
    uint16 byteAddr, bitAddr, row;
    uint8 mask;
    uint8 status;

    /* Let the User know he is entered wrong parameter
    * which is pixel state greater than 2 or unconnected pixel 
    */
    if((pixelState > `$INSTANCE_NAME`_PIXEL_STATE_INVERT) || (pixelNumber == `$INSTANCE_NAME`_NOT_CON)) 
    {
        status = CYRET_BAD_PARAM;
    }
    else
    {
        /* Extract bit position in the byte*/
        bitAddr = (pixelNumber & `$INSTANCE_NAME`_BIT_MASK);
        
        /* Extract byte position in the Row*/
        byteAddr = (pixelNumber & `$INSTANCE_NAME`_BYTE_MASK) >> `$INSTANCE_NAME`_BYTE_SHIFT;
        
        /* Extract Row position*/
        row = (pixelNumber & `$INSTANCE_NAME`_ROW_MASK) >> `$INSTANCE_NAME`_ROW_SHIFT;

        if(pixelState == `$INSTANCE_NAME`_PIXEL_STATE_INVERT)
        {
            /* Invert actual pixel state */
            pixelState = !(`$INSTANCE_NAME`_ReadPixel(pixelNumber));
        }

        mask = ~(`$INSTANCE_NAME`_PIXEL_STATE_ON << bitAddr);
        
        /* Set pixel in first frame buffer line */
        `$INSTANCE_NAME`_buffer[row * `$INSTANCE_NAME`_ROW_BYTE_LENGTH + byteAddr] = \
            (`$INSTANCE_NAME`_buffer[row * `$INSTANCE_NAME`_ROW_BYTE_LENGTH + byteAddr] & mask) | \
            (pixelState << bitAddr);
                                                                                    
        /* Invert pixel state */
        pixelState ^= `$INSTANCE_NAME`_PIXEL_STATE_ON;
        
        `$INSTANCE_NAME`_buffer[(row + 1u) * `$INSTANCE_NAME`_ROW_BYTE_LENGTH + byteAddr] = \
            (`$INSTANCE_NAME`_buffer[(row + 1u) * `$INSTANCE_NAME`_ROW_BYTE_LENGTH + byteAddr] & mask) | \
            (pixelState << bitAddr);
        
        status = CYRET_SUCCESS;
    }
    
    return(status);
}


/*******************************************************************************
* Function Name: `$INSTANCE_NAME`_ReadPixel
********************************************************************************
*
* Summary:
*  This function reads a pixels state in a large frame
*  buffer. The Pixel is addressed by a packed number. Only included
*  in 'large' pixel address mode compliant configurations with more
*  than 4 common drivers and/or more than 64 consecutive segment
*  drivers. User code can also directly read the frame buffer RAM to
*  create optimized interactions.
*
* Parameters:
*  pixelNumber: is the packed number that points to the pixels location in the
*  frame buffer. The lowest three bits in the LSB low nibble are the bit 
*  position in the byte, the LSB upper nibble (4 bits) is the byte address in 
*  the multiplex Row and the MSB low nibble (4 bits) is the multiplex Row 
*  number.
*  `$INSTANCE_NAME`_buffer[] - Global variable.
*
* Return:
*  Pixel State: 0 for pixel off,1 for pixel on.
*
*******************************************************************************/
uint8 `$INSTANCE_NAME`_ReadPixel(uint8 pixelNumber) `=ReentrantKeil($INSTANCE_NAME . "_ReadPixel")`
{
    uint8 byteAddr,bitAddr,row;
    uint8 pixelState;
    
    pixelState = 0u;
    
    /* Extract all required information, to addres pixel, from pixelNumber */
    bitAddr = (pixelNumber & `$INSTANCE_NAME`_BIT_MASK);
    byteAddr = (pixelNumber & `$INSTANCE_NAME`_BYTE_MASK) >> `$INSTANCE_NAME`_BYTE_SHIFT;
    row = (pixelNumber & `$INSTANCE_NAME`_ROW_MASK) >> `$INSTANCE_NAME`_ROW_SHIFT;
    
    pixelState = ((`$INSTANCE_NAME`_buffer[row * `$INSTANCE_NAME`_ROW_BYTE_LENGTH + byteAddr] >> bitAddr) & \
                   `$INSTANCE_NAME`_PIXEL_STATE_ON);
   
    return (pixelState);
}


/* Start of customizer generated code */
`$writerCFunctions`


/* [] END OF FILE */
