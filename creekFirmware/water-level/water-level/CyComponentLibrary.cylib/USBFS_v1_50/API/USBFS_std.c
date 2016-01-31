/*******************************************************************************
* File Name: `$INSTANCE_NAME`_std.c
* Version `$CY_MAJOR_VERSION`.`$CY_MINOR_VERSION`
*
* Description:
*  USB Standard request handler.
*
* Note:
*
********************************************************************************
* Copyright 2008-2010, Cypress Semiconductor Corporation.  All rights reserved.
* You may use this file only in accordance with the license, terms, conditions,
* disclaimers, and limitations in the end user license agreement accompanying
* the software package with which this file was provided.
********************************************************************************/

#include "`$INSTANCE_NAME`.h"


/***************************************
* External references
***************************************/

extern uint8 CYCODE `$INSTANCE_NAME`_DEVICE0_DESCR[];
extern uint8 CYCODE `$INSTANCE_NAME`_DEVICE0_CONFIGURATION0_DESCR[];
extern uint8 CYCODE `$INSTANCE_NAME`_STRING_DESCRIPTORS[];
extern uint8 CYCODE `$INSTANCE_NAME`_MSOS_DESCRIPTOR[];
extern uint8 CYCODE `$INSTANCE_NAME`_SN_STRING_DESCRIPTOR[];

extern volatile uint8 `$INSTANCE_NAME`_epHalt;
extern volatile uint8 `$INSTANCE_NAME`_device;
extern volatile uint8 `$INSTANCE_NAME`_configuration;
extern volatile uint8 `$INSTANCE_NAME`_interfaceSetting[];
extern volatile uint8 `$INSTANCE_NAME`_deviceAddress;
extern volatile uint8 `$INSTANCE_NAME`_deviceStatus;
extern T_`$INSTANCE_NAME`_LUT CYCODE `$INSTANCE_NAME`_TABLE[];
extern volatile T_`$INSTANCE_NAME`_EP_CTL_BLOCK `$INSTANCE_NAME`_EP[];
extern volatile T_`$INSTANCE_NAME`_TD currentTD;


/***************************************
*         Forward references
***************************************/

uint8 `$INSTANCE_NAME`_InitControlRead(void);
uint8 `$INSTANCE_NAME`_InitControlWrite(void);
uint8 `$INSTANCE_NAME`_InitNoDataControlTransfer(void);
uint8 `$INSTANCE_NAME`_DispatchClassRqst(void);

void `$INSTANCE_NAME`_Config(uint8 clearAltSetting);
T_`$INSTANCE_NAME`_LUT *`$INSTANCE_NAME`_GetConfigTablePtr(uint8 c) \
                                                            `=ReentrantKeil($INSTANCE_NAME . "_GetConfigTablePtr")`;
T_`$INSTANCE_NAME`_LUT *`$INSTANCE_NAME`_GetDeviceTablePtr(void) \
                                                            `=ReentrantKeil($INSTANCE_NAME . "_GetDeviceTablePtr")`;
uint8 `$INSTANCE_NAME`_ClearEndpointHalt(void);
uint8 `$INSTANCE_NAME`_SetEndpointHalt(void);
uint8 `$INSTANCE_NAME`_ValidateAlternateSetting(void);

/*DIE ID string descriptor for 8 bytes ID*/
#if defined(`$INSTANCE_NAME`_ENABLE_IDSN_STRING)
    void `$INSTANCE_NAME`_ReadDieID(uint8 *descr);
    uint8 `$INSTANCE_NAME`_idSerialNumberStringDescriptor[0x22u]={0x22u, `$INSTANCE_NAME`_DESCR_STRING};
#endif /* `$INSTANCE_NAME`_ENABLE_IDSN_STRING */


/***************************************
* Global data allocation
***************************************/

uint8 tBuffer[2u];
uint8 *`$INSTANCE_NAME`_fwSerialNumberStringDescriptor;
uint8 `$INSTANCE_NAME`_snStringConfirm = `$INSTANCE_NAME`_FALSE;


/*******************************************************************************
* Function Name: `$INSTANCE_NAME`_SerialNumString
********************************************************************************
*
* Summary:
*  Application firmware may supply the source of the USB device descriptors 
*  serial number string during runtime.
*
* Parameters:
*  snString:  pointer to string.
*
* Return:
*  None.
*
* Reentrant:
*  No.
*
*******************************************************************************/
void  `$INSTANCE_NAME`_SerialNumString(uint8 *snString)
{
    #if defined(`$INSTANCE_NAME`_ENABLE_FWSN_STRING)
        `$INSTANCE_NAME`_snStringConfirm = `$INSTANCE_NAME`_FALSE;
        if(snString != `$INSTANCE_NAME`_NULL)
        {
            `$INSTANCE_NAME`_fwSerialNumberStringDescriptor = snString;
            /* check descriptor validation */
            if( (`$INSTANCE_NAME`_fwSerialNumberStringDescriptor[0u] > 1u ) &&  \
                (`$INSTANCE_NAME`_fwSerialNumberStringDescriptor[1u] == `$INSTANCE_NAME`_DESCR_STRING) )
            {
                `$INSTANCE_NAME`_snStringConfirm = `$INSTANCE_NAME`_TRUE;
            }
        }
    #else
        snString = snString;
    #endif  /* `$INSTANCE_NAME`_ENABLE_FWSN_STRING */
}


/*******************************************************************************
* Function Name: `$INSTANCE_NAME`_HandleStandardRqst
********************************************************************************
*
* Summary:
*  This Routine dispatches standard requests
*
* Parameters:
*  None.
*
* Return:
*  TRUE if request handled.
*
* Reentrant:
*  No.
*
*******************************************************************************/
uint8 `$INSTANCE_NAME`_HandleStandardRqst(void)
{
    uint8 requestHandled = `$INSTANCE_NAME`_FALSE;
    #if defined(`$INSTANCE_NAME`_ENABLE_STRINGS)
        uint8 *pStr = 0u;
        #if defined(`$INSTANCE_NAME`_ENABLE_DESCRIPTOR_STRINGS)
            uint8 nStr;
        #endif /* `$INSTANCE_NAME`_ENABLE_DESCRIPTOR_STRINGS */
    #endif /* `$INSTANCE_NAME`_ENABLE_STRINGS */
    uint16 count;

    T_`$INSTANCE_NAME`_LUT *pTmp;
    currentTD.count = 0u;

    if ((CY_GET_REG8(`$INSTANCE_NAME`_bmRequestType) & `$INSTANCE_NAME`_RQST_DIR_MASK) == `$INSTANCE_NAME`_RQST_DIR_D2H)
    {
        /* Control Read */
        switch (CY_GET_REG8(`$INSTANCE_NAME`_bRequest))
        {
            case `$INSTANCE_NAME`_GET_DESCRIPTOR:
                if (CY_GET_REG8(`$INSTANCE_NAME`_wValueHi) == `$INSTANCE_NAME`_DESCR_DEVICE)
                {
                    pTmp = `$INSTANCE_NAME`_GetDeviceTablePtr();
                    currentTD.pData = pTmp->p_list;
                    currentTD.count = `$INSTANCE_NAME`_DEVICE_DESCR_LENGTH;
                    requestHandled  = `$INSTANCE_NAME`_InitControlRead();
                }
                else if (CY_GET_REG8(`$INSTANCE_NAME`_wValueHi) == `$INSTANCE_NAME`_DESCR_CONFIG)
                {
                    pTmp = `$INSTANCE_NAME`_GetConfigTablePtr(CY_GET_REG8(`$INSTANCE_NAME`_wValueLo));
                    currentTD.pData = pTmp->p_list;
                    count = ((uint16)(currentTD.pData)[`$INSTANCE_NAME`_CONFIG_DESCR_TOTAL_LENGTH_HI] << 8u) | \
                                                    (currentTD.pData)[`$INSTANCE_NAME`_CONFIG_DESCR_TOTAL_LENGTH_LOW];
                    currentTD.count = count;
                    requestHandled  = `$INSTANCE_NAME`_InitControlRead();
                }
                #if defined(`$INSTANCE_NAME`_ENABLE_STRINGS)
                else if (CY_GET_REG8(`$INSTANCE_NAME`_wValueHi) == `$INSTANCE_NAME`_DESCR_STRING)
                {
                    /* Descriptor Strings*/
                    #if defined(`$INSTANCE_NAME`_ENABLE_DESCRIPTOR_STRINGS)
                        nStr = 0u;
                        pStr = &`$INSTANCE_NAME`_STRING_DESCRIPTORS[0u];
                        while ( (CY_GET_REG8(`$INSTANCE_NAME`_wValueLo) > nStr) && (*pStr != 0u ))
                        {
                            pStr += *pStr;
                            nStr++;
                        };
                    #endif /* End `$INSTANCE_NAME`_ENABLE_DESCRIPTOR_STRINGS */
                    /* Microsoft OS String*/
                    #if defined(`$INSTANCE_NAME`_ENABLE_MSOS_STRING)
                        if( CY_GET_REG8(`$INSTANCE_NAME`_wValueLo) == `$INSTANCE_NAME`_STRING_MSOS )
                        {
                            pStr = &`$INSTANCE_NAME`_MSOS_DESCRIPTOR[0u];
                        }
                    #endif /* End `$INSTANCE_NAME`_ENABLE_MSOS_STRING*/
                    /* SN string*/
                    #if defined(`$INSTANCE_NAME`_ENABLE_SN_STRING)
                        if( (CY_GET_REG8(`$INSTANCE_NAME`_wValueLo) != 0 ) && \
                            (CY_GET_REG8(`$INSTANCE_NAME`_wValueLo) == \
                            `$INSTANCE_NAME`_DEVICE0_DESCR[`$INSTANCE_NAME`_DEVICE_DESCR_SN_SHIFT]) )
                        {
                            pStr = &`$INSTANCE_NAME`_SN_STRING_DESCRIPTOR[0u];
                            if(`$INSTANCE_NAME`_snStringConfirm != `$INSTANCE_NAME`_FALSE)
                            {
                                pStr = `$INSTANCE_NAME`_fwSerialNumberStringDescriptor;
                            }
                            #if defined(`$INSTANCE_NAME`_ENABLE_IDSN_STRING)
                                /* TODO: read DIE ID and genarete string descriptor in RAM*/
                                `$INSTANCE_NAME`_ReadDieID(`$INSTANCE_NAME`_idSerialNumberStringDescriptor);
                                pStr = `$INSTANCE_NAME`_idSerialNumberStringDescriptor;
                            #endif    /* End `$INSTANCE_NAME`_ENABLE_IDSN_STRING */
                        }
                    #endif    /* End `$INSTANCE_NAME`_ENABLE_SN_STRING */
                    if (*pStr != 0u)
                    {
                        currentTD.count = *pStr;
                        currentTD.pData = pStr;
                        requestHandled  = `$INSTANCE_NAME`_InitControlRead();
                    }
                }
                #endif /* End `$INSTANCE_NAME`_ENABLE_STRINGS*/
                else
                {
                    requestHandled = `$INSTANCE_NAME`_DispatchClassRqst();
                }
                break;
            case `$INSTANCE_NAME`_GET_STATUS:
                switch ((CY_GET_REG8(`$INSTANCE_NAME`_bmRequestType) & `$INSTANCE_NAME`_RQST_RCPT_MASK))
                {
                    case `$INSTANCE_NAME`_RQST_RCPT_EP:
                        currentTD.count = `$INSTANCE_NAME`_EP_STATUS_LENGTH;
                        tBuffer[0] = `$INSTANCE_NAME`_EP[ \
                                        CY_GET_REG8(`$INSTANCE_NAME`_wIndexLo) & `$INSTANCE_NAME`_DIR_UNUSED].hwEpState;
                        tBuffer[1] = 0u;
                        currentTD.pData = &tBuffer[0u];
                        requestHandled  = `$INSTANCE_NAME`_InitControlRead();
                        break;
                    case `$INSTANCE_NAME`_RQST_RCPT_DEV:
                        currentTD.count = `$INSTANCE_NAME`_DEVICE_STATUS_LENGTH;
                        tBuffer[0u] = `$INSTANCE_NAME`_deviceStatus;
                        tBuffer[1u] = 0u;
                        currentTD.pData = &tBuffer[0u];
                        requestHandled  = `$INSTANCE_NAME`_InitControlRead();
                        break;
                    default:    /* requestHandled is initialezed as FALSE by default */
                        break;
                }
                break;
            case `$INSTANCE_NAME`_GET_CONFIGURATION:
                currentTD.count = 1u;
                currentTD.pData = (uint8 *)&`$INSTANCE_NAME`_configuration;
                requestHandled  = `$INSTANCE_NAME`_InitControlRead();
                break;
            case `$INSTANCE_NAME`_GET_INTERFACE:
                currentTD.count = 1u;
                currentTD.pData = (uint8 *)&`$INSTANCE_NAME`_interfaceSetting[CY_GET_REG8(`$INSTANCE_NAME`_wIndexLo)];
                requestHandled  = `$INSTANCE_NAME`_InitControlRead();
                break;
            default: /* requestHandled is initialezed as FALSE by default */
                break;
        }
    }
    else {
        /* Control Write */
        switch (CY_GET_REG8(`$INSTANCE_NAME`_bRequest))
        {
            case `$INSTANCE_NAME`_SET_ADDRESS:
                `$INSTANCE_NAME`_deviceAddress = CY_GET_REG8(`$INSTANCE_NAME`_wValueLo);
                requestHandled = `$INSTANCE_NAME`_InitNoDataControlTransfer();
                break;
            case `$INSTANCE_NAME`_SET_CONFIGURATION:
                `$INSTANCE_NAME`_configuration = CY_GET_REG8(`$INSTANCE_NAME`_wValueLo);
                `$INSTANCE_NAME`_Config(`$INSTANCE_NAME`_TRUE);
                requestHandled = `$INSTANCE_NAME`_InitNoDataControlTransfer();
                break;
            case `$INSTANCE_NAME`_SET_INTERFACE:
                if (`$INSTANCE_NAME`_ValidateAlternateSetting())
                {
                    `$INSTANCE_NAME`_Config(`$INSTANCE_NAME`_FALSE);
                    requestHandled = `$INSTANCE_NAME`_InitNoDataControlTransfer();
                }
                break;
            case `$INSTANCE_NAME`_CLEAR_FEATURE:
                switch (CY_GET_REG8(`$INSTANCE_NAME`_bmRequestType) & `$INSTANCE_NAME`_RQST_RCPT_MASK)
                {
                case `$INSTANCE_NAME`_RQST_RCPT_EP:
                    if (CY_GET_REG8(`$INSTANCE_NAME`_wValueLo) == `$INSTANCE_NAME`_ENDPOINT_HALT)
                    {
                        requestHandled = `$INSTANCE_NAME`_ClearEndpointHalt();
                    }
                    break;
                case `$INSTANCE_NAME`_RQST_RCPT_DEV:
                    /* Clear device REMOTE_WAKEUP */
                    if (CY_GET_REG8(`$INSTANCE_NAME`_wValueLo) == `$INSTANCE_NAME`_DEVICE_REMOTE_WAKEUP)
                    {
                        `$INSTANCE_NAME`_deviceStatus &= ~`$INSTANCE_NAME`_DEVICE_STATUS_REMOTE_WAKEUP;
                        requestHandled = `$INSTANCE_NAME`_InitNoDataControlTransfer();
                    }
                    break;
                }
                break;
            case `$INSTANCE_NAME`_SET_FEATURE:
                switch (CY_GET_REG8(`$INSTANCE_NAME`_bmRequestType) & `$INSTANCE_NAME`_RQST_RCPT_MASK)
                {
                    case `$INSTANCE_NAME`_RQST_RCPT_EP:
                        if (CY_GET_REG8(`$INSTANCE_NAME`_wValueLo) == `$INSTANCE_NAME`_ENDPOINT_HALT)
                        {
                            requestHandled = `$INSTANCE_NAME`_SetEndpointHalt();
                        }
                        break;
                    case `$INSTANCE_NAME`_RQST_RCPT_DEV:
                        /* Set device REMOTE_WAKEUP */
                        if (CY_GET_REG8(`$INSTANCE_NAME`_wValueLo) == `$INSTANCE_NAME`_DEVICE_REMOTE_WAKEUP)
                        {
                            `$INSTANCE_NAME`_deviceStatus |= `$INSTANCE_NAME`_DEVICE_STATUS_REMOTE_WAKEUP;
                            requestHandled = `$INSTANCE_NAME`_InitNoDataControlTransfer();
                        }
                        break;
                    default:    /* requestHandled is initialezed as FALSE by default */
                        break;
                }
                break;
            default:    /* requestHandled is initialezed as FALSE by default */
                break;
        }
    }
    return(requestHandled);
}


#if defined(`$INSTANCE_NAME`_ENABLE_IDSN_STRING)

    /***************************************************************************
    * Function Name: `$INSTANCE_NAME`_ReadDieID
    ****************************************************************************
    *
    * Summary:
    *  This routine read Die ID and genarete Serian Number string descriptor.
    *
    * Parameters:
    *  descr:  pointer on string descriptor.
    *
    * Return:
    *  None.
    *
    * Reentrant:
    *  No.
    *
    ***************************************************************************/
    void `$INSTANCE_NAME`_ReadDieID(uint8 *descr)
    {
        uint8 i,j;
        uint8 value;
        static char8 const hex[16u] = "0123456789ABCDEF";

        /* check descriptor validation */
        if( (descr[0u] > 1u ) && (descr[1u] == `$INSTANCE_NAME`_DESCR_STRING) )
        {
            /* fill descriptor */
            for(j = 0u, i = 2u; i < descr[0u]; i += 2u)
            {
                value = CY_GET_XTND_REG8((void CYFAR *)(`$INSTANCE_NAME`_DIE_ID + j++));
                descr[i] = (uint8)hex[value >> 4u];
                i += 2u;
                descr[i] = (uint8)hex[value & 0x0Fu];
            }
        }
    }

#endif /* End $INSTANCE_NAME`_ENABLE_IDSN_STRING*/


/*******************************************************************************
* Function Name: `$INSTANCE_NAME`_Config
********************************************************************************
*
* Summary:
*  This routine configures endpoints for the entire configuration by scanning
*  the configuration descriptor.
*
* Parameters:
*  clearAltSetting: It configures the bAlternateSetting 0 for each interface.
*
* Return:
*  None.
*
* Reentrant:
*  No.
*
*******************************************************************************/
void `$INSTANCE_NAME`_Config(uint8 clearAltSetting)
{
    uint8 ep,cur_ep,i;
    uint8 iso;
    uint16 count;
    uint8 *pDescr;
    T_`$INSTANCE_NAME`_LUT *pTmp;
    T_`$INSTANCE_NAME`_EP_SETTINGS_BLOCK *pEP;

    /* Clear all of the endpoints */
    for (ep = 0u; ep < `$INSTANCE_NAME`_MAX_EP; ep++)
    {
        `$INSTANCE_NAME`_EP[ep].attrib = 0u;
        `$INSTANCE_NAME`_EP[ep].hwEpState = 0u;
        `$INSTANCE_NAME`_EP[ep].apiEpState = `$INSTANCE_NAME`_NO_EVENT_PENDING;
        `$INSTANCE_NAME`_EP[ep].epToggle = 0u;
        `$INSTANCE_NAME`_EP[ep].epMode = `$INSTANCE_NAME`_MODE_DISABLE;
        `$INSTANCE_NAME`_EP[ep].bufferSize = 0u;
    }

    /* Clear Alternate settings for all interfaces */
    if(clearAltSetting != 0u)
    {
        for (i = 0u; i < `$INSTANCE_NAME`_MAX_INTERFACES_NUMBER; i++)
        {
            `$INSTANCE_NAME`_interfaceSetting[i] = 0x00u;
        }
    }

    /* Init Endpoints and Device Status if configured */
    if(`$INSTANCE_NAME`_configuration > 0u)
    {
        pTmp = `$INSTANCE_NAME`_GetConfigTablePtr(`$INSTANCE_NAME`_configuration - 1u);
        /* Set Power status for current configuration */
        pDescr = (uint8 *)pTmp->p_list;
        `$INSTANCE_NAME`_SetPowerStatus(pDescr[`$INSTANCE_NAME`_CONFIG_DESCR_ATTRIB] & \
                                        `$INSTANCE_NAME`_CONFIG_DESCR_ATTRIB_SELF_POWERED);
        pTmp++;
        ep = pTmp->c;  /* For this table, c is the number of endpoints  */
        /* and p_list points the endpoint setting table. */
        pEP = (T_`$INSTANCE_NAME`_EP_SETTINGS_BLOCK *) pTmp->p_list;   

        for (i = 0u; i < ep; i++, pEP++)
        {
            /* compate current Alternate setting with EP Alt*/
            if(`$INSTANCE_NAME`_interfaceSetting[pEP->interface] == pEP->altSetting)
            {
                cur_ep = pEP->addr;
                cur_ep &= 0x0Fu;
                iso  = ((pEP->attributes & `$INSTANCE_NAME`_EP_TYPE_MASK) == `$INSTANCE_NAME`_EP_TYPE_ISOC);
                if (pEP->addr & `$INSTANCE_NAME`_DIR_IN)
                {
                    /* IN Endpoint */
                    `$INSTANCE_NAME`_EP[cur_ep].apiEpState = `$INSTANCE_NAME`_EVENT_PENDING;
                    `$INSTANCE_NAME`_EP[cur_ep].epMode = \
                                            (iso ? `$INSTANCE_NAME`_MODE_ISO_IN : `$INSTANCE_NAME`_MODE_ACK_IN);
                }
                else
                {
                    /* OUT Endpoint */
                    `$INSTANCE_NAME`_EP[cur_ep].apiEpState = `$INSTANCE_NAME`_NO_EVENT_PENDING;
                    `$INSTANCE_NAME`_EP[cur_ep].epMode = \
                                            (iso ? `$INSTANCE_NAME`_MODE_ISO_OUT : `$INSTANCE_NAME`_MODE_ACK_OUT);
                }
                `$INSTANCE_NAME`_EP[cur_ep].bufferSize = pEP->bufferSize;
                `$INSTANCE_NAME`_EP[cur_ep].addr = pEP->addr;
            }
        }

        /* Set the endpoint buffer addresses */
        count = 0u;
        ep = 1u;
        for (i = 0u; i < 0x80u; i+= 0x10u)
        {
            CY_SET_REG8(&`$INSTANCE_NAME`_ARB_EP1_CFG_PTR[i], `$INSTANCE_NAME`_ARB_EPX_CFG_CRC_BYPASS);

            if(`$INSTANCE_NAME`_EP[ep].epMode != `$INSTANCE_NAME`_MODE_DISABLE)
            {
                if((`$INSTANCE_NAME`_EP[ep].addr & `$INSTANCE_NAME`_DIR_IN) != 0u ) 
                {
                    CY_SET_REG8(&`$INSTANCE_NAME`_SIE_EP1_CR0_PTR[i], `$INSTANCE_NAME`_MODE_NAK_IN);
                }
                else
                {
                    CY_SET_REG8(&`$INSTANCE_NAME`_SIE_EP1_CR0_PTR[i], `$INSTANCE_NAME`_MODE_NAK_OUT);
                }
            }
            else
            {
                CY_SET_REG8(&`$INSTANCE_NAME`_SIE_EP1_CR0_PTR[i], `$INSTANCE_NAME`_MODE_STALL_DATA_EP);
            }    

            `$INSTANCE_NAME`_EP[ep].buffOffset = count;
            count += `$INSTANCE_NAME`_EP[ep].bufferSize;
            CY_SET_REG8(&`$INSTANCE_NAME`_SIE_EP1_CNT0_PTR[i],   `$INSTANCE_NAME`_EP[ep].bufferSize >> 8u);
            CY_SET_REG8(&`$INSTANCE_NAME`_SIE_EP1_CNT1_PTR[i],   `$INSTANCE_NAME`_EP[ep].bufferSize & 0xFFu);

            CY_SET_REG8(&`$INSTANCE_NAME`_ARB_RW1_RA_PTR[i],     `$INSTANCE_NAME`_EP[ep].buffOffset & 0xFFu);
            CY_SET_REG8(&`$INSTANCE_NAME`_ARB_RW1_RA_MSB_PTR[i], `$INSTANCE_NAME`_EP[ep].buffOffset >> 8u);
            CY_SET_REG8(&`$INSTANCE_NAME`_ARB_RW1_WA_PTR[i],     `$INSTANCE_NAME`_EP[ep].buffOffset & 0xFFu);
            CY_SET_REG8(&`$INSTANCE_NAME`_ARB_RW1_WA_MSB_PTR[i], `$INSTANCE_NAME`_EP[ep].buffOffset >> 8u);

            ep++;
        }
    } /* `$INSTANCE_NAME`_configuration > 0 */
    CY_SET_REG8(`$INSTANCE_NAME`_SIE_EP_INT_EN_PTR, 0xFFu);
}


/*******************************************************************************
* Function Name: `$INSTANCE_NAME`_GetConfigTablePtr
********************************************************************************
*
* Summary:
*  This routine returns a pointer a configuration table entry
*
* Parameters:
*  c:  Configuration Index
*
* Return:
*  Device Descriptor pointer.
*
*******************************************************************************/
T_`$INSTANCE_NAME`_LUT *`$INSTANCE_NAME`_GetConfigTablePtr(uint8 c) \
                                                        `=ReentrantKeil($INSTANCE_NAME . "_GetConfigTablePtr")`
{
    /* Device Table */
    T_`$INSTANCE_NAME`_LUT *pTmp;

    pTmp = `$INSTANCE_NAME`_GetDeviceTablePtr();
    
    /* The first entry points to the Device Descriptor, 
       the the configuration entries  */    
    return(pTmp[c + 1u].p_list);
}


/*******************************************************************************
* Function Name: `$INSTANCE_NAME`_GetDeviceTablePtr
********************************************************************************
*
* Summary:
*  This routine returns a pointer to the Device table
*
* Parameters:
*  None.
*
* Return:
*  Device Table pointer
*
*******************************************************************************/
T_`$INSTANCE_NAME`_LUT *`$INSTANCE_NAME`_GetDeviceTablePtr(void) `=ReentrantKeil($INSTANCE_NAME . "_GetDeviceTablePtr")`
{
    /* Device Table */
    return(`$INSTANCE_NAME`_TABLE[`$INSTANCE_NAME`_device].p_list);
}


/*******************************************************************************
* Function Name: `$INSTANCE_NAME`_SetEndpointHalt
********************************************************************************
*
* Summary:
*  This routine handles set endpoint halt.
*
* Parameters:
*  None.
*
* Return:
*  requestHandled.
*
* Reentrant:
*  No.
*
*******************************************************************************/
uint8 `$INSTANCE_NAME`_SetEndpointHalt(void)
{
    uint8 ep, ri;
    uint8 requestHandled = `$INSTANCE_NAME`_FALSE;

    /* Clear endpoint halt */
    ep = CY_GET_REG8(`$INSTANCE_NAME`_wIndexLo) & `$INSTANCE_NAME`_DIR_UNUSED;
    ri = ((ep - `$INSTANCE_NAME`_EP1) << `$INSTANCE_NAME`_EPX_CNTX_ADDR_SHIFT);

    if ((ep > `$INSTANCE_NAME`_EP0) && (ep < `$INSTANCE_NAME`_MAX_EP))
    {
        /* Set the endpoint Halt */
        `$INSTANCE_NAME`_EP[ep].hwEpState |= (`$INSTANCE_NAME`_ENDPOINT_STATUS_HALT);

        /* Clear the data toggle */
        `$INSTANCE_NAME`_EP[ep].epToggle = 0u;

        if (`$INSTANCE_NAME`_EP[ep].addr & `$INSTANCE_NAME`_DIR_IN)
        {
            /* IN Endpoint */
            `$INSTANCE_NAME`_EP[ep].apiEpState = `$INSTANCE_NAME`_NO_EVENT_ALLOWED;
            CY_SET_REG8(&`$INSTANCE_NAME`_SIE_EP1_CR0_PTR[ri], (`$INSTANCE_NAME`_MODE_STALL_DATA_EP | \
                                                            `$INSTANCE_NAME`_MODE_ACK_IN));
        }
        else
        {
            /* OUT Endpoint */
            `$INSTANCE_NAME`_EP[ep].apiEpState = `$INSTANCE_NAME`_NO_EVENT_ALLOWED;
            CY_SET_REG8(&`$INSTANCE_NAME`_SIE_EP1_CR0_PTR[ri], (`$INSTANCE_NAME`_MODE_STALL_DATA_EP | \
                                                            `$INSTANCE_NAME`_MODE_ACK_OUT));
        }
        requestHandled = `$INSTANCE_NAME`_InitNoDataControlTransfer();
    }

    return(requestHandled);
}


/*******************************************************************************
* Function Name: `$INSTANCE_NAME`_ClearEndpointHalt
********************************************************************************
*
* Summary:
*  This routine handles clear endpoint halt.
*
* Parameters:
*  None.
*
* Return:
*  requestHandled.
*
* Reentrant:
*  No.
*
*******************************************************************************/
uint8 `$INSTANCE_NAME`_ClearEndpointHalt(void)
{
    uint8 ep, ri;
    uint8 requestHandled = `$INSTANCE_NAME`_FALSE;

    /* Clear endpoint halt */
    ep = CY_GET_REG8(`$INSTANCE_NAME`_wIndexLo) & `$INSTANCE_NAME`_DIR_UNUSED;
    ri = ((ep - `$INSTANCE_NAME`_EP1) << `$INSTANCE_NAME`_EPX_CNTX_ADDR_SHIFT);

    if ((ep > `$INSTANCE_NAME`_EP0) && (ep < `$INSTANCE_NAME`_MAX_EP))
    {
        /* Set the endpoint Halt */
        `$INSTANCE_NAME`_EP[ep].hwEpState &= ~(`$INSTANCE_NAME`_ENDPOINT_STATUS_HALT);

        /* Clear the data toggle */
        `$INSTANCE_NAME`_EP[ep].epToggle = 0u;

        if (`$INSTANCE_NAME`_EP[ep].addr & `$INSTANCE_NAME`_DIR_IN)
        {
            /* IN Endpoint */
            `$INSTANCE_NAME`_EP[ep].apiEpState = `$INSTANCE_NAME`_NO_EVENT_PENDING;
            CY_SET_REG8(&`$INSTANCE_NAME`_SIE_EP1_CR0_PTR[ri], `$INSTANCE_NAME`_MODE_NAK_IN);
        }
        else
        {
            /* OUT Endpoint */
            `$INSTANCE_NAME`_EP[ep].apiEpState = `$INSTANCE_NAME`_EVENT_PENDING;
            CY_SET_REG8(&`$INSTANCE_NAME`_SIE_EP1_CR0_PTR[ri], `$INSTANCE_NAME`_MODE_ACK_OUT);
        }
        requestHandled = `$INSTANCE_NAME`_InitNoDataControlTransfer();
    }

    return(requestHandled);
}


/*******************************************************************************
* Function Name: `$INSTANCE_NAME`_ValidateAlternateSetting
********************************************************************************
*
* Summary:
*  Validates (and records) a SET INTERFACE request.
*
* Parameters:
*  None.
*
* Return:
*  requestHandled.
*
* Reentrant:
*  No.
*
*******************************************************************************/
uint8 `$INSTANCE_NAME`_ValidateAlternateSetting(void)
{
    uint8 requestHandled = `$INSTANCE_NAME`_TRUE;
    uint8 interfaceNum;
    T_`$INSTANCE_NAME`_LUT *pTmp;
    uint8 currentInterfacesNum;

    interfaceNum = CY_GET_REG8(`$INSTANCE_NAME`_wIndexLo);
    /* Validate interface setting, stall if invalid. */
    pTmp = `$INSTANCE_NAME`_GetConfigTablePtr(`$INSTANCE_NAME`_configuration - 1u);
    currentInterfacesNum  = ((uint8 *) pTmp->p_list)[`$INSTANCE_NAME`_CONFIG_DESCR_NUM_INTERFACES];

    if((interfaceNum >= currentInterfacesNum) || (interfaceNum >= `$INSTANCE_NAME`_MAX_INTERFACES_NUMBER))
    {   /* wrong interface number */
        requestHandled = `$INSTANCE_NAME`_FALSE;
    }
    else
    {
        `$INSTANCE_NAME`_interfaceSetting[interfaceNum] = CY_GET_REG8(`$INSTANCE_NAME`_wValueLo);
    }

    return (requestHandled);
}


/* [] END OF FILE */
