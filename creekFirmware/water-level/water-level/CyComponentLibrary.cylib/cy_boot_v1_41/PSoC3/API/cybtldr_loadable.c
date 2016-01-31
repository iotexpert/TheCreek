/*******************************************************************************
* File Name: cybldr_loadable.c
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



#include <device.h>
#include "cybtldr_loadable.h"

#ifndef CYDEV_FLASH_SIZE
#define CYDEV_FLASH_SIZE CYDEV_FLS_SIZE
#define CYDEV_FLASH_BASE CYDEV_FLS_BASE
#endif

uint8 rowBuffer[CYDEV_FLS_ROW_SIZE + CYDEV_ECC_ROW_SIZE];

void CyBtldr_SetFlashRunType(uint8 runType)
{
	uint8 rowData[CYDEV_FLS_ROW_SIZE];
	uint8 arrayId = (CYDEV_FLASH_SIZE / CYDEV_FLS_ROW_SIZE / 256) - 1;
	uint16 rowNum = (CYDEV_FLASH_SIZE / CYDEV_FLS_ROW_SIZE / (arrayId + 1)) - 1;
	uint16 idx;
	for (idx = 0; idx <= CYDEV_FLS_ROW_SIZE; ++idx)
	{
		rowData[idx] = CY_GETCODEDATA((CYDEV_FLASH_BASE + CYDEV_FLASH_SIZE - CYDEV_FLS_ROW_SIZE) + idx);
	}
	rowData[CYDEV_FLS_ROW_SIZE - CY_META_DATA_SIZE + CY_APP_RUN_TYPE] = runType;
    CyWriteRowData(arrayId, rowNum, rowData);
}

void CyBtldr_Load(void)
{
#if defined (WORKAROUND_OPT_XRES)
	CySetTemp();
	CySetFlashEEBuffer(rowBuffer);
	CyBtldr_SetFlashRunType(0x40);
#else
	(*(uint8 XDATA*)CYREG_RESET_SR0) |= 0x40; /* set bit to indicate we want bootloading to start */
#endif
	(*(uint8 XDATA*)CYREG_RESET_CR2) |= 0x01;                /* set bit to cause a software reset */
}

/* [] END OF FILE */
