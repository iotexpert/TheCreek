/*******************************************************************************
* File Name: `$INSTANCE_NAME`.c  
* Version `$CY_MAJOR_VERSION`.`$CY_MINOR_VERSION`
*
*  Description:
*     The Counter User Module consists of a 8, 16, 24 or 32-bit counter with
*     a selectable period between 2 and 2^Width - 1.  
*
*   Note:
*     None
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
*  Enables the counter for operation 
*
* Parameters:  
*  void:  
*
* Return: 
*  (void)
*
* Theory: 
*
* Side Effects: If the Enable mode is set to Hardware only then this function
*               has no effect on the operation of the counter.
*
*******************************************************************************/
void `$INSTANCE_NAME`_Start(void) 
{
	if(`$INSTANCE_NAME`_initvar == 0)
	{
        #if (!`$INSTANCE_NAME`_UsingFixedFunction && !`$INSTANCE_NAME`_ControlRegRemoved)
            uint8 ctrl;
        #endif
        
		`$INSTANCE_NAME`_initvar = 1; /* Clear this bit for Initialization */
        
        #if (`$INSTANCE_NAME`_UsingFixedFunction)
            /* Clear all bits but the enable bit (if it's already set for Timer operation */
            `$INSTANCE_NAME`_CONTROL &= `$INSTANCE_NAME`_CTRL_ENABLE;
            
            /* Clear the mode bits to be 000 for continuous mode */
            `$INSTANCE_NAME`_CONTROL2 &= ~`$INSTANCE_NAME`_CTRL_CMPMODE_MASK; 
            /* Compare Mode is not available with Fixed Function block so don't set the compare mode */
            
            /* Set the IRQ to use the status register interrupts */
            `$INSTANCE_NAME`_CONTROL2 |= `$INSTANCE_NAME`_CTRL2_IRQ_SEL;
        #else
            #if(!`$INSTANCE_NAME`_ControlRegRemoved)
    		/* Set the default compare mode defined in the parameter */
    		ctrl = `$INSTANCE_NAME`_CONTROL & ~`$INSTANCE_NAME`_CTRL_CMPMODE_MASK;
    		`$INSTANCE_NAME`_CONTROL = ctrl | `$INSTANCE_NAME`_DEFAULT_COMPARE_MODE;
            
    		/* Set the default capture mode defined in the parameter */
    		ctrl = `$INSTANCE_NAME`_CONTROL & ~`$INSTANCE_NAME`_CTRL_CAPMODE_MASK;
    		`$INSTANCE_NAME`_CONTROL = ctrl | `$INSTANCE_NAME`_DEFAULT_CAPTURE_MODE;
            #endif
        #endif 
        
        /* Clear all data in the FIFO's */
        #if (!`$INSTANCE_NAME`_UsingFixedFunction)
            `$INSTANCE_NAME`_ClearFIFO();
        #endif
        
         /* Set Initial values from Configuration */
		`$INSTANCE_NAME`_WritePeriod(`$INSTANCE_NAME`_INIT_PERIOD_VALUE);
        `$INSTANCE_NAME`_WriteCounter(`$INSTANCE_NAME`_INIT_COUNTER_VALUE);
		`$INSTANCE_NAME`_SetInterruptMode(`$INSTANCE_NAME`_INIT_INTERRUPTS_MASK);
        
        #if (`$INSTANCE_NAME`_UsingFixedFunction)
			/* Globally Enable the Fixed Function Block chosen */
			`$INSTANCE_NAME`_GLOBAL_ENABLE |= `$INSTANCE_NAME`_BLOCK_EN_MASK;
            /* Set the Interrupt source to come from the status register */
            `$INSTANCE_NAME`_CONTROL2 |= `$INSTANCE_NAME`_CTRL2_IRQ_SEL;
        #else
            /* Set the compare value (only available to non-fixed function implementation */
            `$INSTANCE_NAME`_WriteCompare(`$INSTANCE_NAME`_INIT_COMPARE_VALUE);
            /* Use the interrupt output of the status register for IRQ output */
            `$INSTANCE_NAME`_STATUS_AUX_CTRL |= `$INSTANCE_NAME`_STATUS_ACTL_INT_EN_MASK;
		#endif

	}
    
    /* Enable the counter from the control register  */
    /* If Fixed Function then make sure Mode is set correctly */
    /* else make sure reset is clear */
    #if(!`$INSTANCE_NAME`_ControlRegRemoved || `$INSTANCE_NAME`_UsingFixedFunction)
   `$INSTANCE_NAME`_CONTROL |= `$INSTANCE_NAME`_CTRL_ENABLE;
   #endif
}

/*******************************************************************************
* Function Name: `$INSTANCE_NAME`_Stop
********************************************************************************
* Summary:
* Halts the counter, but does not change any modes or disable
* interrupts.
*
* Parameters:  
*  void:  
*
* Return: 
*  (void)
*
* Theory: 
*
* Side Effects: If the Enable mode is set to Hardware only then this function
*               has no effect on the operation of the counter.
*
*******************************************************************************/
void `$INSTANCE_NAME`_Stop(void)
{
    #if(!`$INSTANCE_NAME`_ControlRegRemoved || `$INSTANCE_NAME`_UsingFixedFunction)
        `$INSTANCE_NAME`_CONTROL &= ~`$INSTANCE_NAME`_CTRL_ENABLE;
    #endif
}

/*******************************************************************************
* Function Name: `$INSTANCE_NAME`_SetInterruptMode
********************************************************************************
* Summary:
* Configures which interrupt sources are enabled to generate the final interrupt
*
* Parameters:  
*  InterruptsMask: This parameter is an or'd collection of the status bits
*                   which will be allowed to generate the counters interrupt.   
*
* Return: 
*  (void)
*
* Theory: 
*
* Side Effects:
*
*******************************************************************************/
void `$INSTANCE_NAME`_SetInterruptMode(uint8 interruptsMask)
{
    `$INSTANCE_NAME`_STATUS_MASK = interruptsMask;
}

/*******************************************************************************
* Function Name: `$INSTANCE_NAME`_GetInterruptSource
********************************************************************************
* Summary:
* Returns the status register with data about the interrupt source.
*
* Parameters:  
*  void:  
*
* Return: 
*  (uint8): Status Register Bit-Field of interrupt source(s)
*
* Theory: 
*
* Side Effects:  The Status register may be clear on read and all interrupt sources
*  				  must be handled.
*
*******************************************************************************/
uint8 `$INSTANCE_NAME`_GetInterruptSource(void)
{
	return `$INSTANCE_NAME`_STATUS;
}

/*******************************************************************************
* Function Name: `$INSTANCE_NAME`_ReadStatusRegister
********************************************************************************
* Summary:
*   Reads the status register and returns it's state. This function should use
*       defined types for the bit-field information as the bits in this
*       register may be permuteable.
*
* Parameters:  
*  void
*
* Return: 
*  (uint8) The contents of the status register
*
* Theory: 
*
* Side Effects:
*   Status register bits may be clear on read. 
*******************************************************************************/
uint8   `$INSTANCE_NAME`_ReadStatusRegister(void)
{
    return `$INSTANCE_NAME`_STATUS;
}

#if(!`$INSTANCE_NAME`_ControlRegRemoved)
/*******************************************************************************
* Function Name: `$INSTANCE_NAME`_ReadControlRegister
********************************************************************************
* Summary:
*   Reads the control register and returns it's state. This function should use
*       defined types for the bit-field information as the bits in this
*       register may be permuteable.
*
* Parameters:  
*  void
*
* Return: 
*  (uint8) The contents of the control register
*
* Theory: 
*
* Side Effects:
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
*   Sets the bit-field of the control register.  This function should use
*       defined types for the bit-field information as the bits in this
*       register may be permuteable.
*
* Parameters:  
*  void
*
* Return: 
*  (uint8) The contents of the control register
*
* Theory: 
*
* Side Effects:
*   
*******************************************************************************/
void    `$INSTANCE_NAME`_WriteControlRegister(uint8 control)
{
    `$INSTANCE_NAME`_CONTROL = control;
}
#endif

/*******************************************************************************
* Function Name: `$INSTANCE_NAME`_WriteCounter
********************************************************************************
* Summary:
*   This funtion is used to set the counter to a specific value
*
* Parameters:  
*  counter:  New counter value. 
*
* Return: 
*  (void) 
*
* Theory: 
*
* Side Effects:
*   
*******************************************************************************/
void `$INSTANCE_NAME`_WriteCounter(`$RegSizeReplacementString` counter )
{
    #if(`$INSTANCE_NAME`_UsingFixedFunction)
    	uint16 counter_temp = (uint16)counter;
    	CY_SET_REG16(`$INSTANCE_NAME`_COUNTER_LSB_PTR, counter_temp);
    #else
    	`$CySetRegReplacementString`(`$INSTANCE_NAME`_COUNTER_LSB_PTR, counter);
    #endif
}

/*******************************************************************************
* Function Name: `$INSTANCE_NAME`_ReadCounter
********************************************************************************
* Summary:
* Returns the current value of the counter.  It doesn't matter
* if the counter is enabled or running.
*
* Parameters:  
*  void:  
*
* Return: 
*  (`$RegSizeReplacementString`) The present value of the counter.
*
* Theory: 
*
* Side Effects:
*
*******************************************************************************/
`$RegSizeReplacementString` `$INSTANCE_NAME`_ReadCounter(void)
{
    //#if (!`$INSTANCE_NAME`_UsingFixedFunction)
    //    /* If the FIFO is empty then read from the counter into the FIFO */
    //    if((`$INSTANCE_NAME`_STATUS && `$INSTANCE_NAME`_STATUS_FIFONEMP) == 0)
    //		/* Datapath is setup to copy from Counter to FIFO on a read of the counter */
    //		`$CyGetRegReplacementString`(`$INSTANCE_NAME`_COUNTER_LSB_PTR);
    //#endif
	//
	/* FIFO need to be cleared before software cature. */
	
	#if (!`$INSTANCE_NAME`_UsingFixedFunction)
	   `$INSTANCE_NAME`_ClearFIFO();
	#endif
	
	/* Read the data from the FIFO (or capture register for Fixed Function)*/
    `$CyGetRegReplacementString`(`$INSTANCE_NAME`_COUNTER_LSB_PTR);
	return (`$CyGetRegReplacementString`(`$INSTANCE_NAME`_STATICCOUNT_LSB_PTR));
}

/*******************************************************************************
* Function Name: `$INSTANCE_NAME`_ReadCapture
********************************************************************************
* Summary:
*   This function returns the last value captured.
*
* Parameters:  
*  void: 
*
* Return: 
*  (`$RegSizeReplacementString`) Present Capture value.
*
* Theory: 
*
* Side Effects:
*  
*******************************************************************************/
`$RegSizeReplacementString` `$INSTANCE_NAME`_ReadCapture( void )
{
   return ( `$CyGetRegReplacementString`(`$INSTANCE_NAME`_STATICCOUNT_LSB_PTR) );  
}

/*******************************************************************************
* Function Name: `$INSTANCE_NAME`_WritePeriod
********************************************************************************
* Summary:
* Changes the period of the counter.  The new period 
* will be loaded the next time terminal count is detected.
*
* Parameters:  
*  period: (`$RegSizeReplacementString`) A value of 0 will result in
*         the counter remaining at zero.  
*
* Return: 
*  (void)
*
* Theory: 
*
* Side Effects:
*
*******************************************************************************/
void `$INSTANCE_NAME`_WritePeriod(`$RegSizeReplacementString` period)
{
    #if(`$INSTANCE_NAME`_UsingFixedFunction)
        uint16 period_temp = (uint16)period;
        CY_SET_REG16(`$INSTANCE_NAME`_PERIOD_LSB_PTR,period_temp);
    #else
        `$CySetRegReplacementString`(`$INSTANCE_NAME`_PERIOD_LSB_PTR,period);
    #endif
}

/*******************************************************************************
* Function Name: `$INSTANCE_NAME`_ReadPeriod
********************************************************************************
* Summary:
* Reads the current period value without affecting counter operation.
*
* Parameters:  
*  void:  
*
* Return: 
*  (`$RegSizeReplacementString`) Present period value.
*
* Theory: 
*
* Side Effects:
*
*******************************************************************************/
`$RegSizeReplacementString` `$INSTANCE_NAME`_ReadPeriod(void)
{
   return ( `$CyGetRegReplacementString`(`$INSTANCE_NAME`_PERIOD_LSB_PTR));
}

#if (!`$INSTANCE_NAME`_UsingFixedFunction)
/*******************************************************************************
* Function Name: `$INSTANCE_NAME`_WriteCompare
********************************************************************************
* Summary:
* Changes the compare value.  The compare output will 
* reflect the new value on the next UDB clock.  The compare output will be 
* driven high when the present counter value compares true based on the 
* configured compare mode setting. 
*
* Parameters:  
*  Compare:  New compare value. 
*
* Return: 
*  void
*
* Theory: 
*
* Side Effects:
*
*******************************************************************************/
void `$INSTANCE_NAME`_WriteCompare(`$RegSizeReplacementString` compare)
{
    #if(`$INSTANCE_NAME`_UsingFixedFunction)
        uint16 compare_temp = (uint16)compare;
        CY_SET_REG16(`$INSTANCE_NAME`_COMPARE_LSB_PTR,compare_temp);
    #else
        `$CySetRegReplacementString`(`$INSTANCE_NAME`_COMPARE_LSB_PTR,compare);
    #endif
}

/*******************************************************************************
* Function Name: `$INSTANCE_NAME`_ReadCompare
********************************************************************************
* Summary:
* Returns the compare value.
*
* Parameters:  
*  void:
*
* Return: 
*  (`$RegSizeReplacementString`) Present compare value.
*
* Theory: 
*
* Side Effects:
*
*******************************************************************************/
`$RegSizeReplacementString` `$INSTANCE_NAME`_ReadCompare(void)
{
   return ( `$CyGetRegReplacementString`(`$INSTANCE_NAME`_COMPARE_LSB_PTR));
}

#if (`$INSTANCE_NAME`_COMPARE_MODE_SOFTWARE)
/*******************************************************************************
* Function Name: `$INSTANCE_NAME`_SetCompareMode
********************************************************************************
* Summary:
* Sets the software controlled Compare Mode
*
* Parameters:  
*  uint8: Compare Mode Enumerated Type
*
* Return: 
*  None
*
* Theory: 
*
* Side Effects:
*
*******************************************************************************/
void    `$INSTANCE_NAME`_SetCompareMode(uint8 comparemode)
{
    `$INSTANCE_NAME`_CONTROL |= ((comparemode << `$INSTANCE_NAME`_CTRL_CMPMODE0_SHIFT) & `$INSTANCE_NAME`_CTRL_CMPMODE_MASK);
}
#endif

#if (`$INSTANCE_NAME`_CAPTURE_MODE_SOFTWARE)
/*******************************************************************************
* Function Name: `$INSTANCE_NAME`_SetCaptureMode
********************************************************************************
* Summary:
* Sets the software controlled Capture Mode
*
* Parameters:  
*  uint8: Capture Mode Enumerated Type
*
* Return: 
*  None
*
* Theory: 
*
* Side Effects:
*
*******************************************************************************/
void    `$INSTANCE_NAME`_SetCaptureMode(uint8 capturemode)
{
    `$INSTANCE_NAME`_CONTROL |= ((capturemode << `$INSTANCE_NAME`_CTRL_CAPMODE0_SHIFT) & `$INSTANCE_NAME`_CTRL_CAPMODE_MASK);
}
#endif

/*******************************************************************************
* Function Name: `$INSTANCE_NAME`_ClearFIFO
********************************************************************************
* Summary:
*   This function clears all capture data from the capture FIFO
*
* Parameters:  
*  void:
*
* Return: 
*  None
*
* Theory: 
*
* Side Effects:
*   
*******************************************************************************/
void `$INSTANCE_NAME`_ClearFIFO(void)
{
    while(`$INSTANCE_NAME`_ReadStatusRegister() & `$INSTANCE_NAME`_STATUS_FIFONEMP)
        `$INSTANCE_NAME`_ReadCapture();
}
#endif


/* [] END OF FILE */
