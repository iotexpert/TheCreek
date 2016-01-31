/*******************************************************************************
* File Name: `$INSTANCE_NAME`.c
* Version `$CY_MAJOR_VERSION`.`$CY_MINOR_VERSION`
*
* Description:
* This file provides the API source code for the Shift Register component.
*
* Note:
*
********************************************************************************
* Copyright 2008-2009, Cypress Semiconductor Corporation.  All rights reserved.
* You may use this file only in accordance with the license, terms, conditions, 
* disclaimers, and limitations in the end user license agreement accompanying 
* the software package with which this file was provided.
********************************************************************************/



#include "`$INSTANCE_NAME`.h"


/*******************************************************************************
* Function Name: `$INSTANCE_NAME`_Start
********************************************************************************
*
* Summary:
*  Starts the Shift Register.
*
* Parameters:  
*  void:  
*
* Return: 
*  void
*
* Theory: 
*
* Side Effects:
*
*******************************************************************************/ 
void `$INSTANCE_NAME`_Start(void)
{
    `$INSTANCE_NAME`_SR_CONTROL |= `$INSTANCE_NAME`_CLK_EN ; 
    `$INSTANCE_NAME`_EnableInt();
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
*  void:  
*
* Return: 
*  void
*
* Theory: 
*
* Side Effects:
*
*******************************************************************************/
void `$INSTANCE_NAME`_Stop(void)
{
    `$INSTANCE_NAME`_SR_CONTROL &= ~`$INSTANCE_NAME`_CLK_EN;
    `$INSTANCE_NAME`_DisableInt();
}


/*******************************************************************************
* Function Name: `$INSTANCE_NAME`_EnableInt
********************************************************************************
*
* Summary:
*  Enables the Shift Register interrupt
*
* Parameters:  
*  void:  
*
* Return: 
*  void
*
* Theory: 
*
* Side Effects:
*
*******************************************************************************/
void `$INSTANCE_NAME`_EnableInt(void)
{
    `$INSTANCE_NAME`_SR_AUX_CONTROL |= `$INSTANCE_NAME`_INTERRUPTS_ENABLE;
}


/*******************************************************************************
* Function Name: `$INSTANCE_NAME`_DisableInt
********************************************************************************
*
* Summary:
*  Disables the Shift Register interrupt
*
* Parameters:  
*  void:  
*
* Return: 
*  void
*
* Theory: 
*
* Side Effects:
*
*******************************************************************************/
void `$INSTANCE_NAME`_DisableInt(void)
{
    `$INSTANCE_NAME`_SR_AUX_CONTROL &= ~`$INSTANCE_NAME`_INTERRUPTS_ENABLE;
}


#ifdef `$INSTANCE_NAME`_FIFO_USED
/*******************************************************************************
* Function Name: `$INSTANCE_NAME`_GetFIFOStatus
********************************************************************************
*
* Summary:
*  Disables the Shift Register interrupt
*
* Parameters:  
*  void:  
*
* Return: 
*  void
*
* Theory: 
*
* Side Effects:
*
*******************************************************************************/
uint8 `$INSTANCE_NAME`_GetFIFOStatus(uint8 fifoId)
{
    uint8 temp;
    uint8 result;

    result = `$INSTANCE_NAME`_RET_FIFO_NOT_DEFINED;
    temp = `$INSTANCE_NAME`_SR_STATUS;

#if (`$INSTANCE_NAME`_USE_INPUT_FIFO == 1)

    if(fifoId == `$INSTANCE_NAME`_IN_FIFO)
    {
        temp = (temp & `$INSTANCE_NAME`_IN_FIFO_MASK) >> 3;

#if(`$INSTANCE_NAME`_FIFO_SIZE == 1)

        switch(temp)
        {
            case `$INSTANCE_NAME`_IN_FIFO_EMPTY :
                result = `$INSTANCE_NAME`_RET_FIFO_EMPTY;
            break;

            default: result = `$INSTANCE_NAME`_RET_FIFO_FULL;;
        }
#endif /* `$INSTANCE_NAME`_FIFO_SIZE */

#if(`$INSTANCE_NAME`_FIFO_SIZE == 4)
        switch(temp)
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

            default: result = `$INSTANCE_NAME`_RET_FIFO_EMPTY;
        }

#endif /* `$INSTANCE_NAME`_FIFO_SIZE */
    }
#endif

#if (`$INSTANCE_NAME`_USE_OUTPUT_FIFO == 1)

    if(fifoId == `$INSTANCE_NAME`_OUT_FIFO)
    {
        temp = (temp & `$INSTANCE_NAME`_OUT_FIFO_MASK) >> 5;

#if(`$INSTANCE_NAME`_FIFO_SIZE == 1)

        switch(temp)
        {
            case `$INSTANCE_NAME`_OUT_FIFO_EMPTY :
                result = `$INSTANCE_NAME`_RET_FIFO_EMPTY;
            break;

            default: result = `$INSTANCE_NAME`_RET_FIFO_FULL;;
        }
#endif /* `$INSTANCE_NAME`_FIFO_SIZE */

#if(`$INSTANCE_NAME`_FIFO_SIZE == 4)
        switch(temp)
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

            default: result = `$INSTANCE_NAME`_RET_FIFO_FULL;
        }

#endif /* `$INSTANCE_NAME`_FIFO_SIZE */
    }
#endif
	
   return (result); 
}
#endif /*`$INSTANCE_NAME`_FIFO_USED*/


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
*  void
*
* Theory: 
*
* Side Effects:
*
*******************************************************************************/
void `$INSTANCE_NAME`_SetIntMode(uint8 interruptSource)
{
    interruptSource &= `$INSTANCE_NAME`_INTS_EN_MASK;
    `$INSTANCE_NAME`_SR_STATUS_MASK = (`$INSTANCE_NAME`_SR_STATUS_MASK  & ~`$INSTANCE_NAME`_INTS_EN_MASK) | interruptSource; 
}


/*******************************************************************************
* Function Name: `$INSTANCE_NAME`_GetIntStatus
********************************************************************************
*
* Summary:
*  Gets the Shift Register Interrupt status.
*
* Parameters:  
*  void  
*
* Return: 
*  Byte containing the constant for the selected interrupt source/s.
*
* Theory: 
*
* Side Effects:
*
*******************************************************************************/
uint8 `$INSTANCE_NAME`_GetIntStatus(void)
{
    return(`$INSTANCE_NAME`_SR_STATUS & `$INSTANCE_NAME`_INTS_EN_MASK);
}


#if (`$INSTANCE_NAME`_SR_SIZE <= 8)
/*******************************************************************************
* Function Name: `$INSTANCE_NAME`_WriteRegValue
********************************************************************************
*
* Summary:
*  Send state directly to shift register
*
* Parameters:  
*  txDataByte: containing shift register state. 
*
* Return: 
*  void
*
* Theory: 
*
* Side Effects:
*
*******************************************************************************/
void `$INSTANCE_NAME`_WriteRegValue(uint8 txDataByte)
{
    CY_SET_REG8(`$INSTANCE_NAME`_SHIFT_REG_LSB_PTR, txDataByte);  
}


/*******************************************************************************
* Function Name: `$INSTANCE_NAME`_ReadRegValue
********************************************************************************
*
* Summary:
*  Directly returns current state in shift register, not data in FIFO due 
*  to Store input.
*
* Parameters:  
*  void 
*
* Return: 
*  Shift Register state
*
* Theory: 
*
* Side Effects:
*  Clears output FIFO. 
*
*******************************************************************************/
uint8 `$INSTANCE_NAME`_ReadRegValue(void)
{
    uint8 result;
	
    /* Clear FIFO before software capture */
    `$INSTANCE_NAME`_SR8_AUX_CONTROL |= `$INSTANCE_NAME`_OUT_FIFO_CLR_BIT; 
    `$INSTANCE_NAME`_SR8_AUX_CONTROL &= ~`$INSTANCE_NAME`_OUT_FIFO_CLR_BIT;
	  
	  /* Capture A1 to output FIFO */
    result = CY_GET_REG8(`$INSTANCE_NAME`_SHIFT_REG_VALUE_LSB_PTR);
    
	  /* Read output FIFO */
    result = CY_GET_REG8(`$INSTANCE_NAME`_OUT_FIFO_VAL_LSB_PTR); 
	
    if(`$INSTANCE_NAME`_SR_SIZE != 8)
    {
        result = result & `$INSTANCE_NAME`_SR_MASK;
	  }
    return(result);
}


#if (`$INSTANCE_NAME`_USE_INPUT_FIFO == 1)
/*******************************************************************************
* Function Name: `$INSTANCE_NAME`_WriteData
********************************************************************************
*
* Summary:
*  Send state to FIFO for later transfer to shift register based on the Load 
*  input
*
* Parameters:  
*  txDataByte: containing shift register state. 
*
* Return: 
*  Indicates: successful execution of function when FIFO is empty; and error when
*  FIFO is full.
*
* Theory: 
*
* Side Effects:
*
*******************************************************************************/
uint8 `$INSTANCE_NAME`_WriteData(uint8 txDataByte)
{
    uint8 result;
    
    result = CYRET_INVALID_STATE;
    
    if((`$INSTANCE_NAME`_GetFIFOStatus(`$INSTANCE_NAME`_IN_FIFO)) != `$INSTANCE_NAME`_RET_FIFO_FULL)
    {
        CY_SET_REG8(`$INSTANCE_NAME`_IN_FIFO_VAL_LSB_PTR, txDataByte);
        result = CYRET_SUCCESS;
    }
    return(result);
}


#endif
#if (`$INSTANCE_NAME`_USE_OUTPUT_FIFO == 1)
/*******************************************************************************
* Function Name: `$INSTANCE_NAME`_ReadData
********************************************************************************
*
* Summary:
*  Returns state in FIFO due to Store input.
*
* Parameters:  
*  void 
*
* Return: 
*  Shift Register state
*
* Theory: 
*
* Side Effects:
*
*******************************************************************************/
uint8 `$INSTANCE_NAME`_ReadData(void)
{
    return(CY_GET_REG8(`$INSTANCE_NAME`_OUT_FIFO_VAL_LSB_PTR));
}


#endif
#elif (`$INSTANCE_NAME`_SR_SIZE <= 16)
/*******************************************************************************
* Function Name: `$INSTANCE_NAME`_WriteRegValue
********************************************************************************
*
* Summary:
*  Send state directly to shift register
*
* Parameters:  
*  txDataByte: containing shift register state. 
*
* Return: 
*  void
*
* Theory: 
*
* Side Effects:
*
*******************************************************************************/
void `$INSTANCE_NAME`_WriteRegValue(uint16 txDataByte)
{
   CY_SET_REG16(`$INSTANCE_NAME`_SHIFT_REG_LSB_PTR, txDataByte);
}


/*******************************************************************************
* Function Name: `$INSTANCE_NAME`_ReadRegValue
********************************************************************************
*
* Summary:
*  Directly rreturns current state in shift register, not data in FIFO due 
*  to Store input.
*
* Parameters:  
*  void 
*
* Return: 
*  Shift Register state
*
* Theory: 
*
* Side Effects:
*  Clears output FIFO. 
*
*******************************************************************************/
uint16 `$INSTANCE_NAME`_ReadRegValue(void)
{
   uint16 result;
   
    /* Clear FIFO before software capture */
    `$INSTANCE_NAME`_SR16_AUX_CONTROL1 |= `$INSTANCE_NAME`_OUT_FIFO_CLR_BIT; 
    `$INSTANCE_NAME`_SR16_AUX_CONTROL1 &= ~`$INSTANCE_NAME`_OUT_FIFO_CLR_BIT;
    `$INSTANCE_NAME`_SR16_AUX_CONTROL2 |= `$INSTANCE_NAME`_OUT_FIFO_CLR_BIT; 
    `$INSTANCE_NAME`_SR16_AUX_CONTROL2 &= ~`$INSTANCE_NAME`_OUT_FIFO_CLR_BIT;
	
    /* Capture A1 to output FIFO */
    result = CY_GET_REG16(`$INSTANCE_NAME`_SHIFT_REG_VALUE_LSB_PTR);
   
    /* Read output FIFO */
    result = CY_GET_REG16(`$INSTANCE_NAME`_OUT_FIFO_VAL_LSB_PTR); 
   
    if(`$INSTANCE_NAME`_SR_SIZE != 16)
    {
        result = result & `$INSTANCE_NAME`_SR_MASK;
    }
    return(result);
}


#if (`$INSTANCE_NAME`_USE_INPUT_FIFO == 1)
/*******************************************************************************
* Function Name: `$INSTANCE_NAME`_WriteData
********************************************************************************
*
* Summary:
*  Send state to FIFO for later transfer to shift register based on the Load 
*  input
*
* Parameters:  
*  txDataByte: containing shift register state. 
*
* Return: 
*  Indicates: successful execution of function when FIFO is empty; and error when
*  FIFO is full.
*
* Theory: 
*
* Side Effects:
*  Clears output FIFO. 
*
*******************************************************************************/
uint8 `$INSTANCE_NAME`_WriteData(uint16 txDataByte)
{
    uint8 result;
    
    result = CYRET_INVALID_STATE;
    
    if((`$INSTANCE_NAME`_GetFIFOStatus(`$INSTANCE_NAME`_IN_FIFO)) != `$INSTANCE_NAME`_RET_FIFO_FULL)
    {
        CY_SET_REG16(`$INSTANCE_NAME`_IN_FIFO_VAL_LSB_PTR, txDataByte);
        result = CYRET_SUCCESS;
    }
    return(result);
}


#endif
#if (`$INSTANCE_NAME`_USE_OUTPUT_FIFO == 1)
/*******************************************************************************
* Function Name: `$INSTANCE_NAME`_ReadData
********************************************************************************
*
* Summary:
*  Returns state in FIFO due to Store input.
*
* Parameters:  
*  void 
*
* Return: 
*  Shift Register state
*
* Theory: 
*
* Side Effects:
*
*******************************************************************************/
uint16 `$INSTANCE_NAME`_ReadData(void)
{
    return( CY_GET_REG16(`$INSTANCE_NAME`_OUT_FIFO_VAL_LSB_PTR));
}


#endif
#elif (`$INSTANCE_NAME`_SR_SIZE <= 24)
/*******************************************************************************
* Function Name: `$INSTANCE_NAME`_WriteRegValue
********************************************************************************
*
* Summary:
*  Send state directly to shift register
*
* Parameters:  
*  txDataByte: containing shift register state. 
*
* Return: 
*  void
*
* Theory: 
*
* Side Effects:
*
*******************************************************************************/
void `$INSTANCE_NAME`_WriteRegValue(uint32 txDataByte)
{
    CY_SET_REG24(`$INSTANCE_NAME`_SHIFT_REG_LSB_PTR, txDataByte); 
}

/*******************************************************************************
* Function Name: `$INSTANCE_NAME`_ReadRegValue
********************************************************************************
* Summary:
*  Directly rreturns current state in shift register, not data in FIFO due 
*  to Store input.
*
* Parameters:  
*  void 
*
* Return: 
*  Shift Register state
*
* Theory: 
*
* Side Effects:
*  Clears output FIFO. 
*
*******************************************************************************/
uint32 `$INSTANCE_NAME`_ReadRegValue(void)
{
    uint32 result;
	
    /* Clear FIFO before software capture */
    `$INSTANCE_NAME`_SR24_AUX_CONTROL1 |= `$INSTANCE_NAME`_OUT_FIFO_CLR_BIT; 
    `$INSTANCE_NAME`_SR24_AUX_CONTROL1 &= ~`$INSTANCE_NAME`_OUT_FIFO_CLR_BIT;
    `$INSTANCE_NAME`_SR24_AUX_CONTROL2 |= `$INSTANCE_NAME`_OUT_FIFO_CLR_BIT; 
    `$INSTANCE_NAME`_SR24_AUX_CONTROL2 &= ~`$INSTANCE_NAME`_OUT_FIFO_CLR_BIT;
	  `$INSTANCE_NAME`_SR24_AUX_CONTROL3 |= `$INSTANCE_NAME`_OUT_FIFO_CLR_BIT; 
    `$INSTANCE_NAME`_SR24_AUX_CONTROL3 &= ~`$INSTANCE_NAME`_OUT_FIFO_CLR_BIT;
	
    /* Capture A1 to output FIFO */	
    result = CY_GET_REG24(`$INSTANCE_NAME`_SHIFT_REG_VALUE_LSB_PTR);
          
    result = CY_GET_REG24(`$INSTANCE_NAME`_OUT_FIFO_VAL_LSB_PTR); 
    
    if(`$INSTANCE_NAME`_SR_SIZE != 24)
    {
        result = result & `$INSTANCE_NAME`_SR_MASK;
	  }
		
    return result;
}


#if (`$INSTANCE_NAME`_USE_INPUT_FIFO == 1)
/*******************************************************************************
* Function Name: `$INSTANCE_NAME`_WriteData
********************************************************************************
*
* Summary:
*  Send state to FIFO for later transfer to shift register based on the Load 
*  input
*
* Parameters:  
*  txDataByte: containing shift register state. 
*
* Return: 
*  Indicates: successful execution of function when FIFO is empty; and error
*  when FIFO is full.
*
* Theory: 
*
* Side Effects:
*
*******************************************************************************/
uint8 `$INSTANCE_NAME`_WriteData(uint32 txDataByte)
{
    uint8 result;
    
    result = CYRET_INVALID_STATE;
    
    if((`$INSTANCE_NAME`_GetFIFOStatus(`$INSTANCE_NAME`_IN_FIFO)) != `$INSTANCE_NAME`_RET_FIFO_FULL)
    {
        CY_SET_REG24(`$INSTANCE_NAME`_IN_FIFO_VAL_LSB_PTR, txDataByte);
        result = CYRET_SUCCESS;
    }
    return(result);
}


#endif
#if (`$INSTANCE_NAME`_USE_OUTPUT_FIFO == 1)
/*******************************************************************************
* Function Name: `$INSTANCE_NAME`_ReadData
********************************************************************************
*
* Summary:
*   Returns state in FIFO due to Store input.
*
* Parameters:  
*  void 
*
* Return: 
*  Shift Register state
*
* Theory: 
*
* Side Effects:
*
*******************************************************************************/
uint32 `$INSTANCE_NAME`_ReadData(void)
{
    return( CY_GET_REG24(`$INSTANCE_NAME`_OUT_FIFO_VAL_LSB_PTR) );
}


#endif
#elif (`$INSTANCE_NAME`_SR_SIZE <= 32)
/*******************************************************************************
* Function Name: `$INSTANCE_NAME`_WriteRegValue
********************************************************************************
*
* Summary:
*  Send state directly to shift register
*
* Parameters:  
*  txDataByte: containing shift register state. 
*
* Return: 
*  void
*
* Theory: 
*
* Side Effects:
*
*******************************************************************************/
void `$INSTANCE_NAME`_WriteRegValue(uint32 txDataByte)
{
    CY_SET_REG32(`$INSTANCE_NAME`_SHIFT_REG_LSB_PTR, txDataByte);
}


/*******************************************************************************
* Function Name: `$INSTANCE_NAME`_ReadRegValue
********************************************************************************
*
* Summary:
*  Directly rreturns current state in shift register, not data in FIFO due 
*  to Store input.
*
* Parameters:  
*  void 
*
* Return: 
*  Shift Register state
*
* Theory: 
*
* Side Effects:
*  Clears output FIFO. 
*
*******************************************************************************/
uint32 `$INSTANCE_NAME`_ReadRegValue(void)
{
    uint32 result;
	
	/* Clear FIFO before software capture */
    `$INSTANCE_NAME`_SR32_AUX_CONTROL1 |= `$INSTANCE_NAME`_OUT_FIFO_CLR_BIT; 
    `$INSTANCE_NAME`_SR32_AUX_CONTROL1 &= ~`$INSTANCE_NAME`_OUT_FIFO_CLR_BIT;
    `$INSTANCE_NAME`_SR32_AUX_CONTROL2 |= `$INSTANCE_NAME`_OUT_FIFO_CLR_BIT; 
    `$INSTANCE_NAME`_SR32_AUX_CONTROL2 &= ~`$INSTANCE_NAME`_OUT_FIFO_CLR_BIT;
    `$INSTANCE_NAME`_SR32_AUX_CONTROL3 |= `$INSTANCE_NAME`_OUT_FIFO_CLR_BIT; 
    `$INSTANCE_NAME`_SR32_AUX_CONTROL3 &= ~`$INSTANCE_NAME`_OUT_FIFO_CLR_BIT;
    `$INSTANCE_NAME`_SR32_AUX_CONTROL4 |= `$INSTANCE_NAME`_OUT_FIFO_CLR_BIT; 
    `$INSTANCE_NAME`_SR32_AUX_CONTROL4 &= ~`$INSTANCE_NAME`_OUT_FIFO_CLR_BIT;
	
    /* Capture A1 to output FIFO */	
	  result = CY_GET_REG32(`$INSTANCE_NAME`_SHIFT_REG_VALUE_LSB_PTR);
	
    result = CY_GET_REG32(`$INSTANCE_NAME`_OUT_FIFO_VAL_LSB_PTR); 
	
    if(`$INSTANCE_NAME`_SR_SIZE != 32)
	  {
	      result = result & `$INSTANCE_NAME`_SR_MASK;
    }

    return result;
}


#if (`$INSTANCE_NAME`_USE_INPUT_FIFO == 1)
/*******************************************************************************
* Function Name: `$INSTANCE_NAME`_WriteData
********************************************************************************
*
* Summary:
*  Send state to FIFO for later transfer to shift register based on the Load 
*  input
*
* Parameters:  
*  txDataByte: containing shift register state. 
*
* Return: 
*  Indicates: successful execution of function when FIFO is empty; and error
*  when FIFO is full.
*
* Theory: 
*
* Side Effects:
*
*******************************************************************************/
uint8 `$INSTANCE_NAME`_WriteData(uint32 txDataByte)
{
    uint8 result;
    
    result = CYRET_INVALID_STATE;
    
    if((`$INSTANCE_NAME`_GetFIFOStatus(`$INSTANCE_NAME`_IN_FIFO)) != `$INSTANCE_NAME`_RET_FIFO_FULL)
    {
	      CY_SET_REG32(`$INSTANCE_NAME`_IN_FIFO_VAL_LSB_PTR, txDataByte);
        result = CYRET_SUCCESS;
    }
    return(result);
}


#endif
#if (`$INSTANCE_NAME`_USE_OUTPUT_FIFO == 1)
/*******************************************************************************
* Function Name: `$INSTANCE_NAME`_ReadData
********************************************************************************
*
* Summary:
*  Returns state in FIFO due to Store input.
*
* Parameters:  
*  void 
*
* Return: 
*  Shift Register state
*
* Theory: 
*
* Side Effects:
*
*******************************************************************************/
uint32 `$INSTANCE_NAME`_ReadData(void)
{
    return(CY_GET_REG32(`$INSTANCE_NAME`_OUT_FIFO_VAL_LSB_PTR));
}


#endif
#endif
/* [] END OF FILE */
