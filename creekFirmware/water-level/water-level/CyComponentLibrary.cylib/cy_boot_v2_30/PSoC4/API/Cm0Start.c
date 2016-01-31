/*******************************************************************************
* File Name: Cm0Start.c  
* Version `$CY_MAJOR_VERSION`.`$CY_MINOR_VERSION`
*
*  Description:
*  Startup code for the ARM CM0.
*
*
*
*  Note:
*
*   
*
********************************************************************************
* Copyright 2010-2011, Cypress Semiconductor Corporation.  All rights reserved.
* You may use this file only in accordance with the license, terms, conditions, 
* disclaimers, and limitations in the end user license agreement accompanying 
* the software package with which this file was provided.
********************************************************************************/

#include <CYDEVICE_TRM.H>
#include <CYTYPES.H>
#include <CYFITTER_CFG.H>
#include <CYLIB.H>
#include <CYFITTER.H>

#if CY_PSOC4
    #define NUM_INTERRUPTS          16
#endif
#define NUM_VECTORS                 (CYINT_IRQ_BASE + NUM_INTERRUPTS)
#define CPUSS_CONFIG                ((reg32 *) CYREG_CPUSS_CONFIG)

#if defined(__ARMCC_VERSION)
    #define INITIAL_STACK_POINTER (cyisraddress)(uint32)&Image$$ARM_LIB_STACK$$ZI$$Limit 
#elif defined (__GNUC__)
    #define INITIAL_STACK_POINTER __cs3_stack
#endif

/* extern functions */
extern void CyBtldr_CheckLaunch(void);
/* function prototypes */
void initialize_psoc(void);

/*******************************************************************************
* Function Name: IntDefaultHandler
********************************************************************************
* Summary:
*   This function is called for all interrupts, other than reset, that get
*   called before the system is setup.
*
*
* Parameters:
*   void.
*
*
* Return:
*   void.
*   
*
* Theory:
*   Any value other than zero is acceptable.
*
*
*******************************************************************************/
__attribute__ ((noreturn))
CY_ISR(IntDefaultHandler)
{
    /* We should never get here. If we do, a serious problem occured, so 
     * go into an infinite loop. 
     */
    while(1);
}

#if defined(__ARMCC_VERSION)

/* Local function for the device reset. */
extern void Reset(void);

/* Application entry point. */
extern void $Super$$main(void);

/* Linker-generated Stack Base addresses, Two Region and One Region */
extern unsigned long Image$$ARM_LIB_STACK$$ZI$$Limit;

/* RealView C Library initialization. */
extern int __main(void);


/*******************************************************************************
* Function Name: Reset
********************************************************************************
* Summary:
*   This function handles the reset interrupt for the RVDS/MDK toolchains.  
*   This is the first bit of code that is executed at startup.
*   
*
* Parameters:
*   void.
*
*
* Return:
*   void.
*   
*
*******************************************************************************/
__asm void Reset(void)
{
	PRESERVE8 
	EXTERN |Image$$ARM_LIB_STACK$$ZI$$Limit|
	EXTERN __main
	EXTERN CyBtldr_CheckLaunch
	LDR R3, =|Image$$ARM_LIB_STACK$$ZI$$Limit|
    MOV SP, R3
#if (CYDEV_BOOTLOADER_ENABLE)
	BL  CyBtldr_CheckLaunch
#endif /* CYDEV_BOOTLOADER_ENABLE */
	LDR R0, =__main
    BX R0

    ALIGN
}


/*******************************************************************************
* Function Name: $Sub$$main
********************************************************************************
* Summary:
*   This function is called imediatly before the users main
*
*
* Parameters:
*   void.
*
*
* Return:
*   void.
*   
*
*******************************************************************************/
__attribute__ ((noreturn))
void $Sub$$main(void)
{
    initialize_psoc();

    /* Call original main */
    $Super$$main();

    /* If main returns it is undefined what we should do. */
    while (1);
}

#elif defined(__GNUC__)

extern void __cs3_stack(void);
extern void __cs3_start_c(void);



/*******************************************************************************
* Function Name: Reset
********************************************************************************
* Summary:
*   This function handles the reset interrupt for the GCC toolchain.  This is 
*   the first bit of code that is executed at startup.
*
*
* Parameters:
*   void.
*
*
* Return:
*   void.
*
*
*******************************************************************************/
__attribute__ ((naked))
void Reset(void)
{
    __asm volatile(
		  "    MOV sp, %0\n"
#if (CYDEV_BOOTLOADER_ENABLE)
		  "    BL  CyBtldr_CheckLaunch\n"
#endif /* CYDEV_BOOTLOADER_ENABLE */
		  : : "r" (__cs3_stack));
    __asm volatile(
		  "    BX   %0\n"
		  : : "r" (__cs3_start_c));
}

#endif /* __GNUC__ */

/*******************************************************************************
*
* Ram Interrupt Vector table storage area. Must be placed at 0x20000000.
*
*******************************************************************************/
__attribute__ ((section(".ramvectors")))
cyisraddress CyRamVectors[NUM_VECTORS];

/*******************************************************************************
*
* Rom Interrupt Vector table storage area. Must be 256-byte aligned.
*
*******************************************************************************/
#if defined(__ARMCC_VERSION)
    #pragma diag_suppress 1296
#endif
__attribute__ ((section(".romvectors")))
const cyisraddress RomVectors[NUM_VECTORS] =
{
    INITIAL_STACK_POINTER,                                              /* The initial stack pointer  0 */
    (cyisraddress)Reset,                                                /* The reset handler          1 */
    IntDefaultHandler,                                                  /* The NMI handler            2 */
    IntDefaultHandler,                                                  /* The hard fault handler     3 */
    IntDefaultHandler,                                                  /* The MPU fault handler      4 */
    IntDefaultHandler,                                                  /* The bus fault handler      5 */
    IntDefaultHandler,                                                  /* The usage fault handler    6 */
    IntDefaultHandler,                                                  /* Reserved                   7 */
    IntDefaultHandler,                                                  /* Reserved                   8 */
    IntDefaultHandler,                                                  /* Reserved                   9 */
    IntDefaultHandler,                                                  /* Reserved                  10 */
    IntDefaultHandler,                                                  /* SVCall handler            11 */
    IntDefaultHandler,                                                  /* Debug monitor handler     12 */
    IntDefaultHandler,                                                  /* Reserved                  13 */
    IntDefaultHandler,                                                  /* The PendSV handler        14 */
    IntDefaultHandler,                                                  /* The SysTick handler       15 */
    IntDefaultHandler,                                                  /* External Interrupt(0)     16 */
    IntDefaultHandler,                                                  /* External Interrupt(1)     17 */
    IntDefaultHandler,                                                  /* External Interrupt(2)     18 */
    IntDefaultHandler,                                                  /* External Interrupt(3)     19 */
    IntDefaultHandler,                                                  /* External Interrupt(4)     20 */
    IntDefaultHandler,                                                  /* External Interrupt(5)     21 */
    IntDefaultHandler,                                                  /* External Interrupt(6)     22 */
    IntDefaultHandler,                                                  /* External Interrupt(7)     23 */
    IntDefaultHandler,                                                  /* External Interrupt(8)     24 */
    IntDefaultHandler,                                                  /* External Interrupt(9)     25 */
    IntDefaultHandler,                                                  /* External Interrupt(A)     26 */
    IntDefaultHandler,                                                  /* External Interrupt(B)     27 */
    IntDefaultHandler,                                                  /* External Interrupt(C)     28 */
    IntDefaultHandler,                                                  /* External Interrupt(D)     29 */
    IntDefaultHandler,                                                  /* External Interrupt(E)     30 */
    IntDefaultHandler,                                                  /* External Interrupt(F)     31 */
#if (NUM_INTERRUPTS > 16)
    IntDefaultHandler,                                                  /* External Interrupt(10)    32 */
    IntDefaultHandler,                                                  /* External Interrupt(11)    33 */
    IntDefaultHandler,                                                  /* External Interrupt(12)    34 */
    IntDefaultHandler,                                                  /* External Interrupt(13)    35 */
    IntDefaultHandler,                                                  /* External Interrupt(14)    36 */
    IntDefaultHandler,                                                  /* External Interrupt(15)    37 */
    IntDefaultHandler,                                                  /* External Interrupt(16)    38 */
    IntDefaultHandler,                                                  /* External Interrupt(17)    39 */
    IntDefaultHandler,                                                  /* External Interrupt(18)    40 */
    IntDefaultHandler,                                                  /* External Interrupt(19)    41 */
    IntDefaultHandler,                                                  /* External Interrupt(1A)    42 */
    IntDefaultHandler,                                                  /* External Interrupt(1B)    43 */
    IntDefaultHandler,                                                  /* External Interrupt(1C)    44 */
    IntDefaultHandler,                                                  /* External Interrupt(1D)    45 */
    IntDefaultHandler,                                                  /* External Interrupt(1E)    46 */
    IntDefaultHandler                                                   /* External Interrupt(1F)    47 */
#endif
};

/*******************************************************************************
* Function Name: initialize_psoc
********************************************************************************
* Summary:
*   This function used to initialize the PSoC chip before calling main.
*
*
* Parameters:
*   void.
*
*
* Return:
*   void.
*
*
*******************************************************************************/
#if (defined(__GNUC__) && !defined(__ARMCC_VERSION))
__attribute__ ((constructor(101)))
#endif
void initialize_psoc(void)
{
    uint32 index;
	
    /* Set Ram interrupt vectors to default functions. */
    for (index = 0; index < NUM_VECTORS; index++)
    {
        CyRamVectors[index] = RomVectors[index];
    }

    /* Point Vector Table at the RAM vector table. */
#if CYDEV_CONFIG_READ_ACCELERATOR
    *CPUSS_CONFIG = 1;
#else
    *CPUSS_CONFIG = 3;
#endif
    
    /* Initialize the configuration registers. */
    cyfitter_cfg();
}
