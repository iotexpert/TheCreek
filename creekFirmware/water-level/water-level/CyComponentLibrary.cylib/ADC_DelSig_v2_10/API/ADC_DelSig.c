/*******************************************************************************
* File Name: `$INSTANCE_NAME`.c  
* Version `$CY_MAJOR_VERSION`.`$CY_MINOR_VERSION`
*
* Description:
*  This file provides the source code to the API for the Delta-Sigma ADC
*  Component.
*
* Note:
*
*******************************************************************************
* Copyright 2008-2010, Cypress Semiconductor Corporation.  All rights reserved.
* You may use this file only in accordance with the license, terms, conditions, 
* disclaimers, and limitations in the end user license agreement accompanying 
* the software package with which this file was provided.
********************************************************************************/

#include "`$INSTANCE_NAME`.h"

#if(`$INSTANCE_NAME`_DEFAULT_INTERNAL_CLK)
    #include "`$INSTANCE_NAME`_theACLK.h"
#endif /* `$INSTANCE_NAME`_DEFAULT_INTERNAL_CLK */

#if(`$INSTANCE_NAME`_DEFAULT_CHARGE_PUMP_CLOCK)
    #include "`$INSTANCE_NAME`_Ext_CP_Clk.h"
#endif /* `$INSTANCE_NAME`_DEFAULT_CHARGE_PUMP_CLOCK */

#if(`$INSTANCE_NAME`_DEFAULT_INPUT_MODE)
    #include "`$INSTANCE_NAME`_AMux.h"
#endif /* `$INSTANCE_NAME`_DEFAULT_INPUT_MODE */

/* Software flag for checking conversion completed or not */
volatile uint8 `$INSTANCE_NAME`_convDone = 0u;

/* Software flag to stop conversion for single sample conversion mode 
   with resolution above 16 bits */
volatile uint8 `$INSTANCE_NAME`_stopConversion = 0;

/* To run the initialization block only at the start up */
uint8 `$INSTANCE_NAME`_initVar = 0u;

volatile int32 `$INSTANCE_NAME`_Offset = 0;
volatile int32 `$INSTANCE_NAME`_CountsPerVolt = (uint32)`$INSTANCE_NAME`_DFLT_COUNTS_PER_VOLT;


/******************************************************************************* 
* Function Name: `$INSTANCE_NAME`_Init
********************************************************************************
*
* Summary:
*  Initialize component's parameters to the parameters set by user in the 
*  customizer of the component placed onto schematic. Usually called in 
* `$INSTANCE_NAME`_Start().
*  
*
* Parameters:  
*  void
*
* Return: 
*  void 
*
* Reentrance: 
*  Yes
*
*******************************************************************************/
void `$INSTANCE_NAME`_Init(void) `=ReentrantKeil($INSTANCE_NAME . "_Init")`
{
    /* Initialize the registers with default customizer settings for config 1 */
    `$INSTANCE_NAME`_InitConfig(1);

    /* This is only valid if there is an internal clock */
    #if(`$INSTANCE_NAME`_DEFAULT_INTERNAL_CLK)
        `$INSTANCE_NAME`_theACLK_SetMode(CYCLK_DUTY);
    #endif /* `$INSTANCE_NAME`_DEFAULT_INTERNAL_CLK */
    
    /* This is only valid if there is an external charge pump clock */
    #if(`$INSTANCE_NAME`_DEFAULT_CHARGE_PUMP_CLOCK)
        `$INSTANCE_NAME`_Ext_CP_Clk_SetMode(CYCLK_DUTY);
    #endif /* `$INSTANCE_NAME`_DEFAULT_CHARGE_PUMP_CLOCK */

    /* To perform ADC calibration */
    `$INSTANCE_NAME`_GainCompensation(`$INSTANCE_NAME`_DEFAULT_RANGE, 
                                      `$INSTANCE_NAME`_DEFAULT_IDEAL_DEC_GAIN, 
                                      `$INSTANCE_NAME`_DEFAULT_IDEAL_ODDDEC_GAIN, 
                                      `$INSTANCE_NAME`_DEFAULT_RESOLUTION);        
}


/******************************************************************************* 
* Function Name: `$INSTANCE_NAME`_Enable
********************************************************************************
*
* Summary: 
*  Enables the ADC DelSig block operation.
*  
*
* Parameters:  
*  void
*
* Return: 
*  void 
*
* Reentrance: 
*  Yes
*
*******************************************************************************/
void `$INSTANCE_NAME`_Enable(void) `=ReentrantKeil($INSTANCE_NAME . "_Enable")`
{
    /* Enable active mode power for ADC */
    `$INSTANCE_NAME`_PWRMGR_DEC_REG |= `$INSTANCE_NAME`_ACT_PWR_DEC_EN;
    `$INSTANCE_NAME`_PWRMGR_DSM_REG |= `$INSTANCE_NAME`_ACT_PWR_DSM_EN;
    
     /* Enable alternative active mode power for ADC */
    `$INSTANCE_NAME`_STBY_PWRMGR_DEC_REG |= `$INSTANCE_NAME`_STBY_PWR_DEC_EN;
    #if (`$INSTANCE_NAME`_PSOC3_ES3 || `$INSTANCE_NAME`_PSOC5_ES2)
    `$INSTANCE_NAME`_STBY_PWRMGR_DSM_REG |= `$INSTANCE_NAME`_STBY_PWR_DSM_EN;
    #endif /* `$INSTANCE_NAME`_PSOC3_ES3 || `$INSTANCE_NAME`_PSOC5_ES2 */

    /* Disable PRES, Enable power to VCMBUF0, REFBUF0 and REFBUF1, enable PRES */
    #if (`$INSTANCE_NAME`_PSOC3_ES3 || `$INSTANCE_NAME`_PSOC5_ES2)
        `$INSTANCE_NAME`_RESET_CR4_REG |= `$INSTANCE_NAME`_IGNORE_PRESA1;
        `$INSTANCE_NAME`_RESET_CR5_REG |= `$INSTANCE_NAME`_IGNORE_PRESA2;
    #elif (`$INSTANCE_NAME`_PSOC5_ES1 || `$INSTANCE_NAME`_PSOC3_ES2)
        `$INSTANCE_NAME`_RESET_CR1_REG |= `$INSTANCE_NAME`_DIS_PRES1;
        `$INSTANCE_NAME`_RESET_CR3_REG |= `$INSTANCE_NAME`_DIS_PRES2;
    #endif /* `$INSTANCE_NAME`_PSOC5_ES1 */
    
    `$INSTANCE_NAME`_DSM_CR17_REG |= (`$INSTANCE_NAME`_DSM_EN_BUF_VREF | `$INSTANCE_NAME`_DSM_EN_BUF_VCM);
    `$INSTANCE_NAME`_DSM_REF0_REG |= `$INSTANCE_NAME`_DSM_EN_BUF_VREF_INN;
    
        /* Wait for 3 microseconds */
    CyDelayUs(3);
    
    #if (`$INSTANCE_NAME`_PSOC3_ES3 || `$INSTANCE_NAME`_PSOC5_ES2)
        `$INSTANCE_NAME`_RESET_CR4_REG &= ~`$INSTANCE_NAME`_IGNORE_PRESA1;
        `$INSTANCE_NAME`_RESET_CR5_REG &= ~`$INSTANCE_NAME`_IGNORE_PRESA2;
        
        `$INSTANCE_NAME`_RESET_CR3_REG = `$INSTANCE_NAME`_EN_PRESA;
    #elif (`$INSTANCE_NAME`_PSOC5_ES1 || `$INSTANCE_NAME`_PSOC3_ES2)
        `$INSTANCE_NAME`_RESET_CR1_REG &= ~`$INSTANCE_NAME`_DIS_PRES1;
        `$INSTANCE_NAME`_RESET_CR3_REG &= ~`$INSTANCE_NAME`_DIS_PRES2;
    #endif /* `$INSTANCE_NAME`_PSOC5_ES1 */
    
    /* Enable negative pumps for DSM  */
    `$INSTANCE_NAME`_PUMP_CR1_REG  |= ( `$INSTANCE_NAME`_PUMP_CR1_CLKSEL | `$INSTANCE_NAME`_PUMP_CR1_FORCE );
 
    /* This is only valid if there is an internal clock */
    #if(`$INSTANCE_NAME`_DEFAULT_INTERNAL_CLK)
        `$INSTANCE_NAME`_PWRMGR_CLK_REG |= `$INSTANCE_NAME`_ACT_PWR_CLK_EN;        
        `$INSTANCE_NAME`_STBY_PWRMGR_CLK_REG |= `$INSTANCE_NAME`_STBY_PWR_CLK_EN;
        
        `$INSTANCE_NAME`_theACLK_Enable();
    #endif /* `$INSTANCE_NAME`_DEFAULT_INTERNAL_CLK */
    
    /* This is only valid if there is an external charge pump clock */
    #if(`$INSTANCE_NAME`_DEFAULT_CHARGE_PUMP_CLOCK)
        `$INSTANCE_NAME`_PWRMGR_CHARGE_PUMP_CLK_REG |= `$INSTANCE_NAME`_ACT_PWR_CHARGE_PUMP_CLK_EN;
        `$INSTANCE_NAME`_STBY_PWRMGR_CHARGE_PUMP_CLK_REG |= `$INSTANCE_NAME`_STBY_PWR_CHARGE_PUMP_CLK_EN;
        
        `$INSTANCE_NAME`_Ext_CP_Clk_Enable();
    #endif /* `$INSTANCE_NAME`_DEFAULT_CHARGE_PUMP_CLOCK */

}


/******************************************************************************* 
* Function Name: `$INSTANCE_NAME`_Start
********************************************************************************
*
* Summary:
*  The start function initializes the Delta Sigma Modulator with the default values, 
*  and sets the power to the given level.  A power level of 0, is the same as executing
*  the stop function.
*
* Parameters:  
*  None
*
* Return: 
*  void 
*
* Global variables:
*  `$INSTANCE_NAME`_initVar:  Used to check the initial configuration,
*  modified when this function is called for the first time.

*
* Reetrance: 
*  No
*
*******************************************************************************/
void `$INSTANCE_NAME`_Start() 
{
    if(`$INSTANCE_NAME`_initVar == 0u)
    {
        `$INSTANCE_NAME`_Init();
        `$INSTANCE_NAME`_initVar = 1u;
    }

    `$INSTANCE_NAME`_Enable();
}


/*******************************************************************************
* Function Name: `$INSTANCE_NAME`_Stop
********************************************************************************
*
* Summary:
*  Stops and powers down ADC to lowest power state.
*
* Parameters:  
*  void
*
* Return: 
*  void 
*
* Reentrance: 
*  Yes
*
*******************************************************************************/
void `$INSTANCE_NAME`_Stop(void) `=ReentrantKeil($INSTANCE_NAME . "_Stop")`
{
    /* Stop conversions */
    `$INSTANCE_NAME`_DEC_CR_REG &= ~`$INSTANCE_NAME`_DEC_START_CONV;
    `$INSTANCE_NAME`_DEC_SR_REG |=  `$INSTANCE_NAME`_DEC_INTR_CLEAR;
    
    /* Disable PRES, Disable power to VCMBUF0, REFBUF0 and REFBUF1, enable PRES */
    #if (`$INSTANCE_NAME`_PSOC3_ES3 || `$INSTANCE_NAME`_PSOC5_ES2)
        `$INSTANCE_NAME`_RESET_CR4_REG |= `$INSTANCE_NAME`_IGNORE_PRESA1;
        `$INSTANCE_NAME`_RESET_CR5_REG |= `$INSTANCE_NAME`_IGNORE_PRESA2;
    #elif (`$INSTANCE_NAME`_PSOC5_ES1 || `$INSTANCE_NAME`_PSOC3_ES2)
        `$INSTANCE_NAME`_RESET_CR1_REG |= `$INSTANCE_NAME`_DIS_PRES1;
        `$INSTANCE_NAME`_RESET_CR3_REG |= `$INSTANCE_NAME`_DIS_PRES2;
    #endif /* `$INSTANCE_NAME`_PSOC5_ES1 */
    
    `$INSTANCE_NAME`_DSM_CR17_REG &= ~(`$INSTANCE_NAME`_DSM_EN_BUF_VREF | `$INSTANCE_NAME`_DSM_EN_BUF_VCM);
    `$INSTANCE_NAME`_DSM_REF0_REG &= ~`$INSTANCE_NAME`_DSM_EN_BUF_VREF_INN;
    
    /* Wait for 3 microseconds. */
    CyDelayUs(3);
    
    #if (`$INSTANCE_NAME`_PSOC3_ES3 || `$INSTANCE_NAME`_PSOC5_ES2)
        `$INSTANCE_NAME`_RESET_CR4_REG &= ~`$INSTANCE_NAME`_IGNORE_PRESA1;
        `$INSTANCE_NAME`_RESET_CR5_REG &= ~`$INSTANCE_NAME`_IGNORE_PRESA2;
        
        /* Enable the press circuit */
        `$INSTANCE_NAME`_RESET_CR3_REG = `$INSTANCE_NAME`_EN_PRESA;
    #elif (`$INSTANCE_NAME`_PSOC5_ES1 || `$INSTANCE_NAME`_PSOC3_ES2)
        `$INSTANCE_NAME`_RESET_CR1_REG &= ~`$INSTANCE_NAME`_DIS_PRES1;
        `$INSTANCE_NAME`_RESET_CR3_REG &= ~`$INSTANCE_NAME`_DIS_PRES2;
    #endif /* `$INSTANCE_NAME`_PSOC5_ES1 */
    
    /* Disable power to the ADC */
    `$INSTANCE_NAME`_PWRMGR_DSM_REG &= ~`$INSTANCE_NAME`_ACT_PWR_DSM_EN;
    `$INSTANCE_NAME`_PWRMGR_DEC_REG &= ~`$INSTANCE_NAME`_ACT_PWR_DEC_EN;
    
    /* Disable alternative active power to the ADC */
    `$INSTANCE_NAME`_STBY_PWRMGR_DEC_REG &= ~`$INSTANCE_NAME`_STBY_PWR_DEC_EN;
    #if (`$INSTANCE_NAME`_PSOC3_ES3 || `$INSTANCE_NAME`_PSOC5_ES2)
    `$INSTANCE_NAME`_STBY_PWRMGR_DSM_REG &= ~`$INSTANCE_NAME`_STBY_PWR_DSM_EN;
    #endif /* `$INSTANCE_NAME`_PSOC3_ES3 || `$INSTANCE_NAME`_PSOC5_ES2 */

   /* Disable negative pumps for DSM  */
    `$INSTANCE_NAME`_PUMP_CR1_REG &= ~(`$INSTANCE_NAME`_PUMP_CR1_CLKSEL | `$INSTANCE_NAME`_PUMP_CR1_FORCE );
    
    /* This is only valid if there is an internal clock */
    #if(`$INSTANCE_NAME`_DEFAULT_INTERNAL_CLK)
        `$INSTANCE_NAME`_PWRMGR_CLK_REG &= ~`$INSTANCE_NAME`_ACT_PWR_CLK_EN;
        `$INSTANCE_NAME`_STBY_PWRMGR_CLK_REG &= ~`$INSTANCE_NAME`_STBY_PWR_CLK_EN;
        
        `$INSTANCE_NAME`_theACLK_Disable();
    #endif
    
    /* This is only valid if there is an external charge pump clock */
    #if(`$INSTANCE_NAME`_DEFAULT_CHARGE_PUMP_CLOCK)
        `$INSTANCE_NAME`_PWRMGR_CHARGE_PUMP_CLK_REG &= ~`$INSTANCE_NAME`_ACT_PWR_CHARGE_PUMP_CLK_EN;
        `$INSTANCE_NAME`_STBY_PWRMGR_CHARGE_PUMP_CLK_REG &= ~`$INSTANCE_NAME`_STBY_PWR_CHARGE_PUMP_CLK_EN;
        
        `$INSTANCE_NAME`_Ext_CP_Clk_Disable();
    #endif /* `$INSTANCE_NAME`_DEFAULT_CHARGE_PUMP_CLOCK */    
}


/*******************************************************************************
* Function Name: `$INSTANCE_NAME`_SetBufferGain
********************************************************************************
*
* Summary:
*  Set input buffer range.
*
* Parameters:  
*  gain:  Two bit value to select a gain of 1, 2, 4, or 8.
*
* Return: 
*  void
*
* Reentrance: 
*  Yes
*
*******************************************************************************/
void `$INSTANCE_NAME`_SetBufferGain(uint8 gain) `=ReentrantKeil($INSTANCE_NAME . "_SetBufferGain")`
{
    uint8 tmpReg;
    tmpReg = `$INSTANCE_NAME`_DSM_BUF1_REG & ~`$INSTANCE_NAME`_DSM_GAIN_MASK;
    tmpReg |= (gain << 2);
    `$INSTANCE_NAME`_DSM_BUF1_REG = tmpReg;
}


/*******************************************************************************
* Function Name: `$INSTANCE_NAME`_SetBufferChop
********************************************************************************
*
* Summary:
*  Sets the Delta Sigma Modulator Buffer chopper mode.
*
* Parameters:  
*  chopen:  If non-zero set the chopper mode.
*  chopFreq:  Chop frequency value.
*
* Return: 
*  void
*
* Reentrance: 
*  Yes
*
*******************************************************************************/
void `$INSTANCE_NAME`_SetBufferChop(uint8 chopen, uint8 chopFreq) `=ReentrantKeil($INSTANCE_NAME . "_SetBufferChop")`
{
    if(chopen != 0)
    {
        `$INSTANCE_NAME`_DSM_BUF2_REG = (`$INSTANCE_NAME`_DSM_BUF_FCHOP_MASK & chopFreq) | `$INSTANCE_NAME`_DSM_BUF_CHOP_EN;
    }
    else
    {
        `$INSTANCE_NAME`_DSM_BUF2_REG = 0x00;
    }
}


/*******************************************************************************
* Function Name: `$INSTANCE_NAME`_StartConvert
********************************************************************************
*
* Summary:
*  Starts ADC conversion using the given mode.
*
* Parameters:  
*  void
*
* Return: 
*  void 
*
* Reentrance: 
*  Yes
*
*******************************************************************************/
void `$INSTANCE_NAME`_StartConvert(void) `=ReentrantKeil($INSTANCE_NAME . "_StartConvert")`
{
    /* Start the conversion */
    `$INSTANCE_NAME`_DEC_CR_REG |= `$INSTANCE_NAME`_DEC_START_CONV;  
}


/*******************************************************************************
* Function Name: `$INSTANCE_NAME`_StopConvert
********************************************************************************
*
* Summary:
*  Starts ADC conversion using the given mode.
*
* Parameters:  
*  void
*
* Return: 
*  void
*
* Global variables:
*  `$INSTANCE_NAME`_convDone:  Modified when conversion is complete for single
*  sample mode with resolution is above 16.
*
* Reentrance: 
*  No
*
*******************************************************************************/
void `$INSTANCE_NAME`_StopConvert(void)
{
    /* Stop all conversions */
    `$INSTANCE_NAME`_DEC_CR_REG &= ~`$INSTANCE_NAME`_DEC_START_CONV; 
    
    /* Software flag for checking conversion complete or not. Will be used when
       resolution is above 16 bits and conversion mode is single sample */
    `$INSTANCE_NAME`_convDone = 1u;
}

/*******************************************************************************
* Function Name: `$INSTANCE_NAME`_IsEndConversion
********************************************************************************
*
* Summary:
*  Queries the ADC to see if conversion is complete
*
* Parameters:  
*  wMode:  Wait mode, 0 if return with answer immediately.
*                     1 if wait until conversion is complete, then return.
*
* Return: 
*  uint8 status:  0 =>  Conversion not complete.
*                 1 =>  Conversion complete.
*
* Global variables:
*  `$INSTANCE_NAME`_convDone:  Used to check whether conversion is complete
*  or not for single sample mode with resolution is above 16
*
* Reentrance: 
*  Yes
*
*******************************************************************************/
uint8 `$INSTANCE_NAME`_IsEndConversion(uint8 wMode) `=ReentrantKeil($INSTANCE_NAME . "_IsEndConversion")`
{
    uint8 status;
        
    /* Check for stop convert if conversion mode is Single Sample with resolution above 16 bit */
    if(`$INSTANCE_NAME`_stopConversion == 1)
    {
        do
        {
            status = `$INSTANCE_NAME`_convDone;
        } while((status != `$INSTANCE_NAME`_DEC_CONV_DONE) && (wMode == `$INSTANCE_NAME`_WAIT_FOR_RESULT));
    }
    else
    {
        do 
        {
            status = `$INSTANCE_NAME`_DEC_SR_REG & `$INSTANCE_NAME`_DEC_CONV_DONE;
        } while((status != `$INSTANCE_NAME`_DEC_CONV_DONE) && (wMode == `$INSTANCE_NAME`_WAIT_FOR_RESULT));
    }
    return(status);
}


/*******************************************************************************
* Function Name: `$INSTANCE_NAME`_GetResult8
********************************************************************************
*
* Summary:
*  Returns an 8-bit result or the LSB of the last conversion.
*
* Parameters:  
*  void
*
* Return: 
*  int8:  ADC result.
*
* Reentrance: 
*  Yes
*
*******************************************************************************/
int8 `$INSTANCE_NAME`_GetResult8( void ) `=ReentrantKeil($INSTANCE_NAME . "_GetResult8")`
{
    return( `$INSTANCE_NAME`_DEC_SAMP_REG );
}


/*******************************************************************************
* Function Name: `$INSTANCE_NAME`_GetResult16
********************************************************************************
*
* Summary:
*  Returns a 16-bit result from the last ADC conversion.
*
* Parameters:  
*   void
*
* Return: 
*  int16:  ADC result.
*
* Reentrance: 
*  Yes
*
*******************************************************************************/
int16 `$INSTANCE_NAME`_GetResult16(void) `=ReentrantKeil($INSTANCE_NAME . "_GetResult16")`
{
    uint16 result;
    result = `$INSTANCE_NAME`_DEC_SAMPM_REG ;
    result = (result << 8 ) | `$INSTANCE_NAME`_DEC_SAMP_REG;
    return( result );
}


/*******************************************************************************
* Function Name: `$INSTANCE_NAME`_GetResult32
********************************************************************************
*
* Summary:
*  Returns an 32-bit result from ADC
*
* Parameters:  
*  void
*
* Return: 
*  int32:  ADC result.
*
* Reentrance: 
*  Yes
*
*******************************************************************************/
int32 `$INSTANCE_NAME`_GetResult32(void) `=ReentrantKeil($INSTANCE_NAME . "_GetResult32")`
{
    uint32 result;

    result = (int8) `$INSTANCE_NAME`_DEC_SAMPH_REG;
    result = (result << 8) | `$INSTANCE_NAME`_DEC_SAMPM_REG;
    result = (result << 8) | `$INSTANCE_NAME`_DEC_SAMP_REG;
    return( result );
}


/*******************************************************************************
* Function Name: `$INSTANCE_NAME`_SetOffset
********************************************************************************
*
* Summary:
*  This function sets the offset for voltage readings.
*
* Parameters:  
*  int32:  offset  Offset in counts
*
* Return: 
*  void
*
* Global variables:
*  `$INSTANCE_NAME`_Offset:  Modified to set the user provided offset. This 
*  variable is used for offset calibration purpose.
*  Affects the `$INSTANCE_NAME`_CountsTo_Volts, 
*  `$INSTANCE_NAME`_CountsTo_mVolts, `$INSTANCE_NAME`_CountsTo_uVolts functions 
*  by subtracting the given offset. 
*
* Reentrance: 
*  No
*
*******************************************************************************/
void `$INSTANCE_NAME`_SetOffset(int32 offset)
{
 
    `$INSTANCE_NAME`_Offset = offset;
}


/*******************************************************************************
* Function Name: `$INSTANCE_NAME`_SetGain
********************************************************************************
*
* Summary:
*  This function sets the ADC gain in counts per volt.
*
* Parameters:  
*  int32:  offset  Offset in counts
*
* Return: 
*  void 
*
* Global variables:
*  `$INSTANCE_NAME`_CountsPerVolt:  modified to set the ADC gain in counts per volt.
*
* Reentrance: 
*  No
*
*******************************************************************************/
void `$INSTANCE_NAME`_SetGain(int32 adcGain)
{
 
    `$INSTANCE_NAME`_CountsPerVolt = adcGain;
}


/*******************************************************************************
* Function Name: `$INSTANCE_NAME`_CountsTo_mVolts
********************************************************************************
*
* Summary:
*  This function converts ADC counts to mVolts.
*
* Parameters:  
*  int32:  adcCounts   Reading from ADC.
*
* Return: 
*  int32:  Result in mVolts
*
* Global variables:
*  `$INSTANCE_NAME`_CountsPerVolt:  used to convert ADC counts to mVolts.
*  `$INSTANCE_NAME`_Offset:  Used as the offset while converting ADC counts to mVolts.
*
* Reentrance: 
*  Yes
*
*******************************************************************************/
int16 `$INSTANCE_NAME`_CountsTo_mVolts( int32 adcCounts) `=ReentrantKeil($INSTANCE_NAME . "_CountsTo_mVolts")`
{

    int32 mVolts = 0;
    int32 A, B;

    /* Subtract ADC offset */
    adcCounts -= `$INSTANCE_NAME`_Offset;

    if(`$INSTANCE_NAME`_DEFAULT_RESOLUTION < 17)
    {
        A = 1000;
        B = 1;
    }
    else
    {
        A = 100;
        B = 10;
    }

    mVolts = ((A * adcCounts) / ((int32)`$INSTANCE_NAME`_CountsPerVolt/B)) ;   

    return( (int16)mVolts );
}


/*******************************************************************************
* Function Name: `$INSTANCE_NAME`_CountsTo_Volts
********************************************************************************
*
* Summary:
*  This function converts ADC counts to Volts
*
* Parameters:  
*  int32:  adcCounts   Reading from ADC.
*
* Return: 
*  float:  Result in Volts
*
* Global variables:
*  `$INSTANCE_NAME`_CountsPerVolt:  used to convert to Volts.
*  `$INSTANCE_NAME`_Offset:  Used as the offset while converting ADC counts to Volts.
*
* Reentrance: 
*  Yes
*
*******************************************************************************/
float `$INSTANCE_NAME`_CountsTo_Volts( int32 adcCounts) `=ReentrantKeil($INSTANCE_NAME . "_CountsTo_Volts")`
{

    float Volts = 0;

    /* Subtract ADC offset */
    adcCounts -= `$INSTANCE_NAME`_Offset;

    Volts = (float)adcCounts / (float)`$INSTANCE_NAME`_CountsPerVolt;   
    
    return( Volts );
}


/*******************************************************************************
* Function Name: `$INSTANCE_NAME`_CountsTo_uVolts
********************************************************************************
*
* Summary:
*  This function converts ADC counts to uVolts
*
* Parameters:  
*  int32:  adcCounts   Reading from ADC.
*
* Return: 
*  int32:  Result in uVolts
*
* Global variables:
*  `$INSTANCE_NAME`_CountsPerVolt:  used to convert ADC counts to mVolts.
*  `$INSTANCE_NAME`_Offset:  Used as the offset while converting ADC counts to mVolts.
*
* Theory: 
* Care must be taken to not exceed the maximum value for a 32 bit signed
* number in the conversion to uVolts and at the same time not loose 
* resolution.
*
* uVolts = ((A * adcCounts) / ((int32)`$INSTANCE_NAME`_CountsPerVolt/B)) ;   
*
*  Resolution       A           B
*   8 - 11      1,000,000         1
*  12 - 14        100,000        10
*  15 - 17         10,000       100
*  18 - 20           1000      1000
*
* Reentrance:
*  Yes
*
*******************************************************************************/
int32 `$INSTANCE_NAME`_CountsTo_uVolts( int32 adcCounts) `=ReentrantKeil($INSTANCE_NAME . "_CountsTo_uVolts")`
{

    int32 uVolts = 0;
    int32 A, B;
    
    if(`$INSTANCE_NAME`_DEFAULT_RESOLUTION < 12)
    {
        A = 1000000;
        B = 1;
    }
    else if(`$INSTANCE_NAME`_DEFAULT_RESOLUTION < 15)
    {
        A = 100000;
        B = 10;
    }
    else if(`$INSTANCE_NAME`_DEFAULT_RESOLUTION < 18)
    {
        A = 10000;
        B = 100;
    }
    else
    {
        A = 1000;
        B = 1000;
    }

    /* Subtract ADC offset */
    adcCounts -= `$INSTANCE_NAME`_Offset;

    uVolts = ((A * adcCounts) / ((int32)`$INSTANCE_NAME`_CountsPerVolt/B)) ;   
  
    return( uVolts );
}


/*******************************************************************************
* Function Name: `$INSTANCE_NAME`_IRQ_Start(void)
********************************************************************************
*
* Summary:
*  Set up the interrupt and enable it. The IRQ_Start() API is included to 
*  support legacy code. The routine has been replaced by the library source 
*  code for interrupts. IRQ_Start() should not be used in new designs.
* 
* Parameters:  
*   void.
*
* Return:
*   void.
*
* Reentrance:
*  No
*
*******************************************************************************/
void `$INSTANCE_NAME`_IRQ_Start(void) `=ReentrantKeil($INSTANCE_NAME . "_IRQ_Start")`
{
    /* For all we know the interrupt is active. */
    CyIntDisable(`$INSTANCE_NAME`_IRQ__INTC_NUMBER );

    /* Set the ISR to point to the ADC_DelSig_1_IRQ Interrupt. */
    CyIntSetVector(`$INSTANCE_NAME`_IRQ__INTC_NUMBER, `$INSTANCE_NAME`_ISR1);

    /* Set the priority. */
    CyIntSetPriority(`$INSTANCE_NAME`_IRQ__INTC_NUMBER, `$INSTANCE_NAME`_IRQ_INTC_PRIOR_NUMBER);

    /* Enable interrupt. */
    CyIntEnable(`$INSTANCE_NAME`_IRQ__INTC_NUMBER);
}


/*******************************************************************************
* Function Name: `$INSTANCE_NAME`_InitConfig(uint8 config)
********************************************************************************
*
* Summary:
*  Initializes all registers based on customizer settings
*
* Parameters:  
*   void
*
* Return: 
*  void
*
* Global variables:
*  `$INSTANCE_NAME`_initVar:  used to set the common registers for the first time.
*  `$INSTANCE_NAME`_CountsPerVolt:  Used to set the default counts per volt value.
*
* Reentrance: 
*  No
*
*******************************************************************************/
void `$INSTANCE_NAME`_InitConfig(uint8 config)
{
    `$INSTANCE_NAME`_stopConversion = 0;
    switch (config)
    {
        case 1:
            `$INSTANCE_NAME`_DEC_CR_REG      = `$INSTANCE_NAME`_DFLT_DEC_CR;      
            `$INSTANCE_NAME`_DEC_SR_REG      = `$INSTANCE_NAME`_DFLT_DEC_SR;      
            `$INSTANCE_NAME`_DEC_SHIFT1_REG  = `$INSTANCE_NAME`_DFLT_DEC_SHIFT1;  
            `$INSTANCE_NAME`_DEC_SHIFT2_REG  = `$INSTANCE_NAME`_DFLT_DEC_SHIFT2;  
            `$INSTANCE_NAME`_DEC_DR2_REG     = `$INSTANCE_NAME`_DFLT_DEC_DR2;     
            `$INSTANCE_NAME`_DEC_DR2H_REG    = `$INSTANCE_NAME`_DFLT_DEC_DR2H;    
            `$INSTANCE_NAME`_DEC_DR1_REG     = `$INSTANCE_NAME`_DFLT_DEC_DR1;     
            `$INSTANCE_NAME`_DEC_OCOR_REG    = `$INSTANCE_NAME`_DFLT_DEC_OCOR;    
            `$INSTANCE_NAME`_DEC_OCORM_REG   = `$INSTANCE_NAME`_DFLT_DEC_OCORM;   
            `$INSTANCE_NAME`_DEC_OCORH_REG   = `$INSTANCE_NAME`_DFLT_DEC_OCORH;   
            `$INSTANCE_NAME`_DEC_COHER_REG   = `$INSTANCE_NAME`_DFLT_DEC_COHER;   
         
            `$INSTANCE_NAME`_DSM_CR0_REG     = `$INSTANCE_NAME`_DFLT_DSM_CR0;     
            `$INSTANCE_NAME`_DSM_CR1_REG     = `$INSTANCE_NAME`_DFLT_DSM_CR1;     
            `$INSTANCE_NAME`_DSM_CR2_REG     = `$INSTANCE_NAME`_DFLT_DSM_CR2;     
            `$INSTANCE_NAME`_DSM_CR3_REG     = `$INSTANCE_NAME`_DFLT_DSM_CR3;     
            `$INSTANCE_NAME`_DSM_CR4_REG     = `$INSTANCE_NAME`_DFLT_DSM_CR4;     
            `$INSTANCE_NAME`_DSM_CR5_REG     = `$INSTANCE_NAME`_DFLT_DSM_CR5;     
            `$INSTANCE_NAME`_DSM_CR6_REG     = `$INSTANCE_NAME`_DFLT_DSM_CR6;     
            `$INSTANCE_NAME`_DSM_CR7_REG     = `$INSTANCE_NAME`_DFLT_DSM_CR7;     
            `$INSTANCE_NAME`_DSM_CR8_REG     = `$INSTANCE_NAME`_DFLT_DSM_CR8;     
            `$INSTANCE_NAME`_DSM_CR9_REG     = `$INSTANCE_NAME`_DFLT_DSM_CR9;     
            `$INSTANCE_NAME`_DSM_CR10_REG    = `$INSTANCE_NAME`_DFLT_DSM_CR10;    
            `$INSTANCE_NAME`_DSM_CR11_REG    = `$INSTANCE_NAME`_DFLT_DSM_CR11;    
            `$INSTANCE_NAME`_DSM_CR12_REG    = `$INSTANCE_NAME`_DFLT_DSM_CR12;    
            `$INSTANCE_NAME`_DSM_CR13_REG    = `$INSTANCE_NAME`_DFLT_DSM_CR13;    
            `$INSTANCE_NAME`_DSM_CR14_REG    = `$INSTANCE_NAME`_DFLT_DSM_CR14;    
            `$INSTANCE_NAME`_DSM_CR15_REG    = `$INSTANCE_NAME`_DFLT_DSM_CR15;    
            `$INSTANCE_NAME`_DSM_CR16_REG    = `$INSTANCE_NAME`_DFLT_DSM_CR16;    
            `$INSTANCE_NAME`_DSM_CR17_REG    = `$INSTANCE_NAME`_DFLT_DSM_CR17;    
            `$INSTANCE_NAME`_DSM_REF0_REG    = `$INSTANCE_NAME`_DFLT_DSM_REF0;    
            `$INSTANCE_NAME`_DSM_REF2_REG    = `$INSTANCE_NAME`_DFLT_DSM_REF2;    
            `$INSTANCE_NAME`_DSM_REF3_REG    = `$INSTANCE_NAME`_DFLT_DSM_REF3;    
        
            `$INSTANCE_NAME`_DSM_BUF0_REG    = `$INSTANCE_NAME`_DFLT_DSM_BUF0;    
            `$INSTANCE_NAME`_DSM_BUF1_REG    = `$INSTANCE_NAME`_DFLT_DSM_BUF1;    
            `$INSTANCE_NAME`_DSM_BUF2_REG    = `$INSTANCE_NAME`_DFLT_DSM_BUF2;    
            `$INSTANCE_NAME`_DSM_BUF3_REG    = `$INSTANCE_NAME`_DFLT_DSM_BUF3;   
            
            /* To select either Vssa or Vref to -ve input of DSM depending on the input
               range selected.
            */
        
            #if(`$INSTANCE_NAME`_DEFAULT_INPUT_MODE)
                #if (`$INSTANCE_NAME`_PSOC3_ES3 || `$INSTANCE_NAME`_PSOC5_ES2)
                    #if (`$INSTANCE_NAME`_DEFAULT_RANGE == `$INSTANCE_NAME`_IR_VSSA_TO_2VREF)
                        `$INSTANCE_NAME`_AMux_Select(1);
                    #else
                        `$INSTANCE_NAME`_AMux_Select(0);
                    #endif /* `$INSTANCE_NAME`_DEFAULT_RANGE == `$INSTANCE_NAME`_IR_VSSA_TO_2VREF) */
                #elif (`$INSTANCE_NAME`_PSOC3_ES2 || `$INSTANCE_NAME`_PSOC5_ES1)
                    `$INSTANCE_NAME`_AMux_Select(0);
                #endif /* `$INSTANCE_NAME`_DEFAULT_RANGE == `$INSTANCE_NAME`_IR_VSSA_TO_2VREF) */
            #endif /* `$INSTANCE_NAME`_DEFAULT_INPUT_MODE */
                        
            if(`$INSTANCE_NAME`_initVar == 0u)
            {
                `$INSTANCE_NAME`_DSM_DEM0_REG    = `$INSTANCE_NAME`_DFLT_DSM_DEM0;    
                `$INSTANCE_NAME`_DSM_DEM1_REG    = `$INSTANCE_NAME`_DFLT_DSM_DEM1;    
                `$INSTANCE_NAME`_DSM_MISC_REG    = `$INSTANCE_NAME`_DFLT_DSM_MISC;    
                `$INSTANCE_NAME`_DSM_CLK_REG    |= `$INSTANCE_NAME`_DFLT_DSM_CLK; 
                `$INSTANCE_NAME`_DSM_REF1_REG    = `$INSTANCE_NAME`_DFLT_DSM_REF1;    
             
                `$INSTANCE_NAME`_DSM_OUT0_REG    = `$INSTANCE_NAME`_DFLT_DSM_OUT0;    
                `$INSTANCE_NAME`_DSM_OUT1_REG    = `$INSTANCE_NAME`_DFLT_DSM_OUT1;   
            }
             
             /* Set the Conversion stop if resolution is above 16 bit and conversion 
               mode is Single sample */
            #if(`$INSTANCE_NAME`_DEFAULT_RESOLUTION > 16 && \
                `$INSTANCE_NAME`_DEFAULT_CONV_MODE == `$INSTANCE_NAME`_MODE_SINGLE_SAMPLE) 
                `$INSTANCE_NAME`_stopConversion = 1;
            #endif
            `$INSTANCE_NAME`_CountsPerVolt = (uint32)`$INSTANCE_NAME`_DFLT_COUNTS_PER_VOLT;
              
            /* Start and set interrupt vector */
            CyIntSetPriority(`$INSTANCE_NAME`_IRQ__INTC_NUMBER, `$INSTANCE_NAME`_IRQ_INTC_PRIOR_NUMBER);
            CyIntSetVector(`$INSTANCE_NAME`_IRQ__INTC_NUMBER, `$INSTANCE_NAME`_ISR1 );
            CyIntEnable(`$INSTANCE_NAME`_IRQ__INTC_NUMBER);

            break;
          
        case 2:
            `$INSTANCE_NAME`_DEC_CR_REG      = `$INSTANCE_NAME`_DFLT_DEC_CR_CFG2;      
            `$INSTANCE_NAME`_DEC_SR_REG      = `$INSTANCE_NAME`_DFLT_DEC_SR_CFG2;      
            `$INSTANCE_NAME`_DEC_SHIFT1_REG  = `$INSTANCE_NAME`_DFLT_DEC_SHIFT1_CFG2;  
            `$INSTANCE_NAME`_DEC_SHIFT2_REG  = `$INSTANCE_NAME`_DFLT_DEC_SHIFT2_CFG2;  
            `$INSTANCE_NAME`_DEC_DR2_REG     = `$INSTANCE_NAME`_DFLT_DEC_DR2_CFG2;     
            `$INSTANCE_NAME`_DEC_DR2H_REG    = `$INSTANCE_NAME`_DFLT_DEC_DR2H_CFG2;    
            `$INSTANCE_NAME`_DEC_DR1_REG     = `$INSTANCE_NAME`_DFLT_DEC_DR1_CFG2;     
            `$INSTANCE_NAME`_DEC_OCOR_REG    = `$INSTANCE_NAME`_DFLT_DEC_OCOR_CFG2;    
            `$INSTANCE_NAME`_DEC_OCORM_REG   = `$INSTANCE_NAME`_DFLT_DEC_OCORM_CFG2;   
            `$INSTANCE_NAME`_DEC_OCORH_REG   = `$INSTANCE_NAME`_DFLT_DEC_OCORH_CFG2;   
            `$INSTANCE_NAME`_DEC_COHER_REG   = `$INSTANCE_NAME`_DFLT_DEC_COHER_CFG2;   
        
            `$INSTANCE_NAME`_DSM_CR0_REG     = `$INSTANCE_NAME`_DFLT_DSM_CR0_CFG2;     
            `$INSTANCE_NAME`_DSM_CR1_REG     = `$INSTANCE_NAME`_DFLT_DSM_CR1_CFG2;     
            `$INSTANCE_NAME`_DSM_CR2_REG     = `$INSTANCE_NAME`_DFLT_DSM_CR2_CFG2;     
            `$INSTANCE_NAME`_DSM_CR3_REG     = `$INSTANCE_NAME`_DFLT_DSM_CR3_CFG2;     
            `$INSTANCE_NAME`_DSM_CR4_REG     = `$INSTANCE_NAME`_DFLT_DSM_CR4_CFG2;     
            `$INSTANCE_NAME`_DSM_CR5_REG     = `$INSTANCE_NAME`_DFLT_DSM_CR5_CFG2;     
            `$INSTANCE_NAME`_DSM_CR6_REG     = `$INSTANCE_NAME`_DFLT_DSM_CR6_CFG2;     
            `$INSTANCE_NAME`_DSM_CR7_REG     = `$INSTANCE_NAME`_DFLT_DSM_CR7_CFG2;     
            `$INSTANCE_NAME`_DSM_CR8_REG     = `$INSTANCE_NAME`_DFLT_DSM_CR8_CFG2;     
            `$INSTANCE_NAME`_DSM_CR9_REG     = `$INSTANCE_NAME`_DFLT_DSM_CR9_CFG2;     
            `$INSTANCE_NAME`_DSM_CR10_REG    = `$INSTANCE_NAME`_DFLT_DSM_CR10_CFG2;    
            `$INSTANCE_NAME`_DSM_CR11_REG    = `$INSTANCE_NAME`_DFLT_DSM_CR11_CFG2;    
            `$INSTANCE_NAME`_DSM_CR12_REG    = `$INSTANCE_NAME`_DFLT_DSM_CR12_CFG2;    
            `$INSTANCE_NAME`_DSM_CR13_REG    = `$INSTANCE_NAME`_DFLT_DSM_CR13_CFG2;    
            `$INSTANCE_NAME`_DSM_CR14_REG    = `$INSTANCE_NAME`_DFLT_DSM_CR14_CFG2;    
            `$INSTANCE_NAME`_DSM_CR15_REG    = `$INSTANCE_NAME`_DFLT_DSM_CR15_CFG2;    
            `$INSTANCE_NAME`_DSM_CR16_REG    = `$INSTANCE_NAME`_DFLT_DSM_CR16_CFG2;    
            `$INSTANCE_NAME`_DSM_CR17_REG    = `$INSTANCE_NAME`_DFLT_DSM_CR17_CFG2;    
            `$INSTANCE_NAME`_DSM_REF0_REG    = `$INSTANCE_NAME`_DFLT_DSM_REF0_CFG2;    
            `$INSTANCE_NAME`_DSM_REF2_REG    = `$INSTANCE_NAME`_DFLT_DSM_REF2_CFG2;    
            `$INSTANCE_NAME`_DSM_REF3_REG    = `$INSTANCE_NAME`_DFLT_DSM_REF3_CFG2;    
        
            `$INSTANCE_NAME`_DSM_BUF0_REG    = `$INSTANCE_NAME`_DFLT_DSM_BUF0_CFG2;    
            `$INSTANCE_NAME`_DSM_BUF1_REG    = `$INSTANCE_NAME`_DFLT_DSM_BUF1_CFG2;    
            `$INSTANCE_NAME`_DSM_BUF2_REG    = `$INSTANCE_NAME`_DFLT_DSM_BUF2_CFG2;    
            `$INSTANCE_NAME`_DSM_BUF3_REG    = `$INSTANCE_NAME`_DFLT_DSM_BUF3_CFG2; 
            
            /* To select either Vssa or Vref to -ve input of DSM depending on the input
               range selected.
            */
            
            #if(`$INSTANCE_NAME`_DEFAULT_INPUT_MODE)
                #if (`$INSTANCE_NAME`_PSOC3_ES3 || `$INSTANCE_NAME`_PSOC5_ES2)
                    #if (`$INSTANCE_NAME`_DEFAULT_INPUT_RANGE_CFG2 == `$INSTANCE_NAME`_IR_VSSA_TO_2VREF)
                        `$INSTANCE_NAME`_AMux_Select(1);
                    #else
                        `$INSTANCE_NAME`_AMux_Select(0);
                    #endif /* `$INSTANCE_NAME`_DEFAULT_RANGE == `$INSTANCE_NAME`_IR_VSSA_TO_2VREF) */
                #elif (`$INSTANCE_NAME`_PSOC3_ES2 || `$INSTANCE_NAME`_PSOC5_ES1)
                    `$INSTANCE_NAME`_AMux_Select(0);
                #endif /* `$INSTANCE_NAME`_DEFAULT_INPUT_RANGE_CFG2 == `$INSTANCE_NAME`_IR_VSSA_TO_2VREF) */
            #endif /* `$INSTANCE_NAME`_DEFAULT_INPUT_MODE */
            
            /* Set the Conversion stop if resolution is above 16 bit and conversion 
               mode is Single sample */
            #if(`$INSTANCE_NAME`_DEFAULT_RESOLUTION_CFG2 > 16 && \
                `$INSTANCE_NAME`_CONVMODE_CFG2 == `$INSTANCE_NAME`_MODE_SINGLE_SAMPLE) 
                `$INSTANCE_NAME`_stopConversion = 1;
            #endif
            
            `$INSTANCE_NAME`_CountsPerVolt = (uint32)`$INSTANCE_NAME`_DFLT_COUNTS_PER_VOLT_CFG2;
            
            /* Start and set interrupt vector */
            CyIntSetPriority(`$INSTANCE_NAME`_IRQ__INTC_NUMBER, `$INSTANCE_NAME`_IRQ_INTC_PRIOR_NUMBER);
            CyIntSetVector(`$INSTANCE_NAME`_IRQ__INTC_NUMBER, `$INSTANCE_NAME`_ISR2 );
            CyIntEnable(`$INSTANCE_NAME`_IRQ__INTC_NUMBER);

            break;
          
        case 3:
            `$INSTANCE_NAME`_DEC_CR_REG      = `$INSTANCE_NAME`_DFLT_DEC_CR_CFG3;      
            `$INSTANCE_NAME`_DEC_SR_REG      = `$INSTANCE_NAME`_DFLT_DEC_SR_CFG3;      
            `$INSTANCE_NAME`_DEC_SHIFT1_REG  = `$INSTANCE_NAME`_DFLT_DEC_SHIFT1_CFG3;  
            `$INSTANCE_NAME`_DEC_SHIFT2_REG  = `$INSTANCE_NAME`_DFLT_DEC_SHIFT2_CFG3;  
            `$INSTANCE_NAME`_DEC_DR2_REG     = `$INSTANCE_NAME`_DFLT_DEC_DR2_CFG3;     
            `$INSTANCE_NAME`_DEC_DR2H_REG    = `$INSTANCE_NAME`_DFLT_DEC_DR2H_CFG3;    
            `$INSTANCE_NAME`_DEC_DR1_REG     = `$INSTANCE_NAME`_DFLT_DEC_DR1_CFG3;     
            `$INSTANCE_NAME`_DEC_OCOR_REG    = `$INSTANCE_NAME`_DFLT_DEC_OCOR_CFG3;    
            `$INSTANCE_NAME`_DEC_OCORM_REG   = `$INSTANCE_NAME`_DFLT_DEC_OCORM_CFG3;   
            `$INSTANCE_NAME`_DEC_OCORH_REG   = `$INSTANCE_NAME`_DFLT_DEC_OCORH_CFG3;   
            `$INSTANCE_NAME`_DEC_COHER_REG   = `$INSTANCE_NAME`_DFLT_DEC_COHER_CFG3;   
         
            `$INSTANCE_NAME`_DSM_CR0_REG     = `$INSTANCE_NAME`_DFLT_DSM_CR0_CFG3;     
            `$INSTANCE_NAME`_DSM_CR1_REG     = `$INSTANCE_NAME`_DFLT_DSM_CR1_CFG3;     
            `$INSTANCE_NAME`_DSM_CR2_REG     = `$INSTANCE_NAME`_DFLT_DSM_CR2_CFG3;     
            `$INSTANCE_NAME`_DSM_CR3_REG     = `$INSTANCE_NAME`_DFLT_DSM_CR3_CFG3;     
            `$INSTANCE_NAME`_DSM_CR4_REG     = `$INSTANCE_NAME`_DFLT_DSM_CR4_CFG3;     
            `$INSTANCE_NAME`_DSM_CR5_REG     = `$INSTANCE_NAME`_DFLT_DSM_CR5_CFG3;     
            `$INSTANCE_NAME`_DSM_CR6_REG     = `$INSTANCE_NAME`_DFLT_DSM_CR6_CFG3;     
            `$INSTANCE_NAME`_DSM_CR7_REG     = `$INSTANCE_NAME`_DFLT_DSM_CR7_CFG3;     
            `$INSTANCE_NAME`_DSM_CR8_REG     = `$INSTANCE_NAME`_DFLT_DSM_CR8_CFG3;     
            `$INSTANCE_NAME`_DSM_CR9_REG     = `$INSTANCE_NAME`_DFLT_DSM_CR9_CFG3;     
            `$INSTANCE_NAME`_DSM_CR10_REG    = `$INSTANCE_NAME`_DFLT_DSM_CR10_CFG3;    
            `$INSTANCE_NAME`_DSM_CR11_REG    = `$INSTANCE_NAME`_DFLT_DSM_CR11_CFG3;    
            `$INSTANCE_NAME`_DSM_CR12_REG    = `$INSTANCE_NAME`_DFLT_DSM_CR12_CFG3;    
            `$INSTANCE_NAME`_DSM_CR13_REG    = `$INSTANCE_NAME`_DFLT_DSM_CR13_CFG3;    
            `$INSTANCE_NAME`_DSM_CR14_REG    = `$INSTANCE_NAME`_DFLT_DSM_CR14_CFG3;    
            `$INSTANCE_NAME`_DSM_CR15_REG    = `$INSTANCE_NAME`_DFLT_DSM_CR15_CFG3;    
            `$INSTANCE_NAME`_DSM_CR16_REG    = `$INSTANCE_NAME`_DFLT_DSM_CR16_CFG3;    
            `$INSTANCE_NAME`_DSM_CR17_REG    = `$INSTANCE_NAME`_DFLT_DSM_CR17_CFG3;    
            `$INSTANCE_NAME`_DSM_REF0_REG    = `$INSTANCE_NAME`_DFLT_DSM_REF0_CFG3;    
            `$INSTANCE_NAME`_DSM_REF2_REG    = `$INSTANCE_NAME`_DFLT_DSM_REF2_CFG3;    
            `$INSTANCE_NAME`_DSM_REF3_REG    = `$INSTANCE_NAME`_DFLT_DSM_REF3_CFG3;    
        
            `$INSTANCE_NAME`_DSM_BUF0_REG    = `$INSTANCE_NAME`_DFLT_DSM_BUF0_CFG3;    
            `$INSTANCE_NAME`_DSM_BUF1_REG    = `$INSTANCE_NAME`_DFLT_DSM_BUF1_CFG3;    
            `$INSTANCE_NAME`_DSM_BUF2_REG    = `$INSTANCE_NAME`_DFLT_DSM_BUF2_CFG3;    
            `$INSTANCE_NAME`_DSM_BUF3_REG    = `$INSTANCE_NAME`_DFLT_DSM_BUF3_CFG3;   
            
            /* To select either Vssa or Vref to -ve input of DSM depending on the input
               range selected.
            */
            
            #if(`$INSTANCE_NAME`_DEFAULT_INPUT_MODE)
                #if (`$INSTANCE_NAME`_PSOC3_ES3 || `$INSTANCE_NAME`_PSOC5_ES2)
                    #if (`$INSTANCE_NAME`_DEFAULT_INPUT_RANGE_CFG3 == `$INSTANCE_NAME`_IR_VSSA_TO_2VREF)
                        `$INSTANCE_NAME`_AMux_Select(1);
                    #else
                        `$INSTANCE_NAME`_AMux_Select(0);
                    #endif /* `$INSTANCE_NAME`_DEFAULT_RANGE == `$INSTANCE_NAME`_IR_VSSA_TO_2VREF) */
                #elif (`$INSTANCE_NAME`_PSOC3_ES2 || `$INSTANCE_NAME`_PSOC5_ES1)
                    `$INSTANCE_NAME`_AMux_Select(0);
                #endif /* `$INSTANCE_NAME`_DEFAULT_INPUT_RANGE_CFG3 == `$INSTANCE_NAME`_IR_VSSA_TO_2VREF) */
            #endif /* `$INSTANCE_NAME`_DEFAULT_INPUT_MODE */
                       
            /* Set the Conversion stop if resolution is above 16 bit and conversion 
               mode is Single sample */
            #if(`$INSTANCE_NAME`_DEFAULT_RESOLUTION_CFG3 > 16 && \
                `$INSTANCE_NAME`_CONVMODE_CFG3 == `$INSTANCE_NAME`_MODE_SINGLE_SAMPLE) 
                `$INSTANCE_NAME`_stopConversion = 1;
            #endif
            
            `$INSTANCE_NAME`_CountsPerVolt = (uint32)`$INSTANCE_NAME`_DFLT_COUNTS_PER_VOLT_CFG3;
            
            /* Start and set interrupt vector */
            CyIntSetPriority(`$INSTANCE_NAME`_IRQ__INTC_NUMBER, `$INSTANCE_NAME`_IRQ_INTC_PRIOR_NUMBER);
            CyIntSetVector(`$INSTANCE_NAME`_IRQ__INTC_NUMBER, `$INSTANCE_NAME`_ISR3 );
            CyIntEnable(`$INSTANCE_NAME`_IRQ__INTC_NUMBER);
            
            break;
          
        case 4:
            `$INSTANCE_NAME`_DEC_CR_REG      = `$INSTANCE_NAME`_DFLT_DEC_CR_CFG4;      
            `$INSTANCE_NAME`_DEC_SR_REG      = `$INSTANCE_NAME`_DFLT_DEC_SR_CFG4;      
            `$INSTANCE_NAME`_DEC_SHIFT1_REG  = `$INSTANCE_NAME`_DFLT_DEC_SHIFT1_CFG4;  
            `$INSTANCE_NAME`_DEC_SHIFT2_REG  = `$INSTANCE_NAME`_DFLT_DEC_SHIFT2_CFG4;  
            `$INSTANCE_NAME`_DEC_DR2_REG     = `$INSTANCE_NAME`_DFLT_DEC_DR2_CFG4;     
            `$INSTANCE_NAME`_DEC_DR2H_REG    = `$INSTANCE_NAME`_DFLT_DEC_DR2H_CFG4;    
            `$INSTANCE_NAME`_DEC_DR1_REG     = `$INSTANCE_NAME`_DFLT_DEC_DR1_CFG4;     
            `$INSTANCE_NAME`_DEC_OCOR_REG    = `$INSTANCE_NAME`_DFLT_DEC_OCOR_CFG4;    
            `$INSTANCE_NAME`_DEC_OCORM_REG   = `$INSTANCE_NAME`_DFLT_DEC_OCORM_CFG4;   
            `$INSTANCE_NAME`_DEC_OCORH_REG   = `$INSTANCE_NAME`_DFLT_DEC_OCORH_CFG4;   
            `$INSTANCE_NAME`_DEC_COHER_REG   = `$INSTANCE_NAME`_DFLT_DEC_COHER_CFG4;   
         
            `$INSTANCE_NAME`_DSM_CR0_REG     = `$INSTANCE_NAME`_DFLT_DSM_CR0_CFG4;     
            `$INSTANCE_NAME`_DSM_CR1_REG     = `$INSTANCE_NAME`_DFLT_DSM_CR1_CFG4;     
            `$INSTANCE_NAME`_DSM_CR2_REG     = `$INSTANCE_NAME`_DFLT_DSM_CR2_CFG4;     
            `$INSTANCE_NAME`_DSM_CR3_REG     = `$INSTANCE_NAME`_DFLT_DSM_CR3_CFG4;     
            `$INSTANCE_NAME`_DSM_CR4_REG     = `$INSTANCE_NAME`_DFLT_DSM_CR4_CFG4;     
            `$INSTANCE_NAME`_DSM_CR5_REG     = `$INSTANCE_NAME`_DFLT_DSM_CR5_CFG4;     
            `$INSTANCE_NAME`_DSM_CR6_REG     = `$INSTANCE_NAME`_DFLT_DSM_CR6_CFG4;     
            `$INSTANCE_NAME`_DSM_CR7_REG     = `$INSTANCE_NAME`_DFLT_DSM_CR7_CFG4;     
            `$INSTANCE_NAME`_DSM_CR8_REG     = `$INSTANCE_NAME`_DFLT_DSM_CR8_CFG4;     
            `$INSTANCE_NAME`_DSM_CR9_REG     = `$INSTANCE_NAME`_DFLT_DSM_CR9_CFG4;     
            `$INSTANCE_NAME`_DSM_CR10_REG    = `$INSTANCE_NAME`_DFLT_DSM_CR10_CFG4;    
            `$INSTANCE_NAME`_DSM_CR11_REG    = `$INSTANCE_NAME`_DFLT_DSM_CR11_CFG4;    
            `$INSTANCE_NAME`_DSM_CR12_REG    = `$INSTANCE_NAME`_DFLT_DSM_CR12_CFG4;    
            `$INSTANCE_NAME`_DSM_CR13_REG    = `$INSTANCE_NAME`_DFLT_DSM_CR13_CFG4;    
            `$INSTANCE_NAME`_DSM_CR14_REG    = `$INSTANCE_NAME`_DFLT_DSM_CR14_CFG4;    
            `$INSTANCE_NAME`_DSM_CR15_REG    = `$INSTANCE_NAME`_DFLT_DSM_CR15_CFG4;    
            `$INSTANCE_NAME`_DSM_CR16_REG    = `$INSTANCE_NAME`_DFLT_DSM_CR16_CFG4;    
            `$INSTANCE_NAME`_DSM_CR17_REG    = `$INSTANCE_NAME`_DFLT_DSM_CR17_CFG4;    
            `$INSTANCE_NAME`_DSM_REF0_REG    = `$INSTANCE_NAME`_DFLT_DSM_REF0_CFG4;    
            `$INSTANCE_NAME`_DSM_REF2_REG    = `$INSTANCE_NAME`_DFLT_DSM_REF2_CFG4;    
            `$INSTANCE_NAME`_DSM_REF3_REG    = `$INSTANCE_NAME`_DFLT_DSM_REF3_CFG4;    
        
            `$INSTANCE_NAME`_DSM_BUF0_REG    = `$INSTANCE_NAME`_DFLT_DSM_BUF0_CFG4;    
            `$INSTANCE_NAME`_DSM_BUF1_REG    = `$INSTANCE_NAME`_DFLT_DSM_BUF1_CFG4;    
            `$INSTANCE_NAME`_DSM_BUF2_REG    = `$INSTANCE_NAME`_DFLT_DSM_BUF2_CFG4;    
            `$INSTANCE_NAME`_DSM_BUF3_REG    = `$INSTANCE_NAME`_DFLT_DSM_BUF3_CFG4;   
            
            /* To select either Vssa or Vref to -ve input of DSM depending on the input
               range selected.
            */
            
            #if(`$INSTANCE_NAME`_DEFAULT_INPUT_MODE)
                #if (`$INSTANCE_NAME`_PSOC3_ES3 || `$INSTANCE_NAME`_PSOC5_ES2)
                    #if (`$INSTANCE_NAME`_DEFAULT_INPUT_RANGE_CFG4 == `$INSTANCE_NAME`_IR_VSSA_TO_2VREF)
                        `$INSTANCE_NAME`_AMux_Select(1);
                    #else
                        `$INSTANCE_NAME`_AMux_Select(0);
                    #endif /* `$INSTANCE_NAME`_DEFAULT_RANGE == `$INSTANCE_NAME`_IR_VSSA_TO_2VREF) */
                #elif (`$INSTANCE_NAME`_PSOC3_ES2 || `$INSTANCE_NAME`_PSOC5_ES1)
                    `$INSTANCE_NAME`_AMux_Select(0);
                #endif /* `$INSTANCE_NAME`_DEFAULT_INPUT_RANGE_CFG4 == `$INSTANCE_NAME`_IR_VSSA_TO_2VREF) */
            #endif /* `$INSTANCE_NAME`_DEFAULT_INPUT_MODE */
                       
            /* Set the Conversion stop if resolution is above 16 bit and conversion 
               mode is Single sample */
            #if(`$INSTANCE_NAME`_DEFAULT_RESOLUTION_CFG4 > 16 && \
                `$INSTANCE_NAME`_CONVMODE_CFG4 == `$INSTANCE_NAME`_MODE_SINGLE_SAMPLE) 
                `$INSTANCE_NAME`_stopConversion = 1;
            #endif
            
            `$INSTANCE_NAME`_CountsPerVolt = (uint32)`$INSTANCE_NAME`_DFLT_COUNTS_PER_VOLT_CFG4;

            /* Start and set interrupt vector */
            CyIntSetPriority(`$INSTANCE_NAME`_IRQ__INTC_NUMBER, `$INSTANCE_NAME`_IRQ_INTC_PRIOR_NUMBER);
            CyIntSetVector(`$INSTANCE_NAME`_IRQ__INTC_NUMBER, `$INSTANCE_NAME`_ISR4 );
            CyIntEnable(`$INSTANCE_NAME`_IRQ__INTC_NUMBER);
            
            break;
    }
}


/*******************************************************************************
* Function Name: `$INSTANCE_NAME`_RoundValue(double busClockFreq, double clockFreq)
********************************************************************************
*
* Summary:
*  Takes the float value and rounds it to an integer value.
*
* Parameters:  
*  value:  float value which is to be converted to integer.
*
* Return: 
*  uint16: rounded integer value.
*
* Reentrance: 
*  Yes
*
*******************************************************************************/
uint16 `$INSTANCE_NAME`_RoundValue(double busClockFreq, double clockFreq) \
                                  `=ReentrantKeil($INSTANCE_NAME . "_RoundValue")`
{
     uint16 x;
     double divider;
     
     divider = busClockFreq / clockFreq;
     if ((divider - (uint16)divider) >= 0.5)
     {
         x = (uint16)divider + 1;
     }
     else
     {
         x = (uint16)divider;
     }
     return x;
}
         

/*******************************************************************************
* Function Name: `$INSTANCE_NAME`_SelectCofiguration(uint8 config, uint8 restart)
********************************************************************************
*
* Summary:
*  Selects the user defined configuration. This API first stops the current ADC
*  and then initializes the registers with the default values for that config. 
*  This also performs calibration by writing GCOR registers with trim values 
*  stored in the flash.
*
* Parameters:  
*  config:  configuration user wants to select.
*
* Return: 
*  void
*
* Reentrance: 
*  Yes
*
*******************************************************************************/
void `$INSTANCE_NAME`_SelectConfiguration(uint8 config, uint8 restart) \
                                              `=ReentrantKeil($INSTANCE_NAME . "_SelectConfiguration")`
{
    uint8 inputRange, resolution;
    uint16 idealGain, clockDivider;    
    uint16 idealOddGain;
    
    /* Stop the ADC  */
    `$INSTANCE_NAME`_Stop();
    
    /* Check whether the config is valid or not */
    if( config <= `$INSTANCE_NAME`_DEFAULT_NUM_CONFIGS)
    {   
        /* Set the  ADC registers based on the configuration */
        `$INSTANCE_NAME`_InitConfig(config);
        
        /* Trim value calculation */
        switch(config)
        {
            case 1:
                inputRange = `$INSTANCE_NAME`_DEFAULT_RANGE;
                resolution = `$INSTANCE_NAME`_DEFAULT_RESOLUTION;
                idealGain = `$INSTANCE_NAME`_DEFAULT_IDEAL_DEC_GAIN;
                idealOddGain = `$INSTANCE_NAME`_DEFAULT_IDEAL_ODDDEC_GAIN;
                clockDivider = `$INSTANCE_NAME`_RoundValue((double)BCLK__BUS_CLK__HZ,
                                                            (double)`$INSTANCE_NAME`_DFLT_CLOCK_FREQ);
            break;
            
            case 2:
                inputRange = `$INSTANCE_NAME`_DEFAULT_INPUT_RANGE_CFG2;
                resolution = `$INSTANCE_NAME`_DEFAULT_RESOLUTION_CFG2;
                idealGain = `$INSTANCE_NAME`_DEFAULT_IDEAL_DEC_GAIN_CFG2;
                idealOddGain = `$INSTANCE_NAME`_DEFAULT_IDEAL_ODDDEC_GAIN_CFG2;
                clockDivider = `$INSTANCE_NAME`_RoundValue((double)BCLK__BUS_CLK__HZ,
                                                            (double)`$INSTANCE_NAME`_DFLT_CLOCK_FREQ_CFG2);
            break;
            
            case 3:
                inputRange = `$INSTANCE_NAME`_DEFAULT_INPUT_RANGE_CFG3;
                resolution = `$INSTANCE_NAME`_DEFAULT_RESOLUTION_CFG3;
                idealGain = `$INSTANCE_NAME`_DEFAULT_IDEAL_DEC_GAIN_CFG3;
                idealOddGain = `$INSTANCE_NAME`_DEFAULT_IDEAL_ODDDEC_GAIN_CFG3;
                clockDivider = `$INSTANCE_NAME`_RoundValue((double)BCLK__BUS_CLK__HZ,
                                                            (double)`$INSTANCE_NAME`_DFLT_CLOCK_FREQ_CFG3);
            break;
            
            case 4:
                inputRange = `$INSTANCE_NAME`_DEFAULT_INPUT_RANGE_CFG4;
                resolution = `$INSTANCE_NAME`_DEFAULT_RESOLUTION_CFG4;
                idealGain = `$INSTANCE_NAME`_DEFAULT_IDEAL_DEC_GAIN_CFG4;
                idealOddGain = `$INSTANCE_NAME`_DEFAULT_IDEAL_ODDDEC_GAIN_CFG4;
                clockDivider = `$INSTANCE_NAME`_RoundValue((double)BCLK__BUS_CLK__HZ,  
                                                            (double)`$INSTANCE_NAME`_DFLT_CLOCK_FREQ_CFG4);
            break;
            
            default:
                inputRange = `$INSTANCE_NAME`_DEFAULT_RANGE;
                resolution = `$INSTANCE_NAME`_DEFAULT_RESOLUTION;
                idealGain = `$INSTANCE_NAME`_DEFAULT_IDEAL_DEC_GAIN;
                idealOddGain = `$INSTANCE_NAME`_DEFAULT_IDEAL_ODDDEC_GAIN;
                clockDivider = `$INSTANCE_NAME`_RoundValue((double)BCLK__BUS_CLK__HZ,
                                                            (double)`$INSTANCE_NAME`_DFLT_CLOCK_FREQ);
            break;
        }
        
        clockDivider = clockDivider - 1;
        /* Set the proper divider for the internal clock */
        #if(`$INSTANCE_NAME`_DEFAULT_INTERNAL_CLK)
            `$INSTANCE_NAME`_theACLK_SetDividerRegister(clockDivider, 1);
        #endif
        
        /* Compensate the gain */
        `$INSTANCE_NAME`_GainCompensation(inputRange, idealGain, idealOddGain, resolution);
        
        if(restart == 1)
        {        
            /* Restart the ADC */
            `$INSTANCE_NAME`_Start();
        
            /* Restart the ADC conversion */
            `$INSTANCE_NAME`_StartConvert();
        }
    }
}     


/*******************************************************************************
* Function Name: `$INSTANCE_NAME`_GainCompensation(uint8, uint16, uint16, uint8)
********************************************************************************
*
* Summary:
*  This API calculates the trim value and then loads this to GCOR register.
*
* Parameters:  
*  inputRange:  input range for which trim value is to be calculated.
*  IdealDecGain:  Ideal Decimator gain for the selected resolution and conversion 
*                 mode.
*  IdealOddDecGain:  Ideal odd decimation gain for the selected resolution and 
                     conversion mode.
*  resolution:  Resolution to select the proper flash location for trim value.
*
* Return: 
*  void
*
* Reentrance: 
*  Yes
*
*******************************************************************************/
void `$INSTANCE_NAME`_GainCompensation(uint8 inputRange, uint16 IdealDecGain, uint16 IdealOddDecGain,  \
                                       uint8 resolution) `=ReentrantKeil($INSTANCE_NAME . "_GainCompensation")`
{
    int8 flash;
    int16 Normalised;
    uint16 GcorValue;
    uint32 GcorTmp;
    
    switch(inputRange)
    {         
        case `$INSTANCE_NAME`_IR_VNEG_VREF_DIFF:
        
        /* Normalize the flash Value */
        if(resolution > 15)
        {
            flash = `$INSTANCE_NAME`_DEC_TRIM_VREF_DIFF_16_20; 
        }    
        else
        {
            flash = `$INSTANCE_NAME`_DEC_TRIM_VREF_DIFF_8_15;
        }        
        break;
        
        case `$INSTANCE_NAME`_IR_VNEG_VREF_2_DIFF:
        
        /* Normalize the flash Value */
        if(resolution > 15)
        {
            flash = `$INSTANCE_NAME`_DEC_TRIM_VREF_2_DIFF_16_20;
        }    
        else
        {
            flash = `$INSTANCE_NAME`_DEC_TRIM_VREF_2_DIFF_8_15;
        }    
        break;
        
        case `$INSTANCE_NAME`_IR_VNEG_VREF_4_DIFF:
        
        /* Normalize the flash Value */
        if(resolution > 15)
        {
            flash = `$INSTANCE_NAME`_DEC_TRIM_VREF_4_DIFF_16_20;
        }    
        else
        {
            flash = `$INSTANCE_NAME`_DEC_TRIM_VREF_4_DIFF_8_15;
        }    
        break;
        
        case `$INSTANCE_NAME`_IR_VNEG_VREF_16_DIFF:
        
        /* Normalize the flash Value */
        if(resolution > 15)
        {
            flash = `$INSTANCE_NAME`_DEC_TRIM_VREF_16_DIFF_16_20;
        }    
        else
        {
            flash = `$INSTANCE_NAME`_DEC_TRIM_VREF_16_DIFF_8_15;
        }    
        break;
        
        default:
            flash = 0;
        break;
    }    
    if(inputRange == `$INSTANCE_NAME`_IR_VSSA_TO_VREF || inputRange == `$INSTANCE_NAME`_IR_VSSA_TO_2VREF || 
       inputRange == `$INSTANCE_NAME`_IR_VSSA_TO_VDDA || inputRange == `$INSTANCE_NAME`_IR_VSSA_TO_6VREF || 
       inputRange == `$INSTANCE_NAME`_IR_VNEG_2VREF_DIFF || inputRange == `$INSTANCE_NAME`_IR_VNEG_6VREF_DIFF ||
       inputRange == `$INSTANCE_NAME`_IR_VNEG_VREF_8_DIFF)
    {
        Normalised  = 0;
    }
    else
    {
        Normalised  = ((int16)flash) * 32;
    }
                    
    /* Add two values */
    GcorValue = IdealDecGain + Normalised;  
    GcorTmp = (uint32)GcorValue * (uint32)IdealOddDecGain;
    GcorValue = (uint16)(GcorTmp / `$INSTANCE_NAME`_IDEAL_GAIN_CONST);
        
    if (resolution < 14)
    {
        GcorValue = (GcorValue >> (15 - (resolution + 1)));
        `$INSTANCE_NAME`_DEC_GVAL_REG = (resolution + 1);
    }
    else
    {
        `$INSTANCE_NAME`_DEC_GVAL_REG = 15;  // Use all 16 bits
    }
      
    /* Load the gain correction register */    
    `$INSTANCE_NAME`_DEC_GCOR_REG  = (uint8)GcorValue;
    `$INSTANCE_NAME`_DEC_GCORH_REG = (uint8)(GcorValue >> 8);    
    
    /* Workaround for 0 to 2*Vref mode with PSoC3 ES2 and PSoC5 ES1 siliocn */
    #if( `$INSTANCE_NAME`_PSOC3_ES2 || `$INSTANCE_NAME`_PSOC5_ES1) 
        if( inputRange == `$INSTANCE_NAME`_IR_VSSA_TO_2VREF)
        {
            `$INSTANCE_NAME`_DEC_GCOR_REG = 0x00;
            `$INSTANCE_NAME`_DEC_GCORH_REG = 0x00;
            `$INSTANCE_NAME`_DEC_GVAL_REG = 0x00;
        }
    #endif
    
}        


/* [] END OF FILE */
