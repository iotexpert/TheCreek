/*******************************************************************************
* File Name: `$INSTANCE_NAME`.c
* Version `$CY_MAJOR_VERSION`.`$CY_MINOR_VERSION`
*
* Description:
*  This file provides the source code of scanning APIs for the Resistive Touch 
*  component.
*
* Note:
*
********************************************************************************
* Copyright 2012, Cypress Semiconductor Corporation.  All rights reserved.
* You may use this file only in accordance with the license, terms, conditions,
* disclaimers, and limitations in the end user license agreement accompanying
* the software package with which this file was provided.
*******************************************************************************/

#include "`$INSTANCE_NAME`.h"

uint8 `$INSTANCE_NAME`_initVar = 0u;
uint8 `$INSTANCE_NAME`_measureVar = 0u;
volatile uint8 `$INSTANCE_NAME`_enableVar = 0u;


/*******************************************************************************
* Function Name: `$INSTANCE_NAME`_Init
********************************************************************************
*
* Summary:
*  Calls the Init functions of the DelSig ADC or SAR ADC and AMux components.
*
* Parameters:
*  None
*
* Return:
*  None
*
*******************************************************************************/
void `$INSTANCE_NAME`_Init(void) `=ReentrantKeil($INSTANCE_NAME . "_Init")`
{
    /* Amux component initialization */
    `$INSTANCE_NAME`_AMux_Init();
    
    /* ADC component initialization */
    #if(`$INSTANCE_NAME`_SAR_SELECT == `$INSTANCE_NAME`_SAR)
        `$INSTANCE_NAME`_ADC_SAR_Init();
    #else
        `$INSTANCE_NAME`_ADC_Init();
    #endif  /* (`$INSTANCE_NAME`_SAR_SELECT) */
}


/*******************************************************************************
* Function Name: `$INSTANCE_NAME`_Start
********************************************************************************
*
* Summary:
*  Calls ResistiveTouct_Init() and ResistiveTouch_Enable() APIs.
*
* Parameters:
*  None
*
* Return:
*  None
* 
* Global variables:
*  `$INSTANCE_NAME`_initVar - used to indicate enable/disable component state.
*
*******************************************************************************/
void `$INSTANCE_NAME`_Start(void)  `=ReentrantKeil($INSTANCE_NAME . "_Start")`
{
    /* Call Init function */
    if(`$INSTANCE_NAME`_initVar == 0u)
    {    
        /* Call Init function */
        `$INSTANCE_NAME`_Init();
        `$INSTANCE_NAME`_initVar = 1u;
    }

    /* Call Enable function */
   `$INSTANCE_NAME`_Enable();
   
}


/*******************************************************************************
* Function Name: `$INSTANCE_NAME`_Stop
********************************************************************************
*
* Summary:
*  Stops the DelSig ADC or SAR ADC and stops the AMux component.
*
* Parameters:
*  None
*
* Return:
*  None
* 
* Global variables:
*  `$INSTANCE_NAME`_enableVar - used to check initial configuration, modified on 
*  first function call.
*
*******************************************************************************/
void `$INSTANCE_NAME`_Stop(void)  `=ReentrantKeil($INSTANCE_NAME . "_Stop")`
{
    /* Stop ADC component */
    #if(`$INSTANCE_NAME`_SAR_SELECT == `$INSTANCE_NAME`_SAR) 
        `$INSTANCE_NAME`_ADC_SAR_Stop();
    
    #else
        `$INSTANCE_NAME`_ADC_Stop();
    
    #endif  /* (`$INSTANCE_NAME`_SAR_SELECT) */
    
    /* Stop AMux component */
    `$INSTANCE_NAME`_AMux_Stop();

    /* Clear variable Enable */
    `$INSTANCE_NAME`_enableVar = 0u;
}


/*******************************************************************************
* Function Name: `$INSTANCE_NAME`_Enable
********************************************************************************
*
* Summary:
*  Enables the DelSig ADC or SAR ADC and enables the AMux component.
*
* Parameters:
*  None
*
* Return:
*  None
* 
* 
* Global variables:
*  `$INSTANCE_NAME`_initVar - used to indicate enable/disable component state.
*
*******************************************************************************/
void `$INSTANCE_NAME`_Enable(void)  `=ReentrantKeil($INSTANCE_NAME . "_Enable")`
{
    /* Enable ADC component */
    #if(`$INSTANCE_NAME`_SAR_SELECT == `$INSTANCE_NAME`_SAR) 
        `$INSTANCE_NAME`_ADC_SAR_Enable();

    #else
        `$INSTANCE_NAME`_ADC_Enable();

    #endif  /* (`$INSTANCE_NAME`_SAR_SELECT) */
    
    /* Set variable Enable */
   `$INSTANCE_NAME`_enableVar = 1u;
}


/*******************************************************************************
* Function Name: `$INSTANCE_NAME`_ActivateX
********************************************************************************
*
* Summary: 
*  Configures the pins for measurement of Y-axis.
*
* Parameters:
*  None
*
* Return:
*  None
*
*******************************************************************************/
void `$INSTANCE_NAME`_ActivateX(void) `=ReentrantKeil($INSTANCE_NAME . "_ActivateX")`
{
    /* Pins configuration for Y measure */
    CyPins_SetPinDriveMode(`$INSTANCE_NAME`_ym_0, PIN_DM_STRONG);
    CyPins_SetPinDriveMode(`$INSTANCE_NAME`_yp_0, PIN_DM_STRONG);
    CyPins_SetPinDriveMode(`$INSTANCE_NAME`_xp_0, PIN_DM_ALG_HIZ);
    CyPins_SetPinDriveMode(`$INSTANCE_NAME`_xm_0, PIN_DM_ALG_HIZ);
    
    /* Switch AMux if Channel 0 not selected */
    if(`$INSTANCE_NAME`_AMux_GetChannel() != `$INSTANCE_NAME`_AMUX_XP_CHAN)
    {
        `$INSTANCE_NAME`_AMux_Next();
    }
}


/*******************************************************************************
* Function Name: `$INSTANCE_NAME`_ActivateY
********************************************************************************
*
* Summary: 
*  Configures the pins for measurement of X-axis.
*
* Parameters:
*  None
*
* Return:
*  None
*
*******************************************************************************/
void `$INSTANCE_NAME`_ActivateY(void) `=ReentrantKeil($INSTANCE_NAME . "_ActivateY")`
{
    /* Pins configuration for X measure */
    CyPins_SetPinDriveMode(`$INSTANCE_NAME`_xm_0, PIN_DM_STRONG);
    CyPins_SetPinDriveMode(`$INSTANCE_NAME`_xp_0, PIN_DM_STRONG);
    CyPins_SetPinDriveMode(`$INSTANCE_NAME`_yp_0, PIN_DM_ALG_HIZ);
    CyPins_SetPinDriveMode(`$INSTANCE_NAME`_ym_0, PIN_DM_ALG_HIZ);
    
    /* Switch AMux if Channel 1 not selected */
    if(`$INSTANCE_NAME`_AMux_GetChannel() == `$INSTANCE_NAME`_AMUX_XP_CHAN)
    {
        `$INSTANCE_NAME`_AMux_Next();
    }
    /* Switch AMux to Channel 1 if disconnected */
    else if(`$INSTANCE_NAME`_AMux_GetChannel() == `$INSTANCE_NAME`_AMUX_NO_CHAN)
    {
        `$INSTANCE_NAME`_AMux_Next();
        `$INSTANCE_NAME`_AMux_Next();
    }
    else
    {
        /* Nothing to do */
    }
}


/*******************************************************************************
* Function Name: `$INSTANCE_NAME`_TouchDetect
********************************************************************************
*
* Summary:
*  Determines if the screen is pressed.
*
* Parameters:
*  None
*
* Return:
*  uint8: the touch state
*  0 – untouched
*  1 – touched
* 
* Global variables:
*  `$INSTANCE_NAME`_measureVar - used to indicate measure function call.
*

*******************************************************************************/
uint8 `$INSTANCE_NAME`_TouchDetect(void) `=ReentrantKeil($INSTANCE_NAME . "_TouchDetect")`
{
    uint8 val = 0u;

     /* Pin configuration for touch detect */    
    CyPins_SetPinDriveMode(`$INSTANCE_NAME`_xp_0, PIN_DM_STRONG);

    /* Set delay for signal stabilization */
    CyDelayUs(5u);

    /* Pin configuration for touch detect */
    CyPins_SetPinDriveMode(`$INSTANCE_NAME`_xp_0, PIN_DM_RES_UP);
    
        /* Switch AMux if Channel 0 not selected */
    if(`$INSTANCE_NAME`_AMux_GetChannel() != `$INSTANCE_NAME`_AMUX_XP_CHAN)
    {
        `$INSTANCE_NAME`_AMux_Next();
    }

    /* Pin configuration for touch detect */
    CyPins_SetPinDriveMode(`$INSTANCE_NAME`_xm_0, PIN_DM_ALG_HIZ);

    /* Pin configuration for touch detect */
    CyPins_SetPinDriveMode(`$INSTANCE_NAME`_yp_0, PIN_DM_ALG_HIZ);
    
    /* Pin configuration for touch detect */    
    CyPins_SetPinDriveMode(`$INSTANCE_NAME`_ym_0, PIN_DM_STRONG);

    /* If Measure not happend cofigure pins for TouchDetect */
    if (`$INSTANCE_NAME`_measureVar == 0u)
    {
        /* Set delay for signal stabilization */
        CyDelayUs(20u);
    }
    else
    {
        /* Clear variable Measure */
        `$INSTANCE_NAME`_measureVar = 0u;  
        
        /* Set delay for signal stabilization */
        CyDelayUs(8u);        
    }
   
    /* Set variable val to 1 if touch detected */
    if( `$INSTANCE_NAME`_Measure() <= 0x777u)
    {
        val = 1u; 
    }
    
    return (val);
}


/*******************************************************************************
* Function Name: `$INSTANCE_NAME`_Measure
********************************************************************************
*
* Summary:
*  Returns the result of the A/D converter.
*
* Parameters:
*  None
*
* Return:
*  int16: the result of the ADC conversion.
* 
* Global variables:
*  `$INSTANCE_NAME`_measureVar - used to indicate measure function call.
*
*******************************************************************************/
int16 `$INSTANCE_NAME`_Measure(void) `=ReentrantKeil($INSTANCE_NAME . "_Measure")`
{
    int16   ADC_Result;
    
    /* Start ADC convert & get result */
    #if(`$INSTANCE_NAME`_SAR_SELECT == `$INSTANCE_NAME`_SAR) 
        
        `$INSTANCE_NAME`_ADC_SAR_StartConvert();
        `$INSTANCE_NAME`_ADC_SAR_IsEndConversion(`$INSTANCE_NAME`_ADC_SAR_WAIT_FOR_RESULT);

        ADC_Result = `$INSTANCE_NAME`_ADC_SAR_GetResult16();
    
    #else
        /* Start ADC convert & get result */
        `$INSTANCE_NAME`_ADC_StartConvert();
        `$INSTANCE_NAME`_ADC_IsEndConversion(`$INSTANCE_NAME`_ADC_WAIT_FOR_RESULT);
        
        ADC_Result = `$INSTANCE_NAME`_ADC_GetResult16();
    
    #endif  /* (`$INSTANCE_NAME`_SAR_SELECT) */
    
    /* Set variable Measure */
   `$INSTANCE_NAME`_measureVar = 1u;
    
    return (ADC_Result);
}


/* [] END OF FILE */
