/*******************************************************************************
* File Name: `$INSTANCE_NAME`.c
* Version `$CY_MAJOR_VERSION`.`$CY_MINOR_VERSION`
*
* Description:
*
*
* NOTE:
* 
*
*******************************************************************************
* Copyright 2008-2009, Cypress Semiconductor Corporation.  All rights reserved.
* You may use this file only in accordance with the license, terms, conditions, 
* disclaimers, and limitations in the end user license agreement accompanying 
* the software package with which this file was provided.
********************************************************************************/

/**************************************
* Slave Function Prototypes 
**************************************/

uint8 `$INSTANCE_NAME`_SlaveStatus(void);
uint8 `$INSTANCE_NAME`_SlaveClearReadStatus(void);  
uint8 `$INSTANCE_NAME`_SlaveClearWriteStatus(void); 
void `$INSTANCE_NAME`_SlaveSetAddress(uint8 address);
void `$INSTANCE_NAME`_SlaveSetSleepMode(void);
void `$INSTANCE_NAME`_SlaveSetWakeMode(void);

void `$INSTANCE_NAME`_SlaveInitReadBuf(uint8 * rdBuf, uint8 bufSize);
void `$INSTANCE_NAME`_SlaveInitWriteBuf(uint8 * wrBuf, uint8 bufSize);
uint8 `$INSTANCE_NAME`_SlaveGetReadBufSize(void);
uint8 `$INSTANCE_NAME`_SlaveGetWriteBufSize(void);
void `$INSTANCE_NAME`_SlaveClearReadBuf(void);
void `$INSTANCE_NAME`_SlaveClearWriteBuf(void); 

/* Manual operation commands */
void `$INSTANCE_NAME`_SlavePutReadByte(uint8 transmitDataByte); 
uint8 `$INSTANCE_NAME`_SlaveGetWriteByte(uint8 ackNak); 


/**************************************
 *  Registers
 *************************************/

#define `$INSTANCE_NAME`_CFG                        (*(reg8 *) `$INSTANCE_NAME`_B_I2CS_UDB_1_ctrlreg__CONTROL_REG)
#define `$INSTANCE_NAME`_CFG_PTR                    ((reg8 *) `$INSTANCE_NAME`_B_I2CS_UDB_1_ctrlreg__CONTROL_REG)

#define `$INSTANCE_NAME`_CSR                        (*(reg8 *) `$INSTANCE_NAME`_B_I2CS_UDB_1_stsreg__STATUS_REG)
#define `$INSTANCE_NAME`_CSR_PTR                    ((reg8 *) `$INSTANCE_NAME`_B_I2CS_UDB_1_stsreg__STATUS_REG)

#define `$INSTANCE_NAME`_INT_MASK                   (*(reg8 *) `$INSTANCE_NAME`_B_I2CS_UDB_1_stsreg__MASK_REG)
#define `$INSTANCE_NAME`_INT_MASK_PTR               ((reg8 *) `$INSTANCE_NAME`_B_I2CS_UDB_1_stsreg__MASK_REG)

#define `$INSTANCE_NAME`_INT_ENABLE                 (*(reg8 *) `$INSTANCE_NAME`_B_I2CS_UDB_1_stsreg__STATUS_AUX_CTL_REG)
#define `$INSTANCE_NAME`_INT_ENABLE_PTR             ((reg8 *) `$INSTANCE_NAME`_B_I2CS_UDB_1_stsreg__STATUS_AUX_CTL_REG)

#define `$INSTANCE_NAME`_ADDRESS                    (*(reg8 *) `$INSTANCE_NAME`_B_I2CS_UDB_1_shifter_u0__D0_REG)
#define `$INSTANCE_NAME`_ADDRESS_PTR                ((reg8 *) `$INSTANCE_NAME`_B_I2CS_UDB_1_shifter_u0__D0_REG)

#define `$INSTANCE_NAME`_DATA                       (*(reg8 *) `$INSTANCE_NAME`_B_I2CS_UDB_1_shifter_u0__A0_REG)
#define `$INSTANCE_NAME`_DATA_PTR                   ((reg8 *) `$INSTANCE_NAME`_B_I2CS_UDB_1_shifter_u0__A0_REG)

#define `$INSTANCE_NAME`_TX_DATA                    (*(reg8 *) `$INSTANCE_NAME`_B_I2CS_UDB_1_shifter_u0__A0_REG)
#define `$INSTANCE_NAME`_TX_DATA_PTR                ((reg8 *) `$INSTANCE_NAME`_B_I2CS_UDB_1_shifter_u0__A0_REG)

#define `$INSTANCE_NAME`_GO                         (*(reg8 *) `$INSTANCE_NAME`_B_I2CS_UDB_1_shifter_u0__F1_REG)
#define `$INSTANCE_NAME`_GO_PTR                     ((reg8 *) `$INSTANCE_NAME`_B_I2CS_UDB_1_shifter_u0__F1_REG)
                                                                          
#define `$INSTANCE_NAME`_COUNTER                    (*(reg8 *)  `$INSTANCE_NAME`_B_I2CS_UDB_1_counter__COUNT_REG)
#define `$INSTANCE_NAME`_COUNTER_PTR                ((reg8 *)  `$INSTANCE_NAME`_B_I2CS_UDB_1_counter__COUNT_REG)      
#define `$INSTANCE_NAME`_COUNTER_AUX_CTL            (*(reg8 *)  `$INSTANCE_NAME`_B_I2CS_UDB_1_counter__CONTROL_AUX_CTL_REG)
#define `$INSTANCE_NAME`_COUNTER_AUX_CTL_PTR        ((reg8 *)  `$INSTANCE_NAME`_B_I2CS_UDB_1_counter__CONTROL_AUX_CTL_REG) 

/********************************
*    Constants
********************************/
    /* Control Register Bit Locations */
#define `$INSTANCE_NAME`_CTRL_ENABLE_SHIFT          `$INSTANCE_NAME`_B_I2CS_UDB_1_ctrlreg__0__POS
#define `$INSTANCE_NAME`_CTRL_ENABLE_MASK           (0x01u << `$INSTANCE_NAME`_CTRL_ENABLE_SHIFT)

#define `$INSTANCE_NAME`_CTRL_NACK_SHIFT            `$INSTANCE_NAME`_B_I2CS_UDB_1_ctrlreg__2__POS
#define `$INSTANCE_NAME`_CTRL_NACK_MASK             (0x01u << `$INSTANCE_NAME`_CTRL_NACK_SHIFT)

#define `$INSTANCE_NAME`_CTRL_TRANSMIT_SHIFT        `$INSTANCE_NAME`_B_I2CS_UDB_1_ctrlreg__3__POS
#define `$INSTANCE_NAME`_CTRL_TRANSMIT_MASK         (0x01u << `$INSTANCE_NAME`_CTRL_TRANSMIT_SHIFT)

#define `$INSTANCE_NAME`_CTRL_ANY_ADDRESS_SHIFT     `$INSTANCE_NAME`_B_I2CS_UDB_1_ctrlreg__4__POS
#define `$INSTANCE_NAME`_CTRL_ANY_ADDRESS_MASK      (0x01u << `$INSTANCE_NAME`_CTRL_ANY_ADDRESS_SHIFT)

/* TX Status Register Bit Locations */
#define `$INSTANCE_NAME`_ADDR_SHIFT                 `$INSTANCE_NAME`_B_I2CS_UDB_1_stsreg__3__POS
#define `$INSTANCE_NAME`_ADDR_MASK                  (0x01u << `$INSTANCE_NAME`_ADDR_SHIFT)

#define `$INSTANCE_NAME`_STOP_SHIFT                 `$INSTANCE_NAME`_B_I2CS_UDB_1_stsreg__4__POS
#define `$INSTANCE_NAME`_STOP_MASK                  (0x01u << `$INSTANCE_NAME`_STOP_SHIFT)

#define `$INSTANCE_NAME`_BYTE_COMPLETE_SHIFT        `$INSTANCE_NAME`_B_I2CS_UDB_1_stsreg__0__POS
#define `$INSTANCE_NAME`_BYTE_COMPLETE_MASK         (0x01u << `$INSTANCE_NAME`_BYTE_COMPLETE_SHIFT)


/***************************************
*            API Constants        
***************************************/

/* Other master control constants */
#define `$INSTANCE_NAME`_READ_XFER_MODE             0x01u    /* Read */
#define `$INSTANCE_NAME`_WRITE_XFER_MODE            0x00u    /* Write */
#define `$INSTANCE_NAME`_ACK_DATA                   0x01u    /* Send ACK */
#define `$INSTANCE_NAME`_NAK_DATA                   0x00u    /* Send NAK */

/* Slave Status Constants */
#define `$INSTANCE_NAME`_SSTAT_RD_CMPT              0x01u    /* Read transfer complete */
#define `$INSTANCE_NAME`_SSTAT_RD_BUSY              0x02u    /* Read transfer in progress */
#define `$INSTANCE_NAME`_SSTAT_RD_ERR               0x08u    /* Read Error buffer */
#define `$INSTANCE_NAME`_SSTAT_RD_NO_ERR            0x00u    /* Read no Error */
#define `$INSTANCE_NAME`_SSTAT_RD_ERR_OVFL          0x04u    /* Read overflow Error */
#define `$INSTANCE_NAME`_SSTAT_RD_MASK              0x0Fu    /* Read Status Mask */
#define `$INSTANCE_NAME`_SSTAT_WR_CMPT              0x10u    /* Write transfer complete */
#define `$INSTANCE_NAME`_SSTAT_WR_BUSY              0x20u    /* Write transfer in progress */
#define `$INSTANCE_NAME`_SSTAT_WR_ERR               0xC0u    /* Write Error buffer */
#define `$INSTANCE_NAME`_SSTAT_WR_NO_ERR            0x00u    /* Write no Error */
#define `$INSTANCE_NAME`_SSTAT_WR_ERR_OVFL          0x40u    /* Write overflow Error */
#define `$INSTANCE_NAME`_SSTAT_WR_MASK              0xF0u    /* Write Status Mask  */
#define `$INSTANCE_NAME`_SSTAT_RD_CLEAR             0x0Du    /* Read Status clear */
#define `$INSTANCE_NAME`_SSTAT_WR_CLEAR             0xD0u    /* Write Status Clear */

#define `$INSTANCE_NAME`_COUNTER_ENABLE_MASK        0x20u
#define `$INSTANCE_NAME`_INT_ENABLE_MASK            0x10u

/********************************************/
/* Defines to make UDB match Fixed Function */  
/********************************************/
#define `$INSTANCE_NAME`_CSR_STOP_STATUS            `$INSTANCE_NAME`_STOP_MASK   /* Set if Stop has been detected */
#define `$INSTANCE_NAME`_CSR_ADDRESS                `$INSTANCE_NAME`_ADDR_MASK   /* Set in firmware 0 = status bit, 1 Address is slave */
#define `$INSTANCE_NAME`_CSR_BYTE_COMPLETE          `$INSTANCE_NAME`_BYTE_COMPLETE_MASK   /* Informs that last byte has been sent. */
#define `$INSTANCE_NAME`_CSR_TRANSMIT               `$INSTANCE_NAME`_CTRL_TRANSMIT_MASK   /* Set in firmware 1 = transmit, 0 = receive. */
#define `$INSTANCE_NAME`_CSR_BUS_ERROR              0x00u   /* Active high when bus error has occured */
#define `$INSTANCE_NAME`_CSR_LRB                    0x02u //`$INSTANCE_NAME`_CTRL_NACK_SHIFT   /* Last received bit. */
#define `$INSTANCE_NAME`_CSR_LRB_ACK                0x00u   /* Last received bit was an ACK */
#define `$INSTANCE_NAME`_READ_FLAG                  0x01u

#define `$INSTANCE_NAME`_DISABLE_INT_ON_STOP        {`$INSTANCE_NAME`_INT_MASK &= ~(`$INSTANCE_NAME`_STOP_MASK);}
#define `$INSTANCE_NAME`_ENABLE_INT_ON_STOP         {`$INSTANCE_NAME`_INT_MASK |= `$INSTANCE_NAME`_STOP_MASK;}

#define `$INSTANCE_NAME`_ACK_AND_TRANSMIT           {`$INSTANCE_NAME`_CFG &= ~(`$INSTANCE_NAME`_CTRL_NACK_MASK); \
                                                     `$INSTANCE_NAME`_CFG |= (`$INSTANCE_NAME`_CTRL_TRANSMIT_MASK); \
                                                     `$INSTANCE_NAME`_GO = 0x00;}

#define `$INSTANCE_NAME`_NAK_AND_TRANSMIT           {`$INSTANCE_NAME`_CFG |= (`$INSTANCE_NAME`_CTRL_NACK_MASK); \
                                                     `$INSTANCE_NAME`_CFG |= (`$INSTANCE_NAME`_CTRL_TRANSMIT_MASK); \
                                                     `$INSTANCE_NAME`_GO = 0x00;}

#define `$INSTANCE_NAME`_ACK_AND_RECIVE             {`$INSTANCE_NAME`_CFG &= (~(`$INSTANCE_NAME`_CTRL_TRANSMIT_MASK) & ~(`$INSTANCE_NAME`_CTRL_NACK_MASK)); \
                                                     `$INSTANCE_NAME`_GO = 0x00;}

#define `$INSTANCE_NAME`_NAK_AND_RECIVE             {`$INSTANCE_NAME`_CFG &= ~(`$INSTANCE_NAME`_CTRL_TRANSMIT_MASK); \
                                                     `$INSTANCE_NAME`_CFG |= `$INSTANCE_NAME`_CTRL_NACK_MASK; \
                                                     `$INSTANCE_NAME`_GO = 0x00;}

#define `$INSTANCE_NAME`_TRANSMIT_DATA              {`$INSTANCE_NAME`_CFG &= ~(`$INSTANCE_NAME`_CTRL_NACK_MASK); \
                                                     `$INSTANCE_NAME`_CFG |= (`$INSTANCE_NAME`_CTRL_TRANSMIT_MASK); \
                                                     `$INSTANCE_NAME`_GO = 0x00;}

/* Special case: udb needs to ack, ff needs to nak. */
#define `$INSTANCE_NAME`_ACKNAK_AND_TRANSMIT        {`$INSTANCE_NAME`_CFG &= ~(`$INSTANCE_NAME`_CTRL_NACK_MASK); \
                                                     `$INSTANCE_NAME`_CFG |= (`$INSTANCE_NAME`_CTRL_TRANSMIT_MASK); \
                                                     `$INSTANCE_NAME`_GO = 0x00;}






































