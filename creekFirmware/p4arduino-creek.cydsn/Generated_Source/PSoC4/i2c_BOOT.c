/*******************************************************************************
* File Name: i2c_BOOT.c
* Version 1.10
*
* Description:
*  This file provides the source code to the API for the bootloader
*  communication support in SCB Component.
*
* Note:
*
********************************************************************************
* Copyright 2013, Cypress Semiconductor Corporation.  All rights reserved.
* You may use this file only in accordance with the license, terms, conditions,
* disclaimers, and limitations in the end user license agreement accompanying
* the software package with which this file was provided.
*******************************************************************************/

#include "i2c.h"

#if(i2c_SCB_MODE_I2C_INC)
    #include "i2c_I2C.h"
#endif /* (i2c_SCB_MODE_I2C_INC) */

#if(i2c_SCB_MODE_EZI2C_INC)
    #include "i2c_EZI2C.h"
#endif /* (i2c_SCB_MODE_EZI2C_INC) */

#if(i2c_SCB_MODE_SPI_INC || i2c_SCB_MODE_UART_INC)
    #include "i2c_SPI_UART.h"
#endif /* (i2c_SCB_MODE_SPI_INC || i2c_SCB_MODE_UART_INC) */


#if defined(CYDEV_BOOTLOADER_IO_COMP) && ((CYDEV_BOOTLOADER_IO_COMP == CyBtldr_i2c) || \
                                          (CYDEV_BOOTLOADER_IO_COMP == CyBtldr_Custom_Interface))

/*******************************************************************************
* Function Name: i2c_CyBtldrCommStart
********************************************************************************
*
* Summary:
*  Calls Start function fucntion of the bootloader communication component for
*  selected mode.
*
* Parameters:
*  None
*
* Return:
*  None
*
*******************************************************************************/
void i2c_CyBtldrCommStart(void)
{
    #if(i2c_SCB_MODE_UNCONFIG_CONST_CFG)
        if(i2c_SCB_MODE_I2C_RUNTM_CFG)
        {
            i2c_I2CCyBtldrCommStart();
        }
        else if(i2c_SCB_MODE_SPI_RUNTM_CFG)
        {
            i2c_SpiCyBtldrCommStart();
        }
        else if(i2c_SCB_MODE_UART_RUNTM_CFG)
        {
            i2c_UartCyBtldrCommStart();
        }
        else if(i2c_SCB_MODE_EZI2C_RUNTM_CFG)
        {
             i2c_EzI2CCyBtldrCommStart();
        }
        else
        {
            /* Unknown mode: do nothing */
        }
    #elif(i2c_SCB_MODE_I2C_CONST_CFG)
        i2c_I2CCyBtldrCommStart();

    #elif(i2c_SCB_MODE_SPI_CONST_CFG)
        i2c_SpiCyBtldrCommStart();

    #elif(i2c_SCB_MODE_UART_CONST_CFG)
        i2c_UartCyBtldrCommStart();

    #elif(i2c_SCB_MODE_EZI2C_CONST_CFG)
        i2c_EzI2CCyBtldrCommStart();

    #else
        /* Do nothing */

    #endif /* (i2c_SCB_MODE_UNCONFIG_CONST_CFG) */
}


/*******************************************************************************
* Function Name: i2c_CyBtldrCommStop
********************************************************************************
*
* Summary:
*  Calls Stop function fucntion of the bootloader communication component for
*  selected mode.
*
* Parameters:
*  None
*
* Return:
*  None
*
*******************************************************************************/
void i2c_CyBtldrCommStop(void)
{
    #if(i2c_SCB_MODE_UNCONFIG_CONST_CFG)
        if(i2c_SCB_MODE_I2C_RUNTM_CFG)
        {
            i2c_I2CCyBtldrCommStop();
        }
        else if(i2c_SCB_MODE_SPI_RUNTM_CFG)
        {
            i2c_SpiCyBtldrCommStop();
        }
        else if(i2c_SCB_MODE_UART_RUNTM_CFG)
        {
            i2c_UartCyBtldrCommStop();
        }
        else if(i2c_SCB_MODE_EZI2C_RUNTM_CFG)
        {
            i2c_EzI2CCyBtldrCommStop();
        }
        else
        {
            /* Unknown mode: do nothing */
        }
    #elif(i2c_SCB_MODE_I2C_CONST_CFG)
        i2c_I2CCyBtldrCommStop();

    #elif(i2c_SCB_MODE_SPI_CONST_CFG)
        i2c_SpiCyBtldrCommStop();

    #elif(i2c_SCB_MODE_UART_CONST_CFG)
        i2c_UartCyBtldrCommStop();

    #elif(i2c_SCB_MODE_EZI2C_CONST_CFG)
        i2c_EzI2CCyBtldrCommStop();

    #else
        /* Do nothing */

    #endif /* (i2c_SCB_MODE_UNCONFIG_CONST_CFG) */
}


/*******************************************************************************
* Function Name: i2c_CyBtldrCommReset
********************************************************************************
*
* Summary:
*  Calls reset function fucntion of the bootloader communication component for
*  selected mode.
*
* Parameters:
*  None
*
* Return:
*  None
*
*******************************************************************************/
void i2c_CyBtldrCommReset(void)
{
    #if(i2c_SCB_MODE_UNCONFIG_CONST_CFG)
        if(i2c_SCB_MODE_I2C_RUNTM_CFG)
        {
            i2c_I2CCyBtldrCommReset();
        }
        else if(i2c_SCB_MODE_SPI_RUNTM_CFG)
        {
            i2c_SpiCyBtldrCommReset();
        }
        else if(i2c_SCB_MODE_UART_RUNTM_CFG)
        {
            i2c_UartCyBtldrCommReset();
        }
        else if(i2c_SCB_MODE_EZI2C_RUNTM_CFG)
        {
            i2c_EzI2CCyBtldrCommReset();
        }
        else
        {
            /* Unknown mode: do nothing */
        }
    #elif(i2c_SCB_MODE_I2C_CONST_CFG)
        i2c_I2CCyBtldrCommReset();

    #elif(i2c_SCB_MODE_SPI_CONST_CFG)
        i2c_SpiCyBtldrCommReset();

    #elif(i2c_SCB_MODE_UART_CONST_CFG)
        i2c_UartCyBtldrCommReset();

    #elif(i2c_SCB_MODE_EZI2C_CONST_CFG)
        i2c_EzI2CCyBtldrCommReset();

    #else
        /* Do nothing */

    #endif /* (i2c_SCB_MODE_UNCONFIG_CONST_CFG) */
}


/*******************************************************************************
* Function Name: i2c_CyBtldrCommRead
********************************************************************************
*
* Summary:
*  Calls read fucntion of the bootloader communication component for selected
*  mode.
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
cystatus i2c_CyBtldrCommRead(uint8 pData[], uint16 size, uint16 * count, uint8 timeOut)
{
    cystatus status;

    #if(i2c_SCB_MODE_UNCONFIG_CONST_CFG)
        if(i2c_SCB_MODE_I2C_RUNTM_CFG)
        {
            status = i2c_I2CCyBtldrCommRead(pData, size, count, timeOut);
        }
        else if(i2c_SCB_MODE_SPI_RUNTM_CFG)
        {
            status = i2c_SpiCyBtldrCommRead(pData, size, count, timeOut);
        }
        else if(i2c_SCB_MODE_UART_RUNTM_CFG)
        {
            status = i2c_UartCyBtldrCommRead(pData, size, count, timeOut);
        }
        else if(i2c_SCB_MODE_EZI2C_RUNTM_CFG)
        {
            status = i2c_EzI2CCyBtldrCommRead(pData, size, count, timeOut);
        }
        else
        {
            status = CYRET_INVALID_STATE; /* Unknown mode: return status */
        }

    #elif(i2c_SCB_MODE_I2C_CONST_CFG)
        status = i2c_I2CCyBtldrCommRead(pData, size, count, timeOut);

    #elif(i2c_SCB_MODE_SPI_CONST_CFG)
        status = i2c_SpiCyBtldrCommRead(pData, size, count, timeOut);

    #elif(i2c_SCB_MODE_UART_CONST_CFG)
        status = i2c_UartCyBtldrCommRead(pData, size, count, timeOut);

    #elif(i2c_SCB_MODE_EZI2C_CONST_CFG)
        status = i2c_EzI2CCyBtldrCommRead(pData, size, count, timeOut);

    #else
        status = CYRET_INVALID_STATE; /* Unknown mode: return status */

    #endif /* (i2c_SCB_MODE_UNCONFIG_CONST_CFG) */

    return(status);
}


/*******************************************************************************
* Function Name: i2c_CyBtldrCommWrite
********************************************************************************
*
* Summary:
*  Calls write fucntion of the bootloader communication component for selected
*  mode.
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
cystatus i2c_CyBtldrCommWrite(const uint8 pData[], uint16 size, uint16 * count, uint8 timeOut)
{
    cystatus status;

    #if(i2c_SCB_MODE_UNCONFIG_CONST_CFG)
        if(i2c_SCB_MODE_I2C_RUNTM_CFG)
        {
            status = i2c_I2CCyBtldrCommWrite(pData, size, count, timeOut);
        }
        else if(i2c_SCB_MODE_SPI_RUNTM_CFG)
        {
            status = i2c_SpiCyBtldrCommWrite(pData, size, count, timeOut);
        }
        else if(i2c_SCB_MODE_UART_RUNTM_CFG)
        {
            status = i2c_UartCyBtldrCommWrite(pData, size, count, timeOut);
        }
        else if(i2c_SCB_MODE_EZI2C_RUNTM_CFG)
        {
            status = i2c_EzI2CCyBtldrCommWrite(pData, size, count, timeOut);
        }
        else
        {
            status = CYRET_INVALID_STATE; /* Unknown mode: return status */
        }
    #elif(i2c_SCB_MODE_I2C_CONST_CFG)
        status = i2c_I2CCyBtldrCommWrite(pData, size, count, timeOut);

    #elif(i2c_SCB_MODE_SPI_CONST_CFG)
        status = i2c_SpiCyBtldrCommWrite(pData, size, count, timeOut);

    #elif(i2c_SCB_MODE_UART_CONST_CFG)
        status = i2c_UartCyBtldrCommWrite(pData, size, count, timeOut);

    #elif(i2c_SCB_MODE_EZI2C_CONST_CFG)
        status = i2c_EzI2CCyBtldrCommWrite(pData, size, count, timeOut);

    #else
        status = CYRET_INVALID_STATE; /* Unknown mode: return status */

    #endif /* (i2c_SCB_MODE_UNCONFIG_CONST_CFG) */

    return(status);
}

#endif /* defined(CYDEV_BOOTLOADER_IO_COMP) && ((CYDEV_BOOTLOADER_IO_COMP == CyBtldr_i2c) || \
                                                    (CYDEV_BOOTLOADER_IO_COMP == CyBtldr_Custom_Interface)) */


/* [] END OF FILE */
