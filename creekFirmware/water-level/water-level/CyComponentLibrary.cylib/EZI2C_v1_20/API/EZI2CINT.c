/*******************************************************************************
* File Name: `$INSTANCE_NAME`INT.c  
* Version `$CY_MAJOR_VERSION`.`$CY_MINOR_VERSION`
*
* Description:
*    This file contains the code that operates during the interrupt service
*    routine.  For this component, most of the runtime code is located in
*    the ISR.
*
* Note:
*
*******************************************************************************
* Copyright 2008-2009, Cypress Semiconductor Corporation.  All rights reserved.
* You may use this file only in accordance with the license, terms, conditions, 
* disclaimers, and limitations in the end user license agreement accompanying 
* the software package with which this file was provided.
********************************************************************************/




#include "`$INSTANCE_NAME`.h"  

/***************************************
*         System Variable          
***************************************/

uint8   `$INSTANCE_NAME`_State;              /* Current state of I2C state machine */
uint8   `$INSTANCE_NAME`_Status;             /* Status byte */
uint8 * `$INSTANCE_NAME`_DataPtr;            /* Pointer to data exposed to I2C Master */       

#if (`$INSTANCE_NAME`_SUBADDR_WIDTH == `$INSTANCE_NAME`_SUBADDR_8BIT)
    uint8   `$INSTANCE_NAME`_RwOffset1;      /* Offset for read and write operations, set at each write sequence */
    uint8   `$INSTANCE_NAME`_RwIndex1;       /* Points to next value to be read or written */
    uint8   `$INSTANCE_NAME`_WrProtect1;     /* Offset where data is read only */
    uint8   `$INSTANCE_NAME`_BufSize1;       /* Size of array between 1 and 255 */
#else
    uint16  `$INSTANCE_NAME`_RwOffset1;      /* Offset for read and write operations, set at each write sequence */
    uint16  `$INSTANCE_NAME`_RwIndex1;       /* Points to next value to be read or written */
    uint16  `$INSTANCE_NAME`_WrProtect1;     /* Offset where data is read only */
    uint16  `$INSTANCE_NAME`_BufSize1;       /* Size of array between 1 and 65535 */
#endif

/* If two slave addresses, creat second set of varaibles  */
#if (`$INSTANCE_NAME`_ADDRESSES == `$INSTANCE_NAME`_TWO_ADDRESSES)
    uint8 * `$INSTANCE_NAME`_DataPtr2;       /* Pointer to data exposed to I2C Master */       

    uint8   `$INSTANCE_NAME`_Address1;       /* Software address compare 1 */
    uint8   `$INSTANCE_NAME`_Address2;       /* Software address compare 2 */

    /* Select 8 or 16 bit secondary addresses */
    #if (`$INSTANCE_NAME`_SUBADDR_WIDTH == `$INSTANCE_NAME`_SUBADDR_8BIT)
        uint8   `$INSTANCE_NAME`_RwOffset2;  /* Offset for read and write operations, set at each write sequence */
        uint8   `$INSTANCE_NAME`_RwIndex2;   /* Points to next value to be read or written */
        uint8   `$INSTANCE_NAME`_WrProtect2; /* Offset where data is read only */
        uint8   `$INSTANCE_NAME`_BufSize2;   /* Size of array between 1 and 255 */
    #else
        uint16  `$INSTANCE_NAME`_RwOffset2;  /* Offset for read and write operations, set at each write sequence */
        uint16  `$INSTANCE_NAME`_RwIndex2;   /* Points to next value to be read or written */
        uint16  `$INSTANCE_NAME`_WrProtect2; /* Offset where data is read only */
        uint16  `$INSTANCE_NAME`_BufSize2;   /* Size of array between 1 and 65535 */
    #endif
#endif


/*******************************************************************************
* Function Name: `$INSTANCE_NAME`_ISR
********************************************************************************
* Summary:
*  Handle Interrupt Service Routine.  
*
* Parameters:  
*  void
*
* Return: 
*  void 
*
* Theory: 
*
* Side Effects:
*
*******************************************************************************/
CY_ISR( `$INSTANCE_NAME`_ISR )
{
    static uint8  tmp8;    /* Making these static so not wasting time allocating */
    static uint8  tmpCsr;  /* on the stack each time and no one else can see them */
    #if (`$INSTANCE_NAME`_SUBADDR_WIDTH == `$INSTANCE_NAME`_SUBADDR_16BIT)
        static uint16 tmp16;
    #endif

    /* Entry from interrupt */
    /* In hardware address compare mode, we can assume we only get interrupted when */
    /* a valid address is recognized.  In software address compare mode, we have to */
    /* check every address after a start condition.                                 */

    tmpCsr = `$INSTANCE_NAME`_CSR;            /* Make temp copy so that we can check */
                                              /* for stop condition after we are done */

    if(tmpCsr & `$INSTANCE_NAME`_CSR_ADDRESS) /* Check to see if a Start/Address is detected */
    {                                         /* This is a Start or ReStart  */
                                              /* So Reset the state machine  */
                                              /* Check for a Read/Write condition  */

        #if (`$INSTANCE_NAME`_ADDRESSES == `$INSTANCE_NAME`_TWO_ADDRESSES)
        tmp8 = ((`$INSTANCE_NAME`_Data >> 1) & 0x7Fu);
        if( tmp8 == `$INSTANCE_NAME`_Address1 )   /* Check for address 1  */
        {
            if(`$INSTANCE_NAME`_Data & `$INSTANCE_NAME`_READ_FLAG)
             {
                 /* Prepare next opeation to read, Get data and place in data register */
                `$INSTANCE_NAME`_Data = `$INSTANCE_NAME`_DataPtr[`$INSTANCE_NAME`_RwOffset1];   /* Load first data byte  */
                `$INSTANCE_NAME`_CSR = (`$INSTANCE_NAME`_CSR_ACK | `$INSTANCE_NAME`_CSR_TRANSMIT); /* ACK and transmit */
                `$INSTANCE_NAME`_RwIndex1 = `$INSTANCE_NAME`_RwOffset1;                        /* Reset pointer to previous offset */
                `$INSTANCE_NAME`_RwIndex1++;                                                   /* Advance to data location */
                `$INSTANCE_NAME`_Status |= `$INSTANCE_NAME`_STATUS_RD1BUSY;                    /* Set Read activity */
                `$INSTANCE_NAME`_State = `$INSTANCE_NAME`_SM_DEV1_RD_DATA;                     /* Prepare for read transaction */
                /* Done, Return */
             }
             else  /* Start of a Write transaction, reset pointers, first byte is address */
             {
                 /* Prepare next opeation to write offset */
                `$INSTANCE_NAME`_CSR = `$INSTANCE_NAME`_CSR_ACK ;                 /* ACK and ready to receive addr */
                `$INSTANCE_NAME`_Status |= `$INSTANCE_NAME`_STATUS_WR1BUSY;       /* Set Write activity */
                `$INSTANCE_NAME`_State = `$INSTANCE_NAME`_SM_DEV1_WR_ADDR;        /* Prepare for read transaction */
                `$INSTANCE_NAME`_CFG  |= `$INSTANCE_NAME`_CFG_STOP_IE;            /* Enable interrupt on Stop */
                /* Done, Return */
             }
            /* Done, Return */
        }
        else if( tmp8 == `$INSTANCE_NAME`_Address2 )   /* Check for address 2  */
        {
            if(`$INSTANCE_NAME`_Data & `$INSTANCE_NAME`_READ_FLAG)
             {
                 /* Prepare next opeation to read, Get data and place in data register */
                `$INSTANCE_NAME`_Data = `$INSTANCE_NAME`_DataPtr2[`$INSTANCE_NAME`_RwOffset2];     /* Load first data byte  */
                `$INSTANCE_NAME`_CSR = (`$INSTANCE_NAME`_CSR_ACK | `$INSTANCE_NAME`_CSR_TRANSMIT); /* ACK and transmit */
                `$INSTANCE_NAME`_RwIndex2 = `$INSTANCE_NAME`_RwOffset2;                            /* Reset pointer to previous offset */
                `$INSTANCE_NAME`_RwIndex2++;                                                       /* Advance to data location */
                `$INSTANCE_NAME`_Status |= `$INSTANCE_NAME`_STATUS_RD2BUSY;                        /* Set Read activity */
                `$INSTANCE_NAME`_State = `$INSTANCE_NAME`_SM_DEV2_RD_DATA;                         /* Prepare for read transaction */
                /* Done, Return */
             }
             else  /* Start of a Write transaction, reset pointers, first byte is address */
             {
                 /* Prepare next opeation to write offset */
                `$INSTANCE_NAME`_CSR = `$INSTANCE_NAME`_CSR_ACK ;                /* ACK and ready to receive addr */
                `$INSTANCE_NAME`_Status |= `$INSTANCE_NAME`_STATUS_WR2BUSY;      /* Set Write activity */
                `$INSTANCE_NAME`_State = `$INSTANCE_NAME`_SM_DEV2_WR_ADDR;       /* Prepare for read transaction */
                `$INSTANCE_NAME`_CFG  |= `$INSTANCE_NAME`_CFG_STOP_IE;           /* Enable interrupt on Stop */
                /* Done, Return */
             }
        }
        else   /* No address match */
        {
            `$INSTANCE_NAME`_CSR = `$INSTANCE_NAME`_CSR_NAK ;                    /* NAK address Match  */
        }

#else 
         if(`$INSTANCE_NAME`_Data & `$INSTANCE_NAME`_READ_FLAG)
        {
             /* Prepare next opeation to read, Get data and place in data register */
            `$INSTANCE_NAME`_Data = `$INSTANCE_NAME`_DataPtr[`$INSTANCE_NAME`_RwOffset1];  /* Load first data byte  */
            `$INSTANCE_NAME`_CSR = (`$INSTANCE_NAME`_CSR_ACK | `$INSTANCE_NAME`_CSR_TRANSMIT); /* ACK and transmit */
            `$INSTANCE_NAME`_RwIndex1 = `$INSTANCE_NAME`_RwOffset1;                        /* Reset pointer to previous offset */
            `$INSTANCE_NAME`_RwIndex1++;                                                   /* Advance to data location */
            `$INSTANCE_NAME`_Status |= `$INSTANCE_NAME`_STATUS_RD1BUSY;                    /* Set Read activity */
            `$INSTANCE_NAME`_State = `$INSTANCE_NAME`_SM_DEV1_RD_DATA;                     /* Prepare for read transaction */
            /* Done, Return */
        }
         else  /* Start of a Write transaction, reset pointers, first byte is address */
        {
             /* Prepare next opeation to write offset */
            `$INSTANCE_NAME`_CSR     = `$INSTANCE_NAME`_CSR_ACK ;         /* ACK and ready to receive addr */
            `$INSTANCE_NAME`_Status |= `$INSTANCE_NAME`_STATUS_WR1BUSY;   /* Set Write activity */
            `$INSTANCE_NAME`_State   = `$INSTANCE_NAME`_SM_DEV1_WR_ADDR;  /* Prepare for read transaction */
            `$INSTANCE_NAME`_CFG    |= `$INSTANCE_NAME`_CFG_STOP_IE;      /* Enable interrupt on Stop */
            /* Done, Return */
         }
        /* Done, Return */
#endif
    }  
    else if ( tmpCsr & `$INSTANCE_NAME`_CSR_BYTE_COMPLETE )               /* Check for data transfer */
    {
        switch(`$INSTANCE_NAME`_State)                                    /* Data transfer state machine */
        {
        case `$INSTANCE_NAME`_SM_DEV1_WR_ADDR:

            /* If 8-bit interface, Advance to WR_Data, else to ADDR2 */
            #if (`$INSTANCE_NAME`_SUBADDR_WIDTH == `$INSTANCE_NAME`_SUBADDR_8BIT)
            tmp8 = `$INSTANCE_NAME`_Data;
            if(tmp8 < `$INSTANCE_NAME`_BufSize1) 
            {
                `$INSTANCE_NAME`_CSR = `$INSTANCE_NAME`_CSR_ACK ;            /* ACK and ready to receive data */
                `$INSTANCE_NAME`_RwOffset1 = tmp8;                           /* Set offset to new value */
                `$INSTANCE_NAME`_RwIndex1 = tmp8;                            /* reset index to offset value */
                `$INSTANCE_NAME`_State = `$INSTANCE_NAME`_SM_DEV1_WR_DATA;   /* Prepare for write transaction */
                /* Done, Return */
            }
            else    /* Out of range, NAK data and don't set offset */
            {
                `$INSTANCE_NAME`_CSR = `$INSTANCE_NAME`_CSR_NAK;             /* NAK the master */
                /* Should the state be change here? Or will next byte be offset? */
                /* Done, Return */
            }
            #else
            tmp16 = `$INSTANCE_NAME`_Data;
            `$INSTANCE_NAME`_CSR = `$INSTANCE_NAME`_CSR_ACK ;               /* ACK and ready to receive addr */
            `$INSTANCE_NAME`_State = `$INSTANCE_NAME`_SM_DEV1_WR_ADDR_LSB;  /* Prepare to get LSB of Addr */
            #endif
            break;

        #if (`$INSTANCE_NAME`_SUBADDR_WIDTH == `$INSTANCE_NAME`_SUBADDR_16BIT)
        case `$INSTANCE_NAME`_SM_DEV1_WR_ADDR_LSB:                          /* Only used with 16-bit interface */
            tmp16 = (tmp16 << 8) | `$INSTANCE_NAME`_Data;                   /* Create offset */
            if(tmp16 < `$INSTANCE_NAME`_BufSize1) 
            {
                `$INSTANCE_NAME`_CSR = `$INSTANCE_NAME`_CSR_ACK ;           /* ACK and ready to receive addr */
                `$INSTANCE_NAME`_RwOffset1 = tmp16;                         /* Set offset to new value */
                `$INSTANCE_NAME`_RwIndex1 = tmp16;                          /* reset index to offset value */
                `$INSTANCE_NAME`_State = `$INSTANCE_NAME`_SM_DEV1_WR_DATA;  /* Prepare for write transaction */
                /* Done, Return */
            }
            else    /* Out of range, NAK data and don't set offset */
            {
                `$INSTANCE_NAME`_CSR = `$INSTANCE_NAME`_CSR_NAK;            /* NAK the master */
                /* Should the state be change here? Or will next byte be offset? */
                /* Done, Return */
            }
            break;
        #endif

        case `$INSTANCE_NAME`_SM_DEV1_WR_DATA:                             /* Data written from Master to Slave. */
            if(`$INSTANCE_NAME`_RwIndex1 < `$INSTANCE_NAME`_WrProtect1)    /* Check for valid range */
            {
                tmp8 = `$INSTANCE_NAME`_Data;                               /* Get data, to ACK quickly */
                `$INSTANCE_NAME`_CSR = `$INSTANCE_NAME`_CSR_ACK ;           /* ACK and ready to receive sub addr */
                `$INSTANCE_NAME`_DataPtr[`$INSTANCE_NAME`_RwIndex1] = tmp8; /* Write data to array */
                `$INSTANCE_NAME`_RwIndex1++;                                /* Inc pointer */
            }
            else
            {
                `$INSTANCE_NAME`_CSR = `$INSTANCE_NAME`_CSR_NAK ;           /* NAK cause beyond write area */
            }
            break;

        case `$INSTANCE_NAME`_SM_DEV1_RD_DATA:                              /* Data read by Master from Slave */
            if(`$INSTANCE_NAME`_RwIndex1 < `$INSTANCE_NAME`_BufSize1)       /* Check for valid range */
            {
                if((tmpCsr & `$INSTANCE_NAME`_CSR_LRB ) == `$INSTANCE_NAME`_CSR_LRB_ACK )    /* Check ACK/NAK */
                {
                    `$INSTANCE_NAME`_Data = `$INSTANCE_NAME`_DataPtr[`$INSTANCE_NAME`_RwIndex1]; /* Get data from array */
                    `$INSTANCE_NAME`_CSR = `$INSTANCE_NAME`_CSR_TRANSMIT;   /* Send Data */
                    `$INSTANCE_NAME`_RwIndex1++;                            /* Inc pointer */
                }
                else
                {
                    /* Data was NAKed, don't load data */
                   `$INSTANCE_NAME`_Data = 0xFF;                              /* Send dummy data */
                   `$INSTANCE_NAME`_CSR = `$INSTANCE_NAME`_CSR_TRANSMIT;      /* Send Data */
                   `$INSTANCE_NAME`_Status &= ~`$INSTANCE_NAME`_STATUS_BUSY;  /* Clear Busy Flag */
                   `$INSTANCE_NAME`_State = `$INSTANCE_NAME`_SM_IDLE;         /* Error or Stop, reset state */
                }
            }
            else
            {
                `$INSTANCE_NAME`_Data = 0xFF;                               /* Out of range send FFs */
                `$INSTANCE_NAME`_CSR = `$INSTANCE_NAME`_CSR_TRANSMIT;       /* Send Data */
            }
            /* Done, Return */
            break;

         /* Second Device Address */
        #if (`$INSTANCE_NAME`_ADDRESSES == `$INSTANCE_NAME`_TWO_ADDRESSES)

        case `$INSTANCE_NAME`_SM_DEV2_WR_ADDR:

            /* If 8-bit interface, Advance to WR_Data, else to ADDR2 */
            #if (`$INSTANCE_NAME`_SUBADDR_WIDTH == `$INSTANCE_NAME`_SUBADDR_8BIT)
            tmp8 = `$INSTANCE_NAME`_Data;
            if(tmp8 < `$INSTANCE_NAME`_BufSize2) 
            {
                `$INSTANCE_NAME`_CSR = `$INSTANCE_NAME`_CSR_ACK ;           /* ACK and ready to receive addr */
                `$INSTANCE_NAME`_RwOffset2 = tmp8;                          /* Set offset to new value */
                `$INSTANCE_NAME`_RwIndex2 = tmp8;                           /* reset index to offset value */
                `$INSTANCE_NAME`_State = `$INSTANCE_NAME`_SM_DEV2_WR_DATA;  /* Prepare for write transaction */
                /* Done, Return */
            }
            else    /* Out of range, NAK data and don't set offset */
            {
                `$INSTANCE_NAME`_CSR = `$INSTANCE_NAME`_CSR_NAK;            /* NAK the master */
                /* Should the state be change here? Or will next byte be offset? */
                /* Done, Return */
            }
            #else
            tmp16 = `$INSTANCE_NAME`_Data;
            `$INSTANCE_NAME`_CSR = `$INSTANCE_NAME`_CSR_ACK ;               /* ACK and ready to receive addr */
            `$INSTANCE_NAME`_State = `$INSTANCE_NAME`_SM_DEV2_WR_ADDR_LSB;  /* Prepare to get LSB of Addr */
            #endif
            break;

        #if (`$INSTANCE_NAME`_SUBADDR_WIDTH == `$INSTANCE_NAME`_SUBADDR_16BIT)
        case `$INSTANCE_NAME`_SM_DEV2_WR_ADDR_LSB:                          /* Only used with 16-bit interface */
            tmp16 = (tmp16 << 8) | `$INSTANCE_NAME`_Data;                   /* Create offset */
            if(tmp16 < `$INSTANCE_NAME`_BufSize2) 
            {
                `$INSTANCE_NAME`_CSR = `$INSTANCE_NAME`_CSR_ACK ;           /* ACK and ready to receive addr */
                `$INSTANCE_NAME`_RwOffset2 = tmp16;                         /* Set offset to new value */
                `$INSTANCE_NAME`_RwIndex2 = tmp16;                          /* reset index to offset value */
                `$INSTANCE_NAME`_State = `$INSTANCE_NAME`_SM_DEV2_WR_DATA;  /* Prepare for write transaction */
                /* Done, Return */
            }
            else    /* Out of range, NAK data and don't set offset */
            {
                `$INSTANCE_NAME`_CSR = `$INSTANCE_NAME`_CSR_NAK;            /* NAK the master */
                /* Should the state be change here? Or will next byte be offset? */
                /* Done, Return */
            }
            break;
        #endif   /* End 16-bit sub addressing */

        case `$INSTANCE_NAME`_SM_DEV2_WR_DATA:                              /* Data written from Master to Slave. */
            if(`$INSTANCE_NAME`_RwIndex2 < `$INSTANCE_NAME`_WrProtect2)     /* Check for valid range */
            {
                tmp8 = `$INSTANCE_NAME`_Data;                               /* Get data, to ACK quickly */
                `$INSTANCE_NAME`_CSR = `$INSTANCE_NAME`_CSR_ACK ;           /* ACK and ready to receive sub addr */
                `$INSTANCE_NAME`_DataPtr2[`$INSTANCE_NAME`_RwIndex2] = tmp8; /* Write data to array */
                `$INSTANCE_NAME`_RwIndex2++;                                /* Inc pointer */
            }
            else
            {
                `$INSTANCE_NAME`_CSR = `$INSTANCE_NAME`_CSR_NAK ;           /* NAK cause beyond write area */
            }
            break;

        case `$INSTANCE_NAME`_SM_DEV2_RD_DATA:                              /* Data read by Master from Slave */
            if(`$INSTANCE_NAME`_RwIndex2 < `$INSTANCE_NAME`_BufSize2)       /* Check for valid range */
            {
               if((tmpCsr & `$INSTANCE_NAME`_CSR_LRB ) == `$INSTANCE_NAME`_CSR_LRB_ACK )    /* Check ACK/NAK */
                {
                    `$INSTANCE_NAME`_Data = `$INSTANCE_NAME`_DataPtr2[`$INSTANCE_NAME`_RwIndex2]; /* Get data from array */
                    `$INSTANCE_NAME`_CSR = `$INSTANCE_NAME`_CSR_TRANSMIT;       /* Send Data */
                    `$INSTANCE_NAME`_RwIndex2++;                                /* Inc pointer */
                }
                else
                {
                    `$INSTANCE_NAME`_Data = 0xFF;                               /* Out of range send FFs */
                    `$INSTANCE_NAME`_CSR = `$INSTANCE_NAME`_CSR_TRANSMIT;       /* Send Data */
                    `$INSTANCE_NAME`_Status &= ~`$INSTANCE_NAME`_STATUS_BUSY;   /* Clear Busy Flag */
                    `$INSTANCE_NAME`_State = `$INSTANCE_NAME`_SM_IDLE;          /* Error or Stop, reset state */
                }
            }
            else
            {
                `$INSTANCE_NAME`_Data = 0xFF;                               /* Out of range send FFs */
                `$INSTANCE_NAME`_CSR = `$INSTANCE_NAME`_CSR_TRANSMIT;       /* Send Data */
            }
            /* Done, Return */
            break;

        #endif  /* End Second Device Address */

        default:
            `$INSTANCE_NAME`_State = `$INSTANCE_NAME`_SM_IDLE;            /* Invalid state, Reset */
            `$INSTANCE_NAME`_RwOffset1 = 0;                               /* Reset offsets and index */
            `$INSTANCE_NAME`_RwIndex1 = 0;             
            `$INSTANCE_NAME`_CSR = `$INSTANCE_NAME`_CSR_NAK ;             /* Dummy NAK to release bus */
            break;
        }  /* End switch/case */

//        `$INSTANCE_NAME`_Status |= `$INSTANCE_NAME`_STATUS_BUSY;          /* Set Busy flag */
    }  
    else if ( tmpCsr & `$INSTANCE_NAME`_CSR_BUS_ERROR )                   /* Quick check for Error */
    {
        if( `$INSTANCE_NAME`_CSR & `$INSTANCE_NAME`_CSR_BUS_ERROR) 
        {
     //       `$INSTANCE_NAME`_CSR = `$INSTANCE_NAME`_CSR_NAK ;             /* Dummy NAK to release bus */
     //       `$INSTANCE_NAME`_State = `$INSTANCE_NAME`_SM_IDLE;            /* Error or Stop, reset state */
     //       `$INSTANCE_NAME`_RwIndex1 = `$INSTANCE_NAME`_RwOffset1 = 0;   /* Reset offsets and index */
            /* May want to reset bus here CHECK */
        }
    }  /* end if */

    if( `$INSTANCE_NAME`_CSR  & `$INSTANCE_NAME`_CSR_STOP_STATUS) /* Check if Stop was detected */
    {
        `$INSTANCE_NAME`_Status &= ~`$INSTANCE_NAME`_STATUS_BUSY;          /* Clear Busy Flag */
        `$INSTANCE_NAME`_State   = `$INSTANCE_NAME`_SM_IDLE;               /* Error or Stop, reset state */
        `$INSTANCE_NAME`_CFG    &=  ~`$INSTANCE_NAME`_CFG_STOP_IE;         /* Disable interrupt on Stop */
    }
    
    #if(CYDEV_CHIP_DIE_EXPECT == CYDEV_CHIP_DIE_LEOPARD)     
        #if((CYDEV_CHIP_REV_EXPECT <= CYDEV_CHIP_REV_LEOPARD_ES2) && (`$INSTANCE_NAME`_isr__ES2_PATCH))                  
            `$INSTANCE_NAME`_ISR_PATCH();
        #endif
    #endif
    
}

/* [] END OF FILE */

