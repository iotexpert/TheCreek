/*******************************************************************************
* File Name: ezi2c_EZI2C.h
* Version 3.10
*
* Description:
*  This file provides constants and parameter values for the SCB Component in
*  the EZI2C mode.
*
* Note:
*
********************************************************************************
* Copyright 2013-2015, Cypress Semiconductor Corporation.  All rights reserved.
* You may use this file only in accordance with the license, terms, conditions,
* disclaimers, and limitations in the end user license agreement accompanying
* the software package with which this file was provided.
*******************************************************************************/

#if !defined(CY_SCB_EZI2C_ezi2c_H)
#define CY_SCB_EZI2C_ezi2c_H

#include "ezi2c.h"


/***************************************
*   Initial Parameter Constants
****************************************/

#define ezi2c_EZI2C_CLOCK_STRETCHING         (0u)
#define ezi2c_EZI2C_MEDIAN_FILTER_ENABLE     (1u)
#define ezi2c_EZI2C_NUMBER_OF_ADDRESSES      (0u)
#define ezi2c_EZI2C_PRIMARY_SLAVE_ADDRESS    (8u)
#define ezi2c_EZI2C_SECONDARY_SLAVE_ADDRESS  (9u)
#define ezi2c_EZI2C_SUB_ADDRESS_SIZE         (0u)
#define ezi2c_EZI2C_WAKE_ENABLE              (0u)
#define ezi2c_EZI2C_DATA_RATE                (100u)
#define ezi2c_EZI2C_SLAVE_ADDRESS_MASK       (254u)
#define ezi2c_EZI2C_BYTE_MODE_ENABLE         (0u)


/***************************************
*  Conditional Compilation Parameters
****************************************/

#if(ezi2c_SCB_MODE_UNCONFIG_CONST_CFG)

    #define ezi2c_SUB_ADDRESS_SIZE16             (0u != ezi2c_subAddrSize)
    #define ezi2c_SECONDARY_ADDRESS_ENABLE       (0u != ezi2c_numberOfAddr)

    #define ezi2c_EZI2C_EC_AM_ENABLE         (0u != (ezi2c_CTRL_REG & \
                                                                ezi2c_CTRL_EC_AM_MODE))
    #define ezi2c_EZI2C_SCL_STRETCH_ENABLE   (0u != (ezi2c_GetSlaveInterruptMode() & \
                                                                ezi2c_INTR_SLAVE_I2C_ADDR_MATCH))
    #define ezi2c_EZI2C_SCL_STRETCH_DISABLE       (!ezi2c_EZI2C_SCL_STRETCH_ENABLE)

    #define ezi2c_SECONDARY_ADDRESS_ENABLE_CONST  (1u)
    #define ezi2c_SUB_ADDRESS_SIZE16_CONST        (1u)
    #define ezi2c_EZI2C_SCL_STRETCH_ENABLE_CONST  (1u)
    #define ezi2c_EZI2C_SCL_STRETCH_DISABLE_CONST (1u)
    #define ezi2c_EZI2C_WAKE_ENABLE_CONST         (1u)

    #if (ezi2c_CY_SCBIP_V0 || ezi2c_CY_SCBIP_V1)
        #define ezi2c_EZI2C_FIFO_SIZE    (ezi2c_FIFO_SIZE)
    #else
        #define ezi2c_EZI2C_FIFO_SIZE    (ezi2c_GET_FIFO_SIZE(ezi2c_CTRL_REG & \
                                                                                    ezi2c_CTRL_BYTE_MODE))
    #endif /* (ezi2c_CY_SCBIP_V0 || ezi2c_CY_SCBIP_V1) */

#else

    #define ezi2c_SUB_ADDRESS_SIZE16         (0u != ezi2c_EZI2C_SUB_ADDRESS_SIZE)
    #define ezi2c_SUB_ADDRESS_SIZE16_CONST   (ezi2c_SUB_ADDRESS_SIZE16)

    #define ezi2c_SECONDARY_ADDRESS_ENABLE        (0u != ezi2c_EZI2C_NUMBER_OF_ADDRESSES)
    #define ezi2c_SECONDARY_ADDRESS_ENABLE_CONST  (ezi2c_SECONDARY_ADDRESS_ENABLE)

    #define ezi2c_EZI2C_SCL_STRETCH_ENABLE        (0u != ezi2c_EZI2C_CLOCK_STRETCHING)
    #define ezi2c_EZI2C_SCL_STRETCH_DISABLE       (!ezi2c_EZI2C_SCL_STRETCH_ENABLE)
    #define ezi2c_EZI2C_SCL_STRETCH_ENABLE_CONST  (ezi2c_EZI2C_SCL_STRETCH_ENABLE)
    #define ezi2c_EZI2C_SCL_STRETCH_DISABLE_CONST (ezi2c_EZI2C_SCL_STRETCH_DISABLE)

    #define ezi2c_EZI2C_WAKE_ENABLE_CONST         (0u != ezi2c_EZI2C_WAKE_ENABLE)
    #define ezi2c_EZI2C_EC_AM_ENABLE              (0u != ezi2c_EZI2C_WAKE_ENABLE)

    #if (ezi2c_CY_SCBIP_V0 || ezi2c_CY_SCBIP_V1)
       #define ezi2c_EZI2C_FIFO_SIZE    (ezi2c_FIFO_SIZE)

    #else
        #define ezi2c_EZI2C_FIFO_SIZE \
                                            ezi2c_GET_FIFO_SIZE(ezi2c_EZI2C_BYTE_MODE_ENABLE)
    #endif /* (ezi2c_CY_SCBIP_V0 || ezi2c_CY_SCBIP_V1) */

#endif /* (ezi2c_SCB_MODE_UNCONFIG_CONST_CFG) */


/***************************************
*       Type Definitions
***************************************/

typedef struct
{
    uint32 enableClockStretch;
    uint32 enableMedianFilter;
    uint32 numberOfAddresses;
    uint32 primarySlaveAddr;
    uint32 secondarySlaveAddr;
    uint32 subAddrSize;
    uint32 enableWake;
    uint8  enableByteMode;
} ezi2c_EZI2C_INIT_STRUCT;


/***************************************
*        Function Prototypes
***************************************/

#if(ezi2c_SCB_MODE_UNCONFIG_CONST_CFG)
    void ezi2c_EzI2CInit(const ezi2c_EZI2C_INIT_STRUCT *config);
#endif /* (ezi2c_SCB_MODE_UNCONFIG_CONST_CFG) */

uint32 ezi2c_EzI2CGetActivity(void);

void   ezi2c_EzI2CSetAddress1(uint32 address);
uint32 ezi2c_EzI2CGetAddress1(void);
void   ezi2c_EzI2CSetBuffer1(uint32 bufSize, uint32 rwBoundary, volatile uint8 * buffer);
void   ezi2c_EzI2CSetReadBoundaryBuffer1(uint32 rwBoundary);

#if(ezi2c_SECONDARY_ADDRESS_ENABLE_CONST)
    void   ezi2c_EzI2CSetAddress2(uint32 address);
    uint32 ezi2c_EzI2CGetAddress2(void);
    void   ezi2c_EzI2CSetBuffer2(uint32 bufSize, uint32 rwBoundary, volatile uint8 * buffer);
    void   ezi2c_EzI2CSetReadBoundaryBuffer2(uint32 rwBoundary);
#endif /* (ezi2c_SECONDARY_ADDRESS_ENABLE_CONST) */

#if(ezi2c_EZI2C_SCL_STRETCH_ENABLE_CONST)
    CY_ISR_PROTO(ezi2c_EZI2C_STRETCH_ISR);
#endif /* (ezi2c_EZI2C_SCL_STRETCH_ENABLE_CONST) */

#if(ezi2c_EZI2C_SCL_STRETCH_DISABLE_CONST)
    CY_ISR_PROTO(ezi2c_EZI2C_NO_STRETCH_ISR);
#endif /* (ezi2c_EZI2C_SCL_STRETCH_DISABLE) */


/***************************************
*            API Constants
***************************************/

/* Configuration structure constants */
#define ezi2c_EZI2C_ONE_ADDRESS      (0u)
#define ezi2c_EZI2C_TWO_ADDRESSES    (1u)

#define ezi2c_EZI2C_PRIMARY_ADDRESS  (0u)
#define ezi2c_EZI2C_BOTH_ADDRESSES   (1u)

#define ezi2c_EZI2C_SUB_ADDR8_BITS   (0u)
#define ezi2c_EZI2C_SUB_ADDR16_BITS  (1u)

/* ezi2c_EzI2CGetActivity() return constants */
#define ezi2c_EZI2C_STATUS_SECOND_OFFSET (1u)
#define ezi2c_EZI2C_STATUS_READ1     ((uint32) (ezi2c_INTR_SLAVE_I2C_NACK))        /* Bit [1]   */
#define ezi2c_EZI2C_STATUS_WRITE1    ((uint32) (ezi2c_INTR_SLAVE_I2C_WRITE_STOP))  /* Bit [3]   */
#define ezi2c_EZI2C_STATUS_READ2     ((uint32) (ezi2c_INTR_SLAVE_I2C_NACK >> \
                                                           ezi2c_EZI2C_STATUS_SECOND_OFFSET)) /* Bit [0]   */
#define ezi2c_EZI2C_STATUS_WRITE2    ((uint32) (ezi2c_INTR_SLAVE_I2C_WRITE_STOP >> \
                                                           ezi2c_EZI2C_STATUS_SECOND_OFFSET)) /* Bit [2]   */
#define ezi2c_EZI2C_STATUS_ERR       ((uint32) (0x10u))                                       /* Bit [4]   */
#define ezi2c_EZI2C_STATUS_BUSY      ((uint32) (0x20u))                                       /* Bit [5]   */
#define ezi2c_EZI2C_CLEAR_STATUS     ((uint32) (0x1Fu))                                       /* Bits [0-4]*/
#define ezi2c_EZI2C_CMPLT_INTR_MASK  ((uint32) (ezi2c_INTR_SLAVE_I2C_NACK | \
                                                           ezi2c_INTR_SLAVE_I2C_WRITE_STOP))


/***************************************
*     Vars with External Linkage
***************************************/

#if(ezi2c_SCB_MODE_UNCONFIG_CONST_CFG)
    extern const ezi2c_EZI2C_INIT_STRUCT ezi2c_configEzI2C;
#endif /* (ezi2c_SCB_MODE_UNCONFIG_CONST_CFG) */


/***************************************
*           FSM states
***************************************/

/* Returns to master when there is no data to transmit */
#define ezi2c_EZI2C_OVFL_RETURN  (0xFFu)

/* States of EZI2C Slave FSM */
#define ezi2c_EZI2C_FSM_OFFSET_HI8 ((uint8) (0x02u)) /* Idle state for 1 addr: waits for high byte offset */
#define ezi2c_EZI2C_FSM_OFFSET_LO8 ((uint8) (0x00u)) /* Idle state for 2 addr: waits for low byte offset  */
#define ezi2c_EZI2C_FSM_BYTE_WRITE ((uint8) (0x01u)) /* Data byte write state: byte by byte mode          */
#define ezi2c_EZI2C_FSM_WAIT_STOP  ((uint8) (0x03u)) /* Discards written bytes as they do not match write
                                                                   criteria */
#define ezi2c_EZI2C_FSM_WRITE_MASK ((uint8) (0x01u)) /* Incorporates write states: EZI2C_FSM_BYTE_WRITE and
                                                                   EZI2C_FSM_WAIT_STOP  */

#define ezi2c_EZI2C_FSM_IDLE     ((ezi2c_SUB_ADDRESS_SIZE16) ?      \
                                                (ezi2c_EZI2C_FSM_OFFSET_HI8) : \
                                                (ezi2c_EZI2C_FSM_OFFSET_LO8))

#define ezi2c_EZI2C_CONTINUE_TRANSFER    (1u)
#define ezi2c_EZI2C_COMPLETE_TRANSFER    (0u)

#define ezi2c_EZI2C_NACK_RECEIVED_ADDRESS  (0u)
#define ezi2c_EZI2C_ACK_RECEIVED_ADDRESS   (1u)

#define ezi2c_EZI2C_ACTIVE_ADDRESS1  (0u)
#define ezi2c_EZI2C_ACTIVE_ADDRESS2  (1u)


/***************************************
*       Macro Definitions
***************************************/

/* Access to global variables */
#if(ezi2c_SECONDARY_ADDRESS_ENABLE_CONST)

    #define ezi2c_EZI2C_UPDATE_LOC_STATUS(activeAddress, locStatus) \
            do{ \
                (locStatus) >>= (activeAddress); \
            }while(0)

    #define ezi2c_EZI2C_GET_INDEX(activeAddress) \
            ((ezi2c_EZI2C_ACTIVE_ADDRESS1 == (activeAddress)) ? \
                ((uint32) ezi2c_indexBuf1) :                    \
                ((uint32) ezi2c_indexBuf2))

    #define ezi2c_EZI2C_GET_OFFSET(activeAddress) \
            ((ezi2c_EZI2C_ACTIVE_ADDRESS1 == (activeAddress)) ? \
                ((uint32) ezi2c_offsetBuf1) :                   \
                ((uint32) ezi2c_offsetBuf2))

    #define ezi2c_EZI2C_SET_INDEX(activeAddress, locIndex) \
            do{ \
                if(ezi2c_EZI2C_ACTIVE_ADDRESS1 == (activeAddress)) \
                {    \
                    ezi2c_indexBuf1 = (uint16) (locIndex); \
                }    \
                else \
                {    \
                    ezi2c_indexBuf2 = (uint16) (locIndex); \
                }    \
            }while(0)

    #define ezi2c_EZI2C_SET_OFFSET(activeAddress, locOffset) \
            do{ \
                if(ezi2c_EZI2C_ACTIVE_ADDRESS1 == (activeAddress)) \
                {    \
                    ezi2c_offsetBuf1 = (uint8) (locOffset); \
                }    \
                else \
                {    \
                    ezi2c_offsetBuf2 = (uint8) (locOffset); \
                }    \
            }while(0)
#else
    #define ezi2c_EZI2C_UPDATE_LOC_STATUS(activeAddress, locStatus)  do{ /* Empty*/ }while(0)

    #define ezi2c_EZI2C_GET_INDEX(activeAddress)  ((uint32) (ezi2c_indexBuf1))

    #define ezi2c_EZI2C_GET_OFFSET(activeAddress) ((uint32) (ezi2c_offsetBuf1))

    #define ezi2c_EZI2C_SET_INDEX(activeAddress, locIndex) \
            do{ \
                ezi2c_indexBuf1 = (uint16) (locIndex); \
            }while(0)

    #define ezi2c_EZI2C_SET_OFFSET(activeAddress, locOffset) \
            do{ \
                ezi2c_offsetBuf1 = (uint8) (locOffset); \
            }while(0)

#endif  /* (ezi2c_SUB_ADDRESS_SIZE16_CONST) */


/* This macro only applicable for EZI2C slave without clock stretching.
* It should not be used for other pusposes. */
#define ezi2c_EZI2C_TX_FIFO_CTRL_SET   (ezi2c_EZI2C_TX_FIFO_CTRL | \
                                                   ezi2c_TX_FIFO_CTRL_CLEAR)
#define ezi2c_EZI2C_TX_FIFO_CTRL_CLEAR (ezi2c_EZI2C_TX_FIFO_CTRL)
#define ezi2c_FAST_CLEAR_TX_FIFO \
                            do{             \
                                ezi2c_TX_FIFO_CTRL_REG = ezi2c_EZI2C_TX_FIFO_CTRL_SET;   \
                                ezi2c_TX_FIFO_CTRL_REG = ezi2c_EZI2C_TX_FIFO_CTRL_CLEAR; \
                            }while(0)


/***************************************
*      Common Register Settings
***************************************/

#define ezi2c_CTRL_EZI2C     (ezi2c_CTRL_MODE_I2C)

#define ezi2c_EZI2C_CTRL     (ezi2c_I2C_CTRL_S_GENERAL_IGNORE | \
                                         ezi2c_I2C_CTRL_SLAVE_MODE)

#define ezi2c_EZI2C_CTRL_AUTO    (ezi2c_I2C_CTRL_S_READY_ADDR_ACK      | \
                                             ezi2c_I2C_CTRL_S_READY_DATA_ACK      | \
                                             ezi2c_I2C_CTRL_S_NOT_READY_ADDR_NACK | \
                                             ezi2c_I2C_CTRL_S_NOT_READY_DATA_NACK)

#define ezi2c_EZI2C_RX_CTRL  ((ezi2c_FIFO_SIZE - 1u)   | \
                                          ezi2c_RX_CTRL_MSB_FIRST | \
                                          ezi2c_RX_CTRL_ENABLED)

#define ezi2c_EZI2C_TX_FIFO_CTRL (2u)
#define ezi2c_TX_LOAD_SIZE       (2u)

#define ezi2c_EZI2C_TX_CTRL  ((ezi2c_FIFO_SIZE - 1u)   | \
                                          ezi2c_TX_CTRL_MSB_FIRST | \
                                          ezi2c_TX_CTRL_ENABLED)

#define ezi2c_EZI2C_INTR_SLAVE_MASK  (ezi2c_INTR_SLAVE_I2C_BUS_ERROR | \
                                                 ezi2c_INTR_SLAVE_I2C_ARB_LOST  | \
                                                 ezi2c_INTR_SLAVE_I2C_STOP)

#define ezi2c_INTR_SLAVE_COMPLETE    (ezi2c_INTR_SLAVE_I2C_STOP | \
                                                 ezi2c_INTR_SLAVE_I2C_NACK | \
                                                 ezi2c_INTR_SLAVE_I2C_WRITE_STOP)


/***************************************
*    Initialization Register Settings
***************************************/

#if(ezi2c_SCB_MODE_EZI2C_CONST_CFG)

    #define ezi2c_EZI2C_DEFAULT_CTRL \
                                (ezi2c_GET_CTRL_BYTE_MODE  (ezi2c_EZI2C_BYTE_MODE_ENABLE)    | \
                                 ezi2c_GET_CTRL_ADDR_ACCEPT(ezi2c_EZI2C_NUMBER_OF_ADDRESSES) | \
                                 ezi2c_GET_CTRL_EC_AM_MODE (ezi2c_EZI2C_WAKE_ENABLE))

    #if(ezi2c_EZI2C_SCL_STRETCH_ENABLE_CONST)
        #define ezi2c_EZI2C_DEFAULT_I2C_CTRL (ezi2c_EZI2C_CTRL)
    #else
        #define ezi2c_EZI2C_DEFAULT_I2C_CTRL (ezi2c_EZI2C_CTRL_AUTO | ezi2c_EZI2C_CTRL)
    #endif /* (ezi2c_EZI2C_SCL_STRETCH_ENABLE_CONST) */


    #define ezi2c_EZI2C_DEFAULT_RX_MATCH \
                                (ezi2c_GET_I2C_8BIT_ADDRESS(ezi2c_EZI2C_PRIMARY_SLAVE_ADDRESS) | \
                                 ezi2c_GET_RX_MATCH_MASK   (ezi2c_EZI2C_SLAVE_ADDRESS_MASK))

    #define ezi2c_EZI2C_DEFAULT_RX_CTRL  (ezi2c_EZI2C_RX_CTRL)
    #define ezi2c_EZI2C_DEFAULT_TX_CTRL  (ezi2c_EZI2C_TX_CTRL)

    #define ezi2c_EZI2C_DEFAULT_RX_FIFO_CTRL (0u)
    #if(ezi2c_EZI2C_SCL_STRETCH_ENABLE_CONST)
        #define ezi2c_EZI2C_DEFAULT_TX_FIFO_CTRL (0u)
    #else
        #define ezi2c_EZI2C_DEFAULT_TX_FIFO_CTRL (2u)
    #endif /* (ezi2c_EZI2C_SCL_STRETCH_ENABLE_CONST) */

    /* Interrupt sources */
    #define ezi2c_EZI2C_DEFAULT_INTR_I2C_EC_MASK (ezi2c_NO_INTR_SOURCES)
    #define ezi2c_EZI2C_DEFAULT_INTR_SPI_EC_MASK (ezi2c_NO_INTR_SOURCES)
    #define ezi2c_EZI2C_DEFAULT_INTR_MASTER_MASK (ezi2c_NO_INTR_SOURCES)
    #define ezi2c_EZI2C_DEFAULT_INTR_TX_MASK     (ezi2c_NO_INTR_SOURCES)

    #if(ezi2c_EZI2C_SCL_STRETCH_ENABLE_CONST)
        #define ezi2c_EZI2C_DEFAULT_INTR_RX_MASK     (ezi2c_NO_INTR_SOURCES)
        #define ezi2c_EZI2C_DEFAULT_INTR_SLAVE_MASK  (ezi2c_INTR_SLAVE_I2C_ADDR_MATCH | \
                                                                 ezi2c_EZI2C_INTR_SLAVE_MASK)
    #else
        #define ezi2c_EZI2C_DEFAULT_INTR_RX_MASK     (ezi2c_INTR_RX_NOT_EMPTY)
        #define ezi2c_EZI2C_DEFAULT_INTR_SLAVE_MASK  (ezi2c_INTR_SLAVE_I2C_START | \
                                                                 ezi2c_EZI2C_INTR_SLAVE_MASK)
    #endif /* (ezi2c_EZI2C_SCL_STRETCH_ENABLE_CONST) */

#endif /* (ezi2c_SCB_MODE_EZI2C_CONST_CFG) */

#endif /* (CY_SCB_EZI2C_ezi2c_H) */


/* [] END OF FILE */
