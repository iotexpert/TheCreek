/*******************************************************************************
* File Name: `$INSTANCE_NAME`_drv.c
* Version `$CY_MAJOR_VERSION`.`$CY_MINOR_VERSION`
*
*  Description:
*    Endpoint 0 Driver for the USBFS Component.
*
*   Note:
*
********************************************************************************
* Copyright 2008-2009, Cypress Semiconductor Corporation.  All rights reserved.
* You may use this file only in accordance with the license, terms, conditions, 
* disclaimers, and limitations in the end user license agreement accompanying 
* the software package with which this file was provided.
********************************************************************************/



#include "cydevice.h"
#include "cyfitter.h"
#include "`$INSTANCE_NAME`.h"

/*******************************************************************************
* Forward references for the EP0 ISR
********************************************************************************/
void  `$INSTANCE_NAME`_HandleSetup(void);
void  `$INSTANCE_NAME`_HandleIN(void);
void  `$INSTANCE_NAME`_HandleOUT(void);
uint8 `$INSTANCE_NAME`_InitControlRead(void);
void  `$INSTANCE_NAME`_ControlReadDataStage(void);
void  `$INSTANCE_NAME`_ControlReadStatusStage(void);
void  `$INSTANCE_NAME`_ControlReadPrematureStatus(void);
uint8 `$INSTANCE_NAME`_InitControlWrite(void);
void  `$INSTANCE_NAME`_ControlWriteDataStage(void);
void  `$INSTANCE_NAME`_ControlWriteStatusStage(void);
void  `$INSTANCE_NAME`_ControlWritePrematureStatus(void);
uint8 `$INSTANCE_NAME`_InitNoDataControlTransfer(void);
void  `$INSTANCE_NAME`_NoDataControlStatusStage(void);
void `$INSTANCE_NAME`_InitializeStatusBlock(void);
void `$INSTANCE_NAME`_UpdateStatusBlock(uint8 bCompletionCode);

/*******************************************************************************
* Request Handlers
********************************************************************************/
uint8 `$INSTANCE_NAME`_HandleStandardRqst(void);
uint8 `$INSTANCE_NAME`_DispatchClassRqst(void);
uint8 `$INSTANCE_NAME`_HandleVendorRqst(void);

/*******************************************************************************
* External data references
********************************************************************************/

/*******************************************************************************
* Global data allocation
********************************************************************************/
T_`$INSTANCE_NAME`_EP_CTL_BLOCK `$INSTANCE_NAME`_EP[9];
uint8 `$INSTANCE_NAME`_bEPHalt;
uint8 `$INSTANCE_NAME`_bConfiguration;
uint8 `$INSTANCE_NAME`_bInterfaceSetting[9];
uint8 `$INSTANCE_NAME`_bDeviceAddress;
uint8 `$INSTANCE_NAME`_bDeviceStatus;
uint8 `$INSTANCE_NAME`_bDevice;
/*******************************************************************************
* Local data allocation
********************************************************************************/
uint8 `$INSTANCE_NAME`_bEP0Toggle;
uint8 `$INSTANCE_NAME`_bLastPacketSize;
uint8 `$INSTANCE_NAME`_bTransferState;
T_`$INSTANCE_NAME`_TD CurrentTD;
uint8 `$INSTANCE_NAME`_bEP0Mode;
uint8 `$INSTANCE_NAME`_bEP0Count;
uint16 `$INSTANCE_NAME`_TransferByteCount;

/*******************************************************************************
* Function Name: `$INSTANCE_NAME`_ep_0_Interrupt
********************************************************************************
* Summary:
*   This Interrupt Service Routine handles Endpoint 0 (Control Pipe) traffic.  It
*   dispactches setup requests and handles the data and status stages.
*   
* Parameters:  
*   None
*******************************************************************************/
CY_ISR(`$INSTANCE_NAME`_EP_0_ISR)
{
	uint8 bRegTemp = *`$INSTANCE_NAME`_EP0_CR;
		
	if (!bRegTemp & `$INSTANCE_NAME`_MODE_ACKD) return;             
	if (bRegTemp & `$INSTANCE_NAME`_MODE_SETUP_RCVD) {
		`$INSTANCE_NAME`_HandleSetup();
	} 
	else if (bRegTemp & `$INSTANCE_NAME`_MODE_IN_RCVD) {
		`$INSTANCE_NAME`_HandleIN();	
	}
	else if (bRegTemp & `$INSTANCE_NAME`_MODE_OUT_RCVD) {
		`$INSTANCE_NAME`_HandleOUT();
	}
	else {
//		ASSERT(0);
	}
	CY_SET_REG8(`$INSTANCE_NAME`_EP0_CNT, `$INSTANCE_NAME`_bEP0Toggle | `$INSTANCE_NAME`_bEP0Count);
	/* Set the Mode Register  */
	CY_SET_REG8(`$INSTANCE_NAME`_EP0_CR, `$INSTANCE_NAME`_bEP0Mode);
}
/*******************************************************************************
* Function Name: `$INSTANCE_NAME`_HandleSetup
********************************************************************************
* Summary:
*   This Routine dispatches requests for the four USB request types
*   
* Parameters:  
*   None
*******************************************************************************/
void `$INSTANCE_NAME`_HandleSetup(void)
{
	uint8 bRequestHandled;
    /* In case the previous transfer did not complete, close it out */
    `$INSTANCE_NAME`_UpdateStatusBlock(`$INSTANCE_NAME`_XFER_PREMATURE);
    
    switch (CY_GET_REG8(`$INSTANCE_NAME`_bmRequestType) & `$INSTANCE_NAME`_RQST_TYPE_MASK)
	{
	case `$INSTANCE_NAME`_RQST_TYPE_STD:
		bRequestHandled = `$INSTANCE_NAME`_HandleStandardRqst();
		break;
	case `$INSTANCE_NAME`_RQST_TYPE_CLS:
		bRequestHandled = `$INSTANCE_NAME`_DispatchClassRqst();
		break;
	case `$INSTANCE_NAME`_RQST_TYPE_VND:
		bRequestHandled = `$INSTANCE_NAME`_HandleVendorRqst();
		break;
	default:
		bRequestHandled = `$INSTANCE_NAME`_FALSE;
		break;
	}
	if (bRequestHandled == `$INSTANCE_NAME`_FALSE){
		`$INSTANCE_NAME`_bEP0Mode = `$INSTANCE_NAME`_MODE_STALL_IN_OUT;
	}
}
/*******************************************************************************
* Function Name: `$INSTANCE_NAME`_HandleIN
********************************************************************************
* Summary:
*   This routine handles EP0 IN transfers.
*   
* Parameters:  
*   None
*******************************************************************************/
void `$INSTANCE_NAME`_HandleIN(void)
{
	switch (`$INSTANCE_NAME`_bTransferState)
	{
	case `$INSTANCE_NAME`_TRANS_STATE_IDLE:
		break;
	case `$INSTANCE_NAME`_TRANS_STATE_CONTROL_READ:
		`$INSTANCE_NAME`_ControlReadDataStage();
		break;
	case `$INSTANCE_NAME`_TRANS_STATE_CONTROL_WRITE:
		`$INSTANCE_NAME`_ControlReadStatusStage();
		break;
	case `$INSTANCE_NAME`_TRANS_STATE_NO_DATA_CONTROL:
		`$INSTANCE_NAME`_NoDataControlStatusStage();
		break;
	default:
		break;	
	}
}
/*******************************************************************************
* Function Name: `$INSTANCE_NAME`_HandleOUT
********************************************************************************
* Summary:
*   This routine handles EP0 OUT transfers.
*   
* Parameters:  
*   None
*******************************************************************************/
void `$INSTANCE_NAME`_HandleOUT(void)
{
	switch (`$INSTANCE_NAME`_bTransferState)
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
        `$INSTANCE_NAME`_bEP0Mode = `$INSTANCE_NAME`_MODE_STALL_IN_OUT;
		break;
	default:
		break;	
	}
}
/*******************************************************************************
* Function Name: `$INSTANCE_NAME`_LoadEP0
********************************************************************************
* Summary:
*   This routine loads the EP0 data registers for OUT transfers.  It uses the
*   CurrentTD (previously initialized by the _InitControlWrite function and
*   updated for each OUT transfer, and the bLastPacketSize) to determine how
*   many uint8s to transfer on the current OUT.
*
*   If the number of uint8s remaining is zero and the last transfer was full, 
*   we need to send a zero length packet.  Otherwise we send the minimum
*   of the control endpoint size (8) or remaining number of uint8s for the
*   transaction.
*   
* Parameters:  
*   None
*******************************************************************************/
void `$INSTANCE_NAME`_LoadEP0(void)
{
	/* Update the transfer byte count from the last transaction */
    `$INSTANCE_NAME`_TransferByteCount += `$INSTANCE_NAME`_bLastPacketSize;
    /* Now load the next transaction */
    `$INSTANCE_NAME`_bEP0Count = 0;
	while ((CurrentTD.wCount > 0) && (`$INSTANCE_NAME`_bEP0Count < 8))
	{
        `$INSTANCE_NAME`_EP0_DR0[`$INSTANCE_NAME`_bEP0Count] = *CurrentTD.pData++;
		`$INSTANCE_NAME`_bEP0Count++;
		CurrentTD.wCount--;
	}

	if ((`$INSTANCE_NAME`_bEP0Count > 0) || (`$INSTANCE_NAME`_bLastPacketSize == 8))
	{
		/* Update the data toggle */
		`$INSTANCE_NAME`_bEP0Toggle ^= `$INSTANCE_NAME`_EP0_CNT_DATA_TOGGLE;
		/* Set the Mode Register  */
		`$INSTANCE_NAME`_bEP0Mode = `$INSTANCE_NAME`_MODE_ACK_IN_STATUS_OUT;
		/* Update the state (or stay the same) */
		`$INSTANCE_NAME`_bTransferState = `$INSTANCE_NAME`_TRANS_STATE_CONTROL_READ;
	}
	else
	{
		/* Expect Status Stage Out */
		`$INSTANCE_NAME`_bEP0Mode = `$INSTANCE_NAME`_MODE_STATUS_OUT_ONLY;
		/* Update the state (or stay the same) */
		`$INSTANCE_NAME`_bTransferState = `$INSTANCE_NAME`_TRANS_STATE_CONTROL_READ;
	}

	/* Save the packet size for next time */
	`$INSTANCE_NAME`_bLastPacketSize = `$INSTANCE_NAME`_bEP0Count;
}
/*******************************************************************************
* Function Name: `$INSTANCE_NAME`_InitControlRead
********************************************************************************
* Summary:
*   Initialize a control read transaction
*   
* Parameters:  
*   None
*******************************************************************************/
uint8 `$INSTANCE_NAME`_InitControlRead(void)
{
    uint16 wXferCount;

    /* Set up the state machine */
    `$INSTANCE_NAME`_bTransferState = `$INSTANCE_NAME`_TRANS_STATE_CONTROL_READ;
    /* Set the toggle, it gets updated in LoadEP */
    `$INSTANCE_NAME`_bEP0Toggle = 0;
    /* Initialize the Status Block */
    `$INSTANCE_NAME`_InitializeStatusBlock();
    wXferCount = ((*`$INSTANCE_NAME`_wLengthHi << 8) | (*`$INSTANCE_NAME`_wLengthLo));

    if (CurrentTD.wCount > wXferCount)
    {
        CurrentTD.wCount = wXferCount;
    }
    `$INSTANCE_NAME`_LoadEP0();

    return `$INSTANCE_NAME`_TRUE;
}
/*******************************************************************************
* Function Name: `$INSTANCE_NAME`_ControlReadDataStage
********************************************************************************
* Summary:
*   Handle the Data Stage of a control read transfer
*   
* Parameters:  
*   None
*******************************************************************************/
void `$INSTANCE_NAME`_ControlReadDataStage(void)
{
	`$INSTANCE_NAME`_LoadEP0();
}
/*******************************************************************************
* Function Name: `$INSTANCE_NAME`_ControlReadStatusStage
********************************************************************************
* Summary:
*   Handle the Status Stage of a control read transfer
*   
* Parameters:  
*   None
*******************************************************************************/
void `$INSTANCE_NAME`_ControlReadStatusStage(void)
{
	/* Update the transfer byte count */
    `$INSTANCE_NAME`_TransferByteCount += `$INSTANCE_NAME`_bLastPacketSize;
    /* Go Idle */
    `$INSTANCE_NAME`_bTransferState = `$INSTANCE_NAME`_TRANS_STATE_IDLE;
    /* Update the completion block */
    `$INSTANCE_NAME`_UpdateStatusBlock(`$INSTANCE_NAME`_XFER_STATUS_ACK);
 	/* We expect no more data, so stall INs and OUTs */
	`$INSTANCE_NAME`_bEP0Mode = `$INSTANCE_NAME`_MODE_STALL_IN_OUT;
}
/*******************************************************************************
* Function Name: `$INSTANCE_NAME`_InitControlWrite
********************************************************************************
* Summary:
*   Initialize a control write transaction
*   
* Parameters:  
*   None
*******************************************************************************/
uint8 `$INSTANCE_NAME`_InitControlWrite(void)
{
    uint16 wXferCount;
    
    /* Set up the state machine */
    `$INSTANCE_NAME`_bTransferState = `$INSTANCE_NAME`_TRANS_STATE_CONTROL_WRITE;
    /* This migh not be necessary */
    `$INSTANCE_NAME`_bEP0Toggle = `$INSTANCE_NAME`_EP0_CNT_DATA_TOGGLE;;
    /* Initialize the Status Block */
    `$INSTANCE_NAME`_InitializeStatusBlock();

    wXferCount = ((CY_GET_REG8(`$INSTANCE_NAME`_wLengthHi) << 8) | (CY_GET_REG8(`$INSTANCE_NAME`_wLengthLo)));

    if (CurrentTD.wCount > wXferCount)
    {
        CurrentTD.wCount = wXferCount;
    }
    
	/* Expect Data or Status Stage */
	`$INSTANCE_NAME`_bEP0Mode = `$INSTANCE_NAME`_MODE_ACK_OUT_STATUS_IN;
    
    return `$INSTANCE_NAME`_TRUE;
}
/*******************************************************************************
* Function Name: `$INSTANCE_NAME`_ControlWriteDataStage
********************************************************************************
* Summary:
*   Handle the Data Stage of a control write transfer
*       1. Get the data (We assume the destination was validated previously)
*       3. Update the count and data toggle
*       2. Update the mode register for the next transaction
* Parameters:  
*   None
*******************************************************************************/
void `$INSTANCE_NAME`_ControlWriteDataStage(void)
{
	uint8 *pReg = `$INSTANCE_NAME`_EP0_DR0; 
	`$INSTANCE_NAME`_bEP0Count = (CY_GET_REG8(`$INSTANCE_NAME`_EP0_CNT ) & 0x0F) - 2;
    
    `$INSTANCE_NAME`_TransferByteCount += `$INSTANCE_NAME`_bEP0Count;
    
    while ((CurrentTD.wCount > 0) && (`$INSTANCE_NAME`_bEP0Count > 0))
	{
        *CurrentTD.pData++ = CY_GET_REG8(pReg++);
        `$INSTANCE_NAME`_bEP0Count--;
		CurrentTD.wCount--;
	}
	/* Update the data toggle */
	`$INSTANCE_NAME`_bEP0Toggle ^= `$INSTANCE_NAME`_EP0_CNT_DATA_TOGGLE;
	/* Expect Data or Status Stage */
	`$INSTANCE_NAME`_bEP0Mode = `$INSTANCE_NAME`_MODE_ACK_OUT_STATUS_IN;
}
/*******************************************************************************
* Function Name: `$INSTANCE_NAME`_ControlWriteStatusStage
********************************************************************************
* Summary:
*   Handle the Status Stage of a control write transfer
*   
* Parameters:  
*   None
*******************************************************************************/
void `$INSTANCE_NAME`_ControlWriteStatusStage(void)
{
	/* Go Idle */
    `$INSTANCE_NAME`_bTransferState = `$INSTANCE_NAME`_TRANS_STATE_IDLE;
    /* Update the completion block */
    `$INSTANCE_NAME`_UpdateStatusBlock(`$INSTANCE_NAME`_XFER_STATUS_ACK);
 	/* We expect no more data, so stall INs and OUTs */
	`$INSTANCE_NAME`_bEP0Mode = `$INSTANCE_NAME`_MODE_STALL_IN_OUT;
}
/*******************************************************************************
* Function Name: `$INSTANCE_NAME`_InitNoDataControlTransfer
********************************************************************************
* Summary:
*   Initialize a no data control transfer
*   
* Parameters:  
*   None
*******************************************************************************/
uint8 `$INSTANCE_NAME`_InitNoDataControlTransfer(void)
{
	`$INSTANCE_NAME`_bTransferState = `$INSTANCE_NAME`_TRANS_STATE_NO_DATA_CONTROL;
	`$INSTANCE_NAME`_bEP0Mode = `$INSTANCE_NAME`_MODE_STATUS_IN_ONLY;
	return `$INSTANCE_NAME`_TRUE;
}
/*******************************************************************************
* Function Name: `$INSTANCE_NAME`_NoDataControlStatusStage
********************************************************************************
* Summary:
*   Handle the Status Stage of a no data control transfer.
*
*   SET_ADDRESS is special, since we need to receive the status stage with
*   the old address.
*   
* Parameters:  
*   None
*******************************************************************************/
void `$INSTANCE_NAME`_NoDataControlStatusStage(void)
{
	/* Change the USB address register if we got a SET_ADDRESS. */
    if (`$INSTANCE_NAME`_bDeviceAddress != 0)
    {
        CY_SET_REG8(`$INSTANCE_NAME`_CR0, `$INSTANCE_NAME`_bDeviceAddress | `$INSTANCE_NAME`_CR0_ENABLE);
        `$INSTANCE_NAME`_bDeviceAddress = 0;
    }
    	/* Go Idle */
    `$INSTANCE_NAME`_bTransferState = `$INSTANCE_NAME`_TRANS_STATE_IDLE;
    /* Update the completion block */
    `$INSTANCE_NAME`_UpdateStatusBlock(`$INSTANCE_NAME`_XFER_STATUS_ACK);
 	/* We expect no more data, so stall INs and OUTs */
	`$INSTANCE_NAME`_bEP0Mode = `$INSTANCE_NAME`_MODE_STALL_IN_OUT;
}
/*******************************************************************************
* Function Name: `$INSTANCE_NAME`_UpdateStatusBlock
********************************************************************************
* Summary:
*   Update the Completion Status Block for a Request.  The block is updated
*   with the completion code the `$INSTANCE_NAME`_TransferByteCount.  The
*   StatusBlock Pointer  is set to NULL.
*   
* Parameters:  
*   None
*******************************************************************************/
void `$INSTANCE_NAME`_UpdateStatusBlock(uint8 bCompletionCode)
{
    if (CurrentTD.pStatusBlock != `$INSTANCE_NAME`_NULL)
    {
        CurrentTD.pStatusBlock->bStatus = bCompletionCode;
        CurrentTD.pStatusBlock->wLength = `$INSTANCE_NAME`_TransferByteCount;
        CurrentTD.pStatusBlock = `$INSTANCE_NAME`_NULL;
    }
}
/*******************************************************************************
* Function Name: `$INSTANCE_NAME`_InitializeStatusBlock
********************************************************************************
* Summary:
*   Initialize the Completion Status Block for a Request.  The completion
*   code is set to USB_XFER_IDLE.
*
*   Also, initializes `$INSTANCE_NAME`_TransferByteCount.  Save some space,
*   this is the only consumer.
*   
* Parameters:  
*   None
*******************************************************************************/
void `$INSTANCE_NAME`_InitializeStatusBlock(void)
{
    `$INSTANCE_NAME`_TransferByteCount = 0;
    if (CurrentTD.pStatusBlock != `$INSTANCE_NAME`_NULL)
    {
        CurrentTD.pStatusBlock->bStatus = `$INSTANCE_NAME`_XFER_IDLE;
        CurrentTD.pStatusBlock->wLength = 0;
    }
}