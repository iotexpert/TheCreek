/*******************************************************************************
* Copyright 2008-2009, Cypress Semiconductor Corporation.  All rights reserved.
* You may use this file only in accordance with the license, terms, conditions, 
* disclaimers, and limitations in the end user license agreement accompanying 
* the software package with which this file was provided.
********************************************************************************/



#include "cybtldr_loadable.h"

void CyBtldr_Load(void)
{
	uint8 XDATA *resetSR0;
	uint8 XDATA *resetCR2;
	resetSR0 = (uint8 XDATA *)CYDEV_RESET_SR0;
	resetCR2 = (uint8 XDATA *)CYDEV_RESET_CR2;
	*resetSR0 |= 0x40; /* set second MSB gpsw_s bit to indicate we want bootloading to start */
	*resetCR2 |= 0x01; /* set swr bit to cause a software reset */
}

/* [] END OF FILE */
