/*******************************************************************************
* File Name: `$INSTANCE_NAME`_CSHL.c
* Version `$CY_MAJOR_VERSION`.`$CY_MINOR_VERSION`
*
* Description:
*  This file provides the source code to the High Level APIs for the CapSesne
*  CSD component.
*
* Note:
*
********************************************************************************
* Copyright 2008-2010, Cypress Semiconductor Corporation.  All rights reserved.
* You may use this file only in accordance with the license, terms, conditions,
* disclaimers, and limitations in the end user license agreement accompanying
* the software package with which this file was provided.
*******************************************************************************/

#include "`$INSTANCE_NAME`_CSHL.h"

/* SmartSense functions */
#if (`$INSTANCE_NAME`_TUNNING_METHOD == `$INSTANCE_NAME`_AUTO_TUNNING)
	extern void `$INSTANCE_NAME`_CalculateThresholds(uint8 SensorNumber);
#endif /* End (`$INSTANCE_NAME`_TUNNING_METHOD == `$INSTANCE_NAME`_AUTO_TUNNING) */

/* Median filter function prototype */
#if ( (`$INSTANCE_NAME`_RAW_FILTER_MASK & `$INSTANCE_NAME`_MEDIAN_FILTER) || \
      (`$INSTANCE_NAME`_POS_FILTERS_MASK & `$INSTANCE_NAME`_MEDIAN_FILTER) )
    uint16 `$INSTANCE_NAME`_MedianFilter(uint16 x1, uint16 x2, uint16 x3) `=ReentrantKeil($INSTANCE_NAME . "_MedianFilter")`;
#endif /* End `$INSTANCE_NAME`_RAW_FILTER_MASK && `$INSTANCE_NAME`_POS_FILTERS_MASK */

/* Averaging filter function prototype */
#if ( (`$INSTANCE_NAME`_RAW_FILTER_MASK & `$INSTANCE_NAME`_AVERAGING_FILTER) || \
      (`$INSTANCE_NAME`_POS_FILTERS_MASK & `$INSTANCE_NAME`_AVERAGING_FILTER) )
    uint16 `$INSTANCE_NAME`_AveragingFilter(uint16 x1, uint16 x2, uint16 x3) `=ReentrantKeil($INSTANCE_NAME . "_AveragingFilter")`;
#endif /* End `$INSTANCE_NAME`_RAW_FILTER_MASK && `$INSTANCE_NAME`_POS_FILTERS_MASK */

/* IIR2Filter(1/2prev + 1/2cur) filter function prototype */
#if ( (`$INSTANCE_NAME`_RAW_FILTER_MASK & `$INSTANCE_NAME`_IIR2_FILTER) || \
      (`$INSTANCE_NAME`_POS_FILTERS_MASK & `$INSTANCE_NAME`_IIR2_FILTER) )
uint16 `$INSTANCE_NAME`_IIR2Filter(uint16 x1, uint16 x2) `=ReentrantKeil($INSTANCE_NAME . "_IIR2Filter")`;
#endif /* End `$INSTANCE_NAME`_RAW_FILTER_MASK && `$INSTANCE_NAME`_POS_FILTERS_MASK */

/* IIR4Filter(3/4prev + 1/4cur) filter function prototype */
#if ( (`$INSTANCE_NAME`_RAW_FILTER_MASK & `$INSTANCE_NAME`_IIR4_FILTER) || \
      (`$INSTANCE_NAME`_POS_FILTERS_MASK & `$INSTANCE_NAME`_IIR4_FILTER) )
    uint16 `$INSTANCE_NAME`_IIR4Filter(uint16 x1, uint16 x2) `=ReentrantKeil($INSTANCE_NAME . "_IIR4Filter")`;
#endif /* End `$INSTANCE_NAME`_RAW_FILTER_MASK && `$INSTANCE_NAME`_POS_FILTERS_MASK */

/* IIR8Filter(7/8prev + 1/8cur) filter function prototype - RawCounts only */
#if (`$INSTANCE_NAME`_RAW_FILTER_MASK & `$INSTANCE_NAME`_IIR8_FILTER)
    uint16 `$INSTANCE_NAME`_IIR8Filter(uint16 x1, uint16 x2) `=ReentrantKeil($INSTANCE_NAME . "_IIR8Filter")`;
#endif /* End `$INSTANCE_NAME`_RAW_FILTER_MASK */

/* IIR16Filter(15/16prev + 1/16cur) filter function prototype - RawCounts only */
#if (`$INSTANCE_NAME`_RAW_FILTER_MASK & `$INSTANCE_NAME`_IIR16_FILTER)
    uint16 `$INSTANCE_NAME`_IIR16Filter(uint16 x1, uint16 x2) `=ReentrantKeil($INSTANCE_NAME . "_IIR16Filter")`;
#endif /* End `$INSTANCE_NAME`_RAW_FILTER_MASK */

/* JitterFilter filter function prototype */
#if ( (`$INSTANCE_NAME`_RAW_FILTER_MASK & `$INSTANCE_NAME`_JITTER_FILTER) || \
      (`$INSTANCE_NAME`_POS_FILTERS_MASK & `$INSTANCE_NAME`_JITTER_FILTER) )
    uint16 `$INSTANCE_NAME`_JitterFilter(uint16 x1, uint16 x2) `=ReentrantKeil($INSTANCE_NAME . "_JitterFilter")`;
#endif /* End `$INSTANCE_NAME`_RAW_FILTER_MASK && `$INSTANCE_NAME`_POS_FILTERS_MASK */

/* Storage of filters data */
#if ( (`$INSTANCE_NAME`_RAW_FILTER_MASK & `$INSTANCE_NAME`_MEDIAN_FILTER) || \
      (`$INSTANCE_NAME`_RAW_FILTER_MASK & `$INSTANCE_NAME`_AVERAGING_FILTER) )

    uint16 `$INSTANCE_NAME`_rawFilterData1[`$INSTANCE_NAME`_TOTAL_SENSOR_COUNT];
    uint16 `$INSTANCE_NAME`_rawFilterData2[`$INSTANCE_NAME`_TOTAL_SENSOR_COUNT];

#elif ( (`$INSTANCE_NAME`_RAW_FILTER_MASK & `$INSTANCE_NAME`_IIR2_FILTER)   || \
        (`$INSTANCE_NAME`_RAW_FILTER_MASK & `$INSTANCE_NAME`_IIR4_FILTER)   || \
        (`$INSTANCE_NAME`_RAW_FILTER_MASK & `$INSTANCE_NAME`_JITTER_FILTER) || \
        (`$INSTANCE_NAME`_RAW_FILTER_MASK & `$INSTANCE_NAME`_IIR8_FILTER)   || \
        (`$INSTANCE_NAME`_RAW_FILTER_MASK & `$INSTANCE_NAME`_IIR16_FILTER) )
        
    uint16 `$INSTANCE_NAME`_rawFilterData1[`$INSTANCE_NAME`_TOTAL_SENSOR_COUNT];

#else
    /* No Raw filters */
#endif  /* End `$INSTANCE_NAME`_RAW_FILTER_MASK */

extern uint16 `$INSTANCE_NAME`_SensorRaw[`$INSTANCE_NAME`_TOTAL_SENSOR_COUNT];
extern uint8 `$INSTANCE_NAME`_SensorEnableMask[(((`$INSTANCE_NAME`_TOTAL_SENSOR_COUNT - 1u) / 8u) + 1u)];
extern const uint8 CYCODE `$INSTANCE_NAME`_widgetNumber[];

uint16 `$INSTANCE_NAME`_SensorBaseline[`$INSTANCE_NAME`_TOTAL_SENSOR_COUNT] = {0u};
uint8 `$INSTANCE_NAME`_SensorBaselineLow[`$INSTANCE_NAME`_TOTAL_SENSOR_COUNT] = {0u};
`$SignalSizeReplacementString` `$INSTANCE_NAME`_SensorSignal[`$INSTANCE_NAME`_TOTAL_SENSOR_COUNT] = {0u};
uint8 `$INSTANCE_NAME`_SensorOnMask[(((`$INSTANCE_NAME`_TOTAL_SENSOR_COUNT - 1u) / 8u) + 1u)] = {0u};

/* Helps while centroid calulation */
#if (`$INSTANCE_NAME`_TOTAL_CENTROIDS_COUNT)
    static `$SignalSizeReplacementString` `$INSTANCE_NAME`_centroid[3];
#endif  /* End (`$INSTANCE_NAME`_TOTAL_CENTROIDS_COUNT) */


`$writerCSHLCVariables`

#if ( (`$INSTANCE_NAME`_TOTAL_CENTROIDS_COUNT && (`$INSTANCE_NAME`_TUNNING_METHOD != `$INSTANCE_NAME`_NO_TUNNING)) || \
       `$INSTANCE_NAME`_TOTAL_TOUCH_PADS_COUNT )
    uint16 `$INSTANCE_NAME`_position[`$INSTANCE_NAME`_TOTAL_CENTROIDS_COUNT] = {0xFFFFu};
#endif  /* End (`$INSTANCE_NAME`_TOTAL_TOUCH_PADS_COUNT) */


/*******************************************************************************
* Function Name: `$INSTANCE_NAME`_BaseInit
********************************************************************************
*
* Summary:
*  Loads the `$INSTANCE_NAME`_SensorBaseline[slot] array element with an initial
*  value by scanning the selected slot. The raw count value is copied into
*  the baseline array for each slot. The raw data filters are initialized
*  if enabled.
*
* Parameters:
*  sensor:  Sensor number
*
* Return:
*  void
*
* Reentrant:
*  No
*
*******************************************************************************/
void `$INSTANCE_NAME`_BaseInit(uint8 sensor)
{
    uint8 widget;
    
    #if ((`$INSTANCE_NAME`_TOTAL_BUTTONS_COUNT != 0u) || (`$INSTANCE_NAME`_TOTAL_MATRIX_BUTTONS_COUNT != 0u))
        uint8 debounce;
    #endif  /* End ((`$INSTANCE_NAME`_TOTAL_BUTTONS_COUNT != 0u)||(`$INSTANCE_NAME`_TOTAL_MATRIX_BUTTONS_COUNT != 0u))*/
    
    #if (`$INSTANCE_NAME`_TOTAL_MATRIX_BUTTONS_COUNT != 0u)
        uint8 debounceIndex, rawDataIndex;
    #endif  /* End (`$INSTANCE_NAME`_TOTAL_MATRIX_BUTTONS_COUNT != 0u) */
    
    widget = `$INSTANCE_NAME`_widgetNumber[sensor];
    
    #if (`$INSTANCE_NAME`_TOTAL_GENERICS_COUNT > 0)
        if(widget < `$INSTANCE_NAME`_TOTAL_WIDGET_COUNT)
        {
    #endif  /* End `$INSTANCE_NAME`_TOTAL_GENERICS_COUNT */
    
    /* Initialize Baseline */
    `$INSTANCE_NAME`_SensorBaseline[sensor] = `$INSTANCE_NAME`_SensorRaw[sensor];
    `$INSTANCE_NAME`_SensorBaselineLow[sensor] = 0u;
    `$INSTANCE_NAME`_SensorSignal[sensor] = 0u;
        
`$writerCSHLDebounceInit`
    
    #if ( (`$INSTANCE_NAME`_RAW_FILTER_MASK & `$INSTANCE_NAME`_MEDIAN_FILTER) || \
          (`$INSTANCE_NAME`_RAW_FILTER_MASK & `$INSTANCE_NAME`_AVERAGING_FILTER) )

        `$INSTANCE_NAME`_rawFilterData1[sensor] = `$INSTANCE_NAME`_SensorRaw[sensor];
        `$INSTANCE_NAME`_rawFilterData2[sensor] = `$INSTANCE_NAME`_SensorRaw[sensor];
    
    #elif ( (`$INSTANCE_NAME`_RAW_FILTER_MASK & `$INSTANCE_NAME`_IIR2_FILTER)   || \
            (`$INSTANCE_NAME`_RAW_FILTER_MASK & `$INSTANCE_NAME`_IIR4_FILTER)   || \
            (`$INSTANCE_NAME`_RAW_FILTER_MASK & `$INSTANCE_NAME`_JITTER_FILTER) || \
            (`$INSTANCE_NAME`_RAW_FILTER_MASK & `$INSTANCE_NAME`_IIR8_FILTER)   || \
            (`$INSTANCE_NAME`_RAW_FILTER_MASK & `$INSTANCE_NAME`_IIR16_FILTER) )
            
        `$INSTANCE_NAME`_rawFilterData1[sensor] = `$INSTANCE_NAME`_SensorRaw[sensor];
    
    #else
        /* No Raw filters */
    #endif  /* End `$INSTANCE_NAME`_RAW_FILTER_MASK */
    
    #if (`$INSTANCE_NAME`_TOTAL_GENERICS_COUNT > 0)
        }
    #endif  /* End `$INSTANCE_NAME`_TOTAL_GENERICS_COUNT */
}


/*******************************************************************************
* Function Name: `$INSTANCE_NAME`_InitializeSensorBaseline
********************************************************************************
*
* Summary:
*  Loads the `$INSTANCE_NAME`_SensorBaseline[slot] array element with an initial
*  value by scanning the selected slot. The raw count value is copied into
*  the baseline array for each slot. The raw data filters are initialized
*  if enabled.
*
* Parameters:
*  sensor:  Sensor number
*
* Return:
*  void
*
* Reentrant:
*  No
*
*******************************************************************************/
void `$INSTANCE_NAME`_InitializeSensorBaseline(uint8 sensor)
{
    /* Scan sensor */
    `$INSTANCE_NAME`_ScanSensor(sensor);
        while(`$INSTANCE_NAME`_IsBusy()) {;}

    #if (`$INSTANCE_NAME`_DESIGN_TYPE == `$INSTANCE_NAME`_ONE_CHANNEL_DESIGN)
        /* Initialize Baseline, Signal and debounce counters */       
        `$INSTANCE_NAME`_BaseInit(sensor);
        
    #else
    
        if(sensor < `$INSTANCE_NAME`_TOTAL_SENSOR_COUNT__CH0)
        {
             /* Initialize Baseline, Signal and debounce counters */ 
            `$INSTANCE_NAME`_BaseInit(sensor);
        }
        
        if(sensor < `$INSTANCE_NAME`_TOTAL_SENSOR_COUNT__CH1)
        {
            /* Initialize Baseline, Signal and debounce counters */ 
            sensor += `$INSTANCE_NAME`_TOTAL_SENSOR_COUNT__CH0;
            `$INSTANCE_NAME`_BaseInit(sensor);
        }
    
    #endif  /* End (`$INSTANCE_NAME`_DESIGN_TYPE == `$INSTANCE_NAME`_ONE_CHANNEL_DESIGN) */
}


/*******************************************************************************
* Function Name: `$INSTANCE_NAME`_InitializeAllBaselines
********************************************************************************
*
* Summary:
*  Uses the `$INSTANCE_NAME`_InitializeSlotBaselines function to
*  loads the `$INSTANCE_NAME`_SensorBaseline[ ] array with an initial values by
*  scanning all slots. The raw count values are copied into the baseline array
*  for all slots. The raw data filters are initialized if enabled.
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
void `$INSTANCE_NAME`_InitializeAllBaselines(void)
{
    uint8 i;
 
    /* The baseline initialize by sensor of sensor pair */
    for(i = 0u; i < `$INSTANCE_NAME`_TOTAL_SCANSLOT_COUNT; i++)
    {
        `$INSTANCE_NAME`_InitializeSensorBaseline(i);
    }
}


/*******************************************************************************
* Function Name: `$INSTANCE_NAME`_UpdateSensorBaseline
********************************************************************************
*
* Summary:
*  Updates the `$INSTANCE_NAME`_SensorBaseline[ ] array using the LP filter with
*  k = 256. The signal calculates the difference between raw count and baseline.
*  The baseline stops updating if signal is greater that zero.
*  Raw data filters are applied to the values if enabled.
*
* Parameters:
*  sensor:  Sensor number
*
* Return:
*  void
*
* Reentrant:
*  No
*
*******************************************************************************/
 void `$INSTANCE_NAME`_UpdateSensorBaseline(uint8 sensor)
{
    uint32 calc;
    uint16 tempRaw;
    uint16 filteredRawData;    
    uint8 widget, noiseThreshold;

    widget = `$INSTANCE_NAME`_widgetNumber[sensor];
    
    #if (`$INSTANCE_NAME`_TOTAL_GENERICS_COUNT > 0)
        if(widget < `$INSTANCE_NAME`_TOTAL_WIDGET_COUNT)
        {
    #endif  /* End `$INSTANCE_NAME`_TOTAL_GENERICS_COUNT */
    
    noiseThreshold = `$INSTANCE_NAME`_noiseThreshold[widget];
    filteredRawData = `$INSTANCE_NAME`_SensorRaw[sensor];
    
    #if (`$INSTANCE_NAME`_RAW_FILTER_MASK & `$INSTANCE_NAME`_MEDIAN_FILTER)
        tempRaw = filteredRawData;
        filteredRawData = `$INSTANCE_NAME`_MedianFilter(filteredRawData, `$INSTANCE_NAME`_rawFilterData1[sensor], `$INSTANCE_NAME`_rawFilterData2[sensor]);
        `$INSTANCE_NAME`_rawFilterData2[sensor] = `$INSTANCE_NAME`_rawFilterData1[sensor];
        `$INSTANCE_NAME`_rawFilterData1[sensor] = tempRaw;

                
    #elif (`$INSTANCE_NAME`_RAW_FILTER_MASK & `$INSTANCE_NAME`_AVERAGING_FILTER)
        tempRaw = filteredRawData;
        filteredRawData = `$INSTANCE_NAME`_AveragingFilter(filteredRawData, `$INSTANCE_NAME`_rawFilterData1[sensor], `$INSTANCE_NAME`_rawFilterData2[sensor]);
        `$INSTANCE_NAME`_rawFilterData2[sensor] = `$INSTANCE_NAME`_rawFilterData1[sensor];
        `$INSTANCE_NAME`_rawFilterData1[sensor] = tempRaw;
    
    #elif (`$INSTANCE_NAME`_RAW_FILTER_MASK & `$INSTANCE_NAME`_IIR2_FILTER)
        filteredRawData = `$INSTANCE_NAME`_IIR2Filter(filteredRawData, `$INSTANCE_NAME`_rawFilterData1[sensor]);
        `$INSTANCE_NAME`_rawFilterData1[sensor] = filteredRawData;
        
    #elif (`$INSTANCE_NAME`_RAW_FILTER_MASK & `$INSTANCE_NAME`_IIR4_FILTER)
        filteredRawData = `$INSTANCE_NAME`_IIR4Filter(filteredRawData, `$INSTANCE_NAME`_rawFilterData1[sensor]);
        `$INSTANCE_NAME`_rawFilterData1[sensor] = filteredRawData;
            
    #elif (`$INSTANCE_NAME`_RAW_FILTER_MASK & `$INSTANCE_NAME`_JITTER_FILTER)
        tempRaw = filteredRawData;
        filteredRawData = `$INSTANCE_NAME`_JitterFilter(filteredRawData, `$INSTANCE_NAME`_rawFilterData1[sensor]);
        `$INSTANCE_NAME`_rawFilterData1[sensor] = tempRaw;
    
    #elif (`$INSTANCE_NAME`_RAW_FILTER_MASK & `$INSTANCE_NAME`_IIR8_FILTER)
        filteredRawData = `$INSTANCE_NAME`_IIR8Filter(filteredRawData, `$INSTANCE_NAME`_rawFilterData1[sensor]);
        `$INSTANCE_NAME`_rawFilterData1[sensor] = filteredRawData;
        
    #elif (`$INSTANCE_NAME`_RAW_FILTER_MASK & `$INSTANCE_NAME`_IIR16_FILTER)
        filteredRawData = `$INSTANCE_NAME`_IIR16Filter(filteredRawData, `$INSTANCE_NAME`_rawFilterData1[sensor]);
        `$INSTANCE_NAME`_rawFilterData1[sensor] = filteredRawData;
        
    #else
        /* No Raw filters */
    #endif  /* End `$INSTANCE_NAME`_RAW_FILTER_MASK */
	
	#if (`$INSTANCE_NAME`_TUNNING_METHOD == `$INSTANCE_NAME`_AUTO_TUNNING)
		`$INSTANCE_NAME`_CalculateThresholds(sensor);
	#endif /* End (`$INSTANCE_NAME`_TUNNING_METHOD == `$INSTANCE_NAME`_AUTO_TUNNING) */

    /* Baseline calculation */
    #if (`$INSTANCE_NAME`_AUTO_RESET == `$INSTANCE_NAME`_AUTO_RESET_DISABLE)
        
        if(filteredRawData >= `$INSTANCE_NAME`_SensorBaseline[sensor])
        {
            tempRaw = filteredRawData - `$INSTANCE_NAME`_SensorBaseline[sensor];
            widget = 1u;
        }
        else
        {
            tempRaw = `$INSTANCE_NAME`_SensorBaseline[sensor] - filteredRawData;
            widget = 0u;  
        }
        
        /* Update Baseline if lower that noiseThreshold */
        if (tempRaw <= noiseThreshold)
        {
            /* Make full Baseline 23 bits */
            calc = (uint32) `$INSTANCE_NAME`_SensorBaseline[sensor] << 8u;
            calc |= (uint32) `$INSTANCE_NAME`_SensorBaselineLow[sensor];
            
            /* Add Raw Data to Baseline */
            calc += filteredRawData;
            
            /* Sub the high Baseline */
            calc -= `$INSTANCE_NAME`_SensorBaseline[sensor];
            
            /* Put Baseline and BaselineLow */
            `$INSTANCE_NAME`_SensorBaseline[sensor] = ((uint16) (calc >> 8u));
            `$INSTANCE_NAME`_SensorBaselineLow[sensor] = ((uint8) calc);
            
            /* Signal is zero */
            `$INSTANCE_NAME`_SensorSignal[sensor] = 0u;
        }
        else if (widget != 0u)
        {
            #if (`$INSTANCE_NAME`_SIGNAL_SIZE == `$INSTANCE_NAME`_SIGNAL_SIZE_UINT8)
                /* Over flow defence for uint8 */
                if (tempRaw > 0xFFu)
                {
                    `$INSTANCE_NAME`_SensorSignal[sensor] = 0xFFu;
                }    
                else 
                {
                    `$INSTANCE_NAME`_SensorSignal[sensor] = ((uint8) tempRaw);
                }
            #else
                `$INSTANCE_NAME`_SensorSignal[sensor] = ((uint16) tempRaw);
                
            #endif  /* End `$INSTANCE_NAME`_SIGNAL_SIZE */
        }
        else
        {
            /* Baseline and BaselineLow  ReInit */
            `$INSTANCE_NAME`_SensorBaseline[sensor] = filteredRawData;
            `$INSTANCE_NAME`_SensorBaselineLow[sensor] = 0u;
            
            /* Signal is zero */
            `$INSTANCE_NAME`_SensorSignal[sensor] = 0u;
        }

    #else    
        /* Calculate Signal if greater noiseTheshold */
        if(filteredRawData >= `$INSTANCE_NAME`_SensorBaseline[sensor])
        {
            tempRaw = filteredRawData - `$INSTANCE_NAME`_SensorBaseline[sensor];
            widget = 1u;
        }
        else
        {
            tempRaw = `$INSTANCE_NAME`_SensorBaseline[sensor] - filteredRawData;
            widget = 0u;  
        }
        
        if ( (tempRaw > noiseThreshold) && (widget != 0u) )
        {
            #if (`$INSTANCE_NAME`_SIGNAL_SIZE == `$INSTANCE_NAME`_SIGNAL_SIZE_UINT8)
                /* Over flow defence for uint8 */
                if (tempRaw > 0xFFu)
                {
                    `$INSTANCE_NAME`_SensorSignal[sensor] = 0xFFu;
                }    
                else 
                {
                    `$INSTANCE_NAME`_SensorSignal[sensor] = ((uint8) tempRaw);
                }
            #else
                `$INSTANCE_NAME`_SensorSignal[sensor] = ((uint16) tempRaw);
                
            #endif  /* End `$INSTANCE_NAME`_SIGNAL_SIZE */
        }
        else
        {
            /* Signal is zero */
            `$INSTANCE_NAME`_SensorSignal[sensor] = 0u;
        }
        
        /* Update Baseline */
        /* Make full Baseline 23 bits */
        calc = (uint32) `$INSTANCE_NAME`_SensorBaseline[sensor] << 8u;
        calc |= (uint32) `$INSTANCE_NAME`_SensorBaselineLow[sensor];
        
        /* Add Raw Data to Baseline */
        calc += filteredRawData;
        
        /* Sub the high Baseline */
        calc -= `$INSTANCE_NAME`_SensorBaseline[sensor];
        
        /* Put Baseline and BaselineLow */
        `$INSTANCE_NAME`_SensorBaseline[sensor] = ((uint16) (calc >> 8u));
        `$INSTANCE_NAME`_SensorBaselineLow[sensor] = ((uint8) calc);
     
    #endif  /* End (`$INSTANCE_NAME`_AUTO_RESET == `$INSTANCE_NAME`_AUTO_RESET_DISABLE) */
    
    #if (`$INSTANCE_NAME`_TOTAL_GENERICS_COUNT > 0)
        }
    #endif  /* End `$INSTANCE_NAME`_TOTAL_GENERICS_COUNT */
}


/*******************************************************************************
* Function Name: `$INSTANCE_NAME`_UpdateAllBaselines
********************************************************************************
*
* Summary:
*  Uses the `$INSTANCE_NAME`_UpdateSlotBaselines function to update
*  baselines for all slots. Raw data filters are applied to the values if
*  enabled.
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
 void `$INSTANCE_NAME`_UpdateEnabledBaselines(void)
{
    uint8 i, pos, enMask;
    
    for(i = 0; i < `$INSTANCE_NAME`_TOTAL_SENSOR_COUNT; i++)
    {
        pos = (i >> 3u);
        enMask = 0x01u << (i & 0x07u);
        if((`$INSTANCE_NAME`_SensorEnableMask[pos] & enMask) != 0u)
        {
            `$INSTANCE_NAME`_UpdateSensorBaseline(i);
        }
    }
}


/*******************************************************************************
* Function Name: `$INSTANCE_NAME`_CheckIsSensorActive
********************************************************************************
*
* Summary:
*  Compares the selected slot of the CapSenseSignal[ ] array to its finger
*  threshold. Hysteresis and Debounce are taken into account. The Hysteresis value
*  is added or subtracted from the finger threshold based on whether the slot is
*  currently active. If the slot is active, the threshold is lowered by the
*  hysteresis amount. If it is inactive, the threshold is raised by the
*  hysteresis amount. The Debounce counter added to the slot active transition.
*  This function also updates the slot's bit in the
*  `$INSTANCE_NAME`_SlotOnMask[ ] array
*
* Parameters:
*  slot:  Scan slot number
*
* Return:
*  Returns scan slot state 1 if active, 0 if inactive
*
* Reentrant:
*  No
*
*******************************************************************************/
uint8 `$INSTANCE_NAME`_CheckIsSensorActive(uint8 sensor)
{
    uint8 widget, debounceIndex;
    uint8 fingerThreshold, hysteresis, debounce;
    uint8 pos, onMask;
    
    #if (`$INSTANCE_NAME`_TOTAL_MATRIX_BUTTONS_COUNT != 0u)
        uint8 rawDataIndex;
    #endif  /* End (`$INSTANCE_NAME`_TOTAL_MATRIX_BUTTONS_COUNT != 0u) */
    
    /* Get On/Off mask */
    pos = (sensor >> 3u);
    onMask = 0x01u << (sensor & 0x07u);
    
    /* Prepare to find bounce counter index */
    widget = `$INSTANCE_NAME`_widgetNumber[sensor];
    fingerThreshold = `$INSTANCE_NAME`_fingerThreshold[widget];
    hysteresis = `$INSTANCE_NAME`_hysteresis[widget];
    debounce = `$INSTANCE_NAME`_debounce[widget];
    
`$writerCSHLIsSensor`
    
    /* Was on */
    if (`$INSTANCE_NAME`_SensorOnMask[pos] & onMask)
    {
        /* Hysteresis minus */
        if (`$INSTANCE_NAME`_SensorSignal[sensor] < (fingerThreshold - hysteresis))
        {
            `$INSTANCE_NAME`_SensorOnMask[pos] &= ~onMask;
            `$INSTANCE_NAME`_debounceCounter[debounceIndex] = debounce; 
        }
    }
    else    /* Was off */
    {
        /* Hysteresis plus */
        if (`$INSTANCE_NAME`_SensorSignal[sensor] > (fingerThreshold + hysteresis))
        {
            /* Sensor active */
            if (`$INSTANCE_NAME`_debounceCounter[debounceIndex]-- == 0)
            {
                `$INSTANCE_NAME`_SensorOnMask[pos] |= onMask; 
            }
        }
        else
        {
            /* Sensor inactive - reset Debounce count  */
            `$INSTANCE_NAME`_debounceCounter[debounceIndex] = debounce;
        }
    }
    
    return (`$INSTANCE_NAME`_SensorOnMask[pos] & onMask) ? 1u : 0u;
}


/*******************************************************************************
* Function Name: `$INSTANCE_NAME`_CheckIsWidgetActive
********************************************************************************
*
* Summary:
*
* Parameters:
*  widget:  Scan slot number
*
* Return:
*  Returns scan slot state 1 if active, 0 if inactive
*
* Reentrant:
*  No
*
*******************************************************************************/
uint8 `$INSTANCE_NAME`_CheckIsWidgetActive(uint8 widget)
{
    uint8 numberOfSensors, rawIndex, state;

    numberOfSensors = `$INSTANCE_NAME`_numberOfSensors[widget];
    rawIndex = `$INSTANCE_NAME`_rawDataIndex[widget];
    numberOfSensors += rawIndex;               
    state = 0u;

    /* Check all sensors of the widget */
    do
    {
        if(`$INSTANCE_NAME`_CheckIsSensorActive(rawIndex) != 0u)
        {
            state = `$INSTANCE_NAME`_SENSOR_1_IS_ACTIVE;
        }
        rawIndex++;
    }
    while(rawIndex < numberOfSensors);

`$writerCSHLIsWidget`
    
    return state;
}


/*******************************************************************************
* Function Name: `$INSTANCE_NAME`_CheckIsAnyWidgetActive
********************************************************************************
*
* Summary:
*
* Parameters:
*  widget:  Scan slot number
*
* Return:
*  Returns scan slot state 1 if active, 0 if inactive
*
* Reentrant:
*  No
*
*******************************************************************************/
uint8 `$INSTANCE_NAME`_CheckIsAnyWidgetActive(void)
{
`$writerCSHLIsAnyWidget`   
    return state;
}


/*******************************************************************************
* Function Name: `$INSTANCE_NAME`_EnableWidget
********************************************************************************
*
* Summary:
*
* Parameters:
*  widget:  Scan slot number
*
* Return:
*  Returns scan slot state 1 if active, 0 if inactive
*
* Reentrant:
*  No
*
*******************************************************************************/
void `$INSTANCE_NAME`_EnableWidget(uint8 widget)
{
    uint8 numberOfSensors, rawIndex;
    uint8 pos, enMask;
    
    numberOfSensors = `$INSTANCE_NAME`_numberOfSensors[widget];
    rawIndex =  `$INSTANCE_NAME`_rawDataIndex[widget];
    numberOfSensors += rawIndex;
    
    /* Enable all sensors of the widget */
    do
    {
        pos = (rawIndex >> 3u);
        enMask = 0x01u << (rawIndex & 0x07u);
        
        `$INSTANCE_NAME`_SensorEnableMask[pos] |= enMask;
        rawIndex++;
    }
    while(rawIndex < numberOfSensors);
    
`$writerCSHLEnWidget`
}


/*******************************************************************************
* Function Name: `$INSTANCE_NAME`_DisableWidget
********************************************************************************
*
* Summary:
*
* Parameters:
*  widget:  Scan slot number
*
* Return:
*  Returns scan slot state 1 if active, 0 if inactive
*
* Reentrant:
*  No
*
*******************************************************************************/
void `$INSTANCE_NAME`_DisableWidget(uint8 widget)
{
    uint8 numberOfSensors, rawIndex;
    uint8 pos, enMask;
    
    numberOfSensors = `$INSTANCE_NAME`_numberOfSensors[widget];
    rawIndex =  `$INSTANCE_NAME`_rawDataIndex[widget];
    numberOfSensors += rawIndex;
 
    /* Disable all sensors of the widget */
    do
    {
        pos = (rawIndex >> 3u);
        enMask = 0x01u << (rawIndex & 0x07u);
        
        `$INSTANCE_NAME`_SensorEnableMask[pos] &= ~enMask;
        rawIndex++;
    }
    while(rawIndex < numberOfSensors);
    
`$writerCSHLDisWidget`
}

`$writerCSHLCCode`

#if ( (`$INSTANCE_NAME`_RAW_FILTER_MASK & `$INSTANCE_NAME`_MEDIAN_FILTER) || \
      (`$INSTANCE_NAME`_POS_FILTERS_MASK & `$INSTANCE_NAME`_MEDIAN_FILTER) )
    /*******************************************************************************
    * Function Name: `$INSTANCE_NAME`_MedianFilter
    ********************************************************************************
    *
    * Summary:
    *  Median filter funciton.
    *
    * Parameters:
    *  x1:  Current value
    *  x2:  Previous value
    *  x3:  Before previous value
    *
    * Return:
    *  Returns filtered value.
    *
    *******************************************************************************/
    uint16 `$INSTANCE_NAME`_MedianFilter(uint16 x1, uint16 x2, uint16 x3) `=ReentrantKeil($INSTANCE_NAME . "_MedianFilter")`
    {
        uint16 tmp;
    
        if (x1 > x2)
        {
            tmp = x2;
            x2 = x1;
            x1 = tmp;
        }
    
        if (x2 > x3)
        {
            x2 = x3;
        }
    
        return ((x1 > x2) ? x1 : x2);
    }
#endif /* End `$INSTANCE_NAME`_RAW_FILTER_MASK && `$INSTANCE_NAME`_POS_FILTERS_MASK */


#if ( (`$INSTANCE_NAME`_RAW_FILTER_MASK & `$INSTANCE_NAME`_AVERAGING_FILTER) || \
      (`$INSTANCE_NAME`_POS_FILTERS_MASK & `$INSTANCE_NAME`_AVERAGING_FILTER) )
    /*******************************************************************************
    * Function Name: `$INSTANCE_NAME`_AveragingFilter
    ********************************************************************************
    *
    * Summary:
    *  Averaging filter function.
    *
    * Parameters:
    *  x1:  Current value
    *  x2:  Previous value
    *  x3:  Before previous value
    *
    * Return:
    *  Returns filtered value.
    *
    *******************************************************************************/
    uint16 `$INSTANCE_NAME`_AveragingFilter(uint16 x1, uint16 x2, uint16 x3) `=ReentrantKeil($INSTANCE_NAME . "_AveragingFilter")`
    {
        uint32 tmp = ((uint32)x1 + (uint32)x2 + (uint32)x3) / 3u;
    
        return ((uint16) tmp);
    }
#endif /* End `$INSTANCE_NAME`_RAW_FILTER_MASK && `$INSTANCE_NAME`_POS_FILTERS_MASK */


#if ( (`$INSTANCE_NAME`_RAW_FILTER_MASK & `$INSTANCE_NAME`_IIR2_FILTER) || \
      (`$INSTANCE_NAME`_POS_FILTERS_MASK & `$INSTANCE_NAME`_IIR2_FILTER) )
    /*******************************************************************************
    * Function Name: `$INSTANCE_NAME`_IIR2Filter
    ********************************************************************************
    *
    * Summary:
    *  IIR filter function.
    *
    * Parameters:
    *  x1:  Current value
    *  x2:  Previous value
    *
    * Return:
    *  Returns filtered value.
    *
    *******************************************************************************/
    uint16 `$INSTANCE_NAME`_IIR2Filter(uint16 x1, uint16 x2) `=ReentrantKeil($INSTANCE_NAME . "_IIR2Filter")`
    {
        uint32 tmp;
    
        /* IIR = 1/2 Current Value+ 1/2 Previous Value */
        tmp = (uint32)x1 + (uint32)x2;
        tmp >>= 1u;
    
    
        return ((uint16) tmp);
    }
#endif /* End `$INSTANCE_NAME`_RAW_FILTER_MASK && `$INSTANCE_NAME`_POS_FILTERS_MASK */


#if ( (`$INSTANCE_NAME`_RAW_FILTER_MASK & `$INSTANCE_NAME`_IIR4_FILTER) || \
      (`$INSTANCE_NAME`_POS_FILTERS_MASK & `$INSTANCE_NAME`_IIR4_FILTER) )
    /*******************************************************************************
    * Function Name: `$INSTANCE_NAME`_IIRFilter
    ********************************************************************************
    *
    * Summary:
    *  IIR filter function.
    *
    * Parameters:
    *  x1:  Current value
    *  x2:  Previous value
    *
    * Return:
    *  Returns filtered value.
    *
    *******************************************************************************/
    uint16 `$INSTANCE_NAME`_IIR4Filter(uint16 x1, uint16 x2) `=ReentrantKeil($INSTANCE_NAME . "_IIR4Filter")`
    {
        uint32 tmp;
    
        /* IIR = 1/4 Current Value + 3/4 Previous Value */
        tmp = (uint32)x1 + (uint32)x2;
        tmp += ((uint32)x2 << 1u);
        tmp >>= 2u;
    
        return ((uint16) tmp);
    }
#endif /* End `$INSTANCE_NAME`_RAW_FILTER_MASK && `$INSTANCE_NAME`_POS_FILTERS_MASK */


#if ( (`$INSTANCE_NAME`_RAW_FILTER_MASK & `$INSTANCE_NAME`_JITTER_FILTER) || \
      (`$INSTANCE_NAME`_POS_FILTERS_MASK & `$INSTANCE_NAME`_JITTER_FILTER) )
    /*******************************************************************************
    * Function Name: uint16 `$INSTANCE_NAME`_JitterFilter
    ********************************************************************************
    *
    * Summary:
    *  Jitter filter funciton.
    *
    * Parameters:
    *  x1:  Current value
    *  x2:  Previous value
    *
    * Return:
    *  Returns filtered value.
    *
    *******************************************************************************/
    uint16 `$INSTANCE_NAME`_JitterFilter(uint16 x1, uint16 x2) `=ReentrantKeil($INSTANCE_NAME . "_JitterFilter")`
    {
        if (x1 > x2)
        {
            x1--;
        }
        else
        {
            if (x1 < x2)
            {
                x1++;
            }
        }
    
        return x1;
    }
#endif /* End `$INSTANCE_NAME`_RAW_FILTER_MASK && `$INSTANCE_NAME`_POS_FILTERS_MASK */


#if (`$INSTANCE_NAME`_RAW_FILTER_MASK & `$INSTANCE_NAME`_IIR8_FILTER)
    /*******************************************************************************
    * Function Name: `$INSTANCE_NAME`_IIR8Filter
    ********************************************************************************
    *
    * Summary:
    *  IIR filter function.
    *
    * Parameters:
    *  x1:  Current value
    *  x2:  Previous value
    *
    * Return:
    *  Returns filtered value.
    *
    *******************************************************************************/
    uint16 `$INSTANCE_NAME`_IIR8Filter(uint16 x1, uint16 x2) `=ReentrantKeil($INSTANCE_NAME . "_IIR8Filter")`
    {
        uint32 tmp;
    
        /* IIR = 1/8 Current Value + 7/8 Previous Value */
        tmp = (uint32)x1;
        tmp += (((uint32)x2 << 3u) - ((uint32)x2));
        tmp >>= 3u;
    
        return ((uint16) tmp);
    }
#endif /* End (`$INSTANCE_NAME`_RAW_FILTER_MASK & `$INSTANCE_NAME`_IIR8_FILTER) */


#if (`$INSTANCE_NAME`_RAW_FILTER_MASK & `$INSTANCE_NAME`_IIR16_FILTER)
    /*******************************************************************************
    * Function Name: `$INSTANCE_NAME`_IIR16Filter
    ********************************************************************************
    *
    * Summary:
    *  IIR filter function.
    *
    * Parameters:
    *  x1:  Current value
    *  x2:  Previous value
    *
    * Return:
    *  Returns filtered value.
    *
    *******************************************************************************/
    uint16 `$INSTANCE_NAME`_IIR16Filter(uint16 x1, uint16 x2) `=ReentrantKeil($INSTANCE_NAME . "_IIR16Filter")`
    {
        uint32 tmp;
    
        /* IIR = 1/16 Current Value + 15/16 Previous Value */
        tmp = (uint32)x1;
        tmp += (((uint32)x2 << 4u) - ((uint32)x2));
        tmp >>= 4u;
    
        return ((uint16) tmp);
    }
#endif /* End (`$INSTANCE_NAME`_RAW_FILTER_MASK & `$INSTANCE_NAME`_IIR16_FILTER) */


/* [] END OF FILE */
