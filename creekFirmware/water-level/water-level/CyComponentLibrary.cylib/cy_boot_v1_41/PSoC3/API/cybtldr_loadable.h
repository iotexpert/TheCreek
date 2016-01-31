/*******************************************************************************
* File Name: cybldr_loadable.h  
* Version `$CY_MAJOR_VERSION`.`$CY_MINOR_VERSION`
*
*  Description:
*   Provides an API for the Bootloadable application. The API includes a
*   single function for starting bootloader.
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



#ifndef __CYBYTLDR_LOADABLE_H__
#define __CYBYTLDR_LOADABLE_H__

#include "cydevice.h"

#if ((CYDEV_DEBUGGING_XRES) && \
	(((CYDEV_CHIP_DIE_ACTUAL == CYDEV_CHIP_DIE_LEOPARD) && (CYDEV_CHIP_REV_EXPECT == CYDEV_CHIP_REV_LEOPARD_ES2)) || \
	((CYDEV_CHIP_DIE_ACTUAL == CYDEV_CHIP_DIE_PANTHER) && (CYDEV_CHIP_REV_EXPECT == CYDEV_CHIP_REV_PANTHER_ES1))))
#define WORKAROUND_OPT_XRES 1
#endif

#define CY_META_DATA_SIZE 64

#if (defined(__C51__))
#ifndef CYCODE
#define CYCODE code
#endif
#define CY_APP_RUN_TYPE	15
#define CY_GETCODEDATA(idx) *((uint8 CYCODE *)idx)
#elif (defined(__GNUC__) || defined(__ARMCC_VERSION))
#ifndef CYCODE
#define CYCODE
#endif
#define CY_APP_RUN_TYPE	13
#define CY_GETCODEDATA(idx) *((uint8 *)(CYDEV_FLS_BASE + idx)
#endif /* __GNUC__ || __ARMCC_VERSION */

/*******************************************************************************
* Function Name: CyBtldr_Load
********************************************************************************
* Summary:
*   Begins the bootloading algorithm, downloading a new ACD image from the host.
*
* Parameters:
*   void:
*   
* Return:
*   This method will never return. It will load a new application and reset
*	the device.
*
*******************************************************************************/
extern void CyBtldr_Load(void);

#endif /* __CYBYTLDR_LOADABLE_H__ */

/* [] END OF FILE */
