/*******************************************************************************
* File Name: `$INSTANCE_NAME`.c
* Version `$CY_MAJOR_VERSION`.`$CY_MINOR_VERSION`
*
* Description:
*  This file provides all API functionality of the UART component
*
* Note:
*
********************************************************************************
* Copyright 2008-2012, Cypress Semiconductor Corporation.  All rights reserved.
* You may use this file only in accordance with the license, terms, conditions, 
* disclaimers, and limitations in the end user license agreement accompanying 
* the software package with which this file was provided.
*******************************************************************************/

#include "`$INSTANCE_NAME`.h"
#include "CyLib.h"


/***************************************
* Global data allocation
***************************************/

#if( `$INSTANCE_NAME`_TX_ENABLED && (`$INSTANCE_NAME`_TXBUFFERSIZE > `$INSTANCE_NAME`_FIFO_LENGTH))
    volatile uint8 `$INSTANCE_NAME`_txBuffer[`$INSTANCE_NAME`_TXBUFFERSIZE];
    volatile `$TxBuffRegSizeReplacementString` `$INSTANCE_NAME`_txBufferRead = 0u;
    `$TxBuffRegSizeReplacementString` `$INSTANCE_NAME`_txBufferWrite = 0u;
#endif /* End `$INSTANCE_NAME`_TX_ENABLED */
#if( ( `$INSTANCE_NAME`_RX_ENABLED || `$INSTANCE_NAME`_HD_ENABLED ) && \
     (`$INSTANCE_NAME`_RXBUFFERSIZE > `$INSTANCE_NAME`_FIFO_LENGTH) )
    volatile uint8 `$INSTANCE_NAME`_rxBuffer[`$INSTANCE_NAME`_RXBUFFERSIZE];
    volatile `$RxBuffRegSizeReplacementString` `$INSTANCE_NAME`_rxBufferRead = 0u;
    volatile `$RxBuffRegSizeReplacementString` `$INSTANCE_NAME`_rxBufferWrite = 0u;
    volatile uint8 `$INSTANCE_NAME`_rxBufferLoopDetect = 0u;
    volatile uint8 `$INSTANCE_NAME`_rxBufferOverflow = 0u;
    #if (`$INSTANCE_NAME`_RXHW_ADDRESS_ENABLED)
        volatile uint8 `$INSTANCE_NAME`_rxAddressMode = `$INSTANCE_NAME`_RXADDRESSMODE;
        volatile uint8 `$INSTANCE_NAME`_rxAddressDetected = 0u;
    #endif /* End EnableHWAddress */    
#endif /* End `$INSTANCE_NAME`_RX_ENABLED */


/***************************************
* Local data allocation
***************************************/

uint8 `$INSTANCE_NAME`_initVar = 0u;


/*******************************************************************************
* Function Name: `$INSTANCE_NAME`_Start
********************************************************************************
*
* Summary:
*  Initialize and Enable the UART component.
*  Enable the clock input to enable operation.
*
* Parameters:
*  None.
*
* Return:
*  None.
*
* Global variables:
*  The `$INSTANCE_NAME`_intiVar variable is used to indicate initial 
*  configuration of this component. The variable is initialized to zero (0u) 
*  and set to one (1u) the first time UART_Start() is called. This allows for 
*  component initialization without re-initialization in all subsequent calls 
*  to the `$INSTANCE_NAME`_Start() routine. 
*
* Reentrant:
*  No.
*
*******************************************************************************/
void `$INSTANCE_NAME`_Start(void) `=ReentrantKeil($INSTANCE_NAME . "_Start")`
{
    /* If not Initialized then initialize all required hardware and software */
    if(`$INSTANCE_NAME`_initVar == 0u)
    {
        `$INSTANCE_NAME`_Init();
        `$INSTANCE_NAME`_initVar = 1u;
    }
    `$INSTANCE_NAME`_Enable();
}


/*******************************************************************************
* Function Name: `$INSTANCE_NAME`_Init
********************************************************************************
*
* Summary:
*  Initialize component's parameters to the parameters set by user in the
*  customizer of the component placed onto schematic. Usually called in
*  `$INSTANCE_NAME`_Start().
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
    #if(`$INSTANCE_NAME`_RX_ENABLED || `$INSTANCE_NAME`_HD_ENABLED)

        #if(`$INSTANCE_NAME`_RX_INTERRUPT_ENABLED && (`$INSTANCE_NAME`_RXBUFFERSIZE > `$INSTANCE_NAME`_FIFO_LENGTH))
            /* Set the RX Interrupt. */
            CyIntSetVector(`$INSTANCE_NAME`_RX_VECT_NUM,   `$INSTANCE_NAME`_RXISR);
            CyIntSetPriority(`$INSTANCE_NAME`_RX_VECT_NUM, `$INSTANCE_NAME`_RX_PRIOR_NUM);
        #endif /* End `$INSTANCE_NAME`_RX_INTERRUPT_ENABLED */

        #if (`$INSTANCE_NAME`_RXHW_ADDRESS_ENABLED)
            `$INSTANCE_NAME`_SetRxAddressMode(`$INSTANCE_NAME`_RXAddressMode);
            `$INSTANCE_NAME`_SetRxAddress1(`$INSTANCE_NAME`_RXHWADDRESS1);
            `$INSTANCE_NAME`_SetRxAddress2(`$INSTANCE_NAME`_RXHWADDRESS2);
        #endif /* End `$INSTANCE_NAME`_RXHW_ADDRESS_ENABLED */

        /* Configure the Initial RX interrupt mask */
        `$INSTANCE_NAME`_RXSTATUS_MASK_REG  = `$INSTANCE_NAME`_INIT_RX_INTERRUPTS_MASK;
    #endif /* End `$INSTANCE_NAME`_RX_ENABLED || `$INSTANCE_NAME`_HD_ENABLED*/

    #if(`$INSTANCE_NAME`_TX_ENABLED)
        #if(`$INSTANCE_NAME`_TX_INTERRUPT_ENABLED && (`$INSTANCE_NAME`_TXBUFFERSIZE > `$INSTANCE_NAME`_FIFO_LENGTH))
            /* Set the TX Interrupt. */
            CyIntSetVector(`$INSTANCE_NAME`_TX_VECT_NUM,   `$INSTANCE_NAME`_TXISR);
            CyIntSetPriority(`$INSTANCE_NAME`_TX_VECT_NUM, `$INSTANCE_NAME`_TX_PRIOR_NUM);
        #endif /* End `$INSTANCE_NAME`_TX_INTERRUPT_ENABLED */

        /* Write Counter Value for TX Bit Clk Generator*/
        #if(`$INSTANCE_NAME`_TXCLKGEN_DP)
            `$INSTANCE_NAME`_TXBITCLKGEN_CTR_REG = `$INSTANCE_NAME`_BIT_CENTER;
            `$INSTANCE_NAME`_TXBITCLKTX_COMPLETE_REG = (`$INSTANCE_NAME`_NUMBER_OF_DATA_BITS + \
                                                    `$INSTANCE_NAME`_NUMBER_OF_START_BIT) * \
                                                    `$INSTANCE_NAME`_OVER_SAMPLE_COUNT;
        #else
            `$INSTANCE_NAME`_TXBITCTR_COUNTER_REG = (`$INSTANCE_NAME`_NUMBER_OF_DATA_BITS + \
                                                    `$INSTANCE_NAME`_NUMBER_OF_START_BIT) * \
                                                    `$INSTANCE_NAME`_OVER_SAMPLE_8;
        #endif /* End `$INSTANCE_NAME`_TXCLKGEN_DP */

        /* Configure the Initial TX interrupt mask */
        #if(`$INSTANCE_NAME`_TX_INTERRUPT_ENABLED && (`$INSTANCE_NAME`_TXBUFFERSIZE > `$INSTANCE_NAME`_FIFO_LENGTH))
            `$INSTANCE_NAME`_TXSTATUS_MASK_REG = `$INSTANCE_NAME`_TX_STS_FIFO_EMPTY;
        #else
            `$INSTANCE_NAME`_TXSTATUS_MASK_REG = `$INSTANCE_NAME`_INIT_TX_INTERRUPTS_MASK;
        #endif /*End `$INSTANCE_NAME`_TX_INTERRUPT_ENABLED*/
        
    #endif /* End `$INSTANCE_NAME`_TX_ENABLED */

    #if(`$INSTANCE_NAME`_PARITY_TYPE_SW)  /* Write Parity to Control Register */
        `$INSTANCE_NAME`_WriteControlRegister( \
            (`$INSTANCE_NAME`_ReadControlRegister() & ~`$INSTANCE_NAME`_CTRL_PARITY_TYPE_MASK) | \
            (`$INSTANCE_NAME`_PARITY_TYPE << `$INSTANCE_NAME`_CTRL_PARITY_TYPE0_SHIFT) );
    #endif /* End `$INSTANCE_NAME`_PARITY_TYPE_SW */
}


/*******************************************************************************
* Function Name: `$INSTANCE_NAME`_Enable
********************************************************************************
*
* Summary:
*  Enables the UART block operation
*
* Parameters:
*  None.
*
* Return:
*  None.
*
* Global Variables:
*  `$INSTANCE_NAME`_rxAddressDetected - set to initial state (0).
*
*******************************************************************************/
void `$INSTANCE_NAME`_Enable(void) `=ReentrantKeil($INSTANCE_NAME . "_Enable")`
{
    uint8 enableInterrupts;
    enableInterrupts = CyEnterCriticalSection();
    
    #if(`$INSTANCE_NAME`_RX_ENABLED || `$INSTANCE_NAME`_HD_ENABLED)
        /*RX Counter (Count7) Enable */
        `$INSTANCE_NAME`_RXBITCTR_CONTROL_REG |= `$INSTANCE_NAME`_CNTR_ENABLE;
        /* Enable the RX Interrupt. */
        `$INSTANCE_NAME`_RXSTATUS_ACTL_REG  |= `$INSTANCE_NAME`_INT_ENABLE;
        #if(`$INSTANCE_NAME`_RX_INTERRUPT_ENABLED && (`$INSTANCE_NAME`_RXBUFFERSIZE > `$INSTANCE_NAME`_FIFO_LENGTH))
            CyIntEnable(`$INSTANCE_NAME`_RX_VECT_NUM);
            #if (`$INSTANCE_NAME`_RXHW_ADDRESS_ENABLED)
                `$INSTANCE_NAME`_rxAddressDetected = 0u;
            #endif /* End `$INSTANCE_NAME`_RXHW_ADDRESS_ENABLED */
        #endif /* End `$INSTANCE_NAME`_RX_INTERRUPT_ENABLED */
    #endif /* End `$INSTANCE_NAME`_RX_ENABLED || `$INSTANCE_NAME`_HD_ENABLED*/

    #if(`$INSTANCE_NAME`_TX_ENABLED)
        /*TX Counter (DP/Count7) Enable */
        #if(!`$INSTANCE_NAME`_TXCLKGEN_DP)
            `$INSTANCE_NAME`_TXBITCTR_CONTROL_REG |= `$INSTANCE_NAME`_CNTR_ENABLE;
        #endif /* End `$INSTANCE_NAME`_TXCLKGEN_DP */
        /* Enable the TX Interrupt. */
        `$INSTANCE_NAME`_TXSTATUS_ACTL_REG |= `$INSTANCE_NAME`_INT_ENABLE;
        #if(`$INSTANCE_NAME`_TX_INTERRUPT_ENABLED && (`$INSTANCE_NAME`_TXBUFFERSIZE > `$INSTANCE_NAME`_FIFO_LENGTH))
            CyIntEnable(`$INSTANCE_NAME`_TX_VECT_NUM);
        #endif /* End `$INSTANCE_NAME`_TX_INTERRUPT_ENABLED*/
     #endif /* End `$INSTANCE_NAME`_TX_ENABLED */

    #if(`$INSTANCE_NAME`_INTERNAL_CLOCK_USED)
        /* Set the bit to enable the clock. */
        `$INSTANCE_NAME`_INTCLOCK_CLKEN_REG |= `$INSTANCE_NAME`_INTCLOCK_CLKEN_MASK;
    #endif /* End `$INSTANCE_NAME`_INTERNAL_CLOCK_USED */
    
    CyExitCriticalSection(enableInterrupts);
}


/*******************************************************************************
* Function Name: `$INSTANCE_NAME`_Stop
********************************************************************************
*
* Summary:
*  Disable the UART component
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
    enableInterrupts = CyEnterCriticalSection();

    /*Write Bit Counter Disable */
    #if(`$INSTANCE_NAME`_RX_ENABLED || `$INSTANCE_NAME`_HD_ENABLED)
        `$INSTANCE_NAME`_RXBITCTR_CONTROL_REG &= ~`$INSTANCE_NAME`_CNTR_ENABLE;
    #endif /* End `$INSTANCE_NAME`_RX_ENABLED */

    #if(`$INSTANCE_NAME`_TX_ENABLED)
        #if(!`$INSTANCE_NAME`_TXCLKGEN_DP)
            `$INSTANCE_NAME`_TXBITCTR_CONTROL_REG &= ~`$INSTANCE_NAME`_CNTR_ENABLE;
        #endif /* End `$INSTANCE_NAME`_TXCLKGEN_DP */
    #endif /* `$INSTANCE_NAME`_TX_ENABLED */

    #if(`$INSTANCE_NAME`_INTERNAL_CLOCK_USED)
        /* Clear the bit to enable the clock. */
        `$INSTANCE_NAME`_INTCLOCK_CLKEN_REG &= ~`$INSTANCE_NAME`_INTCLOCK_CLKEN_MASK;
    #endif /* End `$INSTANCE_NAME`_INTERNAL_CLOCK_USED */
    
    /*Disable internal interrupt component*/
    #if(`$INSTANCE_NAME`_RX_ENABLED || `$INSTANCE_NAME`_HD_ENABLED)
        `$INSTANCE_NAME`_RXSTATUS_ACTL_REG  &= ~`$INSTANCE_NAME`_INT_ENABLE;
        #if(`$INSTANCE_NAME`_RX_INTERRUPT_ENABLED && (`$INSTANCE_NAME`_RXBUFFERSIZE > `$INSTANCE_NAME`_FIFO_LENGTH))
            `$INSTANCE_NAME`_DisableRxInt();
        #endif /* End `$INSTANCE_NAME`_RX_INTERRUPT_ENABLED */
    #endif /* End `$INSTANCE_NAME`_RX_ENABLED */
    
    #if(`$INSTANCE_NAME`_TX_ENABLED)
        `$INSTANCE_NAME`_TXSTATUS_ACTL_REG &= ~`$INSTANCE_NAME`_INT_ENABLE;
        #if(`$INSTANCE_NAME`_TX_INTERRUPT_ENABLED && (`$INSTANCE_NAME`_TXBUFFERSIZE > `$INSTANCE_NAME`_FIFO_LENGTH))
            `$INSTANCE_NAME`_DisableTxInt();
        #endif /* End `$INSTANCE_NAME`_TX_INTERRUPT_ENABLED */
    #endif /* End `$INSTANCE_NAME`_TX_ENABLED */

    CyExitCriticalSection(enableInterrupts);
}


/*******************************************************************************
* Function Name: `$INSTANCE_NAME`_ReadControlRegister
********************************************************************************
*
* Summary:
*  Read the current state of the control register
*
* Parameters:
*  None.
*
* Return:
*  Current state of the control register.
*
*******************************************************************************/
uint8 `$INSTANCE_NAME`_ReadControlRegister(void) `=ReentrantKeil($INSTANCE_NAME . "_ReadControlRegister")`
{
    #if( `$INSTANCE_NAME`_CONTROL_REG_REMOVED )
        return(0u);
    #else
        return(`$INSTANCE_NAME`_CONTROL_REG);
    #endif /* End `$INSTANCE_NAME`_CONTROL_REG_REMOVED */
}


/*******************************************************************************
* Function Name: `$INSTANCE_NAME`_WriteControlRegister
********************************************************************************
*
* Summary:
*  Writes an 8-bit value into the control register
*
* Parameters:
*  control:  control register value
*
* Return:
*  None.
*
*******************************************************************************/
void  `$INSTANCE_NAME`_WriteControlRegister(uint8 control) `=ReentrantKeil($INSTANCE_NAME . "_WriteControlRegister")`
{
    #if( `$INSTANCE_NAME`_CONTROL_REG_REMOVED )
        control = control;      /* Reassigning to release compiler warning */ 
    #else
       `$INSTANCE_NAME`_CONTROL_REG = control;
    #endif /* End `$INSTANCE_NAME`_CONTROL_REG_REMOVED */
}


#if(`$INSTANCE_NAME`_RX_ENABLED || `$INSTANCE_NAME`_HD_ENABLED)

    #if(`$INSTANCE_NAME`_RX_INTERRUPT_ENABLED)

        /*******************************************************************************
        * Function Name: `$INSTANCE_NAME`_EnableRxInt
        ********************************************************************************
        *
        * Summary:
        *  Enable RX interrupt generation
        *
        * Parameters:
        *  None.
        *
        * Return:
        *  None.
        *
        * Theory:
        *  Enable the interrupt output -or- the interrupt component itself
        *
        *******************************************************************************/
        void `$INSTANCE_NAME`_EnableRxInt(void) `=ReentrantKeil($INSTANCE_NAME . "_EnableRxInt")`
        {
            CyIntEnable(`$INSTANCE_NAME`_RX_VECT_NUM);
        }


        /*******************************************************************************
        * Function Name: `$INSTANCE_NAME`_DisableRxInt
        ********************************************************************************
        *
        * Summary:
        *  Disable RX interrupt generation
        *
        * Parameters:
        *  None.
        *
        * Return:
        *  None.
        *
        * Theory:
        *  Disable the interrupt output -or- the interrupt component itself
        *
        *******************************************************************************/
        void `$INSTANCE_NAME`_DisableRxInt(void) `=ReentrantKeil($INSTANCE_NAME . "_DisableRxInt")`
        {
            CyIntDisable(`$INSTANCE_NAME`_RX_VECT_NUM);
        }

    #endif /* `$INSTANCE_NAME`_RX_INTERRUPT_ENABLED */


    /*******************************************************************************
    * Function Name: `$INSTANCE_NAME`_SetRxInterruptMode
    ********************************************************************************
    *
    * Summary:
    *  Configure which status bits trigger an interrupt event
    *
    * Parameters:
    *  IntSrc:  An or'd combination of the desired status bit masks (defined in
    *           the header file)
    *
    * Return:
    *  None.
    *
    * Theory:
    *  Enables the output of specific status bits to the interrupt controller
    *
    *******************************************************************************/
    void `$INSTANCE_NAME`_SetRxInterruptMode(uint8 intSrc) `=ReentrantKeil($INSTANCE_NAME . "_SetRxInterruptMode")`
    {
        `$INSTANCE_NAME`_RXSTATUS_MASK_REG  = intSrc;
    }


    /*******************************************************************************
    * Function Name: `$INSTANCE_NAME`_ReadRxData
    ********************************************************************************
    *
    * Summary:
    *  Returns data in RX Data register without checking status register to 
    *  determine if data is valid
    *
    * Parameters:
    *  None.
    *
    * Return:
    *  Received data from RX register
    *
    * Global Variables:
    *  `$INSTANCE_NAME`_rxBuffer - RAM buffer pointer for save received data.
    *  `$INSTANCE_NAME`_rxBufferWrite - cyclic index for write to rxBuffer, 
    *     checked to identify new data. 
    *  `$INSTANCE_NAME`_rxBufferRead - cyclic index for read from rxBuffer, 
    *     incremented after each byte has been read from buffer.
    *  `$INSTANCE_NAME`_rxBufferLoopDetect - creared if loop condition was detected
    *     in RX ISR. 
    *
    * Reentrant:
    *  No.
    *
    *******************************************************************************/
    uint8 `$INSTANCE_NAME`_ReadRxData(void) `=ReentrantKeil($INSTANCE_NAME . "_ReadRxData")`
    {
        uint8 rxData;

        #if(`$INSTANCE_NAME`_RXBUFFERSIZE > `$INSTANCE_NAME`_FIFO_LENGTH)

            /* Disable Rx interrupt. */
            /* Protect variables that could change on interrupt. */
            #if(`$INSTANCE_NAME`_RX_INTERRUPT_ENABLED)
                `$INSTANCE_NAME`_DisableRxInt();
            #endif /* End `$INSTANCE_NAME`_RX_INTERRUPT_ENABLED */

            if( (`$INSTANCE_NAME`_rxBufferRead != `$INSTANCE_NAME`_rxBufferWrite) ||
                (`$INSTANCE_NAME`_rxBufferLoopDetect > 0u) )
            {

                rxData = `$INSTANCE_NAME`_rxBuffer[`$INSTANCE_NAME`_rxBufferRead];

                `$INSTANCE_NAME`_rxBufferRead++;

                if(`$INSTANCE_NAME`_rxBufferRead >= `$INSTANCE_NAME`_RXBUFFERSIZE)
                {
                    `$INSTANCE_NAME`_rxBufferRead = 0u;
                }

                if(`$INSTANCE_NAME`_rxBufferLoopDetect > 0u )
                {
                    `$INSTANCE_NAME`_rxBufferLoopDetect = 0u;
                    #if( (`$INSTANCE_NAME`_RX_INTERRUPT_ENABLED) && (`$INSTANCE_NAME`_FLOW_CONTROL != 0u) && \
                         (`$INSTANCE_NAME`_RXBUFFERSIZE > `$INSTANCE_NAME`_FIFO_LENGTH) )
                        /* When Hardware Flow Control selected - return RX mask */
                        #if( `$INSTANCE_NAME`_HD_ENABLED )
                            if((`$INSTANCE_NAME`_CONTROL_REG & `$INSTANCE_NAME`_CTRL_HD_SEND) == 0)
                            {   /* In Half duplex mode return RX mask only in RX 
                                *  configuration set, otherwise 
                                *  mask will be returned in LoadRxConfig() API. 
                                */
                                `$INSTANCE_NAME`_RXSTATUS_MASK_REG  |= `$INSTANCE_NAME`_RX_STS_FIFO_NOTEMPTY;
                            }
                        #else
                            `$INSTANCE_NAME`_RXSTATUS_MASK_REG  |= `$INSTANCE_NAME`_RX_STS_FIFO_NOTEMPTY;
                        #endif /* end `$INSTANCE_NAME`_HD_ENABLED */
                    #endif /* `$INSTANCE_NAME`_RX_INTERRUPT_ENABLED and Hardware flow control*/
                }
            }
            else
            {   /* Needs to check status for RX_STS_FIFO_NOTEMPTY bit*/
                rxData = `$INSTANCE_NAME`_RXDATA_REG;
            }

            /* Enable Rx interrupt. */
            #if(`$INSTANCE_NAME`_RX_INTERRUPT_ENABLED)
                `$INSTANCE_NAME`_EnableRxInt();
            #endif /* End `$INSTANCE_NAME`_RX_INTERRUPT_ENABLED */

        #else /* `$INSTANCE_NAME`_RXBUFFERSIZE > `$INSTANCE_NAME`_FIFO_LENGTH */

            /* Needs to check status for RX_STS_FIFO_NOTEMPTY bit*/
            rxData = `$INSTANCE_NAME`_RXDATA_REG;

        #endif /* `$INSTANCE_NAME`_RXBUFFERSIZE > `$INSTANCE_NAME`_FIFO_LENGTH */

        return(rxData);
    }


    /*******************************************************************************
    * Function Name: `$INSTANCE_NAME`_ReadRxStatus
    ********************************************************************************
    *
    * Summary:
    *  Read the current state of the status register
    *  And detect software buffer overflow.
    *
    * Parameters:
    *  None.
    *
    * Return:
    *  Current state of the status register.
    *
    * Global Variables:
    *  `$INSTANCE_NAME`_rxBufferOverflow - used to indicate overload condition. 
    *   It set to one in RX interrupt when there isn?t free space in 
    *   `$INSTANCE_NAME`_rxBufferRead to write new data. This condition returned 
    *   and cleared to zero by this API as an 
    *   `$INSTANCE_NAME`_RX_STS_SOFT_BUFF_OVER bit along with RX Status register 
    *   bits.
    *
    *******************************************************************************/
    uint8 `$INSTANCE_NAME`_ReadRxStatus(void) `=ReentrantKeil($INSTANCE_NAME . "_ReadRxStatus")`
    {
        uint8 status;

        status = `$INSTANCE_NAME`_RXSTATUS_REG;
        status &= `$INSTANCE_NAME`_RX_HW_MASK;

        #if(`$INSTANCE_NAME`_RXBUFFERSIZE > `$INSTANCE_NAME`_FIFO_LENGTH)
            if( `$INSTANCE_NAME`_rxBufferOverflow )
            {
                status |= `$INSTANCE_NAME`_RX_STS_SOFT_BUFF_OVER;
                `$INSTANCE_NAME`_rxBufferOverflow = 0u;
            }
        #endif /* `$INSTANCE_NAME`_RXBUFFERSIZE */

        return(status);
    }


    /*******************************************************************************
    * Function Name: `$INSTANCE_NAME`_GetChar
    ********************************************************************************
    *
    * Summary:
    *  Reads UART RX buffer immediately, if data is not available or an error 
    *  condition exists, zero is returned; otherwise, character is read and 
    *  returned.
    *
    * Parameters:
    *  None.
    *
    * Return:
    *  Character read from UART RX buffer. ASCII characters from 1 to 255 are valid.
    *  A returned zero signifies an error condition or no data available.
    *
    * Global Variables:
    *  `$INSTANCE_NAME`_rxBuffer - RAM buffer pointer for save received data.
    *  `$INSTANCE_NAME`_rxBufferWrite - cyclic index for write to rxBuffer, 
    *     checked to identify new data. 
    *  `$INSTANCE_NAME`_rxBufferRead - cyclic index for read from rxBuffer, 
    *     incremented after each byte has been read from buffer.
    *  `$INSTANCE_NAME`_rxBufferLoopDetect - creared if loop condition was detected
    *     in RX ISR. 
    *
    * Reentrant:
    *  No.
    *
    *******************************************************************************/
    uint8 `$INSTANCE_NAME`_GetChar(void) `=ReentrantKeil($INSTANCE_NAME . "_GetChar")`
    {
        uint8 rxData = 0u;
        uint8 rxStatus;

        #if(`$INSTANCE_NAME`_RXBUFFERSIZE > `$INSTANCE_NAME`_FIFO_LENGTH)

            /* Disable Rx interrupt. */
            /* Protect variables that could change on interrupt. */
            #if(`$INSTANCE_NAME`_RX_INTERRUPT_ENABLED)
                `$INSTANCE_NAME`_DisableRxInt();
            #endif /* `$INSTANCE_NAME`_RX_INTERRUPT_ENABLED */

            if( (`$INSTANCE_NAME`_rxBufferRead != `$INSTANCE_NAME`_rxBufferWrite) ||
                (`$INSTANCE_NAME`_rxBufferLoopDetect > 0u) )
            {
                rxData = `$INSTANCE_NAME`_rxBuffer[`$INSTANCE_NAME`_rxBufferRead];

                `$INSTANCE_NAME`_rxBufferRead++;

                if(`$INSTANCE_NAME`_rxBufferRead >= `$INSTANCE_NAME`_RXBUFFERSIZE)
                {
                    `$INSTANCE_NAME`_rxBufferRead = 0u;
                }

                if(`$INSTANCE_NAME`_rxBufferLoopDetect > 0u ) 
                {
                    `$INSTANCE_NAME`_rxBufferLoopDetect = 0u;
                    #if( (`$INSTANCE_NAME`_RX_INTERRUPT_ENABLED) && (`$INSTANCE_NAME`_FLOW_CONTROL != 0u) && \
                         (`$INSTANCE_NAME`_RXBUFFERSIZE > `$INSTANCE_NAME`_FIFO_LENGTH) )
                        /* When Hardware Flow Control selected - return RX mask */
                        #if( `$INSTANCE_NAME`_HD_ENABLED )
                            if((`$INSTANCE_NAME`_CONTROL_REG & `$INSTANCE_NAME`_CTRL_HD_SEND) == 0)
                            {   /* In Half duplex mode return RX mask only if 
                                *  RX configuration set, otherwise 
                                *  mask will be returned in LoadRxConfig() API. 
                                */
                                `$INSTANCE_NAME`_RXSTATUS_MASK_REG  |= `$INSTANCE_NAME`_RX_STS_FIFO_NOTEMPTY;
                            }
                        #else
                            `$INSTANCE_NAME`_RXSTATUS_MASK_REG  |= `$INSTANCE_NAME`_RX_STS_FIFO_NOTEMPTY;
                        #endif /* end `$INSTANCE_NAME`_HD_ENABLED */
                    #endif /* `$INSTANCE_NAME`_RX_INTERRUPT_ENABLED and Hardware flow control*/
                }

            }
            else
            {   rxStatus =`$INSTANCE_NAME`_RXSTATUS_REG;
                if(rxStatus & `$INSTANCE_NAME`_RX_STS_FIFO_NOTEMPTY)
                {   /* Read received data from FIFO*/
                    rxData = `$INSTANCE_NAME`_RXDATA_REG;
                    /*Check status on error*/
                    if(rxStatus & (`$INSTANCE_NAME`_RX_STS_BREAK | `$INSTANCE_NAME`_RX_STS_PAR_ERROR |
                                   `$INSTANCE_NAME`_RX_STS_STOP_ERROR | `$INSTANCE_NAME`_RX_STS_OVERRUN))
                    {
                        rxData = 0u;
                    }    
                }
            }

            /* Enable Rx interrupt. */
            #if(`$INSTANCE_NAME`_RX_INTERRUPT_ENABLED)
                `$INSTANCE_NAME`_EnableRxInt();
            #endif /* `$INSTANCE_NAME`_RX_INTERRUPT_ENABLED */

        #else /* `$INSTANCE_NAME`_RXBUFFERSIZE > `$INSTANCE_NAME`_FIFO_LENGTH */

            rxStatus =`$INSTANCE_NAME`_RXSTATUS_REG;
            if(rxStatus & `$INSTANCE_NAME`_RX_STS_FIFO_NOTEMPTY)
            {   /* Read received data from FIFO*/
                rxData = `$INSTANCE_NAME`_RXDATA_REG;
                /*Check status on error*/
                if(rxStatus & (`$INSTANCE_NAME`_RX_STS_BREAK | `$INSTANCE_NAME`_RX_STS_PAR_ERROR |
                               `$INSTANCE_NAME`_RX_STS_STOP_ERROR | `$INSTANCE_NAME`_RX_STS_OVERRUN))
                {
                    rxData = 0u;
                }
            }
        #endif /* `$INSTANCE_NAME`_RXBUFFERSIZE > `$INSTANCE_NAME`_FIFO_LENGTH */

        return(rxData);
    }


    /*******************************************************************************
    * Function Name: `$INSTANCE_NAME`_GetByte
    ********************************************************************************
    *
    * Summary:
    *  Grab the next available byte of data from the recieve FIFO
    *
    * Parameters:
    *  None.
    *
    * Return:
    *  MSB contains Status Register and LSB contains UART RX data
    *
    * Reentrant:
    *  No.
    *
    *******************************************************************************/
    uint16 `$INSTANCE_NAME`_GetByte(void) `=ReentrantKeil($INSTANCE_NAME . "_GetByte")`
    {
        return ( ((uint16)`$INSTANCE_NAME`_ReadRxStatus() << 8u) | `$INSTANCE_NAME`_ReadRxData() );
    }


    /*******************************************************************************
    * Function Name: `$INSTANCE_NAME`_GetRxBufferSize
    ********************************************************************************
    *
    * Summary:
    *  Determine the amount of bytes left in the RX buffer and return the count in
    *  bytes
    *
    * Parameters:
    *  None.
    *
    * Return:
    *  `$RxBuffRegSizeReplacementString`: Integer count of the number of bytes left 
    *  in the RX buffer
    *
    * Global Variables:
    *  `$INSTANCE_NAME`_rxBufferWrite - used to calculate left bytes. 
    *  `$INSTANCE_NAME`_rxBufferRead - used to calculate left bytes.
    *  `$INSTANCE_NAME`_rxBufferLoopDetect - checked to decide left bytes amount. 
    *
    * Reentrant:
    *  No.
    *
    * Theory:
    *  Allows the user to find out how full the RX Buffer is.
    *
    *******************************************************************************/
    `$RxBuffRegSizeReplacementString` `$INSTANCE_NAME`_GetRxBufferSize(void) 
                                                            `=ReentrantKeil($INSTANCE_NAME . "_GetRxBufferSize")`
    {
        `$RxBuffRegSizeReplacementString` size;

        #if(`$INSTANCE_NAME`_RXBUFFERSIZE > `$INSTANCE_NAME`_FIFO_LENGTH)

            /* Disable Rx interrupt. */
            /* Protect variables that could change on interrupt. */
            #if(`$INSTANCE_NAME`_RX_INTERRUPT_ENABLED)
                `$INSTANCE_NAME`_DisableRxInt();
            #endif /* `$INSTANCE_NAME`_RX_INTERRUPT_ENABLED */

            if(`$INSTANCE_NAME`_rxBufferRead == `$INSTANCE_NAME`_rxBufferWrite)
            {
                if(`$INSTANCE_NAME`_rxBufferLoopDetect > 0u)
                {
                    size = `$INSTANCE_NAME`_RXBUFFERSIZE;
                }
                else
                {
                    size = 0u;
                }
            }
            else if(`$INSTANCE_NAME`_rxBufferRead < `$INSTANCE_NAME`_rxBufferWrite)
            {
                size = (`$INSTANCE_NAME`_rxBufferWrite - `$INSTANCE_NAME`_rxBufferRead);
            }
            else
            {
                size = (`$INSTANCE_NAME`_RXBUFFERSIZE - `$INSTANCE_NAME`_rxBufferRead) + `$INSTANCE_NAME`_rxBufferWrite;
            }

            /* Enable Rx interrupt. */
            #if(`$INSTANCE_NAME`_RX_INTERRUPT_ENABLED)
                `$INSTANCE_NAME`_EnableRxInt();
            #endif /* End `$INSTANCE_NAME`_RX_INTERRUPT_ENABLED */

        #else /* `$INSTANCE_NAME`_RXBUFFERSIZE > `$INSTANCE_NAME`_FIFO_LENGTH */

            /* We can only know if there is data in the fifo. */
            size = (`$INSTANCE_NAME`_RXSTATUS_REG & `$INSTANCE_NAME`_RX_STS_FIFO_NOTEMPTY) ? 1u : 0u;

        #endif /* End `$INSTANCE_NAME`_RXBUFFERSIZE > `$INSTANCE_NAME`_FIFO_LENGTH */

        return(size);
    }


    /*******************************************************************************
    * Function Name: `$INSTANCE_NAME`_ClearRxBuffer
    ********************************************************************************
    *
    * Summary:
    *  Clears the RX RAM buffer by setting the read and write pointers both to zero.
    *  Clears hardware RX FIFO.
    *
    * Parameters:
    *  None.
    *
    * Return:
    *  None.
    *
    * Global Variables:
    *  `$INSTANCE_NAME`_rxBufferWrite - cleared to zero. 
    *  `$INSTANCE_NAME`_rxBufferRead - cleared to zero.
    *  `$INSTANCE_NAME`_rxBufferLoopDetect - cleared to zero. 
    *  `$INSTANCE_NAME`_rxBufferOverflow - cleared to zero. 
    *
    * Reentrant:
    *  No.
    *
    * Theory:
    *  Setting the pointers to zero makes the system believe there is no data to 
    *  read and writing will resume at address 0 overwriting any data that may 
    *  have remained in the RAM.
    *
    * Side Effects:
    *  Any received data not read from the RAM or FIFO buffer will be lost.
    *******************************************************************************/
    void `$INSTANCE_NAME`_ClearRxBuffer(void) `=ReentrantKeil($INSTANCE_NAME . "_ClearRxBuffer")`
    {
        uint8 enableInterrupts;
        
        /* clear the HW FIFO */
        /* Enter critical section */
        enableInterrupts = CyEnterCriticalSection();        
        `$INSTANCE_NAME`_RXDATA_AUX_CTL_REG |=  `$INSTANCE_NAME`_RX_FIFO_CLR;
        `$INSTANCE_NAME`_RXDATA_AUX_CTL_REG &= ~`$INSTANCE_NAME`_RX_FIFO_CLR;
        /* Exit critical section */
        CyExitCriticalSection(enableInterrupts);
        
        #if(`$INSTANCE_NAME`_RXBUFFERSIZE > `$INSTANCE_NAME`_FIFO_LENGTH)
            /* Disable Rx interrupt. */
            /* Protect variables that could change on interrupt. */
            #if(`$INSTANCE_NAME`_RX_INTERRUPT_ENABLED)
                `$INSTANCE_NAME`_DisableRxInt();
            #endif /* End `$INSTANCE_NAME`_RX_INTERRUPT_ENABLED */

            `$INSTANCE_NAME`_rxBufferRead = 0u;
            `$INSTANCE_NAME`_rxBufferWrite = 0u;
            `$INSTANCE_NAME`_rxBufferLoopDetect = 0u;
            `$INSTANCE_NAME`_rxBufferOverflow = 0u;

            /* Enable Rx interrupt. */
            #if(`$INSTANCE_NAME`_RX_INTERRUPT_ENABLED)
                `$INSTANCE_NAME`_EnableRxInt();
            #endif /* End `$INSTANCE_NAME`_RX_INTERRUPT_ENABLED */
        #endif /* End `$INSTANCE_NAME`_RXBUFFERSIZE > `$INSTANCE_NAME`_FIFO_LENGTH */
        
    }


    /*******************************************************************************
    * Function Name: `$INSTANCE_NAME`_SetRxAddressMode
    ********************************************************************************
    *
    * Summary:
    *  Set the receive addressing mode
    *
    * Parameters:
    *  addressMode: Enumerated value indicating the mode of RX addressing
    *  `$INSTANCE_NAME`__B_UART__AM_SW_BYTE_BYTE -  Software Byte-by-Byte address 
    *                                               detection
    *  `$INSTANCE_NAME`__B_UART__AM_SW_DETECT_TO_BUFFER - Software Detect to Buffer 
    *                                               address detection
    *  `$INSTANCE_NAME`__B_UART__AM_HW_BYTE_BY_BYTE - Hardware Byte-by-Byte address 
    *                                               detection
    *  `$INSTANCE_NAME`__B_UART__AM_HW_DETECT_TO_BUFFER - Hardware Detect to Buffer 
    *                                               address detection
    *  `$INSTANCE_NAME`__B_UART__AM_NONE - No address detection
    *
    * Return:
    *  None.
    *
    * Global Variables:
    *  `$INSTANCE_NAME`_rxAddressMode - the parameter stored in this variable for 
    *   the farther usage in RX ISR.
    *  `$INSTANCE_NAME`_rxAddressDetected - set to initial state (0).
    *
    *******************************************************************************/
    void `$INSTANCE_NAME`_SetRxAddressMode(uint8 addressMode)  
                                                        `=ReentrantKeil($INSTANCE_NAME . "_SetRxAddressMode")`
    {
        #if(`$INSTANCE_NAME`_RXHW_ADDRESS_ENABLED)
            #if(`$INSTANCE_NAME`_CONTROL_REG_REMOVED)
                addressMode = addressMode;
            #else /* `$INSTANCE_NAME`_CONTROL_REG_REMOVED */
                uint8 tmpCtrl = 0u;
                tmpCtrl = `$INSTANCE_NAME`_CONTROL_REG & ~`$INSTANCE_NAME`_CTRL_RXADDR_MODE_MASK;
                tmpCtrl |= ((addressMode << `$INSTANCE_NAME`_CTRL_RXADDR_MODE0_SHIFT) & 
                           `$INSTANCE_NAME`_CTRL_RXADDR_MODE_MASK);
                `$INSTANCE_NAME`_CONTROL_REG = tmpCtrl;
                #if(`$INSTANCE_NAME`_RX_INTERRUPT_ENABLED && \
                   (`$INSTANCE_NAME`_RXBUFFERSIZE > `$INSTANCE_NAME`_FIFO_LENGTH) )
                    `$INSTANCE_NAME`_rxAddressMode = addressMode;
                    `$INSTANCE_NAME`_rxAddressDetected = 0u;
                #endif /* End `$INSTANCE_NAME`_RXBUFFERSIZE > `$INSTANCE_NAME`_FIFO_LENGTH*/   
            #endif /* End `$INSTANCE_NAME`_CONTROL_REG_REMOVED */
        #else /* `$INSTANCE_NAME`_RXHW_ADDRESS_ENABLED */
            addressMode = addressMode;
        #endif /* End `$INSTANCE_NAME`_RXHW_ADDRESS_ENABLED */
    }


    /*******************************************************************************
    * Function Name: `$INSTANCE_NAME`_SetRxAddress1
    ********************************************************************************
    *
    * Summary:
    *  Set the first hardware address compare value
    *
    * Parameters:
    *  address
    *
    * Return:
    *  None.
    *
    *******************************************************************************/
    void `$INSTANCE_NAME`_SetRxAddress1(uint8 address) `=ReentrantKeil($INSTANCE_NAME . "_SetRxAddress1")`

    {
        `$INSTANCE_NAME`_RXADDRESS1_REG = address;
    }


    /*******************************************************************************
    * Function Name: `$INSTANCE_NAME`_SetRxAddress2
    ********************************************************************************
    *
    * Summary:
    *  Set the second hardware address compare value
    *
    * Parameters:
    *  address
    *
    * Return:
    *  None.
    *
    *******************************************************************************/
    void `$INSTANCE_NAME`_SetRxAddress2(uint8 address) `=ReentrantKeil($INSTANCE_NAME . "_SetRxAddress2")`
    {
        `$INSTANCE_NAME`_RXADDRESS2_REG = address;
    }
        
#endif  /* `$INSTANCE_NAME`_RX_ENABLED || `$INSTANCE_NAME`_HD_ENABLED*/


#if( (`$INSTANCE_NAME`_TX_ENABLED) || (`$INSTANCE_NAME`_HD_ENABLED) )

    #if(`$INSTANCE_NAME`_TX_INTERRUPT_ENABLED)

        /*******************************************************************************
        * Function Name: `$INSTANCE_NAME`_EnableTxInt
        ********************************************************************************
        *
        * Summary:
        *  Enable TX interrupt generation
        *
        * Parameters:
        *  None.
        *
        * Return:
        *  None.
        *
        * Theory:
        *  Enable the interrupt output -or- the interrupt component itself
        *
        *******************************************************************************/
        void `$INSTANCE_NAME`_EnableTxInt(void) `=ReentrantKeil($INSTANCE_NAME . "_EnableTxInt")`
        {
            CyIntEnable(`$INSTANCE_NAME`_TX_VECT_NUM);
        }


        /*******************************************************************************
        * Function Name: `$INSTANCE_NAME`_DisableTxInt
        ********************************************************************************
        *
        * Summary:
        *  Disable TX interrupt generation
        *
        * Parameters:
        *  None.
        *
        * Return:
        *  None.
        *
        * Theory:
        *  Disable the interrupt output -or- the interrupt component itself
        *
        *******************************************************************************/
        void `$INSTANCE_NAME`_DisableTxInt(void) `=ReentrantKeil($INSTANCE_NAME . "_DisableTxInt")`
        {
            CyIntDisable(`$INSTANCE_NAME`_TX_VECT_NUM);
        }

    #endif /* `$INSTANCE_NAME`_TX_INTERRUPT_ENABLED */


    /*******************************************************************************
    * Function Name: `$INSTANCE_NAME`_SetTxInterruptMode
    ********************************************************************************
    *
    * Summary:
    *  Configure which status bits trigger an interrupt event
    *
    * Parameters:
    *  intSrc: An or'd combination of the desired status bit masks (defined in
    *          the header file)
    *
    * Return:
    *  None.
    *
    * Theory:
    *  Enables the output of specific status bits to the interrupt controller
    *
    *******************************************************************************/
    void `$INSTANCE_NAME`_SetTxInterruptMode(uint8 intSrc) `=ReentrantKeil($INSTANCE_NAME . "_SetTxInterruptMode")`
    {
        `$INSTANCE_NAME`_TXSTATUS_MASK_REG = intSrc;
    }


    /*******************************************************************************
    * Function Name: `$INSTANCE_NAME`_WriteTxData
    ********************************************************************************
    *
    * Summary:
    *  Write a byte of data to the Transmit FIFO or TX buffer to be sent when the 
    *  bus is available. WriteTxData sends a byte without checking for buffer room 
    *  or status. It is up to the user to separately check status.    
    *
    * Parameters:
    *  TXDataByte: byte of data to place in the transmit FIFO
    *
    * Return:
    * void
    *
    * Global Variables:
    *  `$INSTANCE_NAME`_txBuffer - RAM buffer pointer for save data for transmission
    *  `$INSTANCE_NAME`_txBufferWrite - cyclic index for write to txBuffer, 
    *    incremented after each byte saved to buffer.
    *  `$INSTANCE_NAME`_txBufferRead - cyclic index for read from txBuffer, 
    *    checked to identify the condition to write to FIFO directly or to TX buffer
    *  `$INSTANCE_NAME`_initVar - checked to identify that the component has been  
    *    initialized.
    *
    * Reentrant:
    *  No.
    *
    *******************************************************************************/
    void `$INSTANCE_NAME`_WriteTxData(uint8 txDataByte) `=ReentrantKeil($INSTANCE_NAME . "_WriteTxData")`
    {
        /* If not Initialized then skip this function*/
        if(`$INSTANCE_NAME`_initVar != 0u)
        {
            #if(`$INSTANCE_NAME`_TXBUFFERSIZE > `$INSTANCE_NAME`_FIFO_LENGTH)

                /* Disable Tx interrupt. */
                /* Protect variables that could change on interrupt. */
                #if(`$INSTANCE_NAME`_TX_INTERRUPT_ENABLED)
                    `$INSTANCE_NAME`_DisableTxInt();
                #endif /* End `$INSTANCE_NAME`_TX_INTERRUPT_ENABLED */

                if( (`$INSTANCE_NAME`_txBufferRead == `$INSTANCE_NAME`_txBufferWrite) &&
                   !(`$INSTANCE_NAME`_TXSTATUS_REG & `$INSTANCE_NAME`_TX_STS_FIFO_FULL) )
                {
                    /* Add directly to the FIFO. */
                    `$INSTANCE_NAME`_TXDATA_REG = txDataByte;
                }
                else
                {
                    if(`$INSTANCE_NAME`_txBufferWrite >= `$INSTANCE_NAME`_TXBUFFERSIZE)
                    {
                        `$INSTANCE_NAME`_txBufferWrite = 0;
                    }

                    `$INSTANCE_NAME`_txBuffer[`$INSTANCE_NAME`_txBufferWrite] = txDataByte;

                    /* Add to the software buffer. */
                    `$INSTANCE_NAME`_txBufferWrite++;

                }

                /* Enable Tx interrupt. */
                #if(`$INSTANCE_NAME`_TX_INTERRUPT_ENABLED)
                    `$INSTANCE_NAME`_EnableTxInt();
                #endif /* End `$INSTANCE_NAME`_TX_INTERRUPT_ENABLED */

            #else /* `$INSTANCE_NAME`_TXBUFFERSIZE > `$INSTANCE_NAME`_FIFO_LENGTH */

                /* Add directly to the FIFO. */
                `$INSTANCE_NAME`_TXDATA_REG = txDataByte;

            #endif /* End `$INSTANCE_NAME`_TXBUFFERSIZE > `$INSTANCE_NAME`_FIFO_LENGTH */
        }
    }


    /*******************************************************************************
    * Function Name: `$INSTANCE_NAME`_ReadTxStatus
    ********************************************************************************
    *
    * Summary:
    *  Read the status register for the component
    *
    * Parameters:
    *  None.
    *
    * Return:
    *  Contents of the status register
    *
    * Theory:
    *  This function reads the status register which is clear on read. It is up to 
    *  the user to handle all bits in this return value accordingly, even if the bit 
    *  was not enabled as an interrupt source the event happened and must be handled
    *  accordingly.    
    *
    *******************************************************************************/
    uint8 `$INSTANCE_NAME`_ReadTxStatus(void) `=ReentrantKeil($INSTANCE_NAME . "_ReadTxStatus")`
    {
        return(`$INSTANCE_NAME`_TXSTATUS_REG);
    }


    /*******************************************************************************
    * Function Name: `$INSTANCE_NAME`_PutChar
    ********************************************************************************
    *
    * Summary:
    *  Wait to send byte until TX register or buffer has room.
    *
    * Parameters:
    *  txDataByte: The 8-bit data value to send across the UART.
    *
    * Return:
    *  None.
    *
    * Global Variables:
    *  `$INSTANCE_NAME`_txBuffer - RAM buffer pointer for save data for transmission
    *  `$INSTANCE_NAME`_txBufferWrite - cyclic index for write to txBuffer, 
    *     checked to identify free space in txBuffer and incremented after each byte 
    *     saved to buffer.
    *  `$INSTANCE_NAME`_txBufferRead - cyclic index for read from txBuffer, 
    *     checked to identify free space in txBuffer.
    *  `$INSTANCE_NAME`_initVar - checked to identify that the component has been  
    *     initialized.
    *
    * Reentrant:
    *  No.
    *
    * Theory:
    *  Allows the user to transmit any byte of data in a single transfer
    *
    *******************************************************************************/
    void `$INSTANCE_NAME`_PutChar(uint8 txDataByte) `=ReentrantKeil($INSTANCE_NAME . "_PutChar")`
    {
            #if(`$INSTANCE_NAME`_TXBUFFERSIZE > `$INSTANCE_NAME`_FIFO_LENGTH)

                /* Block if buffer is full, so we dont overwrite. */
                while( `$INSTANCE_NAME`_txBufferWrite == (`$INSTANCE_NAME`_txBufferRead - 1u) ||
                    (uint8)(`$INSTANCE_NAME`_txBufferWrite - `$INSTANCE_NAME`_txBufferRead) ==
                    (uint8)(`$INSTANCE_NAME`_TXBUFFERSIZE - 1u) )
                {
                    /* Software buffer is full. */
                }
                /* Disable Tx interrupt. */
                /* Protect variables that could change on interrupt. */
                #if(`$INSTANCE_NAME`_TX_INTERRUPT_ENABLED)
                    `$INSTANCE_NAME`_DisableTxInt();
                #endif /* End `$INSTANCE_NAME`_TX_INTERRUPT_ENABLED */

                if( (`$INSTANCE_NAME`_txBufferRead == `$INSTANCE_NAME`_txBufferWrite) &&
                   !(`$INSTANCE_NAME`_TXSTATUS_REG & `$INSTANCE_NAME`_TX_STS_FIFO_FULL) )
                {
                    /* Add directly to the FIFO. */
                    `$INSTANCE_NAME`_TXDATA_REG = txDataByte;
                }
                else
                {
                    if(`$INSTANCE_NAME`_txBufferWrite >= `$INSTANCE_NAME`_TXBUFFERSIZE)
                    {
                        `$INSTANCE_NAME`_txBufferWrite = 0;
                    }

                    `$INSTANCE_NAME`_txBuffer[`$INSTANCE_NAME`_txBufferWrite] = txDataByte;

                    /* Add to the software buffer. */
                    `$INSTANCE_NAME`_txBufferWrite++;

                }

                /* Enable Rx interrupt. */
                #if(`$INSTANCE_NAME`_TX_INTERRUPT_ENABLED)
                    `$INSTANCE_NAME`_EnableTxInt();
                #endif /* End `$INSTANCE_NAME`_TX_INTERRUPT_ENABLED */

            #else /* `$INSTANCE_NAME`_TXBUFFERSIZE > `$INSTANCE_NAME`_FIFO_LENGTH */

                /* Block if there isnt room. */
                while(`$INSTANCE_NAME`_TXSTATUS_REG & `$INSTANCE_NAME`_TX_STS_FIFO_FULL);

                /* Add directly to the FIFO. */
                `$INSTANCE_NAME`_TXDATA_REG = txDataByte;

            #endif /* End `$INSTANCE_NAME`_TXBUFFERSIZE > `$INSTANCE_NAME`_FIFO_LENGTH */
    }


    /*******************************************************************************
    * Function Name: `$INSTANCE_NAME`_PutString
    ********************************************************************************
    *
    * Summary:
    *  Write a Sequence of bytes on the Transmit line. Data comes from RAM or ROM.
    *
    * Parameters:
    *  string: char pointer to character string of Data to Send.
    *
    * Return:
    *  None.
    *
    * Global Variables:
    *  `$INSTANCE_NAME`_initVar - checked to identify that the component has been  
    *     initialized.
    *
    * Reentrant:
    *  No.
    *
    * Theory:
    *  This function will block if there is not enough memory to place the whole 
    *  string, it will block until the entire string has been written to the 
    *  transmit buffer.
    *
    *******************************************************************************/
    void `$INSTANCE_NAME`_PutString(char* string) `=ReentrantKeil($INSTANCE_NAME . "_PutString")`
    {
        /* If not Initialized then skip this function*/
        if(`$INSTANCE_NAME`_initVar != 0u)
        {
            /* This is a blocking function, it will not exit until all data is sent*/
            while(*string != 0u)
            {
                `$INSTANCE_NAME`_PutChar(*string++);
            }
        }
    }


    /*******************************************************************************
    * Function Name: `$INSTANCE_NAME`_PutArray
    ********************************************************************************
    *
    * Summary:
    *  Write a Sequence of bytes on the Transmit line. Data comes from RAM or ROM.
    *
    * Parameters:
    *  string: Address of the memory array residing in RAM or ROM.
    *  byteCount: Number of Bytes to be transmitted.
    *
    * Return:
    *  None.
    *
    * Global Variables:
    *  `$INSTANCE_NAME`_initVar - checked to identify that the component has been  
    *     initialized.
    *
    * Reentrant:
    *  No.
    *
    *******************************************************************************/
    void `$INSTANCE_NAME`_PutArray(uint8* string, `$TxBuffRegSizeReplacementString` byteCount) 
                                                                    `=ReentrantKeil($INSTANCE_NAME . "_PutArray")`
    {
        /* If not Initialized then skip this function*/
        if(`$INSTANCE_NAME`_initVar != 0u)
        {
            while(byteCount > 0u)
            {
                `$INSTANCE_NAME`_PutChar(*string++);
                byteCount--;
            }
        }
    }


    /*******************************************************************************
    * Function Name: `$INSTANCE_NAME`_PutCRLF
    ********************************************************************************
    *
    * Summary:
    *  Write a character and then carriage return and line feed.
    *
    * Parameters:
    *  txDataByte: uint8 Character to send.
    *
    * Return:
    *  None.
    *
    * Global Variables:
    *  `$INSTANCE_NAME`_initVar - checked to identify that the component has been  
    *     initialized.
    *
    * Reentrant:
    *  No.
    *
    *******************************************************************************/
    void `$INSTANCE_NAME`_PutCRLF(uint8 txDataByte) `=ReentrantKeil($INSTANCE_NAME . "_PutCRLF")`
    {
        /* If not Initialized then skip this function*/
        if(`$INSTANCE_NAME`_initVar != 0u)
        {
            `$INSTANCE_NAME`_PutChar(txDataByte);
            `$INSTANCE_NAME`_PutChar(0x0Du);
            `$INSTANCE_NAME`_PutChar(0x0Au);
        }
    }


    /*******************************************************************************
    * Function Name: `$INSTANCE_NAME`_GetTxBufferSize
    ********************************************************************************
    *
    * Summary:
    *  Determine the amount of space left in the TX buffer and return the count in
    *  bytes
    *
    * Parameters:
    *  None.
    *
    * Return:
    *  Integer count of the number of bytes left in the TX buffer
    *
    * Global Variables:
    *  `$INSTANCE_NAME`_txBufferWrite - used to calculate left space. 
    *  `$INSTANCE_NAME`_txBufferRead - used to calculate left space.
    *
    * Reentrant:
    *  No.
    *
    * Theory:
    *  Allows the user to find out how full the TX Buffer is.
    *
    *******************************************************************************/
    `$TxBuffRegSizeReplacementString` `$INSTANCE_NAME`_GetTxBufferSize(void) 
                                                            `=ReentrantKeil($INSTANCE_NAME . "_GetTxBufferSize")`
    {
        `$TxBuffRegSizeReplacementString` size;

        #if(`$INSTANCE_NAME`_TXBUFFERSIZE > `$INSTANCE_NAME`_FIFO_LENGTH)

            /* Disable Tx interrupt. */
            /* Protect variables that could change on interrupt. */
            #if(`$INSTANCE_NAME`_TX_INTERRUPT_ENABLED)
                `$INSTANCE_NAME`_DisableTxInt();
            #endif /* End `$INSTANCE_NAME`_TX_INTERRUPT_ENABLED */

            if(`$INSTANCE_NAME`_txBufferRead == `$INSTANCE_NAME`_txBufferWrite)
            {
                size = 0u;
            }
            else if(`$INSTANCE_NAME`_txBufferRead < `$INSTANCE_NAME`_txBufferWrite)
            {
                size = (`$INSTANCE_NAME`_txBufferWrite - `$INSTANCE_NAME`_txBufferRead);
            }
            else
            {
                size = (`$INSTANCE_NAME`_TXBUFFERSIZE - `$INSTANCE_NAME`_txBufferRead) + `$INSTANCE_NAME`_txBufferWrite;
            }

            /* Enable Tx interrupt. */
            #if(`$INSTANCE_NAME`_TX_INTERRUPT_ENABLED)
                `$INSTANCE_NAME`_EnableTxInt();
            #endif /* End `$INSTANCE_NAME`_TX_INTERRUPT_ENABLED */

        #else /* `$INSTANCE_NAME`_TXBUFFERSIZE > `$INSTANCE_NAME`_FIFO_LENGTH */

            size = `$INSTANCE_NAME`_TXSTATUS_REG;

            /* Is the fifo is full. */
            if(size & `$INSTANCE_NAME`_TX_STS_FIFO_FULL)
            {
                size = `$INSTANCE_NAME`_FIFO_LENGTH;
            }
            else if(size & `$INSTANCE_NAME`_TX_STS_FIFO_EMPTY)
            {
                size = 0u;
            }
            else
            {
                /* We only know there is data in the fifo. */
                size = 1u;
            }

        #endif /* End `$INSTANCE_NAME`_TXBUFFERSIZE > `$INSTANCE_NAME`_FIFO_LENGTH */

        return(size);
    }


    /*******************************************************************************
    * Function Name: `$INSTANCE_NAME`_ClearTxBuffer
    ********************************************************************************
    *
    * Summary:
    *  Clears the TX RAM buffer by setting the read and write pointers both to zero.
    *  Clears the hardware TX FIFO.  Any data present in the FIFO will not be sent.
    *
    * Parameters:
    *  None.
    *
    * Return:
    *  None.
    *
    * Global Variables:
    *  `$INSTANCE_NAME`_txBufferWrite - cleared to zero. 
    *  `$INSTANCE_NAME`_txBufferRead - cleared to zero.
    *
    * Reentrant:
    *  No.
    *
    * Theory:
    *  Setting the pointers to zero makes the system believe there is no data to 
    *  read and writing will resume at address 0 overwriting any data that may have
    *  remained in the RAM.
    *
    * Side Effects:
    *  Any received data not read from the RAM buffer will be lost when overwritten.
    *
    *******************************************************************************/
    void `$INSTANCE_NAME`_ClearTxBuffer(void) `=ReentrantKeil($INSTANCE_NAME . "_ClearTxBuffer")`
    {
        uint8 enableInterrupts;
        
        /* Enter critical section */
        enableInterrupts = CyEnterCriticalSection();        
        /* clear the HW FIFO */
        `$INSTANCE_NAME`_TXDATA_AUX_CTL_REG |=  `$INSTANCE_NAME`_TX_FIFO_CLR;
        `$INSTANCE_NAME`_TXDATA_AUX_CTL_REG &= ~`$INSTANCE_NAME`_TX_FIFO_CLR;
        /* Exit critical section */
        CyExitCriticalSection(enableInterrupts);

        #if(`$INSTANCE_NAME`_TXBUFFERSIZE > `$INSTANCE_NAME`_FIFO_LENGTH)

            /* Disable Tx interrupt. */
            /* Protect variables that could change on interrupt. */
            #if(`$INSTANCE_NAME`_TX_INTERRUPT_ENABLED)
                `$INSTANCE_NAME`_DisableTxInt();
            #endif /* End `$INSTANCE_NAME`_TX_INTERRUPT_ENABLED */

            `$INSTANCE_NAME`_txBufferRead = 0u;
            `$INSTANCE_NAME`_txBufferWrite = 0u;

            /* Enable Tx interrupt. */
            #if(`$INSTANCE_NAME`_TX_INTERRUPT_ENABLED)
                `$INSTANCE_NAME`_EnableTxInt();
            #endif /* End `$INSTANCE_NAME`_TX_INTERRUPT_ENABLED */

        #endif /* End `$INSTANCE_NAME`_TXBUFFERSIZE > `$INSTANCE_NAME`_FIFO_LENGTH */
    }


    /*******************************************************************************
    * Function Name: `$INSTANCE_NAME`_SendBreak
    ********************************************************************************
    *
    * Summary:
    *  Write a Break command to the UART
    *
    * Parameters:
    *  uint8 retMode:  Wait mode,
    *   0 - Initialize registers for Break, sends the Break signal and return 
    *       imediately.
    *   1 - Wait until Break sending is complete, reinitialize registers to normal
    *       transmission mode then return.
    *   2 - Reinitialize registers to normal transmission mode then return.
    *   3 - both steps: 0 and 1
    *       init registers for Break, send Break signal
    *       wait until Break sending is complete, reinit registers to normal
    *       transmission mode then return.
    *
    * Return:
    *  None.
    *
    * Global Variables:
    *  `$INSTANCE_NAME`_initVar - checked to identify that the component has been  
    *     initialized.
    *  tx_period - static variable, used for keeping TX period configuration.
    *
    * Reentrant:
    *  No.
    *
    * Theory:
    *  SendBreak function initializes registers to send 13-bit break signal. It is
    *  important to return the registers configuration to normal for continue 8-bit
    *  operation.
    *  Trere are 3 variants for this API usage:
    *  1) SendBreak(3) - function will send the Break signal and take care on the
    *     configuration returning. Funcition will block CPU untill transmition 
    *     complete.
    *  2) User may want to use bloking time if UART configured to the low speed 
    *     operation
    *     Emample for this case:
    *     SendBreak(0);     - init Break signal transmition
    *         Add your code here to use CPU time
    *     SendBreak(1);     - complete Break operation
    *  3) Same to 2) but user may want to init and use the interrupt for complete 
    *     break operation.
    *     Example for this case:
    *     Init TX interrupt whith "TX - On TX Complete" parameter
    *     SendBreak(0);     - init Break signal transmition
    *         Add your code here to use CPU time
    *     When interrupt appear with UART_TX_STS_COMPLETE status:
    *     SendBreak(2);     - complete Break operation
    *
    * Side Effects:
    *   Uses static variable to keep registers configuration.
    *
    *******************************************************************************/
    void `$INSTANCE_NAME`_SendBreak(uint8 retMode) `=ReentrantKeil($INSTANCE_NAME . "_SendBreak")`
    {

        /* If not Initialized then skip this function*/
        if(`$INSTANCE_NAME`_initVar != 0u)
        {
            /*Set the Counter to 13-bits and transmit a 00 byte*/
            /*When that is done then reset the counter value back*/
            uint8 tmpStat;

            #if(`$INSTANCE_NAME`_HD_ENABLED) /* Half Duplex mode*/

                if( (retMode == `$INSTANCE_NAME`_SEND_BREAK) ||
                    (retMode == `$INSTANCE_NAME`_SEND_WAIT_REINIT ) )
                {
                    /* CTRL_HD_SEND_BREAK - sends break bits in HD mode*/
                    `$INSTANCE_NAME`_WriteControlRegister(`$INSTANCE_NAME`_ReadControlRegister() |
                                                          `$INSTANCE_NAME`_CTRL_HD_SEND_BREAK);
                    /* Send zeros*/
                    `$INSTANCE_NAME`_TXDATA_REG = 0u;

                    do /*wait until transmit starts*/
                    {
                        tmpStat = `$INSTANCE_NAME`_TXSTATUS_REG;
                    }while(tmpStat & `$INSTANCE_NAME`_TX_STS_FIFO_EMPTY);
                }

                if( (retMode == `$INSTANCE_NAME`_WAIT_FOR_COMPLETE_REINIT) ||
                    (retMode == `$INSTANCE_NAME`_SEND_WAIT_REINIT) )
                {
                    do /*wait until transmit complete*/
                    {
                        tmpStat = `$INSTANCE_NAME`_TXSTATUS_REG;
                    }while(~tmpStat & `$INSTANCE_NAME`_TX_STS_COMPLETE);
                }

                if( (retMode == `$INSTANCE_NAME`_WAIT_FOR_COMPLETE_REINIT) ||
                    (retMode == `$INSTANCE_NAME`_REINIT) ||
                    (retMode == `$INSTANCE_NAME`_SEND_WAIT_REINIT) )
                {
                    `$INSTANCE_NAME`_WriteControlRegister(`$INSTANCE_NAME`_ReadControlRegister() &
                                                         ~`$INSTANCE_NAME`_CTRL_HD_SEND_BREAK);
                }

            #else /* `$INSTANCE_NAME`_HD_ENABLED Full Duplex mode */

                static uint8 tx_period; 
                
                if( (retMode == `$INSTANCE_NAME`_SEND_BREAK) ||
                    (retMode == `$INSTANCE_NAME`_SEND_WAIT_REINIT) )
                {
                    /* CTRL_HD_SEND_BREAK - skip to send parity bit @ Break signal in Full Duplex mode*/
                    if( (`$INSTANCE_NAME`_PARITY_TYPE != `$INSTANCE_NAME`__B_UART__NONE_REVB) ||
                         `$INSTANCE_NAME`_PARITY_TYPE_SW )
                    {
                        `$INSTANCE_NAME`_WriteControlRegister(`$INSTANCE_NAME`_ReadControlRegister() |
                                                              `$INSTANCE_NAME`_CTRL_HD_SEND_BREAK);
                    }                                                          

                    #if(`$INSTANCE_NAME`_TXCLKGEN_DP)
                        tx_period = `$INSTANCE_NAME`_TXBITCLKTX_COMPLETE_REG;
                        `$INSTANCE_NAME`_TXBITCLKTX_COMPLETE_REG = `$INSTANCE_NAME`_TXBITCTR_BREAKBITS;
                    #else
                        tx_period = `$INSTANCE_NAME`_TXBITCTR_PERIOD_REG;
                        `$INSTANCE_NAME`_TXBITCTR_PERIOD_REG = `$INSTANCE_NAME`_TXBITCTR_BREAKBITS8X;
                    #endif /* End `$INSTANCE_NAME`_TXCLKGEN_DP */

                    /* Send zeros*/
                    `$INSTANCE_NAME`_TXDATA_REG = 0u;

                    do /*wait until transmit starts*/
                    {
                        tmpStat = `$INSTANCE_NAME`_TXSTATUS_REG;
                    }while(tmpStat & `$INSTANCE_NAME`_TX_STS_FIFO_EMPTY);
                }

                if( (retMode == `$INSTANCE_NAME`_WAIT_FOR_COMPLETE_REINIT) ||
                    (retMode == `$INSTANCE_NAME`_SEND_WAIT_REINIT) )
                {
                    do /*wait until transmit complete*/
                    {
                        tmpStat = `$INSTANCE_NAME`_TXSTATUS_REG;
                    }while(~tmpStat & `$INSTANCE_NAME`_TX_STS_COMPLETE);
                }

                if( (retMode == `$INSTANCE_NAME`_WAIT_FOR_COMPLETE_REINIT) ||
                    (retMode == `$INSTANCE_NAME`_REINIT) ||
                    (retMode == `$INSTANCE_NAME`_SEND_WAIT_REINIT) )
                {

                    #if(`$INSTANCE_NAME`_TXCLKGEN_DP)
                        `$INSTANCE_NAME`_TXBITCLKTX_COMPLETE_REG = tx_period;
                    #else
                        `$INSTANCE_NAME`_TXBITCTR_PERIOD_REG = tx_period;
                    #endif /* End `$INSTANCE_NAME`_TXCLKGEN_DP */

                    if( (`$INSTANCE_NAME`_PARITY_TYPE != `$INSTANCE_NAME`__B_UART__NONE_REVB) || 
                         `$INSTANCE_NAME`_PARITY_TYPE_SW )
                    {
                        `$INSTANCE_NAME`_WriteControlRegister(`$INSTANCE_NAME`_ReadControlRegister() &
                                                             ~`$INSTANCE_NAME`_CTRL_HD_SEND_BREAK);
                    }                                     
                }
            #endif    /* End `$INSTANCE_NAME`_HD_ENABLED */
        }
    }


    /*******************************************************************************
    * Function Name: `$INSTANCE_NAME`_SetTxAddressMode
    ********************************************************************************
    *
    * Summary:
    *  Set the transmit addressing mode
    *
    * Parameters:
    *  addressMode: 0 -> Space
    *               1 -> Mark
    *
    * Return:
    *  None.
    *
    *******************************************************************************/
    void `$INSTANCE_NAME`_SetTxAddressMode(uint8 addressMode) `=ReentrantKeil($INSTANCE_NAME . "_SetTxAddressMode")`
    {
        /* Mark/Space sending enable*/
        if(addressMode != 0)
        {
            `$INSTANCE_NAME`_WriteControlRegister(`$INSTANCE_NAME`_ReadControlRegister() | `$INSTANCE_NAME`_CTRL_MARK);
        }
        else
        {
            `$INSTANCE_NAME`_WriteControlRegister(`$INSTANCE_NAME`_ReadControlRegister() & ~`$INSTANCE_NAME`_CTRL_MARK);
        }
    }

#endif  /* End`$INSTANCE_NAME`_TX_ENABLED */

#if(`$INSTANCE_NAME`_HD_ENABLED)


    /*******************************************************************************
    * Function Name: `$INSTANCE_NAME`_LoadTxConfig
    ********************************************************************************
    *
    * Summary:
    *  Unloads the Rx configuration if required and loads the
    *  Tx configuration. It is the users responsibility to ensure that any
    *  transaction is complete and it is safe to unload the Tx
    *  configuration.
    *
    * Parameters:
    *  None.
    *
    * Return:
    *  None.
    *
    * Theory:
    *  Valid only for half duplex UART. 
    *
    * Side Effects:
    *  Disable RX interrupt mask, when software buffer has been used.
    *
    *******************************************************************************/
    void `$INSTANCE_NAME`_LoadTxConfig(void) `=ReentrantKeil($INSTANCE_NAME . "_LoadTxConfig")`
    {
        #if((`$INSTANCE_NAME`_RX_INTERRUPT_ENABLED) && (`$INSTANCE_NAME`_RXBUFFERSIZE > `$INSTANCE_NAME`_FIFO_LENGTH))
            /* Disable RX interrupts before set TX configuration */
            `$INSTANCE_NAME`_SetRxInterruptMode(0);
        #endif /* `$INSTANCE_NAME`_RX_INTERRUPT_ENABLED */

        `$INSTANCE_NAME`_WriteControlRegister(`$INSTANCE_NAME`_ReadControlRegister() | `$INSTANCE_NAME`_CTRL_HD_SEND);
        `$INSTANCE_NAME`_RXBITCTR_PERIOD_REG = `$INSTANCE_NAME`_HD_TXBITCTR_INIT;
        #if(CY_UDB_V0) /* Manually clear status register when mode has been changed */
            /* Clear status register */
            CY_GET_REG8(`$INSTANCE_NAME`_RXSTATUS_PTR);
        #endif /* CY_UDB_V0 */
    }


    /*******************************************************************************
    * Function Name: `$INSTANCE_NAME`_LoadRxConfig
    ********************************************************************************
    *
    * Summary:
    *  Unloads the Tx configuration if required and loads the
    *  Rx configuration. It is the users responsibility to ensure that any
    *  transaction is complete and it is safe to unload the Rx
    *  configuration.
    *
    * Parameters:
    *  None.
    *
    * Return:
    *  None.
    *
    * Theory:
    *  Valid only for half duplex UART
    *
    * Side Effects:
    *  Set RX interrupt mask based on customizer settings, when software buffer 
    *  has been used.
    *
    *******************************************************************************/
    void `$INSTANCE_NAME`_LoadRxConfig(void) `=ReentrantKeil($INSTANCE_NAME . "_LoadRxConfig")`
    {
        `$INSTANCE_NAME`_WriteControlRegister(`$INSTANCE_NAME`_ReadControlRegister() & ~`$INSTANCE_NAME`_CTRL_HD_SEND);
        `$INSTANCE_NAME`_RXBITCTR_PERIOD_REG = `$INSTANCE_NAME`_HD_RXBITCTR_INIT;
        #if(CY_UDB_V0) /* Manually clear status register when mode has been changed */
            /* Clear status register */
            CY_GET_REG8(`$INSTANCE_NAME`_RXSTATUS_PTR);
        #endif /* CY_UDB_V0 */

        #if((`$INSTANCE_NAME`_RX_INTERRUPT_ENABLED) && (`$INSTANCE_NAME`_RXBUFFERSIZE > `$INSTANCE_NAME`_FIFO_LENGTH))
            /* Enable RX interrupt after set RX configuration */
            `$INSTANCE_NAME`_SetRxInterruptMode(`$INSTANCE_NAME`_INIT_RX_INTERRUPTS_MASK);    
        #endif /* `$INSTANCE_NAME`_RX_INTERRUPT_ENABLED */
    }

#endif  /* `$INSTANCE_NAME`_HD_ENABLED */


/* [] END OF FILE */
