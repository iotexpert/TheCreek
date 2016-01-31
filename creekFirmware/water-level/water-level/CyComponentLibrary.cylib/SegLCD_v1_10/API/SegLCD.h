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
#include <`$INSTANCE_NAME``[LCD_Dma]`dma.h>

#ifndef `$INSTANCE_NAME`_SegLCD_H
#define `$INSTANCE_NAME`_SegLCD_H

/********************************
 ****** Function Prototypes *****
 *******************************/

uint8    `$INSTANCE_NAME`_Start(void);
void     `$INSTANCE_NAME`_Stop(void);
void     `$INSTANCE_NAME`_EnableInt(void);
void     `$INSTANCE_NAME`_DisableInt(void);
uint8    `$INSTANCE_NAME`_SetBias(uint8 BiasLevel);
uint8    `$INSTANCE_NAME`_WriteInvertState(uint8 InvertState);
uint8    `$INSTANCE_NAME`_ReadInvertState(void);
void     `$INSTANCE_NAME`_ClearDisplay(void);
uint8    `$INSTANCE_NAME`_WritePixel(uint16 PixelNumber, uint8 PixelState);
uint8    `$INSTANCE_NAME`_ReadPixel(uint16 PixelNumber);
void     `$INSTANCE_NAME`_SetAwakeMode(void);
void     `$INSTANCE_NAME`_SetSleepMode(void);

/*** Start of customizer generated code ***/
`$writerHFuncDeclarations`
/*** End of customizer generated code ***/

/****************************************************
*-----		Parameter values				    -----
****************************************************/

#define `$INSTANCE_NAME`_COMM_NUM              `$NumCommonLines`
#define `$INSTANCE_NAME`_SEG_NUM               `$NumSegmentLines`
#define `$INSTANCE_NAME`_BIAS_TYPE             `$BiasType`
#define `$INSTANCE_NAME`_BIAS_VOLTAGE          `$BiasVoltage` * 2
#define `$INSTANCE_NAME`_DRIVER_POWER_MODE     `$DriverPowerMode`
#define `$INSTANCE_NAME`_FRAME_RATE            `$FrameRate`
#define `$INSTANCE_NAME`_HI_DRIVE_TIME         `$HiDriveTime`
#define `$INSTANCE_NAME`_LO_DRIVE_IINIT_TIME   `$LowDriveInitTime`
#define `$INSTANCE_NAME`_LOW_DRIVE_MODE        `$LowDriveMode`
#define `$INSTANCE_NAME`_WAVEFORM_TYPE         `$WaveformType`
#define `$INSTANCE_NAME`_DAC_DIS_INIT_TIME     `$DacDisInitTime`

`#cy_declare_enum Waveform_type`
`#cy_declare_enum DriverPowerModes`
/*******************************
******     Macros          *****    
*******************************/

#define   `$INSTANCE_NAME`_WRITE_PIXEL(PixelNumber,PixelState)   `$INSTANCE_NAME`_WritePixel(PixelNumber,PixelState)
#define   `$INSTANCE_NAME`_READ_PIXEL(PixelNumber)               `$INSTANCE_NAME`_ReadPixel(PixelNumber)
#define   `$INSTANCE_NAME`_FIND_PIXEL(Port,Pin,Row)              (uint16)((((Row * 128) + (Port * 8))<<1)+ Pin)

/********************************
 ******     Constants       *****
 *******************************/
                              
#define `$INSTANCE_NAME`_TD_SIZE                   0x10U
#define `$INSTANCE_NAME`_BUFFER_LENGTH             256U 

#define `$INSTANCE_NAME`_LOW_RANGE                 0x00U 
#define `$INSTANCE_NAME`_HI_RANGE                  0x01U

#define `$INSTANCE_NAME`_INVERT_BIT_MASK           0x02U 
#define `$INSTANCE_NAME`_SLEEP_ENABLE              0x01U  
#define `$INSTANCE_NAME`_SLEEP_BIT_MASK            0xF5U  

#define `$INSTANCE_NAME`_BIT_MASK                  0x0007U 
#define `$INSTANCE_NAME`_BYTE_MASK                 0x00F0U
#define `$INSTANCE_NAME`_ROW_MASK                  0x0F00U

#define `$INSTANCE_NAME`_NORMAL_STATE              0x00U      
#define `$INSTANCE_NAME`_INVERTED_STATE            0x01U  
#define `$INSTANCE_NAME`_STATE_MASK                0xFBU      
	 
#define `$INSTANCE_NAME`_PIXEL_STATE_OFF           0x00U    
#define `$INSTANCE_NAME`_PIXEL_STATE_ON            0x01U     
#define `$INSTANCE_NAME`_PIXEL_STATE_INVERT        0x02U     
#define `$INSTANCE_NAME`_PIXEL_UNKNOWN_STATE       0xFFU 

/* 0 - No leadig zeros, 1 - leadig zeros */
#define `$INSTANCE_NAME`_MODE_0                    0x00U       
#define `$INSTANCE_NAME`_MODE_1                    0x01U       

#define `$INSTANCE_NAME`_MAX_BUFF_ROWS             0x10U       
#define `$INSTANCE_NAME`_MAX_BUFF_COLS             0xFFU    
#define `$INSTANCE_NAME`_ROW_BYTE_LEN              0x10U

/*** Start of customizer generated code ***/
`$writerHPixelDef`
/*** End of customizer generated code ***/

/********************************
 ******     Registers       *****
 *******************************/

#define `$INSTANCE_NAME`_LCDDAC_CONTROL            `$INSTANCE_NAME`_LCD__CR0
#define `$INSTANCE_NAME`_CONTRAST_CONTROL          `$INSTANCE_NAME`_LCD__CR1
#define `$INSTANCE_NAME`_DRIVER_CONTROL            `$INSTANCE_NAME`_LCD__CR
#define `$INSTANCE_NAME`_ALIASED_AREA_PTR          (reg32) CYDEV_IO_DR_PRT0_DR_ALIAS
#define `$INSTANCE_NAME`_PWR_MGR                   (* (reg8 *) `$INSTANCE_NAME`_LCD__PM_ACT_CFG)

#define `$INSTANCE_NAME`_CONTROL                   (* (reg8 *) `$INSTANCE_NAME`_bLCDDSD_ctrlreg__CONTROL_REG)   
#define `$INSTANCE_NAME`_CNT_DELAY                 (* (reg8 *) `$INSTANCE_NAME`_bLCDDSD_bSegLCDdp_u0__D0_REG)
#define `$INSTANCE_NAME`_EN_HI_DELAY               (* (reg8 *) `$INSTANCE_NAME`_bLCDDSD_bSegLCDdp_u0__D1_REG)
#define `$INSTANCE_NAME`_CNT_PERIOD                (* (reg8 *) `$INSTANCE_NAME`_bLCDDSD_bSegLCDdp_u0__A1_REG)
#define `$INSTANCE_NAME`_FRAME_CNT7_PERIOD         (* (reg8 *) `$INSTANCE_NAME`_bLCDDSD_DivCounter__CONTROL_AUX_CTL_REG)

#define `$INSTANCE_NAME`_LCDDAC_SWITCH_REG0        `$INSTANCE_NAME`_LCD__SW0
#define `$INSTANCE_NAME`_LCDDAC_SWITCH_REG1        `$INSTANCE_NAME`_LCD__SW1
#define `$INSTANCE_NAME`_LCDDAC_SWITCH_REG2        `$INSTANCE_NAME`_LCD__SW2
#define `$INSTANCE_NAME`_LCDDAC_SWITCH_REG3        `$INSTANCE_NAME`_LCD__SW3
#define `$INSTANCE_NAME`_LCDDAC_SWITCH_REG4        `$INSTANCE_NAME`_LCD__SW4

 /*************************************
 ******     Register values       *****
 *************************************/
#define `$INSTANCE_NAME`_LCD_EN                    `$INSTANCE_NAME`_LCD__PM_ACT_MSK
 
#define `$INSTANCE_NAME`_BIAS_TYPE_MASK            0xFCU      
#define `$INSTANCE_NAME`_LCDDAC_EN                 0x04U

#define `$INSTANCE_NAME`_CLK_ENABLE                0x01U      
#define `$INSTANCE_NAME`_RESET                     0x02U
#define `$INSTANCE_NAME`_POST_RESET                0x04U
#define `$INSTANCE_NAME`_CNTR7_ENABLE              0x20U

/* Set En_hi Active state for (`$HiDriveTime`+1) cycles of input frequency */
#define `$INSTANCE_NAME`_EN_HI_ACT_VAL             `$INSTANCE_NAME`_HI_DRIVE_TIME //(0xFFU - `$HiDriveTime`)

/* Defines compare value for DacDisable signal */
#define `$INSTANCE_NAME`_CNT_PERIOD_VAL            0x5FU	
#define `$INSTANCE_NAME`_CNT_DELAY_VAL             127 + `$INSTANCE_NAME`_DAC_DIS_INIT_TIME

#define `$INSTANCE_NAME`_LCDDAC_VOLTAGE_SEL        0x01U       
 
#endif  /* SegLCD_H */