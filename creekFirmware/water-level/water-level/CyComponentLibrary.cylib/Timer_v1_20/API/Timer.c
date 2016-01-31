/*******************************************************************************
* File Name: `$INSTANCE_NAME`.c  
* Version `$CY_MAJOR_VERSION`.`$CY_MINOR_VERSION`
*
* Description:
*  The Timer User Module consists of a 8, 16, 24 or 32-bit timer with
*  a selectable period between 2 and 2^Width - 1.  The timer may free run
*  or be used as a capture timer as well.  The capture can be initiated
*  by a positive or negative edge signal as well as via software.
*  A trigger input can be programmed to enable the timer on rising edge
*  falling edge, either edge or continous run.
*  Interrupts may be generated due to a terminal count condition
*  or a capture event.
*
* Note:
*
********************************************************************************
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
*  The start function initializes the timer with the default values, the 
*  enables the timerto begin counting.  It does not enable interrupts,
*  the EnableInt command should be called if interrupt generation is required.
*
* Parameters:  
*  void:  
*
* Return: 
*  void
*
*******************************************************************************/
void `$INSTANCE_NAME`_Start(void) 
{
    if(`$INSTANCE_NAME`_initvar == 0)
    {
        `$INSTANCE_NAME`_initvar = 1;   /* Clear this bit for Initialization */
        
        #if (`$INSTANCE_NAME`_UsingFixedFunction)
            #if !defined(`$INSTANCE_NAME`_TimerUDB_ctrlreg__REMOVED)  /* Remove assignment if control register is removed */
                /* Clear all bits but the enable bit (if it's already set) for Timer operation */
                `$INSTANCE_NAME`_CONTROL &= `$INSTANCE_NAME`_CTRL_ENABLE;
            #endif 
            /* Clear the mode bits to be 000 for continuous mode */
            `$INSTANCE_NAME`_CONTROL2 &= ~`$INSTANCE_NAME`_CTRL_MODE_MASK; 
            
            /* Check if One Shot mode is enabled i.e. RunMode !=0*/
            #if (`$INSTANCE_NAME`_RunModeUsed != 0)
                /* Set 3rd bit of Control register to enable one shot mode */
                `$INSTANCE_NAME`_CONTROL |= 0x04u;
            #endif
            #if (`$INSTANCE_NAME`_RunModeUsed == 2)
                /* Set last 2 bits of control2 register if one shot(halt on 
                interrupt) is enabled*/
                `$INSTANCE_NAME`_CONTROL2 |= 0x03u;
            #endif
            
            /* If the Enable HW controllable, then set bit one of CONTROL2 */
            #if (`$INSTANCE_NAME`_UsingHWEnable != 0)
                `$INSTANCE_NAME`_CONTROL2 |= `$INSTANCE_NAME`_CTRL_MODE_PULSEWIDTH; 
            #endif
            
            /* Set the IRQ to use the status register interrupts */
            `$INSTANCE_NAME`_CONTROL2 |= `$INSTANCE_NAME`_CTRL2_IRQ_SEL;
        #endif 
        
        /* Set Initial values from Configuration */
        `$INSTANCE_NAME`_WritePeriod(`$INSTANCE_NAME`_INIT_PERIOD);
        `$INSTANCE_NAME`_WriteCounter(`$INSTANCE_NAME`_INIT_PERIOD);
        
        #if (`$INSTANCE_NAME`_UsingHWCaptureCounter)/* Capture counter is enabled */
            `$INSTANCE_NAME`_CAPTURE_COUNT_CTRL |= `$INSTANCE_NAME`_CNTR_ENABLE;
            `$INSTANCE_NAME`_SetCaptureCount(`$INSTANCE_NAME`_INIT_CAPTURE_COUNT);
        #endif
            
        #if (!`$INSTANCE_NAME`_UsingFixedFunction)
            /* Initialize the Configuration bits of the Control Register */
            #if (`$INSTANCE_NAME`_SoftwareCaptureMode)
                `$INSTANCE_NAME`_SetCaptureMode(`$INSTANCE_NAME`_INIT_CAPTURE_MODE);
            #endif
            
            #if (`$INSTANCE_NAME`_SoftwareTriggerMode)
                if (!(`$INSTANCE_NAME`_CONTROL & `$INSTANCE_NAME`__B_TIMER__TM_SOFTWARE))
                {
                    `$INSTANCE_NAME`_SetTriggerMode(`$INSTANCE_NAME`_INIT_TRIGGER_MODE);
                }
            #endif
            
                /* Use the interrupt output of the status register for IRQ output */
            `$INSTANCE_NAME`_STATUS_AUX_CTRL |= `$INSTANCE_NAME`_STATUS_ACTL_INT_EN_MASK;

            #if (`$INSTANCE_NAME`_EnableTriggerMode)
                `$INSTANCE_NAME`_EnableTrigger();
            #endif

            #if (`$INSTANCE_NAME`_InterruptOnCaptureCount)
                `$INSTANCE_NAME`_SetInterruptCount(`$INSTANCE_NAME`_INIT_INT_CAPTURE_COUNT);
            #endif
            
            `$INSTANCE_NAME`_ClearFIFO();        

        #else /* (`$INSTANCE_NAME`_UsingFixedFunction) */
            /* Globally Enable the Fixed Function Block chosen */
            `$INSTANCE_NAME`_GLOBAL_ENABLE |= `$INSTANCE_NAME`_BLOCK_EN_MASK;
            /* Set the Interrupt source to come from the status register */
            `$INSTANCE_NAME`_CONTROL2 |= `$INSTANCE_NAME`_CTRL2_IRQ_SEL;
        #endif

        `$INSTANCE_NAME`_SetInterruptMode(`$INSTANCE_NAME`_INIT_INTERRUPT_MODE);
    }
    #if !defined(`$INSTANCE_NAME`_TimerUDB_ctrlreg__REMOVED) /* Remove assignment if control register is removed */
        `$INSTANCE_NAME`_CONTROL |= `$INSTANCE_NAME`_CTRL_ENABLE;
    #endif
}


/*******************************************************************************
* Function Name: `$INSTANCE_NAME`_Stop
********************************************************************************
* Summary:
*  The stop function halts the timer, but does not change any modes or disable
*  interrupts.
*
* Parameters:  
*  void:  
*
* Return: 
*  void
*
*******************************************************************************/
void `$INSTANCE_NAME`_Stop(void)
{
    #if !defined(`$INSTANCE_NAME`_TimerUDB_ctrlreg__REMOVED)   /* Remove assignment if control register is removed */
        `$INSTANCE_NAME`_CONTROL &= ~`$INSTANCE_NAME`_CTRL_ENABLE;
    #endif  
}


/*******************************************************************************
* Function Name: `$INSTANCE_NAME`_SetInterruptMode
********************************************************************************
* Summary:
*  This function selects which of the interrupt inputs may cause an interrupt.  
*  The twosources are caputure and terminal.  One, both or neither may 
*  be selected.
*
* Parameters:  
*  interruptsource: This parameter is used to enable interrups on either/or 
*                   terminal count or capture.  
*
* Return: 
*  void
*
*******************************************************************************/
void `$INSTANCE_NAME`_SetInterruptMode( uint8 interruptMode )
{
    `$INSTANCE_NAME`_STATUS_MASK = interruptMode;
}


/*******************************************************************************
* Function Name: `$INSTANCE_NAME`_GetInterruptSource
********************************************************************************
* Summary:
*  Returns the status register with the information on the interrupt source
*
* Parameters:  
*  void:  
*
* Return: 
*  Status register bit-field containing the interrupt source
*
* Side Effects: 
*  Status register is clear on Read, therefore all interrupt sources must be 
*  handled.
*
*******************************************************************************/
uint8 `$INSTANCE_NAME`_GetInterruptSource()
{
    return `$INSTANCE_NAME`_STATUS;
}


/*******************************************************************************
* Function Name: `$INSTANCE_NAME`_SoftwareCapture
********************************************************************************
* Summary:
*  This function forces a capture independent of the capture signal.
*
* Parameters:  
*  void
*
* Return: 
*  void
*
* Side Effects:
*  An existing hardware capture could be overwritten.
*
*******************************************************************************/
void `$INSTANCE_NAME`_SoftwareCapture(void)
{
    /* Generate a software capture by reading the counter register */
    CY_GET_REG8(`$INSTANCE_NAME`_COUNTER_LSB_PTR);
    /* Capture Data is now in the FIFO */
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
*  Status register bits may be clear on read. 
*
*******************************************************************************/
uint8   `$INSTANCE_NAME`_ReadStatusRegister(void)
{
    return `$INSTANCE_NAME`_STATUS;
}


#if !defined(`$INSTANCE_NAME`_TimerUDB_ctrlreg__REMOVED)   /* Remove API if control register is removed */
/*******************************************************************************
* Function Name: `$INSTANCE_NAME`_ReadControlRegister
********************************************************************************
* Summary:
*  Reads the control register and returns it's value. 
*
* Parameters:  
*  void
*
* Return: 
*  The contents of the control register
*
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
*  Sets the bit-field of the control register.  
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
#endif /* Remove API if control register is removed */


/*******************************************************************************
* Function Name: `$INSTANCE_NAME`_ReadPeriod
********************************************************************************
* Summary:
*  This function returns the current value of the Period.
*
* Parameters:  
*  void:
*
* Return: 
*  The present value of the counter.
*
*******************************************************************************/
`$RegSizeReplacementString` `$INSTANCE_NAME`_ReadPeriod(void)
{
   return ( `$CyGetRegReplacementString`(`$INSTANCE_NAME`_PERIOD_LSB_PTR) );  
}


/*******************************************************************************
* Function Name: `$INSTANCE_NAME`_WritePeriod
********************************************************************************
* Summary:
*  This function is used to change the period of the counter.  The new period 
*  will be loaded the next time terminal count is detected.
*
* Parameters:  
*  period: This value may be between 1 and (2^Resolution)-1.  A value of 0 will result in
*          the counter remaining at zero.
*
* Return: 
*  void
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
* Function Name: `$INSTANCE_NAME`_ReadCapture
********************************************************************************
* Summary:
*  This function returns the last value captured.
*
* Parameters:  
*  void: 
*
* Return: 
*  Present Capture value.
*
*******************************************************************************/
`$RegSizeReplacementString` `$INSTANCE_NAME`_ReadCapture( void )
{
   return ( `$CyGetRegReplacementString`(`$INSTANCE_NAME`_CAPTURE_LSB_PTR) );  
}


/*******************************************************************************
* Function Name: `$INSTANCE_NAME`_WriteCounter
********************************************************************************
* Summary:
*  This funtion is used to set the counter to a specific value
*
* Parameters:  
*  counter:  New counter value. 
*
* Return: 
*  void 
*
*******************************************************************************/
void `$INSTANCE_NAME`_WriteCounter(`$RegSizeReplacementString` counter )
{
    #if(`$INSTANCE_NAME`_UsingFixedFunction)
        counter = counter;
        /* This functionality is removed until a FixedFunction HW update to 
         * allow this register to be written 
         */
        /* uint16 counter_temp = (uint16)counter;
         * CY_SET_REG16(`$INSTANCE_NAME`_COUNTER_LSB_PTR, counter_temp);
         */
    #else
        `$CySetRegReplacementString`(`$INSTANCE_NAME`_COUNTER_LSB_PTR, counter);
    #endif
}


/*******************************************************************************
* Function Name: `$INSTANCE_NAME`_ReadCounter
********************************************************************************
* Summary:
*  This function returns the current counter value.
*
* Parameters:  
*  void:
*
* Return: 
*  Present compare value. 
*
*******************************************************************************/
`$RegSizeReplacementString` `$INSTANCE_NAME`_ReadCounter( void )
{
    /* Read the data from the FIFO (or capture register for Fixed Function)*/
    /* Must first do a software capture to be able to read the counter */
    /* It is up to the user code to make sure there isn't already captured data in the FIFO */
    `$CyGetRegReplacementString`(`$INSTANCE_NAME`_COUNTER_LSB_PTR);
    return (`$CyGetRegReplacementString`(`$INSTANCE_NAME`_CAPTURE_LSB_PTR));
}


#if(!`$INSTANCE_NAME`_UsingFixedFunction) /* UDB Specific Functions */

/*******************************************************************************
 * The functions below this point are only available using the UDB 
 * implementation.  If a feature is selected, then the API is enabled.
 ******************************************************************************/


#if (`$INSTANCE_NAME`_SoftwareCaptureMode)
/*******************************************************************************
* Function Name: `$INSTANCE_NAME`_SetCaptureMode
********************************************************************************
* Summary:
*  This function sets the capture mode to either rising or falling edge.
*
* Parameters:  
*  capturemode:  This parameter sets the capture polarity. If 0, capture will 
*                be on rising edge of caputure input, if non-zero, capture 
*                will occure on falling edge. Use any of these 
*                Enumerated Types Except Software mode:
*                `#cy_declare_enum B_Timer__CaptureModes`
*
* Return: 
*  void
*
*******************************************************************************/
void `$INSTANCE_NAME`_SetCaptureMode( uint8 capturemode )
{
    /* This must only set to two bits of the control register associated */
    capturemode &= `$INSTANCE_NAME`_CTRL_CAP_MODE_MASK;
    
    #if !defined(`$INSTANCE_NAME`_TimerUDB_ctrlreg__REMOVED)   /* Remove assignment if control register is removed */
        /* Clear the Current Setting */
        `$INSTANCE_NAME`_CONTROL &= ~`$INSTANCE_NAME`_CTRL_CAP_MODE_MASK;
    
        /* Write The New Setting */   
        `$INSTANCE_NAME`_CONTROL |= capturemode; 
    #endif
}
#endif

#if (`$INSTANCE_NAME`_SoftwareTriggerMode)
/*******************************************************************************
* Function Name: `$INSTANCE_NAME`_SetTriggerMode
********************************************************************************
* Summary:
*  This function sets the trigger input mode
*
* Parameters:  
*  triggermode: Pass one of the pre-defined Trigger Modes (except Software)
    #define `$INSTANCE_NAME`__B_TIMER__TM_NONE 0x00
    #define `$INSTANCE_NAME`__B_TIMER__TM_RISINGEDGE 0x04
    #define `$INSTANCE_NAME`__B_TIMER__TM_FALLINGEDGE 0x08
    #define `$INSTANCE_NAME`__B_TIMER__TM_EITHEREDGE 0x0C
    #define `$INSTANCE_NAME`__B_TIMER__TM_SOFTWARE 0x10
*
* Return: 
*  void
*
*******************************************************************************/
void `$INSTANCE_NAME`_SetTriggerMode( uint8 triggermode )
{
    /* This must only set to two bits of the control register associated */
    triggermode &= `$INSTANCE_NAME`_CTRL_TRIG_MODE_MASK;
    
    #if !defined(`$INSTANCE_NAME`_TimerUDB_ctrlreg__REMOVED)   /* Remove assignment if control register is removed */
       /* Clear the Current Setting */
       `$INSTANCE_NAME`_CONTROL &= ~`$INSTANCE_NAME`_CTRL_TRIG_MODE_MASK;
       /* Write The New Setting */   
       `$INSTANCE_NAME`_CONTROL |= (triggermode | `$INSTANCE_NAME`__B_TIMER__TM_SOFTWARE);
    #endif 
}
#endif

#if (`$INSTANCE_NAME`_EnableTriggerMode)
/*******************************************************************************
* Function Name: `$INSTANCE_NAME`_EnableTrigger
********************************************************************************
* Summary:
*  Sets the control bit enabling Hardware Trigger mode
*
* Parameters:  
*  void:
*
* Return: 
*  void
*
*******************************************************************************/
void    `$INSTANCE_NAME`_EnableTrigger(void)
{
    #if !defined(`$INSTANCE_NAME`_TimerUDB_ctrlreg__REMOVED)   /* Remove assignment if control register is removed */
        `$INSTANCE_NAME`_CONTROL |= `$INSTANCE_NAME`_CTRL_TRIG_EN;
    #endif
}

/*******************************************************************************
* Function Name: `$INSTANCE_NAME`_DisableTrigger
********************************************************************************
* Summary:
*  Clears the control bit enabling Hardware Trigger mode
*
* Parameters:  
*  void:
*
* Return: 
*  void
*
*******************************************************************************/
void    `$INSTANCE_NAME`_DisableTrigger(void)
{
    #if !defined(`$INSTANCE_NAME`_TimerUDB_ctrlreg__REMOVED)   /* Remove assignment if control register is removed */
        `$INSTANCE_NAME`_CONTROL &= ~`$INSTANCE_NAME`_CTRL_TRIG_EN;
    #endif
}
#endif


#if(`$INSTANCE_NAME`_InterruptOnCaptureCount)
/*******************************************************************************
* Function Name: `$INSTANCE_NAME`_SetInterruptCount
********************************************************************************
* Summary:
*  This function sets the capture count before an interrupt is triggered.
*
* Parameters:  
*  interruptcount:  A value between 0 and 3 is valid.  If the value is 0, then 
*                   an interrupt will occur each time a capture occurs.  
*                   A value of 1 to 3 will cause the interrupt  
*                   to delay by the same number of captures.
*
* Return: 
*  void
*
*******************************************************************************/
void `$INSTANCE_NAME`_SetInterruptCount( uint8 interruptcount )
{
    /* This must only set to two bits of the control register associated */
    interruptcount &= `$INSTANCE_NAME`_CTRL_INTCNT_MASK;
    
    #if !defined(`$INSTANCE_NAME`_TimerUDB_ctrlreg__REMOVED)   /* Remove assignment if control register is removed */
        /* Clear the Current Setting */
        `$INSTANCE_NAME`_CONTROL &= ~`$INSTANCE_NAME`_CTRL_INTCNT_MASK;
        /* Write The New Setting */   
        `$INSTANCE_NAME`_CONTROL |= interruptcount; 
    #endif
}
#endif


#if (`$INSTANCE_NAME`_UsingHWCaptureCounter)
/*******************************************************************************
* Function Name: `$INSTANCE_NAME`_SetCaptureCount
********************************************************************************
* Summary:
*  This function sets the capture count
*
* Parameters:  
*  capturecount: A value between 0 and 3 is valid.  If the value is 0, then an 
*                interrupt will occur each time a capture occurs.  A value of 1 
*                to 3 will cause the interrupt to delay by the same number of 
*                captures.
*
* Return: 
*  void
*
*******************************************************************************/
void    `$INSTANCE_NAME`_SetCaptureCount(uint8 capturecount)
{
    `$INSTANCE_NAME`_CAP_COUNT = capturecount;
}


/*******************************************************************************
* Function Name: `$INSTANCE_NAME`_ReadCaptureCount
********************************************************************************
* Summary:
*  This function reads the capture count setting
*
* Parameters:  
*  void
*
* Return: 
*  Returns the Capture Count Setting
*
*******************************************************************************/
uint8   `$INSTANCE_NAME`_ReadCaptureCount(void)
{
    return `$INSTANCE_NAME`_CAP_COUNT ;
}
#endif /* `$INSTANCE_NAME`_UsingHWCaptureCounter */


/*******************************************************************************
* Function Name: `$INSTANCE_NAME`_ClearFIFO
********************************************************************************
* Summary:
*  This function clears all capture data from the capture FIFO
*
* Parameters:  
*  void:
*
* Return: 
*  void
*
*******************************************************************************/
void `$INSTANCE_NAME`_ClearFIFO(void)
{
    while(`$INSTANCE_NAME`_ReadStatusRegister() & `$INSTANCE_NAME`_STATUS_FIFONEMP)
    {
        `$INSTANCE_NAME`_ReadCapture();
    }
}

#endif /* UDB Specific Functions */

/* [] END OF FILE */
