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
#define CYLIB_STRING	1
#include <CYlIB.H>
#include <CYSPC.H>





cystatus CySetTemp(void);
cystatus CySetFlashEEBuffer(uint8 * buffer);
cystatus CyWriteRowData(uint8 arrayId, uint16 rowAddress, uint8 * rowData);
cystatus CyWriteRowConfig(uint8 arrayId, uint16 rowAddress, uint8 * rowECC);

void CyFlashEEActivePower(uint8 state);
void CyFlashEEStandbyPower(uint8 state);


extern uint8 dieTemperature[2];



/* __CYFLASH_H__ */
#endif















