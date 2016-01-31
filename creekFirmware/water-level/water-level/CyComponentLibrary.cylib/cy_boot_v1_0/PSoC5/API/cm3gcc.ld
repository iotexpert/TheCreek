/*******************************************************************************
* File Name: Cm3Gcc.ld  
* Version `$CY_MAJOR_VERSION`.`$CY_MINOR_VERSION`
*
*  Description:
*  This Linker Descriptor file describes the memory layout of the PSoC5
*  device. The memory layout of the final binary and hex images as well as
*   the placement in PSoC5 memory is described.
*
*
*  Note:
*
*  romvectors:  Cypress default Interrupt sevice routine vector table.
*
*       This is the ISR vector table at bootup. Used only for the reset vector.
*
*
*  ramvectors:  Cypress ram interrupt service routine vector table.
*
*       This is the ISR vector table used by the application.
*
*
********************************************************************************
* Copyright 2008-2009, Cypress Semiconductor Corporation.  All rights reserved.
* You may use this file only in accordance with the license, terms, conditions, 
* disclaimers, and limitations in the end user license agreement accompanying 
* the software package with which this file was provided.
********************************************************************************/

MEMORY
{
    FLASH(rx) : ORIGIN =  `$PROJ_FIRST_FLS_BYTE`, LENGTH = `$FLASH_BYTES`
    CSRAM(rwx) : ORIGIN = 0x1FFF8000, LENGTH = (`$SRAM_BYTES` / 2)
    DSRAM(rwx) : ORIGIN = 0x20000000, LENGTH = (`$SRAM_BYTES` / 2)
}

SECTIONS
{
    .text :
    {
		. = ALIGN(4);

        KEEP(*(.romvectors))
        *(.romvectors*)
        *(.text*)
        *(.rodata*)

        /* ?NON MISRA ? */
        . = ALIGN(4);
        *(.jcr)
        *(.fini)
        *(.fini_array)
        *(.eh_frame)
        *(.init)
        *(.init_array)
        /* ?NON MISRA ? */

        __etext = .;
        PROVIDE(_etext = .);

    } > FLASH

	.ramvectors (NOLOAD) :
    {
        . = ALIGN(4);
        *(.ramvectors)

    } > CSRAM

    .data : AT (ADDR(.text) + SIZEOF(.text))
    {
        . = ALIGN(4);
        PROVIDE(_data = .);
        *(.data*)
        PROVIDE(_edata = .);

    } > DSRAM

    .bss : AT (ADDR(.text) + SIZEOF(.text) + SIZEOF(.data))
    {
        PROVIDE(_bss = .);
        *(.bss*)
        *(COMMON)
        PROVIDE(_ebss = .);

        . = ALIGN(8);
        PROVIDE(_end = .);

    } > DSRAM

	.heap : AT (ADDR(.text) + SIZEOF(.text) + SIZEOF(.data) + SIZEOF(.bss))
    {
        PROVIDE(_heap = .);
        PROVIDE(_stack = ALIGN(`$SRAM_BYTES` / 2));

    } > DSRAM

}




