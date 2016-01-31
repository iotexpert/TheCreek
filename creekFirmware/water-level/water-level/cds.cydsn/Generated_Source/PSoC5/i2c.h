/*******************************************************************************
* File Name: i2c.h
* Version 1.70
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

#if !defined(CY_EZI2C_i2c_H)
#define CY_EZI2C_i2c_H

#include "cytypes.h"
#include "cyfitter.h"


/***************************************
*   Conditional Compilation Parameters
***************************************/

#define i2c_ADDRESSES         (1u)
#define i2c_ONE_ADDRESS       (0x01u)
#define i2c_TWO_ADDRESSES     (0x02u)

/* Check to see if required defines such as CY_PSOC5A are available */
/* They are defined starting with cy_boot v3.0 */
#if !defined (CY_PSOC5A)
    #error Component EZI2C_v1_70 requires cy_boot v3.0 or later
#endif /* (CY_ PSOC5A) */


/***************************************
*   Data Struct Definition
***************************************/

/* Low power modes API Support */
typedef struct _i2c_backupStruct
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
}   i2c_BACKUP_STRUCT;


/***************************************
*        Function Prototypes
***************************************/

void    i2c_Start(void) ;
void    i2c_Stop(void) ;
void    i2c_EnableInt(void) ;
void    i2c_DisableInt(void) ;

void    i2c_SetAddress1(uint8 address) ;
uint8   i2c_GetAddress1(void) ;
void    i2c_SetBuffer1(uint16 bufSize, uint16 rwBoundry, void * dataPtr) ;
uint8   i2c_GetActivity(void) ;
void    i2c_Init(void) ;
void    i2c_Enable(void) ;
void    i2c_Sleep(void) ;
void    i2c_Wakeup(void) ;
void    i2c_SaveConfig(void) ;
void    i2c_RestoreConfig(void) ;

CY_ISR_PROTO(i2c_ISR);

#if(i2c_ADDRESSES == i2c_ONE_ADDRESS)

    void    i2c_SlaveSetSleepMode(void) ;
    void    i2c_SlaveSetWakeMode(void) ;

#endif /* (i2c_ADDRESSES == i2c_ONE_ADDRESS)*/

#if(i2c_ADDRESSES == i2c_TWO_ADDRESSES)

    void    i2c_SetAddress2(uint8 address );
    uint8   i2c_GetAddress2(void) ;
    void    i2c_SetBuffer2(uint16 bufSize, uint16 rwBoundry, void * dataPtr) ;

#endif  /* (i2c_ADDRESSES == i2c_TWO_ADDRESSES) */


/***************************************
*   Initial Parameter Constants
***************************************/

#define i2c_DEFAULT_ADDR1      (8u)
#define i2c_DEFAULT_ADDR2      (9u)
#define i2c_ENABLE_WAKEUP      (0u)
#define i2c_BUS_SPEED          (100u)
#define i2c_SUBADDR_WIDTH      (0u)
#define i2c_BUS_PORT           (0u)


/***************************************
*   Enumerated Types and Parameters
***************************************/

/* Enumerated type*/
#define i2c__ANY 0
#define i2c__I2C0 1
#define i2c__I2C1 2



/***************************************
*              API Constants
***************************************/

/* Status bit definition */

/* A read addr 1 operation occured since last status check */
#define i2c_STATUS_READ1       (0x01u)

/* A Write addr 1 opereation occured since last status check */
#define i2c_STATUS_WRITE1      (0x02u)

/* A read addr 2 operation occured since last status check */
#define i2c_STATUS_READ2       (0x04u)

/* A Write addr 2 opereation occured since last status check */
#define i2c_STATUS_WRITE2      (0x08u)

/* A start has occured, but a Stop has not been detected */
#define i2c_STATUS_BUSY        (0x10u)

/* Addr 1 read busy */
#define i2c_STATUS_RD1BUSY     (0x11u)

/* Addr 1 write busy */
#define i2c_STATUS_WR1BUSY     (0x12u)

/* Addr 2 read busy */
#define i2c_STATUS_RD2BUSY     (0x14u)

/* Addr 2 write busy */
#define i2c_STATUS_WR2BUSY     (0x18u)

/* Mask for status bits. */
#define i2c_STATUS_MASK        (0x1Fu)

/* An Error occured since last read */
#define i2c_STATUS_ERR         (0x80u)

/* Dummy data to be sent to master */
#define i2c_DUMMY_DATA         (0xFFu)

/* The I2C Master bits in I2C cinfiguration register */
#define i2c_I2C_MASTER_MASK    (0xDDu)

/* Component's enable/disable state */
#define i2c_ENABLED            (0x01u)
#define i2c_DISABLED           (0x00u)


#if(CY_PSOC5A)

    /* Samples per bit. Other value is not used */
    #define i2c_16_SAMPLES_PER_BIT    (16u)

    /* Default CLK Divider =
    *  BusClock / (I2C_Bus_Speed * i2c_16_SAMPLES_PER_BIT)
    */
    #define i2c_DEFAULT_CLKDIV (BCLK__BUS_CLK__KHZ / \
       (i2c_BUS_SPEED * i2c_16_SAMPLES_PER_BIT))
    
#else

    #define i2c_BUS_SPEED_50KHZ      (50u)
	
	/* Bus speed grater 50kHz requires 16 oversample rate */
	#if (i2c_BUS_SPEED <= i2c_BUS_SPEED_50KHZ)

		#define i2c_OVER_SAMPLE_RATE       (32u)

	#else

		#define i2c_OVER_SAMPLE_RATE       (16u)

	#endif  /* End (i2c_BUS_SPEED <= i2c_BUS_SPEED_50KHZ) */
    
	/* Divide factor calculation */
	#define i2c_DIVIDE_FACTOR_WITH_FRACT_BYTE  \
                    (((uint32) BCLK__BUS_CLK__KHZ << 8u) / ((uint32)i2c_BUS_SPEED * \
                    i2c_OVER_SAMPLE_RATE))
                    
	#define i2c_DIVIDE_FACTOR  (((uint32) i2c_DIVIDE_FACTOR_WITH_FRACT_BYTE) >> 8u)

#endif  /* (CY_PSOC5A) */

/* Following definitions are for the COMPATIBILITY ONLY, they are OBSOLETE. */
#define i2c_State          i2c_curState
#define i2c_Status         i2c_curStatus
#define i2c_DataPtr        i2c_dataPtrS1

#define i2c_RwOffset1      i2c_rwOffsetS1
#define i2c_RwIndex1       i2c_rwIndexS1
#define i2c_WrProtect1     i2c_wrProtectS1
#define i2c_BufSize1       i2c_bufSizeS1

#if(i2c_ADDRESSES == i2c_TWO_ADDRESSES)

    #define i2c_DataPtr2   i2c_dataPtrS2
    #define i2c_Address1   i2c_addrS1
    #define i2c_Address2   i2c_addrS2

    #define i2c_RwOffset2  i2c_rwOffsetS2
    #define i2c_RwIndex2   i2c_rwIndexS2
    #define i2c_WrProtect2 i2c_wrProtectS2
    #define i2c_BufSize2   i2c_bufSizeS2

#endif /* i2c_ADDRESSES == i2c_TWO_ADDRESSES */

/* Returns 1 if corresponding bit is set, otherwise 0 */
#define i2c_IS_BIT_SET(value, mask) (((mask) == ((value) & (mask))) ? 0x01u : 0x00u)

#define i2c_ADDRESS_SHIFT      (1u)
#define i2c_ADDRESS_LSB_SHIFT  (8u)

/* 8-bit sub-address width */
#define i2c_SUBADDR_8BIT       (0x00u)

/* 16-bit sub-address width */
#define i2c_SUBADDR_16BIT      (0x01u)


/***************************************
*              Registers
***************************************/

/* I2C Extended Configuration Register */
#define i2c_XCFG_REG       (* (reg8 *) i2c_I2C_Prim__XCFG )
#define i2c_XCFG_PTR       (  (reg8 *) i2c_I2C_Prim__XCFG )

/* I2C Slave Adddress Register */
#define i2c_ADDR_REG       (* (reg8 *) i2c_I2C_Prim__ADR )
#define i2c_ADDR_PTR       (  (reg8 *) i2c_I2C_Prim__ADR )

/* I2C Configuration Register */
#define i2c_CFG_REG        (* (reg8 *) i2c_I2C_Prim__CFG )
#define i2c_CFG_PTR        (  (reg8 *) i2c_I2C_Prim__CFG )

/* I2C Control and Status Register */
#define i2c_CSR_REG        (* (reg8 *) i2c_I2C_Prim__CSR )
#define i2c_CSR_PTR        (  (reg8 *) i2c_I2C_Prim__CSR )

/* I2C Data Register */
#define i2c_DATA_REG       (* (reg8 *) i2c_I2C_Prim__D )
#define i2c_DATA_PTR       (  (reg8 *) i2c_I2C_Prim__D )

#if(CY_PSOC5A)

    /* I2C Clock Divide Factor Register */
    #define i2c_CLKDIV_REG         (* (reg8 *) i2c_I2C_Prim__CLK_DIV )
    #define i2c_CLKDIV_PTR         (  (reg8 *) i2c_I2C_Prim__CLK_DIV )

#else

     /*  8 LSB bits of the 10-bit Clock Divider */
    #define i2c_CLKDIV1_REG        (* (reg8 *) i2c_I2C_Prim__CLK_DIV1 )
    #define i2c_CLKDIV1_PTR        (  (reg8 *) i2c_I2C_Prim__CLK_DIV1 )

    /* 2 MSB bits of the 10-bit Clock Divider */
    #define i2c_CLKDIV2_REG        (* (reg8 *) i2c_I2C_Prim__CLK_DIV2 )
    #define i2c_CLKDIV2_PTR        (  (reg8 *) i2c_I2C_Prim__CLK_DIV2 )

#endif  /* (CY_PSOC5A) */

/* Power System Control Register 1 */
#define i2c_PWRSYS_CR1_REG     (* (reg8 *) CYREG_PWRSYS_CR1 )
#define i2c_PWRSYS_CR1_PTR     (  (reg8 *) CYREG_PWRSYS_CR1 )

/* I2C operation in Active Mode */
#define i2c_PM_ACT_CFG_REG     (* (reg8 *) i2c_I2C_Prim__PM_ACT_CFG )
#define i2c_PM_ACT_CFG_PTR     (  (reg8 *) i2c_I2C_Prim__PM_ACT_CFG )

/* I2C operation in Alternate Active (Standby) Mode */
#define i2c_PM_STBY_CFG_REG    (* (reg8 *) i2c_I2C_Prim__PM_STBY_CFG )
#define i2c_PM_STBY_CFG_PTR    (  (reg8 *) i2c_I2C_Prim__PM_STBY_CFG )


/***************************************
*       Register Constants
***************************************/

/* XCFG I2C Extended Configuration Register */
#define i2c_XCFG_CLK_EN            (0x80u)
#define i2c_XCFG_HDWR_ADDR_EN      (0x01u)

#if(CY_PSOC3 || CY_PSOC5LP)

    /* Force nack */
    #define i2c_XCFG_FORCE_NACK            (0x10u)
    
    /* Ready to sleep */
    #define i2c_XCFG_SLEEP_READY           (0x20u)
    
    /* if I2C block will be used as wake up source */
    #if(i2c_ENABLE_WAKEUP == 1u)

        /* Should be set before entering sleep mode */
        #define i2c_XCFG_I2C_ON            (0x40u)

        /* Enables the I2C regulator backup */
        #define i2c_PWRSYS_CR1_I2C_BACKUP  (0x04u)

    #endif  /* (i2c_ENABLE_WAKEUP == 1u) */

#endif  /* (CY_PSOC3 || CY_PSOC5LP) */

/* Data I2C Slave Data Register */
#define i2c_SADDR_MASK        (0x7Fu)
#define i2c_DATA_MASK         (0xFFu)
#define i2c_READ_FLAG         (0x01u)

/* CFG I2C Configuration Register */

/* Pin Select for SCL/SDA lines */
#define i2c_CFG_SIO_SELECT    (0x80u)

/* Pin Select */
#define i2c_CFG_PSELECT       (0x40u)

/* Bus Error Interrupt Enable */
#define i2c_CFG_BUS_ERR_IE    (0x20u)

/* Enable Interrupt on STOP condition */
#define i2c_CFG_STOP_IE       (0x10u)

/* Enable Interrupt on STOP condition */
#define i2c_CFG_STOP_ERR_IE   (0x10u)


#if(CY_PSOC5A)

    /* Clock rate select */
    #define i2c_CFG_CLK_RATE_MSK  (0x0Cu)

    /* Clock rate select 100K */
    #define i2c_CFG_CLK_RATE_100  (0x00u)

    /* Clock rate select 400K */
    #define i2c_CFG_CLK_RATE_400  (0x04u)

    /* Clock rate select 50K */
    #define i2c_CFG_CLK_RATE_050  (0x08u)

    /* Clock rate select Invalid */
    #define i2c_CFG_CLK_RATE_RSVD (0x0Cu)

#else

    /* Clock rate mask. 1 for 50K, 0 for 100K and 400K */
    #define i2c_CFG_CLK_RATE      (0x04u)

#endif  /* (CY_PSOC5A) */

/* Enable Slave operation */
#define i2c_CFG_EN_SLAVE      (0x01u)

/* CSR I2C Control and Status Register */

/* Active high when bus error has occured */
#define i2c_CSR_BUS_ERROR     (0x80u)

/* Set to 1 if lost arbitration in host mode */
#define i2c_CSR_LOST_ARB      (0x40u)

/* Set to 1 if Stop has been detected */
#define i2c_CSR_STOP_STATUS   (0x20u)

/* ACK response */
#define i2c_CSR_ACK           (0x10u)

/* NAK response */
#define i2c_CSR_NAK           (0x00u)

/* Set in firmware 0 = status bit, 1 Address is slave */
#define i2c_CSR_ADDRESS       (0x08u)

/* Set in firmware 1 = transmit, 0 = receive. */
#define i2c_CSR_TRANSMIT      (0x04u)

/* Last received bit. */
#define i2c_CSR_LRB           (0x02u)

 /* Last received bit was an ACK */
#define i2c_CSR_LRB_ACK       (0x00u)

/* Last received bit was an NAK */
#define i2c_CSR_LRB_NAK       (0x02u)

/* Informs that last byte has been sent. */
#define i2c_CSR_BYTE_COMPLETE (0x01u)

/* CLK_DIV I2C Clock Divide Factor Register */

/* Status bit, Set at Start and cleared at Stop condition */
#define i2c_CLK_DIV_MSK       (0x07u)

/* Divide input clock by  1 */
#define i2c_CLK_DIV_1         (0x00u)

/* Divide input clock by  2 */
#define i2c_CLK_DIV_2         (0x01u)

/* Divide input clock by  4 */
#define i2c_CLK_DIV_4         (0x02u)

/* Divide input clock by  8 */
#define i2c_CLK_DIV_8         (0x03u)

/* Divide input clock by 16 */
#define i2c_CLK_DIV_16        (0x04u)

/* Divide input clock by 32 */
#define i2c_CLK_DIV_32        (0x05u)

/* Divide input clock by 64 */
#define i2c_CLK_DIV_64        (0x06u)

/* Active Power Mode CFG Register - power enable mask */
#define i2c_ACT_PWR_EN    i2c_I2C_Prim__PM_ACT_MSK

/* Alternate Active (Standby) Power Mode CFG Register - power enable mask */
#define i2c_STBY_PWR_EN    i2c_I2C_Prim__PM_STBY_MSK

/* Number of the i2c_isr interrupt. */
#define i2c_ISR_NUMBER    i2c_isr__INTC_NUMBER

/* Priority of the i2c_isr interrupt. */
#define i2c_ISR_PRIORITY  i2c_isr__INTC_PRIOR_NUM

/* I2C state machine constants */

/* Wait for Start */
#define  i2c_SM_IDLE              (0x00u)

/* Default address states */

/* Wait for sub-address */
#define  i2c_SM_DEV1_WR_ADDR      (0x01u)

/* Wait for sub-address MSB */
#define  i2c_SM_DEV1_WR_ADDR_MSB  (0x01u)

/* Wait for sub-address LSB */
#define  i2c_SM_DEV1_WR_ADDR_LSB  (0x02u)

/* Get data from Master */
#define  i2c_SM_DEV1_WR_DATA      (0x04u)

/* Send data to Master */
#define  i2c_SM_DEV1_RD_DATA      (0x08u)

/* Second address states */

/* Wait for sub-address */
#define  i2c_SM_DEV2_WR_ADDR      (0x11u)

/* Wait for sub-address MSB */
#define  i2c_SM_DEV2_WR_ADDR_MSB  (0x11u)

/* Wait for sub-address LSB */
#define  i2c_SM_DEV2_WR_ADDR_LSB  (0x12u)

/* Get data from Master */
#define  i2c_SM_DEV2_WR_DATA      (0x14u)

/* Send data to Master */
#define  i2c_SM_DEV2_RD_DATA      (0x18u)

#endif /* CY_EZI2C_i2c_H */


/* [] END OF FILE */
