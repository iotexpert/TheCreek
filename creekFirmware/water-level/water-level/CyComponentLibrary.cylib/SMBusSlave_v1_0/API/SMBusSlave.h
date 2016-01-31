/*******************************************************************************
* File Name: `$INSTANCE_NAME`.h
* Version `$CY_MAJOR_VERSION`.`$CY_MINOR_VERSION`
*
* Description:
*  This file provides constants and parameter values and API definition for the
*  SM/PM Bus Component.
*
********************************************************************************
* Copyright 2012, Cypress Semiconductor Corporation.  All rights reserved.
* You may use this file only in accordance with the license, terms, conditions,
* disclaimers, and limitations in the end user license agreement accompanying
* the software package with which this file was provided.
********************************************************************************/

#if !defined(CY_SMBusSlave_`$INSTANCE_NAME`_H)
#define CY_SMBusSlave_`$INSTANCE_NAME`_H

#include "`$INSTANCE_NAME`_I2C.h"
#include "cydevice_trm.h"


/***************************************
*   Conditional Compilation Parameters
***************************************/

#define `$INSTANCE_NAME`_MODE                       (`$Mode`)
#define `$INSTANCE_NAME`_RECEIVE_BYTE_PROTOCOL      (`$EnableRecieveByteProtocol`)
#define `$INSTANCE_NAME`_SMB_ALERT_PIN_ENABLED      (`$EnableSmbAlertPin`)
#define `$INSTANCE_NAME`_SMB_ALERT_MODE_INIT        (`$SmbAlertMode`)


/***************************************
*        Structure Definitions
***************************************/

typedef struct
{
    uint8 read;          /* r/w flag - 1=read 0=write */
    uint8 commandCode;   /* SMBus/PMBus command code */
    uint8 page;          /* PMBus page (if applicable) */
    uint8 length;        /* bytes transferred */
    uint8 payload[65u];  /* payload for the transaction */
} `$INSTANCE_NAME`_TRANSACTION_STRUCT;

/**********************************
*      Generated Code
**********************************/
/* Define each command based on the inputs from the customizer. */
`$RegsStructElements`


/***************************************
*        Function Prototypes
***************************************/
void    `$INSTANCE_NAME`_Init(void)                      `=ReentrantKeil($INSTANCE_NAME . "_Init")`;
void    `$INSTANCE_NAME`_Enable(void)                    `=ReentrantKeil($INSTANCE_NAME . "_Enable")`;
void    `$INSTANCE_NAME`_Start(void)                     `=ReentrantKeil($INSTANCE_NAME . "_Start")`;
void    `$INSTANCE_NAME`_Stop(void)                      `=ReentrantKeil($INSTANCE_NAME . "_Stop")`;
`$INSTANCE_NAME`_TRANSACTION_STRUCT *
        `$INSTANCE_NAME`_GetNextTransaction(void)        `=ReentrantKeil($INSTANCE_NAME . "_GetNextTransaction")`;
uint8   `$INSTANCE_NAME`_GetTransactionCount(void)       `=ReentrantKeil($INSTANCE_NAME . "_GetTransactionCount")`;
void    `$INSTANCE_NAME`_CompleteTransaction(void)       `=ReentrantKeil($INSTANCE_NAME . "_CompleteTransaction")`;
#if (0u != `$INSTANCE_NAME`_SMB_ALERT_PIN_ENABLED)
    void    `$INSTANCE_NAME`_SetAlertResponseAddress(uint8 address)
                                                         `=ReentrantKeil($INSTANCE_NAME . "_SetAlertResponseAddress")`;
    void    `$INSTANCE_NAME`_SetSmbAlert(uint8 assert)   `=ReentrantKeil($INSTANCE_NAME . "_SetSmbAlert")`;
    void    `$INSTANCE_NAME`_SetSmbAlertMode(uint8 alertMode)
                                                          `=ReentrantKeil($INSTANCE_NAME . "_SetSmbAlertMode")`;
    void    `$INSTANCE_NAME`_HandleSmbAlertResponse(void) `=ReentrantKeil($INSTANCE_NAME . "_HandleSmbAlertResponse")`;
#endif /* (0u != `$INSTANCE_NAME`_SMB_ALERT_PIN_ENABLED) */

uint8   `$INSTANCE_NAME`_GetReceiveByteResponse(void)    `=ReentrantKeil($INSTANCE_NAME . "_GetReceiveByteResponse")`;
void    `$INSTANCE_NAME`_HandleBusError(uint8 errorCode) `=ReentrantKeil($INSTANCE_NAME . "_HandleBusError")`;
uint8   `$INSTANCE_NAME`_StoreUserAll(char * flashRegs)  `=ReentrantKeil($INSTANCE_NAME . "_StoreUserAll")`;
uint8   `$INSTANCE_NAME`_RestoreUserAll(char * flashRegs)
                                                         `=ReentrantKeil($INSTANCE_NAME . "_RestoreUserAll")`;
uint8   `$INSTANCE_NAME`_RestoreDefaultAll(void)         `=ReentrantKeil($INSTANCE_NAME . "_RestoreDefaultAll")`;
uint8   `$INSTANCE_NAME`_StoreComponentAll(void)         `=ReentrantKeil($INSTANCE_NAME . "_StoreComponentAll")`;
uint8   `$INSTANCE_NAME`_RestoreComponentAll(void)       `=ReentrantKeil($INSTANCE_NAME . "RestoreComponentAll")`;
float   `$INSTANCE_NAME`_Lin11ToFloat(uint16 linear11)   CYREENTRANT;
uint16  `$INSTANCE_NAME`_FloatToLin11(float floatvar)    CYREENTRANT;
float   `$INSTANCE_NAME`_Lin16ToFloat(uint16 linear16, int8 inExponent)
                                                         CYREENTRANT;
uint16  `$INSTANCE_NAME`_FloatToLin16(float floatvar, int8 outExponent)
                                                         CYREENTRANT;

/* Macros' */
#define `$INSTANCE_NAME`_EnableInt()                    `$INSTANCE_NAME`_I2C_EnableInt()
#define `$INSTANCE_NAME`_DisableInt()                   `$INSTANCE_NAME`_I2C_DisableInt()
#define `$INSTANCE_NAME`_SetAddress(address)            `$INSTANCE_NAME`_I2C_SlaveSetAddress(address)

#define `$INSTANCE_NAME`_EnableTimeoutInt()             `$INSTANCE_NAME`_I2C_TimeoutEnableInt()
#define `$INSTANCE_NAME`_DisableTimeoutInt()            `$INSTANCE_NAME`_I2C_TimeoutDisableInt()

/* Communication bootloader SM/PM Bus Slave APIs */
#if defined(CYDEV_BOOTLOADER_IO_COMP) && ((CYDEV_BOOTLOADER_IO_COMP == CyBtldr_`$INSTANCE_NAME`) || \
                                          (CYDEV_BOOTLOADER_IO_COMP == CyBtldr_Custom_Interface))
    /* Physical layer functions. */
        void     `$INSTANCE_NAME`_CyBtldrCommStart(void) CYSMALL `=ReentrantKeil($INSTANCE_NAME . "_CyBtldrCommStart")`;
        void     `$INSTANCE_NAME`_CyBtldrCommStop(void) CYSMALL `=ReentrantKeil($INSTANCE_NAME . "_CyBtldrCommStop")`;
        void     `$INSTANCE_NAME`_CyBtldrCommReset(void) CYSMALL `=ReentrantKeil($INSTANCE_NAME . "_CyBtldrCommReset")`;
        cystatus `$INSTANCE_NAME`_CyBtldrCommWrite(uint8 * Data, uint16 size, uint16 * count, uint8 timeOut) CYSMALL
                 `=ReentrantKeil($INSTANCE_NAME . "_CyBtldrCommWrite")`;
        cystatus `$INSTANCE_NAME`_CyBtldrCommRead(uint8 * Data, uint16 size, uint16 * count, uint8 timeOut) CYSMALL
                 `=ReentrantKeil($INSTANCE_NAME . "_CyBtldrCommRead")`;

#endif /* if defined(CYDEV_BOOTLOADER_IO_COMP) && \
            ((CYDEV_BOOTLOADER_IO_COMP == CyBtldr_`$INSTANCE_NAME`) || \
                (CYDEV_BOOTLOADER_IO_COMP == CyBtldr_Custom_Interface)) */

/* Interrupt handlers */
CY_ISR_PROTO(`$INSTANCE_NAME`_ISR);
CY_ISR_PROTO(`$INSTANCE_NAME`_TIMEOUT_ISR);


/***************************************
*    Initial Parameter Constants
***************************************/

/* Defines the number of all enabled commands */
#define `$INSTANCE_NAME`_NUM_COMMANDS               (`$NumCommands`)

/* Defines the number of pages used for each command */
#define `$INSTANCE_NAME`_MAX_PAGES                  (`$PagedCommandsSize`)


/***************************************
*           Global Variables
***************************************/
extern `$INSTANCE_NAME`_TRANSACTION_STRUCT `$INSTANCE_NAME`_transactionData[];

extern volatile uint8 `$INSTANCE_NAME`_buffer[];             /* Intermediate I2C buffer */
extern volatile uint8 `$INSTANCE_NAME`_bufferIndex;          /* Index used to navigate throught the buffer */
extern volatile uint8 `$INSTANCE_NAME`_bufferSize;           /* Size of data for last received command */

/* Defines number of active manual transaction records in Transaction Queue */
extern volatile uint8 `$INSTANCE_NAME`_trActiveCount;

/* This is "Operating Memory Register Store" (RAM) */
extern `$INSTANCE_NAME`_REGS `$INSTANCE_NAME`_regs;

/* This is Default "Operating Memory Register Store" (Flash) */
extern `$INSTANCE_NAME`_REGS CYCODE `$INSTANCE_NAME`_regsDefault;


/***************************************
*              Registers
***************************************/

#if(CY_PSOC5A)
    #define `$INSTANCE_NAME`_SMB_ALERT_CONTROL_REG   (*(reg8 *) \
                                    `$INSTANCE_NAME`_AlertAsyncCtrlReg_Async_ctrl_reg__CONTROL_REG)
#else
    #define `$INSTANCE_NAME`_SMB_ALERT_CONTROL_REG   (*(reg8 *) \
                                    `$INSTANCE_NAME`_AlertSyncCtrlReg_Sync_ctrl_reg__CONTROL_REG)
#endif /* CY_PSOC5A */

/***************************************
*           API Constants
***************************************/

#define `$INSTANCE_NAME`_SIGNATURE                  (0x000055AAu)

/* Size of Operation Memory */
#define `$INSTANCE_NAME`_REGS_SIZE                  (sizeof(`$INSTANCE_NAME`_REGS))

/* Number and priority of the SM/PM Bus interrupt.
* this #defines are derived from I2C.
*/
#define `$INSTANCE_NAME`_ISR_NUMBER                 (`$INSTANCE_NAME`_I2C_ISR_NUMBER)
#define `$INSTANCE_NAME`_ISR_PRIORITY               (`$INSTANCE_NAME`_I2C_ISR_PRIORITY)

/* Default return value for `$INSTANCE_NAME`_GetReceiveByteResponse() */
#define `$INSTANCE_NAME`_RET_UNDEFINED              (0xFFu)

/* Defines the size of intermediate buffer */
#define `$INSTANCE_NAME`_MAX_BUFFER_SIZE            (64u)

/* SMBALERT pin related constants */
#if (0u != `$INSTANCE_NAME`_SMB_ALERT_PIN_ENABLED)

    #define `$INSTANCE_NAME`_SMBALERT_ASSERT        (0x01u)
    #define `$INSTANCE_NAME`_SMBALERT_DEASSERT      (0x00u)

    #define `$INSTANCE_NAME`_STATE_ASSERTED         (0x01u)
    #define `$INSTANCE_NAME`_STATE_DEASSERTED       (0x00u)

    #define `$INSTANCE_NAME`_SMBALERT_PIN_MASK      (0x01u)

    /* Constants for `$INSTANCE_NAME`_SetSmbAlertMode() */
    #define `$INSTANCE_NAME`_AUTO_MODE              (0x01u)
    #define `$INSTANCE_NAME`_FIRMWARE_MODE          (0x02u)
    #define `$INSTANCE_NAME`_DO_NOTHING             (0x00u)

    /* Reserved Alert Response Address */
    #define `$INSTANCE_NAME`_ALERT_REPONSE_ADDR     (0x0Cu)

#endif /* (0u != `$INSTANCE_NAME`_SMB_ALERT_PIN_ENABLED) */

#define `$INSTANCE_NAME`_RECEIVE_BYTE_ENABLED   (0x01u)

/* Transaction Queue size */
#define `$INSTANCE_NAME`_TRANS_QUEUE_SIZE       (1u)

#define `$INSTANCE_NAME`_NEG_EXP_MIN            (-16)

/* Defines related to CRC calculation */
#define `$INSTANCE_NAME`_CRC_SEED               (0xFFFFu)
#define `$INSTANCE_NAME`_CRC_BYTE_SHIFT         (0x8u)
#define `$INSTANCE_NAME`_CRC_BYTE_MASK          (0xFFu)

/* Error codes for `$INSTANCE_NAME`_HandleBusError() */
#define `$INSTANCE_NAME`_ERR_READ_FLAG          (0x01u)
#define `$INSTANCE_NAME`_ERR_RD_TO_MANY_BYTES   (0x02u)
#define `$INSTANCE_NAME`_ERR_WR_TO_MANY_BYTES   (0x03u)
#define `$INSTANCE_NAME`_ERR_UNSUPORTED_CMD     (0x04u)
#define `$INSTANCE_NAME`_ERR_INVALID_DATA       (0x05u)
#define `$INSTANCE_NAME`_ERR_TIMEOUT            (0x06u)
#define `$INSTANCE_NAME`_ERR_WR_TO_FEW_BYTES    (0x07u)

#define `$INSTANCE_NAME`_PHASE_CMD                  (0x01u)
#define `$INSTANCE_NAME`_PHASE_DATA             (0x00u)

#if defined(CYDEV_BOOTLOADER_IO_COMP) && ((CYDEV_BOOTLOADER_IO_COMP == CyBtldr_`$INSTANCE_NAME`) || \
                                          (CYDEV_BOOTLOADER_IO_COMP == CyBtldr_Custom_Interface))
                                          
    #define `$INSTANCE_NAME`_BTLDR_WR_CMPT              (0x10u)    /* Write transfer complete */
    #define `$INSTANCE_NAME`_BTLDR_RD_CMPT              (0x01u)    /* Read transfer complete */
    
    #if defined(`$INSTANCE_NAME`_BOOTLOAD_READ)
        #define `$INSTANCE_NAME`_BOOTLOADER_READ_EN         (0x01u)
    #else    
        #define `$INSTANCE_NAME`_BOOTLOADER_READ_EN         (0x00u)
    #endif /* `$INSTANCE_NAME`_BOOTLOAD_READ */
    
#else
    #define `$INSTANCE_NAME`_BOOTLOADER_READ_EN         (0x00u)
#endif /* if defined(CYDEV_BOOTLOADER_IO_COMP) && \
            ((CYDEV_BOOTLOADER_IO_COMP == CyBtldr_`$INSTANCE_NAME`) || \
                (CYDEV_BOOTLOADER_IO_COMP == CyBtldr_Custom_Interface)) */

/*  */
#define `$INSTANCE_NAME`_FLASH_ROW_SIZE             (CYDEV_FLS_ROW_SIZE)
#define `$INSTANCE_NAME`_FLASH_END_ADDR             (CYDEV_FLS_SIZE - 1u)
#define `$INSTANCE_NAME`_FLASH_ROW_MASK             (0x0000FF00u)
#define `$INSTANCE_NAME`_FLASH_ARRYID_MASK          (0x00FF0000u)
#define `$INSTANCE_NAME`_BYTE_SHIFT                 (8u)
#define `$INSTANCE_NAME`_TWO_BYTES_SHIFT            (16u)


/***************************************
*              Macros
***************************************/
/* Converst Flash address to Flash row */
#define `$INSTANCE_NAME`_FL_ADDR_TO_ROW(addr)       ((uint8) (((addr & `$INSTANCE_NAME`_FLASH_ROW_MASK)) >> \
                                                        `$INSTANCE_NAME`_BYTE_SHIFT))
/* Converts Flash row to Flash Array ID */
#define `$INSTANCE_NAME`_FL_ADDR_TO_ARRAYID(addr)   ((uint8) (((addr & `$INSTANCE_NAME`_FLASH_ARRYID_MASK)) >> \
                                                        `$INSTANCE_NAME`_TWO_BYTES_SHIFT))

#define `$INSTANCE_NAME`_SIZE_TO_ROW(size)          ((uint8) ((size) >> `$INSTANCE_NAME`_BYTE_SHIFT) + 1u)

#define `$INSTANCE_NAME`_MAKE_FLASH_ADDR(arrayId, row)   ((((uint32) arrayId) <<  `$INSTANCE_NAME`_TWO_BYTES_SHIFT) | \
                                                        (((uint32) row) << `$INSTANCE_NAME`_BYTE_SHIFT))
                                                        

/***************************************
* Enumerated Types and Parameters
***************************************/
`#declare_enum ModeSelType`

#endif /* CY_SMBusSlave_`$INSTANCE_NAME`_H */

/* [] END OF FILE */
