/*******************************************************************************
* File Name: ledhb.c  
* Version 1.70
*
* Description:
*  This file contains API to enable firmware control of a Pins component.
*
* Note:
*
********************************************************************************
* Copyright 2008-2010, Cypress Semiconductor Corporation.  All rights reserved.
* You may use this file only in accordance with the license, terms, conditions, 
* disclaimers, and limitations in the end user license agreement accompanying 
* the software package with which this file was provided.
********************************************************************************/

#include "cytypes.h"
#include "ledhb.h"

/* APIs are not generated for P15[7:6] */
#if !(CY_PSOC5A &&\
	 ledhb__PORT == 15 && (ledhb__MASK & 0xC0))

/*******************************************************************************
* Function Name: ledhb_Write
********************************************************************************
* Summary:
*  Assign a new value to the digital port's data output register.  
*
* Parameters:  
*  prtValue:  The value to be assigned to the Digital Port. 
*
* Return: 
*  void 
*  
*******************************************************************************/
void ledhb_Write(uint8 value) 
{
    uint8 staticBits = ledhb_DR & ~ledhb_MASK;
    ledhb_DR = staticBits | ((value << ledhb_SHIFT) & ledhb_MASK);
}


/*******************************************************************************
* Function Name: ledhb_SetDriveMode
********************************************************************************
* Summary:
*  Change the drive mode on the pins of the port.
* 
* Parameters:  
*  mode:  Change the pins to this drive mode.
*
* Return: 
*  void
*
*******************************************************************************/
void ledhb_SetDriveMode(uint8 mode) 
{
	CyPins_SetPinDriveMode(ledhb_0, mode);
}


/*******************************************************************************
* Function Name: ledhb_Read
********************************************************************************
* Summary:
*  Read the current value on the pins of the Digital Port in right justified 
*  form.
*
* Parameters:  
*  void 
*
* Return: 
*  Returns the current value of the Digital Port as a right justified number
*  
* Note:
*  Macro ledhb_ReadPS calls this function. 
*  
*******************************************************************************/
uint8 ledhb_Read(void) 
{
    return (ledhb_PS & ledhb_MASK) >> ledhb_SHIFT;
}


/*******************************************************************************
* Function Name: ledhb_ReadDataReg
********************************************************************************
* Summary:
*  Read the current value assigned to a Digital Port's data output register
*
* Parameters:  
*  void 
*
* Return: 
*  Returns the current value assigned to the Digital Port's data output register
*  
*******************************************************************************/
uint8 ledhb_ReadDataReg(void) 
{
    return (ledhb_DR & ledhb_MASK) >> ledhb_SHIFT;
}


/* If Interrupts Are Enabled for this Pins component */ 
#if defined(ledhb_INTSTAT) 

    /*******************************************************************************
    * Function Name: ledhb_ClearInterrupt
    ********************************************************************************
    * Summary:
    *  Clears any active interrupts attached to port and returns the value of the 
    *  interrupt status register.
    *
    * Parameters:  
    *  void 
    *
    * Return: 
    *  Returns the value of the interrupt status register
    *  
    *******************************************************************************/
    uint8 ledhb_ClearInterrupt(void) 
    {
        return (ledhb_INTSTAT & ledhb_MASK) >> ledhb_SHIFT;
    }

#endif /* If Interrupts Are Enabled for this Pins component */ 

#endif
/* [] END OF FILE */ 
