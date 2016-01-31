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




// Temporary until a real lib is created. !!!!!!!!!!
#define CYLIB_STRING    1




#if !defined(NULL)

#define NULL                                ((void *) 0x0u)

#endif








#if defined(CYLIB_POWER_MANAGMENT)

/* Power Managment functions. */
void CyWait(void);
void CyIdle(void);
void CySleep(void);
void CyHibernate(void);




/* CYLIB_POWER_MANAGMENT */
#endif



#if defined(CYLIB_DEBUG)

/* prototype of a user defined function to handle asserts. */
void CyAssert(uint32 arg);

/* This define is a place holder for the user to rewrite. */
#define CYASSERT(x)                         {if(x) CyAssert((uint32) x);}
#define CYBREAK(x)                          CYASSERT(1)

//#define CYASSERT(x) CyAssert x

/* CYLIB_DEBUG */
#else

#define CYASSERT(x)
#define CYBREAK(x)

/* CYLIB_DEBUG */
#endif


#define LONG_MAX                            0x7FFFFFFFu
#define LONG_MIN                            (-LONG_MAX - 1L)
#define ULONG_MAX                           0xFFFFFFFFu  /* maximum unsigned long value */



#if defined(CYLIB_STRING)

#define isspace(x)                          ((x) == ' ' || (x) == '\t' || (x) == '\n')
#define toupper(x)                          ((x) - 32 * ((x) >= 'a' && (x) <= 'z')
#define isdigit(x)                          ((x) >= '0' && (x) <= '9')
#define isascii(x)                          ((unsigned)((x) < 0x80)
#define isalpha(x)                          (((x) >= 'A' && (x) <= 'Z') || ((x) >= 'a' && (x) <= 'z'))  // NEEDS MORE WORK!!!!!!!!!!!!!!!

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


#if defined(CYLIB_CACHE)


/* Cache memory spaces. */
#define CACHE_ADRR_SPACE_CONTROL            0x004800
#define CACHE_ADRR_SPACE_FLASH              0x100000
#define CACHE_ADRR_SPACE_ECC                0x080000
#define CACHE_ADRR_SPACE_CACHESRAM          0x030000
#define CACHE_ADRR_SPACE_FLASHHIDDENROWS    0x0C0080


/* A row is 64 bits or 8 bytes. */
#define CACHE_ROW_SIZEOF                    8


/* A line is 8 rows. */
#define CACHE_LINE_SIZEOF                   (8 * CACHE_ROW_SIZEOF)

#define CACHE_CTLREG                        ((reg8 *) CYDEV_CACHE_CR)

#define CR_FLASH_CYCLES(x)                  (x << 6)      // Specifies the number of clock cycles the cache will wait before it samples data coming back from Flash. 0x0: 5 cycles. -> 73 MHz - 100 MHz 0x1: 2 cycle. -> 1Hz - 24 MHz 0x2: 3 cycles. -> 25 MHz - 48 MHz 0x3: 4 cycles. -> 49 MHz - 72 MHz The following stalls are computed using: 1-register cycle to break timing path from 8051-cache. Flash access time: 32.5 ns ECC enable delay: 4.4 ns Margin delay: 4.1 ns 
#define CR_CACHEABLE                        (1 << 4)      // 0x0: Disables HW ability to write data coming from the flash to the cache SRAM. 0x1: Enables HW ability to write data coming from the flash to the cache SRAM. 
#define CR_UNLOCK                           (1 << 3)      // This is a 'virtual flop' that clears immediately. Reading this bit will always return 0. 0x1: 0x1: Writing 1 will cause all lock bits in the tags and itself to be cleared the following cycle. 
#define CR_CACHE_INV                        (1 << 2)      // This is a 'virtual flop' that clears immediately. Reading this bit will always return 0. 0x1: Writing 1 will cause all valid bits in the tags, the valid bit for the 8 byte CPU interface and itself to be cleared the following cycle. 
#define CR_PHUB_INV                         (1 << 1)      // This is a 'virtual flop' that clears immediately. Reading this bit will always return 0. 0x1: Writing 1 will cause the valid bit for the 8 byte PHUB interface read buffer and itself to be cleared the following cycle.  
#define CACHE_CR_LP_MODE                    ((reg8 *) CYDEV_CACHE_CR + 1) // Specifies the number of cache hits after which the cache controller will assert SLEEP_REQ. 0xff: Low Power Mode disabled. 

#define CACHE_SREG                          ((reg8 *) CYDEV_CACHE_SR) // Reports status of flush operation RO!
#define CACHE_TAG_SREG                      ((reg16 *) CYDEV_CACHE_TAG_SR) // Vector formed of valid and lock bits for each tag.
#define CACHE_TAG                           ((reg32 *) CYDEV_CACHE_TAG0) // Valid, lock and address bits for line i, i = 0..7

#define CC_TAG_LOCK                         (1 << 1)
#define TAG_VALID                           (1 << 0)

#define CACHE_ENABLE                        {*CACHE_CTLREG |= CR_CACHEABLE;}
#define CACHE_DISABLE                       {*CACHE_CTLREG &= ~CR_CACHEABLE;}
#define CACHE_ENTER                         CACHE_DISABLE
#define CACHE_EXIT                          {*CACHE_CTLREG |= CR_CACHE_INV | CR_PHUB_INV; *CACHE_CTLREG |= CR_CACHEABLE;}
#define CACHE_FLUSH                         {*CACHE_CTLREG |= CR_UNLOCK | CR_CACHE_INV | CR_PHUB_INV;}


/* Cache Controller API. */
cystatus CyCacheLoadLockedLine(uint8 line, void * address);
cystatus CyCacheUnlockLine(uint8 line);


/* CYLIB_CACHE */
#endif



#define CYLIB_WDT			1
#if defined(CYLIB_WDT)

/* Watchdog timer registers. */
#define CYWDT_CFG                   		((reg8 *) CYDEV_PM_WDT_CFG)
#define CYWDT_CR                    		((reg8 *) CYDEV_PM_WDT_CR) 


/* Valid tick counts. */
#define CYWDT_2_TICKS               		0x0 /* 2 CTW ticks ==> 4ms - 6ms. */
#define CYWDT_16_TICKS              		0x1 /* 16 CTW ticks ==> 32ms - 48ms. */ 
#define CYWDT_128_TICKS             		0x2 /* 128 CTW ticks ==> 256ms - 384ms. */ 
#define CYWDT_1024_TICKS            		0x3 /* 1024 CTW ticks ==> 2.048s - 3.072s. */ 


/* Enables the watchdog timer, also clears the central time wheel. */
#define CYWDT_ENABLE               			{*CYWDT_CFG = 0x90 | *CYWDT_CFG;}

/* Set the number of ticks before the watchdog expires. */
#define CYWDT_TICKS(Ticks)         			{*CYWDT_CFG = (Ticks & 0x3) | (*CYWDT_CFG & ~0x3);}

/* Clears the Watchdog timer. */
#define CYWDT_CLEAR                			{*CYWDT_CR = 1;}

/* The system does not automatically change the interval or feed the WDT when entering sleep. */
/* This can only be set before the WDT is enabled. */
#define CYWDT_DISABLE_AUTO_FEED    			{*CYWDT_CFG = 0x20 | *CYWDT_CFG;}


/* CYLIB_WDT */
#endif


#define CYGlobalIntEnable    				{EA = 1;}
#define CYGlobalIntDisable   				{EA = 0;}


void CyEnableDigitalArray();
void CyDisableDigitalArray();
void CyDelay(uint32 milliSeconds);
uint32 CyDisableInts(void);
void CyEnableInts(uint32 intState);


/* __CYLIB_H__ */
#endif






