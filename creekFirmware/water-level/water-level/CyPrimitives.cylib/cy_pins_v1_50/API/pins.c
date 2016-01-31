/*******************************************************************************
* File Name: `$INSTANCE_NAME`.c  
* Version `$CY_MAJOR_VERSION`.`$CY_MINOR_VERSION`
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
#include "`$INSTANCE_NAME`.h"


/*******************************************************************************
* Function Name: `$INSTANCE_NAME`_Write
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
void `$INSTANCE_NAME`_Write(uint8 value) `=ReentrantKeil($INSTANCE_NAME ."_Write")`
{
    uint8 staticBits = `$INSTANCE_NAME`_DR & ~`$INSTANCE_NAME`_MASK;
    `$INSTANCE_NAME`_DR = staticBits | ((value << `$INSTANCE_NAME`_SHIFT) & `$INSTANCE_NAME`_MASK);
}


/*******************************************************************************
* Function Name: `$INSTANCE_NAME`_SetDriveMode
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
void `$INSTANCE_NAME`_SetDriveMode(uint8 mode) `=ReentrantKeil($INSTANCE_NAME ."_SetDriveMode")`
{
`$SetDriveModeImpl_API_GEN`
}


/*******************************************************************************
* Function Name: `$INSTANCE_NAME`_Read
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
*  Macro `$INSTANCE_NAME`_ReadPS calls this function. 
*  
*******************************************************************************/
uint8 `$INSTANCE_NAME`_Read(void) `=ReentrantKeil($INSTANCE_NAME ."_Read")`
{
    return (`$INSTANCE_NAME`_PS & `$INSTANCE_NAME`_MASK) >> `$INSTANCE_NAME`_SHIFT;
}


/*******************************************************************************
* Function Name: `$INSTANCE_NAME`_ReadDataReg
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
uint8 `$INSTANCE_NAME`_ReadDataReg(void) `=ReentrantKeil($INSTANCE_NAME ."_ReadDataReg")`
{
    return (`$INSTANCE_NAME`_DR & `$INSTANCE_NAME`_MASK) >> `$INSTANCE_NAME`_SHIFT;
}


/* If Interrupts Are Enabled for this Pins component */ 
#if defined(`$INSTANCE_NAME`_INTSTAT) 

    /*******************************************************************************
    * Function Name: `$INSTANCE_NAME`_ClearInterrupt
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
    uint8 `$INSTANCE_NAME`_ClearInterrupt(void) `=ReentrantKeil($INSTANCE_NAME ."_ClearInterrupt")`
    {
        return (`$INSTANCE_NAME`_INTSTAT & `$INSTANCE_NAME`_MASK) >> `$INSTANCE_NAME`_SHIFT;
    }

#endif /* If Interrupts Are Enabled for this Pins component */ 


/* [] END OF FILE */ 
