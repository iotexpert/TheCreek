/*******************************************************************************
* File Name: TMP05.h
* Version 1.10
*
* Description:
*  Contains the function prototypes, constants and register definition
*  of the TMP05Intf component.
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

#if !defined(CY_TMP05_H)
#define CY_TMP05_H

#include "cyfitter.h"
#include "cytypes.h"
#include "CyLib.h"


/***************************************
*   Conditional Compilation Parameters
****************************************/

#define TMP05_CUSTOM_CONTINUOUS_MODE     (1u)
#define TMP05_CUSTOM_NUM_SENSORS         (1u)

#if (CY_PSOC3)
    #define TMP05_LO_CNT_REG         (* (reg16 *) TMP05_bTMP05Intf_1_Tmp05Timer_u0__F0_REG)
    #define TMP05_LO_CNT_PTR         (  (reg16 *) TMP05_bTMP05Intf_1_Tmp05Timer_u0__F0_REG)
    #define TMP05_HI_CNT_REG         (* (reg16 *) TMP05_bTMP05Intf_1_Tmp05Timer_u0__F1_REG)
    #define TMP05_HI_CNT_PTR         (  (reg16 *) TMP05_bTMP05Intf_1_Tmp05Timer_u0__F1_REG)
    #define TMP05_STATUS_REG         (* (reg8  *) TMP05_TMP05_STS_sts_sts_reg__STATUS_REG)
    #define TMP05_STATUS_PTR         (  (reg8  *) TMP05_TMP05_STS_sts_sts_reg__STATUS_REG)
    #define TMP05_FIFO_AUXCTL_REG    (* (reg16 *)    \
                                                    TMP05_bTMP05Intf_1_Tmp05Timer_u0__DP_AUX_CTL_REG)
    #define TMP05_FIFO_AUXCTL_PTR    (  (reg16 *)    \
                                                    TMP05_bTMP05Intf_1_Tmp05Timer_u0__DP_AUX_CTL_REG)

#else
    #define TMP05_LO_CNT_REG         (* (reg16 *) TMP05_bTMP05Intf_1_Tmp05Timer_u0__16BIT_F0_REG)
    #define TMP05_LO_CNT_PTR         (  (reg16 *) TMP05_bTMP05Intf_1_Tmp05Timer_u0__16BIT_F0_REG)
    #define TMP05_HI_CNT_REG         (* (reg16 *) TMP05_bTMP05Intf_1_Tmp05Timer_u0__16BIT_F1_REG)
    #define TMP05_HI_CNT_PTR         (  (reg16 *) TMP05_bTMP05Intf_1_Tmp05Timer_u0__16BIT_F1_REG)
    #define TMP05_STATUS_REG         (* (reg8  *) TMP05_TMP05_STS_sts_sts_reg__STATUS_REG)
    #define TMP05_STATUS_PTR         (  (reg8  *) TMP05_TMP05_STS_sts_sts_reg__STATUS_REG)
    #define TMP05_FIFO_AUXCTL_REG    (* (reg16 *)    \
                                                    TMP05_bTMP05Intf_1_Tmp05Timer_u0__16BIT_DP_AUX_CTL_REG)
    #define TMP05_FIFO_AUXCTL_PTR    (  (reg16 *)    \
                                                    TMP05_bTMP05Intf_1_Tmp05Timer_u0__16BIT_DP_AUX_CTL_REG)
#endif

/* PSoC 5 support */
#if (CY_UDB_V0)
    #define TMP05_CONTROL_REG    (* (reg8  *) TMP05_bTMP05Intf_1_AsyncCtrl_CtrlReg__CONTROL_REG)
    #define TMP05_CONTROL_PTR    (  (reg8  *) TMP05_bTMP05Intf_1_AsyncCtrl_CtrlReg__CONTROL_REG)
#else
    #define TMP05_CONTROL_REG    (* (reg8  *) TMP05_bTMP05Intf_1_SyncCtrl_CtrlReg__CONTROL_REG)
    #define TMP05_CONTROL_PTR    (  (reg8  *) TMP05_bTMP05Intf_1_SyncCtrl_CtrlReg__CONTROL_REG)
#endif /* (End CY_UDB_V0) */


/*******************************************************************************
* Variables
*******************************************************************************/
extern volatile uint8  TMP05_busyFlag;
extern uint8  TMP05_initVar;


/***************************************
*         TMP05 Function Prototypes
***************************************/
void    TMP05_Start(void) ;
void    TMP05_Stop(void) ;
void    TMP05_Init(void) ;
void    TMP05_Enable(void) ;
void    TMP05_SaveConfig(void) ;
void    TMP05_RestoreConfig(void) ;
void    TMP05_Sleep(void) ;
void    TMP05_Wakeup(void) ;
void    TMP05_Trigger(void) ;
int16   TMP05_GetTemperature (uint8 sensorNum) ;
void    TMP05_SetMode (uint8 mode) ;
uint8   TMP05_DiscoverSensors(void) ;
uint8   TMP05_ConversionStatus(void) ;


/***************************************
*       Enum Types
***************************************/

/* TMP05 status codes */
#define TMP05_STATUS_IN_PROGRESS             (0x0u)
#define TMP05_STATUS_COMPLETE                (0x1u)
#define TMP05_STATUS_ERROR                   (0x2u)

/* TMP05 Modes */
#define TMP05_MODE_CONTINUOUS                (0x1u)
#define TMP05_MODE_ONESHOT                   (0x0u)

#endif /* TMP05_H */


/* [] END OF FILE */
