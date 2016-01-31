/*******************************************************************************
* File Name: cybldr_common.h  
* Version `$CY_MAJOR_VERSION`.`$CY_MINOR_VERSION`
*
*  Description:
*   Provides an API for the common functionality shared by both Bootloader and
*   Boot Loadable applications.
*
*  Note: 
*   Documentation of the API's in this file is located in the
*   System Reference Guide provided with PSoC Creator.
*
********************************************************************************
* Copyright 2008-2010, Cypress Semiconductor Corporation.  All rights reserved.
* You may use this file only in accordance with the license, terms, conditions, 
* disclaimers, and limitations in the end user license agreement accompanying 
* the software package with which this file was provided.
********************************************************************************/

#if !defined(__CYBTLDR_COMMON_H__)
#define __CYBTLDR_COMMON_H__

#include <device.h>

#define CYBTLDR_START_APP   0x80  /* Mask used for CYREG_RESET_SR0 to indicate starting application */
#define CYBTLDR_START_BTLDR 0x40  /* Mask used for CYREG_RESET_SR0 to indicate starting bootloader */

#ifndef CYDEV_FLASH_BASE
#define CYDEV_FLASH_BASE          CYDEV_FLS_BASE
#define CYDEV_FLASH_SIZE          CYDEV_FLS_SIZE
#endif

#define CY_META_DATA_SIZE           64
#define CY_APP_CHECKSUM_OFFSET      0

#if (defined(__C51__))
#define CYAPPADDRESS                uint16
/* offset by 2 from 32 bit start because only need 16 bits */
#define CY_APP_ADDR_OFFSET          3   /* 2 bytes */
#define CY_APP_BL_LAST_ROW_OFFSET   7   /* 4 bytes */
#define CY_APP_BYTE_LEN             11  /* 4 bytes */
#define CY_APP_RUN_TYPE                15  /* 1 byte  */
#define CY_GETCODEDATA(idx)         *((uint8 CYCODE *)idx)
#define CY_GETCODEWORD(idx)         *((uint32 CYCODE *)idx)

#elif (defined(__GNUC__) || defined(__ARMCC_VERSION))
#define CYAPPADDRESS                uint32
#define CY_APP_ADDR_OFFSET          1   /* 2 bytes */
#define CY_APP_BL_LAST_ROW_OFFSET   5   /* 4 bytes */
#define CY_APP_BYTE_LEN             9   /* 4 bytes */
#define CY_APP_RUN_TYPE                13  /* 1 byte  */
#define CY_GETCODEDATA(idx)         *((uint8 *)(CYDEV_FLASH_BASE + (idx)))
#define CY_GETCODEWORD(idx)         *((uint32 *)(CYDEV_FLASH_BASE + (idx)))
#endif /* __GNUC__ || __ARMCC_VERSION */

#if ((CYDEV_DEBUGGING_XRES) && \
    (((CYDEV_CHIP_MEMBER_USED == CYDEV_CHIP_MEMBER_3A) && (CYDEV_CHIP_REVISION_USED == CYDEV_CHIP_REVISION_3A_ES2)) || \
    ((CYDEV_CHIP_MEMBER_USED == CYDEV_CHIP_MEMBER_5A) && (CYDEV_CHIP_REVISION_USED == CYDEV_CHIP_REVISION_5A_ES1))))

#define WORKAROUND_OPT_XRES 1

#endif

void CyBtldr_SetFlashRunType(uint8 runType) `=ReentrantKeil("CyBtldr_SetFlashRunType")`;

#endif /* __CYBTLDR_COMMON_H__ */

/* [] END OF FILE */
