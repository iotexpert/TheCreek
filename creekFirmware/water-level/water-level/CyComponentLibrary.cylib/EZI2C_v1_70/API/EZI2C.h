/*******************************************************************************
* File Name: `$INSTANCE_NAME`.h
* Version `$CY_MAJOR_VERSION`.`$CY_MINOR_VERSION`
*
* Description:
*  This is the header file for the EzI2C user module.  It contains function
*  prototypes and constants for the users convenience.
*
********************************************************************************
* Copyright 2008-2012, Cypress Semiconductor Corporation.  All rights reserved.
* You may use this file only in accordance with the license, terms, conditions,
* disclaimers, and limitations in the end user license agreement accompanying
* the software package with which this file was provided.
*******************************************************************************/

#if !defined(CY_EZI2C_`$INSTANCE_NAME`_H)
#define CY_EZI2C_`$INSTANCE_NAME`_H

#include "cytypes.h"
#include "cyfitter.h"


/***************************************
*   Conditional Compilation Parameters
***************************************/

#define `$INSTANCE_NAME`_ADDRESSES         (`$I2C_Addresses`u)
#define `$INSTANCE_NAME`_ONE_ADDRESS       (0x01u)
#define `$INSTANCE_NAME`_TWO_ADDRESSES     (0x02u)

/* Check to see if required defines such as CY_PSOC5A are available */
/* They are defined starting with cy_boot v3.0 */
#if !defined (CY_PSOC5A)
    #error Component `$CY_COMPONENT_NAME` requires cy_boot v3.0 or later
#endif /* (CY_ PSOC5A) */


/***************************************
*   Data Struct Definition
***************************************/

/* Low power modes API Support */
typedef struct _`$INSTANCE_NAME`_backupStruct
{
    uint8   enableState;

    uint8   xcfg;
    uint8   adr;
    uint8   cfg;
    
    #if(CY_PSOC5A)
        
        uint8   clkDiv;
        
    #else
        
        uint8   clkDiv1;
        uint8   clkDiv2;
        
    #endif /* (CY_PSOC5A) */
}   `$INSTANCE_NAME`_BACKUP_STRUCT;


/***************************************
*        Function Prototypes
***************************************/

void    `$INSTANCE_NAME`_Start(void) `=ReentrantKeil($INSTANCE_NAME . "_Start")`;
void    `$INSTANCE_NAME`_Stop(void) `=ReentrantKeil($INSTANCE_NAME . "_Stop")`;
void    `$INSTANCE_NAME`_EnableInt(void) `=ReentrantKeil($INSTANCE_NAME . "_EnableInt")`;
void    `$INSTANCE_NAME`_DisableInt(void) `=ReentrantKeil($INSTANCE_NAME . "_DisableInt")`;

void    `$INSTANCE_NAME`_SetAddress1(uint8 address) `=ReentrantKeil($INSTANCE_NAME . "_SetAddress1")`;
uint8   `$INSTANCE_NAME`_GetAddress1(void) `=ReentrantKeil($INSTANCE_NAME . "_GetAddress1")`;
void    `$INSTANCE_NAME`_SetBuffer1(uint16 bufSize, uint16 rwBoundry, void * dataPtr) `=ReentrantKeil($INSTANCE_NAME . "_SetBuffer1")`;
uint8   `$INSTANCE_NAME`_GetActivity(void) `=ReentrantKeil($INSTANCE_NAME . "_GetActivity")`;
void    `$INSTANCE_NAME`_Init(void) `=ReentrantKeil($INSTANCE_NAME . "_Init")`;
void    `$INSTANCE_NAME`_Enable(void) `=ReentrantKeil($INSTANCE_NAME . "_Enable")`;
void    `$INSTANCE_NAME`_Sleep(void) `=ReentrantKeil($INSTANCE_NAME . "_Sleep")`;
void    `$INSTANCE_NAME`_Wakeup(void) `=ReentrantKeil($INSTANCE_NAME . "_Wakeup")`;
void    `$INSTANCE_NAME`_SaveConfig(void) `=ReentrantKeil($INSTANCE_NAME . "_SaveConfig")`;
void    `$INSTANCE_NAME`_RestoreConfig(void) `=ReentrantKeil($INSTANCE_NAME . "_RestoreConfig")`;

CY_ISR_PROTO(`$INSTANCE_NAME`_ISR);

#if(`$INSTANCE_NAME`_ADDRESSES == `$INSTANCE_NAME`_ONE_ADDRESS)

    void    `$INSTANCE_NAME`_SlaveSetSleepMode(void) `=ReentrantKeil($INSTANCE_NAME . "_SlaveSetSleepMode")`;
    void    `$INSTANCE_NAME`_SlaveSetWakeMode(void) `=ReentrantKeil($INSTANCE_NAME . "_SlaveSetWakeMode")`;

#endif /* (`$INSTANCE_NAME`_ADDRESSES == `$INSTANCE_NAME`_ONE_ADDRESS)*/

#if(`$INSTANCE_NAME`_ADDRESSES == `$INSTANCE_NAME`_TWO_ADDRESSES)

    void    `$INSTANCE_NAME`_SetAddress2(uint8 address )`=ReentrantKeil($INSTANCE_NAME . "_SetAddress2")`;
    uint8   `$INSTANCE_NAME`_GetAddress2(void) `=ReentrantKeil($INSTANCE_NAME . "_GetAddress2")`;
    void    `$INSTANCE_NAME`_SetBuffer2(uint16 bufSize, uint16 rwBoundry, void * dataPtr) `=ReentrantKeil($INSTANCE_NAME . "_SetBuffer2")`;

#endif  /* (`$INSTANCE_NAME`_ADDRESSES == `$INSTANCE_NAME`_TWO_ADDRESSES) */


/***************************************
*   Initial Parameter Constants
***************************************/

#define `$INSTANCE_NAME`_DEFAULT_ADDR1      (`$I2C_Address1`u)
#define `$INSTANCE_NAME`_DEFAULT_ADDR2      (`$I2C_Address2`u)
#define `$INSTANCE_NAME`_ENABLE_WAKEUP      (`$EnableWakeup`u)
#define `$INSTANCE_NAME`_BUS_SPEED          (`$BusSpeed_kHz`u)
#define `$INSTANCE_NAME`_SUBADDR_WIDTH      (`$Sub_Address_Size`u)
#define `$INSTANCE_NAME`_BUS_PORT           (`$I2cBusPort`u)


/***************************************
*   Enumerated Types and Parameters
***************************************/

/* Enumerated type*/
`#declare_enum I2cBusPortType`


/***************************************
*              API Constants
***************************************/

/* Status bit definition */

/* A read addr 1 operation occured since last status check */
#define `$INSTANCE_NAME`_STATUS_READ1       (0x01u)

/* A Write addr 1 opereation occured since last status check */
#define `$INSTANCE_NAME`_STATUS_WRITE1      (0x02u)

/* A read addr 2 operation occured since last status check */
#define `$INSTANCE_NAME`_STATUS_READ2       (0x04u)

/* A Write addr 2 opereation occured since last status check */
#define `$INSTANCE_NAME`_STATUS_WRITE2      (0x08u)

/* A start has occured, but a Stop has not been detected */
#define `$INSTANCE_NAME`_STATUS_BUSY        (0x10u)

/* Addr 1 read busy */
#define `$INSTANCE_NAME`_STATUS_RD1BUSY     (0x11u)

/* Addr 1 write busy */
#define `$INSTANCE_NAME`_STATUS_WR1BUSY     (0x12u)

/* Addr 2 read busy */
#define `$INSTANCE_NAME`_STATUS_RD2BUSY     (0x14u)

/* Addr 2 write busy */
#define `$INSTANCE_NAME`_STATUS_WR2BUSY     (0x18u)

/* Mask for status bits. */
#define `$INSTANCE_NAME`_STATUS_MASK        (0x1Fu)

/* An Error occured since last read */
#define `$INSTANCE_NAME`_STATUS_ERR         (0x80u)

/* Dummy data to be sent to master */
#define `$INSTANCE_NAME`_DUMMY_DATA         (0xFFu)

/* The I2C Master bits in I2C cinfiguration register */
#define `$INSTANCE_NAME`_I2C_MASTER_MASK    (0xDDu)

/* Component's enable/disable state */
#define `$INSTANCE_NAME`_ENABLED            (0x01u)
#define `$INSTANCE_NAME`_DISABLED           (0x00u)


#if(CY_PSOC5A)

    /* Samples per bit. Other value is not used */
    #define `$INSTANCE_NAME`_16_SAMPLES_PER_BIT    (16u)

    /* Default CLK Divider =
    *  BusClock / (I2C_Bus_Speed * `$INSTANCE_NAME`_16_SAMPLES_PER_BIT)
    */
    #define `$INSTANCE_NAME`_DEFAULT_CLKDIV (BCLK__BUS_CLK__KHZ / \
       (`$INSTANCE_NAME`_BUS_SPEED * `$INSTANCE_NAME`_16_SAMPLES_PER_BIT))
    
#else

    #define `$INSTANCE_NAME`_BUS_SPEED_50KHZ      (50u)
	
	/* Bus speed grater 50kHz requires 16 oversample rate */
	#if (`$INSTANCE_NAME`_BUS_SPEED <= `$INSTANCE_NAME`_BUS_SPEED_50KHZ)

		#define `$INSTANCE_NAME`_OVER_SAMPLE_RATE       (32u)

	#else

		#define `$INSTANCE_NAME`_OVER_SAMPLE_RATE       (16u)

	#endif  /* End (`$INSTANCE_NAME`_BUS_SPEED <= `$INSTANCE_NAME`_BUS_SPEED_50KHZ) */
    
	/* Divide factor calculation */
	#define `$INSTANCE_NAME`_DIVIDE_FACTOR_WITH_FRACT_BYTE  \
                    (((uint32) BCLK__BUS_CLK__KHZ << 8u) / ((uint32)`$INSTANCE_NAME`_BUS_SPEED * \
                    `$INSTANCE_NAME`_OVER_SAMPLE_RATE))
                    
	#define `$INSTANCE_NAME`_DIVIDE_FACTOR  (((uint32) `$INSTANCE_NAME`_DIVIDE_FACTOR_WITH_FRACT_BYTE) >> 8u)

#endif  /* (CY_PSOC5A) */

/* Following definitions are for the COMPATIBILITY ONLY, they are OBSOLETE. */
#define `$INSTANCE_NAME`_State          `$INSTANCE_NAME`_curState
#define `$INSTANCE_NAME`_Status         `$INSTANCE_NAME`_curStatus
#define `$INSTANCE_NAME`_DataPtr        `$INSTANCE_NAME`_dataPtrS1

#define `$INSTANCE_NAME`_RwOffset1      `$INSTANCE_NAME`_rwOffsetS1
#define `$INSTANCE_NAME`_RwIndex1       `$INSTANCE_NAME`_rwIndexS1
#define `$INSTANCE_NAME`_WrProtect1     `$INSTANCE_NAME`_wrProtectS1
#define `$INSTANCE_NAME`_BufSize1       `$INSTANCE_NAME`_bufSizeS1

#if(`$INSTANCE_NAME`_ADDRESSES == `$INSTANCE_NAME`_TWO_ADDRESSES)

    #define `$INSTANCE_NAME`_DataPtr2   `$INSTANCE_NAME`_dataPtrS2
    #define `$INSTANCE_NAME`_Address1   `$INSTANCE_NAME`_addrS1
    #define `$INSTANCE_NAME`_Address2   `$INSTANCE_NAME`_addrS2

    #define `$INSTANCE_NAME`_RwOffset2  `$INSTANCE_NAME`_rwOffsetS2
    #define `$INSTANCE_NAME`_RwIndex2   `$INSTANCE_NAME`_rwIndexS2
    #define `$INSTANCE_NAME`_WrProtect2 `$INSTANCE_NAME`_wrProtectS2
    #define `$INSTANCE_NAME`_BufSize2   `$INSTANCE_NAME`_bufSizeS2

#endif /* `$INSTANCE_NAME`_ADDRESSES == `$INSTANCE_NAME`_TWO_ADDRESSES */

/* Returns 1 if corresponding bit is set, otherwise 0 */
#define `$INSTANCE_NAME`_IS_BIT_SET(value, mask) (((mask) == ((value) & (mask))) ? 0x01u : 0x00u)

#define `$INSTANCE_NAME`_ADDRESS_SHIFT      (1u)
#define `$INSTANCE_NAME`_ADDRESS_LSB_SHIFT  (8u)

/* 8-bit sub-address width */
#define `$INSTANCE_NAME`_SUBADDR_8BIT       (0x00u)

/* 16-bit sub-address width */
#define `$INSTANCE_NAME`_SUBADDR_16BIT      (0x01u)


/***************************************
*              Registers
***************************************/

/* I2C Extended Configuration Register */
#define `$INSTANCE_NAME`_XCFG_REG       (* (reg8 *) `$INSTANCE_NAME`_I2C_Prim__XCFG )
#define `$INSTANCE_NAME`_XCFG_PTR       (  (reg8 *) `$INSTANCE_NAME`_I2C_Prim__XCFG )

/* I2C Slave Adddress Register */
#define `$INSTANCE_NAME`_ADDR_REG       (* (reg8 *) `$INSTANCE_NAME`_I2C_Prim__ADR )
#define `$INSTANCE_NAME`_ADDR_PTR       (  (reg8 *) `$INSTANCE_NAME`_I2C_Prim__ADR )

/* I2C Configuration Register */
#define `$INSTANCE_NAME`_CFG_REG        (* (reg8 *) `$INSTANCE_NAME`_I2C_Prim__CFG )
#define `$INSTANCE_NAME`_CFG_PTR        (  (reg8 *) `$INSTANCE_NAME`_I2C_Prim__CFG )

/* I2C Control and Status Register */
#define `$INSTANCE_NAME`_CSR_REG        (* (reg8 *) `$INSTANCE_NAME`_I2C_Prim__CSR )
#define `$INSTANCE_NAME`_CSR_PTR        (  (reg8 *) `$INSTANCE_NAME`_I2C_Prim__CSR )

/* I2C Data Register */
#define `$INSTANCE_NAME`_DATA_REG       (* (reg8 *) `$INSTANCE_NAME`_I2C_Prim__D )
#define `$INSTANCE_NAME`_DATA_PTR       (  (reg8 *) `$INSTANCE_NAME`_I2C_Prim__D )

#if(CY_PSOC5A)

    /* I2C Clock Divide Factor Register */
    #define `$INSTANCE_NAME`_CLKDIV_REG         (* (reg8 *) `$INSTANCE_NAME`_I2C_Prim__CLK_DIV )
    #define `$INSTANCE_NAME`_CLKDIV_PTR         (  (reg8 *) `$INSTANCE_NAME`_I2C_Prim__CLK_DIV )

#else

     /*  8 LSB bits of the 10-bit Clock Divider */
    #define `$INSTANCE_NAME`_CLKDIV1_REG        (* (reg8 *) `$INSTANCE_NAME`_I2C_Prim__CLK_DIV1 )
    #define `$INSTANCE_NAME`_CLKDIV1_PTR        (  (reg8 *) `$INSTANCE_NAME`_I2C_Prim__CLK_DIV1 )

    /* 2 MSB bits of the 10-bit Clock Divider */
    #define `$INSTANCE_NAME`_CLKDIV2_REG        (* (reg8 *) `$INSTANCE_NAME`_I2C_Prim__CLK_DIV2 )
    #define `$INSTANCE_NAME`_CLKDIV2_PTR        (  (reg8 *) `$INSTANCE_NAME`_I2C_Prim__CLK_DIV2 )

#endif  /* (CY_PSOC5A) */

/* Power System Control Register 1 */
#define `$INSTANCE_NAME`_PWRSYS_CR1_REG     (* (reg8 *) CYREG_PWRSYS_CR1 )
#define `$INSTANCE_NAME`_PWRSYS_CR1_PTR     (  (reg8 *) CYREG_PWRSYS_CR1 )

/* I2C operation in Active Mode */
#define `$INSTANCE_NAME`_PM_ACT_CFG_REG     (* (reg8 *) `$INSTANCE_NAME`_I2C_Prim__PM_ACT_CFG )
#define `$INSTANCE_NAME`_PM_ACT_CFG_PTR     (  (reg8 *) `$INSTANCE_NAME`_I2C_Prim__PM_ACT_CFG )

/* I2C operation in Alternate Active (Standby) Mode */
#define `$INSTANCE_NAME`_PM_STBY_CFG_REG    (* (reg8 *) `$INSTANCE_NAME`_I2C_Prim__PM_STBY_CFG )
#define `$INSTANCE_NAME`_PM_STBY_CFG_PTR    (  (reg8 *) `$INSTANCE_NAME`_I2C_Prim__PM_STBY_CFG )


/***************************************
*       Register Constants
***************************************/

/* XCFG I2C Extended Configuration Register */
#define `$INSTANCE_NAME`_XCFG_CLK_EN            (0x80u)
#define `$INSTANCE_NAME`_XCFG_HDWR_ADDR_EN      (0x01u)

#if(CY_PSOC3 || CY_PSOC5LP)

    /* Force nack */
    #define `$INSTANCE_NAME`_XCFG_FORCE_NACK            (0x10u)
    
    /* Ready to sleep */
    #define `$INSTANCE_NAME`_XCFG_SLEEP_READY           (0x20u)
    
    /* if I2C block will be used as wake up source */
    #if(`$INSTANCE_NAME`_ENABLE_WAKEUP == 1u)

        /* Should be set before entering sleep mode */
        #define `$INSTANCE_NAME`_XCFG_I2C_ON            (0x40u)

        /* Enables the I2C regulator backup */
        #define `$INSTANCE_NAME`_PWRSYS_CR1_I2C_BACKUP  (0x04u)

    #endif  /* (`$INSTANCE_NAME`_ENABLE_WAKEUP == 1u) */

#endif  /* (CY_PSOC3 || CY_PSOC5LP) */

/* Data I2C Slave Data Register */
#define `$INSTANCE_NAME`_SADDR_MASK        (0x7Fu)
#define `$INSTANCE_NAME`_DATA_MASK         (0xFFu)
#define `$INSTANCE_NAME`_READ_FLAG         (0x01u)

/* CFG I2C Configuration Register */

/* Pin Select for SCL/SDA lines */
#define `$INSTANCE_NAME`_CFG_SIO_SELECT    (0x80u)

/* Pin Select */
#define `$INSTANCE_NAME`_CFG_PSELECT       (0x40u)

/* Bus Error Interrupt Enable */
#define `$INSTANCE_NAME`_CFG_BUS_ERR_IE    (0x20u)

/* Enable Interrupt on STOP condition */
#define `$INSTANCE_NAME`_CFG_STOP_IE       (0x10u)

/* Enable Interrupt on STOP condition */
#define `$INSTANCE_NAME`_CFG_STOP_ERR_IE   (0x10u)


#if(CY_PSOC5A)

    /* Clock rate select */
    #define `$INSTANCE_NAME`_CFG_CLK_RATE_MSK  (0x0Cu)

    /* Clock rate select 100K */
    #define `$INSTANCE_NAME`_CFG_CLK_RATE_100  (0x00u)

    /* Clock rate select 400K */
    #define `$INSTANCE_NAME`_CFG_CLK_RATE_400  (0x04u)

    /* Clock rate select 50K */
    #define `$INSTANCE_NAME`_CFG_CLK_RATE_050  (0x08u)

    /* Clock rate select Invalid */
    #define `$INSTANCE_NAME`_CFG_CLK_RATE_RSVD (0x0Cu)

#else

    /* Clock rate mask. 1 for 50K, 0 for 100K and 400K */
    #define `$INSTANCE_NAME`_CFG_CLK_RATE      (0x04u)

#endif  /* (CY_PSOC5A) */

/* Enable Slave operation */
#define `$INSTANCE_NAME`_CFG_EN_SLAVE      (0x01u)

/* CSR I2C Control and Status Register */

/* Active high when bus error has occured */
#define `$INSTANCE_NAME`_CSR_BUS_ERROR     (0x80u)

/* Set to 1 if lost arbitration in host mode */
#define `$INSTANCE_NAME`_CSR_LOST_ARB      (0x40u)

/* Set to 1 if Stop has been detected */
#define `$INSTANCE_NAME`_CSR_STOP_STATUS   (0x20u)

/* ACK response */
#define `$INSTANCE_NAME`_CSR_ACK           (0x10u)

/* NAK response */
#define `$INSTANCE_NAME`_CSR_NAK           (0x00u)

/* Set in firmware 0 = status bit, 1 Address is slave */
#define `$INSTANCE_NAME`_CSR_ADDRESS       (0x08u)

/* Set in firmware 1 = transmit, 0 = receive. */
#define `$INSTANCE_NAME`_CSR_TRANSMIT      (0x04u)

/* Last received bit. */
#define `$INSTANCE_NAME`_CSR_LRB           (0x02u)

 /* Last received bit was an ACK */
#define `$INSTANCE_NAME`_CSR_LRB_ACK       (0x00u)

/* Last received bit was an NAK */
#define `$INSTANCE_NAME`_CSR_LRB_NAK       (0x02u)

/* Informs that last byte has been sent. */
#define `$INSTANCE_NAME`_CSR_BYTE_COMPLETE (0x01u)

/* CLK_DIV I2C Clock Divide Factor Register */

/* Status bit, Set at Start and cleared at Stop condition */
#define `$INSTANCE_NAME`_CLK_DIV_MSK       (0x07u)

/* Divide input clock by  1 */
#define `$INSTANCE_NAME`_CLK_DIV_1         (0x00u)

/* Divide input clock by  2 */
#define `$INSTANCE_NAME`_CLK_DIV_2         (0x01u)

/* Divide input clock by  4 */
#define `$INSTANCE_NAME`_CLK_DIV_4         (0x02u)

/* Divide input clock by  8 */
#define `$INSTANCE_NAME`_CLK_DIV_8         (0x03u)

/* Divide input clock by 16 */
#define `$INSTANCE_NAME`_CLK_DIV_16        (0x04u)

/* Divide input clock by 32 */
#define `$INSTANCE_NAME`_CLK_DIV_32        (0x05u)

/* Divide input clock by 64 */
#define `$INSTANCE_NAME`_CLK_DIV_64        (0x06u)

/* Active Power Mode CFG Register - power enable mask */
#define `$INSTANCE_NAME`_ACT_PWR_EN    `$INSTANCE_NAME`_I2C_Prim__PM_ACT_MSK

/* Alternate Active (Standby) Power Mode CFG Register - power enable mask */
#define `$INSTANCE_NAME`_STBY_PWR_EN    `$INSTANCE_NAME`_I2C_Prim__PM_STBY_MSK

/* Number of the `$INSTANCE_NAME`_isr interrupt. */
#define `$INSTANCE_NAME`_ISR_NUMBER    `$INSTANCE_NAME``[isr]`_INTC_NUMBER

/* Priority of the `$INSTANCE_NAME`_isr interrupt. */
#define `$INSTANCE_NAME`_ISR_PRIORITY  `$INSTANCE_NAME``[isr]`_INTC_PRIOR_NUM

/* I2C state machine constants */

/* Wait for Start */
#define  `$INSTANCE_NAME`_SM_IDLE              (0x00u)

/* Default address states */

/* Wait for sub-address */
#define  `$INSTANCE_NAME`_SM_DEV1_WR_ADDR      (0x01u)

/* Wait for sub-address MSB */
#define  `$INSTANCE_NAME`_SM_DEV1_WR_ADDR_MSB  (0x01u)

/* Wait for sub-address LSB */
#define  `$INSTANCE_NAME`_SM_DEV1_WR_ADDR_LSB  (0x02u)

/* Get data from Master */
#define  `$INSTANCE_NAME`_SM_DEV1_WR_DATA      (0x04u)

/* Send data to Master */
#define  `$INSTANCE_NAME`_SM_DEV1_RD_DATA      (0x08u)

/* Second address states */

/* Wait for sub-address */
#define  `$INSTANCE_NAME`_SM_DEV2_WR_ADDR      (0x11u)

/* Wait for sub-address MSB */
#define  `$INSTANCE_NAME`_SM_DEV2_WR_ADDR_MSB  (0x11u)

/* Wait for sub-address LSB */
#define  `$INSTANCE_NAME`_SM_DEV2_WR_ADDR_LSB  (0x12u)

/* Get data from Master */
#define  `$INSTANCE_NAME`_SM_DEV2_WR_DATA      (0x14u)

/* Send data to Master */
#define  `$INSTANCE_NAME`_SM_DEV2_RD_DATA      (0x18u)

#endif /* CY_EZI2C_`$INSTANCE_NAME`_H */


/* [] END OF FILE */
