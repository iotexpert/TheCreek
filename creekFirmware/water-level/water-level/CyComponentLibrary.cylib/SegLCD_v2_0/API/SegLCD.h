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
* Copyright 2008-2010, Cypress Semiconductor Corporation.  All rights reserved.
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
#include <`$INSTANCE_NAME`_Int_Clock.h>


/***************************************
*   Conditional Compilation Parameters
***************************************/

#define `$INSTANCE_NAME`_COMM_NUM            (`$NumCommonLines`u)

#define `$INSTANCE_NAME`_BIT_MASK            (0x0007u)
#define `$INSTANCE_NAME`_BYTE_MASK           (0x00F0u)
#define `$INSTANCE_NAME`_ROW_MASK            (0x0F00u)

#define `$INSTANCE_NAME`_BYTE_SHIFT          (0x04u)
#define `$INSTANCE_NAME`_ROW_SHIFT           (0x08u)

`#cy_declare_enum Waveform_type`

#define `$INSTANCE_NAME`_WAVEFORM_TYPE       (`$WaveformType`u)
                     
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

void    `$INSTANCE_NAME`_Init(void);
uint8   `$INSTANCE_NAME`_Enable(void);
uint8   `$INSTANCE_NAME`_Start(void);
void    `$INSTANCE_NAME`_Stop(void);
void    `$INSTANCE_NAME`_EnableInt(void) `=ReentrantKeil($INSTANCE_NAME . "_EnableInt")`;
void    `$INSTANCE_NAME`_DisableInt(void) `=ReentrantKeil($INSTANCE_NAME . "_DisableInt")`;
uint8   `$INSTANCE_NAME`_SetBias(uint8 biasLevel) `=ReentrantKeil($INSTANCE_NAME . "_SetBias")`;
uint8   `$INSTANCE_NAME`_WriteInvertState(uint8 invertState) `=ReentrantKeil($INSTANCE_NAME . "_WriteInvertState")`;
uint8   `$INSTANCE_NAME`_ReadInvertState(void) `=ReentrantKeil($INSTANCE_NAME . "_ReadInvertState")`;
void    `$INSTANCE_NAME`_ClearDisplay(void);
uint8   `$INSTANCE_NAME`_WritePixel(uint16 pixelNumber, uint8 pixelState);
uint8   `$INSTANCE_NAME`_ReadPixel(uint16 pixelNumber)`=ReentrantKeil($INSTANCE_NAME . "_ReadPixel")`;
void    `$INSTANCE_NAME`_Sleep(void);
void    `$INSTANCE_NAME`_Wakeup(void);
void    `$INSTANCE_NAME`_SaveConfig(void);
void    `$INSTANCE_NAME`_RestoreConfig(void) `=ReentrantKeil($INSTANCE_NAME . "_RestoreConfig")`;

`$writerHFuncDeclarations`
CY_ISR_PROTO(`$INSTANCE_NAME`_ISR);

#define   `$INSTANCE_NAME`_WRITE_PIXEL(pixelNumber,pixelState)  `$INSTANCE_NAME`_WritePixel(pixelNumber,pixelState)
#define   `$INSTANCE_NAME`_READ_PIXEL(pixelNumber)              `$INSTANCE_NAME`_ReadPixel(pixelNumber)

/* Calculates pixel location in the frame buffer. */
#define `$INSTANCE_NAME`_FIND_PIXEL(port, pin, row)             (uint16) ((((row << 7u) + (port << 3u)) << 1u) + pin)

/*  */
#define `$INSTANCE_NAME`_EXTRACT_ROW(pixel)                     (uint8) ((pixel & `$INSTANCE_NAME`_ROW_MASK) >> \
                                                                            `$INSTANCE_NAME`_ROW_SHIFT)
#define `$INSTANCE_NAME`_EXTRACT_PORT(pixel)                    (uint8) (pixelNumber & `$INSTANCE_NAME`_BYTE_MASK) >> \
                                                                            `$INSTANCE_NAME`_BYTE_SHIFT 
#define `$INSTANCE_NAME`_EXTRACT_PIN(pixel)                     (uint8) (pixelNumber & `$INSTANCE_NAME`_BIT_MASK)


/***************************************
*           API Constants
***************************************/

#if(`$INSTANCE_NAME`_PSOC5)
    #define `$INSTANCE_NAME`_DMA_ADDRESS_MASK      (0xFFFFFFFFu)
#else
    #define `$INSTANCE_NAME`_DMA_ADDRESS_MASK      (0x0000FFFFu)
#endif /* `$INSTANCE_NAME`_PSOC5 */

#define `$INSTANCE_NAME`_LOW_POWER                 (0x01u)

/* Frame buffer row length */
#define `$INSTANCE_NAME`_ROW_LENGTH                (0x10u)
#define `$INSTANCE_NAME`_BUFFER_LENGTH             (`$INSTANCE_NAME`_COMM_NUM * `$INSTANCE_NAME`_ROW_LENGTH)

#if(`$INSTANCE_NAME`_WAVEFORM_TYPE == `$INSTANCE_NAME`__TYPE_A)
    #define `$INSTANCE_NAME`_CONTROL_WRITE_COUNT   (0x02u)
#else
    #define `$INSTANCE_NAME`_CONTROL_WRITE_COUNT   (2u * `$INSTANCE_NAME`_COMM_NUM)
#endif /* (`$INSTANCE_NAME`_WAVEFORM_TYPE == `$INSTANCE_NAME`__TYPE_A) */

#define `$INSTANCE_NAME`_INVERT_BIT_MASK           (0x04u)
#define `$INSTANCE_NAME`_INVERT_SHIFT              (0x02u)

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

#define `$INSTANCE_NAME`_MODE_MASK                 (0x06u)


/***************************************
*    Enumerated Types and Parameters
***************************************/

`#cy_declare_enum DriverPowerModes_revA`

/***************************************
*    Initial Parameter Constants
***************************************/

#define `$INSTANCE_NAME`_SEG_NUM               (`$NumSegmentLines`u)
#define `$INSTANCE_NAME`_BIAS_TYPE             (`$BiasType`u)
#define `$INSTANCE_NAME`_BIAS_VOLTAGE          (`$BiasVoltage`)
#define `$INSTANCE_NAME`_POWER_MODE            (`$Mode`u)
#define `$INSTANCE_NAME`_FRAME_RATE            (`$FrameRate`u)
#define `$INSTANCE_NAME`_HI_DRIVE_TIME         (`$HiDriveTime`u)
#define `$INSTANCE_NAME`_LOW_DRIVE_TIME        (`$LowDriveTime`u)
#define `$INSTANCE_NAME`_HIDRIVE_STRENGTH      (`$HiDriveStrength`u)
#define `$INSTANCE_NAME`_LOWDRIVE_STRENGTH     (`$LowDriveStrength`u)

#if(`$INSTANCE_NAME`_POWER_MODE == `$INSTANCE_NAME`__NO_SLEEP)
    #define `$INSTANCE_NAME`_DRIVE_TIME            (`$INSTANCE_NAME`_LOW_DRIVE_TIME + `$INSTANCE_NAME`_HI_DRIVE_TIME)
#else
    #define `$INSTANCE_NAME`_DRIVE_TIME            (`$INSTANCE_NAME`_HI_DRIVE_TIME)
#endif /* `$INSTANCE_NAME`_POWER_MODE == `$INSTANCE_NAME`__NO_SLEEP */

#if(`$INSTANCE_NAME`_POWER_MODE == `$INSTANCE_NAME`__NO_SLEEP)
    
    /* Set PWM period for 255 in NoSleep mode */
    #define `$INSTANCE_NAME`_PWM_PERIOD_VAL        (0xFFu)
    /* D0 = 255 - HiDrive time */
    #define `$INSTANCE_NAME`_PWM_DRIVE_VAL         (`$INSTANCE_NAME`_PWM_PERIOD_VAL - `$INSTANCE_NAME`_HI_DRIVE_TIME)
    /* D1 = 255 - Drive time */
    #define `$INSTANCE_NAME`_PWM_LOWDRIVE_VAL      (`$INSTANCE_NAME`_PWM_PERIOD_VAL - `$INSTANCE_NAME`_DRIVE_TIME)

#else

    /* PWM period will be equal to 0 */
    #define `$INSTANCE_NAME`_PWM_PERIOD_VAL        (0x00)
    /* D0 is equal to HiDrive time, as it used as period reg for Low Power mode */
    #define `$INSTANCE_NAME`_PWM_DRIVE_VAL         (`$INSTANCE_NAME`_HI_DRIVE_TIME)
    /* Make sure D1 is 0 */
    #define `$INSTANCE_NAME`_PWM_LOWDRIVE_VAL      (0x00u)

#endif /* `$INSTANCE_NAME`_POWER_MODE == `$INSTANCE_NAME`__NO_SLEEP */


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
#define `$INSTANCE_NAME`_TIMER_CONTROL_REG        (* (reg8*) `$INSTANCE_NAME`_LCD__CFG)
#define `$INSTANCE_NAME`_TIMER_CONTROL_PTR        ((reg8*) `$INSTANCE_NAME`_LCD__CFG)
#define `$INSTANCE_NAME`_ALIASED_AREA_PTR         ((reg32) CYDEV_IO_DR_PRT0_DR_ALIAS)
#define `$INSTANCE_NAME`_PWR_MGR_REG              (* (reg8 *) `$INSTANCE_NAME`_LCD__PM_ACT_CFG)
#define `$INSTANCE_NAME`_PWR_MGR_PTR              ((reg8 *) `$INSTANCE_NAME`_LCD__PM_ACT_CFG)
#define `$INSTANCE_NAME`_PWR_MGR_STBY_REG         (* (reg8 *) `$INSTANCE_NAME`_LCD__PM_STBY_CFG)
#define `$INSTANCE_NAME`_PWR_MGR_STBY_PTR         ((reg8 *) `$INSTANCE_NAME`_LCD__PM_STBY_CFG)

#if(`$INSTANCE_NAME`_POWER_MODE == `$INSTANCE_NAME`__LOW_POWER_32XTAL)
    #define `$INSTANCE_NAME`_TM_WL_GFG_REG         (*(reg8 *) CYREG_PM_TW_CFG2)
    #define `$INSTANCE_NAME`_TM_WL_GFG_PTR         ((reg8 *) CYREG_PM_TW_CFG2)
#endif /* `$INSTANCE_NAME`_POWER_MODE == `$INSTANCE_NAME`__LOW_POWER_32XTAL */

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

/* UDB registers */
#if(`$INSTANCE_NAME`_POWER_MODE == `$INSTANCE_NAME`__NO_SLEEP)
    #define `$INSTANCE_NAME`_PWM_PERIOD_REG         (* (reg8 *) `$INSTANCE_NAME`_bLCDDSD_NoSleep_bSegLCDdp_u0__A1_REG)
    #define `$INSTANCE_NAME`_PWM_PERIOD_PTR         ((reg8 *) `$INSTANCE_NAME`_bLCDDSD_NoSleep_bSegLCDdp_u0__A1_REG)
    #define `$INSTANCE_NAME`_PWM_DRIVE_REG          (* (reg8 *) `$INSTANCE_NAME`_bLCDDSD_NoSleep_bSegLCDdp_u0__D0_REG)
    #define `$INSTANCE_NAME`_PWM_DRIVE_PTR          ((reg8 *) `$INSTANCE_NAME`_bLCDDSD_NoSleep_bSegLCDdp_u0__D0_REG)
    #define `$INSTANCE_NAME`_PWM_LODRIVE_REG        (* (reg8 *) `$INSTANCE_NAME`_bLCDDSD_NoSleep_bSegLCDdp_u0__D1_REG)
    #define `$INSTANCE_NAME`_PWM_LODRIVE_PTR        ((reg8 *) `$INSTANCE_NAME`_bLCDDSD_NoSleep_bSegLCDdp_u0__D1_REG)
#else
    #define `$INSTANCE_NAME`_PWM_PERIOD_REG         (* (reg8 *) `$INSTANCE_NAME`_bLCDDSD_LowPower_bSegLCDdp_u0__A1_REG)
    #define `$INSTANCE_NAME`_PWM_PERIOD_PTR         ((reg8 *) `$INSTANCE_NAME`_bLCDDSD_LowPower_bSegLCDdp_u0__A1_REG)
    #define `$INSTANCE_NAME`_PWM_DRIVE_REG          (* (reg8 *) `$INSTANCE_NAME`_bLCDDSD_LowPower_bSegLCDdp_u0__D0_REG)
    #define `$INSTANCE_NAME`_PWM_DRIVE_PTR          ((reg8 *) `$INSTANCE_NAME`_bLCDDSD_LowPower_bSegLCDdp_u0__D0_REG)
    #define `$INSTANCE_NAME`_PWM_LODRIVE_REG        (* (reg8 *) `$INSTANCE_NAME`_bLCDDSD_LowPower_bSegLCDdp_u0__D1_REG)
    #define `$INSTANCE_NAME`_PWM_LODRIVE_PTR        ((reg8 *) `$INSTANCE_NAME`_bLCDDSD_LowPower_bSegLCDdp_u0__D1_REG)
#endif /* `$INSTANCE_NAME`_POWER_MODE == `$INSTANCE_NAME`__NO_SLEEP */

#define `$INSTANCE_NAME`_CONTROL_REG              (* (reg8 *) `$INSTANCE_NAME`_bLCDDSD_CtrlReg__CONTROL_REG)
#define `$INSTANCE_NAME`_CONTROL_PTR              ((reg8 *) `$INSTANCE_NAME`_bLCDDSD_CtrlReg__CONTROL_REG)


/***************************************
*       Register Constants
***************************************/

/* PM Control Register bits */
#define `$INSTANCE_NAME`_LCD_EN                    (`$INSTANCE_NAME`_LCD__PM_ACT_MSK)
#define `$INSTANCE_NAME`_LCD_STBY_EN               (`$INSTANCE_NAME`_LCD__PM_STBY_MSK)

/* LCD DAC Control Register Bits */
#define `$INSTANCE_NAME`_LCDDAC_UDB_LP_EN          (0x80u)
#define `$INSTANCE_NAME`_LCDDAC_CONT_DRIVE         (0x08u)
#define `$INSTANCE_NAME`_BIAS_TYPE_MASK            (0x03u)

#define `$INSTANCE_NAME`_LCDDAC_VOLTAGE_SEL        (0x01u)
#define `$INSTANCE_NAME`_LCDDAC_CONT_DRIVE         (0x08u)
#define `$INSTANCE_NAME`_LCDDAC_CONT_DRIVE_MASK    (~0x08u)

/* LCD Driver Control Register Bits */
#define `$INSTANCE_NAME`_LCDDRV_DISPLAY_BLNK       (0x01u)
#define `$INSTANCE_NAME`_LCDDRV_MODE0_MASK         (~0x02u)
#define `$INSTANCE_NAME`_LCDDRV_MODE0_SHIFT        (0x01u)
#define `$INSTANCE_NAME`_LCDDRV_INVERT             (0x04u)
#define `$INSTANCE_NAME`_LCDDRV_PTS                (0x08u)
#define `$INSTANCE_NAME`_LCDDRV_BYPASS_EN          (0x10u)

#if(`$INSTANCE_NAME`_POWER_MODE == `$INSTANCE_NAME`__LOW_POWER_32XTAL)
    #define `$INSTANCE_NAME`_ONE_PPS_EN            (0x10u)    
#endif /* `$INSTANCE_NAME`_POWER_MODE == `$INSTANCE_NAME`__LOW_POWER_32XTAL */

#if(`$INSTANCE_NAME`_POWER_MODE != `$INSTANCE_NAME`__NO_SLEEP)

    /* LCD Timer Control Register Bits */
    #define `$INSTANCE_NAME`_TIMER_EN                  (0x01u)
    #define `$INSTANCE_NAME`_TIMER_ILO_SEL             (0x00u)
    #define `$INSTANCE_NAME`_TIMER_32XTAL_SEL          (0x02u)
    #define `$INSTANCE_NAME`_TIMER_CLK_SEL_MASK        (0x02u)
    #define `$INSTANCE_NAME`_TIMER_PERIOD_MASK         (0xFCu)
    #define `$INSTANCE_NAME`_TIMER_PERIOD_SHIFT        (0x02u)

    #define `$INSTANCE_NAME`_TIMER_PERIOD              (`$TimerPeriod`u)   

#endif /* `$INSTANCE_NAME`_POWER_MODE != `$INSTANCE_NAME`__NO_SLEEP */

/* UDB Control Register bits */
#define `$INSTANCE_NAME`_CNTL_CLK_EN               (0x01u)
#define `$INSTANCE_NAME`_CNTL_CLK_EN_MASK          (~`$INSTANCE_NAME`_CNTL_CLK_EN)
#define `$INSTANCE_NAME`_CNTL_MODE_SHIFT           (0x01u)
#define `$INSTANCE_NAME`_CNTL_MODE_MASK            (~0x06u)
#define `$INSTANCE_NAME`_CNTL_FRAME_DONE           (0x08u)
#define `$INSTANCE_NAME`_CNTL_FRAME_DONE_MASK      (~`$INSTANCE_NAME`_CNTL_FRAME_DONE)


#endif  /* CY_SEGLCD_`$INSTANCE_NAME`_H */

/* [] END OF FILE */
