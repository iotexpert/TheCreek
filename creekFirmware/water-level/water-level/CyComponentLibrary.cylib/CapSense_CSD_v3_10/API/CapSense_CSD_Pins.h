/*******************************************************************************
* File Name: `$INSTANCE_NAME`_Pins.h
* Version `$CY_MAJOR_VERSION`.`$CY_MINOR_VERSION`
*
* Description:
*  This file contains API to enable firmware control of a Pins component.
*
* Note:
*  None
*
********************************************************************************
* Copyright 2008-2011, Cypress Semiconductor Corporation.  All rights reserved.
* You may use this file only in accordance with the license, terms, conditions,
* disclaimers, and limitations in the end user license agreement accompanying
* the software package with which this file was provided.
*******************************************************************************/

#if !defined(CY_CAPSENSE_CSD_Pins_`$INSTANCE_NAME`_H)
#define CY_CAPSENSE_CSD_Pins_`$INSTANCE_NAME`_H

#include "cytypes.h"
#include "cyfitter.h"
#include "cypins.h"
#include "`$INSTANCE_NAME`.h"


/***************************************
*        Function Prototypes
***************************************/

void `$INSTANCE_NAME`_SetAllSensorsDriveMode(uint8 mode) `=ReentrantKeil($INSTANCE_NAME . "_SetAllSensorsDriveMode")`;
void `$INSTANCE_NAME`_SetAllCmodsDriveMode(uint8 mode) `=ReentrantKeil($INSTANCE_NAME . "_SetAllCmodsDriveMode")`;
#if (`$INSTANCE_NAME`_CURRENT_SOURCE == `$INSTANCE_NAME`_EXTERNAL_RB)
    void `$INSTANCE_NAME`_SetAllRbsDriveMode(uint8 mode) `=ReentrantKeil($INSTANCE_NAME . "_SetAllRbsDriveMode")`;    
#endif  /* End (`$INSTANCE_NAME`_CURRENT_SOURCE == `$INSTANCE_NAME`_EXTERNAL_RB) */   


/***************************************
*           API Constants        
***************************************/

/* Drive Modes */
#define `$INSTANCE_NAME`_DM_ALG_HIZ         (PIN_DM_ALG_HIZ)
#define `$INSTANCE_NAME`_DM_DIG_HIZ         (PIN_DM_DIG_HIZ)
#define `$INSTANCE_NAME`_DM_RES_UP          (PIN_DM_RES_UP)
#define `$INSTANCE_NAME`_DM_RES_DWN         (PIN_DM_RES_DWN)
#define `$INSTANCE_NAME`_DM_OD_LO           (PIN_DM_OD_LO)
#define `$INSTANCE_NAME`_DM_OD_HI           (PIN_DM_OD_HI)
#define `$INSTANCE_NAME`_DM_STRONG          (PIN_DM_STRONG)
#define `$INSTANCE_NAME`_DM_RES_UPDWN       (PIN_DM_RES_UPDWN)

/* PC registers defines for sensors */
`$writerHpinsDefines`

#endif /* End (CY_CAPSENSE_CSD_Pins_`$INSTANCE_NAME`_H) */


/* [] END OF FILE */
