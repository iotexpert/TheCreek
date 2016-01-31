/*******************************************************************************
* File Name: `$INSTANCE_NAME`.c  
* Version `$CY_MAJOR_VERSION`.`$CY_MINOR_VERSION`
*
*  Description:
*    This file provides the source code to the API for the inverting PGA 
*    User Module.
*
*   Note:
*
*******************************************************************************
* Copyright 2008-2009, Cypress Semiconductor Corporation.  All rights reserved.
* You may use this file only in accordance with the license, terms, conditions, 
* disclaimers, and limitations in the end user license agreement accompanying 
* the software package with which this file was provided.
********************************************************************************/




#include "cytypes.h"
#include "`$INSTANCE_NAME`.h"

#if (`$INSTANCE_NAME`_ACLK_ENABLE)
    #include "`$INSTANCE_NAME`_ScBoostClk.h"
#endif

/* Constant array for gain settings */
const uint8 `$INSTANCE_NAME`_GainArray[10] = { 
   (`$INSTANCE_NAME`_PGA_INV_GAIN_01 | `$INSTANCE_NAME`_BIAS_LOW), /* G=-1  */
   (`$INSTANCE_NAME`_PGA_INV_GAIN_03 | `$INSTANCE_NAME`_BIAS_LOW), /* G=-3  */
   (`$INSTANCE_NAME`_PGA_INV_GAIN_07 | `$INSTANCE_NAME`_BIAS_LOW), /* G=-7  */
   (`$INSTANCE_NAME`_PGA_INV_GAIN_15 | `$INSTANCE_NAME`_BIAS_LOW), /* G=-15 */
   (`$INSTANCE_NAME`_PGA_INV_GAIN_22 | `$INSTANCE_NAME`_BIAS_LOW), /* G=-22 */
   (`$INSTANCE_NAME`_PGA_INV_GAIN_23 | `$INSTANCE_NAME`_BIAS_LOW), /* G=-23 */
   (`$INSTANCE_NAME`_PGA_INV_GAIN_24 | `$INSTANCE_NAME`_BIAS_LOW), /* G=-24 */
   (`$INSTANCE_NAME`_PGA_INV_GAIN_31 | `$INSTANCE_NAME`_BIAS_LOW), /* G=-31 */
   (`$INSTANCE_NAME`_PGA_INV_GAIN_47 | `$INSTANCE_NAME`_BIAS_LOW), /* G=-47 */
   (`$INSTANCE_NAME`_PGA_INV_GAIN_49 | `$INSTANCE_NAME`_BIAS_LOW)  /* G=-49 */
};

/* Constant array for gain compenstion settings */
const uint8 `$INSTANCE_NAME`_GainComp[10] = { 
   ( `$INSTANCE_NAME`_COMP_4P35PF  | ( `$INSTANCE_NAME`_REDC_00 >> 2 )), /* G=-1  */
   ( `$INSTANCE_NAME`_COMP_4P35PF  | ( `$INSTANCE_NAME`_REDC_01 >> 2 )), /* G=-3  */
   ( `$INSTANCE_NAME`_COMP_3P0PF   | ( `$INSTANCE_NAME`_REDC_01 >> 2 )), /* G=-7  */
   ( `$INSTANCE_NAME`_COMP_3P0PF   | ( `$INSTANCE_NAME`_REDC_01 >> 2 )), /* G=-15 */
   ( `$INSTANCE_NAME`_COMP_3P6PF   | ( `$INSTANCE_NAME`_REDC_01 >> 2 )), /* G=-22 */
   ( `$INSTANCE_NAME`_COMP_3P0PF   | ( `$INSTANCE_NAME`_REDC_11 >> 2 )), /* G=-23 */
   ( `$INSTANCE_NAME`_COMP_3P0PF   | ( `$INSTANCE_NAME`_REDC_10 >> 2 )), /* G=-24 */
   ( `$INSTANCE_NAME`_COMP_3P6PF   | ( `$INSTANCE_NAME`_REDC_11 >> 2 )), /* G=-31 */
   ( `$INSTANCE_NAME`_COMP_3P6PF   | ( `$INSTANCE_NAME`_REDC_00 >> 2 )), /* G=-47 */
   ( `$INSTANCE_NAME`_COMP_3P6PF   | ( `$INSTANCE_NAME`_REDC_00 >> 2 ))  /* G=-49 */
};

uint8 `$INSTANCE_NAME`_initVar = 0;

/*******************************************************************************
* Function Name: `$INSTANCE_NAME`_Start
********************************************************************************
* Summary:
*  The start function initializes the inverting PGA with the default values, and sets
*  the power to the given level.  A power level of 0, is the same as executing
*  the stop function.
*
* Parameters:  
*  power:   Sets power level between off (0) and (3) high power
*
* Return: 
*  (void) 
*
* Theory: 
*
* Side Effects:
*
*******************************************************************************/
void `$INSTANCE_NAME`_Start(void) 
{

   if(`$INSTANCE_NAME`_initVar == 0)
   {
      `$INSTANCE_NAME`_initVar = 1;

      /* Set PGA mode */
      `$INSTANCE_NAME`_CR0 = `$INSTANCE_NAME`_MODE_PGA;  
	
      /* Set inverting PGA mode  and reference mode */
      `$INSTANCE_NAME`_CR1 &= ~`$INSTANCE_NAME`_PGA_INV;  

      /* Set gain and compensation */
      `$INSTANCE_NAME`_SetGain(`$INSTANCE_NAME`_DEFAULT_GAIN);

      /* Set power */
      `$INSTANCE_NAME`_SetPower(`$INSTANCE_NAME`_DEFAULT_POWER);
   }

   /* If a boost clock is required  */
   #if (`$INSTANCE_NAME`_ACLK_ENABLE)
      `$INSTANCE_NAME`_ACLK_PWRMGR |= `$INSTANCE_NAME`_ACLK_ACT_PWR_EN;
      `$INSTANCE_NAME`_ScBoostClk_Enable();  // turn on boost pump
	   `$INSTANCE_NAME`_BST |= 0x08;
   #endif 

   /* Enable power to the Amp */
   `$INSTANCE_NAME`_PWRMGR |= `$INSTANCE_NAME`_ACT_PWR_EN;
}

/*******************************************************************************
* Function Name: `$INSTANCE_NAME`_Stop
********************************************************************************
* Summary:
*  Summary of function.
*
* Parameters:  
*  Powers down amplifier to lowest power state.
*
* Return: 
*  (void) 
*
* Theory: 
*
* Side Effects:
*
*******************************************************************************/
void `$INSTANCE_NAME`_Stop(void)
{
   /* Disble power to the Amp */
   `$INSTANCE_NAME`_PWRMGR &= ~`$INSTANCE_NAME`_ACT_PWR_EN;

   /* If a boost clock is required  */
   /* Disable and power down boost clock */
   #if (`$INSTANCE_NAME`_ACLK_ENABLE)
      `$INSTANCE_NAME`_ACLK_PWRMGR &= ~`$INSTANCE_NAME`_ACLK_ACT_PWR_EN;
      `$INSTANCE_NAME`_ScBoostClk_Disable();  
   #endif 
}

/*******************************************************************************
* Function Name: `$INSTANCE_NAME`_SetPower
********************************************************************************
* Summary:
*  Set the power of the PGA
*
* Parameters:  
*  power:   Sets power level between (0) and (3) high power
*
* Return: 
*  (void) 
*
* Theory: 
*
* Side Effects:
*
*******************************************************************************/
void `$INSTANCE_NAME`_SetPower(uint8 power) 
{
   uint8 tmpCR;

   tmpCR = `$INSTANCE_NAME`_CR1 & ~`$INSTANCE_NAME`_DRIVE_MASK;
   tmpCR |= (power & `$INSTANCE_NAME`_DRIVE_MASK);
   `$INSTANCE_NAME`_CR1 = tmpCR;  
}

/*******************************************************************************
* Function Name: `$INSTANCE_NAME`_SetGain
********************************************************************************
* Summary:
*  This function sets values of the input and feedback resistors to set a 
*  specific gain of the amplifier.
*
* Parameters:  
*  gain:  Sets gain of amplifier.     
*
* Return: 
*  (void)
*
* Theory: 
*
* Side Effects:
*
*******************************************************************************/
void `$INSTANCE_NAME`_SetGain(uint8 gain)
{
    /* Only set new gain if it is a valid gain */
   if( gain <= `$INSTANCE_NAME`_GAIN_MAX)
   {
      /* Clear resistors, redc, and bias */
      `$INSTANCE_NAME`_CR2 &= ~(`$INSTANCE_NAME`_RVAL_MASK | `$INSTANCE_NAME`_R20_40B_MASK | 
                                `$INSTANCE_NAME`_REDC_MASK | `$INSTANCE_NAME`_BIAS_MASK );

      /* Set gain value resistors, redc comp, and bias */
      `$INSTANCE_NAME`_CR2 |= (`$INSTANCE_NAME`_GainArray[gain] |
                             ( (`$INSTANCE_NAME`_GainComp[gain] << 2 ) & `$INSTANCE_NAME`_REDC_MASK) );
            
      /* Clear sc_comp  */
      `$INSTANCE_NAME`_CR1 &= ~`$INSTANCE_NAME`_COMP_MASK;
      /* Set sc_comp  */
      `$INSTANCE_NAME`_CR1 |= ( `$INSTANCE_NAME`_GainComp[gain] | `$INSTANCE_NAME`_COMP_MASK );
   }
}


/* [] END OF FILE */
