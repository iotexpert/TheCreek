/*******************************************************************************
* File Name: `$INSTANCE_NAME`_INT.c 
* Version `$CY_MAJOR_VERSION`.`$CY_MINOR_VERSION`
*
* Description:
*     This file contains the Interrupt Service Routine (ISR) for the RTC
*     Component. This interrupt routine has entry pointes to overflow on time 
*     or date.
*
* Note:
*
********************************************************************************
* Copyright 2008-2009, Cypress Semiconductor Corporation.  All rights reserved.
* You may use this file only in accordance with the license, terms, conditions, 
* disclaimers, and limitations in the end user license agreement accompanying 
* the software package with which this file was provided.
********************************************************************************/


#include "`@INSTANCE_NAME`.h"

extern uint8 `$INSTANCE_NAME`_IsLeapYear(uint16 year);

#if (`$INSTANCE_NAME`_DST_FUNC_ENABLE == 1)
    extern void `$INSTANCE_NAME`_DSTDateConversion(void);
#endif /* `$INSTANCE_NAME`_DST_FUNC_ENABLE == 1 */

/* `#START RTC_ISR_DEFINITION` */

/* `#END` */

/*******************************************************************************
* FUNCTION NAME:   void `@INSTANCE_NAME`_EverySecondHandler(void)
********************************************************************************
* Summary:
*  This function is called every second. 
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
**********************************************************************************/
void `@INSTANCE_NAME`_EverySecondHandler(void)
{
    /* `#START EVERY_SECOND_HANDLER_CODE` */
    
    /* `#END` */
}


/*******************************************************************************
* FUNCTION NAME:   void `@INSTANCE_NAME`_EveryMinuteHandler(void)
********************************************************************************
* Summary:
*  This function is called every minute. 
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
**********************************************************************************/
void `@INSTANCE_NAME`_EveryMinuteHandler(void)
{
    /* `#START EVERY_MINUTE_HANDLER_CODE` */

    /* `#END` */
}


/*******************************************************************************
* FUNCTION NAME:   void `@INSTANCE_NAME`_EveryHourHandler(void)
********************************************************************************
* Summary:
*  This function is called every hour. 
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
**********************************************************************************/
void `@INSTANCE_NAME`_EveryHourHandler(void)
{
    /* `#START EVERY_HOUR_HANDLER_CODE` */

    /* `#END` */
}


/*******************************************************************************
* FUNCTION NAME:   void `@INSTANCE_NAME`_EveryDayHandler(void)
********************************************************************************
* Summary:
*  This function is called every day. 
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
**********************************************************************************/
void `@INSTANCE_NAME`_EveryDayHandler(void)
{
    /* `#START EVERY_DAY_HANDLER_CODE` */

    /* `#END` */
}


 /*******************************************************************************
 * FUNCTION NAME:   void `@INSTANCE_NAME`_EveryWeekHandler(void)
 ********************************************************************************
 * Summary:
 *  This function is called every week. 
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
 **********************************************************************************/
void `@INSTANCE_NAME`_EveryWeekHandler(void)
{
    /* `#START EVERY_WEEK_HANDLER_CODE` */

    /* `#END` */
}


 /*******************************************************************************
 * FUNCTION NAME:   void `@INSTANCE_NAME`_EveryMonthHandler(void)
 ********************************************************************************
 * Summary:
 *  This function is called every month. 
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
 **********************************************************************************/
void `@INSTANCE_NAME`_EveryMonthHandler(void)
{
    /* `#START EVERY_MONTH_HANDLER_CODE` */

    /* `#END` */
}


 /*******************************************************************************
 * FUNCTION NAME:   void `@INSTANCE_NAME`_EveryYearHandler(void)
 ********************************************************************************
 * Summary:
 *  This function is called every year. 
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
 **********************************************************************************/
void `@INSTANCE_NAME`_EveryYearHandler(void)
{
    /* `#START EVERY_YEAR_HANDLER_CODE` */

    /* `#END` */
}

/*******************************************************************************
* FUNCTION NAME:   void `$INSTANCE_NAME`_ISR(void)
********************************************************************************
* Summary:
*  This ISR is executed when a terminal count occurs. Global interrupt must be 
*  enable to invoke this ISR. This interrupt trig every second. 
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
*******************************************************************************/
CY_ISR(`$INSTANCE_NAME`_ISR)
{
    /* Clear OPPS interrupt status flag */
    if (0 == `$INSTANCE_NAME`_OPPS_INT_SR)
    {
        `$INSTANCE_NAME`_CurTimeDate.Sec = `$INSTANCE_NAME`_CurTimeDate.Sec + 1 - 1;   
    }
    
    `$INSTANCE_NAME`_CurTimeDate.Sec++;
    
        if(`$INSTANCE_NAME`_CurTimeDate.Sec > `$INSTANCE_NAME`_MINUTE_ELAPSED)
    {
        /* Inc Min */
        `$INSTANCE_NAME`_CurTimeDate.Min++;
        `$INSTANCE_NAME`_CurTimeDate.Sec = 0;
        
        if (`$INSTANCE_NAME`_CurTimeDate.Min > `$INSTANCE_NAME`_HOUR_ELAPSED)
        {
            /* Inc HOUR */
            `$INSTANCE_NAME`_CurTimeDate.Hour++; 
            `$INSTANCE_NAME`_CurTimeDate.Min = 0;            
      
            if(`$INSTANCE_NAME`_CurTimeDate.Hour > `$INSTANCE_NAME`_DAY_ELAPSED)
            {
                /* Inc DayOfMonth */
                `$INSTANCE_NAME`_CurTimeDate.DayOfMonth++; 
                `$INSTANCE_NAME`_CurTimeDate.Hour = 0;
                /* Inc DayOfYear */
                `$INSTANCE_NAME`_CurTimeDate.DayOfYear++;
                /* Calculate DayOfWeek */
                `$INSTANCE_NAME`_CurTimeDate.DayOfWeek++;
                if (`$INSTANCE_NAME`_CurTimeDate.DayOfWeek > `$INSTANCE_NAME`_WEEK_ELAPSED)
                {
                    `$INSTANCE_NAME`_CurTimeDate.DayOfWeek = 1;
                }
                                   
                if( (((`$INSTANCE_NAME`_Status & `$INSTANCE_NAME`_STATUS_LY) && (`$INSTANCE_NAME`_CurTimeDate.Month == `$INSTANCE_NAME`_FEBRUARY))
                && (`$INSTANCE_NAME`_CurTimeDate.DayOfMonth > `$INSTANCE_NAME`_Dim[`$INSTANCE_NAME`_CurTimeDate.Month-1] + 1)) ||
                (((`$INSTANCE_NAME`_Status & `$INSTANCE_NAME`_STATUS_LY) && (!(`$INSTANCE_NAME`_CurTimeDate.Month == `$INSTANCE_NAME`_FEBRUARY)))
                && (`$INSTANCE_NAME`_CurTimeDate.DayOfMonth > `$INSTANCE_NAME`_Dim[`$INSTANCE_NAME`_CurTimeDate.Month-1])) ||                
                ((!(`$INSTANCE_NAME`_Status & `$INSTANCE_NAME`_STATUS_LY)) && (`$INSTANCE_NAME`_CurTimeDate.DayOfMonth > `$INSTANCE_NAME`_Dim[`$INSTANCE_NAME`_CurTimeDate.Month-1])) )
                {                
                    /* Inc Month */
                    `$INSTANCE_NAME`_CurTimeDate.Month++;
                    `$INSTANCE_NAME`_CurTimeDate.DayOfMonth = 1;                            
                    
                    if(`$INSTANCE_NAME`_CurTimeDate.Month > `$INSTANCE_NAME`_YEAR_ELAPSED)
                    {
                        /* Inc Year */
                        `$INSTANCE_NAME`_CurTimeDate.Year++;
                        `$INSTANCE_NAME`_CurTimeDate.Month = 1;
                        `$INSTANCE_NAME`_CurTimeDate.DayOfYear = 1; 
                        
                        if(`$INSTANCE_NAME`_IsLeapYear(`$INSTANCE_NAME`_CurTimeDate.Year))  
                        { 
                            /* LP - true, else - false */
                            `$INSTANCE_NAME`_Status |= `$INSTANCE_NAME`_STATUS_LY;
                        }
                        else
                        {
                            `$INSTANCE_NAME`_Status &= ~`$INSTANCE_NAME`_STATUS_LY;
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
                        
                        /* Every Year*/
                        if (`$INSTANCE_NAME`_IntervalMask & `$INSTANCE_NAME`_INTERVAL_YEAR_MASK)
                        { 
                            `$INSTANCE_NAME`_EveryYearHandler();
                        }
                        
                        #if (`$INSTANCE_NAME`_DST_FUNC_ENABLE == 1)
                            if (`$INSTANCE_NAME`_DstMode & `$INSTANCE_NAME`_DST_RELDATE)
                            {
                                `$INSTANCE_NAME`_DSTDateConversion();
                            }
                        #endif /* `$INSTANCE_NAME`_DST_FUNC_ENABLE == 1 */
                                                
                    } /* Month > 12 */
                        
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
                    
                    #if (`$INSTANCE_NAME`_DST_FUNC_ENABLE == 1)
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
                    #endif /* `$INSTANCE_NAME`_DST_FUNC_ENABLE == 1 */
                    
                    /* Set Alarm flag event */
                      if ((`$INSTANCE_NAME`_AlarmMask == (`$INSTANCE_NAME`_AlarmMask & `$INSTANCE_NAME`_AlarmStatus)) && `$INSTANCE_NAME`_AlarmMask )
                    {
                        `$INSTANCE_NAME`_Status |= `$INSTANCE_NAME`_STATUS_AA;
                        `$INSTANCE_NAME`_AlarmStatus = 0;
                    } 
                    
                    /* Every Month */
                    if (`$INSTANCE_NAME`_IntervalMask & `$INSTANCE_NAME`_INTERVAL_MONTH_MASK)
                    { 
                        `$INSTANCE_NAME`_EveryMonthHandler();
                    }
                    
                }    /* DayOfMonth > 28,29,30,31 = Month */
                
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
                                
                #if (`$INSTANCE_NAME`_DST_FUNC_ENABLE == 1)
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
                #endif /* `$INSTANCE_NAME`_DST_FUNC_ENABLE == 1 */
                
                /* Set Alarm flag event */
                  if ((`$INSTANCE_NAME`_AlarmMask == (`$INSTANCE_NAME`_AlarmMask & `$INSTANCE_NAME`_AlarmStatus)) && `$INSTANCE_NAME`_AlarmMask )
                {
                    `$INSTANCE_NAME`_Status |= `$INSTANCE_NAME`_STATUS_AA;
                    `$INSTANCE_NAME`_AlarmStatus = 0;
                } 
                
                /* Every Day */
                if (`$INSTANCE_NAME`_IntervalMask & `$INSTANCE_NAME`_INTERVAL_DAY_MASK)
                { 
                    `$INSTANCE_NAME`_EveryDayHandler();
                }
                 
                if (`$INSTANCE_NAME`_CurTimeDate.DayOfWeek == 1)
                {
                    /* Every Week */
                    if (`$INSTANCE_NAME`_IntervalMask & `$INSTANCE_NAME`_INTERVAL_WEEK_MASK)
                    {     
                       `$INSTANCE_NAME`_EveryWeekHandler();
                    }
                }            
            
            } /* Hour > 23 = Day */
                                        
            /* Status set PM/AM flag */
            if (`$INSTANCE_NAME`_CurTimeDate.Hour < `$INSTANCE_NAME`_HALF_OF_DAY_ELAPSED)
            {
                /* AM Hour 00:00-11:59, flag zero */
                `$INSTANCE_NAME`_Status &= ~`$INSTANCE_NAME`_STATUS_AM_PM;
            }
            else
            {
                /* PM Hour 12:00-23:59, flag set */
                `$INSTANCE_NAME`_Status |= `$INSTANCE_NAME`_STATUS_AM_PM;
            }
            
            #if (`$INSTANCE_NAME`_DST_FUNC_ENABLE == 1)
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
                                
                                /* Status set PM/AM flag */
                                if (`$INSTANCE_NAME`_CurTimeDate.Hour < `$INSTANCE_NAME`_HALF_OF_DAY_ELAPSED)
                                {
                                    /* AM Hour 00:00-11:59, flag zero */
                                    `$INSTANCE_NAME`_Status &= ~`$INSTANCE_NAME`_STATUS_AM_PM;
                                }
                                else
                                {
                                    /* PM Hour 12:00-23:59, flag set */
                                    `$INSTANCE_NAME`_Status |= `$INSTANCE_NAME`_STATUS_AM_PM;
                                }
                                
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
                                        `$INSTANCE_NAME`_EveryYearHandler();        
                                    }
                                    else
                                    {
                                        `$INSTANCE_NAME`_CurTimeDate.DayOfMonth = `$INSTANCE_NAME`_Dim[`$INSTANCE_NAME`_CurTimeDate.Month-1];
                                    }
                                    `$INSTANCE_NAME`_EveryMonthHandler();
                                }
                                `$INSTANCE_NAME`_EveryDayHandler();
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
                                
                                /* Status set PM/AM flag */
                                if (`$INSTANCE_NAME`_CurTimeDate.Hour < `$INSTANCE_NAME`_HALF_OF_DAY_ELAPSED)
                                {
                                    /* AM Hour 00:00-11:59, flag zero */
                                    `$INSTANCE_NAME`_Status &= ~`$INSTANCE_NAME`_STATUS_AM_PM;
                                }
                                else
                                {
                                    /* PM Hour 12:00-23:59, flag set */
                                    `$INSTANCE_NAME`_Status |= `$INSTANCE_NAME`_STATUS_AM_PM;
                                }
                                
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
                                        
                                        `$INSTANCE_NAME`_EveryYearHandler();
                                    }
                                    `$INSTANCE_NAME`_EveryMonthHandler();
                                }
                                `$INSTANCE_NAME`_EveryDayHandler();
                            }
                            `$INSTANCE_NAME`_Status |= `$INSTANCE_NAME`_STATUS_DST;
                            `$INSTANCE_NAME`_DstStatusStart = 0;
                            
                            /* Month */
                            if(`$INSTANCE_NAME`_DstStopTimeDate.Month == `$INSTANCE_NAME`_CurTimeDate.Month)
                            {
                                `$INSTANCE_NAME`_DstStatusStop |= `$INSTANCE_NAME`_DST_MONTH;
                            }
                            else
                            {
                                `$INSTANCE_NAME`_DstStatusStop &= ~`$INSTANCE_NAME`_DST_MONTH;
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
            #endif /* `$INSTANCE_NAME`_DST_FUNC_ENABLE == 1 */
            
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
            
            /* Set Alarm flag event */
              if ((`$INSTANCE_NAME`_AlarmMask == (`$INSTANCE_NAME`_AlarmMask & `$INSTANCE_NAME`_AlarmStatus)) && `$INSTANCE_NAME`_AlarmMask )
            {
                `$INSTANCE_NAME`_Status |= `$INSTANCE_NAME`_STATUS_AA;
                `$INSTANCE_NAME`_AlarmStatus = 0;
            } 
            /* Every Hour */
            if (`$INSTANCE_NAME`_IntervalMask & `$INSTANCE_NAME`_INTERVAL_HOUR_MASK)
            { 
                `$INSTANCE_NAME`_EveryHourHandler();
            }
            
        } /* Min > 59 = Hour */
        
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
        
        /* Set Alarm flag event */
          if ((`$INSTANCE_NAME`_AlarmMask == (`$INSTANCE_NAME`_AlarmMask & `$INSTANCE_NAME`_AlarmStatus)) && `$INSTANCE_NAME`_AlarmMask )
        {
            `$INSTANCE_NAME`_Status |= `$INSTANCE_NAME`_STATUS_AA;
            `$INSTANCE_NAME`_AlarmStatus = 0;
        } 
        /* Every Min */
        if (`$INSTANCE_NAME`_IntervalMask & `$INSTANCE_NAME`_INTERVAL_MIN_MASK)
        {
            `$INSTANCE_NAME`_EveryMinuteHandler();
        }
    
    } /* Sec */
    
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
    
    /* Set Alarm flag event */
    if ((`$INSTANCE_NAME`_AlarmMask == (`$INSTANCE_NAME`_AlarmMask & `$INSTANCE_NAME`_AlarmStatus)) && `$INSTANCE_NAME`_AlarmMask )
    {
         `$INSTANCE_NAME`_Status |= `$INSTANCE_NAME`_STATUS_AA;
        `$INSTANCE_NAME`_AlarmStatus = 0;
    } 
    /* Every Sec */ 
    if (`$INSTANCE_NAME`_IntervalMask & `$INSTANCE_NAME`_INTERVAL_SEC_MASK)
    {
        `$INSTANCE_NAME`_EverySecondHandler();
    }
        
    /* PSoC3 ES1, ES2 RTC ISR PATCH  */ 
    #if(CYDEV_CHIP_DIE_EXPECT == CYDEV_CHIP_DIE_LEOPARD)
        #if((CYDEV_CHIP_REV_EXPECT <= CYDEV_CHIP_REV_LEOPARD_ES2) && (`$INSTANCE_NAME`_isr__ES2_PATCH))
            `$INSTANCE_NAME`_ISR_PATCH();
        #endif
    #endif
                
}

/* [] END OF FILE */


