/*******************************************************************************
* File Name: `$INSTANCE_NAME`.h
* Version `$CY_MAJOR_VERSION`.`$CY_MINOR_VERSION`
*
* Description:
*  This file provides constants and parameter values for the PrISM
*  Component.
*
* Note:
*
********************************************************************************
* Copyright 2008-2009, Cypress Semiconductor Corporation.  All rights reserved.
* You may use this file only in accordance with the license, terms, conditions, 
* disclaimers, and limitations in the end user license agreement accompanying 
* the software package with which this file was provided.
*******************************************************************************/

#if !defined(CY_PrISM_`$INSTANCE_NAME`_H)    /* CY_PrISM_`$INSTANCE_NAME`_H */
#define CY_PrISM_`$INSTANCE_NAME`_H

#include "cytypes.h"
#include "cyfitter.h"


/***************************************
* Conditional Compilation Parameters
***************************************/

#define `$INSTANCE_NAME`_RESOLUTION             `$Resolution`u
#define `$INSTANCE_NAME`_PULSE_TYPE_HARDCODED   `$PulseTypeHardcoded`u


/***************************************
*       Function Prototypes
***************************************/

void  `$INSTANCE_NAME`_Start(void);
void  `$INSTANCE_NAME`_Stop(void);
void  `$INSTANCE_NAME`_SetPulse0Mode(uint8 pulse0Type);
void  `$INSTANCE_NAME`_SetPulse1Mode(uint8 pulse1Type);
`$RegSizeReplacementString` `$INSTANCE_NAME`_ReadSeed(void);
void   `$INSTANCE_NAME`_WriteSeed(`$RegSizeReplacementString` seed);
`$RegSizeReplacementString` `$INSTANCE_NAME`_ReadPolynomial(void);
void   `$INSTANCE_NAME`_WritePolynomial(`$RegSizeReplacementString` polynomial);
`$RegSizeReplacementString` `$INSTANCE_NAME`_ReadPusle0(void);
void   `$INSTANCE_NAME`_WritePulse0(`$RegSizeReplacementString` pulseDesity0);
`$RegSizeReplacementString` `$INSTANCE_NAME`_ReadPusle1(void);
void   `$INSTANCE_NAME`_WritePulse1(`$RegSizeReplacementString` pulseDesity1);


/***************************************
*          API Constants
***************************************/

/* Constants for SetPulse0Mode(), SetPulse1Mode(), pulse type */
#define `$INSTANCE_NAME`_LESSTHAN_OR_EQUAL      0x00u
#define `$INSTANCE_NAME`_GREATERTHAN_OR_EQUAL   0x01u


/***************************************
*    Initial Parameter Constants
***************************************/

#define `$INSTANCE_NAME`_POLYNOM                `$PolyValue`
#define `$INSTANCE_NAME`_SEED                   `$SeedValue`
#define `$INSTANCE_NAME`_DENSITY0               `$Density0`
#define `$INSTANCE_NAME`_DENSITY1               `$Density1`


/***************************************
*              Registers
***************************************/

#if (`$INSTANCE_NAME`_RESOLUTION  <= 8) /* 8bit - PrISM */
    #define `$INSTANCE_NAME`_DENSITY0_REG       (* (reg8 *) `$INSTANCE_NAME`_sC8_PrISMdp_u0__D0_REG)
    #define `$INSTANCE_NAME`_DENSITY0_PTR       ((reg8 *) `$INSTANCE_NAME`_sC8_PrISMdp_u0__D0_REG)
    #define `$INSTANCE_NAME`_DENSITY1_REG       (* (reg8 *) `$INSTANCE_NAME`_sC8_PrISMdp_u0__D1_REG)
    #define `$INSTANCE_NAME`_DENSITY1_PTR       ((reg8 *) `$INSTANCE_NAME`_sC8_PrISMdp_u0__D1_REG)
    #define `$INSTANCE_NAME`_POLYNOM_REG        (* (reg8 *) `$INSTANCE_NAME`_sC8_PrISMdp_u0__A1_REG)
    #define `$INSTANCE_NAME`_POLYNOM_PTR        ((reg8 *) `$INSTANCE_NAME`_sC8_PrISMdp_u0__A1_REG)
    #define `$INSTANCE_NAME`_SEED_REG           (* (reg8 *) `$INSTANCE_NAME`_sC8_PrISMdp_u0__A0_REG)
    #define `$INSTANCE_NAME`_SEED_PTR           ((reg8 *) `$INSTANCE_NAME`_sC8_PrISMdp_u0__A0_REG)
    #define `$INSTANCE_NAME`_SEED_COPY_REG      (* (reg8 *) `$INSTANCE_NAME`_sC8_PrISMdp_u0__F0_REG)
    #define `$INSTANCE_NAME`_SEED_COPY_PTR      ((reg8 *) `$INSTANCE_NAME`_sC8_PrISMdp_u0__F0_REG)
    #define `$INSTANCE_NAME`_AUX_CONTROL_REG    (* (reg8 *) `$INSTANCE_NAME`_sC8_PrISMdp_u0__DP_AUX_CTL_REG)
    #define `$INSTANCE_NAME`_AUX_CONTROL_PTR    ((reg8 *) `$INSTANCE_NAME`_sC8_PrISMdp_u0__DP_AUX_CTL_REG)
#elif (`$INSTANCE_NAME`_RESOLUTION <= 16) /* 16bit - PrISM */
    #define `$INSTANCE_NAME`_DENSITY0_REG       (* (reg16 *) `$INSTANCE_NAME`_sC16_PrISMdp_u0__D0_REG)
    #define `$INSTANCE_NAME`_DENSITY0_PTR       ((reg16 *) `$INSTANCE_NAME`_sC16_PrISMdp_u0__D0_REG)
    #define `$INSTANCE_NAME`_DENSITY1_REG       (* (reg16 *) `$INSTANCE_NAME`_sC16_PrISMdp_u0__D1_REG)
    #define `$INSTANCE_NAME`_DENSITY1_PTR       ((reg16 *) `$INSTANCE_NAME`_sC16_PrISMdp_u0__D1_REG)
    #define `$INSTANCE_NAME`_POLYNOM_REG        (* (reg16 *) `$INSTANCE_NAME`_sC16_PrISMdp_u0__A1_REG)
    #define `$INSTANCE_NAME`_POLYNOM_PTR        ((reg16 *) `$INSTANCE_NAME`_sC16_PrISMdp_u0__A1_REG)
    #define `$INSTANCE_NAME`_SEED_REG           (* (reg16 *) `$INSTANCE_NAME`_sC16_PrISMdp_u0__A0_REG)
    #define `$INSTANCE_NAME`_SEED_PTR           ((reg16 *) `$INSTANCE_NAME`_sC16_PrISMdp_u0__A0_REG)
    #define `$INSTANCE_NAME`_SEED_COPY_REG      (* (reg16 *) `$INSTANCE_NAME`_sC16_PrISMdp_u0__F0_REG)
    #define `$INSTANCE_NAME`_SEED_COPY_PTR      ((reg16 *) `$INSTANCE_NAME`_sC16_PrISMdp_u0__F0_REG)
    #define `$INSTANCE_NAME`_AUX_CONTROL_REG    (* (reg16 *) `$INSTANCE_NAME`_sC16_PrISMdp_u0__DP_AUX_CTL_REG)
    #define `$INSTANCE_NAME`_AUX_CONTROL_PTR    ((reg16 *) `$INSTANCE_NAME`_sC16_PrISMdp_u0__DP_AUX_CTL_REG)
#elif (`$INSTANCE_NAME`_RESOLUTION <= 24) /* 24bit - PrISM */
    #define `$INSTANCE_NAME`_DENSITY0_REG       (* (reg32 *) `$INSTANCE_NAME`_sC24_PrISMdp_u0__D0_REG)
    #define `$INSTANCE_NAME`_DENSITY0_PTR       ((reg32 *) `$INSTANCE_NAME`_sC24_PrISMdp_u0__D0_REG)
    #define `$INSTANCE_NAME`_DENSITY1_REG       (* (reg32 *) `$INSTANCE_NAME`_sC24_PrISMdp_u0__D1_REG)
    #define `$INSTANCE_NAME`_DENSITY1_PTR       ((reg32 *) `$INSTANCE_NAME`_sC24_PrISMdp_u0__D1_REG)
    #define `$INSTANCE_NAME`_POLYNOM_REG        (* (reg32 *) `$INSTANCE_NAME`_sC24_PrISMdp_u0__A1_REG)
    #define `$INSTANCE_NAME`_POLYNOM_PTR        ((reg32 *) `$INSTANCE_NAME`_sC24_PrISMdp_u0__A1_REG)
    #define `$INSTANCE_NAME`_SEED_REG           (* (reg32 *) `$INSTANCE_NAME`_sC24_PrISMdp_u0__A0_REG)
    #define `$INSTANCE_NAME`_SEED_PTR           ((reg32 *) `$INSTANCE_NAME`_sC24_PrISMdp_u0__A0_REG)
    #define `$INSTANCE_NAME`_SEED_COPY_REG      (* (reg32 *) `$INSTANCE_NAME`_sC24_PrISMdp_u0__F0_REG)
    #define `$INSTANCE_NAME`_SEED_COPY_PTR      ((reg32 *) `$INSTANCE_NAME`_sC24_PrISMdp_u0__F0_REG)
    #define `$INSTANCE_NAME`_AUX_CONTROL_REG    (* (reg32 *) `$INSTANCE_NAME`_sC24_PrISMdp_u0__DP_AUX_CTL_REG)
    #define `$INSTANCE_NAME`_AUX_CONTROL_PTR    ((reg32 *) `$INSTANCE_NAME`_sC24_PrISMdp_u0__DP_AUX_CTL_REG)
    #define `$INSTANCE_NAME`_AUX_CONTROL2_REG   (* (reg32 *) `$INSTANCE_NAME`_sC24_PrISMdp_u2__DP_AUX_CTL_REG)
    #define `$INSTANCE_NAME`_AUX_CONTROL2_PTR   ((reg32 *) `$INSTANCE_NAME`_sC24_PrISMdp_u2__DP_AUX_CTL_REG)
#else /* 32bit - PrISM */
    #define `$INSTANCE_NAME`_DENSITY0_REG       (* (reg32 *) `$INSTANCE_NAME`_sC32_PrISMdp_u0__D0_REG)
    #define `$INSTANCE_NAME`_DENSITY0_PTR       ((reg32 *) `$INSTANCE_NAME`_sC32_PrISMdp_u0__D0_REG)
    #define `$INSTANCE_NAME`_DENSITY1_REG       (* (reg32 *) `$INSTANCE_NAME`_sC32_PrISMdp_u0__D1_REG)
    #define `$INSTANCE_NAME`_DENSITY1_PTR       ((reg32 *) `$INSTANCE_NAME`_sC32_PrISMdp_u0__D1_REG)
    #define `$INSTANCE_NAME`_POLYNOM_REG        (* (reg32 *) `$INSTANCE_NAME`_sC32_PrISMdp_u0__A1_REG)
    #define `$INSTANCE_NAME`_POLYNOM_PTR        ((reg32 *) `$INSTANCE_NAME`_sC32_PrISMdp_u0__A1_REG)
    #define `$INSTANCE_NAME`_SEED_REG           (* (reg32 *) `$INSTANCE_NAME`_sC32_PrISMdp_u0__A0_REG)
    #define `$INSTANCE_NAME`_SEED_PTR           ((reg32 *) `$INSTANCE_NAME`_sC32_PrISMdp_u0__A0_REG)
    #define `$INSTANCE_NAME`_SEED_COPY_REG      (* (reg32 *) `$INSTANCE_NAME`_sC32_PrISMdp_u0__F0_REG)
    #define `$INSTANCE_NAME`_SEED_COPY_PTR      ((reg32 *) `$INSTANCE_NAME`_sC32_PrISMdp_u0__F0_REG)
    #define `$INSTANCE_NAME`_AUX_CONTROL_REG    (* (reg32 *) `$INSTANCE_NAME`_sC32_PrISMdp_u0__DP_AUX_CTL_REG)
    #define `$INSTANCE_NAME`_AUX_CONTROL_PTR    ((reg32 *) `$INSTANCE_NAME`_sC32_PrISMdp_u0__DP_AUX_CTL_REG)
    #define `$INSTANCE_NAME`_AUX_CONTROL2_REG   (* (reg32 *) `$INSTANCE_NAME`_sC32_PrISMdp_u2__DP_AUX_CTL_REG)
    #define `$INSTANCE_NAME`_AUX_CONTROL2_PTR   ((reg32 *) `$INSTANCE_NAME`_sC32_PrISMdp_u2__DP_AUX_CTL_REG)
#endif /* End `$INSTANCE_NAME`_RESOLUTION */

#define `$INSTANCE_NAME`_CONTROL                (* (reg8 *) `$INSTANCE_NAME`_ctrl_CtrlReg__CONTROL_REG )
#define `$INSTANCE_NAME`_CONTROL_PTR            ( (reg8 *) `$INSTANCE_NAME`_ctrl_CtrlReg__CONTROL_REG )


/***************************************
*       Register Constants
***************************************/

#define `$INSTANCE_NAME`_CTRL_ENABLE                                0x01u
#define `$INSTANCE_NAME`_CTRL_COMPARE_TYPE0_GREATER_THAN_OR_EQUAL   0x02u
#define `$INSTANCE_NAME`_CTRL_COMPARE_TYPE1_GREATER_THAN_OR_EQUAL   0x04u

#define `$INSTANCE_NAME`_FIFO0_CLR                                  0x01u

#endif  /* End CY_PrISM_`$INSTANCE_NAME`_H */


/* [] END OF FILE */
