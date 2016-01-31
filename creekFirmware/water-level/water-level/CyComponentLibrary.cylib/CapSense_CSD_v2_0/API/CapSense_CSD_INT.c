/*******************************************************************************
* File Name: `$INSTANCE_NAME`_INT.c
* Version `$CY_MAJOR_VERSION`.`$CY_MINOR_VERSION`
*
* Description:
*  This file provides the source code of Interrupt Service Routine (ISR)
*  for CapSense CSD component.
*
* Note:
*
********************************************************************************
* Copyright 2008-2010, Cypress Semiconductor Corporation.  All rights reserved.
* You may use this file only in accordance with the license, terms, conditions,
* disclaimers, and limitations in the end user license agreement accompanying
* the software package with which this file was provided.
*******************************************************************************/

#include "`$INSTANCE_NAME`.h"

/* Extern functions declarations, Start and Compete the scan */
extern void `$INSTANCE_NAME`_PreScan(uint8 sensor) `=ReentrantKeil($INSTANCE_NAME . "_PreScan")`;
#if (`$INSTANCE_NAME`_DESIGN_TYPE == `$INSTANCE_NAME`_ONE_CHANNEL_DESIGN)
    extern void `$INSTANCE_NAME`_PostScan(uint8 sensor);
#else
    extern void `$INSTANCE_NAME`_PostScanCh0(uint8 sensor);
    extern void `$INSTANCE_NAME`_PostScanCh1(uint8 sensor);
#endif  /* End `$INSTANCE_NAME`_DESIGN_TYPE */

 #if (`$INSTANCE_NAME`_DESIGN_TYPE == `$INSTANCE_NAME`_ONE_CHANNEL_DESIGN)
    extern uint8 `$INSTANCE_NAME`_FindNextSensor(uint8 snsIndex) `=ReentrantKeil($INSTANCE_NAME . "_FindNextSensor")`;
#endif  /* End (`$INSTANCE_NAME`_DESIGN_TYPE == `$INSTANCE_NAME`_ONE_CHANNEL_DESIGN) */
 
#if (`$INSTANCE_NAME`_DESIGN_TYPE == `$INSTANCE_NAME`_TWO_CHANNELS_DESIGN)
    extern uint8 `$INSTANCE_NAME`_FindNextPair(uint8 snsIndex) `=ReentrantKeil($INSTANCE_NAME . "_FindNextPair")`;
#endif  /* End (`$INSTANCE_NAME`_DESIGN_TYPE == `$INSTANCE_NAME`_TWO_CHANNELS_DESIGN) */

/* Global variables and arrays */
extern volatile uint8 `$INSTANCE_NAME`_csv;
extern volatile uint8 `$INSTANCE_NAME`_sensorIndex;
extern uint8 `$INSTANCE_NAME`_SensorEnableMask[(((`$INSTANCE_NAME`_TOTAL_SENSOR_COUNT - 1u) / 8u) + 1u)];


 #if (`$INSTANCE_NAME`_DESIGN_TYPE == `$INSTANCE_NAME`_ONE_CHANNEL_DESIGN)
    /*******************************************************************************
    *  Place your includes, defines and code here
    *******************************************************************************/
    /* `#START `$INSTANCE_NAME`_IsrCH0_ISR_inc` */
    
    /* `#END` */
    
    /*******************************************************************************
    * Function Name: `$INSTANCE_NAME`_IsrCH0_ISR
    ********************************************************************************
    * Summary:
    *  This ISR is executed when PWM window is closed. The widow depends on PWM 
    *  resolution paramter. This interrupt handler only used in one channel designs.
    *
    * Parameters:
    *  void:
    *
    * Return:
    *  void
    *
    * Reentrant:
    *  No
    *
    *******************************************************************************/
    CY_ISR(`$INSTANCE_NAME`_IsrCH0_ISR)
    {
        /*  Place your Interrupt code here. */
        /* `#START `$INSTANCE_NAME`_IsrCH0_ISR` */
    
        /* `#END` */
    
        /* Save results and disable sensor */
        `$INSTANCE_NAME`_PostScan(`$INSTANCE_NAME`_sensorIndex);
                
        if ((`$INSTANCE_NAME`_csv & `$INSTANCE_NAME`_SW_CTRL_SINGLE_SCAN) != 0u)
        {
            `$INSTANCE_NAME`_csv &= ~`$INSTANCE_NAME`_SW_STS_BUSY;
        }
        else
        {
            /* Procced the scanning */
            `$INSTANCE_NAME`_sensorIndex = `$INSTANCE_NAME`_FindNextSensor(`$INSTANCE_NAME`_sensorIndex);
            
            /* Check end of scan */
            if(`$INSTANCE_NAME`_sensorIndex < `$INSTANCE_NAME`_TOTAL_SENSOR_COUNT)
            {
                `$INSTANCE_NAME`_PreScan(`$INSTANCE_NAME`_sensorIndex);
            }
            else
            {
                `$INSTANCE_NAME`_csv &= ~`$INSTANCE_NAME`_SW_STS_BUSY;
            } 
        }
    
        #if (`$INSTANCE_NAME`_PSOC3_ES2 && (`$INSTANCE_NAME`_IsrCH0__ES2_PATCH))
                `$INSTANCE_NAME`_ISR_PATCH();
        #endif /* End (`$INSTANCE_NAME`_PSOC3_ES2 && (`$INSTANCE_NAME`_IsrCH0__ES2_PATCH)) */
    }
 #endif  /* End `$INSTANCE_NAME`_DESIGN_TYPE */


 #if (`$INSTANCE_NAME`_DESIGN_TYPE == `$INSTANCE_NAME`_TWO_CHANNELS_DESIGN)
    /*******************************************************************************
    *  Place your includes, defines and code here
    *******************************************************************************/
    /* `#START `$INSTANCE_NAME`_IsrCH0_ISR_inc` */
    
    /* `#END` */
    
    /*******************************************************************************
    * Function Name: `$INSTANCE_NAME`_IsrCH0_ISR
    ********************************************************************************
    * Summary:
    *  This ISR is executed when PWM window is closed. The widow depends on PWM 
    *  resolution paramter. This interrupt handler only used in two channel designs.
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
    CY_ISR(`$INSTANCE_NAME`_IsrCH0_ISR)
    {
        /*  Place your Interrupt code here. */
        /* `#START `$INSTANCE_NAME`_IsrCH0_ISR` */
    
        /* `#END` */
        
        `$INSTANCE_NAME`_csv |= `$INSTANCE_NAME`_SW_STS_SCAN_DONE__CH0;
        `$INSTANCE_NAME`_PostScanCh0(`$INSTANCE_NAME`_sensorIndex);
        
        if((`$INSTANCE_NAME`_csv & `$INSTANCE_NAME`_SW_STS_SCAN_DONE__CH1) != 0u)
        {
            if ((`$INSTANCE_NAME`_csv & `$INSTANCE_NAME`_SW_CTRL_SINGLE_SCAN) != 0u)
            {
                `$INSTANCE_NAME`_csv &= ~`$INSTANCE_NAME`_SW_STS_BUSY;
            }
            else
            {
                /* Procced the scanning */
                `$INSTANCE_NAME`_sensorIndex = `$INSTANCE_NAME`_FindNextPair(`$INSTANCE_NAME`_sensorIndex);
                
                /* Check end of scan conditions */
                if( ((`$INSTANCE_NAME`_csv & `$INSTANCE_NAME`_SW_STS_SCANS_CMPLT__CH0) != 0u) && \
                    ((`$INSTANCE_NAME`_csv & `$INSTANCE_NAME`_SW_STS_SCANS_CMPLT__CH1) != 0u) )
                {
                    `$INSTANCE_NAME`_csv &= ~`$INSTANCE_NAME`_SW_STS_BUSY;
                }
                else
                {
                    `$INSTANCE_NAME`_PreScan(`$INSTANCE_NAME`_sensorIndex);        
                }
            }
        }
        else
        {
            /* Do nothing */
        }
    
        #if (`$INSTANCE_NAME`_PSOC3_ES2 && (`$INSTANCE_NAME`_IsrCH0__ES2_PATCH))
                `$INSTANCE_NAME`_ISR_PATCH();
        #endif /* End (`$INSTANCE_NAME`_PSOC3_ES2 && (`$INSTANCE_NAME`_IsrCH0__ES2_PATCH)) */
    }
    
    
    /*******************************************************************************
    *  Place your includes, defines and code here
    *******************************************************************************/
    /* `#START `$INSTANCE_NAME`_IsrCH1_ISR_inc` */
    
    /* `#END` */
    
    /*******************************************************************************
    * Function Name: `$INSTANCE_NAME`_IsrCH1_ISR
    ********************************************************************************
    * Summary:
    *  This ISR is executed when PWM window is closed. The widow depends on PWM 
    *  resolution paramter. This interrupt handler only used in two channel designs.
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
    CY_ISR(`$INSTANCE_NAME`_IsrCH1_ISR)
    {
        /*  Place your Interrupt code here. */
        /* `#START `$INSTANCE_NAME`_IsrCH1_ISR` */
    
        /* `#END` */
        
        `$INSTANCE_NAME`_csv |= `$INSTANCE_NAME`_SW_STS_SCAN_DONE__CH1;
        `$INSTANCE_NAME`_PostScanCh1(`$INSTANCE_NAME`_sensorIndex + `$INSTANCE_NAME`_TOTAL_SENSOR_COUNT__CH0);
        
        if((`$INSTANCE_NAME`_csv & `$INSTANCE_NAME`_SW_STS_SCAN_DONE__CH0) != 0u)
        {
            if ((`$INSTANCE_NAME`_csv & `$INSTANCE_NAME`_SW_CTRL_SINGLE_SCAN) != 0u)
            {
                `$INSTANCE_NAME`_csv &= ~`$INSTANCE_NAME`_SW_STS_BUSY;
            }
            else 
            {
                /* Procced the scanning */
                `$INSTANCE_NAME`_sensorIndex = `$INSTANCE_NAME`_FindNextPair(`$INSTANCE_NAME`_sensorIndex);
                
                /* Check end of scan conditions */
                if( ((`$INSTANCE_NAME`_csv & `$INSTANCE_NAME`_SW_STS_SCANS_CMPLT__CH0) != 0u) && \
                    ((`$INSTANCE_NAME`_csv & `$INSTANCE_NAME`_SW_STS_SCANS_CMPLT__CH1) != 0u) )
                {
                    `$INSTANCE_NAME`_csv &= ~`$INSTANCE_NAME`_SW_STS_BUSY;
                }
                else
                {
                    `$INSTANCE_NAME`_PreScan(`$INSTANCE_NAME`_sensorIndex);
                }
            }
        }
        else
        {
            /* Do nothing */
        }
    
        #if (`$INSTANCE_NAME`_PSOC3_ES2 && (`$INSTANCE_NAME`_IsrCH1__ES2_PATCH))
                `$INSTANCE_NAME`_ISR_PATCH();
        #endif /* End (`$INSTANCE_NAME`_PSOC3_ES2 && (`$INSTANCE_NAME`_IsrCH1__ES2_PATCH)) */
    }
#endif  /* End `$INSTANCE_NAME`_DESIGN_TYPE */


/* [] END OF FILE */
