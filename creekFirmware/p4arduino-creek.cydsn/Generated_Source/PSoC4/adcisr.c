/*******************************************************************************
* File Name: adcisr.c  
* Version 1.70
*
*  Description:
*   API for controlling the state of an interrupt.
*
*
*  Note:
*
********************************************************************************
* Copyright 2008-2012, Cypress Semiconductor Corporation.  All rights reserved.
* You may use this file only in accordance with the license, terms, conditions, 
* disclaimers, and limitations in the end user license agreement accompanying 
* the software package with which this file was provided.
*******************************************************************************/


#include <cydevice_trm.h>
#include <CyLib.h>
#include <adcisr.h>

#if !defined(adcisr__REMOVED) /* Check for removal by optimization */

/*******************************************************************************
*  Place your includes, defines and code here 
********************************************************************************/
/* `#START adcisr_intc` */

	#include "adc.h"
	#include "timer.h"
	#include "amux.h"
	
	extern uint8 adcmode;
	extern int16 adcval;
	extern uint8 chan;
	extern uint8 mode;
	
/* `#END` */

extern cyisraddress CyRamVectors[CYINT_IRQ_BASE + CY_NUM_INTERRUPTS];

/* Declared in startup, used to set unused interrupts to. */
CY_ISR_PROTO(IntDefaultHandler);


/*******************************************************************************
* Function Name: adcisr_Start
********************************************************************************
*
* Summary:
*  Set up the interrupt and enable it.
*
* Parameters:  
*   None
*
* Return:
*   None
*
*******************************************************************************/
void adcisr_Start(void)
{
    /* For all we know the interrupt is active. */
    adcisr_Disable();

    /* Set the ISR to point to the adcisr Interrupt. */
    adcisr_SetVector(&adcisr_Interrupt);

    /* Set the priority. */
    adcisr_SetPriority((uint8)adcisr_INTC_PRIOR_NUMBER);

    /* Enable it. */
    adcisr_Enable();
}


/*******************************************************************************
* Function Name: adcisr_StartEx
********************************************************************************
*
* Summary:
*  Set up the interrupt and enable it.
*
* Parameters:  
*   address: Address of the ISR to set in the interrupt vector table.
*
* Return:
*   None
*
*******************************************************************************/
void adcisr_StartEx(cyisraddress address)
{
    /* For all we know the interrupt is active. */
    adcisr_Disable();

    /* Set the ISR to point to the adcisr Interrupt. */
    adcisr_SetVector(address);

    /* Set the priority. */
    adcisr_SetPriority((uint8)adcisr_INTC_PRIOR_NUMBER);

    /* Enable it. */
    adcisr_Enable();
}


/*******************************************************************************
* Function Name: adcisr_Stop
********************************************************************************
*
* Summary:
*   Disables and removes the interrupt.
*
* Parameters:  
*
* Return:
*   None
*
*******************************************************************************/
void adcisr_Stop(void)
{
    /* Disable this interrupt. */
    adcisr_Disable();

    /* Set the ISR to point to the passive one. */
    adcisr_SetVector(&IntDefaultHandler);
}


/*******************************************************************************
* Function Name: adcisr_Interrupt
********************************************************************************
*
* Summary:
*   The default Interrupt Service Routine for adcisr.
*
*   Add custom code between the coments to keep the next version of this file
*   from over writting your code.
*
* Parameters:  
*   None
*
* Return:
*   None
*
*******************************************************************************/
CY_ISR(adcisr_Interrupt)
{
    /*  Place your Interrupt code here. */
    /* `#START adcisr_Interrupt` */

	adcval = adc_GetResult16(0);
	adcmode = 1;

	amux_FastSelect(chan);
	
	if(chan == 0)
	{
		chan = 1;
		timer_WriteCompare(5); // delay 5 microseconds
		mode = 1;		
	}
	else
	{
		chan = 0;
		timer_WriteCompare(1000); // delay x microseconds
		mode = 2;
	}
	
	adcisr_ClearPending();
	
    /* `#END` */
}


/*******************************************************************************
* Function Name: adcisr_SetVector
********************************************************************************
*
* Summary:
*   Change the ISR vector for the Interrupt. Note calling adcisr_Start
*   will override any effect this method would have had. To set the vector 
*   before the component has been started use adcisr_StartEx instead.
*
* Parameters:
*   address: Address of the ISR to set in the interrupt vector table.
*
* Return:
*   None
*
*******************************************************************************/
void adcisr_SetVector(cyisraddress address)
{
    CyRamVectors[CYINT_IRQ_BASE + adcisr__INTC_NUMBER] = address;
}


/*******************************************************************************
* Function Name: adcisr_GetVector
********************************************************************************
*
* Summary:
*   Gets the "address" of the current ISR vector for the Interrupt.
*
* Parameters:
*   None
*
* Return:
*   Address of the ISR in the interrupt vector table.
*
*******************************************************************************/
cyisraddress adcisr_GetVector(void)
{
    return CyRamVectors[CYINT_IRQ_BASE + adcisr__INTC_NUMBER];
}


/*******************************************************************************
* Function Name: adcisr_SetPriority
********************************************************************************
*
* Summary:
*   Sets the Priority of the Interrupt. Note calling adcisr_Start
*   or adcisr_StartEx will override any effect this method would 
*   have had. This method should only be called after adcisr_Start or 
*   adcisr_StartEx has been called. To set the initial
*   priority for the component use the cydwr file in the tool.
*
* Parameters:
*   priority: Priority of the interrupt. 0 - 3, 0 being the highest.
*
* Return:
*   None
*
*******************************************************************************/
void adcisr_SetPriority(uint8 priority)
{
	uint8 interruptState;
    uint32 priorityOffset = ((adcisr__INTC_NUMBER % 4u) * 8u) + 6u;
    
	interruptState = CyEnterCriticalSection();
    *adcisr_INTC_PRIOR = (*adcisr_INTC_PRIOR & (uint32)(~adcisr__INTC_PRIOR_MASK)) |
                                    ((uint32)priority << priorityOffset);
	CyExitCriticalSection(interruptState);
}


/*******************************************************************************
* Function Name: adcisr_GetPriority
********************************************************************************
*
* Summary:
*   Gets the Priority of the Interrupt.
*
* Parameters:
*   None
*
* Return:
*   Priority of the interrupt. 0 - 3, 0 being the highest.
*
*******************************************************************************/
uint8 adcisr_GetPriority(void)
{
    uint32 priority;
	uint32 priorityOffset = ((adcisr__INTC_NUMBER % 4u) * 8u) + 6u;

    priority = (*adcisr_INTC_PRIOR & adcisr__INTC_PRIOR_MASK) >> priorityOffset;

    return (uint8)priority;
}


/*******************************************************************************
* Function Name: adcisr_Enable
********************************************************************************
*
* Summary:
*   Enables the interrupt.
*
* Parameters:
*   None
*
* Return:
*   None
*
*******************************************************************************/
void adcisr_Enable(void)
{
    /* Enable the general interrupt. */
    *adcisr_INTC_SET_EN = adcisr__INTC_MASK;
}


/*******************************************************************************
* Function Name: adcisr_GetState
********************************************************************************
*
* Summary:
*   Gets the state (enabled, disabled) of the Interrupt.
*
* Parameters:
*   None
*
* Return:
*   1 if enabled, 0 if disabled.
*
*******************************************************************************/
uint8 adcisr_GetState(void)
{
    /* Get the state of the general interrupt. */
    return ((*adcisr_INTC_SET_EN & (uint32)adcisr__INTC_MASK) != 0u) ? 1u:0u;
}


/*******************************************************************************
* Function Name: adcisr_Disable
********************************************************************************
*
* Summary:
*   Disables the Interrupt.
*
* Parameters:
*   None
*
* Return:
*   None
*
*******************************************************************************/
void adcisr_Disable(void)
{
    /* Disable the general interrupt. */
    *adcisr_INTC_CLR_EN = adcisr__INTC_MASK;
}


/*******************************************************************************
* Function Name: adcisr_SetPending
********************************************************************************
*
* Summary:
*   Causes the Interrupt to enter the pending state, a software method of
*   generating the interrupt.
*
* Parameters:
*   None
*
* Return:
*   None
*
*******************************************************************************/
void adcisr_SetPending(void)
{
    *adcisr_INTC_SET_PD = adcisr__INTC_MASK;
}


/*******************************************************************************
* Function Name: adcisr_ClearPending
********************************************************************************
*
* Summary:
*   Clears a pending interrupt.
*
* Parameters:
*   None
*
* Return:
*   None
*
*******************************************************************************/
void adcisr_ClearPending(void)
{
    *adcisr_INTC_CLR_PD = adcisr__INTC_MASK;
}

#endif /* End check for removal by optimization */


/* [] END OF FILE */
