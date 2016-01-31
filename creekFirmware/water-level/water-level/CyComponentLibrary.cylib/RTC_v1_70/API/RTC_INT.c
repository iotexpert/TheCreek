/*******************************************************************************
* File Name: `$INSTANCE_NAME`_INT.c
* Version `$CY_MAJOR_VERSION`.`$CY_MINOR_VERSION`
*
* Description:
*  This file contains the Interrupt Service Routine (ISR) for the RTC component.
*  This interrupt routine has entry pointes to overflow on time or date.
*
********************************************************************************
* Copyright 2008-2012, Cypress Semiconductor Corporation.  All rights reserved.
* You may use this file only in accordance with the license, terms, conditions,
* disclaimers, and limitations in the end user license agreement accompanying
* the software package with which this file was provided.
*******************************************************************************/

#include "`$INSTANCE_NAME`.h"


/*******************************************************************************
*  Place your includes, defines and code here
*******************************************************************************/
/* `#START RTC_ISR_DEFINITION` */

/* `#END` */


/*******************************************************************************
* Function Name:   `$INSTANCE_NAME`_EverySecondHandler
********************************************************************************
*
* Summary:
*  This function is called every second.
*
* Parameters:
*  None.
*
* Return:
*  None.
*
*******************************************************************************/
void `$INSTANCE_NAME`_EverySecondHandler(void)
{
    /*  Place your every second handler code here. */
    /* `#START EVERY_SECOND_HANDLER_CODE` */

    /* `#END` */
}


/*******************************************************************************
* Function Name:   `$INSTANCE_NAME`_EveryMinuteHandler
********************************************************************************
*
* Summary:
*  This function is called every minute.
*
* Parameters:
*  None.
*
* Return:
*  None.
*
*******************************************************************************/
void `$INSTANCE_NAME`_EveryMinuteHandler(void)
{
    /*  Place your every minute handler code here. */
    /* `#START EVERY_MINUTE_HANDLER_CODE` */

    /* `#END` */
}


/*******************************************************************************
* Function Name:   `$INSTANCE_NAME`_EveryHourHandler
********************************************************************************
*
* Summary:
*  This function is called every hour.
*
* Parameters:
*  None.
*
* Return:
*  None.
*
*******************************************************************************/
void `$INSTANCE_NAME`_EveryHourHandler(void)
{
    /*  Place your every hour handler code here. */
    /* `#START EVERY_HOUR_HANDLER_CODE` */

    /* `#END` */
}


/*******************************************************************************
* Function Name:   `$INSTANCE_NAME`_EveryDayHandler
********************************************************************************
*
* Summary:
*  This function is called every day.
*
* Parameters:
*  None.
*
* Return:
*  None.
*
*******************************************************************************/
void `$INSTANCE_NAME`_EveryDayHandler(void)
{
    /*  Place your every day handler code here. */
    /* `#START EVERY_DAY_HANDLER_CODE` */

    /* `#END` */
}


/*******************************************************************************
* Function Name:   `$INSTANCE_NAME`_EveryWeekHandler
********************************************************************************
*
* Summary:
*  This function is called every week.
*
* Parameters:
*  None.
*
* Return:
*  None.
*
*******************************************************************************/
void `$INSTANCE_NAME`_EveryWeekHandler(void)
{
    /*  Place your every week handler code here. */
    /* `#START EVERY_WEEK_HANDLER_CODE` */

    /* `#END` */
}


/*******************************************************************************
* Function Name:   `$INSTANCE_NAME`_EveryMonthHandler
********************************************************************************
*
* Summary:
*  This function is called every month.
*
* Parameters:
*  None.
*
* Return:
*  None.
*
*******************************************************************************/
void `$INSTANCE_NAME`_EveryMonthHandler(void)
{
    /*  Place your every month handler code here. */
    /* `#START EVERY_MONTH_HANDLER_CODE` */

    /* `#END` */
}


/*******************************************************************************
* Function Name:   `$INSTANCE_NAME`_EveryYearHandler
********************************************************************************
*
* Summary:
*  This function is called every year.
*
* Parameters:
*  None.
*
* Return:
*  None.
*
*******************************************************************************/
void `$INSTANCE_NAME`_EveryYearHandler(void)
{
    /*  Place your every year handler code here. */
    /* `#START EVERY_YEAR_HANDLER_CODE` */

    /* `#END` */
}


/*******************************************************************************
* Function Name: `$INSTANCE_NAME`_ISR
********************************************************************************
*
* Summary:
*  This ISR is executed on 1PPS (one pulse per second) event.
*  Global interrupt must be enabled to invoke this ISR.
*  This interrupt trigs every second.
*
* Parameters:
*  None.
*
* Return:
*  None.
*
* Global variables:
* `$INSTANCE_NAME`_currentTimeDate, `$INSTANCE_NAME`_dstTimeDateStart, 
*  `$INSTANCE_NAME`_dstTimeDateStop, `$INSTANCE_NAME`_dstTimeDateStart,
*  `$INSTANCE_NAME`_alarmCfgTimeDate, `$INSTANCE_NAME`_statusDateTime,
*  `$INSTANCE_NAME`_dstStartStatus, `$INSTANCE_NAME`_dstStartStatus,
*  `$INSTANCE_NAME`_dstStopStatus, `$INSTANCE_NAME`_alarmCurStatus: global
*  variables are used for the time/date, DST and alarm update procedure.
*
*  `$INSTANCE_NAME`_dstTimeDateStart and `$INSTANCE_NAME`_currentTimeDate:
*  are modified with the updated values.
*
*  `$INSTANCE_NAME`_statusDateTime, `$INSTANCE_NAME`_dstStartStatus, 
*  `$INSTANCE_NAME`_dstStartStatus, `$INSTANCE_NAME`_dstStopStatus, 
*  `$INSTANCE_NAME`_alarmCurStatus: global variables could be modified while
*  current time/date is updated.
*
* Side Effects:
*  Clears all interrupt status bits (react_int, limact_int, onepps_int, ctw_int,
*  and  ftw_int) in Power Manager Interrupt Status Register. If an interrupt
*  gets generated at the same time as a clear, the bit will remain set (which
*  causes another interrupt).
*
*******************************************************************************/
CY_ISR(`$INSTANCE_NAME`_ISR)
{
    /* Clear OPPS interrupt status flag */
    (void) CyPmReadStatus(CY_PM_ONEPPS_INT);

    /* Increment seconds counter */
    `$INSTANCE_NAME`_currentTimeDate.Sec++;

    /* Check if minute elapsed */
    if(`$INSTANCE_NAME`_currentTimeDate.Sec > `$INSTANCE_NAME`_MINUTE_ELAPSED)
    {
        /* Inc Min */
        `$INSTANCE_NAME`_currentTimeDate.Min++;

        /* Clear Sec */
        `$INSTANCE_NAME`_currentTimeDate.Sec = 0u;

        if(`$INSTANCE_NAME`_currentTimeDate.Min > `$INSTANCE_NAME`_HOUR_ELAPSED)
        {
            /* Inc HOUR */
            `$INSTANCE_NAME`_currentTimeDate.Hour++;

            /* Clear Min */
            `$INSTANCE_NAME`_currentTimeDate.Min = 0u;

            /* Day roll over */
            if(`$INSTANCE_NAME`_currentTimeDate.Hour > `$INSTANCE_NAME`_DAY_ELAPSED)
            {
                /* Inc DayOfMonth */
                `$INSTANCE_NAME`_currentTimeDate.DayOfMonth++;

                /* Clear Hour */
                `$INSTANCE_NAME`_currentTimeDate.Hour = 0u;

                /* Inc DayOfYear */
                `$INSTANCE_NAME`_currentTimeDate.DayOfYear++;

                /* Inc DayOfWeek */
                `$INSTANCE_NAME`_currentTimeDate.DayOfWeek++;

                /* Check DayOfWeek */
                if(`$INSTANCE_NAME`_currentTimeDate.DayOfWeek > `$INSTANCE_NAME`_WEEK_ELAPSED)
                {
                    /* start new week */
                    `$INSTANCE_NAME`_currentTimeDate.DayOfWeek = 1u;
                }
                
                /* Day of month roll over.
                * Check if day of month greater than 29 in February of leap year or
                * if day of month greater than 28 NOT in February of NON leap year or
                * if day of month greater than it should be in every month in non leap year
                */
                if((((0u != (`$INSTANCE_NAME`_statusDateTime & `$INSTANCE_NAME`_STATUS_LY)) && \
                    (`$INSTANCE_NAME`_currentTimeDate.Month == `$INSTANCE_NAME`_FEBRUARY)) && \
                   (`$INSTANCE_NAME`_currentTimeDate.DayOfMonth > \
                                    `$INSTANCE_NAME`_daysInMonths[`$INSTANCE_NAME`_currentTimeDate.Month-1] + 1)) || \
                   (((0u != (`$INSTANCE_NAME`_statusDateTime & `$INSTANCE_NAME`_STATUS_LY)) && \
                    (!(`$INSTANCE_NAME`_currentTimeDate.Month == `$INSTANCE_NAME`_FEBRUARY))) && \
                    (`$INSTANCE_NAME`_currentTimeDate.DayOfMonth > \
                                    `$INSTANCE_NAME`_daysInMonths[`$INSTANCE_NAME`_currentTimeDate.Month-1])) || \
                   ((!(0u != (`$INSTANCE_NAME`_statusDateTime & `$INSTANCE_NAME`_STATUS_LY))) && \
                   (`$INSTANCE_NAME`_currentTimeDate.DayOfMonth > \
                                    `$INSTANCE_NAME`_daysInMonths[`$INSTANCE_NAME`_currentTimeDate.Month-1])))
                {                
                    /* Inc Month */
                    `$INSTANCE_NAME`_currentTimeDate.Month++;

                    /* Set first day of month */
                    `$INSTANCE_NAME`_currentTimeDate.DayOfMonth = 1u;

                    /* Year roll over */
                    if(`$INSTANCE_NAME`_currentTimeDate.Month > `$INSTANCE_NAME`_YEAR_ELAPSED)
                    {
                        /* Inc Year */
                        `$INSTANCE_NAME`_currentTimeDate.Year++;

                        /* Set first month of year */
                        `$INSTANCE_NAME`_currentTimeDate.Month = 1u;

                        /* Set first day of year */
                        `$INSTANCE_NAME`_currentTimeDate.DayOfYear = 1u;

                        /* Is this year leap */
                        if(1u == `$INSTANCE_NAME`_LEAP_YEAR(`$INSTANCE_NAME`_currentTimeDate.Year))
                        {
                            /* Set leap year flag */
                            `$INSTANCE_NAME`_statusDateTime |= `$INSTANCE_NAME`_STATUS_LY;
                        }
                        else    /* not leap year */
                        {
                            /* Clear leap year */
                            `$INSTANCE_NAME`_statusDateTime &= ~`$INSTANCE_NAME`_STATUS_LY;
                        }

                        /* Alarm YEAR */
                        if(`$INSTANCE_NAME`_IS_BIT_SET(`$INSTANCE_NAME`_alarmCfgMask, `$INSTANCE_NAME`_ALARM_YEAR_MASK))
                        {
                            /* Years match */
                            if(`$INSTANCE_NAME`_alarmCfgTimeDate.Year == `$INSTANCE_NAME`_currentTimeDate.Year)
                            {
                                /* Rise year alarm */
                                `$INSTANCE_NAME`_alarmCurStatus |= `$INSTANCE_NAME`_ALARM_YEAR_MASK;
                            }
                            else    /* Years do not match */
                            {
                                /* Clear year alarm */
                                `$INSTANCE_NAME`_alarmCurStatus &= ~`$INSTANCE_NAME`_ALARM_YEAR_MASK;
                            }
                        } /* do not alarm year */

                        /* Set Alarm flag event */
                        `$INSTANCE_NAME`_SET_ALARM(`$INSTANCE_NAME`_alarmCfgMask,   \
                                                   `$INSTANCE_NAME`_alarmCurStatus, \
                                                   `$INSTANCE_NAME`_statusDateTime);

                        /* Every Year */
                        if(`$INSTANCE_NAME`_IS_BIT_SET(`$INSTANCE_NAME`_intervalCfgMask, \
                                                                                `$INSTANCE_NAME`_INTERVAL_YEAR_MASK))
                        {
                            `$INSTANCE_NAME`_EveryYearHandler();
                        }

                    } /* Month > 12 */

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
                    }   /* Month alarm is masked */

                    #if(1u == `$INSTANCE_NAME`_DST_FUNC_ENABLE)
                        if(`$INSTANCE_NAME`_dstTimeDateStop.Month == `$INSTANCE_NAME`_currentTimeDate.Month)
                        {
                            `$INSTANCE_NAME`_dstStopStatus |= `$INSTANCE_NAME`_DST_MONTH;
                        }
                        else
                        {
                            `$INSTANCE_NAME`_dstStopStatus &= ~`$INSTANCE_NAME`_DST_MONTH;
                        }

                        if(`$INSTANCE_NAME`_dstTimeDateStart.Month == `$INSTANCE_NAME`_currentTimeDate.Month)
                        {
                            `$INSTANCE_NAME`_dstStartStatus |= `$INSTANCE_NAME`_DST_MONTH;
                        }
                        else
                        {
                            `$INSTANCE_NAME`_dstStartStatus &= ~`$INSTANCE_NAME`_DST_MONTH;
                        }
                    #endif /* 1u == `$INSTANCE_NAME`_DST_FUNC_ENABLE */

                    /* Set Alarm flag event */
                    `$INSTANCE_NAME`_SET_ALARM(`$INSTANCE_NAME`_alarmCfgMask,   \
                                               `$INSTANCE_NAME`_alarmCurStatus, \
                                               `$INSTANCE_NAME`_statusDateTime);
                               
                    /* Every Month */
                    if(`$INSTANCE_NAME`_IS_BIT_SET(`$INSTANCE_NAME`_intervalCfgMask, \
                                                   `$INSTANCE_NAME`_INTERVAL_MONTH_MASK))
                    {
                        `$INSTANCE_NAME`_EveryMonthHandler();
                    }
                }   /* Day roll over */

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
                }   /* Day of week alarm is masked */

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
                }   /* Day of year alarm is masked */

                /* Alarm DAYOFMONTH */
                if(`$INSTANCE_NAME`_IS_BIT_SET(`$INSTANCE_NAME`_alarmCfgMask, `$INSTANCE_NAME`_ALARM_DAYOFMONTH_MASK))
                {
                    if(`$INSTANCE_NAME`_alarmCfgTimeDate.DayOfMonth == `$INSTANCE_NAME`_currentTimeDate.DayOfMonth)
                    {
                        /* Set day of month alarm */
                        `$INSTANCE_NAME`_alarmCurStatus |= `$INSTANCE_NAME`_ALARM_DAYOFMONTH_MASK;
                    }
                       else
                    {
                        /* Clear day of month alarm */
                        `$INSTANCE_NAME`_alarmCurStatus &= ~`$INSTANCE_NAME`_ALARM_DAYOFMONTH_MASK;
                    }
                }   /* Day of month alarm is masked */

                #if(1u == `$INSTANCE_NAME`_DST_FUNC_ENABLE)
                    if (`$INSTANCE_NAME`_dstTimeDateStop.DayOfMonth == `$INSTANCE_NAME`_currentTimeDate.DayOfMonth)
                    {
                        `$INSTANCE_NAME`_dstStopStatus |= `$INSTANCE_NAME`_DST_DAYOFMONTH;
                    }
                    else
                    {
                        `$INSTANCE_NAME`_dstStopStatus &= ~`$INSTANCE_NAME`_DST_DAYOFMONTH;
                    }

                    if(`$INSTANCE_NAME`_dstTimeDateStart.DayOfMonth == `$INSTANCE_NAME`_currentTimeDate.DayOfMonth)
                    {
                        `$INSTANCE_NAME`_dstStartStatus |= `$INSTANCE_NAME`_DST_DAYOFMONTH;
                    }
                    else
                    {
                        `$INSTANCE_NAME`_dstStartStatus &= ~`$INSTANCE_NAME`_DST_DAYOFMONTH;
                    }
                #endif /* 1u == `$INSTANCE_NAME`_DST_FUNC_ENABLE */

                /* Set Alarm flag event */
                `$INSTANCE_NAME`_SET_ALARM(`$INSTANCE_NAME`_alarmCfgMask,   \
                                           `$INSTANCE_NAME`_alarmCurStatus, \
                                           `$INSTANCE_NAME`_statusDateTime);
                           
                /* Every Day */
                if(`$INSTANCE_NAME`_IS_BIT_SET(`$INSTANCE_NAME`_intervalCfgMask, `$INSTANCE_NAME`_INTERVAL_DAY_MASK))
                {
                    `$INSTANCE_NAME`_EveryDayHandler();
                }

                if(1u == `$INSTANCE_NAME`_currentTimeDate.DayOfWeek)
                {
                    /* Every Week */
                    if(`$INSTANCE_NAME`_IS_BIT_SET(`$INSTANCE_NAME`_intervalCfgMask, \
                                                                                `$INSTANCE_NAME`_INTERVAL_WEEK_MASK))
                    {
                       `$INSTANCE_NAME`_EveryWeekHandler();
                    }
                }

            } /* End of day roll over */

            /* Status set PM/AM flag */
            if(`$INSTANCE_NAME`_currentTimeDate.Hour < `$INSTANCE_NAME`_HALF_OF_DAY_ELAPSED)
            {
                /* AM Hour 00:00-11:59, flag zero */
                `$INSTANCE_NAME`_statusDateTime &= ~`$INSTANCE_NAME`_STATUS_AM_PM;
            }
            else
            {
                /* PM Hour 12:00-23:59, flag set */
                `$INSTANCE_NAME`_statusDateTime |= `$INSTANCE_NAME`_STATUS_AM_PM;
            }

            #if(1u == `$INSTANCE_NAME`_DST_FUNC_ENABLE)
                if(`$INSTANCE_NAME`_dstTimeDateStop.Hour == `$INSTANCE_NAME`_currentTimeDate.Hour)
                {
                    `$INSTANCE_NAME`_dstStopStatus |= `$INSTANCE_NAME`_DST_HOUR;
                }
                else
                {
                    `$INSTANCE_NAME`_dstStopStatus &= ~`$INSTANCE_NAME`_DST_HOUR;
                }

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
                    if(`$INSTANCE_NAME`_IS_BIT_SET(`$INSTANCE_NAME`_statusDateTime, `$INSTANCE_NAME`_STATUS_DST))
                    {
                        if(`$INSTANCE_NAME`_IS_BIT_SET(`$INSTANCE_NAME`_dstStopStatus, `$INSTANCE_NAME`_DST_HOUR    | \
                                                                                    `$INSTANCE_NAME`_DST_DAYOFMONTH | \
                                                                                    `$INSTANCE_NAME`_DST_MONTH))
                        {
                            /* Substruct from current value of minutes, number of minutes
                            * in DST offset which is out of complete hour
                            */
                            `$INSTANCE_NAME`_currentTimeDate.Min -= \
                                                `$INSTANCE_NAME`_dstOffsetMin % (`$INSTANCE_NAME`_HOUR_ELAPSED + 1u);

                            /* Is minute value negative? */
                            if(`$INSTANCE_NAME`_currentTimeDate.Min > `$INSTANCE_NAME`_HOUR_ELAPSED)
                            {
                                /* Convert to the positive. 
                                * HOUR_ELAPSED -     (~currentTimeDate.Min    ) == 
                                * HOUR_ELAPSED + 1 - (~currentTimeDate.Min + 1)
                                */
                                `$INSTANCE_NAME`_currentTimeDate.Min = \
                                                `$INSTANCE_NAME`_HOUR_ELAPSED - (~`$INSTANCE_NAME`_currentTimeDate.Min);

                                `$INSTANCE_NAME`_currentTimeDate.Hour--;
                            }

                            `$INSTANCE_NAME`_currentTimeDate.Hour -= \
                                                `$INSTANCE_NAME`_dstOffsetMin / (`$INSTANCE_NAME`_HOUR_ELAPSED + 1u);

                            /* Day roll over
                            * Is hour value negative? */
                            if(`$INSTANCE_NAME`_currentTimeDate.Hour > `$INSTANCE_NAME`_DAY_ELAPSED)
                            {
                                /* Convert to the positive. 
                                * DAY_ELAPSED - (~currentTimeDate.Hour) == 
                                * DAY_ELAPSED + 1 - (~currentTimeDate.Hour + 1)
                                */
                                `$INSTANCE_NAME`_currentTimeDate.Hour = \
                                                `$INSTANCE_NAME`_DAY_ELAPSED - (~`$INSTANCE_NAME`_currentTimeDate.Hour);

                                /* Status set PM/AM flag */
                                if(`$INSTANCE_NAME`_currentTimeDate.Hour < `$INSTANCE_NAME`_HALF_OF_DAY_ELAPSED)
                                {
                                    /* AM Hour 00:00-11:59, flag zero */
                                    `$INSTANCE_NAME`_statusDateTime &= ~`$INSTANCE_NAME`_STATUS_AM_PM;
                                }
                                else
                                {
                                    /* PM Hour 12:00-23:59, flag set */
                                    `$INSTANCE_NAME`_statusDateTime |= `$INSTANCE_NAME`_STATUS_AM_PM;
                                }

                                `$INSTANCE_NAME`_currentTimeDate.DayOfMonth--;
                                `$INSTANCE_NAME`_currentTimeDate.DayOfYear--;
                                `$INSTANCE_NAME`_currentTimeDate.DayOfWeek--;

                                if(0u == `$INSTANCE_NAME`_currentTimeDate.DayOfWeek)
                                {
                                    `$INSTANCE_NAME`_currentTimeDate.DayOfWeek = `$INSTANCE_NAME`_DAYS_IN_WEEK;
                                }

                                if(0u == `$INSTANCE_NAME`_currentTimeDate.DayOfMonth)
                                {
                                    `$INSTANCE_NAME`_currentTimeDate.Month--;
                                    if(0u == `$INSTANCE_NAME`_currentTimeDate.Month)
                                    {
                                        `$INSTANCE_NAME`_currentTimeDate.Month = `$INSTANCE_NAME`_DECEMBER;

                                        `$INSTANCE_NAME`_currentTimeDate.DayOfMonth = \
                                            `$INSTANCE_NAME`_daysInMonths[`$INSTANCE_NAME`_currentTimeDate.Month - 1u];

                                        `$INSTANCE_NAME`_currentTimeDate.Year--;

                                        if(1u == `$INSTANCE_NAME`_LEAP_YEAR(`$INSTANCE_NAME`_currentTimeDate.Year))
                                        {
                                            /* LP - true, else - false */
                                            `$INSTANCE_NAME`_statusDateTime |= `$INSTANCE_NAME`_STATUS_LY;
                                            `$INSTANCE_NAME`_currentTimeDate.DayOfYear = \
                                                                                    `$INSTANCE_NAME`_DAYS_IN_LEAP_YEAR;
                                        }
                                        else
                                        {
                                            `$INSTANCE_NAME`_statusDateTime &= ~`$INSTANCE_NAME`_STATUS_LY;
                                            `$INSTANCE_NAME`_currentTimeDate.DayOfYear = `$INSTANCE_NAME`_DAYS_IN_YEAR;
                                        }
                                        `$INSTANCE_NAME`_EveryYearHandler();
                                    }
                                    else
                                    {
                                        `$INSTANCE_NAME`_currentTimeDate.DayOfMonth = \
                                            `$INSTANCE_NAME`_daysInMonths[`$INSTANCE_NAME`_currentTimeDate.Month - 1u];
                                    }
                                    `$INSTANCE_NAME`_EveryMonthHandler();
                                }
                                `$INSTANCE_NAME`_EveryDayHandler();
                            }
                            `$INSTANCE_NAME`_statusDateTime &= ~`$INSTANCE_NAME`_STATUS_DST;
                            `$INSTANCE_NAME`_dstStopStatus = 0u;
                        }
                    }
                    else
                    {
                        if(`$INSTANCE_NAME`_IS_BIT_SET(`$INSTANCE_NAME`_dstStartStatus, \
                                                      (`$INSTANCE_NAME`_DST_HOUR | `$INSTANCE_NAME`_DST_DAYOFMONTH | \
                                                       `$INSTANCE_NAME`_DST_MONTH)))
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

                                /* Status set PM/AM flag */
                                if(`$INSTANCE_NAME`_currentTimeDate.Hour < `$INSTANCE_NAME`_HALF_OF_DAY_ELAPSED)
                                {
                                    /* AM Hour 00:00-11:59, flag zero */
                                    `$INSTANCE_NAME`_statusDateTime &= ~`$INSTANCE_NAME`_STATUS_AM_PM;
                                }
                                else
                                {
                                    /* PM Hour 12:00-23:59, flag set */
                                    `$INSTANCE_NAME`_statusDateTime |= `$INSTANCE_NAME`_STATUS_AM_PM;
                                }

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
                                    if(`$INSTANCE_NAME`_currentTimeDate.Month > `$INSTANCE_NAME`_YEAR_ELAPSED)
                                    {
                                        `$INSTANCE_NAME`_currentTimeDate.Month = `$INSTANCE_NAME`_JANUARY;
                                        `$INSTANCE_NAME`_currentTimeDate.Year++;
                                        
                                        if(1u == `$INSTANCE_NAME`_LEAP_YEAR(`$INSTANCE_NAME`_currentTimeDate.Year))
                                        {
                                            /* LP - true, else - false */
                                            `$INSTANCE_NAME`_statusDateTime |= `$INSTANCE_NAME`_STATUS_LY;
                                        }
                                        else
                                        {
                                            `$INSTANCE_NAME`_statusDateTime &= ~`$INSTANCE_NAME`_STATUS_LY;
                                        }
                                        `$INSTANCE_NAME`_currentTimeDate.DayOfYear = 1u;

                                        `$INSTANCE_NAME`_EveryYearHandler();
                                    }
                                    `$INSTANCE_NAME`_EveryMonthHandler();
                                }
                                `$INSTANCE_NAME`_EveryDayHandler();
                            }
                            `$INSTANCE_NAME`_statusDateTime |= `$INSTANCE_NAME`_STATUS_DST;
                            `$INSTANCE_NAME`_dstStartStatus = 0u;

                            /* Month */
                            if(`$INSTANCE_NAME`_dstTimeDateStop.Month == `$INSTANCE_NAME`_currentTimeDate.Month)
                            {
                                `$INSTANCE_NAME`_dstStopStatus |= `$INSTANCE_NAME`_DST_MONTH;
                            }
                            else
                            {
                                `$INSTANCE_NAME`_dstStopStatus &= ~`$INSTANCE_NAME`_DST_MONTH;
                            }

                            /* DayOfMonth */
                            if(`$INSTANCE_NAME`_dstTimeDateStop.DayOfMonth == \
                                                                            `$INSTANCE_NAME`_currentTimeDate.DayOfMonth)
                            {
                                `$INSTANCE_NAME`_dstStopStatus |= `$INSTANCE_NAME`_DST_DAYOFMONTH;
                            }
                            else
                            {
                                `$INSTANCE_NAME`_dstStopStatus &= ~`$INSTANCE_NAME`_DST_DAYOFMONTH;
                            }
                        }
                    }

                    /* Alarm DAYOFWEEK */
                    if(`$INSTANCE_NAME`_IS_BIT_SET(`$INSTANCE_NAME`_alarmCfgMask,`$INSTANCE_NAME`_ALARM_DAYOFWEEK_MASK))
                    {
                        if(`$INSTANCE_NAME`_alarmCfgTimeDate.DayOfWeek == `$INSTANCE_NAME`_currentTimeDate.DayOfWeek)
                        {
                            `$INSTANCE_NAME`_alarmCurStatus |= `$INSTANCE_NAME`_ALARM_DAYOFWEEK_MASK;
                        }
                        else
                        {
                            `$INSTANCE_NAME`_alarmCurStatus &= ~`$INSTANCE_NAME`_ALARM_DAYOFWEEK_MASK;
                        }
                    }

                    /* Alarm DAYOFYEAR */
                    if(`$INSTANCE_NAME`_IS_BIT_SET(`$INSTANCE_NAME`_alarmCfgMask,`$INSTANCE_NAME`_ALARM_DAYOFYEAR_MASK))
                    {
                        if(`$INSTANCE_NAME`_alarmCfgTimeDate.DayOfYear == `$INSTANCE_NAME`_currentTimeDate.DayOfYear)
                        {
                            `$INSTANCE_NAME`_alarmCurStatus |= `$INSTANCE_NAME`_ALARM_DAYOFYEAR_MASK;
                        }
                        else
                        {
                            `$INSTANCE_NAME`_alarmCurStatus &= ~`$INSTANCE_NAME`_ALARM_DAYOFYEAR_MASK;
                        }
                    }

                    /* Alarm DAYOFMONTH */
                    if(`$INSTANCE_NAME`_IS_BIT_SET(`$INSTANCE_NAME`_alarmCfgMask, \
                                                   `$INSTANCE_NAME`_ALARM_DAYOFMONTH_MASK))
                    {
                        if(`$INSTANCE_NAME`_alarmCfgTimeDate.DayOfMonth == `$INSTANCE_NAME`_currentTimeDate.DayOfMonth)
                        {
                            `$INSTANCE_NAME`_alarmCurStatus |= `$INSTANCE_NAME`_ALARM_DAYOFMONTH_MASK;
                        }
                        else
                        {
                            `$INSTANCE_NAME`_alarmCurStatus &= ~`$INSTANCE_NAME`_ALARM_DAYOFMONTH_MASK;
                        }
                    }

                    /* Alarm MONTH */
                    if(`$INSTANCE_NAME`_IS_BIT_SET(`$INSTANCE_NAME`_alarmCfgMask,`$INSTANCE_NAME`_ALARM_MONTH_MASK))
                    {
                        if(`$INSTANCE_NAME`_alarmCfgTimeDate.Month == `$INSTANCE_NAME`_currentTimeDate.Month)
                        {
                            `$INSTANCE_NAME`_alarmCurStatus |= `$INSTANCE_NAME`_ALARM_MONTH_MASK;
                        }
                        else
                        {
                            `$INSTANCE_NAME`_alarmCurStatus &= ~`$INSTANCE_NAME`_ALARM_MONTH_MASK;
                        }
                    }

                    /* Alarm YEAR */
                    if(`$INSTANCE_NAME`_IS_BIT_SET(`$INSTANCE_NAME`_alarmCfgMask, `$INSTANCE_NAME`_ALARM_YEAR_MASK))
                    {
                        if(`$INSTANCE_NAME`_alarmCfgTimeDate.Year == `$INSTANCE_NAME`_currentTimeDate.Year)
                        {
                            `$INSTANCE_NAME`_alarmCurStatus |= `$INSTANCE_NAME`_ALARM_YEAR_MASK;
                        }
                        else
                        {
                            `$INSTANCE_NAME`_alarmCurStatus &= ~`$INSTANCE_NAME`_ALARM_YEAR_MASK;
                        }
                    }
                    
                    /* Set Alarm flag event */
                    `$INSTANCE_NAME`_SET_ALARM(`$INSTANCE_NAME`_alarmCfgMask,   \
                                               `$INSTANCE_NAME`_alarmCurStatus, \
                                               `$INSTANCE_NAME`_statusDateTime);
                }
            #endif /* 1u == `$INSTANCE_NAME`_DST_FUNC_ENABLE */

            /* Alarm HOUR */
            if(`$INSTANCE_NAME`_IS_BIT_SET(`$INSTANCE_NAME`_alarmCfgMask ,`$INSTANCE_NAME`_ALARM_HOUR_MASK))
            {
                if(`$INSTANCE_NAME`_alarmCfgTimeDate.Hour == `$INSTANCE_NAME`_currentTimeDate.Hour)
                {
                    `$INSTANCE_NAME`_alarmCurStatus |= `$INSTANCE_NAME`_ALARM_HOUR_MASK;
                }
                else
                {
                    `$INSTANCE_NAME`_alarmCurStatus &= ~`$INSTANCE_NAME`_ALARM_HOUR_MASK;
                }
            }
            
            /* Set Alarm flag event */
            `$INSTANCE_NAME`_SET_ALARM(`$INSTANCE_NAME`_alarmCfgMask,   \
                                       `$INSTANCE_NAME`_alarmCurStatus, \
                                       `$INSTANCE_NAME`_statusDateTime);
            
            /* Every Hour */
            if(`$INSTANCE_NAME`_IS_BIT_SET(`$INSTANCE_NAME`_intervalCfgMask,`$INSTANCE_NAME`_INTERVAL_HOUR_MASK))
            {
                `$INSTANCE_NAME`_EveryHourHandler();
            }
        } /* Min > 59 = Hour */

        /* Alarm MIN */
        if(`$INSTANCE_NAME`_IS_BIT_SET(`$INSTANCE_NAME`_alarmCfgMask, `$INSTANCE_NAME`_ALARM_MIN_MASK))
        {
            if(`$INSTANCE_NAME`_alarmCfgTimeDate.Min == `$INSTANCE_NAME`_currentTimeDate.Min)
            {
                `$INSTANCE_NAME`_alarmCurStatus |= `$INSTANCE_NAME`_ALARM_MIN_MASK;
            }
            else
            {
                `$INSTANCE_NAME`_alarmCurStatus &= ~`$INSTANCE_NAME`_ALARM_MIN_MASK;
            }
        }

        /* Set Alarm flag event */
        `$INSTANCE_NAME`_SET_ALARM(`$INSTANCE_NAME`_alarmCfgMask,   \
                                   `$INSTANCE_NAME`_alarmCurStatus, \
                                   `$INSTANCE_NAME`_statusDateTime);
        
        /* Every Min */
        if(`$INSTANCE_NAME`_IS_BIT_SET(`$INSTANCE_NAME`_intervalCfgMask, `$INSTANCE_NAME`_INTERVAL_MIN_MASK))
        {
            `$INSTANCE_NAME`_EveryMinuteHandler();
        }
    } /* Sec */

    /* Alarm SEC */
    if(`$INSTANCE_NAME`_IS_BIT_SET(`$INSTANCE_NAME`_alarmCfgMask, `$INSTANCE_NAME`_ALARM_SEC_MASK))
    {
        if(`$INSTANCE_NAME`_alarmCfgTimeDate.Sec == `$INSTANCE_NAME`_currentTimeDate.Sec)
        {
            `$INSTANCE_NAME`_alarmCurStatus |= `$INSTANCE_NAME`_ALARM_SEC_MASK;
        }
        else
        {
            `$INSTANCE_NAME`_alarmCurStatus &= ~`$INSTANCE_NAME`_ALARM_SEC_MASK;
        }
    }

    /* Set Alarm flag event */
    `$INSTANCE_NAME`_SET_ALARM(`$INSTANCE_NAME`_alarmCfgMask,   \
                               `$INSTANCE_NAME`_alarmCurStatus, \
                               `$INSTANCE_NAME`_statusDateTime);
   
    /* Execute every second handler if needed */
    if(`$INSTANCE_NAME`_IS_BIT_SET(`$INSTANCE_NAME`_intervalCfgMask,`$INSTANCE_NAME`_INTERVAL_SEC_MASK))
    {
        `$INSTANCE_NAME`_EverySecondHandler();
    }

}


/* [] END OF FILE */
