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


#include "cybtldr_loadable.h"
#include "cybtldr_common.h"

void CyBtldr_Load(void) `=ReentrantKeil("CyBtldr_Load")`
{
#if defined (WORKAROUND_OPT_XRES)
    uint8 rowBuffer[CYDEV_FLS_ROW_SIZE + CYDEV_ECC_ROW_SIZE];

	CySetTemp();
	CySetFlashEEBuffer(rowBuffer);
	CyBtldr_SetFlashRunType(CYBTLDR_START_BTLDR);
#else
	(*(uint8 CYXDATA*)CYREG_RESET_SR0) |= CYBTLDR_START_BTLDR; /* set bit to indicate we want bootloading to start */
#endif
	(*(uint8 CYXDATA*)CYREG_RESET_CR2) |= 0x01;                /* set bit to cause a software reset */
}

/* [] END OF FILE */
