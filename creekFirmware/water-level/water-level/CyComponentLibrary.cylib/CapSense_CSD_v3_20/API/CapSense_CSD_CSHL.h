/*******************************************************************************
* File Name: `$INSTANCE_NAME`_CSHL.h
* Version `$CY_MAJOR_VERSION`.`$CY_MINOR_VERSION`
*
* Description:
*  This file provides constants and parameter values for the High Level APIs
*  for CapSense CSD component.
*
* Note:
*
********************************************************************************
* Copyright 2008-2012, Cypress Semiconductor Corporation.  All rights reserved.
* You may use this file only in accordance with the license, terms, conditions,
* disclaimers, and limitations in the end user license agreement accompanying
* the software package with which this file was provided.
*******************************************************************************/

#if !defined(CY_CAPSENSE_CSD_CSHL_`$INSTANCE_NAME`_H)
#define CY_CAPSENSE_CSD_CSHL_`$INSTANCE_NAME`_H

#include "`$INSTANCE_NAME`.h"


/***************************************
*   Condition compilation parameters
***************************************/

#define `$INSTANCE_NAME`_SIGNAL_SIZE                (`$WidgetResolution`u)
#define `$INSTANCE_NAME`_AUTO_RESET                 (`$SensorAutoReset`u)
#define `$INSTANCE_NAME`_RAW_FILTER_MASK            (`$RawDataFilterType`u)

/* Signal size definition */
#define `$INSTANCE_NAME`_SIGNAL_SIZE_UINT8          (8u)
#define `$INSTANCE_NAME`_SIGNAL_SIZE_UINT16         (16u)

/* Auto reset definition */
#define `$INSTANCE_NAME`_AUTO_RESET_DISABLE         (0u)
#define `$INSTANCE_NAME`_AUTO_RESET_ENABLE          (1u)

/* Mask for RAW and POS filters */
#define `$INSTANCE_NAME`_MEDIAN_FILTER              (0x01u)
#define `$INSTANCE_NAME`_AVERAGING_FILTER           (0x02u)
#define `$INSTANCE_NAME`_IIR2_FILTER                (0x04u)
#define `$INSTANCE_NAME`_IIR4_FILTER                (0x08u)
#define `$INSTANCE_NAME`_JITTER_FILTER              (0x10u)
#define `$INSTANCE_NAME`_IIR8_FILTER                (0x20u)
#define `$INSTANCE_NAME`_IIR16_FILTER               (0x40u)


/***************************************
*           API Constants
***************************************/

/* Widgets constants definition */
`$writerCSHLHFile`

#define `$INSTANCE_NAME`_TOTAL_SLIDERS_COUNT            ( `$INSTANCE_NAME`_TOTAL_LINEAR_SLIDERS_COUNT + \
                                                          `$INSTANCE_NAME`_TOTAL_RADIAL_SLIDERS_COUNT )
                                                          
#define `$INSTANCE_NAME`_TOTAL_CENTROIDS_COUNT          ( `$INSTANCE_NAME`_TOTAL_SLIDERS_COUNT + \
                                                         (`$INSTANCE_NAME`_TOTAL_TOUCH_PADS_COUNT*2u) )

#define `$INSTANCE_NAME`_TOTAL_WIDGET_COUNT             ( `$INSTANCE_NAME`_TOTAL_LINEAR_SLIDERS_COUNT + \
                                                          `$INSTANCE_NAME`_TOTAL_RADIAL_SLIDERS_COUNT + \
                                                          `$INSTANCE_NAME`_TOTAL_TOUCH_PADS_COUNT + \
                                                          `$INSTANCE_NAME`_TOTAL_BUTTONS_COUNT + \
                                                          `$INSTANCE_NAME`_TOTAL_MATRIX_BUTTONS_COUNT )
                                                           
#define `$INSTANCE_NAME`_ANY_POS_FILTER                 ( `$INSTANCE_NAME`_MEDIAN_FILTER | \
                                                          `$INSTANCE_NAME`_AVERAGING_FILTER | \
                                                          `$INSTANCE_NAME`_IIR2_FILTER | \
                                                          `$INSTANCE_NAME`_IIR4_FILTER | \
                                                          `$INSTANCE_NAME`_JITTER_FILTER )
                                                         
#define `$INSTANCE_NAME`_IS_DIPLEX_SLIDER               ( `$INSTANCE_NAME`_TOTAL_DIPLEXED_SLIDERS_COUNT > 0u)

#define `$INSTANCE_NAME`_IS_NON_DIPLEX_SLIDER           ( (`$INSTANCE_NAME`_TOTAL_LINEAR_SLIDERS_COUNT - \
                                                           `$INSTANCE_NAME`_TOTAL_DIPLEXED_SLIDERS_COUNT) > 0u)
#define `$INSTANCE_NAME`_ADD_SLIDER_TYPE                ((`$INSTANCE_NAME`_TOTAL_RADIAL_SLIDERS_COUNT > 0u) ? \
                                                        ((`$INSTANCE_NAME`_TOTAL_TOUCH_PADS_COUNT > 0u) || \
                                                         (`$INSTANCE_NAME`_TOTAL_LINEAR_SLIDERS_COUNT > 0u)) : 0u)

/*Types of centroids */
#define `$INSTANCE_NAME`_TYPE_RADIAL_SLIDER         (0x01u)
#define `$INSTANCE_NAME`_TYPE_LINEAR_SLIDER         (0x02u)
#define `$INSTANCE_NAME`_TYPE_GENERIC               (0xFFu)

/* Defines is slot active */
#define `$INSTANCE_NAME`_SENSOR_1_IS_ACTIVE     (0x01u)
#define `$INSTANCE_NAME`_SENSOR_2_IS_ACTIVE     (0x02u)
#define `$INSTANCE_NAME`_WIDGET_IS_ACTIVE       (0x01u)

/* Defines diplex type of Slider */
#define `$INSTANCE_NAME`_IS_DIPLEX              (0x80u)

/* Defines max fingers on TouchPad  */
#define `$INSTANCE_NAME`_POS_PREV               (0u)
#define `$INSTANCE_NAME`_POS                    (1u)
#define `$INSTANCE_NAME`_POS_NEXT               (2u)
#define `$INSTANCE_NAME`_CENTROID_ROUND_VALUE   (0x7F00u)

#define `$INSTANCE_NAME`_NEGATIVE_NOISE_THRESHOLD        (`$NegativeNoiseThreshold`u)
#define `$INSTANCE_NAME`_LOW_BASELINE_RESET              (`$LowBaselineReset`u)


/***************************************
*        Function Prototypes
***************************************/

void `$INSTANCE_NAME`_InitializeSensorBaseline(uint8 sensor) \
                                               `=ReentrantKeil($INSTANCE_NAME . "_InitializeSensorBaseline")`;
void `$INSTANCE_NAME`_InitializeAllBaselines(void) `=ReentrantKeil($INSTANCE_NAME . "_InitializeAllBaselines")`;
void `$INSTANCE_NAME`_InitializeEnabledBaselines(void) \
                                                 `=ReentrantKeil($INSTANCE_NAME . "_InitializeEnabledBaselines")`;
void `$INSTANCE_NAME`_UpdateSensorBaseline(uint8 sensor) `=ReentrantKeil($INSTANCE_NAME . "_UpdateSensorBaseline")`;
void `$INSTANCE_NAME`_UpdateEnabledBaselines(void) `=ReentrantKeil($INSTANCE_NAME . "_UpdateEnabledBaselines")`;
uint8 `$INSTANCE_NAME`_CheckIsSensorActive(uint8 sensor) `=ReentrantKeil($INSTANCE_NAME . "_CheckIsSensorActive")`;
uint8 `$INSTANCE_NAME`_CheckIsWidgetActive(uint8 widget) `=ReentrantKeil($INSTANCE_NAME . "_CheckIsWidgetActive")`;
uint8 `$INSTANCE_NAME`_CheckIsAnyWidgetActive(void) `=ReentrantKeil($INSTANCE_NAME . "_CheckIsAnyWidgetActive")`;
void `$INSTANCE_NAME`_EnableWidget(uint8 widget) `=ReentrantKeil($INSTANCE_NAME . "_EnableWidget")`;
void `$INSTANCE_NAME`_DisableWidget(uint8 widget) `=ReentrantKeil($INSTANCE_NAME . "_DisableWidget")`;
#if (`$INSTANCE_NAME`_TOTAL_MATRIX_BUTTONS_COUNT)
    uint8 `$INSTANCE_NAME`_GetMatrixButtonPos(uint8 widget, uint8* pos) \
	                                          `=ReentrantKeil($INSTANCE_NAME . "_GetMatrixButtonPos")`;
#endif /* (`$INSTANCE_NAME`_TOTAL_MATRIX_BUTTONS_COUNT) */

#if (`$INSTANCE_NAME`_TOTAL_LINEAR_SLIDERS_COUNT)
    uint16 `$INSTANCE_NAME`_GetCentroidPos(uint8 widget) `=ReentrantKeil($INSTANCE_NAME . "_GetCentroidPos")`;
#endif /* (`$INSTANCE_NAME`_TOTAL_LINEAR_SLIDERS_COUNT) */
#if (`$INSTANCE_NAME`_TOTAL_RADIAL_SLIDERS_COUNT)
    uint16 `$INSTANCE_NAME`_GetRadialCentroidPos(uint8 widget) \
                                                 `=ReentrantKeil($INSTANCE_NAME . "_GetRadialCentroidPos")`;
#endif /* (`$INSTANCE_NAME`_TOTAL_RADIAL_SLIDERS_COUNT) */
#if (`$INSTANCE_NAME`_TOTAL_TOUCH_PADS_COUNT)
    uint8 `$INSTANCE_NAME`_GetTouchCentroidPos(uint8 widget, uint16* pos) \
	                                           `=ReentrantKeil($INSTANCE_NAME . "_GetTouchCentroidPos")`;
#endif /* (`$INSTANCE_NAME`_TOTAL_TOUCH_PADS_COUNT) */

#endif /* CY_CAPSENSE_CSD_CSHL_`$INSTANCE_NAME`_H */

/* [] END OF FILE */
