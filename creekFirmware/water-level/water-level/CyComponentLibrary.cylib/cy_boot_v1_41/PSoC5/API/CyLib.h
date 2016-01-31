/******************************************************************************
* File Name: CyLib.h
* Version `$CY_MAJOR_VERSION`.`$CY_MINOR_VERSION`
*
*  Description:
*
*  Note: 
*   Documentation of the API's in this file is located in the
*   System Reference Guide provided with PSoC Creator.
*
********************************************************************************
* Copyright 2008-2009, Cypress Semiconductor Corporation.  All rights reserved.
* You may use this file only in accordance with the license, terms, conditions, 
* disclaimers, and limitations in the end user license agreement accompanying 
* the software package with which this file was provided.
********************************************************************************/



#if !defined(__CYLIB_H__)
#define __CYLIB_H__



#include <CYTYPES.H>
#include <CYFITTER.H>



#define CYLIB_STRING            1 // needs to move with library stuff.


#if !defined(NULL)

#define NULL                    ((void *) 0x0u)

#endif

#if defined(CYLIB_SYSTEM_CLOCKS)

void CyPLL_OUT_Start(void);
void CyPLL_OUT_Stop(void);
void CyIMO_Start(void);
void CyIMO_Stop(void);
void CyXTAL_Start(void);
void CyXTAL_Stop(void);
void CyXTAL_32KHZ_Start(void);
void CyXTAL_32KHZ_Stop(void);
uint8 CyXTAL_32KHZ_SetPowerMode(uint8 mode);
uint8 CyILO_SetPowerMode(uint8 mode);

/* CYLIB_SYSTEM_CLOCKS */
#endif



#if defined(CYLIB_POWER_MANAGEMENT)

/* Power Managment functions. */
void CyWait(void);
void CyIdle(void);
void CySleep(void);
void CyHibernate(void);

/* CYLIB_POWER_MANAGEMENT */
#endif


#if defined(CYLIB_DEBUG)

/* prototype of a user defined function to handle asserts. */
void CyAssert(uint32 arg);

/* This define is a place holder for the user to rewrite. */
#define CYASSERT(x)             {if(!(x)) CyAssert((uint32) x);}

#if defined(__ARMCC_VERSION)

/* Generate a bkpt instruction. */
#define CYBREAK(x)              			__breakpoint(0x0)

#elif defined(__C51__)

/* Generate a bkpt instruction. */
#define CYBREAK(x)

#else

/* Generate a bkpt instruction. */
#define CYBREAK(x)              			__asm("    bkpt    1");

#endif



/* CYLIB_DEBUG */
#else

#define CYASSERT(x)
#define CYBREAK(x)

/* CYLIB_DEBUG */
#endif


#if defined(CYLIB_STRING)

/* Memory functions. */
void * cymemset(void * s, int32 c, int32 n);
void * cymemcpy(void * s1, const void * s2, int32 n);
void * cymemmove(void * s1, void * s2, int32 n);
int cymemcmp(const void * s1, const void * s2, uint32 n);
void * cymemchr(const void * s, int c, uint32 n);

/* String functions. */
/* These functions are obsolete and should not be used for development. */
char * cystrcat(char * s1, char * s2);
char * cystrcpy(char * s1, char * s2);
char * cystrncpy(char * s1, char * s2, uint32 n);
uint32 cystrlen(const char * s);
int32 cystrcmp(char * s1, char * s2);
int32 cystrncmp(const char * s1, const char * s2, uint32 n);
char * cystrchr(const char * s, char c);
char * cystrrchr(const char * s, char c);
char * cystrstr(const char * s1, const char * s2);
char * cystrncat(char * s1, const char * s2, uint32 n);
int32 cystrtol(const char * s, char * * e, int8 b);

/* CYLIB_STRING */
#endif


#define CYLIB_WDT           1
#if defined(CYLIB_WDT)


/* Watchdog timer registers. */
#define CYWDT_CFG                   ((reg8 *) CYDEV_PM_WDT_CFG)
#define CYWDT_CR                    ((reg8 *) CYDEV_PM_WDT_CR) 


/* Valid tick counts. */
#define CYWDT_2_TICKS               0x0 /* 2 CTW ticks ==> 4ms - 6ms. */
#define CYWDT_16_TICKS              0x1 /* 16 CTW ticks ==> 32ms - 48ms. */ 
#define CYWDT_128_TICKS             0x2 /* 128 CTW ticks ==> 256ms - 384ms. */ 
#define CYWDT_1024_TICKS            0x3 /* 1024 CTW ticks ==> 2.048s - 3.072s. */ 


/* Enables the watchdog timer, also clears the central time wheel. */
#define CYWDT_ENABLE                {*CYWDT_CFG = 0x90 | *CYWDT_CFG;}

/* Set the number of ticks before the watchdog expires. */
#define CYWDT_TICKS(Ticks)          {*CYWDT_CFG = (Ticks & 0x3) | (*CYWDT_CFG & ~0x3);}

/* Clears the Watchdog timer. */
#define CYWDT_CLEAR                 {*CYWDT_CR = 1;}

/* The system does not automatically change the interval or feed the WDT when entering sleep. */
/* This can only be set before the WDT is enabled. */
#define CYWDT_DISABLE_AUTO_FEED     {*CYWDT_CFG = 0x20 | *CYWDT_CFG;}


/* CYLIB_WDT */
#endif


#if (defined(__GNUC__))
#define CYGlobalIntEnable           {__asm("CPSIE   i");}
#define CYGlobalIntDisable          {__asm("CPSID   i");}
#endif

#if (defined(__ARMCC_VERSION))
#define CYGlobalIntEnable           {__enable_irq();}
#define CYGlobalIntDisable          {__disable_irq();} 
#endif

#ifdef CYDEV_MFGCFG_MLOGIC_CPU_SCR_CPU_SCR
#define CYDEV_HALT_CPU              CY_SET_REG8(CYDEV_MFGCFG_MLOGIC_CPU_SCR_CPU_SCR, 0x01)
#else
#define CYDEV_HALT_CPU              CY_SET_REG8(CYDEV_MFGCFG_MLOGIC_CPU_SCR, 0x01)
#endif

#ifdef CYDEV_MFGCFG_MLOGIC_REV_ID_REV_ID
#define CYDEV_CHIP_REV_ACTUAL       (CY_GET_REG8(CYDEV_MFGCFG_MLOGIC_REV_ID_REV_ID))
#else
#define CYDEV_CHIP_REV_ACTUAL       (CY_GET_REG8(CYDEV_MFGCFG_MLOGIC_REV_ID))
#endif


void CyEnableDigitalArray(void);
void CyDisableDigitalArray(void);

void CyDelay(uint32 milliseconds);
void CyDelayUs(uint16 microseconds);
void CyDelayFreq(uint32 freq);
void CyDelayCycles(uint32 cycles);

uint32 CyDisableInts(void);
void CyEnableInts(uint32 intState);
void CYDEV_FATAL_ABORT(unsigned char reason);
void CySoftwareReset(void);
void CyFlushCache(void);



/* __CYLIB_H__ */
#endif

#define CYINT_IRQ_BASE	16

#define CYINT_VECTORS       ((cyisraddress *) CYDEV_NVIC_VECT_OFFSET)
#define CYINT_PRIORITY      ((reg8 *) CYDEV_NVIC_PRI_0)
#define CYINT_ENABLE        ((reg32 *) CYDEV_NVIC_SETENA0)
#define CYINT_CLEAR         ((reg32 *) CYDEV_NVIC_CLRENA0)
#define CYINT_SET_PEND      ((reg32 *) CYDEV_NVIC_SETPEND0)
#define CYINT_CLR_PEND      ((reg32 *) CYDEV_NVIC_CLRPEND0)


cyisraddress CyIntSetSysVector(uint8 number, cyisraddress address);
cyisraddress CyIntGetSysVector(uint8 number);


cyisraddress CyIntSetVector(uint8 number, cyisraddress address);
cyisraddress CyIntGetVector(uint8 number);

void CyIntSetPriority(uint8 number, uint8 priority);
uint8 CyIntGetPriority(uint8 number);

void CyIntEnable(uint8 number);
uint8 CyIntGetState(uint8 number);
void CyIntDisable(uint8 number);

void CyIntSetPending(uint8 number);
void CyIntClearPending(uint8 number);

