/*******************************************************************************
* File Name: V036.c  
* Version 2.20
*
* Description:
*  This file contains APIs to set up the Pins component for low power modes.
*
* Note:
*
********************************************************************************
* Copyright 2015, Cypress Semiconductor Corporation.  All rights reserved.
* You may use this file only in accordance with the license, terms, conditions, 
* disclaimers, and limitations in the end user license agreement accompanying 
* the software package with which this file was provided.
*******************************************************************************/

#include "cytypes.h"
#include "V036.h"

static V036_BACKUP_STRUCT  V036_backup = {0u, 0u, 0u};


/*******************************************************************************
* Function Name: V036_Sleep
****************************************************************************//**
*
* \brief Stores the pin configuration and prepares the pin for entering chip 
*  deep-sleep/hibernate modes. This function must be called for SIO and USBIO
*  pins. It is not essential if using GPIO or GPIO_OVT pins.
*
* <b>Note</b> This function is available in PSoC 4 only.
*
* \return 
*  None 
*  
* \sideeffect
*  For SIO pins, this function configures the pin input threshold to CMOS and
*  drive level to Vddio. This is needed for SIO pins when in device 
*  deep-sleep/hibernate modes.
*
* \funcusage
*  \snippet V036_SUT.c usage_V036_Sleep_Wakeup
*******************************************************************************/
void V036_Sleep(void)
{
    #if defined(V036__PC)
        V036_backup.pcState = V036_PC;
    #else
        #if (CY_PSOC4_4200L)
            /* Save the regulator state and put the PHY into suspend mode */
            V036_backup.usbState = V036_CR1_REG;
            V036_USB_POWER_REG |= V036_USBIO_ENTER_SLEEP;
            V036_CR1_REG &= V036_USBIO_CR1_OFF;
        #endif
    #endif
    #if defined(CYIPBLOCK_m0s8ioss_VERSION) && defined(V036__SIO)
        V036_backup.sioState = V036_SIO_REG;
        /* SIO requires unregulated output buffer and single ended input buffer */
        V036_SIO_REG &= (uint32)(~V036_SIO_LPM_MASK);
    #endif  
}


/*******************************************************************************
* Function Name: V036_Wakeup
****************************************************************************//**
*
* \brief Restores the pin configuration that was saved during Pin_Sleep().
*
* For USBIO pins, the wakeup is only triggered for falling edge interrupts.
*
* <b>Note</b> This function is available in PSoC 4 only.
*
* \return 
*  None
*  
* \funcusage
*  Refer to V036_Sleep() for an example usage.
*******************************************************************************/
void V036_Wakeup(void)
{
    #if defined(V036__PC)
        V036_PC = V036_backup.pcState;
    #else
        #if (CY_PSOC4_4200L)
            /* Restore the regulator state and come out of suspend mode */
            V036_USB_POWER_REG &= V036_USBIO_EXIT_SLEEP_PH1;
            V036_CR1_REG = V036_backup.usbState;
            V036_USB_POWER_REG &= V036_USBIO_EXIT_SLEEP_PH2;
        #endif
    #endif
    #if defined(CYIPBLOCK_m0s8ioss_VERSION) && defined(V036__SIO)
        V036_SIO_REG = V036_backup.sioState;
    #endif
}


/* [] END OF FILE */
