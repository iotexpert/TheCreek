 /*******************************************************************************
* File Name: `$INSTANCE_NAME`_RTC_INT.c  
* Version `$CY_MAJOR_VERSION`.`$CY_MINOR_VERSION`
*
*  Description:
*     This file contains the Interrupt Service Routine (ISR) for the RTC
*     Component. This interrupt routine has entry pointes to overflow on time 
*     or date.
*
*   Note:
*     None
*
*******************************************************************************
* Copyright 2008-2009, Cypress Semiconductor Corporation.  All rights reserved.
* You may use this file only in accordance with the license, terms, conditions, 
* disclaimers, and limitations in the end user license agreement accompanying 
* the software package with which this file was provided.
********************************************************************************/



#include "`@INSTANCE_NAME`.h"

extern uint8 `$INSTANCE_NAME`_IsLeapYear(uint16 year);

#if (`$INSTANCE_NAME`_DST_FUNC_ENABLE)
extern void `$INSTANCE_NAME`_DSTDateConversion(void);
#endif /* `$INSTANCE_NAME`_DST_FUNC_ENABLE */

/* `#START RTC_ISR_DEFINITION` */

/* `#END` */

/*-----------------------------------------------------------------------------
 * FUNCTION NAME:   void `@INSTANCE_NAME`_EverySecond_ISR( void )
 *-----------------------------------------------------------------------------
 * Summary:
 *  This ISR is triggered  every second. 
 * 
 * Parameters:  
 *  void:  
 *
 * Return: 
 *  (void)
 *
 * Theory:
 *  See summary
 *
 * Side Effects:
 *  None
 *
 *---------------------------------------------------------------------------*/
void `@INSTANCE_NAME`_EverySecond_ISR( void )
{
	/* `#START EVERY_SECOND_ISR` */
	
	/* `#END` */
}

/*-----------------------------------------------------------------------------
 * FUNCTION NAME:   void `@INSTANCE_NAME`_EveryMinute_ISR( void )
 *-----------------------------------------------------------------------------
 * Summary:
 *  This ISR is triggered  every minute. 
 * 
 * Parameters:  
 *  void:  
 *
 * Return: 
 *  (void)
 *
 * Theory:
 *  See summary
 *
 * Side Effects:
 *  None
 *
 *---------------------------------------------------------------------------*/
void `@INSTANCE_NAME`_EveryMinute_ISR( void )
{
	/* `#START EVERY_MINUTE_ISR` */

	/* `#END` */
}

/*-----------------------------------------------------------------------------
 * FUNCTION NAME:   void `@INSTANCE_NAME`_EveryHour_ISR( void )
 *-----------------------------------------------------------------------------
 * Summary:
 *  This ISR is triggered  every hour. 
 * 
 * Parameters:  
 *  void:  
 *
 * Return: 
 *  (void)
 *
 * Theory:
 *  See summary
 *
 * Side Effects:
 *  None
 *
 *---------------------------------------------------------------------------*/
void `@INSTANCE_NAME`_EveryHour_ISR( void )
{
	/* `#START EVERY_HOUR_ISR` */

	/* `#END` */
}

/*-----------------------------------------------------------------------------
 * FUNCTION NAME:   void `@INSTANCE_NAME`_EveryDayOfMonth_ISR( void )
 *-----------------------------------------------------------------------------
 * Summary:
 *  This ISR is triggered  every day. 
 * 
 * Parameters:  
 *  void:  
 *
 * Return: 
 *  (void)
 *
 * Theory:
 *  See summary
 *
 * Side Effects:
 *  None
 *
 *---------------------------------------------------------------------------*/
void `@INSTANCE_NAME`_EveryDayOfMonth_ISR( void )
{
	/* `#START EVERY_DAY_ISR` */

	/* `#END` */
}

/*-----------------------------------------------------------------------------
 * FUNCTION NAME:   void `@INSTANCE_NAME`_EveryWeek_ISR( void )
 *-----------------------------------------------------------------------------
 * Summary:
 *  This ISR is triggered  every week. 
 * 
 * Parameters:  
 *  void:  
 *
 * Return: 
 *  (void)
 *
 * Theory:
 *  See summary
 *
 * Side Effects:
 *  None
 *
 *---------------------------------------------------------------------------*/
void `@INSTANCE_NAME`_EveryWeek_ISR( void )
{
	/* `#START EVERY_WEEK_ISR` */

	/* `#END` */
}

/*-----------------------------------------------------------------------------
 * FUNCTION NAME:   void `@INSTANCE_NAME`_EveryMonth_ISR( void )
 *-----------------------------------------------------------------------------
 * Summary:
 *  This ISR is triggered  every month. 
 * 
 * Parameters:  
 *  void:  
 *
 * Return: 
 *  (void)
 *
 * Theory:
 *  See summary
 *
 * Side Effects:
 *  None
 *
 *---------------------------------------------------------------------------*/
void `@INSTANCE_NAME`_EveryMonth_ISR( void )
{
	/* `#START EVERY_MONTH_ISR` */

	/* `#END` */
}

/*-----------------------------------------------------------------------------
 * FUNCTION NAME:   void `@INSTANCE_NAME`_EveryYear_ISR( void )
 *-----------------------------------------------------------------------------
 * Summary:
 *  This ISR is triggered  every year. 
 * 
 * Parameters:  
 *  void:  
 *
 * Return: 
 *  (void)
 *
 * Theory:
 *  See summary
 *
 * Side Effects:
 *  None
 *
 *---------------------------------------------------------------------------*/
void `@INSTANCE_NAME`_EveryYear_ISR( void )
{
	/* `#START EVERY_YEAR_ISR` */

	/* `#END` */
}

`$writeISR`
