/*******************************************************************************
* File Name: i2cINT.c
* Version 1.70
*
* Description:
*  This file contains the code that operates during the interrupt service
*  routine.  For this component, most of the runtime code is located in
*  the ISR.
*
*******************************************************************************
* Copyright 2008-2012, Cypress Semiconductor Corporation.  All rights reserved.
* You may use this file only in accordance with the license, terms, conditions,
* disclaimers, and limitations in the end user license agreement accompanying
* the software package with which this file was provided.
*******************************************************************************/

#include "i2c.h"


/***************************************
*         System Variable
***************************************/

/* Current state of I2C state machine */
volatile uint8   i2c_curState;

/* Status byte */
volatile uint8   i2c_curStatus;

/* Pointer to data exposed to I2C Master */
volatile uint8 * i2c_dataPtrS1;

#if(i2c_SUBADDR_WIDTH == i2c_SUBADDR_8BIT)

    /* Offset for read and write operations, set at each write sequence */
    volatile uint8   i2c_rwOffsetS1;

    /* Points to next value to be read or written */
    volatile uint8   i2c_rwIndexS1;

    /* Offset where data is read only */
    volatile uint8   i2c_wrProtectS1;

    /* Size of array between 1 and 255 */
    volatile uint8   i2c_bufSizeS1;

#else   /* 16 bit sub-address */

    /* Offset for read and write operations, set at each write sequence */
    volatile uint16  i2c_rwOffsetS1;

    /* Points to next value to be read or written */
    volatile uint16  i2c_rwIndexS1;

    /* Offset where data is read only */
    volatile uint16  i2c_wrProtectS1;

    /* Size of array between 1 and 65535 */
    volatile uint16  i2c_bufSizeS1;

#endif  /* (i2c_SUBADDR_WIDTH == i2c_SUBADDR_8BIT) */

/* If two slave addresses, creat second set of varaibles  */
#if(i2c_ADDRESSES == i2c_TWO_ADDRESSES)

    /* Pointer to data exposed to I2C Master */
    volatile uint8 * i2c_dataPtrS2;

    /* Software address compare 1 */
    volatile uint8   i2c_addrS1;

    /* Software address compare 2 */
    volatile uint8   i2c_addrS2;

    /* Select 8 or 16 bit secondary addresses */
    #if(i2c_SUBADDR_WIDTH == i2c_SUBADDR_8BIT)

        /* Offset for read and write operations, set at each write sequence */
        volatile uint8   i2c_rwOffsetS2;

        /* Points to next value to be read or written */
        volatile uint8   i2c_rwIndexS2;

        /* Offset where data is read only */
        volatile uint8   i2c_wrProtectS2;

        /* Size of array between 1 and 255 */
        volatile uint8   i2c_bufSizeS2;

    #else   /* 16 bit sub-address */

        /* Offset for read and write operations, set at each write sequence */
        volatile uint16  i2c_rwOffsetS2;

        /* Points to next value to be read or written */
        volatile uint16  i2c_rwIndexS2;

        /* Offset where data is read only */
        volatile uint16  i2c_wrProtectS2;

        /* Size of array between 1 and 65535 */
        volatile uint16  i2c_bufSizeS2;

    #endif  /* (i2c_SUBADDR_WIDTH == i2c_SUBADDR_8BIT) */

#endif  /* (i2c_ADDRESSES == i2c_TWO_ADDRESSES) */


/*******************************************************************************
* Function Name: i2c_ISR
********************************************************************************
*
* Summary:
*  Handle Interrupt Service Routine.
*
* Parameters:
*  i2c_dataPtrS1: global variable, which stores pointer to the
*  data exposed to an I2C master for the first slave address.
*
*  i2c_rwOffsetS1: global variable, which stores offset for read
*  and write operations, is set at each write sequence of the first slave
*  address.
*
*  i2c_rwIndexS1: global variable, which stores pointer to the next
*  value to be read or written for the first slave address.
*
* i2c_wrProtectS1: global variable, which stores offset where data
*  is read only for the first slave address.
*
* i2c_bufSizeS1: global variable, which stores size of data array
*  exposed to an I2C master for the first slave address.
*
*  i2c_dataPtrS2: global variable, which stores pointer to the
*  data exposed to an I2C master for the second slave address.
*
*  i2c_rwOffsetS2: global variable, which stores offset for read
*  and write operations, is set at each write sequence of the second slave
*  device.
*
*  i2c_rwIndexS2: global variable, which stores pointer to the next
*  value to be read or written for the second slave address.
*
* i2c_wrProtectS2: global variable, which stores offset where data
*  is read only for the second slave address.
*
* i2c_bufSizeS2: global variable, which stores size of data array
*  exposed to an I2C master for the second slave address.
*
* i2c_curState: global variable, which stores current state of an
*  I2C state machine.
*
*  i2c_curStatus: global variable, which stores current status of
*  the component.
*
* Return:
*  i2c_rwOffsetS1: global variable, which stores offset for read
*  and write operations, is set at each write sequence of the first slave
*  address. Is reset if received slave address matches first slave address
*  and next operation will be read.
*
*  i2c_rwIndexS1: global variable, which stores pointer to the next
*  value to be read or written for the first slave address. Is set to
*  i2c_rwOffsetS1 and than incremented if received slave address
*  matches first slave address and next operation will be read.
*
*  i2c_rwOffsetS2: global variable, which stores offset for read
*  and write operations, is set at each write sequence of the second slave
*  address. This variable is changes if new sub-address is passed to slave.
*
*  i2c_rwIndexS2: global variable, which stores pointer to the next
*  value to be read or written for the second slave address. This variable is 
*  changes if new sub-address is passed to slave.
*
*******************************************************************************/
CY_ISR(i2c_ISR)
{
    /* Making these static so not wasting time allocating
    *  on the stack each time and no one else can see them 
    */
    static uint8  tmp8;
    static uint8  tmpCsr;

    #if(i2c_SUBADDR_WIDTH == i2c_SUBADDR_16BIT)
        static uint16 tmp16;
    #endif /* (i2c_SUBADDR_WIDTH == i2c_SUBADDR_16BIT) */

    /* Entry from interrupt
    *  In hardware address compare mode, we can assume we only get interrupted
    *  when a valid address is recognized. In software address compare mode,
    *  we have to check every address after a start condition.
    */

    /* Make a copy so that we can check for stop condition after we are done */
    tmpCsr = i2c_CSR_REG;
    
    /* Check to see if a Start/Address is detected - reset the state machine.
    *  Check for a Read/Write condition.
    */
    if(i2c_IS_BIT_SET(tmpCsr, i2c_CSR_ADDRESS)) 
    {
        #if(i2c_ADDRESSES == i2c_TWO_ADDRESSES)

            /* Get slave address from data register */
            tmp8 = ((i2c_DATA_REG >> i2c_ADDRESS_SHIFT) & i2c_SADDR_MASK);

            if(tmp8 == i2c_addrS1)   /* Check for address 1  */
            {
                if(i2c_IS_BIT_SET(i2c_DATA_REG, i2c_READ_FLAG))
                {  /* Prepare next read op, get data and place in register */

                    /* Load first data byte  */
                    i2c_DATA_REG = i2c_dataPtrS1[i2c_rwOffsetS1];

                    /* ACK and transmit */
                    i2c_CSR_REG = (i2c_CSR_ACK | i2c_CSR_TRANSMIT);

                    /* Set index to offset */
                    i2c_rwIndexS1 = i2c_rwOffsetS1;

                    /* Advance to data location */
                    i2c_rwIndexS1++;

                    /* Set Read busy status */
                    i2c_curStatus |= i2c_STATUS_RD1BUSY;

                    /* Prepare for read transaction */
                    i2c_curState = i2c_SM_DEV1_RD_DATA;
                }
                else  /* Start of a Write transaction, reset pointers, first byte is address */
                {  /* Prepare next opeation to write offset */

                    /* ACK and ready to receive sub address */
                    i2c_CSR_REG = i2c_CSR_ACK;

                    /* Set Write busy status */
                    i2c_curStatus |= i2c_STATUS_WR1BUSY;

                    /* Prepare for read transaction */
                    i2c_curState = i2c_SM_DEV1_WR_ADDR;

                    /* Stop Interrupt Enable */
                    i2c_CFG_REG  |= i2c_CFG_STOP_IE;

                }  /* Prepared for the next Write transaction */
            }   /* Slave address #1 is processed */
            else if(tmp8 == i2c_addrS2)   /* Check for address 2  */
            {
                if(i2c_IS_BIT_SET(i2c_DATA_REG, i2c_READ_FLAG))
                {  /* Prepare next read op, get data and place in register */

                    /* Load first data byte  */
                    i2c_DATA_REG = i2c_dataPtrS2[i2c_rwOffsetS2];

                    /* ACK and transmit */
                    i2c_CSR_REG = (i2c_CSR_ACK | i2c_CSR_TRANSMIT);

                    /* Reset pointer to previous offset */
                    i2c_rwIndexS2 = i2c_rwOffsetS2;

                    /* Advance to data location */
                    i2c_rwIndexS2++;

                    /* Set read busy status */
                    i2c_curStatus |= i2c_STATUS_RD2BUSY;

                    /* Prepare for read transaction */
                    i2c_curState = i2c_SM_DEV2_RD_DATA;

                }  /* Prepared for the next Read transaction */
                else  /* Start of a write trans, reset ptrs, 1st byte is addr */
                {  /* Prepare next opeation to write offset */

                    /* ACK and ready to receive addr */
                    i2c_CSR_REG = i2c_CSR_ACK;

                    /* Set Write busy status */
                    i2c_curStatus |= i2c_STATUS_WR2BUSY;

                    /* Prepare for read transaction */
                    i2c_curState = i2c_SM_DEV2_WR_ADDR;

                    /* Enable interrupt on Stop */
                    i2c_CFG_REG  |= i2c_CFG_STOP_IE;
                } /* Prepared for the next Write transaction */
            }
            else   /* No address match */
            {   /* NAK address Match  */
                i2c_CSR_REG = i2c_CSR_NAK;
            }
        #else /* One slave address - hardware address matching */
            
            if(i2c_IS_BIT_SET(i2c_DATA_REG, i2c_READ_FLAG))
            {   /* Prepare next read op, get data and place in register */

                /* Load first data byte  */
                i2c_DATA_REG = i2c_dataPtrS1[i2c_rwOffsetS1];

                /* ACK and transmit */
                i2c_CSR_REG = (i2c_CSR_ACK | i2c_CSR_TRANSMIT);

                /* Reset pointer to previous offset */
                i2c_rwIndexS1 = i2c_rwOffsetS1;

                /* Advance to data location */
                i2c_rwIndexS1++;

                /* Set read busy status */
                i2c_curStatus |= i2c_STATUS_RD1BUSY;

                /* Prepare for read transaction */
                i2c_curState = i2c_SM_DEV1_RD_DATA;
            }
            else  /* Start of a write trans, reset ptrs, 1st byte is address */
            {   /* Prepare next opeation to write offset */

                /* ACK and ready to receive addr */
                i2c_CSR_REG = i2c_CSR_ACK;

                /* Set Write activity */
                i2c_curStatus |= i2c_STATUS_WR1BUSY;

                /* Prepare for read transaction */
                i2c_curState = i2c_SM_DEV1_WR_ADDR;

                /* Enable interrupt on stop */
                i2c_CFG_REG |= i2c_CFG_STOP_IE;
            }
        #endif  /* (i2c_ADDRESSES == i2c_TWO_ADDRESSES) */
    }
    else if(i2c_IS_BIT_SET(tmpCsr, i2c_CSR_BYTE_COMPLETE))
    {   /* Check for data transfer */
        
        /* Data transfer state machine */
        switch(i2c_curState)
        {
            /* Address written from Master to Slave. */
            case i2c_SM_DEV1_WR_ADDR:

                /* If 8-bit interface, Advance to WR_Data, else to ADDR2 */
                #if(i2c_SUBADDR_WIDTH == i2c_SUBADDR_8BIT)
                    
                    tmp8 = i2c_DATA_REG;
                    if(tmp8 < i2c_bufSizeS1)
                    {
                        /* ACK and ready to receive data */
                        i2c_CSR_REG = i2c_CSR_ACK;

                        /* Set offset to new value */
                        i2c_rwOffsetS1 = tmp8;

                        /* Reset index to offset value */
                        i2c_rwIndexS1 = tmp8;

                        /* Prepare for write transaction */
                        i2c_curState = i2c_SM_DEV1_WR_DATA;
                    }
                    else    /* Out of range, NAK data and don't set offset */
                    {
                        /* NAK the master */
                        i2c_CSR_REG = i2c_CSR_NAK;
                    }

                #else   /* 16-bit */

                    /* Save MSB of address */
                    tmp16 = i2c_DATA_REG;

                    /* ACK and ready to receive addr */
                    i2c_CSR_REG = i2c_CSR_ACK;

                    /* Prepare to get LSB of Addr */
                    i2c_curState = i2c_SM_DEV1_WR_ADDR_LSB;

                #endif  /* (i2c_SUBADDR_WIDTH == i2c_SUBADDR_8BIT) */

            break;  /* case i2c_SM_DEV1_WR_ADDR */

            #if(i2c_SUBADDR_WIDTH == i2c_SUBADDR_16BIT)

                /* Only used with 16-bit interface */
                case i2c_SM_DEV1_WR_ADDR_LSB:

                    /* Create offset */
                    tmp16 = (tmp16 << i2c_ADDRESS_LSB_SHIFT) | i2c_DATA_REG;

                    /* Check range */
                    if(tmp16 < i2c_bufSizeS1)
                    {
                        /* ACK and ready to receive addr */
                        i2c_CSR_REG = i2c_CSR_ACK;

                        /* Set offset to new value */
                        i2c_rwOffsetS1 = tmp16;

                        /* Reset index to offset value */
                        i2c_rwIndexS1 = tmp16;

                        /* Prepare for write transaction */
                        i2c_curState = i2c_SM_DEV1_WR_DATA;
                    }
                    else    /* Out of range, NAK data and don't set offset */
                    {
                        /* NAK the master */
                        i2c_CSR_REG = i2c_CSR_NAK;
                    }
                break; /* case i2c_SM_DEV1_WR_ADDR_LSB */

            #endif  /* (i2c_SUBADDR_WIDTH == i2c_SUBADDR_16BIT) */


            /* Data written from Master to Slave. */
            case i2c_SM_DEV1_WR_DATA:
                
                /* Check for valid range */
                if(i2c_rwIndexS1 < i2c_wrProtectS1)
                {
                    /* Get data, to ACK quickly */
                    tmp8 = i2c_DATA_REG;

                    /* ACK and ready to receive sub addr */
                    i2c_CSR_REG = i2c_CSR_ACK;

                    /* Write data to array */
                    i2c_dataPtrS1[i2c_rwIndexS1] = tmp8;

                    /* Increment pointer */
                    i2c_rwIndexS1++;
                }
                else
                {
                    /* NAK cause beyond write area */
                    i2c_CSR_REG = i2c_CSR_NAK;
                }
            break;  /* i2c_SM_DEV1_WR_DATA */


            /* Data read by Master from Slave */
            case i2c_SM_DEV1_RD_DATA:

                /* Check for valid range */
                if(i2c_rwIndexS1 < i2c_bufSizeS1)
                {   /* Check ACK/NAK */
                    if((tmpCsr & i2c_CSR_LRB) == i2c_CSR_LRB_ACK)
                    {

                        /* Get data from array */
                        i2c_DATA_REG = i2c_dataPtrS1[i2c_rwIndexS1];

                        /* Send Data */
                        i2c_CSR_REG = i2c_CSR_TRANSMIT;

                        /* Increment pointer */
                        i2c_rwIndexS1++;
                    }
                    else    /* Data was NAKed */
                    {

                        /* Send dummy data at the end of read transaction */
                        i2c_DATA_REG = i2c_DUMMY_DATA;

                        /* Clear transmit bit at the end of read transaction */
                        i2c_CSR_REG = i2c_CSR_NAK;
                        
                        /* Clear Busy Flag */
                        i2c_curStatus &= ~i2c_STATUS_BUSY;

                        /* Error or Stop, reset state */
                        i2c_curState = i2c_SM_IDLE;

                    }
                }
                else    /* No valid range */
                {
                    /* Out of range send FFs */
                    i2c_DATA_REG = i2c_DUMMY_DATA;

                    /* Send Data */
                    i2c_CSR_REG = i2c_CSR_TRANSMIT;
                }
            break;  /* i2c_SM_DEV1_RD_DATA */

            /* Second Device Address */
            #if(i2c_ADDRESSES == i2c_TWO_ADDRESSES)

                case i2c_SM_DEV2_WR_ADDR:

                    /* If 8-bit interface, Advance to WR_Data, else to ADDR2 */
                    #if(i2c_SUBADDR_WIDTH == i2c_SUBADDR_8BIT)
                        
                        tmp8 = i2c_DATA_REG;
                        if(tmp8 < i2c_bufSizeS2)
                        {
                            /* ACK and ready to receive addr */
                            i2c_CSR_REG = i2c_CSR_ACK;

                            /* Set offset to new value */
                            i2c_rwOffsetS2 = tmp8;

                            /* Reset index to offset value */
                            i2c_rwIndexS2 = tmp8;

                            /* Prepare for write transaction */
                            i2c_curState = i2c_SM_DEV2_WR_DATA;
                        }
                        else    /* Out of range, NAK data and don't set offset */
                        {
                            /* NAK the master */
                            i2c_CSR_REG = i2c_CSR_NAK;
                        }
                    #else
                        /* Save LSB of address */
                        tmp16 = i2c_DATA_REG;

                        /* ACK and ready to receive addr */
                        i2c_CSR_REG = i2c_CSR_ACK;

                        /* Prepare to get LSB of Addr */
                        i2c_curState = i2c_SM_DEV2_WR_ADDR_LSB;
                    #endif  /* (i2c_SUBADDR_WIDTH == i2c_SUBADDR_8BIT) */

                break;  /* i2c_SM_DEV2_WR_ADDR */

                #if(i2c_SUBADDR_WIDTH == i2c_SUBADDR_16BIT)

                    /* Only used with 16-bit interface */
                    case i2c_SM_DEV2_WR_ADDR_LSB:
                        /* Create offset */
                        tmp16 = (tmp16 << 8u) | i2c_DATA_REG;
                        if(tmp16 < i2c_bufSizeS2)
                        {
                            /* ACK and ready to receive addr */
                            i2c_CSR_REG = i2c_CSR_ACK;

                            /* Set offset to new value */
                            i2c_rwOffsetS2 = tmp16;

                            /* Reset index to offset value */
                            i2c_rwIndexS2 = tmp16;

                            /* Prepare for write transaction */
                            i2c_curState = i2c_SM_DEV2_WR_DATA;
                        }
                        else    /* Out of range, NAK data and don't set offset */
                        {
                            /* NAK the master */
                            i2c_CSR_REG = i2c_CSR_NAK;
                        }
                        break; /* i2c_SM_DEV2_WR_ADDR_LSB */

                #endif   /* (i2c_SUBADDR_WIDTH == i2c_SUBADDR_16BIT) */


                /* Data written from Master to Slave. */
                case i2c_SM_DEV2_WR_DATA:
                    
                    /* Check for valid range */
                    if(i2c_rwIndexS2 < i2c_wrProtectS2)
                    {
                        /* Get data, to ACK quickly */
                        tmp8 = i2c_DATA_REG;

                        /* ACK and ready to receive sub addr */
                        i2c_CSR_REG = i2c_CSR_ACK;

                        /* Write data to array */
                        i2c_dataPtrS2[i2c_rwIndexS2] = tmp8;

                        /* Inc pointer */
                        i2c_rwIndexS2++;
                    }
                    else
                    {
                        /* NAK cause beyond write area */
                        i2c_CSR_REG = i2c_CSR_NAK;
                    }
                    break;  /* i2c_SM_DEV2_WR_DATA */

                    /* Data read by Master from Slave */
                    case i2c_SM_DEV2_RD_DATA:
                    
                        /* Check for valid range */
                        if(i2c_rwIndexS2 < i2c_bufSizeS2)
                        {   /* Check ACK/NAK */
                            if((tmpCsr & i2c_CSR_LRB) == i2c_CSR_LRB_ACK)
                            {   /* ACKed */

                                /* Get data from array */
                                i2c_DATA_REG = i2c_dataPtrS2[i2c_rwIndexS2];

                                /* Send Data */
                                i2c_CSR_REG = i2c_CSR_TRANSMIT;

                                /* Increment pointer */
                                i2c_rwIndexS2++;
                            }
                            else    /* NAKed */
                            {
                                /* Out of range send FFs */
                                i2c_DATA_REG = i2c_DUMMY_DATA;

                                /* Send Data */
                                i2c_CSR_REG = i2c_CSR_TRANSMIT;

                                /* Clear busy status */
                                i2c_curStatus &= ~i2c_STATUS_BUSY;

                                /* Error or Stop, reset state */
                                i2c_curState = i2c_SM_IDLE;
                            }   /* End if ACK/NAK */
                        }
                        else    /* Not valid range */
                        {
                            /* Out of range send FFs */
                            i2c_DATA_REG = i2c_DUMMY_DATA;

                            /* Send Data */
                            i2c_CSR_REG = i2c_CSR_TRANSMIT;
                        }
                        break;  /* i2c_SM_DEV2_RD_DATA */

            #endif  /* (i2c_ADDRESSES == i2c_TWO_ADDRESSES) */

            default:
                /* Invalid state, Reset */
                i2c_curState = i2c_SM_IDLE;

                /* Reset offsets and index */
                i2c_rwOffsetS1 = 0u;

                /* Clear index */
                i2c_rwIndexS1 = 0u;

                /* Dummy NAK to release bus */
                i2c_CSR_REG = i2c_CSR_NAK;
                break;

        }  /* End switch/case i2c_curState */

    }   /* (tmpCsr & i2c_CSR_ADDRESS) */

    /* Check if Stop was detected */
    if(i2c_IS_BIT_SET(i2c_CSR_REG, i2c_CSR_STOP_STATUS))
    {
        /* Clear Busy Flag */
        i2c_curStatus &= ~i2c_STATUS_BUSY;

        /* error or stop - reset state */
        i2c_curState = i2c_SM_IDLE;

        /* Disable interrupt on Stop */
        i2c_CFG_REG &= ~i2c_CFG_STOP_IE;
    }
}


/* [] END OF FILE */
