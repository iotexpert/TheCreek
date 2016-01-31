/*******************************************************************************
* File Name: `$INSTANCE_NAME`.h
* Version `$CY_MAJOR_VERSION`.`$CY_MINOR_VERSION`
*
* Description:
*  This file contains the function prototypes and constants used in the emFile
*  component.
*
* Note:
*
********************************************************************************
* Copyright 2011-2012, Cypress Semiconductor Corporation.  All rights reserved.
* You may use this file only in accordance with the license, terms, conditions,
* disclaimers, and limitations in the end user license agreement accompanying
* the software package with which this file was provided.
*******************************************************************************/

#if !defined(CY_EM_FILE_`$INSTANCE_NAME`_H)
#define CY_EM_FILE_`$INSTANCE_NAME`_H

#include "cyfitter.h"
#include "MMC_X_HW.h"


/***************************************
*   Conditional Compilation Parameters
***************************************/

/* Number of configured SD cards */
#define `$INSTANCE_NAME`_NUMBER_SD_CARDS    (`$NumberSDCards`u)

/* Max frequency in KHz */
#define `$INSTANCE_NAME`_MAX_SPI_FREQ       (`$Max_SPI_Frequency`u)

/* Enable Write Protect */
#define `$INSTANCE_NAME`_WP0_EN             (`$WP0_En`u)
#define `$INSTANCE_NAME`_WP1_EN             (`$WP1_En`u)
#define `$INSTANCE_NAME`_WP2_EN             (`$WP2_En`u)
#define `$INSTANCE_NAME`_WP3_EN             (`$WP3_En`u)


/***************************************
*        Function Prototypes
***************************************/

void `$INSTANCE_NAME`_SaveConfig(void) `=ReentrantKeil($INSTANCE_NAME . "_SaveConfig")`;
void `$INSTANCE_NAME`_RestoreConfig(void) `=ReentrantKeil($INSTANCE_NAME . "_RestoreConfig")`;
void `$INSTANCE_NAME`_Sleep(void) `=ReentrantKeil($INSTANCE_NAME . "_Sleep")`;
void `$INSTANCE_NAME`_Wakeup(void) `=ReentrantKeil($INSTANCE_NAME . "_Wakeup")`;


/***************************************
*           API Constants
***************************************/

#define `$INSTANCE_NAME`_RET_SUCCCESS       (0x01u)
#define `$INSTANCE_NAME`_RET_FAIL           (0x00u)

#endif /* CY_EM_FILE_`$INSTANCE_NAME`_H */


/* [] END OF FILE */
