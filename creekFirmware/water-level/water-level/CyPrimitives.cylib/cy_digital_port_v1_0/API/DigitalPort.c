/*******************************************************************************
* File Name: `$INSTANCE_NAME`.c  
* Version `$CY_MAJOR_VERSION`.`$CY_MINOR_VERSION`
*
* Description:
*  This file contains API to enable firmware control of a Digital Port.
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
void `$INSTANCE_NAME`_Write(uint8 prtValue)
{
    uint8 staticBits = `$INSTANCE_NAME`_DR & ~`$INSTANCE_NAME`_MASK;
    `$INSTANCE_NAME`_DR = staticBits | ((prtValue << `$INSTANCE_NAME`_SHIFT) & `$INSTANCE_NAME`_MASK);
}


/*******************************************************************************
* Function Name: `$INSTANCE_NAME`_WriteDM
********************************************************************************
* Summary:
*  Change the drive mode on the pins of the port.  Use the input mask to change
*  a selected subset of the pins.
* 
* Parameters:  
*  mode:  Change the pins to this drive mode.
*  mask:  Bits of the mask that are set to 1 will allow the associated pins to 
*         be set to the new drive mode.
*
* Return: 
*  void
*  
* Note: 
*  The mask parameter is a mask of the `$INSTANCE_NAME` Digital Port component.
*  It is _NOT_ a mask for the physical port. 
*
*******************************************************************************/
void `$INSTANCE_NAME`_WriteDM(uint8 mode, uint8 mask)
{
    /* Temp variable for read, modify, write of DM registers */
    uint8 staticBits = 0;
    /* Temp value for state of each drive mode pin*/
    uint8 dmBitState = 0;
    /* Map the right justified Digital Port mask to the physical location */ 
    mask = mask << `$INSTANCE_NAME`_SHIFT;
    /* Ensure that no pins outside the digital port are attempted to be masked
     * Then Mask is adjusted for the physical port.  For this function, it 
     * should be used instead of `INSTANCE_NAME`_MASK.
     */
    mask &= `$INSTANCE_NAME`_MASK;
    

    /* Mode must be between 0 and 7 */
    mode &= `$INSTANCE_NAME`_MODE_MASK;
    
    
    /* Should bit-0 be set or cleared? */
    if((mode & `$INSTANCE_NAME`_MODE_BIT_0) != 0)
        dmBitState = `$INSTANCE_NAME`_BIT_SET;
 
    /* Read, modify, write drive mode Bit-0 */
    staticBits = `$INSTANCE_NAME`_DM0 & ~ mask;
    `$INSTANCE_NAME`_DM0 = staticBits | (dmBitState & mask); 
    
    
    /* Should bit-1 be set or cleared? */
    if((mode & `$INSTANCE_NAME`_MODE_BIT_1) != 0)
        dmBitState = `$INSTANCE_NAME`_BIT_SET;
    else
        dmBitState = `$INSTANCE_NAME`_BIT_CLEAR;
    
    /* Read, modify, write drive mode Bit-1 */
    staticBits = `$INSTANCE_NAME`_DM1 & ~ mask;
    `$INSTANCE_NAME`_DM1 = staticBits | (dmBitState & mask); 
    
    
    /* Should bit-2 be set or cleared? */
    if((mode & `$INSTANCE_NAME`_MODE_BIT_2) != 0)
        dmBitState = `$INSTANCE_NAME`_BIT_SET;
    else
        dmBitState = `$INSTANCE_NAME`_BIT_CLEAR;

    /* Read, modify, write drive mode Bit-2 */
    staticBits = `$INSTANCE_NAME`_DM2 & ~ mask;
    `$INSTANCE_NAME`_DM2 = staticBits | (dmBitState & mask); 
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
uint8 `$INSTANCE_NAME`_Read(void)
{
    return (`$INSTANCE_NAME`_PS & `$INSTANCE_NAME`_MASK) >> `$INSTANCE_NAME`_SHIFT;
}


/*******************************************************************************
* Function Name: `$INSTANCE_NAME`_ReadDR
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
uint8 `$INSTANCE_NAME`_ReadDR(void)
{
    return (`$INSTANCE_NAME`_DR & `$INSTANCE_NAME`_MASK) >> `$INSTANCE_NAME`_SHIFT;
}


/* If Interrupts Are Enabled for this Digital Port */ 
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
    uint8 `$INSTANCE_NAME`_ClearInterrupt(void)
    {
        return (`$INSTANCE_NAME`_INTSTAT & `$INSTANCE_NAME`_MASK) >> `$INSTANCE_NAME`_SHIFT;
    }


    /*******************************************************************************
    * Function Name: `$INSTANCE_NAME`_GetLastInterrupt
    ********************************************************************************
    * Summary:
    *  Gets the value of the interrupt status register from the last time it was 
    *  read by reading the Snapshot Register.  
    *
    * Parameters:  
    *  void 
    *
    * Return: 
    *  Returns the last value of the interrupt status register
    *  
    *******************************************************************************/
    uint8 `$INSTANCE_NAME`_GetLastInterrupt(void)
    {
        return (`$INSTANCE_NAME`_SNAP & `$INSTANCE_NAME`_MASK) >> `$INSTANCE_NAME`_SHIFT;
    }

#endif /* If Interrupts Are Enabled for this Digital Port */ 


/* [] END OF FILE */ 
