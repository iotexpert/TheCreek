/*******************************************************************************
* File Name: cybldr_common.c
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
* Copyright 2008-2011, Cypress Semiconductor Corporation.  All rights reserved.
* You may use this file only in accordance with the license, terms, conditions, 
* disclaimers, and limitations in the end user license agreement accompanying 
* the software package with which this file was provided.
********************************************************************************/

#include <cybtldr_common.h>

void CyBtldr_SetFlashRunType(uint8 runType) `=ReentrantKeil("CyBtldr_SetFlashRunType")`
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

/* [] END OF FILE */
