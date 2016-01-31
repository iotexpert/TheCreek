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

ZERO_REG        EQU     CYDEV_PHUB_CFGMEM23_CFG1
SIZEOF_IDATA    EQU     100h

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
                DS      1 ; Declare some data so the assembler will keep the labeled segment.

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
       ljmp     STARTUP1                            ; 3 bytes

;*******************************************************************************
;* Aligned routines
;*******************************************************************************

       ; The following routines require alignment to avoid crossing an 8-byte
       ; boundary in an inner loop.

       ; dma_wait
       ; Arguments:
       ; R6:R7  Address of BASIC_STATUS
       ; Waits for the CHAIN_ACTIVE bit to be cleared by the DMA controller.
       nop                                          ; 1 byte
       PUBLIC   _dma_wait
_dma_wait:
       mov      DPL, R7                             ; 2 bytes
       mov      DPH, R6                             ; 2 bytes
       ; The loop needs to be aligned to an 8-byte boundary
_dma_wait_loop:
       movx     A, @DPTR                            ; 1 byte
       anl      A, #01h                             ; 2 bytes   CHAIN_ACTIVE
       nop                                          ; 1 byte
       nop                                          ; 1 byte
       nop                                          ; 1 byte
       jnz      _dma_wait_loop                      ; 2 bytes
       ret                                          ; 1 byte

       ; clear_idata
       ; Arguments: None
       ; Writes zeros to IDATA.
       ; This code should not be called because it overwrites the call stack.
clear_idata:
       mov      R0, #SIZEOF_IDATA - 1               ; 2 bytes
       clr      A                                   ; 1 byte
       ; The loop needs to be aligned to a 4-byte boundary
clear_idata_loop:
       mov      @R0, A                              ; 1 byte
       djnz     R0, clear_idata_loop                ; 2 bytes
       ljmp     clear_idata_done                    ; 3 bytes

;*******************************************************************************
;* Startup entry Point.
;*******************************************************************************
                RSEG    ?C_C51STARTUP
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

       mov      ?C?XPAGE1SFR, #?C?XPAGE1RST         ; Set XDATA page
       mov      DPTR, #CYDEV_CACHE_CR
IF (CYDEV_CHIP_REV_EXPECT == CYDEV_CHIP_REV_LEOPARD_ES1 OR CYDEV_CHIP_REV_EXPECT == CYDEV_CHIP_REV_LEOPARD_ES2)
 IF (CYDEV_CONFIGURATION_IMOENABLED <> 0)
       mov      A, #(02h SHL 6)                     ; FLASH_CYCLES = 2 (3 cycles, up to 48MHz)
 ELSE
       mov      A, #(01h SHL 6)                     ; FLASH_CYCLES = 1 (2 cycles, up to 24MHz)
 ENDIF
ELSE
 IF (CYDEV_CONFIGURATION_IMOENABLED <> 0)
       mov      A, #(03h SHL 6)                     ; FLASH_CYCLES = 3 (3 cycles, up to 67MHz)
 ELSE
       mov      A, #(01h SHL 6)                     ; FLASH_CYCLES = 1 (1 cycle, up to 22MHz)
 ENDIF
ENDIF
       movx     @DPTR, A
       mov      sp, #?STACK-1                       ; Set the stack pointer.

IF CYDEV_BOOTLOADER_ENABLE <> 0
       ;* if second MSB of reset status register is high then we are to do a bootload operation
       ;* so we should jump to CONFIGURE
       mov      DPTR, #WORD0 CYDEV_RESET_SR0        ; move the low 16 bits of data into DPTR
 IF (BYTE2 CYDEV_RESET_SR0) <> ?C?XPAGE1RST
       mov      DPX, #BYTE2 CYDEV_RESET_SR0         ; move the high 8 bits of data into DPX
 ENDIF
       movx     A, @DPTR
 IF (BYTE2 CYDEV_RESET_SR0) <> ?C?XPAGE1RST
       mov      DPX, #?C?XPAGE1RST
 ENDIF
       anl      A, #040h
       jnz      Btldr_NoLaunch

       ;* check if need to start loadable application, bootloaders always do this check first so
       ;* that the device does not get configured before we launch the user application which
       ;* has its own unique configuration
       lcall    CyBtldr_CheckLaunch
Btldr_NoLaunch:
ENDIF

IF (CYDEV_CONFIGURATION_CLEAR_SRAM <> 0)
       ;* Clear SRAM using DMA
       mov      DPTR, #ZERO_REG
       clr      A
       movx     @DPTR, A
       inc      DPTR
       movx     @DPTR, A
       inc      DPTR
       movx     @DPTR, A
       inc      DPTR
       movx     @DPTR, A
       mov      DPTR, #CYDEV_PHUB_CH0_BASIC_CFG
       movx     @DPTR, A                            ; Make sure channel is disabled
       mov      DPTR, #(CYDEV_PHUB_CH0_BASIC_STATUS + 1)
       movx     @DPTR, A                            ; Initial TD 0
       mov      DPTR, #CYDEV_PHUB_CH1_BASIC_CFG
       movx     @DPTR, A                            ; Disable channel

       ;* CFGMEM0 Setup
       mov      DPTR, #CYDEV_PHUB_CFGMEM0_CFG0
       mov      A, #010h                            ; burstcnt
       movx     @DPTR, A
       mov      DPTR, #CYDEV_PHUB_CFGMEM0_CFG1
       clr      A
       movx     @DPTR, A                            ; src_base_adr low
       inc      DPTR
       mov      A, #BYTE2 ZERO_REG
       movx     @DPTR, A                            ; src_base_adr high
       inc      DPTR
       clr      A
       movx     @DPTR, A                            ; dst_base_adr low
       inc      DPTR
       movx     @DPTR, A                            ; dst_base_adr high

       ;* TD0 configuration
       mov      DPTR, #CYDEV_PHUB_TDMEM0_ORIG_TD0
 IF (CYDEV_SRAM_SIZE < 4096)
       mov      A, #LOW CYDEV_SRAM_SIZE
       movx     @DPTR, A                            ; xfercnt low
       inc      DPTR
       mov      A, #HIGH CYDEV_SRAM_SIZE
       movx     @DPTR, A                            ; xfercnt high
 ELSE
       mov      A, #0FFh
       movx     @DPTR, A                            ; xfercnt low
       inc      DPTR
       mov      A, #0Fh
       movx     @DPTR, A                            ; xfercnt high
 ENDIF
       inc      DPTR
 IF (CYDEV_SRAM_SIZE <= 4096)
       mov      A, #0FFh
 ELSE
       mov      A, #01h
 ENDIF
       movx     @DPTR, A                            ; next_td_ptr
       inc      DPTR
       mov      A, #22h                             ; auto_exec_next, inc_dst_adr
       movx     @DPTR, A
       inc      DPTR

       ;* TD0 addresses
       mov      A, #LOW ZERO_REG                    ; this register resets to 0
       movx     @DPTR, A                            ; src_adr low
       inc      DPTR
       mov      A, #HIGH ZERO_REG
       movx     @DPTR, A                            ; src_adr high
       inc      DPTR
       clr      A
       movx     @DPTR, A                            ; dst_adr low
       inc      DPTR
       movx     @DPTR, A                            ; dst_adr high

 IF (CYDEV_SRAM_SIZE > 4096)
       ;* TD1 configuration
       inc      DPTR
  IF (CYDEV_SRAM_SIZE < 8192)
       mov      A, #LOW (CYDEV_SRAM_SIZE - 4096)
       movx     @DPTR, A                            ; xfercnt low
       inc      DPTR
       mov      A, #HIGH (CYDEV_SRAM_SIZE - 4096)
       movx     @DPTR, A                            ; xfercnt high
  ELSE
       mov      A, #0FFh
       movx     @DPTR, A                            ; xfercnt low
       inc      DPTR
       mov      A, #0Fh
       movx     @DPTR, A                            ; xfercnt high
  ENDIF
       inc      DPTR
       mov      A, #0FFh
       movx     @DPTR, A                            ; next_td_ptr
       inc      DPTR
       mov      A, #22h                             ; auto_exec_next, inc_dst_adr
       movx     @DPTR, A
       inc      DPTR

       ;* TD1 addresses
       mov      A, #LOW ZERO_REG
       movx     @DPTR, A                            ; src_adr low
       inc      DPTR
       mov      A, #HIGH ZERO_REG
       movx     @DPTR, A                            ; src_adr high
       inc      DPTR
       clr      A
       movx     @DPTR, A                            ; dst_adr low
       inc      DPTR
       mov      A, #10h
       movx     @DPTR, A                            ; dst_adr high
 ENDIF

       ;* Start DMA
       mov      DPTR, #CYDEV_PHUB_CH0_BASIC_CFG
       mov      A, #01h
       movx     @DPTR, A
       mov      DPTR, #CYDEV_PHUB_CH0_ACTION
       movx     @DPTR, A

 IF (CYDEV_SRAM_SIZE >= 4096)
       clr      A
       mov      DPTR, #0FFFh
       movx     @DPTR, A                            ; DMA will skip this byte

 ENDIF
 IF (CYDEV_SRAM_SIZE >= 8192)
       mov      DPTR, #1FFFh
       movx     @DPTR, A                            ; DMA will skip this byte
 ENDIF
 IF (CYDEV_SRAM_SIZE > 8192)
      __ERROR__ "Unsupported SRAM size"
 ENDIF
ENDIF

IF (CYDEV_CHIP_DIE_EXPECT == CYDEV_CHIP_DIE_LEOPARD AND CYDEV_ECC_ENABLE <> 0)
       ;* Workaround for an issue that could cause a false ECC interrupt at startup
       mov      DPTR, #CYDEV_CACHE_INT_LOG3
       movx     A, @DPTR                            ; clear CACHE_INT_LOG3 (clear on read)
       mov      DPTR, #CYDEV_CACHE_INT_LOG4
       movx     A, @DPTR                            ; clear CACHE_INT_LOG4 (clear on read)
       mov      DPTR, #CYDEV_INTC_CLR_PD0
       mov      A, #02h
       movx     @DPTR, A                            ; clear interrupt 1
ENDIF

       ;* Clear idata.
       ljmp    clear_idata
clear_idata_done:

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

IF (CYDEV_CONFIGURATION_CLEAR_SRAM <> 0)
       ;* Wait for DMA to finish
       mov      R6, #HIGH CYDEV_PHUB_CH0_BASIC_STATUS
       mov      R7, #LOW CYDEV_PHUB_CH0_BASIC_STATUS
       lcall    _dma_wait
       mov      DPTR, #CYDEV_PHUB_CH0_BASIC_CFG
       movx     @DPTR, A                            ; Disable channel
ENDIF

       ;* Initialize the configuration registers.
       lcall    cyfitter_cfg

       ;* Setup DMA.
       lcall    CyDmacConfigure

       ;* Jump to Keil's variable initialization and then main.
       ljmp     ?C_START

;*******************************************************************************
;* End of startup code.
;*******************************************************************************

       END
