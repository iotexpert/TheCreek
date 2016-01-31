/*******************************************************************************
* File Name: `$INSTANCE_NAME`.h  
* Version `$CY_MAJOR_VERSION`.`$CY_MINOR_VERSION`
*
*  Description:
*   This is the header file for the EzI2C user module.  It contains function
*   prototypes and constants for the users convenience. 
*
*   Note:
*
********************************************************************************
* Copyright 2008-2009, Cypress Semiconductor Corporation.  All rights reserved.
* You may use this file only in accordance with the license, terms, conditions, 
* disclaimers, and limitations in the end user license agreement accompanying 
* the software package with which this file was provided.
********************************************************************************/



#if !defined(`$INSTANCE_NAME`_H) 
#define `$INSTANCE_NAME`_H 

#include "cytypes.h"
#include "cyfitter.h"


#if(CYDEV_CHIP_DIE_EXPECT == CYDEV_CHIP_DIE_LEOPARD)     
    #if((CYDEV_CHIP_REV_EXPECT <= CYDEV_CHIP_REV_LEOPARD_ES2) && (`$INSTANCE_NAME`_isr__ES2_PATCH))      
        #include <intrins.h>
        #define `$INSTANCE_NAME`_ISR_PATCH() _nop_(); _nop_(); _nop_(); _nop_(); _nop_(); _nop_(); _nop_(); _nop_();
    #endif
#endif


/***************************************
*   Conditional Compilation Parameters 
***************************************/

#define `$INSTANCE_NAME`_ADDRESSES         `$I2C_Addresses` 
#define `$INSTANCE_NAME`_ONE_ADDRESS       0x01u   
#define `$INSTANCE_NAME`_TWO_ADDRESSES     0x02u   


/***************************************
*        Function Prototypes 
***************************************/

void    `$INSTANCE_NAME`_Start(void);
void    `$INSTANCE_NAME`_Stop(void);
void    `$INSTANCE_NAME`_EnableInt(void);
void    `$INSTANCE_NAME`_DisableInt(void);
void    `$INSTANCE_NAME`_SetAddress1( uint8 address);
uint8   `$INSTANCE_NAME`_GetAddress1( void );
void    `$INSTANCE_NAME`_SetBuffer1(uint16 bufSize, uint16 rwBoundry, void * dataPtr);
uint8   `$INSTANCE_NAME`_GetActivity(void);
CY_ISR_PROTO( `$INSTANCE_NAME`_ISR );

#if (`$INSTANCE_NAME`_ADDRESSES == `$INSTANCE_NAME`_ONE_ADDRESS)
    void    `$INSTANCE_NAME`_SlaveSetSleepMode(void);
    void    `$INSTANCE_NAME`_SlaveSetWakeMode(void);
#endif

#if (`$INSTANCE_NAME`_ADDRESSES == `$INSTANCE_NAME`_TWO_ADDRESSES)
    void    `$INSTANCE_NAME`_SetAddress2( uint8 address);
    uint8   `$INSTANCE_NAME`_GetAddress2( void );
    void    `$INSTANCE_NAME`_SetBuffer2(uint16 bufSize, uint16 rwBoundry, void * dataPtr);
#endif


/***************************************
*              API Constants        
***************************************/

/* Status bit definition */
#define `$INSTANCE_NAME`_STATUS_READ1      0x01u    /* A read addr 1 operation occured since last status check */
#define `$INSTANCE_NAME`_STATUS_WRITE1     0x02u    /* A Write addr 1 opereation occured since last status check */
#define `$INSTANCE_NAME`_STATUS_READ2      0x04u    /* A read addr 2 operation occured since last status check */
#define `$INSTANCE_NAME`_STATUS_WRITE2     0x08u    /* A Write addr 2 opereation occured since last status check */
#define `$INSTANCE_NAME`_STATUS_BUSY       0x10u    /* A start has occured, but a Stop has not been detected */
#define `$INSTANCE_NAME`_STATUS_RD1BUSY    0x11u    /* Addr 1 read busy */
#define `$INSTANCE_NAME`_STATUS_WR1BUSY    0x12u    /* Addr 1 write busy */
#define `$INSTANCE_NAME`_STATUS_RD2BUSY    0x14u    /* Addr 2 read busy */
#define `$INSTANCE_NAME`_STATUS_WR2BUSY    0x18u    /* Addr 2 write busy */
#define `$INSTANCE_NAME`_STATUS_MASK       0x1Fu    /* Mask for status bits.  */
#define `$INSTANCE_NAME`_STATUS_ERR        0x80u    /* An Error occured since last read */


/***************************************
*   Initial Parameter Constants 
***************************************/

#define `$INSTANCE_NAME`_DEFAULT_ADDR1     `$I2C_Address1`   
#define `$INSTANCE_NAME`_DEFAULT_ADDR2     `$I2C_Address2`    

#define `$INSTANCE_NAME`_ENABLE_WAKEUP     `$EnableWakeup`   
#define `$INSTANCE_NAME`_BUS_SPEED         `$BusSpeed_kHz`   
#define `$INSTANCE_NAME`_DEFAULT_CLKDIV    (BCLK__BUS_CLK__KHZ/(`$INSTANCE_NAME`_BUS_SPEED*16*4)) 

#define `$INSTANCE_NAME`_SUBADDR_WIDTH      `$Sub_Address_Size`  
#define `$INSTANCE_NAME`_SUBADDR_8BIT       0x00u    /* 8-bit sub-address width */
#define `$INSTANCE_NAME`_SUBADDR_16BIT      0x01u    /* 16-bit sub-address width */


/***************************************
*              Registers        
***************************************/

#define `$INSTANCE_NAME`_XCFG   (* (reg8 *) `$INSTANCE_NAME`_I2C_Prim__XCFG )
#define `$INSTANCE_NAME`_ADDR   (* (reg8 *) `$INSTANCE_NAME`_I2C_Prim__ADR )
#define `$INSTANCE_NAME`_CFG    (* (reg8 *) `$INSTANCE_NAME`_I2C_Prim__CFG )
#define `$INSTANCE_NAME`_CSR    (* (reg8 *) `$INSTANCE_NAME`_I2C_Prim__CSR )
#define `$INSTANCE_NAME`_Data   (* (reg8 *) `$INSTANCE_NAME`_I2C_Prim__D )
#define `$INSTANCE_NAME`_MCSR   (* (reg8 *) `$INSTANCE_NAME`_I2C_Prim__MCSR )
#define `$INSTANCE_NAME`_CLKDIV (* (reg8 *) `$INSTANCE_NAME`_I2C_Prim__CLK_DIV )
#define `$INSTANCE_NAME`_PWRMGR (* (reg8 *) `$INSTANCE_NAME`_I2C_Prim__PM_ACT_CFG )  /* Power manager */


/***************************************
*       Register Constants        
***************************************/

/* XCFG I2C Extended Configuration Register */  
#define `$INSTANCE_NAME`_XCFG_CLK_EN       0x80u
#define `$INSTANCE_NAME`_XCFG_HDWR_ADDR_EN 0x01u

/* D(ata) I2C Slave Data Register */  
#define `$INSTANCE_NAME`_SADDR_MASK        0x7Fu
#define `$INSTANCE_NAME`_DATA_MASK         0xFFu
#define `$INSTANCE_NAME`_READ_FLAG         0x01u

/* CFG I2C Configuration Register */
#define `$INSTANCE_NAME`_CFG_SIO_SELECT    0x80u   /* Pin Select for SCL/SDA lines */
#define `$INSTANCE_NAME`_CFG_PSELECT       0x40u   /* Pin Select */
#define `$INSTANCE_NAME`_CFG_BUS_ERR_IE    0x20u   /* Bus Error Interrupt Enable */
#define `$INSTANCE_NAME`_CFG_STOP_IE       0x10u   /* Enable Interrupt on STOP condition */
#define `$INSTANCE_NAME`_CFG_STOP_ERR_IE   0x10u   /* Enable Interrupt on STOP condition */
#define `$INSTANCE_NAME`_CFG_CLK_RATE_MSK  0x0Cu   /* Clock rate select  */  // TODO: IC change
#define `$INSTANCE_NAME`_CFG_CLK_RATE_100  0x00u   /* Clock rate select 100K */
#define `$INSTANCE_NAME`_CFG_CLK_RATE_400  0x04u   /* Clock rate select 400K */
#define `$INSTANCE_NAME`_CFG_CLK_RATE_050  0x08u   /* Clock rate select 50K  */
#define `$INSTANCE_NAME`_CFG_CLK_RATE_RSVD 0x0Cu   /* Clock rate select Invalid */
#define `$INSTANCE_NAME`_CFG_EN_MSTR       0x02u   /* Enable Master operation */
#define `$INSTANCE_NAME`_CFG_EN_SLAVE      0x01u   /* Enable Slave operation */

/* CSR I2C Control and Status Register */  
#define `$INSTANCE_NAME`_CSR_BUS_ERROR     0x80u   /* Active high when bus error has occured */
#define `$INSTANCE_NAME`_CSR_LOST_ARB      0x40u   /* Set to 1 if lost arbitration in host mode */
#define `$INSTANCE_NAME`_CSR_STOP_STATUS   0x20u   /* Set to 1 if Stop has been detected */
#define `$INSTANCE_NAME`_CSR_ACK           0x10u   /* ACK response */
#define `$INSTANCE_NAME`_CSR_NAK           0x00u   /* NAK response */
#define `$INSTANCE_NAME`_CSR_ADDRESS       0x08u   /* Set in firmware 0 = status bit, 1 Address is slave */
#define `$INSTANCE_NAME`_CSR_TRANSMIT      0x04u   /* Set in firmware 1 = transmit, 0 = receive. */
#define `$INSTANCE_NAME`_CSR_LRB           0x02u   /* Last received bit. */
#define `$INSTANCE_NAME`_CSR_LRB_ACK       0x00u   /* Last received bit was an ACK */
#define `$INSTANCE_NAME`_CSR_LRB_NAK       0x02u   /* Last received bit was an NAK */
#define `$INSTANCE_NAME`_CSR_BYTE_COMPLETE 0x01u   /* Informs that last byte has been sent. */

/* MCSR I2C Master Control and Status Register */  
#define `$INSTANCE_NAME`_MCSR_BUS_BUSY     0x08u   /* Status bit, Set at Start and cleared at Stop condition */
#define `$INSTANCE_NAME`_MCSR_MSTR_MODE    0x04u   /* Status bit, Set at Start and cleared at Stop condition */
#define `$INSTANCE_NAME`_MCSR_RESTART_GEN  0x02u   /* Firmware sets this bit to initiate a ReStart condition */
#define `$INSTANCE_NAME`_MCSR_START_GEN    0x01u   /* Firmware sets this bit to initiate a Start condition */

/* CLK_DIV I2C Clock Divide Factor Register */  
#define `$INSTANCE_NAME`_CLK_DIV_MSK       0x07u   /* Status bit, Set at Start and cleared at Stop condition */
#define `$INSTANCE_NAME`_CLK_DIV_1         0x00u   /* Divide input clock by  1 */
#define `$INSTANCE_NAME`_CLK_DIV_2         0x01u   /* Divide input clock by  2 */
#define `$INSTANCE_NAME`_CLK_DIV_4         0x02u   /* Divide input clock by  4 */
#define `$INSTANCE_NAME`_CLK_DIV_8         0x03u   /* Divide input clock by  8 */
#define `$INSTANCE_NAME`_CLK_DIV_16        0x04u   /* Divide input clock by 16 */
#define `$INSTANCE_NAME`_CLK_DIV_32        0x05u   /* Divide input clock by 32 */
#define `$INSTANCE_NAME`_CLK_DIV_64        0x06u   /* Divide input clock by 64 */

/* PM_ACT_CFG (Active Power Mode CFG Register)     */ 
#define `$INSTANCE_NAME`_ACT_PWR_EN    `$INSTANCE_NAME`_I2C_Prim__PM_ACT_MSK /* Power enable mask */

/* Number of the `$INSTANCE_NAME`_isr interrupt. */
#define `$INSTANCE_NAME`_ISR_NUMBER    `$INSTANCE_NAME``[isr]`_INTC_NUMBER

/* Priority of the `$INSTANCE_NAME`_isr interrupt. */
#define `$INSTANCE_NAME`_ISR_PRIORITY  `$INSTANCE_NAME``[isr]`_INTC_PRIOR_NUM


/***************************************
*     I2C state machine constants   
***************************************/

#define  `$INSTANCE_NAME`_SM_IDLE              0x00   /* Wait for Start */

/* Default address states */
#define  `$INSTANCE_NAME`_SM_DEV1_WR_ADDR      0x01   /* Wait for sub-address */
#define  `$INSTANCE_NAME`_SM_DEV1_WR_ADDR_MSB  0x01   /* Wait for sub-address MSB */
#define  `$INSTANCE_NAME`_SM_DEV1_WR_ADDR_LSB  0x02   /* Wait for sub-address LSB */
#define  `$INSTANCE_NAME`_SM_DEV1_WR_DATA      0x04   /* Get data from Master */
#define  `$INSTANCE_NAME`_SM_DEV1_RD_DATA      0x08   /* Send data to Master */

/* Second address states */
#define  `$INSTANCE_NAME`_SM_DEV2_WR_ADDR      0x11   /* Wait for sub-address */
#define  `$INSTANCE_NAME`_SM_DEV2_WR_ADDR_MSB  0x11   /* Wait for sub-address MSB */
#define  `$INSTANCE_NAME`_SM_DEV2_WR_ADDR_LSB  0x12   /* Wait for sub-address LSB */
#define  `$INSTANCE_NAME`_SM_DEV2_WR_DATA      0x14   /* Get data from Master */
#define  `$INSTANCE_NAME`_SM_DEV2_RD_DATA      0x18   /* Send data to Master */


#endif /* `$INSTANCE_NAME`_H */

/* [] END OF FILE */


