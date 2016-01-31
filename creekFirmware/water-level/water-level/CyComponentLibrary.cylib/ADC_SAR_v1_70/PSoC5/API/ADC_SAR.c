/*******************************************************************************
* File Name: `$INSTANCE_NAME`.c
* Version `$CY_MAJOR_VERSION`.`$CY_MINOR_VERSION`
*
* Description:
*  This file provides the source code to the API for the Successive
*  approximation ADC Component.
*
* Note:
*
*******************************************************************************
* Copyright 2008-2011, Cypress Semiconductor Corporation.  All rights reserved.
* You may use this file only in accordance with the license, terms, conditions,
* disclaimers, and limitations in the end user license agreement accompanying
* the software package with which this file was provided.
*******************************************************************************/

#include "CyLib.h"
#include "`$INSTANCE_NAME`.h"

#if(`$INSTANCE_NAME`_DEFAULT_INTERNAL_CLK)
    #include "`$INSTANCE_NAME`_theACLK.h"
#endif /* End `$INSTANCE_NAME`_DEFAULT_INTERNAL_CLK */


/***************************************
* Forward function references
***************************************/
void `$INSTANCE_NAME`_SetRef(int8 refMode);
void `$INSTANCE_NAME`_CalcGain(uint8 resolution);


/***************************************
* Global data allocation
***************************************/
uint8 `$INSTANCE_NAME`_initVar = 0u;
volatile int16 `$INSTANCE_NAME`_offset;
volatile int16 `$INSTANCE_NAME`_countsPerVolt;   /* Gain compensation */
volatile int16 `$INSTANCE_NAME`_shift;
#if(CY_PSOC5_ES1)
    uint8 `$INSTANCE_NAME`_resolution;
#endif /* End CY_PSOC5_ES1 */


/*******************************************************************************
* Function Name: `$INSTANCE_NAME`_Start
********************************************************************************
*
* Summary:
*  Performs all required initialization for this component and enables the
*  power.
*
* Parameters:
*  None.
*
* Return:
*  None.
*
* Global variables:
*  The `$INSTANCE_NAME`_initVar variable is used to indicate when/if initial 
*  configuration of this component has happened. The variable is initialized to 
*  zero and set to 1 the first time ADC_Start() is called. This allows for 
*  component Re-Start without re-initialization in all subsequent calls to the 
*  `$INSTANCE_NAME`_Start() routine.
*  If re-initialization of the component is required the variable should be set 
*  to zero before call of `$INSTANCE_NAME`_Start() routine, or the user may call 
*  `$INSTANCE_NAME`_Init() and `$INSTANCE_NAME`_Enable() as done in the 
*  `$INSTANCE_NAME`_Start() routine.
*
* Reentrant:
*  No.
*
*******************************************************************************/
void `$INSTANCE_NAME`_Start(void)
{

    /* If not Initialized then initialize all required hardware and software */
    if(`$INSTANCE_NAME`_initVar == 0u)
    {
        `$INSTANCE_NAME`_Init();
        `$INSTANCE_NAME`_initVar = 1u;
    }
    `$INSTANCE_NAME`_Enable();
}


/*******************************************************************************
* Function Name: `$INSTANCE_NAME`_Init
********************************************************************************
*
* Summary:
*  Initialize component's parameters to the parameters set by user in the 
*  customizer of the component placed onto schematic. Usually called in 
*  `$INSTANCE_NAME`_Start().
*
* Parameters:  
*  None.
*
* Return: 
*  None.
*
* Global variables:
*  The `$INSTANCE_NAME`_offset variable is initialized to 0.
*
* Reentrant:
*  No.
*
*******************************************************************************/
void `$INSTANCE_NAME`_Init(void)
{

    /* This is only valid if there is an internal clock */
    #if(`$INSTANCE_NAME`_DEFAULT_INTERNAL_CLK)
        `$INSTANCE_NAME`_theACLK_SetMode(CYCLK_DUTY);
    #endif /* End `$INSTANCE_NAME`_DEFAULT_INTERNAL_CLK */

    /* Start and set interrupt vector */
    CyIntSetPriority(`$INSTANCE_NAME`_INTC_NUMBER, `$INSTANCE_NAME`_INTC_PRIOR_NUMBER);
    CyIntSetVector(`$INSTANCE_NAME`_INTC_NUMBER, `$INSTANCE_NAME`_ISR );

    /* Enable IRQ mode*/
    `$INSTANCE_NAME`_SAR_CSR1_REG |= `$INSTANCE_NAME`_SAR_IRQ_MASK_EN | `$INSTANCE_NAME`_SAR_IRQ_MODE_EDGE;
    
    /*Set SAR ADC resolution ADC */
    `$INSTANCE_NAME`_SetResolution(`$INSTANCE_NAME`_DEFAULT_RESOLUTION);
    `$INSTANCE_NAME`_offset = 0;
}


/*******************************************************************************
* Function Name: `$INSTANCE_NAME`_Enable
********************************************************************************
*  
* Summary: 
*  Enables the reference, clock and power for SAR ADC.
*
* Parameters:  
*  None.
*
* Return: 
*  None.
*
* Reentrant:
*  No.
*
*******************************************************************************/
void `$INSTANCE_NAME`_Enable(void)
{
    uint8 tmpReg;
    uint8 enableInterrupts;
    enableInterrupts = CyEnterCriticalSection();

     /* Enable the SAR ADC block in Active Power Mode */
    `$INSTANCE_NAME`_PWRMGR_SAR_REG |= `$INSTANCE_NAME`_ACT_PWR_SAR_EN;

     /* Enable the SAR ADC in Standby Power Mode*/
    `$INSTANCE_NAME`_STBY_PWRMGR_SAR_REG |= `$INSTANCE_NAME`_STBY_PWR_SAR_EN;

    /* This is only valid if there is an internal clock */
    #if(`$INSTANCE_NAME`_DEFAULT_INTERNAL_CLK)
        `$INSTANCE_NAME`_PWRMGR_CLK_REG |= `$INSTANCE_NAME`_ACT_PWR_CLK_EN;
        `$INSTANCE_NAME`_STBY_PWRMGR_CLK_REG |= `$INSTANCE_NAME`_STBY_PWR_CLK_EN;
    #endif /* End `$INSTANCE_NAME`_DEFAULT_INTERNAL_CLK */

    /*Sets input range of the ADC*/
    #if(`$INSTANCE_NAME`_DEFAULT_RANGE == `$INSTANCE_NAME`__VSS_TO_VREF)
        `$INSTANCE_NAME`_SetRef(`$INSTANCE_NAME`_DEFAULT_RANGE);
    #elif(`$INSTANCE_NAME`_DEFAULT_RANGE == `$INSTANCE_NAME`__VSSA_TO_VDDA)
        `$INSTANCE_NAME`_SAR_CSR3_REG = `$INSTANCE_NAME`_SAR_EN_RESVDA_EN;     /* Set bit for VDD/2 mode */
        `$INSTANCE_NAME`_SetRef(`$INSTANCE_NAME`__VSSA_TO_VDDA);
    #elif(`$INSTANCE_NAME`_DEFAULT_RANGE == `$INSTANCE_NAME`__VSSA_TO_VDAC)
        `$INSTANCE_NAME`_SetRef(`$INSTANCE_NAME`_DEFAULT_RANGE);
    #elif(`$INSTANCE_NAME`_DEFAULT_RANGE == `$INSTANCE_NAME`__VNEG_VREF_DIFF)
        `$INSTANCE_NAME`_SAR_CSR3_REG = `$INSTANCE_NAME`_SAR_EN_CP_EN;         /* Enable charge pump*/
        `$INSTANCE_NAME`_SetRef(`$INSTANCE_NAME`_DEFAULT_RANGE);
    #elif(`$INSTANCE_NAME`_DEFAULT_RANGE == `$INSTANCE_NAME`__VNEG_VDDA_DIFF)
        `$INSTANCE_NAME`_SAR_CSR3_REG = `$INSTANCE_NAME`_SAR_EN_CP_EN;         /* Enable charge pump*/
        `$INSTANCE_NAME`_SetRef(`$INSTANCE_NAME`__VSSA_TO_VDDA);
    #elif(`$INSTANCE_NAME`_DEFAULT_RANGE == `$INSTANCE_NAME`__VNEG_VDDA_2_DIFF)
        `$INSTANCE_NAME`_SAR_CSR3_REG = `$INSTANCE_NAME`_SAR_EN_RESVDA_EN |    /* Set bit for VDD/2 mode*/
                                        `$INSTANCE_NAME`_SAR_EN_CP_EN;         /* Enable charge pump*/
        `$INSTANCE_NAME`_SetRef(`$INSTANCE_NAME`__VSSA_TO_VDDA);
    #elif(`$INSTANCE_NAME`_DEFAULT_RANGE == `$INSTANCE_NAME`__VNEG_VDAC_DIFF)
        `$INSTANCE_NAME`_SetRef(`$INSTANCE_NAME`_DEFAULT_RANGE);
    #endif /* end `$INSTANCE_NAME`_DEFAULT_RANGE */

    /* Set default power */
    tmpReg = (`$INSTANCE_NAME`_DEFAULT_POWER << `$INSTANCE_NAME`_SAR_POWER_SHIFT);
    /* Clear DAC value at beginning of sampling when internal reference used */
    #if(`$INSTANCE_NAME`_DEFAULT_REFERENCE != `$INSTANCE_NAME`__EXT_REF)
        tmpReg |= `$INSTANCE_NAME`_SAR_HIZ_CLEAR;
    #endif /* End `$INSTANCE_NAME`_DEFAULT_REFERENCE != `$INSTANCE_NAME`__EXT_REF */    
    /*Set Convertion mode */
    #if(`$INSTANCE_NAME`_DEFAULT_CONV_MODE == `$INSTANCE_NAME`__TRIGGERED)      /* If triggered mode */
        tmpReg |= `$INSTANCE_NAME`_SAR_MX_SOF_UDB |          /* source: UDB */
                  `$INSTANCE_NAME`_SAR_SOF_MODE_EDGE;        /* Set edge-sensitive sof source */
    #endif /* `$INSTANCE_NAME`_DEFAULT_CONV_MODE */
    `$INSTANCE_NAME`_SAR_CSR0_REG = tmpReg;

    /* Enable clock for SAR ADC*/
    `$INSTANCE_NAME`_SAR_CLK_REG |= `$INSTANCE_NAME`_SAR_MX_CLK_EN;
   
    #if(CY_PSOC5_ES1)
        /* Software Reset */
        `$INSTANCE_NAME`_SAR_CSR0_REG |= `$INSTANCE_NAME`_SAR_RESET_SOFT_ACTIVE;
        CyDelayUs(2); /* 2us delay is required for the lowest 1Mhz clock connected to SAR */
        `$INSTANCE_NAME`_SAR_CSR0_REG &= ~`$INSTANCE_NAME`_SAR_RESET_SOFT_ACTIVE;
    #endif /* End CY_PSOC5_ES1 */

    /* Clear a pending interrupt */
    CyIntClearPending(`$INSTANCE_NAME`_INTC_NUMBER);
    
    CyExitCriticalSection(enableInterrupts);
}


/*******************************************************************************
* Function Name: `$INSTANCE_NAME`_SetRef
********************************************************************************
*
* Summary:
*   Sets reference for ADC
*
* Parameters:
*  refMode: Reference configuration.
*
* Return:
*  None.
*
* Reentrant:
*  No.
*
*******************************************************************************/
void `$INSTANCE_NAME`_SetRef(int8 refMode)
{
    uint8 tmpReg;
    if(refMode == `$INSTANCE_NAME`__VSSA_TO_VDDA)
    {
        #if(`$INSTANCE_NAME`_DEFAULT_REFERENCE == `$INSTANCE_NAME`__INT_REF_NOT_BYPASSED)
            tmpReg = `$INSTANCE_NAME`_SAR_REF_S3_LSB_EN | `$INSTANCE_NAME`_SAR_REF_S4_LSB_EN;
            `$INSTANCE_NAME`_SAR_CSR3_REG |= `$INSTANCE_NAME`_SAR_EN_BUF_VREF_EN; /* Enable Int Ref Amp*/
        #elif(`$INSTANCE_NAME`_DEFAULT_REFERENCE == `$INSTANCE_NAME`__INT_REF_BYPASS)
            tmpReg = `$INSTANCE_NAME`_SAR_REF_S2_LSB_EN | `$INSTANCE_NAME`_SAR_REF_S3_LSB_EN | 
                     `$INSTANCE_NAME`_SAR_REF_S4_LSB_EN;
            `$INSTANCE_NAME`_SAR_CSR3_REG |= `$INSTANCE_NAME`_SAR_EN_BUF_VREF_EN; /* Enable Int Ref Amp*/
        #elif(`$INSTANCE_NAME`_DEFAULT_REFERENCE == `$INSTANCE_NAME`__EXT_REF)
            tmpReg = `$INSTANCE_NAME`_SAR_REF_S2_LSB_EN;
            `$INSTANCE_NAME`_SAR_CSR3_REG &= ~`$INSTANCE_NAME`_SAR_EN_BUF_VREF_EN; /* Disable Int Ref Amp*/
        #endif /*  End `$INSTANCE_NAME`_DEFAULT_REFERENCE */
    }
    else
    {
        #if(`$INSTANCE_NAME`_DEFAULT_REFERENCE == `$INSTANCE_NAME`__INT_REF_NOT_BYPASSED)
            tmpReg = `$INSTANCE_NAME`_SAR_REF_S3_LSB_EN | `$INSTANCE_NAME`_SAR_REF_S4_LSB_EN;
            `$INSTANCE_NAME`_SAR_CSR3_REG |= `$INSTANCE_NAME`_SAR_EN_BUF_VREF_EN; /* Enable Int Ref Amp*/
        #elif(`$INSTANCE_NAME`_DEFAULT_REFERENCE == `$INSTANCE_NAME`__INT_REF_BYPASS)
            tmpReg = `$INSTANCE_NAME`_SAR_REF_S2_LSB_EN | `$INSTANCE_NAME`_SAR_REF_S3_LSB_EN | 
                     `$INSTANCE_NAME`_SAR_REF_S4_LSB_EN;
            `$INSTANCE_NAME`_SAR_CSR3_REG |= `$INSTANCE_NAME`_SAR_EN_BUF_VREF_EN; /* Enable Int Ref Amp*/
        #elif(`$INSTANCE_NAME`_DEFAULT_REFERENCE == `$INSTANCE_NAME`__EXT_REF)
            tmpReg = `$INSTANCE_NAME`_SAR_REF_S2_LSB_EN;
            `$INSTANCE_NAME`_SAR_CSR3_REG &= ~`$INSTANCE_NAME`_SAR_EN_BUF_VREF_EN; /* Disable Int Ref Amp*/
        #endif /*  End `$INSTANCE_NAME`_DEFAULT_REFERENCE */
    }
    `$INSTANCE_NAME`_SAR_CSR6_REG = tmpReg;
    `$INSTANCE_NAME`_SAR_CSR7_REG = `$INSTANCE_NAME`_SAR_REF_S_MSB_DIS;
}


/*******************************************************************************
* Function Name: `$INSTANCE_NAME`_Stop
********************************************************************************
*
* Summary:
*  Stops convertion and reduce the power to the minimum.
*
* Parameters:
*  None.
*
* Return:
*  None.
*
* Reentrant:
*  No.
*
*******************************************************************************/
void `$INSTANCE_NAME`_Stop(void)
{
    uint8 enableInterrupts;
    enableInterrupts = CyEnterCriticalSection();

    /* Stop all conversions */
    `$INSTANCE_NAME`_SAR_CSR0_REG &= ~`$INSTANCE_NAME`_SAR_SOF_START_CONV;
    
    #if(CY_PSOC5_ES1)
        /* Leave the SAR block powered and reduce the power to the minimum */
        `$INSTANCE_NAME`_SAR_CSR0_REG |= `$INSTANCE_NAME`_ICONT_LV_3;
        /* Disable reference buffer and reduce the reference power to the minimum */
        `$INSTANCE_NAME`_SAR_CSR3_REG = `$INSTANCE_NAME`_SAR_PWR_CTRL_VREF_DIV_BY4;    
    #else    
        /* Disable the SAR ADC block in Active Power Mode */
        `$INSTANCE_NAME`_PWRMGR_SAR_REG &= ~`$INSTANCE_NAME`_ACT_PWR_SAR_EN;
        /* Disable the SAR ADC in Standby Power Mode */
        `$INSTANCE_NAME`_STBY_PWRMGR_SAR_REG &= ~`$INSTANCE_NAME`_STBY_PWR_SAR_EN;
        /* Disable power for reference buffer and charge pump*/
        `$INSTANCE_NAME`_SAR_CSR3_REG = `$INSTANCE_NAME`_SAR_EN_BUF_VREF_DIS;    
    #endif /* End CY_PSOC5_ES1 */
    

    /* This is only valid if there is an internal clock */
    #if(`$INSTANCE_NAME`_DEFAULT_INTERNAL_CLK)
        `$INSTANCE_NAME`_PWRMGR_CLK_REG &= ~`$INSTANCE_NAME`_ACT_PWR_CLK_EN;
        `$INSTANCE_NAME`_STBY_PWRMGR_CLK_REG &= ~`$INSTANCE_NAME`_STBY_PWR_CLK_EN;
    #endif /* End `$INSTANCE_NAME`_DEFAULT_INTERNAL_CLK */

    CyExitCriticalSection(enableInterrupts);
    
}


/*******************************************************************************
* Function Name: `$INSTANCE_NAME`_IRQ_Enable
********************************************************************************
* Summary:
*  Enables the interrupt.
*
* Parameters:
*  None.
*
* Return:
*  None.
*
* Reentrant:
*  Yes.
*
*******************************************************************************/
void `$INSTANCE_NAME`_IRQ_Enable(void)
{
    /* Enable the general interrupt. */
    CyIntEnable(`$INSTANCE_NAME`_INTC_NUMBER);
}


/*******************************************************************************
* Function Name: `$INSTANCE_NAME`_IRQ_Disable
********************************************************************************
*
* Summary:
*  Disables the Interrupt.
*
* Parameters:
*  None.
*
* Return:
*  None.
*
* Reentrant:
*  Yes.
*
*******************************************************************************/
void `$INSTANCE_NAME`_IRQ_Disable(void)
{
    /* Disable the general interrupt. */
    CyIntDisable(`$INSTANCE_NAME`_INTC_NUMBER);
}


/*******************************************************************************
* Function Name: `$INSTANCE_NAME`_SetPower
********************************************************************************
*
* Summary:
*  Sets the Power mode.
*
* Parameters:
*  power:  Power setting for ADC
*  0 ->    Normal
*  1 ->    Half power
*  2 ->    1/3rd power
*  3 ->    Quarter power.
*
* Return:
*  None.
*
* Reentrant:
*  Yes.
*
*******************************************************************************/
void `$INSTANCE_NAME`_SetPower(uint8 power)
{
    uint8 tmpReg;

    /* mask off invalid power settings */
    power &= `$INSTANCE_NAME`_SAR_API_POWER_MASK;

    /* Set Power parameter  */
    tmpReg = `$INSTANCE_NAME`_SAR_CSR0_REG & ~`$INSTANCE_NAME`_SAR_POWER_MASK;
    tmpReg |= (power << `$INSTANCE_NAME`_SAR_POWER_SHIFT);
    `$INSTANCE_NAME`_SAR_CSR0_REG = tmpReg;
}


/*******************************************************************************
* Function Name: `$INSTANCE_NAME`_SetResolution
********************************************************************************
*
* Summary:
*  Sets the Relution of the SAR.
*
* Parameters:
*  resolution:
*  12 ->    RES12
*  10 ->    RES10
*  8  ->    RES8
*
* Return:
*  None.
*
* Reentrant:
*  No.
*
* Theory:
*
* Side Effects:
*  This function calls CalcGain procedure to calculate new gain based on 
*  resolution 
*
*******************************************************************************/
void `$INSTANCE_NAME`_SetResolution(uint8 resolution)
{
    uint8 tmpReg;

    /* remember resolution for the GetResult APIs */
    #if(CY_PSOC5_ES1)
        `$INSTANCE_NAME`_resolution = resolution;
    #endif /* End CY_PSOC5_ES1 */
    
    /* Set SAR ADC resolution */
    switch (resolution)
    {
        case `$INSTANCE_NAME`__BITS_12:
            tmpReg = `$INSTANCE_NAME`_SAR_RESOLUTION_12BIT;
            break;
        case `$INSTANCE_NAME`__BITS_10:
            /* Use 12bits for PSoC5 production silicon and shift the 
            *  results for lower resolution in GetResult16() API 
            */
            #if(CY_PSOC5_ES1)
                tmpReg = `$INSTANCE_NAME`_SAR_RESOLUTION_12BIT;
            #else    
                tmpReg = `$INSTANCE_NAME`_SAR_RESOLUTION_10BIT;
            #endif /* End CY_PSOC5_ES1 */
            break;
        case `$INSTANCE_NAME`__BITS_8:
            #if(CY_PSOC5_ES1)
                tmpReg = `$INSTANCE_NAME`_SAR_RESOLUTION_12BIT;
            #else    
                tmpReg = `$INSTANCE_NAME`_SAR_RESOLUTION_8BIT;
            #endif /* End CY_PSOC5_ES1 */
            break;
        default:
            tmpReg = `$INSTANCE_NAME`_SAR_RESOLUTION_12BIT;
            break;
    }
    
     tmpReg |= `$INSTANCE_NAME`_SAR_SAMPLE_WIDTH;   /* 18 conversion cycles @ 12bits + 1 gap*/
    `$INSTANCE_NAME`_SAR_CSR2_REG = tmpReg;
    
     /* Calculate gain for convert counts to volts */
    `$INSTANCE_NAME`_CalcGain(resolution);
}


/*******************************************************************************
* Function Name: `$INSTANCE_NAME`_StartConvert
********************************************************************************
*
* Summary:
*  Starts ADC conversion using the given mode.
*
* Parameters:
*  None.
*
* Return:
*  None.
*
* Reentrant:
*  No.
*
* Theory:
*  Forces the ADC to initiate a conversion. In Free Running mode, the ADC will 
*  run continuously. In a triggered mode the function also acts as a software
*  version of the SOC. Here every conversion has to be triggered by the routine. 
*  This writes into the SOC bit in SAR_CTRL reg.
*
* Side Effects:
*  In a triggered mode the function switches source for SOF from the external 
*  pin to the internal SOF generation. Application should not call StartConvert
*  if external source used for SOF.
*******************************************************************************/
void `$INSTANCE_NAME`_StartConvert(void)
{
    #if(`$INSTANCE_NAME`_DEFAULT_CONV_MODE == `$INSTANCE_NAME`__TRIGGERED)   /* If triggered mode */
        `$INSTANCE_NAME`_SAR_CSR0_REG &= ~`$INSTANCE_NAME`_SAR_MX_SOF_UDB;   /* source: SOF bit */
    #endif /* End `$INSTANCE_NAME`_DEFAULT_CONV_MODE */
    
    /* Start the conversion */
    `$INSTANCE_NAME`_SAR_CSR0_REG |= `$INSTANCE_NAME`_SAR_SOF_START_CONV;
}


/*******************************************************************************
* Function Name: `$INSTANCE_NAME`_StopConvert
********************************************************************************
*
* Summary:
*  Stops ADC conversion using the given mode.
*
* Parameters:
*  None.
*
* Return:
*  None.
*
* Reentrant:
*  No.
*
* Theory:
*  Stops ADC conversion in Free Running mode. 
*  This writes into the SOC bit in SAR_CTRL reg.
*
* Side Effects:
*  In a triggered mode the function set a software version of the SOC to low level
*  and switch SOF source to hardware SOF input.
*  
*******************************************************************************/
void `$INSTANCE_NAME`_StopConvert(void)
{
    /* Stop all conversions */
    `$INSTANCE_NAME`_SAR_CSR0_REG &= ~`$INSTANCE_NAME`_SAR_SOF_START_CONV;

    #if(`$INSTANCE_NAME`_DEFAULT_CONV_MODE == `$INSTANCE_NAME`__TRIGGERED)   /* If triggered mode */
        /* Return source to UDB for hardware SOF signal */
        `$INSTANCE_NAME`_SAR_CSR0_REG |= `$INSTANCE_NAME`_SAR_MX_SOF_UDB;    /* source: UDB */
    #endif /* End `$INSTANCE_NAME`_DEFAULT_CONV_MODE */
    
}


/*******************************************************************************
* Function Name: `$INSTANCE_NAME`_IsEndConversion
********************************************************************************
*
* Summary:
*  Queries the ADC to see if conversion is complete
*
* Parameters:
*  retMode:  Wait mode,
*   0 if return with answer imediately.
*   1 if wait until conversion is complete, then return.
*
* Return:
*  (uint8)  0 =>  Conversion not complete.
*           1 =>  Conversion complete.
*
* Reentrant:
*  No.
*
*******************************************************************************/
uint8 `$INSTANCE_NAME`_IsEndConversion(uint8 retMode)
{
    uint8 status;

    do
    {
        status = `$INSTANCE_NAME`_SAR_CSR1_REG & `$INSTANCE_NAME`_SAR_EOF_1;
    } while ((status != `$INSTANCE_NAME`_SAR_EOF_1) && (retMode == `$INSTANCE_NAME`_WAIT_FOR_RESULT));

    return(status);
}


/*******************************************************************************
* Function Name: `$INSTANCE_NAME`_GetResult8
********************************************************************************
*
* Summary:
*  Returns an 8-bit result or the LSB of the last conversion. 
*  `$INSTANCE_NAME`_IsEndConversion() should be called to verify that the data 
*   sample is ready 
*
* Parameters:
*  None.
*
* Return:
*  ADC result.
*
* Global Variables:
*  `$INSTANCE_NAME`_shift - used to convert the ADC counts to the 2's 
*  compliment form.
*  `$INSTANCE_NAME`_resolution – used to shift the results for lower 
*   resolution.
*
* Reentrant:
*  Yes.
*
*******************************************************************************/
int8 `$INSTANCE_NAME`_GetResult8( void )
{
    
    #if(CY_PSOC5_ES1)

        int16 res;

        res = ((`$INSTANCE_NAME`_SAR_WRK1_REG << 8u ) | `$INSTANCE_NAME`_SAR_WRK0_REG ) - `$INSTANCE_NAME`_shift;
        
        /* Use 12bits for PSoC5 production silicon and shift the results for lower resolution in this API */
        if(`$INSTANCE_NAME`_resolution == `$INSTANCE_NAME`__BITS_10)
        {
            res >>= 2u;
        }
        else if(`$INSTANCE_NAME`_resolution == `$INSTANCE_NAME`__BITS_8)
        {
            res >>= 4u;
        }
        else    /* Do not shift for 12 bits */
        {
        }
        return( (int8)res );
        
    #else
        return( `$INSTANCE_NAME`_SAR_WRK0_REG - (int8)`$INSTANCE_NAME`_shift);
    #endif /* End CY_PSOC5_ES1 */

}


/*******************************************************************************
* Function Name: `$INSTANCE_NAME`_GetResult16
********************************************************************************
*
* Summary:
*  Gets the data available in the SAR DATA registers.
*  `$INSTANCE_NAME`_IsEndConversion() should be called to verify that the data 
*   sample is ready 
*
* Parameters:
*  None.
*
* Return:
*  ADC result. WORD value which has the converted 12bits. In the differential 
*  input mode the SAR ADC outputs digitally converted data in binary offset 
*  scheme, this function converts the data into 2's compliment form. 
*
* Global Variables:
*  `$INSTANCE_NAME`_shift - used to convert the ADC counts to the 2's 
*  compliment form.
*  `$INSTANCE_NAME`_resolution – used to shift the results for lower 
*   resolution.
*
* Reentrant:
*  Yes.
*
*******************************************************************************/
int16 `$INSTANCE_NAME`_GetResult16( void )
{
    int16 res;
    
    res = ((`$INSTANCE_NAME`_SAR_WRK1_REG << 8u ) | `$INSTANCE_NAME`_SAR_WRK0_REG ) - `$INSTANCE_NAME`_shift;

    #if(CY_PSOC5_ES1)
        /* Use 12bits for PSoC5 production silicon and shift the results for lower resolution in this API */
        if(`$INSTANCE_NAME`_resolution == `$INSTANCE_NAME`__BITS_10)
        {
            res >>= 2u;
        }
        else if(`$INSTANCE_NAME`_resolution == `$INSTANCE_NAME`__BITS_8)
        {
            res >>= 4u;
        }
        else    /* Do not shift for 12 bits */
        {
        }
    #endif /* End CY_PSOC5_ES1 */

    return( res );
}


/*******************************************************************************
* Function Name: `$INSTANCE_NAME`_SetOffset
********************************************************************************
*
* Summary:
*  This function sets the offset for voltage readings.
*
* Parameters:
*  int16: Offset in counts
*
* Return:
*  None.
*
* Global Variables:
*  The `$INSTANCE_NAME`_offset variable modified. This variable is used for 
*  offset calibration purpose. 
*  Affects the `$INSTANCE_NAME`_CountsTo_Volts, 
*  `$INSTANCE_NAME`_CountsTo_mVolts, `$INSTANCE_NAME`_CountsTo_uVolts functions 
*  by subtracting the given offset. 
*
* Reentrant:
*  Yes.
*
*******************************************************************************/
void `$INSTANCE_NAME`_SetOffset(int16 offset)
{
    `$INSTANCE_NAME`_offset = offset;
}


/*******************************************************************************
* Function Name: `$INSTANCE_NAME`_CalcGain
********************************************************************************
*
* Summary:
*  This function calculates the ADC gain in counts per volt.
*
* Parameters:
*  uint8: resolution
*
* Return:
*  None.
*
* Global Variables:
*  `$INSTANCE_NAME`_shift variable initialized. This variable is used to 
*  convert the ADC counts to the 2's compliment form. 
*  `$INSTANCE_NAME`_countsPerVolt variable initialized. This variable is used 
*  for gain calibration purpose. 
*
* Reentrant:
*  No.
*
*******************************************************************************/
void `$INSTANCE_NAME`_CalcGain( uint8 resolution )
{
    uint32 counts = `$INSTANCE_NAME`_SAR_WRK_MAX;       /*default 12 bits*/
    uint16 diff_zero = `$INSTANCE_NAME`_SAR_DIFF_SHIFT;
    
    if(resolution == `$INSTANCE_NAME`__BITS_10)
    {
        counts >>= 2u;
        /* Use 12bits for PSoC5 production silicon */
        #if(CY_PSOC5_ES2)
            diff_zero >>= 2u;
        #else  /* To avoid the warning */  
            diff_zero = diff_zero;
        #endif /* End CY_PSOC5_ES2 */
    }
    if(resolution == `$INSTANCE_NAME`__BITS_8)
    {
        counts >>= 4u;
        /* Use 12bits for PSoC5 production silicon */
        #if(CY_PSOC5_ES2)
            diff_zero >>= 4u;
        #else  /* To avoid the warning */  
            diff_zero = diff_zero;
        #endif /* End CY_PSOC5_ES2 */
    }
    counts *= 1000u; /* To avoid float point arithmetic*/

    #if(`$INSTANCE_NAME`_DEFAULT_RANGE == `$INSTANCE_NAME`__VSS_TO_VREF)
        `$INSTANCE_NAME`_countsPerVolt = counts / `$INSTANCE_NAME`_DEFAULT_REF_VOLTAGE_MV / 2;
        `$INSTANCE_NAME`_shift = 0;
    #elif(`$INSTANCE_NAME`_DEFAULT_RANGE == `$INSTANCE_NAME`__VSSA_TO_VDDA)
        `$INSTANCE_NAME`_countsPerVolt = counts / `$INSTANCE_NAME`_DEFAULT_REF_VOLTAGE_MV / 2;
        `$INSTANCE_NAME`_shift = 0;
    #elif(`$INSTANCE_NAME`_DEFAULT_RANGE == `$INSTANCE_NAME`__VSSA_TO_VDAC)
        `$INSTANCE_NAME`_countsPerVolt = counts / `$INSTANCE_NAME`_DEFAULT_REF_VOLTAGE_MV / 2;
        `$INSTANCE_NAME`_shift = 0;
    #elif(`$INSTANCE_NAME`_DEFAULT_RANGE == `$INSTANCE_NAME`__VNEG_VREF_DIFF)
        `$INSTANCE_NAME`_countsPerVolt = counts / `$INSTANCE_NAME`_DEFAULT_REF_VOLTAGE_MV / 2;
        `$INSTANCE_NAME`_shift = diff_zero;
    #elif(`$INSTANCE_NAME`_DEFAULT_RANGE == `$INSTANCE_NAME`__VNEG_VDDA_DIFF)
        `$INSTANCE_NAME`_countsPerVolt = counts / `$INSTANCE_NAME`_DEFAULT_REF_VOLTAGE_MV / 2;
        `$INSTANCE_NAME`_shift = diff_zero;
    #elif(`$INSTANCE_NAME`_DEFAULT_RANGE == `$INSTANCE_NAME`__VNEG_VDDA_2_DIFF)
        `$INSTANCE_NAME`_countsPerVolt = counts / `$INSTANCE_NAME`_DEFAULT_REF_VOLTAGE_MV / 2;
        `$INSTANCE_NAME`_shift = diff_zero;
    #elif(`$INSTANCE_NAME`_DEFAULT_RANGE == `$INSTANCE_NAME`__VNEG_VDAC_DIFF)
        `$INSTANCE_NAME`_countsPerVolt = counts / `$INSTANCE_NAME`_DEFAULT_REF_VOLTAGE_MV / 2;
        `$INSTANCE_NAME`_shift = diff_zero;
    #endif /* End `$INSTANCE_NAME`_DEFAULT_RANGE */    
}


/*******************************************************************************
* Function Name: `$INSTANCE_NAME`_SetGain
********************************************************************************
*
* Summary:
*  This function sets the ADC gain in counts per volt.
*
* Parameters:
*  int16  adcGain  counts per volt
*
* Return:
*  None.
*
* Global Variables:
*  `$INSTANCE_NAME`_countsPerVolt variable modified. This variable is used 
*  for gain calibration purpose. 
*
* Reentrant:
*  No.
*
*******************************************************************************/
void `$INSTANCE_NAME`_SetGain(int16 adcGain)
{
    `$INSTANCE_NAME`_countsPerVolt = adcGain;
}


/*******************************************************************************
* Function Name: `$INSTANCE_NAME`_CountsTo_mVolts
********************************************************************************
*
* Summary:
*  This function converts ADC counts to mVolts
*
* Parameters:
*  int16  adcCounts   Reading from ADC.
*
* Return:
*  int16  Result in mVolts
*
* Global Variables:
*  `$INSTANCE_NAME`_offset variable used.
*  `$INSTANCE_NAME`_countsPerVolt variable used.
*
* Reentrant:
*  Yes.
*
*******************************************************************************/
int16 `$INSTANCE_NAME`_CountsTo_mVolts(int16 adcCounts)
{

    int16 mVolts;

    /* Subtract ADC offset */
    adcCounts -= `$INSTANCE_NAME`_offset;

    mVolts = ( (int32)adcCounts * 1000 ) / `$INSTANCE_NAME`_countsPerVolt ;

    return( mVolts );
}


/*******************************************************************************
* Function Name: `$INSTANCE_NAME`_CountsTo_uVolts
********************************************************************************
*
* Summary:
*  This function converts ADC counts to micro Volts
*
* Parameters:
*  int16  adcCounts   Reading from ADC.
*
* Return:
*  int32  Result in micro Volts
*
* Global Variables:
*  `$INSTANCE_NAME`_offset variable used.
*  `$INSTANCE_NAME`_countsPerVolt variable used.
*
* Reentrant:
*  Yes.
*
*******************************************************************************/
int32 `$INSTANCE_NAME`_CountsTo_uVolts(int16 adcCounts)
{

    int32 uVolts;

    /* Subtract ADC offset */
    adcCounts -= `$INSTANCE_NAME`_offset;
    /* To convert adcCounts to microVolts it is required to be multiplied
    *  on 1 million. It is multiplied on 500000 and later on 2 to 
    *  to avoid 32bit arithmetic overflows. 
    */
    uVolts = (( (int32)adcCounts * 500000 ) / `$INSTANCE_NAME`_countsPerVolt) * 2;

    return( uVolts );
}


/*******************************************************************************
* Function Name: `$INSTANCE_NAME`_CountsTo_Volts
********************************************************************************
*
* Summary:
*  This function converts ADC counts to Volts
*
* Parameters:
*  int16  adcCounts   Reading from ADC.
*
* Return:
*  float  Result in mVolts
*
* Global Variables:
*  `$INSTANCE_NAME`_offset variable used.
*  `$INSTANCE_NAME`_countsPerVolt variable used.
*
* Reentrant:
*  Yes.
*
*******************************************************************************/
float `$INSTANCE_NAME`_CountsTo_Volts(int16 adcCounts)
{
    float volts;

    /* Subtract ADC offset */
    adcCounts -= `$INSTANCE_NAME`_offset;

    volts = (float)adcCounts / (float)`$INSTANCE_NAME`_countsPerVolt;   

    return( volts );
}


/* [] END OF FILE */
