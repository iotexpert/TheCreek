/*******************************************************************************
* File Name: highside.c  
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
#include "highside.h"

static highside_BACKUP_STRUCT  highside_backup = {0u, 0u, 0u};


/*******************************************************************************
* Function Name: highside_Sleep
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
*  \snippet highside_SUT.c usage_highside_Sleep_Wakeup
*******************************************************************************/
void highside_Sleep(void)
{
    #if defined(highside__PC)
        highside_backup.pcState = highside_PC;
    #else
        #if (CY_PSOC4_4200L)
            /* Save the regulator state and put the PHY into suspend mode */
            highside_backup.usbState = highside_CR1_REG;
            highside_USB_POWER_REG |= highside_USBIO_ENTER_SLEEP;
            highside_CR1_REG &= highside_USBIO_CR1_OFF;
        #endif
    #endif
    #if defined(CYIPBLOCK_m0s8ioss_VERSION) && defined(highside__SIO)
        highside_backup.sioState = highside_SIO_REG;
        /* SIO requires unregulated output buffer and single ended input buffer */
        highside_SIO_REG &= (uint32)(~highside_SIO_LPM_MASK);
    #endif  
}


/*******************************************************************************
* Function Name: highside_Wakeup
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
*  Refer to highside_Sleep() for an example usage.
*******************************************************************************/
void highside_Wakeup(void)
{
    #if defined(highside__PC)
        highside_PC = highside_backup.pcState;
    #else
        #if (CY_PSOC4_4200L)
            /* Restore the regulator state and come out of suspend mode */
            highside_USB_POWER_REG &= highside_USBIO_EXIT_SLEEP_PH1;
            highside_CR1_REG = highside_backup.usbState;
            highside_USB_POWER_REG &= highside_USBIO_EXIT_SLEEP_PH2;
        #endif
    #endif
    #if defined(CYIPBLOCK_m0s8ioss_VERSION) && defined(highside__SIO)
        highside_SIO_REG = highside_backup.sioState;
    #endif
}


/* [] END OF FILE */
