/*******************************************************************************
* File Name: `$INSTANCE_NAME`.h
* Version `$CY_MAJOR_VERSION`.`$CY_MINOR_VERSION`
*
*  Description:
*   Provides an API for the Bootloadable application. The API includes a
*   single function for starting bootloader.
*
********************************************************************************
* Copyright 2008-2012, Cypress Semiconductor Corporation.  All rights reserved.
* You may use this file only in accordance with the license, terms, conditions,
* disclaimers, and limitations in the end user license agreement accompanying
* the software package with which this file was provided.
********************************************************************************/


#ifndef CY_BOOTLOADABLE_`$INSTANCE_NAME`_H
#define CY_BOOTLOADABLE_`$INSTANCE_NAME`_H

#include "cydevice_trm.h"
#include "CyFlash.h"


/* Check to see if required defines such as CY_PSOC5LP are available */
/* They are defined starting with cy_boot v3.0 */
#if !defined (CY_PSOC5LP)
    #error Component `$CY_COMPONENT_NAME` requires cy_boot v3.0 or later
#endif /* (CY_ PSOC5X) */


#define `$INSTANCE_NAME`_START_APP      (0x80u)  /* Mask used to indicate starting application */
#define `$INSTANCE_NAME`_START_BTLDR    (0x40u)  /* Mask used to indicate starting bootloader */

#ifndef CYDEV_FLASH_BASE
    #define CYDEV_FLASH_BASE            CYDEV_FLS_BASE
    #define CYDEV_FLASH_SIZE            CYDEV_FLS_SIZE
#endif /* CYDEV_FLASH_BASE */

#define `$INSTANCE_NAME`_META_DATA_SIZE                 64
#define `$INSTANCE_NAME`_META_APP_CHECKSUM_OFFSET       0

#if(CY_PSOC3)

    #define `$INSTANCE_NAME`_APP_ADDRESS                uint16
    #define `$INSTANCE_NAME`_GET_CODE_DATA(idx)         *((uint8  CYCODE *) idx)
    #define `$INSTANCE_NAME`_GET_CODE_WORD(idx)         *((uint32 CYCODE *) idx)

    /* Offset by 2 from 32 bit start because only need 16 bits */
    #define `$INSTANCE_NAME`_META_APP_ADDR_OFFSET         3   /* 2 bytes */
    #define `$INSTANCE_NAME`_META_APP_BL_LAST_ROW_OFFSET  7   /* 4 bytes */
    #define `$INSTANCE_NAME`_META_APP_BYTE_LEN_OFFSET     11  /* 4 bytes */
    #define `$INSTANCE_NAME`_META_APP_RUN_TYPE_OFFSET     15  /* 1 byte  */

#else

    #define `$INSTANCE_NAME`_APP_ADDRESS                uint32
    #define `$INSTANCE_NAME`_GET_CODE_DATA(idx)         *((uint8  *)(CYDEV_FLASH_BASE + (idx)))
    #define `$INSTANCE_NAME`_GET_CODE_WORD(idx)         *((uint32 *)(CYDEV_FLASH_BASE + (idx)))

    #define `$INSTANCE_NAME`_META_APP_ADDR_OFFSET         1   /* 4 bytes */
    #define `$INSTANCE_NAME`_META_APP_BL_LAST_ROW_OFFSET  5   /* 4 bytes */
    #define `$INSTANCE_NAME`_META_APP_BYTE_LEN_OFFSET     9   /* 4 bytes */
    #define `$INSTANCE_NAME`_META_APP_RUN_TYPE_OFFSET     13  /* 1 byte  */

#endif /* (CY_PSOC3) */

#define `$INSTANCE_NAME`_META_APP_ACTIVE_OFFSET           16  /* 1 byte  */
#define `$INSTANCE_NAME`_META_APP_VERIFIED_OFFSET         17  /* 1 byte  */

#define `$INSTANCE_NAME`_META_APP_BL_BUILD_VER_OFFSET     18  /* 2 bytes */
#define `$INSTANCE_NAME`_META_APP_ID_OFFSET               20  /* 2 bytes */
#define `$INSTANCE_NAME`_META_APP_VER_OFFSET              22  /* 2 bytes */
#define `$INSTANCE_NAME`_META_APP_CUST_ID_OFFSET          24  /* 4 bytes */


#if ((CYDEV_DEBUGGING_XRES) && (CY_PSOC5_ES1))
    #define WORKAROUND_OPT_XRES 1
#endif  /* ((CYDEV_DEBUGGING_XRES) && (CY_PSOC5_ES1)) */


#if(CY_PSOC4)

    extern uint8 appRunType;

    #define `$INSTANCE_NAME`_SOFTWARE_RESET CY_SET_REG32(CYREG_CM0_AIRCR, 0x05FA0004)

    #define `$INSTANCE_NAME`_GET_RUN_TYPE   (((CY_GET_REG32(CYREG_RES_CAUSE) & 0x10) > 0) \
                                            ? (CY_GET_REG8(CYDEV_SRAM_BASE + (4 * (CYINT_IRQ_BASE + NUM_INTERRUPTS)))) \
                                            : 0)

    #define CYBTDLR_SET_RUN_TYPE(x)         CY_SET_REG8(CYDEV_SRAM_BASE + (4 * (CYINT_IRQ_BASE + NUM_INTERRUPTS)), (x))

#else

    #define `$INSTANCE_NAME`_SOFTWARE_RESET CY_SET_REG8(CYREG_RESET_CR2, 0x01)

    #define `$INSTANCE_NAME`_GET_RUN_TYPE   (CY_GET_REG8(CYREG_RESET_SR0) & \
                                            (`$INSTANCE_NAME`_START_BTLDR | `$INSTANCE_NAME`_START_APP))

    #if defined(WORKAROUND_OPT_XRES)
        #define CYBTDLR_SET_RUN_TYPE(x)     `$INSTANCE_NAME`_SetFlashByte(`$INSTANCE_NAME`_MD_APP_RUN_ADDR(`$INSTANCE_NAME`_activeApp), (x))
    #else
        #define CYBTDLR_SET_RUN_TYPE(x)     CY_SET_REG8(CYREG_RESET_SR0, (x))
    #endif  /* defined(WORKAROUND_OPT_XRES) */

#endif  /* (CY_PSOC4) */


#define `$INSTANCE_NAME`_SetFlashRunType(runType)        `$INSTANCE_NAME`_SetFlashByte(`$INSTANCE_NAME`_MD_APP_RUN_ADDR(0), (runType))


/***************************************
*     Function Prototypes
***************************************/
void `$INSTANCE_NAME`_SetFlashByte(uint32 address, uint8 value) `=ReentrantKeil("`$INSTANCE_NAME`_SetFlashByte")`;
extern void `$INSTANCE_NAME`_Load(void) `=ReentrantKeil("`$INSTANCE_NAME`_Load")`;


#endif /* CY_BOOTLOADABLE_`$INSTANCE_NAME`_H */


/* [] END OF FILE */
