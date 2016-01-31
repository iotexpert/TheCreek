/*******************************************************************************
* File Name: `$INSTANCE_NAME`.c  
* Version `$CY_MAJOR_VERSION`.`$CY_MINOR_VERSION`
*
* Description:
*  This file provides the source code to the API for the MIXER Component.
*
* Note:
*
*******************************************************************************
* Copyright 2008-2011, Cypress Semiconductor Corporation.  All rights reserved.
* You may use this file only in accordance with the license, terms, conditions, 
* disclaimers, and limitations in the end user license agreement accompanying 
* the software package with which this file was provided.
*******************************************************************************/

#include "`$INSTANCE_NAME`.h"

#if (`$INSTANCE_NAME`_CYDEV_VDDA_MV < `$INSTANCE_NAME`_MINIMUM_VDDA_THRESHOLD_MV)
    #include "`$INSTANCE_NAME`_bst_clk.h"
#endif /* `$INSTANCE_NAME`_MIN_VDDA */

#if (`$INSTANCE_NAME`_LO_SOURCE == `$INSTANCE_NAME`_LO_SOURCE_INTERNAL)
    #include "`$INSTANCE_NAME`_aclk.h"
#endif /* `$INSTANCE_NAME`_LO_SOURCE */

/* Fixed configuration of SC Block only performed once */
uint8 `$INSTANCE_NAME`_initVar = 0u;

/* To restore the registers for PSoC5 ES1 */
#if (CY_PSOC5_ES1)
    uint8 `$INSTANCE_NAME`_restoreReg = 0u;
#endif /* CY_PSOC5_ES1 */

/* Only for PSoC5 ES1 */
#if (CY_PSOC5_ES1)
    static `$INSTANCE_NAME`_LOWPOWER_BACKUP_STRUCT  `$INSTANCE_NAME`_lowPowerBackup;
#endif /* CY_PSOC5_ES1 */


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
*  Yes
*
*******************************************************************************/
void `$INSTANCE_NAME`_Init(void) `=ReentrantKeil($INSTANCE_NAME . "_Init")`
{
    uint8 tempCR2 = 0u;

    /* Configure SC Block based on selected Mixer type */
    /* Continuous Time (up) mixer */
    if (`$INSTANCE_NAME`_MIXER_TYPE == `$INSTANCE_NAME`_CTMIXER) 
    {
        /* SC Block mode -  - SCxx_CR0[3:1] */
        `$INSTANCE_NAME`_CR0_REG = `$INSTANCE_NAME`_MODE_CTMIXER;   

        /* SC Block CR1 */ 
        `$INSTANCE_NAME`_CR1_REG = (`$INSTANCE_NAME`_COMP_4P35PF  |
                                `$INSTANCE_NAME`_DIV2_DISABLE |
                                `$INSTANCE_NAME`_GAIN_0DB);                                 

        /* SC Block CR2 */
        tempCR2 = (`$INSTANCE_NAME`_BIAS_LOW |
                   `$INSTANCE_NAME`_REDC_01 |
                   `$INSTANCE_NAME`_GNDVREF_DI);

        /* 
            Set SC Block resistor values based on Local Oscillator being above 
            or below 100kHz - controled by r20_40 (Rmix) bit for CT mode mixer 
        */

          if (`$INSTANCE_NAME`_LO_CLOCK_FREQ < `$INSTANCE_NAME`_LO_CLOCK_FREQ_100_KHz)
          {
              tempCR2 |= `$INSTANCE_NAME`_R20_40B_40K; 
          }
          else
          {
              tempCR2 |= `$INSTANCE_NAME`_R20_40B_20K;
          }
        `$INSTANCE_NAME`_CR2_REG = tempCR2;
    }
    else
    {   /* Discrete Time (down) mixer */
        /* SC Block mode -  - SCxx_CR0[3:1] */
        `$INSTANCE_NAME`_CR0_REG = `$INSTANCE_NAME`_MODE_DTMIXER;       

        /* SC Block CR1 */ 
        `$INSTANCE_NAME`_CR1_REG = (`$INSTANCE_NAME`_COMP_4P35PF  |
                                `$INSTANCE_NAME`_DIV2_ENABLE |
                                `$INSTANCE_NAME`_GAIN_0DB);

        /* SC Block CR2 */
        tempCR2 = (`$INSTANCE_NAME`_BIAS_LOW |`$INSTANCE_NAME`_GNDVREF_DI);

        /* 
            Set SC Block resistor values based on Local Oscillator beign above 
            or below 100kHz - set r20_40 (input resitance) and rval (feedback) 
            resistor values seperately for DT mode
        */
        if (`$INSTANCE_NAME`_LO_CLOCK_FREQ < `$INSTANCE_NAME`_LO_CLOCK_FREQ_100_KHz)
        {
            tempCR2 |= (`$INSTANCE_NAME`_R20_40B_40K | `$INSTANCE_NAME`_RVAL_40K); 
        }
        else
        {
            tempCR2 |= (`$INSTANCE_NAME`_R20_40B_20K | `$INSTANCE_NAME`_RVAL_20K);
        }
        `$INSTANCE_NAME`_CR2_REG = tempCR2; 
    }/* end if - Mixer Type */

    /* Enable SC Block clocking */
    `$INSTANCE_NAME`_CLK_REG |= `$INSTANCE_NAME`_CLK_EN;

    /* Set default power */
    `$INSTANCE_NAME`_SetPower(`$INSTANCE_NAME`_INIT_POWER);
    
    /* Set 50 % Duty cycle for LO clock if LO source is internal */
    #if(`$INSTANCE_NAME`_LO_SOURCE == `$INSTANCE_NAME`_LO_SOURCE_INTERNAL)
        `$INSTANCE_NAME`_aclk_SetMode(CYCLK_DUTY);
    #endif /* `$INSTANCE_NAME`_LO_SOURCE == `$INSTANCE_NAME`_LO_SOURCE_INTERNAL */

}


/*******************************************************************************
* Function Name: `$INSTANCE_NAME`_Enable
********************************************************************************
*  
* Summary: 
*  Enables the Mixer block operation
*
* Parameters:  
*  void
*
* Return: 
*  void
*
* Reentrant:
*  Yes
*
*******************************************************************************/
void `$INSTANCE_NAME`_Enable(void) `=ReentrantKeil($INSTANCE_NAME . "_Enable")`
{
    /* if PSoC5 ES1, load the registers from the backup structure */
    #if(CY_PSOC5_ES1)
        if(`$INSTANCE_NAME`_restoreReg)
        {
            `$INSTANCE_NAME`_CR1_REG = `$INSTANCE_NAME`_lowPowerBackup.CR1_REG;
            `$INSTANCE_NAME`_CR2_REG = `$INSTANCE_NAME`_lowPowerBackup.CR2_REG;
        
             /* Clear the flag */
             `$INSTANCE_NAME`_restoreReg = 0u;
        }
    #endif /* CY_PSOC5_ES1 */
    /* Enable power to SC block in active mode */
    `$INSTANCE_NAME`_PM_ACT_CFG_REG |= `$INSTANCE_NAME`_ACT_PWR_EN;

    /* Enable power to SC block in Alternative active mode */
    `$INSTANCE_NAME`_PM_STBY_CFG_REG |= `$INSTANCE_NAME`_STBY_PWR_EN;

    /* Enable SC Block boost clock control for low Vdda operation */
    #if(`$INSTANCE_NAME`_CYDEV_VDDA_MV < `$INSTANCE_NAME`_MINIMUM_VDDA_THRESHOLD_MV)
    /* enable for Vdda < 2.7V */
        `$INSTANCE_NAME`_BSTCLK_REG |= `$INSTANCE_NAME`_BST_CLK_EN;  
        `$INSTANCE_NAME`_bst_clk_Enable();
    #endif /* `$INSTANCE_NAME`_MIN_VDDA */
    
    /* Enable LO_Internal clock if LO source is choosen as internal */
    #if(`$INSTANCE_NAME`_LO_SOURCE == `$INSTANCE_NAME`_LO_SOURCE_INTERNAL)
        /* Enable power to the LO clock */
        `$INSTANCE_NAME`_PWRMGR_ACLK_REG |= `$INSTANCE_NAME`_ACT_PWR_ACLK_EN;        
        `$INSTANCE_NAME`_STBY_PWRMGR_ACLK_REG |= `$INSTANCE_NAME`_STBY_PWR_ACLK_EN;
        
        /* Enable the clock */
        `$INSTANCE_NAME`_aclk_Enable();        
    #endif /* `$INSTANCE_NAME`_LO_SOURCE == `$INSTANCE_NAME`_LO_SOURCE_INTERNAL */

    /* PSoC3 ES2 or early, PSoC5 ES1 */
    #if (CY_PSOC3_ES2 || CY_PSOC5_ES1)
        /* Enable Pump only if voltage is below threshold */
        if (`$INSTANCE_NAME`_CYDEV_VDDA_MV < `$INSTANCE_NAME`_MINIMUM_VDDA_THRESHOLD_MV)
        {
            `$INSTANCE_NAME`_SC_MISC_REG |= `$INSTANCE_NAME`_PUMP_FORCE;
        }
    /* PSoC3 ES3 or later, PSoC5 ES2 or later */
    #elif (CY_PSOC3_ES3 || CY_PSOC5_ES2)
        /* Enable charge Pump clock for SC block */
        `$INSTANCE_NAME`_PUMP_CR1_REG |= `$INSTANCE_NAME`_PUMP_CR1_SC_CLKSEL;
        /* Enable Pump only if voltage is below threshold */
        if (`$INSTANCE_NAME`_CYDEV_VDDA_MV < `$INSTANCE_NAME`_MINIMUM_VDDA_THRESHOLD_MV)
        {
            `$INSTANCE_NAME`_SC_MISC_REG |= `$INSTANCE_NAME`_PUMP_FORCE;
        }
    #endif /* CY_PSOC3_ES2 || CY_PSOC5_ES1 */
}


/*******************************************************************************
* Function Name: `$INSTANCE_NAME`_Start
********************************************************************************
*
* Summary:
*  The start function initializes the SC block control registers based on the
*  Mixer type selected and and enables power to the SC block
*
* Parameters:  
*  void
*
* Return: 
*  void 
*
* Global variables:
*  `$INSTANCE_NAME`_initVar:  Used to check the initial configuration,
*  modified when this function is called for the first time.
*
* Theory: 
*  Full initialization of the SC Block configuration registers is only perfomed 
*  the first time the routine executes - global variable 
*  `$INSTANCE_NAME`_initVar is used indicate that the static configuration has 
*  been completed.
*
* Reentrant:
*  No
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
    /* Enable the power to the block */
    `$INSTANCE_NAME`_Enable();
    
}


/*******************************************************************************
* Function Name: `$INSTANCE_NAME`_Stop
********************************************************************************
*
* Summary:
*  Disables power to SC block.
*
* Parameters:  
*  void 
*
* Return: 
*  void 
*
* Reentrant: 
*  Yes
*
*******************************************************************************/
void `$INSTANCE_NAME`_Stop(void) `=ReentrantKeil($INSTANCE_NAME . "_Stop")`
{
    /* Disable pumps only if only one SC block is in use */
    if(`$INSTANCE_NAME`_PM_ACT_CFG_REG == `$INSTANCE_NAME`_ACT_PWR_EN)
    {
        `$INSTANCE_NAME`_SC_MISC_REG &= ~`$INSTANCE_NAME`_PUMP_FORCE;
    }
   
   /* If PSoC5 ES1, save the control registers using backup structure 
      and then zero out these registers */    
    #if(CY_PSOC5_ES1)
        /* Set the global variable which is used to restore the registers in 
           Enable API. */
        `$INSTANCE_NAME`_restoreReg = 1u;
        
        /* keep the backup for control registers */
        `$INSTANCE_NAME`_lowPowerBackup.CR1_REG = `$INSTANCE_NAME`_CR1_REG;
        `$INSTANCE_NAME`_lowPowerBackup.CR2_REG = `$INSTANCE_NAME`_CR2_REG;
        
        /* Zero out these control registers */
        `$INSTANCE_NAME`_CR1_REG &= ~(`$INSTANCE_NAME`_DRIVE_MASK | `$INSTANCE_NAME`_GAIN |
                                      `$INSTANCE_NAME`_COMP_MASK | `$INSTANCE_NAME`_DIV2);
                                      
        `$INSTANCE_NAME`_CR2_REG &= ~(`$INSTANCE_NAME`_BIAS | `$INSTANCE_NAME`_R20_40B |
                                      `$INSTANCE_NAME`_REDC_MASK | `$INSTANCE_NAME`_RVAL_MASK |
                                      `$INSTANCE_NAME`_GNDVREF);
    #endif /* CY_PSOC5_ES1 */
    
    /* Disble power to the Amp in Active mode template */
    `$INSTANCE_NAME`_PM_ACT_CFG_REG &= ~`$INSTANCE_NAME`_ACT_PWR_EN;

    /* Disble power to the Amp in Alternative Active mode template */
    `$INSTANCE_NAME`_PM_STBY_CFG_REG &= ~`$INSTANCE_NAME`_STBY_PWR_EN;
    

    /* Disable SC Block boost clk control, if used (MinVdda < 2.7V) */
    #if(`$INSTANCE_NAME`_CYDEV_VDDA_MV < `$INSTANCE_NAME`_MINIMUM_VDDA_THRESHOLD_MV)
        `$INSTANCE_NAME`_BSTCLK_REG &= ~`$INSTANCE_NAME`_BST_CLK_EN;
        `$INSTANCE_NAME`_bst_clk_Disable();
    #endif /* `$INSTANCE_NAME`_MIN_VDDA */  
    
    /* Disable aclk */
    #if(`$INSTANCE_NAME`_LO_SOURCE == `$INSTANCE_NAME`_LO_SOURCE_INTERNAL)
        /* Disable power to clock */
        `$INSTANCE_NAME`_PWRMGR_ACLK_REG &= ~`$INSTANCE_NAME`_ACT_PWR_ACLK_EN;
        `$INSTANCE_NAME`_STBY_PWRMGR_ACLK_REG &= ~`$INSTANCE_NAME`_STBY_PWR_ACLK_EN;

        /* Disable the clock */
        `$INSTANCE_NAME`_aclk_Disable();
    #endif /* `$INSTANCE_NAME`_LO_SOURCE == `$INSTANCE_NAME`_LO_SOURCE_INTERNAL */
}


/*******************************************************************************
* Function Name: `$INSTANCE_NAME`_SetPower
********************************************************************************
*
* Summary:
*  Set the drive power of the MIXER
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

    /* Sets drive bits in SC Block Control Register 1:  SCxx_CR[1:0] */    
    tmpCR = `$INSTANCE_NAME`_CR1_REG & ~`$INSTANCE_NAME`_DRIVE_MASK;
    tmpCR |= (power & `$INSTANCE_NAME`_DRIVE_MASK);
    `$INSTANCE_NAME`_CR1_REG = tmpCR;  
}


/* [] END OF FILE */
