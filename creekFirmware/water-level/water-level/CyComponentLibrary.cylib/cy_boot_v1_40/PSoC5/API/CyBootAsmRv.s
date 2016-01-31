;*******************************************************************************
;* FILENAME: CyBootAsmRv.s
;* Version `$CY_MAJOR_VERSION`.`$CY_MINOR_VERSION`
;*
;*  DESCRIPTION:
;*    Assembly routines for RealView.
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

	AREA |.text|,CODE,ALIGN=8
	THUMB
	ALIGN 8
CyDelayCycles FUNCTION
	EXPORT CyDelayCycles
	                    ; cycles bytes
	ADDS r0, r0, #2         ;	1	2	Round to nearest multiple of 4
	LSRS r0, r0, #2         ;	1	2	Divide by 4 and set flags
	BEQ CyDelayCycles_done  ;	2	2	Skip if 0
	NOP                     ;	1	2	Loop alignment padding
CyDelayCycles_loop
	SUBS r0, r0, #1         ;	1	2
	MOV r0, r0              ;	1	2	Pad loop to power of two cycles
	BNE CyDelayCycles_loop  ;	2	2
CyDelayCycles_done
	BX lr                   ;	3	2
	ENDFUNC

	END


; [] END OF FILE
