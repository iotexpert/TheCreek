/******************************************************************************
* File Name: CyLib.h
* Version `$CY_VERSION_MAJOR`.`$CY_VERSION_MINOR`
*
*  Description:
*
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



#if defined(CYLIB_DEBUG)

/* prototype of a user defined function to handle asserts. */
void CyAssert(uint32 arg);

/* This define is a place holder for the user to rewrite. */
#define CYASSERT(x)             {if(x) CyAssert((uint32) x);}
#define CYBREAK(x)              __asm("    bkpt    1");

/* CYLIB_DEBUG */
#else

#define CYASSERT(x)
#define CYBREAK(x)

/* CYLIB_DEBUG */
#endif


#define LONG_MAX                0x7FFFFFFFu
#define LONG_MIN                (-LONG_MAX - 1L)
#define ULONG_MAX               0xFFFFFFFFu  /* maximum unsigned long value */


#if defined(CYLIB_STRING)

#define isspace(x)              ((x) == ' ' || (x) == '\t' || (x) == '\n')
#define toupper(x)              ((x) - 32 * ((x) >= 'a' && (x) <= 'z')
#define isdigit(x)              ((x) >= '0' && (x) <= '9')
#define isascii(x)              ((unsigned)((x) < 0x80)
#define isalpha(x)              (((x) >= 'A' && (x) <= 'Z') || ((x) >= 'a' && (x) <= 'z'))  // NEEDS MORE WORK!!!!!!!!!!!!!!!

/* Memory functions. */
void * cymemset(void * s, int32 c, int32 n);
void * cymemcpy(void * s1, const void * s2, int32 n);
void * cymemmove(void * s1, void * s2, int32 n);
int cymemcmp(const void * s1, const void * s2, uint32 n);
void * cymemchr(const void * s, int c, uint32 n);

/* String functions. */
int8 * cystrcat(int8 * s1, int8 * s2);
int8 * cystrcpy(int8 * s1, int8 * s2);
int8 * cystrncpy(int8 * s1, int8 * s2, uint32 n);
uint32 cystrlen(const int8 * s);
int32 cystrcmp(int8 * s1, int8 * s2);
int32 cystrncmp(const int8 * s1, const int8 * s2, uint32 n);
int8 * cystrchr(const int8 * s, int8 c);
int8 * cystrrchr(const int8 * s, int8 c);
int8 * cystrstr(const int8 * s1, const int8 * s2);
int8 * cystrncat(int8 * s1, const int8 * s2, uint32 n);
int32 cystrtol(const int8 * s, int8 * * e, int8 b);

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


void CyEnableDigitalArray();
void CyDisableDigitalArray();
void CyDelay(uint32 milliSeconds);
uint32 CyDisableInts(void);
void CyEnableInts(uint32 intState);



/* __CYLIB_H__ */
#endif


