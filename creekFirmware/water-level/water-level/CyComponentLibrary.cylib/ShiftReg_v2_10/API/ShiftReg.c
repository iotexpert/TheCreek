/*******************************************************************************
* File Name: `$INSTANCE_NAME`.c
* Version `$CY_MAJOR_VERSION`.`$CY_MINOR_VERSION`
*
* Description:
*  This file provides the API source code for the Shift Register component.
*
* Note: none
*
********************************************************************************
* Copyright 2008-2012, Cypress Semiconductor Corporation.  All rights reserved.
* You may use this file only in accordance with the license, terms, conditions,
* disclaimers, and limitations in the end user license agreement accompanying
* the software package with which this file was provided.
********************************************************************************/


#include "`$INSTANCE_NAME`.h"
#include "CyLib.h"


uint8 `$INSTANCE_NAME`_initVar = 0u;


/*******************************************************************************
* Function Name: `$INSTANCE_NAME`_Start
********************************************************************************
*
* Summary:
*  Starts the Shift Register.
*
* Parameters:
*  None.
*
* Return:
*  None.
*
* Global Variables:
*  `$INSTANCE_NAME`_initVar - used to check initial configuration, modified on 
*  first function call.
*
* Reentrant:
*  No.
*
*******************************************************************************/
void `$INSTANCE_NAME`_Start(void) `=ReentrantKeil($INSTANCE_NAME . "_Start")`
{
    if (`$INSTANCE_NAME`_initVar == 0u)
    {
        `$INSTANCE_NAME`_Init();
        /* variable _initVar is set to indicate the fact of initialization */
        `$INSTANCE_NAME`_initVar = 1u; 
    }

    `$INSTANCE_NAME`_Enable();
}


/*******************************************************************************
* Function Name: `$INSTANCE_NAME`_Enable
********************************************************************************
*
* Summary:
*  Enables the Shift Register.
*
* Parameters:
*  void.
*
* Return:
*  void.
*
*******************************************************************************/
void `$INSTANCE_NAME`_Enable(void) `=ReentrantKeil($INSTANCE_NAME . "_Enable")`
{
    /* changing address in Datapath Control Store
       from NOP to component state machine commands space */
    `$INSTANCE_NAME`_SR_CONTROL |= `$INSTANCE_NAME`_CLK_EN; 
    
    `$INSTANCE_NAME`_EnableInt();
}


/*******************************************************************************
* Function Name: `$INSTANCE_NAME`_Init
********************************************************************************
*
* Summary:
*  Initializes Tx and/or Rx interrupt sources with initial values.
*
* Parameters:
*  void.
*
* Return:
*  void.
*
*******************************************************************************/
void `$INSTANCE_NAME`_Init(void) `=ReentrantKeil($INSTANCE_NAME . "_Init")`
{
    `$INSTANCE_NAME`_SetIntMode(`$INSTANCE_NAME`_INT_SRC);
}


/*******************************************************************************
* Function Name: `$INSTANCE_NAME`_Stop
********************************************************************************
*
* Summary:
*  Disables the Shift Register
*
* Parameters:
*  None.
*
* Return:
*  None.
*
*******************************************************************************/
void `$INSTANCE_NAME`_Stop(void) `=ReentrantKeil($INSTANCE_NAME . "_Stop")`
{
    /*changing Datapath Control Store address to NOP space*/
    `$INSTANCE_NAME`_SR_CONTROL &= ~`$INSTANCE_NAME`_CLK_EN; 
    `$INSTANCE_NAME`_DisableInt();
}


/*******************************************************************************
* Function Name: `$INSTANCE_NAME`_EnableInt
********************************************************************************
*
* Summary:
*  Enables the Shift Register interrupt.
*
* Parameters:
*  None.
*
* Return:
*  None.
*
*******************************************************************************/
void `$INSTANCE_NAME`_EnableInt(void) `=ReentrantKeil($INSTANCE_NAME . "_EnableInt")`
{
    uint8 interruptState = CyEnterCriticalSection();
    `$INSTANCE_NAME`_SR_AUX_CONTROL |= `$INSTANCE_NAME`_INTERRUPTS_ENABLE;
    CyExitCriticalSection(interruptState);
}


/*******************************************************************************
* Function Name: `$INSTANCE_NAME`_DisableInt
********************************************************************************
*
* Summary:
*  Disables the Shift Register interrupt.
*
* Parameters:
*  None.
*
* Return:
*  None.
*
*******************************************************************************/
void `$INSTANCE_NAME`_DisableInt(void) `=ReentrantKeil($INSTANCE_NAME . "_DisableInt")`
{
    uint8 interruptState = CyEnterCriticalSection();
    `$INSTANCE_NAME`_SR_AUX_CONTROL &= ~`$INSTANCE_NAME`_INTERRUPTS_ENABLE;
    CyExitCriticalSection(interruptState);
}


/*******************************************************************************
* Function Name: `$INSTANCE_NAME`_GetFIFOStatus
********************************************************************************
*
* Summary:
*  Returns current status of input or output FIFO.
*
* Parameters:
*  fifoId.
*
* Return:
*  FIFO status.
*
*******************************************************************************/
uint8 `$INSTANCE_NAME`_GetFIFOStatus(uint8 fifoId) `=ReentrantKeil($INSTANCE_NAME . "_GetFIFOStatus")`
{
    uint8 result;

    result = `$INSTANCE_NAME`_RET_FIFO_NOT_DEFINED; /*default status value*/

    #if (`$INSTANCE_NAME`_USE_INPUT_FIFO == 1u)
        
        if(fifoId == `$INSTANCE_NAME`_IN_FIFO)
        {    
            switch((`$INSTANCE_NAME`_SR_STATUS & `$INSTANCE_NAME`_IN_FIFO_MASK) >> 
                    `$INSTANCE_NAME`_IN_FIFO_SHFT_MASK)
            {
                case `$INSTANCE_NAME`_IN_FIFO_FULL :
                    result = `$INSTANCE_NAME`_RET_FIFO_FULL;
                    break;

                case `$INSTANCE_NAME`_IN_FIFO_EMPTY :
                    result = `$INSTANCE_NAME`_RET_FIFO_EMPTY;
                    break;

                case `$INSTANCE_NAME`_IN_FIFO_NOT_EMPTY :
                    result = `$INSTANCE_NAME`_RET_FIFO_NOT_EMPTY;
                    break;
                default:
                    result = `$INSTANCE_NAME`_RET_FIFO_EMPTY;
            }
        }
    #endif/*(`$INSTANCE_NAME`_USE_INPUT_FIFO == 1u)*/
    
    if(fifoId == `$INSTANCE_NAME`_OUT_FIFO)
    {
        switch((`$INSTANCE_NAME`_SR_STATUS & `$INSTANCE_NAME`_OUT_FIFO_MASK) >> 
                `$INSTANCE_NAME`_OUT_FIFO_SHFT_MASK)
        {
            case `$INSTANCE_NAME`_OUT_FIFO_FULL :
                result = `$INSTANCE_NAME`_RET_FIFO_FULL;
                break;

            case `$INSTANCE_NAME`_OUT_FIFO_EMPTY :
                result = `$INSTANCE_NAME`_RET_FIFO_EMPTY;
                break;

            case `$INSTANCE_NAME`_OUT_FIFO_NOT_EMPTY :
                result = `$INSTANCE_NAME`_RET_FIFO_NOT_EMPTY;
                break;

            default:
                result = `$INSTANCE_NAME`_RET_FIFO_FULL;
        }
    }

    return (result);
}


/*******************************************************************************
* Function Name: `$INSTANCE_NAME`_SetIntMode
********************************************************************************
*
* Summary:
*  Sets the Interrupt Source for the Shift Register interrupt. Multiple
*  sources may be ORed together
*
* Parameters:
*  interruptSource: Byte containing the constant for the selected interrupt
*  source/s.
*
* Return:
*  None.
*
*******************************************************************************/
void `$INSTANCE_NAME`_SetIntMode(uint8 interruptSource) `=ReentrantKeil($INSTANCE_NAME . "_SetIntMode")`
{
    `$INSTANCE_NAME`_SR_STATUS_MASK = (`$INSTANCE_NAME`_SR_STATUS_MASK & ~`$INSTANCE_NAME`_INTS_EN_MASK) | \
                                      (interruptSource & `$INSTANCE_NAME`_INTS_EN_MASK);
}


/*******************************************************************************
* Function Name: `$INSTANCE_NAME`_GetIntStatus
********************************************************************************
*
* Summary:
*  Gets the Shift Register Interrupt status.
*
* Parameters:
*  None.
*
* Return:
*  Byte containing the constant for the selected interrupt source/s.
*
*******************************************************************************/
uint8 `$INSTANCE_NAME`_GetIntStatus(void) `=ReentrantKeil($INSTANCE_NAME . "_GetIntStatus")`
{
    return(`$INSTANCE_NAME`_SR_STATUS & `$INSTANCE_NAME`_INTS_EN_MASK);
}


/*******************************************************************************
* Function Name: `$INSTANCE_NAME`_WriteRegValue
********************************************************************************
*
* Summary:
*  Send state directly to shift register
*
* Parameters:
*  shiftData: containing shift register state.
*
* Return:
*  None.
*
*******************************************************************************/
void `$INSTANCE_NAME`_WriteRegValue(`$RegSizeReplacementString` shiftData) \
                `=ReentrantKeil($INSTANCE_NAME . "_WriteRegValue")`
{
    `$CySetRegReplacementString`(`$INSTANCE_NAME`_SHIFT_REG_LSB_PTR, shiftData);
}


#if (`$INSTANCE_NAME`_USE_INPUT_FIFO == 1u) /* if input FIFO is used */

    /*******************************************************************************
    * Function Name: `$INSTANCE_NAME`_WriteData
    ********************************************************************************
    *
    * Summary:
    *  Send state to FIFO for later transfer to shift register based on the Load
    *  input
    *
    * Parameters:
    *  shiftData: containing shift register state.
    *
    * Return:
    *  Indicates: successful execution of function
    *  when FIFO is empty; and error when
    *  FIFO is full.
    *
    * Reentrant:
    *  No.
    *
    *******************************************************************************/
    uint8 `$INSTANCE_NAME`_WriteData(`$RegSizeReplacementString` shiftData) \
                    `=ReentrantKeil($INSTANCE_NAME . "_WriteData")`
    {
        uint8 result;
    
        result = CYRET_INVALID_STATE;/*default result state*/
    
        /*write data to input FIFO if FIFO is not full*/
        if((`$INSTANCE_NAME`_GetFIFOStatus(`$INSTANCE_NAME`_IN_FIFO)) != `$INSTANCE_NAME`_RET_FIFO_FULL)
        {
            `$CySetRegReplacementString`(`$INSTANCE_NAME`_IN_FIFO_VAL_LSB_PTR, shiftData);
            result = CYRET_SUCCESS; /*return 'success' status*/
        }
        return(result);
    }


#endif/*(`$INSTANCE_NAME`_USE_INPUT_FIFO == 1u)*/

#if (`$INSTANCE_NAME`_USE_OUTPUT_FIFO == 1u)/*if output FIFO is used*/

    /*******************************************************************************
    * Function Name: `$INSTANCE_NAME`_ReadData
    ********************************************************************************
    *
    * Summary:
    *  Returns state in FIFO due to Store input.
    *
    * Parameters:
    *  None.
    *
    * Return:
    *  Shift Register state
    *
    * Reentrant:
    *  No.
    *
    *******************************************************************************/
    `$RegSizeReplacementString` `$INSTANCE_NAME`_ReadData(void) \
            `=ReentrantKeil($INSTANCE_NAME . "_ReadData")`
    {
        return(`$CyGetRegReplacementString`(`$INSTANCE_NAME`_OUT_FIFO_VAL_LSB_PTR));
    }

#endif/*(`$INSTANCE_NAME`_USE_OUTPUT_FIFO == 1u)*/


/*******************************************************************************
* Function Name: `$INSTANCE_NAME`_ReadRegValue
********************************************************************************
*
* Summary:
*  Directly returns current state in shift register, not data in FIFO due
*  to Store input.
*
* Parameters:
*  None.
*
* Return:
*  Shift Register state
*
*  Clears output FIFO.
*
* Reentrant:
*  No.
*
*******************************************************************************/
`$RegSizeReplacementString` `$INSTANCE_NAME`_ReadRegValue(void) `=ReentrantKeil($INSTANCE_NAME . "_ReadRegValue")`
{
    `$RegSizeReplacementString` result;

    /* Clear FIFO before software capture */

    while(`$INSTANCE_NAME`_GetFIFOStatus(`$INSTANCE_NAME`_OUT_FIFO) != `$INSTANCE_NAME`_RET_FIFO_EMPTY)
    {
        result = `$CyGetRegReplacementString`(`$INSTANCE_NAME`_OUT_FIFO_VAL_LSB_PTR);
    }
    
    /* Capture A1 to output FIFO */
    result = CY_GET_REG8(`$INSTANCE_NAME`_SHIFT_REG_VALUE_LSB_PTR);
    
    /* Read output FIFO */
    result = `$CyGetRegReplacementString`(`$INSTANCE_NAME`_OUT_FIFO_VAL_LSB_PTR);
    result = result & `$INSTANCE_NAME`_SR_MASK;

    return(result);
}

/* [] END OF FILE */
