/*******************************************************************************
* File Name: `$INSTANCE_NAME`_TunerHelper.c
* Version `$CY_MAJOR_VERSION`.`$CY_MINOR_VERSION`
*
* Description:
*  This file provides the source code of Tuner helper APIs for the CapSense CSD 
*  component.
*
* Note:
*
********************************************************************************
* Copyright 2008-2011, Cypress Semiconductor Corporation.  All rights reserved.
* You may use this file only in accordance with the license, terms, conditions,
* disclaimers, and limitations in the end user license agreement accompanying
* the software package with which this file was provided.
*******************************************************************************/

#include "`$INSTANCE_NAME`_TunerHelper.h"

#if (`$INSTANCE_NAME`_TUNER_API_GENERATE)
    void `$INSTANCE_NAME`_ProcessAllWidgets(volatile `$INSTANCE_NAME`_OUTBOX *outbox);
    
    volatile `$INSTANCE_NAME`_MAILBOXES `$INSTANCE_NAME`_mailboxesComm;

    extern uint8 `$INSTANCE_NAME`_SensorOnMask[(((`$INSTANCE_NAME`_TOTAL_SENSOR_COUNT - 1u) / 8u) + 1u)];
#endif  /* End (`$INSTANCE_NAME`_TUNER_API_GENERATE) */


/*******************************************************************************
* Function Name: `$INSTANCE_NAME`_TunerStart
********************************************************************************
*
* Summary:
*  Initializes CapSense CSD component and EzI2C communication componenet to use
*  mailbox data structure for communication with Tuner GUI.
*  Start the scanning, after initialization complete.
*
* Parameters:
*  None
*
* Return:
*  None
*
* Reentrant:
*  No
*
*******************************************************************************/
void `$INSTANCE_NAME`_TunerStart(void)
{
    #if (`$INSTANCE_NAME`_TUNER_API_GENERATE)
        
        /* Init mbx and quick check */
        `$INSTANCE_NAME`_InitMailbox(&`$INSTANCE_NAME`_mailboxesComm.csdMailbox);
        `$INSTANCE_NAME`_mailboxesComm.numMailBoxes = `$INSTANCE_NAME`_DEFAULT_MAILBOXES_NUMBER;
        
        /* Start CapSense and baselines */
        `$INSTANCE_NAME`_Start();
        
        /* Initialize baselines */ 
        `$INSTANCE_NAME`_InitializeAllBaselines();
        `$INSTANCE_NAME`_InitializeAllBaselines();
        
        /* Start EzI2C, clears buf pointers */
        `$EzI2CInstanceName`_Start();
        
        /* Setup EzI2C buffers */
        `$EzI2CInstanceName`_SetBuffer1(sizeof(`$INSTANCE_NAME`_mailboxesComm), sizeof(`$INSTANCE_NAME`_mailboxesComm),
                                        (void *) &`$INSTANCE_NAME`_mailboxesComm);
        
        /* Starts scan all enabled sensors */
        `$INSTANCE_NAME`_ScanEnabledWidgets();
    
    #endif  /* End (`$INSTANCE_NAME`_TUNER_API_GENERATE) */
}


/*******************************************************************************
* Function Name: `$INSTANCE_NAME`_TunerComm
********************************************************************************
*
* Summary:
*  This function is blocking. It waits till scaning loop is completed and apply
*  new parameters from Tuner GUI if available (manual tuning mode only). Updates
*  enabled baselines and state of widgets. Waits while Tuner GUI reports that 
*  content of mailbox could be modified. Then loads the report data into outbox 
*  and sets the busy flag. Starts new scanning loop.
*  
* Parameters:
*  None
*
* Return:
*  None
*
* Reentrant:
*  No
*
*******************************************************************************/
void `$INSTANCE_NAME`_TunerComm(void)
{
    #if (`$INSTANCE_NAME`_TUNER_API_GENERATE)
        if (0u == `$INSTANCE_NAME`_IsBusy())
        {   
            /* Apply new settings */
            #if (`$INSTANCE_NAME`_TUNING_METHOD == `$INSTANCE_NAME`_MANUAL_TUNING)
                `$INSTANCE_NAME`_ReadMessage(&`$INSTANCE_NAME`_mailboxesComm.csdMailbox);
            #endif  /* End (`$INSTANCE_NAME`_TUNING_METHOD == `$INSTANCE_NAME`_MANUAL_TUNING) */

            /* Update all baselines and process all widgets */
            `$INSTANCE_NAME`_UpdateEnabledBaselines();
            `$INSTANCE_NAME`_ProcessAllWidgets(&`$INSTANCE_NAME`_mailboxesComm.csdMailbox.outbox);
            `$INSTANCE_NAME`_PostMessage(&`$INSTANCE_NAME`_mailboxesComm.csdMailbox);

            /* Enable EZI2C interrupts, after scan complete */
            `$EzI2CInstanceName`_EnableInt();

            while((`$INSTANCE_NAME`_mailboxesComm.csdMailbox.type != `$INSTANCE_NAME`_TYPE_ID) || \
                  (`$EzI2CInstanceName`_GetActivity() & `$EzI2CInstanceName`_STATUS_BUSY)){}
            
            /* Disable EZI2C interrupts, while scanning */
            `$EzI2CInstanceName`_DisableInt();
            
            /* Start scan all sensors */
            `$INSTANCE_NAME`_ScanEnabledWidgets();
        }
    #endif  /* End (`$INSTANCE_NAME`_TUNER_API_GENERATE) */
}


#if (`$INSTANCE_NAME`_TUNER_API_GENERATE)
    /*******************************************************************************
    * Function Name: `$INSTANCE_NAME`_ProcessAllWidgets
    ********************************************************************************
    *
    * Summary:
    *  Call required functions to update all widgets state:
    *   - `$INSTANCE_NAME`_GetCentroidPos() - calls only if linear sliders 
    *     available.
    *   - `$INSTANCE_NAME`_GetRadialCentroidPos() - calls only if radial slider 
    *     available.
    *   - `$INSTANCE_NAME`_GetTouchCentroidPos() - calls only if touch pad slider 
    *     available.
    *   - `$INSTANCE_NAME`_CheckIsAnyWidgetActive();
    *  The results of opeartions are copied to OUTBOX.
    *   
    * Parameters:
    *  None
    *
    * Return:
    *  None
    *
    * Global Variables:
    *  `$INSTANCE_NAME`_OUTBOX outbox - structure which is used as ouput 
    *  buffer for report data to Tuner GUI.
    *  Update fields:
    *    - position[];
    *    - OnMask[];
    *
    * Reentrant:
    *  No
    *
    *******************************************************************************/
    void `$INSTANCE_NAME`_ProcessAllWidgets(volatile `$INSTANCE_NAME`_OUTBOX *outbox)
    {
        uint8 i = 0u;

        #if (`$INSTANCE_NAME`_TOTAL_TOUCH_PADS_COUNT)
            uint16 pos[2];
        #endif  /* End (`$INSTANCE_NAME`_TOTAL_TOUCH_PADS_COUNT) */
        
        #if ( (`$INSTANCE_NAME`_TOTAL_RADIAL_SLIDERS_COUNT) || \
              (`$INSTANCE_NAME`_TOTAL_TOUCH_PADS_COUNT) || \
              (`$INSTANCE_NAME`_TOTAL_MATRIX_BUTTONS_COUNT) )
            uint8 widgetCnt = 0u;
        #endif  /* End ((`$INSTANCE_NAME`_TOTAL_RADIAL_SLIDERS_COUNT) || (`$INSTANCE_NAME`_TOTAL_TOUCH_PADS_COUNT)) || 
                        (`$INSTANCE_NAME`_TOTAL_MATRIX_BUTTONS_COUNT) */
        
        /* Calculate widget with centroids */
        #if (`$INSTANCE_NAME`_TOTAL_LINEAR_SLIDERS_COUNT)
            for(; i < `$INSTANCE_NAME`_TOTAL_LINEAR_SLIDERS_COUNT; i++)
            {
                outbox->position[i] = `$INSTANCE_NAME`_SWAP_ENDIAN16(`$INSTANCE_NAME`_GetCentroidPos(i));
            }
        #endif   /* End (`$INSTANCE_NAME`_TOTAL_LINEAR_SLIDERS_COUNT) */
        
        #if (`$INSTANCE_NAME`_TOTAL_RADIAL_SLIDERS_COUNT)
            widgetCnt = i;
            for(; i < widgetCnt + `$INSTANCE_NAME`_TOTAL_RADIAL_SLIDERS_COUNT; i++)
            {
                outbox->position[i] = `$INSTANCE_NAME`_SWAP_ENDIAN16(`$INSTANCE_NAME`_GetRadialCentroidPos(i));
            }
        #endif  /* End (`$INSTANCE_NAME`_TOTAL_RADIAL_SLIDERS_COUNT) */
        
        #if (`$INSTANCE_NAME`_TOTAL_TOUCH_PADS_COUNT)
            widgetCnt = i;
            for(; i < (widgetCnt + (`$INSTANCE_NAME`_TOTAL_TOUCH_PADS_COUNT * 2)); i=i+2)
            {
                if(`$INSTANCE_NAME`_GetTouchCentroidPos(i, pos) == 0u)
                {
                    outbox->position[i] = 0xFFFFu;
                    outbox->position[i+1] = 0xFFFFu;
                }
                else
                {
                    outbox->position[i] = `$INSTANCE_NAME`_SWAP_ENDIAN16( (uint16) pos[0u]);
                    outbox->position[i+1] = `$INSTANCE_NAME`_SWAP_ENDIAN16( (uint16) pos[1u]);
                }
            }
        #endif  /* End (`$INSTANCE_NAME`_TOTAL_TOUCH_PADS_COUNT) */

        #if (`$INSTANCE_NAME`_TOTAL_MATRIX_BUTTONS_COUNT)
            i += `$INSTANCE_NAME`_TOTAL_BUTTONS_COUNT;
            widgetCnt = 0;
            for(; widgetCnt < (`$INSTANCE_NAME`_TOTAL_MATRIX_BUTTONS_COUNT * 2); i += 2)
            {
                if(`$INSTANCE_NAME`_GetMatrixButtonPos(i, ((uint8*) &outbox->mb_position[widgetCnt])) == 0)
                {
                    outbox->mb_position[widgetCnt] = 0xFFu;
                    outbox->mb_position[widgetCnt+1] = 0xFF;
                }
                widgetCnt += 2;
            }
        #endif /* End (`$INSTANCE_NAME`_TOTAL_MATRIX_BUTTONS_COUNT)*/

        /* Update On/Off State */
        `$INSTANCE_NAME`_CheckIsAnyWidgetActive();

        /* Copy OnMask */
        for(i=0; i < `$INSTANCE_NAME`_TOTAL_SENSOR_MASK_COUNT; i++)
        {
            outbox->onMask[i]  = `$INSTANCE_NAME`_SensorOnMask[i];
        }
    }
#endif /* End (`$INSTANCE_NAME`_TUNER_API_GENERATE) */


/* [] END OF FILE */
