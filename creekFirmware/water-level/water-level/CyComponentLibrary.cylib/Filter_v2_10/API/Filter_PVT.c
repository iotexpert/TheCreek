/*******************************************************************************
* File Name: `$INSTANCE_NAME`_PVT.c
* Version `$CY_MAJOR_VERSION`.`$CY_MINOR_VERSION`
*
* Description:
*  This file provides source code for FILT component internal functions.
*
* Note:
*  
*******************************************************************************
* Copyright 2008-2012, Cypress Semiconductor Corporation.  All rights reserved.
* You may use this file only in accordance with the license, terms, conditions, 
* disclaimers, and limitations in the end user license agreement accompanying 
* the software package with which this file was provided.
********************************************************************************/

#include "`$INSTANCE_NAME`_PVT.h"


/*******************************************************************************
* Function Name: `$INSTANCE_NAME`_SetInterruptMode
********************************************************************************
*
* Summary:
*  Assign the events which will trigger a DFB interrupt to be triggered. 
*
* Parameters:  
*  events:  Bits [0:5] of events represent the events which will trigger DFB
*           interrupts.
*            Bit 0 - Output value ready in the holding register on channel A
*            Bit 1 - Output value ready in the holding register on channel B
*            Bit 2 - '1' written to Semaphore 0  - See Note
*            Bit 3 - '1' written to Semaphore 1  - See Note
*            Bit 4 - '1' written to Semaphore 2
*
* Return: 
*  void
*
* Note:
*  Semaphore 0 and Semaphore 1 should not be configured as both a DMA request 
*  and an interrupt event, because, after one clock cycle, the system 
*  automatically clears any semaphore which triggered a DMA request.
*
*******************************************************************************/
void `$INSTANCE_NAME`_SetInterruptMode(uint8 events) `=ReentrantKeil($INSTANCE_NAME . "_SetInterruptMode")`
{
    events &= `$INSTANCE_NAME`_EVENT_MASK;
    `$INSTANCE_NAME`_INT_CTRL_REG = events;
}


/*******************************************************************************
* Function Name: `$INSTANCE_NAME`_SetDMAMode
********************************************************************************
*
* Summary:
*  Assign the events which will trigger a DMA request for the DFB.  There are 
*  two different DMA requests that can be triggered.
*
* Parameters:  
*  events:  A set of 4 bits which configure what event, if any, triggers a DMA 
*           request for the DFB.  
*           Bits [0:1] configure the trigger for DMA Request 1
*           Bits [2:3] configure the trigger for DMA Request 2
*            DMA Request 1: 
*               0 - no request will be generated
*               1 - Output value ready in the holding register on channel A
*               2 - Semaphore 0
*               3 - Sempahore 1
*            DMA Request 2: 
*               0 - no request will be generated
*               1 - Output value ready in the holding register on channel B
*               2 - Semaphore 0
*               3 - Sempahore 1
*
* Return: 
*  void
*
* Note:
*  Semaphore 0 and Semaphore 1 should not be configured as both a DMA request 
*  and an interrupt event, because, after one clock cycle, the system 
*  automatically clears any semaphore which triggered a DMA request.
*
*******************************************************************************/
void `$INSTANCE_NAME`_SetDMAMode(uint8 events) `=ReentrantKeil($INSTANCE_NAME . "_SetDMAMode")`
{
    events &= `$INSTANCE_NAME`_DMA_CTRL_MASK;
    `$INSTANCE_NAME`_DMA_CTRL_REG = events;
}


/* [] END OF FILE */
