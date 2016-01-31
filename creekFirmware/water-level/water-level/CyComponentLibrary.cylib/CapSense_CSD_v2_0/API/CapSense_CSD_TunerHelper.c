/*******************************************************************************
* File Name: `$INSTANCE_NAME`_TunerHelper.c
* Version `$CY_MAJOR_VERSION`.`$CY_MINOR_VERSION`
*
* Description:
*  This file provides the source code of Tuner helper APIs for the CapSense CSD 
*  component.
*
* Note:
*  None
*
********************************************************************************
* Copyright 2008-2010, Cypress Semiconductor Corporation.  All rights reserved.
* You may use this file only in accordance with the license, terms, conditions,
* disclaimers, and limitations in the end user license agreement accompanying
* the software package with which this file was provided.
*******************************************************************************/

#include "`$INSTANCE_NAME`_TunerHelper.h"

#if ((`$INSTANCE_NAME`_TUNER_API_GENERATE) && (`$INSTANCE_NAME`_TUNNING_METHOD != `$INSTANCE_NAME`_NO_TUNNING))
    void `$INSTANCE_NAME`_ProcessAllWidgets(void);
    
    volatile `$INSTANCE_NAME`_MAILBOXES `$INSTANCE_NAME`_mailboxesComm;
    
 #endif  /* End (`$INSTANCE_NAME`_TUNER_API_GENERATE) */


/*******************************************************************************
* Function Name: `$INSTANCE_NAME`_TunerStart
********************************************************************************
*
* Summary:
*  Initializes CapSense CSD component and EzI2C communication componenet and 
*  start the scanning.
*
* Parameters:
*  void
*
* Return:
*  void
*
* Reentrant:
*  No
*
*******************************************************************************/
void `$INSTANCE_NAME`_TunerStart(void)
{
    #if ((`$INSTANCE_NAME`_TUNER_API_GENERATE) && (`$INSTANCE_NAME`_TUNNING_METHOD != `$INSTANCE_NAME`_NO_TUNNING))
        /* Setup I2C buffers */
        `$EzI2CInstanceName`_Start();
        `$EzI2CInstanceName`_SetBuffer1(sizeof(`$INSTANCE_NAME`_mailboxesComm), sizeof(`$INSTANCE_NAME`_mailboxesComm),\
                                        (void *) &`$INSTANCE_NAME`_mailboxesComm);

        /* Init mbx and quick check */
        `$INSTANCE_NAME`_InitMailbox(&`$INSTANCE_NAME`_mailboxesComm.csdMailbox);
        `$INSTANCE_NAME`_mailboxesComm.numMailBoxes = `$INSTANCE_NAME`_DEFAULT_MAILBOXES_NUMBER;
    
        /* Start CapSense and baselines */
        `$INSTANCE_NAME`_Start();
        
        /* Initialize baselines */ 
        `$INSTANCE_NAME`_InitializeAllBaselines();
        
        /* Starts scan all sensors */
        `$INSTANCE_NAME`_ScanEnabledWidgets();
    
    #endif  /* End (`$INSTANCE_NAME`_TUNER_API_GENERATE) */
}


/*******************************************************************************
* Function Name: `$INSTANCE_NAME`_TunerComm
********************************************************************************
*
* Summary:
*  The paramters from tunner apply each time when scanning is complete. 
*  This function is block the main after scanning complete while Host scanning 
*  results from device.
*
* Parameters:
*  void
*
* Return:
*  void
*
* Reentrant:
*  No
*
*******************************************************************************/
void `$INSTANCE_NAME`_TunerComm(void)
{
    #if ((`$INSTANCE_NAME`_TUNER_API_GENERATE) && (`$INSTANCE_NAME`_TUNNING_METHOD != `$INSTANCE_NAME`_NO_TUNNING))   
        if (0u == `$INSTANCE_NAME`_IsBusy())
        {   
            /* Apply new settings */
            #if (`$INSTANCE_NAME`_TUNNING_METHOD == `$INSTANCE_NAME`_MANUAL_TUNNING)
                `$EzI2CInstanceName`_DisableInt();
                `$INSTANCE_NAME`_ReadMessage(&`$INSTANCE_NAME`_mailboxesComm.csdMailbox);
                `$EzI2CInstanceName`_EnableInt();
            #endif  /* End (`$INSTANCE_NAME`_TUNNING_METHOD == `$INSTANCE_NAME`_MANUAL_TUNNING) */
            
            /* Update all baselines and process all widgets */
            `$INSTANCE_NAME`_UpdateEnabledBaselines();
            `$INSTANCE_NAME`_ProcessAllWidgets();
                
            `$INSTANCE_NAME`_PostMessage(&`$INSTANCE_NAME`_mailboxesComm.csdMailbox);
                    
            /* Start scan all sensors */
            `$INSTANCE_NAME`_ScanEnabledWidgets();
        }
    #endif  /* End (`$INSTANCE_NAME`_TUNER_API_GENERATE) */
}


#if ((`$INSTANCE_NAME`_TUNER_API_GENERATE) && (`$INSTANCE_NAME`_TUNNING_METHOD != `$INSTANCE_NAME`_NO_TUNNING))
    /*******************************************************************************
    * Function Name: `$INSTANCE_NAME`_ProcessAllWidgets
    ********************************************************************************
    *
    * Summary:
    *  Call required all functions to update widgets state after scanning is 
    *  complete.
    *
    * Parameters:
    *  void
    *
    * Return:
    *  void
    *
    * Reentrant:
    *  No
    *
    *******************************************************************************/
    void `$INSTANCE_NAME`_ProcessAllWidgets(void)
    {
        #if (`$INSTANCE_NAME`_TOTAL_CENTROIDS_COUNT)
            uint8 i = 0u;
        #endif  /* End (`$INSTANCE_NAME`_TOTAL_CENTROIDS_COUNT)*/
        
        #if ((`$INSTANCE_NAME`_TOTAL_RADIAL_SLIDERS_COUNT) || (`$INSTANCE_NAME`_TOTAL_TOUCH_PADS_COUNT))
            uint8 widgetCnt = 0u;
        #endif  /* End ((`$INSTANCE_NAME`_TOTAL_RADIAL_SLIDERS_COUNT) || (`$INSTANCE_NAME`_TOTAL_TOUCH_PADS_COUNT)) */
        
        /* Update On/Off State */
        `$INSTANCE_NAME`_CheckIsAnyWidgetActive();	
        
        /* Calculate widget with centroids */
        #if (`$INSTANCE_NAME`_TOTAL_LINEAR_SLIDERS_COUNT)
            for(; i < `$INSTANCE_NAME`_TOTAL_LINEAR_SLIDERS_COUNT; i++)
            {
                `$INSTANCE_NAME`_GetCentroidPos(i);
            }
        #endif   /* End (`$INSTANCE_NAME`_TOTAL_LINEAR_SLIDERS_COUNT) */
        
        #if (`$INSTANCE_NAME`_TOTAL_RADIAL_SLIDERS_COUNT)
            widgetCnt = i;
            for(; i < widgetCnt + `$INSTANCE_NAME`_TOTAL_RADIAL_SLIDERS_COUNT; i++)
            {
                `$INSTANCE_NAME`_GetRadialCentroidPos(i);
            }           
        #endif  /* End (`$INSTANCE_NAME`_TOTAL_RADIAL_SLIDERS_COUNT) */
        
        #if (`$INSTANCE_NAME`_TOTAL_TOUCH_PADS_COUNT)
            widgetCnt = i;
            for(; i < widgetCnt + (`$INSTANCE_NAME`_TOTAL_TOUCH_PADS_COUNT * 2); i=i+2)
            {
                `$INSTANCE_NAME`_GetTouchCentroidPos(i);
            }
        #endif  /* End (`$INSTANCE_NAME`_TOTAL_TOUCH_PADS_COUNT) */
    }
#endif /* End (`$INSTANCE_NAME`_TUNER_API_GENERATE) */


/* [] END OF FILE */
