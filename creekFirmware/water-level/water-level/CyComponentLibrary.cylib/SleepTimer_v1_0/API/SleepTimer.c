/*******************************************************************************
* File Name: `$INSTANCE_NAME`.c  
* Version `$CY_MAJOR_VERSION`.`$CY_MINOR_VERSION`
*
*  Description:
*   This file provides the source code to API for the Sleep Timer.
*
*  Note:
*
********************************************************************************
* Copyright 2008-2009, Cypress Semiconductor Corporation.  All rights reserved.
* You may use this file only in accordance with the license, terms, conditions, 
* disclaimers, and limitations in the end user license agreement accompanying 
* the software package with which this file was provided.
********************************************************************************/



#include "`$INSTANCE_NAME`.h"

/* Take parameters from:
 *  xxxxxx00 - No Interval, nor EnableInt was changed by API
 *  xxxxxx01 - No Interval, but EnableInt was changed by API
 *  xxxxxx10 - Interval, but not EnableInt was changed by API
 *  xxxxxx11 - Interval and EnableInt was changed by API
*/
uint8 `$INSTANCE_NAME`_flags = 0;


/*******************************************************************************
* Function Name: `$INSTANCE_NAME`_Start       
********************************************************************************
* 
* Summary: 
*  Starts the Sleep Timer operation: enables 1 kHz clock, sets default 
*  parameters if they were not set previously by corresponding API functions.
*   
* Parameters:
*  void
*
* Return:
*  void
*
* Side Effects: 
*  Enables 1 kHz ILO clocks and leaves it enabled after Sleep Time component 
*  is stopped.
* 
*******************************************************************************/
void `$INSTANCE_NAME`_Start(void)
{
	/* Check if 1kHz ILO is disabled */
	if (0 == (`$INSTANCE_NAME`_ILO_CFG & `$INSTANCE_NAME`_ILO_1KHZ_EN))
	{
		`$INSTANCE_NAME`_ILO_CFG |=`$INSTANCE_NAME`_ILO_1KHZ_EN;
	}
	/* If 1kHz ILO is enabled - do nothing */
	
	
	/* Sets default (passed by parameter from customizer) interval if it was 
	 *   not manually set by API function 									  */
	if (0 == (`$INSTANCE_NAME`_flags & `$INSTANCE_NAME`_INTERVAL_CHNG_BIT))
	{
		`$INSTANCE_NAME`_SetInterval(`$INSTANCE_NAME`_INTERVAL);
	}
	/* Interval is already set */


	/* Sets default (customizer's) interrupt enable if it was not set by API  */
	if (0 == (`$INSTANCE_NAME`_flags & `$INSTANCE_NAME`_INTERRUPT_CHNG_BIT))
	{
		/* Check if user sets to issue an interrupt in customizer */
		if ( 1 == `$INSTANCE_NAME`_ENABLE_INTERRUPT )
		{
			/* Enable interrupt */
			`$INSTANCE_NAME`_EnableInt();
		} else
		{
			/* Disable interrupt */
			`$INSTANCE_NAME`_DisableInt();
		}			
	}
	/* Interrupt enable/disable is already applied */
	
	/* Enable wake up on CTW counter */
	`$INSTANCE_NAME`_TW_CFG |= `$INSTANCE_NAME`_CTW_EN;
	
}


/*******************************************************************************
* Function Name: `$INSTANCE_NAME`_Stop       
********************************************************************************
* 
* Summary:
*  Stops the Sleep Timer component?s operation: disables wake up on CTW counter
*  terminal count reached and disables interrupt issuing on foregoing condition.
*   
* Parameters:
*  void
*
* Return:
*  void
*
*******************************************************************************/
void `$INSTANCE_NAME`_Stop(void)
{
	/* Disable wake up on CTW counter */
	`$INSTANCE_NAME`_TW_CFG &= ~`$INSTANCE_NAME`_CTW_EN;
	/* Just for sure of next time start */
	`$INSTANCE_NAME`_TW_CFG &= ~`$INSTANCE_NAME`_CTW_IE;
}


/*******************************************************************************
* Function Name: `$INSTANCE_NAME`_EnableInt     
********************************************************************************
* 
* Summary: 
*  Enables Sleep Timer component issuing interrupt on wake up.   
*
* Parameters:
*  void
*
* Return:
*  void
*
*******************************************************************************/
void `$INSTANCE_NAME`_EnableInt(void)
{

	 /* Update flag that interrupt was set by API */
	`$INSTANCE_NAME`_flags |= `$INSTANCE_NAME`_INTERRUPT_CHNG_BIT;
	 /* Enable interrupt on wake up */
	`$INSTANCE_NAME`_TW_CFG |= `$INSTANCE_NAME`_CTW_IE;
}


/*******************************************************************************
* Function Name: `$INSTANCE_NAME`_DisableInt     
********************************************************************************
* 
* Summary: 
*  Disables Sleep Timer component issuing interrupt on wake up.
*   
* Parameters:
*  void
*
* Return:
*  void
*
*******************************************************************************/
void `$INSTANCE_NAME`_DisableInt(void)
{
	/* Update flag that interrupt was set by API */
	`$INSTANCE_NAME`_flags |= `$INSTANCE_NAME`_INTERRUPT_CHNG_BIT;
	/* Disable interrupt on wake up */
	`$INSTANCE_NAME`_TW_CFG &= ~`$INSTANCE_NAME`_CTW_IE; 
}


/*******************************************************************************
* Function Name: `$INSTANCE_NAME`_SetInterval     
********************************************************************************
* 
* Summary: 
*  Sets interval for Sleep Timer to wake up. 
*   
* Parameters:
*  interval:  This parameter is used to define interval value in milliseconds. 
*  Acceptable is only values from a fixed range:
*   Name	                        Value	    Period 
*   `$INSTANCE_NAME`__CTW_2_MS	    4'b0001	    2 ms
*   `$INSTANCE_NAME`__CTW_4_MS	    4'b0010	    4 ms
*   `$INSTANCE_NAME`__CTW_8_MS	    4'b0011	    8 ms
*   `$INSTANCE_NAME`__CTW_16_MS	    4'b0100	    16 ms
*   `$INSTANCE_NAME`__CTW_32_MS	    4'b0101	    32 ms
*   `$INSTANCE_NAME`__CTW_64_MS	    4'b0110	    64 ms
*   `$INSTANCE_NAME`__CTW_128_MS	4'b0111	    128 ms
*   `$INSTANCE_NAME`__CTW_256_MS	4'b1000	    256 ms
*   `$INSTANCE_NAME`__CTW_512_MS	4'b1001	    512 ms
*   `$INSTANCE_NAME`__CTW_1024_MS	4'b1010	    1024 ms
*   `$INSTANCE_NAME`__CTW_2048_MS	4'b1011	    2048 ms
*   `$INSTANCE_NAME`__CTW_4096_MS	4'b1100	    4096 ms
* 
* Return:
*  void
*
* Side Effects:
*  The first interval can range from 1 to (period + 1)ms. Additional intervals
*  occur at the nominal period. Interval value can be only changed when 
*  Sleep Timer is stopped.
* 
*******************************************************************************/
void `$INSTANCE_NAME`_SetInterval(uint8 interval)
{
	/* Check if CTW is disabled */
	if (0 == (`$INSTANCE_NAME`_TW_CFG & `$INSTANCE_NAME`_CTW_EN))
	{
		/* Clear interval setting */
		`$INSTANCE_NAME`_CTW_INTERVAL &= ~ `$INSTANCE_NAME`_INTERVAL_MASK;
		/* Set wake up interval */
		`$INSTANCE_NAME`_CTW_INTERVAL |= \
            (interval & `$INSTANCE_NAME`_INTERVAL_MASK);
		/* Update flag that interval was set by API */
		`$INSTANCE_NAME`_flags |= `$INSTANCE_NAME`_INTERVAL_CHNG_BIT;	
	}
	/* Do nothing if CTW is enabled. */

}


/*******************************************************************************
* Function Name: `$INSTANCE_NAME`_Reset  
********************************************************************************
* 
* Summary: 
*  Resets the CTW counter for proper Sleep Timer first time wake up.
*   
* Parameters:
*  void
*
* Return:
*  void
*
* Side Effects: 
*  Note that Watch Dog Timer shares the CTW, so if it is reset, the WDT will 
*  take longer to trigger a hardware reset. Note that it?s impossible to reset 
*  CTW if Watch Dog Timer is enabled.
*
*******************************************************************************/
void `$INSTANCE_NAME`_Reset(void)
{

	/* Check if WDT is disabled */
	if ( 0 == (`$INSTANCE_NAME`_WDT_CFG & `$INSTANCE_NAME`_WDR_EN) )
	{
		/*  CTW is reset to 0 and held there */
		`$INSTANCE_NAME`_WDT_CFG |= `$INSTANCE_NAME`_CTW_RESET;
		/*  Exit the CTW's reset state */
		`$INSTANCE_NAME`_WDT_CFG &= ~`$INSTANCE_NAME`_CTW_RESET;
	}
	/*  Do nothing if WDT is enabled. */
	
}


/* [] END OF FILE */
