/*******************************************************************************
* File Name: `$INSTANCE_NAME`_MBX.c
* Version `$CY_MAJOR_VERSION`.`$CY_MINOR_VERSION`
*
* Description:
*  This file provides the source code of tuner APIs for the CapSense CSD
*  component.
*
* Note:
*
********************************************************************************
* Copyright 2008-2010, Cypress Semiconductor Corporation.  All rights reserved.
* You may use this file only in accordance with the license, terms, conditions,
* disclaimers, and limitations in the end user license agreement accompanying
* the software package with which this file was provided.
*******************************************************************************/

#include "`$INSTANCE_NAME`_MBX.h"

#if (`$INSTANCE_NAME`_CURRENT_SOURCE)
    extern uint8 `$INSTANCE_NAME`_idacSettings[];
#endif /* End `$INSTANCE_NAME`_CURRENT_SOURCE */

extern uint8 `$INSTANCE_NAME`_widgetResolution[];
extern uint16 `$INSTANCE_NAME`_SensorRaw[`$INSTANCE_NAME`_TOTAL_SENSOR_COUNT];
extern const uint8 CYCODE `$INSTANCE_NAME`_widgetNumber[];

extern uint16 `$INSTANCE_NAME`_SensorBaseline[`$INSTANCE_NAME`_TOTAL_SENSOR_COUNT];
extern `$SignalSizeReplacementString` `$INSTANCE_NAME`_SensorSignal[`$INSTANCE_NAME`_TOTAL_SENSOR_COUNT];
extern uint8 `$INSTANCE_NAME`_SensorOnMask[(((`$INSTANCE_NAME`_TOTAL_SENSOR_COUNT - 1u) / 8u) + 1u)];

#if (`$INSTANCE_NAME`_TUNNING_METHOD == `$INSTANCE_NAME`_MANUAL_TUNNING)
    extern uint8 `$INSTANCE_NAME`_fingerThreshold[];
    extern uint8 `$INSTANCE_NAME`_noiseThreshold[];
    extern uint8 `$INSTANCE_NAME`_hysteresis[];
    extern uint8 `$INSTANCE_NAME`_debounce[];
    
#elif (`$INSTANCE_NAME`_TUNNING_METHOD == `$INSTANCE_NAME`_AUTO_TUNNING)
    extern uint8 `$INSTANCE_NAME`_fingerThreshold[];
    extern uint8 `$INSTANCE_NAME`_noiseThreshold[];
    extern uint8 `$INSTANCE_NAME`_selectedPrescaler;
#else
    /* No tunning */
#endif  /* End `$INSTANCE_NAME`_TUNNING_METHOD */

#if (`$INSTANCE_NAME`_TOTAL_CENTROIDS_COUNT && (`$INSTANCE_NAME`_TUNNING_METHOD != `$INSTANCE_NAME`_NO_TUNNING))
	extern uint16 `$INSTANCE_NAME`_position[`$INSTANCE_NAME`_TOTAL_CENTROIDS_COUNT];
#endif  /* End (`$INSTANCE_NAME`_TOTAL_CENTROIDS_COUNT) */ 


#if (`$INSTANCE_NAME`_TUNNING_METHOD == `$INSTANCE_NAME`_MANUAL_TUNNING)
    /*******************************************************************************
    * Function Name: `$INSTANCE_NAME`_InitParams
    ********************************************************************************
    *
    * Summary:
    *  Configures component to match the parameters in the inbox.
    *
    * Parameters:
    *  inbox:  Pointer to Inbox structure.  Contents of the structure are not modified.
    *
    * Return:
    *  None
    * 
    * Side Effects: 
    *  Resets baseline values. 
    *  
    * Reentrant:
    *  No
    *
    *******************************************************************************/
    static void `$INSTANCE_NAME`_InitParams(volatile `$INSTANCE_NAME`_INBOX *inbox)
    {
        uint8 sensor, widget;
        
        /* Define widget sensor belongs to */
        sensor = inbox->sensorIndex;
        widget = `$INSTANCE_NAME`_widgetNumber[sensor];
        
        /* Scanning parameters */
        #if (`$INSTANCE_NAME`_CURRENT_SOURCE)
            `$INSTANCE_NAME`_idacSettings[sensor] = inbox->idacSettings;
        #endif /* End `$INSTANCE_NAME`_CURRENT_SOURCE */   
        `$INSTANCE_NAME`_widgetResolution[widget] = inbox->resolution;
    
        /* High level parameters */
        `$INSTANCE_NAME`_fingerThreshold[widget] = inbox->fingerThreshold;
        `$INSTANCE_NAME`_noiseThreshold[widget]  = inbox->noiseThreshold;
        `$INSTANCE_NAME`_hysteresis[widget]      = inbox->hysteresis;
        `$INSTANCE_NAME`_debounce[widget]        = inbox->debounce;
                    
        /* Re-Init baselines */
        `$INSTANCE_NAME`_InitializeAllBaselines();    
    }
#endif  /* End (`$INSTANCE_NAME`_TUNNING_METHOD == `$INSTANCE_NAME`_MANUAL_TUNNING) */


/*******************************************************************************
* Function Name: `$INSTANCE_NAME`_InitMailbox
********************************************************************************
*
* Summary:
*  Initialize paramters of mailbox structure.
*
* Parameters:
*  mbx:  Pointer to Mailbox structure.
*
* Return:
*  Modifies the contents of mbx.
*
* Reentrant:
*  No
*
*******************************************************************************/
void `$INSTANCE_NAME`_InitMailbox(volatile `$INSTANCE_NAME`_MAILBOX *mbx)
{
    /* Restore TYPE_ID (clear busy flag) to indicate "action complete" */
    mbx->type = `$INSTANCE_NAME`_TYPE_ID;
    /* Restore default value - clear "have_msg" */
    mbx->size = `$INSTANCE_NAME`_SWAP_ENDIAN16(sizeof(`$INSTANCE_NAME`_MAILBOX)); 
    
    /* Addtional fields for sync and reset handling */
    mbx->outbox.checkSum = `$INSTANCE_NAME`_SWAP_ENDIAN16(`$INSTANCE_NAME`_CHECK_SUM); 
    mbx->outbox.noReadMsg = 1u;
}


/*******************************************************************************
* Function Name:  `$INSTANCE_NAME`_InitMailbox
********************************************************************************
*
* Summary:
*  Loads outbox with the report data.  Sets the busy_flag, waits for the host 
*  to clear the busy flag.
*
* Parameters:
*  mbx:  Pointer to Mailbox structure.
*
* Return:
*  Modifies the contents of mbx.
*
* Reentrant:
*  No
*
*******************************************************************************/
void `$INSTANCE_NAME`_PostMessage(volatile `$INSTANCE_NAME`_MAILBOX *mbx)
{
    uint8 i;

    /* Check busy flag */
    while (mbx->type != `$INSTANCE_NAME`_TYPE_ID);

    /* Copy centroids positions if exist */
    #if (`$INSTANCE_NAME`_TOTAL_CENTROIDS_COUNT && (`$INSTANCE_NAME`_TUNNING_METHOD != `$INSTANCE_NAME`_NO_TUNNING))
        for(i=0; i < `$INSTANCE_NAME`_TOTAL_CENTROIDS_COUNT; i++)
        {
            mbx->outbox.position[i] = `$INSTANCE_NAME`_SWAP_ENDIAN16(`$INSTANCE_NAME`_position[i]);
        }
    #endif  /* End (`$INSTANCE_NAME`_TOTAL_CENTROIDS_COUNT) */
    
    /* Copy all data - Raw, Base, Signal, OnMask */
    for(i=0; i < `$INSTANCE_NAME`_TOTAL_SENSOR_COUNT; i++)
    {
        mbx->outbox.rawData[i]  = `$INSTANCE_NAME`_SWAP_ENDIAN16(`$INSTANCE_NAME`_SensorRaw[i]);
        mbx->outbox.baseLine[i] = `$INSTANCE_NAME`_SWAP_ENDIAN16(`$INSTANCE_NAME`_SensorBaseline[i]);
        #if (`$INSTANCE_NAME`_SIGNAL_SIZE == `$INSTANCE_NAME`_SIGNAL_SIZE_UINT8)
            mbx->outbox.signal[i]   = `$INSTANCE_NAME`_SensorSignal[i];
        #else
            mbx->outbox.signal[i]   = `$INSTANCE_NAME`_SWAP_ENDIAN16(`$INSTANCE_NAME`_SensorSignal[i]);
        #endif  /* End (`$INSTANCE_NAME`_SIGNAL_SIZE == `$INSTANCE_NAME`_SIGNAL_SIZE_UINT8) */
    }
    
    /* Copy OnMask */
    for(i=0; i < `$INSTANCE_NAME`_TOTAL_SENSOR_MASK_COUNT; i++)
    {
        mbx->outbox.onMask[i]  = `$INSTANCE_NAME`_SensorOnMask[i];
    }
    
    /* AutoTuning - need to copy all High Level parameters */
    #if (`$INSTANCE_NAME`_TUNNING_METHOD == `$INSTANCE_NAME`_AUTO_TUNNING)
        /* Copy sensor threshoulds */
        for(i=0; i < `$INSTANCE_NAME`_TOTAL_WIDGET_COUNT; i++)
        {
            mbx->outbox.fingerThreshold[i] = `$INSTANCE_NAME`_fingerThreshold[i];
            mbx->outbox.noiseThreshold[i]  = `$INSTANCE_NAME`_noiseThreshold[i];
            mbx->outbox.scanResolution[i]  = `$INSTANCE_NAME`_widgetResolution[i];
        }
        
        /* Generic goes the after all widgets */
        #if (`$INSTANCE_NAME`_TOTAL_GENERICS_COUNT)
            for(; i < (`$INSTANCE_NAME`_TOTAL_WIDGET_COUNT + `$INSTANCE_NAME`_TOTAL_GENERICS_COUNT); i++)
            {
                mbx->outbox.scanResolution[i]  = `$INSTANCE_NAME`_widgetResolution[i];
            }
        #endif /* (`$INSTANCE_NAME`_TOTAL_GENERICS_COUNT) */
        
        /* Copy tuned idac values */
        for(i=0; i < `$INSTANCE_NAME`_TOTAL_SENSOR_COUNT; i++)
        {
            mbx->outbox.idacValue[i] = `$INSTANCE_NAME`_idacSettings[i];
        }
        
        /* Copy scan parameters */
        mbx->outbox.prescaler = `$INSTANCE_NAME`_selectedPrescaler;
        
    #endif  /* End (`$INSTANCE_NAME`_TUNNING_METHOD == `$INSTANCE_NAME`_MANUAL_TUNNING) */

    /* Set busy flag */
    mbx->type = `$INSTANCE_NAME`_BUSY_FLAG;
    
}


#if (`$INSTANCE_NAME`_TUNNING_METHOD == `$INSTANCE_NAME`_MANUAL_TUNNING)
    /*******************************************************************************
    * Function Name: `$INSTANCE_NAME`_ReadMessage
    ********************************************************************************
    *
    * Summary:
    *  If `$INSTANCE_NAME`_HAVE_MSG is found in the buffer, initialize component 
    *  with parameters found in the buffer, signal DONE by overwriting the
    *  value in size.
    *
    * Parameters:
    *  mbx:  Pointer to Mailbox structure in RAM
    *
    * Return:
    *  Modifies the contents of mbx.
    *
    * Side Effects: 
    *  Initializes component parameters if `$INSTANCE_NAME`_HAVE_MSG is received.
    *
    * Reentrant:
    *  No
    *
    *******************************************************************************/
    void `$INSTANCE_NAME`_ReadMessage(volatile `$INSTANCE_NAME`_MAILBOX *mbx)
    {
        volatile `$INSTANCE_NAME`_INBOX *tmpInbox;
        /* Do we have a message waiting? */
        if (`$INSTANCE_NAME`_SWAP_ENDIAN16(mbx->size) == `$INSTANCE_NAME`_HAVE_MSG)
        {
            tmpInbox = &(mbx->inbox);
            
            `$INSTANCE_NAME`_InitParams(tmpInbox);
    
            /* Notify host/tuner that we have consumed the message */
            mbx->size = `$INSTANCE_NAME`_SWAP_ENDIAN16(sizeof(`$INSTANCE_NAME`_MAILBOX));
            mbx->outbox.noReadMsg = 0u;
        }       
    }
#endif  /* End (`$INSTANCE_NAME`_TUNNING_METHOD == `$INSTANCE_NAME`_MANUAL_TUNNING) */


/* [] END OF FILE */
