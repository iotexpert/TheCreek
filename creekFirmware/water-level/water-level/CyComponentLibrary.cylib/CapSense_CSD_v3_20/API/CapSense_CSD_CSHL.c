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
* Copyright 2008-2012, Cypress Semiconductor Corporation.  All rights reserved.
* You may use this file only in accordance with the license, terms, conditions,
* disclaimers, and limitations in the end user license agreement accompanying
* the software package with which this file was provided.
*******************************************************************************/

#include "`$INSTANCE_NAME`_CSHL.h"

/* SmartSense functions */
#if (`$INSTANCE_NAME`_TUNING_METHOD == `$INSTANCE_NAME`_AUTO_TUNING)
    extern void `$INSTANCE_NAME`_CalculateThresholds(uint8 SensorNumber)
           `=ReentrantKeil($INSTANCE_NAME . "_CalculateThresholds")`;
#endif /* (`$INSTANCE_NAME`_TUNING_METHOD == `$INSTANCE_NAME`_AUTO_TUNING) */

/* Median filter function prototype */
#if ( (`$INSTANCE_NAME`_RAW_FILTER_MASK & `$INSTANCE_NAME`_MEDIAN_FILTER) || \
      (`$INSTANCE_NAME`_POS_FILTERS_MASK & `$INSTANCE_NAME`_MEDIAN_FILTER) )
    uint16 `$INSTANCE_NAME`_MedianFilter(uint16 x1, uint16 x2, uint16 x3)
    `=ReentrantKeil($INSTANCE_NAME . "_MedianFilter")`;
#endif /* `$INSTANCE_NAME`_RAW_FILTER_MASK && `$INSTANCE_NAME`_POS_FILTERS_MASK */

/* Averaging filter function prototype */
#if ( (`$INSTANCE_NAME`_RAW_FILTER_MASK & `$INSTANCE_NAME`_AVERAGING_FILTER) || \
      (`$INSTANCE_NAME`_POS_FILTERS_MASK & `$INSTANCE_NAME`_AVERAGING_FILTER) )
    uint16 `$INSTANCE_NAME`_AveragingFilter(uint16 x1, uint16 x2, uint16 x3)
    `=ReentrantKeil($INSTANCE_NAME . "_AveragingFilter")`;
#endif /* `$INSTANCE_NAME`_RAW_FILTER_MASK && `$INSTANCE_NAME`_POS_FILTERS_MASK */

/* IIR2Filter(1/2prev + 1/2cur) filter function prototype */
#if ( (`$INSTANCE_NAME`_RAW_FILTER_MASK & `$INSTANCE_NAME`_IIR2_FILTER) || \
      (`$INSTANCE_NAME`_POS_FILTERS_MASK & `$INSTANCE_NAME`_IIR2_FILTER) )
    uint16 `$INSTANCE_NAME`_IIR2Filter(uint16 x1, uint16 x2) `=ReentrantKeil($INSTANCE_NAME . "_IIR2Filter")`;
#endif /* `$INSTANCE_NAME`_RAW_FILTER_MASK && `$INSTANCE_NAME`_POS_FILTERS_MASK */

/* IIR4Filter(3/4prev + 1/4cur) filter function prototype */
#if ( (`$INSTANCE_NAME`_RAW_FILTER_MASK & `$INSTANCE_NAME`_IIR4_FILTER) || \
      (`$INSTANCE_NAME`_POS_FILTERS_MASK & `$INSTANCE_NAME`_IIR4_FILTER) )
    uint16 `$INSTANCE_NAME`_IIR4Filter(uint16 x1, uint16 x2) `=ReentrantKeil($INSTANCE_NAME . "_IIR4Filter")`;
#endif /* `$INSTANCE_NAME`_RAW_FILTER_MASK && `$INSTANCE_NAME`_POS_FILTERS_MASK */

/* IIR8Filter(7/8prev + 1/8cur) filter function prototype - RawCounts only */
#if (`$INSTANCE_NAME`_RAW_FILTER_MASK & `$INSTANCE_NAME`_IIR8_FILTER)
    uint16 `$INSTANCE_NAME`_IIR8Filter(uint16 x1, uint16 x2) `=ReentrantKeil($INSTANCE_NAME . "_IIR8Filter")`;
#endif /* `$INSTANCE_NAME`_RAW_FILTER_MASK */

/* IIR16Filter(15/16prev + 1/16cur) filter function prototype - RawCounts only */
#if (`$INSTANCE_NAME`_RAW_FILTER_MASK & `$INSTANCE_NAME`_IIR16_FILTER)
    uint16 `$INSTANCE_NAME`_IIR16Filter(uint16 x1, uint16 x2) `=ReentrantKeil($INSTANCE_NAME . "_IIR16Filter")`;
#endif /* `$INSTANCE_NAME`_RAW_FILTER_MASK */

/* JitterFilter filter function prototype */
#if ( (`$INSTANCE_NAME`_RAW_FILTER_MASK & `$INSTANCE_NAME`_JITTER_FILTER) || \
      (`$INSTANCE_NAME`_POS_FILTERS_MASK & `$INSTANCE_NAME`_JITTER_FILTER) )
    uint16 `$INSTANCE_NAME`_JitterFilter(uint16 x1, uint16 x2) `=ReentrantKeil($INSTANCE_NAME . "_JitterFilter")`;
#endif /* `$INSTANCE_NAME`_RAW_FILTER_MASK && `$INSTANCE_NAME`_POS_FILTERS_MASK */

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
#endif  /* ( (`$INSTANCE_NAME`_RAW_FILTER_MASK & `$INSTANCE_NAME`_MEDIAN_FILTER) || \
        *    (`$INSTANCE_NAME`_RAW_FILTER_MASK & `$INSTANCE_NAME`_AVERAGING_FILTER) )
        */

extern uint16 `$INSTANCE_NAME`_SensorRaw[`$INSTANCE_NAME`_TOTAL_SENSOR_COUNT];
extern uint8 `$INSTANCE_NAME`_SensorEnableMask[(((`$INSTANCE_NAME`_TOTAL_SENSOR_COUNT - 1u) / 8u) + 1u)];
extern const uint8 CYCODE `$INSTANCE_NAME`_widgetNumber[];

uint16 `$INSTANCE_NAME`_SensorBaseline[`$INSTANCE_NAME`_TOTAL_SENSOR_COUNT] = {0u};
uint8 `$INSTANCE_NAME`_SensorBaselineLow[`$INSTANCE_NAME`_TOTAL_SENSOR_COUNT] = {0u};
`$SignalSizeReplacementString` `$INSTANCE_NAME`_SensorSignal[`$INSTANCE_NAME`_TOTAL_SENSOR_COUNT] = {0u};
uint8 `$INSTANCE_NAME`_SensorOnMask[(((`$INSTANCE_NAME`_TOTAL_SENSOR_COUNT - 1u) / 8u) + 1u)] = {0u};

uint8 `$INSTANCE_NAME`_LowBaselineResetCnt[`$INSTANCE_NAME`_TOTAL_SENSOR_COUNT];

/* Helps while centroid calulation */
#if (`$INSTANCE_NAME`_TOTAL_CENTROIDS_COUNT)
    static `$SignalSizeReplacementString` `$INSTANCE_NAME`_centroid[3];
#endif  /* (`$INSTANCE_NAME`_TOTAL_CENTROIDS_COUNT) */

`$writerCSHLCVariables`


/*******************************************************************************
* Function Name: `$INSTANCE_NAME`_BaseInit
********************************************************************************
*
* Summary:
*  Loads the `$INSTANCE_NAME`_SensorBaseline[sensor] array element with an 
*  initial value which is equal to raw count value. 
*  Resets to zero `$INSTANCE_NAME`_SensorBaselineLow[senesor] and 
*  `$INSTANCE_NAME`_SensorSignal[sensor] array element.
*  Loads `$INSTANCE_NAME`_debounceCounter[sensor] array element with initial 
*  value equal `$INSTANCE_NAME`_debounce[].
*  Loads the `$INSTANCE_NAME`_rawFilterData2[sensor] and 
*  `$INSTANCE_NAME`_rawFilterData2[sensor] array element with an 
*  initial value which is equal raw count value if raw data filter is enabled.
*
* Parameters:
*  sensor:  Sensor number.
*
* Return:
*  None
*
* Global Variables:
*  `$INSTANCE_NAME`_SensorBaseline[]    - used to store baseline value.
*  `$INSTANCE_NAME`_SensorBaselineLow[] - used to store fraction byte of 
*  baseline value.
*  `$INSTANCE_NAME`_SensorSignal[]      - used to store diffence between 
*  current value of raw data and previous value of baseline.
*  `$INSTANCE_NAME`_debounceCounter[]   - used to store current debounce 
*  counter of sensor. Widget which has this parameter are buttons, matrix 
*  buttons, proximity, guard. All other widgets haven't debounce parameter
*  and use the last element of this array with value 0 (it means no debounce).
*  `$INSTANCE_NAME`_rawFilterData1[]    - used to store previous sample of 
*  any enabled raw data filter.
*  `$INSTANCE_NAME`_rawFilterData2[]    - used to store before previous sample
*  of enabled raw data filter. Only required for median or average filters.
*
* Reentrant:
*  No
*
*******************************************************************************/
void `$INSTANCE_NAME`_BaseInit(uint8 sensor) `=ReentrantKeil($INSTANCE_NAME . "_BaseInit")`
{
    #if ((`$INSTANCE_NAME`_TOTAL_BUTTONS_COUNT) || (`$INSTANCE_NAME`_TOTAL_MATRIX_BUTTONS_COUNT) || \
         (`$INSTANCE_NAME`_TOTAL_GENERICS_COUNT))
        uint8 widget = `$INSTANCE_NAME`_widgetNumber[sensor];
    #endif /* ((`$INSTANCE_NAME`_TOTAL_MATRIX_BUTTONS_COUNT) || (`$INSTANCE_NAME`_TOTAL_MATRIX_BUTTONS_COUNT)) */
    
    #if (`$INSTANCE_NAME`_TOTAL_MATRIX_BUTTONS_COUNT)
        uint8 debounceIndex;
    #endif  /* (`$INSTANCE_NAME`_TOTAL_MATRIX_BUTTONS_COUNT) */
    
    #if (`$INSTANCE_NAME`_TOTAL_GENERICS_COUNT)
        /* Exclude generic widget */
        if(widget < `$INSTANCE_NAME`_END_OF_WIDGETS_INDEX)
        {
    #endif  /* `$INSTANCE_NAME`_TOTAL_GENERICS_COUNT */
    
    /* Initialize Baseline */
    `$INSTANCE_NAME`_SensorBaseline[sensor] = `$INSTANCE_NAME`_SensorRaw[sensor];
    `$INSTANCE_NAME`_SensorBaselineLow[sensor] = 0u;
    `$INSTANCE_NAME`_SensorSignal[sensor] = 0u;
        
`$writerCSHLDebounceInit`
    
    #if ((`$INSTANCE_NAME`_RAW_FILTER_MASK & `$INSTANCE_NAME`_MEDIAN_FILTER) ||\
         (`$INSTANCE_NAME`_RAW_FILTER_MASK & `$INSTANCE_NAME`_AVERAGING_FILTER))

        `$INSTANCE_NAME`_rawFilterData1[sensor] = `$INSTANCE_NAME`_SensorRaw[sensor];
        `$INSTANCE_NAME`_rawFilterData2[sensor] = `$INSTANCE_NAME`_SensorRaw[sensor];
    
    #elif ((`$INSTANCE_NAME`_RAW_FILTER_MASK & `$INSTANCE_NAME`_IIR2_FILTER) ||\
           (`$INSTANCE_NAME`_RAW_FILTER_MASK & `$INSTANCE_NAME`_IIR4_FILTER) ||\
           (`$INSTANCE_NAME`_RAW_FILTER_MASK & `$INSTANCE_NAME`_JITTER_FILTER) ||\
           (`$INSTANCE_NAME`_RAW_FILTER_MASK & `$INSTANCE_NAME`_IIR8_FILTER) ||\
           (`$INSTANCE_NAME`_RAW_FILTER_MASK & `$INSTANCE_NAME`_IIR16_FILTER))
            
        `$INSTANCE_NAME`_rawFilterData1[sensor] = `$INSTANCE_NAME`_SensorRaw[sensor];
    
    #else
        /* No Raw filters */
    #endif  /* ((`$INSTANCE_NAME`_RAW_FILTER_MASK & `$INSTANCE_NAME`_MEDIAN_FILTER) || \
            *   (`$INSTANCE_NAME`_RAW_FILTER_MASK & `$INSTANCE_NAME`_AVERAGING_FILTER))
            */
    
    #if (`$INSTANCE_NAME`_TOTAL_GENERICS_COUNT)
        /* Exclude generic widget */
        }
    #endif  /* `$INSTANCE_NAME`_TOTAL_GENERICS_COUNT */
}


/*******************************************************************************
* Function Name: `$INSTANCE_NAME`_InitializeSensorBaseline
********************************************************************************
*
* Summary:
*  Loads the `$INSTANCE_NAME`_SensorBaseline[sensor] array element with an 
*  initial value by scanning the selected sensor (one channel design) or pair 
*  of sensors (two channels designs). The raw count value is copied into the 
*  baseline array for each sensor. The raw data filters are initialized if 
*  enabled.
*
* Parameters:
*  sensor:  Sensor number.
*
* Return:
*  None
*
* Reentrant:
*  No
*
*******************************************************************************/
void `$INSTANCE_NAME`_InitializeSensorBaseline(uint8 sensor)
                          `=ReentrantKeil($INSTANCE_NAME . "_InitializeSensorBaseline")`
{
    /* Scan sensor */
    `$INSTANCE_NAME`_ScanSensor(sensor);
    while(`$INSTANCE_NAME`_IsBusy() != 0u) {;}
    
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
            `$INSTANCE_NAME`_BaseInit(sensor + `$INSTANCE_NAME`_TOTAL_SENSOR_COUNT__CH0);
        }
    
    #endif  /* (`$INSTANCE_NAME`_DESIGN_TYPE == `$INSTANCE_NAME`_ONE_CHANNEL_DESIGN) */
}


/*******************************************************************************
* Function Name: `$INSTANCE_NAME`_InitializeAllBaselines
********************************************************************************
*
* Summary:
*  Uses the `$INSTANCE_NAME`_InitializeSensorBaseline function to loads the 
*  `$INSTANCE_NAME`_SensorBaseline[] array with an initial values by scanning 
*  all sensors. The raw count values are copied into the baseline array for 
*  all sensors. The raw data filters are initialized if enabled.
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
void `$INSTANCE_NAME`_InitializeAllBaselines(void)
                          `=ReentrantKeil($INSTANCE_NAME . "_InitializeAllBaselines")`
{
    uint8 i;
    
    /* The baseline initialize by sensor of sensor pair */
    for(i = 0u; i < `$INSTANCE_NAME`_TOTAL_SCANSLOT_COUNT; i++)
    {
        `$INSTANCE_NAME`_InitializeSensorBaseline(i);
    }
}


/*******************************************************************************
* Function Name: `$INSTANCE_NAME`_InitializeEnabledBaselines
********************************************************************************
*
* Summary:
*  Scans all enabled widgets and the raw count values are copied into the 
*  baseline array for all sensors enabled in scanning process. Baselines 
*  initialize with zero values for sensors disabled from scanning process. 
*  The raw data filters are initialized if enabled.
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
void `$INSTANCE_NAME`_InitializeEnabledBaselines(void)
                             `=ReentrantKeil($INSTANCE_NAME . "_InitializeEnabledBaselines")`
{
    uint8 i;
    uint8 pos;
    uint8 enMask;
    
    `$INSTANCE_NAME`_ScanEnabledWidgets();
    while(`$INSTANCE_NAME`_IsBusy() != 0u){;}
    
    for(i = 0u; i < `$INSTANCE_NAME`_TOTAL_SENSOR_COUNT; i++)
    {
        pos = (i >> 3u);
        enMask = 0x01u << (i & 0x07u);
        
        /* Clear raw data if sensor is disabled from scanning process */
        if((`$INSTANCE_NAME`_SensorEnableMask[pos] & enMask) == 0u)
        {
            `$INSTANCE_NAME`_SensorRaw[i] = 0u;
        }
        
        /* Initialize baselines */
        `$INSTANCE_NAME`_BaseInit(i);
    }
}  


/*******************************************************************************
* Function Name: `$INSTANCE_NAME`_UpdateSensorBaseline
********************************************************************************
*
* Summary:
*  Updates the `$INSTANCE_NAME`_SensorBaseline[sensor] array element using the 
*  LP filter with k = 256. The signal calculates the difference of count by 
*  subtracting the previous baseline from the current raw count value and stores
*  it in `$INSTANCE_NAME`_SensorSignal[sensor]. 
*  If auto reset option is enabled the baseline updated regards noise threshold. 
*  If auto reset option is disabled the baseline stops updating if signal is 
*  greater that zero and baseline loads with raw count value if signal is less 
*  that noise threshold.
*  Raw data filters are applied to the values if enabled before baseline 
*  calculation.
*
* Parameters:
*  sensor:  Sensor number.
*
* Return:
*  None
*
* Global Variables:
*  `$INSTANCE_NAME`_SensorBaseline[]    - used to store baseline value.
*  `$INSTANCE_NAME`_SensorBaselineLow[] - used to store fraction byte of 
*  baseline value.
*  `$INSTANCE_NAME`_SensorSignal[]      - used to store diffence between 
*  current value of raw data and previous value of baseline.
*  `$INSTANCE_NAME`_rawFilterData1[]    - used to store previous sample of 
*  any enabled raw data filter.
*  `$INSTANCE_NAME`_rawFilterData2[]    - used to store before previous sample
*  of enabled raw data filter. Only required for median or average filters.
*
* Reentrant:
*  No
*
*******************************************************************************/
 void `$INSTANCE_NAME`_UpdateSensorBaseline(uint8 sensor)
                                 `=ReentrantKeil($INSTANCE_NAME . "_UpdateSensorBaseline")`
{
    uint32 calc;
    uint16 tempRaw;
    uint16 filteredRawData;
    uint8 widget = `$INSTANCE_NAME`_widgetNumber[sensor];
    uint8 noiseThreshold = `$INSTANCE_NAME`_noiseThreshold[widget];
    
    #if (`$INSTANCE_NAME`_TOTAL_GENERICS_COUNT)
        /* Exclude generic widget */
        if(widget < `$INSTANCE_NAME`_END_OF_WIDGETS_INDEX)
        {
    #endif  /* `$INSTANCE_NAME`_TOTAL_GENERICS_COUNT */
    
    filteredRawData = `$INSTANCE_NAME`_SensorRaw[sensor];
    
    #if (`$INSTANCE_NAME`_RAW_FILTER_MASK & `$INSTANCE_NAME`_MEDIAN_FILTER)
        tempRaw = filteredRawData;
        filteredRawData = `$INSTANCE_NAME`_MedianFilter(filteredRawData, `$INSTANCE_NAME`_rawFilterData1[sensor], 
                                                        `$INSTANCE_NAME`_rawFilterData2[sensor]);
        `$INSTANCE_NAME`_rawFilterData2[sensor] = `$INSTANCE_NAME`_rawFilterData1[sensor];
        `$INSTANCE_NAME`_rawFilterData1[sensor] = tempRaw;
        
    #elif (`$INSTANCE_NAME`_RAW_FILTER_MASK & `$INSTANCE_NAME`_AVERAGING_FILTER)
        tempRaw = filteredRawData;
        filteredRawData = `$INSTANCE_NAME`_AveragingFilter(filteredRawData, `$INSTANCE_NAME`_rawFilterData1[sensor],
                                                           `$INSTANCE_NAME`_rawFilterData2[sensor]);
        `$INSTANCE_NAME`_rawFilterData2[sensor] = `$INSTANCE_NAME`_rawFilterData1[sensor];
        `$INSTANCE_NAME`_rawFilterData1[sensor] = tempRaw;
        
    #elif (`$INSTANCE_NAME`_RAW_FILTER_MASK & `$INSTANCE_NAME`_IIR2_FILTER)
        filteredRawData = `$INSTANCE_NAME`_IIR2Filter(filteredRawData, `$INSTANCE_NAME`_rawFilterData1[sensor]);
        `$INSTANCE_NAME`_rawFilterData1[sensor] = filteredRawData;
        
    #elif (`$INSTANCE_NAME`_RAW_FILTER_MASK & `$INSTANCE_NAME`_IIR4_FILTER)
        filteredRawData = `$INSTANCE_NAME`_IIR4Filter(filteredRawData, `$INSTANCE_NAME`_rawFilterData1[sensor]);
        `$INSTANCE_NAME`_rawFilterData1[sensor] = filteredRawData;
            
    #elif (`$INSTANCE_NAME`_RAW_FILTER_MASK & `$INSTANCE_NAME`_JITTER_FILTER)
        filteredRawData = `$INSTANCE_NAME`_JitterFilter(filteredRawData, `$INSTANCE_NAME`_rawFilterData1[sensor]);
        `$INSTANCE_NAME`_rawFilterData1[sensor] = filteredRawData;
        
    #elif (`$INSTANCE_NAME`_RAW_FILTER_MASK & `$INSTANCE_NAME`_IIR8_FILTER)
        filteredRawData = `$INSTANCE_NAME`_IIR8Filter(filteredRawData, `$INSTANCE_NAME`_rawFilterData1[sensor]);
        `$INSTANCE_NAME`_rawFilterData1[sensor] = filteredRawData;
        
    #elif (`$INSTANCE_NAME`_RAW_FILTER_MASK & `$INSTANCE_NAME`_IIR16_FILTER)
        filteredRawData = `$INSTANCE_NAME`_IIR16Filter(filteredRawData, `$INSTANCE_NAME`_rawFilterData1[sensor]);
        `$INSTANCE_NAME`_rawFilterData1[sensor] = filteredRawData;
        
    #else
        /* No Raw filters */
    #endif  /* (`$INSTANCE_NAME`_RAW_FILTER_MASK & `$INSTANCE_NAME`_MEDIAN_FILTER) */
    
    #if (`$INSTANCE_NAME`_TUNING_METHOD == `$INSTANCE_NAME`_AUTO_TUNING)
        `$INSTANCE_NAME`_CalculateThresholds(sensor);
    #endif /* (`$INSTANCE_NAME`_TUNING_METHOD == `$INSTANCE_NAME`_AUTO_TUNING) */


    /* Baseline calculation */
    /* Calculate difference RawData[cur] - Baseline[prev] */
    if(filteredRawData >= `$INSTANCE_NAME`_SensorBaseline[sensor])
    {
        tempRaw = filteredRawData - `$INSTANCE_NAME`_SensorBaseline[sensor];
        widget = 1u;    /* Positive difference - Calculate the Signal */
    }
    else
    {
        tempRaw = `$INSTANCE_NAME`_SensorBaseline[sensor] - filteredRawData;
        widget = 0u;    /* Negative difference - Do NOT calculate the Signal */
    }

    if((widget == 0u) && (tempRaw > (uint16) `$INSTANCE_NAME`_NEGATIVE_NOISE_THRESHOLD))
    {
        if(`$INSTANCE_NAME`_LowBaselineResetCnt[sensor] >= `$INSTANCE_NAME`_LOW_BASELINE_RESET)
        {
            `$INSTANCE_NAME`_BaseInit(sensor);
            `$INSTANCE_NAME`_LowBaselineResetCnt[sensor] = 0;
        }
        else
        {
            `$INSTANCE_NAME`_LowBaselineResetCnt[sensor]++;
        }
    }
    else
    {
        #if (`$INSTANCE_NAME`_AUTO_RESET == `$INSTANCE_NAME`_AUTO_RESET_DISABLE)
            /* Update Baseline if lower that noiseThreshold */
            if ( (tempRaw <= (uint16) noiseThreshold) || 
                 ((tempRaw < (uint16) `$INSTANCE_NAME`_NEGATIVE_NOISE_THRESHOLD)
                   && widget == 0))
            {
        #endif /* (`$INSTANCE_NAME`_AUTO_RESET == `$INSTANCE_NAME`_AUTO_RESET_DISABLE) */
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

                `$INSTANCE_NAME`_LowBaselineResetCnt[sensor] = 0;
        #if (`$INSTANCE_NAME`_AUTO_RESET == `$INSTANCE_NAME`_AUTO_RESET_DISABLE)
            }
        #endif /* (`$INSTANCE_NAME`_AUTO_RESET == `$INSTANCE_NAME`_AUTO_RESET_DISABLE) */
    }

    /* Calculate Signal if possitive difference > noiseThreshold */
    if((tempRaw > (uint16) noiseThreshold) && (widget != 0u))
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
        #endif  /* (`$INSTANCE_NAME`_SIGNAL_SIZE == `$INSTANCE_NAME`_SIGNAL_SIZE_UINT8) */
    }
    else
    {
        /* Signal is zero */
        `$INSTANCE_NAME`_SensorSignal[sensor] = 0u;
    }

    #if (`$INSTANCE_NAME`_TOTAL_GENERICS_COUNT)
        /* Exclude generic widget */
        }
    #endif  /* `$INSTANCE_NAME`_TOTAL_GENERICS_COUNT */
}


/*******************************************************************************
* Function Name: `$INSTANCE_NAME`_UpdateEnabledBaselines
********************************************************************************
*
* Summary:
*  Checks `$INSTANCE_NAME`_SensorEnableMask[] array and calls the 
*  `$INSTANCE_NAME`_UpdateSensorBaseline function to update the baselines 
*  for enabled sensors.
*
* Parameters:
*  None
*
* Return:
*  None
*
* Global Variables:
*  `$INSTANCE_NAME`_SensorEnableMask[] - used to store the sensor scanning 
*  state.
*  `$INSTANCE_NAME`_SensorEnableMask[0] contains the masked bits for sensors 
*   0 through 7 (sensor 0 is bit 0, sensor 1 is bit 1).
*  `$INSTANCE_NAME`_SensorEnableMask[1] contains the masked bits for 
*  sensors 8 through 15 (if needed), and so on.
*  0 - sensor doesn't scan by `$INSTANCE_NAME`_ScanEnabledWidgets().
*  1 - sensor scans by `$INSTANCE_NAME`_ScanEnabledWidgets().
*
* Reentrant:
*  No
*
*******************************************************************************/
 void `$INSTANCE_NAME`_UpdateEnabledBaselines(void)
                                 `=ReentrantKeil($INSTANCE_NAME . "_UpdateEnabledBaselines")`
{
    uint8 i;
    uint8 pos;
    uint8 enMask;
    
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
*  Compares the `$INSTANCE_NAME`_SensorSignal[sensor] array element to finger
*  threshold of widget it belongs to. The hysteresis and debounce are taken into 
*  account. The hysteresis is added or subtracted from the finger threshold 
*  based on whether the sensor is currently active. 
*  If the sensor is active, the threshold is lowered by the hysteresis amount.
*  If the sensor is inactive, the threshold is raised by the hysteresis amount.
*  The debounce counter added to the sensor active transition.
*  This function updates `$INSTANCE_NAME`_SensorOnMask[] array element.
*
* Parameters:
*  sensor:  Sensor number.
*
* Return:
*  Returns sensor state 1 if active, 0 if not active.
*
* Global Variables:
*  `$INSTANCE_NAME`_SensorSignal[]      - used to store diffence between 
*  current value of raw data and previous value of baseline.
*  `$INSTANCE_NAME`_debounceCounter[]   - used to store current debounce 
*  counter of sensor. Widget which has this parameter are buttons, matrix 
*  buttons, proximity, guard. All other widgets haven't debounce parameter
*  and use the last element of this array with value 0 (it means no debounce).
*  `$INSTANCE_NAME`_SensorOnMask[] - used to store sensors on/off state.
*  `$INSTANCE_NAME`_SensorOnMask[0] contains the masked bits for sensors 
*   0 through 7 (sensor 0 is bit 0, sensor 1 is bit 1).
*  `$INSTANCE_NAME`_SensorEnableMask[1] contains the masked bits for 
*  sensors 8 through 15 (if needed), and so on.
*  0 - sensor is inactive.
*  1 - sensor is active.
*
* Reentrant:
*  No
*
*******************************************************************************/
uint8 `$INSTANCE_NAME`_CheckIsSensorActive(uint8 sensor)
                                  `=ReentrantKeil($INSTANCE_NAME . "_CheckIsSensorActive")`
{
    uint8 debounceIndex;
    /* Get On/Off mask */
    uint8 pos = (sensor >> 3u);
    uint8 onMask = 0x01u << (sensor & 0x07u);
    /* Prepare to find debounce counter index */
    uint8 widget = `$INSTANCE_NAME`_widgetNumber[sensor];
    uint8 fingerThreshold = `$INSTANCE_NAME`_fingerThreshold[widget];
    uint8 hysteresis = `$INSTANCE_NAME`_hysteresis[widget];
    uint8 debounce = `$INSTANCE_NAME`_debounce[widget];
    
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
            /* Sensor active, decrement debounce counter */
            if (`$INSTANCE_NAME`_debounceCounter[debounceIndex]-- <= 1u)
            {
                `$INSTANCE_NAME`_SensorOnMask[pos] |= onMask; 
            }
        }
        else
        {
            /* Sensor inactive - reset Debounce counter */
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
*  Use function `$INSTANCE_NAME`_CheckIsSensorActive() to update 
*  `$INSTANCE_NAME`_SensorOnMask[] for all sensors within the widget.
*  If one of sensors within widget is active the function return that widget is 
*  active.
*  The touch pad and matrix buttons widgets need to have active sensor within 
*  col and row to return widget active status.
*
* Parameters:
*  widget:  widget number.
*
* Return:
*  Returns widget sensor state 1 if one or more sensors within widget is/are 
*  active, 0 if all sensors within widget are inactive.
*
* Reentrant:
*  No
*
*******************************************************************************/
uint8 `$INSTANCE_NAME`_CheckIsWidgetActive(uint8 widget)
                                 `=ReentrantKeil($INSTANCE_NAME . "_CheckIsWidgetActive")`
{
    uint8 rawIndex = `$INSTANCE_NAME`_rawDataIndex[widget];
    uint8 numberOfSensors = `$INSTANCE_NAME`_numberOfSensors[widget] + rawIndex;
    uint8 state = 0u;

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
*  Compares all sensors of the `$INSTANCE_NAME`_Signal[] array to their finger 
*  threshold. Calls `$INSTANCE_NAME`_CheckIsWidgetActive() for each widget so 
*  the `$INSTANCE_NAME`_SensorOnMask[] array is up to date after calling this 
*  function.
*
* Parameters:
*  widget:  widget number.
*
* Return:
*  Returns 1 if any widget is active, 0 none of widgets are active.
*
* Reentrant:
*  No
*
*******************************************************************************/
uint8 `$INSTANCE_NAME`_CheckIsAnyWidgetActive(void)
                                 `=ReentrantKeil($INSTANCE_NAME . "_CheckIsAnyWidgetActive")`
{
`$writerCSHLIsAnyWidget`
    return state;
}


/*******************************************************************************
* Function Name: `$INSTANCE_NAME`_EnableWidget
********************************************************************************
*
* Summary:
*  Enable all widget elements (sensors) to scanning process.
*
* Parameters:
*  widget:  widget number.
*
* Return:
*  None
*
* Global Variables:
*  `$INSTANCE_NAME`_SensorEnableMask[] - used to store the sensor scanning 
*  state.
*  `$INSTANCE_NAME`_SensorEnableMask[0] contains the masked bits for sensors 
*  0 through 7 (sensor 0 is bit 0, sensor 1 is bit 1).
*  `$INSTANCE_NAME`_SensorEnableMask[1] contains the masked bits for 
*  sensors 8 through 15 (if needed), and so on.
*  0 - sensor doesn't scan by `$INSTANCE_NAME`_ScanEnabledWidgets().
*  1 - sensor scans by `$INSTANCE_NAME`_ScanEnabledWidgets().
*
* Reentrant:
*  No
*
*******************************************************************************/
void `$INSTANCE_NAME`_EnableWidget(uint8 widget)
                                   `=ReentrantKeil($INSTANCE_NAME . "_EnableWidget")`
{
    uint8 pos;
    uint8 enMask;
    uint8 rawIndex = `$INSTANCE_NAME`_rawDataIndex[widget];
    uint8 numberOfSensors = `$INSTANCE_NAME`_numberOfSensors[widget] + rawIndex;
    
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
*  Disable all widget elements (sensors) from scanning process.
*
* Parameters:
*  widget:  widget number.
*
* Return:
*  None
*
* Global Variables:
*  `$INSTANCE_NAME`_SensorEnableMask[] - used to store the sensor scanning 
*  state.
*  `$INSTANCE_NAME`_SensorEnableMask[0] contains the masked bits for sensors 
*  0 through 7 (sensor 0 is bit 0, sensor 1 is bit 1).
*  `$INSTANCE_NAME`_SensorEnableMask[1] contains the masked bits for 
*  sensors 8 through 15 (if needed), and so on.
*  0 - sensor doesn't scan by `$INSTANCE_NAME`_ScanEnabledWidgets().
*  1 - sensor scans by `$INSTANCE_NAME`_ScanEnabledWidgets().
*
* Reentrant:
*  No
*
*******************************************************************************/
void `$INSTANCE_NAME`_DisableWidget(uint8 widget)
                                    `=ReentrantKeil($INSTANCE_NAME . "_DisableWidget")`
{
    uint8 pos;
    uint8 enMask;
    uint8 rawIndex = `$INSTANCE_NAME`_rawDataIndex[widget];
    uint8 numberOfSensors = `$INSTANCE_NAME`_numberOfSensors[widget] + rawIndex;
   
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
#if(`$INSTANCE_NAME`_TOTAL_CENTROIDS_COUNT)
    /*******************************************************************************
    * Function Name: `$INSTANCE_NAME`_FindMaximum
    ********************************************************************************
    *
    * Summary:
    *  Finds index of maximum element within defined centroid. Checks 
    *  `$INSTANCE_NAME`_SensorSignal[] within defined cenrtoid and 
    *  returns index of maximum element. The values below finger threshold are 
    *  ignored.
    *  The centrod defines by offset of first element and number of elements - count.
    *  The diplexed centroid requires at least consecutive two elements above
    *  FingerThreshold to find index of maximum element.
    * 
    * Parameters:
    *  offset:  Start index of cetroid in `$INSTANCE_NAME`_SensorSignal[] array.
    *  count:   number of elements within centroid.
    *  fingerThreshold:  Finger threshould.
    *  diplex:   pointer to diplex table.
    * 
    * Return:
    *  Returns index of maximum element within defined centroid.
    *  If index of maximum element doesn't find the 0xFF returns.
    * 
    *******************************************************************************/
    #if (`$INSTANCE_NAME`_IS_DIPLEX_SLIDER)
        uint8 `$INSTANCE_NAME`_FindMaximum(uint8 offset, uint8 count, uint8 fingerThreshold, const uint8 CYCODE *diplex)
	                                       `=ReentrantKeil($INSTANCE_NAME . "_FindMaximum")`
    #else 
        uint8 `$INSTANCE_NAME`_FindMaximum(uint8 offset, uint8 count, uint8 fingerThreshold)
	                                       `=ReentrantKeil($INSTANCE_NAME . "_FindMaximum")`
    #endif /* (`$INSTANCE_NAME`_IS_DIPLEX_SLIDER) */
    {
        uint8 i;
        #if (`$INSTANCE_NAME`_IS_DIPLEX_SLIDER)        
            uint8 curPos = 0u;
            /* No centroid at the Start */
            uint8 curCntrdSize = 0u;
            uint8 curCtrdStartPos = 0xFFu;
            /* The biggset centroid is zero */
            uint8 biggestCtrdSize = 0u;
            uint8 biggestCtrdStartPos = 0u;
        #endif /* (`$INSTANCE_NAME`_IS_DIPLEX_SLIDER) */
        uint8 maximum = 0xFFu;
        `$SignalSizeReplacementString` temp = 0u;
        `$SignalSizeReplacementString` *startOfSlider = &`$INSTANCE_NAME`_SensorSignal[offset]; 

        #if (`$INSTANCE_NAME`_IS_DIPLEX_SLIDER)        
            if(diplex != 0u)
            {
                /* Initialize */
                i = 0u;
                
                /* Make slider x2 as Diplexed */
                count <<= 1u;
                while(1u)
                { 
                    if (startOfSlider[curPos] > 0u)    /* Looking for centroids */
                    {
                        if (curCtrdStartPos == 0xFFu)
                        {
                            /* Start of centroid */
                            curCtrdStartPos = i;
                            curCntrdSize++;
                        }
                        else
                        {
                            curCntrdSize++;
                        }
                    }
                    else   /* Select the bigest and indicate zero start */
                    {          
                        if(curCntrdSize > 0)
                        {
                            /* We are in the end of current */
                            if(curCntrdSize > biggestCtrdSize)
                            {
                                biggestCtrdSize = curCntrdSize;
                                biggestCtrdStartPos = curCtrdStartPos;
                            }
                            
                            curCntrdSize = 0u;
                            curCtrdStartPos = 0xFFu;
                        }
                    }
                    
                    i++; 
                    curPos = diplex[i];
                    if(i == count)
                    {
                        break;
                    }            
                }
                    
                    /* Find the biggest centroid if two are the same size, last one wins
                       We are in the end of current */
                if (curCntrdSize >= biggestCtrdSize) 
                {
                    biggestCtrdSize = curCntrdSize;
                    biggestCtrdStartPos = curCtrdStartPos;
                }
            }
            else
            {
                /* Without diplexing */ 
                biggestCtrdSize = count;
            }
                        

            /* Check centroid size */
            #if (`$INSTANCE_NAME`_IS_NON_DIPLEX_SLIDER)
                if((biggestCtrdSize >= 2u) || ((biggestCtrdSize == 1u) && (diplex == 0u)))
            #else                    
                if(biggestCtrdSize >= 2u)
            #endif /* (`$INSTANCE_NAME`_IS_NON_DIPLEX_SLIDER) */
                {
                    for (i = biggestCtrdStartPos; i < (biggestCtrdStartPos + biggestCtrdSize); i++)
                    {
                        #if (`$INSTANCE_NAME`_IS_DIPLEX_SLIDER && `$INSTANCE_NAME`_IS_NON_DIPLEX_SLIDER)
                            if (diplex == 0u)
                            {
                                curPos = i;
                            }
                            else
                            {
                                curPos = diplex[i];
                            }                    
                        #elif (`$INSTANCE_NAME`_IS_DIPLEX_SLIDER)                    
                            curPos = diplex[i];                    
                        #endif /* (`$INSTANCE_NAME`_IS_DIPLEX_SLIDER && `$INSTANCE_NAME`_IS_NON_DIPLEX_SLIDER) */
                        /* Looking for the grater element within centroid */
                        if(startOfSlider[curPos] > fingerThreshold)
                        {
                            if(startOfSlider[curPos] > temp)
                            {
                                maximum = i;
                                temp = startOfSlider[curPos];
                            }
                        }
                    }
                } 
        #else
            for (i = 0u; i < count; i++)
            {                      
                /* Looking for the grater element within centroid */
                if(startOfSlider[i] > fingerThreshold)
                {
                    if(startOfSlider[i] > temp)
                    {
                        maximum = i;
                        temp = startOfSlider[i];
                    }
                }
            }    
        #endif /* (`$INSTANCE_NAME`_IS_DIPLEX_SLIDER) */
        return (maximum);
    }
    
    
    /*******************************************************************************
    * Function Name: `$INSTANCE_NAME`_CalcCentroid
    ********************************************************************************
    *
    * Summary:
    *  Returns position value calculated accoring index of maximum element and API
    *  resolution.
    *
    * Parameters:
    *  type:  widget type.
    *  diplex:  pointer to diplex table.
    *  maximum:  Index of maximum element within centroid.
    *  offset:   Start index of cetroid in `$INSTANCE_NAME`_SensorSignal[] array.
    *  count:    Number of elements within centroid.
    *  resolution:  multiplicator calculated according to centroid type and
    *  API resolution.
    *  noiseThreshold:  Noise threshould.
    * 
    * Return:
    *  Returns position value of the slider.
    * 
    *
    * Reentrant:
    *  No.
    *
    *******************************************************************************/
    uint8 `$INSTANCE_NAME`_CalcCentroid(`$CalcCentroidPrototypeCallReplacementString`uint8 maximum, uint8 offset, 
                                        uint8 count, uint16 resolution, uint8 noiseThreshold)
	                                    `=ReentrantKeil($INSTANCE_NAME . "_CalcCentroid")`
    {
        #if ((`$INSTANCE_NAME`_TOTAL_LINEAR_SLIDERS_COUNT > 0u) || (`$INSTANCE_NAME`_TOTAL_TOUCH_PADS_COUNT > 0u))
            uint8 posPrev;
            uint8 posNext;
        #endif /* ((`$INSTANCE_NAME`_TOTAL_LINEAR_SLIDERS_COUNT>0u) || (`$INSTANCE_NAME`_TOTAL_TOUCH_PADS_COUNT>0u)) */
               
        #if (`$INSTANCE_NAME`_IS_DIPLEX_SLIDER)                
            uint8 pos;
        #endif /* (`$INSTANCE_NAME`_IS_DIPLEX_SLIDER) */

        uint8 position;
        int32 numerator;
        int32 denominator;
        `$SignalSizeReplacementString` *startOfSlider = &`$INSTANCE_NAME`_SensorSignal[offset];
                    
        #if (`$INSTANCE_NAME`_ADD_SLIDER_TYPE)
            if(type == `$INSTANCE_NAME`_TYPE_RADIAL_SLIDER)
            {
        #endif /* (`$INSTANCE_NAME`_ADD_SLIDER_TYPE) */

            #if (`$INSTANCE_NAME`_TOTAL_RADIAL_SLIDERS_COUNT > 0u)                
                /* Copy Signal for found centriod */
                `$INSTANCE_NAME`_centroid[`$INSTANCE_NAME`_POS] = startOfSlider[maximum];
                 
                /* Check borders for ROTARY Slider */
                if (maximum == 0u)                   /* Start of centroid */
                { 
                    `$INSTANCE_NAME`_centroid[`$INSTANCE_NAME`_POS_PREV] = startOfSlider[count - 1u];
                    `$INSTANCE_NAME`_centroid[`$INSTANCE_NAME`_POS_NEXT] = startOfSlider[maximum + 1u];
                }
                else if (maximum == (count - 1u))    /* End of centroid */
                {
                    `$INSTANCE_NAME`_centroid[`$INSTANCE_NAME`_POS_PREV] = startOfSlider[maximum - 1u];
                    `$INSTANCE_NAME`_centroid[`$INSTANCE_NAME`_POS_NEXT] = startOfSlider[0u];
                }
                else                                /* Not first Not last */
                {
                    `$INSTANCE_NAME`_centroid[`$INSTANCE_NAME`_POS_PREV] = startOfSlider[maximum - 1u];
                    `$INSTANCE_NAME`_centroid[`$INSTANCE_NAME`_POS_NEXT] = startOfSlider[maximum + 1u];
                }
            #endif /* (`$INSTANCE_NAME`_TOTAL_RADIAL_SLIDERS_COUNT > 0u) */

        #if (`$INSTANCE_NAME`_ADD_SLIDER_TYPE)
            }
            else
            {
        #endif

            #if ((`$INSTANCE_NAME`_TOTAL_LINEAR_SLIDERS_COUNT > 0u) || (`$INSTANCE_NAME`_TOTAL_TOUCH_PADS_COUNT > 0u))
                #if (`$INSTANCE_NAME`_IS_DIPLEX_SLIDER && `$INSTANCE_NAME`_IS_NON_DIPLEX_SLIDER)                    
                    /* Calculate next and previous near to maximum */
                    if(diplex == 0u)
                    {
                        pos     = maximum;
                        posPrev = maximum - 1u;
                        posNext = maximum + 1u; 
                    }
                    else
                    {
                        pos     = diplex[maximum];
                        posPrev = diplex[maximum - 1u];
                        posNext = diplex[maximum + 1u];
                        count <<= 1u;
                    }                    
                #elif (`$INSTANCE_NAME`_IS_DIPLEX_SLIDER)
                    /* Calculate next and previous near to maximum */
                    pos     = diplex[maximum];
                    posPrev = diplex[maximum - 1u];
                    posNext = diplex[maximum + 1u];
                    count <<= 1u;                    
                #else                    
                    /* Calculate next and previous near to maximum */
                    posPrev = maximum - 1u;
                    posNext = maximum + 1u; 
                #endif /* (`$INSTANCE_NAME`_IS_DIPLEX_SLIDER && `$INSTANCE_NAME`_IS_NON_DIPLEX_SLIDER) */
                        
                /* Copy Signal for found centriod */
                #if (`$INSTANCE_NAME`_IS_DIPLEX_SLIDER)
                    `$INSTANCE_NAME`_centroid[`$INSTANCE_NAME`_POS] = startOfSlider[pos];
                #else
                    `$INSTANCE_NAME`_centroid[`$INSTANCE_NAME`_POS] = startOfSlider[maximum];
                #endif /* (`$INSTANCE_NAME`_IS_DIPLEX_SLIDER) */
                    
                /* Check borders for LINEAR Slider */
                if (maximum == 0u)                   /* Start of centroid */
                { 
                    `$INSTANCE_NAME`_centroid[`$INSTANCE_NAME`_POS_PREV] = 0u;
                    `$INSTANCE_NAME`_centroid[`$INSTANCE_NAME`_POS_NEXT] = startOfSlider[posNext];
                }
                else if (maximum == ((count) - 1u)) /* End of centroid */
                {
                    `$INSTANCE_NAME`_centroid[`$INSTANCE_NAME`_POS_PREV] = startOfSlider[posPrev];
                    `$INSTANCE_NAME`_centroid[`$INSTANCE_NAME`_POS_NEXT] = 0u;
                }
                else                                /* Not first Not last */
                {
                    `$INSTANCE_NAME`_centroid[`$INSTANCE_NAME`_POS_PREV] = startOfSlider[posPrev];
                    `$INSTANCE_NAME`_centroid[`$INSTANCE_NAME`_POS_NEXT] = startOfSlider[posNext];
                }
            #endif /* ((`$INSTANCE_NAME`_TOTAL_LINEAR_SLIDERS_COUNT>0u)||(`$INSTANCE_NAME`_TOTAL_TOUCH_PADS_COUNT>0u))*/

        #if (`$INSTANCE_NAME`_ADD_SLIDER_TYPE)
            }
        #endif /* (`$INSTANCE_NAME`_ADD_SLIDER_TYPE) */
    
        /* Subtract noiseThreshold */
        if(`$INSTANCE_NAME`_centroid[`$INSTANCE_NAME`_POS_PREV] > noiseThreshold)
        {
            `$INSTANCE_NAME`_centroid[`$INSTANCE_NAME`_POS_PREV] -= noiseThreshold;
        }
        else
        {
            `$INSTANCE_NAME`_centroid[`$INSTANCE_NAME`_POS_PREV] = 0u;
        }
        
        /* Maximum always grater than fingerThreshold, so grate than noiseThreshold */
        `$INSTANCE_NAME`_centroid[`$INSTANCE_NAME`_POS] -= noiseThreshold;
        
        /* Subtract noiseThreshold */
        if(`$INSTANCE_NAME`_centroid[`$INSTANCE_NAME`_POS_NEXT] > noiseThreshold)
        {
            `$INSTANCE_NAME`_centroid[`$INSTANCE_NAME`_POS_NEXT] -= noiseThreshold;
        }
        else
        {
            `$INSTANCE_NAME`_centroid[`$INSTANCE_NAME`_POS_NEXT] = 0u;
        }
        
        
        /* Si+1 - Si-1 */
        numerator = (int32) `$INSTANCE_NAME`_centroid[`$INSTANCE_NAME`_POS_NEXT] - 
                    (int32) `$INSTANCE_NAME`_centroid[`$INSTANCE_NAME`_POS_PREV];
        
        /* Si+1 + Si + Si-1 */
        denominator = (int32) `$INSTANCE_NAME`_centroid[`$INSTANCE_NAME`_POS_PREV] + 
                      (int32) `$INSTANCE_NAME`_centroid[`$INSTANCE_NAME`_POS] + 
                      (int32) `$INSTANCE_NAME`_centroid[`$INSTANCE_NAME`_POS_NEXT];
        
        /* (numerator/denominator) + maximum */
        denominator = (numerator << 8u)/denominator + ((uint16) maximum << 8u);
        
        #if(`$INSTANCE_NAME`_TOTAL_RADIAL_SLIDERS_COUNT > 0u)
            /* Only required for RADIAL Slider */
            if(denominator < 0)
            {
                denominator += ((uint16) count << 8u);
            }
        #endif /* (`$INSTANCE_NAME`_TOTAL_RADIAL_SLIDERS_COUNT > 0u) */
        
        denominator *= resolution;
        
        /* Round the relust and put it to uint8 */
        position = ((uint8) HI16(denominator + `$INSTANCE_NAME`_CENTROID_ROUND_VALUE));

        return (position);
    }    
#endif /* (`$INSTANCE_NAME`_TOTAL_CENTROIDS_COUNT) */


#if(`$INSTANCE_NAME`_TOTAL_LINEAR_SLIDERS_COUNT > 0u)
    /*******************************************************************************
    * Function Name: `$INSTANCE_NAME`_GetCentroidPos
    ********************************************************************************
    *
    * Summary:
    *  Checks the `$INSTANCE_NAME`_Signal[ ] array for a centroid within
    *  slider specified range. The centroid position is calculated to the resolution
    *  specified in the CapSense customizer. The position filters are applied to the
    *  result if enabled.
    *
    * Parameters:
    *  widget:  Widget number.
    *  For every linear slider widget there are defines in this format:
    *  #define `$INSTANCE_NAME`_LS__"widget_name"            5
    * 
    * Return:
    *  Returns position value of the linear slider.
    *
    * Side Effects:
    *  If any sensor within the slider widget is active, the function returns values
    *  from zero to the API resolution value set in the CapSense customizer. If no
    *  sensors are active, the function returns 0xFFFF. If an error occurs during
    *  execution of the centroid/diplexing algorithm, the function returns 0xFFFF.
    *  There are no checks of widget type argument provided to this function.
    *  The unproper widget type provided will cause unexpected position calculations.
    *
    * Note:
    *  If noise counts on the slider segments are greater than the noise
    *  threshold, this subroutine may generate a false centroid result. The noise
    *  threshold should be set carefully (high enough above the noise level) so
    *  that noise will not generate a false centroid.
    *
    * Reentrant:
    *  No.
    *
    *******************************************************************************/
    uint16 `$INSTANCE_NAME`_GetCentroidPos(uint8 widget) `=ReentrantKeil($INSTANCE_NAME . "_GetCentroidPos")`
    {
        #if (`$INSTANCE_NAME`_IS_DIPLEX_SLIDER)
            const uint8 CYCODE *diplex;
        #endif /* (`$INSTANCE_NAME`_IS_DIPLEX_SLIDER) */
                
        #if (0u != `$INSTANCE_NAME`_LINEAR_SLIDERS_POS_FILTERS_MASK)
            uint8 posIndex;
            uint8 firstTimeIndex = `$INSTANCE_NAME`_posFiltersData[widget];
            uint8 posFiltersMask = `$INSTANCE_NAME`_posFiltersMask[widget];  
        #endif /* (0u != `$INSTANCE_NAME`_LINEAR_SLIDERS_POS_FILTERS_MASK) */

        #if ((0u != (`$INSTANCE_NAME`_MEDIAN_FILTER & `$INSTANCE_NAME`_LINEAR_SLIDERS_POS_FILTERS_MASK)) || \
             (0u != (`$INSTANCE_NAME`_AVERAGING_FILTER & `$INSTANCE_NAME`_LINEAR_SLIDERS_POS_FILTERS_MASK)))
            uint8 tempPos;
        #endif /* ((0u != (`$INSTANCE_NAME`_MEDIAN_FILTER & `$INSTANCE_NAME`_LINEAR_SLIDERS_POS_FILTERS_MASK)) || \
               *   (0u != (`$INSTANCE_NAME`_AVERAGING_FILTER & `$INSTANCE_NAME`_LINEAR_SLIDERS_POS_FILTERS_MASK)))
               */

        uint8 maximum;
        uint16 position;
        uint8 offset = `$INSTANCE_NAME`_rawDataIndex[widget];
        uint8 count = `$INSTANCE_NAME`_numberOfSensors[widget];
                        
        #if (`$INSTANCE_NAME`_IS_DIPLEX_SLIDER)
            if(widget < `$INSTANCE_NAME`_TOTAL_DIPLEXED_SLIDERS_COUNT)
            {
                maximum = `$INSTANCE_NAME`_diplexTable[widget];
                diplex = &`$INSTANCE_NAME`_diplexTable[maximum];
            }
            else
            {
                diplex = 0u;
            }
        #endif /* (`$INSTANCE_NAME`_IS_DIPLEX_SLIDER) */

        /* Find Maximum within centroid */      
        #if (`$INSTANCE_NAME`_IS_DIPLEX_SLIDER)        
            maximum = `$INSTANCE_NAME`_FindMaximum(offset, count, `$INSTANCE_NAME`_fingerThreshold[widget], diplex);
        #else
            maximum = `$INSTANCE_NAME`_FindMaximum(offset, count, `$INSTANCE_NAME`_fingerThreshold[widget]);
        #endif /* (`$INSTANCE_NAME`_IS_DIPLEX_SLIDER) */

        if (maximum != 0xFFu)
        {
            /* Calculate centroid */
            position = (uint16) `$INSTANCE_NAME`_CalcCentroid(`$CalcCentroidLinearSliderCallReplacementString`maximum, 
                         offset, count, `$INSTANCE_NAME`_centroidMult[widget], `$INSTANCE_NAME`_noiseThreshold[widget]);

            #if (0u != `$INSTANCE_NAME`_LINEAR_SLIDERS_POS_FILTERS_MASK)
                /* Check if this linear slider has enabled filters */
                if (0u != (posFiltersMask & `$INSTANCE_NAME`_ANY_POS_FILTER))
                {
                    /* Caluclate position to store filters data */
                    posIndex  = firstTimeIndex + 1u;
                    
                    if (0u == `$INSTANCE_NAME`_posFiltersData[firstTimeIndex])
                    {
                        /* Init filters */
                        `$INSTANCE_NAME`_posFiltersData[posIndex] = (uint8) position;
                        #if ((0u != (`$INSTANCE_NAME`_MEDIAN_FILTER & \
                                     `$INSTANCE_NAME`_LINEAR_SLIDERS_POS_FILTERS_MASK)) || \
                             (0u != (`$INSTANCE_NAME`_AVERAGING_FILTER & \
                                     `$INSTANCE_NAME`_LINEAR_SLIDERS_POS_FILTERS_MASK)))

                            if ( (0u != (posFiltersMask & `$INSTANCE_NAME`_MEDIAN_FILTER)) || 
                                 (0u != (posFiltersMask & `$INSTANCE_NAME`_AVERAGING_FILTER)) )
                            {
                                `$INSTANCE_NAME`_posFiltersData[posIndex + 1u] = (uint8) position;
                            }
                        #endif /* ((0u != (`$INSTANCE_NAME`_MEDIAN_FILTER & \
                               *           `$INSTANCE_NAME`_LINEAR_SLIDERS_POS_FILTERS_MASK)) || \
                               *   (0u != (`$INSTANCE_NAME`_AVERAGING_FILTER & \
                               *           `$INSTANCE_NAME`_LINEAR_SLIDERS_POS_FILTERS_MASK)))
                               */
                        
                        `$INSTANCE_NAME`_posFiltersData[firstTimeIndex] = 1u;
                    }
                    else
                    {
                        /* Do filtering */
                        #if (0u != (`$INSTANCE_NAME`_MEDIAN_FILTER & `$INSTANCE_NAME`_LINEAR_SLIDERS_POS_FILTERS_MASK))
                            if (0u != (posFiltersMask & `$INSTANCE_NAME`_MEDIAN_FILTER))
                            {
                                tempPos = (uint8) position;
                                position = `$INSTANCE_NAME`_MedianFilter(position, 
                                                                    `$INSTANCE_NAME`_posFiltersData[posIndex],
                                                                    `$INSTANCE_NAME`_posFiltersData[posIndex + 1u]);
                                `$INSTANCE_NAME`_posFiltersData[posIndex + 1u] = 
                                                                             `$INSTANCE_NAME`_posFiltersData[posIndex];
                                `$INSTANCE_NAME`_posFiltersData[posIndex] = tempPos;
                            }
                        #endif /*(0u != (`$INSTANCE_NAME`_MEDIAN_FILTER &
                               *         `$INSTANCE_NAME`_LINEAR_SLIDERS_POS_FILTERS_MASK))
                               */

                        #if(0u!=(`$INSTANCE_NAME`_AVERAGING_FILTER & `$INSTANCE_NAME`_LINEAR_SLIDERS_POS_FILTERS_MASK))
                            if (0u != (posFiltersMask & `$INSTANCE_NAME`_AVERAGING_FILTER)) 
                            {
                                tempPos = (uint8) position;
                                position = `$INSTANCE_NAME`_AveragingFilter(position, 
                                                                       `$INSTANCE_NAME`_posFiltersData[posIndex],
                                                                       `$INSTANCE_NAME`_posFiltersData[posIndex + 1u]);
                                `$INSTANCE_NAME`_posFiltersData[posIndex+1u]=`$INSTANCE_NAME`_posFiltersData[posIndex];
                                `$INSTANCE_NAME`_posFiltersData[posIndex] = tempPos;
                            }
                        #endif /* (0u != (`$INSTANCE_NAME`_AVERAGING_FILTER & \
                               *           `$INSTANCE_NAME`_LINEAR_SLIDERS_POS_FILTERS_MASK))
                               */

                        #if (0u != (`$INSTANCE_NAME`_IIR2_FILTER & `$INSTANCE_NAME`_LINEAR_SLIDERS_POS_FILTERS_MASK)) 
                            if (0u != (posFiltersMask & `$INSTANCE_NAME`_IIR2_FILTER)) 
                            {
                                position = `$INSTANCE_NAME`_IIR2Filter(position,
                                                                       `$INSTANCE_NAME`_posFiltersData[posIndex]);
                                `$INSTANCE_NAME`_posFiltersData[posIndex] = (uint8) position;
                            }
                        #endif /* (0u != (`$INSTANCE_NAME`_IIR2_FILTER & \
                               *          `$INSTANCE_NAME`_LINEAR_SLIDERS_POS_FILTERS_MASK))
                               */

                        #if (0u != (`$INSTANCE_NAME`_IIR4_FILTER & `$INSTANCE_NAME`_LINEAR_SLIDERS_POS_FILTERS_MASK))
                            if (0u != (posFiltersMask & `$INSTANCE_NAME`_IIR4_FILTER))
                            {
                                position = `$INSTANCE_NAME`_IIR4Filter(position, 
                                                                       `$INSTANCE_NAME`_posFiltersData[posIndex]);
                                `$INSTANCE_NAME`_posFiltersData[posIndex] = (uint8) position;
                            }                                
                        #endif /* (0u != (`$INSTANCE_NAME`_IIR4_FILTER & \
                               *          `$INSTANCE_NAME`_LINEAR_SLIDERS_POS_FILTERS_MASK))
                               */

                        #if (0u != (`$INSTANCE_NAME`_JITTER_FILTER & `$INSTANCE_NAME`_LINEAR_SLIDERS_POS_FILTERS_MASK))
                            if (0u != (posFiltersMask & `$INSTANCE_NAME`_JITTER_FILTER))
                            {
                                position = `$INSTANCE_NAME`_JitterFilter(position, 
                                                                         `$INSTANCE_NAME`_posFiltersData[posIndex]);
                                `$INSTANCE_NAME`_posFiltersData[posIndex] = (uint8) position;
                            }
                        #endif /* (0u != (`$INSTANCE_NAME`_JITTER_FILTER & \
                               *           `$INSTANCE_NAME`_LINEAR_SLIDERS_POS_FILTERS_MASK))
                               */
                    }
                }
            #endif /* (0u != `$INSTANCE_NAME`_LINEAR_SLIDERS_POS_FILTERS_MASK) */

        }
        else
        {
            /* The maximum didn't find */
            position = 0xFFFFu;

            #if(0u != `$INSTANCE_NAME`_LINEAR_SLIDERS_POS_FILTERS_MASK)
                /* Reset the filters */
                if(0u != (posFiltersMask & `$INSTANCE_NAME`_ANY_POS_FILTER))
                {
                    `$INSTANCE_NAME`_posFiltersData[firstTimeIndex] = 0u;
                }
            #endif /* (0u != `$INSTANCE_NAME`_LINEAR_SLIDERS_POS_FILTERS_MASK) */
        }

        
        return (position);
    }
#endif /* (`$INSTANCE_NAME`_TOTAL_LINEAR_SLIDERS_COUNT > 0u) */


#if(`$INSTANCE_NAME`_TOTAL_RADIAL_SLIDERS_COUNT > 0u)
    /*******************************************************************************
    * Function Name: `$INSTANCE_NAME`_GetRadialCentroidPos
    ********************************************************************************
    *
    * Summary:
    *  Checks the `$INSTANCE_NAME`_Signal[ ] array for a centroid within
    *  slider specified range. The centroid position is calculated to the resolution
    *  specified in the CapSense customizer. The position filters are applied to the
    *  result if enabled.
    *
    * Parameters:
    *  widget:  Widget number.
    *  For every radial slider widget there are defines in this format:
    *  #define `$INSTANCE_NAME`_RS_"widget_name"            5
    * 
    * Return:
    *  Returns position value of the radial slider.
    *
    * Side Effects:
    *  If any sensor within the slider widget is active, the function returns values
    *  from zero to the API resolution value set in the CapSense customizer. If no
    *  sensors are active, the function returns 0xFFFF.
    *  There are no checks of widget type argument provided to this function.
    *  The unproper widget type provided will cause unexpected position calculations.
    *
    * Note:
    *  If noise counts on the slider segments are greater than the noise
    *  threshold, this subroutine may generate a false centroid result. The noise
    *  threshold should be set carefully (high enough above the noise level) so 
    *  that noise will not generate a false centroid.
    *
    *
    * Reentrant:
    *  No.
    *
    *******************************************************************************/
     uint16 `$INSTANCE_NAME`_GetRadialCentroidPos(uint8 widget)
	                                       `=ReentrantKeil($INSTANCE_NAME . "_GetRadialCentroidPos")`
    {
        #if (0u != `$INSTANCE_NAME`_RADIAL_SLIDERS_POS_FILTERS_MASK)
            uint8 posIndex;
            uint8 firstTimeIndex = `$INSTANCE_NAME`_posFiltersData[widget];
            uint8 posFiltersMask = `$INSTANCE_NAME`_posFiltersMask[widget]; 
        #endif /* (0u != `$INSTANCE_NAME`_RADIAL_SLIDERS_POS_FILTERS_MASK) */

        #if ((0u != (`$INSTANCE_NAME`_MEDIAN_FILTER & `$INSTANCE_NAME`_RADIAL_SLIDERS_POS_FILTERS_MASK)) || \
             (0u != (`$INSTANCE_NAME`_AVERAGING_FILTER & `$INSTANCE_NAME`_RADIAL_SLIDERS_POS_FILTERS_MASK)))
            uint8 tempPos;
        #endif /* ((0u != (`$INSTANCE_NAME`_MEDIAN_FILTER & `$INSTANCE_NAME`_RADIAL_SLIDERS_POS_FILTERS_MASK)) || \
               *   (0u != (`$INSTANCE_NAME`_AVERAGING_FILTER & `$INSTANCE_NAME`_RADIAL_SLIDERS_POS_FILTERS_MASK)))
               */

        uint8 maximum;
        uint16 position;
        uint8 offset = `$INSTANCE_NAME`_rawDataIndex[widget];
        uint8 count = `$INSTANCE_NAME`_numberOfSensors[widget];
        
        /* Find Maximum within centroid */        
        #if (`$INSTANCE_NAME`_IS_DIPLEX_SLIDER)
            maximum = `$INSTANCE_NAME`_FindMaximum(offset, count, `$INSTANCE_NAME`_fingerThreshold[widget], 0u);
        #else
            maximum = `$INSTANCE_NAME`_FindMaximum(offset, count, `$INSTANCE_NAME`_fingerThreshold[widget]);
        #endif /* (`$INSTANCE_NAME`_IS_DIPLEX_SLIDER) */
        
        if (maximum != 0xFFu)
        {
            /* Calculate centroid */
            position = (uint16) `$INSTANCE_NAME`_CalcCentroid(`$CalcCentroidRadialSliderCallReplacementString`maximum, 
                         offset, count, `$INSTANCE_NAME`_centroidMult[widget], `$INSTANCE_NAME`_noiseThreshold[widget]);

            #if (0u != `$INSTANCE_NAME`_RADIAL_SLIDERS_POS_FILTERS_MASK)
                /* Check if this Radial slider has enabled filters */
                if (0u != (posFiltersMask & `$INSTANCE_NAME`_ANY_POS_FILTER))
                {
                    /* Caluclate position to store filters data */
                    posIndex  = firstTimeIndex + 1u;
                    
                    if (0u == `$INSTANCE_NAME`_posFiltersData[firstTimeIndex])
                    {
                        /* Init filters */
                        `$INSTANCE_NAME`_posFiltersData[posIndex] = (uint8) position;
                        #if ((0u != (`$INSTANCE_NAME`_MEDIAN_FILTER & \
                                     `$INSTANCE_NAME`_RADIAL_SLIDERS_POS_FILTERS_MASK)) || \
                             (0u != (`$INSTANCE_NAME`_AVERAGING_FILTER & \
                                     `$INSTANCE_NAME`_RADIAL_SLIDERS_POS_FILTERS_MASK)))

                            if ( (0u != (posFiltersMask & `$INSTANCE_NAME`_MEDIAN_FILTER))  || 
                                 (0u != (posFiltersMask & `$INSTANCE_NAME`_AVERAGING_FILTER)) )
                            {
                                `$INSTANCE_NAME`_posFiltersData[posIndex + 1u] = (uint8) position;
                            }
                        #endif /* ((0u != (`$INSTANCE_NAME`_MEDIAN_FILTER & \
                               *           `$INSTANCE_NAME`_RADIAL_SLIDERS_POS_FILTERS_MASK)) || \
                               *   (0u != (`$INSTANCE_NAME`_AVERAGING_FILTER & \
                               *           `$INSTANCE_NAME`_RADIAL_SLIDERS_POS_FILTERS_MASK)))
                               */
                        
                        `$INSTANCE_NAME`_posFiltersData[firstTimeIndex] = 1u;
                    }
                    else
                    {
                        /* Do filtering */
                        #if (0u != (`$INSTANCE_NAME`_MEDIAN_FILTER & `$INSTANCE_NAME`_RADIAL_SLIDERS_POS_FILTERS_MASK))
                            if (0u != (posFiltersMask & `$INSTANCE_NAME`_MEDIAN_FILTER))
                            {
                                tempPos = (uint8) position;
                                position = `$INSTANCE_NAME`_MedianFilter(position,
                                                                        `$INSTANCE_NAME`_posFiltersData[posIndex],
                                                                        `$INSTANCE_NAME`_posFiltersData[posIndex + 1u]);
                                `$INSTANCE_NAME`_posFiltersData[posIndex + 1u] = 
                                                                              `$INSTANCE_NAME`_posFiltersData[posIndex];
                                `$INSTANCE_NAME`_posFiltersData[posIndex] = tempPos;
                            }
                        #endif /* (0u != (`$INSTANCE_NAME`_MEDIAN_FILTER & 
                               *          `$INSTANCE_NAME`_RADIAL_SLIDERS_POS_FILTERS_MASK))
                               */

                        #if (0u != (`$INSTANCE_NAME`_AVERAGING_FILTER & \
                                    `$INSTANCE_NAME`_RADIAL_SLIDERS_POS_FILTERS_MASK))
                            if (0u != (posFiltersMask & `$INSTANCE_NAME`_AVERAGING_FILTER))
                            {
                                tempPos = (uint8) position;
                                position = `$INSTANCE_NAME`_AveragingFilter(position, 
                                                                       `$INSTANCE_NAME`_posFiltersData[posIndex],
                                                                       `$INSTANCE_NAME`_posFiltersData[posIndex + 1u]);
                                `$INSTANCE_NAME`_posFiltersData[posIndex+1u]= `$INSTANCE_NAME`_posFiltersData[posIndex];
                                `$INSTANCE_NAME`_posFiltersData[posIndex] = tempPos;
                            }
                        #endif /* (0u != (`$INSTANCE_NAME`_AVERAGING_FILTER & \
                               *          `$INSTANCE_NAME`_RADIAL_SLIDERS_POS_FILTERS_MASK))
                               */

                        #if (0u != (`$INSTANCE_NAME`_IIR2_FILTER & `$INSTANCE_NAME`_RADIAL_SLIDERS_POS_FILTERS_MASK))
                            if (0u != (posFiltersMask & `$INSTANCE_NAME`_IIR2_FILTER))
                            {
                                position = `$INSTANCE_NAME`_IIR2Filter(position, 
                                                                       `$INSTANCE_NAME`_posFiltersData[posIndex]);
                                `$INSTANCE_NAME`_posFiltersData[posIndex] = (uint8) position;
                            }
                        #endif /* (0u != (`$INSTANCE_NAME`_IIR2_FILTER & 
                               *          `$INSTANCE_NAME`_RADIAL_SLIDERS_POS_FILTERS_MASK))
                               */

                        #if (0u != (`$INSTANCE_NAME`_IIR4_FILTER & `$INSTANCE_NAME`_RADIAL_SLIDERS_POS_FILTERS_MASK))
                            if (0u != (posFiltersMask & `$INSTANCE_NAME`_IIR4_FILTER))
                            {
                                position = `$INSTANCE_NAME`_IIR4Filter(position, 
                                                                       `$INSTANCE_NAME`_posFiltersData[posIndex]);
                                `$INSTANCE_NAME`_posFiltersData[posIndex] = (uint8) position;
                            }
                        #endif /* (0u != (`$INSTANCE_NAME`_IIR4_FILTER & 
                               *          `$INSTANCE_NAME`_RADIAL_SLIDERS_POS_FILTERS_MASK))
                               */

                        #if (0u != (`$INSTANCE_NAME`_JITTER_FILTER & `$INSTANCE_NAME`_RADIAL_SLIDERS_POS_FILTERS_MASK))
                            if (0u != (posFiltersMask & `$INSTANCE_NAME`_JITTER_FILTER))
                            {
                                position = `$INSTANCE_NAME`_JitterFilter(position, 
                                                                         `$INSTANCE_NAME`_posFiltersData[posIndex]);
                                `$INSTANCE_NAME`_posFiltersData[posIndex] = (uint8) position;
                            }
                        #endif /* (0u != (`$INSTANCE_NAME`_JITTER_FILTER &
                               *           `$INSTANCE_NAME`_RADIAL_SLIDERS_POS_FILTERS_MASK))
                               */
                    }
                }
            #endif /* (0u != `$INSTANCE_NAME`_RADIAL_SLIDERS_POS_FILTERS_MASK) */

        }
        else
        {
            /* The maximum didn't find */
            position = 0xFFFFu;

            #if (0u != `$INSTANCE_NAME`_RADIAL_SLIDERS_POS_FILTERS_MASK)
                /* Reset the filters */
                if((posFiltersMask & `$INSTANCE_NAME`_ANY_POS_FILTER) != 0u)
                {
                    `$INSTANCE_NAME`_posFiltersData[firstTimeIndex] = 0u;
                }
            #endif /* (0u != `$INSTANCE_NAME`_RADIAL_SLIDERS_POS_FILTERS_MASK) */
        }
        
        return (position);
    }
#endif


#if(`$INSTANCE_NAME`_TOTAL_TOUCH_PADS_COUNT > 0u)
    /*******************************************************************************
    * Function Name: `$INSTANCE_NAME`_GetTouchCentroidPos
    ********************************************************************************
    *
    * Summary:
    *  If a finger is present on touch pad, this function calculates the X and Y
    *  position of the finger by calculating the centroids within touch pad specified
    *  range. The X and Y positions are calculated to the API resolutions set in the
    *  CapSense customizer. Returns a 1 if a finger is on the touchpad.
    *  The position filter is applied to the result if enabled.
    *  This function is available only if a touch pad is defined by the CapSense
    *  customizer.
    *
    * Parameters:
    *  widget:  Widget number. 
    *  For every touchpad widget there are defines in this format:
    *  #define `$INSTANCE_NAME`_TP_"widget_name"            5
    *
    *  pos:     Pointer to the array of two uint16 elements, where result
    *  result of calculation of X and Y position are stored.
    *  pos[0u]  - position of X
    *  pos[1u]  - position of Y
    *
    * Return:
    *  Returns a 1 if a finger is on the touch pad, 0 - if not.
    *
    * Side Effects:
    *   There are no checks of widget type argument provided to this function.
    *   The unproper widget type provided will cause unexpected position
    *   calculations.
    *
    * Reentrant:
    *  No.
    *
    *******************************************************************************/
    uint8 `$INSTANCE_NAME`_GetTouchCentroidPos(uint8 widget, uint16* pos)
	                                    `=ReentrantKeil($INSTANCE_NAME . "_GetTouchCentroidPos")`
    {
        #if (0u != `$INSTANCE_NAME`_TOUCH_PADS_POS_FILTERS_MASK)
            uint8 posXIndex;
            uint8 posYIndex;
            uint8 firstTimeIndex = `$INSTANCE_NAME`_posFiltersData[widget];
            uint8 posFiltersMask = `$INSTANCE_NAME`_posFiltersMask[widget];
        #endif /* (0u != `$INSTANCE_NAME`_TOUCH_PADS_POS_FILTERS_MASK) */

        #if ((0u != (`$INSTANCE_NAME`_MEDIAN_FILTER & `$INSTANCE_NAME`_TOUCH_PADS_POS_FILTERS_MASK)) || \
             (0u != (`$INSTANCE_NAME`_AVERAGING_FILTER & `$INSTANCE_NAME`_TOUCH_PADS_POS_FILTERS_MASK)))
            uint16 tempPos;
        #endif /* ((0u != (`$INSTANCE_NAME`_MEDIAN_FILTER & `$INSTANCE_NAME`_TOUCH_PADS_POS_FILTERS_MASK)) || \
               *   (0u != (`$INSTANCE_NAME`_AVERAGING_FILTER & `$INSTANCE_NAME`_TOUCH_PADS_POS_FILTERS_MASK)))
               */

        uint8 MaxX;
        uint8 MaxY;
        uint8 posX;
        uint8 posY;
        uint8 touch = 0u;
        uint8 offset = `$INSTANCE_NAME`_rawDataIndex[widget];
        uint8 count = `$INSTANCE_NAME`_numberOfSensors[widget];
        
        /* Find Maximum within X centroid */
        #if (`$INSTANCE_NAME`_IS_DIPLEX_SLIDER)
            MaxX = `$INSTANCE_NAME`_FindMaximum(offset, count, `$INSTANCE_NAME`_fingerThreshold[widget], 0u);
        #else
            MaxX = `$INSTANCE_NAME`_FindMaximum(offset, count, `$INSTANCE_NAME`_fingerThreshold[widget]);
        #endif /* (`$INSTANCE_NAME`_IS_DIPLEX_SLIDER) */

        if (MaxX != 0xFFu)
        {
            offset = `$INSTANCE_NAME`_rawDataIndex[widget + 1u];
            count = `$INSTANCE_NAME`_numberOfSensors[widget + 1u];

            /* Find Maximum within Y centroid */
            #if (`$INSTANCE_NAME`_IS_DIPLEX_SLIDER)
                MaxY = `$INSTANCE_NAME`_FindMaximum(offset, count, `$INSTANCE_NAME`_fingerThreshold[widget + 1u], 0u);
            #else
                MaxY = `$INSTANCE_NAME`_FindMaximum(offset, count, `$INSTANCE_NAME`_fingerThreshold[widget + 1u]);
            #endif /* (`$INSTANCE_NAME`_IS_DIPLEX_SLIDER) */

            if (MaxY != 0xFFu)
            {
                /* X and Y maximums are found = true touch */
                touch = 1u;
                
                /* Calculate Y centroid */
                posY = `$INSTANCE_NAME`_CalcCentroid(`$CalcCentroidTouchPadCallReplacementString`MaxY, offset, count, 
                            `$INSTANCE_NAME`_centroidMult[widget + 1u], `$INSTANCE_NAME`_noiseThreshold[widget + 1u]);
                
                /* Calculate X centroid */
                offset = `$INSTANCE_NAME`_rawDataIndex[widget];
                count = `$INSTANCE_NAME`_numberOfSensors[widget];
                
                posX = `$INSTANCE_NAME`_CalcCentroid(`$CalcCentroidTouchPadCallReplacementString`MaxX, offset, count, 
                            `$INSTANCE_NAME`_centroidMult[widget],`$INSTANCE_NAME`_noiseThreshold[widget]);
    
                #if (0u != `$INSTANCE_NAME`_TOUCH_PADS_POS_FILTERS_MASK)
                    /* Check if this TP has enabled filters */
                    if (0u != (posFiltersMask & `$INSTANCE_NAME`_ANY_POS_FILTER))
                    {
                        /* Caluclate position to store filters data */
                        posXIndex  = firstTimeIndex + 1u;
                        posYIndex  = `$INSTANCE_NAME`_posFiltersData[widget + 1u];
                        
                        if (0u == `$INSTANCE_NAME`_posFiltersData[firstTimeIndex])
                        {
                            /* Init filters */
                            `$INSTANCE_NAME`_posFiltersData[posXIndex] = posX;
                            `$INSTANCE_NAME`_posFiltersData[posYIndex] = posY;

                            #if((0u != (`$INSTANCE_NAME`_MEDIAN_FILTER & \
                                        `$INSTANCE_NAME`_TOUCH_PADS_POS_FILTERS_MASK))|| \
                                (0u != (`$INSTANCE_NAME`_AVERAGING_FILTER & \
                                        `$INSTANCE_NAME`_TOUCH_PADS_POS_FILTERS_MASK)))

                                if ( (0u != (posFiltersMask & `$INSTANCE_NAME`_MEDIAN_FILTER)) || 
                                     (0u != (posFiltersMask & `$INSTANCE_NAME`_AVERAGING_FILTER)) )
                                {
                                    `$INSTANCE_NAME`_posFiltersData[posXIndex + 1u] = posX;
                                    `$INSTANCE_NAME`_posFiltersData[posYIndex + 1u] = posY;
                                }
                            #endif /* ((0u != (`$INSTANCE_NAME`_MEDIAN_FILTER & \
                                   *           `$INSTANCE_NAME`_TOUCH_PADS_POS_FILTERS_MASK)) || \
                                   *    (0u != (`$INSTANCE_NAME`_AVERAGING_FILTER & \
                                   *            `$INSTANCE_NAME`_TOUCH_PADS_POS_FILTERS_MASK)))
                                   */
                            
                            `$INSTANCE_NAME`_posFiltersData[firstTimeIndex] = 1u;
                        }
                        else
                        {
                            /* Do filtering */
                            #if (0u != (`$INSTANCE_NAME`_MEDIAN_FILTER & `$INSTANCE_NAME`_TOUCH_PADS_POS_FILTERS_MASK))
                                if (0u != (posFiltersMask & `$INSTANCE_NAME`_MEDIAN_FILTER))
                                {
                                    tempPos = posX;
                                    posX = (uint8) `$INSTANCE_NAME`_MedianFilter(posX,
                                                                      `$INSTANCE_NAME`_posFiltersData[posXIndex],
                                                                      `$INSTANCE_NAME`_posFiltersData[posXIndex + 1u]);
                                    `$INSTANCE_NAME`_posFiltersData[posXIndex + 1u] = 
                                                                             `$INSTANCE_NAME`_posFiltersData[posXIndex];
                                    `$INSTANCE_NAME`_posFiltersData[posXIndex] = tempPos;
                                    
                                    tempPos = posY;
                                    posY = (uint8) `$INSTANCE_NAME`_MedianFilter(posY,
                                                                       `$INSTANCE_NAME`_posFiltersData[posYIndex], 
                                                                       `$INSTANCE_NAME`_posFiltersData[posYIndex + 1u]);
                                    `$INSTANCE_NAME`_posFiltersData[posYIndex + 1u] = 
                                                                             `$INSTANCE_NAME`_posFiltersData[posYIndex];
                                    `$INSTANCE_NAME`_posFiltersData[posYIndex] = tempPos;
                                }
                                
                            #endif /* (0u != (`$INSTANCE_NAME`_MEDIAN_FILTER & \
                                   *          `$INSTANCE_NAME`_TOUCH_PADS_POS_FILTERS_MASK))
                                   */

                            #if(0u !=(`$INSTANCE_NAME`_AVERAGING_FILTER & `$INSTANCE_NAME`_TOUCH_PADS_POS_FILTERS_MASK))
                                if (0u != (posFiltersMask & `$INSTANCE_NAME`_AVERAGING_FILTER))
                                {
                                    tempPos = posX;
                                    posX = (uint8) `$INSTANCE_NAME`_AveragingFilter(posX,
                                                                       `$INSTANCE_NAME`_posFiltersData[posXIndex], 
                                                                       `$INSTANCE_NAME`_posFiltersData[posXIndex + 1u]);
                                    `$INSTANCE_NAME`_posFiltersData[posXIndex + 1u] = 
                                                                             `$INSTANCE_NAME`_posFiltersData[posXIndex];
                                    `$INSTANCE_NAME`_posFiltersData[posXIndex] = tempPos;
                                    
                                    tempPos = posY;
                                    posY = (uint8) `$INSTANCE_NAME`_AveragingFilter(posY, 
                                                                      `$INSTANCE_NAME`_posFiltersData[posYIndex], 
                                                                      `$INSTANCE_NAME`_posFiltersData[posYIndex + 1u]);
                                    `$INSTANCE_NAME`_posFiltersData[posYIndex + 1u] = 
                                                                            `$INSTANCE_NAME`_posFiltersData[posYIndex];
                                    `$INSTANCE_NAME`_posFiltersData[posYIndex] = tempPos;
                                }

                            #endif /* (0u != (`$INSTANCE_NAME`_AVERAGING_FILTER & \
                                   *           `$INSTANCE_NAME`_TOUCH_PADS_POS_FILTERS_MASK))
                                   */

                            #if (0u != (`$INSTANCE_NAME`_IIR2_FILTER & `$INSTANCE_NAME`_TOUCH_PADS_POS_FILTERS_MASK))
                                if (0u != (posFiltersMask & `$INSTANCE_NAME`_IIR2_FILTER))
                                {
                                    posX = (uint8) `$INSTANCE_NAME`_IIR2Filter(posX, 
                                                                           `$INSTANCE_NAME`_posFiltersData[posXIndex]);
                                    `$INSTANCE_NAME`_posFiltersData[posXIndex] = posX;
                                    
                                    posY = (uint8) `$INSTANCE_NAME`_IIR2Filter(posY, 
                                                                            `$INSTANCE_NAME`_posFiltersData[posYIndex]);
                                    `$INSTANCE_NAME`_posFiltersData[posYIndex] = posY;
                                }
                                
                            #endif /* (0u != (`$INSTANCE_NAME`_IIR2_FILTER & \
                                   *          `$INSTANCE_NAME`_TOUCH_PADS_POS_FILTERS_MASK))
                                   */

                            #if (0u != (`$INSTANCE_NAME`_IIR4_FILTER & `$INSTANCE_NAME`_TOUCH_PADS_POS_FILTERS_MASK))
                                if (0u != (posFiltersMask & `$INSTANCE_NAME`_IIR4_FILTER))
                                {
                                    posX = (uint8) `$INSTANCE_NAME`_IIR4Filter(posX, 
                                                                            `$INSTANCE_NAME`_posFiltersData[posXIndex]);
                                    `$INSTANCE_NAME`_posFiltersData[posXIndex] = posX;
                                    
                                    posY = (uint8) `$INSTANCE_NAME`_IIR4Filter(posY, 
                                                                            `$INSTANCE_NAME`_posFiltersData[posYIndex]);
                                    `$INSTANCE_NAME`_posFiltersData[posYIndex] = posY;
                                }
                                
                            #endif /* (0u != (`$INSTANCE_NAME`_IIR4_FILTER & \
                                   *           `$INSTANCE_NAME`_TOUCH_PADS_POS_FILTERS_MASK))
                                   */

                            #if (0u != (`$INSTANCE_NAME`_JITTER_FILTER & `$INSTANCE_NAME`_TOUCH_PADS_POS_FILTERS_MASK))
                                if (0u != (posFiltersMask & `$INSTANCE_NAME`_JITTER_FILTER))
                                    {
                                        posX = (uint8) `$INSTANCE_NAME`_JitterFilter(posX, 
                                                                            `$INSTANCE_NAME`_posFiltersData[posXIndex]);
                                        `$INSTANCE_NAME`_posFiltersData[posXIndex] = posX;
                                        
                                        posY = (uint8) `$INSTANCE_NAME`_JitterFilter(posY, 
                                                                            `$INSTANCE_NAME`_posFiltersData[posYIndex]);
                                        `$INSTANCE_NAME`_posFiltersData[posYIndex] = posY;
                                    }
                            #endif /* (0u != (`$INSTANCE_NAME`_JITTER_FILTER & \
                                   *           `$INSTANCE_NAME`_TOUCH_PADS_POS_FILTERS_MASK))
                                   */
                        }
                    }
                #endif /* (0u != `$INSTANCE_NAME`_TOUCH_PADS_POS_FILTERS_MASK) */

                /* Save positions */
                pos[0u] = posX;
                pos[1u] = posY;
            }
        }

        #if (0u != `$INSTANCE_NAME`_TOUCH_PADS_POS_FILTERS_MASK)
            if(touch == 0u)
            {
                /* Reset the filters */
                if ((posFiltersMask & `$INSTANCE_NAME`_ANY_POS_FILTER) != 0u)
                {
                    `$INSTANCE_NAME`_posFiltersData[firstTimeIndex] = 0u;
                }
            }
        #endif /* (0u != `$INSTANCE_NAME`_TOUCH_PADS_POS_FILTERS_MASK) */
        
        return (touch);
    }
#endif /* (`$INSTANCE_NAME`_TOTAL_TOUCH_PADS_COUNT > 0u) */


#if ( (`$INSTANCE_NAME`_RAW_FILTER_MASK & `$INSTANCE_NAME`_MEDIAN_FILTER) || \
      (`$INSTANCE_NAME`_POS_FILTERS_MASK & `$INSTANCE_NAME`_MEDIAN_FILTER) )
    /*******************************************************************************
    * Function Name: `$INSTANCE_NAME`_MedianFilter
    ********************************************************************************
    *
    * Summary:
    *  Median filter function. 
    *  The median filter looks at the three most recent samples and reports the 
    *  median value.
    *
    * Parameters:
    *  x1:  Current value.
    *  x2:  Previous value.
    *  x3:  Before previous value.
    *
    * Return:
    *  Returns filtered value.
    *
    *******************************************************************************/
    uint16 `$INSTANCE_NAME`_MedianFilter(uint16 x1, uint16 x2, uint16 x3)
                                         `=ReentrantKeil($INSTANCE_NAME . "_MedianFilter")`
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
#endif /* ( (`$INSTANCE_NAME`_RAW_FILTER_MASK & `$INSTANCE_NAME`_MEDIAN_FILTER) || \
       *    (`$INSTANCE_NAME`_POS_FILTERS_MASK & `$INSTANCE_NAME`_MEDIAN_FILTER) )
       */


#if ( (`$INSTANCE_NAME`_RAW_FILTER_MASK & `$INSTANCE_NAME`_AVERAGING_FILTER) || \
      (`$INSTANCE_NAME`_POS_FILTERS_MASK & `$INSTANCE_NAME`_AVERAGING_FILTER) )
    /*******************************************************************************
    * Function Name: `$INSTANCE_NAME`_AveragingFilter
    ********************************************************************************
    *
    * Summary:
    *  Averaging filter function.
    *  The averaging filter looks at the three most recent samples of position and
    *  reports the averaging value.
    *
    * Parameters:
    *  x1:  Current value.
    *  x2:  Previous value.
    *  x3:  Before previous value.
    *
    * Return:
    *  Returns filtered value.
    *
    *******************************************************************************/
    uint16 `$INSTANCE_NAME`_AveragingFilter(uint16 x1, uint16 x2, uint16 x3)
                                            `=ReentrantKeil($INSTANCE_NAME . "_AveragingFilter")`
    {
        uint32 tmp = ((uint32)x1 + (uint32)x2 + (uint32)x3) / 3u;
        
        return ((uint16) tmp);
    }
#endif /* ( (`$INSTANCE_NAME`_RAW_FILTER_MASK & `$INSTANCE_NAME`_AVERAGING_FILTER) || \
       *    (`$INSTANCE_NAME`_POS_FILTERS_MASK & `$INSTANCE_NAME`_AVERAGING_FILTER) )
       */


#if ( (`$INSTANCE_NAME`_RAW_FILTER_MASK & `$INSTANCE_NAME`_IIR2_FILTER) || \
      (`$INSTANCE_NAME`_POS_FILTERS_MASK & `$INSTANCE_NAME`_IIR2_FILTER) )
    /*******************************************************************************
    * Function Name: `$INSTANCE_NAME`_IIR2Filter
    ********************************************************************************
    *
    * Summary:
    *  IIR1/2 filter function. IIR1/2 = 1/2current + 1/2previous.
    *
    * Parameters:
    *  x1:  Current value.
    *  x2:  Previous value.
    *
    * Return:
    *  Returns filtered value.
    *
    *******************************************************************************/
    uint16 `$INSTANCE_NAME`_IIR2Filter(uint16 x1, uint16 x2)
                                       `=ReentrantKeil($INSTANCE_NAME . "_IIR2Filter")`
    {
        uint32 tmp;
        
        /* IIR = 1/2 Current Value+ 1/2 Previous Value */
        tmp = (uint32)x1 + (uint32)x2;
        tmp >>= 1u;
    
        return ((uint16) tmp);
    }
#endif /* ( (`$INSTANCE_NAME`_RAW_FILTER_MASK & `$INSTANCE_NAME`_IIR2_FILTER) || \
       *    (`$INSTANCE_NAME`_POS_FILTERS_MASK & `$INSTANCE_NAME`_IIR2_FILTER) )
       */


#if ( (`$INSTANCE_NAME`_RAW_FILTER_MASK & `$INSTANCE_NAME`_IIR4_FILTER) || \
      (`$INSTANCE_NAME`_POS_FILTERS_MASK & `$INSTANCE_NAME`_IIR4_FILTER) )
    /*******************************************************************************
    * Function Name: `$INSTANCE_NAME`_IIR4Filter
    ********************************************************************************
    *
    * Summary:
    *  IIR1/4 filter function. IIR1/4 = 1/4current + 3/4previous.
    *
    * Parameters:
    *  x1:  Current value.
    *  x2:  Previous value.
    *
    * Return:
    *  Returns filtered value.
    *
    *******************************************************************************/
    uint16 `$INSTANCE_NAME`_IIR4Filter(uint16 x1, uint16 x2)
                                       `=ReentrantKeil($INSTANCE_NAME . "_IIR4Filter")`
    {
        uint32 tmp;
        
        /* IIR = 1/4 Current Value + 3/4 Previous Value */
        tmp = (uint32)x1 + (uint32)x2;
        tmp += ((uint32)x2 << 1u);
        tmp >>= 2u;
        
        return ((uint16) tmp);
    }
#endif /* ( (`$INSTANCE_NAME`_RAW_FILTER_MASK & `$INSTANCE_NAME`_IIR4_FILTER) || \
       *    (`$INSTANCE_NAME`_POS_FILTERS_MASK & `$INSTANCE_NAME`_IIR4_FILTER) )
       */


#if ( (`$INSTANCE_NAME`_RAW_FILTER_MASK & `$INSTANCE_NAME`_JITTER_FILTER) || \
      (`$INSTANCE_NAME`_POS_FILTERS_MASK & `$INSTANCE_NAME`_JITTER_FILTER) )
    /*******************************************************************************
    * Function Name: uint16 `$INSTANCE_NAME`_JitterFilter
    ********************************************************************************
    *
    * Summary:
    *  Jitter filter function.
    *
    * Parameters:
    *  x1:  Current value.
    *  x2:  Previous value.
    *
    * Return:
    *  Returns filtered value.
    *
    *******************************************************************************/
    uint16 `$INSTANCE_NAME`_JitterFilter(uint16 x1, uint16 x2)
                                         `=ReentrantKeil($INSTANCE_NAME . "_JitterFilter")`
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
#endif /* ( (`$INSTANCE_NAME`_RAW_FILTER_MASK & `$INSTANCE_NAME`_JITTER_FILTER) || \
       *    (`$INSTANCE_NAME`_POS_FILTERS_MASK & `$INSTANCE_NAME`_JITTER_FILTER) )
       */


#if (`$INSTANCE_NAME`_RAW_FILTER_MASK & `$INSTANCE_NAME`_IIR8_FILTER)
    /*******************************************************************************
    * Function Name: `$INSTANCE_NAME`_IIR8Filter
    ********************************************************************************
    *
    * Summary:
    *  IIR1/8 filter function. IIR1/8 = 1/8current + 7/8previous.
    *  Only applies for raw data.
    *
    * Parameters:
    *  x1:  Current value.
    *  x2:  Previous value.
    *
    * Return:
    *  Returns filtered value.
    *
    *******************************************************************************/
    uint16 `$INSTANCE_NAME`_IIR8Filter(uint16 x1, uint16 x2)
                                       `=ReentrantKeil($INSTANCE_NAME . "_IIR8Filter")`
    {
        uint32 tmp;
        
        /* IIR = 1/8 Current Value + 7/8 Previous Value */
        tmp = (uint32)x1;
        tmp += (((uint32)x2 << 3u) - ((uint32)x2));
        tmp >>= 3u;
    
        return ((uint16) tmp);
    }
#endif /* (`$INSTANCE_NAME`_RAW_FILTER_MASK & `$INSTANCE_NAME`_IIR8_FILTER) */


#if (`$INSTANCE_NAME`_RAW_FILTER_MASK & `$INSTANCE_NAME`_IIR16_FILTER)
    /*******************************************************************************
    * Function Name: `$INSTANCE_NAME`_IIR16Filter
    ********************************************************************************
    *
    * Summary:
    *  IIR1/16 filter function. IIR1/16 = 1/16current + 15/16previous.
    *  Only applies for raw data.
    *
    * Parameters:
    *  x1:  Current value.
    *  x2:  Previous value.
    *
    * Return:
    *  Returns filtered value.
    *
    *******************************************************************************/
    uint16 `$INSTANCE_NAME`_IIR16Filter(uint16 x1, uint16 x2)
                                        `=ReentrantKeil($INSTANCE_NAME . "_IIR16Filter")`
    {
        uint32 tmp;
        
        /* IIR = 1/16 Current Value + 15/16 Previous Value */
        tmp = (uint32)x1;
        tmp += (((uint32)x2 << 4u) - ((uint32)x2));
        tmp >>= 4u;
        
        return ((uint16) tmp);
    }
#endif /* (`$INSTANCE_NAME`_RAW_FILTER_MASK & `$INSTANCE_NAME`_IIR16_FILTER) */


#if (`$INSTANCE_NAME`_TOTAL_MATRIX_BUTTONS_COUNT)

    /*******************************************************************************
    * Function Name: `$INSTANCE_NAME`_GetMatrixButtonPos
    ********************************************************************************
    *
    * Summary:
    *  Function calculates and returns touch position (column and row) for matrix
    *  button widget.
    *
    * Parameters:
    *  widget:  widget number;
    *  pos:     pointer to an array of two uint8, where touch postion will be 
    *           stored:
    *           pos[0] - column position;
    *           pos[1] - raw position.
    *
    * Return:
    *  Returns 1 if row and column sensors of matrix button are active, 0 - in other
    *  cases.
    *
    * Reentrant:
    *  No
    *
    *******************************************************************************/
    uint8 `$INSTANCE_NAME`_GetMatrixButtonPos(uint8 widget, uint8* pos)
	                                          `=ReentrantKeil($INSTANCE_NAME . "_GetMatrixButtonPos")`
    {
        uint8 i;
        uint16 row_sig_max = 0u;
        uint16 col_sig_max = 0u;
        uint8 row_ind = 0u;
        uint8 col_ind = 0u;

        if (`$INSTANCE_NAME`_CheckIsWidgetActive(widget))
        {
            /* Find row number with maximal signal value */
            for(i = `$INSTANCE_NAME`_rawDataIndex[widget]; i < `$INSTANCE_NAME`_rawDataIndex[widget] + \
                 `$INSTANCE_NAME`_numberOfSensors[widget]; i++) 
            {          
                if (`$INSTANCE_NAME`_SensorSignal[i] > col_sig_max)
                {
                    col_ind = i;
                    col_sig_max = `$INSTANCE_NAME`_SensorSignal[i];
                }
            }

            /* Find row number with maximal signal value */
            for(i = `$INSTANCE_NAME`_rawDataIndex[widget+1u]; i < `$INSTANCE_NAME`_rawDataIndex[widget+1u] + \
                 `$INSTANCE_NAME`_numberOfSensors[widget+1u]; i++) 
            {          
                if (`$INSTANCE_NAME`_SensorSignal[i] > row_sig_max)
                {
                    row_ind = i;
                    row_sig_max = `$INSTANCE_NAME`_SensorSignal[i];
                }
            }

            if(col_sig_max >= `$INSTANCE_NAME`_fingerThreshold[widget] && \
               row_sig_max >= `$INSTANCE_NAME`_fingerThreshold[widget+1u])
            {
                pos[0u] = col_ind - `$INSTANCE_NAME`_rawDataIndex[widget];
                pos[1u] = row_ind - `$INSTANCE_NAME`_rawDataIndex[widget+1u];
                return 1u;
            }
        }
        return 0u;
    }

#endif /* (`$INSTANCE_NAME`_TOTAL_MATRIX_BUTTONS_COUNT) */

/* [] END OF FILE */
