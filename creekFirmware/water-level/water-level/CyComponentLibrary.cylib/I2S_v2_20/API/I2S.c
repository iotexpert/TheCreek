/*******************************************************************************
* File Name: `$INSTANCE_NAME`.c  
* Version `$CY_MAJOR_VERSION`.`$CY_MINOR_VERSION`
*
* Description:
*  This file contains the setup, control and status commands for the I2S
*  component.  
*
* Note: 
*
*******************************************************************************
* Copyright 2008-2011, Cypress Semiconductor Corporation.  All rights reserved.
* You may use this file only in accordance with the license, terms, conditions, 
* disclaimers, and limitations in the end user license agreement accompanying 
* the software package with which this file was provided.
*******************************************************************************/

#include "`$INSTANCE_NAME`.h"  

uint8 `$INSTANCE_NAME`_initVar = 0u;


/*******************************************************************************
* Function Name: `$INSTANCE_NAME`_Enable
********************************************************************************
*
* Summary:
*  Enables I2S interface.
*
* Parameters:
*  None.
*
* Return:
*  None.
*
*******************************************************************************/
void `$INSTANCE_NAME`_Enable(void) `=ReentrantKeil($INSTANCE_NAME . "_Enable")`
{  
    uint8 enableInterrupts;
       
    /* Enter critical section */
    enableInterrupts = CyEnterCriticalSection();
    `$INSTANCE_NAME`_AUX_CONTROL_REG |= `$INSTANCE_NAME`_CNTR7_EN;
    /* Exit critical section */
    CyExitCriticalSection(enableInterrupts);    

    `$INSTANCE_NAME`_CONTROL_REG |= `$INSTANCE_NAME`_EN; 
}


/*******************************************************************************
* Function Name: `$INSTANCE_NAME`_Init
********************************************************************************
*
* Summary:
*  Initializes Tx and/or Rx interrupt sources with initial values.
*
* Parameters:
*  None.
*
* Return:
*  None.
*
*******************************************************************************/
void `$INSTANCE_NAME`_Init(void) `=ReentrantKeil($INSTANCE_NAME . "_Init")`
{
    /* Writes Tx and/or Rx interrupt source values provided for customizer */
    #if ((`$INSTANCE_NAME`_DIRECTION == `$INSTANCE_NAME`__TX) || \
        (`$INSTANCE_NAME`_DIRECTION == `$INSTANCE_NAME`__RX_AND_TX))
        `$INSTANCE_NAME`_SetTxInterruptMode(`$INSTANCE_NAME`_DEFAULT_INT_SOURCE);
    #endif /* `$INSTANCE_NAME`_DIRECTION == `$INSTANCE_NAME`__TX */

    #if ((`$INSTANCE_NAME`_DIRECTION == `$INSTANCE_NAME`__RX) || \
        (`$INSTANCE_NAME`_DIRECTION == `$INSTANCE_NAME`__RX_AND_TX))
        `$INSTANCE_NAME`_SetRxInterruptMode(`$INSTANCE_NAME`_DEFAULT_INT_SOURCE >> 3);
    #endif /* `$INSTANCE_NAME`_DIRECTION == `$INSTANCE_NAME`__RX */
}


/*******************************************************************************
* Function Name: `$INSTANCE_NAME`_Start
********************************************************************************
*
* Summary:
*  Starts the I2S interface.
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
void `$INSTANCE_NAME`_Start(void)
{
    /* Writes Tx and/or Rx interrupt source values provided for customizer */
    if (`$INSTANCE_NAME`_initVar == 0u)
    {
        `$INSTANCE_NAME`_Init();
        `$INSTANCE_NAME`_initVar = 1u;
    }
      
    `$INSTANCE_NAME`_Enable();
}


/*******************************************************************************
* Function Name: `$INSTANCE_NAME`_Stop
********************************************************************************
*
* Summary:
*  Disables the I2S interface.
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
    uint8 enableInterrupts;
    
    `$INSTANCE_NAME`_CONTROL_REG &= ~`$INSTANCE_NAME`_EN;    
    /* Enter critical section */
    enableInterrupts = CyEnterCriticalSection();
    `$INSTANCE_NAME`_AUX_CONTROL_REG &= ~`$INSTANCE_NAME`_CNTR7_EN;
    /* Exit critical section */
    CyExitCriticalSection(enableInterrupts); 
    
    #if ((`$INSTANCE_NAME`_DIRECTION == `$INSTANCE_NAME`__TX) || \
        (`$INSTANCE_NAME`_DIRECTION == `$INSTANCE_NAME`__RX_AND_TX))
        `$INSTANCE_NAME`_DisableTx();
        `$INSTANCE_NAME`_ClearTxFIFO();
    #endif /* `$INSTANCE_NAME`_DIRECTION == `$INSTANCE_NAME`__TX */

    #if ((`$INSTANCE_NAME`_DIRECTION == `$INSTANCE_NAME`__RX) || \
        (`$INSTANCE_NAME`_DIRECTION == `$INSTANCE_NAME`__RX_AND_TX))
        `$INSTANCE_NAME`_DisableRx();
        `$INSTANCE_NAME`_ClearRxFIFO();
    #endif /* `$INSTANCE_NAME`_DIRECTION == `$INSTANCE_NAME`__RX */
}

/* Implementation of TX Direction */ 
#if ((`$INSTANCE_NAME`_DIRECTION == `$INSTANCE_NAME`__TX) || \
    (`$INSTANCE_NAME`_DIRECTION == `$INSTANCE_NAME`__RX_AND_TX))

    /*******************************************************************************
    * Function Name: `$INSTANCE_NAME`_EnableTx
    ********************************************************************************
    *
    * Summary:
    *  Enables the Tx direction of the I2S interface.  At the next word
    *  select falling edge transmission will begin.
    *
    * Parameters:
    *  None.
    *
    * Return:
    *  None.
    *
    *******************************************************************************/
    void `$INSTANCE_NAME`_EnableTx(void) `=ReentrantKeil($INSTANCE_NAME . "_EnableTx")`
    {
        uint8 enableInterrupts;
        
        `$INSTANCE_NAME`_CONTROL_REG |= `$INSTANCE_NAME`_TX_EN;     
        /* Enter critical section */
        enableInterrupts = CyEnterCriticalSection();    
        `$INSTANCE_NAME`_TX_STATUS_AUX_CONTROL_REG |= `$INSTANCE_NAME`_TX_INT_EN;
        /* Exit critical section */
        CyExitCriticalSection(enableInterrupts);
    }
        
        
    /*******************************************************************************
    * Function Name: `$INSTANCE_NAME`_DisableTx
    ********************************************************************************
    *
    * Summary:
    *  Disables the Tx direction of the I2S interface.  At the next word 
    *  select falling edge transmission of data will stop and a constant 0 value 
    *  will be transmitted.
    *
    * Parameters:
    *  None.
    *
    * Return:
    *  None.
    *
    *******************************************************************************/
    void `$INSTANCE_NAME`_DisableTx(void) `=ReentrantKeil($INSTANCE_NAME . "_DisableTx")`
    {
        uint8 enableInterrupts;
        
        `$INSTANCE_NAME`_CONTROL_REG &= ~`$INSTANCE_NAME`_TX_EN;        
        /* Enter critical section */
        enableInterrupts = CyEnterCriticalSection();
        `$INSTANCE_NAME`_TX_STATUS_AUX_CONTROL_REG &= ~`$INSTANCE_NAME`_TX_INT_EN;
        /* Exit critical section */
        CyExitCriticalSection(enableInterrupts);
    }
        
        
    /*******************************************************************************
    * Function Name: `$INSTANCE_NAME`_SetTxInterruptMode
    ********************************************************************************
    *
    * Summary:
    *  Sets the interrupt source for the I2S Tx direction interrupt.
    *
    * Parameters:
    *  Byte containing the constant for the selected interrupt sources.
    *   `$INSTANCE_NAME`_TX_FIFO_UNDEFLOW         
    *   `$INSTANCE_NAME`_TX_FIFO_0_NOT_FULL        
    *   `$INSTANCE_NAME`_TX_FIFO_1_NOT_FULL       
    *
    * Return:
    *  None
    *
    *******************************************************************************/
    void `$INSTANCE_NAME`_SetTxInterruptMode(uint8 interruptSource) \
                                                                `=ReentrantKeil($INSTANCE_NAME . "_SetTxInterruptMode")`
    {
        `$INSTANCE_NAME`_TX_STATUS_MASK_REG = (interruptSource & `$INSTANCE_NAME`_TX_ST_MASK); 
    }
        
        
    /*******************************************************************************
    * Function Name: `$INSTANCE_NAME`_ReadTxStatus
    ********************************************************************************
    *
    * Summary:
    *  Returns state in the I2S Tx status register.
    *
    * Parameters:
    *  None.
    *
    * Return:
    *  State of the I2S Tx status register
    *   `$INSTANCE_NAME`_RET_TX_FIFO_UNDEFLOW (Clear on Read)    
    *   `$INSTANCE_NAME`_RET_TX_FIFO_0_NOT_FULL    
    *   `$INSTANCE_NAME`_RET_TX_FIFO_1_NOT_FULL   
    *
    * Side Effects: 
    *  Clears the bits of I2S Tx status register that are Clear on Read.
    *
    *******************************************************************************/
    uint8 `$INSTANCE_NAME`_ReadTxStatus(void) `=ReentrantKeil($INSTANCE_NAME . "_ReadTxStatus")`
    {
        return (`$INSTANCE_NAME`_TX_STATUS_REG & `$INSTANCE_NAME`_TX_ST_MASK);
    }
        
        
    /*******************************************************************************
    * Function Name: `$INSTANCE_NAME`_WriteByte
    ********************************************************************************
    *
    * Summary:
    *  Writes a single byte into the Tx FIFO.
    *
    * Parameters:
    *  wrData: Byte containing the data to transmit.
    *  wordSelect: Indicates to write to the Left (0) or Right (1) channel.  In the 
    *              interleaved mode this parameter is ignored
    *
    * Return:
    *  None.
    * 
    * Reentrant:
    *  No.
    *
    *******************************************************************************/
    void `$INSTANCE_NAME`_WriteByte(uint8 wrData, uint8 wordSelect)
    {        
        #if(`$INSTANCE_NAME`_TX_DATA_INTERLEAVING == `$INSTANCE_NAME`__INTERLEAVED)
            
            wordSelect = wordSelect; /* Supress compiler warning */
            
            `$INSTANCE_NAME`_TX_FIFO_0_REG = wrData;
            
        #else
            
            wordSelect &= `$INSTANCE_NAME`_WORD_SEL_MASK;
            
            if(wordSelect == `$INSTANCE_NAME`_TX_LEFT_CHANNEL)
            {
                `$INSTANCE_NAME`_TX_FIFO_0_REG = wrData; 
            }
            else
            {
                `$INSTANCE_NAME`_TX_FIFO_1_REG = wrData;    
            }
            
        #endif /* `$INSTANCE_NAME`_RX_DATA_INTERLEAVING == `$INSTANCE_NAME`__INTERLEAVED */
    }
        
        
    /*******************************************************************************
    * Function Name: `$INSTANCE_NAME`_ClearTxFIFO
    ********************************************************************************
    *
    * Summary:
    *  Clears out the Tx FIFO.  Any data present in the FIFO will not be
    *  sent. This call should be made only when the Tx direction is disabled.
    *
    * Parameters:
    *  None.
    *
    * Return:
    *  None.
    *
    *******************************************************************************/
    void `$INSTANCE_NAME`_ClearTxFIFO(void) `=ReentrantKeil($INSTANCE_NAME . "_ClearTxFIFO")`
    {
        uint8 enableInterrupts;
        
        /* Enter critical section */
        enableInterrupts = CyEnterCriticalSection();        
        `$INSTANCE_NAME`_TX_AUX_CONTROL_REG |=  `$INSTANCE_NAME`_TX_FIFO_0_CLR;
        `$INSTANCE_NAME`_TX_AUX_CONTROL_REG &= ~`$INSTANCE_NAME`_TX_FIFO_0_CLR;
        
        #if(`$INSTANCE_NAME`_TX_DATA_INTERLEAVING == `$INSTANCE_NAME`__SEPARATE)
            `$INSTANCE_NAME`_TX_AUX_CONTROL_REG |=  `$INSTANCE_NAME`_TX_FIFO_1_CLR;
            `$INSTANCE_NAME`_TX_AUX_CONTROL_REG &= ~`$INSTANCE_NAME`_TX_FIFO_1_CLR;    
        #endif /* `$INSTANCE_NAME`_RX_DATA_INTERLEAVING == `$INSTANCE_NAME`__SEPARATE */ 
        /* Exit critical section */
        CyExitCriticalSection(enableInterrupts);
    }

#endif /* `$INSTANCE_NAME`_DIRECTION == `$INSTANCE_NAME`__TX */ 

/* Implementation of RX Direction */
#if ((`$INSTANCE_NAME`_DIRECTION == `$INSTANCE_NAME`__RX) || \
    (`$INSTANCE_NAME`_DIRECTION == `$INSTANCE_NAME`__RX_AND_TX)) 

    /*******************************************************************************
    * Function Name: `$INSTANCE_NAME`_EnableRx
    ********************************************************************************
    *
    * Summary:
    *  Enables the Rx direction of the I2S interface.  At the next word
    *  select falling edge reception of data will begin.
    *
    * Parameters:
    *  None.
    *
    * Return:
    *  None.
    *
    *******************************************************************************/
    void `$INSTANCE_NAME`_EnableRx(void) `=ReentrantKeil($INSTANCE_NAME . "_EnableRx")`
    {
        uint8 enableInterrupts;
        
        `$INSTANCE_NAME`_CONTROL_REG |= `$INSTANCE_NAME`_RX_EN;
        /* Enter critical section */
        enableInterrupts = CyEnterCriticalSection(); 
        `$INSTANCE_NAME`_RX_STATUS_AUX_CONTROL_REG |= `$INSTANCE_NAME`_RX_INT_EN;
        /* Exit critical section */
        CyExitCriticalSection(enableInterrupts);
    }
        
        
    /*******************************************************************************
    * Function Name: `$INSTANCE_NAME`_DisableRx
    ********************************************************************************
    *
    * Summary:
    *  Disables the Rx direction of the I2S interface.
    *
    * Parameters:
    *  None.
    *
    * Return:
    *  None.
    *
    *******************************************************************************/
    void `$INSTANCE_NAME`_DisableRx(void) `=ReentrantKeil($INSTANCE_NAME . "_DisableRx")`
    {
        uint8 enableInterrupts;
        
        `$INSTANCE_NAME`_CONTROL_REG &= ~`$INSTANCE_NAME`_RX_EN;
        /* Enter critical section */
        enableInterrupts = CyEnterCriticalSection();         
        `$INSTANCE_NAME`_RX_STATUS_AUX_CONTROL_REG &= ~`$INSTANCE_NAME`_RX_INT_EN;
        /* Exit critical section */
        CyExitCriticalSection(enableInterrupts);
    }
        
        
    /*******************************************************************************
    * Function Name: `$INSTANCE_NAME`_SetRxInterruptMode
    ********************************************************************************
    *
    * Summary:
    *  Sets the interrupt source for the I2S Rx direction interrupt.
    *
    * Parameters:
    *  Byte containing the constant for the selected interrupt sources.
    *   `$INSTANCE_NAME`_RX_FIFO_OVERFLOW          
    *   `$INSTANCE_NAME`_RX_FIFO_0_NOT_EMPTY       
    *   `$INSTANCE_NAME`_RX_FIFO_1_NOT_EMPTY       
    *
    * Return:
    *  None.
    *
    *******************************************************************************/
    void `$INSTANCE_NAME`_SetRxInterruptMode(uint8 interruptSource) \
                                                                `=ReentrantKeil($INSTANCE_NAME . "_SetRxInterruptMode")`
    {
        `$INSTANCE_NAME`_RX_STATUS_MASK_REG = (interruptSource & `$INSTANCE_NAME`_RX_ST_MASK); 
    }
        
        
    /*******************************************************************************
    * Function Name: `$INSTANCE_NAME`_ReadRxStatus
    ********************************************************************************
    *
    * Summary:
    *  Returns state in the I2S Rx status register.
    *
    * Parameters:
    *  None.
    *
    * Return:
    *  State of the I2S Rx status register
    *   `$INSTANCE_NAME`_RET_RX_FIFO_OVERFLOW (Clear on Read)     
    *   `$INSTANCE_NAME`_RET_RX_FIFO_0_NOT_EMPTY   
    *   `$INSTANCE_NAME`_RET_RX_FIFO_1_NOT_EMPTY   
    *
    * Side Effects: 
    *  Clears the bits of I2S Rx status register that are Clear on Read.
    *
    *******************************************************************************/
    uint8 `$INSTANCE_NAME`_ReadRxStatus(void) `=ReentrantKeil($INSTANCE_NAME . "_ReadRxStatus")`
    {
        return (`$INSTANCE_NAME`_RX_STATUS_REG & `$INSTANCE_NAME`_RX_ST_MASK);
    }
        
        
    /*******************************************************************************
    * Function Name: `$INSTANCE_NAME`_ReadByte
    ********************************************************************************
    *
    * Summary:
    *  Returns a single byte from the Rx FIFO.
    *
    * Parameters:
    *  wordSelect: Indicates to read from the Left (0) or Right (1) channel. In the 
    *              interleaved mode this parameter is ignored.
    *
    * Return:
    *  Byte containing the data received.
    * 
    * Reentrant:
    *  No.
    *
    *******************************************************************************/
    uint8 `$INSTANCE_NAME`_ReadByte(uint8 wordSelect)
    {               
        #if(`$INSTANCE_NAME`_RX_DATA_INTERLEAVING == `$INSTANCE_NAME`__INTERLEAVED)
            
            wordSelect = wordSelect; /* Supress compiler warning */
            
            return(`$INSTANCE_NAME`_RX_FIFO_0_REG);
            
        #else
           
            uint8 result;
            
            wordSelect &= `$INSTANCE_NAME`_WORD_SEL_MASK;
            
            if(wordSelect == `$INSTANCE_NAME`_RX_LEFT_CHANNEL)
            {
                result = `$INSTANCE_NAME`_RX_FIFO_0_REG; 
            }
            else
            {
                result = `$INSTANCE_NAME`_RX_FIFO_1_REG;    
            }
            
            return (result);
            
        #endif /* `$INSTANCE_NAME`_RX_DATA_INTERLEAVING == `$INSTANCE_NAME`__INTERLEAVED */
    }
        
        
    /*******************************************************************************
    * Function Name: `$INSTANCE_NAME`_ClearRxFIFO
    ********************************************************************************
    *
    * Summary:
    *  Clears out the Rx FIFO.  Any data present in the FIFO will be lost.
    *  This call should be made only when the Rx direction is disabled.
    *
    * Parameters:
    *  None.
    *
    * Return:
    *  None.
    * 
    *******************************************************************************/
    void `$INSTANCE_NAME`_ClearRxFIFO(void) `=ReentrantKeil($INSTANCE_NAME . "_ClearRxFIFO")`
    {
        uint8 enableInterrupts;
    
        /* Enter critical section */
        enableInterrupts = CyEnterCriticalSection();
        `$INSTANCE_NAME`_RX_AUX_CONTROL_REG |=  `$INSTANCE_NAME`_RX_FIFO_0_CLR;
        `$INSTANCE_NAME`_RX_AUX_CONTROL_REG &= ~`$INSTANCE_NAME`_RX_FIFO_0_CLR;
        
        #if(`$INSTANCE_NAME`_RX_DATA_INTERLEAVING == `$INSTANCE_NAME`__SEPARATE)
            `$INSTANCE_NAME`_RX_AUX_CONTROL_REG |=  `$INSTANCE_NAME`_RX_FIFO_1_CLR;
            `$INSTANCE_NAME`_RX_AUX_CONTROL_REG &= ~`$INSTANCE_NAME`_RX_FIFO_1_CLR;    
        #endif /* `$INSTANCE_NAME`_RX_DATA_INTERLEAVING == `$INSTANCE_NAME`__SEPARATE */
        /* Exit critical section */
        CyExitCriticalSection(enableInterrupts);
    }

#endif /* `$INSTANCE_NAME`_DIRECTION == `$INSTANCE_NAME`__RX */


/* [] END OF FILE */
