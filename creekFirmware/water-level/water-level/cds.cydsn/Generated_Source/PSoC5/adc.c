/*******************************************************************************
* File Name: adc.c  
* Version 2.30
*
* Description:
*  This file provides the source code to the API for the Delta-Sigma ADC
*  Component.
*
* Note:
*
*******************************************************************************
* Copyright 2008-2012, Cypress Semiconductor Corporation.  All rights reserved.
* You may use this file only in accordance with the license, terms, conditions, 
* disclaimers, and limitations in the end user license agreement accompanying 
* the software package with which this file was provided.
********************************************************************************/

#include "adc.h"

#if(adc_DEFAULT_INTERNAL_CLK)
    #include "adc_theACLK.h"
#endif /* adc_DEFAULT_INTERNAL_CLK */

#include "adc_Ext_CP_Clk.h"

#if(adc_DEFAULT_INPUT_MODE)
    #include "adc_AMux.h"
#endif /* adc_DEFAULT_INPUT_MODE */

/* Software flag for checking conversion completed or not */
volatile uint8 adc_convDone = 0u;

/* Software flag to stop conversion for single sample conversion mode 
   with resolution above 16 bits */
volatile uint8 adc_stopConversion = 0u;

/* To run the initialization block only at the start up */
uint8 adc_initVar = 0u;

/* To check whether ADC started or not before switching the configuration */
volatile uint8 adc_started = 0u;

/* Flag to hold ADC config number. By default active config is 1. */
volatile uint8 adc_Config = 1u;

volatile int32 adc_Offset = 0u;
volatile int32 adc_CountsPerVolt = (uint32)adc_CFG1_COUNTS_PER_VOLT;

/* Only valid for PSoC5A */
/* Variable to decide whether or not to restore the register values from
    backup structure */
#if (CY_PSOC5A)
    uint8 adc_restoreReg = 0u;
#endif /* CY_PSOC5A */

/* Valid only for PSoC5A silicon */
#if (CY_PSOC5A)
    /* Backup struct for power mode registers */
    static adc_POWERMODE_BACKUP_STRUCT adc_powerModeBackup = 
    {
        /* Initializing the structure fields with power mode registers of 
           config1 */
        adc_CFG1_DSM_CR14,
        adc_CFG1_DSM_CR15,
        adc_CFG1_DSM_CR16,
        adc_CFG1_DSM_CR17,
        adc_CFG1_DSM_REF0,
        adc_CFG1_DSM_BUF0,
        adc_CFG1_DSM_BUF1,
        adc_CFG1_DSM_CLK,
        adc_BYPASS_DISABLED
    };
#endif /* CY_PSOC5A */


/****************************************************************************** 
* Function Name: adc_Init
*******************************************************************************
*
* Summary:
*  Initialize component's parameters to the parameters set by user in the 
*  customizer of the component placed onto schematic. Usually called in 
* adc_Start().
*  
*
* Parameters:  
*  void
*
* Return: 
*  void 
*
*******************************************************************************/
void adc_Init(void) 
{
    /* Initialize the registers with default customizer settings for config1 */
    adc_InitConfig(1);

    /* This is only valid if there is an internal clock */
    #if(adc_DEFAULT_INTERNAL_CLK)
        adc_theACLK_SetMode(CYCLK_DUTY);
    #endif /* adc_DEFAULT_INTERNAL_CLK */
    
    /* Set the duty cycle for charge pump clock */
    adc_Ext_CP_Clk_SetMode(CYCLK_DUTY);

    /* To perform ADC calibration */
    adc_GainCompensation(adc_CFG1_RANGE, 
                                      adc_CFG1_IDEAL_DEC_GAIN, 
                                      adc_CFG1_IDEAL_ODDDEC_GAIN, 
                                      adc_CFG1_RESOLUTION);        
}


/******************************************************************************
* Function Name: adc_Enable
*******************************************************************************
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
*******************************************************************************/
void adc_Enable(void) 
{
    /* Restore the power registers if silicon used is PSoC5A */
    #if (CY_PSOC5A)
        if(adc_restoreReg && 
           (!(adc_powerModeBackup.bypassRestore)))
        {
            adc_RestorePowerRegisters();
            adc_restoreReg = 0u;
        }
        adc_powerModeBackup.bypassRestore = adc_BYPASS_DISABLED;
    #endif /* CY_PSOC5A */

    /* Enable active mode power for ADC */
    adc_PWRMGR_DEC_REG |= adc_ACT_PWR_DEC_EN;
    adc_PWRMGR_DSM_REG |= adc_ACT_PWR_DSM_EN;
    
     /* Enable alternative active mode power for ADC */
    adc_STBY_PWRMGR_DEC_REG |= adc_STBY_PWR_DEC_EN;
    #if (CY_PSOC3 || CY_PSOC5LP)
    adc_STBY_PWRMGR_DSM_REG |= adc_STBY_PWR_DSM_EN;
    #endif /* CY_PSOC3 || CY_PSOC5LP */

    /* Config selected is 1, then before enablign the REFBUF0, REFBUF1 and 
	   VCMBUF's enable and press circuit and then after a delay of 3
	   microseconds, disable the press circuit. */
    if (adc_Config == 1u)
	{
	    /* Disable PRES, Enable power to VCMBUF0, REFBUF0 and REFBUF1, enable 
	       PRES */
	    #if (CY_PSOC3 || CY_PSOC5LP)
	        adc_RESET_CR4_REG |= adc_IGNORE_PRESA1;
	        adc_RESET_CR5_REG |= adc_IGNORE_PRESA2;
	    #elif (CY_PSOC5A)
	        adc_RESET_CR1_REG |= adc_DIS_PRES1;
	        adc_RESET_CR3_REG |= adc_DIS_PRES2;
	    #endif /* CY_PSOC5A */
	    
	    adc_DSM_CR17_REG |= (adc_DSM_EN_BUF_VREF | adc_DSM_EN_BUF_VCM);
	    #if ((adc_CFG1_REFERENCE == adc_EXT_REF_ON_P03) || \
	         (adc_CFG1_REFERENCE == adc_EXT_REF_ON_P32))
	        adc_DSM_CR17_REG &= ~adc_DSM_EN_BUF_VREF;
	    #endif /* Check for exteranl reference option */
	    #if ((adc_CFG1_RANGE == adc_IR_VSSA_TO_2VREF) && \
	          (CY_PSOC3 || CY_PSOC5LP) && \
	          ((adc_CFG1_REFERENCE != adc_EXT_REF_ON_P03) && \
	         (adc_CFG1_REFERENCE != adc_EXT_REF_ON_P32)))
	        adc_DSM_REF0_REG |= adc_DSM_EN_BUF_VREF_INN;
	    #endif /* adc_IR_VSSA_TO_2VREF */
	    
	        /* Wait for 3 microseconds */
	    CyDelayUs(3);
        /* Enable the press circuit */
	    #if (CY_PSOC3 || CY_PSOC5LP)
	        adc_RESET_CR4_REG &= ~adc_IGNORE_PRESA1;
	        adc_RESET_CR5_REG &= ~adc_IGNORE_PRESA2;
	    #elif (CY_PSOC5A)
	        adc_RESET_CR1_REG &= ~adc_DIS_PRES1;
	        adc_RESET_CR3_REG &= ~adc_DIS_PRES2;
	    #endif /* CY_PSOC5A */
	}
    
    /* Enable negative pumps for DSM  */
    adc_PUMP_CR1_REG  |= ( adc_PUMP_CR1_CLKSEL | adc_PUMP_CR1_FORCE );
 
    /* This is only valid if there is an internal clock */
    #if(adc_DEFAULT_INTERNAL_CLK)
        adc_PWRMGR_CLK_REG |= adc_ACT_PWR_CLK_EN;        
        adc_STBY_PWRMGR_CLK_REG |= adc_STBY_PWR_CLK_EN;
    #endif /* adc_DEFAULT_INTERNAL_CLK */
    
    /* Enable the active and alternate active power for charge pump clock */
    adc_PWRMGR_CHARGE_PUMP_CLK_REG |= adc_ACT_PWR_CHARGE_PUMP_CLK_EN;
    adc_STBY_PWRMGR_CHARGE_PUMP_CLK_REG |= adc_STBY_PWR_CHARGE_PUMP_CLK_EN;
        
}


#if (CY_PSOC5A) /* valid only for PSoC5A silicon */
    /******************************************************************************* 
    * Function Name: adc_RestorePowerRegisters
    ********************************************************************************
    *
    * Summary: 
    *  Restores the power registers on calling Start() API. This API is only 
    *   for internal use and valid only for PSoC5A.
    *  
    *
    * Parameters:  
    *  void
    *
    * Return: 
    *  void 
    *
    ***************************************************************************/
    void adc_RestorePowerRegisters(void) 
    {
        adc_DSM_CR14_REG = adc_powerModeBackup.DSM_CR_14;
        adc_DSM_CR15_REG = adc_powerModeBackup.DSM_CR_15;
        adc_DSM_CR16_REG = adc_powerModeBackup.DSM_CR_16;
        adc_DSM_CR17_REG = adc_powerModeBackup.DSM_CR_17;
        adc_DSM_REF0_REG = adc_powerModeBackup.DSM_REF0;
        adc_DSM_BUF0_REG = adc_powerModeBackup.DSM_BUF0;
        adc_DSM_BUF1_REG = adc_powerModeBackup.DSM_BUF1;
        adc_DSM_CLK_REG = adc_powerModeBackup.DSM_CLK;
    }


    /************************************************************************** 
    * Function Name: adc_SavePowerRegisters
    ***************************************************************************
    *
    * Summary: 
    *  Save the power registers before stopping the block operation. This is 
    *  called by Stop() API. This API is only for internal use and valid only 
    *  for PSoC5A.
    *  
    *
    * Parameters:  
    *  void
    *
    * Return: 
    *  void 
    *
    **************************************************************************/
    void adc_SavePowerRegisters(void) 
    {
        adc_powerModeBackup.DSM_CR_14 = adc_DSM_CR14_REG;
        adc_powerModeBackup.DSM_CR_15 = adc_DSM_CR15_REG;
        adc_powerModeBackup.DSM_CR_16 = adc_DSM_CR16_REG;
        adc_powerModeBackup.DSM_CR_17 = adc_DSM_CR17_REG;
        adc_powerModeBackup.DSM_REF0 = adc_DSM_REF0_REG;
        adc_powerModeBackup.DSM_BUF0 = adc_DSM_BUF0_REG;
        adc_powerModeBackup.DSM_BUF1 = adc_DSM_BUF1_REG;
        adc_powerModeBackup.DSM_CLK = adc_DSM_CLK_REG;
    }


    /************************************************************************* 
    * Function Name: adc_SetLowPower
    ***************************************************************************
    *
    * Summary: 
    *  Set all the power registers of DSM block to low power mode. This API is
    *   called by Stop() API. This API is only for internal use and valid for 
    *   only PSoC5A.
    *  
    *
    * Parameters:  
    *  void
    *
    * Return: 
    *  void 
    *
    ***************************************************************************/
    void adc_SetLowPower(void) 
    {
        adc_DSM_CR14_REG &= ~adc_DSM_POWER1_MASK;
        adc_DSM_CR14_REG |= adc_DSM_POWER1_44UA;
        
        adc_DSM_CR15_REG &= ~(adc_DSM_POWER2_3_MASK | adc_DSM_POWER_COMP_MASK);
        adc_DSM_CR15_REG |= (adc_DSM_POWER2_3_LOW | adc_DSM_POWER_VERYLOW);
        
        adc_DSM_CR16_REG &= ~(adc_DSM_CP_PWRCTL_MASK | adc_DSM_POWER_SUM_MASK |
                                           adc_DSM_CP_ENABLE);
        adc_DSM_CR16_REG |= (adc_DSM_POWER_SUM_LOW | adc_DSM_CP_PWRCTL_LOW);
        
        adc_DSM_CR17_REG &= ~(adc_DSM_EN_BUF_VREF | adc_DSM_PWR_CTRL_VCM_MASK |
                                           adc_DSM_PWR_CTRL_VREF_MASK | adc_DSM_EN_BUF_VCM |
                                           adc_DSM_PWR_CTRL_VREF_INN_MASK);
        adc_DSM_CR17_REG |= (adc_DSM_PWR_CTRL_VREF_0 | adc_DSM_PWR_CTRL_VCM_0 |
                                           adc_DSM_PWR_CTRL_VREF_INN_0);
        
        /* Disable reference buffers */
        adc_DSM_REF0_REG &= ~(adc_DSM_EN_BUF_VREF_INN | adc_DSM_VREF_RES_DIV_EN |
                                           adc_DSM_VCM_RES_DIV_EN);
            
        /* Disable the positive input buffer */
        adc_DSM_BUF0_REG &= ~adc_DSM_ENABLE_P;
        /* Disable the negative input buffer */
        adc_DSM_BUF1_REG &= ~adc_DSM_ENABLE_N;
        /* Disable the clock to DSM */
        adc_DSM_CLK_REG &= ~(adc_DSM_CLK_CLK_EN | adc_DSM_CLK_BYPASS_SYNC);
    }
#endif /* CY_PSOC5A */


/******************************************************************************* 
* Function Name: adc_Start
********************************************************************************
*
* Summary:
*  The start function initializes the Delta Sigma Modulator with the default  
*  values, and sets the power to the given level.  A power level of 0, is the 
*  same as executing the stop function.
*
* Parameters:  
*  None
*
* Return: 
*  void 
*
* Global variables:
*  adc_initVar:  Used to check the initial configuration,
*  modified when this function is called for the first time.
*
*******************************************************************************/
void adc_Start() 
{
    if(adc_initVar == 0u)
    {
        if(!(adc_started))
        {
            adc_Init();
        }
        adc_initVar = 1u;
    }

    /* Enable the ADC */
    adc_Enable();
}


/*******************************************************************************
* Function Name: adc_Stop
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
*******************************************************************************/
void adc_Stop(void) 
{
    /* Stop conversions */
    adc_DEC_CR_REG &= ~adc_DEC_START_CONV;
    adc_DEC_SR_REG |=  adc_DEC_INTR_CLEAR;
    
    /* Disable PRES, Disable power to VCMBUF0, REFBUF0 and REFBUF1, 
       enable PRES */
    #if (CY_PSOC3 || CY_PSOC5LP)
        adc_RESET_CR4_REG |= adc_IGNORE_PRESA1;
        adc_RESET_CR5_REG |= adc_IGNORE_PRESA2;
    #elif (CY_PSOC5A)
        adc_RESET_CR1_REG |= adc_DIS_PRES1;
        adc_RESET_CR3_REG |= adc_DIS_PRES2;
    #endif /* CY_PSOC5A */
    
    adc_DSM_CR17_REG &= ~(adc_DSM_EN_BUF_VREF | adc_DSM_EN_BUF_VCM);
    adc_DSM_REF0_REG &= ~adc_DSM_EN_BUF_VREF_INN;
    
    /* Wait for 3 microseconds. */
    CyDelayUs(3);
    
	/* Enable the press circuit */
    #if (CY_PSOC3 || CY_PSOC5LP)
        adc_RESET_CR4_REG &= ~adc_IGNORE_PRESA1;
        adc_RESET_CR5_REG &= ~adc_IGNORE_PRESA2;
    #elif (CY_PSOC5A)
        adc_RESET_CR1_REG &= ~adc_DIS_PRES1;
        adc_RESET_CR3_REG &= ~adc_DIS_PRES2;
    #endif /* CY_PSOC5A */
    
    /* If PSoC5A then don't disable the power instead put the block to  
       low power mode. Also, save current state of all the power configuration 
       registers before putting block to low power mode */
    #if (CY_PSOC5A)
        
        /* set the flag */
        adc_restoreReg = 1u;
        
        adc_SavePowerRegisters();
        adc_SetLowPower();
    #else    
        /* Disable power to the ADC */
        adc_PWRMGR_DSM_REG &= ~adc_ACT_PWR_DSM_EN;
    #endif /* CY_PSOC5A */
    
    /* Disable power to Decimator block */
    adc_PWRMGR_DEC_REG &= ~adc_ACT_PWR_DEC_EN;
    
    /* Disable alternative active power to the ADC */
    adc_STBY_PWRMGR_DEC_REG &= ~adc_STBY_PWR_DEC_EN;
    #if (CY_PSOC3 || CY_PSOC5LP)
    adc_STBY_PWRMGR_DSM_REG &= ~adc_STBY_PWR_DSM_EN;
    #endif /* CY_PSOC3 || CY_PSOC5LP */

   /* Disable negative pumps for DSM  */
    adc_PUMP_CR1_REG &= ~(adc_PUMP_CR1_CLKSEL | adc_PUMP_CR1_FORCE );
    
    /* This is only valid if there is an internal clock */
    #if(adc_DEFAULT_INTERNAL_CLK)
        adc_PWRMGR_CLK_REG &= ~adc_ACT_PWR_CLK_EN;
        adc_STBY_PWRMGR_CLK_REG &= ~adc_STBY_PWR_CLK_EN;
    #endif /* adc_DEFAULT_INTERNAL_CLK */
    
    /* Disable power to charge pump clock */
    adc_PWRMGR_CHARGE_PUMP_CLK_REG &= ~adc_ACT_PWR_CHARGE_PUMP_CLK_EN;
    adc_STBY_PWRMGR_CHARGE_PUMP_CLK_REG &= ~adc_STBY_PWR_CHARGE_PUMP_CLK_EN;
}


/*******************************************************************************
* Function Name: adc_SetBufferGain
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
*******************************************************************************/
void adc_SetBufferGain(uint8 gain) 
{
    uint8 tmpReg;
    tmpReg = adc_DSM_BUF1_REG & ~adc_DSM_GAIN_MASK;
    tmpReg |= (gain << 2);
    adc_DSM_BUF1_REG = tmpReg;
}


/*******************************************************************************
* Function Name: adc_SetCoherency
********************************************************************************
*
* Summary:
*  Sets the ADC output register coherency bit.
*
* Parameters:  
*  gain:  Two bit value to set the coherency bit.
          00-Coherency checking off
          01-low byte is key byte
          02-middle byte is the key byte
          03-high byte is the key byte
*
* Return: 
*  void
*
* Side Effects:  If the coherency is changed, for any reason, it should be 
*                changed back to the LSB when the provided "GetResult" API 
*                is used.
*
*******************************************************************************/
void adc_SetCoherency(uint8 coherency) 
{
    uint8 tmpReg;    
    tmpReg = adc_DEC_COHER_REG & ~adc_DEC_SAMP_KEY_MASK;
    tmpReg |= coherency;
    adc_DEC_COHER_REG = tmpReg;
}


/*******************************************************************************
* Function Name: adc_SetGCOR
********************************************************************************
*
* Summary:
*  Calculates a new GCOR value and writes it into the GCOR register.
*
* Parameters:  
*  gainAdjust:  floating point value to set GCOR registers.
*
* Return: 
*  uint8:  0-if GCOR value is within the expected range.
           1- if GCOR is outside the expected range.
*
* Side Effects:  The GVAL register is set to the amount of valid bits in the
*                GCOR  register minus one. If GVAL is 15 (0x0F), all 16 bits
*                of the GCOR registers will be valid. If for example GVAL is 
*                11 (0x0B) only 12 bits will be valid. The least 4 bits will
*                be lost when the GCOR value is shifted 4 places to the right.
*
******************************************************************************/
uint8 adc_SetGCOR(float gainAdjust) 
{
    uint16 tmpReg;
    uint8 status;
    float tmpValue;
    
    tmpReg = adc_DEC_GCORH_REG;
    tmpReg = (tmpReg << 8) | adc_DEC_GCOR_REG;
    tmpValue = ((float)tmpReg / adc_IDEAL_GAIN_CONST);
    tmpValue = tmpValue * gainAdjust;
    
    if (tmpValue > 1.9999)
    {
        status = 1;
    }
    else
    {
        tmpReg = (uint16)(tmpValue * adc_IDEAL_GAIN_CONST);
        adc_DEC_GCOR_REG = (uint8)tmpReg;
        adc_DEC_GCORH_REG = (uint8) (tmpReg >> 8);
        status = 0;
    }
    
    return status;
}


/******************************************************************************
* Function Name: adc_ReadGCOR
*******************************************************************************
*
* Summary:
*  This API returns the current GCOR register value, normalized based on the 
*  GVAL register settings.
*
* Parameters:  
*  void
*
* Return: 
*  uint16:  Normalized GCOR value.
*
* Side Effects:  If the GVAL register is set to a value greater than 0x0F, then
                 it gives unexpected value.
*
*******************************************************************************/
uint16 adc_ReadGCOR(void) 
{
    uint8 gValue;
    uint16 gcorValue, gcorRegValue;
    
    gValue = adc_DEC_GVAL_REG;
    gcorRegValue = CY_GET_REG16(adc_DEC_GCOR_PTR);
    
    switch (gValue)
    {
        case adc_GVAL_RESOLUTIN_8:
            gcorValue = gcorRegValue << (adc_MAX_GVAL - gValue);
            break;
        
        case adc_GVAL_RESOLUTIN_9:
            gcorValue = gcorRegValue << (adc_MAX_GVAL - gValue);
            break;
            
        case adc_GVAL_RESOLUTIN_10:
            gcorValue = gcorRegValue << (adc_MAX_GVAL - gValue);
            break;
            
        case adc_GVAL_RESOLUTIN_11:
            gcorValue = gcorRegValue << (adc_MAX_GVAL - gValue);
            break;
            
        case adc_GVAL_RESOLUTIN_12:
            gcorValue = gcorRegValue << (adc_MAX_GVAL - gValue);
            break;
            
        case adc_GVAL_RESOLUTIN_13:
            gcorValue = gcorRegValue << (adc_MAX_GVAL - gValue);
            break;
            
        default:
            gcorValue = gcorRegValue;
            break;
    }
        
    return gcorValue;
}


/*******************************************************************************
* Function Name: adc_SetBufferChop
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
*******************************************************************************/
void adc_SetBufferChop(uint8 chopen, uint8 chopFreq) 
{
    if(chopen != 0u)
    {
        adc_DSM_BUF2_REG = (adc_DSM_BUF_FCHOP_MASK & chopFreq) | \
                                         adc_DSM_BUF_CHOP_EN;
    }
    else
    {
        adc_DSM_BUF2_REG = 0x00u;
    }
}


/*******************************************************************************
* Function Name: adc_StartConvert
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
*******************************************************************************/
void adc_StartConvert(void) 
{
    /* Start the conversion */
    adc_DEC_CR_REG |= adc_DEC_START_CONV;  
}


/*******************************************************************************
* Function Name: adc_StopConvert
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
*  adc_convDone:  Modified when conversion is complete for single
*   sample mode with resolution is above 16.
*
*******************************************************************************/
void adc_StopConvert(void) 
{
    /* Stop all conversions */
    adc_DEC_CR_REG &= ~adc_DEC_START_CONV; 
    
    /* Software flag for checking conversion complete or not. Will be used when
       resolution is above 16 bits and conversion mode is single sample */
    adc_convDone = 1u;
}


/*******************************************************************************
* Function Name: adc_IsEndConversion
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
*  adc_convDone:  Used to check whether conversion is complete
*  or not for single sample mode with resolution is above 16
*
*******************************************************************************/
uint8 adc_IsEndConversion(uint8 wMode) 
{
    uint8 status;
        
    /* Check for stop convert if conversion mode is Single Sample with 
       resolution above 16 bit */
    if(adc_stopConversion == 1u)
    {
        do
        {
            status = adc_convDone;
        } while((status != adc_DEC_CONV_DONE) && (wMode == adc_WAIT_FOR_RESULT));
    }
    else
    {
        do 
        {
            status = adc_DEC_SR_REG & adc_DEC_CONV_DONE;
        } while((status != adc_DEC_CONV_DONE) && (wMode == adc_WAIT_FOR_RESULT));
    }
    return(status);
}


/*******************************************************************************
* Function Name: adc_GetResult8
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
*******************************************************************************/
int8 adc_GetResult8( void ) 
{
     return( adc_DEC_SAMP_REG );
}


/*******************************************************************************
* Function Name: adc_GetResult16
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
*******************************************************************************/
int16 adc_GetResult16(void) 
{
    uint16 result;
    
    #if (CY_PSOC3)
        result = adc_DEC_SAMPM_REG ;
        result = (result << 8 ) | adc_DEC_SAMP_REG;
    #else
        result = (CY_GET_REG16(adc_DEC_SAMP_PTR));
    #endif /* CY_PSOC3 */
    
    return result;
}


/*******************************************************************************
* Function Name: adc_GetResult32
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
*******************************************************************************/
int32 adc_GetResult32(void) 
{
    uint32 result;

    #if (CY_PSOC3)
        /* to make upper bits fo result to 0 */
        result = (int8) (0xff & adc_DEC_SAMPH_REG); 
        result = (result << 8) | adc_DEC_SAMPM_REG;
        result = (result << 8) | adc_DEC_SAMP_REG;
    #else
        result = CY_GET_REG16(adc_DEC_SAMPH_PTR);
        result = (result << 16) | (CY_GET_REG16(adc_DEC_SAMP_PTR));
    #endif /* CY_PSOC3 */
    
    return result;
}


/*******************************************************************************
* Function Name: adc_SetOffset
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
*  adc_Offset:  Modified to set the user provided offset. This 
*  variable is used for offset calibration purpose.
*  Affects the adc_CountsTo_Volts, 
*  adc_CountsTo_mVolts, adc_CountsTo_uVolts functions 
*  by subtracting the given offset. 
*
*******************************************************************************/
void adc_SetOffset(int32 offset) 
{
 
    adc_Offset = offset;
}


/*******************************************************************************
* Function Name: adc_SetGain
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
*  adc_CountsPerVolt:  modified to set the ADC gain in counts 
*   per volt.
*
*******************************************************************************/
void adc_SetGain(int32 adcGain) 
{
 
    adc_CountsPerVolt = adcGain;
}


/*******************************************************************************
* Function Name: adc_CountsTo_mVolts
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
*  adc_CountsPerVolt:  used to convert ADC counts to mVolts.
*  adc_Offset:  Used as the offset while converting ADC counts 
*   to mVolts.
*
*******************************************************************************/
int16 adc_CountsTo_mVolts( int32 adcCounts) 
{

    int32 mVolts = 0;
    int32 A, B;
	uint8 resolution = 16;

    /* Subtract ADC offset */
    adcCounts -= adc_Offset;
	
	/* Set the resolution based on the configuration */
	if (adc_Config == adc_CFG1)
	{
        resolution = adc_CFG1_RESOLUTION;
	}	
	else if (adc_Config == adc_CFG2)
	{
	    resolution = adc_CFG2_RESOLUTION;
	}
	else if (adc_Config == adc_CFG3)
	{
	    resolution = adc_CFG3_RESOLUTION;
	}
	else
	{
	    resolution = adc_CFG4_RESOLUTION;
	}
	
    if(resolution < 17)
    {
        A = 1000;
        B = 1;
    }
    else
    {
        A = 100;
        B = 10;
    }

    mVolts = ((A * adcCounts) / ((int32)adc_CountsPerVolt/B)) ;   

    return( (int16)mVolts );
}


/*******************************************************************************
* Function Name: adc_CountsTo_Volts
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
*  adc_CountsPerVolt:  used to convert to Volts.
*  adc_Offset:  Used as the offset while converting ADC counts 
*   to Volts.
*
*******************************************************************************/
float adc_CountsTo_Volts( int32 adcCounts) 
{

    float Volts = 0;

    /* Subtract ADC offset */
    adcCounts -= adc_Offset;

    Volts = (float)adcCounts / (float)adc_CountsPerVolt;   
    
    return( Volts );
}


/*******************************************************************************
* Function Name: adc_CountsTo_uVolts
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
*  adc_CountsPerVolt:  used to convert ADC counts to mVolts.
*  adc_Offset:  Used as the offset while converting ADC counts 
*   to mVolts.
*
* Theory: 
* Care must be taken to not exceed the maximum value for a 32 bit signed
* number in the conversion to uVolts and at the same time not loose 
* resolution.
*
* uVolts = ((A * adcCounts) / ((int32)adc_CountsPerVolt/B)) ;   
*
*  Resolution       A           B
*   8 - 11      1,000,000         1
*  12 - 14        100,000        10
*  15 - 17         10,000       100
*  18 - 20           1000      1000
*
*******************************************************************************/
int32 adc_CountsTo_uVolts( int32 adcCounts) 
{

    int32 uVolts = 0;
    int32 A, B;
	uint8 resolution = 16;
	
	/* Set the resolution based on the configuration */
	if (adc_Config == adc_CFG1)
	{
        resolution = adc_CFG1_RESOLUTION;
	}	
	else if (adc_Config == adc_CFG2)
	{
	    resolution = adc_CFG2_RESOLUTION;
	}
	else if (adc_Config == adc_CFG3)
	{
	    resolution = adc_CFG3_RESOLUTION;
	}
	else
	{
	    resolution = adc_CFG4_RESOLUTION;
	}
    
    if(resolution < 12)
    {
        A = 1000000;
        B = 1;
    }
    else if(resolution < 15)
    {
        A = 100000;
        B = 10;
    }
    else if(resolution < 18)
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
    adcCounts -= adc_Offset;

    uVolts = ((A * adcCounts) / ((int32)adc_CountsPerVolt/B)) ;   
  
    return( uVolts );
}


/*******************************************************************************
* Function Name: adc_IRQ_Start(void)
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
*******************************************************************************/
void adc_IRQ_Start(void) 
{
    /* For all we know the interrupt is active. */
    CyIntDisable(adc_IRQ__INTC_NUMBER );

    /* Set the ISR to point to the ADC_DelSig_1_IRQ Interrupt. */
    CyIntSetVector(adc_IRQ__INTC_NUMBER, adc_ISR1);

    /* Set the priority. */
    CyIntSetPriority(adc_IRQ__INTC_NUMBER, adc_IRQ_INTC_PRIOR_NUMBER);

    /* Enable interrupt. */
    CyIntEnable(adc_IRQ__INTC_NUMBER);
}


/*******************************************************************************
* Function Name: adc_InitConfig(uint8 config)
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
*  adc_initVar:  used to set the common registers in the beginning.
*  adc_CountsPerVolt:  Used to set the default counts per volt.
*
*******************************************************************************/
void adc_InitConfig(uint8 config) 
{
    adc_stopConversion = 0u;
    
    if(adc_initVar == 0u)
        {
            adc_DSM_DEM0_REG    = adc_CFG1_DSM_DEM0;    
            adc_DSM_DEM1_REG    = adc_CFG1_DSM_DEM1;    
            adc_DSM_MISC_REG    = adc_CFG1_DSM_MISC;    
            adc_DSM_CLK_REG    |= adc_CFG1_DSM_CLK; 
            adc_DSM_REF1_REG    = adc_CFG1_DSM_REF1;    
        
            adc_DSM_OUT0_REG    = adc_CFG1_DSM_OUT0;    
            adc_DSM_OUT1_REG    = adc_CFG1_DSM_OUT1;   
        
            adc_DSM_CR0_REG     = adc_CFG1_DSM_CR0;     
            adc_DSM_CR1_REG     = adc_CFG1_DSM_CR1;     
            adc_DSM_CR2_REG     = adc_CFG1_DSM_CR2;     
            adc_DSM_CR3_REG     = adc_CFG1_DSM_CR3;     
            adc_DSM_CR13_REG    = adc_CFG1_DSM_CR13;     
            
            adc_DEC_SR_REG      = adc_CFG1_DEC_SR;      
            adc_DEC_COHER_REG   = adc_CFG1_DEC_COHER;   
        }

    if (config == 1u)
    {
        /* Default Config */   
        adc_DEC_CR_REG      = adc_CFG1_DEC_CR;      
        adc_DEC_SHIFT1_REG  = adc_CFG1_DEC_SHIFT1;  
        adc_DEC_SHIFT2_REG  = adc_CFG1_DEC_SHIFT2;  
        adc_DEC_DR2_REG     = adc_CFG1_DEC_DR2;     
        adc_DEC_DR2H_REG    = adc_CFG1_DEC_DR2H;    
        adc_DEC_DR1_REG     = adc_CFG1_DEC_DR1;     
        adc_DEC_OCOR_REG    = adc_CFG1_DEC_OCOR;    
        adc_DEC_OCORM_REG   = adc_CFG1_DEC_OCORM;   
        adc_DEC_OCORH_REG   = adc_CFG1_DEC_OCORH;   
        
        adc_DSM_CR4_REG     = adc_CFG1_DSM_CR4;     
        adc_DSM_CR5_REG     = adc_CFG1_DSM_CR5;     
        adc_DSM_CR6_REG     = adc_CFG1_DSM_CR6;     
        adc_DSM_CR7_REG     = adc_CFG1_DSM_CR7;     
        adc_DSM_CR8_REG     = adc_CFG1_DSM_CR8;     
        adc_DSM_CR9_REG     = adc_CFG1_DSM_CR9;     
        adc_DSM_CR10_REG    = adc_CFG1_DSM_CR10;    
        adc_DSM_CR11_REG    = adc_CFG1_DSM_CR11;    
        adc_DSM_CR12_REG    = adc_CFG1_DSM_CR12;    
        adc_DSM_CR14_REG    = adc_CFG1_DSM_CR14;    
        adc_DSM_CR15_REG    = adc_CFG1_DSM_CR15;    
        adc_DSM_CR16_REG    = adc_CFG1_DSM_CR16;    
        adc_DSM_CR17_REG    = adc_CFG1_DSM_CR17;
		/* Set DSM_REF0_REG by disabling and enabling the PRESS cirucit */
		adc_SetDSMRef0Reg(adc_CFG1_DSM_REF0);
        adc_DSM_REF2_REG    = adc_CFG1_DSM_REF2;    
        adc_DSM_REF3_REG    = adc_CFG1_DSM_REF3;    
        
        adc_DSM_BUF0_REG    = adc_CFG1_DSM_BUF0;    
        adc_DSM_BUF1_REG    = adc_CFG1_DSM_BUF1;    
        adc_DSM_BUF2_REG    = adc_CFG1_DSM_BUF2;   
        adc_DSM_BUF3_REG    = adc_CFG1_DSM_BUF3;
        #if (CY_PSOC5A)
            adc_DSM_CLK_REG    |= adc_CFG1_DSM_CLK; 
        #endif /* CY_PSOC5A */
        
        /* To select either Vssa or Vref to -ve input of DSM depending on 
          the input  range selected.
        */
        
        #if(adc_DEFAULT_INPUT_MODE)
            #if (CY_PSOC3 || CY_PSOC5LP)
                #if (adc_CFG1_RANGE == adc_IR_VSSA_TO_2VREF)
                    adc_AMux_Select(1);
                #else
                    adc_AMux_Select(0);
                #endif /* adc_IR_VSSA_TO_2VREF) */
            #elif (CY_PSOC5A)
                adc_AMux_Select(0);
            #endif /* CY_PSOC3 || CY_PSOC5LP */
        #endif /* adc_DEFAULT_INPUT_MODE */
        
        /* Set the Conversion stop if resolution is above 16 bit and conversion 
           mode is Single sample */
        #if(adc_CFG1_RESOLUTION > 16 && \
            adc_CFG1_CONV_MODE == adc_MODE_SINGLE_SAMPLE) 
            adc_stopConversion = 1;
        #endif /* Single sample with resolution above 16 bits. */
        adc_CountsPerVolt = (uint32)adc_CFG1_COUNTS_PER_VOLT;
        
        /* Start and set interrupt vector */
        CyIntSetPriority(adc_IRQ__INTC_NUMBER, adc_IRQ_INTC_PRIOR_NUMBER);
        CyIntSetVector(adc_IRQ__INTC_NUMBER, adc_ISR1 );
        CyIntEnable(adc_IRQ__INTC_NUMBER);
    }
    
    #if(adc_DEFAULT_NUM_CONFIGS > 1)
        if(config == 2u)
        {
            /* Second Config */
            adc_DEC_CR_REG      = adc_CFG2_DEC_CR;      
            adc_DEC_SHIFT1_REG  = adc_CFG2_DEC_SHIFT1;  
            adc_DEC_SHIFT2_REG  = adc_CFG2_DEC_SHIFT2;  
            adc_DEC_DR2_REG     = adc_CFG2_DEC_DR2;     
            adc_DEC_DR2H_REG    = adc_CFG2_DEC_DR2H;    
            adc_DEC_DR1_REG     = adc_CFG2_DEC_DR1;     
            adc_DEC_OCOR_REG    = adc_CFG2_DEC_OCOR;    
            adc_DEC_OCORM_REG   = adc_CFG2_DEC_OCORM;   
            adc_DEC_OCORH_REG   = adc_CFG2_DEC_OCORH;   
        
            adc_DSM_CR4_REG     = adc_CFG2_DSM_CR4;     
            adc_DSM_CR5_REG     = adc_CFG2_DSM_CR5;     
            adc_DSM_CR6_REG     = adc_CFG2_DSM_CR6;     
            adc_DSM_CR7_REG     = adc_CFG2_DSM_CR7;     
            adc_DSM_CR8_REG     = adc_CFG2_DSM_CR8;     
            adc_DSM_CR9_REG     = adc_CFG2_DSM_CR9;     
            adc_DSM_CR10_REG    = adc_CFG2_DSM_CR10;    
            adc_DSM_CR11_REG    = adc_CFG2_DSM_CR11;    
            adc_DSM_CR12_REG    = adc_CFG2_DSM_CR12;    
            adc_DSM_CR14_REG    = adc_CFG2_DSM_CR14;    
            adc_DSM_CR15_REG    = adc_CFG2_DSM_CR15;    
            adc_DSM_CR16_REG    = adc_CFG2_DSM_CR16;    
            adc_DSM_CR17_REG    = adc_CFG2_DSM_CR17;    
			/* Set DSM_REF0_REG by disabling and enabling the PRESS cirucit */
			adc_SetDSMRef0Reg(adc_CFG2_DSM_REF0);
            adc_DSM_REF2_REG    = adc_CFG2_DSM_REF2;    
            adc_DSM_REF3_REG    = adc_CFG2_DSM_REF3;    
        
            adc_DSM_BUF0_REG    = adc_CFG2_DSM_BUF0;    
            adc_DSM_BUF1_REG    = adc_CFG2_DSM_BUF1;    
            adc_DSM_BUF2_REG    = adc_CFG2_DSM_BUF2;    
            adc_DSM_BUF3_REG    = adc_CFG2_DSM_BUF3; 
            #if (CY_PSOC5A)
                adc_DSM_CLK_REG    |= adc_CFG1_DSM_CLK; 
            #endif /* CY_PSOC5A */
            
            /* To select either Vssa or Vref to -ve input of DSM depending on 
               the input range selected.
            */
            
            #if(adc_DEFAULT_INPUT_MODE)
                #if (CY_PSOC3 || CY_PSOC5LP)
                    #if (adc_CFG2_INPUT_RANGE == adc_IR_VSSA_TO_2VREF)
                        adc_AMux_Select(1);
                    #else
                        adc_AMux_Select(0);
                    #endif /* adc_IR_VSSA_TO_2VREF) */
                #elif (CY_PSOC5A)
                    adc_AMux_Select(0);
                #endif /* CY_PSOC3 || CY_PSOC5LP */
            #endif /* adc_DEFAULT_INPUT_MODE */
            
            /* Set the Conversion stop if resolution is above 16 bit and 
               conversion mode is Single sample */
            #if(adc_CFG2_RESOLUTION > 16 && \
                adc_CFG2_CONVMODE == adc_MODE_SINGLE_SAMPLE) 
                adc_stopConversion = 1;
            #endif /* Single sample with resolution above 16 bits. */
            
            adc_CountsPerVolt = (uint32)adc_CFG2_COUNTS_PER_VOLT;
            
            /* Start and set interrupt vector */
            CyIntSetPriority(adc_IRQ__INTC_NUMBER, adc_IRQ_INTC_PRIOR_NUMBER);
            CyIntSetVector(adc_IRQ__INTC_NUMBER, adc_ISR2 );
            CyIntEnable(adc_IRQ__INTC_NUMBER);
        }
    #endif /* adc_DEFAULT_NUM_CONFIGS > 1 */

    #if(adc_DEFAULT_NUM_CONFIGS > 2)
        if(config == 3u)
        {
            /* Third Config */
            adc_DEC_CR_REG      = adc_CFG3_DEC_CR;      
            adc_DEC_SHIFT1_REG  = adc_CFG3_DEC_SHIFT1;  
            adc_DEC_SHIFT2_REG  = adc_CFG3_DEC_SHIFT2;  
            adc_DEC_DR2_REG     = adc_CFG3_DEC_DR2;     
            adc_DEC_DR2H_REG    = adc_CFG3_DEC_DR2H;    
            adc_DEC_DR1_REG     = adc_CFG3_DEC_DR1;     
            adc_DEC_OCOR_REG    = adc_CFG3_DEC_OCOR;    
            adc_DEC_OCORM_REG   = adc_CFG3_DEC_OCORM;   
            adc_DEC_OCORH_REG   = adc_CFG3_DEC_OCORH;   
         
            adc_DSM_CR4_REG     = adc_CFG3_DSM_CR4;     
            adc_DSM_CR5_REG     = adc_CFG3_DSM_CR5;     
            adc_DSM_CR6_REG     = adc_CFG3_DSM_CR6;     
            adc_DSM_CR7_REG     = adc_CFG3_DSM_CR7;     
            adc_DSM_CR8_REG     = adc_CFG3_DSM_CR8;     
            adc_DSM_CR9_REG     = adc_CFG3_DSM_CR9;     
            adc_DSM_CR10_REG    = adc_CFG3_DSM_CR10;    
            adc_DSM_CR11_REG    = adc_CFG3_DSM_CR11;    
            adc_DSM_CR12_REG    = adc_CFG3_DSM_CR12;    
            adc_DSM_CR14_REG    = adc_CFG3_DSM_CR14;    
            adc_DSM_CR15_REG    = adc_CFG3_DSM_CR15;    
            adc_DSM_CR16_REG    = adc_CFG3_DSM_CR16;    
            adc_DSM_CR17_REG    = adc_CFG3_DSM_CR17;    
			/* Set DSM_REF0_REG by disabling and enabling the PRESS cirucit */
			adc_SetDSMRef0Reg(adc_CFG3_DSM_REF0);
            adc_DSM_REF2_REG    = adc_CFG3_DSM_REF2;    
            adc_DSM_REF3_REG    = adc_CFG3_DSM_REF3;    
        
            adc_DSM_BUF0_REG    = adc_CFG3_DSM_BUF0;    
            adc_DSM_BUF1_REG    = adc_CFG3_DSM_BUF1;    
            adc_DSM_BUF2_REG    = adc_CFG3_DSM_BUF2;
            adc_DSM_BUF3_REG    = adc_CFG3_DSM_BUF3;
            #if (CY_PSOC5A)
                adc_DSM_CLK_REG    |= adc_CFG1_DSM_CLK; 
            #endif /* CY_PSOC5A */
            
            /* To select either Vssa or Vref to -ve input of DSM depending on 
               the input range selected.
            */
            
            #if(adc_DEFAULT_INPUT_MODE)
                #if (CY_PSOC3 || CY_PSOC5LP)
                    #if (adc_CFG3_INPUT_RANGE == adc_IR_VSSA_TO_2VREF)
                        adc_AMux_Select(1);
                    #else
                        adc_AMux_Select(0);
                    #endif /* adc_IR_VSSA_TO_2VREF) */
                #elif (CY_PSOC5A)
                    adc_AMux_Select(0);
                #endif /* CY_PSOC3 || CY_PSOC5LP */
            #endif /* adc_DEFAULT_INPUT_MODE */
                       
            /* Set the Conversion stop if resolution is above 16 bit and 
               conversion  mode is Single sample */
            #if(adc_CFG3_RESOLUTION > 16 && \
                adc_CFG3_CONVMODE == adc_MODE_SINGLE_SAMPLE) 
                adc_stopConversion = 1;
            #endif /* Single sample with resolution above 16 bits */
            
            adc_CountsPerVolt = (uint32)adc_CFG3_COUNTS_PER_VOLT;
            
            /* Start and set interrupt vector */
            CyIntSetPriority(adc_IRQ__INTC_NUMBER, adc_IRQ_INTC_PRIOR_NUMBER);
            CyIntSetVector(adc_IRQ__INTC_NUMBER, adc_ISR3 );
            CyIntEnable(adc_IRQ__INTC_NUMBER);
        }
    #endif /* adc_DEFAULT_NUM_CONFIGS > 3 */

    #if(adc_DEFAULT_NUM_CONFIGS == 4)
        if (config == 4u)
        {
            /* Fourth Config */
            adc_DEC_CR_REG      = adc_CFG4_DEC_CR;      
            adc_DEC_SHIFT1_REG  = adc_CFG4_DEC_SHIFT1;  
            adc_DEC_SHIFT2_REG  = adc_CFG4_DEC_SHIFT2;  
            adc_DEC_DR2_REG     = adc_CFG4_DEC_DR2;     
            adc_DEC_DR2H_REG    = adc_CFG4_DEC_DR2H;    
            adc_DEC_DR1_REG     = adc_CFG4_DEC_DR1;     
            adc_DEC_OCOR_REG    = adc_CFG4_DEC_OCOR;    
            adc_DEC_OCORM_REG   = adc_CFG4_DEC_OCORM;   
            adc_DEC_OCORH_REG   = adc_CFG4_DEC_OCORH;   

            adc_DSM_CR4_REG     = adc_CFG4_DSM_CR4;     
            adc_DSM_CR5_REG     = adc_CFG4_DSM_CR5;     
            adc_DSM_CR6_REG     = adc_CFG4_DSM_CR6;     
            adc_DSM_CR7_REG     = adc_CFG4_DSM_CR7;     
            adc_DSM_CR8_REG     = adc_CFG4_DSM_CR8;     
            adc_DSM_CR9_REG     = adc_CFG4_DSM_CR9;     
            adc_DSM_CR10_REG    = adc_CFG4_DSM_CR10;    
            adc_DSM_CR11_REG    = adc_CFG4_DSM_CR11;    
            adc_DSM_CR12_REG    = adc_CFG4_DSM_CR12;    
            adc_DSM_CR14_REG    = adc_CFG4_DSM_CR14;    
            adc_DSM_CR15_REG    = adc_CFG4_DSM_CR15;    
            adc_DSM_CR16_REG    = adc_CFG4_DSM_CR16;    
            adc_DSM_CR17_REG    = adc_CFG4_DSM_CR17;
			/* Set DSM_REF0_REG by disabling and enabling the PRESS cirucit */
			adc_SetDSMRef0Reg(adc_CFG4_DSM_REF0);
            adc_DSM_REF2_REG    = adc_CFG4_DSM_REF2;    
            adc_DSM_REF3_REG    = adc_CFG4_DSM_REF3;    
        
            adc_DSM_BUF0_REG    = adc_CFG4_DSM_BUF0;    
            adc_DSM_BUF1_REG    = adc_CFG4_DSM_BUF1;    
            adc_DSM_BUF2_REG    = adc_CFG4_DSM_BUF2;
            adc_DSM_BUF3_REG    = adc_CFG4_DSM_BUF3;
            #if (CY_PSOC5A)
                adc_DSM_CLK_REG    |= adc_CFG1_DSM_CLK; 
            #endif /* CY_PSOC5A */
            
            /* To select either Vssa or Vref to -ve input of DSM depending on 
               the input range selected.
            */
            
            #if(adc_DEFAULT_INPUT_MODE)
                #if (CY_PSOC3 || CY_PSOC5LP)
                    #if (adc_CFG4_INPUT_RANGE == adc_IR_VSSA_TO_2VREF)
                        adc_AMux_Select(1);
                    #else
                        adc_AMux_Select(0);
                    #endif /* adc_IR_VSSA_TO_2VREF) */
                #elif (CY_PSOC5A)
                    adc_AMux_Select(0);
                #endif /* CY_PSOC3 || CY_PSOC5LP */
            #endif /* adc_DEFAULT_INPUT_MODE */
                       
            /* Set the Conversion stop if resolution is above 16 bit and 
               conversion mode is Single sample */
            #if(adc_CFG4_RESOLUTION > 16 && \
                adc_CFG4_CONVMODE == adc_MODE_SINGLE_SAMPLE) 
                adc_stopConversion = 1;
            #endif /* Single sample with resolution above 16 bits */
            
            adc_CountsPerVolt = (uint32)adc_CFG4_COUNTS_PER_VOLT;

            /* Start and set interrupt vector */
            CyIntSetPriority(adc_IRQ__INTC_NUMBER, adc_IRQ_INTC_PRIOR_NUMBER);
            CyIntSetVector(adc_IRQ__INTC_NUMBER, adc_ISR4 );
            CyIntEnable(adc_IRQ__INTC_NUMBER);
        }
    #endif /* adc_DEFAULT_NUM_CONFIGS > 4 */
}


/*******************************************************************************
* Function Name: adc_RoundValue(uint32 busClockFreq, 
*                                            uint32 clockFreq)
********************************************************************************
*
* Summary:
*  Function to round an integer value.
*
* Parameters:  
*  busClockFreq:  Frequency of the bus clock.
*  clockFreq:  Frequency of the component clock.
*
* Return: 
*  uint16: rounded integer value.
*
*******************************************************************************/
uint16 adc_RoundValue(uint32 busClockFreq, uint32 clockFreq) \
                                  
{
    uint16 divider1, divider2;
    
    divider1 = ((busClockFreq * 10) / clockFreq);
    divider2 = (busClockFreq / clockFreq);
    if ( divider1 - (divider2 * 10) >= 5)
    {
        divider2 += 1;
    }
    return divider2;
}
         

/*******************************************************************************
* Function Name: adc_SelectCofiguration(uint8 config, 
*                                                    uint8 restart)
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
*******************************************************************************/
void adc_SelectConfiguration(uint8 config, uint8 restart) \
                                              
{
    uint8 inputRange = 0, resolution = 16;
    uint16 idealGain = 0, adcClockDivider = 1;    
    uint16 cpClockDivider = 1;
    uint16 idealOddGain = 0;
    
    /* Check whether the config is valid or not */
    if( config <= adc_DEFAULT_NUM_CONFIGS)
    {   
       /* Set the flag to ensure start() API dont override the config
           selected if ADC is not already started */
        if(adc_initVar == 0u)
        {
            adc_started = 1u;
        }
		
		/* Update the config flag */
		adc_Config = config;
       
        /* Stop the ADC  */
        adc_Stop();
        
        #if (CY_PSOC5A)
            /* Set the structure field which checks whether or not to
               restore the power registers */
            adc_powerModeBackup.bypassRestore = adc_BYPASS_ENABLED;
        #endif /* CY_PSOC5A */
        
        /* Set the  ADC registers based on the configuration */
        adc_InitConfig(config);
        
        /* Trim value calculation */
        if(config == 1u)
        {
            inputRange = adc_CFG1_RANGE;
            resolution = adc_CFG1_RESOLUTION;
            idealGain = adc_CFG1_IDEAL_DEC_GAIN;
            idealOddGain = adc_CFG1_IDEAL_ODDDEC_GAIN;
            adcClockDivider = adc_RoundValue((uint32)BCLK__BUS_CLK__HZ,
                                                       (uint32)adc_CFG1_CLOCK_FREQ);
            cpClockDivider = adc_RoundValue((uint32)BCLK__BUS_CLK__HZ,
                                                       (uint32)adc_CFG1_CP_CLOCK_FREQ);
        }

        #if (adc_DEFAULT_NUM_CONFIGS > 1)
            if(config == 2u)
            {
                inputRange = adc_CFG2_INPUT_RANGE;
                resolution = adc_CFG2_RESOLUTION;
                idealGain = adc_CFG2_IDEAL_DEC_GAIN;
                idealOddGain = adc_CFG2_IDEAL_ODDDEC_GAIN;
                adcClockDivider = adc_RoundValue((uint32)BCLK__BUS_CLK__HZ,
                                                            (uint32)adc_CFG2_CLOCK_FREQ);
                cpClockDivider = adc_RoundValue((uint32)BCLK__BUS_CLK__HZ,
                                                       (uint32)adc_CFG2_CP_CLOCK_FREQ);
            }
        #endif /* adc_DEFAULT_NUM_CONFIGS > 1 */

        #if(adc_DEFAULT_NUM_CONFIGS > 2)
            if(config == 3u)
            {
                inputRange = adc_CFG3_INPUT_RANGE;
                resolution = adc_CFG3_RESOLUTION;
                idealGain = adc_CFG3_IDEAL_DEC_GAIN;
                idealOddGain = adc_CFG3_IDEAL_ODDDEC_GAIN;
                adcClockDivider = adc_RoundValue((uint32)BCLK__BUS_CLK__HZ,
                                                            (uint32)adc_CFG3_CLOCK_FREQ);
                cpClockDivider = adc_RoundValue((uint32)BCLK__BUS_CLK__HZ,
                                                       (uint32)adc_CFG3_CP_CLOCK_FREQ);
            }
        #endif /* adc_DEFAULT_NUM_CONFIGS > 2 */

        #if (adc_DEFAULT_NUM_CONFIGS > 3)
            if(config == 4u)
            {
                inputRange = adc_CFG4_INPUT_RANGE;
                resolution = adc_CFG4_RESOLUTION;
                idealGain = adc_CFG4_IDEAL_DEC_GAIN;
                idealOddGain = adc_CFG4_IDEAL_ODDDEC_GAIN;
                adcClockDivider = adc_RoundValue((uint32)BCLK__BUS_CLK__HZ,  
                                                            (uint32)adc_CFG4_CLOCK_FREQ);
                cpClockDivider = adc_RoundValue((uint32)BCLK__BUS_CLK__HZ,
                                                       (uint32)adc_CFG4_CP_CLOCK_FREQ);
            }
        #endif /* adc_DEFAULT_NUM_CONFIGS > 3 */
        
        adcClockDivider = adcClockDivider - 1;
        /* Set the proper divider for the internal clock */
        #if(adc_DEFAULT_INTERNAL_CLK)
            adc_theACLK_SetDividerRegister(adcClockDivider, 1);
        #endif /* adc_DEFAULT_INTERNAL_CLK */
        
        cpClockDivider = cpClockDivider - 1;
        /* Set the proper divider for the Charge pump clock */
        adc_Ext_CP_Clk_SetDividerRegister(cpClockDivider, 1);
        
        /* Compensate the gain */
        adc_GainCompensation(inputRange, idealGain, idealOddGain, resolution);
        
        if(restart == 1u)
        {        
            /* Restart the ADC */
            adc_Start();
            
            /* Code to disable the REFBUF0 if reference chosen is External ref */
            #if (((adc_CFG2_REFERENCE == adc_EXT_REF_ON_P03) || \
                 (adc_CFG2_REFERENCE == adc_EXT_REF_ON_P32)) || \
                 ((adc_CFG3_REFERENCE == adc_EXT_REF_ON_P03) || \
                 (adc_CFG3_REFERENCE == adc_EXT_REF_ON_P32)) || \
                 ((adc_CFG4_REFERENCE == adc_EXT_REF_ON_P03) || \
                 (adc_CFG4_REFERENCE == adc_EXT_REF_ON_P32)))
                if (((config == 2) && 
                    ((adc_CFG2_REFERENCE == adc_EXT_REF_ON_P03) ||
                    (adc_CFG2_REFERENCE == adc_EXT_REF_ON_P32))) ||
                    ((config == 3) && 
                    ((adc_CFG3_REFERENCE == adc_EXT_REF_ON_P03) ||
                    (adc_CFG3_REFERENCE == adc_EXT_REF_ON_P32))) ||
                    ((config == 4) && 
                    ((adc_CFG4_REFERENCE == adc_EXT_REF_ON_P03) ||
                    (adc_CFG4_REFERENCE == adc_EXT_REF_ON_P32))))
                {
                    /* Disable PRES, Enable power to VCMBUF0, REFBUF0 and 
                       REFBUF1, enable PRES */
                    #if (CY_PSOC3 || CY_PSOC5LP)
                        adc_RESET_CR4_REG |= adc_IGNORE_PRESA1;
                        adc_RESET_CR5_REG |= adc_IGNORE_PRESA2;
                    #elif (CY_PSOC5A)
                        adc_RESET_CR1_REG |= adc_DIS_PRES1;
                        adc_RESET_CR3_REG |= adc_DIS_PRES2;
                    #endif /* CY_PSOC5A */
        
                    /* Disable the REFBUF0 */
                    adc_DSM_CR17_REG &= ~adc_DSM_EN_BUF_VREF;
                    
                    /* Wait for 3 microseconds */
                    CyDelayUs(3);
                    /* Enable the press circuit */
                    #if (CY_PSOC3 || CY_PSOC5LP)
                        adc_RESET_CR4_REG &= ~adc_IGNORE_PRESA1;
                        adc_RESET_CR5_REG &= ~adc_IGNORE_PRESA2;
                    #elif (CY_PSOC5A)
                        adc_RESET_CR1_REG &= ~adc_DIS_PRES1;
                        adc_RESET_CR3_REG &= ~adc_DIS_PRES2;
                    #endif /* CY_PSOC5A */
                }
            #endif /* */
            
            #if ((CY_PSOC3 || CY_PSOC5LP) && \
                 ((adc_CFG2_INPUT_RANGE == adc_IR_VSSA_TO_2VREF) || \
                  (adc_CFG3_INPUT_RANGE == adc_IR_VSSA_TO_2VREF) || \
                  (adc_CFG4_INPUT_RANGE == adc_IR_VSSA_TO_2VREF)))
                if(((config == 2) && 
                    (adc_CFG2_INPUT_RANGE == adc_IR_VSSA_TO_2VREF) && 
                    ((adc_CFG2_REFERENCE != adc_EXT_REF_ON_P03) && 
                     (adc_CFG2_REFERENCE != adc_EXT_REF_ON_P32))) ||
                     ((config == 3) && 
                      (adc_CFG3_INPUT_RANGE == adc_IR_VSSA_TO_2VREF) && 
                     ((adc_CFG3_REFERENCE != adc_EXT_REF_ON_P03) && 
                     (adc_CFG3_REFERENCE != adc_EXT_REF_ON_P32))) ||
                     ((config == 4) && 
                      (adc_CFG4_INPUT_RANGE == adc_IR_VSSA_TO_2VREF) && 
                     ((adc_CFG4_REFERENCE != adc_EXT_REF_ON_P03) && 
                     (adc_CFG4_REFERENCE != adc_EXT_REF_ON_P32))))
                {
                    /* Disable PRES, Enable power to VCMBUF0, REFBUF0 and 
                       REFBUF1, enable PRES */
                    #if (CY_PSOC3 || CY_PSOC5LP)
                        adc_RESET_CR4_REG |= adc_IGNORE_PRESA1;
                        adc_RESET_CR5_REG |= adc_IGNORE_PRESA2;
                    #elif (CY_PSOC5A)
                        adc_RESET_CR1_REG |= adc_DIS_PRES1;
                        adc_RESET_CR3_REG |= adc_DIS_PRES2;
                    #endif /* CY_PSOC5A */
        
                    /* Enable the REFBUF1 */
                    adc_DSM_REF0_REG |= adc_DSM_EN_BUF_VREF_INN;
                    
                    /* Wait for 3 microseconds */
                    CyDelayUs(3);
                    /* Enable the press circuit */
                    #if (CY_PSOC3 || CY_PSOC5LP)
                        adc_RESET_CR4_REG &= ~adc_IGNORE_PRESA1;
                        adc_RESET_CR5_REG &= ~adc_IGNORE_PRESA2;
                    #elif (CY_PSOC5A)
                        adc_RESET_CR1_REG &= ~adc_DIS_PRES1;
                        adc_RESET_CR3_REG &= ~adc_DIS_PRES2;
                    #endif /* CY_PSOC5A */
                }
            #endif /* (CY_PSOC3 || CY_PSOC5LP) */
        
            /* Restart the ADC conversion */
            adc_StartConvert();
        }
    }
}     


/*******************************************************************************
* Function Name: adc_GainCompensation(uint8, uint16, uint16, uint8)
********************************************************************************
*
* Summary:
*  This API calculates the trim value and then loads this to GCOR register.
*
* Parameters:  
*  inputRange:  input range for which trim value is to be calculated.
*  IdealDecGain:  Ideal Decimator gain for the selected resolution and 
*                 conversion  mode.
*  IdealOddDecGain:  Ideal odd decimation gain for the selected resolution and 
                     conversion mode.
*  resolution:  Resolution to select the proper flash location for trim value.
*
* Return: 
*  void
*
*******************************************************************************/
void adc_GainCompensation(uint8 inputRange, uint16 IdealDecGain, uint16 IdealOddDecGain,  \
                                       uint8 resolution) 
{
    int8 flash;
    int16 Normalised;
    uint16 GcorValue;
    uint32 GcorTmp;
    
    switch(inputRange)
    {         
        case adc_IR_VNEG_VREF_DIFF:
        
        /* Normalize the flash Value */
        if(resolution > 15)
        {
            flash = adc_DEC_TRIM_VREF_DIFF_16_20; 
        }    
        else
        {
            flash = adc_DEC_TRIM_VREF_DIFF_8_15;
        }        
        break;
        
        case adc_IR_VNEG_VREF_2_DIFF:
        
        /* Normalize the flash Value */
        if(resolution > 15)
        {
            flash = adc_DEC_TRIM_VREF_2_DIFF_16_20;
        }    
        else
        {
            flash = adc_DEC_TRIM_VREF_2_DIFF_8_15;
        }    
        break;
        
        case adc_IR_VNEG_VREF_4_DIFF:
        
        /* Normalize the flash Value */
        if(resolution > 15)
        {
            flash = adc_DEC_TRIM_VREF_4_DIFF_16_20;
        }    
        else
        {
            flash = adc_DEC_TRIM_VREF_4_DIFF_8_15;
        }    
        break;
        
        case adc_IR_VNEG_VREF_16_DIFF:
        
        /* Normalize the flash Value */
        if(resolution > 15)
        {
            flash = adc_DEC_TRIM_VREF_16_DIFF_16_20;
        }    
        else
        {
            flash = adc_DEC_TRIM_VREF_16_DIFF_8_15;
        }    
        break;
        
        default:
            flash = 0;
        break;
    }    
    if(inputRange == adc_IR_VSSA_TO_VREF || inputRange == adc_IR_VSSA_TO_2VREF || 
       inputRange == adc_IR_VSSA_TO_VDDA || inputRange == adc_IR_VSSA_TO_6VREF || 
       inputRange == adc_IR_VNEG_2VREF_DIFF || inputRange == adc_IR_VNEG_6VREF_DIFF ||
       inputRange == adc_IR_VNEG_VREF_8_DIFF)
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
    GcorValue = (uint16)(GcorTmp / adc_IDEAL_GAIN_CONST);
        
    if (resolution < 14)
    {
        GcorValue = (GcorValue >> (15 - (resolution + 1)));
        adc_DEC_GVAL_REG = (resolution + 1);
    }
    else
    {
        /* Use all 16 bits */
        adc_DEC_GVAL_REG = 15u;  
    }
      
    /* Load the gain correction register */    
    adc_DEC_GCOR_REG  = (uint8)GcorValue;
    adc_DEC_GCORH_REG = (uint8)(GcorValue >> 8);    
    
    /* Workaround for 0 to 2*Vref mode with PSoC5A siliocn */
    #if( CY_PSOC5A) 
        if( inputRange == adc_IR_VSSA_TO_2VREF)
        {
            adc_DEC_GCOR_REG = 0x00u;
            adc_DEC_GCORH_REG = 0x00u;
            adc_DEC_GVAL_REG = 0x00u;
        }
    #endif /* CY_PSOC5A */
}


/******************************************************************************
* Function Name: adc_SetDSMRef0Reg(uint8)
******************************************************************************
*
* Summary:
*  This API sets the DSM_REF0 register. This is written for internal use.
*
* Parameters:  
*  value:  Value to be written to DSM_REF0 register.
*
* Return: 
*  void
*
******************************************************************************/
void adc_SetDSMRef0Reg(uint8 value) 
{
    /* Disable PRES, Enable power to VCMBUF0, REFBUF0 and REFBUF1, enable 
       PRES */
    #if (CY_PSOC3 || CY_PSOC5LP)
        adc_RESET_CR4_REG |= (adc_IGNORE_PRESA1 | adc_IGNORE_PRESD1);
        adc_RESET_CR5_REG |= (adc_IGNORE_PRESA2 | adc_IGNORE_PRESD2);
    #elif (CY_PSOC5A)
        adc_RESET_CR1_REG |= adc_DIS_PRES1;
        adc_RESET_CR3_REG |= adc_DIS_PRES2;
    #endif /* CY_PSOC5A */
        adc_DSM_REF0_REG = value;   
		
	/* Wait for 3 microseconds */
    CyDelayUs(3);
    /* Enable the press circuit */
    #if (CY_PSOC3 || CY_PSOC5LP)
        adc_RESET_CR4_REG &= ~(adc_IGNORE_PRESA1 | adc_IGNORE_PRESD1);
        adc_RESET_CR5_REG &= ~(adc_IGNORE_PRESA2 | adc_IGNORE_PRESD2);
    #elif (CY_PSOC5A)
        adc_RESET_CR1_REG &= ~adc_DIS_PRES1;
        adc_RESET_CR3_REG &= ~adc_DIS_PRES2;
    #endif /* CY_PSOC5A */
}


/* [] END OF FILE */
