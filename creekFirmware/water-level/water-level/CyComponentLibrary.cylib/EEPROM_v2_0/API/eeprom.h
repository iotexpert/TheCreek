/*******************************************************************************
* File Name: `$INSTANCE_NAME`.h
* Version `$CY_MAJOR_VERSION`.`$CY_MINOR_VERSION`
*
* Description:
*  Provides the function definitions for the EEPROM APIs.
*
********************************************************************************
* Copyright 2008-2012, Cypress Semiconductor Corporation.  All rights reserved.
* You may use this file only in accordance with the license, terms, conditions, 
* disclaimers, and limitations in the end user license agreement accompanying 
* the software package with which this file was provided.
*******************************************************************************/

#if !defined(CY_EEPROM_`$INSTANCE_NAME`_H)
#define CY_EEPROM_`$INSTANCE_NAME`_H

#include "cydevice_trm.h"
#include "CyFlash.h"

#if !defined(CY_PSOC5LP)
    #error Component `$CY_COMPONENT_NAME` requires cy_boot v3.0 or later
#endif /* (CY_PSOC5LP) */


/***************************************
*        Function Prototypes
***************************************/

#if (CY_PSOC3 || CY_PSOC5LP) 
    void `$INSTANCE_NAME`_Enable(void) `=ReentrantKeil($INSTANCE_NAME . "_Enable")`;
    void `$INSTANCE_NAME`_Start(void)`=ReentrantKeil($INSTANCE_NAME . "_Start")`; 
    void `$INSTANCE_NAME`_Stop(void) `=ReentrantKeil($INSTANCE_NAME . "_Stop")`;
#endif /* (CY_PSOC3 || CY_PSOC5LP) */

cystatus `$INSTANCE_NAME`_EraseSector(uint8 sectorNumber) `=ReentrantKeil($INSTANCE_NAME . "_EraseSector")`;
cystatus `$INSTANCE_NAME`_Write(const uint8 * rowData, uint8 rowNumber) `=ReentrantKeil($INSTANCE_NAME . "_Write")`;
cystatus `$INSTANCE_NAME`_StartWrite(const uint8 * rowData, uint8 rowNumber) \
            `=ReentrantKeil($INSTANCE_NAME . "_StartWrite")`;
cystatus `$INSTANCE_NAME`_QueryWrite(void) `=ReentrantKeil($INSTANCE_NAME . "_QueryWrite")`;
cystatus `$INSTANCE_NAME`_ByteWrite(uint8 dataByte, uint8 rowNumber, uint8 byteNumber) \
            `=ReentrantKeil($INSTANCE_NAME . "_ByteWrite")`;


/****************************************
*           API Constants
****************************************/

#define `$INSTANCE_NAME`_EEPROM_SIZE    CYDEV_EE_SIZE
#define SPC_BYTE_WRITE_SIZE             0x01u

#endif /* CY_EEPROM_`$INSTANCE_NAME`_H */


/* [] END OF FILE */
