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
* Copyright 2008-2010, Cypress Semiconductor Corporation.  All rights reserved.
* You may use this file only in accordance with the license, terms, conditions,
* disclaimers, and limitations in the end user license agreement accompanying
* the software package with which this file was provided.
********************************************************************************/

#include "`$INSTANCE_NAME`.h"

#if defined(`$INSTANCE_NAME`_ENABLE_AUDIO_CLASS)

#include "`$INSTANCE_NAME`_audio.h"


/***************************************
*    AUDIO Variables
***************************************/

volatile uint8 `$INSTANCE_NAME`_currentSampleFrequency[`$INSTANCE_NAME`_MAX_EP][`$INSTANCE_NAME`_SAMPLE_FREQ_LEN];
volatile uint8 `$INSTANCE_NAME`_currentMute;
volatile uint8 `$INSTANCE_NAME`_currentVolume[`$INSTANCE_NAME`_VOLUME_LEN];
volatile uint8 `$INSTANCE_NAME`_frequencyChanged;


/***************************************
* Custom Declratations
***************************************/

/* `#START CUSTOM_DECLARATIONS` Place your declaration here */

/* `#END` */


/***************************************
*    External references
***************************************/

uint8 `$INSTANCE_NAME`_InitControlRead(void);
uint8 `$INSTANCE_NAME`_InitControlWrite(void);
uint8 `$INSTANCE_NAME`_InitNoDataControlTransfer(void);

extern volatile T_`$INSTANCE_NAME`_TD `$INSTANCE_NAME`_currentTD;


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
*       if((USBFS_frequencyChanged != 0) && (USBFS_transferState == USBFS_TRANS_STATE_IDLE))
*       {	
*          // Add core here.
*          USBFS_frequencyChanged = 0;
*       }
*       USBFS_transferState variable is checked to be sure that transfer completes.
*   `$INSTANCE_NAME`_currentMute: Contains mute configuration set by Host.
*   `$INSTANCE_NAME`_currentVolume: Contains volume level set by Host.
*
* Reentrant:
*  No.
*
*******************************************************************************/
uint8 `$INSTANCE_NAME`_DispatchAUDIOClassRqst()
{
    uint8 requestHandled = `$INSTANCE_NAME`_FALSE;
    uint8 epNumber;

    epNumber = CY_GET_REG8(`$INSTANCE_NAME`_wIndexLo) & `$INSTANCE_NAME`_DIR_UNUSED;
  
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
					if(CY_GET_REG8(`$INSTANCE_NAME`_wValueHi) == `$INSTANCE_NAME`_SAMPLING_FREQ_CONTROL) 
                    {
					 	/* Endpoint Control Selector is Sampling Frequency */
						`$INSTANCE_NAME`_currentTD.wCount = `$INSTANCE_NAME`_SAMPLE_FREQ_LEN;
						`$INSTANCE_NAME`_currentTD.pData  = `$INSTANCE_NAME`_currentSampleFrequency[epNumber];
						requestHandled   = `$INSTANCE_NAME`_InitControlRead();					
					}
                /* `#START AUDIO_REQUESTS` Place other request handler here */

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
                /* `#START AUDIO_REQUESTS` Place other request handler here */

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
					if(CY_GET_REG8(`$INSTANCE_NAME`_wValueHi) == `$INSTANCE_NAME`_SAMPLING_FREQ_CONTROL) 
                    {
					 	/* Endpoint Control Selector is Sampling Frequency */
						`$INSTANCE_NAME`_currentTD.wCount = `$INSTANCE_NAME`_SAMPLE_FREQ_LEN;
						`$INSTANCE_NAME`_currentTD.pData  = `$INSTANCE_NAME`_currentSampleFrequency[epNumber];
					    requestHandled   = `$INSTANCE_NAME`_InitControlWrite();						
					    `$INSTANCE_NAME`_frequencyChanged = epNumber;
					}
                /* `#START AUDIO_REQUESTS` Place other request handler here */

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
                /* `#START AUDIO_REQUESTS` Place other request handler here */

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
