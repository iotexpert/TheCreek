/*******************************************************************************
* File Name: `$INSTANCE_NAME`.c  
* Version `$CY_MAJOR_VERSION`.`$CY_MINOR_VERSION`
*
* Description:
*  This file provides the source code to the API for the Quadrature Decoder
*  component.
*
* Note:
*  None
*   
*
********************************************************************************
* Copyright 2008-2009, Cypress Semiconductor Corporation.  All rights reserved.
* You may use this file only in accordance with the license, terms, conditions, 
* disclaimers, and limitations in the end user license agreement accompanying 
* the software package with which this file was provided.
*******************************************************************************/

#include "`$INSTANCE_NAME`.h"
#include "CyLib.h"

#if(`$INSTANCE_NAME`_COUNTER_SIZE == 32)
    extern int32 `$INSTANCE_NAME`_Count32SoftPart;
#endif /*`$INSTANCE_NAME`_COUNTER_SIZE == 32*/


uint8 `$INSTANCE_NAME`_initVar = 0;


/*******************************************************************************
* Function Name: `$INSTANCE_NAME`_Start
********************************************************************************
*
* Summary:
*  Initializes UDBs and other relevant hardware. 
*  Resets counter to 0, enables or disables all relevant interrupts.
*  Starts monitoring the inputs and counting.
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
    #if (`$INSTANCE_NAME`_COUNTER_SIZE == 8)
        `$INSTANCE_NAME`_Cnt8_Start();
    #else    	
        `$INSTANCE_NAME`_Cnt16_Start();
    #endif /* `$INSTANCE_NAME`_COUNTER_SIZE == 8 */
	
    `$INSTANCE_NAME`_SR_AUX_CONTROL |= `$INSTANCE_NAME`_INTERRUPTS_ENABLE;      /* enable interrupts */
    
    if(`$INSTANCE_NAME`_initVar == 0)
    {
        `$INSTANCE_NAME`_initVar = 1;
    
        #if (`$INSTANCE_NAME`_COUNTER_SIZE == 32)
            /* Disable Interrupt. */
            CyIntDisable(`$INSTANCE_NAME`_ISR_NUMBER);
            
            /* Set the ISR to point to the `$INSTANCE_NAME`_isr Interrupt. */
            CyIntSetVector(`$INSTANCE_NAME`_ISR_NUMBER, `$INSTANCE_NAME`_ISR);
            
            /* Set the priority. */
            CyIntSetPriority(`$INSTANCE_NAME`_ISR_NUMBER, `$INSTANCE_NAME`_ISR_PRIORITY);
            
        #endif /* `$INSTANCE_NAME`_COUNTER_SIZE == 32 */
        
        `$INSTANCE_NAME`_Control_Reg_Write(`$INSTANCE_NAME`_ENABLE);            /* QD enable */
    }
    #if (`$INSTANCE_NAME`_COUNTER_SIZE == 32)
	    /* Enable it. */
        CyIntEnable(`$INSTANCE_NAME`_ISR_NUMBER);
    #endif /* `$INSTANCE_NAME`_COUNTER_SIZE == 32 */
}


/*******************************************************************************
* Function Name: `$INSTANCE_NAME`_Stop
********************************************************************************
*
* Summary:
*  Turns off UDBs and other relevant hardware.
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
    #if (`$INSTANCE_NAME`_COUNTER_SIZE == 8)
        `$INSTANCE_NAME`_Cnt8_Stop();
    #else
        `$INSTANCE_NAME`_Cnt16_Stop();                      					/* counter disable */
    #endif /* (`$INSTANCE_NAME`_COUNTER_SIZE == 8) */
    #if (`$INSTANCE_NAME`_COUNTER_SIZE == 32)
	    CyIntDisable(`$INSTANCE_NAME`_ISR_NUMBER);                              /* interrupt disable */
    #endif /* `$INSTANCE_NAME`_COUNTER_SIZE == 32 */
}


/*******************************************************************************
* Function Name: `$INSTANCE_NAME`_GetCounter
********************************************************************************
*
* Summary:
*  Reports the current value of the counter.
*
* Parameters:  
*  void  
*
* Return: 
*  The counter value. Return type is signed and per 
*  the counter size setting. A positive value indicates 
*  clockwise movement (B before A).
*
*******************************************************************************/
`$CounterSizeReplacementString` `$INSTANCE_NAME`_GetCounter(void)
{
    `$CounterSizeReplacementString` count;
    `$CounterSizeReplacementStringUnsigned` tmpCnt;    
    #if (`$INSTANCE_NAME`_COUNTER_SIZE == 32)
        int16 hwCount;     
    #endif /* `$INSTANCE_NAME`_COUNTER_SIZE == 32 */
    
    #if (`$INSTANCE_NAME`_COUNTER_SIZE == 8)
        tmpCnt = `$INSTANCE_NAME`_Cnt8_ReadCounter();        
        count = tmpCnt ^ 0x80;
    #endif /* `$INSTANCE_NAME`_COUNTER_SIZE == 8 */
    #if (`$INSTANCE_NAME`_COUNTER_SIZE == 16)
        tmpCnt = `$INSTANCE_NAME`_Cnt16_ReadCounter();
        count = tmpCnt ^ 0x8000;    
    #endif /* `$INSTANCE_NAME`_COUNTER_SIZE == 16 */    
    #if (`$INSTANCE_NAME`_COUNTER_SIZE == 32)   
        tmpCnt = `$INSTANCE_NAME`_Cnt16_ReadCounter();
        hwCount = tmpCnt ^ 0x8000;
        count = `$INSTANCE_NAME`_Count32SoftPart + hwCount;
    #endif /* `$INSTANCE_NAME`_COUNTER_SIZE == 32 */   
        
    return(count);    
}


/*******************************************************************************
* Function Name: `$INSTANCE_NAME`_SetCounter
********************************************************************************
*
* Summary:
*  Sets the current value of the counter.
*
* Parameters:  
*  value:  The new value. Parameter type is signed and per the counter size  
*  setting.  
*
* Return: 
*  void
*
*******************************************************************************/
void `$INSTANCE_NAME`_SetCounter(`$CounterSizeReplacementString` value)
{    
    #if ((`$INSTANCE_NAME`_COUNTER_SIZE == 8) || (`$INSTANCE_NAME`_COUNTER_SIZE == 16))
        `$CounterSizeReplacementStringUnsigned` count; 
    #endif  /* (`$INSTANCE_NAME`_COUNTER_SIZE == 8) || (`$INSTANCE_NAME`_COUNTER_SIZE == 16) */   
    
    #if (`$INSTANCE_NAME`_COUNTER_SIZE == 8)     
        count = (value ^ 0x80);
        `$INSTANCE_NAME`_Cnt8_WriteCounter(count);
    #endif  /* `$INSTANCE_NAME`_COUNTER_SIZE == 8 */
    #if (`$INSTANCE_NAME`_COUNTER_SIZE == 16)        
        count = (value ^ 0x8000);
        `$INSTANCE_NAME`_Cnt16_WriteCounter(count);
    #endif  /* `$INSTANCE_NAME`_COUNTER_SIZE == 16 */
    #if (`$INSTANCE_NAME`_COUNTER_SIZE == 32)
        `$INSTANCE_NAME`_Count32SoftPart = value;
        `$INSTANCE_NAME`_Cnt16_WriteCounter(0x8000);
    #endif  /* `$INSTANCE_NAME`_COUNTER_SIZE == 32 */
}


/*******************************************************************************
* Function Name: `$INSTANCE_NAME`_SetInterruptMask
********************************************************************************
*
* Summary:
*  Enables / disables interrupts due to the events. 
*  For the 32-bit counter, the overflow and underflow interrupts cannot be 
*  disabled; these bits are ignored.
*
* Parameters:  
*  mask:  Enable / disable bits in an 8-bit value,where 1 enables the interrupt. 
*
* Return: 
*  void
*
*******************************************************************************/
void `$INSTANCE_NAME`_SetInterruptMask(uint8 mask)
{
    #if (`$INSTANCE_NAME`_COUNTER_SIZE == 32)
        /* Underflow & Overflow interrupt for 32-bit Counter  always enable */
        mask |= (`$INSTANCE_NAME`_COUNTER_OVERFLOW | `$INSTANCE_NAME`_COUNTER_UNDERFLOW);                                            
    #endif /* `$INSTANCE_NAME`_COUNTER_SIZE == 32 */
    `$INSTANCE_NAME`_STATUS_MASK = mask;
}


/*******************************************************************************
* Function Name: `$INSTANCE_NAME`_GetEvents
********************************************************************************
* 
* Summary:
*   Reports the current status of events.
*
* Parameters:  
*  void  
*
* Return: 
*  The events, as bits in an unsigned 8-bit value:
*        Bit        Description
*
*        0        Counter overflow.
*        1        Counter underflow.
*        2        Counter reset due to index, if index input is used.
*        3        Invalid A, B inputs state transition.
*
*******************************************************************************/
uint8 `$INSTANCE_NAME`_GetEvents(void)
{   
    return (`$INSTANCE_NAME`_STATUS & `$INSTANCE_NAME`_INIT_INT_MASK);
}


/*******************************************************************************
* Function Name: `$INSTANCE_NAME`_GetInterruptMask
********************************************************************************
* 
* Summary:
*   Reports the current interrupt mask settings.
*
* Parameters:  
*  void
*
* Return: 
*  Enable / disable bits in an 8-bit value, where 1 enables the interrupt.
*  For the 32-bit counter, the overflow and underflow enable bits are always 
*  set.
*
*******************************************************************************/
uint8 `$INSTANCE_NAME`_GetInterruptMask(void)
{
    return (`$INSTANCE_NAME`_STATUS_MASK);
}


/* [] END OF FILE */
