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

/***************************************
*      Master Function Prototypes 
***************************************/

uint8   `$INSTANCE_NAME`_MasterStatus(void);
uint8   `$INSTANCE_NAME`_MasterClearStatus(void);

uint8   `$INSTANCE_NAME`_MasterSendStart(uint8 SlaveAddress, uint8 R_nW);
uint8   `$INSTANCE_NAME`_MasterSendRestart(uint8 SlaveAddress, uint8 R_nW);
uint8   `$INSTANCE_NAME`_MasterSendStop(void);
uint8   `$INSTANCE_NAME`_MasterWriteBuf(uint8 SlaveAddr, uint8 * wrData, uint8 cnt, uint8 mode);
uint8   `$INSTANCE_NAME`_MasterReadBuf(uint8 SlaveAddr, uint8 * rdData, uint8 cnt, uint8 mode);
uint8   `$INSTANCE_NAME`_MasterWriteByte(uint8 theByte);
uint8   `$INSTANCE_NAME`_MasterReadByte(uint8 acknNak);

uint16  `$INSTANCE_NAME`_MasterGetReadBufSize(void);
uint16  `$INSTANCE_NAME`_MasterGetWriteBufSize(void);
void    `$INSTANCE_NAME`_MasterClearReadBuf(void);
void    `$INSTANCE_NAME`_MasterClearWriteBuf(void);


/***************************************
*             Registers
*************************************/

#define `$INSTANCE_NAME`_CFG                        (*(reg8 *) `$INSTANCE_NAME`_B_I2CM_UDB_1_ctrlreg__CONTROL_REG)
#define `$INSTANCE_NAME`_CFG_PTR                    ((reg8 *) `$INSTANCE_NAME`_B_I2CM_UDB_1_ctrlreg__CONTROL_REG)

#define `$INSTANCE_NAME`_CSR                        (*(reg8 *) `$INSTANCE_NAME`_B_I2CM_UDB_1_stsreg__STATUS_REG)
#define `$INSTANCE_NAME`_CSR_PTR                    ((reg8 *) `$INSTANCE_NAME`_B_I2CM_UDB_1_stsreg__STATUS_REG)

#define `$INSTANCE_NAME`_INT_MASK                   (*(reg8 *) `$INSTANCE_NAME`_B_I2CM_UDB_1_stsreg__MASK_REG)
#define `$INSTANCE_NAME`_INT_MASK_PTR               ((reg8 *) `$INSTANCE_NAME`_B_I2CM_UDB_1_stsreg__MASK_REG)

#define `$INSTANCE_NAME`_INT_ENABLE                 (*(reg8 *) `$INSTANCE_NAME`_B_I2CM_UDB_1_stsreg__STATUS_AUX_CTL_REG)
#define `$INSTANCE_NAME`_INT_ENABLE_PTR             ((reg8 *) `$INSTANCE_NAME`_B_I2CM_UDB_1_stsreg__STATUS_AUX_CTL_REG)

#define `$INSTANCE_NAME`_DATA                       (*(reg8 *) `$INSTANCE_NAME`_B_I2CM_UDB_1_shifter_u0__A0_REG)
#define `$INSTANCE_NAME`_DATA_PTR                   ((reg8 *) `$INSTANCE_NAME`_B_I2CM_UDB_1_shifter_u0__A0_REG)

#define `$INSTANCE_NAME`_GO                         (*(reg8 *) `$INSTANCE_NAME`_B_I2CM_UDB_1_shifter_u0__F1_REG)
#define `$INSTANCE_NAME`_GO_PTR                     ((reg8 *) `$INSTANCE_NAME`_B_I2CM_UDB_1_shifter_u0__F1_REG)


/***************************************
*             Constants
***************************************/

/* Control Register Bit Locations */
#define `$INSTANCE_NAME`_CTRL_ENABLE_SHIFT          `$INSTANCE_NAME`_B_I2CM_UDB_1_ctrlreg__1__POS
#define `$INSTANCE_NAME`_CTRL_ENABLE_MASK           (0x01u << `$INSTANCE_NAME`_CTRL_ENABLE_SHIFT)

#define `$INSTANCE_NAME`_CTRL_NACK_SHIFT            `$INSTANCE_NAME`_B_I2CM_UDB_1_ctrlreg__2__POS
#define `$INSTANCE_NAME`_CTRL_NACK_MASK             (0x01u << `$INSTANCE_NAME`_CTRL_NACK_SHIFT)

#define `$INSTANCE_NAME`_CTRL_TRANSMIT_SHIFT        `$INSTANCE_NAME`_B_I2CM_UDB_1_ctrlreg__3__POS
#define `$INSTANCE_NAME`_CTRL_TRANSMIT_MASK         (0x01u << `$INSTANCE_NAME`_CTRL_TRANSMIT_SHIFT)

#define `$INSTANCE_NAME`_CTRL_RESTART_SHIFT         `$INSTANCE_NAME`_B_I2CM_UDB_1_ctrlreg__5__POS
#define `$INSTANCE_NAME`_CTRL_RESTART_MASK          (0x01u << `$INSTANCE_NAME`_CTRL_RESTART_SHIFT)

#define `$INSTANCE_NAME`_CTRL_STOP_SHIFT            `$INSTANCE_NAME`_B_I2CM_UDB_1_ctrlreg__6__POS
#define `$INSTANCE_NAME`_CTRL_STOP_MASK             (0x01u << `$INSTANCE_NAME`_CTRL_STOP_SHIFT)

#define `$INSTANCE_NAME`_INT_ENABLE_MASK            0x10u

/* Status Register Bit Locations */
#define `$INSTANCE_NAME`_BYTE_COMPLETE_SHIFT        `$INSTANCE_NAME`_B_I2CM_UDB_1_stsreg__0__POS
#define `$INSTANCE_NAME`_BYTE_COMPLETE_MASK         (0x01u << `$INSTANCE_NAME`_BYTE_COMPLETE_SHIFT)

#define `$INSTANCE_NAME`_LRB_SHIFT                  `$INSTANCE_NAME`_B_I2CM_UDB_1_stsreg__1__POS
#define `$INSTANCE_NAME`_LRB_MASK                   (0x01u << `$INSTANCE_NAME`_LRB_SHIFT)

#define `$INSTANCE_NAME`_BUSY_SHIFT                 `$INSTANCE_NAME`_B_I2CM_UDB_1_stsreg__4__POS
#define `$INSTANCE_NAME`_BUSY_MASK                  (0x01u << `$INSTANCE_NAME`_BUSY_SHIFT)

#if (defined(`$INSTANCE_NAME`_multimaster) && `$INSTANCE_NAME`_multimaster)
    #define `$INSTANCE_NAME`_LOST_ARB_SHIFT         `$INSTANCE_NAME`_B_I2CM_UDB_1_stsreg__5__POS
    #define `$INSTANCE_NAME`_LOST_ARB_MASK          (0x01u << `$INSTANCE_NAME`_LOST_ARB_SHIFT)
#else

#endif


/***************************************
*            API Constants        
***************************************/

/* "mode" constants for MasterWriteBuf() or MasterReadBuf() function */
#define `$INSTANCE_NAME`_MODE_COMPLETE_XFER         0x00u    /* Full transfer with Start and Stop */
#define `$INSTANCE_NAME`_MODE_REPEAT_START          0x01u    /* Begin with a ReStart instead of a Start */
#define `$INSTANCE_NAME`_MODE_NO_STOP               0x02u    /* Complete the transfer without a Stop */

/* Master status */
#define `$INSTANCE_NAME`_MSTAT_CLEAR                0x00u   /* Clear (init) status value */

#define `$INSTANCE_NAME`_MSTAT_RD_CMPLT             0x01u   /* Read complete */
#define `$INSTANCE_NAME`_MSTAT_WR_CMPLT             0x02u   /* Write complete */
#define `$INSTANCE_NAME`_MSTAT_XFER_INP             0x04u   /* Master transfer in progress */
#define `$INSTANCE_NAME`_MSTAT_XFER_HALT            0x08u   /* Transfer is halted */

#define `$INSTANCE_NAME`_MSTAT_ERR_MASK             0xF0u   /* Mask for all errors */
#define `$INSTANCE_NAME`_MSTAT_ERR_SHORT_XFER       0x10u   /* Master NAKed before end of packet */
#define `$INSTANCE_NAME`_MSTAT_ERR_ADDR_NAK         0x20u   /* Slave did not ACK */
#define `$INSTANCE_NAME`_MSTAT_ERR_ARB_LOST         0x40u   /* Master lost arbitration during communication */
#define `$INSTANCE_NAME`_MSTAT_ERR_XFER             0x80u   /* Error during transfer */
#define `$INSTANCE_NAME`_MSTAT_ERR_BUF_OVFL         0x80u   /* Buffer overflow/underflow */

/* Master API returns */
#define `$INSTANCE_NAME`_MSTR_NO_ERROR              0x00u    /* Function complete without error */
#define `$INSTANCE_NAME`_MSTR_BUS_TIMEOUT           0x01u    /* Bus timeout occured, process not started */
#define `$INSTANCE_NAME`_MSTR_SLAVE_BUSY            0x02u    /* Slave operation in progress */ 
#define `$INSTANCE_NAME`_MSTR_ERR_LB_NAK            0x03u    /* Last Byte Naked */

/* Other master control constants */
#define `$INSTANCE_NAME`_READ_XFER_MODE             0x01u    /* Read */
#define `$INSTANCE_NAME`_WRITE_XFER_MODE            0x00u    /* Write */
#define `$INSTANCE_NAME`_ACK_DATA                   0x01u    /* Send ACK */
#define `$INSTANCE_NAME`_NAK_DATA                   0x00u    /* Send NAK */

#define `$INSTANCE_NAME`_READ_FLAG                  0x01u


/***************************************
*   Fixed Function compatible defines
***************************************/

/***************************************
* Defines to make UDB match Fixed Function  
***************************************/

#define `$INSTANCE_NAME`_MCSR                       `$INSTANCE_NAME`_CSR
#define `$INSTANCE_NAME`_MCSR_BUS_BUSY              `$INSTANCE_NAME`_BUSY_MASK
#define `$INSTANCE_NAME`_MCSR_START_GEN             0x01u   /* Firmware sets this bit to initiate a Start condition */    // pROBLY WRONG !!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!
#define `$INSTANCE_NAME`_MCSR_RESTART_GEN           `$INSTANCE_NAME`_CTRL_RESTART_MASK   /* Firmware sets this bit to initiate a ReStart condition */
#define `$INSTANCE_NAME`_CSR_BYTE_COMPLETE          `$INSTANCE_NAME`_BYTE_COMPLETE_MASK
#define `$INSTANCE_NAME`_CSR_LRB                    `$INSTANCE_NAME`_LRB_MASK   /* Last received bit. */
#define `$INSTANCE_NAME`_CSR_LRB_NAK                `$INSTANCE_NAME`_LRB_MASK   /* Last received bit was an NAK */
#define `$INSTANCE_NAME`_CSR_LRB_ACK                0x00u   /* Last received bit was an ACK */
#define `$INSTANCE_NAME`_CSR_LOST_ARB               `$INSTANCE_NAME`_LOST_ARB_MASK   /* Set to 1 if lost arbitration in host mode */

#define `$INSTANCE_NAME`_DISABLE_INT_ON_STOP        {`$INSTANCE_NAME`_INT_MASK &= ~(`$INSTANCE_NAME`_CTRL_STOP_MASK);}
#define `$INSTANCE_NAME`_ENABLE_INT_ON_STOP         {`$INSTANCE_NAME`_INT_MASK |= `$INSTANCE_NAME`_CTRL_STOP_MASK;}

#define `$INSTANCE_NAME`_ACK_AND_TRANSMIT           {`$INSTANCE_NAME`_CFG &= ~(`$INSTANCE_NAME`_CTRL_NACK_MASK); \
                                                     `$INSTANCE_NAME`_CFG |= (`$INSTANCE_NAME`_CTRL_TRANSMIT_MASK); \
                                                     `$INSTANCE_NAME`_GO = 0x00;}

#define `$INSTANCE_NAME`_NAK_AND_TRANSMIT           {`$INSTANCE_NAME`_CFG |= (`$INSTANCE_NAME`_CTRL_NACK_MASK); \
                                                     `$INSTANCE_NAME`_CFG |= (`$INSTANCE_NAME`_CTRL_TRANSMIT_MASK); \
                                                     `$INSTANCE_NAME`_GO = 0x00;}

#define `$INSTANCE_NAME`_ACK_AND_RECEIVE             {`$INSTANCE_NAME`_CFG &= (~(`$INSTANCE_NAME`_CTRL_TRANSMIT_MASK) & ~(`$INSTANCE_NAME`_CTRL_NACK_MASK)); \
                                                     `$INSTANCE_NAME`_GO = 0x00;}

#define `$INSTANCE_NAME`_NAK_AND_RECEIVE             {`$INSTANCE_NAME`_CFG &= ~(`$INSTANCE_NAME`_CTRL_TRANSMIT_MASK); \
                                                     `$INSTANCE_NAME`_CFG |= `$INSTANCE_NAME`_CTRL_NACK_MASK; \
                                                     `$INSTANCE_NAME`_GO = 0x00;}

#define `$INSTANCE_NAME`_TRANSMIT_DATA              {`$INSTANCE_NAME`_CFG &= (~(`$INSTANCE_NAME`_CTRL_NACK_MASK) & ~(`$INSTANCE_NAME`_CTRL_STOP_MASK)); \
                                                     `$INSTANCE_NAME`_CFG |= (`$INSTANCE_NAME`_CTRL_TRANSMIT_MASK); \
                                                     `$INSTANCE_NAME`_GO = 0x00;}

#define `$INSTANCE_NAME`_GENERATE_STOP              {`$INSTANCE_NAME`_CFG |= `$INSTANCE_NAME`_CTRL_STOP_MASK; \
                                                     `$INSTANCE_NAME`_GO = 0x00;}

//#define `$INSTANCE_NAME`_CLEAR_STOP                 {`$INSTANCE_NAME`_CFG &= ~`$INSTANCE_NAME`_CTRL_STOP_MASK;}
#define `$INSTANCE_NAME`_READY_TO_RD                {`$INSTANCE_NAME`_CFG &= ~`$INSTANCE_NAME`_CTRL_STOP_MASK; \
                                                     `$INSTANCE_NAME`_CFG &= (~(`$INSTANCE_NAME`_CTRL_TRANSMIT_MASK) & ~(`$INSTANCE_NAME`_CTRL_NACK_MASK)); \
                                                     `$INSTANCE_NAME`_GO = 0x00;}

#define `$INSTANCE_NAME`_CHECK_ADDR_ACK(csr)         ((csr & `$INSTANCE_NAME`_CSR_LRB) == `$INSTANCE_NAME`_CSR_LRB_ACK)
#define `$INSTANCE_NAME`_CHECK_ADDR_NAK(csr)         ((csr & `$INSTANCE_NAME`_CSR_LRB) == `$INSTANCE_NAME`_CSR_LRB_NAK)
