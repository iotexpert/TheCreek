/*******************************************************************************
* File Name: `$INSTANCE_NAME`.c  
* Version `$CY_MAJOR_VERSION`.`$CY_MINOR_VERSION`
*
* Description:
*  This file provides the source code to the API for the Power Monitor
*  Component.
*
* Note:
*
*******************************************************************************
* Copyright 2012, Cypress Semiconductor Corporation.  All rights reserved.
* You may use this file only in accordance with the license, terms, conditions, 
* disclaimers, and limitations in the end user license agreement accompanying 
* the software package with which this file was provided.
********************************************************************************/

#include "`$INSTANCE_NAME`.h"
#include "`$INSTANCE_NAME`_PGA.h"
#include "`$INSTANCE_NAME`_PM_AMux_Voltage.h"
#include "`$INSTANCE_NAME`_PM_AMux_Current.h"
#include "`$INSTANCE_NAME`_ADC.h"


/* To run the initialization block only at the start up */
uint8 `$INSTANCE_NAME`_initVar = 0u;

/* To run the threshold initialization only at the start up */
static uint8 `$INSTANCE_NAME`_initThreshold = 0u;

/* last voltage, current annd auxiliary voltage channels */
extern uint8 CYDATA `$INSTANCE_NAME`_lastVoltageChan;
extern uint8 CYDATA `$INSTANCE_NAME`_lastCurrentChan;
extern uint8 CYDATA `$INSTANCE_NAME`_lastAuxVoltChan;

/* Fault and Warn Mask variables */
volatile uint32 CYDATA `$INSTANCE_NAME`_faultMask;
volatile uint32 CYDATA `$INSTANCE_NAME`_warnMask;

/* Enable fault and Enable warn variables */
CYBIT `$INSTANCE_NAME`_faultEnable;
CYBIT `$INSTANCE_NAME`_warnEnable;

/* Fault and Warn source variables */
volatile uint8 CYDATA `$INSTANCE_NAME`_warnSources;
volatile uint8 CYDATA `$INSTANCE_NAME`_faultSources;

/* Warn and Fault source status */
volatile uint8 CYDATA `$INSTANCE_NAME`_warnSourceStatus = 0u;
volatile uint8 CYDATA `$INSTANCE_NAME`_faultSourceStatus = 0u;

/* Warn and Fault status for power monitors */
volatile uint32 CYDATA `$INSTANCE_NAME`_OCWarnStatus = 0u;
volatile uint32 CYDATA `$INSTANCE_NAME`_UVWarnStatus = 0u;
volatile uint32 CYDATA `$INSTANCE_NAME`_OVWarnStatus = 0u;
volatile uint32 CYDATA `$INSTANCE_NAME`_OCFaultStatus = 0u;
volatile uint32 CYDATA `$INSTANCE_NAME`_UVFaultStatus = 0u;
volatile uint32 CYDATA `$INSTANCE_NAME`_OVFaultStatus = 0u;

/* Voltage tab related arrays as set in the customizer */
`$VoltageType`
`$UVWarnThreshold`
`$OVWarnThreshold`
`$UVFaultThreshold`
`$OVFaultThreshold`
`$VoltateScale`

/* Current tab related arrays as set in the customizer */
`$CurrentType`
`$OCWarnThreshold`
`$OCFaultThrehsold`
`$RShunt`
`$CSAGain`
`$ActiveCurrentChan`

#if (`$INSTANCE_NAME`_NUM_AUX_INPUTS > 0)
    /* Auxiliary voltage table related arrays as set in the customizer */
    `$AuxVoltageType`
#endif /* (`$INSTANCE_NAME`_NUM_AUX_INPUTS > 0) */

/* Local function protoptype */
static void `$INSTANCE_NAME`_StartupIsr(void);
int16 `$INSTANCE_NAME`_mVToCounts(int16 mV, uint8 chan) `=ReentrantKeil($INSTANCE_NAME . "_mVToCounts")`;
int32 `$INSTANCE_NAME`_AToCounts(float current, uint8 chan) `=ReentrantKeil($INSTANCE_NAME . "_AToCounts")`;
float `$INSTANCE_NAME`_GetMeasuredVal(uint8 chan, uint8 chanType) `=ReentrantKeil($INSTANCE_NAME . "_GetMeasuredVal")`;
static float `$INSTANCE_NAME`_GetFiltRaw(uint8 chan, uint8 chanType) `=ReentrantKeil($INSTANCE_NAME . "_GetFiltRaw")`;
static uint8 `$INSTANCE_NAME`_GetArrayIndex(uint8 chan) `=ReentrantKeil($INSTANCE_NAME . "_GetArrayIndex")`;


/****************************************************************************** 
* Function Name: `$INSTANCE_NAME`_Init
*******************************************************************************
*
* Summary:
*  Initialize component's parameters to the parameters set by user in the 
*  customizer of the component placed onto schematic. Usually called in 
* `$INSTANCE_NAME`_Start(). Also includes method for running self calibration.
*  
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
    /* Set the mode, and mask for warn and fault. */
    `$INSTANCE_NAME`_EnableFault();
    `$INSTANCE_NAME`_EnableWarn();
    `$INSTANCE_NAME`_SetFaultMode(`$INSTANCE_NAME`_DEFAULT_FAULT_MODE);
    `$INSTANCE_NAME`_SetWarnMode(`$INSTANCE_NAME`_DEFAULT_WARN_MODE);
    `$INSTANCE_NAME`_SetFaultMask(`$INSTANCE_NAME`_DEFAULT_FAULT_MASK);
    `$INSTANCE_NAME`_SetWarnMask(`$INSTANCE_NAME`_DEFAULT_WARN_MASK);
}


/******************************************************************************
* Function Name: `$INSTANCE_NAME`_Enable
*******************************************************************************
*
* Summary: 
*  Enables the power monitor component hardware blocks and starts scanning.
*  
*
* Parameters:  
*  None
*
* Return: 
*  None 
*
*******************************************************************************/
void `$INSTANCE_NAME`_Enable(void) `=ReentrantKeil($INSTANCE_NAME . "_Enable")`
{
    uint8 index;
    /* Start the hardware blocks */
    `$INSTANCE_NAME`_PM_AMux_Voltage_Start();
    `$INSTANCE_NAME`_PM_AMux_Current_Start();
    `$INSTANCE_NAME`_PGA_Start();
    `$INSTANCE_NAME`_StartupIsr();
    
    if (`$INSTANCE_NAME`_initThreshold == 0u)
    {
        /* Set warn and fault thresholds */
        for (index = 0u; index < `$INSTANCE_NAME`_NUM_CONVERTERS; index++)
        {
            `$INSTANCE_NAME`_SetOVFaultThreshold((index + 1), `$INSTANCE_NAME`_OVFaultThreshold[index]);
            `$INSTANCE_NAME`_SetUVFaultThreshold((index + 1), `$INSTANCE_NAME`_UVFaultThreshold[index]);
            `$INSTANCE_NAME`_SetOCFaultThreshold((index + 1), `$INSTANCE_NAME`_OCFaultThreshold[index]);
            `$INSTANCE_NAME`_SetOVWarnThreshold((index + 1), `$INSTANCE_NAME`_OVWarnThreshold[index]);
            `$INSTANCE_NAME`_SetUVWarnThreshold((index + 1), `$INSTANCE_NAME`_UVWarnThreshold[index]);
            `$INSTANCE_NAME`_SetOCWarnThreshold((index + 1), `$INSTANCE_NAME`_OCWarnThreshold[index]);
        }
        
        `$INSTANCE_NAME`_initThreshold = 1u;
    }
}


/******************************************************************************* 
* Function Name: `$INSTANCE_NAME`_Start
********************************************************************************
*
* Summary:
*  The start function initializes the power monitor with the default  
*  values by calling Init() API if component has been not initialized before.
*  Also calls Enable() API.
*
* Parameters:  
*  None
*
* Return: 
*  None 
*
* Global variables:
*  `$INSTANCE_NAME`_initVar:  Used to check the initial configuration,
*  modified when this function is called for the first time.
*
*******************************************************************************/
void `$INSTANCE_NAME`_Start() `=ReentrantKeil($INSTANCE_NAME . "_Start")`
{
    if(`$INSTANCE_NAME`_initVar == 0u)
    {
       `$INSTANCE_NAME`_Init();
       `$INSTANCE_NAME`_initVar = 1u;
    }

    /* Enable the component */
    `$INSTANCE_NAME`_Enable();
}


/*******************************************************************************
* Function Name: `$INSTANCE_NAME`_Stop
********************************************************************************
*
* Summary:
*  Stops the power monitor block operation which includes stopping of ADC 
*  sampling.
*
* Parameters:  
*  None
*
* Return: 
*  None 
*
* Side Effects: 
*  pgood, warn, fault and eoc outputs are de-asserted.
*
*******************************************************************************/
void `$INSTANCE_NAME`_Stop(void) `=ReentrantKeil($INSTANCE_NAME . "_Stop")`
{
    /* Stop the hardware */
    `$INSTANCE_NAME`_PM_AMux_Voltage_Stop();
    `$INSTANCE_NAME`_PM_AMux_Current_Stop();
    `$INSTANCE_NAME`_PGA_Stop();
    `$INSTANCE_NAME`_ADC_Stop();
    
    /* De-assert the output signals */
    #if !defined(`$INSTANCE_NAME`_B_PowerMonitor_`$Ctrl_Reg_RplcmntString`_ctrlreg1__REMOVED)
        `$INSTANCE_NAME`_CONTROL1_REG &= ~(`$INSTANCE_NAME`_EOC_MASK | `$INSTANCE_NAME`_WARN_MASK |
                                           `$INSTANCE_NAME`_FAULT_MASK);
    #endif /* (`$INSTANCE_NAME`_B_PowerMonitor_`$Ctrl_Reg_RplcmntString`_ctrlreg1__REMOVED) */
    
    #if !defined(`$INSTANCE_NAME`_B_PowerMonitor_`$CtrlReg_RplcString`_PM_1_8_Ctrl1__REMOVED)
         `$INSTANCE_NAME`_PGOOD_CONTROL1_REG &= ~`$INSTANCE_NAME`_PGOOD_CTRL_MASK;
    #endif /* (`$INSTANCE_NAME`_B_PowerMonitor_`$CtrlReg_RplcString`_PM_1_8_Ctrl1__REMOVED) */
    
    #if !defined(`$INSTANCE_NAME`_B_PowerMonitor_`$CtrlReg_RplcString`_PM_9_16_Ctrl2__REMOVED)
        #if (`$INSTANCE_NAME`_NUM_CONVERTERS > `$INSTANCE_NAME`_CONVERTERS_8)
            `$INSTANCE_NAME`_PGOOD_CONTROL2_REG &= ~`$INSTANCE_NAME`_PGOOD_CTRL_MASK;
        #endif /* (`$INSTANCE_NAME`_NUM_CONVERTERS > `$INSTANCE_NAME`_CONVERTERS_8) */
    #endif /* (`$INSTANCE_NAME`_B_PowerMonitor_`$CtrlReg_RplcString`_PM_9_16_Ctrl2__REMOVED) */
    
    #if !defined(`$INSTANCE_NAME`_B_PowerMonitor_`$CtrlReg_RplcString`_PM_17_24_Ctrl3__REMOVED)
        #if (`$INSTANCE_NAME`_NUM_CONVERTERS > `$INSTANCE_NAME`_CONVERTERS_16)
            `$INSTANCE_NAME`_PGOOD_CONTROL3_REG &= ~`$INSTANCE_NAME`_PGOOD_CTRL_MASK; 
        #endif /* (`$INSTANCE_NAME`_NUM_CONVERTERS > `$INSTANCE_NAME`_CONVERTERS_16) */
    #endif /* (`$INSTANCE_NAME`_B_PowerMonitor_`$CtrlReg_RplcString`_PM_17_24_Ctrl3__REMOVED) */
    
    #if !defined(`$INSTANCE_NAME`_B_PowerMonitor_`$CtrlReg_RplcString`_PM_25_32_Ctrl4__REMOVED)
        #if (`$INSTANCE_NAME`_NUM_CONVERTERS > `$INSTANCE_NAME`_CONVERTERS_24)
            `$INSTANCE_NAME`_PGOOD_CONTROL4_REG &= ~`$INSTANCE_NAME`_PGOOD_CTRL_MASK;
        #endif /* (`$INSTANCE_NAME`_NUM_CONVERTERS > `$INSTANCE_NAME`_CONVERTERS_24)*/
    #endif /* (`$INSTANCE_NAME`_B_PowerMonitor_`$CtrlReg_RplcString`_PM_25_32_Ctrl4__REMOVED) */
                
    /* Disable the interrupt */
    CyIntDisable(`$INSTANCE_NAME`_IRQ__INTC_NUMBER);
}


/*******************************************************************************
* Function Name: `$INSTANCE_NAME`_EnableFault
********************************************************************************
*
* Summary:
*  This API enables the generation of Fault signal. Enabling of fault sources
*  is configured using the `$INSTANCE_NAME`_SetFaultMode() and the 
*  `$INSTANCE_NAME`_SetFaultMask() APIs. Fault signal generation is 
*  automatically enabled by Init() API.
*
* Parameters:  
*  None
*
* Return: 
*  None 
*
*******************************************************************************/
void `$INSTANCE_NAME`_EnableFault(void) `=ReentrantKeil($INSTANCE_NAME . "_EnableFault")`
{
    /* Disable the interrupt */
    CyIntDisable(`$INSTANCE_NAME`_IRQ__INTC_NUMBER);
    /* Enable fault */
    `$INSTANCE_NAME`_faultEnable = `$INSTANCE_NAME`_CYTRUE;
    /* Enable the interrupt */
    CyIntEnable(`$INSTANCE_NAME`_IRQ__INTC_NUMBER);
}


/*******************************************************************************
* Function Name: `$INSTANCE_NAME`_DisableFault
********************************************************************************
*
* Summary:
*  Disables the genration of Fault signal.
*
* Parameters:  
*  None
*
* Return: 
*  None 
*
* Side Effects: 
*  Fault output is de-asserted.
*
*******************************************************************************/
void `$INSTANCE_NAME`_DisableFault(void) `=ReentrantKeil($INSTANCE_NAME . "_DisableFault")`
{
    /* Disable the interrupt */
    CyIntDisable(`$INSTANCE_NAME`_IRQ__INTC_NUMBER);
    /* Disable the fault */
    `$INSTANCE_NAME`_faultEnable = `$INSTANCE_NAME`_CYFALSE;
    /* Enable the interrupt */
    CyIntEnable(`$INSTANCE_NAME`_IRQ__INTC_NUMBER);
    /* De-assert the fault output signal */
    #if !defined(`$INSTANCE_NAME`_B_PowerMonitor_`$Ctrl_Reg_RplcmntString`_ctrlreg1__REMOVED)
        `$INSTANCE_NAME`_CONTROL1_REG &= ~`$INSTANCE_NAME`_FAULT_MASK;
    #endif /* (`$INSTANCE_NAME`_B_PowerMonitor_`$Ctrl_Reg_RplcmntString`_ctrlreg1__REMOVED) */
}


/*******************************************************************************
* Function Name: `$INSTANCE_NAME`_SetFaultMode
********************************************************************************
*
* Summary:
*  This API configures Fault sources for the component. Three Fault sources
*  available are: OV, UV, and OC.
*
* Parameters:  
*  uint8 faultMode: Value to set Fault sources.
*
* Return: 
*  None 
*
*******************************************************************************/
void `$INSTANCE_NAME`_SetFaultMode(uint8 faultMode) `=ReentrantKeil($INSTANCE_NAME . "_SetFaultMode")`
{
    /* Disable the interrupt */
    CyIntDisable(`$INSTANCE_NAME`_IRQ__INTC_NUMBER);
    /* Set the fault mode */
    `$INSTANCE_NAME`_faultSources = faultMode & `$INSTANCE_NAME`_FAULT_MODE_MASK;
    /* Enable the interrupt */
    CyIntEnable(`$INSTANCE_NAME`_IRQ__INTC_NUMBER);
}


/*******************************************************************************
* Function Name: `$INSTANCE_NAME`_GetFaultMode
********************************************************************************
*
* Summary:
*  This API returns the enabled Fault sources for the component.
*
* Parameters:  
*  None
*
* Return: 
*  uint8: The value corresponding to the enabled fault sources. 
*
*******************************************************************************/
uint8 `$INSTANCE_NAME`_GetFaultMode(void) `=ReentrantKeil($INSTANCE_NAME . "_GetFaultMode")`
{
    return (`$INSTANCE_NAME`_faultSources);
}


/*******************************************************************************
* Function Name: `$INSTANCE_NAME`_SetFaultMask
********************************************************************************
*
* Summary:
*  This API enables or disables faults from each power converters through a 
*  mask. Masking applies to all fault sources. By default all power converters
*  have their fault masks enabled.
*
* Parameters:  
*  uint32 faultMask: 32-bit mask value 
*
* Return: 
*  None 
*
*******************************************************************************/
void `$INSTANCE_NAME`_SetFaultMask(uint32 faultMask) `=ReentrantKeil($INSTANCE_NAME . "_SetFaultMask")`
{
    /* Disable the interrupt */
    CyIntDisable(`$INSTANCE_NAME`_IRQ__INTC_NUMBER);
    /* Set the fault mask */
    `$INSTANCE_NAME`_faultMask = faultMask & `$INSTANCE_NAME`_DEFAULT_FAULT_MASK;
    /* Enable the interrupt */
    CyIntEnable(`$INSTANCE_NAME`_IRQ__INTC_NUMBER);
}


/*******************************************************************************
* Function Name: `$INSTANCE_NAME`_GetFaultMask
********************************************************************************
*
* Summary:
*  This API returns fault mask status of each power converter. Masking applies
*  to all sources.
*
* Parameters:  
*  None
*
* Return: 
*  uint32: 32-bit mask value set for each of the sources. 
*
*******************************************************************************/
uint32 `$INSTANCE_NAME`_GetFaultMask(void) `=ReentrantKeil($INSTANCE_NAME . "_GetFaultMask")`
{
    return (`$INSTANCE_NAME`_faultMask);
}


/*******************************************************************************
* Function Name: `$INSTANCE_NAME`_GetFaultSource
********************************************************************************
*
* Summary:
*  This API returns the pending fault sources from the component. This API can 
*  be used to poll the fault status of the component.
*
* Parameters:  
*  None
*
* Return: 
*  uint8: 8-bit value indicating pending fault sources form the component.
*
*******************************************************************************/
uint8 `$INSTANCE_NAME`_GetFaultSource(void) `=ReentrantKeil($INSTANCE_NAME . "_GetFaultSource")`
{
    uint8 faultStatus;
    
    /* Disable the interrupt */
    CyIntDisable(`$INSTANCE_NAME`_IRQ__INTC_NUMBER);
    /* Get the status */
    faultStatus = `$INSTANCE_NAME`_faultSourceStatus;
    /* Re-enable the interrupt */
    CyIntEnable(`$INSTANCE_NAME`_IRQ__INTC_NUMBER);
    return (faultStatus);
}


/*******************************************************************************
* Function Name: `$INSTANCE_NAME`_GetOVFaultStatus
********************************************************************************
*
* Summary:
*  This API returns over voltage fault status of each of the power converters.
*  Status is reported regardless of the Fault Mask.
*
* Parameters:  
*  None
*
* Return: 
*  uint32: 32-bit value indicating OV status of power converters 
*
* Side Effects:
*  Calling this API clears the Fault condition source sticky bits.
* 
*******************************************************************************/
uint32 `$INSTANCE_NAME`_GetOVFaultStatus(void) `=ReentrantKeil($INSTANCE_NAME . "_GetOVFaultStatus")`
{
    uint8 OVFaultStatus;
    /* Disable the interrupt */
    CyIntDisable(`$INSTANCE_NAME`_IRQ__INTC_NUMBER);
    /* Read the over voltage fault status */
    OVFaultStatus = `$INSTANCE_NAME`_OVFaultStatus;
    /* Clear OV fault source status bit */
    `$INSTANCE_NAME`_faultSourceStatus &= ~`$INSTANCE_NAME`_ENABLE_OV_FAULT_SOURCE;
    /* Clear OV fault status */
    `$INSTANCE_NAME`_OVFaultStatus = `$INSTANCE_NAME`_RESET_OV_FAULT_STATUS;
    /* Check for other possible fault and then clear the hardware */
    #if !defined(`$INSTANCE_NAME`_B_PowerMonitor_`$Ctrl_Reg_RplcmntString`_ctrlreg1__REMOVED)
        if((`$INSTANCE_NAME`_UVFaultStatus == `$INSTANCE_NAME`_RESET_UV_FAULT_STATUS) && 
           (`$INSTANCE_NAME`_OCFaultStatus == `$INSTANCE_NAME`_RESET_OC_FAULT_STATUS))
        {
            `$INSTANCE_NAME`_CONTROL1_REG &= ~`$INSTANCE_NAME`_FAULT_MASK;
        }
    #endif /* (`$INSTANCE_NAME`_B_PowerMonitor_`$Ctrl_Reg_RplcmntString`_ctrlreg1__REMOVED) */
    
    /* Enable the interrupt */
    CyIntEnable(`$INSTANCE_NAME`_IRQ__INTC_NUMBER);
    
    return (OVFaultStatus);
}


/*******************************************************************************
* Function Name: `$INSTANCE_NAME`_GetUVFaultStatus
********************************************************************************
*
* Summary:
*  This API returns under voltage fault status of each of the power converters.
*
* Parameters:  
*  None
*
* Return: 
*  uint32: 32-bit value indicating under voltage status of power converters. 
*
* Side Effects:
*  Calling this API clears the fault condition source sticky bits.
*
*******************************************************************************/
uint32 `$INSTANCE_NAME`_GetUVFaultStatus(void) `=ReentrantKeil($INSTANCE_NAME . "_GetUVFaultStatus")`
{
    uint8 UVFaultStatus;
    
    /* Disable the interrupt */
    CyIntDisable(`$INSTANCE_NAME`_IRQ__INTC_NUMBER);
    /* Read the under voltage fault status */
    UVFaultStatus = `$INSTANCE_NAME`_UVFaultStatus;
    /* Clear UV fault source status bit */
    `$INSTANCE_NAME`_faultSourceStatus &= ~`$INSTANCE_NAME`_ENABLE_UV_FAULT_SOURCE;
    /* Clear UV fault status */
    `$INSTANCE_NAME`_UVFaultStatus = `$INSTANCE_NAME`_RESET_UV_FAULT_STATUS;
    /* Check for other possible fault and then clear the hardware */
    #if !defined(`$INSTANCE_NAME`_B_PowerMonitor_`$Ctrl_Reg_RplcmntString`_ctrlreg1__REMOVED)
        if((`$INSTANCE_NAME`_OVFaultStatus == `$INSTANCE_NAME`_RESET_OV_FAULT_STATUS) && 
           (`$INSTANCE_NAME`_OCFaultStatus == `$INSTANCE_NAME`_RESET_OC_FAULT_STATUS))
        {
            `$INSTANCE_NAME`_CONTROL1_REG &= ~`$INSTANCE_NAME`_FAULT_MASK;
        }
    #endif /* (`$INSTANCE_NAME`_B_PowerMonitor_`$Ctrl_Reg_RplcmntString`_ctrlreg1__REMOVED) */
    
    /* Enable the interrupt */
    CyIntEnable(`$INSTANCE_NAME`_IRQ__INTC_NUMBER);
    
    return (UVFaultStatus);
}


/*******************************************************************************
* Function Name: `$INSTANCE_NAME`_GetOCFaultStatus
********************************************************************************
*
* Summary:
*  This API returns Over current fault status of each of the power converters. 
*  The status is regardless of fault mask.
*
* Parameters:  
*  None
*
* Return: 
*  uint32: 32-bit valud indicating Over current status of power converters.
* 
* Side Effects:
*  Calling this API clears the fault condition source sticky bits.
*
*******************************************************************************/
uint32 `$INSTANCE_NAME`_GetOCFaultStatus(void) `=ReentrantKeil($INSTANCE_NAME . "_GetOCFaultStatus")`
{
    uint8 OCFaultStatus;
    /* Disable the interrupt */
    CyIntDisable(`$INSTANCE_NAME`_IRQ__INTC_NUMBER);
    /* Read the over current fault status */
    OCFaultStatus = `$INSTANCE_NAME`_OCFaultStatus;
    /* Clear OC fault source status bit */
    `$INSTANCE_NAME`_faultSourceStatus &= ~`$INSTANCE_NAME`_ENABLE_OC_FAULT_SOURCE;
    /* Clear OC fault status */
    `$INSTANCE_NAME`_OCFaultStatus = `$INSTANCE_NAME`_RESET_OC_FAULT_STATUS;
    /* Check for other possible fault and then clear the hardware */
    #if !defined(`$INSTANCE_NAME`_B_PowerMonitor_`$Ctrl_Reg_RplcmntString`_ctrlreg1__REMOVED)
        if((`$INSTANCE_NAME`_UVFaultStatus == `$INSTANCE_NAME`_RESET_UV_FAULT_STATUS) && 
           (`$INSTANCE_NAME`_OVFaultStatus == `$INSTANCE_NAME`_RESET_OV_FAULT_STATUS))
        {
            `$INSTANCE_NAME`_CONTROL1_REG &= ~`$INSTANCE_NAME`_FAULT_MASK;
        }
    #endif /* (`$INSTANCE_NAME`_B_PowerMonitor_`$Ctrl_Reg_RplcmntString`_ctrlreg1__REMOVED) */
    
    /* Enable the interrupt */
    CyIntEnable(`$INSTANCE_NAME`_IRQ__INTC_NUMBER);
    
    return (OCFaultStatus);
}


/*******************************************************************************
* Function Name: `$INSTANCE_NAME`_EnableWarn
********************************************************************************
*
* Summary:
*  This API enables the generation of warning signal. Enabling of warning 
*  sources is configured using `$INSTANCE_NAME`_SetWarnMode() and 
*  `$INSTANCE_NAME`_SetWarnMask() APIs. Warning signal generation is 
*  automatically enabled by the Init() API.
*
* Parameters:  
*  None
*
* Return: 
*  None 
*
*******************************************************************************/
void `$INSTANCE_NAME`_EnableWarn(void) `=ReentrantKeil($INSTANCE_NAME . "_EnableWarn")`
{
    /* Disable the interrupt */
    CyIntDisable(`$INSTANCE_NAME`_IRQ__INTC_NUMBER);
    /* Enable the warn */
    `$INSTANCE_NAME`_warnEnable = `$INSTANCE_NAME`_CYTRUE;
    /* Enable the interrupt */
    CyIntEnable(`$INSTANCE_NAME`_IRQ__INTC_NUMBER);
}


/*******************************************************************************
* Function Name: `$INSTANCE_NAME`_DisableWarn
********************************************************************************
*
* Summary:
*  Disables the generation of warning signal.
*
* Parameters:  
*  None
*
* Return: 
*  None 
*
* Side Effects:
*  Warning output is de-asserted.
*
*******************************************************************************/
void `$INSTANCE_NAME`_DisableWarn(void) `=ReentrantKeil($INSTANCE_NAME . "_DisableWarn")`
{
    /* Disable the interrupt */
    CyIntDisable(`$INSTANCE_NAME`_IRQ__INTC_NUMBER);
    /* Disable the warn */
    `$INSTANCE_NAME`_warnEnable = `$INSTANCE_NAME`_CYFALSE;
    /* Enable the interrupt */
    CyIntEnable(`$INSTANCE_NAME`_IRQ__INTC_NUMBER);
    /* De-assert the warn signal */
    #if !defined(`$INSTANCE_NAME`_B_PowerMonitor_`$Ctrl_Reg_RplcmntString`_ctrlreg1__REMOVED)
        `$INSTANCE_NAME`_CONTROL1_REG &= ~`$INSTANCE_NAME`_WARN_MASK;
    #endif /* (`$INSTANCE_NAME`_B_PowerMonitor_`$Ctrl_Reg_RplcmntString`_ctrlreg1__REMOVED) */
}


/*******************************************************************************
* Function Name: `$INSTANCE_NAME`_SetWarnMode
********************************************************************************
*
* Summary:
*  This API sets the warning sources for the component. Available three 
*  warning sources are OV, UV, OC.
*
* Parameters:  
*  uint8 warnMode: 8-bit value indicating the warn source to set.
*
* Return: 
*  None 
*
*******************************************************************************/
void `$INSTANCE_NAME`_SetWarnMode(uint8 warnMode) `=ReentrantKeil($INSTANCE_NAME . "_SetWarnMode")`
{
    /* Disable the interrupt */
    CyIntDisable(`$INSTANCE_NAME`_IRQ__INTC_NUMBER);
    /* Set the warn mode */
    `$INSTANCE_NAME`_warnSources = (warnMode & `$INSTANCE_NAME`_WARN_MODE_MASK);
    /* Enable the interrupt */
    CyIntEnable(`$INSTANCE_NAME`_IRQ__INTC_NUMBER);
}


/*******************************************************************************
* Function Name: `$INSTANCE_NAME`_GetWarnMode
********************************************************************************
*
* Summary:
*  This API retruns the enabled warning sources for the component.
*
* Parameters:  
*  None
*
* Return: 
*  uint8: 8-bit value indicating the enabled warning sources 
*
*******************************************************************************/
uint8 `$INSTANCE_NAME`_GetWarnMode(void) `=ReentrantKeil($INSTANCE_NAME . "_GetWarnMode")`
{
    return (`$INSTANCE_NAME`_warnSources);
}


/*******************************************************************************
* Function Name: `$INSTANCE_NAME`_SetWarnMask
********************************************************************************
*
* Summary:
*  Calling this API enables or disables warnings from each power converter 
*  through a mask. Masking applies to all warning sources. By default 
*  warning masks are enabled for each of the power converter.
*
* Parameters:  
*  uint32 warnMask: 32-bit warning mask value.
*
* Return: 
*  None 
*
*******************************************************************************/
void `$INSTANCE_NAME`_SetWarnMask(uint32 warnMask) `=ReentrantKeil($INSTANCE_NAME . "_SetWarnMask")`
{
    /* Disable the interrupt */
    CyIntDisable(`$INSTANCE_NAME`_IRQ__INTC_NUMBER);
    /* Set the warn mask */
    `$INSTANCE_NAME`_warnMask = (warnMask & `$INSTANCE_NAME`_DEFAULT_WARN_MASK);
    /* Enable the interrupt */
    CyIntEnable(`$INSTANCE_NAME`_IRQ__INTC_NUMBER);
}


/*******************************************************************************
* Function Name: `$INSTANCE_NAME`_GetWarnMask
********************************************************************************
*
* Summary:
*  This API returns the warning mask status of each of the power converter.
*
* Parameters:  
*  None
*
* Return: 
*  uint32: 32-bit value indicating warning mask status. 
*
*******************************************************************************/
uint32 `$INSTANCE_NAME`_GetWarnMask(void) `=ReentrantKeil($INSTANCE_NAME . "_GetWarnMask")`
{
    return (`$INSTANCE_NAME`_warnMask);
}


/*******************************************************************************
* Function Name: `$INSTANCE_NAME`_GetWarnSource
********************************************************************************
*
* Summary:
*  This API returns pending warning sources from the component.This API can be
*  used to poll the warning status of the component.
*
* Parameters:  
*  None
*
* Return: 
*  uint8: 8-bit value indicating the pending warning sources. 
*
*******************************************************************************/
uint8 `$INSTANCE_NAME`_GetWarnSource(void) `=ReentrantKeil($INSTANCE_NAME . "_GetWarnSource")`
{
    uint8 warnStatus;
    
    /* Disable the interrupt */
    CyIntDisable(`$INSTANCE_NAME`_IRQ__INTC_NUMBER);
    /* Get the status */
    warnStatus = `$INSTANCE_NAME`_warnSourceStatus;
    /* Enable the status */
    CyIntEnable(`$INSTANCE_NAME`_IRQ__INTC_NUMBER);
    return (warnStatus);
}


/*******************************************************************************
* Function Name: `$INSTANCE_NAME`_GetOVWarnStatus
********************************************************************************
*
* Summary:
*  This API can be used to get the over voltage warning status of each power
*  converter. The status is regardless of Warning Mask set.
*
* Parameters:  
*  None
*
* Return: 
*  uint32: 32-bit status indicating the over voltage warning status of each 
*  power converter.
*
*******************************************************************************/
uint32 `$INSTANCE_NAME`_GetOVWarnStatus(void) `=ReentrantKeil($INSTANCE_NAME . "_GetOVWarnStatus")`
{
    uint8 OVWarnStatus;
    
    /* Disable the interrupt */
    CyIntDisable(`$INSTANCE_NAME`_IRQ__INTC_NUMBER);
    /* Read the over voltage warn status */
    OVWarnStatus = `$INSTANCE_NAME`_OVWarnStatus;
    /* Clear OV warn status bit */
    `$INSTANCE_NAME`_warnSourceStatus &= ~`$INSTANCE_NAME`_ENABLE_OV_WARN_SOURCE;
    /* Clear OV warn status */
    `$INSTANCE_NAME`_OVWarnStatus = `$INSTANCE_NAME`_RESET_OV_WARN_STATUS;
    /* Check for other possible warn and then clear the hardware */
    #if !defined(`$INSTANCE_NAME`_B_PowerMonitor_`$Ctrl_Reg_RplcmntString`_ctrlreg1__REMOVED)
        if((`$INSTANCE_NAME`_UVWarnStatus == `$INSTANCE_NAME`_RESET_UV_WARN_STATUS) && 
           (`$INSTANCE_NAME`_OCWarnStatus == `$INSTANCE_NAME`_RESET_OC_WARN_STATUS))
        {
            `$INSTANCE_NAME`_CONTROL1_REG &= ~`$INSTANCE_NAME`_WARN_MASK;
        }
    #endif /* (`$INSTANCE_NAME`_B_PowerMonitor_`$Ctrl_Reg_RplcmntString`_ctrlreg1__REMOVED) */
    
    /* Enable the interrupt */
    CyIntEnable(`$INSTANCE_NAME`_IRQ__INTC_NUMBER);
    
    return (OVWarnStatus);
}


/*******************************************************************************
* Function Name: `$INSTANCE_NAME`_GetUVWarnStatus
********************************************************************************
*
* Summary:
*  This API returns the under voltage warning status of each power converter.
*  The status is regardless of Warning mask.
*
* Parameters:  
*  None
*
* Return: 
*  uint32: 32-bit value indicating under voltage warning status for each power
*  converter.
*
*******************************************************************************/
uint32 `$INSTANCE_NAME`_GetUVWarnStatus(void) `=ReentrantKeil($INSTANCE_NAME . "_GetUVWarnStatus")`
{
    uint8 UVWarnStatus;
    
    /* Disable the interrupt */
    CyIntDisable(`$INSTANCE_NAME`_IRQ__INTC_NUMBER);
    /* Read the under voltage warn status */
    UVWarnStatus = `$INSTANCE_NAME`_UVWarnStatus;
    /* Clear UV warn source status bit */
    `$INSTANCE_NAME`_warnSourceStatus &= ~`$INSTANCE_NAME`_ENABLE_UV_WARN_SOURCE;
    /* Clear UV warn status */
    `$INSTANCE_NAME`_UVWarnStatus = `$INSTANCE_NAME`_RESET_UV_WARN_STATUS;
    /* Check for other possible warn and then clear the hardware */
    #if !defined(`$INSTANCE_NAME`_B_PowerMonitor_`$Ctrl_Reg_RplcmntString`_ctrlreg1__REMOVED)
        if((`$INSTANCE_NAME`_OVWarnStatus == `$INSTANCE_NAME`_RESET_OV_WARN_STATUS) && 
           (`$INSTANCE_NAME`_OCWarnStatus == `$INSTANCE_NAME`_RESET_OC_WARN_STATUS))
        {
            `$INSTANCE_NAME`_CONTROL1_REG &= ~`$INSTANCE_NAME`_WARN_MASK;
        }
    #endif /* (`$INSTANCE_NAME`_B_PowerMonitor_`$Ctrl_Reg_RplcmntString`_ctrlreg1__REMOVED) */
    
    /* Enable the interrupt */
    CyIntEnable(`$INSTANCE_NAME`_IRQ__INTC_NUMBER);
    
    return (UVWarnStatus);
}


/*******************************************************************************
* Function Name: `$INSTANCE_NAME`_GetOCWarnStatus
********************************************************************************
*
* Summary:
*  This API returns over current warning status for each power converter. The 
*  status is regardless of Warning mask.
*
* Parameters:  
*  None
*
* Return: 
*  uint32: 32-bit value indicating the over current warning status of each 
*  power converter.
*
*******************************************************************************/
uint32 `$INSTANCE_NAME`_GetOCWarnStatus(void) `=ReentrantKeil($INSTANCE_NAME . "_GetOCWarnStatus")`
{
    uint8 OCWarnStatus;
    
    /* Disable the interrupt */
    CyIntDisable(`$INSTANCE_NAME`_IRQ__INTC_NUMBER);
    /* Read the over current warn status */
    OCWarnStatus = `$INSTANCE_NAME`_OCWarnStatus;
    /* Clear OC warn source status bit */
    `$INSTANCE_NAME`_warnSourceStatus &= ~`$INSTANCE_NAME`_ENABLE_OC_WARN_SOURCE;
    /* Clear OC warn status */
    `$INSTANCE_NAME`_OCWarnStatus = `$INSTANCE_NAME`_RESET_OC_WARN_STATUS;
    /* Check for other possible warn and then clear the hardware */
    #if !defined(`$INSTANCE_NAME`_B_PowerMonitor_`$Ctrl_Reg_RplcmntString`_ctrlreg1__REMOVED)
        if((`$INSTANCE_NAME`_UVWarnStatus == `$INSTANCE_NAME`_RESET_UV_WARN_STATUS) && 
           (`$INSTANCE_NAME`_OVWarnStatus == `$INSTANCE_NAME`_RESET_OV_WARN_STATUS))
        {
            `$INSTANCE_NAME`_CONTROL1_REG &= ~`$INSTANCE_NAME`_WARN_MASK;
        }
    #endif /* (`$INSTANCE_NAME`_B_PowerMonitor_`$Ctrl_Reg_RplcmntString`_ctrlreg1__REMOVED) */
    
    /* Enable the interrupt */
    CyIntEnable(`$INSTANCE_NAME`_IRQ__INTC_NUMBER);
    
    return (OCWarnStatus);
}


/*******************************************************************************
* Function Name: `$INSTANCE_NAME`_SetUVWarnThreshold
********************************************************************************
*
* Summary:
*  This API can be used to set the under voltage warning threshold for the 
*  specified power converter.
*
* Parameters:  
*  uint8 converterNum: Converter number for which the threshold to be set. 
*                      Valid range is 1-32.
*  uint16 uvWarnThreshold: Under voltage warning threshold in mv. Valid range
*                          is 1-65,535.
*
* Return: 
*  None 
*
*******************************************************************************/
void `$INSTANCE_NAME`_SetUVWarnThreshold(uint8 converterNum, uint16 uvWarnThreshold) \
     `=ReentrantKeil($INSTANCE_NAME . "_SetUVWarnThreshold")`
{
    int32 uvWarnCounts;
    float threshold;
    if (!(((converterNum > `$INSTANCE_NAME`_NUM_CONVERTERS) || (converterNum < 1u)) || 
        ((uvWarnThreshold < 1u) || (uvWarnThreshold > `$INSTANCE_NAME`_THRESHOLD_MAX_RANGE))))
    {
        threshold = uvWarnThreshold * `$INSTANCE_NAME`_VoltageScale[converterNum - 1];
        /* Convert mV to counts */
        uvWarnCounts = (int32)`$INSTANCE_NAME`_mVToCounts(threshold, converterNum - 1);
        uvWarnCounts *= `$INSTANCE_NAME`_VOLTAGE_FILTER_SIZE;
        /* Disable the interrupt */
        CyIntDisable(`$INSTANCE_NAME`_IRQ__INTC_NUMBER);
        /* Set the under voltage warn threshold */
        `$INSTANCE_NAME`_warnWin[converterNum - 1].UVWarnThrshldCounts = uvWarnCounts;
        `$INSTANCE_NAME`_warnWin[converterNum - 1].UVWarnThrshldVolts = uvWarnThreshold;
        /* Enable the interrupt */
        CyIntEnable(`$INSTANCE_NAME`_IRQ__INTC_NUMBER);
    }
}


/*******************************************************************************
* Function Name: `$INSTANCE_NAME`_GetUVWarnThreshold
********************************************************************************
*
* Summary:
*  This API returns the power converter under voltage warning threshold for 
*  the specified power converter.
*
* Parameters:  
*  uint8 converterNum: Converter number for which to get the UV threshold 
*                      value. Valid range is 1-32.
*
* Return: 
*  uint16: 16-bit value indicating the under voltage threshold in mV. 
*
*******************************************************************************/
uint16 `$INSTANCE_NAME`_GetUVWarnThreshold(uint8 converterNum) \
       `=ReentrantKeil($INSTANCE_NAME . "_GetUVWarnThreshold")`
{
    uint16 uvWarnThreshold;
    
    if ((converterNum < 1u) || (converterNum > `$INSTANCE_NAME`_NUM_CONVERTERS))
    {
        uvWarnThreshold = 0u;
    }
    else
    {
          uvWarnThreshold = `$INSTANCE_NAME`_warnWin[converterNum - 1].UVWarnThrshldVolts;
    }
    
    return (uvWarnThreshold);
}


/*******************************************************************************
* Function Name: `$INSTANCE_NAME`_SetOVWarnThreshold
********************************************************************************
*
* Summary:
*  This API can be used to set the over voltage warning threshold for the 
*  specified.power converter.
*
* Parameters:  
*  uint8 converterNum: 8-bit value indicating the converter number for which
                       OV warning threshold to be set.
*  uint16 ovWarnThreshold: Over voltage warning threshold in mV. Valid range
                           is 1 - 65,535.
*
* Return: 
*  None 
*
*******************************************************************************/
void `$INSTANCE_NAME`_SetOVWarnThreshold(uint8 converterNum, uint16 ovWarnThreshold) \
     `=ReentrantKeil($INSTANCE_NAME . "_SetOVWarnThreshold")`
{
    int32 ovWarnCounts;
    float threshold;
    if (!(((converterNum > `$INSTANCE_NAME`_NUM_CONVERTERS) || (converterNum < 1u)) || 
        ((ovWarnThreshold < 1u) || (ovWarnThreshold > `$INSTANCE_NAME`_THRESHOLD_MAX_RANGE))))
    {
        threshold = ovWarnThreshold * `$INSTANCE_NAME`_VoltageScale[converterNum - 1];
        /* Convert the mV to Counts */
        ovWarnCounts = (int32)`$INSTANCE_NAME`_mVToCounts(threshold, converterNum - 1);
        ovWarnCounts *= `$INSTANCE_NAME`_VOLTAGE_FILTER_SIZE;
        /* Disable the interrupt */
        CyIntDisable(`$INSTANCE_NAME`_IRQ__INTC_NUMBER);
        /* Set the over voltage warn threshold for warn checking */
        `$INSTANCE_NAME`_warnWin[converterNum - 1].OVWarnThrshldCounts = ovWarnCounts;
        `$INSTANCE_NAME`_warnWin[converterNum - 1].OVWarnThrshldVolts = ovWarnThreshold;
        /* Enable the interrupt */
        CyIntEnable(`$INSTANCE_NAME`_IRQ__INTC_NUMBER);
    }
}


/*******************************************************************************
* Function Name: `$INSTANCE_NAME`_GetOVWarnThreshold
********************************************************************************
*
* Summary:
*  This API returns the Over voltage warning threshold set for specified.power
*  converter.
*
* Parameters:  
*  uint8 converterNum: Converter number for which to get the OV threshold 
*                      value. Valid range is 1-32.
*
* Return: 
*  uint16: 16-bit value indicating the over voltage threshold in mV.
*
*******************************************************************************/
uint16 `$INSTANCE_NAME`_GetOVWarnThreshold(uint8 converterNum) \
       `=ReentrantKeil($INSTANCE_NAME . "_GetOVWarnThreshold")`
{
    uint16 ovWarnThreshold;
    
    if ((converterNum < 1u) || (converterNum > `$INSTANCE_NAME`_NUM_CONVERTERS))
    {
        ovWarnThreshold = 0u;
    }
    else
    {
        ovWarnThreshold = `$INSTANCE_NAME`_warnWin[converterNum - 1].OVWarnThrshldVolts;
    }
    
    return (ovWarnThreshold);
}


/*******************************************************************************
* Function Name: `$INSTANCE_NAME`_SetUVFaultThreshold
********************************************************************************
*
* Summary:
*  This API can be used to set the under voltage fault threshold for the 
*  specified power converter.
*
* Parameters:  
*  uint8 converterNum: 8-bit value indicating the converter number for which
                       UV fault threshold to be set.
*  uint16 UVFaultThreshold: Under voltage fault threshold in mV. Valid range
                           is 1 - 65,535.
*
* Return: 
*  None 
*
*******************************************************************************/
void `$INSTANCE_NAME`_SetUVFaultThreshold(uint8 converterNum, uint16 uvFaultThreshold) \
     `=ReentrantKeil($INSTANCE_NAME . "_SetUVFaultThreshold")`
{
    int32 uvFaultCounts;
    float threshold;
    if (!(((converterNum > `$INSTANCE_NAME`_NUM_CONVERTERS) || (converterNum < 1u)) || 
        ((uvFaultThreshold < 1u) || (uvFaultThreshold > `$INSTANCE_NAME`_THRESHOLD_MAX_RANGE))))
    { 
        threshold = uvFaultThreshold * `$INSTANCE_NAME`_VoltageScale[converterNum - 1];
        uvFaultCounts = (int32)`$INSTANCE_NAME`_mVToCounts(threshold, converterNum - 1);
        uvFaultCounts *= `$INSTANCE_NAME`_VOLTAGE_FILTER_SIZE;
        /* Disable the interrupt */
        CyIntDisable(`$INSTANCE_NAME`_IRQ__INTC_NUMBER);
        /* Set the threshold */
        `$INSTANCE_NAME`_faultWin[converterNum - 1].UVFaultThrshldCounts = uvFaultCounts;
        `$INSTANCE_NAME`_faultWin[converterNum - 1].UVFaultThrshldVolts = uvFaultThreshold;
        /* Enable the interrupt */
        CyIntEnable(`$INSTANCE_NAME`_IRQ__INTC_NUMBER);
    }
}


/*******************************************************************************
* Function Name: `$INSTANCE_NAME`_GetUVFaultThreshold
********************************************************************************
*
* Summary:
*  This API can be used to get the under voltage fault threshold set for the 
*  specified power converter..
*
* Parameters:  
*  uint8 converterNum: Converter number for which to get the UV fault threshold 
*                      value. Valid range is 1-32.
*
* Return: 
*  uint16: 16-bit value indicating the under voltage fault threshold in mV.
*
*******************************************************************************/
uint16 `$INSTANCE_NAME`_GetUVFaultThreshold(uint8 converterNum) \
       `=ReentrantKeil($INSTANCE_NAME . "_GetUVFaultThreshold")`
{
    uint16 uvFaultThreshold;
    
    if ((converterNum < 1u) || (converterNum > `$INSTANCE_NAME`_NUM_CONVERTERS))
    {
        uvFaultThreshold = 0u;
    }
    else
    {
        uvFaultThreshold = `$INSTANCE_NAME`_faultWin[converterNum - 1].UVFaultThrshldVolts;
    }
        
    return (uvFaultThreshold);
}


/*******************************************************************************
* Function Name: `$INSTANCE_NAME`_SetOVFaultThreshold
********************************************************************************
*
* Summary:
*  This API can be used to set the over voltage fault threshold for the 
*  specified power converter.
*
* Parameters:  
*  uint8 converterNum: 8-bit value indicating the converter number for which
                       OV fault threshold to be set.
*  uint16 OVFaultThreshold: Over voltage fault threshold in mV. Valid range
                           is 1 - 65,535.
*
* Return: 
*  None 
*
*******************************************************************************/
void `$INSTANCE_NAME`_SetOVFaultThreshold(uint8 converterNum, uint16 ovFaultThreshold) \
     `=ReentrantKeil($INSTANCE_NAME . "_SetOVFaultThreshold")`
{
    int32 ovFaultCounts;
    float threshold;
    if (!(((converterNum > `$INSTANCE_NAME`_NUM_CONVERTERS) || (converterNum < 1u)) || 
        ((ovFaultThreshold < 1u) || (ovFaultThreshold > `$INSTANCE_NAME`_THRESHOLD_MAX_RANGE))))
    {
        threshold = ovFaultThreshold * `$INSTANCE_NAME`_VoltageScale[converterNum - 1];
        /* Convert mV to counts */
        ovFaultCounts = (int32)`$INSTANCE_NAME`_mVToCounts(threshold, converterNum - 1);
        ovFaultCounts *= `$INSTANCE_NAME`_VOLTAGE_FILTER_SIZE;
        /* Disable the interrupt */
        CyIntDisable(`$INSTANCE_NAME`_IRQ__INTC_NUMBER);
        /* Set the over voltage fault threshold */
        `$INSTANCE_NAME`_faultWin[converterNum - 1].OVFaultThrshldCounts = ovFaultCounts;
        `$INSTANCE_NAME`_faultWin[converterNum - 1].OVFaultThrshldVolts = ovFaultThreshold;
        CyIntEnable(`$INSTANCE_NAME`_IRQ__INTC_NUMBER);
    }
}


/*******************************************************************************
* Function Name: `$INSTANCE_NAME`_GetOVFaultThreshold
********************************************************************************
*
* Summary:
*  This API can be used to get the over voltage fault threshold for the 
*  specified power converter.
*
* Parameters:  
*  uint8 converterNum: Converter number for which to get the OV fault threshold 
*                      value. Valid range is 1-32.
*
* Return: 
*  uint16: 16-bit value indicating the over voltage fault threshold in mV.
*
*******************************************************************************/
uint16 `$INSTANCE_NAME`_GetOVFaultThreshold(uint8 converterNum) \
       `=ReentrantKeil($INSTANCE_NAME . "_GetOVFaultThreshold")`
{
    uint16 ovFaultThreshold;
    
    if ((converterNum < 1u) || (converterNum > `$INSTANCE_NAME`_NUM_CONVERTERS))
    {
        ovFaultThreshold = 0u;
    }
    else
    {
       ovFaultThreshold = `$INSTANCE_NAME`_faultWin[converterNum - 1].OVFaultThrshldVolts;
    }
        
    return (ovFaultThreshold);
}


/*******************************************************************************
* Function Name: `$INSTANCE_NAME`_SetOCWarnThreshold
********************************************************************************
*
* Summary:
*  This API can be used to set the over current warning threshold for the 
*  specified power converter.
*
* Parameters:  
*  uint8 converterNum: 8-bit value indicating the converter number for which
                       over current warning threshold to be set.
*  uint16 OCWarnThreshold: Over Current warning threshold in mV. Valid range
                           is 1 - 65,535.
*
* Return: 
*  None 
*
*******************************************************************************/
void `$INSTANCE_NAME`_SetOCWarnThreshold(uint8 converterNum, float ocWarnThreshold) \
     `=ReentrantKeil($INSTANCE_NAME . "_SetOCWarnThreshold")`
{
    int32 ocWarnCounts;
    
    if (!((converterNum > `$INSTANCE_NAME`_NUM_CONVERTERS) || (converterNum < 1u)))
    {
        /* Convert current reading to ADC counts */
        ocWarnCounts = `$INSTANCE_NAME`_AToCounts(ocWarnThreshold, (converterNum - 1));
        /* Disable the interrupt */
        CyIntDisable(`$INSTANCE_NAME`_IRQ__INTC_NUMBER);
        `$INSTANCE_NAME`_warnWin[converterNum - 1].OCWarnThrshldCounts = ocWarnCounts;
        `$INSTANCE_NAME`_warnWin[converterNum - 1].OCWarnThrshldAmps = ocWarnThreshold;
        CyIntEnable(`$INSTANCE_NAME`_IRQ__INTC_NUMBER);
    }
}


/*******************************************************************************
* Function Name: `$INSTANCE_NAME`_GetOCWarnThreshold
********************************************************************************
*
* Summary:
*  This API returns the over current warning theshold set for specified power
*  converter.
*
* Parameters:  
*  uint8 converterNum: Converter number for which to get the over current  
*                      warning threshold value. Valid range is 1-32.
*
* Return: 
*  uint16: 16-bit value indicating the over current warning threshold in mV.
*
*******************************************************************************/
float `$INSTANCE_NAME`_GetOCWarnThreshold(uint8 converterNum) \
       `=ReentrantKeil($INSTANCE_NAME . "_GetOCWarnThreshold")`
{
    float ocWarnThreshold;
    
    if ((converterNum < 1u) || (converterNum > `$INSTANCE_NAME`_NUM_CONVERTERS))
    {
        ocWarnThreshold = 0.0;
    }
    else
    {
        ocWarnThreshold = `$INSTANCE_NAME`_warnWin[converterNum - 1].OCWarnThrshldAmps;
    }
    
    return (ocWarnThreshold);
}


/*******************************************************************************
* Function Name: `$INSTANCE_NAME`_SetOCFaultThreshold
********************************************************************************
*
* Summary:
*  This API can be used to set the over current fault threshold for the 
*  specified power converter.
*
* Parameters:  
*  uint8 converterNum: 8-bit value indicating the converter number for which
                       over current fault threshold to be set.
*  uint16 OCFaultThreshold: Over Current fault threshold in mV. Valid range
                           is 1 - 65,535.
*
* Return: 
*  None  
*
*******************************************************************************/
void `$INSTANCE_NAME`_SetOCFaultThreshold(uint8 converterNum, float ocFaultThreshold) \
     `=ReentrantKeil($INSTANCE_NAME . "_SetOCFaultThreshold")`
{
    int32 ocFaultCounts;
    
    if (!((converterNum > `$INSTANCE_NAME`_NUM_CONVERTERS) || (converterNum < 1u)))
    {
        /* Convert the current threshold to ADC counts value */
        ocFaultCounts = `$INSTANCE_NAME`_AToCounts(ocFaultThreshold, (converterNum - 1));
        /* Disable the interrupt */
        CyIntDisable(`$INSTANCE_NAME`_IRQ__INTC_NUMBER);
        `$INSTANCE_NAME`_faultWin[converterNum - 1].OCFaultThrshldCounts = ocFaultCounts;
        `$INSTANCE_NAME`_faultWin[converterNum - 1].OCFaultThrshldAmps = ocFaultThreshold;
        CyIntEnable(`$INSTANCE_NAME`_IRQ__INTC_NUMBER);
    }
}


/*******************************************************************************
* Function Name: `$INSTANCE_NAME`_GetOCFaultThreshold
********************************************************************************
*
* Summary:
*  This API returns the over current fault theshold set for specified power
*  converter.
*
* Parameters:  
*  uint8 converterNum: Converter number for which to get the over current  
*                      fault threshold value. Valid range is 1-32.
*
* Return: 
*  uint16: 16-bit value indicating the over current fault threshold in mV.
*
*******************************************************************************/
float `$INSTANCE_NAME`_GetOCFaultThreshold(uint8 converterNum) \
       `=ReentrantKeil($INSTANCE_NAME . "_GetOCFaultThreshold")`
{
    float ocFaultThreshold;
    
    if ((converterNum < 1u) || (converterNum > `$INSTANCE_NAME`_NUM_CONVERTERS))
    {
        ocFaultThreshold = 0.0;
    }
    else
    {
        ocFaultThreshold = `$INSTANCE_NAME`_faultWin[converterNum - 1].OCFaultThrshldAmps;
    }
        return (ocFaultThreshold);
}


/*******************************************************************************
* Function Name: `$INSTANCE_NAME`_GetConverterVoltage
********************************************************************************
*
* Summary:
*  This API returns the power converter output voltage for the specified power
*  converter. If averaging is enabled, then value returned is the averaged
*  value.
*
* Parameters:  
*  uint8 converterNum: Converter number for which to read the voltage. Valid
*                      Range is 1 - 32.
*
* Return: 
*  uint16: Output voltage for the specified converter in mV Valid range 
*          is 1 - 65535.
*
*******************************************************************************/
uint16 `$INSTANCE_NAME`_GetConverterVoltage(uint8 converterNum) \
       `=ReentrantKeil($INSTANCE_NAME . "_GetConverterVoltage")`
{
    float retval;

    if ((converterNum > `$INSTANCE_NAME`_NUM_CONVERTERS) || (converterNum < 1u)) 
    {
        retval = 0.0;    /* There is no ADC Channel for this converter */
    }
    else
    {
        /* raw pin voltage, in mV   */
        retval  = `$INSTANCE_NAME`_GetMeasuredVal((converterNum - 1), `$INSTANCE_NAME`_VOLTAGE); 
        retval /= `$INSTANCE_NAME`_VoltageScale[converterNum - 1];
        retval = (retval + 0.5);              /* Ret rounded-off integer  */
    }
    return ((uint16)retval);
}


/*******************************************************************************
* Function Name: `$INSTANCE_NAME`_GetConverterCurrent
********************************************************************************
*
* Summary:
*  This API returns the power converter load current for the specified power
*  converter. If averaging is enabled, then value returned is the averaged
*  value.
*
* Parameters:  
*  uint8 converterNum: Converter number for which to read the current. Valid
*                      Range is 1 - 32.
*
* Return: 
*  uint16: Output current for the specified converter in 10mA steps 
*          Valid range is 1 - 65535.
*
*******************************************************************************/
float `$INSTANCE_NAME`_GetConverterCurrent(uint8 converterNum) \
       `=ReentrantKeil($INSTANCE_NAME . "_GetConverterCurrent")`
{
    float retval = 0.0;

    if ((converterNum > `$INSTANCE_NAME`_NUM_CONVERTERS) || (converterNum < 1u)) 
    {
        retval = 0.0;    /* There is no ADC Channel for this converter */
    }
    else
    {
        #if (`$INSTANCE_NAME`_NUM_CURRENT_SOURCES > 0)
            /* raw pin voltage, in mV   */
            retval  = `$INSTANCE_NAME`_GetMeasuredVal((converterNum - 1), `$INSTANCE_NAME`_CURRENT); 
            /* Convert pin voltage to current */
            if (`$INSTANCE_NAME`_CurrentType[converterNum - 1] == `$INSTANCE_NAME`_CURRENT_TYPE_DIRECT)
            {
                retval /= `$INSTANCE_NAME`_RShunt[converterNum - 1];
            }
            else if (`$INSTANCE_NAME`_CurrentType[converterNum - 1] == `$INSTANCE_NAME`_CURRENT_TYPE_CSA)
            {
                retval /= (`$INSTANCE_NAME`_CSAGain[converterNum - 1] * `$INSTANCE_NAME`_RShunt[converterNum - 1]);
            }
            else
            {
                /* No action */
            }
        #endif /* (`$INSTANCE_NAME`_NUM_CURRENT_SOURCES > 0 */
    }
    return (retval);             
}


/*******************************************************************************
* Function Name: `$INSTANCE_NAME`_AuxiliaryVoltage
********************************************************************************
*
* Summary:
*  This API returns the output voltage for one of the four auxillary inputs.
*
* Parameters:  
*  uint8 auxNum: Auxillary input number. Valid range is 1 - 4.
*
* Return: 
*  uint16: 16-bit value indicating the output voltage for one of the four 
*          auxillary inputs. Valid range is 1 - 65535.
*
*******************************************************************************/
float `$INSTANCE_NAME`_GetAuxiliaryVoltage(uint8 auxNum) `=ReentrantKeil($INSTANCE_NAME . "_GetAuxiliaryVoltage")`
{
    float retval = 0.0;

    if ((auxNum > `$INSTANCE_NAME`_NUM_AUX_INPUTS) || (auxNum == 0u)) 
    {
        retval = 0.0;    /* There is no ADC Channel for this converter */
    }
    else
    {
        #if (`$INSTANCE_NAME`_NUM_AUX_INPUTS > 0)
            /* Disable the interrupt before fetching the raw ADC data */
            CyIntDisable(`$INSTANCE_NAME`_IRQ__INTC_NUMBER);
            retval  = `$INSTANCE_NAME`_auxVoltCtl[auxNum - 1].filtVal;
            /* Re-enable the interrupt */
            CyIntEnable(`$INSTANCE_NAME`_IRQ__INTC_NUMBER);
            
            retval /= `$INSTANCE_NAME`_AUX_VOLTAGE_FILTER_SIZE;
            
            if ((`$INSTANCE_NAME`_AuxVoltageType[auxNum - 1] == `$INSTANCE_NAME`_AUX_VOLTAGE_64MV_DIFF) ||
                (`$INSTANCE_NAME`_AuxVoltageType[auxNum - 1] == `$INSTANCE_NAME`_AUX_VOLTAGE_128MV_DIFF))
            {
                /* Use config-2 correction value */
                retval *= `$INSTANCE_NAME`_adcGainCfg2;   
            }
            else
            {
                /* Use config-1 correction value */
                retval *= `$INSTANCE_NAME`_adcGainCfg1;    /* Convert counts to 1mV units   */
                if (`$INSTANCE_NAME`_AuxVoltageType[auxNum - 1] == `$INSTANCE_NAME`_AUX_VOLTAGE_SINGLE)
                {
                    retval += `$INSTANCE_NAME`_adcSeAdjCfg1;
                }
            }
            retval = retval / `$INSTANCE_NAME`_VOLTAGE_SCALE; 
        #endif /* `$INSTANCE_NAME`_NUM_AUX_INPUTS > 0 */
    }
    return (retval);    
}


/*******************************************************************************
* Function Name: `$INSTANCE_NAME`_StartupISR
********************************************************************************
*
* Summary:
*  This is an internal API. This starts the ISR routine manually by setting the 
*  interrupt pending register. This also sets the initial state for ADC to start 
*  after software reset happens..
*
* Parameters:  
*  None.
*
* Return: 
*  None.
*
*******************************************************************************/
static void `$INSTANCE_NAME`_StartupIsr(void)
{
     /* last voltage, current annd auxiliary voltage channels */
    `$INSTANCE_NAME`_lastVoltageChan = `$INSTANCE_NAME`_NUM_CONVERTERS;
    `$INSTANCE_NAME`_lastCurrentChan = `$INSTANCE_NAME`_NUM_CURRENT_SOURCES + `$INSTANCE_NAME`_NUM_CONVERTERS;
    `$INSTANCE_NAME`_lastAuxVoltChan = `$INSTANCE_NAME`_NUM_AUX_INPUTS + `$INSTANCE_NAME`_lastCurrentChan;

    /* Manually start the first ADC ISR */
    `$INSTANCE_NAME`_IsrStart();       
    
    /* Start the calibration process */
    `$INSTANCE_NAME`_Calibrate();

}

/*******************************************************************************
* Function Name: `$INSTANCE_NAME`_mVToCounts()
********************************************************************************
*
* Summary:
*  Convert mV to Counts for DelSig Config #1 +/- 2048 mV range
*
* Parameters:
*  mV = millivolt value to convert to counts
*
* Return:
*  counts = DelSig raw counts corresponding to desired mV value
*
*******************************************************************************/
int16 `$INSTANCE_NAME`_mVToCounts(int16 mV, uint8 chan) `=ReentrantKeil($INSTANCE_NAME . "_mVToCounts")`
{
    int16 counts = mV;

    if (`$INSTANCE_NAME`_VoltageType[chan] == `$INSTANCE_NAME`_VOLTAGE_TYPE_SINGLE)
    {
        counts -= `$INSTANCE_NAME`_adcSeAdjCfg1;      /* +/-2048 + 2048*/
    }
    counts  = (int16) ((counts / `$INSTANCE_NAME`_adcGainCfg1) + 0.5);
    return (counts);
}


/*******************************************************************************
* Function Name: `$INSTANCE_NAME`_AToCounts()
********************************************************************************
*
* Summary:
*  Converts current to Counts for DelSig Config #1 +/- 2048 mV range
*  and Config #2 +/- 64mV range
*
* Parameters:
*  mA = milliAmp value to convert to counts
*
* Return:
*  counts = DelSig raw counts corresponding to desired mA value
*
*******************************************************************************/
int32 `$INSTANCE_NAME`_AToCounts(float current, uint8 chan) `=ReentrantKeil($INSTANCE_NAME . "_AToCounts")`
{
    float pinV = current;
    int32 uint16Limit;
    
    if (`$INSTANCE_NAME`_CurrentType[chan] == `$INSTANCE_NAME`_CURRENT_TYPE_CSA)
    {
        /* Convert current to mV */
        pinV *= (`$INSTANCE_NAME`_CSAGain[chan] * `$INSTANCE_NAME`_RShunt[chan]);
        pinV -= `$INSTANCE_NAME`_adcSeAdjCfg1;     /* Apply PGA 2048 mV offset   */
        pinV /= `$INSTANCE_NAME`_adcGainCfg1;      /* counts = mV / mV/count     */
    }
    else if (`$INSTANCE_NAME`_CurrentType[chan] == `$INSTANCE_NAME`_CURRENT_TYPE_DIRECT)
    {
        /* Convert current to mV */
        pinV *= `$INSTANCE_NAME`_RShunt[chan];  /* current is in 10mA steps. */
        pinV /= `$INSTANCE_NAME`_adcGainCfg2;   /* divide by mV/count         */
    }
    else
    {
       /* Nothing to do */
    }
    
    /***************************************************************************
    * Ensure OV/UV limit does not exceed the "running total" maintained by
    *  the ADC ISR filter.
    ***************************************************************************/
    uint16Limit = (int32)(pinV * `$INSTANCE_NAME`_CURRENT_FILTER_SIZE);   /* runing total */
    if (uint16Limit > LONG_MAX) 
    {
        uint16Limit = LONG_MAX;       /* saturate to prevent wraparound */
    }
    return (uint16Limit);
}


/*******************************************************************************
* Function Name: `$INSTANCE_NAME`_GetMeasuredVal()
********************************************************************************
*
* Summary:
*  Fetch specified channel measurement value.
*
* Parameters:
*  chan = channel number
*
* Return:
*  floating point are milli-volts
*
*******************************************************************************/
float `$INSTANCE_NAME`_GetMeasuredVal(uint8 chan, uint8 chanType) `=ReentrantKeil($INSTANCE_NAME . "_GetMeasuredVal")`
{
    float retval = 0.0;
    uint8 index;

    /* Get filtered ADC mV reading */
    if (chanType == `$INSTANCE_NAME`_CURRENT)
    {
        /* Check and select valid array index to fetch filtered raw current value */
        index = `$INSTANCE_NAME`_GetArrayIndex(chan);
        if (index != 0)
        {
            index = index - 1; /* zero based index for array */
            retval = (float)`$INSTANCE_NAME`_GetFiltRaw(index, chanType);
        }
        if (`$INSTANCE_NAME`_CurrentType[chan] == `$INSTANCE_NAME`_CURRENT_TYPE_CSA)
        {
            /* Convert counts to 1mV units   */
            retval *= `$INSTANCE_NAME`_adcGainCfg1;    
            retval += `$INSTANCE_NAME`_adcSeAdjCfg1;
        }
        else if (`$INSTANCE_NAME`_CurrentType[chan] == `$INSTANCE_NAME`_CURRENT_TYPE_DIRECT)
        {
            /* Convert mV units  */
            retval *= `$INSTANCE_NAME`_adcGainCfg2;   
        }
        else
        {
            retval = 0; 
        }
    }
    else
    {
        index = chan;
        retval = (float)`$INSTANCE_NAME`_GetFiltRaw(index, chanType);
        /* Convert counts to 1mV units   */
        retval *= `$INSTANCE_NAME`_adcGainCfg1;    
        if (`$INSTANCE_NAME`_VoltageType[chan] == `$INSTANCE_NAME`_VOLTAGE_TYPE_SINGLE)
        {
            retval += `$INSTANCE_NAME`_adcSeAdjCfg1;   /* fix PGA 2048mV measure ref    */
        }
    }
    return(retval);
}


/*******************************************************************************
* Function Name: `$INSTANCE_NAME`_GetFiltRaw()
********************************************************************************
*
* Summary:
*  Fetch raw filtered ADC value in counts for specified channel.
*
* Parameters:
*  chan = channel number
*  chanType = Voltage or Current or Auxiliary channel
*
* Return:
*  floating point are milli-volts
*
*******************************************************************************/
static float `$INSTANCE_NAME`_GetFiltRaw(uint8 chan, uint8 chanType) `=ReentrantKeil($INSTANCE_NAME . "_GetFiltRaw")`
{
    float retval = 0;

    CyIntDisable(`$INSTANCE_NAME`_IRQ__INTC_NUMBER); 
    if (chanType == `$INSTANCE_NAME`_VOLTAGE)
    {
        retval = (float)`$INSTANCE_NAME`_voltCtl[chan].filtVal;
    }
    #if (`$INSTANCE_NAME`_NUM_CURRENT_SOURCES > 0)
        else if (chanType == `$INSTANCE_NAME`_CURRENT)
        {
            retval = (float)`$INSTANCE_NAME`_ampCtl[chan].filtVal;
        }    
        else
        {
            /* No action */
        }
    #endif /* (`$INSTANCE_NAME`_NUM_CURRENT_SOURCES > 0) */

    CyIntEnable(`$INSTANCE_NAME`_IRQ__INTC_NUMBER);

    if (chanType == `$INSTANCE_NAME`_VOLTAGE)
    {
        retval /= `$INSTANCE_NAME`_VOLTAGE_FILTER_SIZE;
    }
    #if (`$INSTANCE_NAME`_NUM_CURRENT_SOURCES > 0)
        else
        {
            retval /= `$INSTANCE_NAME`_CURRENT_FILTER_SIZE;
        }
    #endif /* (`$INSTANCE_NAME`_NUM_CURRENT_SOURCES > 0) */
    return (retval);
}


/*******************************************************************************
* Function Name: `$INSTANCE_NAME`_GetArrayIndex()
********************************************************************************
*
* Summary:
*  Fetchs the array index for Current raw value array.
*
* Parameters:
*  chan = channel number
*
* Return:
*  valid array index.
*
*******************************************************************************/
static uint8 `$INSTANCE_NAME`_GetArrayIndex(uint8 chan) `=ReentrantKeil($INSTANCE_NAME . "_GetArrayIndex")`
{
    uint8 chanNum = 0u;
    uint8 i;
    for (i = 0u; i <= chan; i++)
    {
        if (`$INSTANCE_NAME`_CurrentType[i] != `$INSTANCE_NAME`_CURRENT_TYPE_NA)
        {
            chanNum++;
        }
    }
 
    return (chanNum);
}


/* [] END OF FILE */
