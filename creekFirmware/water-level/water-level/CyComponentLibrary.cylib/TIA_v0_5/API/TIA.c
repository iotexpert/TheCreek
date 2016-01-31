
/*******************************************************************************
* File Name: `$INSTANCE_NAME`.c  
* Version `$CY_MAJOR_VERSION`.`$CY_MINOR_VERSION`
*
*  Description:
*    This file provides the source code to the API for the TIA User Module.
*
*******************************************************************************
* Copyright 2008-2009, Cypress Semiconductor Corporation.  All rights reserved.
* You may use this file only in accordance with the license, terms, conditions, 
* disclaimers, and limitations in the end user license agreement accompanying 
* the software package with which this file was provided.
********************************************************************************/



#include "cytypes.h"
#include "`$INSTANCE_NAME`.h"

#if(`$INSTANCE_NAME`_MIN_VDDA)
#include "`$INSTANCE_NAME`_bst_clk.h"
#endif

/* Fixed configuration of SC Block only performed once */
uint8 `$INSTANCE_NAME`_initVar = 0;

/*******************************************************************************
* Function Name: `$INSTANCE_NAME`_Start
********************************************************************************
* Summary:
*  The start function initializes the TIA with the default values and enables
*  power to the SC block
*
* Parameters:  
*  void
*
* Return: 
*  void 
*
* Theory: 
*  Full initialization of the SC Block configuration registers is only perfomed the
*  first time the routine executes - global variable `$INSTANCE_NAME`_initVar is used
*  indicate that the static configuration has been completed. 
*
* Side Effects:
*
*******************************************************************************/
void `$INSTANCE_NAME`_Start(void) 
{
    if(`$INSTANCE_NAME`_initVar == 0)
    {    
        `$INSTANCE_NAME`_initVar = 1;  

        /* Set SC Block Mode to TIA - SCxx_CR0[3:1] */
        `$INSTANCE_NAME`_CR0 = `$INSTANCE_NAME`_MODE_TIA;  
	
        /* 
            Set SC Block CR1 Control Register fields to support TIA mode 
                drive field set by SetPower() API
                compensation Cap - 3.0 pF
                div2 - disable, n/a for TIA mode
                Mixer/PGA Gain - 0db, n/a for TIA mode
        */
        `$INSTANCE_NAME`_CR1 = (`$INSTANCE_NAME`_COMP_3P0PF |
                                `$INSTANCE_NAME`_DIV2_DISABLE |
                                `$INSTANCE_NAME`_GAIN_0DB);  

        /* 
            Set SC Block CR2 Control Register fields to support TIA mode
                Bias - Low
                r20_r40 - 40K, n/a for TIA mode
                redc - set by SetResFB() API
                rval - set by SetCapFB() API
                PGA Ground Ref - disable, n/a for TIA mode
        */
        `$INSTANCE_NAME`_CR2 = (`$INSTANCE_NAME`_BIAS_LOW |
                                `$INSTANCE_NAME`_R20_40B_40K |
                                `$INSTANCE_NAME`_GNDVREF_DI);
                          
        /* Set default resistive feedback value */
        `$INSTANCE_NAME`_SetResFB(`$INSTANCE_NAME`_INIT_RES_FEEDBACK);
      
        /* Set default capacitive feedback value */
        `$INSTANCE_NAME`_SetCapFB(`$INSTANCE_NAME`_INIT_CAP_FEEDBACK);

        /* Set default power */
        `$INSTANCE_NAME`_SetPower(`$INSTANCE_NAME`_INIT_POWER);
    }

    /* Enable the Amp */
    `$INSTANCE_NAME`_PWRMGR |= `$INSTANCE_NAME`_ACT_PWR_EN;
   
    /* Enable SC Block boost clock control for low Vdda operation */
    #if(`$INSTANCE_NAME`_MIN_VDDA)
        `$INSTANCE_NAME`_bst_clk_Enable();
        `$INSTANCE_NAME`_BSTCLK |= `$INSTANCE_NAME`_BST_CLK_EN;
    #endif   
    
    /* Enable Pump only if voltage is below threshold */
    if (`$INSTANCE_NAME`_CYDEV_VDDA_MV < `$INSTANCE_NAME`_VDDA_THRESHOLD_MV)
    {
        `$INSTANCE_NAME`_SC_MISC |= `$INSTANCE_NAME`_PUMP_FORCE;
    }
}

/*******************************************************************************
* Function Name: `$INSTANCE_NAME`_Stop
********************************************************************************
* Summary:
*  Disables power to SC block.
*
* Parameters:  
*  void 
*
* Return: 
*  void 
*
* Theory: 
*
* Side Effects:
*
*******************************************************************************/
void `$INSTANCE_NAME`_Stop(void)
{
    /* Disable pumps only if only one SC block is in use */
    if (`$INSTANCE_NAME`_PWRMGR == `$INSTANCE_NAME`_ACT_PWR_EN)
    {
        `$INSTANCE_NAME`_SC_MISC &= ~`$INSTANCE_NAME`_PUMP_FORCE;
    }
   
    /* Disble power to the Amp */
    `$INSTANCE_NAME`_PWRMGR &= ~`$INSTANCE_NAME`_ACT_PWR_EN;
     
	 /* Disable SC Block boost clk control, if used (MinVdda < 2.7V) */
#if(`$INSTANCE_NAME`_MIN_VDDA)	
    `$INSTANCE_NAME`_BSTCLK &= ~`$INSTANCE_NAME`_BST_CLK_EN;
    `$INSTANCE_NAME`_bst_clk_Disable();
#endif 
}

/*******************************************************************************
* Function Name: `$INSTANCE_NAME`_SetPower
********************************************************************************
* Summary:
*  Set the drive power of the TIA
*
* Parameters:  
*  power:   Sets power level between (0) and (3) high power
*
* Return: 
*  void 
*
* Theory: 
*
* Side Effects:
*
*******************************************************************************/
void `$INSTANCE_NAME`_SetPower(uint8 power) 
{
    uint8 tmpCR;
    
    /* Sets drive bits in SC Block Control Register 1:  SCxx_CR[1:0] */    
    tmpCR = `$INSTANCE_NAME`_CR1 & ~`$INSTANCE_NAME`_DRIVE_MASK;
    tmpCR |= (power & `$INSTANCE_NAME`_DRIVE_MASK);
    `$INSTANCE_NAME`_CR1 = tmpCR;  
}

/*******************************************************************************
* Function Name: `$INSTANCE_NAME`_SetResFB
********************************************************************************
* Summary:
*  This function sets value for the resistive feedback of the amplifier.
*
* Parameters:  
*  res_feedback:  specifies resistive feedback value (see header file for gain values.)
*
* Return: 
*  void 
*
* Theory: 
*
* Side Effects:
*
*******************************************************************************/
void `$INSTANCE_NAME`_SetResFB(uint8 res_feedback)
{
   /* Only set new gain if it is a valid gain */
   if( res_feedback <= `$INSTANCE_NAME`_RES_FEEDBACK_MAX)
   {
      /* Clear SCxx_CR2 rval bits - SCxx_CR2[6:4]*/
      `$INSTANCE_NAME`_CR2 &= ~`$INSTANCE_NAME`_RVAL_MASK;

      /* Set resistive feedback value */
      `$INSTANCE_NAME`_CR2 |= (res_feedback << 4);
   }
}

/*******************************************************************************
* Function Name: `$INSTANCE_NAME`_SetCapFB
********************************************************************************
* Summary:
*  This function sets the value for the capacitive feedback of the amplifier.
*
* Parameters:  
*  cap_feedback:  specifies capacitive feedback value (see header file for gain values.)
*
* Return: 
*  void 
*
* Theory: 
*
* Side Effects:
*
*******************************************************************************/
void `$INSTANCE_NAME`_SetCapFB(uint8 cap_feedback)
{
   /* Only set new gain if it is a valid gain */
   if( cap_feedback <= `$INSTANCE_NAME`_CAP_FEEDBACK_MAX)
   {
      /* Clear SCxx_CR2 redc bits -  - CR2[3:2] */
      `$INSTANCE_NAME`_CR2 &= ~`$INSTANCE_NAME`_REDC_MASK;

      /* Set redc capacitive feedback value */
      `$INSTANCE_NAME`_CR2 |= (cap_feedback << 2);
   }
}

/* [] END OF FILE */

