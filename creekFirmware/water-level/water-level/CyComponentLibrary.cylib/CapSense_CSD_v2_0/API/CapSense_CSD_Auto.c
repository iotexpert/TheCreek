/*******************************************************************************
* File Name: `$INSTANCE_NAME`_Auto.c
* Version `$CY_MAJOR_VERSION`.`$CY_MINOR_VERSION`
*
* Description:
*  This file provides the source code of CapSense autotuning APIs for the
*  CapSense CSD Component.
*
* Note:
*
********************************************************************************
* Copyright 2008-2010, Cypress Semiconductor Corporation.  All rights reserved.
* You may use this file only in accordance with the license, terms, conditions,
* disclaimers, and limitations in the end user license agreement accompanying
* the software package with which this file was provided.
*******************************************************************************/

#include "`$INSTANCE_NAME`_Auto.h"

#if (`$INSTANCE_NAME`_TUNNING_METHOD == `$INSTANCE_NAME`_AUTO_TUNNING)


#error Smart-Sense is not supported by the CapSense CSD v2.0. You should use CapSense CSD v2.10.

/*******************************************************************************
* Function Name: `$INSTANCE_NAME`_AutoTune
********************************************************************************
*
* Summary:
*  Provides tunning procedure for all sensors.
*
* Parameters:
*  None.
*
* Return:
*  None.
*
*******************************************************************************/
void `$INSTANCE_NAME`_AutoTune()
{
}

/*******************************************************************************
* Function Name: `$INSTANCE_NAME`_CalculateThresholds
********************************************************************************
*
* Summary:
*    Computes the noise and finger thresholds based on the high frequency noise
*    found in Noise_Calc and the amplitude of the signal from a recent finger
*    press.
*
* Parameters:
*    SensorNumber: Sensor number.
*
* Return:
*    None.
*
*******************************************************************************/
void `$INSTANCE_NAME`_CalculateThresholds(uint8 SensorNumber)
{
}


#endif /* End (`$INSTANCE_NAME`_TUNNING_METHOD == `$INSTANCE_NAME`_AUTO_TUNNING) */


/* [] END OF FILE */