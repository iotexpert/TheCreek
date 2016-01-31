/*******************************************************************************
* File Name: `$INSTANCE_NAME`.c
* Version `$CY_MAJOR_VERSION`.`$CY_MINOR_VERSION`
*
*  Description:
*   This file provides the source code to API for the Sleep Timer.
*
*  Note:
*
********************************************************************************
* Copyright 2008-2009, Cypress Semiconductor Corporation.  All rights reserved.
* You may use this file only in accordance with the license, terms, conditions,
* disclaimers, and limitations in the end user license agreement accompanying
* the software package with which this file was provided.
********************************************************************************/ 


#include "`$INSTANCE_NAME`.h"

/* Take parameters from:
 *  xxxxxx00 - No Interval, nor EnableInt was changed by API
 *  xxxxxx01 - No Interval, but EnableInt was changed by API
 *  xxxxxx10 - Interval, but not EnableInt was changed by API
 *  xxxxxx11 - Interval and EnableInt was changed by API
*/
uint8 `$INSTANCE_NAME`_flags = 0;


/*******************************************************************************
* Function Name: `$INSTANCE_NAME`_Start
********************************************************************************
*
* Summary:
*  Starts the Sleep Timer operation: enables 1 kHz clock, sets default
*  parameters if they were not set previously by corresponding API functions.
*
* Parameters:
*  void
*
* Return:
*  void
*
* Side Effects:
*  Enables 1 kHz ILO clocks and leaves it enabled after Sleep Time component
*  is stopped.
*
*******************************************************************************/
void `$INSTANCE_NAME`_Start(void)
{
    /* Check if 1kHz ILO is disabled */
    if (0 == (`$INSTANCE_NAME`_ILO_CFG & `$INSTANCE_NAME`_ILO_1KHZ_EN))
    {
        `$INSTANCE_NAME`_ILO_CFG |=`$INSTANCE_NAME`_ILO_1KHZ_EN;
    }
    /* If 1kHz ILO is enabled - do nothing */


    /* Sets default (passed by parameter from customizer) interval if it was
     *   not manually set by API function                                       */
    if (0 == (`$INSTANCE_NAME`_flags & `$INSTANCE_NAME`_INTERVAL_CHNG_BIT))
    {
        `$INSTANCE_NAME`_SetInterval(`$INSTANCE_NAME`_INTERVAL);
    }
    /* Interval is already set */


    /* Sets default (customizer's) interrupt enable if it was not set by API  */
    if (0 == (`$INSTANCE_NAME`_flags & `$INSTANCE_NAME`_INTERRUPT_CHNG_BIT))
    {
        /* Check if user sets to issue an interrupt in customizer */
        if ( 1 == `$INSTANCE_NAME`_ENABLE_INTERRUPT )
        {
            /* Enable interrupt */
            `$INSTANCE_NAME`_EnableInt();
        } else
        {
            /* Disable interrupt */
            `$INSTANCE_NAME`_DisableInt();
        }
    }
    /* Interrupt enable/disable is already applied */

    /* Enable wake up on CTW counter */
    `$INSTANCE_NAME`_TW_CFG |= `$INSTANCE_NAME`_CTW_EN;

}


/*******************************************************************************
* Function Name: `$INSTANCE_NAME`_Stop
********************************************************************************
*
* Summary:
*  Stops the Sleep Timer operation: disables wake up on CTW counter terminal
*  count reached and disables interrupt issuing on foregoing condition.
*
* Parameters:
*  void
*
* Return:
*  void
*
* Side Effects:
* All changes made by API to configuration of SleepTimer before this function
* was called will not be used, values from customizer will be used instead, when
* `$INSTANCE_NAME`_Start() will be executed next time.
*
*******************************************************************************/
void `$INSTANCE_NAME`_Stop(void)
{
    /* Disable wake up on CTW counter */
    `$INSTANCE_NAME`_TW_CFG &= ~`$INSTANCE_NAME`_CTW_EN;
    /* Just for sure of next time start */
    `$INSTANCE_NAME`_TW_CFG &= ~`$INSTANCE_NAME`_CTW_IE;
    /* Clear flag of changes was made by API */
    `$INSTANCE_NAME`_flags = 0;
}


/*******************************************************************************
* Function Name: `$INSTANCE_NAME`_EnableInt
********************************************************************************
*
* Summary:
*  Enables Sleep Timer component issuing interrupt on wake up.
*
* Parameters:
*  void
*
* Return:
*  void
*
*******************************************************************************/
void `$INSTANCE_NAME`_EnableInt(void)
{

     /* Update flag that interrupt was set by API */
    `$INSTANCE_NAME`_flags |= `$INSTANCE_NAME`_INTERRUPT_CHNG_BIT;
     /* Enable interrupt on wake up */
    `$INSTANCE_NAME`_TW_CFG |= `$INSTANCE_NAME`_CTW_IE;
}


/*******************************************************************************
* Function Name: `$INSTANCE_NAME`_DisableInt
********************************************************************************
*
* Summary:
*  Disables Sleep Timer component issuing interrupt on wake up.
*
* Parameters:
*  void
*
* Return:
*  void
*
*******************************************************************************/
void `$INSTANCE_NAME`_DisableInt(void)
{
    /* Update flag that interrupt was set by API */
    `$INSTANCE_NAME`_flags |= `$INSTANCE_NAME`_INTERRUPT_CHNG_BIT;
    /* Disable interrupt on wake up */
    `$INSTANCE_NAME`_TW_CFG &= ~`$INSTANCE_NAME`_CTW_IE;
}


/*******************************************************************************
* Function Name: `$INSTANCE_NAME`_SetInterval
********************************************************************************
*
* Summary:
*  Sets interval for Sleep Timer to wake up.
*
* Parameters:
*  interval:  This parameter is used to define interval value in milliseconds.
*  Acceptable is only values from a fixed range:
*   Name                            Value        Period
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
*  void
*
* Side Effects:
*  The first interval can range from 1 to (period + 1)ms. Additional intervals
*  occur at the nominal period. Interval value can be only changed when
*  Sleep Timer is stopped.
*
*******************************************************************************/
void `$INSTANCE_NAME`_SetInterval(uint8 interval)
{
    /* Check if CTW is disabled */
    if (0 == (`$INSTANCE_NAME`_TW_CFG & `$INSTANCE_NAME`_CTW_EN))
    {
        /* Clear interval setting */
        `$INSTANCE_NAME`_CTW_INTERVAL &= ~ `$INSTANCE_NAME`_INTERVAL_MASK;
        /* Set wake up interval */
        `$INSTANCE_NAME`_CTW_INTERVAL |= \
            (interval & `$INSTANCE_NAME`_INTERVAL_MASK);
        /* Update flag that interval was set by API */
        `$INSTANCE_NAME`_flags |= `$INSTANCE_NAME`_INTERVAL_CHNG_BIT;
    }
    /* Do nothing if CTW is enabled. */

}


/*******************************************************************************
* Function Name: `$INSTANCE_NAME`_GetStatus
********************************************************************************
*
* Summary:
*  Clears all interrupt status bits (react_int, limact_int, onepps_int, ctw_int,
*  and  ftw_int)  in Power Manager Interrupt Status Register. This function 
*  should always be called (when Sleep Timer?s interrupt is disabled or enabled)
*  after wake up to clear ctw_int status bit.
*
* Parameters:
*  void
*
* Return:
* uint8 with bits set if corresponding event has occurred
* Name                               Value   Description
*`$INSTANCE_NAME`_PM_INT_SR_REACT    0x80u   A transition from limited active to 
*                                            active mode event occured 
*`$INSTANCE_NAME`_PM_INT_SR_LIMACT   0x08u   Limited active ready event occured
*`$INSTANCE_NAME`_PM_INT_SR_ONEPPSP  0x04u   A onepps event has occured
*`$INSTANCE_NAME`_PM_INT_SR_CTW      0x02u   A central timewheel event occured
*`$INSTANCE_NAME`_PM_INT_SR_FTW      0x01u   A fast timewheel event occured
*
* Side Effects:
*  If an interrupt gets generated at the same time as a clear, the bit will 
*  remain set (which causes another interrupt).
*
*******************************************************************************/
uint8 `$INSTANCE_NAME`_GetStatus(void)
{
    uint8 status;

    /* Read to clear  */
    status = `$INSTANCE_NAME`_INT_SR;

    return status;
}

/* [] END OF FILE */
