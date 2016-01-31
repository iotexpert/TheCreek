/*******************************************************************************
* File Name: `$INSTANCE_NAME`.c
* Version `$CY_MAJOR_VERSION`.`$CY_MINOR_VERSION`
*
* Description:
*  This file provides the source code to the API for the RTC Component.
*
* Note:
*
********************************************************************************
* Copyright 2008-2010, Cypress Semiconductor Corporation.  All rights reserved.
* You may use this file only in accordance with the license, terms, conditions,
* disclaimers, and limitations in the end user license agreement accompanying
* the software package with which this file was provided.
*******************************************************************************/

#include "`$INSTANCE_NAME`.h"
#include "CyLib.h"

/* Function Prototypes */
void `$INSTANCE_NAME`_SetInitValues(void);
uint8 `$INSTANCE_NAME`_DayOfWeek(uint8, uint8, uint16) `=ReentrantKeil($INSTANCE_NAME . "_DayOfWeek")`;

/* Variables were not initialized */
uint8 `$INSTANCE_NAME`_initVar = 0u;

/* Time and date variables 
* Initial value are: Second = 0-59, minute = 0-59, hour = 0-23, DayOfWeek = 1-7,
* DayOfMonth = 1-31, DayOfYear = 1?366, Month = 1-12, Year = 1900?2200.
*/
`$INSTANCE_NAME`_TIME_DATE      `$INSTANCE_NAME`_currentTimeDate = {0u, 0u, 0u, 1u, 1u, 1u, 1u, 1900u};

/* Alarm time and date variables
* Initial value are: Second = 0-59, minute = 0-59, hour = 0-23, DayOfWeek = 1-7,
* DayOfMonth = 1-31, DayOfYear = 1?366, Month = 1-12, Year = 1900?2200.
*/
`$INSTANCE_NAME`_TIME_DATE `$INSTANCE_NAME`_alarmCfgTimeDate = {0u, 0u, 0u, 1u, 1u, 1u, 1u, 1900u};

#if(1u == `$INSTANCE_NAME`_DST_FUNC_ENABLE) /* DST enabled */

    /* Define DST format: '0' - fixed, '1' - relative */
    uint8   `$INSTANCE_NAME`_dstModeType = 0u;
    
    /* Hour 0-23, DayOfWeek 1-7, Week 1-5, DayOfMonth 1-31, Month 1-12  */
    `$INSTANCE_NAME`_DSTIME `$INSTANCE_NAME`_dstTimeDateStart = {0u, 1u, 1u, 1u, 1u};
    `$INSTANCE_NAME`_DSTIME `$INSTANCE_NAME`_dstTimeDateStop =  {0u, 1u, 1u, 1u, 1u};

    /* Number of Hours to add/dec to time */
    uint8   `$INSTANCE_NAME`_dstOffsetMin = 0u;
    uint8   `$INSTANCE_NAME`_dstStartStatus = 0u;
    uint8   `$INSTANCE_NAME`_dstStopStatus = 0u;

#endif /* 1u == `$INSTANCE_NAME`_DST_FUNC_ENABLE*/

/* Mask Registers */
uint8   `$INSTANCE_NAME`_alarmCfgMask = 0u;
uint8   `$INSTANCE_NAME`_alarmCurStatus = 0u;
uint8   `$INSTANCE_NAME`_intervalCfgMask = 0u;

/* Status & Control Variables */
uint8   `$INSTANCE_NAME`_statusDateTime = 0u;

/* Month Day Array - number of days in the months */
const uint8 CYCODE `$INSTANCE_NAME`_daysInMonths[`$INSTANCE_NAME`_MONTHS_IN_YEAR] = {
    `$INSTANCE_NAME`_DAYS_IN_JANUARY,
    `$INSTANCE_NAME`_DAYS_IN_FEBRUARY,
    `$INSTANCE_NAME`_DAYS_IN_MARCH,
    `$INSTANCE_NAME`_DAYS_IN_APRIL,
    `$INSTANCE_NAME`_DAYS_IN_MAY,
    `$INSTANCE_NAME`_DAYS_IN_JUNE,
    `$INSTANCE_NAME`_DAYS_IN_JULY,
    `$INSTANCE_NAME`_DAYS_IN_AUGUST,
    `$INSTANCE_NAME`_DAYS_IN_SEPTEMBER,
    `$INSTANCE_NAME`_DAYS_IN_OCTOBER,
    `$INSTANCE_NAME`_DAYS_IN_NOVEMBER,
    `$INSTANCE_NAME`_DAYS_IN_DECEMBER};

/* Calculated sequence (31 * month )/ 12 ) mod 7 from the Zeller's congruence */
 const uint8 CYCODE `$INSTANCE_NAME`_monthTemplate[`$INSTANCE_NAME`_MONTHS_IN_YEAR] = \
                                                        {0u, 3u, 2u, 5u, 0u, 3u, 5u, 1u, 4u, 6u, 2u, 4u};


/*******************************************************************************
* Function Name:   `$INSTANCE_NAME`_Start
********************************************************************************
*
* Summary:
*  Enables RTC component: configurate counter, setup interrupts, done all
*  requered calculation and starts counter.
*
* Parameters:
*  None.
*
* Return:
*  None.
*
* Global variables:
*  `$INSTANCE_NAME`_initVar: global variable is used to indicate initial
*  configuration of this component.  The variable is initialized to zero and set
*  to 1 the first time `$INSTANCE_NAME`_Start() is called. This allows for
*  component initialization without re-initialization in all subsequent calls
*  to the `$INSTANCE_NAME`_Start() routine.
*
*  `$INSTANCE_NAME`_currentTimeDate, `$INSTANCE_NAME`_dstTimeDateStart, 
*  `$INSTANCE_NAME`_dstTimeDateStop, `$INSTANCE_NAME`_dstTimeDateStart,
*  `$INSTANCE_NAME`_alarmCfgTimeDate, `$INSTANCE_NAME`_statusDateTime,
*  `$INSTANCE_NAME`_dstStartStatus, `$INSTANCE_NAME`_dstStopStatus, 
*  `$INSTANCE_NAME`_alarmCurStatus: global variables are modified by the 
*  functions called from `$INSTANCE_NAME`_Init().
*
* Reentrant:
*  No.
*
* Side Effects:
*  Enables for the one pulse per second (for the RTC component) and 
*  Central Time Wheel (for the Sleep Timer component) signals to wake up device
*  from the low power (Sleep and Alternate Active) modes and leaves them
*  enabled.
*
*  The Power Manager API has the higher priority on resource usage: it is NOT
*  guaranteed that the Sleep Timer's configuration will be the same on exit
*  from the Power Manager APIs as on the entry. To prevent this use the Sleep
*  Timer's Sleep() - to save configuration and stop the component and Wakeup()
*  function to restore configuration and enable the component.
*
*  The Sleep Timer and Real Time Clock (RTC) components could be configured as
*  a wake up source from the low power modes only both at once.
*
*******************************************************************************/
void `$INSTANCE_NAME`_Start(void)
{
    /* Execute once in normal flow */
    if(0u == `$INSTANCE_NAME`_initVar)
    {
        `$INSTANCE_NAME`_Init();
        `$INSTANCE_NAME`_initVar = 1u;
    }

    /* Enable component's operation */
    `$INSTANCE_NAME`_Enable();
}


/*******************************************************************************
* Function Name: `$INSTANCE_NAME`_Stop
********************************************************************************
*
* Summary:
*  Stops the RTC component.
*
* Parameters:
*  None.
*
* Return:
*  None.
*
* Side Effects:
*  Leaves the one pulse per second (for the RTC component) and the Central Time
*  Wheel (for the Sleep Timer component) signals to wake up device from the low
*  power (Sleep and Alternate Active) modes enabled after Sleep Time component
*  is stopped. 
*
*******************************************************************************/
void `$INSTANCE_NAME`_Stop(void) `=ReentrantKeil($INSTANCE_NAME . "_Stop")`
{
    uint8 interruptState;
	
	/* Disable the interrupt. */
    CyIntDisable(`$INSTANCE_NAME`_ISR_NUMBER);

    /* Enter critical section */
    interruptState = CyEnterCriticalSection();

    /* Stop one pulse per second counter and interrupt */
    `$INSTANCE_NAME`_OPPS_CFG_REG &= ~(`$INSTANCE_NAME`_OPPSIE_EN_MASK | `$INSTANCE_NAME`_OPPS_EN_MASK);
	
    /* Exit critical section */
    CyExitCriticalSection(interruptState);
}


/*******************************************************************************
* Function Name:   `$INSTANCE_NAME`_EnableInt
********************************************************************************
*
* Summary:
*  Enables interrupts of RTC Component.
*
* Parameters:
*  None.
*
* Return:
*  None.
*
*******************************************************************************/
void `$INSTANCE_NAME`_EnableInt(void) `=ReentrantKeil($INSTANCE_NAME . "_EnableInt")`
{
    /* Enable the interrupt */
    CyIntEnable(`$INSTANCE_NAME`_ISR_NUMBER);
}


/*******************************************************************************
* Function Name:   `$INSTANCE_NAME`_DisableInt
********************************************************************************
*
* Summary:
*  Disables interrupts of RTC Component, time and date stop running.
*
* Parameters:
*  None.
*
* Return:
*  None.
*
*******************************************************************************/
void `$INSTANCE_NAME`_DisableInt(void) `=ReentrantKeil($INSTANCE_NAME . "_DisableInt")`
{
    /* Disable the interrupt. */
    CyIntDisable(`$INSTANCE_NAME`_ISR_NUMBER);
}


#if (1u == `$INSTANCE_NAME`_DST_FUNC_ENABLE)
    /*******************************************************************************
    * Function Name:   `$INSTANCE_NAME`_DSTDateConversion
    ********************************************************************************
    *
    * Summary:
    * Converts relative to absolute date.
    *
    * Parameters:
	*  None.
    *
    * Return:
	*  None.
    *
    * Global variables:
    *  `$INSTANCE_NAME`_dstTimeDateStart.Month, 
    *  `$INSTANCE_NAME`_dstTimeDateStart.DayOfWeek,
    *  `$INSTANCE_NAME`_dstTimeDateStart.Week,
    *  `$INSTANCE_NAME`_dstTimeDateStop.Month,
    *  `$INSTANCE_NAME`_dstTimeDateStop.DayOfWeek,
    *  `$INSTANCE_NAME`_dstTimeDateStop.Week,
	*  `$INSTANCE_NAME`_currentTimeDate.Year: these global variables are
	*  used to correct day of week.
	*
    *  `$INSTANCE_NAME`_dstTimeDateStart.DayOfMonth,
    *  `$INSTANCE_NAME`_dstTimeDateStop.DayOfMonth: these global variables are
	*  modified after convertion.
	*
	* Reentrant:
    *  No.
    *
    *******************************************************************************/
    void `$INSTANCE_NAME`_DSTDateConversion(void)
    {
        uint8 week = 1u;
        uint8 day = 1u;
        uint8 dayOfWeek;

        /* Get day of week   */
        dayOfWeek = `$INSTANCE_NAME`_DayOfWeek(day, \
                                      `$INSTANCE_NAME`_dstTimeDateStart.Month, \
                                      `$INSTANCE_NAME`_currentTimeDate.Year) + 1u;

        /* Normalize day of week */
        if(dayOfWeek > `$INSTANCE_NAME`_START_OF_WEEK)
        {
            dayOfWeek -= `$INSTANCE_NAME`_START_OF_WEEK;
        }
        else
        {
            dayOfWeek = `$INSTANCE_NAME`_DAYS_IN_WEEK - (`$INSTANCE_NAME`_START_OF_WEEK - dayOfWeek);
        }

        /* Correct if out of DST range */
        while(dayOfWeek != `$INSTANCE_NAME`_dstTimeDateStart.DayOfWeek)
        {
            day++;
            dayOfWeek++;
            if (dayOfWeek > `$INSTANCE_NAME`_WEEK_ELAPSED)
            {
                dayOfWeek = 1u;
                week++;
            }
        }

        while(week != `$INSTANCE_NAME`_dstTimeDateStart.Week)
        {
            day += `$INSTANCE_NAME`_DAYS_IN_WEEK;
            week++;
        }
        `$INSTANCE_NAME`_dstTimeDateStart.DayOfMonth = day;

        /* Stop of DST time */
        week = 1u;
        day = 1u;

        dayOfWeek = `$INSTANCE_NAME`_DayOfWeek(day, `$INSTANCE_NAME`_dstTimeDateStop.Month, \
                                                    `$INSTANCE_NAME`_currentTimeDate.Year) + 1u;

        if(dayOfWeek > `$INSTANCE_NAME`_START_OF_WEEK)
        {
            dayOfWeek -= `$INSTANCE_NAME`_START_OF_WEEK;
        }
        else
        {
            dayOfWeek = `$INSTANCE_NAME`_DAYS_IN_WEEK - (`$INSTANCE_NAME`_START_OF_WEEK - dayOfWeek);
        }

        while(dayOfWeek != `$INSTANCE_NAME`_dstTimeDateStop.DayOfWeek)
        {
            day++;
            dayOfWeek++;
            if (dayOfWeek > `$INSTANCE_NAME`_WEEK_ELAPSED)
            {
                dayOfWeek = 1u;
                week++;
            }
        }

        while(week != `$INSTANCE_NAME`_dstTimeDateStop.Week)
        {
            day += `$INSTANCE_NAME`_DAYS_IN_WEEK;
            week++;
        }

        `$INSTANCE_NAME`_dstTimeDateStop.DayOfMonth = day;
    }
#endif /* 1u == `$INSTANCE_NAME`_DST_FUNC_ENABLE */


/*******************************************************************************
* Function Name:   `$INSTANCE_NAME`_Init
********************************************************************************
*
* Summary:
*  Calculates required date and flags, sets interrupt vector and priority.
*
* Parameters:
*  None.
*
* Return:
*  None.
*
* Global variables:
*  `$INSTANCE_NAME`_currentTimeDate, `$INSTANCE_NAME`_dstTimeDateStart, 
*  `$INSTANCE_NAME`_dstTimeDateStop, `$INSTANCE_NAME`_dstTimeDateStart,
*  `$INSTANCE_NAME`_alarmCfgTimeDate, `$INSTANCE_NAME`_statusDateTime,
*  `$INSTANCE_NAME`_dstStartStatus, `$INSTANCE_NAME`_dstStartStatus,
*  `$INSTANCE_NAME`_dstStopStatus, `$INSTANCE_NAME`_alarmCurStatus: 
*  global variables are used by the `$INSTANCE_NAME`_SetInitValues().
*
*  `$INSTANCE_NAME`_dstTimeDateStart, `$INSTANCE_NAME`_currentTimeDate:
*  `$INSTANCE_NAME`_statusDateTime, `$INSTANCE_NAME`_dstStartStatus, 
*  `$INSTANCE_NAME`_dstStartStatus, `$INSTANCE_NAME`_dstStopStatus, 
*  `$INSTANCE_NAME`_alarmCurStatus: are modified by the 
*  `$INSTANCE_NAME`_SetInitValues() function.
*
* Reentrant:
*  No.
*
*******************************************************************************/
void `$INSTANCE_NAME`_Init(void)
{
    /* Start calculation of required date and flags */
    `$INSTANCE_NAME`_SetInitValues();

    /* Disable Interrupt. */
    CyIntDisable(`$INSTANCE_NAME`_ISR_NUMBER);

    /* Set the ISR to point to the RTC_SUT_isr Interrupt. */
    CyIntSetVector(`$INSTANCE_NAME`_ISR_NUMBER, `$INSTANCE_NAME`_ISR);

    /* Set the priority. */
    CyIntSetPriority(`$INSTANCE_NAME`_ISR_NUMBER, `$INSTANCE_NAME`_ISR_PRIORITY);
}


/*******************************************************************************
* Function Name: `$INSTANCE_NAME`_Enable
********************************************************************************
*
* Summary:
*  Enables the interrupts, one pulse per second and interrupt generation on OPPS
*  event.
*
* Parameters:
*  None.
*
* Return:
*  None.
*
* Side Effects:
*  Enables for the one pulse per second and cetral time wheel signals to wake up
*  device from the low power (Sleep and Alternate Active) modes and leaves them
*  enabled.
*
*******************************************************************************/
void `$INSTANCE_NAME`_Enable(void) `=ReentrantKeil($INSTANCE_NAME . "_Enable")`
{
	uint8 interruptState;

    /* Enter critical section */
    interruptState = CyEnterCriticalSection();
	
	/* Enable one pulse per second event and interrupt */
    `$INSTANCE_NAME`_OPPS_CFG_REG |= (`$INSTANCE_NAME`_OPPS_EN_MASK | `$INSTANCE_NAME`_OPPSIE_EN_MASK);

    /* Exit critical section */
    CyExitCriticalSection(interruptState);
	
	/* Enable interrupt */
    CyIntEnable(`$INSTANCE_NAME`_ISR_NUMBER);
}


/*******************************************************************************
* Function Name:   `$INSTANCE_NAME`_ReadTime
********************************************************************************
*
* Summary:
*  Returns a pointer to the current time and date structure.
*
* Parameters:
*  None.
*
* Return:
*  `$INSTANCE_NAME`_currentTimeDate: pointer to the global structure with the
*  current date and time values.
*
* Global variables:
*  `$INSTANCE_NAME`_currentTimeDate: global variable with current date and 
*   time is used.
*
* Side Effects:
*  You should disable the interrupt for the RTC component before calling any 
*  read API to avoid an RTC Counter increment in the middle of a time or date
*  read operation. Re-enable the interrupts after the data is read.
*
*******************************************************************************/
`$INSTANCE_NAME`_TIME_DATE* `$INSTANCE_NAME`_ReadTime(void) `=ReentrantKeil($INSTANCE_NAME . "_ReadTime")`
{
    /* Returns a pointer to the current time and date structure */
    return (&`$INSTANCE_NAME`_currentTimeDate);
}


/*******************************************************************************
* Function Name:   `$INSTANCE_NAME`_WriteTime
********************************************************************************
*
* Summary:
*  Writes time and date values as current time and date. Only
*  passes Milliseconds(optionaly), Seconds, Minutes, Hours, Month,
*  Day Of Month and Year.
*
* Parameters:
*  timeDate: Pointer to `$INSTANCE_NAME`_TIME_DATE global stucture where new 
*  values of time and date are stored.
*
* Return:
*  None.
*
* Global variables:
*  `$INSTANCE_NAME`_currentTimeDate: global structure is modified with the new
*  values of current date and time.
*
* Reentrant:
*  No.
*
*******************************************************************************/
void `$INSTANCE_NAME`_WriteTime(`$INSTANCE_NAME`_TIME_DATE *timeDate)
{
    /* Disable Interrupt of RTC Component */
    `$INSTANCE_NAME`_DisableInt();

    /* Write current time and date */
    `$INSTANCE_NAME`_currentTimeDate.Sec = timeDate->Sec ;
    `$INSTANCE_NAME`_currentTimeDate.Min = timeDate->Min ;
    `$INSTANCE_NAME`_currentTimeDate.Hour = timeDate->Hour ;
    `$INSTANCE_NAME`_currentTimeDate.DayOfMonth = timeDate->DayOfMonth ;
    `$INSTANCE_NAME`_currentTimeDate.Month = timeDate->Month;
    `$INSTANCE_NAME`_currentTimeDate.Year = timeDate->Year;

    /* Enable Interrupt of RTC Component */
    `$INSTANCE_NAME`_EnableInt();
}


/*******************************************************************************
* Function Name:   `$INSTANCE_NAME`_WriteSecond
********************************************************************************
*
* Summary:
*  Writes Sec software register value.
*
* Parameters:
*  second: Seconds value.
*
* Return:
*  None.
*
* Global variables:
*  `$INSTANCE_NAME`_currentTimeDate.Sec: global structure's field where current
*  second's value is modified.
*
* Reentrant:
*  No.
*
*******************************************************************************/
void `$INSTANCE_NAME`_WriteSecond(uint8 second)
{
    /* Save seconds to the current time and date structure */
    `$INSTANCE_NAME`_currentTimeDate.Sec = second;
}


/*******************************************************************************
* Function Name:   `$INSTANCE_NAME`_WriteMinute
********************************************************************************
*
* Summary:
*  Writes Minute value in minutes counter register.
*
* Parameters:
*  minute: Minutes value.
*
* Return:
*  None.
*
* Global variables:
*  `$INSTANCE_NAME`_currentTimeDate.Min: global structure's field where
*  current minute's value is mmodified.
*
* Reentrant:
*  No.
*
*******************************************************************************/
void `$INSTANCE_NAME`_WriteMinute(uint8 minute)
{
    /* Save minutes to the current time and date structure */
    `$INSTANCE_NAME`_currentTimeDate.Min = minute;
}


/*******************************************************************************
* Function Name:   `$INSTANCE_NAME`_WriteHour
********************************************************************************
*
* Summary:
*  Writes Hour software register value.
*
* Parameters:
*  hour: Hours value.
*
* Return:
*  None.
*
* Global variables:
*  `$INSTANCE_NAME`_currentTimeDate.Hour: global structure's field where
*  current hour's value is modified.
*
* Reentrant:
*  No.
*
*******************************************************************************/
void `$INSTANCE_NAME`_WriteHour(uint8 hour)
{
    /* Save hours to the current time and date structure */
    `$INSTANCE_NAME`_currentTimeDate.Hour = hour;
}


/*******************************************************************************
* Function Name:   `$INSTANCE_NAME`_WriteDayOfMonth
********************************************************************************
*
* Summary:
*  Writes DayOfMonth software register value.
*
* Parameters:
*  dayOfMonth: Day Of Month value.
*
* Return:
*  None.
*
* Global variables:
*  `$INSTANCE_NAME`_currentTimeDate.DayOfMonth: global structure's field where
*  current day of month's value is modified.
*
* Reentrant:
*  No.
*
*******************************************************************************/
void `$INSTANCE_NAME`_WriteDayOfMonth(uint8 dayOfMonth)
{
    /* Save day of month to the current time and date structure */
    `$INSTANCE_NAME`_currentTimeDate.DayOfMonth = dayOfMonth;
}


/*******************************************************************************
* Function Name:   `$INSTANCE_NAME`_WriteMonth
********************************************************************************
*
* Summary:
*  Writes Month software register value.
*
* Parameters:
*  month: Month value.
*
* Return:
*  None.
*
* Global variables:
*  `$INSTANCE_NAME`_currentTimeDate.Month: global structure's field where
*  current day of month's value is modified.
*
* Reentrant:
*  No.
*
*******************************************************************************/
void `$INSTANCE_NAME`_WriteMonth(uint8 month)
{
    /* Save months to the current time and date structure */
    `$INSTANCE_NAME`_currentTimeDate.Month = month;
}


/*******************************************************************************
* Function Name:   `$INSTANCE_NAME`_WriteYear
********************************************************************************
*
* Summary:
*  Writes Year software register value.
*
* Parameters:
*  year: Years value.
*
* Return:
*  None.
*
* Global variables:
*  `$INSTANCE_NAME`_currentTimeDate.Year: global structure's field where
*  current year's value is modified.
*
* Reentrant:
*  No.
*
*******************************************************************************/
void `$INSTANCE_NAME`_WriteYear(uint16 year)
{
    /* Save years to the current time and date structure */
    `$INSTANCE_NAME`_currentTimeDate.Year = year;
}


/*******************************************************************************
* Function Name:   `$INSTANCE_NAME`_WriteAlarmSecond
********************************************************************************
*
* Summary:
*  Writes Alarm Sec software register value.
*
* Parameters:
*  second: Alarm Seconds value.
*
* Return:
*  None.
*
* Global variables:
*  `$INSTANCE_NAME`_currentTimeDate.Sec: this global variable is used for 
*  comparison while setting and clearing seconds alarm status variable.
*
*  `$INSTANCE_NAME`_alarmCfgTimeDate.Sec: this global variable is modified to
*  store of the new seconds alarm.
*
*  `$INSTANCE_NAME`_alarmCurStatus: this global variable could be changed if
*  second's alarm will be raisen.
*
* Reentrant:
*  No.
*
*******************************************************************************/
void `$INSTANCE_NAME`_WriteAlarmSecond(uint8 second)
{
    `$INSTANCE_NAME`_alarmCfgTimeDate.Sec = second;

    /* Check second alarm */
    if(`$INSTANCE_NAME`_alarmCfgTimeDate.Sec == `$INSTANCE_NAME`_currentTimeDate.Sec)
    {
        /* Set second alarm */
        `$INSTANCE_NAME`_alarmCurStatus |= `$INSTANCE_NAME`_ALARM_SEC_MASK;
    }
    else    /* no second alarm */
    {
        /* Clear second alarm */
        `$INSTANCE_NAME`_alarmCurStatus &= ~`$INSTANCE_NAME`_ALARM_SEC_MASK;
    }
}


/*******************************************************************************
* Function Name:   `$INSTANCE_NAME`_WriteAlarmMinute
********************************************************************************
*
* Summary:
*  Writes Alarm Min software register value.
*
* Parameters:
*  minute: Alarm minutes value.
*
* Return:
*  None.
*
* Global variables:
*  `$INSTANCE_NAME`_currentTimeDate.Min: this global variable is used for 
*  comparison while setting and clearing minutes alarm status variable.
*
*  `$INSTANCE_NAME`_alarmCfgTimeDate.Min: this global variable is modified to
*  store of the new minutes alarm.
*
*  `$INSTANCE_NAME`_alarmCurStatus: this global variable could be changed if
*  minute's alarm will be raisen.
*
* Reentrant:
*  No.
*
*******************************************************************************/
void `$INSTANCE_NAME`_WriteAlarmMinute(uint8 minute)
{
    `$INSTANCE_NAME`_alarmCfgTimeDate.Min = minute;

    /* Check minute alarm */
    if(`$INSTANCE_NAME`_alarmCfgTimeDate.Min == `$INSTANCE_NAME`_currentTimeDate.Min)
    {
        /* Set minute alarm */
        `$INSTANCE_NAME`_alarmCurStatus |= `$INSTANCE_NAME`_ALARM_MIN_MASK;
    }
    else    /* no minute alarm */
    {
        /* Clear minute alarm */
        `$INSTANCE_NAME`_alarmCurStatus &= ~`$INSTANCE_NAME`_ALARM_MIN_MASK;
    }
}


/*******************************************************************************
* Function Name:   `$INSTANCE_NAME`_WriteAlarmHour
********************************************************************************
*
* Summary:
*  Writes Alarm Hour software register value.
*
* Parameters:
*  hour: Alarm hours value.
*
* Return:
*  None.
*
* Global variables:
*  `$INSTANCE_NAME`_currentTimeDate.Hour: this global variable is used for 
*  comparison while setting and clearing hours alarm status variable.
*
*  `$INSTANCE_NAME`_alarmCfgTimeDate.Hour: this global variable is modified to
*  store of the new hours alarm.
*
*  `$INSTANCE_NAME`_alarmCurStatus: this global variable could be changed if
*  hours alarm will be raisen.
*
* Reentrant:
*  No.
*
*******************************************************************************/
void `$INSTANCE_NAME`_WriteAlarmHour(uint8 hour)
{
    `$INSTANCE_NAME`_alarmCfgTimeDate.Hour = hour;

    /* Check hour alarm */
    if(`$INSTANCE_NAME`_alarmCfgTimeDate.Hour == `$INSTANCE_NAME`_currentTimeDate.Hour)
    {
        /* Set hour alarm */
        `$INSTANCE_NAME`_alarmCurStatus |= `$INSTANCE_NAME`_ALARM_HOUR_MASK;
    }
    else    /* no hour alarm */
    {
        /* Clear hour alarm */
        `$INSTANCE_NAME`_alarmCurStatus &= ~`$INSTANCE_NAME`_ALARM_HOUR_MASK;
    }
}


/*******************************************************************************
* Function Name:   `$INSTANCE_NAME`_WriteAlarmDayOfMonth
********************************************************************************
*
* Summary:
*  Writes Alarm DayOfMonth software register value.
*
* Parameters:
*  dayOfMonth: Alarm day of month value.
*
* Return:
*  None.
*
* Global variables:
*  `$INSTANCE_NAME`_currentTimeDate.DayOfMonth: this global variable is used for 
*  comparison while setting and clearing day of month alarm status variable.
*
*  `$INSTANCE_NAME`_alarmCfgTimeDate.DayOfMonth: this global variable is
*  modified to store of the new day of month alarm.
*
*  `$INSTANCE_NAME`_alarmCurStatus: this global variable could be changed if
*  day of month alarm will be raisen.
*
* Reentrant:
*  No.
*
*******************************************************************************/
void `$INSTANCE_NAME`_WriteAlarmDayOfMonth(uint8 dayOfMonth)
{
    `$INSTANCE_NAME`_alarmCfgTimeDate.DayOfMonth = dayOfMonth;

    /* Check day of month alarm */
    if(`$INSTANCE_NAME`_alarmCfgTimeDate.DayOfMonth == `$INSTANCE_NAME`_currentTimeDate.DayOfMonth)
    {
        /* Set day of month alarm */
        `$INSTANCE_NAME`_alarmCurStatus |= `$INSTANCE_NAME`_ALARM_DAYOFMONTH_MASK;
    }
    else    /* no day of month alarm */
    {
        /* Clear day of month alarm */
        `$INSTANCE_NAME`_alarmCurStatus &= ~`$INSTANCE_NAME`_ALARM_DAYOFMONTH_MASK;
    }
}


/*******************************************************************************
* Function Name:   `$INSTANCE_NAME`_WriteAlarmMonth
********************************************************************************
*
* Summary:
*  Writes Alarm Month software register value.
*
* Parameters:
*  month: Alarm month value.
*
* Return:
*  None.
*
* Global variables:
*  `$INSTANCE_NAME`_currentTimeDate.Month: this global variable is used for 
*  comparison while setting and clearing month alarm status variable.
*
*  `$INSTANCE_NAME`_alarmCfgTimeDate.Month: this global variable is modified
*  to store of the new month alarm.
*
*  `$INSTANCE_NAME`_alarmCurStatus: this global variable could be changed if
*  month alarm will be raisen.
*
* Reentrant:
*  No.
*
*******************************************************************************/
void `$INSTANCE_NAME`_WriteAlarmMonth(uint8 month)
{
    `$INSTANCE_NAME`_alarmCfgTimeDate.Month = month;

    /* Check month alarm */
    if(`$INSTANCE_NAME`_alarmCfgTimeDate.Month == `$INSTANCE_NAME`_currentTimeDate.Month)
    {
        /* Set month alarm */
        `$INSTANCE_NAME`_alarmCurStatus |= `$INSTANCE_NAME`_ALARM_MONTH_MASK;
    }
    else    /* no month alarm */
    {
        /* Clear month alarm */
        `$INSTANCE_NAME`_alarmCurStatus &= ~`$INSTANCE_NAME`_ALARM_MONTH_MASK;
    }
}


/*******************************************************************************
* Function Name:   `$INSTANCE_NAME`_WriteAlarmYear
********************************************************************************
*
* Summary:
*  Writes Alarm Year software register value.
*
* Parameters:
*  year: Alarm year value.
*
* Return:
*  None.
*
* Global variables:
*  `$INSTANCE_NAME`_currentTimeDate.Year: this global variable is used for 
*  comparison while setting and clearing year alarm status variable.
*
*  `$INSTANCE_NAME`_alarmCfgTimeDate.Year: this global variable is modified
*  to store of the new year alarm.
*
*  `$INSTANCE_NAME`_alarmCurStatus: this global variable could be changed if
*  year alarm will be raisen.
*
* Reentrant:
*  No.
*
*******************************************************************************/
void `$INSTANCE_NAME`_WriteAlarmYear(uint16 year)
{
   `$INSTANCE_NAME`_alarmCfgTimeDate.Year = year;

    /* Check year alarm */
    if(`$INSTANCE_NAME`_alarmCfgTimeDate.Year == `$INSTANCE_NAME`_currentTimeDate.Year)
    {
        /* Set year alarm */
        `$INSTANCE_NAME`_alarmCurStatus |= `$INSTANCE_NAME`_ALARM_YEAR_MASK;
    }
    else    /* no year alarm */
    {
        /* Set year alarm */
        `$INSTANCE_NAME`_alarmCurStatus &= ~`$INSTANCE_NAME`_ALARM_YEAR_MASK;
    }
}


/*******************************************************************************
* Function Name:   `$INSTANCE_NAME`_WriteAlarmDayOfWeek
********************************************************************************
*
* Summary:
*   Writes Alarm DayOfWeek software register value.
*   Days values {Sun = 1, Mon = 2, Tue = 3, Wen = 4, Thu = 5, Fri = 6, Sut = 7}
*
* Parameters:
*  dayOfWeek: Alarm day of week value.
*
* Return:
*  None.
*
* Global variables:
*  `$INSTANCE_NAME`_currentTimeDate.DayOfWeek: this global variable is used for 
*  comparison while setting and clearing day of week alarm status variable.
*
*  `$INSTANCE_NAME`_alarmCfgTimeDate.DayOfWeek: this global variable is modified
*  to store of the new day of week alarm.
*
*  `$INSTANCE_NAME`_alarmCurStatus: this global variable could be changed if
*  day of week alarm will be raisen.
*
* Reentrant:
*  No.
*
*******************************************************************************/
void `$INSTANCE_NAME`_WriteAlarmDayOfWeek(uint8 dayOfWeek)
{
    `$INSTANCE_NAME`_alarmCfgTimeDate.DayOfWeek = dayOfWeek;

    /* Check day of week alarm */
    if(`$INSTANCE_NAME`_alarmCfgTimeDate.DayOfWeek == `$INSTANCE_NAME`_currentTimeDate.DayOfWeek)
    {
        /* Set day of week alarm */
        `$INSTANCE_NAME`_alarmCurStatus |= `$INSTANCE_NAME`_ALARM_DAYOFWEEK_MASK;
    }
    else    /* no day of week alarm */
    {
        /* Set day of week alarm */
        `$INSTANCE_NAME`_alarmCurStatus &= ~`$INSTANCE_NAME`_ALARM_DAYOFWEEK_MASK;
    }
}


/*******************************************************************************
* Function Name:   `$INSTANCE_NAME`_WriteAlarmDayOfYear
********************************************************************************
*
* Summary:
*  Writes Alarm DayOfYear software register value.
*
* Parameters:
*  dayOfYear: Alarm day of year value.
*
* Return:
*  None.
*
* Global variables:
*  `$INSTANCE_NAME`_currentTimeDate.DayOfYear: this global variable is used for 
*  comparison while setting and clearing day of year alarm status variable.
*
*  `$INSTANCE_NAME`_alarmCfgTimeDate.DayOfYear: this global variable is modified
*  to store of the new day of year alarm.
*
*  `$INSTANCE_NAME`_alarmCurStatus: this global variable could be changed if
*  day of year alarm will be raisen.
*
* Reentrant:
*  No.
*
*******************************************************************************/
void `$INSTANCE_NAME`_WriteAlarmDayOfYear(uint16 dayOfYear)
{
  `$INSTANCE_NAME`_alarmCfgTimeDate.DayOfYear = dayOfYear;

    /* Check day of year alarm */
    if(`$INSTANCE_NAME`_alarmCfgTimeDate.DayOfYear == `$INSTANCE_NAME`_currentTimeDate.DayOfYear)
    {
        /* Set day of year alarm */
        `$INSTANCE_NAME`_alarmCurStatus |= `$INSTANCE_NAME`_ALARM_DAYOFYEAR_MASK;
    }
    else    /* no day of year alarm */
    {
        /* Set day of year alarm */
        `$INSTANCE_NAME`_alarmCurStatus &= ~`$INSTANCE_NAME`_ALARM_DAYOFYEAR_MASK;
    }
}


/*******************************************************************************
* Function Name:   `$INSTANCE_NAME`_ReadSecond
********************************************************************************
*
* Summary:
*  Reads Sec software register value.
*
* Parameters:
*  None.
*
* Return:
*  Current seconds value.
*
* Global variables:
*  `$INSTANCE_NAME`_currentTimeDate.Sec: the current second's value is used.
*
*******************************************************************************/
uint8 `$INSTANCE_NAME`_ReadSecond(void) `=ReentrantKeil($INSTANCE_NAME . "_ReadSecond")`
{
    /* Return current second */
    return (`$INSTANCE_NAME`_currentTimeDate.Sec);
}


/*******************************************************************************
* Function Name:   `$INSTANCE_NAME`_ReadMinute
********************************************************************************
*
* Summary:
*  Reads Min software register value.
*
* Parameters:
*  None.
*
* Return:
*  Current field's value is returned.
*
* Global variables:
*  `$INSTANCE_NAME`_currentTimeDate.Min: the current field's value is used.
*
*******************************************************************************/
uint8 `$INSTANCE_NAME`_ReadMinute(void) `=ReentrantKeil($INSTANCE_NAME . "_ReadMinute")`
{
    /* Return current minute */
    return (`$INSTANCE_NAME`_currentTimeDate.Min);
}


/*******************************************************************************
* Function Name:   `$INSTANCE_NAME`_ReadHour
********************************************************************************
*
* Summary:
*  Reads Hour software register value.
*
* Parameters:
*  None.
*
* Return:
*  Current hour's value.
*
* Global variables:
*  `$INSTANCE_NAME`_currentTimeDate.Hour: the current field's value is used.
*
*******************************************************************************/
uint8 `$INSTANCE_NAME`_ReadHour(void) `=ReentrantKeil($INSTANCE_NAME . "_ReadHour")`
{
    /* Return current hour */
    return (`$INSTANCE_NAME`_currentTimeDate.Hour);
}


/*******************************************************************************
* Function Name:   `$INSTANCE_NAME`_ReadDayOfMonth
********************************************************************************
*
* Summary:
*  Reads DayOfMonth software register value.
*
* Parameters:
*  None.
*
* Return:
*  Current value of the day of month. 
*  returned.
*
* Global variables:
*  `$INSTANCE_NAME`_currentTimeDate.DayOfMonth: the current day of month's 
*  value is used.
*
*******************************************************************************/
uint8 `$INSTANCE_NAME`_ReadDayOfMonth(void) `=ReentrantKeil($INSTANCE_NAME . "_ReadDayOfMonth")`
{
    /* Return current day of the month */
    return (`$INSTANCE_NAME`_currentTimeDate.DayOfMonth);
}


/*******************************************************************************
* Function Name:   `$INSTANCE_NAME`_ReadMonth
********************************************************************************
*
* Summary:
*  Reads Month software register value.
*
* Parameters:
*  None.
*
* Return:
*  Current value of the month.  
*
* Global variables:
*  `$INSTANCE_NAME`_currentTimeDate.Month: the current month's value is used.
*
*******************************************************************************/
uint8 `$INSTANCE_NAME`_ReadMonth(void) `=ReentrantKeil($INSTANCE_NAME . "_ReadMonth")`
{
    /* Return current month */
    return (`$INSTANCE_NAME`_currentTimeDate.Month);
}


/*******************************************************************************
* Function Name:   `$INSTANCE_NAME`_ReadYear
********************************************************************************
*
* Summary:
*  Reads Year software register value.
*
* Parameters:
*  None.
*
* Return:
*  Current value of the year. 
*
* Global variables:
*  `$INSTANCE_NAME`_currentTimeDate.Year: the current year's value is used.
*
*******************************************************************************/
uint16 `$INSTANCE_NAME`_ReadYear(void) `=ReentrantKeil($INSTANCE_NAME . "_ReadYear")`
{
    /* Return current year */
    return (`$INSTANCE_NAME`_currentTimeDate.Year);
}


/*******************************************************************************
* Function Name:   `$INSTANCE_NAME`_ReadAlarmSecond
********************************************************************************
*
* Summary:
*  Reads Alarm Sec software register value.
*
* Parameters:
*  None.
*
* Return:
*  Current alarm value of the seconds.
*
* Global variables:
*  `$INSTANCE_NAME`_alarmCfgTimeDate.Sec: the current second alarm value is
*  used.
*
********************************************************************************/
uint8 `$INSTANCE_NAME`_ReadAlarmSecond(void) `=ReentrantKeil($INSTANCE_NAME . "_ReadAlarmSecond")`
{
    /* Return current alarm second */
    return (`$INSTANCE_NAME`_alarmCfgTimeDate.Sec);
}


/*******************************************************************************
* Function Name:   `$INSTANCE_NAME`_ReadAlarmMinute
********************************************************************************
*
* Summary:
*  Reads Alarm Min software register value.
*
* Parameters:
*  None.
*
* Return:
*  Current alarm value of the minutes.
*
* Global variables:
*  `$INSTANCE_NAME`_alarmCfgTimeDate.Min: the current minute alarm is used.
*
*******************************************************************************/
uint8 `$INSTANCE_NAME`_ReadAlarmMinute(void) `=ReentrantKeil($INSTANCE_NAME . "_ReadAlarmMinute")`
{
    /* Return current alarm minute */
    return (`$INSTANCE_NAME`_alarmCfgTimeDate.Min);
}


/*******************************************************************************
* Function Name:   `$INSTANCE_NAME`_ReadAlarmHour
********************************************************************************
*
* Summary:
*  Reads Alarm Hour software register value.
*
* Parameters:
*  None.  
*
* Return:
*  Current alarm value of the hours.
*
* Global variables:
*  `$INSTANCE_NAME`_alarmCfgTimeDate.Hour: the current hour alarm value is used. 
*
*******************************************************************************/
uint8 `$INSTANCE_NAME`_ReadAlarmHour(void) `=ReentrantKeil($INSTANCE_NAME . "_ReadAlarmHour")`
{
    /* Return current alarm hour */
    return (`$INSTANCE_NAME`_alarmCfgTimeDate.Hour);
}


/*******************************************************************************
* Function Name:   `$INSTANCE_NAME`_ReadAlarmDayOfMonth
********************************************************************************
*
* Summary:
*  Reads Alarm DayOfMonth software register value.
*
* Parameters:
*  None.
*
* Return:
*  Current alarm value of the day of month.
*
* Global variables:
*  `$INSTANCE_NAME`_alarmCfgTimeDate.DayOfMonth: the current day of month alarm 
*  value is used.
*
*******************************************************************************/
uint8 `$INSTANCE_NAME`_ReadAlarmDayOfMonth(void) `=ReentrantKeil($INSTANCE_NAME . "_ReadAlarmDayOfMonth")`
{
    /* Return current alarm day of month */
    return (`$INSTANCE_NAME`_alarmCfgTimeDate.DayOfMonth);
}


/*******************************************************************************
* Function Name:   `$INSTANCE_NAME`_ReadAlarmMonth
********************************************************************************
*
* Summary:
*  Reads Alarm Month software register value.
*
* Parameters:
*  None.
*
* Return:
*  Current alarm value of the month.
*
* Global variables:
*  `$INSTANCE_NAME`_alarmCfgTimeDate.Month: the current month alarm value is
*  used.
*
*******************************************************************************/
uint8 `$INSTANCE_NAME`_ReadAlarmMonth(void) `=ReentrantKeil($INSTANCE_NAME . "_ReadAlarmMonth")`
{
    /* Return current alarm month */
    return (`$INSTANCE_NAME`_alarmCfgTimeDate.Month);
}


/*******************************************************************************
* Function Name:   `$INSTANCE_NAME`_ReadAlarmYear
********************************************************************************
*
* Summary:
*  Reads Alarm Year software register value.
*
* Parameters:
*  None.
*
* Return:
*  Current alarm value of the years.
*
* Global variables:
*  `$INSTANCE_NAME`_alarmCfgTimeDate.Year: the current year alarm value is used.
*
*******************************************************************************/
uint16 `$INSTANCE_NAME`_ReadAlarmYear(void) `=ReentrantKeil($INSTANCE_NAME . "_ReadAlarmYear")`
{
    /* Return current alarm year */
    return (`$INSTANCE_NAME`_alarmCfgTimeDate.Year);
}


/*******************************************************************************
* Function Name:   `$INSTANCE_NAME`_ReadAlarmDayOfWeek
********************************************************************************
*
* Summary:
*  Reads Alarm DayOfWeek software register value.
*
* Parameters:
*  None.
*
* Return:
*  Current alarm value of the day of week.
*
* Global variables:
*  `$INSTANCE_NAME`_alarmCfgTimeDate.DayOfWeek: the current day of week alarm
*  value is used.
*
*******************************************************************************/
uint8 `$INSTANCE_NAME`_ReadAlarmDayOfWeek(void) `=ReentrantKeil($INSTANCE_NAME . "_ReadAlarmDayOfWeek")`
{
    /* Return current alarm day of the week */
    return (`$INSTANCE_NAME`_alarmCfgTimeDate.DayOfWeek);
}


/*******************************************************************************
* Function Name:   `$INSTANCE_NAME`_ReadAlarmDayOfYear
********************************************************************************
*
* Summary:
*  Reads Alarm DayOfYear software register value.
*
* Parameters:
*  None.
*
* Return:
*  Current alarm value of the day of year.
*
* Global variables:
*  `$INSTANCE_NAME`_alarmCfgTimeDate.DayOfYear: the current day of year alarm 
*  value is used.
*
*******************************************************************************/
uint16 `$INSTANCE_NAME`_ReadAlarmDayOfYear(void) `=ReentrantKeil($INSTANCE_NAME . "_ReadAlarmDayOfYear")`
{
    /* Return current alarm day of the year */
    return  (`$INSTANCE_NAME`_alarmCfgTimeDate.DayOfYear);
}


/*******************************************************************************
* Function Name:   `$INSTANCE_NAME`_WriteAlarmMask
********************************************************************************
*
* Summary:
*  Writes the Alarm Mask software register with 1 bit per time/date entry. Alarm
*  true when all masked time/date values match Alarm values.
*
* Parameters:
*  mask: Alarm Mask software register value.
*
* Return:
*  None.
*
* Global variables:
*  `$INSTANCE_NAME`_alarmCfgMask: global variable which stores masks for 
*  time/date alarm configuration is modified.
*
* Reentrant:
*  No.
*
*******************************************************************************/
void `$INSTANCE_NAME`_WriteAlarmMask(uint8 mask)
{
    `$INSTANCE_NAME`_alarmCfgMask = mask;
}


/*******************************************************************************
* Function Name:   `$INSTANCE_NAME`_WriteIntervalMask
********************************************************************************
*
* Summary:
*  Writes the Interval Mask software register with 1 bit per time/date entry.
*  Interrupt true when any masked time/date overflow occur.
*
* Parameters:
*  mask: Alarm Mask software register value.
*
* Return:
*  None. 
*
* Global variables:
*  `$INSTANCE_NAME`_intervalCfgMask: this global variable is modified - the new
*  value of interval mask is stored here.
*
* Reentrant:
*  No.
*
*******************************************************************************/
void `$INSTANCE_NAME`_WriteIntervalMask(uint8 mask)
{
    `$INSTANCE_NAME`_intervalCfgMask = mask;
}


/*******************************************************************************
* Function Name:   `$INSTANCE_NAME`_ReadStatus
********************************************************************************
*
* Summary:
*  Reads the Status software register which has flags for DST
*  (DST), Leap Year (LY) and AM/PM (AM_PM), Alarm active (AA).
*
* Parameters:
*  None.
*
* Return:
*  None.
*
* Global variables:
*  `$INSTANCE_NAME`_statusDateTime: global variable is modified - active alarm
*  status bit is cleared.
*
* Reentrant:
*  No.
*
* Side Effects:
*  Alarm active(AA) flag clear after read.
*
*******************************************************************************/
uint8 `$INSTANCE_NAME`_ReadStatus(void)
{
    uint8 status = 0u;

    /* Save status */
    status = (uint8)`$INSTANCE_NAME`_statusDateTime;

    /* Clean AA flag after read of Status Register */
    `$INSTANCE_NAME`_statusDateTime &= ~`$INSTANCE_NAME`_STATUS_AA;

    return (status);
}


/*******************************************************************************
* Function Name:   `$INSTANCE_NAME`_DayOfWeek
********************************************************************************
*
* Summary:
*  Calculates Day Of Week value use Zeller's congruence.
*
* Parameters:
*  dayOfMonth: Day Of Month value.
*  month: Month value.
*  year: Year value.
*
* Return:
*  Day Of Week value.
*
*******************************************************************************/
uint8 `$INSTANCE_NAME`_DayOfWeek(uint8 dayOfMonth, uint8 month, uint16 year) `=ReentrantKeil($INSTANCE_NAME . "_DayOfWeek")`
{
    /* It is simpler to handle the modified year year, which is year - 1 during 
    * January and February
    */
    if (month < `$INSTANCE_NAME`_MARCH)
    {
        year = year - 1;
    }
    
    /* For Gregorian calendar: d = (day + y + y/4 - y/100 + y/400 + (31*m)/12) mod 7 */
    return ((uint8)((year + year/4 - year/100 + year/400 + `$INSTANCE_NAME`_monthTemplate[month-1] + dayOfMonth) % \
                    `$INSTANCE_NAME`_DAYS_IN_WEEK));
}


/*******************************************************************************
* Function Name:   `$INSTANCE_NAME`_SetInitValues
********************************************************************************
*
* Summary:
*    Does all initial calculation.
*    - Set LP Year flag;
*    - Set AM/PM flag;
*    - DayOfWeek;
*    - DayOfYear;
*    - Set DST flag;
*    - Convert relative to absolute date.
*
* Parameters:
*  None.
*
* Return:
*  None.   
*
* Global variables:
*  `$INSTANCE_NAME`_currentTimeDate, `$INSTANCE_NAME`_dstTimeDateStart, 
*  `$INSTANCE_NAME`_dstTimeDateStop, `$INSTANCE_NAME`_dstTimeDateStart,
*  `$INSTANCE_NAME`_alarmCfgTimeDate, `$INSTANCE_NAME`_statusDateTime,
*  `$INSTANCE_NAME`_dstStartStatus, `$INSTANCE_NAME`_dstStartStatus,
*  `$INSTANCE_NAME`_dstStopStatus, `$INSTANCE_NAME`_alarmCurStatus: 
*  global variables are used while the initial calculation.
*
* `$INSTANCE_NAME`_dstTimeDateStart, `$INSTANCE_NAME`_currentTimeDate,
*  `$INSTANCE_NAME`_statusDateTime, `$INSTANCE_NAME`_dstStartStatus, 
*  `$INSTANCE_NAME`_dstStartStatus, `$INSTANCE_NAME`_dstStopStatus, 
*  `$INSTANCE_NAME`_alarmCurStatus: global variables are modified with the
*  initial calculated data.
*
* Reentrant:
*  No.
*
*******************************************************************************/
void `$INSTANCE_NAME`_SetInitValues(void)
{
    uint8 i;

    /* Clears day of month counter */
    `$INSTANCE_NAME`_currentTimeDate.DayOfYear = 0u;
    
    /* Increments day of year value with day in current month */
    `$INSTANCE_NAME`_currentTimeDate.DayOfYear += `$INSTANCE_NAME`_currentTimeDate.DayOfMonth;

    /* Check leap year */
    if(1u == `$INSTANCE_NAME`_LEAP_YEAR(`$INSTANCE_NAME`_currentTimeDate.Year))
    {
        /* Set LP Year flag */
        `$INSTANCE_NAME`_statusDateTime |= `$INSTANCE_NAME`_STATUS_LY;
    }   /* leap year flag was set */
    else
    {
        /* Clear LP Year flag */
        `$INSTANCE_NAME`_statusDateTime &= ~`$INSTANCE_NAME`_STATUS_LY;
    }   /* leap year flag was cleared */

    /* Day Of Year */
    for(i = 0u; i < `$INSTANCE_NAME`_currentTimeDate.Month - 1u; i++)
    {
        /* Increment on days in passed months */
        `$INSTANCE_NAME`_currentTimeDate.DayOfYear += `$INSTANCE_NAME`_daysInMonths[i];
    }   /* day of year is calculated */

    /* Leap year check */
    if(`$INSTANCE_NAME`_IS_BIT_SET(`$INSTANCE_NAME`_statusDateTime, `$INSTANCE_NAME`_STATUS_LY))
    {
        /* Leap day check */
        if(!(`$INSTANCE_NAME`_currentTimeDate.DayOfMonth <= `$INSTANCE_NAME`_DAYS_IN_FEBRUARY + 1u &&
              `$INSTANCE_NAME`_currentTimeDate.Month <= `$INSTANCE_NAME`_FEBRUARY))
        {
            /* Add leap day */
            `$INSTANCE_NAME`_currentTimeDate.DayOfYear++;
        }   /* Do nothing for non leap day */
    }   /* Do nothing for not leap year */

    /* DayOfWeek */
    `$INSTANCE_NAME`_currentTimeDate.DayOfWeek = `$INSTANCE_NAME`_DayOfWeek( \
                                                                        `$INSTANCE_NAME`_currentTimeDate.DayOfMonth, \
                                                                        `$INSTANCE_NAME`_currentTimeDate.Month, \
                                                                        `$INSTANCE_NAME`_currentTimeDate.Year) + 1u;
    
    if (`$INSTANCE_NAME`_currentTimeDate.DayOfWeek > `$INSTANCE_NAME`_START_OF_WEEK)
    {
        `$INSTANCE_NAME`_currentTimeDate.DayOfWeek -= `$INSTANCE_NAME`_START_OF_WEEK;
    }
    else
    {
        `$INSTANCE_NAME`_currentTimeDate.DayOfWeek = `$INSTANCE_NAME`_DAYS_IN_WEEK - \
                                        (`$INSTANCE_NAME`_START_OF_WEEK - `$INSTANCE_NAME`_currentTimeDate.DayOfWeek);
    }

    #if(1u == `$INSTANCE_NAME`_DST_FUNC_ENABLE)

        /* If DST values is given in a relative manner, converts to the absolute
        * values
        */
        if(`$INSTANCE_NAME`_IS_BIT_SET(`$INSTANCE_NAME`_dstModeType, `$INSTANCE_NAME`_DST_RELDATE))
        {
            `$INSTANCE_NAME`_DSTDateConversion();
        }

        /* Sets DST status respect to the DST start date and time */
        if(`$INSTANCE_NAME`_currentTimeDate.Month > `$INSTANCE_NAME`_dstTimeDateStart.Month)
        {
            `$INSTANCE_NAME`_statusDateTime |= `$INSTANCE_NAME`_STATUS_DST;
        }
        else if(`$INSTANCE_NAME`_currentTimeDate.Month == `$INSTANCE_NAME`_dstTimeDateStart.Month)
        {
            if(`$INSTANCE_NAME`_currentTimeDate.DayOfMonth > `$INSTANCE_NAME`_dstTimeDateStart.DayOfMonth)
            {
                `$INSTANCE_NAME`_statusDateTime |= `$INSTANCE_NAME`_STATUS_DST;
            }
            else if(`$INSTANCE_NAME`_currentTimeDate.DayOfMonth == `$INSTANCE_NAME`_dstTimeDateStart.DayOfMonth)
            {
                if(`$INSTANCE_NAME`_currentTimeDate.Hour > `$INSTANCE_NAME`_dstTimeDateStart.Hour)
                {
                    `$INSTANCE_NAME`_statusDateTime |= `$INSTANCE_NAME`_STATUS_DST;
                }
            }
        }

        /* Clears DST status respect to the DST start date and time */
        if(`$INSTANCE_NAME`_currentTimeDate.Month > `$INSTANCE_NAME`_dstTimeDateStop.Month)
        {
            `$INSTANCE_NAME`_statusDateTime &= ~`$INSTANCE_NAME`_STATUS_DST;
        }
        else if(`$INSTANCE_NAME`_currentTimeDate.Month == `$INSTANCE_NAME`_dstTimeDateStop.Month)
        {
            if(`$INSTANCE_NAME`_currentTimeDate.DayOfMonth > `$INSTANCE_NAME`_dstTimeDateStop.DayOfMonth)
            {
                `$INSTANCE_NAME`_statusDateTime &= ~`$INSTANCE_NAME`_STATUS_DST;
            }
            else if(`$INSTANCE_NAME`_currentTimeDate.DayOfMonth == `$INSTANCE_NAME`_dstTimeDateStop.DayOfMonth)
            {
                if(`$INSTANCE_NAME`_currentTimeDate.Hour > `$INSTANCE_NAME`_dstTimeDateStop.Hour)
                {
                    `$INSTANCE_NAME`_statusDateTime &= ~`$INSTANCE_NAME`_STATUS_DST;
                }
            }
            else
            {
                /* Do nothing if current day of month is less than DST stop day of month */
            }
        }
        else
        {
            /* Do nothing if current month is before than DST stop month */
        }

        /* Clear DST start/stop statuses */
        `$INSTANCE_NAME`_dstStartStatus = 0u;
        `$INSTANCE_NAME`_dstStopStatus = 0u;

        /* Sets DST stop status month flag if DST stop month is equal to the 
        * current month, otherwise clears that flag.
        */
        if(`$INSTANCE_NAME`_dstTimeDateStop.Month == `$INSTANCE_NAME`_currentTimeDate.Month)
        {
            `$INSTANCE_NAME`_dstStopStatus |= `$INSTANCE_NAME`_DST_MONTH;
        }
        else
        {
            `$INSTANCE_NAME`_dstStopStatus &= ~`$INSTANCE_NAME`_DST_MONTH;
        }

        /* Sets DST start status month flag if DST start month is equal to the 
        * current month, otherwise clears that flag.
        */
        if(`$INSTANCE_NAME`_dstTimeDateStart.Month == `$INSTANCE_NAME`_currentTimeDate.Month)
        {
            `$INSTANCE_NAME`_dstStartStatus |= `$INSTANCE_NAME`_DST_MONTH;
        }
        else
        {
            `$INSTANCE_NAME`_dstStartStatus &= ~`$INSTANCE_NAME`_DST_MONTH;
        }

        /* Sets DST stop status day of month flag if DST stop day of month is
        * equal to the current day of month, otherwise clears that flag.
        */        
        if(`$INSTANCE_NAME`_dstTimeDateStop.DayOfMonth == `$INSTANCE_NAME`_currentTimeDate.DayOfMonth)
        {
            `$INSTANCE_NAME`_dstStopStatus |= `$INSTANCE_NAME`_DST_DAYOFMONTH;
        }
        else
        {
            `$INSTANCE_NAME`_dstStopStatus &= ~`$INSTANCE_NAME`_DST_DAYOFMONTH;
        }

        /* Sets DST start status day of month flag if DST start day of month is
        * equal to the current day of month, otherwise clears that flag.
        */ 
        if(`$INSTANCE_NAME`_dstTimeDateStart.DayOfMonth == `$INSTANCE_NAME`_currentTimeDate.DayOfMonth)
        {
            `$INSTANCE_NAME`_dstStartStatus |= `$INSTANCE_NAME`_DST_DAYOFMONTH;
        }
        else
        {
            `$INSTANCE_NAME`_dstStartStatus &= ~`$INSTANCE_NAME`_DST_DAYOFMONTH;
        }

        /* Sets DST stop status hour flag if DST stop hour is equal to the
        * current hour, otherwise clears that flag.
        */   
        if(`$INSTANCE_NAME`_dstTimeDateStop.Hour == `$INSTANCE_NAME`_currentTimeDate.Hour)
        {
            `$INSTANCE_NAME`_dstStopStatus |= `$INSTANCE_NAME`_DST_HOUR;
        }
        else
        {
            `$INSTANCE_NAME`_dstStopStatus &= ~`$INSTANCE_NAME`_DST_HOUR;
        }

        /* Sets DST start status hour flag if DST start hour is equal to the
        * current hour, otherwise clears that flag.
        */   
        if(`$INSTANCE_NAME`_dstTimeDateStart.Hour == `$INSTANCE_NAME`_currentTimeDate.Hour)
        {
            `$INSTANCE_NAME`_dstStartStatus |= `$INSTANCE_NAME`_DST_HOUR;
        }
        else
        {
            `$INSTANCE_NAME`_dstStartStatus &= ~`$INSTANCE_NAME`_DST_HOUR;
        }

        /* DST Enable ? */
        if(`$INSTANCE_NAME`_IS_BIT_SET(`$INSTANCE_NAME`_dstModeType, `$INSTANCE_NAME`_DST_ENABLE))
        {
            if(`$INSTANCE_NAME`_IS_BIT_SET(`$INSTANCE_NAME`_statusDateTime,`$INSTANCE_NAME`_STATUS_DST))
            {
                if(`$INSTANCE_NAME`_IS_BIT_SET(`$INSTANCE_NAME`_dstStartStatus, \
                                               (`$INSTANCE_NAME`_DST_HOUR | `$INSTANCE_NAME`_DST_DAYOFMONTH | \
                                               `$INSTANCE_NAME`_DST_MONTH)))
                {
                    /* Substruct current minutes value with minutes value, what
                    * are out of full hour in DST offset.
                    */
                    `$INSTANCE_NAME`_currentTimeDate.Min -= `$INSTANCE_NAME`_dstOffsetMin % \
                                                        (`$INSTANCE_NAME`_HOUR_ELAPSED + 1u);
                    
                    /* If current minutes value is greater than number of 
                    * minutes in hour - could be only if hour's value is negative
                    */
                    if(`$INSTANCE_NAME`_currentTimeDate.Min > `$INSTANCE_NAME`_HOUR_ELAPSED)
                    {
                        /* Adjust current minutes value. Convert to the positive. */
                        `$INSTANCE_NAME`_currentTimeDate.Min = `$INSTANCE_NAME`_HOUR_ELAPSED - \
                                                            (~`$INSTANCE_NAME`_currentTimeDate.Min);
                        
                        /* Decrement current hours value. */
                        `$INSTANCE_NAME`_currentTimeDate.Hour--;
                    }

                    /* Substruct current hours value with hours value, what
                    *  are full part of hours in DST offset.
                    */
                    `$INSTANCE_NAME`_currentTimeDate.Hour -= `$INSTANCE_NAME`_dstOffsetMin / \
                                                         (`$INSTANCE_NAME`_HOUR_ELAPSED + 1u);

                    /* Check if current hour's value is negative */
                    if(`$INSTANCE_NAME`_currentTimeDate.Hour > `$INSTANCE_NAME`_DAY_ELAPSED)
                    {
                        /* Adjust hour */
                        `$INSTANCE_NAME`_currentTimeDate.Hour = `$INSTANCE_NAME`_DAY_ELAPSED - \
                                                            (~`$INSTANCE_NAME`_currentTimeDate.Hour);
                        
                        /* Decrement day of month, year and week */
                        `$INSTANCE_NAME`_currentTimeDate.DayOfMonth--;
                        `$INSTANCE_NAME`_currentTimeDate.DayOfYear--;
                        `$INSTANCE_NAME`_currentTimeDate.DayOfWeek--;

                        if(0u == `$INSTANCE_NAME`_currentTimeDate.DayOfWeek)
                        {
                            `$INSTANCE_NAME`_currentTimeDate.DayOfWeek = `$INSTANCE_NAME`_DAYS_IN_WEEK;
                        }

                        if(0u == `$INSTANCE_NAME`_currentTimeDate.DayOfMonth)
                        {
                            /* Decrement months value */
                            `$INSTANCE_NAME`_currentTimeDate.Month--;
                            
                            /* The current month is month before 1st one. */
                            if(0u == `$INSTANCE_NAME`_currentTimeDate.Month)
                            {
                                /* December is the month before January */
                                `$INSTANCE_NAME`_currentTimeDate.Month = `$INSTANCE_NAME`_DECEMBER;
                                `$INSTANCE_NAME`_currentTimeDate.DayOfMonth = \
                                            `$INSTANCE_NAME`_daysInMonths[`$INSTANCE_NAME`_currentTimeDate.Month - 1u];

                                /* Decrement years value */
                                `$INSTANCE_NAME`_currentTimeDate.Year--;
                                if(1u == `$INSTANCE_NAME`_LEAP_YEAR(`$INSTANCE_NAME`_currentTimeDate.Year))
                                {
                                    /* Set leap year status flag */
                                    `$INSTANCE_NAME`_statusDateTime |= `$INSTANCE_NAME`_STATUS_LY;
                                    `$INSTANCE_NAME`_currentTimeDate.DayOfYear = `$INSTANCE_NAME`_DAYS_IN_LEAP_YEAR;
                                }
                                else
                                {
                                    /* Clear leap year status flag */
                                    `$INSTANCE_NAME`_statusDateTime &= ~`$INSTANCE_NAME`_STATUS_LY;
                                    `$INSTANCE_NAME`_currentTimeDate.DayOfYear = `$INSTANCE_NAME`_DAYS_IN_YEAR;
                                }
                            }   /* 0u == `$INSTANCE_NAME`_currentTimeDate.Month */
                            else
                            {
                                `$INSTANCE_NAME`_currentTimeDate.DayOfMonth = \
                                            `$INSTANCE_NAME`_daysInMonths[`$INSTANCE_NAME`_currentTimeDate.Month - 1u];
                            }   /* 0u != End of `$INSTANCE_NAME`_currentTimeDate.Month */
                        }   /* 0u == End of `$INSTANCE_NAME`_currentTimeDate.DayOfMonth */
                    }   /* End of `$INSTANCE_NAME`_currentTimeDate.Hour > `$INSTANCE_NAME`_DAY_ELAPSED */
                    
                    /* Clear DST status flag */
                    `$INSTANCE_NAME`_statusDateTime &= ~`$INSTANCE_NAME`_STATUS_DST;
                    /* Clear DST stop status */
                    `$INSTANCE_NAME`_dstStopStatus = 0u;
                }
            }
            else    /* Current time and date DO NOT match DST time and date */
            {
                if(`$INSTANCE_NAME`_IS_BIT_SET(`$INSTANCE_NAME`_dstStartStatus, `$INSTANCE_NAME`_DST_HOUR       | \
                                                                                `$INSTANCE_NAME`_DST_DAYOFMONTH | \
                                                                                `$INSTANCE_NAME`_DST_MONTH))  
                {
                    /* Add Hour and Min */
                    `$INSTANCE_NAME`_currentTimeDate.Min += \
                                                `$INSTANCE_NAME`_dstOffsetMin % (`$INSTANCE_NAME`_HOUR_ELAPSED + 1u);

                    if(`$INSTANCE_NAME`_currentTimeDate.Min > `$INSTANCE_NAME`_HOUR_ELAPSED)
                    {
                        /* Adjust Min */
                        `$INSTANCE_NAME`_currentTimeDate.Min -= (`$INSTANCE_NAME`_HOUR_ELAPSED + 1u);
                        `$INSTANCE_NAME`_currentTimeDate.Hour++;
                    }

                    `$INSTANCE_NAME`_currentTimeDate.Hour += \
                                                `$INSTANCE_NAME`_dstOffsetMin / (`$INSTANCE_NAME`_HOUR_ELAPSED + 1u);
                    if(`$INSTANCE_NAME`_currentTimeDate.Hour > `$INSTANCE_NAME`_DAY_ELAPSED)
                    {
                        /* Adjust hour, add day */
                        `$INSTANCE_NAME`_currentTimeDate.Hour -= (`$INSTANCE_NAME`_DAY_ELAPSED + 1u);
                        `$INSTANCE_NAME`_currentTimeDate.DayOfMonth++;
                        `$INSTANCE_NAME`_currentTimeDate.DayOfYear++;
                        `$INSTANCE_NAME`_currentTimeDate.DayOfWeek++;

                        if(`$INSTANCE_NAME`_currentTimeDate.DayOfWeek > `$INSTANCE_NAME`_WEEK_ELAPSED)
                        {
                            `$INSTANCE_NAME`_currentTimeDate.DayOfWeek = 1u;
                        }

                        if(`$INSTANCE_NAME`_currentTimeDate.DayOfMonth > \
                                            `$INSTANCE_NAME`_daysInMonths[`$INSTANCE_NAME`_currentTimeDate.Month - 1u])
                        {
                            `$INSTANCE_NAME`_currentTimeDate.Month++;
                            `$INSTANCE_NAME`_currentTimeDate.DayOfMonth = 1u;
                            
                            /* Has new year come? */
                            if(`$INSTANCE_NAME`_currentTimeDate.Month > `$INSTANCE_NAME`_YEAR_ELAPSED)
                            {
                                /* Set first month of the year */
                                `$INSTANCE_NAME`_currentTimeDate.Month = `$INSTANCE_NAME`_JANUARY;
                                
                                /* Increment year */
                                `$INSTANCE_NAME`_currentTimeDate.Year++;
                                
                                /* Update leap year status */
                                if(1u == `$INSTANCE_NAME`_LEAP_YEAR(`$INSTANCE_NAME`_currentTimeDate.Year))
                                {
                                    /* LP - true, else - false */
                                    `$INSTANCE_NAME`_statusDateTime |= `$INSTANCE_NAME`_STATUS_LY;
                                }
                                else
                                {
                                    `$INSTANCE_NAME`_statusDateTime &= ~`$INSTANCE_NAME`_STATUS_LY;
                                }
                                
                                /* Set day of year to the first one */
                                `$INSTANCE_NAME`_currentTimeDate.DayOfYear = 1u;
                            }
                        }
                    }
                    `$INSTANCE_NAME`_statusDateTime |= `$INSTANCE_NAME`_STATUS_DST;
                    `$INSTANCE_NAME`_dstStartStatus = 0u;
                }
            }
        }
    #endif /* 1u == `$INSTANCE_NAME`_DST_FUNC_ENABLE */

    /* Set AM/PM flag */
    if(`$INSTANCE_NAME`_currentTimeDate.Hour < `$INSTANCE_NAME`_HALF_OF_DAY_ELAPSED)
    {
        /* AM Hour 00:00-11:59, flag zero */
        `$INSTANCE_NAME`_statusDateTime &= ~`$INSTANCE_NAME`_STATUS_AM_PM;
    }
    else
    {
        /* PM Hour 12:00 - 23:59, flag set */
        `$INSTANCE_NAME`_statusDateTime |= `$INSTANCE_NAME`_STATUS_AM_PM;
    }

    /* Alarm calculation */

    /* Alarm SEC */
    if(`$INSTANCE_NAME`_IS_BIT_SET(`$INSTANCE_NAME`_alarmCfgMask,`$INSTANCE_NAME`_ALARM_SEC_MASK))
    {
        if(`$INSTANCE_NAME`_alarmCfgTimeDate.Sec == `$INSTANCE_NAME`_currentTimeDate.Sec)
        {
            /* Set second alarm */
            `$INSTANCE_NAME`_alarmCurStatus |= `$INSTANCE_NAME`_ALARM_SEC_MASK;
        }
        else
        {
            /* Clear second alarm */
            `$INSTANCE_NAME`_alarmCurStatus &= ~`$INSTANCE_NAME`_ALARM_SEC_MASK;
        }
    }

    /* Alarm MIN */
    if(`$INSTANCE_NAME`_IS_BIT_SET(`$INSTANCE_NAME`_alarmCfgMask, `$INSTANCE_NAME`_ALARM_MIN_MASK))
    {
        if(`$INSTANCE_NAME`_alarmCfgTimeDate.Min == `$INSTANCE_NAME`_currentTimeDate.Min)
        {
            /* Set minute alarm */
            `$INSTANCE_NAME`_alarmCurStatus |= `$INSTANCE_NAME`_ALARM_MIN_MASK;
        }
        else
        {
            /* Clear minute alarm */
            `$INSTANCE_NAME`_alarmCurStatus &= ~`$INSTANCE_NAME`_ALARM_MIN_MASK;
        }
    }

    /* Alarm HOUR */
    if(`$INSTANCE_NAME`_IS_BIT_SET(`$INSTANCE_NAME`_alarmCfgMask, `$INSTANCE_NAME`_ALARM_HOUR_MASK))
    {
        if(`$INSTANCE_NAME`_alarmCfgTimeDate.Hour == `$INSTANCE_NAME`_currentTimeDate.Hour)
        {
            /* Set hour alarm */
            `$INSTANCE_NAME`_alarmCurStatus |= `$INSTANCE_NAME`_ALARM_HOUR_MASK;
        }
        else
        {
            /* Clear hour alarm */
            `$INSTANCE_NAME`_alarmCurStatus &= ~`$INSTANCE_NAME`_ALARM_HOUR_MASK;
        }
    }

    /* Alarm DAYOFWEEK */
    if(`$INSTANCE_NAME`_IS_BIT_SET(`$INSTANCE_NAME`_alarmCfgMask, `$INSTANCE_NAME`_ALARM_DAYOFWEEK_MASK))
    {
        if(`$INSTANCE_NAME`_alarmCfgTimeDate.DayOfWeek == `$INSTANCE_NAME`_currentTimeDate.DayOfWeek)
        {
            /* Set day of week alarm */
            `$INSTANCE_NAME`_alarmCurStatus |= `$INSTANCE_NAME`_ALARM_DAYOFWEEK_MASK;
        }
        else
        {
            /* Clear day of week alarm */
            `$INSTANCE_NAME`_alarmCurStatus &= ~`$INSTANCE_NAME`_ALARM_DAYOFWEEK_MASK;
        }
    }

    /* Alarm DAYOFYEAR */
    if(`$INSTANCE_NAME`_IS_BIT_SET(`$INSTANCE_NAME`_alarmCfgMask, `$INSTANCE_NAME`_ALARM_DAYOFYEAR_MASK))
    {
        if(`$INSTANCE_NAME`_alarmCfgTimeDate.DayOfYear == `$INSTANCE_NAME`_currentTimeDate.DayOfYear)
        {
            /* Set day of year alarm */
            `$INSTANCE_NAME`_alarmCurStatus |= `$INSTANCE_NAME`_ALARM_DAYOFYEAR_MASK;
        }
        else
        {
            /* Clear day of year alarm */
            `$INSTANCE_NAME`_alarmCurStatus &= ~`$INSTANCE_NAME`_ALARM_DAYOFYEAR_MASK;
        }
    }

    /* Alarm DAYOFMONTH */
    if(`$INSTANCE_NAME`_IS_BIT_SET(`$INSTANCE_NAME`_alarmCfgMask, `$INSTANCE_NAME`_ALARM_DAYOFMONTH_MASK))
    {
        if (`$INSTANCE_NAME`_alarmCfgTimeDate.DayOfMonth == `$INSTANCE_NAME`_currentTimeDate.DayOfMonth)
        {
            /* Set day of month alarm */
            `$INSTANCE_NAME`_alarmCurStatus |= `$INSTANCE_NAME`_ALARM_DAYOFMONTH_MASK;
        }
           else
        {
            /* Clear day of month alarm */
            `$INSTANCE_NAME`_alarmCurStatus &= ~`$INSTANCE_NAME`_ALARM_DAYOFMONTH_MASK;
        }
    }

    /* Alarm MONTH */
    if(`$INSTANCE_NAME`_IS_BIT_SET(`$INSTANCE_NAME`_alarmCfgMask, `$INSTANCE_NAME`_ALARM_MONTH_MASK))
    {
        if(`$INSTANCE_NAME`_alarmCfgTimeDate.Month == `$INSTANCE_NAME`_currentTimeDate.Month)
        {
            /* Set month alarm */
            `$INSTANCE_NAME`_alarmCurStatus |= `$INSTANCE_NAME`_ALARM_MONTH_MASK;
        }
        else
        {
            /* Clear month alarm */
            `$INSTANCE_NAME`_alarmCurStatus &= ~`$INSTANCE_NAME`_ALARM_MONTH_MASK;
        }
    }

    /* Alarm YEAR */
    if(`$INSTANCE_NAME`_IS_BIT_SET(`$INSTANCE_NAME`_alarmCfgMask, `$INSTANCE_NAME`_ALARM_YEAR_MASK))
    {
        if(`$INSTANCE_NAME`_alarmCfgTimeDate.Year == `$INSTANCE_NAME`_currentTimeDate.Year)
        {
            /* Set year alarm */
            `$INSTANCE_NAME`_alarmCurStatus |= `$INSTANCE_NAME`_ALARM_YEAR_MASK;
        }
        else
        {
            /* Clear year alarm */
            `$INSTANCE_NAME`_alarmCurStatus &= ~`$INSTANCE_NAME`_ALARM_YEAR_MASK;
        }
    }

    /* Set Alarm flag event */
    `$INSTANCE_NAME`_SET_ALARM(`$INSTANCE_NAME`_alarmCfgMask,   \
                               `$INSTANCE_NAME`_alarmCurStatus, \
                               `$INSTANCE_NAME`_statusDateTime);
}


#if (1u == `$INSTANCE_NAME`_DST_FUNC_ENABLE)
    /*******************************************************************************
    * Function Name:   `$INSTANCE_NAME`_WriteDSTMode
    ********************************************************************************
    *
    * Summary:
    *  Writes the DST mode software register. That enables or disables DST changes
    *  and sets the date mode to fixed date or relative date. Only generated if DST
    *  enabled.
    *
    * Parameters:
    *  mode: DST Mode software register value.
    *
    * Return:
	*  None.
    *
	* Global variables:
	*  `$INSTANCE_NAME`_dstModeType: global variable is modified with the new
	*  DST mode type: relative or fixed.
    *
    *  `$INSTANCE_NAME`_dstTimeDateStart.Month, 
    *  `$INSTANCE_NAME`_dstTimeDateStart.DayOfWeek,
    *  `$INSTANCE_NAME`_dstTimeDateStart.Week:
    *  `$INSTANCE_NAME`_dstTimeDateStop.Month,
    *  `$INSTANCE_NAME`_dstTimeDateStop.DayOfWeek,
    *  `$INSTANCE_NAME`_dstTimeDateStop.Week,
	*  `$INSTANCE_NAME`_currentTimeDate.Year: for the day of week correction,
    *   they are used by `$INSTANCE_NAME`_DSTDateConversion() function if DST
    *   mode is configured to be relative.
    *
    *  `$INSTANCE_NAME`_dstTimeDateStart.DayOfMonth,
    *  `$INSTANCE_NAME`_dstTimeDateStop.DayOfMonth: updated after convertion by 
    *  the `$INSTANCE_NAME`_DSTDateConversion() function if DST mode is 
    *  configured to be relative.
	*
    * Reentrant:
    *  No.
    *
    *******************************************************************************/
    void `$INSTANCE_NAME`_WriteDSTMode(uint8 mode)
    {
        /* Set DST mode */
        `$INSTANCE_NAME`_dstModeType = mode;
        
        if(`$INSTANCE_NAME`_IS_BIT_SET(mode,`$INSTANCE_NAME`_DST_RELDATE))
        {
            /* Convert DST date */
            `$INSTANCE_NAME`_DSTDateConversion();
        }
    }


    /*******************************************************************************
    * Function Name:   `$INSTANCE_NAME`_WriteDSTStartHour
    ********************************************************************************
    *
    * Summary:
    *  Writes the DST Start Hour software register. Used for absolute date entry.
    *  Only generated if DST enabled.
    *
    * Parameters:
    *  hour: DST Start Hour software register value.
    *
    * Return:
    *  None. 
    *
	* Global variables:
	*  `$INSTANCE_NAME`_dstTimeDateStart.Hour: global variable is modified with
	*  the new value. 
	*
    * Reentrant:
    *  No.
    *
    *******************************************************************************/
    void `$INSTANCE_NAME`_WriteDSTStartHour(uint8 hour)
    {
        /* Set DST Start Hour */
        `$INSTANCE_NAME`_dstTimeDateStart.Hour = hour;
    }


    /*******************************************************************************
    * Function Name:   `$INSTANCE_NAME`_WriteDSTStartOfMonth
    ********************************************************************************
    *
    * Summary:
    *  Writes the DST Start DayOfMonth software register. Used for absolute date
    *  entry. Only generated if DST enabled.
    *
    * Parameters:
    *  dayOfMonth: DST Start DayOfMonth software register value.
    *
    * Return:
    *  None. 
    *
	* Global variables:
	*  `$INSTANCE_NAME`_dstTimeDateStart.DayOfMonth: global variable is modified
	*  with the new value. 
	*
    * Reentrant:
    *  No.
    *
    *******************************************************************************/
    void `$INSTANCE_NAME`_WriteDSTStartDayOfMonth(uint8 dayOfMonth)
    {
        /* Set DST Start day of month */
        `$INSTANCE_NAME`_dstTimeDateStart.DayOfMonth = dayOfMonth;
    }


    /*******************************************************************************
    * Function Name:   `$INSTANCE_NAME`_WriteDSTStartMonth
    ********************************************************************************
    *
    * Summary:
    *  Writes the DST Start Month software register. Used for absolute date entry.
    *  Only generated if DST enabled.
    *
    * Parameters:
    *  month: DST Start month software register value.
    *
    * Return:
	*  None.
    *
	* Global variables:
	*  `$INSTANCE_NAME`_dstTimeDateStart.Month: global variable is modified
	*  with the new value. 
	*
    * Reentrant:
    *  No.
    *
    *******************************************************************************/
    void `$INSTANCE_NAME`_WriteDSTStartMonth(uint8 month)
    {
        /* Set DST Start month */
        `$INSTANCE_NAME`_dstTimeDateStart.Month = month;
    }


    /*******************************************************************************
    * Function Name:   `$INSTANCE_NAME`_WriteDSTStartDayOfWeek
    ********************************************************************************
    *
    * Summary:
    *  Writes the DST Start DayOfWeek software register. Used for absolute date
    *  entry. Only generated if DST enabled.
    *
    * Parameters:
    *  dayOfWeek: DST start day of week software register value.
    *
    * Return:
	*  None.
    *
	* Global variables:
    *  `$INSTANCE_NAME`_dstModeType: global variable, where DST mode type:
    *  relative or fixed is stored.
    *
    *  `$INSTANCE_NAME`_dstTimeDateStart.Month, 
    *  `$INSTANCE_NAME`_dstTimeDateStart.DayOfWeek,
    *  `$INSTANCE_NAME`_dstTimeDateStart.Week,
	*  `$INSTANCE_NAME`_dstTimeDateStop.Month,
    *  `$INSTANCE_NAME`_dstTimeDateStop.DayOfWeek,
    *  `$INSTANCE_NAME`_dstTimeDateStop.Week: for the day of week correction,
    *   they are used by `$INSTANCE_NAME`_DSTDateConversion() function if DST
    *   mode is configured to be relative.
    *
    *  `$INSTANCE_NAME`_currentTimeDate.Year: for the day of week calculation,
    *   it is used by `$INSTANCE_NAME`_DSTDateConversion() function if DST
    *   mode is configured to be relative. 
	*
	*  `$INSTANCE_NAME`_dstTimeDateStart.DayOfWeek: global variable is modified
    *  with the new day of week value.
    *
    *  `$INSTANCE_NAME`_dstTimeDateStart.DayOfMonth and
    *  `$INSTANCE_NAME`_dstTimeDateStop.DayOfMonth are modified by 
    *  the `$INSTANCE_NAME`_DSTDateConversion() function if DST mode is 
    *  configured to be relative.
	*
    * Reentrant:
    *  No.
    *
    *******************************************************************************/
    void `$INSTANCE_NAME`_WriteDSTStartDayOfWeek(uint8 dayOfWeek)
    {
        /* Set DST Start day of week */
        `$INSTANCE_NAME`_dstTimeDateStart.DayOfWeek = dayOfWeek;
        
        if(`$INSTANCE_NAME`_IS_BIT_SET(`$INSTANCE_NAME`_dstModeType,`$INSTANCE_NAME`_DST_RELDATE))
        {
            /* Convert DST date */
            `$INSTANCE_NAME`_DSTDateConversion();
        }
    }


    /*******************************************************************************
    * Function Name:   `$INSTANCE_NAME`_WriteDSTStartWeek
    ********************************************************************************
    *
    * Summary:
    *  Writes the DST Start Week software register. Used for absolute date entry.
    *  Only generated if DST enabled.
    *
    * Parameters:
    *  week: DST start week software register value.
    *
    * Return:
    *  None.
    *
	* Global variables:
    *  `$INSTANCE_NAME`_dstTimeDateStart.Week: global variable is modified with
    *   the new week's value of the DST start time/date.
    *
    *  `$INSTANCE_NAME`_dstTimeDateStart.DayOfMonth,
    *  `$INSTANCE_NAME`_dstTimeDateStop.DayOfMonth: is modified after convertion
    *  by the `$INSTANCE_NAME`_DSTDateConversion() function if DST mode is 
    *  configured to be relative.
    *
    *  `$INSTANCE_NAME`_dstModeType: global variable is used for theDST mode
    *   type: relative or fixed store.
    *
    *  `$INSTANCE_NAME`_dstTimeDateStart.Month, 
    *  `$INSTANCE_NAME`_dstTimeDateStart.DayOfWeek,
    *  `$INSTANCE_NAME`_dstTimeDateStart.Week: for the day of week correction,
    *   they are used by `$INSTANCE_NAME`_DSTDateConversion() function if DST
    *   mode is configured to be relative.
    *
    *  `$INSTANCE_NAME`_dstTimeDateStop.Month,
    *  `$INSTANCE_NAME`_dstTimeDateStop.DayOfWeek,
    *  `$INSTANCE_NAME`_dstTimeDateStop.Week: for the day of week correction,
    *   they are used by `$INSTANCE_NAME`_DSTDateConversion() function if DST
    *   mode is configured to be relative.
    *
    *  `$INSTANCE_NAME`_currentTimeDate.Year: for the day of week calculation,
    *   it is used by `$INSTANCE_NAME`_DSTDateConversion() function if DST
    *   mode is configured to be relative.    
	*
    * Reentrant:
    *  No.
    *
    *******************************************************************************/
    void `$INSTANCE_NAME`_WriteDSTStartWeek(uint8 week)
    {
        /* Set DST Start week */
        `$INSTANCE_NAME`_dstTimeDateStart.Week = week;
        
        if(`$INSTANCE_NAME`_IS_BIT_SET(`$INSTANCE_NAME`_dstModeType,`$INSTANCE_NAME`_DST_RELDATE))
        {
            /* Convert DST date */
            `$INSTANCE_NAME`_DSTDateConversion();
        }
    }


    /*******************************************************************************
    * Function Name:   `$INSTANCE_NAME`_WriteDSTStopHour
    ********************************************************************************
    *
    * Summary:
    *  Writes the DST Stop Hour software register. Used for absolute date entry.
    *  Only generated if DST enabled.
    *
    * Parameters:
    *  hour: DST stop hour software register value.
    *
    * Return:
    *  None.
    *
	* Global variables:
	*  RTC_dstTimeDateStart.Hour: global variable is modified with the new hour
    *   of the DST start time/date.
	*
    * Reentrant:
    *  No.
    *
    *******************************************************************************/
    void `$INSTANCE_NAME`_WriteDSTStopHour(uint8 hour)
    {
        /* Set DST Stop hour */
        `$INSTANCE_NAME`_dstTimeDateStop.Hour = hour;
    }


    /*******************************************************************************
    * Function Name:   `$INSTANCE_NAME`_WriteDSTStopDayOfMonth
    ********************************************************************************
    *
    * Summary:
    *  Writes the DST Stop DayOfMonth software register. Used for absolute date
    *  entry. Only generated if DST enabled.
    *
    * Parameters:
    *  dayOfMonth: DST stop day of month software register value.
    *
    * Return:
    *  None.
    *
	* Global variables:
    *  `$INSTANCE_NAME`_dstTimeDateStop.DayOfMonth: global variable is modified
    *  where new day of month's value of the DST stop time/date. 
	*
    * Reentrant:
    *  No.
    *
    *******************************************************************************/
    void `$INSTANCE_NAME`_WriteDSTStopDayOfMonth(uint8 dayOfMonth)
    {
        /* Set DST Start day of month */
        `$INSTANCE_NAME`_dstTimeDateStop.DayOfMonth = dayOfMonth;
    }


    /*******************************************************************************
    * Function Name:   `$INSTANCE_NAME`_WriteDSTStopMonth
    ********************************************************************************
    *
    * Summary:
    *  Writes the DST Stop Month software  register. Used for absolute date entry.
    *  Only generated if DST enabled.
    *
    * Parameters:
    *  month: DST Stop Month software register value.
    *
    * Return:
    *  None.
    *
	* Global variables:
    *  `$INSTANCE_NAME`_dstTimeDateStop.Month: global variable is modified with
    *   the new month of the DST stop time/date.
	*
    * Reentrant:
    *  No.
    *
    *******************************************************************************/
    void `$INSTANCE_NAME`_WriteDSTStopMonth(uint8 month)
    {
        /* Set DST Stop month */
        `$INSTANCE_NAME`_dstTimeDateStop.Month = month;
    }


    /*******************************************************************************
    * Function Name:   `$INSTANCE_NAME`_WriteDSTStopDayOfWeek
    ********************************************************************************
    *
    * Summary:
    *  Writes the DST Stop DayOfWeek software register. Used for relative date
    *  entry. Only generated if DST enabled.
    *
    * Parameters:
    *  dayOfWeek: DST stop day of week software register value.
    *
    * Return:
    *  None.
    *
	* Global variables:
	*  `$INSTANCE_NAME`_dstTimeDateStop.DayOfWeek: global variable is modified
    *   with the day of week of the DST stop time/date.
    *
    *  `$INSTANCE_NAME`_dstModeType: global variable is used to store DST mode
    *   type: relative or fixed.
    *
    *  `$INSTANCE_NAME`_dstTimeDateStart.Month, 
    *  `$INSTANCE_NAME`_dstTimeDateStart.DayOfWeek,
    *  `$INSTANCE_NAME`_dstTimeDateStart.Week,
	*  `$INSTANCE_NAME`_dstTimeDateStop.Month,
    *  `$INSTANCE_NAME`_dstTimeDateStop.DayOfWeek,
    *  `$INSTANCE_NAME`_dstTimeDateStop.Weekfor the day of week correction,
    *   they are used by `$INSTANCE_NAME`_DSTDateConversion() function if DST
    *   mode is configured to be relative.
    *
    *  `$INSTANCE_NAME`_currentTimeDate.Year: for the day of week calculation,
    *   it is used by `$INSTANCE_NAME`_DSTDateConversion() function if DST
    *   mode is configured to be relative.
	*
	*  `$INSTANCE_NAME`_dstTimeDateStop.DayOfWeek: global variable is modified
    *  with the new day of week's value.
    *
    *  `$INSTANCE_NAME`_dstTimeDateStart.DayOfMonth and
    *  `$INSTANCE_NAME`_dstTimeDateStop.DayOfMonth are modified by 
    *  the `$INSTANCE_NAME`_DSTDateConversion() function if DST mode is 
    *  configured to be relative.
	*
    * Reentrant:
    *  No.
    *
    *******************************************************************************/
    void `$INSTANCE_NAME`_WriteDSTStopDayOfWeek(uint8 dayOfWeek)
    {
        /* Set DST Stop day of week */
        `$INSTANCE_NAME`_dstTimeDateStop.DayOfWeek = dayOfWeek;
        
        if(`$INSTANCE_NAME`_IS_BIT_SET(`$INSTANCE_NAME`_dstModeType,`$INSTANCE_NAME`_DST_RELDATE))
        {
            /* Convert DST date */
            `$INSTANCE_NAME`_DSTDateConversion();
        }
    }


    /*******************************************************************************
    * Function Name:   `$INSTANCE_NAME`_WriteDSTStopWeek
    ********************************************************************************
    *
    * Summary:
    *  Writes the DST Stop Week software register. Used for relative date entry.
    *  Only generated if DST enabled.
    *
    * Parameters:
    *  week: DST stop week software register value.
    *
    * Return:
	*  None.
    *
	* Global variables:
	*  `$INSTANCE_NAME`_dstTimeDateStop.Week: global variable used to store the
	*  DST stop time/date is stored.
    *
    *  `$INSTANCE_NAME`_dstModeType: global variable is used to store DST mode
    *   type: relative or fixed.
    *
    *  `$INSTANCE_NAME`_dstTimeDateStart.Month, 
    *  `$INSTANCE_NAME`_dstTimeDateStart.DayOfWeek,
    *  `$INSTANCE_NAME`_dstTimeDateStart.Week,
	*  `$INSTANCE_NAME`_dstTimeDateStop.Month,
    *  `$INSTANCE_NAME`_dstTimeDateStop.DayOfWeek,
    *  `$INSTANCE_NAME`_dstTimeDateStop.Week: used for the day of week correction,
    *   they are used by `$INSTANCE_NAME`_DSTDateConversion() function if DST
    *   mode is configured to be relative.
    *
    *  `$INSTANCE_NAME`_currentTimeDate.Year: for the day of week calculation,
    *   it is used by `$INSTANCE_NAME`_DSTDateConversion() function if DST
    *   mode is configured to be relative. 
	*
	*  `$INSTANCE_NAME`_dstTimeDateStop.Week: global variable is modified with
    *  the new value.
    *
    *  `$INSTANCE_NAME`_dstTimeDateStart.DayOfMonth and 
    *  `$INSTANCE_NAME`_dstTimeDateStop.DayOfMonth are modified by 
    *  the `$INSTANCE_NAME`_DSTDateConversion() function if DST mode is 
    *  configured to be relative.
	*
    * Reentrant:
    *  No.
    *
    *******************************************************************************/
    void `$INSTANCE_NAME`_WriteDSTStopWeek(uint8 week)
    {
        /* Set DST Stop week */
        `$INSTANCE_NAME`_dstTimeDateStop.Week = week;
        
        if(`$INSTANCE_NAME`_IS_BIT_SET(`$INSTANCE_NAME`_dstModeType,`$INSTANCE_NAME`_DST_RELDATE))
        {
            /* Convert DST date */
            `$INSTANCE_NAME`_DSTDateConversion();
        }
    }


    /*******************************************************************************
    * Function Name:   `$INSTANCE_NAME`_WriteDSTOffset
    ********************************************************************************
    *
    * Summary:
    *  Writes the DST Offset register. Allows a configurable incrementor decrement
    *  of time between 0 and 255 minutes. Increment occures on DST Start and
    *  decrement on DST Stop. Only generated if DST enabled.
    *
    * Parameters:
    *  offset: The DST offset to be saved.
    *
    * Return:
    *  None.
    *
	* Global variables:
	*  `$INSTANCE_NAME`_dstOffsetMin: global variable is modified with the new
    *  DST offset value (in minutes).
	*
    * Reentrant:
    *  No.
    *
    *******************************************************************************/
    void `$INSTANCE_NAME`_WriteDSTOffset(uint8 offset)
    {
        /* Set DST offset */
        `$INSTANCE_NAME`_dstOffsetMin = offset;
    }

#endif /* 1u == `$INSTANCE_NAME`_DST_FUNC_ENABLE */


/* [] END OF FILE */
