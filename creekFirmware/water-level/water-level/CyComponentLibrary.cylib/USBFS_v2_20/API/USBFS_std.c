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
* Copyright 2008-2012, Cypress Semiconductor Corporation.  All rights reserved.
* You may use this file only in accordance with the license, terms, conditions,
* disclaimers, and limitations in the end user license agreement accompanying
* the software package with which this file was provided.
*******************************************************************************/

#include "`$INSTANCE_NAME`.h"


/***************************************
* External references
***************************************/

extern const uint8 CYCODE `$INSTANCE_NAME`_DEVICE0_DESCR[];
extern const uint8 CYCODE `$INSTANCE_NAME`_DEVICE0_CONFIGURATION0_DESCR[];
extern const uint8 CYCODE `$INSTANCE_NAME`_STRING_DESCRIPTORS[];
extern const uint8 CYCODE `$INSTANCE_NAME`_MSOS_DESCRIPTOR[];
extern const uint8 CYCODE `$INSTANCE_NAME`_SN_STRING_DESCRIPTOR[];

extern volatile uint8 `$INSTANCE_NAME`_device;
extern volatile uint8 `$INSTANCE_NAME`_configuration;
extern volatile uint8 `$INSTANCE_NAME`_configurationChanged;
extern volatile uint8 `$INSTANCE_NAME`_interfaceSetting[];
extern volatile uint8 `$INSTANCE_NAME`_interfaceSetting_last[];
extern volatile uint8 `$INSTANCE_NAME`_deviceAddress;
extern volatile uint8 `$INSTANCE_NAME`_deviceStatus;
extern volatile uint8 `$INSTANCE_NAME`_interfaceStatus[];
extern uint8 CYCODE *`$INSTANCE_NAME`_interfaceClass;
extern const T_`$INSTANCE_NAME`_LUT CYCODE `$INSTANCE_NAME`_TABLE[];
extern volatile T_`$INSTANCE_NAME`_EP_CTL_BLOCK `$INSTANCE_NAME`_EP[];
extern volatile T_`$INSTANCE_NAME`_TD `$INSTANCE_NAME`_currentTD;
#if defined(`$INSTANCE_NAME`_ENABLE_CDC_CLASS)
    extern volatile uint8 `$INSTANCE_NAME`_cdc_data_in_ep;
    extern volatile uint8 `$INSTANCE_NAME`_cdc_data_out_ep;
#endif  /* End `$INSTANCE_NAME`_ENABLE_CDC_CLASS*/
#if defined(`$INSTANCE_NAME`_ENABLE_MIDI_STREAMING)
    extern volatile uint8 `$INSTANCE_NAME`_midi_in_ep;
    extern volatile uint8 `$INSTANCE_NAME`_midi_out_ep;
#endif /* End `$INSTANCE_NAME`_ENABLE_MIDI_STREAMING */


/***************************************
*         Forward references
***************************************/

uint8 `$INSTANCE_NAME`_InitControlRead(void) `=ReentrantKeil($INSTANCE_NAME . "_InitControlRead")`;
uint8 `$INSTANCE_NAME`_InitControlWrite(void) `=ReentrantKeil($INSTANCE_NAME . "_InitControlWrite")`;
uint8 `$INSTANCE_NAME`_InitNoDataControlTransfer(void) `=ReentrantKeil($INSTANCE_NAME . "_InitNoDataControlTransfer")`;
uint8 `$INSTANCE_NAME`_DispatchClassRqst(void) `=ReentrantKeil($INSTANCE_NAME . "_DispatchClassRqst")`;

void `$INSTANCE_NAME`_Config(uint8 clearAltSetting) `=ReentrantKeil($INSTANCE_NAME . "_Config")`;
void `$INSTANCE_NAME`_ConfigAltChanged(void) `=ReentrantKeil($INSTANCE_NAME . "_ConfigAltChanged")`;
T_`$INSTANCE_NAME`_LUT *`$INSTANCE_NAME`_GetConfigTablePtr(uint8 c)
                                                            `=ReentrantKeil($INSTANCE_NAME . "_GetConfigTablePtr")`;
T_`$INSTANCE_NAME`_LUT *`$INSTANCE_NAME`_GetDeviceTablePtr(void)
                                                            `=ReentrantKeil($INSTANCE_NAME . "_GetDeviceTablePtr")`;
uint8 `$INSTANCE_NAME`_ClearEndpointHalt(void) `=ReentrantKeil($INSTANCE_NAME . "_ClearEndpointHalt")`;
uint8 `$INSTANCE_NAME`_SetEndpointHalt(void) `=ReentrantKeil($INSTANCE_NAME . "_SetEndpointHalt")`;
uint8 `$INSTANCE_NAME`_ValidateAlternateSetting(void) `=ReentrantKeil($INSTANCE_NAME . "_ValidateAlternateSetting")`;

/*DIE ID string descriptor for 8 bytes ID*/
#if defined(`$INSTANCE_NAME`_ENABLE_IDSN_STRING)
    void `$INSTANCE_NAME`_ReadDieID(uint8 *descr) `=ReentrantKeil($INSTANCE_NAME . "_ReadDieID")`;
    uint8 `$INSTANCE_NAME`_idSerialNumberStringDescriptor[0x22u]={0x22u, `$INSTANCE_NAME`_DESCR_STRING};
#endif /* `$INSTANCE_NAME`_ENABLE_IDSN_STRING */

#if(`$INSTANCE_NAME`_EP_MM != `$INSTANCE_NAME`__EP_MANUAL)
    void `$INSTANCE_NAME`_Stop_DMA(uint8 epNumber) `=ReentrantKeil($INSTANCE_NAME . "_Stop_DMA")`;
#endif   /* End `$INSTANCE_NAME`_EP_MM != `$INSTANCE_NAME`__EP_MANUAL */
uint8 CYCODE *`$INSTANCE_NAME`_GetInterfaceClassTablePtr(void)
                                                    `=ReentrantKeil($INSTANCE_NAME . "_GetInterfaceClassTablePtr")`;


/***************************************
* Global data allocation
***************************************/

volatile uint8 `$INSTANCE_NAME`_tBuffer[`$INSTANCE_NAME`_EP_STATUS_LENGTH > `$INSTANCE_NAME`_DEVICE_STATUS_LENGTH ? \
                               `$INSTANCE_NAME`_EP_STATUS_LENGTH : `$INSTANCE_NAME`_DEVICE_STATUS_LENGTH];
volatile uint8 *`$INSTANCE_NAME`_fwSerialNumberStringDescriptor;
volatile uint8 `$INSTANCE_NAME`_snStringConfirm = `$INSTANCE_NAME`_FALSE;


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
void  `$INSTANCE_NAME`_SerialNumString(uint8 *snString) `=ReentrantKeil($INSTANCE_NAME . "_SerialNumString")`
{
    #if defined(`$INSTANCE_NAME`_ENABLE_FWSN_STRING)
        `$INSTANCE_NAME`_snStringConfirm = `$INSTANCE_NAME`_FALSE;
        if(snString != NULL)
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
uint8 `$INSTANCE_NAME`_HandleStandardRqst(void) `=ReentrantKeil($INSTANCE_NAME . "_HandleStandardRqst")`
{
    uint8 requestHandled = `$INSTANCE_NAME`_FALSE;
    #if defined(`$INSTANCE_NAME`_ENABLE_STRINGS)
        volatile uint8 *pStr = 0u;
        #if defined(`$INSTANCE_NAME`_ENABLE_DESCRIPTOR_STRINGS)
            uint8 nStr;
        #endif /* `$INSTANCE_NAME`_ENABLE_DESCRIPTOR_STRINGS */
    #endif /* `$INSTANCE_NAME`_ENABLE_STRINGS */
    uint16 count;

    T_`$INSTANCE_NAME`_LUT *pTmp;
    `$INSTANCE_NAME`_currentTD.count = 0u;

    if ((CY_GET_REG8(`$INSTANCE_NAME`_bmRequestType) & `$INSTANCE_NAME`_RQST_DIR_MASK) == `$INSTANCE_NAME`_RQST_DIR_D2H)
    {
        /* Control Read */
        switch (CY_GET_REG8(`$INSTANCE_NAME`_bRequest))
        {
            case `$INSTANCE_NAME`_GET_DESCRIPTOR:
                if (CY_GET_REG8(`$INSTANCE_NAME`_wValueHi) == `$INSTANCE_NAME`_DESCR_DEVICE)
                {
                    pTmp = `$INSTANCE_NAME`_GetDeviceTablePtr();
                    `$INSTANCE_NAME`_currentTD.pData = pTmp->p_list;
                    `$INSTANCE_NAME`_currentTD.count = `$INSTANCE_NAME`_DEVICE_DESCR_LENGTH;
                    requestHandled  = `$INSTANCE_NAME`_InitControlRead();
                }
                else if (CY_GET_REG8(`$INSTANCE_NAME`_wValueHi) == `$INSTANCE_NAME`_DESCR_CONFIG)
                {
                    pTmp = `$INSTANCE_NAME`_GetConfigTablePtr(CY_GET_REG8(`$INSTANCE_NAME`_wValueLo));
                    `$INSTANCE_NAME`_currentTD.pData = pTmp->p_list;
                    count = ((uint16)(`$INSTANCE_NAME`_currentTD.pData)[ \
                                      `$INSTANCE_NAME`_CONFIG_DESCR_TOTAL_LENGTH_HI] << 8u) | \
                                     (`$INSTANCE_NAME`_currentTD.pData)[`$INSTANCE_NAME`_CONFIG_DESCR_TOTAL_LENGTH_LOW];
                    `$INSTANCE_NAME`_currentTD.count = count;
                    requestHandled  = `$INSTANCE_NAME`_InitControlRead();
                }
                #if defined(`$INSTANCE_NAME`_ENABLE_STRINGS)
                else if (CY_GET_REG8(`$INSTANCE_NAME`_wValueHi) == `$INSTANCE_NAME`_DESCR_STRING)
                {
                    /* Descriptor Strings*/
                    #if defined(`$INSTANCE_NAME`_ENABLE_DESCRIPTOR_STRINGS)
                        nStr = 0u;
                        pStr = (volatile uint8 *)&`$INSTANCE_NAME`_STRING_DESCRIPTORS[0u];
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
                            pStr = (volatile uint8 *)&`$INSTANCE_NAME`_MSOS_DESCRIPTOR[0u];
                        }
                    #endif /* End `$INSTANCE_NAME`_ENABLE_MSOS_STRING*/
                    /* SN string*/
                    #if defined(`$INSTANCE_NAME`_ENABLE_SN_STRING)
                        if( (CY_GET_REG8(`$INSTANCE_NAME`_wValueLo) != 0) && 
                            (CY_GET_REG8(`$INSTANCE_NAME`_wValueLo) == 
                            `$INSTANCE_NAME`_DEVICE0_DESCR[`$INSTANCE_NAME`_DEVICE_DESCR_SN_SHIFT]) )
                        {
                            pStr = (volatile uint8 *)&`$INSTANCE_NAME`_SN_STRING_DESCRIPTOR[0u];
                            if(`$INSTANCE_NAME`_snStringConfirm != `$INSTANCE_NAME`_FALSE)
                            {
                                pStr = `$INSTANCE_NAME`_fwSerialNumberStringDescriptor;
                            }
                            #if defined(`$INSTANCE_NAME`_ENABLE_IDSN_STRING)
                                /* Read DIE ID and genarete string descriptor in RAM*/
                                `$INSTANCE_NAME`_ReadDieID(`$INSTANCE_NAME`_idSerialNumberStringDescriptor);
                                pStr = `$INSTANCE_NAME`_idSerialNumberStringDescriptor;
                            #endif    /* End `$INSTANCE_NAME`_ENABLE_IDSN_STRING */
                        }
                    #endif    /* End `$INSTANCE_NAME`_ENABLE_SN_STRING */
                    if (*pStr != 0u)
                    {
                        `$INSTANCE_NAME`_currentTD.count = *pStr;
                        `$INSTANCE_NAME`_currentTD.pData = pStr;
                        requestHandled  = `$INSTANCE_NAME`_InitControlRead();
                    }
                }
                #endif /* End `$INSTANCE_NAME`_ENABLE_STRINGS */
                else
                {
                    requestHandled = `$INSTANCE_NAME`_DispatchClassRqst();
                }
                break;
            case `$INSTANCE_NAME`_GET_STATUS:
                switch ((CY_GET_REG8(`$INSTANCE_NAME`_bmRequestType) & `$INSTANCE_NAME`_RQST_RCPT_MASK))
                {
                    case `$INSTANCE_NAME`_RQST_RCPT_EP:
                        `$INSTANCE_NAME`_currentTD.count = `$INSTANCE_NAME`_EP_STATUS_LENGTH;
                        `$INSTANCE_NAME`_tBuffer[0] = `$INSTANCE_NAME`_EP[ \
                                        CY_GET_REG8(`$INSTANCE_NAME`_wIndexLo) & `$INSTANCE_NAME`_DIR_UNUSED].hwEpState;
                        `$INSTANCE_NAME`_tBuffer[1] = 0u;
                        `$INSTANCE_NAME`_currentTD.pData = &`$INSTANCE_NAME`_tBuffer[0u];
                        requestHandled  = `$INSTANCE_NAME`_InitControlRead();
                        break;
                    case `$INSTANCE_NAME`_RQST_RCPT_DEV:
                        `$INSTANCE_NAME`_currentTD.count = `$INSTANCE_NAME`_DEVICE_STATUS_LENGTH;
                        `$INSTANCE_NAME`_tBuffer[0u] = `$INSTANCE_NAME`_deviceStatus;
                        `$INSTANCE_NAME`_tBuffer[1u] = 0u;
                        `$INSTANCE_NAME`_currentTD.pData = &`$INSTANCE_NAME`_tBuffer[0u];
                        requestHandled  = `$INSTANCE_NAME`_InitControlRead();
                        break;
                    default:    /* requestHandled is initialezed as FALSE by default */
                        break;
                }
                break;
            case `$INSTANCE_NAME`_GET_CONFIGURATION:
                `$INSTANCE_NAME`_currentTD.count = 1u;
                `$INSTANCE_NAME`_currentTD.pData = (uint8 *)&`$INSTANCE_NAME`_configuration;
                requestHandled  = `$INSTANCE_NAME`_InitControlRead();
                break;
            case `$INSTANCE_NAME`_GET_INTERFACE:
                `$INSTANCE_NAME`_currentTD.count = 1u;
                `$INSTANCE_NAME`_currentTD.pData = (uint8 *)&`$INSTANCE_NAME`_interfaceSetting[ \
                                                                            CY_GET_REG8(`$INSTANCE_NAME`_wIndexLo)];
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
                `$INSTANCE_NAME`_configurationChanged = `$INSTANCE_NAME`_TRUE;
                `$INSTANCE_NAME`_Config(`$INSTANCE_NAME`_TRUE);
                requestHandled = `$INSTANCE_NAME`_InitNoDataControlTransfer();
                break;
            case `$INSTANCE_NAME`_SET_INTERFACE:
                if (`$INSTANCE_NAME`_ValidateAlternateSetting())
                {
                    `$INSTANCE_NAME`_configurationChanged = `$INSTANCE_NAME`_TRUE;
                    #if ((`$INSTANCE_NAME`_EP_MA == `$INSTANCE_NAME`__MA_DYNAMIC) && \
                         (`$INSTANCE_NAME`_EP_MM == `$INSTANCE_NAME`__EP_MANUAL) )
                        `$INSTANCE_NAME`_Config(`$INSTANCE_NAME`_FALSE);
                    #else
                        `$INSTANCE_NAME`_ConfigAltChanged();
                    #endif /* End (`$INSTANCE_NAME`_EP_MA == `$INSTANCE_NAME`__MA_DYNAMIC) */

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
                    case `$INSTANCE_NAME`_RQST_RCPT_IFC:
                        /* Validate interfaceNumber */
                        if (CY_GET_REG8(`$INSTANCE_NAME`_wIndexLo) < `$INSTANCE_NAME`_MAX_INTERFACES_NUMBER)
                        {
                            `$INSTANCE_NAME`_interfaceStatus[CY_GET_REG8(`$INSTANCE_NAME`_wIndexLo)] &= \
                                                                ~(CY_GET_REG8(`$INSTANCE_NAME`_wValueLo)) ;
                            requestHandled = `$INSTANCE_NAME`_InitNoDataControlTransfer();
                        }
                        break;
                    default:    /* requestHandled is initialezed as FALSE by default */
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
                    case `$INSTANCE_NAME`_RQST_RCPT_IFC:
                        /* Validate interfaceNumber */
                        if (CY_GET_REG8(`$INSTANCE_NAME`_wIndexLo) < `$INSTANCE_NAME`_MAX_INTERFACES_NUMBER)
                        {
                            `$INSTANCE_NAME`_interfaceStatus[CY_GET_REG8(`$INSTANCE_NAME`_wIndexLo)] &= \
                                                                ~(CY_GET_REG8(`$INSTANCE_NAME`_wValueLo)) ;
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
    void `$INSTANCE_NAME`_ReadDieID(uint8 *descr) `=ReentrantKeil($INSTANCE_NAME . "_ReadDieID")`
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
* Function Name: `$INSTANCE_NAME`_ConfigReg
********************************************************************************
*
* Summary:
*  This routine configures hardware registers from the variables.
*  It is called from `$INSTANCE_NAME`_Config() function and from RestoreConfig
*  after Wakeup.
*
* Parameters:
*  None.
*
* Return:
*  None.
*
*******************************************************************************/
void `$INSTANCE_NAME`_ConfigReg(void) `=ReentrantKeil($INSTANCE_NAME . "_ConfigReg")`
{
    uint8 ep;
    uint8 i;
    #if(`$INSTANCE_NAME`_EP_MM == `$INSTANCE_NAME`__EP_DMAAUTO)
        uint8 ep_type = 0u;
    #endif /* End `$INSTANCE_NAME`_EP_MM == `$INSTANCE_NAME`__EP_DMAAUTO */

    /* Set the endpoint buffer addresses */
    ep = `$INSTANCE_NAME`_EP1;
    for (i = 0u; i < 0x80u; i+= 0x10u)
    {
        CY_SET_REG8(&`$INSTANCE_NAME`_ARB_EP1_CFG_PTR[i], `$INSTANCE_NAME`_ARB_EPX_CFG_CRC_BYPASS |
                                                          `$INSTANCE_NAME`_ARB_EPX_CFG_RESET);

        #if(`$INSTANCE_NAME`_EP_MM != `$INSTANCE_NAME`__EP_MANUAL)
            /* Enable all Arbiter EP Interrupts : err, buf under, buf over, dma gnt, in buf full */
            `$INSTANCE_NAME`_ARB_EP1_INT_EN_PTR[i] = `$INSTANCE_NAME`_ARB_EPX_INT_MASK;
        #endif   /* End `$INSTANCE_NAME`_EP_MM != `$INSTANCE_NAME`__EP_MANUAL */

        if(`$INSTANCE_NAME`_EP[ep].epMode != `$INSTANCE_NAME`_MODE_DISABLE)
        {
            if((`$INSTANCE_NAME`_EP[ep].addr & `$INSTANCE_NAME`_DIR_IN) != 0u )
            {
                CY_SET_REG8(&`$INSTANCE_NAME`_SIE_EP1_CR0_PTR[i], `$INSTANCE_NAME`_MODE_NAK_IN);
            }
            else
            {
                CY_SET_REG8(&`$INSTANCE_NAME`_SIE_EP1_CR0_PTR[i], `$INSTANCE_NAME`_MODE_NAK_OUT);
                /* Prepare EP type mask for automatic memory allocation */
                #if(`$INSTANCE_NAME`_EP_MM == `$INSTANCE_NAME`__EP_DMAAUTO)
                    ep_type |= 0x01u << (ep - `$INSTANCE_NAME`_EP1);
                #endif /* End `$INSTANCE_NAME`_EP_MM == `$INSTANCE_NAME`__EP_DMAAUTO */
            }
        }
        else
        {
            CY_SET_REG8(&`$INSTANCE_NAME`_SIE_EP1_CR0_PTR[i], `$INSTANCE_NAME`_MODE_STALL_DATA_EP);
        }

        #if(`$INSTANCE_NAME`_EP_MM != `$INSTANCE_NAME`__EP_DMAAUTO)
            CY_SET_REG8(&`$INSTANCE_NAME`_SIE_EP1_CNT0_PTR[i],   `$INSTANCE_NAME`_EP[ep].bufferSize >> 8u);
            CY_SET_REG8(&`$INSTANCE_NAME`_SIE_EP1_CNT1_PTR[i],   `$INSTANCE_NAME`_EP[ep].bufferSize & 0xFFu);

            CY_SET_REG8(&`$INSTANCE_NAME`_ARB_RW1_RA_PTR[i],     `$INSTANCE_NAME`_EP[ep].buffOffset & 0xFFu);
            CY_SET_REG8(&`$INSTANCE_NAME`_ARB_RW1_RA_MSB_PTR[i], `$INSTANCE_NAME`_EP[ep].buffOffset >> 8u);
            CY_SET_REG8(&`$INSTANCE_NAME`_ARB_RW1_WA_PTR[i],     `$INSTANCE_NAME`_EP[ep].buffOffset & 0xFFu);
            CY_SET_REG8(&`$INSTANCE_NAME`_ARB_RW1_WA_MSB_PTR[i], `$INSTANCE_NAME`_EP[ep].buffOffset >> 8u);
        #endif /* End `$INSTANCE_NAME`_EP_MM != `$INSTANCE_NAME`__EP_DMAAUTO */

        ep++;
    }

    #if(`$INSTANCE_NAME`_EP_MM == `$INSTANCE_NAME`__EP_DMAAUTO)
         /* BUF_SIZE depend on DMA_THRESS value: 55-32 bytes  44-16 bytes 33-8 bytes 22-4 bytes 11-2 bytes */
        `$INSTANCE_NAME`_BUF_SIZE_REG = `$INSTANCE_NAME`_DMA_BUF_SIZE;
        `$INSTANCE_NAME`_DMA_THRES_REG = `$INSTANCE_NAME`_DMA_BYTES_PER_BURST;   /* DMA burst threshold */
        `$INSTANCE_NAME`_DMA_THRES_MSB_REG = 0u;
        `$INSTANCE_NAME`_EP_ACTIVE_REG = `$INSTANCE_NAME`_ARB_INT_MASK;
        `$INSTANCE_NAME`_EP_TYPE_REG = ep_type;
        /* Cfg_cmp bit set to 1 once configuration is complete. */
        `$INSTANCE_NAME`_ARB_CFG_REG = `$INSTANCE_NAME`_ARB_CFG_AUTO_DMA | `$INSTANCE_NAME`_ARB_CFG_AUTO_MEM |
                                       `$INSTANCE_NAME`_ARB_CFG_CFG_CPM;
        /* Cfg_cmp bit set to 0 during configuration of PFSUSB Registers. */
        `$INSTANCE_NAME`_ARB_CFG_REG = `$INSTANCE_NAME`_ARB_CFG_AUTO_DMA | `$INSTANCE_NAME`_ARB_CFG_AUTO_MEM;
    #endif /* End `$INSTANCE_NAME`_EP_MM == `$INSTANCE_NAME`__EP_DMAAUTO */

    CY_SET_REG8(`$INSTANCE_NAME`_SIE_EP_INT_EN_PTR, 0xFFu);
}


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
* `$INSTANCE_NAME`_interfaceClass - Initialized class array for each interface.
*   It is used for hangling Class specific requests depend on interface class.
*   Different classes in multiple Alternate settings does not supported.
*
* Reentrant:
*  No.
*
*******************************************************************************/
void `$INSTANCE_NAME`_Config(uint8 clearAltSetting) `=ReentrantKeil($INSTANCE_NAME . "_Config")`
{
    uint8 ep;
    uint8 cur_ep;
    uint8 i;
    uint8 iso;
    uint8 *pDescr;
    #if(`$INSTANCE_NAME`_EP_MM != `$INSTANCE_NAME`__EP_DMAAUTO)
        uint16 count = 0u;
    #endif /* End `$INSTANCE_NAME`_EP_MM != `$INSTANCE_NAME`__EP_DMAAUTO */
    
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
        `$INSTANCE_NAME`_EP[ep].interface = 0u;

    }

    /* Clear Alternate settings for all interfaces */
    if(clearAltSetting != 0u)
    {
        for (i = 0u; i < `$INSTANCE_NAME`_MAX_INTERFACES_NUMBER; i++)
        {
            `$INSTANCE_NAME`_interfaceSetting[i] = 0x00u;
            `$INSTANCE_NAME`_interfaceSetting_last[i] = 0x00u;
        }
    }

    /* Init Endpoints and Device Status if configured */
    if(`$INSTANCE_NAME`_configuration > 0u)
    {
        pTmp = `$INSTANCE_NAME`_GetConfigTablePtr(`$INSTANCE_NAME`_configuration - 1u);
        /* Set Power status for current configuration */
        pDescr = (uint8 *)pTmp->p_list;
        if((pDescr[`$INSTANCE_NAME`_CONFIG_DESCR_ATTRIB] & `$INSTANCE_NAME`_CONFIG_DESCR_ATTRIB_SELF_POWERED) != 0u)
        {
            `$INSTANCE_NAME`_deviceStatus |=  `$INSTANCE_NAME`_DEVICE_STATUS_SELF_POWERED;
        }
        else
        {
            `$INSTANCE_NAME`_deviceStatus &=  ~`$INSTANCE_NAME`_DEVICE_STATUS_SELF_POWERED;
        }
        pTmp++;
        ep = pTmp->c;  /* For this table, c is the number of endpoints configurations  */

        #if ((`$INSTANCE_NAME`_EP_MA == `$INSTANCE_NAME`__MA_DYNAMIC) && \
             (`$INSTANCE_NAME`_EP_MM == `$INSTANCE_NAME`__EP_MANUAL) )
            /* Config for dynamic EP memory allocation */
            /* p_list points the endpoint setting table. */
            pEP = (T_`$INSTANCE_NAME`_EP_SETTINGS_BLOCK *) pTmp->p_list;
            for (i = 0u; i < ep; i++, pEP++)
            {
                /* compate current Alternate setting with EP Alt*/
                if(`$INSTANCE_NAME`_interfaceSetting[pEP->interface] == pEP->altSetting)
                {
                    cur_ep = pEP->addr & `$INSTANCE_NAME`_DIR_UNUSED;
                    iso  = ((pEP->attributes & `$INSTANCE_NAME`_EP_TYPE_MASK) == `$INSTANCE_NAME`_EP_TYPE_ISOC);
                    if (pEP->addr & `$INSTANCE_NAME`_DIR_IN)
                    {
                        /* IN Endpoint */
                        `$INSTANCE_NAME`_EP[cur_ep].apiEpState = `$INSTANCE_NAME`_EVENT_PENDING;
                        `$INSTANCE_NAME`_EP[cur_ep].epMode = \
                                                (iso ? `$INSTANCE_NAME`_MODE_ISO_IN : `$INSTANCE_NAME`_MODE_ACK_IN);
                        #if defined(`$INSTANCE_NAME`_ENABLE_CDC_CLASS)
                            if(((pEP->bMisc == `$INSTANCE_NAME`_CLASS_CDC_DATA) ||
                                (pEP->bMisc == `$INSTANCE_NAME`_CLASS_CDC)) &&
                                ((pEP->attributes & `$INSTANCE_NAME`_EP_TYPE_MASK) != `$INSTANCE_NAME`_EP_TYPE_INT))
                            {
                                `$INSTANCE_NAME`_cdc_data_in_ep = cur_ep;
                            }
                        #endif  /* End `$INSTANCE_NAME`_ENABLE_CDC_CLASS*/
                        #if defined(`$INSTANCE_NAME`_ENABLE_MIDI_STREAMING)
                            if((pEP->bMisc == `$INSTANCE_NAME`_CLASS_AUDIO) &&
                               ((pEP->attributes & `$INSTANCE_NAME`_EP_TYPE_MASK) == `$INSTANCE_NAME`_EP_TYPE_BULK))
                            {
                                `$INSTANCE_NAME`_midi_in_ep = cur_ep;
                            }
                        #endif  /* End `$INSTANCE_NAME`_ENABLE_MIDI_STREAMING*/
                    }
                    else
                    {
                        /* OUT Endpoint */
                        `$INSTANCE_NAME`_EP[cur_ep].apiEpState = `$INSTANCE_NAME`_NO_EVENT_PENDING;
                        `$INSTANCE_NAME`_EP[cur_ep].epMode = \
                                                (iso ? `$INSTANCE_NAME`_MODE_ISO_OUT : `$INSTANCE_NAME`_MODE_ACK_OUT);
                        #if defined(`$INSTANCE_NAME`_ENABLE_CDC_CLASS)
                            if(((pEP->bMisc == `$INSTANCE_NAME`_CLASS_CDC_DATA) ||
                                (pEP->bMisc == `$INSTANCE_NAME`_CLASS_CDC)) &&
                                ((pEP->attributes & `$INSTANCE_NAME`_EP_TYPE_MASK) != `$INSTANCE_NAME`_EP_TYPE_INT))
                            {
                                `$INSTANCE_NAME`_cdc_data_out_ep = cur_ep;
                            }
                        #endif  /* End `$INSTANCE_NAME`_ENABLE_CDC_CLASS*/
                        #if defined(`$INSTANCE_NAME`_ENABLE_MIDI_STREAMING)
                            if((pEP->bMisc == `$INSTANCE_NAME`_CLASS_AUDIO) &&
                               ((pEP->attributes & `$INSTANCE_NAME`_EP_TYPE_MASK) == `$INSTANCE_NAME`_EP_TYPE_BULK))
                            {
                                `$INSTANCE_NAME`_midi_out_ep = cur_ep;
                            }
                        #endif  /* End `$INSTANCE_NAME`_ENABLE_MIDI_STREAMING*/
                    }
                    `$INSTANCE_NAME`_EP[cur_ep].bufferSize = pEP->bufferSize;
                    `$INSTANCE_NAME`_EP[cur_ep].addr = pEP->addr;
                    `$INSTANCE_NAME`_EP[cur_ep].attrib = pEP->attributes;
                }
            }
        #else /* Config for static EP memory allocation  */
            for (i = `$INSTANCE_NAME`_EP1; i < `$INSTANCE_NAME`_MAX_EP; i++)
            {
                /* and p_list points the endpoint setting table. */
                pEP = (T_`$INSTANCE_NAME`_EP_SETTINGS_BLOCK *) pTmp->p_list;
                /* find max length for each EP and select it (length could be different in different Alt settings) */
                /* but other settings should be correct with regards to Interface alt Setting */
                for (cur_ep = 0u; cur_ep < ep; cur_ep++, pEP++)
                {
                    /* EP count is equal to EP # in table and we found larger EP length than have before*/
                    if(i == (pEP->addr & `$INSTANCE_NAME`_DIR_UNUSED))
                    {
                        if(`$INSTANCE_NAME`_EP[i].bufferSize < pEP->bufferSize)
                        {
                            `$INSTANCE_NAME`_EP[i].bufferSize = pEP->bufferSize;
                        }
                        /* compate current Alternate setting with EP Alt*/
                        if(`$INSTANCE_NAME`_interfaceSetting[pEP->interface] == pEP->altSetting)
                        {
                            iso  = ((pEP->attributes & `$INSTANCE_NAME`_EP_TYPE_MASK) == `$INSTANCE_NAME`_EP_TYPE_ISOC);
                            if (pEP->addr & `$INSTANCE_NAME`_DIR_IN)
                            {
                                /* IN Endpoint */
                                `$INSTANCE_NAME`_EP[i].apiEpState = `$INSTANCE_NAME`_EVENT_PENDING;
                                `$INSTANCE_NAME`_EP[i].epMode =
                                                    (iso ? `$INSTANCE_NAME`_MODE_ISO_IN : `$INSTANCE_NAME`_MODE_ACK_IN);
                                /* Find and init CDC IN endpoint number */
                                #if defined(`$INSTANCE_NAME`_ENABLE_CDC_CLASS)
                                    if(((pEP->bMisc == `$INSTANCE_NAME`_CLASS_CDC_DATA) ||
                                        (pEP->bMisc == `$INSTANCE_NAME`_CLASS_CDC)) &&
                                        ((pEP->attributes & `$INSTANCE_NAME`_EP_TYPE_MASK) !=
                                                                                        `$INSTANCE_NAME`_EP_TYPE_INT))
                                    {
                                        `$INSTANCE_NAME`_cdc_data_in_ep = i;
                                    }
                                #endif  /* End `$INSTANCE_NAME`_ENABLE_CDC_CLASS*/
                                #if defined(`$INSTANCE_NAME`_ENABLE_MIDI_STREAMING)
                                    if((pEP->bMisc == `$INSTANCE_NAME`_CLASS_AUDIO) &&
                                       ((pEP->attributes & `$INSTANCE_NAME`_EP_TYPE_MASK) ==
                                                                                        `$INSTANCE_NAME`_EP_TYPE_BULK))
                                    {
                                        `$INSTANCE_NAME`_midi_in_ep = i;
                                    }
                                #endif  /* End `$INSTANCE_NAME`_ENABLE_MIDI_STREAMING*/
                            }
                            else
                            {
                                /* OUT Endpoint */
                                `$INSTANCE_NAME`_EP[i].apiEpState = `$INSTANCE_NAME`_NO_EVENT_PENDING;
                                `$INSTANCE_NAME`_EP[i].epMode =
                                                (iso ? `$INSTANCE_NAME`_MODE_ISO_OUT : `$INSTANCE_NAME`_MODE_ACK_OUT);
                                /* Find and init CDC IN endpoint number */
                                #if defined(`$INSTANCE_NAME`_ENABLE_CDC_CLASS)
                                    if(((pEP->bMisc == `$INSTANCE_NAME`_CLASS_CDC_DATA) ||
                                        (pEP->bMisc == `$INSTANCE_NAME`_CLASS_CDC)) &&
                                        ((pEP->attributes & `$INSTANCE_NAME`_EP_TYPE_MASK) !=
                                                                                    `$INSTANCE_NAME`_EP_TYPE_INT))
                                    {
                                        `$INSTANCE_NAME`_cdc_data_out_ep = i;
                                    }
                                #endif  /* End `$INSTANCE_NAME`_ENABLE_CDC_CLASS*/
                                #if defined(`$INSTANCE_NAME`_ENABLE_MIDI_STREAMING)
                                    if((pEP->bMisc == `$INSTANCE_NAME`_CLASS_AUDIO) &&
                                       ((pEP->attributes & `$INSTANCE_NAME`_EP_TYPE_MASK) ==
                                                                                        `$INSTANCE_NAME`_EP_TYPE_BULK))
                                    {
                                        `$INSTANCE_NAME`_midi_out_ep = i;
                                    }
                                #endif  /* End `$INSTANCE_NAME`_ENABLE_MIDI_STREAMING*/
                            }
                            `$INSTANCE_NAME`_EP[i].addr = pEP->addr;
                            `$INSTANCE_NAME`_EP[i].attrib = pEP->attributes;

                            #if(`$INSTANCE_NAME`_EP_MM == `$INSTANCE_NAME`__EP_DMAAUTO)
                                break;      /* use first EP setting in Auto memory managment */
                            #endif /* End `$INSTANCE_NAME`_EP_MM == `$INSTANCE_NAME`__EP_DMAAUTO */
                        }
                    }
                }
            }
        #endif /* End (`$INSTANCE_NAME`_EP_MA == `$INSTANCE_NAME`__MA_DYNAMIC) */

        /* Init class array for each interface and interface number for each EP.
        *  It is used for hangling Class specific requests directed to either an
        *  interface or the endpoint.
        */
        /* p_list points the endpoint setting table. */
        pEP = (T_`$INSTANCE_NAME`_EP_SETTINGS_BLOCK *) pTmp->p_list;
        for (i = 0u; i < ep; i++, pEP++)
        {
            /* config interface number for each EP*/
            `$INSTANCE_NAME`_EP[pEP->addr & `$INSTANCE_NAME`_DIR_UNUSED].interface = pEP->interface;
        }
        /* init pointer on interface class table*/
        `$INSTANCE_NAME`_interfaceClass = `$INSTANCE_NAME`_GetInterfaceClassTablePtr();
        /* Set the endpoint buffer addresses */
        
        #if(`$INSTANCE_NAME`_EP_MM != `$INSTANCE_NAME`__EP_DMAAUTO)
            for (ep = `$INSTANCE_NAME`_EP1; ep < `$INSTANCE_NAME`_MAX_EP; ep++)
            {
                `$INSTANCE_NAME`_EP[ep].buffOffset = count;
                 count += `$INSTANCE_NAME`_EP[ep].bufferSize;
            }
        #endif /* End `$INSTANCE_NAME`_EP_MM != `$INSTANCE_NAME`__EP_DMAAUTO */

        /* Congigure hardware registers */
        `$INSTANCE_NAME`_ConfigReg();
    } /* `$INSTANCE_NAME`_configuration > 0 */
}


/*******************************************************************************
* Function Name: `$INSTANCE_NAME`_ConfigAltChanged
********************************************************************************
*
* Summary:
*  This routine undate configuration for the required endpoints only.
*  It is called after SET_INTERFACE request when Static memory allocation used.
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
void `$INSTANCE_NAME`_ConfigAltChanged() `=ReentrantKeil($INSTANCE_NAME . "_ConfigAltChanged")`
{
    uint8 ep,cur_ep,i;
    uint8 iso;
    uint8 ri;

    T_`$INSTANCE_NAME`_LUT *pTmp;
    T_`$INSTANCE_NAME`_EP_SETTINGS_BLOCK *pEP;


    /* Init Endpoints and Device Status if configured */
    if(`$INSTANCE_NAME`_configuration > 0u)
    {
        pTmp = `$INSTANCE_NAME`_GetConfigTablePtr(`$INSTANCE_NAME`_configuration - 1u);
        pTmp++;
        ep = pTmp->c;  /* For this table, c is the number of endpoints configurations  */

        /* Do not touch EP wich doesn't need reconfiguration */
        /* When Alt setting chaged, the only required endpoints need to be reconfigured */
        /* p_list points the endpoint setting table. */
        pEP = (T_`$INSTANCE_NAME`_EP_SETTINGS_BLOCK *) pTmp->p_list;
        for (i = 0u; i < ep; i++, pEP++)
        {
            /*If Alt setting changed and new is same with EP Alt */
            if((`$INSTANCE_NAME`_interfaceSetting[pEP->interface] !=
                `$INSTANCE_NAME`_interfaceSetting_last[pEP->interface] ) &&
               (`$INSTANCE_NAME`_interfaceSetting[pEP->interface] == pEP->altSetting) )
            {
                cur_ep = pEP->addr & `$INSTANCE_NAME`_DIR_UNUSED;
                ri = ((cur_ep - `$INSTANCE_NAME`_EP1) << `$INSTANCE_NAME`_EPX_CNTX_ADDR_SHIFT);
                iso  = ((pEP->attributes & `$INSTANCE_NAME`_EP_TYPE_MASK) == `$INSTANCE_NAME`_EP_TYPE_ISOC);
                if (pEP->addr & `$INSTANCE_NAME`_DIR_IN)
                {
                    /* IN Endpoint */
                    `$INSTANCE_NAME`_EP[cur_ep].apiEpState = `$INSTANCE_NAME`_EVENT_PENDING;
                    `$INSTANCE_NAME`_EP[cur_ep].epMode =
                                            (iso ? `$INSTANCE_NAME`_MODE_ISO_IN : `$INSTANCE_NAME`_MODE_ACK_IN);
                }
                else
                {
                    /* OUT Endpoint */
                    `$INSTANCE_NAME`_EP[cur_ep].apiEpState = `$INSTANCE_NAME`_NO_EVENT_PENDING;
                    `$INSTANCE_NAME`_EP[cur_ep].epMode =
                                            (iso ? `$INSTANCE_NAME`_MODE_ISO_OUT : `$INSTANCE_NAME`_MODE_ACK_OUT);
                }
                 /* Change the SIE mode for the selected EP to NAK ALL */
                 CY_SET_REG8(&`$INSTANCE_NAME`_SIE_EP1_CR0_PTR[ri], `$INSTANCE_NAME`_MODE_NAK_IN_OUT);
                `$INSTANCE_NAME`_EP[cur_ep].bufferSize = pEP->bufferSize;
                `$INSTANCE_NAME`_EP[cur_ep].addr = pEP->addr;
                `$INSTANCE_NAME`_EP[cur_ep].attrib = pEP->attributes;

                /* Clear the data toggle */
                `$INSTANCE_NAME`_EP[cur_ep].epToggle = 0u;

                /* Dynamic reconfiguration for mode 3 transfer */
                #if((`$INSTANCE_NAME`_EP_MM == `$INSTANCE_NAME`__EP_DMAAUTO) && (!CY_PSOC5A) )
                    /* In_data_rdy for selected EP should be set to 0 */
                    `$INSTANCE_NAME`_ARB_EP1_CFG_PTR[ri] &= ~`$INSTANCE_NAME`_ARB_EPX_CFG_IN_DATA_RDY;

                    /* write the EP number for which reconfiguration is required */
                    `$INSTANCE_NAME`_DYN_RECONFIG_REG = (cur_ep << `$INSTANCE_NAME`_DYN_RECONFIG_EP_SHIFT);
                    /* Set the dyn_config_en bit in dynamic reconfiguration register */
                    `$INSTANCE_NAME`_DYN_RECONFIG_REG |= `$INSTANCE_NAME`_DYN_RECONFIG_ENABLE;
                    /* wait for the dyn_config_rdy bit to set by the block,
                    *  this bit will be set to 1 when block is ready for reconfiguration.
                    */
                    while((`$INSTANCE_NAME`_DYN_RECONFIG_REG & `$INSTANCE_NAME`_DYN_RECONFIG_RDY_STS) == 0u);
                    /* Once dyn_config_rdy bit is set, FW can change the EP configuration. */
                    /* Change EP Type with new direction */
                    if((pEP->addr & `$INSTANCE_NAME`_DIR_IN) == 0)
                    {
                        `$INSTANCE_NAME`_EP_TYPE_REG |= 0x01u << (cur_ep - `$INSTANCE_NAME`_EP1);
                    }
                    else
                    {
                        `$INSTANCE_NAME`_EP_TYPE_REG &= ~(0x01u << (cur_ep - `$INSTANCE_NAME`_EP1));
                    }
                    /* dynamic reconfiguration enable bit cleared, pointers and control/status
                    *  signals for the selected EP is cleared/re-initialized on negedge
                    *  of dynamic reconfiguration enable bit).
                    */
                    `$INSTANCE_NAME`_DYN_RECONFIG_REG &= ~`$INSTANCE_NAME`_DYN_RECONFIG_ENABLE;

                    /* Cfg_cmp bit set to 1 once configuration is complete. */
                    `$INSTANCE_NAME`_ARB_CFG_REG = `$INSTANCE_NAME`_ARB_CFG_AUTO_DMA |
                                                `$INSTANCE_NAME`_ARB_CFG_AUTO_MEM | `$INSTANCE_NAME`_ARB_CFG_CFG_CPM;
                    /* Cfg_cmp bit set to 0 during configuration of PFSUSB Registers. */
                    `$INSTANCE_NAME`_ARB_CFG_REG = `$INSTANCE_NAME`_ARB_CFG_AUTO_DMA |
                                                   `$INSTANCE_NAME`_ARB_CFG_AUTO_MEM;
                    /* The main loop will reenable DMA and OUT endpoint*/
                #else
                CY_SET_REG8(&`$INSTANCE_NAME`_SIE_EP1_CNT0_PTR[ri],   `$INSTANCE_NAME`_EP[cur_ep].bufferSize >> 8u);
                CY_SET_REG8(&`$INSTANCE_NAME`_SIE_EP1_CNT1_PTR[ri],   `$INSTANCE_NAME`_EP[cur_ep].bufferSize & 0xFFu);

                CY_SET_REG8(&`$INSTANCE_NAME`_ARB_RW1_RA_PTR[ri],     `$INSTANCE_NAME`_EP[cur_ep].buffOffset & 0xFFu);
                CY_SET_REG8(&`$INSTANCE_NAME`_ARB_RW1_RA_MSB_PTR[ri], `$INSTANCE_NAME`_EP[cur_ep].buffOffset >> 8u);
                CY_SET_REG8(&`$INSTANCE_NAME`_ARB_RW1_WA_PTR[ri],     `$INSTANCE_NAME`_EP[cur_ep].buffOffset & 0xFFu);
                CY_SET_REG8(&`$INSTANCE_NAME`_ARB_RW1_WA_MSB_PTR[ri], `$INSTANCE_NAME`_EP[cur_ep].buffOffset >> 8u);
                #endif /* End `$INSTANCE_NAME`_EP_MM == `$INSTANCE_NAME`__EP_DMAAUTO */
            }
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
* Function Name: USB_GetInterfaceClassTablePtr
********************************************************************************
*
* Summary:
*  This routine returns Interface Class table pointer, which contains
*  the relation between interface number and interface class.
*
* Parameters:
*  None.
*
* Return:
*  Interface Class table pointer.
*
*******************************************************************************/
uint8 CYCODE *`$INSTANCE_NAME`_GetInterfaceClassTablePtr(void)
                                                        `=ReentrantKeil($INSTANCE_NAME . "_GetInterfaceClassTablePtr")`
{
    T_`$INSTANCE_NAME`_LUT *pTmp;
    uint8 currentInterfacesNum;

    pTmp = `$INSTANCE_NAME`_GetConfigTablePtr(`$INSTANCE_NAME`_configuration - 1u);
    currentInterfacesNum  = ((uint8 *) pTmp->p_list)[`$INSTANCE_NAME`_CONFIG_DESCR_NUM_INTERFACES];
    /* Third entry in the LUT starts the Interface Table pointers */
    pTmp += 2;
    /* The INTERFACE_CLASS table is located after all interfaces*/
    pTmp += currentInterfacesNum;
    return(pTmp->p_list);
}


/*******************************************************************************
* Function Name: `$INSTANCE_NAME`_TerminateEP
********************************************************************************
*
* Summary:
*  This routine handles set endpoint halt.
*
* Parameters:
*  Endpoint number.
*
* Return:
*  None.
*
* Reentrant:
*  No.
*
*******************************************************************************/
void `$INSTANCE_NAME`_TerminateEP(uint8 ep) `=ReentrantKeil($INSTANCE_NAME . "_TerminateEP")`
{
    uint8 ri;

    ep &= `$INSTANCE_NAME`_DIR_UNUSED;
    ri = ((ep - `$INSTANCE_NAME`_EP1) << `$INSTANCE_NAME`_EPX_CNTX_ADDR_SHIFT);

    if ((ep > `$INSTANCE_NAME`_EP0) && (ep < `$INSTANCE_NAME`_MAX_EP))
    {
        /* Set the endpoint Halt */
        `$INSTANCE_NAME`_EP[ep].hwEpState |= (`$INSTANCE_NAME`_ENDPOINT_STATUS_HALT);

        /* Clear the data toggle */
        `$INSTANCE_NAME`_EP[ep].epToggle = 0u;
        `$INSTANCE_NAME`_EP[ep].apiEpState = `$INSTANCE_NAME`_NO_EVENT_ALLOWED;

        if (`$INSTANCE_NAME`_EP[ep].addr & `$INSTANCE_NAME`_DIR_IN)
        {
            /* IN Endpoint */
            CY_SET_REG8(&`$INSTANCE_NAME`_SIE_EP1_CR0_PTR[ri], `$INSTANCE_NAME`_MODE_NAK_IN);
        }
        else
        {
            /* OUT Endpoint */
            CY_SET_REG8(&`$INSTANCE_NAME`_SIE_EP1_CR0_PTR[ri], `$INSTANCE_NAME`_MODE_NAK_OUT);
        }
    }
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
uint8 `$INSTANCE_NAME`_SetEndpointHalt(void) `=ReentrantKeil($INSTANCE_NAME . "_SetEndpointHalt")`
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
        `$INSTANCE_NAME`_EP[ep].apiEpState = `$INSTANCE_NAME`_NO_EVENT_ALLOWED;

        if (`$INSTANCE_NAME`_EP[ep].addr & `$INSTANCE_NAME`_DIR_IN)
        {
            /* IN Endpoint */
            CY_SET_REG8(&`$INSTANCE_NAME`_SIE_EP1_CR0_PTR[ri], `$INSTANCE_NAME`_MODE_STALL_DATA_EP |
                                                               `$INSTANCE_NAME`_MODE_ACK_IN);
        }
        else
        {
            /* OUT Endpoint */
            CY_SET_REG8(&`$INSTANCE_NAME`_SIE_EP1_CR0_PTR[ri], `$INSTANCE_NAME`_MODE_STALL_DATA_EP |
                                                               `$INSTANCE_NAME`_MODE_ACK_OUT);
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
uint8 `$INSTANCE_NAME`_ClearEndpointHalt(void) `=ReentrantKeil($INSTANCE_NAME . "_ClearEndpointHalt")`
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
uint8 `$INSTANCE_NAME`_ValidateAlternateSetting(void) `=ReentrantKeil($INSTANCE_NAME . "_ValidateAlternateSetting")`
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
        /* save current Alt setting to find out the difference in Config() function */
        `$INSTANCE_NAME`_interfaceSetting_last[interfaceNum] = `$INSTANCE_NAME`_interfaceSetting[interfaceNum];
        `$INSTANCE_NAME`_interfaceSetting[interfaceNum] = CY_GET_REG8(`$INSTANCE_NAME`_wValueLo);
    }

    return (requestHandled);
}


/* [] END OF FILE */
