/******************************************************************************
* File Name: CyBootAsmGnu.s
* Version `$CY_MAJOR_VERSION`.`$CY_MINOR_VERSION`
*
*  Description:
*   Assembly routines for GNU as.
*
*  Note: 
*
********************************************************************************
* Copyright 2010, Cypress Semiconductor Corporation.  All rights reserved.
* You may use this file only in accordance with the license, terms, conditions, 
* disclaimers, and limitations in the end user license agreement accompanying 
* the software package with which this file was provided.
********************************************************************************/

.syntax unified
.text
.thumb
.global CyDelayCycles

.align 8
.func CyDelayCycles, CyDelayCycles
.type CyDelayCycles, %function
.thumb_func
CyDelayCycles:              /* cycles bytes */
	ADDS r0, r0, #2         /*	1	2	Round to nearest multiple of 4 */
	LSRS r0, r0, #2         /*	1	2	Divide by 4 and set flags */
	BEQ CyDelayCycles_done  /*	2	2	Skip if 0 */
	NOP                     /*	1	2	Loop alignment padding */
CyDelayCycles_loop:
	SUBS r0, r0, #1         /*	1	2 */
	MOV r0, r0              /*	1	2	Pad loop to power of two cycles */
	BNE CyDelayCycles_loop  /*	2	2 */
CyDelayCycles_done:
	BX lr                   /*	3	2 */
.endfunc

.end


/* [] END OF FILE */
