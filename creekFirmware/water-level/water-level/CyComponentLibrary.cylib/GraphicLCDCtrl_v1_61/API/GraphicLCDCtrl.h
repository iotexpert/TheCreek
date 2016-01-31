/*******************************************************************************
* File Name: `$INSTANCE_NAME`.h
* Version `$CY_MAJOR_VERSION`.`$CY_MINOR_VERSION`
*
* Description:
*  This file provides constants and parameter values and API definition for the 
*  GraphicLCDCtrl Component
*
* Note:
*
********************************************************************************
* Copyright 2008-2012, Cypress Semiconductor Corporation.  All rights reserved.
* You may use this file only in accordance with the license, terms, conditions, 
* disclaimers, and limitations in the end user license agreement accompanying 
* the software package with which this file was provided.
********************************************************************************/

#if !defined(CY_GraphicLCDCtrl_`$INSTANCE_NAME`_H)
#define CY_GraphicLCDCtrl_`$INSTANCE_NAME`_H

#include "cytypes.h"
#include "cyfitter.h"
#include "CyLib.h"

/* Check to see if required defines such as CY_PSOC3 and CY_PSOC5 are available */
/* They are defined starting with cy_boot v2.30 */
#ifndef CY_PSOC3
    #error Component `$CY_COMPONENT_NAME` requires cy_boot v2.30 or later
#endif


/*************************************** 
*   Conditional Compilation Parameters
***************************************/
 
                                    
/***************************************
*        Structure Definitions
***************************************/

/* Sleep Mode API Support */
typedef struct _`$INSTANCE_NAME`_backupStruct
{   
    uint8   enableState;
    
    /* Horizontal timing configuration */
    uint8   horizBp;        /* Back Porch    (F0) */
    uint8   horizAct;       /* Active Region (F1) */
    #if (CY_PSOC3_ES2 || CY_PSOC5_ES1)
        uint8   horizFp;    /* Front Porch (D0) */
        uint8   horizSync;  /* Sync Widht  (D1) */
    #endif /* (CY_PSOC3_ES2 || CY_PSOC5_ES1) */
    
    /* Vertical timing configuration */
    uint8   vertBp;         /* Back Porch    (F0) */    
    uint8   vertAct;        /* Active Region (F1) */
    #if (CY_PSOC3_ES2 || CY_PSOC5_ES1)
        uint8   vertFp;     /* Front Porch (D0) */
        uint8   vertSync;   /* Sync Width  (D1) */
    #endif /* (CY_PSOC3_ES2 || CY_PSOC5_ES1) */
    
    /* SRAM access configuration for panel refreshing */
    #if (CY_PSOC3_ES2 || CY_PSOC5_ES1)
        uint32 frameAddr;   /* Frame buffer address */
        uint32 lineIncr;    /* Increment at the end of a line */
    #endif /* (CY_PSOC3_ES2 || CY_PSOC5_ES1) */   
} `$INSTANCE_NAME`_BACKUP_STRUCT;


/***************************************
*        Function Prototypes            
***************************************/

void    `$INSTANCE_NAME`_Start(void)                        `=ReentrantKeil($INSTANCE_NAME . "_Start")`;
void    `$INSTANCE_NAME`_Stop(void)                         `=ReentrantKeil($INSTANCE_NAME . "_Stop")`;
void    `$INSTANCE_NAME`_Write(uint32 addr, uint16 wrData)  `=ReentrantKeil($INSTANCE_NAME . "_Write")`;
uint16  `$INSTANCE_NAME`_Read(uint32 addr)                  `=ReentrantKeil($INSTANCE_NAME . "_Read")`;
void    `$INSTANCE_NAME`_WriteFrameAddr(uint32 addr)        `=ReentrantKeil($INSTANCE_NAME . "_WriteFrameAddr")`;
uint32  `$INSTANCE_NAME`_ReadFrameAddr(void)                `=ReentrantKeil($INSTANCE_NAME . "_ReadFrameAddr")`;
void    `$INSTANCE_NAME`_WriteLineIncr(uint32 incr)         `=ReentrantKeil($INSTANCE_NAME . "_WriteLineIncr")`;
uint32  `$INSTANCE_NAME`_ReadLineIncr(void)                 `=ReentrantKeil($INSTANCE_NAME . "_ReadLineIncr")`;

void    `$INSTANCE_NAME`_Sleep(void)                        `=ReentrantKeil($INSTANCE_NAME . "_Sleep")`;
void    `$INSTANCE_NAME`_Wakeup(void)                       `=ReentrantKeil($INSTANCE_NAME . "_Wakeup")`;
void    `$INSTANCE_NAME`_Enable(void)                       `=ReentrantKeil($INSTANCE_NAME . "_Enable")`;
void    `$INSTANCE_NAME`_Init(void)                         `=ReentrantKeil($INSTANCE_NAME . "_Init")`;
void    `$INSTANCE_NAME`_SaveConfig(void)                   `=ReentrantKeil($INSTANCE_NAME . "_SaveConfig")`;
void    `$INSTANCE_NAME`_RestoreConfig(void)                `=ReentrantKeil($INSTANCE_NAME . "_RestoreConfig")`;

/* Defines to allow call these APIs by emWinGraphics */
#define CYGRAPHICS_START()              `$INSTANCE_NAME`_Start()
#define CYGRAPHICS_STOP()               `$INSTANCE_NAME`_Stop()
#define CYGRAPHICS_READ(addr)           `$INSTANCE_NAME`_Read(addr)
#define CYGRAPHICS_WRITE(addr, data)    `$INSTANCE_NAME`_Write(addr, data)
#define CYGRAPHICS_WRITE_FRAME(addr)    `$INSTANCE_NAME`_WriteFrameAddr(addr)
#define CYGRAPHICS_READ_FRAME()         `$INSTANCE_NAME`_ReadFrameAddr()
#define CYGRAPHICS_WRITE_INCR(incr)     `$INSTANCE_NAME`_WriteLineIncr(incr)
#define CYGRAPHICS_READ_INCR()          `$INSTANCE_NAME`_ReadLineIncr()


/***************************************
*           API Constants               
***************************************/

#define `$INSTANCE_NAME`_CMD_FIFO_FULL            (0x01u)
#define `$INSTANCE_NAME`_DATA_VALID               (0x02u)
#define `$INSTANCE_NAME`_HORIZ_BLANKING           (0x04u)
#define `$INSTANCE_NAME`_VERT_BLANKING            (0x08u)

#define `$INSTANCE_NAME`_READ_ADDR_MASK           (0x800000u)
#define `$INSTANCE_NAME`_WRITE_ADDR_MASK          (0x7FFFFFu)

#define `$INSTANCE_NAME`_INIT_FRAME_ADDRESS       (0x000000u)
#define `$INSTANCE_NAME`_INIT_LINE_INCR           (0x000000u)

#define `$INSTANCE_NAME`_DISABLED                 (0u)


/***************************************
*    Enumerated Types and Parameters    
***************************************/


/***************************************
*    Initial Parameter Constants        
***************************************/

/* Sync Width in dotclks. Set one less than period */
#define `$INSTANCE_NAME`_HORIZ_SYNC_WIDTH   (`$HorizSyncWidth`u - 1u)
/* Back Porch in dotclks. Set 6 less than period because the BP is split into
 * two pieces to keep random access from extending into the active period. */
#define `$INSTANCE_NAME`_HORIZ_BACK_PORCH   (`$HorizBackPorch`u - 6u)
/* Active Region in dotclks. Set as (period / 4) - 1. This allows for regions 
 * as large as 1024x1024 while only using 8 bit counters. */
#define `$INSTANCE_NAME`_HORIZ_ACTIVE_REG   ((`$HorizActiveRegion`u >> 2u) - 1u)
/* Front Porch in dotclks. Set one less than period */
#define `$INSTANCE_NAME`_HORIZ_FRONT_PORCH  (`$HorizFrontPorch`u - 1u)

/* Sync Width in lines. Set one less than period. */
#define `$INSTANCE_NAME`_VERT_SYNC_WIDTH    (`$VertSyncWidth`u - 1u)
/* Back Porch in lines. Set one less than period. */
#define `$INSTANCE_NAME`_VERT_BACK_PORCH    (`$VertBackPorch`u - 1u)
/* Active Region in lines. Set as (period / 4) - 1. This allows for regions 
 * as large as 1024x1024 while only using 8 bit counters. */
#define `$INSTANCE_NAME`_VERT_ACTIVE_REG    ((`$VertActiveRegion`u >> 2u) - 1u)
/* Front Porch in lines. Set one less than period. */
#define `$INSTANCE_NAME`_VERT_FRONT_PORCH   (`$VertFrontPorch`u - 1u)

/* Phisical line width. 
 * Used to compute the value to add at the end of each line. */
#define `$INSTANCE_NAME`_PHYS_LINE_WIDTH    (`$HorizActiveRegion`u)

/***************************************
*             Registers                 
***************************************/

/* Control register definition */
#define `$INSTANCE_NAME`_CONTROL_REG   (* (reg8 *) `$INSTANCE_NAME`_`$CtlModeReplacementString`_ControlReg__CONTROL_REG)
#define `$INSTANCE_NAME`_CONTROL_PTR   (  (reg8 *) `$INSTANCE_NAME`_`$CtlModeReplacementString`_ControlReg__CONTROL_REG)

/* Status register definition */
#define `$INSTANCE_NAME`_STATUS_REG                 (* (reg8 *) `$INSTANCE_NAME`_StatusReg__STATUS_REG)
#define `$INSTANCE_NAME`_STATUS_PTR                 (  (reg8 *) `$INSTANCE_NAME`_StatusReg__STATUS_REG)

/* Internal defines to use with emWin. For some functionality emWin requires
 * the knowledge of what interval is going on. */
#define CYGRAPHICS_IS_VBLANKING     (0u != (`$INSTANCE_NAME`_STATUS_REG & `$INSTANCE_NAME`_VERT_BLANKING))
#define CYGRAPHICS_IS_HBLANKING     (0u != (`$INSTANCE_NAME`_STATUS_REG & `$INSTANCE_NAME`_HORIZ_BLANKING))

/* Horizontal timing configuration registers definition */
#define `$INSTANCE_NAME`_HORIZ_DP_AUX_CTL_REG       (* (reg8 *) `$INSTANCE_NAME`_HorizDatapath_u0__DP_AUX_CTL_REG)
#define `$INSTANCE_NAME`_HORIZ_DP_AUX_CTL_PTR       (  (reg8 *) `$INSTANCE_NAME`_HorizDatapath_u0__DP_AUX_CTL_REG)
#define `$INSTANCE_NAME`_HORIZ_SYNC_REG             (* (reg8 *) `$INSTANCE_NAME`_HorizDatapath_u0__D1_REG)
#define `$INSTANCE_NAME`_HORIZ_SYNC_PTR             (  (reg8 *) `$INSTANCE_NAME`_HorizDatapath_u0__D1_REG)
#define `$INSTANCE_NAME`_HORIZ_BP_REG               (* (reg8 *) `$INSTANCE_NAME`_HorizDatapath_u0__F0_REG)
#define `$INSTANCE_NAME`_HORIZ_BP_PTR               (  (reg8 *) `$INSTANCE_NAME`_HorizDatapath_u0__F0_REG)
#define `$INSTANCE_NAME`_HORIZ_ACT_REG              (* (reg8 *) `$INSTANCE_NAME`_HorizDatapath_u0__F1_REG)
#define `$INSTANCE_NAME`_HORIZ_ACT_PTR              (  (reg8 *) `$INSTANCE_NAME`_HorizDatapath_u0__F1_REG)
#define `$INSTANCE_NAME`_HORIZ_FP_REG               (* (reg8 *) `$INSTANCE_NAME`_HorizDatapath_u0__D0_REG)
#define `$INSTANCE_NAME`_HORIZ_FP_PTR               (  (reg8 *) `$INSTANCE_NAME`_HorizDatapath_u0__D0_REG)

/* Vertical timing configuration registers definition */
#define `$INSTANCE_NAME`_VERT_DP_AUX_CTL_REG        (* (reg8 *) `$INSTANCE_NAME`_VertDatapath_u0__DP_AUX_CTL_REG)
#define `$INSTANCE_NAME`_VERT_DP_AUX_CTL_PTR        (  (reg8 *) `$INSTANCE_NAME`_VertDatapath_u0__DP_AUX_CTL_REG)
#define `$INSTANCE_NAME`_VERT_SYNC_REG              (* (reg8 *) `$INSTANCE_NAME`_VertDatapath_u0__D1_REG)
#define `$INSTANCE_NAME`_VERT_SYNC_PTR              (  (reg8 *) `$INSTANCE_NAME`_VertDatapath_u0__D1_REG)
#define `$INSTANCE_NAME`_VERT_BP_REG                (* (reg8 *) `$INSTANCE_NAME`_VertDatapath_u0__F0_REG)
#define `$INSTANCE_NAME`_VERT_BP_PTR                (  (reg8 *) `$INSTANCE_NAME`_VertDatapath_u0__F0_REG)
#define `$INSTANCE_NAME`_VERT_ACT_REG               (* (reg8 *) `$INSTANCE_NAME`_VertDatapath_u0__F1_REG)
#define `$INSTANCE_NAME`_VERT_ACT_PTR               (  (reg8 *) `$INSTANCE_NAME`_VertDatapath_u0__F1_REG)
#define `$INSTANCE_NAME`_VERT_FP_REG                (* (reg8 *) `$INSTANCE_NAME`_VertDatapath_u0__D0_REG)
#define `$INSTANCE_NAME`_VERT_FP_PTR                (  (reg8 *) `$INSTANCE_NAME`_VertDatapath_u0__D0_REG)

/* Frame buffer address register definition */
#define `$INSTANCE_NAME`_FRAME_BUF_LSB_PTR          (  (reg32 *) `$INSTANCE_NAME`_AddrDp0__D0_REG)

/* Increment at the and of a line register definition */
#define `$INSTANCE_NAME`_LINE_INCR_LSB_PTR          (  (reg32 *) `$INSTANCE_NAME`_AddrDp0__D1_REG)

/* Command/random memory access address register definition */ 
#define `$INSTANCE_NAME`_ADDR_LSB_PTR               (  (reg32 *) `$INSTANCE_NAME`_AddrDp0__F1_REG)

/* 16-bit input data bus. Used for data during read transaction */  
#define `$INSTANCE_NAME`_DIN_LSB_PTR                (  (reg16 *) `$INSTANCE_NAME`_LsbDp__F1_REG)

/* Lower 8 bit of the output data bus. Used for data during write transaction */
#define `$INSTANCE_NAME`_DOUT_LSB_REG               (* (reg8 *) `$INSTANCE_NAME`_LsbDp__F0_REG)
#define `$INSTANCE_NAME`_DOUT_LSB_PTR               (  (reg8 *) `$INSTANCE_NAME`_LsbDp__F0_REG)
/* Lower 8 bit of the output data bus. Used for data during write transaction */
#define `$INSTANCE_NAME`_DOUT_MSB_REG               (* (reg8 *) `$INSTANCE_NAME`_MsbDp__F0_REG)
#define `$INSTANCE_NAME`_DOUT_MSB_PTR               (  (reg8 *) `$INSTANCE_NAME`_MsbDp__F0_REG)


/***************************************
*       Register Constants              
***************************************/

#define `$INSTANCE_NAME`_FX_CLEAR                   (0x03u)
#define `$INSTANCE_NAME`_ENABLE                     (0x01u)
#define `$INSTANCE_NAME`_RESET                      (0x2u)

#endif /* CY_GraphicLCDCtrl_`$INSTANCE_NAME`_H */


/* [] END OF FILE */
