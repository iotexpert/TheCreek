;*******************************************************************************
;* FILENAME: PSoC3_8051.inc 
;* Version `$CY_MAJOR_VERSION`.`$CY_MINOR_VERSION`
;*
;*  DESCRIPTION:
;*    8051 register definitions for the PSoC3 family of parts.
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

SFRPRT0DR       DATA 080H

SP              DATA 081H
DPL             DATA 082H
DPH             DATA 083H
DPL1            DATA 084H
DPH1            DATA 085H
DPS             DATA 086H

SFRPRT0PS       DATA 089H
SFRPRT0SEL      DATA 08AH
SFRPRT1DR       DATA 090H
SFRPRT1PS       DATA 091H

DPX             DATA 093H
DPX1            DATA 095H

SFRPRT2DR       DATA 098H
SFRPRT2PS       DATA 099H
SFRPRT2SEL      DATA 09AH

P2AX            DATA 0A0H
CPUCLK_DIV      DATA 0A1H
SFRPRT1SEL      DATA 0A2H

IE              DATA 0A8H
  EA            BIT 0A8H+7H

SFRPRT3DR       DATA 0B0H
SFRPRT3PS       DATA 0B1H
SFRPRT3SEL      DATA 0B2H
SFRPRT4DR       DATA 0C0H
SFRPRT4PS       DATA 0C1H
SFRPRT4SEL      DATA 0C2H
SFRPRT5DR       DATA 0C8H
SFRPRT5PS       DATA 0C9H
SFRPRT5SEL      DATA 0CAH

PSW             DATA 0D0H
  P             BIT 0D0H+0H
  F1            BIT 0D0H+1H
  OV            BIT 0D0H+2H
  RS0           BIT 0D0H+3H
  RS1           BIT 0D0H+4H
  F0            BIT 0D0H+5H
  AC            BIT 0D0H+6H
  CY            BIT 0D0H+7H

SFRPRT6DR       DATA 0D8H
SFRPRT6PS       DATA 0D9H
SFRPRT6SEL      DATA 0DAH

ACC             DATA 0E0H

SFRPRT12DR      DATA 0E8H
SFRPRT12PS      DATA 0E9H
MXAX            DATA 0EAH

B               DATA 0F0H

SFRPRT12SEL     DATA 0F2H
SFRPRT15DR      DATA 0F8H
SFRPRT15PS      DATA 0F9H
SFRPRT15SEL     DATA 0FAH