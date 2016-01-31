/*******************************************************************************
* File Name: `$INSTANCE_NAME`.h
* Version `$CY_MAJOR_VERSION`.`$CY_MINOR_VERSION`
*
* Description:
*  This header file contains definitions associated with the Static Segment LCD
*  component.
*
* Note:
*
********************************************************************************
* Copyright 2008-2010, Cypress Semiconductor Corporation.  All rights reserved.
* You may use this file only in accordance with the license, terms, conditions,
* disclaimers, and limitations in the end user license agreement accompanying
* the software package with which this file was provided.
*******************************************************************************/

#if !defined(CY_ST_SEG_LCD_`$INSTANCE_NAME`_H)
#define CY_ST_SEG_LCD_`$INSTANCE_NAME`_H

#include "cytypes.h"
#include "cydevice_trm.h"
#include "cyfitter.h"
#include <`$INSTANCE_NAME`_Pins.h>

#if(CYDEV_CHIP_MEMBER_USED == CYDEV_CHIP_MEMBER_3A)
    #if((CYDEV_CHIP_REVISION_USED <= CYDEV_CHIP_REVISION_3A_ES2) && (`$INSTANCE_NAME`_LCD_ISR__ES2_PATCH))      
        #include <intrins.h>
    #endif /* (CYDEV_CHIP_REVISION_USED <= CYDEV_CHIP_REVISION_3A_ES2) */
#endif /* (CYDEV_CHIP_MEMBER_USED == CYDEV_CHIP_MEMBER_3A) */


/*************************************** 
* Conditional Compilation Parameters 
***************************************/

/* Condition to check is chip family used in the project is PSoC5 */
#define `$INSTANCE_NAME`_PSOC5               (CYDEV_CHIP_MEMBER_USED == CYDEV_CHIP_MEMBER_5A)

/***************************************
*     Data Struct Definitions
***************************************/

/* Sleep Mode API Support */
typedef struct _`$INSTANCE_NAME`_bakupStruct
{
    uint8 enableState;
} `$INSTANCE_NAME`_BACKUP_STRUCT;


/***************************************
*        Function Prototypes
***************************************/

void   `$INSTANCE_NAME`_Init(void);
uint8  `$INSTANCE_NAME`_Enable(void);
uint8  `$INSTANCE_NAME`_Start(void);
void   `$INSTANCE_NAME`_Stop(void);
void   `$INSTANCE_NAME`_EnableInt(void) `=ReentrantKeil($INSTANCE_NAME . "_EnableInt")`;
void   `$INSTANCE_NAME`_DisableInt(void) `=ReentrantKeil($INSTANCE_NAME . "_DisableInt")`;
void   `$INSTANCE_NAME`_ClearDisplay(void);
uint8  `$INSTANCE_NAME`_WritePixel(uint8 pixelNumber, uint8 pixelState);
uint8  `$INSTANCE_NAME`_ReadPixel(uint8 pixelNumber) `=ReentrantKeil($INSTANCE_NAME . "_ReadPixel")`;
void   `$INSTANCE_NAME`_Sleep(void);
uint8  `$INSTANCE_NAME`_Wakeup(void);
void   `$INSTANCE_NAME`_RestoreConfig(void) `=ReentrantKeil($INSTANCE_NAME . "_RestoreConfig")`;
void   `$INSTANCE_NAME`_SaveConfig(void);
`$writerHFuncDeclarations`

CY_ISR_PROTO(`$INSTANCE_NAME`_ISR);

#define `$INSTANCE_NAME`_WRITE_PIXEL(pixelNumber, pixelState)    `$INSTANCE_NAME`_WritePixel(pixelNumber, pixelState)
#define `$INSTANCE_NAME`_READ_PIXEL(pixelNumber)                 `$INSTANCE_NAME`_ReadPixel(pixelNumber)
#define `$INSTANCE_NAME`_FIND_PIXEL(port, pin, row)              (uint16) ((((row << 7u) + (port << 3u)) << 1u)+ pin)

#if(CYDEV_CHIP_MEMBER_USED == CYDEV_CHIP_MEMBER_3A)
    #if((CYDEV_CHIP_REVISION_USED <= CYDEV_CHIP_REVISION_3A_ES2) && (`$INSTANCE_NAME`_LCD_ISR__ES2_PATCH))
        #define `$INSTANCE_NAME`_ISR_PATCH() _nop_(); _nop_(); _nop_(); _nop_(); _nop_(); _nop_(); _nop_(); _nop_();
    #endif /* (CYDEV_CHIP_REVISION_USED <= CYDEV_CHIP_REVISION_3A_ES2) */
#endif /* (CYDEV_CHIP_MEMBER_USED == CYDEV_CHIP_MEMBER_3A) */

/***************************************
*           API Constants
***************************************/

#if(`$INSTANCE_NAME`_PSOC5)
    #define `$INSTANCE_NAME`_DMA_ADDRESS_MASK      (0xFFFFFFFFu)
#else
    #define `$INSTANCE_NAME`_DMA_ADDRESS_MASK      (0x0000FFFFu)
#endif /* (CYDEV_CHIP_DIE_EXPECT == CYDEV_CHIP_DIE_LEOPARD) */

#define `$INSTANCE_NAME`_ROW_LENGTH                (0x10u)

/* Actually there is only one common line but we need to 
* invret the signal on the common line for proper waveform generation.
*/
#define `$INSTANCE_NAME`_COMM_NUM                  (0x02u)

#define `$INSTANCE_NAME`_BIT_MASK                  (0x0007u) 
#define `$INSTANCE_NAME`_BYTE_MASK                 (0x00F0u)
#define `$INSTANCE_NAME`_ROW_MASK                  (0x0F00u)

#define `$INSTANCE_NAME`_NORMAL_STATE              (0x00u)
#define `$INSTANCE_NAME`_INVERTED_STATE            (0x01u)
#define `$INSTANCE_NAME`_STATE_MASK                (0x01u)

#define `$INSTANCE_NAME`_PIXEL_STATE_OFF           (0x00u)   
#define `$INSTANCE_NAME`_PIXEL_STATE_ON            (0x01u)    
#define `$INSTANCE_NAME`_PIXEL_STATE_INVERT        (0x02u)
#define `$INSTANCE_NAME`_PIXEL_UNKNOWN_STATE       (0xFFu)

#define `$INSTANCE_NAME`_TD_SIZE                   (0x10u)
#define `$INSTANCE_NAME`_BUFFER_LENGTH             (32u) 

/* 0 - No leadig zeros, 1 - leadig zeros */
#define `$INSTANCE_NAME`_MODE_0                    (0x00u)
#define `$INSTANCE_NAME`_MODE_1                    (0x01u)

#define `$INSTANCE_NAME`_ROW_BYTE_LENGTH           (0x10u) 

#define `$INSTANCE_NAME`_MAX_PORTS                 (0x10u)

#define `$INSTANCE_NAME`_BYTE_SHIFT                (0x04u)
#define `$INSTANCE_NAME`_ROW_SHIFT                 (0x08u)

#define `$INSTANCE_NAME`_ISR_NUMBER                (`$INSTANCE_NAME`_LCD_ISR__INTC_NUMBER)
#define `$INSTANCE_NAME`_ISR_PRIORITY              (`$INSTANCE_NAME`_LCD_ISR__INTC_PRIOR_NUM)

/* Following definition of the global variables 
* which are obsolete and which will be removed in the near future.
*/
#define `$INSTANCE_NAME`_Buffer                    `$INSTANCE_NAME`_buffer
#define `$INSTANCE_NAME`_Channel                   `$INSTANCE_NAME`_channel
#define `$INSTANCE_NAME`_TermOut                   `$INSTANCE_NAME`_termOut
#define `$INSTANCE_NAME`_TD                        `$INSTANCE_NAME`_td
#define `$INSTANCE_NAME`_GCommons                  `$INSTANCE_NAME`_gCommons
#define `$INSTANCE_NAME`_Commons                   `$INSTANCE_NAME`_commons

`$writerHPixelDef`


#define `$INSTANCE_NAME`_LCD_TD_SIZE               (`$INSTANCE_NAME`_StSegLcdPort_DMA_TD_PROTO_COUNT)

/***************************************
*             Registers
***************************************/

#define `$INSTANCE_NAME`_ALIASED_AREA_PTR                        ((reg32) CYDEV_IO_DR_PRT0_DR_ALIAS)
#define `$INSTANCE_NAME`_PORT_BASE_PTR                           ((reg8 *) CYDEV_IO_PRT_PRT0_BASE)

#endif /* CY_ST_SEG_LCD_`$INSTANCE_NAME`_H */


/* [] END OF FILE */
