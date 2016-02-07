/*******************************************************************************
* File Name: countisr.c  
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
#include <countisr.h>

#if !defined(countisr__REMOVED) /* Check for removal by optimization */

/*******************************************************************************
*  Place your includes, defines and code here 
********************************************************************************/
/* `#START countisr_intc` */
	
	extern uint8 mode;

/* `#END` */

extern cyisraddress CyRamVectors[CYINT_IRQ_BASE + CY_NUM_INTERRUPTS];

/* Declared in startup, used to set unused interrupts to. */
CY_ISR_PROTO(IntDefaultHandler);


/*******************************************************************************
* Function Name: countisr_Start
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
void countisr_Start(void)
{
    /* For all we know the interrupt is active. */
    countisr_Disable();

    /* Set the ISR to point to the countisr Interrupt. */
    countisr_SetVector(&countisr_Interrupt);

    /* Set the priority. */
    countisr_SetPriority((uint8)countisr_INTC_PRIOR_NUMBER);

    /* Enable it. */
    countisr_Enable();
}


/*******************************************************************************
* Function Name: countisr_StartEx
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
void countisr_StartEx(cyisraddress address)
{
    /* For all we know the interrupt is active. */
    countisr_Disable();

    /* Set the ISR to point to the countisr Interrupt. */
    countisr_SetVector(address);

    /* Set the priority. */
    countisr_SetPriority((uint8)countisr_INTC_PRIOR_NUMBER);

    /* Enable it. */
    countisr_Enable();
}


/*******************************************************************************
* Function Name: countisr_Stop
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
void countisr_Stop(void)
{
    /* Disable this interrupt. */
    countisr_Disable();

    /* Set the ISR to point to the passive one. */
    countisr_SetVector(&IntDefaultHandler);
}


/*******************************************************************************
* Function Name: countisr_Interrupt
********************************************************************************
*
* Summary:
*   The default Interrupt Service Routine for countisr.
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
CY_ISR(countisr_Interrupt)
{
    /*  Place your Interrupt code here. */
    /* `#START countisr_Interrupt` */
	
	switch(mode) {
		case 0:
		
		case 1:
		case 2:
		break;
	}
	
	countisr_ClearPending();

    /* `#END` */
}


/*******************************************************************************
* Function Name: countisr_SetVector
********************************************************************************
*
* Summary:
*   Change the ISR vector for the Interrupt. Note calling countisr_Start
*   will override any effect this method would have had. To set the vector 
*   before the component has been started use countisr_StartEx instead.
*
* Parameters:
*   address: Address of the ISR to set in the interrupt vector table.
*
* Return:
*   None
*
*******************************************************************************/
void countisr_SetVector(cyisraddress address)
{
    CyRamVectors[CYINT_IRQ_BASE + countisr__INTC_NUMBER] = address;
}


/*******************************************************************************
* Function Name: countisr_GetVector
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
cyisraddress countisr_GetVector(void)
{
    return CyRamVectors[CYINT_IRQ_BASE + countisr__INTC_NUMBER];
}


/*******************************************************************************
* Function Name: countisr_SetPriority
********************************************************************************
*
* Summary:
*   Sets the Priority of the Interrupt. Note calling countisr_Start
*   or countisr_StartEx will override any effect this method would 
*   have had. This method should only be called after countisr_Start or 
*   countisr_StartEx has been called. To set the initial
*   priority for the component use the cydwr file in the tool.
*
* Parameters:
*   priority: Priority of the interrupt. 0 - 3, 0 being the highest.
*
* Return:
*   None
*
*******************************************************************************/
void countisr_SetPriority(uint8 priority)
{
	uint8 interruptState;
    uint32 priorityOffset = ((countisr__INTC_NUMBER % 4u) * 8u) + 6u;
    
	interruptState = CyEnterCriticalSection();
    *countisr_INTC_PRIOR = (*countisr_INTC_PRIOR & (uint32)(~countisr__INTC_PRIOR_MASK)) |
                                    ((uint32)priority << priorityOffset);
	CyExitCriticalSection(interruptState);
}


/*******************************************************************************
* Function Name: countisr_GetPriority
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
uint8 countisr_GetPriority(void)
{
    uint32 priority;
	uint32 priorityOffset = ((countisr__INTC_NUMBER % 4u) * 8u) + 6u;

    priority = (*countisr_INTC_PRIOR & countisr__INTC_PRIOR_MASK) >> priorityOffset;

    return (uint8)priority;
}


/*******************************************************************************
* Function Name: countisr_Enable
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
void countisr_Enable(void)
{
    /* Enable the general interrupt. */
    *countisr_INTC_SET_EN = countisr__INTC_MASK;
}


/*******************************************************************************
* Function Name: countisr_GetState
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
uint8 countisr_GetState(void)
{
    /* Get the state of the general interrupt. */
    return ((*countisr_INTC_SET_EN & (uint32)countisr__INTC_MASK) != 0u) ? 1u:0u;
}


/*******************************************************************************
* Function Name: countisr_Disable
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
void countisr_Disable(void)
{
    /* Disable the general interrupt. */
    *countisr_INTC_CLR_EN = countisr__INTC_MASK;
}


/*******************************************************************************
* Function Name: countisr_SetPending
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
void countisr_SetPending(void)
{
    *countisr_INTC_SET_PD = countisr__INTC_MASK;
}


/*******************************************************************************
* Function Name: countisr_ClearPending
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
void countisr_ClearPending(void)
{
    *countisr_INTC_CLR_PD = countisr__INTC_MASK;
}

#endif /* End check for removal by optimization */


/* [] END OF FILE */
