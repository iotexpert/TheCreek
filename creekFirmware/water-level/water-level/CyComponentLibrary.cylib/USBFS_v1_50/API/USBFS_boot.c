/*******************************************************************************
* File Name: `$INSTANCE_NAME`_boot.c
* Version `$CY_MAJOR_VERSION`.`$CY_MINOR_VERSION`
*
* Description:
*  Boot loader API for USBFS Component.
*
*  Note:
*
********************************************************************************
* Copyright 2008-2010, Cypress Semiconductor Corporation.  All rights reserved.
* You may use this file only in accordance with the license, terms, conditions,
* disclaimers, and limitations in the end user license agreement accompanying
* the software package with which this file was provided.
********************************************************************************/

#include "`$INSTANCE_NAME`.h"

#if defined(CYDEV_BOOTLOADER_IO_COMP) && (CYDEV_BOOTLOADER_IO_COMP == CyBtldr_`$INSTANCE_NAME`)

#include "CyLib.h"


/***************************************
*    Bootloader defines
***************************************/

#define CyBtLdrStarttimer(X, T)         {universalTime = T * 10; X = 0u;}
#define CyBtLdrChecktimer(X)            ((X++ < universalTime) ? 1u : 0u)

#define BTLDR_OUT_EP      (0x01u)
#define BTLDR_IN_EP       (0x02u)


/***************************************
*    Bootloader Variables
***************************************/

uint16 universalTime = 0u;
uint8 started = 0u;


/*******************************************************************************
* Function Name: CyBtldrCommStart
********************************************************************************
*
* Summary:
*  Starts the component and enables the interupt.
*
* Parameters:
*  None.
*
* Return:
*  None.
*
* Side Effects:
*  This function starts the USB with 3V or 5V operation.
*
* Reentrant:
*  No.
*
*******************************************************************************/
void CyBtldrCommStart(void)
{
    CYGlobalIntEnable;      /* Enable Global Interrupts */

    /*Start USBFS Operation/device 0 and with 5V or 3V operation depend on Voltage Congiguration in DWR */
    `$INSTANCE_NAME`_Start(0u, `$INSTANCE_NAME`_DWR_VDDD_OPERATION);
    
    /* USB componet started, the correct enumeration will be checked in first Read operation */
    started = 1u;

}


/*******************************************************************************
* Function Name: CyBtldrCommStop.
********************************************************************************
*
* Summary:
*  Disable the component and disable the interrupt.
*
* Parameters:
*  None.
*
* Return:
*  None.
*
*******************************************************************************/
void CyBtldrCommStop(void) `=ReentrantKeil($INSTANCE_NAME . "_CyBtldrCommStop")`
{
    `$INSTANCE_NAME`_Stop();
}


/*******************************************************************************
* Function Name: CyBtldrCommReset.
********************************************************************************
*
* Summary:
*  Resets the receive and transmit communication Buffers.
*
* Parameters:
*  None.
*
* Return:
*  None.
*
* Reentrant:
*  No.
*
*******************************************************************************/
void CyBtldrCommReset(void)
{
    `$INSTANCE_NAME`_EnableOutEP(BTLDR_OUT_EP);  /* Enable the OUT endpoint */
}


/*******************************************************************************
* Function Name: CyBtldrCommWrite.
********************************************************************************
*
* Summary:
*  Allows the caller to write data to the boot loader host. The function will
*  handle polling to allow a block of data to be completely sent to the host
*  device.
*
* Parameters:
*  pData:    A pointer to the block of data to send to the device
*  size:     The number of bytes to write.
*  count:    Pointer to an unsigned short variable to write the number of
*             bytes actually written.
*  timeOut:  Number of units to wait before returning because of a timeout.
*
* Return:
*  Returns the value that best describes the problem.
*
* Reentrant:
*  No.
*
*******************************************************************************/
cystatus CyBtldrCommWrite(uint8 *pData, uint16 size, uint16 *count, uint8 timeOut)
{
    uint16 time;
    cystatus status;

    /* Enable IN transfer */
    `$INSTANCE_NAME`_LoadInEP(BTLDR_IN_EP, pData, BTLDR_SIZEOF_READ_BUFFER); 

    /* Start a timer to wait on. */
    CyBtLdrStarttimer(time, timeOut);

    /* Wait for the master to read it. */
    while((`$INSTANCE_NAME`_GetEPState(BTLDR_IN_EP) == `$INSTANCE_NAME`_IN_BUFFER_FULL) && CyBtLdrChecktimer(time))
    {
        CyDelay(1); /* 1ms delay */
    }
      
    if (`$INSTANCE_NAME`_GetEPState(BTLDR_IN_EP) == `$INSTANCE_NAME`_IN_BUFFER_FULL)
    {
        status = CYRET_INVALID_STATE;
    }
    else
    {
        *count = size;        
        status = CYRET_SUCCESS;
    }

    return(status);
}


/*******************************************************************************
* Function Name: CyBtldrCommRead.
********************************************************************************
*
* Summary:
*  Allows the caller to read data from the boot loader host. The function will
*  handle polling to allow a block of data to be completely received from the
*  host device. 
*
* Parameters:
*  pData:    A pointer to the area to store the block of data received
*             from the device.
*  size:     The number of bytes to read.
*  count:    Pointer to an unsigned short variable to write the number
*             of bytes actually read.
*  timeOut:  Number of units to wait before returning because of a timeOut.
*            Timeout is measured in 10s of ms.
*
* Return:
*  Returns the value that best describes the problem.
*
* Reentrant:
*  No.
*
*******************************************************************************/
cystatus CyBtldrCommRead(uint8 *pData, uint16 size, uint16 *count, uint8 timeOut)
{
    cystatus status;
    uint16 time;
    
    if(size > BTLDR_SIZEOF_WRITE_BUFFER) 
    {
        size = BTLDR_SIZEOF_WRITE_BUFFER;
    }
    /* Start a timer to wait on. */
    CyBtLdrStarttimer(time, timeOut);

    /* Wait on enumeration in first time */
    if(started)
    {
        /* Wait for Device to enumerate */
        while(!`$INSTANCE_NAME`_GetConfiguration() && CyBtLdrChecktimer(time))
        {
            CyDelay(1); /* 1ms delay */
        }
        /* Enable first OUT, if enumeration complete */
        if(`$INSTANCE_NAME`_GetConfiguration())
        {
            CyBtldrCommReset();
            started = 0u;
        }
    }

    /* Wait on next packet */
    while((`$INSTANCE_NAME`_GetEPState(BTLDR_OUT_EP) != `$INSTANCE_NAME`_OUT_BUFFER_FULL) && CyBtLdrChecktimer(time))
    {
        CyDelay(1); /* 1ms delay */
    }
    
    /* OUT EP has completed */
    if (`$INSTANCE_NAME`_GetEPState(BTLDR_OUT_EP) == `$INSTANCE_NAME`_OUT_BUFFER_FULL)
    {
        *count = `$INSTANCE_NAME`_ReadOutEP(BTLDR_OUT_EP, pData, size);
        `$INSTANCE_NAME`_EnableOutEP(BTLDR_OUT_EP); /* Enable next OUTt */
        status = CYRET_SUCCESS;
    }
    else
    {
        *count = 0u;
        status = CYRET_EMPTY;
    }
    return(status);
}

#endif /* End CYDEV_BOOTLOADER_IO_COMP == CyBtldr_`$INSTANCE_NAME` */


/* [] END OF FILE */
