/*******************************************************************************
* File Name: `$INSTANCE_NAME`_INT.c  
* Version `$CY_MAJOR_VERSION`.`$CY_MINOR_VERSION`
*
*  Description:
*    This file contains the code that operates during the interrupt service
*    routine.  
*
*   Note:
*
*******************************************************************************
* Copyright 2008-2010, Cypress Semiconductor Corporation.  All rights reserved.
* You may use this file only in accordance with the license, terms, conditions, 
* disclaimers, and limitations in the end user license agreement accompanying 
* the software package with which this file was provided.
********************************************************************************/

#include "cytypes.h"
#include "`$INSTANCE_NAME`.h"  


/**********************************
*      System variables
**********************************/

volatile uint8 `$INSTANCE_NAME`_State;               /* Current state of I2C state machine */
volatile uint8 `$INSTANCE_NAME`_Status;              /* Status byte */

/* Master variables */
#if (`$INSTANCE_NAME`_MODE & `$INSTANCE_NAME`_MODE_MASTER)
    volatile uint8 `$INSTANCE_NAME`_mstrStatus;         /* Master Status byte */
    volatile uint8 `$INSTANCE_NAME`_mstrControl;        /* Master Control byte */
    
    /* Transmit buffer variables */
    uint8 * `$INSTANCE_NAME`_mstrRdBufPtr;              /* Pointer to Master Tx/Rx buffer */       
    volatile uint8 `$INSTANCE_NAME`_mstrRdBufSize;     /* Master buffer size  */
    volatile uint8  `$INSTANCE_NAME`_mstrRdBufIndex;     /* Master buffer Index */
    
    /* Receive buffer variables */
    uint8 * `$INSTANCE_NAME`_mstrWrBufPtr;              /* Pointer to Master Tx/Rx buffer */       
    volatile uint8 `$INSTANCE_NAME`_mstrWrBufSize;      /* Master buffer size  */
    volatile uint8 `$INSTANCE_NAME`_mstrWrBufIndex;     /* Master buffer Index */

#endif  /* End (`$INSTANCE_NAME`_MODE & `$INSTANCE_NAME`_MODE_MASTER) */

/* Slave variables */
#if (`$INSTANCE_NAME`_MODE & `$INSTANCE_NAME`_MODE_SLAVE)
    volatile uint8 `$INSTANCE_NAME`_slStatus;          /* Slave Status byte */

    #if (`$INSTANCE_NAME`_ADDR_DECODE == `$INSTANCE_NAME`_SW_DECODE)
        volatile uint8 `$INSTANCE_NAME`_Address;       /* Software address variable */
    #endif  /* End (`$INSTANCE_NAME`_ADDR_DECODE == `$INSTANCE_NAME`_SW_DECODE) */

    /* Transmit buffer variables */
    uint8 * `$INSTANCE_NAME`_readBufPtr;                /* Pointer to Transmit buffer */       
    volatile uint8 `$INSTANCE_NAME`_readBufSize;        /* Slave Transmit buffer size */
    volatile uint8 `$INSTANCE_NAME`_readBufIndex;       /* Slave Transmit buffer Index */

    /* Receive buffer variables */
    uint8 * `$INSTANCE_NAME`_writeBufPtr;               /* Pointer to Receive buffer */       
    volatile uint8 `$INSTANCE_NAME`_writeBufSize;       /* Slave Receive buffer size */
    volatile uint8 `$INSTANCE_NAME`_writeBufIndex;      /* Slave Receive buffer Index */

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
    /* a valid address is recognized. In software address compare mode, we have to */
    /* check every address after a start condition.                                 */

    tmpCsr = `$INSTANCE_NAME`_CSR_REG;          /* Make temp copy so that we can check */
                                                /* for stop condition after we are done */
    
    #if (`$INSTANCE_NAME`_MODE & `$INSTANCE_NAME`_MODE_MULTI_MASTER_ENABLE)
       
        /* Check for loss of arbitration  */
        if(`$INSTANCE_NAME`_CHECK_LOST_ARB(tmpCsr))
        {
            /* Clear CSR to release the bus, if no Slave */
            #if ((`$INSTANCE_NAME`_MODE & `$INSTANCE_NAME`_MODE_SLAVE) == 0u)
                `$INSTANCE_NAME`_READY_TO_READ;
            #endif  /* (`$INSTANCE_NAME`_MODE == `$INSTANCE_NAME`_MULTI_MASTER_ENABLE) */    

            /* Arbitration has been lost, reset state machine to Idle */
            `$INSTANCE_NAME`_State = `$INSTANCE_NAME`_SM_IDLE;
  
            /* Set status transfer complete and arbitration lost */
            `$INSTANCE_NAME`_mstrStatus |= (`$INSTANCE_NAME`_MSTAT_ERR_ARB_LOST | `$INSTANCE_NAME`_MSTAT_ERR_XFER);
        }
    #endif  /* End (`$INSTANCE_NAME`_MODE & `$INSTANCE_NAME`_MODE_MULTI_MASTER_ENABLE) */    
     
    /* Check for Master operation mode  */
    if(`$INSTANCE_NAME`_State & `$INSTANCE_NAME`_SM_MASTER)    /*******  Master *******/
    { 
        #if (`$INSTANCE_NAME`_MODE & `$INSTANCE_NAME`_MODE_MASTER)
        /* Enter Master state machine */
      
            if(`$INSTANCE_NAME`_CHECK_BYTE_COMPLETE(tmpCsr))
            {
                switch(`$INSTANCE_NAME`_State)
                {
                    case `$INSTANCE_NAME`_SM_MSTR_WR_ADDR:    /* After address is sent, Write some data */
                    case `$INSTANCE_NAME`_SM_MSTR_RD_ADDR:    /* After address is sent, Read some data */
                
                        /* Check for Slave address ACK */
                        if(`$INSTANCE_NAME`_CHECK_ADDR_ACK(tmpCsr))  /* Check ACK/NAK */
                        {
                            /* Setup for transmit or receive of data */
                            if((`$INSTANCE_NAME`_State & `$INSTANCE_NAME`_SM_MSTR_ADDR) == `$INSTANCE_NAME`_SM_MSTR_WR_ADDR)  /* Write data */
                            {
                                if(`$INSTANCE_NAME`_mstrWrBufSize > 0)    /* Check if at least one byte is transfered */
                                {
                                    `$INSTANCE_NAME`_DATA_REG = `$INSTANCE_NAME`_mstrWrBufPtr[0u];   /* Load first data byte  */
                                    `$INSTANCE_NAME`_TRANSMIT_DATA;                                  /* Transmit data */
                                    `$INSTANCE_NAME`_mstrWrBufIndex = 1u;                            /* Set index to 2nd location */
                                    `$INSTANCE_NAME`_State = `$INSTANCE_NAME`_SM_MSTR_WR_DATA;       /* Set transmit state until done */
                                }
                                else   /* No data to tranfer */
                                {
                                    /* Handles 0 bytes transfer */
                                    #if (`$INSTANCE_NAME`_PSOC3_ES2 || `$INSTANCE_NAME`_PSOC5_ES1)
                                        `$INSTANCE_NAME`_GENERATE_STOP;
                                        `$INSTANCE_NAME`_mstrStatus |= `$INSTANCE_NAME`_MSTAT_WR_CMPLT;  /* Set status to Transfer complete */
                                        `$INSTANCE_NAME`_State  = `$INSTANCE_NAME`_SM_IDLE;              /* Reset State Machine to idle */
                                        
                                    #else  /* The PSoC3 ES3 only handles this well */
                                        if(`$INSTANCE_NAME`_CHECK_NO_STOP(`$INSTANCE_NAME`_mstrControl))
                                        {
                                            /* Don't do stop, just halt */
                                            `$INSTANCE_NAME`_State  = `$INSTANCE_NAME`_SM_MSTR_HALT;    /* Reset State Machine to Halt, expect ReStart */
                                            `$INSTANCE_NAME`_mstrStatus |= `$INSTANCE_NAME`_MSTAT_XFER_HALT; 
                                            CyIntClearPending(`$INSTANCE_NAME`_ISR_NUMBER);
                                            `$INSTANCE_NAME`_DisableInt();
                                        }
                                        else  /* Do normal Stop */
                                        {
                                            `$INSTANCE_NAME`_GENERATE_STOP;
                                            `$INSTANCE_NAME`_State = `$INSTANCE_NAME`_SM_IDLE;    /* Reset State Machine to idle */
                                        }
                                        
                                    #endif  /* End (`$INSTANCE_NAME`_PSOC3_ES2 || `$INSTANCE_NAME`_PSOC5_ES1) */
                                }
                            }
                            else  /* Master Receive data */
                            {                                
                                `$INSTANCE_NAME`_READY_TO_READ;                              /* Ready to Read data */
                                `$INSTANCE_NAME`_State  = `$INSTANCE_NAME`_SM_MSTR_RD_DATA;  /* Set state machine to Read data */
                            }
                        }
                        /* The Address was NAKed */
                        else if(`$INSTANCE_NAME`_CHECK_ADDR_NAK(tmpCsr))
                        {
                            if(`$INSTANCE_NAME`_CHECK_NO_STOP(`$INSTANCE_NAME`_mstrControl))
                            {
                                /* Don't do stop, just halt */
                                `$INSTANCE_NAME`_State  = `$INSTANCE_NAME`_SM_MSTR_HALT;    /* Reset State Machine to Halt, expect ReStart */
                                `$INSTANCE_NAME`_mstrStatus |= `$INSTANCE_NAME`_MSTAT_ERR_ADDR_NAK | \
                                                               `$INSTANCE_NAME`_MSTAT_XFER_HALT; 
                                CyIntClearPending(`$INSTANCE_NAME`_ISR_NUMBER);
                                `$INSTANCE_NAME`_DisableInt();
                            }
                            else  /* Do normal Stop */
                            {
                                `$INSTANCE_NAME`_GENERATE_STOP;
                                `$INSTANCE_NAME`_State = `$INSTANCE_NAME`_SM_IDLE;    /* Reset State Machine to idle */
                                /* Set Address NAK Error */
                                `$INSTANCE_NAME`_mstrStatus |= `$INSTANCE_NAME`_MSTAT_ERR_ADDR_NAK; 
                            }
                        }
                        else   
                        {
                            /* Bogus */
                            CyIntClearPending(`$INSTANCE_NAME`_ISR_NUMBER);
                            `$INSTANCE_NAME`_DisableInt();
                        }
                        break;
                
                    case `$INSTANCE_NAME`_SM_MSTR_WR_DATA:                                             /* Write data to slave */
                
                        if(`$INSTANCE_NAME`_CHECK_DATA_ACK(tmpCsr))       /* Check ACK */
                        {
                            if(`$INSTANCE_NAME`_mstrWrBufIndex  < `$INSTANCE_NAME`_mstrWrBufSize)    /* Check if end of buffer */
                            {
                                 /* Load first data byte  */
                                `$INSTANCE_NAME`_DATA_REG = `$INSTANCE_NAME`_mstrWrBufPtr[`$INSTANCE_NAME`_mstrWrBufIndex];
                                `$INSTANCE_NAME`_TRANSMIT_DATA;                                        /* Transmit */
                                
                                `$INSTANCE_NAME`_mstrWrBufIndex++;                                     /* Advance to data location */
                            }
                            else   /* Last byte was transmitted, send STOP */
                            {
                                if(`$INSTANCE_NAME`_CHECK_NO_STOP(`$INSTANCE_NAME`_mstrControl))
                                {
                                    /* Don't do stop, just halt */
                                    `$INSTANCE_NAME`_State  = `$INSTANCE_NAME`_SM_MSTR_HALT;    /* Reset State Machine to Halt, expect ReStart */
                                    `$INSTANCE_NAME`_mstrStatus |= (`$INSTANCE_NAME`_MSTAT_WR_CMPLT | \
                                                                    `$INSTANCE_NAME`_MSTAT_XFER_HALT);
                                    CyIntClearPending(`$INSTANCE_NAME`_ISR_NUMBER);
                                    `$INSTANCE_NAME`_DisableInt();
                                }
                                else  /* Do normal Stop */
                                {
                                    `$INSTANCE_NAME`_Workaround(); /* Workaround for CDT 78083 */
                                    `$INSTANCE_NAME`_GENERATE_STOP;
                                    `$INSTANCE_NAME`_State = `$INSTANCE_NAME`_SM_IDLE;    /* Reset State Machine to idle */
                                    `$INSTANCE_NAME`_mstrStatus |= `$INSTANCE_NAME`_MSTAT_WR_CMPLT;
                                }
                            }
                        }
                        else /* If last byte NAKed, stop transmit and send STOP. */
                        {
                            if(`$INSTANCE_NAME`_CHECK_NO_STOP(`$INSTANCE_NAME`_mstrControl))
                            {
                                /* Don't do stop, just halt */
                                `$INSTANCE_NAME`_State  = `$INSTANCE_NAME`_SM_MSTR_HALT;    /* Reset State Machine to Halt, expect ReStart */
                                `$INSTANCE_NAME`_mstrStatus |= (`$INSTANCE_NAME`_MSTAT_WR_CMPLT | \
                                                                `$INSTANCE_NAME`_MSTAT_XFER_HALT | \
                                                                `$INSTANCE_NAME`_MSTAT_ERR_SHORT_XFER);
                                CyIntClearPending(`$INSTANCE_NAME`_ISR_NUMBER);  
                                `$INSTANCE_NAME`_DisableInt();
                            }
                            else  /* Do normal Stop */
                            {
                                `$INSTANCE_NAME`_GENERATE_STOP;
                                `$INSTANCE_NAME`_State = `$INSTANCE_NAME`_SM_IDLE;    /* Reset State Machine to idle */
                                `$INSTANCE_NAME`_mstrStatus |= (`$INSTANCE_NAME`_MSTAT_WR_CMPLT | \
                                                                `$INSTANCE_NAME`_MSTAT_ERR_SHORT_XFER);
                            }
                        }
                        break;
                
                    case `$INSTANCE_NAME`_SM_MSTR_RD_DATA:  /* Data received */  
                
                        `$INSTANCE_NAME`_mstrRdBufPtr[`$INSTANCE_NAME`_mstrRdBufIndex] = `$INSTANCE_NAME`_DATA_REG; 
                        `$INSTANCE_NAME`_mstrRdBufIndex++;
                        if(`$INSTANCE_NAME`_mstrRdBufIndex  < `$INSTANCE_NAME`_mstrRdBufSize)    /* Check if end of buffer */
                        {
                            `$INSTANCE_NAME`_ACK_AND_RECEIVE;
                        }
                        else   /* End of data, generate a STOP */
                        {
                            if(`$INSTANCE_NAME`_CHECK_NO_STOP(`$INSTANCE_NAME`_mstrControl))
                            {
                                `$INSTANCE_NAME`_State = `$INSTANCE_NAME`_SM_MSTR_HALT;    /* Reset State Machine to Halt, expect ReStart */
                                `$INSTANCE_NAME`_mstrStatus |= (`$INSTANCE_NAME`_MSTAT_RD_CMPLT | \
                                                                `$INSTANCE_NAME`_MSTAT_XFER_HALT );
                            }
                            else   /* Do normal Stop */
                            {
                                `$INSTANCE_NAME`_NAK_AND_RECEIVE;
                                `$INSTANCE_NAME`_State = `$INSTANCE_NAME`_SM_IDLE;                /* Set state to idle */
                                `$INSTANCE_NAME`_mstrStatus |= `$INSTANCE_NAME`_MSTAT_RD_CMPLT;   /* Set status to complete read */
                            }
                        }
                        break;
                
                    case `$INSTANCE_NAME`_SM_MSTR_WAIT_STOP:
                        `$INSTANCE_NAME`_mstrStatus |= `$INSTANCE_NAME`_MSTAT_ERR_XFER;
                        break;
                
                    /* This case used for NO STOP condition, ready for a restart */
                    case `$INSTANCE_NAME`_SM_MSTR_HALT:   /* Do one transfer and halt, used for polling or single stepping */
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
            
        #endif  /* End (`$INSTANCE_NAME`_MODE & `$INSTANCE_NAME`_MODE_MASTER) */
    }
    else  /******** Slave Mode ********/                                        
    {
        #if (`$INSTANCE_NAME`_MODE & `$INSTANCE_NAME`_MODE_SLAVE)
            /* Check to see if a Start/Address is detected */
            if((tmpCsr & `$INSTANCE_NAME`_CSR_ADDRESS) != 0u)
            {
                /* CSR bit _STOP_STATUS clears when Read/Write opearion */
                tmpCsr &=  ~`$INSTANCE_NAME`_CSR_STOP_STATUS;  /* Clear Stop bit */
                
                /* This is a Start or ReStart, So Reset the state machine, Check for a Read/Write condition */
                    
                #if (`$INSTANCE_NAME`_ADDR_DECODE == `$INSTANCE_NAME`_SW_DECODE)  /* Check for software address detection */
                    /******************* Software address detection ************************/
                    tmp8 = ((`$INSTANCE_NAME`_DATA_REG >> 1u) & `$INSTANCE_NAME`_SADDR_MASK);
                    if(tmp8 == `$INSTANCE_NAME`_Address)   /* Check for address match  */
                    {
                        if((`$INSTANCE_NAME`_DATA_REG & `$INSTANCE_NAME`_READ_FLAG) != 0u)  /* Check for read or write command */
                        {
                            /*******************************************/
                            /*  Place code to prepare read buffer here */
                            /*******************************************/
                            /* `#START SW_PREPARE_READ_BUF`  */
                
                            /* `#END`  */
                
                            /* Prepare next opeation to read, Get data and place in data register */
                            if(`$INSTANCE_NAME`_readBufIndex < `$INSTANCE_NAME`_readBufSize)  
                            {
                                `$INSTANCE_NAME`_DATA_REG = `$INSTANCE_NAME`_readBufPtr[`$INSTANCE_NAME`_readBufIndex];   /* Load first data byte  */
                                `$INSTANCE_NAME`_ACK_AND_TRANSMIT;
                                `$INSTANCE_NAME`_readBufIndex++;                                                      /* Advance to data location */
                                `$INSTANCE_NAME`_slStatus |= `$INSTANCE_NAME`_SSTAT_RD_BUSY;                          /* Set Read activity */
                            }
                            else   /* Data overflow */
                            {
                                `$INSTANCE_NAME`_DATA_REG = 0xFFu;    /* Out of range, send 0xFF  */
                                `$INSTANCE_NAME`_ACK_AND_TRANSMIT;
                                `$INSTANCE_NAME`_slStatus  |= (`$INSTANCE_NAME`_SSTAT_RD_BUSY | \
                                                               `$INSTANCE_NAME`_SSTAT_RD_ERR_OVFL); /* Set Read activity */
                            }
                            
                            `$INSTANCE_NAME`_State = `$INSTANCE_NAME`_SM_SL_RD_DATA;                                /* Prepare for read transaction */
                        }
                        else  /* Start of a Write transaction, reset pointers, first byte is address */
                        {
                            /* Prepare next opeation to write offset */
                            `$INSTANCE_NAME`_ACK_AND_RECEIVE;
                            `$INSTANCE_NAME`_slStatus |= `$INSTANCE_NAME`_SSTAT_WR_BUSY;       /* Set Write activity */
                            `$INSTANCE_NAME`_State     = `$INSTANCE_NAME`_SM_SL_WR_DATA;       /* Prepare for read transaction */
                
                            /* Enable interrupt on Stop */
                            `$INSTANCE_NAME`_ENABLE_INT_ON_STOP;
                        } 
                    }
                    /**********************************************/
                    /* Place compare for additional address here  */
                    /**********************************************/
                    /* `#START ADDR_COMPARE`  */
                
                    /* `#END`  */
                
                    else   /* No address match */
                    {
                        /* NAK address Match  */
                        `$INSTANCE_NAME`_NAK_AND_RECEIVE;
                    }
            
                #else  /* Hardware address detection */
                    if((`$INSTANCE_NAME`_DATA_REG & `$INSTANCE_NAME`_READ_FLAG) != 0u)  /* Check for read or write command */
                    {
                        /*******************************************/
                        /*  Place code to prepare read buffer here */
                        /*******************************************/
                        /* `#START SW_PREPARE_READ_BUF`  */
                
                        /* `#END`  */
                
                        /* Prepare next opeation to read, Get data and place in data register */
                        if(`$INSTANCE_NAME`_readBufIndex < `$INSTANCE_NAME`_readBufSize)  
                        {
                            `$INSTANCE_NAME`_DATA_REG = `$INSTANCE_NAME`_readBufPtr[`$INSTANCE_NAME`_readBufIndex];   /* Load first data byte  */
                            `$INSTANCE_NAME`_ACK_AND_TRANSMIT;                                                    /* ACK and transmit */
                            `$INSTANCE_NAME`_readBufIndex++;                                                      /* Advance to data location */
                            `$INSTANCE_NAME`_slStatus  |= `$INSTANCE_NAME`_SSTAT_RD_BUSY;                         /* Set Read activity */
                        }
                        else   /* Data overflow */
                        {
                            #if defined(CYDEV_BOOTLOADER_IO_COMP) && (CYDEV_BOOTLOADER_IO_COMP == CyBtldr_`@INSTANCE_NAME`)
                                `$INSTANCE_NAME`_slStatus |= (`$INSTANCE_NAME`_SSTAT_RD_BUSY | \
                                                              `$INSTANCE_NAME`_SSTAT_RD_ERR_OVFL);      /* Set Read activity and Start clock stretching,
                                                                                                           SCL = 0, till the CyBtldrCommWrite() gives the 
                                                                                                           ACK response and data */	
                            #else
                                `$INSTANCE_NAME`_DATA_REG = 0xFFu;    /* Out of range, send 0xFF  */
                                `$INSTANCE_NAME`_ACK_AND_TRANSMIT;
                                `$INSTANCE_NAME`_slStatus  |= (`$INSTANCE_NAME`_SSTAT_RD_BUSY | \
                                                               `$INSTANCE_NAME`_SSTAT_RD_ERR_OVFL); /* Set Read activity */
                            
                            #endif  /* End defined(CYDEV_BOOTLOADER_IO_COMP) && 
                                       (CYDEV_BOOTLOADER_IO_COMP == CyBtldr_`@INSTANCE_NAME`) */
                        }
                        
                        `$INSTANCE_NAME`_State = `$INSTANCE_NAME`_SM_SL_RD_DATA;    /* Prepare for read transaction */
                    }
                    else  /* Start of a Write transaction, reset pointers, first byte is address */
                    {
                        /* Prepare next opeation to write offset */
                        `$INSTANCE_NAME`_ACK_AND_RECEIVE; /* ACK and ready to receive addr */
                        `$INSTANCE_NAME`_slStatus |= `$INSTANCE_NAME`_SSTAT_WR_BUSY;       /* Set Write activity */
                        `$INSTANCE_NAME`_State     = `$INSTANCE_NAME`_SM_SL_WR_DATA;       /* Prepare for read transaction */
                        `$INSTANCE_NAME`_ENABLE_INT_ON_STOP;                               /* Enable interrupt on Stop */
                    }
                    
                #endif  /* End (`$INSTANCE_NAME`_ADDR_DECODE == `$INSTANCE_NAME`_SW_DECODE) */
                
            }
            else if(`$INSTANCE_NAME`_CHECK_BYTE_COMPLETE(tmpCsr))    /* Check for data transfer */
            {
                if (`$INSTANCE_NAME`_State == `$INSTANCE_NAME`_SM_SL_WR_DATA)    /* Data write from Master to Slave. */
                {
                    if(`$INSTANCE_NAME`_writeBufIndex < `$INSTANCE_NAME`_writeBufSize)       /* Check for valid range */
                    {
                        tmp8 = `$INSTANCE_NAME`_DATA_REG;                                        /* Get data, to ACK quickly */
                        `$INSTANCE_NAME`_ACK_AND_RECEIVE;                                    /* ACK and ready to receive sub addr */
                        `$INSTANCE_NAME`_writeBufPtr[`$INSTANCE_NAME`_writeBufIndex] = tmp8; /* Write data to array */
                        `$INSTANCE_NAME`_writeBufIndex++;                                    /* Inc pointer */
                    }
                    else
                    {
                        /* NAK cause beyond write area */
                        `$INSTANCE_NAME`_NAK_AND_RECEIVE;
                        `$INSTANCE_NAME`_slStatus &= ~`$INSTANCE_NAME`_SSTAT_WR_BUSY;        /* Set Write activity */
                        `$INSTANCE_NAME`_slStatus |= (`$INSTANCE_NAME`_SSTAT_WR_CMPT | \
                                                      `$INSTANCE_NAME`_SSTAT_WR_ERR_OVFL);   /* Set Write activity */
                    }
                }
                else if (`$INSTANCE_NAME`_State == `$INSTANCE_NAME`_SM_SL_RD_DATA)    /* Data Read from Slave to Master */
                {
                    if(`$INSTANCE_NAME`_CHECK_DATA_ACK(tmpCsr))
                    {
                        if(`$INSTANCE_NAME`_readBufIndex < `$INSTANCE_NAME`_readBufSize) 
                        {
                             /* Get data from array */
                            `$INSTANCE_NAME`_DATA_REG = `$INSTANCE_NAME`_readBufPtr[`$INSTANCE_NAME`_readBufIndex];
            
                            /* Send Data */
                            `$INSTANCE_NAME`_TRANSMIT_DATA;
                            `$INSTANCE_NAME`_readBufIndex++;                                  /* Inc pointer */
                        }
                        else   /* Over flow */
                        {
                            /* Send 0xFF at the end of the buffer */
                            `$INSTANCE_NAME`_DATA_REG = 0xFFu;
            
                            /* Send Data */
                            `$INSTANCE_NAME`_TRANSMIT_DATA;
                            `$INSTANCE_NAME`_slStatus  |= `$INSTANCE_NAME`_SSTAT_RD_ERR_OVFL; /* Set overflow */
                        }
                    }
                    else  /* Last byte NAKed, done */
                    {
                        /* End of read transaction */
                        `$INSTANCE_NAME`_DATA_REG = 0xFFu;
                         
                        /* Clear transmit bit at the end of read transaction */
                        `$INSTANCE_NAME`_NAK_AND_TRANSMIT;
                        `$INSTANCE_NAME`_slStatus &= ~`$INSTANCE_NAME`_SSTAT_RD_BUSY;   /* Clear Busy Flag */
                        `$INSTANCE_NAME`_slStatus |= `$INSTANCE_NAME`_SSTAT_RD_CMPT;    /* Set complete Flag */
                        `$INSTANCE_NAME`_State = `$INSTANCE_NAME`_SM_IDLE;              /* Return to idle state */
                    }
                }
                else  /* This is an invalid state and should not occur  */
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
            } /* End if without else */
            
            if((tmpCsr & `$INSTANCE_NAME`_CSR_STOP_STATUS) != 0u)    /* Check if Stop was detected */
            {
                /* 1) The Write transaction only IE on Stop, so Read never gets here */
                /* 2) The WR_BUSY flag will be cleared at the end of "Write-ReStart-Read-Stop" transaction */
                `$INSTANCE_NAME`_slStatus &= ~`$INSTANCE_NAME`_SSTAT_WR_BUSY;   /* Clear Busy Flag */
                `$INSTANCE_NAME`_slStatus |= `$INSTANCE_NAME`_SSTAT_WR_CMPT;    /* Set complete Flag */
                `$INSTANCE_NAME`_DISABLE_INT_ON_STOP;
                `$INSTANCE_NAME`_State = `$INSTANCE_NAME`_SM_IDLE;
            }       
        
        #endif  /* End (`$INSTANCE_NAME`_MODE & `$INSTANCE_NAME`_MODE_SLAVE) */
    }
    
    #if (`$INSTANCE_NAME`_PSOC3_ES2 && (`$INSTANCE_NAME`_I2C_IRQ__ES2_PATCH))
        `$INSTANCE_NAME`_ISR_PATCH();
    #endif  /* End (`$INSTANCE_NAME`_PSOC3_ES2 && (`$INSTANCE_NAME`_I2C_IRQ__ES2_PATCH)) */
}


/* [] END OF FILE */
