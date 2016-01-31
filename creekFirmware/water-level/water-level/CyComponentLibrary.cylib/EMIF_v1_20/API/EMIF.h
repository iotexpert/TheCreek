/*******************************************************************************
* File Name: `$INSTANCE_NAME`.h
* Version `$CY_MAJOR_VERSION`.`$CY_MINOR_VERSION`
*
* Description:
*  This file provides constants and parameter values for the EMIF component.
*
* Note:
*  None
*
********************************************************************************
* Copyright 2011-2012, Cypress Semiconductor Corporation.  All rights reserved.
* You may use this file only in accordance with the license, terms, conditions, 
* disclaimers, and limitations in the end user license agreement accompanying 
* the software package with which this file was provided.
*******************************************************************************/

#if !defined(CY_EMIF_`$INSTANCE_NAME`_H)
#define CY_EMIF_`$INSTANCE_NAME`_H

#include "`$INSTANCE_NAME`.h"
#include "cyfitter.h"
#include "cyLib.h"


/***************************************
*   Conditional Compilation Parameters
****************************************/

/* Check to see if required defines such as CY_PSOC5A are available */
/* They are defined starting with cy_boot v3.0 */

#if !defined (CY_PSOC5A)
    #error Component `$CY_COMPONENT_NAME` requires cy_boot v3.0 or later
#endif /* (CY_ PSOC5A) */

#define `$INSTANCE_NAME`_DATA_SIZE      (`$EMIF_Data`u)
#define `$INSTANCE_NAME`_ADDR_SIZE      (`$EMIF_Addr`u)
#define `$INSTANCE_NAME`_MEM_SPEED      (`$EMIF_MemSpeed`u)
#define `$INSTANCE_NAME`_MODE           (`$EMIF_Mode`u)

/* EMIF Modes */
#define `$INSTANCE_NAME`_CUSTOM (2u)
#define `$INSTANCE_NAME`_ASYNCH (0u)
#define `$INSTANCE_NAME`_SYNCH  (1u)


/***************************************
*       Type defines
***************************************/

/* Structure to save state before go to sleep */
typedef struct _`$INSTANCE_NAME`_backupStruct
{
    uint8  enableState;
} `$INSTANCE_NAME`_BACKUP_STRUCT;


/***************************************
*        Function Prototypes
****************************************/

void `$INSTANCE_NAME`_Init(void) `=ReentrantKeil($INSTANCE_NAME . "_Init")`;
void `$INSTANCE_NAME`_Enable(void) `=ReentrantKeil($INSTANCE_NAME . "_Enable")`;
void `$INSTANCE_NAME`_Start(void) `=ReentrantKeil($INSTANCE_NAME . "_Start")`;
void `$INSTANCE_NAME`_Stop(void) `=ReentrantKeil($INSTANCE_NAME . "_Stop")`;
void `$INSTANCE_NAME`_SaveConfig(void) `=ReentrantKeil($INSTANCE_NAME . "_SaveConfig")`;
void `$INSTANCE_NAME`_RestoreConfig(void) `=ReentrantKeil($INSTANCE_NAME . "_RestoreConfig")`;
void `$INSTANCE_NAME`_Sleep(void) `=ReentrantKeil($INSTANCE_NAME . "_Sleep")`;
void `$INSTANCE_NAME`_Wakeup(void) `=ReentrantKeil($INSTANCE_NAME . "_Wakeup")`;
void `$INSTANCE_NAME`_ExtMemSleep(void) `=ReentrantKeil($INSTANCE_NAME . "_ExtMemSleep")`;
void `$INSTANCE_NAME`_ExtMemWakeup(void) `=ReentrantKeil($INSTANCE_NAME . "_ExtMemWakeup")`;


/***************************************
*    Initial Parameter Constants
***************************************/


/***************************************
*           API Constants
***************************************/

#define `$INSTANCE_NAME`_CLOCK_DIV          (`$emifClkDiv`u)
#define `$INSTANCE_NAME`_READ_WTSTATES      (`$emifRpStates`u)
#define `$INSTANCE_NAME`_WRITE_WTSTATES     (`$emifWpStates`u)


/***************************************
*             Registers
***************************************/

#define `$INSTANCE_NAME`_ENABLE_REG         (*(reg8 *)   `$INSTANCE_NAME`_EMIF_ES3__CLOCK_EN)
#define `$INSTANCE_NAME`_ENABLE_PTR         ( (reg8 *)   `$INSTANCE_NAME`_EMIF_ES3__CLOCK_EN)

#define `$INSTANCE_NAME`_MEM_TYPE_REG       (*(reg8 *)   `$INSTANCE_NAME`_EMIF_ES3__EM_TYPE)
#define `$INSTANCE_NAME`_MEM_TYPE_PTR       ( (reg8 *)   `$INSTANCE_NAME`_EMIF_ES3__EM_TYPE)

#if (`$INSTANCE_NAME`_MODE == `$INSTANCE_NAME`_SYNCH)
    #define `$INSTANCE_NAME`_MEM_PWR_REG        (*(reg8 *)   `$INSTANCE_NAME`_EMIF_ES3__MEM_DWN)
    #define `$INSTANCE_NAME`_MEM_PWR_PTR        ( (reg8 *)   `$INSTANCE_NAME`_EMIF_ES3__MEM_DWN)
#endif /* End `$INSTANCE_NAME`_MODE == `$INSTANCE_NAME`_SYNCH */

#define `$INSTANCE_NAME`_CLK_DIV_REG        (*(reg8 *)   `$INSTANCE_NAME`_EMIF_ES3__MEMCLK_DIV)
#define `$INSTANCE_NAME`_CLK_DIV_PTR        ( (reg8 *)   `$INSTANCE_NAME`_EMIF_ES3__MEMCLK_DIV)

#define `$INSTANCE_NAME`_NO_UDB_REG         (*(reg8 *)   `$INSTANCE_NAME`_EMIF_ES3__NO_UDB)
#define `$INSTANCE_NAME`_NO_UDB_PTR         ( (reg8 *)   `$INSTANCE_NAME`_EMIF_ES3__NO_UDB)

#define `$INSTANCE_NAME`_POWER_REG          (*(reg8 *)   `$INSTANCE_NAME`_EMIF_ES3__PM_ACT_CFG)
#define `$INSTANCE_NAME`_POWER_PTR          ( (reg8 *)   `$INSTANCE_NAME`_EMIF_ES3__PM_ACT_CFG)

#define `$INSTANCE_NAME`_STBY_REG           (*(reg8 *)   `$INSTANCE_NAME`_EMIF_ES3__PM_STBY_CFG)
#define `$INSTANCE_NAME`_STBY_PTR           ( (reg8 *)   `$INSTANCE_NAME`_EMIF_ES3__PM_STBY_CFG)

#define `$INSTANCE_NAME`_RD_WAIT_STATE_REG  (*(reg8 *)   `$INSTANCE_NAME`_EMIF_ES3__RP_WAIT_STATES)
#define `$INSTANCE_NAME`_RD_WAIT_STATE_PTR  ( (reg8 *)   `$INSTANCE_NAME`_EMIF_ES3__RP_WAIT_STATES)
    
#define `$INSTANCE_NAME`_WR_WAIT_STATE_REG  (*(reg8 *)   `$INSTANCE_NAME`_EMIF_ES3__WP_WAIT_STATES)
#define `$INSTANCE_NAME`_WR_WAIT_STATE_PTR  ( (reg8 *)   `$INSTANCE_NAME`_EMIF_ES3__WP_WAIT_STATES)


/***************************************
*       Register Constants
***************************************/

#define `$INSTANCE_NAME`_DEFAULT_STATE      (0x00u)
#define `$INSTANCE_NAME`_ENABLE             (0x01u)
#define `$INSTANCE_NAME`_MEM_SYNC           (0x00u)
#define `$INSTANCE_NAME`_MEM_ASYNC          (0x01u)
#define `$INSTANCE_NAME`_MODE_UDB           (0x00u)
#define `$INSTANCE_NAME`_MODE_NOUDB         (0x01u)
#define `$INSTANCE_NAME`_MEM_PWR_DOWN       (0x01u)
#define `$INSTANCE_NAME`_POWER_ON           (0x40u)


#endif  /* End CY_EMIF_`$INSTANCE_NAME`_H */


/* [] END OF FILE */
