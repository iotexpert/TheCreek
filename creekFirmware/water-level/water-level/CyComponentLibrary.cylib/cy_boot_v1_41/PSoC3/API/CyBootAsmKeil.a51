;*******************************************************************************
;* FILENAME: CyBootAsmKeil.a51
;* Version `$CY_MAJOR_VERSION`.`$CY_MINOR_VERSION`
;*
;*  DESCRIPTION:
;*    Assembly routines for Keil A51.
;*
;*   NOTE:
;*     
;*
;*******************************************************************************
;* Copyright (2010), Cypress Semiconductor Corporation.
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

$NOMOD51
$INCLUDE(cyfitterkeil.inc)
$INCLUDE(PSoC3_8051.inc)

NAME CYBOOTASMKEIL
PUBLIC _CyDelayCycles
PUBLIC _CyDelayUs
EXTERN XDATA:BYTE (cydelay_freq_mhz)

; void CyDelayCycles(unsigned long count);
; Parameters:
; R4:R7    count (MSB in R4)
; Destroys: R0, R3, R4, R5, R6, R7
?PR?CYDELAYCYCLES?CYBOOTASMKEIL SEGMENT CODE PAGE	; Align to 256-byte boundary
RSEG ?PR?CYDELAYCYCLES?CYBOOTASMKEIL
_CyDelayCycles:               ; cycles bytes
	MOV DPTR, #CYDEV_CACHE_CR ;	3	3
	MOVX A, @DPTR             ;	2+	1
	SWAP A                    ;	1	1
	RRC A                     ;	1	1
	RRC A                     ;	1	1
	ANL A, #03h               ;	2	2	(8-byte boundary)
	MOV DPTR, #cy_flash_cycles;	3	3
	MOVC A, @A+DPTR           ;	5+	1
	INC A                     ;	1	1
	MOV R3, A                 ;	1	1
	ADD A, R3                 ;	1	1
	ADD A, R3                 ;	1	1	(8-byte boundary)
	ADD A, #23                ;	2	2
	CPL A                     ;	1	1	Negate
	INC A                     ;	1	1
	MOV R3, A                 ;	1	1
	MOV R0, #0FFh             ;	2	2
	NOP                       ;	1	1	(8-byte boundary)
_CyDelayCycles_loop:
	MOV A, R4                 ;	1	1
	ORL A, R5                 ;	1	1
	ORL A, R6                 ;	1	1
	ORL A, R7                 ;	1	1
	JZ _CyDelayCycles_done    ;	4	2
	MOV A, R7                 ;	1	1
	ADD A, R3                 ;	1	1	(8-byte boundary)
	MOV R7, A                 ;	1	1
	MOV A, R6                 ;	1	1
	ADDC A, R0                ;	1	1
	MOV R6, A                 ;	1	1
	MOV A, R5                 ;	1	1
	ADDC A, R0                ;	1	1
	MOV R5, A                 ;	1	1
	MOV A, R4                 ;	1	1	(8-byte boundary)
	ADDC A, R0                ;	1	1
	MOV R4, A                 ;	1	1
	JC _CyDelayCycles_loop    ; 3	2
_CyDelayCycles_done:
	RET                       ;	4	1
cy_flash_cycles:
IF CYDEV_CHIP_REV_EXPECT <= CYDEV_CHIP_REV_LEOPARD_ES2
	DB 5, 2, 3, 4
ELSE
	DB 4, 1, 2, 3
ENDIF

; void CyDelayUs(unsigned short count);
; Parameters:
; R6:R7    count (MSB in R6)
; Destroys: R0, R1, R2, R3, R4, R5, R6, R7
?PR?CYDELAYUS?CYBOOTASMKEIL SEGMENT CODE DWORD	; Align to 4-byte boundary
RSEG ?PR?CYDELAYUS?CYBOOTASMKEIL
USING 0
_CyDelayUs:                   ; cycles bytes
	MOV A, R6                 ;	1	1
	MOV R2, A                 ;	1	1
	MOV DPTR, #cydelay_freq_mhz	;	3	3	R5:R6:R7 = R2:R7 * cydelay_freq_mhz
	MOVX A, @DPTR             ;	2+	1
	MOV R0, A                 ;	1	1	Save cydelay_freq_mhz
	MOV B, R7                 ;	2	2
	MUL AB                    ;	2	1	A * LSB
	MOV R7, A                 ;	1	1	product LSB
	MOV R6, B                 ;	3	2	product MSB
	MOV A, R0                 ;	1	1	cydelay_freq_mhz
	MOV B, R2                 ;	3	3
	MUL AB                    ;	2	1	A * MSB
	ADD A, R6                 ;	1	1
	MOV R6, A                 ;	1	1	product LSB
	MOV A, B                  ;	2	2	product MSB
	ADDC A, #0                ;	2	2	carry
	MOV R5, A                 ;	1	1
	MOV A, R7                 ;	1	1
	ADD A, #0A6h              ;	2	2	Subtract estimated overhead (128 cycles - 38 margin)
	MOV R7, A                 ;	1	1
	MOV R0, #0FFh             ;	2	2
	MOV A, R6                 ;	1	1
	ADDC A, R0                ;	1	1
	MOV R6, A                 ;	1	1
	MOV A, R5                 ;	1	1
	ADDC A, R0                ;	1	1
	MOV R5, A                 ;	1	1
	CLR A                     ;	1	1
	ADDC A, R0                ;	1	1
	MOV R4, A                 ;	1	1
	JNC _CyDelayUs_done       ;	3	2	Overhead exceeds requested delay
	LJMP _CyDelayCycles       ;	4	3
_CyDelayUs_done:
	RET

	END


;[] END OF FILE
