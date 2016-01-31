/*******************************************************************************
* File Name: `$INSTANCE_NAME`_std.c
* Version `$CY_MAJOR_VERSION`.`$CY_MINOR_VERSION`
*
*  Description:
*    USB Standard request handler.
*
*   Note:
*
********************************************************************************
* Copyright 2008-2009, Cypress Semiconductor Corporation.  All rights reserved.
* You may use this file only in accordance with the license, terms, conditions, 
* disclaimers, and limitations in the end user license agreement accompanying 
* the software package with which this file was provided.
********************************************************************************/



#include "cydevice_trm.h"
#include "cyfitter.h"
#include "`$INSTANCE_NAME`.h"

/*******************************************************************************
* External references
********************************************************************************/
uint8 `$INSTANCE_NAME`_InitControlRead(void);
uint8 `$INSTANCE_NAME`_InitControlWrite(void);
uint8 `$INSTANCE_NAME`_InitNoDataControlTransfer(void);
uint8 `$INSTANCE_NAME`_DispatchClassRqst(void);

extern uint8 `$INSTANCE_NAME`_CODE `$INSTANCE_NAME`_DEVICE0_DESCR[];
extern uint8 `$INSTANCE_NAME`_CODE `$INSTANCE_NAME`_DEVICE0_CONFIGURATION0_DESCR[]; 
extern uint8 `$INSTANCE_NAME`_CODE `$INSTANCE_NAME`_STRING_DESCRIPTORS[];
extern uint8 `$INSTANCE_NAME`_CODE `$INSTANCE_NAME`_MSOS_DESCRIPTOR[];
extern uint8 `$INSTANCE_NAME`_CODE `$INSTANCE_NAME`_SN_STRING_DESCRIPTOR[];


extern uint8 `$INSTANCE_NAME`_bEPHalt;
extern uint8 `$INSTANCE_NAME`_bDevice;
extern uint8 `$INSTANCE_NAME`_bConfiguration;
extern uint8 `$INSTANCE_NAME`_bInterfaceSetting[];
extern uint8 `$INSTANCE_NAME`_bDeviceAddress;
extern uint8 `$INSTANCE_NAME`_bDeviceStatus;
extern T_`$INSTANCE_NAME`_LUT `$INSTANCE_NAME`_CODE `$INSTANCE_NAME`_TABLE[];
extern T_`$INSTANCE_NAME`_EP_CTL_BLOCK `$INSTANCE_NAME`_EP[];
extern T_`$INSTANCE_NAME`_TD CurrentTD;
uint8 tBuffer[2];
uint8 *`$INSTANCE_NAME`_bFWSerialNumberStringDescriptor;
uint8 `$INSTANCE_NAME`_bSNStringConfirm = 0;

/*DIE ID string descriptor for 8 bytes ID*/
#if defined(`$INSTANCE_NAME`_ENABLE_IDSN_STRING)
void `$INSTANCE_NAME`_ReadDieID(uint8 *descr);
uint8 `$INSTANCE_NAME`_bIDSerialNumberStringDescriptor[0x22]={0x22,0x03};
#endif	

/*******************************************************************************
* Forward references
********************************************************************************/
void `$INSTANCE_NAME`_Config(void);
T_`$INSTANCE_NAME`_LUT *`$INSTANCE_NAME`_GetConfigTablePtr(uint8 c);
T_`$INSTANCE_NAME`_LUT *`$INSTANCE_NAME`_GetDeviceTablePtr(void);
uint8 `$INSTANCE_NAME`_ClearEndpointHalt(void);
uint8 `$INSTANCE_NAME`_SetEndpointHalt(void);
uint8 `$INSTANCE_NAME`_ValidateAlternateSetting(void);
//uint8 `$INSTANCE_NAME`_GetDeviceStatus(void);

/*******************************************************************************
* Function Name: `$INSTANCE_NAME`_SerialNumString
********************************************************************************
* Summary:
*   Application firmware may supply the source of the USB device descriptors serial 
*   number string during runtime.
*   
* Parameters:  
*   SNstring - pointer to string
*******************************************************************************/
void  `$INSTANCE_NAME`_SerialNumString(uint8 *SNstring)
{
#if defined(`$INSTANCE_NAME`_ENABLE_FWSN_STRING)
    `$INSTANCE_NAME`_bFWSerialNumberStringDescriptor = SNstring;
	/* check descriptor validation */
	if( (`$INSTANCE_NAME`_bFWSerialNumberStringDescriptor[0] > 1 ) &&   /* Descriptor Length */
	    (`$INSTANCE_NAME`_bFWSerialNumberStringDescriptor[1] == 3) )    /* DescriptorType: STRING */    
	{
	    `$INSTANCE_NAME`_bSNStringConfirm = 1;
	}
    else
    {
	    `$INSTANCE_NAME`_bSNStringConfirm = 0;
    }
#else
    SNstring = SNstring;
#endif     
}

/*******************************************************************************
* Function Name: `$INSTANCE_NAME`_HandleStandardRqst
********************************************************************************
* Summary:
*   This Routine dispatches standard requests
*   
* Parameters:  
*   None
*******************************************************************************/
uint8 `$INSTANCE_NAME`_HandleStandardRqst()
{
    uint8 bRequestHandled = `$INSTANCE_NAME`_FALSE;
#if defined(`$INSTANCE_NAME`_ENABLE_STRINGS)
    uint8 *pStr;
  #if defined(`$INSTANCE_NAME`_ENABLE_DESCRIPTOR_STRINGS)
    uint8 nStr;
  #endif
#endif
    uint16 wCount;
    
    T_`$INSTANCE_NAME`_LUT *pTmp;
    CurrentTD.wCount = 0;
    
    if ((CY_GET_REG8(`$INSTANCE_NAME`_bmRequestType) & `$INSTANCE_NAME`_RQST_DIR_MASK) == `$INSTANCE_NAME`_RQST_DIR_D2H)
    {
        /* Control Read */
        switch (CY_GET_REG8(`$INSTANCE_NAME`_bRequest)) 
        {
        case `$INSTANCE_NAME`_GET_DESCRIPTOR:
            if (CY_GET_REG8(`$INSTANCE_NAME`_wValueHi) == `$INSTANCE_NAME`_DESCR_DEVICE)  
            {
                pTmp = `$INSTANCE_NAME`_GetDeviceTablePtr();
                CurrentTD.pData     = pTmp->p_list;
                CurrentTD.wCount    = 18;
                bRequestHandled     = `$INSTANCE_NAME`_InitControlRead();
                }
            else if (CY_GET_REG8(`$INSTANCE_NAME`_wValueHi) == `$INSTANCE_NAME`_DESCR_CONFIG)  {
                pTmp = `$INSTANCE_NAME`_GetConfigTablePtr(CY_GET_REG8(`$INSTANCE_NAME`_wValueLo));
                CurrentTD.pData     = pTmp->p_list;
                wCount    = ((uint16)(CurrentTD.pData)[3] << 8) | (CurrentTD.pData)[2];
                CurrentTD.wCount    = wCount;
                bRequestHandled     = `$INSTANCE_NAME`_InitControlRead();
            }
            #if defined(`$INSTANCE_NAME`_ENABLE_STRINGS)
                else if (CY_GET_REG8(`$INSTANCE_NAME`_wValueHi) == `$INSTANCE_NAME`_DESCR_STRING)
                {
                /* Descriptor Strings*/	
                #if defined(`$INSTANCE_NAME`_ENABLE_DESCRIPTOR_STRINGS)
                    nStr = 0;
                    pStr = &`$INSTANCE_NAME`_STRING_DESCRIPTORS[0];
                    while ((CY_GET_REG8(`$INSTANCE_NAME`_wValueLo) > nStr) && (*pStr != 0)){
                        pStr += *pStr;
                        nStr++;
                    };
                #endif /* End `$INSTANCE_NAME`_ENABLE_DESCRIPTOR_STRINGS */
                /* Microsoft OS String*/	
                #if defined(`$INSTANCE_NAME`_ENABLE_MSOS_STRING)
                    if( CY_GET_REG8(`$INSTANCE_NAME`_wValueLo) == 0xEE )
                    {
                        pStr = &`$INSTANCE_NAME`_MSOS_DESCRIPTOR[0];
                    }
                #endif /* End `$INSTANCE_NAME`_ENABLE_MSOS_STRING*/
                /* SN string*/
                #if defined(`$INSTANCE_NAME`_ENABLE_SN_STRING)
                    if( CY_GET_REG8(`$INSTANCE_NAME`_wValueLo) == 
                        `$INSTANCE_NAME`_DEVICE0_DESCR[`$INSTANCE_NAME`_DEVICE_DESCR_SN_SHIFT] )
                    {
                        pStr = &`$INSTANCE_NAME`_SN_STRING_DESCRIPTOR[0];
                        if(`$INSTANCE_NAME`_bSNStringConfirm != 0)
                        {
                            pStr = `$INSTANCE_NAME`_bFWSerialNumberStringDescriptor;
                        }	
                        #if defined(`$INSTANCE_NAME`_ENABLE_IDSN_STRING)
                        /* TODO: read DIE ID and genarete string descriptor in RAM*/
                            `$INSTANCE_NAME`_ReadDieID(`$INSTANCE_NAME`_bIDSerialNumberStringDescriptor);
                            pStr = `$INSTANCE_NAME`_bIDSerialNumberStringDescriptor;
                        #endif	
                    }
                #endif	/* End `$INSTANCE_NAME`_ENABLE_SN_STRING */ 
                    if (*pStr != 0)
                    {
                        CurrentTD.wCount    = *pStr;
                        CurrentTD.pData     = pStr;
                        bRequestHandled     = `$INSTANCE_NAME`_InitControlRead();
                    }
                }
            #endif /* End `$INSTANCE_NAME`_ENABLE_STRINGS*/
            else
            {
                bRequestHandled = `$INSTANCE_NAME`_DispatchClassRqst();
            }
            break;
        case `$INSTANCE_NAME`_GET_STATUS:
            switch ((CY_GET_REG8(`$INSTANCE_NAME`_bmRequestType) & `$INSTANCE_NAME`_RQST_RCPT_MASK))
            {
            case `$INSTANCE_NAME`_RQST_RCPT_EP:
                    CurrentTD.wCount = 1;
                    CurrentTD.pData  = &`$INSTANCE_NAME`_EP[CY_GET_REG8(`$INSTANCE_NAME`_wIndexLo) & 0x07].bHWEPState;
                    bRequestHandled  = `$INSTANCE_NAME`_InitControlRead();
                break;
            case `$INSTANCE_NAME`_RQST_RCPT_DEV:
                CurrentTD.wCount    = 2;
                tBuffer[0] = `$INSTANCE_NAME`_bDeviceStatus;
                tBuffer[1] = 0;
                CurrentTD.pData     = &tBuffer[0];
                bRequestHandled     = `$INSTANCE_NAME`_InitControlRead();
                break;
            }
            break;
        case `$INSTANCE_NAME`_GET_CONFIGURATION:
            CurrentTD.wCount    = 1;
            CurrentTD.pData     = &`$INSTANCE_NAME`_bConfiguration;
            bRequestHandled     = `$INSTANCE_NAME`_InitControlRead();
            break;
        default:
            break;
        }
    }
    else {
        /* Control Write */
        switch (CY_GET_REG8(`$INSTANCE_NAME`_bRequest)) 
        {
        case `$INSTANCE_NAME`_SET_ADDRESS:
            `$INSTANCE_NAME`_bDeviceAddress = (CY_GET_REG8(`$INSTANCE_NAME`_wValueLo));
            bRequestHandled = `$INSTANCE_NAME`_InitNoDataControlTransfer();
            break;
        case `$INSTANCE_NAME`_SET_CONFIGURATION:
            `$INSTANCE_NAME`_bConfiguration	= CY_GET_REG8(`$INSTANCE_NAME`_wValueLo);
            `$INSTANCE_NAME`_Config();
            bRequestHandled	= `$INSTANCE_NAME`_InitNoDataControlTransfer();
            break;
		case `$INSTANCE_NAME`_SET_INTERFACE:
			if (`$INSTANCE_NAME`_ValidateAlternateSetting())
			{
	            bRequestHandled	= `$INSTANCE_NAME`_InitNoDataControlTransfer();
			}
			break;
		case `$INSTANCE_NAME`_CLEAR_FEATURE:
            switch ((CY_GET_REG8(`$INSTANCE_NAME`_bmRequestType) & `$INSTANCE_NAME`_RQST_RCPT_MASK))
            {
            case `$INSTANCE_NAME`_RQST_RCPT_EP:
                if (CY_GET_REG8(`$INSTANCE_NAME`_wValueLo) ==`$INSTANCE_NAME`_ENDPOINT_HALT)
                {
                    bRequestHandled	= `$INSTANCE_NAME`_ClearEndpointHalt();
                }
                break;
            case `$INSTANCE_NAME`_RQST_RCPT_DEV:
                /* Clear device REMOTE_WAKEUP */
                `$INSTANCE_NAME`_bDeviceStatus &= ~`$INSTANCE_NAME`_DEVICE_STATUS_REMOTE_WAKEUP;
                bRequestHandled	= `$INSTANCE_NAME`_InitNoDataControlTransfer();
                break;
            }
            break;
		case `$INSTANCE_NAME`_SET_FEATURE:
            switch ((CY_GET_REG8(`$INSTANCE_NAME`_bmRequestType) & `$INSTANCE_NAME`_RQST_RCPT_MASK))
            {
            case `$INSTANCE_NAME`_RQST_RCPT_EP:
                if (CY_GET_REG8(`$INSTANCE_NAME`_wValueLo) ==`$INSTANCE_NAME`_ENDPOINT_HALT)
                {
                    bRequestHandled	= `$INSTANCE_NAME`_SetEndpointHalt();
                }
                break;
            case `$INSTANCE_NAME`_RQST_RCPT_DEV:
                /* Clear device REMOTE_WAKEUP */
                `$INSTANCE_NAME`_bDeviceStatus &= ~`$INSTANCE_NAME`_DEVICE_STATUS_REMOTE_WAKEUP;
                bRequestHandled	= `$INSTANCE_NAME`_InitNoDataControlTransfer();
                break;
            }
        default:
            break;
        }
	}
	return bRequestHandled;
}    

#if defined(`$INSTANCE_NAME`_ENABLE_IDSN_STRING)


    /***************************************************************************
    * Function Name: `$INSTANCE_NAME`_ReadDieID
    ****************************************************************************
    * Summary:
    *   This routine read Die ID and genarete Serian Number string descriptor
    *   
    * Parameters:  
    *   uint8 *descr - pointer on string descriptor
    ***************************************************************************/
    void `$INSTANCE_NAME`_ReadDieID(uint8 *descr)
    {
        uint8 i;
        /* check descriptor validation */
        if( (descr[0] > 1 ) &&   /* Descriptor Length */
            (descr[1] == 3) )    /* DescriptorType: STRING */    
        {
            /* fill descriptor by counter for test only*/
            for(i=2; i < descr[0]; i+=2)
            descr[i] = 0x30 + ((i/2) & 0x7);
        }
    }

#endif /* End $INSTANCE_NAME`_ENABLE_IDSN_STRING*/


/*******************************************************************************
* Function Name: `$INSTANCE_NAME`_Config
********************************************************************************
* Summary:
*   This routine configures endpoints for the entire configuration by scanning
*   the configuration descriptor.  It configures the bAlternateSetting 0 for
*   each interface.
*   
* Parameters:  
*   None
*******************************************************************************/
void `$INSTANCE_NAME`_Config()
{
    uint8 ep,cur_ep,i;
    uint8 iso;
    uint16 wCount;
    T_`$INSTANCE_NAME`_LUT *pTmp;
    T_`$INSTANCE_NAME`_EP_SETTINGS_BLOCK *pEP;

    /* Clear all of the endpoints */
    for (ep = 0; ep < 9; ep++)
    {
        `$INSTANCE_NAME`_EP[ep].bAttrib = 0;
        `$INSTANCE_NAME`_EP[ep].bHWEPState = 0;
        `$INSTANCE_NAME`_EP[ep].bAPIEPState = `$INSTANCE_NAME`_NO_EVENT_PENDING;
        `$INSTANCE_NAME`_EP[ep].bEPToggle = 0;
        `$INSTANCE_NAME`_EP[ep].bEPMode = 0;
        `$INSTANCE_NAME`_EP[ep].wBufferSize = 0;
		`$INSTANCE_NAME`_bInterfaceSetting[ep] = 0x00;
    }

    pTmp = `$INSTANCE_NAME`_GetConfigTablePtr(`$INSTANCE_NAME`_bConfiguration - 1);
    pTmp++;
    ep = pTmp->c;                                       /* For this table, c is the number of endpoints  */
    pEP = (T_`$INSTANCE_NAME`_EP_SETTINGS_BLOCK *) pTmp->p_list;     /* and p_list points the endpoint setting table. */
    
    for (i = 0; i < ep; i++, pEP++)
    {
        cur_ep = pEP->bAddr; 
        cur_ep &= 0x0F;
        iso  = ((pEP->bmAttributes & `$INSTANCE_NAME`_EP_TYPE_MASK) == `$INSTANCE_NAME`_EP_TYPE_ISOC);
        if (pEP->bAddr & `$INSTANCE_NAME`_DIR_IN)
        {
        /* IN Endpoint */
            `$INSTANCE_NAME`_EP[cur_ep].bAPIEPState = `$INSTANCE_NAME`_EVENT_PENDING;
            `$INSTANCE_NAME`_EP[cur_ep].bEPMode = (iso ? `$INSTANCE_NAME`_MODE_ISO_IN : `$INSTANCE_NAME`_MODE_ACK_IN);
        }
        else
        {
        /* OUT Endpoint */
            `$INSTANCE_NAME`_EP[cur_ep].bAPIEPState = `$INSTANCE_NAME`_NO_EVENT_PENDING;
            `$INSTANCE_NAME`_EP[cur_ep].bEPMode = (iso ? `$INSTANCE_NAME`_MODE_ISO_OUT : `$INSTANCE_NAME`_MODE_ACK_OUT);
        }
        `$INSTANCE_NAME`_EP[cur_ep].wBufferSize = pEP->wBufferSize;
        `$INSTANCE_NAME`_EP[cur_ep].bAddr = pEP->bAddr;
    } 

    /* Set the endpoint buffer addresses */
    wCount = 0;
    ep = 1;
    for (i = 0; i < 0x80; i+= 0x10)
    {
        CY_SET_REG8(&`$INSTANCE_NAME`_ARB_EP1_CFG[i], `$INSTANCE_NAME`_ARB_EPX_CFG_CRC_BYPASS);
        
        
        CY_SET_REG8(&`$INSTANCE_NAME`_SIE_EP1_CR0[i], `$INSTANCE_NAME`_MODE_NAK_IN_OUT);

        `$INSTANCE_NAME`_EP[ep].wBuffOffset = wCount;
        wCount += `$INSTANCE_NAME`_EP[ep].wBufferSize;
        CY_SET_REG8(&`$INSTANCE_NAME`_SIE_EP1_CNT0[i],  (`$INSTANCE_NAME`_EP[ep].wBufferSize >> 8));
        CY_SET_REG8(&`$INSTANCE_NAME`_SIE_EP1_CNT1[i],  `$INSTANCE_NAME`_EP[ep].wBufferSize & 0xFFu);

        CY_SET_REG8(&`$INSTANCE_NAME`_ARB_RW1_RA[i],        `$INSTANCE_NAME`_EP[ep].wBuffOffset & 0xFFu);
        CY_SET_REG8(&`$INSTANCE_NAME`_ARB_RW1_RA_MSB[i],    (`$INSTANCE_NAME`_EP[ep].wBuffOffset >> 8));
        CY_SET_REG8(&`$INSTANCE_NAME`_ARB_RW1_WA[i],        `$INSTANCE_NAME`_EP[ep].wBuffOffset & 0xFFu);
        CY_SET_REG8(&`$INSTANCE_NAME`_ARB_RW1_WA_MSB[i],    (`$INSTANCE_NAME`_EP[ep].wBuffOffset >> 8));
        
        ep++;
    }
    CY_SET_REG8(`$INSTANCE_NAME`_SIE_EP_INT_EN, 0xFF);
}


/*******************************************************************************
* Function Name: `$INSTANCE_NAME`_GetConfigTablePtr
********************************************************************************
* Summary:
*   This routine returns a pointer a configuration table entry
*   
* Parameters:  
*   c Configuration Index
*******************************************************************************/
T_`$INSTANCE_NAME`_LUT *`$INSTANCE_NAME`_GetConfigTablePtr(uint8 c)
{
    /* Device Table */
    T_`$INSTANCE_NAME`_LUT *pTmp = `$INSTANCE_NAME`_GetDeviceTablePtr();

    /* The first entry points to the Device Descriptor, the the configuration entries  */
    return pTmp[c + 1].p_list;
}


/*******************************************************************************
* Function Name: `$INSTANCE_NAME`_GetDeviceTablePtr
********************************************************************************
* Summary:
*   This routine returns a pointer to the current config table based on the
*   selector in the request wValueLo
*   
* Parameters:  
*   None
*******************************************************************************/
T_`$INSTANCE_NAME`_LUT *`$INSTANCE_NAME`_GetDeviceTablePtr()
{
    /* Device Table */
    return `$INSTANCE_NAME`_TABLE[`$INSTANCE_NAME`_bDevice].p_list;
}


/*******************************************************************************
* Function Name: `$INSTANCE_NAME`_SetEndpointHalt
********************************************************************************
* Summary:
*   This routine handles set endpoint halt
*   
* Parameters:  
*   None
*******************************************************************************/
uint8 `$INSTANCE_NAME`_SetEndpointHalt()
{
    uint8 ep, ri;
    uint8 bRequestHandled = `$INSTANCE_NAME`_FALSE;

    /* Clear endpoint halt */
    ep = (CY_GET_REG8(`$INSTANCE_NAME`_wIndexLo) & 0x07);
    ri = ((ep - 1) << 4);
    if (ep != 0)
    {
        /* Set the endpoint Halt */
        `$INSTANCE_NAME`_EP[ep].bHWEPState |= (`$INSTANCE_NAME`_ENDPOINT_STATUS_HALT);
        
        /* Clear the data toggle */
        `$INSTANCE_NAME`_EP[ep].bEPToggle = 0;
        
        if (`$INSTANCE_NAME`_EP[ep].bAddr & `$INSTANCE_NAME`_DIR_IN)
        {
            /* IN Endpoint */
            `$INSTANCE_NAME`_EP[ep].bAPIEPState = `$INSTANCE_NAME`_NO_EVENT_ALLOWED;
            CY_SET_REG8(&`$INSTANCE_NAME`_SIE_EP1_CR0[ri], (`$INSTANCE_NAME`_MODE_STALL_DATA_EP | 
                                                            `$INSTANCE_NAME`_MODE_ACK_IN));
        }
        else
        {
            /* OUT Endpoint */
            `$INSTANCE_NAME`_EP[ep].bAPIEPState = `$INSTANCE_NAME`_NO_EVENT_ALLOWED;
            CY_SET_REG8(&`$INSTANCE_NAME`_SIE_EP1_CR0[ri], (`$INSTANCE_NAME`_MODE_STALL_DATA_EP | 
                                                            `$INSTANCE_NAME`_MODE_ACK_OUT));
        }
        bRequestHandled = `$INSTANCE_NAME`_InitNoDataControlTransfer();
    }
    return bRequestHandled;
}


/*******************************************************************************
* Function Name: `$INSTANCE_NAME`_ClearEndpointHalt
********************************************************************************
* Summary:
*   This routine handles clear endpoint halt
*   
* Parameters:  
*   None
*******************************************************************************/
uint8 `$INSTANCE_NAME`_ClearEndpointHalt()
{
    uint8 ep, ri;
    uint8 bRequestHandled = `$INSTANCE_NAME`_FALSE;

    /* Clear endpoint halt */
    ep = (CY_GET_REG8(`$INSTANCE_NAME`_wIndexLo) & 0x07);
    ri = ((ep  - 1) << 4);

    if (ep != 0)
    {
        /* Set the endpoint Halt */
        `$INSTANCE_NAME`_EP[ep].bHWEPState &= ~(`$INSTANCE_NAME`_ENDPOINT_STATUS_HALT);
        
        /* Clear the data toggle */
        `$INSTANCE_NAME`_EP[ep].bEPToggle = 0;
        
        if (`$INSTANCE_NAME`_EP[ep].bAddr & `$INSTANCE_NAME`_DIR_IN)
        {
            /* IN Endpoint */
            `$INSTANCE_NAME`_EP[ep].bAPIEPState = `$INSTANCE_NAME`_NO_EVENT_PENDING;
            CY_SET_REG8(&`$INSTANCE_NAME`_SIE_EP1_CR0[ri], `$INSTANCE_NAME`_MODE_NAK_IN);
        }
        else
        {
            /* OUT Endpoint */
            `$INSTANCE_NAME`_EP[ep].bAPIEPState = `$INSTANCE_NAME`_EVENT_PENDING;
            CY_SET_REG8(&`$INSTANCE_NAME`_SIE_EP1_CR0[ri], `$INSTANCE_NAME`_MODE_ACK_OUT);
        }
        bRequestHandled = `$INSTANCE_NAME`_InitNoDataControlTransfer();
    }
    return bRequestHandled;
}


/*******************************************************************************
* Function Name: `$INSTANCE_NAME`_ValidateAlternateSetting
********************************************************************************
* Summary:
*   Validates (and records) a SET INTERFACE request
*   
* Parameters:  
*   None
*******************************************************************************/
uint8 `$INSTANCE_NAME`_ValidateAlternateSetting(void)
{
    uint8 bRequestHandled = `$INSTANCE_NAME`_TRUE;

	// TODO: Validate interface setting, stall if invalid.
	`$INSTANCE_NAME`_bInterfaceSetting[CY_GET_REG8(`$INSTANCE_NAME`_wIndexLo)] = CY_GET_REG8(`$INSTANCE_NAME`_wValueLo);
	return bRequestHandled;
}
