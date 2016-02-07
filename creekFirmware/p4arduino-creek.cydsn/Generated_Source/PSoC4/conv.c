/*******************************************************************************
* File Name: conv.c  
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
#include "conv.h"

#define SetP4PinDriveMode(shift, mode)  \
    do { \
        conv_PC =   (conv_PC & \
                                (uint32)(~(uint32)(conv_DRIVE_MODE_IND_MASK << (conv_DRIVE_MODE_BITS * (shift))))) | \
                                (uint32)((uint32)(mode) << (conv_DRIVE_MODE_BITS * (shift))); \
    } while (0)


/*******************************************************************************
* Function Name: conv_Write
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
void conv_Write(uint8 value) 
{
    uint8 drVal = (uint8)(conv_DR & (uint8)(~conv_MASK));
    drVal = (drVal | ((uint8)(value << conv_SHIFT) & conv_MASK));
    conv_DR = (uint32)drVal;
}


/*******************************************************************************
* Function Name: conv_SetDriveMode
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
void conv_SetDriveMode(uint8 mode) 
{
	SetP4PinDriveMode(conv__0__SHIFT, mode);
}


/*******************************************************************************
* Function Name: conv_Read
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
*  Macro conv_ReadPS calls this function. 
*  
*******************************************************************************/
uint8 conv_Read(void) 
{
    return (uint8)((conv_PS & conv_MASK) >> conv_SHIFT);
}


/*******************************************************************************
* Function Name: conv_ReadDataReg
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
uint8 conv_ReadDataReg(void) 
{
    return (uint8)((conv_DR & conv_MASK) >> conv_SHIFT);
}


/* If Interrupts Are Enabled for this Pins component */ 
#if defined(conv_INTSTAT) 

    /*******************************************************************************
    * Function Name: conv_ClearInterrupt
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
    uint8 conv_ClearInterrupt(void) 
    {
		uint8 maskedStatus = (uint8)(conv_INTSTAT & conv_MASK);
		conv_INTSTAT = maskedStatus;
        return maskedStatus >> conv_SHIFT;
    }

#endif /* If Interrupts Are Enabled for this Pins component */ 


/* [] END OF FILE */
