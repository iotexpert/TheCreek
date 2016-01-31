/*******************************************************************************
* File Name: `$INSTANCE_NAME`_audio.c
* Version `$CY_MAJOR_VERSION`.`$CY_MINOR_VERSION`
*
* Description:
*  USB AUDIO Class request handler.
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

#if defined(`$INSTANCE_NAME`_ENABLE_AUDIO_CLASS)

#include "`$INSTANCE_NAME`_audio.h"
#include "`$INSTANCE_NAME`_midi.h"


/***************************************
*    AUDIO Variables
***************************************/

#if defined(`$INSTANCE_NAME`_ENABLE_AUDIO_STREAMING)
    volatile uint8 `$INSTANCE_NAME`_currentSampleFrequency[`$INSTANCE_NAME`_MAX_EP][`$INSTANCE_NAME`_SAMPLE_FREQ_LEN];
    volatile uint8 `$INSTANCE_NAME`_frequencyChanged;
    volatile uint8 `$INSTANCE_NAME`_currentMute;
    volatile uint8 `$INSTANCE_NAME`_currentVolume[`$INSTANCE_NAME`_VOLUME_LEN];
    volatile uint8 `$INSTANCE_NAME`_minimumVolume[] = {0x01, 0x80};
    volatile uint8 `$INSTANCE_NAME`_maximumVolume[] = {0xFF, 0x7F};
    volatile uint8 `$INSTANCE_NAME`_resolutionVolume[] = {0x01, 0x00};
#endif /* End `$INSTANCE_NAME`_ENABLE_AUDIO_STREAMING */


/***************************************
* Custom Declarations
***************************************/

/* `#START CUSTOM_DECLARATIONS` Place your declaration here */

/* `#END` */


/***************************************
*    External references
***************************************/

uint8 `$INSTANCE_NAME`_InitControlRead(void) `=ReentrantKeil($INSTANCE_NAME . "_InitControlRead")`;
uint8 `$INSTANCE_NAME`_InitControlWrite(void) `=ReentrantKeil($INSTANCE_NAME . "_InitControlWrite")`;
uint8 `$INSTANCE_NAME`_InitNoDataControlTransfer(void) `=ReentrantKeil($INSTANCE_NAME . "_InitNoDataControlTransfer")`;
uint8 `$INSTANCE_NAME`_InitZeroLengthControlTransfer(void)
                                                `=ReentrantKeil($INSTANCE_NAME . "_InitZeroLengthControlTransfer")`;

extern volatile T_`$INSTANCE_NAME`_EP_CTL_BLOCK `$INSTANCE_NAME`_EP[];
extern volatile T_`$INSTANCE_NAME`_TD `$INSTANCE_NAME`_currentTD;


/***************************************
*    Private function prototypes
***************************************/


/*******************************************************************************
* Function Name: `$INSTANCE_NAME`_DispatchAUDIOClassRqst
********************************************************************************
*
* Summary:
*  This routine dispatches class requests
*
* Parameters:
*  None.
*
* Return:
*  requestHandled
*
* Global variables:
*   `$INSTANCE_NAME`_currentSampleFrequency: Contains the current audio Sample
*       Frequency. It is set by the Host using SET_CUR request to the endpoint.
*   `$INSTANCE_NAME`_frequencyChanged: This variable is used as a flag for the
*       user code, to be aware that Host has been sent request for changing
*       Sample Frequency. Sample frequency will be sent on the next OUT
*       transaction. It is contains endpoint address when set. The following
*       code is recommended for detecting new Sample Frequency in main code:
*       if((`$INSTANCE_NAME`_frequencyChanged != 0) &&
*       (`$INSTANCE_NAME`_transferState == `$INSTANCE_NAME`_TRANS_STATE_IDLE))
*       {
*          `$INSTANCE_NAME`_frequencyChanged = 0;
*       }
*       `$INSTANCE_NAME`_transferState variable is checked to be sure that
*             transfer completes.
*   `$INSTANCE_NAME`_currentMute: Contains mute configuration set by Host.
*   `$INSTANCE_NAME`_currentVolume: Contains volume level set by Host.
*
* Reentrant:
*  No.
*
*******************************************************************************/
uint8 `$INSTANCE_NAME`_DispatchAUDIOClassRqst() `=ReentrantKeil($INSTANCE_NAME . "_DispatchAUDIOClassRqst")`
{
    uint8 requestHandled = `$INSTANCE_NAME`_FALSE;
    
    #if defined(`$INSTANCE_NAME`_ENABLE_AUDIO_STREAMING)
        uint8 epNumber;
        epNumber = CY_GET_REG8(`$INSTANCE_NAME`_wIndexLo) & `$INSTANCE_NAME`_DIR_UNUSED;
    #endif /* End `$INSTANCE_NAME`_ENABLE_AUDIO_STREAMING */

    if ((CY_GET_REG8(`$INSTANCE_NAME`_bmRequestType) & `$INSTANCE_NAME`_RQST_DIR_MASK) == `$INSTANCE_NAME`_RQST_DIR_D2H)
    {
        /* Control Read */
        if((CY_GET_REG8(`$INSTANCE_NAME`_bmRequestType) & `$INSTANCE_NAME`_RQST_RCPT_MASK) == \
                                                                                    `$INSTANCE_NAME`_RQST_RCPT_EP)
        {
            /* Endpoint */
            switch (CY_GET_REG8(`$INSTANCE_NAME`_bRequest))
            {
                case `$INSTANCE_NAME`_GET_CUR:
                #if defined(`$INSTANCE_NAME`_ENABLE_AUDIO_STREAMING)
                    if(CY_GET_REG8(`$INSTANCE_NAME`_wValueHi) == `$INSTANCE_NAME`_SAMPLING_FREQ_CONTROL)
                    {
                         /* Endpoint Control Selector is Sampling Frequency */
                        `$INSTANCE_NAME`_currentTD.wCount = `$INSTANCE_NAME`_SAMPLE_FREQ_LEN;
                        `$INSTANCE_NAME`_currentTD.pData  = `$INSTANCE_NAME`_currentSampleFrequency[epNumber];
                        requestHandled   = `$INSTANCE_NAME`_InitControlRead();
                    }
                #endif /* End `$INSTANCE_NAME`_ENABLE_AUDIO_STREAMING */

                /* `#START AUDIO_READ_REQUESTS` Place other request handler here */

                /* `#END` */
                    break;
                default:
                    break;
            }
        }
        else if((CY_GET_REG8(`$INSTANCE_NAME`_bmRequestType) & `$INSTANCE_NAME`_RQST_RCPT_MASK) == \
                                                                                    `$INSTANCE_NAME`_RQST_RCPT_IFC)
        {
            /* Interface or Entity ID */
            switch (CY_GET_REG8(`$INSTANCE_NAME`_bRequest))
            {
                case `$INSTANCE_NAME`_GET_CUR:
                #if defined(`$INSTANCE_NAME`_ENABLE_AUDIO_STREAMING)
                    if(CY_GET_REG8(`$INSTANCE_NAME`_wValueHi) == `$INSTANCE_NAME`_MUTE_CONTROL)
                    {
                         /* Entity ID Control Selector is MUTE */
                        `$INSTANCE_NAME`_currentTD.wCount = 1;
                        `$INSTANCE_NAME`_currentTD.pData  = &`$INSTANCE_NAME`_currentMute;
                        requestHandled   = `$INSTANCE_NAME`_InitControlRead();
                    }
                    else if(CY_GET_REG8(`$INSTANCE_NAME`_wValueHi) == `$INSTANCE_NAME`_VOLUME_CONTROL)
                    {
                         /* Entity ID Control Selector is VOLUME, */
                        `$INSTANCE_NAME`_currentTD.wCount = `$INSTANCE_NAME`_VOLUME_LEN;
                        `$INSTANCE_NAME`_currentTD.pData  = `$INSTANCE_NAME`_currentVolume;
                        requestHandled   = `$INSTANCE_NAME`_InitControlRead();
                    }
                    else
                    {
                    }
                    break;
                case `$INSTANCE_NAME`_GET_MIN:    /* GET_MIN */
                    if(CY_GET_REG8(`$INSTANCE_NAME`_wValueHi) == `$INSTANCE_NAME`_VOLUME_CONTROL)
                    {
                         /* Entity ID Control Selector is VOLUME, */
                        `$INSTANCE_NAME`_currentTD.wCount = `$INSTANCE_NAME`_VOLUME_LEN;
                        `$INSTANCE_NAME`_currentTD.pData  = &`$INSTANCE_NAME`_minimumVolume[0];
                        requestHandled   = `$INSTANCE_NAME`_InitControlRead();
                    }
                    break;
                case `$INSTANCE_NAME`_GET_MAX:    /* GET_MAX */
                    if(CY_GET_REG8(`$INSTANCE_NAME`_wValueHi) == `$INSTANCE_NAME`_VOLUME_CONTROL)
                    {
                             /* Entity ID Control Selector is VOLUME, */
                        `$INSTANCE_NAME`_currentTD.wCount = `$INSTANCE_NAME`_VOLUME_LEN;
                        `$INSTANCE_NAME`_currentTD.pData  = &`$INSTANCE_NAME`_maximumVolume[0];
                        requestHandled   = `$INSTANCE_NAME`_InitControlRead();
                    }
                    break;
                case `$INSTANCE_NAME`_GET_RES:    /* GET_RES */
                    if(CY_GET_REG8(`$INSTANCE_NAME`_wValueHi) == `$INSTANCE_NAME`_VOLUME_CONTROL)
                    {
                         /* Entity ID Control Selector is VOLUME, */
                        `$INSTANCE_NAME`_currentTD.wCount = `$INSTANCE_NAME`_VOLUME_LEN;
                        `$INSTANCE_NAME`_currentTD.pData  = &`$INSTANCE_NAME`_resolutionVolume[0];
                        requestHandled   = `$INSTANCE_NAME`_InitControlRead();
                    }
                    break;
                /* The contents of the status message is reserved for future use.
                *  For the time being, a null packet should be returned in the data stage of the 
                *  control transfer, and the received null packet should be ACKed.
                */
                case `$INSTANCE_NAME`_GET_STAT:
                        `$INSTANCE_NAME`_currentTD.wCount = 0;
                        requestHandled   = `$INSTANCE_NAME`_InitControlWrite();

                #endif /* End `$INSTANCE_NAME`_ENABLE_AUDIO_STREAMING */

                /* `#START AUDIO_WRITE_REQUESTS` Place other request handler here */

                /* `#END` */
                    break;
                default:
                    break;
            }
        }
        else
        {   /* `$INSTANCE_NAME`_RQST_RCPT_OTHER */
        }
    }
    else if ((CY_GET_REG8(`$INSTANCE_NAME`_bmRequestType) & `$INSTANCE_NAME`_RQST_DIR_MASK) == \
                                                                                    `$INSTANCE_NAME`_RQST_DIR_H2D)
    {
        /* Control Write */
        if((CY_GET_REG8(`$INSTANCE_NAME`_bmRequestType) & `$INSTANCE_NAME`_RQST_RCPT_MASK) == \
                                                                                    `$INSTANCE_NAME`_RQST_RCPT_EP)
        {
            /* Endpoint */
            switch (CY_GET_REG8(`$INSTANCE_NAME`_bRequest))
            {
                case `$INSTANCE_NAME`_SET_CUR:
                #if defined(`$INSTANCE_NAME`_ENABLE_AUDIO_STREAMING)
                    if(CY_GET_REG8(`$INSTANCE_NAME`_wValueHi) == `$INSTANCE_NAME`_SAMPLING_FREQ_CONTROL)
                    {
                         /* Endpoint Control Selector is Sampling Frequency */
                        `$INSTANCE_NAME`_currentTD.wCount = `$INSTANCE_NAME`_SAMPLE_FREQ_LEN;
                        `$INSTANCE_NAME`_currentTD.pData  = `$INSTANCE_NAME`_currentSampleFrequency[epNumber];
                        requestHandled   = `$INSTANCE_NAME`_InitControlWrite();
                        `$INSTANCE_NAME`_frequencyChanged = epNumber;
                    }
                #endif /* End `$INSTANCE_NAME`_ENABLE_AUDIO_STREAMING */

                /* `#START AUDIO_SAMPLING_FREQ_REQUESTS` Place other request handler here */

                /* `#END` */
                    break;
                default:
                    break;
            }
        }
        else if((CY_GET_REG8(`$INSTANCE_NAME`_bmRequestType) & `$INSTANCE_NAME`_RQST_RCPT_MASK) == \
                                                                                    `$INSTANCE_NAME`_RQST_RCPT_IFC)
        {
            /* Interface or Entity ID */
            switch (CY_GET_REG8(`$INSTANCE_NAME`_bRequest))
            {
                case `$INSTANCE_NAME`_SET_CUR:
                #if defined(`$INSTANCE_NAME`_ENABLE_AUDIO_STREAMING)
                    if(CY_GET_REG8(`$INSTANCE_NAME`_wValueHi) == `$INSTANCE_NAME`_MUTE_CONTROL)
                    {
                         /* Entity ID Control Selector is MUTE */
                        `$INSTANCE_NAME`_currentTD.wCount = 1;
                        `$INSTANCE_NAME`_currentTD.pData  = &`$INSTANCE_NAME`_currentMute;
                        requestHandled   = `$INSTANCE_NAME`_InitControlWrite();
                    }
                    else if(CY_GET_REG8(`$INSTANCE_NAME`_wValueHi) == `$INSTANCE_NAME`_VOLUME_CONTROL)
                    {
                         /* Entity ID Control Selector is VOLUME */
                        `$INSTANCE_NAME`_currentTD.wCount = `$INSTANCE_NAME`_VOLUME_LEN;
                        `$INSTANCE_NAME`_currentTD.pData  = `$INSTANCE_NAME`_currentVolume;
                        requestHandled   = `$INSTANCE_NAME`_InitControlWrite();
                    }
                #endif /* End `$INSTANCE_NAME`_ENABLE_AUDIO_STREAMING */

                /* `#START AUDIO_CONTROL_SEL_REQUESTS` Place other request handler here */

                /* `#END` */
                    break;
                default:
                    break;
            }
        }
        else
        {   /* `$INSTANCE_NAME`_RQST_RCPT_OTHER */
        }
    }
    else
    {   /* requestHandled is initialezed as FALSE by default */
    }

    return(requestHandled);
}


/*******************************************************************************
* Additional user functions supporting AUDIO Requests
********************************************************************************/

/* `#START AUDIO_FUNCTIONS` Place any additional functions here */

/* `#END` */

#endif  /* End `$INSTANCE_NAME`_ENABLE_AUDIO_CLASS*/


/* [] END OF FILE */
