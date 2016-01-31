/******************************************************************************
* File Name: '$INSTANCE_NAME`_eeprom.h
* Version `$CY_MAJOR_VERSION`.`$CY_MINOR_VERSION`
*
*  Description:
*   Provides the function definitions for the EEPROM API.
*
*
********************************************************************************
* Copyright 2008-2009, Cypress Semiconductor Corporation.  All rights reserved.
* You may use this file only in accordance with the license, terms, conditions, 
* disclaimers, and limitations in the end user license agreement accompanying 
* the software package with which this file was provided.
********************************************************************************/


#if !defined(__`$INSTANCE_NAME`_EEPROM_H__)
#define __`$INSTANCE_NAME`_EEPROM_H__



#include <CYFLASH.H>

/* Size of our EEPROM. */
#define `$INSTANCE_NAME`_EEPROM_SIZE    CYDEV_EE_SIZE

cystatus `$INSTANCE_NAME`_EraseSector(uint16 address);
cystatus `$INSTANCE_NAME`_Write(uint8 * line, uint16 address);
cystatus `$INSTANCE_NAME`_StartWrite(uint8 * line, uint16 address);
cystatus `$INSTANCE_NAME`_QueryWrite(void);


/* __`$INSTANCE_NAME`_EEPROM_H__ */
#endif
