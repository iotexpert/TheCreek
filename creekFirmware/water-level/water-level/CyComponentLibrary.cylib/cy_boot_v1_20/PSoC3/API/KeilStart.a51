;*******************************************************************************
;* FILENAME: KeilStart.a51
;* Version `$CY_MAJOR_VERSION`.`$CY_MINOR_VERSION`
;*
;*  DESCRIPTION:
;*    Bootup Code for PSoC3 chips using Keil tools.
;*
;*   NOTE:
;*     
;*     
;*
;*******************************************************************************
;* Copyright (2008), Cypress Semiconductor Corporation.
;*******************************************************************************
;* This software is owned by Cypress Semiconductor Corporation (Cypress) and is 
;* protected by and subject to worldwide patent protection (United States and 
;* foreign), United States copyright laws and international treaty provisions. 
;* Cypress hereby grants to licensee a personal, non-exclusive, non-transferable 
;* license to copy, use, modify, create derivative works of, and compile the 
;* Cypress Source Code and derivative works for the sole purpose of creating 
;* custom software in support of licensee product to be used only in conjunction 
;* with a Cypress integrated circuit as specified in the applicable agreement. 
;* Any reproduction, modification, translation, compilation, or representation of 
;* this software except as specified above is prohibited without the express 
;* written permission of Cypress.
;*
;* Disclaimer: CYPRESS MAKES NO WARRANTY OF ANY KIND, EXPRESS OR IMPLIED, WITH 
;* REGARD TO THIS MATERIAL, INCLUDING, BUT NOT LIMITED TO, THE IMPLIED 
;* WARRANTIES OF MERCHANTABILITY AND FITNESS FOR A PARTICULAR PURPOSE.
;* Cypress reserves the right to make changes without further notice to the 
;* materials described herein. Cypress does not assume any liability arising out 
;* of the application or use of any product or circuit described herein. Cypress 
;* does not authorize its products for use as critical components in life-support 
;* systems where a malfunction or failure may reasonably be expected to result in 
;* significant injury to the user. The inclusion of Cypress' product in a life-
;* support systems application implies that the manufacturer assumes all risk of 
;* such use and in doing so indemnifies Cypress against all charges. Use may be 
;* limited by and subject to the applicable Cypress software license agreement. 
;*******************************************************************************

;* We will supply our own register definitions.
$NOMOD51

;* PSoC Register definitions.
$INCLUDE (PSoC3_8051.inc)

;* Application specific data and definitions + PSoC Register definitions.
$INCLUDE (cyfitterkeil.INC)


SIZEOF_IDATA    EQU     100h
SIZEOF_XDATA    EQU     CYDEV_SRAM_SIZE

IBPSTACK        EQU     0
XBPSTACK        EQU     0
PBPSTACK        EQU     0

?C?XPAGE1SFR    DATA    093h
?C?XPAGE1RST    EQU     0

;*******************************************************************************
;* Placement.
;*******************************************************************************
                NAME    ?C_STARTUP


?C_C51STARTUP   SEGMENT   CODE
?STACK          SEGMENT   IDATA

                RSEG    ?STACK
                DS      1                  			; Declare some data so the assembler will keep the labeled segment.

                EXTRN CODE (?C_START)
                EXTRN CODE (cyfitter_cfg)
                EXTRN CODE (CyDmacConfigure)
				
IF CYDEV_BOOTLOADER_ENABLE <> 0
				EXTRN CODE (CyBtldr_CheckLaunch)
ENDIF

                PUBLIC  ?C_STARTUP, ?C?XPAGE1SFR, ?C?XPAGE1RST, STARTUP1 ; include STARTUP1 for bootloader

;*******************************************************************************
;* Reset vector.
;*******************************************************************************

                CSEG    AT      `$PROJ_FIRST_FLS_BYTE`
?C_STARTUP:
       ljmp     STARTUP1

                RSEG    ?C_C51STARTUP

;*******************************************************************************
;* Startup entry Point.
;*******************************************************************************
STARTUP1:

IF CYDEV_DEBUGGING_ENABLE <> 0
      DPL0    EQU 082H
      DPH0    EQU 083H
      DPX0    EQU 093H

      MOV DPX0, #0
      MOV DPH0, #HIGH (CYDEV_DEBUG_ENABLE_REGISTER)
      MOV DPL0, #LOW (CYDEV_DEBUG_ENABLE_REGISTER)
      MOVX A,@DPTR
      ORL A, #CYDEV_DEBUG_ENABLE_MASK
      MOVX @DPTR, A
ENDIF

       ;* Clear idata.
       mov      R0, #SIZEOF_IDATA - 1               ; Get size of idata and zero base the number.
       clr      A                                   ; Clear the accumulator.
IDATALOOP:
       mov      @R0, A                              ; Clear byte of idata.
       djnz     R0, IDATALOOP                       ; Decrement r0 and loop if it is not zero.

       mov      ?C?XPAGE1SFR, ?C?XPAGE1RST

IF (CYDEV_CONFIGURATION_CLEAR_SRAM <> 0 && SIZEOF_XDATA <> 0)
       ;* Clear xdata.
       mov      DPTR, #0                            ; Point data pointer to the base of xdata.
       mov      R7, #LOW (SIZEOF_XDATA)             ; Get the low byte size of xdata.
IF (LOW (SIZEOF_XDATA)) <> 0
       mov      R6, #(HIGH (SIZEOF_XDATA) + 1       ; Since 
ELSE
       mov      R6, #HIGH (SIZEOF_XDATA)
ENDIF
       ;clr     A                                   ; A is clear from above.

XDATALOOP:                                          ; 
       movx     @DPTR, A                            ; Clear byte of xdata.
       inc      DPTR                                ; Point to next byte in xdata.
       djnz     R7, XDATALOOP                       ; Decrement r7 and loop if it is not zero.
       djnz     R6, XDATALOOP                       ; Decrement r6 and loop if it is not zero.
ENDIF


IF IBPSTACK <> 0
       EXTRN DATA (?C_IBP)

       ;* ?C_IBP acts as a base pointer to the reentrant stack for the SMALL model.
       mov      ?C_IBP, #LOW IBPSTACKTOP
ENDIF


IF XBPSTACK <> 0
       EXTRN DATA (?C_XBP)

       ;* ?C_XBP acts as a base pointer to the reentrant stack for the LARGE model.
       mov      ?C_XBP, #HIGH XBPSTACKTOP
       mov      ?C_XBP+1, #LOW XBPSTACKTOP
ENDIF

IF PBPSTACK <> 0
       EXTRN DATA (?C_PBP)

       ;* ?C_XBP acts as a base pointer to the reentrant stack for the COMPACT model.
       mov      ?C_PBP, #LOW PBPSTACKTOP
ENDIF


       ;* Set the stack pointer.
       mov      sp, #?STACK-1


       ;* Set the 8051 enable interrupts bits.
	   ;* This is only to be ARM-8051 compatible.
	   ;* Interrupts will not be active until the INTC bits are enabled.
	   ;setb     EA
	   
IF CYDEV_BOOTLOADER_ENABLE <> 0

	   ;* if second MSB of reset status register is high then we are to do a bootload operation
	   ;* so we should jump to CONFIGURE
	   MOV DPTR, #WORD0 CYDEV_RESET_SR0 ; move the low 16 bits of data into DPTR
	   MOV DPX, #BYTE2 CYDEV_RESET_SR0  ; move the high 8 bits of data into DPX
	   MOVX A, @DPTR
	   ANL A, #040h
	   JNZ CONFIGURE

	   ;* check if need to start loadable application, bootloaders always do this check first so
	   ;* that the device does not get configured before we launch the user application which
	   ;* has its own unique configuration
	   lcall	CyBtldr_CheckLaunch
ENDIF

CONFIGURE:

       ;* Initialize the configuration registers.
       lcall    cyfitter_cfg

       ;* Setup DMA.
	   lcall	CyDmacConfigure

       ;* Jump to Keil's variable initialization and then main.
       ljmp     ?C_START

       

;*******************************************************************************
;* End of startup code.
;*******************************************************************************

       END

