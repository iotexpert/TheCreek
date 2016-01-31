/*******************************************************************************
* File Name: `$INSTANCE_NAME`.c  
* Version `$CY_MAJOR_VERSION`.`$CY_MINOR_VERSION`
*
* Description:
*  The PWM User Module consist of an 8 or 16-bit counter with two 8 or 16-bit
*  comparitors. Each instance of this user module is capable of generating
*  two PWM outputs with the same period. The pulse width is selectable between
*  1 and 255/65535. The period is selectable between 2 and 255/65536 clocks. 
*  The compare value output may be configured to be active when the present 
*  counter is less than or less than/equal to the compare value.
*  A terminal count output is also provided. It generates a pulse one clock
*  width wide when the counter is equal to zero.
*
* Note:
*
*******************************************************************************
* Copyright 2008-2010, Cypress Semiconductor Corporation.  All rights reserved.
* You may use this file only in accordance with the license, terms, conditions, 
* disclaimers, and limitations in the end user license agreement accompanying 
* the software package with which this file was provided.
********************************************************************************/

#include "cytypes.h"
#include "`$INSTANCE_NAME`.h"

uint8 `$INSTANCE_NAME`_initVar = 0u;

/*******************************************************************************
* Function Name: `$INSTANCE_NAME`_Start
********************************************************************************
*
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
* Global variables:
*  `$INSTANCE_NAME`_initVar: Is modified when this function is called for the first 
*   time. Is used to ensure that initialization happens only once.
*
* Reentrant:
*  Yes
*
*******************************************************************************/
void `$INSTANCE_NAME`_Start(void) 
{
    /* If not Initialized then initialize all required hardware and software */
    if(`$INSTANCE_NAME`_initVar == 0u)
    {
        `$INSTANCE_NAME`_Init();
        `$INSTANCE_NAME`_initVar = 1u;
    }
    `$INSTANCE_NAME`_Enable();

}


/*******************************************************************************
* Function Name: `$INSTANCE_NAME`_Init
********************************************************************************
*
* Summary:
*  Initialize component's parameters to the parameters set by user in the 
*  customizer of the component placed onto schematic. Usually called in 
*  `$INSTANCE_NAME`_Start().
*
* Parameters:  
*  void
*
* Return: 
*  void
*
* Reentrant:
*  Yes
*
*******************************************************************************/
void `$INSTANCE_NAME`_Init(void) `=ReentrantKeil($INSTANCE_NAME . "_Init")`
{
    #if (`$INSTANCE_NAME`_UsingFixedFunction || `$INSTANCE_NAME`_UseControl)
        uint8 ctrl;
    #endif
    
   #if (`$INSTANCE_NAME`_UsingFixedFunction)
        /* You are allowed to write the compare value (FF only) */
        `$INSTANCE_NAME`_CONTROL |= `$INSTANCE_NAME`_CFG0_MODE;
        #if (`$INSTANCE_NAME`_DeadBand2_4)
            `$INSTANCE_NAME`_CONTROL |= `$INSTANCE_NAME`_CFG0_DB;
        #endif
		
		/* Set the default Compare Mode */
		#if(`$INSTANCE_NAME`_PSOC3_ES2 || `$INSTANCE_NAME`_PSOC5_ES1)
			ctrl = `$INSTANCE_NAME`_CONTROL2 & ~`$INSTANCE_NAME`_CTRL_CMPMODE1_MASK;
			`$INSTANCE_NAME`_CONTROL2 = ctrl | `$INSTANCE_NAME`_DEFAULT_COMPARE1_MODE;
		#endif
		#if(`$INSTANCE_NAME`_PSOC3_ES3 || `$INSTANCE_NAME`_PSOC5_ES2)
			ctrl = `$INSTANCE_NAME`_CONTROL3 & ~`$INSTANCE_NAME`_CTRL_CMPMODE1_MASK;
			`$INSTANCE_NAME`_CONTROL3 = ctrl | `$INSTANCE_NAME`_DEFAULT_COMPARE1_MODE;
		#endif
        
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
    `$INSTANCE_NAME`_WriteCounter(`$INSTANCE_NAME`_INIT_PERIOD_VALUE);
        
        #if (`$INSTANCE_NAME`_UseOneCompareMode)
            `$INSTANCE_NAME`_WriteCompare(`$INSTANCE_NAME`_INIT_COMPARE_VALUE1);
        #else
            `$INSTANCE_NAME`_WriteCompare1(`$INSTANCE_NAME`_INIT_COMPARE_VALUE1);
            `$INSTANCE_NAME`_WriteCompare2(`$INSTANCE_NAME`_INIT_COMPARE_VALUE2);
        #endif
        
        #if (`$INSTANCE_NAME`_KillModeMinTime)
            `$INSTANCE_NAME`_WriteKillTime(`$INSTANCE_NAME`_MinimumKillTime);
        #endif
        
        #if (`$INSTANCE_NAME`_DeadBandUsed)
            `$INSTANCE_NAME`_WriteDeadTime(`$INSTANCE_NAME`_INIT_DEAD_TIME);
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
            /* Clear the FIFO to enable the `$INSTANCE_NAME`_STATUS_FIFOFULL
                   bit to be set on FIFO full. */
            `$INSTANCE_NAME`_ClearFIFO();
        #endif
    #endif
}


/*******************************************************************************
* Function Name: `$INSTANCE_NAME`_Enable
********************************************************************************
*
* Summary: 
*  Enables the PWM block operation
*
* Parameters:  
*  void
*
* Return: 
*  void
*
* Side Effects: 
*  This works only if software enable mode is chosen
*
* Reentrant:
*  Yes
*
*******************************************************************************/
void `$INSTANCE_NAME`_Enable(void) `=ReentrantKeil($INSTANCE_NAME . "_Enable")`
{
    #if (`$INSTANCE_NAME`_UseControl || `$INSTANCE_NAME`_UsingFixedFunction)
        `$INSTANCE_NAME`_CONTROL |= `$INSTANCE_NAME`_CTRL_ENABLE;
    #endif
}


/*******************************************************************************
* Function Name: `$INSTANCE_NAME`_Stop
********************************************************************************
*
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
* Side Effects:
*  If the Enable mode is set to Hardware only then this function
*  has no effect on the operation of the PWM
*
* Reentrant:
*  Yes
*
*******************************************************************************/
void `$INSTANCE_NAME`_Stop(void) `=ReentrantKeil($INSTANCE_NAME . "_Stop")`
{
    #if (`$INSTANCE_NAME`_UseControl || `$INSTANCE_NAME`_UsingFixedFunction)
        `$INSTANCE_NAME`_CONTROL &= ~`$INSTANCE_NAME`_CTRL_ENABLE;
    #endif
}


#if (`$INSTANCE_NAME`_UseOneCompareMode)
#if (`$INSTANCE_NAME`_CompareMode1SW)
/*******************************************************************************
* Function Name: `$INSTANCE_NAME`_SetCompareMode
********************************************************************************
* 
* Summary:
*  This function writes the Compare Mode for the pwm output when in Dither mode,
*  Center Align Mode or One Output Mode.
*
* Parameters:
*  comparemode:  The new compare mode for the PWM output. Use the compare types
*                defined in the H file as input arguments.
*
* Return:
*  void
*
* Reentrant:
*  Yes
*
*******************************************************************************/
void `$INSTANCE_NAME`_SetCompareMode(uint8 comparemode) `=ReentrantKeil($INSTANCE_NAME . "_SetCompareMode")`
{
    #if(`$INSTANCE_NAME`_UsingFixedFunction)
	    #if(`$INSTANCE_NAME`_PSOC3_ES2 || `$INSTANCE_NAME`_PSOC5_ES1)
			uint8 comparemodemasked = (comparemode << `$INSTANCE_NAME`_CTRL_CMPMODE1_SHIFT);
			`$INSTANCE_NAME`_CONTROL2 &= ~`$INSTANCE_NAME`_CTRL_CMPMODE1_MASK; /*Clear Existing Data */
			`$INSTANCE_NAME`_CONTROL2 |= comparemodemasked;  
		#endif
		
	    #if(`$INSTANCE_NAME`_PSOC3_ES3 || `$INSTANCE_NAME`_PSOC5_ES2)
		    uint8 comparemodemasked = (comparemode << `$INSTANCE_NAME`_CTRL_CMPMODE1_SHIFT);
            `$INSTANCE_NAME`_CONTROL3 &= ~`$INSTANCE_NAME`_CTRL_CMPMODE1_MASK; /*Clear Existing Data */
            `$INSTANCE_NAME`_CONTROL3 |= comparemodemasked;  	
		#endif
                
    #elif (`$INSTANCE_NAME`_UseControl)
        uint8 comparemode1masked = (comparemode << `$INSTANCE_NAME`_CTRL_CMPMODE1_SHIFT) & `$INSTANCE_NAME`_CTRL_CMPMODE1_MASK;
        uint8 comparemode2masked = (comparemode << `$INSTANCE_NAME`_CTRL_CMPMODE2_SHIFT) & `$INSTANCE_NAME`_CTRL_CMPMODE2_MASK;
        `$INSTANCE_NAME`_CONTROL &= ~(`$INSTANCE_NAME`_CTRL_CMPMODE1_MASK | `$INSTANCE_NAME`_CTRL_CMPMODE2_MASK); /*Clear existing mode */
        `$INSTANCE_NAME`_CONTROL |= (comparemode1masked | comparemode2masked);
        
    #else
        uint8 temp = comparemode;
    #endif
}
#endif /* `$INSTANCE_NAME`_CompareMode1SW */

#else /* UseOneCompareMode */


#if (`$INSTANCE_NAME`_CompareMode1SW)
/*******************************************************************************
* Function Name: `$INSTANCE_NAME`_SetCompareMode1
********************************************************************************
* 
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
* Reentrant:
*  Yes
*
*******************************************************************************/
void `$INSTANCE_NAME`_SetCompareMode1(uint8 comparemode) `=ReentrantKeil($INSTANCE_NAME . "_SetCompareMode1")`
{
    uint8 comparemodemasked = (comparemode << `$INSTANCE_NAME`_CTRL_CMPMODE1_SHIFT) & `$INSTANCE_NAME`_CTRL_CMPMODE1_MASK;
    #if(`$INSTANCE_NAME`_UsingFixedFunction)
	    #if(`$INSTANCE_NAME`_PSOC3_ES2 || `$INSTANCE_NAME`_PSOC5_ES1)
			`$INSTANCE_NAME`_CONTROL2 &= `$INSTANCE_NAME`_CTRL_CMPMODE1_MASK; /*Clear existing mode */
			`$INSTANCE_NAME`_CONTROL2 |= comparemodemasked; 
	    #endif
		
		#if(`$INSTANCE_NAME`_PSOC3_ES3 || `$INSTANCE_NAME`_PSOC5_ES2)
		    `$INSTANCE_NAME`_CONTROL3 &= `$INSTANCE_NAME`_CTRL_CMPMODE1_MASK; /*Clear existing mode */
			`$INSTANCE_NAME`_CONTROL3 |= comparemodemasked; 
	    #endif
		
    #elif (`$INSTANCE_NAME`_UseControl)
        `$INSTANCE_NAME`_CONTROL &= `$INSTANCE_NAME`_CTRL_CMPMODE1_MASK; /*Clear existing mode */
        `$INSTANCE_NAME`_CONTROL |= comparemodemasked;
    #endif    
}
#endif /* `$INSTANCE_NAME`_CompareMode1SW */


#if (`$INSTANCE_NAME`_CompareMode2SW)
/*******************************************************************************
* Function Name: `$INSTANCE_NAME`_SetCompareMode2
********************************************************************************
* 
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
* Reentrant:
*  Yes
*
*******************************************************************************/
void `$INSTANCE_NAME`_SetCompareMode2(uint8 comparemode) `=ReentrantKeil($INSTANCE_NAME . "_SetCompareMode2")`
{
    #if(`$INSTANCE_NAME`_UsingFixedFunction)
        /* Do Nothing because there is no second Compare Mode Register in FF block*/ 
    #elif (`$INSTANCE_NAME`_UseControl)
    uint8 comparemodemasked = (comparemode << `$INSTANCE_NAME`_CTRL_CMPMODE2_SHIFT) & `$INSTANCE_NAME`_CTRL_CMPMODE2_MASK;
    `$INSTANCE_NAME`_CONTROL &= `$INSTANCE_NAME`_CTRL_CMPMODE2_MASK; /*Clear existing mode */
    `$INSTANCE_NAME`_CONTROL |= comparemodemasked;
    #endif    
}
#endif /*`$INSTANCE_NAME`_CompareMode2SW */
#endif /* UseOneCompareMode */


/*******************************************************************************
* Function Name: `$INSTANCE_NAME`_WriteCounter
********************************************************************************
* 
* Summary:
*  This function is used to change the counter value.
*
* Parameters:  
*  counter:  This value may be between 1 and (2^Resolution)-1.   
*
* Return: 
*  void
*
* Reentrant:
*  Yes
*
*******************************************************************************/
void `$INSTANCE_NAME`_WriteCounter(`$RegSizeReplacementString` counter) `=ReentrantKeil($INSTANCE_NAME . "_WriteCounter")`
{
    #if(`$INSTANCE_NAME`_UsingFixedFunction)
        CY_SET_REG16(`$INSTANCE_NAME`_COUNTER_LSB_PTR, (uint16)counter);
    #else
        `$CySetRegReplacementString`(`$INSTANCE_NAME`_COUNTER_LSB_PTR, counter);
    #endif
}


#if (!`$INSTANCE_NAME`_UsingFixedFunction)
/*******************************************************************************
* Function Name: `$INSTANCE_NAME`_ReadCounter
********************************************************************************
* 
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
* Reentrant:
*  Yes
*
*******************************************************************************/
`$RegSizeReplacementString` `$INSTANCE_NAME`_ReadCounter(void) `=ReentrantKeil($INSTANCE_NAME . "_ReadCounter")`
{
    /* Force capture by reading Accumulator */
    /* Must first do a software capture to be able to read the counter */
    /* It is up to the user code to make sure there isn't already captured data in the FIFO */
    CY_GET_REG8(`$INSTANCE_NAME`_COUNTERCAP_LSB_PTR);
    
    /* Read the data from the FIFO (or capture register for Fixed Function)*/
    return (`$CyGetRegReplacementString`(`$INSTANCE_NAME`_CAPTURE_LSB_PTR));
}


#if (`$INSTANCE_NAME`_UseStatus)
/*******************************************************************************
* Function Name: `$INSTANCE_NAME`_ClearFIFO
********************************************************************************
* 
* Summary:
*  This function clears all capture data from the capture FIFO
*
* Parameters:  
*  void
*
* Return: 
*  void
*
* Reentrant:
*  Yes
*
*******************************************************************************/
void `$INSTANCE_NAME`_ClearFIFO(void) `=ReentrantKeil($INSTANCE_NAME . "_ClearFIFO")`
{
    while(`$INSTANCE_NAME`_ReadStatusRegister() & `$INSTANCE_NAME`_STATUS_FIFONEMPTY)
        `$INSTANCE_NAME`_ReadCapture();
}
#endif /* `$INSTANCE_NAME`_UseStatus */
#endif /* !`$INSTANCE_NAME`_UsingFixedFunction */


/*******************************************************************************
* Function Name: `$INSTANCE_NAME`_WritePeriod
********************************************************************************
* 
* Summary:
*  This function is used to change the period of the counter.  The new period 
*  will be loaded the next time terminal count is detected.
*
* Parameters:  
*  period:  Period value. May be between 1 and (2^Resolution)-1.  A value of 0 
*           will result in the counter remaining at zero.
*
* Return: 
*  void
*
* Reentrant:
*  Yes
*
*******************************************************************************/
void `$INSTANCE_NAME`_WritePeriod(`$RegSizeReplacementString` period) `=ReentrantKeil($INSTANCE_NAME . "_WritePeriod")`
{
    #if(`$INSTANCE_NAME`_UsingFixedFunction)
        CY_SET_REG16(`$INSTANCE_NAME`_PERIOD_LSB_PTR, (uint16)period);
    #else
        `$CySetRegReplacementString`(`$INSTANCE_NAME`_PERIOD_LSB_PTR, period);
    #endif
}


#if (`$INSTANCE_NAME`_UseOneCompareMode)
/*******************************************************************************
* Function Name: `$INSTANCE_NAME`_WriteCompare
********************************************************************************
* 
* Summary:
*  This funtion is used to change the compare1 value when the PWM is in Dither
*  mode. The compare output will reflect the new value on the next UDB clock. 
*  The compare output will be driven high when the present counter value is 
*  compared to the compare value based on the compare mode defined in 
*  Dither Mode.
*
* Parameters:  
*  compare:  New compare value.  
*
* Return: 
*  void
*
* Side Effects:
*  This function is only available if the PWM mode parameter is set to
*  Dither Mode, Center Aligned Mode or One Output Mode
*
* Reentrant:
*  Yes
*
*******************************************************************************/
void `$INSTANCE_NAME`_WriteCompare(`$RegSizeReplacementString` compare) `=ReentrantKeil($INSTANCE_NAME . "_WriteCompare")`
{
   `$CySetRegReplacementString`(`$INSTANCE_NAME`_COMPARE1_LSB_PTR, compare);
   #if (`$INSTANCE_NAME`_PWMMode == `$INSTANCE_NAME`__B_PWM__DITHER)
        `$CySetRegReplacementString`(`$INSTANCE_NAME`_COMPARE2_LSB_PTR, compare+1);
   #endif
}


#else


/*******************************************************************************
* Function Name: `$INSTANCE_NAME`_WriteCompare1
********************************************************************************
* 
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
* Reentrant:
*  Yes
*
*******************************************************************************/
void `$INSTANCE_NAME`_WriteCompare1(`$RegSizeReplacementString` compare) `=ReentrantKeil($INSTANCE_NAME . "_WriteCompare1")`
{
    #if(`$INSTANCE_NAME`_UsingFixedFunction)
        CY_SET_REG16(`$INSTANCE_NAME`_COMPARE1_LSB_PTR, (uint16)compare);
    #else
        `$CySetRegReplacementString`(`$INSTANCE_NAME`_COMPARE1_LSB_PTR, compare);
    #endif
}


/*******************************************************************************
* Function Name: `$INSTANCE_NAME`_WriteCompare2
********************************************************************************
* 
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
* Reentrant:
*  Yes
*
*******************************************************************************/
void `$INSTANCE_NAME`_WriteCompare2(`$RegSizeReplacementString` compare) `=ReentrantKeil($INSTANCE_NAME . "_WriteCompare2")`
{
    #if(`$INSTANCE_NAME`_UsingFixedFunction)
        CY_SET_REG16(`$INSTANCE_NAME`_COMPARE2_LSB_PTR, compare);
    #else
        `$CySetRegReplacementString`(`$INSTANCE_NAME`_COMPARE2_LSB_PTR, compare);
    #endif
}
#endif /* UseOneCompareMode */


#if (`$INSTANCE_NAME`_DeadBandUsed)
/*******************************************************************************
* Function Name: `$INSTANCE_NAME`_WriteDeadTime
********************************************************************************
* 
* Summary:
*  This function writes the dead-band counts to the corresponding register
*
* Parameters:  
*  deadtime:  Number of counts for dead time 
*
* Return: 
*  void
*
* Reentrant:
*  Yes
*
*******************************************************************************/
void `$INSTANCE_NAME`_WriteDeadTime(uint8 deadtime) `=ReentrantKeil($INSTANCE_NAME . "_WriteDeadTime")`
{
    /* If using the Dead Band 1-255 mode then just write the register */
    #if(!`$INSTANCE_NAME`_DeadBand2_4)
        CY_SET_REG8(`$INSTANCE_NAME`_DEADBAND_COUNT_PTR, deadtime);
    #else
        /* Otherwise the data has to be masked and offset */        
        /* Clear existing data */
        `$INSTANCE_NAME`_DEADBAND_COUNT &= ~`$INSTANCE_NAME`_DEADBAND_COUNT_MASK; 
	    /* Set new dead time */
        `$INSTANCE_NAME`_DEADBAND_COUNT |= (deadtime << `$INSTANCE_NAME`_DEADBAND_COUNT_SHIFT) & `$INSTANCE_NAME`_DEADBAND_COUNT_MASK; 
    #endif
}


/*******************************************************************************
* Function Name: `$INSTANCE_NAME`_ReadDeadTime
********************************************************************************
* 
* Summary:
*  This function reads the dead-band counts from the corresponding register
*
* Parameters:  
*  void
*
* Return: 
*  Dead Band Counts
*
* Reentrant:
*  Yes
*
*******************************************************************************/
uint8 `$INSTANCE_NAME`_ReadDeadTime(void) `=ReentrantKeil($INSTANCE_NAME . "_ReadDeadTime")`
{
    /* If using the Dead Band 1-255 mode then just read the register */
    #if(!`$INSTANCE_NAME`_DeadBand2_4)
        return (CY_GET_REG8(`$INSTANCE_NAME`_DEADBAND_COUNT_PTR));
    #else
        /* Otherwise the data has to be masked and offset */
        return ((`$INSTANCE_NAME`_DEADBAND_COUNT & `$INSTANCE_NAME`_DEADBAND_COUNT_MASK) >> `$INSTANCE_NAME`_DEADBAND_COUNT_SHIFT);
    #endif
}
#endif /* DeadBandUsed */

