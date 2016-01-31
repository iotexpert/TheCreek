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
* Copyright 2008-2009, Cypress Semiconductor Corporation.  All rights reserved.
* You may use this file only in accordance with the license, terms, conditions,
* disclaimers, and limitations in the end user license agreement accompanying
* the software package with which this file was provided.
*******************************************************************************/


#include "cytypes.h"
#include "`$INSTANCE_NAME`.h"
#include <`$INSTANCE_NAME`_InClock.h>
#include <`$INSTANCE_NAME`_LCD_ISR.h>
/* Look-up tables for different kinds of displays */

#ifdef `$INSTANCE_NAME`_7SEG

const uint8 `$INSTANCE_NAME`_7SegDigits[] =
    /*'0'  '1'  '2'  '3'  '4'  '5'  '6'  '7'  '8'  '9'  'A'  'B'  'C'  'D'  'E'  'F'  blank */
    {0x3f,0x06,0x5b,0x4f,0x66,0x6d,0x7d,0x07,0x7f,0x6f,0x77,0x7c,0x39,0x5e,0x79,0x71,0x00};
#endif /* `$INSTANCE_NAME`_7SEG */

#ifdef ALPHANUMERIC

#ifdef `$INSTANCE_NAME`_14SEG
const uint16 `$INSTANCE_NAME`_14SegChars[] = {
/*----------------------------------------------------*/
/*                     Blank                          */
/*----------------------------------------------------*/
0x0000,0x0000,0x0000,0x0000,0x0000,0x0000,0x0000,0x0000,
0x0000,0x0000,0x0000,0x0000,0x0000,0x0000,0x0000,0x0000,
0x0000,0x0000,0x0000,0x0000,0x0000,0x0000,0x0000,0x0000,
0x0000,0x0000,0x0000,0x0000,0x0000,0x0000,0x0000,0x0000,
/*----------------------------------------------------*/
/*        !      "      #      $      %      &      ' */
/*----------------------------------------------------*/
0x0000,0x0006,0x0120,0x3fff,0x156d,0x2ee4,0x2a8d,0x0200,
/*----------------------------------------------------*/
/* (      )      *      +      ,      -      .      / */
/*----------------------------------------------------*/
0x0a00,0x2080,0x3fc0,0x1540,0x2000,0x0440,0x1058,0x2200,
/*----------------------------------------------------*/
/* 0      1      2      3      4      5      6      7 */
/*----------------------------------------------------*/
0x223f,0x0206,0x045b,0x040f,0x0466,0x0869,0x047d,0x1201,
/*----------------------------------------------------*/
/* 8      9      :      ;      <      =      >      ? */
/*----------------------------------------------------*/
0x047f,0x046f,0x2200,0x1100,0x2100,0x0a00,0x0448,0x2080,
/*----------------------------------------------------*/
/* @      A      B      C      D      E      F      G */
/*----------------------------------------------------*/
0x053b,0x0477,0x150f,0x0039,0x110F,0x0079,0x0071,0x043d,
/*----------------------------------------------------*/
/* H      I      J      K      L      M      N      O */
/*----------------------------------------------------*/
0x0476,0x1100,0x001e,0x0a70,0x0038,0x02b6,0x08b6,0x003f,
/*----------------------------------------------------*/
/* P      Q      R      S      T      U      V      W */
/*----------------------------------------------------*/
0x0473,0x083f,0x0C73,0x046d,0x1101,0x003e,0x2230,0x2836,
/*----------------------------------------------------*/
/* X      Y      Z      [      \      ]      ^      _ */
/*----------------------------------------------------*/
0x2a80,0x1280,0x2209,0x0039,0x0880,0x000f,0x2203,0x2008,
/*----------------------------------------------------*/
/* @      a      b      c      d      e      f      g */
/*----------------------------------------------------*/
0x0080,0x0477,0x150f,0x0039,0x110F,0x0079,0x0071,0x043d,
/*----------------------------------------------------*/
/* h      i      j      k      l      m      n      o */
/*----------------------------------------------------*/
0x0476,0x1100,0x001e,0x0a70,0x0038,0x02b6,0x08b6,0x003f,
/*----------------------------------------------------*/
/* p      q      r      s      t      u      v      w */
/*----------------------------------------------------*/
0x0473,0x083f,0x0C73,0x046d,0x1101,0x003e,0x2230,0x2836,
/*----------------------------------------------------*/
/* x      y      z      {      |      }      ~        */
/*----------------------------------------------------*/
0x2a80,0x1280,0x2209,0x0e00,0x1100,0x20C0,0x0452,0x003f};
#endif /* `$INSTANCE_NAME`_14SEG */

#ifdef `$INSTANCE_NAME`_16SEG
const uint16 `$INSTANCE_NAME`_16SegChars[] = {
/*----------------------------------------------------*/
/*                     Blank                          */
/*----------------------------------------------------*/
0x0000,0x0000,0x0000,0x0000,0x0000,0x0000,0x0000,0x0000,
0x0000,0x0000,0x0000,0x0000,0x0000,0x0000,0x0000,0x0000,
0x0000,0x0000,0x0000,0x0000,0x0000,0x0000,0x0000,0x0000,
0x0000,0x0000,0x0000,0x0000,0x0000,0x0000,0x0000,0x0000,
/*----------------------------------------------------*/
/*        !      "      #      $      %      &      ' */
/*----------------------------------------------------*/
0x0000,0x000c,0x0480,0xffff,0x55bb,0xdd99,0xaa3b,0x0800,
/*----------------------------------------------------*/
/* (      )      *      +      ,      -      .      / */
/*----------------------------------------------------*/
0x2800,0x8200,0xff00,0x5500,0x8000,0x1100,0x4160,0x8800,
/*----------------------------------------------------*/
/* 0      1      2      3      4      5      6      7 */
/*----------------------------------------------------*/
0x88ff,0x080c,0x1177,0x103f,0x118c,0x21b3,0x11fb,0x4803,
/*----------------------------------------------------*/
/* 8      9      :      ;      <      =      >      ? */
/*----------------------------------------------------*/
0x11ff,0x11bf,0x4400,0x8400,0x2800,0x1130,0x8200,0x5006,
/*----------------------------------------------------*/
/* @      A      B      C      D      E      F      G */
/*----------------------------------------------------*/
0x14f7,0x11cf,0x543f,0x00f3,0x443f,0x01f3,0x01c3,0x10fb,
/*----------------------------------------------------*/
/* H      I      J      K      L      M      N      O */
/*----------------------------------------------------*/
0x11cc,0x4400,0x007e,0x29c0,0x00f0,0x0acc,0x22cc,0x00ff,
/*----------------------------------------------------*/
/* P      Q      R      S      T      U      V      W */
/*----------------------------------------------------*/
0x11c7,0x20ff,0x31c7,0x11bb,0x4403,0x00fc,0x88c0,0xa0cc,
/*----------------------------------------------------*/
/* X      Y      Z      [      \      ]      ^      _ */
/*----------------------------------------------------*/
0xaa00,0x4A00,0x8833,0x4412,0x2200,0x4421,0x8806,0x0030,
/*----------------------------------------------------*/
/* @      a      b      c      d      e      f      g */
/*----------------------------------------------------*/
0x0200,0x11cf,0x543f,0x00f3,0x443f,0x01f3,0x01c3,0x10fb,
/*----------------------------------------------------*/
/* h      i      j      k      l      m      n      o */
/*----------------------------------------------------*/
0x11cc,0x4400,0x007e,0x29c0,0x00f0,0x0acc,0x22cc,0x00ff,
/*----------------------------------------------------*/
/* p      q      r      s      t      u      v      w */
/*----------------------------------------------------*/
0x11c7,0x20ff,0x31c7,0x11bb,0x4403,0x00fc,0x88c0,0xa0cc,
/*----------------------------------------------------*/
/* x      y      z      {      |      }      ~        */
/*----------------------------------------------------*/
0xaa00,0x4A00,0x8833,0x3800,0x4400,0x8300,0x1144,0x0000};
#endif /* `$INSTANCE_NAME`_16SEG */

#endif /* ALPHANUMERIC */

/* Frame Buffer */
static uint8        `$INSTANCE_NAME`_Buffer[`$INSTANCE_NAME`_BUFFER_LENGTH] ;

/* Array of TDs */
static uint8        `$INSTANCE_NAME`_TD[2];

/* DMA channel dedicated for SegLCD  */
static uint8        `$INSTANCE_NAME`_Channel;

uint8 `$INSTANCE_NAME`_TermOut = (`$INSTANCE_NAME`_StSegLCDDma__TERMOUT0_EN ? TD_TERMOUT0_EN : 0) |
                                     (`$INSTANCE_NAME`_StSegLCDDma__TERMOUT1_EN ? TD_TERMOUT1_EN : 0);

uint8 `$INSTANCE_NAME`_initVar = 0;

/******************************************/
/*** Start of customizer generated code ***/
/******************************************/
`$writerCVariables`
/******************************************/
/***  End of customizer generated code  ***/
/******************************************/


/*******************************************************************************
* Function Name: `$INSTANCE_NAME`_Start
********************************************************************************
* Summary:
*  Starts the LCD component and enables any required
*  interrupts, DMA channels frame buffer and hardware. Does not
*  clear the frame buffer SRAM if previously defined.
*
* Parameters:
*  void:
*
* Return:
*  Status one of standard status for PSoC3 Component
*  CYRET_SUCCESS - Function completed successfully.
*  CYRET_LOCKED - The object was locked, already in use. Some of TDs or
*                 a channel already i use.
*
* Theory:
*
* Side Effects:
*
*******************************************************************************/
uint8 `$INSTANCE_NAME`_Start()
{
    uint8   Status;
    uint8   ErrorCnt = 0;
    uint8   i;
    uint32  FSrcAddr;
    uint32  Addr;
    uint8   `$INSTANCE_NAME`_NextTD;

    FSrcAddr = `$INSTANCE_NAME`_DMA_ADDRESS_MASK & ((uint32) `$INSTANCE_NAME`_Buffer);
 
    Addr = ((Addr | 0xFFFFFFFF) & `$INSTANCE_NAME`_ALIASED_AREA_PTR);

    /* If not Initialized then initialize all required hardware and software */
    if(`$INSTANCE_NAME`_initVar == 0)
    {`$INSTANCE_NAME`_initVar = 1;
        /* Need to clear display to start normal work */
        `$INSTANCE_NAME`_ClearDisplay();
    }

/***************************** LCD DMA initialization *************************/

    `$INSTANCE_NAME`_Channel = `$INSTANCE_NAME``[StSegLCDDma]`DmaInitialize(0, 0, HI16(FSrcAddr), HI16(Addr));

    if(`$INSTANCE_NAME`_Channel == DMA_INVALID_CHANNEL)
    {
        ErrorCnt++;
    }

    /* Get first Transaction Descriptor. */
    `$INSTANCE_NAME`_TD[0] = CyDmaTdAllocate();
    if(`$INSTANCE_NAME`_TD[0] == DMA_INVALID_TD)
    {
        ErrorCnt++;
    }

    for(i = 1; i <= 2; i++)
    {
        if(i < 2)
        {
            `$INSTANCE_NAME`_TD[i] = CyDmaTdAllocate();

            if(`$INSTANCE_NAME`_TD[i] == DMA_INVALID_TD)
            {
                ErrorCnt++;
            }
            /* Setup a TD. */
            `$INSTANCE_NAME`_NextTD = `$INSTANCE_NAME`_TD[i];
        }
        else
            `$INSTANCE_NAME`_NextTD = `$INSTANCE_NAME`_TD[0];

        /* Configure TD as chained,  */
        CyDmaTdSetConfiguration( `$INSTANCE_NAME`_TD[i-1],
                                 `$INSTANCE_NAME`_TD_SIZE,
                                `$INSTANCE_NAME`_NextTD,
                                 /* no specific configuration */
                                 TD_INC_DST_ADR | TD_INC_SRC_ADR | `$INSTANCE_NAME`_TermOut);

        CyDmaTdSetAddress(`$INSTANCE_NAME`_TD[i-1], LO16(FSrcAddr) + (i-1)*`$INSTANCE_NAME`_TD_SIZE, LO16(Addr));

    }
    CyDmaChSetInitialTd(`$INSTANCE_NAME`_Channel, `$INSTANCE_NAME`_TD[0]);


    CyDmaChEnable(`$INSTANCE_NAME`_Channel, 1);

/********************** ISR initialization  ********************************/

        CyIntDisable(`$INSTANCE_NAME`_ISR_NUMBER);

        /* Set the ISR to point to the RTC_SUT_isr Interrupt. */
        CyIntSetVector(`$INSTANCE_NAME`_ISR_NUMBER, `$INSTANCE_NAME`_ISR);

        /* Set the priority. */
        CyIntSetPriority(`$INSTANCE_NAME`_ISR_NUMBER, `$INSTANCE_NAME`_ISR_PRIORITY);

    /* Starts Static Segment LCD */

    if(ErrorCnt == 0)
    {
        /* Enable required clocks */
        `$INSTANCE_NAME``[InClock]`Enable();
        CyIntEnable(`$INSTANCE_NAME`_ISR_NUMBER);
        Status = CYRET_SUCCESS;
    }
    else
        Status = CYRET_LOCKED;

    return(Status);
}


/*******************************************************************************
* Function Name: `$INSTANCE_NAME`_Stop
********************************************************************************
* Summary:
*  Disables the LCD component and disables any required interrupts and DMA 
*  channels. Automatically blanks the display to avoid damage from DC offsets. 
*  Does not clear the frame buffer.
*
* Parameters:
*  void:
*
* Return:
*  void
*
* Theory:
*
* Side Effects:
*
*******************************************************************************/
void `$INSTANCE_NAME`_Stop()
{
    uint8 i;
    for(i = 0; i < 2; i++)
    CyDmaTdFree(`$INSTANCE_NAME`_TD[i]);

    `$INSTANCE_NAME``[StSegLCDDma]`DmaRelease();
    CyIntDisable(`$INSTANCE_NAME`_ISR_NUMBER);
    `$INSTANCE_NAME``[InClock]`Disable();
}


/*******************************************************************************
* Function Name: `$INSTANCE_NAME`_EnableInt
********************************************************************************
* Summary:
*  Description: Enables the LCD interrupt/s. Not required if `$INSTANCE_NAME`
*  _Start called.
*
* Parameters:
*  void:
*
* Return:
*  void
*
* Theory:
*
* Side Effects:
*
*******************************************************************************/
void `$INSTANCE_NAME`_EnableInt()
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
*  void:
*
* Return:
*  void
*
* Theory:
*
* Side Effects:
*
*******************************************************************************/
void `$INSTANCE_NAME`_DisableInt()
{
   CyIntDisable(`$INSTANCE_NAME`_ISR_NUMBER);
}


/*******************************************************************************
* Function Name: `$INSTANCE_NAME`_ClearDisplay
********************************************************************************
* Summary:
*  This function clears the display RAM of the frame buffer.
*
* Parameters:
*  void:
*
* Return:
*  void
*
* Theory:
*
* Side Effects:
*
*******************************************************************************/
void `$INSTANCE_NAME`_ClearDisplay()
{
    uint16 i;

    /* Clear entire frame buffer to all zeroes */
    for(i=0; i<`$INSTANCE_NAME`_BUFFER_LENGTH; i++)
    {
        if(i<(`$INSTANCE_NAME`_BUFFER_LENGTH/2))
            `$INSTANCE_NAME`_Buffer[i] = 0x00;
        else
            `$INSTANCE_NAME`_Buffer[i] = 0xFF;
    }

    #ifdef GANG
        `$INSTANCE_NAME`_WRITE_PIXEL(`$INSTANCE_NAME`_GCommons[0], `$INSTANCE_NAME`_PIXEL_STATE_OFF);
    #endif /* GANG */

    /* Reinitialize the commons */
    `$INSTANCE_NAME`_WRITE_PIXEL(`$INSTANCE_NAME`_Commons[0], `$INSTANCE_NAME`_PIXEL_STATE_OFF);
}


/*******************************************************************************
* Function Name: `$INSTANCE_NAME`_WritePixel
********************************************************************************
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
*  nibble (4 bits) is the byte address in the multiplex row and
*  the MSB low nibble (4 bits) is the multiplex row number.
*  pixelState : 0 for pixel off,1 for pixel on, 2 for pixel invert.
*
* Return:
*  Status returns the standardized return value for pass
*  or fail on a range check of the byte address and multiplex row
*  number:
*  CYRET_SUCCESS - Function completed successfully.
*  CYRET_BAD_PARAM - One of the parameters is invalid.
*
* Theory:
*
* Side Effects:
*
*******************************************************************************/
uint8 `$INSTANCE_NAME`_WritePixel(uint8 pixelNumber, uint8 pixelState)
{
    uint16 ByteAddr,BitAddr,Row;
    uint8 Mask;
    uint8 status;

    /* Let the User know he is entered wrong parameter
       which is pixel state greater than 2 or unconnected pixel */
    if((pixelState > 2) || (pixelNumber == NOT_CON)) status = CYRET_BAD_PARAM;
    else
    {
        /* Extract bit position in the byte*/
        BitAddr = (pixelNumber & `$INSTANCE_NAME`_BIT_MASK);
        /* Extract byte position in the row*/
        ByteAddr = (pixelNumber & `$INSTANCE_NAME`_BYTE_MASK) >> 4;
        /* Extract row position*/
        Row = (pixelNumber & `$INSTANCE_NAME`_ROW_MASK) >> 8;

        if(pixelState == 2)
        {
            /* Invert actual pixel state */
            pixelState = !(`$INSTANCE_NAME`_ReadPixel(pixelNumber));
        }

        Mask = ~(`$INSTANCE_NAME`_PIXEL_STATE_ON << BitAddr);
        /************** Set pixel in first frame buffer line ***************/
        `$INSTANCE_NAME`_Buffer[Row * 16 + ByteAddr] =
            (`$INSTANCE_NAME`_Buffer[Row * 16 + ByteAddr] & Mask) | (pixelState << BitAddr);
        /******** Invert pixel value in the second frame buffer line *******/
        pixelState ^= 0x01;
        `$INSTANCE_NAME`_Buffer[(Row + 1) * 16 + ByteAddr] =
            (`$INSTANCE_NAME`_Buffer[(Row + 1) * 16 + ByteAddr] & Mask) | (pixelState << BitAddr);
        status = CYRET_SUCCESS;
    }
    return(status);
}


/*******************************************************************************
* Function Name: `$INSTANCE_NAME`_ReadPixel
********************************************************************************
* Summary:
*  This function reads a pixels state in a large frame
*  buffer. The Pixel is addressed by a packed number. Only included
*  in 'large' pixel address mode compliant configurations with more
*  than 4 common drivers and/or more than 64 consecutive segment
*  drivers. User code can also directly read the frame buffer RAM to
*  create optimized interactions.
*
* Parameters:
*  pixelNumber: is the packed number that points to the
*  pixels location in the frame buffer. The lowest three bits in the LSB
*  low nibble are the bit position in the byte, the LSB upper nibble (4
*  bits) is the byte address in the multiplex row and the MSB low
*  nibble (4 bits) is the multiplex row number.
*
* Return:
*  Pixel State: 0 for pixel off,1 for pixel on
*
* Theory:
*
* Side Effects:
*
*******************************************************************************/
uint8 `$INSTANCE_NAME`_ReadPixel(uint8 pixelNumber)
{
  uint8 ByteAddr,BitAddr,Row;
  uint8 pixelState;

  pixelState = 0;

  /* Extract all required information, to addres pixel, from pixelNumber */
  BitAddr = (pixelNumber & `$INSTANCE_NAME`_BIT_MASK);
  ByteAddr = (pixelNumber & `$INSTANCE_NAME`_BYTE_MASK) >> 4;
  Row = (pixelNumber & `$INSTANCE_NAME`_ROW_MASK) >> 8;

  pixelState = (( `$INSTANCE_NAME`_Buffer[Row * 16 + ByteAddr] >> BitAddr) & 1);
  return (pixelState);
}


/******************************************/
/*** Start of customizer generated code ***/
/******************************************/
`$writerCFunctions`
/******************************************/
/***  End of customizer generated code  ***/
/******************************************/


/* [] END OF FILE */
