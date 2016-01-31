/*******************************************************************************
* File Name: `$INSTANCE_NAME`.h  
* Version `$CY_MAJOR_VERSION`.`$CY_MINOR_VERSION`
*
* Description:
*  This file provides constants and parameter values for the I2C component.
*
* Note:
*
********************************************************************************
* Copyright 2008-2011, Cypress Semiconductor Corporation.  All rights reserved.
* You may use this file only in accordance with the license, terms, conditions, 
* disclaimers, and limitations in the end user license agreement accompanying 
* the software package with which this file was provided.
********************************************************************************/

#if !defined(CY_I2C_`$INSTANCE_NAME`_H)
#define CY_I2C_`$INSTANCE_NAME`_H

#include "cytypes.h"
#include "cyfitter.h"
#include "CyLib.h"


/***************************************
*   Conditional Compilation Parameters
****************************************/

#define `$INSTANCE_NAME`_IMPLEMENTATION             (`$Implementation`u)
#define `$INSTANCE_NAME`_MODE                       (`$I2C_Mode`u)
#define `$INSTANCE_NAME`_ADDR_DECODE                (`$Address_Decode`u)
#define `$INSTANCE_NAME`_ENABLE_WAKEUP              (`$EnableWakeup`u)
#define `$INSTANCE_NAME`_I2C_PAIR_SELECTED          (`$I2cBusPort`u)

/* I2C implementation types */
#define `$INSTANCE_NAME`_UDB                        (0x00u)
#define `$INSTANCE_NAME`_FF                         (0x01u)

/* I2C modes */
#define `$INSTANCE_NAME`_MODE_SLAVE                 (0x01u) /* I2C Slave Mode */
#define `$INSTANCE_NAME`_MODE_MASTER                (0x02u) /* I2C Master Mode */
#define `$INSTANCE_NAME`_MODE_MULTI_MASTER          (0x06u) /* I2C Multi-Master Mode */
#define `$INSTANCE_NAME`_MODE_MULTI_MASTER_SLAVE    (0x07u) /* I2C Multi-Master Slave Mode */
#define `$INSTANCE_NAME`_MODE_MULTI_MASTER_ENABLE   (0x04u) /* I2C Multi-Master Mode enable */

/* Address detection */
#define `$INSTANCE_NAME`_SW_DECODE                  (0x00u) /* Software address decode type */
#define `$INSTANCE_NAME`_HDWR_DECODE                (0x01u) /* Hardware address decode type */

#define `$INSTANCE_NAME`_I2C_PAIR0                  (0x01u) /* SIO pair 0 - P12[0] & P12[1] */
#define `$INSTANCE_NAME`_I2C_PAIR1                  (0x02u) /* SIO pair 1 - P12[4] & P12[5] */

/* PSoC3 ES2 or early */
#define `$INSTANCE_NAME`_PSOC3_ES2  ( (CYDEV_CHIP_MEMBER_USED == CYDEV_CHIP_MEMBER_3A) && \
                                      (CYDEV_CHIP_REVISION_USED <= CYDEV_CHIP_REVISION_3A_ES2) )
/* PSoC5 ES1 or early */
#define `$INSTANCE_NAME`_PSOC5_ES1  ( (CYDEV_CHIP_MEMBER_USED == CYDEV_CHIP_MEMBER_5A) && \
                                      (CYDEV_CHIP_REVISION_USED <= CYDEV_CHIP_REVISION_5A_ES1) )
/* PSoC3 ES3 or later */
#define `$INSTANCE_NAME`_PSOC3_ES3  ( (CYDEV_CHIP_MEMBER_USED == CYDEV_CHIP_MEMBER_3A) && \
                                      (CYDEV_CHIP_REVISION_USED >= CYDEV_CHIP_REVISION_3A_ES3) )
/* PSoC5 ES2 or later */
#define `$INSTANCE_NAME`_PSOC5_ES2  ( (CYDEV_CHIP_MEMBER_USED == CYDEV_CHIP_MEMBER_5A) && \
                                      (CYDEV_CHIP_REVISION_USED > CYDEV_CHIP_REVISION_5A_ES1) )


/***************************************
*       Type defines
***************************************/

/* Structure to save registers before go to sleep */
typedef struct _`$INSTANCE_NAME`_BACKUP_STRUCT
{
    uint8 enableState;
        
    #if((`$INSTANCE_NAME`_IMPLEMENTATION == `$INSTANCE_NAME`_FF) && (`$INSTANCE_NAME`_ENABLE_WAKEUP == 0u))
        uint8 xcfg;
        uint8 cfg;

        #if (`$INSTANCE_NAME`_MODE & `$INSTANCE_NAME`_MODE_SLAVE)
            uint8 addr;
        #endif  /* End (`$INSTANCE_NAME`_MODE & `$INSTANCE_NAME`_MODE_SLAVE) */
        
        #if (`$INSTANCE_NAME`_PSOC3_ES2 || `$INSTANCE_NAME`_PSOC5_ES1)
            uint8   clk_div;        /* only for TO3 */
        #else
            uint8   clk_div1;       /* only for TO4 */
            uint8   clk_div2;       /* only for TO4 */
        #endif  /* End  (`$INSTANCE_NAME`_PSOC3_ES2 || `$INSTANCE_NAME`_PSOC5_ES1) */
        
    #else
        #if(`$INSTANCE_NAME`_PSOC3_ES2 || `$INSTANCE_NAME`_PSOC5_ES1)
            uint8 int_mask;         /* Status interrupt mask register */
            
            #if(`$INSTANCE_NAME`_MODE & `$INSTANCE_NAME`_MODE_SLAVE)
                uint8 addr;         /* D0 register */
                                    /* Auxiliary Control register, clears on Stop() */
                                    /* Auxiliary Status register, disables after _Start() */
                                    /* Period Register = `$INSTANCE_NAME`_PERIOD_VALUE */
            #endif  /* End (`$INSTANCE_NAME`_MODE & `$INSTANCE_NAME`_MODE_SLAVE) */
        #else
            /* ES3 Saves: 
                - Status Int mask: int_mask;
                - D0 register: addr;
                - Auxiliary Control: aux_ctl;
                - Period Register;
            */
        #endif  /* End (`$INSTANCE_NAME`_PSOC3_ES2 || `$INSTANCE_NAME`_PSOC5_ES1)*/
        
    #endif  /* End (`$INSTANCE_NAME`_IMPLEMENTATION == `$INSTANCE_NAME`_FF)*/
    
} `$INSTANCE_NAME`_BACKUP_STRUCT;


/***************************************
*        Function Prototypes 
***************************************/

void `$INSTANCE_NAME`_Init(void);
void `$INSTANCE_NAME`_Enable(void) `=ReentrantKeil($INSTANCE_NAME . "_Enable")`;

void `$INSTANCE_NAME`_Start(void);
void `$INSTANCE_NAME`_Stop(void) `=ReentrantKeil($INSTANCE_NAME . "_Stop")`;
void `$INSTANCE_NAME`_EnableInt(void) `=ReentrantKeil($INSTANCE_NAME . "_EnableInt")` ;
void `$INSTANCE_NAME`_DisableInt(void) `=ReentrantKeil($INSTANCE_NAME . "_DisableInt")`;

void `$INSTANCE_NAME`_SaveConfig(void);
void `$INSTANCE_NAME`_Sleep(void);
void `$INSTANCE_NAME`_RestoreConfig(void) `=ReentrantKeil($INSTANCE_NAME . "_RestoreConfig")`;
void `$INSTANCE_NAME`_Wakeup(void) `=ReentrantKeil($INSTANCE_NAME . "_Wakeup")`;

/* I2C Master functions prototypes */
#if(`$INSTANCE_NAME`_MODE & `$INSTANCE_NAME`_MODE_MASTER)
    /* Read and Clear status functions */
    uint8   `$INSTANCE_NAME`_MasterStatus(void) `=ReentrantKeil($INSTANCE_NAME . "_MasterStatus")`;
    uint8   `$INSTANCE_NAME`_MasterClearStatus(void);
    
    /* Interrupt based operation functions */
    uint8   `$INSTANCE_NAME`_MasterWriteBuf(uint8 slaveAddr, uint8 * wrData, uint8 cnt, uint8 mode);
    uint8   `$INSTANCE_NAME`_MasterReadBuf(uint8 slaveAddr, uint8 * rdData, uint8 cnt, uint8 mode);
    uint16  `$INSTANCE_NAME`_MasterGetReadBufSize(void) `=ReentrantKeil($INSTANCE_NAME . "_MasterGetReadBufSize")`;
    uint16  `$INSTANCE_NAME`_MasterGetWriteBufSize(void)  `=ReentrantKeil($INSTANCE_NAME . "_MasterGetWriteBufSize")`;
    void    `$INSTANCE_NAME`_MasterClearReadBuf(void);
    void    `$INSTANCE_NAME`_MasterClearWriteBuf(void);
    
    /* Manual operation functions */
    uint8   `$INSTANCE_NAME`_MasterSendStart(uint8 slaveAddress, uint8 R_nW);
    uint8   `$INSTANCE_NAME`_MasterSendRestart(uint8 slaveAddress, uint8 R_nW);
    uint8   `$INSTANCE_NAME`_MasterSendStop(void);
    uint8   `$INSTANCE_NAME`_MasterWriteByte(uint8 theByte);
    uint8   `$INSTANCE_NAME`_MasterReadByte(uint8 acknNak);
    
    /* This fake function use as workaround for CDT 78083 */
    void `$INSTANCE_NAME`_Workaround(void);
    
#endif  /* End (`$INSTANCE_NAME`_MODE & `$INSTANCE_NAME`_MODE_MASTER) */

/* I2C Slave functions prototypes */
#if(`$INSTANCE_NAME`_MODE & `$INSTANCE_NAME`_MODE_SLAVE)
    /* Read and Clear status functions */
    uint8 `$INSTANCE_NAME`_SlaveStatus(void) `=ReentrantKeil($INSTANCE_NAME . "_SlaveStatus")`;
    uint8 `$INSTANCE_NAME`_SlaveClearReadStatus(void);
    uint8 `$INSTANCE_NAME`_SlaveClearWriteStatus(void);
    #if (`$INSTANCE_NAME`_ADDR_DECODE == `$INSTANCE_NAME`_HDWR_DECODE)
        void `$INSTANCE_NAME`_SlaveSetAddress(uint8 address) `=ReentrantKeil($INSTANCE_NAME . "_SlaveSetAddress")`;
    #else
        void `$INSTANCE_NAME`_SlaveSetAddress(uint8 address);
    #endif  /* End (`$INSTANCE_NAME`_ADDR_DECODE == `$INSTANCE_NAME`_HDWR_DECODE) */ 
    
    /* Interrupt based operation functions */
    void `$INSTANCE_NAME`_SlaveInitReadBuf(uint8 * rdBuf, uint8 bufSize);
    void `$INSTANCE_NAME`_SlaveInitWriteBuf(uint8 * wrBuf, uint8 bufSize);
    uint8 `$INSTANCE_NAME`_SlaveGetReadBufSize(void) `=ReentrantKeil($INSTANCE_NAME . "_SlaveGetReadBufSize")`;
    uint8 `$INSTANCE_NAME`_SlaveGetWriteBufSize(void) `=ReentrantKeil($INSTANCE_NAME . "_SlaveGetWriteBufSize")`;
    void `$INSTANCE_NAME`_SlaveClearReadBuf(void);
    void `$INSTANCE_NAME`_SlaveClearWriteBuf(void);
    
    void `$INSTANCE_NAME`_SlavePutReadByte(uint8 transmitDataByte) `=ReentrantKeil($INSTANCE_NAME . "_SlavePutReadByte")`;
    uint8 `$INSTANCE_NAME`_SlaveGetWriteByte(uint8 ackNak) `=ReentrantKeil($INSTANCE_NAME . "_SlaveGetWriteByte")`;

    /* Communication bootloader I2C Slave APIs */
    #if defined(CYDEV_BOOTLOADER_IO_COMP) && ((CYDEV_BOOTLOADER_IO_COMP == CyBtldr_`@INSTANCE_NAME`) || \
                                              (CYDEV_BOOTLOADER_IO_COMP == CyBtldr_Custom_Interface))   
        /* Physical layer functions */
        void `$INSTANCE_NAME`_CyBtldrCommStart(void);
        void `$INSTANCE_NAME`_CyBtldrCommStop(void);
        void `$INSTANCE_NAME`_CyBtldrCommReset(void);
        cystatus `$INSTANCE_NAME`_CyBtldrCommWrite(uint8 * Data, uint16 Size, uint16 * Count, uint8 TimeOut);
        cystatus `$INSTANCE_NAME`_CyBtldrCommRead(uint8 * Data, uint16 Size, uint16 * Count, uint8 TimeOut);
        
        /* Size of Read/Write buffers for I2C bootloader  */
        #define `$INSTANCE_NAME`_BTLDR_SIZEOF_READ_BUFFER   (0x80u)
        #define `$INSTANCE_NAME`_BTLDR_SIZEOF_WRITE_BUFFER  (0x80u)
        
        #if (CYDEV_BOOTLOADER_IO_COMP == CyBtldr_`@INSTANCE_NAME`)
            #define CyBtldrCommStart    `$INSTANCE_NAME`_CyBtldrCommStart
            #define CyBtldrCommStop     `$INSTANCE_NAME`_CyBtldrCommStop
            #define CyBtldrCommReset    `$INSTANCE_NAME`_CyBtldrCommReset
            #define CyBtldrCommWrite    `$INSTANCE_NAME`_CyBtldrCommWrite
            #define CyBtldrCommRead     `$INSTANCE_NAME`_CyBtldrCommRead
        #endif  /* End (CYDEV_BOOTLOADER_IO_COMP == CyBtldr_`@INSTANCE_NAME`)*/
        
    #endif  /* End defined(CYDEV_BOOTLOADER_IO_COMP) && ((CYDEV_BOOTLOADER_IO_COMP == CyBtldr_`@INSTANCE_NAME`) || \
                                                         (CYDEV_BOOTLOADER_IO_COMP == CyBtldr_Custom_Interface))*/

#endif  /* End (`$INSTANCE_NAME`_MODE & `$INSTANCE_NAME`_MODE_SLAVE) */

/* Interrupt handler */
CY_ISR_PROTO(`$INSTANCE_NAME`_ISR);

#if(`$INSTANCE_NAME`_PSOC3_ES2 && (`$INSTANCE_NAME`_I2C_IRQ__ES2_PATCH))
    #include <intrins.h>
    #define `$INSTANCE_NAME`_ISR_PATCH() _nop_(); _nop_(); _nop_(); _nop_(); _nop_(); _nop_(); _nop_(); _nop_();
#endif  /* End (`$INSTANCE_NAME`_PSOC3_ES2 && (`$INSTANCE_NAME`_I2C_IRQ__ES2_PATCH)) */


/***************************************
*   Initial Parameter Constants 
***************************************/

#define `$INSTANCE_NAME`_DEFAULT_ADDR               (`$Slave_Address`u) 
#define `$INSTANCE_NAME`_BUS_SPEED                  (`$BusSpeed_kHz`u)

#define `$INSTANCE_NAME`_BUS_SPEED_50KHZ            (50u)
#define `$INSTANCE_NAME`_BUS_SPEED_100KHZ           (100u)

/*
    CLK_DIV = BUS_CLK(kHz) / (BusSpeed * OversampleRate);
    For Slave picks up the grater unsigned integer.
    For Master/MultiMaster/MultiMaster-Slave picks up the smallest unsigned integer(round).
    The OversampleRate equal 16 for BusSpeed >= 100, for others 32 (truncate).
    The real BusSpeed could be differ from desired due division with round/truncate.
*/
#if(`$INSTANCE_NAME`_PSOC3_ES2 || `$INSTANCE_NAME`_PSOC5_ES1)
    /* Define CLK_DIV */
    #define `$INSTANCE_NAME`_DEFAULT_DIVIDE_FACTOR      (`$ClkDiv`u)
    
    /* Define proper clock rate according to Bus Speed */
    #if (`$INSTANCE_NAME`_BUS_SPEED <= `$INSTANCE_NAME`_BUS_SPEED_50KHZ)
        #define `$INSTANCE_NAME`_DEFAULT_CLK_RATE       `$INSTANCE_NAME`_CFG_CLK_RATE_050
    #elif (`$INSTANCE_NAME`_BUS_SPEED <= `$INSTANCE_NAME`_BUS_SPEED_100KHZ)
        #define `$INSTANCE_NAME`_DEFAULT_CLK_RATE       `$INSTANCE_NAME`_CFG_CLK_RATE_100
    #else
        #define `$INSTANCE_NAME`_DEFAULT_CLK_RATE       `$INSTANCE_NAME`_CFG_CLK_RATE_400
    #endif  /* End (`$INSTANCE_NAME`_BUS_SPEED <= `$INSTANCE_NAME`_BUS_SPEED_50KHZ) */
    
#else
    /* Define CLK_DIV1 and CLK_DIV2 */
    #define `$INSTANCE_NAME`_DEFAULT_DIVIDE_FACTOR      ((uint16) `$ClkDiv1`u )
    
    /* Define proper clock rate accordinf to Bus Speed */
    #if (`$INSTANCE_NAME`_BUS_SPEED <= `$INSTANCE_NAME`_BUS_SPEED_50KHZ)
        #define `$INSTANCE_NAME`_DEFAULT_CLK_RATE       `$INSTANCE_NAME`_CFG_CLK_RATE_LESS_EQUAL_50
    #else
        #define `$INSTANCE_NAME`_DEFAULT_CLK_RATE       `$INSTANCE_NAME`_CFG_CLK_RATE_GRATER_50
    #endif  /* End (`$INSTANCE_NAME`_BUS_SPEED <= `$INSTANCE_NAME`_BUS_SPEED_50KHZ) */
    
#endif /* End (I2C_PSOC3_ES2 || I2C_PSOC5_ES1) */


/***************************************
* I2C state machine constants 
***************************************/

/* Default slave address states */
#define  `$INSTANCE_NAME`_DEV_MASK                  (0xF0u)    /* Wait for sub-address */
#define  `$INSTANCE_NAME`_SM_IDLE                   (0x10u)    /* Idle I2C state */
#define  `$INSTANCE_NAME`_DEV_MASTER_XFER           (0x40u)    /* Wait for sub-address */

/* Default slave address states */
#define  `$INSTANCE_NAME`_SM_SL_WR_IDLE             (0x10u)    /* Slave Idle, waiting for start */
#define  `$INSTANCE_NAME`_SM_SL_WR_DATA             (0x11u)    /* Slave waiting for master to write data */
#define  `$INSTANCE_NAME`_SM_SL_RD_DATA             (0x12u)    /* Slave waiting for master to read data */
#define  `$INSTANCE_NAME`_SM_SL_STOP                (0x14u)    /* Slave waiting for stop */

/* Master mode states */
#define  `$INSTANCE_NAME`_SM_MASTER                 (0x40u)    /* Master or Multi-Master mode is set */
#define  `$INSTANCE_NAME`_SM_MASTER_IDLE            (0x40u)    /* Hardware in Master mode and sitting idle */

#define  `$INSTANCE_NAME`_SM_MSTR_ADDR              (0x43u)    /* Master has sent Start/Address */
#define  `$INSTANCE_NAME`_SM_MSTR_WR_ADDR           (0x42u)    /* Master has sent a Start/Address/WR */
#define  `$INSTANCE_NAME`_SM_MSTR_RD_ADDR           (0x43u)    /* Master has sent a Start/Address/RD */

#define  `$INSTANCE_NAME`_SM_MSTR_DATA              (0x44u)    /* Master is writing data to external slave */
#define  `$INSTANCE_NAME`_SM_MSTR_WR_DATA           (0x44u)    /* Master is writing data to external slave */
#define  `$INSTANCE_NAME`_SM_MSTR_RD_DATA           (0x45u)    /* Master is receiving data from external slave */

#define  `$INSTANCE_NAME`_SM_MSTR_WAIT_STOP         (0x48u)    /* Master Send Stop */

#define  `$INSTANCE_NAME`_SM_MSTR_HALT              (0x60u)    /* Master Halt state */


/***************************************
*            API Constants        
***************************************/

/* Master/Slave control constants */
#define `$INSTANCE_NAME`_READ_XFER_MODE             (0x01u)    /* Read */
#define `$INSTANCE_NAME`_WRITE_XFER_MODE            (0x00u)    /* Write */
#define `$INSTANCE_NAME`_ACK_DATA                   (0x01u)    /* Send ACK */
#define `$INSTANCE_NAME`_NAK_DATA                   (0x00u)    /* Send NAK */
    
#if(`$INSTANCE_NAME`_MODE & `$INSTANCE_NAME`_MODE_MASTER)

    /* "Mode" constants for MasterWriteBuf() or MasterReadBuf() function */
    #define `$INSTANCE_NAME`_MODE_COMPLETE_XFER     (0x00u)    /* Full transfer with Start and Stop */
    #define `$INSTANCE_NAME`_MODE_REPEAT_START      (0x01u)    /* Begin with a ReStart instead of a Start */
    #define `$INSTANCE_NAME`_MODE_NO_STOP           (0x02u)    /* Complete the transfer without a Stop */

    /* Master status */
    #define `$INSTANCE_NAME`_MSTAT_CLEAR            (0x00u)    /* Clear (init) status value */
    
    #define `$INSTANCE_NAME`_MSTAT_RD_CMPLT         (0x01u)    /* Read complete */
    #define `$INSTANCE_NAME`_MSTAT_WR_CMPLT         (0x02u)    /* Write complete */
    #define `$INSTANCE_NAME`_MSTAT_XFER_INP         (0x04u)    /* Master transfer in progress */
    #define `$INSTANCE_NAME`_MSTAT_XFER_HALT        (0x08u)    /* Transfer is halted */
    
    #define `$INSTANCE_NAME`_MSTAT_ERR_MASK         (0xF0u)    /* Mask for all errors */
    #define `$INSTANCE_NAME`_MSTAT_ERR_SHORT_XFER   (0x10u)    /* Master NAKed before end of packet */
    #define `$INSTANCE_NAME`_MSTAT_ERR_ADDR_NAK     (0x20u)    /* Slave did not ACK */
    #define `$INSTANCE_NAME`_MSTAT_ERR_ARB_LOST     (0x40u)    /* Master lost arbitration during communication */
    #define `$INSTANCE_NAME`_MSTAT_ERR_XFER         (0x80u)    /* Error during transfer */
    #define `$INSTANCE_NAME`_MSTAT_ERR_BUF_OVFL     (0x80u)    /* Buffer overflow/underflow */
    
    /* Master API returns */
    #define `$INSTANCE_NAME`_MSTR_NO_ERROR          (0x00u)    /* Function complete without error */
    #define `$INSTANCE_NAME`_MSTR_BUS_BUSY          (0x01u)    /* Bus is busy, process not started */
    #define `$INSTANCE_NAME`_MSTR_SLAVE_BUSY        (0x02u)    /* Master not Master on the bus or 
                                                                  Slave operation in progress */
    #define `$INSTANCE_NAME`_MSTR_ERR_LB_NAK        (0x03u)    /* Last Byte Naked */
    #define `$INSTANCE_NAME`_MSTR_ERR_ARB_LOST      (0x04u)    /* Master lost arbitration during communication */
    
    /* mstrControl bit definitions */
    #define  `$INSTANCE_NAME`_MSTR_GEN_STOP         (0x01u)    /* Generate a stop after a data transfer */
    #define  `$INSTANCE_NAME`_MSTR_NO_STOP          (0x01u)    /* Do not generate a stop after a data transfer */
    
    #define `$INSTANCE_NAME`_READ_FLAG              (0x01u)     /* Read flag of the Address */
    
#endif  /* End (`$INSTANCE_NAME`_MODE & `$INSTANCE_NAME`_MODE_MASTER) */

#if(`$INSTANCE_NAME`_MODE & `$INSTANCE_NAME`_MODE_SLAVE)

    /* Slave Status Constants */
    #define `$INSTANCE_NAME`_SSTAT_RD_CMPT          (0x01u)    /* Read transfer complete */
    #define `$INSTANCE_NAME`_SSTAT_RD_BUSY          (0x02u)    /* Read transfer in progress */
    #define `$INSTANCE_NAME`_SSTAT_RD_ERR           (0x08u)    /* Read Error buffer */
    #define `$INSTANCE_NAME`_SSTAT_RD_NO_ERR        (0x00u)    /* Read no Error */
    #define `$INSTANCE_NAME`_SSTAT_RD_ERR_OVFL      (0x04u)    /* Read overflow Error */
    #define `$INSTANCE_NAME`_SSTAT_RD_MASK          (0x0Fu)    /* Read Status Mask */
    
    #define `$INSTANCE_NAME`_SSTAT_WR_CMPT          (0x10u)    /* Write transfer complete */
    #define `$INSTANCE_NAME`_SSTAT_WR_BUSY          (0x20u)    /* Write transfer in progress */
    #define `$INSTANCE_NAME`_SSTAT_WR_ERR           (0xC0u)    /* Write Error buffer */
    #define `$INSTANCE_NAME`_SSTAT_WR_NO_ERR        (0x00u)    /* Write no Error */
    #define `$INSTANCE_NAME`_SSTAT_WR_ERR_OVFL      (0x40u)    /* Write overflow Error */
    #define `$INSTANCE_NAME`_SSTAT_WR_MASK          (0xF0u)    /* Write Status Mask  */
    
    #define `$INSTANCE_NAME`_SSTAT_RD_CLEAR         (0x0Du)    /* Read Status clear */
    #define `$INSTANCE_NAME`_SSTAT_WR_CLEAR         (0xD0u)    /* Write Status Clear */
        
#endif  /* End (`$INSTANCE_NAME`_MODE & `$INSTANCE_NAME`_MODE_SLAVE) */


/***************************************
*              Registers
***************************************/

#if(`$INSTANCE_NAME`_IMPLEMENTATION == `$INSTANCE_NAME`_FF)

    /* Fixed Function registers */
    #define `$INSTANCE_NAME`_XCFG_REG               (*(reg8 *) `$INSTANCE_NAME`_I2C_FF__XCFG )
    #define `$INSTANCE_NAME`_XCFG_PTR               ( (reg8 *) `$INSTANCE_NAME`_I2C_FF__XCFG )
    
    #define `$INSTANCE_NAME`_ADDR_REG               (*(reg8 *) `$INSTANCE_NAME`_I2C_FF__ADR )
    #define `$INSTANCE_NAME`_ADDR_PTR               ( (reg8 *) `$INSTANCE_NAME`_I2C_FF__ADR )
    
    #define `$INSTANCE_NAME`_CFG_REG                (*(reg8 *) `$INSTANCE_NAME`_I2C_FF__CFG )
    #define `$INSTANCE_NAME`_CFG_PTR                ( (reg8 *) `$INSTANCE_NAME`_I2C_FF__CFG )
    
    #define `$INSTANCE_NAME`_CSR_REG                (*(reg8 *) `$INSTANCE_NAME`_I2C_FF__CSR )
    #define `$INSTANCE_NAME`_CSR_PTR                ( (reg8 *) `$INSTANCE_NAME`_I2C_FF__CSR )
    
    #define `$INSTANCE_NAME`_DATA_REG               (*(reg8 *) `$INSTANCE_NAME`_I2C_FF__D )
    #define `$INSTANCE_NAME`_DATA_PTR               ( (reg8 *) `$INSTANCE_NAME`_I2C_FF__D )
    
    #define `$INSTANCE_NAME`_MCSR_REG               (*(reg8 *) `$INSTANCE_NAME`_I2C_FF__MCSR )
    #define `$INSTANCE_NAME`_MCSR_PTR               ( (reg8 *) `$INSTANCE_NAME`_I2C_FF__MCSR )
    
    #define `$INSTANCE_NAME`_ACT_PWRMGR_REG         (*(reg8 *) `$INSTANCE_NAME`_I2C_FF__PM_ACT_CFG )
    #define `$INSTANCE_NAME`_ACT_PWRMGR_PTR         ( (reg8 *) `$INSTANCE_NAME`_I2C_FF__PM_ACT_CFG )
    #define `$INSTANCE_NAME`_ACT_PWR_EN                        `$INSTANCE_NAME`_I2C_FF__PM_ACT_MSK
    
    #define `$INSTANCE_NAME`_STBY_PWRMGR_REG        (*(reg8 *) `$INSTANCE_NAME`_I2C_FF__PM_STBY_CFG )
    #define `$INSTANCE_NAME`_STBY_PWRMGR_PTR        ( (reg8 *) `$INSTANCE_NAME`_I2C_FF__PM_STBY_CFG ) 
    #define `$INSTANCE_NAME`_STBY_PWR_EN                       `$INSTANCE_NAME`_I2C_FF__PM_STBY_MSK
    
    #define `$INSTANCE_NAME`_PWRSYS_CR1_REG         (*(reg8 *) CYREG_PWRSYS_CR1 )
    #define `$INSTANCE_NAME`_PWRSYS_CR1_PTR         ( (reg8 *) CYREG_PWRSYS_CR1 )
    
    /* Clock divider register depends on silicon */
    #if(`$INSTANCE_NAME`_PSOC3_ES2 || `$INSTANCE_NAME`_PSOC5_ES1)
        #define `$INSTANCE_NAME`_CLKDIV_REG         (*(reg8 *) `$INSTANCE_NAME`_I2C_FF__CLK_DIV )
        #define `$INSTANCE_NAME`_CLKDIV_PTR         ( (reg8 *) `$INSTANCE_NAME`_I2C_FF__CLK_DIV )
        
    #else
        #define `$INSTANCE_NAME`_CLKDIV1_REG        (*(reg8 *) `$INSTANCE_NAME`_I2C_FF__CLK_DIV1 )
        #define `$INSTANCE_NAME`_CLKDIV1_PTR        ( (reg8 *) `$INSTANCE_NAME`_I2C_FF__CLK_DIV1 )
        #define `$INSTANCE_NAME`_CLKDIV2_REG        (*(reg8 *) `$INSTANCE_NAME`_I2C_FF__CLK_DIV2 )
        #define `$INSTANCE_NAME`_CLKDIV2_PTR        ( (reg8 *) `$INSTANCE_NAME`_I2C_FF__CLK_DIV2 )
        
    #endif  /* End (`$INSTANCE_NAME`_PSOC3_ES2 || `$INSTANCE_NAME`_PSOC5_ES1)*/
    
#else

    /* UDB implementation registers */
    #define `$INSTANCE_NAME`_CFG_REG    (*(reg8 *) \
                                           `$INSTANCE_NAME`_bI2C_UDB_`$CtlModeReplacementString`_CtrlReg__CONTROL_REG )
    #define `$INSTANCE_NAME`_CFG_PTR    ( (reg8 *) \
                                           `$INSTANCE_NAME`_bI2C_UDB_`$CtlModeReplacementString`_CtrlReg__CONTROL_REG )
    
    #define `$INSTANCE_NAME`_CSR_REG                (*(reg8 *) `$INSTANCE_NAME`_bI2C_UDB_StsReg__STATUS_REG )
    #define `$INSTANCE_NAME`_CSR_PTR                ( (reg8 *) `$INSTANCE_NAME`_bI2C_UDB_StsReg__STATUS_REG )
    
    #define `$INSTANCE_NAME`_INT_MASK_REG           (*(reg8 *) `$INSTANCE_NAME`_bI2C_UDB_StsReg__MASK_REG )
    #define `$INSTANCE_NAME`_INT_MASK_PTR           ( (reg8 *) `$INSTANCE_NAME`_bI2C_UDB_StsReg__MASK_REG )
    
    #define `$INSTANCE_NAME`_INT_ENABLE_REG         (*(reg8 *) `$INSTANCE_NAME`_bI2C_UDB_StsReg__STATUS_AUX_CTL_REG )
    #define `$INSTANCE_NAME`_INT_ENABLE_PTR         ( (reg8 *) `$INSTANCE_NAME`_bI2C_UDB_StsReg__STATUS_AUX_CTL_REG )
    
    #define `$INSTANCE_NAME`_DATA_REG               (*(reg8 *) `$INSTANCE_NAME`_bI2C_UDB_Shifter_u0__A0_REG )
    #define `$INSTANCE_NAME`_DATA_PTR               ( (reg8 *) `$INSTANCE_NAME`_bI2C_UDB_Shifter_u0__A0_REG )
    
    #define `$INSTANCE_NAME`_GO_REG                 (*(reg8 *) `$INSTANCE_NAME`_bI2C_UDB_Shifter_u0__F1_REG )
    #define `$INSTANCE_NAME`_GO_PTR                 ( (reg8 *) `$INSTANCE_NAME`_bI2C_UDB_Shifter_u0__F1_REG )
    
    #define `$INSTANCE_NAME`_MCLK_PRD_REG           (*(reg8 *) `$INSTANCE_NAME`_bI2C_UDB_Master_ClkGen_u0__D0_REG )
    #define `$INSTANCE_NAME`_MCLK_PRD_PTR           ( (reg8 *) `$INSTANCE_NAME`_bI2C_UDB_Master_ClkGen_u0__D0_REG )
    
    #define `$INSTANCE_NAME`_MCLK_CMP_REG           (*(reg8 *) `$INSTANCE_NAME`_bI2C_UDB_Master_ClkGen_u0__D1_REG )
    #define `$INSTANCE_NAME`_MCLK_CMP_PTR           ( (reg8 *) `$INSTANCE_NAME`_bI2C_UDB_Master_ClkGen_u0__D1_REG )
    
    #if(`$INSTANCE_NAME`_MODE & `$INSTANCE_NAME`_MODE_SLAVE)          
        
        /* UDB implementation registers --- Slave only */
        #define `$INSTANCE_NAME`_ADDR_REG       (*(reg8 *) `$INSTANCE_NAME`_bI2C_UDB_Shifter_u0__D0_REG )
        #define `$INSTANCE_NAME`_ADDR_PTR       ( (reg8 *) `$INSTANCE_NAME`_bI2C_UDB_Shifter_u0__D0_REG )
                                                                                              
        #define `$INSTANCE_NAME`_PERIOD_REG     (*(reg8 *) `$INSTANCE_NAME`_bI2C_UDB_Slave_BitCounter__PERIOD_REG )
        #define `$INSTANCE_NAME`_PERIOD_PTR     ( (reg8 *) `$INSTANCE_NAME`_bI2C_UDB_Slave_BitCounter__PERIOD_REG )
        
        #define `$INSTANCE_NAME`_COUNTER_REG    (*(reg8 *) `$INSTANCE_NAME`_bI2C_UDB_Slave_BitCounter__COUNT_REG )
        #define `$INSTANCE_NAME`_COUNTER_PTR    ( (reg8 *) `$INSTANCE_NAME`_bI2C_UDB_Slave_BitCounter__COUNT_REG )
        
        #define `$INSTANCE_NAME`_COUNTER_AUX_CTL_REG  (*(reg8 *) \
                                                       `$INSTANCE_NAME`_bI2C_UDB_Slave_BitCounter__CONTROL_AUX_CTL_REG )
        #define `$INSTANCE_NAME`_COUNTER_AUX_CTL_PTR  ( (reg8 *) \
                                                       `$INSTANCE_NAME`_bI2C_UDB_Slave_BitCounter__CONTROL_AUX_CTL_REG )
        
    #endif  /* End (`$INSTANCE_NAME`_MODE & `$INSTANCE_NAME`_MODE_SLAVE) */
    
#endif  /* End (`$INSTANCE_NAME`_IMPLEMENTATION == `$INSTANCE_NAME`_FF) */


/***************************************
*        Registers Constants       
***************************************/ 

/* Number and priority of the I2C interrupt */
#define `$INSTANCE_NAME`_ISR_NUMBER                 `$INSTANCE_NAME`_I2C_IRQ__INTC_NUMBER
#define `$INSTANCE_NAME`_ISR_PRIORITY               `$INSTANCE_NAME`_I2C_IRQ__INTC_PRIOR_NUM

/* I2C Slave Data Register */
#define `$INSTANCE_NAME`_SLAVE_ADDR_MASK            (0x7Fu)
#define `$INSTANCE_NAME`_DATA_MASK                  (0xFFu)
#define `$INSTANCE_NAME`_READ_FLAG                  (0x01u)
#if(`$INSTANCE_NAME`_IMPLEMENTATION == `$INSTANCE_NAME`_FF)
    
    /* XCFG I2C Extended Configuration Register */
    #define `$INSTANCE_NAME`_XCFG_CLK_EN            (0x80u)    /* Enable gated clock to block */
    #define `$INSTANCE_NAME`_XCFG_I2C_ON            (0x40u)    /* Enable I2C as wake up source*/
    #define `$INSTANCE_NAME`_XCFG_RDY_TO_SLEEP      (0x20u)    /* I2C ready go to sleep */
    #define `$INSTANCE_NAME`_XCFG_FORCE_NACK        (0x10u)    /* Force NACK all incomming transactions */
    #define `$INSTANCE_NAME`_XCFG_NO_BC_INT         (0x08u)    /* No interrupt on byte complete */
    #define `$INSTANCE_NAME`_XCFG_BUF_MODE          (0x02u)    /* Enable buffer mode */
    #define `$INSTANCE_NAME`_XCFG_HDWR_ADDR_EN      (0x01u)    /* Enable Hardware address match */
    
    /* CFG I2C Configuration Register */
    #define `$INSTANCE_NAME`_CFG_SIO_SELECT         (0x80u)    /* Pin Select for SCL/SDA lines */
    #define `$INSTANCE_NAME`_CFG_PSELECT            (0x40u)    /* Pin Select */
    #define `$INSTANCE_NAME`_CFG_BUS_ERR_IE         (0x20u)    /* Bus Error Interrupt Enable */
    #define `$INSTANCE_NAME`_CFG_STOP_IE            (0x10u)    /* Enable Interrupt on STOP condition */
    #define `$INSTANCE_NAME`_CFG_STOP_ERR_IE        (0x10u)    /* Enable Interrupt on STOP condition */
    #define `$INSTANCE_NAME`_CFG_CLK_RATE_MSK       (0x0Cu)    /* Clock rate select  **CHECK**  */
    #define `$INSTANCE_NAME`_CFG_CLK_RATE_100       (0x00u)    /* Clock rate select 100K */
    #define `$INSTANCE_NAME`_CFG_CLK_RATE_400       (0x04u)    /* Clock rate select 400K */
    #define `$INSTANCE_NAME`_CFG_CLK_RATE_050       (0x08u)    /* Clock rate select 50K  */
    #define `$INSTANCE_NAME`_CFG_CLK_RATE_RSVD      (0x0Cu)    /* Clock rate select Invalid */
    #define `$INSTANCE_NAME`_CFG_EN_MSTR            (0x02u)    /* Enable Master operation */
    #define `$INSTANCE_NAME`_CFG_EN_SLAVE           (0x01u)    /* Enable Slave operation */
    
    #define `$INSTANCE_NAME`_CFG_CLK_RATE_LESS_EQUAL_50 (0x04u) /* Clock rate select <= 50kHz */
    #define `$INSTANCE_NAME`_CFG_CLK_RATE_GRATER_50     (0x00u) /* Clock rate select > 50kHz */
   
    /* CSR I2C Control and Status Register */
    #define `$INSTANCE_NAME`_CSR_BUS_ERROR          (0x80u)    /* Active high when bus error has occured */
    #define `$INSTANCE_NAME`_CSR_LOST_ARB           (0x40u)    /* Set to 1 if lost arbitration in host mode */
    #define `$INSTANCE_NAME`_CSR_STOP_STATUS        (0x20u)    /* Set if Stop has been detected */
    #define `$INSTANCE_NAME`_CSR_ACK                (0x10u)    /* ACK response */
    #define `$INSTANCE_NAME`_CSR_NAK                (0x00u)    /* NAK response */
    #define `$INSTANCE_NAME`_CSR_ADDRESS            (0x08u)    /* Set in firmware 0 = status bit, 1 Address is slave */
    #define `$INSTANCE_NAME`_CSR_TRANSMIT           (0x04u)    /* Set in firmware 1 = transmit, 0 = receive */
    #define `$INSTANCE_NAME`_CSR_LRB                (0x02u)    /* Last received bit */
    #define `$INSTANCE_NAME`_CSR_LRB_ACK            (0x00u)    /* Last received bit was an ACK */
    #define `$INSTANCE_NAME`_CSR_LRB_NAK            (0x02u)    /* Last received bit was an NAK */
    #define `$INSTANCE_NAME`_CSR_BYTE_COMPLETE      (0x01u)    /* Informs that last byte has been sent */
    #define `$INSTANCE_NAME`_CSR_STOP_GEN           (0x00u)    /* Generate a stop condition */
    #define `$INSTANCE_NAME`_CSR_RDY_TO_RD          (0x00u)    /* Set to recieve mode */
      
    /* MCSR I2C Master Control and Status Register */
    #define `$INSTANCE_NAME`_MCSR_STOP_GEN          (0x10u)    /* Firmware sets this bit to initiate a Stop condition */
    #define `$INSTANCE_NAME`_MCSR_BUS_BUSY          (0x08u)    /* Status bit, Set at Start and cleared at Stop condition */
    #define `$INSTANCE_NAME`_MCSR_MSTR_MODE         (0x04u)    /* Status bit, Set at Start and cleared at Stop condition */
    #define `$INSTANCE_NAME`_MCSR_RESTART_GEN       (0x02u)    /* Firmware sets this bit to initiate a ReStart condition */
    #define `$INSTANCE_NAME`_MCSR_START_GEN         (0x01u)    /* Firmware sets this bit to initiate a Start condition */
    
    /* CLK_DIV I2C Clock Divide Factor Register */
    #define `$INSTANCE_NAME`_CLK_DIV_MSK            (0x07u)    /* Status bit, Set at Start and cleared at Stop condition */
    #define `$INSTANCE_NAME`_CLK_DIV_1              (0x00u)    /* Divide input clock by  1 */
    #define `$INSTANCE_NAME`_CLK_DIV_2              (0x01u)    /* Divide input clock by  2 */
    #define `$INSTANCE_NAME`_CLK_DIV_4              (0x02u)    /* Divide input clock by  4 */
    #define `$INSTANCE_NAME`_CLK_DIV_8              (0x03u)    /* Divide input clock by  8 */
    #define `$INSTANCE_NAME`_CLK_DIV_16             (0x04u)    /* Divide input clock by 16 */
    #define `$INSTANCE_NAME`_CLK_DIV_32             (0x05u)    /* Divide input clock by 32 */
    #define `$INSTANCE_NAME`_CLK_DIV_64             (0x06u)    /* Divide input clock by 64 */
    
    /* PWRSYS_CR1 to handle Sleep */
    #define `$INSTANCE_NAME`_PWRSYS_CR1_I2C_REG_BACKUP      (0x04u)    /* Enables, power to I2C regs while sleep */
    
    /* UDB compatible defines */
    #define `$INSTANCE_NAME`_DISABLE_INT_ON_STOP        { `$INSTANCE_NAME`_CFG_REG &= ~`$INSTANCE_NAME`_CFG_STOP_IE; }
    #define `$INSTANCE_NAME`_ENABLE_INT_ON_STOP         { `$INSTANCE_NAME`_CFG_REG |= `$INSTANCE_NAME`_CFG_STOP_IE; }
    
    #define `$INSTANCE_NAME`_TRANSMIT_DATA              { `$INSTANCE_NAME`_CSR_REG = `$INSTANCE_NAME`_CSR_TRANSMIT; }
    #define `$INSTANCE_NAME`_ACK_AND_TRANSMIT           { `$INSTANCE_NAME`_CSR_REG = (`$INSTANCE_NAME`_CSR_ACK | \
                                                                                      `$INSTANCE_NAME`_CSR_TRANSMIT); \
                                                        }
                                                        
    #define `$INSTANCE_NAME`_NAK_AND_TRANSMIT           { `$INSTANCE_NAME`_CSR_REG = `$INSTANCE_NAME`_CSR_NAK; }
    
    /* Special case: udb needs to ack, ff needs to nak. */
    #define `$INSTANCE_NAME`_ACKNAK_AND_TRANSMIT        { `$INSTANCE_NAME`_CSR_REG  = (`$INSTANCE_NAME`_CSR_NAK | \
                                                                                       `$INSTANCE_NAME`_CSR_TRANSMIT); \
                                                        }
    
    #define `$INSTANCE_NAME`_ACK_AND_RECEIVE            { `$INSTANCE_NAME`_CSR_REG = `$INSTANCE_NAME`_CSR_ACK; }
    #define `$INSTANCE_NAME`_NAK_AND_RECEIVE            { `$INSTANCE_NAME`_CSR_REG = `$INSTANCE_NAME`_CSR_NAK; }
    #define `$INSTANCE_NAME`_READY_TO_READ              { `$INSTANCE_NAME`_CSR_REG = `$INSTANCE_NAME`_CSR_RDY_TO_RD; }
        
    #define `$INSTANCE_NAME`_GENERATE_START             { `$INSTANCE_NAME`_MCSR_REG = `$INSTANCE_NAME`_MCSR_START_GEN; }
    
    #if(`$INSTANCE_NAME`_PSOC3_ES2 || `$INSTANCE_NAME`_PSOC5_ES1)
        #define `$INSTANCE_NAME`_GENERATE_RESTART   { `$INSTANCE_NAME`_MCSR_REG = `$INSTANCE_NAME`_MCSR_RESTART_GEN; \
                                                      `$INSTANCE_NAME`_NAK_AND_RECEIVE; \
                                                    }
                                                    
        #define `$INSTANCE_NAME`_GENERATE_STOP      { `$INSTANCE_NAME`_CSR_REG = `$INSTANCE_NAME`_CSR_STOP_GEN; }
    
    #else   /* PSoC3 ES3 handlees zero lenght packets */
        #define `$INSTANCE_NAME`_GENERATE_RESTART   { `$INSTANCE_NAME`_MCSR_REG = (`$INSTANCE_NAME`_MCSR_RESTART_GEN | \
                                                                                   `$INSTANCE_NAME`_MCSR_STOP_GEN); \
                                                      `$INSTANCE_NAME`_TRANSMIT_DATA; \
                                                    }
                                                    
        #define `$INSTANCE_NAME`_GENERATE_STOP      { `$INSTANCE_NAME`_MCSR_REG = `$INSTANCE_NAME`_MCSR_STOP_GEN; \
                                                      `$INSTANCE_NAME`_TRANSMIT_DATA; }
    #endif  /* End (`$INSTANCE_NAME`_PSOC3_ES2 || `$INSTANCE_NAME`_PSOC5_ES1) */
    
    /* Conditions definitions */
    #define `$INSTANCE_NAME`_CHECK_ADDR_ACK(csr)    ( ((csr) & \
                                                      (`$INSTANCE_NAME`_CSR_LRB | `$INSTANCE_NAME`_CSR_ADDRESS)) == \
                                                      (`$INSTANCE_NAME`_CSR_LRB_ACK | `$INSTANCE_NAME`_CSR_ADDRESS) \
                                                    )
                                                    
    #define `$INSTANCE_NAME`_CHECK_ADDR_NAK(csr)    ( ((csr) & \
                                                      (`$INSTANCE_NAME`_CSR_LRB | `$INSTANCE_NAME`_CSR_ADDRESS)) == \
                                                      (`$INSTANCE_NAME`_CSR_LRB_NAK | `$INSTANCE_NAME`_CSR_ADDRESS) \
                                                    )
    
    #define `$INSTANCE_NAME`_CHECK_DATA_ACK(csr)    ( ((csr) & `$INSTANCE_NAME`_CSR_LRB) == \
                                                       `$INSTANCE_NAME`_CSR_LRB_ACK \
                                                    )

    #define `$INSTANCE_NAME`_CHECK_MASTER_MODE(mcsr)    ( ((mcsr) & `$INSTANCE_NAME`_MCSR_MSTR_MODE) != 0u )
    #define `$INSTANCE_NAME`_CHECK_BUS_FREE(mcsr)       ( ((mcsr) & `$INSTANCE_NAME`_MCSR_BUS_BUSY) == 0u )
    #define `$INSTANCE_NAME`_CHECK_LOST_ARB(csr)        ( ((csr) & `$INSTANCE_NAME`_CSR_LOST_ARB) != 0u )  
    #define `$INSTANCE_NAME`_CHECK_BYTE_COMPLETE(csr)   ( ((csr) & `$INSTANCE_NAME`_CSR_BYTE_COMPLETE) != 0u )
    #define `$INSTANCE_NAME`_WAIT_BYTE_COMPLETE(csr)    ( ((csr) & `$INSTANCE_NAME`_CSR_BYTE_COMPLETE) == 0u )
    #define `$INSTANCE_NAME`_CHECK_NO_STOP(mstrCntl)    ( ((mstrCntl) & `$INSTANCE_NAME`_MSTR_NO_STOP) != 0u )

#else
    /* Control Register Bit Locations */
    #if(`$INSTANCE_NAME`_MODE & `$INSTANCE_NAME`_MODE_MASTER)
    
        #define `$INSTANCE_NAME`_CTRL_STOP_SHIFT            `$INSTANCE_NAME`_bI2C_UDB_`$CtlModeReplacementString`_CtrlReg__6__POS
        #define `$INSTANCE_NAME`_CTRL_STOP_MASK             ( 0x01u << `$INSTANCE_NAME`_CTRL_STOP_SHIFT )
    
        #define `$INSTANCE_NAME`_CTRL_RESTART_SHIFT         `$INSTANCE_NAME`_bI2C_UDB_`$CtlModeReplacementString`_CtrlReg__5__POS
        #define `$INSTANCE_NAME`_CTRL_RESTART_MASK          ( 0x01u << `$INSTANCE_NAME`_CTRL_RESTART_SHIFT)

        #define `$INSTANCE_NAME`_CTRL_ENABLE_MASTER_SHIFT   `$INSTANCE_NAME`_bI2C_UDB_`$CtlModeReplacementString`_CtrlReg__1__POS
        #define `$INSTANCE_NAME`_CTRL_ENABLE_MASTER_MASK    ( 0x01u << `$INSTANCE_NAME`_CTRL_ENABLE_MASTER_SHIFT )
    
    #endif  /* End (`$INSTANCE_NAME`_MODE & `$INSTANCE_NAME`_MODE_MASTER) */
    
    #if(`$INSTANCE_NAME`_MODE & `$INSTANCE_NAME`_MODE_SLAVE)
               
        #define `$INSTANCE_NAME`_CTRL_ANY_ADDRESS_SHIFT     `$INSTANCE_NAME`_bI2C_UDB_`$CtlModeReplacementString`_CtrlReg__3__POS
        #define `$INSTANCE_NAME`_CTRL_ANY_ADDRESS_MASK      ( 0x01u << `$INSTANCE_NAME`_CTRL_ANY_ADDRESS_SHIFT )
     
        #define `$INSTANCE_NAME`_CTRL_ENABLE_SLAVE_SHIFT    `$INSTANCE_NAME`_bI2C_UDB_`$CtlModeReplacementString`_CtrlReg__0__POS
        #define `$INSTANCE_NAME`_CTRL_ENABLE_SLAVE_MASK     ( 0x01u << `$INSTANCE_NAME`_CTRL_ENABLE_SLAVE_SHIFT )
    
        #if(`$INSTANCE_NAME`_MODE == `$INSTANCE_NAME`_MODE_SLAVE)
            #define `$INSTANCE_NAME`_CTRL_RESTART_MASK          0x20u       /* NOT used in this mode */
        #endif  /* (`$INSTANCE_NAME`_MODE == `$INSTANCE_NAME`_MODE_SLAVE) */
        
    #endif  /* End (`$INSTANCE_NAME`_MODE & `$INSTANCE_NAME`_MODE_SLAVE) */
    
    #define `$INSTANCE_NAME`_CTRL_TRANSMIT_SHIFT        `$INSTANCE_NAME`_bI2C_UDB_`$CtlModeReplacementString`_CtrlReg__2__POS
    #define `$INSTANCE_NAME`_CTRL_TRANSMIT_MASK         ( 0x01u << `$INSTANCE_NAME`_CTRL_TRANSMIT_SHIFT )
    
    #define `$INSTANCE_NAME`_CTRL_NACK_SHIFT            `$INSTANCE_NAME`_bI2C_UDB_`$CtlModeReplacementString`_CtrlReg__4__POS
    #define `$INSTANCE_NAME`_CTRL_NACK_MASK             ( 0x01u << `$INSTANCE_NAME`_CTRL_NACK_SHIFT )

    /* Status Register Bit Locations */
    #define `$INSTANCE_NAME`_STS_LOST_ARB_SHIFT         `$INSTANCE_NAME`_bI2C_UDB_StsReg__6__POS
    #define `$INSTANCE_NAME`_STS_LOST_ARB_MASK          ( 0x01u << `$INSTANCE_NAME`_STS_LOST_ARB_SHIFT )
    
    /* NOT available in Master mode */
    #if(`$INSTANCE_NAME`_MODE != `$INSTANCE_NAME`_MODE_MASTER)
        #define `$INSTANCE_NAME`_STS_STOP_SHIFT             `$INSTANCE_NAME`_bI2C_UDB_StsReg__5__POS
        #define `$INSTANCE_NAME`_STS_STOP_MASK              ( 0x01u << `$INSTANCE_NAME`_STS_STOP_SHIFT )
    #endif  /* End (`$INSTANCE_NAME`_MODE != `$INSTANCE_NAME`_MASTER)*/

    #define `$INSTANCE_NAME`_STS_BUSY_SHIFT             `$INSTANCE_NAME`_bI2C_UDB_StsReg__4__POS
    #define `$INSTANCE_NAME`_STS_BUSY_MASK              ( 0x01u << `$INSTANCE_NAME`_STS_BUSY_SHIFT )
    
    #define `$INSTANCE_NAME`_STS_ADDR_SHIFT             `$INSTANCE_NAME`_bI2C_UDB_StsReg__3__POS
    #define `$INSTANCE_NAME`_STS_ADDR_MASK              ( 0x01u << `$INSTANCE_NAME`_STS_ADDR_SHIFT )
    
    /* NOT available in Slave mode */
    #if(`$INSTANCE_NAME`_MODE != `$INSTANCE_NAME`_MODE_SLAVE)
        #define `$INSTANCE_NAME`_STS_MASTER_MODE_SHIFT      `$INSTANCE_NAME`_bI2C_UDB_StsReg__2__POS
        #define `$INSTANCE_NAME`_STS_MASTER_MODE_MASK      ( 0x01u << `$INSTANCE_NAME`_STS_MASTER_MODE_SHIFT )
    #endif  /* End (`$INSTANCE_NAME`_MODE != `$INSTANCE_NAME`_MODE_MASTER) */
    
    #define `$INSTANCE_NAME`_STS_LRB_SHIFT              `$INSTANCE_NAME`_bI2C_UDB_StsReg__1__POS
    #define `$INSTANCE_NAME`_STS_LRB_MASK               ( 0x01u << `$INSTANCE_NAME`_STS_LRB_SHIFT )

    #define `$INSTANCE_NAME`_STS_BYTE_COMPLETE_SHIFT    `$INSTANCE_NAME`_bI2C_UDB_StsReg__0__POS
    #define `$INSTANCE_NAME`_STS_BYTE_COMPLETE_MASK     ( 0x01u << `$INSTANCE_NAME`_STS_BYTE_COMPLETE_SHIFT )
    
    /* Master clock devider values */
    #define `$INSTANCE_NAME`_MCLK_PERIOD_VALUE      (0x0Fu)
    #define `$INSTANCE_NAME`_MCLK_COMPARE_VALUE     (0x08u)
    
    /* Enable counter and interrupts mask */
    #define `$INSTANCE_NAME`_COUNTER_ENABLE_MASK    (0x20u)     /* Enable counter7 */
    #define `$INSTANCE_NAME`_INT_ENABLE_MASK        (0x10u)     /* Enable interrupts */
    /* Conter period register value */ 
    #define `$INSTANCE_NAME`_PERIOD_VALUE           (0x07u)
     
    /* Masks to enalbe interrupts from Status register */
    #define `$INSTANCE_NAME`_STOP_IE_MASK               `$INSTANCE_NAME`_STS_STOP_MASK
    #define `$INSTANCE_NAME`_BYTE_COMPLETE_IE_MASK      `$INSTANCE_NAME`_STS_BYTE_COMPLETE_MASK
    
    /* Defines to make UDB match Fixed Function */
    #define `$INSTANCE_NAME`_CSR_LOST_ARB           `$INSTANCE_NAME`_STS_LOST_ARB_MASK          /* Set if Master Lost Arbitrage while send Start */
    #define `$INSTANCE_NAME`_CSR_STOP_STATUS        `$INSTANCE_NAME`_STS_STOP_MASK              /* Set if Stop has been detected */
    #define `$INSTANCE_NAME`_CSR_BUS_ERROR          (0x00u)                                     /* NOT used - Active high when bus error has occured */
    #define `$INSTANCE_NAME`_CSR_ADDRESS            `$INSTANCE_NAME`_STS_ADDR_MASK              /* Set in firmware 0 = status bit, 1 Address is slave */
    #define `$INSTANCE_NAME`_CSR_TRANSMIT           `$INSTANCE_NAME`_CTRL_TRANSMIT_MASK         /* Set in firmware 1 = transmit, 0 = receive. */
    #define `$INSTANCE_NAME`_CSR_LRB                `$INSTANCE_NAME`_STS_LRB_MASK               /* Last received bit */
    #define `$INSTANCE_NAME`_CSR_LRB_NAK            `$INSTANCE_NAME`_STS_LRB_MASK               /* Last received bit was an NAK */
    #define `$INSTANCE_NAME`_CSR_LRB_ACK            (0x00u)                                     /* Last received bit was an ACK */
    #define `$INSTANCE_NAME`_CSR_BYTE_COMPLETE      `$INSTANCE_NAME`_STS_BYTE_COMPLETE_MASK     /* Informs that last byte has been sent */
                
    /* MCSR Registers definitions */
    #if (`$INSTANCE_NAME`_MODE != `$INSTANCE_NAME`_MODE_SLAVE)
        #define `$INSTANCE_NAME`_MCSR_REG           `$INSTANCE_NAME`_CSR_REG
        #define `$INSTANCE_NAME`_MCSR_BUS_BUSY      `$INSTANCE_NAME`_STS_BUSY_MASK
        #define `$INSTANCE_NAME`_MCSR_START_GEN     (0x00u)             /* NOT used - always is 0 */
        #define `$INSTANCE_NAME`_MCSR_RESTART_GEN   `$INSTANCE_NAME`_CTRL_RESTART_MASK  /* Generates RESTART condition */
        #define `$INSTANCE_NAME`_MCSR_MSTR_MODE     `$INSTANCE_NAME`_STS_MASTER_MODE_MASK    /* Define if Master drives the bus */
    #endif  /* End (`$INSTANCE_NAME`_MODE & `$INSTANCE_NAME`_MODE_MASTER) */
      
    /* FF compatible defines */
    #define `$INSTANCE_NAME`_DISABLE_INT_ON_STOP    { `$INSTANCE_NAME`_INT_MASK_REG &= ~`$INSTANCE_NAME`_STOP_IE_MASK; }
    #define `$INSTANCE_NAME`_ENABLE_INT_ON_STOP     { `$INSTANCE_NAME`_INT_MASK_REG |= `$INSTANCE_NAME`_STOP_IE_MASK; }
    
    
    #define `$INSTANCE_NAME`_TRANSMIT_DATA      { `$INSTANCE_NAME`_CFG_REG |= `$INSTANCE_NAME`_CTRL_TRANSMIT_MASK; \
                                                  `$INSTANCE_NAME`_CFG_REG &= ~(`$INSTANCE_NAME`_CTRL_NACK_MASK | \
                                                                                `$INSTANCE_NAME`_CTRL_RESTART_MASK); \
                                                  `$INSTANCE_NAME`_GO_REG = 0x00u; \
                                                }
    
    #define `$INSTANCE_NAME`_ACK_AND_TRANSMIT   { `$INSTANCE_NAME`_CFG_REG |= `$INSTANCE_NAME`_CTRL_TRANSMIT_MASK; \
                                                  `$INSTANCE_NAME`_CFG_REG &= ~`$INSTANCE_NAME`_CTRL_NACK_MASK; \
                                                  `$INSTANCE_NAME`_GO_REG = 0x00u; \
                                                }
    
    #define `$INSTANCE_NAME`_NAK_AND_TRANSMIT   { `$INSTANCE_NAME`_CFG_REG |= `$INSTANCE_NAME`_CTRL_NACK_MASK; \
                                                  `$INSTANCE_NAME`_CFG_REG |= `$INSTANCE_NAME`_CTRL_TRANSMIT_MASK; \
                                                  `$INSTANCE_NAME`_GO_REG = 0x00u; \
                                                }
                                                          
    /* Special case: udb needs to ack, ff needs to nak. */
    #define `$INSTANCE_NAME`_ACKNAK_AND_TRANSMIT    { `$INSTANCE_NAME`_CFG_REG |= `$INSTANCE_NAME`_CTRL_TRANSMIT_MASK; \
                                                      `$INSTANCE_NAME`_CFG_REG &= ~`$INSTANCE_NAME`_CTRL_NACK_MASK; \
                                                      `$INSTANCE_NAME`_GO_REG = 0x00; \
                                                    }
    
    #define `$INSTANCE_NAME`_ACK_AND_RECEIVE    { `$INSTANCE_NAME`_CFG_REG &= ~(`$INSTANCE_NAME`_CTRL_TRANSMIT_MASK | \
                                                                                `$INSTANCE_NAME`_CTRL_NACK_MASK); \
                                                  `$INSTANCE_NAME`_GO_REG = 0x00u; \
                                                }
    
    #define `$INSTANCE_NAME`_NAK_AND_RECEIVE    { `$INSTANCE_NAME`_CFG_REG |= `$INSTANCE_NAME`_CTRL_NACK_MASK; \
                                                  `$INSTANCE_NAME`_CFG_REG &= ~`$INSTANCE_NAME`_CTRL_TRANSMIT_MASK; \
                                                  `$INSTANCE_NAME`_GO_REG = 0x00u; \
                                                }
    
    #define `$INSTANCE_NAME`_GENERATE_START     { `$INSTANCE_NAME`_CFG_REG &= ~(`$INSTANCE_NAME`_CTRL_TRANSMIT_MASK | \
                                                                                `$INSTANCE_NAME`_CTRL_NACK_MASK | \
                                                                                `$INSTANCE_NAME`_CTRL_STOP_MASK | \
                                                                                `$INSTANCE_NAME`_CTRL_RESTART_MASK); \
                                                  `$INSTANCE_NAME`_GO_REG = 0x00u; \
                                                }
                                                                  
    #define `$INSTANCE_NAME`_GENERATE_RESTART   { `$INSTANCE_NAME`_CFG_REG |= `$INSTANCE_NAME`_CTRL_RESTART_MASK; \
                                                  `$INSTANCE_NAME`_CFG_REG |= `$INSTANCE_NAME`_CTRL_NACK_MASK; \
                                                  `$INSTANCE_NAME`_CFG_REG &= ~(`$INSTANCE_NAME`_CTRL_TRANSMIT_MASK | \
                                                                                `$INSTANCE_NAME`_CTRL_STOP_MASK); \
                                                  `$INSTANCE_NAME`_GO_REG = 0x00u; \
                                                }
    
    #define `$INSTANCE_NAME`_GENERATE_STOP      { `$INSTANCE_NAME`_CFG_REG |= `$INSTANCE_NAME`_CTRL_STOP_MASK; \
                                                  `$INSTANCE_NAME`_CFG_REG |= `$INSTANCE_NAME`_CTRL_NACK_MASK; \
                                                  `$INSTANCE_NAME`_GO_REG = 0x00u; \
                                                }
        
    #define `$INSTANCE_NAME`_READY_TO_READ      { `$INSTANCE_NAME`_CFG_REG &= ~(`$INSTANCE_NAME`_CTRL_TRANSMIT_MASK | \
                                                                                `$INSTANCE_NAME`_CTRL_NACK_MASK | \
                                                                                `$INSTANCE_NAME`_CTRL_RESTART_MASK); \
                                                  `$INSTANCE_NAME`_GO_REG = 0x00u; \
                                                }
    /* Conditions definitions */
    #define `$INSTANCE_NAME`_CHECK_ADDR_ACK(csr)  ( ((csr) & `$INSTANCE_NAME`_CSR_LRB) == `$INSTANCE_NAME`_CSR_LRB_ACK )
    #define `$INSTANCE_NAME`_CHECK_ADDR_NAK(csr)  ( ((csr) & `$INSTANCE_NAME`_CSR_LRB) == `$INSTANCE_NAME`_CSR_LRB_NAK )
    #define `$INSTANCE_NAME`_CHECK_DATA_ACK(csr)  ( ((csr) & `$INSTANCE_NAME`_CSR_LRB) == `$INSTANCE_NAME`_CSR_LRB_ACK )
    #define `$INSTANCE_NAME`_CHECK_MASTER_MODE(mcsr)    ( ((mcsr) & `$INSTANCE_NAME`_MCSR_MSTR_MODE) != 0u )
    #define `$INSTANCE_NAME`_CHECK_BUS_FREE(mcsr)       ( ((mcsr) & `$INSTANCE_NAME`_MCSR_BUS_BUSY) == 0u )
    #define `$INSTANCE_NAME`_CHECK_LOST_ARB(csr)        ( ((csr) & `$INSTANCE_NAME`_CSR_LOST_ARB) != 0u)  
    #define `$INSTANCE_NAME`_CHECK_BYTE_COMPLETE(csr)   ( ((csr) & `$INSTANCE_NAME`_CSR_BYTE_COMPLETE) != 0u )
    #define `$INSTANCE_NAME`_WAIT_BYTE_COMPLETE(csr)    ( ((csr) & `$INSTANCE_NAME`_CSR_BYTE_COMPLETE) == 0u )
    #define `$INSTANCE_NAME`_CHECK_NO_STOP(mstrCntl)    ( ((mstrCntl) & `$INSTANCE_NAME`_MSTR_NO_STOP) != 0u )
    
#endif  /* End (`$INSTANCE_NAME`_IMPLEMENTATION == `$INSTANCE_NAME`_FF)*/
    
/* Create constansts to enable slave */
#if (`$INSTANCE_NAME`_MODE & `$INSTANCE_NAME`_MODE_SLAVE)
    #if(`$INSTANCE_NAME`_IMPLEMENTATION == `$INSTANCE_NAME`_FF)
        #define `$INSTANCE_NAME`_ENABLE_SLAVE  `$INSTANCE_NAME`_CFG_EN_SLAVE
        
    #else
        #define `$INSTANCE_NAME`_ENABLE_SLAVE  `$INSTANCE_NAME`_CTRL_ENABLE_SLAVE_MASK
        
    #endif  /* End (`$INSTANCE_NAME`_IMPLEMENTATION == `$INSTANCE_NAME`_FF)*/
    
#else
    #define  `$INSTANCE_NAME`_ENABLE_SLAVE    (0u)
    
#endif  /* End (`$INSTANCE_NAME`_MODE & `$INSTANCE_NAME`_MODE_SLAVE) */

/* Create constansts to enable master */
#if (`$INSTANCE_NAME`_MODE & `$INSTANCE_NAME`_MODE_MASTER)
    #if (`$INSTANCE_NAME`_IMPLEMENTATION == `$INSTANCE_NAME`_FF)
        #define `$INSTANCE_NAME`_ENABLE_MASTER  `$INSTANCE_NAME`_CFG_EN_MSTR
        
    #else
        #define `$INSTANCE_NAME`_ENABLE_MASTER  `$INSTANCE_NAME`_CTRL_ENABLE_MASTER_MASK
        
    #endif  /* End (`$INSTANCE_NAME`_IMPLEMENTATION == `$INSTANCE_NAME`_FF) */
    
#else
    #define `$INSTANCE_NAME`_ENABLE_MASTER    (0u)
    
#endif  /* End (`$INSTANCE_NAME`_MODE & `$INSTANCE_NAME`_MODE_MASTER) */
 
#endif  /* End CY_I2C_`$INSTANCE_NAME`_H */


/* [] END OF FILE */
