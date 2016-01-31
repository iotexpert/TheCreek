/*******************************************************************************
* File Name: `$INSTANCE_NAME`.c 
* Version `$CY_MAJOR_VERSION`.`$CY_MINOR_VERSION`
*
* Description:
*     This file provides the source code to the API for the RTC Component.
*
* Note:
*
********************************************************************************
* Copyright 2008-2009, Cypress Semiconductor Corporation.  All rights reserved.
* You may use this file only in accordance with the license, terms, conditions, 
* disclaimers, and limitations in the end user license agreement accompanying 
* the software package with which this file was provided.
********************************************************************************/


#include "`$INSTANCE_NAME`.h"
#include "CyLib.h"


/*******************************************************************************
*                            Software Registers
********************************************************************************/

/* Time and date software registers */
`$INSTANCE_NAME`_TimeDate `$INSTANCE_NAME`_CurTimeDate = {0};

/***************************************
*        Alarm Registers
***************************************/

/* Alarm time and date software registers */
`$INSTANCE_NAME`_TimeDate `$INSTANCE_NAME`_AlarmTimeDate = {0};

#if (`$INSTANCE_NAME`_DST_FUNC_ENABLE == 1) /* */
    /***************************************
    *        DST Registers
    ***************************************/
    
    /* Define DST format: '0' - fixed, '1' - relative */
    uint8   `$INSTANCE_NAME`_DstMode = 0;
    
    /* Hour 0-24, DayOfWeek 0-6, Week 1-5, DayOfMonth 1-31, Month 1-12  */
    `$INSTANCE_NAME`_Dst `$INSTANCE_NAME`_DstStartTimeDate = {0};
    `$INSTANCE_NAME`_Dst `$INSTANCE_NAME`_DstStopTimeDate = {0};     
    
    /* Number of Hours to add/dec to time */
    uint8   `$INSTANCE_NAME`_DstOffset= 0;    
    uint8   `$INSTANCE_NAME`_DstStatusStart = 0;    
    uint8   `$INSTANCE_NAME`_DstStatusStop = 0;
#endif /* `$INSTANCE_NAME`_DST_FUNC_ENABLE == 1*/

/***************************************
*        Mask Registers
***************************************/

uint8   `$INSTANCE_NAME`_AlarmMask = 0;
uint8   `$INSTANCE_NAME`_AlarmStatus = 0;
uint8   `$INSTANCE_NAME`_IntervalMask = 0;

/***************************************
*      Status & Control Registers
***************************************/

uint8   `$INSTANCE_NAME`_Status = 0;

/***************************************
*        Month Day Array
***************************************/

const uint8 `$INSTANCE_NAME`_Dim[12] = {
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

 const uint8 `$INSTANCE_NAME`_Seq[12] = {0, 3, 2, 5, 0, 3, 5, 1, 4, 6, 2, 4};

 
/*******************************************************************************
* FUNCTION NAME: uint8 `$INSTANCE_NAME`_IsLeapYear(uint16 year)
********************************************************************************
* Summary: 
*  This function detemines if year is leap year.
*
* Parameters: 
*    (uint16) year: Year value.
*
* Return:
*  (uint8) 1 - Leap Year, 0 - No Leap Year.
*
* Theory:
*  See summary
*
* Side Effects:
*  None
*
*******************************************************************************/
uint8 `$INSTANCE_NAME`_IsLeapYear(uint16 year)
{
    /* A year will be a leap year if it is divisible by 4 but not by 100. 
      If a year is divisible by 4 and by 100, it is not a leap year unless 
      it is also divisible by 400. */
    return ( (!(year%400) || (!(year%4) && (year%100))) ? 0x01u : 0x00u);    
}


/*******************************************************************************
* FUNCTION NAME: uint8 `$INSTANCE_NAME`_DayOfWeek(uint8 dayofmonth, uint8 month, uint16 year)
********************************************************************************
* Summary: 
*  This function calculate Day Of Week value use Zeller's congruence.
*
* Parameters: 
*  (uint8) dayofmonth: Day Of Month value;
*    (uint8) month: Month value;
*    (uint16) year: Year value.
* Return:
*  (uint8) Day Of Week value.
*
* Theory:
*  See summary
*
* Side Effects:
*  None
*
*******************************************************************************/
uint8 `$INSTANCE_NAME`_DayOfWeek(uint8 dayofmonth, uint8 month, uint16 year)
{
    uint8 day;
    
    /* This is Zeller's congruence for calculations day of week, so all constants put 
    accordingly it */
    if (month < `$INSTANCE_NAME`_MARCH)
        year = year - 1;
        
    day = (year + year/4 - year/100 + year/400 + `$INSTANCE_NAME`_Seq[month-1] + dayofmonth) % `$INSTANCE_NAME`_DAYS_IN_WEEK;
    
    return day;
}


#if (`$INSTANCE_NAME`_DST_FUNC_ENABLE == 1)
    /*******************************************************************************
    * FUNCTION NAME: void `$INSTANCE_NAME`_DSTDateConversion(void)
    ********************************************************************************
    * Summary:
    *  This function convert relative to absolute date.
    *
    * Parameters: 
    *  None
    *
    * Return:
    *  None
    *
    * Theory:
    *  See summary
    *
    * Side Effects:
    *  None
    *
    *******************************************************************************/
    void `$INSTANCE_NAME`_DSTDateConversion(void)
    {
        uint8 week = 1 ,day = 1, dayofweek;
        
        /*Start*/
        dayofweek = `$INSTANCE_NAME`_DayOfWeek(day, `$INSTANCE_NAME`_DstStartTimeDate.Month, `$INSTANCE_NAME`_CurTimeDate.Year) + 1;
        if (dayofweek > `$INSTANCE_NAME`_START_OF_WEEK)
        {
            dayofweek-= `$INSTANCE_NAME`_START_OF_WEEK;
        }
        else
        {
            dayofweek = `$INSTANCE_NAME`_DAYS_IN_WEEK - (`$INSTANCE_NAME`_START_OF_WEEK - dayofweek);
        }
        
        while(dayofweek != `$INSTANCE_NAME`_DstStartTimeDate.DayOfWeek)
        {
            day++;        
            dayofweek++;
            if (dayofweek > `$INSTANCE_NAME`_WEEK_ELAPSED)
            {
                dayofweek = 1;
                week++;
            }
        }
    
        while(week != `$INSTANCE_NAME`_DstStartTimeDate.Week)
        {
            day += `$INSTANCE_NAME`_DAYS_IN_WEEK;
            week++;
        }
        `$INSTANCE_NAME`_DstStartTimeDate.DayOfMonth = day; 
        
        /* Stop */
        week = 1; day = 1; 
        dayofweek = `$INSTANCE_NAME`_DayOfWeek(day, `$INSTANCE_NAME`_DstStopTimeDate.Month, `$INSTANCE_NAME`_CurTimeDate.Year) + 1;
        if (dayofweek > `$INSTANCE_NAME`_START_OF_WEEK)
        {
            dayofweek-= `$INSTANCE_NAME`_START_OF_WEEK;
        }
        else
        {
            dayofweek = `$INSTANCE_NAME`_DAYS_IN_WEEK - (`$INSTANCE_NAME`_START_OF_WEEK - dayofweek);
        }
        
        while(dayofweek != `$INSTANCE_NAME`_DstStopTimeDate.DayOfWeek)
        {
            day++;        
            dayofweek++;
            if (dayofweek > `$INSTANCE_NAME`_WEEK_ELAPSED)
            {
                dayofweek = 1;
                week++;
            }
        }
    
        while(week != `$INSTANCE_NAME`_DstStopTimeDate.Week)
        {
            day += `$INSTANCE_NAME`_DAYS_IN_WEEK;
            week++;
        }
        `$INSTANCE_NAME`_DstStopTimeDate.DayOfMonth = day; 
        
    }
#endif /* `$INSTANCE_NAME`_DST_FUNC_ENABLE == 1*/


/*******************************************************************************
* FUNCTION NAME: void `$INSTANCE_NAME`_SetInitValues(void)
********************************************************************************
* Summary:
*    This function does all required calculation.
*    - Set LP Year flag;
*    - Set AM/PM flag;
*    - DayOfWeek;
*    - DayOfYear; 
*    - Set DST flag;
*      - Convert relative to absolute date.
*
* Parameters: 
*    None
*
* Return:
*    None
*
* Theory:
*  See summary
*
* Side Effects:
*  None
*
*******************************************************************************/
void `$INSTANCE_NAME`_SetInitValues(void)
{
    uint8 i;
    
    `$INSTANCE_NAME`_CurTimeDate.DayOfYear = 0;

    `$INSTANCE_NAME`_CurTimeDate.DayOfYear += `$INSTANCE_NAME`_CurTimeDate.DayOfMonth;

    /* Set LP Year flag */
    if(`$INSTANCE_NAME`_IsLeapYear(`$INSTANCE_NAME`_CurTimeDate.Year))    
    {            
        `$INSTANCE_NAME`_Status |= `$INSTANCE_NAME`_STATUS_LY;
    }
    else
    {
        `$INSTANCE_NAME`_Status &= ~`$INSTANCE_NAME`_STATUS_LY;
    }
    
    /* DayOfYear */
    for(i = 0; i < `$INSTANCE_NAME`_CurTimeDate.Month-1; i++)
    {
        `$INSTANCE_NAME`_CurTimeDate.DayOfYear += `$INSTANCE_NAME`_Dim[i];
    }
    
    if(`$INSTANCE_NAME`_Status & `$INSTANCE_NAME`_STATUS_LY)    
    {
        if (!(`$INSTANCE_NAME`_CurTimeDate.DayOfMonth <= `$INSTANCE_NAME`_DAYS_IN_FEBRUARY+1 && `$INSTANCE_NAME`_CurTimeDate.Month <= `$INSTANCE_NAME`_FEBRUARY ))
            `$INSTANCE_NAME`_CurTimeDate.DayOfYear++;
    }

    /* DayOfWeek */
    `$INSTANCE_NAME`_CurTimeDate.DayOfWeek = `$INSTANCE_NAME`_DayOfWeek(`$INSTANCE_NAME`_CurTimeDate.DayOfMonth, `$INSTANCE_NAME`_CurTimeDate.Month, `$INSTANCE_NAME`_CurTimeDate.Year) + 1;
    if (`$INSTANCE_NAME`_CurTimeDate.DayOfWeek > `$INSTANCE_NAME`_START_OF_WEEK)
    {
        `$INSTANCE_NAME`_CurTimeDate.DayOfWeek -= `$INSTANCE_NAME`_START_OF_WEEK;
    }
    else
    {
        `$INSTANCE_NAME`_CurTimeDate.DayOfWeek = `$INSTANCE_NAME`_DAYS_IN_WEEK - (`$INSTANCE_NAME`_START_OF_WEEK - `$INSTANCE_NAME`_CurTimeDate.DayOfWeek );
    }
        
    #if (`$INSTANCE_NAME`_DST_FUNC_ENABLE == 1)
        /* DST flag calculation */
        if (`$INSTANCE_NAME`_DstMode & `$INSTANCE_NAME`_DST_RELDATE)
        {
            `$INSTANCE_NAME`_DSTDateConversion();
        }    
            
        if (`$INSTANCE_NAME`_CurTimeDate.Month > `$INSTANCE_NAME`_DstStartTimeDate.Month)
        {
            `$INSTANCE_NAME`_Status |= `$INSTANCE_NAME`_STATUS_DST;
        }
        else if (`$INSTANCE_NAME`_CurTimeDate.Month == `$INSTANCE_NAME`_DstStartTimeDate.Month)
        {
            if (`$INSTANCE_NAME`_CurTimeDate.DayOfMonth > `$INSTANCE_NAME`_DstStartTimeDate.DayOfMonth)
            {
                `$INSTANCE_NAME`_Status |= `$INSTANCE_NAME`_STATUS_DST;
            }
            else if (`$INSTANCE_NAME`_CurTimeDate.DayOfMonth == `$INSTANCE_NAME`_DstStartTimeDate.DayOfMonth)
            {
                if (`$INSTANCE_NAME`_CurTimeDate.Hour > `$INSTANCE_NAME`_DstStartTimeDate.Hour)
                {
                    `$INSTANCE_NAME`_Status |= `$INSTANCE_NAME`_STATUS_DST;
                }
            }    
        }
            
        if (`$INSTANCE_NAME`_CurTimeDate.Month > `$INSTANCE_NAME`_DstStopTimeDate.Month)
        {
            `$INSTANCE_NAME`_Status &= ~`$INSTANCE_NAME`_STATUS_DST;
        }
        else if (`$INSTANCE_NAME`_CurTimeDate.Month == `$INSTANCE_NAME`_DstStopTimeDate.Month)
        {
            if (`$INSTANCE_NAME`_CurTimeDate.DayOfMonth > `$INSTANCE_NAME`_DstStopTimeDate.DayOfMonth)
            {
                `$INSTANCE_NAME`_Status &= ~`$INSTANCE_NAME`_STATUS_DST;
            }
            else if (`$INSTANCE_NAME`_CurTimeDate.DayOfMonth == `$INSTANCE_NAME`_DstStopTimeDate.DayOfMonth)
            {
                if (`$INSTANCE_NAME`_CurTimeDate.Hour > `$INSTANCE_NAME`_DstStopTimeDate.Hour)
                {
                    `$INSTANCE_NAME`_Status &= ~`$INSTANCE_NAME`_STATUS_DST;
                }
            }    
        }                                
        
        `$INSTANCE_NAME`_DstStatusStart = 0;    
        `$INSTANCE_NAME`_DstStatusStop = 0;
    
        /* Month */
        if(`$INSTANCE_NAME`_DstStopTimeDate.Month == `$INSTANCE_NAME`_CurTimeDate.Month)
        {
            `$INSTANCE_NAME`_DstStatusStop |= `$INSTANCE_NAME`_DST_MONTH;
        }
        else
        {
            `$INSTANCE_NAME`_DstStatusStop &= ~`$INSTANCE_NAME`_DST_MONTH;
        }
        
        if(`$INSTANCE_NAME`_DstStartTimeDate.Month == `$INSTANCE_NAME`_CurTimeDate.Month)
        {
            `$INSTANCE_NAME`_DstStatusStart |= `$INSTANCE_NAME`_DST_MONTH;
        }
        else
        {
            `$INSTANCE_NAME`_DstStatusStart &= ~`$INSTANCE_NAME`_DST_MONTH;
        }
        
        /*DayOfMonth*/
        if (`$INSTANCE_NAME`_DstStopTimeDate.DayOfMonth == `$INSTANCE_NAME`_CurTimeDate.DayOfMonth)
        { 
            `$INSTANCE_NAME`_DstStatusStop |= `$INSTANCE_NAME`_DST_DAYOFMONTH;
        }
        else
        {
            `$INSTANCE_NAME`_DstStatusStop &= ~`$INSTANCE_NAME`_DST_DAYOFMONTH;
        }
    
        if (`$INSTANCE_NAME`_DstStartTimeDate.DayOfMonth == `$INSTANCE_NAME`_CurTimeDate.DayOfMonth)
        { 
            `$INSTANCE_NAME`_DstStatusStart |= `$INSTANCE_NAME`_DST_DAYOFMONTH;
        }
        else
        {
            `$INSTANCE_NAME`_DstStatusStart &= ~`$INSTANCE_NAME`_DST_DAYOFMONTH;
        }
    
        /* Hour */   
        if (`$INSTANCE_NAME`_DstStopTimeDate.Hour == `$INSTANCE_NAME`_CurTimeDate.Hour)
        {
            `$INSTANCE_NAME`_DstStatusStop |= `$INSTANCE_NAME`_DST_HOUR;
        }
        else
        {
            `$INSTANCE_NAME`_DstStatusStop &= ~`$INSTANCE_NAME`_DST_HOUR;
        }
        
        if (`$INSTANCE_NAME`_DstStartTimeDate.Hour == `$INSTANCE_NAME`_CurTimeDate.Hour)
        {
            `$INSTANCE_NAME`_DstStatusStart |= `$INSTANCE_NAME`_DST_HOUR;
        }
        else
        {
            `$INSTANCE_NAME`_DstStatusStart &= ~`$INSTANCE_NAME`_DST_HOUR;
        }
        
        /* DST Enable ? */
        if (`$INSTANCE_NAME`_DstMode & `$INSTANCE_NAME`_DST_ENABLE)
        {
            if(`$INSTANCE_NAME`_Status & `$INSTANCE_NAME`_STATUS_DST)
            {
                if ((`$INSTANCE_NAME`_DstStatusStop & `$INSTANCE_NAME`_DST_HOUR) && (`$INSTANCE_NAME`_DstStatusStop & `$INSTANCE_NAME`_DST_DAYOFMONTH) && (`$INSTANCE_NAME`_DstStatusStop & `$INSTANCE_NAME`_DST_MONTH))
                {        
                    /* Dec Hour and Min */
                    `$INSTANCE_NAME`_CurTimeDate.Min -= `$INSTANCE_NAME`_DstOffset % (`$INSTANCE_NAME`_HOUR_ELAPSED + 1);
                    if (`$INSTANCE_NAME`_CurTimeDate.Min > `$INSTANCE_NAME`_HOUR_ELAPSED)
                    {
                        /* Adjust Min */
                        `$INSTANCE_NAME`_CurTimeDate.Min = `$INSTANCE_NAME`_HOUR_ELAPSED - (~`$INSTANCE_NAME`_CurTimeDate.Min);
                        `$INSTANCE_NAME`_CurTimeDate.Hour--;
                    }
                
                    `$INSTANCE_NAME`_CurTimeDate.Hour -= `$INSTANCE_NAME`_DstOffset / (`$INSTANCE_NAME`_HOUR_ELAPSED + 1);
                    if (`$INSTANCE_NAME`_CurTimeDate.Hour > `$INSTANCE_NAME`_DAY_ELAPSED)
                    {
                        /* Adjust Hour, DEC DOM */
                        `$INSTANCE_NAME`_CurTimeDate.Hour = `$INSTANCE_NAME`_DAY_ELAPSED - (~`$INSTANCE_NAME`_CurTimeDate.Hour);
                        `$INSTANCE_NAME`_CurTimeDate.DayOfMonth--;
                        `$INSTANCE_NAME`_CurTimeDate.DayOfYear--;    
                        `$INSTANCE_NAME`_CurTimeDate.DayOfWeek--;
                
                        if(`$INSTANCE_NAME`_CurTimeDate.DayOfWeek == 0)
                        {
                            `$INSTANCE_NAME`_CurTimeDate.DayOfWeek = `$INSTANCE_NAME`_DAYS_IN_WEEK;
                        }
                        
                        if (`$INSTANCE_NAME`_CurTimeDate.DayOfMonth == 0)
                        {
                            `$INSTANCE_NAME`_CurTimeDate.Month--;
                            if(`$INSTANCE_NAME`_CurTimeDate.Month == 0)
                            {
                                `$INSTANCE_NAME`_CurTimeDate.Month = `$INSTANCE_NAME`_DECEMBER;
                                `$INSTANCE_NAME`_CurTimeDate.DayOfMonth = `$INSTANCE_NAME`_Dim[`$INSTANCE_NAME`_CurTimeDate.Month-1];
                                `$INSTANCE_NAME`_CurTimeDate.Year--;
                                if(`$INSTANCE_NAME`_IsLeapYear(`$INSTANCE_NAME`_CurTimeDate.Year))  
                                { 
                                    /* LP - true, else - false */
                                    `$INSTANCE_NAME`_Status |= `$INSTANCE_NAME`_STATUS_LY;
                                    `$INSTANCE_NAME`_CurTimeDate.DayOfYear = 366;
                                }
                                else
                                {
                                    `$INSTANCE_NAME`_Status &= ~`$INSTANCE_NAME`_STATUS_LY;
                                    `$INSTANCE_NAME`_CurTimeDate.DayOfYear = 365;
                                }
                            }
                            else
                            {
                                `$INSTANCE_NAME`_CurTimeDate.DayOfMonth = `$INSTANCE_NAME`_Dim[`$INSTANCE_NAME`_CurTimeDate.Month-1];
                            }
                        }
                    }
                    `$INSTANCE_NAME`_Status &= ~`$INSTANCE_NAME`_STATUS_DST;
                    `$INSTANCE_NAME`_DstStatusStop = 0;            
                }
            }
            else
            {
                if ((`$INSTANCE_NAME`_DstStatusStart & `$INSTANCE_NAME`_DST_HOUR) && (`$INSTANCE_NAME`_DstStatusStart & `$INSTANCE_NAME`_DST_DAYOFMONTH) && (`$INSTANCE_NAME`_DstStatusStart & `$INSTANCE_NAME`_DST_MONTH))
                {
                    /* Add Hour and Min */
                    `$INSTANCE_NAME`_CurTimeDate.Min += `$INSTANCE_NAME`_DstOffset % (`$INSTANCE_NAME`_HOUR_ELAPSED + 1);
                    if (`$INSTANCE_NAME`_CurTimeDate.Min > `$INSTANCE_NAME`_HOUR_ELAPSED)
                    {
                        /* Adjust Min */
                        `$INSTANCE_NAME`_CurTimeDate.Min -= (`$INSTANCE_NAME`_HOUR_ELAPSED + 1);
                        `$INSTANCE_NAME`_CurTimeDate.Hour++;
                    }
                
                    `$INSTANCE_NAME`_CurTimeDate.Hour += `$INSTANCE_NAME`_DstOffset / (`$INSTANCE_NAME`_HOUR_ELAPSED + 1);
                    if (`$INSTANCE_NAME`_CurTimeDate.Hour > `$INSTANCE_NAME`_DAY_ELAPSED)
                    {
                        /* Adjust hour, add day */
                        `$INSTANCE_NAME`_CurTimeDate.Hour -= (`$INSTANCE_NAME`_DAY_ELAPSED + 1);
                        `$INSTANCE_NAME`_CurTimeDate.DayOfMonth++;
                        `$INSTANCE_NAME`_CurTimeDate.DayOfYear++;
                        `$INSTANCE_NAME`_CurTimeDate.DayOfWeek++;
                        
                        if(`$INSTANCE_NAME`_CurTimeDate.DayOfWeek > `$INSTANCE_NAME`_WEEK_ELAPSED)
                        {
                            `$INSTANCE_NAME`_CurTimeDate.DayOfWeek = 1;
                        }
                        
                        if (`$INSTANCE_NAME`_CurTimeDate.DayOfMonth > `$INSTANCE_NAME`_Dim[`$INSTANCE_NAME`_CurTimeDate.Month-1])
                        {
                            `$INSTANCE_NAME`_CurTimeDate.Month++;
                            `$INSTANCE_NAME`_CurTimeDate.DayOfMonth = 1;
                            if(`$INSTANCE_NAME`_CurTimeDate.Month > `$INSTANCE_NAME`_YEAR_ELAPSED)
                            {
                                `$INSTANCE_NAME`_CurTimeDate.Month = `$INSTANCE_NAME`_JANUARY;
                                `$INSTANCE_NAME`_CurTimeDate.Year++;
                                if(`$INSTANCE_NAME`_IsLeapYear(`$INSTANCE_NAME`_CurTimeDate.Year))  
                                { 
                                    /* LP - true, else - false */
                                    `$INSTANCE_NAME`_Status |= `$INSTANCE_NAME`_STATUS_LY;
                                }
                                else
                                {
                                    `$INSTANCE_NAME`_Status &= ~`$INSTANCE_NAME`_STATUS_LY;
                                }
                                `$INSTANCE_NAME`_CurTimeDate.DayOfYear = 1;
                            }
                        }
                    }
                    `$INSTANCE_NAME`_Status |= `$INSTANCE_NAME`_STATUS_DST;
                    `$INSTANCE_NAME`_DstStatusStart = 0;    
                }
            }
        }
    #endif /* `$INSTANCE_NAME`_DST_FUNC_ENABLE == 1*/

    /* Set AM/PM flag */
    if (`$INSTANCE_NAME`_CurTimeDate.Hour < `$INSTANCE_NAME`_HALF_OF_DAY_ELAPSED)
    {
        /* AM Hour 00:00-11:59, flag zero */
        `$INSTANCE_NAME`_Status &= ~`$INSTANCE_NAME`_STATUS_AM_PM;
    }
    else
    {
        /* PM Hour 12:00 - 23:59, flag set */
        `$INSTANCE_NAME`_Status |= `$INSTANCE_NAME`_STATUS_AM_PM;
    }
    
    /* Alarm calculation */

    /* Alarm SEC */
    if (`$INSTANCE_NAME`_AlarmMask & `$INSTANCE_NAME`_ALARM_SEC_MASK)
    {
        if (`$INSTANCE_NAME`_AlarmTimeDate.Sec == `$INSTANCE_NAME`_CurTimeDate.Sec)
        {
            `$INSTANCE_NAME`_AlarmStatus |= `$INSTANCE_NAME`_ALARM_SEC_MASK;
        }
        else
        {
            `$INSTANCE_NAME`_AlarmStatus &= ~`$INSTANCE_NAME`_ALARM_SEC_MASK;   
        }    
    } 

    /* Alarm MIN */
    if (`$INSTANCE_NAME`_AlarmMask & `$INSTANCE_NAME`_ALARM_MIN_MASK)
    {
        if (`$INSTANCE_NAME`_AlarmTimeDate.Min == `$INSTANCE_NAME`_CurTimeDate.Min)
        {
            `$INSTANCE_NAME`_AlarmStatus |= `$INSTANCE_NAME`_ALARM_MIN_MASK;
        }
        else
        {
            `$INSTANCE_NAME`_AlarmStatus &= ~`$INSTANCE_NAME`_ALARM_MIN_MASK;
        }
    }

    /* Alarm HOUR */
    if (`$INSTANCE_NAME`_AlarmMask & `$INSTANCE_NAME`_ALARM_HOUR_MASK)
    {
        if (`$INSTANCE_NAME`_AlarmTimeDate.Hour == `$INSTANCE_NAME`_CurTimeDate.Hour)
        {
            `$INSTANCE_NAME`_AlarmStatus |= `$INSTANCE_NAME`_ALARM_HOUR_MASK;
        }
        else
        {
            `$INSTANCE_NAME`_AlarmStatus &= ~`$INSTANCE_NAME`_ALARM_HOUR_MASK;
        }
    }

    /* Alarm DAYOFWEEK */
    if (`$INSTANCE_NAME`_AlarmMask & `$INSTANCE_NAME`_ALARM_DAYOFWEEK_MASK)
    {
        if(`$INSTANCE_NAME`_AlarmTimeDate.DayOfWeek == `$INSTANCE_NAME`_CurTimeDate.DayOfWeek)
        {
              `$INSTANCE_NAME`_AlarmStatus |= `$INSTANCE_NAME`_ALARM_DAYOFWEEK_MASK;
        }
        else
        {
              `$INSTANCE_NAME`_AlarmStatus &= ~`$INSTANCE_NAME`_ALARM_DAYOFWEEK_MASK;
        }
    }

    /* Alarm DAYOFYEAR */
    if( `$INSTANCE_NAME`_AlarmMask & `$INSTANCE_NAME`_ALARM_DAYOFYEAR_MASK )
    {                                        
        if(`$INSTANCE_NAME`_AlarmTimeDate.DayOfYear == `$INSTANCE_NAME`_CurTimeDate.DayOfYear)
        {
            `$INSTANCE_NAME`_AlarmStatus |= `$INSTANCE_NAME`_ALARM_DAYOFYEAR_MASK;  
        }
        else
        {
              `$INSTANCE_NAME`_AlarmStatus &= ~`$INSTANCE_NAME`_ALARM_DAYOFYEAR_MASK;
        }
    }
    
    /* Alarm DAYOFMONTH */
    if (`$INSTANCE_NAME`_AlarmMask & `$INSTANCE_NAME`_ALARM_DAYOFMONTH_MASK )  
    {
        if (`$INSTANCE_NAME`_AlarmTimeDate.DayOfMonth == `$INSTANCE_NAME`_CurTimeDate.DayOfMonth)
        { 
              `$INSTANCE_NAME`_AlarmStatus |= `$INSTANCE_NAME`_ALARM_DAYOFMONTH_MASK;
        }
           else
        {
              `$INSTANCE_NAME`_AlarmStatus &= ~`$INSTANCE_NAME`_ALARM_DAYOFMONTH_MASK;
        }
    }

    /* Alarm MONTH */
    if (`$INSTANCE_NAME`_AlarmMask & `$INSTANCE_NAME`_ALARM_MONTH_MASK )
    {
        if(`$INSTANCE_NAME`_AlarmTimeDate.Month == `$INSTANCE_NAME`_CurTimeDate.Month)
        {
            `$INSTANCE_NAME`_AlarmStatus |= `$INSTANCE_NAME`_ALARM_MONTH_MASK;
        }
        else
        {
            `$INSTANCE_NAME`_AlarmStatus &= ~`$INSTANCE_NAME`_ALARM_MONTH_MASK;
        }
    } 

    /* Alarm YEAR */
    if (`$INSTANCE_NAME`_AlarmMask & `$INSTANCE_NAME`_ALARM_YEAR_MASK)
    {
        if(`$INSTANCE_NAME`_AlarmTimeDate.Year == `$INSTANCE_NAME`_CurTimeDate.Year)
        {
            `$INSTANCE_NAME`_AlarmStatus |= `$INSTANCE_NAME`_ALARM_YEAR_MASK;
        }
        else
        {
            `$INSTANCE_NAME`_AlarmStatus &= ~`$INSTANCE_NAME`_ALARM_YEAR_MASK;
        }
    }

     /* Set Alarm flag event */
    if ((`$INSTANCE_NAME`_AlarmMask == (`$INSTANCE_NAME`_AlarmMask & `$INSTANCE_NAME`_AlarmStatus)) && `$INSTANCE_NAME`_AlarmMask )
    {
         `$INSTANCE_NAME`_Status |= `$INSTANCE_NAME`_STATUS_AA;
        `$INSTANCE_NAME`_AlarmStatus = 0;
    } 
}


/*******************************************************************************
* FUNCTION NAME: void `$INSTANCE_NAME`_Start(void)
********************************************************************************
* Summary:
*  This function enables  RTC component to opearation: configurate counter, 
*  setup interrupts, done all requered calculation and starts counter.
*
* Parameters:  
*  None
*
* Return:     
*  None
* 
*
* Theory:
*  See summary
*
* Side Effects:
*  None
*
*******************************************************************************/
void `$INSTANCE_NAME`_Start(void) 
{
    
    /* Start calculation of required date and flags */
    `$INSTANCE_NAME`_SetInitValues();
    
    /* Disable Interrupt. */
    CyIntDisable(`$INSTANCE_NAME`_ISR_NUMBER);

    /* Set the ISR to point to the RTC_SUT_isr Interrupt. */
    CyIntSetVector(`$INSTANCE_NAME`_ISR_NUMBER, `$INSTANCE_NAME`_ISR);

    /* Set the priority. */
    CyIntSetPriority(`$INSTANCE_NAME`_ISR_NUMBER, `$INSTANCE_NAME`_ISR_PRIORITY);

    /* Enable it. */
    CyIntEnable(`$INSTANCE_NAME`_ISR_NUMBER);
    
    /* Start one pulse per second interrupt. */
    `$INSTANCE_NAME`_OPPS_CFG |= `$INSTANCE_NAME`_OPPS_EN_MASK;
    `$INSTANCE_NAME`_OPPS_CFG |= `$INSTANCE_NAME`_OPPSIE_EN_MASK;    
}


/*******************************************************************************
* FUNCTION NAME: void `$INSTANCE_NAME`_Stop( void )
********************************************************************************
* Summary:
*  This function, stops RTC Component operation.
*
* Parameters:  
*  None
*
* Return:     
*  None
* 
*
* Theory:
*  See summary
*
* Side Effects:
*   None
*
*******************************************************************************/
void `$INSTANCE_NAME`_Stop(void)
{
    /* Disable the " + m_instanceName + " interrupt. */
    CyIntDisable(`$INSTANCE_NAME`_ISR_NUMBER);
    
    /* Stop one pulse per second interrupt. */
    `$INSTANCE_NAME`_OPPS_CFG &= ~`$INSTANCE_NAME`_OPPS_EN_MASK;
    `$INSTANCE_NAME`_OPPS_CFG &= ~`$INSTANCE_NAME`_OPPSIE_EN_MASK;      
}


/*******************************************************************************
* FUNCTION NAME: void `$INSTANCE_NAME`_EnableInt(void)
********************************************************************************
* Summary:
*  This fucntion enables interrupts of RTC Component.
*
* Parameters: 
*  None
*
* Return:    
*  None
*
* Theory:
*  See summary
*
* Side Effects:
*   None
*
*******************************************************************************/
void `$INSTANCE_NAME`_EnableInt(void)
{
    /* Enable the " + m_instanceName + " interrupt */
    CyIntEnable(`$INSTANCE_NAME`_ISR_NUMBER);
}


/*******************************************************************************
* FUNCTION NAME: void `$INSTANCE_NAME`_DisableInt(void)
********************************************************************************
* Summary:
*  This function disables interrupts of RTC Component, time and date stop running.
*
* Parameters: 
*  None
*
* Return:    
*  None
*
* Theory:
*  See summary
*
* Side Effects:
*   None
*
*******************************************************************************/
void `$INSTANCE_NAME`_DisableInt(void)
{
    /* Disable the " + m_instanceName + " interrupt. */
    CyIntDisable(`$INSTANCE_NAME`_ISR_NUMBER);
}


/*******************************************************************************
* FUNCTION NAME: `$INSTANCE_NAME`_TimeDate* `$INSTANCE_NAME`_ReadTime(void)
********************************************************************************
* Summary:
*  This funciton returns a pointer to the current time and date structure.
*
* Parameters: 
*  None
*
* Return:    
*  `$INSTANCE_NAME`_TimeDate* : Pointer to internal stuct where
*  current time and date are stored.
*
* Theory:
*  See summary
*
* Side Effects:
*  None 
*
*******************************************************************************/
`$INSTANCE_NAME`_TimeDate* `$INSTANCE_NAME`_ReadTime(void)
{
     return &`$INSTANCE_NAME`_CurTimeDate;
}


/*******************************************************************************
* FUNCTION NAME: void `$INSTANCE_NAME`_WriteTime(`$INSTANCE_NAME`_TimeDate *timedate)
********************************************************************************
* Summary:
*  This function writes time and date values as current time and date. Only 
*  passes Milliseconds(optionaly), Seconds, Minutes, Hours, Month, 
*  Day Of Month and Year.
*
* Parameters: 
*  (`$INSTANCE_NAME`_TimeDate *) timedate: Pointer to stuct of time and date values.
*
* Return:    
*  None
*
* Theory:
*  See summary
*
* Side Effects:
*  None
*
*******************************************************************************/
void `$INSTANCE_NAME`_WriteTime(`$INSTANCE_NAME`_TimeDate *timedate)
{
    /* Disable Interrupt of RTC Component */ 
    `$INSTANCE_NAME`_DisableInt();
    
    /* Write current time and date */
    `$INSTANCE_NAME`_CurTimeDate.Sec = timedate->Sec ;
    `$INSTANCE_NAME`_CurTimeDate.Min = timedate->Min ;
    `$INSTANCE_NAME`_CurTimeDate.Hour = timedate->Hour ;
    `$INSTANCE_NAME`_CurTimeDate.DayOfMonth = timedate->DayOfMonth ;
    `$INSTANCE_NAME`_CurTimeDate.Month = timedate->Month;
    `$INSTANCE_NAME`_CurTimeDate.Year = timedate->Year;
      
    /* Enable Interrupt of RTC Component */ 
      `$INSTANCE_NAME`_EnableInt();
}


/*******************************************************************************
* FUNCTION NAME: void `$INSTANCE_NAME`_WriteSecond(uint8 second)
********************************************************************************
* Summary:
*  This function writes Sec software register value.
*
* Parameters: 
*  (uint8) second: Seconds value. 
*
* Return:    
*  None
*
* Theory:
*  See summary
*
* Side Effects:
*  None
*
*******************************************************************************/
void `$INSTANCE_NAME`_WriteSecond(uint8 second)
{
    `$INSTANCE_NAME`_CurTimeDate.Sec = second;
}


/*******************************************************************************
* FUNCTION NAME:   void `$INSTANCE_NAME`_WriteMinute(uint8 minute)
*------------------------------------------------------------------------------
* Summary:
*  Write Minute value in minutes counter register.
*
* Parameters:
* (uint8) minute: Minutes value.
*
* Return:    
*  None
*
* Theory:
*  See summary
*
* Side Effects:
*  None
*
*******************************************************************************/
void `$INSTANCE_NAME`_WriteMinute(uint8 minute)
{
     `$INSTANCE_NAME`_CurTimeDate.Min = minute;   
}


/*******************************************************************************
* FUNCTION NAME: void `$INSTANCE_NAME`_WriteHour(uint8 hour)
********************************************************************************
* Summary:
*  This function writes Hour software register value.
*
* Parameters: 
*  (uint8) hour: Hours value.
*
* Return:    
*  None
*
* Theory:
*  See summary
*
* Side Effects:
*  None
*
*******************************************************************************/
void `$INSTANCE_NAME`_WriteHour(uint8 hour)
{    
     `$INSTANCE_NAME`_CurTimeDate.Hour = hour; 
}


/*******************************************************************************
* FUNCTION NAME: void `$INSTANCE_NAME`_WriteDayOfMonth(uint8 dayofmonth)
********************************************************************************
* Summary:
*  This function writes DayOfMonth software register value.
*
* Parameters:
*  (uint8) dayofmonth: Day Of Month value
*
* Return:    
*  None
*
* Theory:
*  See summary
*
* Side Effects:
*  None
*
*******************************************************************************/
void `$INSTANCE_NAME`_WriteDayOfMonth(uint8 dayofmonth)
{
    `$INSTANCE_NAME`_CurTimeDate.DayOfMonth = dayofmonth;
}


/*******************************************************************************
* FUNCTION NAME: void `$INSTANCE_NAME`_WriteMonth(uint8 month)
********************************************************************************
* Summary:
*  This function writes Month software register value.
*
* Parameters:
*  (uint8) month: Month value.
*
* Return:
*  None
*
* Theory:
*  See summary
*
* Side Effects:
*   None
*
*******************************************************************************/
void `$INSTANCE_NAME`_WriteMonth(uint8 month)
{
    `$INSTANCE_NAME`_CurTimeDate.Month = month;
}


/*******************************************************************************
* FUNCTION NAME: void `$INSTANCE_NAME`_WriteYear(uint16 year)
********************************************************************************
* Summary:
*   This function writes Year software register value.
*
* Parameters:
*  (uint16) year: Years value.
*
* Return:  
*   None
*
* Theory:
*  See summary
*
* Side Effects:
*  None
*
*******************************************************************************/
void `$INSTANCE_NAME`_WriteYear(uint16 year)
{
    `$INSTANCE_NAME`_CurTimeDate.Year = year;
}


/*******************************************************************************
* FUNCTION NAME: void `$INSTANCE_NAME`_WriteAlarmSecond(uint8 second)
********************************************************************************
* Summary:
*  This function writes Alarm Sec software register value.
*
* Parameters:
*  (uint8) second: Alarm Seconds value.
*
* Return: 
*  None
*
* Theory:
*  See summary
*
* Side Effects:
*  None
*
*******************************************************************************/
void `$INSTANCE_NAME`_WriteAlarmSecond(uint8 second)
{
    `$INSTANCE_NAME`_AlarmTimeDate.Sec = second;
  
    /* Alarm SEC */
    if (`$INSTANCE_NAME`_AlarmTimeDate.Sec == `$INSTANCE_NAME`_CurTimeDate.Sec)
    {
        `$INSTANCE_NAME`_AlarmStatus |= `$INSTANCE_NAME`_ALARM_SEC_MASK;
    }
    else
    {
        `$INSTANCE_NAME`_AlarmStatus &= ~`$INSTANCE_NAME`_ALARM_SEC_MASK;   
    }    
}


/*******************************************************************************
* FUNCTION NAME: void `$INSTANCE_NAME`_WriteAlarmMinute(uint8 minute);
********************************************************************************
* Summary:
*  This function writes Alarm Min software register value.
*
* Parameters: 
*  (uint8) minute: Alarm Minutes value.
*
* Return:  
*  None 
*
* Theory:
*  See summary
*
* Side Effects:
*  None
*
*******************************************************************************/
void `$INSTANCE_NAME`_WriteAlarmMinute(uint8 minute)
{
    `$INSTANCE_NAME`_AlarmTimeDate.Min = minute;
  
    /* Alarm MIN */
    if (`$INSTANCE_NAME`_AlarmTimeDate.Min == `$INSTANCE_NAME`_CurTimeDate.Min)
    {
        `$INSTANCE_NAME`_AlarmStatus |= `$INSTANCE_NAME`_ALARM_MIN_MASK;
    }
    else
    {
        `$INSTANCE_NAME`_AlarmStatus &= ~`$INSTANCE_NAME`_ALARM_MIN_MASK;   
    }    
}


/*******************************************************************************
* FUNCTION NAME: void `$INSTANCE_NAME`_WriteAlarmHour(uint8 hour)
********************************************************************************
* Summary:
*  This function writes Alarm Hour software register value.
*
* Parameters: 
*  (uint8) hour: Alarm Hours value.
*
* Return:
*  None
* 
* Theory:
*  See summary
*
* Side Effects:
*  None
*
*******************************************************************************/
void `$INSTANCE_NAME`_WriteAlarmHour(uint8 hour)
{
    `$INSTANCE_NAME`_AlarmTimeDate.Hour = hour; 
  
    /* Alarm HOUR */
    if (`$INSTANCE_NAME`_AlarmTimeDate.Hour == `$INSTANCE_NAME`_CurTimeDate.Hour)
    {
        `$INSTANCE_NAME`_AlarmStatus |= `$INSTANCE_NAME`_ALARM_HOUR_MASK;
    }
    else
    {
        `$INSTANCE_NAME`_AlarmStatus &= ~`$INSTANCE_NAME`_ALARM_HOUR_MASK;   
    }    
}


/*******************************************************************************
* FUNCTION NAME:   void `$INSTANCE_NAME`_WriteAlarmDayOfMonth(uint8 dayofmonth)
********************************************************************************
* Summary:
*  This function writes Alarm DayOfMonth software register value.
*
* Parameters: 
*  (uint8) dayofmonth: Alarm Day Of Month value.
*
* Return:
*  None
*
* Theory:
*  See summary
*
* Side Effects:
*  None
*
*******************************************************************************/
void `$INSTANCE_NAME`_WriteAlarmDayOfMonth(uint8 dayofmonth)
{
    `$INSTANCE_NAME`_AlarmTimeDate.DayOfMonth = dayofmonth; 
  
    /* Alarm DAYOFMONTH */
    if (`$INSTANCE_NAME`_AlarmTimeDate.DayOfMonth == `$INSTANCE_NAME`_CurTimeDate.DayOfMonth)
    {
        `$INSTANCE_NAME`_AlarmStatus |= `$INSTANCE_NAME`_ALARM_DAYOFMONTH_MASK;
    }
    else
    {
        `$INSTANCE_NAME`_AlarmStatus &= ~`$INSTANCE_NAME`_ALARM_DAYOFMONTH_MASK;   
    }    
}


/*******************************************************************************
* FUNCTION NAME: void `$INSTANCE_NAME`_WriteAlarmMonth(uint8 month)
********************************************************************************
* Summary:
*  This function writes Alarm Month software register value.
*
* Parameters:
*  (uint8) month: Alarm Months value.
*
* Return:  
*  None
*
* Theory:
*  See summary
*
* Side Effects:
*  None
*
*******************************************************************************/
void `$INSTANCE_NAME`_WriteAlarmMonth(uint8 month)
{
    `$INSTANCE_NAME`_AlarmTimeDate.Month = month; 
  
    /* Alarm MONTH */
    if (`$INSTANCE_NAME`_AlarmTimeDate.Month == `$INSTANCE_NAME`_CurTimeDate.Month)
    {
        `$INSTANCE_NAME`_AlarmStatus |= `$INSTANCE_NAME`_ALARM_MONTH_MASK;
    }
    else
    {
        `$INSTANCE_NAME`_AlarmStatus &= ~`$INSTANCE_NAME`_ALARM_MONTH_MASK;   
    }    
}


/*******************************************************************************
* FUNCTION NAME: void `$INSTANCE_NAME`_WriteAlarmYear(uint16 year)
********************************************************************************
* Summary:
*  This function writes Alarm Year software register value.
*
* Parameters: None
*  (uint16) year: Alarm  Years value.
*
* Return: 
*  None
*
* Theory:
*  See summary
*
* Side Effects:
*  None
*
*******************************************************************************/
void `$INSTANCE_NAME`_WriteAlarmYear(uint16 year)
{
   `$INSTANCE_NAME`_AlarmTimeDate.Year = year;
  
    /* Alarm YEAR */
    if (`$INSTANCE_NAME`_AlarmTimeDate.Year == `$INSTANCE_NAME`_CurTimeDate.Year)
    {
        `$INSTANCE_NAME`_AlarmStatus |= `$INSTANCE_NAME`_ALARM_YEAR_MASK;
    }
    else
    {
        `$INSTANCE_NAME`_AlarmStatus &= ~`$INSTANCE_NAME`_ALARM_YEAR_MASK;   
    }    
}


/*******************************************************************************
* FUNCTION NAME: void `$INSTANCE_NAME`_WriteAlarmDayOfWeek(uint8 dayofweek)
********************************************************************************
* Summary:
*   This function writes Alarm DayOfWeek software register value. 
*   Days values { Sun = 1, Mon = 2, Tue = 3, Wen = 4, Thu = 5, Fri = 6, Sut = 7 }
*
* Parameters: 
*  (uint8) dayofweek: Alarm Day Of Week value.
*
* Return:  
*  None 
*
* Theory:
*  See summary
*
* Side Effects:
*  None
*
*******************************************************************************/
void `$INSTANCE_NAME`_WriteAlarmDayOfWeek(uint8 dayofweek)
{
    `$INSTANCE_NAME`_AlarmTimeDate.DayOfWeek = dayofweek;
  
    /* Alarm DAYOFWEEK */
    if (`$INSTANCE_NAME`_AlarmTimeDate.DayOfWeek == `$INSTANCE_NAME`_CurTimeDate.DayOfWeek)
    {
        `$INSTANCE_NAME`_AlarmStatus |= `$INSTANCE_NAME`_ALARM_DAYOFWEEK_MASK;
    }
    else
    {
        `$INSTANCE_NAME`_AlarmStatus &= ~`$INSTANCE_NAME`_ALARM_DAYOFWEEK_MASK;   
    }    
}


/*******************************************************************************
* FUNCTION NAME: void `$INSTANCE_NAME`_WriteAlarmDayOfYear(uint16 dayofyear)
********************************************************************************
* Summary:
*  This function writes Alarm DayOfYear software register value.
*
* Parameters: 
*  (uint16) dayofyear: Alarm Day Of Year value.
*
* Return:
*  None
*
* Theory:
*  See summary
*
* Side Effects:
*  None
*
*******************************************************************************/
void `$INSTANCE_NAME`_WriteAlarmDayOfYear(uint16 dayofyear)
{
  `$INSTANCE_NAME`_AlarmTimeDate.DayOfYear = dayofyear;
  
    /* Alarm DAYOFYEAR */
    if (`$INSTANCE_NAME`_AlarmTimeDate.DayOfYear == `$INSTANCE_NAME`_CurTimeDate.DayOfYear)
    {
        `$INSTANCE_NAME`_AlarmStatus |= `$INSTANCE_NAME`_ALARM_DAYOFYEAR_MASK;
    }
    else
    {
        `$INSTANCE_NAME`_AlarmStatus &= ~`$INSTANCE_NAME`_ALARM_DAYOFYEAR_MASK;   
    }    
}


/*******************************************************************************
* FUNCTION NAME:   uint8 `$INSTANCE_NAME`_ReadSecond(void)
********************************************************************************
* Summary:
*  This function reads Sec software register value.
*
* Parameters: 
*  None
*
* Return:
*  (uint8) Seconds current value.
*
* Theory:
*  See summary
*
* Side Effects:
*   None
*
*******************************************************************************/
uint8 `$INSTANCE_NAME`_ReadSecond(void)
{
    return `$INSTANCE_NAME`_CurTimeDate.Sec;
}


/*******************************************************************************
* FUNCTION NAME:   uint8 `$INSTANCE_NAME`_ReadMinute(void)
********************************************************************************
* Summary:
*  This function reads Min software register value.
*
* Parameters:
*  None
*
* Return:  
*  (uint8) Minutes current value.
*
* Theory:
*  See summary
*
* Side Effects:
*  None
*
*******************************************************************************/
uint8 `$INSTANCE_NAME`_ReadMinute(void)
{
    return `$INSTANCE_NAME`_CurTimeDate.Min;
}


/*******************************************************************************
* FUNCTION NAME:   uint8 `$INSTANCE_NAME`_ReadHour(void)
********************************************************************************
* Summary:
*  This function reads Hour software register value.
*
* Parameters:
*  None
*
* Return:  
*  (uint8) Hours current value.
*
* Theory:
*  See summary
*
* Side Effects:
*  None
*
*******************************************************************************/
uint8 `$INSTANCE_NAME`_ReadHour(void)
{
    return `$INSTANCE_NAME`_CurTimeDate.Hour;
}


/*******************************************************************************
* FUNCTION NAME:   uint8 `$INSTANCE_NAME`_ReadDayOfMonth(void)
********************************************************************************
* Summary:
*  This function reads DayOfMonth software register value.
*
* Parameters: 
*  None
*
* Return:  
*  (uint8) Day Of Month current value.
*
* Theory:
*  See summary
*
* Side Effects:
*  None
*
*******************************************************************************/
uint8 `$INSTANCE_NAME`_ReadDayOfMonth(void)
{
    return `$INSTANCE_NAME`_CurTimeDate.DayOfMonth;
}


/*******************************************************************************
* FUNCTION NAME:   uint8 `$INSTANCE_NAME`_ReadMonth(void)
********************************************************************************
* Summary:
*  This function reads Month software register value. 
*
* Parameters:
*   None
*
* Return:
*  (uint8) Months current value.
*
* Theory:
*  See summary
*
* Side Effects:
*   None
*
*******************************************************************************/
uint8 `$INSTANCE_NAME`_ReadMonth(void)
{
    return `$INSTANCE_NAME`_CurTimeDate.Month; 
}

/*******************************************************************************
* FUNCTION NAME:  uint16 `$INSTANCE_NAME`_ReadYear(void)
********************************************************************************
* Summary:
*  This function reads Year software register value.
*
* Parameters: None
*   None
*
* Return:
*  (uint16) Years current value.
*
* Theory:
*  See summary
*
* Side Effects:
*   None
*
*******************************************************************************/
uint16 `$INSTANCE_NAME`_ReadYear(void)
{
    return `$INSTANCE_NAME`_CurTimeDate.Year;
}


/*******************************************************************************
* FUNCTION NAME:   uint8 `$INSTANCE_NAME`_ReadAlarmSecond(void)
********************************************************************************
* Summary:
*  This function reads Alarm Sec software register value. 
*
* Parameters:
*  None
*
* Return:  
*  (uint8) Alarm Sec software register value.
*
* Theory:
*  See summary
*
* Side Effects:
*  None
*
 *******************************************************************************/
uint8 `$INSTANCE_NAME`_ReadAlarmSecond(void)
{
    return `$INSTANCE_NAME`_AlarmTimeDate.Sec;
}


/*******************************************************************************
* FUNCTION NAME:   uint8 `$INSTANCE_NAME`_ReadAlarmMinute(void)
********************************************************************************
* Summary:
*  This function reads Alarm Min software register value.
*
* Parameters:
*  None
*
* Return: 
*  (uint8) Alarm Min software register value.
*
* Theory:
*  See summary
*
* Side Effects:
*  None
*
*******************************************************************************/
uint8 `$INSTANCE_NAME`_ReadAlarmMinute(void)
{
    return `$INSTANCE_NAME`_AlarmTimeDate.Min;
}


/*******************************************************************************
* FUNCTION NAME:   uint8 `$INSTANCE_NAME`_ReadAlarmHour(void)
********************************************************************************
* Summary:
*  This function reads Alarm Hour software register value.
*
* Parameters: 
*  None
*
* Return:  
*  (uint8) Alarm Hour software register value.
*
* Theory:
*  See summary
*
* Side Effects:
*  None
*
*******************************************************************************/
uint8 `$INSTANCE_NAME`_ReadAlarmHour(void)
{
    return `$INSTANCE_NAME`_AlarmTimeDate.Hour;
}


/*******************************************************************************
* FUNCTION NAME:   uint8 `$INSTANCE_NAME`_ReadAlarmDayOfMonth(void)
********************************************************************************
* Summary:
*  This function reads Alarm DayOfMonth software register value.
*
* Parameters: 
*  None
*
* Return:
*  (uint8) Alarm DayOfMonth software register value.
* 
* Theory:
*  See summary
*
* Side Effects:
*  None
*
*******************************************************************************/
uint8 `$INSTANCE_NAME`_ReadAlarmDayOfMonth(void)
{
    return `$INSTANCE_NAME`_AlarmTimeDate.DayOfMonth;
}


/*******************************************************************************
* FUNCTION NAME:   uint8 `$INSTANCE_NAME`_ReadAlarmMonth(void)
********************************************************************************
* Summary:
*  This function reads Alarm Month software register value.
*
* Parameters: 
*  None
*
* Return:
*  (uint8) Alarm Month software register value.
*
* Theory:
*  See summary
*
* Side Effects:
*  None
*
*******************************************************************************/
uint8 `$INSTANCE_NAME`_ReadAlarmMonth(void)
{
    return `$INSTANCE_NAME`_AlarmTimeDate.Month;
}


/*******************************************************************************
* FUNCTION NAME:   uint16 `$INSTANCE_NAME`_ReadAlarmYear(void)
********************************************************************************
* Summary:
*  This function reads Alarm Year software register value.
*
* Parameters:
*  None
*
* Return:  
*  (uint8) Alarm Year software register value.
*
* Theory:
*  See summary
*
* Side Effects:
*  None
*
*******************************************************************************/
uint16 `$INSTANCE_NAME`_ReadAlarmYear(void)
{
    return `$INSTANCE_NAME`_AlarmTimeDate.Year;
}


/*******************************************************************************
* FUNCTION NAME:   uint8 `$INSTANCE_NAME`_ReadAlarmDayOfWeek(void)
********************************************************************************
* Summary:
*  This function reads Alarm DayOfWeek software register value.
*
* Parameters:
*  None
*
* Return:
*  (uint8) Alarm DayOfWeek software register value.
*
* Theory:
*  See summary
*
* Side Effects:
*  None
*
*******************************************************************************/
uint8 `$INSTANCE_NAME`_ReadAlarmDayOfWeek(void)
{
    return `$INSTANCE_NAME`_AlarmTimeDate.DayOfWeek;  
}


/*******************************************************************************
* FUNCTION NAME:  uint16 `$INSTANCE_NAME`_ReadAlarmDayOfYear(void)
********************************************************************************
* Summary:
*  This function reads Alarm DayOfYear software register value.
*
* Parameters:
*  None
*
* Return:
*  (uint16) Alarm DayOfYear software register value.
*
* Theory:
*  See summary
*
* Side Effects:
*  None
*
*******************************************************************************/
uint16 `$INSTANCE_NAME`_ReadAlarmDayOfYear(void)
{
    return  `$INSTANCE_NAME`_AlarmTimeDate.DayOfYear;
}


/*******************************************************************************
* FUNCTION NAME: void `$INSTANCE_NAME`_WriteAlarmMask(uint8 mask)
********************************************************************************
* Summary:
*  This function writes the Alarm Mask software register with 1 bit per 
*  time/date entry. Alarm true when all masked time/date values match 
*  Alarm values. 
*
* Parameters:
*  (uint8) mask: Alarm Mask software register value.
*
* Return:  
*  None
*
* Theory:
*  See summary
*
* Side Effects:
*  None
*
*******************************************************************************/
void `$INSTANCE_NAME`_WriteAlarmMask(uint8 mask)
{
    `$INSTANCE_NAME`_AlarmMask = mask;
}


/*******************************************************************************
* FUNCTION NAME: void `$INSTANCE_NAME`_WriteIntervalMask(uint8 mask)
********************************************************************************
* Summary:
*  Writes the Interval Mask software register with 1 bit per time/date entry. 
*  Interrupt true when any masked time/date overflow occur. 
*
* Parameters:
*  (uint8) mask: Interval Mask software register value.
*
* Return: 
*  None
*
* Theory:
*  See summary
*
* Side Effects:
*  None
*
*******************************************************************************/
void `$INSTANCE_NAME`_WriteIntervalMask(uint8 mask)
{
    `$INSTANCE_NAME`_IntervalMask = mask;
}


/*******************************************************************************
* FUNCTION NAME: uint8 `$INSTANCE_NAME`_ReadStatus(void)
********************************************************************************
* Summary:
*  This function reads the Status software register which has flags for DST (DST), 
*  Leap Year (LY) and AM/PM (AM_PM), Alarm active (AA).
*
* Parameters: 
*  None
*
* Return:  
*  (uint8) Status software register value. 
*
* Theory:
*  See summary
*
* Side Effects:
*  None
*
*******************************************************************************/
uint8 `$INSTANCE_NAME`_ReadStatus(void)
{
    uint8 status = 0;
    
    status = (uint8)`$INSTANCE_NAME`_Status;
    `$INSTANCE_NAME`_Status &= ~`$INSTANCE_NAME`_STATUS_AA;  /* Clean AA flag after read of Status Register */
    
    return status;
}


#if (`$INSTANCE_NAME`_DST_FUNC_ENABLE == 1)
    /*******************************************************************************
    * FUNCTION NAME: void `$INSTANCE_NAME`_WriteDSTMode(uint8 mode)
    ********************************************************************************
    * Summary:
    *  This function writes the DST mode software register. That enables or disables DST changes and 
    *  sets the date mode to fixed date or relative date. Only generated if DST enabled.
    *
    * Parameters: 
    *  (uint8) mode: DST Mode software register value.
    *
    * Return:
    *  None
    *
    * Theory:
    *  See summary
    *
    * Side Effects:
    *  None
    *
    *******************************************************************************/
    void `$INSTANCE_NAME`_WriteDSTMode(uint8 mode)
    {
        `$INSTANCE_NAME`_DstMode = mode;
    }
    
    
    /*******************************************************************************
    * FUNCTION NAME: void `$INSTANCE_NAME`_WriteDSTStartHour(uint8 hour)
    ********************************************************************************
    * Summary:
    *  This function writes the DST Start Hour software register. 
    *  Used for absolute date entry. Only generated if DST enabled. 
    *
    * Parameters: 
    *  (uint8) hour: DST Start Hour software register value.
    *
    * Return:
    *  None
    *
    * Theory:
    *  See summary
    *
    * Side Effects:
    *  None
    *
    *******************************************************************************/
    void `$INSTANCE_NAME`_WriteDSTStartHour(uint8 hour)
    {
        `$INSTANCE_NAME`_DstStartTimeDate.Hour = hour;
    }
    
    
    /*******************************************************************************
    * FUNCTION NAME: void `$INSTANCE_NAME`_WriteDSTStartOfMonth(uint8 dayofmonth)
    ********************************************************************************
    * Summary:
    *  This function writes the DST Start DayOfMonth software register. Used for 
    *  absolute date entry. Only generated if DST enabled.
    *
    * Parameters: 
    *  (uint8) dayofmonth: DST Start DayOfMonth software register value.
    *
    * Return:  
    *  None 
    *
    * Theory:
    *  See summary
    *
    * Side Effects:
    *  None
    *
    *******************************************************************************/
    void `$INSTANCE_NAME`_WriteDSTStartDayOfMonth(uint8 dayofmonth)
    {
        `$INSTANCE_NAME`_DstStartTimeDate.DayOfMonth = dayofmonth;
    }
    
    
    /*******************************************************************************
    * FUNCTION NAME: void `$INSTANCE_NAME`_WriteDSTStartMonth(uint8 month)
    ********************************************************************************
    * Summary:
    *  This function writes the DST Start Month software register. Used for 
    *  absolute date entry. Only generated if DST enabled.
    *
    * Parameters: None
    *  (uint8) month: DST Start Month software register value.
    *
    * Return:
    *  None
    *
    * Theory:
    *  See summary
    *
    * Side Effects:
    *  None
    *
    *******************************************************************************/
    void `$INSTANCE_NAME`_WriteDSTStartMonth(uint8 month)
    {
        `$INSTANCE_NAME`_DstStartTimeDate.Month = month;
    }
    
    
    /*******************************************************************************
    * FUNCTION NAME: void `$INSTANCE_NAME`_WriteDSTStartDayOfWeek(uint8 dayofweek)
    ********************************************************************************
    * Summary:
    *  This function writes the DST Start DayOfWeek software register. Used for 
    *  absolute date entry. Only generated if DST enabled.
    *
    * Parameters:
    *  (uint8) dayofweek: DST Start DayOfWeek software register value.
    *
    * Return:
    *  None
    *
    * Theory:
    *  See summary
    *
    * Side Effects:
    *  None
    *
    *******************************************************************************/
    void `$INSTANCE_NAME`_WriteDSTStartDayOfWeek(uint8 dayofweek)
    {
        `$INSTANCE_NAME`_DstStartTimeDate.DayOfWeek = dayofweek;
    }
    
    
    /*******************************************************************************
    * FUNCTION NAME:   void `$INSTANCE_NAME`_WriteDSTStartWeek(uint8 Week)
    ********************************************************************************
    * Summary:
    *  This function writes the DST Start Week software register. Used for 
    *  absolute date entry. Only generated if DST enabled.
    *
    * Parameters:
    *  (uint8) DST Start Week software register value.
    *
    * Return:  
    *  None
    *
    * Theory:
    *  See summary
    *
    * Side Effects:
    *  None
    *
    *******************************************************************************/
    void `$INSTANCE_NAME`_WriteDSTStartWeek(uint8 Week)
    {
        `$INSTANCE_NAME`_DstStartTimeDate.Week = Week;
    }
    
    
    /*******************************************************************************
    * FUNCTION NAME: void `$INSTANCE_NAME`_WriteDSTStopHour(uint8 hour)
    ********************************************************************************
    * Summary:
    *  This function writes the DST Stop Hour software register. Used for 
    *  absolute date entry. Only generated if DST enabled. 
    *
    * Parameters:
    *  (uint8) hour: DST Stop Hour software register value.
    *
    * Return:
    *  None
    *
    * Theory:
    *  See summary
    *
    * Side Effects:
    *  None
    *
    *******************************************************************************/
    void `$INSTANCE_NAME`_WriteDSTStopHour(uint8 hour)
    {
        `$INSTANCE_NAME`_DstStopTimeDate.Hour = hour;
    }
    
    
    /*******************************************************************************
    * FUNCTION NAME: void `$INSTANCE_NAME`_WriteDSTStopDayOfMonth(uint8 dayofmonth)
    ********************************************************************************
    * Summary:
    *  This function writes the DST Stop DayOfMonth software register. Used for 
    *  absolute date entry. Only generated if DST enabled.
    *
    * Parameters:
    *  (uint8) dayofmonth: DST Stop DayOfMonth software register value.
    *
    * Return:
    *  None
    *
    * Theory:
    *  See summary
    *
    * Side Effects:
    *  None
    *
    *******************************************************************************/
    void `$INSTANCE_NAME`_WriteDSTStopDayOfMonth(uint8 dayofmonth)
    {
        `$INSTANCE_NAME`_DstStopTimeDate.DayOfMonth = dayofmonth;
    }
    
    
    /*******************************************************************************
    * FUNCTION NAME: void `$INSTANCE_NAME`_WriteDSTStopMonth(uint8 month)
    ********************************************************************************
    * Summary:
    *  This function writes the DST Stop Month software  register. Used for 
    *  absolute date entry. Only generated if DST enabled.
    *
    * Parameters:
    *  (uint8) month: DST Stop Month software register value.
    *
    * Return:  
    *  None 
    *
    * Theory:
    *  See summary
    *
    * Side Effects:
    *  None
    *
    *******************************************************************************/
    void `$INSTANCE_NAME`_WriteDSTStopMonth(uint8 month)
    {
        `$INSTANCE_NAME`_DstStopTimeDate.Month = month;
    }
    
    
    /*******************************************************************************
    * FUNCTION NAME: void `$INSTANCE_NAME`_WriteDSTStopDayOfWeek(uint8 dayofweek)
    ********************************************************************************
    * Summary:
    *  This function writes the DST Stop DayOfWeek software register. Used for 
    *  relative date entry. Only generated if DST enabled.
    *
    * Parameters:
    *  (uint8) dayofweek: DST Stop DayOfWeek software register value.
    *
    * Return:
    *  None
    *
    * Theory:
    *  See summary
    *
    * Side Effects:
    *  None
    *
    *******************************************************************************/
    void `$INSTANCE_NAME`_WriteDSTStopDayOfWeek(uint8 dayofweek)
    {
        `$INSTANCE_NAME`_DstStopTimeDate.DayOfWeek = dayofweek;
    }
    
    
    /*******************************************************************************
    * FUNCTION NAME: void `$INSTANCE_NAME`_WriteDSTStopWeek(uint8 week)
    ********************************************************************************
    * Summary:
    *  This function writes the DST Stop Week software register. Used for 
    *  relative date entry. Only generated if DST enabled.
    *
    * Parameters:
    *  (uint8) week: DST Stop Week software register value.
    *
    * Return:  
    *  None 
    *
    * Theory:
    *  See summary
    *
    * Side Effects:
    *  None
    *
    *******************************************************************************/
    void `$INSTANCE_NAME`_WriteDSTStopWeek(uint8 week)
    {
        `$INSTANCE_NAME`_DstStopTimeDate.Week = week;
    }
    
    
    /*******************************************************************************
    * FUNCTION NAME: void `$INSTANCE_NAME`_WriteDSTOffset(uint8 offset)
    ********************************************************************************
    * Summary:
    *  This function writes the DST Offset register. Allows a configurable increment
    *  or decrement of time between 0 and 255 minutes. Increment occures on DST Start 
    *  and decrement on DST Stop. Only generated if DST enabled. 
    *
    * Parameters: 
    *  (uint8) offset: DST Offset software register value.
    *
    * Return:
    *  None
    *
    * Theory:
    *  See summary
    *
    * Side Effects:
    *  None
    *
    *******************************************************************************/
    void `$INSTANCE_NAME`_WriteDSTOffset(uint8 offset)
    {
        `$INSTANCE_NAME`_DstOffset = offset;
    }
#endif /* `$INSTANCE_NAME`_DST_FUNC_ENABLE == 1*/

/* [] END OF FILE */


