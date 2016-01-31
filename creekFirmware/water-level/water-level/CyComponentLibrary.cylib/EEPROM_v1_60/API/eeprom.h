/*******************************************************************************
* File Name: `$INSTANCE_NAME`.h
* Version `$CY_MAJOR_VERSION`.`$CY_MINOR_VERSION`
*
*  Description:
*   Provides the function definitions for the EEPROM API.
*
********************************************************************************
* Copyright 2008-2011, Cypress Semiconductor Corporation.  All rights reserved.
* You may use this file only in accordance with the license, terms, conditions, 
* disclaimers, and limitations in the end user license agreement accompanying 
* the software package with which this file was provided.
*******************************************************************************/


#if !defined(__`$INSTANCE_NAME`_EEPROM_H__)
#define __`$INSTANCE_NAME`_EEPROM_H__


#include <CYFLASH.H>

#define `$INSTANCE_NAME`_EEPROM_SIZE    CYDEV_EE_SIZE

#ifndef CY_PSOC3
#error Component EEPROM_v1_60 requires cy_boot v2.30 or later
#endif /* CY_PSOC3 */


/***************************************
* Function Prototypes
***************************************/
#if (CY_PSOC3_ES3 || CY_PSOC5_ES2) 
    void `$INSTANCE_NAME`_Enable(void) `=ReentrantKeil($INSTANCE_NAME . "_Enable")`;
    void `$INSTANCE_NAME`_Start(void)`=ReentrantKeil($INSTANCE_NAME . "_Start")`; 
    void `$INSTANCE_NAME`_Stop(void) `=ReentrantKeil($INSTANCE_NAME . "_Stop")`;
#endif /* (CY_PSOC3_ES3 || CY_PSOC5_ES2) */
cystatus `$INSTANCE_NAME`_EraseSector(uint8 sectorNumber) `=ReentrantKeil($INSTANCE_NAME . "_EraseSector")`;
cystatus `$INSTANCE_NAME`_Write(uint8 * rowData, uint8 rowNumber) `=ReentrantKeil($INSTANCE_NAME . "_Write")`;
cystatus `$INSTANCE_NAME`_StartWrite(uint8 * rowData, uint8 rowNumber) `=ReentrantKeil($INSTANCE_NAME . "_StartWrite")`;
cystatus `$INSTANCE_NAME`_QueryWrite(void) `=ReentrantKeil($INSTANCE_NAME . "_QueryWrite")`;


#endif /* __`$INSTANCE_NAME`_EEPROM_H__ */
