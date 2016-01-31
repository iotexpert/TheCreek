/*******************************************************************************
* Copyright 2008-2009, Cypress Semiconductor Corporation.  All rights reserved.
* You may use this file only in accordance with the license, terms, conditions, 
* disclaimers, and limitations in the end user license agreement accompanying 
* the software package with which this file was provided.
********************************************************************************/



/***************************************
*        Function Prototypes 
***************************************/

/* I2C Master commands */  
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
*            API Constants        
***************************************/

/* "mode" constants for MasterWriteBuf() or MasterReadBuf() function */
#define `$INSTANCE_NAME`_MODE_COMPLETE_XFER  0x00u    /* Full transfer with Start and Stop */
#define `$INSTANCE_NAME`_MODE_REPEAT_START   0x01u    /* Begin with a ReStart instead of a Start */
#define `$INSTANCE_NAME`_MODE_NO_STOP        0x02u    /* Complete the transfer without a Stop */

/* Master status constants.  These constants used for the following API return values. */
/*     `$INSTANCE_NAME`_MasterStatus      */
/*     `$INSTANCE_NAME`_MasterSendStart   */
/*     `$INSTANCE_NAME`_MasterSendRestart */
/*     `$INSTANCE_NAME`_MasterWriteBuf    */
/*     `$INSTANCE_NAME`_MasterReadBuf     */
/*     `$INSTANCE_NAME`_MasterWriteByte   */
/*     `$INSTANCE_NAME`_MasterReadByte    */

/* Master status */
#define `$INSTANCE_NAME`_MSTAT_CLEAR          0x00u   /* Clear (init) status value */

#define `$INSTANCE_NAME`_MSTAT_RD_CMPLT       0x01u   /* Read complete */
#define `$INSTANCE_NAME`_MSTAT_WR_CMPLT       0x02u   /* Write complete */
#define `$INSTANCE_NAME`_MSTAT_XFER_INP       0x04u   /* Master transfer in progress */
#define `$INSTANCE_NAME`_MSTAT_XFER_HALT      0x08u   /* Transfer is halted */

#define `$INSTANCE_NAME`_MSTAT_ERR_MASK       0xF0u   /* Mask for all errors */
#define `$INSTANCE_NAME`_MSTAT_ERR_SHORT_XFER 0x10u   /* Master NAKed before end of packet */
#define `$INSTANCE_NAME`_MSTAT_ERR_ADDR_NAK   0x20u   /* Slave did not ACK */
#define `$INSTANCE_NAME`_MSTAT_ERR_ARB_LOST   0x40u   /* Master lost arbitration during communication */
#define `$INSTANCE_NAME`_MSTAT_ERR_XFER       0x80u   /* Error during transfer */
#define `$INSTANCE_NAME`_MSTAT_ERR_BUF_OVFL   0x80u   /* Buffer overflow/underflow */


/* Master API returns */
#define `$INSTANCE_NAME`_MSTR_NO_ERROR        0x00u    /* Function complete without error */
#define `$INSTANCE_NAME`_MSTR_BUS_TIMEOUT     0x01u    /* Bus timeout occured, process not started */
#define `$INSTANCE_NAME`_MSTR_SLAVE_BUSY      0x02u    /* Slave operation in progress */ 
#define `$INSTANCE_NAME`_MSTR_ERR_LB_NAK      0x03u    /* Last Byte Naked */


/* Other master control constants */
#define `$INSTANCE_NAME`_READ_XFER_MODE      0x01u    /* Read */
#define `$INSTANCE_NAME`_WRITE_XFER_MODE     0x00u    /* Write */
#define `$INSTANCE_NAME`_ACK_DATA            0x01u    /* Send ACK */
#define `$INSTANCE_NAME`_NAK_DATA            0x00u    /* Send NAK */

/* Slave Status Constants */
#define `$INSTANCE_NAME`_SSTAT_RD_CMPT       0x01u    /* Read transfer complete */
#define `$INSTANCE_NAME`_SSTAT_RD_BUSY       0x02u    /* Read transfer in progress */
#define `$INSTANCE_NAME`_SSTAT_RD_ERR        0x08u    /* Read Error buffer */
#define `$INSTANCE_NAME`_SSTAT_RD_NO_ERR     0x00u    /* Read no Error */
#define `$INSTANCE_NAME`_SSTAT_RD_ERR_OVFL   0x04u    /* Read overflow Error */
#define `$INSTANCE_NAME`_SSTAT_RD_MASK       0x0Fu    /* Read Status Mask */

#define `$INSTANCE_NAME`_SSTAT_WR_CMPT       0x10u    /* Write transfer complete */
#define `$INSTANCE_NAME`_SSTAT_WR_BUSY       0x20u    /* Write transfer in progress */
#define `$INSTANCE_NAME`_SSTAT_WR_ERR        0xC0u    /* Write Error buffer */
#define `$INSTANCE_NAME`_SSTAT_WR_NO_ERR     0x00u    /* Write no Error */
#define `$INSTANCE_NAME`_SSTAT_WR_ERR_OVFL   0x40u    /* Write overflow Error */
#define `$INSTANCE_NAME`_SSTAT_WR_MASK       0xF0u    /* Write Status Mask  */

#define `$INSTANCE_NAME`_SSTAT_RD_CLEAR      0x0Du    /* Read Status clear */
#define `$INSTANCE_NAME`_SSTAT_WR_CLEAR      0xD0u    /* Write Status Clear */

/***************************************
*              Registers        
***************************************/

#define `$INSTANCE_NAME`_XCFG   (* (reg8 *) `$INSTANCE_NAME`_I2C_Prim__XCFG )
#define `$INSTANCE_NAME`_ADDR   (* (reg8 *) `$INSTANCE_NAME`_I2C_Prim__ADR )
#define `$INSTANCE_NAME`_CFG    (* (reg8 *) `$INSTANCE_NAME`_I2C_Prim__CFG )
#define `$INSTANCE_NAME`_CSR    (* (reg8 *) `$INSTANCE_NAME`_I2C_Prim__CSR )
#define `$INSTANCE_NAME`_DATA   (* (reg8 *) `$INSTANCE_NAME`_I2C_Prim__D )
#define `$INSTANCE_NAME`_MCSR   (* (reg8 *) `$INSTANCE_NAME`_I2C_Prim__MCSR )
#define `$INSTANCE_NAME`_CLKDIV (* (reg8 *) `$INSTANCE_NAME`_I2C_Prim__CLK_DIV )
#define `$INSTANCE_NAME`_PWRMGR (* (reg8 *) `$INSTANCE_NAME`_I2C_Prim__PM_ACT_CFG )  /* Power manager */

/********************************************/
/* XCFG I2C Extended Configuration Register */  
/********************************************/
#define `$INSTANCE_NAME`_XCFG_CLK_EN       0x80u
#define `$INSTANCE_NAME`_XCFG_HDWR_ADDR_EN 0x01u

/********************************************/
/* D(ata) I2C Slave Data Register           */  
/********************************************/
#define `$INSTANCE_NAME`_SADDR_MASK        0x7Fu
#define `$INSTANCE_NAME`_DATA_MASK         0xFFu
#define `$INSTANCE_NAME`_READ_FLAG         0x01u


/********************************************/
/* CFG I2C Configuration Register           */
/********************************************/
#define `$INSTANCE_NAME`_CFG_SIO_SELECT    0x80u   /* Pin Select for SCL/SDA lines */
#define `$INSTANCE_NAME`_CFG_PSELECT       0x40u   /* Pin Select */
#define `$INSTANCE_NAME`_CFG_BUS_ERR_IE    0x20u   /* Bus Error Interrupt Enable */
#define `$INSTANCE_NAME`_CFG_STOP_IE       0x10u   /* Enable Interrupt on STOP condition */
#define `$INSTANCE_NAME`_CFG_STOP_ERR_IE   0x10u   /* Enable Interrupt on STOP condition */
#define `$INSTANCE_NAME`_CFG_CLK_RATE_MSK  0x0Cu   /* Clock rate select  **CHECK**  */
#define `$INSTANCE_NAME`_CFG_CLK_RATE_100  0x00u   /* Clock rate select 100K */
#define `$INSTANCE_NAME`_CFG_CLK_RATE_400  0x04u   /* Clock rate select 400K */
#define `$INSTANCE_NAME`_CFG_CLK_RATE_050  0x08u   /* Clock rate select 50K  */
#define `$INSTANCE_NAME`_CFG_CLK_RATE_RSVD 0x0Cu   /* Clock rate select Invalid */
#define `$INSTANCE_NAME`_CFG_EN_MSTR       0x02u   /* Enable Master operation */
#define `$INSTANCE_NAME`_CFG_EN_SLAVE      0x01u   /* Enable Slave operation */


/********************************************/
/* CSR I2C Control and Status Register      */  
/********************************************/
#define `$INSTANCE_NAME`_CSR_BUS_ERROR     0x80u   /* Active high when bus error has occured */
#define `$INSTANCE_NAME`_CSR_LOST_ARB      0x40u   /* Set to 1 if lost arbitration in host mode */
#define `$INSTANCE_NAME`_CSR_STOP_STATUS   0x20u   /* Set if Stop has been detected */
#define `$INSTANCE_NAME`_CSR_ACK           0x10u   /* ACK response */
#define `$INSTANCE_NAME`_CSR_NAK           0x00u   /* NAK response */
#define `$INSTANCE_NAME`_CSR_ADDRESS       0x08u   /* Set in firmware 0 = status bit, 1 Address is slave */
#define `$INSTANCE_NAME`_CSR_TRANSMIT      0x04u   /* Set in firmware 1 = transmit, 0 = receive. */
#define `$INSTANCE_NAME`_CSR_LRB           0x02u   /* Last received bit. */
#define `$INSTANCE_NAME`_CSR_LRB_ACK       0x00u   /* Last received bit was an ACK */
#define `$INSTANCE_NAME`_CSR_LRB_NAK       0x02u   /* Last received bit was an NAK */
#define `$INSTANCE_NAME`_CSR_BYTE_COMPLETE 0x01u   /* Informs that last byte has been sent. */
#define `$INSTANCE_NAME`_CSR_GEN_STOP      0x00u   /* Generate a stop condition */
#define `$INSTANCE_NAME`_CSR_RDY_TO_RD     0x00u   /* Generate a stop condition */

/***********************************************/
/* MCSR I2C Master Control and Status Register */  
/***********************************************/
#define `$INSTANCE_NAME`_MCSR_BUS_BUSY     0x08u   /* Status bit, Set at Start and cleared at Stop condition */
#define `$INSTANCE_NAME`_MCSR_MSTR_MODE    0x04u   /* Status bit, Set at Start and cleared at Stop condition */
#define `$INSTANCE_NAME`_MCSR_RESTART_GEN  0x02u   /* Firmware sets this bit to initiate a ReStart condition */
#define `$INSTANCE_NAME`_MCSR_START_GEN    0x01u   /* Firmware sets this bit to initiate a Start condition */

/********************************************/
/* CLK_DIV I2C Clock Divide Factor Register */  
/********************************************/
#define `$INSTANCE_NAME`_CLK_DIV_MSK       0x07u   /* Status bit, Set at Start and cleared at Stop condition */
#define `$INSTANCE_NAME`_CLK_DIV_1         0x00u   /* Divide input clock by  1 */
#define `$INSTANCE_NAME`_CLK_DIV_2         0x01u   /* Divide input clock by  2 */
#define `$INSTANCE_NAME`_CLK_DIV_4         0x02u   /* Divide input clock by  4 */
#define `$INSTANCE_NAME`_CLK_DIV_8         0x03u   /* Divide input clock by  8 */
#define `$INSTANCE_NAME`_CLK_DIV_16        0x04u   /* Divide input clock by 16 */
#define `$INSTANCE_NAME`_CLK_DIV_32        0x05u   /* Divide input clock by 32 */
#define `$INSTANCE_NAME`_CLK_DIV_64        0x06u   /* Divide input clock by 64 */

/************************************************/
/* PM_ACT_CFG (Active Power Mode CFG Register)  */ 
/************************************************/
#define `$INSTANCE_NAME`_ACT_PWR_EN   `$INSTANCE_NAME`_I2C_Prim__PM_ACT_MSK /* Power enable mask */


/* Create constansts to enable slave and/or master  */
#if  (`$INSTANCE_NAME`_MODE & `$INSTANCE_NAME`_MODE_SLAVE)     
   #define  `$INSTANCE_NAME`_ENABLE_SLAVE  `$INSTANCE_NAME`_CFG_EN_SLAVE
#else
   #define  `$INSTANCE_NAME`_ENABLE_SLAVE  0u
#endif

#if  (`$INSTANCE_NAME`_MODE & `$INSTANCE_NAME`_MODE_MASTER)     
   #define  `$INSTANCE_NAME`_ENABLE_MASTER  `$INSTANCE_NAME`_CFG_EN_MSTR
#else
   #define  `$INSTANCE_NAME`_ENABLE_MASTER  0u
#endif

/*******************************
 * I2C state machine constants *
 *******************************/

/* Default slave address states */
#define  `$INSTANCE_NAME`_DEV_MASK             0xF0   /* Wait for sub-address */
#define  `$INSTANCE_NAME`_SM_IDLE              0x10   /* Idle I2C state */
#define  `$INSTANCE_NAME`_DEV_MASTER_XFER      0x40   /* Wait for sub-address */

/* Default slave address states */
#define  `$INSTANCE_NAME`_SM_SL_WR_IDLE        0x10   /* Slave Idle, waiting for start */
#define  `$INSTANCE_NAME`_SM_SL_WR_DATA        0x11   /* Slave waiting for master to write data */
#define  `$INSTANCE_NAME`_SM_SL_RD_DATA        0x12   /* Slave waiting for master to read data */
#define  `$INSTANCE_NAME`_SM_SL_STOP           0x14   /* Slave waiting for stop */

/* Master mode states */
#define  `$INSTANCE_NAME`_SM_MASTER            0x40   /* Master or Multi-Master mode is set */
#define  `$INSTANCE_NAME`_SM_MASTER_IDLE       0x40   /* Hardware in Master mode and sitting idle  */

#define  `$INSTANCE_NAME`_SM_MSTR_ADDR         0x43   /* Master has sent Start/Address */
#define  `$INSTANCE_NAME`_SM_MSTR_WR_ADDR      0x42   /* Master has sent a Start/Address/WR */
#define  `$INSTANCE_NAME`_SM_MSTR_RD_ADDR      0x43   /* Master has sent a Start/Address/RD */

#define  `$INSTANCE_NAME`_SM_MSTR_DATA         0x44   /* Master is writing data to external slave */
#define  `$INSTANCE_NAME`_SM_MSTR_WR_DATA      0x44   /* Master is writing data to external slave */
#define  `$INSTANCE_NAME`_SM_MSTR_RD_DATA      0x45   /* Master is receiving data from external slave */

#define  `$INSTANCE_NAME`_SM_MSTR_WAIT_STOP    0x48   /* Master Send Stop */
#define  `$INSTANCE_NAME`_SM_MSTR_HALT         0x60   /* Master Halt state */


/* mstrControl bit definitions */
#define  `$INSTANCE_NAME`_MSTR_GEN_STOP        0x01   /* Generate a stop after a data transfer */ 
#define  `$INSTANCE_NAME`_MSTR_NO_STOP         0x01   /* Do not generate a stop after a data transfer */ 


/*******************************
 * UDB compatible defines
 *******************************/

#define `$INSTANCE_NAME`_DISABLE_INT_ON_STOP   {`$INSTANCE_NAME`_CFG &= ~`$INSTANCE_NAME`_CFG_STOP_IE;}
#define `$INSTANCE_NAME`_ENABLE_INT_ON_STOP    {`$INSTANCE_NAME`_CFG |= `$INSTANCE_NAME`_CFG_STOP_IE;}

#define `$INSTANCE_NAME`_ACK_AND_TRANSMIT      {`$INSTANCE_NAME`_CSR  = (`$INSTANCE_NAME`_CSR_ACK | `$INSTANCE_NAME`_CSR_TRANSMIT);}
#define `$INSTANCE_NAME`_NAK_AND_TRANSMIT      {`$INSTANCE_NAME`_CSR  = (`$INSTANCE_NAME`_CSR_NAK | `$INSTANCE_NAME`_CSR_TRANSMIT);}

#define `$INSTANCE_NAME`_ACK_AND_RECIVE        {`$INSTANCE_NAME`_CSR = `$INSTANCE_NAME`_CSR_ACK;}
#define `$INSTANCE_NAME`_NAK_AND_RECIVE        {`$INSTANCE_NAME`_CSR = `$INSTANCE_NAME`_CSR_NAK;}

#define `$INSTANCE_NAME`_TRANSMIT_DATA         {`$INSTANCE_NAME`_CSR  = `$INSTANCE_NAME`_CSR_TRANSMIT;}

#define `$INSTANCE_NAME`_GENERATE_STOP         {`$INSTANCE_NAME`_CSR = `$INSTANCE_NAME`_CSR_GEN_STOP;}

#define `$INSTANCE_NAME`_READY_TO_RD           {`$INSTANCE_NAME`_CSR = `$INSTANCE_NAME`_CSR_RDY_TO_RD;}       /* Ready to Read data */

#define `$INSTANCE_NAME`_CHECK_ADDR_ACK(csr)   ((csr & (`$INSTANCE_NAME`_CSR_LRB | `$INSTANCE_NAME`_CSR_ADDRESS)) == (`$INSTANCE_NAME`_CSR_LRB_ACK | `$INSTANCE_NAME`_CSR_ADDRESS))
#define `$INSTANCE_NAME`_CHECK_ADDR_NAK(csr)   ((csr & (`$INSTANCE_NAME`_CSR_LRB | `$INSTANCE_NAME`_CSR_ADDRESS)) == (`$INSTANCE_NAME`_CSR_LRB_NAK | `$INSTANCE_NAME`_CSR_ADDRESS))
 

