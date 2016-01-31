/*******************************************************************************
* File Name: `$INSTANCE_NAME`_INT.c
* Version `$CY_MAJOR_VERSION`.`$CY_MINOR_VERSION`
*
* Description:
*  This file contains the code that operates during the interrupt service
*  routine.
*
********************************************************************************
* Copyright 2011-2012, Cypress Semiconductor Corporation.  All rights reserved.
* You may use this file only in accordance with the license, terms, conditions,
* disclaimers, and limitations in the end user license agreement accompanying
* the software package with which this file was provided.
*******************************************************************************/

#include "`$INSTANCE_NAME`.h"

CY_ISR(`$INSTANCE_NAME`_BLIN_ISR)
{
    l_ifc_aux_`$INSTANCE_NAME`();
}


CY_ISR(`$INSTANCE_NAME`_UART_ISR)
{
    l_ifc_rx_`$INSTANCE_NAME`();
}


/* [] END OF FILE */
