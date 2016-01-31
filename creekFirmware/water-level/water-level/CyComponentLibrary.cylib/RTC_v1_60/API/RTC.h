/*******************************************************************************
* File Name: `$INSTANCE_NAME`.h
* Version `$CY_MAJOR_VERSION`.`$CY_MINOR_VERSION`
*
* Description:
*  This file provides constants and parameter values for the RTC Component.
*
* Note:
*
********************************************************************************
* Copyright 2008-2010, Cypress Semiconductor Corporation.  All rights reserved.
* You may use this file only in accordance with the license, terms, conditions,
* disclaimers, and limitations in the end user license agreement accompanying
* the software package with which this file was provided.
*******************************************************************************/

#if !defined(CY_RTC_`$INSTANCE_NAME`_H)
#define CY_RTC_`$INSTANCE_NAME`_H

#include "cytypes.h"
#include "cydevice_trm.h"
#include "cyfitter.h"
#include "cyPm.h"


/***************************************
*   Conditional Compilation Parameters
***************************************/

/* PSoC3 ES2 or early */
#define `$INSTANCE_NAME`_PSOC3_ES2   ((CYDEV_CHIP_MEMBER_USED == CYDEV_CHIP_MEMBER_3A)    && \
                     			      (CYDEV_CHIP_REVISION_USED <= CYDEV_CHIP_REVISION_3A_ES2))
                     
#if((`$INSTANCE_NAME`_PSOC3_ES2) && (`$INSTANCE_NAME`_isr__ES2_PATCH))
    #include <intrins.h>
    #define `$INSTANCE_NAME`_ISR_PATCH() _nop_(); _nop_(); _nop_(); _nop_(); _nop_(); _nop_(); _nop_(); _nop_();
#endif  /* (`$INSTANCE_NAME`_PSOC3_ES2 && (`$INSTANCE_NAME`_isr__ES2_PATCH)) */

/* what day of the week is start of week */
#define `$INSTANCE_NAME`_START_OF_WEEK          (`$StartOfWeek`u)

/* Daylight saving time */
#define `$INSTANCE_NAME`_DST_FUNC_ENABLE        (`$DstEnable`u)


/***************************************
*    Data Struct Definitions
***************************************/

typedef struct _`$INSTANCE_NAME`_TIME_DATE
{
    uint8 Sec;
    uint8 Min;
    uint8 Hour;
    uint8 DayOfWeek;
    uint8 DayOfMonth;
    uint16 DayOfYear;
    uint8 Month;
    uint16 Year;
} volatile `$INSTANCE_NAME`_TIME_DATE;

typedef struct _`$INSTANCE_NAME`_DSTIME
{
    uint8 Hour;
    uint8 DayOfWeek;
    uint8 Week;
    uint8 DayOfMonth;
    uint8 Month;
} volatile `$INSTANCE_NAME`_DSTIME;


/***************************************
*    Function Prototypes
***************************************/

CY_ISR_PROTO(`$INSTANCE_NAME`_ISR);

void `$INSTANCE_NAME`_Start(void);
void `$INSTANCE_NAME`_Stop(void) `=ReentrantKeil($INSTANCE_NAME . "_Stop")`;
void `$INSTANCE_NAME`_EnableInt(void) `=ReentrantKeil($INSTANCE_NAME . "_EnableInt")`;
void `$INSTANCE_NAME`_DisableInt(void) `=ReentrantKeil($INSTANCE_NAME . "_DisableInt")`;
void  `$INSTANCE_NAME`_Init(void);
void `$INSTANCE_NAME`_Enable(void) `=ReentrantKeil($INSTANCE_NAME . "_Enable")`;

`$INSTANCE_NAME`_TIME_DATE* `$INSTANCE_NAME`_ReadTime(void) `=ReentrantKeil($INSTANCE_NAME . "_ReadTime")`;
void `$INSTANCE_NAME`_WriteTime(`$INSTANCE_NAME`_TIME_DATE *timeDate);

/* RTC write functions to set Start Values */
void `$INSTANCE_NAME`_WriteSecond(uint8 second);
void `$INSTANCE_NAME`_WriteMinute(uint8 minute);
void `$INSTANCE_NAME`_WriteHour(uint8 hour);
void `$INSTANCE_NAME`_WriteDayOfMonth(uint8 dayOfMonth);
void `$INSTANCE_NAME`_WriteMonth(uint8 month);
void `$INSTANCE_NAME`_WriteYear(uint16 year);

/* RTC Alarm write settings */
void `$INSTANCE_NAME`_WriteAlarmSecond(uint8 second);
void `$INSTANCE_NAME`_WriteAlarmMinute(uint8 minute);
void `$INSTANCE_NAME`_WriteAlarmHour(uint8 hour);
void `$INSTANCE_NAME`_WriteAlarmDayOfMonth(uint8 dayOfMonth);
void `$INSTANCE_NAME`_WriteAlarmMonth(uint8 month);
void `$INSTANCE_NAME`_WriteAlarmYear(uint16 year);
void `$INSTANCE_NAME`_WriteAlarmDayOfWeek(uint8 dayOfWeek);
void `$INSTANCE_NAME`_WriteAlarmDayOfYear(uint16 dayOfYear);

/* RTC read settings to set start values */
uint8 `$INSTANCE_NAME`_ReadSecond(void) `=ReentrantKeil($INSTANCE_NAME . "_ReadSecond")`;
uint8 `$INSTANCE_NAME`_ReadMinute(void) `=ReentrantKeil($INSTANCE_NAME . "_ReadMinute")`;
uint8 `$INSTANCE_NAME`_ReadHour(void) `=ReentrantKeil($INSTANCE_NAME . "_ReadHour")`;
uint8 `$INSTANCE_NAME`_ReadDayOfMonth(void) `=ReentrantKeil($INSTANCE_NAME . "_ReadDayOfMonth")`;
uint8 `$INSTANCE_NAME`_ReadMonth(void) `=ReentrantKeil($INSTANCE_NAME . "_ReadMonth")`;
uint16 `$INSTANCE_NAME`_ReadYear(void) `=ReentrantKeil($INSTANCE_NAME . "_ReadYear")`;

/* Alarm read settings */
uint8 `$INSTANCE_NAME`_ReadAlarmSecond(void) `=ReentrantKeil($INSTANCE_NAME . "_ReadAlarmSecond")`;
uint8 `$INSTANCE_NAME`_ReadAlarmMinute(void) `=ReentrantKeil($INSTANCE_NAME . "_ReadAlarmMinute")`;
uint8 `$INSTANCE_NAME`_ReadAlarmHour(void) `=ReentrantKeil($INSTANCE_NAME . "_ReadAlarmHour")`;
uint8 `$INSTANCE_NAME`_ReadAlarmDayOfMonth(void) `=ReentrantKeil($INSTANCE_NAME . "_ReadAlarmDayOfMonth")`;
uint8 `$INSTANCE_NAME`_ReadAlarmMonth(void) `=ReentrantKeil($INSTANCE_NAME . "_ReadAlarmMonth")`;
uint16 `$INSTANCE_NAME`_ReadAlarmYear(void) `=ReentrantKeil($INSTANCE_NAME . "_ReadAlarmYear")`;
uint8 `$INSTANCE_NAME`_ReadAlarmDayOfWeek(void) `=ReentrantKeil($INSTANCE_NAME . "_ReadAlarmDayOfWeek")`;
uint16 `$INSTANCE_NAME`_ReadAlarmDayOfYear(void) `=ReentrantKeil($INSTANCE_NAME . "_ReadAlarmDayOfYear")`;

/* Set mask interrupt registers */
void `$INSTANCE_NAME`_WriteAlarmMask(uint8 mask);
void `$INSTANCE_NAME`_WriteIntervalMask(uint8 mask);

/* Read status register */
uint8 `$INSTANCE_NAME`_ReadStatus(void);

#if (1u == `$INSTANCE_NAME`_DST_FUNC_ENABLE)

    /* DST write settings  */
    void `$INSTANCE_NAME`_WriteDSTMode(uint8 mode);
    void `$INSTANCE_NAME`_WriteDSTStartHour(uint8 hour);
    void `$INSTANCE_NAME`_WriteDSTStartDayOfMonth(uint8 dayOfMonth);
    void `$INSTANCE_NAME`_WriteDSTStartMonth(uint8 month);
    void `$INSTANCE_NAME`_WriteDSTStartDayOfWeek(uint8 dayOfWeek);
    void `$INSTANCE_NAME`_WriteDSTStartWeek(uint8 week);

    void `$INSTANCE_NAME`_WriteDSTStopHour(uint8 hour);
    void `$INSTANCE_NAME`_WriteDSTStopDayOfMonth(uint8 dayOfMonth);
    void `$INSTANCE_NAME`_WriteDSTStopMonth(uint8 month);
    void `$INSTANCE_NAME`_WriteDSTStopDayOfWeek(uint8 dayOfWeek);
    void `$INSTANCE_NAME`_WriteDSTStopWeek(uint8 week);
    void `$INSTANCE_NAME`_WriteDSTOffset(uint8 offset);
    
#endif /* 1u == `$INSTANCE_NAME`_DST_FUNC_ENABLE */


/***************************************
*        API Constants
***************************************/

/* Number of the `$INSTANCE_NAME`_isr interrupt */
#define `$INSTANCE_NAME`_ISR_NUMBER             `$INSTANCE_NAME``[isr]`_INTC_NUMBER

/* Priority of the `$INSTANCE_NAME`_isr interrupt */
#define `$INSTANCE_NAME`_ISR_PRIORITY           `$INSTANCE_NAME``[isr]`_INTC_PRIOR_NUM

/* Time elapse constants */
#define `$INSTANCE_NAME`_MINUTE_ELAPSED         (59u)
#define `$INSTANCE_NAME`_HOUR_ELAPSED           (59u)
#define `$INSTANCE_NAME`_HALF_OF_DAY_ELAPSED    (12u)
#define `$INSTANCE_NAME`_DAY_ELAPSED            (23u)
#define `$INSTANCE_NAME`_WEEK_ELAPSED           (7u)
#define `$INSTANCE_NAME`_YEAR_ELAPSED           (12u)
#define `$INSTANCE_NAME`_DAYS_IN_WEEK           (7u)

/* Interval software register bit location */
#define `$INSTANCE_NAME`_INTERVAL_SEC_MASK      (0x01u)       /* SEC */
#define `$INSTANCE_NAME`_INTERVAL_MIN_MASK      (0x02u)       /* MIN */
#define `$INSTANCE_NAME`_INTERVAL_HOUR_MASK     (0x04u)       /* HOUR*/
#define `$INSTANCE_NAME`_INTERVAL_DAY_MASK      (0x08u)       /* DOM */
#define `$INSTANCE_NAME`_INTERVAL_WEEK_MASK     (0x10u)       /* DOM */
#define `$INSTANCE_NAME`_INTERVAL_MONTH_MASK    (0x20u)       /* MONTH */
#define `$INSTANCE_NAME`_INTERVAL_YEAR_MASK     (0x40u)       /* YEAR */

/* Alarm software register bit location */
#define `$INSTANCE_NAME`_ALARM_SEC_MASK         (0x01u)       /* SEC */
#define `$INSTANCE_NAME`_ALARM_MIN_MASK         (0x02u)       /* MIN */
#define `$INSTANCE_NAME`_ALARM_HOUR_MASK        (0x04u)       /* HOUR*/
#define `$INSTANCE_NAME`_ALARM_DAYOFWEEK_MASK   (0x08u)       /* DOW */
#define `$INSTANCE_NAME`_ALARM_DAYOFMONTH_MASK  (0x10u)       /* DOM */
#define `$INSTANCE_NAME`_ALARM_DAYOFYEAR_MASK   (0x20u)       /* DOY */
#define `$INSTANCE_NAME`_ALARM_MONTH_MASK       (0x40u)       /* MONTH */
#define `$INSTANCE_NAME`_ALARM_YEAR_MASK        (0x80u)       /* YEAR */

/* Status software register bit location */

/* DST stutus bit */
#define `$INSTANCE_NAME`_STATUS_DST             (0x01u)

/* Leap Year status bit */
#define `$INSTANCE_NAME`_STATUS_LY              (0x02u)

/* AM/PM status bit */
#define `$INSTANCE_NAME`_STATUS_AM_PM           (0x04u)

/* Alarm Active status bit */
#define `$INSTANCE_NAME`_STATUS_AA              (0x08u)

/* Days Of Week definition */
`$DaysOfWeek`

/* Month definition */
#define `$INSTANCE_NAME`_JANUARY                (1u)
#define `$INSTANCE_NAME`_DAYS_IN_JANUARY        (31u)
#define `$INSTANCE_NAME`_FEBRUARY               (2u)
#define `$INSTANCE_NAME`_DAYS_IN_FEBRUARY       (28u)
#define `$INSTANCE_NAME`_MARCH                  (3u)
#define `$INSTANCE_NAME`_DAYS_IN_MARCH          (31u)
#define `$INSTANCE_NAME`_APRIL                  (4u)
#define `$INSTANCE_NAME`_DAYS_IN_APRIL          (30u)
#define `$INSTANCE_NAME`_MAY                    (5u)
#define `$INSTANCE_NAME`_DAYS_IN_MAY            (31u)
#define `$INSTANCE_NAME`_JUNE                   (6u)
#define `$INSTANCE_NAME`_DAYS_IN_JUNE           (30u)
#define `$INSTANCE_NAME`_JULY                   (7u)
#define `$INSTANCE_NAME`_DAYS_IN_JULY           (31u)
#define `$INSTANCE_NAME`_AUGUST                 (8u)
#define `$INSTANCE_NAME`_DAYS_IN_AUGUST         (31u)
#define `$INSTANCE_NAME`_SEPTEMBER              (9u)
#define `$INSTANCE_NAME`_DAYS_IN_SEPTEMBER      (30u)
#define `$INSTANCE_NAME`_OCTOBER                (10u)
#define `$INSTANCE_NAME`_DAYS_IN_OCTOBER        (31u)
#define `$INSTANCE_NAME`_NOVEMBER               (11u)
#define `$INSTANCE_NAME`_DAYS_IN_NOVEMBER       (30u)
#define `$INSTANCE_NAME`_DECEMBER               (12u)
#define `$INSTANCE_NAME`_DAYS_IN_DECEMBER       (31u)

/* DTS software registers bit location */

/* DST Enable */
#define `$INSTANCE_NAME`_DST_ENABLE             (0x01u)

/* Fixed data selected  */
#define `$INSTANCE_NAME`_DST_FIXDATE            (0x00u)

/* Relative data selected */
#define `$INSTANCE_NAME`_DST_RELDATE            (0x02u)

/* DST hour match flag */
#define `$INSTANCE_NAME`_DST_HOUR               (0x01u)

/* DST day of month match flag */
#define `$INSTANCE_NAME`_DST_DAYOFMONTH         (0x02u)

/* DST month match flag */
#define `$INSTANCE_NAME`_DST_MONTH              (0x04u)

#define `$INSTANCE_NAME`_MONTHS_IN_YEAR         (12u)
#define `$INSTANCE_NAME`_DAYS_IN_YEAR           (365u)
#define `$INSTANCE_NAME`_DAYS_IN_LEAP_YEAR      (366u)

/* Returns 1 if leap year, otherwise 0 */
#define `$INSTANCE_NAME`_LEAP_YEAR(year) ((!((year) % 400u) || (!((year) % 4u) && ((year) % 100u))) ? 0x01u : 0x00u)

/* Returns 1 if corresponding bit is set, otherwise 0 */
#define `$INSTANCE_NAME`_IS_BIT_SET(value, mask) (((mask) == ((value) & (mask))) ? 0x01u : 0x00u)

/* Set alarm if needed */
#define `$INSTANCE_NAME`_SET_ALARM(alarmCfg, alarmCur, status) \
    if((alarmCfg) && (`$INSTANCE_NAME`_IS_BIT_SET((alarmCur),(alarmCfg)))) \
    { \
        (status)  |= `$INSTANCE_NAME`_STATUS_AA; \
        (alarmCur) = 0u; \
    }

/* Following definitions are for the COMPATIBILITY ONLY, they are OBSOLETE. */
#define `$INSTANCE_NAME`_IsLeapYear         `$INSTANCE_NAME`_LEAP_YEAR
#define `$INSTANCE_NAME`_Dst                `$INSTANCE_NAME`_DSTIME
#define `$INSTANCE_NAME`_TimeDate           `$INSTANCE_NAME`_TIME_DATE

#define `$INSTANCE_NAME`_CurTimeDate        `$INSTANCE_NAME`_currentTimeDate
#define `$INSTANCE_NAME`_AlarmTimeDate      `$INSTANCE_NAME`_alarmCfgTimeDate

#if (1u == `$INSTANCE_NAME`_DST_FUNC_ENABLE)
    #define `$INSTANCE_NAME`_DstMode            `$INSTANCE_NAME`_dstModeType
    #define `$INSTANCE_NAME`_DstStartTimeDate   `$INSTANCE_NAME`_dstTimeDateStart
    #define `$INSTANCE_NAME`_DstStopTimeDate    `$INSTANCE_NAME`_dstTimeDateStop
    
    #define `$INSTANCE_NAME`_DstOffset          `$INSTANCE_NAME`_dstOffsetMin
    #define `$INSTANCE_NAME`_DstStatusStart     `$INSTANCE_NAME`_dstStartStatus
    #define `$INSTANCE_NAME`_DstStatusStop      `$INSTANCE_NAME`_dstStopStatus
#endif /* 1u == `$INSTANCE_NAME`_DST_FUNC_ENABLE */

#define `$INSTANCE_NAME`_AlarmMask      `$INSTANCE_NAME`_alarmCfgMask
#define `$INSTANCE_NAME`_AlarmStatus    `$INSTANCE_NAME`_alarmCurStatus
#define `$INSTANCE_NAME`_IntervalMask   `$INSTANCE_NAME`_intervalCfgMask
#define `$INSTANCE_NAME`_Status         `$INSTANCE_NAME`_statusDateTime
#define `$INSTANCE_NAME`_Dim            `$INSTANCE_NAME`_daysInMonths
#define `$INSTANCE_NAME`_Seq            `$INSTANCE_NAME`_monthTemplate


/***************************************
*    Registers
***************************************/

/* Timewheel Configuration Register 2 */
#define `$INSTANCE_NAME`_OPPS_CFG_REG           (* (reg8 *) CYREG_PM_TW_CFG2 )
#define `$INSTANCE_NAME`_OPPS_CFG_PTR           (  (reg8 *) CYREG_PM_TW_CFG2 )

/* Power Manager Interrupt Status Register */
#define `$INSTANCE_NAME`_OPPS_INT_SR_REG        (* (reg8 *) CYREG_PM_INT_SR )
#define `$INSTANCE_NAME`_OPPS_INT_SR_PTR        (  (reg8 *) CYREG_PM_INT_SR )


/***************************************
*    External Software Registers
***************************************/

extern `$INSTANCE_NAME`_TIME_DATE    `$INSTANCE_NAME`_alarmCfgTimeDate;
extern `$INSTANCE_NAME`_TIME_DATE    `$INSTANCE_NAME`_currentTimeDate;

#if (1u == `$INSTANCE_NAME`_DST_FUNC_ENABLE)
    extern volatile uint8             `$INSTANCE_NAME`_dstModeType;
    extern `$INSTANCE_NAME`_DSTIME     `$INSTANCE_NAME`_dstTimeDateStart;
    extern `$INSTANCE_NAME`_DSTIME     `$INSTANCE_NAME`_dstTimeDateStop;
    extern volatile uint8   `$INSTANCE_NAME`_dstOffsetMin;
    extern volatile uint8   `$INSTANCE_NAME`_dstStartStatus;
    extern volatile uint8   `$INSTANCE_NAME`_dstStopStatus;
#endif /* 1u == `$INSTANCE_NAME`_DST_FUNC_ENABLE */

extern volatile uint8       `$INSTANCE_NAME`_alarmCfgMask;
extern volatile uint8       `$INSTANCE_NAME`_alarmCurStatus;
extern volatile uint8       `$INSTANCE_NAME`_intervalCfgMask;
extern volatile uint8       `$INSTANCE_NAME`_statusDateTime;

extern const uint8 CYCODE   `$INSTANCE_NAME`_daysInMonths [`$INSTANCE_NAME`_MONTHS_IN_YEAR];
extern const uint8 CYCODE   `$INSTANCE_NAME`_monthTemplate [`$INSTANCE_NAME`_MONTHS_IN_YEAR];

    
/***************************************
*        Register Constants
****************************************/

#define `$INSTANCE_NAME`_OPPS_EN_MASK           (0x10u)
#define `$INSTANCE_NAME`_OPPSIE_EN_MASK         (0x20u)

/* Enable wakeup from the Sleeep low power mode */
#define `$INSTANCE_NAME`_PM_WAKEUP_CTW_1PPS     (0x80u)

#endif /* CY_RTC_`$INSTANCE_NAME`_H */


/* [] END OF FILE */
