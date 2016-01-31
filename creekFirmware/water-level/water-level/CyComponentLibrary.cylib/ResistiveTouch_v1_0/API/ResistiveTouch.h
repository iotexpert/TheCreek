/*******************************************************************************
* File Name: `$INSTANCE_NAME`.h
* Version `$CY_MAJOR_VERSION`.`$CY_MINOR_VERSION`
*
* Description:
*  This file provides constants and parameter values for the 
*  ResistiveTouch component.
*
* Note:
*
********************************************************************************
* Copyright 2012, Cypress Semiconductor Corporation.  All rights reserved.
* You may use this file only in accordance with the license, terms, conditions, 
* disclaimers, and limitations in the end user license agreement accompanying 
* the software package with which this file was provided.
*******************************************************************************/

#if !defined(CY_RESISTIVE_TOUCH_`$INSTANCE_NAME`_H)
#define CY_RESISTIVE_TOUCH_`$INSTANCE_NAME`_H

#include "cytypes.h"
#include "cyfitter.h"
#include "cydevice_trm.h"
#include "CyLib.h"
#include "project.h"


/***************************************
*   Conditional Compilation Parameters
****************************************/

#define `$INSTANCE_NAME`_SAR_SELECT (`$ADC_Select`u)


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

void `$INSTANCE_NAME`_Start(void) `=ReentrantKeil($INSTANCE_NAME . "_Start")`;
void `$INSTANCE_NAME`_Stop(void) `=ReentrantKeil($INSTANCE_NAME . "_Stop")`;
void `$INSTANCE_NAME`_Init(void) `=ReentrantKeil($INSTANCE_NAME . "_Init")`; 
void `$INSTANCE_NAME`_Enable(void) `=ReentrantKeil($INSTANCE_NAME . "_Enable")`;
void `$INSTANCE_NAME`_ActivateX(void) `=ReentrantKeil($INSTANCE_NAME . "_ActivateX")`;
void `$INSTANCE_NAME`_ActivateY(void)`=ReentrantKeil($INSTANCE_NAME . "_ActivateY")`;
int16 `$INSTANCE_NAME`_Measure(void) `=ReentrantKeil($INSTANCE_NAME . "_Measure")`;
uint8 `$INSTANCE_NAME`_TouchDetect(void) `=ReentrantKeil($INSTANCE_NAME . "_TouchDetect")`;

void `$INSTANCE_NAME`_SaveConfig(void) `=ReentrantKeil($INSTANCE_NAME . "_SaveConfig")`;
void `$INSTANCE_NAME`_RestoreConfig(void) `=ReentrantKeil($INSTANCE_NAME . "_RestoreConfig")`;
void `$INSTANCE_NAME`_Sleep(void) `=ReentrantKeil($INSTANCE_NAME . "_Sleep")`; 
void `$INSTANCE_NAME`_Wakeup(void) `=ReentrantKeil($INSTANCE_NAME . "_Wakeup")`; 

/* Macros for emWinGraphics Library */
#define CYTOUCH_START()         `$INSTANCE_NAME`_Start() 
#define CYTOUCH_STOP()          `$INSTANCE_NAME`_Stop()
#define CYTOUCH_MEASURE()       `$INSTANCE_NAME`_Measure()
#define CYTOUCH_ACTIVATE_X()    `$INSTANCE_NAME`_ActivateX()
#define CYTOUCH_ACTIVATE_Y()    `$INSTANCE_NAME`_ActivateY()
#define CYTOUCH_TOUCHED()       `$INSTANCE_NAME`_TouchDetect()


/***************************************
*       Constants
***************************************/

/* SAR_SELECT definitions */
#define `$INSTANCE_NAME`_SAR    (1u)
#define `$INSTANCE_NAME`_DSIGMA (0u)

/* AMUX channel definitions */
#define `$INSTANCE_NAME`_AMUX_XP_CHAN   (0u)
#define `$INSTANCE_NAME`_AMUX_YP_CHAN   (1u)
#define `$INSTANCE_NAME`_AMUX_NO_CHAN   (-1)


#endif /* CY_RESIST_TOUCH_`$INSTANCE_NAME`_H */


/* [] END OF FILE */
