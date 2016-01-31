/*******************************************************************************
* File Name: `$INSTANCE_NAME`.c
* Version `$CY_MAJOR_VERSION`.`$CY_MINOR_VERSION`
*
* Description:
*  This file provides the source code to the API for the Comparator component
*
* Note:
*  None
*
********************************************************************************
* Copyright 2008-2012, Cypress Semiconductor Corporation.  All rights reserved.
* You may use this file only in accordance with the license, terms, conditions, 
* disclaimers, and limitations in the end user license agreement accompanying 
* the software package with which this file was provided.
*******************************************************************************/

#include "`$INSTANCE_NAME`.h"

uint8 `$INSTANCE_NAME`_initVar = 0u;


/* static `$INSTANCE_NAME`_backupStruct  `$INSTANCE_NAME`_backup; */
#if (CY_PSOC5A)
    static `$INSTANCE_NAME`_LOWPOWER_BACKUP_STRUCT  `$INSTANCE_NAME`_lowPowerBackup;
#endif /* CY_PSOC5A */

/* variable to decide whether or not to restore the control register in 
   Enable() API for PSoC5A only */
#if (CY_PSOC5A)
    uint8 `$INSTANCE_NAME`_restoreReg = 0u;
#endif /* CY_PSOC5A */


/*******************************************************************************
* Function Name: `$INSTANCE_NAME`_Init
********************************************************************************
*
* Summary:
*  Initialize to the schematic state
* 
* Parameters:
*  void
*
* Return:
*  void
*
*******************************************************************************/
void `$INSTANCE_NAME`_Init(void) `=ReentrantKeil($INSTANCE_NAME . "_Init")`
{
    /* Set default speed/power */
    `$INSTANCE_NAME`_SetSpeed(`$INSTANCE_NAME`_DEFAULT_SPEED);

    /* Set default Hysteresis */
    if ( `$INSTANCE_NAME`_DEFAULT_HYSTERESIS == 0u )
    {
        `$INSTANCE_NAME`_CR |= `$INSTANCE_NAME`_HYST_OFF;
    }
    else
    {
        `$INSTANCE_NAME`_CR &= ~`$INSTANCE_NAME`_HYST_OFF; 
    }
    
    /* Power down override feature is not supported for PSoC5A. */
    #if (CY_PSOC3 || CY_PSOC5LP)
        /* Set default Power Down Override */
        if ( `$INSTANCE_NAME`_DEFAULT_PWRDWN_OVRD == 0u )
        {
            `$INSTANCE_NAME`_CR &= ~`$INSTANCE_NAME`_PWRDWN_OVRD;
        }
        else 
        {
            `$INSTANCE_NAME`_CR |= `$INSTANCE_NAME`_PWRDWN_OVRD;
        }
    #endif /* CY_PSOC3 || CY_PSOC5LP */
    
    /* Set mux always on logic */
    `$INSTANCE_NAME`_CR |= `$INSTANCE_NAME`_MX_AO;

    /* Set default sync */
    `$INSTANCE_NAME`_CLK &= ~`$INSTANCE_NAME`_SYNCCLK_MASK;
    if ( `$INSTANCE_NAME`_DEFAULT_BYPASS_SYNC == 0u )
    {
        `$INSTANCE_NAME`_CLK |= `$INSTANCE_NAME`_SYNC_CLK_EN;
    }
    else
    {
        `$INSTANCE_NAME`_CLK |= `$INSTANCE_NAME`_BYPASS_SYNC;
    }
}


/*******************************************************************************
* Function Name: `$INSTANCE_NAME`_Enable
********************************************************************************
*
* Summary:
*  Enable the Comparator
* 
* Parameters:
*  void
*
* Return:
*  void
*
*******************************************************************************/
void `$INSTANCE_NAME`_Enable(void) `=ReentrantKeil($INSTANCE_NAME . "_Enable")`
{
    `$INSTANCE_NAME`_PWRMGR |= `$INSTANCE_NAME`_ACT_PWR_EN;
    `$INSTANCE_NAME`_STBY_PWRMGR |= `$INSTANCE_NAME`_STBY_PWR_EN;
     
     /* This is to restore the value of register CR which is saved 
    in prior to the modification in stop() API */
    #if (CY_PSOC5A)
        if(`$INSTANCE_NAME`_restoreReg == 1u)
        {
            `$INSTANCE_NAME`_CR = `$INSTANCE_NAME`_lowPowerBackup.compCRReg;

            /* Clear the flag */
            `$INSTANCE_NAME`_restoreReg = 0u;
        }
    #endif /* CY_PSOC5A */
}


/*******************************************************************************
* Function Name: `$INSTANCE_NAME`_Start
********************************************************************************
*
* Summary:
*  The start function initializes the Analog Comparator with the default values.
*
* Parameters:
*  void
*
* Return:
*  void 
*
* Global variables:
*  `$INSTANCE_NAME`_initVar: Is modified when this function is called for the 
*   first time. Is used to ensure that initialization happens only once.
*  
*******************************************************************************/
void `$INSTANCE_NAME`_Start(void) `=ReentrantKeil($INSTANCE_NAME . "_Start")`
{

    if ( `$INSTANCE_NAME`_initVar == 0u )
    {
        `$INSTANCE_NAME`_Init();
        
        `$INSTANCE_NAME`_initVar = 1u;
    }   

    /* Enable power to comparator */
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
*******************************************************************************/
void `$INSTANCE_NAME`_Stop(void) `=ReentrantKeil($INSTANCE_NAME . "_Stop")`
{
    /* Disable power to comparator */
    `$INSTANCE_NAME`_PWRMGR &= ~`$INSTANCE_NAME`_ACT_PWR_EN;
    `$INSTANCE_NAME`_STBY_PWRMGR &= ~`$INSTANCE_NAME`_STBY_PWR_EN;    

    #if (CY_PSOC5A)
        /* Enable the variable */
        `$INSTANCE_NAME`_restoreReg = 1u;

        /* Save the control register before clearing it */
        `$INSTANCE_NAME`_lowPowerBackup.compCRReg = `$INSTANCE_NAME`_CR;
        `$INSTANCE_NAME`_CR = `$INSTANCE_NAME`_COMP_REG_CLR;
    #endif /* CY_PSOC5A */
}


/*******************************************************************************
* Function Name: `$INSTANCE_NAME`_SetSpeed
********************************************************************************
*
* Summary:
*  This function sets the speed of the Analog Comparator. The faster the speed
*  the more power that is used.
*
* Parameters:
*  speed: (uint8) Sets operation mode of Comparator
*
* Return:
*  void
*
*******************************************************************************/
void `$INSTANCE_NAME`_SetSpeed(uint8 speed) `=ReentrantKeil($INSTANCE_NAME . "_SetSpeed")`
{
    /* Clear and Set power level */    
    `$INSTANCE_NAME`_CR = (`$INSTANCE_NAME`_CR & ~`$INSTANCE_NAME`_PWR_MODE_MASK) |
                           (speed & `$INSTANCE_NAME`_PWR_MODE_MASK);

    /* Set trim value for high speed comparator */
    if(speed == `$INSTANCE_NAME`_HIGHSPEED)
    {
        /* PSoC5A */
        #if (CY_PSOC5A)
            `$INSTANCE_NAME`_TR = `$INSTANCE_NAME`_HS_TRIM_TR0;
        #endif /* CY_PSOC5A */
        
        /* PSoC3, PSoC5LP or later */
        #if (CY_PSOC3 || CY_PSOC5LP) 
            `$INSTANCE_NAME`_TR0 = `$INSTANCE_NAME`_HS_TRIM_TR0;
            `$INSTANCE_NAME`_TR1 = `$INSTANCE_NAME`_HS_TRIM_TR1;
        #endif /* CY_PSOC3 || CY_PSOC5LP */
    }
    else
    {
    /* PSoC5A */
        #if (CY_PSOC5A)
            `$INSTANCE_NAME`_TR = `$INSTANCE_NAME`_LS_TRIM_TR0;
        #endif /* CY_PSOC5A */
        
        /* PSoC3, PSoC5LP or later */
        #if (CY_PSOC3 || CY_PSOC5LP) 
            `$INSTANCE_NAME`_TR0 = `$INSTANCE_NAME`_LS_TRIM_TR0;
            `$INSTANCE_NAME`_TR1 = `$INSTANCE_NAME`_LS_TRIM_TR1;
        #endif /* CY_PSOC3 || CY_PSOC5LP */
    }

}


/*******************************************************************************
* Function Name: `$INSTANCE_NAME`_GetCompare
********************************************************************************
*
* Summary:
*  This function returns the comparator output value.
*
* Parameters:
*   None
*
* Return:
*  (uint8)  0  if Pos_Input less than Neg_input
*           1  if Pos_Input greater than Neg_input.
*
*******************************************************************************/
uint8 `$INSTANCE_NAME`_GetCompare(void) `=ReentrantKeil($INSTANCE_NAME . "_GetCompare")`
{
    return( `$INSTANCE_NAME`_WRK & `$INSTANCE_NAME`_CMP_OUT_MASK);
}


/*******************************************************************************
* Function Name: `$INSTANCE_NAME`_trimAdjust
********************************************************************************
*
* Summary:
*  This function adjusts the value in the low nibble/high nibble of the Analog 
*  Comparator trim register
*
* Parameters:  
*  nibble:
*      0 -- adjusts the value in the low nibble
*      1 -- adjusts the value in the high nibble
*
* Return:
*  None
*
* Theory: 
*  Function assumes comparator block is setup for trim adjust.
*  Intended to be called from Comp_ZeroCal()
* 
* Side Effects:
*  Routine uses a course 1ms delay following each trim adjustment to allow 
*  the comparator output to respond.
*
*******************************************************************************/
void `$INSTANCE_NAME`_trimAdjust(uint8 nibble) `=ReentrantKeil($INSTANCE_NAME . "_trimAdjust")`
{
    uint8 trimCnt, trimCntMax;
    uint8 cmpState;   

    /* get current state of comparator output */
    cmpState = `$INSTANCE_NAME`_WRK & `$INSTANCE_NAME`_CMP_OUT_MASK;
    
    if (nibble == 0u)
    {    
        /* if comparator output is high, negative offset adjust is required */
        if ( cmpState != 0u )
        {
            /* PSoC5A */
            #if (CY_PSOC5A)
                `$INSTANCE_NAME`_TR |= `$INSTANCE_NAME`_CMP_TRIM1_DIR;
            #endif /* CY_PSOC5A */
            
            /* PSoC3, PSoC5LP or later */
            #if (CY_PSOC3 || CY_PSOC5LP)
                `$INSTANCE_NAME`_TR0 |= `$INSTANCE_NAME`_CMP_TR0_DIR;
            #endif /* CY_PSOC3 || CY_PSOC5LP */
        }
    }
    else
    {
        /* if comparator output is low, positive offset adjust is required */
        if ( cmpState == 0u )
        {
            /* PSoC5A */
            #if (CY_PSOC5A)
                `$INSTANCE_NAME`_TR |= `$INSTANCE_NAME`_CMP_TRIM2_DIR; 
            #endif /* CY_PSOC5A */
            
            /* PSoC3, PSoC5LP or later */
            #if (CY_PSOC3 || CY_PSOC5LP)
                `$INSTANCE_NAME`_TR1 |= `$INSTANCE_NAME`_CMP_TR1_DIR;
            #endif /* CY_PSOC3 || CY_PSOC5LP */
        }
    }

    /* Increment trim value until compare output changes state */
	
    /* PSoC5A */
	#if (CY_PSOC5A)
	trimCntMax = 7;
    #endif
	
	/* PSoC3, PSoC5LP or later */
	#if (CY_PSOC3 || CY_PSOC5LP)
	if(nibble == 0u)
	{
		trimCntMax = 15;
	}
	else
	{
		trimCntMax = 7;
	}
	#endif
	
    for ( trimCnt = 0; trimCnt < trimCntMax; trimCnt++ )
	{
        if (nibble == 0u)
        {
            /* PSoC5A */
            #if (CY_PSOC5A)
                `$INSTANCE_NAME`_TR += 1u;
            #endif /* CY_PSOC5A */
            
            /* PSoC3, PSoC5LP or later */
            #if (CY_PSOC3 || CY_PSOC5LP)
                `$INSTANCE_NAME`_TR0 += 1u;
            #endif /* CY_PSOC3 || CY_PSOC5LP */
        }
        else
        {
            /* PSoC5A */
            #if (CY_PSOC5A)
                `$INSTANCE_NAME`_TR += 0x10u;
            #endif /* CY_PSOC5A */
            
            /* PSoC3, PSoC5LP or later */
            #if (CY_PSOC3 || CY_PSOC5LP)
                `$INSTANCE_NAME`_TR1 += 1u;
            #endif /* CY_PSOC3 || CY_PSOC5LP */
        }
        
        CyDelayUs(10);
        
        /* Check for change in comparator output */
        if ((`$INSTANCE_NAME`_WRK & `$INSTANCE_NAME`_CMP_OUT_MASK) != cmpState)
        {
            break;      /* output changed state, trim phase is complete */
        }        
    }    
}


/*******************************************************************************
* Function Name: `$INSTANCE_NAME`_ZeroCal
********************************************************************************
*
* Summary:
*  This function calibrates the offset of the Analog Comparator.
*
* Parameters:
*  None
*
* Return:
*  (uint16)  value written in trim register when calibration complete.
*
* Theory: 
*  This function is used to optimize the calibration for user specific voltage
*  range.  The comparator trim is adjusted to counter transistor offsets
*   - offset is defined as positive if the output transitions to high before inP
*     is greater than inN
*   - offset is defined as negative if the output transitions to high after inP
*     is greater than inP
*
*  PSoC5A
*  The Analog Comparator provides 1 byte for offset trim.  The byte contains two
*  4 bit trim fields - one is a course trim and the other allows smaller
*  offset adjustments only for slow modes.
*  - low nibble - fine trim
*  - high nibble - course trim
*  PSoC3, PSoC5LP or later
*  The Analog Comparator provides 2 bytes for offset trim.  The bytes contain two
*  5 bit trim fields - one is a course trim and the other allows smaller
*  offset adjustments only for slow modes.
*  - TR0 - fine trim
*  - TR1 - course trim
*
*  Trim algorithm is a two phase process
*  The first phase performs course offset adjustment
*  The second phase serves one of two purposes depending on the outcome of the
*  first phase
*  - if the first trim value was maxed out without a comparator output 
*    transition, more offset will be added by adjusting the second trim value.
*  - if the first trim phase resulted in a comparator output transition, the
*    second trim value will serve as fine trim (in the opposite direction)to
*    ensure the offset is < 1 mV.
*
* Trim Process:   
*  1) User applies a voltage to the negative input.  Voltage should be in the
*     comparator operating range or an average of the operating voltage range.
*  2) Clear registers associated with analog routing to the positive input.
*  3) Disable Hysteresis
*  4) Set the calibration bit to short the negative and positive inputs to
*     the users calibration voltage.
*  5) Clear the TR register  ( TR = 0x00 )
*  ** LOW MODES
*  6) Check if compare output is high, if so, set the MSb of course trim field 
*     to a 1.
*  7) Increment the course trim field until the compare output changes
*  8) Check if compare output is low, if so, set the MSb of fine trim field
*     to a 1.
*  9) Increment the fine trim field until the compare output changes
*  ** FAST MODE - skip the steps 8,9
*
* Side Effects:
*  Routine clears analog routing associated with the comparator positive input.  
*  This may affect routing of signals from other components that are connected
*  to the positive input of the comparator.
*
*******************************************************************************/
uint16 `$INSTANCE_NAME`_ZeroCal(void) `=ReentrantKeil($INSTANCE_NAME . "_ZeroCal")`
{
    uint8 tmpSW0;
    uint8 tmpSW2;
    uint8 tmpSW3;
    uint8 tmpCR;

    /* Save a copy of routing registers associated with inP */
    tmpSW0 = `$INSTANCE_NAME`_SW0;
    tmpSW2 = `$INSTANCE_NAME`_SW2;
    tmpSW3 = `$INSTANCE_NAME`_SW3;

     /* Clear routing for inP, retain routing for inN */
    `$INSTANCE_NAME`_SW0 = 0x00u;
    `$INSTANCE_NAME`_SW2 = 0x00u;
    `$INSTANCE_NAME`_SW3 = tmpSW3 & ~`$INSTANCE_NAME`_CMP_SW3_INPCTL_MASK;

    /* Preserve original configuration
     * - turn off Hysteresis
     * - set calibration bit - shorts inN to inP
    */
    tmpCR = `$INSTANCE_NAME`_CR;
    `$INSTANCE_NAME`_CR |= (`$INSTANCE_NAME`_CAL_ON | `$INSTANCE_NAME`_HYST_OFF);
    
    /* Write default low values to trim register - no offset adjust */
    /* PSoC5A */
    #if (CY_PSOC5A)
        `$INSTANCE_NAME`_TR = `$INSTANCE_NAME`_DEFAULT_CMP_TRIM;
    #endif /* CY_PSOC5A */
    
    /* PSoC3, PSoC5LP or later */
    #if (CY_PSOC3 || CY_PSOC5LP)
        `$INSTANCE_NAME`_TR0 = `$INSTANCE_NAME`_DEFAULT_CMP_TRIM;
        `$INSTANCE_NAME`_TR1 = `$INSTANCE_NAME`_DEFAULT_CMP_TRIM;
    #endif /* CY_PSOC3 || CY_PSOC5LP */
	
	/* Two phase trim - slow modes, one phase trim - for fast */ 
    if ( (`$INSTANCE_NAME`_CR & `$INSTANCE_NAME`_PWR_MODE_MASK) == `$INSTANCE_NAME`_PWR_MODE_FAST)
    {
        `$INSTANCE_NAME`_trimAdjust(0);
    }
    else /* default to trim for fast modes */
    {
        `$INSTANCE_NAME`_trimAdjust(1);
		`$INSTANCE_NAME`_trimAdjust(0);
    }
   
    /* Restore Config Register */
    `$INSTANCE_NAME`_CR = tmpCR;
    
    /* Restore routing registers for inP */
    `$INSTANCE_NAME`_SW0 = tmpSW0;
    `$INSTANCE_NAME`_SW2 = tmpSW2;
    `$INSTANCE_NAME`_SW3 = tmpSW3;
    
    /* PSoC5A */
    #if (CY_PSOC5A)
        return (uint16) `$INSTANCE_NAME`_TR;
    #endif /* CY_PSOC5A */
    
    /* PSoC3, PSoC5LP or later */
    #if (CY_PSOC3 || CY_PSOC5LP)
        return (((uint16)`$INSTANCE_NAME`_TR1 << 8) | (`$INSTANCE_NAME`_TR0));        
    #endif /* CY_PSOC3 || CY_PSOC5LP */
}


/*******************************************************************************
* Function Name: `$INSTANCE_NAME`_LoadTrim
********************************************************************************
*
* Summary:
*  This function stores a value in the Analog Comparator trim register.
*
* Parameters:  
*  uint8 trimVal - trim value.  This value is the same format as the value 
*  returned by the _ZeroCal routine.
*
* Return:
*  None
*
*******************************************************************************/
void `$INSTANCE_NAME`_LoadTrim(uint16 trimVal) `=ReentrantKeil($INSTANCE_NAME . "_LoadTrim")`
{
    /* Stores value in the Analog Comparator trim register */
    /* PSoC5A */
    #if (CY_PSOC5A)
        `$INSTANCE_NAME`_TR = (uint8) trimVal;
    #endif /* CY_PSOC5A */
    
    /* PSoC3, PSoC5LP or later */
    #if (CY_PSOC3 || CY_PSOC5LP)
        /* Stores value in the Analog Comparator trim register for P-type load */
        `$INSTANCE_NAME`_TR0 = (uint8) trimVal;
        
        /* Stores value in the Analog Comparator trim register for N-type load */
        `$INSTANCE_NAME`_TR1 = (uint8) (trimVal >> 8); 
    #endif /* CY_PSOC3 || CY_PSOC5LP */
}


#if (CY_PSOC3 || CY_PSOC5LP)

    /*******************************************************************************
    * Function Name: `$INSTANCE_NAME`_PwrDwnOverrideEnable
    ********************************************************************************
    *
    * Summary:
    *  This is the power down over-ride feature. This function ignores sleep 
    *  parameter and allows the component to stay active during sleep mode.
    *
    * Parameters:
    *  None
    *
    * Return:
    *  None
    *
    *******************************************************************************/
    void `$INSTANCE_NAME`_PwrDwnOverrideEnable(void) `=ReentrantKeil($INSTANCE_NAME . "_PwrDwnOverrideEnable")`
    {
        /* Set the pd_override bit in CMP_CR register */
        `$INSTANCE_NAME`_CR |= `$INSTANCE_NAME`_PWRDWN_OVRD;
    }


    /*******************************************************************************
    * Function Name: `$INSTANCE_NAME`_PwrDwnOverrideDisable
    ********************************************************************************
    *
    * Summary:
    *  This is the power down over-ride feature. This allows the component to stay
    *  inactive during sleep.
    *
    * Parameters:
    *  None
    *
    * Return:
    *  None
    *
    *******************************************************************************/
    void `$INSTANCE_NAME`_PwrDwnOverrideDisable(void) `=ReentrantKeil($INSTANCE_NAME . "_PwrDwnOverrideDisable")`
    {
        /* Reset the pd_override bit in CMP_CR register */
        `$INSTANCE_NAME`_CR &= ~`$INSTANCE_NAME`_PWRDWN_OVRD;
    }
#endif /* (CY_PSOC3 || CY_PSOC5LP) */


/* [] END OF FILE */
