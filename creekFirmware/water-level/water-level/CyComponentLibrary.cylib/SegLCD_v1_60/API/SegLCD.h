/*******************************************************************************
* File Name: `$INSTANCE_NAME`.h
* Version `$CY_MAJOR_VERSION`.`$CY_MINOR_VERSION`
*
* Description:
*  This header file contains definitions associated with the Segment LCD
*  component.
*
* Note:
*
********************************************************************************
* Copyright 2008-2011, Cypress Semiconductor Corporation.  All rights reserved.
* You may use this file only in accordance with the license, terms, conditions,
* disclaimers, and limitations in the end user license agreement accompanying
* the software package with which this file was provided.
*******************************************************************************/

#if !defined(CY_SEGLCD_`$INSTANCE_NAME`_H)
#define CY_SEGLCD_`$INSTANCE_NAME`_H


#include "cytypes.h"
#include "cydevice_trm.h"
#include "cyfitter.h"
#include "CyLib.h"

#if(CYDEV_CHIP_MEMBER_USED == CYDEV_CHIP_MEMBER_3A)
    #if((CYDEV_CHIP_REVISION_USED <= CYDEV_CHIP_REVISION_3A_ES2) && (`$INSTANCE_NAME`_TD_DoneInt__ES2_PATCH))
        #include <intrins.h>
    #endif /* (CYDEV_CHIP_REVISION_USED <= CYDEV_CHIP_REVISION_3A_ES2) */
#endif /* (CYDEV_CHIP_MEMBER_USED == CYDEV_CHIP_MEMBER_3A) */


/***************************************
*   Conditional Compilation Parameters
***************************************/

#define `$INSTANCE_NAME`_COMM_NUM            (`$NumCommonLines`u)

#define `$INSTANCE_NAME`_BIT_MASK            (0x0007u)
#define `$INSTANCE_NAME`_BYTE_MASK           (0x00F0u)
#define `$INSTANCE_NAME`_ROW_MASK            (0x0F00u)

#define `$INSTANCE_NAME`_BYTE_SHIFT          (0x04u)
#define `$INSTANCE_NAME`_ROW_SHIFT           (0x08u)


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
void    `$INSTANCE_NAME`_Init(void);
uint8   `$INSTANCE_NAME`_Start(void);
uint8   `$INSTANCE_NAME`_Enable(void);
void    `$INSTANCE_NAME`_Stop(void);
void    `$INSTANCE_NAME`_EnableInt(void) `=ReentrantKeil($INSTANCE_NAME . "_EnableInt")`;
void    `$INSTANCE_NAME`_DisableInt(void) `=ReentrantKeil($INSTANCE_NAME . "_DisableInt")`;
uint8   `$INSTANCE_NAME`_SetBias(uint8 biasLevel) `=ReentrantKeil($INSTANCE_NAME . "_SetBias")`;
uint8   `$INSTANCE_NAME`_WriteInvertState(uint8 invertState);
uint8   `$INSTANCE_NAME`_ReadInvertState(void) `=ReentrantKeil($INSTANCE_NAME . "_ReadInvertState")`;
void    `$INSTANCE_NAME`_ClearDisplay(void);
uint8   `$INSTANCE_NAME`_WritePixel(uint16 pixelNumber, uint8 pixelState);
uint8   `$INSTANCE_NAME`_ReadPixel(uint16 pixelNumber) `=ReentrantKeil($INSTANCE_NAME . "_ReadPixel")`;
void    `$INSTANCE_NAME`_SetAwakeMode(void) `=ReentrantKeil($INSTANCE_NAME . "_SetAwakeMode")`;
void    `$INSTANCE_NAME`_SetSleepMode(void) `=ReentrantKeil($INSTANCE_NAME . "_SetSleepMode")`;
void    `$INSTANCE_NAME`_Sleep(void);
uint8   `$INSTANCE_NAME`_Wakeup(void);
void    `$INSTANCE_NAME`_SaveConfig(void);
void    `$INSTANCE_NAME`_RestoreConfig(void) `=ReentrantKeil($INSTANCE_NAME . "_RestoreConfig")`;

`$writerHFuncDeclarations`
CY_ISR_PROTO(`$INSTANCE_NAME`_ISR);

#define `$INSTANCE_NAME`_WRITE_PIXEL(pixelNumber, pixelState)   (void) `$INSTANCE_NAME`_WritePixel(pixelNumber,pixelState)
#define `$INSTANCE_NAME`_READ_PIXEL(pixelNumber)                `$INSTANCE_NAME`_ReadPixel(pixelNumber)


#if(CYDEV_CHIP_MEMBER_USED == CYDEV_CHIP_MEMBER_3A)
    #if((CYDEV_CHIP_REVISION_USED <= CYDEV_CHIP_REVISION_3A_ES2) && (`$INSTANCE_NAME`_TD_DoneInt__ES2_PATCH))
        #define `$INSTANCE_NAME`_ISR_PATCH() _nop_(); _nop_(); _nop_(); _nop_(); _nop_(); _nop_(); _nop_(); _nop_();
    #endif /* (CYDEV_CHIP_REVISION_USED <= CYDEV_CHIP_REVISION_3A_ES2) */
#endif /* (CYDEV_CHIP_MEMBER_USED == CYDEV_CHIP_MEMBER_3A) */

/***************************************
               Macros          
***************************************/

/* Calculates pixel location in the frame buffer. */
#define `$INSTANCE_NAME`_FIND_PIXEL(port, pin, row)             (uint16) (((((row) << 7u) + ((port) << 3u)) << 1u) + (pin))

#define `$INSTANCE_NAME`_EXTRACT_ROW(pixel)                     ((uint8) ((pixel & `$INSTANCE_NAME`_ROW_MASK) >> \
                                                                            `$INSTANCE_NAME`_ROW_SHIFT))
#define `$INSTANCE_NAME`_EXTRACT_PORT(pixel)                    ((uint8) ((pixel & `$INSTANCE_NAME`_BYTE_MASK) >> \
                                                                            `$INSTANCE_NAME`_BYTE_SHIFT))
#define `$INSTANCE_NAME`_EXTRACT_PIN(pixel)                     ((uint8) (pixel & `$INSTANCE_NAME`_BIT_MASK))


/***************************************
*           API Constants
***************************************/

#define `$INSTANCE_NAME`_DMA_ADDRESS_MASK          (0x0000FFFFu)

#define `$INSTANCE_NAME`_LOW_POWER                 (0x01u)

/* Frame buffer row length */
#define `$INSTANCE_NAME`_ROW_LENGTH                (0x10u)
#define `$INSTANCE_NAME`_BUFFER_LENGTH             (`$INSTANCE_NAME`_COMM_NUM * `$INSTANCE_NAME`_ROW_LENGTH) 

#define `$INSTANCE_NAME`_LOW_RANGE                 (0x00u)
#define `$INSTANCE_NAME`_HI_RANGE                  (0x01u)

#define `$INSTANCE_NAME`_INVERT_BIT_MASK           (0x04u)
#define `$INSTANCE_NAME`_SLEEP_ENABLE              (0x01u)
#define `$INSTANCE_NAME`_SLEEP_BIT_MASK            (0xF5u)

#define `$INSTANCE_NAME`_INVERT_SHIFT              (0x02u)

/* LCD state constants */
#define `$INSTANCE_NAME`_LCD_STATE_DISABLED        (0x00u)
#define `$INSTANCE_NAME`_LCD_STATE_ENABLED         (0x01u)

/* Return pixel state constants */
#define `$INSTANCE_NAME`_NORMAL_STATE              (0x00u)
#define `$INSTANCE_NAME`_INVERTED_STATE            (0x01u)
#define `$INSTANCE_NAME`_STATE_MASK                (0x01u)

/* Number of pixels for different kind of LCDs */
#define `$INSTANCE_NAME`_7SEG_PIX_NUM              (0x07u)
#define `$INSTANCE_NAME`_14SEG_PIX_NUM             (0x0Eu)
#define `$INSTANCE_NAME`_16SEG_PIX_NUM             (0x10u)
#define `$INSTANCE_NAME`_DM_CHAR_HEIGHT            (0x08u)
#define `$INSTANCE_NAME`_DM_CHAR_WIDTH             (0x05u)

/* API parameter pixel state constants */
#define `$INSTANCE_NAME`_PIXEL_STATE_OFF           (0x00u)
#define `$INSTANCE_NAME`_PIXEL_STATE_ON            (0x01u)
#define `$INSTANCE_NAME`_PIXEL_STATE_INVERT        (0x02u)
#define `$INSTANCE_NAME`_PIXEL_UNKNOWN_STATE       (0xFFu)

/* 0 - No leadig zeros, 1 - leadig zeros */
#define `$INSTANCE_NAME`_MODE_0                    (0x00u)
#define `$INSTANCE_NAME`_MODE_1                    (0x01u)

#define `$INSTANCE_NAME`_MAX_BUFF_ROWS             (0x10u)
#define `$INSTANCE_NAME`_ROW_BYTE_LEN              (0x10u)

#define `$INSTANCE_NAME`_ISR_NUMBER                (`$INSTANCE_NAME`_TD_DoneInt__INTC_NUMBER)
#define `$INSTANCE_NAME`_ISR_PRIORITY              (`$INSTANCE_NAME`_TD_DoneInt__INTC_PRIOR_NUM)

`$writerHPixelDef`

#define `$INSTANCE_NAME`_LCD_TD_SIZE               (`$INSTANCE_NAME`_SegLcdPort_DMA_TD_PROTO_COUNT)


/***************************************
*    Enumerated Types and Parameters
***************************************/

`#cy_declare_enum Waveform_type`

`#cy_declare_enum DriverPowerModes`

/***************************************
*    Initial Parameter Constants
***************************************/

#define `$INSTANCE_NAME`_SEG_NUM               (`$NumSegmentLines`u)
#define `$INSTANCE_NAME`_BIAS_TYPE             (`$BiasType`u)
#define `$INSTANCE_NAME`_BIAS_VOLTAGE          (`$BiasVoltage`u << 1u)
#define `$INSTANCE_NAME`_DRIVER_POWER_MODE     (`$DriverPowerMode`u)
#define `$INSTANCE_NAME`_FRAME_RATE            (`$FrameRate`u)
#define `$INSTANCE_NAME`_HI_DRIVE_TIME         (`$HiDriveTime`u + 1u)
#define `$INSTANCE_NAME`_LOW_DRIVE_INIT_TIME   (`$LowDriveInitTime`u)
#define `$INSTANCE_NAME`_LOW_DRIVE_MODE        (`$LowDriveMode`u)
#define `$INSTANCE_NAME`_WAVEFORM_TYPE         (`$WaveformType`u)
#define `$INSTANCE_NAME`_DAC_DIS_INIT_TIME     (`$DacDisInitTime`u)


/***************************************
*             Registers
***************************************/

/* LCD's fixed block registers */
#define `$INSTANCE_NAME`_LCDDAC_CONTROL_REG       (* (reg8*) `$INSTANCE_NAME`_LCD__CR0)
#define `$INSTANCE_NAME`_LCDDAC_CONTROL_PTR       ((reg8*) `$INSTANCE_NAME`_LCD__CR0)
#define `$INSTANCE_NAME`_CONTRAST_CONTROL_REG     (* (reg8*) `$INSTANCE_NAME`_LCD__CR1)
#define `$INSTANCE_NAME`_CONTRAST_CONTROL_PTR     ((reg8*) `$INSTANCE_NAME`_LCD__CR1)
#define `$INSTANCE_NAME`_DRIVER_CONTROL_REG       (* (reg8*)`$INSTANCE_NAME`_LCD__CR)
#define `$INSTANCE_NAME`_DRIVER_CONTROL_PTR       ((reg8*)`$INSTANCE_NAME`_LCD__CR)
#define `$INSTANCE_NAME`_ALIASED_AREA_PTR         ((reg32) CYDEV_IO_DR_PRT0_DR_ALIAS)
#define `$INSTANCE_NAME`_PWR_MGR_REG              (* (reg8 *) `$INSTANCE_NAME`_LCD__PM_ACT_CFG)
#define `$INSTANCE_NAME`_PWR_MGR_PTR              ((reg8 *) `$INSTANCE_NAME`_LCD__PM_ACT_CFG)
#define `$INSTANCE_NAME`_PWR_MGR_STBY_REG         (* (reg8 *) `$INSTANCE_NAME`_LCD__PM_STBY_CFG)
#define `$INSTANCE_NAME`_PWR_MGR_STBY_PTR         ((reg8 *) `$INSTANCE_NAME`_LCD__PM_STBY_CFG)

#define `$INSTANCE_NAME`_LCDDAC_SWITCH_REG0_REG   (* (reg8*)`$INSTANCE_NAME`_LCD__SW0)
#define `$INSTANCE_NAME`_LCDDAC_SWITCH_REG1_REG   (* (reg8*)`$INSTANCE_NAME`_LCD__SW1)
#define `$INSTANCE_NAME`_LCDDAC_SWITCH_REG2_REG   (* (reg8*)`$INSTANCE_NAME`_LCD__SW2)
#define `$INSTANCE_NAME`_LCDDAC_SWITCH_REG3_REG   (* (reg8*)`$INSTANCE_NAME`_LCD__SW3)
#define `$INSTANCE_NAME`_LCDDAC_SWITCH_REG4_REG   (* (reg8*)`$INSTANCE_NAME`_LCD__SW4)

#define `$INSTANCE_NAME`_LCDDAC_SWITCH_REG0_PTR   ((reg8*)`$INSTANCE_NAME`_LCD__SW0)
#define `$INSTANCE_NAME`_LCDDAC_SWITCH_REG1_PTR   ((reg8*)`$INSTANCE_NAME`_LCD__SW1)
#define `$INSTANCE_NAME`_LCDDAC_SWITCH_REG2_PTR   ((reg8*)`$INSTANCE_NAME`_LCD__SW2)
#define `$INSTANCE_NAME`_LCDDAC_SWITCH_REG3_PTR   ((reg8*)`$INSTANCE_NAME`_LCD__SW3)
#define `$INSTANCE_NAME`_LCDDAC_SWITCH_REG4_PTR   ((reg8*)`$INSTANCE_NAME`_LCD__SW4)

/* UDB registers used for generation of timming and control signals */
#define `$INSTANCE_NAME`_CONTROL_REG              (* (reg8 *) `$INSTANCE_NAME`_bLCDDSD_ctrlreg__CONTROL_REG)
#define `$INSTANCE_NAME`_CNT_DELAY_REG            (* (reg8 *) `$INSTANCE_NAME`_bLCDDSD_bSegLCDdp_u0__D0_REG)
#define `$INSTANCE_NAME`_EN_HI_DELAY_REG          (* (reg8 *) `$INSTANCE_NAME`_bLCDDSD_bSegLCDdp_u0__D1_REG)
#define `$INSTANCE_NAME`_CNT_PERIOD_REG           (* (reg8 *) `$INSTANCE_NAME`_bLCDDSD_bSegLCDdp_u0__A1_REG)

#if(`$INSTANCE_NAME`_WAVEFORM_TYPE == `$INSTANCE_NAME`__TYPE_B)

    #define `$INSTANCE_NAME`_FRAME_CNT7_PERIOD_REG    (* (reg8 *) `$INSTANCE_NAME`_bLCDDSD_TypeB_DivCounter__PERIOD_REG) 
    #define `$INSTANCE_NAME`_FRAME_CNT7_CTRL_REG  \
                                             (* (reg8 *) `$INSTANCE_NAME`_bLCDDSD_TypeB_DivCounter__CONTROL_AUX_CTL_REG)   
#endif /* `$INSTANCE_NAME`_WAVEFORM_TYPE == `$INSTANCE_NAME`__TYPE_B */
                                                     
#if(`$INSTANCE_NAME`_DRIVER_POWER_MODE == `$INSTANCE_NAME`_LOW_POWER)
    #define `$INSTANCE_NAME`_LOW_DRIVE_DELAY_REG  (* (reg8 *) `$INSTANCE_NAME`_bLCDDSD_LowPower_bLowPowerdp_u0__D0_REG)
#endif /* `$INSTANCE_NAME`_DRIVER_POWER_MODE */

/* PTR version of UDB registers */
#define `$INSTANCE_NAME`_CONTROL_PTR           ((reg8 *) `$INSTANCE_NAME`_bLCDDSD_ctrlreg__CONTROL_REG)
#define `$INSTANCE_NAME`_CNT_DELAY_PTR         ((reg8 *) `$INSTANCE_NAME`_bLCDDSD_bSegLCDdp_u0__D0_REG)
#define `$INSTANCE_NAME`_EN_HI_DELAY_PTR       ((reg8 *) `$INSTANCE_NAME`_bLCDDSD_bSegLCDdp_u0__D1_REG)
#define `$INSTANCE_NAME`_CNT_PERIOD_PTR        ((reg8 *) `$INSTANCE_NAME`_bLCDDSD_bSegLCDdp_u0__A1_REG)

#if(`$INSTANCE_NAME`_WAVEFORM_TYPE == `$INSTANCE_NAME`__TYPE_B)
    #define `$INSTANCE_NAME`_FRAME_CNT7_PERIOD_PTR ((reg8 *) `$INSTANCE_NAME`_bLCDDSD_TypeB_DivCounter__PERIOD_REG) 
    #define `$INSTANCE_NAME`_FRAME_CNT7_CTRL_PTR   \
                                               ((reg8 *) `$INSTANCE_NAME`_bLCDDSD_TypeB_DivCounter__CONTROL_AUX_CTL_REG)
#endif /* `$INSTANCE_NAME`_WAVEFORM_TYPE == `$INSTANCE_NAME`__TYPE_B */

#if(`$INSTANCE_NAME`_DRIVER_POWER_MODE == `$INSTANCE_NAME`_LOW_POWER)
    #define `$INSTANCE_NAME`_LOW_DRIVE_DELAY_PTR  ((reg8 *) `$INSTANCE_NAME`_bLCDDSD_LowPower_bLowPowerdp_u0__D0_REG)
#endif /* `$INSTANCE_NAME`_DRIVER_POWER_MODE */


/***************************************
*       Register Constants
***************************************/

#define `$INSTANCE_NAME`_LCD_EN                   (`$INSTANCE_NAME`_LCD__PM_ACT_MSK)
#define `$INSTANCE_NAME`_LCD_STBY_EN              (`$INSTANCE_NAME`_LCD__PM_STBY_MSK)

#define `$INSTANCE_NAME`_BIAS_TYPE_MASK           (0xFCu)
#define `$INSTANCE_NAME`_LCDDAC_EN                (0x04u)
#define `$INSTANCE_NAME`_LCDDAC_DIS               (0x04u)

#define `$INSTANCE_NAME`_HI_RANGE_VAL             (0x02u)

#define `$INSTANCE_NAME`_CLK_ENABLE               (0x01u)
#define `$INSTANCE_NAME`_RESET                    (0x02u)
#define `$INSTANCE_NAME`_POST_RESET               (0x04u)
#define `$INSTANCE_NAME`_CNTR7_ENABLE             (0x20u)

/* Set En_hi Active state for (`$HiDriveTime`) cycles of input frequency */
#define `$INSTANCE_NAME`_EN_HI_ACT_VAL            (`$INSTANCE_NAME`_HI_DRIVE_TIME + 1u)
#define `$INSTANCE_NAME`_CNT_DELAY_VAL            (`$INSTANCE_NAME`_HI_DRIVE_TIME)

#if(`$INSTANCE_NAME`_DRIVER_POWER_MODE == `$INSTANCE_NAME`_LOW_POWER)
   
    #define `$INSTANCE_NAME`_LOW_POWER_DELAY_VAL  (`$INSTANCE_NAME`_LOW_DRIVE_INIT_TIME - 1u)
    #define `$INSTANCE_NAME`_CNT_PERIOD_VAL       (`$INSTANCE_NAME`_DAC_DIS_INIT_TIME + 1u)
    
#else
    
    /* Defines compare value for DacDisable signal */
    #define `$INSTANCE_NAME`_CNT_PERIOD_VAL       (0xFFu)

#endif /* `$INSTANCE_NAME`_DRIVER_POWER_MODE == `$INSTANCE_NAME`_LOW_POWER */

#define `$INSTANCE_NAME`_LCDDAC_VOLTAGE_SEL       (0x01u)

#endif  /* CY_SEGLCD_`$INSTANCE_NAME`_H */


/* [] END OF FILE */
