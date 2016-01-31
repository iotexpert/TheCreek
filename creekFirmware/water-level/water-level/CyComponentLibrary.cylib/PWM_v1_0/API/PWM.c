/*******************************************************************************
* File Name: `$INSTANCE_NAME`.c  
* Version `$CY_MAJOR_VERSION`.`$CY_MINOR_VERSION`
*
* Description:
*  The PWM User Module consist of an 8 or 16-bit counter with two 8 or 16-bit
*  comparitors.  Each instance of this user module is capable of generating
*  two PWM outputs with the same period.  The pulse width is selectable
*  between 1 and 255/65535.  The period is selectable between 2 and 255/65536 clocks. 
*  The compare value output may be configured to be active when the present 
*  counter is less than or less than/equal to the compare value.
*  A terminal count output is also provided.  It generates a pulse one clock
*  width wide when the counter is equal to zero.
*
* Note:
*
*******************************************************************************
* Copyright 2008-2009, Cypress Semiconductor Corporation.  All rights reserved.
* You may use this file only in accordance with the license, terms, conditions, 
* disclaimers, and limitations in the end user license agreement accompanying 
* the software package with which this file was provided.
********************************************************************************/



#include "cytypes.h"
#include "`$INSTANCE_NAME`.h"

uint8 `$INSTANCE_NAME`_initvar = 0;


/*******************************************************************************
* Function Name: `$INSTANCE_NAME`_Start
********************************************************************************
* Summary:
*  The start function initializes the pwm with the default values, the 
*  enables the counter to begin counting.  It does not enable interrupts,
*  the EnableInt command should be called if interrupt generation is required.
*
* Parameters:  
*  void  
*
* Return: 
*  void
*
*******************************************************************************/
void `$INSTANCE_NAME`_Start(void) 
{
    if(`$INSTANCE_NAME`_initvar == 0)
    {
        #if (`$INSTANCE_NAME`_UsingFixedFunction || `$INSTANCE_NAME`_UseControl)
        uint8 ctrl;
        #endif
        `$INSTANCE_NAME`_initvar = 1;
        
        #if (`$INSTANCE_NAME`_UsingFixedFunction)
            /* Mode Bit of the Configuration Register must be 1 before */
            /* you are allowed to write the compare value (FF only) */
            `$INSTANCE_NAME`_CONTROL |= `$INSTANCE_NAME`_CFG0_MODE;
            #if `$INSTANCE_NAME`_DeadBand2_4
                `$INSTANCE_NAME`_CONTROL |= `$INSTANCE_NAME`_CFG0_DB;
            #endif
            ctrl = `$INSTANCE_NAME`_CONTROL2 & ~`$INSTANCE_NAME`_CTRL_CMPMODE1_MASK;
            `$INSTANCE_NAME`_CONTROL2 = ctrl | `$INSTANCE_NAME`_DEFAULT_COMPARE1_MODE;
        #elif (`$INSTANCE_NAME`_UseControl)
            /* Set the default compare mode defined in the parameter */
            ctrl = `$INSTANCE_NAME`_CONTROL & ~`$INSTANCE_NAME`_CTRL_CMPMODE2_MASK & ~`$INSTANCE_NAME`_CTRL_CMPMODE1_MASK;
            `$INSTANCE_NAME`_CONTROL = ctrl | `$INSTANCE_NAME`_DEFAULT_COMPARE2_MODE | `$INSTANCE_NAME`_DEFAULT_COMPARE1_MODE;
        #endif 
        
        #if (!`$INSTANCE_NAME`_UsingFixedFunction)
            #if (`$INSTANCE_NAME`_Resolution == 8)
                /* Set FIFO 0 to 1 byte register for period*/
               `$INSTANCE_NAME`_AUX_CONTROLDP0 |= (`$INSTANCE_NAME`_AUX_CTRL_FIFO0_CLR);
            #else /* (`$INSTANCE_NAME`_Resolution == 16)*/
               /* Set FIFO 0 to 1 byte register for period */
               `$INSTANCE_NAME`_AUX_CONTROLDP0 |= (`$INSTANCE_NAME`_AUX_CTRL_FIFO0_CLR);
               `$INSTANCE_NAME`_AUX_CONTROLDP1 |= (`$INSTANCE_NAME`_AUX_CTRL_FIFO0_CLR);
            #endif
        #endif
        
        `$INSTANCE_NAME`_WritePeriod(`$INSTANCE_NAME`_INIT_PERIOD_VALUE);
        
        #if `$INSTANCE_NAME`_UseOneCompareMode
            `$INSTANCE_NAME`_WriteCompare(`$INSTANCE_NAME`_INIT_COMPARE_VALUE1);
        #else
            `$INSTANCE_NAME`_WriteCompare1(`$INSTANCE_NAME`_INIT_COMPARE_VALUE1);
            `$INSTANCE_NAME`_WriteCompare2(`$INSTANCE_NAME`_INIT_COMPARE_VALUE2);
        #endif
        
        #if `$INSTANCE_NAME`_KillModeMinTime
            `$INSTANCE_NAME`_WriteKillTime(`$INSTANCE_NAME`_MinimumKillTime);
        #endif
        
        #if `$INSTANCE_NAME`_DeadBandUsed
            `$INSTANCE_NAME`_WriteDeadTime( `$INSTANCE_NAME`_INIT_DEAD_TIME );
        #endif

        #if (`$INSTANCE_NAME`_UseStatus || `$INSTANCE_NAME`_UsingFixedFunction)
        `$INSTANCE_NAME`_SetInterruptMode(`$INSTANCE_NAME`_INIT_INTERRUPTS_MODE);
        #endif
        
        #if (`$INSTANCE_NAME`_UsingFixedFunction)
            /* Globally Enable the Fixed Function Block chosen */
            `$INSTANCE_NAME`_GLOBAL_ENABLE |= `$INSTANCE_NAME`_BLOCK_EN_MASK;
            /* Set the Interrupt source to come from the status register */
            `$INSTANCE_NAME`_CONTROL2 |= `$INSTANCE_NAME`_CTRL2_IRQ_SEL;
        #else
            #if(`$INSTANCE_NAME`_UseStatus)
                /* Use the interrupt output of the status register for IRQ output */
                `$INSTANCE_NAME`_STATUS_AUX_CTRL |= `$INSTANCE_NAME`_STATUS_ACTL_INT_EN_MASK;
            #endif
        #endif
    }
    #if (`$INSTANCE_NAME`_UseControl)
        `$INSTANCE_NAME`_CONTROL |= `$INSTANCE_NAME`_CTRL_ENABLE;
    #endif
}


/*******************************************************************************
* Function Name: `$INSTANCE_NAME`_Stop
********************************************************************************
* Summary:
*  The stop function halts the PWM, but does not change any modes or disable
*  interrupts.
*
* Parameters:  
*  void  
*
* Return: 
*  void
*
*******************************************************************************/
void `$INSTANCE_NAME`_Stop(void)
{
    #if (`$INSTANCE_NAME`_UseControl || `$INSTANCE_NAME`_UsingFixedFunction)
        `$INSTANCE_NAME`_CONTROL &= ~`$INSTANCE_NAME`_CTRL_ENABLE;
    #endif
}


#if (`$INSTANCE_NAME`_UseStatus || `$INSTANCE_NAME`_UsingFixedFunction)
/*******************************************************************************
* Function Name: `$INSTANCE_NAME`_SetInterruptMode
********************************************************************************
* Summary:
*  This function is used to set enable the individual interrupt sources.
*
* Parameters:  
*  irqMode:  Enables or disables the interrupt source, compare1, compare2, and 
*            terminal count.  Also, the Less Than or Less Than or equal To 
*            mode may be changed as well with this function. 
*
* Return: 
*  void
*
*******************************************************************************/
void `$INSTANCE_NAME`_SetInterruptMode(uint8 interruptMode)
{
    /* Set the status Registers Mask Register with the bit-field */
    `$INSTANCE_NAME`_STATUS_MASK = interruptMode; 
}


/*******************************************************************************
* Function Name: `$INSTANCE_NAME`_GetInterruptSource
********************************************************************************
* Summary:
*  This function is used to get the source of an interrupt by the ISR routine
*
* Parameters:  
*  void  
*
* Return: 
*  Status Register containing bit-field of interrupt source
*
* Side Effects:  The status register is clear on Read.  This will clear the 
*                interrupt.
*
*******************************************************************************/
uint8 `$INSTANCE_NAME`_GetInterruptSource()
{
    return `$INSTANCE_NAME`_STATUS;
}


/*******************************************************************************
* Function Name: `$INSTANCE_NAME`_ReadStatusRegister
********************************************************************************
* Summary:
*  Reads the status register and returns it's state. This function should use
*  defined types for the bit-field information as the bits in this register may
*  be permuteable.
*
* Parameters:  
*  void
*
* Return: 
*  The contents of the status register
*
* Side Effects:
*  Some status register bits are clear on read. 
*******************************************************************************/
uint8   `$INSTANCE_NAME`_ReadStatusRegister(void)
{
    return `$INSTANCE_NAME`_STATUS;
}
#endif /* (`$INSTANCE_NAME`_UseStatus || `$INSTANCE_NAME`_UsingFixedFunction) */


#if (`$INSTANCE_NAME`_UseControl || `$INSTANCE_NAME`_UsingFixedFunction)
/*******************************************************************************
* Function Name: `$INSTANCE_NAME`_ReadControlRegister
********************************************************************************
* Summary:
*  Reads the control register and returns it's state. This function should use
*  defined types for the bit-field information as the bits in this register may
*  be permuteable.
*
* Parameters:  
*  void
*
* Return: 
*  The contents of the control register
*
*******************************************************************************/
uint8   `$INSTANCE_NAME`_ReadControlRegister(void)
{
    return `$INSTANCE_NAME`_CONTROL;
}


/*******************************************************************************
* Function Name: `$INSTANCE_NAME`_WriteControlRegister
********************************************************************************
* Summary:
*  Sets the bit-field of the control register.  This function should use
*  defined types for the bit-field information as the bits in this
*  register may be permuteable.
*
* Parameters:  
*  void
*
* Return: 
*  The contents of the control register
*
*******************************************************************************/
void    `$INSTANCE_NAME`_WriteControlRegister(uint8 control)
{
    `$INSTANCE_NAME`_CONTROL = control;
}
#endif  /* (`$INSTANCE_NAME`_UseControl || `$INSTANCE_NAME`_UsingFixedFunction) */


#if `$INSTANCE_NAME`_UseOneCompareMode
/*******************************************************************************
* Function Name: `$INSTANCE_NAME`_SetCompareMode
********************************************************************************
* Summary:
*  This function writes the Compare Mode for the pwm output when in Dither mode
*
* Parameters:
*  comparemode:  The new compare mode for the PWM output. Use the compare types
*                defined in the H file as input arguments.
*
* Return:
*  void
*
*******************************************************************************/
void `$INSTANCE_NAME`_SetCompareMode( uint8 comparemode )
{
    #if(`$INSTANCE_NAME`_UsingFixedFunction)
        uint8 comparemodemasked = (comparemode << `$INSTANCE_NAME`_CTRL_CMPMODE1_SHIFT);
        `$INSTANCE_NAME`_CONTROL2 &= ~`$INSTANCE_NAME`_CTRL_CMPMODE1_MASK; /*Clear Existing Data */
        `$INSTANCE_NAME`_CONTROL2 |= comparemodemasked;
        
    #elif (`$INSTANCE_NAME`_UseControl)
        uint8 comparemode1masked = (comparemode << `$INSTANCE_NAME`_CTRL_CMPMODE1_SHIFT) & `$INSTANCE_NAME`_CTRL_CMPMODE1_MASK;
        uint8 comparemode2masked = (comparemode << `$INSTANCE_NAME`_CTRL_CMPMODE2_SHIFT) & `$INSTANCE_NAME`_CTRL_CMPMODE2_MASK;
        `$INSTANCE_NAME`_CONTROL &= ~(`$INSTANCE_NAME`_CTRL_CMPMODE1_MASK | `$INSTANCE_NAME`_CTRL_CMPMODE2_MASK); /*Clear existing mode */
        `$INSTANCE_NAME`_CONTROL |= (comparemode1masked | comparemode2masked);
        
    #else
        uint8 temp = comparemode;
    #endif
}

#else /* UseOneCompareMode */


/*******************************************************************************
* Function Name: `$INSTANCE_NAME`_SetCompareMode1
********************************************************************************
* Summary:
*  This function writes the Compare Mode for the pwm or pwm1 output
*
* Parameters:  
*  comparemode:  The new compare mode for the PWM output. Use the compare types
*                defined in the H file as input arguments.
*
* Return: 
*  void
*
*******************************************************************************/
void `$INSTANCE_NAME`_SetCompareMode1( uint8 comparemode )
{
    uint8 comparemodemasked = (comparemode << `$INSTANCE_NAME`_CTRL_CMPMODE1_SHIFT) & `$INSTANCE_NAME`_CTRL_CMPMODE1_MASK;
    #if(`$INSTANCE_NAME`_UsingFixedFunction)
        `$INSTANCE_NAME`_CONTROL2 &= `$INSTANCE_NAME`_CTRL_CMPMODE1_MASK; /*Clear existing mode */
        `$INSTANCE_NAME`_CONTROL2 |= comparemodemasked; 
    #elif (`$INSTANCE_NAME`_UseControl)
        `$INSTANCE_NAME`_CONTROL &= `$INSTANCE_NAME`_CTRL_CMPMODE1_MASK; /*Clear existing mode */
        `$INSTANCE_NAME`_CONTROL |= comparemodemasked;
    #endif    
}


/*******************************************************************************
* Function Name: `$INSTANCE_NAME`_SetCompareMode2
********************************************************************************
* Summary:
*  This function writes the Compare Mode for the pwm or pwm2 output
*
* Parameters:  
*  comparemode:  The new compare mode for the PWM output. Use the compare types
*                defined in the H file as input arguments.
*
* Return: 
*  void
*
*******************************************************************************/
void `$INSTANCE_NAME`_SetCompareMode2( uint8 comparemode )
{
    #if(`$INSTANCE_NAME`_UsingFixedFunction)
        /* Do Nothing because there is no second Compare Mode Register in FF block*/ 
    #elif (`$INSTANCE_NAME`_UseControl)
    uint8 comparemodemasked = (comparemode << `$INSTANCE_NAME`_CTRL_CMPMODE2_SHIFT) & `$INSTANCE_NAME`_CTRL_CMPMODE2_MASK;
    `$INSTANCE_NAME`_CONTROL &= `$INSTANCE_NAME`_CTRL_CMPMODE2_MASK; /*Clear existing mode */
    `$INSTANCE_NAME`_CONTROL |= comparemodemasked;
    #endif    
}
#endif /* UseOneCompareMode */


/*******************************************************************************
* Function Name: `$INSTANCE_NAME`_WriteCounter
********************************************************************************
* Summary:
*  This function is used to change the counter value.
*
* Parameters:  
*  counter:  This value may be between 1 and (2^Resolution)-1.   
*
* Return: 
*  void
*
*******************************************************************************/
void `$INSTANCE_NAME`_WriteCounter(`$RegSizeReplacementString` counter)
{
    #if(`$INSTANCE_NAME`_UsingFixedFunction)
        uint16 counter_temp = (uint16)counter;
        CY_SET_REG16(`$INSTANCE_NAME`_COUNTER_LSB_PTR, counter_temp);
    #else
        `$CySetRegReplacementString`(`$INSTANCE_NAME`_COUNTER_LSB_PTR, counter);
    #endif
}


#if (!`$INSTANCE_NAME`_UsingFixedFunction)
/*******************************************************************************
* Function Name: `$INSTANCE_NAME`_ReadCounter
********************************************************************************
* Summary:
*  This function returns the current value of the counter.  It doesn't matter
*  if the counter is enabled or running.
*
* Parameters:  
*  void  
*
* Return: 
*  The current value of the counter.
*
*******************************************************************************/
`$RegSizeReplacementString` `$INSTANCE_NAME`_ReadCounter(void)
{
    `$CyGetRegReplacementString`(`$INSTANCE_NAME`_COUNTER_LSB_PTR);
    return ( `$CyGetRegReplacementString`(`$INSTANCE_NAME`_CAPTURE_LSB_PTR) );
}


/*******************************************************************************
* Function Name: `$INSTANCE_NAME`_ReadCapture
********************************************************************************
* Summary:
*  This function returns the last value captured.
*
* Parameters:  
*  void 
*
* Return: 
*  Present Capture value.
*
*******************************************************************************/
`$RegSizeReplacementString` `$INSTANCE_NAME`_ReadCapture( void )
{
   return ( `$CyGetRegReplacementString`(`$INSTANCE_NAME`_CAPTURE_LSB_PTR) );  
}


#if (`$INSTANCE_NAME`_UseStatus)
/*******************************************************************************
* Function Name: `$INSTANCE_NAME`_ClearFIFO
********************************************************************************
* Summary:
*  This function clears all capture data from the capture FIFO
*
* Parameters:  
*  void
*
* Return: 
*  void
*
*******************************************************************************/
void `$INSTANCE_NAME`_ClearFIFO(void)
{
    while(`$INSTANCE_NAME`_ReadStatusRegister() & `$INSTANCE_NAME`_STATUS_FIFONEMPTY_SHIFT)
        `$INSTANCE_NAME`_ReadCapture();
}
#endif /* `$INSTANCE_NAME`_UseStatus */
#endif /* !`$INSTANCE_NAME`_UsingFixedFunction */


/*******************************************************************************
* Function Name: `$INSTANCE_NAME`_WritePeriod
********************************************************************************
* Summary:
*  This function is used to change the period of the counter.  The new period 
*  will be loaded the next time terminal count is detected.
*
* Parameters:  
*  void  
*
* Return: 
*  Period value. May be between 1 and (2^Resolution)-1.  A value of 0 will result in
*  the counter remaining at zero.
*
*******************************************************************************/
void `$INSTANCE_NAME`_WritePeriod(`$RegSizeReplacementString` period)
{
    #if(`$INSTANCE_NAME`_UsingFixedFunction)
        uint16 period_temp = (uint16)period;
        CY_SET_REG16(`$INSTANCE_NAME`_PERIOD_LSB_PTR, period_temp);
    #else
        `$CySetRegReplacementString`(`$INSTANCE_NAME`_PERIOD_LSB_PTR, period);
    #endif
}


/*******************************************************************************
* Function Name: `$INSTANCE_NAME`_ReadPeriod
********************************************************************************
* Summary:
*  This function reads the period without affecting pwm operation.
*
* Parameters:  
*  `$RegSizeReplacementString`:  Current Period Value
*
* Return: 
*  (void)
*
*******************************************************************************/
`$RegSizeReplacementString` `$INSTANCE_NAME`_ReadPeriod( void )
{
   return ( `$CyGetRegReplacementString`(`$INSTANCE_NAME`_PERIOD_LSB_PTR) );
}


#if `$INSTANCE_NAME`_UseOneCompareMode
/*******************************************************************************
* Function Name: `$INSTANCE_NAME`_WriteCompare
********************************************************************************
* Summary:
*  This funtion is used to change the compare1 value when the PWM is in Dither
*  mode.  The compare output will 
*  reflect the new value on the next UDB clock.  The compare output will be 
*  driven high when the present counter value is compared to the compare value
*  based on the compare mode defined in Dither Mode.
*
* Parameters:  
*  compare:  New compare value.  
*
* Return: 
*  void
*
*******************************************************************************/
void `$INSTANCE_NAME`_WriteCompare(`$RegSizeReplacementString` compare)
{
   `$CySetRegReplacementString`(`$INSTANCE_NAME`_COMPARE1_LSB_PTR, compare);
   #if (`$INSTANCE_NAME`_PWMMode == `$INSTANCE_NAME`__B_PWM__DITHER)
        `$CySetRegReplacementString`(`$INSTANCE_NAME`_COMPARE2_LSB_PTR, compare+1);
   #endif
}


/*******************************************************************************
* Function Name: `$INSTANCE_NAME`_ReadCompare
********************************************************************************
* Summary:
*  This function returns the compare value.
*
* Parameters:  
*  void  
*
* Return: 
*  Current compare value.
*
*******************************************************************************/
`$RegSizeReplacementString` `$INSTANCE_NAME`_ReadCompare( void )
{
  return ( `$CyGetRegReplacementString`(`$INSTANCE_NAME`_COMPARE1_LSB_PTR) );
}


#else


/*******************************************************************************
* Function Name: `$INSTANCE_NAME`_WriteCompare1
********************************************************************************
* Summary:
*  This funtion is used to change the compare1 value.  The compare output will 
*  reflect the new value on the next UDB clock.  The compare output will be 
*  driven high when the present counter value is less than or less than or 
*  equal to the compare register, depending on the mode.
*
* Parameters:  
*  compare:  New compare value.  
*
* Return: 
*  void
*
*******************************************************************************/
void `$INSTANCE_NAME`_WriteCompare1(`$RegSizeReplacementString` compare)
{
    #if(`$INSTANCE_NAME`_UsingFixedFunction)
        uint16 compare_temp = (uint16)compare;
        CY_SET_REG16(`$INSTANCE_NAME`_COMPARE1_LSB_PTR, compare_temp);
    #else
        `$CySetRegReplacementString`(`$INSTANCE_NAME`_COMPARE1_LSB_PTR, compare);
    #endif
}


/*******************************************************************************
* Function Name: `$INSTANCE_NAME`_ReadCompare1
********************************************************************************
* Summary:
*  This function returns the compare1 value.
*
* Parameters:  
*  void  
*
* Return: 
*  Current compare value.
*
*******************************************************************************/
`$RegSizeReplacementString` `$INSTANCE_NAME`_ReadCompare1( void )
{
  return ( `$CyGetRegReplacementString`(`$INSTANCE_NAME`_COMPARE1_LSB_PTR) );
}


/*******************************************************************************
* Function Name: `$INSTANCE_NAME`_WriteCompare2
********************************************************************************
* Summary:
*  This funtion is used to change the compare value, for compare1 output.  
*  The compare output will reflect the new value on the next UDB clock.  
*  The compare output will be driven high when the present counter value is 
*  less than or less than or equal to the compare register, depending on the 
*  mode.
*
* Parameters:  
*  compare:  New compare value.  
*
* Return: 
*  void
*
*******************************************************************************/
void `$INSTANCE_NAME`_WriteCompare2(`$RegSizeReplacementString` compare)
{
    #if(`$INSTANCE_NAME`_UsingFixedFunction)
       //TODO: This should generate an error because the fixed function block doesn't have a compare 2 register
       uint16 compare_temp = (uint16)compare;
       CY_SET_REG16(`$INSTANCE_NAME`_COMPARE2_LSB_PTR, compare_temp);
    #else
        `$CySetRegReplacementString`(`$INSTANCE_NAME`_COMPARE2_LSB_PTR, compare);
    #endif
}


/*******************************************************************************
* Function Name: `$INSTANCE_NAME`_ReadCompare2
********************************************************************************
* Summary:
*  This function returns the compare value, for the second compare output.
*
* Parameters:  
*  void
*
* Return: 
*  Present compare2 value.
*
*******************************************************************************/
`$RegSizeReplacementString` `$INSTANCE_NAME`_ReadCompare2( void )
{
    return ( `$CyGetRegReplacementString`(`$INSTANCE_NAME`_COMPARE2_LSB_PTR) );
}
#endif /* UseOneCompareMode */


#if (`$INSTANCE_NAME`_DeadBandUsed)
/*******************************************************************************
* Function Name: `$INSTANCE_NAME`_WriteDeadTime
********************************************************************************
* Summary:
*  This function writes the dead-band counts to the corresponding register
*
* Parameters:  
*  deadtime:  Number of counts for dead time 
*
* Return: 
*  void
*
*******************************************************************************/
void `$INSTANCE_NAME`_WriteDeadTime( uint8 deadtime )
{
    /* If using the Dead Band 1-255 mode then just write the register */
    #if(!`$INSTANCE_NAME`_DeadBand2_4)
        CY_SET_REG8(`$INSTANCE_NAME`_DEADBAND_COUNT_PTR, deadtime);
    #else
        /* Otherwise the data has to be masked and offset */
        uint8 deadtimemasked = (deadtime << `$INSTANCE_NAME`_DEADBAND_COUNT_SHIFT) & `$INSTANCE_NAME`_DEADBAND_COUNT_MASK;
        `$INSTANCE_NAME`_DEADBAND_COUNT &= ~`$INSTANCE_NAME`_DEADBAND_COUNT_MASK; /* Clear existing data */
        `$INSTANCE_NAME`_DEADBAND_COUNT |= deadtimemasked; /* Set new dead time */
    #endif
}


/*******************************************************************************
* Function Name: `$INSTANCE_NAME`_ReadDeadTime
********************************************************************************
* Summary:
*  This function reads the dead-band counts from the corresponding register
*
* Parameters:  
*  void
*
* Return: 
*  Dead Band Counts
*
*******************************************************************************/
uint8 `$INSTANCE_NAME`_ReadDeadTime()
{
    /* If using the Dead Band 1-255 mode then just read the register */
    #if(!`$INSTANCE_NAME`_DeadBand2_4)
        return ( CY_GET_REG8(`$INSTANCE_NAME`_DEADBAND_COUNT_PTR) );
    #else
        /* Otherwise the data has to be masked and offset */
        return ((`$INSTANCE_NAME`_DEADBAND_COUNT & `$INSTANCE_NAME`_DEADBAND_COUNT_MASK) >> `$INSTANCE_NAME`_DEADBAND_COUNT_SHIFT);
    #endif
}
#endif /* DeadBandUsed */


#if ( `$INSTANCE_NAME`_KillModeMinTime)
/*******************************************************************************
* Function Name: `$INSTANCE_NAME`_WriteKillTime
********************************************************************************
* Summary:
*  This function writes the kill-time counts to the corresponding register
*
* Parameters:
*  killtime:  Number of counts for killtime 
*
* Return: 
*  void
*
*******************************************************************************/
void `$INSTANCE_NAME`_WriteKillTime( uint8 killtime )
{
    /* Not available in Fixed Function mode.  This is taken care of by the 
     * customizer to not allow the user to set Fixed Function and set 
     * the Kill Mode*/
    CY_SET_REG8(`$INSTANCE_NAME`_KILLMODEMINTIME_PTR, killtime);
}


/*******************************************************************************
* Function Name: `$INSTANCE_NAME`_ReadKillTime
********************************************************************************
* Summary:
*  This function reads the Kill-time counts from the corresponding register
*
* Parameters:  
*  void
*
* Return: 
*  Kill Time Counts
*
*******************************************************************************/
uint8 `$INSTANCE_NAME`_ReadKillTime()
{
    return ( CY_GET_REG8(`$INSTANCE_NAME`_KILLMODEMINTIME_PTR) );
}
#endif /* KillModeMinTime */


/* [] END OF FILE */
