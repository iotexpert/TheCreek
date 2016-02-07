/*******************************************************************************
* File Name: vlow.c  
* Version 1.90
*
* Description:
*  This file contains API to enable firmware control of a Pins component.
*
* Note:
*
********************************************************************************
* Copyright 2008-2012, Cypress Semiconductor Corporation.  All rights reserved.
* You may use this file only in accordance with the license, terms, conditions, 
* disclaimers, and limitations in the end user license agreement accompanying 
* the software package with which this file was provided.
*******************************************************************************/

#include "cytypes.h"
#include "vlow.h"

#define SetP4PinDriveMode(shift, mode)  \
    do { \
        vlow_PC =   (vlow_PC & \
                                (uint32)(~(uint32)(vlow_DRIVE_MODE_IND_MASK << (vlow_DRIVE_MODE_BITS * (shift))))) | \
                                (uint32)((uint32)(mode) << (vlow_DRIVE_MODE_BITS * (shift))); \
    } while (0)


/*******************************************************************************
* Function Name: vlow_Write
********************************************************************************
*
* Summary:
*  Assign a new value to the digital port's data output register.  
*
* Parameters:  
*  prtValue:  The value to be assigned to the Digital Port. 
*
* Return: 
*  None 
*  
*******************************************************************************/
void vlow_Write(uint8 value) 
{
    uint8 drVal = (uint8)(vlow_DR & (uint8)(~vlow_MASK));
    drVal = (drVal | ((uint8)(value << vlow_SHIFT) & vlow_MASK));
    vlow_DR = (uint32)drVal;
}


/*******************************************************************************
* Function Name: vlow_SetDriveMode
********************************************************************************
*
* Summary:
*  Change the drive mode on the pins of the port.
* 
* Parameters:  
*  mode:  Change the pins to this drive mode.
*
* Return: 
*  None
*
*******************************************************************************/
void vlow_SetDriveMode(uint8 mode) 
{
	SetP4PinDriveMode(vlow__0__SHIFT, mode);
}


/*******************************************************************************
* Function Name: vlow_Read
********************************************************************************
*
* Summary:
*  Read the current value on the pins of the Digital Port in right justified 
*  form.
*
* Parameters:  
*  None 
*
* Return: 
*  Returns the current value of the Digital Port as a right justified number
*  
* Note:
*  Macro vlow_ReadPS calls this function. 
*  
*******************************************************************************/
uint8 vlow_Read(void) 
{
    return (uint8)((vlow_PS & vlow_MASK) >> vlow_SHIFT);
}


/*******************************************************************************
* Function Name: vlow_ReadDataReg
********************************************************************************
*
* Summary:
*  Read the current value assigned to a Digital Port's data output register
*
* Parameters:  
*  None 
*
* Return: 
*  Returns the current value assigned to the Digital Port's data output register
*  
*******************************************************************************/
uint8 vlow_ReadDataReg(void) 
{
    return (uint8)((vlow_DR & vlow_MASK) >> vlow_SHIFT);
}


/* If Interrupts Are Enabled for this Pins component */ 
#if defined(vlow_INTSTAT) 

    /*******************************************************************************
    * Function Name: vlow_ClearInterrupt
    ********************************************************************************
    *
    * Summary:
    *  Clears any active interrupts attached to port and returns the value of the 
    *  interrupt status register.
    *
    * Parameters:  
    *  None 
    *
    * Return: 
    *  Returns the value of the interrupt status register
    *  
    *******************************************************************************/
    uint8 vlow_ClearInterrupt(void) 
    {
		uint8 maskedStatus = (uint8)(vlow_INTSTAT & vlow_MASK);
		vlow_INTSTAT = maskedStatus;
        return maskedStatus >> vlow_SHIFT;
    }

#endif /* If Interrupts Are Enabled for this Pins component */ 


/* [] END OF FILE */
