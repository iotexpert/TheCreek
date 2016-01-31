/*******************************************************************************
* File Name: `$INSTANCE_NAME`.c  
* Version `$CY_MAJOR_VERSION`.`$CY_MINOR_VERSION`
*
*  Description:
*    This file provides the source code to the API for the Analog Buffer 
*    User Module.
*
*   Note:
*     
*
*******************************************************************************
* Copyright 2008-2009, Cypress Semiconductor Corporation.  All rights reserved.
* You may use this file only in accordance with the license, terms, conditions, 
* disclaimers, and limitations in the end user license agreement accompanying 
* the software package with which this file was provided.
********************************************************************************/




#include "cytypes.h"
#include "`$INSTANCE_NAME`.h"

uint8  `$INSTANCE_NAME`_initVar = 0;

/*******************************************************************************
* Function Name: `$INSTANCE_NAME`_Start
********************************************************************************
* Summary:
*  The start function initializes the Analog Comparitor with the default values.
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
   if(`$INSTANCE_NAME`_initVar == 0)
   {
      `$INSTANCE_NAME`_initVar = 1;

      /* Set default speed/power */
      `$INSTANCE_NAME`_SetSpeed(`$INSTANCE_NAME`_DEFAULT_SPEED);

      /* Set default Hysteresis */
      if(`$INSTANCE_NAME`_DEFAULT_HYSTERESIS == 0 )
      {
         `$INSTANCE_NAME`_CR &= ~`$INSTANCE_NAME`_HYST_ON;
      }
      else
      {
         `$INSTANCE_NAME`_CR |= `$INSTANCE_NAME`_HYST_ON;
      }

      /* Set default sync */
      `$INSTANCE_NAME`_CLK &= ~`$INSTANCE_NAME`_CLKSYNC_MASK;
      if(`$INSTANCE_NAME`_DEFAULT_BYPASS_SYNC == 0 )
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
*  This function sets the speed of the Analog Comparitor.  The faster the speed
*  the more power that is used.
*
* Parameters:  
*  speed:   (uint8) Sets operation mode of Comparitor
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
   `$INSTANCE_NAME`_CR |= (speed & `$INSTANCE_NAME`_PWR_MODE_MASK);   /* Power up device */
}

/*******************************************************************************
* Function Name: `$INSTANCE_NAME`_GetCompare
********************************************************************************
* Summary:
*  This function returns the result of a compare.
*
* Parameters:  
*   None
*
* Return:  
*  (uint8)  Zero if Neg_Input greater than Pos_input, set to one if Pos_Input
*           greater than Neg_input. 
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
*  This function calibrates the offset Analog Comparitor.
*
* Parameters:  
*  speed:   (uint8) Sets operation mode of Comparitor
*
* Return:  
*  (uint8)  Copy of value written in trim register when calibration complete.
*
* Theory: 
*   This function is used to optimize the calibration for a specific user
*   voltage range.
*
*   1) User applies a voltage to the negative input.  This should be a value
*      that the comparator will be operating in, or an average of his operating
*      range.
*   2) Set the calibration bit to short the negative and positive inputs to
*      the users calibration voltage.
*   3) Clear the TR registers  ( TR = 0x00 )
*   4) Check if compare output is high, if so, set trim1[3] to a 1.
*   5) Increment trim1[2:0] until the compare output changes.
*   6) If the output does not change by the time trim1[2:0] = 7 then proceed
*      to step 7, else you are done.
*   7) Check if compare output is low, if so, set trim2[3] to a 1.
*   8) Increment trim2[2:0] until the compare output changes state.
*   
*
* Side Effects:
*
*******************************************************************************/
uint8 `$INSTANCE_NAME`_ZeroCal(void) 
{
   uint8 trimCnt;
   uint8 calComplete;
   uint8 cmpState;   /* State of compare output */
   uint8 tmpSW0, tmpSW2, tmpSW3, tmpSW4, tmpSW6;

   calComplete = 0;
   /* Save a copy of routing registers */
   tmpSW0 = `$INSTANCE_NAME`_SW0;
   tmpSW2 = `$INSTANCE_NAME`_SW2;
   tmpSW3 = `$INSTANCE_NAME`_SW3;
   tmpSW4 = `$INSTANCE_NAME`_SW4;
   tmpSW6 = `$INSTANCE_NAME`_SW6;

   /* Setup register for test */
   `$INSTANCE_NAME`_SW0 = 0x00;
   `$INSTANCE_NAME`_SW2 = 0x00;
//   `$INSTANCE_NAME`_SW3 = ;     //:TODO find actual value
   `$INSTANCE_NAME`_SW4 = 0x00;
   `$INSTANCE_NAME`_SW6 = 0x00;

#if (`$INSTANCE_NAME`_RECALMODE == 1)
    //:TODO switch to 0.256 volt reference
    //:TODO save specific registers
#endif

   /* Short inN to inP, by setting calibration bit  */
   `$INSTANCE_NAME`_CR |= `$INSTANCE_NAME`_CAL_ON;

   /* Write default low values to register first */
   `$INSTANCE_NAME`_TR = 0x00;

   cmpState = `$INSTANCE_NAME`_WRK & `$INSTANCE_NAME`_CMP_OUT_MASK;
   if(cmpState != 0)
   {
      `$INSTANCE_NAME`_TR = `$INSTANCE_NAME`_CMP_TRIM1_DIR;
   }

   /* Increment trim value until compare output changes state */
   for(trimCnt = 0; trimCnt <= 7; trimCnt++ )
   {
      `$INSTANCE_NAME`_TR += 1;
      if((`$INSTANCE_NAME`_WRK & `$INSTANCE_NAME`_CMP_OUT_MASK) != cmpState)
      {
         /* Compare changed state, trim is complete */
         calComplete = 1;
         break;
      }
   }

#if (`$INSTANCE_NAME`_RECALMODE == 1)
    //:TODO  switch to 1.024 volt reference
    //:TODO save specific registers
    calComplete = 0;  /* Always do second step */
#endif

   /* First trim (trim1) did not move the trim far enough, use trim2 */
   if(calComplete == 0)   /* If the first trim did not work do the second stage */
   {
      cmpState = `$INSTANCE_NAME`_WRK & `$INSTANCE_NAME`_CMP_OUT_MASK;
      if(cmpState == 0)
      {
         `$INSTANCE_NAME`_TR |= `$INSTANCE_NAME`_CMP_TRIM2_DIR; /* Set direction */
      }

      /* Increment trim value until compare output changes state */
      for(trimCnt = 0; trimCnt <= 7; trimCnt++ )
      {
         `$INSTANCE_NAME`_TR += 0x10;
         if((`$INSTANCE_NAME`_WRK & `$INSTANCE_NAME`_CMP_OUT_MASK) != cmpState)
         {
            /* Compare changed state, trim is complete */
            break;
         }
      }
   }

#if (`$INSTANCE_NAME`_RECALMODE == 1)
    //:TODO Restore specific reference registers
#endif


   /* Disconnect short between inN to inP  */
   `$INSTANCE_NAME`_CR &= ~`$INSTANCE_NAME`_CAL_ON;

   /* Restore routing registers */
   `$INSTANCE_NAME`_SW0 = tmpSW0;
   `$INSTANCE_NAME`_SW2 = tmpSW2;
   `$INSTANCE_NAME`_SW3 = tmpSW3;
   `$INSTANCE_NAME`_SW4 = tmpSW4;
   `$INSTANCE_NAME`_SW6 = tmpSW6;

   return(`$INSTANCE_NAME`_TR);
}


/* [] END OF FILE */



