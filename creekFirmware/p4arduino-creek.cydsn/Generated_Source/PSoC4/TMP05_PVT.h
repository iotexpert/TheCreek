/*******************************************************************************
* File Name: TMP05_PVT.h
* Version 1.10
*
* Description:
*  This header file contains internal definitions for the TMP05
*  component. It must be included after TMP05.h.
*
* Note:
*  None
*
********************************************************************************
* Copyright 2012-2013, Cypress Semiconductor Corporation.  All rights reserved.
* You may use this file only in accordance with the license, terms, conditions,
* disclaimers, and limitations in the end user license agreement accompanying
* the software package with which this file was provided.
*******************************************************************************/

#if !defined(CY_TMP05INTF_TMP05_PVT_H)
#define CY_TMP05INTF_TMP05_PVT_H

#include "TMP05.h"
#include "TMP05_EOC_ISR.h"


/* Check to see if required defines such as CY_PSOC5A are available */
/* They are defined starting with cy_boot v3.0 */

#if !defined (CY_PSOC5A)
    #error Component TMP05Intf_v1_10 requires cy_boot v3.0 or later
#endif /* (CY_ PSOC5A) */


/*******************************************************************************
* Internal Type defines
*******************************************************************************/

/* Structure to save state before go to sleep */
typedef struct
{
    uint8  enableState;
} TMP05_BACKUP_STRUCT;


/*******************************************************************************
* Internal variables
*******************************************************************************/
extern volatile uint16 TMP05_lo[TMP05_CUSTOM_NUM_SENSORS];
extern volatile uint16 TMP05_hi[TMP05_CUSTOM_NUM_SENSORS];
extern volatile uint8  TMP05_contMode;


/*******************************************************************************
* Internal register contents
*******************************************************************************/

/* Status Reg defines */
#define TMP05_STATUS_EOC                 (0x01u)
#define TMP05_STATUS_ERR                 (0x02u)
#define TMP05_STATUS_CLR_MASK            (0x03u)

/* Control Reg Enable */
#define TMP05_CTRL_TRIG                  (0x01u)

/* Control Reg EOC */
#define TMP05_CTRL_EOC                   (0x8u)

/* Control Reg EOC */
#define TMP05_CTRL_EOC_TRIG              (TMP05_CTRL_TRIG | TMP05_CTRL_EOC)

/* Control Reg Sensors Mask */
#define TMP05_CTRL_NUM_SNS_MASK          ((uint8)(~0x6u))

/* Component Enable */
#define TMP05_CTRL_REG_ENABLE            (0x80u)

/* Component Disable */
#define TMP05_CTRL_REG_DISABLE           ((uint8)(~TMP05_COMP_CTRL_REG_ENABLE))

/* Control Reg sensor position */
#define TMP05_CTRL_REG_SNS_SHIFT         (0x01u)

/* FIFO clear define */
#define TMP05_FIFO_CLEAR_MASK            (0x0303u)


/*******************************************************************************
* Internal constants
*******************************************************************************/
#define TMP05_SCALED_CONST_TMP1          ((int32)42100)
#define TMP05_SCALED_CONST_TMP2          ((int32)75100)


/******************************************
* Buried Interrupt Support
******************************************/
CY_ISR_PROTO(TMP05_EOC_ISR_Int);

#endif /* End CY_TMP05_PVT_H */


/* [] END OF FILE */
