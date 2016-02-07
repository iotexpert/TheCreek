/*******************************************************************************
* File Name: vmid2.c  
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
#include "vmid2.h"

#define SetP4PinDriveMode(shift, mode)  \
    do { \
        vmid2_PC =   (vmid2_PC & \
                                (uint32)(~(uint32)(vmid2_DRIVE_MODE_IND_MASK << (vmid2_DRIVE_MODE_BITS * (shift))))) | \
                                (uint32)((uint32)(mode) << (vmid2_DRIVE_MODE_BITS * (shift))); \
    } while (0)


/*******************************************************************************
* Function Name: vmid2_Write
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
void vmid2_Write(uint8 value) 
{
    uint8 drVal = (uint8)(vmid2_DR & (uint8)(~vmid2_MASK));
    drVal = (drVal | ((uint8)(value << vmid2_SHIFT) & vmid2_MASK));
    vmid2_DR = (uint32)drVal;
}


/*******************************************************************************
* Function Name: vmid2_SetDriveMode
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
void vmid2_SetDriveMode(uint8 mode) 
{
	SetP4PinDriveMode(vmid2__0__SHIFT, mode);
}


/*******************************************************************************
* Function Name: vmid2_Read
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
*  Macro vmid2_ReadPS calls this function. 
*  
*******************************************************************************/
uint8 vmid2_Read(void) 
{
    return (uint8)((vmid2_PS & vmid2_MASK) >> vmid2_SHIFT);
}


/*******************************************************************************
* Function Name: vmid2_ReadDataReg
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
uint8 vmid2_ReadDataReg(void) 
{
    return (uint8)((vmid2_DR & vmid2_MASK) >> vmid2_SHIFT);
}


/* If Interrupts Are Enabled for this Pins component */ 
#if defined(vmid2_INTSTAT) 

    /*******************************************************************************
    * Function Name: vmid2_ClearInterrupt
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
    uint8 vmid2_ClearInterrupt(void) 
    {
		uint8 maskedStatus = (uint8)(vmid2_INTSTAT & vmid2_MASK);
		vmid2_INTSTAT = maskedStatus;
        return maskedStatus >> vmid2_SHIFT;
    }

#endif /* If Interrupts Are Enabled for this Pins component */ 


/* [] END OF FILE */
