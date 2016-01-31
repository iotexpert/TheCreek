;/***************************************************************************
;* FILENAME: cybtldrhelper.a51
;* Version `$CY_MAJOR_VERSION`.`$CY_MINOR_VERSION`
;*
;*  DESCRIPTION:
;*    Means to jump to an instruction in the Application and clear the stack on our way over.
;*
;*   C DECLARATIONS:
;*		extern void JumpToAddr(uint32 addr)
;*
;*---------------------------------------------------------------------------
;* Copyright (2009), Cypress Semiconductor Corporation.
;*---------------------------------------------------------------------------
;* This software is owned by Cypress Semiconductor Corporation (Cypress)
;* and is protected by and subject to worldwide patent protection (United
;* States and foreign), United States copyright laws and international treaty
;* provisions. Cypress hereby grants to licensee a personal, non-exclusive,
;* non-transferable license to copy, use, modify, create derivative works of,
;* and compile the Cypress Source Code and derivative works for the sole
;* purpose of creating custom software in support of licensee product to be
;* used only in conjunction with a Cypress integrated circuit as specified in
;* the applicable agreement. Any reproduction, modification, translation,
;* compilation, or representation of this software except as specified above 
;* is prohibited without the express written permission of Cypress.
;****************************************************************************/
$NOMOD51

;* PSoC Register definitions.
$INCLUDE (PSoC3_8051.inc)

;*******************************************************************************
;* Symbols
;*******************************************************************************
NAME CYBLMEM

PUBLIC _LaunchApp

SP_RESET_VALUE EQU 1h

;*******************************************************************************
;* void LaunchApp(uint32 *);
;* Jump to memory in CODE
;* Parameters:
;* R4: Bits [31:24] IGNORED
;* R5: Bits [23:16] IGNORED
;* R6: Bits [15:8] of start address
;* R7: Bits [7:0] of start address
;*******************************************************************************
?PR?CYMEMZERO?CYBLMEM SEGMENT CODE
RSEG ?PR?CYMEMZERO?CYBLMEM
_LaunchApp:
	MOV DPH, R6
	MOV DPL, R7
	MOV A, #0h
	MOV SP, #SP_RESET_VALUE
	JMP @A+DPTR
_LaunchApp_end:

	
END
