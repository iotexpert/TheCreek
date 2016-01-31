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

volatile uint8 `$INSTANCE_NAME`_state = `$INSTANCE_NAME`_SM_IDLE; /* Current state of I2C state machine */

/* Master variables */
#if (0u != (`$INSTANCE_NAME`_MODE & `$INSTANCE_NAME`_MODE_MASTER))
    volatile uint8 `$INSTANCE_NAME`_mstrStatus;         /* Master Status byte */
    volatile uint8 `$INSTANCE_NAME`_mstrControl;        /* Master Control byte */
    
    /* Transmit buffer variables */
    volatile uint8 * `$INSTANCE_NAME`_mstrRdBufPtr;     /* Pointer to Master Tx/Rx buffer */
    volatile uint8   `$INSTANCE_NAME`_mstrRdBufSize;    /* Master buffer size */
    volatile uint8   `$INSTANCE_NAME`_mstrRdBufIndex;   /* Master buffer Index */
    
    /* Receive buffer variables */
    volatile uint8 * `$INSTANCE_NAME`_mstrWrBufPtr;     /* Pointer to Master Tx/Rx buffer */
    volatile uint8   `$INSTANCE_NAME`_mstrWrBufSize;    /* Master buffer size  */
    volatile uint8   `$INSTANCE_NAME`_mstrWrBufIndex;   /* Master buffer Index */

#endif  /* End (0u != (`$INSTANCE_NAME`_MODE & `$INSTANCE_NAME`_MODE_MASTER)) */

/* Slave variables */
#if (0u != (`$INSTANCE_NAME`_MODE & `$INSTANCE_NAME`_MODE_SLAVE))
    volatile uint8 `$INSTANCE_NAME`_slStatus;             /* Slave Status byte */

    #if (`$INSTANCE_NAME`_ADDR_DECODE == `$INSTANCE_NAME`_SW_DECODE)
        volatile uint8 `$INSTANCE_NAME`_slAddress;        /* Software address variable */
    #endif  /* End (`$INSTANCE_NAME`_ADDR_DECODE == `$INSTANCE_NAME`_SW_DECODE) */
    
    /* Transmit buffer variables */
    volatile uint8 * `$INSTANCE_NAME`_slRdBufPtr;     /* Pointer to Transmit buffer */
    volatile uint8   `$INSTANCE_NAME`_slRdBufSize;    /* Slave Transmit buffer size */
    volatile uint8   `$INSTANCE_NAME`_slRdBufIndex;   /* Slave Transmit buffer Index */
    
    /* Receive buffer variables */
    volatile uint8 * `$INSTANCE_NAME`_slWrBufPtr;     /* Pointer to Receive buffer */
    volatile uint8   `$INSTANCE_NAME`_slWrBufSize;    /* Slave Receive buffer size */
    volatile uint8   `$INSTANCE_NAME`_slWrBufIndex;   /* Slave Receive buffer Index */

#endif  /* End (0u != (`$INSTANCE_NAME`_MODE & `$INSTANCE_NAME`_MODE_SLAVE)) */


/*******************************************************************************
* Function Name: `$INSTANCE_NAME`_ISR
********************************************************************************
*
* Summary:
*  Handle Interrupt Service Routine.  
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
    #if (0u != (`$INSTANCE_NAME`_MODE & `$INSTANCE_NAME`_MODE_SLAVE))
       static uint8  tmp8;    /* Making these static so not wasting time allocating */
    #endif  /* End (0u != (`$INSTANCE_NAME`_MODE & `$INSTANCE_NAME`_MODE_SLAVE)) */
    
    static uint8  tmpCsr;  /* on the stack each time and no one else can see them */
    
    /* Entry from interrupt */
    /* In hardware address compare mode, we can assume we only get interrupted when */
    /* a valid address is recognized. In software address compare mode, we have to  */
    /* check every address after a start condition.                                 */
    
    tmpCsr = `$INSTANCE_NAME`_CSR_REG;          /* Make temp copy so that we can check */
                                                /* for stop condition after we are done */
    
    /* Check if Start Condition was generated */
    #if (`$INSTANCE_NAME`_MODE == `$INSTANCE_NAME`_MODE_MULTI_MASTER_SLAVE)
        if (`$INSTANCE_NAME`_CHECK_START_GEN(`$INSTANCE_NAME`_MCSR_REG))
        {
            /* Clear Start Gen bit */
            `$INSTANCE_NAME`_CLEAR_START_GEN;
            
            /* Check State for READ one: SM_MSTR_RD_ADDR or SM_MSTR_RD_DATA */
            if (0u != (`$INSTANCE_NAME`_state & `$INSTANCE_NAME`_SM_MSTR_RD))
            {
                /* Set READ complete, but was aborted */
                `$INSTANCE_NAME`_mstrStatus |= (`$INSTANCE_NAME`_MSTAT_RD_CMPLT | `$INSTANCE_NAME`_MSTAT_ERR_XFER);
            }
            else /* All other: should be only write states */
            {
                /* Set WRITE complete, but was aborted */
                `$INSTANCE_NAME`_mstrStatus |= (`$INSTANCE_NAME`_MSTAT_WR_CMPLT | `$INSTANCE_NAME`_MSTAT_ERR_XFER);
            }
            
            /* Reset State Machine to IDLE to enable the Slave */
            `$INSTANCE_NAME`_state = `$INSTANCE_NAME`_SM_IDLE;
        }
    #endif  /* End (`$INSTANCE_NAME`_MODE == `$INSTANCE_NAME`_MULTI_MASTER_ENABLE) */
    
    #if (0u != (`$INSTANCE_NAME`_MODE & `$INSTANCE_NAME`_MODE_MULTI_MASTER_ENABLE))
        /* Check for lost of arbitration  */
        if (`$INSTANCE_NAME`_CHECK_LOST_ARB(tmpCsr))
        {
            /* MultiMaster-Slave */
            #if (`$INSTANCE_NAME`_MODE == `$INSTANCE_NAME`_MODE_MULTI_MASTER_SLAVE)
                /* Check on which state of transaction lost ARBITRAGE:
                  Address    - if Address and pass control to Slave
                  No Address - release the bus */
                if (0u == (tmpCsr & `$INSTANCE_NAME`_CSR_ADDRESS))
                {
            #endif  /* End (`$INSTANCE_NAME`_MODE == `$INSTANCE_NAME`_MULTI_MASTER_ENABLE) */
            
                    /* Lost ARBITRAGE:
                       Data - reset state machine to IDLE, Disable Slave enable event */
                    
                    /* Disable interrupt on STOP in case that it was READ */
                    `$INSTANCE_NAME`_DISABLE_INT_ON_STOP;
                    
                    /* Clear CSR to release the bus, if MultiMaster */
                    `$INSTANCE_NAME`_READY_TO_READ;
            
            #if (`$INSTANCE_NAME`_MODE == `$INSTANCE_NAME`_MODE_MULTI_MASTER_SLAVE)
                    /* Clean up the Slave enable events: Byte Complete and Stop */
                    tmpCsr &= ~ (`$INSTANCE_NAME`_CSR_BYTE_COMPLETE | `$INSTANCE_NAME`_CSR_STOP_STATUS);
                }
            #endif  /* End (`$INSTANCE_NAME`_MODE == `$INSTANCE_NAME`_MULTI_MASTER_ENABLE) */
            
            /* Check State for READ one: SM_MSTR_RD_ADDR or SM_MSTR_RD_DATA */
            if (0u != (`$INSTANCE_NAME`_state & `$INSTANCE_NAME`_SM_MSTR_RD))
            {
                /* Set READ complete */
                `$INSTANCE_NAME`_mstrStatus |= `$INSTANCE_NAME`_MSTAT_RD_CMPLT;
            }
            else /* All other: should be only write states */
            {
                /* Set WRITE complete */
                `$INSTANCE_NAME`_mstrStatus |= `$INSTANCE_NAME`_MSTAT_WR_CMPLT;
            }
            
            /* Set status error transfer and arbitration lost */
            `$INSTANCE_NAME`_mstrStatus |= (`$INSTANCE_NAME`_MSTAT_ERR_ARB_LOST | 
                                            `$INSTANCE_NAME`_MSTAT_ERR_XFER);
            
            /* Reset State Machine to IDLE */
            `$INSTANCE_NAME`_state = `$INSTANCE_NAME`_SM_IDLE;
        }
    #endif  /* End (0u != (`$INSTANCE_NAME`_MODE & `$INSTANCE_NAME`_MODE_MULTI_MASTER_ENABLE)) */
     
    /* Check for Master operation mode */
    if (0u != (`$INSTANCE_NAME`_state & `$INSTANCE_NAME`_SM_MASTER))
    {
        #if (0u != (`$INSTANCE_NAME`_MODE & `$INSTANCE_NAME`_MODE_MASTER))
            
            /* Enter Master state machine */
            if (`$INSTANCE_NAME`_CHECK_BYTE_COMPLETE(tmpCsr))
            {
                /* Clear external or previous stop event */
                tmpCsr &= ~`$INSTANCE_NAME`_CSR_STOP_STATUS;  /* Clear STOP bit */
                
                switch (`$INSTANCE_NAME`_state)
                {
                    case `$INSTANCE_NAME`_SM_MSTR_WR_ADDR:    /* After address is sent, WRITE some data */
                    case `$INSTANCE_NAME`_SM_MSTR_RD_ADDR:    /* After address is sent, READ some data */
                    
                        /* Check for Slave address ACK */
                        if (`$INSTANCE_NAME`_CHECK_ADDR_ACK(tmpCsr))  /* Check ACK */
                        {
                            /* Setup for transmit or receive of data */
                            if (`$INSTANCE_NAME`_state == `$INSTANCE_NAME`_SM_MSTR_WR_ADDR)   /* TRANSMIT data */
                            {
                                if (`$INSTANCE_NAME`_mstrWrBufSize > 0u)    /* Check if at least one byte is transfered */
                                {
                                    /* Load first data byte */
                                    `$INSTANCE_NAME`_DATA_REG = `$INSTANCE_NAME`_mstrWrBufPtr[0u];
                                    `$INSTANCE_NAME`_TRANSMIT_DATA;            /* Transmit data */
                                    `$INSTANCE_NAME`_mstrWrBufIndex = 1u;      /* Set index to 2nd location */
                                    
                                    /* Set transmit state until done */
                                    `$INSTANCE_NAME`_state = `$INSTANCE_NAME`_SM_MSTR_WR_DATA; 
                                }
                                else   /* No data to tranfer */
                                {
                                    /* Handles 0 bytes transfer - Not HALT is allowed */
                                    #if (CY_PSOC5A)
                                        `$INSTANCE_NAME`_GENERATE_STOP;     /* Generate STOP */
                                        
                                        /* Set WRITE complete */
                                        `$INSTANCE_NAME`_mstrStatus |= `$INSTANCE_NAME`_MSTAT_WR_CMPLT;
                                        
                                        /* Reset State Machine to IDLE */
                                        `$INSTANCE_NAME`_state  = `$INSTANCE_NAME`_SM_IDLE;
                                        
                                    #else  /* The PSoC3 and PSoC5LP only handles this well */
                                        if (`$INSTANCE_NAME`_CHECK_NO_STOP(`$INSTANCE_NAME`_mstrControl))
                                        {
                                            /* Reset State Machine to HALT, expect RESTART */
                                            `$INSTANCE_NAME`_state  = `$INSTANCE_NAME`_SM_MSTR_HALT;
                                            
                                            /* Set WRITE complete and Master HALTED */
                                            `$INSTANCE_NAME`_mstrStatus |= (`$INSTANCE_NAME`_MSTAT_WR_CMPLT |
                                                                            `$INSTANCE_NAME`_MSTAT_XFER_HALT);
                                            
                                            `$INSTANCE_NAME`_DisableInt();
                                        }
                                        else  /* Do normal STOP */
                                        {
                                            `$INSTANCE_NAME`_ENABLE_INT_ON_STOP;    /* Enable interrupt on STOP, to catch it */
                                            `$INSTANCE_NAME`_GENERATE_STOP;         /* Generate STOP */
                                        }
                                        
                                    #endif  /* End (CY_PSOC5A) */
                                }
                            }
                            else  /* Master Receive data */
                            {
                                `$INSTANCE_NAME`_READY_TO_READ;     /* Ready to READ data */
                                
                                /* Set state machine to READ data */
                                `$INSTANCE_NAME`_state  = `$INSTANCE_NAME`_SM_MSTR_RD_DATA;
                            }
                        }
                        /* Check for Slave address NAK */
                        else if (`$INSTANCE_NAME`_CHECK_ADDR_NAK(tmpCsr))  /* Check NACK */
                        {
                            if (`$INSTANCE_NAME`_CHECK_NO_STOP(`$INSTANCE_NAME`_mstrControl))
                            {
                                /* Check State for READ one: SM_MSTR_RD_ADDR or SM_MSTR_RD_DATA */
                                if (0u != (`$INSTANCE_NAME`_state & `$INSTANCE_NAME`_SM_MSTR_RD))
                                {
                                    /* Set READ complete */
                                    `$INSTANCE_NAME`_mstrStatus |= `$INSTANCE_NAME`_MSTAT_RD_CMPLT;
                                }
                                else /* All other: should be only write states */
                                {
                                    /* Set WRITE complete */
                                    `$INSTANCE_NAME`_mstrStatus |= `$INSTANCE_NAME`_MSTAT_WR_CMPLT;
                                }
                                
                                /* Set Address NAK Error and Master HALTED */
                                `$INSTANCE_NAME`_mstrStatus |= (`$INSTANCE_NAME`_MSTAT_XFER_HALT |
                                                                `$INSTANCE_NAME`_MSTAT_ERR_ADDR_NAK |
                                                                `$INSTANCE_NAME`_MSTAT_ERR_XFER);
                                                                
                                /* Reset State Machine to HALT, expect RESTART */
                                `$INSTANCE_NAME`_state  = `$INSTANCE_NAME`_SM_MSTR_HALT;
                                `$INSTANCE_NAME`_DisableInt();
                            }
                            else  /* Do normal Stop */
                            {
                                `$INSTANCE_NAME`_ENABLE_INT_ON_STOP;    /* Enable interrupt on STOP, to catch it */
                                `$INSTANCE_NAME`_GENERATE_STOP;         /* Generate STOP */
                                
                                /* Set Address NAK and ERR transfer */
                                `$INSTANCE_NAME`_mstrStatus |= (`$INSTANCE_NAME`_MSTAT_ERR_ADDR_NAK | 
                                                                `$INSTANCE_NAME`_MSTAT_ERR_XFER);
                            }
                        }
                        /* Should never gets here: Address status is NOT set */
                        else
                        {
                            CYASSERT(0);
                        }
                        break;
                        
                    case `$INSTANCE_NAME`_SM_MSTR_WR_DATA:    /* Write data to slave */
                        
                        if (`$INSTANCE_NAME`_CHECK_DATA_ACK(tmpCsr))       /* Check ACK */
                        {
                            /* Check if end buffer */
                            if (`$INSTANCE_NAME`_mstrWrBufIndex  < `$INSTANCE_NAME`_mstrWrBufSize)
                            {
                                 /* Load first data byte  */
                                `$INSTANCE_NAME`_DATA_REG = `$INSTANCE_NAME`_mstrWrBufPtr[`$INSTANCE_NAME`_mstrWrBufIndex];
                                `$INSTANCE_NAME`_TRANSMIT_DATA;     /* Transmit */
                                
                                `$INSTANCE_NAME`_mstrWrBufIndex++;  /* Advance to data location */
                            }
                            else   /* Last byte was transmitted, send STOP */
                            {
                                if (`$INSTANCE_NAME`_CHECK_NO_STOP(`$INSTANCE_NAME`_mstrControl))
                                {
                                    /* Reset State Machine to HALT, expect RESTART */
                                    `$INSTANCE_NAME`_state  = `$INSTANCE_NAME`_SM_MSTR_HALT;
                                    
                                    /* Set WRITE complete and Master HALTED */
                                    `$INSTANCE_NAME`_mstrStatus |= (`$INSTANCE_NAME`_MSTAT_WR_CMPLT |
                                                                    `$INSTANCE_NAME`_MSTAT_XFER_HALT);
                                    
                                    `$INSTANCE_NAME`_DisableInt();
                                }
                                else  /* Do normal STOP */
                                {
                                    `$INSTANCE_NAME`_Workaround();          /* Workaround for CDT 78083 */
                                    `$INSTANCE_NAME`_ENABLE_INT_ON_STOP;    /* Enable interrupt on STOP, to catch it */
                                    `$INSTANCE_NAME`_GENERATE_STOP;         /* Generate STOP */
                                }
                            }
                        }
                        else /* If last byte NAKed, stop transmit and send STOP */
                        {
                            /* Check STOP generation */
                            if (`$INSTANCE_NAME`_CHECK_NO_STOP(`$INSTANCE_NAME`_mstrControl))
                            {
                                /* Reset State Machine to HALT, expect RESTART */
                                `$INSTANCE_NAME`_state  = `$INSTANCE_NAME`_SM_MSTR_HALT;
                                
                                /* Set WRITE complete, SHORT transfer and Master HALTED */
                                `$INSTANCE_NAME`_mstrStatus |= (`$INSTANCE_NAME`_MSTAT_WR_CMPLT |
                                                                `$INSTANCE_NAME`_MSTAT_XFER_HALT |
                                                                `$INSTANCE_NAME`_MSTAT_ERR_SHORT_XFER |
                                                                `$INSTANCE_NAME`_MSTAT_ERR_XFER);
                                
                                `$INSTANCE_NAME`_DisableInt();
                            }
                            else  /* Do normal STOP */
                            {
                                `$INSTANCE_NAME`_ENABLE_INT_ON_STOP;    /* Enable interrupt on STOP, to catch it */
                                `$INSTANCE_NAME`_GENERATE_STOP;         /* Generate STOP */
                                                                
                                /* Set SHORT and ERR transfer */
                                `$INSTANCE_NAME`_mstrStatus |= (`$INSTANCE_NAME`_MSTAT_ERR_SHORT_XFER | 
                                                                `$INSTANCE_NAME`_MSTAT_ERR_XFER);
                            }
                        }
                        break;
                        
                    case `$INSTANCE_NAME`_SM_MSTR_RD_DATA:    /* Data received */
                        
                        `$INSTANCE_NAME`_mstrRdBufPtr[`$INSTANCE_NAME`_mstrRdBufIndex] = `$INSTANCE_NAME`_DATA_REG;
                        `$INSTANCE_NAME`_mstrRdBufIndex++;      /* Inc pointer */
                        /* Check if end of buffer */
                        if (`$INSTANCE_NAME`_mstrRdBufIndex < `$INSTANCE_NAME`_mstrRdBufSize)
                        {
                            `$INSTANCE_NAME`_ACK_AND_RECEIVE;       /* ACK and receive */
                        }
                        else   /* End of data, generate a STOP */
                        {
                            if (`$INSTANCE_NAME`_CHECK_NO_STOP(`$INSTANCE_NAME`_mstrControl)) /* Check STOP generation */
                            {
                                /* Reset State Machine to HALT, expect RESTART */
                                `$INSTANCE_NAME`_state = `$INSTANCE_NAME`_SM_MSTR_HALT;
                                
                                /* Set READ complete and Master HALTED */
                                `$INSTANCE_NAME`_mstrStatus |= (`$INSTANCE_NAME`_MSTAT_RD_CMPLT |
                                                                `$INSTANCE_NAME`_MSTAT_XFER_HALT );
                            }
                            else   /* Do normal STOP */
                            {
                                `$INSTANCE_NAME`_ENABLE_INT_ON_STOP;        /* Enable interrupt on STOP, to catch it */
                                `$INSTANCE_NAME`_NAK_AND_RECEIVE;           /* NACK and TRY to generate STOP */
                            }
                        }
                        break;
                        
                    default: /* This is an invalid state and should not occur */
                        
                        CYASSERT(0);
                        break;
                }
            }
            
            /* Check if STOP was detected */
            if (`$INSTANCE_NAME`_CHECK_STOP_STS(tmpCsr))
            {
                /* Check State for READ one: SM_MSTR_RD_ADDR or SM_MSTR_RD_DATA */
                if (0u != (`$INSTANCE_NAME`_state & `$INSTANCE_NAME`_SM_MSTR_RD))
                {
                    /* Set READ complete */
                    `$INSTANCE_NAME`_mstrStatus |= `$INSTANCE_NAME`_MSTAT_RD_CMPLT;
                }
                else /* All other: should be only write states */
                {
                    /* Set WRITE complete */
                    `$INSTANCE_NAME`_mstrStatus |= `$INSTANCE_NAME`_MSTAT_WR_CMPLT;
                }
                
                /* Catch STOP, disable the interrupt on STOP */
                `$INSTANCE_NAME`_DISABLE_INT_ON_STOP;
                `$INSTANCE_NAME`_state = `$INSTANCE_NAME`_SM_IDLE;  /* Set state to IDLE */
            }
        #endif  /* End (0u != (`$INSTANCE_NAME`_MODE & `$INSTANCE_NAME`_MODE_MASTER)) */
    }
    else    /* Slave */
    {
        #if (0u != (`$INSTANCE_NAME`_MODE & `$INSTANCE_NAME`_MODE_SLAVE))
            /* Check to see if a Start/Address is detected */
            if (0u != (tmpCsr & `$INSTANCE_NAME`_CSR_ADDRESS))
            {
                /* Clears STOP status bit. This status bit sets by ANY of STOP condition detection on the bus */
                tmpCsr &= ~`$INSTANCE_NAME`_CSR_STOP_STATUS;  /* Clear STOP bit */
                
                /* This is a Start or ReStart. Reset the state machine and check for a Read/Write condition */
                
                /* Check for software address detection */
                #if (`$INSTANCE_NAME`_ADDR_DECODE == `$INSTANCE_NAME`_SW_DECODE)
                    tmp8 = ((`$INSTANCE_NAME`_DATA_REG >> `$INSTANCE_NAME`_SLAVE_ADDR_SHIFT) & 
                             `$INSTANCE_NAME`_SLAVE_ADDR_MASK);
                    if (tmp8 == `$INSTANCE_NAME`_slAddress)   /* Check for address match */
                    {
                        /* Check for read or write command */
                        if (0u != (`$INSTANCE_NAME`_DATA_REG & `$INSTANCE_NAME`_READ_FLAG))
                        {
                            /*        Place code to prepare read buffer here           */
                            /* `#START `$INSTANCE_NAME`_SW_PREPARE_READ_BUF_interrupt` */
                            
                            /* `#END` */
                            
                            /* Prepare next opeation to read, get data and place in data register */
                            if (`$INSTANCE_NAME`_slRdBufIndex < `$INSTANCE_NAME`_slRdBufSize)
                            {
                                /* Load first data byte */
                                `$INSTANCE_NAME`_DATA_REG = `$INSTANCE_NAME`_slRdBufPtr[`$INSTANCE_NAME`_slRdBufIndex];
                                `$INSTANCE_NAME`_ACK_AND_TRANSMIT;  /* ACK and transmit */
                                `$INSTANCE_NAME`_slRdBufIndex++;    /* Advance to data location */
                                
                                /* Set READ activity */
                                `$INSTANCE_NAME`_slStatus |= `$INSTANCE_NAME`_SSTAT_RD_BUSY;
                            }
                            else    /* Data overflow */
                            {
                                `$INSTANCE_NAME`_DATA_REG = 0xFFu;    /* Out of range, send 0xFF */
                                `$INSTANCE_NAME`_ACK_AND_TRANSMIT;    /* ACK and transmit */
                                
                                /* Set READ activity with OVERFLOW */
                                `$INSTANCE_NAME`_slStatus  |= (`$INSTANCE_NAME`_SSTAT_RD_BUSY | 
                                                               `$INSTANCE_NAME`_SSTAT_RD_ERR_OVFL);
                            }
                            
                            `$INSTANCE_NAME`_state = `$INSTANCE_NAME`_SM_SL_RD_DATA; /* Prepare for Read transaction */
                        }
                        else  /* Start of a Write transaction, ready to write of the first byte */
                        {
                            /* Prepare to write the first byte */
                            `$INSTANCE_NAME`_ACK_AND_RECEIVE;
                            `$INSTANCE_NAME`_state = `$INSTANCE_NAME`_SM_SL_WR_DATA; /* Prepare for Write transaction */
                            
                            /* Set WRITE activity */
                            `$INSTANCE_NAME`_slStatus |= `$INSTANCE_NAME`_SSTAT_WR_BUSY;
                            `$INSTANCE_NAME`_ENABLE_INT_ON_STOP;    /* Enable interrupt on Stop */
                        }
                    }
                    else   /* No address match */
                    {
                        /*     Place code to compare for additional address here    */
                        /* `#START `$INSTANCE_NAME`_SW_ADDR_COMPARE_interruptStart` */
                        
                        /* `#END` */
                        
                            `$INSTANCE_NAME`_NAK_AND_RECEIVE;   /* NAK address */
                        
                        /* Place code to end of condition for NACK generation here */
                        /* `#START `$INSTANCE_NAME`_SW_ADDR_COMPARE_interruptEnd`  */
                        
                        /* `#END` */
                    }
                    
                #else  /* Hardware address detection */
                    /* Check for read or write command */
                    if (0u != (`$INSTANCE_NAME`_DATA_REG & `$INSTANCE_NAME`_READ_FLAG))
                    {
                        /*          Place code to prepare read buffer here         */
                        /* `#START `$INSTANCE_NAME`_HW_PREPARE_READ_BUF_interrupt` */
                         
                        /* `#END` */
                         
                        /* Prepare next opeation to read, get data and place in data register */
                        if (`$INSTANCE_NAME`_slRdBufIndex < `$INSTANCE_NAME`_slRdBufSize)
                        {
                            /* Load first data byte */
                            `$INSTANCE_NAME`_DATA_REG = `$INSTANCE_NAME`_slRdBufPtr[`$INSTANCE_NAME`_slRdBufIndex];
                            `$INSTANCE_NAME`_ACK_AND_TRANSMIT;  /* ACK and transmit */
                            `$INSTANCE_NAME`_slRdBufIndex++;    /* Advance to data location */
                            
                            /* Set READ activity */
                            `$INSTANCE_NAME`_slStatus  |= `$INSTANCE_NAME`_SSTAT_RD_BUSY;
                        }
                        else    /* Data overflow */
                        {
                            `$INSTANCE_NAME`_DATA_REG = 0xFFu;    /* Out of range, send 0xFF  */
                            `$INSTANCE_NAME`_ACK_AND_TRANSMIT;    /* ACK and transmit */
                            
                            /* Set READ activity with OVERFLOW */
                            `$INSTANCE_NAME`_slStatus  |= (`$INSTANCE_NAME`_SSTAT_RD_BUSY |
                                                           `$INSTANCE_NAME`_SSTAT_RD_ERR_OVFL);
                        }
                        
                        `$INSTANCE_NAME`_state = `$INSTANCE_NAME`_SM_SL_RD_DATA;    /* Prepare for Read transaction */
                    }
                    else  /* Start of a Write transaction, ready to write of the first byte */
                    {
                        /* Prepare to write the first byte */
                        `$INSTANCE_NAME`_ACK_AND_RECEIVE;       /* ACK and ready to receive addr */
                        `$INSTANCE_NAME`_state = `$INSTANCE_NAME`_SM_SL_WR_DATA;    /* Prepare for write transaction */
                        
                        /* Set WRITE activity */
                        `$INSTANCE_NAME`_slStatus |= `$INSTANCE_NAME`_SSTAT_WR_BUSY;
                        `$INSTANCE_NAME`_ENABLE_INT_ON_STOP;    /* Enable interrupt on Stop */
                    }
                    
                #endif  /* End (`$INSTANCE_NAME`_ADDR_DECODE == `$INSTANCE_NAME`_SW_DECODE) */
            }
            /* Check for data transfer */
            else if (`$INSTANCE_NAME`_CHECK_BYTE_COMPLETE(tmpCsr))  
            {
                /* Data write from Master to Slave */
                if (`$INSTANCE_NAME`_state == `$INSTANCE_NAME`_SM_SL_WR_DATA)
                {
                    if (`$INSTANCE_NAME`_slWrBufIndex < `$INSTANCE_NAME`_slWrBufSize)       /* Check for valid range */
                    {
                        tmp8 = `$INSTANCE_NAME`_DATA_REG;                        /* Get data, to ACK quickly */
                        `$INSTANCE_NAME`_ACK_AND_RECEIVE;                        /* ACK and ready to receive */
                        `$INSTANCE_NAME`_slWrBufPtr[`$INSTANCE_NAME`_slWrBufIndex] = tmp8; /* Write data to array */
                        `$INSTANCE_NAME`_slWrBufIndex++;                        /* Inc pointer */
                    }
                    else
                    {
                        `$INSTANCE_NAME`_NAK_AND_RECEIVE;       /* NAK cause beyond write area */
                        
                        /* Set OVERFLOW, write completes on Stop */
                        `$INSTANCE_NAME`_slStatus |= `$INSTANCE_NAME`_SSTAT_WR_ERR_OVFL;
                    }
                }
                /* Data Read from Slave to Master */
                else if (`$INSTANCE_NAME`_state == `$INSTANCE_NAME`_SM_SL_RD_DATA)
                {
                    if (`$INSTANCE_NAME`_CHECK_DATA_ACK(tmpCsr))
                    {
                        if (`$INSTANCE_NAME`_slRdBufIndex < `$INSTANCE_NAME`_slRdBufSize) 
                        {
                             /* Get data from array */
                            `$INSTANCE_NAME`_DATA_REG = `$INSTANCE_NAME`_slRdBufPtr[`$INSTANCE_NAME`_slRdBufIndex];
                            `$INSTANCE_NAME`_TRANSMIT_DATA;         /* Send Data */
                            `$INSTANCE_NAME`_slRdBufIndex++;        /* Inc pointer */
                        }
                        else   /* Over flow */
                        {
                            `$INSTANCE_NAME`_DATA_REG = 0xFFu;  /* Send 0xFF at the end of the buffer */
                            `$INSTANCE_NAME`_TRANSMIT_DATA;     /* Send Data */
                            
                            /* Set OVERFLOW */
                            `$INSTANCE_NAME`_slStatus |= `$INSTANCE_NAME`_SSTAT_RD_ERR_OVFL;
                        }
                    }
                    else  /* Last byte NAKed, done */
                    {
                        `$INSTANCE_NAME`_DATA_REG = 0xFFu;  /* End of read transaction */
                        `$INSTANCE_NAME`_NAK_AND_TRANSMIT;  /* Clear transmit bit at the end of read transaction */
                         
                        `$INSTANCE_NAME`_slStatus &= ~`$INSTANCE_NAME`_SSTAT_RD_BUSY;   /* Clear RD_BUSY Flag */
                        `$INSTANCE_NAME`_slStatus |= `$INSTANCE_NAME`_SSTAT_RD_CMPLT;    /* Set RD_CMPLT Flag */
                        
                        `$INSTANCE_NAME`_state = `$INSTANCE_NAME`_SM_IDLE;  /* Return to IDLE state */
                    }
                }
                /* This is an invalid state and should not occur */
                else
                {
                    CYASSERT(0);
                }   /* End Transfer mode */
            }
            /* EMPTY else: there is no Slave enable event. */
            else
            {
                /* The Multi-Master-Slave exist here when arbitrage happen on other than
                   address stage of transaction == No Slave enable event. */
            }
            
            /* Check if STOP was detected */
            if (`$INSTANCE_NAME`_CHECK_STOP_STS(tmpCsr))
            {
                /* The Write transaction only IE on STOP, so Read never gets here */
                /* The WR_BUSY flag will be cleared at the end of "Write-ReStart-Read-Stop" transaction */
                
                `$INSTANCE_NAME`_slStatus &= ~`$INSTANCE_NAME`_SSTAT_WR_BUSY;   /* Clear WR_BUSY Flag */
                `$INSTANCE_NAME`_slStatus |= `$INSTANCE_NAME`_SSTAT_WR_CMPLT;    /* Set WR_CMPT Flag */
                
                `$INSTANCE_NAME`_DISABLE_INT_ON_STOP;               /* Disable interrupt on STOP */
                `$INSTANCE_NAME`_state = `$INSTANCE_NAME`_SM_IDLE;  /* Return to IDLE */
            }
        
        #endif  /* End (0u != (`$INSTANCE_NAME`_MODE & `$INSTANCE_NAME`_MODE_SLAVE)) */
    }
}


/* [] END OF FILE */
