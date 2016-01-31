/*******************************************************************************
* File Name: `$INSTANCE_NAME`_drv.c
* Version `$CY_MAJOR_VERSION`.`$CY_MINOR_VERSION`
*
* Description:
*  Endpoint 0 Driver for the USBFS Component.
*
* Note:
*
********************************************************************************
* Copyright 2008-2012, Cypress Semiconductor Corporation.  All rights reserved.
* You may use this file only in accordance with the license, terms, conditions,
* disclaimers, and limitations in the end user license agreement accompanying
* the software package with which this file was provided.
*******************************************************************************/

#include "`$INSTANCE_NAME`.h"


/***************************************
* Forward references for the EP0 ISR
***************************************/

void  `$INSTANCE_NAME`_HandleSetup(void) `=ReentrantKeil($INSTANCE_NAME . "_HandleSetup")`;
void  `$INSTANCE_NAME`_HandleIN(void) `=ReentrantKeil($INSTANCE_NAME . "_HandleIN")`;
void  `$INSTANCE_NAME`_HandleOUT(void) `=ReentrantKeil($INSTANCE_NAME . "_HandleOUT")`;
uint8 `$INSTANCE_NAME`_InitControlRead(void) `=ReentrantKeil($INSTANCE_NAME . "_InitControlRead")`;
void  `$INSTANCE_NAME`_ControlReadDataStage(void) `=ReentrantKeil($INSTANCE_NAME . "_ControlReadDataStage")`;
void  `$INSTANCE_NAME`_ControlReadStatusStage(void) `=ReentrantKeil($INSTANCE_NAME . "_ControlReadStatusStage")`;
void  `$INSTANCE_NAME`_ControlReadPrematureStatus(void)
                                                `=ReentrantKeil($INSTANCE_NAME . "_ControlReadPrematureStatus")`;
uint8 `$INSTANCE_NAME`_InitControlWrite(void) `=ReentrantKeil($INSTANCE_NAME . "_InitControlWrite")`;
uint8 `$INSTANCE_NAME`_InitZeroLengthControlTransfer(void)
                                                `=ReentrantKeil($INSTANCE_NAME . "_InitZeroLengthControlTransfer")`;
void  `$INSTANCE_NAME`_ControlWriteDataStage(void) `=ReentrantKeil($INSTANCE_NAME . "_ControlWriteDataStage")`;
void  `$INSTANCE_NAME`_ControlWriteStatusStage(void) `=ReentrantKeil($INSTANCE_NAME . "_ControlWriteStatusStage")`;
void  `$INSTANCE_NAME`_ControlWritePrematureStatus(void)
                                                `=ReentrantKeil($INSTANCE_NAME . "_ControlWritePrematureStatus")`;
uint8 `$INSTANCE_NAME`_InitNoDataControlTransfer(void) `=ReentrantKeil($INSTANCE_NAME . "_InitNoDataControlTransfer")`;
void  `$INSTANCE_NAME`_NoDataControlStatusStage(void) `=ReentrantKeil($INSTANCE_NAME . "_NoDataControlStatusStage")`;
void  `$INSTANCE_NAME`_InitializeStatusBlock(void) `=ReentrantKeil($INSTANCE_NAME . "_InitializeStatusBlock")`;
void  `$INSTANCE_NAME`_UpdateStatusBlock(uint8 completionCode) `=ReentrantKeil($INSTANCE_NAME . "_UpdateStatusBlock")`;


/***************************************
* Request Handlers
***************************************/

uint8 `$INSTANCE_NAME`_HandleStandardRqst(void) `=ReentrantKeil($INSTANCE_NAME . "_HandleStandardRqst")`;
uint8 `$INSTANCE_NAME`_DispatchClassRqst(void) `=ReentrantKeil($INSTANCE_NAME . "_DispatchClassRqst")`;
uint8 `$INSTANCE_NAME`_HandleVendorRqst(void) `=ReentrantKeil($INSTANCE_NAME . "_HandleVendorRqst")`;


/***************************************
* External data references
***************************************/


/***************************************
* Global data allocation
***************************************/

volatile T_`$INSTANCE_NAME`_EP_CTL_BLOCK `$INSTANCE_NAME`_EP[`$INSTANCE_NAME`_MAX_EP];
volatile uint8 `$INSTANCE_NAME`_configuration;
volatile uint8 `$INSTANCE_NAME`_configurationChanged;
volatile uint8 `$INSTANCE_NAME`_deviceAddress;
volatile uint8 `$INSTANCE_NAME`_deviceStatus;
volatile uint8 `$INSTANCE_NAME`_interfaceSetting[`$INSTANCE_NAME`_MAX_INTERFACES_NUMBER];
volatile uint8 `$INSTANCE_NAME`_interfaceSetting_last[`$INSTANCE_NAME`_MAX_INTERFACES_NUMBER];
volatile uint8 `$INSTANCE_NAME`_interfaceStatus[`$INSTANCE_NAME`_MAX_INTERFACES_NUMBER];
volatile uint8 `$INSTANCE_NAME`_device;
const uint8 CYCODE *`$INSTANCE_NAME`_interfaceClass;


/***************************************
* Local data allocation
***************************************/

volatile uint8 `$INSTANCE_NAME`_ep0Toggle;
volatile uint8 `$INSTANCE_NAME`_lastPacketSize;
volatile uint8 `$INSTANCE_NAME`_transferState;
volatile T_`$INSTANCE_NAME`_TD `$INSTANCE_NAME`_currentTD;
volatile uint8 `$INSTANCE_NAME`_ep0Mode;
volatile uint8 `$INSTANCE_NAME`_ep0Count;
volatile uint16 `$INSTANCE_NAME`_transferByteCount;


/*******************************************************************************
* Function Name: `$INSTANCE_NAME`_ep_0_Interrupt
********************************************************************************
*
* Summary:
*  This Interrupt Service Routine handles Endpoint 0 (Control Pipe) traffic.
*  It dispactches setup requests and handles the data and status stages.
*
* Parameters:
*  None.
*
* Return:
*  None.
*
*******************************************************************************/
CY_ISR(`$INSTANCE_NAME`_EP_0_ISR)
{
    uint8 bRegTemp;
    uint8 modifyReg;


    bRegTemp = CY_GET_REG8(`$INSTANCE_NAME`_EP0_CR_PTR);
    if (bRegTemp & `$INSTANCE_NAME`_MODE_ACKD)
    {
        modifyReg = 1u;
        if (bRegTemp & `$INSTANCE_NAME`_MODE_SETUP_RCVD)
        {
            if((bRegTemp & `$INSTANCE_NAME`_MODE_MASK) != `$INSTANCE_NAME`_MODE_NAK_IN_OUT)
            {
                modifyReg = 0u;                                     /* When mode not NAK_IN_OUT => invalid setup */
            }
            else
            {
                `$INSTANCE_NAME`_HandleSetup();
                if(`$INSTANCE_NAME`_ep0Mode & `$INSTANCE_NAME`_MODE_SETUP_RCVD)
                {
                    modifyReg = 0u;                         /* if SETUP bit set -> exit without modifying the mode */
                }

            }
        }
        else if (bRegTemp & `$INSTANCE_NAME`_MODE_IN_RCVD)
        {
            `$INSTANCE_NAME`_HandleIN();
        }
        else if (bRegTemp & `$INSTANCE_NAME`_MODE_OUT_RCVD)
        {
            `$INSTANCE_NAME`_HandleOUT();
        }
        else
        {
            modifyReg = 0u;
        }
        if(modifyReg != 0u)
        {
            bRegTemp = CY_GET_REG8(`$INSTANCE_NAME`_EP0_CR_PTR);    /* unlock registers */
            if((bRegTemp & `$INSTANCE_NAME`_MODE_SETUP_RCVD) == 0)  /* Check if SETUP bit is not set, otherwise exit */
            {
                /* Update the count register */
                bRegTemp = `$INSTANCE_NAME`_ep0Toggle | `$INSTANCE_NAME`_ep0Count;
                CY_SET_REG8(`$INSTANCE_NAME`_EP0_CNT_PTR, bRegTemp);
                if(bRegTemp == CY_GET_REG8(`$INSTANCE_NAME`_EP0_CNT_PTR))   /* continue if writing was succesful */
                {
                    do
                    {
                        modifyReg = `$INSTANCE_NAME`_ep0Mode;       /* init temporary variable */
                        /* unlock registers */
                        bRegTemp = CY_GET_REG8(`$INSTANCE_NAME`_EP0_CR_PTR) & `$INSTANCE_NAME`_MODE_SETUP_RCVD;
                        if(bRegTemp == 0)                           /* Check if SETUP bit is not set */
                        {
                            /* Set the Mode Register  */
                            CY_SET_REG8(`$INSTANCE_NAME`_EP0_CR_PTR, `$INSTANCE_NAME`_ep0Mode);
                            /* writing check */
                            modifyReg = CY_GET_REG8(`$INSTANCE_NAME`_EP0_CR_PTR) & `$INSTANCE_NAME`_MODE_MASK;
                        }
                    }while(modifyReg != `$INSTANCE_NAME`_ep0Mode);  /* repeat if writing was not successful */
                }
            }
        }
    }
    /* PSoC3 ES1, ES2 RTC ISR PATCH  */
    #if(CY_PSOC3_ES2 && (`$INSTANCE_NAME`_ep_0__ES2_PATCH))
        `$INSTANCE_NAME`_ISR_PATCH();
    #endif /* End CY_PSOC3_ES2*/
}


/*******************************************************************************
* Function Name: `$INSTANCE_NAME`_HandleSetup
********************************************************************************
*
* Summary:
*  This Routine dispatches requests for the four USB request types
*
* Parameters:
*  None.
*
* Return:
*  None.
*
* Reentrant:
*  No.
*
*******************************************************************************/
void `$INSTANCE_NAME`_HandleSetup(void) `=ReentrantKeil($INSTANCE_NAME . "_HandleSetup")`
{
    uint8 requestHandled;

    requestHandled = CY_GET_REG8(`$INSTANCE_NAME`_EP0_CR_PTR);      /* unlock registers */
    CY_SET_REG8(`$INSTANCE_NAME`_EP0_CR_PTR, requestHandled);       /* clear setup bit */
    requestHandled = CY_GET_REG8(`$INSTANCE_NAME`_EP0_CR_PTR);      /* reread register */
    if(requestHandled & `$INSTANCE_NAME`_MODE_SETUP_RCVD)
    {
        `$INSTANCE_NAME`_ep0Mode = requestHandled;        /* if SETUP bit set -> exit without modifying the mode */
    }
    else
    {
        /* In case the previous transfer did not complete, close it out */
        `$INSTANCE_NAME`_UpdateStatusBlock(`$INSTANCE_NAME`_XFER_PREMATURE);

        switch (CY_GET_REG8(`$INSTANCE_NAME`_bmRequestType) & `$INSTANCE_NAME`_RQST_TYPE_MASK)
        {
            case `$INSTANCE_NAME`_RQST_TYPE_STD:
                requestHandled = `$INSTANCE_NAME`_HandleStandardRqst();
                break;
            case `$INSTANCE_NAME`_RQST_TYPE_CLS:
                requestHandled = `$INSTANCE_NAME`_DispatchClassRqst();
                break;
            case `$INSTANCE_NAME`_RQST_TYPE_VND:
                requestHandled = `$INSTANCE_NAME`_HandleVendorRqst();
                break;
            default:
                requestHandled = `$INSTANCE_NAME`_FALSE;
                break;
        }
        if (requestHandled == `$INSTANCE_NAME`_FALSE)
        {
            `$INSTANCE_NAME`_ep0Mode = `$INSTANCE_NAME`_MODE_STALL_IN_OUT;
        }
    }
}


/*******************************************************************************
* Function Name: `$INSTANCE_NAME`_HandleIN
********************************************************************************
*
* Summary:
*  This routine handles EP0 IN transfers.
*
* Parameters:
*  None.
*
* Return:
*  None.
*
* Reentrant:
*  No.
*
*******************************************************************************/
void `$INSTANCE_NAME`_HandleIN(void) `=ReentrantKeil($INSTANCE_NAME . "_HandleIN")`
{
    switch (`$INSTANCE_NAME`_transferState)
    {
        case `$INSTANCE_NAME`_TRANS_STATE_IDLE:
            break;
        case `$INSTANCE_NAME`_TRANS_STATE_CONTROL_READ:
            `$INSTANCE_NAME`_ControlReadDataStage();
            break;
        case `$INSTANCE_NAME`_TRANS_STATE_CONTROL_WRITE:
            `$INSTANCE_NAME`_ControlWriteStatusStage();
            break;
        case `$INSTANCE_NAME`_TRANS_STATE_NO_DATA_CONTROL:
            `$INSTANCE_NAME`_NoDataControlStatusStage();
            break;
        default:    /* there are no more states */
            break;
    }
}


/*******************************************************************************
* Function Name: `$INSTANCE_NAME`_HandleOUT
********************************************************************************
*
* Summary:
*  This routine handles EP0 OUT transfers.
*
* Parameters:
*  None.
*
* Return:
*  None.
*
* Reentrant:
*  No.
*
*******************************************************************************/
void `$INSTANCE_NAME`_HandleOUT(void) `=ReentrantKeil($INSTANCE_NAME . "_HandleOUT")`
{
    switch (`$INSTANCE_NAME`_transferState)
    {
        case `$INSTANCE_NAME`_TRANS_STATE_IDLE:
            break;
        case `$INSTANCE_NAME`_TRANS_STATE_CONTROL_READ:
            `$INSTANCE_NAME`_ControlReadStatusStage();
            break;
        case `$INSTANCE_NAME`_TRANS_STATE_CONTROL_WRITE:
            `$INSTANCE_NAME`_ControlWriteDataStage();
            break;
        case `$INSTANCE_NAME`_TRANS_STATE_NO_DATA_CONTROL:
            /* Update the completion block */
            `$INSTANCE_NAME`_UpdateStatusBlock(`$INSTANCE_NAME`_XFER_ERROR);
            /* We expect no more data, so stall INs and OUTs */
            `$INSTANCE_NAME`_ep0Mode = `$INSTANCE_NAME`_MODE_STALL_IN_OUT;
            break;
        default:    /* there are no more states */
            break;
    }
}


/*******************************************************************************
* Function Name: `$INSTANCE_NAME`_LoadEP0
********************************************************************************
*
* Summary:
*  This routine loads the EP0 data registers for OUT transfers.  It uses the
*  currentTD (previously initialized by the _InitControlWrite function and
*  updated for each OUT transfer, and the bLastPacketSize) to determine how
*  many uint8s to transfer on the current OUT.
*
*  If the number of uint8s remaining is zero and the last transfer was full,
*  we need to send a zero length packet.  Otherwise we send the minimum
*  of the control endpoint size (8) or remaining number of uint8s for the
*  transaction.
*
* Parameters:
*  None.
*
* Return:
*  None.
*
* Global variables:
*  `$INSTANCE_NAME`_transferByteCount - Update the transfer byte count from the
*     last transaction.
*  `$INSTANCE_NAME`_ep0Count - counts the data loaded to the SIE memory in
*     current packet.
*  `$INSTANCE_NAME`_lastPacketSize - remebers the USBFS_ep0Count value for the
*     next packet.
*  `$INSTANCE_NAME`_transferByteCount - suma of the previos bytes transferred
*     on previous packets(suma of USBFS_lastPacketSize)
*  `$INSTANCE_NAME`_ep0Toggle - inverted
*  `$INSTANCE_NAME`_ep0Mode  - prepare for mode register content.
*  `$INSTANCE_NAME`_transferState - set to TRANS_STATE_CONTROL_READ
*
* Reentrant:
*  No.
*
*******************************************************************************/
void `$INSTANCE_NAME`_LoadEP0(void) `=ReentrantKeil($INSTANCE_NAME . "_LoadEP0")`
{
    /* Update the transfer byte count from the last transaction */
    `$INSTANCE_NAME`_transferByteCount += `$INSTANCE_NAME`_lastPacketSize;
    /* Now load the next transaction */
    `$INSTANCE_NAME`_ep0Count = 0u;
    while ((`$INSTANCE_NAME`_currentTD.count > 0u) && (`$INSTANCE_NAME`_ep0Count < 8u))
    {
        `$INSTANCE_NAME`_EP0_DR0_PTR[`$INSTANCE_NAME`_ep0Count] = *`$INSTANCE_NAME`_currentTD.pData++;
        `$INSTANCE_NAME`_ep0Count++;
        `$INSTANCE_NAME`_currentTD.count--;
    }
    /* Support zero-length packet*/
    if( (`$INSTANCE_NAME`_ep0Count > 0u) || (`$INSTANCE_NAME`_lastPacketSize == 8u) )
    {
        /* Update the data toggle */
        `$INSTANCE_NAME`_ep0Toggle ^= `$INSTANCE_NAME`_EP0_CNT_DATA_TOGGLE;
        /* Set the Mode Register  */
        `$INSTANCE_NAME`_ep0Mode = `$INSTANCE_NAME`_MODE_ACK_IN_STATUS_OUT;
        /* Update the state (or stay the same) */
        `$INSTANCE_NAME`_transferState = `$INSTANCE_NAME`_TRANS_STATE_CONTROL_READ;
    }
    else
    {
        /* Expect Status Stage Out */
        `$INSTANCE_NAME`_ep0Mode = `$INSTANCE_NAME`_MODE_STATUS_OUT_ONLY;
        /* Update the state (or stay the same) */
        `$INSTANCE_NAME`_transferState = `$INSTANCE_NAME`_TRANS_STATE_CONTROL_READ;
    }

    /* Save the packet size for next time */
    `$INSTANCE_NAME`_lastPacketSize = `$INSTANCE_NAME`_ep0Count;
}


/*******************************************************************************
* Function Name: `$INSTANCE_NAME`_InitControlRead
********************************************************************************
*
* Summary:
*  Initialize a control read transaction, usable to send data to the host.
*  The following global variables shold be initialized before this function
*  called. To send zero length packet use InitZeroLengthControlTransfer
*  function.
*
* Parameters:
*  None.
*
* Return:
*  requestHandled state.
*
* Global variables:
*  `$INSTANCE_NAME`_currentTD.count - counts of data to be sent.
*  `$INSTANCE_NAME`_currentTD.pData - data pointer.
*
* Reentrant:
*  No.
*
*******************************************************************************/
uint8 `$INSTANCE_NAME`_InitControlRead(void) `=ReentrantKeil($INSTANCE_NAME . "_InitControlRead")`
{
    uint16 xferCount;
    if(`$INSTANCE_NAME`_currentTD.count == 0)
    {
        `$INSTANCE_NAME`_InitZeroLengthControlTransfer();
    }
    else
    {
        /* Set up the state machine */
        `$INSTANCE_NAME`_transferState = `$INSTANCE_NAME`_TRANS_STATE_CONTROL_READ;
        /* Set the toggle, it gets updated in LoadEP */
        `$INSTANCE_NAME`_ep0Toggle = 0u;
        /* Initialize the Status Block */
        `$INSTANCE_NAME`_InitializeStatusBlock();
        xferCount = (((uint16)CY_GET_REG8(`$INSTANCE_NAME`_lengthHi) << 8u) | (CY_GET_REG8(`$INSTANCE_NAME`_lengthLo)));

        if (`$INSTANCE_NAME`_currentTD.count > xferCount)
        {
            `$INSTANCE_NAME`_currentTD.count = xferCount;
        }
        `$INSTANCE_NAME`_LoadEP0();
    }

    return(`$INSTANCE_NAME`_TRUE);
}


/*******************************************************************************
* Function Name: `$INSTANCE_NAME`_InitZeroLengthControlTransfer
********************************************************************************
*
* Summary:
*  Initialize a zero length data IN transfer.
*
* Parameters:
*  None.
*
* Return:
*  requestHandled state.
*
* Global variables:
*  `$INSTANCE_NAME`_ep0Toggle - set to EP0_CNT_DATA_TOGGLE
*  `$INSTANCE_NAME`_ep0Mode  - prepare for mode register content.
*  `$INSTANCE_NAME`_transferState - set to TRANS_STATE_CONTROL_READ
*  `$INSTANCE_NAME`_ep0Count - cleared, means the zero-length packet.
*  `$INSTANCE_NAME`_lastPacketSize - cleared.
*
* Reentrant:
*  No.
*
*******************************************************************************/
uint8 `$INSTANCE_NAME`_InitZeroLengthControlTransfer(void)
                                                `=ReentrantKeil($INSTANCE_NAME . "_InitZeroLengthControlTransfer")`
{
    /* Update the state */
    `$INSTANCE_NAME`_transferState = `$INSTANCE_NAME`_TRANS_STATE_CONTROL_READ;
    /* Set the data toggle */
    `$INSTANCE_NAME`_ep0Toggle = `$INSTANCE_NAME`_EP0_CNT_DATA_TOGGLE;
    /* Set the Mode Register  */
    `$INSTANCE_NAME`_ep0Mode = `$INSTANCE_NAME`_MODE_ACK_IN_STATUS_OUT;
    /* Save the packet size for next time */
    `$INSTANCE_NAME`_lastPacketSize = 0u;
    `$INSTANCE_NAME`_ep0Count = 0u;

    return(`$INSTANCE_NAME`_TRUE);
}


/*******************************************************************************
* Function Name: `$INSTANCE_NAME`_ControlReadDataStage
********************************************************************************
*
* Summary:
*  Handle the Data Stage of a control read transfer.
*
* Parameters:
*  None.
*
* Return:
*  None.
*
* Reentrant:
*  No.
*
*******************************************************************************/
void `$INSTANCE_NAME`_ControlReadDataStage(void) `=ReentrantKeil($INSTANCE_NAME . "_ControlReadDataStage")`

{
    `$INSTANCE_NAME`_LoadEP0();
}


/*******************************************************************************
* Function Name: `$INSTANCE_NAME`_ControlReadStatusStage
********************************************************************************
*
* Summary:
*  Handle the Status Stage of a control read transfer.
*
* Parameters:
*  None.
*
* Return:
*  None.
*
* Global variables:
*  `$INSTANCE_NAME`_USBFS_transferByteCount - updated with last packet size.
*  `$INSTANCE_NAME`_transferState - set to TRANS_STATE_IDLE.
*  `$INSTANCE_NAME`_ep0Mode  - set to MODE_STALL_IN_OUT.
*
* Reentrant:
*  No.
*
*******************************************************************************/
void `$INSTANCE_NAME`_ControlReadStatusStage(void) `=ReentrantKeil($INSTANCE_NAME . "_ControlReadStatusStage")`
{
    /* Update the transfer byte count */
    `$INSTANCE_NAME`_transferByteCount += `$INSTANCE_NAME`_lastPacketSize;
    /* Go Idle */
    `$INSTANCE_NAME`_transferState = `$INSTANCE_NAME`_TRANS_STATE_IDLE;
    /* Update the completion block */
    `$INSTANCE_NAME`_UpdateStatusBlock(`$INSTANCE_NAME`_XFER_STATUS_ACK);
     /* We expect no more data, so stall INs and OUTs */
    `$INSTANCE_NAME`_ep0Mode =  `$INSTANCE_NAME`_MODE_STALL_IN_OUT;
}


/*******************************************************************************
* Function Name: `$INSTANCE_NAME`_InitControlWrite
********************************************************************************
*
* Summary:
*  Initialize a control write transaction
*
* Parameters:
*  None.
*
* Return:
*  requestHandled state.
*
* Global variables:
*  `$INSTANCE_NAME`_USBFS_transferState - set to TRANS_STATE_CONTROL_WRITE
*  `$INSTANCE_NAME`_ep0Toggle - set to EP0_CNT_DATA_TOGGLE
*  `$INSTANCE_NAME`_ep0Mode  - set to MODE_ACK_OUT_STATUS_IN
*
* Reentrant:
*  No.
*
*******************************************************************************/
uint8 `$INSTANCE_NAME`_InitControlWrite(void) `=ReentrantKeil($INSTANCE_NAME . "_InitControlWrite")`
{
    uint16 xferCount;

    /* Set up the state machine */
    `$INSTANCE_NAME`_transferState = `$INSTANCE_NAME`_TRANS_STATE_CONTROL_WRITE;
    /* This migh not be necessary */
    `$INSTANCE_NAME`_ep0Toggle = `$INSTANCE_NAME`_EP0_CNT_DATA_TOGGLE;
    /* Initialize the Status Block */
    `$INSTANCE_NAME`_InitializeStatusBlock();

    xferCount = (((uint16)CY_GET_REG8(`$INSTANCE_NAME`_lengthHi) << 8u) | (CY_GET_REG8(`$INSTANCE_NAME`_lengthLo)));

    if (`$INSTANCE_NAME`_currentTD.count > xferCount)
    {
        `$INSTANCE_NAME`_currentTD.count = xferCount;
    }

    /* Expect Data or Status Stage */
    `$INSTANCE_NAME`_ep0Mode = `$INSTANCE_NAME`_MODE_ACK_OUT_STATUS_IN;

    return(`$INSTANCE_NAME`_TRUE);
}


/*******************************************************************************
* Function Name: `$INSTANCE_NAME`_ControlWriteDataStage
********************************************************************************
*
* Summary:
*  Handle the Data Stage of a control write transfer
*       1. Get the data (We assume the destination was validated previously)
*       2. Update the count and data toggle
*       3. Update the mode register for the next transaction
*
* Parameters:
*  None.
*
* Return:
*  None.
*
* Global variables:
*  `$INSTANCE_NAME`_transferByteCount - Update the transfer byte count from the
*    last transaction.
*  `$INSTANCE_NAME`_ep0Count - counts the data loaded from the SIE memory
*    in current packet.
*  `$INSTANCE_NAME`_transferByteCount - suma of the previos bytes transferred
*    on previous packets(suma of USBFS_lastPacketSize)
*  `$INSTANCE_NAME`_ep0Toggle - inverted
*  `$INSTANCE_NAME`_ep0Mode  - set to MODE_ACK_OUT_STATUS_IN.
*
* Reentrant:
*  No.
*
*******************************************************************************/
void `$INSTANCE_NAME`_ControlWriteDataStage(void) `=ReentrantKeil($INSTANCE_NAME . "_ControlWriteDataStage")`
{
    uint8 *pReg = (uint8 *)`$INSTANCE_NAME`_EP0_DR0_PTR;

    `$INSTANCE_NAME`_ep0Count = (CY_GET_REG8(`$INSTANCE_NAME`_EP0_CNT_PTR ) & 0x0Fu) - 2u;

    `$INSTANCE_NAME`_transferByteCount += `$INSTANCE_NAME`_ep0Count;

    while ((`$INSTANCE_NAME`_currentTD.count > 0u) && (`$INSTANCE_NAME`_ep0Count > 0u))
    {
        *`$INSTANCE_NAME`_currentTD.pData++ = CY_GET_REG8(pReg++);
        `$INSTANCE_NAME`_ep0Count--;
        `$INSTANCE_NAME`_currentTD.count--;
    }
    /* Update the data toggle */
    `$INSTANCE_NAME`_ep0Toggle ^= `$INSTANCE_NAME`_EP0_CNT_DATA_TOGGLE;
    /* Expect Data or Status Stage */
    `$INSTANCE_NAME`_ep0Mode = `$INSTANCE_NAME`_MODE_ACK_OUT_STATUS_IN;
}


/*******************************************************************************
* Function Name: `$INSTANCE_NAME`_ControlWriteStatusStage
********************************************************************************
*
* Summary:
*  Handle the Status Stage of a control write transfer
*
* Parameters:
*  None.
*
* Return:
*  None.
*
* Global variables:
*  `$INSTANCE_NAME`_transferState - set to TRANS_STATE_IDLE.
*  `$INSTANCE_NAME`_USBFS_ep0Mode  - set to MODE_STALL_IN_OUT.
*
* Reentrant:
*  No.
*
*******************************************************************************/
void `$INSTANCE_NAME`_ControlWriteStatusStage(void) `=ReentrantKeil($INSTANCE_NAME . "_ControlWriteStatusStage")`
{
    /* Go Idle */
    `$INSTANCE_NAME`_transferState = `$INSTANCE_NAME`_TRANS_STATE_IDLE;
    /* Update the completion block */
    `$INSTANCE_NAME`_UpdateStatusBlock(`$INSTANCE_NAME`_XFER_STATUS_ACK);
     /* We expect no more data, so stall INs and OUTs */
    `$INSTANCE_NAME`_ep0Mode = `$INSTANCE_NAME`_MODE_STALL_IN_OUT;
}


/*******************************************************************************
* Function Name: `$INSTANCE_NAME`_InitNoDataControlTransfer
********************************************************************************
*
* Summary:
*  Initialize a no data control transfer
*
* Parameters:
*  None.
*
* Return:
*  requestHandled state.
*
* Global variables:
*  `$INSTANCE_NAME`_transferState - set to TRANS_STATE_NO_DATA_CONTROL.
*  `$INSTANCE_NAME`_ep0Mode  - set to MODE_STATUS_IN_ONLY.
*  `$INSTANCE_NAME`_ep0Count - cleared.
*  `$INSTANCE_NAME`_ep0Toggle - set to EP0_CNT_DATA_TOGGLE
*
* Reentrant:
*  No.
*
*******************************************************************************/
uint8 `$INSTANCE_NAME`_InitNoDataControlTransfer(void) `=ReentrantKeil($INSTANCE_NAME . "_InitNoDataControlTransfer")`
{
    `$INSTANCE_NAME`_transferState = `$INSTANCE_NAME`_TRANS_STATE_NO_DATA_CONTROL;
    `$INSTANCE_NAME`_ep0Mode = `$INSTANCE_NAME`_MODE_STATUS_IN_ONLY;
    `$INSTANCE_NAME`_ep0Toggle = `$INSTANCE_NAME`_EP0_CNT_DATA_TOGGLE;
    `$INSTANCE_NAME`_ep0Count = 0u;

    return(`$INSTANCE_NAME`_TRUE);
}


/*******************************************************************************
* Function Name: `$INSTANCE_NAME`_NoDataControlStatusStage
********************************************************************************
* Summary:
*  Handle the Status Stage of a no data control transfer.
*
*  SET_ADDRESS is special, since we need to receive the status stage with
*  the old address.
*
* Parameters:
*  None.
*
* Return:
*  None.
*
* Global variables:
*  `$INSTANCE_NAME`_transferState - set to TRANS_STATE_IDLE.
*  `$INSTANCE_NAME`_ep0Mode  - set to MODE_STALL_IN_OUT.
*  `$INSTANCE_NAME`_ep0Toggle - set to EP0_CNT_DATA_TOGGLE
*  `$INSTANCE_NAME`_deviceAddress - used to set new address and cleared
*
* Reentrant:
*  No.
*
*******************************************************************************/
void `$INSTANCE_NAME`_NoDataControlStatusStage(void) `=ReentrantKeil($INSTANCE_NAME . "_NoDataControlStatusStage")`
{
    /* Change the USB address register if we got a SET_ADDRESS. */
    if (`$INSTANCE_NAME`_deviceAddress != 0u)
    {
        CY_SET_REG8(`$INSTANCE_NAME`_CR0_PTR, `$INSTANCE_NAME`_deviceAddress | `$INSTANCE_NAME`_CR0_ENABLE);
        `$INSTANCE_NAME`_deviceAddress = 0u;
    }
    /* Go Idle */
    `$INSTANCE_NAME`_transferState = `$INSTANCE_NAME`_TRANS_STATE_IDLE;
    /* Update the completion block */
    `$INSTANCE_NAME`_UpdateStatusBlock(`$INSTANCE_NAME`_XFER_STATUS_ACK);
     /* We expect no more data, so stall INs and OUTs */
    `$INSTANCE_NAME`_ep0Mode = `$INSTANCE_NAME`_MODE_STALL_IN_OUT;
}


/*******************************************************************************
* Function Name: `$INSTANCE_NAME`_UpdateStatusBlock
********************************************************************************
*
* Summary:
*  Update the Completion Status Block for a Request.  The block is updated
*  with the completion code the `$INSTANCE_NAME`_transferByteCount.  The
*  StatusBlock Pointer is set to NULL.
*
* Parameters:
*  completionCode - status.
*
* Return:
*  None.
*
* Global variables:
*  `$INSTANCE_NAME`_currentTD.pStatusBlock->status - updated by the 
*    completionCode parameter.
*  `$INSTANCE_NAME`_currentTD.pStatusBlock->length - updated.
*  `$INSTANCE_NAME`_currentTD.pStatusBlock - cleared.
*
* Reentrant:
*  No.
*
*******************************************************************************/
void `$INSTANCE_NAME`_UpdateStatusBlock(uint8 completionCode) `=ReentrantKeil($INSTANCE_NAME . "_UpdateStatusBlock")`
{
    if (`$INSTANCE_NAME`_currentTD.pStatusBlock != NULL)
    {
        `$INSTANCE_NAME`_currentTD.pStatusBlock->status = completionCode;
        `$INSTANCE_NAME`_currentTD.pStatusBlock->length = `$INSTANCE_NAME`_transferByteCount;
        `$INSTANCE_NAME`_currentTD.pStatusBlock = NULL;
    }
}


/*******************************************************************************
* Function Name: `$INSTANCE_NAME`_InitializeStatusBlock
********************************************************************************
*
* Summary:
*  Initialize the Completion Status Block for a Request.  The completion
*  code is set to USB_XFER_IDLE.
*
*  Also, initializes `$INSTANCE_NAME`_transferByteCount.  Save some space,
*  this is the only consumer.
*
* Parameters:
*  None.
*
* Return:
*  None.
*
* Global variables:
*  `$INSTANCE_NAME`_currentTD.pStatusBlock->status - set to XFER_IDLE.
*  `$INSTANCE_NAME`_currentTD.pStatusBlock->length - cleared.
*  `$INSTANCE_NAME`_transferByteCount - cleared.
*
* Reentrant:
*  No.
*
*******************************************************************************/
void `$INSTANCE_NAME`_InitializeStatusBlock(void) `=ReentrantKeil($INSTANCE_NAME . "_InitializeStatusBlock")`
{
    `$INSTANCE_NAME`_transferByteCount = 0u;
    if(`$INSTANCE_NAME`_currentTD.pStatusBlock != NULL)
    {
        `$INSTANCE_NAME`_currentTD.pStatusBlock->status = `$INSTANCE_NAME`_XFER_IDLE;
        `$INSTANCE_NAME`_currentTD.pStatusBlock->length = 0u;
    }
}


/* [] END OF FILE */
