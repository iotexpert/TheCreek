/*******************************************************************************
* File Name: `$INSTANCE_NAME`_SMS_Wrapper.c
* Version `$CY_MAJOR_VERSION`.`$CY_MINOR_VERSION`
*
* Description:
*  This file provides the source code of wrapper between CapSense CSD component 
*  and Auto Tuning library.
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
#include "`$INSTANCE_NAME`_CSHL.h"

#if (`$INSTANCE_NAME`_TUNING_METHOD == `$INSTANCE_NAME`_AUTO_TUNING)

extern uint8 `$INSTANCE_NAME`_noiseThreshold[];
extern uint8 `$INSTANCE_NAME`_hysteresis[];

extern uint8 `$INSTANCE_NAME`_widgetResolution[];

extern const uint8 CYCODE `$INSTANCE_NAME`_widgetNumber[];
extern const uint8 CYCODE `$INSTANCE_NAME`_numberOfSensors[];

extern uint8 `$INSTANCE_NAME`_fingerThreshold[];
extern uint8 `$INSTANCE_NAME`_idacSettings[];

extern uint16 `$INSTANCE_NAME`_SensorRaw[];
extern uint16 `$INSTANCE_NAME`_SensorBaseline[];
extern uint8  `$INSTANCE_NAME`_SensorSignal[];

extern void  `$INSTANCE_NAME`_SetPrescaler(uint8);

extern void SMS_LIB_CalculateThresholds(uint8 SensorNumber);
extern void SMS_LIB_AutoTune1Ch(void);
extern void SMS_LIB_AutoTune2Ch(void);

uint8 * SMS_LIB_noiseThreshold = `$INSTANCE_NAME`_noiseThreshold;
uint8 * SMS_LIB_hysteresis = `$INSTANCE_NAME`_hysteresis;

uint8 * SMS_LIB_widgetResolution = `$INSTANCE_NAME`_widgetResolution;

const uint8 CYCODE * SMS_LIB_widgetNumber = `$INSTANCE_NAME`_widgetNumber;
const uint8 CYCODE * SMS_LIB_numberOfSensors = `$INSTANCE_NAME`_numberOfSensors;

uint8 * SMS_LIB_fingerThreshold = `$INSTANCE_NAME`_fingerThreshold;
uint8 * SMS_LIB_idacSettings = `$INSTANCE_NAME`_idacSettings;

uint16 * SMS_LIB_SensorRaw = `$INSTANCE_NAME`_SensorRaw;
uint16 * SMS_LIB_SensorBaseline = `$INSTANCE_NAME`_SensorBaseline;

`$writerCAutoSensitivity`
`$writerCAutoPrescalerTbl`

uint8 SMS_LIB_Table1[`$INSTANCE_NAME`_TOTAL_SENSOR_COUNT];
uint8 SMS_LIB_Table2[`$INSTANCE_NAME`_TOTAL_SENSOR_COUNT];
uint8 SMS_LIB_Table3[`$INSTANCE_NAME`_TOTAL_SENSOR_COUNT];
uint16 SMS_LIB_Table4[`$INSTANCE_NAME`_TOTAL_SENSOR_COUNT];
uint16 SMS_LIB_Table5[`$INSTANCE_NAME`_TOTAL_SENSOR_COUNT];
uint8 SMS_LIB_Table6[`$INSTANCE_NAME`_TOTAL_SENSOR_COUNT];
uint8 SMS_LIB_Table7[`$INSTANCE_NAME`_TOTAL_SENSOR_COUNT];

uint8 SMS_LIB_Table8[`$INSTANCE_NAME`_END_OF_WIDGETS_INDEX];
uint8 SMS_LIB_Table9[`$INSTANCE_NAME`_END_OF_WIDGETS_INDEX];
uint8 SMS_LIB_Table10[`$INSTANCE_NAME`_END_OF_WIDGETS_INDEX];

uint8 SMS_LIB_Var1 = (`$writerCAutoInitialPrescaler`);
uint16 SMS_LIB_Var2 = (`$writerCAutoKCalcValue`);

uint8 SMS_LIB_TotalSnsCnt = `$INSTANCE_NAME`_TOTAL_SENSOR_COUNT;
uint8 SMS_LIB_TotalScanSlCnt = `$INSTANCE_NAME`_TOTAL_SCANSLOT_COUNT;
uint8 SMS_LIB_EndOfWidgInd = `$INSTANCE_NAME`_END_OF_WIDGETS_INDEX;

#if (`$INSTANCE_NAME`_DESIGN_TYPE == `$INSTANCE_NAME`_TWO_CHANNELS_DESIGN)
    uint8 SMS_LIB_TotalSnsCnt_CH0 = `$INSTANCE_NAME`_TOTAL_SENSOR_COUNT__CH0;
    uint8 SMS_LIB_TotalSnsCnt_CH1 = `$INSTANCE_NAME`_TOTAL_SENSOR_COUNT__CH1;
#else
    uint8 SMS_LIB_TotalSnsCnt_CH0 = 0;
    uint8 SMS_LIB_TotalSnsCnt_CH1 = 0;
#endif  /* End (`$INSTANCE_NAME`_DESIGN_TYPE == `$INSTANCE_NAME`_TWO_CHANNELS_DESIGN) */

/*This code removes conflicts with AutoSense_v3_0 lib*/
uint8 * SMS_LIB_prescaler;
void SMS_LIB_V3_SetAnalogSwitchesSrc_Prescaler(void) {}
void SMS_LIB_V3_SetAnalogSwitchesSrc_PRS(void){}
/*End of patch*/


/*******************************************************************************
* Function Name: SMS_LIB_ScanSensor
********************************************************************************
*
* Summary:
*  Wrapper to `$INSTANCE_NAME`_ScanSensor function.
*
* Parameters:
*  SensorNumber:  Sensor number.
*
* Return:
*  None
*
* Reentrant:
*  No
*
*******************************************************************************/
void SMS_LIB_ScanSensor(uint8 SensorNumber)
{
    `$INSTANCE_NAME`_ScanSensor(SensorNumber);
}


/*******************************************************************************
* Function Name: SMS_LIB_IsBusy
********************************************************************************
*
* Summary:
*  Wrapper to `$INSTANCE_NAME`_IsBusy function.
*  
* Parameters:
*  None
*
* Return:
*  None
*
*******************************************************************************/
uint8 SMS_LIB_IsBusy(void)
{
    return `$INSTANCE_NAME`_IsBusy();
}


/*******************************************************************************
* Function Name: SMS_LIB_SetPrescaler
********************************************************************************
*
* Summary:
*  Wrapper to `$INSTANCE_NAME`_SetPrescaler function.
*
* Parameters:
*  Prescaler:  Prascaler value.
*
* Return:
*  None
*
*******************************************************************************/
void SMS_LIB_SetPrescaler(uint8 Prescaler)
{
    `$INSTANCE_NAME`_SetPrescaler(Prescaler);
}


/*******************************************************************************
* Function Name: `$INSTANCE_NAME`_CalculateThresholds
********************************************************************************
*
* Summary:
*  Wrapper to SMS_LIB_CalculateThresholds function.
*
* Parameters:
*  SensorNumber:  Sensor number.
*
* Return:
*  None
*
* Reentrant:
*  No
*
*******************************************************************************/
void `$INSTANCE_NAME`_CalculateThresholds(uint8 SensorNumber)
{
    SMS_LIB_CalculateThresholds(SensorNumber);
}


/*******************************************************************************
* Function Name: `$INSTANCE_NAME`_AutoTune
********************************************************************************
*
* Summary:
*  Wrapper for SMS_LIB_AutoTune1Ch or SMS_LIB_AutoTune2Ch function.
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
void `$INSTANCE_NAME`_AutoTune(void)
{
    #if (`$INSTANCE_NAME`_DESIGN_TYPE == `$INSTANCE_NAME`_ONE_CHANNEL_DESIGN)
        SMS_LIB_AutoTune1Ch();
    #elif (`$INSTANCE_NAME`_DESIGN_TYPE == `$INSTANCE_NAME`_TWO_CHANNELS_DESIGN)
        SMS_LIB_AutoTune2Ch();
    #endif /* End (`$INSTANCE_NAME`_DESIGN_TYPE == `$INSTANCE_NAME`_ONE_CHANNEL_DESIGN) */
}
#endif  /* End (`$INSTANCE_NAME`_TUNING_METHOD == `$INSTANCE_NAME`_AUTO_TUNING) */


/* [] END OF FILE */
