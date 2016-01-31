/*******************************************************************************
* File Name: `$INSTANCE_NAME`_midi.c
* Version `$CY_MAJOR_VERSION`.`$CY_MINOR_VERSION`
*
* Description:
*  MIDI Streaming request handler.
*  This file contains routines for sending and receiving MIDI
*  messages, and handles running status in both directions.
*
********************************************************************************
* Copyright 2008-2012, Cypress Semiconductor Corporation.  All rights reserved.
* You may use this file only in accordance with the license, terms, conditions,
* disclaimers, and limitations in the end user license agreement accompanying
* the software package with which this file was provided.
*******************************************************************************/

#include "`$INSTANCE_NAME`.h"

#if defined(`$INSTANCE_NAME`_ENABLE_MIDI_STREAMING)

#include "`$INSTANCE_NAME`_midi.h"
#if (`$INSTANCE_NAME`_MIDI_EXT_MODE >= `$INSTANCE_NAME`_ONE_EXT_INTRF)
    #include "MIDI1_UART.h"
#endif /* End `$INSTANCE_NAME`_MIDI_EXT_MODE >= `$INSTANCE_NAME`_ONE_EXT_INTRF */    
#if (`$INSTANCE_NAME`_MIDI_EXT_MODE >= `$INSTANCE_NAME`_TWO_EXT_INTRF)
    #include "MIDI2_UART.h"
#endif /* End `$INSTANCE_NAME`_MIDI_EXT_MODE >= `$INSTANCE_NAME`_TWO_EXT_INTRF */    
#if(`$INSTANCE_NAME`_EP_MM != `$INSTANCE_NAME`__EP_MANUAL)
    #include <CYDMAC.H>
#endif /* End `$INSTANCE_NAME`_EP_MM */


/***************************************
*    MIDI Constants
***************************************/

/* The Size of the MIDI messages (MIDI Table 4-1) */
const uint8 CYCODE `$INSTANCE_NAME`_MIDI_SIZE[] = {
/*  Miscellaneous function codes(Reserved)  */ 0x03u,
/*  Cable events (Reserved)                 */ 0x03u,
/*  Two-byte System Common messages         */ 0x02u,
/*  Three-byte System Common messages       */ 0x03u,
/*  SysEx starts or continues               */ 0x03u,
/*  Single-byte System Common Message or
    SysEx ends with following single byte   */ 0x01u,
/*  SysEx ends with following two bytes     */ 0x02u,
/*  SysEx ends with following three bytes   */ 0x03u,
/*  Note-off                                */ 0x03u,
/*  Note-on                                 */ 0x03u,
/*  Poly-KeyPress                           */ 0x03u,
/*  Control Change                          */ 0x03u,
/*  Program Change                          */ 0x02u,
/*  Channel Pressure                        */ 0x02u,
/*  PitchBend Change                        */ 0x03u,
/*  Single Byte                             */ 0x01u
};


/***************************************
*  Global variables
***************************************/

volatile uint8 `$INSTANCE_NAME`_midiInBuffer[`$INSTANCE_NAME`_MIDI_IN_BUFF_SIZE];   /* Input endpoint buffer */
volatile uint8 `$INSTANCE_NAME`_midiOutBuffer[`$INSTANCE_NAME`_MIDI_OUT_BUFF_SIZE]; /* Output endpoint buffer */
#if `$INSTANCE_NAME`_MIDI_IN_BUFF_SIZE >= 256
    volatile uint16 `$INSTANCE_NAME`_midiInPointer;                                 /* Input endpoint buffer pointer */
#else
    volatile uint8 `$INSTANCE_NAME`_midiInPointer;                                  /* Input endpoint buffer pointer */
#endif /* End `$INSTANCE_NAME`_MIDI_IN_BUFF_SIZE >=256 */    

volatile uint8 `$INSTANCE_NAME`_midi_in_ep;                                         /* Input endpoint number */
volatile uint8 `$INSTANCE_NAME`_midi_out_ep;                                        /* Output endpoint number */

#if (`$INSTANCE_NAME`_MIDI_EXT_MODE >= `$INSTANCE_NAME`_ONE_EXT_INTRF)
    volatile `$INSTANCE_NAME`_MIDI_RX_STATUS `$INSTANCE_NAME`_MIDI1_Event;          /* MIDI RX status structure */
    volatile uint8 `$INSTANCE_NAME`_MIDI1_TxRunStat;                                /* MIDI Output running status */
    volatile uint8 `$INSTANCE_NAME`_MIDI1_InqFlags;                                 /* Device inquiry flag */    
    
#if (`$INSTANCE_NAME`_MIDI_EXT_MODE >= `$INSTANCE_NAME`_TWO_EXT_INTRF)
    volatile `$INSTANCE_NAME`_MIDI_RX_STATUS `$INSTANCE_NAME`_MIDI2_Event;          /* MIDI RX status structure */
    volatile uint8 `$INSTANCE_NAME`_MIDI2_TxRunStat;                                /* MIDI Output running status */
    volatile uint8 `$INSTANCE_NAME`_MIDI2_InqFlags;                                 /* Device inquiry flag */    
#endif /* End `$INSTANCE_NAME`_MIDI_EXT_MODE >= `$INSTANCE_NAME`_TWO_EXT_INTRF */
#endif /* End `$INSTANCE_NAME`_MIDI_EXT_MODE >= `$INSTANCE_NAME`_ONE_EXT_INTRF */


/***************************************
* Custom Declarations
***************************************/

/* `#START MIDI_CUSTOM_DECLARATIONS` Place your declaration here */

/* `#END` */


/***************************************
* Optional MIDI APIs
***************************************/
#if (`$INSTANCE_NAME`_ENABLE_MIDI_API != 0u)


/***************************************
* External function references
***************************************/

void `$INSTANCE_NAME`_callbackLocalMidiEvent(uint8 cable, uint8 *midiMsg) 
                                                     `=ReentrantKeil($INSTANCE_NAME . "_callbackLocalMidiEvent")`;


/***************************************
*    External references
***************************************/

extern volatile T_`$INSTANCE_NAME`_EP_CTL_BLOCK `$INSTANCE_NAME`_EP[];
#if(`$INSTANCE_NAME`_EP_MM != `$INSTANCE_NAME`__EP_MANUAL)
    extern uint8 `$INSTANCE_NAME`_DmaChan[`$INSTANCE_NAME`_MAX_EP];
#endif /* End `$INSTANCE_NAME`_EP_MM */

#if (`$INSTANCE_NAME`_MIDI_EXT_MODE >= `$INSTANCE_NAME`_ONE_EXT_INTRF)
    #if (MIDI1_UART_RXBUFFERSIZE >= 256)
        extern volatile uint16 MIDI1_UART_rxBufferRead;
        extern volatile uint16 MIDI1_UART_rxBufferWrite;
    #else    
        extern volatile uint8 MIDI1_UART_rxBufferRead;
        extern volatile uint8 MIDI1_UART_rxBufferWrite;
    #endif /* End MIDI1_UART_RXBUFFERSIZE >= 256 */    
    extern volatile uint8 MIDI1_UART_rxBufferLoopDetect;
    extern volatile uint8 MIDI1_UART_rxBuffer[MIDI1_UART_RXBUFFERSIZE];
    
#endif /* End `$INSTANCE_NAME`_MIDI_EXT_MODE >= `$INSTANCE_NAME`_ONE_EXT_INTRF */    
#if (`$INSTANCE_NAME`_MIDI_EXT_MODE >= `$INSTANCE_NAME`_TWO_EXT_INTRF)
    #if (MIDI2_UART_RXBUFFERSIZE >= 256)
        extern volatile uint16 MIDI2_UART_rxBufferRead;
        extern volatile uint16 MIDI2_UART_rxBufferWrite;
    #else    
        extern volatile uint8 MIDI2_UART_rxBufferRead;
        extern volatile uint8 MIDI2_UART_rxBufferWrite;
    #endif /* End MIDI2_UART_RXBUFFERSIZE >= 256 */    
    extern volatile uint8 MIDI2_UART_rxBufferLoopDetect;
    extern volatile uint8 MIDI2_UART_rxBuffer[MIDI2_UART_RXBUFFERSIZE];
#endif /* End `$INSTANCE_NAME`_MIDI_EXT_MODE >= `$INSTANCE_NAME`_TWO_EXT_INTRF */    



/***************************************
*    Private function prototypes
***************************************/

void `$INSTANCE_NAME`_PrepareInBuffer(uint8 ic, uint8 *srcBuff, uint8 eventLen, uint8 cable)
                                                                 `=ReentrantKeil($INSTANCE_NAME . "_PrepareInBuffer")`;
#if (`$INSTANCE_NAME`_MIDI_EXT_MODE >= `$INSTANCE_NAME`_ONE_EXT_INTRF)
    void `$INSTANCE_NAME`_MIDI_Init(void) `=ReentrantKeil($INSTANCE_NAME . "_MIDI_Init")`;

    uint8 `$INSTANCE_NAME`_MIDI1_GetEvent(void) `=ReentrantKeil($INSTANCE_NAME . "_MIDI1_GetEvent")`;
    void `$INSTANCE_NAME`_MIDI1_ProcessUsbOut(uint8 *epBuf) `=ReentrantKeil($INSTANCE_NAME . "_MIDI1_ProcessUsbOut")`;
    
    #if (`$INSTANCE_NAME`_MIDI_EXT_MODE >= `$INSTANCE_NAME`_TWO_EXT_INTRF)
        uint8 `$INSTANCE_NAME`_MIDI2_GetEvent(void) `=ReentrantKeil($INSTANCE_NAME . "_MIDI2_GetEvent")`;
        void `$INSTANCE_NAME`_MIDI2_ProcessUsbOut(uint8 *epBuf) 
                                                    `=ReentrantKeil($INSTANCE_NAME . "_MIDI2_ProcessUsbOut")`;
    #endif /* End `$INSTANCE_NAME`_MIDI_EXT_MODE >= `$INSTANCE_NAME`_TWO_EXT_INTRF */
#endif /* End `$INSTANCE_NAME`_MIDI_EXT_MODE >= `$INSTANCE_NAME`_ONE_EXT_INTRF */


/*******************************************************************************
* Function Name: `$INSTANCE_NAME`_MIDI_EP_Init
********************************************************************************
*
* Summary:
*  This function initializes the MIDI interface and UART(s) to be ready to 
*   receive data from the PC and MIDI ports.
*
* Parameters:
*  None
*
* Return:
*  None
*
* Global variables:
*  `$INSTANCE_NAME`_midiInBuffer: This buffer is used for saving and combining 
*    the received data from UART(s) and(or) generated internally by 
*    PutUsbMidiIn() function messages. `$INSTANCE_NAME`_MIDI_IN_EP_Service() 
*    function transfers the data from this buffer to the PC.
*  `$INSTANCE_NAME`_midiOutBuffer: This buffer is used by the
*    `$INSTANCE_NAME`_MIDI_OUT_EP_Service() function for saving the received
*    from the PC data, then the data are parsed and transferred to UART(s) 
*    buffer and to the internal processing by the
*    `$INSTANCE_NAME`_callbackLocalMidiEvent function.
*  `$INSTANCE_NAME`_midi_out_ep: Used as an OUT endpoint number.
*  `$INSTANCE_NAME`_midi_in_ep: Used as an IN endpoint number.
*   `$INSTANCE_NAME`_midiInPointer: Initialized to zero.
*
* Reentrant:
*  No
*
*******************************************************************************/
void `$INSTANCE_NAME`_MIDI_EP_Init(void) `=ReentrantKeil($INSTANCE_NAME . "_MIDI_EP_Init")`
{
    `$INSTANCE_NAME`_midiInPointer = 0u;
    
    #if(`$INSTANCE_NAME`_EP_MM == `$INSTANCE_NAME`__EP_DMAAUTO)
        /* Init DMA configurations for IN EP*/
        `$INSTANCE_NAME`_LoadInEP(`$INSTANCE_NAME`_midi_in_ep, (uint8 *)`$INSTANCE_NAME`_midiInBuffer,
                                                                                `$INSTANCE_NAME`_MIDI_IN_BUFF_SIZE);
        /* Init DMA configurations for OUT EP*/
        `$INSTANCE_NAME`_ReadOutEP(`$INSTANCE_NAME`_midi_out_ep, (uint8 *)`$INSTANCE_NAME`_midiOutBuffer, 
                                                                                `$INSTANCE_NAME`_MIDI_OUT_BUFF_SIZE);
    #endif  /* End `$INSTANCE_NAME`__EP_DMAAUTO */  
    
    `$INSTANCE_NAME`_EnableOutEP(`$INSTANCE_NAME`_midi_out_ep);
    
    /* Initialize the MIDI port(s) */
    #if (`$INSTANCE_NAME`_MIDI_EXT_MODE >= `$INSTANCE_NAME`_ONE_EXT_INTRF)
        `$INSTANCE_NAME`_MIDI_Init();
    #endif /* `$INSTANCE_NAME`_MIDI_EXT_MODE >= `$INSTANCE_NAME`_ONE_EXT_INTRF */    
}


/*******************************************************************************
* Function Name: `$INSTANCE_NAME`_MIDI_OUT_EP_Service
********************************************************************************
*
* Summary: 
*  Services the USB MIDI OUT endpoints.
*  This function is called from OUT EP ISR. It transfers the received from PC
*  data to the external MIDI port(UART TX buffer) and calls the 
*  `$INSTANCE_NAME`_callbackLocalMidiEvent() function to internal process
*  of the MIDI data.
*  This function is blocked by UART, if not enough space is available in UART 
*  TX buffer. Therefore it is recommended to use large UART TX buffer size.
*
* Parameters:
*  None
*
* Return:
*  None
*
* Global variables:
*  `$INSTANCE_NAME`_midiOutBuffer: Used as temporary buffer between USB internal 
*   memory and UART TX buffer.
*  `$INSTANCE_NAME`_midi_out_ep: Used as an OUT endpoint number.
*
* Reentrant:
*  No
*
*******************************************************************************/
void `$INSTANCE_NAME`_MIDI_OUT_EP_Service(void) `=ReentrantKeil($INSTANCE_NAME . "_MIDI_OUT_EP_Service")`
{
    #if `$INSTANCE_NAME`_MIDI_OUT_BUFF_SIZE >= 256
        uint16 outLength;
        uint16 outPointer;
    #else
        uint8 outLength;
        uint8 outPointer;
    #endif /* End `$INSTANCE_NAME`_MIDI_OUT_BUFF_SIZE >=256 */    
    
    uint8 dmaState = 0u;

    /* Service the USB MIDI output endpoint */
    if (`$INSTANCE_NAME`_GetEPState(`$INSTANCE_NAME`_midi_out_ep) == `$INSTANCE_NAME`_OUT_BUFFER_FULL)
    {
        outLength = `$INSTANCE_NAME`_GetEPCount(`$INSTANCE_NAME`_midi_out_ep);
        #if(`$INSTANCE_NAME`_EP_MM != `$INSTANCE_NAME`__EP_DMAAUTO)
            outLength = `$INSTANCE_NAME`_ReadOutEP(`$INSTANCE_NAME`_midi_out_ep, 
                                                   (uint8 *)`$INSTANCE_NAME`_midiOutBuffer, outLength);
            #if(`$INSTANCE_NAME`_EP_MM == `$INSTANCE_NAME`__EP_DMAMANUAL)
                do  /* wait for DMA transfer complete */
                {
                    CyDmaChStatus(`$INSTANCE_NAME`_DmaChan[`$INSTANCE_NAME`_midi_out_ep], NULL, &dmaState);
                }while((dmaState & (STATUS_TD_ACTIVE | STATUS_CHAIN_ACTIVE)) != 0);
            #endif /* End USBFS_EP_MM == USBFS__EP_DMAMANUAL */
        #endif  /* End `$INSTANCE_NAME`_EP_MM != `$INSTANCE_NAME`__EP_DMAAUTO */  
        dmaState = dmaState;    /* remove warning */
        
        if (outLength >= `$INSTANCE_NAME`_EVENT_LENGTH)
        {
            outPointer = 0u;
            while (outPointer < outLength)
            {
                /* In some OS OUT packet could be appended by nulls which could be skipped */
                if (`$INSTANCE_NAME`_midiOutBuffer[outPointer] == 0u)
                {
                    break;
                }
                /* Route USB MIDI to the External connection */
                #if (`$INSTANCE_NAME`_MIDI_EXT_MODE >= `$INSTANCE_NAME`_ONE_EXT_INTRF)
                    if ((`$INSTANCE_NAME`_midiOutBuffer[outPointer] & `$INSTANCE_NAME`_CABLE_MASK) == 
                        `$INSTANCE_NAME`_MIDI_CABLE_00)
                    {
                        `$INSTANCE_NAME`_MIDI1_ProcessUsbOut((uint8 *)&`$INSTANCE_NAME`_midiOutBuffer[outPointer]);
                    }
                    else if ((`$INSTANCE_NAME`_midiOutBuffer[outPointer] & `$INSTANCE_NAME`_CABLE_MASK) == 
                             `$INSTANCE_NAME`_MIDI_CABLE_01)
                    {
                        #if (`$INSTANCE_NAME`_MIDI_EXT_MODE >= `$INSTANCE_NAME`_TWO_EXT_INTRF)
                            `$INSTANCE_NAME`_MIDI2_ProcessUsbOut((uint8 *)&`$INSTANCE_NAME`_midiOutBuffer[outPointer]);
                        #endif /* End `$INSTANCE_NAME`_MIDI_EXT_MODE >= `$INSTANCE_NAME`_TWO_EXT_INTRF */
                    }
                    else
                    {
                        /* `#START CUSTOM_MIDI_OUT_EP_SERV` Place your code here */

                        /* `#END` */
                    }
                #endif /* End `$INSTANCE_NAME`_MIDI_EXT_MODE >= `$INSTANCE_NAME`_ONE_EXT_INTRF */

                /* Process any local MIDI output functions */
                `$INSTANCE_NAME`_callbackLocalMidiEvent(
                    `$INSTANCE_NAME`_midiOutBuffer[outPointer] & `$INSTANCE_NAME`_CABLE_MASK,
                    (uint8 *)&`$INSTANCE_NAME`_midiOutBuffer[outPointer + `$INSTANCE_NAME`_EVENT_BYTE1]);
                outPointer += `$INSTANCE_NAME`_EVENT_LENGTH;
            }
        }
        #if(`$INSTANCE_NAME`_EP_MM == `$INSTANCE_NAME`__EP_DMAAUTO)
            /* Enable Out EP*/
            `$INSTANCE_NAME`_EnableOutEP(`$INSTANCE_NAME`_midi_out_ep);   
        #endif  /* End `$INSTANCE_NAME`_EP_MM == `$INSTANCE_NAME`__EP_DMAAUTO */  
    }
}


/*******************************************************************************
* Function Name: `$INSTANCE_NAME`_MIDI_IN_EP_Service
********************************************************************************
*
* Summary: 
*  Services the USB MIDI IN endpoint. Non-blocking. 
*  Checks that previous packet was processed by HOST, otherwise service the 
*  input endpoint on the subsequent call. It is called from the 
*  `$INSTANCE_NAME`_MIDI_IN_Service() and from the 
*  `$INSTANCE_NAME`_PutUsbMidiIn() function.
*
* Parameters:
*  None
*
* Return:
*  None
*
* Global variables:
*  `$INSTANCE_NAME`_midi_in_ep: Used as an IN endpoint number.
*  `$INSTANCE_NAME`_midiInBuffer: Function loads the data from this buffer to 
*    the USB IN endpoint.
*   `$INSTANCE_NAME`_midiInPointer: Cleared to zero when data are sent.
*
* Reentrant:
*  No
*
*******************************************************************************/
void `$INSTANCE_NAME`_MIDI_IN_EP_Service(void) `=ReentrantKeil($INSTANCE_NAME . "_MIDI_IN_EP_Service")`
{
    /* Service the USB MIDI input endpoint */
    /* Check that previous packet was processed by HOST, otherwise service the USB later */
    if ((`$INSTANCE_NAME`_midiInPointer != 0u) && 
        (`$INSTANCE_NAME`_GetEPState(`$INSTANCE_NAME`_midi_in_ep) == `$INSTANCE_NAME`_EVENT_PENDING))
    {
        #if(`$INSTANCE_NAME`_EP_MM != `$INSTANCE_NAME`__EP_DMAAUTO)
            `$INSTANCE_NAME`_LoadInEP(`$INSTANCE_NAME`_midi_in_ep, (uint8 *)`$INSTANCE_NAME`_midiInBuffer,
                                                                                `$INSTANCE_NAME`_midiInPointer);
        #else /* `$INSTANCE_NAME`_EP_MM != `$INSTANCE_NAME`__EP_DMAAUTO */                  
            /* rearm IN EP */
            `$INSTANCE_NAME`_LoadInEP(`$INSTANCE_NAME`_midi_in_ep, NULL, `$INSTANCE_NAME`_midiInPointer);
        #endif /* End `$INSTANCE_NAME`_EP_MM != `$INSTANCE_NAME`__EP_DMAAUTO*/
        
        /* Clear the midiInPointer. For DMA mode, clear this pointer in the ARB ISR when data are moved by DMA */
        #if(`$INSTANCE_NAME`_EP_MM == `$INSTANCE_NAME`__EP_MANUAL)
            `$INSTANCE_NAME`_midiInPointer = 0u;
        #endif /* `$INSTANCE_NAME`_EP_MM == `$INSTANCE_NAME`__EP_MANUAL */                  
    }
}


/*******************************************************************************
* Function Name: `$INSTANCE_NAME`_MIDI_IN_Service
********************************************************************************
*
* Summary: 
*  Services the traffic from the MIDI input ports (RX UART) and prepare data 
*  in USB MIDI IN endpoint buffer. 
*  Calls the `$INSTANCE_NAME`_MIDI_IN_EP_Service() function to sent the 
*  data from buffer to PC. Non-blocking. Should be called from main foreground 
*  task. 
*  This function is not protected from the reentrant calls. When it is required 
*  to use this function in UART RX ISR to guaranty low latency, care should be
*  taken to protect from reentrant calls.
*
* Parameters:
*  None
*
* Return:
*  None
*
* Global variables:
*   `$INSTANCE_NAME`_midiInPointer: Cleared to zero when data are sent.
*
* Reentrant:
*  No
*
*******************************************************************************/
void `$INSTANCE_NAME`_MIDI_IN_Service(void) `=ReentrantKeil($INSTANCE_NAME . "_MIDI_IN_Service")`
{
    /* Service the MIDI UART inputs until either both receivers have no more
    *   events or until the input endpoint buffer fills up. 
    */
    #if (`$INSTANCE_NAME`_MIDI_EXT_MODE >= `$INSTANCE_NAME`_ONE_EXT_INTRF)
        uint8 m1 = 0;
        uint8 m2 = 0;
        do 
        {
            if (`$INSTANCE_NAME`_midiInPointer <= (`$INSTANCE_NAME`_MIDI_IN_BUFF_SIZE - `$INSTANCE_NAME`_EVENT_LENGTH))
            {
                /* Check MIDI1 input port for a complete event */
                if ((m1 = `$INSTANCE_NAME`_MIDI1_GetEvent()) != 0u)
                {
                    `$INSTANCE_NAME`_PrepareInBuffer(m1, (uint8 *)&`$INSTANCE_NAME`_MIDI1_Event.msgBuff[0], 
                                                `$INSTANCE_NAME`_MIDI1_Event.size, `$INSTANCE_NAME`_MIDI_CABLE_00);
                }
            }

        #if (`$INSTANCE_NAME`_MIDI_EXT_MODE >= `$INSTANCE_NAME`_TWO_EXT_INTRF)
            if (`$INSTANCE_NAME`_midiInPointer <= (`$INSTANCE_NAME`_MIDI_IN_BUFF_SIZE - `$INSTANCE_NAME`_EVENT_LENGTH))
            {
                /* Check MIDI2 input port for a complete event */
                if ((m2 = `$INSTANCE_NAME`_MIDI2_GetEvent()) != 0u)
                {
                    `$INSTANCE_NAME`_PrepareInBuffer(m2, (uint8 *)&`$INSTANCE_NAME`_MIDI2_Event.msgBuff[0], 
                                                `$INSTANCE_NAME`_MIDI2_Event.size, `$INSTANCE_NAME`_MIDI_CABLE_01);
                }
            }
        #endif /* End `$INSTANCE_NAME`_MIDI_EXT_MODE >= `$INSTANCE_NAME`_TWO_EXT_INTRF */

        } while (((m1 != 0u) || (m2 != 0u)) && 
             (`$INSTANCE_NAME`_midiInPointer <= (`$INSTANCE_NAME`_MIDI_IN_BUFF_SIZE - `$INSTANCE_NAME`_EVENT_LENGTH)));
    #endif /* End `$INSTANCE_NAME`_MIDI_EXT_MODE >= `$INSTANCE_NAME`_ONE_EXT_INTRF */

    /* Service the USB MIDI input endpoint */
    `$INSTANCE_NAME`_MIDI_IN_EP_Service();
    
}


/*******************************************************************************
* Function Name: `$INSTANCE_NAME`_PutUsbMidiIn
********************************************************************************
*
* Summary:
*  Puts one MIDI messages into the USB MIDI In endpoint buffer. These are 
*  MIDI input messages to the host. This function is only used if the device 
*  has internal MIDI input functionality. USBMIDI_MIDI_IN_Service() function 
*  should additionally be called to send the message from local buffer to 
*  IN endpoint.  
*
* Parameters:
*  ic:   0 = No message (should never happen)
*        1 - 3 = Complete MIDI message in midiMsg
*        3 - IN EP LENGTH = Complete SySEx message(without EOSEX byte) in 
*            midiMsg. The length is limited by the max BULK EP size(64) 
*        MIDI_SYSEX = Start or continuation of SysEx message
*                     (put event bytes in midiMsg buffer)
*        MIDI_EOSEX = End of SysEx message
*                     (put event bytes in midiMsg buffer)
*        MIDI_TUNEREQ = Tune Request message (single byte system common msg)
*        0xf8 - 0xff = Single byte real-time message
*  midiMsg: pointer to MIDI message.
*  cable:   cable number.
*
* Return:
*  `$INSTANCE_NAME`_TRUE if error.
*  `$INSTANCE_NAME`_FALSE if success.
*
* Global variables:
*  `$INSTANCE_NAME`_midi_in_ep: MIDI IN endpoint number used for sending data.
*  `$INSTANCE_NAME`_midiInPointer: Checked this variable to see if there is 
*    enough free space in the IN endpoint buffer. If buffer is full, initiate 
*    sending to PC. 
*
* Reentrant:
*  No
*
*******************************************************************************/
uint8 `$INSTANCE_NAME`_PutUsbMidiIn(uint8 ic, uint8* midiMsg, uint8 cable)
                                                            `=ReentrantKeil($INSTANCE_NAME . "_PutUsbMidiIn")`
{
    uint8 retError = `$INSTANCE_NAME`_FALSE;

    /* Protect PrepareInBuffer() function from concurent calls */
    #if (`$INSTANCE_NAME`_MIDI_EXT_MODE >= `$INSTANCE_NAME`_ONE_EXT_INTRF)
        MIDI1_UART_DisableRxInt();
        #if (`$INSTANCE_NAME`_MIDI_EXT_MODE >= `$INSTANCE_NAME`_TWO_EXT_INTRF)
            MIDI2_UART_DisableRxInt();
        #endif /* End `$INSTANCE_NAME`_MIDI_EXT_MODE >= `$INSTANCE_NAME`_TWO_EXT_INTRF */
    #endif /* End `$INSTANCE_NAME`_MIDI_EXT_MODE >= `$INSTANCE_NAME`_ONE_EXT_INTRF */

    if (`$INSTANCE_NAME`_midiInPointer > 
                (`$INSTANCE_NAME`_EP[`$INSTANCE_NAME`_midi_in_ep].bufferSize - `$INSTANCE_NAME`_EVENT_LENGTH))
    {
        `$INSTANCE_NAME`_MIDI_IN_EP_Service(); 
    }
    if (`$INSTANCE_NAME`_midiInPointer <= 
                (`$INSTANCE_NAME`_EP[`$INSTANCE_NAME`_midi_in_ep].bufferSize - `$INSTANCE_NAME`_EVENT_LENGTH))
    {
        if((ic < `$INSTANCE_NAME`_EVENT_LENGTH) || (ic >= `$INSTANCE_NAME`_MIDI_STATUS_MASK))
        {
            `$INSTANCE_NAME`_PrepareInBuffer(ic, midiMsg, ic, cable);
        }
        else
        {   /* only SysEx message is greater than 4 bytes */
            do
            {
                `$INSTANCE_NAME`_PrepareInBuffer(`$INSTANCE_NAME`_MIDI_SYSEX, midiMsg, 
                                                 `$INSTANCE_NAME`_EVENT_BYTE3, cable);
                ic -= `$INSTANCE_NAME`_EVENT_BYTE3;
                midiMsg += `$INSTANCE_NAME`_EVENT_BYTE3;
                if (`$INSTANCE_NAME`_midiInPointer > 
                    (`$INSTANCE_NAME`_EP[`$INSTANCE_NAME`_midi_in_ep].bufferSize - `$INSTANCE_NAME`_EVENT_LENGTH))
                {
                    `$INSTANCE_NAME`_MIDI_IN_EP_Service();
                    if (`$INSTANCE_NAME`_midiInPointer >
                        (`$INSTANCE_NAME`_EP[`$INSTANCE_NAME`_midi_in_ep].bufferSize - `$INSTANCE_NAME`_EVENT_LENGTH))
                    {
                        /* Error condition. HOST is not ready to receive this packet */
                        retError = `$INSTANCE_NAME`_TRUE;
                        break;
                    }
                }
            }while(ic > `$INSTANCE_NAME`_EVENT_BYTE3);
            
            if(retError == `$INSTANCE_NAME`_FALSE)
            {
                `$INSTANCE_NAME`_PrepareInBuffer(`$INSTANCE_NAME`_MIDI_EOSEX, midiMsg, ic, cable);
            }    
        }
    }
    else
    {
        /* Error condition. HOST is not ready to receive this packet */
        retError = `$INSTANCE_NAME`_TRUE;
    }

    #if (`$INSTANCE_NAME`_MIDI_EXT_MODE >= `$INSTANCE_NAME`_ONE_EXT_INTRF)
        MIDI1_UART_EnableRxInt();
        #if (`$INSTANCE_NAME`_MIDI_EXT_MODE >= `$INSTANCE_NAME`_TWO_EXT_INTRF)
            MIDI2_UART_EnableRxInt();
        #endif /* End `$INSTANCE_NAME`_MIDI_EXT_MODE >= `$INSTANCE_NAME`_TWO_EXT_INTRF */
    #endif /* End `$INSTANCE_NAME`_MIDI_EXT_MODE >= `$INSTANCE_NAME`_ONE_EXT_INTRF */

    return (retError);
}


/*******************************************************************************
* Function Name: `$INSTANCE_NAME`_PrepareInBuffer
********************************************************************************
*
* Summary:
*  Builds a USB MIDI event in the input endpoint buffer at the current pointer.
*  Puts one MIDI message into the USB MIDI In endpoint buffer.
*
* Parameters:
*  ic:   0 = No message (should never happen)
*        1 - 3 = Complete MIDI message at pMdat[0]
*        MIDI_SYSEX = Start or continuation of SysEx message
*                     (put eventLen bytes in buffer)
*        MIDI_EOSEX = End of SysEx message
*                     (put eventLen bytes in buffer,
*                      and append MIDI_EOSEX)
*        MIDI_TUNEREQ = Tune Request message (single byte system common msg)
*        0xf8 - 0xff = Single byte real-time message
*
*  srcBuff: pointer to MIDI data
*  eventLen: number of bytes in MIDI event
*  cable: MIDI source port number
*
* Return:
*  None
*
* Global variables:
*  `$INSTANCE_NAME`_midiInBuffer: This buffer is used for saving and combine the
*    received from UART(s) and(or) generated internally by 
*    `$INSTANCE_NAME`_PutUsbMidiIn() function messages. 
*  `$INSTANCE_NAME`_midiInPointer: Used as an index for midiInBuffer to 
*     write data.
*
* Reentrant:
*  No
*
*******************************************************************************/
void `$INSTANCE_NAME`_PrepareInBuffer(uint8 ic, uint8 *srcBuff, uint8 eventLen, uint8 cable)
                                                             `=ReentrantKeil($INSTANCE_NAME . "_PrepareInBuffer")`
{
    uint8 srcBuffZero;
    uint8 srcBuffOne;

    srcBuffZero = srcBuff[0u];
    srcBuffOne  = srcBuff[1u];
    
    if (ic >= (`$INSTANCE_NAME`_MIDI_STATUS_MASK | `$INSTANCE_NAME`_MIDI_SINGLE_BYTE_MASK))
    {
        `$INSTANCE_NAME`_midiInBuffer[`$INSTANCE_NAME`_midiInPointer] = `$INSTANCE_NAME`_SINGLE_BYTE | cable;
        `$INSTANCE_NAME`_midiInPointer++;
        `$INSTANCE_NAME`_midiInBuffer[`$INSTANCE_NAME`_midiInPointer] = ic;
        `$INSTANCE_NAME`_midiInPointer++;
        `$INSTANCE_NAME`_midiInBuffer[`$INSTANCE_NAME`_midiInPointer] = 0u;
        `$INSTANCE_NAME`_midiInPointer++;
        `$INSTANCE_NAME`_midiInBuffer[`$INSTANCE_NAME`_midiInPointer] = 0u;
        `$INSTANCE_NAME`_midiInPointer++;
    }
    else if((ic < `$INSTANCE_NAME`_EVENT_LENGTH) || (ic == `$INSTANCE_NAME`_MIDI_SYSEX))
    {
        if(ic == `$INSTANCE_NAME`_MIDI_SYSEX)
        {
            `$INSTANCE_NAME`_midiInBuffer[`$INSTANCE_NAME`_midiInPointer] = `$INSTANCE_NAME`_SYSEX | cable;
            `$INSTANCE_NAME`_midiInPointer++;
        }
        else if (srcBuffZero < `$INSTANCE_NAME`_MIDI_SYSEX)
        {
            `$INSTANCE_NAME`_midiInBuffer[`$INSTANCE_NAME`_midiInPointer] =
                                                    ((srcBuffZero >> 4u) & `$INSTANCE_NAME`_CIN_MASK) | cable;
            `$INSTANCE_NAME`_midiInPointer++;
        }
        else if (srcBuffZero == `$INSTANCE_NAME`_MIDI_TUNEREQ)
        {
            `$INSTANCE_NAME`_midiInBuffer[`$INSTANCE_NAME`_midiInPointer] = `$INSTANCE_NAME`_1BYTE_COMMON | cable;
            `$INSTANCE_NAME`_midiInPointer++;
        }
        else if ((srcBuffZero == `$INSTANCE_NAME`_MIDI_QFM) || (srcBuffZero == `$INSTANCE_NAME`_MIDI_SONGSEL))
        {
            `$INSTANCE_NAME`_midiInBuffer[`$INSTANCE_NAME`_midiInPointer] = `$INSTANCE_NAME`_2BYTE_COMMON | cable;
            `$INSTANCE_NAME`_midiInPointer++;
        }
        else if (srcBuffZero == `$INSTANCE_NAME`_MIDI_SPP)
        {
            `$INSTANCE_NAME`_midiInBuffer[`$INSTANCE_NAME`_midiInPointer] = `$INSTANCE_NAME`_3BYTE_COMMON | cable;
            `$INSTANCE_NAME`_midiInPointer++;
        }
        else
        {
        }
        
        `$INSTANCE_NAME`_midiInBuffer[`$INSTANCE_NAME`_midiInPointer] = srcBuffZero;
        `$INSTANCE_NAME`_midiInPointer++;
        `$INSTANCE_NAME`_midiInBuffer[`$INSTANCE_NAME`_midiInPointer] = srcBuffOne;
        `$INSTANCE_NAME`_midiInPointer++;
        `$INSTANCE_NAME`_midiInBuffer[`$INSTANCE_NAME`_midiInPointer] = srcBuff[2u];
        `$INSTANCE_NAME`_midiInPointer++;
    }
    else if (ic == `$INSTANCE_NAME`_MIDI_EOSEX)
    {
        switch (eventLen)
        {
            case 0:
                `$INSTANCE_NAME`_midiInBuffer[`$INSTANCE_NAME`_midiInPointer] =
                                                                    `$INSTANCE_NAME`_SYSEX_ENDS_WITH1 | cable;
                `$INSTANCE_NAME`_midiInPointer++;
                `$INSTANCE_NAME`_midiInBuffer[`$INSTANCE_NAME`_midiInPointer] = `$INSTANCE_NAME`_MIDI_EOSEX;
                `$INSTANCE_NAME`_midiInPointer++;
                `$INSTANCE_NAME`_midiInBuffer[`$INSTANCE_NAME`_midiInPointer] = 0u;
                `$INSTANCE_NAME`_midiInPointer++;
                `$INSTANCE_NAME`_midiInBuffer[`$INSTANCE_NAME`_midiInPointer] = 0u;
                `$INSTANCE_NAME`_midiInPointer++;
                break;
            case 1:
                `$INSTANCE_NAME`_midiInBuffer[`$INSTANCE_NAME`_midiInPointer] =
                                                                    `$INSTANCE_NAME`_SYSEX_ENDS_WITH2 | cable;
                `$INSTANCE_NAME`_midiInPointer++;
                `$INSTANCE_NAME`_midiInBuffer[`$INSTANCE_NAME`_midiInPointer] = srcBuffZero;
                `$INSTANCE_NAME`_midiInPointer++;
                `$INSTANCE_NAME`_midiInBuffer[`$INSTANCE_NAME`_midiInPointer] = `$INSTANCE_NAME`_MIDI_EOSEX;
                `$INSTANCE_NAME`_midiInPointer++;
                `$INSTANCE_NAME`_midiInBuffer[`$INSTANCE_NAME`_midiInPointer] = 0u;
                `$INSTANCE_NAME`_midiInPointer++;
                break;
            case 2:
                `$INSTANCE_NAME`_midiInBuffer[`$INSTANCE_NAME`_midiInPointer] =
                                                                    `$INSTANCE_NAME`_SYSEX_ENDS_WITH3 | cable;
                `$INSTANCE_NAME`_midiInPointer++;
                `$INSTANCE_NAME`_midiInBuffer[`$INSTANCE_NAME`_midiInPointer] = srcBuffZero;
                `$INSTANCE_NAME`_midiInPointer++;
                `$INSTANCE_NAME`_midiInBuffer[`$INSTANCE_NAME`_midiInPointer] = srcBuffOne;
                `$INSTANCE_NAME`_midiInPointer++;
                `$INSTANCE_NAME`_midiInBuffer[`$INSTANCE_NAME`_midiInPointer] = `$INSTANCE_NAME`_MIDI_EOSEX;
                `$INSTANCE_NAME`_midiInPointer++;
                break;
            default:
                break;
        }
    }
}

/* The implementation for external serial input and output connections 
*  to route USB MIDI data to and from those connections.
*/   
#if (`$INSTANCE_NAME`_MIDI_EXT_MODE >= `$INSTANCE_NAME`_ONE_EXT_INTRF)


    /*******************************************************************************
    * Function Name: `$INSTANCE_NAME`_MIDI_Init
    ********************************************************************************
    *
    * Summary:
    *  Initializes MIDI variables and starts the UART(s) hardware block(s).
    *
    * Parameters:
    *  None
    *
    * Return:
    *  None
    *
    * Side Effects:
    *  Change the priority of the UART(s) TX interrupts to be higher than the 
    *  default EP ISR priority.
    *
    * Global variables:
    *   `$INSTANCE_NAME`_MIDI_Event: initialized to zero.
    *   `$INSTANCE_NAME`_MIDI_TxRunStat: initialized to zero.
    *
    *******************************************************************************/
    void `$INSTANCE_NAME`_MIDI_Init(void) `=ReentrantKeil($INSTANCE_NAME . "_MIDI_Init")`
    {
        `$INSTANCE_NAME`_MIDI1_Event.length = 0u;
        `$INSTANCE_NAME`_MIDI1_Event.count = 0u;
        `$INSTANCE_NAME`_MIDI1_Event.size = 0u;
        `$INSTANCE_NAME`_MIDI1_Event.runstat = 0u;
        `$INSTANCE_NAME`_MIDI1_TxRunStat = 0u;
        `$INSTANCE_NAME`_MIDI1_InqFlags = 0u;
        /* Start UART block */
        MIDI1_UART_Start();
        /* Change the priority of the UART TX and RX interrupt */
        CyIntSetPriority(MIDI1_UART_TX_VECT_NUM, `$INSTANCE_NAME`_CUSTOM_UART_TX_PRIOR_NUM);
        CyIntSetPriority(MIDI1_UART_RX_VECT_NUM, `$INSTANCE_NAME`_CUSTOM_UART_RX_PRIOR_NUM);

        #if (`$INSTANCE_NAME`_MIDI_EXT_MODE >= `$INSTANCE_NAME`_TWO_EXT_INTRF)
            `$INSTANCE_NAME`_MIDI2_Event.length = 0u;
            `$INSTANCE_NAME`_MIDI2_Event.count = 0u;
            `$INSTANCE_NAME`_MIDI2_Event.size = 0u;
            `$INSTANCE_NAME`_MIDI2_Event.runstat = 0u;
            `$INSTANCE_NAME`_MIDI2_TxRunStat = 0u;
            `$INSTANCE_NAME`_MIDI2_InqFlags = 0u;
            /* Start second UART block */
            MIDI2_UART_Start();
            /* Change the priority of the UART TX interrupt */
            CyIntSetPriority(MIDI2_UART_TX_VECT_NUM, `$INSTANCE_NAME`_CUSTOM_UART_TX_PRIOR_NUM);
            CyIntSetPriority(MIDI2_UART_RX_VECT_NUM, `$INSTANCE_NAME`_CUSTOM_UART_RX_PRIOR_NUM);
        #endif /* End `$INSTANCE_NAME`_MIDI_EXT_MODE >= `$INSTANCE_NAME`_TWO_EXT_INTRF*/
        
        /* `#START MIDI_INIT_CUSTOM` Init other extended UARTs here */

        /* `#END` */    

    }


    /*******************************************************************************
    * Function Name: `$INSTANCE_NAME`_ProcessMidiIn
    ********************************************************************************
    *
    * Summary:
    *  Processes one byte of incoming MIDI data.
    *
    * Parameters:
    *   mData = current MIDI input data byte
    *   *rxStat = pointer to a MIDI_RX_STATUS structure    
    *
    * Return:
    *   0, if no complete message
    *   1 - 4, if message complete
    *   MIDI_SYSEX, if start or continuation of system exclusive
    *   MIDI_EOSEX, if end of system exclusive
    *   0xf8 - 0xff, if single byte real time message
    *
    *******************************************************************************/
    uint8 `$INSTANCE_NAME`_ProcessMidiIn(uint8 mData, `$INSTANCE_NAME`_MIDI_RX_STATUS *rxStat)
                                                                `=ReentrantKeil($INSTANCE_NAME . "_ProcessMidiIn")`
    {
        uint8 midiReturn = 0u;

        /* Check for a MIDI status byte.  All status bytes, except real time messages,
        *  which are a single byte, force the start of a new buffer cycle. 
        */
        if ((mData & `$INSTANCE_NAME`_MIDI_STATUS_BYTE_MASK) != 0u)
        {
            if ((mData & `$INSTANCE_NAME`_MIDI_STATUS_MASK) == `$INSTANCE_NAME`_MIDI_STATUS_MASK)
            {
                if ((mData & `$INSTANCE_NAME`_MIDI_SINGLE_BYTE_MASK) != 0u) /* System Real-Time Messages(singl byte) */
                {
                    midiReturn = mData;
                }
                else                              /* System Common Messages */
                {
                    switch (mData)
                    {
                        case `$INSTANCE_NAME`_MIDI_SYSEX:
                            rxStat->msgBuff[0u] = `$INSTANCE_NAME`_MIDI_SYSEX;
                            rxStat->runstat = `$INSTANCE_NAME`_MIDI_SYSEX;
                            rxStat->count = 1u;
                            rxStat->length = 3u;
                            break;
                        case `$INSTANCE_NAME`_MIDI_EOSEX:
                            rxStat->runstat = 0u;
                            rxStat->size = rxStat->count;
                            rxStat->count = 0u;
                            midiReturn = `$INSTANCE_NAME`_MIDI_EOSEX;
                            break;
                        case `$INSTANCE_NAME`_MIDI_SPP:
                            rxStat->msgBuff[0u] = `$INSTANCE_NAME`_MIDI_SPP;
                            rxStat->runstat = 0u;
                            rxStat->count = 1u;
                            rxStat->length = 3u;
                            break;
                        case `$INSTANCE_NAME`_MIDI_SONGSEL:
                            rxStat->msgBuff[0u] = `$INSTANCE_NAME`_MIDI_SONGSEL;
                            rxStat->runstat = 0u;
                            rxStat->count = 1u;
                            rxStat->length = 2u;
                            break;
                        case `$INSTANCE_NAME`_MIDI_QFM:
                            rxStat->msgBuff[0u] = `$INSTANCE_NAME`_MIDI_QFM;
                            rxStat->runstat = 0u;
                            rxStat->count = 1u;
                            rxStat->length = 2u;
                            break;
                        case `$INSTANCE_NAME`_MIDI_TUNEREQ:
                            rxStat->msgBuff[0u] = `$INSTANCE_NAME`_MIDI_TUNEREQ;
                            rxStat->runstat = 0u;
                            rxStat->size = 1u;
                            rxStat->count = 0u;
                            midiReturn = rxStat->size;
                            break;
                        default:
                            break;
                    }
                }
            }
            else /* Channel Messages */
            {
                rxStat->msgBuff[0u] = mData;
                rxStat->runstat = mData;
                rxStat->count = 1u;
                switch (mData & `$INSTANCE_NAME`_MIDI_STATUS_MASK)
                {
                    case `$INSTANCE_NAME`_MIDI_NOTE_OFF:
                    case `$INSTANCE_NAME`_MIDI_NOTE_ON:
                    case `$INSTANCE_NAME`_MIDI_POLY_KEY_PRESSURE:
                    case `$INSTANCE_NAME`_MIDI_CONTROL_CHANGE:
                    case `$INSTANCE_NAME`_MIDI_PITCH_BEND_CHANGE:
                        rxStat->length = 3u;
                        break;
                    case `$INSTANCE_NAME`_MIDI_PROGRAM_CHANGE:
                    case `$INSTANCE_NAME`_MIDI_CHANNEL_PRESSURE:
                        rxStat->length = 2u;
                        break;
                    default:
                        rxStat->runstat = 0u;
                        rxStat->count = 0u;
                        break;
                }
            }
        }

        /* Otherwise, it's a data byte */
        else
        {
            if (rxStat->runstat == `$INSTANCE_NAME`_MIDI_SYSEX)
            {
                rxStat->msgBuff[rxStat->count] = mData;
                rxStat->count++;
                if (rxStat->count >= rxStat->length)
                {
                    rxStat->size = rxStat->count;
                    rxStat->count = 0u;
                    midiReturn = `$INSTANCE_NAME`_MIDI_SYSEX;
                }
            }
            else if (rxStat->count > 0u)
            {
                rxStat->msgBuff[rxStat->count] = mData;
                rxStat->count++;
                if (rxStat->count >= rxStat->length)
                {
                    rxStat->size = rxStat->count;
                    rxStat->count = 0u;
                    midiReturn = rxStat->size;
                }
            }
            else if (rxStat->runstat != 0u)
            {
                rxStat->msgBuff[0u] = rxStat->runstat;
                rxStat->msgBuff[1u] = mData;
                rxStat->count = 2u;
                switch (rxStat->runstat & `$INSTANCE_NAME`_MIDI_STATUS_MASK)
                {
                    case `$INSTANCE_NAME`_MIDI_NOTE_OFF:
                    case `$INSTANCE_NAME`_MIDI_NOTE_ON:
                    case `$INSTANCE_NAME`_MIDI_POLY_KEY_PRESSURE:
                    case `$INSTANCE_NAME`_MIDI_CONTROL_CHANGE:
                    case `$INSTANCE_NAME`_MIDI_PITCH_BEND_CHANGE:
                        rxStat->length = 3u;
                        break;
                    case `$INSTANCE_NAME`_MIDI_PROGRAM_CHANGE:
                    case `$INSTANCE_NAME`_MIDI_CHANNEL_PRESSURE:
                        rxStat->size =rxStat->count;
                        rxStat->count = 0u;
                        midiReturn = rxStat->size;
                        break;
                    default:
                        rxStat->count = 0u;
                    break;
                }
            }
            else
            {
            }
        }
        return (midiReturn);
    }


    /*******************************************************************************
    * Function Name: `$INSTANCE_NAME`_MIDI1_GetEvent
    ********************************************************************************
    *
    * Summary:
    *  Checks for incoming MIDI data, calls the MIDI event builder if so. 
    *  Returns either empty or with a complete event.
    *
    * Parameters:
    *  None
    *
    * Return:
    *   0, if no complete message
    *   1 - 4, if message complete
    *   MIDI_SYSEX, if start or continuation of system exclusive
    *   MIDI_EOSEX, if end of system exclusive
    *   0xf8 - 0xff, if single byte real time message
    *    
    * Global variables:
    *  `$INSTANCE_NAME`_MIDI1_Event: RX status structure used to parse received 
    *    data.
    *
    *******************************************************************************/
    uint8 `$INSTANCE_NAME`_MIDI1_GetEvent(void) `=ReentrantKeil($INSTANCE_NAME . "_MIDI1_GetEvent")`
    {
        uint8 msgRtn = 0u;
        uint8 rxData;
        #if (MIDI1_UART_RXBUFFERSIZE >= 256u)
            uint16 loc_rxBufferRead;
            #if CY_PSOC3 /* This local variable is required only for PSOC3 and large buffer */
                uint16 loc_rxBufferWrite;
            #endif /* end CY_PSOC3 */    
        #else    
            uint8 loc_rxBufferRead;
        #endif /* End MIDI1_UART_RXBUFFERSIZE >= 256 */    

        if ( ((MIDI1_UART_rxBufferRead != MIDI1_UART_rxBufferWrite) || (MIDI1_UART_rxBufferLoopDetect > 0u)) 
             && (msgRtn == 0u) )
        {
            /* Protect variables that could change on interrupt by disabling Rx interrupt.*/        
            #if ((MIDI1_UART_RXBUFFERSIZE >= 256u) && (CY_PSOC3))
                CyIntDisable(MIDI1_UART_RX_VECT_NUM);
            #endif /* End MIDI1_UART_RXBUFFERSIZE >= 256 */    
            loc_rxBufferRead = MIDI1_UART_rxBufferRead;
            #if ((MIDI1_UART_RXBUFFERSIZE >= 256u) && (CY_PSOC3))
                loc_rxBufferWrite = MIDI1_UART_rxBufferWrite;
                CyIntEnable(MIDI1_UART_RX_VECT_NUM);
            #endif /* End MIDI1_UART_RXBUFFERSIZE >= 256 */  
            
            /* Stay here until either the buffer is empty or we have a complete message
            *  in the message buffer. Note that we must use a temporary output pointer to
            *   since it takes two instructions to increment with a wrap, and we can't
            *   risk doing that with the real pointer and getting an interrupt in between
            *   instructions. 
            */

            #if ((MIDI1_UART_RXBUFFERSIZE >= 256u) && (CY_PSOC3))
                while ( ((loc_rxBufferRead != loc_rxBufferWrite) || (MIDI1_UART_rxBufferLoopDetect > 0u))
                    && (msgRtn == 0u) )
            #else        
                while ( ((loc_rxBufferRead != MIDI1_UART_rxBufferWrite) || (MIDI1_UART_rxBufferLoopDetect > 0u))
                    && (msgRtn == 0u) )
            #endif /* End MIDI1_UART_RXBUFFERSIZE >= 256 && CY_PSOC3 */  
                {
                    rxData = MIDI1_UART_rxBuffer[loc_rxBufferRead];
                    loc_rxBufferRead++;
                    if(loc_rxBufferRead >= MIDI1_UART_RXBUFFERSIZE)
                    {
                        loc_rxBufferRead = 0u;
                    }
                    if(MIDI1_UART_rxBufferLoopDetect > 0u )
                    {
                        MIDI1_UART_rxBufferLoopDetect = 0u;
                    }
                        
                    msgRtn = `$INSTANCE_NAME`_ProcessMidiIn(rxData, 
                                                    (`$INSTANCE_NAME`_MIDI_RX_STATUS *)&`$INSTANCE_NAME`_MIDI1_Event);
                }
            
            /* Finally, update the real output pointer, then return with
            *  an indication as to whether there's a complete message in the buffer. 
            */
            #if ((MIDI1_UART_RXBUFFERSIZE >= 256u) && (CY_PSOC3))
                CyIntDisable(MIDI1_UART_RX_VECT_NUM);
            #endif /* End MIDI1_UART_RXBUFFERSIZE >= 256 */    
            MIDI1_UART_rxBufferRead = loc_rxBufferRead;
            #if ((MIDI1_UART_RXBUFFERSIZE >= 256u) && (CY_PSOC3))
                CyIntEnable(MIDI1_UART_RX_VECT_NUM);
            #endif /* End MIDI1_UART_RXBUFFERSIZE >= 256 */   
        }
        
        return (msgRtn);
    }


    /*******************************************************************************
    * Function Name: `$INSTANCE_NAME`_MIDI1_ProcessUsbOut
    ********************************************************************************
    *
    * Summary:
    *  Process a USB MIDI output event.
    *  Puts data into the MIDI TX output buffer.
    *
    * Parameters:
    *  *epBuf: pointer on MIDI event.
    *
    * Return:
    *   None
    *
    * Global variables:
    *  `$INSTANCE_NAME`_MIDI1_TxRunStat: This variable used to save the MIDI 
    *    status byte and skip to send the repeated status byte in subsequent event.
    *  `$INSTANCE_NAME`_MIDI1_InqFlags: The following flags are set when SysEx 
    *    message comes. 
    *    `$INSTANCE_NAME`_INQ_SYSEX_FLAG: Non-Real Time SySEx message received.
    *    `$INSTANCE_NAME`_INQ_IDENTITY_REQ_FLAG: Identity Request received. 
    *      This bit should be cleared by user when Identity Reply message generated.
    *    
    *******************************************************************************/
    void `$INSTANCE_NAME`_MIDI1_ProcessUsbOut(uint8 *epBuf) `=ReentrantKeil($INSTANCE_NAME . "_MIDI1_ProcessUsbOut")`
    {
        uint8 cmd;
        uint8 len;
        uint8 i;

        /* User code is required at the beginning of the procedure */
        /* `#START MIDI1_PROCESS_OUT_BEGIN` */

        /* `#END` */

        cmd = epBuf[`$INSTANCE_NAME`_EVENT_BYTE0] & `$INSTANCE_NAME`_CIN_MASK;
        if((cmd != `$INSTANCE_NAME`_RESERVED0) && (cmd != `$INSTANCE_NAME`_RESERVED1))
        {
            len = `$INSTANCE_NAME`_MIDI_SIZE[cmd];
            i = `$INSTANCE_NAME`_EVENT_BYTE1;
            /* Universal System Exclusive message parsing */
            if(cmd == `$INSTANCE_NAME`_SYSEX)
            {
                if((epBuf[`$INSTANCE_NAME`_EVENT_BYTE1] == `$INSTANCE_NAME`_MIDI_SYSEX) &&
                   (epBuf[`$INSTANCE_NAME`_EVENT_BYTE2] == `$INSTANCE_NAME`_MIDI_SYSEX_NON_REAL_TIME))
                {   /* Non-Real Time SySEx starts */
                    `$INSTANCE_NAME`_MIDI1_InqFlags |= `$INSTANCE_NAME`_INQ_SYSEX_FLAG;
                }
                else
                {
                    `$INSTANCE_NAME`_MIDI1_InqFlags &= ~`$INSTANCE_NAME`_INQ_SYSEX_FLAG;
                }
            }
            else if(cmd == `$INSTANCE_NAME`_SYSEX_ENDS_WITH1)
            {   
                `$INSTANCE_NAME`_MIDI1_InqFlags &= ~`$INSTANCE_NAME`_INQ_SYSEX_FLAG;
            }
            else if(cmd == `$INSTANCE_NAME`_SYSEX_ENDS_WITH2)
            {   
                `$INSTANCE_NAME`_MIDI1_InqFlags &= ~`$INSTANCE_NAME`_INQ_SYSEX_FLAG;
            }
            else if(cmd == `$INSTANCE_NAME`_SYSEX_ENDS_WITH3)
            {
                /* Identify Request support */
		        if((`$INSTANCE_NAME`_MIDI1_InqFlags & `$INSTANCE_NAME`_INQ_SYSEX_FLAG) != 0u)
			    {
                    `$INSTANCE_NAME`_MIDI1_InqFlags &= ~`$INSTANCE_NAME`_INQ_SYSEX_FLAG;
                    if((epBuf[`$INSTANCE_NAME`_EVENT_BYTE1] == `$INSTANCE_NAME`_MIDI_SYSEX_GEN_INFORMATION) &&
                    (epBuf[`$INSTANCE_NAME`_EVENT_BYTE2] == `$INSTANCE_NAME`_MIDI_SYSEX_IDENTITY_REQ))
                    {   /* Set the flag about received the Identity Request.
                        *  The Identity Reply message may be send by user code.
                        */
                        `$INSTANCE_NAME`_MIDI1_InqFlags |= `$INSTANCE_NAME`_INQ_IDENTITY_REQ_FLAG;
                    }
                }    
			}            
            /* Running Status for Voice and Mode messages only. */
            if((cmd >= `$INSTANCE_NAME`_NOTE_OFF) && ( cmd <= `$INSTANCE_NAME`_PITCH_BEND_CHANGE)) 
            {
                if(`$INSTANCE_NAME`_MIDI1_TxRunStat == epBuf[`$INSTANCE_NAME`_EVENT_BYTE1])
                {   /* Skip the repeated Status byte */
                    i++;
                }
                else
                {   /* Save Status byte for next event */
                    `$INSTANCE_NAME`_MIDI1_TxRunStat = epBuf[`$INSTANCE_NAME`_EVENT_BYTE1];
                }
            }    
            else
            {   /* Clear Running Status */
                `$INSTANCE_NAME`_MIDI1_TxRunStat = 0u;
            }
            /* Puts data into the MIDI TX output buffer.*/
            do
            {
                MIDI1_UART_PutChar(epBuf[i]);
                i++;
            } while (i <= len);
        }

        /* User code is required at the end of the procedure */
        /* `#START MIDI1_PROCESS_OUT_END` */

        /* `#END` */
    }

#if (`$INSTANCE_NAME`_MIDI_EXT_MODE >= `$INSTANCE_NAME`_TWO_EXT_INTRF)


    /*******************************************************************************
    * Function Name: `$INSTANCE_NAME`_MIDI2_GetEvent
    ********************************************************************************
    *
    * Summary:
    *  Checks for incoming MIDI data, calls the MIDI event builder if so. 
    *  Returns either empty or with a complete event.
    *
    * Parameters:
    *  None
    *
    * Return:
    *   0, if no complete message
    *   1 - 4, if message complete
    *   MIDI_SYSEX, if start or continuation of system exclusive
    *   MIDI_EOSEX, if end of system exclusive
    *   0xf8 - 0xff, if single byte real time message
    *
    * Global variables:
    *  `$INSTANCE_NAME`_MIDI2_Event: RX status structure used to parse received 
    *    data.
    *
    *******************************************************************************/
    uint8 `$INSTANCE_NAME`_MIDI2_GetEvent(void) `=ReentrantKeil($INSTANCE_NAME . "_MIDI2_GetEvent")`
    {
        uint8 msgRtn = 0u;
        uint8 rxData;
        #if (MIDI2_UART_RXBUFFERSIZE >= 256u)
            uint16 loc_rxBufferRead;
            #if CY_PSOC3 /* This local variable required only for PSOC3 and large buffer */
                uint16 loc_rxBufferWrite;
            #endif /* end CY_PSOC3 */    
        #else    
            uint8 loc_rxBufferRead;
        #endif /* End MIDI2_UART_RXBUFFERSIZE >= 256 */    

        if ( ((MIDI2_UART_rxBufferRead != MIDI2_UART_rxBufferWrite) || (MIDI2_UART_rxBufferLoopDetect > 0u)) 
             && (msgRtn == 0u) )
        {
            /* Protect variables that could change on interrupt by disabling Rx interrupt.*/        
            #if ((MIDI2_UART_RXBUFFERSIZE >= 256u) && (CY_PSOC3))
                CyIntDisable(MIDI2_UART_RX_VECT_NUM);
            #endif /* End MIDI2_UART_RXBUFFERSIZE >= 256 */    
            loc_rxBufferRead = MIDI2_UART_rxBufferRead;
            #if ((MIDI2_UART_RXBUFFERSIZE >= 256u) && (CY_PSOC3))
                loc_rxBufferWrite = MIDI2_UART_rxBufferWrite;
                CyIntEnable(MIDI2_UART_RX_VECT_NUM);
            #endif /* End MIDI2_UART_RXBUFFERSIZE >= 256 */  
            
            /* Stay here until either the buffer is empty or we have a complete message
            *  in the message buffer. Note that we must use a temporary output pointer to
            *   since it takes two instructions to increment with a wrap, and we can't
            *   risk doing that with the real pointer and getting an interrupt in between
            *   instructions. 
            */

            #if ((MIDI2_UART_RXBUFFERSIZE >= 256u) && (CY_PSOC3))
                while ( ((loc_rxBufferRead != loc_rxBufferWrite) || (MIDI1_UART_rxBufferLoopDetect > 0u))
                    && (msgRtn == 0u) )
            #else        
                while ( ((loc_rxBufferRead != MIDI2_UART_rxBufferWrite) || (MIDI1_UART_rxBufferLoopDetect > 0u))
                    && (msgRtn == 0u) )
            #endif /* End MIDI1_UART_RXBUFFERSIZE >= 256 && CY_PSOC3 */  
                {
                    rxData = MIDI2_UART_rxBuffer[loc_rxBufferRead];
                    loc_rxBufferRead++;
                    if(loc_rxBufferRead >= MIDI2_UART_RXBUFFERSIZE)
                    {
                        loc_rxBufferRead = 0u;
                    }
                    if(MIDI2_UART_rxBufferLoopDetect > 0u )
                    {
                        MIDI2_UART_rxBufferLoopDetect = 0u;
                    }
                        
                    msgRtn = `$INSTANCE_NAME`_ProcessMidiIn(rxData, 
                                                    (`$INSTANCE_NAME`_MIDI_RX_STATUS *)&`$INSTANCE_NAME`_MIDI2_Event);
                }
            
            /* Finally, update the real output pointer, then return with
            *  an indication as to whether there's a complete message in the buffer. 
            */
            #if ((MIDI2_UART_RXBUFFERSIZE >= 256u) && (CY_PSOC3))
                CyIntDisable(MIDI2_UART_RX_VECT_NUM);
            #endif /* End MIDI2_UART_RXBUFFERSIZE >= 256 */    
            MIDI2_UART_rxBufferRead = loc_rxBufferRead;
            #if ((MIDI2_UART_RXBUFFERSIZE >= 256u) && (CY_PSOC3))
                CyIntEnable(MIDI2_UART_RX_VECT_NUM);
            #endif /* End MIDI2_UART_RXBUFFERSIZE >= 256 */   
        }
        
        return (msgRtn);
    }


    /*******************************************************************************
    * Function Name: `$INSTANCE_NAME`_MIDI2_ProcessUsbOut
    ********************************************************************************
    *
    * Summary:
    *  Process a USB MIDI output event.
    *  Puts data into the MIDI TX output buffer.
    *
    * Parameters:
    *  *epBuf: pointer on MIDI event.
    *
    * Return:
    *   None
    *
    * Global variables:
    *  `$INSTANCE_NAME`_MIDI1_TxRunStat: This variable used to save the MIDI 
    *    status byte and skip to send the repeated status byte in subsequent event.
    *  `$INSTANCE_NAME`_MIDI1_InqFlags: The following flags are set when SysEx 
    *    message comes. 
    *    `$INSTANCE_NAME`_INQ_SYSEX_FLAG: Non-Real Time SySEx message received.
    *    `$INSTANCE_NAME`_INQ_IDENTITY_REQ_FLAG: Identity Request received. 
    *      This bit should be cleared by user when Identity Reply message generated.
    *    
    *******************************************************************************/
    void `$INSTANCE_NAME`_MIDI2_ProcessUsbOut(uint8 *epBuf) `=ReentrantKeil($INSTANCE_NAME . "_MIDI2_ProcessUsbOut")`
    {
        uint8 cmd;
        uint8 len;
        uint8 i;

        /* User code is required at the beginning of the procedure */
        /* `#START MIDI2_PROCESS_OUT_START` */

        /* `#END` */

        cmd = epBuf[`$INSTANCE_NAME`_EVENT_BYTE0] & `$INSTANCE_NAME`_CIN_MASK;
        if((cmd != `$INSTANCE_NAME`_RESERVED0) && (cmd != `$INSTANCE_NAME`_RESERVED1))
        {
            len = `$INSTANCE_NAME`_MIDI_SIZE[cmd];
            i = `$INSTANCE_NAME`_EVENT_BYTE1;
            /* Universal System Exclusive message parsing */
            if(cmd == `$INSTANCE_NAME`_SYSEX)
            {
                if((epBuf[`$INSTANCE_NAME`_EVENT_BYTE1] == `$INSTANCE_NAME`_MIDI_SYSEX) &&
                   (epBuf[`$INSTANCE_NAME`_EVENT_BYTE2] == `$INSTANCE_NAME`_MIDI_SYSEX_NON_REAL_TIME))
                {   /* SySEx starts */
                    `$INSTANCE_NAME`_MIDI2_InqFlags |= `$INSTANCE_NAME`_INQ_SYSEX_FLAG;
                }
                else
                {
                    `$INSTANCE_NAME`_MIDI2_InqFlags &= ~`$INSTANCE_NAME`_INQ_SYSEX_FLAG;
                }
            }
            else if(cmd == `$INSTANCE_NAME`_SYSEX_ENDS_WITH1)
            {   
                `$INSTANCE_NAME`_MIDI2_InqFlags &= ~`$INSTANCE_NAME`_INQ_SYSEX_FLAG;
            }
            else if(cmd == `$INSTANCE_NAME`_SYSEX_ENDS_WITH2)
            {   
                `$INSTANCE_NAME`_MIDI2_InqFlags &= ~`$INSTANCE_NAME`_INQ_SYSEX_FLAG;
            }
            else if(cmd == `$INSTANCE_NAME`_SYSEX_ENDS_WITH3)
            {
                /* Identify Request support */
		        if((`$INSTANCE_NAME`_MIDI2_InqFlags & `$INSTANCE_NAME`_INQ_SYSEX_FLAG) != 0u)
			    {
                    `$INSTANCE_NAME`_MIDI2_InqFlags &= ~`$INSTANCE_NAME`_INQ_SYSEX_FLAG;
                    if((epBuf[`$INSTANCE_NAME`_EVENT_BYTE1] == `$INSTANCE_NAME`_MIDI_SYSEX_GEN_INFORMATION) &&
                       (epBuf[`$INSTANCE_NAME`_EVENT_BYTE2] == `$INSTANCE_NAME`_MIDI_SYSEX_IDENTITY_REQ))
                    {   /* Set the flag about received the Identity Request.
                        *  The Identity Reply message may be send by user code.
                        */
                        `$INSTANCE_NAME`_MIDI2_InqFlags |= `$INSTANCE_NAME`_INQ_IDENTITY_REQ_FLAG;
                    }
                }    
			}
            else /* Do nothing for other command */
            {
            }
            /* Running Status for Voice and Mode messages only. */
            if((cmd >= `$INSTANCE_NAME`_NOTE_OFF) && ( cmd <= `$INSTANCE_NAME`_PITCH_BEND_CHANGE)) 
            {
                if(`$INSTANCE_NAME`_MIDI2_TxRunStat == epBuf[`$INSTANCE_NAME`_EVENT_BYTE1])
                {   /* Skip the repeated Status byte */
                    i++;
                }
                else
                {   /* Save Status byte for next event */
                    `$INSTANCE_NAME`_MIDI2_TxRunStat = epBuf[`$INSTANCE_NAME`_EVENT_BYTE1];
                }
            }    
            else
            {   /* Clear Running Status */
                `$INSTANCE_NAME`_MIDI2_TxRunStat = 0u;
            }
            /* Puts data into the MIDI TX output buffer.*/
            do
            {
                MIDI2_UART_PutChar(epBuf[i]);
                i++;
            } while (i <= len);
        }

        /* User code is required at the end of the procedure */
        /* `#START MIDI2_PROCESS_OUT_END` */

        /* `#END` */
    }
#endif /* End `$INSTANCE_NAME`_MIDI_EXT_MODE >= `$INSTANCE_NAME`_TWO_EXT_INTRF */
#endif /* End `$INSTANCE_NAME`_MIDI_EXT_MODE >= `$INSTANCE_NAME`_ONE_EXT_INTRF */

#endif  /* End (`$INSTANCE_NAME`_ENABLE_MIDI_API != 0u) */


/* `#START MIDI_FUNCTIONS` Place any additional functions here */

/* `#END` */

#endif  /* End defined(`$INSTANCE_NAME`_ENABLE_MIDI_STREAMING) */


/* [] END OF FILE */
