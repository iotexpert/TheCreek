/*******************************************************************************
* File Name: `$INSTANCE_NAME`_midi.h
* Version `$CY_MAJOR_VERSION`.`$CY_MINOR_VERSION`
*
* Description:
*  Header File for the USBFS MIDI module.
*  Contains prototypes and constant values.
*
********************************************************************************
* Copyright 2008-2012, Cypress Semiconductor Corporation.  All rights reserved.
* You may use this file only in accordance with the license, terms, conditions,
* disclaimers, and limitations in the end user license agreement accompanying
* the software package with which this file was provided.
*******************************************************************************/

#if !defined(`$INSTANCE_NAME`_midi_H)
#define `$INSTANCE_NAME`_midi_H

#include "cytypes.h"
#include "`$INSTANCE_NAME`.h"


/***************************************
*    Data Struct Definition
***************************************/

/* The following structure is used to hold status information for
   building and parsing incoming MIDI messages. */
typedef struct _`$INSTANCE_NAME`_MIDI_RX_STATUS
{
    uint8    length;        /* expected length */
    uint8    count;         /* current byte count */
    uint8    size;          /* complete size */
    uint8    runstat;       /* running status */
    uint8    msgBuff[4];    /* message buffer */
} `$INSTANCE_NAME`_MIDI_RX_STATUS;


/***************************************
*           MIDI Constants.
***************************************/

#define `$INSTANCE_NAME`_ONE_EXT_INTRF              (0x01u)
#define `$INSTANCE_NAME`_TWO_EXT_INTRF              (0x02u)

/* Flag definitions for use with MIDI device inquiry */
#define `$INSTANCE_NAME`_INQ_SYSEX_FLAG             (0x01u)
#define `$INSTANCE_NAME`_INQ_IDENTITY_REQ_FLAG      (0x02u)

/* USB-MIDI Code Index Number Classifications (MIDI Table 4-1) */
#define `$INSTANCE_NAME`_CIN_MASK                   (0x0Fu)
#define `$INSTANCE_NAME`_RESERVED0                  (0x00u)
#define `$INSTANCE_NAME`_RESERVED1                  (0x01u)
#define `$INSTANCE_NAME`_2BYTE_COMMON               (0x02u)
#define `$INSTANCE_NAME`_3BYTE_COMMON               (0x03u)
#define `$INSTANCE_NAME`_SYSEX                      (0x04u)
#define `$INSTANCE_NAME`_1BYTE_COMMON               (0x05u)
#define `$INSTANCE_NAME`_SYSEX_ENDS_WITH1           (0x05u)
#define `$INSTANCE_NAME`_SYSEX_ENDS_WITH2           (0x06u)
#define `$INSTANCE_NAME`_SYSEX_ENDS_WITH3           (0x07u)
#define `$INSTANCE_NAME`_NOTE_OFF                   (0x08u)
#define `$INSTANCE_NAME`_NOTE_ON                    (0x09u)
#define `$INSTANCE_NAME`_POLY_KEY_PRESSURE          (0x0Au)
#define `$INSTANCE_NAME`_CONTROL_CHANGE             (0x0Bu)
#define `$INSTANCE_NAME`_PROGRAM_CHANGE             (0x0Cu)
#define `$INSTANCE_NAME`_CHANNEL_PRESSURE           (0x0Du)
#define `$INSTANCE_NAME`_PITCH_BEND_CHANGE          (0x0Eu)
#define `$INSTANCE_NAME`_SINGLE_BYTE                (0x0Fu)

#define `$INSTANCE_NAME`_CABLE_MASK                 (0xF0u)
#define `$INSTANCE_NAME`_MIDI_CABLE_00              (0x00u)
#define `$INSTANCE_NAME`_MIDI_CABLE_01              (0x10u)

#define `$INSTANCE_NAME`_EVENT_BYTE0                (0x00u)
#define `$INSTANCE_NAME`_EVENT_BYTE1                (0x01u)
#define `$INSTANCE_NAME`_EVENT_BYTE2                (0x02u)
#define `$INSTANCE_NAME`_EVENT_BYTE3                (0x03u)
#define `$INSTANCE_NAME`_EVENT_LENGTH               (0x04u)

#define `$INSTANCE_NAME`_MIDI_STATUS_BYTE_MASK      (0x80u)
#define `$INSTANCE_NAME`_MIDI_STATUS_MASK           (0xF0u)
#define `$INSTANCE_NAME`_MIDI_SINGLE_BYTE_MASK      (0x08u)
#define `$INSTANCE_NAME`_MIDI_NOTE_OFF              (0x80u)
#define `$INSTANCE_NAME`_MIDI_NOTE_ON               (0x90u)
#define `$INSTANCE_NAME`_MIDI_POLY_KEY_PRESSURE     (0xA0u)
#define `$INSTANCE_NAME`_MIDI_CONTROL_CHANGE        (0xB0u)
#define `$INSTANCE_NAME`_MIDI_PROGRAM_CHANGE        (0xC0u)
#define `$INSTANCE_NAME`_MIDI_CHANNEL_PRESSURE      (0xD0u)
#define `$INSTANCE_NAME`_MIDI_PITCH_BEND_CHANGE     (0xE0u)
#define `$INSTANCE_NAME`_MIDI_SYSEX                 (0xF0u)
#define `$INSTANCE_NAME`_MIDI_EOSEX                 (0xF7u)
#define `$INSTANCE_NAME`_MIDI_QFM                   (0xF1u)
#define `$INSTANCE_NAME`_MIDI_SPP                   (0xF2u)
#define `$INSTANCE_NAME`_MIDI_SONGSEL               (0xF3u)
#define `$INSTANCE_NAME`_MIDI_TUNEREQ               (0xF6u)
#define `$INSTANCE_NAME`_MIDI_ACTIVESENSE           (0xFEu)

/* MIDI Universal System Exclusive defines */
#define `$INSTANCE_NAME`_MIDI_SYSEX_NON_REAL_TIME   (0x7Eu)
#define `$INSTANCE_NAME`_MIDI_SYSEX_REALTIME        (0x7Fu)
/* ID of target device */
#define `$INSTANCE_NAME`_MIDI_SYSEX_ID_ALL          (0x7Fu)
/* Sub-ID#1*/
#define `$INSTANCE_NAME`_MIDI_SYSEX_GEN_INFORMATION (0x06u)
#define `$INSTANCE_NAME`_MIDI_SYSEX_GEN_MESSAGE     (0x09u)
/* Sub-ID#2*/
#define `$INSTANCE_NAME`_MIDI_SYSEX_IDENTITY_REQ    (0x01u)
#define `$INSTANCE_NAME`_MIDI_SYSEX_IDENTITY_REPLY  (0x02u)
#define `$INSTANCE_NAME`_MIDI_SYSEX_SYSTEM_ON       (0x01u)
#define `$INSTANCE_NAME`_MIDI_SYSEX_SYSTEM_OFF      (0x02u)

#define `$INSTANCE_NAME`_CUSTOM_UART_TX_PRIOR_NUM   (0x04u)
#define `$INSTANCE_NAME`_CUSTOM_UART_RX_PRIOR_NUM   (0x02u)

#endif /* End `$INSTANCE_NAME`_midi_H */


/* [] END OF FILE */
