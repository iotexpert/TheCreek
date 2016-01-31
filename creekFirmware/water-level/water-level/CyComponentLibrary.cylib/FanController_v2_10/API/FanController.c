/*******************************************************************************
* File Name: `$INSTANCE_NAME`.c
* Version `$CY_MAJOR_VERSION`.`$CY_MINOR_VERSION`
*
* Description:
*  This file contains the API files for the FanController component.
*  The FanController component supports up to 16 pulse width modulator (PWM)
*  controlled 4-wire fans and through an intuitive graphical interface,
*  enabling designers to quickly configure the number of fans, their electro-
*  mechanical properties, organization into banks (if desired) and the control
*  algorithm type: firmware or hardware based.
*
*  Refer to AN66627 "PSoC(R) 3 and PSoC 5 - Intelligent Fan Controller" for
*  more details and example projects.
*
* Note:
*
*******************************************************************************
* Copyright 2008-2012, Cypress Semiconductor Corporation.  All rights reserved.
* You may use this file only in accordance with the license, terms, conditions,
* disclaimers, and limitations in the end user license agreement accompanying
* the software package with which this file was provided.
********************************************************************************/

#include "`$INSTANCE_NAME`.h"


/*******************************************************************************
* Variables
*
* Fan Control Data Structure declaration and initialization can be found at the
* end of this file due to its large size.
********************************************************************************/
static uint8   `$INSTANCE_NAME`_initVar = 0u;

/* DMA controller variables */
uint8   `$INSTANCE_NAME`_TachOutDMA_dmaHandle = DMA_INVALID_CHANNEL;
uint8   `$INSTANCE_NAME`_TachOutDMA_channel = 0u;

#if(`$INSTANCE_NAME`_FAN_CTL_MODE == `$INSTANCE_NAME`_FANCTLMODE_HARDWARE)
    uint8   `$INSTANCE_NAME`_TachInDMA_dmaHandle = DMA_INVALID_CHANNEL;
    uint8   `$INSTANCE_NAME`_TachInDMA_channel = 0u;
#endif /* `$INSTANCE_NAME`_FAN_CTL_MODE == `$INSTANCE_NAME`_FANCTLMODE_HARDWARE */

/* DMA transfer descriptors used by the tachometer block for fan speed control/reporting */
`$INSTANCE_NAME`_fanTdOutStruct `$INSTANCE_NAME`_fanOutTds[`$INSTANCE_NAME`_NUMBER_OF_FANS];
#if(`$INSTANCE_NAME`_FAN_CTL_MODE == `$INSTANCE_NAME`_FANCTLMODE_HARDWARE)
    `$INSTANCE_NAME`_fanTdInStruct `$INSTANCE_NAME`_fanInTds[`$INSTANCE_NAME`_NUMBER_OF_FANS];
#endif /* `$INSTANCE_NAME`_FAN_CTL_MODE == `$INSTANCE_NAME`_FANCTLMODE_HARDWARE */

/*******************************************************************************
* Private API Prototypes
********************************************************************************/
void  `$INSTANCE_NAME`_SetupDMA(void) CYREENTRANT;
uint8 `$INSTANCE_NAME`_TachOutDMA_DmaInitialize(uint8 burstCount, uint8 requestPerBurst, uint16 upperSrcAddress,
                                                uint16 upperDestAddress) CYREENTRANT;
void  `$INSTANCE_NAME`_TachOutDMA_DmaRelease(void) CYREENTRANT;

#if(`$INSTANCE_NAME`_FAN_CTL_MODE == `$INSTANCE_NAME`_FANCTLMODE_HARDWARE)
    uint8 `$INSTANCE_NAME`_TachInDMA_DmaInitialize(uint8 burstCount, uint8 requestPerBurst, uint16 upperSrcAddress,
                                                   uint16 upperDestAddress) CYREENTRANT;
    void  `$INSTANCE_NAME`_TachInDMA_DmaRelease(void) CYREENTRANT;
#endif /* `$INSTANCE_NAME`_FAN_CTL_MODE == `$INSTANCE_NAME`_FANCTLMODE_HARDWARE */


/******************************************************************************
* Function Name: `$INSTANCE_NAME`_Start
*******************************************************************************
*
* Summary:
*  Initializes component if not already initialized, then re-enables it.
*
* Parameters:
*  None
*
* Return:
*  None
*
* Side Effects:
*  None
*
****************************************************************************/
void `$INSTANCE_NAME`_Start(void) `=ReentrantKeil($INSTANCE_NAME . "_Start")`
{
    /* If not already initialized, then initialize hardware and variables */
    if(`$INSTANCE_NAME`_initVar == 0u)
    {
        `$INSTANCE_NAME`_Init();
        `$INSTANCE_NAME`_initVar = 1u;
    }
    `$INSTANCE_NAME`_Enable();
}


/******************************************************************************
* Function Name: `$INSTANCE_NAME`_Stop
*******************************************************************************
*
* Summary:
*  Stop the fan controller component.  Disables all hardware sub-components,
*  drives fan outputs high and de-asserts the alert pin.
*
* Parameters:
*  None
*
* Return:
*  None
*
* Side Effects:
*  All PWM outputs to fans will be driven high (100% duty cycle).
*  Alert ouput de-asserted
*
******************************************************************************/
void `$INSTANCE_NAME`_Stop(void) `=ReentrantKeil($INSTANCE_NAME . "_Stop")`
{
    /* Turn off the hardware sub-components */
    `$INSTANCE_NAME`_GLOBAL_CONTROL_REG = 0u;

    /* De-assert the alert pin */
    `$INSTANCE_NAME`_DisableAlert();

    /* Release DMA resources */
    `$INSTANCE_NAME`_TachOutDMA_DmaRelease();
    
    #if(`$INSTANCE_NAME`_FAN_CTL_MODE == `$INSTANCE_NAME`_FANCTLMODE_HARDWARE)
        `$INSTANCE_NAME`_TachInDMA_DmaRelease();
    #endif /* `$INSTANCE_NAME`_FAN_CTL_MODE == `$INSTANCE_NAME`_FANCTLMODE_HARDWARE */

}


/******************************************************************************
* Function Name: `$INSTANCE_NAME`_Init
*******************************************************************************
*
* Summary:
*  Sets up DMA channels and transaction descriptors. Configures PWMs and
*  Tachometer hardware blocks.
*
* Parameters:
*  None
*
*Return:
*  None
*
* Side Effects:
*  Allocates DMA channels and transaction descriptors (TDs)
*
******************************************************************************/
void `$INSTANCE_NAME`_Init(void) `=ReentrantKeil($INSTANCE_NAME . "_Init")`
{
    uint8 counti;
    uint8 interruptState;

    `$INSTANCE_NAME`_SetupDMA();

    /***********************************************************************
    * Initialize the PWMs
    ***********************************************************************/
    #if(`$INSTANCE_NAME`_FAN_CTL_MODE == `$INSTANCE_NAME`_FANCTLMODE_HARDWARE)

        /* Configure hardware controlled PWMs */
        for (counti = 0u; counti < `$INSTANCE_NAME`_NUMBER_OF_FANS; counti++)
        {
            /* 8-bit hardware controlled PWM */
            #if(`$INSTANCE_NAME`_PWMRES == `$INSTANCE_NAME`_PWMRES_EIGHTBIT)
                /* Set max PWM period */
                CY_SET_REG8(`$INSTANCE_NAME`_fanDriverRegs[counti].pwmPeriodReg, `$INSTANCE_NAME`_PWM_PERIOD);
                /* Set max PWM duty cycle (must be <= PWM period) */
                CY_SET_REG8(`$INSTANCE_NAME`_fanDriverRegs[counti].pwmMaxDutyReg, `$INSTANCE_NAME`_PWM_PERIOD);
                /* Initial duty is set to max */
                `$INSTANCE_NAME`_SetDutyCycle((counti + 1), `$INSTANCE_NAME`_fanProperties[counti].initDuty);
                /* Initial speed is set to max */
                `$INSTANCE_NAME`_SetDesiredSpeed((counti + 1), `$INSTANCE_NAME`_fanProperties[counti].initRpm);
            /* 10-bit hardware controlled PWM */
            #else
                /* Set max PWM period */
                CY_SET_REG16(`$INSTANCE_NAME`_fanDriverRegs[counti].pwmPeriodReg, `$INSTANCE_NAME`_PWM_PERIOD);
                /* Set max PWM duty cycle (must be <= PWM period) */
                CY_SET_REG16(`$INSTANCE_NAME`_fanDriverRegs[counti].pwmMaxDutyReg, `$INSTANCE_NAME`_PWM_PERIOD);
                /* Initial duty is set to max */
                `$INSTANCE_NAME`_SetDutyCycle((counti + 1), `$INSTANCE_NAME`_fanProperties[counti].initDuty);
                /* Initial speed is set to max */
                `$INSTANCE_NAME`_SetDesiredSpeed((counti + 1), `$INSTANCE_NAME`_fanProperties[counti].initRpm);
            #endif /* `$INSTANCE_NAME`_PWMRES == `$INSTANCE_NAME`_PWMRES_EIGHTBIT */

            /* Enable cnt7 to track speed regulation errors */
            CY_SET_REG8(`$INSTANCE_NAME`_fanDriverRegs[counti].errorCountReg,
                CY_GET_REG8(`$INSTANCE_NAME`_fanDriverRegs[counti].errorCountReg) | `$INSTANCE_NAME`_COUNT7_ENABLE);
        }
    #elif (`$INSTANCE_NAME`_FAN_CTL_MODE == `$INSTANCE_NAME`_FANCTLMODE_FIRMWARE)
        
        
        /* Configure firmware controlled PWMs */
        for (counti = 0u; counti < `$INSTANCE_NAME`_FANPWM_COUNT; counti++)
        {
            /* 8-bit firmware controlled PWM */
            #if(`$INSTANCE_NAME`_PWMRES == `$INSTANCE_NAME`_PWMRES_EIGHTBIT)
                CY_SET_REG8(`$INSTANCE_NAME`_fanPwmInitRegs[counti].pwmPeriodReg, `$INSTANCE_NAME`_PWM_PERIOD);
                
                interruptState = CyEnterCriticalSection();
                
                CY_SET_REG8(`$INSTANCE_NAME`_fanPwmInitRegs[counti].pwmAuxControlReg,
                    CY_GET_REG8(`$INSTANCE_NAME`_fanPwmInitRegs[counti].pwmAuxControlReg) |
                        `$INSTANCE_NAME`_FANPWM_AUX_CTRL_FIFO0_CLR_8);
                
                CyExitCriticalSection(interruptState);
                
            /* 10-bit firmware controlled PWM */
            #else
                CY_SET_REG16(`$INSTANCE_NAME`_fanPwmInitRegs[counti].pwmPeriodReg, `$INSTANCE_NAME`_PWM_PERIOD);
                
                interruptState = CyEnterCriticalSection();
                
                CY_SET_REG16(`$INSTANCE_NAME`_fanPwmInitRegs[counti].pwmAuxControlReg,
                    CY_GET_REG16(`$INSTANCE_NAME`_fanPwmInitRegs[counti].pwmAuxControlReg) |
                        `$INSTANCE_NAME`_FANPWM_AUX_CTRL_FIFO0_CLR_10);
                
                CyExitCriticalSection(interruptState);
                        
            #endif /* `$INSTANCE_NAME`_PWMRES == `$INSTANCE_NAME`_PWMRES_EIGHTBIT */
        }

        for (counti = 0u; counti < `$INSTANCE_NAME`_NUMBER_OF_FAN_OUTPUTS; counti++)
        {
            /* Set Desired speed to initial RPM from the customizer */
            `$INSTANCE_NAME`_SetDesiredSpeed((counti + 1), `$INSTANCE_NAME`_fanProperties[counti].initRpm);
        }

    #endif /* `$INSTANCE_NAME`_FAN_CTL_MODE == `$INSTANCE_NAME`_FANCTLMODE_HARDWARE */

    /***********************************************************************
    * Initialize the Tachometer
    ***********************************************************************/
    
    interruptState = CyEnterCriticalSection();
    
    /* Enable count7 hardware block to drive fan address */
    `$INSTANCE_NAME`_TACH_FAN_COUNTR_AUX_CTL_REG   |= `$INSTANCE_NAME`_COUNT7_ENABLE;
    /* Enable count7 hardware block for tachometer glitch filter */
    `$INSTANCE_NAME`_TACH_GLITCH_FILTER_AUX_CTL_REG |= `$INSTANCE_NAME`_COUNT7_ENABLE;
    
    CyExitCriticalSection(interruptState);

    #if((`$INSTANCE_NAME`_FAN_CTL_MODE == `$INSTANCE_NAME`_FANCTLMODE_HARDWARE) && \
        (`$INSTANCE_NAME`_DAMPING_FACTOR != 0u))

        /* Init lower 16 bits of 32-bit Famping Factor Counter */
         CY_SET_REG16(`$INSTANCE_NAME`_TACH_DAMPING_PERIOD_LOW_LSB_PTR, `$INSTANCE_NAME`_DAMPING_FACTOR_PERIOD_LOW);
        /* Init high 16 bits of 32-bit Famping Factor Counter */
        `$INSTANCE_NAME`_TACH_DAMPING_PERIOD_HIGH_LSB_REG = `$INSTANCE_NAME`_DAMPING_FACTOR_PERIOD_HIGH;

    #endif /* (`$INSTANCE_NAME`_FAN_CTL_MODE == 
            `$INSTANCE_NAME`_FANCTLMODE_HARDWARE) && (`$INSTANCE_NAME`_DAMPING_FACTOR != 0) */

    /***********************************************************************
    * Initialize the Alert Mask Register based on customizer defaults
    ***********************************************************************/
    `$INSTANCE_NAME`_SetAlertMask(`$INSTANCE_NAME`_INIT_ALERT_MASK);
}


/******************************************************************************
* Function Name: `$INSTANCE_NAME`_Enable
*******************************************************************************
*
* Summary:
*  Enables hardware blocks in the component
*
* Parameters:
*  None
*
* Return:
*  None
*
* Side Effects:
*  None
*
****************************************************************************/
void `$INSTANCE_NAME`_Enable(void) `=ReentrantKeil($INSTANCE_NAME . "_Enable")`
{
    uint8 interruptState;
    
    /* Write to control register based on customizer defaults */
    if(`$INSTANCE_NAME`_INIT_ALERT_ENABLE)
    {
        `$INSTANCE_NAME`_GLOBAL_CONTROL_REG =
            (`$INSTANCE_NAME`_ENABLE) |
            (`$INSTANCE_NAME`_INIT_ALERT_ENABLE << `$INSTANCE_NAME`_ALERT_ENABLE_SHIFT) |
            (`$INSTANCE_NAME`_ALERT_PIN_ENABLE) ;
    }
    else
    {
        `$INSTANCE_NAME`_GLOBAL_CONTROL_REG = `$INSTANCE_NAME`_ENABLE;
    }
    
    interruptState = CyEnterCriticalSection();
    
    /* Enable alerts from the Alert Status register */
    `$INSTANCE_NAME`_STATUS_ALERT_AUX_CTL_REG |= `$INSTANCE_NAME`_STATUS_ALERT_ENABLE ;
    
    CyExitCriticalSection(interruptState);
}


/******************************************************************************
* Function Name: `$INSTANCE_NAME`_EnableAlert
*******************************************************************************
*
* Summary:
*  Enables alerts from this component. Specifically which alert sources are
*  enabled is configured using SetAlertMode() API.  This API only
*  enables alert conditions to propogate to the Fan Controller component
*  Alert output/pin
*
* Parameters:
*  None
*
* Return:
*  None
*
* Side Effects:
*  None
*
****************************************************************************/
void `$INSTANCE_NAME`_EnableAlert(void) `=ReentrantKeil($INSTANCE_NAME . "_EnableAlert")`
{
    uint8 interruptState = CyEnterCriticalSection();
    
    `$INSTANCE_NAME`_GLOBAL_CONTROL_REG |= `$INSTANCE_NAME`_ALERT_PIN_ENABLE;
    
    CyExitCriticalSection(interruptState);
}


/******************************************************************************
* Function Name: `$INSTANCE_NAME`_DisableAlert
*******************************************************************************
*
* Summary:
*  Disables alerts from this component. This API only disables alert
*  conditions from propogating to the fan controller component Alert output/pin
*
* Parameters:
*  None
*
* Return:
*  None
*
* Side Effects:
*  None
*
****************************************************************************/
void `$INSTANCE_NAME`_DisableAlert(void) `=ReentrantKeil($INSTANCE_NAME . "_DisableAlert")`
{
    uint8 interruptState = CyEnterCriticalSection();
    
    `$INSTANCE_NAME`_GLOBAL_CONTROL_REG &= ~`$INSTANCE_NAME`_ALERT_PIN_ENABLE;
    
    CyExitCriticalSection(interruptState);
}


/******************************************************************************
* Function Name: `$INSTANCE_NAME`_SetAlertMode
*******************************************************************************
*
* Summary:
*  Enables alert sources from the component.
*
* Parameters:
*  alertMode:
*   b0:   STALL_ALERT_ENABLE (1==Enable, 0==Disable)
*   b1:   SPEED_ALERT_ENABLE (1==Enable, 0==Disable)
*   b7-2: RESERVED (write with zeroes)
*
* Return:
*  None
*
* Side Effects:
*  None
*
****************************************************************************/
void `$INSTANCE_NAME`_SetAlertMode(uint8 alertMode) `=ReentrantKeil($INSTANCE_NAME . "_SetAlertMode")`
{
    uint8 interruptState = CyEnterCriticalSection();
    
    `$INSTANCE_NAME`_GLOBAL_CONTROL_REG = (`$INSTANCE_NAME`_GLOBAL_CONTROL_REG & ~`$INSTANCE_NAME`_ALERT_ENABLE_MASK) |
               ((alertMode << `$INSTANCE_NAME`_ALERT_ENABLE_SHIFT) & `$INSTANCE_NAME`_ALERT_ENABLE_MASK);
    
    CyExitCriticalSection(interruptState);
}


/******************************************************************************
* Function Name: `$INSTANCE_NAME`_GetAlertMode
*******************************************************************************
*
* Summary:
* Returns enabled alert sources from this component
*
* Parameters:
*  None
*
* Return:
*  alertMode:
*   b0:   STALL_ALERT_ENABLE (1==Enable, 0==Disable)
*   b1:   SPEED_ALERT_ENABLE (1==Enable, 0==Disable)
*   b7-2: RESERVED (write with zeroes)
*
* Side Effects:
*  None
*
****************************************************************************/
uint8 `$INSTANCE_NAME`_GetAlertMode(void) `=ReentrantKeil($INSTANCE_NAME . "_GetAlertMode")`
{
    return((`$INSTANCE_NAME`_GLOBAL_CONTROL_REG & `$INSTANCE_NAME`_ALERT_ENABLE_MASK) >>
                `$INSTANCE_NAME`_ALERT_ENABLE_SHIFT);
}


/******************************************************************************
* Function Name: `$INSTANCE_NAME`_SetAlertMask
*******************************************************************************
*
* Summary:
*  Sets the per fan Alert Mask for both Stall and Speed errors
*
* Parameters:
*  alertMask (1==Enable Alert, 0==Disable Alert)
*   b0:   FAN1 ALERT ENABLE/DISABLE
*   b1:   FAN2 ALERT ENABLE/DISABLE
*   ...
*   b15:  FAN16 ALERT ENABLE/DISABLE
*
* Return:
*  None
*
* Side Effects:
*  None
*
****************************************************************************/
void `$INSTANCE_NAME`_SetAlertMask(uint16 alertMask) `=ReentrantKeil($INSTANCE_NAME . "_SetAlertMask")`
{
    `$INSTANCE_NAME`_ALERT_MASK_LSB_CONTROL_REG = LO8(alertMask);

    #if(`$INSTANCE_NAME`_NUMBER_OF_FANS > 8u)
        `$INSTANCE_NAME`_ALERT_MASK_MSB_CONTROL_REG = HI8(alertMask);
    #endif /* `$INSTANCE_NAME`_NUMBER_OF_FANS > 8u */
}


/******************************************************************************
* Function Name: `$INSTANCE_NAME`_GetAlertMask
*******************************************************************************
*
* Summary:
*  Gets the current per fan Alert Mask for both Stall and Speed errors
*
* Parameters:
*  None
*
* Return:
*  alertMask (1==Alert Enabled, 0==Alert Disabled)
*   b0:   FAN1 ALERT ENABLE/DISABLE
*   b1:   FAN2 ALERT ENABLE/DISABLE
*   ...
*   b15:  FAN16 ALERT ENABLE/DISABLE
*
* Side Effects:
*  None
*
****************************************************************************/
uint16 `$INSTANCE_NAME`_GetAlertMask(void) `=ReentrantKeil($INSTANCE_NAME . "_GetAlertMask")`
{
    uint16 alertMask;

    #if(`$INSTANCE_NAME`_NUMBER_OF_FANS > 8u)
        alertMask = (uint16)`$INSTANCE_NAME`_ALERT_MASK_LSB_CONTROL_REG |
            ((uint16)`$INSTANCE_NAME`_ALERT_MASK_MSB_CONTROL_REG << 8u);
    #else
        alertMask = (uint16)`$INSTANCE_NAME`_ALERT_MASK_LSB_CONTROL_REG;
    #endif /* `$INSTANCE_NAME`_NUMBER_OF_FANS > 8u */

    return(alertMask);
}


/******************************************************************************
* Function Name: `$INSTANCE_NAME`_GetAlertSource
*******************************************************************************
*
* Summary:
*  Returns pending alert sources. The alert pin is not de-asserted through
*  this API call. If this API call returns a non-zero value, call the
*  GetFanStallStatus() or GetFanSpeedStatus() APIs to determine the exact
*  source of the alert and simultaneously de-assert the pending alert and
*  alert pin.
*
* Parameters:
*  None
*
* Return:
*  alertSource:
*   b0:   STALL_ALERT (1==Present, 0==Not Present)
*   b1:   SPEED_ALERT (1==Present, 0==Not Present)
*   b7-2: RESERVED (returns all zeroes)
*
* Side Effects:
*  None
*
****************************************************************************/
uint8 `$INSTANCE_NAME`_GetAlertSource(void) `=ReentrantKeil($INSTANCE_NAME . "_GetAlertSource")`
{
    return ((`$INSTANCE_NAME`_ALERT_STATUS_REG & `$INSTANCE_NAME`_ALERT_STATUS_MASK));
}


/******************************************************************************
* Function Name: `$INSTANCE_NAME`_GetFanStallStatus
*******************************************************************************
*
* Summary:
*  Returns the stall status of all fans
*
* Parameters:
*  None
*
* Return:
*  stallStatus (1==Fan Stalled, 0=Fan OK)
*   b0:   FAN1 STALL
*   b1:   FAN2 STALL
*   ...
*   b15:  FAN16 STALL
*
* Side Effects:
*  Calling this API de-asserts the alert pin and clears all pending stall
*  status alerts
*
******************************************************************************/
uint16 `$INSTANCE_NAME`_GetFanStallStatus(void) `=ReentrantKeil($INSTANCE_NAME . "_GetFanStallStatus")`
{
    uint16 stallStatus;

    #if(`$INSTANCE_NAME`_NUMBER_OF_FANS > 8u)
        stallStatus = (uint16)`$INSTANCE_NAME`_STALL_ERROR_LSB_STATUS_REG |
            ((uint16)`$INSTANCE_NAME`_STALL_ERROR_MSB_STATUS_REG << 8u);
    #else
        stallStatus = (uint16)`$INSTANCE_NAME`_STALL_ERROR_LSB_STATUS_REG;
    #endif /* `$INSTANCE_NAME`_NUMBER_OF_FANS > 8u */


    return(stallStatus);
}


#if(`$INSTANCE_NAME`_FAN_CTL_MODE == `$INSTANCE_NAME`_FANCTLMODE_HARDWARE)
    /******************************************************************************
    * Function Name: `$INSTANCE_NAME`_GetFanSpeedStatus
    *******************************************************************************
    *
    * Summary:
    *  Returns the speed regulation status of all fans.
    *  This API is not available if Firmware Controller fan control methed is
    *  selected.
    *
    * Parameters:
    *  None
    *
    * Return:
    *  speedStatus (1==Fan Speed Failure, 0=Fan OK)
    *   b0:   FAN1 SPEED REGULATION FAIL
    *   b1:   FAN2 SPEED REGULATION FAIL
    *   ...
    *   b15:  FAN16 SPEED REGULATION FAIL
    *
    * Side Effects:
    *  Calling this API de-asserts the alert pin and clears all pending speed
    *  regulation failure status alerts
    *
    ******************************************************************************/
    uint16 `$INSTANCE_NAME`_GetFanSpeedStatus(void) `=ReentrantKeil($INSTANCE_NAME . "_GetFanSpeedStatus")`
    {
        uint16 speedStatus;

        #if(`$INSTANCE_NAME`_NUMBER_OF_FANS > 8u)
            speedStatus = (uint16)`$INSTANCE_NAME`_SPEED_ERROR_LSB_STATUS_REG |
                ((uint16)`$INSTANCE_NAME`_SPEED_ERROR_MSB_STATUS_REG << 8u);
        #else
            speedStatus = (uint16)`$INSTANCE_NAME`_SPEED_ERROR_LSB_STATUS_REG;
        #endif /* `$INSTANCE_NAME`_NUMBER_OF_FANS > 8u */

        return(speedStatus);
    }

#endif /* `$INSTANCE_NAME`_FAN_CTL_MODE == `$INSTANCE_NAME`_FANCTLMODE_HARDWARE */


/******************************************************************************
* Function Name: `$INSTANCE_NAME`_SetDutyCycle
*******************************************************************************
*
* Summary:
*  Sets the duty cycle of selected fan or bank in hundredths of a percent,
*  i.e. 5000=50% duty cycle. In hardware controlled fan mode, if manual duty
*  cycle control is desirable, call the OverrideHardwareControl(true) API prior
*  to calling this API.
*
* Parameters:
*  uint8 fanOrBankNumber
*   Valid range is 1-16 and should not exceed the number of fans or banks in the
*   system
*
*  uint16 dutyCycle
*   dutyCycle is entered in hundredths of a percent, i.e. 5000=50% duty cycle
*   Valid range is 0 to 10000
*
* Return:
*  None
*
* Side Effects:
*  None
*
****************************************************************************/
void `$INSTANCE_NAME`_SetDutyCycle(uint8 fanOrBankNumber, uint16 dutyCycle)
        `=ReentrantKeil($INSTANCE_NAME . "_SetDutyCycle")`
{
    uint32 newCompare;      /* Needs to be 32-bit to allow for overflow during the math calculations */

    /* Make sure the Fan/Bank number is valid */
    if(fanOrBankNumber <= `$INSTANCE_NAME`_NUMBER_OF_FAN_OUTPUTS)
    {
         newCompare = (`$INSTANCE_NAME`_PWM_PERIOD * (uint32) dutyCycle) / `$INSTANCE_NAME`_PWM_DUTY_DIVIDER;

        /* Make sure the Compare value is in range */
        if(newCompare <= `$INSTANCE_NAME`_PWM_PERIOD)
        {
            #if(`$INSTANCE_NAME`_PWMRES == `$INSTANCE_NAME`_PWMRES_EIGHTBIT)
                CY_SET_REG8(`$INSTANCE_NAME`_fanDriverRegs[fanOrBankNumber - 1].pwmSetDutyReg, newCompare);
            #else
                CY_SET_REG16(`$INSTANCE_NAME`_fanDriverRegs[fanOrBankNumber - 1].pwmSetDutyReg, newCompare);
            #endif /* `$INSTANCE_NAME`_PWMRES == `$INSTANCE_NAME`_PWMRES_EIGHTBIT */
        }
    }
}


/******************************************************************************
* Function Name: `$INSTANCE_NAME`_GetDutyCycle
*******************************************************************************
*
* Summary:
*  Returns the current duty cycle of the selected fan or bank in hundredths
*  of a percent, i.e. 5000=50% duty cycle
*
* Parameters:
*  uint8 fanOrBankNumber
*  Valid range is 1-16 and should not exceed the number of fans or banks in 
*  the system.
*
* Return:
*  uint16 duyCycle
*  Current duty cycle in hundredths of a percent
*
* Side Effects:
*  None
*
****************************************************************************/
uint16 `$INSTANCE_NAME`_GetDutyCycle(uint8 fanOrBankNumber) `=ReentrantKeil($INSTANCE_NAME . "_GetDutyCycle")`
{
    uint16 duyCycle = 0u;

    if(fanOrBankNumber <= `$INSTANCE_NAME`_NUMBER_OF_FAN_OUTPUTS)
    {
        #if(`$INSTANCE_NAME`_PWMRES == `$INSTANCE_NAME`_PWMRES_EIGHTBIT)
            duyCycle = ((uint32) CY_GET_REG8(`$INSTANCE_NAME`_fanDriverRegs[fanOrBankNumber - 1u].pwmSetDutyReg)
                    * `$INSTANCE_NAME`_PWM_DUTY_DIVIDER) / (`$INSTANCE_NAME`_PWM_PERIOD);
        #else
            duyCycle = ((uint32) CY_GET_REG16(`$INSTANCE_NAME`_fanDriverRegs[fanOrBankNumber - 1u].pwmSetDutyReg)
                    * `$INSTANCE_NAME`_PWM_DUTY_DIVIDER) / (`$INSTANCE_NAME`_PWM_PERIOD);
        #endif /* `$INSTANCE_NAME`_PWMRES == `$INSTANCE_NAME`_PWMRES_EIGHTBIT */
    }

    return(duyCycle);
}


/******************************************************************************
* Function Name: `$INSTANCE_NAME`_SetDesiredSpeed
*******************************************************************************
*
* Summary:
*  Sets the desired RPM of specified fan. If the AcousticNoiseReduction
*  parameter is true (enabled via the component customizer) in hardware
*  controlled fan mode, the desired speed is converted to a fan rotation period
*  and written to SRAM for subsequent DMA into the tachometer block. If the
*  AcousticNoiseReduction parameter is false, the desired speed is converted to
*  a duty cycle and written directly into the PWM register for the selected fan.
*
* Parameters:
*  uint8 fanNumber
*  Valid range is 1-16 and should not exceed the number of fans in the system.
*
* uint16 rpm
*  Valid range is 500 to 25,000, but should not exceed the max RPM of the
*  selected fan.
*
* Return:
*  None
*
* Side Effects:
*  None
*
****************************************************************************/
void `$INSTANCE_NAME`_SetDesiredSpeed(uint8 fanNumber, uint16 rpm) `=ReentrantKeil($INSTANCE_NAME . "_SetDesiredSpeed")`
{
    uint32 overrideDuty;        /* Needs to be 32-bit to allow for overflow during the math calculations */
    uint16 currentSpeed;

    /* Check for valid Fan number */
    if(fanNumber <= `$INSTANCE_NAME`_NUMBER_OF_FANS)
    {
        currentSpeed = `$INSTANCE_NAME`_GetActualSpeed(fanNumber);

        /* 3 cases where we will directly write a new duty cycle in firmware: */
            /* Case 1) firmware controlled fan mode */
        if(( `$INSTANCE_NAME`_FAN_CTL_MODE == `$INSTANCE_NAME`_FANCTLMODE_FIRMWARE)   ||
            /* Case 2) hardware controlled fan mode and noise reduction is off */
            ((`$INSTANCE_NAME`_FAN_CTL_MODE == `$INSTANCE_NAME`_FANCTLMODE_HARDWARE) &&
                (`$INSTANCE_NAME`_NOISE_REDUCTION_MODE == `$INSTANCE_NAME`_NOISE_REDUCTION_OFF)) ||
            /* Case 3) hardware controlled fan mode and new rpm is slower than current */
            ((`$INSTANCE_NAME`_FAN_CTL_MODE == `$INSTANCE_NAME`_FANCTLMODE_HARDWARE) && (rpm < currentSpeed)))
        {
            /* Override hardware controlled fan mode temporarily to write new duty cycle */
            #if(`$INSTANCE_NAME`_FAN_CTL_MODE == `$INSTANCE_NAME`_FANCTLMODE_HARDWARE)
                `$INSTANCE_NAME`_OverrideHardwareControl(1u);
            #endif /* `$INSTANCE_NAME`_FAN_CTL_MODE == `$INSTANCE_NAME`_FANCTLMODE_HARDWARE */

            /* Desired speed is slower than rpmA */
            if(rpm < `$INSTANCE_NAME`_fanProperties[fanNumber - 1u].rpmA)
            {
                overrideDuty =  (uint32)`$INSTANCE_NAME`_fanProperties[fanNumber - 1u].dutyA -
                                (((uint32)`$INSTANCE_NAME`_fanProperties[fanNumber - 1u].dutyRpmSlope *
                                   ((uint32)`$INSTANCE_NAME`_fanProperties[fanNumber - 1u].rpmA - (uint32)rpm)) / 100u);

                /* Check for math underflow */
                if(overrideDuty > 10000u)
                {
                    overrideDuty = 0u;
                }
            }

            /* Desired speed is faster than rpmA */
            else
            {
                overrideDuty =  (uint32)`$INSTANCE_NAME`_fanProperties[fanNumber - 1u].dutyA +
                                (((uint32)`$INSTANCE_NAME`_fanProperties[fanNumber - 1u].dutyRpmSlope *
                                   ((uint32)rpm - (uint32)`$INSTANCE_NAME`_fanProperties[fanNumber - 1u].rpmA)) / 100u);

                /* Check for math overflow */
                if(overrideDuty > 10000u)
                {
                    overrideDuty = 10000u;
                }
            }
        
            /* Set the newly calculated duty cycle */
            `$INSTANCE_NAME`_SetDutyCycle(fanNumber, LO16(overrideDuty));
        }

        /* Set newly requested desired rpm including tolerance calculation and store
        * in SRAM for the tachometer block DMA controller
        */
        #if(`$INSTANCE_NAME`_FAN_CTL_MODE == `$INSTANCE_NAME`_FANCTLMODE_HARDWARE)
            `$INSTANCE_NAME`_fanControl.desiredPeriod[fanNumber - 1u] =
                `$INSTANCE_NAME`_RPM_PERIOD_CONV_FACTOR / (uint32) (rpm * (1 + `$INSTANCE_NAME`_TOLERANCE_FACTOR));
            if(rpm > 500u)
            {
                `$INSTANCE_NAME`_fanControl.tolerance[fanNumber - 1u] =
                    (`$INSTANCE_NAME`_RPM_PERIOD_CONV_FACTOR * `$INSTANCE_NAME`_TOLERANCE_FACTOR) / (uint32) rpm;
            }
            else
            {
                `$INSTANCE_NAME`_fanControl.tolerance[fanNumber - 1u] = 100u;
            }

            /* Go back to hardware controlled fan mode */
            `$INSTANCE_NAME`_OverrideHardwareControl(0u);
        #endif /* `$INSTANCE_NAME`_FAN_CTL_MODE == `$INSTANCE_NAME`_FANCTLMODE_HARDWARE */
    }
}


#if(`$INSTANCE_NAME`_FAN_CTL_MODE == `$INSTANCE_NAME`_FANCTLMODE_HARDWARE)
    /******************************************************************************
    * Function Name: `$INSTANCE_NAME`_GetDesiredSpeed
    *******************************************************************************
    *
    * Summary:
    *  Returns the currently desired speed of the selected fan in RPMs in hardware
    *  controlled fan mode. This API is not available if firmware controlled fan 
    *  mode is selected.
    *
    * Parameters:
    *  uint8 fanNumber
    *  Valid range is 1-16 and should not exceed the number of fans in the system.
    *
    * Return:
    *  uint16 desiredSpeed: Current desired speed for selected fan
    *
    * Side Effects:
    *  None
    *
    ******************************************************************************/
    uint16 `$INSTANCE_NAME`_GetDesiredSpeed(uint8 fanNumber) `=ReentrantKeil($INSTANCE_NAME . "_GetDesiredSpeed")`
    {
        uint16 desiredSpeed = 0u;

        if(fanNumber <= `$INSTANCE_NAME`_NUMBER_OF_FANS)
        {
            desiredSpeed = `$INSTANCE_NAME`_RPM_PERIOD_CONV_FACTOR /
                (uint32) (`$INSTANCE_NAME`_fanControl.desiredPeriod[fanNumber - 1u] *
                    (1u + `$INSTANCE_NAME`_TOLERANCE_FACTOR));
        }

        return(desiredSpeed);
    }

#endif /* `$INSTANCE_NAME`_FAN_CTL_MODE == `$INSTANCE_NAME`_FANCTLMODE_HARDWARE */


/******************************************************************************
* Function Name: `$INSTANCE_NAME`_GetActualSpeed
*******************************************************************************
*
* Summary:
*  Returns the actual measured RPM of selected fan.
*
* Parameters:
*  uint8 fanNumber
*  Valid range is 1-16 and should not exceed the number of fans in the system.
*
* Return:
*  uint16 actualSpeed: Actual measured RPM of target fan
*
* Side Effects:
*  None
*
****************************************************************************/
uint16 `$INSTANCE_NAME`_GetActualSpeed(uint8 fanNumber) `=ReentrantKeil($INSTANCE_NAME . "_GetActualSpeed")`
{
    uint16 actualSpeed = 0u;

    if(fanNumber <= `$INSTANCE_NAME`_NUMBER_OF_FANS)
    {
        if(`$INSTANCE_NAME`_fanControl.actualPeriod[fanNumber - 1u] != 0u)
        {
            actualSpeed = `$INSTANCE_NAME`_RPM_PERIOD_CONV_FACTOR /
                (uint32) `$INSTANCE_NAME`_fanControl.actualPeriod[fanNumber - 1u];
        }
    }

    return(actualSpeed);
}


#if(`$INSTANCE_NAME`_FAN_CTL_MODE == `$INSTANCE_NAME`_FANCTLMODE_HARDWARE)
    /******************************************************************************
    * Function Name: `$INSTANCE_NAME`_OverrideHardwareControl
    *******************************************************************************
    *
    * Summary:
    *  Formerly called OverrideClosedLoop.
    *
    *  Allows firmware to take over fan control when hardware controlled fan mode
    *  is enabled. That is, directly control fan speed using the SetDutyCycle() API.
    *  This API is not available if firmware controlled fan mode is selected.
    *
    * Parameters:
    *  uint8 override
    *   non-zero = firmware assumes control of fans
    *   zero     = hardware assumes control of fans
    *
    * Return:
    *  None
    *
    * Side Effects:
    *  None
    ******************************************************************************/

    void `$INSTANCE_NAME`_OverrideHardwareControl(uint8 override)
                                `=ReentrantKeil($INSTANCE_NAME . "_OverrideHardwareControl")`
    {
        uint8 interruptState = CyEnterCriticalSection();
        
        if(override != 0u)
        {
            `$INSTANCE_NAME`_GLOBAL_CONTROL_REG |= `$INSTANCE_NAME`_OVERRIDE;
        }
        else
        {
            `$INSTANCE_NAME`_GLOBAL_CONTROL_REG &= ~`$INSTANCE_NAME`_OVERRIDE;
        }
        
        CyExitCriticalSection(interruptState);
        
    }

#endif /* `$INSTANCE_NAME`_FAN_CTL_MODE == `$INSTANCE_NAME`_FANCTLMODE_HARDWARE */


/******************************************************************************
* Function Name: uint8 `$INSTANCE_NAME`_SetupDMA
*******************************************************************************
*
* Summary:
*  This is a private API not exposed to users.
*
*  Sets up the DMA controllers depending on firmware or hardware controlled fan
*  mode. The number and the sequence of the transaction descriptors depends on
*  the number of fans in the system.
*
*  The NRQ output of the DMA controllers is used in different ways depending
*  on the fan control mode:
*
*  1) In firmware controlled fan mode, the NRQ of the TachOutDMA is asserted
*     only once at the end of the TD chain. This is used to generate the
*     end-of-cylce (eoc) pulse for the component.
*
*  2) In hardware controlled fan mode, the NRQ of the TachOutDMA is asserted
*     after every TD and connects to the DRQ of the TachInDMA. This ensures
*     that reading desired speeds and hysteresis into the tachometer block 
*     occurs automatically when the fan input is changed. In this 
*     configuration, the NRQ output of the TachInDMA is used to generate the 
*     end-of-cylce (eoc) pulse for the component.
*
* Parameters:
*  None
*
* Return:
*  None
*
* Side Effects:
*  None
*
****************************************************************************/
void `$INSTANCE_NAME`_SetupDMA(void) CYREENTRANT
{
    uint8 counti;
    uint8 fanNum;

    /* Get DMA controller channels allocated */
    /* PSoC 3 family memory spaces */
    #if defined(__C51__)
        `$INSTANCE_NAME`_TachOutDMA_channel =       `$INSTANCE_NAME`_TachOutDMA_DmaInitialize(
                                                    2u,
                                                    1u,
                                                    HI16(CYDEV_PERIPH_BASE),
                                                    HI16(CYDEV_SRAM_BASE));

        #if(`$INSTANCE_NAME`_FAN_CTL_MODE == `$INSTANCE_NAME`_FANCTLMODE_HARDWARE)
            `$INSTANCE_NAME`_TachInDMA_channel =    `$INSTANCE_NAME`_TachInDMA_DmaInitialize(
                                                    2u,
                                                    1u,
                                                    HI16(CYDEV_SRAM_BASE),
                                                    HI16(CYDEV_PERIPH_BASE));
        #endif /* `$INSTANCE_NAME`_FAN_CTL_MODE == `$INSTANCE_NAME`_FANCTLMODE_HARDWARE */

    /* PSoC 5 family memory spaces */
    #else
        `$INSTANCE_NAME`_TachOutDMA_channel =       `$INSTANCE_NAME`_TachOutDMA_DmaInitialize(
                                                    2u,
                                                    1u,
                                                    HI16(`$INSTANCE_NAME`_TACH_ACTUAL_PERIOD_PTR),
                                                    HI16(((uint32)&`$INSTANCE_NAME`_fanControl.actualPeriod[0])));
                                                    
        #if(`$INSTANCE_NAME`_FAN_CTL_MODE == `$INSTANCE_NAME`_FANCTLMODE_HARDWARE)
            `$INSTANCE_NAME`_TachInDMA_channel =    `$INSTANCE_NAME`_TachInDMA_DmaInitialize(
                                                    2u,
                                                    1u,
                                                    HI16(((uint32)&`$INSTANCE_NAME`_fanControl.desiredPeriod[0])),
                                                    HI16(`$INSTANCE_NAME`_TACH_DESIRED_PERIOD_PTR));
        #endif /* `$INSTANCE_NAME`_FAN_CTL_MODE == `$INSTANCE_NAME`_FANCTLMODE_HARDWARE */
    #endif /* __C51__ */

    /* Get transaction descriptors allocated */
    for (counti = 0u; counti < `$INSTANCE_NAME`_NUMBER_OF_FANS; counti++)
    {
        `$INSTANCE_NAME`_fanOutTds[counti].setActualPeriodTD = CyDmaTdAllocate();

        #if(`$INSTANCE_NAME`_FAN_CTL_MODE == `$INSTANCE_NAME`_FANCTLMODE_HARDWARE)
            `$INSTANCE_NAME`_fanInTds[counti].getDesiredPeriodTD = CyDmaTdAllocate();
            `$INSTANCE_NAME`_fanInTds[counti].getToleranceTD = CyDmaTdAllocate();
        #endif /* `$INSTANCE_NAME`_FAN_CTL_MODE == `$INSTANCE_NAME`_FANCTLMODE_HARDWARE */
    }
    
    /* Configure the transaction descriptors and sequence depending on fan control mode */
    for (counti = 0u; counti < `$INSTANCE_NAME`_NUMBER_OF_FANS; counti++)
    {
        fanNum = `$INSTANCE_NAME`_NUMBER_OF_FANS - counti - 1u;

        /* Put current actual tachometer periods (hardware and firmware
        *   controlled fan modes) to SRAM for the GetActualSpeed API
        */
        if(fanNum == 0u)
        {
            /* TDs need to be sequenced in reverse to match the Tachometer hardware
            * block address bus sequence. Next TD after fan[0] is final fan:
            fan[NUMBER_OF_FANS-1]
            */
            CyDmaTdSetConfiguration(`$INSTANCE_NAME`_fanOutTds[fanNum].setActualPeriodTD,
                                    2u,
                                    `$INSTANCE_NAME`_fanOutTds[`$INSTANCE_NAME`_NUMBER_OF_FANS - 1u].setActualPeriodTD,
                                    TD_INC_DST_ADR| `$INSTANCE_NAME`_TD_SWAP_ENDIAN_FLAG  |
                                    /* Assert NRQ at final TD in hardware and firmware controlled fan modes */
                                    `$INSTANCE_NAME`_TachOutDMA__TD_TERMOUT_EN);
        }
        else
        {
            /* TDs need to be sequenced in reverse to match the Tachometer hardware
            * block address bus sequence. Next TD after fan[n] is fan[n-1] */
            #if(`$INSTANCE_NAME`_FAN_CTL_MODE == `$INSTANCE_NAME`_FANCTLMODE_HARDWARE)
                CyDmaTdSetConfiguration(`$INSTANCE_NAME`_fanOutTds[fanNum].setActualPeriodTD,
                                        2u,
                                        `$INSTANCE_NAME`_fanOutTds[fanNum - 1u].setActualPeriodTD,
                                        TD_INC_DST_ADR|`$INSTANCE_NAME`_TD_SWAP_ENDIAN_FLAG  |
                                        /* Also assert NRQ at end of each TD in hardware controlled fan mode */
                                        `$INSTANCE_NAME`_TachOutDMA__TD_TERMOUT_EN);
            #else
                CyDmaTdSetConfiguration(`$INSTANCE_NAME`_fanOutTds[fanNum].setActualPeriodTD,
                                        2u,
                                        `$INSTANCE_NAME`_fanOutTds[fanNum - 1u].setActualPeriodTD,
                                        TD_INC_DST_ADR|
                                        /* Don't assert NRQ at end of each TD in firmware controlled fan mode */
                                        `$INSTANCE_NAME`_TD_SWAP_ENDIAN_FLAG);
            #endif /* `$INSTANCE_NAME`_FAN_CTL_MODE == `$INSTANCE_NAME`_FANCTLMODE_HARDWARE */
        }

        /* Get desired tachometer periods and hysteresis (hardware controlled fan mode
        * only) from SRAM from the SetActualSpeed API
        */
        #if(`$INSTANCE_NAME`_FAN_CTL_MODE == `$INSTANCE_NAME`_FANCTLMODE_HARDWARE)
             /* Get desired tachometer periods */
            CyDmaTdSetConfiguration(`$INSTANCE_NAME`_fanInTds[fanNum].getDesiredPeriodTD,
                                    2u,
                                    `$INSTANCE_NAME`_fanInTds[fanNum].getToleranceTD,
                                    TD_INC_SRC_ADR| `$INSTANCE_NAME`_TD_SWAP_ENDIAN_FLAG | TD_AUTO_EXEC_NEXT);

            /* And get tachometer tolerances - either chain to the next fan OR wrap around to the last fan */
            if(fanNum == 0u)
            {
                /* Next TD after fan[0] is final fan: fan[NUMBER_OF_FANS-1] */
                CyDmaTdSetConfiguration(`$INSTANCE_NAME`_fanInTds[fanNum].getToleranceTD,
                                     2u,
                                     `$INSTANCE_NAME`_fanInTds[`$INSTANCE_NAME`_NUMBER_OF_FANS - 1u].getDesiredPeriodTD,
                                     TD_INC_SRC_ADR| `$INSTANCE_NAME`_TD_SWAP_ENDIAN_FLAG |
                                     `$INSTANCE_NAME`_TachInDMA__TD_TERMOUT_EN);  /* Assert NRQ at end of all TDs */
            }
            else
            {
                /* Next TD after fan[n] is fan[n-1] */
                CyDmaTdSetConfiguration(`$INSTANCE_NAME`_fanInTds[fanNum].getToleranceTD,
                                        2u,
                                        `$INSTANCE_NAME`_fanInTds[fanNum - 1u].getDesiredPeriodTD,
                                        TD_INC_SRC_ADR |
                                        /* Don't assert NRQ on intermediate TDs */
                                        `$INSTANCE_NAME`_TD_SWAP_ENDIAN_FLAG);
            }
        #endif /* `$INSTANCE_NAME`_FAN_CTL_MODE == `$INSTANCE_NAME`_FANCTLMODE_HARDWARE */
    }

    /* Setup SRAM source/destination addresses and datapath register addreses */
    for (counti = 0u; counti < `$INSTANCE_NAME`_NUMBER_OF_FANS; counti++)
    {
        /* TD for actual period to RAM */
        CyDmaTdSetAddress(`$INSTANCE_NAME`_fanOutTds[counti].setActualPeriodTD,
                          LO16(`$INSTANCE_NAME`_TACH_ACTUAL_PERIOD_PTR),
                          LO16(((uint32)&`$INSTANCE_NAME`_fanControl.actualPeriod[counti])));

        #if(`$INSTANCE_NAME`_FAN_CTL_MODE == `$INSTANCE_NAME`_FANCTLMODE_HARDWARE)
            /* TD for desired period to tachometer */
            CyDmaTdSetAddress(`$INSTANCE_NAME`_fanInTds[counti].getDesiredPeriodTD,
                            LO16(((uint32)&`$INSTANCE_NAME`_fanControl.desiredPeriod[counti])),
                            LO16(`$INSTANCE_NAME`_TACH_DESIRED_PERIOD_PTR));

            /* TD for tolerance to tachometer */
            CyDmaTdSetAddress(`$INSTANCE_NAME`_fanInTds[counti].getToleranceTD,
                            LO16(((uint32)&`$INSTANCE_NAME`_fanControl.tolerance[counti])),
                            LO16(`$INSTANCE_NAME`_TACH_TOLERANCE_PTR));
        #endif /* `$INSTANCE_NAME`_FAN_CTL_MODE == `$INSTANCE_NAME`_FANCTLMODE_HARDWARE */
    }

    /* Set the initial transaction descriptor to kick things off */
    CyDmaChSetInitialTd(`$INSTANCE_NAME`_TachOutDMA_channel, `$INSTANCE_NAME`_fanOutTds[0u].setActualPeriodTD);
    CyDmaChEnable(`$INSTANCE_NAME`_TachOutDMA_channel, 1u);

    #if(`$INSTANCE_NAME`_FAN_CTL_MODE == `$INSTANCE_NAME`_FANCTLMODE_HARDWARE)
        CyDmaChSetInitialTd(`$INSTANCE_NAME`_TachInDMA_channel,  `$INSTANCE_NAME`_fanInTds[0u].getDesiredPeriodTD);
        CyDmaChEnable(`$INSTANCE_NAME`_TachInDMA_channel, 1u);
    #endif /* `$INSTANCE_NAME`_FAN_CTL_MODE == `$INSTANCE_NAME`_FANCTLMODE_HARDWARE */
}


/*********************************************************************
* Function Name: uint8 `$INSTANCE_NAME`_TachOutDMA_DmaInitalize
**********************************************************************
*
* Summary:
*  This is a private API not exposed to users.
*
*  Allocates and initialises a channel of the DMAC to be used by the
*  caller.
*
* Parameters:
*  uint8  burstCount
*  uint8  requestPerBurst
*  uint16 upperSrcAddress
*  uint16 upperDestAddress
*
* Return:
*  The channel that can be used by the caller for DMA activity.
*  DMA_INVALID_CHANNEL (0xFF) if there are no channels left.
*
* Side Effects:
*  None
*
*****************************************************************/
uint8 `$INSTANCE_NAME`_TachOutDMA_DmaInitialize(uint8 burstCount, uint8 requestPerBurst, uint16 upperSrcAddress,
                                                    uint16 upperDestAddress) CYREENTRANT
{
    /* Allocate a DMA channel */
    `$INSTANCE_NAME`_TachOutDMA_dmaHandle = `$INSTANCE_NAME`_TachOutDMA__DRQ_NUMBER;

    if(`$INSTANCE_NAME`_TachOutDMA_dmaHandle != DMA_INVALID_CHANNEL)
    {
        /* Configure the channel */
        CyDmaChSetConfiguration(`$INSTANCE_NAME`_TachOutDMA_dmaHandle,
                                burstCount,
                                requestPerBurst,
                                `$INSTANCE_NAME`_TachOutDMA__TERMOUT0_SEL,
                                `$INSTANCE_NAME`_TachOutDMA__TERMOUT1_SEL,
                                `$INSTANCE_NAME`_TachOutDMA__TERMIN_SEL);

        /* Set the extended address for the transfers */
        CyDmaChSetExtendedAddress(`$INSTANCE_NAME`_TachOutDMA_dmaHandle, upperSrcAddress, upperDestAddress);

        /* Set the priority for this channel */
        CyDmaChPriority(`$INSTANCE_NAME`_TachOutDMA_dmaHandle, `$INSTANCE_NAME`_TachOutDMA__PRIORITY);
    }

    return(`$INSTANCE_NAME`_TachOutDMA_dmaHandle);
}


/*********************************************************************
* Function Name: void `$INSTANCE_NAME`_TachOutDMA_DmaRelease
**********************************************************************
*
* Summary:
*   Frees the channel associated with `$INSTANCE_NAME`_TachOutDMA and
*   also frees the TD descriptors
*
* Parameters:
*   none
*
* Return:
*   none
*
* Side Effects:
*  None
*
*****************************************************************/
void `$INSTANCE_NAME`_TachOutDMA_DmaRelease(void) CYREENTRANT
{
    uint8 counti;

    /* Disable the channel, even if someone just did! */
    CyDmaChDisable(`$INSTANCE_NAME`_TachOutDMA_dmaHandle);

    for (counti = 0u; counti < `$INSTANCE_NAME`_NUMBER_OF_FANS; counti++)
    {
        CyDmaTdFree(`$INSTANCE_NAME`_fanOutTds[counti].setActualPeriodTD);
    }
}


#if(`$INSTANCE_NAME`_FAN_CTL_MODE == `$INSTANCE_NAME`_FANCTLMODE_HARDWARE)
    /*********************************************************************
    * Function Name: uint8 `$INSTANCE_NAME`_TachInDMA_DmaInitalize
    **********************************************************************
    *
    * Summary:
    *   Allocates and initialises a channel of the DMAC to be used by the
    *   caller.
    *
    * Parameters:
    *  uint8  burstCount
    *  uint8  requestPerBurst
    *  uint16 upperSrcAddress
    *  uint16 upperDestAddress
    *
    * Return:
    *  The channel that can be used by the caller for DMA activity.
    *  DMA_INVALID_CHANNEL (0xFF) if there are no channels left.
    *
    * Side Effects:
    *  None
    *
    *******************************************************************/
    uint8 `$INSTANCE_NAME`_TachInDMA_DmaInitialize(uint8 burstCount, uint8 requestPerBurst, uint16 upperSrcAddress,
                                                        uint16 upperDestAddress) CYREENTRANT
    {
        /* Allocate a DMA channel */
        `$INSTANCE_NAME`_TachInDMA_dmaHandle = `$INSTANCE_NAME`_TachInDMA__DRQ_NUMBER;

        if(`$INSTANCE_NAME`_TachInDMA_dmaHandle != DMA_INVALID_CHANNEL)
        {
            /* Configure the channel */
            CyDmaChSetConfiguration(`$INSTANCE_NAME`_TachInDMA_dmaHandle,
                                    burstCount,
                                    requestPerBurst,
                                    `$INSTANCE_NAME`_TachInDMA__TERMOUT0_SEL,
                                    `$INSTANCE_NAME`_TachInDMA__TERMOUT1_SEL,
                                    `$INSTANCE_NAME`_TachInDMA__TERMIN_SEL);

            /* Set the extended address for the transfers */
            CyDmaChSetExtendedAddress(`$INSTANCE_NAME`_TachInDMA_dmaHandle, upperSrcAddress, upperDestAddress);

            /* Set the priority for this channel */
            CyDmaChPriority(`$INSTANCE_NAME`_TachInDMA_dmaHandle, `$INSTANCE_NAME`_TachInDMA__PRIORITY);
        }

        return(`$INSTANCE_NAME`_TachInDMA_dmaHandle);
    }

    /*********************************************************************
    * Function Name: void `$INSTANCE_NAME`_TachInDMA_DmaRelease
    **********************************************************************
    *
    * Summary:
    *   Frees the channel associated with `$INSTANCE_NAME`_TachInDMA and
    *   also frees the TD descriptors
    *
    * Parameters:
    *   none
    *
    * Return:
    *   none
    *
    * Side Effects:
    *  None
    *
    *******************************************************************/
    void `$INSTANCE_NAME`_TachInDMA_DmaRelease(void) CYREENTRANT
    {
        uint8 counti;

        /* Disable the channel, even if someone just did! */
        CyDmaChDisable(`$INSTANCE_NAME`_TachInDMA_dmaHandle);

        for (counti = 0u; counti < `$INSTANCE_NAME`_NUMBER_OF_FANS; counti++)
        {
            CyDmaTdFree(`$INSTANCE_NAME`_fanInTds[counti].getDesiredPeriodTD);
            CyDmaTdFree(`$INSTANCE_NAME`_fanInTds[counti].getToleranceTD);
        }
    }

#endif /* `$INSTANCE_NAME`_FAN_CTL_MODE == `$INSTANCE_NAME`_FANCTLMODE_HARDWARE */


/*****************************************************************************
 * PWM Data Structure Initialization
 *****************************************************************************/
#if(`$INSTANCE_NAME`_FAN_CTL_MODE == `$INSTANCE_NAME`_FANCTLMODE_HARDWARE)

    /***********************************************************************
    * Hardware Controlled PWM Control Registers (8-bit)
    ***********************************************************************/
    #if(`$INSTANCE_NAME`_PWMRES == `$INSTANCE_NAME`_PWMRES_EIGHTBIT)

        `$INSTANCE_NAME`_fanDriverRegsStruct `$INSTANCE_NAME`_fanDriverRegs[`$INSTANCE_NAME`_NUMBER_OF_FANS] =
        {
            #if (`$INSTANCE_NAME`_NUMBER_OF_FANS >= 1u)
                {
                    `$INSTANCE_NAME`_B_FanCtrl_CLOSED_LOOP_Fan_1_PWM8_ClosedLoopFan8_u0__D1_REG,
                    `$INSTANCE_NAME`_B_FanCtrl_CLOSED_LOOP_Fan_1_PWM8_ClosedLoopFan8_u0__D0_REG,
                    `$INSTANCE_NAME`_B_FanCtrl_CLOSED_LOOP_Fan_1_PWM8_ClosedLoopFan8_u0__A0_REG,
                    `$INSTANCE_NAME`_B_FanCtrl_CLOSED_LOOP_Fan_1_SpeedErrorCounter__CONTROL_AUX_CTL_REG
                },
            #endif /* `$INSTANCE_NAME`_NUMBER_OF_FANS >= 1u */

            #if(`$INSTANCE_NAME`_NUMBER_OF_FANS >= 2u)
                {
                    `$INSTANCE_NAME`_B_FanCtrl_CLOSED_LOOP_FAN2_Fan_2_PWM8_ClosedLoopFan8_u0__D1_REG,
                    `$INSTANCE_NAME`_B_FanCtrl_CLOSED_LOOP_FAN2_Fan_2_PWM8_ClosedLoopFan8_u0__D0_REG,
                    `$INSTANCE_NAME`_B_FanCtrl_CLOSED_LOOP_FAN2_Fan_2_PWM8_ClosedLoopFan8_u0__A0_REG,
                    `$INSTANCE_NAME`_B_FanCtrl_CLOSED_LOOP_FAN2_Fan_2_SpeedErrorCounter__CONTROL_AUX_CTL_REG
                },
            #endif /* `$INSTANCE_NAME`_NUMBER_OF_FANS >= 2u */

            #if(`$INSTANCE_NAME`_NUMBER_OF_FANS >= 3u)
                {
                    `$INSTANCE_NAME`_B_FanCtrl_CLOSED_LOOP_FAN3_Fan_3_PWM8_ClosedLoopFan8_u0__D1_REG,
                    `$INSTANCE_NAME`_B_FanCtrl_CLOSED_LOOP_FAN3_Fan_3_PWM8_ClosedLoopFan8_u0__D0_REG,
                    `$INSTANCE_NAME`_B_FanCtrl_CLOSED_LOOP_FAN3_Fan_3_PWM8_ClosedLoopFan8_u0__A0_REG,
                    `$INSTANCE_NAME`_B_FanCtrl_CLOSED_LOOP_FAN3_Fan_3_SpeedErrorCounter__CONTROL_AUX_CTL_REG
                },
            #endif /* `$INSTANCE_NAME`_NUMBER_OF_FANS >= 3u */

            #if(`$INSTANCE_NAME`_NUMBER_OF_FANS >= 4u)
                {
                    `$INSTANCE_NAME`_B_FanCtrl_CLOSED_LOOP_FAN4_Fan_4_PWM8_ClosedLoopFan8_u0__D1_REG,
                    `$INSTANCE_NAME`_B_FanCtrl_CLOSED_LOOP_FAN4_Fan_4_PWM8_ClosedLoopFan8_u0__D0_REG,
                    `$INSTANCE_NAME`_B_FanCtrl_CLOSED_LOOP_FAN4_Fan_4_PWM8_ClosedLoopFan8_u0__A0_REG,
                    `$INSTANCE_NAME`_B_FanCtrl_CLOSED_LOOP_FAN4_Fan_4_SpeedErrorCounter__CONTROL_AUX_CTL_REG
                },
            #endif /* `$INSTANCE_NAME`_NUMBER_OF_FANS >= 4u */

            #if(`$INSTANCE_NAME`_NUMBER_OF_FANS >= 5u)
                {
                    `$INSTANCE_NAME`_B_FanCtrl_CLOSED_LOOP_FAN5_Fan_5_PWM8_ClosedLoopFan8_u0__D1_REG,
                    `$INSTANCE_NAME`_B_FanCtrl_CLOSED_LOOP_FAN5_Fan_5_PWM8_ClosedLoopFan8_u0__D0_REG,
                    `$INSTANCE_NAME`_B_FanCtrl_CLOSED_LOOP_FAN5_Fan_5_PWM8_ClosedLoopFan8_u0__A0_REG,
                    `$INSTANCE_NAME`_B_FanCtrl_CLOSED_LOOP_FAN5_Fan_5_SpeedErrorCounter__CONTROL_AUX_CTL_REG
                },
            #endif /* `$INSTANCE_NAME`_NUMBER_OF_FANS >= 5u */

            #if(`$INSTANCE_NAME`_NUMBER_OF_FANS >= 6u)
                {
                    `$INSTANCE_NAME`_B_FanCtrl_CLOSED_LOOP_FAN6_Fan_6_PWM8_ClosedLoopFan8_u0__D1_REG,
                    `$INSTANCE_NAME`_B_FanCtrl_CLOSED_LOOP_FAN6_Fan_6_PWM8_ClosedLoopFan8_u0__D0_REG,
                    `$INSTANCE_NAME`_B_FanCtrl_CLOSED_LOOP_FAN6_Fan_6_PWM8_ClosedLoopFan8_u0__A0_REG,
                    `$INSTANCE_NAME`_B_FanCtrl_CLOSED_LOOP_FAN6_Fan_6_SpeedErrorCounter__CONTROL_AUX_CTL_REG
                },
            #endif  /* `$INSTANCE_NAME`_NUMBER_OF_FANS >= 6u */

            #if(`$INSTANCE_NAME`_NUMBER_OF_FANS >= 7u)
                {
                    `$INSTANCE_NAME`_B_FanCtrl_CLOSED_LOOP_FAN7_Fan_7_PWM8_ClosedLoopFan8_u0__D1_REG,
                    `$INSTANCE_NAME`_B_FanCtrl_CLOSED_LOOP_FAN7_Fan_7_PWM8_ClosedLoopFan8_u0__D0_REG,
                    `$INSTANCE_NAME`_B_FanCtrl_CLOSED_LOOP_FAN7_Fan_7_PWM8_ClosedLoopFan8_u0__A0_REG,
                    `$INSTANCE_NAME`_B_FanCtrl_CLOSED_LOOP_FAN7_Fan_7_SpeedErrorCounter__CONTROL_AUX_CTL_REG
                },
            #endif /* `$INSTANCE_NAME`_NUMBER_OF_FANS >= 7u */

            #if(`$INSTANCE_NAME`_NUMBER_OF_FANS >= 8u)
                {
                    `$INSTANCE_NAME`_B_FanCtrl_CLOSED_LOOP_FAN8_Fan_8_PWM8_ClosedLoopFan8_u0__D1_REG,
                    `$INSTANCE_NAME`_B_FanCtrl_CLOSED_LOOP_FAN8_Fan_8_PWM8_ClosedLoopFan8_u0__D0_REG,
                    `$INSTANCE_NAME`_B_FanCtrl_CLOSED_LOOP_FAN8_Fan_8_PWM8_ClosedLoopFan8_u0__A0_REG,
                    `$INSTANCE_NAME`_B_FanCtrl_CLOSED_LOOP_FAN8_Fan_8_SpeedErrorCounter__CONTROL_AUX_CTL_REG
                },
            #endif /* `$INSTANCE_NAME`_NUMBER_OF_FANS >= 8u */

            #if(`$INSTANCE_NAME`_NUMBER_OF_FANS >= 9u)
                {
                    `$INSTANCE_NAME`_B_FanCtrl_CLOSED_LOOP_FAN9_Fan_9_PWM8_ClosedLoopFan8_u0__D1_REG,
                    `$INSTANCE_NAME`_B_FanCtrl_CLOSED_LOOP_FAN9_Fan_9_PWM8_ClosedLoopFan8_u0__D0_REG,
                    `$INSTANCE_NAME`_B_FanCtrl_CLOSED_LOOP_FAN9_Fan_9_PWM8_ClosedLoopFan8_u0__A0_REG,
                    `$INSTANCE_NAME`_B_FanCtrl_CLOSED_LOOP_FAN9_Fan_9_SpeedErrorCounter__CONTROL_AUX_CTL_REG
                },
            #endif /* `$INSTANCE_NAME`_NUMBER_OF_FANS >= 9u */

            #if(`$INSTANCE_NAME`_NUMBER_OF_FANS >= 10u)
                {
                    `$INSTANCE_NAME`_B_FanCtrl_CLOSED_LOOP_FAN10_Fan_10_PWM8_ClosedLoopFan8_u0__D1_REG,
                    `$INSTANCE_NAME`_B_FanCtrl_CLOSED_LOOP_FAN10_Fan_10_PWM8_ClosedLoopFan8_u0__D0_REG,
                    `$INSTANCE_NAME`_B_FanCtrl_CLOSED_LOOP_FAN10_Fan_10_PWM8_ClosedLoopFan8_u0__A0_REG,
                    `$INSTANCE_NAME`_B_FanCtrl_CLOSED_LOOP_FAN10_Fan_10_SpeedErrorCounter__CONTROL_AUX_CTL_REG
                },
            #endif /* `$INSTANCE_NAME`_NUMBER_OF_FANS >= 10u */

            #if(`$INSTANCE_NAME`_NUMBER_OF_FANS >= 11u)
                {
                    `$INSTANCE_NAME`_B_FanCtrl_CLOSED_LOOP_FAN11_Fan_11_PWM8_ClosedLoopFan8_u0__D1_REG,
                    `$INSTANCE_NAME`_B_FanCtrl_CLOSED_LOOP_FAN11_Fan_11_PWM8_ClosedLoopFan8_u0__D0_REG,
                    `$INSTANCE_NAME`_B_FanCtrl_CLOSED_LOOP_FAN11_Fan_11_PWM8_ClosedLoopFan8_u0__A0_REG,
                    `$INSTANCE_NAME`_B_FanCtrl_CLOSED_LOOP_FAN11_Fan_11_SpeedErrorCounter__CONTROL_AUX_CTL_REG
                },
            #endif /* `$INSTANCE_NAME`_NUMBER_OF_FANS >= 11u */

            #if(`$INSTANCE_NAME`_NUMBER_OF_FANS >= 12u)
                {
                    `$INSTANCE_NAME`_B_FanCtrl_CLOSED_LOOP_FAN12_Fan_12_PWM8_ClosedLoopFan8_u0__D1_REG,
                    `$INSTANCE_NAME`_B_FanCtrl_CLOSED_LOOP_FAN12_Fan_12_PWM8_ClosedLoopFan8_u0__D0_REG,
                    `$INSTANCE_NAME`_B_FanCtrl_CLOSED_LOOP_FAN12_Fan_12_PWM8_ClosedLoopFan8_u0__A0_REG,
                    `$INSTANCE_NAME`_B_FanCtrl_CLOSED_LOOP_FAN12_Fan_12_SpeedErrorCounter__CONTROL_AUX_CTL_REG
                },
            #endif /* `$INSTANCE_NAME`_NUMBER_OF_FANS >= 12u */

            #if(`$INSTANCE_NAME`_NUMBER_OF_FANS >= 13u)
                {
                    `$INSTANCE_NAME`_B_FanCtrl_CLOSED_LOOP_FAN13_Fan_13_PWM8_ClosedLoopFan8_u0__D1_REG,
                    `$INSTANCE_NAME`_B_FanCtrl_CLOSED_LOOP_FAN13_Fan_13_PWM8_ClosedLoopFan8_u0__D0_REG,
                    `$INSTANCE_NAME`_B_FanCtrl_CLOSED_LOOP_FAN13_Fan_13_PWM8_ClosedLoopFan8_u0__A0_REG,
                    `$INSTANCE_NAME`_B_FanCtrl_CLOSED_LOOP_FAN13_Fan_13_SpeedErrorCounter__CONTROL_AUX_CTL_REG
                },
            #endif /* `$INSTANCE_NAME`_NUMBER_OF_FANS >= 13u */

            #if(`$INSTANCE_NAME`_NUMBER_OF_FANS >= 14u)
                {
                    `$INSTANCE_NAME`_B_FanCtrl_CLOSED_LOOP_FAN14_Fan_14_PWM8_ClosedLoopFan8_u0__D1_REG,
                    `$INSTANCE_NAME`_B_FanCtrl_CLOSED_LOOP_FAN14_Fan_14_PWM8_ClosedLoopFan8_u0__D0_REG,
                    `$INSTANCE_NAME`_B_FanCtrl_CLOSED_LOOP_FAN14_Fan_14_PWM8_ClosedLoopFan8_u0__A0_REG,
                    `$INSTANCE_NAME`_B_FanCtrl_CLOSED_LOOP_FAN14_Fan_14_SpeedErrorCounter__CONTROL_AUX_CTL_REG
                },
            #endif /* `$INSTANCE_NAME`_NUMBER_OF_FANS >= 14u */

            #if(`$INSTANCE_NAME`_NUMBER_OF_FANS >= 15u)
                {
                    `$INSTANCE_NAME`_B_FanCtrl_CLOSED_LOOP_FAN15_Fan_15_PWM8_ClosedLoopFan8_u0__D1_REG,
                    `$INSTANCE_NAME`_B_FanCtrl_CLOSED_LOOP_FAN15_Fan_15_PWM8_ClosedLoopFan8_u0__D0_REG,
                    `$INSTANCE_NAME`_B_FanCtrl_CLOSED_LOOP_FAN15_Fan_15_PWM8_ClosedLoopFan8_u0__A0_REG,
                    `$INSTANCE_NAME`_B_FanCtrl_CLOSED_LOOP_FAN15_Fan_15_SpeedErrorCounter__CONTROL_AUX_CTL_REG
                },
            #endif /* `$INSTANCE_NAME`_NUMBER_OF_FANS >= 15u */

            #if(`$INSTANCE_NAME`_NUMBER_OF_FANS >= 16u)
                {
                    `$INSTANCE_NAME`_B_FanCtrl_CLOSED_LOOP_FAN16_Fan_16_PWM8_ClosedLoopFan8_u0__D1_REG,
                    `$INSTANCE_NAME`_B_FanCtrl_CLOSED_LOOP_FAN16_Fan_16_PWM8_ClosedLoopFan8_u0__D0_REG,
                    `$INSTANCE_NAME`_B_FanCtrl_CLOSED_LOOP_FAN16_Fan_16_PWM8_ClosedLoopFan8_u0__A0_REG,
                    `$INSTANCE_NAME`_B_FanCtrl_CLOSED_LOOP_FAN16_Fan_16_SpeedErrorCounter__CONTROL_AUX_CTL_REG
                }
            #endif /* `$INSTANCE_NAME`_NUMBER_OF_FANS >= 16u */
        };

    /***********************************************************************
    * Hardware Controlled PWM Control Registers (10-bit)
    ***********************************************************************/
    #else
        `$INSTANCE_NAME`_fanDriverRegsStruct `$INSTANCE_NAME`_fanDriverRegs[`$INSTANCE_NAME`_NUMBER_OF_FANS] =
        {
            #if(`$INSTANCE_NAME`_NUMBER_OF_FANS >= 1u)
                {
                    `$INSTANCE_NAME`_B_FanCtrl_CLOSED_LOOP_Fan_1_PWM10_ClosedLoopFan10_u0__D1_REG,
                    `$INSTANCE_NAME`_B_FanCtrl_CLOSED_LOOP_Fan_1_PWM10_ClosedLoopFan10_u0__D0_REG,
                    `$INSTANCE_NAME`_B_FanCtrl_CLOSED_LOOP_Fan_1_PWM10_ClosedLoopFan10_u0__A0_REG,
                    `$INSTANCE_NAME`_B_FanCtrl_CLOSED_LOOP_Fan_1_SpeedErrorCounter__CONTROL_AUX_CTL_REG
                },
            #endif /* `$INSTANCE_NAME`_NUMBER_OF_FANS >= 1u */

            #if(`$INSTANCE_NAME`_NUMBER_OF_FANS >= 2u)
                {
                    `$INSTANCE_NAME`_B_FanCtrl_CLOSED_LOOP_FAN2_Fan_2_PWM10_ClosedLoopFan10_u0__D1_REG,
                    `$INSTANCE_NAME`_B_FanCtrl_CLOSED_LOOP_FAN2_Fan_2_PWM10_ClosedLoopFan10_u0__D0_REG,
                    `$INSTANCE_NAME`_B_FanCtrl_CLOSED_LOOP_FAN2_Fan_2_PWM10_ClosedLoopFan10_u0__A0_REG,
                    `$INSTANCE_NAME`_B_FanCtrl_CLOSED_LOOP_FAN2_Fan_2_SpeedErrorCounter__CONTROL_AUX_CTL_REG
                },
            #endif /* `$INSTANCE_NAME`_NUMBER_OF_FANS >= 2u */

            #if(`$INSTANCE_NAME`_NUMBER_OF_FANS >= 3u)
                {
                    `$INSTANCE_NAME`_B_FanCtrl_CLOSED_LOOP_FAN3_Fan_3_PWM10_ClosedLoopFan10_u0__D1_REG,
                    `$INSTANCE_NAME`_B_FanCtrl_CLOSED_LOOP_FAN3_Fan_3_PWM10_ClosedLoopFan10_u0__D0_REG,
                    `$INSTANCE_NAME`_B_FanCtrl_CLOSED_LOOP_FAN3_Fan_3_PWM10_ClosedLoopFan10_u0__A0_REG,
                    `$INSTANCE_NAME`_B_FanCtrl_CLOSED_LOOP_FAN3_Fan_3_SpeedErrorCounter__CONTROL_AUX_CTL_REG
                },
            #endif /* `$INSTANCE_NAME`_NUMBER_OF_FANS >= 3u */

            #if(`$INSTANCE_NAME`_NUMBER_OF_FANS >= 4u)
                {
                    `$INSTANCE_NAME`_B_FanCtrl_CLOSED_LOOP_FAN4_Fan_4_PWM10_ClosedLoopFan10_u0__D1_REG,
                    `$INSTANCE_NAME`_B_FanCtrl_CLOSED_LOOP_FAN4_Fan_4_PWM10_ClosedLoopFan10_u0__D0_REG,
                    `$INSTANCE_NAME`_B_FanCtrl_CLOSED_LOOP_FAN4_Fan_4_PWM10_ClosedLoopFan10_u0__A0_REG,
                    `$INSTANCE_NAME`_B_FanCtrl_CLOSED_LOOP_FAN4_Fan_4_SpeedErrorCounter__CONTROL_AUX_CTL_REG
                },
            #endif /* `$INSTANCE_NAME`_NUMBER_OF_FANS >= 4u */

            #if(`$INSTANCE_NAME`_NUMBER_OF_FANS >= 5u)
                {
                    `$INSTANCE_NAME`_B_FanCtrl_CLOSED_LOOP_FAN5_Fan_5_PWM10_ClosedLoopFan10_u0__D1_REG,
                    `$INSTANCE_NAME`_B_FanCtrl_CLOSED_LOOP_FAN5_Fan_5_PWM10_ClosedLoopFan10_u0__D0_REG,
                    `$INSTANCE_NAME`_B_FanCtrl_CLOSED_LOOP_FAN5_Fan_5_PWM10_ClosedLoopFan10_u0__A0_REG,
                    `$INSTANCE_NAME`_B_FanCtrl_CLOSED_LOOP_FAN5_Fan_5_SpeedErrorCounter__CONTROL_AUX_CTL_REG
                },
            #endif /* `$INSTANCE_NAME`_NUMBER_OF_FANS >= 5u */

            #if(`$INSTANCE_NAME`_NUMBER_OF_FANS >= 6u)
                {
                    `$INSTANCE_NAME`_B_FanCtrl_CLOSED_LOOP_FAN6_Fan_6_PWM10_ClosedLoopFan10_u0__D1_REG,
                    `$INSTANCE_NAME`_B_FanCtrl_CLOSED_LOOP_FAN6_Fan_6_PWM10_ClosedLoopFan10_u0__D0_REG,
                    `$INSTANCE_NAME`_B_FanCtrl_CLOSED_LOOP_FAN6_Fan_6_PWM10_ClosedLoopFan10_u0__A0_REG,
                    `$INSTANCE_NAME`_B_FanCtrl_CLOSED_LOOP_FAN6_Fan_6_SpeedErrorCounter__CONTROL_AUX_CTL_REG
                },
            #endif /* `$INSTANCE_NAME`_NUMBER_OF_FANS >= 6u */

            #if(`$INSTANCE_NAME`_NUMBER_OF_FANS >= 7u)
                {
                    `$INSTANCE_NAME`_B_FanCtrl_CLOSED_LOOP_FAN7_Fan_7_PWM10_ClosedLoopFan10_u0__D1_REG,
                    `$INSTANCE_NAME`_B_FanCtrl_CLOSED_LOOP_FAN7_Fan_7_PWM10_ClosedLoopFan10_u0__D0_REG,
                    `$INSTANCE_NAME`_B_FanCtrl_CLOSED_LOOP_FAN7_Fan_7_PWM10_ClosedLoopFan10_u0__A0_REG,
                    `$INSTANCE_NAME`_B_FanCtrl_CLOSED_LOOP_FAN7_Fan_7_SpeedErrorCounter__CONTROL_AUX_CTL_REG
                },
            #endif /* `$INSTANCE_NAME`_NUMBER_OF_FANS >= 7u */

            #if(`$INSTANCE_NAME`_NUMBER_OF_FANS >= 8u)
                {
                    `$INSTANCE_NAME`_B_FanCtrl_CLOSED_LOOP_FAN8_Fan_8_PWM10_ClosedLoopFan10_u0__D1_REG,
                    `$INSTANCE_NAME`_B_FanCtrl_CLOSED_LOOP_FAN8_Fan_8_PWM10_ClosedLoopFan10_u0__D0_REG,
                    `$INSTANCE_NAME`_B_FanCtrl_CLOSED_LOOP_FAN8_Fan_8_PWM10_ClosedLoopFan10_u0__A0_REG,
                    `$INSTANCE_NAME`_B_FanCtrl_CLOSED_LOOP_FAN8_Fan_8_SpeedErrorCounter__CONTROL_AUX_CTL_REG
                },
            #endif /* `$INSTANCE_NAME`_NUMBER_OF_FANS >= 8u */

            #if(`$INSTANCE_NAME`_NUMBER_OF_FANS >= 9u)
                {
                    `$INSTANCE_NAME`_B_FanCtrl_CLOSED_LOOP_FAN9_Fan_9_PWM10_ClosedLoopFan10_u0__D1_REG,
                    `$INSTANCE_NAME`_B_FanCtrl_CLOSED_LOOP_FAN9_Fan_9_PWM10_ClosedLoopFan10_u0__D0_REG,
                    `$INSTANCE_NAME`_B_FanCtrl_CLOSED_LOOP_FAN9_Fan_9_PWM10_ClosedLoopFan10_u0__A0_REG,
                    `$INSTANCE_NAME`_B_FanCtrl_CLOSED_LOOP_FAN9_Fan_9_SpeedErrorCounter__CONTROL_AUX_CTL_REG
                },
            #endif /* `$INSTANCE_NAME`_NUMBER_OF_FANS >= 9u */

            #if(`$INSTANCE_NAME`_NUMBER_OF_FANS >= 10u)
                {
                    `$INSTANCE_NAME`_B_FanCtrl_CLOSED_LOOP_FAN10_Fan_10_PWM10_ClosedLoopFan10_u0__D1_REG,
                    `$INSTANCE_NAME`_B_FanCtrl_CLOSED_LOOP_FAN10_Fan_10_PWM10_ClosedLoopFan10_u0__D0_REG,
                    `$INSTANCE_NAME`_B_FanCtrl_CLOSED_LOOP_FAN10_Fan_10_PWM10_ClosedLoopFan10_u0__A0_REG,
                    `$INSTANCE_NAME`_B_FanCtrl_CLOSED_LOOP_FAN10_Fan_10_SpeedErrorCounter__CONTROL_AUX_CTL_REG
                },
            #endif /* `$INSTANCE_NAME`_NUMBER_OF_FANS >= 10u */

            #if(`$INSTANCE_NAME`_NUMBER_OF_FANS >= 11u)
                {
                    `$INSTANCE_NAME`_B_FanCtrl_CLOSED_LOOP_FAN11_Fan_11_PWM10_ClosedLoopFan10_u0__D1_REG,
                    `$INSTANCE_NAME`_B_FanCtrl_CLOSED_LOOP_FAN11_Fan_11_PWM10_ClosedLoopFan10_u0__D0_REG,
                    `$INSTANCE_NAME`_B_FanCtrl_CLOSED_LOOP_FAN11_Fan_11_PWM10_ClosedLoopFan10_u0__A0_REG,
                    `$INSTANCE_NAME`_B_FanCtrl_CLOSED_LOOP_FAN11_Fan_11_SpeedErrorCounter__CONTROL_AUX_CTL_REG
                },
            #endif /* `$INSTANCE_NAME`_NUMBER_OF_FANS >= 11u */

            #if(`$INSTANCE_NAME`_NUMBER_OF_FANS >= 12u)
                {
                    `$INSTANCE_NAME`_B_FanCtrl_CLOSED_LOOP_FAN12_Fan_12_PWM10_ClosedLoopFan10_u0__D1_REG,
                    `$INSTANCE_NAME`_B_FanCtrl_CLOSED_LOOP_FAN12_Fan_12_PWM10_ClosedLoopFan10_u0__D0_REG,
                    `$INSTANCE_NAME`_B_FanCtrl_CLOSED_LOOP_FAN12_Fan_12_PWM10_ClosedLoopFan10_u0__A0_REG,
                    `$INSTANCE_NAME`_B_FanCtrl_CLOSED_LOOP_FAN12_Fan_12_SpeedErrorCounter__CONTROL_AUX_CTL_REG
                },
            #endif /* `$INSTANCE_NAME`_NUMBER_OF_FANS >= 12u */

            #if(`$INSTANCE_NAME`_NUMBER_OF_FANS >= 13u)
                {
                    `$INSTANCE_NAME`_B_FanCtrl_CLOSED_LOOP_FAN13_Fan_13_PWM10_ClosedLoopFan10_u0__D1_REG,
                    `$INSTANCE_NAME`_B_FanCtrl_CLOSED_LOOP_FAN13_Fan_13_PWM10_ClosedLoopFan10_u0__D0_REG,
                    `$INSTANCE_NAME`_B_FanCtrl_CLOSED_LOOP_FAN13_Fan_13_PWM10_ClosedLoopFan10_u0__A0_REG,
                    `$INSTANCE_NAME`_B_FanCtrl_CLOSED_LOOP_FAN13_Fan_13_SpeedErrorCounter__CONTROL_AUX_CTL_REG
                },
            #endif /* `$INSTANCE_NAME`_NUMBER_OF_FANS >= 13u */

            #if(`$INSTANCE_NAME`_NUMBER_OF_FANS >= 14u)
                {
                    `$INSTANCE_NAME`_B_FanCtrl_CLOSED_LOOP_FAN14_Fan_14_PWM10_ClosedLoopFan10_u0__D1_REG,
                    `$INSTANCE_NAME`_B_FanCtrl_CLOSED_LOOP_FAN14_Fan_14_PWM10_ClosedLoopFan10_u0__D0_REG,
                    `$INSTANCE_NAME`_B_FanCtrl_CLOSED_LOOP_FAN14_Fan_14_PWM10_ClosedLoopFan10_u0__A0_REG,
                    `$INSTANCE_NAME`_B_FanCtrl_CLOSED_LOOP_FAN14_Fan_14_SpeedErrorCounter__CONTROL_AUX_CTL_REG
                },
            #endif /* `$INSTANCE_NAME`_NUMBER_OF_FANS >= 14u */

            #if(`$INSTANCE_NAME`_NUMBER_OF_FANS >= 15u)
                {
                    `$INSTANCE_NAME`_B_FanCtrl_CLOSED_LOOP_FAN15_Fan_15_PWM10_ClosedLoopFan10_u0__D1_REG,
                    `$INSTANCE_NAME`_B_FanCtrl_CLOSED_LOOP_FAN15_Fan_15_PWM10_ClosedLoopFan10_u0__D0_REG,
                    `$INSTANCE_NAME`_B_FanCtrl_CLOSED_LOOP_FAN15_Fan_15_PWM10_ClosedLoopFan10_u0__A0_REG,
                    `$INSTANCE_NAME`_B_FanCtrl_CLOSED_LOOP_FAN15_Fan_15_SpeedErrorCounter__CONTROL_AUX_CTL_REG
                },
            #endif /* `$INSTANCE_NAME`_NUMBER_OF_FANS >= 15u */

            #if(`$INSTANCE_NAME`_NUMBER_OF_FANS >= 16u)
                {
                    `$INSTANCE_NAME`_B_FanCtrl_CLOSED_LOOP_FAN16_Fan_16_PWM10_ClosedLoopFan10_u0__D1_REG,
                    `$INSTANCE_NAME`_B_FanCtrl_CLOSED_LOOP_FAN16_Fan_16_PWM10_ClosedLoopFan10_u0__D0_REG,
                    `$INSTANCE_NAME`_B_FanCtrl_CLOSED_LOOP_FAN16_Fan_16_PWM10_ClosedLoopFan10_u0__A0_REG,
                    `$INSTANCE_NAME`_B_FanCtrl_CLOSED_LOOP_FAN16_Fan_16_SpeedErrorCounter__CONTROL_AUX_CTL_REG
                }
            #endif /* `$INSTANCE_NAME`_NUMBER_OF_FANS >= 1u */
        };
    #endif /* Hardware Controlled Fan Mode */

#elif (`$INSTANCE_NAME`_FAN_CTL_MODE == `$INSTANCE_NAME`_FANCTLMODE_FIRMWARE)
    /***********************************************************************
    * Firmware Controlled PWM Control Registers (8-bit)
    ***********************************************************************/
    #if(`$INSTANCE_NAME`_PWMRES == `$INSTANCE_NAME`_PWMRES_EIGHTBIT)
        `$INSTANCE_NAME`_fanDriverRegsStruct `$INSTANCE_NAME`_fanDriverRegs[`$INSTANCE_NAME`_NUMBER_OF_FAN_OUTPUTS] =
        {
            #if(`$INSTANCE_NAME`_NUMBER_OF_FAN_OUTPUTS >= 1u)
                {
                    `$INSTANCE_NAME`_B_FanCtrl_OPEN_LOOP_FanPWM_1_2_PWM8_OpenLoopFan8_u0__F0_REG
                },
            #endif /* `$INSTANCE_NAME`_NUMBER_OF_FAN_OUTPUTS >= 1u */

            #if(`$INSTANCE_NAME`_NUMBER_OF_FAN_OUTPUTS >= 2u)
                {
                    `$INSTANCE_NAME`_B_FanCtrl_OPEN_LOOP_FanPWM_1_2_PWM8_OpenLoopFan8_u0__F1_REG
                },
            #endif /* `$INSTANCE_NAME`_NUMBER_OF_FAN_OUTPUTS >= 2u */

            #if(`$INSTANCE_NAME`_NUMBER_OF_FAN_OUTPUTS >= 3u)
                {
                    `$INSTANCE_NAME`_B_FanCtrl_OPEN_LOOP_FAN34_FanPWM_3_4_PWM8_OpenLoopFan8_u0__F0_REG
                },
            #endif /* `$INSTANCE_NAME`_NUMBER_OF_FAN_OUTPUTS >= 3u */

            #if(`$INSTANCE_NAME`_NUMBER_OF_FAN_OUTPUTS >= 4u)
                {
                    `$INSTANCE_NAME`_B_FanCtrl_OPEN_LOOP_FAN34_FanPWM_3_4_PWM8_OpenLoopFan8_u0__F1_REG
                },
            #endif /* `$INSTANCE_NAME`_NUMBER_OF_FAN_OUTPUTS >= 4u */

            #if(`$INSTANCE_NAME`_NUMBER_OF_FAN_OUTPUTS >= 5u)
                {
                    `$INSTANCE_NAME`_B_FanCtrl_OPEN_LOOP_FAN56_FanPWM_5_6_PWM8_OpenLoopFan8_u0__F0_REG
                },
            #endif /* `$INSTANCE_NAME`_NUMBER_OF_FAN_OUTPUTS >= 5u */

            #if(`$INSTANCE_NAME`_NUMBER_OF_FAN_OUTPUTS >= 6u)
                {
                    `$INSTANCE_NAME`_B_FanCtrl_OPEN_LOOP_FAN56_FanPWM_5_6_PWM8_OpenLoopFan8_u0__F1_REG
                },
            #endif /* `$INSTANCE_NAME`_NUMBER_OF_FAN_OUTPUTS >= 6u */

            #if(`$INSTANCE_NAME`_NUMBER_OF_FAN_OUTPUTS >= 7u)
                {
                    `$INSTANCE_NAME`_B_FanCtrl_OPEN_LOOP_FAN78_FanPWM_7_8_PWM8_OpenLoopFan8_u0__F0_REG
                },
            #endif /* `$INSTANCE_NAME`_NUMBER_OF_FAN_OUTPUTS >= 7u */

            #if(`$INSTANCE_NAME`_NUMBER_OF_FAN_OUTPUTS >= 8u)
                {
                    `$INSTANCE_NAME`_B_FanCtrl_OPEN_LOOP_FAN78_FanPWM_7_8_PWM8_OpenLoopFan8_u0__F1_REG
                },
            #endif /* `$INSTANCE_NAME`_NUMBER_OF_FAN_OUTPUTS >= 8u */

            #if(`$INSTANCE_NAME`_NUMBER_OF_FAN_OUTPUTS >= 9u)
                {
                    `$INSTANCE_NAME`_B_FanCtrl_OPEN_LOOP_FAN910_FanPWM_9_10_PWM8_OpenLoopFan8_u0__F0_REG
                },
            #endif /* `$INSTANCE_NAME`_NUMBER_OF_FAN_OUTPUTS >= 9u */

            #if(`$INSTANCE_NAME`_NUMBER_OF_FAN_OUTPUTS >= 10u)
                {
                    `$INSTANCE_NAME`_B_FanCtrl_OPEN_LOOP_FAN910_FanPWM_9_10_PWM8_OpenLoopFan8_u0__F1_REG
                },
            #endif /* `$INSTANCE_NAME`_NUMBER_OF_FAN_OUTPUTS >= 10u */

            #if(`$INSTANCE_NAME`_NUMBER_OF_FAN_OUTPUTS >= 11u)
                {
                    `$INSTANCE_NAME`_B_FanCtrl_OPEN_LOOP_FAN1112_FanPWM_11_12_PWM8_OpenLoopFan8_u0__F0_REG
                },
            #endif /* `$INSTANCE_NAME`_NUMBER_OF_FAN_OUTPUTS >= 11u */

            #if(`$INSTANCE_NAME`_NUMBER_OF_FAN_OUTPUTS >= 12u)
                {
                    `$INSTANCE_NAME`_B_FanCtrl_OPEN_LOOP_FAN1112_FanPWM_11_12_PWM8_OpenLoopFan8_u0__F1_REG
                },
            #endif /* `$INSTANCE_NAME`_NUMBER_OF_FAN_OUTPUTS >= 12u */

            #if(`$INSTANCE_NAME`_NUMBER_OF_FAN_OUTPUTS >= 13u)
                {
                    `$INSTANCE_NAME`_B_FanCtrl_OPEN_LOOP_FAN1314_FanPWM_13_14_PWM8_OpenLoopFan8_u0__F0_REG
                },
            #endif /* `$INSTANCE_NAME`_NUMBER_OF_FAN_OUTPUTS >= 13u */

            #if(`$INSTANCE_NAME`_NUMBER_OF_FAN_OUTPUTS >= 14u)
                {
                    `$INSTANCE_NAME`_B_FanCtrl_OPEN_LOOP_FAN1314_FanPWM_13_14_PWM8_OpenLoopFan8_u0__F1_REG
                },
            #endif /* `$INSTANCE_NAME`_NUMBER_OF_FAN_OUTPUTS >= 14u */

            #if(`$INSTANCE_NAME`_NUMBER_OF_FAN_OUTPUTS >= 15u)
                {
                    `$INSTANCE_NAME`_B_FanCtrl_OPEN_LOOP_FAN1516_FanPWM_15_16_PWM8_OpenLoopFan8_u0__F0_REG
                },
            #endif /* `$INSTANCE_NAME`_NUMBER_OF_FAN_OUTPUTS >= 15u */

            #if(`$INSTANCE_NAME`_NUMBER_OF_FAN_OUTPUTS >= 16u)
                {
                    `$INSTANCE_NAME`_B_FanCtrl_OPEN_LOOP_FAN1516_FanPWM_15_16_PWM8_OpenLoopFan8_u0__F1_REG
                }
            #endif /* `$INSTANCE_NAME`_NUMBER_OF_FAN_OUTPUTS >= 16u */
        };

        `$INSTANCE_NAME`_fanPwmInitRegsStruct `$INSTANCE_NAME`_fanPwmInitRegs[`$INSTANCE_NAME`_FANPWM_COUNT] =
        {
            #if `$INSTANCE_NAME`_NUMBER_OF_FAN_OUTPUTS >= 1u
                {
                    `$INSTANCE_NAME`_B_FanCtrl_OPEN_LOOP_FanPWM_1_2_PWM8_OpenLoopFan8_u0__A1_REG,
                    `$INSTANCE_NAME`_B_FanCtrl_OPEN_LOOP_FanPWM_1_2_PWM8_OpenLoopFan8_u0__DP_AUX_CTL_REG
                },
            #endif /* `$INSTANCE_NAME`_NUMBER_OF_FAN_OUTPUTS >= 1u */

            #if(`$INSTANCE_NAME`_NUMBER_OF_FAN_OUTPUTS >= 3u)
                {
                    `$INSTANCE_NAME`_B_FanCtrl_OPEN_LOOP_FAN34_FanPWM_3_4_PWM8_OpenLoopFan8_u0__A1_REG,
                    `$INSTANCE_NAME`_B_FanCtrl_OPEN_LOOP_FAN34_FanPWM_3_4_PWM8_OpenLoopFan8_u0__DP_AUX_CTL_REG
                },
            #endif /* `$INSTANCE_NAME`_NUMBER_OF_FAN_OUTPUTS >= 3u */

            #if(`$INSTANCE_NAME`_NUMBER_OF_FAN_OUTPUTS >= 5u)
                {
                    `$INSTANCE_NAME`_B_FanCtrl_OPEN_LOOP_FAN56_FanPWM_5_6_PWM8_OpenLoopFan8_u0__A1_REG,
                    `$INSTANCE_NAME`_B_FanCtrl_OPEN_LOOP_FAN56_FanPWM_5_6_PWM8_OpenLoopFan8_u0__DP_AUX_CTL_REG
                },
            #endif /* `$INSTANCE_NAME`_NUMBER_OF_FAN_OUTPUTS >= 5u */

            #if(`$INSTANCE_NAME`_NUMBER_OF_FAN_OUTPUTS >= 7u)
                {
                    `$INSTANCE_NAME`_B_FanCtrl_OPEN_LOOP_FAN78_FanPWM_7_8_PWM8_OpenLoopFan8_u0__A1_REG,
                    `$INSTANCE_NAME`_B_FanCtrl_OPEN_LOOP_FAN78_FanPWM_7_8_PWM8_OpenLoopFan8_u0__DP_AUX_CTL_REG
                },
            #endif /* `$INSTANCE_NAME`_NUMBER_OF_FAN_OUTPUTS >= 7 */

            #if(`$INSTANCE_NAME`_NUMBER_OF_FAN_OUTPUTS >= 9u)
                {
                    `$INSTANCE_NAME`_B_FanCtrl_OPEN_LOOP_FAN910_FanPWM_9_10_PWM8_OpenLoopFan8_u0__A1_REG,
                    `$INSTANCE_NAME`_B_FanCtrl_OPEN_LOOP_FAN910_FanPWM_9_10_PWM8_OpenLoopFan8_u0__DP_AUX_CTL_REG
                },
            #endif /* `$INSTANCE_NAME`_NUMBER_OF_FAN_OUTPUTS >= 9u */

            #if(`$INSTANCE_NAME`_NUMBER_OF_FAN_OUTPUTS >= 11u)
                {
                    `$INSTANCE_NAME`_B_FanCtrl_OPEN_LOOP_FAN1112_FanPWM_11_12_PWM8_OpenLoopFan8_u0__A1_REG,
                    `$INSTANCE_NAME`_B_FanCtrl_OPEN_LOOP_FAN1112_FanPWM_11_12_PWM8_OpenLoopFan8_u0__DP_AUX_CTL_REG
                },
            #endif /* `$INSTANCE_NAME`_NUMBER_OF_FAN_OUTPUTS >= 11u */

            #if(`$INSTANCE_NAME`_NUMBER_OF_FAN_OUTPUTS >= 13u)
                {
                    `$INSTANCE_NAME`_B_FanCtrl_OPEN_LOOP_FAN1314_FanPWM_13_14_PWM8_OpenLoopFan8_u0__A1_REG,
                    `$INSTANCE_NAME`_B_FanCtrl_OPEN_LOOP_FAN1314_FanPWM_13_14_PWM8_OpenLoopFan8_u0__DP_AUX_CTL_REG
                },
            #endif /* `$INSTANCE_NAME`_NUMBER_OF_FAN_OUTPUTS >= 13 */

            #if(`$INSTANCE_NAME`_NUMBER_OF_FAN_OUTPUTS >= 15u)
                {
                    `$INSTANCE_NAME`_B_FanCtrl_OPEN_LOOP_FAN1516_FanPWM_15_16_PWM8_OpenLoopFan8_u0__A1_REG,
                    `$INSTANCE_NAME`_B_FanCtrl_OPEN_LOOP_FAN1516_FanPWM_15_16_PWM8_OpenLoopFan8_u0__DP_AUX_CTL_REG
                }
            #endif /* `$INSTANCE_NAME`_NUMBER_OF_FAN_OUTPUTS >= 15u */
        };

    /***********************************************************************
    * Firmware Controlled PWM Control Registers (10-bit)
    ***********************************************************************/
    #else
        `$INSTANCE_NAME`_fanDriverRegsStruct `$INSTANCE_NAME`_fanDriverRegs[`$INSTANCE_NAME`_NUMBER_OF_FAN_OUTPUTS] =
        {
            #if(`$INSTANCE_NAME`_NUMBER_OF_FAN_OUTPUTS >= 1u)
                {
                    `$INSTANCE_NAME`_B_FanCtrl_OPEN_LOOP_FanPWM_1_2_PWM10_OpenLoopFan10_u0__F0_REG
                },
            #endif /* `$INSTANCE_NAME`_NUMBER_OF_FAN_OUTPUTS >= 1u */

            #if(`$INSTANCE_NAME`_NUMBER_OF_FAN_OUTPUTS >= 2u)
                {
                    `$INSTANCE_NAME`_B_FanCtrl_OPEN_LOOP_FanPWM_1_2_PWM10_OpenLoopFan10_u0__F1_REG
                },
            #endif /* `$INSTANCE_NAME`_NUMBER_OF_FAN_OUTPUTS >= 2 */

            #if(`$INSTANCE_NAME`_NUMBER_OF_FAN_OUTPUTS >= 3u)
                {
                    `$INSTANCE_NAME`_B_FanCtrl_OPEN_LOOP_FAN34_FanPWM_3_4_PWM10_OpenLoopFan10_u0__F0_REG
                },
            #endif /* `$INSTANCE_NAME`_NUMBER_OF_FAN_OUTPUTS >= 3u */

            #if(`$INSTANCE_NAME`_NUMBER_OF_FAN_OUTPUTS >= 4u)
                {
                    `$INSTANCE_NAME`_B_FanCtrl_OPEN_LOOP_FAN34_FanPWM_3_4_PWM10_OpenLoopFan10_u0__F1_REG
                },
            #endif /* `$INSTANCE_NAME`_NUMBER_OF_FAN_OUTPUTS >= 4u */

            #if(`$INSTANCE_NAME`_NUMBER_OF_FAN_OUTPUTS >= 5u)
                {
                    `$INSTANCE_NAME`_B_FanCtrl_OPEN_LOOP_FAN56_FanPWM_5_6_PWM10_OpenLoopFan10_u0__F0_REG
                },
            #endif /* `$INSTANCE_NAME`_NUMBER_OF_FAN_OUTPUTS >= 5u */

            #if(`$INSTANCE_NAME`_NUMBER_OF_FAN_OUTPUTS >= 6u)
                {
                    `$INSTANCE_NAME`_B_FanCtrl_OPEN_LOOP_FAN56_FanPWM_5_6_PWM10_OpenLoopFan10_u0__F1_REG
                },
            #endif /* `$INSTANCE_NAME`_NUMBER_OF_FAN_OUTPUTS >= 6u */

            #if(`$INSTANCE_NAME`_NUMBER_OF_FAN_OUTPUTS >= 7u)
                {
                    `$INSTANCE_NAME`_B_FanCtrl_OPEN_LOOP_FAN78_FanPWM_7_8_PWM10_OpenLoopFan10_u0__F0_REG
                },
            #endif /* `$INSTANCE_NAME`_NUMBER_OF_FAN_OUTPUTS >= 7u */

            #if(`$INSTANCE_NAME`_NUMBER_OF_FAN_OUTPUTS >= 8u)
                {
                    `$INSTANCE_NAME`_B_FanCtrl_OPEN_LOOP_FAN78_FanPWM_7_8_PWM10_OpenLoopFan10_u0__F1_REG
                },
            #endif /* `$INSTANCE_NAME`_NUMBER_OF_FAN_OUTPUTS >= 8u */

            #if(`$INSTANCE_NAME`_NUMBER_OF_FAN_OUTPUTS >= 9u)
                {
                    `$INSTANCE_NAME`_B_FanCtrl_OPEN_LOOP_FAN910_FanPWM_9_10_PWM10_OpenLoopFan10_u0__F0_REG
                },
            #endif /* `$INSTANCE_NAME`_NUMBER_OF_FAN_OUTPUTS >= 9u */

            #if(`$INSTANCE_NAME`_NUMBER_OF_FAN_OUTPUTS >= 10u)
                {
                    `$INSTANCE_NAME`_B_FanCtrl_OPEN_LOOP_FAN910_FanPWM_9_10_PWM10_OpenLoopFan10_u0__F1_REG
                },
            #endif /* `$INSTANCE_NAME`_NUMBER_OF_FAN_OUTPUTS >= 10u */

            #if(`$INSTANCE_NAME`_NUMBER_OF_FAN_OUTPUTS >= 11u)
                {
                    `$INSTANCE_NAME`_B_FanCtrl_OPEN_LOOP_FAN1112_FanPWM_11_12_PWM10_OpenLoopFan10_u0__F0_REG
                },
            #endif /* `$INSTANCE_NAME`_NUMBER_OF_FAN_OUTPUTS >= 11u */

            #if(`$INSTANCE_NAME`_NUMBER_OF_FAN_OUTPUTS >= 12u)
                {
                    `$INSTANCE_NAME`_B_FanCtrl_OPEN_LOOP_FAN1112_FanPWM_11_12_PWM10_OpenLoopFan10_u0__F1_REG
                },
            #endif /* `$INSTANCE_NAME`_NUMBER_OF_FAN_OUTPUTS >= 12u */

            #if(`$INSTANCE_NAME`_NUMBER_OF_FAN_OUTPUTS >= 13u)
                {
                    `$INSTANCE_NAME`_B_FanCtrl_OPEN_LOOP_FAN1314_FanPWM_13_14_PWM10_OpenLoopFan10_u0__F0_REG
                },
            #endif /* `$INSTANCE_NAME`_NUMBER_OF_FAN_OUTPUTS >= 13u */

            #if(`$INSTANCE_NAME`_NUMBER_OF_FAN_OUTPUTS >= 14u)
                {
                    `$INSTANCE_NAME`_B_FanCtrl_OPEN_LOOP_FAN1314_FanPWM_13_14_PWM10_OpenLoopFan10_u0__F1_REG
                },
            #endif /* `$INSTANCE_NAME`_NUMBER_OF_FAN_OUTPUTS >= 14u */

            #if(`$INSTANCE_NAME`_NUMBER_OF_FAN_OUTPUTS >= 15u)
                {
                    `$INSTANCE_NAME`_B_FanCtrl_OPEN_LOOP_FAN1516_FanPWM_15_16_PWM10_OpenLoopFan10_u0__F0_REG
                },
            #endif /* `$INSTANCE_NAME`_NUMBER_OF_FAN_OUTPUTS >= 15u */

            #if(`$INSTANCE_NAME`_NUMBER_OF_FAN_OUTPUTS >= 16u)
                {
                    `$INSTANCE_NAME`_B_FanCtrl_OPEN_LOOP_FAN1516_FanPWM_15_16_PWM10_OpenLoopFan10_u0__F1_REG
                }
            #endif /* `$INSTANCE_NAME`_NUMBER_OF_FAN_OUTPUTS >= 16u */
        };

        `$INSTANCE_NAME`_fanPwmInitRegsStruct `$INSTANCE_NAME`_fanPwmInitRegs[`$INSTANCE_NAME`_FANPWM_COUNT] =
        {
            #if(`$INSTANCE_NAME`_NUMBER_OF_FAN_OUTPUTS >= 1u)
                {
                    `$INSTANCE_NAME`_B_FanCtrl_OPEN_LOOP_FanPWM_1_2_PWM10_OpenLoopFan10_u0__A1_REG,
                    `$INSTANCE_NAME`_B_FanCtrl_OPEN_LOOP_FanPWM_1_2_PWM10_OpenLoopFan10_u0__DP_AUX_CTL_REG
                },
            #endif /* `$INSTANCE_NAME`_NUMBER_OF_FAN_OUTPUTS >= 1u */

            #if(`$INSTANCE_NAME`_NUMBER_OF_FAN_OUTPUTS >= 3u)
                {
                    `$INSTANCE_NAME`_B_FanCtrl_OPEN_LOOP_FAN34_FanPWM_3_4_PWM10_OpenLoopFan10_u0__A1_REG,
                    `$INSTANCE_NAME`_B_FanCtrl_OPEN_LOOP_FAN34_FanPWM_3_4_PWM10_OpenLoopFan10_u0__DP_AUX_CTL_REG
                },
            #endif /* `$INSTANCE_NAME`_NUMBER_OF_FAN_OUTPUTS >= 3u */

            #if(`$INSTANCE_NAME`_NUMBER_OF_FAN_OUTPUTS >= 5u)
                {
                    `$INSTANCE_NAME`_B_FanCtrl_OPEN_LOOP_FAN56_FanPWM_5_6_PWM10_OpenLoopFan10_u0__A1_REG,
                    `$INSTANCE_NAME`_B_FanCtrl_OPEN_LOOP_FAN56_FanPWM_5_6_PWM10_OpenLoopFan10_u0__DP_AUX_CTL_REG
                },
            #endif /* `$INSTANCE_NAME`_NUMBER_OF_FAN_OUTPUTS >= 5u */

            #if(`$INSTANCE_NAME`_NUMBER_OF_FAN_OUTPUTS >= 7u)
                {
                    `$INSTANCE_NAME`_B_FanCtrl_OPEN_LOOP_FAN78_FanPWM_7_8_PWM10_OpenLoopFan10_u0__A1_REG,
                    `$INSTANCE_NAME`_B_FanCtrl_OPEN_LOOP_FAN78_FanPWM_7_8_PWM10_OpenLoopFan10_u0__DP_AUX_CTL_REG
                },
            #endif /* `$INSTANCE_NAME`_NUMBER_OF_FAN_OUTPUTS >= 7u */

            #if(`$INSTANCE_NAME`_NUMBER_OF_FAN_OUTPUTS >= 9u)
                {
                    `$INSTANCE_NAME`_B_FanCtrl_OPEN_LOOP_FAN910_FanPWM_9_10_PWM10_OpenLoopFan10_u0__A1_REG,
                    `$INSTANCE_NAME`_B_FanCtrl_OPEN_LOOP_FAN910_FanPWM_9_10_PWM10_OpenLoopFan10_u0__DP_AUX_CTL_REG
                },
            #endif /* `$INSTANCE_NAME`_NUMBER_OF_FAN_OUTPUTS >= 9u */

            #if(`$INSTANCE_NAME`_NUMBER_OF_FAN_OUTPUTS >= 11u)
                {
                    `$INSTANCE_NAME`_B_FanCtrl_OPEN_LOOP_FAN1112_FanPWM_11_12_PWM10_OpenLoopFan10_u0__A1_REG,
                    `$INSTANCE_NAME`_B_FanCtrl_OPEN_LOOP_FAN1112_FanPWM_11_12_PWM10_OpenLoopFan10_u0__DP_AUX_CTL_REG
                },
            #endif /* `$INSTANCE_NAME`_NUMBER_OF_FAN_OUTPUTS >= 11u */

            #if(`$INSTANCE_NAME`_NUMBER_OF_FAN_OUTPUTS >= 13u)
                {
                    `$INSTANCE_NAME`_B_FanCtrl_OPEN_LOOP_FAN1314_FanPWM_13_14_PWM10_OpenLoopFan10_u0__A1_REG,
                    `$INSTANCE_NAME`_B_FanCtrl_OPEN_LOOP_FAN1314_FanPWM_13_14_PWM10_OpenLoopFan10_u0__DP_AUX_CTL_REG
                },
            #endif /* `$INSTANCE_NAME`_NUMBER_OF_FAN_OUTPUTS >= 13u */

            #if(`$INSTANCE_NAME`_NUMBER_OF_FAN_OUTPUTS >= 15u)
                {
                    `$INSTANCE_NAME`_B_FanCtrl_OPEN_LOOP_FAN1516_FanPWM_15_16_PWM10_OpenLoopFan10_u0__A1_REG,
                    `$INSTANCE_NAME`_B_FanCtrl_OPEN_LOOP_FAN1516_FanPWM_15_16_PWM10_OpenLoopFan10_u0__DP_AUX_CTL_REG
                }
            #endif /* `$INSTANCE_NAME`_NUMBER_OF_FAN_OUTPUTS >= 15u */
        };
    #endif    /* 10-bit Resolution */
#endif /* `$INSTANCE_NAME`_FAN_CTL_MODE == `$INSTANCE_NAME`_FANCTLMODE_HARDWARE */


/*****************************************************************************
 * Hardware Controlled Fan Mode Fan Control Data Structure Initialization
 *****************************************************************************/
`$INSTANCE_NAME`_fanControlStruct `$INSTANCE_NAME`_fanControl
#if(`$INSTANCE_NAME`_FAN_CTL_MODE == `$INSTANCE_NAME`_FANCTLMODE_HARDWARE)
= {
    {
        /* Initialize the Desired Period Field (RPM B scaled up by tolerance factor %) */
        #if(`$INSTANCE_NAME`_NUMBER_OF_FANS >= 1u)
            `$INSTANCE_NAME`_RPM_PERIOD_CONV_FACTOR / (`$MaxRPM01` * (1u + `$INSTANCE_NAME`_TOLERANCE_FACTOR)),
        #endif /* `$INSTANCE_NAME`_NUMBER_OF_FANS >= 1u */

        #if(`$INSTANCE_NAME`_NUMBER_OF_FANS >= 2u)
               `$INSTANCE_NAME`_RPM_PERIOD_CONV_FACTOR / (`$MaxRPM02` * (1u + `$INSTANCE_NAME`_TOLERANCE_FACTOR)),
        #endif /* `$INSTANCE_NAME`_NUMBER_OF_FANS >= 2u */

        #if(`$INSTANCE_NAME`_NUMBER_OF_FANS >= 3u)
            `$INSTANCE_NAME`_RPM_PERIOD_CONV_FACTOR / (`$MaxRPM03` * (1u + `$INSTANCE_NAME`_TOLERANCE_FACTOR)),
        #endif /* `$INSTANCE_NAME`_NUMBER_OF_FANS >= 3u */

        #if(`$INSTANCE_NAME`_NUMBER_OF_FANS >= 4u)
               `$INSTANCE_NAME`_RPM_PERIOD_CONV_FACTOR / (`$MaxRPM04` * (1u + `$INSTANCE_NAME`_TOLERANCE_FACTOR)),
        #endif /* `$INSTANCE_NAME`_NUMBER_OF_FANS >= 4 */

        #if(`$INSTANCE_NAME`_NUMBER_OF_FANS >= 5)
            `$INSTANCE_NAME`_RPM_PERIOD_CONV_FACTOR / (`$MaxRPM05` * (1u + `$INSTANCE_NAME`_TOLERANCE_FACTOR)),
        #endif /* `$INSTANCE_NAME`_NUMBER_OF_FANS >= 5u */

        #if(`$INSTANCE_NAME`_NUMBER_OF_FANS >= 6u)
            `$INSTANCE_NAME`_RPM_PERIOD_CONV_FACTOR / (`$MaxRPM06` * (1u + `$INSTANCE_NAME`_TOLERANCE_FACTOR)),
        #endif /* `$INSTANCE_NAME`_NUMBER_OF_FANS >= 6u */

        #if(`$INSTANCE_NAME`_NUMBER_OF_FANS >= 7u)
            `$INSTANCE_NAME`_RPM_PERIOD_CONV_FACTOR / (`$MaxRPM07` * (1u + `$INSTANCE_NAME`_TOLERANCE_FACTOR)),
        #endif /* `$INSTANCE_NAME`_NUMBER_OF_FANS >= 7u */

        #if(`$INSTANCE_NAME`_NUMBER_OF_FANS >= 8u)
            `$INSTANCE_NAME`_RPM_PERIOD_CONV_FACTOR / (`$MaxRPM08` * (1u + `$INSTANCE_NAME`_TOLERANCE_FACTOR)),
        #endif /* `$INSTANCE_NAME`_NUMBER_OF_FANS >= 8u */

        #if(`$INSTANCE_NAME`_NUMBER_OF_FANS >= 9u)
            `$INSTANCE_NAME`_RPM_PERIOD_CONV_FACTOR / (`$MaxRPM09` * (1u + `$INSTANCE_NAME`_TOLERANCE_FACTOR)),
        #endif /* `$INSTANCE_NAME`_NUMBER_OF_FANS >= 9u */

        #if(`$INSTANCE_NAME`_NUMBER_OF_FANS >= 10u)
            `$INSTANCE_NAME`_RPM_PERIOD_CONV_FACTOR / (`$MaxRPM10` * (1u + `$INSTANCE_NAME`_TOLERANCE_FACTOR)),
        #endif /* `$INSTANCE_NAME`_NUMBER_OF_FANS >= 10u */

        #if(`$INSTANCE_NAME`_NUMBER_OF_FANS >= 11u)
            `$INSTANCE_NAME`_RPM_PERIOD_CONV_FACTOR / (`$MaxRPM11` * (1u + `$INSTANCE_NAME`_TOLERANCE_FACTOR)),
        #endif /* `$INSTANCE_NAME`_NUMBER_OF_FANS >= 11u */

        #if(`$INSTANCE_NAME`_NUMBER_OF_FANS >= 12u)
            `$INSTANCE_NAME`_RPM_PERIOD_CONV_FACTOR / (`$MaxRPM12` * (1u + `$INSTANCE_NAME`_TOLERANCE_FACTOR)),
        #endif /* `$INSTANCE_NAME`_NUMBER_OF_FANS >= 12u */

        #if(`$INSTANCE_NAME`_NUMBER_OF_FANS >= 13u)
            `$INSTANCE_NAME`_RPM_PERIOD_CONV_FACTOR / (`$MaxRPM13` * (1u + `$INSTANCE_NAME`_TOLERANCE_FACTOR)),
        #endif /* `$INSTANCE_NAME`_NUMBER_OF_FANS >= 13u */

        #if(`$INSTANCE_NAME`_NUMBER_OF_FANS >= 14u)
            `$INSTANCE_NAME`_RPM_PERIOD_CONV_FACTOR / (`$MaxRPM14` * (1u + `$INSTANCE_NAME`_TOLERANCE_FACTOR)),
        #endif /* `$INSTANCE_NAME`_NUMBER_OF_FANS >= 14u */

        #if(`$INSTANCE_NAME`_NUMBER_OF_FANS >= 15u)
            `$INSTANCE_NAME`_RPM_PERIOD_CONV_FACTOR / (`$MaxRPM15` * (1u + `$INSTANCE_NAME`_TOLERANCE_FACTOR)),
        #endif /* `$INSTANCE_NAME`_NUMBER_OF_FANS >= 15 */

        #if(`$INSTANCE_NAME`_NUMBER_OF_FANS >= 16u)
            `$INSTANCE_NAME`_RPM_PERIOD_CONV_FACTOR / (`$MaxRPM16` * (1u + `$INSTANCE_NAME`_TOLERANCE_FACTOR))
        #endif /* `$INSTANCE_NAME`_NUMBER_OF_FANS >= 16u */
    },
    {
        /* Initialize the Tolerance Field (% of RPM B) */
        #if(`$INSTANCE_NAME`_NUMBER_OF_FANS >= 1u)
            (`$INSTANCE_NAME`_RPM_PERIOD_CONV_FACTOR * `$INSTANCE_NAME`_TOLERANCE_FACTOR) / (`$MaxRPM01`),
        #endif /* `$INSTANCE_NAME`_NUMBER_OF_FANS >= 1u */

        #if(`$INSTANCE_NAME`_NUMBER_OF_FANS >= 2u)
            (`$INSTANCE_NAME`_RPM_PERIOD_CONV_FACTOR * `$INSTANCE_NAME`_TOLERANCE_FACTOR) / (`$MaxRPM02`),
        #endif /* `$INSTANCE_NAME`_NUMBER_OF_FANS >= 2u */

        #if(`$INSTANCE_NAME`_NUMBER_OF_FANS >= 3u)
            (`$INSTANCE_NAME`_RPM_PERIOD_CONV_FACTOR * `$INSTANCE_NAME`_TOLERANCE_FACTOR) / (`$MaxRPM03`),
        #endif /* `$INSTANCE_NAME`_NUMBER_OF_FANS >= 3u */

        #if(`$INSTANCE_NAME`_NUMBER_OF_FANS >= 4u)
            (`$INSTANCE_NAME`_RPM_PERIOD_CONV_FACTOR * `$INSTANCE_NAME`_TOLERANCE_FACTOR) / (`$MaxRPM04`),
        #endif /* `$INSTANCE_NAME`_NUMBER_OF_FANS >= 4u */

        #if(`$INSTANCE_NAME`_NUMBER_OF_FANS >= 5u)
            (`$INSTANCE_NAME`_RPM_PERIOD_CONV_FACTOR * `$INSTANCE_NAME`_TOLERANCE_FACTOR) / (`$MaxRPM05`),
        #endif /* `$INSTANCE_NAME`_NUMBER_OF_FANS >= 5u */

        #if(`$INSTANCE_NAME`_NUMBER_OF_FANS >= 6u)
            (`$INSTANCE_NAME`_RPM_PERIOD_CONV_FACTOR * `$INSTANCE_NAME`_TOLERANCE_FACTOR) / (`$MaxRPM06`),
        #endif /* `$INSTANCE_NAME`_NUMBER_OF_FANS >= 6u */

        #if(`$INSTANCE_NAME`_NUMBER_OF_FANS >= 7u)
            (`$INSTANCE_NAME`_RPM_PERIOD_CONV_FACTOR * `$INSTANCE_NAME`_TOLERANCE_FACTOR) / (`$MaxRPM07`),
        #endif /* `$INSTANCE_NAME`_NUMBER_OF_FANS >= 7u */

        #if(`$INSTANCE_NAME`_NUMBER_OF_FANS >= 8u)
            (`$INSTANCE_NAME`_RPM_PERIOD_CONV_FACTOR * `$INSTANCE_NAME`_TOLERANCE_FACTOR) / (`$MaxRPM08`),
        #endif /* `$INSTANCE_NAME`_NUMBER_OF_FANS >= 8u */

        #if(`$INSTANCE_NAME`_NUMBER_OF_FANS >= 9u)
            (`$INSTANCE_NAME`_RPM_PERIOD_CONV_FACTOR * `$INSTANCE_NAME`_TOLERANCE_FACTOR) / (`$MaxRPM09`),
        #endif /* `$INSTANCE_NAME`_NUMBER_OF_FANS >= 9u */

        #if(`$INSTANCE_NAME`_NUMBER_OF_FANS >= 10)
            (`$INSTANCE_NAME`_RPM_PERIOD_CONV_FACTOR * `$INSTANCE_NAME`_TOLERANCE_FACTOR) / (`$MaxRPM10`),
        #endif /* `$INSTANCE_NAME`_NUMBER_OF_FANS >= 10u */

        #if(`$INSTANCE_NAME`_NUMBER_OF_FANS >= 11u)
            (`$INSTANCE_NAME`_RPM_PERIOD_CONV_FACTOR * `$INSTANCE_NAME`_TOLERANCE_FACTOR) / (`$MaxRPM11`),
        #endif /* `$INSTANCE_NAME`_NUMBER_OF_FANS >= 11u */

        #if(`$INSTANCE_NAME`_NUMBER_OF_FANS >= 12u)
            (`$INSTANCE_NAME`_RPM_PERIOD_CONV_FACTOR * `$INSTANCE_NAME`_TOLERANCE_FACTOR) / (`$MaxRPM12`),
        #endif /* `$INSTANCE_NAME`_NUMBER_OF_FANS >= 12 */

        #if(`$INSTANCE_NAME`_NUMBER_OF_FANS >= 13u)
            (`$INSTANCE_NAME`_RPM_PERIOD_CONV_FACTOR * `$INSTANCE_NAME`_TOLERANCE_FACTOR) / (`$MaxRPM13`),
        #endif /* `$INSTANCE_NAME`_NUMBER_OF_FANS >= 13u */

        #if(`$INSTANCE_NAME`_NUMBER_OF_FANS >= 14u)
            (`$INSTANCE_NAME`_RPM_PERIOD_CONV_FACTOR * `$INSTANCE_NAME`_TOLERANCE_FACTOR) / (`$MaxRPM14`),
        #endif /* `$INSTANCE_NAME`_NUMBER_OF_FANS >= 14u */

        #if(`$INSTANCE_NAME`_NUMBER_OF_FANS >= 15u)
            (`$INSTANCE_NAME`_RPM_PERIOD_CONV_FACTOR * `$INSTANCE_NAME`_TOLERANCE_FACTOR) / (`$MaxRPM15`),
        #endif /* `$INSTANCE_NAME`_NUMBER_OF_FANS >= 15u */

        #if(`$INSTANCE_NAME`_NUMBER_OF_FANS >= 16u)
            (`$INSTANCE_NAME`_RPM_PERIOD_CONV_FACTOR * `$INSTANCE_NAME`_TOLERANCE_FACTOR) / (`$MaxRPM16`)
        #endif /* `$INSTANCE_NAME`_NUMBER_OF_FANS >= 16u */
    }
}
#endif /* `$INSTANCE_NAME`_FAN_CTL_MODE == `$INSTANCE_NAME`_FANCTLMODE_HARDWARE */
;


/*****************************************************************************
 * Fan Electromechanical Properties Data Structure Initialization
 *****************************************************************************/
 /* Properties come from customizer GUI */
`$INSTANCE_NAME`_fanPropertiesStruct `$INSTANCE_NAME`_fanProperties[`$INSTANCE_NAME`_NUMBER_OF_FANS] =
{
    #if(`$INSTANCE_NAME`_NUMBER_OF_FANS >= 1u)
        {
            `$MinRPM01`,
            `$MaxRPM01`,
            (`$MinDuty01` * 100u),
            (`$MaxDuty01` * 100u),
            (((`$MaxDuty01` - `$MinDuty01`) * 100u) / ((`$MaxRPM01`-`$MinRPM01`) / 100u)),
            `$InitialRPM01`,
            (`$InitialDutyCycle01` * 100u)
        },
    #endif /* `$INSTANCE_NAME`_NUMBER_OF_FANS >= 1u */

    #if(`$INSTANCE_NAME`_NUMBER_OF_FANS >= 2u)
        {
            `$MinRPM02`,
            `$MaxRPM02`,
            (`$MinDuty02` * 100u),
            (`$MaxDuty02` * 100u),
            (((`$MaxDuty02` - `$MinDuty02`) * 100u) / ((`$MaxRPM02`-`$MinRPM02`) / 100u)),
            `$InitialRPM02`,
            (`$InitialDutyCycle02` * 100u)
        },
    #endif /* `$INSTANCE_NAME`_NUMBER_OF_FANS >= 2 */

    #if(`$INSTANCE_NAME`_NUMBER_OF_FANS >= 3u)
        {
            `$MinRPM03`,
            `$MaxRPM03`,
            (`$MinDuty03` * 100u),
            (`$MaxDuty03` * 100u),
            (((`$MaxDuty03` - `$MinDuty03`) * 100u) / ((`$MaxRPM03`-`$MinRPM03`) / 100u)),
            `$InitialRPM03`,
            (`$InitialDutyCycle03` * 100u)
        },
    #endif /* `$INSTANCE_NAME`_NUMBER_OF_FANS >= 3u */

    #if(`$INSTANCE_NAME`_NUMBER_OF_FANS >= 4u)
        {
            `$MinRPM04`,
            `$MaxRPM04`,
            (`$MinDuty04` * 100u),
            (`$MaxDuty04` * 100u),
            (((`$MaxDuty04` - `$MinDuty04`) * 100u) / ((`$MaxRPM04`-`$MinRPM04`) / 100u)),
            `$InitialRPM04`,
            (`$InitialDutyCycle04` * 100u)
        },
    #endif /* `$INSTANCE_NAME`_NUMBER_OF_FANS >= 4u */

    #if(`$INSTANCE_NAME`_NUMBER_OF_FANS >= 5u)
        {
            `$MinRPM05`,
            `$MaxRPM05`,
            (`$MinDuty05` * 100u),
            (`$MaxDuty05` * 100u),
            (((`$MaxDuty05` - `$MinDuty05`) * 100u) / ((`$MaxRPM05`-`$MinRPM05`) / 100u)),
            `$InitialRPM05`,
            (`$InitialDutyCycle05` * 100u)
        },
    #endif /* `$INSTANCE_NAME`_NUMBER_OF_FANS >= 5u */

    #if(`$INSTANCE_NAME`_NUMBER_OF_FANS >= 6u)
        {
            `$MinRPM06`,
            `$MaxRPM06`,
            (`$MinDuty06` * 100u),
            (`$MaxDuty06` * 100u),
            (((`$MaxDuty06` - `$MinDuty06`) * 100u) / ((`$MaxRPM06`-`$MinRPM06`) / 100u)),
            `$InitialRPM06`,
            (`$InitialDutyCycle06` * 100u)
        },
    #endif /* `$INSTANCE_NAME`_NUMBER_OF_FANS >= 6u */

    #if(`$INSTANCE_NAME`_NUMBER_OF_FANS >= 7u)
        {
            `$MinRPM07`,
            `$MaxRPM07`,
            (`$MinDuty07` * 100u),
            (`$MaxDuty07` * 100u),
            (((`$MaxDuty07` - `$MinDuty07`) * 100u) / ((`$MaxRPM07`-`$MinRPM07`) / 100u)),
            `$InitialRPM07`,
            (`$InitialDutyCycle07` * 100u)
        },
    #endif /* `$INSTANCE_NAME`_NUMBER_OF_FANS >= 7u */

    #if(`$INSTANCE_NAME`_NUMBER_OF_FANS >= 8u)
        {
            `$MinRPM08`,
            `$MaxRPM08`,
            (`$MinDuty08` * 100u),
            (`$MaxDuty08` * 100u),
            (((`$MaxDuty08` - `$MinDuty08`) * 100u) / ((`$MaxRPM08`-`$MinRPM08`) / 100u)),
            `$InitialRPM08`,
            (`$InitialDutyCycle08` * 100u)
        },
    #endif /* `$INSTANCE_NAME`_NUMBER_OF_FANS >= 8u */

    #if(`$INSTANCE_NAME`_NUMBER_OF_FANS >= 9u)
        {
            `$MinRPM09`,
            `$MaxRPM09`,
            (`$MinDuty09` * 100u),
            (`$MaxDuty09` * 100u),
            (((`$MaxDuty09` - `$MinDuty09`) * 100u) / ((`$MaxRPM09`-`$MinRPM09`) / 100u)),
            `$InitialRPM09`,
            (`$InitialDutyCycle09` * 100u)
        },
    #endif /* `$INSTANCE_NAME`_NUMBER_OF_FANS >= 9u */

    #if(`$INSTANCE_NAME`_NUMBER_OF_FANS >= 10u)
        {
            `$MinRPM10`,
            `$MaxRPM10`,
            (`$MinDuty10` * 100u),
            (`$MaxDuty10` * 100u),
            (((`$MaxDuty10` - `$MinDuty10`) * 100u) / ((`$MaxRPM10`-`$MinRPM10`) / 100u)),
            `$InitialRPM10`,
            (`$InitialDutyCycle10` * 100u)
        },
    #endif /* `$INSTANCE_NAME`_NUMBER_OF_FANS >= 10u */

    #if(`$INSTANCE_NAME`_NUMBER_OF_FANS >= 11u)
        {
            `$MinRPM11`,
            `$MaxRPM11`,
            (`$MinDuty11` * 100u),
            (`$MaxDuty11` * 100u),
            (((`$MaxDuty11` - `$MinDuty11`) * 100u) / ((`$MaxRPM11`-`$MinRPM11`) / 100u)),
            `$InitialRPM11`,
            (`$InitialDutyCycle11`* 100u)
        },
    #endif /* `$INSTANCE_NAME`_NUMBER_OF_FANS >= 11u */

    #if(`$INSTANCE_NAME`_NUMBER_OF_FANS >= 12u)
        {
            `$MinRPM12`,
            `$MaxRPM12`,
            (`$MinDuty12` * 100u),
            (`$MaxDuty12` * 100u),
            (((`$MaxDuty12` - `$MinDuty12`) * 100u) / ((`$MaxRPM12`-`$MinRPM12`) / 100u)),
            `$InitialRPM12`,
            (`$InitialDutyCycle12` * 100u)
        },
    #endif /* `$INSTANCE_NAME`_NUMBER_OF_FANS >= 12u */

    #if(`$INSTANCE_NAME`_NUMBER_OF_FANS >= 13u)
        {
            `$MinRPM13`,
            `$MaxRPM13`,
            (`$MinDuty13` * 100u),
            (`$MaxDuty13` * 100u),
            (((`$MaxDuty13` - `$MinDuty13`) * 100u) / ((`$MaxRPM13`-`$MinRPM13`) / 100u)),
            `$InitialRPM13`,
            (`$InitialDutyCycle13` * 100u)
        },
    #endif /* `$INSTANCE_NAME`_NUMBER_OF_FANS >= 13u */

    #if(`$INSTANCE_NAME`_NUMBER_OF_FANS >= 14u)
        {
            `$MinRPM14`,
            `$MaxRPM14`,
            (`$MinDuty14` * 100u),
            (`$MaxDuty14` * 100u),
            (((`$MaxDuty14` - `$MinDuty14`) * 100u) / ((`$MaxRPM14`-`$MinRPM14`) / 100u)),
            `$InitialRPM14`,
            (`$InitialDutyCycle14` * 100u)
        },
    #endif /* `$INSTANCE_NAME`_NUMBER_OF_FANS >= 14u */

    #if(`$INSTANCE_NAME`_NUMBER_OF_FANS >= 15u)
        {
            `$MinRPM15`,
            `$MaxRPM15`,
            (`$MinDuty15` * 100u),
            (`$MaxDuty15` * 100u),
            (((`$MaxDuty15` - `$MinDuty15`) * 100u) / ((`$MaxRPM15`-`$MinRPM15`) / 100u)),
            `$InitialRPM15`,
            (`$InitialDutyCycle15` * 100u)
        },
    #endif /* `$INSTANCE_NAME`_NUMBER_OF_FANS >= 15u */

    #if(`$INSTANCE_NAME`_NUMBER_OF_FANS >= 16u)
        {
            `$MinRPM16`,
            `$MaxRPM16`,
            (`$MinDuty16` * 100u),
            (`$MaxDuty16` * 100u),
            (((`$MaxDuty16` - `$MinDuty16`) * 100u) / ((`$MaxRPM16`-`$MinRPM16`) / 100u)),
             `$InitialRPM16`,
            (`$InitialDutyCycle16` * 100u)
        }
    #endif /* `$INSTANCE_NAME`_NUMBER_OF_FANS >= 16u */
};


/* [] END OF FILE */
