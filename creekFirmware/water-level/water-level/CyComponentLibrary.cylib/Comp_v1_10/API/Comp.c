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
* Copyright 2008-2009, Cypress Semiconductor Corporation.  All rights reserved.
* You may use this file only in accordance with the license, terms, conditions, 
* disclaimers, and limitations in the end user license agreement accompanying 
* the software package with which this file was provided.
********************************************************************************/



#include "cytypes.h"
#include "`$INSTANCE_NAME`.h"
#include "CyLib.h"

/* Local Function Prototypes */
void `$INSTANCE_NAME`_trim1Adjust(void);
void `$INSTANCE_NAME`_trim2Adjust(void);
void `$INSTANCE_NAME`_CalDelay(void);

static uint8   `$INSTANCE_NAME`_initVar = 0;

/*******************************************************************************
* Function Name: `$INSTANCE_NAME`_Start
********************************************************************************
* Summary:
*  The start function initializes the Analog Comparator with the default values.
*
* Parameters:   none
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
    if ( `$INSTANCE_NAME`_initVar == 0 )
    {
        `$INSTANCE_NAME`_initVar = 1;

        /* Set default speed/power */
        `$INSTANCE_NAME`_SetSpeed(`$INSTANCE_NAME`_DEFAULT_SPEED);

        /* Set default Hysteresis */
        if ( `$INSTANCE_NAME`_DEFAULT_HYSTERESIS == 0 )
        {
            `$INSTANCE_NAME`_CR |= `$INSTANCE_NAME`_HYST_OFF;
        }
        else
        {
            `$INSTANCE_NAME`_CR &= ~`$INSTANCE_NAME`_HYST_OFF;  
        }

        /* Set default sync */
        `$INSTANCE_NAME`_CLK &= ~`$INSTANCE_NAME`_SYNCCLK_MASK;
        if ( `$INSTANCE_NAME`_DEFAULT_BYPASS_SYNC == 0 )
        {
            `$INSTANCE_NAME`_CLK |= `$INSTANCE_NAME`_SYNC_CLK_EN;
        }
        else
        {
            `$INSTANCE_NAME`_CLK |= `$INSTANCE_NAME`_BYPASS_SYNC;
        }
    }

    /* Enable power to comparator */
    `$INSTANCE_NAME`_PWRMGR |= `$INSTANCE_NAME`_ACT_PWR_EN;
}


/*******************************************************************************
* Function Name: `$INSTANCE_NAME`_Stop
********************************************************************************
* Summary:
*  Powers down amplifier to lowest power state.
*
* Parameters:  
*   (void)
*
* Return: 
*   (void) 
*
* Theory: 
*
* Side Effects:
*
*******************************************************************************/
void `$INSTANCE_NAME`_Stop(void)
{
    /* Disable power to comparator */
    `$INSTANCE_NAME`_PWRMGR &= ~`$INSTANCE_NAME`_ACT_PWR_EN;
}


/*******************************************************************************
* Function Name: `$INSTANCE_NAME`_SetSpeed
********************************************************************************
* Summary:
*  This function sets the speed of the Analog Comparator.  The faster the speed
*  the more power that is used.
*
* Parameters:  
*  speed:   (uint8) Sets operation mode of Comparator
*
* Return:  
*  (void) 
*
* Theory: 
*
* Side Effects:
*
*******************************************************************************/
void `$INSTANCE_NAME`_SetSpeed(uint8 speed) 
{
    `$INSTANCE_NAME`_CR &= ~`$INSTANCE_NAME`_PWR_MODE_MASK;            /* Clear power level */
    `$INSTANCE_NAME`_CR |= (speed & `$INSTANCE_NAME`_PWR_MODE_MASK);   /* Set power level */
}


/*******************************************************************************
* Function Name: `$INSTANCE_NAME`_GetCompare
********************************************************************************
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
* Theory: 
*
* Side Effects:
*
*******************************************************************************/
uint8 `$INSTANCE_NAME`_GetCompare(void) 
{
    return( `$INSTANCE_NAME`_WRK & `$INSTANCE_NAME`_CMP_OUT_MASK);
}


/*******************************************************************************
* Function Name: `$INSTANCE_NAME`_ZeroCal
********************************************************************************
* Summary:
*  This function calibrates the offset of the Analog Comparator.
*
* Parameters:  
*  None
*
* Return:  
*  (uint8)  value written in trim register when calibration complete.
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
*  - low nibble is refered to as trim1 - fine trim for Fast mode, course trim
*    for Slow mode
*  - high nibble is refered to as trim2 - fine trim for Slow mode, course trim
*    for Fast mode
*  - trimX[3] selects postive or negative offset adjust
*  - for trim1: trim1[3]is set high to add negative offset
*  - for trim2: trim2[3]is set high to add positive offset  
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
*  6) Check if compare output is high, if so, set trim1[3] to a 1.
*  7) Increment trim1[2:0] until the compare output changes  (course trim).
*  8) Check if compare output is low, if so, set trim2[3] to a 1.
*  9) Increment trim2[2:0] until the compare output changes state (fine trim)
*  ** FAST MODE - change order of steps 6,7 vs. steps 8,9
*
* Side Effects:
*  Routine clears analog routing associated with the comparator positive input.  
*  This may affect routing of signals from other components that are connected
*  to the positive input of the comparator.
*
*******************************************************************************/
uint8 `$INSTANCE_NAME`_ZeroCal(void) 
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
    `$INSTANCE_NAME`_SW0 = 0x00;
    `$INSTANCE_NAME`_SW2 = 0x00;
    `$INSTANCE_NAME`_SW3 = tmpSW3 & ~`$INSTANCE_NAME`_CMP_SW3_INPCTL_MASK;

    /* Preserve original configuration
     * - turn off Hysteresis
     * - set calibration bit - shorts inN to inP
    */
    tmpCR = `$INSTANCE_NAME`_CR;
    `$INSTANCE_NAME`_CR |= (`$INSTANCE_NAME`_CAL_ON | `$INSTANCE_NAME`_HYST_OFF);
    
    /* Write default low values to trim register - no offset adjust */
    `$INSTANCE_NAME`_TR = 0x00;

    /* Two phase trim - mode determines which value is trimed first */   
    if ( (`$INSTANCE_NAME`_CR & `$INSTANCE_NAME`_PWR_MODE_MASK) == `$INSTANCE_NAME`_PWR_MODE_FAST)
    {
        `$INSTANCE_NAME`_trim2Adjust();          /* course trim */
        `$INSTANCE_NAME`_trim1Adjust();          /* fine trim */
    }
    else /* default to trim for slow mode */
    {
        `$INSTANCE_NAME`_trim1Adjust();          /* course trim */
        `$INSTANCE_NAME`_trim2Adjust();          /* fine trim */
    }
   
    /* Restore Config Register */
    `$INSTANCE_NAME`_CR = tmpCR;
    
    /* Restore routing registers for inP */
    `$INSTANCE_NAME`_SW0 = tmpSW0;
    `$INSTANCE_NAME`_SW2 = tmpSW2;
    `$INSTANCE_NAME`_SW3 = tmpSW3;
    
    return(`$INSTANCE_NAME`_TR);
}


/*******************************************************************************
* Function Name: `$INSTANCE_NAME`_LoadTrim
********************************************************************************
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
*******************************************************************************/
void `$INSTANCE_NAME`_LoadTrim(uint8 trimVal) 
{
      `$INSTANCE_NAME`_TR = trimVal;
}



/*******************************************************************************
* Function Name: `$INSTANCE_NAME`_trim1Adjust
********************************************************************************
* Summary:
*  This function adjusts the value in the low nibble of the Analog Comparator 
*  trim register (trim1)
*
* Parameters:  
*  None
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
*  the comparator output to respond.  Worse case trim adjustment can result in
*  upto a 16ms delay.
*
*******************************************************************************/
void `$INSTANCE_NAME`_trim1Adjust(void) 
{
    uint8 trimCnt;
    uint8 cmpState;   

    /* get current state of comparator output */
    cmpState = `$INSTANCE_NAME`_WRK & `$INSTANCE_NAME`_CMP_OUT_MASK;
    
    /* if comparator output is high, negative offset adjust is required */
    if ( cmpState != 0 )
    {
        `$INSTANCE_NAME`_TR |= `$INSTANCE_NAME`_CMP_TRIM1_DIR;
    }

    /* Increment trim value until compare output changes state */
    for ( trimCnt = 0; trimCnt < 7; trimCnt++ )
    {
        `$INSTANCE_NAME`_TR += 1;
        CyDelay(1);

        /* Check for change in comparator output */
        if ((`$INSTANCE_NAME`_WRK & `$INSTANCE_NAME`_CMP_OUT_MASK) != cmpState)
        {
            break;      /* output changed state, trim phase is complete */
        }
    }
}

/*******************************************************************************
* Function Name: `$INSTANCE_NAME`_trim2Adjust
********************************************************************************
* Summary:
*  This function adjusts the value in the high nibble of the Analog Comparator 
*  trim register (trim2)
*
* Parameters:  
*  None
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
*  the comparator output to respond.  Worse case trim adjustment can result in
*  upto a 16ms delay.
*
*******************************************************************************/
void `$INSTANCE_NAME`_trim2Adjust(void) 
{
    uint8 trimCnt;
    uint8 cmpState;

    /* get current state of comparator output */
    cmpState = `$INSTANCE_NAME`_WRK & `$INSTANCE_NAME`_CMP_OUT_MASK;
    
    /* if comparator output is low, positive offset adjust is required */
    if ( cmpState == 0 )
    {
        `$INSTANCE_NAME`_TR |= `$INSTANCE_NAME`_CMP_TRIM2_DIR; 
    }

    /* Increment trim value until compare output changes state */
    for ( trimCnt = 0; trimCnt < 7; trimCnt++ )
    {
        `$INSTANCE_NAME`_TR += 0x10;
        CyDelay(1);
        
        /* Check for change in comparator output */
        if ( (`$INSTANCE_NAME`_WRK & `$INSTANCE_NAME`_CMP_OUT_MASK) != cmpState )
        {
            break;      /* output changed state, trim phase is complete */
        }
    }
}

/* [] END OF FILE */

