/*******************************************************************************
* File Name: `$INSTANCE_NAME`.c  
* Version `$CY_MAJOR_VERSION`.`$CY_MINOR_VERSION`
*
*  Description:
*    This file provides the source code to the API for the Comparator component.
*
*   Note:
*     
*******************************************************************************
* Copyright 2008-2010, Cypress Semiconductor Corporation.  All rights reserved.
* You may use this file only in accordance with the license, terms, conditions, 
* disclaimers, and limitations in the end user license agreement accompanying 
* the software package with which this file was provided.
********************************************************************************/

#include "`$INSTANCE_NAME`.h"
#include "CyLib.h"

uint8 `$INSTANCE_NAME`_initVar = 0u;


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
* Reentrant:
*  Yes
*
*******************************************************************************/
void `$INSTANCE_NAME`_Init(void) `=ReentrantKeil($INSTANCE_NAME . "_Init")`
{
    /* Set default speed/power */
    `$INSTANCE_NAME`_SetSpeed(`$INSTANCE_NAME`_DEFAULT_SPEED);

    /* Set default Hysteresis */
    if ( `$INSTANCE_NAME`_DEFAULT_HYSTERESIS == 0u )
    {
         /* PSoC3 ES3 or later, PSoC5 ES2 or later */
         #if (`$INSTANCE_NAME`_PSOC3_ES3 || `$INSTANCE_NAME`_PSOC5_ES2)
            `$INSTANCE_NAME`_CR |= `$INSTANCE_NAME`_HYST_OFF;
         #endif /* `$INSTANCE_NAME`_PSOC3_ES3 || `$INSTANCE_NAME`_PSOC5_ES2 */
         
         /* PSoC3 ES2 or early, PSoC5 ES1 or early */
         #if (`$INSTANCE_NAME`_PSOC3_ES2 || `$INSTANCE_NAME`_PSOC5_ES1)
             `$INSTANCE_NAME`_CR &= ~`$INSTANCE_NAME`_HYST_OFF;
         #endif /* `$INSTANCE_NAME`_PSOC3_ES2 || `$INSTANCE_NAME`_PSOC5_ES1 */
    }
    else
    {
         /* PSoC3 ES3 or later, PSoC5 ES2 or later */
         #if (`$INSTANCE_NAME`_PSOC3_ES3 || `$INSTANCE_NAME`_PSOC5_ES2)
            `$INSTANCE_NAME`_CR &= ~`$INSTANCE_NAME`_HYST_OFF; 
         #endif /* `$INSTANCE_NAME`_PSOC3_ES3 || `$INSTANCE_NAME`_PSOC5_ES2 */
         
         /* PSoC3 ES2 or early, PSoC5 ES1 or early */
         #if (`$INSTANCE_NAME`_PSOC3_ES2 || `$INSTANCE_NAME`_PSOC5_ES1)
            `$INSTANCE_NAME`_CR |= `$INSTANCE_NAME`_HYST_OFF; 
         #endif /* `$INSTANCE_NAME`_PSOC3_ES2 || `$INSTANCE_NAME`_PSOC5_ES1 */  
    }
    
    /* Set default Power Down Override */
    if ( `$INSTANCE_NAME`_DEFAULT_PWRDWN_OVRD == 0u )
    {
        `$INSTANCE_NAME`_CR &= ~`$INSTANCE_NAME`_PWRDWN_OVRD;
    }
    else 
    {
        `$INSTANCE_NAME`_CR |= `$INSTANCE_NAME`_PWRDWN_OVRD;
    }

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
    /* Set trim value for high speed compator */
    if(`$INSTANCE_NAME`_DEFAULT_SPEED == `$INSTANCE_NAME`_HIGHSPEED)
    {
        /* PSoC3 ES2 or early, PSoC5 ES1 or early */
        #if (`$INSTANCE_NAME`_PSOC3_ES2 || `$INSTANCE_NAME`_PSOC5_ES1)
        `$INSTANCE_NAME`_TR = `$INSTANCE_NAME`_HS_TRIM_TR0;
        #endif 
        
        /* PSoC3 ES3 or later, PSoC5 ES2 or later */
        #if (`$INSTANCE_NAME`_PSOC3_ES3 || `$INSTANCE_NAME`_PSOC5_ES2) 
        `$INSTANCE_NAME`_TR0 = `$INSTANCE_NAME`_HS_TRIM_TR0;
        `$INSTANCE_NAME`_TR1 = `$INSTANCE_NAME`_HS_TRIM_TR1;
        #endif 
        
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
* Reentrant:
*  Yes
*
*******************************************************************************/
void `$INSTANCE_NAME`_Enable(void) `=ReentrantKeil($INSTANCE_NAME . "_Enable")`
{
    `$INSTANCE_NAME`_PWRMGR |= `$INSTANCE_NAME`_ACT_PWR_EN;
    `$INSTANCE_NAME`_STBY_PWRMGR |= `$INSTANCE_NAME`_STBY_PWR_EN;
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
*  `$INSTANCE_NAME`_initVar: Is modified when this function is called for the first 
*   time. Is used to ensure that initialization happens only once.
*  
* Reentrant:
*  No
*
*******************************************************************************/
void `$INSTANCE_NAME`_Start(void) 
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
* Reentrant:
*  Yes
*
*******************************************************************************/
void `$INSTANCE_NAME`_Stop(void) `=ReentrantKeil($INSTANCE_NAME . "_Stop")`
{
    /* Disable power to comparator */
    `$INSTANCE_NAME`_PWRMGR &= ~`$INSTANCE_NAME`_ACT_PWR_EN;
    `$INSTANCE_NAME`_STBY_PWRMGR &= ~`$INSTANCE_NAME`_STBY_PWR_EN;    
}


/*******************************************************************************
* Function Name: `$INSTANCE_NAME`_SetSpeed
********************************************************************************
*
* Summary:
*  This function sets the speed of the Analog Comparator.  The faster the speed
*  the more power that is used.
*
* Parameters:  
*  speed:   (uint8) Sets operation mode of Comparator
*
* Return:  
*  void 
*
* Reentrant:
*  Yes
*
*******************************************************************************/
void `$INSTANCE_NAME`_SetSpeed(uint8 speed) `=ReentrantKeil($INSTANCE_NAME . "_SetSpeed")`
{
    /* Clear and Set power level */    
    `$INSTANCE_NAME`_CR = (`$INSTANCE_NAME`_CR & ~`$INSTANCE_NAME`_PWR_MODE_MASK) |
                           (speed & `$INSTANCE_NAME`_PWR_MODE_MASK);
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
* Reentrant:
*  Yes
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
* Reentrant:
*  No
*
*******************************************************************************/
void `$INSTANCE_NAME`_trimAdjust(uint8 nibble)
{
    uint8 trimCnt;
    uint8 cmpState;   

    /* get current state of comparator output */
    cmpState = `$INSTANCE_NAME`_WRK & `$INSTANCE_NAME`_CMP_OUT_MASK;
    
    if (nibble == 0u)
    {    
        /* if comparator output is high, negative offset adjust is required */
        if ( cmpState != 0u )
        {
            /* PSoC3 ES2 or early, PSoC5 ES1 or early */
            #if (`$INSTANCE_NAME`_PSOC3_ES2 || `$INSTANCE_NAME`_PSOC5_ES1)
                `$INSTANCE_NAME`_TR |= `$INSTANCE_NAME`_CMP_TRIM1_DIR;
            #endif
            
            /* PSoC3 ES3 or later, PSoC5 ES2 or later */
            #if (`$INSTANCE_NAME`_PSOC3_ES3 || `$INSTANCE_NAME`_PSOC5_ES2)
                `$INSTANCE_NAME`_TR0 |= `$INSTANCE_NAME`_CMP_TR0_DIR;
            #endif
        }
    }
    else
    {
        /* if comparator output is low, positive offset adjust is required */
        if ( cmpState == 0u )
        {
            /* PSoC3 ES2 or early, PSoC5 ES1 or early */
            #if (`$INSTANCE_NAME`_PSOC3_ES2 || `$INSTANCE_NAME`_PSOC5_ES1)
                `$INSTANCE_NAME`_TR |= `$INSTANCE_NAME`_CMP_TRIM2_DIR; 
            #endif
            
            /* PSoC3 ES3 or later, PSoC5 ES2 or later */
            #if (`$INSTANCE_NAME`_PSOC3_ES3 || `$INSTANCE_NAME`_PSOC5_ES2)
                `$INSTANCE_NAME`_TR1 |= `$INSTANCE_NAME`_CMP_TR1_DIR;
            #endif
        }
    }

    /* Increment trim value until compare output changes state */
    for ( trimCnt = 0; trimCnt < 7; trimCnt++ )
    {
        if (nibble == 0u)
        {
            /* PSoC3 ES2 or early, PSoC5 ES1 or early */
            #if (`$INSTANCE_NAME`_PSOC3_ES2 || `$INSTANCE_NAME`_PSOC5_ES1)
                `$INSTANCE_NAME`_TR += 1u;
            #endif
            
            /* PSoC3 ES3 or later, PSoC5 ES2 or later */
            #if (`$INSTANCE_NAME`_PSOC3_ES3 || `$INSTANCE_NAME`_PSOC5_ES2)
                `$INSTANCE_NAME`_TR0 += 1u;
            #endif            
        }
        else
        {
            /* PSoC3 ES2 or early, PSoC5 ES1 or early */
            #if (`$INSTANCE_NAME`_PSOC3_ES2 || `$INSTANCE_NAME`_PSOC5_ES1)
                `$INSTANCE_NAME`_TR += 0x10u;
            #endif
            
            /* PSoC3 ES3 or later, PSoC5 ES2 or later */
            #if (`$INSTANCE_NAME`_PSOC3_ES3 || `$INSTANCE_NAME`_PSOC5_ES2)
                `$INSTANCE_NAME`_TR1 += 0x10u;
            #endif            
        }
        
        CyDelay(1);
        
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
*  range.  The comprator trim is adjusted to counter transitor offsets
*   - offset is defined as positive if the output transitions to high before inP
*     is greater than inN
*   - offset is defined as negative if the output transitions to high avter inP
*     is greater than inP
*
*  The Analog Comparator provides 1 byte for offset trim.  The byte contains two
*  4 bit trim fields - one is a course trim and the other allows smaller
*  offset adjustments.
*  - low nibble - fine trim for Fast mode, course trim for Slow mode
*  - high nibble - fine trim for Slow mode, course trim
*    for Fast mode
*  - trim[3] selects postive or negative offset adjust
*  - for low nibble: trim[3]is set high to add negative offset
*  - for high nibble: trim[3]is set high to add positive offset  
*
*  Trim algorithm is a two phase process
*  The first phase performs course offset adjustment
*  The second phase serves one of two purposes depending on the outcome of the
*  first phase
*  - if the first trim value was maxed out without a comparator output 
*    transition, more offset will be added by adjusting the second trim value.
*  - if the first trim phase resulted in a comparator output transition, the
*    second trim value will serve as fine trim (in the opposite direction)to
*    ensure the offset is < 1mv.
*
*  Trim Process:   
*  1) User applies a voltage to the negative input.  Voltage should in the
*     comparator operating range or an average of the operating voltage range.
*  2) Clear registers associted analog routing to the positive input.
*  3) Disable Hysteresis
*  4) Set the calibration bit to short the negative and positive inputs to
*     the users calibration voltage.
*  5) Clear the TR register  ( TR = 0x00 )
*  ** SLOW MODE
*  6) Check if compare output is high, if so, set trim[3] of lower nibble to a 1.
*  7) Increment trim[2:0] of lower nibble until the compare output changes
*     (course trim).
*  8) Check if compare output is low, if so, set trim[3] of higher nibble to a 1.
*  9) Increment trim[2:0] of higher nibble until the compare output changes state
*     (fine trim)
*  ** FAST MODE - change order of steps 6,7 vs. steps 8,9
*
* Side Effects:
*  Routine clears analog routing associated with the comparator positive input.  
*  This may affect routing of signals from other components that are connected
*  to the positive input of the comparator.
*
* Reentrant:
*  Yes
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
    /* PSoC3 ES2 or early, PSoC5 ES1 or early */
    #if (`$INSTANCE_NAME`_PSOC3_ES2 || `$INSTANCE_NAME`_PSOC5_ES1)
        `$INSTANCE_NAME`_TR = `$INSTANCE_NAME`_DEFAULT_CMP_TRIM;
    #endif
    
    /* PSoC3 ES3 or later, PSoC5 ES2 or later */
    #if (`$INSTANCE_NAME`_PSOC3_ES3 || `$INSTANCE_NAME`_PSOC5_ES2)
        `$INSTANCE_NAME`_TR0 = `$INSTANCE_NAME`_DEFAULT_CMP_TRIM;
        `$INSTANCE_NAME`_TR1 = `$INSTANCE_NAME`_DEFAULT_CMP_TRIM;
    #endif

    /* Two phase trim - mode determines which value is trimed first */   
    if ( (`$INSTANCE_NAME`_CR & `$INSTANCE_NAME`_PWR_MODE_MASK) == `$INSTANCE_NAME`_PWR_MODE_FAST)
    {
        `$INSTANCE_NAME`_trimAdjust(1);          /* course trim */
        `$INSTANCE_NAME`_trimAdjust(0);          /* fine trim */
    }
    else /* default to trim for slow mode */
    {
        `$INSTANCE_NAME`_trimAdjust(0);          /* course trim */
        `$INSTANCE_NAME`_trimAdjust(1);          /* fine trim */
    }
   
    /* Restore Config Register */
    `$INSTANCE_NAME`_CR = tmpCR;
    
    /* Restore routing registers for inP */
    `$INSTANCE_NAME`_SW0 = tmpSW0;
    `$INSTANCE_NAME`_SW2 = tmpSW2;
    `$INSTANCE_NAME`_SW3 = tmpSW3;
    
    /* PSoC3 ES2 or early, PSoC5 ES1 or early */
    #if (`$INSTANCE_NAME`_PSOC3_ES2 || `$INSTANCE_NAME`_PSOC5_ES1)
        return (uint16) `$INSTANCE_NAME`_TR;
    #endif
    
    /* PSoC3 ES3 or later, PSoC5 ES2 or later */
    #if (`$INSTANCE_NAME`_PSOC3_ES3 || `$INSTANCE_NAME`_PSOC5_ES2)
        return ((`$INSTANCE_NAME`_TR0)|(`$INSTANCE_NAME`_TR1));        
    #endif
}


/*******************************************************************************
* Function Name: `$INSTANCE_NAME`_LoadTrim
********************************************************************************
*
* Summary:
*  This function stores a value in the Analog Comparator trim register.
*
* Parameters:  
*  uint8    trimVal - trim value.  This value is the same format as the value 
*           returned by the _ZeroCal routine.
*
* Return:  
*  None
*
* Reentrant:
*  Yes
*
*******************************************************************************/
void `$INSTANCE_NAME`_LoadTrim(uint16 trimVal) `=ReentrantKeil($INSTANCE_NAME . "_LoadTrim")`
{
    /* Stores value in the Analog Comparator trim register */
    /* PSoC3 ES2 or early, PSoC5 ES1 or early */
    #if (`$INSTANCE_NAME`_PSOC3_ES2 || `$INSTANCE_NAME`_PSOC5_ES1)
        `$INSTANCE_NAME`_TR = (uint8) trimVal;
    #endif
    
    /* PSoC3 ES3 or later, PSoC5 ES2 or later */
    #if (`$INSTANCE_NAME`_PSOC3_ES3 || `$INSTANCE_NAME`_PSOC5_ES2)
        /* Stores value in the Analog Comparator trim register for P-type load */
        `$INSTANCE_NAME`_TR0 = (uint8) trimVal;
        
        /* Stores value in the Analog Comparator trim register for N-type load */
        `$INSTANCE_NAME`_TR1 = (uint8) (trimVal >> 8); 
    #endif
}


/*******************************************************************************
* Function Name: `$INSTANCE_NAME`_PwrDwnOverrideEnable
********************************************************************************
*
* Summary:
*  This is the power down over-ride feature. This function ignores sleep parameter.  
*  and allows the component to stay active during sleep mode.
*
* Parameters:  
*  None
*
* Return:  
*  None
*
* Reentrant:
*  Yes
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
* Reentrant:
*  Yes
*
*******************************************************************************/
void `$INSTANCE_NAME`_PwrDwnOverrideDisable(void) `=ReentrantKeil($INSTANCE_NAME . "_PwrDwnOverrideDisable")`
{
    /* Reset the pd_override bit in CMP_CR register */
    `$INSTANCE_NAME`_CR &= ~`$INSTANCE_NAME`_PWRDWN_OVRD;
}


/* [] END OF FILE */
