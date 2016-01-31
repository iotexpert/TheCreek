/*******************************************************************************
* File Name: cybldr.h  
* Version `$CY_MAJOR_VERSION`.`$CY_MINOR_VERSION`
*
*  Description:
*   Provides an API for the Bootloader. The API includes functions for starting
*   boot loading operations, validating the application and jumping to the 
*   application.
*
*  Note: 
*   Documentation of the API's in this file is located in the
*   System Reference Guide provided with PSoC Creator.
*
********************************************************************************
* Copyright 2008-2011, Cypress Semiconductor Corporation.  All rights reserved.
* You may use this file only in accordance with the license, terms, conditions, 
* disclaimers, and limitations in the end user license agreement accompanying 
* the software package with which this file was provided.
********************************************************************************/


#if !defined(__CYBTLDR_H__)
#define __CYBTLDR_H__
#include <device.h>
#include <cybtldr_common.h>

/*******************************************************************************
* Metadata addresses and pointer defines
********************************************************************************/
#define CY_BTLDR_MD_ARRAY                   (FLASH_NUMBER_SECTORS - 1)
#define CY_BTLDR_MD_ROW                     ((FLASH_NUMBER_ROWS / FLASH_NUMBER_SECTORS) - 1)

#define CY_BTLDR_MD_BASE                    (CYDEV_FLASH_BASE + (CYDEV_FLASH_SIZE - CY_META_DATA_SIZE))

#define CY_BTLDR_MD_CHECKSUM_ADDR           (CY_BTLDR_MD_BASE   +   CY_APP_CHECKSUM_OFFSET)
#define CY_BTLDR_MD_APP_ENTRY_POINT_ADDR    (CY_BTLDR_MD_BASE   +   CY_APP_ADDR_OFFSET)
#define CY_BTLDR_MD_LAST_BLDR_ROW_ADDR      (CY_BTLDR_MD_BASE   +   CY_APP_BL_LAST_ROW_OFFSET)
#define CY_BTLDR_MD_APP_BYTE_LEN            (CY_BTLDR_MD_BASE   +   CY_APP_BYTE_LEN)
#define CY_BTLDR_MD_APP_RUN_ADDR            (CY_BTLDR_MD_BASE   +   CY_APP_RUN_TYPE)

/* Pointers to the Bootloader metadata items */
#define CY_BTLDR_P_CHECKSUM                 ((uint8  CYCODE *) CY_BTLDR_MD_CHECKSUM_ADDR)
#define CY_BTLDR_P_APP_ENTRY_POINT          ((CYAPPADDRESS CYCODE *) CY_BTLDR_MD_APP_ENTRY_POINT_ADDR)
#define CY_BTLDR_P_LAST_BLDR_ROW            ((uint16 CYCODE *) CY_BTLDR_MD_LAST_BLDR_ROW_ADDR)
#define CY_BTLDR_P_APP_BYTE_LEN             ((CYAPPADDRESS CYCODE *) CY_BTLDR_MD_APP_BYTE_LEN)
#define CY_BTLDR_P_APP_RUN_ADDR             ((uint8 CYCODE *) CY_BTLDR_MD_APP_RUN_ADDR)

/*******************************************************************************
* External References
********************************************************************************/
extern void LaunchApp(uint32 addr);
extern void CyBtldr_Start(void);
extern cystatus CyBtldr_ValidateApp(void);
extern void CyBtldr_LaunchApplication(void);

/* If using custom interface as the IO Component, user must provide these functions */
#if defined(CYDEV_BOOTLOADER_IO_COMP) && (CYDEV_BOOTLOADER_IO_COMP == CyBtldr_Custom_Interface)
    extern void CyBtldrCommStart(void);
    extern void CyBtldrCommStop (void);
    extern void CyBtldrCommReset(void);
    extern cystatus CyBtldrCommWrite(uint8* buffer, uint16 size, uint16* count, uint8 timeOut);
    extern cystatus CyBtldrCommRead (uint8* buffer, uint16 size, uint16* count, uint8 timeOut);
#endif

#endif /* __CYBTLDR_H__ */

