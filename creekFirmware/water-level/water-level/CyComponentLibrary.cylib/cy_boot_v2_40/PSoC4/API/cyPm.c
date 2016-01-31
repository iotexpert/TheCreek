/*******************************************************************************
* File Name: cyPm.c
* Version `$CY_MAJOR_VERSION`.`$CY_MINOR_VERSION`
*
*  Description:
*   Provides an API for the power management.
*
*  Note:
*   Documentation of the API's in this file is located in the
*   System Reference Guide provided with PSoC Creator.
*
********************************************************************************
* Copyright 2011, Cypress Semiconductor Corporation.  All rights reserved.
* You may use this file only in accordance with the license, terms, conditions,
* disclaimers, and limitations in the end user license agreement accompanying
* the software package with which this file was provided.
*******************************************************************************/

#include "cyPm.h"


/*******************************************************************************
* Function Name: CySysPmSleep
********************************************************************************
*
* Summary:
*  Puts the part into the Sleep state.
*
* Parameters:
*  None
*
* Return:
*  None
*
*******************************************************************************/
void CySysPmSleep(void)
{
    /* CM0 enters Sleep mode upon execution of WFI */
    CY_PM_CM0_SCR_REG &= ~CY_PM_CM0_SCR_SLEEPDEEP;

    /* Sleep and wait for interrupt */
    CY_PM_WFI;
}


/*******************************************************************************
* Function Name: CySysPmDeepSleep
********************************************************************************
*
* Summary:
*  Puts the part into the Deep-Sleep state.
*
* Parameters:
*  None
*
* Return:
*  None
*
*******************************************************************************/
void CySysPmDeepSleep(void)
{
    /* Device enters DeepSleep mode when CPU asserts SLEEPDEEP signal */
    CY_PM_PWR_CONTROL_REG &= ~CY_PM_PWR_CONTROL_HIBERNATE;

    /* CM0 enters DeepSleep/Hibernate mode upon execution of WFI */
    CY_PM_CM0_SCR_REG |= CY_PM_CM0_SCR_SLEEPDEEP;

    /* Sleep and wait for interrupt */
    CY_PM_WFI;
}


/*******************************************************************************
* Function Name: CySysPmHibernate
********************************************************************************
*
* Summary:
*  Puts the part into the Hibernate state.
*
* Parameters:
*  None
*
* Return:
*  None
*
*******************************************************************************/
void CySysPmHibernate(void)
{
    /* Device enters Hibernate mode when CPU asserts SLEEPDEEP signal */
    CY_PM_PWR_CONTROL_REG |= CY_PM_PWR_CONTROL_HIBERNATE;

    /* CM0 enters DeepSleep/Hibernate mode upon execution of WFI */
    CY_PM_CM0_SCR_REG |= CY_PM_CM0_SCR_SLEEPDEEP;

    /* Sleep and wait for interrupt */
    CY_PM_WFI;
}


/* [] END OF FILE */
