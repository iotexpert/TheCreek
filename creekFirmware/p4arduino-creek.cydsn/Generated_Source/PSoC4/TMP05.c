/*******************************************************************************
* File Name: TMP05.c
* Version 1.10
*
* Description:
*  This file provides all API functionality of the TMP05 component.
*
* Note:
*  None
*
********************************************************************************
* Copyright 2012-2013, Cypress Semiconductor Corporation.  All rights reserved.
* You may use this file only in accordance with the license, terms, conditions,
* disclaimers, and limitations in the end user license agreement accompanying
* the software package with which this file was provided.
*******************************************************************************/

#include "TMP05_PVT.h"


/*******************************************************************************
* Variables
********************************************************************************/
uint8  TMP05_initVar = 0u;
volatile uint16 TMP05_lo[TMP05_CUSTOM_NUM_SENSORS];
volatile uint16 TMP05_hi[TMP05_CUSTOM_NUM_SENSORS];
volatile uint8  TMP05_busyFlag;
volatile uint8  TMP05_contMode;


/*******************************************************************************
* Function Name: TMP05_Start
********************************************************************************
*
* Summary:
* Starts the component. Calls the TMP05_Init() API if the component
* has not been initialized before. Calls the enable API.
*
* Parameters:
*  None
*
* Return:
*  None
*
* Global Variables:
*  TMP05_initVar - used to check initial configuration, modified on
*  first function call.
*
*******************************************************************************/
void TMP05_Start(void) 
{
    /* If not already initialized, then initialize hardware and software */
    if(0u == TMP05_initVar)
    {
        TMP05_Init();
        TMP05_initVar = 1u;
    }
    TMP05_Enable();
}


/*******************************************************************************
* Function Name: TMP05_Stop
********************************************************************************
*
* Summary:
*  Disables and stops the component.
*
* Parameters:
*  None
*
* Return:
*  None
*
* Global Variables:
*  TMP05_busyFlag - used for reflect sensor masuring.
*
***************u****************************************************************/
void TMP05_Stop(void) 
{
    uint8 enableInterrupts;

    /* Change shared regs, need to be safety */
    enableInterrupts = CyEnterCriticalSection();

    TMP05_CONTROL_REG = (TMP05_CONTROL_REG & TMP05_CTRL_EOC_TRIG);
 
    TMP05_EOC_ISR_Disable();

    TMP05_busyFlag = 0u;

    /* shared regs config are done */
    CyExitCriticalSection(enableInterrupts);
}

/*******************************************************************************
* Function Name: TMP05_Init()
********************************************************************************
*
* Summary:
*  Initializes the component.
*
* Parameters:
*  None
*
* Return:
*  None
*
* Side Effects:
*  None
*
* Global Variables:
*  TMP05_busyFlag - used for reflect sensor masuring.
*  TMP05_contMode - used for reflect modes of operation used:
*     - TMP05_MODE_CONTINUOUS.
*     - TMP05_MODE_ONESHOT.
*
********************************************************************************/
void TMP05_Init(void) 
{
    TMP05_contMode = TMP05_CUSTOM_CONTINUOUS_MODE;
    TMP05_busyFlag = 0u;
}


/*******************************************************************************
* Function Name: TMP05_Enable()
********************************************************************************
*
* Summary:
*  Enables the component.
*
* Parameters:
*  None
*
* Return:
*  None
*
* Side Effects:
*  None
*
********************************************************************************/
void TMP05_Enable(void) 
{
    uint8 enableInterrupts;

    /* Change shared regs, need to be safety */
    enableInterrupts = CyEnterCriticalSection();

    /* Setup the number of Sensors from the customizer */
    TMP05_CONTROL_REG = ((TMP05_CONTROL_REG & TMP05_CTRL_EOC_TRIG) | 
                                   ((uint8)((TMP05_CUSTOM_NUM_SENSORS - 1u)  << 
                                             TMP05_CTRL_REG_SNS_SHIFT))  |
                                             TMP05_CTRL_REG_ENABLE);

    /* Reset Timer FIFOs */
    CY_SET_REG16(TMP05_FIFO_AUXCTL_PTR, (CY_GET_REG16(TMP05_FIFO_AUXCTL_PTR) |
                                                                 TMP05_FIFO_CLEAR_MASK));
    CY_SET_REG16(TMP05_FIFO_AUXCTL_PTR, (CY_GET_REG16(TMP05_FIFO_AUXCTL_PTR) &
                                                                 (uint16)~TMP05_FIFO_CLEAR_MASK));

    /* shared regs config are done */
    CyExitCriticalSection(enableInterrupts);

    /* Enable the buried ISR component */
    TMP05_EOC_ISR_StartEx(&TMP05_EOC_ISR_Int);
}


/*******************************************************************************
* Function Name: TMP05_Trigger
********************************************************************************
*
* Summary:
*  Provides a valid strobe/trigger output on the conv terminal.
*
* Parameters:
*  None
*
* Return:
*  None
*
* Global Variables:
*  TMP05_busyFlag - used for reflect sensor masuring.
*
*******************************************************************************/
void TMP05_Trigger(void) 
{
    uint8 enableInterrupts;

    if( 0u == TMP05_busyFlag)
    {
        /* Change shared regs, need to be safety */
        enableInterrupts = CyEnterCriticalSection();

        /* Generate a CONV strobe */
        TMP05_CONTROL_REG ^= TMP05_CTRL_TRIG;

        TMP05_busyFlag = 1u;

        /* shared regs config are done */
        CyExitCriticalSection(enableInterrupts);
    }
    else
    {
        /* Do nothing */
    }

}


/*******************************************************************************
* Function Name: TMP05_GetTemperature
********************************************************************************
*
* Summary:
*  Calculates the temperature in degrees Celsius.
*
* Parameters:
*  uint8 SensorNum. The TMP05 sensor number from 0..3.
*
* Return:
*  int16 Temperature in 1/100ths degrees C of the requested sensor.
*
*******************************************************************************/
int16 TMP05_GetTemperature(uint8 sensorNum) 
{
    uint16 hi_temp;
    uint16 lo_temp;
    
    TMP05_EOC_ISR_Disable();
    hi_temp = TMP05_hi[sensorNum];
    lo_temp = TMP05_lo[sensorNum];
    TMP05_EOC_ISR_Enable();
    
    /* Calculates temp for each sensor based on mathematical equation shown in TMP05 datasheet */
    return ((int16)((TMP05_SCALED_CONST_TMP1 - 
		   ((TMP05_SCALED_CONST_TMP2 * (int32) hi_temp) / (int32) lo_temp))));
}


/*******************************************************************************
* Function Name: TMP05_ConversionStatus
********************************************************************************
*
* Summary:
*  Enables firmware to synchronize with the hardware.
*
* Parameters:
*  None
*
* Return:
*  uint8 status code:
*   TMP05_STATUS_IN_PROGRESS - Conversion in progress.
*   TMP05_STATUS_COMPLETE - Conversion complete.
*   TMP05_STATUS_ERROR - Sensor Error.
*
*******************************************************************************/
uint8 TMP05_ConversionStatus(void) 
{
    return (TMP05_STATUS_REG & TMP05_STATUS_CLR_MASK);
}


/*******************************************************************************
* Function Name: TMP05_SetMode
********************************************************************************
*
* Summary:
*  Sets the operating mode of the component.
*
* Parameters:
*  uint8 mode: operating mode:
*   TMP05_MODE_CONTINUOUS - Continuous mode.
*   TMP05_MODE_ONESHOT - One-shot mode.
*
* Return:
*  None
*
* Global Variables:
*  TMP05_contMode - used for reflect modes of operation used:
*     - TMP05_MODE_CONTINUOUS.
*     - TMP05_MODE_ONESHOT.
*
*******************************************************************************/
void TMP05_SetMode(uint8 mode) 
{
   TMP05_contMode = mode;
}


/*******************************************************************************
* Function Name: TMP05_DiscoverSensors
********************************************************************************
*
* Summary:
*  This API is provided for applications that might have a variable number of
*  temperature sensors connected. It automatically detects how many temperature
*  sensors are daisy-chained to the component. The algorithm starts by checking
*  to see if the number of sensors actually connected matches the NumSensors
*  parameter setting in the Basic Tab of the component customizer. If not,
*  it will retry assuming 1 less sensor is connected. This process will repeat
*  until the actual number of sensors connected is known.
*  Confirming whether or not a sensor is attached or not takes a few hundred
*  milliseconds per sensor per iteration of the algorithm. To limit the
*  sensing time, reduce the NumSensors parameter setting in the General Tab
*  of the component customizer to the maximum number of possible sensors
*  in the system.
*
* Parameters:
*  None
*
* Return:
*  uint8 representing the number of sensors actually connected (0..4).
*
* Global Variables:
*  TMP05_contMode - used for reflect modes of operation used:
*     - TMP05_MODE_CONTINUOUS.
*     - TMP05_MODE_ONESHOT.
*
*******************************************************************************/
uint8 TMP05_DiscoverSensors(void) 
{
    uint8 sensorCount = TMP05_CUSTOM_NUM_SENSORS;
    uint8 enableInterrupts;
    uint8 eocRegStatus;
    uint8 contFlag;

    if (0u != (TMP05_MODE_CONTINUOUS & TMP05_contMode))
    {
        TMP05_contMode = TMP05_MODE_ONESHOT;
        contFlag = 1u;
    }
    else
    {
        contFlag = 0u;
    }

    /* Change shared regs, need to be safety */
    enableInterrupts = CyEnterCriticalSection();

    /* Setup the number of Sensors from the customizer */
    TMP05_CONTROL_REG = ((TMP05_CONTROL_REG & TMP05_CTRL_NUM_SNS_MASK) | 
                                   (uint8)((TMP05_CUSTOM_NUM_SENSORS - 1u) << 
                                            TMP05_CTRL_REG_SNS_SHIFT));
                                     
    /* shared regs config are done */
    CyExitCriticalSection(enableInterrupts);

    /* Start conversion */
    TMP05_Trigger();

    /* Wait for conversion complete or error */
    do
    {
        eocRegStatus = TMP05_ConversionStatus();
    }
    while (eocRegStatus == TMP05_STATUS_IN_PROGRESS);

    /* Error returned, re-try with 1 less sensor */
    if (0u != (eocRegStatus & TMP05_STATUS_ERR))
    {
        do
        {
            sensorCount--;

            if (0u == sensorCount)
            {
                eocRegStatus = 0u;
            }
            else
            {
                /* Change shared regs, need to be safety */
                enableInterrupts = CyEnterCriticalSection();

                TMP05_CONTROL_REG = ((TMP05_CONTROL_REG & TMP05_CTRL_NUM_SNS_MASK) | 
                                               (uint8)((sensorCount - 1u) << TMP05_CTRL_REG_SNS_SHIFT));

                /* shared regs config are done */
                CyExitCriticalSection(enableInterrupts);

                 /* Start conversion */
                TMP05_Trigger();

                /* Wait for conversion complete or error */
                do
                {
                    eocRegStatus = TMP05_ConversionStatus();
                }
                while (eocRegStatus == TMP05_STATUS_IN_PROGRESS);
            }
        }
        while (0u != (eocRegStatus & TMP05_STATUS_ERR));
    }

    if (0u != contFlag)
    {
        TMP05_contMode = TMP05_MODE_CONTINUOUS;
    }

    return (sensorCount);
}


/* [] END OF FILE */
