/*******************************************************************************
* File Name: `$INSTANCE_NAME`_INT.c
* Version `$CY_MAJOR_VERSION`.`$CY_MINOR_VERSION`
*
* Description:
*  This file contains the Interrupt Service Routine (ISR) for the CAN
*  Component. Interrupt handlers functions generate accordingly to PSoC Creator 
*  Customizer inputs.
*
* Note:
*  None
*
********************************************************************************
* Copyright 2008-2010, Cypress Semiconductor Corporation.  All rights reserved.
* You may use this file only in accordance with the license, terms, conditions,
* disclaimers, and limitations in the end user license agreement accompanying
* the software package with which this file was provided.
*******************************************************************************/

#include "`$INSTANCE_NAME`.h"

/* `#START CAN_INT_C_CODE_DEFINITION` */

/* `#END` */


#if (`$INSTANCE_NAME`_ARB_LOST)

    /*******************************************************************************
    * FUNCTION NAME:   `$INSTANCE_NAME`_ArbLost
    ********************************************************************************
    *
    * Summary:
    *  This function is entry point to Arbitration Lost Interrupt. Clears
    *  Arbitration Lost interrupt flag. Only generated if Arbitration Lost
    *  Interrupt enable in Customizer.
    *
    * Parameters:
    *  None.
    *
    * Return:
    *  None.
    * 
    * Reentrant:
    *  Depends on Customer code.
    *
    *******************************************************************************/
    void `$INSTANCE_NAME`_ArbLostIsr(void) `=ReentrantKeil($INSTANCE_NAME . "_ArbLostIsr")`
    {
        /* `#START ARBITRATION_LOST_ISR` */
    
        /* `#END` */
    
        /* Clear Arbitration Lost flag */
        `$INSTANCE_NAME`_INT_SR_REG.byte[0u] |= `$INSTANCE_NAME`_ARBITRATION_LOST_MASK;
    }
    
#endif /* `$INSTANCE_NAME`_ARB_LOST */


#if (`$INSTANCE_NAME`_OVERLOAD)

    /*******************************************************************************
    * FUNCTION NAME:   `$INSTANCE_NAME`_OvrLdErrror
    ********************************************************************************
    *
    * Summary:
    *  This function is entry point to Overload Error Interrupt. Clears Overload
    *  Error interrupt flag. Only generated if Overload Error Interrupt enable
    *  in Customizer.
    *
    * Parameters:
    *  None.
    *
    * Return:
    *  None.        
    *
    * Reentrant:
    *  Depends on Customer code.
    *
    *******************************************************************************/
    void `$INSTANCE_NAME`_OvrLdErrorIsr(void) `=ReentrantKeil($INSTANCE_NAME . "_OvrLdErrorIsr")`
    {
        /* `#START OVER_LOAD_ERROR_ISR` */
    
        /* `#END` */
    
        /* Clear Overload Error flag */
        `$INSTANCE_NAME`_INT_SR_REG.byte[0u] |= `$INSTANCE_NAME`_OVERLOAD_ERROR_MASK;
    }
    
#endif /* `$INSTANCE_NAME`_OVERLOAD */


#if (`$INSTANCE_NAME`_BIT_ERR)

    /*******************************************************************************
    * FUNCTION NAME:   `$INSTANCE_NAME`_BitError
    ********************************************************************************
    *
    * Summary:
    *  This function is entry point to Bit Error Interrupt. Clears Bit Error
    *  interrupt flag. Only generated if Bit Error Interrupt enable in Customizer.
    *
    * Parameters:
    *  None.
    *
    * Return:
    *  None.
    *
    * Reentrant:
    *  Depends on Customer code.
    *
    *******************************************************************************/
    void `$INSTANCE_NAME`_BitErrorIsr(void) `=ReentrantKeil($INSTANCE_NAME . "_BitErrorIsr")`
    {
        /* `#START BIT_ERROR_ISR` */
    
        /* `#END` */
    
        /* Clear Bit Error flag */
        `$INSTANCE_NAME`_INT_SR_REG.byte[0u] |= `$INSTANCE_NAME`_BIT_ERROR_MASK;
    }
    
#endif /* `$INSTANCE_NAME`_BIT_ERR */


#if (`$INSTANCE_NAME`_STUFF_ERR)

    /*******************************************************************************
    * FUNCTION NAME:   `$INSTANCE_NAME`_BitStuffError
    ********************************************************************************
    *
    * Summary:
    *  This function is entry point to Bit Stuff Error Interrupt. Clears Bit Stuff
    *  Error interrupt flag. Only generated if Bit Stuff Error Interrupt enable
    *  in Customizer.
    *
    * Parameters:
    *  None.
    *
    * Return:
    *  None.
    *
    * Reentrant:
    *  Depends on Customer code.
    *
    *******************************************************************************/
    void `$INSTANCE_NAME`_BitStuffErrorIsr(void) `=ReentrantKeil($INSTANCE_NAME . "_BitStuffErrorIsr")`
    {
        /* `#START BIT_STUFF_ERROR_ISR` */
    
        /* `#END` */
    
        /* Clear Stuff Error flag */
        `$INSTANCE_NAME`_INT_SR_REG.byte[0u] |= `$INSTANCE_NAME`_STUFF_ERROR_MASK;
    }
    
#endif /* `$INSTANCE_NAME`_STUFF_ERR */


#if (`$INSTANCE_NAME`_ACK_ERR)

    /*******************************************************************************
    * FUNCTION NAME:   `$INSTANCE_NAME`_AckErrorIsr
    ********************************************************************************
    *
    * Summary:
    *  This function is entry point to Acknowledge Error Interrupt. Clears
    *  Acknowledge Error interrupt flag. Only generated if Acknowledge Error
    *  Interrupt enable in Customizer.
    *
    * Parameters:
    *  None.
    *
    * Return:
    *  None.
    *
    * Reentrant:
    *  Depends on Customer code.
    *
    *******************************************************************************/
    void `$INSTANCE_NAME`_AckErrorIsr(void) `=ReentrantKeil($INSTANCE_NAME . "_AckErrorIsr")`
    {
        /* `#START ACKNOWLEDGE_ERROR_ISR` */
    
        /* `#END` */
    
        /* Clear Acknoledge Error flag */
        `$INSTANCE_NAME`_INT_SR_REG.byte[0u] |= `$INSTANCE_NAME`_ACK_ERROR_MASK;
    }
    
#endif /* `$INSTANCE_NAME`_ACK_ERR */


#if (`$INSTANCE_NAME`_FORM_ERR)

    /*******************************************************************************
    * FUNCTION NAME:   `$INSTANCE_NAME`_MsgError
    ********************************************************************************
    *
    * Summary:
    *  This function is entry point to Form Error Interrupt. Clears Form Error
    *  interrupt flag. Only generated if Form Error Interrupt enable in Customizer.
    *
    * Parameters:
    *  None.
    *
    * Return:
    *  None.
    *
    * Reentrant:
    *  Depends on Customer code.
    *
    *******************************************************************************/
    void `$INSTANCE_NAME`_MsgErrorIsr(void) `=ReentrantKeil($INSTANCE_NAME . "_MsgErrorIsr")`
    {
        /* `#START MESSAGE_ERROR_ISR` */
    
        /* `#END` */
    
        /* Clear Form Error flag */
        `$INSTANCE_NAME`_INT_SR_REG.byte[0u] |= `$INSTANCE_NAME`_FORM_ERROR_MASK;
    }
    
#endif /* `$INSTANCE_NAME`_FORM_ERR */


#if (`$INSTANCE_NAME`_CRC_ERR)

    /*******************************************************************************
    * FUNCTION NAME:   `$INSTANCE_NAME`_CrcError
    ********************************************************************************
    *
    * Summary:
    *  This function is entry point to CRC Error Interrupt. Clears CRC Error
    *  interrupt flag. Only generated if CRC Error Interrupt enable in Customizer.
    *
    * Parameters:
    *  None.
    *
    * Return:
    *  None.
    *
    * Reentrant:
    *  Depends on Customer code.
    *
    *******************************************************************************/
    void `$INSTANCE_NAME`_CrcErrorIsr(void) `=ReentrantKeil($INSTANCE_NAME . "_CrcErrorIsr")`
    {
        /* `#START CRC_ERROR_ISR` */
    
        /* `#END` */
    
        /* Clear CRC Error flag */
        `$INSTANCE_NAME`_INT_SR_REG.byte[1u] |= `$INSTANCE_NAME`_CRC_ERROR_MASK;
    }
    
#endif /* `$INSTANCE_NAME`_CRC_ERR */


#if (`$INSTANCE_NAME`_BUS_OFF)

    /*******************************************************************************
    * FUNCTION NAME:   `$INSTANCE_NAME`_BusOff
    ********************************************************************************
    *
    * Summary:
    *  This function is entry point to Bus Off Interrupt. Places CAN Component
    *  to Stop mode. Only generated if Bus Off Interrupt enable in Customizer.
    *  Recommended setting to enable this interrupt.
    *
    * Parameters:
    *  None.
    *
    * Return:
    *  None.
    *
    * Side Effects:
    *  Disable power to the CAN Core.
    *
    *******************************************************************************/
    void `$INSTANCE_NAME`_BusOffIsr(void) `=ReentrantKeil($INSTANCE_NAME . "_BusOffIsr")`
    {
        /* `#START BUS_OFF_ISR` */
    
        /* `#END` */
    
        /* Clear Bus Off flag */
        `$INSTANCE_NAME`_INT_SR_REG.byte[1u] |= `$INSTANCE_NAME`_BUS_OFF_MASK;
        `$INSTANCE_NAME`_Stop();
    }
    
#endif /* `$INSTANCE_NAME`_BUS_OFF */


#if (`$INSTANCE_NAME`_RX_MSG_LOST)

    /*******************************************************************************
    * FUNCTION NAME:   `$INSTANCE_NAME`_MsgLost
    ********************************************************************************
    *
    * Summary:
    *  This function is entry point to Message Lost Interrupt. Clears Message Lost
    *  interrupt flag. Only generated if Message Lost Interrupt enable in Customizer
    *
    * Parameters:
    *  None.
    *
    * Return:
    *  None.
    *
    * Reentrant:
    *  Depends on Customer code.
    *
    *******************************************************************************/
    void `$INSTANCE_NAME`_MsgLostIsr(void) `=ReentrantKeil($INSTANCE_NAME . "_MsgLostIsr")`
    {
        /* `#START MESSAGE_LOST_ISR` */
    
        /* `#END` */
    
        /* Clear Receive Message Lost flag */
        `$INSTANCE_NAME`_INT_SR_REG.byte[1u] |= `$INSTANCE_NAME`_RX_MSG_LOST_MASK;
    }
    
#endif /* `$INSTANCE_NAME`_RX_MSG_LOST */


#if (`$INSTANCE_NAME`_TX_MESSAGE)

    /*******************************************************************************
    * FUNCTION NAME:   `$INSTANCE_NAME`_MsgTXIsr
    ********************************************************************************
    *
    * Summary:
    *  This function is entry point to Transmit Message Interrupt. Clears Transmit
    *  Message interrupt flag. Only generated if Transmit Message Interrupt enable
    *  in Customizer.
    *
    * Parameters:
    *  None.
    *
    * Return:
    *  None.    
    *
    * Reentrant:
    *  Depends on Customer code.
    *
    *******************************************************************************/
    void `$INSTANCE_NAME`_MsgTXIsr(void) `=ReentrantKeil($INSTANCE_NAME . "_MsgTXIsr")`
    {
        /* Clear Transmit Message flag */
        `$INSTANCE_NAME`_INT_SR_REG.byte[1u] |= `$INSTANCE_NAME`_TX_MESSAGE_MASK;
        
        /* `#START MESSAGE_TRANSMITTED_ISR` */
    
        /* `#END` */
    }
    
#endif /* `$INSTANCE_NAME`_TX_MESSAGE */


#if (`$INSTANCE_NAME`_RX_MESSAGE)

    /*******************************************************************************
    * FUNCTION NAME:   `$INSTANCE_NAME`_MsgRXIsr
    ********************************************************************************
    *
    * Summary:
    *  This function is entry point to Receive Message Interrupt. Clears Receive
    *  Message interrupt flag and call appropriate handlers for Basic and Full
    *  interrupt based mailboxes. Only generated if Receive Message Interrupt
    *  enable in Customizer. Recommended setting to enable this interrupt.
    *
    * Parameters:
    *  None.
    *
    * Return:
    *  None.
    *
    *******************************************************************************/
    void `$INSTANCE_NAME`_MsgRXIsr(void) `=ReentrantKeil($INSTANCE_NAME . "_MsgRXIsr")`
    {
        uint8 i;
        uint16 shift = 0x01;
    
        /* Clear Receive Message flag */
        `$INSTANCE_NAME`_INT_SR_REG.byte[1u] |= `$INSTANCE_NAME`_RX_MESSAGE_MASK;
        
        /* `#START MESSAGE_RECEIVE_ISR` */
    
        /* `#END` */
    
        for (i = 0u; i < `$INSTANCE_NAME`_NUMBER_OF_RX_MAILBOXES; i++)
        {
            if ((CY_GET_REG16((reg16 *) &`$INSTANCE_NAME`_BUF_SR_REG.byte[0u]) & shift) == shift)
            {
                if ((`$INSTANCE_NAME`_RX[i].rxcmd.byte[0u] & `$INSTANCE_NAME`_RX_INT_ENABLE_MASK) ==
                    `$INSTANCE_NAME`_RX_INT_ENABLE_MASK)
                {
                    if ((`$INSTANCE_NAME`_RX_MAILBOX_TYPE & shift) == shift)
                    {
                        /* RX Full mailboxes handler */
                        switch(i)
                        {
    `$APIRxIRQHandleFunction` 
                            default:
                                break;
                        }
    
                    }
                    else
                    {
                        /* RX Basic mailbox handler */
                        `$INSTANCE_NAME`_ReceiveMsg(i);
                    }
                }
            }
            shift <<= 1u;
        }            
    }
    
#endif /* `$INSTANCE_NAME`_RX_MESSAGE */


/*******************************************************************************
* Function Name: `$INSTANCE_NAME`_ISR
********************************************************************************
*
* Summary:
*  This ISR is executed when CAN core generate interrupt on one of events:
*  Arb_lost, Overload, Bit_err, Stuff_err, Ack_err, Form_err, Crc_err,
*  Buss_off, Rx_msg_lost, Tx_msg or Rx_msg. The interrupt sources depends
*  on Customizer inputs.
*
* Parameters:
*  None.
*
* Return:
*  None.
*
*******************************************************************************/
CY_ISR(`$INSTANCE_NAME`_ISR)
{
    /* Arbitration */
    #if (`$INSTANCE_NAME`_ARB_LOST)    
        if ((`$INSTANCE_NAME`_INT_SR_REG.byte[0u] & `$INSTANCE_NAME`_ARBITRATION_LOST_MASK) ==
             `$INSTANCE_NAME`_ARBITRATION_LOST_MASK)
        {
            `$INSTANCE_NAME`_ArbLostIsr();
        } 
    #endif /* `$INSTANCE_NAME`_ARB_LOST */
    
    /* Overload Error */
    #if (`$INSTANCE_NAME`_OVERLOAD)
        if ((`$INSTANCE_NAME`_INT_SR_REG.byte[0u] & `$INSTANCE_NAME`_OVERLOAD_ERROR_MASK) ==
             `$INSTANCE_NAME`_OVERLOAD_ERROR_MASK)
        {
            `$INSTANCE_NAME`_OvrLdErrorIsr();
        }
    #endif /* `$INSTANCE_NAME`_OVERLOAD */
    
    /* Bit Error */
    #if (`$INSTANCE_NAME`_BIT_ERR)
        if ((`$INSTANCE_NAME`_INT_SR_REG.byte[0u] & `$INSTANCE_NAME`_BIT_ERROR_MASK) == `$INSTANCE_NAME`_BIT_ERROR_MASK)
        {
            `$INSTANCE_NAME`_BitErrorIsr();
        }
    #endif /* `$INSTANCE_NAME`_BIT_ERR */
    
    /* Bit Staff Error */
    #if (`$INSTANCE_NAME`_STUFF_ERR)
        if ((`$INSTANCE_NAME`_INT_SR_REG.byte[0u] & `$INSTANCE_NAME`_STUFF_ERROR_MASK) ==
             `$INSTANCE_NAME`_STUFF_ERROR_MASK)
        {
            `$INSTANCE_NAME`_BitStuffErrorIsr();
        }
    #endif /* `$INSTANCE_NAME`_STUFF_ERR */
    
    /* ACK Error */
    #if (`$INSTANCE_NAME`_ACK_ERR)
        if ((`$INSTANCE_NAME`_INT_SR_REG.byte[0u] & `$INSTANCE_NAME`_ACK_ERROR_MASK) ==
             `$INSTANCE_NAME`_ACK_ERROR_MASK)
        {
            `$INSTANCE_NAME`_AckErrorIsr();
        }
    #endif /* `$INSTANCE_NAME`_ACK_ERR */
    
    /* Form(msg) Error */
    #if (`$INSTANCE_NAME`_FORM_ERR)
        if ((`$INSTANCE_NAME`_INT_SR_REG.byte[0u] & `$INSTANCE_NAME`_FORM_ERROR_MASK) ==
             `$INSTANCE_NAME`_FORM_ERROR_MASK)
        {
            `$INSTANCE_NAME`_MsgErrorIsr();
        }
    #endif /* `$INSTANCE_NAME`_FORM_ERR */
    
    /* CRC Error */
    #if (`$INSTANCE_NAME`_CRC_ERR)
        if ((`$INSTANCE_NAME`_INT_SR_REG.byte[1u] & `$INSTANCE_NAME`_CRC_ERROR_MASK) == `$INSTANCE_NAME`_CRC_ERROR_MASK)
        {
            `$INSTANCE_NAME`_CrcErrorIsr();
        }
    #endif /* `$INSTANCE_NAME`_CRC_ERR */
    
    /* Bus Off state */
    #if (`$INSTANCE_NAME`_BUS_OFF)
        if ((`$INSTANCE_NAME`_INT_SR_REG.byte[1u] & `$INSTANCE_NAME`_BUS_OFF_MASK) == `$INSTANCE_NAME`_BUS_OFF_MASK)
        {
            `$INSTANCE_NAME`_BusOffIsr();
        }
    #endif /* `$INSTANCE_NAME`_BUS_OFF */
    
    /* Message Lost */
    #if (`$INSTANCE_NAME`_RX_MSG_LOST)
        if ((`$INSTANCE_NAME`_INT_SR_REG.byte[1u] & `$INSTANCE_NAME`_RX_MSG_LOST_MASK) ==
             `$INSTANCE_NAME`_RX_MSG_LOST_MASK)
        {
            `$INSTANCE_NAME`_MsgLostIsr();
        }
    #endif /* `$INSTANCE_NAME`_RX_MSG_LOST */
    
    /* TX Message Send */
    #if (`$INSTANCE_NAME`_TX_MESSAGE)
        if ((`$INSTANCE_NAME`_INT_SR_REG.byte[1u] & `$INSTANCE_NAME`_TX_MESSAGE_MASK) ==
             `$INSTANCE_NAME`_TX_MESSAGE_MASK)
        {
            `$INSTANCE_NAME`_MsgTXIsr();
        }
    #endif /* `$INSTANCE_NAME`_TX_MESSAGE */
    
    /* RX Message Available */
    #if (`$INSTANCE_NAME`_RX_MESSAGE)
        if ((`$INSTANCE_NAME`_INT_SR_REG.byte[1u] & `$INSTANCE_NAME`_RX_MESSAGE_MASK) ==
             `$INSTANCE_NAME`_RX_MESSAGE_MASK)
        {
            `$INSTANCE_NAME`_MsgRXIsr();
        }
    #endif /* `$INSTANCE_NAME`_RX_MESSAGE */
    
    /* PSoC3 ES1, ES2 `$INSTANCE_NAME` ISR PATCH  */     
    #if(`$INSTANCE_NAME`_PSOC3_ES2 && (`$INSTANCE_NAME`_isr__ES2_PATCH))
        `$INSTANCE_NAME`_ISR_PATCH();
    #endif /* End `$INSTANCE_NAME`_PSOC3_ES2*/       
}


/* [] END OF FILE */
