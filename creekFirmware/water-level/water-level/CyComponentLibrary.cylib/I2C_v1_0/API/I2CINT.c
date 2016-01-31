/*******************************************************************************
* File Name: `$INSTANCE_NAME`INT.c  
* Version `$CY_MAJOR_VERSION`.`$CY_MINOR_VERSION`
*
*  Description:
*    This file contains the code that operates during the interrupt service
*    routine.  
*
*   Note:
*
*******************************************************************************
* Copyright 2008-2009, Cypress Semiconductor Corporation.  All rights reserved.
* You may use this file only in accordance with the license, terms, conditions, 
* disclaimers, and limitations in the end user license agreement accompanying 
* the software package with which this file was provided.
********************************************************************************/



#include "cytypes.h"
#include "`$INSTANCE_NAME`.h"  
#include <`$INSTANCE_NAME`_I2C_IRQ.h>


/**********************************
*      System variables
**********************************/

/*** Slave variables ***/
uint8   `$INSTANCE_NAME`_State;            /* Current state of I2C state machine */
uint8   `$INSTANCE_NAME`_Status;           /* Status byte */

#if (`$INSTANCE_NAME`_MODE & `$INSTANCE_NAME`_MODE_SLAVE)
   uint8   `$INSTANCE_NAME`_slStatus;      /* Slave Status byte */

   #if (`$INSTANCE_NAME`_ADDR_DECODE == `$INSTANCE_NAME`_SW_DECODE)
      uint8   `$INSTANCE_NAME`_Address;    /* Software address variable */
   #endif

   /*** Slave Buffer variables ***/
   /* Transmit buffer vars */
   uint8 * `$INSTANCE_NAME`_readBufPtr;    /* Pointer to Transmit buffer */       
   uint8   `$INSTANCE_NAME`_readBufSize;   /* Slave Transmit buffer size */
   uint8   `$INSTANCE_NAME`_readBufIndex;  /* Slave Transmit buffer Index */

   /* Receive buffer vars */
   uint8 * `$INSTANCE_NAME`_writeBufPtr;    /* Pointer to Receive buffer */       
   uint8   `$INSTANCE_NAME`_writeBufSize;   /* Slave Receive buffer size */
   uint8   `$INSTANCE_NAME`_writeBufIndex;  /* Slave Receive buffer Index */

#endif

/*** Master Buffer variables ***/
#if ( `$INSTANCE_NAME`_MODE & `$INSTANCE_NAME`_MODE_MASTER )
   uint8 * `$INSTANCE_NAME`_mstrRdBufPtr;    /* Pointer to Master Tx/Rx buffer */       
   uint8   `$INSTANCE_NAME`_mstrRdBufSize;   /* Master buffer size  */
   uint8   `$INSTANCE_NAME`_mstrRdBufIndex;  /* Master buffer Index */

   uint8 * `$INSTANCE_NAME`_mstrWrBufPtr;    /* Pointer to Master Tx/Rx buffer */       
   uint8   `$INSTANCE_NAME`_mstrWrBufSize;   /* Master buffer size  */
   uint8   `$INSTANCE_NAME`_mstrWrBufIndex;  /* Master buffer Index */

   uint8   `$INSTANCE_NAME`_mstrStatus;      /* Master Status byte */
   uint8   `$INSTANCE_NAME`_mstrControl;     /* Master Control byte */
#endif


/*******************************************************************************
* Function Name: `$INSTANCE_NAME`_ISR
********************************************************************************
* Summary:
*  Handle Interrupt Service Routine.  
*
* Parameters:  
*  (void)
*
* Return: 
*  (void) 
*
* Theory: 
*
* Side Effects:
*
*******************************************************************************/
CY_ISR( `$INSTANCE_NAME`_ISR )
{
    #if ( `$INSTANCE_NAME`_MODE & `$INSTANCE_NAME`_MODE_SLAVE )
       static uint8  tmp8;    /* Making these static so not wasting time allocating */
    #endif

    static uint8  tmpCsr;  /* on the stack each time and no one else can see them */

    /* Entry from interrupt */
    /* In hardware address compare mode, we can assume we only get interrupted when */
    /* a valid address is recognized.  In software address compare mode, we have to */
    /* check every address after a start condition.                                 */

    tmpCsr = `$INSTANCE_NAME`_CSR;             /* Make temp copy so that we can check */
                                               /* for stop condition after we are done */

    /* Check for Master operation mode  */
    if( `$INSTANCE_NAME`_State & `$INSTANCE_NAME`_SM_MASTER )    /*******  Master *******/
    { 
        #if ( `$INSTANCE_NAME`_MODE & `$INSTANCE_NAME`_MODE_MASTER )

        /* Check for loss of arbitration  */
//        if( tmpCsr & `$INSTANCE_NAME`_CSR_LOST_ARB)
//        {
//            /* Arbitration has been lost, reset state machine to Idle  */
//            `$INSTANCE_NAME`_State  = `$INSTANCE_NAME`_SM_IDLE;
//
//            /* Set status transfer complete and arbitration lost. */
//            `$INSTANCE_NAME`_mstrStatus |= `$INSTANCE_NAME`_MSTAT_ERR_ARB_LOST & `$INSTANCE_NAME`_MSTAT_ERR_XFER;
//        }

        /*******  Enter Master state machine.  *******/
      
        if( tmpCsr & `$INSTANCE_NAME`_CSR_BYTE_COMPLETE)
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
                        if( `$INSTANCE_NAME`_mstrWrBufSize > 0 )    /* Check if at least one byte is transfered */
                        {
                            `$INSTANCE_NAME`_DATA = `$INSTANCE_NAME`_mstrWrBufPtr[0];                             /* Load first data byte  */
                            //`$INSTANCE_NAME`_CSR = `$INSTANCE_NAME`_CSR_TRANSMIT;
                            `$INSTANCE_NAME`_TRANSMIT_DATA;                                                       /* Transmit data */
                            `$INSTANCE_NAME`_mstrWrBufIndex = 1;                                                  /* Set index to 2nd location */
                            `$INSTANCE_NAME`_State = `$INSTANCE_NAME`_SM_MSTR_WR_DATA;                            /* Set transmit state until done */
                        }
                        else   /* No data to tranfer */
                        {
                            /* Sending 0 bytes is actually illegal, but just in case */
                            // FF   `$INSTANCE_NAME`_CSR = `$INSTANCE_NAME`_CSR_GEN_STOP;            /* Send stop */
                            `$INSTANCE_NAME`_GENERATE_STOP;

                            `$INSTANCE_NAME`_mstrStatus |= `$INSTANCE_NAME`_MSTAT_WR_CMPLT;  /* Set status to Transfer complete */
                            `$INSTANCE_NAME`_State  = `$INSTANCE_NAME`_SM_IDLE;              /* Reset State Machine to idle */
                        }
                    }
                    else  /* Master Receive data */
                    {
                        `$INSTANCE_NAME`_READY_TO_RD;                                /* Ready to Read data */
                        `$INSTANCE_NAME`_State  = `$INSTANCE_NAME`_SM_MSTR_RD_DATA;  /* Set state machine to Read data */
                    }

                }
                 /* The Address was NAKed, do Stop and halt transfer */
                else if(`$INSTANCE_NAME`_CHECK_ADDR_NAK(tmpCsr))
                {
                    `$INSTANCE_NAME`_GENERATE_STOP;
                    `$INSTANCE_NAME`_State  = `$INSTANCE_NAME`_SM_IDLE;    /* Reset State Machine to idle */

                    /* Set transfer complete and Address NAK Error */
                    `$INSTANCE_NAME`_mstrStatus |= `$INSTANCE_NAME`_MSTAT_ERR_ADDR_NAK;
                }
                else   
                {
                    /* Bogus */
                    `$INSTANCE_NAME`_I2C_IRQ_ClearPending();
                    `$INSTANCE_NAME`_DisableInt();
                }
                break;

            case `$INSTANCE_NAME`_SM_MSTR_WR_DATA:                                             /* Write data to slave */

                if( (tmpCsr & `$INSTANCE_NAME`_CSR_LRB) == `$INSTANCE_NAME`_CSR_LRB_ACK)       /* Check ACK */
                {
                    if( `$INSTANCE_NAME`_mstrWrBufIndex  < `$INSTANCE_NAME`_mstrWrBufSize )    /* Check if end of buffer */
                    {
                        `$INSTANCE_NAME`_DATA = `$INSTANCE_NAME`_mstrWrBufPtr[`$INSTANCE_NAME`_mstrWrBufIndex]; /* Load first data byte  */
                        `$INSTANCE_NAME`_TRANSMIT_DATA;                                        /* Transmit */
                        
                        `$INSTANCE_NAME`_mstrWrBufIndex++;                                     /* Advance to data location */
                    }
                    else   /* Last byte was transmitted, send STOP */
                    {
                       if(`$INSTANCE_NAME`_mstrControl & `$INSTANCE_NAME`_MSTR_NO_STOP)
                       {
                          /* Don't do stop, just halt */
                           `$INSTANCE_NAME`_State  = `$INSTANCE_NAME`_SM_MSTR_HALT;    /* Reset State Machine to idle */
                           `$INSTANCE_NAME`_mstrStatus |= `$INSTANCE_NAME`_MSTAT_WR_CMPLT | `$INSTANCE_NAME`_MSTAT_XFER_HALT ;
                           `$INSTANCE_NAME`_I2C_IRQ_ClearPending();
                           `$INSTANCE_NAME`_DisableInt();
                       }
                       else  /* Do normal Stop */
                       {
                           `$INSTANCE_NAME`_GENERATE_STOP;
                           `$INSTANCE_NAME`_State  = `$INSTANCE_NAME`_SM_IDLE;    /* Reset State Machine to idle */
                           `$INSTANCE_NAME`_mstrStatus |= `$INSTANCE_NAME`_MSTAT_WR_CMPLT;
                       }
                    }
                }
                else /*  If last byte NAKed, stop transmit and send STOP. */
                {
                    if(`$INSTANCE_NAME`_mstrControl & `$INSTANCE_NAME`_MSTR_NO_STOP)
                     {
                        /* Don't do stop, just halt */
                         `$INSTANCE_NAME`_State  = `$INSTANCE_NAME`_SM_MSTR_HALT;    /* Reset State Machine to idle */
                         `$INSTANCE_NAME`_mstrStatus |= (`$INSTANCE_NAME`_MSTAT_WR_CMPLT | `$INSTANCE_NAME`_MSTAT_XFER_HALT | `$INSTANCE_NAME`_MSTAT_ERR_SHORT_XFER);
                         `$INSTANCE_NAME`_I2C_IRQ_ClearPending();  
                         `$INSTANCE_NAME`_DisableInt();
                     }
                     else  /* Do normal Stop */
                     {
                         `$INSTANCE_NAME`_GENERATE_STOP;
                         
                         `$INSTANCE_NAME`_State  = `$INSTANCE_NAME`_SM_IDLE;    /* Reset State Machine to idle */
                         `$INSTANCE_NAME`_mstrStatus |= (`$INSTANCE_NAME`_MSTAT_WR_CMPLT | `$INSTANCE_NAME`_MSTAT_ERR_SHORT_XFER);
                     }
                }
                break;

            case `$INSTANCE_NAME`_SM_MSTR_RD_DATA:  /* Data received */  

                `$INSTANCE_NAME`_mstrRdBufPtr[`$INSTANCE_NAME`_mstrRdBufIndex] = `$INSTANCE_NAME`_DATA; 
                `$INSTANCE_NAME`_mstrRdBufIndex++;
                if( `$INSTANCE_NAME`_mstrRdBufIndex  < `$INSTANCE_NAME`_mstrRdBufSize )    /* Check if end of buffer */
                {
                   `$INSTANCE_NAME`_ACK_AND_RECIVE;
                }
                else   /* End of data, generate a STOP */
                {
                    if(`$INSTANCE_NAME`_mstrControl & `$INSTANCE_NAME`_MSTR_NO_STOP)
                    {
                        `$INSTANCE_NAME`_State  = `$INSTANCE_NAME`_SM_MSTR_HALT;    /* Reset State Machine to idle */
                        `$INSTANCE_NAME`_mstrStatus |= (`$INSTANCE_NAME`_MSTAT_RD_CMPLT | `$INSTANCE_NAME`_MSTAT_XFER_HALT );
                    }
                    else   /* Do normal Stop */
                    {
                        `$INSTANCE_NAME`_NAK_AND_RECIVE;
                        `$INSTANCE_NAME`_State  = `$INSTANCE_NAME`_SM_IDLE;                /* Set state to idle */
                        `$INSTANCE_NAME`_mstrStatus |= `$INSTANCE_NAME`_MSTAT_RD_CMPLT ;   /* Set status to complete read */
                    }
                }
                break;

            case `$INSTANCE_NAME`_SM_MSTR_WAIT_STOP:
                `$INSTANCE_NAME`_mstrStatus |= `$INSTANCE_NAME`_MSTAT_ERR_XFER;
                break;

            /* This case used for NO STOP condition, ready for a restart */
            case `$INSTANCE_NAME`_SM_MSTR_HALT:   /* Do one transfer and halt, used for polling or single stepping */
                `$INSTANCE_NAME`_mstrStatus |= `$INSTANCE_NAME`_MSTAT_WR_CMPLT ;
                `$INSTANCE_NAME`_DisableInt();
                break;

            default:
                /* Invalid state, reset state machine  to a known state */
                `$INSTANCE_NAME`_State  = `$INSTANCE_NAME`_SM_IDLE;

                /* Set transfer complete with error */
                `$INSTANCE_NAME`_mstrStatus |= `$INSTANCE_NAME`_MSTAT_ERR_XFER ;

                break;
            }
        }
        #endif
    }
    else                                          /******** Slave Mode ********/
    {
        #if ( `$INSTANCE_NAME`_MODE & `$INSTANCE_NAME`_MODE_SLAVE )

        if(tmpCsr & `$INSTANCE_NAME`_CSR_ADDRESS)  /* Check to see if a Start/Address is detected */
        {                                                       /* This is a Start or ReStart  */
                                                                /* So Reset the state machine  */
                                                                /* Check for a Read/Write condition  */

            #if (`$INSTANCE_NAME`_ADDR_DECODE == `$INSTANCE_NAME`_SW_DECODE)  /* Check for software address detection */
             /******************* Software address detection ************************/

            tmp8 = ((`$INSTANCE_NAME`_DATA >> 1) & 0x7Fu);
            if( tmp8 == `$INSTANCE_NAME`_Address )   /* Check for address match  */
            {
               if(`$INSTANCE_NAME`_DATA & `$INSTANCE_NAME`_READ_FLAG)  /* Check for read or write command */
               {
                   /*******************************************/
                   /*  Place code to prepare read buffer here */
                   /*******************************************/
                   /* `#START SW_PREPARE_READ_BUF`  */

                   /* `#END`  */

                   /* Prepare next opeation to read, Get data and place in data register */
                   if(`$INSTANCE_NAME`_readBufIndex < `$INSTANCE_NAME`_readBufSize)  
                   {
                        `$INSTANCE_NAME`_DATA = `$INSTANCE_NAME`_readBufPtr[`$INSTANCE_NAME`_readBufIndex];   /* Load first data byte  */
                        `$INSTANCE_NAME`_ACK_AND_TRANSMIT;
                        `$INSTANCE_NAME`_readBufIndex++;                                                      /* Advance to data location */
                        `$INSTANCE_NAME`_slStatus |= `$INSTANCE_NAME`_SSTAT_RD_BUSY;                          /* Set Read activity */
                   }
                   else   /* Data overflow */
                   {
                        `$INSTANCE_NAME`_DATA = 0xFF;    /* Out of range, send 0xFF  */
                        `$INSTANCE_NAME`_ACK_AND_TRANSMIT;
                        `$INSTANCE_NAME`_slStatus  |= `$INSTANCE_NAME`_SSTAT_RD_BUSY | `$INSTANCE_NAME`_SSTAT_RD_ERR_OVFL; /* Set Read activity */
                   }
                   `$INSTANCE_NAME`_State = `$INSTANCE_NAME`_SM_SL_RD_DATA;                                /* Prepare for read transaction */
               }
               else  /* Start of a Write transaction, reset pointers, first byte is address */
               {
                    /* Prepare next opeation to write offset */
                    `$INSTANCE_NAME`_ACK_AND_RECIVE;
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
                `$INSTANCE_NAME`_NAK_AND_RECIVE;
            }

            #else  /******************* Hardware address detection ************************/

            if(`$INSTANCE_NAME`_DATA & `$INSTANCE_NAME`_READ_FLAG)  /* Check for read or write command */
            {
                /*******************************************/
                /*  Place code to prepare read buffer here */
                /*******************************************/
                /* `#START SW_PREPARE_READ_BUF`  */

                /* `#END`  */

                /* Prepare next opeation to read, Get data and place in data register */
                if(`$INSTANCE_NAME`_readBufIndex < `$INSTANCE_NAME`_readBufSize)  
                {
                    `$INSTANCE_NAME`_DATA = `$INSTANCE_NAME`_readBufPtr[`$INSTANCE_NAME`_readBufIndex];   /* Load first data byte  */
                    `$INSTANCE_NAME`_ACK_AND_TRANSMIT;  /* ACK and transmit */
                    `$INSTANCE_NAME`_readBufIndex++;                                                      /* Advance to data location */
                    `$INSTANCE_NAME`_slStatus  |= `$INSTANCE_NAME`_SSTAT_RD_BUSY;                         /* Set Read activity */
                }
                else   /* Data overflow */
                {
                    `$INSTANCE_NAME`_DATA = 0xFF;    /* Out of range, send 0xFF  */
                    /* Special case: udb needs to ack, ff needs to nak. */
                    `$INSTANCE_NAME`_ACKNAK_AND_TRANSMIT;  /* NAK and transmit */
                    `$INSTANCE_NAME`_slStatus  |= `$INSTANCE_NAME`_SSTAT_RD_BUSY | `$INSTANCE_NAME`_SSTAT_RD_ERR_OVFL; /* Set Read activity */
                }
                
                `$INSTANCE_NAME`_State = `$INSTANCE_NAME`_SM_SL_RD_DATA;                                /* Prepare for read transaction */
            }
            else  /* Start of a Write transaction, reset pointers, first byte is address */
            {
                /* Prepare next opeation to write offset */
                `$INSTANCE_NAME`_ACK_AND_RECIVE; /* ACK and ready to receive addr */
                `$INSTANCE_NAME`_slStatus |= `$INSTANCE_NAME`_SSTAT_WR_BUSY;       /* Set Write activity */
                `$INSTANCE_NAME`_State     = `$INSTANCE_NAME`_SM_SL_WR_DATA;       /* Prepare for read transaction */
                `$INSTANCE_NAME`_ENABLE_INT_ON_STOP;                               /* Enable interrupt on Stop */
            }
            #endif
        }

        else if ( tmpCsr & `$INSTANCE_NAME`_CSR_BYTE_COMPLETE )                          /* Check for data transfer */
        {

            if (`$INSTANCE_NAME`_State == `$INSTANCE_NAME`_SM_SL_WR_DATA)                /* Data write from Master to Slave. */
            {
                if(`$INSTANCE_NAME`_writeBufIndex < `$INSTANCE_NAME`_writeBufSize)       /* Check for valid range */
                {
                    tmp8 = `$INSTANCE_NAME`_DATA;                                        /* Get data, to ACK quickly */
                    `$INSTANCE_NAME`_ACK_AND_RECIVE; /* ACK and ready to receive sub addr */
                    `$INSTANCE_NAME`_writeBufPtr[`$INSTANCE_NAME`_writeBufIndex] = tmp8; /* Write data to array */
                    `$INSTANCE_NAME`_writeBufIndex++;                                    /* Inc pointer */
                }
                else
                {
                    /* NAK cause beyond write area */
                    `$INSTANCE_NAME`_NAK_AND_RECIVE;
                    `$INSTANCE_NAME`_slStatus &= ~`$INSTANCE_NAME`_SSTAT_WR_BUSY;        /* Set Write activity */
                    `$INSTANCE_NAME`_slStatus |= (`$INSTANCE_NAME`_SSTAT_WR_CMPT | `$INSTANCE_NAME`_SSTAT_WR_ERR_OVFL);   /* Set Write activity */
                }

            }
            else if (`$INSTANCE_NAME`_State == `$INSTANCE_NAME`_SM_SL_RD_DATA)           /* Data Read from Slave to Master */
            {

                if((tmpCsr & `$INSTANCE_NAME`_CSR_LRB) == `$INSTANCE_NAME`_CSR_LRB_ACK)  
                {
                    if(`$INSTANCE_NAME`_readBufIndex < `$INSTANCE_NAME`_readBufSize) 
                    {
                        `$INSTANCE_NAME`_DATA = `$INSTANCE_NAME`_readBufPtr[`$INSTANCE_NAME`_readBufIndex]; /* Get data from array */
 
                        /* Send Data */
                        `$INSTANCE_NAME`_TRANSMIT_DATA;
                        `$INSTANCE_NAME`_readBufIndex++;                                  /* Inc pointer */
                    }
                    else   /* Over flow */
                    {
                        `$INSTANCE_NAME`_DATA = 0xFF;                                     /* Get data from array */

                        /* Send Data */
                        `$INSTANCE_NAME`_TRANSMIT_DATA;
                        `$INSTANCE_NAME`_slStatus  |= `$INSTANCE_NAME`_SSTAT_RD_ERR_OVFL; /* Set overflow */
                    }
                }
                else  /* Last byte NAKed, done */
                {
                    `$INSTANCE_NAME`_DATA = 0xFF;                                   /* Get data from array */

                    /* Send Data */
                    `$INSTANCE_NAME`_TRANSMIT_DATA;
                    `$INSTANCE_NAME`_slStatus &= ~`$INSTANCE_NAME`_SSTAT_RD_BUSY;   /* Clear Busy Flag */
                    `$INSTANCE_NAME`_slStatus |= `$INSTANCE_NAME`_SSTAT_RD_CMPT;    /* Set complete Flag */
                    `$INSTANCE_NAME`_State = `$INSTANCE_NAME`_SM_IDLE;              /* Return to idle state */
                }

            }
            else  /* This is an invalid state and should not occur  */
            {
                `$INSTANCE_NAME`_State = `$INSTANCE_NAME`_SM_IDLE;                 /* Invalid state, Reset */
                `$INSTANCE_NAME`_NAK_AND_RECIVE;
            }  /* End Transfer mode */

        }  
        else if ( tmpCsr & `$INSTANCE_NAME`_CSR_BUS_ERROR )                        /* Quick check for Error */
        {
            if( `$INSTANCE_NAME`_CSR & `$INSTANCE_NAME`_CSR_BUS_ERROR) 
            {
                /* May want to reset bus here CHECK */
            }
        }  /* end if */

        if( (`$INSTANCE_NAME`_CSR ) & `$INSTANCE_NAME`_CSR_STOP_STATUS)         /* Check if Stop was detected */
        {
            if(`$INSTANCE_NAME`_State == `$INSTANCE_NAME`_SM_SL_WR_DATA)
            {
                `$INSTANCE_NAME`_slStatus &= ~`$INSTANCE_NAME`_SSTAT_WR_BUSY;   /* Clear Busy Flag */
                `$INSTANCE_NAME`_slStatus |= `$INSTANCE_NAME`_SSTAT_WR_CMPT;    /* Set complete Flag */
                `$INSTANCE_NAME`_DISABLE_INT_ON_STOP;
            }
            `$INSTANCE_NAME`_State = `$INSTANCE_NAME`_SM_IDLE;                  /* Error or Stop, reset state */
        }
        #endif
    }
}
