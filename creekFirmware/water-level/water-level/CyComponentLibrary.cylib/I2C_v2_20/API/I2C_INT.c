/*******************************************************************************
* File Name: `$INSTANCE_NAME`_INT.c  
* Version `$CY_MAJOR_VERSION`.`$CY_MINOR_VERSION`
*
* Description:
*  This file provides the source code of Interrupt Service Routine (ISR)
*  for I2C component.
*
*   Note:
*
*******************************************************************************
* Copyright 2008-2011, Cypress Semiconductor Corporation.  All rights reserved.
* You may use this file only in accordance with the license, terms, conditions, 
* disclaimers, and limitations in the end user license agreement accompanying 
* the software package with which this file was provided.
********************************************************************************/

#include "cytypes.h"
#include "`$INSTANCE_NAME`.h"  


/**********************************
*      System variables
**********************************/

volatile uint8 `$INSTANCE_NAME`_State;    /* Current state of I2C state machine */

/* Master variables */
#if (`$INSTANCE_NAME`_MODE & `$INSTANCE_NAME`_MODE_MASTER)
    volatile uint8 `$INSTANCE_NAME`_mstrStatus;           /* Master Status byte */
    volatile uint8 `$INSTANCE_NAME`_mstrControl;          /* Master Control byte */
    
    /* Transmit buffer variables */
    volatile uint8 * `$INSTANCE_NAME`_mstrRdBufPtr;       /* Pointer to Master Tx/Rx buffer */
    volatile uint8   `$INSTANCE_NAME`_mstrRdBufSize;      /* Master buffer size */
    volatile uint8   `$INSTANCE_NAME`_mstrRdBufIndex;     /* Master buffer Index */
    
    /* Receive buffer variables */
    volatile uint8 * `$INSTANCE_NAME`_mstrWrBufPtr;       /* Pointer to Master Tx/Rx buffer */
    volatile uint8   `$INSTANCE_NAME`_mstrWrBufSize;      /* Master buffer size  */
    volatile uint8   `$INSTANCE_NAME`_mstrWrBufIndex;     /* Master buffer Index */

#endif  /* End (`$INSTANCE_NAME`_MODE & `$INSTANCE_NAME`_MODE_MASTER) */

/* Slave variables */
#if (`$INSTANCE_NAME`_MODE & `$INSTANCE_NAME`_MODE_SLAVE)
    volatile uint8 `$INSTANCE_NAME`_slStatus;             /* Slave Status byte */

    #if (`$INSTANCE_NAME`_ADDR_DECODE == `$INSTANCE_NAME`_SW_DECODE)
        volatile uint8 `$INSTANCE_NAME`_slAddress;        /* Software address variable */
    #endif  /* End (`$INSTANCE_NAME`_ADDR_DECODE == `$INSTANCE_NAME`_SW_DECODE) */
    
    /* Transmit buffer variables */
    volatile uint8 * `$INSTANCE_NAME`_slRdBufPtr;         /* Pointer to Transmit buffer */
    volatile uint8   `$INSTANCE_NAME`_slRdBufSize;        /* Slave Transmit buffer size */
    volatile uint8   `$INSTANCE_NAME`_slRdBufIndex;       /* Slave Transmit buffer Index */
    
    /* Receive buffer variables */
    volatile uint8 * `$INSTANCE_NAME`_slWrBufPtr;         /* Pointer to Receive buffer */
    volatile uint8   `$INSTANCE_NAME`_slWrBufSize;        /* Slave Receive buffer size */
    volatile uint8   `$INSTANCE_NAME`_slWrBufIndex;       /* Slave Receive buffer Index */

#endif  /* End (`$INSTANCE_NAME`_MODE & `$INSTANCE_NAME`_MODE_SLAVE) */


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
    #if (`$INSTANCE_NAME`_MODE & `$INSTANCE_NAME`_MODE_SLAVE)
       static uint8  tmp8;    /* Making these static so not wasting time allocating */
    #endif  /* End (`$INSTANCE_NAME`_MODE & `$INSTANCE_NAME`_MODE_SLAVE) */
    
    static uint8  tmpCsr;  /* on the stack each time and no one else can see them */
    
    /* Entry from interrupt */
    /* In hardware address compare mode, we can assume we only get interrupted when */
    /* a valid address is recognized. In software address compare mode, we have to  */
    /* check every address after a start condition.                                 */
    
    tmpCsr = `$INSTANCE_NAME`_CSR_REG;          /* Make temp copy so that we can check */
                                                /* for stop condition after we are done */
    
    #if(`$INSTANCE_NAME`_MODE & `$INSTANCE_NAME`_MODE_MULTI_MASTER_ENABLE)
       
       /* MultiMaster-Slave */
        #if(`$INSTANCE_NAME`_MODE == `$INSTANCE_NAME`_MODE_MULTI_MASTER_SLAVE)
            /* Check if START has been generated */
            if((`$INSTANCE_NAME`_MCSR_REG & `$INSTANCE_NAME`_MCSR_START_GEN) != 0u)
            {
                /* Clear if was not generated */
                `$INSTANCE_NAME`_MCSR_REG &= ~`$INSTANCE_NAME`_MCSR_START_GEN;
            }
        #endif  /* End (`$INSTANCE_NAME`_MODE == `$INSTANCE_NAME`_MODE_MULTI_MASTER_SLAVE) */
                
        /* Check for loss of arbitration  */
        if(`$INSTANCE_NAME`_CHECK_LOST_ARB(tmpCsr))
        {
            /* MultiMaster-Slave */
            #if(`$INSTANCE_NAME`_MODE == `$INSTANCE_NAME`_MODE_MULTI_MASTER_SLAVE)    
            /* Check on which state of transaction lost ARBITRAGE:
              Address    - if Address and pass control to Slave
              No Address - release the bus */
            if((tmpCsr & `$INSTANCE_NAME`_CSR_ADDRESS) == 0u)
            {
                /* Lost ARBITRAGE:
                   On Adress, R/W bit, Data - reset state machine to IDLE
                   On ACK/NACK bit - pass control to SM_MSTR_WAIT_STOP state 
                                     to release the bus */
                if(`$INSTANCE_NAME`_State != `$INSTANCE_NAME`_SM_MSTR_WAIT_STOP)
                {
                    /* Clear ByteComplete to clear Enable Slave event */
                    tmpCsr &= ~`$INSTANCE_NAME`_CSR_BYTE_COMPLETE;
                    
                    /* Clear CSR to release the bus */
                    `$INSTANCE_NAME`_READY_TO_READ;
                    `$INSTANCE_NAME`_State = `$INSTANCE_NAME`_SM_IDLE;  /* Reset State Machine to IDLE */
                }
            }
            else
            {
                /* Let Slave decide the next step, the Stop status will be cleared if set */
                `$INSTANCE_NAME`_State = `$INSTANCE_NAME`_SM_IDLE;  /* Reset State Machine to IDLE */
            }
            
            /* MultiMaster */
            #else
                /* Lost ARBITRAGE:
                   On Adress, R/W bit, Data - reset state machine to IDLE
                   On ACK/NACK bit - pass control to SM_MSTR_WAIT_STOP state 
                                     to release the bus */
                if(`$INSTANCE_NAME`_State != `$INSTANCE_NAME`_SM_MSTR_WAIT_STOP)
                {
                    /* Clear CSR to release the bus, if MultiMaster */
                    `$INSTANCE_NAME`_READY_TO_READ;
                    `$INSTANCE_NAME`_State = `$INSTANCE_NAME`_SM_IDLE;  /* Reset State Machine to IDLE */
                }
            #endif  /* (`$INSTANCE_NAME`_MODE == `$INSTANCE_NAME`_MULTI_MASTER_ENABLE) */
            
            /* Set status error transfer and arbitration lost */
            `$INSTANCE_NAME`_mstrStatus |= (`$INSTANCE_NAME`_MSTAT_ERR_ARB_LOST | `$INSTANCE_NAME`_MSTAT_ERR_XFER);
        }
    #endif  /* End (`$INSTANCE_NAME`_MODE & `$INSTANCE_NAME`_MODE_MULTI_MASTER_ENABLE) */
     
    /* Check for Master operation mode */
    if(`$INSTANCE_NAME`_State & `$INSTANCE_NAME`_SM_MASTER)    /*******  Master *******/
    {
        #if (`$INSTANCE_NAME`_MODE & `$INSTANCE_NAME`_MODE_MASTER)
        /* Enter Master state machine */
        
            if(`$INSTANCE_NAME`_CHECK_BYTE_COMPLETE(tmpCsr))
            {
                #if(`$INSTANCE_NAME`_MODE & `$INSTANCE_NAME`_MODE_MULTI_MASTER_ENABLE)
                    tmpCsr &= ~`$INSTANCE_NAME`_CSR_STOP_STATUS;  /* Clear STOP bit */
                #endif  /* End (`$INSTANCE_NAME`_MODE & `$INSTANCE_NAME`_MODE_MULTI_MASTER_ENABLE) */
                
                switch(`$INSTANCE_NAME`_State)
                {
                    case `$INSTANCE_NAME`_SM_MSTR_WR_ADDR:    /* After address is sent, WRITE some data */
                    case `$INSTANCE_NAME`_SM_MSTR_RD_ADDR:    /* After address is sent, READ some data */
                    
                        /* Check for Slave address ACK */
                        if(`$INSTANCE_NAME`_CHECK_ADDR_ACK(tmpCsr))  /* Check ACK */
                        {
                            /* Setup for transmit or receive of data */
                            if(`$INSTANCE_NAME`_State == `$INSTANCE_NAME`_SM_MSTR_WR_ADDR)   /* TRANSMIT data */
                            {
                                if(`$INSTANCE_NAME`_mstrWrBufSize > 0)    /* Check if at least one byte is transfered */
                                {
                                    /* Load first data byte */
                                    `$INSTANCE_NAME`_DATA_REG = `$INSTANCE_NAME`_mstrWrBufPtr[0u];
                                    `$INSTANCE_NAME`_TRANSMIT_DATA;            /* Transmit data */
                                    `$INSTANCE_NAME`_mstrWrBufIndex = 1u;      /* Set index to 2nd location */
                                    
                                    /* Set transmit state until done */
                                    `$INSTANCE_NAME`_State = `$INSTANCE_NAME`_SM_MSTR_WR_DATA; 
                                }
                                else   /* No data to tranfer */
                                {
                                    /* Handles 0 bytes transfer */
                                    #if (`$INSTANCE_NAME`_PSOC3_ES2 || `$INSTANCE_NAME`_PSOC5_ES1)
                                        `$INSTANCE_NAME`_GENERATE_STOP;     /* Generate STOP */
                                        
                                        /* Set WRITE complete */
                                        `$INSTANCE_NAME`_mstrStatus |= `$INSTANCE_NAME`_MSTAT_WR_CMPLT;
                                        
                                        /* Reset State Machine to IDLE */
                                        `$INSTANCE_NAME`_State  = `$INSTANCE_NAME`_SM_IDLE; 
                                        
                                    #else  /* The PSoC3 ES3 only handles this well */
                                        if(`$INSTANCE_NAME`_CHECK_NO_STOP(`$INSTANCE_NAME`_mstrControl))
                                        {
                                            /* Reset State Machine to HALT, expect RESTART */
                                            `$INSTANCE_NAME`_State  = `$INSTANCE_NAME`_SM_MSTR_HALT;
                                            
                                            /* Set WRITE complete and Master HALTED */
                                            `$INSTANCE_NAME`_mstrStatus |= (`$INSTANCE_NAME`_MSTAT_XFER_HALT | 
                                                                            `$INSTANCE_NAME`_MSTAT_WR_CMPLT); 
                                            
                                            CyIntClearPending(`$INSTANCE_NAME`_ISR_NUMBER);
                                            `$INSTANCE_NAME`_DisableInt();
                                        }
                                        else  /* Do normal STOP */
                                        {
                                            `$INSTANCE_NAME`_GENERATE_STOP;     /* Generate STOP */
                                             
                                            /* Set WRITE complete */
                                            `$INSTANCE_NAME`_mstrStatus |= `$INSTANCE_NAME`_MSTAT_WR_CMPLT;
                                        
                                             /* Reset State Machine to IDLE */
                                            `$INSTANCE_NAME`_State = `$INSTANCE_NAME`_SM_IDLE;
                                        }
                                        
                                    #endif  /* End (`$INSTANCE_NAME`_PSOC3_ES2 || `$INSTANCE_NAME`_PSOC5_ES1) */
                                }
                            }
                            else  /* Master Receive data */
                            {                                
                                `$INSTANCE_NAME`_READY_TO_READ;     /* Ready to READ data */
                                
                                /* Set state machine to READ data */
                                `$INSTANCE_NAME`_State  = `$INSTANCE_NAME`_SM_MSTR_RD_DATA;
                            }
                        }
                        /* Check for Slave address NAK */
                        else if(`$INSTANCE_NAME`_CHECK_ADDR_NAK(tmpCsr))  /* Check NACK */
                        {
                            if(`$INSTANCE_NAME`_CHECK_NO_STOP(`$INSTANCE_NAME`_mstrControl))
                            {
                                /* Reset State Machine to HALT, expect RESTART */
                                `$INSTANCE_NAME`_State  = `$INSTANCE_NAME`_SM_MSTR_HALT;
                                
                                /* Set Address NAK Error and Master HALTED */
                                `$INSTANCE_NAME`_mstrStatus |= `$INSTANCE_NAME`_MSTAT_ERR_ADDR_NAK |
                                                               `$INSTANCE_NAME`_MSTAT_XFER_HALT;
                                
                                CyIntClearPending(`$INSTANCE_NAME`_ISR_NUMBER);
                                `$INSTANCE_NAME`_DisableInt();
                            }
                            else  /* Do normal Stop */
                            {
                                `$INSTANCE_NAME`_GENERATE_STOP;     /* Generate STOP */
                                `$INSTANCE_NAME`_State = `$INSTANCE_NAME`_SM_IDLE;  /* Reset State Machine to idle */
                                
                                /* Set Address NAK Error */
                                `$INSTANCE_NAME`_mstrStatus |= `$INSTANCE_NAME`_MSTAT_ERR_ADDR_NAK; 
                            }
                        }
                        /* Bogus */
                        else
                        {
                            CyIntClearPending(`$INSTANCE_NAME`_ISR_NUMBER);
                            `$INSTANCE_NAME`_DisableInt();
                        }
                        break;
                        
                    case `$INSTANCE_NAME`_SM_MSTR_WR_DATA:    /* Write data to slave */
                        
                        if(`$INSTANCE_NAME`_CHECK_DATA_ACK(tmpCsr))       /* Check ACK */
                        {
                            if(`$INSTANCE_NAME`_mstrWrBufIndex  < `$INSTANCE_NAME`_mstrWrBufSize) /* Check if end buf */
                            {
                                 /* Load first data byte  */
                                `$INSTANCE_NAME`_DATA_REG = `$INSTANCE_NAME`_mstrWrBufPtr[`$INSTANCE_NAME`_mstrWrBufIndex];
                                `$INSTANCE_NAME`_TRANSMIT_DATA;     /* Transmit */
                                
                                `$INSTANCE_NAME`_mstrWrBufIndex++;  /* Advance to data location */
                            }
                            else   /* Last byte was transmitted, send STOP */
                            {
                                if(`$INSTANCE_NAME`_CHECK_NO_STOP(`$INSTANCE_NAME`_mstrControl))
                                {
                                    /* Reset State Machine to HALT, expect RESTART */
                                    `$INSTANCE_NAME`_State  = `$INSTANCE_NAME`_SM_MSTR_HALT;
                                    
                                    /* Set WRITE complete and Master HALTED */
                                    `$INSTANCE_NAME`_mstrStatus |= (`$INSTANCE_NAME`_MSTAT_WR_CMPLT |
                                                                    `$INSTANCE_NAME`_MSTAT_XFER_HALT);
                                    
                                    CyIntClearPending(`$INSTANCE_NAME`_ISR_NUMBER);
                                    `$INSTANCE_NAME`_DisableInt();
                                }
                                else  /* Do normal Stop */
                                {
                                    `$INSTANCE_NAME`_Workaround();      /* Workaround for CDT 78083 */
                                    `$INSTANCE_NAME`_GENERATE_STOP;     /* Generate STOP */
                                    `$INSTANCE_NAME`_State = `$INSTANCE_NAME`_SM_IDLE; /* Reset State Machine to idle */
                                    
                                    /* Set WRITE complete */
                                    `$INSTANCE_NAME`_mstrStatus |= `$INSTANCE_NAME`_MSTAT_WR_CMPLT;
                                }
                            }
                        }
                        else /* If last byte NAKed, stop transmit and send STOP */
                        {
                            if(`$INSTANCE_NAME`_CHECK_NO_STOP(`$INSTANCE_NAME`_mstrControl)) /* Check STOP generation */
                            {
                                /* Reset State Machine to HALT, expect RESTART */
                                `$INSTANCE_NAME`_State  = `$INSTANCE_NAME`_SM_MSTR_HALT;
                                
                                /* Set WRITE complete, SHORT transfer and Master HALTED */
                                `$INSTANCE_NAME`_mstrStatus |= (`$INSTANCE_NAME`_MSTAT_WR_CMPLT |
                                                                `$INSTANCE_NAME`_MSTAT_ERR_SHORT_XFER |
                                                                `$INSTANCE_NAME`_MSTAT_XFER_HALT);
                                
                                CyIntClearPending(`$INSTANCE_NAME`_ISR_NUMBER);  
                                `$INSTANCE_NAME`_DisableInt();
                            }
                            else  /* Do normal Stop */
                            {
                                `$INSTANCE_NAME`_GENERATE_STOP;         /* Generate STOP */
                                `$INSTANCE_NAME`_State = `$INSTANCE_NAME`_SM_IDLE;    /* Reset State Machine to IDLE */
                                
                                /* Set WRITE complete and SHORT transfer */
                                `$INSTANCE_NAME`_mstrStatus |= (`$INSTANCE_NAME`_MSTAT_WR_CMPLT | 
                                                                `$INSTANCE_NAME`_MSTAT_ERR_SHORT_XFER);
                            }
                        }
                        break;
                        
                    case `$INSTANCE_NAME`_SM_MSTR_RD_DATA:    /* Data received */
                        
                        `$INSTANCE_NAME`_mstrRdBufPtr[`$INSTANCE_NAME`_mstrRdBufIndex] = `$INSTANCE_NAME`_DATA_REG;
                        `$INSTANCE_NAME`_mstrRdBufIndex++;      /* Inc pointer */
                        if(`$INSTANCE_NAME`_mstrRdBufIndex < `$INSTANCE_NAME`_mstrRdBufSize)/* Check if end of buffer */
                        {
                            `$INSTANCE_NAME`_ACK_AND_RECEIVE;       /* ACK and receive */
                        }
                        else   /* End of data, generate a STOP */
                        {
                            if(`$INSTANCE_NAME`_CHECK_NO_STOP(`$INSTANCE_NAME`_mstrControl)) /* Check STOP generation */
                            {
                                /* Reset State Machine to HALT, expect RESTART */
                                `$INSTANCE_NAME`_State = `$INSTANCE_NAME`_SM_MSTR_HALT;    
                                
                                /* Set READ complete and Master HALTED */
                                `$INSTANCE_NAME`_mstrStatus |= (`$INSTANCE_NAME`_MSTAT_RD_CMPLT |
                                                                `$INSTANCE_NAME`_MSTAT_XFER_HALT );
                            }
                            else   /* Do normal Stop */
                            {
                                #if(`$INSTANCE_NAME`_MODE & `$INSTANCE_NAME`_MODE_MULTI_MASTER_ENABLE)
                                    `$INSTANCE_NAME`_NAK_AND_RECEIVE;           /* NACK and TRY to generate STOP */
                                    `$INSTANCE_NAME`_ENABLE_INT_ON_STOP;        /* Enable interrupt on STOP */
                                     /* Wait for STOP or LOST ARBITRAGE */
                                    `$INSTANCE_NAME`_State = `$INSTANCE_NAME`_SM_MSTR_WAIT_STOP;
                                    
                                #else
                                    `$INSTANCE_NAME`_NAK_AND_RECEIVE;                   /* NACK and and generate STOP */
                                    `$INSTANCE_NAME`_State = `$INSTANCE_NAME`_SM_IDLE;  /* Set state to IDLE */
                                    
                                    /* Set READ complete */
                                    `$INSTANCE_NAME`_mstrStatus |= `$INSTANCE_NAME`_MSTAT_RD_CMPLT;
                                    
                                #endif  /* End (`$INSTANCE_NAME`_MODE & `$INSTANCE_NAME`_MODE_MULTI_MASTER_ENABLE) */
                            }
                        }
                        break;
                        
                    /* Only wait for STOP if MultiMaster is used */
                    #if(`$INSTANCE_NAME`_MODE & `$INSTANCE_NAME`_MODE_MULTI_MASTER_ENABLE)
                        case `$INSTANCE_NAME`_SM_MSTR_WAIT_STOP:
                            `$INSTANCE_NAME`_READY_TO_READ;         /* The ARBITRAGE has been lost, release the bus */
                            `$INSTANCE_NAME`_DISABLE_INT_ON_STOP;                        /* Disable interrupt on STOP */
                            `$INSTANCE_NAME`_State = `$INSTANCE_NAME`_SM_IDLE;           /* Set state to IDLE */
                            /* Set status to complete READ */
                            `$INSTANCE_NAME`_mstrStatus |= `$INSTANCE_NAME`_MSTAT_RD_CMPLT;
                            break;
                    #endif  /* End (`$INSTANCE_NAME`_MODE & `$INSTANCE_NAME`_MODE_MULTI_MASTER_ENABLE) */
                                            
                    /* This case used for NO STOP condition, ready for a restart */
                    case `$INSTANCE_NAME`_SM_MSTR_HALT: /* Do one transfer and halt, used for polling or single stepping */
                        `$INSTANCE_NAME`_mstrStatus |= `$INSTANCE_NAME`_MSTAT_WR_CMPLT;
                        `$INSTANCE_NAME`_DisableInt();
                        break;
                        
                    default:
                        /* Invalid state, reset state machine  to a known state */
                        `$INSTANCE_NAME`_State = `$INSTANCE_NAME`_SM_IDLE;
                        
                        /* Set transfer complete with error */
                        `$INSTANCE_NAME`_mstrStatus |= `$INSTANCE_NAME`_MSTAT_ERR_XFER;
                        break;
                }
            }
            
            #if(`$INSTANCE_NAME`_MODE & `$INSTANCE_NAME`_MODE_MULTI_MASTER_ENABLE)
                /* Check if STOP was detected */
                if((tmpCsr & `$INSTANCE_NAME`_CSR_STOP_STATUS) != 0u)
                {
                    /* The READ transaction only gets here on STOP */
                    `$INSTANCE_NAME`_DISABLE_INT_ON_STOP;
                    `$INSTANCE_NAME`_State = `$INSTANCE_NAME`_SM_IDLE;  /* Set state to IDLE */
                    
                    /* Set status to complete READ */
                    `$INSTANCE_NAME`_mstrStatus |= `$INSTANCE_NAME`_MSTAT_RD_CMPLT;
                }
            #endif  /* End (`$INSTANCE_NAME`_MODE & `$INSTANCE_NAME`_MODE_MULTI_MASTER_ENABLE) */
            
        #endif  /* End (`$INSTANCE_NAME`_MODE & `$INSTANCE_NAME`_MODE_MASTER) */
    }
    else    /******** Slave Mode ********/
    {
        #if (`$INSTANCE_NAME`_MODE & `$INSTANCE_NAME`_MODE_SLAVE)
            /* Check to see if a Start/Address is detected */
            if((tmpCsr & `$INSTANCE_NAME`_CSR_ADDRESS) != 0u)
            {
                /* Clears CSR STOP_STATUS bit. This bit sets by ANY of Stop condition detection on the bus */
                tmpCsr &= ~`$INSTANCE_NAME`_CSR_STOP_STATUS;  /* Clear Stop bit */
                
                /* This is a Start or ReStart, So Reset the state machine, Check for a Read/Write condition */
                    
                /* Check for software address detection */
                #if (`$INSTANCE_NAME`_ADDR_DECODE == `$INSTANCE_NAME`_SW_DECODE)  
                    /******************* Software address detection ************************/
                    tmp8 = ((`$INSTANCE_NAME`_DATA_REG >> 1u) & `$INSTANCE_NAME`_SLAVE_ADDR_MASK);
                    if(tmp8 == `$INSTANCE_NAME`_slAddress)   /* Check for address match  */
                    {
                        /* Check for read or write command */
                        if((`$INSTANCE_NAME`_DATA_REG & `$INSTANCE_NAME`_READ_FLAG) != 0u) 
                        {
                            /*******************************************/
                            /*  Place code to prepare read buffer here */
                            /*******************************************/
                            /* `#START SW_PREPARE_READ_BUF` */
                            
                            /* `#END`  */
                            
                            /* Prepare next opeation to read, get data and place in data register */
                            if(`$INSTANCE_NAME`_slRdBufIndex < `$INSTANCE_NAME`_slRdBufSize)
                            {
                                /* Load first data byte */
                                `$INSTANCE_NAME`_DATA_REG = `$INSTANCE_NAME`_slRdBufPtr[`$INSTANCE_NAME`_slRdBufIndex];
                                `$INSTANCE_NAME`_ACK_AND_TRANSMIT;  /* ACK and transmit */
                                `$INSTANCE_NAME`_slRdBufIndex++;    /* Advance to data location */
                                
                                /* Set Read activity */
                                `$INSTANCE_NAME`_slStatus |= `$INSTANCE_NAME`_SSTAT_RD_BUSY; 
                            }
                            else    /* Data overflow */
                            {
                                `$INSTANCE_NAME`_DATA_REG = 0xFFu;    /* Out of range, send 0xFF  */
                                `$INSTANCE_NAME`_ACK_AND_TRANSMIT;    /* ACK and transmit */
                                
                                /* Set Read activity */
                                `$INSTANCE_NAME`_slStatus  |= (`$INSTANCE_NAME`_SSTAT_RD_BUSY | 
                                                               `$INSTANCE_NAME`_SSTAT_RD_ERR_OVFL); 
                            }
                            
                            `$INSTANCE_NAME`_State = `$INSTANCE_NAME`_SM_SL_RD_DATA; /* Prepare for Read transaction */
                        }
                        else  /* Start of a Write transaction, ready to write of the first byte */
                        {
                            /* Prepare to write the first byte */
                            `$INSTANCE_NAME`_ACK_AND_RECEIVE;
                            `$INSTANCE_NAME`_State = `$INSTANCE_NAME`_SM_SL_WR_DATA; /* Prepare for Write transaction */
                            
                            /* Set Write activity */
                            `$INSTANCE_NAME`_slStatus |= `$INSTANCE_NAME`_SSTAT_WR_BUSY;
                            `$INSTANCE_NAME`_ENABLE_INT_ON_STOP;    /* Enable interrupt on Stop */
                        }
                    }
                    /**********************************************/
                    /* Place compare for additional address here  */
                    /**********************************************/
                    /* `#START ADDR_COMPARE`  */
                    
                    /* `#END`  */
                    
                    else   /* No address match */
                    {
                        `$INSTANCE_NAME`_NAK_AND_RECEIVE;   /* NAK address */
                    }
                    
                #else  /* Hardware address detection */
                    /* Check for read or write command */
                    if((`$INSTANCE_NAME`_DATA_REG & `$INSTANCE_NAME`_READ_FLAG) != 0u)
                    {
                        /*******************************************/
                        /*  Place code to prepare read buffer here */
                        /*******************************************/
                        /* `#START SW_PREPARE_READ_BUF` */
                         
                        /* `#END`  */
                         
                        /* Prepare next opeation to read, get data and place in data register */
                        if(`$INSTANCE_NAME`_slRdBufIndex < `$INSTANCE_NAME`_slRdBufSize)
                        {
                            /* Load first data byte */
                            `$INSTANCE_NAME`_DATA_REG = `$INSTANCE_NAME`_slRdBufPtr[`$INSTANCE_NAME`_slRdBufIndex];
                            `$INSTANCE_NAME`_ACK_AND_TRANSMIT;                            /* ACK and transmit */
                            `$INSTANCE_NAME`_slRdBufIndex++;                              /* Advance to data location */
                            
                            /* Set Read activity */
                            `$INSTANCE_NAME`_slStatus  |= `$INSTANCE_NAME`_SSTAT_RD_BUSY;
                        }
                        else    /* Data overflow */
                        {
                            `$INSTANCE_NAME`_DATA_REG = 0xFFu;    /* Out of range, send 0xFF  */
                            `$INSTANCE_NAME`_ACK_AND_TRANSMIT;    /* ACK and transmit */
                            
                            /* Set Read activity */
                            `$INSTANCE_NAME`_slStatus  |= (`$INSTANCE_NAME`_SSTAT_RD_BUSY |
                                                           `$INSTANCE_NAME`_SSTAT_RD_ERR_OVFL);
                        }
                        
                        `$INSTANCE_NAME`_State = `$INSTANCE_NAME`_SM_SL_RD_DATA;    /* Prepare for Read transaction */
                    }
                    else  /* Start of a Write transaction, ready to write of the first byte */
                    {
                        /* Prepare to write the first byte */
                        `$INSTANCE_NAME`_ACK_AND_RECEIVE;       /* ACK and ready to receive addr */
                        `$INSTANCE_NAME`_State = `$INSTANCE_NAME`_SM_SL_WR_DATA;    /* Prepare for write transaction */
                        
                        /* Set Write activity */
                        `$INSTANCE_NAME`_slStatus |= `$INSTANCE_NAME`_SSTAT_WR_BUSY;
                        `$INSTANCE_NAME`_ENABLE_INT_ON_STOP;    /* Enable interrupt on Stop */
                    }
                    
                #endif  /* End (`$INSTANCE_NAME`_ADDR_DECODE == `$INSTANCE_NAME`_SW_DECODE) */
            }
            else if(`$INSTANCE_NAME`_CHECK_BYTE_COMPLETE(tmpCsr))    /* Check for data transfer */
            {
                if (`$INSTANCE_NAME`_State == `$INSTANCE_NAME`_SM_SL_WR_DATA)    /* Data write from Master to Slave */
                {
                    if(`$INSTANCE_NAME`_slWrBufIndex < `$INSTANCE_NAME`_slWrBufSize)       /* Check for valid range */
                    {
                        tmp8 = `$INSTANCE_NAME`_DATA_REG;                        /* Get data, to ACK quickly */
                        `$INSTANCE_NAME`_ACK_AND_RECEIVE;                        /* ACK and ready to receive */
                        `$INSTANCE_NAME`_slWrBufPtr[`$INSTANCE_NAME`_slWrBufIndex] = tmp8; /* Write data to array */
                        `$INSTANCE_NAME`_slWrBufIndex++;                        /* Inc pointer */
                    }
                    else
                    {
                        `$INSTANCE_NAME`_NAK_AND_RECEIVE;       /* NAK cause beyond write area */
                        
                        /* Set overflow, write completes on Stop */
                        `$INSTANCE_NAME`_slStatus |= `$INSTANCE_NAME`_SSTAT_WR_ERR_OVFL;
                    }
                }
                else if (`$INSTANCE_NAME`_State == `$INSTANCE_NAME`_SM_SL_RD_DATA) /* Data Read from Slave to Master */
                {
                    if(`$INSTANCE_NAME`_CHECK_DATA_ACK(tmpCsr))
                    {
                        if(`$INSTANCE_NAME`_slRdBufIndex < `$INSTANCE_NAME`_slRdBufSize) 
                        {
                             /* Get data from array */
                            `$INSTANCE_NAME`_DATA_REG = `$INSTANCE_NAME`_slRdBufPtr[`$INSTANCE_NAME`_slRdBufIndex];
                            `$INSTANCE_NAME`_TRANSMIT_DATA;         /* Send Data */
                            `$INSTANCE_NAME`_slRdBufIndex++;        /* Inc pointer */
                        }
                        else   /* Over flow */
                        {
                            `$INSTANCE_NAME`_DATA_REG = 0xFFu;          /* Send 0xFF at the end of the buffer */
                            `$INSTANCE_NAME`_TRANSMIT_DATA;             /* Send Data */
                            
                            /* Set overflow */
                            `$INSTANCE_NAME`_slStatus |= `$INSTANCE_NAME`_SSTAT_RD_ERR_OVFL;
                        }
                    }
                    else  /* Last byte NAKed, done */
                    {
                        `$INSTANCE_NAME`_DATA_REG = 0xFFu;      /* End of read transaction */
                        
                        `$INSTANCE_NAME`_NAK_AND_TRANSMIT;      /* Clear transmit bit at the end of read transaction */
                         /* Clear Busy Flag */
                        `$INSTANCE_NAME`_slStatus &= ~`$INSTANCE_NAME`_SSTAT_RD_BUSY;
                        /* Set Read complete Flag */
                        `$INSTANCE_NAME`_slStatus |= `$INSTANCE_NAME`_SSTAT_RD_CMPT;
                        
                        `$INSTANCE_NAME`_State = `$INSTANCE_NAME`_SM_IDLE;      /* Return to IDLE state */
                    }
                }
                else  /* This is an invalid state and should not occur */
                {
                    /* Invalid state, Reset */
                    `$INSTANCE_NAME`_State = `$INSTANCE_NAME`_SM_IDLE;
                    `$INSTANCE_NAME`_NAK_AND_RECEIVE;
                }   /* End Transfer mode */
            }  
            else if ((tmpCsr & `$INSTANCE_NAME`_CSR_BUS_ERROR) != 0u)    /* Quick check for Error */
            {
                if((`$INSTANCE_NAME`_CSR_REG & `$INSTANCE_NAME`_CSR_BUS_ERROR) != 0u)
                {
                    /* May want to reset bus here CHECK */
                }
            }  /* End if without else */
            
            if((tmpCsr & `$INSTANCE_NAME`_CSR_STOP_STATUS) != 0u)    /* Check if STOP was detected */
            {
                /* The Write transaction only IE on Stop, so Read never gets here */
                /* The WR_BUSY flag will be cleared at the end of "Write-ReStart-Read-Stop" transaction */
                /* Clear Busy Flag */
                `$INSTANCE_NAME`_slStatus &= ~`$INSTANCE_NAME`_SSTAT_WR_BUSY;
                /* Set complete Flag */
                `$INSTANCE_NAME`_slStatus |= `$INSTANCE_NAME`_SSTAT_WR_CMPT;
                
                `$INSTANCE_NAME`_DISABLE_INT_ON_STOP;               /* Disable interrupt on STOP */
                `$INSTANCE_NAME`_State = `$INSTANCE_NAME`_SM_IDLE;  /* Return to IDLE */
            }
        
        #endif  /* End (`$INSTANCE_NAME`_MODE & `$INSTANCE_NAME`_MODE_SLAVE) */
    }
    
    #if (`$INSTANCE_NAME`_PSOC3_ES2 && (`$INSTANCE_NAME`_I2C_IRQ__ES2_PATCH))
        `$INSTANCE_NAME`_ISR_PATCH();
    #endif  /* End (`$INSTANCE_NAME`_PSOC3_ES2 && (`$INSTANCE_NAME`_I2C_IRQ__ES2_PATCH)) */
}


/* [] END OF FILE */
