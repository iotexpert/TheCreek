/*******************************************************************************
* File Name: `$INSTANCE_NAME`_INT.c
* Version `$CY_MAJOR_VERSION`.`$CY_MINOR_VERSION`
*
* Description:
*  This file provides the source code of Interrupt Service Routine (ISR)
*  for I2C component.
*
*  Note:
*
********************************************************************************
* Copyright 2008-2012, Cypress Semiconductor Corporation.  All rights reserved.
* You may use this file only in accordance with the license, terms, conditions,
* disclaimers, and limitations in the end user license agreement accompanying
* the software package with which this file was provided.
*******************************************************************************/

#include "`$INSTANCE_NAME`.h"

/*******************************************************************************
*  Place your includes, defines and code here
********************************************************************************/
/* `#START `$INSTANCE_NAME`_ISR_intc` */

/* `#END` */


/**********************************
*      System variables
**********************************/

extern volatile uint8 `$INSTANCE_NAME`_state;   /* Current state of I2C FSM */

/* Master variables */
#if(`$INSTANCE_NAME`_MODE_MASTER_ENABLED)
   extern volatile uint8 `$INSTANCE_NAME`_mstrStatus;   /* Master Status byte  */
   extern volatile uint8 `$INSTANCE_NAME`_mstrControl;  /* Master Control byte */

   /* Transmit buffer variables */
   extern volatile uint8 * `$INSTANCE_NAME`_mstrRdBufPtr;   /* Pointer to Master Read buffer */
   extern volatile uint8   `$INSTANCE_NAME`_mstrRdBufSize;  /* Master Read buffer size       */
   extern volatile uint8   `$INSTANCE_NAME`_mstrRdBufIndex; /* Master Read buffer Index      */

   /* Receive buffer variables */
   extern volatile uint8 * `$INSTANCE_NAME`_mstrWrBufPtr;   /* Pointer to Master Write buffer */
   extern volatile uint8   `$INSTANCE_NAME`_mstrWrBufSize;  /* Master Write buffer size       */
   extern volatile uint8   `$INSTANCE_NAME`_mstrWrBufIndex; /* Master Write buffer Index      */

#endif  /* End (`$INSTANCE_NAME`_MODE_MASTER_ENABLED) */

/* Slave variables */
#if(`$INSTANCE_NAME`_MODE_SLAVE_ENABLED)
   extern volatile uint8 `$INSTANCE_NAME`_slStatus;            /* Slave Status  */

   #if(`$INSTANCE_NAME`_ADDR_DECODE == `$INSTANCE_NAME`_SW_DECODE)
      extern volatile uint8 `$INSTANCE_NAME`_slAddress; /* Software address variable */
   #endif   /* End (`$INSTANCE_NAME`_ADDR_DECODE == `$INSTANCE_NAME`_SW_DECODE) */

   /* Transmit buffer variables */
   extern volatile uint8 * `$INSTANCE_NAME`_slRdBufPtr;     /* Pointer to Transmit buffer  */
   extern volatile uint8   `$INSTANCE_NAME`_slRdBufSize;    /* Slave Transmit buffer size  */
   extern volatile uint8   `$INSTANCE_NAME`_slRdBufIndex;   /* Slave Transmit buffer Index */

   /* Receive buffer variables */
   extern volatile uint8 * `$INSTANCE_NAME`_slWrBufPtr;     /* Pointer to Receive buffer  */
   extern volatile uint8   `$INSTANCE_NAME`_slWrBufSize;    /* Slave Receive buffer size  */
   extern volatile uint8   `$INSTANCE_NAME`_slWrBufIndex;   /* Slave Receive buffer Index */

#endif  /* End (`$INSTANCE_NAME`_MODE_SLAVE_ENABLED) */

#if(`$INSTANCE_NAME`_TIMEOUT_FF_ENABLED)
    extern void `$INSTANCE_NAME`_TimeoutReset(void) `=ReentrantKeil($INSTANCE_NAME . "_TimeoutReset")`;
#endif  /* End (`$INSTANCE_NAME`_TIMEOUT_FF_ENABLED) */


/*******************************************************************************
* Function Name: `$INSTANCE_NAME`_ISR
********************************************************************************
*
* Summary:
*  Handler for I2C interrupt. The Slave and Master operations are handled here.
*
* Parameters:
*  void
*
* Return:
*  void
*
* Reentrant:
*  No
*
*******************************************************************************/
CY_ISR(`$INSTANCE_NAME`_ISR)
{
    #if(`$INSTANCE_NAME`_MODE_SLAVE_ENABLED)
       static uint8  tmp8;    /* Making these static so not wasting time allocating */
    #endif  /* End (`$INSTANCE_NAME`_MODE_SLAVE_ENABLED) */

    static uint8  tmpCsr;

    #if(`$INSTANCE_NAME`_TIMEOUT_FF_ENABLED)
        if(0u != `$INSTANCE_NAME`_TimeoutGetStatus())
        {
            `$INSTANCE_NAME`_TimeoutReset();
            `$INSTANCE_NAME`_state = `$INSTANCE_NAME`_SM_EXIT_IDLE;
            /* `$INSTANCE_NAME`_CSR_REG should be cleared after reset */
        }
    #endif  /* End (`$INSTANCE_NAME`_TIMEOUT_FF_ENABLED) */


    tmpCsr = `$INSTANCE_NAME`_CSR_REG;      /* Make copy as interrupts clear */

    #if(`$INSTANCE_NAME`_MODE_MULTI_MASTER_SLAVE_ENABLED)
        if(`$INSTANCE_NAME`_CHECK_START_GEN(`$INSTANCE_NAME`_MCSR_REG))
        {
            `$INSTANCE_NAME`_CLEAR_START_GEN;

            /* Set READ complete, but was aborted */
            `$INSTANCE_NAME`_mstrStatus |= (`$INSTANCE_NAME`_MSTAT_ERR_XFER |
                                            `$INSTANCE_NAME`_GET_MSTAT_CMPLT);

            `$INSTANCE_NAME`_state = `$INSTANCE_NAME`_SM_SLAVE; /* The slave was addressed */
        }
    #endif  /* End (`$INSTANCE_NAME`_MODE_MULTI_MASTER_SLAVE_ENABLED) */


    #if(`$INSTANCE_NAME`_MODE_MULTI_MASTER_ENABLED)
        if(`$INSTANCE_NAME`_CHECK_LOST_ARB(tmpCsr))
        {
            /* Set errors */
            `$INSTANCE_NAME`_mstrStatus |= (`$INSTANCE_NAME`_MSTAT_ERR_XFER     |
                                            `$INSTANCE_NAME`_MSTAT_ERR_ARB_LOST |
                                            `$INSTANCE_NAME`_GET_MSTAT_CMPLT);

            
            `$INSTANCE_NAME`_DISABLE_INT_ON_STOP;   /* Interrupt on Stop is enabled by write */
            
            #if(`$INSTANCE_NAME`_MODE_MULTI_MASTER_SLAVE_ENABLED)
                if(0u != (tmpCsr & `$INSTANCE_NAME`_CSR_ADDRESS))
                {
                    `$INSTANCE_NAME`_state = `$INSTANCE_NAME`_SM_SLAVE; /* The slave was addressed */
                }
                else
                {
                    `$INSTANCE_NAME`_READY_TO_READ;         /* Release the bus */

                    `$INSTANCE_NAME`_state = `$INSTANCE_NAME`_SM_EXIT_IDLE;
                }
            #else
                `$INSTANCE_NAME`_READY_TO_READ;         /* Release the bus */

                `$INSTANCE_NAME`_state = `$INSTANCE_NAME`_SM_EXIT_IDLE;

            #endif  /* End (`$INSTANCE_NAME`_MODE_MULTI_MASTER_SLAVE_ENABLED) */
        }
    #endif  /* End (`$INSTANCE_NAME`_MODE_MULTI_MASTER_ENABLED) */


    /* Check for Master operation mode */
    if(0u != (`$INSTANCE_NAME`_SM_MASTER & `$INSTANCE_NAME`_state))
    {
        #if(`$INSTANCE_NAME`_MODE_MASTER_ENABLED)
            if(`$INSTANCE_NAME`_CHECK_BYTE_COMPLETE(tmpCsr))
            {
                tmpCsr &= ~`$INSTANCE_NAME`_CSR_STOP_STATUS;    /* Clear STOP bit history after address phase */

                switch (`$INSTANCE_NAME`_state)
                {
                    case `$INSTANCE_NAME`_SM_MSTR_WR_ADDR:  /* After address is sent, WRITE data */
                    case `$INSTANCE_NAME`_SM_MSTR_RD_ADDR:  /* After address is sent, READ  data */

                        if(`$INSTANCE_NAME`_CHECK_ADDR_ACK(tmpCsr))
                        {
                            /* Setup for transmit or receive of data */
                            if(`$INSTANCE_NAME`_state == `$INSTANCE_NAME`_SM_MSTR_WR_ADDR)   /* TRANSMIT data */
                            {
                                if(`$INSTANCE_NAME`_mstrWrBufSize > 0u)    /* Check if at least one byte is transfered */
                                {
                                    `$INSTANCE_NAME`_DATA_REG = `$INSTANCE_NAME`_mstrWrBufPtr[0u];  /* Load first data byte */
                                    `$INSTANCE_NAME`_TRANSMIT_DATA;         /* Transmit data */
                                    `$INSTANCE_NAME`_mstrWrBufIndex = 1u;   /* Set index to 2nd location */

                                    /* Set transmit state until done */
                                    `$INSTANCE_NAME`_state = `$INSTANCE_NAME`_SM_MSTR_WR_DATA;
                                }
                                else   /* No data to tranfer */
                                {
                                    #if(CY_PSOC5A)
                                        /* Do not handles 0 bytes transfer - Not HALT is allowed */
                                        `$INSTANCE_NAME`_GENERATE_STOP;

                                        `$INSTANCE_NAME`_mstrStatus |= `$INSTANCE_NAME`_MSTAT_WR_CMPLT;
                                        `$INSTANCE_NAME`_state       = `$INSTANCE_NAME`_SM_IDLE;

                                    #else  /* The PSoC3A and PSOC5LP handles 0 bytes transfer */
                                        if(`$INSTANCE_NAME`_CHECK_NO_STOP(`$INSTANCE_NAME`_mstrControl))
                                        {
                                            /* Set WRITE complete and Master HALTED */
                                            `$INSTANCE_NAME`_mstrStatus |= (`$INSTANCE_NAME`_MSTAT_XFER_HALT |
                                                                            `$INSTANCE_NAME`_MSTAT_WR_CMPLT);

                                            `$INSTANCE_NAME`_state       = `$INSTANCE_NAME`_SM_MSTR_HALT;

                                            `$INSTANCE_NAME`_DisableInt();
                                        }
                                        else  /* Do normal STOP */
                                        {
                                            `$INSTANCE_NAME`_ENABLE_INT_ON_STOP;
                                            `$INSTANCE_NAME`_GENERATE_STOP;
                                        }

                                    #endif  /* End (CY_PSOC5A) */
                                }
                            }
                            else  /* Master Receive data */
                            {
                                `$INSTANCE_NAME`_READY_TO_READ; /* Ready to READ data */

                                `$INSTANCE_NAME`_state  = `$INSTANCE_NAME`_SM_MSTR_RD_DATA; /* Set FSM to READ data */
                            }
                        }
                        else if(`$INSTANCE_NAME`_CHECK_ADDR_NAK(tmpCsr))
                        {
                            if(`$INSTANCE_NAME`_CHECK_NO_STOP(`$INSTANCE_NAME`_mstrControl))
                            {
                                /* Set Address NAK Error and Master HALTED */
                                `$INSTANCE_NAME`_mstrStatus |= (`$INSTANCE_NAME`_MSTAT_ERR_XFER     |
                                                                `$INSTANCE_NAME`_MSTAT_ERR_ADDR_NAK |
                                                                `$INSTANCE_NAME`_MSTAT_XFER_HALT    |
                                                                `$INSTANCE_NAME`_GET_MSTAT_CMPLT);

                                `$INSTANCE_NAME`_state  = `$INSTANCE_NAME`_SM_MSTR_HALT;    /* Set FSM to HALT: expect RESTART */
                                `$INSTANCE_NAME`_DisableInt();
                            }
                            else  /* Do normal Stop */
                            {
                                `$INSTANCE_NAME`_ENABLE_INT_ON_STOP;    /* Enable interrupt on STOP, to catch it */
                                `$INSTANCE_NAME`_GENERATE_STOP;

                                /* Set Address NAK and ERR transfer */
                                `$INSTANCE_NAME`_mstrStatus |= (`$INSTANCE_NAME`_MSTAT_ERR_XFER |
                                                                `$INSTANCE_NAME`_MSTAT_ERR_ADDR_NAK);
                            }
                        }
                        else
                        {
                            ;/* Address phase is not set for some reason: error */
                        }
                        break;

                    case `$INSTANCE_NAME`_SM_MSTR_WR_DATA:

                        if(`$INSTANCE_NAME`_CHECK_DATA_ACK(tmpCsr))
                        {
                            /* Check if end buffer */
                            if(`$INSTANCE_NAME`_mstrWrBufIndex  < `$INSTANCE_NAME`_mstrWrBufSize)
                            {
                                `$INSTANCE_NAME`_DATA_REG = `$INSTANCE_NAME`_mstrWrBufPtr[`$INSTANCE_NAME`_mstrWrBufIndex];
                                `$INSTANCE_NAME`_TRANSMIT_DATA;     /* Transmit byte */
                                `$INSTANCE_NAME`_mstrWrBufIndex++;
                            }
                            else   /* Last byte was transmitted, send STOP */
                            {
                                if(`$INSTANCE_NAME`_CHECK_NO_STOP(`$INSTANCE_NAME`_mstrControl))
                                {
                                    `$INSTANCE_NAME`_state  = `$INSTANCE_NAME`_SM_MSTR_HALT;    /* Set FSM to HALT: expect RESTART */

                                    /* Set WRITE complete and Master HALTED */
                                    `$INSTANCE_NAME`_mstrStatus |= (`$INSTANCE_NAME`_MSTAT_XFER_HALT |
                                                                    `$INSTANCE_NAME`_MSTAT_WR_CMPLT);

                                    `$INSTANCE_NAME`_DisableInt();
                                }
                                else  /* Do normal STOP */
                                {
                                    `$INSTANCE_NAME`_Workaround();          /* Workaround for CDT 78083 */
                                    `$INSTANCE_NAME`_ENABLE_INT_ON_STOP;    /* Enable interrupt on STOP, to catch it */
                                    `$INSTANCE_NAME`_GENERATE_STOP;
                                }
                            }
                        }
                        else /* If last byte NAKed: send STOP or RESTART*/
                        {
                            if(`$INSTANCE_NAME`_CHECK_NO_STOP(`$INSTANCE_NAME`_mstrControl))
                            {
                                `$INSTANCE_NAME`_state  = `$INSTANCE_NAME`_SM_MSTR_HALT;    /* Set FSM to HALT: expect RESTART */

                                /* Set WRITE complete, SHORT transfer and Master HALTED */
                                `$INSTANCE_NAME`_mstrStatus |= (`$INSTANCE_NAME`_MSTAT_ERR_XFER       |
                                                                `$INSTANCE_NAME`_MSTAT_ERR_SHORT_XFER |
                                                                `$INSTANCE_NAME`_MSTAT_XFER_HALT      |
                                                                `$INSTANCE_NAME`_MSTAT_WR_CMPLT);

                                `$INSTANCE_NAME`_DisableInt();
                            }
                            else  /* Do normal STOP */
                            {
                                `$INSTANCE_NAME`_ENABLE_INT_ON_STOP;    /* Enable interrupt on STOP, to catch it */
                                `$INSTANCE_NAME`_GENERATE_STOP;

                                /* Set SHORT and ERR transfer */
                                `$INSTANCE_NAME`_mstrStatus |= (`$INSTANCE_NAME`_MSTAT_ERR_SHORT_XFER |
                                                                `$INSTANCE_NAME`_MSTAT_ERR_XFER);
                            }
                        }
                        break;

                    case `$INSTANCE_NAME`_SM_MSTR_RD_DATA:

                        `$INSTANCE_NAME`_mstrRdBufPtr[`$INSTANCE_NAME`_mstrRdBufIndex] = `$INSTANCE_NAME`_DATA_REG;
                        `$INSTANCE_NAME`_mstrRdBufIndex++;

                        /* Check if end of buffer */
                        if(`$INSTANCE_NAME`_mstrRdBufIndex < `$INSTANCE_NAME`_mstrRdBufSize)
                        {
                            `$INSTANCE_NAME`_ACK_AND_RECEIVE;       /* ACK and receive byte */
                        }
                        else   /* End of data, generate a STOP */
                        {
                            if(`$INSTANCE_NAME`_CHECK_NO_STOP(`$INSTANCE_NAME`_mstrControl))
                            {
                                `$INSTANCE_NAME`_state = `$INSTANCE_NAME`_SM_MSTR_HALT; /* Set FSM to HALT: expect RESTART */

                                /* Set READ complete and Master HALTED */
                                `$INSTANCE_NAME`_mstrStatus |= (`$INSTANCE_NAME`_MSTAT_XFER_HALT |
                                                                `$INSTANCE_NAME`_MSTAT_RD_CMPLT);
                            }
                            else   /* Do normal STOP */
                            {
                                `$INSTANCE_NAME`_ENABLE_INT_ON_STOP;
                                `$INSTANCE_NAME`_NAK_AND_RECEIVE;       /* NACK and TRY to generate STOP */
                            }
                        }
                        break;

                    default: /* This is an invalid state and should not occur */

                        CYASSERT(0);
                        break;
                }
            }


            if(`$INSTANCE_NAME`_CHECK_STOP_STS(tmpCsr))
            {
                `$INSTANCE_NAME`_mstrStatus |= `$INSTANCE_NAME`_GET_MSTAT_CMPLT;

                `$INSTANCE_NAME`_DISABLE_INT_ON_STOP;
                `$INSTANCE_NAME`_state = `$INSTANCE_NAME`_SM_IDLE;
            }
        #endif  /* End (`$INSTANCE_NAME`_MODE_MASTER_ENABLED) */
    }
    else if(0u != (`$INSTANCE_NAME`_SM_SLAVE & `$INSTANCE_NAME`_state))
    {
        #if(`$INSTANCE_NAME`_MODE_SLAVE_ENABLED)
            if((`$INSTANCE_NAME`_CHECK_STOP_STS(tmpCsr)) || /* Stop || Restart */
               (`$INSTANCE_NAME`_CHECK_BYTE_COMPLETE(tmpCsr) && `$INSTANCE_NAME`_CHECK_ADDRESS_STS(tmpCsr)))
            {
                /* Catch end of master write transcation: use interrupt on Stop */
                if(`$INSTANCE_NAME`_SM_SL_WR_DATA == `$INSTANCE_NAME`_state)
                {
                    `$INSTANCE_NAME`_DISABLE_INT_ON_STOP;

                    `$INSTANCE_NAME`_slStatus &= ~`$INSTANCE_NAME`_SSTAT_WR_BUSY;
                    `$INSTANCE_NAME`_slStatus |=  `$INSTANCE_NAME`_SSTAT_WR_CMPLT;

                    `$INSTANCE_NAME`_state = `$INSTANCE_NAME`_SM_IDLE;
                }
            }

            if(`$INSTANCE_NAME`_CHECK_BYTE_COMPLETE(tmpCsr))
            {
                /* The address only issued after Start or ReStart: so check address to catch this events
                    FF : sets Addr phase with byte_complete interrupt trigger.
                    UDB: sets Addr phase immediately after Start or ReStart. */
                if(0u != (tmpCsr & `$INSTANCE_NAME`_CSR_ADDRESS))
                {
                    /* Clear STOP bit history because it sets on every Stop condition detected on the bus */
                    tmpCsr &= ~`$INSTANCE_NAME`_CSR_STOP_STATUS;

                    /* Check for software address detection */
                    #if(`$INSTANCE_NAME`_ADDR_DECODE == `$INSTANCE_NAME`_SW_DECODE)
                        tmp8 = `$INSTANCE_NAME`_GET_SLAVE_ADDR(`$INSTANCE_NAME`_DATA_REG);

                        if(tmp8 != `$INSTANCE_NAME`_slAddress)   /* Check for address match */
                        {
                            /*     Place code to compare for additional address here    */
                            /* `#START `$INSTANCE_NAME`_SW_ADDR_COMPARE_interruptStart` */

                            /* `#END` */

                            tmpCsr = 0u;    /* Clear this to exclude address check */
                            `$INSTANCE_NAME`_NAK_AND_RECEIVE;   /* NACK address */

                            /* Place code to end of condition for NACK generation here */
                            /* `#START `$INSTANCE_NAME`_SW_ADDR_COMPARE_interruptEnd`  */

                            /* `#END` */
                        }
                    #endif  /* End (`$INSTANCE_NAME`_ADDR_DECODE == `$INSTANCE_NAME`_SW_DECODE) */

                    /* Check for read or write command */
                    if(0u != tmpCsr)
                    {
                        if(0u != (`$INSTANCE_NAME`_DATA_REG & `$INSTANCE_NAME`_READ_FLAG))
                        {
                            /* Place code to prepare read buffer here                  */
                            /* `#START `$INSTANCE_NAME`_SW_PREPARE_READ_BUF_interrupt` */

                            /* `#END` */

                            /* Prepare next opeation to read, get data and place in data register */
                            if(`$INSTANCE_NAME`_slRdBufIndex < `$INSTANCE_NAME`_slRdBufSize)
                            {
                                /* Load first data byte from array */
                                `$INSTANCE_NAME`_DATA_REG = `$INSTANCE_NAME`_slRdBufPtr[`$INSTANCE_NAME`_slRdBufIndex];
                                `$INSTANCE_NAME`_ACK_AND_TRANSMIT;
                                `$INSTANCE_NAME`_slRdBufIndex++;

                                `$INSTANCE_NAME`_slStatus |= `$INSTANCE_NAME`_SSTAT_RD_BUSY;
                            }
                            else    /* Overflow: provide 0xFF on the bus */
                            {
                                `$INSTANCE_NAME`_DATA_REG = `$INSTANCE_NAME`_OVERFLOW_RETURN;
                                `$INSTANCE_NAME`_ACK_AND_TRANSMIT;

                                `$INSTANCE_NAME`_slStatus  |= (`$INSTANCE_NAME`_SSTAT_RD_BUSY |
                                                               `$INSTANCE_NAME`_SSTAT_RD_ERR_OVFL);
                            }

                            `$INSTANCE_NAME`_state = `$INSTANCE_NAME`_SM_SL_RD_DATA;
                        }
                        else  /* Write transaction: receive 1st byte */
                        {
                            `$INSTANCE_NAME`_ACK_AND_RECEIVE;
                            `$INSTANCE_NAME`_state = `$INSTANCE_NAME`_SM_SL_WR_DATA;

                            `$INSTANCE_NAME`_slStatus |= `$INSTANCE_NAME`_SSTAT_WR_BUSY;
                            `$INSTANCE_NAME`_ENABLE_INT_ON_STOP;
                        }
                    }
                }
                /* Data states */
                else
                {
                    /* Data write from Master to Slave */
                    if(`$INSTANCE_NAME`_state == `$INSTANCE_NAME`_SM_SL_WR_DATA)
                    {
                        if(`$INSTANCE_NAME`_slWrBufIndex < `$INSTANCE_NAME`_slWrBufSize)
                        {
                            tmp8 = `$INSTANCE_NAME`_DATA_REG;
                            `$INSTANCE_NAME`_ACK_AND_RECEIVE;
                            `$INSTANCE_NAME`_slWrBufPtr[`$INSTANCE_NAME`_slWrBufIndex++] = tmp8;
                        }
                        else  /* End of array: complete write, send NACK */
                        {
                            `$INSTANCE_NAME`_NAK_AND_RECEIVE;

                            `$INSTANCE_NAME`_slStatus |= `$INSTANCE_NAME`_SSTAT_WR_ERR_OVFL;
                        }
                    }
                    /* Data Read from Slave to Master */
                    else if(`$INSTANCE_NAME`_state == `$INSTANCE_NAME`_SM_SL_RD_DATA)
                    {
                        if(`$INSTANCE_NAME`_CHECK_DATA_ACK(tmpCsr))
                        {
                            if(`$INSTANCE_NAME`_slRdBufIndex < `$INSTANCE_NAME`_slRdBufSize)
                            {
                                 /* Get data from array */
                                `$INSTANCE_NAME`_DATA_REG = `$INSTANCE_NAME`_slRdBufPtr[`$INSTANCE_NAME`_slRdBufIndex];
                                `$INSTANCE_NAME`_TRANSMIT_DATA;
                                `$INSTANCE_NAME`_slRdBufIndex++;
                            }
                            else   /* Overflow: provide 0xFF on the bus */
                            {
                                `$INSTANCE_NAME`_DATA_REG = `$INSTANCE_NAME`_OVERFLOW_RETURN;
                                `$INSTANCE_NAME`_TRANSMIT_DATA;

                                `$INSTANCE_NAME`_slStatus |= `$INSTANCE_NAME`_SSTAT_RD_ERR_OVFL;
                            }
                        }
                        else  /* Last byte was NACKed: read complete */
                        {
                            /* Only NACK appears on the bus */
                            `$INSTANCE_NAME`_DATA_REG = `$INSTANCE_NAME`_OVERFLOW_RETURN;
                            `$INSTANCE_NAME`_NAK_AND_TRANSMIT;

                            `$INSTANCE_NAME`_slStatus &= ~`$INSTANCE_NAME`_SSTAT_RD_BUSY;
                            `$INSTANCE_NAME`_slStatus |=  `$INSTANCE_NAME`_SSTAT_RD_CMPLT;

                            `$INSTANCE_NAME`_state = `$INSTANCE_NAME`_SM_IDLE;
                        }
                    }
                    else
                    {
                        /* Get there is byte_complete and wrong FSM is set */
                        `$INSTANCE_NAME`_DisableInt();
                        `$INSTANCE_NAME`_ClearPendingInt();
                    }
                }
            }
        #endif  /* End (`$INSTANCE_NAME`_MODE_SLAVE_ENABLED) */
    }
    else
    {
        /* The FSM skip master and slave processing: return to IDLE */
        `$INSTANCE_NAME`_state = `$INSTANCE_NAME`_SM_IDLE;
    }
}


#if((`$INSTANCE_NAME`_FF_IMPLEMENTED) && (`$INSTANCE_NAME`_ENABLE_WAKEUP))
    extern volatile uint8 `$INSTANCE_NAME`_wakeupSource;

    /*******************************************************************************
    * Function Name: `$INSTANCE_NAME`_WAKEUP_ISR
    ********************************************************************************
    *
    * Summary:
    *  Empty interrupt handler to trigger after wakeup.
    *
    * Parameters:
    *  void
    *
    * Return:
    *  void
    *
    *******************************************************************************/
    CY_ISR(`$INSTANCE_NAME`_WAKEUP_ISR)
    {
        `$INSTANCE_NAME`_wakeupSource = 1u;  /* I2C was wakeup source */
    }
#endif  /* End ((`$INSTANCE_NAME`_FF_IMPLEMENTED) && (`$INSTANCE_NAME`_ENABLE_WAKEUP))*/


/* [] END OF FILE */
