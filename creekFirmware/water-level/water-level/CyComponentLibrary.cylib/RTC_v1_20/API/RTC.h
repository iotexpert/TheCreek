/*******************************************************************************
* File Name: `$INSTANCE_NAME`.h
* Version `$CY_MAJOR_VERSION`.`$CY_MINOR_VERSION`
*
* Description:
*     This file provides constants and parameter values for the RTC Component.
*
* Note:
*********************************************************************************
* Copyright 2008-2009, Cypress Semiconductor Corporation.  All rights reserved.
* You may use this file only in accordance with the license, terms, conditions, 
* disclaimers, and limitations in the end user license agreement accompanying 
* the software package with which this file was provided.
********************************************************************************/


#if !defined(CY_RTC_`$INSTANCE_NAME`_H)
#define CY_RTC_`$INSTANCE_NAME`_H

#include "cytypes.h"
#include "cydevice_trm.h"
#include "cyfitter.h"


#if(CYDEV_CHIP_DIE_EXPECT == CYDEV_CHIP_DIE_LEOPARD)     
    #if((CYDEV_CHIP_REV_EXPECT <= CYDEV_CHIP_REV_LEOPARD_ES2) && (`$INSTANCE_NAME`_isr__ES2_PATCH))      
        #include <intrins.h>
        #define `$INSTANCE_NAME`_ISR_PATCH() _nop_(); _nop_(); _nop_(); _nop_(); _nop_(); _nop_(); _nop_(); _nop_();
    #endif
#endif    

/***************************************
 *        Types definition
 ***************************************/      

typedef struct _`$INSTANCE_NAME`_TimeDate
{
    uint8 Sec;
    uint8 Min;
    uint8 Hour;
    uint8 DayOfWeek;
    uint8 DayOfMonth;
    uint16 DayOfYear;
    uint8 Month;
    uint16 Year;
} `$INSTANCE_NAME`_TimeDate;

typedef struct _`$INSTANCE_NAME`_Dst
{
    uint8 Hour;
    uint8 DayOfWeek;
    uint8 Week;
    uint8 DayOfMonth;
    uint8 Month;
} `$INSTANCE_NAME`_Dst;


/***************************************
 *  Conditional Compilation Parameters
 ***************************************/

#define `$INSTANCE_NAME`_START_OF_WEEK          `$StartOfWeek`
#define `$INSTANCE_NAME`_DST_FUNC_ENABLE        `$DstEnable`


/***************************************
 *  Function Prototypes
 ***************************************/

CY_ISR_PROTO(`$INSTANCE_NAME`_ISR);

void `$INSTANCE_NAME`_Start(void);
void `$INSTANCE_NAME`_Stop(void);
void `$INSTANCE_NAME`_EnableInt(void);
void `$INSTANCE_NAME`_DisableInt(void);

`$INSTANCE_NAME`_TimeDate* `$INSTANCE_NAME`_ReadTime(void);
void `$INSTANCE_NAME`_WriteTime(`$INSTANCE_NAME`_TimeDate *timedate);    

/* RTC write functions to set Start Values */
void `$INSTANCE_NAME`_WriteSecond(uint8 second);
void `$INSTANCE_NAME`_WriteMinute(uint8 minute);
void `$INSTANCE_NAME`_WriteHour(uint8 hour);
void `$INSTANCE_NAME`_WriteDayOfMonth(uint8 dayofmonth);
void `$INSTANCE_NAME`_WriteMonth(uint8 month);
void `$INSTANCE_NAME`_WriteYear(uint16 year);

/* RTC Alarm write settings */
void `$INSTANCE_NAME`_WriteAlarmSecond(uint8 second);
void `$INSTANCE_NAME`_WriteAlarmMinute(uint8 minute);
void `$INSTANCE_NAME`_WriteAlarmHour(uint8 hour);
void `$INSTANCE_NAME`_WriteAlarmDayOfMonth(uint8 dayofmonth);
void `$INSTANCE_NAME`_WriteAlarmMonth(uint8 month);
void `$INSTANCE_NAME`_WriteAlarmYear(uint16 year);
void `$INSTANCE_NAME`_WriteAlarmDayOfWeek(uint8 dayofweek);
void `$INSTANCE_NAME`_WriteAlarmDayOfYear(uint16 dayofyear);

/* RTC read settings to set start values */
uint8 `$INSTANCE_NAME`_ReadSecond(void);
uint8 `$INSTANCE_NAME`_ReadMinute(void);
uint8 `$INSTANCE_NAME`_ReadHour(void);
uint8 `$INSTANCE_NAME`_ReadDayOfMonth(void);
uint8 `$INSTANCE_NAME`_ReadMonth(void);
uint16 `$INSTANCE_NAME`_ReadYear(void);

/* RTCAlarm read settings */
uint8 `$INSTANCE_NAME`_ReadAlarmSecond(void);
uint8 `$INSTANCE_NAME`_ReadAlarmMinute(void);
uint8 `$INSTANCE_NAME`_ReadAlarmHour(void);
uint8 `$INSTANCE_NAME`_ReadAlarmDayOfMonth(void);
uint8 `$INSTANCE_NAME`_ReadAlarmMonth(void);
uint16 `$INSTANCE_NAME`_ReadAlarmYear(void);
uint8 `$INSTANCE_NAME`_ReadAlarmDayOfWeek(void);
uint16 `$INSTANCE_NAME`_ReadAlarmDayOfYear(void);

/* Set mask interrupt registers */
void `$INSTANCE_NAME`_WriteAlarmMask(uint8 mask);
void `$INSTANCE_NAME`_WriteIntervalMask(uint8 mask);

/* Read status register */
uint8 `$INSTANCE_NAME`_ReadStatus(void);

#if (`$INSTANCE_NAME`_DST_FUNC_ENABLE == 1)
    /* DST write settings  */
    void `$INSTANCE_NAME`_WriteDSTMode(uint8 mode);
    void `$INSTANCE_NAME`_WriteDSTStartHour(uint8 hour);
    void `$INSTANCE_NAME`_WriteDSTStartDayOfMonth(uint8 dayofmonth);
    void `$INSTANCE_NAME`_WriteDSTStartMonth(uint8 month);
    void `$INSTANCE_NAME`_WriteDSTStartDayOfWeek(uint8 dayofweek);
    void `$INSTANCE_NAME`_WriteDSTStartWeek(uint8 week);
    
    void `$INSTANCE_NAME`_WriteDSTStopHour(uint8 hour);
    void `$INSTANCE_NAME`_WriteDSTStopDayOfMonth(uint8 dayofmonth);
    void `$INSTANCE_NAME`_WriteDSTStopMonth(uint8 month);
    void `$INSTANCE_NAME`_WriteDSTStopDayOfWeek(uint8 dayofweek);
    void `$INSTANCE_NAME`_WriteDSTStopWeek(uint8 week);
    void `$INSTANCE_NAME`_WriteDSTOffset(uint8 offset);
#endif /* `$INSTANCE_NAME`_DST_FUNC_ENABLE == 1 */


/***************************************
 *        API Constants
 ***************************************/

/* Time elapse constants */
#define `$INSTANCE_NAME`_MINUTE_ELAPSED         59
#define `$INSTANCE_NAME`_HOUR_ELAPSED           59
#define `$INSTANCE_NAME`_HALF_OF_DAY_ELAPSED    12
#define `$INSTANCE_NAME`_DAY_ELAPSED            23
#define `$INSTANCE_NAME`_WEEK_ELAPSED           7
#define `$INSTANCE_NAME`_YEAR_ELAPSED           12
#define `$INSTANCE_NAME`_DAYS_IN_WEEK           7


/***************************************
 *        Hardware Registers
 ***************************************/

/* Number of the `$INSTANCE_NAME`_isr interrupt. */
#define `$INSTANCE_NAME`_ISR_NUMBER           `$INSTANCE_NAME``[isr]`_INTC_NUMBER

/* Priority of the `$INSTANCE_NAME`_isr interrupt. */
#define `$INSTANCE_NAME`_ISR_PRIORITY         `$INSTANCE_NAME``[isr]`_INTC_PRIOR_NUM

/* One pulse per second interrupt registers. */
#define `$INSTANCE_NAME`_OPPS_CFG             ( *(reg8 *) CYREG_PM_TW_CFG2 )
#define `$INSTANCE_NAME`_OPPS_INT_SR          ( *(reg8 *) CYREG_PM_INT_SR )

#define `$INSTANCE_NAME`_OPPS_EN_MASK         0x10u 
#define `$INSTANCE_NAME`_OPPSIE_EN_MASK       0x20u


/***************************************
 *  External Software Registers
 ***************************************/

extern `$INSTANCE_NAME`_TimeDate `$INSTANCE_NAME`_CurTimeDate;        
extern `$INSTANCE_NAME`_TimeDate `$INSTANCE_NAME`_AlarmTimeDate;     
#if (`$INSTANCE_NAME`_DST_FUNC_ENABLE == 1)
    extern uint8            `$INSTANCE_NAME`_DstMode;
    extern `$INSTANCE_NAME`_Dst      `$INSTANCE_NAME`_DstStartTimeDate;
    extern `$INSTANCE_NAME`_Dst      `$INSTANCE_NAME`_DstStopTimeDate;  
    extern uint8            `$INSTANCE_NAME`_DstOffset;
    extern uint8            `$INSTANCE_NAME`_DstStatusStart;    
    extern uint8            `$INSTANCE_NAME`_DstStatusStop;
#endif /* `$INSTANCE_NAME`_DST_FUNC_ENABLE == 1*/

extern uint8            `$INSTANCE_NAME`_AlarmMask;
extern uint8            `$INSTANCE_NAME`_AlarmStatus;

extern uint8            `$INSTANCE_NAME`_IntervalMask;
extern uint8            `$INSTANCE_NAME`_Status;

extern const uint8      `$INSTANCE_NAME`_Dim[12];
extern const uint8      `$INSTANCE_NAME`_Seq[12];


/***************************************
 *        Register Constants
 ***************************************/

/* Interval software register bit location */
#define `$INSTANCE_NAME`_INTERVAL_SEC_MASK            0x01u    /* SEC */
#define `$INSTANCE_NAME`_INTERVAL_MIN_MASK            0x02u    /* MIN */    
#define `$INSTANCE_NAME`_INTERVAL_HOUR_MASK           0x04u    /* HOUR*/
#define `$INSTANCE_NAME`_INTERVAL_DAY_MASK            0x08u    /* DOM */
#define `$INSTANCE_NAME`_INTERVAL_WEEK_MASK           0x10u    /* DOM */
#define `$INSTANCE_NAME`_INTERVAL_MONTH_MASK          0x20u    /* MONTH */
#define `$INSTANCE_NAME`_INTERVAL_YEAR_MASK           0x40u    /* YEAR */  

/* Alarm software register bit location */
#define `$INSTANCE_NAME`_ALARM_SEC_MASK               0x01u    /* SEC */
#define `$INSTANCE_NAME`_ALARM_MIN_MASK               0x02u    /* MIN */    
#define `$INSTANCE_NAME`_ALARM_HOUR_MASK              0x04u    /* HOUR*/
#define `$INSTANCE_NAME`_ALARM_DAYOFWEEK_MASK         0x08u    /* DOW */    
#define `$INSTANCE_NAME`_ALARM_DAYOFMONTH_MASK        0x10u    /* DOM */
#define `$INSTANCE_NAME`_ALARM_DAYOFYEAR_MASK         0x20u    /* DOY*/ 
#define `$INSTANCE_NAME`_ALARM_MONTH_MASK             0x40u    /* MONTH */
#define `$INSTANCE_NAME`_ALARM_YEAR_MASK              0x80u    /* YEAR */ 

/* Status software register bit location */
#define `$INSTANCE_NAME`_STATUS_DST                   0x01u    /* DST stutus bit */
#define `$INSTANCE_NAME`_STATUS_LY                    0x02u    /* Leap Year status bit */
#define `$INSTANCE_NAME`_STATUS_AM_PM                 0x04u    /* AM/PM status bit */
#define `$INSTANCE_NAME`_STATUS_AA                    0x08u    /* Alarm Active status bit */ 

`$DaysOfWeek`
/* Month definition */
#define `$INSTANCE_NAME`_JANUARY                      1
#define `$INSTANCE_NAME`_DAYS_IN_JANUARY              31
#define `$INSTANCE_NAME`_FEBRUARY                     2
#define `$INSTANCE_NAME`_DAYS_IN_FEBRUARY             28
#define `$INSTANCE_NAME`_MARCH                        3
#define `$INSTANCE_NAME`_DAYS_IN_MARCH                31
#define `$INSTANCE_NAME`_APRIL                        4
#define `$INSTANCE_NAME`_DAYS_IN_APRIL                30
#define `$INSTANCE_NAME`_MAY                          5
#define `$INSTANCE_NAME`_DAYS_IN_MAY                  31
#define `$INSTANCE_NAME`_JUNE                         6
#define `$INSTANCE_NAME`_DAYS_IN_JUNE                 30
#define `$INSTANCE_NAME`_JULY                         7
#define `$INSTANCE_NAME`_DAYS_IN_JULY                 31
#define `$INSTANCE_NAME`_AUGUST                       8
#define `$INSTANCE_NAME`_DAYS_IN_AUGUST               31
#define `$INSTANCE_NAME`_SEPTEMBER                    9
#define `$INSTANCE_NAME`_DAYS_IN_SEPTEMBER            30
#define `$INSTANCE_NAME`_OCTOBER                      10
#define `$INSTANCE_NAME`_DAYS_IN_OCTOBER              31
#define `$INSTANCE_NAME`_NOVEMBER                     11
#define `$INSTANCE_NAME`_DAYS_IN_NOVEMBER             30
#define `$INSTANCE_NAME`_DECEMBER                     12
#define `$INSTANCE_NAME`_DAYS_IN_DECEMBER             31

/* DTS software registers bit location */
#define `$INSTANCE_NAME`_DST_ENABLE                   0x01u    /* DST Enable */
#define `$INSTANCE_NAME`_DST_FIXDATE                  0x00u    /* Fixed data selected  */
#define `$INSTANCE_NAME`_DST_RELDATE                  0x02u    /* Relative data selected */

#define `$INSTANCE_NAME`_DST_HOUR                     0x01u    /* DST hour match flag */ 
#define `$INSTANCE_NAME`_DST_DAYOFMONTH               0x02u    /* DST day of month match flag*/ 
#define `$INSTANCE_NAME`_DST_MONTH                    0x04u    /* DST month match flag */ 


#endif /* CY_RTC_`$INSTANCE_NAME`_H */


/* [] END OF FILE */

