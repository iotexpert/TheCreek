/******************************************************************************
* File Name: `$INSTANCE_NAME`_DieTemp.h
* Version `$CY_MAJOR_VERSION`.`$CY_MINOR_VERSION`
*
* Description:
*  Defines the API to acquire the die temperature.
*
*
********************************************************************************
* Copyright 2008-2010, Cypress Semiconductor Corporation.  All rights reserved.
* You may use this file only in accordance with the license, terms, conditions, 
* disclaimers, and limitations in the end user license agreement accompanying 
* the software package with which this file was provided.
********************************************************************************/


#if !defined(__`$INSTANCE_NAME`_H__)
#define __`$INSTANCE_NAME`_H__


#include <CYTYPES.H>
#include <CYLIB.H>
#include <CYSPC.H>


/***************************************
*       Function Prototypes
***************************************/

cystatus `$INSTANCE_NAME`_Start(void) `=ReentrantKeil($INSTANCE_NAME . "_Start")`;
cystatus `$INSTANCE_NAME`_Query(int16 * temperature) `=ReentrantKeil($INSTANCE_NAME . "_Query")`;
cystatus `$INSTANCE_NAME`_GetTemp(int16 * temperature)`=ReentrantKeil($INSTANCE_NAME . "_GetTemp")`;
void `$INSTANCE_NAME`_Stop(void) `=ReentrantKeil($INSTANCE_NAME . "_Stop")`;


#endif /* __`$INSTANCE_NAME`_H__ */






