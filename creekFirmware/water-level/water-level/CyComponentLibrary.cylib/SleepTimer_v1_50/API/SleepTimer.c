/*******************************************************************************
* File Name: `$INSTANCE_NAME`.c
* Version `$CY_MAJOR_VERSION`.`$CY_MINOR_VERSION`
*
* Description:
*  This file provides the source code to API for the Sleep Timer.
*
* Note:
*
********************************************************************************
* Copyright 2008-2010, Cypress Semiconductor Corporation.  All rights reserved.
* You may use this file only in accordance with the license, terms, conditions,
* disclaimers, and limitations in the end user license agreement accompanying
* the software package with which this file was provided.
*******************************************************************************/ 

#include "`$INSTANCE_NAME`.h"
#include "CyLib.h"

/* Variables were not initialized */
uint8 `$INSTANCE_NAME`_initVar = 0u;


/*******************************************************************************
* Function Name: `$INSTANCE_NAME`_Start
********************************************************************************
*
* Summary:
*  Starts the Sleep Timer operation. If this function is called, then the
*  component will be initialized with values defined in the customizer. If
*  parameters need to be changed, then stop component, change needed parameters
*  by calling the desired API functions, and restart the component.
*
* Parameters:
*  None.
*
* Return:
*  None.
*
* Global variables:
*  The `$INSTANCE_NAME`_initVar variable is used to indicate initial
*  configuration of this component.  The variable is initialized to zero and
*  set to 1 the first time `$INSTANCE_NAME`_Start() is called. This allows for
*  component initialization without re-initialization in all subsequent calls
*  to the `$INSTANCE_NAME`_Start() routine. 
*
* Reentrant:
*  No.
*
* Side Effects:
*  Enables 1 kHz ILO clock and leaves it enabled after the Sleep Time component
*  is stopped. 
*
*  Enables for the one pulse per second and central time wheel signals to wake
*  up device from the low power (Sleep and Alternate Active) modes and leaves
*  them enabled.
*
*  The Power Manager API has the higher priority on resource usage: it is NOT
*  guaranteed that the Sleep Timer's configuration will be the same on exit from
*  the Power Manager APIs as on the entry. To prevent this use the Sleep Timer's
*  Sleep() - to save configuration and stop the component and Wakeup() function
*  to restore configuration and enable the component.
*
*******************************************************************************/
void `$INSTANCE_NAME`_Start(void)
{
    /* Execute once in normal flow */
    if (0u == `$INSTANCE_NAME`_initVar)
    {
        `$INSTANCE_NAME`_Init();
        `$INSTANCE_NAME`_initVar = 1u;
    }
    
    /* Enables the component operation */
    `$INSTANCE_NAME`_Enable();
}


/*******************************************************************************
* Function Name: `$INSTANCE_NAME`_Init
********************************************************************************
*
* Summary: 
*  Configures component's parameters with values defined in customizer, enables 
*  1 kHz ILO clock if it was not enabled previously and enables or disables
*  CTW interrupt (according to the customizer's settings).
*
* Parameters:
*  None.
*
* Return:
*  None.
*
*******************************************************************************/
void `$INSTANCE_NAME`_Init(void)  `=ReentrantKeil($INSTANCE_NAME . "_Init")`
{
    /* Sets default (passed from customizer) interval */
    `$INSTANCE_NAME`_SetInterval(`$INSTANCE_NAME`_INTERVAL);
   
    /* Check if user sets to issue an interrupt in customizer */
    if (1u == `$INSTANCE_NAME`_ENABLE_INTERRUPT)
    {
        /* Enable interrupt */
        `$INSTANCE_NAME`_EnableInt();
    }   
    else /* interrupt should be disabled */
    {
        /* Disable interrupt */
        `$INSTANCE_NAME`_DisableInt();
    }   /* Interrupt is disabled  */
}


/*******************************************************************************
* Function Name: `$INSTANCE_NAME`_Enable
********************************************************************************
* Summary: 
*  Enables the 1kHz ILO clock, enables the CTW counter.
*
* Parameters:
*  None.
*
* Return:
*  None.
*
* Side Effects:
*  Enables 1 kHz ILO clocks and leaves it enabled after Sleep Time component
*  is stopped. 
*
*  Enables for the one pulse per second (for the RTC component) and 
*  cetral timewheel (for the Sleep Timer component) signals to wake up device
*  from the low power (Sleep and Alternate Active) modes and leaves them
*  enabled.
*
*******************************************************************************/
void `$INSTANCE_NAME`_Enable(void) `=ReentrantKeil($INSTANCE_NAME . "_Enable")`
{
    uint8 interruptState;

    /* If 1kHz ILO is disabled */
    if(0u == (`$INSTANCE_NAME`_ILO_CFG_REG & `$INSTANCE_NAME`_ILO_1KHZ_EN))
    {
        /* Enter critical section */
        interruptState = CyEnterCriticalSection();
        
        /* Enable 1kHz ILO */
        `$INSTANCE_NAME`_ILO_CFG_REG |=`$INSTANCE_NAME`_ILO_1KHZ_EN;
        
        /* Exit critical section */
        CyExitCriticalSection(interruptState);
        
    }   /* If 1kHz ILO is enabled - do nothing */
    
    /* Enable the CTW counter */
    `$INSTANCE_NAME`_TW_CFG_REG |= `$INSTANCE_NAME`_CTW_EN;
}


/*******************************************************************************
* Function Name: `$INSTANCE_NAME`_Stop
********************************************************************************
*
* Summary:
*  Stops the Sleep Timer operation: disables wake up on the Central Time Wheel
*  (CTW) counter terminal count reached. 
*
* Parameters:
*  None.
*
* Return:
*  None.
*
* Side Effects:
*  Leaves the one pulse per second and the Central Time Wheel signals to wake up
*  device from the low power (Sleep and Alternate Active) modes enabled after
*  Sleep Time component is stopped.
*
*  Leaves the 1 kHz ILO clock enabled after Sleep Time component is stopped.
*
*******************************************************************************/
void `$INSTANCE_NAME`_Stop(void) `=ReentrantKeil($INSTANCE_NAME . "_Stop")`
{
    uint8 interruptState;

    /* Enter critical section */
    interruptState = CyEnterCriticalSection();
    
    /* Disable CTW counter */
    `$INSTANCE_NAME`_TW_CFG_REG &= ~`$INSTANCE_NAME`_CTW_EN;
    
    /* Exit critical section */
    CyExitCriticalSection(interruptState);
}


/*******************************************************************************
* Function Name: `$INSTANCE_NAME`_EnableInt
********************************************************************************
*
* Summary:
*  Enables interrupt on the Central Time Wheel (CTW) terminal count reached.
*
* Parameters:
*  None.
*
* Return:
*  None.
*
*******************************************************************************/
void `$INSTANCE_NAME`_EnableInt(void) `=ReentrantKeil($INSTANCE_NAME . "_EnableInt")`
{
    uint8 interruptState;

    /* Enter critical section */
    interruptState = CyEnterCriticalSection();

     /* Enable interrupt on wake up */
    `$INSTANCE_NAME`_TW_CFG_REG |= `$INSTANCE_NAME`_CTW_IE;
    
    /* Exit critical section */
    CyExitCriticalSection(interruptState);
}


/*******************************************************************************
* Function Name: `$INSTANCE_NAME`_DisableInt
********************************************************************************
*
* Summary:
*  Disables interrupt on the Central Time Wheel (CTW) terminal count reached.
*
* Parameters:
*  None.
*
* Return:
*  None.
*
*******************************************************************************/
void `$INSTANCE_NAME`_DisableInt(void) `=ReentrantKeil($INSTANCE_NAME . "_DisableInt")`
{
    uint8 interruptState;

    /* Enter critical section */
    interruptState = CyEnterCriticalSection();

    /* Disable interrupt on wake up */
    `$INSTANCE_NAME`_TW_CFG_REG &= ~`$INSTANCE_NAME`_CTW_IE;
    
    /* Exit critical section */
    CyExitCriticalSection(interruptState);
}


/*******************************************************************************
* Function Name: `$INSTANCE_NAME`_SetInterval
********************************************************************************
*
* Summary:
*  Sets CTW interval period. The first interval can range from 1 to (period + 1)
*  milliseconds. Additional intervals occur at the nominal period.
*
* Parameters:
*  uint8 interval: interval?s value for the CTW.
*
*           Name                    Value        Period
*   `$INSTANCE_NAME`__CTW_2_MS      4'b0001        2 ms
*   `$INSTANCE_NAME`__CTW_4_MS      4'b0010        4 ms
*   `$INSTANCE_NAME`__CTW_8_MS      4'b0011        8 ms
*   `$INSTANCE_NAME`__CTW_16_MS     4'b0100        16 ms
*   `$INSTANCE_NAME`__CTW_32_MS     4'b0101        32 ms
*   `$INSTANCE_NAME`__CTW_64_MS     4'b0110        64 ms
*   `$INSTANCE_NAME`__CTW_128_MS    4'b0111        128 ms
*   `$INSTANCE_NAME`__CTW_256_MS    4'b1000        256 ms
*   `$INSTANCE_NAME`__CTW_512_MS    4'b1001        512 ms
*   `$INSTANCE_NAME`__CTW_1024_MS   4'b1010        1024 ms
*   `$INSTANCE_NAME`__CTW_2048_MS   4'b1011        2048 ms
*   `$INSTANCE_NAME`__CTW_4096_MS   4'b1100        4096 ms
*
* Return:
*  None.
*
* Side Effects:
*  Interval value can be only changed when CTW is disabled.
*
*******************************************************************************/
void `$INSTANCE_NAME`_SetInterval(uint8 interval) `=ReentrantKeil($INSTANCE_NAME . "_SetInterval")`
{
    /* Check if CTW is disabled */
    if (0u == (`$INSTANCE_NAME`_TW_CFG_REG & `$INSTANCE_NAME`_CTW_EN))
    {
        /* Set CTW interval */
        `$INSTANCE_NAME`_CTW_INTERVAL_REG = \
            (`$INSTANCE_NAME`_CTW_INTERVAL_REG & ~`$INSTANCE_NAME`_INTERVAL_MASK) | \
            (interval & `$INSTANCE_NAME`_INTERVAL_MASK);
    }   
    /* Do nothing if CTW is enabled. */
}


/*******************************************************************************
* Function Name: `$INSTANCE_NAME`_GetStatus
********************************************************************************
*
* Summary:
*  This function should always be called (when the Sleep Timer?s interrupt is
*  disabled or enabled) after wake up to clear the ctw_int status bit.
*
* Parameters:
*  None.
*
* Return:
*  uint8 with bits set if corresponding event has occurred
*            Name                             Description
*  `$INSTANCE_NAME`_PM_INT_SR_ONEPPSP  A onepps event has occured
*  `$INSTANCE_NAME`_PM_INT_SR_CTW      A central timewheel event occured
*  `$INSTANCE_NAME`_PM_INT_SR_FTW      A fast timewheel event occured
*
* Side Effects:
*  If an interrupt gets generated at the same time as a register read/clear,
*  the bit will remain set (which causes another interrupt). Reports and then
*  clears all interrupt status bits in the Power Manager Interrupt Status
*  Register. Some of the bits are not relevant to operation of this component.
*  
*  If the GetStatus() function is not called in an interrupt associated with
*  the SleepTimer, the interrupt is not cleared and as soon as the interrupt is
*  exited it will be re-entered. 
*
*  Once the Sleep timer has expired, until the GetStatus() function is called
*  the sleep interval is functionally 0mSec, since GetStatus clears the ctw_int
*  bit.
*
*******************************************************************************/
uint8 `$INSTANCE_NAME`_GetStatus(void) `=ReentrantKeil($INSTANCE_NAME . "_GetStatus")`
{
    /* Read to clear  */
    return CyPmReadStatus(CY_PM_FTW_INT | CY_PM_CTW_INT | CY_PM_ONEPPS_INT);  
}


/* [] END OF FILE */
