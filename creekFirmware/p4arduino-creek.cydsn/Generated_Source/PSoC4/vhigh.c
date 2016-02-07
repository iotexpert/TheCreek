/*******************************************************************************
* File Name: vhigh.c  
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
#include "vhigh.h"

#define SetP4PinDriveMode(shift, mode)  \
    do { \
        vhigh_PC =   (vhigh_PC & \
                                (uint32)(~(uint32)(vhigh_DRIVE_MODE_IND_MASK << (vhigh_DRIVE_MODE_BITS * (shift))))) | \
                                (uint32)((uint32)(mode) << (vhigh_DRIVE_MODE_BITS * (shift))); \
    } while (0)


/*******************************************************************************
* Function Name: vhigh_Write
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
void vhigh_Write(uint8 value) 
{
    uint8 drVal = (uint8)(vhigh_DR & (uint8)(~vhigh_MASK));
    drVal = (drVal | ((uint8)(value << vhigh_SHIFT) & vhigh_MASK));
    vhigh_DR = (uint32)drVal;
}


/*******************************************************************************
* Function Name: vhigh_SetDriveMode
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
void vhigh_SetDriveMode(uint8 mode) 
{
	SetP4PinDriveMode(vhigh__0__SHIFT, mode);
}


/*******************************************************************************
* Function Name: vhigh_Read
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
*  Macro vhigh_ReadPS calls this function. 
*  
*******************************************************************************/
uint8 vhigh_Read(void) 
{
    return (uint8)((vhigh_PS & vhigh_MASK) >> vhigh_SHIFT);
}


/*******************************************************************************
* Function Name: vhigh_ReadDataReg
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
uint8 vhigh_ReadDataReg(void) 
{
    return (uint8)((vhigh_DR & vhigh_MASK) >> vhigh_SHIFT);
}


/* If Interrupts Are Enabled for this Pins component */ 
#if defined(vhigh_INTSTAT) 

    /*******************************************************************************
    * Function Name: vhigh_ClearInterrupt
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
    uint8 vhigh_ClearInterrupt(void) 
    {
		uint8 maskedStatus = (uint8)(vhigh_INTSTAT & vhigh_MASK);
		vhigh_INTSTAT = maskedStatus;
        return maskedStatus >> vhigh_SHIFT;
    }

#endif /* If Interrupts Are Enabled for this Pins component */ 


/* [] END OF FILE */
