/*******************************************************************************
* File Name: `$INSTANCE_NAME`.h
* Version `$CY_MAJOR_VERSION`.`$CY_MINOR_VERSION`
*
* Description:
*  Header File for the USFS component. Contains prototypes and constant values.
*
********************************************************************************
* Copyright 2008-2012, Cypress Semiconductor Corporation.  All rights reserved.
* You may use this file only in accordance with the license, terms, conditions,
* disclaimers, and limitations in the end user license agreement accompanying
* the software package with which this file was provided.
*******************************************************************************/

#if !defined(CY_USBFS_`$INSTANCE_NAME`_H)
#define CY_USBFS_`$INSTANCE_NAME`_H

#include "cytypes.h"
#include "cydevice_trm.h"
#include "cyfitter.h"
#include "CyLib.h"


/***************************************
* Conditional Compilation Parameters
***************************************/

/* Check to see if required defines such as CY_PSOC5LP are available */
/* They are defined starting with cy_boot v3.0 */
#if !defined (CY_PSOC5LP)
    #error Component `$CY_COMPONENT_NAME` requires cy_boot v3.0 or later
#endif /* (CY_PSOC5LP) */


/***************************************
*  Memory Type Definitions
***************************************/

/* Renamed Type Definitions for backward compatibility.
*  Should not be used in new designs.
*/
#define `$INSTANCE_NAME`_CODE CYCODE
#define `$INSTANCE_NAME`_FAR CYFAR
#if defined(__C51__) || defined(__CX51__)
    #define `$INSTANCE_NAME`_DATA data
    #define `$INSTANCE_NAME`_XDATA xdata
#else
    #define `$INSTANCE_NAME`_DATA
    #define `$INSTANCE_NAME`_XDATA
#endif /* End __C51__ */
#define `$INSTANCE_NAME`_NULL       NULL


/***************************************
* Enumerated Types and Parameters
***************************************/

`#cy_declare_enum EndpointMMType`
`#cy_declare_enum EndpointMAType`


/***************************************
*    Initial Parameter Constants
***************************************/

`$APIGEN_DEFINES`
#define `$INSTANCE_NAME`_MON_VBUS                       (`$mon_vbus`u)
#define `$INSTANCE_NAME`_EXTERN_VND                     (`$extern_vnd`u)
#define `$INSTANCE_NAME`_EXTERN_CLS                     (`$extern_cls`u)
#define `$INSTANCE_NAME`_MAX_INTERFACES_NUMBER          (`$max_interfaces_num`u)
#define `$INSTANCE_NAME`_EP0_ISR_REMOVE                 (`$rm_ep_isr_0`u)
#define `$INSTANCE_NAME`_EP1_ISR_REMOVE                 (`$rm_ep_isr_1`u)
#define `$INSTANCE_NAME`_EP2_ISR_REMOVE                 (`$rm_ep_isr_2`u)
#define `$INSTANCE_NAME`_EP3_ISR_REMOVE                 (`$rm_ep_isr_3`u)
#define `$INSTANCE_NAME`_EP4_ISR_REMOVE                 (`$rm_ep_isr_4`u)
#define `$INSTANCE_NAME`_EP5_ISR_REMOVE                 (`$rm_ep_isr_5`u)
#define `$INSTANCE_NAME`_EP6_ISR_REMOVE                 (`$rm_ep_isr_6`u)
#define `$INSTANCE_NAME`_EP7_ISR_REMOVE                 (`$rm_ep_isr_7`u)
#define `$INSTANCE_NAME`_EP8_ISR_REMOVE                 (`$rm_ep_isr_8`u)
#define `$INSTANCE_NAME`_EP_MM                          (`$endpointMM`u)
#define `$INSTANCE_NAME`_EP_MA                          (`$endpointMA`u)
#define `$INSTANCE_NAME`_DMA1_REMOVE                    (`$rm_dma_1`u)
#define `$INSTANCE_NAME`_DMA2_REMOVE                    (`$rm_dma_2`u)
#define `$INSTANCE_NAME`_DMA3_REMOVE                    (`$rm_dma_3`u)
#define `$INSTANCE_NAME`_DMA4_REMOVE                    (`$rm_dma_4`u)
#define `$INSTANCE_NAME`_DMA5_REMOVE                    (`$rm_dma_5`u)
#define `$INSTANCE_NAME`_DMA6_REMOVE                    (`$rm_dma_6`u)
#define `$INSTANCE_NAME`_DMA7_REMOVE                    (`$rm_dma_7`u)
#define `$INSTANCE_NAME`_DMA8_REMOVE                    (`$rm_dma_8`u)
#define `$INSTANCE_NAME`_SOF_ISR_REMOVE                 (`$rm_sof_int`u)
#define `$INSTANCE_NAME`_ARB_ISR_REMOVE                 (`$rm_arb_int`u)
#define `$INSTANCE_NAME`_DP_ISR_REMOVE                  (`$rm_dp_int`u)
#define `$INSTANCE_NAME`_ENABLE_CDC_CLASS_API           (`$EnableCDCApi`u)
#define `$INSTANCE_NAME`_ENABLE_MIDI_API                (`$EnableMidiApi`u)
#define `$INSTANCE_NAME`_MIDI_EXT_MODE                  (`$extJackCount`u)


/***************************************
*    Data Struct Definition
***************************************/

typedef struct _`$INSTANCE_NAME`_EpCtlBlock
{
    uint8  attrib;
    uint8  apiEpState;
    uint8  hwEpState;
    uint8  epToggle;
    uint8  addr;
    uint8  epMode;
    uint16 buffOffset;
    uint16 bufferSize;
    uint8  interface;
} T_`$INSTANCE_NAME`_EP_CTL_BLOCK;

typedef struct _`$INSTANCE_NAME`_EpSettingsBlock
{
    uint8  interface;
    uint8  altSetting;
    uint8  addr;
    uint8  attributes;
    uint16 bufferSize;
    uint8  bMisc;
} T_`$INSTANCE_NAME`_EP_SETTINGS_BLOCK;

typedef struct _`$INSTANCE_NAME`_XferStatusBlock
{
    uint8  status;
    uint16 length;
} T_`$INSTANCE_NAME`_XFER_STATUS_BLOCK;

typedef struct _`$INSTANCE_NAME`_Td
{
    uint16  count;
    volatile uint8 *pData;
    T_`$INSTANCE_NAME`_XFER_STATUS_BLOCK *pStatusBlock;
} T_`$INSTANCE_NAME`_TD;

typedef struct _`$INSTANCE_NAME`_Lut
{
    uint8   c;
    void    *p_list;
} T_`$INSTANCE_NAME`_LUT;

/* Resume/Suspend API Support */
typedef struct _`$INSTANCE_NAME`_BackupStruct
{
    uint8 enableState;
    uint8 mode;
} `$INSTANCE_NAME`_BACKUP_STRUCT;


/* Renamed struct fields for backward compatibility.
*  Should not be used in new designs.
*/
#define wBuffOffset         buffOffset
#define wBufferSize         bufferSize
#define bStatus             status
#define wLength             length
#define wCount              count

/* Renamed global variable for backward compatibility.
*  Should not be used in new designs.
*/
#define CurrentTD           `$INSTANCE_NAME`_currentTD


/***************************************
*       Function Prototypes
***************************************/

void   `$INSTANCE_NAME`_Start(uint8 device, uint8 mode) `=ReentrantKeil($INSTANCE_NAME . "_Start")`;
void   `$INSTANCE_NAME`_Init(void) `=ReentrantKeil($INSTANCE_NAME . "_Init")`;
void   `$INSTANCE_NAME`_InitComponent(uint8 device, uint8 mode) `=ReentrantKeil($INSTANCE_NAME . "_InitComponent")`;
void   `$INSTANCE_NAME`_Stop(void) `=ReentrantKeil($INSTANCE_NAME . "_Stop")`;
uint8  `$INSTANCE_NAME`_CheckActivity(void) `=ReentrantKeil($INSTANCE_NAME . "_CheckActivity")`;
uint8  `$INSTANCE_NAME`_GetConfiguration(void) `=ReentrantKeil($INSTANCE_NAME . "_GetConfiguration")`;
uint8  `$INSTANCE_NAME`_IsConfigurationChanged(void) `=ReentrantKeil($INSTANCE_NAME . "_IsConfigurationChanged")`;
uint8  `$INSTANCE_NAME`_GetInterfaceSetting(uint8 interfaceNumber)
                                                        `=ReentrantKeil($INSTANCE_NAME . "_GetInterfaceSetting")`;
uint8  `$INSTANCE_NAME`_GetEPState(uint8 epNumber) `=ReentrantKeil($INSTANCE_NAME . "_GetEPState")`;
uint16 `$INSTANCE_NAME`_GetEPCount(uint8 epNumber) `=ReentrantKeil($INSTANCE_NAME . "_GetEPCount")`;
void   `$INSTANCE_NAME`_LoadInEP(uint8 epNumber, uint8 *pData, uint16 length)
                                                                    `=ReentrantKeil($INSTANCE_NAME . "_LoadInEP")`;
uint16 `$INSTANCE_NAME`_ReadOutEP(uint8 epNumber, uint8 *pData, uint16 length)
                                                                    `=ReentrantKeil($INSTANCE_NAME . "_ReadOutEP")`;
void   `$INSTANCE_NAME`_EnableOutEP(uint8 epNumber) `=ReentrantKeil($INSTANCE_NAME . "_EnableOutEP")`;
void   `$INSTANCE_NAME`_DisableOutEP(uint8 epNumber) `=ReentrantKeil($INSTANCE_NAME . "_DisableOutEP")`;
void   `$INSTANCE_NAME`_Force(uint8 bState) `=ReentrantKeil($INSTANCE_NAME . "_Force")`;
uint8  `$INSTANCE_NAME`_GetEPAckState(uint8 epNumber) `=ReentrantKeil($INSTANCE_NAME . "_GetEPAckState")`;
void   `$INSTANCE_NAME`_SetPowerStatus(uint8 powerStatus) `=ReentrantKeil($INSTANCE_NAME . "_SetPowerStatus")`;
uint8  `$INSTANCE_NAME`_RWUEnabled(void) `=ReentrantKeil($INSTANCE_NAME . "_RWUEnabled")`;
void   `$INSTANCE_NAME`_SerialNumString(uint8 *snString) `=ReentrantKeil($INSTANCE_NAME . "_SerialNumString")`;
void   `$INSTANCE_NAME`_TerminateEP(uint8 ep) `=ReentrantKeil($INSTANCE_NAME . "_TerminateEP")`;

void   `$INSTANCE_NAME`_Suspend(void) `=ReentrantKeil($INSTANCE_NAME . "_Suspend")`;
void   `$INSTANCE_NAME`_Resume(void) `=ReentrantKeil($INSTANCE_NAME . "_Resume")`;
#if(CY_PSOC5A)
    uint8 `$INSTANCE_NAME`_Resume_Condition(void);
#endif /* CY_PSOC5A */    

#if (`$INSTANCE_NAME`_MON_VBUS == 1u)
    uint8  `$INSTANCE_NAME`_VBusPresent(void) `=ReentrantKeil($INSTANCE_NAME . "_VBusPresent")`;
#endif /* End `$INSTANCE_NAME`_MON_VBUS */

#if defined(CYDEV_BOOTLOADER_IO_COMP) && ((CYDEV_BOOTLOADER_IO_COMP == CyBtldr_`$INSTANCE_NAME`) || \
                                          (CYDEV_BOOTLOADER_IO_COMP == CyBtldr_Custom_Interface))

    void `$INSTANCE_NAME`_CyBtldrCommStart(void) `=ReentrantKeil($INSTANCE_NAME . "_CyBtldrCommStart")`;
    void `$INSTANCE_NAME`_CyBtldrCommStop(void) `=ReentrantKeil($INSTANCE_NAME . "_CyBtldrCommStop")`;
    void `$INSTANCE_NAME`_CyBtldrCommReset(void) `=ReentrantKeil($INSTANCE_NAME . "_CyBtldrCommReset")`;
    cystatus `$INSTANCE_NAME`_CyBtldrCommWrite(uint8 *pData, uint16 size, uint16 *count, uint8 timeOut) CYSMALL
                                                        `=ReentrantKeil($INSTANCE_NAME . "_CyBtldrCommWrite")`;
    cystatus `$INSTANCE_NAME`_CyBtldrCommRead( uint8 *pData, uint16 size, uint16 *count, uint8 timeOut) CYSMALL
                                                        `=ReentrantKeil($INSTANCE_NAME . "_CyBtldrCommRead")`;

    #define `$INSTANCE_NAME`_BTLDR_SIZEOF_WRITE_BUFFER      (64u)    /* EP 1 OUT */
    #define `$INSTANCE_NAME`_BTLDR_SIZEOF_READ_BUFFER       (64u)    /* EP 2 IN */
    #define `$INSTANCE_NAME`_BTLDR_MAX_PACKET_SIZE          `$INSTANCE_NAME`_BTLDR_SIZEOF_WRITE_BUFFER

    /* These defines active if used USBFS interface as an
    *  IO Component for bootloading. When Custom_Interface selected
    *  in Bootloder configuration as the IO Component, user must
    *  provide these functions
    */
    #if (CYDEV_BOOTLOADER_IO_COMP == CyBtldr_`$INSTANCE_NAME`)
        #define CyBtldrCommStart        `$INSTANCE_NAME`_CyBtldrCommStart
        #define CyBtldrCommStop         `$INSTANCE_NAME`_CyBtldrCommStop
        #define CyBtldrCommReset        `$INSTANCE_NAME`_CyBtldrCommReset
        #define CyBtldrCommWrite        `$INSTANCE_NAME`_CyBtldrCommWrite
        #define CyBtldrCommRead         `$INSTANCE_NAME`_CyBtldrCommRead
    #endif  /*End   CYDEV_BOOTLOADER_IO_COMP == CyBtldr_`$INSTANCE_NAME` */

#endif /* End CYDEV_BOOTLOADER_IO_COMP  */

#if(`$INSTANCE_NAME`_EP_MM != `$INSTANCE_NAME`__EP_MANUAL)
    void `$INSTANCE_NAME`_InitEP_DMA(uint8 epNumber, uint8 *pData) `=ReentrantKeil($INSTANCE_NAME . "_InitEP_DMA")`;
    void `$INSTANCE_NAME`_Stop_DMA(uint8 epNumber) `=ReentrantKeil($INSTANCE_NAME . "_Stop_DMA")`;
#endif /* End `$INSTANCE_NAME`_EP_MM != `$INSTANCE_NAME`__EP_MANUAL) */

#if defined(`$INSTANCE_NAME`_ENABLE_MIDI_STREAMING) && (`$INSTANCE_NAME`_ENABLE_MIDI_API != 0u)
    void `$INSTANCE_NAME`_MIDI_EP_Init(void) `=ReentrantKeil($INSTANCE_NAME . "_MIDI_EP_Init")`;
    void `$INSTANCE_NAME`_MIDI_IN_Service(void) `=ReentrantKeil($INSTANCE_NAME . "_MIDI_IN_Service")`;
    uint8 `$INSTANCE_NAME`_PutUsbMidiIn(uint8 ic, uint8* midiMsg, uint8 cable)
                                                            `=ReentrantKeil($INSTANCE_NAME . "_PutUsbMidiIn")`;
    void `$INSTANCE_NAME`_MIDI_OUT_EP_Service(void) `=ReentrantKeil($INSTANCE_NAME . "_MIDI_OUT_EP_Service")`;
#endif /* End `$INSTANCE_NAME`_ENABLE_MIDI_API != 0u */

/* Renamed Functions for backward compatibility.
*  Should not be used in new designs.
*/

#define `$INSTANCE_NAME`_bCheckActivity             `$INSTANCE_NAME`_CheckActivity
#define `$INSTANCE_NAME`_bGetConfiguration          `$INSTANCE_NAME`_GetConfiguration
#define `$INSTANCE_NAME`_bGetInterfaceSetting       `$INSTANCE_NAME`_GetInterfaceSetting
#define `$INSTANCE_NAME`_bGetEPState                `$INSTANCE_NAME`_GetEPState
#define `$INSTANCE_NAME`_wGetEPCount                `$INSTANCE_NAME`_GetEPCount
#define `$INSTANCE_NAME`_bGetEPAckState             `$INSTANCE_NAME`_GetEPAckState
#define `$INSTANCE_NAME`_bRWUEnabled                `$INSTANCE_NAME`_RWUEnabled
#define `$INSTANCE_NAME`_bVBusPresent               `$INSTANCE_NAME`_VBusPresent

#define `$INSTANCE_NAME`_bConfiguration             `$INSTANCE_NAME`_configuration
#define `$INSTANCE_NAME`_bInterfaceSetting          `$INSTANCE_NAME`_interfaceSetting
#define `$INSTANCE_NAME`_bDeviceAddress             `$INSTANCE_NAME`_deviceAddress
#define `$INSTANCE_NAME`_bDeviceStatus              `$INSTANCE_NAME`_deviceStatus
#define `$INSTANCE_NAME`_bDevice                    `$INSTANCE_NAME`_device
#define `$INSTANCE_NAME`_bTransferState             `$INSTANCE_NAME`_transferState
#define `$INSTANCE_NAME`_bLastPacketSize            `$INSTANCE_NAME`_lastPacketSize

#define `$INSTANCE_NAME`_LoadEP                     `$INSTANCE_NAME`_LoadInEP
#define `$INSTANCE_NAME`_LoadInISOCEP               `$INSTANCE_NAME`_LoadInEP
#define `$INSTANCE_NAME`_EnableOutISOCEP(e)         `$INSTANCE_NAME`_EnableOutEP(e)

#define `$INSTANCE_NAME`_SetVector                  CyIntSetVector
#define `$INSTANCE_NAME`_SetPriority                CyIntSetPriority
#define `$INSTANCE_NAME`_EnableInt(a, b)            CyIntEnable(a)


/***************************************
*          API Constants
***************************************/

#define `$INSTANCE_NAME`_EP0                        (0u)
#define `$INSTANCE_NAME`_EP1                        (1u)
#define `$INSTANCE_NAME`_EP2                        (2u)
#define `$INSTANCE_NAME`_EP3                        (3u)
#define `$INSTANCE_NAME`_EP4                        (4u)
#define `$INSTANCE_NAME`_EP5                        (5u)
#define `$INSTANCE_NAME`_EP6                        (6u)
#define `$INSTANCE_NAME`_EP7                        (7u)
#define `$INSTANCE_NAME`_EP8                        (8u)
#define `$INSTANCE_NAME`_MAX_EP                     (9u)

#define `$INSTANCE_NAME`_TRUE                       (1u)
#define `$INSTANCE_NAME`_FALSE                      (0u)

#define `$INSTANCE_NAME`_NO_EVENT_ALLOWED           (2u)
#define `$INSTANCE_NAME`_EVENT_PENDING              (1u)
#define `$INSTANCE_NAME`_NO_EVENT_PENDING           (0u)

#define `$INSTANCE_NAME`_IN_BUFFER_FULL             `$INSTANCE_NAME`_NO_EVENT_PENDING
#define `$INSTANCE_NAME`_IN_BUFFER_EMPTY            `$INSTANCE_NAME`_EVENT_PENDING
#define `$INSTANCE_NAME`_OUT_BUFFER_FULL            `$INSTANCE_NAME`_EVENT_PENDING
#define `$INSTANCE_NAME`_OUT_BUFFER_EMPTY           `$INSTANCE_NAME`_NO_EVENT_PENDING

#define `$INSTANCE_NAME`_FORCE_J                    (0xA0u)
#define `$INSTANCE_NAME`_FORCE_K                    (0x80u)
#define `$INSTANCE_NAME`_FORCE_SE0                  (0xC0u)
#define `$INSTANCE_NAME`_FORCE_NONE                 (0x00u)

#define `$INSTANCE_NAME`_IDLE_TIMER_RUNNING         (0x02u)
#define `$INSTANCE_NAME`_IDLE_TIMER_EXPIRED         (0x01u)
#define `$INSTANCE_NAME`_IDLE_TIMER_INDEFINITE      (0x00u)

#define `$INSTANCE_NAME`_DEVICE_STATUS_BUS_POWERED  (0x00u)
#define `$INSTANCE_NAME`_DEVICE_STATUS_SELF_POWERED (0x01u)

#define `$INSTANCE_NAME`_3V_OPERATION               (0x00u)
#define `$INSTANCE_NAME`_5V_OPERATION               (0x01u)
#define `$INSTANCE_NAME`_DWR_VDDD_OPERATION         (0x02u)

#define `$INSTANCE_NAME`_MODE_DISABLE               (0x00u)
#define `$INSTANCE_NAME`_MODE_NAK_IN_OUT            (0x01u)
#define `$INSTANCE_NAME`_MODE_STATUS_OUT_ONLY       (0x02u)
#define `$INSTANCE_NAME`_MODE_STALL_IN_OUT          (0x03u)
#define `$INSTANCE_NAME`_MODE_RESERVED_0100         (0x04u)
#define `$INSTANCE_NAME`_MODE_ISO_OUT               (0x05u)
#define `$INSTANCE_NAME`_MODE_STATUS_IN_ONLY        (0x06u)
#define `$INSTANCE_NAME`_MODE_ISO_IN                (0x07u)
#define `$INSTANCE_NAME`_MODE_NAK_OUT               (0x08u)
#define `$INSTANCE_NAME`_MODE_ACK_OUT               (0x09u)
#define `$INSTANCE_NAME`_MODE_RESERVED_1010         (0x0Au)
#define `$INSTANCE_NAME`_MODE_ACK_OUT_STATUS_IN     (0x0Bu)
#define `$INSTANCE_NAME`_MODE_NAK_IN                (0x0Cu)
#define `$INSTANCE_NAME`_MODE_ACK_IN                (0x0Du)
#define `$INSTANCE_NAME`_MODE_RESERVED_1110         (0x0Eu)
#define `$INSTANCE_NAME`_MODE_ACK_IN_STATUS_OUT     (0x0Fu)
#define `$INSTANCE_NAME`_MODE_MASK                  (0x0Fu)
#define `$INSTANCE_NAME`_MODE_STALL_DATA_EP         (0x80u)

#define `$INSTANCE_NAME`_MODE_ACKD                  (0x10u)
#define `$INSTANCE_NAME`_MODE_OUT_RCVD              (0x20u)
#define `$INSTANCE_NAME`_MODE_IN_RCVD               (0x40u)
#define `$INSTANCE_NAME`_MODE_SETUP_RCVD            (0x80u)

#define `$INSTANCE_NAME`_RQST_TYPE_MASK             (0x60u)
#define `$INSTANCE_NAME`_RQST_TYPE_STD              (0x00u)
#define `$INSTANCE_NAME`_RQST_TYPE_CLS              (0x20u)
#define `$INSTANCE_NAME`_RQST_TYPE_VND              (0x40u)
#define `$INSTANCE_NAME`_RQST_DIR_MASK              (0x80u)
#define `$INSTANCE_NAME`_RQST_DIR_D2H               (0x80u)
#define `$INSTANCE_NAME`_RQST_DIR_H2D               (0x00u)
#define `$INSTANCE_NAME`_RQST_RCPT_MASK             (0x03u)
#define `$INSTANCE_NAME`_RQST_RCPT_DEV              (0x00u)
#define `$INSTANCE_NAME`_RQST_RCPT_IFC              (0x01u)
#define `$INSTANCE_NAME`_RQST_RCPT_EP               (0x02u)
#define `$INSTANCE_NAME`_RQST_RCPT_OTHER            (0x03u)

/* USB Class Codes */
#define `$INSTANCE_NAME`_CLASS_DEVICE               (0x00u)     /* Use class code info from Interface Descriptors */
#define `$INSTANCE_NAME`_CLASS_AUDIO                (0x01u)     /* Audio device */
#define `$INSTANCE_NAME`_CLASS_CDC                  (0x02u)     /* Communication device class */
#define `$INSTANCE_NAME`_CLASS_HID                  (0x03u)     /* Human Interface Device */
#define `$INSTANCE_NAME`_CLASS_PDC                  (0x05u)     /* Physical device class */
#define `$INSTANCE_NAME`_CLASS_IMAGE                (0x06u)     /* Still Imaging device */
#define `$INSTANCE_NAME`_CLASS_PRINTER              (0x07u)     /* Printer device  */
#define `$INSTANCE_NAME`_CLASS_MSD                  (0x08u)     /* Mass Storage device  */
#define `$INSTANCE_NAME`_CLASS_HUB                  (0x09u)     /* Full/Hi speed Hub */
#define `$INSTANCE_NAME`_CLASS_CDC_DATA             (0x0Au)     /* CDC data device */
#define `$INSTANCE_NAME`_CLASS_SMART_CARD           (0x0Bu)     /* Smart Card device */
#define `$INSTANCE_NAME`_CLASS_CSD                  (0x0Du)     /* Content Security device */
#define `$INSTANCE_NAME`_CLASS_VIDEO                (0x0Eu)     /* Video device */
#define `$INSTANCE_NAME`_CLASS_PHD                  (0x0Fu)     /* Personal Healthcare device */
#define `$INSTANCE_NAME`_CLASS_WIRELESSD            (0xDCu)     /* Wireless Controller */
#define `$INSTANCE_NAME`_CLASS_MIS                  (0xE0u)     /* Miscellaneous */
#define `$INSTANCE_NAME`_CLASS_APP                  (0xEFu)     /* Application Specific */
#define `$INSTANCE_NAME`_CLASS_VENDOR               (0xFFu)     /* Vendor specific */


/* Standard Request Types (Table 9-4) */
#define `$INSTANCE_NAME`_GET_STATUS                 (0x00u)
#define `$INSTANCE_NAME`_CLEAR_FEATURE              (0x01u)
#define `$INSTANCE_NAME`_SET_FEATURE                (0x03u)
#define `$INSTANCE_NAME`_SET_ADDRESS                (0x05u)
#define `$INSTANCE_NAME`_GET_DESCRIPTOR             (0x06u)
#define `$INSTANCE_NAME`_SET_DESCRIPTOR             (0x07u)
#define `$INSTANCE_NAME`_GET_CONFIGURATION          (0x08u)
#define `$INSTANCE_NAME`_SET_CONFIGURATION          (0x09u)
#define `$INSTANCE_NAME`_GET_INTERFACE              (0x0Au)
#define `$INSTANCE_NAME`_SET_INTERFACE              (0x0Bu)
#define `$INSTANCE_NAME`_SYNCH_FRAME                (0x0Cu)

/* Vendor Specific Request Types */
/* Request for Microsoft OS String Descriptor*/
#define `$INSTANCE_NAME`_GET_EXTENDED_CONFIG_DESCRIPTOR (0x01u)

/* Descriptor Types (Table 9-5) */
#define `$INSTANCE_NAME`_DESCR_DEVICE                   (1u)
#define `$INSTANCE_NAME`_DESCR_CONFIG                   (2u)
#define `$INSTANCE_NAME`_DESCR_STRING                   (3u)
#define `$INSTANCE_NAME`_DESCR_INTERFACE                (4u)
#define `$INSTANCE_NAME`_DESCR_ENDPOINT                 (5u)
#define `$INSTANCE_NAME`_DESCR_DEVICE_QUALIFIER         (6u)
#define `$INSTANCE_NAME`_DESCR_OTHER_SPEED              (7u)
#define `$INSTANCE_NAME`_DESCR_INTERFACE_POWER          (8u)

/* Device Descriptor Defines */
#define `$INSTANCE_NAME`_DEVICE_DESCR_LENGTH            (18u)
#define `$INSTANCE_NAME`_DEVICE_DESCR_SN_SHIFT          (16u)

/* Config Descriptor Shifts and Masks */
#define `$INSTANCE_NAME`_CONFIG_DESCR_LENGTH                (0u)
#define `$INSTANCE_NAME`_CONFIG_DESCR_TYPE                  (1u)
#define `$INSTANCE_NAME`_CONFIG_DESCR_TOTAL_LENGTH_LOW      (2u)
#define `$INSTANCE_NAME`_CONFIG_DESCR_TOTAL_LENGTH_HI       (3u)
#define `$INSTANCE_NAME`_CONFIG_DESCR_NUM_INTERFACES        (4u)
#define `$INSTANCE_NAME`_CONFIG_DESCR_CONFIG_VALUE          (5u)
#define `$INSTANCE_NAME`_CONFIG_DESCR_CONFIGURATION         (6u)
#define `$INSTANCE_NAME`_CONFIG_DESCR_ATTRIB                (7u)
#define `$INSTANCE_NAME`_CONFIG_DESCR_ATTRIB_SELF_POWERED   (0x40u)
#define `$INSTANCE_NAME`_CONFIG_DESCR_ATTRIB_RWU_EN         (0x20u)

/* Feature Selectors (Table 9-6) */
#define `$INSTANCE_NAME`_DEVICE_REMOTE_WAKEUP           (0x01u)
#define `$INSTANCE_NAME`_ENDPOINT_HALT                  (0x00u)
#define `$INSTANCE_NAME`_TEST_MODE                      (0x02u)

/* USB Device Status (Figure 9-4) */
#define `$INSTANCE_NAME`_DEVICE_STATUS_BUS_POWERED      (0x00u)
#define `$INSTANCE_NAME`_DEVICE_STATUS_SELF_POWERED     (0x01u)
#define `$INSTANCE_NAME`_DEVICE_STATUS_REMOTE_WAKEUP    (0x02u)

/* USB Endpoint Status (Figure 9-4) */
#define `$INSTANCE_NAME`_ENDPOINT_STATUS_HALT           (0x01u)

/* USB Endpoint Directions */
#define `$INSTANCE_NAME`_DIR_IN                         (0x80u)
#define `$INSTANCE_NAME`_DIR_OUT                        (0x00u)
#define `$INSTANCE_NAME`_DIR_UNUSED                     (0x7Fu)

/* USB Endpoint Attributes */
#define `$INSTANCE_NAME`_EP_TYPE_CTRL                   (0x00u)
#define `$INSTANCE_NAME`_EP_TYPE_ISOC                   (0x01u)
#define `$INSTANCE_NAME`_EP_TYPE_BULK                   (0x02u)
#define `$INSTANCE_NAME`_EP_TYPE_INT                    (0x03u)
#define `$INSTANCE_NAME`_EP_TYPE_MASK                   (0x03u)

#define `$INSTANCE_NAME`_EP_SYNC_TYPE_NO_SYNC           (0x00u)
#define `$INSTANCE_NAME`_EP_SYNC_TYPE_ASYNC             (0x04u)
#define `$INSTANCE_NAME`_EP_SYNC_TYPE_ADAPTIVE          (0x08u)
#define `$INSTANCE_NAME`_EP_SYNC_TYPE_SYNCHRONOUS       (0x0Cu)
#define `$INSTANCE_NAME`_EP_SYNC_TYPE_MASK              (0x0Cu)

#define `$INSTANCE_NAME`_EP_USAGE_TYPE_DATA             (0x00u)
#define `$INSTANCE_NAME`_EP_USAGE_TYPE_FEEDBACK         (0x10u)
#define `$INSTANCE_NAME`_EP_USAGE_TYPE_IMPLICIT         (0x20u)
#define `$INSTANCE_NAME`_EP_USAGE_TYPE_RESERVED         (0x30u)
#define `$INSTANCE_NAME`_EP_USAGE_TYPE_MASK             (0x30u)

/* Endpoint Status defines */
#define `$INSTANCE_NAME`_EP_STATUS_LENGTH               (0x02u)

/* Endpoint Device defines */
#define `$INSTANCE_NAME`_DEVICE_STATUS_LENGTH           (0x02u)

/* Transfer Completion Notification */
#define `$INSTANCE_NAME`_XFER_IDLE                      (0x00u)
#define `$INSTANCE_NAME`_XFER_STATUS_ACK                (0x01u)
#define `$INSTANCE_NAME`_XFER_PREMATURE                 (0x02u)
#define `$INSTANCE_NAME`_XFER_ERROR                     (0x03u)

/* Driver State defines */
#define `$INSTANCE_NAME`_TRANS_STATE_IDLE               (0x00u)
#define `$INSTANCE_NAME`_TRANS_STATE_CONTROL_READ       (0x02u)
#define `$INSTANCE_NAME`_TRANS_STATE_CONTROL_WRITE      (0x04u)
#define `$INSTANCE_NAME`_TRANS_STATE_NO_DATA_CONTROL    (0x06u)

/* String Descriptor defines */
#define `$INSTANCE_NAME`_STRING_MSOS                    (0xEEu)

#if(`$INSTANCE_NAME`_EP_MM == `$INSTANCE_NAME`__EP_DMAMANUAL)
    /* DMA manual mode defines */
    #define `$INSTANCE_NAME`_DMA_BYTES_PER_BURST            (0u)
    #define `$INSTANCE_NAME`_DMA_REQUEST_PER_BURST          (0u)
#endif /* End `$INSTANCE_NAME`_EP_MM == `$INSTANCE_NAME`__EP_DMAMANUAL */
#if(`$INSTANCE_NAME`_EP_MM == `$INSTANCE_NAME`__EP_DMAAUTO)
    /* DMA automatic mode defines */
    #define `$INSTANCE_NAME`_DMA_BYTES_PER_BURST            (32u)
    /* BUF_SIZE-BYTES_PER_BURST examples: 55-32 bytes  44-16 bytes 33-8 bytes 22-4 bytes 11-2 bytes */
    #define `$INSTANCE_NAME`_DMA_BUF_SIZE                   (0x55u)
    #define `$INSTANCE_NAME`_DMA_REQUEST_PER_BURST          (1u)
#endif /* End `$INSTANCE_NAME`_EP_MM == `$INSTANCE_NAME`__EP_DMAAUTO */


/***************************************
*              Registers
***************************************/

#define `$INSTANCE_NAME`_ARB_CFG_PTR        (  (reg8 *) `$INSTANCE_NAME`_USB__ARB_CFG)
#define `$INSTANCE_NAME`_ARB_CFG_REG        (* (reg8 *) `$INSTANCE_NAME`_USB__ARB_CFG)

#define `$INSTANCE_NAME`_ARB_EP1_CFG_PTR    (  (reg8 *) `$INSTANCE_NAME`_USB__ARB_EP1_CFG)
#define `$INSTANCE_NAME`_ARB_EP1_CFG_REG    (* (reg8 *) `$INSTANCE_NAME`_USB__ARB_EP1_CFG)
#define `$INSTANCE_NAME`_ARB_EP1_INT_EN_PTR (  (reg8 *) `$INSTANCE_NAME`_USB__ARB_EP1_INT_EN)
#define `$INSTANCE_NAME`_ARB_EP1_INT_EN_REG (* (reg8 *) `$INSTANCE_NAME`_USB__ARB_EP1_INT_EN)
#define `$INSTANCE_NAME`_ARB_EP1_SR_PTR     (  (reg8 *) `$INSTANCE_NAME`_USB__ARB_EP1_SR)
#define `$INSTANCE_NAME`_ARB_EP1_SR_REG     (* (reg8 *) `$INSTANCE_NAME`_USB__ARB_EP1_SR)

#define `$INSTANCE_NAME`_ARB_EP2_CFG_PTR    (  (reg8 *) `$INSTANCE_NAME`_USB__ARB_EP2_CFG)
#define `$INSTANCE_NAME`_ARB_EP2_CFG_REG    (* (reg8 *) `$INSTANCE_NAME`_USB__ARB_EP2_CFG)
#define `$INSTANCE_NAME`_ARB_EP2_INT_EN_PTR (  (reg8 *) `$INSTANCE_NAME`_USB__ARB_EP2_INT_EN)
#define `$INSTANCE_NAME`_ARB_EP2_INT_EN_REG (* (reg8 *) `$INSTANCE_NAME`_USB__ARB_EP2_INT_EN)
#define `$INSTANCE_NAME`_ARB_EP2_SR_PTR     (  (reg8 *) `$INSTANCE_NAME`_USB__ARB_EP2_SR)
#define `$INSTANCE_NAME`_ARB_EP2_SR_REG     (* (reg8 *) `$INSTANCE_NAME`_USB__ARB_EP2_SR)

#define `$INSTANCE_NAME`_ARB_EP3_CFG_PTR    (  (reg8 *) `$INSTANCE_NAME`_USB__ARB_EP3_CFG)
#define `$INSTANCE_NAME`_ARB_EP3_CFG_REG    (* (reg8 *) `$INSTANCE_NAME`_USB__ARB_EP3_CFG)
#define `$INSTANCE_NAME`_ARB_EP3_INT_EN_PTR (  (reg8 *) `$INSTANCE_NAME`_USB__ARB_EP3_INT_EN)
#define `$INSTANCE_NAME`_ARB_EP3_INT_EN_REG (* (reg8 *) `$INSTANCE_NAME`_USB__ARB_EP3_INT_EN)
#define `$INSTANCE_NAME`_ARB_EP3_SR_PTR     (  (reg8 *) `$INSTANCE_NAME`_USB__ARB_EP3_SR)
#define `$INSTANCE_NAME`_ARB_EP3_SR_REG     (* (reg8 *) `$INSTANCE_NAME`_USB__ARB_EP3_SR)

#define `$INSTANCE_NAME`_ARB_EP4_CFG_PTR    (  (reg8 *) `$INSTANCE_NAME`_USB__ARB_EP4_CFG)
#define `$INSTANCE_NAME`_ARB_EP4_CFG_REG    (* (reg8 *) `$INSTANCE_NAME`_USB__ARB_EP4_CFG)
#define `$INSTANCE_NAME`_ARB_EP4_INT_EN_PTR (  (reg8 *) `$INSTANCE_NAME`_USB__ARB_EP4_INT_EN)
#define `$INSTANCE_NAME`_ARB_EP4_INT_EN_REG (* (reg8 *) `$INSTANCE_NAME`_USB__ARB_EP4_INT_EN)
#define `$INSTANCE_NAME`_ARB_EP4_SR_PTR     (  (reg8 *) `$INSTANCE_NAME`_USB__ARB_EP4_SR)
#define `$INSTANCE_NAME`_ARB_EP4_SR_REG     (* (reg8 *) `$INSTANCE_NAME`_USB__ARB_EP4_SR)

#define `$INSTANCE_NAME`_ARB_EP5_CFG_PTR    (  (reg8 *) `$INSTANCE_NAME`_USB__ARB_EP5_CFG)
#define `$INSTANCE_NAME`_ARB_EP5_CFG_REG    (* (reg8 *) `$INSTANCE_NAME`_USB__ARB_EP5_CFG)
#define `$INSTANCE_NAME`_ARB_EP5_INT_EN_PTR (  (reg8 *) `$INSTANCE_NAME`_USB__ARB_EP5_INT_EN)
#define `$INSTANCE_NAME`_ARB_EP5_INT_EN_REG (* (reg8 *) `$INSTANCE_NAME`_USB__ARB_EP5_INT_EN)
#define `$INSTANCE_NAME`_ARB_EP5_SR_PTR     (  (reg8 *) `$INSTANCE_NAME`_USB__ARB_EP5_SR)
#define `$INSTANCE_NAME`_ARB_EP5_SR_REG     (* (reg8 *) `$INSTANCE_NAME`_USB__ARB_EP5_SR)

#define `$INSTANCE_NAME`_ARB_EP6_CFG_PTR    (  (reg8 *) `$INSTANCE_NAME`_USB__ARB_EP6_CFG)
#define `$INSTANCE_NAME`_ARB_EP6_CFG_REG    (* (reg8 *) `$INSTANCE_NAME`_USB__ARB_EP6_CFG)
#define `$INSTANCE_NAME`_ARB_EP6_INT_EN_PTR (  (reg8 *) `$INSTANCE_NAME`_USB__ARB_EP6_INT_EN)
#define `$INSTANCE_NAME`_ARB_EP6_INT_EN_REG (* (reg8 *) `$INSTANCE_NAME`_USB__ARB_EP6_INT_EN)
#define `$INSTANCE_NAME`_ARB_EP6_SR_PTR     (  (reg8 *) `$INSTANCE_NAME`_USB__ARB_EP6_SR)
#define `$INSTANCE_NAME`_ARB_EP6_SR_REG     (* (reg8 *) `$INSTANCE_NAME`_USB__ARB_EP6_SR)

#define `$INSTANCE_NAME`_ARB_EP7_CFG_PTR    (  (reg8 *) `$INSTANCE_NAME`_USB__ARB_EP7_CFG)
#define `$INSTANCE_NAME`_ARB_EP7_CFG_REG    (* (reg8 *) `$INSTANCE_NAME`_USB__ARB_EP7_CFG)
#define `$INSTANCE_NAME`_ARB_EP7_INT_EN_PTR (  (reg8 *) `$INSTANCE_NAME`_USB__ARB_EP7_INT_EN)
#define `$INSTANCE_NAME`_ARB_EP7_INT_EN_REG (* (reg8 *) `$INSTANCE_NAME`_USB__ARB_EP7_INT_EN)
#define `$INSTANCE_NAME`_ARB_EP7_SR_PTR     (  (reg8 *) `$INSTANCE_NAME`_USB__ARB_EP7_SR)
#define `$INSTANCE_NAME`_ARB_EP7_SR_REG     (* (reg8 *) `$INSTANCE_NAME`_USB__ARB_EP7_SR)

#define `$INSTANCE_NAME`_ARB_EP8_CFG_PTR    (  (reg8 *) `$INSTANCE_NAME`_USB__ARB_EP8_CFG)
#define `$INSTANCE_NAME`_ARB_EP8_CFG_REG    (* (reg8 *) `$INSTANCE_NAME`_USB__ARB_EP8_CFG)
#define `$INSTANCE_NAME`_ARB_EP8_INT_EN_PTR (  (reg8 *) `$INSTANCE_NAME`_USB__ARB_EP8_INT_EN)
#define `$INSTANCE_NAME`_ARB_EP8_INT_EN_REG (* (reg8 *) `$INSTANCE_NAME`_USB__ARB_EP8_INT_EN)
#define `$INSTANCE_NAME`_ARB_EP8_SR_PTR     (  (reg8 *) `$INSTANCE_NAME`_USB__ARB_EP8_SR)
#define `$INSTANCE_NAME`_ARB_EP8_SR_REG     (* (reg8 *) `$INSTANCE_NAME`_USB__ARB_EP8_SR)

#define `$INSTANCE_NAME`_ARB_INT_EN_PTR     (  (reg8 *) `$INSTANCE_NAME`_USB__ARB_INT_EN)
#define `$INSTANCE_NAME`_ARB_INT_EN_REG     (* (reg8 *) `$INSTANCE_NAME`_USB__ARB_INT_EN)
#define `$INSTANCE_NAME`_ARB_INT_SR_PTR     (  (reg8 *) `$INSTANCE_NAME`_USB__ARB_INT_SR)
#define `$INSTANCE_NAME`_ARB_INT_SR_REG     (* (reg8 *) `$INSTANCE_NAME`_USB__ARB_INT_SR)

#define `$INSTANCE_NAME`_ARB_RW1_DR_PTR     ((reg8 *) `$INSTANCE_NAME`_USB__ARB_RW1_DR)
#define `$INSTANCE_NAME`_ARB_RW1_RA_PTR     ((reg8 *) `$INSTANCE_NAME`_USB__ARB_RW1_RA)
#define `$INSTANCE_NAME`_ARB_RW1_RA_MSB_PTR ((reg8 *) `$INSTANCE_NAME`_USB__ARB_RW1_RA_MSB)
#define `$INSTANCE_NAME`_ARB_RW1_WA_PTR     ((reg8 *) `$INSTANCE_NAME`_USB__ARB_RW1_WA)
#define `$INSTANCE_NAME`_ARB_RW1_WA_MSB_PTR ((reg8 *) `$INSTANCE_NAME`_USB__ARB_RW1_WA_MSB)

#define `$INSTANCE_NAME`_ARB_RW2_DR_PTR     ((reg8 *) `$INSTANCE_NAME`_USB__ARB_RW2_DR)
#define `$INSTANCE_NAME`_ARB_RW2_RA_PTR     ((reg8 *) `$INSTANCE_NAME`_USB__ARB_RW2_RA)
#define `$INSTANCE_NAME`_ARB_RW2_RA_MSB_PTR ((reg8 *) `$INSTANCE_NAME`_USB__ARB_RW2_RA_MSB)
#define `$INSTANCE_NAME`_ARB_RW2_WA_PTR     ((reg8 *) `$INSTANCE_NAME`_USB__ARB_RW2_WA)
#define `$INSTANCE_NAME`_ARB_RW2_WA_MSB_PTR ((reg8 *) `$INSTANCE_NAME`_USB__ARB_RW2_WA_MSB)

#define `$INSTANCE_NAME`_ARB_RW3_DR_PTR     ((reg8 *) `$INSTANCE_NAME`_USB__ARB_RW3_DR)
#define `$INSTANCE_NAME`_ARB_RW3_RA_PTR     ((reg8 *) `$INSTANCE_NAME`_USB__ARB_RW3_RA)
#define `$INSTANCE_NAME`_ARB_RW3_RA_MSB_PTR ((reg8 *) `$INSTANCE_NAME`_USB__ARB_RW3_RA_MSB)
#define `$INSTANCE_NAME`_ARB_RW3_WA_PTR     ((reg8 *) `$INSTANCE_NAME`_USB__ARB_RW3_WA)
#define `$INSTANCE_NAME`_ARB_RW3_WA_MSB_PTR ((reg8 *) `$INSTANCE_NAME`_USB__ARB_RW3_WA_MSB)

#define `$INSTANCE_NAME`_ARB_RW4_DR_PTR     ((reg8 *) `$INSTANCE_NAME`_USB__ARB_RW4_DR)
#define `$INSTANCE_NAME`_ARB_RW4_RA_PTR     ((reg8 *) `$INSTANCE_NAME`_USB__ARB_RW4_RA)
#define `$INSTANCE_NAME`_ARB_RW4_RA_MSB_PTR ((reg8 *) `$INSTANCE_NAME`_USB__ARB_RW4_RA_MSB)
#define `$INSTANCE_NAME`_ARB_RW4_WA_PTR     ((reg8 *) `$INSTANCE_NAME`_USB__ARB_RW4_WA)
#define `$INSTANCE_NAME`_ARB_RW4_WA_MSB_PTR ((reg8 *) `$INSTANCE_NAME`_USB__ARB_RW4_WA_MSB)

#define `$INSTANCE_NAME`_ARB_RW5_DR_PTR     ((reg8 *) `$INSTANCE_NAME`_USB__ARB_RW5_DR)
#define `$INSTANCE_NAME`_ARB_RW5_RA_PTR     ((reg8 *) `$INSTANCE_NAME`_USB__ARB_RW5_RA)
#define `$INSTANCE_NAME`_ARB_RW5_RA_MSB_PTR ((reg8 *) `$INSTANCE_NAME`_USB__ARB_RW5_RA_MSB)
#define `$INSTANCE_NAME`_ARB_RW5_WA_PTR     ((reg8 *) `$INSTANCE_NAME`_USB__ARB_RW5_WA)
#define `$INSTANCE_NAME`_ARB_RW5_WA_MSB_PTR ((reg8 *) `$INSTANCE_NAME`_USB__ARB_RW5_WA_MSB)

#define `$INSTANCE_NAME`_ARB_RW6_DR_PTR     ((reg8 *) `$INSTANCE_NAME`_USB__ARB_RW6_DR)
#define `$INSTANCE_NAME`_ARB_RW6_RA_PTR     ((reg8 *) `$INSTANCE_NAME`_USB__ARB_RW6_RA)
#define `$INSTANCE_NAME`_ARB_RW6_RA_MSB_PTR ((reg8 *) `$INSTANCE_NAME`_USB__ARB_RW6_RA_MSB)
#define `$INSTANCE_NAME`_ARB_RW6_WA_PTR     ((reg8 *) `$INSTANCE_NAME`_USB__ARB_RW6_WA)
#define `$INSTANCE_NAME`_ARB_RW6_WA_MSB_PTR ((reg8 *) `$INSTANCE_NAME`_USB__ARB_RW6_WA_MSB)

#define `$INSTANCE_NAME`_ARB_RW7_DR_PTR     ((reg8 *) `$INSTANCE_NAME`_USB__ARB_RW7_DR)
#define `$INSTANCE_NAME`_ARB_RW7_RA_PTR     ((reg8 *) `$INSTANCE_NAME`_USB__ARB_RW7_RA)
#define `$INSTANCE_NAME`_ARB_RW7_RA_MSB_PTR ((reg8 *) `$INSTANCE_NAME`_USB__ARB_RW7_RA_MSB)
#define `$INSTANCE_NAME`_ARB_RW7_WA_PTR     ((reg8 *) `$INSTANCE_NAME`_USB__ARB_RW7_WA)
#define `$INSTANCE_NAME`_ARB_RW7_WA_MSB_PTR ((reg8 *) `$INSTANCE_NAME`_USB__ARB_RW7_WA_MSB)

#define `$INSTANCE_NAME`_ARB_RW8_DR_PTR     ((reg8 *) `$INSTANCE_NAME`_USB__ARB_RW8_DR)
#define `$INSTANCE_NAME`_ARB_RW8_RA_PTR     ((reg8 *) `$INSTANCE_NAME`_USB__ARB_RW8_RA)
#define `$INSTANCE_NAME`_ARB_RW8_RA_MSB_PTR ((reg8 *) `$INSTANCE_NAME`_USB__ARB_RW8_RA_MSB)
#define `$INSTANCE_NAME`_ARB_RW8_WA_PTR     ((reg8 *) `$INSTANCE_NAME`_USB__ARB_RW8_WA)
#define `$INSTANCE_NAME`_ARB_RW8_WA_MSB_PTR ((reg8 *) `$INSTANCE_NAME`_USB__ARB_RW8_WA_MSB)

#define `$INSTANCE_NAME`_BUF_SIZE_PTR       (  (reg8 *) `$INSTANCE_NAME`_USB__BUF_SIZE)
#define `$INSTANCE_NAME`_BUF_SIZE_REG       (* (reg8 *) `$INSTANCE_NAME`_USB__BUF_SIZE)
#define `$INSTANCE_NAME`_BUS_RST_CNT_PTR    (  (reg8 *) `$INSTANCE_NAME`_USB__BUS_RST_CNT)
#define `$INSTANCE_NAME`_BUS_RST_CNT_REG    (* (reg8 *) `$INSTANCE_NAME`_USB__BUS_RST_CNT)
#define `$INSTANCE_NAME`_CWA_PTR            (  (reg8 *) `$INSTANCE_NAME`_USB__CWA)
#define `$INSTANCE_NAME`_CWA_REG            (* (reg8 *) `$INSTANCE_NAME`_USB__CWA)
#define `$INSTANCE_NAME`_CWA_MSB_PTR        (  (reg8 *) `$INSTANCE_NAME`_USB__CWA_MSB)
#define `$INSTANCE_NAME`_CWA_MSB_REG        (* (reg8 *) `$INSTANCE_NAME`_USB__CWA_MSB)
#define `$INSTANCE_NAME`_CR0_PTR            (  (reg8 *) `$INSTANCE_NAME`_USB__CR0)
#define `$INSTANCE_NAME`_CR0_REG            (* (reg8 *) `$INSTANCE_NAME`_USB__CR0)
#define `$INSTANCE_NAME`_CR1_PTR            (  (reg8 *) `$INSTANCE_NAME`_USB__CR1)
#define `$INSTANCE_NAME`_CR1_REG            (* (reg8 *) `$INSTANCE_NAME`_USB__CR1)

#define `$INSTANCE_NAME`_DMA_THRES_PTR      (  (reg8 *) `$INSTANCE_NAME`_USB__DMA_THRES)
#define `$INSTANCE_NAME`_DMA_THRES_REG      (* (reg8 *) `$INSTANCE_NAME`_USB__DMA_THRES)
#define `$INSTANCE_NAME`_DMA_THRES_MSB_PTR  (  (reg8 *) `$INSTANCE_NAME`_USB__DMA_THRES_MSB)
#define `$INSTANCE_NAME`_DMA_THRES_MSB_REG  (* (reg8 *) `$INSTANCE_NAME`_USB__DMA_THRES_MSB)

#define `$INSTANCE_NAME`_EP_ACTIVE_PTR      (  (reg8 *) `$INSTANCE_NAME`_USB__EP_ACTIVE)
#define `$INSTANCE_NAME`_EP_ACTIVE_REG      (* (reg8 *) `$INSTANCE_NAME`_USB__EP_ACTIVE)
#define `$INSTANCE_NAME`_EP_TYPE_PTR        (  (reg8 *) `$INSTANCE_NAME`_USB__EP_TYPE)
#define `$INSTANCE_NAME`_EP_TYPE_REG        (* (reg8 *) `$INSTANCE_NAME`_USB__EP_TYPE)

#define `$INSTANCE_NAME`_EP0_CNT_PTR        (  (reg8 *) `$INSTANCE_NAME`_USB__EP0_CNT)
#define `$INSTANCE_NAME`_EP0_CNT_REG        (* (reg8 *) `$INSTANCE_NAME`_USB__EP0_CNT)
#define `$INSTANCE_NAME`_EP0_CR_PTR         (  (reg8 *) `$INSTANCE_NAME`_USB__EP0_CR)
#define `$INSTANCE_NAME`_EP0_CR_REG         (* (reg8 *) `$INSTANCE_NAME`_USB__EP0_CR)
#define `$INSTANCE_NAME`_EP0_DR0_PTR        (  (reg8 *) `$INSTANCE_NAME`_USB__EP0_DR0)
#define `$INSTANCE_NAME`_EP0_DR0_REG        (* (reg8 *) `$INSTANCE_NAME`_USB__EP0_DR0)
#define `$INSTANCE_NAME`_EP0_DR1_PTR        (  (reg8 *) `$INSTANCE_NAME`_USB__EP0_DR1)
#define `$INSTANCE_NAME`_EP0_DR1_REG        (* (reg8 *) `$INSTANCE_NAME`_USB__EP0_DR1)
#define `$INSTANCE_NAME`_EP0_DR2_PTR        (  (reg8 *) `$INSTANCE_NAME`_USB__EP0_DR2)
#define `$INSTANCE_NAME`_EP0_DR2_REG        (* (reg8 *) `$INSTANCE_NAME`_USB__EP0_DR2)
#define `$INSTANCE_NAME`_EP0_DR3_PTR        (  (reg8 *) `$INSTANCE_NAME`_USB__EP0_DR3)
#define `$INSTANCE_NAME`_EP0_DR3_REG        (* (reg8 *) `$INSTANCE_NAME`_USB__EP0_DR3)
#define `$INSTANCE_NAME`_EP0_DR4_PTR        (  (reg8 *) `$INSTANCE_NAME`_USB__EP0_DR4)
#define `$INSTANCE_NAME`_EP0_DR4_REG        (* (reg8 *) `$INSTANCE_NAME`_USB__EP0_DR4)
#define `$INSTANCE_NAME`_EP0_DR5_PTR        (  (reg8 *) `$INSTANCE_NAME`_USB__EP0_DR5)
#define `$INSTANCE_NAME`_EP0_DR5_REG        (* (reg8 *) `$INSTANCE_NAME`_USB__EP0_DR5)
#define `$INSTANCE_NAME`_EP0_DR6_PTR        (  (reg8 *) `$INSTANCE_NAME`_USB__EP0_DR6)
#define `$INSTANCE_NAME`_EP0_DR6_REG        (* (reg8 *) `$INSTANCE_NAME`_USB__EP0_DR6)
#define `$INSTANCE_NAME`_EP0_DR7_PTR        (  (reg8 *) `$INSTANCE_NAME`_USB__EP0_DR7)
#define `$INSTANCE_NAME`_EP0_DR7_REG        (* (reg8 *) `$INSTANCE_NAME`_USB__EP0_DR7)

#define `$INSTANCE_NAME`_OSCLK_DR0_PTR      (  (reg8 *) `$INSTANCE_NAME`_USB__OSCLK_DR0)
#define `$INSTANCE_NAME`_OSCLK_DR0_REG      (* (reg8 *) `$INSTANCE_NAME`_USB__OSCLK_DR0)
#define `$INSTANCE_NAME`_OSCLK_DR1_PTR      (  (reg8 *) `$INSTANCE_NAME`_USB__OSCLK_DR1)
#define `$INSTANCE_NAME`_OSCLK_DR1_REG      (* (reg8 *) `$INSTANCE_NAME`_USB__OSCLK_DR1)

#define `$INSTANCE_NAME`_PM_ACT_CFG_PTR     (  (reg8 *) `$INSTANCE_NAME`_USB__PM_ACT_CFG)
#define `$INSTANCE_NAME`_PM_ACT_CFG_REG     (* (reg8 *) `$INSTANCE_NAME`_USB__PM_ACT_CFG)
#define `$INSTANCE_NAME`_PM_STBY_CFG_PTR    (  (reg8 *) `$INSTANCE_NAME`_USB__PM_STBY_CFG)
#define `$INSTANCE_NAME`_PM_STBY_CFG_REG    (* (reg8 *) `$INSTANCE_NAME`_USB__PM_STBY_CFG)

#define `$INSTANCE_NAME`_SIE_EP_INT_EN_PTR  (  (reg8 *) `$INSTANCE_NAME`_USB__SIE_EP_INT_EN)
#define `$INSTANCE_NAME`_SIE_EP_INT_EN_REG  (* (reg8 *) `$INSTANCE_NAME`_USB__SIE_EP_INT_EN)
#define `$INSTANCE_NAME`_SIE_EP_INT_SR_PTR  (  (reg8 *) `$INSTANCE_NAME`_USB__SIE_EP_INT_SR)
#define `$INSTANCE_NAME`_SIE_EP_INT_SR_REG  (* (reg8 *) `$INSTANCE_NAME`_USB__SIE_EP_INT_SR)

#define `$INSTANCE_NAME`_SIE_EP1_CNT0_PTR   (  (reg8 *) `$INSTANCE_NAME`_USB__SIE_EP1_CNT0)
#define `$INSTANCE_NAME`_SIE_EP1_CNT0_REG   (* (reg8 *) `$INSTANCE_NAME`_USB__SIE_EP1_CNT0)
#define `$INSTANCE_NAME`_SIE_EP1_CNT1_PTR   (  (reg8 *) `$INSTANCE_NAME`_USB__SIE_EP1_CNT1)
#define `$INSTANCE_NAME`_SIE_EP1_CNT1_REG   (* (reg8 *) `$INSTANCE_NAME`_USB__SIE_EP1_CNT1)
#define `$INSTANCE_NAME`_SIE_EP1_CR0_PTR    (  (reg8 *) `$INSTANCE_NAME`_USB__SIE_EP1_CR0)
#define `$INSTANCE_NAME`_SIE_EP1_CR0_REG    (* (reg8 *) `$INSTANCE_NAME`_USB__SIE_EP1_CR0)

#define `$INSTANCE_NAME`_SIE_EP2_CNT0_PTR   (  (reg8 *) `$INSTANCE_NAME`_USB__SIE_EP2_CNT0)
#define `$INSTANCE_NAME`_SIE_EP2_CNT0_REG   (* (reg8 *) `$INSTANCE_NAME`_USB__SIE_EP2_CNT0)
#define `$INSTANCE_NAME`_SIE_EP2_CNT1_PTR   (  (reg8 *) `$INSTANCE_NAME`_USB__SIE_EP2_CNT1)
#define `$INSTANCE_NAME`_SIE_EP2_CNT1_REG   (* (reg8 *) `$INSTANCE_NAME`_USB__SIE_EP2_CNT1)
#define `$INSTANCE_NAME`_SIE_EP2_CR0_PTR    (  (reg8 *) `$INSTANCE_NAME`_USB__SIE_EP2_CR0)
#define `$INSTANCE_NAME`_SIE_EP2_CR0_REG    (* (reg8 *) `$INSTANCE_NAME`_USB__SIE_EP2_CR0)

#define `$INSTANCE_NAME`_SIE_EP3_CNT0_PTR   (  (reg8 *) `$INSTANCE_NAME`_USB__SIE_EP3_CNT0)
#define `$INSTANCE_NAME`_SIE_EP3_CNT0_REG   (* (reg8 *) `$INSTANCE_NAME`_USB__SIE_EP3_CNT0)
#define `$INSTANCE_NAME`_SIE_EP3_CNT1_PTR   (  (reg8 *) `$INSTANCE_NAME`_USB__SIE_EP3_CNT1)
#define `$INSTANCE_NAME`_SIE_EP3_CNT1_REG   (* (reg8 *) `$INSTANCE_NAME`_USB__SIE_EP3_CNT1)
#define `$INSTANCE_NAME`_SIE_EP3_CR0_PTR    (  (reg8 *) `$INSTANCE_NAME`_USB__SIE_EP3_CR0)
#define `$INSTANCE_NAME`_SIE_EP3_CR0_REG    (* (reg8 *) `$INSTANCE_NAME`_USB__SIE_EP3_CR0)

#define `$INSTANCE_NAME`_SIE_EP4_CNT0_PTR   (  (reg8 *) `$INSTANCE_NAME`_USB__SIE_EP4_CNT0)
#define `$INSTANCE_NAME`_SIE_EP4_CNT0_REG   (* (reg8 *) `$INSTANCE_NAME`_USB__SIE_EP4_CNT0)
#define `$INSTANCE_NAME`_SIE_EP4_CNT1_PTR   (  (reg8 *) `$INSTANCE_NAME`_USB__SIE_EP4_CNT1)
#define `$INSTANCE_NAME`_SIE_EP4_CNT1_REG   (* (reg8 *) `$INSTANCE_NAME`_USB__SIE_EP4_CNT1)
#define `$INSTANCE_NAME`_SIE_EP4_CR0_PTR    (  (reg8 *) `$INSTANCE_NAME`_USB__SIE_EP4_CR0)
#define `$INSTANCE_NAME`_SIE_EP4_CR0_REG    (* (reg8 *) `$INSTANCE_NAME`_USB__SIE_EP4_CR0)

#define `$INSTANCE_NAME`_SIE_EP5_CNT0_PTR   (  (reg8 *) `$INSTANCE_NAME`_USB__SIE_EP5_CNT0)
#define `$INSTANCE_NAME`_SIE_EP5_CNT0_REG   (* (reg8 *) `$INSTANCE_NAME`_USB__SIE_EP5_CNT0)
#define `$INSTANCE_NAME`_SIE_EP5_CNT1_PTR   (  (reg8 *) `$INSTANCE_NAME`_USB__SIE_EP5_CNT1)
#define `$INSTANCE_NAME`_SIE_EP5_CNT1_REG   (* (reg8 *) `$INSTANCE_NAME`_USB__SIE_EP5_CNT1)
#define `$INSTANCE_NAME`_SIE_EP5_CR0_PTR    (  (reg8 *) `$INSTANCE_NAME`_USB__SIE_EP5_CR0)
#define `$INSTANCE_NAME`_SIE_EP5_CR0_REG    (* (reg8 *) `$INSTANCE_NAME`_USB__SIE_EP5_CR0)

#define `$INSTANCE_NAME`_SIE_EP6_CNT0_PTR   (  (reg8 *) `$INSTANCE_NAME`_USB__SIE_EP6_CNT0)
#define `$INSTANCE_NAME`_SIE_EP6_CNT0_REG   (* (reg8 *) `$INSTANCE_NAME`_USB__SIE_EP6_CNT0)
#define `$INSTANCE_NAME`_SIE_EP6_CNT1_PTR   (  (reg8 *) `$INSTANCE_NAME`_USB__SIE_EP6_CNT1)
#define `$INSTANCE_NAME`_SIE_EP6_CNT1_REG   (* (reg8 *) `$INSTANCE_NAME`_USB__SIE_EP6_CNT1)
#define `$INSTANCE_NAME`_SIE_EP6_CR0_PTR    (  (reg8 *) `$INSTANCE_NAME`_USB__SIE_EP6_CR0)
#define `$INSTANCE_NAME`_SIE_EP6_CR0_REG    (* (reg8 *) `$INSTANCE_NAME`_USB__SIE_EP6_CR0)

#define `$INSTANCE_NAME`_SIE_EP7_CNT0_PTR   (  (reg8 *) `$INSTANCE_NAME`_USB__SIE_EP7_CNT0)
#define `$INSTANCE_NAME`_SIE_EP7_CNT0_REG   (* (reg8 *) `$INSTANCE_NAME`_USB__SIE_EP7_CNT0)
#define `$INSTANCE_NAME`_SIE_EP7_CNT1_PTR   (  (reg8 *) `$INSTANCE_NAME`_USB__SIE_EP7_CNT1)
#define `$INSTANCE_NAME`_SIE_EP7_CNT1_REG   (* (reg8 *) `$INSTANCE_NAME`_USB__SIE_EP7_CNT1)
#define `$INSTANCE_NAME`_SIE_EP7_CR0_PTR    (  (reg8 *) `$INSTANCE_NAME`_USB__SIE_EP7_CR0)
#define `$INSTANCE_NAME`_SIE_EP7_CR0_REG    (* (reg8 *) `$INSTANCE_NAME`_USB__SIE_EP7_CR0)

#define `$INSTANCE_NAME`_SIE_EP8_CNT0_PTR   (  (reg8 *) `$INSTANCE_NAME`_USB__SIE_EP8_CNT0)
#define `$INSTANCE_NAME`_SIE_EP8_CNT0_REG   (* (reg8 *) `$INSTANCE_NAME`_USB__SIE_EP8_CNT0)
#define `$INSTANCE_NAME`_SIE_EP8_CNT1_PTR   (  (reg8 *) `$INSTANCE_NAME`_USB__SIE_EP8_CNT1)
#define `$INSTANCE_NAME`_SIE_EP8_CNT1_REG   (* (reg8 *) `$INSTANCE_NAME`_USB__SIE_EP8_CNT1)
#define `$INSTANCE_NAME`_SIE_EP8_CR0_PTR    (  (reg8 *) `$INSTANCE_NAME`_USB__SIE_EP8_CR0)
#define `$INSTANCE_NAME`_SIE_EP8_CR0_REG    (* (reg8 *) `$INSTANCE_NAME`_USB__SIE_EP8_CR0)

#define `$INSTANCE_NAME`_SOF0_PTR           (  (reg8 *) `$INSTANCE_NAME`_USB__SOF0)
#define `$INSTANCE_NAME`_SOF0_REG           (* (reg8 *) `$INSTANCE_NAME`_USB__SOF0)
#define `$INSTANCE_NAME`_SOF1_PTR           (  (reg8 *) `$INSTANCE_NAME`_USB__SOF1)
#define `$INSTANCE_NAME`_SOF1_REG           (* (reg8 *) `$INSTANCE_NAME`_USB__SOF1)

#define `$INSTANCE_NAME`_USB_CLK_EN_PTR     (  (reg8 *) `$INSTANCE_NAME`_USB__USB_CLK_EN)
#define `$INSTANCE_NAME`_USB_CLK_EN_REG     (* (reg8 *) `$INSTANCE_NAME`_USB__USB_CLK_EN)

#define `$INSTANCE_NAME`_USBIO_CR0_PTR      (  (reg8 *) `$INSTANCE_NAME`_USB__USBIO_CR0)
#define `$INSTANCE_NAME`_USBIO_CR0_REG      (* (reg8 *) `$INSTANCE_NAME`_USB__USBIO_CR0)
#define `$INSTANCE_NAME`_USBIO_CR1_PTR      (  (reg8 *) `$INSTANCE_NAME`_USB__USBIO_CR1)
#define `$INSTANCE_NAME`_USBIO_CR1_REG      (* (reg8 *) `$INSTANCE_NAME`_USB__USBIO_CR1)
#if(!CY_PSOC5LP)
    #define `$INSTANCE_NAME`_USBIO_CR2_PTR      (  (reg8 *) `$INSTANCE_NAME`_USB__USBIO_CR2)
    #define `$INSTANCE_NAME`_USBIO_CR2_REG      (* (reg8 *) `$INSTANCE_NAME`_USB__USBIO_CR2)
#endif /* End CY_PSOC5LP */

#define `$INSTANCE_NAME`_DIE_ID             CYDEV_FLSHID_CUST_TABLES_BASE

#if(CY_PSOC5A)
    #define `$INSTANCE_NAME`_PM_AVAIL_CR_PTR    (  (reg8 *) CYREG_PM_AVAIL_CR6)
    #define `$INSTANCE_NAME`_PM_AVAIL_CR_REG    (* (reg8 *) CYREG_PM_AVAIL_CR6)
#else
    #define `$INSTANCE_NAME`_PM_USB_CR0_PTR     (  (reg8 *) CYREG_PM_USB_CR0)
    #define `$INSTANCE_NAME`_PM_USB_CR0_REG     (* (reg8 *) CYREG_PM_USB_CR0)
    #define `$INSTANCE_NAME`_DYN_RECONFIG_PTR   (  (reg8 *) `$INSTANCE_NAME`_USB__DYN_RECONFIG)
    #define `$INSTANCE_NAME`_DYN_RECONFIG_REG   (* (reg8 *) `$INSTANCE_NAME`_USB__DYN_RECONFIG)
#endif /* End CY_PSOC5A */

#define `$INSTANCE_NAME`_DM_INP_DIS_PTR     (  (reg8 *) `$INSTANCE_NAME`_Dm__INP_DIS)
#define `$INSTANCE_NAME`_DM_INP_DIS_REG     (* (reg8 *) `$INSTANCE_NAME`_Dm__INP_DIS)
#define `$INSTANCE_NAME`_DP_INP_DIS_PTR     (  (reg8 *) `$INSTANCE_NAME`_Dp__INP_DIS)
#define `$INSTANCE_NAME`_DP_INP_DIS_REG     (* (reg8 *) `$INSTANCE_NAME`_Dp__INP_DIS)
#define `$INSTANCE_NAME`_DP_INTSTAT_PTR     (  (reg8 *) `$INSTANCE_NAME`_Dp__INTSTAT)
#define `$INSTANCE_NAME`_DP_INTSTAT_REG     (* (reg8 *) `$INSTANCE_NAME`_Dp__INTSTAT)

#if (`$INSTANCE_NAME`_MON_VBUS == 1u)
    #define `$INSTANCE_NAME`_VBUS_DR_PTR        (  (reg8 *) `$INSTANCE_NAME`_VBUS__DR)
    #define `$INSTANCE_NAME`_VBUS_DR_REG        (* (reg8 *) `$INSTANCE_NAME`_VBUS__DR)
    #define `$INSTANCE_NAME`_VBUS_PS_PTR        (  (reg8 *) `$INSTANCE_NAME`_VBUS__PS)
    #define `$INSTANCE_NAME`_VBUS_PS_REG        (* (reg8 *) `$INSTANCE_NAME`_VBUS__PS)
    #define `$INSTANCE_NAME`_VBUS_MASK          `$INSTANCE_NAME`_VBUS__MASK
#endif /* End `$INSTANCE_NAME`_MON_VBUS */

/* Renamed Registers for backward compatibility.
*  Should not be used in new designs.
*/
#define `$INSTANCE_NAME`_ARB_CFG        `$INSTANCE_NAME`_ARB_CFG_PTR

#define `$INSTANCE_NAME`_ARB_EP1_CFG    `$INSTANCE_NAME`_ARB_EP1_CFG_PTR
#define `$INSTANCE_NAME`_ARB_EP1_INT_EN `$INSTANCE_NAME`_ARB_EP1_INT_EN_PTR
#define `$INSTANCE_NAME`_ARB_EP1_SR     `$INSTANCE_NAME`_ARB_EP1_SR_PTR

#define `$INSTANCE_NAME`_ARB_EP2_CFG    `$INSTANCE_NAME`_ARB_EP2_CFG_PTR
#define `$INSTANCE_NAME`_ARB_EP2_INT_EN `$INSTANCE_NAME`_ARB_EP2_INT_EN_PTR
#define `$INSTANCE_NAME`_ARB_EP2_SR     `$INSTANCE_NAME`_ARB_EP2_SR_PTR

#define `$INSTANCE_NAME`_ARB_EP3_CFG    `$INSTANCE_NAME`_ARB_EP3_CFG_PTR
#define `$INSTANCE_NAME`_ARB_EP3_INT_EN `$INSTANCE_NAME`_ARB_EP3_INT_EN_PTR
#define `$INSTANCE_NAME`_ARB_EP3_SR     `$INSTANCE_NAME`_ARB_EP3_SR_PTR

#define `$INSTANCE_NAME`_ARB_EP4_CFG    `$INSTANCE_NAME`_ARB_EP4_CFG_PTR
#define `$INSTANCE_NAME`_ARB_EP4_INT_EN `$INSTANCE_NAME`_ARB_EP4_INT_EN_PTR
#define `$INSTANCE_NAME`_ARB_EP4_SR     `$INSTANCE_NAME`_ARB_EP4_SR_PTR

#define `$INSTANCE_NAME`_ARB_EP5_CFG    `$INSTANCE_NAME`_ARB_EP5_CFG_PTR
#define `$INSTANCE_NAME`_ARB_EP5_INT_EN `$INSTANCE_NAME`_ARB_EP5_INT_EN_PTR
#define `$INSTANCE_NAME`_ARB_EP5_SR     `$INSTANCE_NAME`_ARB_EP5_SR_PTR

#define `$INSTANCE_NAME`_ARB_EP6_CFG    `$INSTANCE_NAME`_ARB_EP6_CFG_PTR
#define `$INSTANCE_NAME`_ARB_EP6_INT_EN `$INSTANCE_NAME`_ARB_EP6_INT_EN_PTR
#define `$INSTANCE_NAME`_ARB_EP6_SR     `$INSTANCE_NAME`_ARB_EP6_SR_PTR

#define `$INSTANCE_NAME`_ARB_EP7_CFG    `$INSTANCE_NAME`_ARB_EP7_CFG_PTR
#define `$INSTANCE_NAME`_ARB_EP7_INT_EN `$INSTANCE_NAME`_ARB_EP7_INT_EN_PTR
#define `$INSTANCE_NAME`_ARB_EP7_SR     `$INSTANCE_NAME`_ARB_EP7_SR_PTR

#define `$INSTANCE_NAME`_ARB_EP8_CFG    `$INSTANCE_NAME`_ARB_EP8_CFG_PTR
#define `$INSTANCE_NAME`_ARB_EP8_INT_EN `$INSTANCE_NAME`_ARB_EP8_INT_EN_PTR
#define `$INSTANCE_NAME`_ARB_EP8_SR     `$INSTANCE_NAME`_ARB_EP8_SR_PTR

#define `$INSTANCE_NAME`_ARB_INT_EN     `$INSTANCE_NAME`_ARB_INT_EN_PTR
#define `$INSTANCE_NAME`_ARB_INT_SR     `$INSTANCE_NAME`_ARB_INT_SR_PTR

#define `$INSTANCE_NAME`_ARB_RW1_DR     `$INSTANCE_NAME`_ARB_RW1_DR_PTR
#define `$INSTANCE_NAME`_ARB_RW1_RA     `$INSTANCE_NAME`_ARB_RW1_RA_PTR
#define `$INSTANCE_NAME`_ARB_RW1_RA_MSB `$INSTANCE_NAME`_ARB_RW1_RA_MSB_PTR
#define `$INSTANCE_NAME`_ARB_RW1_WA     `$INSTANCE_NAME`_ARB_RW1_WA_PTR
#define `$INSTANCE_NAME`_ARB_RW1_WA_MSB `$INSTANCE_NAME`_ARB_RW1_WA_MSB_PTR

#define `$INSTANCE_NAME`_ARB_RW2_DR     `$INSTANCE_NAME`_ARB_RW2_DR_PTR
#define `$INSTANCE_NAME`_ARB_RW2_RA     `$INSTANCE_NAME`_ARB_RW2_RA_PTR
#define `$INSTANCE_NAME`_ARB_RW2_RA_MSB `$INSTANCE_NAME`_ARB_RW2_RA_MSB_PTR
#define `$INSTANCE_NAME`_ARB_RW2_WA     `$INSTANCE_NAME`_ARB_RW2_WA_PTR
#define `$INSTANCE_NAME`_ARB_RW2_WA_MSB `$INSTANCE_NAME`_ARB_RW2_WA_MSB_PTR

#define `$INSTANCE_NAME`_ARB_RW3_DR     `$INSTANCE_NAME`_ARB_RW3_DR_PTR
#define `$INSTANCE_NAME`_ARB_RW3_RA     `$INSTANCE_NAME`_ARB_RW3_RA_PTR
#define `$INSTANCE_NAME`_ARB_RW3_RA_MSB `$INSTANCE_NAME`_ARB_RW3_RA_MSB_PTR
#define `$INSTANCE_NAME`_ARB_RW3_WA     `$INSTANCE_NAME`_ARB_RW3_WA_PTR
#define `$INSTANCE_NAME`_ARB_RW3_WA_MSB `$INSTANCE_NAME`_ARB_RW3_WA_MSB_PTR

#define `$INSTANCE_NAME`_ARB_RW4_DR     `$INSTANCE_NAME`_ARB_RW4_DR_PTR
#define `$INSTANCE_NAME`_ARB_RW4_RA     `$INSTANCE_NAME`_ARB_RW4_RA_PTR
#define `$INSTANCE_NAME`_ARB_RW4_RA_MSB `$INSTANCE_NAME`_ARB_RW4_RA_MSB_PTR
#define `$INSTANCE_NAME`_ARB_RW4_WA     `$INSTANCE_NAME`_ARB_RW4_WA_PTR
#define `$INSTANCE_NAME`_ARB_RW4_WA_MSB `$INSTANCE_NAME`_ARB_RW4_WA_MSB_PTR

#define `$INSTANCE_NAME`_ARB_RW5_DR     `$INSTANCE_NAME`_ARB_RW5_DR_PTR
#define `$INSTANCE_NAME`_ARB_RW5_RA     `$INSTANCE_NAME`_ARB_RW5_RA_PTR
#define `$INSTANCE_NAME`_ARB_RW5_RA_MSB `$INSTANCE_NAME`_ARB_RW5_RA_MSB_PTR
#define `$INSTANCE_NAME`_ARB_RW5_WA     `$INSTANCE_NAME`_ARB_RW5_WA_PTR
#define `$INSTANCE_NAME`_ARB_RW5_WA_MSB `$INSTANCE_NAME`_ARB_RW5_WA_MSB_PTR

#define `$INSTANCE_NAME`_ARB_RW6_DR     `$INSTANCE_NAME`_ARB_RW6_DR_PTR
#define `$INSTANCE_NAME`_ARB_RW6_RA     `$INSTANCE_NAME`_ARB_RW6_RA_PTR
#define `$INSTANCE_NAME`_ARB_RW6_RA_MSB `$INSTANCE_NAME`_ARB_RW6_RA_MSB_PTR
#define `$INSTANCE_NAME`_ARB_RW6_WA     `$INSTANCE_NAME`_ARB_RW6_WA_PTR
#define `$INSTANCE_NAME`_ARB_RW6_WA_MSB `$INSTANCE_NAME`_ARB_RW6_WA_MSB_PTR

#define `$INSTANCE_NAME`_ARB_RW7_DR     `$INSTANCE_NAME`_ARB_RW7_DR_PTR
#define `$INSTANCE_NAME`_ARB_RW7_RA     `$INSTANCE_NAME`_ARB_RW7_RA_PTR
#define `$INSTANCE_NAME`_ARB_RW7_RA_MSB `$INSTANCE_NAME`_ARB_RW7_RA_MSB_PTR
#define `$INSTANCE_NAME`_ARB_RW7_WA     `$INSTANCE_NAME`_ARB_RW7_WA_PTR
#define `$INSTANCE_NAME`_ARB_RW7_WA_MSB `$INSTANCE_NAME`_ARB_RW7_WA_MSB_PTR

#define `$INSTANCE_NAME`_ARB_RW8_DR     `$INSTANCE_NAME`_ARB_RW8_DR_PTR
#define `$INSTANCE_NAME`_ARB_RW8_RA     `$INSTANCE_NAME`_ARB_RW8_RA_PTR
#define `$INSTANCE_NAME`_ARB_RW8_RA_MSB `$INSTANCE_NAME`_ARB_RW8_RA_MSB_PTR
#define `$INSTANCE_NAME`_ARB_RW8_WA     `$INSTANCE_NAME`_ARB_RW8_WA_PTR
#define `$INSTANCE_NAME`_ARB_RW8_WA_MSB `$INSTANCE_NAME`_ARB_RW8_WA_MSB_PTR

#define `$INSTANCE_NAME`_BUF_SIZE       `$INSTANCE_NAME`_BUF_SIZE_PTR
#define `$INSTANCE_NAME`_BUS_RST_CNT    `$INSTANCE_NAME`_BUS_RST_CNT_PTR
#define `$INSTANCE_NAME`_CR0            `$INSTANCE_NAME`_CR0_PTR
#define `$INSTANCE_NAME`_CR1            `$INSTANCE_NAME`_CR1_PTR
#define `$INSTANCE_NAME`_CWA            `$INSTANCE_NAME`_CWA_PTR
#define `$INSTANCE_NAME`_CWA_MSB        `$INSTANCE_NAME`_CWA_MSB_PTR

#define `$INSTANCE_NAME`_DMA_THRES      `$INSTANCE_NAME`_DMA_THRES_PTR
#define `$INSTANCE_NAME`_DMA_THRES_MSB  `$INSTANCE_NAME`_DMA_THRES_MSB_PTR

#define `$INSTANCE_NAME`_EP_ACTIVE      `$INSTANCE_NAME`_EP_ACTIVE_PTR
#define `$INSTANCE_NAME`_EP_TYPE        `$INSTANCE_NAME`_EP_TYPE_PTR

#define `$INSTANCE_NAME`_EP0_CNT        `$INSTANCE_NAME`_EP0_CNT_PTR
#define `$INSTANCE_NAME`_EP0_CR         `$INSTANCE_NAME`_EP0_CR_PTR
#define `$INSTANCE_NAME`_EP0_DR0        `$INSTANCE_NAME`_EP0_DR0_PTR
#define `$INSTANCE_NAME`_EP0_DR1        `$INSTANCE_NAME`_EP0_DR1_PTR
#define `$INSTANCE_NAME`_EP0_DR2        `$INSTANCE_NAME`_EP0_DR2_PTR
#define `$INSTANCE_NAME`_EP0_DR3        `$INSTANCE_NAME`_EP0_DR3_PTR
#define `$INSTANCE_NAME`_EP0_DR4        `$INSTANCE_NAME`_EP0_DR4_PTR
#define `$INSTANCE_NAME`_EP0_DR5        `$INSTANCE_NAME`_EP0_DR5_PTR
#define `$INSTANCE_NAME`_EP0_DR6        `$INSTANCE_NAME`_EP0_DR6_PTR
#define `$INSTANCE_NAME`_EP0_DR7        `$INSTANCE_NAME`_EP0_DR7_PTR

#define `$INSTANCE_NAME`_OSCLK_DR0      `$INSTANCE_NAME`_OSCLK_DR0_PTR
#define `$INSTANCE_NAME`_OSCLK_DR1      `$INSTANCE_NAME`_OSCLK_DR1_PTR

#define `$INSTANCE_NAME`_PM_ACT_CFG     `$INSTANCE_NAME`_PM_ACT_CFG_PTR
#define `$INSTANCE_NAME`_PM_STBY_CFG    `$INSTANCE_NAME`_PM_STBY_CFG_PTR
#if(CY_PSOC5A)
    #define `$INSTANCE_NAME`_PM_AVAIL_CR    `$INSTANCE_NAME`_PM_AVAIL_CR_PTR
#endif /* End CY_PSOC5A */
#define `$INSTANCE_NAME`_SIE_EP_INT_EN  `$INSTANCE_NAME`_SIE_EP_INT_EN_PTR
#define `$INSTANCE_NAME`_SIE_EP_INT_SR  `$INSTANCE_NAME`_SIE_EP_INT_SR_PTR

#define `$INSTANCE_NAME`_SIE_EP1_CNT0   `$INSTANCE_NAME`_SIE_EP1_CNT0_PTR
#define `$INSTANCE_NAME`_SIE_EP1_CNT1   `$INSTANCE_NAME`_SIE_EP1_CNT1_PTR
#define `$INSTANCE_NAME`_SIE_EP1_CR0    `$INSTANCE_NAME`_SIE_EP1_CR0_PTR

#define `$INSTANCE_NAME`_SIE_EP2_CNT0   `$INSTANCE_NAME`_SIE_EP2_CNT0_PTR
#define `$INSTANCE_NAME`_SIE_EP2_CNT1   `$INSTANCE_NAME`_SIE_EP2_CNT1_PTR
#define `$INSTANCE_NAME`_SIE_EP2_CR0    `$INSTANCE_NAME`_SIE_EP2_CR0_PTR

#define `$INSTANCE_NAME`_SIE_EP3_CNT0   `$INSTANCE_NAME`_SIE_EP3_CNT0_PTR
#define `$INSTANCE_NAME`_SIE_EP3_CNT1   `$INSTANCE_NAME`_SIE_EP3_CNT1_PTR
#define `$INSTANCE_NAME`_SIE_EP3_CR0    `$INSTANCE_NAME`_SIE_EP3_CR0_PTR

#define `$INSTANCE_NAME`_SIE_EP4_CNT0   `$INSTANCE_NAME`_SIE_EP4_CNT0_PTR
#define `$INSTANCE_NAME`_SIE_EP4_CNT1   `$INSTANCE_NAME`_SIE_EP4_CNT1_PTR
#define `$INSTANCE_NAME`_SIE_EP4_CR0    `$INSTANCE_NAME`_SIE_EP4_CR0_PTR

#define `$INSTANCE_NAME`_SIE_EP5_CNT0   `$INSTANCE_NAME`_SIE_EP5_CNT0_PTR
#define `$INSTANCE_NAME`_SIE_EP5_CNT1   `$INSTANCE_NAME`_SIE_EP5_CNT1_PTR
#define `$INSTANCE_NAME`_SIE_EP5_CR0    `$INSTANCE_NAME`_SIE_EP5_CR0_PTR

#define `$INSTANCE_NAME`_SIE_EP6_CNT0   `$INSTANCE_NAME`_SIE_EP6_CNT0_PTR
#define `$INSTANCE_NAME`_SIE_EP6_CNT1   `$INSTANCE_NAME`_SIE_EP6_CNT1_PTR
#define `$INSTANCE_NAME`_SIE_EP6_CR0    `$INSTANCE_NAME`_SIE_EP6_CR0_PTR

#define `$INSTANCE_NAME`_SIE_EP7_CNT0   `$INSTANCE_NAME`_SIE_EP7_CNT0_PTR
#define `$INSTANCE_NAME`_SIE_EP7_CNT1   `$INSTANCE_NAME`_SIE_EP7_CNT1_PTR
#define `$INSTANCE_NAME`_SIE_EP7_CR0    `$INSTANCE_NAME`_SIE_EP7_CR0_PTR

#define `$INSTANCE_NAME`_SIE_EP8_CNT0   `$INSTANCE_NAME`_SIE_EP8_CNT0_PTR
#define `$INSTANCE_NAME`_SIE_EP8_CNT1   `$INSTANCE_NAME`_SIE_EP8_CNT1_PTR
#define `$INSTANCE_NAME`_SIE_EP8_CR0    `$INSTANCE_NAME`_SIE_EP8_CR0_PTR

#define `$INSTANCE_NAME`_SOF0           `$INSTANCE_NAME`_SOF0_PTR
#define `$INSTANCE_NAME`_SOF1           `$INSTANCE_NAME`_SOF1_PTR

#define `$INSTANCE_NAME`_USB_CLK_EN     `$INSTANCE_NAME`_USB_CLK_EN_PTR

#define `$INSTANCE_NAME`_USBIO_CR0      `$INSTANCE_NAME`_USBIO_CR0_PTR
#define `$INSTANCE_NAME`_USBIO_CR1      `$INSTANCE_NAME`_USBIO_CR1_PTR
#define `$INSTANCE_NAME`_USBIO_CR2      `$INSTANCE_NAME`_USBIO_CR2_PTR

#define `$INSTANCE_NAME`_USB_MEM        ((reg8 *) CYDEV_USB_MEM_BASE)

#if(CYDEV_CHIP_DIE_EXPECT == CYDEV_CHIP_DIE_LEOPARD)
    /* PSoC3 interrupt registers*/
    #define `$INSTANCE_NAME`_USB_ISR_PRIOR  ((reg8 *) CYDEV_INTC_PRIOR0)
    #define `$INSTANCE_NAME`_USB_ISR_SET_EN ((reg8 *) CYDEV_INTC_SET_EN0)
    #define `$INSTANCE_NAME`_USB_ISR_CLR_EN ((reg8 *) CYDEV_INTC_CLR_EN0)
    #define `$INSTANCE_NAME`_USB_ISR_VECT   ((cyisraddress *) CYDEV_INTC_VECT_MBASE)
#elif(CYDEV_CHIP_DIE_EXPECT == CYDEV_CHIP_DIE_PANTHER)
    /* PSoC5 interrupt registers*/
    #define `$INSTANCE_NAME`_USB_ISR_PRIOR  ((reg8 *) CYDEV_NVIC_PRI_0)
    #define `$INSTANCE_NAME`_USB_ISR_SET_EN ((reg8 *) CYDEV_NVIC_SETENA0)
    #define `$INSTANCE_NAME`_USB_ISR_CLR_EN ((reg8 *) CYDEV_NVIC_CLRENA0)
    #define `$INSTANCE_NAME`_USB_ISR_VECT   ((cyisraddress *) CYDEV_NVIC_VECT_OFFSET)
#endif /* End CYDEV_CHIP_DIE_EXPECT */


/***************************************
* Interrupt vectors, masks and priorities
***************************************/

#define `$INSTANCE_NAME`_BUS_RESET_PRIOR    `$INSTANCE_NAME`_bus_reset__INTC_PRIOR_NUM
#define `$INSTANCE_NAME`_BUS_RESET_MASK     `$INSTANCE_NAME`_bus_reset__INTC_MASK
#define `$INSTANCE_NAME`_BUS_RESET_VECT_NUM `$INSTANCE_NAME`_bus_reset__INTC_NUMBER

#define `$INSTANCE_NAME`_SOF_PRIOR          `$INSTANCE_NAME`_sof_int__INTC_PRIOR_NUM
#define `$INSTANCE_NAME`_SOF_MASK           `$INSTANCE_NAME`_sof_int__INTC_MASK
#define `$INSTANCE_NAME`_SOF_VECT_NUM       `$INSTANCE_NAME`_sof_int__INTC_NUMBER

#define `$INSTANCE_NAME`_EP_0_PRIOR         `$INSTANCE_NAME`_ep_0__INTC_PRIOR_NUM
#define `$INSTANCE_NAME`_EP_0_MASK          `$INSTANCE_NAME`_ep_0__INTC_MASK
#define `$INSTANCE_NAME`_EP_0_VECT_NUM      `$INSTANCE_NAME`_ep_0__INTC_NUMBER

#define `$INSTANCE_NAME`_EP_1_PRIOR         `$INSTANCE_NAME`_ep_1__INTC_PRIOR_NUM
#define `$INSTANCE_NAME`_EP_1_MASK          `$INSTANCE_NAME`_ep_1__INTC_MASK
#define `$INSTANCE_NAME`_EP_1_VECT_NUM      `$INSTANCE_NAME`_ep_1__INTC_NUMBER

#define `$INSTANCE_NAME`_EP_2_PRIOR         `$INSTANCE_NAME`_ep_2__INTC_PRIOR_NUM
#define `$INSTANCE_NAME`_EP_2_MASK          `$INSTANCE_NAME`_ep_2__INTC_MASK
#define `$INSTANCE_NAME`_EP_2_VECT_NUM      `$INSTANCE_NAME`_ep_2__INTC_NUMBER

#define `$INSTANCE_NAME`_EP_3_PRIOR         `$INSTANCE_NAME`_ep_3__INTC_PRIOR_NUM
#define `$INSTANCE_NAME`_EP_3_MASK          `$INSTANCE_NAME`_ep_3__INTC_MASK
#define `$INSTANCE_NAME`_EP_3_VECT_NUM      `$INSTANCE_NAME`_ep_3__INTC_NUMBER

#define `$INSTANCE_NAME`_EP_4_PRIOR         `$INSTANCE_NAME`_ep_4__INTC_PRIOR_NUM
#define `$INSTANCE_NAME`_EP_4_MASK          `$INSTANCE_NAME`_ep_4__INTC_MASK
#define `$INSTANCE_NAME`_EP_4_VECT_NUM      `$INSTANCE_NAME`_ep_4__INTC_NUMBER

#define `$INSTANCE_NAME`_EP_5_PRIOR         `$INSTANCE_NAME`_ep_5__INTC_PRIOR_NUM
#define `$INSTANCE_NAME`_EP_5_MASK          `$INSTANCE_NAME`_ep_5__INTC_MASK
#define `$INSTANCE_NAME`_EP_5_VECT_NUM      `$INSTANCE_NAME`_ep_5__INTC_NUMBER

#define `$INSTANCE_NAME`_EP_6_PRIOR         `$INSTANCE_NAME`_ep_6__INTC_PRIOR_NUM
#define `$INSTANCE_NAME`_EP_6_MASK          `$INSTANCE_NAME`_ep_6__INTC_MASK
#define `$INSTANCE_NAME`_EP_6_VECT_NUM      `$INSTANCE_NAME`_ep_6__INTC_NUMBER

#define `$INSTANCE_NAME`_EP_7_PRIOR         `$INSTANCE_NAME`_ep_7__INTC_PRIOR_NUM
#define `$INSTANCE_NAME`_EP_7_MASK          `$INSTANCE_NAME`_ep_7__INTC_MASK
#define `$INSTANCE_NAME`_EP_7_VECT_NUM      `$INSTANCE_NAME`_ep_7__INTC_NUMBER

#define `$INSTANCE_NAME`_EP_8_PRIOR         `$INSTANCE_NAME`_ep_8__INTC_PRIOR_NUM
#define `$INSTANCE_NAME`_EP_8_MASK          `$INSTANCE_NAME`_ep_8__INTC_MASK
#define `$INSTANCE_NAME`_EP_8_VECT_NUM      `$INSTANCE_NAME`_ep_8__INTC_NUMBER

#define `$INSTANCE_NAME`_DP_INTC_PRIOR      `$INSTANCE_NAME`_dp_int__INTC_PRIOR_NUM
#define `$INSTANCE_NAME`_DP_INTC_MASK       `$INSTANCE_NAME`_dp_int__INTC_MASK
#define `$INSTANCE_NAME`_DP_INTC_VECT_NUM   `$INSTANCE_NAME`_dp_int__INTC_NUMBER

/* ARB ISR should have higher priority from EP_X ISR, therefore it is defined to higest (0) */
#define `$INSTANCE_NAME`_ARB_PRIOR          (0u)
#define `$INSTANCE_NAME`_ARB_MASK           `$INSTANCE_NAME`_arb_int__INTC_MASK
#define `$INSTANCE_NAME`_ARB_VECT_NUM       `$INSTANCE_NAME`_arb_int__INTC_NUMBER

/***************************************
 *  Endpoint 0 offsets (Table 9-2)
 **************************************/

#define `$INSTANCE_NAME`_bmRequestType      `$INSTANCE_NAME`_EP0_DR0_PTR
#define `$INSTANCE_NAME`_bRequest           `$INSTANCE_NAME`_EP0_DR1_PTR
#define `$INSTANCE_NAME`_wValue             `$INSTANCE_NAME`_EP0_DR2_PTR
#define `$INSTANCE_NAME`_wValueHi           `$INSTANCE_NAME`_EP0_DR3_PTR
#define `$INSTANCE_NAME`_wValueLo           `$INSTANCE_NAME`_EP0_DR2_PTR
#define `$INSTANCE_NAME`_wIndex             `$INSTANCE_NAME`_EP0_DR4_PTR
#define `$INSTANCE_NAME`_wIndexHi           `$INSTANCE_NAME`_EP0_DR5_PTR
#define `$INSTANCE_NAME`_wIndexLo           `$INSTANCE_NAME`_EP0_DR4_PTR
#define `$INSTANCE_NAME`_length             `$INSTANCE_NAME`_EP0_DR6_PTR
#define `$INSTANCE_NAME`_lengthHi           `$INSTANCE_NAME`_EP0_DR7_PTR
#define `$INSTANCE_NAME`_lengthLo           `$INSTANCE_NAME`_EP0_DR6_PTR


/***************************************
*       Register Constants
***************************************/
#define `$INSTANCE_NAME`_VDDD_MV                    CYDEV_VDDD_MV
#define `$INSTANCE_NAME`_3500MV                     (3500u)

#define `$INSTANCE_NAME`_CR1_REG_ENABLE             (0x01u)
#define `$INSTANCE_NAME`_CR1_ENABLE_LOCK            (0x02u)
#define `$INSTANCE_NAME`_CR1_BUS_ACTIVITY_SHIFT     (0x02u)
#define `$INSTANCE_NAME`_CR1_BUS_ACTIVITY           (0x01u << `$INSTANCE_NAME`_CR1_BUS_ACTIVITY_SHIFT)
#if(!CY_PSOC5A)
    #define `$INSTANCE_NAME`_CR1_TRIM_MSB_EN            (0x08u)
#endif /* End !CY_PSOC5A */

#define `$INSTANCE_NAME`_EP0_CNT_DATA_TOGGLE        (0x80u)
#define `$INSTANCE_NAME`_EPX_CNT_DATA_TOGGLE        (0x80u)
#define `$INSTANCE_NAME`_EPX_CNT0_MASK              (0x07u)
#define `$INSTANCE_NAME`_EPX_CNTX_ADDR_SHIFT        (0x04u)
#define `$INSTANCE_NAME`_EPX_CNTX_ADDR_OFFSET       (0x10u)
#define `$INSTANCE_NAME`_EPX_CNTX_CRC_COUNT         (0x02u)
#define `$INSTANCE_NAME`_EPX_DATA_BUF_MAX           (512u)

#define `$INSTANCE_NAME`_CR0_ENABLE                 (0x80u)

#if(CY_PSOC5A)
    /* USB_BUS_RST_CNT register 2 bits wide only in PSOC5A, therefore we use max length */
    #define `$INSTANCE_NAME`_BUS_RST_COUNT              (0x03u)
    #define `$INSTANCE_NAME`_USBIO_CR1_IOMODE           (0x80u)
    #define `$INSTANCE_NAME`_USBIO_CR1_DRIVE_MODE       (0x40u)
    #define `$INSTANCE_NAME`_USBIO_CR1_DPI              (0x20u)
    #define `$INSTANCE_NAME`_USBIO_CR1_DMI              (0x10u)
    #define `$INSTANCE_NAME`_USBIO_CR1_P2PUEN           (0x08u)
#else
    /* In leopard, a 100 KHz clock is used. Recommended is to count 10 pulses */
    #define `$INSTANCE_NAME`_BUS_RST_COUNT              (0x0au)
    #define `$INSTANCE_NAME`_USBIO_CR1_IOMODE           (0x20u)
#endif /* End CY_PSOC5A */
#define `$INSTANCE_NAME`_USBIO_CR1_USBPUEN          (0x04u)
#define `$INSTANCE_NAME`_USBIO_CR1_DP0              (0x02u)
#define `$INSTANCE_NAME`_USBIO_CR1_DM0              (0x01u)

#define `$INSTANCE_NAME`_USBIO_CR0_TEN              (0x80u)
#define `$INSTANCE_NAME`_USBIO_CR0_TSE0             (0x40u)
#define `$INSTANCE_NAME`_USBIO_CR0_TD               (0x20u)
#define `$INSTANCE_NAME`_USBIO_CR0_RD               (0x01u)

#define `$INSTANCE_NAME`_FASTCLK_IMO_CR_USBCLK_ON   (0x40u)
#define `$INSTANCE_NAME`_FASTCLK_IMO_CR_XCLKEN      (0x20u)
#define `$INSTANCE_NAME`_FASTCLK_IMO_CR_FX2ON       (0x10u)

#define `$INSTANCE_NAME`_ARB_EPX_CFG_RESET          (0x08u)
#define `$INSTANCE_NAME`_ARB_EPX_CFG_CRC_BYPASS     (0x04u)
#define `$INSTANCE_NAME`_ARB_EPX_CFG_DMA_REQ        (0x02u)
#define `$INSTANCE_NAME`_ARB_EPX_CFG_IN_DATA_RDY    (0x01u)

#define `$INSTANCE_NAME`_ARB_EPX_SR_IN_BUF_FULL     (0x01u)
#define `$INSTANCE_NAME`_ARB_EPX_SR_DMA_GNT         (0x02u)
#define `$INSTANCE_NAME`_ARB_EPX_SR_BUF_OVER        (0x04u)
#define `$INSTANCE_NAME`_ARB_EPX_SR_BUF_UNDER       (0x08u)

#define `$INSTANCE_NAME`_ARB_CFG_AUTO_MEM           (0x10u)
#define `$INSTANCE_NAME`_ARB_CFG_MANUAL_DMA         (0x20u)
#define `$INSTANCE_NAME`_ARB_CFG_AUTO_DMA           (0x40u)
#define `$INSTANCE_NAME`_ARB_CFG_CFG_CPM            (0x80u)

#if(`$INSTANCE_NAME`_EP_MM == `$INSTANCE_NAME`__EP_DMAAUTO)
    #define `$INSTANCE_NAME`_ARB_EPX_INT_MASK           (0x1Du)
#else    
    #define `$INSTANCE_NAME`_ARB_EPX_INT_MASK           (0x1Fu)
#endif /* End `$INSTANCE_NAME`_EP_MM == `$INSTANCE_NAME`__EP_DMAAUTO */
#define `$INSTANCE_NAME`_ARB_INT_MASK              ((`$INSTANCE_NAME`_DMA1_REMOVE ^ 1) | \
                                                    (`$INSTANCE_NAME`_DMA2_REMOVE ^ 1) << 1 | \
                                                    (`$INSTANCE_NAME`_DMA3_REMOVE ^ 1) << 2 | \
                                                    (`$INSTANCE_NAME`_DMA4_REMOVE ^ 1) << 3 | \
                                                    (`$INSTANCE_NAME`_DMA5_REMOVE ^ 1) << 4 | \
                                                    (`$INSTANCE_NAME`_DMA6_REMOVE ^ 1) << 5 | \
                                                    (`$INSTANCE_NAME`_DMA7_REMOVE ^ 1) << 6 | \
                                                    (`$INSTANCE_NAME`_DMA8_REMOVE ^ 1) << 7)

#define `$INSTANCE_NAME`_SIE_EP_INT_EP1_MASK        (0x01u)
#define `$INSTANCE_NAME`_SIE_EP_INT_EP2_MASK        (0x02u)
#define `$INSTANCE_NAME`_SIE_EP_INT_EP3_MASK        (0x04u)
#define `$INSTANCE_NAME`_SIE_EP_INT_EP4_MASK        (0x08u)
#define `$INSTANCE_NAME`_SIE_EP_INT_EP5_MASK        (0x10u)
#define `$INSTANCE_NAME`_SIE_EP_INT_EP6_MASK        (0x20u)
#define `$INSTANCE_NAME`_SIE_EP_INT_EP7_MASK        (0x40u)
#define `$INSTANCE_NAME`_SIE_EP_INT_EP8_MASK        (0x80u)

#define `$INSTANCE_NAME`_PM_ACT_EN_FSUSB            `$INSTANCE_NAME`_USB__PM_ACT_MSK
#define `$INSTANCE_NAME`_PM_STBY_EN_FSUSB           `$INSTANCE_NAME`_USB__PM_STBY_MSK
#define `$INSTANCE_NAME`_PM_AVAIL_EN_FSUSBIO        (0x10u)

#define `$INSTANCE_NAME`_PM_USB_CR0_REF_EN          (0x01u)
#define `$INSTANCE_NAME`_PM_USB_CR0_PD_N            (0x02u)
#define `$INSTANCE_NAME`_PM_USB_CR0_PD_PULLUP_N     (0x04u)

#define `$INSTANCE_NAME`_USB_CLK_ENABLE             (0x01u)

#define `$INSTANCE_NAME`_DM_MASK                    `$INSTANCE_NAME`_Dm__0__MASK
#define `$INSTANCE_NAME`_DP_MASK                    `$INSTANCE_NAME`_Dp__0__MASK

#define `$INSTANCE_NAME`_DYN_RECONFIG_ENABLE        (0x01u)
#define `$INSTANCE_NAME`_DYN_RECONFIG_EP_SHIFT      (0x01u)
#define `$INSTANCE_NAME`_DYN_RECONFIG_RDY_STS       (0x10u)


#endif /* End CY_USBFS_`$INSTANCE_NAME`_H */


/* [] END OF FILE */
