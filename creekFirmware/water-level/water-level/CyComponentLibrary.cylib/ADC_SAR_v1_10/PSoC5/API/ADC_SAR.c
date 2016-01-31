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
* Copyright 2008-2009, Cypress Semiconductor Corporation.  All rights reserved.
* You may use this file only in accordance with the license, terms, conditions,
* disclaimers, and limitations in the end user license agreement accompanying
* the software package with which this file was provided.
*******************************************************************************/

#include "CyLib.h"
#include "`$INSTANCE_NAME`.h"

#if(`$INSTANCE_NAME`_DEFAULT_INTERNAL_CLK)
    #include "`$INSTANCE_NAME`_theACLK.h"
#endif /* End `$INSTANCE_NAME`_DEFAULT_INTERNAL_CLK */

uint8 `$INSTANCE_NAME`_initVar = 0;
int16 `$INSTANCE_NAME`_Offset = 0;
int16 `$INSTANCE_NAME`_CountsPerVolt;   // Gain compensation

void `$INSTANCE_NAME`_SetRef(int8 refMode);
void `$INSTANCE_NAME`_CalcGain(uint8 resolution);


/*******************************************************************************
* Function Name: `$INSTANCE_NAME`_Start
********************************************************************************
*
* Summary:
*  Performs all required initialization for this component and enables the
*  power.
*
* Parameters:
*  void
*
* Return:
*  void
*
*******************************************************************************/
void `$INSTANCE_NAME`_Start(void)
{
    uint8 tmpReg;

    /* If not Initialized then initialize all required hardware and software */
    if(`$INSTANCE_NAME`_initVar == 0)
    {
        `$INSTANCE_NAME`_initVar = 1;
        /* This is only valid if there is an internal clock */
        #if(`$INSTANCE_NAME`_DEFAULT_INTERNAL_CLK)
            `$INSTANCE_NAME`_theACLK_SetMode(CYCLK_DUTY);
        #endif /* End `$INSTANCE_NAME`_DEFAULT_INTERNAL_CLK */

        /* Start and set interrupt vector */
        CyIntSetPriority(`$INSTANCE_NAME`_INTC_NUMBER, `$INSTANCE_NAME`_INTC_PRIOR_NUMBER);
        CyIntSetVector(`$INSTANCE_NAME`_INTC_NUMBER, `$INSTANCE_NAME`_ISR );

        /* default CSR0=0 */
        tmpReg = `$INSTANCE_NAME`_SAR_CSR0;

        /* Set Power parameter  */
        tmpReg |= (`$INSTANCE_NAME`_DEFAULT_POWER << `$INSTANCE_NAME`_SAR_POWER_SHIFT);

        /* Clear DAC value at beginning of sampling */
        tmpReg |= `$INSTANCE_NAME`_SAR_HIZ_CLEAR;

        /*Set Convertion mode */
        if(`$INSTANCE_NAME`_DEFAULT_CONV_MODE)                /* If triggered mode */
        {
            tmpReg |= `$INSTANCE_NAME`_SAR_MX_SOF_UDB;            /* source: UDB */
            tmpReg |= `$INSTANCE_NAME`_SAR_SOF_MODE_EDGE;        /* Set edge-sensitive sof source */
         }
        `$INSTANCE_NAME`_SAR_CSR0 = tmpReg;

        /* Enable IRQ mode*/
        `$INSTANCE_NAME`_SAR_CSR1 |= `$INSTANCE_NAME`_SAR_IRQ_MASK_EN | `$INSTANCE_NAME`_SAR_IRQ_MODE_EDGE;
        
        /*Set SAR ADC resolution ADC */
        `$INSTANCE_NAME`_SetResolution(`$INSTANCE_NAME`_DEFAULT_RESOLUTION);

        /*Sets input range of the ADC*/
        switch (`$INSTANCE_NAME`_DEFAULT_RANGE)
        {
            case `$INSTANCE_NAME`__VSS_TO_VREF:
                `$INSTANCE_NAME`_SetRef(`$INSTANCE_NAME`_DEFAULT_RANGE);
                break;

            case `$INSTANCE_NAME`__VSSA_TO_VDDA:
                /* Set bit for VDD/2 mode */
                `$INSTANCE_NAME`_SAR_CSR3 |= `$INSTANCE_NAME`_SAR_EN_RESVDA_EN;
                `$INSTANCE_NAME`_SetRef(`$INSTANCE_NAME`__VSSA_TO_VDDA);
                break;
            case `$INSTANCE_NAME`__VSSA_TO_VDAC:
                `$INSTANCE_NAME`_SetRef(`$INSTANCE_NAME`__VSSA_TO_VDDA);
                break;

            case `$INSTANCE_NAME`__VNEG_VREF_DIFF:
                `$INSTANCE_NAME`_SAR_CSR3 |= `$INSTANCE_NAME`_SAR_EN_CP_EN;         /* Enable charge pump*/
                `$INSTANCE_NAME`_SAR_CSR3 |= `$INSTANCE_NAME`_SAR_EN_BUF_VCM_EN;    /* Enable VCM reference buffer*/
                `$INSTANCE_NAME`_SetRef(`$INSTANCE_NAME`_DEFAULT_RANGE);
                break;

            case `$INSTANCE_NAME`__VNEG_VDDA_DIFF:
                `$INSTANCE_NAME`_SAR_CSR3 |= `$INSTANCE_NAME`_SAR_EN_CP_EN;         /* Enable charge pump*/
                `$INSTANCE_NAME`_SAR_CSR3 |= `$INSTANCE_NAME`_SAR_EN_BUF_VCM_EN;    /* Enable VCM reference buffer*/
                `$INSTANCE_NAME`_SetRef(`$INSTANCE_NAME`__VSSA_TO_VDDA);
                break;

            case `$INSTANCE_NAME`__VNEG_VDDA_2_DIFF:
                 /* Set bit for VDD/2 mode*/
                `$INSTANCE_NAME`_SAR_CSR3 |= `$INSTANCE_NAME`_SAR_EN_RESVDA_EN;
                `$INSTANCE_NAME`_SAR_CSR3 |= `$INSTANCE_NAME`_SAR_EN_CP_EN;         /* Enable charge pump*/
                `$INSTANCE_NAME`_SAR_CSR3 |= `$INSTANCE_NAME`_SAR_EN_BUF_VCM_EN;    /* Enable VCM reference buffer*/
                `$INSTANCE_NAME`_SetRef(`$INSTANCE_NAME`__VSSA_TO_VDDA);
                break;
            case `$INSTANCE_NAME`__VNEG_VDAC_DIFF:
                `$INSTANCE_NAME`_SetRef(`$INSTANCE_NAME`__VSSA_TO_VDDA);
                break;

            default:
                /* if default case is hit, invalid argument was passed.*/
                break;
         }

    } /* End `$INSTANCE_NAME`_initVar */

    /*`$INSTANCE_NAME`_SAR_TR0 = `$INSTANCE_NAME`_SAR_CAP_TRIM_4;*/ /*Set attenuation capacitor*/

    /* This is only valid if there is an internal clock */
    #if(`$INSTANCE_NAME`_DEFAULT_INTERNAL_CLK)
        `$INSTANCE_NAME`_PWRMGR_CLK |= `$INSTANCE_NAME`_ACT_PWR_CLK_EN;
        `$INSTANCE_NAME`_theACLK_Enable();
    #endif /* End `$INSTANCE_NAME`_DEFAULT_INTERNAL_CLK */

    /* Enable clock for SAR ADC*/
    `$INSTANCE_NAME`_SAR_CLK |= `$INSTANCE_NAME`_SAR_MX_CLK_EN;

     /* Enable power for ADC */
    `$INSTANCE_NAME`_PWRMGR_SAR |= `$INSTANCE_NAME`_ACT_PWR_SAR_EN;
    
    /* Clear a pending interrupt */
    CyIntClearPending(`$INSTANCE_NAME`_INTC_NUMBER);
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
*  void
*
*******************************************************************************/
void `$INSTANCE_NAME`_SetRef(int8 refMode)
{
    uint8 tmpReg;
    if(refMode == `$INSTANCE_NAME`__VSSA_TO_VDDA)
    {
        /* Use internal reference*/
        tmpReg = `$INSTANCE_NAME`_SAR_REF_S3_LSB_EN | `$INSTANCE_NAME`_SAR_REF_S4_LSB_EN;
        /*tmpReg = `$INSTANCE_NAME`_SAR_REF_S4_LSB_EN | `$INSTANCE_NAME`_SAR_REF_S7_LSB_EN;*/   /*IROS need update*/
        `$INSTANCE_NAME`_SAR_CSR3 |= `$INSTANCE_NAME`_SAR_EN_BUF_VREF_EN;    /* Enable Int Ref Amp*/
    }
    else
    {
        switch (`$INSTANCE_NAME`_DEFAULT_REFERENCE)
        {
            case `$INSTANCE_NAME`__INT_REF_NOT_BYPASSED:
                tmpReg = `$INSTANCE_NAME`_SAR_REF_S3_LSB_EN | `$INSTANCE_NAME`_SAR_REF_S4_LSB_EN;
                `$INSTANCE_NAME`_SAR_CSR3 |= `$INSTANCE_NAME`_SAR_EN_BUF_VREF_EN; /* Enable Int Ref Amp*/
                break;
            case `$INSTANCE_NAME`__INT_REF_BYPASS:
                tmpReg = `$INSTANCE_NAME`_SAR_REF_S2_LSB_EN | 
                         `$INSTANCE_NAME`_SAR_REF_S3_LSB_EN | 
                         `$INSTANCE_NAME`_SAR_REF_S4_LSB_EN;
                `$INSTANCE_NAME`_SAR_CSR3 |= `$INSTANCE_NAME`_SAR_EN_BUF_VREF_EN; /* Enable Int Ref Amp*/
                //`$INSTANCE_NAME`_SAR_CSR3 |= `$INSTANCE_NAME`_SAR_EN_BUF_VCM_EN;  /* Enable VCM Ref Amp*/
                break;
            case `$INSTANCE_NAME`__EXT_REF:
                tmpReg = `$INSTANCE_NAME`_SAR_REF_S2_LSB_EN;
                `$INSTANCE_NAME`_SAR_CSR3 &= ~`$INSTANCE_NAME`_SAR_EN_BUF_VREF_EN; /* Disable Int Ref Amp*/
                break;
            default:
                /* if default case is hit, invalid argument was passed.*/
                break;
         }
     }
    `$INSTANCE_NAME`_SAR_CSR6 = tmpReg;
    `$INSTANCE_NAME`_SAR_CSR7 = `$INSTANCE_NAME`_SAR_REF_S_MSB_DIS;

 }


/*******************************************************************************
* Function Name: `$INSTANCE_NAME`_Stop
********************************************************************************
*
* Summary:
*  Stops and powers the SAR ADC component off.
*
* Parameters:
*  void
*
* Return:
*  void
*
*******************************************************************************/
void `$INSTANCE_NAME`_Stop(void)
{
    /* Stop all conversions */
    `$INSTANCE_NAME`_SAR_CSR0 &= ~`$INSTANCE_NAME`_SAR_SOF_START_CONV;

         /* Disable power for ADC */
    `$INSTANCE_NAME`_PWRMGR_SAR &= ~`$INSTANCE_NAME`_ACT_PWR_SAR_EN;

    /* This is only valid if there is an internal clock */
    #if(`$INSTANCE_NAME`_DEFAULT_INTERNAL_CLK)
        `$INSTANCE_NAME`_PWRMGR_CLK &= ~`$INSTANCE_NAME`_ACT_PWR_CLK_EN;
        `$INSTANCE_NAME`_theACLK_Disable();
    #endif /* End `$INSTANCE_NAME`_DEFAULT_INTERNAL_CLK */

}


/*******************************************************************************
* Function Name: `$INSTANCE_NAME`_IRQ_Enable
********************************************************************************
* Summary:
*   Enables the interrupt.
*
*
* Parameters:
*   void.
*
*
* Return:
*   void.
*
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
* Summary:
*   Disables the Interrupt.
*
*
* Parameters:
*   void.
*
*
* Return:
*   void.
*
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
*  void
*
*******************************************************************************/
void `$INSTANCE_NAME`_SetPower(uint8 power)
{
    uint8 tmpReg;

    /* mask off invalid power settings */
    power &= ~`$INSTANCE_NAME`_SAR_API_POWER_MASK;

    /* Set Power parameter  */
    tmpReg = `$INSTANCE_NAME`_SAR_CSR0 & ~`$INSTANCE_NAME`_SAR_POWER_MASK;
    tmpReg |= (power << `$INSTANCE_NAME`_SAR_POWER_SHIFT);
    `$INSTANCE_NAME`_SAR_CSR0 = tmpReg;
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
*  void
*
* Theory:
*
* Side Effects:
*  This function calls CalcGain procedure to calculate new gain based on 
*  resolution 
*******************************************************************************/
void `$INSTANCE_NAME`_SetResolution(uint8 resolution)
{
    uint8 tmpReg;

    /*Set SAR ADC resolution ADC */
    switch (resolution)
    {
        case `$INSTANCE_NAME`__BITS_12:
            tmpReg = `$INSTANCE_NAME`_SAR_RESOLUTION_12BIT;
            break;
        case `$INSTANCE_NAME`__BITS_10:
            tmpReg = `$INSTANCE_NAME`_SAR_RESOLUTION_10BIT;
            break;
        case `$INSTANCE_NAME`__BITS_8:
            tmpReg = `$INSTANCE_NAME`_SAR_RESOLUTION_8BIT;
            break;
        default:
            /* if default case is hit, invalid argument was passed.*/
            break;
    }
    
     tmpReg |= `$INSTANCE_NAME`_SAR_SAMPLE_WIDTH;   /* 17 conversion cycles @ 12bits + 1 gap*/
    `$INSTANCE_NAME`_SAR_CSR2 = tmpReg;
    
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
*  void
*
* Return:
*  void
*
* Theory:
*  Forces the ADC to initiate a conversion. In Free Running mode, the ADC will 
*  run continuously. In a triggered mode the function also acts as a software
*  version of the SOC. Here every conversion has to be triggered by the routine. 
*  This writes into the SOC bit in SAR_CTRL reg.
*
* Side Effects:
*  
*******************************************************************************/
void `$INSTANCE_NAME`_StartConvert(void)
{
    if(`$INSTANCE_NAME`_DEFAULT_CONV_MODE)                /* If triggered mode */
    {
        `$INSTANCE_NAME`_SAR_CSR0 &= ~`$INSTANCE_NAME`_SAR_MX_SOF_UDB;   /* source: SOF bit */
    }
    
    /* Start the conversion */
    `$INSTANCE_NAME`_SAR_CSR0 |= `$INSTANCE_NAME`_SAR_SOF_START_CONV;

}


/*******************************************************************************
* Function Name: `$INSTANCE_NAME`_StopConvert
********************************************************************************
*
* Summary:
*  Stops ADC conversion using the given mode.
*
* Parameters:
*  void
*
* Return:
*  void
*
* Theory:
*  Stops ADC conversion in Free Running mode. 
*  In a triggered mode the function set a software version of the SOC to low level
*  and return SOF source to hardware SOF signal.
*  This writes into the SOC bit in SAR_CTRL reg.
*
* Side Effects:
*  
*******************************************************************************/
void `$INSTANCE_NAME`_StopConvert(void)
{
    /* Stop all conversions */
    `$INSTANCE_NAME`_SAR_CSR0 &= ~`$INSTANCE_NAME`_SAR_SOF_START_CONV;

    if(`$INSTANCE_NAME`_DEFAULT_CONV_MODE)                /* If triggered mode */
    {
        /* Return source to UDB for hardware SOF signal */
        `$INSTANCE_NAME`_SAR_CSR0 |= `$INSTANCE_NAME`_SAR_MX_SOF_UDB;    /* source: UDB */
    }
    
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
*******************************************************************************/
uint8 `$INSTANCE_NAME`_IsEndConversion(uint8 retMode)
{
    uint8 status;

    do
    {
        status = `$INSTANCE_NAME`_SAR_CSR1 & `$INSTANCE_NAME`_SAR_EOF_1;
    } while ((status != `$INSTANCE_NAME`_SAR_EOF_1) && (retMode == `$INSTANCE_NAME`_WAIT_FOR_RESULT));

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
*  ADC result.
*
*******************************************************************************/
int8 `$INSTANCE_NAME`_GetResult8( void )
{
    return( `$INSTANCE_NAME`_SAR_WRK0 );
}


/*******************************************************************************
* Function Name: `$INSTANCE_NAME`_GetResult16
********************************************************************************
*
* Summary:
*  Gets the data available in the SAR DATA registers.
*
* Parameters:
*  void
*
* Return:
*  ADC result. WORD value which has the converted 12bits.
*
*******************************************************************************/
int16 `$INSTANCE_NAME`_GetResult16( void )
{
    return( (`$INSTANCE_NAME`_SAR_WRK1 << 8 ) | `$INSTANCE_NAME`_SAR_WRK0 );
}


/*******************************************************************************
* Function Name: `$INSTANCE_NAME`_SetOffset
********************************************************************************
*
* Summary:
*  This function sets the offset for voltage readings.
*
* Parameters:
*  int16  offset  Offset in counts
*
* Return:
*  void
*
*******************************************************************************/
void `$INSTANCE_NAME`_SetOffset(int16 offset)
{

    `$INSTANCE_NAME`_Offset = offset;
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
*  void
*
*******************************************************************************/
void `$INSTANCE_NAME`_CalcGain( uint8 resolution )
{
    uint32 counts = 0xFFF; /*default 12 bits*/
    uint16 diff_zero = 0x800;
    
    if(resolution == `$INSTANCE_NAME`__BITS_10)
    {
        counts >>= 2;
        diff_zero >>= 2;
    }
    if(resolution == `$INSTANCE_NAME`__BITS_8)
    {
        counts >>= 4;
        diff_zero >>= 4;
    }
    counts *= 1000; /* To avoid float point arithmetic*/

        switch (`$INSTANCE_NAME`_DEFAULT_RANGE)
        {   /*TODO: use float point when it will be available*/
            case `$INSTANCE_NAME`__VSS_TO_VREF:
                `$INSTANCE_NAME`_CountsPerVolt = counts / `$INSTANCE_NAME`_DEFAULT_REF_VOLTAGE_MV / 2;
                `$INSTANCE_NAME`_Offset = 0;
                break;

            case `$INSTANCE_NAME`__VSSA_TO_VDDA:
                `$INSTANCE_NAME`_CountsPerVolt = counts / `$INSTANCE_NAME`_DEFAULT_REF_VOLTAGE_MV;
                `$INSTANCE_NAME`_Offset = 0;
                break;

            case `$INSTANCE_NAME`__VSSA_TO_VDAC:
                `$INSTANCE_NAME`_CountsPerVolt = counts / `$INSTANCE_NAME`_DEFAULT_REF_VOLTAGE_MV / 2;
                `$INSTANCE_NAME`_Offset = 0;
                break;

            case `$INSTANCE_NAME`__VNEG_VREF_DIFF:
                `$INSTANCE_NAME`_CountsPerVolt = counts / `$INSTANCE_NAME`_DEFAULT_REF_VOLTAGE_MV / 2;
                `$INSTANCE_NAME`_Offset = diff_zero;
                break;
                
            case `$INSTANCE_NAME`__VNEG_VDDA_DIFF:
                `$INSTANCE_NAME`_CountsPerVolt = counts / `$INSTANCE_NAME`_DEFAULT_REF_VOLTAGE_MV / 2;
                `$INSTANCE_NAME`_Offset = diff_zero;
                break;

            case `$INSTANCE_NAME`__VNEG_VDDA_2_DIFF:
                `$INSTANCE_NAME`_CountsPerVolt = counts / `$INSTANCE_NAME`_DEFAULT_REF_VOLTAGE_MV;
                `$INSTANCE_NAME`_Offset = diff_zero;
                break;

            case `$INSTANCE_NAME`__VNEG_VDAC_DIFF:
                `$INSTANCE_NAME`_CountsPerVolt = counts / `$INSTANCE_NAME`_DEFAULT_REF_VOLTAGE_MV / 2;
                `$INSTANCE_NAME`_Offset = diff_zero;
                break;

            default:
                /* if default case is hit, invalid argument was passed.*/
                break;
         }
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
*  void
*
*******************************************************************************/
void `$INSTANCE_NAME`_SetGain(int16 adcGain)
{

    `$INSTANCE_NAME`_CountsPerVolt = adcGain;
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
*******************************************************************************/
int16 `$INSTANCE_NAME`_CountsTo_mVolts( int16 adcCounts)
{

    int16 mVolts = 0;

    /* Subtract ADC offset */
    adcCounts -= `$INSTANCE_NAME`_Offset;

    mVolts = ( (int32)adcCounts * 1000 ) / `$INSTANCE_NAME`_CountsPerVolt ;

    return( mVolts );
}


/* [] END OF FILE */
