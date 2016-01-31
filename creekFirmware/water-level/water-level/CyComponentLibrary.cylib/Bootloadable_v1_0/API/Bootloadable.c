/*******************************************************************************
* File Name: `$INSTANCE_NAME`.c
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
*******************************************************************************/

#include "`$INSTANCE_NAME`.h"


/*******************************************************************************
* Function Name: `$INSTANCE_NAME`_Load
********************************************************************************
* Summary:
*  Begins the bootloading algorithm, downloading a new ACD image from the host.
*
* Parameters:  
*  None
*
* Returns:
*  This method will never return. It will load a new application and reset
*  the device.
*
*******************************************************************************/
void `$INSTANCE_NAME`_Load(void) `=ReentrantKeil("`$INSTANCE_NAME`_Load")`
{
    #if defined (WORKAROUND_OPT_XRES)

        uint8 rowBuffer[CYDEV_FLS_ROW_SIZE + CYDEV_ECC_ROW_SIZE];

        /* Either way we need to reset and run the bootloader */
        (void) CySetTemp();
        (void) CySetFlashEEBuffer(rowBuffer);

    #endif  /* defined (WORKAROUND_OPT_XRES) */
    
    CYBTDLR_SET_RUN_TYPE(`$INSTANCE_NAME`_START_BTLDR);

    `$INSTANCE_NAME`_SOFTWARE_RESET;
}


/*******************************************************************************
* Function Name: `$INSTANCE_NAME`_SetFlashByte
********************************************************************************
* Summary:
*  Sets byte at specified address in Flash.
*
* Parameters:  
*  None
*
* Returns:
*  None
*
*******************************************************************************/
void `$INSTANCE_NAME`_SetFlashByte(uint32 address, uint8 runType) `=ReentrantKeil("`$INSTANCE_NAME`_SetFlashByte")`
{
    uint32 flsAddr = address - CYDEV_FLASH_BASE;
    uint8 rowData[CYDEV_FLS_ROW_SIZE];

    #if !(CY_PSOC4)
        uint8 arrayId = (flsAddr / CYDEV_FLS_SECTOR_SIZE);
    #endif  /* !(CY_PSOC4) */

    uint16 rowNum = ((flsAddr % CYDEV_FLS_SECTOR_SIZE) / CYDEV_FLS_ROW_SIZE);
    uint32 baseAddr = address - (address % CYDEV_FLS_ROW_SIZE);
    uint16 idx;


    for (idx = 0; idx < CYDEV_FLS_ROW_SIZE; idx++)
    {
        rowData[idx] = `$INSTANCE_NAME`_GET_CODE_DATA(baseAddr + idx);
    }
    rowData[address % CYDEV_FLS_ROW_SIZE] = runType;

    #if(CY_PSOC4)
        CySysFlashWriteRow(rowNum, rowData);
    #else
        CyWriteRowData(arrayId, rowNum, rowData);
    #endif  /* (CY_PSOC4) */
}

/* [] END OF FILE */
