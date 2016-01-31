/*******************************************************************************
* File Name: Cm3Start.c  
* Version 1.30
*
*  Description:
*  Startp code for the ARM CM3.
*
*
*
*  Note:
*
*   
*
********************************************************************************
* Copyright 2008-2009, Cypress Semiconductor Corporation.  All rights reserved.
* You may use this file only in accordance with the license, terms, conditions, 
* disclaimers, and limitations in the end user license agreement accompanying 
* the software package with which this file was provided.
********************************************************************************/

#include <CYDEVICE_TRM.H>
#include <CYTYPES.H>
#include <CYFITTER_CFG.H>
#include <CYDMAC.H>
#include <CYFITTER.H>


#define NUM_INTERRUPTS              32
#if !defined(CYINT_VECTORS)
#define CYINT_VECTORS       ((cyisraddress *) CYDEV_NVIC_VECT_OFFSET)
#endif
#define NVIC_APINT                  ((volatile unsigned long *) CYREG_NVIC_APPLN_INTR)
#define NVIC_APINT_PRIGROUP_3_5     0x00000400  // Priority group 3.5 split
#define NVIC_APINT_VECTKEY          0x05FA0000  // This key is required in order to write the NVIC_APINT register 

/* extern functions */
extern void CyBtldr_CheckLaunch(void);

/* Local function for the device reset. */
void Reset(void);

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
void IntDefaultHandler(void)
{
    /* We should never get here. */

    /* Go into an infinite loop. */
    while(1)
    {
    }
}

/*******************************************************************************
* Function Name: pvt_CheckBootloader
********************************************************************************
* Summary:
*   This function is called before configuring a bootloader device. This function
*	may never return, instead jumping control directly to the Bootloader's
*	associated Boot Loadable project in flash.
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
*   May never return.
*
*
*******************************************************************************/
void pvt_CheckBootloader()
{
#if (CYDEV_BOOTLOADER_ENABLE)
	/* if second MSB of reset status register is high then we are to do a bootload operation
	 * so we should jump to CONFIGURE
	 */
	uint8 *resetSR0 = (uint8*)CYDEV_RESET_SR0;
	if (((*resetSR0) & 0x40) != 0x40) /* if CYDEV_RESET_SR0 != 0x40 check to see if app should be launched */
		CyBtldr_CheckLaunch();
#endif /* CYDEV_BOOTLOADER_ENABLE */
}

#if defined(__ARMCC_VERSION)


/* Application entry point. */
extern void $Super$$main(void);

/* Linker-generated Stack Base addresses, Two Region and One Region */
extern unsigned long Image$$ARM_LIB_STACK$$ZI$$Limit;

/* RealView C Library initialization. */
extern int __main(void);



//Image$$ARM_LIB_STACK$$Base
//Image$$ARM_LIB_STACK$$ZI$$Limit
//Image$$ARM_LIB_HEAP$$Base
//Image$$ARM_LIB_HEAP$$ZI$$Limit

/*******************************************************************************
*
* Default Ram Interrupt Vector table storage area.
*
*******************************************************************************/
#pragma arm section rwdata="ramvectors"

__attribute__ ((section(".ramvectors"))) cyisraddress CyRamVectors[NUM_INTERRUPTS];

/*******************************************************************************
*
* Default Rom Interrupt Vector table.
*
*******************************************************************************/
//#pragma arm section rodata="RomVectors" 

#pragma diag_suppress 1296
__attribute__ ((section(".romvectors")))
void (* RomVectors[])(void) =
{
    (void (*)(void))((unsigned long) &Image$$ARM_LIB_STACK$$ZI$$Limit), /* The initial stack pointer    */
    Reset,                                                              /* The reset handler          0 */
    IntDefaultHandler,                                                  /* The NMI handler            1 */
    IntDefaultHandler,                                                  /* The hard fault handler     2 */
    IntDefaultHandler,                                                  /* The MPU fault handler      3 */
    IntDefaultHandler,                                                  /* The bus fault handler      4 */
    IntDefaultHandler,                                                  /* The usage fault handler    5 */
    0,                                                                  /* Reserved                   6 */
    0,                                                                  /* Reserved                   7 */
    0,                                                                  /* Reserved                   8 */
    0,                                                                  /* Reserved                   9 */
    IntDefaultHandler,                                                  /* SVCall handler            10 */
    IntDefaultHandler,                                                  /* Debug monitor handler     11 */
    0,                                                                  /* Reserved                  12 */
    IntDefaultHandler,                                                  /* The PendSV handler        13 */
    IntDefaultHandler,                                                  /* The SysTick handler       14 */
    IntDefaultHandler,                                                  /* External Interrupt(0)     15 */
    IntDefaultHandler,                                                  /* External Interrupt(1)     16 */
    IntDefaultHandler,                                                  /* External Interrupt(2)     17 */
    IntDefaultHandler,                                                  /* External Interrupt(3)     18 */
    IntDefaultHandler,                                                  /* External Interrupt(4)     19 */
    IntDefaultHandler,                                                  /* External Interrupt(5)     20 */
    IntDefaultHandler,                                                  /* External Interrupt(6)     21 */
    IntDefaultHandler,                                                  /* External Interrupt(7)     22 */
    IntDefaultHandler,                                                  /* External Interrupt(8)     23 */
    IntDefaultHandler,                                                  /* External Interrupt(9)     24 */
    IntDefaultHandler,                                                  /* External Interrupt(A)     25 */
    IntDefaultHandler,                                                  /* External Interrupt(B)     26 */
    IntDefaultHandler,                                                  /* External Interrupt(C)     27 */
    IntDefaultHandler,                                                  /* External Interrupt(D)     28 */
    IntDefaultHandler,                                                  /* External Interrupt(E)     29 */
    IntDefaultHandler,                                                  /* External Interrupt(F)     30 */
    IntDefaultHandler,                                                  /* External Interrupt(10)    31 */
    IntDefaultHandler,                                                  /* External Interrupt(11)    32 */
    IntDefaultHandler,                                                  /* External Interrupt(12)    33 */
    IntDefaultHandler,                                                  /* External Interrupt(13)    34 */
    IntDefaultHandler,                                                  /* External Interrupt(14)    35 */
    IntDefaultHandler,                                                  /* External Interrupt(15)    36 */
    IntDefaultHandler,                                                  /* External Interrupt(16)    37 */
    IntDefaultHandler,                                                  /* External Interrupt(17)    38 */
    IntDefaultHandler,                                                  /* External Interrupt(18)    39 */
    IntDefaultHandler,                                                  /* External Interrupt(19)    40 */
    IntDefaultHandler,                                                  /* External Interrupt(1A)    41 */
    IntDefaultHandler,                                                  /* External Interrupt(1B)    42 */
    IntDefaultHandler,                                                  /* External Interrupt(1C)    43 */
    IntDefaultHandler,                                                  /* External Interrupt(1D)    44 */
    IntDefaultHandler,                                                  /* External Interrupt(1E)    45 */
    IntDefaultHandler                                                   /* External Interrupt(1F)    46 */

};


/*******************************************************************************
* Function Name: debugEnabled
********************************************************************************
* Summary:
*   This function is called when the DebugEnable bit has been set to allow the
*   debugger to attach after a minimal amount of code has been executed.
*   
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
void debugEnabled()
{
    /* nop instruction to keep this from being optimized out */
	CY_NOP;
}


/*******************************************************************************
* Function Name: Reset
********************************************************************************
* Summary:
*   This function is called when the PSoC5 begins execution on reset.
*   
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
void Reset(void)
{
#if (CYDEV_DEBUGGING_ENABLE)
    char value = CY_GET_XTND_REG8(CYDEV_DEBUG_ENABLE_REGISTER) | CYDEV_DEBUG_ENABLE_MASK;
	CY_SET_XTND_REG8(CYDEV_DEBUG_ENABLE_REGISTER, value);
    debugEnabled();
#endif /* CYDEV_DEBUGGING_ENABLE */

#if (CYDEV_BOOTLOADER_ENABLE)
	pvt_CheckBootloader();
#endif /* CYDEV_BOOTLOADER_ENABLE */

    /* Cannot write real C here. May need to generate .s file. */

    /* Let RealView setup the libraries. */    
    __main();
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
*
*******************************************************************************/
void $Sub$$main(void)
{
    unsigned long index;

    /* Setup the M3. */

    /* Set Priority group 5. */
    /* Writes to the NVIC_APINT register require the VECTKEY in the upper half */
    *NVIC_APINT = NVIC_APINT_VECTKEY | NVIC_APINT_PRIGROUP_3_5;


    /* Set Ram interrupt vectors to default functions. */
    for(index = 0; index < NUM_INTERRUPTS; index++)
    {
        CyRamVectors[index] = IntDefaultHandler;
    }

    /* Point NVIC at the RAM vector table. */
    *CYINT_VECTORS = (cyisraddress) CyRamVectors;

    /* Initialize the configuration registers. */
    cyfitter_cfg();

    /* Setup DMA td linked list. */
    CyDmacConfigure();

    $Super$$main(); // calls original main()

    /* If main returns it is undefined what we should do. */
    for(;;)
    {
    }
}

#endif //__ARMCC_VERSION

#if defined(__GNUC__)




/* Application entry point. */
extern int main(void);


/*******************************************************************************
*
* Default Ram Interrupt Vector table storage area.
*
*******************************************************************************/
__attribute__ ((section(".ramvectors"))) cyisraddress CyRamVectors[NUM_INTERRUPTS];


/*******************************************************************************
*
* The following are constructs created by the linker, indicating where the
* the "data" and "bss" segments reside in memory.  The initializers for the
* for the "data" segment resides immediately following the "text" segment.
*
*******************************************************************************/
extern unsigned long _etext;
extern unsigned long _data;
extern unsigned long _edata;
extern unsigned long _bss;
extern unsigned long _ebss;
extern unsigned long _heap;
extern unsigned long _stack;


/*******************************************************************************
*
* Default Rom Interrupt Vector table.
*
*
*
*******************************************************************************/
__attribute__ ((section(".romvectors")))

void (* RomVectors[])(void) =
{
    (void (*)(void))((unsigned long) &_stack),  /* The initial stack pointer    */
    Reset,                                      /* The reset handler          0 */
    IntDefaultHandler,                          /* The NMI handler            1 */
    IntDefaultHandler,                          /* The hard fault handler     2 */
    IntDefaultHandler,                          /* The MPU fault handler      3 */
    IntDefaultHandler,                          /* The bus fault handler      4 */
    IntDefaultHandler,                          /* The usage fault handler    5 */
    0,                                          /* Reserved                   6 */
    0,                                          /* Reserved                   7 */
    0,                                          /* Reserved                   8 */
    0,                                          /* Reserved                   9 */
    IntDefaultHandler,                          /* SVCall handler            10 */
    IntDefaultHandler,                          /* Debug monitor handler     11 */
    0,                                          /* Reserved                  12 */
    IntDefaultHandler,                          /* The PendSV handler        13 */
    IntDefaultHandler,                          /* The SysTick handler       14 */
    IntDefaultHandler,                          /* External Interrupt(0)     15 */
    IntDefaultHandler,                          /* External Interrupt(1)     16 */
    IntDefaultHandler,                          /* External Interrupt(2)     17 */
    IntDefaultHandler,                          /* External Interrupt(3)     18 */
    IntDefaultHandler,                          /* External Interrupt(4)     19 */
    IntDefaultHandler,                          /* External Interrupt(5)     20 */
    IntDefaultHandler,                          /* External Interrupt(6)     21 */
    IntDefaultHandler,                          /* External Interrupt(7)     22 */
    IntDefaultHandler,                          /* External Interrupt(8)     23 */
    IntDefaultHandler,                          /* External Interrupt(9)     24 */
    IntDefaultHandler,                          /* External Interrupt(A)     25 */
    IntDefaultHandler,                          /* External Interrupt(B)     26 */
    IntDefaultHandler,                          /* External Interrupt(C)     27 */
    IntDefaultHandler,                          /* External Interrupt(D)     28 */
    IntDefaultHandler,                          /* External Interrupt(E)     29 */
    IntDefaultHandler,                          /* External Interrupt(F)     30 */
    IntDefaultHandler,                          /* External Interrupt(10)    31 */
    IntDefaultHandler,                          /* External Interrupt(11)    32 */
    IntDefaultHandler,                          /* External Interrupt(12)    33 */
    IntDefaultHandler,                          /* External Interrupt(13)    34 */
    IntDefaultHandler,                          /* External Interrupt(14)    35 */
    IntDefaultHandler,                          /* External Interrupt(15)    36 */
    IntDefaultHandler,                          /* External Interrupt(16)    37 */
    IntDefaultHandler,                          /* External Interrupt(17)    38 */
    IntDefaultHandler,                          /* External Interrupt(18)    39 */
    IntDefaultHandler,                          /* External Interrupt(19)    40 */
    IntDefaultHandler,                          /* External Interrupt(1A)    41 */
    IntDefaultHandler,                          /* External Interrupt(1B)    42 */
    IntDefaultHandler,                          /* External Interrupt(1C)    43 */
    IntDefaultHandler,                          /* External Interrupt(1D)    44 */
    IntDefaultHandler,                          /* External Interrupt(1E)    45 */
    IntDefaultHandler                           /* External Interrupt(1F)    46 */
};


/*******************************************************************************
* Function Name: IntDefaultHandler
********************************************************************************
* Summary:
*   This function is called when the PSoC5 begins execution on reset.
*   
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
void Reset(void)
{
    unsigned long * pulSrc;
    unsigned long * pulDest;
    unsigned long index;

#if (CYDEV_DEBUGGING_ENABLE)
    char value = CY_GET_XTND_REG8(CYDEV_DEBUG_ENABLE_REGISTER) | CYDEV_DEBUG_ENABLE_MASK;
	CY_SET_XTND_REG8(CYDEV_DEBUG_ENABLE_REGISTER, value);
    __asm("debugEnabled:\n");
#endif /* CYDEV_DEBUGGING_ENABLE */

#if (CYDEV_BOOTLOADER_ENABLE)
	pvt_CheckBootloader();
#endif /* CYDEV_BOOTLOADER_ENABLE */

    /* Setup the M3. */

    /* Copy the data segment initializers from flash to SRAM. */
    pulSrc = &_etext;
    for(pulDest = &_data; pulDest < &_edata;)
    {
        *pulDest++ = *pulSrc++;
    }

    /*
    ** Zero fill the bss segment.  This is done with inline assembly since this
    ** will clear the value of pulDest if it is not kept in a register.
    */
    __asm("    ldr     r0, =_bss\n"
          "    ldr     r1, =_ebss\n"
          "    mov     r2, #0\n"
          "    .thumb_func\n"
          "zero_loop:\n"
          "        cmp     r0, r1\n"
          "        it      lt\n"
          "        strlt   r2, [r0], #4\n"
          "        blt     zero_loop");

    /* Set Priority group 5. */
    /* Writes to the NVIC_APINT register require the VECTKEY in the upper half */
    *NVIC_APINT = NVIC_APINT_VECTKEY | NVIC_APINT_PRIGROUP_3_5;

    /* Set Ram interrupt vectors to default functions. */
    for(index = 0; index < NUM_INTERRUPTS; index++)
    {
        CyRamVectors[index] = IntDefaultHandler;
    }

    /* Point NVIC at the RAM vector table. */
    *CYINT_VECTORS = (cyisraddress) CyRamVectors;
    
    /* Initialize the configuration registers. */
    cyfitter_cfg();

    /* Setup DMA td linked list. */
    CyDmacConfigure();

    /* Call the application's entry point. */
    main();

    /* If main returns it is undefined what we should do. */
    for(;;)
    {
    }

}

#endif //__GNUC__

