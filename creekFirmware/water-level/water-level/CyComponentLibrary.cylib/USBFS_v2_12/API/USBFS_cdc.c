/*******************************************************************************
* File Name: `$INSTANCE_NAME`_cdc.c
* Version `$CY_MAJOR_VERSION`.`$CY_MINOR_VERSION`
*
* Description:
*  USB HID Class request handler.
*
* Note:
*
********************************************************************************
* Copyright 2012, Cypress Semiconductor Corporation.  All rights reserved.
* You may use this file only in accordance with the license, terms, conditions,
* disclaimers, and limitations in the end user license agreement accompanying
* the software package with which this file was provided.
*******************************************************************************/

#include "`$INSTANCE_NAME`.h"

#if defined(`$INSTANCE_NAME`_ENABLE_CDC_CLASS)

#include <string.h>
#include "`$INSTANCE_NAME`_cdc.h"


/***************************************
*    CDC Variables
***************************************/

volatile uint8 `$INSTANCE_NAME`_lineCoding[`$INSTANCE_NAME`_LINE_CODING_SIZE];
volatile uint8 `$INSTANCE_NAME`_lineChanged;
volatile uint16 `$INSTANCE_NAME`_lineControlBitmap;
volatile uint8 `$INSTANCE_NAME`_cdc_data_in_ep;
volatile uint8 `$INSTANCE_NAME`_cdc_data_out_ep;


/***************************************
* Custom Declarations
***************************************/

/* `#START CDC_CUSTOM_DECLARATIONS` Place your declaration here */

/* `#END` */


/***************************************
* External data references
***************************************/

extern volatile T_`$INSTANCE_NAME`_EP_CTL_BLOCK `$INSTANCE_NAME`_EP[];
extern volatile T_`$INSTANCE_NAME`_TD `$INSTANCE_NAME`_currentTD;
extern volatile uint8 `$INSTANCE_NAME`_transferState;


/***************************************
* External function references
***************************************/

uint8 `$INSTANCE_NAME`_InitControlRead(void) `=ReentrantKeil($INSTANCE_NAME . "_InitControlRead")`;
uint8 `$INSTANCE_NAME`_InitControlWrite(void) `=ReentrantKeil($INSTANCE_NAME . "_InitControlWrite")`;
uint8 `$INSTANCE_NAME`_InitNoDataControlTransfer(void) `=ReentrantKeil($INSTANCE_NAME . "_InitNoDataControlTransfer")`;


/*******************************************************************************
* Function Name: `$INSTANCE_NAME`_DispatchCDCClassRqst
********************************************************************************
*
* Summary:
*  This routine dispatches CDC class requests.
*
* Parameters:
*  None.
*
* Return:
*  requestHandled
*
* Global variables:
*   `$INSTANCE_NAME`_lineCoding: Contains the current line coding structure.
*     It is set by the Host using SET_LINE_CODING request and returned to the
*     user code by the USBFS_GetDTERate(), USBFS_GetCharFormat(),
*     USBFS_GetParityType(), USBFS_GetDataBits() APIs.
*   `$INSTANCE_NAME`_lineControlBitmap: Contains the current control signal
*     bitmap. It is set by the Host using SET_CONTROL_LINE request and returned
*     to the user code by the USBFS_GetLineControl() API.
*   `$INSTANCE_NAME`_lineChanged: This variable is used as a flag for the
*     USBFS_IsLineChanged() API, to be aware that Host has been sent request
*     for changing Line Coding or Control Bitmap.
*
* Reentrant:
*  No.
*
*******************************************************************************/
uint8 `$INSTANCE_NAME`_DispatchCDCClassRqst() `=ReentrantKeil($INSTANCE_NAME . "_DispatchCDCClassRqst")`
{
    uint8 requestHandled = `$INSTANCE_NAME`_FALSE;

    if ((CY_GET_REG8(`$INSTANCE_NAME`_bmRequestType) & `$INSTANCE_NAME`_RQST_DIR_MASK) == `$INSTANCE_NAME`_RQST_DIR_D2H)
    {   /* Control Read */
        switch (CY_GET_REG8(`$INSTANCE_NAME`_bRequest))
        {
            case `$INSTANCE_NAME`_CDC_GET_LINE_CODING:
                `$INSTANCE_NAME`_currentTD.count = `$INSTANCE_NAME`_LINE_CODING_SIZE;
                `$INSTANCE_NAME`_currentTD.pData = `$INSTANCE_NAME`_lineCoding;
                requestHandled  = `$INSTANCE_NAME`_InitControlRead();
                break;

            /* `#START CDC_READ_REQUESTS` Place other request handler here */

            /* `#END` */

            default:    /* requestHandled is initialezed as FALSE by default */
                break;
        }
    }
    else if ((CY_GET_REG8(`$INSTANCE_NAME`_bmRequestType) & `$INSTANCE_NAME`_RQST_DIR_MASK) == \
                                                                            `$INSTANCE_NAME`_RQST_DIR_H2D)
    {   /* Control Write */
        switch (CY_GET_REG8(`$INSTANCE_NAME`_bRequest))
        {
            case `$INSTANCE_NAME`_CDC_SET_LINE_CODING:
                `$INSTANCE_NAME`_currentTD.count = `$INSTANCE_NAME`_LINE_CODING_SIZE;
                `$INSTANCE_NAME`_currentTD.pData = `$INSTANCE_NAME`_lineCoding;
                `$INSTANCE_NAME`_lineChanged |= `$INSTANCE_NAME`_LINE_CODING_CHANGED;
                requestHandled = `$INSTANCE_NAME`_InitControlWrite();
                break;

            case `$INSTANCE_NAME`_CDC_SET_CONTROL_LINE_STATE:
                `$INSTANCE_NAME`_lineControlBitmap = CY_GET_REG8(`$INSTANCE_NAME`_wValueLo);
                `$INSTANCE_NAME`_lineChanged |= `$INSTANCE_NAME`_LINE_CONTROL_CHANGED;
                requestHandled = `$INSTANCE_NAME`_InitNoDataControlTransfer();
                break;

            /* `#START CDC_WRITE_REQUESTS` Place other request handler here */

            /* `#END` */

            default:    /* requestHandled is initialezed as FALSE by default */
                break;
        }
    }
    else
    {   /* requestHandled is initialezed as FALSE by default */
    }

    return(requestHandled);
}


/***************************************
* Optional CDC APIs
***************************************/
#if (`$INSTANCE_NAME`_ENABLE_CDC_CLASS_API != 0u)


    /*******************************************************************************
    * Function Name: `$INSTANCE_NAME`_CDC_Init
    ********************************************************************************
    *
    * Summary:
    *  This function initialize the CDC interface to be ready for the receive data
    *  from the PC.
    *
    * Parameters:
    *  None.
    *
    * Return:
    *  None.
    *
    * Global variables:
    *   `$INSTANCE_NAME`_lineChanged: Initialized to zero.
    *   `$INSTANCE_NAME`_cdc_data_out_ep: Used as an OUT endpoint number.
    *
    * Reentrant:
    *  No.
    *
    *******************************************************************************/
    void `$INSTANCE_NAME`_CDC_Init(void) `=ReentrantKeil($INSTANCE_NAME . "_CDC_Init")`
    {
        `$INSTANCE_NAME`_lineChanged = 0u;
        `$INSTANCE_NAME`_EnableOutEP(`$INSTANCE_NAME`_cdc_data_out_ep);
    }


    /*******************************************************************************
    * Function Name: `$INSTANCE_NAME`_PutData
    ********************************************************************************
    *
    * Summary:
    *  Sends a specified number of bytes from the location specified by a
    *  pointer to the PC.
    *
    * Parameters:
    *  pData: pointer to the buffer containing data to be sent.
    *  length: Specifies the number of bytes to send from the pData
    *  buffer. Maximum length will be limited by the maximum packet
    *  size for the endpoint.
    *
    * Return:
    *  None.
    *
    * Global variables:
    *   `$INSTANCE_NAME`_cdc_data_in_ep: CDC IN endpoint number used for sending
    *     data.
    *
    * Reentrant:
    *  No.
    *
    *******************************************************************************/
    void `$INSTANCE_NAME`_PutData(uint8* pData, uint16 length) `=ReentrantKeil($INSTANCE_NAME . "_PutData")`
    {
        /* Limits length to maximim packet size for the EP */
        if(length > `$INSTANCE_NAME`_EP[`$INSTANCE_NAME`_cdc_data_in_ep].bufferSize)
        {
            length = `$INSTANCE_NAME`_EP[`$INSTANCE_NAME`_cdc_data_in_ep].bufferSize;
        }
        `$INSTANCE_NAME`_LoadInEP(`$INSTANCE_NAME`_cdc_data_in_ep, pData, length);
    }


    /*******************************************************************************
    * Function Name: `$INSTANCE_NAME`_PutString
    ********************************************************************************
    *
    * Summary:
    *  Sends a null terminated string to the PC.
    *
    * Parameters:
    *  string: pointer to the string to be sent to the PC
    *
    * Return:
    *  None.
    *
    * Global variables:
    *   `$INSTANCE_NAME`_cdc_data_in_ep: CDC IN endpoint number used for sending
    *     data.
    *
    * Reentrant:
    *  No.
    *
    * Theory:
    *  This function will block if there is not enough memory to place the whole
    *  string, it will block until the entire string has been written to the
    *  transmit buffer.
    *
    *******************************************************************************/
    void `$INSTANCE_NAME`_PutString(char8* string) `=ReentrantKeil($INSTANCE_NAME . "_PutString")`
    {
        uint16 str_length;
        uint16 send_length;

        /* Get length of the null terminated string */
        str_length = strlen(string);
        do
        {
            /* Limits length to maximim packet size for the EP */
            send_length = (str_length > `$INSTANCE_NAME`_EP[`$INSTANCE_NAME`_cdc_data_in_ep].bufferSize) ?
                          `$INSTANCE_NAME`_EP[`$INSTANCE_NAME`_cdc_data_in_ep].bufferSize : str_length;
             /* Enable IN transfer */
            `$INSTANCE_NAME`_LoadInEP(`$INSTANCE_NAME`_cdc_data_in_ep, (uint8 *)string, send_length);
            str_length -= send_length;

            /* If more data are present to send */
            if(str_length > 0)
            {
                string += send_length;
                /* Wait for the Host to read it. */
                while(`$INSTANCE_NAME`_EP[`$INSTANCE_NAME`_cdc_data_in_ep].apiEpState ==
                                          `$INSTANCE_NAME`_IN_BUFFER_FULL);
            }
        }while(str_length > 0);
    }


    /*******************************************************************************
    * Function Name: `$INSTANCE_NAME`_PutChar
    ********************************************************************************
    *
    * Summary:
    *  Writes a single character to the PC.
    *
    * Parameters:
    *  txDataByte: Character to be sent to the PC.
    *
    * Return:
    *  None.
    *
    * Global variables:
    *   `$INSTANCE_NAME`_cdc_data_in_ep: CDC IN endpoint number used for sending
    *     data.
    *
    * Reentrant:
    *  No.
    *
    *******************************************************************************/
    void `$INSTANCE_NAME`_PutChar(char8 txDataByte) `=ReentrantKeil($INSTANCE_NAME . "_PutChar")`
    {
        `$INSTANCE_NAME`_LoadInEP(`$INSTANCE_NAME`_cdc_data_in_ep, (uint8 *)&txDataByte, 1u);
    }


    /*******************************************************************************
    * Function Name: `$INSTANCE_NAME`_PutCRLF
    ********************************************************************************
    *
    * Summary:
    *  Sends a carriage return (0x0D) and line feed (0x0A) to the PC
    *
    * Parameters:
    *  None.
    *
    * Return:
    *  None.
    *
    * Global variables:
    *   `$INSTANCE_NAME`_cdc_data_in_ep: CDC IN endpoint number used for sending
    *     data.
    *
    * Reentrant:
    *  No.
    *
    *******************************************************************************/
    void `$INSTANCE_NAME`_PutCRLF(void) `=ReentrantKeil($INSTANCE_NAME . "_PutCRLF")`
    {
        const uint8 CYCODE txData[] = {0x0Du, 0x0Au};

        `$INSTANCE_NAME`_LoadInEP(`$INSTANCE_NAME`_cdc_data_in_ep, (uint8 *)txData, 2u);
    }


    /*******************************************************************************
    * Function Name: `$INSTANCE_NAME`_GetCount
    ********************************************************************************
    *
    * Summary:
    *  This function returns the number of bytes that were received from the PC.
    *
    * Parameters:
    *  None.
    *
    * Return:
    *  Returns the number of received bytes.
    *
    * Global variables:
    *   `$INSTANCE_NAME`_cdc_data_out_ep: CDC OUT endpoint number used.
    *
    *******************************************************************************/
    uint16 `$INSTANCE_NAME`_GetCount(void) `=ReentrantKeil($INSTANCE_NAME . "_GetCount")`
    {
        uint8 bytesCount = 0u;

        if (`$INSTANCE_NAME`_EP[`$INSTANCE_NAME`_cdc_data_out_ep].apiEpState == `$INSTANCE_NAME`_OUT_BUFFER_FULL)
        {
            bytesCount = `$INSTANCE_NAME`_GetEPCount(`$INSTANCE_NAME`_cdc_data_out_ep);
        }

        return(bytesCount);
    }


    /*******************************************************************************
    * Function Name: `$INSTANCE_NAME`_DataIsReady
    ********************************************************************************
    *
    * Summary:
    *  Returns a nonzero value if the component received data or received
    *  zero-length packet. The GetAll() or GetData() API should be called to read
    *  data from the buffer and reinit OUT endpoint even when zero-length packet
    *  received.
    *
    * Parameters:
    *  None.
    *
    * Return:
    *  If the OUT packet received this function returns a nonzero value.
    *  Otherwise zero is returned.
    *
    * Global variables:
    *   `$INSTANCE_NAME`_cdc_data_out_ep: CDC OUT endpoint number used.
    *
    *******************************************************************************/
    uint8 `$INSTANCE_NAME`_DataIsReady(void) `=ReentrantKeil($INSTANCE_NAME . "_DataIsReady")`
    {
        return(`$INSTANCE_NAME`_EP[`$INSTANCE_NAME`_cdc_data_out_ep].apiEpState);
    }


    /*******************************************************************************
    * Function Name: `$INSTANCE_NAME`_CDCIsReady
    ********************************************************************************
    *
    * Summary:
    *  Returns a nonzero value if the component is ready to send more data to the
    *  PC. Otherwise returns zero. Should be called before sending new data to
    *  ensure the previous data has finished sending.This function returns the
    *  number of bytes that were received from the PC.
    *
    * Parameters:
    *  None.
    *
    * Return:
    *  If the buffer can accept new data then this function returns a nonzero value.
    *  Otherwise zero is returned.
    *
    * Global variables:
    *   `$INSTANCE_NAME`_cdc_data_in_ep: CDC IN endpoint number used.
    *
    *******************************************************************************/
    uint8 `$INSTANCE_NAME`_CDCIsReady(void) `=ReentrantKeil($INSTANCE_NAME . "_CDCIsReady")`
    {
        return(`$INSTANCE_NAME`_EP[`$INSTANCE_NAME`_cdc_data_in_ep].apiEpState);
    }


    /*******************************************************************************
    * Function Name: `$INSTANCE_NAME`_GetData
    ********************************************************************************
    *
    * Summary:
    *  Gets a specified number of bytes from the input buffer and places it in a
    *  data array specified by the passed pointer.
    *  `$INSTANCE_NAME`_DataIsReady() API should be called before, to be sure
    *  that data is received from the Host.
    *
    * Parameters:
    *  pData: Pointer to the data array where data will be placed.
    *  Length: Number of bytes to read into the data array from the RX buffer.
    *          Maximum length is limited by the the number of received bytes.
    *
    * Return:
    *  Number of bytes received.
    *
    * Global variables:
    *   `$INSTANCE_NAME`_cdc_data_out_ep: CDC OUT endpoint number used.
    *
    * Reentrant:
    *  No.
    *
    *******************************************************************************/
    uint16 `$INSTANCE_NAME`_GetData(uint8* pData, uint16 length) `=ReentrantKeil($INSTANCE_NAME . "_GetData")`
    {
        return(`$INSTANCE_NAME`_ReadOutEP(`$INSTANCE_NAME`_cdc_data_out_ep, pData, length));
    }


    /*******************************************************************************
    * Function Name: `$INSTANCE_NAME`_GetAll
    ********************************************************************************
    *
    * Summary:
    *  Gets all bytes of received data from the input buffer and places it into a
    *  specified data array. `$INSTANCE_NAME`_DataIsReady() API should be called
    *  before, to be sure that data is received from the Host.
    *
    * Parameters:
    *  pData: Pointer to the data array where data will be placed.
    *
    * Return:
    *  Number of bytes received.
    *
    * Global variables:
    *   `$INSTANCE_NAME`_cdc_data_out_ep: CDC OUT endpoint number used.
    *   `$INSTANCE_NAME`_EP[].bufferSize: EP max packet size is used as a length
    *     to read all data from the EP buffer.
    *
    * Reentrant:
    *  No.
    *
    *******************************************************************************/
    uint16 `$INSTANCE_NAME`_GetAll(uint8* pData) `=ReentrantKeil($INSTANCE_NAME . "_GetAll")`
    {
        return (`$INSTANCE_NAME`_ReadOutEP(`$INSTANCE_NAME`_cdc_data_out_ep, pData,
                                           `$INSTANCE_NAME`_EP[`$INSTANCE_NAME`_cdc_data_out_ep].bufferSize));
    }


    /*******************************************************************************
    * Function Name: `$INSTANCE_NAME`_GetChar
    ********************************************************************************
    *
    * Summary:
    *  Reads one byte of received data from the buffer.
    *
    * Parameters:
    *  None.
    *
    * Return:
    *  Received one character.
    *
    * Global variables:
    *   `$INSTANCE_NAME`_cdc_data_out_ep: CDC OUT endpoint number used.
    *
    * Reentrant:
    *  No.
    *
    *******************************************************************************/
    uint8 `$INSTANCE_NAME`_GetChar(void) `=ReentrantKeil($INSTANCE_NAME . "_GetChar")`
    {
         uint8 rxData;

        `$INSTANCE_NAME`_ReadOutEP(`$INSTANCE_NAME`_cdc_data_out_ep, &rxData, 1u);

        return(rxData);
    }

    /*******************************************************************************
    * Function Name: `$INSTANCE_NAME`_IsLineChanged
    ********************************************************************************
    *
    * Summary:
    *  This function returns clear on read status of the line.
    *
    * Parameters:
    *  None.
    *
    * Return:
    *  If SET_LINE_CODING or CDC_SET_CONTROL_LINE_STATE request received then not
    *  zero value returned. Otherwise zero is returned.
    *
    * Global variables:
    *  `$INSTANCE_NAME`_transferState - it is checked to be sure then OUT data
    *    phase has been compleate, and data written to the lineCoding or Control
    *    Bitmap buffer.
    *  `$INSTANCE_NAME`_lineChanged: used as a flag to be aware that Host has been
    *    sent request for changing Line Coding or Control Bitmap.
    *
    *******************************************************************************/
    uint8 `$INSTANCE_NAME`_IsLineChanged(void) `=ReentrantKeil($INSTANCE_NAME . "_IsLineChanged")`
    {
        uint8 state = 0u;
        /* transferState is checked to be sure then OUT data phase has been compleate */
        if((`$INSTANCE_NAME`_lineChanged != 0u) &&
           (`$INSTANCE_NAME`_transferState == `$INSTANCE_NAME`_TRANS_STATE_IDLE))
        {
            state = `$INSTANCE_NAME`_lineChanged;
            `$INSTANCE_NAME`_lineChanged = 0u;
        }
        return(state);
    }


    /*******************************************************************************
    * Function Name: `$INSTANCE_NAME`_GetDTERate
    ********************************************************************************
    *
    * Summary:
    *  Returns the data terminal rate set for this port in bits per second.
    *
    * Parameters:
    *  None.
    *
    * Return:
    *  Returns a uint32 value of the data rate in bits per second.
    *
    * Global variables:
    *  `$INSTANCE_NAME`_lineCoding: First four bytes converted to uint32
    *    depend on compiler, and returned as a data rate.
    *
    *******************************************************************************/
    uint32 `$INSTANCE_NAME`_GetDTERate(void) `=ReentrantKeil($INSTANCE_NAME . "_GetDTERate")`
    {
        uint32 rate;
        /* Data terminal rate has little endian format. */
        #if defined(__C51__)
            /*   KEIL for the 8051 is a Big Endian compiler. This requires four bytes swapping. */
            ((uint8 *)&rate)[0u] = `$INSTANCE_NAME`_lineCoding[`$INSTANCE_NAME`_LINE_CODING_RATE + 3u];
            ((uint8 *)&rate)[1u] = `$INSTANCE_NAME`_lineCoding[`$INSTANCE_NAME`_LINE_CODING_RATE + 2u];
            ((uint8 *)&rate)[2u] = `$INSTANCE_NAME`_lineCoding[`$INSTANCE_NAME`_LINE_CODING_RATE + 1u];
            ((uint8 *)&rate)[3u] = `$INSTANCE_NAME`_lineCoding[`$INSTANCE_NAME`_LINE_CODING_RATE];
        #elif defined(__GNUC__) || defined(__ARMCC_VERSION)
            /* ARM compilers (GCC and RealView) are little-endian */
            `$INSTANCE_NAME`_CONVERT_DWORD *convert =
                (`$INSTANCE_NAME`_CONVERT_DWORD *)&`$INSTANCE_NAME`_lineCoding[`$INSTANCE_NAME`_LINE_CODING_RATE];
            rate = convert->dword;
        #endif /* End Compillers */
        return(rate);
    }


    /*******************************************************************************
    * Function Name: `$INSTANCE_NAME`_GetCharFormat
    ********************************************************************************
    *
    * Summary:
    *  Returns the number of stop bits.
    *
    * Parameters:
    *  None.
    *
    * Return:
    *  Returns the number of stop bits.
    *
    * Global variables:
    *  `$INSTANCE_NAME`_lineCoding: used to get a parameter.
    *
    *******************************************************************************/
    uint8 `$INSTANCE_NAME`_GetCharFormat(void) `=ReentrantKeil($INSTANCE_NAME . "_GetCharFormat")`
    {
        return(`$INSTANCE_NAME`_lineCoding[`$INSTANCE_NAME`_LINE_CODING_STOP_BITS]);
    }


    /*******************************************************************************
    * Function Name: `$INSTANCE_NAME`_GetParityType
    ********************************************************************************
    *
    * Summary:
    *  Returns the parity type for the CDC port.
    *
    * Parameters:
    *  None.
    *
    * Return:
    *  Returns the parity type.
    *
    * Global variables:
    *  `$INSTANCE_NAME`_lineCoding: used to get a parameter.
    *
    *******************************************************************************/
    uint8 `$INSTANCE_NAME`_GetParityType(void) `=ReentrantKeil($INSTANCE_NAME . "_GetParityType")`
    {
        return(`$INSTANCE_NAME`_lineCoding[`$INSTANCE_NAME`_LINE_CODING_PARITY]);
    }


    /*******************************************************************************
    * Function Name: `$INSTANCE_NAME`_GetDataBits
    ********************************************************************************
    *
    * Summary:
    *  Returns the number of data bits for the CDC port.
    *
    * Parameters:
    *  None.
    *
    * Return:
    *  Returns the number of data bits.
    *  The number of data bits can be 5, 6, 7, 8 or 16.
    *
    * Global variables:
    *  `$INSTANCE_NAME`_lineCoding: used to get a parameter.
    *
    *******************************************************************************/
    uint8 `$INSTANCE_NAME`_GetDataBits(void) `=ReentrantKeil($INSTANCE_NAME . "_GetDataBits")`
    {
        return(`$INSTANCE_NAME`_lineCoding[`$INSTANCE_NAME`_LINE_CODING_DATA_BITS]);
    }


    /*******************************************************************************
    * Function Name: `$INSTANCE_NAME`_GetLineControl
    ********************************************************************************
    *
    * Summary:
    *  Returns Line control bitmap.
    *
    * Parameters:
    *  None.
    *
    * Return:
    *  Returns Line control bitmap.
    *
    * Global variables:
    *  `$INSTANCE_NAME`_lineControlBitmap: used to get a parameter.
    *
    *******************************************************************************/
    uint16 `$INSTANCE_NAME`_GetLineControl(void) `=ReentrantKeil($INSTANCE_NAME . "_GetLineControl")`
    {
        return(`$INSTANCE_NAME`_lineControlBitmap);
    }

#endif  /* End `$INSTANCE_NAME`_ENABLE_CDC_CLASS_API*/


/*******************************************************************************
* Additional user functions supporting CDC Requests
********************************************************************************/

/* `#START CDC_FUNCTIONS` Place any additional functions here */

/* `#END` */

#endif  /* End `$INSTANCE_NAME`_ENABLE_CDC_CLASS*/


/* [] END OF FILE */
