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
* Copyright 2008-2011, Cypress Semiconductor Corporation.  All rights reserved.
* You may use this file only in accordance with the license, terms, conditions,
* disclaimers, and limitations in the end user license agreement accompanying
* the software package with which this file was provided.
********************************************************************************/

#if !defined(__CYLIB_H__)
#define __CYLIB_H__

#include <string.h>
#include <limits.h>
#include <ctype.h>
#include <cytypes.h>
#include <cyfitter.h>
#include <cydevice_trm.h>
#include <cyPm.h>

#if defined(__C51__)
    #include <PSoC3_8051.h>
#endif

/* CYLIB CLOCK macros/funcs begin*/
/***************************************
*    Clock registers TSG4 
***************************************/
#define CY_CLK_IMO_CONFIG_PTR           ( (reg8 *) CYREG_CLK_IMO_CONFIG)
#define CY_CLK_IMO_CONFIG_REG           (*(reg8 *) CYREG_CLK_IMO_CONFIG)
#define CY_CLK_ILO_CONFIG_PTR           ( (reg8 *) CYREG_CLK_ILO_CONFIG)
#define CY_CLK_ILO_CONFIG_REG           (*(reg8 *) CYREG_CLK_ILO_CONFIG)
#define CY_CLK_SELECT_PTR				( (reg8 *) CYREG_CLK_SELECT)
#define CY_CLK_SELECT_REG				(*(reg8 *) CYREG_CLK_SELECT)

#define CLK_IMO_CONFIG_ENABLE			(0x01u << 31)    /*Bit 31, Master enable for IMO oscillator; 
												         Should be inverted and connected to IMO “PD” input*/
#define CLK_IMO_FREQ_BITS_MASK		    0x3Fu            /*Bit 13:8, 6 bits poisition.*/
#define CLK_IMO_FREQ_CLEAR			    (CLK_IMO_FREQ_BITS_MASK << 8)  /*Bit 13:8, FREQ, Frequency to be selected (default 24MHz).  
												                        Frequencies can be selected from 3..48MHz. */

#define CLK_ILO_CONFIG_ENABLE			(0x01u << 31)    /*Bit 31, Master enable for ILO oscillator*/

#define CLK_HFCLK_SELECT_BIT_POS		16              /* Bit 17:16	HFCLK_SEL,	Selects the source for HFCLK*/

#define CLK_HFCLK_SELECT_SET			(0x03u << 16)    /*Clear bit 17:16*/ 

#define CLK_HFLK_SELECT_MASK			(0x01u)          /*Allow only first bit, as the source for HFCLK. IMO or External Clock*/

#define CLK_HFCLK_SELECT_EXTCLK			(0x01u)          /*EXTCLK as the source for HFCLK*/

#define CLK_SYSCLK_SELECT_MASK			(0x07u)          /*Allow only three bits, as the source for SYSCLK*/

#define CLK_HFLK_FROM_SYSCLK			19              /*bits 21:19	SYSCLK_DIV	, SYSCLK Pre-Scaler Value: 
												           0: SYSCLK= HFCLK/1 
												  	       1: SYSCLK= HFCLK/2 
												  	       2: SYSCLK= HFCLK/4 
												  	        .. 
												  	       7: SYSCLK= HFCLK/128 */

#define CLK_HALFSYSCLK_SELECT_MASK      (0x01u) 	        /*Allow only first bit, as the source for HFLFSYSCLK*/
#define CLK_HALFSYSCLK_BITS_POS		    18     	        /*bit 18	SYSCLK_DIV	, SYSCLK Pre-Scaler Value. */

#define CLK_SYS_WDT_MODE_MASK 0x3u                      /*1:0 are Mode mask bits */

#define CLK_CONFIG_WDT_BITS_MASK		(0x03u << 14)    /*"Prohibits writing to WDT_* registers and 
                                                         CLK_ILO/WCO_CONFIG registerst when not equal 0.  
                                                         Requires at least two different writes to unlock.  
                                                         Writing to this field has the following effect:
                                                         0: No effect
                                                         1: Clears bit 0
                                                         2: Clears bit 1
                                                         3: Sets both bits 0 and 1"*/
#define CLK_CONFIG_WDT_BIT0             (0x01u << 14)
#define CLK_CONFIG_WDT_BIT1             (0x01u << 15)

#define CY_CLK_WDT_CONFIG_BITS2_MASK    0x1Fu           /*5 bits to configure*/
#define CY_CLK_WDT_CONFIG_BITS2_POS     24              /*24:28 bits are Counter2 BITS*/
#define CY_LOWER_16BITS_ONLY            0xFFFFu          /*lower 16bits allowed*/
#define CY_WDT_COUNTERS_MAX             3               /*There are 3 WDT counters */

#define CY_CLK_WDT_CTRLOW_PTR			( (reg8 *) CYREG_CLK_WDT_CTRLOW)
#define CY_CLK_WDT_CTRLOW_REG			(*(reg8 *) CYREG_CLK_WDT_CTRLOW) 
#define CY_CLK_WDT_CTRHIGH_PTR			( (reg8 *) CYREG_CLK_WDT_CTRHIGH)
#define CY_CLK_WDT_CTRHIGH_REG			(*(reg8 *) CYREG_CLK_WDT_CTRHIGH) 
#define CY_CLK_WDT_MATCH_PTR			( (reg8 *) CYREG_CLK_WDT_MATCH)
#define CY_CLK_WDT_MATCH_REG		    (*(reg8 *) CYREG_CLK_WDT_MATCH) 
#define CY_CLK_WDT_CONFIG_PTR 		    ( (reg8 *) CYREG_CLK_WDT_CONFIG)
#define CY_CLK_WDT_CONFIG_REG 			(*(reg8 *) CYREG_CLK_WDT_CONFIG)
#define CY_CLK_WDT_CONTROL_PTR			( (reg8 *) CYREG_CLK_WDT_CONTROL) 
#define CY_CLK_WDT_CONTROL_REG 			(*(reg8 *) CYREG_CLK_WDT_CONTROL)
#define CY_CLK_ILO_TRIM_PTR 			( (reg8 *) CYREG_CLK_ILO_TRIM)
#define CY_CLK_ILO_TRIM_REG				(*(reg8 *) CYREG_CLK_ILO_TRIM)
#define CY_CLK_IMO_TRIM1_PTR			( (reg8 *) CYREG_CLK_IMO_TRIM1) 
#define CY_CLK_IMO_TRIM1_REG 			(*(reg8 *) CYREG_CLK_IMO_TRIM1)
#define CY_CLK_IMO_TRIM2_PTR			( (reg8 *) CYREG_CLK_IMO_TRIM2)
#define CY_CLK_IMO_TRIM2_REG 			(*(reg8 *) CYREG_CLK_IMO_TRIM2)
#define CY_CLK_IMO_TRIM3_PTR			( (reg8 *) CYREG_CLK_IMO_TRIM3)
#define CY_CLK_IMO_TRIM2_REG 			(*(reg8 *) CYREG_CLK_IMO_TRIM2)


/***************************************
*    Function Prototypes
***************************************/
void CySysClkImoStart(void);
void CySysClkImoStop(void);
void CySysClkIloStart(void);
void CySysClkIloStop(void);
void CySysClkWriteHfclkDirect (uint8 clkSelect);
void CySysClkWriteSysclkDiv (uint8 divider);
void CySysClkWriteHalfSysclkDiv (uint8 divider);
void CySysClkWriteImoFreq (uint8 freq);


/* CYLIB WDT macros/funcs begin*/
/***************************************
*    API Constants
***************************************/
#define CY_SYS_WDT_MODE_NONE	         0               /*Free running*/
#define CY_SYS_WDT_MODE_INT		         1               /*Interrupt generated on match for counter 0 and 1, 
                                                          and on bit toggle for counter 2*/
#define CY_SYS_WDT_MODE_RESET	         2               /* Reset on match (valid for counter 0 and 1 only)*/
#define CY_SYS_WDT_MODE_INT_RESET	     3               /*Generate an interrupt.  Generate a reset on 
			                                              the 3rd unhandled interrupt.  
		                                    	          (valid for counter 0 and 1 only)*/

#define CY_SYS_WDT_COUNTER0_ENABLE	    1<<0			/*Counter 0*/
#define CY_SYS_WDT_COUNTER1_ENABLE		1<<8			/*Counter 1*/
#define CY_SYS_WDT_COUNTER2_ENABLE		1<<16			/*Counter 2*/

#define CY_SYS_WDT_CASCADE_NONE         0x0u            /*Neither*/
#define CY_SYS_WDT_CASCADE_01	        1<<3			/*Cascade 01*/
#define CY_SYS_WDT_CASCADE_12	        1<<11           /*Cascade 12*/

#define CY_SYS_WDT_COUNTER0_INT	        1<<2	        /*Interrupt of WDT 0*/
#define CY_SYS_WDT_COUNTER1_INT	        1<<10	        /*Interrupt of WDT 1*/
#define CY_SYS_WDT_COUNTER2_INT	        1<<18	        /*Interrupt of WDT 2*/

#define CY_SYS_WDT_COUNTER0_RESET	    1<<3	        /*Counter of WDT 0*/
#define CY_SYS_WDT_COUNTER1_RESET	    1<<11	        /*Counter of WDT 1*/
#define CY_SYS_WDT_COUNTER2_RESET	    1<<19	        /*Counter of WDT 2*/

/***************************************
*    Defines for clock
***************************************/
/*pre-scaler divide for SYSCLK */
#define CY_SYS_CLK_SYSCLK_DIV1		    0               /*Divider 1*/
#define CY_SYS_CLK_SYSCLK_DIV2		    1               /*Divider 2*/
#define CY_SYS_CLK_SYSCLK_DIV4		    2               /*Divider 3*/
#define CY_SYS_CLK_SYSCLK_DIV8		    3               /*Divider 4*/
#define CY_SYS_CLK_SYSCLK_DIV16		    4               /*Divider 5*/
#define CY_SYS_CLK_SYSCLK_DIV32		    5               /*Divider 6*/
#define CY_SYS_CLK_SYSCLK_DIV64		    6               /*Divider 7*/
#define CY_SYS_CLK_SYSCLK_DIV128	    7               /*Divider 8*/

/*Pre-scalar divider for HALFSYSCLK frequency*/
#define CY_SYS_CLK_HALFSYSCLK_DIV1	    0               /*Divider 1*/
#define CY_SYS_CLK_HALFSYSCLK_DIV2	    1               /*Divider 1*/

#define CY_SYS_CLK_HFCLK_IMO	        0               /*IMO*/
#define CY_SYS_CLK_HFCLK_EXTCLK	        1               /*External clock pin*/

/***************************************
*    Function Prototypes
***************************************/
void CySysWdtLock(void);
void CySysWdtUnlock(void);
void CySysWdtWriteMode(uint8 counterNum, uint8 mode);
uint8 CySysWdtReadMode(uint8 counterNum);
void CySysWdtWriteClearOnMatch(uint8 counterNum, uint8 enable);
uint8 CySysWdtReadClearOnMatch(uint8 counterNum);
void CySysWdtEnable(uint32 counterMask);
void CySysWdtDisable(uint32 counterMask);
void CySysWdtWriteCascade(uint32 cascadeMask);
uint32 CySysWdtReadCascade(void);
void CySysWdtWriteMatch(uint8 counterNum, uint16 match);
void CySysWdtWriteBits2(uint8 bits);
uint8 CySysWdtReadBits2(void);
uint16 CySysWdtReadMatch(uint8 counterNum);
uint32 CySysWdtReadCount(uint8 counterNum);
uint32 CySysWdtReadIntr(void);
void CySysWdtClearIntr(uint32 counterMask);
void CySysWdtResetCount(uint32 counterMask);


/* CYLIB SYSTEM macros/funcs begin*/
void CyHalt(uint8 reason);

#if !defined(NDEBUG)

    /*******************************************************************************
    * Macro Name: CyAssert
    ********************************************************************************
    * Summary:
    *   Macro that evaluates the expression and if it is false (evaluates to 0) 
    *   then the processor is halted.
    *   
    *   This macro is evaluated unless NDEBUG is defined.  
    *   If NDEBUG is defined, then no code is generated for this macro.  
    *   NDEBUG is defined by default for a Release build setting and not defined for 
    *    a Debug build setting.
    *
    *
    * Parameters:
    *   expr: Logical expression.  Asserts if false.
    *
    *
    * Return:
    *   void.
    *
    *******************************************************************************/
    #define CYASSERT(x)             {if(!(x)) CyHalt((uint32) (x));}

#else /* NDEBUG */

    #define CYASSERT(x)

#endif /* NDEBUG */

/* CYLIB SYSTEM macros/funcs end */

/* System function registers. */
#define CY_CM0_AIRCR_PTR           ( (reg8 *)CYREG_CM0_AIRCR)
#define CY_CM0_AIRCR_REG           (*(reg8 *)CYREG_CM0_AIRCR)
#define CY_SOFTWARE_RESET          (0x05FA0004u)

#if defined(__ARMCC_VERSION)
#define CyGlobalIntEnable           {__enable_irq();}
#define CyGlobalIntDisable          {__disable_irq();}
#elif defined(__GNUC__)
#define CyGlobalIntEnable           {__asm("CPSIE   i");}
#define CyGlobalIntDisable          {__asm("CPSID   i");}
#elif defined(__C51__)

#if ((CYDEV_CHIP_MEMBER_USED == CYDEV_CHIP_MEMBER_3A) && (CYDEV_CHIP_REVISION_USED == CYDEV_CHIP_REVISION_3A_ES3))
#define CyGlobalIntEnable           {EA = 1; INTERRUPT_ENABLE_IRQ}
#define CyGlobalIntDisable          {INTERRUPT_DISABLE_IRQ; CY_NOP; EA = 0;}
#else
#define CyGlobalIntEnable           {EA = 1;}
#define CyGlobalIntDisable          {EA = 0;}
#endif /*CYDEV_CHIP_REVISION_3A_ES3*/

#endif
/***************************************
*    System Function Prototypes
***************************************/
void CyDelay(uint32 milliseconds) CYREENTRANT;
void CyDelayUs(uint16 microseconds) CYREENTRANT;
void CyDelayFreq(uint32 freq) CYREENTRANT;
void CyDelayCycles(uint32 cycles);

void CySoftwareReset(void);
uint8 CyEnterCriticalSection(void);
void CyExitCriticalSection(uint8 savedIntrStatus);

/***************************************
*    Interrupt API Constants
***************************************/
#define RESET_CR2                   ((reg8 *) CYREG_RESET_CR2)
#if CY_PSOC5
    #define CYINT_IRQ_BASE          16
    #define CYINT_VECT_TABLE        ((cyisraddress **) CYREG_NVIC_VECT_OFFSET)
    #define CYINT_PRIORITY          ((reg8 *) CYREG_NVIC_PRI_0)
    #define CYINT_ENABLE            ((reg32 *) CYREG_NVIC_SETENA0)
    #define CYINT_CLEAR             ((reg32 *) CYREG_NVIC_CLRENA0)
    #define CYINT_SET_PEND          ((reg32 *) CYREG_NVIC_SETPEND0)
    #define CYINT_CLR_PEND          ((reg32 *) CYREG_NVIC_CLRPEND0)
    #define CACHE_CC_CTL            ((reg16 *) CYREG_CACHE_CC_CTL)
#elif CY_PSOC4
    #define CYINT_IRQ_BASE          16
    #define CYINT_VECT_TABLE        ((cyisraddress **) CYDEV_SRAM_BASE)
    #define CYINT_PRIORITY          ((reg32 *) CYREG_CM0_IPR0)
    #define CYINT_ENABLE            ((reg32 *) CYREG_CM0_ISER)
    #define CYINT_CLEAR             ((reg32 *) CYREG_CM0_ICER)
    #define CYINT_SET_PEND          ((reg32 *) CYREG_CM0_ISPR)
    #define CYINT_CLR_PEND          ((reg32 *) CYREG_CM0_ICPR)
#elif CY_PSOC3
    #define CYINT_IRQ_BASE          0
    #define CYINT_VECT_TABLE        ((cyisraddress CYXDATA *) CYREG_INTC_VECT_MBASE)
    #define CYINT_PRIORITY          ((reg8 *) CYREG_INTC_PRIOR0)
    #define CYINT_ENABLE            ((reg8 *) CYREG_INTC_SET_EN0)
    #define CYINT_CLEAR             ((reg8 *) CYREG_INTC_CLR_EN0)
    #define CYINT_SET_PEND          ((reg8 *) CYREG_INTC_SET_PD0)
    #define CYINT_CLR_PEND          ((reg8 *) CYREG_INTC_CLR_PD0)
#endif

#define CYINT_CLEAR_DISABLE_ALL     0xFFFFFFFF
#define CYINT_ENABLE_RANGE_MASK		0x0F

#if CY_PSOC4
#define NUM_INTERRUPTS          16
#define MIN_PRIORITY            3
/*Register n contains priorities for interrupts N=4n .. 4n+3 at Bits 7:6, 15:14, 23:22, 31:30
*/
#define CYINT_PRIORITY_SHIFT(number)           (6 + (8 * ((number) % 4))) 
#endif

/***************************************
*    Interrupt Function Prototypes
***************************************/
cyisraddress CyIntSetSysVector(uint8 number, cyisraddress address)  `=ReentrantKeil("CyIntSetVector")`;
cyisraddress CyIntGetSysVector(uint8 number) `=ReentrantKeil("CyIntGetVector")`;

cyisraddress CyIntSetVector(uint8 number, cyisraddress address) `=ReentrantKeil("CyIntSetVector")`;
cyisraddress CyIntGetVector(uint8 number) `=ReentrantKeil("CyIntGetVector")`;

void CyIntSetPriority(uint8 number, uint8 priority) `=ReentrantKeil("CyIntSetPriority")`;
uint8 CyIntGetPriority(uint8 number) `=ReentrantKeil("CyIntGetPriority")`;

void CyIntEnable(uint8 number) `=ReentrantKeil("CyIntEnable")`;
uint8 CyIntGetState(uint8 number) `=ReentrantKeil("CyIntGetState")`;
void CyIntDisable(uint8 number) `=ReentrantKeil("CyIntDisable")`;

void CyIntSetPending(uint8 number) `=ReentrantKeil("CyIntSetPending")`;
void CyIntClearPending(uint8 number) `=ReentrantKeil("CyIntClearPending")`;

uint32 CyDisableInts(void) `=ReentrantKeil("CyDisableInts")`;
void CyEnableInts(uint32 mask) `=ReentrantKeil("CyEnableInts")`;

/* __CYLIB_H__ */
#endif
