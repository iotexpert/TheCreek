/******************************************************************************
 ******************************************************************************
 *  FILENAME: `$INSTANCE_NAME`.h
 * Version `$CY_MAJOR_VERSION`.`$CY_MINOR_VERSION`
 *
 *  DESCRIPTION: 
 *     Header file for Timer user module.
 *
********************************************************************************
* Copyright 2008-2009, Cypress Semiconductor Corporation.  All rights reserved.
* You may use this file only in accordance with the license, terms, conditions, 
* disclaimers, and limitations in the end user license agreement accompanying 
* the software package with which this file was provided.
********************************************************************************/



#include "cytypes.h"
#include "cyfitter.h"
#include "cydevice.h"
#include <`$INSTANCE_NAME``[StSegLCDDma]`dma.h>

#ifndef `$INSTANCE_NAME`_H
#define `$INSTANCE_NAME`_H

/********************************
 ****** Function Prototypes *****
 *******************************/

uint8  `$INSTANCE_NAME`_Start(void);
void   `$INSTANCE_NAME`_Stop(void);
void   `$INSTANCE_NAME`_EnableInt(void);
void   `$INSTANCE_NAME`_DisableInt(void);
void   `$INSTANCE_NAME`_ClearDisplay(void);
uint8  `$INSTANCE_NAME`_WritePixel(uint8 PixelNumber, uint8 PixelState);
uint8  `$INSTANCE_NAME`_ReadPixel(uint8 PixelNumber);

/*** Start of customizer generated code ***/
`$writerHFuncDeclarations`
/*** End of customizer generated code ***/

#define   `$INSTANCE_NAME`_WRITE_PIXEL(PixelNumber,PixelState)   `$INSTANCE_NAME`_WritePixel(PixelNumber,PixelState)
#define   `$INSTANCE_NAME`_READ_PIXEL(PixelNumber)               `$INSTANCE_NAME`_ReadPixel(PixelNumber)
#define   `$INSTANCE_NAME`_FIND_PIXEL(Port,Pin,Row)              (uint16)((((Row * 128) + (Port * 8))<<1)+ Pin)

#define `$INSTANCE_NAME`_TD_SIZE                   0x10U
#define `$INSTANCE_NAME`_BUFFER_LENGTH             32U 
#define `$INSTANCE_NAME`_ALIASED_AREA_PTR          (reg32) CYDEV_IO_DR_PRT0_DR_ALIAS

/********************************
 ******     Constants       *****
 *******************************/

#define `$INSTANCE_NAME`_BIT_MASK                  0x0007U 
#define `$INSTANCE_NAME`_BYTE_MASK                 0x00F0U
#define `$INSTANCE_NAME`_ROW_MASK                  0x0F00U

#define `$INSTANCE_NAME`_PIXEL_STATE_OFF           0x00U    
#define `$INSTANCE_NAME`_PIXEL_STATE_ON            0x01U     
#define `$INSTANCE_NAME`_PIXEL_STATE_INVERT        0x02U     
#define `$INSTANCE_NAME`_PIXEL_UNKNOWN_STATE       0xFFU 

/* 0 - No leadig zeros, 1 - leadig zeros */
#define `$INSTANCE_NAME`_MODE_0                    0x00U       
#define `$INSTANCE_NAME`_MODE_1                    0x01U  

/******************************************************************
 ******    Constans and variables for specific displays       *****
 *****************************************************************/

/*** Start of customizer generated code ***/
`$writerHPixelDef`
/*** End of customizer generated code ***/

#endif /* `$INSTANCE_NAME`_H */

//[] END OF FILE
