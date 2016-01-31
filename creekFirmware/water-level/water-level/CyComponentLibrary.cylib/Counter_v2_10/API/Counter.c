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
* Copyright 2008-2011, Cypress Semiconductor Corporation.  All rights reserved.
* You may use this file only in accordance with the license, terms, conditions, 
* disclaimers, and limitations in the end user license agreement accompanying 
* the software package with which this file was provided.
********************************************************************************/

#include "`$INSTANCE_NAME`.h"

uint8 `$INSTANCE_NAME`_initVar = 0u;


/*******************************************************************************
* Function Name: `$INSTANCE_NAME`_Init
********************************************************************************
* Summary:
*     Initialize to the schematic state
* 
* Parameters:  
*  void  
*
* Return: 
*  void
*
* Reentrant
*  Yes
*
*******************************************************************************/
void `$INSTANCE_NAME`_Init(void) `=ReentrantKeil($INSTANCE_NAME . "_Init")`
{
        #if (!`$INSTANCE_NAME`_UsingFixedFunction && !`$INSTANCE_NAME`_ControlRegRemoved)
            uint8 ctrl;
        #endif
        
        #if(!`$INSTANCE_NAME`_UsingFixedFunction) 
            /* Interrupt State Backup for Critical Region*/
            uint8 `$INSTANCE_NAME`_interruptState;
        #endif
        
        #if (`$INSTANCE_NAME`_UsingFixedFunction)
            /* Clear all bits but the enable bit (if it's already set for Timer operation */
            `$INSTANCE_NAME`_CONTROL &= `$INSTANCE_NAME`_CTRL_ENABLE;
            
            /* Clear the mode bits for continuous run mode */
            #if (`$INSTANCE_NAME`_PSOC3_ES2 || `$INSTANCE_NAME`_PSOC5_ES1)
                `$INSTANCE_NAME`_CONTROL2 &= ~`$INSTANCE_NAME`_CTRL_MODE_MASK;
            #endif
            #if (`$INSTANCE_NAME`_PSOC3_ES3 || `$INSTANCE_NAME`_PSOC5_ES2)
                `$INSTANCE_NAME`_CONTROL3 &= ~`$INSTANCE_NAME`_CTRL_MODE_MASK;                
            #endif
            /* Check if One Shot mode is enabled i.e. RunMode !=0*/
            #if (`$INSTANCE_NAME`_RunModeUsed != 0x0u)
                /* Set 3rd bit of Control register to enable one shot mode */
                `$INSTANCE_NAME`_CONTROL |= `$INSTANCE_NAME`_ONESHOT;
            #endif
            
            /* Set the IRQ to use the status register interrupts */
            `$INSTANCE_NAME`_CONTROL2 |= `$INSTANCE_NAME`_CTRL2_IRQ_SEL;
            
            /* Clear and Set SYNCTC and SYNCCMP bits of RT1 register */
            `$INSTANCE_NAME`_RT1 &= ~`$INSTANCE_NAME`_RT1_MASK;
            `$INSTANCE_NAME`_RT1 |= `$INSTANCE_NAME`_SYNC;     
                    
            /*Enable DSI Sync all all inputs of the Timer*/
            `$INSTANCE_NAME`_RT1 &= ~(`$INSTANCE_NAME`_SYNCDSI_MASK);
            `$INSTANCE_NAME`_RT1 |= `$INSTANCE_NAME`_SYNCDSI_EN;

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
        
        #if (!`$INSTANCE_NAME`_UsingFixedFunction)
            /* Read the status register to clear the unwanted interrupts */
            `$INSTANCE_NAME`_ReadStatusRegister();
            /* Set the compare value (only available to non-fixed function implementation */
            `$INSTANCE_NAME`_WriteCompare(`$INSTANCE_NAME`_INIT_COMPARE_VALUE);
            /* Use the interrupt output of the status register for IRQ output */
            
            /* CyEnterCriticalRegion and CyExitCriticalRegion are used to mark following region critical*/
            /* Enter Critical Region*/
            `$INSTANCE_NAME`_interruptState = CyEnterCriticalSection();
            
            `$INSTANCE_NAME`_STATUS_AUX_CTRL |= `$INSTANCE_NAME`_STATUS_ACTL_INT_EN_MASK;
            
            /* Exit Critical Region*/
            CyExitCriticalSection(`$INSTANCE_NAME`_interruptState);
            
        #endif
}


/*******************************************************************************
* Function Name: `$INSTANCE_NAME`_Enable
********************************************************************************
* Summary:
*     Enable the Counter
* 
* Parameters:  
*  void  
*
* Return: 
*  void
*
* Side Effects: 
*   If the Enable mode is set to Hardware only then this function has no effect 
*   on the operation of the counter.
*
* Reentrant
*  Yes
*
*******************************************************************************/
void `$INSTANCE_NAME`_Enable(void) `=ReentrantKeil($INSTANCE_NAME . "_Enable")`
{
    /* Globally Enable the Fixed Function Block chosen */
    #if (`$INSTANCE_NAME`_UsingFixedFunction)
        `$INSTANCE_NAME`_GLOBAL_ENABLE |= `$INSTANCE_NAME`_BLOCK_EN_MASK;
        `$INSTANCE_NAME`_GLOBAL_STBY_ENABLE |= `$INSTANCE_NAME`_BLOCK_STBY_EN_MASK;
    #endif   
        
    /* Enable the counter from the control register  */
    /* If Fixed Function then make sure Mode is set correctly */
    /* else make sure reset is clear */
    #if(!`$INSTANCE_NAME`_ControlRegRemoved || `$INSTANCE_NAME`_UsingFixedFunction)
        `$INSTANCE_NAME`_CONTROL |= `$INSTANCE_NAME`_CTRL_ENABLE;                
    #endif
    
}


/*******************************************************************************
* Function Name: `$INSTANCE_NAME`_Start
********************************************************************************
* Summary:
*  Enables the counter for operation 
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
* Reentrant
*  No
*
*******************************************************************************/
void `$INSTANCE_NAME`_Start(void) 
{
    if(`$INSTANCE_NAME`_initVar == 0u)
    {
        `$INSTANCE_NAME`_Init();
        
        `$INSTANCE_NAME`_initVar = 1u; /* Clear this bit for Initialization */        
    }
    
    /* Enable the Counter */
    `$INSTANCE_NAME`_Enable();        
}


/*******************************************************************************
* Function Name: `$INSTANCE_NAME`_Stop
********************************************************************************
* Summary:
* Halts the counter, but does not change any modes or disable interrupts.
*
* Parameters:  
*  void  
*
* Return: 
*  void
*
* Side Effects: If the Enable mode is set to Hardware only then this function
*               has no effect on the operation of the counter.
*
* Reentrant
*  Yes
*
*******************************************************************************/
void `$INSTANCE_NAME`_Stop(void) `=ReentrantKeil($INSTANCE_NAME . "_Stop")`
{
    /* Disable Counter */
    #if(!`$INSTANCE_NAME`_ControlRegRemoved || `$INSTANCE_NAME`_UsingFixedFunction)
        `$INSTANCE_NAME`_CONTROL &= ~`$INSTANCE_NAME`_CTRL_ENABLE;        
    #endif
    
    /* Globally disable the Fixed Function Block chosen */
    #if (`$INSTANCE_NAME`_UsingFixedFunction)
        `$INSTANCE_NAME`_GLOBAL_ENABLE &= ~`$INSTANCE_NAME`_BLOCK_EN_MASK;
        `$INSTANCE_NAME`_GLOBAL_STBY_ENABLE &= ~`$INSTANCE_NAME`_BLOCK_STBY_EN_MASK;
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
*  void
*
* Reentrant
*  Yes
*
*******************************************************************************/
void `$INSTANCE_NAME`_SetInterruptMode(uint8 interruptsMask) `=ReentrantKeil($INSTANCE_NAME . "_SetInterruptMode")`
{
    `$INSTANCE_NAME`_STATUS_MASK = interruptsMask;
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
* Side Effects:
*   Status register bits may be clear on read. 
*
* Reentrant
*  Yes
*
*******************************************************************************/
uint8   `$INSTANCE_NAME`_ReadStatusRegister(void) `=ReentrantKeil($INSTANCE_NAME . "_ReadStatusRegister")`
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
* Reentrant
*  Yes
*
*******************************************************************************/
uint8   `$INSTANCE_NAME`_ReadControlRegister(void) `=ReentrantKeil($INSTANCE_NAME . "_ReadControlRegister")`
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
* Reentrant
*  Yes
*
*******************************************************************************/
void    `$INSTANCE_NAME`_WriteControlRegister(uint8 control) `=ReentrantKeil($INSTANCE_NAME . "_WriteControlRegister")`
{
    `$INSTANCE_NAME`_CONTROL = control;
}

#endif  /* (!`$INSTANCE_NAME`_ControlRegRemoved) */


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
*  void 
*
* Reentrant
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
* Reentrant
*  Yes
*
*******************************************************************************/
`$RegSizeReplacementString` `$INSTANCE_NAME`_ReadCounter(void) `=ReentrantKeil($INSTANCE_NAME . "_ReadCounter")`
{
    /* Force capture by reading Accumulator */
    /* Must first do a software capture to be able to read the counter */
    /* It is up to the user code to make sure there isn't already captured data in the FIFO */
    CY_GET_REG8(`$INSTANCE_NAME`_COUNTER_LSB_PTR);
    
    /* Read the data from the FIFO (or capture register for Fixed Function)*/
    return (`$CyGetRegReplacementString`(`$INSTANCE_NAME`_STATICCOUNT_LSB_PTR));
}


/*******************************************************************************
* Function Name: `$INSTANCE_NAME`_ReadCapture
********************************************************************************
* Summary:
*   This function returns the last value captured.
*
* Parameters:  
*  void
*
* Return: 
*  (`$RegSizeReplacementString`) Present Capture value.
*
* Reentrant
*  Yes
*
*******************************************************************************/
`$RegSizeReplacementString` `$INSTANCE_NAME`_ReadCapture(void) `=ReentrantKeil($INSTANCE_NAME . "_ReadCapture")`
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
*  void
*
* Reentrant
*  Yes
*
*******************************************************************************/
void `$INSTANCE_NAME`_WritePeriod(`$RegSizeReplacementString` period) `=ReentrantKeil($INSTANCE_NAME . "_WritePeriod")`
{
    #if(`$INSTANCE_NAME`_UsingFixedFunction)
        CY_SET_REG16(`$INSTANCE_NAME`_PERIOD_LSB_PTR,(uint16)period);
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
* Reentrant
*  Yes
*
*******************************************************************************/
`$RegSizeReplacementString` `$INSTANCE_NAME`_ReadPeriod(void) `=ReentrantKeil($INSTANCE_NAME . "_ReadPeriod")`
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
* Reentrant
*  Yes
*
*******************************************************************************/
void `$INSTANCE_NAME`_WriteCompare(`$RegSizeReplacementString` compare) `=ReentrantKeil($INSTANCE_NAME . "_WriteCompare")`
{
    #if(`$INSTANCE_NAME`_UsingFixedFunction)
        CY_SET_REG16(`$INSTANCE_NAME`_COMPARE_LSB_PTR,(uint16)compare);
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
* Reentrant
*  Yes
*
*******************************************************************************/
`$RegSizeReplacementString` `$INSTANCE_NAME`_ReadCompare(void) `=ReentrantKeil($INSTANCE_NAME . "_ReadCompare")`
{
   return ( `$CyGetRegReplacementString`(`$INSTANCE_NAME`_COMPARE_LSB_PTR));
}


#if (`$INSTANCE_NAME`_COMPARE_MODE_SOFTWARE)
/*******************************************************************************
* Function Name: `$INSTANCE_NAME`_SetCompareMode
********************************************************************************
* Summary:
*  Sets the software controlled Compare Mode.
*
* Parameters:
*  compareMode:  Compare Mode Enumerated Type.
*
* Return:
*  void
*
* Reentrant
*  Yes
*
*******************************************************************************/
void `$INSTANCE_NAME`_SetCompareMode(uint8 compareMode) `=ReentrantKeil($INSTANCE_NAME . "_SetCompareMode")`
{
    /* Clear the compare mode bits in the control register */
    `$INSTANCE_NAME`_CONTROL &= ~`$INSTANCE_NAME`_CTRL_CMPMODE_MASK;
    
    /* Write the new setting */
    `$INSTANCE_NAME`_CONTROL |= (compareMode << `$INSTANCE_NAME`_CTRL_CMPMODE0_SHIFT);
}
#endif  /* (`$INSTANCE_NAME`_COMPARE_MODE_SOFTWARE) */


#if (`$INSTANCE_NAME`_CAPTURE_MODE_SOFTWARE)
/*******************************************************************************
* Function Name: `$INSTANCE_NAME`_SetCaptureMode
********************************************************************************
* Summary:
*  Sets the software controlled Capture Mode.
*
* Parameters:
*  captureMode:  Capture Mode Enumerated Type.
*
* Return:
*  void
*
* Reentrant
*  Yes
*
*******************************************************************************/
void `$INSTANCE_NAME`_SetCaptureMode(uint8 captureMode) `=ReentrantKeil($INSTANCE_NAME . "_SetCaptureMode")`
{
    /* Clear the capture mode bits in the control register */
    `$INSTANCE_NAME`_CONTROL &= ~`$INSTANCE_NAME`_CTRL_CAPMODE_MASK;
    
    /* Write the new setting */
    `$INSTANCE_NAME`_CONTROL |= (captureMode << `$INSTANCE_NAME`_CTRL_CAPMODE0_SHIFT);
}
#endif  /* (`$INSTANCE_NAME`_CAPTURE_MODE_SOFTWARE) */


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
* Reentrant
*  Yes
*
*******************************************************************************/
void `$INSTANCE_NAME`_ClearFIFO(void) `=ReentrantKeil($INSTANCE_NAME . "_ClearFIFO")`
{

    while(`$INSTANCE_NAME`_ReadStatusRegister() & `$INSTANCE_NAME`_STATUS_FIFONEMP)
    {
        `$INSTANCE_NAME`_ReadCapture();
    }

}
#endif  /* (!`$INSTANCE_NAME`_UsingFixedFunction) */


/* [] END OF FILE */

