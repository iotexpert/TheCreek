/******************************************************************************
* File Name: `$INSTANCE_NAME`_DieTemp.h
* Version `$CY_MAJOR_VERSION`.`$CY_MINOR_VERSION`
*
*  Description:
*   Defines the API to acquire the die temperature.
*
*
********************************************************************************
* Copyright 2008-2009, Cypress Semiconductor Corporation.  All rights reserved.
* You may use this file only in accordance with the license, terms, conditions, 
* disclaimers, and limitations in the end user license agreement accompanying 
* the software package with which this file was provided.
********************************************************************************/


#if !defined(__`$INSTANCE_NAME`_H__)
#define __`$INSTANCE_NAME`_H__


#include <CYTYPES.H>
#include <CYLIB.H>
#include <CYSPC.H>


cystatus `$INSTANCE_NAME`_Start(void);
cystatus `$INSTANCE_NAME`_Query(int16 * temperature);
cystatus `$INSTANCE_NAME`_GetTemp(int16 * temperature);


/* __`$INSTANCE_NAME`_H__ */
#endif






