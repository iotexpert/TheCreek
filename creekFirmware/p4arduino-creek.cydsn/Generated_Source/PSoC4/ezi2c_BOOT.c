/*******************************************************************************
* File Name: ezi2c_BOOT.c
* Version 3.10
*
* Description:
*  This file provides the source code of the bootloader communication APIs
*  for the SCB Component Unconfigured mode.
*
* Note:
*
********************************************************************************
* Copyright 2013-2015, Cypress Semiconductor Corporation.  All rights reserved.
* You may use this file only in accordance with the license, terms, conditions,
* disclaimers, and limitations in the end user license agreement accompanying
* the software package with which this file was provided.
*******************************************************************************/

#include "ezi2c_BOOT.h"

#if defined(CYDEV_BOOTLOADER_IO_COMP) && (ezi2c_BTLDR_COMM_ENABLED) && \
                                (ezi2c_SCB_MODE_UNCONFIG_CONST_CFG)

/*******************************************************************************
* Function Name: ezi2c_CyBtldrCommStart
********************************************************************************
*
* Summary:
*  Calls the CyBtldrCommStart function of the bootloader communication
*  component for the selected mode.
*
* Parameters:
*  None
*
* Return:
*  None
*
*******************************************************************************/
void ezi2c_CyBtldrCommStart(void)
{
    if (ezi2c_SCB_MODE_I2C_RUNTM_CFG)
    {
        ezi2c_I2CCyBtldrCommStart();
    }
    else if (ezi2c_SCB_MODE_EZI2C_RUNTM_CFG)
    {
        ezi2c_EzI2CCyBtldrCommStart();
    }
#if (!ezi2c_CY_SCBIP_V1)
    else if (ezi2c_SCB_MODE_SPI_RUNTM_CFG)
    {
        ezi2c_SpiCyBtldrCommStart();
    }
    else if (ezi2c_SCB_MODE_UART_RUNTM_CFG)
    {
        ezi2c_UartCyBtldrCommStart();
    }
#endif /* (!ezi2c_CY_SCBIP_V1) */
    else
    {
        /* Unknown mode: do nothing */
    }
}


/*******************************************************************************
* Function Name: ezi2c_CyBtldrCommStop
********************************************************************************
*
* Summary:
*  Calls the CyBtldrCommStop function of the bootloader communication
*  component for the selected mode.
*
* Parameters:
*  None
*
* Return:
*  None
*
*******************************************************************************/
void ezi2c_CyBtldrCommStop(void)
{
    if (ezi2c_SCB_MODE_I2C_RUNTM_CFG)
    {
        ezi2c_I2CCyBtldrCommStop();
    }
    else if (ezi2c_SCB_MODE_EZI2C_RUNTM_CFG)
    {
        ezi2c_EzI2CCyBtldrCommStop();
    }
#if (!ezi2c_CY_SCBIP_V1)
    else if (ezi2c_SCB_MODE_SPI_RUNTM_CFG)
    {
        ezi2c_SpiCyBtldrCommStop();
    }
    else if (ezi2c_SCB_MODE_UART_RUNTM_CFG)
    {
        ezi2c_UartCyBtldrCommStop();
    }
#endif /* (!ezi2c_CY_SCBIP_V1) */
    else
    {
        /* Unknown mode: do nothing */
    }
}


/*******************************************************************************
* Function Name: ezi2c_CyBtldrCommReset
********************************************************************************
*
* Summary:
*  Calls the CyBtldrCommReset function of the bootloader communication
*  component for the selected mode.
*
* Parameters:
*  None
*
* Return:
*  None
*
*******************************************************************************/
void ezi2c_CyBtldrCommReset(void)
{
    if(ezi2c_SCB_MODE_I2C_RUNTM_CFG)
    {
        ezi2c_I2CCyBtldrCommReset();
    }
    else if(ezi2c_SCB_MODE_EZI2C_RUNTM_CFG)
    {
        ezi2c_EzI2CCyBtldrCommReset();
    }
#if (!ezi2c_CY_SCBIP_V1)
    else if(ezi2c_SCB_MODE_SPI_RUNTM_CFG)
    {
        ezi2c_SpiCyBtldrCommReset();
    }
    else if(ezi2c_SCB_MODE_UART_RUNTM_CFG)
    {
        ezi2c_UartCyBtldrCommReset();
    }
#endif /* (!ezi2c_CY_SCBIP_V1) */
    else
    {
        /* Unknown mode: do nothing */
    }
}


/*******************************************************************************
* Function Name: ezi2c_CyBtldrCommRead
********************************************************************************
*
* Summary:
*  Calls the CyBtldrCommRead function of the bootloader communication
*  component for the selected mode.
*
* Parameters:
*  pData:    Pointer to storage for the block of data to be read from the
*            bootloader host
*  size:     Number of bytes to be read.
*  count:    Pointer to the variable to write the number of bytes actually
*            read.
*  timeOut:  Number of units in 10 ms to wait before returning because of a
*            timeout.
*
* Return:
*  Returns CYRET_SUCCESS if no problem was encountered or returns the value
*  that best describes the problem.
*
*******************************************************************************/
cystatus ezi2c_CyBtldrCommRead(uint8 pData[], uint16 size, uint16 * count, uint8 timeOut)
{
    cystatus status;

    if(ezi2c_SCB_MODE_I2C_RUNTM_CFG)
    {
        status = ezi2c_I2CCyBtldrCommRead(pData, size, count, timeOut);
    }
    else if(ezi2c_SCB_MODE_EZI2C_RUNTM_CFG)
    {
        status = ezi2c_EzI2CCyBtldrCommRead(pData, size, count, timeOut);
    }
#if (!ezi2c_CY_SCBIP_V1)
    else if(ezi2c_SCB_MODE_SPI_RUNTM_CFG)
    {
        status = ezi2c_SpiCyBtldrCommRead(pData, size, count, timeOut);
    }
    else if(ezi2c_SCB_MODE_UART_RUNTM_CFG)
    {
        status = ezi2c_UartCyBtldrCommRead(pData, size, count, timeOut);
    }
#endif /* (!ezi2c_CY_SCBIP_V1) */
    else
    {
        status = CYRET_INVALID_STATE; /* Unknown mode: return invalid status */
    }

    return(status);
}


/*******************************************************************************
* Function Name: ezi2c_CyBtldrCommWrite
********************************************************************************
*
* Summary:
*  Calls the CyBtldrCommWrite  function of the bootloader communication
*  component for the selected mode.
*
* Parameters:
*  pData:    Pointer to the block of data to be written to the bootloader host.
*  size:     Number of bytes to be written.
*  count:    Pointer to the variable to write the number of bytes actually
*            written.
*  timeOut:  Number of units in 10 ms to wait before returning because of a
*            timeout.
*
* Return:
*  Returns CYRET_SUCCESS if no problem was encountered or returns the value
*  that best describes the problem.
*
*******************************************************************************/
cystatus ezi2c_CyBtldrCommWrite(const uint8 pData[], uint16 size, uint16 * count, uint8 timeOut)
{
    cystatus status;

    if(ezi2c_SCB_MODE_I2C_RUNTM_CFG)
    {
        status = ezi2c_I2CCyBtldrCommWrite(pData, size, count, timeOut);
    }
    else if(ezi2c_SCB_MODE_EZI2C_RUNTM_CFG)
    {
        status = ezi2c_EzI2CCyBtldrCommWrite(pData, size, count, timeOut);
    }
#if (!ezi2c_CY_SCBIP_V1)
    else if(ezi2c_SCB_MODE_SPI_RUNTM_CFG)
    {
        status = ezi2c_SpiCyBtldrCommWrite(pData, size, count, timeOut);
    }
    else if(ezi2c_SCB_MODE_UART_RUNTM_CFG)
    {
        status = ezi2c_UartCyBtldrCommWrite(pData, size, count, timeOut);
    }
#endif /* (!ezi2c_CY_SCBIP_V1) */
    else
    {
        status = CYRET_INVALID_STATE; /* Unknown mode: return invalid status */
    }

    return(status);
}

#endif /* defined(CYDEV_BOOTLOADER_IO_COMP) && (ezi2c_BTLDR_COMM_MODE_ENABLED) */


/* [] END OF FILE */
