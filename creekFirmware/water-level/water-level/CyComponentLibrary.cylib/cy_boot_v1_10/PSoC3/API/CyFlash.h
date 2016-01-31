/******************************************************************************
* File Name: CyFlash.h
* Version `$CY_MAJOR_VERSION`.`$CY_MINOR_VERSION`
*
*  Description:
*   Provides the function definitions for the FLASH/EEPROM.
*
*
********************************************************************************
* Copyright 2008-2009, Cypress Semiconductor Corporation.  All rights reserved.
* You may use this file only in accordance with the license, terms, conditions, 
* disclaimers, and limitations in the end user license agreement accompanying 
* the software package with which this file was provided.
********************************************************************************/



#if !defined(__CYFLASH_H__)
#define __CYFLASH_H__


#include <CYTYPES.H>
#define CYLIB_STRING    1
#include <CYlIB.H>
#include <CYSPC.H>


// This is not defined for arm. This should be removed when it is.
#ifndef CYDEV_FLS_SIZE
#define CYDEV_FLS_SIZE 0x00010000u
#define CYDEV_ECC_BASE 0x00080000u
#endif

#define FLASH_SIZE                      CYDEV_FLS_SIZE
#define FLASH_NUMBER_ROWS               64
#define FLASH_SIZEOF_SECTOR             (FLASH_SIZEOF_ROW * 64)
#define FLASH_NUMBER_SECTORS            (FLASH_SIZE / FLASH_SIZEOF_SECTOR)

#define EEPROM_SIZE                     CYDEV_EE_SIZE
#define EEPROM_NUMBER_ROWS              (EEPROM_EEPROM_SIZE / SIZEOF_EEPROM_ROW)
#define EEPROM_SIZEOF_SECTOR            (SIZEOF_EEPROM_ROW * 64)
#define EEPROM_NUMBER_SECTORS           (EEPROM_EEPROM_SIZE / (SIZEOF_EEPROM_ROW * 64))


cystatus CySetTemp(void);
cystatus CySetFlashEEBuffer(uint8 * buffer);
cystatus CyWriteRowData(uint8 arrayId, uint16 rowAddress, uint8 * rowData);

#if (CYDEV_ECC_ENABLE == 0 && CYDEV_CONFIGURATION_ECC == 0)
cystatus CyWriteRowConfig(uint8 arrayId, uint16 rowAddress, uint8 * rowECC);
#endif

void CyFlashEEActivePower(uint8 state);
void CyFlashEEStandbyPower(uint8 state);


extern uint8 dieTemperature[2];



/* __CYFLASH_H__ */
#endif















