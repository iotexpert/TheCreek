/*******************************************************************************
* File Name: `$INSTANCE_NAME`.c  
* Version `$CY_MAJOR_VERSION`.`$CY_MINOR_VERSION`
*
*  Description:
*    This file provides the source code to the API for the 8-bit Voltage DAC 
*    (VDAC8) User Module.
*
*   Note:
*     Any unusual or non-standard behavior should be noted here. Other-
*     wise, this section should remain blank.
*
*******************************************************************************
* Copyright 2008-2009, Cypress Semiconductor Corporation.  All rights reserved.
* You may use this file only in accordance with the license, terms, conditions, 
* disclaimers, and limitations in the end user license agreement accompanying 
* the software package with which this file was provided.
********************************************************************************/




#include "cytypes.h"
#include "`$INSTANCE_NAME`.h"

uint8 `$INSTANCE_NAME`_initVar = 0;


/*******************************************************************************
* Function Name: `$INSTANCE_NAME`_Start
********************************************************************************
*
* Summary:
*  The start function initializes the voltage DAC with the default values, 
*  and sets the power to the given level.  A power level of 0, is the same as executing
*  the stop function.
*
* Parameters:  
*  Power:   Sets power level between off (0) and (3) high power
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
  
   /* Hardware initiazation only needs to occure the first time */
   if ( `$INSTANCE_NAME`_initVar == 0)  
   {    
     `$INSTANCE_NAME`_initVar = 1;
      `$INSTANCE_NAME`_CR0 = (`$INSTANCE_NAME`_MODE_V )  ;    

      /* Set default data source */
      if(`$INSTANCE_NAME`_DEFAULT_DATA_SRC != 0 )    
      {
          `$INSTANCE_NAME`_CR1 = (`$INSTANCE_NAME`_DEFAULT_CNTL | `$INSTANCE_NAME`_DACBUS_ENABLE) ;   
      }
      else
      {
          `$INSTANCE_NAME`_CR1 = (`$INSTANCE_NAME`_DEFAULT_CNTL | `$INSTANCE_NAME`_DACBUS_DISABLE) ;  
      } 

       /* Set default strobe mode */
      if(`$INSTANCE_NAME`_DEFAULT_STRB != 0)
      {
          `$INSTANCE_NAME`_Strobe |= `$INSTANCE_NAME`_STRB_EN ;
      }

		/* Set default range */
      `$INSTANCE_NAME`_SetRange(`$INSTANCE_NAME`_DEFAULT_RANGE); 
	  
      /* Set default speed */
      `$INSTANCE_NAME`_SetSpeed(`$INSTANCE_NAME`_DEFAULT_SPEED); 

      /* Enable power to DAC */
      `$INSTANCE_NAME`_PWRMGR |= `$INSTANCE_NAME`_ACT_PWR_EN;

      /* Set default value */
      `$INSTANCE_NAME`_SetValue(`$INSTANCE_NAME`_DEFAULT_DATA);

   }
   else
   {
       /* Enable power to DAC */
       `$INSTANCE_NAME`_PWRMGR |= `$INSTANCE_NAME`_ACT_PWR_EN;
   }

}


/*******************************************************************************
* Function Name: `$INSTANCE_NAME`_Stop
********************************************************************************
*
* Summary:
*  Powers down DAC to lowest power state.
*
* Parameters:  
*  (void)  
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
   /* Disble power to DAC */
   `$INSTANCE_NAME`_PWRMGR &= ~`$INSTANCE_NAME`_ACT_PWR_EN;
}


/*******************************************************************************
* Function Name: `$INSTANCE_NAME`_SetSpeed
********************************************************************************
*
* Summary:
*  Set DAC speed
*
* Parameters:  
*  power:   Sets speed value 
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
   /* Clear power mask then write in new value */
   `$INSTANCE_NAME`_CR0 &= ~`$INSTANCE_NAME`_HS_MASK ;    
   `$INSTANCE_NAME`_CR0 |=  ( speed & `$INSTANCE_NAME`_HS_MASK) ;    
}


/*******************************************************************************
* Function Name: `$INSTANCE_NAME`_SetRange
********************************************************************************
*
* Summary:
*  Set one of three current ranges.
*
* Parameters:  
*  Range:  Sets one of Three valid ranges.
*
* Return: 
*  (void) 
*
* Theory: 
*
* Side Effects:
*
*******************************************************************************/
void `$INSTANCE_NAME`_SetRange(uint8 range)
{
   `$INSTANCE_NAME`_CR0 &= ~`$INSTANCE_NAME`_RANGE_MASK ;      /* Clear existing mode */
   `$INSTANCE_NAME`_CR0 |= ( range & `$INSTANCE_NAME`_RANGE_MASK ) ; /*  Set Range  */
   `$INSTANCE_NAME`_DacTrim();
}


/*******************************************************************************
* Function Name: `$INSTANCE_NAME`_SetValue
********************************************************************************
*
* Summary:
*  Set 8-bit DAC value
*
* Parameters:  
*  value:  Sets DAC value between 0 and 255.
*
* Return: 
*  (void) 
*
* Theory: 
*
* Side Effects:
*
*******************************************************************************/
void `$INSTANCE_NAME`_SetValue(uint8 value)
{
   `$INSTANCE_NAME`_Data = value;                             /*  Set Value  */
   
   #if defined(`$INSTANCE_NAME`_FIRST_SILICON)
   `$INSTANCE_NAME`_Data = value;                             /*  TODO: Remove when Silicon fix comes */
   #endif
}


/*******************************************************************************
* Function Name: `$INSTANCE_NAME`_DacTrim
********************************************************************************
*
* Summary:
*  Set the trim value for the given range.
*
* Parameters:  
*   range:  1V or 4V range.  See constants.  
*
* Return: 
*  (void) 
*
* Theory: 
*
* Side Effects:
*
*******************************************************************************/
void `$INSTANCE_NAME`_DacTrim(void)
{
    uint8 mode;
	
	mode = ((`$INSTANCE_NAME`_CR0 & `$INSTANCE_NAME`_RANGE_MASK) >> 2) + `$INSTANCE_NAME`_TRIM_M7_1V_RNG_OFFSET;
	
	`$INSTANCE_NAME`_TR = CY_GET_XTND_REG8((uint8 *)(`$INSTANCE_NAME`_DAC_TRIM_BASE + mode));
}


/* [] END OF FILE */

