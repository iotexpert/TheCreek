/*******************************************************************************
* File Name: `$INSTANCE_NAME`.h
* Version `$CY_MAJOR_VERSION`.`$CY_MINOR_VERSION`
*
* Description:
*  This file contains implementation of LIN.
*
********************************************************************************
* Copyright 2011-2012, Cypress Semiconductor Corporation.  All rights reserved.
* You may use this file only in accordance with the license, terms, conditions,
* disclaimers, and limitations in the end user license agreement accompanying
* the software package with which this file was provided.
*******************************************************************************/

#if !defined(CY_LIN_`$INSTANCE_NAME`_H)
#define CY_LIN_`$INSTANCE_NAME`_H

#include "cyfitter.h"
#include "CyLib.h"
#include "cytypes.h"

#include "`$INSTANCE_NAME`_UART.h"
#include "`$INSTANCE_NAME`_IntClk.h"


/* Check to see if required defines such as CY_PSOC5A are available */
/* They are defined starting with cy_boot v3.0 */
#if !defined (CY_PSOC5A)
    #error Component `$CY_COMPONENT_NAME` requires cy_boot v3.0 or later
#endif /* (CY_ PSOC5A) */


/***************************************
* Conditional Compilation Parameters
***************************************/

/* General */
#define `$INSTANCE_NAME`_RESPONSE_ERROR_SIGNAL  (`$AutoErrorSignal`u)
#define `$INSTANCE_NAME`_SAE_J2602              (`$SaeJ2602`u)
#define `$INSTANCE_NAME`_LIN_2_0                (`$Lin20`u)

#define `$INSTANCE_NAME`_INACTIVITY_ENABLED     (`$BusInactivityEnabled`u)
#define `$INSTANCE_NAME`_INACTIVITY_THRESHOLD   (`$BusInactivityThreshold`u)
#define `$INSTANCE_NAME`_BREAK_THRESHOLD        (`$BreakThreshold`)

/* Baud Rate */
#define `$INSTANCE_NAME`_AUTO_BAUD_RATE_SYNC    (`$AutoBaudRateSync`u)
#define `$INSTANCE_NAME`_BAUD_RATE              (`$BaudRate`)
#define `$INSTANCE_NAME`_INTERNAL_CLOCK         (`$InternalClock`u)

/* Transport Layer */
#define `$INSTANCE_NAME`_TL_ENABLED             (`$TlEnabled`u)
#define `$INSTANCE_NAME`_TL_INITIAL_NAD         (`$TlInitialNad`u)
#define `$INSTANCE_NAME`_TL_API_FORMAT          (`$TlApiFormat`u)
#define `$INSTANCE_NAME`_TL_BUF_LEN_MAX         (`$TlBufLenMax`u)
#define `$INSTANCE_NAME`_TL_TX_QUEUE_LEN        (`$TlBufLenTx`u)
#define `$INSTANCE_NAME`_TL_RX_QUEUE_LEN        (`$TlBufLenRx`u)

/* Config. Services */
#define `$INSTANCE_NAME`_CS_ENABLED             (`$CsEnabled`u)
#define `$INSTANCE_NAME`_CS_SUPPLIER_ID         (`$CsSupplierId`u)
#define `$INSTANCE_NAME`_CS_FUNCTION_ID         (`$CsFunctionId`u)
#define `$INSTANCE_NAME`_CS_VARIANT             (`$CsVariant`u )
#define `$INSTANCE_NAME`_CS_SAVE_ADDR           (`$CsSaveAddr`u)
#define `$INSTANCE_NAME`_CS_SEL_SERVICES01      (`$Services01`u)

/* Interface handle constant  for `$INSTANCE_NAME` */
#define `$INSTANCE_NAME`_IFC_HANDLE             (0x00u)

/* General frames and signals information */
`$generalInfo`

#define `$INSTANCE_NAME`_NUM_SERVICE_FRAMES     (0x02u)     /* MRF and SRF */

#define `$INSTANCE_NAME`_SIG_FRAME_FLAGS_SIZE   ((`$INSTANCE_NAME`_NUM_FRAMES         + \
                                                  `$INSTANCE_NAME`_NUM_SIGNALS        + \
                                                  `$INSTANCE_NAME`_NUM_UNIQUE_SIGNALS + \
                                                  `$INSTANCE_NAME`_NUM_SERVICE_FRAMES) / 8u + 1u)

#define `$INSTANCE_NAME`_ET_FRAMES_FLAGS_SIZE   (`$INSTANCE_NAME`_NUM_ET_FRAMES / 8u + 1u)

#define `$INSTANCE_NAME`_INACTIVITY_THRESHOLD_IN_100_MS    (`$INSTANCE_NAME`_INACTIVITY_THRESHOLD / 100)

#define `$INSTANCE_NAME`_SWAP_U8_TO_U16(x,y) ((((l_u16)(x)) << 8u) | ((l_u16)(y)))


/***************************************
*     Data Types Definitions
***************************************/

/* LIN Core Types */
typedef unsigned char   l_bool;
typedef unsigned char   l_u8;
typedef unsigned int    l_u16;
typedef unsigned char   l_irqmask;
typedef unsigned char   l_ioctl_op;
typedef unsigned char   l_signal_handle;
typedef unsigned char   l_ifc_handle;
typedef unsigned char   l_flag_handle;

/* Detailed PID information */
typedef struct _`$INSTANCE_NAME`_PID_INFO_TABLE
{
    l_u8   param;
    volatile l_u8   *dataPtr;
}   `$INSTANCE_NAME`_PID_INFO_TABLE;

/* LIN Slave Configuration */
typedef struct _`$INSTANCE_NAME`_SLAVE_CONFIG
{
    #if(1u == `$INSTANCE_NAME`_TL_ENABLED)

        /* Initial NAD */
        l_u8   initialNad;

    #endif /* (1u == `$INSTANCE_NAME`_TL_ENABLED) */

    /* PID Table */
    l_u8 pidTable[`$INSTANCE_NAME`_NUM_FRAMES];

 }   `$INSTANCE_NAME`_SLAVE_CONFIG;

#if(1u == `$INSTANCE_NAME`_CS_ENABLED)

    /* LIN Slave Identification */
    typedef struct _`$INSTANCE_NAME`_SLAVE_ID
    {
        l_u16  supplierId;
        l_u16  functionId;
        l_u8   variant;
    }   `$INSTANCE_NAME`_SLAVE_ID;

#endif /* (1u == `$INSTANCE_NAME`_CS_ENABLED) */


/***************************************
*      Data Struct Definition
***************************************/

/* Sleep Mode API Support */
typedef struct _`$INSTANCE_NAME`_backupStruct
{
    l_u8 enableState;
    l_u8 control;

    #if(CY_UDB_V0)

        l_u8 statusMask;

        #if(1u == `$INSTANCE_NAME`_INACTIVITY_ENABLED)

            l_u8 inactivityDiv0;
            l_u8 inactivityDiv1;

        #endif  /* (1u == `$INSTANCE_NAME`_INACTIVITY_ENABLED) */

    #endif  /* CY_UDB_V0 */

} `$INSTANCE_NAME`_BACKUP_STRUCT;


/**************************************
*  Function Prototypes
**************************************/

l_bool  l_sys_init(void)  `=ReentrantKeil("l_sys_init")`;
l_bool  `$INSTANCE_NAME`_Start(void) `=ReentrantKeil($INSTANCE_NAME . "_Start")`;
l_u8    `$INSTANCE_NAME`_Stop(void) `=ReentrantKeil($INSTANCE_NAME . "_Stop")`;


`$FuncPrototypes`

l_bool l_ifc_init_`$INSTANCE_NAME`(void) `=ReentrantKeil("l_ifc_init_" . $INSTANCE_NAME)`;
l_bool l_ifc_init(l_ifc_handle)`=ReentrantKeil("l_ifc_init")`;

void l_ifc_wake_up_`$INSTANCE_NAME`(void) `=ReentrantKeil("l_ifc_wake_up_" . $INSTANCE_NAME)`;
void l_ifc_wake_up(l_ifc_handle) `=ReentrantKeil("l_ifc_wake_up")`;

l_u16 l_ifc_ioctl_`$INSTANCE_NAME`(l_ioctl_op, void*) `=ReentrantKeil("l_ifc_ioctl_" . $INSTANCE_NAME)`;
l_u16 l_ifc_ioctl(l_ifc_handle, l_ioctl_op, void*) `=ReentrantKeil("l_ifc_ioctl")`;

void l_ifc_rx_`$INSTANCE_NAME`(void) `=ReentrantKeil("l_ifc_rx_" . $INSTANCE_NAME)`;
void l_ifc_rx(l_ifc_handle) `=ReentrantKeil("l_ifc_rx")`;

void l_ifc_tx_`$INSTANCE_NAME`(void) `=ReentrantKeil("l_ifc_tx_" . $INSTANCE_NAME)`;
void l_ifc_tx(l_ifc_handle) `=ReentrantKeil("l_ifc_tx")`;

void l_ifc_aux_`$INSTANCE_NAME`(void) `=ReentrantKeil("l_ifc_aux_" . $INSTANCE_NAME)`;
void l_ifc_aux(l_ifc_handle) `=ReentrantKeil("l_ifc_aux")`;

l_u16 l_ifc_read_status_`$INSTANCE_NAME`(void) `=ReentrantKeil("l_ifc_read_status_" . $INSTANCE_NAME)`;
l_u16 l_ifc_read_status(l_ifc_handle) `=ReentrantKeil("l_ifc_read_status")`;

l_irqmask   l_sys_irq_disable(void)  `=ReentrantKeil("l_sys_irq_disable")`;
void        l_sys_irq_restore(l_irqmask) `=ReentrantKeil("l_sys_irq_restore")`;


#if(1u == `$INSTANCE_NAME`_TL_ENABLED)

    #if(1u == `$INSTANCE_NAME`_CS_ENABLED)

        l_u8 ld_read_by_id_callout(l_ifc_handle, l_u8, volatile l_u8*) `=ReentrantKeil("ld_read_by_id_callout")`;

    #endif /* (1u == `$INSTANCE_NAME`_CS_ENABLED) */

    /* Transport Layer Functions: Initialization */
    void        ld_init(l_ifc_handle) `=ReentrantKeil("ld_init")`;

    /* Transport Layer Functions: Node Configuration Functions */
    l_u8        ld_read_configuration(l_ifc_handle, l_u8*, l_u8*) `=ReentrantKeil("ld_read_configuration")`;
    l_u8        ld_set_configuration(l_ifc_handle, l_u8*, l_u16) `=ReentrantKeil("ld_set_configuration")`;

    #if(1u == `$INSTANCE_NAME`_TL_API_FORMAT)

        /* Transport Layer Functions: Cooked Transport Layer API */
        void        ld_send_message(l_ifc_handle, l_u16, l_u8, l_u8*) `=ReentrantKeil("ld_send_message")`;
        void        ld_receive_message(l_ifc_handle, l_u16* const, l_u8* const, l_u8* const)
                        `=ReentrantKeil("ld_receive_message")`;
        l_u8        ld_tx_status(l_ifc_handle) `=ReentrantKeil("ld_tx_status")`;
        l_u8        ld_rx_status(l_ifc_handle) `=ReentrantKeil("ld_rx_status")`;

    #else

        /* Transport Layer Functions: Raw Transport Layer API */
        void        ld_put_raw(l_ifc_handle, const l_u8* const) `=ReentrantKeil("ld_put_raw")`;
        void        ld_get_raw(l_ifc_handle, l_u8* const) `=ReentrantKeil("ld_get_raw")`;
        l_u8        ld_raw_tx_status(l_ifc_handle) `=ReentrantKeil("ld_raw_tx_status")`;
        l_u8        ld_raw_rx_status(l_ifc_handle) `=ReentrantKeil("ld_raw_rx_status")`;

    #endif /* (1u == `$INSTANCE_NAME`_TL_API_FORMAT) */

#endif /* (1u == `$INSTANCE_NAME`_TL_ENABLED) */

/* ISR Prototypes */
CY_ISR_PROTO(`$INSTANCE_NAME`_BLIN_ISR);
CY_ISR_PROTO(`$INSTANCE_NAME`_UART_ISR);


/***************************************
*   API Constants
***************************************/
`$SignalsDefGen`


`$signalsHandle`


`$flagsHandle`


`$framesFlagMask`


/* Break threshold detection value assuming 16x oversampling */
#define `$INSTANCE_NAME`_BREAK_THRESHOLD_VALUE          (16 * `$INSTANCE_NAME`_BREAK_THRESHOLD)

/* 8 bits with 16x oversampling rate */
#define `$INSTANCE_NAME`_EXPECTED_TIME_COUNTS           (128)

/* Bus inactivity block reconfiguration */
#if(1u == `$INSTANCE_NAME`_INACTIVITY_ENABLED)

    /* Bus inactivity block configuration to issue interrupt */
    #define `$INSTANCE_NAME`_INACT_100MS_IN_S           (10u)
    #define `$INSTANCE_NAME`_INACT_OVERSAMPLE_RATE      (16u)
    #define `$INSTANCE_NAME`_INACT_DIVIDE_FACTOR        (256u)

    #define `$INSTANCE_NAME`_INACT_TIME_FACTOR          \
        ((l_u16) ((`$INSTANCE_NAME`_INACT_OVERSAMPLE_RATE) * ((`$INSTANCE_NAME`_BAUD_RATE) / \
        (`$INSTANCE_NAME`_INACT_100MS_IN_S))))

    /* Divider 1 for specified interrupt rate */
    #define `$INSTANCE_NAME`_INACT_DIV1        (HI8(`$INSTANCE_NAME`_INACT_TIME_FACTOR))

    /* Divider 0 for specified interrupt rate */
    #define `$INSTANCE_NAME`_INACT_DIV0        (LO8(`$INSTANCE_NAME`_INACT_TIME_FACTOR))

#endif  /* (1u == `$INSTANCE_NAME`_INACTIVITY_ENABLED) */

#define `$INSTANCE_NAME`_FALSE                  (0x00u)
#define `$INSTANCE_NAME`_TRUE                   (!(`$INSTANCE_NAME`_FALSE))

#define `$INSTANCE_NAME`_ALL_IRQ_ENABLE         (0x03u)


#define `$INSTANCE_NAME`_FRAME_PID_MRF          (0x3Cu)
#define `$INSTANCE_NAME`_FRAME_PID_SRF          (0x7Du)
#define `$INSTANCE_NAME`_FRAME_SYNC_BYTE        (0x55u)

#if(1u == `$INSTANCE_NAME`_SAE_J2602)

    #define `$INSTANCE_NAME`_FRAME_PID_MRF_J2602    (0xFEu)

#endif /* (1u == `$INSTANCE_NAME`_SAE_J2602) */


/* Numbers and priorities of bLIN and UART interrupts */
#define `$INSTANCE_NAME`_BLIN_ISR_NUMBER        (`$INSTANCE_NAME``[BLIN_ISR]`_INTC_NUMBER)
#define `$INSTANCE_NAME`_BLIN_ISR_PRIORITY      (`$INSTANCE_NAME``[BLIN_ISR]`_INTC_PRIOR_NUM)

#define `$INSTANCE_NAME`_UART_ISR_NUMBER        (`$INSTANCE_NAME``[UART_ISR]`_INTC_NUMBER)
#define `$INSTANCE_NAME`_UART_ISR_PRIORITY      (`$INSTANCE_NAME``[UART_ISR]`_INTC_PRIOR_NUM)

/* Wake up signal length in us */
#define `$INSTANCE_NAME`_WAKE_UP_SIGNAL_LENGTH      (300u)
#define `$INSTANCE_NAME`_INVALID_FRAME_PID          (0xFFu)

/* Used to clear UART Rx FIFO */
#define `$INSTANCE_NAME`_UART_RX_FIFO_CLEAR         (0x01u)

/*******************************************************************************
*                       UART State Machine                                     *
*******************************************************************************/
/* Auto Baud Rate Sync Enabled */
#if(0u == `$INSTANCE_NAME`_AUTO_BAUD_RATE_SYNC)

    /* Sync Field detection */
    #define `$INSTANCE_NAME`_UART_ISR_STATE_0_SNC       (0x00u)

#endif  /* (0u == `$INSTANCE_NAME`_AUTO_BAUD_RATE_SYNC) */

/* Receive PID was detected. Analize recieved PID and determine action */
#define `$INSTANCE_NAME`_UART_ISR_STATE_1_PID       (0x01u)

/* Transmit data and checksum byte to the LIN master */
#define `$INSTANCE_NAME`_UART_ISR_STATE_2_TX        (0x02u)

/* Read data from the LIN master */
#define `$INSTANCE_NAME`_UART_ISR_STATE_3_RX        (0x03u)

/* Checksum verification */
#define `$INSTANCE_NAME`_UART_ISR_STATE_4_CHS       (0x04u)

/* PID Parity Error Mask */
#define `$INSTANCE_NAME`_PID_PARITY_MASK            (0x3Fu)


/*******************************************************************************
*                     `$INSTANCE_NAME`_PID_INFO_TABLE                          *
*******************************************************************************/
#define `$INSTANCE_NAME`_FRAME_DATA_SIZE_1          (0x01u)
#define `$INSTANCE_NAME`_FRAME_DATA_SIZE_2          (0x02u)
#define `$INSTANCE_NAME`_FRAME_DATA_SIZE_3          (0x03u)
#define `$INSTANCE_NAME`_FRAME_DATA_SIZE_4          (0x04u)
#define `$INSTANCE_NAME`_FRAME_DATA_SIZE_5          (0x05u)
#define `$INSTANCE_NAME`_FRAME_DATA_SIZE_6          (0x06u)
#define `$INSTANCE_NAME`_FRAME_DATA_SIZE_7          (0x07u)
#define `$INSTANCE_NAME`_FRAME_DATA_SIZE_8          (0x08u)

/* LIN slave direction = subscribe  */
#define `$INSTANCE_NAME`_FRAME_DIR_SUBSCRIBE        (0x00u)

/* LIN slave direction = publish    */
#define `$INSTANCE_NAME`_FRAME_DIR_PUBLISH          (0x10u)

/* LIN frame type = event-triggered */
#define `$INSTANCE_NAME`_FRAME_TYPE_EVENT           (0x20u)

/* LIN frame type = unconditional   */
#define `$INSTANCE_NAME`_FRAME_TYPE_UNCOND          (0x00u)

#define `$INSTANCE_NAME`_FRAME_DATA_SIZE_MASK       (0x0Fu)
#define `$INSTANCE_NAME`_FRAME_DIR_MASK             (0xEFu)
#define `$INSTANCE_NAME`_FRAME_TYPE_MASK            (0xDEu)


/*******************************************************************************
*                                Interface Status
*                          `$INSTANCE_NAME`_ifcStatus
*******************************************************************************/
/* Error in respons */
#define `$INSTANCE_NAME`_IFC_STS_ERROR_IN_RESPONSE  (0x0001u)

/* Successful frame transfer  */
#define `$INSTANCE_NAME`_IFC_STS_SUCCESSFUL_TRANSFER (0x0002u)

/* Overrun */
#define `$INSTANCE_NAME`_IFC_STS_OVERRUN            (0x0004u)

/* Go to sleep */
#define `$INSTANCE_NAME`_IFC_STS_GO_TO_SLEEP        (0x0008u)

/* Bus activity */
#define `$INSTANCE_NAME`_IFC_STS_BUS_ACTIVITY       (0x0010u)

/* Event-triggered frame collision  */
#define `$INSTANCE_NAME`_IFC_STS_EVTRIG_COLLISION   (0x0020u)

/* Save configuration */
#define `$INSTANCE_NAME`_IFC_STS_SAVE_CONFIG        (0x0040u)

/* Last frame PID mask */
#define `$INSTANCE_NAME`_IFC_STS_PID_MASK           (0xFF00u)

/* Status mask */
#define `$INSTANCE_NAME`_IFC_STS_MASK               (0xFFFFu)

/*******************************************************************************
*                          Internal LIN Slave Status
*                           `$INSTANCE_NAME`_status
*******************************************************************************/

/* This bit indicates that there is a response for ACRH service
*  and it is ready to be sent to master node.
*/
#define `$INSTANCE_NAME`_STATUS_SRVC_RSP_RDY        (0x80u)


#define `$INSTANCE_NAME`_STATUS_ERROR_MIRROR        (0x01u)
#define `$INSTANCE_NAME`_STATUS_NOT_CHECKED         (0x04u)
#define `$INSTANCE_NAME`_STATUS_RESPONSE_ACTIVE     (0x10u)



#if(1u == `$INSTANCE_NAME`_SAE_J2602)
    /***************************************************************************
    *                           SAE J2602-1 Error states
    *                          `$INSTANCE_NAME`_j2602Status
    ***************************************************************************/

    /* Clear all error buts in the j2602 status word mask */
    #define `$INSTANCE_NAME`_J2602_CLEAR_ERR_BITS_MASK  (0x1Fu)

    /* Data error */
    #define `$INSTANCE_NAME`_J2602_STS_DATA_ERR         (0x80u)

    /* Checksum error  */
    #define `$INSTANCE_NAME`_J2602_STS_CHECKSUM_ERR     (0xA0u)

    /* Framing error  */
    #define `$INSTANCE_NAME`_J2602_STS_FRAMING_ERR      (0xC0u)

    /* Parity error  */
    #define `$INSTANCE_NAME`_J2602_STS_PARITY_ERR       (0xE0u)

#endif  /* (1u == `$INSTANCE_NAME`_SAE_J2602) */


/*******************************************************************************
*                       `$INSTANCE_NAME`_EndFrame(l_u16 status)
*******************************************************************************/
#define `$INSTANCE_NAME`_HANDLING_RESET_FSM_ERR     (0x01u)
#define `$INSTANCE_NAME`_HANDLING_DONT_SAVE_PID     (0x10u)
#define `$INSTANCE_NAME`_HANDLING_SKIP_OVERRUN      (0x20u)
#define `$INSTANCE_NAME`_HANDLING_PID_ERR           (0x40u)


/*******************************************************************************
*                           `$INSTANCE_NAME`_fsmFlags
*******************************************************************************/

/* Break signal detected  */
#define `$INSTANCE_NAME`_FSM_BREAK_FLAG             (0x01u)

/* Sleep API was called */
#define `$INSTANCE_NAME`_FSM_SLEEP_API_FLAG         (0x02u)

/* UART receives at least 1 BYTE of data */
#define `$INSTANCE_NAME`_FSM_DATA_RECEIVE           (0x04u)

/* UART enable flag */
#define `$INSTANCE_NAME`_FSM_UART_ENABLE_FLAG       (0x08u)

/* Overrun Flag is used for status word */
#define `$INSTANCE_NAME`_FSM_OVERRUN                (0x40u)

/* Framing error */
#define `$INSTANCE_NAME`_FSM_FRAMING_ERROR_FLAG     (0x80u)


/*******************************************************************************
*                           Transport Layer
*******************************************************************************/

#if(1u == `$INSTANCE_NAME`_TL_ENABLED)

    #if(1u == `$INSTANCE_NAME`_CS_ENABLED)

        /* Wilcard ID understandable for every slave node */
        #define `$INSTANCE_NAME`_CS_SUPPLIER_ID_WILDCARD    (0x7FFFu)
        #define `$INSTANCE_NAME`_CS_FUNCTION_ID_WILDCARD    (0xFFFFu)

        #define `$INSTANCE_NAME`_CS_BYTE_SUPPLIER_ID1       (0x01u)
        #define `$INSTANCE_NAME`_CS_BYTE_SUPPLIER_ID2       (0x02u)
        #define `$INSTANCE_NAME`_CS_BYTE_FUNCTION_ID1       (0x03u)
        #define `$INSTANCE_NAME`_CS_BYTE_FUNCTION_ID2       (0x04u)
        #define `$INSTANCE_NAME`_CS_BYTE_VARIANT            (0x05u)

    #endif /* (1u == `$INSTANCE_NAME`_CS_ENABLED) */

    #if(1u == `$INSTANCE_NAME`_TL_API_FORMAT)

        /* Specifies the value for the TL timeouts */
        #define `$INSTANCE_NAME`_TL_N_AS_TIMEOUT_VALUE      (10u)
        #define `$INSTANCE_NAME`_TL_N_CR_TIMEOUT_VALUE      (10u)

    #endif /* (1u == `$INSTANCE_NAME`_TL_API_FORMAT) */

    /* Specifies the Frame buffer length for Transport Layer */
    #define `$INSTANCE_NAME`_FRAME_BUFF_LEN             (8u)

    /* Specifies the Frame length for Transport Layer */
    #define `$INSTANCE_NAME`_FRAME_LEN                  (8u)

    /* Packet Data Unit (PDU) Offsets */
    #define `$INSTANCE_NAME`_PDU_NAD_IDX                (0u)
    #define `$INSTANCE_NAME`_PDU_PCI_IDX                (1u)
    #define `$INSTANCE_NAME`_PDU_SID_IDX                (2u)
    #define `$INSTANCE_NAME`_PDU_LEN_IDX                (2u)
    #define `$INSTANCE_NAME`_PDU_D1_IDX                 (3u)
    #define `$INSTANCE_NAME`_PDU_D1_START_IDX           (3u)
    #define `$INSTANCE_NAME`_PDU_D1_ID_IDX              (3u)
    #define `$INSTANCE_NAME`_PDU_D2_IDX                 (4u)
    #define `$INSTANCE_NAME`_PDU_D2_PID_IDX             (4u)
    #define `$INSTANCE_NAME`_PDU_D2_BYTE_IDX            (4u)
    #define `$INSTANCE_NAME`_PDU_D3_IDX                 (5u)
    #define `$INSTANCE_NAME`_PDU_D3_MASK_IDX            (5u)
    #define `$INSTANCE_NAME`_PDU_D4_IDX                 (6u)
    #define `$INSTANCE_NAME`_PDU_D4_INVERT_IDX          (6u)
    #define `$INSTANCE_NAME`_PDU_D5_IDX                 (7u)
    #define `$INSTANCE_NAME`_PDU_D5_NEW_NAD_IDX         (7u)

    /* Single Frame data length  */
    #define `$INSTANCE_NAME`_PDU_SF_DATA_LEN            (6u)

    /* Protocol Control Information (PCI) Types */
    #define `$INSTANCE_NAME`_PDU_PCI_TYPE_SF            (0x00u)     /* Single       Frame */
    #define `$INSTANCE_NAME`_PDU_PCI_TYPE_FF            (0x10u)     /* First        Frame */
    #define `$INSTANCE_NAME`_PDU_PCI_TYPE_CF            (0x20u)     /* Consecutive  Frame */
    #define `$INSTANCE_NAME`_PDU_PCI_TYPE_UNKNOWN       (0xFFu)
    #define `$INSTANCE_NAME`_PDU_PCI_TYPE_MASK          (0xF0u)

    #define `$INSTANCE_NAME`_NAD_FUNCTIONAL             (0x7Eu)

    /* Wildcard NAD */
    #define `$INSTANCE_NAME`_NAD_BROADCAST              (0x7Fu)

    /* Go to sleep command ID */
    #define `$INSTANCE_NAME`_NAD_GO_TO_SLEEP            (0x00u)

    /* Max and min Config Services IDs */
    #define `$INSTANCE_NAME`_SID_CONF_MIN               (0xB0u)
    #define `$INSTANCE_NAME`_SID_CONF_MAX               (0xB7u)

    #if(1u == `$INSTANCE_NAME`_CS_ENABLED)

        /* Node Configuration Services */
        #define `$INSTANCE_NAME`_NCS_ASSIGN_NAD             (0xB0u)
        #define `$INSTANCE_NAME`_NCS_ASSIGN_FRAME_ID        (0xB1u) /* Used only in LIN 2.0 */
        #define `$INSTANCE_NAME`_NCS_READ_BY_ID             (0xB2u)
        #define `$INSTANCE_NAME`_NCS_COND_CHANGE_NAD        (0xB3u)
        #define `$INSTANCE_NAME`_NCS_DATA_DUMP              (0xB4u) /* Not supported */
        #define `$INSTANCE_NAME`_NCS_ASSIGN_NAD_SNPD        (0xB5u) /* Not supported */
        #define `$INSTANCE_NAME`_NCS_SAVE_CONFIG            (0xB6u)
        #define `$INSTANCE_NAME`_NCS_ASSIGN_FRAME_ID_RANGE  (0xB7u)

        /* Bit masks for selected services */
        #define `$INSTANCE_NAME`_NCS_0xB0_SEL               (0x01u)
        #define `$INSTANCE_NAME`_NCS_0xB1_SEL               (0x02u)
        #define `$INSTANCE_NAME`_NCS_0xB2_SEL               (0x04u)
        #define `$INSTANCE_NAME`_NCS_0xB3_SEL               (0x08u)
        #define `$INSTANCE_NAME`_NCS_0xB4_SEL               (0x10u)
        #define `$INSTANCE_NAME`_NCS_0xB5_SEL               (0x20u)
        #define `$INSTANCE_NAME`_NCS_0xB6_SEL               (0x40u)
        #define `$INSTANCE_NAME`_NCS_0xB7_SEL               (0x80u)

        /* Positive responses for Node Configuration Services requests */
        #define `$INSTANCE_NAME`_NCS_POS_RESP_ASSIGN_NAD             (0xF0u)
        #define `$INSTANCE_NAME`_NCS_POS_RESP_ASSIGN_FRAME_ID        (0xF1u) /* Used only in LIN 2.0 */
        #define `$INSTANCE_NAME`_NCS_POS_RESP_READ_BY_ID             (0xF2u)
        #define `$INSTANCE_NAME`_NCS_POS_RESP_COND_CHANGE_NAD        (0xF3u)
        #define `$INSTANCE_NAME`_NCS_POS_RESP_DATA_DUMP              (0xF4u) /* Not supported */
        #define `$INSTANCE_NAME`_NCS_POS_RESP_ASSIGN_NAD_SNPD        (0xF5u) /* Not supported */
        #define `$INSTANCE_NAME`_NCS_POS_RESP_SAVE_CONFIG            (0xF6u)
        #define `$INSTANCE_NAME`_NCS_POS_RESP_ASSIGN_FRAME_ID_RANGE  (0xF7u)

        /* Other LIN TL constants */
        #define `$INSTANCE_NAME`_NCS_READ_BY_ID_ID          (0x00u)
        #define `$INSTANCE_NAME`_NCS_READ_BY_ID_SERIAL      (0x01u)
        #define `$INSTANCE_NAME`_NCS_RSID_NEG_REPLY         (0x7Fu)
        #define `$INSTANCE_NAME`_NCS_MAX_FRAME_ID_RANGE     (0x04u)

        /* ld_read_by_id_callout() return constants */
        #define LD_NEGATIVE_RESPONSE                        (0x00u)
        #define LD_NO_RESPONSE                              (0x01u)
        #define LD_POSITIVE_RESPONSE                        (0x02u)

        #if(1u == `$INSTANCE_NAME`_LIN_2_0)

            #define LD_INVALID_MESSAGE_INDEX                (0xFFu)
            #define LD_MESSAGE_ID_BASE                      (0x10u)

        #endif /* (1u == `$INSTANCE_NAME`_LIN_2_0) */


    #endif /* (1u == `$INSTANCE_NAME`_CS_ENABLED) */

    /*******************************************************************************
    *       `$INSTANCE_NAME`_txTlStatus and `$INSTANCE_NAME`_rxTlStatus
    *******************************************************************************/

    /* Reception or transmission is not yet completed */
    #define LD_IN_PROGRESS             (0x01u)

    /* Reception or transmission has completed successfully */
    #define LD_COMPLETED               (0x02u)

    /* Reception or transmittion ended in an error */
    #define LD_FAILED                  (0x03u)

    /* Transmittion failed because of a N_As timeout */
    #define LD_N_AS_TIMEOUT            (0x04u)

    /* Reception failed because of a N_Cr timeout */
    #define LD_N_CR_TIMEOUT            (0x05u)

    /* Reception failed because of unexpected sequence number */
    #define LD_WRONG_SN                (0x06u)

    /* The transmit queue is empty */
    #define LD_QUEUE_EMPTY             (0x07u)

    /* The transmit queue contains entires */
    #define LD_QUEUE_AVAILABLE         (0x08u)

    /* The transmit queue is full and cant accept further frames */
    #define LD_QUEUE_FULL              (0x09u)

    /* LIN protocol errors occured during the transfer */
    #define LD_TRANSMIT_ERROR          (0x0Au)

    /* The receive queue is empty */
    #define LD_NO_DATA                 (0x0Bu)

    /* The receive queue contains data that can be read */
    #define LD_DATA_AVAILABLE          (0x0Cu)

    /* LIN protocol errors occured during the transfer */
    #define LD_RECEIVE_ERROR           (0x0Du)

    /*******************************************************************************
    *                           `$INSTANCE_NAME`_tlFlags
    *******************************************************************************/

    /* The requested service is disabled and diagnostic frame will
    * be "passed" to Transport Layer.
    */
    #define `$INSTANCE_NAME`_TL_CS_SERVICE_DISABLED       (0x01u)

    /* The SID isn't ACRH but a simple diagnostic SID */
    #define `$INSTANCE_NAME`_TL_DIAG_FRAME_DETECTED       (0x02u)

    /* The last PID that occured is SRF PID */
    #define `$INSTANCE_NAME`_TL_TX_DIRECTION              (0x04u)

    /* The last PID that occured is MRF PID */
    #define `$INSTANCE_NAME`_TL_RX_DIRECTION              (0x08u)

    /* Indicates that Cooked API requested trasmit data */
    #define `$INSTANCE_NAME`_TL_TX_REQUESTED              (0x10u)

    /* Indicates that Cooked API requested receive data */
    #define `$INSTANCE_NAME`_TL_RX_REQUESTED              (0x20u)

    /* In dicates that N_AS timeout is monitoring is in progress */
    #define `$INSTANCE_NAME`_TL_N_AS_TIMEOUT_ON           (0x40u)

    /* In dicates that N_CR timeout is monitoring is in progress */
    #define `$INSTANCE_NAME`_TL_N_CR_TIMEOUT_ON           (0x80u)


    /*******************************************************************************
    *            ld_read_configuration() and ld_set_configuration()
    *******************************************************************************/

    /* Read configuration is complete */
    #define `$INSTANCE_NAME`_LD_READ_OK                 (0x01u)

    /* Read configuration is fail. Configuration size is greater than the length. */
    #define `$INSTANCE_NAME`_LD_LENGTH_TOO_SHORT        (0x02u)

    /* Set configuration is complete */
    #define `$INSTANCE_NAME`_LD_SET_OK                  (0x01u)

    /* set configuration is fail.
    *  Required size of the configuration is not equal to the given length.
    */
    #define `$INSTANCE_NAME`_LD_LENGTH_NOT_CORRECT      (0x02u)

    /* Set configuration is fail. Set conf could not be made. */
    #define `$INSTANCE_NAME`_LD_DATA_ERROR              (0x04u)


#endif /* (1u == `$INSTANCE_NAME`_TL_ENABLED) */


/*******************************************************************************
*                   l_ifc_ioctl() parameters
*******************************************************************************/
#define L_IOCTL_READ_STATUS                     (0x00u)
#define L_IOCTL_SET_BAUD_RATE                   (0x01u)
#define L_IOCTL_SLEEP                           (0x02u)
#define L_IOCTL_WAKEUP                          (0x03u)
#define L_IOCTL_SYNC_COUNTS                     (0x04u)
#define L_IOCTL_SET_SERIAL_NUMBER               (0x05u)




/*******************************************************************************
*           Variable `$INSTANCE_NAME`_ioctlStatus flags
*******************************************************************************/
/* Bus inactivity */
#define `$INSTANCE_NAME`_IOCTL_STS_BUS_INACTIVITY (0x0001u)

/* Target reset (0xB5) */
#define `$INSTANCE_NAME`_IOCTL_STS_TARGET_RESET   (0x0002u)


/* Status Register */
#define `$INSTANCE_NAME`_STATUS_BREAK_DETECTED          (0x01u)
#define `$INSTANCE_NAME`_STATUS_EDGE_DETECTED           (0x02u)
#define `$INSTANCE_NAME`_STATUS_SYNC_COMPLETED          (0x08u)
#if(1u == `$INSTANCE_NAME`_INACTIVITY_ENABLED)
    #define `$INSTANCE_NAME`_STATUS_INACTIVITY_INT    (0x04u)
#endif  /* (1u == `$INSTANCE_NAME`_INACTIVITY_ENABLED) */


/* Control Register */
#define `$INSTANCE_NAME`_CONTROL_ENABLE             (0x01u)
#define `$INSTANCE_NAME`_CONTROL_TX_DIS             (0x02u)
#define `$INSTANCE_NAME`_CONTROL_RX_DIS             (0x04u)


/* Status Mask Register */
#define `$INSTANCE_NAME`_INT_MASK_BREAK          (0x01u)
#define `$INSTANCE_NAME`_INT_MASK_EDGE           (0x02u)
#define `$INSTANCE_NAME`_INT_MASK_SYNC           (0x08u)

#if(1u == `$INSTANCE_NAME`_INACTIVITY_ENABLED)
    #define `$INSTANCE_NAME`_INT_MASK_INACTIVITY (0x04u)
#else
    #define `$INSTANCE_NAME`_INT_MASK_INACTIVITY (0x00u)
#endif  /* (1u == `$INSTANCE_NAME`_INACTIVITY_ENABLED) */


#define `$INSTANCE_NAME`_STATUS_AUX_CONTROL_INT_EN     (0x10u)


/***************************************
*             Registers
***************************************/

#define `$INSTANCE_NAME`_CONTROL_REG            \
                                    (* (reg8 *) `$INSTANCE_NAME`_bLIN_`$CtlModeReplacementString`_CtrlReg__CONTROL_REG)
#define `$INSTANCE_NAME`_CONTROL_PTR            \
                                    (  (reg8 *) `$INSTANCE_NAME`_bLIN_`$CtlModeReplacementString`_CtrlReg__CONTROL_REG)

#define `$INSTANCE_NAME`_STATUS_REG             (* (reg8 *) `$INSTANCE_NAME`_bLIN_StsReg__STATUS_REG)
#define `$INSTANCE_NAME`_STATUS_PTR             (  (reg8 *) `$INSTANCE_NAME`_bLIN_StsReg__STATUS_REG)

#define `$INSTANCE_NAME`_INT_MASK_REG           (* (reg8 *) `$INSTANCE_NAME`_bLIN_StsReg__MASK_REG)
#define `$INSTANCE_NAME`_INT_MASK_PTR           (  (reg8 *) `$INSTANCE_NAME`_bLIN_StsReg__MASK_REG)

#define `$INSTANCE_NAME`_BREAK_THRESHOLD_REG    (* (reg8 *) `$INSTANCE_NAME`_bLIN_LINDp_u0__D0_REG)
#define `$INSTANCE_NAME`_BREAK_THRESHOLD_PTR    (  (reg8 *) `$INSTANCE_NAME`_bLIN_LINDp_u0__D0_REG)

#define `$INSTANCE_NAME`_STATUS_AUX_CONTROL_REG (* (reg8 *) `$INSTANCE_NAME`_bLIN_StsReg__STATUS_AUX_CTL_REG)
#define `$INSTANCE_NAME`_STATUS_AUX_CONTROL_PTR (  (reg8 *) `$INSTANCE_NAME`_bLIN_StsReg__STATUS_AUX_CTL_REG)


/* Auto Baud Rate Sync Enabled */
#if(1u == `$INSTANCE_NAME`_AUTO_BAUD_RATE_SYNC)

    #define `$INSTANCE_NAME`_HIGH_BITS_LENGTH_SUM_REG       (* (reg8 *) `$INSTANCE_NAME`_bLIN_LINDp_u0__F1_REG)
    #define `$INSTANCE_NAME`_HIGH_BITS_LENGTH_SUM_PTR       (  (reg8 *) `$INSTANCE_NAME`_bLIN_LINDp_u0__F1_REG)

    #define `$INSTANCE_NAME`_LOW_BIT_LENGTH_SUM_REG             (* (reg8 *) `$INSTANCE_NAME`_bLIN_LINDp_u0__F0_REG)
    #define `$INSTANCE_NAME`_LOW_BIT_LENGTH_SUM_PTR             (  (reg8 *) `$INSTANCE_NAME`_bLIN_LINDp_u0__F0_REG)

#endif  /* (1u == `$INSTANCE_NAME`_AUTO_BAUD_RATE_SYNC) */


/* Bus inactivity block configuration */
#if(1u == `$INSTANCE_NAME`_INACTIVITY_ENABLED)

    #define `$INSTANCE_NAME`_INACTIVITY_DIV0_REG    (* (reg8 *) `$INSTANCE_NAME`_bLIN_InactFSM_BusInactDp_u0__D0_REG)
    #define `$INSTANCE_NAME`_INACTIVITY_DIV0_PTR    (  (reg8 *) `$INSTANCE_NAME`_bLIN_InactFSM_BusInactDp_u0__D0_REG)

    #define `$INSTANCE_NAME`_INACTIVITY_DIV1_REG    (* (reg8 *) `$INSTANCE_NAME`_bLIN_InactFSM_BusInactDp_u0__D1_REG)
    #define `$INSTANCE_NAME`_INACTIVITY_DIV1_PTR    (  (reg8 *) `$INSTANCE_NAME`_bLIN_InactFSM_BusInactDp_u0__D1_REG)

#endif  /* (1u == `$INSTANCE_NAME`_INACTIVITY_ENABLED) */

/* Register used to clear UART RX FIFO */
#define `$INSTANCE_NAME`_UART_RX_FIFO_REG       \
                                            (* (reg8 *) `$INSTANCE_NAME`_UART_BUART_sRX_RxShifter_u0__DP_AUX_CTL_REG)
#define `$INSTANCE_NAME`_UART_RX_FIFO_PTR       \
                                            (  (reg8 *) `$INSTANCE_NAME`_UART_BUART_sRX_RxShifter_u0__DP_AUX_CTL_REG)

#endif /* CY_LIN_`$INSTANCE_NAME`_H */


/* [] END OF FILE */
