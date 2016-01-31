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
* Copyright 2008-2011, Cypress Semiconductor Corporation.  All rights reserved.
* You may use this file only in accordance with the license, terms, conditions,
* disclaimers, and limitations in the end user license agreement accompanying
* the software package with which this file was provided.
********************************************************************************/

#include "`$INSTANCE_NAME`.h"

#if defined(CYDEV_BOOTLOADER_IO_COMP) && ((CYDEV_BOOTLOADER_IO_COMP == CyBtldr_`$INSTANCE_NAME`) || \
                                          (CYDEV_BOOTLOADER_IO_COMP == CyBtldr_Custom_Interface))


/***************************************
*    Bootloader defines
***************************************/

#define `$INSTANCE_NAME`_CyBtLdrStarttimer(X, T)         {`$INSTANCE_NAME`_universalTime = T * 10; X = 0u;}
#define `$INSTANCE_NAME`_CyBtLdrChecktimer(X)            ((X++ < `$INSTANCE_NAME`_universalTime) ? 1u : 0u)

#define `$INSTANCE_NAME`_BTLDR_OUT_EP      (0x01u)
#define `$INSTANCE_NAME`_BTLDR_IN_EP       (0x02u)


/***************************************
*    Bootloader Variables
***************************************/

uint16 `$INSTANCE_NAME`_universalTime = 0u;
uint8 `$INSTANCE_NAME`_started = 0u;


/*******************************************************************************
* Function Name: `$INSTANCE_NAME`_CyBtldrCommStart
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
void `$INSTANCE_NAME`_CyBtldrCommStart(void)
{
    CyGlobalIntEnable;      /* Enable Global Interrupts */

    /*Start USBFS Operation/device 0 and with 5V or 3V operation depend on Voltage Congiguration in DWR */
    `$INSTANCE_NAME`_Start(0u, `$INSTANCE_NAME`_DWR_VDDD_OPERATION);
    
    /* USB componet started, the correct enumeration will be checked in first Read operation */
    `$INSTANCE_NAME`_started = 1u;

}


/*******************************************************************************
* Function Name: `$INSTANCE_NAME`_CyBtldrCommStop.
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
void `$INSTANCE_NAME`_CyBtldrCommStop(void) `=ReentrantKeil($INSTANCE_NAME . "_CyBtldrCommStop")`
{
    `$INSTANCE_NAME`_Stop();
}


/*******************************************************************************
* Function Name: `$INSTANCE_NAME`_CyBtldrCommReset.
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
void `$INSTANCE_NAME`_CyBtldrCommReset(void)
{
    `$INSTANCE_NAME`_EnableOutEP(`$INSTANCE_NAME`_BTLDR_OUT_EP);  /* Enable the OUT endpoint */
}


/*******************************************************************************
* Function Name: `$INSTANCE_NAME`_CyBtldrCommWrite.
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
cystatus `$INSTANCE_NAME`_CyBtldrCommWrite(uint8 *pData, uint16 size, uint16 *count, uint8 timeOut) CYSMALL
{
    uint16 time;
    cystatus status;

    /* Enable IN transfer */
    `$INSTANCE_NAME`_LoadInEP(`$INSTANCE_NAME`_BTLDR_IN_EP, pData, `$INSTANCE_NAME`_BTLDR_SIZEOF_READ_BUFFER); 

    /* Start a timer to wait on. */
    `$INSTANCE_NAME`_CyBtLdrStarttimer(time, timeOut);

    /* Wait for the master to read it. */
    while((`$INSTANCE_NAME`_GetEPState(`$INSTANCE_NAME`_BTLDR_IN_EP) == `$INSTANCE_NAME`_IN_BUFFER_FULL) && \
           `$INSTANCE_NAME`_CyBtLdrChecktimer(time))
    {
        CyDelay(1); /* 1ms delay */
    }
      
    if (`$INSTANCE_NAME`_GetEPState(`$INSTANCE_NAME`_BTLDR_IN_EP) == `$INSTANCE_NAME`_IN_BUFFER_FULL)
    {
        status = CYRET_TIMEOUT;
    }
    else
    {
        *count = size;        
        status = CYRET_SUCCESS;
    }

    return(status);
}


/*******************************************************************************
* Function Name: `$INSTANCE_NAME`_CyBtldrCommRead.
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
cystatus `$INSTANCE_NAME`_CyBtldrCommRead(uint8 *pData, uint16 size, uint16 *count, uint8 timeOut) CYSMALL
{
    cystatus status;
    uint16 time;
    
    if(size > `$INSTANCE_NAME`_BTLDR_SIZEOF_WRITE_BUFFER) 
    {
        size = `$INSTANCE_NAME`_BTLDR_SIZEOF_WRITE_BUFFER;
    }
    /* Start a timer to wait on. */
    `$INSTANCE_NAME`_CyBtLdrStarttimer(time, timeOut);

    /* Wait on enumeration in first time */
    if(`$INSTANCE_NAME`_started)
    {
        /* Wait for Device to enumerate */
        while(!`$INSTANCE_NAME`_GetConfiguration() && `$INSTANCE_NAME`_CyBtLdrChecktimer(time))
        {
            CyDelay(1); /* 1ms delay */
        }
        /* Enable first OUT, if enumeration complete */
        if(`$INSTANCE_NAME`_GetConfiguration())
        {
            `$INSTANCE_NAME`_CyBtldrCommReset();
            `$INSTANCE_NAME`_started = 0u;
        }
    }

    /* Wait on next packet */
    while((`$INSTANCE_NAME`_GetEPState(`$INSTANCE_NAME`_BTLDR_OUT_EP) != `$INSTANCE_NAME`_OUT_BUFFER_FULL) && \
           `$INSTANCE_NAME`_CyBtLdrChecktimer(time))
    {
        CyDelay(1); /* 1ms delay */
    }
    
    /* OUT EP has completed */
    if (`$INSTANCE_NAME`_GetEPState(`$INSTANCE_NAME`_BTLDR_OUT_EP) == `$INSTANCE_NAME`_OUT_BUFFER_FULL)
    {
        *count = `$INSTANCE_NAME`_ReadOutEP(`$INSTANCE_NAME`_BTLDR_OUT_EP, pData, size);
        status = CYRET_SUCCESS;
    }
    else
    {
        *count = 0u;
        status = CYRET_TIMEOUT;
    }
    return(status);
}

#endif /* End CYDEV_BOOTLOADER_IO_COMP == CyBtldr_`$INSTANCE_NAME` */


/* [] END OF FILE */
