/*******************************************************************************
* File Name: i2c.h
* Version 1.10
*
* Description:
*  This file provides constants and parameter values for the SCB Component.
*
* Note:
*
********************************************************************************
* Copyright 2013, Cypress Semiconductor Corporation.  All rights reserved.
* You may use this file only in accordance with the license, terms, conditions,
* disclaimers, and limitations in the end user license agreement accompanying
* the software package with which this file was provided.
*******************************************************************************/

#if !defined(CY_SCB_i2c_H)
#define CY_SCB_i2c_H

#include "cydevice_trm.h"
#include "cyfitter.h"
#include "cytypes.h"
#include "CyLib.h"


/***************************************
*  Conditional Compilation Parameters
****************************************/

#define i2c_SCB_MODE                     (1u)

/* SCB modes enum */
#define i2c_SCB_MODE_I2C                 (0x01u)
#define i2c_SCB_MODE_SPI                 (0x02u)
#define i2c_SCB_MODE_UART                (0x04u)
#define i2c_SCB_MODE_EZI2C               (0x08u)
#define i2c_SCB_MODE_UNCONFIG            (0xFFu)

/* Define run time operation mode */
#define i2c_SCB_MODE_I2C_RUNTM_CFG       (i2c_SCB_MODE_I2C       == i2c_scbMode)
#define i2c_SCB_MODE_SPI_RUNTM_CFG       (i2c_SCB_MODE_SPI       == i2c_scbMode)
#define i2c_SCB_MODE_UART_RUNTM_CFG      (i2c_SCB_MODE_UART      == i2c_scbMode)
#define i2c_SCB_MODE_EZI2C_RUNTM_CFG     (i2c_SCB_MODE_EZI2C     == i2c_scbMode)
#define i2c_SCB_MODE_UNCONFIG_RUNTM_CFG  (i2c_SCB_MODE_UNCONFIG  == i2c_scbMode)

/* Condition compilation depends on operation mode: unconfigured implies apply to all modes */
#define i2c_SCB_MODE_I2C_CONST_CFG       (i2c_SCB_MODE_I2C       == i2c_SCB_MODE)
#define i2c_SCB_MODE_SPI_CONST_CFG       (i2c_SCB_MODE_SPI       == i2c_SCB_MODE)
#define i2c_SCB_MODE_UART_CONST_CFG      (i2c_SCB_MODE_UART      == i2c_SCB_MODE)
#define i2c_SCB_MODE_EZI2C_CONST_CFG     (i2c_SCB_MODE_EZI2C     == i2c_SCB_MODE)
#define i2c_SCB_MODE_UNCONFIG_CONST_CFG  (i2c_SCB_MODE_UNCONFIG  == i2c_SCB_MODE)

/* Condition compilation for includes */
#define i2c_SCB_MODE_I2C_INC       (0u !=(i2c_SCB_MODE_I2C       & i2c_SCB_MODE))
#define i2c_SCB_MODE_SPI_INC       (0u !=(i2c_SCB_MODE_SPI       & i2c_SCB_MODE))
#define i2c_SCB_MODE_UART_INC      (0u !=(i2c_SCB_MODE_UART      & i2c_SCB_MODE))
#define i2c_SCB_MODE_EZI2C_INC     (0u !=(i2c_SCB_MODE_EZI2C     & i2c_SCB_MODE))

/* Interrupts remove options */
#define i2c_REMOVE_SCB_IRQ             (0u)
#define i2c_SCB_IRQ_INTERNAL           (0u == i2c_REMOVE_SCB_IRQ)

#define i2c_REMOVE_UART_RX_WAKEUP_IRQ  (1u)
#define i2c_UART_RX_WAKEUP_IRQ         (0u == i2c_REMOVE_UART_RX_WAKEUP_IRQ)

/* SCB interrupt enum */
#define i2c_SCB_INTR_MODE_NONE     (0u)
#define i2c_SCB_INTR_MODE_INTERNAL (1u)
#define i2c_SCB_INTR_MODE_EXTERNAL (2u)

/* Bootloader communication interface enable */
#define i2c_BTLDR_COMM_ENABLED ((CYDEV_BOOTLOADER_IO_COMP == CyBtldr_i2c) || \
                                             (CYDEV_BOOTLOADER_IO_COMP == CyBtldr_Custom_Interface))


/***************************************
*       Includes
****************************************/

#include "i2c_PINS.h"

#if(i2c_SCB_IRQ_INTERNAL)
    #include "i2c_SCB_IRQ.h"
#endif /* (i2c_SCB_IRQ_INTERNAL) */

#if(i2c_UART_RX_WAKEUP_IRQ)
    #include "i2c_RX_WAKEUP_IRQ.h"
#endif /* (i2c_UART_RX_WAKEUP_IRQ) */


/***************************************
*       Type Definitions
***************************************/

typedef struct
{
    uint8 enableState;
} i2c_BACKUP_STRUCT;


/***************************************
*        Function Prototypes
***************************************/

/* Start and Stop APIs */
void i2c_Init(void);
void i2c_Enable(void);
void i2c_Start(void);
void i2c_Stop(void);

/* Sleep and Wakeup APis */
void i2c_Sleep(void);
void i2c_Wakeup(void);

/* Customer interrupt handler */
void i2c_SetCustomInterruptHandler(cyisraddress func);

#if defined(CYDEV_BOOTLOADER_IO_COMP) && (i2c_BTLDR_COMM_ENABLED)

    /* Bootloader Physical layer functions */
    void i2c_CyBtldrCommStart(void);
    void i2c_CyBtldrCommStop (void);
    void i2c_CyBtldrCommReset(void);
    cystatus i2c_CyBtldrCommRead       (uint8 pData[], uint16 size, uint16 * count, uint8 timeOut);
    cystatus i2c_CyBtldrCommWrite(const uint8 pData[], uint16 size, uint16 * count, uint8 timeOut);

    #if(CYDEV_BOOTLOADER_IO_COMP == CyBtldr_i2c)
        #define CyBtldrCommStart    i2c_CyBtldrCommStart
        #define CyBtldrCommStop     i2c_CyBtldrCommStop
        #define CyBtldrCommReset    i2c_CyBtldrCommReset
        #define CyBtldrCommWrite    i2c_CyBtldrCommWrite
        #define CyBtldrCommRead     i2c_CyBtldrCommRead
    #endif /* (CYDEV_BOOTLOADER_IO_COMP == CyBtldr_i2c) */

#endif /*defined(CYDEV_BOOTLOADER_IO_COMP) && ((CYDEV_BOOTLOADER_IO_COMP == CyBtldr_i2c) || \
                                                     (CYDEV_BOOTLOADER_IO_COMP == CyBtldr_Custom_Interface)) */

/* Interface to internal interrupt component */
#if(i2c_SCB_IRQ_INTERNAL)
    #define i2c_EnableInt()    i2c_SCB_IRQ_Enable()
    #define i2c_DisableInt()   i2c_SCB_IRQ_Disable()
#endif /* (i2c_SCB_IRQ_INTERNAL) */

/* Get interrupt cause */
#define i2c_GetInterruptCause()    (i2c_INTR_CAUSE_REG)

/* APIs to service INTR_RX register */
#define i2c_SetRxInterruptMode(interruptMask)     i2c_WRITE_INTR_RX_MASK(interruptMask)
#define i2c_ClearRxInterruptSource(interruptMask) i2c_CLEAR_INTR_RX(interruptMask)
#define i2c_SetRxInterrupt(interruptMask)         i2c_SET_INTR_RX(interruptMask)
#define i2c_GetRxInterruptSource()                (i2c_INTR_RX_REG)
#define i2c_GetRxInterruptMode()                  (i2c_INTR_RX_MASK_REG)
#define i2c_GetRxInterruptSourceMasked()          (i2c_INTR_RX_MASKED_REG)

/* APIs to service INTR_TX register */
#define i2c_SetTxInterruptMode(interruptMask)     i2c_WRITE_INTR_TX_MASK(interruptMask)
#define i2c_ClearTxInterruptSource(interruptMask) i2c_CLEAR_INTR_TX(interruptMask)
#define i2c_SetTxInterrupt(interruptMask)         i2c_SET_INTR_TX(interruptMask)
#define i2c_GetTxInterruptSource()                (i2c_INTR_TX_REG)
#define i2c_GetTxInterruptMode()                  (i2c_INTR_TX_MASK_REG)
#define i2c_GetTxInterruptSourceMasked()          (i2c_INTR_TX_MASKED_REG)

/* APIs to service INTR_MASTER register */
#define i2c_SetMasterInterruptMode(interruptMask)    i2c_WRITE_INTR_MASTER_MASK(interruptMask)
#define i2c_ClearMasterInterruptSource(interruptMask) i2c_CLEAR_INTR_MASTER(interruptMask)
#define i2c_SetMasterInterrupt(interruptMask)         i2c_SET_INTR_MASTER(interruptMask)
#define i2c_GetMasterInterruptSource()                (i2c_INTR_MASTER_REG)
#define i2c_GetMasterInterruptMode()                  (i2c_INTR_MASTER_MASK_REG)
#define i2c_GetMasterInterruptSourceMasked()          (i2c_INTR_MASTER_MASKED_REG)

/* APIs to service INTR_SLAVE register */
#define i2c_SetSlaveInterruptMode(interruptMask)     i2c_WRITE_INTR_SLAVE_MASK(interruptMask)
#define i2c_ClearSlaveInterruptSource(interruptMask) i2c_CLEAR_INTR_SLAVE(interruptMask)
#define i2c_SetSlaveInterrupt(interruptMask)         i2c_SET_INTR_SLAVE(interruptMask)
#define i2c_GetSlaveInterruptSource()                (i2c_INTR_SLAVE_REG)
#define i2c_GetSlaveInterruptMode()                  (i2c_INTR_SLAVE_MASK_REG)
#define i2c_GetSlaveInterruptSourceMasked()          (i2c_INTR_SLAVE_MASKED_REG)


/**********************************
*     Vars with External Linkage
**********************************/

extern uint8 i2c_initVar;


/***************************************
*              Registers
***************************************/

#define i2c_CTRL_REG               (*(reg32 *) i2c_SCB__CTRL)
#define i2c_CTRL_PTR               ( (reg32 *) i2c_SCB__CTRL)

#define i2c_STATUS_REG             (*(reg32 *) i2c_SCB__STATUS)
#define i2c_STATUS_PTR             ( (reg32 *) i2c_SCB__STATUS)

#define i2c_SPI_CTRL_REG           (*(reg32 *) i2c_SCB__SPI_CTRL)
#define i2c_SPI_CTRL_PTR           ( (reg32 *) i2c_SCB__SPI_CTRL)

#define i2c_SPI_STATUS_REG         (*(reg32 *) i2c_SCB__SPI_STATUS)
#define i2c_SPI_STATUS_PTR         ( (reg32 *) i2c_SCB__SPI_STATUS)

#define i2c_UART_CTRL_REG          (*(reg32 *) i2c_SCB__UART_CTRL)
#define i2c_UART_CTRL_PTR          ( (reg32 *) i2c_SCB__UART_CTRL)

#define i2c_UART_TX_CTRL_REG       (*(reg32 *) i2c_SCB__UART_TX_CTRL)
#define i2c_UART_TX_CTRL_PTR       ( (reg32 *) i2c_SCB__UART_RX_CTRL)

#define i2c_UART_RX_CTRL_REG       (*(reg32 *) i2c_SCB__UART_RX_CTRL)
#define i2c_UART_RX_CTRL_PTR       ( (reg32 *) i2c_SCB__UART_RX_CTRL)

#define i2c_UART_RX_STATUS_REG     (*(reg32 *) i2c_SCB__UART_RX_STATUS)
#define i2c_UART_RX_STATUS_PTR     ( (reg32 *) i2c_SCB__UART_RX_STATUS)

#define i2c_I2C_CTRL_REG           (*(reg32 *) i2c_SCB__I2C_CTRL)
#define i2c_I2C_CTRL_PTR           ( (reg32 *) i2c_SCB__I2C_CTRL)

#define i2c_I2C_STATUS_REG         (*(reg32 *) i2c_SCB__I2C_STATUS)
#define i2c_I2C_STATUS_PTR         ( (reg32 *) i2c_SCB__I2C_STATUS)

#define i2c_I2C_MASTER_CMD_REG     (*(reg32 *) i2c_SCB__I2C_M_CMD)
#define i2c_I2C_MASTER_CMD_PTR     ( (reg32 *) i2c_SCB__I2C_M_CMD)

#define i2c_I2C_SLAVE_CMD_REG      (*(reg32 *) i2c_SCB__I2C_S_CMD)
#define i2c_I2C_SLAVE_CMD_PTR      ( (reg32 *) i2c_SCB__I2C_S_CMD)

#define i2c_I2C_CFG_REG            (*(reg32 *) i2c_SCB__I2C_CFG)
#define i2c_I2C_CFG_PTR            ( (reg32 *) i2c_SCB__I2C_CFG)

#define i2c_TX_CTRL_REG            (*(reg32 *) i2c_SCB__TX_CTRL)
#define i2c_TX_CTRL_PTR            ( (reg32 *) i2c_SCB__TX_CTRL)

#define i2c_TX_FIFO_CTRL_REG       (*(reg32 *) i2c_SCB__TX_FIFO_CTRL)
#define i2c_TX_FIFO_CTRL_PTR       ( (reg32 *) i2c_SCB__TX_FIFO_CTRL)

#define i2c_TX_FIFO_STATUS_REG     (*(reg32 *) i2c_SCB__TX_FIFO_STATUS)
#define i2c_TX_FIFO_STATUS_PTR     ( (reg32 *) i2c_SCB__TX_FIFO_STATUS)

#define i2c_TX_FIFO_WR_REG         (*(reg32 *) i2c_SCB__TX_FIFO_WR)
#define i2c_TX_FIFO_WR_PTR         ( (reg32 *) i2c_SCB__TX_FIFO_WR)

#define i2c_RX_CTRL_REG            (*(reg32 *) i2c_SCB__RX_CTRL)
#define i2c_RX_CTRL_PTR            ( (reg32 *) i2c_SCB__RX_CTRL)

#define i2c_RX_FIFO_CTRL_REG       (*(reg32 *) i2c_SCB__RX_FIFO_CTRL)
#define i2c_RX_FIFO_CTRL_PTR       ( (reg32 *) i2c_SCB__RX_FIFO_CTRL)

#define i2c_RX_FIFO_STATUS_REG     (*(reg32 *) i2c_SCB__RX_FIFO_STATUS)
#define i2c_RX_FIFO_STATUS_PTR     ( (reg32 *) i2c_SCB__RX_FIFO_STATUS)

#define i2c_RX_MATCH_REG           (*(reg32 *) i2c_SCB__RX_MATCH)
#define i2c_RX_MATCH_PTR           ( (reg32 *) i2c_SCB__RX_MATCH)

#define i2c_RX_FIFO_RD_REG         (*(reg32 *) i2c_SCB__RX_FIFO_RD)
#define i2c_RX_FIFO_RD_PTR         ( (reg32 *) i2c_SCB__RX_FIFO_RD)

#define i2c_RX_FIFO_RD_SILENT_REG  (*(reg32 *) i2c_SCB__RX_FIFO_RD_SILENT)
#define i2c_RX_FIFO_RD_SILENT_PTR  ( (reg32 *) i2c_SCB__RX_FIFO_RD_SILENT)

#define i2c_EZBUF_DATA00_REG       (*(reg32 *) i2c_SCB__EZ_DATA00)
#define i2c_EZBUF_DATA00_PTR       ( (reg32 *) i2c_SCB__EZ_DATA00)

#define i2c_INTR_CAUSE_REG         (*(reg32 *) i2c_SCB__INTR_CAUSE)
#define i2c_INTR_CAUSE_PTR         ( (reg32 *) i2c_SCB__INTR_CAUSE)

#define i2c_INTR_I2C_EC_REG        (*(reg32 *) i2c_SCB__INTR_I2C_EC)
#define i2c_INTR_I2C_EC_PTR        ( (reg32 *) i2c_SCB__INTR_I2C_EC)

#define i2c_INTR_I2C_EC_MASK_REG   (*(reg32 *) i2c_SCB__INTR_I2C_EC_MASK)
#define i2c_INTR_I2C_EC_MASK_PTR   ( (reg32 *) i2c_SCB__INTR_I2C_EC_MASK)

#define i2c_INTR_I2C_EC_MASKED_REG (*(reg32 *) i2c_SCB__INTR_I2C_EC_MASKED)
#define i2c_INTR_I2C_EC_MASKED_PTR ( (reg32 *) i2c_SCB__INTR_I2C_EC_MASKED)

#define i2c_INTR_SPI_EC_REG        (*(reg32 *) i2c_SCB__INTR_SPI_EC)
#define i2c_INTR_SPI_EC_PTR        ( (reg32 *) i2c_SCB__INTR_SPI_EC)

#define i2c_INTR_SPI_EC_MASK_REG   (*(reg32 *) i2c_SCB__INTR_SPI_EC_MASK)
#define i2c_INTR_SPI_EC_MASK_PTR   ( (reg32 *) i2c_SCB__INTR_SPI_EC_MASK)

#define i2c_INTR_SPI_EC_MASKED_REG (*(reg32 *) i2c_SCB__INTR_SPI_EC_MASKED)
#define i2c_INTR_SPI_EC_MASKED_PTR ( (reg32 *) i2c_SCB__INTR_SPI_EC_MASKED)

#define i2c_INTR_MASTER_REG        (*(reg32 *) i2c_SCB__INTR_M)
#define i2c_INTR_MASTER_PTR        ( (reg32 *) i2c_SCB__INTR_M)

#define i2c_INTR_MASTER_SET_REG    (*(reg32 *) i2c_SCB__INTR_M_SET)
#define i2c_INTR_MASTER_SET_PTR    ( (reg32 *) i2c_SCB__INTR_M_SET)

#define i2c_INTR_MASTER_MASK_REG   (*(reg32 *) i2c_SCB__INTR_M_MASK)
#define i2c_INTR_MASTER_MASK_PTR   ( (reg32 *) i2c_SCB__INTR_M_MASK)

#define i2c_INTR_MASTER_MASKED_REG (*(reg32 *) i2c_SCB__INTR_M_MASKED)
#define i2c_INTR_MASTER_MASKED_PTR ( (reg32 *) i2c_SCB__INTR_M_MASKED)

#define i2c_INTR_SLAVE_REG         (*(reg32 *) i2c_SCB__INTR_S)
#define i2c_INTR_SLAVE_PTR         ( (reg32 *) i2c_SCB__INTR_S)

#define i2c_INTR_SLAVE_SET_REG     (*(reg32 *) i2c_SCB__INTR_S_SET)
#define i2c_INTR_SLAVE_SET_PTR     ( (reg32 *) i2c_SCB__INTR_S_SET)

#define i2c_INTR_SLAVE_MASK_REG    (*(reg32 *) i2c_SCB__INTR_S_MASK)
#define i2c_INTR_SLAVE_MASK_PTR    ( (reg32 *) i2c_SCB__INTR_S_MASK)

#define i2c_INTR_SLAVE_MASKED_REG  (*(reg32 *) i2c_SCB__INTR_S_MASKED)
#define i2c_INTR_SLAVE_MASKED_PTR  ( (reg32 *) i2c_SCB__INTR_S_MASKED)

#define i2c_INTR_TX_REG            (*(reg32 *) i2c_SCB__INTR_TX)
#define i2c_INTR_TX_PTR            ( (reg32 *) i2c_SCB__INTR_TX)

#define i2c_INTR_TX_SET_REG        (*(reg32 *) i2c_SCB__INTR_TX_SET)
#define i2c_INTR_TX_SET_PTR        ( (reg32 *) i2c_SCB__INTR_TX_SET)

#define i2c_INTR_TX_MASK_REG       (*(reg32 *) i2c_SCB__INTR_TX_MASK)
#define i2c_INTR_TX_MASK_PTR       ( (reg32 *) i2c_SCB__INTR_TX_MASK)

#define i2c_INTR_TX_MASKED_REG     (*(reg32 *) i2c_SCB__INTR_TX_MASKED)
#define i2c_INTR_TX_MASKED_PTR     ( (reg32 *) i2c_SCB__INTR_TX_MASKED)

#define i2c_INTR_RX_REG            (*(reg32 *) i2c_SCB__INTR_RX)
#define i2c_INTR_RX_PTR            ( (reg32 *) i2c_SCB__INTR_RX)

#define i2c_INTR_RX_SET_REG        (*(reg32 *) i2c_SCB__INTR_RX_SET)
#define i2c_INTR_RX_SET_PTR        ( (reg32 *) i2c_SCB__INTR_RX_SET)

#define i2c_INTR_RX_MASK_REG       (*(reg32 *) i2c_SCB__INTR_RX_MASK)
#define i2c_INTR_RX_MASK_PTR       ( (reg32 *) i2c_SCB__INTR_RX_MASK)

#define i2c_INTR_RX_MASKED_REG     (*(reg32 *) i2c_SCB__INTR_RX_MASKED)
#define i2c_INTR_RX_MASKED_PTR     ( (reg32 *) i2c_SCB__INTR_RX_MASKED)


/***************************************
*        Registers Constants
***************************************/

/* i2c_CTRL */
#define i2c_CTRL_OVS_POS           (0u)  /* [3:0]   Oversampling factor                 */
#define i2c_CTRL_EC_AM_MODE_POS    (8u)  /* [8]     Externally clocked address match    */
#define i2c_CTRL_EC_OP_MODE_POS    (9u)  /* [9]     Externally clocked operation mode   */
#define i2c_CTRL_EZBUF_MODE_POS    (10u) /* [10]    EZ buffer is enabled                */
#define i2c_CTRL_ADDR_ACCEPT_POS   (16u) /* [16]    Put matched address in RX FIFO      */
#define i2c_CTRL_BLOCK_POS         (17u) /* [17]    Ext and Int logic to resolve colide */
#define i2c_CTRL_MODE_POS          (24u) /* [25:24] Operation mode                      */
#define i2c_CTRL_ENABLED_POS       (31u) /* [31]    Enable SCB block                    */
#define i2c_CTRL_OVS_MASK          ((uint32) 0x0Fu)
#define i2c_CTRL_EC_AM_MODE        ((uint32) ((uint32) 0x01u << i2c_CTRL_EC_AM_MODE_POS))
#define i2c_CTRL_EC_OP_MODE        ((uint32) ((uint32) 0x01u << i2c_CTRL_EC_OP_MODE_POS))
#define i2c_CTRL_EZBUF_MODE        ((uint32) ((uint32) 0x01u << i2c_CTRL_EZBUF_MODE_POS))
#define i2c_CTRL_ADDR_ACCEPT       ((uint32) ((uint32) 0x01u << i2c_CTRL_ADDR_ACCEPT_POS))
#define i2c_CTRL_BLOCK             ((uint32) ((uint32) 0x01u << i2c_CTRL_BLOCK_POS))
#define i2c_CTRL_MODE_MASK         ((uint32) ((uint32) 0x03u << i2c_CTRL_MODE_POS))
#define i2c_CTRL_MODE_I2C          ((uint32)  0x00u)
#define i2c_CTRL_MODE_SPI          ((uint32) ((uint32) 0x01u << i2c_CTRL_MODE_POS))
#define i2c_CTRL_MODE_UART         ((uint32) ((uint32) 0x02u << i2c_CTRL_MODE_POS))
#define i2c_CTRL_ENABLED           ((uint32) ((uint32) 0x01u << i2c_CTRL_ENABLED_POS))


/* i2c_STATUS_REG */
#define i2c_STATUS_EC_BUSY_POS     (0u)  /* [0] Bus busy. Externaly clocked loigc access to EZ memory */
#define i2c_STATUS_EC_BUSY         ((uint32) 0x0Fu)


/* i2c_SPI_CTRL_REG  */
#define i2c_SPI_CTRL_CONTINUOUS_POS        (0u)  /* [0]     Continuous or Separated SPI data transfers */
#define i2c_SPI_CTRL_SELECT_PRECEDE_POS    (1u)  /* [1]     Precedes or coincides start of data frame  */
#define i2c_SPI_CTRL_CPHA_POS              (2u)  /* [2]     SCLK phase                                 */
#define i2c_SPI_CTRL_CPOL_POS              (3u)  /* [3]     SCLK polarity                              */
#define i2c_SPI_CTRL_LATE_MISO_SAMPLE_POS  (4u)  /* [4]     Late MISO sample enabled                   */
#define i2c_SPI_CTRL_LOOPBACK_POS          (16u) /* [16]    Local loopback control enabled             */
#define i2c_SPI_CTRL_MODE_POS              (24u) /* [25:24] Submode of SPI operation                   */
#define i2c_SPI_CTRL_SLAVE_SELECT_POS      (26u) /* [27:26] Selects SPI SS signal                      */
#define i2c_SPI_CTRL_MASTER_MODE_POS       (31u) /* [31]    Master mode enabled                        */
#define i2c_SPI_CTRL_CONTINUOUS            ((uint32) 0x01u)
#define i2c_SPI_CTRL_SELECT_PRECEDE        ((uint32) ((uint32) 0x01u << \
                                                                    i2c_SPI_CTRL_SELECT_PRECEDE_POS))
#define i2c_SPI_CTRL_SCLK_MODE_MASK        ((uint32) ((uint32) 0x03u << \
                                                                    i2c_SPI_CTRL_CPHA_POS))
#define i2c_SPI_CTRL_CPHA                  ((uint32) ((uint32) 0x01u << \
                                                                    i2c_SPI_CTRL_CPHA_POS))
#define i2c_SPI_CTRL_CPOL                  ((uint32) ((uint32) 0x01u << \
                                                                    i2c_SPI_CTRL_CPOL_POS))
#define i2c_SPI_CTRL_LATE_MISO_SAMPLE      ((uint32) ((uint32) 0x01u << \
                                                                    i2c_SPI_CTRL_LATE_MISO_SAMPLE_POS))
#define i2c_SPI_CTRL_LOOPBACK              ((uint32) ((uint32) 0x01u << \
                                                                    i2c_SPI_CTRL_LOOPBACK_POS))
#define i2c_SPI_CTRL_MODE_MASK             ((uint32) ((uint32) 0x03u << \
                                                                    i2c_SPI_CTRL_MODE_POS))
#define i2c_SPI_CTRL_MODE_MOTOROLA         ((uint32) 0x00u)
#define i2c_SPI_CTRL_MODE_TI               ((uint32) ((uint32) 0x01u << i2c_CTRL_MODE_POS))
#define i2c_SPI_CTRL_MODE_NS               ((uint32) ((uint32) 0x02u << i2c_CTRL_MODE_POS))
#define i2c_SPI_CTRL_SLAVE_SELECT_MASK     ((uint32) ((uint32) 0x03u << \
                                                                    i2c_SPI_CTRL_SLAVE_SELECT_POS))
#define i2c_SPI_CTRL_SLAVE_SELECT0         ((uint32) 0x00u)
#define i2c_SPI_CTRL_SLAVE_SELECT1         ((uint32) ((uint32) 0x01u << \
                                                                    i2c_SPI_CTRL_SLAVE_SELECT_POS))
#define i2c_SPI_CTRL_SLAVE_SELECT2         ((uint32) ((uint32) 0x02u << \
                                                                    i2c_SPI_CTRL_SLAVE_SELECT_POS))
#define i2c_SPI_CTRL_SLAVE_SELECT3         ((uint32) ((uint32) 0x03u << \
                                                                    i2c_SPI_CTRL_SLAVE_SELECT_POS))
#define i2c_SPI_CTRL_MASTER                ((uint32) ((uint32) 0x01u << \
                                                                    i2c_SPI_CTRL_MASTER_MODE_POS))
#define i2c_SPI_CTRL_SLAVE                 ((uint32) 0x00u)


/* i2c_SPI_STATUS_REG  */
#define i2c_SPI_STATUS_BUS_BUSY_POS    (0u)  /* [0]    Bus busy - slave selected */
#define i2c_SPI_STATUS_EZBUF_ADDR_POS  (8u)  /* [15:8] EzAddress                 */
#define i2c_SPI_STATUS_BUS_BUSY        ((uint32) 0x01u)
#define i2c_SPI_STATUS_EZBUF_ADDR_MASK    ((uint32) ((uint32) 0xFFu << \
                                                                    i2c_I2C_STATUS_EZBUF_ADDR_POS))


/* i2c_UART_CTRL */
#define i2c_UART_CTRL_LOOPBACK_POS         (16u) /* [16] Loopback     */
#define i2c_UART_CTRL_MODE_POS             (24u) /* [24] UART subMode */
#define i2c_UART_CTRL_LOOPBACK             ((uint32) ((uint32) 0x01u << \
                                                                        i2c_UART_CTRL_LOOPBACK_POS))
#define i2c_UART_CTRL_MODE_UART_STD        ((uint32) 0x00u)
#define i2c_UART_CTRL_MODE_UART_SMARTCARD  ((uint32) ((uint32) 0x01u << \
                                                                        i2c_UART_CTRL_MODE_POS))
#define i2c_UART_CTRL_MODE_UART_IRDA       ((uint32) ((uint32) 0x02u << \
                                                                        i2c_UART_CTRL_MODE_POS))
#define i2c_UART_CTRL_MODE_MASK            ((uint32) ((uint32) 0x03u << \
                                                                        i2c_UART_CTRL_MODE_POS))


/* i2c_UART_TX_CTRL */
#define i2c_UART_TX_CTRL_STOP_BITS_POS         (0u)  /* [2:0] Stop bits: (Stop bits + 1) * 0.5 period */
#define i2c_UART_TX_CTRL_PARITY_POS            (4u)  /* [4]   Parity bit                              */
#define i2c_UART_TX_CTRL_PARITY_ENABLED_POS    (5u)  /* [5]   Parity enable                           */
#define i2c_UART_TX_CTRL_RETRY_ON_NACK_POS     (8u)  /* [8]   Smart Card: re-send frame on NACK       */
#define i2c_UART_TX_CTRL_ONE_STOP_BIT          ((uint32) 0x01u)
#define i2c_UART_TX_CTRL_ONE_HALF_STOP_BITS    ((uint32) 0x02u)
#define i2c_UART_TX_CTRL_TWO_STOP_BITS         ((uint32) 0x03u)
#define i2c_UART_TX_CTRL_STOP_BITS_MASK        ((uint32) 0x07u)
#define i2c_UART_TX_CTRL_PARITY                ((uint32) ((uint32) 0x01u << \
                                                                    i2c_UART_TX_CTRL_PARITY_POS))
#define i2c_UART_TX_CTRL_PARITY_ENABLED        ((uint32) ((uint32) 0x01u << \
                                                                    i2c_UART_TX_CTRL_PARITY_ENABLED_POS))
#define i2c_UART_TX_CTRL_RETRY_ON_NACK         ((uint32) ((uint32) 0x01u << \
                                                                    i2c_UART_TX_CTRL_RETRY_ON_NACK_POS))


/* i2c_UART_RX_CTRL */
#define i2c_UART_RX_CTRL_STOP_BITS_POS             (0u)  /* [2:0] Stop bits: (Stop bits + 1) * 0.5 prd   */
#define i2c_UART_RX_CTRL_PARITY_POS                (4u)  /* [4]   Parity bit                             */
#define i2c_UART_RX_CTRL_PARITY_ENABLED_POS        (5u)  /* [5]   Parity enable                          */
#define i2c_UART_RX_CTRL_POLARITY_POS              (6u)  /* [6]   IrDA: inverts polarity of RX signal    */
#define i2c_UART_RX_CTRL_DROP_ON_PARITY_ERR_POS    (8u)  /* [8]   Drop and lost RX FIFO on parity error  */
#define i2c_UART_RX_CTRL_DROP_ON_FRAME_ERR_POS     (9u)  /* [9]   Drop and lost RX FIFO on frame error   */
#define i2c_UART_RX_CTRL_MP_MODE_POS               (10u) /* [10]  Multi-processor mode                   */
#define i2c_UART_RX_CTRL_LIN_MODE_POS              (12u) /* [12]  Lin mode: applicable for UART Standart */
#define i2c_UART_RX_CTRL_SKIP_START_POS            (13u) /* [13]  Skip start not: only for UART Standart */
#define i2c_UART_RX_CTRL_BREAK_WIDTH_POS           (16u) /* [19:16]  Break width: (Break width + 1)      */
#define i2c_UART_TX_CTRL_ONE_STOP_BIT              ((uint32) 0x01u)
#define i2c_UART_TX_CTRL_ONE_HALF_STOP_BITS        ((uint32) 0x02u)
#define i2c_UART_TX_CTRL_TWO_STOP_BITS             ((uint32) 0x03u)
#define i2c_UART_RX_CTRL_STOP_BITS_MASK            ((uint32) 0x07u)
#define i2c_UART_RX_CTRL_PARITY                    ((uint32) ((uint32) 0x01u << \
                                                                    i2c_UART_RX_CTRL_PARITY_POS))
#define i2c_UART_RX_CTRL_PARITY_ENABLED            ((uint32) ((uint32) 0x01u << \
                                                                    i2c_UART_RX_CTRL_PARITY_ENABLED_POS))
#define i2c_UART_RX_CTRL_POLARITY                  ((uint32) ((uint32) 0x01u << \
                                                                    i2c_UART_RX_CTRL_POLARITY_POS))
#define i2c_UART_RX_CTRL_DROP_ON_PARITY_ERR        ((uint32) ((uint32) 0x01u << \
                                                                i2c_UART_RX_CTRL_DROP_ON_PARITY_ERR_POS))
#define i2c_UART_RX_CTRL_DROP_ON_FRAME_ERR         ((uint32) ((uint32) 0x01u << \
                                                                i2c_UART_RX_CTRL_DROP_ON_FRAME_ERR_POS))
#define i2c_UART_RX_CTRL_MP_MODE                   ((uint32) ((uint32) 0x01u << \
                                                                    i2c_UART_RX_CTRL_MP_MODE_POS))
#define i2c_UART_RX_CTRL_LIN_MODE                  ((uint32) ((uint32) 0x01u << \
                                                                    i2c_UART_RX_CTRL_LIN_MODE_POS))
#define i2c_UART_RX_CTRL_SKIP_START                ((uint32) ((uint32) 0x01u << \
                                                                  i2c_UART_RX_CTRL_SKIP_START_POS))
#define i2c_UART_RX_CTRL_BREAK_WIDTH_MASK          ((uint32) ((uint32) 0x0Fu << \
                                                                  i2c_UART_RX_CTRL_BREAK_WIDTH_POS))

/* i2c_UART_RX_STATUS_REG */
#define i2c_UART_RX_STATUS_BR_COUNTER_POS     (0u)  /* [11:0] Baute Rate counter */
#define i2c_UART_RX_STATUS_BR_COUNTER_MASK    ((uint32) 0xFFFu)


/* i2c_I2C_CTRL */
#define i2c_I2C_CTRL_HIGH_PHASE_OVS_POS           (0u)   /* [3:0] Oversampling factor high: masrer only */
#define i2c_I2C_CTRL_LOW_PHASE_OVS_POS            (4u)   /* [7:4] Oversampling factor low:  masrer only */
#define i2c_I2C_CTRL_M_READY_DATA_ACK_POS         (8u)   /* [8]   Master ACKs data wgile RX FIFO != FULL*/
#define i2c_I2C_CTRL_M_NOT_READY_DATA_NACK_POS    (9u)   /* [9]   Master NACKs data if RX FIFO ==  FULL */
#define i2c_I2C_CTRL_S_GENERAL_IGNORE_POS         (11u)  /* [11]  Slave ignores General call            */
#define i2c_I2C_CTRL_S_READY_ADDR_ACK_POS         (12u)  /* [12]  Slave ACKs Address if RX FIFO != FULL */
#define i2c_I2C_CTRL_S_READY_DATA_ACK_POS         (13u)  /* [13]  Slave ACKs data while RX FIFO == FULL */
#define i2c_I2C_CTRL_S_NOT_READY_ADDR_NACK_POS    (14u)  /* [14]  Slave NACKs address if RX FIFO == FULL*/
#define i2c_I2C_CTRL_S_NOT_READY_DATA_NACK_POS    (15u)  /* [15]  Slave NACKs data if RX FIFO is  FULL  */
#define i2c_I2C_CTRL_LOOPBACK_POS                 (16u)  /* [16]  Loopback                              */
#define i2c_I2C_CTRL_SLAVE_MODE_POS               (30u)  /* [30]  Slave mode enabled                    */
#define i2c_I2C_CTRL_MASTER_MODE_POS              (31u)  /* [31]  Master mode enabled                   */
#define i2c_I2C_CTRL_HIGH_PHASE_OVS_MASK  ((uint32) 0x0Fu)
#define i2c_I2C_CTRL_LOW_PHASE_OVS_MASK   ((uint32) ((uint32) 0x0Fu << \
                                                                i2c_I2C_CTRL_LOW_PHASE_OVS_POS))
#define i2c_I2C_CTRL_M_READY_DATA_ACK      ((uint32) ((uint32) 0x01u << \
                                                                i2c_I2C_CTRL_M_READY_DATA_ACK_POS))
#define i2c_I2C_CTRL_M_NOT_READY_DATA_NACK ((uint32) ((uint32) 0x01u << \
                                                                i2c_I2C_CTRL_M_NOT_READY_DATA_NACK_POS))
#define i2c_I2C_CTRL_S_GENERAL_IGNORE      ((uint32) ((uint32) 0x01u << \
                                                                i2c_I2C_CTRL_S_GENERAL_IGNORE_POS))
#define i2c_I2C_CTRL_S_READY_ADDR_ACK      ((uint32) ((uint32) 0x01u << \
                                                                i2c_I2C_CTRL_S_READY_ADDR_ACK_POS))
#define i2c_I2C_CTRL_S_READY_DATA_ACK      ((uint32) ((uint32) 0x01u << \
                                                                i2c_I2C_CTRL_S_READY_DATA_ACK_POS))
#define i2c_I2C_CTRL_S_NOT_READY_ADDR_NACK ((uint32) ((uint32) 0x01u << \
                                                                i2c_I2C_CTRL_S_NOT_READY_ADDR_NACK_POS))
#define i2c_I2C_CTRL_S_NOT_READY_DATA_NACK ((uint32) ((uint32) 0x01u << \
                                                                i2c_I2C_CTRL_S_NOT_READY_DATA_NACK_POS))
#define i2c_I2C_CTRL_LOOPBACK              ((uint32) ((uint32) 0x01u << \
                                                                i2c_I2C_CTRL_LOOPBACK_POS))
#define i2c_I2C_CTRL_SLAVE_MODE            ((uint32) ((uint32) 0x01u << \
                                                                i2c_I2C_CTRL_SLAVE_MODE_POS))
#define i2c_I2C_CTRL_MASTER_MODE           ((uint32) ((uint32) 0x01u << \
                                                                i2c_I2C_CTRL_MASTER_MODE_POS))
#define i2c_I2C_CTRL_SLAVE_MASTER_MODE_MASK    ((uint32) ((uint32) 0x03u << \
                                                                i2c_I2C_CTRL_SLAVE_MODE_POS))


/* i2c_I2C_STATUS_REG  */
#define i2c_I2C_STATUS_BUS_BUSY_POS    (0u)  /* [0]    Bus busy: internally clocked */
#define i2c_I2C_STATUS_S_READ_POS      (4u)  /* [4]    Slave is read by master      */
#define i2c_I2C_STATUS_M_READ_POS      (5u)  /* [5]    Master reads Slave           */
#define i2c_I2C_STATUS_EZBUF_ADDR_POS  (8u)  /* [15:8] EZAddress                    */
#define i2c_I2C_STATUS_BUS_BUSY        ((uint32) 0x01u)
#define i2c_I2C_STATUS_S_READ          ((uint32) ((uint32) 0x01u << \
                                                                    i2c_I2C_STATUS_S_READ_POS))
#define i2c_I2C_STATUS_M_READ          ((uint32) ((uint32) 0x01u << \
                                                                    i2c_I2C_STATUS_M_READ_POS))
#define i2c_I2C_STATUS_EZBUF_ADDR_MASK ((uint32) ((uint32) 0xFFu << \
                                                                    i2c_I2C_STATUS_EZBUF_ADDR_POS))


/* i2c_I2C_MASTER_CMD_REG */
#define i2c_I2C_MASTER_CMD_M_START_POS             (0u)  /* [0] Master generate Start                */
#define i2c_I2C_MASTER_CMD_M_START_ON_IDLE_POS     (1u)  /* [1] Master generate Start if bus is free */
#define i2c_I2C_MASTER_CMD_M_ACK_POS               (2u)  /* [2] Master generate ACK                  */
#define i2c_I2C_MASTER_CMD_M_NACK_POS              (3u)  /* [3] Master generate NACK                 */
#define i2c_I2C_MASTER_CMD_M_STOP_POS              (4u)  /* [4] Master generate Stop                 */
#define i2c_I2C_MASTER_CMD_M_START         ((uint32) 0x01u)
#define i2c_I2C_MASTER_CMD_M_START_ON_IDLE ((uint32) ((uint32) 0x01u << \
                                                                   i2c_I2C_MASTER_CMD_M_START_ON_IDLE_POS))
#define i2c_I2C_MASTER_CMD_M_ACK           ((uint32) ((uint32) 0x01u << \
                                                                   i2c_I2C_MASTER_CMD_M_ACK_POS))
#define i2c_I2C_MASTER_CMD_M_NACK          ((uint32) ((uint32) 0x01u << \
                                                                    i2c_I2C_MASTER_CMD_M_NACK_POS))
#define i2c_I2C_MASTER_CMD_M_STOP          ((uint32) ((uint32) 0x01u << \
                                                                    i2c_I2C_MASTER_CMD_M_STOP_POS))


/* i2c_I2C_SLAVE_CMD_REG  */
#define i2c_I2C_SLAVE_CMD_S_ACK_POS    (0u)  /* [0] Slave generate ACK  */
#define i2c_I2C_SLAVE_CMD_S_NACK_POS   (1u)  /* [1] Slave generate NACK */
#define i2c_I2C_SLAVE_CMD_S_ACK        ((uint32) 0x01u)
#define i2c_I2C_SLAVE_CMD_S_NACK       ((uint32) ((uint32) 0x01u << \
                                                                i2c_I2C_SLAVE_CMD_S_NACK_POS))

#define i2c_I2C_SLAVE_CMD_S_ACK_POS    (0u)  /* [0] Slave generate ACK  */
#define i2c_I2C_SLAVE_CMD_S_NACK_POS   (1u)  /* [1] Slave generate NACK */
#define i2c_I2C_SLAVE_CMD_S_ACK        ((uint32) 0x01u)
#define i2c_I2C_SLAVE_CMD_S_NACK       ((uint32) ((uint32) 0x01u << \
                                                                i2c_I2C_SLAVE_CMD_S_NACK_POS))
/* i2c_I2C_CFG  */
#define i2c_I2C_CFG_SDA_FILT_HYS_POS           (0u)  /* [1:0]   Trim bits for the I2C SDA filter         */
#define i2c_I2C_CFG_SDA_FILT_TRIM_POS          (2u)  /* [3:2]   Trim bits for the I2C SDA filter         */
#define i2c_I2C_CFG_SCL_FILT_HYS_POS           (4u)  /* [5:4]   Trim bits for the I2C SCL filter         */
#define i2c_I2C_CFG_SCL_FILT_TRIM_POS          (6u)  /* [7:6]   Trim bits for the I2C SCL filter         */
#define i2c_I2C_CFG_SDA_FILT_OUT_HYS_POS       (8u)  /* [9:8]   Trim bits for I2C SDA filter output path */
#define i2c_I2C_CFG_SDA_FILT_OUT_TRIM_POS      (10u) /* [11:10] Trim bits for I2C SDA filter output path */
#define i2c_I2C_CFG_SDA_FILT_HS_POS            (16u) /* [16]    '0': 50 ns filter, '1': 10 ns filter     */
#define i2c_I2C_CFG_SDA_FILT_ENABLED_POS       (17u) /* [17]    I2C SDA filter enabled                   */
#define i2c_I2C_CFG_SCL_FILT_HS_POS            (24u) /* [24]    '0': 50 ns filter, '1': 10 ns filter     */
#define i2c_I2C_CFG_SCL_FILT_ENABLED_POS       (25u) /* [25]    I2C SCL filter enabled                   */
#define i2c_I2C_CFG_SDA_FILT_OUT_HS_POS        (26u) /* [26]    '0': 50ns filter, '1': 10 ns filter      */
#define i2c_I2C_CFG_SDA_FILT_OUT_ENABLED_POS   (27u) /* [27]    I2C SDA output delay filter enabled      */
#define i2c_I2C_CFG_SDA_FILT_HYS_MASK          ((uint32) 0x00u)
#define i2c_I2C_CFG_SDA_FILT_TRIM_MASK         ((uint32) ((uint32) 0x03u << \
                                                                i2c_I2C_CFG_SDA_FILT_TRIM_POS))
#define i2c_I2C_CFG_SCL_FILT_HYS_MASK          ((uint32) ((uint32) 0x03u << \
                                                                i2c_I2C_CFG_SCL_FILT_HYS_POS))
#define i2c_I2C_CFG_SCL_FILT_TRIM_MASK         ((uint32) ((uint32) 0x03u << \
                                                                i2c_I2C_CFG_SCL_FILT_TRIM_POS))
#define i2c_I2C_CFG_SDA_FILT_OUT_HYS_MASK      ((uint32) ((uint32) 0x03u << \
                                                                i2c_I2C_CFG_SDA_FILT_OUT_HYS_POS))
#define i2c_I2C_CFG_SDA_FILT_OUT_TRIM_MASK     ((uint32) ((uint32) 0x03u << \
                                                                i2c_I2C_CFG_SDA_FILT_OUT_TRIM_POS))
#define i2c_I2C_CFG_SDA_FILT_HS                ((uint32) ((uint32) 0x01u << \
                                                                i2c_I2C_CFG_SDA_FILT_HS_POS))
#define i2c_I2C_CFG_SDA_FILT_ENABLED           ((uint32) ((uint32) 0x01u << \
                                                                i2c_I2C_CFG_SDA_FILT_ENABLED_POS))
#define i2c_I2C_CFG_SCL_FILT_HS                ((uint32) ((uint32) 0x01u << \
                                                                i2c_I2C_CFG_SCL_FILT_HS_POS))
#define i2c_I2C_CFG_SCL_FILT_ENABLED           ((uint32) ((uint32) 0x01u << \
                                                                i2c_I2C_CFG_SCL_FILT_ENABLED_POS))
#define i2c_I2C_CFG_SDA_FILT_OUT_HS            ((uint32) ((uint32) 0x01u << \
                                                                i2c_I2C_CFG_SDA_FILT_OUT_HS_POS))
#define i2c_I2C_CFG_SDA_FILT_OUT_ENABLED       ((uint32) ((uint32) 0x01u << \
                                                                i2c_I2C_CFG_SDA_FILT_OUT_ENABLED_POS))


/* i2c_TX_CTRL_REG */
#define i2c_TX_CTRL_DATA_WIDTH_POS     (0u)  /* [3:0] Dataframe width: (Data width - 1) */
#define i2c_TX_CTRL_MSB_FIRST_POS      (8u)  /* [8]   MSB first shifter-out             */
#define i2c_TX_CTRL_ENABLED_POS        (31u) /* [31]  Transmitter enabled               */
#define i2c_TX_CTRL_DATA_WIDTH_MASK    ((uint32) 0x0Fu)
#define i2c_TX_CTRL_MSB_FIRST          ((uint32) ((uint32) 0x01u << \
                                                                        i2c_TX_CTRL_MSB_FIRST_POS))
#define i2c_TX_CTRL_LSB_FIRST          ((uint32) 0x00u)
#define i2c_TX_CTRL_ENABLED            ((uint32) ((uint32) 0x01u << i2c_TX_CTRL_ENABLED_POS))


/* i2c_TX_CTRL_FIFO_REG */
#define i2c_TX_FIFO_CTRL_TRIGGER_LEVEL_POS     (0u)  /* [2:0] Trigger level                              */
#define i2c_TX_FIFO_CTRL_CLEAR_POS             (16u) /* [16]  Clear TX FIFO: claared after set           */
#define i2c_TX_FIFO_CTRL_FREEZE_POS            (17u) /* [17]  Freeze TX FIFO: HW do not inc read pointer */
#define i2c_TX_FIFO_CTRL_TRIGGER_LEVEL_MASK    ((uint32) 0x07u)
#define i2c_TX_FIFO_CTRL_CLEAR                 ((uint32) ((uint32) 0x01u << \
                                                                    i2c_TX_FIFO_CTRL_CLEAR_POS))
#define i2c_TX_FIFO_CTRL_FREEZE                ((uint32) ((uint32) 0x01u << \
                                                                    i2c_TX_FIFO_CTRL_FREEZE_POS))


/* i2c_TX_FIFO_STATUS_REG */
#define i2c_TX_FIFO_STATUS_USED_POS    (0u)  /* [3:0]   Amount of entries in TX FIFO */
#define i2c_TX_FIFO_SR_VALID_POS       (15u) /* [15]    Shifter status of TX FIFO    */
#define i2c_TX_FIFO_STATUS_RD_PTR_POS  (16u) /* [18:16] TX FIFO read pointer         */
#define i2c_TX_FIFO_STATUS_WR_PTR_POS  (24u) /* [26:24] TX FIFO write pointer        */
#define i2c_TX_FIFO_STATUS_USED_MASK   ((uint32) 0x0Fu)
#define i2c_TX_FIFO_SR_VALID           ((uint32) ((uint32) 0x01u << \
                                                                    i2c_TX_FIFO_SR_VALID_POS))
#define i2c_TX_FIFO_STATUS_RD_PTR_MASK ((uint32) ((uint32) 0x07u << \
                                                                    i2c_TX_FIFO_STATUS_RD_PTR_POS))
#define i2c_TX_FIFO_STATUS_WR_PTR_MASK ((uint32) ((uint32) 0x07u << \
                                                                    i2c_TX_FIFO_STATUS_WR_PTR_POS))


/* i2c_TX_FIFO_WR_REG */
#define i2c_TX_FIFO_WR_POS    (0u)  /* [15:0] Data written into TX FIFO */
#define i2c_TX_FIFO_WR_MASK   ((uint32) 0xFFu)


/* i2c_RX_CTRL_REG */
#define i2c_RX_CTRL_DATA_WIDTH_POS     (0u)  /* [3:0] Dataframe width: (Data width - 1) */
#define i2c_RX_CTRL_MSB_FIRST_POS      (8u)  /* [8]   MSB first shifter-out             */
#define i2c_RX_CTRL_MEDIAN_POS         (9u)  /* [9]   Median filter                     */
#define i2c_RX_CTRL_ENABLED_POS        (31u) /* [31]  Receiver enabled                  */
#define i2c_RX_CTRL_DATA_WIDTH_MASK    ((uint32) 0x0Fu)
#define i2c_RX_CTRL_MSB_FIRST          ((uint32) ((uint32) 0x01u << \
                                                                        i2c_RX_CTRL_MSB_FIRST_POS))
#define i2c_RX_CTRL_LSB_FIRST          ((uint32) 0x00u)
#define i2c_RX_CTRL_MEDIAN             ((uint32) ((uint32) 0x01u << i2c_RX_CTRL_MEDIAN_POS))
#define i2c_RX_CTRL_ENABLED            ((uint32) ((uint32) 0x01u << i2c_RX_CTRL_ENABLED_POS))


/* i2c_RX_FIFO_CTRL_REG */
#define i2c_RX_FIFO_CTRL_TRIGGER_LEVEL_POS     (0u)   /* [2:0] Trigger level                            */
#define i2c_RX_FIFO_CTRL_CLEAR_POS             (16u)  /* [16]  Clear RX FIFO: claar after set           */
#define i2c_RX_FIFO_CTRL_FREEZE_POS            (17u)  /* [17]  Freeze RX FIFO: HW writes has not effect */
#define i2c_RX_FIFO_CTRL_TRIGGER_LEVEL_MASK    ((uint32) 0x07u)
#define i2c_RX_FIFO_CTRL_CLEAR                 ((uint32) ((uint32) 0x01u << \
                                                                    i2c_RX_FIFO_CTRL_CLEAR_POS))
#define i2c_RX_FIFO_CTRL_FREEZE                ((uint32) ((uint32) 0x01u << \
                                                                    i2c_RX_FIFO_CTRL_FREEZE_POS))


/* i2c_RX_FIFO_STATUS_REG */
#define i2c_RX_FIFO_STATUS_USED_POS    (0u)   /* [3:0]   Amount of entries in RX FIFO */
#define i2c_RX_FIFO_SR_VALID_POS       (15u)  /* [15]    Shifter status of RX FIFO    */
#define i2c_RX_FIFO_STATUS_RD_PTR_POS  (16u)  /* [18:16] RX FIFO read pointer         */
#define i2c_RX_FIFO_STATUS_WR_PTR_POS  (24u)  /* [26:24] RX FIFO write pointer        */
#define i2c_RX_FIFO_STATUS_USED_MASK   ((uint32) 0x0Fu)
#define i2c_RX_FIFO_SR_VALID           ((uint32) ((uint32) 0x01u << \
                                                                  i2c_RX_FIFO_SR_VALID_POS))
#define i2c_RX_FIFO_STATUS_RD_PTR_MASK ((uint32) ((uint32) 0x07u << \
                                                                  i2c_RX_FIFO_STATUS_RD_PTR_POS))
#define i2c_RX_FIFO_STATUS_WR_PTR_MASK ((uint32) ((uint32) 0x07u << \
                                                                  i2c_RX_FIFO_STATUS_WR_PTR_POS))


/* i2c_RX_MATCH_REG */
#define i2c_RX_MATCH_ADDR_POS     (0u)  /* [7:0]   Slave address                        */
#define i2c_RX_MATCH_MASK_POS     (16u) /* [23:16] Slave address mask: 0 - doesn't care */
#define i2c_RX_MATCH_ADDR_MASK    ((uint32) 0xFFu)
#define i2c_RX_MATCH_MASK_MASK    ((uint32) ((uint32) 0xFFu << i2c_RX_MATCH_MASK_POS))


/* i2c_RX_FIFO_WR_REG */
#define i2c_RX_FIFO_RD_POS    (0u)  /* [15:0] Data read from RX FIFO */
#define i2c_RX_FIFO_RD_MASK   ((uint32) 0xFFu)


/* i2c_RX_FIFO_RD_SILENT_REG */
#define i2c_RX_FIFO_RD_SILENT_POS     (0u)  /* [15:0] Data read from RX FIFO: not remove data from FIFO */
#define i2c_RX_FIFO_RD_SILENT_MASK    ((uint32) 0xFFu)

/* i2c_RX_FIFO_RD_SILENT_REG */
#define i2c_RX_FIFO_RD_SILENT_POS     (0u)  /* [15:0] Data read from RX FIFO: not remove data from FIFO */
#define i2c_RX_FIFO_RD_SILENT_MASK    ((uint32) 0xFFu)

/* i2c_EZBUF_DATA_REG */
#define i2c_EZBUF_DATA_POS   (0u)  /* [7:0] Data from Ez Memory */
#define i2c_EZBUF_DATA_MASK  ((uint32) 0xFFu)

/*  i2c_INTR_CAUSE_REG */
#define i2c_INTR_CAUSE_MASTER_POS  (0u)  /* [0] Master interrupt active                 */
#define i2c_INTR_CAUSE_SLAVE_POS   (1u)  /* [1] Slave interrupt active                  */
#define i2c_INTR_CAUSE_TX_POS      (2u)  /* [2] Transmitter interrupt active            */
#define i2c_INTR_CAUSE_RX_POS      (3u)  /* [3] Receiver interrupt active               */
#define i2c_INTR_CAUSE_I2C_EC_POS  (4u)  /* [4] Externally clock I2C interrupt active   */
#define i2c_INTR_CAUSE_SPI_EC_POS  (5u)  /* [5] Externally clocked SPI interrupt active */
#define i2c_INTR_CAUSE_MASTER      ((uint32) 0x01u)
#define i2c_INTR_CAUSE_SLAVE       ((uint32) ((uint32) 0x01u << i2c_INTR_CAUSE_SLAVE_POS))
#define i2c_INTR_CAUSE_TX          ((uint32) ((uint32) 0x01u << i2c_INTR_CAUSE_TX_POS))
#define i2c_INTR_CAUSE_RX          ((uint32) ((uint32) 0x01u << i2c_INTR_CAUSE_RX_POS))
#define i2c_INTR_CAUSE_I2C_EC      ((uint32) ((uint32) 0x01u << i2c_INTR_CAUSE_I2C_EC_POS))
#define i2c_INTR_CAUSE_SPI_EC      ((uint32) ((uint32) 0x01u << i2c_INTR_CAUSE_SPI_EC_POS))


/* i2c_INTR_SPI_EC_REG, i2c_INTR_SPI_EC_MASK_REG, i2c_INTR_SPI_EC_MASKED_REG */
#define i2c_INTR_SPI_EC_WAKE_UP_POS          (0u)  /* [0] Address match: triggers wakeup of chip */
#define i2c_INTR_SPI_EC_EZBUF_STOP_POS       (1u)  /* [1] Externally clocked Stop detected       */
#define i2c_INTR_SPI_EC_EZBUF_WRITE_STOP_POS (2u)  /* [2] Externally clocked Write Stop detected */
#define i2c_INTR_SPI_EC_WAKE_UP              ((uint32) 0x01u)
#define i2c_INTR_SPI_EC_EZBUF_STOP           ((uint32) ((uint32) 0x01u << \
                                                                    i2c_INTR_SPI_EC_EZBUF_STOP_POS))
#define i2c_INTR_SPI_EC_EZBUF_WRITE_STOP     ((uint32) ((uint32) 0x01u << \
                                                                    i2c_INTR_SPI_EC_EZBUF_WRITE_STOP_POS))


/* i2c_INTR_I2C_EC, i2c_INTR_I2C_EC_MASK, i2c_INTR_I2C_EC_MASKED */
#define i2c_INTR_I2C_EC_WAKE_UP_POS          (0u)  /* [0] Address match: triggers wakeup of chip */
#define i2c_INTR_I2C_EC_EZBUF_STOP_POS       (1u)  /* [1] Externally clocked Stop detected       */
#define i2c_INTR_I2C_EC_EZBUF_WRITE_STOP_POS (2u)  /* [2] Externally clocked Write Stop detected */
#define i2c_INTR_I2C_EC_WAKE_UP              ((uint32) 0x01u)
#define i2c_INTR_I2C_EC_EZBUF_STOP           ((uint32) ((uint32) 0x01u << \
                                                                    i2c_INTR_I2C_EC_EZBUF_STOP_POS))
#define i2c_INTR_I2C_EC_EZBUF_WRITE_STOP     ((uint32) ((uint32) 0x01u << \
                                                                    i2c_INTR_I2C_EC_EZBUF_WRITE_STOP_POS))


/* i2c_INTR_MASTER, i2c_INTR_MASTER_SET,
   i2c_INTR_MASTER_MASK, i2c_INTR_MASTER_MASKED */
#define i2c_INTR_MASTER_I2C_ARB_LOST_POS   (0u)  /* [0] Master lost arbitration                          */
#define i2c_INTR_MASTER_I2C_NACK_POS       (1u)  /* [1] Master receives NACK: address or write to slave  */
#define i2c_INTR_MASTER_I2C_ACK_POS        (2u)  /* [2] Master receives NACK: address or write to slave  */
#define i2c_INTR_MASTER_I2C_STOP_POS       (4u)  /* [4] Master detects the Stop: only self generated Stop*/
#define i2c_INTR_MASTER_I2C_BUS_ERROR_POS  (8u)  /* [8] Master detects bus error: misplaced Start or Stop*/
#define i2c_INTR_MASTER_SPI_DONE_POS       (9u)  /* [9] Master complete trasfer: Only for SPI            */
#define i2c_INTR_MASTER_I2C_ARB_LOST       ((uint32) 0x01u)
#define i2c_INTR_MASTER_I2C_NACK           ((uint32) ((uint32) 0x01u << \
                                                                    i2c_INTR_MASTER_I2C_NACK_POS))
#define i2c_INTR_MASTER_I2C_ACK            ((uint32) ((uint32) 0x01u << \
                                                                    i2c_INTR_MASTER_I2C_ACK_POS))
#define i2c_INTR_MASTER_I2C_STOP           ((uint32) ((uint32) 0x01u << \
                                                                    i2c_INTR_MASTER_I2C_STOP_POS))
#define i2c_INTR_MASTER_I2C_BUS_ERROR      ((uint32) ((uint32) 0x01u << \
                                                                    i2c_INTR_MASTER_I2C_BUS_ERROR_POS))
#define i2c_INTR_MASTER_SPI_DONE           ((uint32) ((uint32) 0x01u << \
                                                                    i2c_INTR_MASTER_SPI_DONE_POS))

/*
* i2c_INTR_SLAVE, i2c_INTR_SLAVE_SET,
* i2c_INTR_SLAVE_MASK, i2c_INTR_SLAVE_MASKED
*/
#define i2c_INTR_SLAVE_I2C_ARB_LOST_POS         (0u)  /* [0]  Slave lost arbitration                   */
#define i2c_INTR_SLAVE_I2C_NACK_POS             (1u)  /* [1]  Slave receives NACK: master reads data   */
#define i2c_INTR_SLAVE_I2C_ACK_POS              (2u)  /* [2]  Slave receives ACK: master reads data    */
#define i2c_INTR_SLAVE_I2C_WRITE_STOP_POS       (3u)  /* [3]  Slave detects end of write transaction   */
#define i2c_INTR_SLAVE_I2C_STOP_POS             (4u)  /* [4]  Slave detects end of transaction intened */
#define i2c_INTR_SLAVE_I2C_START_POS            (5u)  /* [5]  Slave detects Start                      */
#define i2c_INTR_SLAVE_I2C_ADDR_MATCH_POS       (6u)  /* [6]  Slave address matches                    */
#define i2c_INTR_SLAVE_I2C_GENERAL_POS          (7u)  /* [7]  General call received                    */
#define i2c_INTR_SLAVE_I2C_BUS_ERROR_POS        (8u)  /* [8]  Slave detects bus error                  */
#define i2c_INTR_SLAVE_SPI_EZBUF_WRITE_STOP_POS (9u)  /* [9]  Slave write complete: Only for SPI       */
#define i2c_INTR_SLAVE_SPI_EZBUF_STOP_POS       (10u) /* [10] Slave end of transaciton: Only for SPI   */
#define i2c_INTR_SLAVE_SPI_BUS_ERROR_POS        (11u) /* [11] Slave detects bus error: Only for SPI    */
#define i2c_INTR_SLAVE_I2C_ARB_LOST             ((uint32) 0x01u)
#define i2c_INTR_SLAVE_I2C_NACK                 ((uint32) ((uint32) 0x01u << \
                                                                    i2c_INTR_SLAVE_I2C_NACK_POS))
#define i2c_INTR_SLAVE_I2C_ACK                  ((uint32) ((uint32) 0x01u << \
                                                                    i2c_INTR_SLAVE_I2C_ACK_POS))
#define i2c_INTR_SLAVE_I2C_WRITE_STOP           ((uint32) ((uint32) 0x01u << \
                                                                    i2c_INTR_SLAVE_I2C_WRITE_STOP_POS))
#define i2c_INTR_SLAVE_I2C_STOP                 ((uint32) ((uint32) 0x01u << \
                                                                    i2c_INTR_SLAVE_I2C_STOP_POS))
#define i2c_INTR_SLAVE_I2C_START                ((uint32) ((uint32) 0x01u << \
                                                                    i2c_INTR_SLAVE_I2C_START_POS))
#define i2c_INTR_SLAVE_I2C_ADDR_MATCH           ((uint32) ((uint32) 0x01u << \
                                                                    i2c_INTR_SLAVE_I2C_ADDR_MATCH_POS))
#define i2c_INTR_SLAVE_I2C_GENERAL              ((uint32) ((uint32) 0x01u << \
                                                                    i2c_INTR_SLAVE_I2C_GENERAL_POS))
#define i2c_INTR_SLAVE_I2C_BUS_ERROR            ((uint32) ((uint32) 0x01u << \
                                                                    i2c_INTR_SLAVE_I2C_BUS_ERROR_POS))
#define i2c_INTR_SLAVE_SPI_EZBUF_WRITE_STOP     ((uint32) ((uint32) 0x01u << \
                                                                    i2c_INTR_SLAVE_SPI_EZBUF_WRITE_STOP_POS))
#define i2c_INTR_SLAVE_SPI_EZBUF_STOP           ((uint32) ((uint32) 0x01u << \
                                                                    i2c_INTR_SLAVE_SPI_EZBUF_STOP_POS))
#define i2c_INTR_SLAVE_SPI_BUS_ERROR           ((uint32) ((uint32) 0x01u << \
                                                                    i2c_INTR_SLAVE_SPI_BUS_ERROR_POS))


/*
* i2c_INTR_TX, i2c_INTR_TX_SET,
* i2c_INTR_TX_MASK, i2c_INTR_TX_MASKED
*/
#define i2c_INTR_TX_TRIGGER_POS        (0u)  /* [0]  Trigger on TX FIFO entires                       */
#define i2c_INTR_TX_NOT_FULL_POS       (1u)  /* [1]  TX FIFO is not full                              */
#define i2c_INTR_TX_EMPTY_POS          (4u)  /* [4]  TX FIFO is empty                                 */
#define i2c_INTR_TX_OVERFLOW_POS       (5u)  /* [5]  Attempt to write to a full TX FIFO               */
#define i2c_INTR_TX_UNDERFLOW_POS      (6u)  /* [6]  Attempt to read from an empty TX FIFO            */
#define i2c_INTR_TX_BLOCKED_POS        (7u)  /* [7]  No access to the EZ memory                       */
#define i2c_INTR_TX_UART_NACK_POS      (8u)  /* [8]  UART transmitter received a NACK: SmartCard mode */
#define i2c_INTR_TX_UART_DONE_POS      (9u)  /* [9]  UART transmitter done even                       */
#define i2c_INTR_TX_UART_ARB_LOST_POS  (10u) /* [10] UART lost arbitration: LIN or SmartCard          */
#define i2c_INTR_TX_TRIGGER            ((uint32) 0x01u)
#define i2c_INTR_TX_NOT_FULL           ((uint32) ((uint32) 0x01u << \
                                                                        i2c_INTR_TX_NOT_FULL_POS))
#define i2c_INTR_TX_EMPTY              ((uint32) ((uint32) 0x01u << \
                                                                        i2c_INTR_TX_EMPTY_POS))
#define i2c_INTR_TX_OVERFLOW           ((uint32) ((uint32) 0x01u << \
                                                                        i2c_INTR_TX_OVERFLOW_POS))
#define i2c_INTR_TX_UNDERFLOW          ((uint32) ((uint32) 0x01u << \
                                                                        i2c_INTR_TX_UNDERFLOW_POS))
#define i2c_INTR_TX_BLOCKED            ((uint32) ((uint32) 0x01u << \
                                                                        i2c_INTR_TX_BLOCKED_POS))
#define i2c_INTR_TX_UART_NACK          ((uint32) ((uint32) 0x01u << \
                                                                        i2c_INTR_TX_UART_NACK_POS))
#define i2c_INTR_TX_UART_DONE          ((uint32) ((uint32) 0x01u << \
                                                                        i2c_INTR_TX_UART_DONE_POS))
#define i2c_INTR_TX_UART_ARB_LOST      ((uint32) ((uint32) 0x01u << \
                                                                        i2c_INTR_TX_UART_ARB_LOST_POS))


/*
* i2c_INTR_RX, i2c_INTR_RX_SET,
* i2c_INTR_RX_MASK, i2c_INTR_RX_MASKED
*/
#define i2c_INTR_RX_TRIGGER_POS        (0u)   /* [0]  Trigger on RX FIFO entires            */
#define i2c_INTR_RX_NOT_EMPTY_POS      (2u)   /* [2]  RX FIFO is not empty                  */
#define i2c_INTR_RX_FULL_POS           (3u)   /* [3]  RX FIFO is full                       */
#define i2c_INTR_RX_OVERFLOW_POS       (5u)   /* [5]  Attempt to write to a full RX FIFO    */
#define i2c_INTR_RX_UNDERFLOW_POS      (6u)   /* [6]  Attempt to read from an empty RX FIFO */
#define i2c_INTR_RX_BLOCKED_POS        (7u)   /* [7]  No access to the EZ memory            */
#define i2c_INTR_RX_FRAME_ERROR_POS    (8u)   /* [8]  Frame error in received data frame    */
#define i2c_INTR_RX_PARITY_ERROR_POS   (9u)   /* [9]  Parity error in received data frame   */
#define i2c_INTR_RX_BAUD_DETECT_POS    (10u)  /* [10] LIN baudrate detection is completed   */
#define i2c_INTR_RX_BREAK_DETECT_POS   (11u)  /* [11] Break detection is successful         */
#define i2c_INTR_RX_TRIGGER            ((uint32) 0x01u)
#define i2c_INTR_RX_NOT_EMPTY          ((uint32) ((uint32) 0x01u << \
                                                                        i2c_INTR_RX_NOT_EMPTY_POS))
#define i2c_INTR_RX_FULL               ((uint32) ((uint32) 0x01u << \
                                                                        i2c_INTR_RX_FULL_POS))
#define i2c_INTR_RX_OVERFLOW           ((uint32) ((uint32) 0x01u << \
                                                                        i2c_INTR_RX_OVERFLOW_POS))
#define i2c_INTR_RX_UNDERFLOW          ((uint32) ((uint32) 0x01u << \
                                                                        i2c_INTR_RX_UNDERFLOW_POS))
#define i2c_INTR_RX_BLOCKED            ((uint32) ((uint32) 0x01u << \
                                                                        i2c_INTR_RX_BLOCKED_POS))
#define i2c_INTR_RX_FRAME_ERROR        ((uint32) ((uint32) 0x01u << \
                                                                        i2c_INTR_RX_FRAME_ERROR_POS))
#define i2c_INTR_RX_PARITY_ERROR       ((uint32) ((uint32) 0x01u << \
                                                                        i2c_INTR_RX_PARITY_ERROR_POS))
#define i2c_INTR_RX_BAUD_DETECT        ((uint32) ((uint32) 0x01u << \
                                                                        i2c_INTR_RX_BAUD_DETECT_POS))
#define i2c_INTR_RX_BREAK_DETECT       ((uint32) ((uint32) 0x01u << \
                                                                        i2c_INTR_RX_BREAK_DETECT_POS))


/* Define all interupt soureces */
#define i2c_INTR_I2C_EC_ALL    (i2c_INTR_I2C_EC_WAKE_UP    | \
                                             i2c_INTR_I2C_EC_EZBUF_STOP | \
                                             i2c_INTR_I2C_EC_EZBUF_WRITE_STOP)

#define i2c_INTR_SPI_EC_ALL    (i2c_INTR_SPI_EC_WAKE_UP    | \
                                             i2c_INTR_SPI_EC_EZBUF_STOP | \
                                             i2c_INTR_SPI_EC_EZBUF_WRITE_STOP)

#define i2c_INTR_MASTER_ALL    (i2c_INTR_MASTER_I2C_ARB_LOST  | \
                                             i2c_INTR_MASTER_I2C_NACK      | \
                                             i2c_INTR_MASTER_I2C_ACK       | \
                                             i2c_INTR_MASTER_I2C_STOP      | \
                                             i2c_INTR_MASTER_I2C_BUS_ERROR | \
                                             i2c_INTR_MASTER_SPI_DONE )

#define i2c_INTR_SLAVE_ALL     (i2c_INTR_SLAVE_I2C_ARB_LOST      | \
                                             i2c_INTR_SLAVE_I2C_NACK          | \
                                             i2c_INTR_SLAVE_I2C_ACK           | \
                                             i2c_INTR_SLAVE_I2C_WRITE_STOP    | \
                                             i2c_INTR_SLAVE_I2C_STOP          | \
                                             i2c_INTR_SLAVE_I2C_START         | \
                                             i2c_INTR_SLAVE_I2C_ADDR_MATCH    | \
                                             i2c_INTR_SLAVE_I2C_GENERAL       | \
                                             i2c_INTR_SLAVE_I2C_BUS_ERROR     | \
                                             i2c_INTR_SLAVE_SPI_EZBUF_WRITE_STOP | \
                                             i2c_INTR_SLAVE_SPI_EZBUF_STOP       | \
                                             i2c_INTR_SLAVE_SPI_BUS_ERROR)

#define i2c_INTR_TX_ALL        (i2c_INTR_TX_TRIGGER   | \
                                             i2c_INTR_TX_NOT_FULL  | \
                                             i2c_INTR_TX_EMPTY     | \
                                             i2c_INTR_TX_OVERFLOW  | \
                                             i2c_INTR_TX_UNDERFLOW | \
                                             i2c_INTR_TX_BLOCKED   | \
                                             i2c_INTR_TX_UART_NACK | \
                                             i2c_INTR_TX_UART_DONE | \
                                             i2c_INTR_TX_UART_ARB_LOST)

#define i2c_INTR_RX_ALL        (i2c_INTR_RX_TRIGGER      | \
                                             i2c_INTR_RX_NOT_EMPTY    | \
                                             i2c_INTR_RX_FULL         | \
                                             i2c_INTR_RX_OVERFLOW     | \
                                             i2c_INTR_RX_UNDERFLOW    | \
                                             i2c_INTR_RX_BLOCKED      | \
                                             i2c_INTR_RX_FRAME_ERROR  | \
                                             i2c_INTR_RX_PARITY_ERROR | \
                                             i2c_INTR_RX_BAUD_DETECT  | \
                                             i2c_INTR_RX_BREAK_DETECT)

/* General usage HW definitions */
#define i2c_ONE_BYTE_WIDTH (8u)   /* Number of bits in one byte           */
#define i2c_FIFO_SIZE      (8u)   /* Size of TX or RX FIFO: defined by HW */
#define i2c_EZBUFFER_SIZE  (32u)  /* EZ Buffer size: defined by HW        */

/* I2C and EZI2C slave address defines */
#define i2c_I2C_SLAVE_ADDR_POS    (0x01u)    /* 7-bit address shift */
#define i2c_I2C_SLAVE_ADDR_MASK   (0xFEu)    /* 8-bit address mask */

/* OVS constants for IrDA Low Power operation */
#define i2c_CTRL_OVS_IRDA_LP_OVS16     (0x00u)
#define i2c_CTRL_OVS_IRDA_LP_OVS32     (0x01u)
#define i2c_CTRL_OVS_IRDA_LP_OVS48     (0x02u)
#define i2c_CTRL_OVS_IRDA_LP_OVS96     (0x03u)
#define i2c_CTRL_OVS_IRDA_LP_OVS192    (0x04u)
#define i2c_CTRL_OVS_IRDA_LP_OVS768    (0x05u)
#define i2c_CTRL_OVS_IRDA_LP_OVS1536   (0x06u)

/* OVS constant for IrDA */
#define i2c_CTRL_OVS_IRDA_OVS16        (i2c_UART_IRDA_LP_OVS16)


/***************************************
*    SCB Common Macro Definitions
***************************************/

/*
* Re-enables SCB IP: this cause partial reset of IP: state, status, TX and RX FIFOs.
* The triggered interrupts remains set.
*/
#define i2c_SCB_SW_RESET \
                        do{ \
                            i2c_CTRL_REG &= ((uint32) ~i2c_CTRL_ENABLED ); \
                            i2c_CTRL_REG |= ((uint32)  i2c_CTRL_ENABLED ); \
                        }while(0)

/* TX FIFO macro */
#define i2c_CLEAR_TX_FIFO \
                            do{        \
                                i2c_TX_FIFO_CTRL_REG |= ((uint32)  i2c_TX_FIFO_CTRL_CLEAR); \
                                i2c_TX_FIFO_CTRL_REG &= ((uint32) ~i2c_TX_FIFO_CTRL_CLEAR); \
                            }while(0)

#define i2c_GET_TX_FIFO_ENTRIES    (i2c_TX_FIFO_STATUS_REG & \
                                                 i2c_TX_FIFO_STATUS_USED_MASK)

#define i2c_GET_TX_FIFO_SR_VALID   ((0u != (i2c_TX_FIFO_STATUS_REG & \
                                                         i2c_TX_FIFO_SR_VALID)) ? (1u) : (0u))

/* RX FIFO macro */
#define i2c_CLEAR_RX_FIFO \
                            do{        \
                                i2c_RX_FIFO_CTRL_REG |= ((uint32)  i2c_RX_FIFO_CTRL_CLEAR); \
                                i2c_RX_FIFO_CTRL_REG &= ((uint32) ~i2c_RX_FIFO_CTRL_CLEAR); \
                            }while(0)

#define i2c_GET_RX_FIFO_ENTRIES    (i2c_RX_FIFO_STATUS_REG & \
                                                    i2c_RX_FIFO_STATUS_USED_MASK)

#define i2c_GET_RX_FIFO_SR_VALID   ((0u != (i2c_RX_FIFO_STATUS_REG & \
                                                         i2c_RX_FIFO_SR_VALID)) ? (1u) : (0u))

/* Writes interrupt source: set sourceMask bits in i2c_INTR_X_MASK_REG */
#define i2c_WRITE_INTR_I2C_EC_MASK(sourceMask) \
                                                do{         \
                                                    i2c_INTR_I2C_EC_MASK_REG = (uint32) (sourceMask); \
                                                }while(0)

#define i2c_WRITE_INTR_SPI_EC_MASK(sourceMask) \
                                                do{         \
                                                    i2c_INTR_SPI_EC_MASK_REG = (uint32) (sourceMask); \
                                                }while(0)

#define i2c_WRITE_INTR_MASTER_MASK(sourceMask) \
                                                do{         \
                                                    i2c_INTR_MASTER_MASK_REG = (uint32) (sourceMask); \
                                                }while(0)

#define i2c_WRITE_INTR_SLAVE_MASK(sourceMask)  \
                                                do{         \
                                                    i2c_INTR_SLAVE_MASK_REG = (uint32) (sourceMask); \
                                                }while(0)

#define i2c_WRITE_INTR_TX_MASK(sourceMask)     \
                                                do{         \
                                                    i2c_INTR_TX_MASK_REG = (uint32) (sourceMask); \
                                                }while(0)

#define i2c_WRITE_INTR_RX_MASK(sourceMask)     \
                                                do{         \
                                                    i2c_INTR_RX_MASK_REG = (uint32) (sourceMask); \
                                                }while(0)

/* Enables interrupt source: set sourceMask bits in i2c_INTR_X_MASK_REG */
#define i2c_ENABLE_INTR_I2C_EC(sourceMask) \
                                                do{     \
                                                    i2c_INTR_I2C_EC_MASK_REG |= (uint32) (sourceMask); \
                                                }while(0)

#define i2c_ENABLE_INTR_SPI_EC(sourceMask) \
                                                do{     \
                                                    i2c_INTR_SPI_EC_MASK_REG |= (uint32) (sourceMask); \
                                                }while(0)

#define i2c_ENABLE_INTR_MASTER(sourceMask) \
                                                do{     \
                                                    i2c_INTR_MASTER_MASK_REG |= (uint32) (sourceMask); \
                                                }while(0)

#define i2c_ENABLE_INTR_SLAVE(sourceMask)  \
                                                do{     \
                                                    i2c_INTR_SLAVE_MASK_REG |= (uint32) (sourceMask); \
                                                }while(0)

#define i2c_ENABLE_INTR_TX(sourceMask)     \
                                                do{     \
                                                    i2c_INTR_TX_MASK_REG |= (uint32) (sourceMask); \
                                                }while(0)

#define i2c_ENABLE_INTR_RX(sourceMask)     \
                                                do{     \
                                                    i2c_INTR_RX_MASK_REG |= (uint32) (sourceMask); \
                                                }while(0)

/* Disables interrupt source: clear sourceMask bits in i2c_INTR_X_MASK_REG */
#define i2c_DISABLE_INTR_I2C_EC(sourceMask) \
                                do{                      \
                                    i2c_INTR_I2C_EC_MASK_REG &= ((uint32) ~((uint32) (sourceMask))); \
                                }while(0)

#define i2c_DISABLE_INTR_SPI_EC(sourceMask) \
                                do{                      \
                                    i2c_INTR_SPI_EC_MASK_REG &= ((uint32) ~((uint32) (sourceMask))); \
                                 }while(0)

#define i2c_DISABLE_INTR_MASTER(sourceMask) \
                                do{                      \
                                i2c_INTR_MASTER_MASK_REG &= ((uint32) ~((uint32) (sourceMask))); \
                                }while(0)

#define i2c_DISABLE_INTR_SLAVE(sourceMask) \
                                do{                     \
                                    i2c_INTR_SLAVE_MASK_REG &= ((uint32) ~((uint32) (sourceMask))); \
                                }while(0)

#define i2c_DISABLE_INTR_TX(sourceMask)    \
                                do{                     \
                                    i2c_INTR_TX_MASK_REG &= ((uint32) ~((uint32) (sourceMask))); \
                                 }while(0)

#define i2c_DISABLE_INTR_RX(sourceMask)    \
                                do{                     \
                                    i2c_INTR_RX_MASK_REG &= ((uint32) ~((uint32) (sourceMask))); \
                                }while(0)

/* Sets interrupt sources: write sourceMask bits in i2c_INTR_X_SET_REG */
#define i2c_SET_INTR_MASTER(sourceMask)    \
                                                do{     \
                                                    i2c_INTR_MASTER_SET_REG = (uint32) (sourceMask); \
                                                }while(0)

#define i2c_SET_INTR_SLAVE(sourceMask) \
                                                do{ \
                                                    i2c_INTR_SLAVE_SET_REG = (uint32) (sourceMask); \
                                                }while(0)

#define i2c_SET_INTR_TX(sourceMask)    \
                                                do{ \
                                                    i2c_INTR_TX_SET_REG = (uint32) (sourceMask); \
                                                }while(0)

#define i2c_SET_INTR_RX(sourceMask)    \
                                                do{ \
                                                    i2c_INTR_RX_SET_REG = (uint32) (sourceMask); \
                                                }while(0)

/* Clears interrupt sources: write sourceMask bits in i2c_INTR_X_REG */
#define i2c_CLEAR_INTR_I2C_EC(sourceMask)  \
                                                do{     \
                                                    i2c_INTR_I2C_EC_REG = (uint32) (sourceMask); \
                                                }while(0)

#define i2c_CLEAR_INTR_SPI_EC(sourceMask)  \
                                                do{     \
                                                    i2c_INTR_SPI_EC_REG = (uint32) (sourceMask); \
                                                }while(0)

#define i2c_CLEAR_INTR_MASTER(sourceMask)  \
                                                do{     \
                                                    i2c_INTR_MASTER_REG = (uint32) (sourceMask); \
                                                }while(0)

#define i2c_CLEAR_INTR_SLAVE(sourceMask)   \
                                                do{     \
                                                    i2c_INTR_SLAVE_REG  = (uint32) (sourceMask); \
                                                }while(0)

#define i2c_CLEAR_INTR_TX(sourceMask)      \
                                                do{     \
                                                    i2c_INTR_TX_REG     = (uint32) (sourceMask); \
                                                }while(0)

#define i2c_CLEAR_INTR_RX(sourceMask)      \
                                                do{     \
                                                    i2c_INTR_RX_REG     = (uint32) (sourceMask); \
                                                }while(0)

/* Return true if sourceMask is set in i2c_INTR_CAUSE_REG */
#define i2c_CHECK_CAUSE_INTR(sourceMask)    (0u != (i2c_INTR_CAUSE_REG & (sourceMask)))

/* Return true if sourceMask is set in  INTR_X_MASKED_REG */
#define i2c_CHECK_INTR_EC_I2C(sourceMask)  (0u != (i2c_INTR_I2C_EC_REG & (sourceMask)))
#define i2c_CHECK_INTR_EC_SPI(sourceMask)  (0u != (i2c_INTR_SPI_EC_REG & (sourceMask)))
#define i2c_CHECK_INTR_MASTER(sourceMask)  (0u != (i2c_INTR_MASTER_REG & (sourceMask)))
#define i2c_CHECK_INTR_SLAVE(sourceMask)   (0u != (i2c_INTR_SLAVE_REG  & (sourceMask)))
#define i2c_CHECK_INTR_TX(sourceMask)      (0u != (i2c_INTR_TX_REG     & (sourceMask)))
#define i2c_CHECK_INTR_RX(sourceMask)      (0u != (i2c_INTR_RX_REG     & (sourceMask)))

/* Return true if sourceMask is set in i2c_INTR_X_MASKED_REG */
#define i2c_CHECK_INTR_I2C_EC_MASKED(sourceMask)   (0u != (i2c_INTR_I2C_EC_MASKED_REG & \
                                                                       (sourceMask)))
#define i2c_CHECK_INTR_SPI_EC_MASKED(sourceMask)   (0u != (i2c_INTR_SPI_EC_MASKED_REG & \
                                                                       (sourceMask)))
#define i2c_CHECK_INTR_MASTER_MASKED(sourceMask)   (0u != (i2c_INTR_MASTER_MASKED_REG & \
                                                                       (sourceMask)))
#define i2c_CHECK_INTR_SLAVE_MASKED(sourceMask)    (0u != (i2c_INTR_SLAVE_MASKED_REG  & \
                                                                       (sourceMask)))
#define i2c_CHECK_INTR_TX_MASKED(sourceMask)       (0u != (i2c_INTR_TX_MASKED_REG     & \
                                                                       (sourceMask)))
#define i2c_CHECK_INTR_RX_MASKED(sourceMask)       (0u != (i2c_INTR_RX_MASKED_REG     & \
                                                                       (sourceMask)))

/* Return true if sourceMask is set in i2c_CTRL_REG: generaly is used to check enable bit */
#define i2c_GET_CTRL_ENABLED    (0u != (i2c_CTRL_REG & i2c_CTRL_ENABLED))

#define i2c_CHECK_SLAVE_AUTO_ADDR_NACK     (0u != (i2c_I2C_CTRL_REG & \
                                                                i2c_I2C_CTRL_S_NOT_READY_DATA_NACK))


/***************************************
*      I2C Macro Definitions
***************************************/

/* Enable auto ACK/NACK */
#define i2c_ENABLE_SLAVE_AUTO_ADDR_NACK \
                            do{                      \
                                i2c_I2C_CTRL_REG |= i2c_I2C_CTRL_S_NOT_READY_DATA_NACK; \
                            }while(0)

#define i2c_ENABLE_SLAVE_AUTO_DATA_ACK \
                            do{                     \
                                i2c_I2C_CTRL_REG |= i2c_I2C_CTRL_S_READY_DATA_ACK; \
                            }while(0)

#define i2c_ENABLE_SLAVE_AUTO_DATA_NACK \
                            do{                      \
                                i2c_I2C_CTRL_REG |= i2c_I2C_CTRL_S_NOT_READY_DATA_NACK; \
                            }while(0)

#define i2c_ENABLE_MASTER_AUTO_DATA_ACK \
                            do{                      \
                                i2c_I2C_CTRL_REG |= i2c_I2C_CTRL_M_READY_DATA_ACK; \
                            }while(0)

#define i2c_ENABLE_MASTER_AUTO_DATA_NACK \
                            do{                       \
                                i2c_I2C_CTRL_REG |= i2c_I2C_CTRL_M_NOT_READY_DATA_NACK; \
                            }while(0)

/* Disable auto ACK/NACK */
#define i2c_DISABLE_SLAVE_AUTO_ADDR_NACK \
                            do{                       \
                                i2c_I2C_CTRL_REG &= ~i2c_I2C_CTRL_S_NOT_READY_DATA_NACK; \
                            }while(0)

#define i2c_DISABLE_SLAVE_AUTO_DATA_ACK \
                            do{                      \
                                i2c_I2C_CTRL_REG &= ~i2c_I2C_CTRL_S_READY_DATA_ACK; \
                            }while(0)

#define i2c_DISABLE_SLAVE_AUTO_DATA_NACK \
                            do{                       \
                                i2c_I2C_CTRL_REG &= ~i2c_I2C_CTRL_S_NOT_READY_DATA_NACK; \
                            }while(0)

#define i2c_DISABLE_MASTER_AUTO_DATA_ACK \
                            do{                       \
                                i2c_I2C_CTRL_REG &= ~i2c_I2C_CTRL_M_READY_DATA_ACK; \
                            }while(0)

#define i2c_DISABLE_MASTER_AUTO_DATA_NACK \
                            do{                        \
                                i2c_I2C_CTRL_REG &= ~i2c_I2C_CTRL_M_NOT_READY_DATA_NACK; \
                            }while(0)

/* Enable Slave autoACK/NACK Data */
#define i2c_ENABLE_SLAVE_AUTO_DATA \
                            do{                 \
                                i2c_I2C_CTRL_REG |= (i2c_I2C_CTRL_S_READY_DATA_ACK |      \
                                                                  i2c_I2C_CTRL_S_NOT_READY_DATA_NACK); \
                            }while(0)

/* Disable Slave autoACK/NACK Data */
#define i2c_DISABLE_SLAVE_AUTO_DATA \
                            do{                  \
                                i2c_I2C_CTRL_REG &= ((uint32) \
                                                                  ~(i2c_I2C_CTRL_S_READY_DATA_ACK |       \
                                                                    i2c_I2C_CTRL_S_NOT_READY_DATA_NACK)); \
                            }while(0)

/* Disable Master autoACK/NACK Data */
#define i2c_DISABLE_MASTER_AUTO_DATA \
                            do{                   \
                                i2c_I2C_CTRL_REG &= ((uint32) \
                                                                  ~(i2c_I2C_CTRL_M_READY_DATA_ACK |       \
                                                                    i2c_I2C_CTRL_M_NOT_READY_DATA_NACK)); \
                            }while(0)

/* Master commands */
#define i2c_I2C_MASTER_GENERATE_START \
                            do{                    \
                                i2c_I2C_MASTER_CMD_REG = i2c_I2C_MASTER_CMD_M_START_ON_IDLE; \
                            }while(0)

#define i2c_I2C_MASTER_CLEAR_START \
                            do{                 \
                                i2c_I2C_MASTER_CMD_REG =  ((uint32) 0u); \
                            }while(0)

#define i2c_I2C_MASTER_GENERATE_RESTART i2c_I2CReStartGeneration()

#define i2c_I2C_MASTER_GENERATE_STOP \
                            do{                   \
                                i2c_I2C_MASTER_CMD_REG =                                            \
                                    (i2c_I2C_MASTER_CMD_M_STOP |                                    \
                                        (i2c_CHECK_I2C_STATUS(i2c_I2C_STATUS_M_READ) ? \
                                            (i2c_I2C_MASTER_CMD_M_NACK) : (0u)));                   \
                            }while(0)

#define i2c_I2C_MASTER_GENERATE_ACK \
                            do{                  \
                                i2c_I2C_MASTER_CMD_REG = i2c_I2C_MASTER_CMD_M_ACK; \
                            }while(0)

#define i2c_I2C_MASTER_GENERATE_NACK \
                            do{                   \
                                i2c_I2C_MASTER_CMD_REG = i2c_I2C_MASTER_CMD_M_NACK; \
                            }while(0)

/* Slave comamnds */
#define i2c_I2C_SLAVE_GENERATE_ACK \
                            do{                 \
                                i2c_I2C_SLAVE_CMD_REG = i2c_I2C_SLAVE_CMD_S_ACK; \
                            }while(0)

#define i2c_I2C_SLAVE_GENERATE_NACK \
                            do{                  \
                                i2c_I2C_SLAVE_CMD_REG = i2c_I2C_SLAVE_CMD_S_NACK; \
                            }while(0)


/* Return 8-bit address. The input address should be 7-bits */
#define i2c_GET_I2C_8BIT_ADDRESS(addr) (((uint32) ((uint32) (addr) << \
                                                                    i2c_I2C_SLAVE_ADDR_POS)) & \
                                                                        i2c_I2C_SLAVE_ADDR_MASK)

#define i2c_GET_I2C_7BIT_ADDRESS(addr) ((uint32) (addr) >> i2c_I2C_SLAVE_ADDR_POS)


/* Adjust SDA filter Trim settings */
#define i2c_DEFAULT_I2C_CFG_SDA_FILT_TRIM  (0x02u)
#define i2c_EC_AM_I2C_CFG_SDA_FILT_TRIM    (0x03u)

#define i2c_SET_I2C_CFG_SDA_FILT_TRIM(sdaTrim) \
        do{                                                 \
            i2c_I2C_CFG_REG =                  \
                            ((i2c_I2C_CFG_REG & (uint32) ~i2c_I2C_CFG_SDA_FILT_TRIM_MASK) | \
                             ((uint32) ((uint32) (sdaTrim) <<i2c_I2C_CFG_SDA_FILT_TRIM_POS)));           \
        }while(0)

/* Returns slave select number in SPI_CTRL */
#define i2c_GET_SPI_CTRL_SS(activeSelect) (((uint32) ((uint32) (activeSelect) << \
                                                                    i2c_SPI_CTRL_SLAVE_SELECT_POS)) & \
                                                                        i2c_SPI_CTRL_SLAVE_SELECT_MASK)

/* Returns true if bit is set in i2c_I2C_STATUS_REG */
#define i2c_CHECK_I2C_STATUS(sourceMask)   (0u != (i2c_I2C_STATUS_REG & (sourceMask)))

/* Returns true if bit is set in i2c_SPI_STATUS_REG */
#define i2c_CHECK_SPI_STATUS(sourceMask)   (0u != (i2c_SPI_STATUS_REG & (sourceMask)))


/***************************************
*       SCB Init Macros Definitions
***************************************/

/* i2c_CTRL */
#define i2c_GET_CTRL_OVS(oversample)   ((((uint32) (oversample)) - 1u) & i2c_CTRL_OVS_MASK)

#define i2c_GET_CTRL_EC_OP_MODE(opMode)        ((0u != (opMode)) ? \
                                                                (i2c_CTRL_EC_OP_MODE)  : (0u))

#define i2c_GET_CTRL_EC_AM_MODE(amMode)        ((0u != (amMode)) ? \
                                                                (i2c_CTRL_EC_AM_MODE)  : (0u))

#define i2c_GET_CTRL_BLOCK(block)              ((0u != (block))  ? \
                                                                (i2c_CTRL_BLOCK)       : (0u))

#define i2c_GET_CTRL_ADDR_ACCEPT(acceptAddr)   ((0u != (acceptAddr)) ? \
                                                                (i2c_CTRL_ADDR_ACCEPT) : (0u))

/* i2c_I2C_CTRL */
#define i2c_GET_I2C_CTRL_HIGH_PHASE_OVS(oversampleHigh) (((uint32) (oversampleHigh) - 1u) & \
                                                                        i2c_I2C_CTRL_HIGH_PHASE_OVS_MASK)

#define i2c_GET_I2C_CTRL_LOW_PHASE_OVS(oversampleLow)   (((uint32) (((uint32) (oversampleLow) - 1u) << \
                                                                    i2c_I2C_CTRL_LOW_PHASE_OVS_POS)) &  \
                                                                    i2c_I2C_CTRL_LOW_PHASE_OVS_MASK)

#define i2c_GET_I2C_CTRL_S_NOT_READY_ADDR_NACK(wakeNack) ((0u != (wakeNack)) ? \
                                                            (i2c_I2C_CTRL_S_NOT_READY_ADDR_NACK) : (0u))

#define i2c_GET_I2C_CTRL_SL_MSTR_MODE(mode)    ((uint32) ((uint32)(mode) << \
                                                                    i2c_I2C_CTRL_SLAVE_MODE_POS))

/* i2c_SPI_CTRL */
#define i2c_GET_SPI_CTRL_CONTINUOUS(separate)  ((0u != (separate)) ? \
                                                                (i2c_SPI_CTRL_CONTINUOUS) : (0u))

#define i2c_GET_SPI_CTRL_SELECT_PRECEDE(mode)  ((0u != (mode)) ? \
                                                                      (i2c_SPI_CTRL_SELECT_PRECEDE) : (0u))

#define i2c_GET_SPI_CTRL_SCLK_MODE(mode)       (((uint32) ((uint32) (mode) << \
                                                                        i2c_SPI_CTRL_CPHA_POS)) & \
                                                                        i2c_SPI_CTRL_SCLK_MODE_MASK)

#define i2c_GET_SPI_CTRL_LATE_MISO_SAMPLE(lateMiso) ((0u != (lateMiso)) ? \
                                                                    (i2c_SPI_CTRL_LATE_MISO_SAMPLE) : (0u))

#define i2c_GET_SPI_CTRL_SUB_MODE(mode)        (((uint32) (((uint32) (mode)) << \
                                                                        i2c_SPI_CTRL_MODE_POS)) & \
                                                                        i2c_SPI_CTRL_MODE_MASK)

#define i2c_GET_SPI_CTRL_SLAVE_SELECT(select)  (((uint32) ((uint32) (select) << \
                                                                      i2c_SPI_CTRL_SLAVE_SELECT_POS)) & \
                                                                      i2c_SPI_CTRL_SLAVE_SELECT_MASK)

#define i2c_GET_SPI_CTRL_MASTER_MODE(mode)     ((0u != (mode)) ? \
                                                                (i2c_SPI_CTRL_MASTER) : (0u))

/* i2c_UART_CTRL */
#define i2c_GET_UART_CTRL_MODE(mode)           (((uint32) ((uint32) (mode) << \
                                                                            i2c_UART_CTRL_MODE_POS)) & \
                                                                                i2c_UART_CTRL_MODE_MASK)

/* i2c_UART_RX_CTRL */
#define i2c_GET_UART_RX_CTRL_MODE(stopBits)    (((uint32) (stopBits) - 1u) & \
                                                                        i2c_UART_RX_CTRL_STOP_BITS_MASK)

#define i2c_GET_UART_RX_CTRL_PARITY(parity)    ((0u != (parity)) ? \
                                                                    (i2c_UART_RX_CTRL_PARITY) : (0u))

#define i2c_GET_UART_RX_CTRL_POLARITY(polarity)    ((0u != (polarity)) ? \
                                                                    (i2c_UART_RX_CTRL_POLARITY) : (0u))

#define i2c_GET_UART_RX_CTRL_DROP_ON_PARITY_ERR(dropErr) ((0u != (dropErr)) ? \
                                                        (i2c_UART_RX_CTRL_DROP_ON_PARITY_ERR) : (0u))

#define i2c_GET_UART_RX_CTRL_DROP_ON_FRAME_ERR(dropErr) ((0u != (dropErr)) ? \
                                                        (i2c_UART_RX_CTRL_DROP_ON_FRAME_ERR) : (0u))

#define i2c_GET_UART_RX_CTRL_MP_MODE(mpMode)   ((0u != (mpMode)) ? \
                                                        (i2c_UART_RX_CTRL_MP_MODE) : (0u))

/* i2c_UART_TX_CTRL */
#define i2c_GET_UART_TX_CTRL_MODE(stopBits)    (((uint32) (stopBits) - 1u) & \
                                                                i2c_UART_RX_CTRL_STOP_BITS_MASK)

#define i2c_GET_UART_TX_CTRL_PARITY(parity)    ((0u != (parity)) ? \
                                                               (i2c_UART_TX_CTRL_PARITY) : (0u))

#define i2c_GET_UART_TX_CTRL_RETRY_NACK(nack)  ((0u != (nack)) ? \
                                                               (i2c_UART_TX_CTRL_RETRY_ON_NACK) : (0u))

/* i2c_RX_CTRL */
#define i2c_GET_RX_CTRL_DATA_WIDTH(dataWidth)  (((uint32) (dataWidth) - 1u) & \
                                                                i2c_RX_CTRL_DATA_WIDTH_MASK)

#define i2c_GET_RX_CTRL_BIT_ORDER(bitOrder)    ((0u != (bitOrder)) ? \
                                                                (i2c_RX_CTRL_MSB_FIRST) : (0u))

#define i2c_GET_RX_CTRL_MEDIAN(filterEn)       ((0u != (filterEn)) ? \
                                                                (i2c_RX_CTRL_MEDIAN) : (0u))

/* i2c_RX_MATCH */
#define i2c_GET_RX_MATCH_ADDR(addr)    ((uint32) (addr) & i2c_RX_MATCH_ADDR_MASK)
#define i2c_GET_RX_MATCH_MASK(mask)    (((uint32) ((uint32) (mask) << \
                                                            i2c_RX_MATCH_MASK_POS)) & \
                                                            i2c_RX_MATCH_MASK_MASK)

/* i2c_RX_FIFO_CTRL */
#define i2c_GET_RX_FIFO_CTRL_TRIGGER_LEVEL(level)  ((uint32) (level) & \
                                                                    i2c_RX_FIFO_CTRL_TRIGGER_LEVEL_MASK)

/* i2c_TX_CTRL */
#define i2c_GET_TX_CTRL_DATA_WIDTH(dataWidth)  (((uint32) (dataWidth) - 1u) & \
                                                                i2c_RX_CTRL_DATA_WIDTH_MASK)

#define i2c_GET_TX_CTRL_BIT_ORDER(bitOrder)    ((0u != (bitOrder)) ? \
                                                                (i2c_TX_CTRL_MSB_FIRST) : (0u))

/* i2c_TX_FIFO_CTRL */
#define i2c_GET_TX_FIFO_CTRL_TRIGGER_LEVEL(level)  ((uint32) (level) & \
                                                                    i2c_TX_FIFO_CTRL_TRIGGER_LEVEL_MASK)

/* Clears register: config and interrupt mask */
#define i2c_CLEAR_REG          ((uint32) (0u))
#define i2c_NO_INTR_SOURCES    ((uint32) (0u))
#define i2c_DUMMY_PARAM        ((uint32) (0u))
#define i2c_SUBMODE_SPI_SLAVE  ((uint32) (0u))

/* Return in case I2C read error */
#define i2c_I2C_INVALID_BYTE   ((uint32) 0xFFFFFFFFu)
#define i2c_CHECK_VALID_BYTE   ((uint32) 0x80000000u)

#endif /* (CY_SCB_i2c_H) */


/* [] END OF FILE */
