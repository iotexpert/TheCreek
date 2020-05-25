/***************************************************************************//**
* \file ezi2c_PM.c
* \version 4.0
*
* \brief
*  This file provides the source code to the Power Management support for
*  the SCB Component.
*
* Note:
*
********************************************************************************
* \copyright
* Copyright 2013-2017, Cypress Semiconductor Corporation.  All rights reserved.
* You may use this file only in accordance with the license, terms, conditions,
* disclaimers, and limitations in the end user license agreement accompanying
* the software package with which this file was provided.
*******************************************************************************/

#include "ezi2c.h"
#include "ezi2c_PVT.h"

#if(ezi2c_SCB_MODE_I2C_INC)
    #include "ezi2c_I2C_PVT.h"
#endif /* (ezi2c_SCB_MODE_I2C_INC) */

#if(ezi2c_SCB_MODE_EZI2C_INC)
    #include "ezi2c_EZI2C_PVT.h"
#endif /* (ezi2c_SCB_MODE_EZI2C_INC) */

#if(ezi2c_SCB_MODE_SPI_INC || ezi2c_SCB_MODE_UART_INC)
    #include "ezi2c_SPI_UART_PVT.h"
#endif /* (ezi2c_SCB_MODE_SPI_INC || ezi2c_SCB_MODE_UART_INC) */


/***************************************
*   Backup Structure declaration
***************************************/

#if(ezi2c_SCB_MODE_UNCONFIG_CONST_CFG || \
   (ezi2c_SCB_MODE_I2C_CONST_CFG   && (!ezi2c_I2C_WAKE_ENABLE_CONST))   || \
   (ezi2c_SCB_MODE_EZI2C_CONST_CFG && (!ezi2c_EZI2C_WAKE_ENABLE_CONST)) || \
   (ezi2c_SCB_MODE_SPI_CONST_CFG   && (!ezi2c_SPI_WAKE_ENABLE_CONST))   || \
   (ezi2c_SCB_MODE_UART_CONST_CFG  && (!ezi2c_UART_WAKE_ENABLE_CONST)))

    ezi2c_BACKUP_STRUCT ezi2c_backup =
    {
        0u, /* enableState */
    };
#endif


/*******************************************************************************
* Function Name: ezi2c_Sleep
****************************************************************************//**
*
*  Prepares the ezi2c component to enter Deep Sleep.
*  The “Enable wakeup from Deep Sleep Mode” selection has an influence on this 
*  function implementation:
*  - Checked: configures the component to be wakeup source from Deep Sleep.
*  - Unchecked: stores the current component state (enabled or disabled) and 
*    disables the component. See SCB_Stop() function for details about component 
*    disabling.
*
*  Call the ezi2c_Sleep() function before calling the 
*  CyPmSysDeepSleep() function. 
*  Refer to the PSoC Creator System Reference Guide for more information about 
*  power management functions and Low power section of this document for the 
*  selected mode.
*
*  This function should not be called before entering Sleep.
*
*******************************************************************************/
void ezi2c_Sleep(void)
{
#if(ezi2c_SCB_MODE_UNCONFIG_CONST_CFG)

    if(ezi2c_SCB_WAKE_ENABLE_CHECK)
    {
        if(ezi2c_SCB_MODE_I2C_RUNTM_CFG)
        {
            ezi2c_I2CSaveConfig();
        }
        else if(ezi2c_SCB_MODE_EZI2C_RUNTM_CFG)
        {
            ezi2c_EzI2CSaveConfig();
        }
    #if(!ezi2c_CY_SCBIP_V1)
        else if(ezi2c_SCB_MODE_SPI_RUNTM_CFG)
        {
            ezi2c_SpiSaveConfig();
        }
        else if(ezi2c_SCB_MODE_UART_RUNTM_CFG)
        {
            ezi2c_UartSaveConfig();
        }
    #endif /* (!ezi2c_CY_SCBIP_V1) */
        else
        {
            /* Unknown mode */
        }
    }
    else
    {
        ezi2c_backup.enableState = (uint8) ezi2c_GET_CTRL_ENABLED;

        if(0u != ezi2c_backup.enableState)
        {
            ezi2c_Stop();
        }
    }

#else

    #if (ezi2c_SCB_MODE_I2C_CONST_CFG && ezi2c_I2C_WAKE_ENABLE_CONST)
        ezi2c_I2CSaveConfig();

    #elif (ezi2c_SCB_MODE_EZI2C_CONST_CFG && ezi2c_EZI2C_WAKE_ENABLE_CONST)
        ezi2c_EzI2CSaveConfig();

    #elif (ezi2c_SCB_MODE_SPI_CONST_CFG && ezi2c_SPI_WAKE_ENABLE_CONST)
        ezi2c_SpiSaveConfig();

    #elif (ezi2c_SCB_MODE_UART_CONST_CFG && ezi2c_UART_WAKE_ENABLE_CONST)
        ezi2c_UartSaveConfig();

    #else

        ezi2c_backup.enableState = (uint8) ezi2c_GET_CTRL_ENABLED;

        if(0u != ezi2c_backup.enableState)
        {
            ezi2c_Stop();
        }

    #endif /* defined (ezi2c_SCB_MODE_I2C_CONST_CFG) && (ezi2c_I2C_WAKE_ENABLE_CONST) */

#endif /* (ezi2c_SCB_MODE_UNCONFIG_CONST_CFG) */
}


/*******************************************************************************
* Function Name: ezi2c_Wakeup
****************************************************************************//**
*
*  Prepares the ezi2c component for Active mode operation after 
*  Deep Sleep.
*  The “Enable wakeup from Deep Sleep Mode” selection has influence on this 
*  function implementation:
*  - Checked: restores the component Active mode configuration.
*  - Unchecked: enables the component if it was enabled before enter Deep Sleep.
*
*  This function should not be called after exiting Sleep.
*
*  \sideeffect
*   Calling the ezi2c_Wakeup() function without first calling the 
*   ezi2c_Sleep() function may produce unexpected behavior.
*
*******************************************************************************/
void ezi2c_Wakeup(void)
{
#if(ezi2c_SCB_MODE_UNCONFIG_CONST_CFG)

    if(ezi2c_SCB_WAKE_ENABLE_CHECK)
    {
        if(ezi2c_SCB_MODE_I2C_RUNTM_CFG)
        {
            ezi2c_I2CRestoreConfig();
        }
        else if(ezi2c_SCB_MODE_EZI2C_RUNTM_CFG)
        {
            ezi2c_EzI2CRestoreConfig();
        }
    #if(!ezi2c_CY_SCBIP_V1)
        else if(ezi2c_SCB_MODE_SPI_RUNTM_CFG)
        {
            ezi2c_SpiRestoreConfig();
        }
        else if(ezi2c_SCB_MODE_UART_RUNTM_CFG)
        {
            ezi2c_UartRestoreConfig();
        }
    #endif /* (!ezi2c_CY_SCBIP_V1) */
        else
        {
            /* Unknown mode */
        }
    }
    else
    {
        if(0u != ezi2c_backup.enableState)
        {
            ezi2c_Enable();
        }
    }

#else

    #if (ezi2c_SCB_MODE_I2C_CONST_CFG  && ezi2c_I2C_WAKE_ENABLE_CONST)
        ezi2c_I2CRestoreConfig();

    #elif (ezi2c_SCB_MODE_EZI2C_CONST_CFG && ezi2c_EZI2C_WAKE_ENABLE_CONST)
        ezi2c_EzI2CRestoreConfig();

    #elif (ezi2c_SCB_MODE_SPI_CONST_CFG && ezi2c_SPI_WAKE_ENABLE_CONST)
        ezi2c_SpiRestoreConfig();

    #elif (ezi2c_SCB_MODE_UART_CONST_CFG && ezi2c_UART_WAKE_ENABLE_CONST)
        ezi2c_UartRestoreConfig();

    #else

        if(0u != ezi2c_backup.enableState)
        {
            ezi2c_Enable();
        }

    #endif /* (ezi2c_I2C_WAKE_ENABLE_CONST) */

#endif /* (ezi2c_SCB_MODE_UNCONFIG_CONST_CFG) */
}


/* [] END OF FILE */
