/*******************************************************************************
* File Name: `$INSTANCE_NAME`_TX_RX_func.c
* Version `$CY_MAJOR_VERSION`.`$CY_MINOR_VERSION`
*
* Description:
*  There are fucntions process "Full" Receive and Transmit mailboxes:
*     - `$INSTANCE_NAME`_SendMsg0-7();
*     - `$INSTANCE_NAME`_ReceiveMsg0-15();
*  Transmition of message, and receive routine for "Basic" mailboxes:
*     - `$INSTANCE_NAME`_SendMsg();
*     - `$INSTANCE_NAME`_TxCancel();
*     - `$INSTANCE_NAME`_ReceiveMsg();
*
*  Note:
*   None
*
********************************************************************************
* Copyright 2008-2012, Cypress Semiconductor Corporation.  All rights reserved.
* You may use this file only in accordance with the license, terms, conditions,
* disclaimers, and limitations in the end user license agreement accompanying
* the software package with which this file was provided.
*******************************************************************************/

#include "`$INSTANCE_NAME`.h"

/* `#START TX_RX_FUNCTION` */

/* `#END` */


/*******************************************************************************
* FUNCTION NAME:   `$INSTANCE_NAME`_SendMsg
********************************************************************************
*
* Summary:
*  This function Send Message from one of Basic mailboxes. Function loop through
*  the transmit message buffer designed as Basic CAN mailboxes for first free
*  available and send from it. The number of retries is limited.
*
* Parameters:
*  message: Pointer to structure that contain all required data to send message.
*
* Return:
*  Indication if message has been sent.
*
*******************************************************************************/
uint8 `$INSTANCE_NAME`_SendMsg(`$INSTANCE_NAME`_TX_MSG *message) `=ReentrantKeil($INSTANCE_NAME . "_SendMsg")`
{
    uint8 i, j, shift;
    uint8 retry = 0u;
    uint8 result = `$INSTANCE_NAME`_FAIL;
    uint32 regTemp;

    while (retry < `$INSTANCE_NAME`_RETRY_NUMBER)
    {
        shift = 1u;
        for (i = 0u; i < `$INSTANCE_NAME`_NUMBER_OF_TX_MAILBOXES; i++)
        {
            /* Find Basic TX mailboxes */
            if ((`$INSTANCE_NAME`_TX_MAILBOX_TYPE & shift) == 0u)
            {
                /* Find free mailbox */
                if ((`$INSTANCE_NAME`_BUF_SR_REG.byte[2] & shift) == 0u)
                {
                    regTemp = 0u;

                    /* Set message parameters */                   
                    if ((message->ide) == `$INSTANCE_NAME`_STANDARD_MESSAGE)
                    {
                        `$INSTANCE_NAME`_SET_TX_ID_STANDARD_MSG(i, message->id);                        
                    }
                    else
                    {
                        regTemp |= `$INSTANCE_NAME`_TX_IDE_MASK;
                        `$INSTANCE_NAME`_SET_TX_ID_EXTENDED_MSG(i, message->id);
                    }
                    if (message->dlc < `$INSTANCE_NAME`_TX_DLC_MAX_VALUE)
                    {
                        regTemp |= ((uint32)message->dlc) << `$INSTANCE_NAME`_TWO_BYTE_OFFSET;
                    }
                    else
                    {
                        regTemp |= `$INSTANCE_NAME`_TX_DLC_UPPER_VALUE;
                    }
                    if ((message->irq) != `$INSTANCE_NAME`_TRANSMIT_INT_DISABLE)
                    {
                        regTemp |= `$INSTANCE_NAME`_TX_INT_ENABLE_MASK;    /* Transmit Interrupt Enable */
                    }

                    for(j = 0u; (j < message->dlc) && (j < `$INSTANCE_NAME`_TX_DLC_MAX_VALUE); j++)
                    {
                        `$INSTANCE_NAME`_TX_DATA_BYTE(i, j) = message->msg->byte[j];
                    }
                    
`$ISRDisable`
                    /* WPN[23] and WPN[3] set to 1 for write to CAN Control reg */
                    CY_SET_REG32((reg32 *) &`$INSTANCE_NAME`_TX[i].txcmd, (regTemp | `$INSTANCE_NAME`_TX_WPN_SET));
                    CY_SET_REG32((reg32 *) &`$INSTANCE_NAME`_TX[i].txcmd, `$INSTANCE_NAME`_SEND_MESSAGE);
                    
`$ISREnable`
                    result = CYRET_SUCCESS;
                }
            }
            shift <<= 1u;
            if (result == CYRET_SUCCESS)
            {
                break;
            }
        }
        if (result == CYRET_SUCCESS)
        {
            break;
        }
        else
        {
            retry++;
        }
    }

    return(result);
}


/*******************************************************************************
* FUNCTION NAME:   `$INSTANCE_NAME`_TxCancel
********************************************************************************
*
* Summary:
*  This function cancel transmission of a message that has been queued for
*  transmitted. Values between 0 and 15 are valid.
*
* Parameters:
*  bufferId: Mailbox number.
*
* Return:
*  None.
*
*******************************************************************************/
void `$INSTANCE_NAME`_TxCancel(uint8 bufferId) `=ReentrantKeil($INSTANCE_NAME . "_TxCancel")`
{
    if (bufferId < `$INSTANCE_NAME`_NUMBER_OF_TX_MAILBOXES)
    {
        `$INSTANCE_NAME`_TX_ABORT_MESSAGE(bufferId);
    }
}


#if (`$INSTANCE_NAME`_TX0_FUNC_ENABLE)

    /*******************************************************************************
    * FUNCTION NAME:   `$INSTANCE_NAME`_SendMsg`$TX_name0`
    ********************************************************************************
    *
    * Summary:
    *  This function is entry point to Transmit Message 0. Function check
    *  if mailbox 0 doesn't already have an un-transmitted messages waiting for
    *  arbitration. If not initiate transmission of the message.
    *  Only generated for Transmit mailbox designed as Full.
    *
    * Parameters:
    *  None.
    *
    * Return:
    *  Indication if Message has been sent.    
    *
    *******************************************************************************/
    uint8 `$INSTANCE_NAME`_SendMsg`$TX_name0`(void) `=ReentrantKeil($INSTANCE_NAME . "_SendMsg`$TX_name0`")`
    {
        uint8 result = CYRET_SUCCESS;
        
        if ((`$INSTANCE_NAME`_TX[0u].txcmd.byte[0u] & `$INSTANCE_NAME`_TX_REQUEST_PENDING) ==
            `$INSTANCE_NAME`_TX_REQUEST_PENDING)
        {
            result = `$INSTANCE_NAME`_FAIL;
        }
        else
        {
            /* `#START MESSAGE_`$TX_name0`_TRASMITTED` */
    
            /* `#END` */
            
            CY_SET_REG32((reg32 *) &`$INSTANCE_NAME`_TX[0u].txcmd, `$INSTANCE_NAME`_SEND_MESSAGE);
        }
    
        return(result);
    }
    
#endif /* `$INSTANCE_NAME`_TX0_FUNC_ENABLE */


#if (`$INSTANCE_NAME`_TX1_FUNC_ENABLE)

    /*******************************************************************************
    * FUNCTION NAME:   `$INSTANCE_NAME`_SendMsg`$TX_name1`
    ********************************************************************************
    *
    * Summary:
    *  This function is entry point to Transmit Message 1. Function check
    *  if mailbox 1 doesn't already have an un-transmitted messages waiting for
    *  arbitration. If not initiate transmission of the message.
    *  Only generated for Transmit mailbox designed as Full.
    *
    * Parameters:
    *  None.
    *
    * Return:
    *  Indication if Message has been sent.
    *
    *******************************************************************************/
    uint8 `$INSTANCE_NAME`_SendMsg`$TX_name1`(void) `=ReentrantKeil($INSTANCE_NAME . "_SendMsg`$TX_name1`")`
    {
        uint8 result = CYRET_SUCCESS;
        
        if ((`$INSTANCE_NAME`_TX[1u].txcmd.byte[0u] & `$INSTANCE_NAME`_TX_REQUEST_PENDING) ==
            `$INSTANCE_NAME`_TX_REQUEST_PENDING)
        {
            result = `$INSTANCE_NAME`_FAIL;
        }
        else
        {
            /* `#START MESSAGE_`$TX_name1`_TRASMITTED` */
    
            /* `#END` */
            
            CY_SET_REG32((reg32 *) & `$INSTANCE_NAME`_TX[1u].txcmd, `$INSTANCE_NAME`_SEND_MESSAGE);
        }
    
        return(result);
    }
    
#endif /* `$INSTANCE_NAME`_TX1_FUNC_ENABLE */


#if (`$INSTANCE_NAME`_TX2_FUNC_ENABLE)

    /*******************************************************************************
    * FUNCTION NAME:   `$INSTANCE_NAME`_SendMsg`$TX_name2`
    ********************************************************************************
    *
    * Summary:
    *  This function is entry point to Transmit Message 2. Function check
    *  if mailbox 2 doesn't already have an un-transmitted messages waiting for
    *  arbitration. If not initiate transmission of the message.
    *  Only generated for Transmit mailbox designed as Full.
    *
    * Parameters:
    *  None.
    *
    * Return:
    *  Indication if Message has been sent.
    *
    *******************************************************************************/
    uint8 `$INSTANCE_NAME`_SendMsg`$TX_name2`(void) `=ReentrantKeil($INSTANCE_NAME . "_SendMsg`$TX_name2`")`
    {
        uint8 result = CYRET_SUCCESS;
        
        if ((`$INSTANCE_NAME`_TX[2u].txcmd.byte[0u] & `$INSTANCE_NAME`_TX_REQUEST_PENDING) ==
            `$INSTANCE_NAME`_TX_REQUEST_PENDING)
        {
            result = `$INSTANCE_NAME`_FAIL;
        }
        else
        {
            /* `#START MESSAGE_`$TX_name2`_TRASMITTED` */
    
            /* `#END` */
            
            CY_SET_REG32((reg32 *) & `$INSTANCE_NAME`_TX[2u].txcmd, `$INSTANCE_NAME`_SEND_MESSAGE);
        }
    
        return(result);
    }

#endif /* `$INSTANCE_NAME`_TX2_FUNC_ENABLE */


#if (`$INSTANCE_NAME`_TX3_FUNC_ENABLE)

    /*******************************************************************************
    * FUNCTION NAME:   `$INSTANCE_NAME`_SendMsg`$TX_name3`
    ********************************************************************************
    *
    * Summary:
    *  This function is entry point to Transmit Message 3. Function check
    *  if mailbox 3 doesn't already have an un-transmitted messages waiting for
    *  arbitration. If not initiate transmission of the message.
    *  Only generated for Transmit mailbox designed as Full.
    *
    * Parameters:
    *  None.
    *
    * Return:
    *  Indication if Message has been sent.
    *
    *******************************************************************************/
    uint8 `$INSTANCE_NAME`_SendMsg`$TX_name3`(void) `=ReentrantKeil($INSTANCE_NAME . "_SendMsg`$TX_name3`")`
    {
        uint8 result = CYRET_SUCCESS;
        
        if ((`$INSTANCE_NAME`_TX[3u].txcmd.byte[0u] & `$INSTANCE_NAME`_TX_REQUEST_PENDING) ==
            `$INSTANCE_NAME`_TX_REQUEST_PENDING)
        {
            result = `$INSTANCE_NAME`_FAIL;
        }
        else
        {
            /* `#START MESSAGE_`$TX_name3`_TRASMITTED` */
    
            /* `#END` */
            
            CY_SET_REG32((reg32 *) & `$INSTANCE_NAME`_TX[3u].txcmd, `$INSTANCE_NAME`_SEND_MESSAGE);
        }
    
        return(result);
    }
    
#endif /* `$INSTANCE_NAME`_TX3_FUNC_ENABLE */


#if (`$INSTANCE_NAME`_TX4_FUNC_ENABLE)

    /*******************************************************************************
    * FUNCTION NAME:   `$INSTANCE_NAME`_SendMsg`$TX_name4`
    ********************************************************************************
    *
    * Summary:
    *  This function is entry point to Transmit Message 4. Function check if mailbox
    *  4 doesn't already have an un-transmitted messages waiting for arbitration. 
    *  If not initiate transmission of the message. Only generated for Transmit 
    *  mailbox designed as Full.
    *
    * Parameters:
    *  None.
    *
    * Return:
    *  Indication if Message has been sent.
    *
    *******************************************************************************/
    uint8 `$INSTANCE_NAME`_SendMsg`$TX_name4`(void) `=ReentrantKeil($INSTANCE_NAME . "_SendMsg`$TX_name4`")`
    {
        uint8 result = CYRET_SUCCESS;
        
        if ((`$INSTANCE_NAME`_TX[4u].txcmd.byte[0u] & `$INSTANCE_NAME`_TX_REQUEST_PENDING) ==
            `$INSTANCE_NAME`_TX_REQUEST_PENDING)
        {
            result = `$INSTANCE_NAME`_FAIL;
        }
        else
        {
            /* `#START MESSAGE_`$TX_name4`_TRASMITTED` */
    
            /* `#END` */
            
            CY_SET_REG32((reg32 *) & `$INSTANCE_NAME`_TX[4u].txcmd, `$INSTANCE_NAME`_SEND_MESSAGE);
        }
    
        return(result);
    }
    
#endif /* `$INSTANCE_NAME`_TX4_FUNC_ENABLE */


#if (`$INSTANCE_NAME`_TX5_FUNC_ENABLE)

    /*******************************************************************************
    * FUNCTION NAME:   `$INSTANCE_NAME`_SendMsg`$TX_name5`
    ********************************************************************************
    *
    * Summary:
    *  This function is entry point to Transmit Message 5. Function check
    *  if mailbox 5 doesn't already have an un-transmitted messages waiting for
    *  arbitration. If not initiate transmission of the message. Only generated for
    *  Transmit mailbox designed as Full.
    *
    * Parameters:
    *  None.
    *
    * Return:
    *  Indication if Message has been sent.
    *
    *******************************************************************************/
    uint8 `$INSTANCE_NAME`_SendMsg`$TX_name5`(void) `=ReentrantKeil($INSTANCE_NAME . "_SendMsg`$TX_name5`")`
    {
        uint8 result = CYRET_SUCCESS;
        
        if ((`$INSTANCE_NAME`_TX[5u].txcmd.byte[0u] & `$INSTANCE_NAME`_TX_REQUEST_PENDING) ==
            `$INSTANCE_NAME`_TX_REQUEST_PENDING)
        {
            result = `$INSTANCE_NAME`_FAIL;
        }
        else
        {
            /* `#START MESSAGE_`$TX_name5`_TRASMITTED` */
    
            /* `#END` */
            
            CY_SET_REG32((reg32 *) & `$INSTANCE_NAME`_TX[5u].txcmd, `$INSTANCE_NAME`_SEND_MESSAGE);
        }
    
        return(result);
    }
    
#endif /* `$INSTANCE_NAME`_TX5_FUNC_ENABLE */


#if (`$INSTANCE_NAME`_TX6_FUNC_ENABLE)

    /*******************************************************************************
    * FUNCTION NAME:   `$INSTANCE_NAME`_SendMsg`$TX_name6`
    ********************************************************************************
    *
    * Summary:
    *  This function is entry point to Transmit Message 6. Function check
    *  if mailbox 6 doesn't already have an un-transmitted messages waiting for
    *  arbitration. If not initiate transmission of the message. Only generated for
    *  Transmit mailbox designed as Full.
    *
    * Parameters:
    *  None.
    *
    * Return:
    *  Indication if Message has been sent.
    *
    *******************************************************************************/
    uint8 `$INSTANCE_NAME`_SendMsg`$TX_name6`(void) `=ReentrantKeil($INSTANCE_NAME . "_SendMsg`$TX_name6`")`
    {
        uint8 result = CYRET_SUCCESS;
        
        if ((`$INSTANCE_NAME`_TX[6u].txcmd.byte[0u] & `$INSTANCE_NAME`_TX_REQUEST_PENDING) ==
            `$INSTANCE_NAME`_TX_REQUEST_PENDING)
        {
            result = `$INSTANCE_NAME`_FAIL;
        }
        else
        {
            /* `#START MESSAGE_`$TX_name6`_TRASMITTED` */
    
            /* `#END` */
            
            CY_SET_REG32((reg32 *) & `$INSTANCE_NAME`_TX[6u].txcmd, `$INSTANCE_NAME`_SEND_MESSAGE);
        }
    
    return(result);
    }
    
#endif /* `$INSTANCE_NAME`_TX6_FUNC_ENABLE */


#if (`$INSTANCE_NAME`_TX7_FUNC_ENABLE)

    /*******************************************************************************
    * FUNCTION NAME:   `$INSTANCE_NAME`_SendMsg`$TX_name7`)
    ********************************************************************************
    *
    * Summary:
    *  This function is entry point to Transmit Message 7. Function check
    *  if mailbox 7 doesn't already have an un-transmitted messages waiting for
    *  arbitration. If not initiate transmission of the message. Only generated for
    *  Transmit mailbox designed as Full.
    *
    * Parameters:
    *  None.
    *
    * Return:
    *  Indication if Message has been sent.
    *
    *******************************************************************************/
    uint8 `$INSTANCE_NAME`_SendMsg`$TX_name7`(void) `=ReentrantKeil($INSTANCE_NAME . "_SendMsg`$TX_name7`")`
    {
        uint8 result = CYRET_SUCCESS;
        
        if ((`$INSTANCE_NAME`_TX[7u].txcmd.byte[0u] & `$INSTANCE_NAME`_TX_REQUEST_PENDING) ==
            `$INSTANCE_NAME`_TX_REQUEST_PENDING)
        {
            result = `$INSTANCE_NAME`_FAIL;
        }
        else
        {
            /* `#START MESSAGE_`$TX_name7`_TRASMITTED` */
    
            /* `#END` */
            
            CY_SET_REG32((reg32 *) & `$INSTANCE_NAME`_TX[7u].txcmd, `$INSTANCE_NAME`_SEND_MESSAGE);
        }
    
        return(result);
    }
    
#endif /* `$INSTANCE_NAME`_TX7_FUNC_ENABLE */


/*******************************************************************************
* FUNCTION NAME:   `$INSTANCE_NAME`_ReceiveMsg
********************************************************************************
*
* Summary:
*  This function is entry point to Receive Message Interrupt for Basic 
*  mailboxes. Clears Receive particular Message interrupt flag. Only generated 
*  if one of Receive mailboxes designed as Basic.
*
* Parameters:
*  rxMailbox: Mailbox number that trig Receive Message Interrupt.
*
* Return:
*  None.
*
* Reentrant:
*  Depends on Customer code.
*
*******************************************************************************/
void `$INSTANCE_NAME`_ReceiveMsg(uint8 rxMailbox) `=ReentrantKeil($INSTANCE_NAME . "_ReceiveMsg")`
{
    if ((`$INSTANCE_NAME`_RX[rxMailbox].rxcmd.byte[0u] & `$INSTANCE_NAME`_RX_ACK_MSG) == `$INSTANCE_NAME`_RX_ACK_MSG)
    {
        /* `#START MESSAGE_BASIC_RECEIVED` */

        /* `#END` */
        
        `$INSTANCE_NAME`_RX[rxMailbox].rxcmd.byte[0u] |= `$INSTANCE_NAME`_RX_ACK_MSG;
    }
}


#if (`$INSTANCE_NAME`_RX0_FUNC_ENABLE)

    /*******************************************************************************
    * FUNCTION NAME:   `$INSTANCE_NAME`_ReceiveMsg`$RX_name0`
    ********************************************************************************
    *
    * Summary:
    *  This function is entry point to Receive Message 0 Interrupt. Clears Receive
    *  Message 0 interrupt flag. Only generated for Receive mailbox designed as 
    *  Full.
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
    void `$INSTANCE_NAME`_ReceiveMsg`$RX_name0`(void) `=ReentrantKeil($INSTANCE_NAME . "_ReceiveMsg`$RX_name0`")`
    {
        /* `#START MESSAGE_`$RX_name0`_RECEIVED` */
    
        /* `#END` */
    
        `$RX_ack0`    }
    
#endif /* `$INSTANCE_NAME`_RX0_FUNC_ENABLE */


#if (`$INSTANCE_NAME`_RX1_FUNC_ENABLE)

    /*******************************************************************************
    * FUNCTION NAME:    `$INSTANCE_NAME`_ReceiveMsg`$RX_name1`
    ********************************************************************************
    *
    * Summary:
    *  This function is entry point to Receive Message 1 Interrupt. Clears Receive
    *  Message 1 interrupt flag. Only generated for Receive mailbox designed as 
    *  Full.
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
    void `$INSTANCE_NAME`_ReceiveMsg`$RX_name1`(void) `=ReentrantKeil($INSTANCE_NAME . "_ReceiveMsg`$RX_name1`")`
    {
        /* `#START MESSAGE_`$RX_name1`_RECEIVED` */
    
        /* `#END` */
    
        `$RX_ack1`    }
    
#endif /* `$INSTANCE_NAME`_RX1_FUNC_ENABLE */


#if (`$INSTANCE_NAME`_RX2_FUNC_ENABLE)

    /*******************************************************************************
    * FUNCTION NAME:   `$INSTANCE_NAME`_ReceiveMsg`$RX_name2`
    ********************************************************************************
    *
    * Summary:
    *  This function is entry point to Receive Message 2 Interrupt. Clears Receive
    *  Message 2 interrupt flag. Only generated for Receive mailbox designed as
    *  Full.
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
    void `$INSTANCE_NAME`_ReceiveMsg`$RX_name2`(void) `=ReentrantKeil($INSTANCE_NAME . "_ReceiveMsg`$RX_name2`")`
    {
        /* `#START MESSAGE_`$RX_name2`_RECEIVED` */
    
        /* `#END` */
    
        `$RX_ack2`    }
    
#endif /* `$INSTANCE_NAME`_RX2_FUNC_ENABLE */


#if (`$INSTANCE_NAME`_RX3_FUNC_ENABLE)

    /*******************************************************************************
    * FUNCTION NAME:   `$INSTANCE_NAME`_ReceiveMsg`$RX_name3`
    ********************************************************************************
    *
    * Summary:
    *  This function is entry point to Receive Message 3 Interrupt. Clears Receive
    *  Message 3 interrupt flag. Only generated for Receive mailbox designed as
    *  Full.
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
    void `$INSTANCE_NAME`_ReceiveMsg`$RX_name3`(void) `=ReentrantKeil($INSTANCE_NAME . "_ReceiveMsg`$RX_name3`")`
    {
        /* `#START MESSAGE_`$RX_name3`_RECEIVED` */
    
        /* `#END` */
    
        `$RX_ack3`    }
    
#endif /* `$INSTANCE_NAME`_RX3_FUNC_ENABLE */


#if (`$INSTANCE_NAME`_RX4_FUNC_ENABLE)

    /*******************************************************************************
    * FUNCTION NAME:   `$INSTANCE_NAME`_ReceiveMsg`$RX_name4`
    ********************************************************************************
    *
    * Summary:
    *  This function is entry point to Receive Message 4 Interrupt. Clears Receive
    *  Message 4 interrupt flag. Only generated for Receive mailbox designed as
    *  Full.
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
    void `$INSTANCE_NAME`_ReceiveMsg`$RX_name4`(void) `=ReentrantKeil($INSTANCE_NAME . "_ReceiveMsg`$RX_name4`")`
    {
        /* `#START MESSAGE_`$RX_name4`_RECEIVED` */
    
        /* `#END` */
    
        `$RX_ack4`    }
    
#endif /* `$INSTANCE_NAME`_RX4_FUNC_ENABLE */


#if (`$INSTANCE_NAME`_RX5_FUNC_ENABLE)

    /*******************************************************************************
    * FUNCTION NAME:   `$INSTANCE_NAME`_ReceiveMsg`$RX_name5`
    ********************************************************************************
    *
    * Summary:
    *  This function is entry point to Receive Message 5 Interrupt. Clears Receive
    *  Message 5 interrupt flag. Only generated for Receive mailbox designed as
    *  Full.
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
    void `$INSTANCE_NAME`_ReceiveMsg`$RX_name5`(void) `=ReentrantKeil($INSTANCE_NAME . "_ReceiveMsg`$RX_name5`")`
    {
        /* `#START MESSAGE_`$RX_name5`_RECEIVED` */
    
        /* `#END` */
    
        `$RX_ack5`    }
    
#endif /* `$INSTANCE_NAME`_RX5_FUNC_ENABLE */


#if (`$INSTANCE_NAME`_RX6_FUNC_ENABLE)

    /*******************************************************************************
    * FUNCTION NAME:   `$INSTANCE_NAME`_ReceiveMsg`$RX_name6`
    ********************************************************************************
    *
    * Summary:
    *  This function is entry point to Receive Message 6 Interrupt. Clears Receive
    *  Message 6 interrupt flag. Only generated for Receive mailbox designed as
    *  Full.
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
    void `$INSTANCE_NAME`_ReceiveMsg`$RX_name6`(void) `=ReentrantKeil($INSTANCE_NAME . "_ReceiveMsg`$RX_name6`")`
    {
        /* `#START MESSAGE_`$RX_name6`_RECEIVED` */
    
        /* `#END` */
    
        `$RX_ack6`    }
    
#endif /* `$INSTANCE_NAME`_RX6_FUNC_ENABLE */


#if (`$INSTANCE_NAME`_RX7_FUNC_ENABLE)

    /*******************************************************************************
    * FUNCTION NAME:   `$INSTANCE_NAME`_ReceiveMsg`$RX_name7`
    ********************************************************************************
    *
    * Summary:
    *  This function is entry point to Receive Message 7 Interrupt. Clears Receive
    *  Message 7 interrupt flag. Only generated for Receive mailbox designed as
    *  Full.
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
    void `$INSTANCE_NAME`_ReceiveMsg`$RX_name7`(void) `=ReentrantKeil($INSTANCE_NAME . "_ReceiveMsg`$RX_name7`")`
    {
        /* `#START MESSAGE_`$RX_name7`_RECEIVED` */
    
        /* `#END` */
    
        `$RX_ack7`    }
    
#endif /* `$INSTANCE_NAME`_RX7_FUNC_ENABLE */


#if (`$INSTANCE_NAME`_RX8_FUNC_ENABLE)

    /*******************************************************************************
    * FUNCTION NAME:   `$INSTANCE_NAME`_ReceiveMsg`$RX_name8`
    ********************************************************************************
    *
    * Summary:
    *  This function is entry point to Receive Message 8 Interrupt. Clears Receive
    *  Message 8 interrupt flag. Only generated for Receive mailbox designed as
    *  Full.
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
    void `$INSTANCE_NAME`_ReceiveMsg`$RX_name8`(void) `=ReentrantKeil($INSTANCE_NAME . "_ReceiveMsg`$RX_name8`")`
    {
        /* `#START MESSAGE_`$RX_name8`_RECEIVED` */
    
        /* `#END` */
    
        `$RX_ack8`    }
    
#endif /* `$INSTANCE_NAME`_RX8_FUNC_ENABLE */


#if (`$INSTANCE_NAME`_RX9_FUNC_ENABLE)

    /*******************************************************************************
    * FUNCTION NAME:   `$INSTANCE_NAME`_ReceiveMsg`$RX_name9`
    ********************************************************************************
    *
    * Summary:
    *  This function is entry point to Receive Message 9 Interrupt. Clears Receive
    *  Message 9 interrupt flag. Only generated for Receive mailbox designed as
    *  Full.
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
    void `$INSTANCE_NAME`_ReceiveMsg`$RX_name9`(void) `=ReentrantKeil($INSTANCE_NAME . "_ReceiveMsg`$RX_name9`")`
    {
        /* `#START MESSAGE_`$RX_name9`_RECEIVED` */
    
        /* `#END` */
    
        `$RX_ack9`    }
    
#endif /* `$INSTANCE_NAME`_RX9_FUNC_ENABLE */


#if (`$INSTANCE_NAME`_RX10_FUNC_ENABLE)

    /*******************************************************************************
    * FUNCTION NAME:   `$INSTANCE_NAME`_ReceiveMsg`$RX_name10`
    ********************************************************************************
    *
    * Summary:
    *  This function is entry point to Receive Message 10 Interrupt. Clears Receive
    *  Message 10 interrupt flag. Only generated for Receive mailbox designed as
    *  Full.
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
    void `$INSTANCE_NAME`_ReceiveMsg`$RX_name10`(void) `=ReentrantKeil($INSTANCE_NAME . "_ReceiveMsg`$RX_name10`")`
    {
        /* `#START MESSAGE_`$RX_name10`_RECEIVED` */
    
        /* `#END` */
    
        `$RX_ack10`    }
    
#endif /* `$INSTANCE_NAME`_RX10_FUNC_ENABLE */


#if (`$INSTANCE_NAME`_RX11_FUNC_ENABLE)

    /*******************************************************************************
    * FUNCTION NAME:   `$INSTANCE_NAME`_ReceiveMsg`$RX_name11`
    ********************************************************************************
    *
    * Summary:
    *  This function is entry point to Receive Message 11 Interrupt. Clears Receive
    *  Message 11 interrupt flag. Only generated for Receive mailbox designed as
    *  Full.
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
    void `$INSTANCE_NAME`_ReceiveMsg`$RX_name11`(void) `=ReentrantKeil($INSTANCE_NAME . "_ReceiveMsg`$RX_name11`")`
    {
        /* `#START MESSAGE_`$RX_name11`_RECEIVED` */
    
        /* `#END` */
    
        `$RX_ack11`    }
    
#endif /* `$INSTANCE_NAME`_RX11_FUNC_ENABLE */


#if (`$INSTANCE_NAME`_RX12_FUNC_ENABLE)

    /*******************************************************************************
    * FUNCTION NAME:   `$INSTANCE_NAME`_ReceiveMsg`$RX_name12`
    ********************************************************************************
    *
    * Summary:
    *  This function is entry point to Receive Message 12 Interrupt. Clears Receive
    *  Message 12 interrupt flag. Only generated for Receive mailbox designed as
    *  Full.
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
    void `$INSTANCE_NAME`_ReceiveMsg`$RX_name12`(void) `=ReentrantKeil($INSTANCE_NAME . "_ReceiveMsg`$RX_name12`")`
    {
        /* `#START MESSAGE_`$RX_name12`_RECEIVED` */
    
        /* `#END` */
    
        `$RX_ack12`    }
    
#endif /* `$INSTANCE_NAME`_RX12_FUNC_ENABLE */


#if (`$INSTANCE_NAME`_RX13_FUNC_ENABLE)

    /*******************************************************************************
    * FUNCTION NAME:   `$INSTANCE_NAME`_ReceiveMsg`$RX_name13`
    ********************************************************************************
    *
    * Summary:
    *  This function is entry point to Receive Message 13 Interrupt. Clears Receive
    *  Message 13 interrupt flag. Only generated for Receive mailbox designed as
    *  Full.
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
    void `$INSTANCE_NAME`_ReceiveMsg`$RX_name13`(void) `=ReentrantKeil($INSTANCE_NAME . "_ReceiveMsg`$RX_name13`")`
    {
        /* `#START MESSAGE_`$RX_name13`_RECEIVED` */
    
        /* `#END` */
    
        `$RX_ack13`    }
    
#endif /* `$INSTANCE_NAME`_RX13_FUNC_ENABLE */


#if (`$INSTANCE_NAME`_RX14_FUNC_ENABLE)

    /*******************************************************************************
    * FUNCTION NAME:   `$INSTANCE_NAME`_ReceiveMsg`$RX_name14`
    ********************************************************************************
    *
    * Summary:
    *  This function is entry point to Receive Message 14 Interrupt. Clears Receive
    *  Message 14 interrupt flag. Only generated for Receive mailbox designed as
    *  Full.
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
    void `$INSTANCE_NAME`_ReceiveMsg`$RX_name14`(void) `=ReentrantKeil($INSTANCE_NAME . "_ReceiveMsg`$RX_name14`")`
    {
        /* `#START MESSAGE_`$RX_name14`_RECEIVED` */
    
        /* `#END` */
    
        `$RX_ack14`    }
    
#endif /* `$INSTANCE_NAME`_RX14_FUNC_ENABLE */


#if (`$INSTANCE_NAME`_RX15_FUNC_ENABLE)

    /*******************************************************************************
    * FUNCTION NAME:   `$INSTANCE_NAME`_ReceiveMsg`$RX_name15`
    ********************************************************************************
    *
    * Summary:
    *  This function is entry point to Receive Message 15 Interrupt. Clears Receive
    *  Message 15 interrupt flag. Only generated for Receive mailbox designed as
    *  Full.
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
    void `$INSTANCE_NAME`_ReceiveMsg`$RX_name15`(void) `=ReentrantKeil($INSTANCE_NAME . "_ReceiveMsg`$RX_name15`")`
    {
        /* `#START MESSAGE_`$RX_name15`_RECEIVED` */
    
        /* `#END` */
    
        `$RX_ack15`    }
    
#endif /* `$INSTANCE_NAME`_RX15_FUNC_ENABLE */


/* [] END OF FILE */
