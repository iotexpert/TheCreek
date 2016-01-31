/*******************************************************************************
* File Name: `$INSTANCE_NAME`.c  
* Version `$CY_MAJOR_VERSION`.`$CY_MINOR_VERSION`
*
* Description:
*  This file provides the source code to the API for the PGA 
*  User Module.
*
* Note:
*
********************************************************************************
* Copyright 2008-2010, Cypress Semiconductor Corporation.  All rights reserved.
* You may use this file only in accordance with the license, terms, conditions, 
* disclaimers, and limitations in the end user license agreement accompanying 
* the software package with which this file was provided.
*******************************************************************************/

#include "cytypes.h"
#include "`$INSTANCE_NAME`.h"

#if (`$INSTANCE_NAME`_ACLK_ENABLE)
    #include "`$INSTANCE_NAME`_ScBoostClk.h"
#endif /* `$INSTANCE_NAME`_ACLK_ENABLE */

/* Constant array for gain settings */
const uint8 `$INSTANCE_NAME`_GainArray[9] = { 
   (`$INSTANCE_NAME`_RVAL_20K   | `$INSTANCE_NAME`_R20_40B_40K | `$INSTANCE_NAME`_BIAS_LOW), /* G=1  */
   (`$INSTANCE_NAME`_RVAL_30K   | `$INSTANCE_NAME`_R20_40B_40K | `$INSTANCE_NAME`_BIAS_LOW), /* G=2  */
   (`$INSTANCE_NAME`_RVAL_40K   | `$INSTANCE_NAME`_R20_40B_40K | `$INSTANCE_NAME`_BIAS_LOW), /* G=4  */
   (`$INSTANCE_NAME`_RVAL_60K   | `$INSTANCE_NAME`_R20_40B_40K | `$INSTANCE_NAME`_BIAS_LOW), /* G=8  */
   (`$INSTANCE_NAME`_RVAL_120K  | `$INSTANCE_NAME`_R20_40B_40K | `$INSTANCE_NAME`_BIAS_LOW), /* G=16 */
   (`$INSTANCE_NAME`_RVAL_250K  | `$INSTANCE_NAME`_R20_40B_40K | `$INSTANCE_NAME`_BIAS_LOW), /* G=24 */
   (`$INSTANCE_NAME`_RVAL_250K  | `$INSTANCE_NAME`_R20_40B_20K | `$INSTANCE_NAME`_BIAS_LOW), /* G=32 */
   (`$INSTANCE_NAME`_RVAL_500K  | `$INSTANCE_NAME`_R20_40B_20K | `$INSTANCE_NAME`_BIAS_LOW), /* G=48 */
   (`$INSTANCE_NAME`_RVAL_1000K | `$INSTANCE_NAME`_R20_40B_20K | `$INSTANCE_NAME`_BIAS_LOW)  /* G=50 */
};

/* Constant array for gain compenstion settings */
const uint8 `$INSTANCE_NAME`_GainComp[9] = { 
   ( `$INSTANCE_NAME`_COMP_4P35PF  | ( `$INSTANCE_NAME`_REDC_00 >> 2 )), /* G=1  */
   ( `$INSTANCE_NAME`_COMP_4P35PF  | ( `$INSTANCE_NAME`_REDC_01 >> 2 )), /* G=2  */
   ( `$INSTANCE_NAME`_COMP_3P0PF   | ( `$INSTANCE_NAME`_REDC_01 >> 2 )), /* G=4  */
   ( `$INSTANCE_NAME`_COMP_3P0PF   | ( `$INSTANCE_NAME`_REDC_01 >> 2 )), /* G=8  */
   ( `$INSTANCE_NAME`_COMP_3P6PF   | ( `$INSTANCE_NAME`_REDC_01 >> 2 )), /* G=16 */
   ( `$INSTANCE_NAME`_COMP_3P6PF   | ( `$INSTANCE_NAME`_REDC_11 >> 2 )), /* G=24 */
   ( `$INSTANCE_NAME`_COMP_3P6PF   | ( `$INSTANCE_NAME`_REDC_11 >> 2 )), /* G=32 */
   ( `$INSTANCE_NAME`_COMP_3P6PF   | ( `$INSTANCE_NAME`_REDC_00 >> 2 )), /* G=48 */
   ( `$INSTANCE_NAME`_COMP_3P6PF   | ( `$INSTANCE_NAME`_REDC_00 >> 2 ))  /* G=50 */
};

uint8 `$INSTANCE_NAME`_initVar = 0u;


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
*  void
*
* Return:
*  void
*
* Reentrant:
*  No
* 
*******************************************************************************/
void `$INSTANCE_NAME`_Init(void) `=ReentrantKeil($INSTANCE_NAME . "_Init")`
{
    /* Set PGA mode */
    `$INSTANCE_NAME`_CR0_REG = `$INSTANCE_NAME`_MODE_PGA;  
    
    /* Set non-inverting PGA mode  and reference mode */
    `$INSTANCE_NAME`_CR1_REG |= `$INSTANCE_NAME`_PGA_NINV;  

    /* Set default gain and ref mode */
    `$INSTANCE_NAME`_CR2_REG = `$INSTANCE_NAME`_VREF_MODE;

    /* Set gain and compensation */
    `$INSTANCE_NAME`_SetGain(`$INSTANCE_NAME`_DEFAULT_GAIN);

    /* Set power */
    `$INSTANCE_NAME`_SetPower(`$INSTANCE_NAME`_DEFAULT_POWER);
}


/*******************************************************************************   
* Function Name: `$INSTANCE_NAME`_Enable
********************************************************************************
*
* Summary:
*  Enables the PGA block operation
*
* Parameters:
*  void
*
* Return:
*  void
*
* Reentrant:
*  No
* 
*******************************************************************************/
void `$INSTANCE_NAME`_Enable(void) `=ReentrantKeil($INSTANCE_NAME . "_Enable")`
{
    /* If a boost clock is required  */
    #if (`$INSTANCE_NAME`_ACLK_ENABLE)
        `$INSTANCE_NAME`_ACLK_PM_ACT_CFG_REG |= `$INSTANCE_NAME`_ACLK_ACT_PWR_EN;
        `$INSTANCE_NAME`_ACLK_PM_STBY_CFG_REG |= `$INSTANCE_NAME`_ACLK_STBY_PWR_EN;
        `$INSTANCE_NAME`_ScBoostClk_Enable();  /* turn on boost pump */
        `$INSTANCE_NAME`_BST_REG |= 0x08;
    #endif /* `$INSTANCE_NAME`_ACLK_ENABLE */
   
    /* PSoC3 ES2 or early, PSoC5 ES1 */
    #if (`$INSTANCE_NAME`_PSOC3_ES2 || `$INSTANCE_NAME`_PSOC5_ES1)
        /* Enable Pump only if voltage is below threshold */
        if (`$INSTANCE_NAME`_CYDEV_VDDA_MV < `$INSTANCE_NAME`_VDDA_THRESHOLD_MV)
        {
            `$INSTANCE_NAME`_SC_MISC_REG |= `$INSTANCE_NAME`_PUMP_FORCE;
        }
        
    /* PSoC3 ES3 or later, PSoC5 ES2 or later*/
    #elif (`$INSTANCE_NAME`_PSOC3_ES3 || `$INSTANCE_NAME`_PSOC5_ES2)
        /* Enable charge Pump clock for SC block */
        `$INSTANCE_NAME`_PUMP_CR1_REG |= `$INSTANCE_NAME`_PUMP_CR1_SC_CLKSEL;
        /* Enable Pump only if voltage is below threshold */
        if (`$INSTANCE_NAME`_CYDEV_VDDA_MV < `$INSTANCE_NAME`_VDDA_THRESHOLD_MV)
        {
            `$INSTANCE_NAME`_SC_MISC_REG |= `$INSTANCE_NAME`_PUMP_FORCE;
        }
    #endif /* `$INSTANCE_NAME`_PSOC3_ES2 || `$INSTANCE_NAME`_PSOC5_ES1 */
     
    /* Enable power to the Amp in Active mode*/
    `$INSTANCE_NAME`_PM_ACT_CFG_REG |= `$INSTANCE_NAME`_ACT_PWR_EN;

    /* Enable power to the Amp in Alternative Active mode*/
    `$INSTANCE_NAME`_PM_STBY_CFG_REG |= `$INSTANCE_NAME`_STBY_PWR_EN;
}


/*******************************************************************************
* Function Name: `$INSTANCE_NAME`_Start
********************************************************************************
*
* Summary:
*  The start function initializes the PGA with the default values, and sets
*  the power to the given level.  A power level of 0, is the same as executing
*  the stop function.
*
* Parameters:  
*  void
*
* Return: 
*  void 
*
* Reentrant: 
*  No
*
*******************************************************************************/
void `$INSTANCE_NAME`_Start(void) 
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
*  Powers down amplifier to lowest power state.
*
* Parameters:  
*  void
*
* Return: 
*  void
*
* Reentrant: 
*  No
*
*******************************************************************************/
void `$INSTANCE_NAME`_Stop(void) `=ReentrantKeil($INSTANCE_NAME . "_Stop")`
{
    /* Disable pumps only if only one SC block is in use */
    if (`$INSTANCE_NAME`_PM_ACT_CFG_REG == `$INSTANCE_NAME`_ACT_PWR_EN)
    {
       `$INSTANCE_NAME`_SC_MISC_REG &= ~`$INSTANCE_NAME`_PUMP_FORCE;
    }   
    /* Disble power to the Amp in Active mode template */
    `$INSTANCE_NAME`_PM_ACT_CFG_REG &= ~`$INSTANCE_NAME`_ACT_PWR_EN;

    /* Disble power to the Amp in Alternative Active mode template */
    `$INSTANCE_NAME`_PM_STBY_CFG_REG &= ~`$INSTANCE_NAME`_STBY_PWR_EN;

    /* If a boost clock is required  */
    /* Disable and power down boost clock */
    #if (`$INSTANCE_NAME`_ACLK_ENABLE)
        `$INSTANCE_NAME`_ACLK_PM_ACT_CFG_REG &= ~`$INSTANCE_NAME`_ACLK_ACT_PWR_EN;
        `$INSTANCE_NAME`_ACLK_PM_STBY_CFG_REG &= ~`$INSTANCE_NAME`_ACLK_STBY_PWR_EN;
        `$INSTANCE_NAME`_ScBoostClk_Disable();  
    #endif /* `$INSTANCE_NAME`_ACLK_ENABLE */

}


/*******************************************************************************
* Function Name: `$INSTANCE_NAME`_SetPower
********************************************************************************
*
* Summary:
*  Set the power of the PGA
*
* Parameters:  
*  power:  Sets power level between (0) and (3) high power
*
* Return: 
*  void
*
* Reentrant: 
*  Yes
*
*******************************************************************************/
void `$INSTANCE_NAME`_SetPower(uint8 power) `=ReentrantKeil($INSTANCE_NAME . "_SetPower")`
{
    uint8 tmpCR;

    tmpCR = `$INSTANCE_NAME`_CR1_REG & ~`$INSTANCE_NAME`_DRIVE_MASK;
    tmpCR |= (power & `$INSTANCE_NAME`_DRIVE_MASK);
    `$INSTANCE_NAME`_CR1_REG = tmpCR;  
}


/*******************************************************************************
* Function Name: `$INSTANCE_NAME`_SetGain
********************************************************************************
*
* Summary:
*  This function sets values of the input and feedback resistors to set a 
*  specific gain of the amplifier.
*
* Parameters:  
*  gain:  Gain value of PGA (See header file for gain values.)
*
* Return: 
*  void 
*
* Reentrant: 
*  Yes
*
*******************************************************************************/
void `$INSTANCE_NAME`_SetGain(uint8 gain) `=ReentrantKeil($INSTANCE_NAME . "_SetGain")`
{
    /* Only set new gain if it is a valid gain */
    if( gain <= `$INSTANCE_NAME`_GAIN_MAX)
    {
        /* Clear resistors, redc, and bias */
        `$INSTANCE_NAME`_CR2_REG &= ~(`$INSTANCE_NAME`_RVAL_MASK | `$INSTANCE_NAME`_R20_40B_MASK | 
                                `$INSTANCE_NAME`_REDC_MASK | `$INSTANCE_NAME`_BIAS_MASK );

        /* Set gain value resistors, redc comp, and bias */
        `$INSTANCE_NAME`_CR2_REG |= (`$INSTANCE_NAME`_GainArray[gain] |
                                ( (`$INSTANCE_NAME`_GainComp[gain] << 2 ) & `$INSTANCE_NAME`_REDC_MASK));
             
        /* Clear sc_comp  */
        `$INSTANCE_NAME`_CR1_REG &= ~`$INSTANCE_NAME`_COMP_MASK;
        /* Set sc_comp  */
        `$INSTANCE_NAME`_CR1_REG |= ( `$INSTANCE_NAME`_GainComp[gain] | `$INSTANCE_NAME`_COMP_MASK );
    }
}


/* [] END OF FILE */
