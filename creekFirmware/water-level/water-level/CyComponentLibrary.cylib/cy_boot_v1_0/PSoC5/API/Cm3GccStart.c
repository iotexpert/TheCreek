/*******************************************************************************
* File Name: Cm3GccStart.c  
* Version `$CY_MAJOR_VERSION`.`$CY_MINOR_VERSION`
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


#include <CYDEVICE.H>
#include <CYTYPES.H>
#include <CYFITTER_CFG.H>





#define NVIC_VTABLE                 ((volatile unsigned long *)0xE000ED08)
#define NVIC_APINT                  ((volatile unsigned long *)0xE000ED0C)
#define NVIC_APINT_PRIGROUP_3_5     0x00000400  // Priority group 3.5 split



/* Local function for the device reset. */
void Reset(void);

/* Local function to handle unintended interrupts. */
static void IntDefaultHandler(void);

#define NUM_INTERRUPTS  64 // 61 realy but lets align stuff.

#define SIZEOF_ISR_TABLE    64

__attribute__ ((section(".ramvectors")))
cyisraddress CyRamVectors[SIZEOF_ISR_TABLE];

/* Application entry point. */
extern int main(void);


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

//void (* g_pfnVectors[])(void) =
void (* RomVectors[])(void) =
{
    (void (*)(void))((unsigned long) &_stack),
                        /* The initial stack pointer        */
    Reset,              /* The reset handler                */
    IntDefaultHandler,  /* The NMI handler                  */
    IntDefaultHandler,  /* The hard fault handler           */
    IntDefaultHandler,  /* The MPU fault handler            */
    IntDefaultHandler,  /* The bus fault handler            */
    IntDefaultHandler,  /* The usage fault handler          */
    0,                  /* Reserved                         */
    0,                  /* Reserved                         */
    0,                  /* Reserved                         */
    0,                  /* Reserved                         */
    IntDefaultHandler,  /* SVCall handler                   */
    IntDefaultHandler,  /* Debug monitor handler            */
    0,                  /* Reserved                         */
    IntDefaultHandler,  /* The PendSV handler               */
    IntDefaultHandler,  /* The SysTick handler              */
    IntDefaultHandler,  /* GPIO Port A                      */
    IntDefaultHandler,  /* GPIO Port B                      */
    IntDefaultHandler,  /* GPIO Port C                      */
    IntDefaultHandler,  /* GPIO Port D                      */
    IntDefaultHandler,  /* GPIO Port E                      */
    IntDefaultHandler,  /* UART0 Rx and Tx                  */
    IntDefaultHandler,  /* UART1 Rx and Tx                  */
    IntDefaultHandler,  /* SSI Rx and Tx                    */
    IntDefaultHandler,  /* I2C Master and Slave             */
    IntDefaultHandler,  /* PWM Fault                        */
    IntDefaultHandler,  /* PWM Generator 0                  */
    IntDefaultHandler,  /* PWM Generator 1                  */
    IntDefaultHandler,  /* PWM Generator 2                  */
    IntDefaultHandler,  /* Quadrature Encoder               */
    IntDefaultHandler,  /* ADC Sequence 0                   */
    IntDefaultHandler,  /* ADC Sequence 1                   */
    IntDefaultHandler,  /* ADC Sequence 2                   */
    IntDefaultHandler,  /* ADC Sequence 3                   */
    IntDefaultHandler,  /* Watchdog timer                   */
    IntDefaultHandler,  /* Timer 0 subtimer A               */
    IntDefaultHandler,  /* Timer 0 subtimer B               */
    IntDefaultHandler,  /* Timer 1 subtimer A               */
    IntDefaultHandler,  /* Timer 1 subtimer B               */
    IntDefaultHandler,  /* Timer 2 subtimer A               */
    IntDefaultHandler,  /* Timer 2 subtimer B               */
    IntDefaultHandler,  /* Analog Comparator 0              */
    IntDefaultHandler,  /* Analog Comparator 1              */
    IntDefaultHandler,  /* Analog Comparator 2              */
    IntDefaultHandler,  /* System Control (PLL, OSC, BO)    */
    IntDefaultHandler,  /* FLASH Control                    */
    IntDefaultHandler,  /* GPIO Port F                      */
    IntDefaultHandler,  /* GPIO Port G                      */
    IntDefaultHandler,  /* GPIO Port H                      */
    IntDefaultHandler,  /* UART2 Rx and Tx                  */
    IntDefaultHandler,  /* SSI1 Rx and Tx                   */
    IntDefaultHandler,  /* Timer 3 subtimer A               */
    IntDefaultHandler,  /* Timer 3 subtimer B               */
    IntDefaultHandler,  /* I2C1 Master and Slave            */
    IntDefaultHandler,  /* Quadrature Encoder 1             */
    IntDefaultHandler,  /* CAN0                             */
    IntDefaultHandler,  /* CAN1                             */
    IntDefaultHandler,  /* CAN2                             */
    IntDefaultHandler,  /* Ethernet                         */
    IntDefaultHandler   /* Hibernate                        */
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

	/* Setup the M3. */

    /* Set Priority group 5. */
    *NVIC_APINT |= NVIC_APINT_PRIGROUP_3_5;

	/* Move Interrupt Vector table from Code to RAM. */
    for(index = 0; index < NUM_INTERRUPTS; index++)
    {
        CyRamVectors[index] = 0; //(void (*)(void))HWREG(ulIdx * 4); where? we know from but to where?
    }

    /* Point NVIC at the RAM vector table. */
    *NVIC_VTABLE = (unsigned long) CyRamVectors;
    
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

    /* Initialize the configuration registers. */
    cyfitter_cfg();

    /* Call the application's entry point. */
    main();

    /* If main returns it is undefined what we should do. */
    for(;;)
    {
    }

}

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
static void IntDefaultHandler(void)
{
    /* We should never get here. */

    /* Go into an infinite loop. */
    while(1)
    {
    }
}



